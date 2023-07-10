using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenCvSharp;

namespace RCWS_Client
{
    public partial class Video : Form
    {
        private const int Width = 1080;
        private const int Height = 720;
        private const int Port = 9000;
        private const string ServerIP = "192.168.0.2";

        private UdpClient _udpClient;
        private IPEndPoint _endPoint;
        private CancellationTokenSource _cancellationTokenSource;
        private VideoCapture _capture;

        public Video()
        {
            InitializeComponent();

            _udpClient = new UdpClient(Port);
            _endPoint = new IPEndPoint(IPAddress.Parse(ServerIP), Port);
            _cancellationTokenSource = new CancellationTokenSource();
            _capture = new VideoCapture(0);

            if (!_capture.IsOpened())
            {
                MessageBox.Show("Camera Open failed");
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Camera Open Success");
            }

            Thread.Sleep(2000);

            string clientIP = "clientIP";

            UdpClient udpClient = new UdpClient(clientIP, Port);
            Thread captureThread = new Thread(() => CaptureAndSend(udpClient, _capture));
            captureThread.Start();
            captureThread.Join();
        }

        private static void CaptureAndSend(UdpClient udpClient, VideoCapture Capture)
        {
            Console.WriteLine("Client start");

            while (true)
            {
                try
                {
                    using (var image = new Mat())
                    {
                        Capture.Read(image);
                        if (image.Empty()) continue;
                        var imageData = ConvertImageToByteArray(image);

                        if (imageData != null)
                        {
                            udpClient.Send(imageData, imageData.Length);
                        }
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private static byte[] ConvertImageToByteArray(Mat Image)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                using (var bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(Image))
                {
                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                }
                return ms.ToArray();
            }
        }
    }
}