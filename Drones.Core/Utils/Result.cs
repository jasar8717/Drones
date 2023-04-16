namespace Drones.Core.Utils
{
    public class Result
    {
        public string message { get; set; }
        public string type { get; set; }
        public Result(String Message, String Type = null)
        {
            this.message = Message;
            this.type = Type;
        }
    }
}
