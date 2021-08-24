using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersVesselSupplierFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
            MenuOfficeFilterMain.AccessRights = this.ViewState;
            MenuOfficeFilterMain.MenuList = toolbar.Show();
            txtName.Focus();          
        }
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        string Script = "";
        Script += "<script language='JavaScript' id='BookMarkScript'>";
        Script += "fnReloadList();";
        Script += "</script>";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtCode", txtcode.Text);
            criteria.Add("txtName", txtName.Text);
            criteria.Add("ddlCountry", ddlCountry.SelectedCountry);
            criteria.Add("ddlState", ddlState.SelectedState);
            criteria.Add("ddlCity", ddlCity.SelectedCity);
            
            Filter.CurrentAddressFilterCriteria = criteria;
        }
        
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);

    }
    protected void ddlCountry_TextChanged(object sender, EventArgs e)
    {
        ddlState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ddlCountry.SelectedCountry));
        ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ddlCountry.SelectedCountry), null);
    }
    protected void ddlState_TextChanged(object sender, EventArgs e)
    {
        ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ddlCountry.SelectedCountry), General.GetNullableInteger(ddlState.SelectedState));
    }
}
