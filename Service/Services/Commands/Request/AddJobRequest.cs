using Data.Common.Enum;
using MediatR;
using Service.Services.Commands.Response;
using System;

namespace Service.Services.Commands.Request
{
    public class AddJobRequest : IRequest<AddJobResponse>
    {
        /// <summary> Pozisyon Bilgisi </summary>
        public string Position { get; set; }

        /// <summary> Açıklama </summary>
        public string Description { get; set; }

        /// <summary> Yan Haklar </summary>
        public string Additional { get; set; }

        /// <summary> Ücret Bİlgisi</summary>
        public decimal? Salary { get; set; }

        /// <summary> Çalışma Şekli</summary>
        public JobType? JobType { get; set; }

        public Guid CompanyId { get; set; }
    }
}
