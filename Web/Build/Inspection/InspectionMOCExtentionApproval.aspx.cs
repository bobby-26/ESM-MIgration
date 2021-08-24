using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Data.SqlClient;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionMOCExtentionApproval : PhoenixBasePage
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
            ViewState["MOCEXTENTIONID"] = "";
            if (Request.QueryString["MOCEXTENTIONID"] != null && Request.QueryString["MOCEXTENTIONID"].ToString() != string.Empty)
            {
                ViewState["MOCEXTENTIONID"] = Request.QueryString["MOCEXTENTIONID"].ToString();
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

            if (ViewState["MOCEXTENTIONID"] != null && ViewState["MOCEXTENTIONID"] != null)
            {
                if (CommandName.ToUpper().Equals("APPROVE"))
                {
                    PhoenixInspectionMOCExtention.MOCExtentionApprove(General.GetNullableGuid(ViewState["MOCEXTENTIONID"].ToString()), txtApprovalRemarks.Text);
                }
                else if (CommandName.ToUpper().Equals("DISAPPROVE"))
                {
                    PhoenixInspectionMOCExtention.MOCExtentionReject(General.GetNullableGuid(ViewState["MOCEXTENTIONID"].ToString()), txtApprovalRemarks.Text);
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