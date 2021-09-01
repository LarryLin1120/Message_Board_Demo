using System;
using System.Data.SqlClient;
using System.Data;

namespace Test1
{
    public partial class Message_Create : System.Web.UI.Page
    {
        const string ConnectionString = "dbMessageConnectionString";
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Check Status

            string loginName = Request["p1"];
            //Login
            if (!string.IsNullOrWhiteSpace(loginName))
            {
                loginName = loginName.Replace("%2B", "+"); //由於將+號當作URL時會消失，故需而外處理
                Login(loginName);
            }
            else
            {
                Logout();
            }

            #endregion
        }

        protected void btConfirm_Click(object sender, EventArgs e)
        {
            #region Check Account

            string loginName = Request["p1"];
            string loginCode = Request["p2"].Replace("%2B", "+");

            if (!string.IsNullOrWhiteSpace(loginName))
            {
                loginName = loginName.Replace("%2B", "+"); //由於將+號當作URL時會消失，故需而外處理
                Login(loginName);
            }
            else
            {
                Logout();
            }

            #endregion

            #region Check Input

            IniSetting();

            bool isLegal = true;

            if (string.IsNullOrWhiteSpace(tbTitle.Text))
            {
                lbTitle.ForeColor = System.Drawing.Color.Red;
                lbTitle.Font.Bold = true;
                isLegal = false;
            }

            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                lbName.ForeColor = System.Drawing.Color.Red;
                lbName.Font.Bold = true;
                isLegal = false;
            }

            if (string.IsNullOrWhiteSpace(tbContent.Text))
            {
                lbContent.ForeColor = System.Drawing.Color.Red;
                lbContent.Font.Bold = true;
                isLegal = false;
            }

            #endregion

            if (!isLegal)
            {
                lbAlarmMessage.Visible = true;
            }
            else
            {
                try
                {
                    #region Create Article

                    //取得config連接字串資訊
                    string getconfig = System.Web.Configuration.WebConfigurationManager
                    .ConnectionStrings[ConnectionString].ConnectionString;

                    using (SqlConnection connection = new SqlConnection(getconfig))
                    {
                        //要對SQL Server下達的SQL指令並且參數化
                        SqlCommand command = new SqlCommand($"INSERT INTO message_Board( header,name, main) VALUES(@header, @name, @main)", connection);
                        //賦予參數資料型態與值
                        command.Parameters.Add("@header", SqlDbType.NVarChar);
                        command.Parameters["@header"].Value = tbTitle.Text;

                        command.Parameters.Add("@name", SqlDbType.NVarChar);
                        command.Parameters["@name"].Value = tbName.Text;

                        command.Parameters.Add("@main", SqlDbType.NVarChar);
                        command.Parameters["@main"].Value = tbContent.Text;

                        connection.Open();//開啟通道
                        command.ExecuteNonQuery();//執行command的SQL語法，回傳受影響的資料數目
                    }
                    #endregion

                    Response.Redirect("Message_Index.aspx?ac=" + loginCode.Replace("+", "%2B"));
                }
                catch (Exception ex)
                {
                    lbAlarmMessage.Text = "發文失敗，請通知管理員。" + Environment.NewLine + "原因:" + ex.ToString();

                    lbAlarmMessage.Visible = true;
                }

            }
        }

        protected void btHomePage_Click(object sender, EventArgs e)
        {
            // Check Status
            if (btConfirm.Enabled)
            {
                string loginCode = Request["p2"].Replace("+", "%2B");
                Response.Redirect("Message_Index.aspx?ac="+ loginCode);//回首頁
            }
            else
            {
                Response.Redirect("Message_Index.aspx");//回首頁
            }
        }


        private void IniSetting()
        {
            lbAlarmMessage.Text = "紅字為必填欄位未填，請補填，謝謝!";
            lbAlarmMessage.Visible = false;
            lbTitle.ForeColor = System.Drawing.Color.Black;
            lbTitle.Font.Bold = false;
            lbName.ForeColor = System.Drawing.Color.Black;
            lbName.Font.Bold = false;
            lbContent.ForeColor = System.Drawing.Color.Black;
            lbContent.Font.Bold = false;
        }

        private void Login(string name)
        {
            tbName.Text = name;
            tbName.Enabled = false;
            btConfirm.Enabled = true;
            tbTitle.Enabled = true;
            tbContent.Enabled = true;
        }

        private void Logout()
        {
            lbAlarmMessage.Text = "請登入才能發文";
            lbAlarmMessage.Visible = true;
            btConfirm.Enabled = false;
            tbTitle.Enabled = false;
            tbName.Enabled = false;
            tbContent.Enabled = false;
        }
    }
}