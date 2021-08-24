using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;


public partial class AccountsVesselVIistTravelClaimRegister : PhoenixBasePage
{
    public string strBalanceInSGD = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddButton("Report", "REPORT", ToolBarDirection.Right);
            toolbar.AddButton("Travel Claim", "CLAIM", ToolBarDirection.Right);
            toolbar.AddButton("Office/Travel Advance", "ADVANCE", ToolBarDirection.Right);
            toolbar.AddButton("Vessel Visit", "VISIT", ToolBarDirection.Right);
            MenuTravelClaimMain.AccessRights = this.ViewState;
            MenuTravelClaimMain.MenuList = toolbar.Show();
            MenuTravelClaimMain.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                Session["New"] = "N";
                if (Request.QueryString["VisitStatus"] != "")
                    ViewState["VisitStatus"] = Request.QueryString["VisitStatus"];

                if (Request.QueryString["VisitId"] != "")
                    ViewState["VisitId"] = Request.QueryString["visitId"];
                else
                    ViewState["VisitId"] = null;
                if (Request.QueryString["VisitType"] != "")
                    ViewState["VisitType"] = Request.QueryString["VisitType"];
                else
                    ViewState["VisitType"] = null;
                ViewState["PAGENUMBERLINE"] = 1;
                ViewState["PAGENUMBERADV"] = 1;
                ViewState["PAGENUMBEROTHERADV"] = 1;
                ViewState["VESSELID"] = null;
                ViewState["TravelClaimId"] = null;
                ViewState["ClaimStatus"] = null;
                ViewState["PRINCIPAL"] = null;
                ViewState["SupAttachChange"] = null;
                ViewState["VesselAccountId"] = null;


