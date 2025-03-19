using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ServerUDP
{
    public partial class Form1 : Form
    {
        private UdpClient listener;
        private IPEndPoint clientEndpoint;
        private Pen pen = new Pen(Color.Red, 3);
        public Form1()
        {
            InitializeComponent();
            listener = new UdpClient(8888); // Порт сервера
            clientEndpoint = new IPEndPoint(IPAddress.Any, 0);

            Thread receiveThread = new Thread(new ThreadStart(ReceiveData));
            receiveThread.Start();
        }

        private void ReceiveData()
        {
            while (true)
            {
                byte[] data = listener.Receive(ref clientEndpoint);
                string receivedData = Encoding.ASCII.GetString(data);
                string[] parts = receivedData.Split(',');
                if (parts.Length == 7) // данные содержат все необходимые части (координаты и цвет)
                {
                    int x1 = int.Parse(parts[0]);
                    int y1 = int.Parse(parts[1]);
                    int x2 = int.Parse(parts[2]);
                    int y2 = int.Parse(parts[3]);
                    int r = int.Parse(parts[4]);
                    int g = int.Parse(parts[5]);
                    int b = int.Parse(parts[6]);
                    Point startPoint = new Point(x1, y1);
                    Point endPoint = new Point(x2, y2);
                    Color color = Color.FromArgb(r, g, b); 
                    Pen pen = new Pen(color, 3); 
                    Graphics graph = CreateGraphics();
                    graph.DrawLine(pen, startPoint, endPoint);
                }
            }
        }
    }
}
