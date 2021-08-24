using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionVesselRAApproval : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Reject", "DISAPPROVE", ToolBarDirection.Right);
        toolbarmain.AddButton("Approve", "APPROVE", ToolBarDirection.Right);
        MenuApprovalRemarks.AccessRights = this.ViewState;
        MenuApprovalRemarks.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            if (Request.QueryString["RATEMPLATEID"] != null && Request.QueryString["RATEMPLATEID"].ToString() != string.Empty)
            {
                ViewState["RATEMPLATEID"] = Request.QueryString["RATEMPLATEID"].ToString();
            }

            if (Request.QueryString["TYPE"] != null && Request.QueryString["TYPE"].ToString() != string.Empty)
            {
                ViewState["TYPE"] = Request.QueryString["TYPE"].ToString();
            }
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
                lblMessage.Text = "Office Remarks is required.";
                return;
            }
            
            if (ViewState["RATEMPLATEID"] != null && ViewState["RATEMPLATEID"] != null)
            {
                if (CommandName.ToUpper().Equals("APPROVE"))
                {
                    PhoenixInspectionRiskAssessment.ApproveRAWithOfficeRemarks(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["RATEMPLATEID"].ToString()), int.Parse(ViewState["TYPE"].ToString()), txtApprovalRemarks.Text);
                }
                else if (CommandName.ToUpper().Equals("DISAPPROVE"))
                {
                    PhoenixInspectionRiskAssessment.DisapproveRAWithOfficeRemarks(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["RATEMPLATEID"].ToString()), int.Parse(ViewState["TYPE"].ToString()), txtApprovalRemarks.Text);                    
                }
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }  
}
