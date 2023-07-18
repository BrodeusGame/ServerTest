using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace ServerTest;

public class Handlers
{
    public enum Server
    {
        HelloServer = 1
    }

    public enum Client
    {
        HelloClient = 1
    }

    public static void Handle(string _json)
    {
        Packet mainPacket = JsonConvert.DeserializeObject<Packet>(_json);
        switch (mainPacket.type)
        {
            case (int)Client.HelloClient:
                Get_Hello(JsonConvert.DeserializeObject<Hello>(_json));
                break;
            default:
                break;
        }
    }

    public static void Get_Hello(Hello packet)
    {
        Console.WriteLine($"Oyuncunun ismi : {packet.name}");
        
    }
    

    public class Packet
    {
        public int id;
        public int type;
    }

    public class Hello : Packet
    {
        public string message;
        public string name;
    }

    public static Hello Create_Hello(int _id, int _type, string _message)
    {
        Hello packet = new Hello();
        packet.id = _id;
        packet.type = _type;
        packet.message = _message;
        return packet;
    }
}