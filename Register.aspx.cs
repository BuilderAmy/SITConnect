using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

using System.Text.RegularExpressions;
using System.Drawing;
using System.Net.Mail;
using System.IO;

namespace SITConnect_204826E
{
    public partial class Register : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SITConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private void checkfName(string fname)
        {
            if (fname.Length == 0)
            {
                fNameChecker.Text = "Field is required";
                fNameChecker.ForeColor = Color.Red;
            }
            else if (Regex.IsMatch(fname, "/[a-zA-Z]/") == false && fname.Length < 3)
            {
                fNameChecker.Text = "First name must have more than 3 characters";
                fNameChecker.ForeColor = Color.Red;
            }
            else
            {
                fNameChecker.Text = "";
            }
        }

        private void checklName(string lname)
        {
            if (lname.Length == 0)
            {
                lNameChecker.Text = "Field is required";
                lNameChecker.ForeColor = Color.Red;
            }
            else if (Regex.IsMatch(lname, "[a-zA-Z]") == false && lname.Length < 3)
            {
                lNameChecker.Text = "Last name must have more than 3 characters";
                lNameChecker.ForeColor = Color.Red;
            }
            else
            {
                lNameChecker.Text = "";
            }
        }

        private void checkccNum(string cc)
        {
            if (cc.Length == 0)
            {
                ccChecker.Text = "Field is required";
                ccChecker.ForeColor = Color.Red;

            }
            else if (Regex.IsMatch(cc, "/[0-9]/") == false && cc.Length != 16)
            {
                ccChecker.Text = "Invalid Credit Card Number";
                ccChecker.ForeColor = Color.Red;

            }
            else
            {
                ccChecker.Text = "";
            }
        }

        private void checkcvc(string cvc)
        {
            if (cvc.Length == 0)
            {
                cvcChecker.Text = "Field is required";
                cvcChecker.ForeColor = Color.Red;
            }
            else if (Regex.IsMatch(cvc, "/[0-9]/") == false && cvc.Length != 3)
            {
                cvcChecker.Text = "Invalid CVC";
                cvcChecker.ForeColor = Color.Red;
            }
            else
            {
                cvcChecker.Text = "";
            }
        }

        private void checkexp(string exp)
        {
            if (exp.Length == 0)
            {
                expChecker.Text = "Field is required";
                expChecker.ForeColor = Color.Red;
            }
            else if (Regex.IsMatch(exp, "/[0-9]/") == false && exp.Length != 5)
            {
                expChecker.Text = "Invalid Exp Date";
                expChecker.ForeColor = Color.Red;
            }
            else
            {
                expChecker.Text = "";
            }
        }

        private void checkem(string em)
        {
            if (em.Length == 0)
            {
                emChecker.Text = "Field is required";
                emChecker.ForeColor = Color.Red;
            }
            else
            {
                try
                {
                    MailAddress m = new MailAddress(em);
                    emChecker.Text = "";
                }
                catch (FormatException)
                {
                    emChecker.Text = "Invalid Email";
                    emChecker.ForeColor = Color.Red;
                }
            }
        }

        private void checkPhoto()
        {
            if (photoUp.HasFile)
            {
                string fileExtwnsion = Path.GetExtension(photoUp.FileName);

                if (fileExtwnsion.ToLower() != ".jpg" && fileExtwnsion.ToLower() != ".png")
                {
                    photoChecker.Text = "Only jpg and png file allowed";
                    photoChecker.ForeColor = Color.Red;
                }
                else
                {
                    int fileSize = photoUp.PostedFile.ContentLength;
                    if (fileSize > 2097152)
                    {
                        photoChecker.Text = "Maximum size 2(MB) exceeded ";
                        photoChecker.ForeColor = Color.Red;
                    }
                    else
                    {
                        photoUp.SaveAs(Server.MapPath("~/images/" + photoUp.FileName));
                        photoChecker.Text = "";
                        photoChecker.ForeColor = Color.Green;
                    }
                }
            }
            else
            {
                photoChecker.Text = "File not uploaded";
                photoChecker.ForeColor = Color.Red;
            }
        }

