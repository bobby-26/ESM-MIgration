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

public partial class InspectionMOCApprovalExtn : PhoenixBasePage
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
            if (Request.QueryString["MOCID"] != null && Request.QueryString["MOCID"].ToString() != string.Empty)
            {
                ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();
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
                lblMessage.Text = "Remarks is required.";
                return;
            }

            string Script = "";

            if (ViewState["MOCID"] != null && ViewState["MOCID"] != null)
            {
                if (CommandName.ToUpper().Equals("APPROVE"))
                {
                    PhoenixInspectionMOCApprovalForChange.MOCApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , new Guid((ViewState["MOCID"]).ToString())
                                                             , txtApprovalRemarks.Text);
                    
                }
                else if (CommandName.ToUpper().Equals("DISAPPROVE"))
                {
                    PhoenixInspectionMOCApprovalForChange.MOCApprovalReject(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , new Guid((ViewState["MOCID"]).ToString())
                                                             , txtApprovalRemarks.Text);
                }
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
}
