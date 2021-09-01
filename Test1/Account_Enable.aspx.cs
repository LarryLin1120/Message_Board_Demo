using System;
using System.Data.SqlClient;
using System.Data;
using System.Transactions;


namespace Test1
{
    public partial class Account_Enable : System.Web.UI.Page
    {
        const string ConnectionString = "dbMessageConnectionString";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btConfirm_Click(object sender, EventArgs e)
        {
            IniSetting();

            if (string.IsNullOrWhiteSpace(tbVerifyCode.Text))
            {
                ExceptionModel("驗證碼錯誤，請重新輸入");
                return;
            }

            //由於將+號當作URL時會消失，故需而外處理
            string inputAccount = Request["p1"].Replace("%2B", "+");
            string inputPassword = Request["p2"].Replace("%2B", "+");
            string inputEmail = Request["p3"].Replace("%2B", "+");
            string account = string.Empty;
            string password = string.Empty;
            string email = string.Empty;
            string verifyDataId = string.Empty;
            string config = System.Web.Configuration.WebConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString;
            bool isComplete = true;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection connection = new SqlConnection(config))
                    {
                        #region Get Wait Verify Data
                        SqlCommand command = new SqlCommand($"SELECT Id, Account, Password, Email FROM Verify_Account " +
                                            $"WHERE (VerifyCode =@code) AND (DATEDIFF(minute,GETDATE(), InitDate) < 4)", connection);

                        command.Parameters.Add("@code", SqlDbType.NVarChar);
                        command.Parameters["@code"].Value = tbVerifyCode.Text;
                        connection.Open();

                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            verifyDataId = reader["Id"].ToString();
                            account = reader["Account"].ToString();
                            password = reader["Password"].ToString();
                            email = reader["Email"].ToString();
                        }
                        reader.Close();
                        #endregion

                        #region Validating Data
                        if (inputAccount.Equals(Common.Encryption(account)) &&
                            inputPassword.Equals(Common.Encryption(password)) &&
                            inputEmail.Equals(Common.Encryption(email)))
                        {
                            command = new SqlCommand($"INSERT INTO Account(Account,Password, Name,Email,Enable,Administrator) " +
                                     $"VALUES(@Account, @Password, @Name, @Email, @Enable, @Administrator)", connection);
                            //賦予參數資料型態與值
                            command.Parameters.Add("@Account", SqlDbType.NVarChar);
                            command.Parameters["@Account"].Value = account;

                            command.Parameters.Add("@Password", SqlDbType.NVarChar);
                            command.Parameters["@Password"].Value = password;

                            command.Parameters.Add("@Name", SqlDbType.NVarChar);
                            command.Parameters["@Name"].Value = account;

                            command.Parameters.Add("@Email", SqlDbType.NVarChar);
                            command.Parameters["@Email"].Value = email;

                            command.Parameters.Add("@Enable", SqlDbType.Bit);
                            command.Parameters["@Enable"].Value = true;

                            command.Parameters.Add("@Administrator", SqlDbType.Bit);
                            command.Parameters["@Administrator"].Value = false;


                            //執行command的SQL語法，回傳受影響的資料數目
                            if (command.ExecuteNonQuery() > 0)
                            {
                                #region Delete Verify Data

                                command = new SqlCommand($"DELETE FROM Verify_Account WHERE (Id =@Id)", connection);

                                //賦予參數資料型態與值
                                command.Parameters.Add("@Id", SqlDbType.Int);
                                command.Parameters["@Id"].Value = Convert.ToInt32(verifyDataId);

                                if (command.ExecuteNonQuery() != 1)
                                {
                                    isComplete = false; //Delete Verify_Account Fail 
                                }

                                #endregion
                            }
                            else
                            {
                                isComplete = false; //Insert Account Fail 
                            }

                            //Create Account Successful , Back Homepage
                            if (isComplete)
                            {
                                scope.Complete();
                                Response.Redirect("Message_Index.aspx");
                            }
                            else
                            {
                                ExceptionModel("建置帳號錯誤，請通知管理員");
                            }

                        }
                        else 
                        {
                            ExceptionModel("爛死了重新輸入好嗎");
                        }

                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionModel("資料庫異常，請通知管理員" + Environment.NewLine + ex.Message);
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
