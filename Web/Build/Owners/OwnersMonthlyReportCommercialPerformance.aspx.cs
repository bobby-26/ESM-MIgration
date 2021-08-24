using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;
using SouthNests.Phoenix.VesselPosition;
using System;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class OwnersMonthlyReportCommercialPerformance : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        rdsummary.Visible = SessionUtil.CanAccess(this.ViewState, "SUMMARY");
        rdlubeoil.Visible = SessionUtil.CanAccess(this.ViewState, "LUBEOIL");
        rdfreshwater.Visible = SessionUtil.CanAccess(this.ViewState, "FRESHWATER");
        rdFuleOil.Visible = SessionUtil.CanAccess(this.ViewState, "FUELOIL");

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        BindVPRS();
        BindData();
        if (!IsPostBack)
        {
            lnkSummaryComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Summary', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=SUB');return false; ");
            lnkLubOilsComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Lub Oils', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=LUB');return false; ");
            lnkFreshWaterComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Fresh Water', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=FEW');return false; ");
            lnkFuelOilComments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', 'Fuel Oil', '" + Session["sitepath"] + "/Inspection/InspectionOwnersReportComments.aspx?VESSELID=" + Filter.SelectedOwnersReportVessel + "&DATE=" + Filter.SelectedOwnersReportDate + "&CODE=FOI');return false; ");

            lnkSummaryInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Summary','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=SUB" + "',false, 320, 250,'','',options); return false;");
            lnkLubOilsInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Lub Oils','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=LUB" + "',false, 320, 250,'','',options); return false;");
            lnkFreshWaterInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Fresh Water','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=FEW" + "',false, 320, 250,'','',options); return false;");
            lnkFuelOilInfo.Attributes.Add("onclick", "javascript: var options={disableMinMax:true}; top.openNewWindow('code1','Fuel Oil','" + Session["sitepath"] + "/Owners/OwnersMonthlyReportModuleInfo.aspx?CODE=FOI" + "',false, 320, 250,'','',options); return false;");

            CheckComments();
        }
    }

    private void CheckComments()
    {
        DataTable dt = new DataTable();

        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("SUB", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkSummaryComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("LUB", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkLubOilsComments.Controls.Add(html);
        }

        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("FEW", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkFreshWaterComments.Controls.Add(html);
        }
        dt = new DataTable();
        dt = PhoenixOwnerReportComments.ListOwnerReportsComments("FOI", DateTime.Parse(Filter.SelectedOwnersReportDate), int.Parse(Filter.SelectedOwnersReportVessel));
        if (dt.Rows.Count > 0)
        {
            HtmlGenericControl html = new HtmlGenericControl();
            html.InnerHtml = "<span class=\"icon\"> <i class=\"fa fa-Chat\"></i></span>";
            lnkFuelOilComments.Controls.Add(html);
        }

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        CheckComments();
    }

    private void BindVPRS()
    {
        ViewState["MONTHLYREPORTID"] = string.Empty;
        ViewState["MONTH"] = string.Empty;
        ViewState["YEAR"] = string.Empty;

        DataTable dt = PhoenixOwnerReportQuality.OwnersReportVPRS(int.Parse(Filter.SelectedOwnersReportVessel), General.GetNullableDateTime(Filter.SelectedOwnersReportDate));
        if (dt.Rows.Count > 0)
        {
            ViewState["MONTHLYREPORTID"] = dt.Rows[0]["FLDVESSELMONTHLYREPORTID"].ToString();
            ViewState["MONTH"] = dt.Rows[0]["FLDMONTH"].ToString();
            ViewState["YEAR"] = dt.Rows[0]["FLDYEAR"].ToString();
        }
    }

    private void BindData()
    {
        string monthlyreportid = ViewState["MONTHLYREPORTID"].ToString();

        DataSet ds = new DataSet();

        //if (monthlyreportid == "")
        //    ds = PhoenixVesselPositionMonthlyReport.EditMonthlyReportCalculation(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), General.GetNullableInteger(ViewState["MONTH"].ToString()), General.GetNullableInteger(ViewState["YEAR"].ToString()));
        //else
        ds = PhoenixVesselPositionMonthlyReport.EditMonthlyReport(General.GetNullableGuid(monthlyreportid));

        DataTable dt = ds.Tables[0];

        if (ds.Tables[0].Rows.Count > 0)
        {
            txtballaststeamingtime.Text = dt.Rows[0]["FLDBALLASTSTEAMINGTIME"].ToString();
            txtloadedsteamingtime.Text = dt.Rows[0]["FLDLOADEDSTEAMINGTIME"].ToString();
            txttotalsteamingtime.Text = dt.Rows[0]["FLDTOTALSTEAMINGTIME"].ToString();
            txtballastmanoeveringtime.Text = dt.Rows[0]["FLDBALLASTMANOEVERINGTIME"].ToString();
            txtloadedmanoeveringtime.Text = dt.Rows[0]["FLDLOADEDMANOEVERINGTIME"].ToString();
            txttotalmanoeveringtime.Text = dt.Rows[0]["FLDTOTALMANOEVERINGTIME"].ToString();
            txttotaldeviationordelay.Text = dt.Rows[0]["FLDTOTALDEVIATIONORDELAY"].ToString();
            txtballastdistancesteamed.Text = dt.Rows[0]["FLDBALLASTDISTANCESTEAMED"].ToString();
            txtloadeddistancesteamed.Text = dt.Rows[0]["FLDLOADEDDISTANCESTEAMED"].ToString();
            txttotaldistancesteamed.Text = dt.Rows[0]["FLDTOTALDISTANCESTEAMED"].ToString();
            txtballastmestoppage.Text = dt.Rows[0]["FLDBALLASTMESTOPPAGE"].ToString();
            txtloadedmestoppage.Text = dt.Rows[0]["FLDLOADEDMESTOPPAGE"].ToString();
            txttotalmestoppage.Text = dt.Rows[0]["FLDTOTALMESTOPPAGE"].ToString();
            txtballastavgspeed.Text = dt.Rows[0]["FLDBALLASTAVGSPEED"].ToString();
            txtloadedavgspeed.Text = dt.Rows[0]["FLDLOADEDAVGSPEED"].ToString();
            txttotalavgspeed.Text = dt.Rows[0]["FLDTOTALAVGSPEED"].ToString();
            txttotalavgbhp.Text = dt.Rows[0]["FLDTOTALAVGKW"].ToString();
            txtballastavgrpm.Text = dt.Rows[0]["FLDBALLASTAVGRPM"].ToString();
            txtloadedavgrpm.Text = dt.Rows[0]["FLDLOADEDAVGRPM"].ToString();
            txttotalavgrpm.Text = dt.Rows[0]["FLDTOTALAVGRPM"].ToString();
            txtballastavgslip.Text = dt.Rows[0]["FLDBALLASTAVGSLIP"].ToString();
            txtloadedavgslip.Text = dt.Rows[0]["FLDLOADEDAVGSLIP"].ToString();
            txttotalavgslip.Text = dt.Rows[0]["FLDTOTALAVGSLIP"].ToString();
            txtballastavgfoconsumptionperday.Text = dt.Rows[0]["FLDBALLASTAVGFOCONSUMPTIONPERDAY"].ToString();
            txtloadedavgfoconsumptionperday.Text = dt.Rows[0]["FLDLOADEDAVGFOCONSUMPTIONPERDAY"].ToString();
            txttotalavgfoconsumptionperday.Text = dt.Rows[0]["FLDTOTALAVGFOCONSUMPTIONPERDAY"].ToString();
            txtballastfullspeed.Text = dt.Rows[0]["FLDBALLASTFULLSPEED"].ToString();
            txtloadedfullspeed.Text = dt.Rows[0]["FLDLOADEDFULLSPEED"].ToString();
            txttotalfullspeed.Text = dt.Rows[0]["FLDTOTALFULLSPEED"].ToString();
            txtballastreducedspeed.Text = dt.Rows[0]["FLDBALLASTREDUCEDSPEED"].ToString();
            txtloadedreducedspeed.Text = dt.Rows[0]["FLDLOADEDREDUCEDSPEED"].ToString();
            txttotalreducedspeed.Text = dt.Rows[0]["FLDTOTALREDUCEDSPEED"].ToString();
            txtballastenginedistance.Text = dt.Rows[0]["FLDBALLASTENGINEDISTANCE"].ToString();
            txtloadedenginedistance.Text = dt.Rows[0]["FLDLOADEDENGINEDISTANCE"].ToString();
            txttotalenginedistance.Text = dt.Rows[0]["FLDTOTALENGINEDISTANCE"].ToString();
            txtballastAvgEEOI.Text = dt.Rows[0]["FLDBALLASTAVGEEOI"].ToString();
            txtloadedAvgEEOI.Text = dt.Rows[0]["FLDLOADEDAVGEEOI"].ToString();
            txttotalAvgEEOI.Text = dt.Rows[0]["FLDTOTALAVGEEOI"].ToString();
        }
    }

    protected void gvlubeoil_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        string monthlyreportid = ViewState["MONTHLYREPORTID"].ToString();

        DataSet ds = new DataSet();

        //if (monthlyreportid == "")
        //    ds = PhoenixVesselPositionMonthlyReport.ListMonthlyReportOilConsumptionCalculation(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), General.GetNullableInteger(ViewState["MONTH"].ToString()), General.GetNullableInteger(ViewState["YEAR"].ToString()));
        //else
        ds = PhoenixVesselPositionMonthlyReport.ListMonthlyReportOilConsumption(General.GetNullableInteger(Filter.SelectedOwnersReportVessel),
                                                                          General.GetNullableGuid(ViewState["MONTHLYREPORTID"].ToString()));

        gvlubeoil.DataSource = ds.Tables[1];
    }

    protected void gvfreshwater_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        string monthlyreportid = ViewState["MONTHLYREPORTID"].ToString();

        DataSet ds = new DataSet();

        //if (monthlyreportid == "")
        //    ds = PhoenixVesselPositionMonthlyReport.ListMonthlyReportOilConsumptionCalculation(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), General.GetNullableInteger(ViewState["MONTH"].ToString()), General.GetNullableInteger(ViewState["YEAR"].ToString()));
        //else
        ds = PhoenixVesselPositionMonthlyReport.ListMonthlyReportOilConsumption(General.GetNullableInteger(Filter.SelectedOwnersReportVessel),
                                                                          General.GetNullableGuid(ViewState["MONTHLYREPORTID"].ToString()));
        gvfreshwater.DataSource = ds.Tables[2];
    }

    protected void gvFuleOil_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        string monthlyreportid = ViewState["MONTHLYREPORTID"].ToString();

        DataSet ds = new DataSet();

        //if (monthlyreportid == "")
        //    ds = PhoenixVesselPositionMonthlyReport.ListMonthlyReportOilConsumptionCalculation(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), General.GetNullableInteger(ViewState["MONTH"].ToString()), General.GetNullableInteger(ViewState["YEAR"].ToString()));
        //else
        ds = PhoenixVesselPositionMonthlyReport.ListMonthlyReportOilConsumption(General.GetNullableInteger(Filter.SelectedOwnersReportVessel),
                                                                          General.GetNullableGuid(ViewState["MONTHLYREPORTID"].ToString()));
        gvFuleOil.DataSource = ds.Tables[0];
    }
}