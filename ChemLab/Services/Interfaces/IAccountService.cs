using System;
using Microsoft.AspNetCore.Authentication;

namespace ChemLab.Services.Interfaces
{
    public interface IAccountService
    {
        Task<string[]> HandleGoogleCallback();

        Task Logout();

        AuthenticationProperties GetLoginProperties(string url);
    }
}

