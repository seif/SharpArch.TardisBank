namespace Suteki.TardisBank.Services
{
    public interface IHttpContextService
    {
        string UserName { get; }
        bool UserIsAuthenticated { get; }
    }
}