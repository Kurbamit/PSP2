using System;
using System.Security.Claims;
using ReactApp1.Server.Models.Enums;

namespace ReactApp1.Server.Extensions
{
    public static class PrincipalExtensions
    {
        public static int? GetUserId(this System.Security.Principal.IPrincipal principal)
        {
            if (!principal.Identity.IsAuthenticated)
            {
                return null;
            }
            string userId = (principal.Identity as ClaimsIdentity).FindFirst(f => f.Type == "UserId")?.Value;
            if (!string.IsNullOrEmpty(userId) && int.TryParse(userId, out int id))
            {
                return id;
            }

            return null;
        }

        public static int? GetUserEstablishmentId(this System.Security.Principal.IPrincipal principal)
        {
            if (!principal.Identity.IsAuthenticated)
            {
                return null;
            }
            string establishmentId = (principal.Identity as ClaimsIdentity).FindFirst(f => f.Type == "EstablishmentId")?.Value;
            
            if (!string.IsNullOrEmpty(establishmentId) && int.TryParse(establishmentId, out int id))
            {
                return id;
            }
            
            return null;
        }
        
        public static string? GetUserEmail(this System.Security.Principal.IPrincipal principal)
        {
            if (!principal.Identity.IsAuthenticated)
            {
                return null;
            }
            string userEmail = (principal.Identity as ClaimsIdentity).FindFirst(f => f.Type == ClaimTypes.Name)?.Value;
            
            if (!string.IsNullOrEmpty(userEmail))
            {
                return userEmail;
            }
            
            return null;
        }

        public static TitleEnum? GetUserTitle(this System.Security.Principal.IPrincipal principal)
        {
            if (!principal.Identity.IsAuthenticated)
            {
                return null;
            }
            string title = (principal.Identity as ClaimsIdentity).FindFirst(f => f.Type == "Title")?.Value;
            
            if (!string.IsNullOrEmpty(title) && Enum.TryParse<TitleEnum>(title, out TitleEnum result))
            {
                return result;
            }

            return null;
        }
    }
}

