<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="lista7v2.WebForm1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Książka Telefoniczna</title>
    <style>
        body {
            background-color: #00ff00;
        }
        .form-container {
            text-align: center;
            margin: 0 auto;
            max-width: 600px;
        }
        .add-contact, .search-contact {
            text-align: left;
            margin: 20px 0;
        }
        .add-contact div, .search-contact div {
            margin-bottom: 10px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <h1>Wielka ksiega kontaktow</h1>

            <div class="search-contact">
                <h2>Wyszukaj kontakt</h2>
                <div>
                    <asp:TextBox ID="txtSearch" runat="server" Placeholder="Wyszukaj..." />
                </div>
                <div>
                    <asp:Button ID="btnSearch" runat="server" Text="Szukaj" OnClick="btnSearch_Click" />
                </div>
            </div>

            <h2>Lista Kontaktow</h2>
            <asp:GridView 
                ID="GridView1" 
                runat="server" 
                AutoGenerateColumns="False" 
                AllowSorting="True"
                OnRowEditing="GridView1_RowEditing" 
                OnRowUpdating="GridView1_RowUpdating" 
                OnRowCancelingEdit="GridView1_RowCancelingEdit"
                OnRowDeleting="GridView1_RowDeleting"
                OnSorting="GridView1_Sorting">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Imię">
                        <ItemStyle Wrap="false" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Surname" HeaderText="Nazwisko">
                        <ItemStyle Wrap="false" />
                    </asp:BoundField>
                    <asp:BoundField DataField="City" HeaderText="Miejscowość">
                        <ItemStyle Wrap="false" />
                    </asp:BoundField>
                    <asp:BoundField DataField="PhoneNumber" HeaderText="Numer telefonu">
                        <ItemStyle Wrap="false" />
                    </asp:BoundField>
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>

            <div class="add-contact">
                <h2>Dodaj nowy kontakt</h2>
                <div>
                    <asp:TextBox ID="txtName" runat="server" Placeholder="Imię" />
                </div>
                <div>
                    <asp:TextBox ID="txtSurname" runat="server" Placeholder="Nazwisko" />
                </div>
                <div>
                    <asp:TextBox ID="txtCity" runat="server" Placeholder="Miejscowość" />
                </div>
                <div>
                    <asp:TextBox ID="txtPhoneNumber" runat="server" Placeholder="Numer telefonu" />
                </div>
                <div>
                    <asp:Button ID="btnAdd" runat="server" Text="Dodaj" OnClick="btnAdd_Click" />
                </div>
            </div>

            <div>
                <asp:Button ID="btnExportCsv" runat="server" Text="Eksportuj do CSV" OnClick="btnExportCsv_Click" />
            </div>
        </div>
    </form>
</body>
</html>
