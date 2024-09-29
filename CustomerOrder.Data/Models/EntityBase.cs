using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOrder.Data.Models
{
    public class EntityBase
    {
        [Column("Id"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Required]
        public int Id { get; set; }
        /// <summary>
        /// Kayıt işlemi yapan kullanıcının USERS tablosu Id bilgisi
        /// </summary>
        [Column("CreatedBy")]
        public int CreatedBy { get; set; }
        /// <summary>
        /// Kayıt tarihi. SQL üzerinden GETDATE() ile otomatik verilecek.
        /// </summary>
        [Column("CreatedDate")]
        public DateTime CreatedDate { get; set; }
        /// <summary>
        /// Düzenleme işlemi yapan kullanıcının USERS tablosu Id bilgisi. İlk kayıt anında NULL olacak.
        /// </summary>
        [Column("UpdatedBy")]
        public int? UpdatedBy { get; set; }
        /// <summary>
        /// Güncelleme tarihi. İlk kayıt anında NULL olacak.
        /// </summary>
        [Column("UpdatedDate")]
        public DateTime? UpdatedDate { get; set; }
        /// <summary>
        /// True ise &quot;Silinmemiş aktif kayıt&quot;, False ise &quot;Silinmiş pasif kayıt&quot;
        /// </summary>
        [Column("IsDeleted")]
        public bool IsDeleted { get; set; }
    }
}
