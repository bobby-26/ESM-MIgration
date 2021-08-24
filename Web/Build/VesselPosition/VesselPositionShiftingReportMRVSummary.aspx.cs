using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionShiftingReportMRVSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);


            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "DEPARTUREREPORT");
            toolbar.AddButton("Shifting", "DEPARTURE");
            toolbar.AddButton("Operations", "OPERATIONS");
            toolbar.AddButton("Emission In Port", "MRVSUMMARY");
            DepartureSummary.AccessRights = this.ViewState;
            DepartureSummary.MenuList = toolbar.Show();

            DepartureSummary.SelectedMenuIndex = 3;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void DepartureSummary_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("OPERATIONS"))
            {
                Response.Redirect("../VesselPosition/VesselPositionShiftingReportOperations.aspx");
            }
            if (CommandName.ToUpper().Equals("DEPARTURE"))
            {
                Response.Redirect("../VesselPosition/VesselPositionShiftingReportEdit.aspx");
            }
            if (CommandName.ToUpper().Equals("DEPARTUREREPORT"))
            {
                if (Filter.CurrentNoonReportLaunchFrom != null && Filter.CurrentNoonReportLaunchFrom == "ST")
                    Response.Redirect("VesselPositionReports.aspx", false);
                else
                    Response.Redirect("VesselPositionShiftingReport.aspx", false);
                //Response.Redirect("../VesselPosition/VesselPositionShiftingReport.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    private void BindData()
    {
        DataSet ds = PhoenixVesselPositionDepartureReport.DepartureMRVSummary(General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtVessel.Text = dr["FLDVESSEL"].ToString();
            txtVoyage.Text = dr["FLDVOYAGENUMBER"].ToString();
            chkBallast.Checked = dr["FLDBALLASTYN"].ToString()=="1"?false:true;
            chkLaden.Checked = dr["FLDBALLASTYN"].ToString() == "1" ? true : false;
            chkEUPortYN.Checked = dr["FLDEUPORTYN"].ToString() == "1" ? true : false;
            txtFromPort.Text = dr["FLDSEAPORTNAME"].ToString();
            txtStopped.Text = dr["FLDTOTALTIMEINPORT"].ToString();
            txtarrival.Text = dr["FLDARRIVALDATE"].ToString();
            if (General.GetNullableDateTime(dr["FLDARRIVALDATE"].ToString()) != null)
                txtArivalTime.SelectedDate = Convert.ToDateTime(dr["FLDARRIVALDATE"]);
            txtDeparture.Text = dr["FLDDEPARTUREDATE"].ToString();
            if (General.GetNullableDateTime(dr["FLDDEPARTUREDATE"].ToString()) != null)
                txtDepartureTime.SelectedDate = Convert.ToDateTime(dr["FLDDEPARTUREDATE"]);
            txtCO2Emitted.Text = dr["FLDTOTALCONSUMPTION"].ToString();
            txtmanouveringdist.Text = dr["FLDEMLOGMANOUVERINGDIST"].ToString();
        }

    }


    protected void gvConsumption_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionDepartureReport.DepartureMRVSummary(General.GetNullableGuid(Filter.CurrentVPRSShiftingReportSelection));
        gvConsumption.DataSource = ds.Tables[1];
    }
}
