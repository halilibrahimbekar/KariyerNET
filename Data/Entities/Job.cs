using Data.Common.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Service.Entities
{
    [Table("job")]
    public class Job : BaseEntity
    {
        /// <summary> Pozisyon Bilgisi </summary>
        [Column("position")]
        public string Position { get; set; }

        /// <summary> Açıklama </summary>
        [Column("description")]
        public string Description { get; set; }

        /// <summary> Yan Haklar </summary>
        [Column("additional")]
        public string Additional { get; set; }

        /// <summary> Ücret Bİlgisi</summary>
        [Column("salary")]
        public decimal? Salary { get; set; }

        /// <summary> Çalışma Şekli</summary>
        [Column("jobtype")]
        public int? JobType { get; set; }

        /// <summary> İlan Bitiş Tarihi</summary>
        [Column("enddate")]
        public DateTime EndDate { get; set; }

        /// <summary> İlan Kalitesi</summary>
        [Column("score")]
        public int Score { get; set; }

        [Column("companyid")]
        public Guid CompanyId { get; set; }

        /// <summary> İşveren / Şirket </summary>
        public virtual Company Company { get; set; }
    }
}
