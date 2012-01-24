using System.Web;

namespace Suteki.TardisBank.Services
{
    public class HttpContextService : IHttpContextService
    {
        public string UserName
        {
            get { return CurrentHttpContext.User.Identity.Name; }
        }

        public bool UserIsAuthenticated
        {
            get { return CurrentHttpContext.User.Identity.IsAuthenticated; }
        }

        static HttpContext CurrentHttpContext
        {
            get
            {
                var context = HttpContext.Current;
                if (context == null)
                {
                    throw new TardisBankException("HttpContext.Current is null");
                }
                return context;
            }
        }
    }
}