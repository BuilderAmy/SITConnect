<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITConnect_204826E.Login" ValidateRequest="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Page</title>
    <script src="https://www.google.com/recaptcha/api.js?render=6Le1K2UeAAAAABeDnNWHx_OG3DtP6pZHmBHs97Hl"></script>
    <style type="text/css">
        .auto-style1 {
            width: 703px;
            border: 3px #333333 solid;
        }
    </style>
</head>
<body>
    <form id="loginform" runat="server">
        <div id="container">
            <h3 align="center">Login</h3>
            <table align="center" class="auto-style1">
                <tr>
                    <td>Email Address</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="emailTB" runat="server" Width="545px" Height="25px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>Password</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="pwdTB" runat="server" Width="545px" Height="25px" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td>
                        <asp:Label ID="lChecker" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="loginbtn" runat="server" BackColor="Black" Font-Size="14pt" ForeColor="White" Text="Login" Width="100%" OnClick="loginbtn_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="registerBtn" runat="server" BackColor="White" Font-Size="14pt" ForeColor="Black" Text="Sign Up" Width="100%" OnClick="registerBtn_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
    </form>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6Le1K2UeAAAAABeDnNWHx_OG3DtP6pZHmBHs97Hl', { action: 'login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
       
    </script>
</body>
</html>
