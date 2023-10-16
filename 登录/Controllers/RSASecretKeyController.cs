using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Result;
using System;
using 登录.Model;
using System.Text;
using System.Security.Cryptography;

namespace 登录.Controllers
{
    public record User(string Name,string Password);
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RSASecretKeyController : ControllerBase
    {
         static RSASecretKey rSASecretKey ;

        static RSASecretKeyController()
        {
            rSASecretKey = new RSASecretKey();
        }

        [HttpGet]
        public ResultApi PublicKey() {

            if (rSASecretKey.PublicKey == null)
            {
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    // 获取RSA公钥
                    var publicKeyParams = rsa.ToXmlString(false);
                    var privateKeyParams = rsa.ToXmlString(true);
                    rSASecretKey.PrivateKey = privateKeyParams;
                    rSASecretKey.PublicKey = publicKeyParams;
                }
            }
           
             return ResultHelper.Success("公钥" + rSASecretKey.PublicKey + "/n私钥" + rSASecretKey.PrivateKey);
                       
        }
        [HttpPost]
        public ResultApi Login(User user)
        {
            string privateKey = rSASecretKey.PrivateKey;
            
            string v = RSAEncrypt(rSASecretKey.PublicKey, user.Password);
            
            string v1 = RSADecrypt(rSASecretKey.PrivateKey, v);
            RSAModel rSAModel = new RSAModel(user.Password,v, v1);
            return ResultHelper.Success(v1);
            
        }
        public record RSAModel(string pass,string encyptpass,string decyptpass);
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
                byte[] bytes = Convert.FromBase64String(content);
                byte[] decryptedData = rsa.Decrypt(bytes, false);
                decryptedContent = Encoding.GetEncoding("UTF-8").GetString(decryptedData);
            }
            return decryptedContent;
        }
    }
}
