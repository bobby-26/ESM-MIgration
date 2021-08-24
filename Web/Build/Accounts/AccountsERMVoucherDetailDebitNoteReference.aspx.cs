using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsERMVoucherDetailDebitNoteReference : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsERMVoucherDetailDebitNoteReference.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCountry')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("../Accounts/AccountsERMVoucherFilter.aspx?callfrom=Billed", "Find", "search.png", "FIND");
            toolbar.AddImageLink("../Accounts/AccountsERMVoucherAdvancedFilter.aspx?callfrom=Billedadvanced", "Advance Find", "search.png", "FINDADVANCED");
            toolbar.AddImageButton("../Accounts/AccountsERMVoucherDetailDebitNoteReference.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuRegistersCountry.AccessRights = this.ViewState;
            MenuRegistersCountry.MenuList = toolbar.Show();
        
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvCountry.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
          
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
       
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDACCOUNTCODE", "FLDACCOUNTDESCRIPTION", "FLDESMBUDGETCODE", "FLDBUDGETCODEDESCRIPTION", "FLDOWNERBUDGETCODE", "FLDVOUCHERDATE", "FLDDEBITNOTEREFERENCE", "FLDPHOENIXVOUCHER", "FLDREFERENCE", "FLDAMOUNT", "FLDREPORTINGAMOUNT", "FLDDESCRIPTION", "FLDINCULDEINOWNERREPORT", "FLDSHOWINSUMMARYBALANCE", "FLDISATTACHMENTINEXCEL" };
        string[] alCaptions = { "Account", "Account Description", "Budget Code", "Budget Code Description", "Owner Budget Code", "Voucher Date", "Debit Note Reference", "Phoenix Voucher", "Reference", "Amount", "Report Amount", "Description", "Included in Owner SOA", "Show Separately in the Vessel Summary Balance", "Contains Attachments" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        var month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        DateTime first = month.AddMonths(-1);
        DateTime last = month.AddTicks(-1);

        NameValueCollection nvc = Filter.CurrentSelectedERMVoucher;
        if (nvc == null || nvc.Get("SearchType").ToUpper().Equals("FIND"))
        {
            ds = PhoenixAccountsERMVoucherDetail.AccountsBilledVouchersSearch(
                nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherDateFrom")) : General.GetNullableDateTime(first.ToString())
                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherDateTo")) : General.GetNullableDateTime(last.ToString())
                , nvc != null ? General.GetNullableString(nvc.Get("txtEsmBudgetCode")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtOwnerBudgetCode")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCode")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtDebitNoteReference")) : null
                , 0
                , (int)ViewState["PAGENUMBER"],
                 General.ShowRecords(null) * -1,
                ref iRowCount,
                ref iTotalPageCount
                , sortexpression
                , sortdirection);
        }
        else
        {
            ds = PhoenixAccountsERMVoucherDetail.AccountsBilledVouchersAdvancedSearch(
                nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherDateFrom")) : General.GetNullableDateTime(first.ToString())
                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherDateTo")) : General.GetNullableDateTime(last.ToString())
                , nvc != null ? General.GetNullableString(nvc.Get("txtEsmBudgetCode")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtOwnerBudgetCode")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCode")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtDebitNoteReference")) : null
                , 0
                , (int)ViewState["PAGENUMBER"],
                 General.ShowRecords(null) * -1,
                ref iRowCount,
                ref iTotalPageCount
                , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumber")) : null
                , sortexpression
                , sortdirection
                , nvc != null ? General.GetNullableInteger(nvc.Get("chkShowAll")) : 0);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=BilledVouchers.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Billed Vouchers</h3></td>");
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

    protected void RegistersCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                //gvCountry.EditIndex = -1;
                //gvCountry.SelectedIndex = -1;
                //ViewState["PAGENUMBER"] = 1;
                //BindData();
                //SetPageNavigator();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                NameValueCollection criteria = new NameValueCollection();

                criteria.Clear();
                criteria.Add("txtEsmBudgetCode", string.Empty);
                criteria.Add("txtOwnerBudgetCode", string.Empty);
                criteria.Add("txtVoucherDateFrom", string.Empty);
                criteria.Add("txtVoucherDateTo", string.Empty);
                criteria.Add("txtAccountCode", string.Empty);
                criteria.Add("txtDebitNoteReference", string.Empty);
                criteria.Add("txtVoucherNumber", string.Empty);
                criteria.Add("chkShowAll", "0");
                criteria.Add("SearchType", "FIND");

                Filter.CurrentSelectedERMVoucher = criteria;

                BindData();
                
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
        string[] alColumns = { "FLDACCOUNTCODE", "FLDACCOUNTDESCRIPTION", "FLDESMBUDGETCODE", "FLDBUDGETCODEDESCRIPTION", "FLDOWNERBUDGETCODE", "FLDVOUCHERDATE", "FLDDEBITNOTEREFERENCE", "FLDPHOENIXVOUCHER", "FLDREFERENCE", "FLDAMOUNT", "FLDREPORTINGAMOUNT", "FLDDESCRIPTION", "FLDINCULDEINOWNERREPORT", "FLDSHOWINSUMMARYBALANCE", "FLDISATTACHMENTINEXCEL" };
        string[] alCaptions = { "Account", "Account Description", "Budget Code", "Budget Code Description", "Owner Budget Code", "Voucher Date", "Debit Note Reference", "Phoenix Voucher", "Reference", "Amount", "Report Amount", "Description", "Included in Owner SOA", "Show Separately in the Vessel Summary Balance", "Contains Attachments" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        var month = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        DateTime first = month.AddMonths(-1);
        DateTime last = month.AddTicks(-1);

        DataSet ds = new DataSet();
        
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentSelectedERMVoucher;
        if (nvc == null || nvc.Get("SearchType").ToUpper().Equals("FIND"))
        {
            ds = PhoenixAccountsERMVoucherDetail.AccountsBilledVouchersSearch(
                nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherDateFrom")) : General.GetNullableDateTime(first.ToString())
                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherDateTo")) : General.GetNullableDateTime(last.ToString())
                , nvc != null ? General.GetNullableString(nvc.Get("txtEsmBudgetCode")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtOwnerBudgetCode")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCode")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtDebitNoteReference")) : null
                , 0
                , (int)ViewState["PAGENUMBER"],
                gvCountry.PageSize,
                ref iRowCount,
                ref iTotalPageCount
                , sortexpression
                , sortdirection);
        }
        else
        {
            ds = PhoenixAccountsERMVoucherDetail.AccountsBilledVouchersAdvancedSearch(
                nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherDateFrom")) : General.GetNullableDateTime(first.ToString())
                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtVoucherDateTo")) : General.GetNullableDateTime(last.ToString())
                , nvc != null ? General.GetNullableString(nvc.Get("txtEsmBudgetCode")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtOwnerBudgetCode")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCode")) : null
                , nvc != null ? General.GetNullableString(nvc.Get("txtDebitNoteReference")) : null
                , 0
                , (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount
                , nvc != null ? General.GetNullableString(nvc.Get("txtVoucherNumber")) : null
                , sortexpression
                , sortdirection
                , nvc != null ? General.GetNullableInteger(nvc.Get("chkShowAll")) : 0);
        }
        
        General.SetPrintOptions("gvCountry", "Country", alCaptions, alColumns, ds);
        gvCountry.DataSource = ds;
        gvCountry.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

   
    
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
      
        BindData();
        gvCountry.Rebind();
    }



    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetKeyDownScroll(object sender, GridItemEventArgs e)
    {
        int nextRow = 0;
        RadGrid _gridView = (RadGrid)sender;

        if (e.Item is GridDataItem
            && (e.Item.Equals(DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Item.Equals(DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Item.RowIndex == 0) ? nRows : e.Item.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Item.RowIndex == nRows) ? 0 : e.Item.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Item.Attributes["onkeydown"] = script;
        }

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvCountry.Rebind();
    }

    protected void gvCountry_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCountry.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCountry_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

           
            if (e.CommandName.ToUpper().Equals("ADD"))
            {

            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                PhoenixAccountsERMVoucherDetail.ERMVoucherDetailUpdate(new Guid(((RadLabel)e.Item.FindControl("lblVoucherDetailIdEdit")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtAccountCodeEdit")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtESMBudgetCodeEdit")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtOwnerBudgetCodeEdit")).Text.ToString())
                                                                        , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtVoucherDate")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDebitNoteReferenceEdit")).Text.ToString())
                                                                        , General.GetNullableString(((RadLabel)e.Item.FindControl("lblPhoenixVoucherEdit")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtReferenceEdit")).Text.ToString())
                                                                        , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtAmount")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtAccountDescriptionEdit")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtBudgetCodeDescriptionEdit")).Text.ToString())
                                                                        , General.GetNullableInteger(((CheckBox)e.Item.FindControl("chkIncludeSOA")).Checked ? "1" : "0")
                                                                        , General.GetNullableInteger(((CheckBox)e.Item.FindControl("chkShowinSummaryBalance")).Checked ? "1" : "0")
                                                                        );

               
                BindData();
                gvCountry.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsERMVoucherDetail.ERMDebitNoteReferenceDelete(new Guid(((RadLabel)e.Item.FindControl("lblVoucherDetailId")).Text.ToString())
                    , General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblIsPhoenixVoucher")).Text)
                    );
                gvCountry.Rebind();
            }

            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvCountry_ItemDataBound1(object sender, GridItemEventArgs e)
    {

       
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            int? n = General.GetNullableInteger(drv["FLDISPHOENIXVOUCHER"].ToString());

            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            if (eb != null && n == 1)
                eb.Visible = false;

            ImageButton sb = (ImageButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

            }

            ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
            ImageButton ibtnShowOwnerBudgetEdit = (ImageButton)e.Item.FindControl("btnShowOwnerBudgetEdit");
           RadTextBox txtOwnerBudgetNameEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
           RadTextBox txtOwnerBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
            RadTextBox txtOwnerBudgetgroupIdEdit = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
            if (txtOwnerBudgetNameEdit != null)
                txtOwnerBudgetNameEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetIdEdit != null)
                txtOwnerBudgetIdEdit.Attributes.Add("style", "visibility:hidden");
            if (txtOwnerBudgetgroupIdEdit != null)
                txtOwnerBudgetgroupIdEdit.Attributes.Add("style", "visibility:hidden");
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel lblbudgetid = (RadLabel)e.Item.FindControl("lblBudgetId");

            if (ibtnShowOwnerBudgetEdit != null && lblvesselid != null && lblbudgetid != null)
            {
                ibtnShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + lblvesselid.Text.ToString() + "&budgetid=" + lblbudgetid.Text + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");         //+ "&budgetid=" + lblbudgetid.Text       
                if (!SessionUtil.CanAccess(this.ViewState, ibtnShowOwnerBudgetEdit.CommandName)) ibtnShowOwnerBudgetEdit.Visible = false;
            }

            /* Voucher attachments.. */
            ImageButton cmdAttachment = (ImageButton)e.Item.FindControl("cmdAttachment");
            ImageButton cmdNoAttachment = (ImageButton)e.Item.FindControl("cmdNoAttachment");
            RadLabel lblDtKey = (RadLabel)e.Item.FindControl("lblDtKey");
            RadLabel lblVoucherLineItemId = (RadLabel)e.Item.FindControl("lblVoucherLineItemId");
            RadLabel lblVoucherDetailId = (RadLabel)e.Item.FindControl("lblVoucherDetailId");

            if (drv["FLDISATTACHMENT"].ToString() == "1")
            {
                if (cmdAttachment != null)
                {
                    if (lblVoucherLineItemId != null && General.GetNullableGuid(lblVoucherLineItemId.Text) != null)
                    {
                        cmdAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','"+Session["sitepath"]+"/Accounts/AccountsVoucherAttachments.aspx?dtkey="
                        + lblDtKey.Text + "&qvoucherlineid=" + lblVoucherLineItemId.Text + "&voucherdetailid=" + lblVoucherDetailId.Text + "');return true;");
                    }
                    else
                    {
                        cmdAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Accounts/AccountsVoucherAttachments.aspx?dtkey="
                        + lblDtKey.Text + "&voucherdetailid=" + lblVoucherDetailId.Text + "');return true;");
                    }

                    cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
                }
            }
            else
            {
                if (cmdNoAttachment != null)
                {
                    cmdNoAttachment.Visible = true;

                    if (lblVoucherLineItemId != null && General.GetNullableGuid(lblVoucherLineItemId.Text) != null)
                    {
                        cmdNoAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','"+Session["sitepath"] +"/Accounts/AccountsVoucherAttachments.aspx?dtkey="
                        + lblDtKey.Text + "&qvoucherlineid=" + lblVoucherLineItemId.Text + "&voucherdetailid=" + lblVoucherDetailId.Text + "');return true;");
                    }
                    else
                    {
                        cmdNoAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Accounts/AccountsVoucherAttachments.aspx?dtkey="
                        + lblDtKey.Text + "&voucherdetailid=" + lblVoucherDetailId.Text + "');return true;");
                    }
                    //cmdNoAttachment.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                    //                    + PhoenixModule.ACCOUNTS + "');return true;");
                    cmdNoAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdNoAttachment.CommandName);
                }
            }
            //ImageButton iab = (ImageButton)e.Item.FindControl("cmdAttachment");
            //ImageButton inab = (ImageButton)e.Item.FindControl("cmdNoAttachment");
            //if (iab != null) iab.Visible = true;
            //if (inab != null) inab.Visible = false;
            //int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());
            //if (n == 0)
            //{
            //    if (iab != null) iab.Visible = false;
            //    if (inab != null) inab.Visible = true;
            //}
        }
        if (e.Item is GridDataItem
        && !e.Item.Equals(DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        && !e.Item.Equals(DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Item.TabIndex = -1;
            //e.Item.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCountry, "Edit$" + e.Item.RowIndex.ToString(), false);
            SetKeyDownScroll(sender, e);
        }

       

    }

    protected void gvCountry_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem && 
            !e.Item.Equals(DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
         && !e.Item.Equals(DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Item.TabIndex = -1;
            //e.Item.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCountry, "Edit$" + e.Item.RowIndex.ToString(), false);
        }

        SetKeyDownScroll(sender, e);
    }

    protected void gvCountry_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           
            PhoenixAccountsERMVoucherDetail.ERMVoucherDetailUpdate(new Guid(((RadLabel)e.Item.FindControl("lblVoucherDetailIdEdit")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtAccountCodeEdit")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtESMBudgetCodeEdit")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtOwnerBudgetCodeEdit")).Text.ToString())
                                                                        , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtVoucherDate")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDebitNoteReferenceEdit")).Text.ToString())
                                                                        , General.GetNullableString(((RadLabel)e.Item.FindControl("lblPhoenixVoucherEdit")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtReferenceEdit")).Text.ToString())
                                                                        , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtAmount")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtAccountDescriptionEdit")).Text.ToString())
                                                                        , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtBudgetCodeDescriptionEdit")).Text.ToString())
                                                                        , General.GetNullableInteger(((CheckBox)e.Item.FindControl("chkIncludeSOA")).Checked ? "1" : "0")
                                                                        , General.GetNullableInteger(((CheckBox)e.Item.FindControl("chkShowinSummaryBalance")).Checked ? "1" : "0")
                                                                        );


         
            BindData();
            gvCountry.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCountry_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
