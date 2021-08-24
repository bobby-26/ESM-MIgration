using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;

public partial class Options_OptionsBroadcast : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
       
        toolbar.AddButton("Send", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        lblUserCode.Attributes.Add("style", "visibility:hidden");
        MenuPhoenixBroadcast.MenuList = toolbar.Show();
        cmdShowUsers.Attributes.Add("onclick", "return showPickList('spnPickListUsers', 'codehelp1', '', '../Common/CommonUserList.aspx', true); ");
    }

    protected void PhoenixBroadcast_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            
            PhoenixCommonBroadcast.Send(General.GetNullableInteger(lblUserCode.Text), txtSubject.Text, txtMessage.Text);
            Reset();
        }
        if (CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
        }
    }

    private void Reset()
    {
        txtMessage.Text = "";
        txtSubject.Text = "";
    }
}
