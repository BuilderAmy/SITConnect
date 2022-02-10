<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="SITConnect_204826E.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form" runat="server">
        <div>
            <fieldset>
                <legend>Homepage</legend>
                <br />
                <asp:Label ID="lblMessage" runat="server" EnableViewState="false"></asp:Label>
                <br />
                <br />
                <p><a href="/ChangePassword.aspx">Forgot your password?</a></p>
                <br />
                <asp:Button ID="logoutBtn" runat="server" Text="Logout" Visible="false" BackColor="Red" Font-Size="14pt" ForeColor="White" Width="100%" OnClick="logoutBtn_Click" />
                <p />
            </fieldset>
        </div>
    </form>
</body>
</html>
