<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Message_Add.aspx.cs" Inherits="Test1.Message_Create" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <br />
        <asp:Label ID="lbTitle" runat="server" Text="標題"></asp:Label>
        <asp:TextBox ID="tbTitle" runat="server"></asp:TextBox>
        <br />
        <p>
            <asp:Label ID="lbName" runat="server" Text="暱稱"></asp:Label>
            <asp:TextBox ID="tbName" runat="server"></asp:TextBox>
        </p>
        <asp:Label ID="lbContent" runat="server" Text="內容"></asp:Label>
        <asp:TextBox ID="tbContent" runat="server" Height="366px" TextMode="MultiLine" Width="517px"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="lbAlarmMessage" runat="server" ForeColor="Red" Text="紅字為必填欄位未填，請補填，謝謝!" Visible="False"></asp:Label>
        <br />
        <br />
        <asp:Button ID="btConfirm" runat="server" OnClick="btConfirm_Click" Text="發文" />
        &nbsp;&nbsp;
        <asp:Button ID="btHomePage" runat="server" Text="回首頁" OnClick="btHomePage_Click" />
    </form>
</body>
</html>
