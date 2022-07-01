using Data.Common.Enum;
using System;
using Nest;

namespace Service.Services.Search.SearchModel
{
    public class JobIndex
    {
        /// <summary> Pozisyon Bilgisi </summary>
        public string Position { get; set; }

        /// <summary> Açıklama </summary>
        public string Description { get; set; }

        /// <summary> Ücret Bİlgisi</summary>
        public decimal? Salary { get; set; }

        /// <summary> Çalışma Şekli</summary>
        public JobType? JobType { get; set; }

        /// <summary> İlan Bitiş Tarihi</summary>
        public DateTime EndDate { get; set; }

        /// <summary> İlan Kalitesi</summary>
        public int Score { get; set; }

        /// <summary> Şirket / İşveren Id</summary>
        [Keyword]
        public Guid CompanyId { get; set; }


    }
}
