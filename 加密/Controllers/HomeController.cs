using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using 加密.Models;
using 加密.Service;

namespace 加密.Controllers
{

    public class HomeController : Controller
    {
        private readonly AesEncryptionService aesEncryptionService;

        public HomeController(AesEncryptionService aesEncryptionService)
        {
            this.aesEncryptionService = aesEncryptionService;
        }
     
        
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // 在这里使用加密服务对用户名和密码进行加密
                string encryptedUsername = aesEncryptionService.Encrypt(model.Username);
                string encryptedPassword = aesEncryptionService.Encrypt(model.Password);

                // 进行登录验证

                // 如果验证成功，可以在这里解密用户名和密码并进行后续操作
                string decryptedUsername = aesEncryptionService.Decrypt(encryptedUsername);
                string decryptedPassword = aesEncryptionService.Decrypt(encryptedPassword);

                // 进行后续操作
            }

            // 如果验证失败，返回登录页面
            return View();
            //return Redirect("https://google.com");
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
