using KariyerNET.Data;

namespace Service.Services.Handlers.Classes
{
    public class BaseHandler
    {
        protected readonly KariyerNETContext _context;

        public BaseHandler(KariyerNETContext context)
        {
            _context = context;
        }
    }
}
