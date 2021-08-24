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

public partial class VesselPositionSIPTankCleanandBunkerLubDetail : PhoenixBasePage
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

            ViewState["BunkerRegionid"] = "";
            ViewState["BunkerCountryid"] = "";

            BindBunkerRegion();
            BindData();
        }

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

                    ViewState["BunkerRegionid"] = dr["FLDBUNKERREGION"].ToString();
                    ViewState["BunkerCountryid"] = dr["FLDBUNKERCOUNTRY"].ToString();

                    BindBunkerCountry();
                    BindBunkerPort();

                    UcBunkerDate.Text = dr["FLDFIRSTBUNKERNOLATERTHANDATE"].ToString();
                    ddlBunkerRegion.SelectedValue = General.GetNullableInteger(dr["FLDBUNKERREGION"].ToString()) == null ? "Dummy" : dr["FLDBUNKERREGION"].ToString();
                    ddlBunkerCountry.SelectedValue = General.GetNullableInteger(dr["FLDBUNKERCOUNTRY"].ToString()) == null ? "Dummy" : dr["FLDBUNKERCOUNTRY"].ToString();
                    ddlBunkerPort.SelectedValue = General.GetNullableInteger(dr["FLDBUNKERPORT"].ToString()) == null ? "Dummy" : dr["FLDBUNKERPORT"].ToString();
                    UcFlushingDate.Text = dr["FLDFLUSHINGDATE"].ToString();
                    txtFuelQtyNeed.Text = dr["FLDAMOUNTOFFUELNEEDED"].ToString();
                }
            }else
            {
                BindBunkerCountry();
                BindBunkerPort();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

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
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , General.GetNullableDateTime(UcBunkerDate.Text)
                         , General.GetNullableInteger(ddlBunkerRegion.SelectedValue)
                         , General.GetNullableInteger(ddlBunkerCountry.SelectedValue)
                         , General.GetNullableInteger(ddlBunkerPort.SelectedValue)
                         , General.GetNullableDateTime(UcFlushingDate.Text)
                         , General.GetNullableDecimal(txtFuelQtyNeed.Text)
                         , 2
                        );

                    ViewState["SIPTANKCLEANINGANDBUNKERID"] = siptankcleaningandbunkerid;
                }
                else
                {
                    PhoenixVesselPositionSIPTankCleaning.UpdateSIPTankClean(
                           General.GetNullableGuid(ViewState["SIPTANKCLEANINGANDBUNKERID"].ToString())
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , null
                         , General.GetNullableDateTime(UcBunkerDate.Text)
                         , General.GetNullableInteger(ddlBunkerRegion.SelectedValue)
                         , General.GetNullableInteger(ddlBunkerCountry.SelectedValue)
                         , General.GetNullableInteger(ddlBunkerPort.SelectedValue)
                         , General.GetNullableDateTime(UcFlushingDate.Text)
                         , General.GetNullableDecimal(txtFuelQtyNeed.Text)
                         , 2
                        );
                }

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