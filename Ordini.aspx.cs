using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;


namespace GestioneOrdini
{
    public partial class Ordini : System.Web.UI.Page
    {
        string cs = ConfigurationManager.ConnectionStrings["GestioneOrdini"].ConnectionString; //recupera dal Web.config la stringa di connessione al database
        SqlConnection Conn; //oggetto per eseguire la connessione al database
        SqlDataAdapter adapt; //oggetto per poter eseguire il binding del DataTable in base ai campi del db
        DataTable dt; //oggetto che serve per poter inserire le righe e le colonne nella GridView

        protected void Page_Load(object sender, EventArgs e)
        {
            //se l'utente non è loggato lo reindirizza alla pagina di login
            if ((string)Session["Enabled"] != "1")
                Response.Redirect(".//Login.aspx", true);

            //esegue la funzione ShowData() solamente alla prima apertura della pagina
            if (!IsPostBack)
            {
                ShowData();
            }
        }

        //funzione il quale aggiorna i record nella GridView
        protected void ShowData()
        {
            dt = new DataTable();
            Conn = new SqlConnection(cs); //cs = ConnessionString, già definito precedentemente globalmente
            Conn.Open(); //apre la connessione al db
            adapt = new SqlDataAdapter("SELECT * FROM [Orders] ORDER BY [ID]", Conn); //esegue la query estraendo anche i tipi di variabili
            adapt.Fill(dt); //parametrizza i campi del DataTable in base a quelli del db
            if (dt.Rows.Count > 0)
            {
                OrdersGrid.DataSource = dt;
                OrdersGrid.DataBind(); //associa i dati messi nel DataSource alla GridView
            }
            Conn.Close(); //chiude la connessione al db

            //cerca i controller delle DropList tramite ID
            DropDownList Complete = OrdersGrid.FooterRow.FindControl("ddl_AddComplete") as DropDownList;
            DropDownList UserId = OrdersGrid.FooterRow.FindControl("ddl_AddUserId") as DropDownList;

            //carica nella variabile una lista dell'oggeto DDLItem
            //il quale contiene ID (che fa riferimento al db) e ItemText (il nome mostrato nella GridView)
            //(table    id   showed_name)
            List<DDLItem> ItemsList = LoadDdlItems("Users", "ID", "Username");

            //per ogni oggeto nella lista viene aggiunto nel controller del DropDownList
            foreach (DDLItem Item in ItemsList)
                UserId.Items.Add(new ListItem(Item.ItemText, Item.ID.ToString()));

            //nel caso dell DropDownList Complete le scelte sono staticamente solo due
            //per questo vengono aggiunte manualmente
            Complete.Items.Add(new ListItem("Y", "0"));
            Complete.Items.Add(new ListItem("N", "1"));

        }

        public class DDLItem
        {
            public int ID;
            public string ItemText;
        }


        public List<DDLItem> LoadDdlItems(string TableName, string ColIDName, string ColItemName)
        {
            string query = "select " + ColIDName + " as ID, " + ColItemName + " as Item from " + TableName + " order by ID";

            SqlConnection connection = new SqlConnection(cs);
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = null;

            List<DDLItem> returnList = new List<DDLItem>();
            try
            {
                connection.Open();
                reader = command.ExecuteReader();

                int xID = reader.GetOrdinal("ID");
                int xItem = reader.GetOrdinal("Item");

                DDLItem Item;

                while (reader.Read())
                {
                    Item = new DDLItem();
                    Item.ID = reader.GetInt32(xID);
                    Item.ItemText = reader.GetString(xItem);

                    returnList.Add(Item);
                }
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

            return returnList;

        } // method

        //evento scatenato quando viene eseguito il binding dei dati alla GridView
        protected void OrdersGrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {


            DataRow Row;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                if ((OrdersGrid.EditIndex == e.Row.RowIndex)) // && MustFillEdit) 
                {
                    DataRowView dtvComplete = (DataRowView)e.Row.DataItem;
                    string UserID = dtvComplete["UserId"].ToString();
                    DataTable CompleteItems = new DataTable();
                    CompleteItems.Columns.Add(new DataColumn("ID"));
                    CompleteItems.Columns.Add(new DataColumn("Text"));
                    Row = CompleteItems.NewRow();
                    Row[0] = "Y";
                    Row[1] = "0";
                    CompleteItems.Rows.Add(Row);
                    Row = CompleteItems.NewRow();
                    Row[0] = "N";
                    Row[1] = "1";
                    CompleteItems.Rows.Add(Row);
                    DropDownList Complete = (DropDownList)e.Row.FindControl("ddl_EditComplete");
                    // --------------------------------
                    Complete.DataSource = CompleteItems;
                    Complete.DataValueField = "ID";
                    Complete.DataBind();
                    Complete.SelectedValue = UserID;

                    // carico la seconda dropdown
                    CompleteItems.Rows.Clear();

                    DropDownList UserId = (DropDownList)e.Row.FindControl("ddl_EditUserId");
                    //table    id   showed_name
                    List<DDLItem> ItemsList = LoadDdlItems("Users", "ID", "Username");

                    foreach (DDLItem Item in ItemsList)
                        UserId.Items.Add(new ListItem(Item.ItemText, Item.ID.ToString()));

                }
            }

        }


