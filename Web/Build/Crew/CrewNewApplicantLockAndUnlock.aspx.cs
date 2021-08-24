using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewNewApplicantLockAndUnlock : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Ok", "OK");
        CrewLock.AccessRights = this.ViewState;
        CrewLock.MenuList = toolbarmain.Show();
    }
    protected void CrewLock_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (txtPassword.Text.Trim() == string.Empty) { lblMessage.Text = "* Password is required."; return; }
            string Script = "";
            if (dce.CommandName.ToUpper().Equals("OK"))
            {
                PhoenixNewApplicantInterviewSummary.LockNewApplicantInterviewSummary(Convert.ToInt32(Request.QueryString["EMPID"].ToString())
                                                                                     , Convert.ToInt32(Request.QueryString["INTERVIEWID"].ToString())
                                                                                     ,txtPassword.Text);
                
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
				Session["corres"] = "1";//to differentiate between lock and correspondence filter
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }
}
