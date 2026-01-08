namespace Aegis.Models
{
    public class AppSettings
    {
        public int AutoLockMinutes { get; set; } = 10;
        public bool ClearClipboard { get; set; } = true;
        public string Theme { get; set; } = "Dark";
    }
}
