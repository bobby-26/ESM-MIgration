using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Reports;
using SouthNests.Phoenix.VesselAccounts;
using System;
using System.Collections.Specialized;
using System.Data;
//using iTextSharp.text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsVesselVisitTravelClaimPosting : PhoenixBasePage
{
    public decimal totalAmount = 0;
    public string strAmountTotal = string.Empty;
    public string strBalanceInSGD = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("History", "HISTORY", ToolBarDirection.Right);
            toolbar.AddButton("View Debit Note", "DEBIT", ToolBarDirection.Right);
            toolbar.AddButton("Voucher Posting", "POSTING", ToolBarDirection.Right);
            toolbar.AddButton("Travel Claim", "CLAIM", ToolBarDirection.Right);
            MenuTravelClaimMain.AccessRights = this.ViewState;
            MenuTravelClaimMain.MenuList = toolbar.Show();
            MenuTravelClaimMain.SelectedMenuIndex = 2;

            PhoenixToolbar toolbarsub = new PhoenixToolbar();

            //toolbarsub.AddButton("Generate Payment Voucher", "GENERATEPV");



            toolbarsub.AddButton("Attachment", "ATTACHMENT", ToolBarDirection.Right);
            toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarsub.AddButton("Post", "POST", ToolBarDirection.Right);
            toolbarsub.AddButton("Repost", "REPOST", ToolBarDirection.Right);
            toolbarsub.AddButton("Revoke Approval", "REVOKE", ToolBarDirection.Right);
            MenuTravelClaimSub.AccessRights = this.ViewState;
            MenuTravelClaimSub.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                Session["New"] = "N";
                if (Request.QueryString["VisitId"] != "")
                    ViewState["VisitId"] = Request.QueryString["visitId"];
                else
                    ViewState["VisitId"] = null;
                if (Request.QueryString["TravelClaimId"] != "")
                    ViewState["TravelClaimId"] = Request.QueryString["TravelClaimId"];
                else
                    ViewState["TravelClaimId"] = null;

                ViewState["PAGENUMBERLINE"] = 1;
                ViewState["PAGENUMBERADV"] = 1;
                ViewState["PAGENUMBEROTHERADV"] = 1;

                BindpaymentMode();
                TravelClaimEdit();
                BindUsercontrol();
                if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) != null)
                {
                    DataSet dsaccount = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(General.GetNullableInteger(Convert.ToString(ViewState["VESSELID"])), 1);
                    if (dsaccount.Tables[0].Rows.Count > 0)
                    {
                        Getprincipal(Convert.ToInt32(dsaccount.Tables[0].Rows[0]["FLDACCOUNTID"]));
                    }
                }

               
            }

            PhoenixToolbar toolbarLine = new PhoenixToolbar();
            toolbarLine.AddImageButton("../Accounts/AccountsVesselVisitTravelClaimPosting.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarLine.AddImageLink("javascript:CallPrint('gvLineItem')", "Print Grid", "icon_print.png", "PRINT");
            toolbarLine.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsVesselVisitTravelClaimAddGST.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&ClaimId=" + ViewState["TravelClaimId"].ToString() + "')", "Add GST", "add.png", "ADD");
            MenuTravelClaim.AccessRights = this.ViewState;
            MenuTravelClaim.MenuList = toolbarLine.Show();

            PhoenixToolbar toolbarAdv = new PhoenixToolbar();
            toolbarAdv.AddImageButton("../Accounts/AccountsVesselVisitTravelClaimPosting.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarAdv.AddImageLink("javascript:CallPrint('gvTravelAdvance')", "Print Grid", "icon_print.png", "PRINT");
            toolbarAdv.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsVesselVisitMoneyChanger.aspx?VisitId=" + ViewState["VisitId"].ToString() + "')", "Add Money Changer", "add.png", "ADD");
            MenuTravelAdvance.AccessRights = this.ViewState;
            MenuTravelAdvance.MenuList = toolbarAdv.Show();

            PhoenixToolbar toolbarAdvOther = new PhoenixToolbar();
            toolbarAdvOther.AddImageButton("../Accounts/AccountsVesselVisitTravelClaimPosting.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarAdvOther.AddImageLink("javascript:CallPrint('gvTravelAdvanceOther')", "Print Grid", "icon_print.png", "PRINT");
            MenuTravelAdvanceOther.AccessRights = this.ViewState;
            MenuTravelAdvanceOther.MenuList = toolbarAdvOther.Show();

            BindLineItem();
            BindDataAdv();
            BindDataOtherAdv();
            txtBulkBudgetName.Attributes.Add("style", "visibility:hidden;");
            txtBulkBudgetId.Attributes.Add("style", "visibility:hidden;");
            txtBulkBudgetgroupId.Attributes.Add("style", "visibility:hidden;");
            txtBulkOwnerBudgetName.Attributes.Add("style", "visibility:hidden;");
            txtBulkOwnerBudgetId.Attributes.Add("style", "visibility:hidden;");
            txtBulkOwnerBudgetgroupId.Attributes.Add("style", "visibility:hidden;");

            if (btnShowBulkBudget != null && ViewState["VESSELID"] != null)
            {
                btnShowBulkBudget.Attributes.Add("onclick", "return showPickList('spnPickListBulkBudget', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=106&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
            }
            if (btnShowBulkOwnerBudget != null && ViewState["VESSELID"] != null)
            {
                btnShowBulkOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListBulkOwnerBudget', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=true&vesselid=" + null + "&Ownerid=" + ViewState["PRINCIPAL"] + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBulkBudgetId.Text + "', true); ");          //+ "&budgetid=" + lblbudgetid.Text       

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void BindUsercontrol()
    {
        int? accountid = General.GetNullableInteger(ddlAccountDetails.SelectedValue);
        int? budgetid = General.GetNullableInteger(txtBulkBudgetId.Text);
        ucProjectcode.bind(accountid, budgetid);
    }


    public void BindpaymentMode()
    {
        try
        {
            DataSet ds = PhoenixRegistersQuick.ListQuick(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 136);
            ddlPaymentMode.DataSource = ds;
            ddlPaymentMode.DataTextField = "FLDQUICKNAME";
            ddlPaymentMode.DataValueField = "FLDSHORTNAME";
            ddlPaymentMode.DataBind();
            ddlPaymentMode.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindBankDetails()
    {
        try
        {
            DataTable dt = PhoenixVesselAccountsEmployeeBankAccount.ListEmployeeBankAccount(int.Parse(ViewState["VESSELID"].ToString()), int.Parse(ViewState["EmployeeId"].ToString()));
            DataSet ds = new DataSet();
            ddlBankAccount.DataSource = dt;
            ddlBankAccount.DataTextField = "FLDACCOUNTNUMBER";
            ddlBankAccount.DataValueField = "FLDBANKACCOUNTID";
            ddlBankAccount.DataBind();
            ddlBankAccount.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTravelClaimMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("CLAIM"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitTravelClaimPostMaster.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&TravelClaimId=" + ViewState["TravelClaimId"].ToString());
            }
            else if (CommandName.ToUpper().Equals("DEBIT"))
            {
                Response.Redirect("../Reports/ReportsView.aspx?&applicationcode=5&reportcode=CLAIMDEBITNOTEVIEW&showmenu=0&VisitId=" + ViewState["VisitId"].ToString() + "&TravelClaimId=" + ViewState["TravelClaimId"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("HISTORY"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitTravelClaimHistory.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&TravelClaimId=" + ViewState["TravelClaimId"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTravelClaimSub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("REVOKE"))
            {
                if (ViewState["TravelClaimId"] == null || ViewState["TravelClaimId"].ToString() == string.Empty)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Travel claim not yet created";
                    ucError.Visible = true;
                    return;
                }
                String scriptpopup = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsVesselVisitTravelClaimRevokeRemarks.aspx?visitId=" + ViewState["VisitId"].ToString() + " &TravelClaimId=" + ViewState["TravelClaimId"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);



                //PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimRevokeApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //                                                               , new Guid(ViewState["VisitId"].ToString())
                //                                                               , new Guid(ViewState["TravelClaimId"].ToString())
                //                                                               , txtRevokeRemarks.Text);
                //ucStatus.Text = "Travel claim revoked.";
            }
            else if (CommandName.ToUpper().Equals("POST"))
            {

                GenerateAndSaveDebitnotePdf();
                Response.Redirect("../Reports/ReportsView.aspx?&applicationcode=5&reportcode=CLAIMDEBITNOTEVIEW&showmenu=0&VisitId=" + ViewState["VisitId"].ToString() + "&TravelClaimId=" + ViewState["TravelClaimId"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("REPOST"))
            {
                PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimRepost(new Guid(ViewState["VisitId"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ucStatus.Text = "Travel claim reposted.";
            }

            if (CommandName.ToUpper().Equals("GENERATEPV"))
            {
                PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimPVGenerate(new Guid(ViewState["VisitId"].ToString())
                                                                                , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ucStatus.Text = "Payment voucher generated.";
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData(ddlPaymentMode.SelectedValue, ddlLiabilitycompany.SelectedCompany))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimPostUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , new Guid(ViewState["VisitId"].ToString())
                                                                                , General.GetNullableGuid(ViewState["TravelClaimId"].ToString())
                                                                                , ddlPaymentMode.SelectedValue
                                                                                , General.GetNullableGuid(ddlBankAccount.SelectedValue)
                                                                                , int.Parse(ddlLiabilitycompany.SelectedCompany)
                                                                                , txtRevokeRemarks.Text
                                                                                , General.GetNullableDateTime(ucFromDate.Text)
                                                                                , General.GetNullableDateTime(ucToDate.Text)
                                                                                , General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency)
                                                                                , General.GetNullableInteger(txtBulkBudgetId.Text)
                                                                                , General.GetNullableGuid(txtBulkOwnerBudgetId.Text)
                                                                                , General.GetNullableDateTime(ucVisitStartDate.Text)
                                                                                , General.GetNullableDateTime(ucVisitEndDate.Text)
                                                                                , General.GetNullableInteger(ddlAccountDetails.SelectedValue));
                ucStatus.Text = "Travel claim updated";
                BindLineItem();
            }

            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {

                String scriptpopup = String.Format(
                   "javascript:openNewWindow('att', '', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["VISITDTKEY"].ToString() + "&mod=ACCOUNTS&type=&cmdname=&VESSELID=" + ViewState["VESSELID"].ToString() + "');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            TravelClaimEdit();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTravelClaim_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBERLINE"] = 1;
                BindLineItem();
                // SetPageNavigatorLine();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelLine();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTravelAdvance_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBERADV"] = 1;
                BindDataAdv();
                //SetPageNavigatorAdv();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelAdv();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTravelAdvanceOther_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBEROTHERADV"] = 1;
                BindDataOtherAdv();
                //SetPageNavigatorOtherAdv();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelOtherAdv();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelAdvance_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblManualYN = (RadLabel)e.Item.FindControl("lblManualYN");

            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null) cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
            if (lblManualYN != null && lblManualYN.Text == "0")
                if (cmdDelete != null) cmdDelete.Visible = false;
        }
    }

    protected void gvTravelAdvanceOther_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton cmdAdd = (ImageButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null) cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }
    }

    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadLabel lblRejected = (RadLabel)e.Item.FindControl("lblRejected");
                RadLabel lblMarkup = (RadLabel)e.Item.FindControl("lblMarkup");
                ImageButton cmdEditCancel = (ImageButton)e.Item.FindControl("cmdEditCancel");
                if (cmdEditCancel != null) cmdEditCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdEditCancel.CommandName);

                ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

                ImageButton cmdSave = (ImageButton)e.Item.FindControl("cmdSave");
                if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

                ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
                if (cmdDelete != null) cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);

                if (lblRejected != null && lblRejected.Text == "1")
                {
                    e.Item.Font.Strikeout = true;
                    if (cmdEdit != null) cmdEdit.Visible = false;
                }
                if (lblMarkup != null && lblMarkup.Text == "0")
                {
                    //if (cmdEdit != null) cmdEdit.Visible = false;
                    if (cmdDelete != null) cmdDelete.Visible = false;
                }
            }
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                RadLabel lblRejected = (RadLabel)e.Item.FindControl("lblRejected");
                RadLabel lblCurrencyId = (RadLabel)e.Item.FindControl("lblCurrencyId");
                if (lblCurrencyId != null && lblCurrencyId.Text != "")
                {
                    UserControlCurrency uc = ((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit"));
                    uc.SelectedCurrency = lblCurrencyId.Text.ToString();
                }

                RadLabel lblCountryCode = (RadLabel)e.Item.FindControl("lblCountryCode");
                if (lblCountryCode != null && lblCountryCode.Text != "")
                {
                    UserControlCountry ucCountryEdit = ((UserControlCountry)e.Item.FindControl("ucCountryEdit"));
                    ucCountryEdit.SelectedCountry = lblCountryCode.Text.ToString();
                }

                RadLabel lblSupAttachedEdit = (RadLabel)e.Item.FindControl("lblSupAttachedEdit");
                if (lblSupAttachedEdit != null && lblSupAttachedEdit.Text != "")
                {
                    DropDownList ddlSupAttachedEdit = ((DropDownList)e.Item.FindControl("ddlSupAttachedEdit"));
                    ddlSupAttachedEdit.SelectedValue = lblSupAttachedEdit.Text.ToString();
                }

                RadLabel lblTypeEdit = (RadLabel)e.Item.FindControl("lblTypeEdit");
                if (lblTypeEdit != null && lblTypeEdit.Text != "")
                {
                    RadComboBox ddlTypeEdit = ((RadComboBox)e.Item.FindControl("ddlTypeEdit"));
                    if (ddlTypeEdit != null)
                        ddlTypeEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlTypeEdit.SelectedValue = lblTypeEdit.Text.ToString();
                }

                ImageButton ib1 = (ImageButton)e.Item.FindControl("btnShowBudgetEdit");
                if (ib1 != null && ViewState["VESSELID"] != null)
                {
                    ib1.Attributes.Add("onclick", "return showPickList('spnPickListBudgetEdit', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=102&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                    ib1.Visible = SessionUtil.CanAccess(this.ViewState, ib1.CommandName);
                }

                RadTextBox tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetNameEdit");
                if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
                tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit");
                if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
                tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupIdEdit");
                if (tb != null) tb.Attributes.Add("style", "visibility:hidden");

                RadTextBox txtBudgetIdEdit = (RadTextBox)e.Item.FindControl("txtBudgetIdEdit");
                ImageButton imgShowOwnerBudgetEdit = (ImageButton)e.Item.FindControl("imgShowOwnerBudgetEdit");
                if (imgShowOwnerBudgetEdit != null && txtBudgetIdEdit != null)
                {
                    imgShowOwnerBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCodeEdit', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=&vesselid=" + null + "&Ownerid=" + Convert.ToString(ViewState["PRINCIPAL"]) + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdEdit.Text + "', true); ");
                    imgShowOwnerBudgetEdit.Visible = SessionUtil.CanAccess(this.ViewState, imgShowOwnerBudgetEdit.CommandName);
                }

                UserControls_UserControlProjectCode ProjectCode = (UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeEdit");
                if (ProjectCode != null)
                {
                    int? Account = General.GetNullableInteger(ddlAccountDetails.SelectedValue);
                    int? BudgetId = General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text);
                    ProjectCode.bind(Account, BudgetId);
                    ProjectCode.SelectedProjectCode = drv["FLDPROJECTID"].ToString();
                }

                RadLabel lblReimAmount = (RadLabel)e.Item.FindControl("lblReimAmount");
                if (lblRejected != null && lblRejected.Text != "1")
                {
                    if (lblReimAmount != null && lblReimAmount.Text != string.Empty)
                        totalAmount = totalAmount + Convert.ToDecimal(lblReimAmount.Text);
                }
            }
            if (e.Item is GridFooterItem)
            {
                UserControlCurrency uc = ((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd"));
                uc.ActiveCurrency = true;

                ImageButton cmdAdd = (ImageButton)e.Item.FindControl("cmdAdd");
                if (cmdAdd != null) cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
				
				UserControlDate ucDateAdd = (UserControlDate)e.Item.FindControl("ucDateAdd");
                if (ucDateAdd != null) ucDateAdd.Text = ViewState["VISITSTARTDATE"].ToString();
				
                RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtBudgetNameAdd");
                if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
                tb1 = (RadTextBox)e.Item.FindControl("txtBudgetIdAdd");
                if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
                tb1 = (RadTextBox)e.Item.FindControl("txtBudgetgroupIdAdd");
                if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

                ImageButton ib = (ImageButton)e.Item.FindControl("btnShowBudgetAdd");
                if (ib != null)
                {
                    ib.Attributes.Add("onclick", "return showPickList('spnPickListBudgetAdd', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=102&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
                    ib.Visible = SessionUtil.CanAccess(this.ViewState, ib.CommandName);
                }

                RadTextBox tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetName");
                if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
                tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetId");
                if (tb != null) tb.Attributes.Add("style", "visibility:hidden");
                tb = (RadTextBox)e.Item.FindControl("txtOwnerBudgetgroupId");
                if (tb != null) tb.Attributes.Add("style", "visibility:hidden");

                RadTextBox txtBudgetIdAdd = (RadTextBox)e.Item.FindControl("txtBudgetIdAdd");
                ImageButton imgShowOwnerBudget = (ImageButton)e.Item.FindControl("imgShowOwnerBudget");
                if (imgShowOwnerBudget != null && txtBudgetIdAdd.Text != null)
                {
                    imgShowOwnerBudget.Attributes.Add("onclick", "return showPickList('spnPickListOwnerBudgetCode', 'codehelp1', '', '../Common/CommonPickListOwnerBudget.aspx?iframignore=&vesselid=" + null + "&Ownerid=" + Convert.ToString(ViewState["PRINCIPAL"]) + "&budgetdate=" + DateTime.Now.ToString() + "&budgetid=" + txtBudgetIdAdd.Text + "', true); ");
                    imgShowOwnerBudget.Visible = SessionUtil.CanAccess(this.ViewState, imgShowOwnerBudget.CommandName);
                }
                RadComboBox ddlTypeAdd = ((RadComboBox)e.Item.FindControl("ddlTypeAdd"));
                if (ddlTypeAdd != null)
                    ddlTypeAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

                UserControls_UserControlProjectCode Projectcode = ((UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeAdd"));

                int? VesselAccount = General.GetNullableInteger(ddlAccountDetails.SelectedValue);
                int? Budget = General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetIdAdd")).Text);
                Projectcode.bind(VesselAccount, Budget);
            }
            strAmountTotal = totalAmount.ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLineItem_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        totalAmount = 0;
        BindLineItem();
        // SetPageNavigatorLine();
    }

    protected void LineItemRebind()
    {
        gvLineItem.SelectedIndexes.Clear();
        gvLineItem.EditIndexes.Clear();
        gvLineItem.DataSource = null;
        gvLineItem.Rebind();
    }
    protected void gvLineItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADDNEW"))
            {


                if (!IsValidLineItem(((UserControlDate)e.Item.FindControl("ucDateAdd")).Text
                                        , ((UserControlCountry)e.Item.FindControl("ucCountryAdd")).SelectedCountry
                                        , ((RadComboBox)e.Item.FindControl("ddlTypeAdd")).SelectedValue
                                        , ((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd")).SelectedCurrency
                                        , ((UserControlMaskNumber)e.Item.FindControl("txtAmountAdd")).Text

                                        ))
                {
                    ucError.Visible = true;
                    return;
                }
                
                PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimLineItemInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , new Guid(ViewState["VisitId"].ToString())
                     , new Guid(ViewState["TravelClaimId"].ToString())
                     , Convert.ToDateTime(((UserControlDate)e.Item.FindControl("ucDateAdd")).Text)
                     , int.Parse(((UserControlCountry)e.Item.FindControl("ucCountryAdd")).SelectedCountry)
                     , ((RadComboBox)e.Item.FindControl("ddlTypeAdd")).SelectedValue
                     , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetIdAdd")).Text)
                     , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtOwnerBudgetId")).Text)
                     , ((RadTextBox)e.Item.FindControl("txtExpenseDescAdd")).Text
                     , int.Parse(((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd")).SelectedCurrency)
                     , Convert.ToDecimal(((UserControlMaskNumber)e.Item.FindControl("txtAmountAdd")).Text)
                     , ""
                     , int.Parse("1")
                     , int.Parse(ViewState["VESSELID"].ToString())
                     , General.GetNullableInteger(((UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeAdd")).SelectedProjectCode)
                    

                     );
                ucStatus.Text = "Travel claim line item inserted";
                BindLineItem();

                LineItemRebind();

            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidLineItem(((UserControlDate)e.Item.FindControl("ucDateEdit")).Text
                                        , ((UserControlCountry)e.Item.FindControl("ucCountryEdit")).SelectedCountry
                                        , ((RadComboBox)e.Item.FindControl("ddlTypeEdit")).SelectedValue
                                        , ((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit")).SelectedCurrency
                                        , ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text

                                        ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimLineItemUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , new Guid(((RadLabel)e.Item.FindControl("lblClaimLineitemId")).Text)
                     , Convert.ToDateTime(((UserControlDate)e.Item.FindControl("ucDateEdit")).Text)
                     , int.Parse(((UserControlCountry)e.Item.FindControl("ucCountryEdit")).SelectedCountry)
                     , ((RadComboBox)e.Item.FindControl("ddlTypeEdit")).SelectedValue
                     , General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text)
                     , General.GetNullableGuid(((RadTextBox)e.Item.FindControl("txtOwnerBudgetIdEdit")).Text)
                     , ((RadTextBox)e.Item.FindControl("txtExpenseDescEdit")).Text
                     , int.Parse(((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit")).SelectedCurrency)
                     , Convert.ToDecimal(((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text)
                     , ""
                     , int.Parse("1")
                     , int.Parse(ViewState["VESSELID"].ToString())
                     , General.GetNullableInteger(((UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeEdit")).SelectedProjectCode)
                    

                     );
                ucStatus.Text = "Travel claim line item updated";
                BindLineItem();

                LineItemRebind();

            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(((RadLabel)e.Item.FindControl("lblClaimLineitemId")).Text));

                ucStatus.Text = "Travel claim line item deleted";
                LineItemRebind();
            }
            BindDataAdv();

            if (e.CommandName == "Page")
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

    public void TravelClaimEdit()
    {
        try
        {
            if (ViewState["VisitId"] != null)
            {
                DataSet ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimPostEdit(new Guid(ViewState["VisitId"].ToString()));
                DataRow dr = ds.Tables[0].Rows[0];

                txtPurpose.Text = dr["FLDPURPOSE"].ToString();
                ViewState["EmployeeId"] = dr["FLDEMPLOYEEID"].ToString();
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                ViewState["VISITDTKEY"] = dr["FLDVISITDTKEY"].ToString();

                BindBankDetails();

                ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(General.GetNullableInteger(dr["FLDVESSELID"].ToString()), 1);
                ddlAccountDetails.DataBind();


                txtEmployee.Text = dr["FLDEMPLOYEECODE"].ToString() + " / " + dr["FLDEMPLOYEENAME"].ToString();
                txtVesselVisitId.Text = dr["FLDFORMNUMBER"].ToString();
                //txtReimbursementCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
                ddlCurrencyCode.SelectedCurrency = dr["FLDREIMBURSEMENTCURRENCY"].ToString();
                txtRevokeRemarks.Text = dr["FLDREVOKEREMARKS"].ToString().Replace("^", "\n");

                ucPostingDate.Text = dr["FLDPOSTINGDATE"].ToString();
                ddlPaymentMode.SelectedValue = dr["FLDPAYMENTMODE"].ToString();
                if (ddlPaymentMode.SelectedValue == "REM")
                    ddlBankAccount.Enabled = true;
                else
                    ddlBankAccount.Enabled = false;
                ddlBankAccount.SelectedValue = dr["FLDBANKACCOUNTID"].ToString();
                txtPaymentVoucher.Text = dr["FLDPAYMENTVOUCHERNUMBER"].ToString();
                ddlAccountDetails.SelectedValue = dr["FLDVESSELACCOUNTID"].ToString();
                //txtVesselAccount.Text = dr["FLDDESCRIPTION"].ToString();
                txtVoucherNumber.Text = dr["FLDVOUCHERNUMBER"].ToString();
                if (dr["FLDLIABLITYCOMPANY"].ToString() == "")
                {
                    if (dr["FLDVISITTYPE"].ToString() == "3")
                        ddlLiabilitycompany.SelectedCompany = "12";
                    else
                        ddlLiabilitycompany.SelectedCompany = "16";
                }
                else
                    ddlLiabilitycompany.SelectedCompany = dr["FLDLIABLITYCOMPANY"].ToString();
                ViewState["LIABLITYCOMPANY"] = dr["FLDLIABLITYCOMPANY"].ToString();
                ucFromDate.Text = dr["FLDFROMDATE"].ToString();
				ViewState["VISITSTARTDATE"] = dr["FLDFROMDATE"].ToString();
                ucToDate.Text = dr["FLDTODATE"].ToString();
                ucVisitStartDate.Text = dr["FLDONBOARDDATE"].ToString();
                ucVisitEndDate.Text = dr["FLDDISEMBARKATIONDATE"].ToString();
                txtVoucher.Text = dr["FLDVOUCHERNUMBERS"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvLineItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindLineItem();
    }

    public void BindLineItem()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            totalAmount = 0;
            string[] alColumns = { "FLDROWNO", "FLDDATE", "FLDCOUNTRYNAME", "FLDCLAIMTYPENAME", "FLDSUBACCOUNT", "FLDOWNERBUDGETCODENAME", "FLDPROJECTCODE", "FLDEXPENSEDESCRIPTION", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDBASEEXCHANGERATE", "FLDREIMBURSEMENTAMOUNT" };
            string[] alCaptions = { "Item No.", "Date", "Country", "Type", "Budget Code", "Owner Budget Code","Project Code","Expense Description", "Currency", "Amount", "Exchange", "Reimbursement Amount" };

            DataSet ds = new DataSet();

            ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimLineItemListWithMarkup(new Guid(ViewState["VisitId"].ToString()), General.GetNullableGuid(ViewState["TravelClaimId"].ToString()),
                    (int)ViewState["PAGENUMBERLINE"],
                    gvLineItem.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

            General.SetPrintOptions("gvLineItem", "Travel Claim ", alCaptions, alColumns, ds);

            gvLineItem.DataSource = ds;
            gvLineItem.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNTLINE"] = iRowCount;
            ViewState["TOTALPAGECOUNTLINE"] = iTotalPageCount;
            //SetPageNavigatorLine();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelAdvance_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataAdv();
    }

    public void BindDataAdv()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            decimal iBalanceInSGD = 0;

            string[] alColumns = { "FLDTRAVELADVANCENUMBER", "FLDCURRENCYCODE", "FLDPAYMENTAMOUNT", "FLDRETURNAMOUNT", "FLDBALANCE", "FLDBALANCESGD", "FLDQUICKNAME", "FLDFORMNUMBER" };
            string[] alCaptions = { "Travel Advance Number", "Currency", "Payment Amount", "Return Amount", "Balance", "Balance in SGD", "Advance Status", "Visit Number" };

            DataSet ds = new DataSet();

            ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimAdvanceAccountList(new Guid(ViewState["VisitId"].ToString()),
                    (int)ViewState["PAGENUMBERADV"],
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount,
                    ref iBalanceInSGD);

            strBalanceInSGD = String.Format("{0:n}", iBalanceInSGD);
            General.SetPrintOptions("gvTravelAdvance", "Travel Advance", alCaptions, alColumns, ds);

            gvTravelAdvance.DataSource = ds;
            gvTravelAdvance.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNTADV"] = iRowCount;
            ViewState["TOTALPAGECOUNTADV"] = iTotalPageCount;
            // SetPageNavigatorAdv();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelAdvanceOther_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataOtherAdv();
    }
    public void BindDataOtherAdv()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDTRAVELADVANCENUMBER", "FLDCURRENCYCODE", "FLDPAYMENTAMOUNT", "FLDRETURNAMOUNT", "FLDBALANCE", "FLDBALANCESGD", "FLDQUICKNAME", "FLDFORMNUMBER" };
            string[] alCaptions = { "Travel Advance Number", "Currency", "Payment Amount", "Return Amount", "Balance", "Balance in SGD", "Advance Status", "Visit Number" };

            DataSet ds = new DataSet();

            ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimAdvanceOtherList(new Guid(ViewState["VisitId"].ToString()),
                    (int)ViewState["PAGENUMBEROTHERADV"],
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount);

            General.SetPrintOptions("gvTravelAdvanceOther", "Travel Advance", alCaptions, alColumns, ds);

            gvTravelAdvanceOther.DataSource = ds;
            gvTravelAdvance.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNTOTHERADV"] = iRowCount;
            ViewState["TOTALPAGECOUNTOTHERADV"] = iTotalPageCount;
            //  SetPageNavigatorOtherAdv();
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

    protected void ShowExcelLine()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDROWNO", "FLDDATE", "FLDCOUNTRYNAME", "FLDCLAIMTYPENAME", "FLDSUBACCOUNT", "FLDOWNERBUDGETCODENAME", "FLDPROJECTCODE", "FLDEXPENSEDESCRIPTION", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDREMARKS", "FLDSUPATTACHED" };
        string[] alCaptions = { "Item No", "Date", "Country", "Type", "Budget Code", "Owner Budget Code","Project Code","Expense Description", "Currency", "Amount", "Remarks", "Supporting Attached" };

        if (ViewState["ROWCOUNTLINE"] == null || Int32.Parse(ViewState["ROWCOUNTLINE"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTLINE"].ToString());

        ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimLineItemList(new Guid(ViewState["VisitId"].ToString()), General.GetNullableGuid(ViewState["TravelClaimId"].ToString()),
                    (int)ViewState["PAGENUMBERLINE"],
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename= TravelClaim.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Claim</h3></td>");
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
    protected void ShowExcelAdv()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        decimal iBalanceInSGD = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDTRAVELADVANCENUMBER", "FLDPAIDDATE", "FLDCURRENCYCODE", "FLDPAYMENTAMOUNT", "FLDRETURNAMOUNT", "FLDMONEYCHANGER", "FLDCLAIMAMOUNT", "FLDBALANCE", "FLDBALANCESGD", "FLDQUICKNAME" };
        string[] alCaptions = { "Travel Advance Number", "Paid Date", "Currency", "Payment Amount", "Return Amount", "Money Changer", "Claim Amount", "Balance", "Balance in SGD", "Advance Status" };

        if (ViewState["ROWCOUNTADV"] == null || Int32.Parse(ViewState["ROWCOUNTADV"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTADV"].ToString());

        ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimAdvanceList(new Guid(ViewState["VisitId"].ToString()),
                    (int)ViewState["PAGENUMBERADV"],
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount,
                    ref iBalanceInSGD);

        Response.AddHeader("Content-Disposition", "attachment; filename= TravelAdvance.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Advance</h3></td>");
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

    protected void ShowExcelOtherAdv()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDTRAVELADVANCENUMBER", "FLDCURRENCYCODE", "FLDPAYMENTAMOUNT", "FLDRETURNAMOUNT", "FLDBALANCE", "FLDBALANCESGD", "FLDQUICKNAME", "FLDFORMNUMBER" };
        string[] alCaptions = { "Travel Advance Number", "Currency", "Payment Amount", "Return Amount", "Balance", "Balance in SGD", "Advance Status", "Visit Number" };

        if (ViewState["ROWCOUNTOTHERADV"] == null || Int32.Parse(ViewState["ROWCOUNTOTHERADV"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNTOTHERADV"].ToString());

        ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimAdvanceOtherList(new Guid(ViewState["VisitId"].ToString()),
                    (int)ViewState["PAGENUMBEROTHERADV"],
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename= TravelAdvance.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Advance</h3></td>");
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

    //private void SetPageNavigatorAdv()
    //{
    //    cmdPreviousadv.Enabled = IsPreviousEnabledAdv();
    //    cmdNextadv.Enabled = IsNextEnabledAdv();
    //    lblPagenumberadv.Text = "Page " + ViewState["PAGENUMBERADV"].ToString();
    //    lblPagesadv.Text = " of " + ViewState["TOTALPAGECOUNTADV"].ToString() + " Pages. ";
    //    lblRecordsadv.Text = "(" + ViewState["ROWCOUNTADV"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabledAdv()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBERADV"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNTADV"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabledAdv()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBERADV"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNTADV"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //protected void cmdGoAdv_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    if (Int32.TryParse(txtnopageadv.Text, out result))
    //    {
    //        ViewState["PAGENUMBERADV"] = Int32.Parse(txtnopageadv.Text);

    //        if ((int)ViewState["TOTALPAGECOUNTADV"] < Int32.Parse(txtnopageadv.Text))
    //            ViewState["PAGENUMBERADV"] = ViewState["TOTALPAGECOUNTADV"];


    //        if (0 >= Int32.Parse(txtnopageadv.Text))
    //            ViewState["PAGENUMBERADV"] = 1;

    //        if ((int)ViewState["PAGENUMBERADV"] == 0)
    //            ViewState["PAGENUMBERADV"] = 1;

    //        txtnopageadv.Text = ViewState["PAGENUMBERADV"].ToString();
    //    }
    //    BindDataAdv();
    //}

    //protected void PagerButtonClickadv(object sender, CommandEventArgs ce)
    //{
    //    gvTravelAdvance.SelectedIndex = -1;
    //    gvTravelAdvance.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBERADV"] = (int)ViewState["PAGENUMBERADV"] - 1;
    //    else
    //        ViewState["PAGENUMBERADV"] = (int)ViewState["PAGENUMBERADV"] + 1;
    //    BindDataAdv();
    //}

    //private void SetPageNavigatorOtherAdv()
    //{
    //    cmdPreviousOtheradv.Enabled = IsPreviousEnableOtherdAdv();
    //    cmdNextOtheradv.Enabled = IsNextEnabledOtherAdv();
    //    lblPagenumberOtheradv.Text = "Page " + ViewState["PAGENUMBEROTHERADV"].ToString();
    //    lblPagesOtheradv.Text = " of " + ViewState["TOTALPAGECOUNTOTHERADV"].ToString() + " Pages. ";
    //    lblRecordsOtheradv.Text = "(" + ViewState["ROWCOUNTOTHERADV"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnableOtherdAdv()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBEROTHERADV"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNTOTHERADV"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabledOtherAdv()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBEROTHERADV"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNTOTHERADV"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //protected void cmdGoOtherAdv_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    if (Int32.TryParse(txtnopageOtheradv.Text, out result))
    //    {
    //        ViewState["PAGENUMBEROTHERADV"] = Int32.Parse(txtnopageOtheradv.Text);

    //        if ((int)ViewState["TOTALPAGECOUNTOTHERADV"] < Int32.Parse(txtnopageOtheradv.Text))
    //            ViewState["PAGENUMBEROTHERADV"] = ViewState["TOTALPAGECOUNTOTHERADV"];


    //        if (0 >= Int32.Parse(txtnopageOtheradv.Text))
    //            ViewState["PAGENUMBEROTHERADV"] = 1;

    //        if ((int)ViewState["PAGENUMBEROTHERADV"] == 0)
    //            ViewState["PAGENUMBEROTHERADV"] = 1;

    //        txtnopageOtheradv.Text = ViewState["PAGENUMBEROTHERADV"].ToString();
    //    }
    //    BindDataOtherAdv();
    //}

    //protected void PagerButtonClickOtheradv(object sender, CommandEventArgs ce)
    //{
    //    gvTravelAdvanceOther.SelectedIndex = -1;
    //    gvTravelAdvanceOther.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBEROTHERADV"] = (int)ViewState["PAGENUMBEROTHERADV"] - 1;
    //    else
    //        ViewState["PAGENUMBEROTHERADV"] = (int)ViewState["PAGENUMBEROTHERADV"] + 1;
    //    BindDataOtherAdv();
    //}

    //private void SetPageNavigatorLine()
    //{
    //    cmdPreviousLine.Enabled = IsPreviousEnabledLine();
    //    cmdNextLine.Enabled = IsNextEnabledLine();
    //    lblPagenumberLine.Text = "Page " + ViewState["PAGENUMBERLINE"].ToString();
    //    lblPagesLine.Text = " of " + ViewState["TOTALPAGECOUNTLINE"].ToString() + " Pages. ";
    //    lblRecordsLine.Text = "(" + ViewState["ROWCOUNTLINE"].ToString() + " records found)";
    //}

    //private Boolean IsPreviousEnabledLine()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBERLINE"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNTLINE"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    //private Boolean IsNextEnabledLine()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBERLINE"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNTLINE"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //protected void cmdGoLine_Click(object sender, EventArgs e)
    //{
    //    int result;
    //    if (Int32.TryParse(txtnopageLine.Text, out result))
    //    {
    //        ViewState["PAGENUMBERLINE"] = Int32.Parse(txtnopageLine.Text);

    //        if ((int)ViewState["TOTALPAGECOUNTLINE"] < Int32.Parse(txtnopageLine.Text))
    //            ViewState["PAGENUMBERLINE"] = ViewState["TOTALPAGECOUNTLINE"];


    //        if (0 >= Int32.Parse(txtnopageLine.Text))
    //            ViewState["PAGENUMBERLINE"] = 1;

    //        if ((int)ViewState["PAGENUMBERLINE"] == 0)
    //            ViewState["PAGENUMBERLINE"] = 1;

    //        txtnopageLine.Text = ViewState["PAGENUMBERLINE"].ToString();
    //    }
    //    BindLineItem();
    //}

    //protected void PagerButtonClickLine(object sender, CommandEventArgs ce)
    //{
    //    gvLineItem.SelectedIndex = -1;
    //    gvLineItem.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBERLINE"] = (int)ViewState["PAGENUMBERLINE"] - 1;
    //    else
    //        ViewState["PAGENUMBERLINE"] = (int)ViewState["PAGENUMBERLINE"] + 1;
    //    BindDataAdv();
    //}

    private bool IsValidData(string paymentMode, string companyId)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (paymentMode.Trim().Equals(""))
            ucError.ErrorMessage = "Payment Mode is required.";

        if (companyId.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Liability company is required.";

        return (!ucError.IsError);
    }

    private bool IsValidLineItem(string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Reimbursement amount is required.";

        return (!ucError.IsError);
    }


    private bool IsValidLineItem(string date, string country, string type, string currency, string amount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "Date is required.";
        if (General.GetNullableInteger(country) == null)
            ucError.ErrorMessage = "Country is required.";
        if (type.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Type is required.";
        if (currency.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Currency is required.";
        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";
        


        return (!ucError.IsError);

    }

    protected void ddlPaymentMode_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPaymentMode.SelectedValue == "REM")
            ddlBankAccount.Enabled = true;
        else
            ddlBankAccount.Enabled = false;
    }
    protected void gvTravelAdvanceOther_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                PhoenixAccountsVesselVIistTravelClaimRegister.TravelAdvanceManualInsert(new Guid(ViewState["VisitId"].ToString())
                     , new Guid(((RadLabel)e.Item.FindControl("lblTravelAdvanceId")).Text)
                     , new Guid(ViewState["TravelClaimId"].ToString())
                     , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                ucStatus.Text = "Travel claim line item inserted";
                BindLineItem();
				BindDataOtherAdv();
            }

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            BindDataAdv();
            BindDataOtherAdv();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelAdvance_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindDataAdv();
        // SetPageNavigatorAdv();
    }
    protected void gvTravelAdvance_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //GridView _gridView = (GridView)sender;
            //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsVesselVIistTravelClaimRegister.TravelAdvanceManualDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((RadLabel)e.Item.FindControl("lblTravelAdvanceId")).Text));

                ucStatus.Text = "Travel advance deleted.";
                BindDataAdv();
                BindDataOtherAdv();
            }

            if (e.CommandName == "Page")
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        TravelClaimEdit();
        BindLineItem();
        BindDataAdv();
        BindDataOtherAdv();
    }
    public void Getprincipal(int accountid)
    {
        try
        {
            DataSet ds = null;
            ds = PhoenixRegistersAccount.EditAccount(accountid);
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["PRINCIPAL"] = Convert.ToString(dr["FLDPRINCIPALID"]);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RefreshBudgetCode(object sender, ImageClickEventArgs arg)
    {

        try
        {
            PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimPostUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , new Guid(ViewState["VisitId"].ToString())
                                                                                , General.GetNullableGuid(ViewState["TravelClaimId"].ToString())
                                                                                , ddlPaymentMode.SelectedValue
                                                                                , General.GetNullableGuid(ddlBankAccount.SelectedValue)
                                                                                , int.Parse(ddlLiabilitycompany.SelectedCompany)
                                                                                , txtRevokeRemarks.Text
                                                                                , General.GetNullableDateTime(ucFromDate.Text)
                                                                                , General.GetNullableDateTime(ucToDate.Text)
                                                                                , General.GetNullableInteger(ddlCurrencyCode.SelectedCurrency)
                                                                                , General.GetNullableInteger(txtBulkBudgetId.Text)
                                                                                , General.GetNullableGuid(txtBulkOwnerBudgetId.Text)
                                                                                , General.GetNullableDateTime(ucVisitStartDate.Text)
                                                                                , General.GetNullableDateTime(ucVisitEndDate.Text)
                                                                                , General.GetNullableInteger(ddlAccountDetails.SelectedValue)
                                                                                , General.GetNullableInteger(ucProjectcode.SelectedProjectCode)
                                                                                );


            ucStatus.Text = "Budget Codes updated Successfully.";

            String script = String.Format("javascript:fnReloadList('code1','ifMoreInfo','','');");
            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void RefreshRemarks(object sender, ImageClickEventArgs arg)
    {

        try
        {
            PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimRevokeRemarksUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , new Guid(ViewState["TravelClaimId"].ToString())
                     , txtremarks2.Text);

            DataSet ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimRevokeApprovalMaillist(new Guid(ViewState["VisitId"].ToString()));
            DataRow dr = ds.Tables[0].Rows[0];

            string emailbodytext = "Dear Sir/Madam, </br></br> Following Vessel/Business/Office claims are revoked for your kind review. </br></br>" + dr["FLDFORMNUMBER"].ToString() + "</br> </br> Thanks & Best Regards </br> </br> System Administrative team </br>(Auto email)<br> </br> Please click the below link to view the Revoke Remarks.</br></br>" + dr["FLDURL"].ToString();

            PhoenixMail.SendMail(dr["FLDTOMAIL"].ToString(), null, null, "Vessel/Business/Office claims – Revoke Approval.", emailbodytext, true, System.Net.Mail.MailPriority.Normal, "");

            ucStatus.Text = "Remark Updated.";
            BindLineItem();
            TravelClaimEdit();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void txtBulkBudgetIdClick(object sender, EventArgs e)
    {
        txtBulkOwnerBudgetgroupId.Text = "";
        txtBulkOwnerBudgetId.Text = "";
        txtBulkOwnerBudgetName.Text = "";
        txtBulkOwnerBudgetCode.Text = "";
    }

    protected void GenerateAndSaveDebitnotePdf()
    {
        try
        {

            string[] _reportfile = new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
            string _filename = "";
            DataSet ds = new DataSet();

            NameValueCollection criteria = new NameValueCollection();
            criteria.Add("applicationcode", "5");
            _reportfile = new string[6];

            _filename = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts" + ViewState["VISITDTKEY"].ToString() + ".pdf";
            //_filename = HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts\\") + ViewState["VISITDTKEY"].ToString() + ".pdf";
            string filepath = "ACCOUNTS" + "/" + ViewState["VISITDTKEY"] + _filename.Substring(_filename.LastIndexOf('.'));
            string _filepath = HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts\\");

            criteria.Add("reportcode", "CLAIMDEBITNOTEVIEW");
            _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsTravelClaimDebitNoteView.rpt");
            criteria.Add("VisitId", General.GetNullableGuid(ViewState["VisitId"].ToString()).ToString());
            criteria.Add("TravelClaimId", General.GetNullableGuid(ViewState["TravelClaimId"].ToString()).ToString());
            Session["PHOENIXREPORTPARAMETERS"] = criteria;

            NameValueCollection nvc = (NameValueCollection)Session["PHOENIXREPORTPARAMETERS"];
            ds = PhoenixReportsCommon.GetReportAndSubReportData(nvc, ref _reportfile, ref _filename);
            if (ds.Tables.Count > 0)
            {
                _filename = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts" + ViewState["VISITDTKEY"].ToString() + ".pdf";
                // _filename = HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts\\") + ViewState["VISITDTKEY"].ToString() + ".pdf";
                if (PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
                {

                    PhoenixSsrsReportsCommon.GetInterface(_reportfile, ds, ExportFileFormat.PDF, "GenerateSSRSPDF", ref _filename);
                }
                else
                {
                    _reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsAccountsTravelClaimDebitNoteView.rpt");
                    PhoenixReportClass.ExportReportPDF(_reportfile, ref _filename, ds);
                }
                long length = new System.IO.FileInfo(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts" + ViewState["VISITDTKEY"].ToString() + ".pdf").Length;
                //long length = new System.IO.FileInfo(HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts\\") + ViewState["VISITDTKEY"].ToString() + ".pdf").Length;
                PhoenixCommonFileAttachment.AttachmentDelete(new Guid(ViewState["VISITDTKEY"].ToString()));
                PhoenixCommonFileAttachment.InsertAttachment(new Guid(ViewState["VISITDTKEY"].ToString()), "AccountsTravelClaimDebitNoteView.pdf", filepath, length, null, null, "TravelClaimDebitNote", new Guid(ViewState["VISITDTKEY"].ToString()));
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlAccountDetails_TextChanged(object sender, EventArgs e)
    {
        int? accountid = General.GetNullableInteger(ddlAccountDetails.SelectedValue);
        int? budgetid = General.GetNullableInteger(txtBulkBudgetId.Text);
        ucProjectcode.bind(accountid, budgetid);
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        int? VesselAccountId = General.GetNullableInteger(ddlAccountDetails.SelectedValue);

        GridFooterItem footerItem = (GridFooterItem)gvLineItem.MasterTableView.GetItems(GridItemType.Footer)[0];
        ImageButton btbudget = (ImageButton)footerItem.FindControl("btnShowBudgetAdd");
        UserControls_UserControlProjectCode ProjectCodeAdd = (UserControls_UserControlProjectCode)footerItem.FindControl("ucProjectcodeAdd");
        int? Budget = General.GetNullableInteger(((RadTextBox)footerItem.FindControl("txtBudgetIdAdd")).Text);

        if (btbudget != null)
            ProjectCodeAdd.bind(VesselAccountId, Budget);

        if (btnShowBulkBudget != null)
        {
            int? bulkBudget = General.GetNullableInteger(txtBulkBudgetId.Text);
            ucProjectcode.bind(VesselAccountId, bulkBudget);
        }
    }

    protected void txtBudgetIdEdit_TextChanged(object sender, EventArgs e)
    {
        int? VesselAccountId = General.GetNullableInteger(ddlAccountDetails.SelectedValue);

        GridDataItem Item = (GridDataItem)gvLineItem.MasterTableView.GetItems(GridItemType.EditItem)[0];

        ImageButton ib1 = (ImageButton)Item.FindControl("btnShowBudgetEdit");
        UserControls_UserControlProjectCode ProjectCodeEdit = (UserControls_UserControlProjectCode)Item.FindControl("ucProjectcodeEdit");
        int? BudgetEditid = General.GetNullableInteger(((RadTextBox)Item.FindControl("txtBudgetIdEdit")).Text);

        if (ib1 != null)
        {
            ProjectCodeEdit.bind(VesselAccountId, BudgetEditid);
        }

    }


}
