using System.Text.Json;

namespace Client
{
    public class Message
    {
        public string Text { get; set; }
        public DateTime DateTime { get; set; }
        public string SenderName { get; set; }
        public string RecieverName { get; set; }

        public string SerializeMessageToJson()
        {
            return JsonSerializer.Serialize(this);
        }
        public static Message DeserializeMessageFromJson(string json)
        {
            return JsonSerializer.Deserialize<Message>(json);
        }
        public override string ToString()
        {
            return $"DateTime: {DateTime}, SenderName: {SenderName}, RecieverName: {RecieverName}, Text: {Text}";
        }
    }
}