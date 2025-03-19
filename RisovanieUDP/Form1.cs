using System.Net.Sockets;
using System.Net;
using System.Text;

namespace RisovanieUDP
{
    public partial class Form1 : Form
    {
        private UdpClient client;
        private IPEndPoint serverEndpoint;
        private Pen pen = new Pen(Color.Black, 3);
        private List<Point> currentCurve = new List<Point>();
        private List<List<Point>> curves = new List<List<Point>>();
        public Form1()
        {
            InitializeComponent();
            client = new UdpClient();
            serverEndpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888); // сокет
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            curves.Add(currentCurve);
            currentCurve = null;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                currentCurve.Add(e.Location);
                string data = $"{currentCurve[currentCurve.Count - 2].X},{currentCurve[currentCurve.Count - 2].Y},{currentCurve[currentCurve.Count - 1].X},{currentCurve[currentCurve.Count - 1].Y},{pen.Color.R},{pen.Color.G},{pen.Color.B}";
                byte[] bytes = Encoding.ASCII.GetBytes(data);
                client.Send(bytes, bytes.Length, serverEndpoint); 
                Invalidate(); 
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            currentCurve = new List<Point>();
            currentCurve.Add(e.Location);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                pen.Color = colorDialog.Color; 
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            foreach (var curve in curves)
            {
                if (curve.Count > 1)
                {
                    e.Graphics.DrawCurve(pen, curve.ToArray());
                }
            }
            // текущая кривая
            if (currentCurve != null && currentCurve.Count > 1)
            {
                e.Graphics.DrawCurve(pen, currentCurve.ToArray());
            }
        }
    }
}
