using System;
using System.Security.Cryptography;
using System.Text;

namespace ConsoleApp1
{
    struct RSASecretKey
    {
        public RSASecretKey(string privateKey, string publicKey)
        {
            PrivateKey = privateKey;
            PublicKey = publicKey;
        }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
     
    }

/*     public RSASecretKey GenerateRSASecretKey(int keySize)
    {
        RSASecretKey rsaKey = new RSASecretKey();
        using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(keySize))
        {
            rsaKey.PrivateKey = rsa.ToXmlString(true);
            rsaKey.PublicKey = rsa.ToXmlString(false);
        }
        return rsaKey;
    }*/
    internal class Program
    {

        static void Main(string[] args)
        {
            using (RSA rsa = RSA.Create())
            {
                // 获取RSA公钥
                var publicKeyParams = rsa.ToXmlString(false);
                var privateKeyParams = rsa.ToXmlString(true);
                RSASecretKey rSASecretKey = new RSASecretKey(privateKeyParams, publicKeyParams);

                // 将公钥参数序列化为JSON格式
                Console.WriteLine(publicKeyParams);
                Console.WriteLine("=====================================");
                Console.WriteLine(privateKeyParams);

                string a = "你好";
                string v = RSAEncrypt(rSASecretKey.PublicKey, a);
                Console.WriteLine("加密后内容===="+v);
                string v1 = RSADecrypt(rSASecretKey.PrivateKey, v);
                Console.WriteLine("解密后===="+v1);
            }
        }
        private static string RSAEncrypt(string xmlPublicKey, string content)
        {
            string encryptedContent = string.Empty;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPublicKey);
                byte[] encryptedData = rsa.Encrypt(Encoding.Default.GetBytes(content), false);
                encryptedContent = Convert.ToBase64String(encryptedData);
            }
            return encryptedContent;
        }

        private static string RSADecrypt(string xmlPrivateKey, string content)
        {
            string decryptedContent = string.Empty;
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                rsa.FromXmlString(xmlPrivateKey);
                byte[] decryptedData = rsa.Decrypt(Convert.FromBase64String(content), false);
                decryptedContent = Encoding.GetEncoding("UTF-8").GetString(decryptedData);
            }
            return decryptedContent;
        }
    }
}
