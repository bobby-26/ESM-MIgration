using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class DashboardSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //txtCrewName.Attributes.Add("onkeyup", "setTimeOut('_doPostBack(\'" + txtCrewName.ClientID.Replace("_", "$") + "\',\'\')', 0);");
            SessionUtil.PageAccessRights(this.ViewState);
            ifMoreInfo.Attributes.Add("height", Session["screenheight"].ToString());

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Close", "CLOSE", ToolBarDirection.Right);
            MenuOptionChooseVessel.AccessRights = this.ViewState;
            MenuOptionChooseVessel.MenuList = toolbarmain.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void OptionChooseVessel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                string Script = "";
                Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
                Script += "showVessel();";
                Script += "</script>" + "\n";

                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }
    //public void RaisePostBackEvent(string Arg)
    //{
    //    if (txtCrewName.ID == Arg)
    //        txtSearch_TextChanged(txtCrewName, null);
    //}

    protected void txtSearch_TextChanged(object sender, EventArgs e)
    {
        ifMoreInfo.Attributes["src"] = "../Dashboard/DashboardCrewListSearch.aspx?qSearch=" + txtCrewName.Text + "&Fileno=" + txtFileNo.Text;
    }
    protected void txtSearch1_TextChanged(object sender, EventArgs e)
    {
        ifMoreInfo.Attributes["src"] = "../Dashboard/DashboardCrewListSearch.aspx?qSearch=" + txtCrewName.Text + "&Fileno=" + txtFileNo.Text;
    }

    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        ifMoreInfo.Attributes["src"] = "../Dashboard/DashboardCrewListSearch.aspx?qSearch=" + txtCrewName.Text + "&Fileno=" + txtFileNo.Text;

    }
}
