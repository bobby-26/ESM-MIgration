using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;

public partial class AccountsVesselVisitRidingSuperintendentVisit : PhoenixBasePage
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
            toolbar.AddButton("IT Visit", "ITVISIT");
            toolbar.AddButton("Superintendent Visit - Singapore", "SUPVISIT");
            //toolbar.AddButton("Riding Superintendent Visit - India", "RIDSUPVISIT");
            toolbar.AddButton("Local Claim/Business Travel", "OWNER");
            MenuSupVisit.AccessRights = this.ViewState;
            MenuSupVisit.MenuList = toolbar.Show();
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
            }
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            if (showCancel == 1)
                toolbarsub.AddButton("Cancel", "CANCEL");
            if (showComplete == 1)
                toolbarsub.AddButton("Complete", "COMPLETE");
            if (showApproval == 1)
                toolbarsub.AddButton("Submit for Approval", "APPROVAL");
            if (showApprove == 1)
                toolbarsub.AddButton("Approve", "APPROVE");
            if (showSave == 1)
                toolbarsub.AddButton("Save", "SAVE");
            MenuITVisitSub.AccessRights = this.ViewState;
            if (showCancel == 1 || showComplete == 1 || showApproval == 1 || showApprove == 1 || showSave == 1)
                MenuITVisitSub.MenuList = toolbarsub.Show();
            //Employeelist();

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //public void Employeelist()
    //{
    //    try
    //    {
    //        DataSet ds = null;
    //        ds = PhoenixAccountsVesselVisitITSuperintendentRegister.employeelist();
    //        ddlSubaccount.DataSource = ds;
    //        ddlSubaccount.DataTextField = Convert.ToString("FLDDESCRIPTION");
    //        ddlSubaccount.DataValueField = ("FLDSUBACCOUNTID");
    //        ddlSubaccount.DataBind();
    //        ddlSubaccount.Items.Insert(0, new ListItem("--Select--", ""));

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void MenuITVisitSub_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData(ddlVessel.SelectedVessel.ToString(), ddlAccountDetails.SelectedValue, txtuserid.Text, ucPortMulti.Text,
                    ucFromDate.Text, ucToDate.Text, txtPurpose.Text, ddlBudgetedVisit.SelectedValue))
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
                        General.GetNullableDecimal(txtTravelHours.Text),
                        3,
                        txtPurpose.Text,
                        General.GetNullableInteger(ddlBudgetedVisit.SelectedValue),
                        General.GetNullableInteger(ddlcountrylist.SelectedCountry), null, null);
                    ucStatus.Text = "Riding Superintendent Visit Saved";
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
                        General.GetNullableDecimal(txtTravelHours.Text),
                        3,
                        txtPurpose.Text,
                        General.GetNullableInteger(ddlBudgetedVisit.SelectedValue),
                        General.GetNullableInteger(ddlcountrylist.SelectedCountry), null, null);
                    ucStatus.Text = "Riding Superintendent Visit updated";
                }
                String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
            if (dce.CommandName.ToUpper().Equals("CANCEL"))
            {
                String scriptpopup = String.Format(
                   "javascript:parent.Openpopup('codehelp1', '', '../Accounts/AccountsVesselVisitCancellationRemarks.aspx?visitId=" + ViewState["VisitId"].ToString() + "','medium');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (dce.CommandName.ToUpper().Equals("COMPLETE"))
            {
                PhoenixAccountsVesselVisitITSuperintendentRegister.VisitComplete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["VisitId"].ToString()), null, null);
                ucStatus.Text = "Riding Superintendent visit completed";
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (dce.CommandName.ToUpper().Equals("APPROVAL"))
            {
                String scriptpopup = String.Format(
                   "javascript:parent.Openpopup('codehelp1', '', '../Accounts/AccountsVesselVisitSubmitForApprovalConfirmation.aspx?visitId=" + ViewState["VisitId"].ToString() + "','medium');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
            if (dce.CommandName.ToUpper().Equals("APPROVE"))
            {
                if (General.GetNullableInteger(ddlBudgetedVisit.SelectedValue) == null)
                {
                    ucError.HeaderMessage = "Please provide the following required information";
                    ucError.ErrorMessage = "Budgeted visit is required.";
                    ucError.Visible = true;
                    return;
                }
                String scriptpopup = String.Format(
                   "javascript:parent.Openpopup('codehelp1', '', '../Accounts/AccountsVesselVisitTravelClaimApproveConfirmation.aspx?visitId=" + ViewState["VisitId"].ToString() + "');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
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
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("ITVISIT"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitITVisit.aspx?visitId=");
            }

            else if (dce.CommandName.ToUpper().Equals("SUPVISIT"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitSuperintendentVisit.aspx?visitId=");
            }
            else if (dce.CommandName.ToUpper().Equals("RIDSUPVISIT"))
            {
                ViewState["VisitId"] = string.Empty;
                txtFormNo.Text = string.Empty;
                BindVesselAccount(0);
                ddlVessel.SelectedVessel = string.Empty;
                ddlAccountDetails.SelectedValue = string.Empty;
                //txtuserid.Text = string.Empty;
                //txtMentorName.Text = string.Empty;
                ucPortMulti.SelectedValue = string.Empty;
                ucPortMulti.Text = string.Empty;
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
                ddlBudgetedVisit.SelectedValue = "Dummy";
                ddlcountrylist.SelectedCountry = string.Empty;

                PhoenixToolbar toolbarsub = new PhoenixToolbar();
                toolbarsub.AddButton("Save", "SAVE");
                MenuITVisitSub.AccessRights = this.ViewState;
                MenuITVisitSub.MenuList = toolbarsub.Show();
            }
            else if (dce.CommandName.ToUpper().Equals("OWNER"))
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

                txtFormNo.Text = dr["FLDFORMNUMBER"].ToString();
                ddlVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ddlAccountDetails.SelectedValue = dr["FLDVESSELACCOUNTID"].ToString();
                txtuserid.Text = dr["FLDEMPLOYEEID"].ToString();
                txtMentorName.Text = dr["FLDEMPLOYEENAME"].ToString();
                ucPortMulti.SelectedValue = dr["FLDPORTID"].ToString();
                ucPortMulti.Text = dr["FLDAIRPORTNAME"].ToString();
                ucFromDate.Text = dr["FLDFROMDATE"].ToString();
                ucToDate.Text = dr["FLDTODATE"].ToString();
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
                ddlBudgetedVisit.SelectedValue = (dr["FLDBUDGETEDVISIT"].ToString());
                ddlcountrylist.SelectedCountry = dr["FLDCOUNTRYID"].ToString();
            }
            else
            {
                BindVesselAccount(0);
                txtFormNo.Text = string.Empty;
                ddlVessel.SelectedVessel = string.Empty;
                ddlAccountDetails.SelectedValue = string.Empty;
                // txtuserid.Text = string.Empty;
                // txtMentorName.Text = string.Empty;
                ucPortMulti.SelectedValue = string.Empty;
                ucPortMulti.Text = string.Empty;
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
                ddlBudgetedVisit.SelectedValue = "Dummy";
                ddlcountrylist.SelectedCountry = string.Empty;
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
        ddlAccountDetails.Items.Insert(0, new ListItem("--Select--", ""));
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

    private bool IsValidData(string vessel, string vesselAccount, string employeeId, string port, string fromDate, string toDate, string purpose, string budgetedvisit)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (vessel.Trim().Equals(""))
            ucError.ErrorMessage = "Vessel is required.";

        if ((ddlAccountDetails.Items.Count > 1) && (vesselAccount.Trim().Equals("")))
            ucError.ErrorMessage = "Vessel account is required.";

        if ((txtuserid.Text == ""))
            ucError.ErrorMessage = "Employee Id is required.";

        if (port.Trim().Equals(""))
            ucError.ErrorMessage = "Port is required.";

        if (fromDate == null)
            ucError.ErrorMessage = "From date is required.";

        if (toDate == null)
            ucError.ErrorMessage = "To date is required.";

        if (purpose.Trim().Equals(""))
            ucError.ErrorMessage = "Purpose is required.";

        if (budgetedvisit.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Budgeted visit is required.";

        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        String script = String.Format("javascript:parent.fnReloadList('code1');");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }
}
