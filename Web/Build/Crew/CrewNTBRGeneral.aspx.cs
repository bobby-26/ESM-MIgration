using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewNTBRGeneral : PhoenixBasePage
{
    private string empid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        empid = Request.QueryString["empid"];
        if (!Page.IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("NTBR", "NTBR");
            toolbarmain.AddButton("DE-NTBR", "DENTBR");

            MenuCrewNTBRGeneral.MenuList = toolbarmain.Show();
            MenuCrewNTBRGeneral.SelectedMenuIndex = 0;

            ifMoreInfo.Attributes["src"] = "CrewNTBR.aspx?empid=" + empid;
        }
    }
    protected void CrewNTBRGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("NTBR"))
        {
            ifMoreInfo.Attributes["src"] = "CrewNTBR.aspx?empid=" + empid;
        }
        if (dce.CommandName.ToUpper().Equals("DENTBR"))
        {
            ifMoreInfo.Attributes["src"] = "CrewDENTBR.aspx?empid=" + empid;
        }
    }
}
