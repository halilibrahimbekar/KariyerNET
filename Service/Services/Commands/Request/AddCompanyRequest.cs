using MediatR;
using Service.Services.Commands.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Commands.Request
{
    public class AddCompanyRequest : IRequest<AddCompanyResponse>
    {
        /// <summary> Telefon Numarası </summary>
        public string PhoneNumber { get; set; }

        /// <summary> Şirket / İşveren Adı</summary>
        public string CompanyName { get; set; }

        /// <summary> Adres Bilgisi</summary>
        public string Address { get; set; }
    }
}
