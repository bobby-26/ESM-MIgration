using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewLandTravelTariff : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["REQUESTID"] = "";
            ViewState["ADDRESSCODE"] = "";

            if (Request.QueryString["requestid"] != null)
                ViewState["REQUESTID"] = Request.QueryString["requestid"].ToString();

            if (ViewState["REQUESTID"] != null && ViewState["REQUESTID"].ToString() != "")
                EditLandTravelRequest(new Guid(ViewState["REQUESTID"].ToString()));

        }
        MainMenu();
    }

    private void EditLandTravelRequest(Guid requestid)
    {
        DataTable dt = PhoenixCrewLandTravelRequest.EditLandTravelRequest(requestid);

        if (dt.Rows.Count > 0)
        {
            ViewState["ADDRESSCODE"] = dt.Rows[0]["FLDADDRESSCODE"].ToString();
        }
    }

    private void MainMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Land Travel Request", "LANDTRAVELREQUEST");
        toolbar.AddButton("Request Details", "REQUESTDETAILS");
        if (ViewState["REQUESTID"] != null && ViewState["REQUESTID"].ToString() != "")
            toolbar.AddButton("Tariff", "TARIFF");
        MenuLandTravelRequest.AccessRights = this.ViewState;
        MenuLandTravelRequest.MenuList = toolbar.Show();
        MenuLandTravelRequest.SelectedMenuIndex = 2;
    }

    protected void MenuLandTravelRequest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? requestid = General.GetNullableGuid(ViewState["REQUESTID"] != null ? ViewState["REQUESTID"].ToString().ToString() : null);

            if (CommandName.ToUpper().Equals("LANDTRAVELREQUEST"))
            {
                Response.Redirect("CrewLandTravelRequest.aspx", false);
            }
            if (CommandName.ToUpper().Equals("REQUESTDETAILS"))
            {
                Response.Redirect("CrewLandTravelRequestGeneral.aspx?requestid=" + ViewState["REQUESTID"].ToString(), false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindTariff()
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixRegistersLandTravelTariff.ListLandTravelTariffPackageType(Int64.Parse(ViewState["ADDRESSCODE"].ToString() != "" ? ViewState["ADDRESSCODE"].ToString() : "0"), General.GetNullableDateTime(""));

            //Meter Tariff

            gvMeterTariff.DataSource = ds.Tables[0];
            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCityHourTariff()
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixRegistersLandTravelTariff.ListLandTravelTariffPackageType(Int64.Parse(ViewState["ADDRESSCODE"].ToString() != "" ? ViewState["ADDRESSCODE"].ToString() : "0"), General.GetNullableDateTime(""));
            
            //City Hourly Package
            gvCityHourlyPackage.DataSource = ds.Tables[1];

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindOutofCityDropTariff()
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixRegistersLandTravelTariff.ListLandTravelTariffPackageType(Int64.Parse(ViewState["ADDRESSCODE"].ToString() != "" ? ViewState["ADDRESSCODE"].ToString() : "0"), General.GetNullableDateTime(""));
            
            //Out of City (Only Drop)
            gvOutofCityOnlyDrop.DataSource = ds.Tables[2];
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindOutofCityUpDownTariff()
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixRegistersLandTravelTariff.ListLandTravelTariffPackageType(Int64.Parse(ViewState["ADDRESSCODE"].ToString() != "" ? ViewState["ADDRESSCODE"].ToString() : "0"), General.GetNullableDateTime(""));

            //Out of City (Up & Down)
            gvOutofCityUpandDown.DataSource = ds.Tables[3];

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvMeterTariff_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindTariff();
    }

    protected void gvMeterTariff_ItemDataBound1(object sender, GridItemEventArgs e)
    {

    }

    protected void gvMeterTariff_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvCityHourlyPackage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindCityHourTariff();
    }

    protected void gvCityHourlyPackage_ItemDataBound1(object sender, GridItemEventArgs e)
    {

    }

    protected void gvCityHourlyPackage_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvOutofCityOnlyDrop_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindOutofCityDropTariff();
    }

    protected void gvOutofCityOnlyDrop_ItemDataBound1(object sender, GridItemEventArgs e)
    {

    }

    protected void gvOutofCityOnlyDrop_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvOutofCityUpandDown_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindOutofCityUpDownTariff();
    }

    protected void gvOutofCityUpandDown_ItemDataBound1(object sender, GridItemEventArgs e)
    {

    }

    protected void gvOutofCityUpandDown_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }
}
