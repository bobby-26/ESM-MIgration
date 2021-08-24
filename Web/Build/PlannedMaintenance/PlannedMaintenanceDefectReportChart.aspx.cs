using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;

public partial class PlannedMaintenanceDefectReportChart : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        string compname = "";
        string valuelist = "";
        string colorlist = "";

        if (!IsPostBack)
        {
            ViewState["DEFECT"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["DEFECT"]))
            {
                ViewState["DEFECT"] = Request.QueryString["DEFECT"];
            }

            txtVesselName.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
        }

        if(ViewState["DEFECT"].ToString() == "1")
        {

            DataTable dt = PhoenixPlannedMaintenanceDefectJob.ComponentWiseDefect(General.GetNullableDateTime(txtFromDate.Text)
                                                                                   ,General.GetNullableDateTime(txtToDate.Text));

            if (dt.Rows.Count > 0)
            {
                int i = 0;
                while (i < dt.Rows.Count)
                {
                    compname = compname + "\"" + dt.Rows[i]["FLDCOMPONENTNAME"].ToString() + "\"" + ",";
                    valuelist = valuelist + dt.Rows[i]["FLDDEFECTCOUNT"].ToString() + ",";
                    colorlist = colorlist + "'#27727B'"+ ",";
                    i++;
                }
                compname = compname.TrimEnd(',');
                valuelist = valuelist.TrimEnd(',');
                colorlist = colorlist.TrimEnd(',');
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "chart popup", "var dataSName = [" + compname + "]; var dataValues= [" + valuelist + "]; var colourList = [" + colorlist + "]; callChartDialog('Equipment wise Defect Report', '', 'PMS',dataSName,dataValues,colourList,'Total Defects','ChartDiv');", true);
        }
	}
}