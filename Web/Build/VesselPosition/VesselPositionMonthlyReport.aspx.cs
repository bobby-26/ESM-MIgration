using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselPosition;
using System.Text;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections;
using Telerik.Web.UI;

public partial class VesselPositionMonthlyReport : PhoenixBasePage
{
    DataSet dsDetail = new DataSet();
    DataSet dsOil = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();
            toolbarvoyagetap.AddButton("List", "MONTHLYREPORTLIST");
            toolbarvoyagetap.AddButton("Monthly Report", "MONTHLYREPORT");
            MenuMonthlyReportTap.AccessRights = this.ViewState;
            MenuMonthlyReportTap.MenuList = toolbarvoyagetap.Show();
            MenuMonthlyReportTap.SelectedMenuIndex = 1;


            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            
            cmdHiddenSubmits.Attributes.Add("style", "visibility:hidden;");
            txtCurrentDate.Text = System.DateTime.UtcNow.ToString();
            if (!IsPostBack)
            {
                for (int i = 2005; i <= DateTime.Now.Year; i++)
                {
                    ddlYear.Items.Add(new RadComboBoxItem(i.ToString(), i.ToString()));
                }
                ddlYear.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

                ddlYear.SelectedValue = DateTime.Now.Year.ToString();
                ddlMonth.SelectedValue = DateTime.Now.Month.ToString();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["MONTHLYREPORTID"] = "";
                ViewState["issubmitted"] = "";
                ViewState["MONTH"] = DateTime.Now.Month;
                ViewState["YEAR"] = DateTime.Now.Year;

                if (Request.QueryString["MonthlyReportID"] != null)
                {
                    ViewState["MONTHLYREPORTID"] = Request.QueryString["MonthlyReportID"].ToString();
                    Session["MONTHLYREPORTID"] = ViewState["MONTHLYREPORTID"].ToString();

                    ddlMonth.Enabled = false;
                    ddlYear.Enabled = false;
                }
                else
                {
                    if (Session["MONTHLYREPORTID"] == null)
                    {
                        Session["MONTHLYREPORTID"] = "";
                    }
                }

                BindData();
            }
            
            bindtabmenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void bindtabmenu()
    {
        PhoenixToolbar toolbarMonthlyReporttap = new PhoenixToolbar();
        toolbarMonthlyReporttap.AddButton("Export Excel", "EXCEL", ToolBarDirection.Right);
        toolbarMonthlyReporttap.AddButton("Export PDF", "PDF", ToolBarDirection.Right);
        if (ViewState["issubmitted"].ToString() != "1" && General.GetNullableGuid(ViewState["MONTHLYREPORTID"].ToString()) != null)
            toolbarMonthlyReporttap.AddButton("Submit", "SUMBIT", ToolBarDirection.Right);
        if (ViewState["issubmitted"].ToString() != "1" )
            toolbarMonthlyReporttap.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuNewSaveTabStrip.AccessRights = this.ViewState;
        MenuNewSaveTabStrip.MenuList = toolbarMonthlyReporttap.Show();
    }
    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("NEW"))
            {
                ResetMonthltReport();
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                //if ((ViewState["MONTHLYREPORTID"].ToString() == null) || ViewState["MONTHLYREPORTID"].ToString() == "")
                //{
                AddMontlyReport();
                AddMontlyReportConsumption();
                ucStatus.Text = "Monthly Report Saved Successfully ";
                // }
                //else
                //{
                //    UpdateMonthlyReport();
                //}
                BindData();
                bindtabmenu();
                RebindgvFuelOil();
                RebindgvLubOil();
                RebindgvWater();
            }
            if (CommandName.ToUpper().Equals("SUMBIT"))
            {
                PhoenixVesselPositionMonthlyReport.MonthlyReportStatusUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["MONTHLYREPORTID"].ToString()));
                BindData();
                bindtabmenu();
            }
            if (CommandName.ToUpper().Equals("PDF"))
            {

                    ConvertToPdf(PrepareHtmlDoc());
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {

                ConvertToExcel(PrepareHtmlDoc());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

     private void AddMontlyReport()
    {
        if (!IsValidMonth(ddlMonth.SelectedValue.ToString()))
        {
            ucError.Visible = true;
            return;
        }
        Guid? vesselmonthlyid = null;

        if (ViewState["MONTHLYREPORTID"] != null || ViewState["MONTHLYREPORTID"].ToString() != "")
            vesselmonthlyid = General.GetNullableGuid((ViewState["MONTHLYREPORTID"].ToString()));

        PhoenixVesselPositionMonthlyReport.InsertMonthlyReport(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                                                                int.Parse(ddlMonth.SelectedValue.ToString()),
                                                                int.Parse(ddlYear.SelectedValue.ToString()),
                                                                ref vesselmonthlyid,
                                                                General.GetNullableDecimal(txttotalavgbhp.Text.ToString()),
                                                                 General.GetNullableDecimal(txtradiotrafficcharterersac.Text.ToString()),
                                                                 General.GetNullableDecimal(txttotaldeviationordelay.Text.ToString())
                                                               );

        ViewState["MONTHLYREPORTID"] = vesselmonthlyid;

        Session["MONTHLYREPORTID"] = ViewState["MONTHLYREPORTID"].ToString();
        
        ddlMonth.Enabled = false;
        ddlYear.Enabled = false;
    }

    private void AddMontlyReportConsumption()
    {
        if (!IsValidMonth(ddlMonth.SelectedValue.ToString()))
        {
            ucError.Visible = true;
            return;
        }
        Guid? vesselmonthlyid = null;

        if (ViewState["MONTHLYREPORTID"] != null || ViewState["MONTHLYREPORTID"].ToString() != "")
            vesselmonthlyid = General.GetNullableGuid((ViewState["MONTHLYREPORTID"].ToString()));

        PhoenixVesselPositionMonthlyReport.InsertMonthlyOilConsumption(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                                                                int.Parse(ddlMonth.SelectedValue.ToString()),
                                                                int.Parse(ddlYear.SelectedValue.ToString()),
                                                                vesselmonthlyid
                                                               );

    }
    private void ResetMonthltReport()
    {
        ViewState["MONTHLYREPORTID"] = "";
        txtCurrentDate.Text = System.DateTime.Now.ToString();

        txtballaststeamingtime.Text = "";
        txtloadedsteamingtime.Text = "";
        txttotalsteamingtime.Text = "";
        txtballastmanoeveringtime.Text = "";
        txtloadedmanoeveringtime.Text = "";
        txttotalmanoeveringtime.Text = "";
        txttotaldeviationordelay.Text = "";
        txtballastdistancesteamed.Text = "";
        txtloadeddistancesteamed.Text = "";
        txttotaldistancesteamed.Text = "";
        txtballastmestoppage.Text = "";
        txtloadedmestoppage.Text = "";
        txttotalmestoppage.Text = "";
        txtballastavgspeed.Text = "";
        txtloadedavgspeed.Text = "";
        txttotalavgspeed.Text = "";
        txttotalavgbhp.Text = "";
        txtballastavgrpm.Text = "";
        txtloadedavgrpm.Text = "";
        txttotalavgrpm.Text = "";
        txtballastavgslip.Text = "";
        txtloadedavgslip.Text = "";
        txttotalavgslip.Text = "";
        txtballastavgfoconsumptionperday.Text = "";
        txtloadedavgfoconsumptionperday.Text = "";
        txttotalavgfoconsumptionperday.Text = "";
        txtradiotrafficcharterersac.Text = "";
        txthfoconsumptionfortankcleaning.Text = "";
        txtHFOConsforCargoHeating.Text = "";
        txttotalfwproduction.Text = "";
        txtballastfullspeed.Text = "";
        txtloadedfullspeed.Text = "";
        txttotalfullspeed.Text = "";
        txtballastreducedspeed.Text = "";
        txtloadedreducedspeed.Text = "";
        txttotalreducedspeed.Text = "";
        txtballastenginedistance.Text = "";
        txtloadedenginedistance.Text = "";
        txttotalenginedistance.Text = "";
        txtOverallESI.Text = "";
        txtballastAvgEEOI.Text = "";
        txtloadedAvgEEOI.Text = "";
        txttotalAvgEEOI.Text = "";
        txtFromDate.Text = "";
        txtToDate.Text = "";
        txtCO2Emission.Text  = "";
        txtCO2Index.Text  = "";
        txtEEOICO2.Text  = "";
        txtSOxEmission.Text  = "";
        txtSOxIndex.Text  = "";
        txtEEOISOx.Text  = "";
        txtNOxEmission.Text  = "";
        txtNOxIndex.Text  = "";
        txtEEOINOx.Text  = "";
    }

    protected void MonthlyReportTapp_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("MONTHLYREPORTLIST"))
        {
            if (Request.QueryString["vesselid"] != null)
                Response.Redirect("VesselPositionMonthlyReportList.aspx?vesselid="+ Request.QueryString["vesselid"], false);
            else
                Response.Redirect("VesselPositionMonthlyReportList.aspx", false);
        }
    }

    private void BindData()
    {
        string monthlyreportid = ViewState["MONTHLYREPORTID"].ToString();

        DataSet ds = new DataSet();

        if (monthlyreportid == "")
            ds = PhoenixVesselPositionMonthlyReport.EditMonthlyReportCalculation(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), General.GetNullableInteger(ViewState["MONTH"].ToString()), General.GetNullableInteger(ViewState["YEAR"].ToString()));
        else
            ds = PhoenixVesselPositionMonthlyReport.EditMonthlyReport(General.GetNullableGuid(monthlyreportid));
        
        
        
        DataTable dt = ds.Tables[0];
        dsDetail = ds;

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
            txttotalavgbhp.Text = dt.Rows[0]["FLDAVGBHP"].ToString();
            txttotalavgkw.Text = dt.Rows[0]["FLDTOTALAVGKW"].ToString();
            txtballastavgrpm.Text = dt.Rows[0]["FLDBALLASTAVGRPM"].ToString();
            txtloadedavgrpm.Text = dt.Rows[0]["FLDLOADEDAVGRPM"].ToString();
            txttotalavgrpm.Text = dt.Rows[0]["FLDTOTALAVGRPM"].ToString();
            txtballastavgslip.Text = dt.Rows[0]["FLDBALLASTAVGSLIP"].ToString();
            txtloadedavgslip.Text = dt.Rows[0]["FLDLOADEDAVGSLIP"].ToString();
            txttotalavgslip.Text = dt.Rows[0]["FLDTOTALAVGSLIP"].ToString();
            txtballastavgfoconsumptionperday.Text = dt.Rows[0]["FLDBALLASTAVGFOCONSUMPTIONPERDAY"].ToString();
            txtloadedavgfoconsumptionperday.Text = dt.Rows[0]["FLDLOADEDAVGFOCONSUMPTIONPERDAY"].ToString();
            txttotalavgfoconsumptionperday.Text = dt.Rows[0]["FLDTOTALAVGFOCONSUMPTIONPERDAY"].ToString();
            txtradiotrafficcharterersac.Text = dt.Rows[0]["FLDRADIOTRAFFICCHARTERERSAC"].ToString();
            txthfoconsumptionfortankcleaning.Text = dt.Rows[0]["FLDHFOCONSUMPTIONFORTANKCLEANING"].ToString();
            txtHFOConsforCargoHeating.Text = dt.Rows[0]["FLDHFOCONSUMPTIONFORFORCARGOHEATING"].ToString();
            txttotalfwproduction.Text = dt.Rows[0]["FLDTOTALFWPRODUCTION"].ToString();
            txtballastfullspeed.Text = dt.Rows[0]["FLDBALLASTFULLSPEED"].ToString();
            txtloadedfullspeed.Text = dt.Rows[0]["FLDLOADEDFULLSPEED"].ToString();
            txttotalfullspeed.Text = dt.Rows[0]["FLDTOTALFULLSPEED"].ToString();
            txtballastreducedspeed.Text = dt.Rows[0]["FLDBALLASTREDUCEDSPEED"].ToString();
            txtloadedreducedspeed.Text = dt.Rows[0]["FLDLOADEDREDUCEDSPEED"].ToString();
            txttotalreducedspeed.Text = dt.Rows[0]["FLDTOTALREDUCEDSPEED"].ToString();
            txtballastenginedistance.Text = dt.Rows[0]["FLDBALLASTENGINEDISTANCE"].ToString();
            txtloadedenginedistance.Text = dt.Rows[0]["FLDLOADEDENGINEDISTANCE"].ToString();
            txttotalenginedistance.Text = dt.Rows[0]["FLDTOTALENGINEDISTANCE"].ToString();
            txtOverallESI.Text = dt.Rows[0]["FLDOVERALLESI"].ToString();
            txtballastAvgEEOI.Text = dt.Rows[0]["FLDBALLASTAVGEEOI"].ToString();
            txtloadedAvgEEOI.Text = dt.Rows[0]["FLDLOADEDAVGEEOI"].ToString();
            txttotalAvgEEOI.Text = dt.Rows[0]["FLDTOTALAVGEEOI"].ToString();
            
            txtCO2Emission.Text  = dt.Rows[0]["FLDCO2EMISSION"].ToString();
            txtCO2Index.Text = dt.Rows[0]["FLDCO2INDEX"].ToString();
            txtEEOICO2.Text = dt.Rows[0]["FLDEEOICO2"].ToString();
            txtSOxEmission.Text = dt.Rows[0]["FLDSOXEMISSION"].ToString();
            txtSOxIndex.Text = dt.Rows[0]["FLDSOXINDEX"].ToString();
            txtEEOISOx.Text = dt.Rows[0]["FLDEEOISOX"].ToString();
            txtNOxEmission.Text = dt.Rows[0]["FLDNOXEMISSION"].ToString();
            txtNOxIndex.Text = dt.Rows[0]["FLDNOXINDEX"].ToString();
            txtEEOINOx.Text = dt.Rows[0]["FLDEEOINOX"].ToString();

            ViewState["issubmitted"] = dt.Rows[0]["FLDISSUBMITED"].ToString();
            if (monthlyreportid != "")
            {
                ddlMonth.SelectedValue = dt.Rows[0]["FLDMONTH"].ToString();
                ddlYear.SelectedValue = dt.Rows[0]["FLDYEAR"].ToString();
                txtCurrentDate.Text = dt.Rows[0]["FLDDATE"].ToString();
            }

            txtFromDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDREPORTFROMDATE"].ToString()) + " " + String.Format("{0:HH:mm}",dt.Rows[0]["FLDREPORTFROMDATE"]);
            txtToDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDREPORTTODATE"].ToString()) + " " + String.Format("{0:HH:mm}", dt.Rows[0]["FLDREPORTTODATE"]);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
    }


    protected void ddlMonth_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["MONTH"] = ddlMonth.SelectedValue;
        ViewState["YEAR"] = ddlYear.SelectedValue; ;
        BindData();
        RebindgvFuelOil();
        RebindgvLubOil();
        RebindgvWater();
    }

    private bool IsValidMonth(string month)
    {
        if (!General.GetNullableInteger(month).HasValue)
        {
            ucError.ErrorMessage = "Month is required.";
        }
        return (!ucError.IsError);
    }

    private bool IsValidMonthReport(string monthreport)
    {
        if (!General.GetNullableGuid(monthreport).HasValue)
        {
            ucError.ErrorMessage = "Please Save the Monthly Report First.";
        }
        return (!ucError.IsError);
    }

     private string PrepareHtmlDoc()
    {
        StringBuilder DsHtmlcontent = new StringBuilder();

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            DataRow dr1 = dsDetail.Tables[0].Rows[0];

            DsHtmlcontent.Append("<html><table ID='tbl1'><tr><td align=\"center\"><b>MONTHLY REPORT</b></td></tr></table><br />");

            DsHtmlcontent.Append("<table ID='tbl2' border='1' cellpadding='3' cellspacing='0' style='border-collapse: collapse;' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Vessel</td><td colspan=\"3\">" + dr1["FLDVESSELNAME"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Selected Period</td><td colspan=\"3\">" + dr1["MONTHSHORTNAME"].ToString() + "-" + dr1["FLDYEAR"].ToString() + "</td></tr>");
            //DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Vessel</td><td colspan=\"3\">HIGH JUPITER</td><tr>");
            //DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Selected Period</td><td colspan=\"3\">Apr-Jun-2017</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >From Date</td><td colspan=\"3\">" + General.GetDateTimeToString(dr1["FLDREPORTFROMDATE"].ToString()) + " " + String.Format("{0:HH:mm}", dr1["FLDREPORTFROMDATE"]) + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >From Date</td><td colspan=\"3\">" + General.GetDateTimeToString(dr1["FLDREPORTTODATE"].ToString()) + " " + String.Format("{0:HH:mm}", dr1["FLDREPORTTODATE"]) + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  ></td> <td>Ballast</td><td>Loaded</td><td>Total</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Steaming Time</td> <td>" + dr1["FLDBALLASTSTEAMINGTIME"].ToString() + "</td><td>" + dr1["FLDLOADEDSTEAMINGTIME"].ToString() + "</td><td>" + dr1["FLDTOTALSTEAMINGTIME"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Full Speed</td> <td>" + dr1["FLDBALLASTFULLSPEED"].ToString() + "</td><td>" + dr1["FLDLOADEDFULLSPEED"].ToString() + "</td><td>" + dr1["FLDTOTALFULLSPEED"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Reduced Speed</td> <td>" + dr1["FLDBALLASTREDUCEDSPEED"].ToString() + "</td><td>" + dr1["FLDLOADEDREDUCEDSPEED"].ToString() + "</td><td>" + dr1["FLDTOTALREDUCEDSPEED"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Manoevering Time</td> <td>" + dr1["FLDBALLASTMANOEVERINGTIME"].ToString() + "</td><td>" + dr1["FLDLOADEDMANOEVERINGTIME"].ToString() + "</td><td>" + dr1["FLDTOTALMANOEVERINGTIME"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >M/E Stoppage</td> <td>" + dr1["FLDBALLASTMESTOPPAGE"].ToString() + "</td><td>" + dr1["FLDLOADEDMESTOPPAGE"].ToString() + "</td><td>" + dr1["FLDTOTALMESTOPPAGE"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Deviation or Delay (if any)</td> <td></td><td></td><td>" + dr1["FLDTOTALDEVIATIONORDELAY"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Distance Steamed</td> <td>" + dr1["FLDBALLASTDISTANCESTEAMED"].ToString() + "</td><td>" + dr1["FLDLOADEDDISTANCESTEAMED"].ToString() + "</td><td>" + dr1["FLDTOTALDISTANCESTEAMED"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Engine Distance</td> <td>" + dr1["FLDBALLASTENGINEDISTANCE"].ToString() + "</td><td>" + dr1["FLDLOADEDENGINEDISTANCE"].ToString() + "</td><td>" + dr1["FLDTOTALENGINEDISTANCE"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Average Speed</td> <td>" + dr1["FLDBALLASTAVGSPEED"].ToString() + "</td><td>" + dr1["FLDLOADEDAVGSPEED"].ToString() + "</td><td>" + dr1["FLDTOTALAVGSPEED"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Average Slip</td> <td>" + dr1["FLDBALLASTAVGSLIP"].ToString() + "</td><td>" + dr1["FLDLOADEDAVGSLIP"].ToString() + "</td><td>" + dr1["FLDTOTALAVGSLIP"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Average RPM</td> <td>" + dr1["FLDBALLASTAVGRPM"].ToString() + "</td><td>" + dr1["FLDLOADEDAVGRPM"].ToString() + "</td><td>" + dr1["FLDTOTALAVGRPM"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Average BHP</td> <td></td><td></td><td>" + dr1["FLDTOTALAVGBHP"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Average FO Cons/Day</td> <td>" + dr1["FLDBALLASTAVGFOCONSUMPTIONPERDAY"].ToString() + "</td><td>" + dr1["FLDLOADEDAVGFOCONSUMPTIONPERDAY"].ToString() + "</td><td>" + dr1["FLDTOTALAVGFOCONSUMPTIONPERDAY"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Avg EEOI</td> <td>" + dr1["FLDBALLASTAVGEEOI"].ToString() + "</td><td>" + dr1["FLDLOADEDAVGEEOI"].ToString() + "</td><td>" + dr1["FLDTOTALAVGEEOI"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Overall ESI</td> <td>-</td><td>-</td><td>" + dr1["FLDOVERALLESI"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Co2 Emission (MT)</td> <td>-</td><td>-</td><td>" + dr1["FLDCO2EMISSION"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Co2 Index (kg/nm)</td> <td>-</td><td>-</td><td>" + dr1["FLDCO2INDEX"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >EEOI Co2 (g/nm-t)</td> <td>-</td><td>-</td><td>" + dr1["FLDEEOICO2"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >SOx Emission (MT)</td> <td>-</td><td>-</td><td>" + dr1["FLDSOXEMISSION"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >SOx Index (kg/nm)</td> <td>-</td><td>-</td><td>" + dr1["FLDSOXINDEX"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >EEOI  SOx (g/nm-t)</td> <td>-</td><td>-</td><td>" + dr1["FLDEEOISOX"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >NOx Emission (MT)</td> <td>-</td><td>-</td><td>" + dr1["FLDNOXEMISSION"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >NOx Index (kg/nm)</td> <td>-</td><td>-</td><td>" + dr1["FLDNOXINDEX"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >EEOI NOx (g/nm-t)</td> <td>-</td><td>-</td><td>" + dr1["FLDEEOINOX"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Radio Traffic Charterer's A/C (USD)</td> <td>" + dr1["FLDRADIOTRAFFICCHARTERERSAC"].ToString() + "</td><td>-</td><td>-</td></tr>");
            DsHtmlcontent.Append("</table>");

        }

        if (dsOil.Tables[0].Rows.Count > 0)
        {
            DataTable t1 = new DataTable();
            t1 = dsOil.Tables[0];


            DsHtmlcontent.Append("<br />");

            DsHtmlcontent.Append("<table ID=\"tbl3\" border ='1'  opacity='0.5' cellpadding=\"3\" cellspacing='0' width='100%' style='border:black 1px solid;'>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1' colspan=\"3\">Fuel Oil</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>ROB Prev Mth</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Bunkered</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>M/E</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>A/E</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Boiler</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>IGG</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>C/E</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>C/HTG</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>TK CLNG</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>OTH</th>");          
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>ROB</th>");
            //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Revised ROB</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Total Cons</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Avg Cons/Day</th></tr>");

            if (t1.Rows.Count > 0)
            {
                foreach (DataRow dr in t1.Rows)
                {
                    DsHtmlcontent.Append("<tr>");//colspan='2'
                    DsHtmlcontent.Append("<td colspan=\"3\">" + dr["FLDOILTYPENAME"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDOPENINGROB"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDOILBUNKERD"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDFUELOILCONSUMPTIONME"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDFUELOILCONSUMPTIONAE"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDFUELOILCONSUMPTIONBOILER"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDFUELOILCONSUMPTIONIGG"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDFUELOILCONSUMPTIONCE"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDFUELOILCONSUMPTIONCTHG"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDFUELOILCONSUMPTIONTKCLNG"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDFUELOILCONSUMPTIONOTH"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDFUELOILCONSUMPTIONROB"].ToString() + "</td>");
                   // DsHtmlcontent.Append("<td>" + dr["FLDREVISEDROB"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDTOTALCONSUMPTION"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDAVGCONSUMPTION"].ToString() + "</td>");
                    DsHtmlcontent.Append("</tr>");//colspan='2'
                }
            }
            DsHtmlcontent.Append("</table></br>");
        }


        //if (dsDetail.Tables[0].Rows.Count > 0)
        //{
        //    DataRow dr1 = dsDetail.Tables[0].Rows[0];
        //    DsHtmlcontent.Append("<br />");
        //    DsHtmlcontent.Append("<br/><table ID='tbl4' border='0.5' cellpadding='3' cellspacing='0' >");
        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >HFO Cons for Tank Cleaning</td> <td>" + dr1["FLDHFOCONSUMPTIONFORTANKCLEANING"].ToString() + "</td><td>-</td><td>-</td></tr>");
        //    DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >HFO Cons for Cargo Heating</td> <td>" + dr1["FLDHFOCONSUMPTIONFORFORCARGOHEATING"].ToString() + "</td><td>-</td><td>-</td></tr>");
        //    DsHtmlcontent.Append("</table>");

        //}

        if (dsOil.Tables[1].Rows.Count > 0)
        {
            DataTable t1 = new DataTable();
            t1 = dsOil.Tables[1];


            DsHtmlcontent.Append("<br />");

            DsHtmlcontent.Append("<table ID=\"tbl5\" border ='1'  opacity='0.5' cellpadding=\"3\" cellspacing='0' style='border:black 1px solid'>");
            //DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Vessel</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1' colspan=\"3\">Lub Oils</th>");
            //DsHtmlcontent.Append("<th bgcolor='#f1f1f1' width='60%'>Qty in Sump</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>ROB Prev Mth</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Received</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>ROB</th>");
           // DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Revised ROB</th>");
            //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>SG</th>");
            //DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Sp Cons</th>");
            //DsHtmlcontent.Append("<th bgcolor='#f1f1f1' colspan=\"2\">Reason for Loss/High Con</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Total Cons</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Avg Cons/Day</th></tr>");

            if (t1.Rows.Count > 0)
            {
                foreach (DataRow dr in t1.Rows)
                {
                    DsHtmlcontent.Append("<tr>");//colspan='2'
                    DsHtmlcontent.Append("<td colspan=\"3\">" + dr["FLDOILTYPENAME"].ToString() + "</td>");
                    //DsHtmlcontent.Append("<td>" + dr["FLDLUBOILQTYINSUMP"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDOPENINGROB"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDLUBOILRECEIVED"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDLUBOILROB"].ToString() + "</td>");
                    //DsHtmlcontent.Append("<td>" + dr["FLDREVISEDROB"].ToString() + "</td>");
                    //DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLUBOILSG"].ToString() + "</td>");
                    //DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLUBOILSPCONSUMPTION"].ToString() + "</td>");
                    //DsHtmlcontent.Append("<td colspan=\"2\">" + dr["FLDLUBOILREASONFORLOSSHIGHCONS"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDTOTALCONSUMPTION"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGCONSUMPTION"].ToString() + "</td>");
                    DsHtmlcontent.Append("</tr>");//colspan='2'
                }
            }
            DsHtmlcontent.Append("</table>");
        }

        if (dsOil.Tables[2].Rows.Count > 0)
        {
            DataTable t1 = new DataTable();
            t1 = dsOil.Tables[2];


            DsHtmlcontent.Append("<br />");

            DsHtmlcontent.Append("<table ID=\"tbl6\" border ='1'  opacity='0.5' cellpadding=\"3\" cellspacing='0' style='border:black 1px solid'>");
            //DsHtmlcontent.Append("<tr><th bgcolor='#f1f1f1'>Vessel</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1' width='60%' colspan=\"3\">Fresh Water</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>ROB Prev Mth</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Received</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>ROB</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Total Cons</th>");
            DsHtmlcontent.Append("<th bgcolor='#f1f1f1'>Avg Cons/Day</th></tr>");

            if (t1.Rows.Count > 0)
            {
                foreach (DataRow dr in t1.Rows)
                {
                    DsHtmlcontent.Append("<tr>");//colspan='2'
                    DsHtmlcontent.Append("<td colspan=\"3\">" + dr["FLDOILTYPENAME"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDOPENINGROB"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDWATERRECEIVED"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDWATERROB"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDTOTALCONSUMPTION"].ToString() + "</td>");
                    DsHtmlcontent.Append("<td>" + dr["FLDAVGCONSUMPTION"].ToString() + "</td>");
                    DsHtmlcontent.Append("</tr>");//colspan='2'
                }
            }
            DsHtmlcontent.Append("</table>");
        }

        if (dsDetail.Tables[0].Rows.Count > 0)
        {
            DataRow dr1 = dsDetail.Tables[0].Rows[0];

            DsHtmlcontent.Append("<br/><table ID='tbl7' border='1' cellpadding='3' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'  >Total FW Production</td> <td>" + dr1["FLDTOTALFWPRODUCTION"].ToString() + "</td><td>-</td><td>-</td></tr>");
            DsHtmlcontent.Append("</table></html>");

        }

        return DsHtmlcontent.ToString();

    }

     public void ConvertToPdf(string HTMLString)
     {
         try
         {
             if (HTMLString != "")
             {
                 using (var ms = new MemoryStream())
                 {
                     iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(595f, 842f));
                     document.SetMargins(36f, 36f, 36f, 0f);
                     document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                     string filefullpath = "MonthlyReport" + ".pdf";
                     PdfWriter.GetInstance(document, ms);
                     document.Open();

                     StyleSheet styles = new StyleSheet();
                     styles.LoadStyle(".headertable td", "background-color", "Blue");
                     ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);

                     for (int k = 0; k < htmlarraylist.Count; k++)
                     {
                         document.Add((iTextSharp.text.IElement)htmlarraylist[k]);

                     }
                     document.Close();
                     Response.Buffer = true;
                     var bytes = ms.ToArray();
                     Response.ContentType = "application/pdf";
                     Response.AddHeader("Content-Disposition", "attachment; filename=" + filefullpath);
                     Response.OutputStream.Write(bytes, 0, bytes.Length);
                     Response.Flush();
                     Response.End();
                 }
             }
         }
         catch (Exception ex)
         {
             ucError.ErrorMessage = ex.Message;
             ucError.Visible = true;
         }
     }
     public void ConvertToExcel(string HTMLString)
     {
         try
         {
             if (HTMLString != "")
             {
                 Response.Clear();
                 Response.Buffer = true;
                 Response.AddHeader("content-disposition", "attachment;filename=MonthlyReport.xls");
                 Response.Charset = "";
                 Response.ContentType = "application/vnd.ms-excel";
                 Response.Output.Write(HTMLString);
                 Response.Flush();
                 Response.End();
             }
         }
         catch (Exception ex)
         {
             ucError.ErrorMessage = ex.Message;
             ucError.Visible = true;
         }
     }

    protected void gvFuelOil_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            string monthlyreportid = ViewState["MONTHLYREPORTID"].ToString();

            DataSet ds = new DataSet();

            if (monthlyreportid == "")
                ds = PhoenixVesselPositionMonthlyReport.ListMonthlyReportOilConsumptionCalculation(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), General.GetNullableInteger(ViewState["MONTH"].ToString()), General.GetNullableInteger(ViewState["YEAR"].ToString()));
            else
                ds = PhoenixVesselPositionMonthlyReport.ListMonthlyReportOilConsumption(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                                                                                  General.GetNullableGuid(ViewState["MONTHLYREPORTID"].ToString()));

            dsOil = ds;

            gvFuelOil.DataSource = ds.Tables[0];

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLubOil_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            string monthlyreportid = ViewState["MONTHLYREPORTID"].ToString();

            DataSet ds = new DataSet();

            if (monthlyreportid == "")
                ds = PhoenixVesselPositionMonthlyReport.ListMonthlyReportOilConsumptionCalculation(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), General.GetNullableInteger(ViewState["MONTH"].ToString()), General.GetNullableInteger(ViewState["YEAR"].ToString()));
            else
                ds = PhoenixVesselPositionMonthlyReport.ListMonthlyReportOilConsumption(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                                                                                  General.GetNullableGuid(ViewState["MONTHLYREPORTID"].ToString()));

            dsOil = ds;

            gvLubOil.DataSource = ds.Tables[1];

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWater_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            string monthlyreportid = ViewState["MONTHLYREPORTID"].ToString();

            DataSet ds = new DataSet();

            if (monthlyreportid == "")
                ds = PhoenixVesselPositionMonthlyReport.ListMonthlyReportOilConsumptionCalculation(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), General.GetNullableInteger(ViewState["MONTH"].ToString()), General.GetNullableInteger(ViewState["YEAR"].ToString()));
            else
                ds = PhoenixVesselPositionMonthlyReport.ListMonthlyReportOilConsumption(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()),
                                                                                  General.GetNullableGuid(ViewState["MONTHLYREPORTID"].ToString()));
            dsOil = ds;    
            gvWater.DataSource = ds.Tables[2];
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RebindgvWater()
    {
        gvWater.SelectedIndexes.Clear();
        gvWater.EditIndexes.Clear();
        gvWater.DataSource = null;
        gvWater.Rebind();
    }
    protected void RebindgvLubOil()
    {
        gvLubOil.SelectedIndexes.Clear();
        gvLubOil.EditIndexes.Clear();
        gvLubOil.DataSource = null;
        gvLubOil.Rebind();
    }
    protected void RebindgvFuelOil()
    {
        gvFuelOil.SelectedIndexes.Clear();
        gvFuelOil.EditIndexes.Clear();
        gvFuelOil.DataSource = null;
        gvFuelOil.Rebind();
    }
}
