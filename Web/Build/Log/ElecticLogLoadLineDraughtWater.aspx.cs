using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Log_ElecticLogLoadLineDraughtWater : PhoenixBasePage
{
    int vesselId = 0;
    int usercode = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        ShowToolBar();
        if (IsPostBack == false)
        {
            GetLoadLineData();
        }
    }

    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();
        //toolbarmain.AddFontAwesomeButton("../Inspection/InspectionRegulation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        //toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvNewRegulations')", "Print", "<i class=\"fas fa-print\"></i>", "PRINT");
        gvTabStrip.MenuList = toolbarmain.Show();
        gvTabStrip.AccessRights = this.ViewState;

        toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Record of Drills, Training and Weekly Check of LSA / FFA", "DRILL", ToolBarDirection.Right);
        toolbarmain.AddButton("Deck Log Books", "DECKLOG", ToolBarDirection.Right);
        toolbarmain.AddButton("Configuration", "CONFIG", ToolBarDirection.Right);
        toolbarmain.AddButton("Instructions", "INSTRUCTIONS", ToolBarDirection.Right);
        gvMainTabStrip.MenuList = toolbarmain.Show();
        gvMainTabStrip.AccessRights = this.ViewState;
    }

    private void GetLoadLineData()
    {
        try
        {
            DataSet ds = PhoenixDeckLog.LoadLineDetails(vesselId, usercode);
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataRow row = ds.Tables[0].Rows[0];
                lblCenterOfDiscMeter.Text = row["FLDCENTEROFDISCPLACEDMETER"].ToString();
                lblCenterOfDiscCentiMeter.Text = row["FLDCENTEROFDISCPLACEDCENTIMETER"].ToString();
                lblMaxLoadLineFreshWaterMeter.Text = row["FLDMAXLOADLINEFRESHWATERMETER"].ToString();
                lblMaxLoadLineFreshWaterCentiMeter.Text = row["FLDMAXLOADLINEFRESHWATERCENTIMETER"].ToString();
                lblMaxLoadLineIndianSummerMeter.Text = row["FLDMAXLOADININDIANSUMMERMETER"].ToString();
                lblMaxLoadLineIndianSummerCentiMeter.Text = row["FLDMAXLOADININDIANSUMMERCENTIMETER"].ToString();
                lblMaxLoadLineSummerCenterDiscs.Text = row["FLDMAXLOADLINESUMMERCENTEROFDISC"].ToString();
                lblLoadLineWinterMeter.Text = row["FLDMAXLOADLINEINWINTERMETER"].ToString();
                lblLoadLineWinterCentiMeter.Text = row["FLDMAXLOADLINEINWINTERCENTIMETER"].ToString();
                lblLoadLineWinterNorthAtlanticMeter.Text = row["FLDMAXLOADLINEINWINTERNORTHATLATNICMETER"].ToString();
                lblLoadLineWinterNorthAtlanticCentiMeter.Text = row["FLDMAXLOADLINEINWINTERNORTHATLATNICCENTIMETER"].ToString();
                lblDraughtWaterSummerMeter.Text = row["FLDMAXDRAUGHTOFWATERINSUMMERMETER"].ToString();
                lblDraughtWaterSummerCentiMeter.Text = row["FLDMAXDRAUGHTOFWATERINSUMMERCENTIMETER"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
        }
    }

    protected void gvLoadLine_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        gvLoadLine.DataSource = new string[] { };
    }

    protected void gvLoadLine_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void gvLoadLine_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void gvMainTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("DRILL"))
        {
            Response.Redirect("../Log/ElectricLogDeckLogDrills.aspx");
        }
        if (CommandName.ToUpper().Equals("DECKLOG"))
        {
            Response.Redirect("../Log/ElectricLogDeckBook.aspx");
        }
        if (CommandName.ToUpper().Equals("CONFIG"))
        {
            Response.Redirect("../Log/ElectricLogDeckLogConfiguration.aspx");
        }
        if (CommandName.ToUpper().Equals("INSTRUCTIONS"))
        {
            Response.Redirect("../Log/ElectricLogDeckLogInstructions.aspx");
        }
    }
}