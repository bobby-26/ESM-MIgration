using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersVesselCorrespondenceLockAndUnlock : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Ok", "OK", ToolBarDirection.Right);
        CorrespondenceLock.AccessRights = this.ViewState;
        CorrespondenceLock.MenuList = toolbarmain.Show();
    }

    protected void CorrespondenceLock_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (txtPassword.Text.Trim() == string.Empty) { lblMessage.Text = "* Password is required."; return; }
            string Script = "";

            if (CommandName.ToUpper().Equals("OK"))
            {
                PhoenixRegistersVesselCorrespondence.LockUnLockCorrespondence(
                    Convert.ToInt32(Request.QueryString["id"].ToString())
                    , int.Parse(Request.QueryString["vesselid"])
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
