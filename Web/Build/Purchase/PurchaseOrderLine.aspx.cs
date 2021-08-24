using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;


public partial class PurchaseOrderLine : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Purchase/PurchaseOrderLine.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Purchase/PurchaseOrderLine.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbar.AddImageButton("../Purchase/PurchaseOrderLine.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('rgvForm')", "Print Grid", "icon_print.png", "PRINT");
            MenuNewRequisition.AccessRights = this.ViewState;
            MenuNewRequisition.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                MenuNewRequisition.SelectedMenuIndex = 0;
                if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
                    Response.Redirect("PhoenixLogout.aspx");

                rgvForm.PageSize = General.ShowRecords(null);

                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucVessel.Enabled = true;

                ddlStockType.SelectedValue = "SPARE";

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.Enabled = false;
                    ucVessel.bind();
                    ucVessel.DataBind();
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "FLDPARTNUMBER", "FLDNAME", "FLDPURCHASEAPPROVEDATE", "FLDORDEREDDATE", "FLDRECEIVEDDATE", "FLDORDEREDQUANTITY", "FLDRECEIVEDQUANTITY", "FLDQUOTEDPRICEUSD", "FLDPOCOMMITTEDAMOUNT" };
        string[] alCaptions = { "Form No", "Title", "Number", "Name", "Approved On", "Issued On", "Received On", "Ordered Qty", "Received Qty", "Unit Price (USD)", "PO Committed (USD)" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        
        ds = PhoenixPurchaseSearch.OrderFormSearchByLineItem(General.GetNullableString(txtItemNumber.Text), General.GetNullableString(txtName.Text)
              ,General.GetNullableString(ddlStockType.SelectedValue)
              ,General.GetNullableInteger(ucVessel.SelectedVessel)
              , General.GetNullableDateTime(txtFromDate.Text), General.GetNullableDateTime(txtToDate.Text)
              , General.GetNullableString(txtmakerreference.Text)
              , sortexpression, sortdirection, 1, rgvForm.VirtualItemCount>0? rgvForm.VirtualItemCount:10,
                  ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=LineItems.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Line Items</center></h3></td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                if (alCaptions[i].ToUpper().Equals("FLDCREATEDDATE") || alCaptions[i].ToUpper().Equals("FLDPURCHASEAPPROVEDATE") || alCaptions[i].ToUpper().Equals("FLDORDEREDDATE"))
                    Response.Write(General.GetDateTimeToString(dr[alColumns[i]]));
                else
                    Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void MenuNewRequisition_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                rgvForm.CurrentPageIndex = 0;
                rgvForm.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtItemNumber.Text = "";
                txtName.Text = "";
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ddlStockType.SelectedValue = "SPARE";
                txtmakerreference.Text = "";
                rgvForm.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlStockType_TextChanged(object sender, EventArgs e)
    {
        rgvForm.Rebind();
    }

    protected void rgvForm_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFORMNO", "FLDTITLE", "FLDPARTNUMBER", "FLDNAME", "FLDPURCHASEAPPROVEDATE", "FLDORDEREDDATE", "FLDRECEIVEDDATE", "FLDORDEREDQUANTITY", "FLDRECEIVEDQUANTITY", "FLDQUOTEDPRICEUSD", "FLDPOCOMMITTEDAMOUNT" };
        string[] alCaptions = { "Form No", "Title", "Number", "Name", "Approved On", "Issued On", "Received On", "Ordered Qty", "Received Qty", "Unit Price (USD)", "PO Committed (USD)" };


        int? sortdirection = null;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        rgvForm.MasterTableView.ColumnGroups.Clear();
        rgvForm.MasterTableView.Columns.Clear();

        GridBoundColumn formno = new GridBoundColumn();
        rgvForm.MasterTableView.Columns.Add(formno);
        formno.HeaderText = "Form No";
        formno.UniqueName = "FLDFORMNO";
        formno.DataField = "FLDFORMNO";
        formno.DataType = typeof(System.String);

        GridBoundColumn title = new GridBoundColumn();
        rgvForm.MasterTableView.Columns.Add(title);
        title.HeaderText = "Title";
        title.UniqueName = "FLDTITLE";
        title.DataField = "FLDTITLE";
        title.DataType = typeof(System.String);

        GridBoundColumn itemnumber = new GridBoundColumn();
        rgvForm.MasterTableView.Columns.Add(itemnumber);
        itemnumber.HeaderText = "Number";
        itemnumber.UniqueName = "FLDPARTNUMBER";
        itemnumber.DataField = "FLDPARTNUMBER";
        itemnumber.DataType = typeof(System.String);

        GridBoundColumn name = new GridBoundColumn();
        rgvForm.MasterTableView.Columns.Add(name);
        name.HeaderText = "Name";
        name.UniqueName = "FLDNAME";
        name.DataField = "FLDNAME";
        name.DataType = typeof(System.String);

        GridBoundColumn approvedon = new GridBoundColumn();
        rgvForm.MasterTableView.Columns.Add(approvedon);
        approvedon.HeaderText = "Approved On";
        approvedon.UniqueName = "FLDPURCHASEAPPROVEDATE";
        approvedon.DataField = "FLDPURCHASEAPPROVEDATE";
        approvedon.DataType = typeof(System.DateTime);
        approvedon.DataFormatString = "{0:dd-MM-yyyy}";
        

        GridBoundColumn issuedon = new GridBoundColumn();
        rgvForm.MasterTableView.Columns.Add(issuedon);
        issuedon.HeaderText = "Issued On";
        issuedon.UniqueName = "FLDORDEREDDATE";
        issuedon.DataField = "FLDORDEREDDATE";
        issuedon.DataType = typeof(System.DateTime);
        issuedon.DataFormatString = "{0:dd-MM-yyyy}";
        

        GridBoundColumn receivedon = new GridBoundColumn();
        rgvForm.MasterTableView.Columns.Add(receivedon);
        receivedon.HeaderText = "Received On";
        receivedon.UniqueName = "FLDRECEIVEDDATE";
        receivedon.DataField = "FLDRECEIVEDDATE";
        receivedon.DataType = typeof(System.DateTime);
        receivedon.DataFormatString = "{0:dd-MM-yyyy}";
        receivedon.AllowSorting = false;


        GridBoundColumn ordered = new GridBoundColumn();
        rgvForm.MasterTableView.Columns.Add(ordered);
        ordered.HeaderText = "Ordered Qty";
        ordered.UniqueName = "FLDORDEREDQUANTITY";
        ordered.DataField = "FLDORDEREDQUANTITY";
        ordered.DataType = typeof(System.Decimal);
        ordered.DataFormatString = "{0:n0}";
        ordered.AllowSorting = false;

        GridBoundColumn received = new GridBoundColumn();
        rgvForm.MasterTableView.Columns.Add(received);
        received.HeaderText = "Received Qty";
        received.UniqueName = "FLDRECEIVEDQUANTITY";
        received.DataField = "FLDRECEIVEDQUANTITY";
        received.DataType = typeof(System.Decimal);
        received.DataFormatString = "{0:n0}";
        received.AllowSorting = false;

        GridBoundColumn unitprice = new GridBoundColumn();
        rgvForm.MasterTableView.Columns.Add(unitprice);
        unitprice.HeaderText = "Unit Price (USD)";
        unitprice.UniqueName = "FLDQUOTEDPRICEUSD";
        unitprice.DataField = "FLDQUOTEDPRICEUSD";
        unitprice.DataType = typeof(System.Decimal);
        unitprice.DataFormatString = "{0:n3}";
        unitprice.AllowSorting = false;

        GridBoundColumn committed = new GridBoundColumn();
        rgvForm.MasterTableView.Columns.Add(committed);
        committed.HeaderText = "PO Committed (USD)";
        committed.UniqueName = "FLDPOCOMMITTEDAMOUNT";
        committed.DataField = "FLDPOCOMMITTEDAMOUNT";
        committed.DataType = typeof(System.Decimal);
        committed.DataFormatString = "{0:n2}";
        committed.AllowSorting = false;

        formno.HeaderStyle.Width = Unit.Parse("100px");
        formno.ItemStyle.Width = Unit.Parse("100px");
        approvedon.HeaderStyle.Width = Unit.Parse("100px");
        approvedon.ItemStyle.Width = Unit.Parse("100px");
        issuedon.HeaderStyle.Width = Unit.Parse("100px");
        issuedon.ItemStyle.Width = Unit.Parse("100px");
        receivedon.HeaderStyle.Width = Unit.Parse("100px");
        receivedon.ItemStyle.Width = Unit.Parse("100px");

        ordered.HeaderStyle.Width = Unit.Parse("70px");
        ordered.ItemStyle.Width = Unit.Parse("70px");
        ordered.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

        received.HeaderStyle.Width = Unit.Parse("70px");
        received.ItemStyle.Width = Unit.Parse("70px");
        received.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

        unitprice.HeaderStyle.Width = Unit.Parse("70px");
        unitprice.ItemStyle.Width = Unit.Parse("70px");
        unitprice.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

        committed.HeaderStyle.Width = Unit.Parse("70px");
        committed.ItemStyle.Width = Unit.Parse("70px");
        committed.ItemStyle.HorizontalAlign = HorizontalAlign.Right;

        DataSet ds = PhoenixPurchaseSearch.OrderFormSearchByLineItem(General.GetNullableString(txtItemNumber.Text), General.GetNullableString(txtName.Text)
              , General.GetNullableString(ddlStockType.SelectedValue)
              , General.GetNullableInteger(ucVessel.SelectedVessel)
              , General.GetNullableDateTime(txtFromDate.Text), General.GetNullableDateTime(txtToDate.Text)
              , General.GetNullableString(txtmakerreference.Text)
              , sortexpression, sortdirection, rgvForm.CurrentPageIndex+1, rgvForm.PageSize,
                  ref iRowCount, ref iTotalPageCount);

        rgvForm.DataSource = ds;
        rgvForm.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        General.SetPrintOptions("rgvForm", "Line Items", alCaptions, alColumns, ds);
    }

    protected void rgvForm_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
        }
    }

    protected void rgvForm_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void rgvForm_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        rgvForm.Rebind();
    }

    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        rgvForm.Rebind();
    }
}
