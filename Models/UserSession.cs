using System;

namespace SpacefinderOff.Models
{
    public static class UserSession
    {
        public static User CurrentUser { get; private set; }
        public static bool IsLoggedIn => CurrentUser != null;
        public static bool IsAdmin => CurrentUser?.Status?.ToLower() == "admin";
        public static bool IsUser => CurrentUser?.Status?.ToLower() == "user";

        public static void Login(User user)
        {
            CurrentUser = user;

            // TODO: Update last login in database
            // UpdateLastLoginInDatabase(user.UserID);
        }

        public static void Logout()
        {
            CurrentUser = null;
        }

        public static bool HasPermission(string permission)
        {
            if (!IsLoggedIn) return false;

            switch (permission.ToLower())
            {
                case "admin":
                    return IsAdmin;
                case "user":
                    return IsUser || IsAdmin; // Admins can also do user actions
                case "create_user":
                case "edit_user":
                case "delete_user":
                case "reset_password":
                case "view_admin_dashboard":
                    return IsAdmin;
                default:
                    return false;
            }
        }

        public static void RequireAdmin()
        {
            if (!IsAdmin)
            {
                throw new UnauthorizedAccessException("Admin privileges required for this operation.");
            }
        }

        public static void RequireLogin()
        {
            if (!IsLoggedIn)
            {
                throw new UnauthorizedAccessException("You must be logged in to perform this action.");
            }
        }
    }
}