using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class RegisterProsperCardstatusAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if(!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                MenuCardstatus.AccessRights = this.ViewState;
                MenuCardstatus.MenuList = toolbar.Show();
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuCardstatus_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            string scriptClosePopup = "";
            scriptClosePopup += "<script language='javaScript' id='Cardstatus'>" + "\n";
            scriptClosePopup += "fnReloadList('Cardstatus');";
            scriptClosePopup += "</script>" + "\n";

            //string scriptRefreshDontClose = "";
            //scriptRefreshDontClose += "<script language='javaScript' id='CardStatusNew'>" + "\n";
            //scriptRefreshDontClose += "fnReloadList('CardstatusName', null, 'yes');";
            //scriptRefreshDontClose += "</script>" + "\n";

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                
                if (!IsValidCardstatus())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegisterProsperCardstatus.InsertProsperCardstatus(
                        txtCode.Text.Trim()
                        , txtName.Text.Trim()
                        );
                ucStatus.Text = "Information Added";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "CardstatusNew", scriptClosePopup);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidCardstatus()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtCode.Text.ToString()) == null)
            ucError.ErrorMessage = "Code is required";

        if (General.GetNullableString(txtName.Text) == null)
            ucError.ErrorMessage = "Cardstatus Name is required";

        return (!ucError.IsError);
    }
}