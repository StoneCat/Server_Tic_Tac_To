/*
 * Created by SharpDevelop.
 * User: luis_nikolai
 * Date: 19.02.2018
 * Time: 18:48
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace Server_Tic_Tac_To
{
    class TConnectedClient
    {
        public TcpClient Client { get; private set; }
        public StreamWriter Writer { get; private set; }
        public StreamReader Reader { get; private set; }
        public IPEndPoint ClientEP { get; private set; }

        public TConnectedClient(TcpClient aClient)
        {
            Client = aClient;
            Writer = new StreamWriter(Client.GetStream());
            Reader = new StreamReader(Client.GetStream());
            ClientEP = (IPEndPoint)Client.Client.RemoteEndPoint;
        }

        public void sendMessage (string msg)
        {
            Writer.WriteLine(msg);
            Writer.Flush();
        }

        public string receiveMessage()
        {
            return Reader.ReadLine();
        }
    }
}
