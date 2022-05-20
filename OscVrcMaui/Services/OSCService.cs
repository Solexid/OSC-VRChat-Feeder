using Rug.Osc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OscVrcMaui.Services
{
    // OSC Network service helper
   public class OSCService
    {
         OscReceiver receiver;
        OscSender oscSender;
        Thread thread;
        String SendingIp;
       public delegate void MessageHandler(OscPacket message);
       public event MessageHandler MessageReceived;
        int port = 9001;//recive
        int SendingPort = 9000;//recive
        public OSCService()
        {
            StartReceive();
        }
        public void Close() { 
        if(receiver!=null)
                receiver.Close();



        }
   
        public void StartReceive()
        {
            if (receiver != null)
                receiver.Close();
       
            receiver = new OscReceiver(port);
            // Create a thread to do the listening
            thread = new Thread(new ThreadStart(ListenLoop));

            // Connect the receiver
            receiver.Connect();


        }
        void ListenLoop()
        {
            try
            {
                while (receiver.State != OscSocketState.Closed)
                {
                    // if we are in a state to recieve
                    if (receiver.State == OscSocketState.Connected)
                    {
                        // get the next message 
                        // this will block until one arrives or the socket is closed
                        OscPacket packet = receiver.Receive();
                        MessageReceived?.Invoke(packet);
                        // Write the packet to the console 
                        Console.WriteLine(packet.ToString());

                        // DO SOMETHING HERE!
                    }
                }
            }
            catch (Exception ex)
            {
                // if the socket was connected when this happens
                // then tell the user
                if (receiver.State == OscSocketState.Connected)
                {
                    Console.WriteLine("Exception in listen loop");
                    Console.WriteLine(ex.Message);
                }
            }
        }
        public void SetIpAndRestart(string Ip) {


            SendingIp = Ip;
            IPAddress address = IPAddress.Parse(SendingIp);
            if (oscSender != null) {

                oscSender.Close();
                oscSender.Dispose(); 
            }
            oscSender = new OscSender(address, SendingPort);
            oscSender.Connect();

            oscSender.Send(new OscMessage("/avatar/parameters/SLXOscActive", 1));
          

        }
        public async Task<bool> SendIntAsync(string path,int data) {

            if (oscSender != null) {

                await Task.Run(() => { oscSender.Send(new OscMessage(path, data)); });
            
            }
                return true;
        }
        public async Task<bool> SendBoolAsync(string path, bool data)
        {

            if (oscSender != null)
            {

                await Task.Run(() => { oscSender.Send(new OscMessage(path, data)); });

            }
            return true;
        }

        public async Task<bool> SendFloatAsync(string path,float data)
        {
            if (oscSender != null)
            {

                await Task.Run(() => { oscSender.Send(new OscMessage(path, data)); });

            }
            return true;
        }
       





    }
}