        private void checkPassword(string password)
        {
            if (password.Length == 0)
            {
                pwdChecker.Text = "Field is required";
                pwdChecker.ForeColor = Color.Red;
            }
            else if (password.Length < 12)
            {
                pwdChecker.Text = "Password length must be at least 12 characters";
                pwdChecker.ForeColor = Color.Red;
            }
            else if (Regex.IsMatch(password, "[a-z]") == false)
            {
                pwdChecker.Text = "Password require at least 1 lowercase";
                pwdChecker.ForeColor = Color.Red;
            }
            else if (Regex.IsMatch(password, "[A-Z]") == false)
            {
                pwdChecker.Text = "Password require at least 1 uppercase";
                pwdChecker.ForeColor = Color.Red;
            }
            else if (Regex.IsMatch(password, "[0-9]") == false)
            {
                pwdChecker.Text = "Password require at least 1 number";
                pwdChecker.ForeColor = Color.Red;
            }
            else if (Regex.IsMatch(password, "[^a-zA-Z0-9]") == false)
            {
                pwdChecker.Text = "Password require at least 1 special character";
                pwdChecker.ForeColor = Color.Red;
            }
            else
            {
                pwdChecker.Text = "";
            }
        }

        protected void regbtn_Click(object sender, EventArgs e)
        {

            checkPassword(pwdTB.Text);
            checkfName(fName.Text);
            checklName(lName.Text);
            checkccNum(cardNo.Text);
            checkexp(expDate.Text);
            checkcvc(cvc.Text);
            checkem(email.Text);
            checkPhoto();

            if ((fNameChecker.Text != "") || (lNameChecker.Text != "") || (ccChecker.Text != "") || (cvcChecker.Text != "")
                || (expChecker.Text != "") || (emChecker.Text != "") || (photoChecker.Text != "") || (pwdChecker.Text != ""))
            {
                return;
            }
            else
            {
                string pwd = pwdTB.Text.ToString().Trim();
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];

                rng.GetBytes(saltByte);
                salt = Convert.ToBase64String(saltByte);

                SHA512Managed hashing = new SHA512Managed();

                string pwdWithSalt = pwd + salt;
                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                finalHash = Convert.ToBase64String(hashWithSalt);

                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;

                createAccount();
            }
        }

        protected void createAccount()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Fname, @Lname, @Ccno, @Ccdate, @Email, @Pwdhash, @Pwdsalt, @Dob, @Photo, @Photofilename, @IV, @Key, @Attempt, @otp, @otpCreatedAt, @lockTime)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Fname", fName.Text.Trim());
                            cmd.Parameters.AddWithValue("@Lname", lName.Text.Trim());
                            cmd.Parameters.AddWithValue("@Ccno", Convert.ToBase64String(encryptData(cardNo.Text.Trim())));
                            cmd.Parameters.AddWithValue("@Ccdate", expDate.Text.Trim());
                            cmd.Parameters.AddWithValue("@Email", email.Text.Trim());
                            cmd.Parameters.AddWithValue("@Pwdhash", finalHash);
                            cmd.Parameters.AddWithValue("@Pwdsalt", salt);
                            cmd.Parameters.AddWithValue("@Dob", dob.Text.Trim());
                            cmd.Parameters.AddWithValue("@Photo", photoUp.FileContent);
                            cmd.Parameters.AddWithValue("@Photofilename", photoUp.FileName);
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@Attempt", 0);
                            cmd.Parameters.AddWithValue("@otp", "0");
                            cmd.Parameters.AddWithValue("@otpCreatedAt", "2022 - 01 - 01 00:00:00");
                            cmd.Parameters.AddWithValue("@lockTime", "2022 - 01 - 01 00:00:00");

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
        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return cipherText;
        }

    }
}