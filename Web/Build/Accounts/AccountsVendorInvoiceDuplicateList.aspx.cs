using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsVendorInvoiceDuplicateList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsVendorInvoiceDuplicateList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvInvoice')", "Print Grid", "icon_print.png", "PRINT");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            //   MenuOrderForm.SetTrigger(pnlOrderForm);           
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["invoicecode"] = null;
                ViewState["PAGEURL"] = null;
                if (Request.QueryString["qinvoicecode"] != null)
                {
                    ViewState["invoicecode"] = Request.QueryString["qinvoicecode"].ToString();
                }
            }
            //  BindData();
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

        string[] alCaptions = {
                                "Supplier Code",
                                "Invoice Reference",
                                "Received Date",
                                "Invoice Amount",
                                "Currency Code",
                                "Invoice Status",
                                "Invoice Number",
                                "Entered Date",
                                "Month",
                                "Invoice Type"
                              };

        string[] alColumns = {  "FLDCODE",
                                "FLDINVOICESUPPLIERREFERENCE",
                                "FLDINVOICERECEIVEDDATE",
                                "FLDINVOICEAMOUNT",
                                "FLDCURRENCYNAME",
                                "FLDINVOICESTATUSNAME",
                                "FLDINVOICENUMBER",
                                "FLDINVOICEDATE",
                                "FLDMONTHNAME",
                                "FLDINVOICETYPENAME"
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

        NameValueCollection nvc = Filter.CurrentInvoiceSelection;
        ds = PhoenixAccountsInvoice.VendorDuplicateInvoiceSearch(new Guid(ViewState["invoicecode"].ToString())
                                                , sortexpression
                                                , sortdirection
                                                , (int)ViewState["PAGENUMBER"]
                                                , General.ShowRecords(null)
                                                , ref iRowCount, ref iTotalPageCount
                                                );

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountsInvoice.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Invoice</h3></td>");
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
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentInvoiceSelection;

        ds = PhoenixAccountsInvoice.VendorDuplicateInvoiceSearch(new Guid(ViewState["invoicecode"].ToString())
                                                , sortexpression
                                                , sortdirection
                                                , (int)ViewState["PAGENUMBER"]
                                                , General.ShowRecords(null)
                                                , ref iRowCount, ref iTotalPageCount
                                                );


        string[] alCaptions = {
                                "Supplier Code",
                                "Invoice Reference",
                                "Received Date",
                                "Invoice Amount",
                                "Currency Code",
                                "Invoice Status",
                                "Invoice Number",
                                "Entered Date",
                                "Month",
                                "Invoice Type"
                              };

        string[] alColumns = {  "FLDCODE",
                                "FLDINVOICESUPPLIERREFERENCE",
                                "FLDINVOICERECEIVEDDATE",
                                "FLDINVOICEAMOUNT",
                                "FLDCURRENCYNAME",
                                "FLDINVOICESTATUSNAME",
                                "FLDINVOICENUMBER",
                                "FLDINVOICEDATE",
                                "FLDMONTHNAME",
                                "FLDINVOICETYPENAME"
                             };

        General.SetPrintOptions("gvInvoice", "Accounts Invoice", alCaptions, alColumns, ds);

        gvInvoice.DataSource = ds;
        gvInvoice.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["invoicecode"] == null)
            {
                ViewState["invoicecode"] = ds.Tables[0].Rows[0]["FLDINVOICECODE"].ToString();
                //  gvInvoice.SelectedIndex = 0;
            }

        }
        else
        {
            DataTable dt = ds.Tables[0];
            // ShowNoRecordsFound(dt, gvInvoice);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInvoice_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton lblReference = (LinkButton)e.Item.FindControl("lnkInvoice");
            RadLabel lblDtkey = (RadLabel)e.Item.FindControl("lblDtkey");
            if (lblReference != null)
            {
                lblReference.Attributes.Add("onclick", "openNewWindow('Attachment', '', '" + Session["sitepath"] + "/Accounts/AccountsFileAttachment.aspx?DTKEY=" + lblDtkey.Text + "&MOD=ACCOUNTS&type=Invoice&U=2');return false;");
            }
        }


        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (e.Row.DataItem != null)
        //    {
        //        ImageButton img1 = (ImageButton)e.Row.FindControl("imgAttachment");
        //        RadLabel lblControl = (RadLabel)e.Row.Cells[2].FindControl("lblAttachmentexists");
        //        if (lblControl.Text == "1")
        //        {
        //            img1.Visible = true;
        //        }
        //        else { img1.Visible = false; }
        //    }
        //}
    }

    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    try
    //    {
    //        if (Int32.TryParse(txtnopage.Text, out result))
    //        {
    //            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //            if (0 >= Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = 1;

    //            if ((int)ViewState["PAGENUMBER"] == 0)
    //                ViewState["PAGENUMBER"] = 1;

    //            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //        }
    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    try
    //    {
    //        gvInvoice.SelectedIndex = -1;
    //        gvInvoice.EditIndex = -1;
    //        if (ce.CommandName == "prev")
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //        else
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //        return true;

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["onclick"] = "";
    //}

    protected void gvInvoice_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        //GridView _gridView = (GridView)sender;
        //int iRowno;
        //iRowno = int.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
        {
            Response.Redirect("../Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode=" + ((RadTextBox)e.Item.FindControl("txtInvoiceCode")).Text + "&qfrom=INVOICE");
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        //SetPageNavigator();
        //if (Session["New"].ToString() == "Y")
        //{           
        //    gvInvoice.SelectedIndex = 0;           
        //    Session["New"] = "N";
        //}
    }

    protected void gvInvoice_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        //gvInvoice.SelectedIndex = e.NewSelectedIndex;
    }

    protected void gvInvoice_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}

