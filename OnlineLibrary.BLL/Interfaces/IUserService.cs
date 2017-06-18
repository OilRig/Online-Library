using OnlineLibrary.BLL.DTO;
using OnlineLibrary.BLL.Infrastructure;
using OnlineLibrary.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineLibrary.BLL.Interfaces
{
    public interface IUserService : IDisposable
    {
        Task<OperationDetails> Create(UserDTO userDto);
        Task<ClaimsIdentity> Authenticate(UserDTO userDto);
        Task SetInitialData(UserDTO adminDto, List<string> roles);
        string FindId(UserDTO userDto);
        UserDTO FindById(string id);
        void ResetPass(string id, string newPass);
        IEnumerable<UserDTO> AllUsers();
        ClientProfile FindProfile(string id);
        Task<bool> DeleteUser(string email);
        Task<UserDTO> FindByEmail(string email);
        Task<string> GenerateTokenAsync(string id);
        Task SendEmail(string userId, string subject, string body);
    }
}
