namespace Social.Network.SeedWorks.Models
{
    public class AppSettingsModel
    {
        public string HostAddress { get; set; }
        public bool HostRunAsConsole { get; set; }
        public string AllowedHosts { get; set; }
        public JwtIssuerOptions JwtIssuerOptions { get; set; }
    }
}
