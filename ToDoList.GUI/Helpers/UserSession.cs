using TodoListApp.DAL.Models;

namespace ToDoList.GUI.Helpers
{
    /// <summary>
    /// Qu?n lý phiên ??ng nh?p c?a user
    /// </summary>
    public static class UserSession
    {
        /// <summary>
        /// User hi?n t?i ?ang ??ng nh?p
        /// </summary>
        public static User? CurrentUser { get; private set; }

        /// <summary>
        /// Ki?m tra xem user ?ã ??ng nh?p ch?a
        /// </summary>
        public static bool IsLoggedIn => CurrentUser != null;

        /// <summary>
        /// ??ng nh?p
        /// </summary>
        public static void Login(User user)
        {
            CurrentUser = user;
        }

        /// <summary>
        /// ??ng xu?t
        /// </summary>
        public static void Logout()
        {
            CurrentUser = null;
        }

        /// <summary>
        /// L?y tên hi?n th? c?a user
        /// </summary>
        public static string GetDisplayName()
        {
            if (CurrentUser == null)
                return "User";

            return CurrentUser.FullName ?? CurrentUser.Email ?? "User";
        }

        /// <summary>
        /// L?y UserId c?a user hi?n t?i
        /// </summary>
        public static int GetUserId()
        {
            return CurrentUser?.UserId ?? 0;
        }

        /// <summary>
        /// ??ng xu?t và xóa thông tin ng??i dùng
        /// </summary>
        public static void Clear()
        {
            CurrentUser = null;
        }
    }
}
