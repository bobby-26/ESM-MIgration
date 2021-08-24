using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;

public partial class AccountsVesselVisitTravelClaimSubmitForApprovalConfirmation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["VisitId"] = Request.QueryString["visitId"];
                ViewState["TravelClaimId"] = Request.QueryString["TravelClaimId"];
                decimal iAmount = 0;
                DataSet ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimTotalAdvance(new Guid(ViewState["VisitId"].ToString()), ref iAmount);
                lblamount.Text = "$" +iAmount.ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdYes_Click(object sender, EventArgs e)
    {
        try
        {
            Session["OPTION"] = "Yes";

            DataSet ds = new DataSet();
            ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimSubmitForApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                ,new Guid(ViewState["VisitId"].ToString())
                                                                                ,new Guid(ViewState["TravelClaimId"].ToString()));
             
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
                //ucStatus.Text = "Travel claim approved.";
            }
            else
            {
                //ucStatus.Text = "Travel claim submitted for approval";
            }
            string script = "javascript:fnReloadList('codehelp1');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdNo_Click(object sender, EventArgs e)
    {
        try
        {
            Session["OPTION"] = "No";

            string script = "javascript:fnReloadList('codehelp1');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
