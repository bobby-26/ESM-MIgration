using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class AccountsVesselStatutoryDuesSignOffFormat2 : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
           
                SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Head Count", "HEADCOUNT");
            toolbarmain.AddButton("Monthly", "NOOFDAYS");
            toolbarmain.AddButton("Period", "CBANOOFDAYS");
            toolbarmain.AddButton("Summary For Month", "GROUPBYMONTH");
            toolbarmain.AddButton("Sign Off", "SIGNONOFF");
            MenuStatoryDuesMain.AccessRights = this.ViewState;
            MenuStatoryDuesMain.MenuList = toolbarmain.Show();
            MenuStatoryDuesMain.SelectedMenuIndex = 2;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Format 1", "FORMAT1");
            toolbar.AddButton("Format 2", "FORMAT2");
            MenuStatoryDues1.AccessRights = this.ViewState;
            MenuStatoryDues1.MenuList = toolbar.Show();
            MenuStatoryDues1.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Post", "POST");
            MenuStatoryDues.AccessRights = this.ViewState;
            MenuStatoryDues.MenuList = toolbar1.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsVesselStatutoryDuesSignOffFormat2.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageButton("../Accounts/AccountsVesselStatutoryDuesSignOffFormat2.aspx", "Find", "search.png", "FIND");
            MenuStock.AccessRights = this.ViewState;
            MenuStock.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["CLOSINGDATE"] = null;
                txtAsonDate.Text = DateTime.Now.ToString();
                               
                ddlComponent.Items.Clear();
                ddlComponent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--SELECT--", ""));
            }
            BindData();
            // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStatoryDuesMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("HEADCOUNT"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesHeadCount.aspx";
            }
            if (dce.CommandName.ToUpper().Equals("NOOFDAYS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesNoofDays.aspx";
            }
            if (dce.CommandName.ToUpper().Equals("CBANOOFDAYS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesVesselWiseNoofCrew.aspx";
            }
            if (dce.CommandName.ToUpper().Equals("GROUPBYMONTH"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesGroupbyMonth.aspx";
            }
            if (dce.CommandName.ToUpper().Equals("SIGNONOFF"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesSignOffFormat1.aspx";
            }
            Response.Redirect(ViewState["SETCURRENTNAVIGATIONTAB"].ToString(), false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStatoryDues1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FORMAT1"))
            {
                Response.Redirect("../Accounts/AccountsVesselStatutoryDuesSignOffFormat1.aspx", false);
            }
            else if (dce.CommandName.ToUpper().Equals("FORMAT2"))
            {
                Response.Redirect("../Accounts/AccountsVesselStatutoryDuesSignOffFormat2.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStatoryDues_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("POST"))
            {
                if (!IsValidPosting())
                {
                    ucError.Visible = true;
                    return;
                }
                Guid VoucherFileDtKey = Guid.Empty;
                string title = string.Empty;
                PhoenixAccountsStatutoryDuesReports.PostStatutoryDuesForSignOff(new Guid(ddlComponent.SelectedValue), int.Parse(ddlVessel.SelectedVessel), DateTime.Parse(ViewState["CLOSINGDATE"].ToString()), null, ref VoucherFileDtKey, ref title);
                AddAttachment(VoucherFileDtKey, title);
                ucStatus.Text = "Voucher Posted.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void AddAttachment(Guid VoucherFileDtKey, string title)
    {
        //server folder path which is stored your PDF documents
        string path = HttpContext.Current.Request.MapPath("~/Attachments/Accounts/");
        string filefullpath = path + VoucherFileDtKey + ".pdf";
        System.IO.StringWriter stringwriter = new System.IO.StringWriter();
        HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);

        gvStock.RenderBeginTag(htmlwriter);
        gvStock.HeaderRow.RenderControl(htmlwriter);
        foreach (GridViewRow row in gvStock.Rows)
        {
            row.RenderControl(htmlwriter);
        }
        gvStock.FooterRow.RenderControl(htmlwriter);
        gvStock.RenderEndTag(htmlwriter);

        string html = StrHtml(stringwriter.ToString());
        converttopdf(html, filefullpath, title);

    }
    private string StrHtml(string gvHtmlTable)
    {
        string strHtml = gvHtmlTable;
        strHtml = strHtml.Replace("&nbsp;", "");

        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.LoadXml(strHtml);
        XmlNodeList list = XmlDoc.SelectNodes("//tr");
        for (int i = 0; i < list.Count; i++)
        {
            XmlNode node = list[i];
            node.Attributes.RemoveAll();
            XmlAttribute att1 = XmlDoc.CreateAttribute("style");
            if (i == 0 )
            {
                att1.InnerText = "height:10px;font-family:times new roman;font-size:6px;font-weight:bold";
            }
            else
            {
                att1.InnerText = "height:10px;font-family:times new roman;font-size:6px;";
            }
            node.Attributes.Append(att1);

            foreach (XmlNode xn in node.ChildNodes)
            {
                if (xn.Attributes["class"] != null)
                    xn.Attributes.Remove(xn.Attributes["class"]);

                xn.InnerText = xn.InnerText.Trim();
            }
        }
        return XmlDoc.InnerXml;
    }

    //HTMLString = Pass your Html , fileLocation = File Store Location
    public void converttopdf(string HTMLString, string fileLocation, string Title)
    {
        Document document = new Document(new Rectangle(842f, 595f), 35, 50, 80, 40);
        PdfPageEvents e = new PdfPageEvents(Title, ddlVessel.SelectedVesselName, "http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png");
        PdfWriter pw = PdfWriter.GetInstance(document, new FileStream(fileLocation, FileMode.Create));
        pw.PageEvent = e;
        document.Open();
        ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), GridViewStyle());

        for (int k = 0; k < htmlarraylist.Count; k++)
        {
            document.Add((IElement)htmlarraylist[k]);
        }

        document.Close();
    }
    public StyleSheet GridViewStyle()
    {
        StyleSheet css = new StyleSheet();
        css.LoadStyle("DataGrid-HeaderStyle", "background-color", "#5588bb");
        css.LoadStyle("DataGrid-HeaderStyle", "color", "#000000");
        css.LoadStyle("DataGrid-HeaderStyle", "font-weight", "bold");
        //css.LoadStyle("DataGrid-HeaderStyle", "position", "relative");
        //css.LoadStyle("DataGrid-HeaderStyle", "cursor", "default");
        //css.LoadStyle("DataGrid-HeaderStyle", "z-index", "10");
        css.LoadStyle("datagrid_alternatingstyle", "background-color", "#f9f9fa");
        css.LoadStyle("datagrid_selectedstyle", "background-color", "#bbddff");
        css.LoadStyle("datagrid_selectedstyle", "font-weight", "bold");
        css.LoadStyle("datagrid_footerstyle", "background-color", "#dfdfdf");
        css.LoadStyle("datagrid_footerstyle", "color", "#000000");
        return css;
    }
    private bool IsValidPosting()
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableInteger(ddlVessel.SelectedVessel).HasValue)
            ucError.ErrorMessage = "Vessel is Required";
        if (!General.GetNullableGuid(ddlComponent.SelectedValue).HasValue)
            ucError.ErrorMessage = "Component is Required";
        if (!General.GetNullableDateTime(ViewState["CLOSINGDATE"].ToString()).HasValue)
            ucError.ErrorMessage = "Date is Required";
        return (!ucError.IsError);
    }
    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        try
        {
            PopulateComponent();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void PopulateComponent()
    {
        try
        {
            DateTime? d = General.GetNullableDateTime(txtAsonDate.Text).HasValue ? General.GetNullableDateTime(txtAsonDate.Text) : DateTime.Now;
            ddlComponent.Items.Clear();
            DataTable dt = PhoenixAccountsStatutoryDuesReports.ListStatutoryDuesComponent(General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                          , d.Value.Month
                                                          , d.Value.Year
                                                          , General.GetNullableInteger(ViewState["CALUNIT"].ToString()));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(dr["FLDCOMPONENTNAME"].ToString(), dr["FLDCOMPONENTID"].ToString());
                    item.Attributes["OptionGroup"] = dr["FLDUNION"].ToString();
                    ddlComponent.Items.Add(item);
                }
                ddlComponent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("--Select--", ""));
            }
            else
            {
                ddlComponent.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- No Component Found --", ""));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuStock_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {

                DataSet ds = PhoenixAccountsStatutoryDuesReports.StatutoryDuesOwnerReportsSearch(General.GetNullableGuid(ddlComponent.SelectedValue), General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                                        , General.GetNullableDateTime(txtAsonDate.Text));
                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=StatutoryDues.xls");
                Response.Charset = "";
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                stringwriter.Write("<table><tr><td> </td></tr>");
                stringwriter.Write("<tr>");
                stringwriter.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
                stringwriter.Write("</tr>");
                stringwriter.Write("</table>");
                HtmlTextWriter htw = new HtmlTextWriter(stringwriter);
                gvStock.RenderBeginTag(htw);
                gvStock.HeaderRow.RenderControl(htw);
                foreach (GridViewRow row in gvStock.Rows)
                {
                    row.RenderControl(htw);
                }
                gvStock.FooterRow.RenderControl(htw);
                gvStock.RenderEndTag(htw);
                Response.Write(stringwriter.ToString().Replace("table", "table border =\"1\"").Replace("td", "td valign=\"top\""));
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStock_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
            TableCell HeaderCell;

            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 10;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Contribution in " + ViewState["CURRCODE"];
            HeaderCell.ColumnSpan = 2;
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);
            HeaderCell = new TableCell();
            HeaderCell.ColumnSpan = 2;
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);
            gvStock.Controls[0].Controls.AddAt(0, HeaderGridRow);
            GridViewRow row1 = ((GridViewRow)gvStock.Controls[0].Controls[0]);
            row1.Attributes.Add("style", "position:static");
        }
    }
    public void BindData()
    {
        try
        {

            DataSet ds = PhoenixAccountsStatutoryDuesReports.StatutoryDuesOwnerReportsSearch(General.GetNullableGuid(ddlComponent.SelectedValue), General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                                        , General.GetNullableDateTime(txtAsonDate.Text));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                gvStock.DataSource = ds.Tables[0];
                gvStock.DataBind();
                ViewState["CLOSINGDATE"] = ds.Tables[0].Rows[0]["FLDCLOSINGDATE"].ToString();
                gvStock.HeaderRow.Cells[8].Text = "Rate Per Month (" + ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString() + ")";
                ViewState["CURRCODE"] = ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString();
                //gvStock.FooterRow.Cells[7].Text = "Total";
                //gvStock.FooterRow.Cells[8].Text = ds.Tables[0].Rows[0]["FLDTOTALAMOUNTFOOTER"].ToString();
                //gvStock.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
                //gvStock.FooterRow.Font.Bold = true;
            }
            else
            {
                ViewState["RECORDCOUNT"] = "0";
                ShowNoRecordsFound(ds.Tables[0], gvStock);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvStock_RowDataBound(object sender, GridViewRowEventArgs e)
    {}
    class PdfPageEvents : PdfPageEventHelper
    {
        private string _title = string.Empty;
        private string _logo = string.Empty;
        private string _vessel = string.Empty;
        public PdfPageEvents(string Title, string vessel, string strLogoUrl)
        {
            _title = Title;
            _vessel = vessel;
            _logo = strLogoUrl;
        }

        public override void OnEndPage(PdfWriter writer, Document document)
        {
            PdfPTable table = new PdfPTable(2);
            table.TotalWidth = document.PageSize.Width - document.LeftMargin - document.RightMargin;
            float[] widths = new float[] { 5f, table.TotalWidth - 5f };
            table.SetWidths(widths);

            //logo
            PdfPCell cell2 = new PdfPCell(iTextSharp.text.Image.GetInstance(new Uri(_logo)));
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.Border = Rectangle.NO_BORDER;
            table.AddCell(cell2);

            //title
            cell2 = new PdfPCell(new Phrase("\n               " + _title + "\n\n               " + _vessel, new Font(Font.HELVETICA, 11, Font.BOLD)));
            cell2.HorizontalAlignment = Element.ALIGN_LEFT;
            cell2.Border = Rectangle.NO_BORDER;
            table.AddCell(cell2);
            table.WriteSelectedRows(0, -1, document.LeftMargin, document.PageSize.Height - 36, writer.DirectContent);
        }
    }
}
