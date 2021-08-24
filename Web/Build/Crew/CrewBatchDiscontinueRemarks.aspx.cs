using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewBatchDiscontinueRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("SAVE", "SAVE",ToolBarDirection.Right);
            MenuRemarks.AccessRights = this.ViewState;
            MenuRemarks.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            if (Request.QueryString["enrollmentId"] != null && Request.QueryString["enrollmentId"].ToString() != string.Empty)
            {
                ViewState["ENROLLMENTID"] = Request.QueryString["enrollmentId"].ToString();
                EditRemark();
            }
        }
    }

    private void EditRemark()
    {
        DataSet ds = PhoenixCrewBatchEnrollment.CrewBatchEnrollmentEdit(General.GetNullableGuid(ViewState["ENROLLMENTID"].ToString()).Value);
        if (ds.Tables[0].Rows.Count > 0)
            txtRemarks.Text = ds.Tables[0].Rows[0]["FLDREMARKS"].ToString();
    }
    protected void MenuRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (General.GetNullableString(txtRemarks.Text) == null)
            {
                lblMessage.Text = "Remarks is required.";
                return;
            }
            if (ViewState["ENROLLMENTID"] != null && ViewState["ENROLLMENTID"] != null)
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixCrewBatchEnrollment.UpdateDiscontinueRemarks(new Guid(ViewState["ENROLLMENTID"].ToString()), txtRemarks.Text.Trim());                  
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1','',true);", true);
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
}