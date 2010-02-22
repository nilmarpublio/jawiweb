using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Reporting.WebForms;

public partial class Sales : System.Web.UI.Page
{
    private DataSet dataset = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            InitializePage();
        //WriteXmlWithSchema();//once
        //WriteNisanXmlWithSchema();
    }

    #region Methods
    private void InitializePage()
    {
        #region Bind customer drop down list
        string customerName = string.Empty;
        ArrayList al = new ArrayList();

        DataView dataView = new DataView();
        dataset.ReadXml(Server.MapPath("nisan.xml"));
        dataView = dataset.Tables[0].DefaultView;
        dataView.Sort = "SoldTo";
        for (int i = 0; i < dataView.Count; i++)
        {
            if (customerName != dataView[i]["SoldTo"].ToString().Trim())
            {
                customerName = dataView[i]["SoldTo"].ToString().Trim();
                al.Add(customerName);
            }
        }//end loops

        ListItem[] customerList = new ListItem[al.Count + 1];
        customerList[0] = new ListItem("Select All", "");
        for (int i = 0; i < al.Count; i++)
            customerList[i + 1] = new ListItem(al[i].ToString(), al[i]);

        this.ddlCustomer.DataSource = customerList;
        this.ddlCustomer.DataTextField = "DisplayName";
        this.ddlCustomer.DataValueField = "Value";
        this.ddlCustomer.DataBind();
        #endregion
    }
    private void BindReport(string dataSetName, DataTable table, string reportName)
    {
        try
        {
            ReportViewer1.Reset();

            ReportDataSource data = new ReportDataSource(table.TableName, table);
            data.Name = dataSetName;
            ReportViewer1.LocalReport.ReportPath = "Reports\\" + reportName;// "Dashboard.rdlc";
            ReportViewer1.LocalReport.DataSources.Add(data);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            return;
        }
    }
    private string GetFilter()
    {
        string output = string.Empty;
        if (ddlCustomer.SelectedIndex > 0)
            output = AppendQueryWhere(output, "SoldTo = '" + ddlCustomer.SelectedValue + "'");
        output = AppendQueryWhere(output, "Date >= '" + txtFrom.Text + "' AND Date <= '" + txtTo.Text + "'");
        return output;
    }
    public string AppendQueryWhere(string sender, string sql)
    {
        if (sender.Length > 0)
            return sender + " AND " + sql;
        else
            return sql;
    }
    private void WriteXmlWithSchema()
    {
        DataSet ds = new DataSet();
        DataTable t = CreateTable();

        DataRow row1 = t.NewRow();
        row1["Name"] = "Ali bin Ahmad";
        row1["Date"] = "2008-01-01";
        row1["Price"] = 30;

        DataRow row2 = t.NewRow();
        row2["Name"] = "Ramlah bt Mohd";
        row2["Date"] = "2008-01-02";
        row2["Price"] = 330;

        DataRow row3 = t.NewRow();
        row3["Name"] = "Jamed bin Jon";
        row3["Date"] = "2008-01-03";
        row3["Price"] = 220;

        t.Rows.Add(row1);
        t.Rows.Add(row2);
        t.Rows.Add(row3);
        ds.Tables.Add(t);
        ds.AcceptChanges();
        ds.WriteXml(Server.MapPath("App_Data\\orders.xml"), XmlWriteMode.WriteSchema);
    }
    private void WriteNisanXmlWithSchema()
    {
        DataSet ds = new DataSet();
        //dataset.ReadXmlSchema(Server.MapPath("App_Code\\NisanDataSet.xsd"));//081123tys test
        ds.ReadXml(Server.MapPath("App_Data\\nisan.xml"));
        ds.WriteXml(Server.MapPath("App_Data\\orders2.xml"), XmlWriteMode.WriteSchema);
    }
    private DataTable CreateTable()
    {
        DataTable output = new DataTable("Order");
        DataColumn c1 = new DataColumn("Name", typeof(string));
        DataColumn c2 = new DataColumn("Date", typeof(DateTime));
        DataColumn c3 = new DataColumn("Price", typeof(Decimal));
        output.Columns.Add(c1);
        output.Columns.Add(c2);
        output.Columns.Add(c3);
        output.AcceptChanges();

        return output;
    }
    #endregion

    #region Events
    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable table = new DataTable();
            DataView dataView = new DataView();
            //dataset.ReadXmlSchema(Server.MapPath("App_Code\\NisanDataSet.xsd"));//081123tys test
            dataset.ReadXml(Server.MapPath("nisan.xml"));//081231tys //"App_Data\\nisan.xml"
            dataView = dataset.Tables[0].DefaultView;
            dataView.RowFilter = GetFilter();
            dataView.Sort = "Date";

            //copy to a new table
            table = dataView.Table.Clone();
            for (int i = 0; i < dataView.Count; i++)
            {
                DataRow r = table.NewRow();
                for (int j = 0; j < dataView.Table.Columns.Count; j++)
                    r[j] = dataView[i][j];
                table.Rows.Add(r);
            }//end loops

            if (ddlReportType.SelectedIndex == 1 || ddlReportType.SelectedIndex == 2)
            {
                BindReport("NisanDataSet_Order", table, ddlReportType.SelectedValue);

                List<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("SoldTo", ddlCustomer.Text));
                ReportViewer1.LocalReport.SetParameters(parameters);
            }
            else if (ddlReportType.SelectedIndex == ddlReportType.Items.Count - 1)
            {
                BindReport("NisanDataSet_Order", table, ddlReportType.SelectedValue);

                List<ReportParameter> parameters = new List<ReportParameter>();
                parameters.Add(new ReportParameter("From", txtFrom.Text));
                parameters.Add(new ReportParameter("To", txtTo.Text));
                ReportViewer1.LocalReport.SetParameters(parameters);
            }
            else
                BindReport("NisanDataSet_Order", table, ddlReportType.SelectedValue);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex);
            return;
        }
    }
    #endregion
}

public class ListItem
{
    private string displayName;
    public string DisplayName
    {
        get { return this.displayName; }
        set { this.displayName = value; }
    }
    private object value;
    public object Value
    {
        get { return this.value; }
        set { this.value = value; }
    }
    public ListItem(string displayName, object value)
    {
        this.displayName = displayName;
        this.value = value;
    }
}