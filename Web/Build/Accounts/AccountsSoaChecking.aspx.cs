using System;
using System.Data;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using System.Text;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class Accounts_AccountsSoaChecking : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsSoaChecking.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvOwnersAccount')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsSoaChecking.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageButton("../Accounts/AccountsSoaChecking.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("SOA", "SOA");
            //toolbarmain.AddButton("Line Items", "LINEITEMS");
            toolbarmain.AddButton("Remove old attachments", "REMOVEOLDATTACHMENTS", ToolBarDirection.Right);
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbarmain.Show();
            MenuGeneral.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                vesselAccount();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["DEBITNOTEREFERENCEID"] = null;

                gvOwnersAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
    protected void Rebind()
    {
        gvOwnersAccount.SelectedIndexes.Clear();
        gvOwnersAccount.EditIndexes.Clear();
        gvOwnersAccount.DataSource = null;
        gvOwnersAccount.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDDEBITNOTEREFERENCE", "FLDTYPE", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDNAME", "FLDHARDNAME", "FLDQUICKNAME", "FLDSOASTATUSNAME" };
        string[] alCaptions = { "Statement Reference", "Type", "", "", "Billing Party", "Year", "Month", "Status" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string status = null;
        if (Convert.ToString(ddlstatus.SelectedValue) != "0")
        {
            status = Convert.ToString(ddlstatus.SelectedItem);
        }

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsSoaChecking.StatementOfAccountsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , sortexpression, sortdirection,
                                                (int)ViewState["PAGENUMBER"],
                                                PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                ref iRowCount,
                                                ref iTotalPageCount,
                                                General.GetNullableInteger(rblAccount.SelectedValue),
                                                General.GetNullableInteger(ucMonth.SelectedHard),
                                                 General.GetNullableInteger(ucYear.SelectedValue),
                                                 status
                                                );


        Response.AddHeader("Content-Disposition", "attachment; filename=SOAChecking.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>SOA Checking</center></h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("REMOVEOLDATTACHMENTS"))
        {
            string filepath;
            StringBuilder strdtkey = new StringBuilder();
            foreach (GridDataItem gvr in gvOwnersAccount.Items)
            {
                RadLabel lbldtkey = (RadLabel)gvr.FindControl("lbldtkey");
                ViewState["lbldtkey"] = lbldtkey.Text;
                filepath = HttpContext.Current.Request.MapPath("~/Attachments/Accounts/" + ViewState["lbldtkey"].ToString() + ".pdf");
                if (File.Exists(filepath))
                {
                    File.Delete(filepath);
                }
            }
        }
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

                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {

                ucYear.SelectedText = "";
                ucMonth.SelectedName = "";
                ddlstatus.SelectedIndex = 0;
                ViewState["ACCOUNTID"] = null;
                txtAccountSearch.Text = "";
                cmdSearchAccount_Click(this, new EventArgs());
                Rebind();

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

        string[] alColumns = { "FLDDEBITNOTEREFERENCE", "FLDTYPE", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDNAME", "FLDHARDNAME", "FLDQUICKNAME", "FLDSOASTATUSNAME" };
        string[] alCaptions = { "Statement Reference", "Type", "Account", "Account Description", "Billing Party", "Year", "Month", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        string status = null;
        if (ddlstatus.SelectedIndex > 0)
        {
            status = Convert.ToString(ddlstatus.SelectedItem.Text);
        }

        DataSet ds = new DataSet();

        ds = PhoenixAccountsSoaChecking.StatementOfAccountsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , sortexpression, sortdirection,
                                                 (int)ViewState["PAGENUMBER"],
                                                 gvOwnersAccount.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount,
                                                  General.GetNullableInteger(rblAccount.SelectedValue),
                                                General.GetNullableInteger(ucMonth.SelectedHard),
                                                 General.GetNullableInteger(ucYear.SelectedValue),
                                                 status);

        gvOwnersAccount.DataSource = ds;
        gvOwnersAccount.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOwnersAccount", "Order Form List", alCaptions, alColumns, ds);
    }



    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();

    }


    //protected void gvDebitReference_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{

    //    if (e.Item.RowType == DataControlRowType.DataRow)
    //    {

    //        ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
    //        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

    //        ImageButton sb = (ImageButton)e.Item.FindControl("cmdSave");
    //        if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

    //        ImageButton cb = (ImageButton)e.Item.FindControl("cmdCancel");
    //        if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

    //        ImageButton img1stCheck = (ImageButton)e.Item.FindControl("img1stCheck");
    //        ImageButton img2ndCheck = (ImageButton)e.Item.FindControl("img2ndCheck");
    //        ImageButton cmdPublished = (ImageButton)e.Item.FindControl("cmdPublished");
    //        ImageButton cmdUnPublished = (ImageButton)e.Item.FindControl("cmdUnPublished");          

    //        ImageButton ibtnshowAccount = (ImageButton)e.Item.FindControl("imgShowAccount");
    //        if (ibtnshowAccount != null)
    //            ibtnshowAccount.Attributes.Add("onclick", "return showPickList('spnPickListCreditAccount', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCompanyAccount.aspx', true); ");


    //        DataRowView drv = (DataRowView)e.Item.DataItem;

    //        if (eb != null && (drv["FLDSTATUS"].ToString().ToUpper().Equals("FINALIZED") || drv["FLDSTATUS"].ToString().ToUpper().Equals("CANCELLED")))
    //        {
    //            eb.Visible = false;
    //        }

    //        if (cb != null && (drv["FLDSTATUS"].ToString().ToUpper().Equals("PUBLISHED") || drv["FLDSTATUS"].ToString().ToUpper().Equals("CANCELLED")))
    //        {
    //            cb.Visible = false;
    //            if (img1stCheck != null)
    //                img1stCheck.Visible = false;
    //            if (img2ndCheck != null)
    //                img2ndCheck.Visible = false;
    //            if (cmdPublished != null)
    //            {
    //                cmdPublished.Visible = false;
    //                cmdUnPublished.Visible = true;
    //            }

    //        }

    //        ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
    //        if (att != null)
    //        {
    //            att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
    //            if (drv["FLDISATTACHMENT"].ToString() == "0")
    //                att.ImageUrl = Session["images"] + "/no-attachment.png";
    //            att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
    //                + PhoenixModule.ACCOUNTS + "'); return false;");
    //        }        

    //    }
    //}

    protected void cmdSearchAccount_Click(object sender, EventArgs e)
    {
        vesselAccount();
        Rebind();
        ViewState["ACCOUNTID"] = rblAccount.SelectedValue.ToString();


    }

    protected void vesselAccount()
    {
        DataSet ds = new DataSet();

        try
        {

            ds = PhoenixRegistersAccount.vesselaccount(txtAccountSearch.Text);

            ds.Tables[0].Columns.Add("FLDaccoandept");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr["FLDaccoandept"] = dr["FLDACCOUNTCODE"] + "-" + dr["FLDDESCRIPTION"];

            }

            rblAccount.DataTextField = "FLDaccoandept";
            rblAccount.DataValueField = "FLDACCOUNTID";
            rblAccount.DataSource = ds;
            rblAccount.DataBind();
            if (ViewState["ACCOUNTID"] != null && ViewState["ACCOUNTID"].ToString() != string.Empty)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {

                    if ((dr["FLDACCOUNTID"].ToString().Trim()) == (ViewState["ACCOUNTID"].ToString().Trim()))
                    {
                        rblAccount.SelectedValue = ViewState["ACCOUNTID"].ToString();
                    }

                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }
    protected void AccountSelection(object sender, EventArgs e)
    {
        Rebind();
    }

    protected void gvOwnersAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOwnersAccount.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOwnersAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                RadLabel lblStatementReference = (RadLabel)e.Item.FindControl("lblStatementReference");
                RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
                // RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
                RadLabel lblDebitReferenceId = (RadLabel)e.Item.FindControl("lblDebitReferenceId");
                RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");

                Filter.CurrentOwnerBudgetCodeSelection = null;

                if (lblDebitReferenceId != null)
                    Response.Redirect("../Accounts/AccountsSoaCheckingLineItems.aspx?debitnotereference=" + lblStatementReference.Text + "&accountid=" + lblAccountId.Text + "&Ownerid=" + lblOwnerid.Text + "&VesselId=" + null + "&debitnoteid=" + lblDebitReferenceId.Text + "&Status=" + lblStatus.Text, true);

            }
            else if (e.CommandName.ToUpper().Equals("1STCHECK"))
            {

                PhoenixAccountsDebitNoteReference.DebitNoteReference1stCheckUpdate(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));

               
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("2NDCHECK"))
            {

                PhoenixAccountsDebitNoteReference.DebitNoteReference2ndCheckUpdate(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));

            Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("PUBLISHED"))
            {
                PhoenixAccountsDebitNoteReference.DebitNoteReferencePublish(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));

               Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UNPUBLISHED"))
            {
                PhoenixAccountsDebitNoteReference.DebitNoteReferenceUNPublish(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));

               Rebind();
                ucStatus.Text = "Debit Note Reference Unpublished.";
            }

            else if (e.CommandName.ToUpper().Equals("UNTAGVOUCHERS"))
            {
                PhoenixAccountsSoaChecking.UnTagAllVoucherLineItems(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));
            }
            else if (e.CommandName.ToUpper().Equals("RESET"))
            {
                PhoenixAccountsSOAGeneration.SOAGenerationPendingUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblDebitReferenceId")).Text));

              Rebind();
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

    protected void gvOwnersAccount_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
            RadLabel lbldtkey = (RadLabel)e.Item.FindControl("lbldtkey");
            RadLabel lblAccountCodeDescription = (RadLabel)e.Item.FindControl("lblAccountCodeDescription");
            RadLabel lblYear = (RadLabel)e.Item.FindControl("lblYear");
            RadLabel lblMonth = (RadLabel)e.Item.FindControl("lblMonthNumber");
            RadLabel lblDebitReferenceId = (RadLabel)e.Item.FindControl("lblDebitReferenceId");

            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkAccountCOde");

            LinkButton lnkStatementReference = (LinkButton)e.Item.FindControl("lnkStatementReference");


            ImageButton cmdAttachment = (ImageButton)e.Item.FindControl("cmdAttachment");
            if (cmdAttachment != null)
            {
                cmdAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Accounts/AccountsSoaCheckingPdfReport.aspx?pdfGen=0&debitnotereference=" + lnkStatementReference.Text + "&debitnotereferenceid=" + lblDebitReferenceId.Text + "&accountid="
                                    + lblAccountId.Text + "&Ownerid=" + lblOwnerid.Text + "&dtkey=" + lbldtkey.Text + "&description=" + lblAccountCodeDescription.Text + "&year=" + lblYear.Text + "&month=" + lblMonth.Text + " &Type=0" + "');return true;");
                cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            }


            ImageButton cmdAttachmentESM = (ImageButton)e.Item.FindControl("cmdAttachmentESM");
            if (cmdAttachmentESM != null)
            {
                cmdAttachmentESM.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Accounts/AccountsSoaCheckingESMBudgetCodePdfReport.aspx?pdfGen=0&debitnotereference=" + lnkStatementReference.Text + "&debitnotereferenceid=" + lblDebitReferenceId.Text + "&accountid="
                                    + lblAccountId.Text + "&Ownerid=" + lblOwnerid.Text + "&dtkey=" + lbldtkey.Text + "&description=" + lblAccountCodeDescription.Text + "&year=" + lblYear.Text + "&month=" + lblMonth.Text + " &Type=1" + "');return true;");
                cmdAttachmentESM.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachmentESM.CommandName);
            }

            ImageButton cmdMoreInfo = (ImageButton)e.Item.FindControl("cmdMoreInfo");
            if (cmdMoreInfo != null)
            {
                cmdMoreInfo.Attributes.Add("onclick", "parent.openNewWindow('codeHelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsSoaCheckingpdfAttachment.aspx?debitnotereferenceid=" + lblDebitReferenceId.Text + "');return false;");
            }


            ImageButton cb = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            ImageButton cmdPublished = (ImageButton)e.Item.FindControl("cmdPublished");
            ImageButton cmdUnPublished = (ImageButton)e.Item.FindControl("cmdUnPublished");


            DataRowView drv = (DataRowView)e.Item.DataItem;

            if ((drv["FLDSOASTATUSNAME"].ToString().ToUpper().Equals("PUBLISHED") || drv["FLDSOASTATUSNAME"].ToString().ToUpper().Equals("CANCELLED")))
            {
                if (cmdPublished != null)
                {
                    cmdPublished.Visible = false;
                    if (drv["FLDSOASTATUSNAME"].ToString().ToUpper().Equals("PUBLISHED"))
                    {
                        cmdUnPublished.Visible = true;
                    }
                }
            }


            ImageButton cmdGeneratePdf = (ImageButton)e.Item.FindControl("imgGeneratePdf");
            if (cmdGeneratePdf != null) cmdGeneratePdf.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Accounts/AccountsSoaCheckingSubledgerType.aspx?pdfGen=1&debitnotereference=" + lnkStatementReference.Text + "&debitnotereferenceid=" + lblDebitReferenceId.Text + "&accountid="
                                    + lblAccountId.Text + "&Ownerid=" + lblOwnerid.Text + "&dtkey=" + lbldtkey.Text + "&description=" + lblAccountCodeDescription.Text + "&year=" + lblYear.Text + "&month=" + lblMonth.Text + "&URL=" + drv["FLDURL"].ToString() + "');return true;");

            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");

            if (cmdDelete != null)
            {
                cmdDelete.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Accounts/AccountsSoaCheckingPdfReport.aspx?pdfGen=&debitnotereference=" + lnkStatementReference.Text + "&accountid="
                                    + lblAccountId.Text + "&Ownerid=" + lblOwnerid.Text + "&dtkey=" + lbldtkey.Text + "&description=" + lblAccountCodeDescription.Text + "&year=" + lblYear.Text + "&month=" + lblMonth.Text + "');return true;");
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            }

            ImageButton cmdReset = (ImageButton)e.Item.FindControl("cmdReset");
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                if (cmdReset != null)
                {
                    if (drv["FLDSOASTATUSNAME"].ToString() == "1st Level Checked" || drv["FLDSOASTATUSNAME"].ToString() == "2nd Level Checked")
                    {
                        cmdReset.Visible = true;

                        string msg = "SOA Status will be updated to Pending.";
                        msg = msg + "Combined PDF and Additional Owner attachments, if already generated, will be deleted.";
                        msg = msg + "Reports that have been verified will be reset to Not Verified. 1st Level and 2nd Level Checker will have to re - confirm SOA";

                        cmdReset.Attributes.Add("onclick", "return fnConfirmDelete(event,'" + msg + "'); return false;");
                    }
                }
            }

        }
    }

    protected void gvOwnersAccount_ItemCreated(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem
            && !e.Item.Equals(DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && !e.Item.Equals(DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Item.TabIndex = -1;
            //e.Item.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvOwnersAccount, "Select$" + e.Item.RowIndex.ToString(), false);

            ImageButton cmdUntagVouchers = (ImageButton)e.Item.FindControl("cmdUntagVouchers");
            if (cmdUntagVouchers != null)
            {
                cmdUntagVouchers.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to untag all vouchers?'); return false;");
            }

            DataRowView drv = (DataRowView)e.Item.DataItem;

            ImageButton img1stCheck = (ImageButton)e.Item.FindControl("img1stCheck");
            if (img1stCheck != null)
            {
                if (General.GetNullableDateTime(drv["FLD1STCHECKDATE"].ToString()) == null)
                {
                    img1stCheck.Visible = true;
                }
                else
                    img1stCheck.Visible = false;
            }

            ImageButton img2ndCheck = (ImageButton)e.Item.FindControl("img2ndCheck");
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
            if (imgPublised != null)
            {
                if (General.GetNullableDateTime(drv["FLD1STCHECKDATE"].ToString()) != null
                     && General.GetNullableDateTime(drv["FLD2NDCHECKDATE"].ToString()) != null
                    && General.GetNullableDateTime(drv["FLDPUBLISHEDDATE"].ToString()) == null)
                {
                    imgPublised.Visible = true;
                }
                else
                    imgPublised.Visible = false;
            }

        }
    }
}
