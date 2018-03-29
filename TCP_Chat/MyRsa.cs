using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace TCP_Chat
{
    class MyRsa
    {
        private RSACryptoServiceProvider rsaGenKey = new RSACryptoServiceProvider();
        private string privateXml = "";
        private string publicXml = "";

        public MyRsa()
        {
            privateXml = rsaGenKey.ToXmlString(true);
            publicXml = rsaGenKey.ToXmlString(false);
        }


        public string PrivateKeyget()
        {
            privateXml = rsaGenKey.ToXmlString(true);
            return privateXml;
        }
        public void PrivateKeyset(string temp)
        {
            privateXml = temp;
            rsaGenKey.FromXmlString(privateXml);
        }


        public string PublicKeyget()
        {
            publicXml = rsaGenKey.ToXmlString(false);
            return publicXml;
        }
        public void PublicKeyset(string temp)
        {
            publicXml = temp;
            rsaGenKey.FromXmlString(publicXml);
        }


        public byte[] verschluesseln(string message)
        {
            rsaGenKey.ImportParameters(rsaGenKey.ExportParameters(false));
            byte[] toEncryptData = Encoding.Default.GetBytes(message);
            byte[] encRSA = rsaGenKey.Encrypt(toEncryptData, false);
            //String temp = Encoding.ASCII.GetString(encRSA);
            return encRSA;
        }

        public string entschluesseln(byte[] message)
        {
            //byte[] temp = Encoding.ASCII.GetBytes(message);
            byte[] decRSA = rsaGenKey.Decrypt(message, false);
            string ergebnis = Encoding.Default.GetString(decRSA);
            return ergebnis;
        }
    }
}
