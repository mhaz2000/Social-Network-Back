using Newtonsoft.Json;

namespace Social.Network.SeedWorks
{
    public class ResponseMessage
    {
        public ResponseMessage()
        {

        }

        public ResponseMessage(string message)
        {
            Message = message;
        }

        public ResponseMessage(string message, object content)
        {
            Message = message;
            Content = content;
            Total = 0;
        }

        public ResponseMessage(string message, object content, int total)
        {
            Message = message;
            Content = content;
            Total = total;
        }

        public string Message { get; set; }

        public object Content { get; set; }
        public int Total { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
