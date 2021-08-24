using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class PreSeaExamVenueFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO");
        MenuPreSeaFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
            txtVenueName.Focus();        
    }

    protected void MenuPreSeaFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("txtVenueName", txtVenueName.Text);
            criteria.Add("txtAddress", txtAddress.Text);
            criteria.Add("ucCountry", ucCountry.SelectedCountry);
            criteria.Add("ucState", ucState.SelectedState);
            criteria.Add("ddlCity", ddlCity.SelectedCity);
            criteria.Add("ddlZone", ddlZone.SelectedZone);
            criteria.Add("txtPhone1", txtPhone1.Text);
            criteria.Add("txtEmail1", txtEmail1.Text);
            criteria.Add("txtContactName", txtContactName.Text);
            criteria.Add("txtContactPhone", txtContactPhone.Text);
            criteria.Add("txtContactMobile", txtContactMobile.Text);
            criteria.Add("txtContactMail", txtContactMail.Text);
            Filter.CurrentPreSeaExamVenueFilter = criteria;
        }
        //Response.Redirect("PreSeaExamVenue.aspx", false);
        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
    protected void ucCountry_TextChanged(object sender, EventArgs e)
    {
        ucState.StateList = PhoenixRegistersState.ListState(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ucCountry.SelectedCountry));
    }
    protected void ddlState_TextChanged(object sender, EventArgs e)
    {
        ddlCity.CityList = PhoenixRegistersCity.ListCity(General.GetNullableInteger(ucCountry.SelectedCountry), General.GetNullableInteger(ucState.SelectedState));
    }
}
