using FinalProject.ServiceModels;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Interfaces
{
    public interface IUserManager
    {
        Task<(bool result, string errorMessage)> Login(LoginModel request);
    }
}
