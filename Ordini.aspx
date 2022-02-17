<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Ordini.aspx.cs" Inherits="GestioneOrdini.Ordini" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gestione Ordini</title>

    
       <style>
              body {
                    font-family:'Trebuchet MS', 'Lucida Sans Unicode', 'Lucida Grande', 'Lucida Sans', Arial, sans-serif;
                    margin: 200px;
                    background-color: #e0fff7;
                   }   

        
            input.textbox, select, textarea
            {
              font-family    :  verdana, arial, snas-serif;
              font-size      :  11px;
              color          :  #000000;

              padding        :  3px;
              background     :  #fcfed7;
              border-left    :  solid 1px #c1c1c1;
              border-top     :  solid 1px #cfcfcf;
              border-right   :  solid 1px #cfcfcf;
              border-bottom  :  solid 1px #6f6f6f;
            }
        
            .err_message, select, textarea
            {
              font-family    :  verdana, arial, snas-serif;
              font-size      :  10px;
              color          :  #ff0000;
            }


            .GridBkgr
            {
                background-color:#e7ceb6
            }

        </style>


        <script type="text/javascript">

            function toggleGridContent() {
                var id = "#gridViewContainer";
                if ($(id).hasClass("small")) {
                    $(id).attr("class", "large");
                } else {
                    $(id).attr("class", "small");
                }
            }

        </script>
</head>




<body>
    <form id="form2" runat="server">  
        <div>      
            <asp:GridView ID="OrdersGrid" runat="server" AutoGenerateColumns="False" CellPadding="6" OnRowCancelingEdit="OrdersGrid_RowCancelingEdit" OnRowEditing="OrdersGrid_RowEditing" OnRowUpdating="OrdersGrid_RowUpdating" OnRowDeleting="OrdersGrid_RowDeleting" OnRowDataBound="OrdersGrid_OnRowDataBound"  ShowFooter ="True">  
                <Columns>  
                    <asp:TemplateField>  
                        <ItemTemplate>  
                            <asp:Button ID="btn_Edit" runat="server" Text="Edit" CommandName="Edit" />  
                        </ItemTemplate>  
                        <EditItemTemplate>  
                            <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update"/>  
                            <asp:Button ID="btn_Delete" runat="server" Text="Delete" CommandName="Delete"/>  
                            <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel"/>  
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:Button ID="Add" runat="server" OnClick="btn_Add" Text="Add" /> 
                        </FooterTemplate>
                    </asp:TemplateField>  

                    <asp:TemplateField HeaderText="ID">  
                        <ItemTemplate>  
                            <asp:Label ID="lbl_ID" runat="server"  ReadOnly="true" Text='<%#Eval("ID") %>'></asp:Label>  
                        </ItemTemplate> 
                        <EditItemTemplate>  
                            <asp:Label ID="lbl_ID" runat="server"  ReadOnly="true" Text='<%#Eval("ID") %>'></asp:Label>  
                        </EditItemTemplate>
                    </asp:TemplateField>  
                    <asp:TemplateField HeaderText="OrderCode">  
                        <ItemTemplate>  
                            <asp:Label ID="lbl_OrderCode" runat="server" Text='<%#Eval("OrderCode") %>'></asp:Label>  
                        </ItemTemplate>  
                        <EditItemTemplate>  
                            <asp:TextBox ID="txt_OrderCode" runat="server" Text='<%#Eval("OrderCode") %>'></asp:TextBox>  
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:TextBox ID="txtOrderCode" Width="200px" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>  
                     <asp:TemplateField HeaderText="InsertDate">  
                        <ItemTemplate>  
                            <asp:Label ID="lbl_InsertDate" runat="server" Text='<%#Eval("InsertDate") %>'></asp:Label>  
                        </ItemTemplate>  
                        <EditItemTemplate>  
                            <asp:TextBox ID="txt_InsertDate" runat="server" Text='<%#Eval("InsertDate") %>'></asp:TextBox>  
                        </EditItemTemplate>  
                    </asp:TemplateField>                   
                     <asp:TemplateField HeaderText="Complete">  
                        <ItemTemplate>  
                            <asp:Label ID="lbl_Complete" runat="server" Text='<%#Eval("Complete") %>'></asp:Label>  
                        </ItemTemplate>  
                        <EditItemTemplate>
                                <asp:DropDownList ID="ddl_EditComplete" runat="server"></asp:DropDownList>    
<%--                            <asp:TextBox ID="txt_Complete" runat="server" Text='<%#Eval("Complete") %>'></asp:TextBox> --%> 
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_AddComplete" runat="server"></asp:DropDownList>  
<%--                            <asp:TextBox ID="txtComplete" Width="200px" runat="server" />--%>
                        </FooterTemplate>
                    </asp:TemplateField>                 
                     <asp:TemplateField HeaderText="CompletionDate">  
                        <ItemTemplate>  
                            <asp:Label ID="lbl_CompletionDate" runat="server" Text='<%#Eval("CompletionDate") %>'></asp:Label>  
                        </ItemTemplate>  
                        <EditItemTemplate>  
                            <asp:TextBox ID="txt_CompletionDate" runat="server" Text='<%#Eval("CompletionDate") %>'></asp:TextBox>  
                        </EditItemTemplate> 
                        <FooterTemplate>
                            <asp:TextBox ID="txtCompletionDate" Width="200px" runat="server" />
                        </FooterTemplate>
                    </asp:TemplateField>                   
                     <asp:TemplateField HeaderText="UserId">  
                        <ItemTemplate>  
                            <asp:Label ID="lbl_UserId" runat="server" ReadOnly="true" Text='<%#Eval("UserId") %>'></asp:Label>  
                        </ItemTemplate>   
                        <EditItemTemplate>  
                            <asp:DropDownList ID="ddl_EditUserId" runat="server"></asp:DropDownList>    
                        </EditItemTemplate> 
                        <FooterTemplate>
                            <asp:DropDownList ID="ddl_AddUserId" runat="server"></asp:DropDownList>  
<%--                            <asp:TextBox ID="txtComplete" Width="200px" runat="server" />--%>
                        </FooterTemplate>
                     </asp:TemplateField>  
                </Columns> 
                <HeaderStyle BackColor="#663300" ForeColor="#ffffff"/>  
                <RowStyle BackColor="#e7ceb6"/>  
                <FooterStyle BackColor="#e7ceb6" />
            </asp:GridView>  
        </div>  
        <asp:Label ID="lblError" runat="server" Font-Names="Verdana" Font-Size="Small" ForeColor="Red" Text=" "></asp:Label>
        <p>
            <asp:Button ID="btnLogout" runat="server" OnClick="btn_logout" Text="Log out" />
        </p>



    </form> 
    

       
</body>
</html>
