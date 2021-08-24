using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElectricLogDeckLogConfiguration : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        SessionUtil.PageAccessRights(this.ViewState);
        ShowToolBar();
        if (IsPostBack == false)
        {
            CheckConfigurationVessel();
        }
    }

    private void CheckConfigurationVessel()
    {
        DataSet ds = PhoenixDeckLog.LoadLineDetails(vesselId, usercode);
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            Response.Redirect("../Log/ElecticLogLoadLineDraughtWater.aspx");
        }
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        gvTabStrip.MenuList = toolbar.Show();
        gvTabStrip.AccessRights = this.ViewState;
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (isValidInput() == true)
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixDeckLog.RadarConfiguration(vesselId,
                                                 usercode, 
                                                 Convert.ToInt32(txtRadarNo.Text),
                                                 Convert.ToDateTime(txtDateFitted.Text),
                                                 Convert.ToInt32(txtRadarNo.Text),
                                                 Convert.ToBoolean(chkNorthStablization.Checked),
                                                 Convert.ToInt32(txtPerformanceMonitor.Text),
                                                 Convert.ToInt32(txtMakeType.Text),
                                                 Convert.ToInt32(txtWaveBand.Text),
                                                 txtMajorModification.Text
                                                );

                PhoenixDeckLog.LoadLineConfiguration(vesselId,
                                              usercode,
                                              Convert.ToInt32(txtCenterOfDiscMeter.Text),
                                              Convert.ToInt32(txtCenterOfDiscCentiMeter.Text),
                                              Convert.ToInt32(txtMaxLoadLineFreshWaterMeter.Text),
                                              Convert.ToInt32(txtMaxLoadLineFreshWaterCentiMeter.Text),
                                              Convert.ToInt32(txtMaxLoadLineIndianSummerMeter.Text),
                                              Convert.ToInt32(txtMaxLoadLineIndianSummerCentiMeter.Text),
                                              Convert.ToInt32(txtMaxLoadLineSummerCenterDisc.Text),
                                              Convert.ToInt32(txtLoadLineWinterMeter.Text),
                                              Convert.ToInt32(txtLoadLineWinterCentiMeter.Text),
                                              Convert.ToInt32(txtLoadLineWinterNorthAtlanticMeter.Text),
                                              Convert.ToInt32(txtLoadLineWinterNorthAtlanticCentiMeter.Text),
                                              Convert.ToInt32(txtDraughtWaterSummerMeter.Text),
                                              Convert.ToInt32(txtDraughtWaterSummerCentiMeter.Text)
                                             );

                ucMessage.Text = "Saved Successfully";
                Response.Redirect("../Log/ElecticLogLoadLineDraughtWater.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
        }
    }

    private bool isValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrWhiteSpace(txtRadarNo.Text))
        {
            ucError.ErrorMessage = "Radar Numer is Required";
        }
        if (string.IsNullOrWhiteSpace(txtDateFitted.Text))
        {
            ucError.ErrorMessage = "Date Fitted is Required";
        }
        if (string.IsNullOrWhiteSpace(txtPerformanceMonitor.Text))
        {
            ucError.ErrorMessage = "Performance Monitor is Required";
        }
        if (string.IsNullOrWhiteSpace(txtMakeType.Text))
        {
            ucError.ErrorMessage = "Make Type is Required";
        }
        if (string.IsNullOrWhiteSpace(txtWaveBand.Text))
        {
            ucError.ErrorMessage = "Wave Band is Required";
        }
        if (string.IsNullOrWhiteSpace(txtMajorModification.Text))
        {
            ucError.ErrorMessage = "Major Modification is Required";
        }
        //Convert.ToInt32(txtCenterOfDiscMeter.Text),
        //                                      Convert.ToInt32(txtCenterOfDiscCentiMeter.Text),
        //                                      Convert.ToInt32(txtMaxLoadLineFreshWaterMeter.Text),
        //                                      Convert.ToInt32(txtMaxLoadLineFreshWaterCentiMeter.Text),
        //                                      Convert.ToInt32(txtMaxLoadLineIndianSummerMeter.Text),
        //                                      Convert.ToInt32(txtMaxLoadLineIndianSummerCentiMeter.Text),
        //                                      Convert.ToInt32(txtMaxLoadLineSummerCenterDisc.Text),
        //                                      Convert.ToInt32(txtLoadLineWinterMeter.Text),
        //                                      Convert.ToInt32(txtLoadLineWinterCentiMeter.Text),
        //                                      Convert.ToInt32(txtLoadLineWinterNorthAtlanticMeter.Text),
        //                                      Convert.ToInt32(txtLoadLineWinterNorthAtlanticCentiMeter.Text),
        //                                      Convert.ToInt32(txtDraughtWaterSummerMeter.Text),
        //                                      Convert.ToInt32(txtDraughtWaterSummerCentiMeter.Text)
        if (string.IsNullOrWhiteSpace(txtCenterOfDiscMeter.Text) || string.IsNullOrWhiteSpace(txtCenterOfDiscMeter.Text))
        {
            ucError.ErrorMessage = "Center of Disc is Required";
        }
        if (string.IsNullOrWhiteSpace(txtMaxLoadLineFreshWaterMeter.Text) || string.IsNullOrWhiteSpace(txtMaxLoadLineFreshWaterCentiMeter.Text))
        {
            ucError.ErrorMessage = "Maximum  load-line in fresh water Required";
        }
        if (string.IsNullOrWhiteSpace(txtMaxLoadLineIndianSummerMeter.Text) || string.IsNullOrWhiteSpace(txtMaxLoadLineIndianSummerCentiMeter.Text))
        {
            ucError.ErrorMessage = "Maximum  load-line in Indian summer Required";
        }
        if (string.IsNullOrWhiteSpace(txtLoadLineWinterMeter.Text) || string.IsNullOrWhiteSpace(txtLoadLineWinterCentiMeter.Text))
        {
            ucError.ErrorMessage = "Maximum  load-line in winter Required";
        }
        if (string.IsNullOrWhiteSpace(txtLoadLineWinterNorthAtlanticMeter.Text) || string.IsNullOrWhiteSpace(txtLoadLineWinterNorthAtlanticCentiMeter.Text))
        {
            ucError.ErrorMessage = "Maximum  load-line in winter North Atlantic Required";
        }
        if (string.IsNullOrWhiteSpace(txtDraughtWaterSummerMeter.Text) || string.IsNullOrWhiteSpace(txtDraughtWaterSummerCentiMeter.Text))
        {
            ucError.ErrorMessage = "Maximum  draught of water in summer";
        }
        if (string.IsNullOrWhiteSpace(txtMaxLoadLineSummerCenterDisc.Text))
        {
            ucError.ErrorMessage = "Maximum load-line in the center of the disc summer";
        }
        return ucError.IsError;
    }
}