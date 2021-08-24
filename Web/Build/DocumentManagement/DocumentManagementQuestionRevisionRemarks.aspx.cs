using System;
using System.Web.UI;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.DocumentManagement;

public partial class DocumentManagementQuestionRevisionRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuApprovalRemarks.AccessRights = this.ViewState;
        MenuApprovalRemarks.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["SECTIONID"] = "";
            ViewState["REVISIONID"] = "";
            ViewState["PUBLISHEDYN"] = 0;

            if (Request.QueryString["SECTION"] != null && Request.QueryString["SECTION"].ToString() != string.Empty)
                ViewState["SECTION"] = Request.QueryString["SECTION"].ToString();

            if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != string.Empty)
                ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();

            if (Request.QueryString["REVISIONID"] != null && Request.QueryString["REVISIONID"].ToString() != string.Empty)
            {
                ViewState["REVISIONID"] = Request.QueryString["REVISIONID"].ToString();
            }
            else
            {
                ViewState["PUBLISHEDYN"] = 1;
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
                lblMessage.Text = "Verification Remarks is required.";
                return;
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["PUBLISHEDYN"].ToString() == "0")
                {
                    PhoenixDocumentManagementQuestion.DMSQuestionRevisonRemarks(General.GetNullableGuid(ViewState["SECTIONID"].ToString())
                    , General.GetNullableGuid(ViewState["REVISIONID"].ToString())
                    , General.GetNullableString(txtApprovalRemarks.Text));

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('QuestionEdit', 'Questions');", true);
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
}
