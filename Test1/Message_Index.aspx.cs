using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

namespace Test1
{
    public partial class Message_Index : System.Web.UI.Page
    {
        const string ConnectionString = "dbMessageConnectionString";

        string config =
            System.Web.Configuration.WebConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString; //從config找到資料庫位置

        protected void Page_Load(object sender, EventArgs e)
        {
            IniSetting(); //初始化畫面

            try
            {
                using (SqlConnection connection = new SqlConnection(config))
                {
                    #region Get Login Info

                    string loginCode = Request["ac"]; //取得loginCode
                    string loginName = string.Empty; //登入者名稱

                    SqlCommand command;

                    if (!string.IsNullOrWhiteSpace(loginCode))
                    {
                        loginCode = loginCode.Replace("%2B", "+"); //由於將+號當作URL時會消失，故需而外處理

                        lbLoginCode.Text = loginCode;

                        command = new SqlCommand($"SELECT Name FROM Account WHERE LoginCode =@LoginCode", connection);

                        command.Parameters.Add("@LoginCode", SqlDbType.NVarChar);
                        command.Parameters["@LoginCode"].Value = loginCode;

                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            loginName = reader["Name"].ToString();
                            lbName.Text = loginName;
                            LoginModel();
                        }

                        reader.Close();
                    }

                    #endregion

                    #region Get Article Content


                    if (!string.IsNullOrWhiteSpace(loginCode))
                    {
                        loginCode = loginCode.Replace("+", "%2B");
                    }

                    //把loginCode存入欄位(為了存入Hyplink的URL中)
                    command = new SqlCommand(@"SELECT Id, Header, Name, InitDate, '" + loginCode + @"' as VerifyCode 
                                        FROM Message_board ORDER BY ID ASC", connection);


                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);//取得command資料

                    DataSet dataset = new DataSet();//創立一個dataset的記憶體資料庫
                    dataAdapter.Fill(dataset);//將上面抓到的資料存入dataset內
                    gvArticleList.DataSource = dataset;//DataSource的資料來源是dataset or datatable
                    gvArticleList.DataBind();//資料與欄位合在一起

                    #endregion
                }
            }
            catch (Exception ex)
            {
                ExceptionModel(ex.Message); //異常模式
            }
        }

        protected void PublishArticle_Click(object sender, EventArgs e)
        {
            //檢驗是否已登入，若已登入則將LoginCode傳入
            if (lbLoginName.Visible)
            {
                Response.Redirect("Message_Add.aspx?p1=" + lbName.Text.Replace("+", "%2B") + "&p2=" + lbLoginCode.Text.Replace("+", "%2B"));
            }
            else
            {
                Response.Redirect("Message_Add.aspx");
            }
        }

        protected void btSignUp_Click(object sender, EventArgs e)
        {
            Response.Redirect("Account_Create.aspx");
        }

        protected void btLogin_Click(object sender, EventArgs e)
        {
            IniSetting(); //初始化

            #region Verify Account & Password

            if (!string.IsNullOrWhiteSpace(tbAccount.Text)
                && !string.IsNullOrWhiteSpace(tbPassword.Text))
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(config))
                    {
                        bool getAccountInfo = false;
                        SqlCommand command = new SqlCommand($"SELECT Name FROM Account WHERE Account =@Account AND Password =@Password", connection);

                        command.Parameters.Add("@Account", SqlDbType.NVarChar);
                        command.Parameters["@Account"].Value = tbAccount.Text;
                        command.Parameters.Add("@Password", SqlDbType.NVarChar);
                        command.Parameters["@Password"].Value = tbPassword.Text;
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            getAccountInfo = true;
                        }
                        reader.Close();

                        #region  Login Success

                        //Login Success
                        if (getAccountInfo)
                        {
                            string verifyCode = tbAccount.Text + tbPassword.Text + DateTime.Now.ToString();
                            verifyCode = Common.Encryption(verifyCode);
                            command = new SqlCommand($"UPDATE Account SET LoginCode =@LoginCode" +
                                $" WHERE Account=@Account AND Password=@Password", connection);
                            //賦予參數資料型態與值
                            command.Parameters.Add("@Account", SqlDbType.NVarChar);
                            command.Parameters["@Account"].Value = tbAccount.Text;

                            command.Parameters.Add("@Password", SqlDbType.NVarChar);
                            command.Parameters["@Password"].Value = tbPassword.Text;

                            command.Parameters.Add("@LoginCode", SqlDbType.NVarChar);
                            command.Parameters["@LoginCode"].Value = verifyCode;

                            //執行command的SQL語法，回傳受影響的資料數目
                            if (command.ExecuteNonQuery() > 0)
                            {
                                Response.Redirect("Message_Index.aspx?ac=" + verifyCode.Replace("+", "%2B"));
                            }
                            else
                            {
                                LoginFailModel("登入失敗，請重新登入");
                            }
                        }
                        else
                        {
                            LoginFailModel("登入失敗，請重新登入");

                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    ExceptionModel(ex.Message);
                }
            }
            else
            {
                LoginFailModel("帳號密碼皆不可為空，請重新輸入");
            }

            #endregion
        }


        private void IniSetting()
        {
            lbAlarmMessage.Visible = false;
            btLogin.Visible = true;
            lbAccount.Visible = true;
            tbAccount.Visible = true;
            lbPassword.Visible = true;
            tbPassword.Visible = true;
            btSignUp.Visible = true;
            lbLoginName.Visible = false;
            btLogout.Visible = false;
            lbName.Visible = false;
        }

        private void LoginModel()
        {
            lbAlarmMessage.Visible = false;
            btLogin.Visible = false;
            lbAccount.Visible = false;
            tbAccount.Visible = false;
            lbPassword.Visible = false;
            tbPassword.Visible = false;
            btSignUp.Visible = false;
            lbLoginName.Visible = true;
            lbName.Visible = true;
            btLogout.Visible = true;
        }

        private void ExceptionModel(string message)
        {
            lbAlarmMessage.Text = "系統異常，請通知管理者" + Environment.NewLine + "原因:" + message;
            lbAlarmMessage.Visible = true;
            btPublishArticle.Enabled = false;
            btSignUp.Enabled = false;
            btLogin.Enabled = false;
        }

        private void LoginFailModel(string message)
        {
            lbAlarmMessage.Text = message;
            lbAlarmMessage.Visible = true;
        }

        protected void btLogout_Click(object sender, EventArgs e)
        {
            Response.Redirect("Message_Index.aspx");
        }
    }
}