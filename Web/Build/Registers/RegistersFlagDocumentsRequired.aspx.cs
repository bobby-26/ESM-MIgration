using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;

public partial class RegistersFlagDocumentsRequired : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Flag", "FLAG");
            toolbar.AddButton("Documents", "DOCUMENTSREQUIRED");
            MenuFlag.AccessRights = this.ViewState;
            MenuFlag.MenuList = toolbar.Show();
          
            Filter.CurrentVesselMasterFilter = null;

            MenuFlag.SelectedMenuIndex = 1;

            ifMoreInfo.Attributes["src"] = "../Registers/RegistersDocumentRequiredCourse.aspx?launchedfrom=FLAG";
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void MenuFlag_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        UserControlTabs ucTabs = (UserControlTabs)sender;

        if (dce.CommandName.ToUpper().Equals("FLAG"))
        {
            Response.Redirect("../Registers/RegistersFlag.aspx");
        }
    }
}
