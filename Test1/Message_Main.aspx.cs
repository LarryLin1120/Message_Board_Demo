using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace Test1
{
    public partial class Message_Main : System.Web.UI.Page
    {
        const string ConnectionString = "dbMessageConnectionString";
        const int DisplayReplyCount = 2;
        const int DisplayReplyHeight = 240;

        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request["id"];
            string verifyCode = Request["p1"];

            if (id == null)
            {
                ExceptionModel("非系統正常操作，請回首頁重新操作，謝謝!");
            }
            else
            {
                string config = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString;
                using (SqlConnection connection = new SqlConnection(config))
                {
                    #region Get Article Content

                    SqlCommand command = new SqlCommand($"SELECT id, header, name, main, initDate FROM message_board where(id =@id)", connection);
                    command.Parameters.Add("@id", SqlDbType.NVarChar); //設定參數資料型態
                    command.Parameters["@id"].Value = id; //賦予參數值
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        lbMessage_header.Text = reader["header"].ToString();
                        lbMessage_Name.Text = reader["name"].ToString();
                        lbMessage_Time.Text = reader["initDate"].ToString();
                        lbContent.Text = reader["main"].ToString();
                    }
                    reader.Close();

                    #endregion

                    #region Get Reply

                    command = new SqlCommand($"SELECT Name, Main, InitDate FROM Reply WHERE messageID =@id", connection);

                    command.Parameters.Add("@id", SqlDbType.NVarChar); //設定參數資料型態
                    command.Parameters["@id"].Value = Request.QueryString["id"];

                    reader = command.ExecuteReader();
                    Repeater1.DataSource = reader;//repeater的資料來源是從rereader來
                    Repeater1.DataBind();//執行繫結
                    plReply.Controls.Add(Repeater1);
                    reader.Close();

                    plReply.Height = DisplayReplyHeight;
                    plReply.ScrollBars = ScrollBars.Vertical;

                    #endregion

                    bool isLogin = false;
                    if (verifyCode == null)
                    {
                        LogoutModel();
                        return;
                    }
                    else
                    {
                        command = new SqlCommand($"SELECT Name FROM Account WHERE LoginCode =@LoginCode", connection);
                        command.Parameters.Add("@LoginCode", SqlDbType.NVarChar);
                        command.Parameters["@LoginCode"].Value = verifyCode.Replace("%2B", "+");

                        reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            LoginModel(reader["Name"].ToString());
                            isLogin = true;
                        }

                        if (!isLogin)
                        {
                            LogoutModel();
                            return;
                        }

                        reader.Close();

                    }
                }


            }
        }

        protected void Homepage_Click(object sender, EventArgs e)
        {
            #region Check Status

            //Login
            if (btReply.Visible)
            {
                string verifyCode = Request["p1"];
                if (string.IsNullOrWhiteSpace(verifyCode))
                {
                    Response.Redirect("Message_Index.aspx");
                }
                else
                {
                    Response.Redirect("Message_Index.aspx?ac=" + verifyCode.Replace("+", "%2B"));
                }
            }
            else
            {
                Response.Redirect("Message_Index.aspx");
            }

            #endregion
        }

        protected void btReply_Click(object sender, EventArgs e)
        {
            string id = Request["id"];
            string verifyCode = Request["p1"];

            #region Check Input

            bool islegal = true;
            if (string.IsNullOrWhiteSpace(tbReplyName.Text))
            {
                islegal = false;
                lb1ReplyName.ForeColor = System.Drawing.Color.Red;
                lb1ReplyName.Font.Bold = true;
            }

            if (string.IsNullOrWhiteSpace(tbReplyContent.Text))
            {
                islegal = false;
                lb1ReplyContent.ForeColor = System.Drawing.Color.Red;
                lb1ReplyContent.Font.Bold = true;
            }

            #endregion

            int affectCount = 0;

            if (!islegal)
            {
                lbMessage.Visible = true;
            }
            else
            {
                #region Insert Data

                IniSetting();

                string config = System.Web.Configuration.WebConfigurationManager
                    .ConnectionStrings[ConnectionString].ConnectionString; //取得config的.ConnectionStrings資料

                try
                {
                    using (SqlConnection connection = new SqlConnection(config))
                    {
                        SqlCommand command = new SqlCommand($"INSERT INTO Reply(name,main,messageID) VALUES(@name, @main,@id)", connection);

                        //參數化的資料型態與值
                        command.Parameters.Add("@name", SqlDbType.NVarChar);
                        command.Parameters["@name"].Value = tbReplyName.Text;
                        command.Parameters.Add("@main", SqlDbType.NVarChar);
                        command.Parameters["@main"].Value = tbReplyContent.Text;
                        command.Parameters.Add("@id", SqlDbType.Int);
                        command.Parameters["@id"].Value = Convert.ToInt32(id);//將取得的值轉成int

                        connection.Open(); //開啟通道
                        affectCount = command.ExecuteNonQuery(); //執行SQL並回傳影響的項目
                    }

                    if (affectCount > 0)
                    {
                        //跳轉至回應的留言內容區(用id來抓取)
                        Response.Redirect("Message_Main.aspx?id=" + id + "&p1=" + verifyCode.Replace("+", "%2B"));
                    }
                    else
                    {
                        ExceptionModel("回應文章失敗，請通知管理員");
                    }
                }
                catch (Exception ex)
                {
                    ExceptionModel(ex.Message);
                }

                #endregion
            }

        }

        private void IniSetting()
        {
            lbMessage.Text = "紅字為必填欄位未填，請補填，謝謝!";
            lbMessage.Visible = false;
            lb1ReplyContent.ForeColor = System.Drawing.Color.Black;
            lb1ReplyContent.Font.Bold = false;
            lb1ReplyName.ForeColor = System.Drawing.Color.Black;
            lb1ReplyName.Font.Bold = false;
        }

        private void ExceptionModel(string message)
        {
            lbMessage.Text = message;
            lbMessage.Visible = true;
            btReply.Enabled = false;
            tbReplyName.Enabled = false;
         }

        private void LoginModel(string name)
        {
            tbReplyName.Text = name;
            tbReplyName.Enabled = false;
            tbReplyContent.Enabled = true;
            btReply.Enabled = true;
        }
        private void LogoutModel()
        {
            btReply.Enabled = false;
            tbReplyName.Enabled = false;
            tbReplyContent.Enabled = false;
            lbMessage.Text = "尚未登入，請先登入";
            lbMessage.Visible = true;
        }

    }
}