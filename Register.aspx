<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SITConnect_204826E.Register" ValidateRequest="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Register Now</title>
    <style type="text/css">
        .auto-style1 {
            width: 703px;
            border: 3px #333333 solid;
        }

        .auto-style3 {
            height: 6px;
        }

        .auto-style4 {
            height: 21px;
        }

        .auto-style5 {
            height: 25px;
        }

        .auto-style6 {
            height: 26px;
            width: 347px;
        }

        .auto-style8 {
            height: 26px;
            width: 348px;
        }

        .auto-style12 {
            width: 347px;
        }

        .auto-style13 {
            width: 348px;
        }

        .auto-style14 {
            margin-left: 0px;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            $("#dob").datepicker();
        });
        function fnameValidate() {
            var first = document.getElementById('<%=fName.ClientID %>').value;
            var regx = /^[a-zA-Z ]{3,50}$/;
            if (!regx.test(first)) {
                document.getElementById("fNameChecker").innerHTML = "Invalid or Empty first name";
                document.getElementById("fNameChecker").style.color = "Red";
            } else {
                document.getElementById("fNameChecker").innerHTML = "";
                document.getElementById("fNameChecker").style.color = "";
            }
        }

        function lnameValidate() {
            var last = document.getElementById('<%=lName.ClientID %>').value;
            var regx = /^[a-zA-Z ]{3,50}$/;
            if (!regx.test(last)) {
                document.getElementById("lNameChecker").innerHTML = "Invalid or Empty last name";
                document.getElementById("lNameChecker").style.color = "Red";
            } else {
                document.getElementById("lNameChecker").innerHTML = "";
                document.getElementById("lNameChecker").style.color = "";
            }
        }

        function ccValidate() {
            var cc = document.getElementById('<%=cardNo.ClientID %>').value;
            var regx = /^5[1-5][0-9]{14}$|^2(?:2(?:2[1-9]|[3-9][0-9])|[3-6][0-9][0-9]|7(?:[01][0-9]|20))[0-9]{12}$/;
            if (!regx.test(cc)) {
                document.getElementById("ccChecker").innerHTML = "Invalid or Empty Credit Card Number";
                document.getElementById("ccChecker").style.color = "Red";
            } else {
                document.getElementById("ccChecker").innerHTML = "";
                document.getElementById("ccChecker").style.color = "";
            }
        }

        function expValidate() {
            var expD = document.getElementById('<%=expDate.ClientID %>').value;
            var regx = /^(0[1-9]|1[0-2])\/?([0-9]{2})$/;
            if (!regx.test(expD)) {
                document.getElementById("expChecker").innerHTML = "Invalid or Empty exp date";
                document.getElementById("expChecker").style.color = "Red";
            } else {
                document.getElementById("expChecker").innerHTML = "";
                document.getElementById("expChecker").style.color = "";
            }
        }

        function cvcValidate() {
            var cvv = document.getElementById('<%=cvc.ClientID %>').value;
            var regx = /^[0-9]{3,4}$/;
            if (!regx.test(cvv)) {
                document.getElementById("cvcChecker").innerHTML = "Invalid or Empty CVC";
                document.getElementById("cvcChecker").style.color = "Red";
            } else {
                document.getElementById("cvcChecker").innerHTML = "";
                document.getElementById("cvcChecker").style.color = "";
            }
        }

        function emValidate() {
            var em = document.getElementById('<%=email.ClientID %>').value;
            var regx = /^\w+[\+\.\w-]*@([\w-]+\.)*\w+[\w-]*\.([a-z]{2,4}|\d+)$/i;
            if (!regx.test(em)) {
                document.getElementById("emChecker").innerHTML = "Invalid or Empty email";
                document.getElementById("emChecker").style.color = "Red";
            }
            else {
                document.getElementById("emChecker").innerHTML = "";
                document.getElementById("emChecker").style.color = "";
            }
        }

        function validate() {
            var str = document.getElementById('<%=pwdTB.ClientID %>').value;
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
    <form id="regForm" runat="server">
        <div id="container">
            <h3 align="center">Sign Up Now</h3>
            <table align="center" class="auto-style1">
                <tr>
                    <td class="auto-style6">First Name</td>
                    <td class="auto-style8">Last Name</td>
                </tr>
                <tr>
                    <td class="auto-style12">
                        <asp:TextBox ID="fName" runat="server" Width="200px" Height="25px" onkeyup="javascript:fnameValidate()"></asp:TextBox>
                    </td>
                    <td class="auto-style13">
                        <asp:TextBox ID="lName" runat="server" Width="200px" CssClass="auto-style14" Height="25px" onkeyup="javascript:lnameValidate()"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style12">
                        <asp:Label ID="fNameChecker" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style12">
                        <asp:Label ID="lNameChecker" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Card Number</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="cardNo" runat="server" Width="545px" Height="25px" onkeyup="javascript:ccValidate()"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="ccChecker" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style12">Expiration Date</td>
                    <td class="auto-style13">CVV2 / CV2</td>
                </tr>
                <tr>
                    <td class="auto-style12">
                        <asp:TextBox ID="expDate" runat="server" Height="25px" onkeyup="javascript:expValidate()"></asp:TextBox>
                    </td>
                    <td class="auto-style13">
                        <asp:TextBox ID="cvc" runat="server" Height="25px" onkeyup="javascript:cvcValidate()"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style12">
                        <asp:Label ID="expChecker" runat="server"></asp:Label>
                    </td>
                    <td class="auto-style13">
                        <asp:Label ID="cvcChecker" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="auto-style3" colspan="2">Email Address</td>
                </tr>
                <tr>
                    <td class="auto-style4" colspan="2">
                        <asp:TextBox ID="email" runat="server" Width="332px" Height="25px" onkeyup="javascript:emValidate()"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="emChecker" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Password</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="pwdTB" runat="server" Width="329px" Height="25px" onkeyup="javascript:validate()" TextMode="Password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="pwdChecker" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Date of Birth</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="dob" runat="server" Height="25px" TextMode="Date"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="dobChecker" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">Photo</td>
                </tr>
                <tr>
                    <td class="auto-style5" colspan="2">
                        <asp:FileUpload ID="photoUp" runat="server" Height="25px"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="photoChecker" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="regbtn" runat="server" BackColor="Black" Font-Size="14pt" ForeColor="White" Text="Sign Up" Width="100%" OnClick="regbtn_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
