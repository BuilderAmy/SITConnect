using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect_204826E
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SITConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                }

            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }

        protected void loginbtn_Click(object sender, EventArgs e)
        {
            string em = Session["LoggedIn"].ToString();
            string old = current.Text.ToString().Trim();
            string newPwd = newTB.Text.ToString().Trim();
            checkPassword(newTB.Text);
            
            SHA512Managed hashing = new SHA512Managed();

            string dbHash = getDBHash(em);
            string dbSalt = getDBSalt(em);
            
            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0 && pwdChecker.Text == "")
                {
                    string pwdWithSalt = old + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);
                    if (userHash.Equals(dbHash))
                    {
                        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                        byte[] saltByte = new byte[8];
                        rng.GetBytes(saltByte);
                        salt = Convert.ToBase64String(saltByte);

                        SHA512Managed hashingNew = new SHA512Managed();

                        string newpwdWithSalt = newPwd + salt;
                        byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(newPwd));
                        byte[] newhashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(newpwdWithSalt));

                        finalHash = Convert.ToBase64String(newhashWithSalt);
                        updPwd();
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
        }
        protected void updPwd()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("update Account SET Pwdhash=@Pwdhash, Pwdsalt=@Pwdsalt WHERE Email=@USERID"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            
                            cmd.Parameters.AddWithValue("@USERID", Session["LoggedIn"].ToString());
                            cmd.Parameters.AddWithValue("@Pwdhash", finalHash);
                            cmd.Parameters.AddWithValue("@Pwdsalt", salt);
                            

                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            Response.Redirect("Login.aspx");
        }
        protected string getDBHash(string userid)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Pwdhash FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Pwdhash"] != null)
                        {
                            if (reader["Pwdhash"] != DBNull.Value)
                            {
                                h = reader["Pwdhash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }
        protected string getDBSalt(string userid)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select Pwdsalt FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Pwdsalt"] != null)
                        {
                            if (reader["Pwdsalt"] != DBNull.Value)
                            {
                                s = reader["Pwdsalt"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }
        private void checkPassword(string newpwd)
        {            
            if (newpwd.Length == 0)
            {
                pwdChecker.Text = "Field is required";
                pwdChecker.ForeColor = Color.Red;
            }
            else if (newpwd.Length < 12)
            {
                pwdChecker.Text = "Password length must be at least 12 characters";
                pwdChecker.ForeColor = Color.Red;
            }
            else if (Regex.IsMatch(newpwd, "[a-z]") == false)
            {
                pwdChecker.Text = "Password require at least 1 lowercase";
                pwdChecker.ForeColor = Color.Red;
            }
            else if (Regex.IsMatch(newpwd, "[A-Z]") == false)
            {
                pwdChecker.Text = "Password require at least 1 uppercase";
                pwdChecker.ForeColor = Color.Red;
            }
            else if (Regex.IsMatch(newpwd, "[0-9]") == false)
            {
                pwdChecker.Text = "Password require at least 1 number";
                pwdChecker.ForeColor = Color.Red;
            }
            else if (Regex.IsMatch(newpwd, "[^a-zA-Z0-9]") == false)
            {
                pwdChecker.Text = "Password require at least 1 special character";
                pwdChecker.ForeColor = Color.Red;
            }
            else
            {
                pwdChecker.Text = "";
            }            
        }
    }
}