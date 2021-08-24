using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsOtherOwnerFundRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddButton("Office Debit/Credit Note", "LIST", ToolBarDirection.Right);
            //MenuDebitCreditNote.AccessRights = this.ViewState;
            //MenuDebitCreditNote.MenuList = toolbar.Show();
            //MenuDebitCreditNote.SelectedMenuIndex = 0;

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsOtherOwnerFundRequest.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvDebitCreditNote')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsOtherOwnerFundRequestSearch.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsOtherOwnerFundRequestAdd.aspx?OfficeDebitCreditNoteId=')", "Add", "add.png", "ADD");
            MenuDebitCreditNoteGrid.AccessRights = this.ViewState;
            MenuDebitCreditNoteGrid.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["OfficeDebitCreditNoteId"] = null;
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=OFFICEDEBITCREDITNOTE&OfficeDebitCreditNoteId=null&showmenu=0";
            }
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    //protected void MenuDebitCreditNote_TabStripCommand(object sender, EventArgs e)
    //{
    //    RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
    //    string CommandName = ((RadToolBarButton)dce.Item).CommandName;

    //    try
    //    {

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void MenuDebitCreditNoteGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ViewState["PAGENUMBER"] = 1;
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDebitCreditNote_ItemDataBound(object sender, GridItemEventArgs e)
    {

        ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        if (e.Item is GridDataItem)
        {
            //if (!e.Item.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridEditableItem)
        {
            RadLabel lblLineItem = (RadLabel)e.Item.FindControl("lblLineItem");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTip");
            if (lblLineItem != null && lblLineItem.Text != "")
            {
                lblLineItem.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
                lblLineItem.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");
            }

            RadLabel lblReceivedStatusEdit = (RadLabel)e.Item.FindControl("lblReceivedStatusEdit");
            if (lblReceivedStatusEdit != null)
            {
                DropDownList ddlReceivedStatus = ((DropDownList)e.Item.FindControl("ddlReceivedStatus"));
                if (ddlReceivedStatus != null) ddlReceivedStatus.Items.Insert(0, new ListItem("--Select--", ""));
                ddlReceivedStatus.SelectedValue = lblReceivedStatusEdit.Text.ToString();
            }

            RadLabel lblOpenCloseEdit = (RadLabel)e.Item.FindControl("lblOpenCloseEdit");
            if (lblOpenCloseEdit != null)
            {
                RadComboBox ddlOpenClose = ((RadComboBox)e.Item.FindControl("ddlOpenClose"));
                if (ddlOpenClose != null) ddlOpenClose.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                ddlOpenClose.SelectedValue = lblOpenCloseEdit.Text.ToString();
            }

            RadLabel lblDebitCreditNoteId = (RadLabel)e.Item.FindControl("lblDebitCreditNoteId");
            ImageButton cmdEditopen = (ImageButton)e.Item.FindControl("cmdEditopen");
            if (cmdEditopen != null)
            {
                cmdEditopen.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsOtherOwnerFundRequestAdd.aspx?OfficeDebitCreditNoteId=" + lblDebitCreditNoteId.Text + "'); return true;");
            }

            RadLabel lblOpenClose = (RadLabel)e.Item.FindControl("lblOpenClose");

            if (lblOpenClose != null && lblOpenClose.Text == "Close")
            {
                if (cmdEditopen != null) cmdEditopen.Visible = false;
            }
            else
            {
                if (cmdEditopen != null) cmdEditopen.Visible = true;
            }
            ImageButton noAtt = (ImageButton)e.Item.FindControl("cmdNoAttachment");
            ImageButton att = (ImageButton)e.Item.FindControl("cmdAttachment"); if (drv["FLDATTACHMENTCOUNT"].ToString() != "0")
            {
                if (att != null)
                {
                    att.Visible = true;
                    noAtt.Visible = false;
                    att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=ACCOUNTS&type=&cmdname='); return true;");
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                }
            }
            else
            {

                if (noAtt != null)
                {
                    att.Visible = false;
                    noAtt.Visible = true;
                    noAtt.Attributes.Add("onclick", "javascript:openNewWindow('noAtt','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=ACCOUNTS&type=&cmdname='); return true;");
                    //cmdNoAttachment.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey + "&mod="
                    //                    + PhoenixModule.ACCOUNTS + "');return true;");
                    noAtt.Visible = SessionUtil.CanAccess(this.ViewState, noAtt.CommandName);
                }
            }
        }
    }

    protected void gvDebitCreditNote_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            //RadGrid _gridView = (RadGrid)sender;
            //int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("GETEDIT"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                //if (!IsValidLineItem(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescriptionEdit")).Text
                //                        , ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtAmountEdit")).Text))
                //{
                //    ucError.Visible = true;
                //    return;
                //}
                RadComboBox ddl = ((RadComboBox)e.Item.FindControl("ddlOpenClose"));

                if (ddl != null && ddl.SelectedIndex > 0)
                {
                    PhoenixAccountsOfficeDebitCreditNoteGenerate.OfficeDebitCreditNoteReceivedUpdate(new Guid(((RadLabel)e.Item.FindControl("lblDebitCreditNoteId")).Text)
                         , General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text)
                         , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucReceivedDate")).Text)
                         , ((RadTextBox)e.Item.FindControl("txtRemarks")).Text
                         , ((RadComboBox)e.Item.FindControl("ddlOpenClose")).SelectedValue.ToString()
                         , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                    ucStatus.Text = "Debit/Credit note updated";
                    Rebind();
                }
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {

                PhoenixAccountsOfficeDebitCreditNoteGenerate.OfficeDebitCreditNoteReceiveddelete(new Guid(((RadLabel)e.Item.FindControl("lblDebitCreditNoteId")).Text)
                                                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode);

                ucStatus.Text = "Deleted Successfully";
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDebitCreditNote_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        GridView _gridView = sender as GridView;
        _gridView.SelectedIndex = se.NewSelectedIndex;
        string OfficeDebitCreditNoteId = _gridView.DataKeys[se.NewSelectedIndex].Value.ToString();
        ViewState["OfficeDebitCreditNoteId"] = OfficeDebitCreditNoteId;
        Rebind();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDSUBJECT", "FLDDATE", "FLDREFERENCENUMBER", "FLDSHORTCODE", "FLDTYPENAME", "FLDBANKDESCRIPTION", "FLDLINEITEM", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDRECEIVEDAMOUNT", "FLDDIFFERENCE", "FLDRECEIVEDDATE", "FLDRECEIVEDSTATUSNAME", "FLDREMARKS", "FLDOPENCLOSE", "FLDVOUCHERNUMBER", "FLDATTACHMENT" };
            string[] alCaptions = { "Subject", "Date", "Reference No.", "Billing Company", "Type", "Bank Receiving Funds", "	Line Item", "Currecny", "Amount", "Received Amount", "Difference", "Received Date", "Received Status", "Remarks", "Open/Close", "Voucher No.", "Attachment Exists" };

            NameValueCollection nvc = Filter.OfficeDebitCreditNote;

            DataSet ds = PhoenixAccountsOfficeDebitCreditNoteGenerate.OtherOwnerFundDebitCreditNoteList(nvc != null ? General.GetNullableString(nvc.Get("txtSubject")) : null
                                                              , nvc != null ? General.GetNullableDateTime(nvc.Get("ucFromDate")) : null
                                                              , nvc != null ? General.GetNullableDateTime(nvc.Get("ucToDate")) : null
                                                              , nvc != null ? General.GetNullableString(nvc.Get("txtReferenceNo")) : null
                                                              , nvc != null ? General.GetNullableInteger(nvc.Get("ddlBillToCompany")) : null
                                                              , nvc != null ? General.GetNullableInteger(nvc.Get("ddlBank")) : null
                                                              , nvc != null ? General.GetNullableInteger(nvc.Get("ddlType")) : null
                                                              , nvc != null ? General.GetNullableDecimal(nvc.Get("txtFromAmount")) : null
                                                              , nvc != null ? General.GetNullableDecimal(nvc.Get("txtToAmount")) : null
                                                              , nvc != null ? General.GetNullableInteger(nvc.Get("ucCurrencyCode")) : null
                                                              , nvc != null ? General.GetNullableString(nvc.Get("txtReceivedStatus")) : null
                                                              , nvc != null ? General.GetNullableString(nvc.Get("ddlOpenClose")) : "Open"
                                                              , gvDebitCreditNote.CurrentPageIndex + 1
                                                              , gvDebitCreditNote.PageSize
                                                              , ref iRowCount
                                                              , ref iTotalPageCount);

            General.SetPrintOptions("gvDebitCreditNote", "Other Owner Debit/Credit Note List", alCaptions, alColumns, ds);

            gvDebitCreditNote.DataSource = ds;
            gvDebitCreditNote.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                if (ViewState["OfficeDebitCreditNoteId"] == null)
                {
                    ViewState["OfficeDebitCreditNoteId"] = dr["FLDOFFICEDEBITCREDITNOTEID"].ToString();
                    //gvDebitCreditNote.SelectedIndex = 0;
                }
                SetRowSelection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                //  ShowNoRecordsFound(dt, gvDebitCreditNote);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lbl = ((RadLabel)gvDebitCreditNote.Items[rowindex].FindControl("lblDebitCreditNoteId"));
            if (lbl != null)
                ViewState["OfficeDebitCreditNoteId"] = lbl.Text;
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=OFFICEDEBITCREDITNOTE&OfficeDebitCreditNoteId=" + ViewState["OfficeDebitCreditNoteId"].ToString() + "&showmenu=0";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        gvDebitCreditNote.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvDebitCreditNote.Items)
        {
            if (item.GetDataKeyValue("FLDOFFICEDEBITCREDITNOTEID").ToString().Equals(ViewState["OfficeDebitCreditNoteId"].ToString()))
            {
                gvDebitCreditNote.SelectedIndexes.Add(item.ItemIndex);
            }
        }
        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=OFFICEDEBITCREDITNOTE&OfficeDebitCreditNoteId=" + ViewState["OfficeDebitCreditNoteId"].ToString() + "&showmenu=0";
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSUBJECT", "FLDDATE", "FLDREFERENCENUMBER", "FLDSHORTCODE", "FLDTYPENAME", "FLDBANKDESCRIPTION", "FLDLINEITEM", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDRECEIVEDAMOUNT", "FLDDIFFERENCE", "FLDRECEIVEDDATE", "FLDRECEIVEDSTATUSNAME", "FLDREMARKS", "FLDOPENCLOSE", "FLDVOUCHERNUMBER", "FLDATTACHMENT" };
        string[] alCaptions = { "Subject", "Date", "Reference No.", "Billing Company", "Type", "Bank Receiving Funds", "	Line Item", "Currecny", "Amount", "Received Amount", "Difference", "Received Date", "Received Status", "Remarks", "Open/Close", "Voucher No.", "Attachment Exists" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.OfficeDebitCreditNote;

        DataSet ds = PhoenixAccountsOfficeDebitCreditNoteGenerate.OtherOwnerFundDebitCreditNoteList(nvc != null ? General.GetNullableString(nvc.Get("txtSubject")) : null
                                                          , nvc != null ? General.GetNullableDateTime(nvc.Get("ucFromDate")) : null
                                                          , nvc != null ? General.GetNullableDateTime(nvc.Get("ucToDate")) : null
                                                          , nvc != null ? General.GetNullableString(nvc.Get("txtReferenceNo")) : null
                                                          , nvc != null ? General.GetNullableInteger(nvc.Get("ddlBillToCompany")) : null
                                                          , nvc != null ? General.GetNullableInteger(nvc.Get("ddlBank")) : null
                                                          , nvc != null ? General.GetNullableInteger(nvc.Get("ddlType")) : null
                                                          , nvc != null ? General.GetNullableDecimal(nvc.Get("txtFromAmount")) : null
                                                          , nvc != null ? General.GetNullableDecimal(nvc.Get("txtToAmount")) : null
                                                          , nvc != null ? General.GetNullableInteger(nvc.Get("ucCurrencyCode")) : null
                                                          , nvc != null ? General.GetNullableString(nvc.Get("txtReceivedStatus")) : null
                                                          , nvc != null ? General.GetNullableString(nvc.Get("ddlOpenClose")) : "Open"
                                                          , (int)ViewState["PAGENUMBER"]
                                                          , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                          , ref iRowCount
                                                          , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=OtherOwnerDebitCreditNote.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Other Owner Debit/Credit Note List</h3></td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvDebitCreditNote.Rebind();
    }

    protected void Rebind()
    {
        gvDebitCreditNote.SelectedIndexes.Clear();
        gvDebitCreditNote.EditIndexes.Clear();
        gvDebitCreditNote.DataSource = null;
        gvDebitCreditNote.Rebind();
    }

    protected void gvDebitCreditNote_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDebitCreditNote.CurrentPageIndex + 1;
        BindData();
    }

}