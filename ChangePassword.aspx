<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="SITConnect_204826E.ChangePassword" ValidateRequest="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Change Password</title>
    <style>
        .auto-style1 {
            width: 703px;
            border: 3px #333333 solid;
        }
    </style>
    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=newTB.ClientID %>').value;
            if (str.length < 12) {
                document.getElementById("pwdChecker").innerHTML = "Password length must be at least 12 characters";
                document.getElementById("pwdChecker").style.color = "Red";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("pwdChecker").innerHTML = "Password require at least 1 number";
                document.getElementById("pwdChecker").style.color = "Red";
                return ("no_number");
            }
            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("pwdChecker").innerHTML = "Password require at least 1 uppercase";
                document.getElementById("pwdChecker").style.color = "Red";
                return ("no_uppercase");
            }
            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("pwdChecker").innerHTML = "Password require at least 1 lowercase";
                document.getElementById("pwdChecker").style.color = "Red";
                return ("no_uppercase");
            }
            else if (str.search(/[^A-Za-z0-9]/) == -1) {
                document.getElementById("pwdChecker").innerHTML = "Password require at least 1 special character";
                document.getElementById("pwdChecker").style.color = "Red";
                return ("no_specialcharacters");
            }
            document.getElementById("pwdChecker").innerHTML = "Excellent!";
            document.getElementById("pwdChecker").style.color = "Green";
        }
    </script>
</head>
<body>
    <form id="form" runat="server">
        <div id="container">
            <h3 align="center">Change Password</h3>
            <table align="center" class="auto-style1">
                <tr>
                    <td>Current Password</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="current" runat="server" Width="545px" Height="25px" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>New Password</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="newTB" runat="server" Width="545px" Height="25px" TextMode="Password" onkeyup="javascript:validate()"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="pwdChecker" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="resetbtn" runat="server" BackColor="Black" Font-Size="14pt" ForeColor="White" Text="Login" Width="100%" OnClick="loginbtn_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
