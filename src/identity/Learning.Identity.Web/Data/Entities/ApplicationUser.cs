﻿using Microsoft.AspNetCore.Identity;

namespace Learning.Identity.Web.Data.Entities;

public class ApplicationUser : IdentityUser
{
    public bool IsAdmin { get; set; }
}
