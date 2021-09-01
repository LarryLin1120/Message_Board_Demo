<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Message_Main.aspx.cs" Inherits="Test1.Message_Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server" >
        <div>
            主題:<asp:Label ID="lbMessage_header" runat="server" Text=""></asp:Label>
            <br />
            發文人:<asp:Label ID="lbMessage_Name" runat="server" Text=""></asp:Label>
            <br />
            內容<br />
            <asp:Label ID="lbContent" runat="server" Text=""></asp:Label>
            <br />
            <br />
            發表時間:<asp:Label ID="lbMessage_Time" runat="server" Text=""></asp:Label>
            <br />
            <br />
        </div>
        <hr size="3" />
        <asp:Panel ID="plReply" runat="server" BorderColor="Black">
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <asp:Label ID="lbReplyName" runat="server" Text="回覆者:"></asp:Label>
                    <asp:Label ID="lbName" runat="server" Text='<%# Bind("name") %>'></asp:Label>
                    <br>
                    <asp:Label ID="lbContent" runat="server" Text='<%# Bind("main") %>'></asp:Label>
                    <br>
                    <asp:Label ID="lbReplydate" runat="server" Text="回覆時間:"></asp:Label>
                    <asp:Label ID="lbDate" runat="server" Text='<%# Bind("initdate") %>'></asp:Label>
                    <hr>
                </ItemTemplate>
            </asp:Repeater>
        </asp:Panel>
        <div>
            <p>
                <asp:Label ID="lb1ReplyName" runat="server" Text="暱稱"></asp:Label>
                <asp:TextBox ID="tbReplyName" runat="server"></asp:TextBox>
            </p>
            <asp:Label ID="lb1ReplyContent" runat="server" Text="內容"></asp:Label>
            <asp:TextBox ID="tbReplyContent" runat="server" Height="200px" TextMode="MultiLine" Width="928px"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="lbMessage" runat="server" ForeColor="Red" Text="紅字為必填欄位未填，請補填，謝謝!" Visible="False"></asp:Label>
            <br />
            <br />
            <asp:Button ID="btReply" runat="server" Text="我要回覆" OnClick="btReply_Click" />
            &nbsp;&nbsp;
            <asp:Button ID="btHomepage" runat="server" Text="回首頁" OnClick="Homepage_Click" />
        </div>
    </form>

</body>
</html>
