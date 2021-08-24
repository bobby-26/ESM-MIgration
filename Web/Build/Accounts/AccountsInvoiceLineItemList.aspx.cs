using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsInvoiceLineItemList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvInvoice.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsInvoiceLineItemList.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('divGrid')", "Print Grid", "icon_print.png", "PRINT");
        toolbargrid.AddImageLink("javascript:parent.Openpopup('Filter','','../Accounts/AccountsInvoiceLineItemFilter.aspx'); return false;", "Filter", "search.png", "FIND");
        toolbargrid.AddImageLink("javascript:parent.Openpopup('Add','Add','AccountsInvoiceLineItem.aspx?QINVOICECODE=" + Request.QueryString["QINVOICECODE"] + "'); return false;", "ADD", "Add.png", "ADD");
        
        MenuInvoice.AccessRights = this.ViewState;
        MenuInvoice.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["ACTIVITYID"] = null;
            ViewState["INVOICECODE"] = Request.QueryString["QINVOICECODE"];
        }
        BindData();
        SetPageNavigator();
    }

    protected void RegistersAccountMenu_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            SetPageNavigator();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { 
                                "Purchase Order Number",
                                "Vessel Code", 
                                "Vessel Account",
                                "Pur.Payable Amount",
                                "Pur.Advance Amount",
                                "Inv.Payable Amount ", 
                                "Is Included in SOA"                             
                              };

        string[] alColumns = {  "FLDPURCHASEORDERNUMBER", 
                                "FLDVESSELID", 
                                "FLDVESSELACCOUNT", 
                                "FLDPURCHASEPAYABLEAMOUNT",
                                "FLDPURCHASEADVANCEAMOUNT",
                                "FLDINVOICEPAYABLEAMOUNT",
                                "FLDISINCLUDEDINSOA"
                             };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int? iVesselcode = null;


        NameValueCollection nvc = Filter.CurrentInvoiceLineItemSelection;

        if (nvc != null && nvc.Get("txtVesselCodeSearch").ToString().Trim() != string.Empty)
            iVesselcode = int.Parse(nvc.Get("txtVesselCodeSearch").ToString().Trim());

        ds = PhoenixAccountsInvoice.InvoiceLineItemSearch(
                                                            new Guid(ViewState["INVOICECODE"].ToString())
                                                            , iVesselcode
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtPurchaseOrderNumberSearch").ToString().Trim()) : string.Empty
                                                            , sortexpression
                                                            , sortdirection
                                                            , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                        );

        Response.AddHeader("Content-Disposition", "attachment; filename=Invoice Line Item.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='" + HttpContext.Current.Session["sitepath"] + "/images/esmlogo4_small.png' /></td>");
        Response.Write("<td><h3>Invoice Line Item</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void Invoice_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
            ShowExcel();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        int? iVesselcode = null;

        NameValueCollection nvc = Filter.CurrentInvoiceLineItemSelection;

        if (nvc != null && nvc.Get("txtVesselCodeSearch").ToString().Trim() != string.Empty)
            iVesselcode = int.Parse(nvc.Get("txtVesselCodeSearch").ToString().Trim());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsInvoice.InvoiceLineItemSearch(
                                                            new Guid(ViewState["INVOICECODE"].ToString())
                                                            , iVesselcode
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtPurchaseOrderNumberSearch").ToString().Trim()) : string.Empty
                                                            , sortexpression
                                                            , sortdirection
                                                            , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                        );

        string[] alCaptions = { 
                                "Purchase Order Number",
                                "Vessel Code", 
                                "Vessel Account",
                                "Pur.Payable Amount",
                                "Pur.Advance Amount",
                                "Inv.Payable Amount ", 
                                "Is Included in SOA"                             
                              };

        string[] alColumns = {  "FLDPURCHASEORDERNUMBER", 
                                "FLDVESSELID", 
                                "FLDVESSELACCOUNT", 
                                "FLDPURCHASEPAYABLEAMOUNT",
                                "FLDPURCHASEADVANCEAMOUNT",
                                "FLDINVOICEPAYABLEAMOUNT",
                                "FLDISINCLUDEDINSOA"
                             };

        General.SetPrintOptions("gvInvoice", "Invoice Line Item", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvInvoice.DataSource = ds;
            gvInvoice.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvInvoice);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void InvoiceClick(object sender, CommandEventArgs e)
    {
        ViewState["INVOICELINEITEMCODE"] = e.CommandArgument;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        BindData();
        SetPageNavigator();
    }

    protected void gvInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            BindData();
            _gridView.EditIndex = Int32.Parse(e.CommandArgument.ToString());
        }
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
        }
    }

    protected void gvInvoice_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvInvoice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvInvoice_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            if (cmdEdit != null)
            if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                LinkButton lbtn = (LinkButton)e.Row.FindControl("lnkPurchaseOrderNumber");
                lbtn.Attributes.Add("onclick", "javascript:parent.Openpopup('SubAccount', '', 'AccountsInvoiceLineItemSubAccount.aspx?QINVOICELINEITEMCODE=" + lbtn.CommandArgument + "&QINVOICECODE=" + ViewState["INVOICECODE"].ToString() + "'); return false;");
            }
        }


    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvInvoice.SelectedIndex = -1;
        gvInvoice.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }
        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
