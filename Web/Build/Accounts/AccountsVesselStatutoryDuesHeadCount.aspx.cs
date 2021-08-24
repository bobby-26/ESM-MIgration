using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;

public partial class AccountsVesselStatutoryDuesHeadCount : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Head Count", "HEADCOUNT");
            toolbarmain.AddButton("Monthly", "NOOFDAYS");
            toolbarmain.AddButton("Sign Off", "SIGNONOFF");
            toolbarmain.AddButton("Summary For Month", "GROUPBYMONTH");
            toolbarmain.AddButton("Consolidated", "CBANOOFDAYS");
            MenuStatoryDuesMain.AccessRights = this.ViewState;
            MenuStatoryDuesMain.MenuList = toolbarmain.Show();
            MenuStatoryDuesMain.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Post", "POST", ToolBarDirection.Right);
            MenuStatoryDues.AccessRights = this.ViewState;
            MenuStatoryDues.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsVesselStatutoryDuesHeadCount.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageButton("../Accounts/AccountsVesselStatutoryDuesHeadCount.aspx", "Find", "search.png", "FIND");
            MenuStock.AccessRights = this.ViewState;
            MenuStock.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["CALUNIT"] = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 113, "ARE");
                ViewState["CLOSINGDATE"] = null;

                ddlComponent.Items.Clear();
                ddlComponent.Items.Insert(0, new RadComboBoxItem("-- No Component Found --", ""));
                for (int i = 2005; i <= DateTime.Now.Year; i++)
                {
                    ddlYear.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
                ddlYear.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

                ddlYear.SelectedValue = DateTime.Now.Year.ToString();
                ddlMonth.SelectedValue = DateTime.Now.Month.ToString();

            }
            //BindData();
            // BindData();AccountsStatutoryDuesSWFS
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("HEADCOUNT"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesHeadCount.aspx";
            }
            if (CommandName.ToUpper().Equals("NOOFDAYS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesNoofDays.aspx";
            }
            if (CommandName.ToUpper().Equals("CBANOOFDAYS"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesVesselWiseNoofCrew.aspx";
            }
            if (CommandName.ToUpper().Equals("GROUPBYMONTH"))
            {
                ViewState["SETCURRENTNAVIGATIONTAB"] = "../Accounts/AccountsVesselStatutoryDuesGroupbyMonth.aspx";
            }
            if (CommandName.ToUpper().Equals("SIGNONOFF"))
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
    protected void MenuStatoryDues_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("POST"))
            {
                if (!IsValidPosting())
                {
                    ucError.Visible = true;
                    return;
                }
                Guid VoucherFileDtKey = Guid.Empty;
                string title = string.Empty;
                PhoenixAccountsStatutoryDuesReports.PostStatutoryDues(new Guid(ddlComponent.SelectedValue), int.Parse(ddlVessel.SelectedVessel), DateTime.Parse(ViewState["CLOSINGDATE"].ToString()), null, ref VoucherFileDtKey, ref title);
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

        //string Title = ViewState["DATEFORMAT"].ToString();

        System.IO.StringWriter stringwriter = new System.IO.StringWriter();
        HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);

        gvStock.RenderBeginTag(htmlwriter);
        gvStock.MasterTableView.GetItems(GridItemType.Header)[0].RenderControl(htmlwriter);
        foreach (GridDataItem row in gvStock.Items)
        {
            row.RenderControl(htmlwriter);
        }
        gvStock.MasterTableView.GetItems(GridItemType.Footer)[0].RenderControl(htmlwriter);
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
            if (i == 0)
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
        if (!General.GetNullableDateTime(ViewState["CLOSINGDATE"] == null ? "" : ViewState["CLOSINGDATE"].ToString()).HasValue)
            ucError.ErrorMessage = "Date is Required";
        return (!ucError.IsError);
    }
    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        PopulateComponent();
    }
    private void PopulateComponent()
    {
        try
        {

            ddlComponent.Items.Clear();
            DataTable dt = PhoenixAccountsStatutoryDuesReports.ListStatutoryDuesComponent(General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                            , General.GetNullableInteger(ddlMonth.SelectedValue)
                                                            , General.GetNullableInteger(ddlYear.SelectedValue)
                                                            , General.GetNullableInteger(ViewState["CALUNIT"].ToString()));
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = dt.Rows[i];
                    RadComboBoxItem item = new RadComboBoxItem(dr["FLDCOMPONENTNAME"].ToString(), dr["FLDCOMPONENTID"].ToString());
                    item.Attributes["OptionGroup"] = dr["FLDUNION"].ToString();
                    ddlComponent.Items.Add(item);
                }
                ddlComponent.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
            }
            else
            {
                ddlComponent.Items.Insert(0, new RadComboBoxItem("-- No Component Found --", ""));
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
                gvStock.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                DataSet ds = PhoenixAccountsStatutoryDuesReports.SearchStatutoryDues(General.GetNullableGuid(ddlComponent.SelectedValue), General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                                       , General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue), General.GetNullableInteger(ddlHard.SelectedHard));
                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=StatutoryDues.xls");
                Response.Charset = "";
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                stringwriter.Write("<table>");
                stringwriter.Write("<tr>");
                stringwriter.Write("<td colspan='2' rowspan='2'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
                stringwriter.Write("<td colspan=5><h3>" + ddlVessel.SelectedVesselName + " " + ddlMonth.SelectedItem.Text + " - " + ddlYear.SelectedItem.Text + "</h3></td>");
                stringwriter.Write("</tr><tr><td colspan=5><h3>" + ddlComponent.SelectedItem.Text.Substring(0, (ddlComponent.SelectedItem.Text.Length - 25)) + "</h3></td></tr>");
                stringwriter.Write("</table>");
                System.IO.StringWriter stringwriter1 = new System.IO.StringWriter();
                HtmlTextWriter htw = new HtmlTextWriter(stringwriter1);

                gvStock.RenderBeginTag(htw);
                gvStock.MasterTableView.GetItems(GridItemType.Header)[0].RenderControl(htw);
                foreach (GridDataItem row in gvStock.Items)
                {
                    row.RenderControl(htw);
                }
                gvStock.MasterTableView.GetItems(GridItemType.Footer)[0].RenderControl(htw);
                gvStock.RenderEndTag(htw);
                Response.Write(stringwriter + stringwriter1.ToString().Replace("table", "table border =\"1\"").Replace("td", "td valign=\"top\""));
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlComponent_Changed(object sender, EventArgs e)
    {
        string val = ddlComponent.SelectedValue;
        PopulateComponent();
        ddlComponent.SelectedValue = val;
        BindData();
        gvStock.Rebind();
    }
    protected void BindData()
    {
        try
        {
            DataSet ds = PhoenixAccountsStatutoryDuesReports.SearchStatutoryDues(General.GetNullableGuid(ddlComponent.SelectedValue), General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                                    , General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue), General.GetNullableInteger(ddlHard.SelectedHard));
            gvStock.DataSource = ds.Tables[0];


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

    protected void gvStock_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStock_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (drv != null)
        {

            if (e.Item is GridHeaderItem)
            {
                ViewState["CLOSINGDATE"] = drv["FLDCLOSINGDATE"].ToString();
                gvStock.MasterTableView.GetItems(GridItemType.Header)[0].Cells[7].Text = "Rate per month (" + drv["FLDCURRENCYCODE"].ToString() + ")";
                gvStock.MasterTableView.GetItems(GridItemType.Header)[0].Cells[8].Text = "Amount  (" + drv["FLDCURRENCYCODE"].ToString() + ")";
                gvStock.MasterTableView.GetItems(GridItemType.Header)[0].Cells[7].Text = "Total";
                gvStock.MasterTableView.GetItems(GridItemType.Header)[0].Cells[8].Text = drv["FLDTOTALAMOUNTFOOTER"].ToString();
                //gvStock.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
            }
            if (e.Item is GridFooterItem)
            {
                gvStock.MasterTableView.GetItems(GridItemType.Footer)[0].Font.Bold = true;
            }
        }
        else
        {

            if (e.Item is GridHeaderItem)
            {
                gvStock.MasterTableView.GetItems(GridItemType.Header)[0].Cells[7].Text = "Rate per month";
                gvStock.MasterTableView.GetItems(GridItemType.Header)[0].Cells[8].Text = "Amount";
                gvStock.MasterTableView.GetItems(GridItemType.Header)[0].Cells[7].Text = "Total";
                gvStock.MasterTableView.GetItems(GridItemType.Header)[0].Cells[8].Text = "";
                //gvStock.FooterRow.Cells[8].HorizontalAlign = HorizontalAlign.Right;
            }
            if (e.Item is GridFooterItem)
            {
                gvStock.MasterTableView.GetItems(GridItemType.Footer)[0].Font.Bold = true;
            }
        }
    }
}


