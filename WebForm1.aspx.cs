using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Linq.Dynamic.Core;
using ClosedXML.Excel;

namespace lista7v2
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        private string XmlFilePath => Server.MapPath("\\PhoneBook.xml");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData(string sortExpression = null, string sortDirection = "ASC")
        {
            var doc = XDocument.Load(XmlFilePath);

            var subscribers = from s in doc.Descendants("subscriber")
                              select new
                              {
                                  Name = s.Element("name").Value,
                                  Surname = s.Element("surname").Value,
                                  City = s.Element("city").Value,
                                  PhoneNumber = s.Element("phoneNumber").Value
                              };

            if (!string.IsNullOrEmpty(sortExpression))
            {
                subscribers = sortDirection == "ASC"
                    ? subscribers.AsQueryable().OrderBy(sortExpression)
                    : subscribers.AsQueryable().OrderBy($"{sortExpression} descending");
            }

            GridView1.DataSource = subscribers.ToList();
            GridView1.DataBind();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var searchTerm = txtSearch.Text.ToLower();

            var doc = XDocument.Load(XmlFilePath);

            var filteredSubscribers = from s in doc.Descendants("subscriber")
                                      where s.Element("name").Value.ToLower().Contains(searchTerm) ||
                                            s.Element("surname").Value.ToLower().Contains(searchTerm) ||
                                            s.Element("city").Value.ToLower().Contains(searchTerm) ||
                                            s.Element("phoneNumber").Value.ToLower().Contains(searchTerm)
                                      select new
                                      {
                                          Name = s.Element("name").Value,
                                          Surname = s.Element("surname").Value,
                                          City = s.Element("city").Value,
                                          PhoneNumber = s.Element("phoneNumber").Value
                                      };

            GridView1.DataSource = filteredSubscribers.ToList();
            GridView1.DataBind();
        }

        protected void btnExportCsv_Click(object sender, EventArgs e)
        {
            var doc = XDocument.Load(XmlFilePath);

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Subscribers");

            worksheet.Cell(1, 1).Value = "Name";
            worksheet.Cell(1, 2).Value = "Surname";
            worksheet.Cell(1, 3).Value = "City";
            worksheet.Cell(1, 4).Value = "PhoneNumber";

            int row = 2;

            foreach (var subscriber in doc.Descendants("subscriber"))
            {
                string name = subscriber.Element("name").Value;
                string surname = subscriber.Element("surname").Value;
                string city = subscriber.Element("city").Value;
                string phoneNumber = subscriber.Element("phoneNumber").Value;

                worksheet.Cell(row, 1).Value = name;
                worksheet.Cell(row, 2).Value = surname;
                worksheet.Cell(row, 3).Value = city;
                worksheet.Cell(row, 4).Value = phoneNumber;

                row++;
            }

            string excelFilePath = Server.MapPath("~/App_Data/subscribers.xlsx");

            workbook.SaveAs(excelFilePath);

            Response.Write("Dane zostały zapisane do pliku Excela.");
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            var doc = XDocument.Load(XmlFilePath);

            var newSubscriber = new XElement("subscriber",
                new XElement("name", txtName.Text),
                new XElement("surname", txtSurname.Text),
                new XElement("city", txtCity.Text),
                new XElement("phoneNumber", txtPhoneNumber.Text)
            );

            doc.Root.Add(newSubscriber);
            doc.Save(XmlFilePath);

            LoadData();
        }

        protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
        {
            GridView1.EditIndex = e.NewEditIndex;
            LoadData();
        }

        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var doc = XDocument.Load(XmlFilePath);

            var index = e.RowIndex;
            var row = GridView1.Rows[index];

            var name = ((TextBox)row.Cells[0].Controls[0]).Text;
            var surname = ((TextBox)row.Cells[1].Controls[0]).Text;
            var city = ((TextBox)row.Cells[2].Controls[0]).Text;
            var phoneNumber = ((TextBox)row.Cells[3].Controls[0]).Text;

            var subscriber = doc.Descendants("subscriber").ElementAt(index);
            subscriber.Element("name").Value = name;
            subscriber.Element("surname").Value = surname;
            subscriber.Element("city").Value = city;
            subscriber.Element("phoneNumber").Value = phoneNumber;

            doc.Save(XmlFilePath);

            GridView1.EditIndex = -1;
            LoadData();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var doc = XDocument.Load(XmlFilePath);

            var index = e.RowIndex;

            var subscriber = doc.Descendants("subscriber").ElementAt(index);
            subscriber.Remove();

            doc.Save(XmlFilePath);

            LoadData();
        }

        protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
        {
            string sortExpression = e.SortExpression;
            string sortDirection = ViewState["SortDirection"] as string == "ASC" ? "DESC" : "ASC";

            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = sortExpression;

            LoadData(sortExpression, sortDirection);
        }

        protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridView1.EditIndex = -1;
            LoadData();
        }
    }
}
