namespace CustomerOrder.API.Services.Authority
{
    public interface IAuthorityService
    {
        string GenerateJwtToken(string userId);
    }
}
