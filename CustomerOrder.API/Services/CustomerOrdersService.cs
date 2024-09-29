using AutoMapper;
using CustomerOrder.API.Contracts.CustomerOrders;
using CustomerOrder.API.Contracts.CustomerOrders.Dtos;
using CustomerOrder.API.Services.RabbitMq;
using CustomerOrder.Data.Context;
using CustomerOrder.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CustomerOrder.API.Services
{
    public class CustomerOrdersService : ICustomerOrdersService
    {
        private readonly DataContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerOrdersService> _logger;
        private readonly RabbitMqService _rabbitMqService;
        public CustomerOrdersService(DataContext context, IMapper mapper, ILogger<CustomerOrdersService> logger, RabbitMqService rabbitMqService)
        {
            _dbContext = context;
            _mapper = mapper;
            _logger = logger;
            _rabbitMqService = rabbitMqService;
        }
        public async Task<CustomerOrderDto> AddCustomerOrderAsync(CustomerOrderDto orderDto)
        {
            try
            {
                // Müşteriyi kontrol et
                var existingCustomer = await _dbContext.Customers
                    .FirstOrDefaultAsync(c => c.Email == orderDto.Customer.Email);

                var cusId = 0;
                if (existingCustomer != null)
                {
                    cusId = existingCustomer.Id;
                }
                else
                {
                    // Yeni müşteri oluştur
                    var customer = _mapper.Map<Customer>(orderDto.Customer);
                    customer.CreatedDate = DateTime.Now;
                    customer.CreatedBy = 1;
                    customer.IsDeleted = false;

                    _dbContext.Customers.Add(customer);
                    await _dbContext.SaveChangesAsync();
                    cusId = customer.Id;
                }

                // Sipariş oluşturma işlemi
                var customerOrder = _mapper.Map<CustomerOrder.Data.Models.CustomerOrder>(orderDto);
                customerOrder.CreatedDate = DateTime.Now;
                customerOrder.CreatedBy = 1;
                customerOrder.IsDeleted = false;
                customerOrder.CustomerId = cusId;
                customerOrder.Products = new List<Product>();

                // Ürünleri kontrol et ve ekle
                foreach (var item in orderDto.Products)
                {
                    var existingProduct = await _dbContext.Products
                        .FirstOrDefaultAsync(p => p.Barcode == item.Barcode);

                    if (existingProduct != null)
                    {
                        customerOrder.Products.Add(existingProduct);
                    }
                    else
                    {
                        var product = _mapper.Map<Product>(item);
                        product.CreatedDate = DateTime.Now;
                        product.CreatedBy = 1;
                        product.IsDeleted = false;

                        customerOrder.Products.Add(product);
                    }
                }

                // Siparişi ekle
                customerOrder.Customer = null;
                var products = new List<Product>();
                products = customerOrder.Products;
                customerOrder.Products = null;
                _dbContext.CustomerOrders.Add(customerOrder);
                await _dbContext.SaveChangesAsync();

                //ürünleri ekle
                foreach (var item in products)
                {
                    item.CustomerOrderId = customerOrder.Id;
                }
                _dbContext.Products.AddRange(products);
                await _dbContext.SaveChangesAsync();

                // Mesaj gönder
                _rabbitMqService.SendMessage($"Add new order");

                customerOrder.Products = products;

                return _mapper.Map<CustomerOrderDto>(customerOrder);

            }
            catch (Exception ex)
            {
                _logger.LogInformation("error: " + ex.Message);
                throw new Exception($"An error occurred while adding the order: {ex.Message}");
            }
        }
        public async Task<CustomerOrderDto> GetByIdAsync(int id)
        {
            try
            {
                var customerOrder = await _dbContext.CustomerOrders
                .Include(o => o.Customer)
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => !o.IsDeleted && o.Id == id);

                if (customerOrder == null)
                {
                    throw new Exception($"Order is not available");
                }

                //Mesaj gönder
                _rabbitMqService.SendMessage($"Get order : {id}");

                return _mapper.Map<CustomerOrderDto>(customerOrder);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("error: " + ex.Message);
                throw new Exception($"An error occurred while getting the order: {ex.Message}");
            }
        }
        public async Task<CustomerOrderDto> UpdateAsync(int Id, CustomerOrderDto orderDto)
        {
            try
            {
                var existingOrder = await _dbContext.CustomerOrders
                    .Include(o => o.Customer)
                    .Include(o => o.Products)
                    .FirstOrDefaultAsync(o => o.Id == Id);

                if (existingOrder == null)
                {
                    throw new Exception("Order not found");
                }

                // Müşteri bilgilerini güncelle
                if (existingOrder.Customer != null)
                {
                    existingOrder.Customer.Name = orderDto.Customer.Name;
                    existingOrder.Customer.Address = orderDto.Customer.Address;
                    existingOrder.Customer.Email = orderDto.Customer.Email;
                }

                // Ürünleri güncelle
                existingOrder.Products.Clear();
                foreach (var productDto in orderDto.Products)
                {
                    var product = new Product
                    {
                        Barcode = productDto.Barcode,
                        Description = productDto.Description,
                        Quantity = productDto.Quantity,
                        Price = productDto.Price,
                        CreatedDate = DateTime.Now,
                        CreatedBy = 1,
                        IsDeleted = false
                    };
                    existingOrder.Products.Add(product);
                }

                await _dbContext.SaveChangesAsync();

                // Mesaj gönder
                _rabbitMqService.SendMessage($"update order : {Id}");

                return _mapper.Map<CustomerOrderDto>(existingOrder);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("error: " + ex.Message);
                throw new Exception($"An error occurred while updating the order: {ex.Message}");
            }

        }
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var customerOrder = await _dbContext.CustomerOrders.FindAsync(id);
                if (customerOrder == null)
                {
                    throw new KeyNotFoundException("Order not found.");
                }

                _dbContext.CustomerOrders.Remove(customerOrder);
                await _dbContext.SaveChangesAsync();

                //delete customer
                var customer = await _dbContext.Customers.FindAsync(customerOrder.CustomerId);
                if (customer != null)
                {
                    _dbContext.Customers.Remove(customer);
                    await _dbContext.SaveChangesAsync();
                }

                //delete products
                var products = await _dbContext.Products.Where(x => x.CustomerOrderId == customerOrder.CustomerId).ToListAsync();
                if (products != null)
                {
                    _dbContext.Products.RemoveRange(products);
                    await _dbContext.SaveChangesAsync();
                }

                //customerOrder.IsDeleted = true;
                //_dbContext.CustomerOrders.Update(customerOrder);
                //await _dbContext.SaveChangesAsync();

                //Mesaj gönder
                _rabbitMqService.SendMessage($"delete order : {id}");

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("error: " + ex.Message);
                throw new Exception($"An error occurred while deleting the order: {ex.Message}");
            }
        }
    }
}
