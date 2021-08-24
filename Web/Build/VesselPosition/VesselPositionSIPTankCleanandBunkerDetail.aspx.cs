using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionSIPTankCleanandBunkerDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbabutton = new PhoenixToolbar();
        toolbabutton.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuRiskassessmentplan.AccessRights = this.ViewState;
        MenuRiskassessmentplan.MenuList = toolbabutton.Show();

        if (!IsPostBack)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                UcVessel.Enabled = false;
            }
            else
            {
                UcVessel.SelectedVessel = Request.QueryString["Vesselid"];
            }
            UcVessel.DataBind();
            UcVessel.bind();

            ViewState["SIPTANKCLEANINGANDBUNKERID"] = Request.QueryString["siptankcleaningandbunkerid"];
            ViewState["TANKID"] = Request.QueryString["tankid"];

            if(Request.QueryString["Type"] == "1")
            {
                trbunkerHead.Visible = false;
                trBunkerDate.Visible = false;
                trBunkerArea.Visible = false;
            }

            ViewState["tanktype"] = Request.QueryString["Type"];
            ViewState["Regionid"] = "";

            ViewState["SupplyRegionid"] = "";
            ViewState["BunkerRegionid"] = "";

            ViewState["SupplyCountryid"] = "";
            ViewState["BunkerCountryid"] = "";

            

            BindMethod();
            BindOption();
            BindRegion();
            BindSupplyRegion();
            BindBunkerRegion();
            BindData();
            bindOfficeInstruction();
        }

    }
    private void bindOfficeInstruction()
    {
        DataSet ds = PhoenixVesselPositionSIPConfiguration.EditSIPfficeinstruction();
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtOfficeComment.Text = ds.Tables[0].Rows[0]["FLDTANKCLEANCOMMENT"].ToString();
        }
    }
    private void BindMethod()
    {
        DataSet ds = PhoenixRegistersEUMRVDeterminationofdestination.ListEVMRVDeterminationCategory(9);
        ddlMethod.DataSource = ds;
        ddlMethod.DataTextField = "FLDNAME";
        ddlMethod.DataValueField = "FLDEUMRVCATEGORIESID";
        ddlMethod.DataBind();
            //ddlMethod.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    private void BindOption()
    {
        DataSet ds = PhoenixRegistersEUMRVDeterminationofdestination.ListEVMRVDeterminationCategory(10);
        ddlOption.DataSource = ds;
        ddlOption.DataTextField = "FLDNAME";
        ddlOption.DataValueField = "FLDEUMRVCATEGORIESID";
        ddlOption.DataBind();
        ddlOption.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void BindRegion()
    {
        DataTable dt = PhoenixRegistersRegion.RegionList();
        ddlregion.DataSource = dt;
        ddlregion.DataTextField = "FLDREGIONNAME";
        ddlregion.DataValueField = "FLDREGIONID";
        ddlregion.DataBind();
        ddlregion.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void BindSupplyRegion()
    {
        DataTable dt = PhoenixRegistersRegion.RegionList();
        ddlSupplyregion.DataSource = dt;
        ddlSupplyregion.DataTextField = "FLDREGIONNAME";
        ddlSupplyregion.DataValueField = "FLDREGIONID";
        ddlSupplyregion.DataBind();
        ddlSupplyregion.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void BindBunkerRegion()
    {
        DataTable dt = PhoenixRegistersRegion.RegionList();
        ddlBunkerRegion.DataSource = dt;
        ddlBunkerRegion.DataTextField = "FLDREGIONNAME";
        ddlBunkerRegion.DataValueField = "FLDREGIONID";
        ddlBunkerRegion.DataBind();
        ddlBunkerRegion.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void BindData()
    {
        try
        {
            if (General.GetNullableGuid(ViewState["SIPTANKCLEANINGANDBUNKERID"].ToString()) != null)
            {
                DataSet ds = PhoenixVesselPositionSIPTankCleaning.SIPTankCleanEdit(General.GetNullableGuid(ViewState["SIPTANKCLEANINGANDBUNKERID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];


                    ViewState["Regionid"] = dr["FLDDOCREGION"].ToString();

                    ViewState["SupplyRegionid"] = dr["FLDSUPPLYREGION"].ToString();
                    ViewState["BunkerRegionid"] = dr["FLDBUNKERREGION"].ToString();

                    ViewState["SupplyCountryid"] = dr["FLDSUPPLYCOUNTRY"].ToString();
                    ViewState["BunkerCountryid"] = dr["FLDBUNKERCOUNTRY"].ToString();

                    BindCountry();
                    BindSupplyCountry();
                    BindBunkerCountry();
                    BindSupplyPort();
                    BindBunkerPort();

                    ddlMethod.SelectedValue = General.GetNullableInteger(dr["FLDMETHODID"].ToString()) == null ? "Dummy" : dr["FLDMETHODID"].ToString(); 
                    ddlOption.SelectedValue = General.GetNullableInteger(dr["FLDCLEANOPTIONID"].ToString()) == null ? "Dummy" : dr["FLDCLEANOPTIONID"].ToString(); 
                    ddlregion.SelectedValue = General.GetNullableInteger(dr["FLDDOCREGION"].ToString()) == null ? "Dummy" : dr["FLDDOCREGION"].ToString();
                    ddlCountry.SelectedValue = General.GetNullableInteger(dr["FLDCOUNTRYID"].ToString()) == null ? "Dummy" : dr["FLDCOUNTRYID"].ToString();
                    txtYard.Text = dr["FLDYARDNAME"].ToString();
                    UcCleaningstartDate.Text = dr["FLDCLEANINGSTARTDATE"].ToString();
                    UcCleaningEndDate.Text = dr["FLDCLEANINGENDDATE"].ToString();
                    txtAmountAdditive.Text = dr["FLDAMOUNTADDITIVEREQ"].ToString();
                    txtSupplier.Text = dr["FLDSUPPLIER"].ToString();
                    UcSupplyNoLater.Text = dr["FLDSUPPLYNOLATERTHANDATE"].ToString();
                    ddlSupplyregion.SelectedValue = General.GetNullableInteger(dr["FLDSUPPLYREGION"].ToString())==null?"Dummy": dr["FLDSUPPLYREGION"].ToString();
                    ddlSupplycountry.SelectedValue = General.GetNullableInteger(dr["FLDSUPPLYCOUNTRY"].ToString()) == null ? "Dummy" : dr["FLDSUPPLYCOUNTRY"].ToString(); 
                    ddlSupplyport.SelectedValue = General.GetNullableInteger(dr["FLDSUPPLYPORT"].ToString()) == null ? "Dummy" : dr["FLDSUPPLYPORT"].ToString(); 
                    txtComment.Text = dr["FLDCOMMENT"].ToString();
                    UcBunkerDate.Text = dr["FLDFIRSTBUNKERNOLATERTHANDATE"].ToString();
                    ddlBunkerRegion.SelectedValue = General.GetNullableInteger(dr["FLDBUNKERREGION"].ToString()) == null ? "Dummy" : dr["FLDBUNKERREGION"].ToString(); 
                    ddlBunkerCountry.SelectedValue = General.GetNullableInteger(dr["FLDBUNKERCOUNTRY"].ToString()) == null ? "Dummy" : dr["FLDBUNKERCOUNTRY"].ToString(); 
                    ddlBunkerPort.SelectedValue = General.GetNullableInteger(dr["FLDBUNKERPORT"].ToString()) == null ? "Dummy" : dr["FLDBUNKERPORT"].ToString(); 
                    UcFlushingDate.Text = dr["FLDFLUSHINGDATE"].ToString();
                    txtFuelQtyNeed.Text = dr["FLDAMOUNTOFFUELNEEDED"].ToString();

                    Configure();
                    configtext();
                }
            }
            else
            {
               

                BindCountry();
                BindSupplyCountry();
                BindBunkerCountry();
                BindSupplyPort();
                BindBunkerPort();
                Configure();
                configtext();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuRiskassessmentplan_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (General.GetNullableGuid( ViewState["SIPTANKCLEANINGANDBUNKERID"].ToString()) == null)
                {
                    Guid? siptankcleaningandbunkerid = null;

                    PhoenixVesselPositionSIPTankCleaning.InsertSIPTankClean(
                           ref siptankcleaningandbunkerid
                         , General.GetNullableInteger(UcVessel.SelectedVessel)
                         , General.GetNullableGuid(ViewState["TANKID"].ToString())
                         , General.GetNullableInteger(ddlMethod.SelectedValue)
                         , General.GetNullableString(ddlOption.SelectedValue)
                         , General.GetNullableInteger(ddlOption.SelectedValue)
                         , General.GetNullableString(lblInstruction.Text)
                         , General.GetNullableInteger(ddlregion.SelectedValue)
                         , General.GetNullableInteger(ddlCountry.SelectedValue)
                         , General.GetNullableString(txtYard.Text)
                         , General.GetNullableDateTime(UcCleaningstartDate.Text)
                         , General.GetNullableDateTime(UcCleaningEndDate.Text)
                         , General.GetNullableDecimal(txtAmountAdditive.Text)
                         , General.GetNullableString(txtSupplier.Text)
                         , General.GetNullableDateTime(UcSupplyNoLater.Text)
                         , General.GetNullableInteger(ddlSupplyregion.SelectedValue)
                         , General.GetNullableInteger(ddlSupplycountry.SelectedValue)
                         , General.GetNullableInteger(ddlSupplyport.SelectedValue)
                         , General.GetNullableString(txtComment.Text)
                         , General.GetNullableDateTime(UcBunkerDate.Text)
                         , General.GetNullableInteger(ddlBunkerRegion.SelectedValue)
                         , General.GetNullableInteger(ddlBunkerCountry.SelectedValue)
                         , General.GetNullableInteger(ddlBunkerPort.SelectedValue)
                         , General.GetNullableDateTime(UcFlushingDate.Text)
                         , General.GetNullableDecimal(txtFuelQtyNeed.Text)
                         , General.GetNullableInteger(ViewState["tanktype"].ToString())
                        );

                    ViewState["SIPTANKCLEANINGANDBUNKERID"] = siptankcleaningandbunkerid;
                }
                else
                {
                    PhoenixVesselPositionSIPTankCleaning.UpdateSIPTankClean(
                           General.GetNullableGuid(ViewState["SIPTANKCLEANINGANDBUNKERID"].ToString())
                         , General.GetNullableInteger(ddlMethod.SelectedValue)
                         , General.GetNullableString(ddlOption.SelectedValue)
                         , General.GetNullableInteger(ddlOption.SelectedValue)
                         , General.GetNullableString(lblInstruction.Text)
                         , General.GetNullableInteger(ddlregion.SelectedValue)
                         , General.GetNullableInteger(ddlCountry.SelectedValue)
                         , General.GetNullableString(txtYard.Text)
                         , General.GetNullableDateTime(UcCleaningstartDate.Text)
                         , General.GetNullableDateTime(UcCleaningEndDate.Text)
                         , General.GetNullableDecimal(txtAmountAdditive.Text)
                         , General.GetNullableString(txtSupplier.Text)
                         , General.GetNullableDateTime(UcSupplyNoLater.Text)
                         , General.GetNullableInteger(ddlSupplyregion.SelectedValue)
                         , General.GetNullableInteger(ddlSupplycountry.SelectedValue)
                         , General.GetNullableInteger(ddlSupplyport.SelectedValue)
                         , General.GetNullableString(txtComment.Text)
                         , General.GetNullableDateTime(UcBunkerDate.Text)
                         , General.GetNullableInteger(ddlBunkerRegion.SelectedValue)
                         , General.GetNullableInteger(ddlBunkerCountry.SelectedValue)
                         , General.GetNullableInteger(ddlBunkerPort.SelectedValue)
                         , General.GetNullableDateTime(UcFlushingDate.Text)
                         , General.GetNullableDecimal(txtFuelQtyNeed.Text)
                         , General.GetNullableInteger(ViewState["tanktype"].ToString())
                        );

                    
                }
                //BindData();

                String script = "javascript:fnReloadList('codehelp1',null,'True');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

                ucStatus.Text = "Record Saved Successfully";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ddlMethod_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            Configure();
            configtext();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlOption_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            Configure();
            configtext();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void configtext()
    {

        if (ddlMethod.SelectedValue.ToString().ToUpper() == "29")
            lblInstruction.Text = "* Approx. 1-2 weeks";
        else if ((ddlOption.SelectedValue.ToString().ToUpper() == "34" || ddlOption.SelectedValue.ToString().ToUpper() == "35") && ddlMethod.SelectedValue.ToString().ToUpper() == "30")
            lblInstruction.Text = "* Check with provider";
        else if ((ddlOption.SelectedValue.ToString().ToUpper() == "33") && ddlMethod.SelectedValue.ToString().ToUpper() == "30")
            lblInstruction.Text = "* Approx. 4 days / tank";
        else if ((ddlOption.SelectedValue.ToString().ToUpper() == "32") && ddlMethod.SelectedValue.ToString().ToUpper() == "30")
            lblInstruction.Text = "* Approx. 1 week / tank";
        else
            lblInstruction.Text = "";
    }
    private void Configure()
    {
        if (ddlMethod.SelectedValue.ToString().ToUpper() == "29")
        {
            lblOption.Visible = false;
            ddlOption.Visible = false;

           // tryardplace.Visible = true;
            trDate.Visible = true;
            trSupplier.Visible = false;
            trSupplyArea.Visible = false;

        }

        else if (ddlMethod.SelectedValue.ToString().ToUpper() == "30")
        {
            lblOption.Visible = true;
            ddlOption.Visible = true;
            tryardplace.Visible = false;

            trDate.Visible = true;

            if (ddlOption.SelectedValue.ToString().ToUpper() == "32" || ddlOption.SelectedValue.ToString().ToUpper() == "33")
            {
                trSupplier.Visible = false;
                trSupplyArea.Visible = false;
            }
            else if ((ddlOption.SelectedValue.ToString().ToUpper() == "34" || ddlOption.SelectedValue.ToString().ToUpper() == "35"))
            {
                trSupplier.Visible = true;
                //trSupplyArea.Visible = true;
            }

        }
        else if(ddlMethod.SelectedValue.ToString().ToUpper() == "31")
        {
            lblOption.Visible = false;
            ddlOption.Visible = false;
            tryardplace.Visible = false;
            trDate.Visible = false;
            trSupplier.Visible = false;
            trSupplyArea.Visible = false;
        }
    }
    protected void ddlregion_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Regionid"] = ddlregion.SelectedValue;
        BindCountry();
    }
    private void BindCountry()
    {
        DataTable dt = PhoenixRegistersRegion.ListRegion(General.GetNullableInteger(ViewState["Regionid"].ToString()));
        ddlCountry.DataSource = dt;
        ddlCountry.DataTextField = "FLDCOUNTRYNAME";
        ddlCountry.DataValueField = "FLDCOUNTRYCODE";
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ddlSupplyregion_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["SupplyRegionid"] = ddlSupplyregion.SelectedValue;
        BindSupplyCountry();
        BindSupplyPort();
    }
    protected void ddlSupplycountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["SupplyCountryid"] = ddlSupplycountry.SelectedValue;
        BindSupplyPort();
    }
    private void BindSupplyCountry()
    {
        DataTable dt = PhoenixRegistersRegion.ListRegion(General.GetNullableInteger(ViewState["SupplyRegionid"].ToString()));
        ddlSupplycountry.DataSource = dt;
        ddlSupplycountry.DataTextField = "FLDCOUNTRYNAME";
        ddlSupplycountry.DataValueField = "FLDCOUNTRYCODE";
        ddlSupplycountry.DataBind();
        ddlSupplycountry.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void BindSupplyPort()
    {
        int? countryid = General.GetNullableInteger(ViewState["SupplyCountryid"].ToString()) == null ? 0 : General.GetNullableInteger(ViewState["SupplyCountryid"].ToString());
        DataSet ds = PhoenixRegistersSeaport.ListCountrySeaport(countryid);
        ddlSupplyport.DataSource = ds;
        ddlSupplyport.DataTextField = "FLDSEAPORTNAME";
        ddlSupplyport.DataValueField = "FLDSEAPORTID";
        ddlSupplyport.DataBind();
        ddlSupplyport.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ddlBunkerRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["BunkerRegionid"] = ddlBunkerRegion.SelectedValue;
        BindBunkerCountry();
        BindBunkerPort();
    }
    protected void ddlBunkerCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["BunkerCountryid"] = ddlBunkerCountry.SelectedValue;
        BindBunkerPort();
    }
    private void BindBunkerCountry()
    {
        DataTable dt = PhoenixRegistersRegion.ListRegion(General.GetNullableInteger(ViewState["BunkerRegionid"].ToString()));
        ddlBunkerCountry.DataSource = dt;
        ddlBunkerCountry.DataTextField = "FLDCOUNTRYNAME";
        ddlBunkerCountry.DataValueField = "FLDCOUNTRYCODE";
        ddlBunkerCountry.DataBind();
        ddlBunkerCountry.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void BindBunkerPort()
    {
        int? countryid = General.GetNullableInteger(ViewState["BunkerCountryid"].ToString()) == null ? 0 : General.GetNullableInteger(ViewState["BunkerCountryid"].ToString());
        DataSet ds = PhoenixRegistersSeaport.ListCountrySeaport(countryid);
        ddlBunkerPort.DataSource = ds;
        ddlBunkerPort.DataTextField = "FLDSEAPORTNAME";
        ddlBunkerPort.DataValueField = "FLDSEAPORTID";
        ddlBunkerPort.DataBind();
        ddlBunkerPort.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}