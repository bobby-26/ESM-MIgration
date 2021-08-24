using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class InspectionCopyJHARA : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuOfficeToCopy.AccessRights = this.ViewState;
        MenuOfficeToCopy.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            if (Request.QueryString["JOBHAZARDID"] != null && Request.QueryString["JOBHAZARDID"].ToString() != string.Empty)
            {
                ViewState["JOBHAZARDID"] = Request.QueryString["JOBHAZARDID"].ToString();
            }

            if (Request.QueryString["RAPROCESSID"] != null && Request.QueryString["RAPROCESSID"].ToString() != string.Empty)
            {
                ViewState["RAPROCESSID"] = Request.QueryString["RAPROCESSID"].ToString();
                
            }

            if (Request.QueryString["TYPE"] != null && Request.QueryString["TYPE"].ToString() != string.Empty)
            {
                ViewState["TYPE"] = Request.QueryString["TYPE"].ToString();
                
            }
        }
    }

    protected void MenuOfficeToCopy_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (General.GetNullableString(ucCompanySelect.SelectedCompany) == null)
            {
                lblMessage.Text = "Company is required.";
                return;
            }
            string Script = "";
            if (ViewState["JOBHAZARDID"] != null && ViewState["TYPE"].ToString() == "JHA")
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixInspectionRiskAssessmentJobHazard.RiskAssessmentJobHazardCopyByCompany(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                       , new Guid(ViewState["JOBHAZARDID"].ToString()), General.GetNullableInteger(ucCompanySelect.SelectedCompany));
                }
            }
            else if (ViewState["RAPROCESSID"] != null && ViewState["TYPE"].ToString() == "RAPROCESS")
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixInspectionRiskAssessmentProcess.CopyRiskAssessmentProcessByCompany(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["RAPROCESSID"].ToString()), 0, General.GetNullableInteger(ucCompanySelect.SelectedCompany));
                }
            }
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
}