                //BindCurrency();
                TravelClaimEdit();
                ViewState["Indexno"] = Request.QueryString["gvindex"];
                if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) != null)
                {
                    DataSet dsaccount = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(General.GetNullableInteger(Convert.ToString(ViewState["VESSELID"])), 1);
                    if (dsaccount.Tables[0].Rows.Count > 0)
                    {
                        Getprincipal(Convert.ToInt32(dsaccount.Tables[0].Rows[0]["FLDACCOUNTID"]));
                    }
                }

                BindUsercontrol();
            }


            PhoenixToolbar toolbarLine = new PhoenixToolbar();
            toolbarLine.AddImageButton("../Accounts/AccountsVesselVIistTravelClaimRegister.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarLine.AddImageLink("javascript:CallPrint('gvLineItem')", "Print Grid", "icon_print.png", "PRINT");
            MenuTravelClaim.AccessRights = this.ViewState;
            MenuTravelClaim.MenuList = toolbarLine.Show();

            PhoenixToolbar toolbarAdv = new PhoenixToolbar();
            toolbarAdv.AddImageButton("../Accounts/AccountsVesselVIistTravelClaimRegister.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarAdv.AddImageLink("javascript:CallPrint('gvTravelAdvance')", "Print Grid", "icon_print.png", "PRINT");
            toolbarAdv.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsVesselVisitMoneyChanger.aspx?VisitId=" + ViewState["VisitId"].ToString() + "')", "Add Money Changer", "add.png", "ADD");
            MenuTravelAdvance.AccessRights = this.ViewState;
            MenuTravelAdvance.MenuList = toolbarAdv.Show();

            PhoenixToolbar toolbarAdvOther = new PhoenixToolbar();
            toolbarAdvOther.AddImageButton("../Accounts/AccountsVesselVIistTravelClaimRegister.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbarAdvOther.AddImageLink("javascript:CallPrint('gvTravelAdvanceOther')", "Print Grid", "icon_print.png", "PRINT");
            MenuTravelAdvanceOther.AccessRights = this.ViewState;
            MenuTravelAdvanceOther.MenuList = toolbarAdvOther.Show();


            // TravelClaimEdit();
            SetStatus();
            PhoenixToolbar toolbarsub = new PhoenixToolbar();

            if (ViewState["ClaimStatus"].ToString() == "ACC")
            {
                toolbarsub.AddButton("Revoke Approval", "REVOKE", ToolBarDirection.Right);
                toolbarsub.AddButton("Attachment", "ATTACHMENT", ToolBarDirection.Right);
            }

            else if (ViewState["ClaimStatus"].ToString() == "PA")
            {
                //toolbarsub.AddButton("Rework", "REWORK");
                toolbarsub.AddButton("Approve", "APPROVE", ToolBarDirection.Right);
                toolbarsub.AddButton("Next", "NEXT", ToolBarDirection.Right);
                toolbarsub.AddButton("Revoke Approval", "REVOKE", ToolBarDirection.Right);
                toolbarsub.AddButton("Attachment", "ATTACHMENT", ToolBarDirection.Right);
            }

            else if (ViewState["ClaimStatus"].ToString() == "NSNA")
            {
                toolbarsub.AddButton("Submit for Approval", "APPROVAL", ToolBarDirection.Right);
                toolbarsub.AddButton("Attachment", "ATTACHMENT", ToolBarDirection.Right);
            }

            toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuTravelClaimSub.AccessRights = this.ViewState;
            if (ViewState["ClaimStatus"].ToString() == "NSNA" || ViewState["ClaimStatus"].ToString() == "PA" || ViewState["ClaimStatus"].ToString() == "ACC")
                MenuTravelClaimSub.MenuList = toolbarsub.Show();
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
            BindLineItem();
            BindDataAdv();
            BindDataOtherAdv();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void BindUsercontrol()
    {
        int? accountid = General.GetNullableInteger(ViewState["VesselAccountId"].ToString());
        int? budgetid = General.GetNullableInteger(txtBulkBudgetId.Text);
        ucProjectcode.bind(accountid, budgetid);
    }


    protected void MenuTravelClaimMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("VISIT"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitITSuperintendentVisitRegister.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&VisitType=" + ViewState["VisitType"] + "&VisitStatus=" + ViewState["VisitStatus"].ToString());
            }

            else if (CommandName.ToUpper().Equals("ADVANCE"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitTravelAdvanceRequest.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&VisitType=" + ViewState["VisitType"].ToString() + "&VisitStatus=" + ViewState["VisitStatus"].ToString(), false);
            }
            else if (CommandName.ToUpper().Equals("REPORT"))
            {
                Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&applicationcode=5&reportcode=TRAVELCLIME" + "&VisitStatus=" + ViewState["VisitStatus"].ToString(), false);
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
            if (CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                String scriptpopup = String.Format(
                   "javascript:parent.openNewWindow('att', '', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["VISITDTKEY"].ToString() + "&mod=ACCOUNTS&mimetype=.pdf& cmdname=&VESSELID=" + ViewState["VESSELID"].ToString() + "');");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
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
                   "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsVesselVisitTravelClaimRevokeRemarks.aspx?visitId=" + ViewState["VisitId"].ToString() + " &TravelClaimId=" + ViewState["TravelClaimId"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                TravelClaimEdit();
                BindLineItem();
            }
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                if (ViewState["TravelClaimId"] == null || ViewState["TravelClaimId"].ToString() == string.Empty)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Travel claim not yet created";
                    ucError.Visible = true;
                    return;
                }


                int iApprovalStatusAccounts;
                int? onbehaalf = null;
                DataTable dt = PhoenixCommonApproval.ListApprovalOnbehalf(1585, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);

                if (dt.Rows.Count > 0)
                {
                    onbehaalf = General.GetNullableInteger(dt.Rows[0]["FLDUSERCODE"].ToString());
                }
                string Status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 49, "APP");
                DataTable dt1 = PhoenixCommonApproval.InsertApprovalRecord(ViewState["VisitId"].ToString(), 1585, onbehaalf, int.Parse(Status), ".", PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);
                iApprovalStatusAccounts = int.Parse(dt1.Rows[0][0].ToString());

                byte bAllApproved = 0;
                DataTable dts = PhoenixCommonApproval.ListApprovalRecord(ViewState["VisitId"].ToString(), 1585, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null, 1, ref bAllApproved);

                PhoenixCommonApproval.Approve((PhoenixModule)Enum.Parse(typeof(PhoenixModule), PhoenixModule.ACCOUNTS.ToString()), 1585, ViewState["VisitId"].ToString(), iApprovalStatusAccounts, bAllApproved == 1 ? true : false, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString());


                if (iApprovalStatusAccounts.ToString() == "420")
                {
                    PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                                                , new Guid(ViewState["VisitId"].ToString())
                                                                                                                , General.GetNullableGuid(ViewState["TravelClaimId"].ToString()));
                }
                ucStatus.Text = "Travel claim approved.";
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData(ucDate.Text, ddlCurrency.SelectedCurrency))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , new Guid(ViewState["VisitId"].ToString())
                                                                                , General.GetNullableGuid(ViewState["TravelClaimId"].ToString())
                                                                                , General.GetNullableDateTime(ucDate.Text)
                                                                                , General.GetNullableInteger(ddlCurrency.SelectedCurrency)
                                                                                , txtRemarks.Text,
                                                                                General.GetNullableInteger(txtBulkBudgetId.Text),
                                                                                General.GetNullableGuid(txtBulkOwnerBudgetId.Text));
                ucStatus.Text = "Travel claim updated";
            }
            if (CommandName.ToUpper().Equals("APPROVAL"))
            {
                if (ViewState["TravelClaimId"] == null || ViewState["TravelClaimId"].ToString() == string.Empty)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Travel claim not yet created";
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["VisitType"].ToString() != "4")
                {
                    String scriptpopup = String.Format(
                       "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsVesselVisitTravelClaimSubmitForApprovalConfirmation.aspx?visitId=" + ViewState["VisitId"].ToString() + " &TravelClaimId=" + ViewState["TravelClaimId"].ToString() + "');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                    TravelClaimEdit();
                    BindLineItem();
                }
                else
                {
					DataSet ds = new DataSet();
                    ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimSubmitForApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , new Guid(ViewState["VisitId"].ToString())
                                                                                , new Guid(ViewState["TravelClaimId"].ToString()));

							 DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["ISAPPROVER"] = Convert.ToString(dr["FLDISAPPROVER"]);

                    if (ViewState["ISAPPROVER"] != null && ViewState["ISAPPROVER"].ToString() == "1")
                    {
                        if (ViewState["TravelClaimId"] == null || ViewState["TravelClaimId"].ToString() == string.Empty)
                        {
                            ucError.HeaderMessage = "Please provide the following required information";
                            ucError.ErrorMessage = "Travel claim not yet created";
                            ucError.Visible = true;
                            return;
                        }


                        int iApprovalStatusAccounts;
                        int? onbehaalf = null;
                        DataTable dt = PhoenixCommonApproval.ListApprovalOnbehalf(1585, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);

                        if (dt.Rows.Count > 0)
                        {
                            onbehaalf = General.GetNullableInteger(dt.Rows[0]["FLDUSERCODE"].ToString());
                        }
                        string Status = PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 49, "APP");
                        DataTable dt1 = PhoenixCommonApproval.InsertApprovalRecord(ViewState["VisitId"].ToString(), 1585, onbehaalf, int.Parse(Status), ".", PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null);
                        iApprovalStatusAccounts = int.Parse(dt1.Rows[0][0].ToString());

                        byte bAllApproved = 0;
                        DataTable dts = PhoenixCommonApproval.ListApprovalRecord(ViewState["VisitId"].ToString(), 1585, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString(), null, 1, ref bAllApproved);

                        PhoenixCommonApproval.Approve((PhoenixModule)Enum.Parse(typeof(PhoenixModule), PhoenixModule.ACCOUNTS.ToString()), 1585, ViewState["VisitId"].ToString(), iApprovalStatusAccounts, bAllApproved == 1 ? true : false, PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString());


                        if (iApprovalStatusAccounts.ToString() == "420")
                        {
                            PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                                                        , new Guid(ViewState["VisitId"].ToString())
                                                                                                                        , General.GetNullableGuid(ViewState["TravelClaimId"].ToString()));
                        }
                        ucStatus.Text = "Travel claim approved.";
                    }
                    else
                    {
                    ucStatus.Text = "Travel claim submitted for approval";
                }
            }
			}
            if (CommandName.ToUpper().Equals("REWORK"))
            {
                if (ViewState["TravelClaimId"] == null || ViewState["TravelClaimId"].ToString() == string.Empty)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Travel claim not yet created";
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimRework(new Guid(ViewState["VisitId"].ToString()), new Guid(ViewState["TravelClaimId"].ToString()));
            }

            if (CommandName.ToUpper().Equals("NEXT"))
            {
                int index;

                if (ViewState["Indexno"].ToString() != null)
                    index = Convert.ToInt32(ViewState["Indexno"].ToString());
                else
                    index = 0;

                index += 1;

                int iRowCount = 0;
                int iTotalPageCount = 0;
                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = null;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                DataSet ds = new DataSet();

                NameValueCollection nvc = Filter.CurrentVesselvisitSelection;

                if (nvc != null && nvc.Get("FIND") != null && nvc.Get("FIND").ToString().ToUpper().Equals("ADVANCE"))
                {

                    ds = PhoenixAccountsVesselVisitITSuperintendentRegister.VisitAdvanceList(
                    nvc != null ? General.GetNullableString(nvc.Get("txtEmpId")) : null,
                    nvc != null ? General.GetNullableString(nvc.Get("txtEmployeeNameSearch")) : null,
                    nvc != null ? General.GetNullableString(nvc.Get("txtFormNo")) : null,
                    nvc != null ? General.GetNullableString(nvc.Get("chkVesselList")) : null,
                    nvc != null ? General.GetNullableDateTime(nvc.Get("txtstartFromDate")) : null,
                    nvc != null ? General.GetNullableDateTime(nvc.Get("txtstartToDate")) : null,
                    nvc != null ? General.GetNullableDateTime(nvc.Get("txtendFromDate")) : null,
                    nvc != null ? General.GetNullableDateTime(nvc.Get("txtendToDate")) : null,
                    nvc != null ? General.GetNullableString(nvc.Get("txtVisitStatus")) : null,
                    nvc != null ? General.GetNullableString("PA") : null,
                    nvc != null ? General.GetNullableString(nvc.Get("ddlVisitTypeSearch")) : null,
                    sortexpression, 1,
                    1,
                   100,
                    ref iRowCount,
                    ref iTotalPageCount,
                    nvc != null ? General.GetNullableInteger(nvc.Get("ddlExpensetype")) : null,
                    nvc != null ? General.GetNullableString(nvc.Get("txtPICName")) : null);
                }
                else
                {

                    ds = PhoenixAccountsVesselVisitITSuperintendentRegister.VisitList(null,
                      nvc != null ? General.GetNullableString(nvc.Get("txtEmployeeNameSearch")) : null,
                       nvc != null ? General.GetNullableString(nvc.Get("txtFormNo")) : null,
                      nvc != null ? General.GetNullableString(nvc.Get("chkVesselList")) : null,
                      nvc != null ? General.GetNullableDateTime(nvc.Get("txtstartFromDate")) : null,
                      nvc != null ? General.GetNullableDateTime(nvc.Get("txtstartToDate")) : null,
                      nvc != null ? General.GetNullableDateTime(nvc.Get("txtendFromDate")) : null,
                      nvc != null ? General.GetNullableDateTime(nvc.Get("txtendToDate")) : null,
                      nvc != null ? General.GetNullableString(nvc.Get("txtVisitStatus")) : null,
                      nvc != null ? General.GetNullableString("PA") : null,
                      nvc != null ? General.GetNullableString(nvc.Get("ddlVisitTypeSearch")) : null,
                      sortexpression, 1,
                      1,
                      100,
                      ref iRowCount,
                      ref iTotalPageCount,
                      nvc != null ? General.GetNullableInteger(nvc.Get("ddlExpensetype")) : null);
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["VisitId"] = ds.Tables[0].Rows[index]["FLDVISITID"].ToString();
                    ViewState["VisitType"] = ds.Tables[0].Rows[index]["FLDVISITTYPE"].ToString();
                    ViewState["Indexno"] = index;
                    Response.Redirect("../Accounts/AccountsVesselVIistTravelClaimRegister.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&VisitType=" + ViewState["VisitType"].ToString() + "&gvindex=" + ViewState["Indexno"], false);
                }
            }

            //String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            TravelClaimEdit();
            BindLineItem();
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
        if (e.Item is GridEditableItem)
        {
            RadLabel lblManualYN = (RadLabel)e.Item.FindControl("lblManualYN");

            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null) cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
            if (lblManualYN != null && lblManualYN.Text == "0")
                if (cmdDelete != null) cmdDelete.Visible = false;
        }
    }

    protected void gvTravelAdvance_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsVesselVIistTravelClaimRegister.TravelAdvanceManualDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , new Guid(((RadLabel)e.Item.FindControl("lblTravelAdvanceId")).Text));

                ucStatus.Text = "Travel advance deleted.";
                BindDataAdv();
                BindDataOtherAdv();
                gvTravelAdvance.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelAdvanceOther_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridFooterItem)
        {
            ImageButton cmdAdd = (ImageButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null) cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }
    }

    protected void gvLineItem_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            ImageButton cmdEditCancel = (ImageButton)e.Item.FindControl("cmdEditCancel");
            if (cmdEditCancel != null) cmdEditCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdEditCancel.CommandName);

            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            ImageButton cmdSave = (ImageButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            ImageButton cmdReject = (ImageButton)e.Item.FindControl("cmdReject");
            if (cmdReject != null) cmdReject.Visible = SessionUtil.CanAccess(this.ViewState, cmdReject.CommandName);

            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");

            RadLabel lblRejected = (RadLabel)e.Item.FindControl("lblRejected");
            if (ViewState["ClaimStatus"].ToString() != "NSNA")
            {
                //if (cmdReject != null) cmdReject.Visible = SessionUtil.CanAccess(this.ViewState, cmdReject.CommandName);
                if (cmdReject != null) cmdReject.Visible = false;
                if (cmdDelete != null) cmdDelete.Visible = false;
                if (cmdEdit != null) cmdEdit.Visible = false;
            }
            else
            {
                if (cmdReject != null) cmdReject.Visible = false;
                if (cmdDelete != null) cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
                if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
            }
            if (lblRejected != null && lblRejected.Text == "1")
            {
                //e.Row.ForeColor = System.Drawing.Color.Red;
                e.Item.Font.Strikeout = true;
                if (cmdEdit != null) cmdEdit.Visible = false;
                if (cmdReject != null) cmdReject.Visible = false;
            }
        }

        if (e.Item is GridEditableItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;

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
                RadComboBox ddlSupAttachedEdit = ((RadComboBox)e.Item.FindControl("ddlSupAttachedEdit"));
                ddlSupAttachedEdit.SelectedValue = lblSupAttachedEdit.Text.ToString();
            }

            RadLabel lblTypeEdit = (RadLabel)e.Item.FindControl("lblTypeEdit");
            if (lblTypeEdit != null && lblTypeEdit.Text != "")
            {
                RadComboBox ddlTypeEdit = ((RadComboBox)e.Item.FindControl("ddlTypeEdit"));
                if (ddlTypeEdit != null)
                {
                    ddlTypeEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                    ddlTypeEdit.SelectedValue = lblTypeEdit.Text.ToString();
                    // ddlTypeEdit.Items.Remove(ddlTypeEdit.Items.FindByValue("TRCH"));
                }
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
                int? Account = General.GetNullableInteger(ViewState["VesselAccountId"].ToString());
                int? BudgetId = General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text);
                ProjectCode.bind(Account, BudgetId);
                ProjectCode.SelectedProjectCode = drv["FLDPROJECTID"].ToString();
            }
        }

        if (e.Item is GridFooterItem)
        {
            UserControlCurrency uc = ((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd"));
            uc.ActiveCurrency = true;

            ImageButton cmdAdd = (ImageButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null) cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);

            DataSet ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimEdit(new Guid(ViewState["VisitId"].ToString()));
            DataRow dr = ds.Tables[0].Rows[0];

            RadTextBox txtBudgetCodeAdd = (RadTextBox)e.Item.FindControl("txtBudgetCodeAdd");
            if (txtBudgetCodeAdd != null) txtBudgetCodeAdd.Text = dr["FLDBUDGETCODE"].ToString();
			
			UserControlDate ucDateAdd = (UserControlDate)e.Item.FindControl("ucDateAdd");
            if (ucDateAdd != null) ucDateAdd.Text = dr["FLDFROMDATE"].ToString();


            RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtBudgetNameAdd");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetIdAdd");
            if (tb1 != null)
            {
                tb1.Attributes.Add("style", "visibility:hidden");
                tb1.Text = dr["FLDBUDGETID"].ToString();

            }
            tb1 = (RadTextBox)e.Item.FindControl("txtBudgetgroupIdAdd");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

            ImageButton ib = (ImageButton)e.Item.FindControl("btnShowBudgetAdd");
            if (ib != null)
            {
                ib.Attributes.Add("onclick", "return showPickList('spnPickListBudgetAdd', 'codehelp1', '', '../Common/CommonPickListBudgetRemainingBalance.aspx?budgetgroup=102&hardtypecode=30&vesselid=" + ViewState["VESSELID"].ToString() + "&Ownerid=" + Convert.ToString(ViewState["PRINCIPAL"]) + "&budgetdate=" + DateTime.Now.ToString() + "', true); ");
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

            if (ViewState["ClaimStatus"].ToString() == "NSNA")
            {
                if (cmdAdd != null) cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName); ;
            }
            else
            {
                if (cmdAdd != null) cmdAdd.Visible = false;
            }

            RadComboBox ddlTypeAdd = ((RadComboBox)e.Item.FindControl("ddlTypeAdd"));
            if (ddlTypeAdd != null)
            {
                //ddlTypeAdd.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                //ddlTypeAdd.Items.Remove(ddlTypeAdd.Items.FindByValue("TRCH"));
            }

            UserControls_UserControlProjectCode Projectcode = ((UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeAdd"));

            int? VesselAccount = General.GetNullableInteger(ViewState["VesselAccountId"].ToString());
            int? Budget = General.GetNullableInteger(((RadTextBox)e.Item.FindControl("txtBudgetIdAdd")).Text);
            Projectcode.bind(VesselAccount, Budget);

        }
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
            if (e.CommandName.ToUpper().Equals("ADDNEW"))
            {
                if (ViewState["TravelClaimId"] == null || ViewState["TravelClaimId"].ToString() == string.Empty)
                {
                    PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , new Guid(ViewState["VisitId"].ToString())
                                                                                    , General.GetNullableGuid(ViewState["TravelClaimId"].ToString())
                                                                                    , General.GetNullableDateTime(ucDate.Text)
                                                                                    , General.GetNullableInteger(ddlCurrency.SelectedCurrency)
                                                                                    , txtRemarks.Text);

                    TravelClaimEdit();

                }
                if (!IsValidLineItem(((UserControlDate)e.Item.FindControl("ucDateAdd")).Text
                                        , ((UserControlCountry)e.Item.FindControl("ucCountryAdd")).SelectedCountry
                                        , ((RadComboBox)e.Item.FindControl("ddlTypeAdd")).SelectedValue
                                        , ((UserControlCurrency)e.Item.FindControl("ucCurrencyAdd")).SelectedCurrency
                                        , ((UserControlMaskNumber)e.Item.FindControl("txtAmountAdd")).Text
                                        , ((RadComboBox)e.Item.FindControl("ddlSupAttachedAdd")).SelectedValue
                                        , ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text
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
                     , ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text
                     , int.Parse(((RadComboBox)e.Item.FindControl("ddlSupAttachedAdd")).SelectedValue)
                     , int.Parse(ViewState["VESSELID"].ToString())
                    , General.GetNullableInteger(((UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeAdd")).SelectedProjectCode)

                     );

                ucStatus.Text = "Travel claim line item inserted";

                BindLineItem();
                BindDataAdv();
                BindDataOtherAdv();
                LineItemRebind();

            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

                if (!IsValidLineItem(((UserControlDate)e.Item.FindControl("ucDateEdit")).Text
                                        , ((UserControlCountry)e.Item.FindControl("ucCountryEdit")).SelectedCountry
                                        , ((RadComboBox)e.Item.FindControl("ddlTypeEdit")).SelectedValue
                                        , ((UserControlCurrency)e.Item.FindControl("ucCurrencyEdit")).SelectedCurrency
                                        , ((UserControlMaskNumber)e.Item.FindControl("txtAmountEdit")).Text
                                        , ((RadComboBox)e.Item.FindControl("ddlSupAttachedEdit")).SelectedValue
                                        , ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text

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
                     , ((RadTextBox)e.Item.FindControl("txtRemarksEdit")).Text
                     , int.Parse(((RadComboBox)e.Item.FindControl("ddlSupAttachedEdit")).SelectedValue)
                     , int.Parse(ViewState["VESSELID"].ToString())
                     , General.GetNullableInteger(((UserControls_UserControlProjectCode)e.Item.FindControl("ucProjectcodeEdit")).SelectedProjectCode)
                     

                     );

                ucStatus.Text = "Travel claim line item updated";
                BindLineItem();
                ViewState["SupAttachChange"] = null;

            }
            if (e.CommandName.ToUpper().Equals("REJECT"))
            {
                String scriptpopup = String.Format(
                   "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsVesselVisitTravelClaimRejectRemarks.aspx?ClaimLineitemId=" + ((RadLabel)e.Item.FindControl("lblClaimLineitemId")).Text + "','medium');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                //PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimLineItemReject(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //     , new Guid(((RadLabel)e.Item.FindControl("lblClaimLineitemId")).Text));
                //ucStatus.Text = "Travel claim line item rejected";
                BindLineItem();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                    new Guid(((RadLabel)e.Item.FindControl("lblClaimLineitemId")).Text)
                                                                                    );

                BindLineItem();
            }
            BindDataAdv();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindCurrency()
    {
        //ddlCurrency.Items.Insert(0, new ListItem("SGD", "9"));
        //ddlCurrency.Items.Insert(0, new ListItem("INR", "4"));
        //ddlCurrency.Items.Insert(0, new ListItem("--Select--", "null"));
    }


    public void SetStatus()
    {
        try
        {
            if (ViewState["VisitId"] != null)
            {
                DataSet ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimEdit(new Guid(ViewState["VisitId"].ToString()));
                DataRow dr = ds.Tables[0].Rows[0];

                //txtEmployee.Text = dr["FLDEMPLOYEEID"].ToString() + " / " + dr["FLDEMPLOYEENAME"].ToString();
                //ucDate.Text = dr["FLDSUBMITTEDDATE"].ToString();
                //txtVessel.Text = dr["FLDVESSELNAME"].ToString();
                //txtPort.Text = dr["FLDSEAPORTNAME"].ToString();
                //txtPurpose.Text = dr["FLDPURPOSE"].ToString();
                //txtAccount.Text = dr["FLDDESCRIPTION"].ToString();
                txtClaimStatus.Text = dr["FLDQUICKNAME"].ToString();
                //txtRemarks.Text = dr["FLDREMARKS"].ToString();
                //ddlCurrency.SelectedCurrency = dr["FLDREIMBURSEMENTCURRENCY"].ToString();
                //ucFromDate.Text = dr["FLDFROMDATE"].ToString();
                //ucToDate.Text = dr["FLDTODATE"].ToString();
                //ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                //ViewState["TravelClaimId"] = dr["FLDTRAVELCLAIMID"].ToString();
                //ViewState["VISITDTKEY"] = dr["FLDVISITDTKEY"].ToString();
                ViewState["ClaimStatus"] = dr["FLDCLAIMSTATUS"].ToString();
                //txtCountry.Text = dr["FLDCOUNTRYNAME"].ToString();
                //ucVisitStartDate.Text = dr["FLDONBOARDDATE"].ToString();
                //ucVisitEndDate.Text = dr["FLDDISEMBARKATIONDATE"].ToString();
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
                DataSet ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimEdit(new Guid(ViewState["VisitId"].ToString()));
                DataRow dr = ds.Tables[0].Rows[0];

                txtEmployee.Text = dr["FLDEMPLOYEEID"].ToString() + " / " + dr["FLDEMPLOYEENAME"].ToString();
                ucDate.Text = dr["FLDSUBMITTEDDATE"].ToString();
                txtVessel.Text = dr["FLDVESSELNAME"].ToString();
                txtPort.Text = dr["FLDSEAPORTNAME"].ToString();
                txtPurpose.Text = dr["FLDPURPOSE"].ToString();
                txtAccount.Text = dr["FLDDESCRIPTION"].ToString();
                ViewState["VesselAccountId"] = dr["FLDVESSELACCOUNTID"].ToString();
                txtClaimStatus.Text = dr["FLDQUICKNAME"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                ddlCurrency.SelectedCurrency = dr["FLDREIMBURSEMENTCURRENCY"].ToString();
                ucFromDate.Text = dr["FLDFROMDATE"].ToString();
                ucToDate.Text = dr["FLDTODATE"].ToString();
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                ViewState["TravelClaimId"] = dr["FLDTRAVELCLAIMID"].ToString();
                ViewState["VISITDTKEY"] = dr["FLDVISITDTKEY"].ToString();
                ViewState["ClaimStatus"] = dr["FLDCLAIMSTATUS"].ToString();
                txtCountry.Text = dr["FLDCOUNTRYNAME"].ToString();
                ucVisitStartDate.Text = dr["FLDONBOARDDATE"].ToString();
                ucVisitEndDate.Text = dr["FLDDISEMBARKATIONDATE"].ToString();
                txtRevokeremarks.Text = dr["FLDREVOKEREMARKS"].ToString().Replace("^", "\n");
                ViewState["Employeeid"] = dr["FLDEMPLOYEEID"].ToString();
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

            string[] alColumns = { "FLDROWNO", "FLDDATE", "FLDCOUNTRYNAME", "FLDCLAIMTYPENAME", "FLDSUBACCOUNT", "FLDOWNERBUDGETCODENAME", "FLDPROJECTCODE", "FLDEXPENSEDESCRIPTION", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDREMARKS", "FLDSUPATTACHED" };
            string[] alCaptions = { "Item No", "Date", "Country", "Type", "Budget Code", "Owner Budget Code","Project Code", "Expense Description", "Currency", "Amount", "Remarks", "Supporting Attached" };

            DataSet ds = new DataSet();

            ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimLineItemList(new Guid(ViewState["VisitId"].ToString())
                    , General.GetNullableGuid(ViewState["TravelClaimId"].ToString()),
                    (int)ViewState["PAGENUMBERLINE"],
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount);

            General.SetPrintOptions("gvLineItem", "Travel Claim", alCaptions, alColumns, ds);

            gvLineItem.DataSource = ds;
            gvLineItem.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNTLINE"] = iRowCount;
            ViewState["TOTALPAGECOUNTLINE"] = iTotalPageCount;
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

            string[] alColumns = { "FLDTRAVELADVANCENUMBER", "FLDPAIDDATE", "FLDCURRENCYCODE", "FLDPAYMENTAMOUNT", "FLDRETURNAMOUNT", "FLDMONEYCHANGER", "FLDCLAIMAMOUNT", "FLDBALANCE", "FLDBALANCESGD", "FLDQUICKNAME" };
            string[] alCaptions = { "Travel Advance Number", "Paid Date", "Currency", "Payment Amount", "Return Amount", "Money Changer", "Claim Amount", "Balance", "Balance in SGD", "Advance Status" };

            DataSet ds = new DataSet();

            ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimAdvanceList(new Guid(ViewState["VisitId"].ToString()),
                    (int)ViewState["PAGENUMBERADV"],
                    General.ShowRecords(null),
                    ref iRowCount,
                    ref iTotalPageCount,
                    ref iBalanceInSGD);

            strBalanceInSGD = String.Format("{0:n}", iBalanceInSGD);
            General.SetPrintOptions("gvTravelAdvance", "Travel Advance", alCaptions, alColumns, ds);

            gvTravelAdvance.DataSource = ds;
            gvTravelAdvance.VirtualItemCount = iRowCount; ;

            ViewState["ROWCOUNTADV"] = iRowCount;
            ViewState["TOTALPAGECOUNTADV"] = iTotalPageCount;
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
            gvTravelAdvanceOther.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNTOTHERADV"] = iRowCount;
            ViewState["TOTALPAGECOUNTOTHERADV"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcelLine()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDROWNO", "FLDDATE", "FLDCOUNTRYNAME", "FLDCLAIMTYPENAME", "FLDSUBACCOUNT", "FLDOWNERBUDGETCODENAME", "FLDPROJECTCODE", "FLDEXPENSEDESCRIPTION", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDREMARKS", "FLDSUPATTACHED" };
        string[] alCaptions = { "Item No", "Date", "Country", "Type", "Budget Code", "Owner Budget Code","Project Code", "Expense Description", "Currency", "Amount", "Remarks", "Supporting Attached" };

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

    private bool IsValidData(string date, string currency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (currency.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Reimbursement curency is required.";

        //if (General.GetNullableDateTime(date)== null)
        //    ucError.ErrorMessage = "Date is required.";

        return (!ucError.IsError);

    }

    private bool IsValidLineItem(string date, string country, string type, string currency, string amount, string supAttached, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "Date is required.";
        if (General.GetNullableInteger(country) == null )
            ucError.ErrorMessage = "Country is required.";
        if (type.Trim().Equals(""))
            ucError.ErrorMessage = "Type is required.";
        if (remarks.Trim().Equals("MEX") && remarks == string.Empty)
            ucError.ErrorMessage = "Remarks is required.";
        if (currency.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Currency is required.";
        if (amount.Trim().Equals(""))
            ucError.ErrorMessage = "Amount is required.";
        if (supAttached.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Supporting attached is required.";

        return (!ucError.IsError);

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        TravelClaimEdit();
        BindLineItem();
        BindDataAdv();
        BindDataOtherAdv();

        if (Session["OPTION"] != null && Session["OPTION"].ToString() == "No")
        {
            Response.Redirect("../Accounts/AccountsVesselVisitTravelAdvanceRequest.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&VisitType=" + ViewState["VisitType"].ToString(), false);
            Session.Remove("OPTION");
        }

        //int? VesselAccount = General.GetNullableInteger(ViewState["VesselAccountId"].ToString());
        //int? Budget = General.GetNullableInteger(txtBulkBudgetId.Text);

        //ucProjectcode.bind(VesselAccount, Budget);
    }

    protected void gvTravelAdvanceOther_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                PhoenixAccountsVesselVIistTravelClaimRegister.TravelAdvanceManualInsert(new Guid(ViewState["VisitId"].ToString())
                     , new Guid(((RadLabel)e.Item.FindControl("lblTravelAdvanceId")).Text)
                     , new Guid(ViewState["TravelClaimId"].ToString())
                     , PhoenixSecurityContext.CurrentSecurityContext.UserCode);

                ucStatus.Text = "Travel claim line item inserted";

                BindLineItem();
                BindDataAdv();
                BindDataOtherAdv();
                gvTravelAdvanceOther.Rebind();
                gvTravelAdvance.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlTypeAdd_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        //RadComboBox ddlTypeAdd = gvLineItem.FooterRow.FindControl("ddlTypeAdd") as RadComboBox;
        //RadComboBox ddlSupAttachedAdd = gvLineItem.FooterRow.FindControl("ddlSupAttachedAdd") as RadComboBox;
        //if (ddlTypeAdd != null && ddlTypeAdd.SelectedValue == "TRA")
        //{
        //    if (ddlSupAttachedAdd != null && ddlSupAttachedAdd.SelectedValue == "Dummy")
        //        ddlSupAttachedAdd.SelectedValue = "0";
        //}
    }

  protected void ddlTypeEdit_OnSelectedIndexChanged(object sender, EventArgs e)
   {
        //int nCurrentRow = gvLineItem.Items.RowIndex;

        //RadComboBox ddlTypeEdit = gvLineItem.Items[nCurrentRow].FindControl("ddlTypeEdit") as RadComboBox;
        //RadComboBox ddlSupAttachedEdit = gvLineItem.Items[nCurrentRow].FindControl("ddlSupAttachedEdit") as RadComboBox;
        //if (ddlTypeEdit != null && ddlTypeEdit.SelectedValue == "TRA")
        //{
        //    if (ddlSupAttachedEdit != null && ViewState["SupAttachChange"] == null)
        //    {
        //        ddlSupAttachedEdit.SelectedValue = "0";
        //        ViewState["SupAttachChange"] = "1";
        //    }
        //}
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
            PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                             , new Guid(ViewState["VisitId"].ToString())
                                                                             , General.GetNullableGuid(ViewState["TravelClaimId"].ToString())
                                                                             , General.GetNullableDateTime(ucDate.Text)
                                                                             , General.GetNullableInteger(ddlCurrency.SelectedCurrency)
                                                                             , txtRemarks.Text,
                                                                             General.GetNullableInteger(txtBulkBudgetId.Text),
                                                                             General.GetNullableGuid(txtBulkOwnerBudgetId.Text),
                                                                             General.GetNullableInteger(ucProjectcode.SelectedProjectCode)
                                                                             );

            ucStatus.Text = "Budget Codes updated Successfully.";

            TravelClaimEdit();
            BindLineItem();
            BindDataAdv();
            BindDataOtherAdv();


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

            ucStatus.Text = "Remark Added.";
            TravelClaimEdit();
            BindLineItem();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        int? VesselAccountId = General.GetNullableInteger(ViewState["VesselAccountId"].ToString());

        GridFooterItem footerItem = (GridFooterItem)gvLineItem.MasterTableView.GetItems(GridItemType.Footer)[0];
        ImageButton btbudget = (ImageButton)footerItem.FindControl("btnShowBudgetAdd");
        UserControls_UserControlProjectCode ProjectCodeAdd = (UserControls_UserControlProjectCode)footerItem.FindControl("ucProjectcodeAdd");
        int? Budget = General.GetNullableInteger(((RadTextBox)footerItem.FindControl("txtBudgetIdAdd")).Text);

        if (btbudget != null)
        {
            ProjectCodeAdd.bind(VesselAccountId, Budget);
        }


        if (btnShowBulkBudget != null)
        {
            int? bulkBudget = General.GetNullableInteger(txtBulkBudgetId.Text);
            ucProjectcode.bind(VesselAccountId, bulkBudget);
        }

    }

    protected void txtBudgetIdEdit_TextChanged(object sender, EventArgs e)
    {
        int? VesselAccountId = General.GetNullableInteger(ViewState["VesselAccountId"].ToString());

        GridDataItem Item = (GridDataItem)gvLineItem.MasterTableView.GetItems(GridItemType.EditItem)[0];

        ImageButton ib1 = (ImageButton)Item.FindControl("btnShowBudgetEdit");
        UserControls_UserControlProjectCode ProjectCodeEdit = (UserControls_UserControlProjectCode)Item.FindControl("ucProjectcodeEdit");
         int ? BudgetEditid = General.GetNullableInteger(((RadTextBox)Item.FindControl("txtBudgetIdEdit")).Text);

        if (ib1 != null)
        {
            ProjectCodeEdit.bind(VesselAccountId, BudgetEditid);
        }

    }


}