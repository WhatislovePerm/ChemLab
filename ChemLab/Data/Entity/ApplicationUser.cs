using System;
using Microsoft.AspNetCore.Identity;

namespace ChemLab.Data.Entity
{
    public class ApplicationUser : IdentityUser
    {
        public string PictureUrl { get; internal set; }
    }
}

