using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;

public partial class AccountsVesselVisitTravelClaimMDApproveConfirmation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["VisitId"] = Request.QueryString["visitId"];
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdApprove_Click(object sender, EventArgs e)
    {
        try
        {

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
                                                                                                            , new Guid(ViewState["VisitId"].ToString()));
            }
            ucError.Text = "Travel claim approved.";
            string script = "javascript:fnReloadList('codehelp1');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void cmdNotApprove_Click(object sender, EventArgs e)
    {
        try
        {
            //String scriptpopup = String.Format(
            //    "javascript:parent.Openpopup('att', '', '../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["VISITDTKEY"].ToString() + "&mod=ACCOUNTS&type=&cmdname=&VESSELID=" + ViewState["VisitId"].ToString() + "');");

            //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            string script = "javascript:fnReloadList('codehelp1');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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
                ViewState["VISITDTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    Response.Write("<table>");
                    Response.Write("<tr>");
                    Response.Write("<td valign='top' colspan='2' >");
                    Response.Write("<font size='2' color= 'red'><b>Current claim exceeds the claim limit of 100 " + ds.Tables[0].Rows[0]["FLDCLAIMCURRENCYCODE"].ToString() + " </br> Do you want to proceed?</b></font>"); //. Do you want to proceed?</b></font>");

                    Response.Write("</td>");
                    Response.Write("</tr>");
                    Response.Write("</table>");
                    Response.Write("<table width='30%'>");
                    Response.Write("</table>");
                    Response.Write("<table  width='30%' border='1px solid #ddd'>");
                    Response.Write("<tr border='1px solid #ddd'><th background-color='#4CAF50'>Currency</th><th>Amount</th></tr>");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Response.Write("<tr border='1px solid #ddd'>");
                        Response.Write("<td align='center'><font size='2'> " + ds.Tables[0].Rows[i]["FLDCLAIMCURRENCYCODE"].ToString() + "</font> </td>");
                        Response.Write("<td align='center'> <font size='2'>" + ds.Tables[0].Rows[i]["FLDCLAIMAMOUNT"].ToString() + "</font> </td>");
                        Response.Write("</tr>");
                    }
                    Response.Write("</table>");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
