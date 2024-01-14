using System;
using Microsoft.AspNetCore.Identity;

namespace ChemLab.Data.Entity
{
    public class ApplicationUser : IdentityUser
{
    public string PictureUrl { get; set; }

    public ApplicationUser()
    {
        PictureUrl = "default-url.jpg";
    }
}

}

