using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class AccountsInterCompanyTransferEntriesCreateVoucher : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Accounts/AccountsInterCompanyTransferEntriesCreateVoucher.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvCreateVoucher')", "Print Grid", "icon_print.png", "PRINT");
        //toolbar.AddImageButton("../Accounts/AccountsInterCompanyTransferEntriesCreateVoucher.aspx", "Find", "search.png", "FIND");
        toolbar.AddImageLink("../Accounts/AccountsInterCompanyTransferEntriesCreateVoucher.aspx", "Add", "add.png", "ADD");
        MenuCreateVoucher.AccessRights = this.ViewState;
        MenuCreateVoucher.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            Session["New"] = "N";
            Session["Rows"] = 1;
            if (Request.QueryString["VOUCHERLINEITEMID"] != null && Request.QueryString["VOUCHERLINEITEMID"] != string.Empty)
                ViewState["VOUCHERLINEITEMID"] = Request.QueryString["VOUCHERLINEITEMID"].ToString();
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Inter-Company Entries", "OFFSETTING");
            MenuCreateVoucherGeneral.AccessRights = this.ViewState;
            MenuCreateVoucherGeneral.MenuList = toolbarmain.Show();          

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
        }
        BindData();
        if (Request.QueryString["VoucherID"] != null && Request.QueryString["VoucherID"] != string.Empty)
        {
            ViewState["VoucherID"] = Request.QueryString["VoucherID"];
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVOUCHERLINEITEMNO", "FLDACCOUNTCODE", "FLDACCOUNTDESCRIPTION", "FLDBUDGETCODE", "FLDCURRENCYNAME", "FLDAMOUNT"
                                 , "FLDBASEEXCHANGERATE", "FLDREPORTEXCHANGERATE", "FLDLONGDESCRIPTION","FLDLONGDESCRIPTION" };

        string[] alCaptions = { "Row Number","Account", "Account Description", "Sub Account","Currency"
                                  ,"Amount","Base Rate","Report Rate","Long Description" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixAccountsOffSettingEntries.VoucherLineItemSearch(
                                           new Guid(ViewState["VOUCHERLINEITEMID"].ToString())
                                           );

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dtLineItem;
            DataSet printDs = new DataSet();
            if (ViewState["VOUCHERLINEITEMID"] != null)
            {
                if (Session["Rows"] != null && Session["Rows"].ToString() == "0")
                {
                    DataTable dt = ds.Tables[0];
                    dt.Clear();
                    ShowNoRecordsFound(dt, gvCreateVoucher);                    
                    printDs.Tables.Add(dt.Copy());
                    Session["printDs"] = printDs;
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInterCompanyTransferEntriesGeneral.aspx";
                }
                else if (Session["VoucherData" + ViewState["VOUCHERLINEITEMID"].ToString()] == null)
                {
                    gvCreateVoucher.DataSource = ds;
                    gvCreateVoucher.DataBind();
                    dtLineItem = ds.Tables[0];
                    dtLineItem.Rows[0]["FLDAMOUNT"] = -Convert.ToDecimal(dtLineItem.Rows[0]["FLDAMOUNT"].ToString());
                    ViewState["CurrencyID"] = dtLineItem.Rows[0]["FLDCURRENCYCODE"].ToString();
                    ViewState["CurrencyName"] = dtLineItem.Rows[0]["FLDCURRENCYNAME"].ToString();
                    Session["VoucherData" + ViewState["VOUCHERLINEITEMID"].ToString()] = dtLineItem;                   
                    printDs.Tables.Add(dtLineItem.Copy());
                    Session["printDs"] = printDs;
                }
                else
                {
                    dtLineItem = (DataTable)Session["VoucherData" + ViewState["VOUCHERLINEITEMID"].ToString()];
                    ViewState["CurrencyID"] = dtLineItem.Rows[0]["FLDCURRENCYCODE"].ToString();
                    ViewState["CurrencyName"] = dtLineItem.Rows[0]["FLDCURRENCYNAME"].ToString();
                    gvCreateVoucher.DataSource = dtLineItem;
                    gvCreateVoucher.DataBind();                    
                    printDs.Tables.Add(dtLineItem.Copy());
                    Session["printDs"] = printDs;
                }
            }
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInterCompanyTransferEntriesGeneral.aspx?VOUCHERLINEITEMID=" + ViewState["VOUCHERLINEITEMID"].ToString() + "&TotalVoucherAmount=" + ViewState["totalamount"].ToString();

        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCreateVoucher);
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInterCompanyTransferEntriesGeneral.aspx";
        }
        General.SetPrintOptions("gvCreateVoucher", "Inter Company Contra Voucher Line Items", alCaptions, alColumns, Session["printDs"] != null ? (DataSet)Session["printDs"] : ds);
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        if (ViewState["totalamount"] == null)
            ViewState["totalamount"] = 0.00;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        //int iTotalPageCount = 0;

        string[] alColumns = { "FLDVOUCHERLINEITEMNO", "FLDACCOUNTCODE", "FLDACCOUNTDESCRIPTION", "FLDBUDGETCODE", "FLDCURRENCYNAME", "FLDAMOUNT"
                                 , "FLDBASEEXCHANGERATE", "FLDREPORTEXCHANGERATE", "FLDLONGDESCRIPTION","FLDLONGDESCRIPTION" };

        string[] alCaptions = { "Row Number","Account", "Account Description", "Sub Account","Currency"
                                  ,"Amount","Base Rate","Report Rate","Long Description" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        
        DataSet ds = PhoenixAccountsOffSettingEntries.VoucherLineItemSearch(
                                           new Guid(ViewState["VOUCHERLINEITEMID"].ToString())
                                           );

        Response.AddHeader("Content-Disposition", "attachment; filename=InterContraVoucherEntriesList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Inter Company Contra Voucher Line Items</h3></td>");
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
        foreach (DataRow dr in (Session["printDs"] != null ? (DataSet)Session["printDs"]: ds).Tables[0].Rows)
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
    protected void MenuCreateVoucher_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
        }
        if (dce.CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (dce.CommandName.ToUpper().Equals("ADD"))
        {
            Response.Redirect("../Accounts/AccountsInterCompanyTransferEntriesAdd.aspx?VOUCHERLINEITEMID=" + ViewState["VOUCHERLINEITEMID"].ToString() + "&VoucherID=" + ViewState["VoucherID"] + "&CurrencyId=" + ViewState["CurrencyID"] + "&BaseRate=" + ViewState["BaseRate"] + "&ReportRate=" + ViewState["ReportRate"] + "&CurrencyName=" + ViewState["CurrencyName"]);
            BindData();
        }
    }
    protected void MenuCreateVoucherGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        UserControlTabs ucTabs = (UserControlTabs)sender;
        try
        {
            //if (dce.CommandName.ToUpper().Equals("CREATEVOUCHER"))
            //{
            //    Response.Redirect("../Accounts/AccountsInterCompanyTransferEntriesCreateVoucher.aspx?VOUCHERLINEITEMID=" + ViewState["VOUCHERLINEITEMID"].ToString());
            //}
            //else
            //{
            //    Response.Redirect("../Accounts/AccountsInterCompanyTransferEntriesList.aspx");
            //}
            if (dce.CommandName.ToUpper().Equals("OFFSETTING"))
            {
                Response.Redirect("../Accounts/AccountsInterCompanyTransferEntriesList.aspx");
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
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void gvCreateVoucher_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvCreateVoucher.SelectedIndex = se.NewSelectedIndex;
        ViewState["VOUCHERLINEITEMID"] = ((Label)gvCreateVoucher.Rows[se.NewSelectedIndex].FindControl("lblVoucherLineId")).Text;
        BindData();
    }
    double total = 0.00;
    protected void gvCreateVoucher_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            total = 0.00;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                Label lblAmount = (Label)e.Row.FindControl("lblAmount");
                Label lblCurrencyCode = (Label)e.Row.FindControl("lblCurrencyCode");
                Label lblBaseRate = (Label)e.Row.FindControl("lblBaseRate");
                Label lblReportRate = (Label)e.Row.FindControl("lblReportRate");               
                if (lblAmount != null && lblAmount.Text != "")
                {
                    total = total + Convert.ToDouble(lblAmount.Text);
                    ViewState["totalamount"] = total;
                    Session["Voucherbal" + ViewState["VOUCHERLINEITEMID"].ToString()] = total;
                }                
                if (lblCurrencyCode != null)
                    ViewState["CurrencyID"] = lblCurrencyCode.Text.ToString();
                if (lblBaseRate != null)
                    ViewState["BaseRate"] = lblBaseRate.Text.ToString();
                if (lblReportRate != null)
                    ViewState["ReportRate"] = lblReportRate.Text.ToString();
            }
        }
    }
    protected void gvCreateVoucher_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }
    protected void gvCreateVoucher_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
    }
    protected void gvCreateVoucher_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            if (!IsValidLineItem(
                                    ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtAmountEdit")).Text
                                  , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtLongDescriptionEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            DataTable dtLineItem = (DataTable)Session["VoucherData" + ViewState["VOUCHERLINEITEMID"].ToString()];
            dtLineItem.Rows[nCurrentRow]["FLDAMOUNT"] = Convert.ToDecimal(((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtAmountEdit")).Text);
            dtLineItem.Rows[nCurrentRow]["FLDLONGDESCRIPTION"] = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtLongDescriptionEdit")).Text;
            Session["VoucherData" + ViewState["VOUCHERLINEITEMID"].ToString()] = dtLineItem;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    public bool IsValidLineItem(string Amount, string LongDescription)
    {
        if (Amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";
        if (LongDescription.Trim().Equals(""))
            ucError.ErrorMessage = "Long Description is required.";

        ucError.HeaderMessage = "Please provide the following required information";
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
        if (Session["New"].ToString() == "Y")
        {
            gvCreateVoucher.SelectedIndex = 0;
            Session["New"] = "N";
            BindPageURL(gvCreateVoucher.SelectedIndex);
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            TextBox tb = ((TextBox)gvCreateVoucher.Rows[rowindex].FindControl("txtInvoiceCode"));
            if (tb != null)
                ViewState["invoicecode"] = tb.Text;
            Label lbl = ((Label)gvCreateVoucher.Rows[rowindex].FindControl("lblInvoiceid"));
            if (lbl != null)
                PhoenixAccountsVoucher.VoucherNumber = lbl.Text;
            //if (ViewState["invoicecode"] != null && ViewState["invoicecode"].ToString() != string.Empty)
            //    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsInvoice.aspx?qinvoicecode=" + ViewState["invoicecode"].ToString();
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
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvCreateVoucher.SelectedIndex = -1;
            gvCreateVoucher.EditIndex = -1;
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


}
