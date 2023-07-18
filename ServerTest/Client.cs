using System.Net.Sockets;
using System.Text;

namespace ServerTest;

public class Client
{
    public int id;
    public Tcp tcp;

    public Client(int _id)
    {
        id = _id;
        tcp = new Tcp(_id);
    }

    public class Tcp
    {
        public int id;
        public string Name;
        public TcpClient socket;
        public NetworkStream stream;
        public byte[] buffer;

        public Tcp(int _id)
        {
            id = _id;
        }

        public void Disconnect()
        {
            if (socket != null)
                if (socket.Connected)
                    socket.Close();

            if (stream != null)
                stream.Close();

            stream = null;
            socket = null;
            Name = null;
            buffer = null;
        }

        public void Connect(TcpClient _socket)
        {
            socket = _socket;
            socket.ReceiveBufferSize = Server.dataBufferSize;
            socket.SendBufferSize = Server.dataBufferSize;
            stream = socket.GetStream();
            buffer = new byte[Server.dataBufferSize];

            stream.BeginRead(buffer, 0, Server.dataBufferSize, ReceiveCallBack, null);
        }

        private void ReceiveCallBack(IAsyncResult ar)
        {
            try
            {
                int receiveDataLenght = stream.EndRead(ar);
                if (receiveDataLenght<=0)
                {
                    Disconnect();
                    return;
                }

                byte[] _data = new byte[receiveDataLenght];
                Array.Copy(buffer,_data,receiveDataLenght);

                string strJson = Encoding.UTF8.GetString(_data);
                Handlers.Handle(strJson);

                stream.BeginRead(buffer, 0, Server.dataBufferSize, ReceiveCallBack, null);


            }
            catch (Exception e)
            {
                Disconnect();
                return;
            }
        }

        public void SendDatafromJson(string _json)
        {
            byte[] _data = Encoding.UTF8.GetBytes(_json);
            try
            {
                stream.BeginWrite(_data, 0, _data.Length, SendCallBack, null);

            }
            catch (Exception e)
            {
                Disconnect();
                
                
            }
        }

        public void SendCallBack(IAsyncResult ar)
        {
            stream.EndWrite(ar);
        }

        
    }
}