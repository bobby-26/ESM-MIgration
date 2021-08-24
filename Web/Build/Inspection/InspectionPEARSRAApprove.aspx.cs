using System;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionPEARSRAApprove : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();            
            toolbarmain.AddButton("Reject", "REJECT", ToolBarDirection.Right);
            toolbarmain.AddButton("Approve", "APPROVE", ToolBarDirection.Right);
            MenuApprovalRemarks.AccessRights = this.ViewState;
            MenuApprovalRemarks.MenuList = toolbarmain.Show();

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

    protected void MenuApprovalRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (General.GetNullableString(txtApprovalRemarks.Text) == null)
            {
                lblMessage.Text = " Remarks is required.";
                return;
            }

            if (ViewState["RAID"] != null && ViewState["RAID"].ToString() != string.Empty)
            {
                if (CommandName.ToUpper().Equals("APPROVE"))
                {
                    PhoenixInspectionPEARSRiskAssessment.ApproveRiskAssessment(new Guid(ViewState["RAID"].ToString()), General.GetNullableString(txtApprovalRemarks.Text));
                }
                if (CommandName.ToUpper().Equals("REJECT"))
                {
                    PhoenixInspectionPEARSRiskAssessment.RejectRiskAssessment(new Guid(ViewState["RAID"].ToString()), General.GetNullableString(txtApprovalRemarks.Text));
                }

                String script = String.Format("javascript:fnReloadList('Approve','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
}