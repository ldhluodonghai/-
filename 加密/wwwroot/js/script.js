// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.addEventListener("DOMContentLoaded", function () {
    const loginForm = document.getElementById("login-form");

    loginForm.addEventListener("submit", function (e) {
        e.preventDefault(); // 阻止默认表单提交行为

        const username = document.getElementById("username").value;
        const password = document.getElementById("password").value;

        // 在此处执行与后端通信的代码，将用户名和密码发送到服务器进行验证
        // 您可以使用Fetch API或其他AJAX库来发送POST请求

        // 示例：使用Fetch API发送POST请求
        fetch("/Home/Login", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                username: username,
                password: password,
            }),
        })
            .then(response => {
                if (response.ok) {
                    // 登录成功，执行后续操作，例如重定向到受保护页面
                    console.error("Login failed");
                    alert("登录成功")
                    //location.href= "@Url.Action("Index", "Home")"
                } else {
                    // 登录失败，处理错误
                    console.error("Login failed");
                }
            })
            .catch(error => {
                console.error("Error:", error);
            });
    });
});
