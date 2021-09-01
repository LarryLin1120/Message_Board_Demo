using System;
using System.Data.SqlClient;
using System.Data;



namespace Test1
{
    public partial class Account_Create : System.Web.UI.Page
    {

        const string ConnectionString = "dbMessageConnectionString";
        string getconfig = System.Web.Configuration.WebConfigurationManager
                .ConnectionStrings[ConnectionString].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            IniSetting(); //畫面初始化
        }

        protected void btHomePage_Click(object sender, EventArgs e)
        {
            Response.Redirect("Message_Index.aspx");
        }

        protected void btConfirm_Click(object sender, EventArgs e)
        {
            IniSetting(); //畫面初始化

            if (!string.IsNullOrWhiteSpace(tbAccount.Text) &&
                !string.IsNullOrWhiteSpace(tbPassword.Text) &&
                !string.IsNullOrWhiteSpace(tbEmail.Text))
            {
                bool accountAlreadyExist = false;
                using (SqlConnection connection = new SqlConnection(getconfig))
                {
                    //要對SQL Server下達的SQL指令並且參數化
                    SqlCommand command = new SqlCommand($"SELECT Name FROM Account WHERE Account =@Account", connection);
                    //賦予參數資料型態與值
                    command.Parameters.Add("@Account", SqlDbType.NVarChar);
                    command.Parameters["@Account"].Value = tbAccount.Text;
                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        accountAlreadyExist = true;
                    }
                    reader.Close();
                }

                if (accountAlreadyExist)
                {
                    ExceptionModel("帳號已存在，請重新輸入");
                    return;
                }

                if (!Common.ValidateEmail(tbEmail.Text))
                {
                    ExceptionModel("Email不存在，請重新輸入");
                    lbAlarmMessage.Visible = true;
                    return;
                }
            }
            else
            {
                ExceptionModel("必填欄位未填寫完成，請補填，謝謝!");
                return;
            }

            #region Send Verify Mail

            //由於將+號當作URL時會消失，故需而外處理
            string verifyCode = tbAccount.Text + tbEmail.Text + DateTime.Now.ToString();
            string parameter1 = Common.Encryption(tbAccount.Text).Replace("+", "%2B");
            string parameter2 = Common.Encryption(tbPassword.Text).Replace("+", "%2B");
            string parameter3 = Common.Encryption(tbEmail.Text).Replace("+", "%2B");
            verifyCode = Common.Encryption(verifyCode);

            bool sendResult = Common.SendMailUseGmail(tbEmail.Text,
                "Homework驗證信", "驗證碼為:" + Environment.NewLine + verifyCode);

            #endregion

            if (sendResult)
            {
                #region Create Wait Verify Data

                int affectCount = 0;
                try
                {
                    using (SqlConnection connection = new SqlConnection(getconfig))
                    {
                        //要對SQL Server下達的SQL指令並且參數化
                        SqlCommand command = new SqlCommand($"INSERT INTO Verify_Account(Account,Password, Email,VerifyCode) VALUES(@Account, @Password, @Email, @VerifyCode)", connection);
                        //賦予參數資料型態與值
                        command.Parameters.Add("@Account", SqlDbType.NVarChar);
                        command.Parameters["@Account"].Value = tbAccount.Text;

                        command.Parameters.Add("@Password", SqlDbType.NVarChar);
                        command.Parameters["@Password"].Value = tbPassword.Text;

                        command.Parameters.Add("@Email", SqlDbType.NVarChar);
                        command.Parameters["@Email"].Value = tbEmail.Text;

                        command.Parameters.Add("@VerifyCode", SqlDbType.NVarChar);
                        command.Parameters["@VerifyCode"].Value = verifyCode;

                        connection.Open();
                        affectCount = command.ExecuteNonQuery();//執行command的SQL語法，回傳受影響的資料數目
                    }
                }
                catch (Exception ex)
                {
                    ExceptionModel("新增驗證資料失敗，請通知管理員" + Environment.NewLine + "原因:" + ex.Message);
                }

                #endregion

                if (affectCount > 0)
                {
                    Response.Redirect("Account_Enable.aspx?p1=" + parameter1 + "&p2=" + parameter2 + "&p3=" + parameter3);
                }
                else
                {
                    ExceptionModel("新增驗證資料失敗，請通知管理員");
                }
            }
            else
            {
                ExceptionModel("寄送email失敗請通知管理員");
            }

        }

        private void IniSetting()
        {
            lbAlarmMessage.Visible = false;
        }
        private void ExceptionModel(string message)
        {
            lbAlarmMessage.Text = message;
            lbAlarmMessage.Visible = true;
        }

    }
}