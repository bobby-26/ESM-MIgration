using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Globalization;
public partial class VesselAccountsReportContract :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                           ViewState["REPORTPAGEURL"] = "../Reports/ReportsViewWithSubReport.aspx";

                DataTable dt = PhoenixVesselAccountsEmployee.CrewContractEmpList(PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(Request.QueryString["EmpId"].ToString()), int.Parse(Request.QueryString["SignonoffId"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    ddlContractid.DataSource = dt;
                    ddlContractid.DataTextField = "FLDPAYDATE";
                    ddlContractid.DataTextFormatString = "{0:" + CultureInfo.CurrentUICulture.DateTimeFormat.ShortDatePattern + "}";
                    ddlContractid.DataValueField ="FLDCONTRACTID";
                    ddlContractid.DataBind();
                }
                ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=4&reportcode=CREWCONTRACT&contractid=" + ddlContractid.SelectedValue + "&reffrom=cl&showmenu=0&accessfrom=2";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuReportHkVsl_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=4&reportcode=CREWCONTRACT&contractid=" + ddlContractid.SelectedValue + "&reffrom=cl&showmenu=0&accessfrom=2";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlContractid_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=4&reportcode=CREWCONTRACT&contractid=" + ddlContractid.SelectedValue + "&reffrom=cl&showmenu=0&accessfrom=2";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