        protected void OrdersGrid_RowCancelingEdit(object sender, System.Web.UI.WebControls.GridViewCancelEditEventArgs e)
        {
            //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview  
            OrdersGrid.EditIndex = -1;
            ShowData();
        }

        protected void OrdersGrid_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
        {
            //NewEditIndex property used to determine the index of the row being edited.  
            OrdersGrid.EditIndex = e.NewEditIndex;
            ShowData();
        }





        protected void OrdersGrid_RowUpdating(object sender, System.Web.UI.WebControls.GridViewUpdateEventArgs e)
        {
            //Finding the controls from Gridview for the row which is going to update  
            Label ID = OrdersGrid.Rows[e.RowIndex].FindControl("lbl_ID") as Label;
            TextBox OrderCode = OrdersGrid.Rows[e.RowIndex].FindControl("txt_OrderCode") as TextBox;
            //TextBox Complete = OrdersGrid.Rows[e.RowIndex].FindControl("txt_Complete") as TextBox;
            TextBox InsertDate = OrdersGrid.Rows[e.RowIndex].FindControl("txt_InsertDate") as TextBox;
            TextBox CompletionDate = OrdersGrid.Rows[e.RowIndex].FindControl("txt_CompletionDate") as TextBox;
            //TextBox UserId = OrdersGrid.Rows[e.RowIndex].FindControl("txt_UserId") as TextBox;
            DropDownList Complete = OrdersGrid.Rows[e.RowIndex].FindControl("ddl_EditComplete") as DropDownList;
            DropDownList UserId = OrdersGrid.Rows[e.RowIndex].FindControl("ddl_EditUserId") as DropDownList;



            //updating the record  
            DateTime InsertDateDTM;
            DateTime CompletionDateDTM;
            DateTime? nCompletionDateDTM;
            int UserIdInt;
            int RowID;

            try
            {
                InsertDateDTM = DateTime.Parse(InsertDate.Text);

                if (DateTime.TryParse(CompletionDate.Text, out CompletionDateDTM))
                    nCompletionDateDTM = CompletionDateDTM;
                else
                    nCompletionDateDTM = null;
                RowID = int.Parse(ID.Text);
                UserIdInt = int.Parse(UserId.SelectedValue);

            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            string Query = @"
                            update Orders set
                                 OrderCode='" + OrderCode.Text.Trim() + @"'
                                ,InsertDate='" + InsertDateDTM.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                                ,Complete='" + Complete.SelectedValue + @"'
                                ,CompletionDate=" + ((nCompletionDateDTM == null) ? "null" : "'" + ((DateTime)nCompletionDateDTM).ToString("yyyy-MM-dd") + "'") + @"
                                ,UserId= " + UserIdInt + @"
                            where 
                                ID = " + RowID + @"
                          ";

            Conn = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand(Query, Conn);

            try
            {
                Conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException Ex)
            {

                lblError.Text = Ex.Message;
            }
            catch (Exception Ex)
            {

                lblError.Text = Ex.Message;
            }
            finally
            {
                Conn.Close();
            }
            //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview  
            OrdersGrid.EditIndex = -1;
            //Call ShowData method for displaying updated data  
            ShowData();
        }

        protected void OrdersGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label ID = OrdersGrid.Rows[e.RowIndex].FindControl("lbl_ID") as Label;
            int RowID = -1;

            try
            {
                RowID = int.Parse(ID.Text);
            }
            catch (Exception Ex)
            {

                lblError.Text = Ex.Message;
            }



            string Query = @"
                            delete from Orders 
                            where 
                                ID = " + RowID.ToString() + @"
                          ";

            Conn = new SqlConnection(cs);
            SqlCommand cmd = new SqlCommand(Query, Conn);

            try
            {
                Conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (SqlException Ex)
            {

                lblError.Text = Ex.Message;
            }
            catch (Exception Ex)
            {

                lblError.Text = Ex.Message;
            }
            finally
            {
                Conn.Close();
            }

            //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview  
            OrdersGrid.EditIndex = -1;
            //Call ShowData method for displaying updated data  
            ShowData();
        }



        protected void btn_Add(object sender, EventArgs e)
        {
            //TextBox ID = OrdersGrid.FooterRow.FindControl("txtID") as TextBox;
            TextBox txtOrderCode = OrdersGrid.FooterRow.FindControl("txtOrderCode") as TextBox;
            TextBox txtComplete = OrdersGrid.FooterRow.FindControl("txtComplete") as TextBox;
            //TextBox InsertDate = OrdersGrid.FooterRow.FindControl("txtInsertDate") as TextBox;
            TextBox txtCompletionDate = OrdersGrid.FooterRow.FindControl("txtCompletionDate") as TextBox;
            DropDownList AddComplete = OrdersGrid.FooterRow.FindControl("ddl_AddComplete") as DropDownList;
            DropDownList AddUserId = OrdersGrid.FooterRow.FindControl("ddl_AddUserId") as DropDownList;
            //TextBox UserId = OrdersGrid.FooterRow.FindControl("txtUserId") as TextBox;

            DateTime InsertDateDTM;
            DateTime CompletionDateDTM;
            DateTime? nCompletionDateDTM;
            int UserIdInt;

            InsertDateDTM = DateTime.Now; // l'ora di immissione del record viene presa dal momento del click/add (non la mette l'utente)
            UserIdInt = Int32.Parse(AddUserId.SelectedValue);

            lblError.Text = "";
            try
            {
                bool InputOK;
                InputOK = (txtOrderCode.Text.Length == 8);
                if (!InputOK)
                {
                    lblError.Text = "Order-code must be made of 8 digits.";
                    return;
                }
                InputOK = (AddComplete.SelectedItem.ToString() == "Y" || AddComplete.SelectedItem.ToString() == "N");
                if (!InputOK)
                {
                    lblError.Text = "Is mandatory to specify the completion status (Y/N)";
                    return;
                }


                if (txtCompletionDate.Text.Trim() == "")
                    nCompletionDateDTM = null;
                else
                {
                    if (DateTime.TryParse(txtCompletionDate.Text, out CompletionDateDTM))
                        nCompletionDateDTM = CompletionDateDTM;
                    else
                    {
                        lblError.Text = "Dates must be formatted as yyyy-mm-dd (or empty if there is not one)";
                        return;
                    }
                }



                // OKK i dati immessi sono corretti 

                string Query = @"
                                    INSERT INTO [dbo].[Orders]
                                               ([OrderCode]
                                               ,[Complete]
                                               ,[InsertDate]
                                               ,[CompletionDate]
                                               ,[UserId])
                                         VALUES
                                               (
                                                 '" + txtOrderCode.Text + @"'
                                                ,'" + AddComplete.SelectedItem.ToString() + @"'
                                                ,'" + InsertDateDTM.ToString("yyyy-MM-dd HH:mm:ss") + @"'
                                                ," + ((nCompletionDateDTM == null) ? "null" : "'" + ((DateTime)nCompletionDateDTM).ToString("yyyy-MM-dd") + "'") + @"
                                                , " + UserIdInt.ToString() + @"
                                                )
                                ";
                Conn = new SqlConnection(cs);
                SqlCommand cmd = new SqlCommand(Query, Conn);

                try
                {
                    Conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException Ex)
                {

                    lblError.Text = Ex.Message;
                }
                catch (Exception Ex)
                {

                    lblError.Text = Ex.Message;
                }
                finally
                {
                    Conn.Close();
                }


                // ripuisce le caselle di input, visto ch il dato è già entrato
                txtOrderCode.Text = "";
                txtCompletionDate.Text = "";
                //Setting the EditIndex property to -1 to cancel the Edit mode in Gridview  
                OrdersGrid.EditIndex = -1;
                //Call ShowData method for displaying updated data  
                ShowData();

            }
            catch (Exception Ex)
            {

                lblError.Text = Ex.Message;
            }



        }

        protected void btn_logout(object sender, EventArgs e)
        {
            Session["Enabled"] = "0";
            Response.Redirect(".//Login.aspx", true);
        }









    }
}