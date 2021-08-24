using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.DataVisualization.Charting;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class VesselPositionESIChart : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbartap = new PhoenixToolbar();
            toolbartap.AddButton("ESI", "ESI");
            toolbartap.AddButton("Quarterly EEOI", "EEOI");
            toolbartap.AddButton("BDN", "BDN");
            toolbartap.AddButton("Chart", "CHART");
            MenuTab.AccessRights = this.ViewState;
            MenuTab.MenuList = toolbartap.Show();
            MenuTab.SelectedMenuIndex = 3;

            if (!IsPostBack)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVessel.Enabled = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ESI"))
        {
            Response.Redirect("../VesselPosition/VesselPositionESIRegister.aspx");
        }
        if (CommandName.ToUpper().Equals("BDN"))
        {
            Response.Redirect("../VesselPosition/VesselPositionBunkerReceiptList.aspx");
        }
        if (CommandName.ToUpper().Equals("EEOI"))
        {
            Response.Redirect("../VesselPosition/vesselpositionyeartodatequaterreport.aspx");
        }
    }

    protected void ShowChart(object sender, EventArgs e)
    {
        try
        {
            DisplayChart();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void chkByDate_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (chkByDate.Checked == true)
            {
                txtFromDate.Enabled = true;
                txtToDate.Enabled = true;
            }
            else
            {
                txtFromDate.Enabled = false;
                txtToDate.Enabled = false;
                txtFromDate.Text = "";
                txtToDate.Text = "";
            }

            DisplayChart();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void rbl_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DisplayChart();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DisplayChart()
    {
        try
        {
            if (!IsValidFilter())
            {
                ucError.Visible = true;
                return;
            }

            DataSet ds = new DataSet();
            DataTable dt = new DataTable();

            ds = PhoenixVesselPositionNoonReport.ESIChartDataList("," + UcVessel.SelectedVessel + ","
                                                                    , General.GetNullableInteger(rblballastladen.SelectedValue)
                                                                    , General.GetNullableInteger(rblday.SelectedValue)
                                                                    , rblParameter.SelectedValue
                                                                    , chkByDate.Checked == true ? General.GetNullableDateTime(txtFromDate.Text) : General.GetNullableDateTime("")
                                                                    , chkByDate.Checked == true ? General.GetNullableDateTime(txtToDate.Text) : General.GetNullableDateTime(""));
            dt = ds.Tables[0];

            string strChartHeader = "";
            if (rblParameter.SelectedItem.Value == "CO2EMISSION")
                strChartHeader = "CO2 Emission (mT)";
            if (rblParameter.SelectedItem.Value == "CO2INDEX")
                strChartHeader = "CO2 index (kg CO2/nm)";
            if (rblParameter.SelectedItem.Value == "EEOI")
                strChartHeader = "EEOI (gm / mT . nm)";

            if (rblParameter.SelectedItem.Value == "SOXIND")
                strChartHeader = "SOx emission indicator (g/t-nm)";
            if (rblParameter.SelectedItem.Value == "NOXIND")
                strChartHeader = "NOx emission indicator (g/t-nm)";

            if (rblParameter.SelectedItem.Value == "ESISOX")
                strChartHeader = "ESI_SOx";
            if (rblParameter.SelectedItem.Value == "OVERALLESI")
                strChartHeader = "OverAll ESI";

            PhoenixCommonChart pcc = new PhoenixCommonChart(ChartESIScores, strChartHeader);
            pcc.ChartType = SeriesChartType.Line;
            pcc.YSeries("", new YAxisColumn("FLDESIYDATA", rblParameter.SelectedItem.Text));
            pcc.XSeries("", 1, "FLDESIXDATA");
            pcc.Enable3D = false;
            pcc.Show(dt);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(UcVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        return (!ucError.IsError);
    }
}
