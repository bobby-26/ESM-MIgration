using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsErmDebitNoteReference : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvDebitReference.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvDebitReference.UniqueID, "Edit$" + r.RowIndex.ToString());
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsErmDebitNoteReference.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvDebitReference')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("../Accounts/AccountsErmDebitNoteReference.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Accounts/AccountsErmDebitNoteReference.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuDebitReference.AccessRights = this.ViewState;
            MenuDebitReference.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                gvDebitReference.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("History", "HISTORY",ToolBarDirection.Right);
            MenuFormMain.AccessRights = this.ViewState;
            MenuFormMain.MenuList = toolbarmain.Show();

         
        }
        catch (Exception ex)
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

        string[] alColumns = { "FLDACCOUNTCODE", "FLDACCOUNTCODEDESCRIPTION", "FLDTYPE", "FLDHARDNAME", "FLDQUICKNAME", "FLDDEBITNOTEREFERENCE", "FLDSTATUS", "FLDURL" };
        string[] alCaptions = { "Account Code", "Account Description", "Type", "Month", "Year", "Debit Note Reference", "Status", "URL"};
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
             , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableString(txtDebitRefernce.Text),
            General.GetNullableString(rblStatus.SelectedValue));


        Response.AddHeader("Content-Disposition", "attachment; filename=StatementReference.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Statement Reference</h3></td>");
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

    protected void MenuFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("HISTORY") && ViewState["DebitNoteReferenceid"] != null && ViewState["DebitNoteReferenceid"].ToString() != string.Empty)
            {
                Response.Redirect("../Accounts/AccountsDebitNoteReferenceHistory.aspx?DebitNoteReferenceid=" + ViewState["DebitNoteReferenceid"] + "&qfrom=DebitNoteReference");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDebitReference_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
              
                ViewState["PAGENUMBER"] = 1;
                //BindData();
                gvDebitReference.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtDebitRefernce.Text = "";
                rblStatus.SelectedValue = "In Progress";

                //BindData();
                gvDebitReference.Rebind();
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
        string[] alColumns = { "FLDACCOUNTCODE", "FLDACCOUNTCODEDESCRIPTION", "FLDTYPE", "FLDHARDNAME", "FLDQUICKNAME", "FLDDEBITNOTEREFERENCE", "FLDSTATUS", "FLDURL" };
        string[] alCaptions = { "Account Code", "Account Description", "Type", "Month", "Year", "Debit Note Reference", "Status", "URL" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        DataSet ds = new DataSet();

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
             , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
          gvDebitReference.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
           General.GetNullableString(txtDebitRefernce.Text),
            General.GetNullableString(rblStatus.SelectedValue));

        General.SetPrintOptions("gvDebitReference", "Statement Reference", alCaptions, alColumns, ds);
        gvDebitReference.DataSource = ds;
        gvDebitReference.VirtualItemCount = iRowCount;
       
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

 
   
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
       
        //  BindData();
        gvDebitReference.Rebind();
    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    private bool IsValidData(string accountcode, string type, string month, string year,string reference )
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(accountcode) == null)
            ucError.ErrorMessage = "Account code is required.";
        if (type== "Dummy")
            ucError.ErrorMessage = "Type is required.";
        if (General.GetNullableInteger(month) == null)
            ucError.ErrorMessage = "Month is required.";
        if (General.GetNullableInteger(year) == null)
            ucError.ErrorMessage = "Year is required.";
        if (General.GetNullableString(reference) == null)
            ucError.ErrorMessage = "Debit note reference is required.";

        return (!ucError.IsError);
    }

    private void SetRowSelection()
    {
        for (int i = 0; i < gvDebitReference.Items.Count; i++)
        {
            if (gvDebitReference.MasterTableView.DataKeyValues[i].ToString().Equals(ViewState["DebitNoteReferenceid"].ToString()))
            {
                gvDebitReference.MasterTableView.Items[i].Selected = true;
                break;
            }
        }

      
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lbl = ((RadLabel)gvDebitReference.Items[rowindex].FindControl("lblDebitReferenceId"));
            if (lbl != null)
                ViewState["DebitNoteReferenceid"] = lbl.Text;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvDebitReference_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDebitReference.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDebitReference_ItemDataBound1(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Item.FindControl("cmdDelete");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            ImageButton img1stCheck = (ImageButton)e.Item.FindControl("img1stCheck");
            ImageButton img2ndCheck = (ImageButton)e.Item.FindControl("img2ndCheck");
            ImageButton cmdPublished = (ImageButton)e.Item.FindControl("cmdPublished");
            ImageButton cmdUnPublished = (ImageButton)e.Item.FindControl("cmdUnPublished");


            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

            }

            ImageButton ibtnshowAccount = (ImageButton)e.Item.FindControl("imgShowAccount");
            if (ibtnshowAccount != null)
                ibtnshowAccount.Attributes.Add("onclick", "return showPickList('spnPickListCreditAccount', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCompanyAccount.aspx', true); ");


            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (eb != null && (drv["FLDSTATUS"].ToString().ToUpper().Equals("FINALIZED") || drv["FLDSTATUS"].ToString().ToUpper().Equals("CANCELLED")) || drv["FLDSTATUS"].ToString().ToUpper().Equals("PUBLISHED"))
            {
                eb.Visible = false;
            }

            if (cb != null && (drv["FLDSTATUS"].ToString().ToUpper().Equals("PUBLISHED") || drv["FLDSTATUS"].ToString().ToUpper().Equals("CANCELLED")))
            {
                cb.Visible = false;
                if (img1stCheck != null)
                    img1stCheck.Visible = false;
                if (img2ndCheck != null)
                    img2ndCheck.Visible = false;
                if (cmdPublished != null)
                {
                    cmdPublished.Visible = false;
                    if (drv["FLDSTATUS"].ToString().ToUpper().Equals("PUBLISHED"))
                    {
                        cmdUnPublished.Visible = true;
                    }
                }

            }

            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','"+Session["sitepath"]+"/Accounts/AccountsStatementRefFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                  + PhoenixModule.ACCOUNTS + "&Status=" + drv["FLDSTATUS"].ToString() + "&debitnotereferenceid=" + drv["FLDDEBINOTEREFERENCEID"].ToString() + "'); return true;");
                //att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                //    + PhoenixModule.ACCOUNTS + "&Status=" + drv["FLDSTATUS"].ToString() + "'); return false;");
            }

            RadComboBox ddlStatus = (RadComboBox)e.Item.FindControl("ddlStatusEdit");

            if (drv != null && ddlStatus != null)
            {
                ddlStatus.SelectedValue = drv["FLDSTATUS"].ToString();
            }
            RadComboBox ddlTypeEdit = (RadComboBox)e.Item.FindControl("ddlTypeEdit");
            UserControlHard ucMonthEdit = (UserControlHard)e.Item.FindControl("ucMonthEdit");
            UserControlQuick ucYearEdit = (UserControlQuick)e.Item.FindControl("ucYearEdit");

            if (drv != null && ddlTypeEdit != null)
            {
                ddlTypeEdit.SelectedValue = drv["FLDTYPE"].ToString();
            }
            if (drv != null && ucMonthEdit != null)
                ucMonthEdit.SelectedHard = drv["FLDMONTH"].ToString();

            if (drv != null && ucYearEdit != null)
                ucYearEdit.SelectedQuick = drv["FLDYEAR"].ToString();


            RadLabel lblURL = (RadLabel)e.Item.FindControl("lblURL");
            UserControlToolTip ucToolTiplblURL = (UserControlToolTip)e.Item.FindControl("ucToolTiplblURL");

            if (lblURL != null)
            {
                lblURL.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTiplblURL.ToolTip + "', 'visible');");
                lblURL.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTiplblURL.ToolTip + "', 'hidden');");
            }

            //ImageButton img1stCheck = (ImageButton)e.Item.FindControl("img1stCheck");
            if (img1stCheck != null)
            {
                if (General.GetNullableDateTime(drv["FLD1STCHECKDATE"].ToString()) == null)
                {
                    img1stCheck.Visible = true;
                }
                else
                    img1stCheck.Visible = false;
            }

           // ImageButton img2ndCheck = (ImageButton)e.Item.FindControl("img2ndCheck");
            if (img2ndCheck != null)
            {
                if (General.GetNullableDateTime(drv["FLD1STCHECKDATE"].ToString()) != null
                    && General.GetNullableDateTime(drv["FLD2NDCHECKDATE"].ToString()) == null)
                {
                    img2ndCheck.Visible = true;
                }
                else
                    img2ndCheck.Visible = false;
            }

            ImageButton imgPublised = (ImageButton)e.Item.FindControl("cmdPublished");
            ImageButton imgedit = (ImageButton)e.Item.FindControl("cmdEdit");

            if (imgPublised != null)
            {
                if (General.GetNullableDateTime(drv["FLD1STCHECKDATE"].ToString()) != null
                     && General.GetNullableDateTime(drv["FLD2NDCHECKDATE"].ToString()) != null
                    && General.GetNullableDateTime(drv["FLDPUBLISHEDDATE"].ToString()) == null)
                {
                    imgPublised.Visible = true;
                }
                else
                {
                    imgPublised.Visible = false;

                }
            }
            ImageButton imgcancel = (ImageButton)e.Item.FindControl("cmdCancel");
            if (imgcancel != null)
            {
                if (General.GetNullableString(drv["FLDSTATUS"].ToString()) == "CANCELLED")
                {
                    imgedit.Visible = false;
                }
            }

           

            if (General.GetNullableString(drv["FLDSTATUS"].ToString()) == "Published")
            {
                imgedit.Visible = false;
            }

        }

        if (e.Item is GridFooterItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            UserControlHard ddlHard = (UserControlHard)e.Item.FindControl("ucMonth");
            ddlHard.HardList = PhoenixRegistersHard.ListHard(1, 55, byte.Parse("1"), "");

            ImageButton ibtnshowAccount = (ImageButton)e.Item.FindControl("imgShowAccount");
            if (ibtnshowAccount != null)
                ibtnshowAccount.Attributes.Add("onclick", "return showPickList('spnPickListCreditAccount', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCompanyAccount.aspx', true); ");


            DateTime curDateTime = DateTime.Now;
            string year = curDateTime.Year.ToString();
            string quickcode = "";

            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();

            ds = PhoenixCommonRegisters.QuickSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 55, year, null, null, (int)ViewState["PAGENUMBER"], 1, ref iRowCount,  ref iTotalPageCount);
            DataRow dr = ds.Tables[0].Rows[0];
            quickcode = dr["FLDQUICKCODE"].ToString();

            UserControlQuick ucYear = (UserControlQuick)e.Item.FindControl("ucYear");
            ucYear.bind();
            ucYear.SelectedQuick = quickcode;
        }
    }

    protected void gvDebitReference_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

          
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((RadTextBox)e.Item.FindControl("txtAccountCode")).Text
                                    , ((RadComboBox)e.Item.FindControl("ddlType")).SelectedValue
                                    , ((UserControlHard)e.Item.FindControl("ucMonth")).SelectedHard
                                    , ((UserControlQuick)e.Item.FindControl("ucYear")).SelectedQuick
                                    , ((RadTextBox)e.Item.FindControl("txtDebitNoteRef")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsDebitNoteReference.DebitNoteReferenceInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , ((RadTextBox)e.Item.FindControl("txtAccountCode")).Text
                                                                            , ((RadComboBox)e.Item.FindControl("ddlType")).SelectedValue
                                                                            , General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucMonth")).SelectedHard)
                                                                            , ((RadTextBox)e.Item.FindControl("txtDebitNoteRef")).Text
                                                                            , ((RadComboBox)e.Item.FindControl("ddlStatusAdd")).SelectedValue
                                                                            , General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucYear")).SelectedQuick)
                                                                            , ((RadTextBox)e.Item.FindControl("txtURL")).Text);
                gvDebitReference.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData(((RadTextBox)e.Item.FindControl("txtAccountCodeEdit")).Text
                    , ((RadComboBox)e.Item.FindControl("ddlTypeEdit")).SelectedValue
                    , ((UserControlHard)e.Item.FindControl("ucMonthEdit")).SelectedHard
                    , ((UserControlQuick)e.Item.FindControl("ucYearEdit")).SelectedQuick
                    , ((RadTextBox)e.Item.FindControl("txtDebitReferenceEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsDebitNoteReference.DebitNoteReferenceUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text)
                                                                            , ((RadTextBox)e.Item.FindControl("txtAccountCodeEdit")).Text
                                                                            , ((RadComboBox)e.Item.FindControl("ddlTypeEdit")).SelectedValue
                                                                            , General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucMonthEdit")).SelectedHard)
                                                                            , ((RadTextBox)e.Item.FindControl("txtDebitReferenceEdit")).Text
                                                                            , General.GetNullableInteger(((UserControlQuick)e.Item.FindControl("ucYearEdit")).SelectedQuick)
                                                                            , ((RadTextBox)e.Item.FindControl("txtURLEdit")).Text);


                gvDebitReference.SelectedIndexes.Clear();
                gvDebitReference.EditIndexes.Clear();
                gvDebitReference.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsERMVoucherDetail.ERMVoucherdelete(((RadLabel)e.Item.FindControl("lblDebitNoteRef")).Text
                                                               , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text)
                                                               , PhoenixSecurityContext.CurrentSecurityContext.UserCode);

                // PhoenixAccountsERMVoucherDetail.ERMVoucherDetailDelete(new Guid(((RadLabel)e.Item.FindControl("lblVoucherDetailId")).Text.ToString()));
            }

            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                int iRowno;
                iRowno = int.Parse(e.CommandArgument.ToString());
                BindPageURL(iRowno);
                SetRowSelection();
            }

            else if (e.CommandName.ToUpper().Equals("1STCHECK"))
            {

                PhoenixAccountsDebitNoteReference.DebitNoteReference1stCheckUpdate(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));

                
                //BindData();
                gvDebitReference.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("2NDCHECK"))
            {

                PhoenixAccountsDebitNoteReference.DebitNoteReference2ndCheckUpdate(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));

                
                //BindData();
                gvDebitReference.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("PUBLISHED"))
            {
                PhoenixAccountsDebitNoteReference.DebitNoteReferencePublish(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));

            
                //BindData();
                gvDebitReference.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("UNPUBLISHED"))
            {
                PhoenixAccountsDebitNoteReference.DebitNoteReferenceUNPublish(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));

          
                //BindData();
                gvDebitReference.Rebind();
                ucStatus.Text = "Debit Note Reference Unpublished.";
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
}
