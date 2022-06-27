namespace Social.Network.SeedWorks.Models
{
    public class JwToken
    {
        public string TokenType { get; set; }
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpiresIn { get; set; }
    }
}
