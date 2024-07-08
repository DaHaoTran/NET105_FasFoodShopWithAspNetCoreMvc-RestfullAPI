namespace UI.Models
{
    public static class LogStateC
    {
        public static bool IsLoggedIn { get; set; } = false;
        public static string Email { get; set; } = string.Empty;
        public static string Username {  get; set; } = string.Empty;
        public static int CartId { get; set; } = 0;
        public static bool NeedCreate { get; set; } = false;
    }
}
