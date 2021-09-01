<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Account_Enable.aspx.cs" Inherits="Test1.Account_Enable" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <br />
        <br />
        <asp:Label ID="lbVerifyCode" runat="server" Text="驗證碼"></asp:Label>
        &nbsp;
        <asp:TextBox ID="tbVerifyCode" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="lbAlarmMessage" runat="server" ForeColor="Red" Text="驗證碼錯誤，請重新輸入" Visible="False"></asp:Label>
        <br />
        <br />
        <asp:Button ID="btConfirm" runat="server" OnClick="btConfirm_Click" Text="帳號激活" />
        &nbsp;&nbsp;
        </form>
</body>
</html>
