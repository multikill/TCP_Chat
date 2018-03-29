using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace TCP_Chat
{
    public partial class MyClient : Form
    {
        Thread threadServer;
        //MyTcpListener TcpServer = new MyTcpListener("127.0.0.1", Convert.ToString(MyTcpListener.ValidPort()));
        //MyTcpListener TcpServer = new MyTcpListener("10.215.121.106", "13000");
        MyTcpListener TcpServer = new MyTcpListener();
        MyRsa serverRSA = new MyRsa();
        MyRsa clientRSA = new MyRsa();
        MyTcpClient Tclient;
        MyAes serverAes = new MyAes();
        MyHmac hmac = new MyHmac();
        string[] hmacVergleich = new string[10];


        public String SenderIP;
        public String SenderPort;

        private bool VerbindungOK = false;

        public MyClient()
        {
            InitializeComponent();

            threadServer = new Thread(server_receiving);
            threadServer.Start();
            this.Text = Convert.ToString("Server is running on   " + TcpServer.IP + ":" + TcpServer.Port);

            //MyHmac hmac1 = new MyHmac();
            //MyHmac hmac2 = new MyHmac();
            //hmac1.secretkey = hmac2.secretkey;
            //String[] Array = { "Erster String", "Zweiter String" };
            //String temp = hmac1.SignFileArray(Array);
            //Console.WriteLine(hmac2.VerifyFileArray(Array, temp));

            hmacVergleich[0] = "testc";
        }

        private void txt_send_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                this.txt_send.Clear();
                //this.txt_send.Text = txt_send.Text.Replace(System.Environment.NewLine, "");
            }
        }


        private void txt_send_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_send_Click(this, new EventArgs());

            }
        }

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (VerbindungOK)
            {
                ReleaseVerbindung();
                String SendingKiv = System.Convert.ToBase64String(serverAes.getNewIV());
                String SendingMsg = System.Convert.ToBase64String(serverAes.EncryptAes(this.txt_send.Text));
                Tclient.Connect("Message" + "|" + TcpServer.IP + "|" + TcpServer.Port + "|" + SendingKiv + "|" + SendingMsg);

                this.txt_receive.AppendText("Ich:\t\t" + this.txt_send.Text + Environment.NewLine);
                this.txt_send.Clear();

            }
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            if (VerbindungOK)
            {
                VerbindungOK = false;
                DisconnectVerbindung();
            }
            else
            {
                Tclient = new MyTcpClient(txt_host.Text, Convert.ToInt32(txt_port.Text));
                hmacVergleich[0] = Convert.ToString("RsaPublicRequest" + "|" + TcpServer.IP + "|" + TcpServer.Port);
                Tclient.Connect(hmacVergleich[0]);

                Thread.Sleep(500);
                if (VerbindungOK)
                {
                    ReleaseVerbindung();
                }
                else
                {
                    DisconnectVerbindung();
                }
            }

        }

        private void server_receiving()
        {

            while (true)
            {
                String temp = TcpServer.WaitForConnectionAndMessage();
                //Console.WriteLine(temp);

                String[] tempsplit = temp.Split('|');

                if (VerbindungOK)
                {
                    if (tempsplit[0].Equals("Message"))
                    {
                        Tclient = new MyTcpClient(tempsplit[1], Convert.ToInt32(tempsplit[2]));

                        byte[] receiveKiv = System.Convert.FromBase64String(tempsplit[3]);
                        byte[] receiveMsg = System.Convert.FromBase64String(tempsplit[4]);
                        //byte[] receiveHash = System.Convert.FromBase64String(tempsplit[5]);
                        serverAes.setIV(receiveKiv);
                        String Nachricht = serverAes.DecryptAes(receiveMsg);

                        SetText(tempsplit[1] + ":\t" + Nachricht);
                    }
                }
                else
                {
                    // Anfrage vom RSA Public Key
                    // Senden vom RSA Public Key
                    if (tempsplit[0].Equals("RsaPublicRequest"))
                    {
                        Tclient = new MyTcpClient(tempsplit[1], Convert.ToInt32(tempsplit[2]));
                        hmacVergleich[0] = temp;
                        hmacVergleich[1] = ("RsaPublicAnswer" + "|" + TcpServer.IP + "|" + TcpServer.Port + "|" + serverRSA.PublicKeyget());
                        Tclient.Connect(hmacVergleich[1]);
                    }
                    // Empfagen von der RSA Public Key 
                    // Senden vom AES Key verschlüsselt mit dem RSA Public Key
                    else if (tempsplit[0].Equals("RsaPublicAnswer"))
                    {
                        Tclient = new MyTcpClient(tempsplit[1], Convert.ToInt32(tempsplit[2]));
                        hmacVergleich[1] = temp;

                        clientRSA.PublicKeyset(tempsplit[3]);

                        //String strAesKey = Encoding.ASCII.GetString(serverAes.getKey());
                        String strAesKey = System.Convert.ToBase64String(serverAes.getKey());
                        byte[] strRsaAesKey = clientRSA.verschluesseln(strAesKey);
                        //byte[] text = System.Text.Encoding.UTF8.GetBytes(strRsaAesKey);
                        String strRsaAesBase64Key = System.Convert.ToBase64String(strRsaAesKey);

                        // HMAC verschlüsseln und senden
                        String strHmacKey = System.Convert.ToBase64String(hmac.secretkey);
                        byte[] strRsaHmacKey = clientRSA.verschluesseln(strHmacKey);
                        String strRsaHmacBase64Key = System.Convert.ToBase64String(strRsaHmacKey);

                        hmacVergleich[2] = ("AesSetKey" + "|" + TcpServer.IP + "|" + TcpServer.Port + "|" + strRsaAesBase64Key + "|" + strRsaHmacBase64Key);
                        Tclient.Connect(hmacVergleich[2]);
                    }
                    // Empfgangen vom AES Key verschlüssellt im RSA Public Key
                    // Sende AES Test string
                    else if (tempsplit[0].Equals("AesSetKey"))
                    {
                        Tclient = new MyTcpClient(tempsplit[1], Convert.ToInt32(tempsplit[2]));
                        hmacVergleich[2] = temp;

                        ////////byte[] strRsaAesBas64Key = System.Convert.FromBase64String(getkeytest());
                        byte[] strRsaAesBas64Key = System.Convert.FromBase64String(tempsplit[3]);
                        //String text = System.Text.Encoding.UTF8.GetString(strRsaAesBas64Key);
                        string AesRsaSchluessel = serverRSA.entschluesseln(strRsaAesBas64Key);
                        byte[] AesSchluessel = System.Convert.FromBase64String(AesRsaSchluessel);
                        serverAes.setKey(AesSchluessel);

                        byte[] strRsaHmacBase64Key = System.Convert.FromBase64String(tempsplit[4]);
                        string strRsaHmacKey = serverRSA.entschluesseln(strRsaHmacBase64Key);
                        byte[] strHmacKey = System.Convert.FromBase64String(strRsaHmacKey);
                        hmac.secretkey = strHmacKey;

                        String SendingKiv = System.Convert.ToBase64String(serverAes.getKIV());
                        String SendingMsg = System.Convert.ToBase64String(serverAes.EncryptAes("ValidString"));

                        hmacVergleich[3] = ("AesCheck" + "|" + TcpServer.IP + "|" + TcpServer.Port + "|" + SendingKiv + "|" + SendingMsg);
                        Tclient.Connect(hmacVergleich[3]);
                    }
                    // Emfange Prüfstring
                    // Sende Prüfstring in AES with new IV. Setze Verbindung auf OK
                    else if (tempsplit[0].Equals("AesCheck"))
                    {
                        Tclient = new MyTcpClient(tempsplit[1], Convert.ToInt32(tempsplit[2]));
                        hmacVergleich[3] = temp;

                        byte[] receiveKiv = System.Convert.FromBase64String(tempsplit[3]);
                        byte[] receiveMsg = System.Convert.FromBase64String(tempsplit[4]);
                        serverAes.setIV(receiveKiv);
                        String Nachricht = serverAes.DecryptAes(receiveMsg);

                        String SendingKiv = System.Convert.ToBase64String(serverAes.getNewIV());
                        String SendingMsg = System.Convert.ToBase64String(serverAes.EncryptAes("ValidRequestString"));

                        hmacVergleich[4] = ("AesRequestCheck" + "|" + TcpServer.IP + "|" + TcpServer.Port + "|" + SendingKiv + "|" + SendingMsg);
                        Tclient.Connect(hmacVergleich[4]);

                        if (Nachricht.Equals("ValidString"))
                        {
                            Console.WriteLine("VERSCHLUESSELTE VERBINDUNG ERFOLGREICH HERGESTELLT!");
                            //VerbindungOK = true;
                        }
                    }
                    // Empfange Prüfstring in AES
                    // Setzte Verbindung auf OK und sende Hmac Check
                    else if (tempsplit[0].Equals("AesRequestCheck"))
                    {
                        Tclient = new MyTcpClient(tempsplit[1], Convert.ToInt32(tempsplit[2]));
                        hmacVergleich[4] = temp;
                        String strHmacSigned = hmac.SignFileArray(hmacVergleich);

                        byte[] receiveKiv = System.Convert.FromBase64String(tempsplit[3]);
                        byte[] receiveMsg = System.Convert.FromBase64String(tempsplit[4]);
                        serverAes.setIV(receiveKiv);
                        String Nachricht = serverAes.DecryptAes(receiveMsg);
                        
                        if (Nachricht.Equals("ValidRequestString"))
                        {
                            SetText("Verbindung mit " + tempsplit[1] + ":" + tempsplit[2] + " erfolgreich hergestellt!");
                            Console.WriteLine("VERSCHLUESSELTE VERBINDUNG ERFOLGREICH HERGESTELLT!");                            
                            VerbindungOK = true;
                            //ReleaseVerbindung();
                        }

                        String SendingKiv = System.Convert.ToBase64String(serverAes.getNewIV());
                        String SendingMsg = System.Convert.ToBase64String(serverAes.EncryptAes(strHmacSigned));

                        Tclient.Connect("HmacCheck" + "|" + TcpServer.IP + "|" + TcpServer.Port + "|" + SendingKiv + "|" + SendingMsg);
                    }
                    else if (tempsplit[0].Equals("HmacCheck"))
                    {
                        Tclient = new MyTcpClient(tempsplit[1], Convert.ToInt32(tempsplit[2]));
                        
                        //String strHmacSigned = hmac.SignFileArray(hmacVergleich);

                        byte[] receiveKiv = System.Convert.FromBase64String(tempsplit[3]);
                        byte[] receiveMsg = System.Convert.FromBase64String(tempsplit[4]);
                        serverAes.setIV(receiveKiv);
                        String Nachricht = serverAes.DecryptAes(receiveMsg);

                        if (hmac.VerifyFileArray(hmacVergleich, Nachricht))
                        {
                            Console.WriteLine("Nachrichten verifiziert!");
                            SetText("Verbindung mit " + tempsplit[1] + ":" + tempsplit[2] + " erfolgreich hergestellt!");
                            VerbindungOK = true;
                        }
                    }
                }

            }

        }

        private void ReleaseVerbindung()
        {
            this.txt_host.ReadOnly = true;
            this.txt_port.ReadOnly = true;
            //this.btn_connect.Enabled = false;

            this.btn_connect.Text = "Disconnect";
        }

        private void DisconnectVerbindung()
        {
            this.txt_host.ReadOnly = false;
            this.txt_port.ReadOnly = false;
            //this.btn_connect.Enabled = false;
            //this.btn_send.enabled = false;

            this.btn_connect.Text = "Connect";
        }

        // This delegate enables asynchronous calls for setting  
        // the text property on a TextBox control.  
        delegate void StringArgReturningVoidDelegate(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            if (this.txt_receive.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.txt_receive.AppendText(text + Environment.NewLine);
            }
        }

        private void MyClient_Load(object sender, EventArgs e)
        {

        }

        private void MyClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            TcpServer.Stop();
            threadServer.Abort();
        }
    }
}
