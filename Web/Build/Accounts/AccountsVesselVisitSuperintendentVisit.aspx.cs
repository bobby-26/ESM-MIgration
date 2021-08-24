using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsVesselVisitSuperintendentVisit : PhoenixBasePage
{
    public int showSave = 1;
    public int showCancel = 0;
    public int showApproval = 0;
    public int showApprove = 0;
    public int showComplete = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
                      
            //toolbar.AddButton("Riding Superintendent Visit - India", "RIDSUPVISIT");
            toolbar.AddButton("Local Claim/Business Travel", "OWNER",ToolBarDirection.Right);
            toolbar.AddButton("Vessel Visit", "SUPVISIT", ToolBarDirection.Right);
            toolbar.AddButton("IT Visit", "ITVISIT", ToolBarDirection.Right);
            MenuSupVisit.AccessRights = this.ViewState;
            MenuSupVisit.MenuList = toolbar.Show();
            MenuSupVisit.SelectedMenuIndex = 1;
            if (!IsPostBack)
            {
                if (Request.QueryString["visitId"] != "")
                    ViewState["VisitId"] = Request.QueryString["visitId"];
                else
                    ViewState["VisitId"] = null;

                ViewState["VisitStatus"] = null;
                ViewState["ClaimStatus"] = null;
                ddlVessel.bind();
                BindData();

                PhoenixToolbar toolbarsub = new PhoenixToolbar();
                if (showComplete == 1)
                {
                    toolbarsub.AddButton("Complete", "COMPLETE", ToolBarDirection.Right);
                    ucVisitStartDate.CssClass = "input_mandatory";
                    ucVisitEndDate.CssClass = "input_mandatory";
                }
                if (showSave == 1)
                {
                    toolbarsub.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    ucVisitStartDate.CssClass = "input";
                    ucVisitEndDate.CssClass = "input";
                }
                if (showApprove == 1)
                    toolbarsub.AddButton("Approve", "APPROVE", ToolBarDirection.Right);

                if (showApproval == 1)
                    toolbarsub.AddButton("Submit for Approval", "APPROVAL", ToolBarDirection.Right);

                if (showCancel == 1)
                    toolbarsub.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);

                MenuITVisitSub.AccessRights = this.ViewState;
                if (showCancel == 1 || showComplete == 1 || showApproval == 1 || showApprove == 1 || showSave == 1)
                    MenuITVisitSub.MenuList = toolbarsub.Show();
            }
         //   BindData();




        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuITVisitSub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData(ddlVessel.SelectedVessel.ToString(), ddlAccountDetails.SelectedValue, txtuserid.Text, ucPortMulti.Text,
                    ucVisitStartDate.Text, ucVisitEndDate.Text, txtPurpose.Text, ddlBudgetedVisit.SelectedValue, ddlDepartment.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["VisitId"] == null || ViewState["VisitId"].ToString() == string.Empty)
                {
                    string fromTime = (txtFromTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtFromTime.Text;
                    string toTime = (txtToTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtToTime.Text;

                    PhoenixAccountsVesselVisitITSuperintendentRegister.InsertVisit(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        ddlVessel.SelectedValue,
                        General.GetNullableInteger(ddlAccountDetails.SelectedValue.ToString()),
                        Int32.Parse(txtuserid.Text),
                        Int32.Parse(ucPortMulti.SelectedValue),
                        General.GetNullableDateTime(ucFromDate.Text + " " + fromTime),
                        General.GetNullableDateTime(ucToDate.Text + " " + toTime),
                         General.GetNullableDateTime(ucVisitStartDate.Text),
                        General.GetNullableDateTime(ucVisitEndDate.Text),
                        General.GetNullableDecimal(txtTravelHours.Text),
                        2,
                        txtPurpose.Text,
                        General.GetNullableInteger(ddlBudgetedVisit.SelectedValue),
                        General.GetNullableInteger(ddlcountrylist.SelectedCountry),
                        General.GetNullableInteger(ddlDepartment.SelectedValue),
                         General.GetNullableInteger(txtpicid.Text));
                    ucStatus.Text = "Superintendent Visit Saved";
                    String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                    Session["New"] = "Y";
                }
                else
                {
                    string fromTime = (txtFromTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtFromTime.Text;
                    string toTime = (txtToTime.Text.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : txtToTime.Text;
                    PhoenixAccountsVesselVisitITSuperintendentRegister.UpdateVisit(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["VisitId"].ToString()),
                        ddlVessel.SelectedValue,
                        General.GetNullableInteger(ddlAccountDetails.SelectedValue.ToString()),
                        Int32.Parse(txtuserid.Text),
                        Int32.Parse(ucPortMulti.SelectedValue),
                        General.GetNullableDateTime(ucFromDate.Text + " " + fromTime),
                        General.GetNullableDateTime(ucToDate.Text + " " + toTime),
                        General.GetNullableDateTime(ucVisitStartDate.Text),
                        General.GetNullableDateTime(ucVisitEndDate.Text),
                        General.GetNullableDecimal(txtTravelHours.Text),
                        2,
                        txtPurpose.Text,
                        General.GetNullableInteger(ddlBudgetedVisit.SelectedValue),
                        General.GetNullableInteger(ddlcountrylist.SelectedCountry),
                        General.GetNullableInteger(ddlDepartment.SelectedValue),
                        General.GetNullableInteger(txtpicid.Text)
                        );
                    ucStatus.Text = "Superintendent Visit updated";
                }
                String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
            if (CommandName.ToUpper().Equals("CANCEL"))
            {
                if (ViewState["Claimexists"].ToString() == "1")
                {
                    ucError.ErrorMessage = "Kindly remove your travel claim line item before cancel";
                    ucError.Visible = true;
                    return;
                }
                String scriptpopup = String.Format(
                   "javascript:parent.openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/Accounts/AccountsVesselVisitCancellationRemarks.aspx?visitId=" + ViewState["VisitId"].ToString() + "','medium');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (CommandName.ToUpper().Equals("COMPLETE"))
            {
                PhoenixAccountsVesselVisitITSuperintendentRegister.VisitComplete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["VisitId"].ToString()),
                    General.GetNullableDateTime(ucVisitStartDate.Text),
                    General.GetNullableDateTime(ucVisitEndDate.Text));
                ucStatus.Text = "Superintendent visit completed";
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (CommandName.ToUpper().Equals("APPROVAL"))
            {

                //String scriptpopup = String.Format(
                //       "javascript:parent.Openpopup('codehelp1', '', '../Accounts/AccountsVesselVisitSubmitForApprovalConfirmation.aspx?visitId=" + ViewState["VisitId"].ToString() + "','medium');");
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

                if (ViewState["Advexists"].ToString() == "1")
                {
                    PhoenixAccountsVesselVisitITSuperintendentRegister.VisitSubmitForApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["VisitId"].ToString()));
                    String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                }
                else
                {
                    String scriptpopup = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"]+"/Accounts/AccountsVesselVisitSubmitForApprovalConfirmation.aspx?visitId=" + ViewState["VisitId"].ToString() + "','large');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }


            }
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                if (General.GetNullableInteger(ddlBudgetedVisit.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Budgeted visit is required.";
                    ucError.Visible = true;
                    return;
                }

                //String scriptpopup = String.Format(
                //   "javascript:parent.Openpopup('codehelp1', '', '../Accounts/AccountsVesselVisitTravelClaimApproveConfirmation.aspx?visitId=" + ViewState["VisitId"].ToString() + "');");
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

                if (ViewState["Advexists"].ToString() == "1")
                {

                    String scriptpopup = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/Accounts/AccountsVesselVisitTravelClaimApproveConfirmation.aspx?visitId=" + ViewState["VisitId"].ToString() + "');");
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
                }
                else
                {
                    PhoenixAccountsVesselVisitITSuperintendentRegister.VisitApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["VisitId"].ToString()));
                    String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSupVisit_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("ITVISIT"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitITVisit.aspx?visitId=");
            }

            else if (CommandName.ToUpper().Equals("SUPVISIT"))
            {
                PhoenixSecurityContext psc = PhoenixSecurityContext.CurrentSecurityContext;
                DataSet ds = PhoenixRegistersOfficeStaff.EditOfficeStaffByUserID(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                DataRow dr = ds.Tables[0].Rows[0];

                txtuserid.Text = dr["FLDOFFICESTAFFID"].ToString();
                txtMentorName.Text = psc.FirstName + " " + psc.MiddleName + " " + psc.LastName;

                ViewState["VisitId"] = string.Empty;
                txtFormNo.Text = string.Empty;
                BindVesselAccount(0);
                ddlVessel.SelectedVessel = string.Empty;
                ddlAccountDetails.SelectedValue = string.Empty;
                //txtuserid.Text = string.Empty;
                //txtMentorName.Text = string.Empty;
                ucPortMulti.SelectedValue = string.Empty;
                ucPortMulti.Text = string.Empty;
                ucVisitStartDate.Text = string.Empty;
                ucVisitEndDate.Text = string.Empty;
                ucFromDate.Text = string.Empty;
                ucToDate.Text = string.Empty;
                txtToTime.Text = string.Empty;
                txtTravelHours.Text = string.Empty;
                txtTravelHours.Text = string.Empty;
                txtCreatedBy.Text = PhoenixSecurityContext.CurrentSecurityContext.UserName.ToString();
                ucCreatedDate.Text = DateTime.Now.ToString();
                txtCompletedBy.Text = string.Empty;
                ucCompletedDate.Text = string.Empty;
                txtPurpose.Text = string.Empty;
                txtAccountVoucherNo.Text = string.Empty;
                ddlVessel.Enabled = true;
                txtMentorName.Enabled = true;
                txtMentorName.CssClass = "input";
                imguser.Visible = true;
                ddlBudgetedVisit.SelectedValue = "Dummy";
                ddlcountrylist.SelectedCountry = string.Empty;
                txtpicid.Text = string.Empty;
                txtPICName.Text = string.Empty;
                txtClaimCurrencycode.Text = string.Empty;
                txtClaimamount.Text = string.Empty;
                txtReimbursementamount.Text = string.Empty;
                txtCancelremarks.Text = string.Empty;

                PhoenixToolbar toolbarsub = new PhoenixToolbar();
                toolbarsub.AddButton("Save", "SAVE",ToolBarDirection.Right);
                ucVisitStartDate.CssClass = "input";
                ucVisitEndDate.CssClass = "input";
                MenuITVisitSub.AccessRights = this.ViewState;
                MenuITVisitSub.MenuList = toolbarsub.Show();

            }
            else if (CommandName.ToUpper().Equals("RIDSUPVISIT"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitRidingSuperintendentVisit.aspx?visitId=");
            }
            else if (CommandName.ToUpper().Equals("OWNER"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitOwnerBusinessTravel.aspx?visitId=");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {

        try
        {
            if (ViewState["VisitId"] != null && ViewState["VisitId"].ToString() != string.Empty)
            {
                DataSet ds = PhoenixAccountsVesselVisitITSuperintendentRegister.VisitSearch(new Guid(ViewState["VisitId"].ToString()));
                DataRow dr = ds.Tables[0].Rows[0];

                ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
                    General.GetNullableInteger(dr["FLDVESSELID"].ToString()), 1);
                ddlAccountDetails.DataBind();

                ddlDepartment.DataSource = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 3, null);
                ddlDepartment.DataBind();
                ddlDepartment.SelectedValue = dr["FLDDEPARTMENTID"].ToString();


                txtFormNo.Text = dr["FLDFORMNUMBER"].ToString();
                ddlVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ddlAccountDetails.SelectedValue = dr["FLDVESSELACCOUNTID"].ToString();
                txtuserid.Text = dr["FLDEMPLOYEEID"].ToString();
                txtMentorName.Text = dr["FLDEMPLOYEENAME"].ToString();
                ucPortMulti.SelectedValue = dr["FLDPORTID"].ToString();
                ucPortMulti.Text = dr["FLDAIRPORTNAME"].ToString();
                ucFromDate.Text = dr["FLDFROMDATE"].ToString();
                ucToDate.Text = dr["FLDTODATE"].ToString();
                ucVisitStartDate.Text = dr["FLDVISITSTARTDATE"].ToString();
                ucVisitEndDate.Text = dr["FLDVISITENDDATE"].ToString();
                txtTravelHours.Text = dr["FLDTRAVELHOURS"].ToString();
                txtCreatedBy.Text = dr["FLDVISITCREATEDBY"].ToString();
                ucCreatedDate.Text = dr["FLDVISITCREATEDDATE"].ToString();
                txtCompletedBy.Text = dr["FLDCOMPLETEDBY"].ToString();
                ucCompletedDate.Text = dr["FLDCOMPLETEDDATE"].ToString();
                txtPurpose.Text = dr["FLDPURPOSE"].ToString();
                txtAccountVoucherNo.Text = dr["FLDVOUCHERNUMBER"].ToString();
                ViewState["VisitStatus"] = dr["FLDVISITSTATUS"].ToString();
                ViewState["ClaimStatus"] = dr["FLDCLAIMSTATUS"].ToString();
                showSave = int.Parse(dr["FLDSHOWSAVE"].ToString());
                showCancel = int.Parse(dr["FLDSHOWCANCEL"].ToString());
                showApproval = int.Parse(dr["FLDSHOWAPPROVAL"].ToString());
                showApprove = int.Parse(dr["FLDSHOWAPPROVE"].ToString());
                showComplete = int.Parse(dr["FLDSHOWCOMPLETE"].ToString());
                ddlVessel.Enabled = false;
                txtMentorName.Enabled = false;
                txtMentorName.CssClass = "readonlytextbox";
                imguser.Visible = false;
                ddlBudgetedVisit.SelectedValue = (dr["FLDBUDGETEDVISIT"].ToString());
                ddlcountrylist.SelectedCountry = dr["FLDCOUNTRYID"].ToString();
                ViewState["Claimexists"] = dr["FLDISCLAIMEXISTS"].ToString();
                ViewState["Advexists"] = dr["FLDADVANCEEXISTS"].ToString();
                txtCancelremarks.Text = dr["FLDCANCELLATIONREMARKS"].ToString();
                txtpicid.Text = dr["FLDCLAIMPIC"].ToString();
                txtPICName.Text = dr["FLDCLAIMPICNAME"].ToString();
                txtClaimCurrencycode.Text = dr["FLDCLAIMCURRENCYCODE"].ToString();
                txtClaimamount.Text = dr["FLDCLAIMAMOUNT"].ToString();
                txtReimbursementamount.Text = dr["FLDREIMBURSEMENTAMOUNT"].ToString();
            }
            else
            {
                ddlDepartment.DataSource = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 3, null);
                ddlDepartment.DataBind();
                ddlDepartment.SelectedValue = string.Empty;

                PhoenixSecurityContext psc = PhoenixSecurityContext.CurrentSecurityContext;
                DataSet ds = PhoenixRegistersOfficeStaff.EditOfficeStaffByUserID(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                DataRow dr = ds.Tables[0].Rows[0];

                txtuserid.Text = dr["FLDOFFICESTAFFID"].ToString();
                txtMentorName.Text = psc.FirstName + " " + psc.MiddleName + " " + psc.LastName;

                BindVesselAccount(0);
                txtFormNo.Text = string.Empty;
                ddlVessel.SelectedVessel = string.Empty;
                ddlAccountDetails.SelectedValue = string.Empty;
                //txtuserid.Text = string.Empty;
                //txtMentorName.Text = string.Empty;
                ucPortMulti.SelectedValue = string.Empty;
                ucPortMulti.Text = string.Empty;
                ucVisitStartDate.Text = string.Empty;
                ucVisitEndDate.Text = string.Empty;
                ucFromDate.Text = string.Empty;
                ucToDate.Text = string.Empty;
                txtTravelHours.Text = string.Empty;
                txtCreatedBy.Text = PhoenixSecurityContext.CurrentSecurityContext.UserName.ToString();
                ucCreatedDate.Text = DateTime.Now.ToString();
                txtCompletedBy.Text = string.Empty;
                ucCompletedDate.Text = string.Empty;
                txtPurpose.Text = string.Empty;
                txtAccountVoucherNo.Text = string.Empty;
                ddlVessel.Enabled = true;
                txtMentorName.Enabled = true;
                txtMentorName.CssClass = "input";
                imguser.Visible = true;
                ddlBudgetedVisit.SelectedValue = "Dummy";
                ddlcountrylist.SelectedCountry = string.Empty;
                txtCancelremarks.Text = string.Empty;
                txtpicid.Text = string.Empty;
                txtPICName.Text = string.Empty;
                txtClaimCurrencycode.Text = string.Empty;
                txtClaimamount.Text = string.Empty;
                txtReimbursementamount.Text = string.Empty;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void ddlDepartment_DataBound(object sender, EventArgs e)
    {
        ddlDepartment.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    protected void ddlVessel_Changed(object sender, EventArgs e)
    {
        BindVesselAccount(General.GetNullableInteger(ddlVessel.SelectedVessel) == null ? 0 : General.GetNullableInteger(ddlVessel.SelectedVessel));
    }
    protected void BindVesselAccount(int? vesselid)
    {
        ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(
            vesselid, 1);
        ddlAccountDetails.DataBind();
        if (ddlAccountDetails.Items.Count == 2)
            ddlAccountDetails.SelectedIndex = 1;
    }

    private bool IsValidData(string vessel, string vesselAccount, string employeeId, string port, string visitStartDate, string visitEndDate, string purpose, string budgetedvisit, string department)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessel.Trim().Equals(""))
            ucError.ErrorMessage = "Vessel is required.";

        if ((ddlAccountDetails.Items.Count > 1) && (vesselAccount.Trim().Equals("")))
            ucError.ErrorMessage = "Vessel account is required.";

        if ((txtuserid.Text == ""))
            ucError.ErrorMessage = "Employee Name is required.";

        if (port.Trim().Equals(""))
            ucError.ErrorMessage = "Port is required.";


        if (purpose.Trim().Equals(""))
            ucError.ErrorMessage = "Purpose is required.";

        if (budgetedvisit.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Budgeted visit is required.";

        if ((ddlDepartment.Items.Count > 1) && (department.Trim().Equals("")))
            ucError.ErrorMessage = "Department is required.";

        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }
}
