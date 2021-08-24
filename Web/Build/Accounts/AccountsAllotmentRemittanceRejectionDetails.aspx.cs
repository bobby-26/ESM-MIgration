using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AccountsAllotmentRemittanceRejectionDetails : PhoenixBasePage
{
    string RemittanceId = string.Empty;
    string VesselId = string.Empty;
    string RemittanceNumber = string.Empty;
    string EmployeeBankId = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            RemittanceId = Request.QueryString["REMITTANCEID"] == null ? string.Empty : Request.QueryString["REMITTANCEID"].ToString();
            VesselId = Request.QueryString["VESSELID"] == null ? string.Empty : Request.QueryString["VESSELID"].ToString();
            RemittanceNumber = Request.QueryString["remittancenumber"] == null ? string.Empty : Request.QueryString["remittancenumber"].ToString();
            EmployeeBankId = Request.QueryString["employeebankaccountid"] == null ? string.Empty : Request.QueryString["employeebankaccountid"].ToString();

            //RemittanceRejectionId = Request.QueryString["REMITTANCEREJECTIONID"] == null ? string.Empty : Request.QueryString["REMITTANCEREJECTIONID"].ToString();
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "REJECT");
            //toolbar.AddButton("Approve", "APPROVE");
            MenuRejectionDetails.AccessRights = this.ViewState;
            MenuRejectionDetails.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(RemittanceId))
                {
                    ViewState["REMITTANCEREJECTIONID"] = null;
                    EditRemittanceRejection(new Guid(RemittanceId));

                    ucTitle.Text = "Allotment Remittance Rejection  (" + RemittanceNumber.ToString() + ")     ";

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditRemittanceRejection(Guid remittanceId)
    {
        try
        {
            DataTable dt = PhoenixAccountsAllotmentRemittanceRejection.AllotmentRemittanceRejectionEdit(remittanceId);

            if (dt.Rows.Count > 0)
            {
                txtRejectionRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
                ddlRejectionReason.SelectedQuick = dt.Rows[0]["FLDREASONID"].ToString();
                txtRejectedBy.Text = dt.Rows[0]["FLDREJECTEDBY"].ToString() == "" ? "" : dt.Rows[0]["FLDREJECTEDBY"].ToString() + " On " + dt.Rows[0]["FLDREJECTEDDATE"].ToString();
                ViewState["REMITTANCEREJECTIONID"] = dt.Rows[0]["FLDREMITTANCEREJECTIONID"].ToString();
                txtApprovedby.Text = dt.Rows[0]["FLDAPPROVEDBY"].ToString() == "" ? "" : dt.Rows[0]["FLDAPPROVEDBY"].ToString() + " On " + dt.Rows[0]["FLDAPPROVEDDATE"].ToString();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void MenuRejectionDetails_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("REJECT"))
            {
                //string remittanceid = RemittanceId.ToString();
                string rejectionremarks = txtRejectionRemarks.Text.Trim();
                string reasonid = ddlRejectionReason.SelectedQuick;
                //string activeyn = "1";

                if (!IsValidRemittance(reasonid, rejectionremarks))
                {
                    ucError.Visible = true;
                    return;
                }
                ucConfirm.Visible = true;
                ucConfirm.Text = "Are you sure want to reject the remittance?" + "<br/> Note : Permenant rejection will create a Bank Receipt voucher.";



            }
            if (dce.CommandName.ToUpper().Equals("APPROVE"))
            {
                    //PhoenixAccountsAllotmentRemittanceRejection.AllotmentRemittanceRejectionApprove(
                    //                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    //                    , new Guid(RemittanceId.ToString())
                    //                    , int.Parse(statusid));

                    //ucStatus.Text = "Approved Successfully.";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRemittance(string reasonid, string rejectionremarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int result;

        if (reasonid.Trim().Equals("Dummy"))
        {
            if (int.TryParse(reasonid, out result) == false)
                ucError.ErrorMessage = "Please select rejection reason.";
        }
        if (rejectionremarks.Trim().Equals(""))
        {
            if (int.TryParse(rejectionremarks, out result) == false)
                ucError.ErrorMessage = "Please enter rejection remarks.";
        }
        return (!ucError.IsError);
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            string remittanceid = RemittanceId.ToString();
            string rejectionremarks = txtRejectionRemarks.Text.Trim();
            string reasonid = ddlRejectionReason.SelectedQuick;
            string activeyn = "1";

            string scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'frmRejectionDetails', 'keeppopupopen');");

            UserControlConfirmMessage ucCM = sender as UserControlConfirmMessage;

            if (ucCM.confirmboxvalue == 1)
            {
                if (ViewState["REMITTANCEREJECTIONID"] == null || ViewState["REMITTANCEREJECTIONID"].ToString() == string.Empty)
                {
                    PhoenixAccountsAllotmentRemittanceRejection.AllotmentRemittanceRejectionInsert(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        remittanceid,
                        rejectionremarks.ToString(),
                        int.Parse(reasonid),
                        int.Parse(activeyn),
                        PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                        int.Parse(VesselId),EmployeeBankId
                        );
                }
                else
                {
                    PhoenixAccountsAllotmentRemittanceRejection.AllotmentRemittanceRejectionUpdate(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        ViewState["REMITTANCEREJECTIONID"].ToString(),
                        rejectionremarks.ToString(),
                        int.Parse(reasonid),
                        PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                        );
                }

                ucStatus.Text = "Remittance Rejected.";

                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "javascript:fnReloadList('codehelp1');", true);

                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //                         "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', 'keepopen');", true);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupopen, true);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}