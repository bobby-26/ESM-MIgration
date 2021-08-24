using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Collections;
using System.Text;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;

public partial class PurchaseReconciledInvoiceList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            txtVendorId.Attributes.Add("style", "visibility:hidden");
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Purchase/PurchaseReconciledInvoiceList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvVoucherDetails')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Purchase/PurchaseReconciledInvoiceList.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Purchase/PurchaseReconciledInvoiceList.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            MenuOrderForm.SetTrigger(pnlOrderForm);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = null;

                ddlCurrencyCode.CurrencyList = PhoenixRegistersCurrency.ListCurrency(1);
                ddlCurrencyCode.DataBind();

                Filter.CurrentReconciledInvoiceFilterSelection = null;
                if (Filter.CurrentReconciledInvoiceFilterSelection == null)
                {
                    DateTime now = DateTime.Now;
                    DateTime fromdate = new DateTime(now.Year, now.Month, 1);
                }

                BindVesselList();
                BindInvoiceStatusList();
            }
            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=130,131,132,135,183', true); ");
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoucherDetails_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
        SetPageNavigator();
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("NEW"))
            {
                BindData();
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
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Invoice Number", "Order Number", "Vessel", "Received Date", "Reconciled Date" };
        string[] alColumns = { "FLDINVOICENUMBER", "FLDFORMNO", "FLDVESSELNAME", "FLDINVOICERECEIVEDDATE", "FLDRECONCILEDDATE" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DateTime? fromdate = null, todate = null;
        DateTime now = DateTime.Now;
        fromdate = new DateTime(now.Year, now.Month, 1);
        todate = DateTime.Today;

        if (Filter.CurrentReconciledInvoiceFilterSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentReconciledInvoiceFilterSelection;

            ds = PhoenixPurchaseOrderForm.ReconciledInvoiceList(nvc != null ? General.GetNullableInteger(nvc.Get("ddlInvoiceType").ToString().Trim()) : null
                     , nvc != null ? General.GetNullableString(nvc.Get("ucInvoiceStatus").ToString().Trim()) : null
                    , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceNumber").ToString().Trim()) : null
                    , nvc != null ? General.GetNullableString(nvc.Get("txtSupplierReference").ToString().Trim()) : null
                    , nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId").ToString().Trim()) : null
                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim()) : null
                    , nvc != null ? General.GetNullableDateTime(nvc.Get("ucFromDate")) : General.GetNullableDateTime(fromdate.ToString())
                    , nvc != null ? General.GetNullableDateTime(nvc.Get("ucToDate")) : General.GetNullableDateTime(todate.ToString())
                    , nvc != null ? General.GetNullableString(nvc.Get("ucVessel").ToString().Trim()) : null
                    , sortexpression
                    , sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount);

        }
        else
        {
            DateTime? dtfromdate = General.GetNullableDateTime(ucFromDate.Text);
            DateTime? dttodate = General.GetNullableDateTime(ucToDate.Text);

            ds = PhoenixPurchaseOrderForm.ReconciledInvoiceList(null
                                                              , null
                                                              , null
                                                              , null
                                                              , null
                                                              , null
                                                              , dtfromdate != null ? dtfromdate : General.GetNullableDateTime(fromdate.ToString())
                                                              , dttodate != null ? dttodate : General.GetNullableDateTime(todate.ToString())
                                                              , null
                                                              , sortexpression
                                                              , sortdirection
                                                              , (int)ViewState["PAGENUMBER"]
                                                              , iRowCount
                                                              , ref iRowCount
                                                              , ref iTotalPageCount);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=ReconciledInvoiceList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Reconciliation Statistics</h3></td>");
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

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();//Filter.CurrentReconciledInvoiceFilterSelection = null;
            }
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                Filter.CurrentReconciledInvoiceFilterSelection = null;
                NameValueCollection criteria = new NameValueCollection();

                StringBuilder stinvoicestautslist = new StringBuilder();
                StringBuilder stvessellist = new StringBuilder();

                foreach (ListItem item in chkInvoiceStatus.Items)
                {
                    if (item.Selected == true)
                    {
                        stinvoicestautslist.Append(item.Value.ToString());
                        stinvoicestautslist.Append(",");
                    }
                }
                if (stinvoicestautslist.Length > 1)
                {
                    stinvoicestautslist.Remove(stinvoicestautslist.Length - 1, 1);
                }
                if (stinvoicestautslist.ToString().Contains("Dummy"))
                {
                    stinvoicestautslist = new StringBuilder();
                    stinvoicestautslist.Append("Dummy");
                }

                foreach (ListItem item in chkVesselList.Items)
                {
                    if (item.Selected == true)
                    {
                        stvessellist.Append(item.Value.ToString());
                        stvessellist.Append(",");
                    }
                }
                if (stvessellist.Length > 1)
                {
                    stvessellist.Remove(stvessellist.Length - 1, 1);
                }
                if (stvessellist.ToString().Contains("Dummy"))
                {
                    stvessellist = new StringBuilder();
                    stvessellist.Append("Dummy");
                }

                DateTime? dtfromdate = General.GetNullableDateTime(ucFromDate.Text);
                DateTime? dttodate = General.GetNullableDateTime(ucToDate.Text);

                criteria.Clear();
                criteria.Add("ddlInvoiceType", ddlInvoiceType.SelectedHard);
                criteria.Add("txtInvoiceNumber", txtInvoiceNumber.Text.Trim());
                criteria.Add("txtSupplierReference", txtSupplierReference.Text.Trim());
                criteria.Add("ddlCurrencyCode", ddlCurrencyCode.SelectedCurrency);
                criteria.Add("ucVessel", stvessellist.ToString());
                criteria.Add("ucFromDate", ucFromDate.Text);
                criteria.Add("ucToDate", ucToDate.Text);
                criteria.Add("ucInvoiceStatus", stinvoicestautslist.ToString());
                criteria.Add("txtVendorId", txtVendorId.Text.Trim());

                Filter.CurrentReconciledInvoiceFilterSelection = criteria;
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentReconciledInvoiceFilterSelection = null;
                ucFromDate.Text = ucToDate.Text = "";
                ddlInvoiceType.SelectedHard = ddlCurrencyCode.SelectedCurrency = "";
                txtInvoiceNumber.Text = txtSupplierReference.Text = txtVenderName.Text = txtVendorCode.Text = txtVendorId.Text = "";
                foreach (ListItem item in chkInvoiceStatus.Items)
                {
                    if (item.Selected == true)
                    {
                        item.Selected = false;
                    }
                }
                foreach (ListItem item in chkVesselList.Items)
                {
                    if (item.Selected == true)
                    {
                        item.Selected = false;
                    }
                }
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DateTime? fromdate = null, todate = null;

        DateTime now = DateTime.Now;
        fromdate = new DateTime(now.Year, now.Month, 1);
        todate = DateTime.Today;

        if (Filter.CurrentReconciledInvoiceFilterSelection != null)
        {
            NameValueCollection nvc = Filter.CurrentReconciledInvoiceFilterSelection;

            ds = PhoenixPurchaseOrderForm.ReconciledInvoiceList(nvc != null ? General.GetNullableInteger(nvc.Get("ddlInvoiceType").ToString().Trim()) : null
                     , nvc != null ? General.GetNullableString(nvc.Get("ucInvoiceStatus").ToString().Trim()) : null
                    , nvc != null ? General.GetNullableString(nvc.Get("txtInvoiceNumber").ToString().Trim()) : null
                    , nvc != null ? General.GetNullableString(nvc.Get("txtSupplierReference").ToString().Trim()) : null
                    , nvc != null ? General.GetNullableInteger(nvc.Get("txtVendorId").ToString().Trim()) : null
                    , nvc != null ? General.GetNullableInteger(nvc.Get("ddlCurrencyCode").ToString().Trim()) : null
                    , nvc != null ? General.GetNullableDateTime(nvc.Get("ucFromDate")) : General.GetNullableDateTime(fromdate.ToString())
                    , nvc != null ? General.GetNullableDateTime(nvc.Get("ucToDate")) : General.GetNullableDateTime(todate.ToString())
                    , nvc != null ? General.GetNullableString(nvc.Get("ucVessel").ToString().Trim()) : null
                    , sortexpression
                    , sortdirection
                    , (int)ViewState["PAGENUMBER"]
                    , General.ShowRecords(null)
                    , ref iRowCount
                    , ref iTotalPageCount);

        }
        else
        {
            DateTime? dtfromdate = General.GetNullableDateTime(ucFromDate.Text);
            DateTime? dttodate = General.GetNullableDateTime(ucToDate.Text);

            ds = PhoenixPurchaseOrderForm.ReconciledInvoiceList(null
                                                              , null
                                                              , null
                                                              , null
                                                              , null
                                                              , null
                                                              , dtfromdate != null ? dtfromdate : General.GetNullableDateTime(fromdate.ToString())
                                                              , dttodate != null ? dttodate : General.GetNullableDateTime(todate.ToString())
                                                              , null
                                                              , sortexpression
                                                              , sortdirection
                                                              , (int)ViewState["PAGENUMBER"]
                                                              , General.ShowRecords(null)
                                                              , ref iRowCount
                                                              , ref iTotalPageCount);
        }

        string[] alCaptions = { "Invoice Number", "Order Number", "Vessel", "Received Date", "Reconciled Date" };
        string[] alColumns = { "FLDINVOICENUMBER", "FLDFORMNO", "FLDVESSELNAME", "FLDINVOICERECEIVEDDATE", "FLDRECONCILEDDATE" };

        General.SetPrintOptions("gvVoucherDetails", "Reconciliation Statistics", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVoucherDetails.DataSource = ds;
            gvVoucherDetails.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVoucherDetails);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void BindVesselList()
    {
        DataSet ds = PhoenixRegistersVessel.ListVessel();
        chkVesselList.Items.Add("select");
        chkVesselList.DataSource = ds;
        chkVesselList.DataTextField = "FLDVESSELNAME";
        chkVesselList.DataValueField = "FLDVESSELID";
        chkVesselList.DataBind();
        //chkVesselList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    private void BindInvoiceStatusList()
    {
        DataSet ds = PhoenixRegistersHard.ListHard(1, 60, 0, "ACH,AMC,APO,ARE,ASI,FPP,ICD,INP,OMA,PAU,PBB,RCK,RPO,RWA");
        chkInvoiceStatus.Items.Add("select");
        chkInvoiceStatus.DataSource = ds;
        chkInvoiceStatus.DataTextField = "FLDHARDNAME";
        chkInvoiceStatus.DataValueField = "FLDHARDCODE";
        chkInvoiceStatus.DataBind();
        // chkInvoiceStatus.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            gvVoucherDetails.SelectedIndex = -1;
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVoucherDetails_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        try
        {
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvVoucherDetails.SelectedIndex = -1;
            gvVoucherDetails.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
            return true;

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
        gv.Rows[0].Attributes["onclick"] = "";
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
}
