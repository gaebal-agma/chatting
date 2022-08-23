
//콘솔 형태로 통신서버개발
using System;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


namespace chatting_server
{
    class Program
    {

        public static int Index = 0;

        public static Socket[] SocketClient = new Socket[5];

        static void Main(string[] args)
        {
            //소켓을 사용하기 위한 코드
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Any, 9999);

            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            try
            {
                //소켓을 바인드 , 리슨
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    //동기식 방식으로 진행
                    SocketClient[Index] = listener.Accept();
                    // 통신되면 index 하나 추가
                    Index++;
                    //연결이 되고 동기식에서 비동기식으로 변환
                    new Thread(delegate () { Receiver(Index - 1); }).Start();

                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        static void Receiver(int idx)
        {
            Console.WriteLine(idx + " connected!");

            byte[] buf = new byte[1024];
            String data;

            while (true)
            {
                //클라이언트의 입력 대기
                SocketClient[idx].Receive(buf);

                data = idx.ToString() + " >> " + Encoding.Default.GetString(buf);

                for (int k = 0; k < Index; k++)
                    if (k != idx) SocketClient[k].Send(Encoding.Default.GetBytes(data));
                // 다른 클라이언트에게 채팅내용 전파
                // 본인에게는 전파하지 않음

                System.Array.Clear(buf, 0, 1024);

            }

        }

    }
}