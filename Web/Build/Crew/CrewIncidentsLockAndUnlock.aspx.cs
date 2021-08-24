using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewIncidentsLockAndUnlock : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Ok", "OK",ToolBarDirection.Right);
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
                PhoenixCrewIncidents.LockUnlockIncidents(new Guid(Request.QueryString["id"].ToString())
                                                                                     , txtPassword.Text);
                
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('CI','ifMoreInfo');";
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
