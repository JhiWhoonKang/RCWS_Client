using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;  // 추가
using System.Net; // 추가
using System.Net.Sockets;  // 추가
using System.IO;  // 추가
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;

namespace RCWS_Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        StreamReader streamReader;  // 데이타 읽기 위한 스트림리더
        StreamWriter streamWriter;  // 데이타 쓰기 위한 스트림라이터   

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Thread thread1 = new Thread(TcpConnect);  // Thread 객채 생성, Form과는 별도 쓰레드에서 connect 함수가 실행
            thread1.IsBackground = true;  // Form이 종료되면 thread1도 종료.
            thread1.Start();  // thread1 시작.
        }

        private void btnConnectUDP_Click(object sender, EventArgs e)
        {
            Thread socketThread = new Thread(new ThreadStart(UdpConnect));
            socketThread.IsBackground = true;
            socketThread.Start();
        }

        private void TcpConnect()  // thread1에 연결된 함수. 메인폼과는 별도로 동작
        {
            TcpClient tcpClient1 = new TcpClient();  // TcpClient 객체 생성
            IPEndPoint ipEnd = new IPEndPoint(IPAddress.Parse(textBox_TCPIP.Text), int.Parse(textBox_TCPPort.Text));  // IP주소와 Port번호를 할당
            tcpClient1.Connect(ipEnd);  // 서버에 연결 요청
            writeTcpRichTextbox("서버 연결됨...");

            streamReader = new StreamReader(tcpClient1.GetStream());  // 읽기 스트림 연결
            streamWriter = new StreamWriter(tcpClient1.GetStream());  // 쓰기 스트림 연결
            streamWriter.AutoFlush = true;  // 쓰기 버퍼 자동으로 뭔가 처리..

            while (tcpClient1.Connected)  // 클라이언트가 연결되어 있는 동안
            {
                string receiveData1 = streamReader.ReadLine();  // 수신 데이타를 읽어서 receiveData1 변수에 저장
                writeTcpRichTextbox(receiveData1);  // 데이타를 수신창에 쓰기
            }
        }

        private static UdpClient udpClient;
        private static IPEndPoint endPoint;

        private void UdpConnect()
        {
            try
            {
                writeUdpRichTextbox("통신 시도 중...");

                int remotePort = int.Parse(textBox_UDPPort.Text);
                IPAddress remoteIPAddress = IPAddress.Parse(textBox_UDPIP.Text);

                udpClient = new UdpClient();
                endPoint = new IPEndPoint(remoteIPAddress, remotePort);
                writeUdpRichTextbox("통신 성공");

                while (true)
                {

                    byte[] receivedData = udpClient.Receive(ref endPoint);
                    string receivedMessage = Encoding.UTF8.GetString(receivedData);

                    writeUdpRichTextbox(receivedMessage);
                }
            }
            catch (Exception ex)
            {
                writeUdpRichTextbox("오류: " + ex.Message);
            }
        }

        private void writeTcpRichTextbox(string str)  // richTextbox1 에 쓰기 함수
        {
            richTcpConnectionStatus.Invoke((MethodInvoker)delegate { richTcpConnectionStatus.AppendText(str + "\r\n"); }); // 데이타를 수신창에 표시, 반드시 invoke 사용. 충돌피함.
            richTcpConnectionStatus.Invoke((MethodInvoker)delegate { richTcpConnectionStatus.ScrollToCaret(); });  // 스크롤을 젤 밑으로.
        }

        private void writeUdpRichTextbox(string str)  // richTextbox1 에 쓰기 함수
        {
            richUdpConnectionStatus.Invoke((MethodInvoker)delegate { richUdpConnectionStatus.AppendText(str + "\r\n"); }); // 데이타를 수신창에 표시, 반드시 invoke 사용. 충돌피함.
            richUdpConnectionStatus.Invoke((MethodInvoker)delegate { richUdpConnectionStatus.ScrollToCaret(); });  // 스크롤을 젤 밑으로.
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Video newVideo = new Video();
            newVideo.ShowDialog();
        }
    }
}