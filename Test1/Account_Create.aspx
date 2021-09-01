<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Account_Create.aspx.cs" Inherits="Test1.Account_Create" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server" >
        <br />
        <br />
        <asp:Label ID="lbAccount" runat="server" Text="帳號"></asp:Label>
        &nbsp;
        <asp:TextBox ID="tbAccount" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="lbPassword" runat="server" Text="密碼"></asp:Label>
        &nbsp;
            <asp:TextBox ID="tbPassword" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="lbEmail" runat="server" Text="Email"></asp:Label>
        &nbsp;
        <asp:TextBox ID="tbEmail" runat="server"></asp:TextBox>
        &nbsp;
        <br />
        <br />
        <asp:Label ID="lbAlarmMessage" runat="server" ForeColor="Red" Text="紅字為必填欄位未填，請補填，謝謝!" Visible="False"></asp:Label>
        <br />
        <br />
        <asp:Button ID="btConfirm" runat="server" OnClick="btConfirm_Click" Text="建立帳號" />
        &nbsp;&nbsp;
        <asp:Button ID="btHomePage" runat="server" Text="回首頁" OnClick="btHomePage_Click"/>
    </form>
</body>
</html>
