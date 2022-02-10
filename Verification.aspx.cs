using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect_204826E
{
    public partial class Verification : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["SITConnection"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (Session["LoggedIn"] != null)
                {

                }


                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }

        }
        protected string retrieveOTP(string emailid)
        {
            string pin = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select otp from Account where Email = @USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", emailid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pin = reader["otp"].ToString();
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
            return pin;
        }
        protected string getOTP(string emailid)
        {
            string pin = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string otpsql = "select otp from Account where Email = @USERID";
            SqlCommand otpcommand = new SqlCommand(otpsql,  connection);
            otpcommand.Parameters.AddWithValue("@USERID", emailid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = otpcommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pin = reader["otp"].ToString();
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
            return pin;
        }

        protected void verifybtn_Click(object sender, EventArgs e)
        {
            if (vcTB.Text.ToString() == getOTP(Session["LoggedIn"].ToString()))
            {
                Response.Redirect("Index.aspx", false);
            }
            else
            {
                vChecker.ForeColor = Color.Green;
                vChecker.Text = "Verification code is wrong, please re-enter.";
            }

        }
    }
}