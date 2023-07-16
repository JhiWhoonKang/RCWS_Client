//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Net.Sockets;
//using System.Net;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using OpenCvSharp;
//using System.IO;

//namespace RCWS_Client
//{
//    public partial class Video : Form
//    {
//        private const int Width = 1080;
//        private const int Height = 720;
//        private const int Port = 5001;
//        private const string ServerIP = "127.0.0.1";

//        private UdpClient _udpClient;
//        private IPEndPoint _endPoint;
//        private CancellationTokenSource _cancellationTokenSource;
//        private VideoCapture _capture;

//        public Video()
//        {
//            InitializeComponent();

//            _udpClient = new UdpClient(Port);
//            _endPoint = new IPEndPoint(IPAddress.Parse(ServerIP), Port);
//            _cancellationTokenSource = new CancellationTokenSource();
//            _capture = new VideoCapture(0);

//            if (!_capture.IsOpened())
//            {
//                MessageBox.Show("Camera Open failed");
//                Application.Exit();
//            }
//            else
//            {
//                MessageBox.Show("Camera Open Success");
//            }

//            Thread.Sleep(2000);

//            string clientIP = "clientIP";

//            UdpClient udpClient = new UdpClient(clientIP, Port);
//            Thread captureThread = new Thread(() => CaptureAndSend(udpClient, _capture));
//            captureThread.Start();
//            //captureThread.Join();
//        }

//        private static void CaptureAndSend(UdpClient udpClient, VideoCapture Capture)
//        {
//            Console.WriteLine("Client start");

//            while (true)
//            {
//                try
//                {
//                    using (var image = new Mat())
//                    {
//                        Capture.Read(image);
//                        if (image.Empty()) continue;
//                        var imageData = ConvertImageToByteArray(image);

//                        if (imageData != null)
//                        {
//                            udpClient.Send(imageData, imageData.Length);

//                            // ACK를 수신할 때까지 대기합니다. 
//                            IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, 0);
//                            byte[] ackBuffer = udpClient.Receive(ref serverEndPoint);
//                            string ackMessage = Encoding.ASCII.GetString(ackBuffer, 0, ackBuffer.Length);
//                            if (ackMessage != "ACK") throw new InvalidDataException("ACK message not received");
//                        }
//                    }
//                }

//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex.Message);
//                }
//            }
//        }

//        private static byte[] ConvertImageToByteArray(Mat Image)
//        {
//            using (var ms = new System.IO.MemoryStream())
//            {
//                using (var bitmap = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(Image))
//                {
//                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
//                }
//                return ms.ToArray();
//            }
//        }
//    }
//}

using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using OpenCvSharp;

namespace RCWS_Client
{
    public partial class Video : Form
    {
        private const int Port = 5001;

        private UdpClient _udpClient;
        private IPEndPoint _remoteEndPoint;

        public Video()
        {
            InitializeComponent();

            _udpClient = new UdpClient(Port);
            _remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

            pictureBox_Display.SizeMode = PictureBoxSizeMode.StretchImage;

            // 영상 수신 및 디코딩 스레드
            Thread receiveAndDisplayThread = new Thread(ReceiveAndDisplay);
            receiveAndDisplayThread.IsBackground = true;
            receiveAndDisplayThread.Start();
        }

        private void ReceiveAndDisplay()
        {
            while (true)
            {
                try
                {
                    byte[] imageData = _udpClient.Receive(ref _remoteEndPoint);

                    // 데이터 응답
                    _udpClient.Send(new byte[] { 1 }, 1, _remoteEndPoint);

                    Mat rawData = new Mat(1, imageData.Length, MatType.CV_8UC3, imageData);
                    var image = Cv2.ImDecode(rawData, ImreadModes.Color);

                    pictureBox_Display.Image = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(image);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Server Error: " + ex.Message);
                }
            }
        }
    }
}
