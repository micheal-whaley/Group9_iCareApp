﻿using Group9_iCareApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;

// anything that is commented out is that way to avoid errors for now until the models are fully implemented

// Created by UserAuthenticationForm or login
namespace Group9_iCareApp.Controllers
{
    public class UserAuthenticationController : Controller
    {
        //public ActionResult Validate(string username, string password)
        //{
        //    // Retrieve the stored hashed password
        //    var storedHashedPassword = GetStoredHashedPassword(username);

        //    // Hash the provided password
        //    var hashedPassword = HashPassword(password);

        //    // Compare the hashed passwords
        //    if (hashedPassword == storedHashedPassword)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }

        //    ModelState.AddModelError("", "Invalid username or password");
        //    return RedirectToAction("Login", "Home");
        //}

        //private string GetStoredHashedPassword(string username)
        //{
        //    using (var context = new iCAREEntities())
        //    {
        //        var userPassword = context.UserPasswords.FirstOrDefault(up => up.Username == username);
        //        return userPassword?.Password ?? string.Empty; // return empty string if null.
        //    }
        //}

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = sha256.ComputeHash(passwordBytes);

                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}