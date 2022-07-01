using System.ComponentModel.DataAnnotations.Schema;

namespace Service.Entities
{
    [Table("company")]
    public class Company : BaseEntity
    {
        /// <summary> Telefon Numarası </summary>
        [Column("phonenumber")]
        public string PhoneNumber { get; set; }

        /// <summary> Şirket / İşveren Adı</summary>
        [Column("companyname")]
        public string CompanyName { get; set; }

        /// <summary> Adres Bilgisi</summary>
        [Column("address")]
        public string Address { get; set; }

        /// <summary> Kalan İlan Hakkı</summary>
        [Column("remainingadvertcount")]
        public int RemainingAdvertCount { get; set; }
    }
}
