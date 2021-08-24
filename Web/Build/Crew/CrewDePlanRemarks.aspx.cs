using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Data;
using Telerik.Web.UI;
public partial class CrewDePlanRemarks : PhoenixBasePage
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
            if (Request.QueryString["planid"] != null && Request.QueryString["planid"].ToString() != string.Empty)
            {
                ViewState["PLANID"] = Request.QueryString["planid"].ToString();
                EditCrewPlan();
            }          
        }
    }
    private void EditCrewPlan()
    {
        DataTable dt = PhoenixCrewPlanning.EditCrewPlan(new Guid(ViewState["PLANID"].ToString()));
        if (dt.Rows.Count > 0)
            txtRemarks.Text = dt.Rows[0]["FLDDEPLANREMARKS"].ToString();
    }
    protected void MenuRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (General.GetNullableString(txtRemarks.Text) == null)
            {
                lblMessage.Text = "De-Plan Remarks is required.";
                return;
            }

            string Script = "";

            if (ViewState["PLANID"] != null && ViewState["PLANID"] != null)
            {
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    PhoenixCrewPlanning.UpdateDePlanRemarks(new Guid(ViewState["PLANID"].ToString()), txtRemarks.Text.Trim());
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('codehelp1','ifMoreInfo',null);";
                    Script += "</script>" + "\n";
                    //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                    RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }  
}
