namespace CRM.OneMedical.Client.Seguridad
{
    public interface ILoginService
    {
        Task Login(string token);

        Task Logout();
    }
}
