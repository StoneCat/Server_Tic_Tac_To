/*
 * Created by SharpDevelop.
 * User: luis_nikolai
 * Date: 19.02.2018
 * Time: 18:46
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

namespace Server_Tic_Tac_To
{
	public class TServer
    {
        public static int ServerPort = 12345;

        bool running;

        // Verwaltung der verbundenen Clients
        List<TConnectedClient> clientList = new List<TConnectedClient>();

        public TServer()
        {
            // Server starten
            running = true;
            Thread serverThread = new Thread(waitForClients);
            serverThread.Start();
        }

        public void Stop()
        {
            running = false;
        }

        void waitForClients()
        {
            // Auf dem Port lauschen
            TcpListener socket = new TcpListener(IPAddress.Any, ServerPort);
            TcpClient acceptedClient;
            
            socket.Start();

            Console.WriteLine("Waiting for client connection.");

            while (running)
            {
                if (socket.Pending())
                {
                    acceptedClient = socket.AcceptTcpClient();

                    // neu akzeptierten Client im eignen Thread behandeln
                    Thread thr = new Thread(processClient);
                    thr.Start(acceptedClient);
                }
            }

            // wenn fertig, dann Liste exklusiv sperren
            lock (clientList)
            {
                // und alle Clients beenden
                foreach (TConnectedClient c in clientList)
                {
                    c.sendMessage("<q>");

                    c.Client.Close();
                }
            }
        }

        // Kommunikation mit dem Client
        public void processClient(Object clientObj)
        {
            bool clientRunning = true;

            // neuen Client in der Liste speichern
            TConnectedClient connectedClient =
                new TConnectedClient((TcpClient)clientObj);

            // Connect des neuen Clients anzeigen
            Console.WriteLine("Connected on IP: " + connectedClient.ClientEP.Address +
                              " Port: " + connectedClient.ClientEP.Port);

            // neuen Client in Liste eintragen
            lock (clientList)
            {
                clientList.Add(connectedClient);
            }

            // Connect-Message an Client schicken
            connectedClient.sendMessage("Successfully connection to the server on port:" +
                      connectedClient.ClientEP.Port);

            // Kommunikations-Schleife
            while (clientRunning)
            {
                try
                {
                    // Warten auf Message vom Client
                    string msg = connectedClient.receiveMessage();

                    // wenn nicht "Ende"
                    if (msg != "<q>")
                    {
                        // Message im Serverfenster anzeigen
                        Console.WriteLine(connectedClient.ClientEP.Port + " " + msg);
                    }
                    else
                    {
                        clientRunning = false;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                    clientRunning = false;
                }
            }

            // Client aus der Liste entfernen
            lock (clientList)
            {
                clientList.Remove(connectedClient);
            }

            // und schließen
            connectedClient.Client.Close();

            // Schließen im Serverfenster anzeigen
            Console.WriteLine("Connection to IP: " + connectedClient.ClientEP.Address +
                                " Port: " + connectedClient.ClientEP.Port + " closed");
        }
    }
}
