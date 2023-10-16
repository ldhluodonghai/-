using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly string key = "YourEncryptionKe"; // AES密钥，与前端一致
        private readonly string iv = "YourEncryptionIV";   // 初始化向量IV，与前端一致

        [HttpPost]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                // 将前端发送的加密数据解密
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);
                byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
                using Aes aesAlg = Aes.Create();
                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] encryptedUsernameBytes = Convert.FromBase64String(request.EncryptedUsername);
                byte[] encryptedPasswordBytes = Convert.FromBase64String(request.EncryptedPassword);

                string username = DecryptStringFromBytes(encryptedUsernameBytes, decryptor);
                string password = DecryptStringFromBytes(encryptedPasswordBytes, decryptor);

                // 在这里进行身份验证和登录逻辑

                return Ok("Login successful"); // 返回成功消息或其他信息
            }
            catch (Exception ex)
            {
                return BadRequest($"Login failed: {ex.Message}"); // 返回登录失败信息
            }
        }

        private string DecryptStringFromBytes(byte[] cipherText, ICryptoTransform decryptor)
        {
            using MemoryStream msDecrypt = new MemoryStream(cipherText);
            using CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using StreamReader srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }
    }

    public class LoginRequest
    {
        public string EncryptedUsername { get; set; }
        public string EncryptedPassword { get; set; }
    }

}
