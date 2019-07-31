using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Diagnostics;
using System.Web.Script.Serialization;
using tcplisten.Controllers;
using tcplisten.Entities;
namespace tcplisten
{
    class server
    {
        public void ListenToConnection()
        {

           

            IPEndPoint ipendPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["TcpEndpoint"].IPEndpoint;

            //TCPListener

            TcpListener listen = new TcpListener(ipendPoint) { ExclusiveAddressUse = false };

            //Start to listen

            listen.Start();

            Trace.WriteLine("Wait for requested connection !");




            TcpClient tcpClient;
            int clientcount = 0;

            //Start to listening
            while (true)
            {
                try
                {
                    tcpClient = listen.AcceptTcpClient();
                    

                    if (tcpClient.Connected)
                    {
                        clientcount += 1;

                        Trace.WriteLine("Accept the connection");


                        //Three-way handshake is done

                        HandleClient hldC = new HandleClient(tcpClient);

                        Thread th = new Thread(new ThreadStart(hldC.Communicate));

                        th.IsBackground = true;

                        th.Start();

                        




                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.Read();
                }
            }

        }

        public class HandleClient
        {

            private TcpClient mclient;
            public HandleClient(TcpClient _client)
            {

                this.mclient = _client;
                
            }


            public void Communicate()
            {

                try
                {

                    CommunicationBase comB = new CommunicationBase();

                    comB.SendMsg("You can send data", mclient);

                    bool run = true;

                    string[] allFile;

                    int count = 0;

                    while (run == true)
                    {

                        string receiveMsg = comB.ReadMessage(mclient);

                        switch (receiveMsg)
                        {
                            case "":
                                run = false;
                                break;
                            default:

                                count++;

                                allFile = receiveMsg.Split('\n');

                                List<string> bfile = allFile.ToList();
                                bfile.Remove(bfile.Last());

                                SendtoAzure(bfile);

                                break;

                        }

                    }

                    this.mclient.Close();
                }
                catch
                {

                    Trace.WriteLine("Client close the connection");

                    this.mclient.Close();



                }
            }




        }

        public static void SendtoAzure(List<string> fileall)
        {

            //JsonObject
            try
            {
                foreach (string onefile in fileall)
                {

                    JavaScriptSerializer js = new JavaScriptSerializer();

                    dynamic jobject = js.Deserialize<dynamic>(onefile);

                    string name = (string)jobject["FileName"];
                    object[] objects = (object[])jobject["Content"];
                    string macadd = (string)jobject["MacAddress"];
                    string timeZone = (string)jobject["timeZone"];
                    
                   
                    switch (name)
                    {
                        case "sleepTable.json":
                            sleepController sleepctrl = new sleepController();


                            List<sleepEntity> sleepentities = sleepctrl.processData(objects, macadd, timeZone);

                            GenericAzureTableEntity<sleepEntity> sleepG = new GenericAzureTableEntity<sleepEntity>("sleepTable");
                            if (sleepentities.Count > 0)
                            {
                                sleepG.BatchInset(sleepentities);
                            }
                            break;
                        case "fitnessTable.json":
                            fitnessController fitctrl = new fitnessController();

                            List<fitnessEntity> fitentities = fitctrl.processData(objects, macadd, timeZone);

                            GenericAzureTableEntity<fitnessEntity> fitgeneric = new GenericAzureTableEntity<fitnessEntity>("fitnessTable");

                            if (fitentities.Count > 0)
                            {
                                fitgeneric.BatchInset(fitentities);
                            }
                            break;

                        case "stepTable.json":

                            StepController stpctrl = new StepController();
                            List<stepEntity> entities = stpctrl.processData(objects, macadd, timeZone);

                            GenericAzureTableEntity<stepEntity> gestepentity = new GenericAzureTableEntity<stepEntity>("stepTable");
                            if (entities.Count > 0)
                            {
                                gestepentity.BatchInset(entities);
                            }

                            break;
                        case "pulseTable.json":
                            pulseController pulsctrl = new pulseController();
                            List<pulseEntity> pulseentities = pulsctrl.processData(objects, macadd, timeZone);

                            GenericAzureTableEntity<pulseEntity> pulseG = new GenericAzureTableEntity<pulseEntity>("pulseTable");
                            if (pulseentities.Count > 0)
                            {
                                pulseG.BatchInset(pulseentities);
                            }
                            break;
                    }



                }
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }


        }

        public static void ProcessStringReceiveFromServer(List<byte[]> receiveallbyte)
        {
            byte[] he = new byte[0];
            //byte[] c = new byte[0];
            int count = receiveallbyte.Count;
            for (int i = 0; i < count; i++)
            {

                int countperbyte = receiveallbyte[i].Length;

                he = he.Concat(receiveallbyte[i]).ToArray();
            }
            int filesize = 0;

            int startAtindex = 0;

            //for file size
            for (int i = 1; i <= 4; i++)
            {


                filesize = int.Parse(Encoding.ASCII.GetString(he, startAtindex, 4));

                //filesizes += int.Parse(Encoding.ASCII.GetString(he,startAtindex+4,4));

                string a = Encoding.ASCII.GetString(he, startAtindex, 1);

                String FileString = Encoding.ASCII.GetString(he, startAtindex + 4, filesize);



                startAtindex = startAtindex + filesize + 4;

                Console.WriteLine("File Size:{0}, FileContent:{1}", filesize, FileString);

            }



        }


        //給客戶端 和 主機端使用

        public class CommunicationBase
        {

            //send Message to client
            public void SendMsg(string message, TcpClient _client)
            {
                NetworkStream fromclientmsg = _client.GetStream();
                if (fromclientmsg.CanWrite)
                {
                    byte[] msgbyte = Encoding.Default.GetBytes(message);

                    //把 message write into client's stream
                    fromclientmsg.Write(msgbyte, 0, msgbyte.Length);

                }
            }

            //received Message from client
            public string readMsg(TcpClient _client)
            {

                //string receivemsg = "";
                StringBuilder receivemsg = new StringBuilder();

                byte[] receiveByte = new byte[_client.ReceiveBufferSize];


                int numberBufferRead = 0;

                NetworkStream ns = _client.GetStream();

                if (ns.CanRead)
                {
                    do
                    {
                        numberBufferRead = ns.Read(receiveByte, 0, receiveByte.Length);
                        //ns.Read() => read the inputStream into byte[]receiveByte
                        //numberBufferRead = ns.Read(receiveByte, 5, receiveByte.Length);

                        receivemsg.Append(Encoding.Default.GetString(receiveByte, 0, numberBufferRead));

                        //if (ns.DataAvailable != true) {
                        //    Thread.Sleep(1000);
                        //}

                        //receivemsg += Encoding.Default.GetString(receiveByte, 0, numberBufferRead);

                    } while (ns.DataAvailable);

                    if (numberBufferRead == 0)
                    {
                        return "";
                    }
                }

                return receivemsg.ToString();
            }

            public string ReadMessage(TcpClient _client)
            {

                //string receivemsg = "";
                StringBuilder receivemsg = new StringBuilder();

                //有沒有buffer size (defualt 8192 bytes 6xxxxbits) 都不夠裝的 危機
                byte[] receiveByte = new byte[_client.ReceiveBufferSize];

                bool readstream = true;

                int numberBufferRead = 0;

                List<byte[]> receivebyte = new List<byte[]>();

                NetworkStream ns = _client.GetStream();

                if (ns.CanRead)
                {
                    while (readstream)
                    {
                        do
                        {
                            numberBufferRead = ns.Read(receiveByte, 0, receiveByte.Length);
                            //ns.Read() => read the inputStream into byte[]receiveByte
                            //numberBufferRead = ns.Read(receiveByte, 5, receiveByte.Length);

                            if (numberBufferRead != 0)
                            {

                                receivebyte.Add(receiveByte.Take(numberBufferRead).ToArray());
                            }

                            receivemsg.Append(Encoding.Default.GetString(receiveByte, 0, numberBufferRead));

                            //if (ns.DataAvailable != true) {
                            //    Thread.Sleep(1000);
                            //}

                            //receivemsg += Encoding.Default.GetString(receiveByte, 0, numberBufferRead);

                        } while (ns.DataAvailable);

                        if (numberBufferRead == 0 && ns.DataAvailable != true)
                        {
                            readstream = false;
                        }
                    }
                }

                //Allreceivebyte = receivebyte;
                return receivemsg.ToString();
            }
        }



    }
}
