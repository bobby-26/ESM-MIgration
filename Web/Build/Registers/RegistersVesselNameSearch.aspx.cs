using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersVesselNameSearch : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();       
      
        toolbar.AddButton("Vessel Search", "SEARCH", ToolBarDirection.Right);
        toolbar.AddButton("List", "LIST", ToolBarDirection.Right);

        MenuRegisters.AccessRights = this.ViewState;
        MenuRegisters.MenuList = toolbar.Show();
        MenuRegisters.SelectedMenuIndex = 0;

        PhoenixToolbar toolbar1 = new PhoenixToolbar();

        toolbar1.AddFontAwesomeButton("../Registers/RegistersVesselNameSearch.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar1.AddFontAwesomeButton("../Registers/RegistersVesselNameSearch.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        MenuRegistersNameSearch.AccessRights = this.ViewState;
        MenuRegistersNameSearch.MenuList = toolbar1.Show();

        if (!IsPostBack)
        {
        }
        BindData();
    }

    protected void RegistersNameSearch_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
         string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            gvVesselNameSearch.Rebind();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            txtSearch.Text = "";
            BindData();
            gvVesselNameSearch.Rebind();
        }
  
      
    }

    private void BindData()
    {

        DataSet ds = PhoenixRegistersVessel.VesselNameSearch(txtSearch.Text);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvVesselNameSearch.DataSource = ds;
            gvVesselNameSearch.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
        }

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void MenuRegisters_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        UserControlTabsTelerik ucTabs = (UserControlTabsTelerik)sender;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Registers/RegistersVesselNameSearch.aspx");
        }
        else
        if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../Registers/RegistersVesselMaster.aspx");
        }
    }

    protected void gvVesselNameSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
