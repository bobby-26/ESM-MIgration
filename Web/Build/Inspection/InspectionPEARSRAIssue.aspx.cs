using System;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionPEARSRAIssue : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Issue", "ISSUE", ToolBarDirection.Right);
            MenuIssueRemarks.AccessRights = this.ViewState;
            MenuIssueRemarks.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["RAID"] != null && Request.QueryString["RAID"].ToString() != string.Empty)
                {
                    ViewState["RAID"] = Request.QueryString["RAID"].ToString().ToUpper();
                }

            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
            return;
        }
    }

    protected void MenuIssueRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (General.GetNullableString(txtCompleteRemarks.Text) == null)
            {
                lblMessage.Text = " Remarks is required.";
                return;
            }

            if (ViewState["RAID"] != null && ViewState["RAID"].ToString() != string.Empty)
            {
                if (CommandName.ToUpper().Equals("ISSUE"))
                {
                    PhoenixInspectionPEARSRiskAssessment.CompleteRiskAssessment(new Guid(ViewState["RAID"].ToString()), General.GetNullableString(txtCompleteRemarks.Text));
                }

                String script = String.Format("javascript:fnReloadList('Issue','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
}