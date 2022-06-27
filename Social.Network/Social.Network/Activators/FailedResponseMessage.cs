using Newtonsoft.Json;

namespace Social.Network.Activators
{
    public class FailedResponseMessage
    {
        public FailedResponseMessage(string message)
        {
            this.message = message;
        }
        public string message { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
