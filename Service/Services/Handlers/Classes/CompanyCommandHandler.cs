using Common.Common.Enum;
using Common.Common.Helper;
using KariyerNET.Data;
using MediatR;
using Service.Entities;
using Service.Services.Commands.Request;
using Service.Services.Commands.Response;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Service.Services.Handlers
{
    public class AddCompanyCommandHandler : IRequestHandler<AddCompanyRequest, AddCompanyResponse>
    {
        private readonly KariyerNETContext _context;

        public AddCompanyCommandHandler(KariyerNETContext context)
        {
            _context = context;
        }
        public async Task<AddCompanyResponse> Handle(AddCompanyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                if (_context.Companies.Any(x => x.PhoneNumber == request.PhoneNumber))
                {
                    return new AddCompanyResponse { Id = Guid.Empty, Status = Data.Common.Enum.ResultStatusEnum.UnSuccess, Message = "Bu Telefon Numarası Daha Önce Kullanılmış!" };
                }

                var id = Guid.NewGuid();
                var company = new Company
                {
                    Id = id,
                    Address = request.Address,
                    CompanyName = request.CompanyName,
                    CreatedDateTime = DateTime.Now,
                    PhoneNumber = request.PhoneNumber,
                    RemainingAdvertCount = Convert.ToInt32(Settings.GetValue(SettingsEnum.DefaultAddvertCount.ToString())) // TODO avoid hardcoded variables..
                };

                _context.Companies.Add(company);
                _context.SaveChanges();

                return new AddCompanyResponse { Id = id, Status = Data.Common.Enum.ResultStatusEnum.Success, Message = "Başarılı!" };
            }
            catch (Exception)
            {
                // error handling...

                return new AddCompanyResponse { Status = Data.Common.Enum.ResultStatusEnum.Error };
            }
        }
    }
}
