using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewCorrespondenceLockAndUnlock : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
		SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Ok", "OK",ToolBarDirection.Right);
		CrewLock.AccessRights = this.ViewState;
        CrewLock.MenuList = toolbarmain.Show();
    }
    protected void CrewLock_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (txtPassword.Text.Trim() == string.Empty) { lblMessage.Text = "* Password is required."; return; }
            string Script = "";
            if (CommandName.ToUpper().Equals("OK"))
            {
                PhoenixCrewCorrespondence.LockUnLockCorrespondence(Convert.ToInt32(Request.QueryString["id"].ToString()),int.Parse(Request.QueryString["empid"])
                                                                                     , txtPassword.Text);
                
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('CI','ifMoreInfo');";
                Script += "</script>" + "\n";

				Session["corres"] = "1";//to differentiate between lock and correspondence filter
                
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
}
