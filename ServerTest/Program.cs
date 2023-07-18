using ServerTest;

static void Main(string[] args)
{
	Server.SetupServer();
	Server.StartServer();
	Console.ReadKey();
}