using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace TCP_Chat
{
    class MyHmac
    {
        RNGCryptoServiceProvider rng;
        public byte[] secretkey = new Byte[64];

        public MyHmac()
        {
            rng = new RNGCryptoServiceProvider();
            rng.GetBytes(secretkey);
        }

        public String SignFile(String source)
        {
            HMACSHA256 hmac = new HMACSHA256(secretkey);

            byte[] temphashValue = Encoding.UTF8.GetBytes(source);
            byte[] hashValue = hmac.ComputeHash(temphashValue);


            String tempDestination = Convert.ToBase64String(hashValue);
            return tempDestination;

        }


        public bool VerifyFile(String source, String hash)
        {
            HMACSHA256 hmac = new HMACSHA256(secretkey);

            byte[] temphashValue = Encoding.UTF8.GetBytes(source);
            byte[] hashValue = hmac.ComputeHash(temphashValue);


            String tempDestination = Convert.ToBase64String(hashValue);
            if (hash.Equals(tempDestination))
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        public String SignFileArray(String[] source)
        {
            StringBuilder sb = new StringBuilder();
            HMACSHA256 hmac = new HMACSHA256(secretkey);


            foreach (string x in source)
            {
                if (!(String.IsNullOrEmpty(x)))
                {
                    byte[] temphashValue0 = Encoding.UTF8.GetBytes(x);
                    byte[] hashValue0 = hmac.ComputeHash(temphashValue0);
                    String tempDestination0 = Convert.ToBase64String(hashValue0);
                    sb.Append(tempDestination0);
                }
            }


            String ReturnValue = Convert.ToString(sb);

            return ReturnValue;

        }

        public bool VerifyFileArray(String[] source, String hash)
        {
            HMACSHA256 hmac = new HMACSHA256(secretkey);

            String tempDestination = SignFileArray(source);


            if (hash.Equals(tempDestination))
            {
                return true;
            }
            else
            {
                return false;
            }


        }
    }
}
