using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Dashboard;

public partial class Accounts_DashboardAccountsOwnerDebitCreditNoteGenerateRegister : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsOwnerDebitCreditNoteGenerateRegister.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvDebitCreditNote')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsOwnerDebitCreditNoteGenerateSearch.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsOwnerDebitCreditNoteGenerate.aspx?OwnerDebitCreditNoteId=')", "Add", "add.png", "ADD");
            toolbargrid.AddImageButton("../Accounts/AccountsOwnerDebitCreditNoteGenerateRegister.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuDebitCreditNoteGrid.AccessRights = this.ViewState;
            MenuDebitCreditNoteGrid.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["principalid"] = "";
                ViewState["Range"] = "";
                ViewState["Code"] = "";
                ViewState["Days"] = "";

                if (Request.QueryString["Code"] != null && Request.QueryString["Code"].ToString() != "")
                    ViewState["Code"] = Request.QueryString["Code"].ToString();

                if (Request.QueryString["Days"] != null && Request.QueryString["Days"].ToString() != "")
                    ViewState["Days"] = Request.QueryString["Days"].ToString();

                if (Request.QueryString["principalid"] != null && Request.QueryString["principalid"].ToString() != "")
                    ViewState["principalid"] = Request.QueryString["principalid"].ToString();

                if (Request.QueryString["Range"] != null && Request.QueryString["Range"].ToString() != "")
                    ViewState["Range"] = Request.QueryString["Range"].ToString();

                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["OwnerDebitCreditNoteId"] = null;
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=OWNERDEBITCREDITNOTE&OwnerDebitCreditNoteId=null&showmenu=0";

                gvDebitCreditNote.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            if (ViewState["Code"].ToString() == "1")
            {
                PhoenixToolbar toolbar1 = new PhoenixToolbar();

                toolbar1.AddButton("Back", "BACK", ToolBarDirection.Right);

                Menuback.AccessRights = this.ViewState;
                Menuback.MenuList = toolbar1.Show();
            }
            else
            {
                Menuback.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDebitCreditNote_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

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
                BindData();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                NameValueCollection nvc = Filter.OwnerDebitCreditNote;
                Filter.OwnerDebitCreditNote = null;
                ViewState["PAGENUMBER"] = 1;
                BindData();

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
        string OwnerDebitCreditNoteId = _gridView.DataKeys[se.NewSelectedIndex].Value.ToString();
        ViewState["OwnerDebitCreditNoteId"] = OwnerDebitCreditNoteId;
        BindData();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSELNAME", "FLDDATE", "FLDREFERENCENUMBER", "FLDSHORTCODE", "FLDTYPENAME", "FLDSUBTYPE", "FLDBANKDESCRIPTION", "FLDLINEITEM", "FLDAMOUNTINUSD", "FLDCURRENCYCODE", "FLDRECEIVEDAMOUNT", "FLDDIFFERENCE", "FLDRECEIVEDDATE", "FLDRECEIVEDSTATUSNAME", "FLDREMARKS", "FLDOPENCLOSE", "FLDVOUCHERNUMBER", "FLDATTACHMENT" };
            string[] alCaptions = { "Vessel", "Date", "Reference No.", "Billing Company", "Type", "SubType", "Bank Receiving Funds", "Line Item", "Amount", "Currency", "Received Amount", "Difference", "Received Date", "Received Status", "Remarks", "Open/Close", "Voucher No.", "Attachment Exists" };

            //NameValueCollection nvc = Filter.OwnerDebitCreditNote;

            DataSet ds = PhoenixDashboardAccounts.Accountdashboardoutstandingfundrequest(
                                                                    General.GetNullableInteger(ViewState["principalid"].ToString())
                                                                 , General.GetNullableInteger(ViewState["Range"].ToString())
                                                                 , (int)ViewState["PAGENUMBER"]
                                                                 , gvDebitCreditNote.PageSize
                                                                 , ref iRowCount
                                                                 , ref iTotalPageCount);

            General.SetPrintOptions("gvDebitCreditNote", "Debit/Credit Note List", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvDebitCreditNote.DataSource = ds;

                DataRow dr = ds.Tables[0].Rows[0];

                if (ViewState["OwnerDebitCreditNoteId"] == null)
                {
                    ViewState["OwnerDebitCreditNoteId"] = dr["FLDOWNERDEBITCREDITNOTEID"].ToString();
                    //gvDebitCreditNote.SelectedIndex = 0;
                }
                SetRowSelection();
            }
            else
            {
                gvDebitCreditNote.DataSource = ds;

            }
            gvDebitCreditNote.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lbl = ((RadLabel)gvDebitCreditNote.Items[rowindex].FindControl("lblDebitCreditNoteId"));
            if (lbl != null)
                ViewState["OwnerDebitCreditNoteId"] = lbl.Text;
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=OWNERDEBITCREDITNOTE&OwnerDebitCreditNoteId=" + ViewState["OwnerDebitCreditNoteId"].ToString() + "&showmenu=0";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {

        for (int i = 0; i < gvDebitCreditNote.Items.Count; i++)
        {
            if (gvDebitCreditNote.MasterTableView.DataKeyValues[i].ToString().Equals(ViewState["OwnerDebitCreditNoteId"].ToString()))
            {
                gvDebitCreditNote.MasterTableView.Items[i].Selected = true;
                break;
            }
        }
        ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=OWNERDEBITCREDITNOTE&OwnerDebitCreditNoteId=" + ViewState["OwnerDebitCreditNoteId"].ToString() + "&showmenu=0";
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDDATE", "FLDREFERENCENUMBER", "FLDSHORTCODE", "FLDTYPENAME", "FLDSUBTYPE", "FLDBANKDESCRIPTION", "FLDLINEITEM", "FLDAMOUNTINUSD", "FLDCURRENCYCODE", "FLDRECEIVEDAMOUNT", "FLDDIFFERENCE", "FLDRECEIVEDDATE", "FLDRECEIVEDSTATUSNAME", "FLDREMARKS", "FLDOPENCLOSE", "FLDVOUCHERNUMBER", "FLDATTACHMENT" };
        string[] alCaptions = { "Vessel", "Date", "Reference No.", "Billing Company", "Type", "Subtype", "Bank Receiving Funds", "Line Item", "Amount", "Currency", "Received Amount", "Difference", "Received Date", "Received Status", "Remarks", "Open/Close", "Voucher No.", "Attachment Exists" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.OwnerDebitCreditNote;

        DataSet ds = PhoenixAccountsOwnerDebitCreditNoteGenerate.OwnerDebitCreditNoteList(
                                                            nvc != null ? General.GetNullableInteger(nvc.Get("ddlVessel")) : null
                                                          , nvc != null ? General.GetNullableDateTime(nvc.Get("ucFromDate")) : null
                                                          , nvc != null ? General.GetNullableDateTime(nvc.Get("ucToDate")) : null
                                                          , nvc != null ? General.GetNullableString(nvc.Get("txtReferenceNo")) : null
                                                          , nvc != null ? General.GetNullableInteger(nvc.Get("ddlBillToCompany")) : null
                                                          , nvc != null ? General.GetNullableInteger(nvc.Get("ddlType")) : null
                                                          , nvc != null ? General.GetNullableInteger(nvc.Get("ddlBank")) : null
                                                          , nvc != null ? General.GetNullableDecimal(nvc.Get("txtFromAmount")) : null
                                                          , nvc != null ? General.GetNullableDecimal(nvc.Get("txtToAmount")) : null
                                                          , nvc != null ? General.GetNullableString(nvc.Get("ddlOpenClose")) : "Open"
                                                          , (int)ViewState["PAGENUMBER"]
                                                          , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                          , ref iRowCount
                                                          , ref iTotalPageCount
                                                          , nvc != null ? General.GetNullableInteger(nvc.Get("ddlSubType")) : null);

        Response.AddHeader("Content-Disposition", "attachment; filename=DebitCreditNote.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Debit/Credit Note List</h3></td>");
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
        BindData();

        if (Session["New"].ToString() == "Y")
        {
            //gvDebitCreditNote.SelectedIndex = 0;
            Session["New"] = "N";
            // BindPageURL(gvDebitCreditNote.SelectedIndex);
        }
    }



    protected void gvDebitCreditNote_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDebitCreditNote.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDebitCreditNote_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

                RadLabel lbl = ((RadLabel)e.Item.FindControl("lblDebitCreditNoteId"));
                if (lbl != null)
                    ViewState["OwnerDebitCreditNoteId"] = lbl.Text;
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=OWNERDEBITCREDITNOTE&OwnerDebitCreditNoteId=" + ViewState["OwnerDebitCreditNoteId"].ToString() + "&showmenu=0";

                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("GETEDIT"))
            {
                RadLabel lbl = ((RadLabel)e.Item.FindControl("lblDebitCreditNoteId"));
                if (lbl != null)
                    ViewState["OwnerDebitCreditNoteId"] = lbl.Text;
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=OWNERDEBITCREDITNOTE&OwnerDebitCreditNoteId=" + ViewState["OwnerDebitCreditNoteId"].ToString() + "&showmenu=0";

                SetRowSelection();
            }

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                RadLabel lbl = ((RadLabel)e.Item.FindControl("lblDebitCreditNoteId"));
                if (lbl != null)
                    ViewState["OwnerDebitCreditNoteId"] = lbl.Text;
                ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=5&reportcode=OWNERDEBITCREDITNOTE&OwnerDebitCreditNoteId=" + ViewState["OwnerDebitCreditNoteId"].ToString() + "&showmenu=0";

                SetRowSelection();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                PhoenixAccountsOwnerDebitCreditNoteGenerate.OwnerDebitCreditNoteReceivedUpdate(new Guid(((RadLabel)e.Item.FindControl("lblDebitCreditNoteId")).Text)
                     , General.GetNullableDecimal(((UserControlDecimal)e.Item.FindControl("txtAmountEdit")).Text)
                     , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucReceivedDate")).Text)
                     , General.GetNullableInteger(((RadComboBox)e.Item.FindControl("ddlReceivedStatus")).SelectedValue)
                     , ((RadTextBox)e.Item.FindControl("txtRemarks")).Text
                     , ((RadComboBox)e.Item.FindControl("ddlOpenClose")).SelectedValue.ToString()
                     , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ucStatus.Text = "Debit/Credit note updated";
                BindData();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {


                PhoenixAccountsOwnerDebitCreditNoteGenerate.OwnerDebitCreditNoteReceiveddelete(new Guid(((RadLabel)e.Item.FindControl("lblDebitCreditNoteId")).Text)
                                                                                    , PhoenixSecurityContext.CurrentSecurityContext.UserCode);

            }
            if (e.CommandName.ToUpper().Equals("HISTORY"))
            {

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

    protected void gvDebitCreditNote_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
        }

        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            ImageButton cmdEditopen = (ImageButton)e.Item.FindControl("cmdEditopen");
            if (cmdEditopen != null) cmdEditopen.Visible = SessionUtil.CanAccess(this.ViewState, cmdEditopen.CommandName);
        }

        if (e.Item is GridDataItem)
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
                RadComboBox ddlReceivedStatus = ((RadComboBox)e.Item.FindControl("ddlReceivedStatus"));
                if (ddlReceivedStatus != null) ddlReceivedStatus.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                ddlReceivedStatus.SelectedValue = lblReceivedStatusEdit.Text.ToString();
            }

            RadLabel lblOpenCloseEdit = (RadLabel)e.Item.FindControl("lblOpenCloseEdit");
            if (lblOpenCloseEdit != null)
            {
                RadComboBox ddlOpenClose = ((RadComboBox)e.Item.FindControl("ddlOpenClose"));
                if (ddlOpenClose != null) //ddlOpenClose.Items.Insert(0, new ListItem("--Select--", ""));
                    ddlOpenClose.SelectedValue = lblOpenCloseEdit.Text.ToString();
            }

            RadLabel lblDebitCreditNoteId = (RadLabel)e.Item.FindControl("lblDebitCreditNoteId");
            ImageButton cmdEditopen = (ImageButton)e.Item.FindControl("cmdEditopen");
            if (cmdEditopen != null)
            {
                cmdEditopen.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsOwnerDebitCreditNoteGenerate.aspx?OwnerDebitCreditNoteId=" + lblDebitCreditNoteId.Text + "'); return true;");
            }
            ImageButton cmdHistory = (ImageButton)e.Item.FindControl("cmdHistory");
            if (cmdHistory != null)
            {
                cmdHistory.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsOwnerFundRequestHistory.aspx?DebitCreditNoteIdItem=" + lblDebitCreditNoteId.Text + "'); return true;");
            }

            RadLabel lblOpenClose = (RadLabel)e.Item.FindControl("lblOpenClose");

            if (lblOpenClose != null && lblOpenClose.Text == "Close")
            {
                if (cmdEditopen != null) cmdEditopen.Visible = false;
            }

            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.ACCOUNTS + "'); return false;");
            }
        }

    }

    protected void Menuback_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper() == "BACK")
        {
            Response.Redirect("../Accounts/AccountsDashboardOutStandingFundsRequest.aspx?Days=" + ViewState["Days"].ToString(), true);
        }

    }
}
