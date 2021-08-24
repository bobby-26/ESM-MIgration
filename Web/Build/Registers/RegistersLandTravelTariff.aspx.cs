using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersLandTravelTariff : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarsubtap = new PhoenixToolbar();
            toolbarsubtap.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarsubtap.AddButton("New", "NEW", ToolBarDirection.Right);
            MenuNewSaveTabStrip.AccessRights = this.ViewState;
            MenuNewSaveTabStrip.MenuList = toolbarsubtap.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["TARIFFID"] = "";

                DataTable dt = PhoenixRegistersVehicleType.ListVehicleType();
                ddlVehicleType.DataSource = dt;
                ddlVehicleType.DataBind();
                ddlVehicleType.Items.Insert(0, "--Select--");

                ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx', true); ");
                txtVendorId.Attributes.Add("style", "display:none");

                gvTariff.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["TARIFFID"].ToString() == "")
                {
                    PhoenixRegistersLandTravelTariff.InsertLandTravelTariff(int.Parse(ddlType.SelectedValue), Int64.Parse(txtVendorId.Text), General.GetNullableDateTime(""), General.GetNullableInteger(ddlVehicleType.SelectedValue)
                        , General.GetNullableInteger(txtHours.Text), General.GetNullableInteger(txtKms.Text), General.GetNullableInteger(ddlCurrency.SelectedCurrency)
                        , General.GetNullableDecimal(txtAmount.Text), General.GetNullableDecimal(txtAddlchargesperkm.Text), General.GetNullableDecimal(txtWtgchargesforonemin.Text)
                        , General.GetNullableString(txtReason.Text));
                }
                else
                {
                    PhoenixRegistersLandTravelTariff.UpdateLandTravelTariff(int.Parse(ViewState["TARIFFID"].ToString())
                        , int.Parse(ddlType.SelectedValue), Int64.Parse(txtVendorId.Text), General.GetNullableDateTime(""), General.GetNullableInteger(ddlVehicleType.SelectedValue)
                        , General.GetNullableInteger(txtHours.Text), General.GetNullableInteger(txtKms.Text), General.GetNullableInteger(ddlCurrency.SelectedCurrency)
                        , General.GetNullableDecimal(txtAmount.Text), General.GetNullableDecimal(txtAddlchargesperkm.Text), General.GetNullableDecimal(txtWtgchargesforonemin.Text)
                        , General.GetNullableString(txtReason.Text));

                }
                BindData();
                gvTariff.Rebind();
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                Reset();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void Reset()
    {
        ViewState["TARIFFID"] = "";
        ddlType.Text = "";
        ddlType.SelectedValue = "";
        txtVendorCode.Text = "";
        txtVenderName.Text = "";
        txtVendorId.Text = "";
        ddlVehicleType.Text = "";
        ddlVehicleType.SelectedValue = "";
        txtHours.Text = "";
        txtKms.Text = "";
        ddlCurrency.SelectedCurrency = "";
        txtAmount.Text = "";
        txtAddlchargesperkm.Text = "";
        txtWtgchargesforonemin.Text = "";
        txtReason.Text = "";

    }

    private bool IsValidData()
    {
        if (General.GetNullableInteger(ddlType.SelectedValue) == null)
            ucError.ErrorMessage = "Type is Required.";

        if (General.GetNullableInteger(txtVendorId.Text) == null)
            ucError.ErrorMessage = "Agent is Required.";

        if (General.GetNullableInteger(ddlCurrency.SelectedCurrency) == null)
            ucError.ErrorMessage = "Currency is Required.";

        if (General.GetNullableInteger(ddlVehicleType.SelectedValue) == null)
            ucError.ErrorMessage = "Vehicle Type is Required.";

        return (!ucError.IsError);
    }

    private void EditData(int TariffId)
    {
        DataTable dt = PhoenixRegistersLandTravelTariff.EditLandTravelTariff(TariffId);
        DataRow dr = dt.Rows[0];
        ddlType.Text = "";
        ddlType.SelectedValue = "";

        ddlType.SelectedValue = dr["FLDPACKAGETYPE"].ToString();
        txtVendorCode.Text = dr["FLDAGENTCODE"].ToString();
        txtVenderName.Text = dr["FLDAGENTNAME"].ToString();
        txtVendorId.Text = dr["FLDADDRESSCODE"].ToString();

        ddlVehicleType.Text = "";
        ddlVehicleType.SelectedValue = "";

        ddlVehicleType.SelectedValue = dr["FLDVEHICLETYPE"].ToString();
        txtHours.Text = dr["FLDHOURS"].ToString();
        txtKms.Text = dr["FLDKMS"].ToString();

        ddlCurrency.SelectedCurrency = "";
       
        ddlCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
        txtAmount.Text = dr["FLDAMOUNT"].ToString();
        txtAddlchargesperkm.Text = dr["FLDADDLCHARGESPERKM"].ToString();
        txtWtgchargesforonemin.Text = dr["FLDWTGCHARGESFORONEMIN"].ToString();
        txtReason.Text = dr["FLDREASON"].ToString();
    }



    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSHORTNAME", "FLDCARGONAME", "FLDCARGOTYPENAME", "FLDTYPEDESCRIPTION" };
        string[] alCaptions = { "Shortname", "Name", "Cargo Type", "Vessel Type" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersLandTravelTariff.SearchLandTravelTariff(
            null,
            null,
            null,
            sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvTariff.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvTariff", "Tariff", alCaptions, alColumns, ds);

        gvTariff.DataSource = ds;
        gvTariff.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;


    }
    
    protected void gvTariff_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTariff.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvTariff_ItemCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["TARIFFID"] = ((RadLabel)e.Item.FindControl("lblTariffId")).Text;

                EditData(int.Parse(ViewState["TARIFFID"].ToString()));
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTariff_ItemDataBound1(object sender, GridItemEventArgs e)
    {

    }
}
