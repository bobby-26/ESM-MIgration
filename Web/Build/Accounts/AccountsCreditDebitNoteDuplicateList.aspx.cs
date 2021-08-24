using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsCreditDebitNoteDuplicateList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsCreditDebitNoteDuplicateList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvCreditNote')", "Print Grid", "icon_print.png", "PRINT");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
            MenuOrderForm.SetTrigger(pnlOrderForm);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["PAGENUMBER1"] = 1;
                ViewState["SORTEXPRESSION1"] = null;
                ViewState["SORTDIRECTION1"] = null;

                ViewState["creditdebitnoteid"] = null;
                ViewState["suppliercode"] = null;
                ViewState["referenceno"] = null;
                ViewState["PAGEURL"] = null;
                if (Request.QueryString["creditdebitnoteid"] != null)
                {
                    ViewState["creditdebitnoteid"] = Request.QueryString["creditdebitnoteid"].ToString();
                    ViewState["suppliercode"] = Request.QueryString["suppliercode"].ToString();
                    ViewState["referenceno"] = Request.QueryString["referenceno"].ToString();
                }
            }
            BindData();
            SetPageNavigator();

            BindInvoiceData();
            SetPageNavigator1();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCreditNote_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
        SetPageNavigator();
    }


    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { 
                                "Supplier Code",
                                "Reference No",
                                "Received Date",
                                "Currency Code",
                                "Amount",
                                "Status",
                                "Register Number",                                                              
                                "Entered Date",                               
                              };

        string[] alColumns = {  "FLDCODE", 
                                "FLDREFERENCENO", 
                                "FLDRECEIVEDDATE",
                                "FLDCURRENCYNAME",
                                "FLDAMOUNT",
                                "FLDSTATUSNAME",
                                "FLDCNREGISTERNO",
                                "FLDCREATEDDATE",
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
        ds = PhoenixAccountsCreditDebitNote.VendorDuplicateCreditDebitNoteSearch(new Guid(ViewState["creditdebitnoteid"].ToString())
                                                , sortexpression
                                                , sortdirection
                                                , (int)ViewState["PAGENUMBER"]
                                                , General.ShowRecords(null)
                                                , ref iRowCount, ref iTotalPageCount
                                                );

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountsCreditNote.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Credit Note</h3></td>");
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

        ds = PhoenixAccountsCreditDebitNote.VendorDuplicateCreditDebitNoteSearch(new Guid(ViewState["creditdebitnoteid"].ToString())
                                                , sortexpression
                                                , sortdirection
                                                , (int)ViewState["PAGENUMBER"]
                                                , General.ShowRecords(null)
                                                , ref iRowCount, ref iTotalPageCount
                                                );


        string[] alCaptions = { 
                                "Supplier Code",
                                "Reference No",
                                "Received Date",
                                "Currency Code",
                                "Amount",
                                "Status",
                                "Register Number",                                                              
                                "Entered Date",                               
                              };

        string[] alColumns = {  "FLDCODE", 
                                "FLDREFERENCENO", 
                                "FLDRECEIVEDDATE",
                                "FLDCURRENCYNAME",
                                "FLDAMOUNT",
                                "FLDSTATUSNAME",
                                "FLDCNREGISTERNO",
                                "FLDCREATEDDATE",
                             };

        General.SetPrintOptions("gvCreditNote", "Accounts Credit Note", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCreditNote.DataSource = ds;
            gvCreditNote.DataBind();

            if (ViewState["creditdebitnoteid"] == null)
            {
                ViewState["creditdebitnoteid"] = ds.Tables[0].Rows[0]["FLDCREDITDEBITNOTEID"].ToString();
                gvCreditNote.SelectedIndex = 0;
            }

        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCreditNote);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            gvCreditNote.SelectedIndex = -1;
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

    protected void gvCreditNote_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvCreditNote_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvCreditNote_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvCreditNote_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            gvCreditNote.SelectedIndex = -1;
            gvCreditNote.EditIndex = -1;
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

    protected void gvCreditNote_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        GridView _gridView = (GridView)sender;
        int iRowno;
        iRowno = int.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        //if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
        //{
        //    Response.Redirect("../Accounts/AccountsInvoiceAttachments.aspx?creditdebitnoteid=" + ((TextBox)_gridView.Rows[iRowno].FindControl("txtCreditNoteCode")).Text + "&qfrom=INVOICE");
        //}
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
        if (Session["New"].ToString() == "Y")
        {
            gvCreditNote.SelectedIndex = 0;
            Session["New"] = "N";
        }
    }

    protected void gvCreditNote_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvCreditNote.SelectedIndex = e.NewSelectedIndex;
    }


    private void BindInvoiceData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION1"] == null) ? null : (ViewState["SORTEXPRESSION1"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION1"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION1"].ToString());

        NameValueCollection nvc = Filter.CurrentInvoiceSelection;

        ds = PhoenixAccountsCreditDebitNote.VendorDuplicateCNInvoiceSearch(int.Parse(ViewState["suppliercode"].ToString())
                                                , ViewState["referenceno"].ToString()
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
        ViewState["ROWCOUNT1"] = iRowCount;
        ViewState["TOTALPAGECOUNT1"] = iTotalPageCount;
    }

    private void SetPageNavigator1()
    {
        cmdPrevious1.Enabled = IsPreviousEnabled1();
        cmdNext1.Enabled = IsNextEnabled1();
        lblPagenumber1.Text = "Page " + ViewState["PAGENUMBER1"].ToString();
        lblPages1.Text = " of " + ViewState["TOTALPAGECOUNT1"].ToString() + " Pages. ";
        lblRecords1.Text = "(" + ViewState["ROWCOUNT1"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled1()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER1"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT1"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled1()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER1"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT1"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    protected void gvInvoice_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION1"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION1"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION1"] == null || ViewState["SORTDIRECTION1"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }

    protected void gvInvoice_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindInvoiceData();
    }

    protected void gvInvoice_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        GridView _gridView = (GridView)sender;
        int iRowno;
        iRowno = int.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
        {
            Response.Redirect("../Accounts/AccountsInvoiceAttachments.aspx?qinvoicecode=" + ((TextBox)_gridView.Rows[iRowno].FindControl("txtInvoiceCode")).Text + "&qfrom=INVOICE");
        }
    }

    protected void gvInvoice_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindInvoiceData();
        SetPageNavigator1();
    }

    protected void gvInvoice_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindInvoiceData();
        SetPageNavigator1();
    }

    protected void gvInvoice_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvInvoice.SelectedIndex = e.NewSelectedIndex;
    }

    protected void gvInvoice_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION1"] = se.SortExpression;

        if (ViewState["SORTDIRECTION1"] != null && ViewState["SORTDIRECTION1"].ToString() == "0")
            ViewState["SORTDIRECTION1"] = 1;
        else
            ViewState["SORTDIRECTION1"] = 0;
        BindInvoiceData();
        SetPageNavigator1();
    }

    protected void PagerButtonClick1(object sender, CommandEventArgs ce)
    {
        try
        {
            gvInvoice.SelectedIndex = -1;
            gvInvoice.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER1"] = (int)ViewState["PAGENUMBER1"] - 1;
            else
                ViewState["PAGENUMBER1"] = (int)ViewState["PAGENUMBER1"] + 1;

            BindInvoiceData();
            SetPageNavigator1();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdGo1_Click(object sender, EventArgs e)
    {
        int result;
        try
        {
            if (Int32.TryParse(txtnopage1.Text, out result))
            {
                ViewState["PAGENUMBER1"] = Int32.Parse(txtnopage1.Text);

                if ((int)ViewState["TOTALPAGECOUNT1"] < Int32.Parse(txtnopage1.Text))
                    ViewState["PAGENUMBER1"] = ViewState["TOTALPAGECOUNT1"];

                if (0 >= Int32.Parse(txtnopage1.Text))
                    ViewState["PAGENUMBER1"] = 1;

                if ((int)ViewState["PAGENUMBER1"] == 0)
                    ViewState["PAGENUMBER1"] = 1;

                txtnopage1.Text = ViewState["PAGENUMBER1"].ToString();
            }
            BindInvoiceData();
            SetPageNavigator1();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
