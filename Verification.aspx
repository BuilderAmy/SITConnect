<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verification.aspx.cs" Inherits="SITConnect_204826E.Verification"  ValidateRequest="true"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Verification Code</title>
</head>
<body>
    <form id="otpform" runat="server">
        <div id="container">
            <h3 align="center">Verify your identity</h3>
            <table align="center" class="auto-style1">
                <tr>
                    <td>Verification Code</td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="vcTB" runat="server" Width="545px" Height="25px"></asp:TextBox>
                    </td>
                </tr>                
                <tr>
                    <td>
                        <asp:Label ID="vChecker" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="verifybtn" runat="server" BackColor="Black" Font-Size="14pt" ForeColor="White" Text="Verify" Width="100%" OnClick="verifybtn_Click"/>
                    </td>
                </tr>                
            </table>
        </div>
    </form>
</body>
</html>
