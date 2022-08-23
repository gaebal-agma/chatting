using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// 추가
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace chatting_client
{
    public partial class Form1 : Form
    {
        IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
        Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            client.Connect(localEndPoint);
            new Thread(Receiver).Start();

        }
        // 메세지 전송하면서 동시에 클라이언트 채팅창에 메세지 출력
        private void button1_Click_1(object sender, EventArgs e)
        {
            Byte[] buf = new byte[1024];
            String data;

            buf = Encoding.Default.GetBytes(textBox1.Text);
            client.Send(buf);

            data = Encoding.Default.GetString(buf);

            textBox2.AppendText("\r\n" + " I say >> " + data);

            textBox1.Clear();
        }

        //다른 클라이언트로부터 온 메세지 채팅창에 출력
        private void Receiver()
        {
            byte[] buf = new byte[1024];
            String data;

            while (true)
            {
                client.Receive(buf);

                data = Encoding.Default.GetString(buf);

                textBox2.AppendText("\r\n" + data);

                System.Array.Clear(buf, 0, 1024);

            }
        }

    }
}