<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Message_Index.aspx.cs" Inherits="Test1.Message_Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        </div>
        <asp:Label ID="lbLoginName" runat="server" Text="登入者" Visible="False"></asp:Label>
        <asp:Label ID="lbName" runat="server" Text="LoginName" Visible="False"></asp:Label>
        &nbsp;<asp:Button ID="btLogout" runat="server" OnClick="btLogout_Click" Text="登出" />
        <br />
        <br />
        <asp:Button ID="btPublishArticle" runat="server" Text="我要發文" OnClick="PublishArticle_Click" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btSignUp" runat="server" OnClick="btSignUp_Click" Text="註冊" />
&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btLogin" runat="server" OnClick="btLogin_Click" Text="登入" />
&nbsp;&nbsp;
        <asp:Label ID="lbAccount" runat="server" Text="帳號"></asp:Label>
&nbsp;<asp:TextBox ID="tbAccount" runat="server"></asp:TextBox>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbLoginCode" runat="server" Text="LoginCode" Visible="False"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />
        <br />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lbPassword" runat="server" Text="密碼"></asp:Label>
&nbsp;<asp:TextBox ID="tbPassword" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="lbAlarmMessage" runat="server" ForeColor="Red" Text="登入失敗，請重新輸入" Visible="False"></asp:Label>
        <br />
        <br />
        <asp:GridView ID="gvArticleList" runat="server" AutoGenerateColumns="False" DataKeyNames="Id">
            <Columns>
                <asp:BoundField DataField="Id" HeaderText="編號" InsertVisible="False" ReadOnly="True" SortExpression="Id" />
                <asp:TemplateField HeaderText="主題" SortExpression="Header">
                    <EditItemTemplate>
                        <asp:TextBox ID="tbHeader" runat="server" Text='<%# Bind("Header") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:HyperLink ID="hlHeader" runat="server" NavigateUrl='<%# "Message_Main.aspx?id="+Eval("id")+"&p1=" +Eval("VerifyCode") %>' Text='<%# Eval("header") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Name" HeaderText="發文者" SortExpression="Name" />
                <asp:BoundField DataField="InitDate" HeaderText="發文日期" SortExpression="InitDate" />
                <asp:BoundField DataField="verifyCode" HeaderText="ya" ReadOnly="True" SortExpression="verifyCode" Visible="False" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbMessageConnectionString %>" SelectCommand="SELECT * FROM [message_Board]"></asp:SqlDataSource>
    </form>
</body>
</html>
