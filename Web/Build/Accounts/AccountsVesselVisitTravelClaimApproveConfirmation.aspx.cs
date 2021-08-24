using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsVesselVisitTravelClaimApproveConfirmation : PhoenixBasePage
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
            PhoenixAccountsVesselVisitITSuperintendentRegister.VisitApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["VisitId"].ToString()));
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
            DataSet ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimTravelAdvanceAmountList(new Guid(ViewState["VisitId"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                Response.Write("<table width='100%'>");
                Response.Write("<tr>");
                Response.Write("<td valign='top'>");
                Response.Write("<font size='2'><b>Confirmation</b></font>");
                Response.Write("</td>");
                Response.Write("</tr>");
                Response.Write("</table>");
                Response.Write("<table width='50%'>");
                Response.Write("<tr>");
                Response.Write("<td >");
                Response.Write("<font size = '2' color='#0000CC'><b>Travel advance requested by "+dr["FLDNAME"].ToString()+"</b></font><br />");
                Response.Write("</td>");
                Response.Write("</tr>");
                Response.Write("</table>");
                Response.Write("<table  width='100%' border='1px solid #ddd'>");
                Response.Write("<tr border='1px solid #ddd'><th background-color='#4CAF50'>Currency</th><th>Amount</th></tr>");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Response.Write("<tr border='1px solid #ddd'>");
                    Response.Write("<td align='center'><font size='2'> " + ds.Tables[0].Rows[i]["FLDCURRENCYCODE"].ToString() + "</font> </td>");
                    Response.Write("<td align='center'> <font size='2'>" + ds.Tables[0].Rows[i]["FLDREQUESTAMOUNT"].ToString() + "</font> </td>");
                    Response.Write("</tr>");
                }
                Response.Write("</table>");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
