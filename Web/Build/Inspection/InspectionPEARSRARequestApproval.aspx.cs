using System;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionPEARSRARequestApproval : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Submit", "REQUEST", ToolBarDirection.Right);
            MenuReviewRemarks.AccessRights = this.ViewState;
            MenuReviewRemarks.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["RAID"] != null && Request.QueryString["RAID"].ToString() != string.Empty)
                {
                    ViewState["RAID"] = Request.QueryString["RAID"].ToString();
                }

            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
            return;
        }
    }

    protected void MenuReviewRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (General.GetNullableString(txtReviewedRemarks.Text) == null)
            {
                lblMessage.Text = "Review Remarks is required.";
                return;
            }

            if (ViewState["RAID"] != null && ViewState["RAID"] != null)
            {
                if (CommandName.ToUpper().Equals("REQUEST"))
                {
                    PhoenixInspectionPEARSRiskAssessment.RequestRiskAssessment(new Guid(ViewState["RAID"].ToString()), General.GetNullableString(txtReviewedRemarks.Text));
                }

                String script = String.Format("javascript:fnReloadList('Request','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
}