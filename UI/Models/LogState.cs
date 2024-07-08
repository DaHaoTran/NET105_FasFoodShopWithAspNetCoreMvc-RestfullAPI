namespace UI.Models
{
    public static class LogState
    {
        public static bool IsLoged { get; set; } = false; 
        public static string Email { get; set; } = string.Empty;

        public static bool? Level {  get; set; }
    }
}
