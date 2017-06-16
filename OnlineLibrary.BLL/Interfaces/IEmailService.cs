using System.Threading.Tasks;

namespace OnlineLibrary.BLL.Interfaces
{
    public interface IEmailService
    {
        Task<bool> Send(string email, string subject, string body);
    }
}
