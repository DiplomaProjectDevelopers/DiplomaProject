using DiplomaProject.Domain.Entities;
using DiplomaProject.Domain.Interfaces;
using DiplomaProject.WebUI.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DiplomaProject.Domain.Extentions
{
    public static class Extentions
    {
        public static async Task<string> GetRoleAsync(this UserManager<User> userManager, User user)
        {
            if (user == null)
                throw new ArgumentNullException();
            var roles = await userManager.GetRolesAsync(user);
            if (roles.Count <= 0)
                return null;
            var role = roles[0];
            return role;
        }

        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Հաստատեք էլ-փոստը",
                $"Խնդրում ենք հաստատեք ձեր էլեկտրոնային փոստի հասցեն սեղմելով հղման վրա՝ <a href='{HtmlEncoder.Default.Encode(link)}'>այստեղ</a>");
        }

        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AdminController.ConfirmEmail),
                controller: "Admin",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AdminController.ResetPassword),
                controller: "Admin",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}
