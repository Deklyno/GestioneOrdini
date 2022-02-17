using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;


namespace GestioneOrdini
{

    public class Login_Entity
    {
        private string u, p;
        public string UserName {
            get {
                return this.u;
            }
            set {
                this.u = value.Replace("'", "\'").Replace('"', '\"');
            }

        }
        public string Password {
            get
            {
                return this.p;
            }
            set
            {
                this.p = value.Replace("'", "\'").Replace('"', '\"');
            }
        }

    }


    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((string)Session["Enabled"] == "1")
                Response.Redirect(".//Ordini.aspx", true);
        }


        protected void Button1_Click(object sender, EventArgs e)
        {
            TextBox Usrname = Username;
            TextBox Pwd = Password;
            Login_Entity LoginRecord = new Login_Entity();
            LoginRecord.UserName = Usrname.Text;
            LoginRecord.Password = Pwd.Text;

            bool authenticated = LoadLoginDataBIZ(LoginRecord);


            if (!authenticated)
            {
                Error_Message.Text = "Username o Password non corretta.";
                Session["Enabled"] = "0";
            }
            else
            {
                Session["UserName"] = LoginRecord.UserName;
                Session["Enabled"] = "1";

                Response.Redirect(".//Ordini.aspx", true);
            }

        }

        /// <summary>
        /// Funzione che sarebbe da mettere nello strato BIZ della aplicazione
        /// </summary>
        /// <returns></returns>

        public bool LoadLoginDataBIZ(Login_Entity LoginUIData)
        {
            return LoadLoginDataDAL(LoginUIData);
        }



        /// <summary>
        /// Funzione che sarebbe da mettere nello strato DAL della aplicazione
        /// </summary>
        /// <returns></returns>

        public bool LoadLoginDataDAL(Login_Entity LoginData)
        {
            int NRecFound = 0;
            string query  = "select count(*) as Cnt from Users where Username = '"+ LoginData.UserName + "' and Password='"+ LoginData.Password+"'";

            string connectionString = ConfigurationManager.ConnectionStrings["GestioneOrdini"].ConnectionString;

            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = null;

            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                int xCnt = reader.GetOrdinal("Cnt");

                if (reader.Read())
                    NRecFound =  reader.GetInt32(xCnt);
                reader.Close();
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                if (connection.State != global::System.Data.ConnectionState.Closed)
                    connection.Close();
            }

            return (NRecFound == 1);

        } // method

    }  // class

} // namespace 