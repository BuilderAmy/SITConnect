using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Drawing;
using System.Net.Mail;
using System.Data;

namespace SITConnect_204826E
{
    public class MyObject
    {
        public string success { get; set; }
        public List<string> ErrorMessage { get; set; }
    }
    public partial class Login : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SITConnection"].ConnectionString;
        DateTime StartTime;
        int count = 0;
        public string pin;       

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void loginbtn_Click(object sender, EventArgs e)
        {

            if (ValidateCaptcha())
            {
                string emailid = emailTB.Text.ToString().Trim();
                string pwd = pwdTB.Text.ToString().Trim();
                getCount(emailid);
                getTime(emailid);

                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(emailid);
                string dbSalt = getDBSalt(emailid);

                TimeSpan span = (DateTime.Now).Subtract(StartTime);
                Int32 minLock = Convert.ToInt32(span.TotalMinutes);
                Int32 minLeft = 5 - minLock;

                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);

                        if (userHash.Equals(dbHash) && minLeft <= 0)
                        {
                            resetTime(emailid);
                            resetCount(emailid);
                            lChecker.Text = "";
                            Session["LoggedIn"] = emailid;
                            string guid = Guid.NewGuid().ToString();
                            Session["AuthToken"] = guid;
                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                            Response.Redirect("Verification.aspx", false);


                            Random random = new Random();
                            pin = random.Next(000000, 999999).ToString();
                            createOtp(emailid, pin);
                            sendOtp(pin);
                        }
                        else
                        {
                            count++;
                            updateCount(emailid, count);
                            if (count == 1)
                            {
                                lChecker.ForeColor = Color.Red;
                                lChecker.Text = "Invalid email or password entered! 2 tries left.";
                            }
                            else if (count == 2)
                            {
                                lChecker.ForeColor = Color.Red;
                                lChecker.Text = "Invalid email or password entered! 1 try left.";
                            }
                            else if (count == 3)
                            {
                                setTime(emailid, DateTime.Now);
                                lChecker.ForeColor = Color.Red;
                                lChecker.Text = "Account has been locked due to multiple failed attempts.";
                            }

                            else if (minLeft > 0)
                            {
                                lChecker.ForeColor = Color.Red;
                                lChecker.Text = "Account has been locked. Please try again later!";
                            }

                        }

                    }

                }
                catch (Exception ex)
                {

                    throw new Exception(ex.ToString());
                }
                finally { }
            }
            else
            {
                Response.Redirect("Login.aspx");
            }

        }

        protected int getCount(string emailid)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string selectsql = "select [Attempt] FROM Account WHERE [Email]=@USERID";

            SqlCommand selcommand = new SqlCommand(selectsql, connection);
            selcommand.Parameters.AddWithValue("@USERID", emailid);

            try
            {
                connection.Open();
                using (SqlDataReader reader = selcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Attempt"] != null)
                        {
                            if (reader["Attempt"] != DBNull.Value)
                            {
                                count = (int)reader["Attempt"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return count;
        }
        protected string getTime(string emailid)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string retrievesql = "select [lockTime] FROM Account WHERE [Email]=@USERID";
            SqlCommand retcommand = new SqlCommand(retrievesql, connection);
            retcommand.Parameters.AddWithValue("@USERID", emailid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = retcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["lockTime"] != DBNull.Value)
                        {
                            StartTime = (DateTime)reader["lockTime"];
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return h;
        }
        protected void setTime(string emailid, DateTime timer)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string utsql = "update Account SET [lockTime]=@TIME WHERE [Email]=@USERID";
            SqlCommand utcommand = new SqlCommand(utsql, connection);
            utcommand.CommandType = CommandType.Text;
            utcommand.Parameters.AddWithValue("@USERID", emailid);
            utcommand.Parameters.AddWithValue("@TIME", timer);
            try
            {
                connection.Open();
                utcommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
        protected void updateCount(string emailid, int count)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string updatesql = "update Account SET [Attempt]=@COUNT WHERE [Email]=@USERID";

            SqlCommand updcommand = new SqlCommand(updatesql, connection);
            updcommand.Parameters.AddWithValue("@USERID", emailid);
            updcommand.Parameters.AddWithValue("@COUNT", count);
            try
            {
                connection.Open();
                updcommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
        protected string resetTime(string emailid)
        {
            string r = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string updatesql = "update Account SET [lockTime]=@TIME WHERE [Email]=@USERID";
            SqlCommand updcommand = new SqlCommand(updatesql, connection);
            updcommand.Parameters.AddWithValue("@USERID", emailid);
            updcommand.Parameters.AddWithValue("@TIME", "2001 - 01 - 21 00:00:00");
            try
            {
                connection.Open();
                updcommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return r;
        }
        protected int resetCount(string emailid)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string updatesql = "update Account SET [Attempt]=@COUNT WHERE [Email]=@USERID";
            SqlCommand updcommand = new SqlCommand(updatesql, connection);
            updcommand.Parameters.AddWithValue("@USERID", emailid);
            updcommand.Parameters.AddWithValue("@COUNT", 0);
            try
            {
                connection.Open();
                updcommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
            return count;
        }

        protected string createOtp(string emailid, string pin)
        {
            string ot = null;
            SqlConnection con = new SqlConnection(MYDBConnectionString);
            string otpsql = "update Account set [otp] = @OTP where [Email] = @USERID";
            SqlCommand otpcommand = new SqlCommand(otpsql, con);
            otpcommand.Parameters.AddWithValue("@USERID", emailid);
            otpcommand.Parameters.AddWithValue("@OTP", pin);

            try
            {
                con.Open();
                using (SqlDataReader reader = otpcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["otp"] != null)
                        {
                            ot = reader["otp"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { con.Close(); }
            return ot;
        }

        protected string sendOtp(string otp)
        {
            string fromaddress = "SITConnect <herobrineblaze@gmail.com>";
            string str = null;
            string s = "herobrineblaze@gmail.com";
            string p = "steveblaze";
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(s, p),
                EnableSsl = true
            };
            var mailMessage = new MailMessage
            {
                Subject = "SIT Connect OTP",
                Body = "Dear " + emailTB.Text.ToString() + ", your verification code is: " + otp
            };
            mailMessage.To.Add(emailTB.Text.ToString());
            mailMessage.From = new MailAddress(fromaddress);
            try
            {
                smtpClient.Send(mailMessage);
                return str;
            }
            catch
            {
                throw;
            }
        }
        public bool ValidateCaptcha()
        {
            bool result = true;
            string captchaResponse = Request.Form["g-recaptcha-response"];
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create("https://www.google.com/recaptcha/api/siteverify?secret=6Le1K2UeAAAAAKyLNc6MR03lxCfNLU5dsdcKRiAu &response=" + captchaResponse);
            try
            {
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        string jsonResponse = readStream.ReadToEnd();
                        JavaScriptSerializer js = new JavaScriptSerializer();
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);
                        result = Convert.ToBoolean(jsonObject.success);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                throw new Exception(ex.ToString());
            }
        }
        protected string getDBHash(string userid)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select [Pwdhash] FROM Account WHERE [Email]=@USERID";
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
            string sql = "select [Pwdsalt] FROM ACCOUNT WHERE [Email]=@USERID";
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
        

        protected void registerBtn_Click(object sender, EventArgs e)
        {
            Response.Redirect("Register.aspx");
        }
    }
}
