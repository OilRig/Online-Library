using AutoMapper;
using Microsoft.AspNet.Identity;
using OnlineLibrary.BLL.DTO;
using OnlineLibrary.BLL.Infrastructure;
using OnlineLibrary.BLL.Interfaces;
using OnlineLibrary.DAL.Entities;
using OnlineLibrary.DAL.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineLibrary.BLL.Services
{
    public class UserService : IUserService
    {
        IUnitOfWork Database { get; set; }
        public UserService(IUnitOfWork uow)
        {
            Database = uow;
        }
        public async Task<OperationDetails> Create(UserDTO userDto)
        {
            LibraryUser user = await Database.UserManager.FindByEmailAsync(userDto.Email);
            if (user == null)
            {
                user = new LibraryUser { Email = userDto.Email, UserName = userDto.Email };
                var result = Database.UserManager.Create(user, userDto.Password);
                if (result.Errors.Count() > 0)
                    return new OperationDetails(false, result.Errors.FirstOrDefault(), "");
                await Database.UserManager.AddToRoleAsync(user.Id, userDto.Role);
                ClientProfile clientProfile = new ClientProfile { Id = user.Id, Adress = userDto.Address, Name = userDto.Name};
                Database.ClientManager.Create(clientProfile);
                await Database.SaveAsync();
                return new OperationDetails(true, "Регистрация успешно пройдена", "");
            }
            else
            {
                return new OperationDetails(false, "Пользователь с таким логином уже существует", "Email");
            }
        }
        public async Task<ClaimsIdentity> Authenticate(UserDTO userDto)
        {
            ClaimsIdentity claim = null;
            LibraryUser user = await Database.UserManager.FindAsync(userDto.Email, userDto.Password);
            if (user != null)
                claim = await Database.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            return claim;
        }
        public UserDTO FindById(string id)
        {
           Mapper.Initialize(cfg => cfg.CreateMap<LibraryUser, UserDTO>());
           var UserDto = Mapper.Map<LibraryUser, UserDTO>(Database.UserManager.FindById(id));
            Database.RoleManager.FindById(id);
           return UserDto;
        }
        public void ResetPass(string id, string newPass)
        {
            LibraryUser user = Database.UserManager.FindById(id);
            user.PasswordHash = Database.UserManager.PasswordHasher.HashPassword(newPass);
            Database.SaveAsync();
        }
        public string FindId(UserDTO userDto)
        {
            return Database.UserManager.Find(userDto.Email, userDto.Password).Id;
        }
        public async Task SetInitialData(UserDTO adminDto, List<string> roles)
        {
            foreach (string roleName in roles)
            {
                var role = await Database.RoleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new LibraryRole { Name = roleName };
                    await Database.RoleManager.CreateAsync(role);
                }
            }
            await Create(adminDto);
        }
        public IEnumerable<UserDTO> AllUsers()
        {
            List<LibraryUser> users = Database.UserManager.Users.ToList();
            Mapper.Initialize(cfg => cfg.CreateMap<LibraryUser, UserDTO>());
            IEnumerable<UserDTO> UserDto = Mapper.Map<List<LibraryUser>, IEnumerable<UserDTO>>(users);
            foreach (UserDTO user in UserDto)
            {
                user.Address = Database.ClientManager.FindById(user.Id).Adress;
                user.Name = Database.ClientManager.FindById(user.Id).Name;
                foreach (string role in Database.UserManager.GetRoles(user.Id))
                    user.Role += role + " ";
            }
            return UserDto;
        }
        public ClientProfile FindProfile(string id)
        {
            return Database.ClientManager.FindById(id);
        }
        public async Task<bool> DeleteUser(string id)
        {
            ClientProfile profile = Database.ClientManager.FindById(id);
            if (profile != null)
                Database.ClientManager.DeleteProfile(profile);
            LibraryUser user = await Database.UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await Database.UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        public async Task<UserDTO> FindByEmail(string email)
        {
            LibraryUser user = await Database.UserManager.FindByEmailAsync(email);
            Mapper.Initialize(cfg => cfg.CreateMap<LibraryUser, UserDTO>());
            UserDTO UserDto = Mapper.Map<LibraryUser, UserDTO>(user);
            return UserDto;
        }
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}
