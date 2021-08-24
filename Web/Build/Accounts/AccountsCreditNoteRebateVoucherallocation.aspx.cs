using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Collections.Specialized;
using System.Collections;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class Accounts_AccountsCreditNoteRebateVoucherallocation : PhoenixBasePage
{
    public decimal allocatingAmount = 0;
    public decimal writeOffAmount = 0;
    public decimal unAllocatedAmount = 0;
    public decimal balanceAmount = 0;

    public string strAllocatingAmount = string.Empty;
    public string strWriteoffAmount = string.Empty;
    public string strUnAllocatingAmount = string.Empty;
    public string strBalanceAmount = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Accounts/AccountsCreditNoteRebateVoucherallocation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");

            // toolbargrid.AddImageLink("../Accounts/AccountsCreditNoteRebateVoucherallocation.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvRebateReceivableUnallocated')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageLink("../Accounts/AccountsCreditNoteRebateVoucherallocation.aspx", "Find", "search.png", "FIND");
            MenuRebateReceivableAllocate.AccessRights = this.ViewState;
            MenuRebateReceivableAllocate.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbargridallocated = new PhoenixToolbar();
            toolbargridallocated.AddImageButton("../Accounts/AccountsCreditNoteRebateVoucherallocation.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargridallocated.AddImageLink("javascript:CallPrint('gvRebateReceivableallocated')", "Print Grid", "icon_print.png", "PRINT");

            MenuRebateReceivableAllocated.AccessRights = this.ViewState;
            MenuRebateReceivableAllocated.MenuList = toolbargridallocated.Show();
            // MenuRebateReceivableAllocated.SetTrigger(pnlStockItemEntry);


            // MenuRebateReceivableAllocate.SetTrigger(pnlStockItemEntry);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["creditdebitnoteid"] = Request.QueryString["creditdebitnoteid"];
                BindDataMain();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRebateReceivableAllocated_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelAllocated();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRebateReceivableAllocate_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelUnallocated();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvRebateReceivableUnallocated.Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRebateReceivableVoucherTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("ALLOCATE SELECTED VOUCHERS"))
            {
                StringBuilder strallocatingvoucherlineid = new StringBuilder();
                strallocatingvoucherlineid.Append(",");

                foreach (GridDataItem row in gvRebateReceivableUnallocated.MasterTableView.Items)
                {
                    RadCheckBox chk1 = (RadCheckBox)row.FindControl("chkIncludeyn");

                    if (chk1 != null && chk1.Checked == true)
                    {
                        string strtemp;
                        strtemp = ((Label)row.FindControl("lblVoucherlineitemid")).Text.ToString();
                        strallocatingvoucherlineid.Append(((Label)row.FindControl("lblVoucherlineitemid")).Text.ToString());
                        strallocatingvoucherlineid.Append(",");

                    }
                }
                if (strallocatingvoucherlineid.Length > 1)
                {
                    PhoenixAccountsCreditDebitNoteRebateReceivable.DebitNoteRebateReceivableAllocatedVouchers(strallocatingvoucherlineid.ToString(), int.Parse(ViewState["currencycode"].ToString()),
                                                                                                           int.Parse(ViewState["suppliercode"].ToString()),
                                                                                                           PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                                           new Guid(ViewState["creditdebitnoteid"].ToString()),
                                                                                                           PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                }

                gvRebateReceivableallocated.Rebind();
                gvRebateReceivableUnallocated.Rebind();

            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelUnallocated();
            }
        }

        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void gvRebateReceivableUnallocated_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRebateReceivableUnallocated.CurrentPageIndex + 1;
            binddata();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRebateReceivableallocated_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            //ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRebateReceivableallocated.CurrentPageIndex + 1;
            bindallocatedvouchers();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataMain()
    {
        DataSet ds = PhoenixAccountsCreditDebitNoteRebateReceivable.RebateReceivableMain(new Guid(ViewState["creditdebitnoteid"].ToString()));

        DataRow dr = ds.Tables[0].Rows[0];

        txtCreditdebitnoteid.Text = ViewState["creditdebitnoteid"].ToString();
        txtsuppliercode.Text = dr["FLDCODE"].ToString() + "/" + dr["FLDNAME"].ToString();
        txtcurrency.Text = dr["FLDCURRENCYCODE"].ToString() + "/" + string.Format(String.Format("{0:###,###,###.00}", dr["FLDAMOUNT"]));
        ViewState["suppliercode"] = dr["FLDSUPPLIERCODE"].ToString();
        ViewState["currencycode"] = dr["FLDCURRENCYID"].ToString();
        ViewState["currencyName"] = dr["FLDCURRENCYCODE"].ToString();
        ViewState["CompanyId"] = dr["FLDCOMPANYID"].ToString();
    }

    private void binddata()
    {
        //int iRowCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDINVOICESUPPLIERREFERENCE", "FLDINVOICESTATUS", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDUNALLOCATEDAMOUNT" };
        string[] alCaptions = { "Voucher No", "Voucher Reference", "Invoice Status", "Currency", "Amount", "Unallocated Amount", };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsCreditDebitNoteRebateReceivable.DebitNoteRebateReceivableUnallocatedSearch(new Guid(ViewState["creditdebitnoteid"].ToString()),
                                                                                                        int.Parse(ViewState["suppliercode"].ToString()),
                                                                                                         int.Parse(ViewState["currencycode"].ToString()),
                                                                                                         int.Parse(ViewState["CompanyId"].ToString()),
                                                                                                         General.GetNullableString(txtvouchernumber.Text));
        General.SetPrintOptions("gvRebateReceivableUnallocated", "Rebate Receivable Unallocated", alCaptions, alColumns, ds);


        gvRebateReceivableUnallocated.DataSource = ds;
        gvRebateReceivableUnallocated.VirtualItemCount = ds.Tables[0].Rows.Count;


    }

    protected void ShowExcelAllocated()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDVOUCHERREFNO", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDUNALLOCATEDAMOUNT", "FLDWRITEOFFAMOUNT", "FLDALLOCATINGAMOUNT", "FLDBALANCE" };
        string[] alCaptions = { "Voucher No", "Voucher Reference", "Currency", "Amount", "Unallocated Amount", "Write Off Amount", "Allocating Amount", "Balance" };
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        NameValueCollection nvc = Filter.CurrentSelectedERMVoucher;

        ds = PhoenixAccountsCreditDebitNoteRebateReceivable.DebitNoteRebateReceivableAllocatedList(new Guid(txtCreditdebitnoteid.Text)
                                                                                                     , 1
                                                                                                     , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                                                                     , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ERMVoucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Rebate Receivable Allocated</h3></td>");
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

    protected void ShowExcelUnallocated()
    {
        int iRowCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDINVOICESUPPLIERREFERENCE", "FLDINVOICESTATUS", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDUNALLOCATEDAMOUNT" };
        string[] alCaptions = { "Voucher No", "Voucher Reference", "Invoice Status", "Currency", "Amount", "Unallocated Amount", };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentSelectedERMVoucher;

        ds = PhoenixAccountsCreditDebitNoteRebateReceivable.DebitNoteRebateReceivableUnallocatedSearch(new Guid(ViewState["creditdebitnoteid"].ToString()),
                                                                                                        int.Parse(ViewState["suppliercode"].ToString()),
                                                                                                         int.Parse(ViewState["currencycode"].ToString()),
                                                                                                         int.Parse(ViewState["CompanyId"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=ERMVoucher.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Rebate Receivable Unallocated</h3></td>");
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

    private void bindallocatedvouchers()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDVOUCHERNUMBER", "FLDVOUCHERREFNO", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDUNALLOCATEDAMOUNT", "FLDWRITEOFFAMOUNT", "FLDALLOCATINGAMOUNT", "FLDBALANCE" };
        string[] alCaptions = { "Voucher No", "Voucher Reference", "Currency", "Amount", "Unallocated Amount", "Write Off Amount", "Allocating Amount", "Balance" };
        DataSet ds = new DataSet();

        ds = PhoenixAccountsCreditDebitNoteRebateReceivable.DebitNoteRebateReceivableAllocatedList(new Guid(ViewState["creditdebitnoteid"].ToString())
                                                                                                     , gvRebateReceivableallocated.CurrentPageIndex + 1
                                                                                                     , gvRebateReceivableallocated.PageSize
                                                                                                     , ref iRowCount, ref iTotalPageCount
                                                                                                    );



        allocatingAmount = 0;
        writeOffAmount = 0;
        unAllocatedAmount = 0;
        balanceAmount = 0;

        gvRebateReceivableallocated.DataSource = ds;
        gvRebateReceivableallocated.VirtualItemCount = iRowCount;

        DataTable dt1 = ds.Tables[0];
        foreach (DataRow row in dt1.Rows)
        {
            row["FLDAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDAMOUNT"].ToString()));
            //row["FLDWRITEOFFAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDWRITEOFFAMOUNT"]));
            row["FLDALLOCATINGAMOUNT"] = string.Format(String.Format("{0:#####.00}", row["FLDALLOCATINGAMOUNT"]));
        }


        General.SetPrintOptions("gvRebateReceivableallocated", "Rebate Receivable Allocated", alCaptions, alColumns, ds);
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        //String script = "javascript:fnReloadList('codehelp1',null,'true');";
        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    protected void gvRebateReceivableUnallocated_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadLabel lblCheckActive = (RadLabel)e.Item.FindControl("lblCheckActive");
                ImageButton cmdallocated = (ImageButton)e.Item.FindControl("cmdallocated");
                RadLabel lblCurrency = (RadLabel)e.Item.FindControl("lblCurrency");

                if (lblCheckActive.Text == "1")
                {
                    cmdallocated.Enabled = false;
                }
                else
                {
                    cmdallocated.Enabled = true;
                }
                if (lblCurrency.Text == ViewState["currencyName"].ToString())
                {
                    cmdallocated.Visible = true;
                }
                else
                {
                    cmdallocated.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvRebateReceivableUnallocated_Itemcommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ALLOCATE"))
            {

                RadLabel strallocatingvoucherlineid = ((RadLabel)e.Item.FindControl("lblVoucherlineitemid"));
                PhoenixAccountsCreditDebitNoteRebateReceivable.DebitNoteRebateReceivableAllocatedAmountouchers(General.GetNullableGuid(strallocatingvoucherlineid.Text), int.Parse(ViewState["currencycode"].ToString()),
                                                                                                            int.Parse(ViewState["suppliercode"].ToString()),
                                                                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                                            new Guid(ViewState["creditdebitnoteid"].ToString()),
                                                                                                            PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                gvRebateReceivableallocated.Rebind();
                gvRebateReceivableUnallocated.Rebind();

            }
            String script = "javascript:fnReloadList('codehelp1',null,'true');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvRebateReceivableallocated_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadLabel lblUnallocatedAmount = (RadLabel)e.Item.FindControl("lblUnallocatedAmount");
                RadLabel lblWriteoffamount = (RadLabel)e.Item.FindControl("lblWriteoffamount");
                RadLabel lblAllocatingAmount = (RadLabel)e.Item.FindControl("lblAllocatingAmount");
                RadLabel lblBalance = (RadLabel)e.Item.FindControl("lblBalance");

                if (lblUnallocatedAmount != null && lblUnallocatedAmount.Text != string.Empty)
                    unAllocatedAmount = unAllocatedAmount + Convert.ToDecimal(lblUnallocatedAmount.Text);

                if (lblWriteoffamount != null && lblWriteoffamount.Text != string.Empty)
                    writeOffAmount = writeOffAmount + Convert.ToDecimal(lblWriteoffamount.Text);

                if (lblAllocatingAmount != null && lblAllocatingAmount.Text != string.Empty)
                    allocatingAmount = allocatingAmount + Convert.ToDecimal(lblAllocatingAmount.Text);

                if (lblBalance != null && lblBalance.Text != string.Empty)
                    balanceAmount = balanceAmount + Convert.ToDecimal(lblBalance.Text);

            }
            strAllocatingAmount = allocatingAmount.ToString();
            strWriteoffAmount = writeOffAmount.ToString();
            strUnAllocatingAmount = unAllocatedAmount.ToString();
            strBalanceAmount = balanceAmount.ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvRebateReceivableallocated_Itemcommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidMain(((RadLabel)e.Item.FindControl("lblAmount")).Text,
                                ((TextBox)e.Item.FindControl("txtallocateamountedit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsCreditDebitNoteRebateReceivable.DebitNoteRebateReceivableAllocatedupdate(new Guid(((RadLabel)e.Item.FindControl("lblVoucherlineitemallocatedid")).Text)
                                                                                                       , General.GetNullableDecimal(((TextBox)e.Item.FindControl("txtwriteoffedit")).Text)
                                                                                                       , General.GetNullableDecimal(((TextBox)e.Item.FindControl("txtallocateamountedit")).Text)
                                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.UserCode);


                unAllocatedAmount = 0;
                writeOffAmount = 0;
                allocatingAmount = 0;

                RebindAllocated();
                gvRebateReceivableUnallocated.Rebind();
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsCreditDebitNoteRebateReceivable.DebitNoteRebateReceivableAllocateddelete(new Guid(((RadLabel)e.Item.FindControl("lblVoucherlineitemallocatedid")).Text),
                                                                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                unAllocatedAmount = 0;
                writeOffAmount = 0;
                allocatingAmount = 0;

                gvRebateReceivableallocated.Rebind();
                gvRebateReceivableUnallocated.Rebind();
            }

            String script = "javascript:fnReloadList('codehelp1',null,'true');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RebindAllocated()
    {
        gvRebateReceivableallocated.SelectedIndexes.Clear();
        gvRebateReceivableallocated.EditIndexes.Clear();
        gvRebateReceivableallocated.DataSource = null;
        gvRebateReceivableallocated.Rebind();
    }
    //protected void RebindUnAllocated()
    //{
    //    gvRebateReceivableUnallocated.SelectedIndexes.Clear();
    //    gvRebateReceivableUnallocated.EditIndexes.Clear();
    //    gvRebateReceivableUnallocated.DataSource = null;
    //    gvRebateReceivableUnallocated.Rebind();
    //}
    private bool IsValidMain(string Amount, string allocatedAmount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((Convert.ToDecimal(Amount)) < (Convert.ToDecimal(allocatedAmount)))
            ucError.ErrorMessage = "Allocated Amount is Greater than the Amount is required.";

        return (!ucError.IsError);
    }
}
