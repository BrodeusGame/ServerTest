using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace ServerTest;

public class Server
{
    public static TcpListener tcpListener = new TcpListener(IPAddress.Any, 26950);
    public static Dictionary<int, Client> players = new Dictionary<int, Client>();
    public static int maxPlayer = 20;
    public static int port = 26950;
    public static int dataBufferSize = 4096;


    public static void SetupServer()
    {
        tcpListener = new TcpListener(IPAddress.Any, port);
        PlayerSetup();
        Console.WriteLine($"Server hazırlandı .Max player : {maxPlayer}");
    }

    public static void StartServer()
    {
        tcpListener.Start(); // dinlemeyi başlatıyor.
        tcpListener.BeginAcceptTcpClient(AcceptCallBack, null); // içeriye alıyoruz
        Console.WriteLine($"Server dinliyor...");
    }

    public static void AcceptCallBack(IAsyncResult asyncResult)
    {
        TcpClient socket = tcpListener.EndAcceptTcpClient(asyncResult);
        tcpListener.BeginAcceptTcpClient(AcceptCallBack, null); // içeriye alıyoruz

        for (int i = 1; i < maxPlayer; i++)
        {
            if (players[i].tcp.socket==null)
            {
                players[i].tcp.Connect(socket);
                players[i].tcp.SendDatafromJson(JsonConvert.SerializeObject(Handlers.Create_Hello(players[i].tcp.id,(int)Handlers.Server.HelloServer,"Bağlantı başarılı!....")));
                return;
            }
            
        }
        try
        {
            socket.Close();
            return;
        }
        catch (Exception e)
        {
            return;
        }
     
    }

    public static void PlayerSetup()
    {
        for (int i = 1; i < maxPlayer; i++)
        {
            players.Add(i, new Client(i));
        }
    }
}