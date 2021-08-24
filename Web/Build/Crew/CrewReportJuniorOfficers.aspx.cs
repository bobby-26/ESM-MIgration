using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Text;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportsJuniorOfficersReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT");
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddImageButton("../Crew/CrewReportJuniorOfficers.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar1.AddImageButton("../Crew/CrewReportJuniorOfficers.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();
           
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
            ShowReport();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ucPrincipal.SelectedAddress = "";
                ucVessel.SelectedVessel = "";
                ucVesselType.SelectedVesseltype = "";
                ddlMonthlist.SelectedHard = "";
                ddlYearlist.SelectedQuick = "";

                ShowReport();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ddlMonthlist.SelectedHard.ToString(),ddlYearlist.SelectedQuick.ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ShowReport();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();

        ds = PhoenixCrewReportsJuniorOfficerReport.CrewJuniorsOfficersReport((ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : ucPrincipal.SelectedAddress.ToString(),
                        (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                        (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                        General.GetNullableInteger(ddlMonthlist.SelectedHard),
                        General.GetNullableInteger(ddlYearlist.SelectedQuick));


        DataTable dttext = ds.Tables[0];

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt1 = ds.Tables[1];
                DataRow dr = ds.Tables[0].Rows[0];
                DataTable dtTempHeader = ds.Tables[0];
                DataRow[] drvHeader = dtTempHeader.Select("FLDROWID = " + dt1.Rows[0]["FLDROWID"].ToString());
                int alColumns = 1;
                foreach (DataRow drTempHeader in drvHeader)
                {
                    alColumns = alColumns + 2;
                }

                Response.AddHeader("Content-Disposition", "attachment; filename=CrewJuniorOfficersExperienceReport.xls");
                Response.ContentType = "application/vnd.msexcel";
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
                Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns).ToString() + "'><h5><center>Crew Junior Offiers Report</center></h5></td></tr>");
                Response.Write("<tr><td style='font-family:Arial; font-size:10px;' align='left'><b>Date:" + date + "</b></td></tr>");
                Response.Write("<tr><td colspan='" + (alColumns).ToString() + "'>&nbsp;</td>");
                Response.Write("</tr>");
                Response.Write("</TABLE>");
                Response.Write("<br />");
                Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
                Response.Write("<tr style='font-size:12px;'>");
                Response.Write("<td><b>Vessel Name</b></td>");
                
                foreach (DataRow drTempHeader in drvHeader)
                {
                    Response.Write("<td style='font-size:12px;' align='CENTER' colspan='2'><b>");
                    Response.Write(drTempHeader["FLDCOLUMNHEADER"].ToString());
                    Response.Write("</b></td>");
                }
                Response.Write("</tr><tr><td></td>");
                int counter = 0;
                foreach (DataRow drTempHeader in drvHeader)
                {
                    Response.Write("<td align='CENTER'><b>Rank Exp</b></td><td align='CENTER'><b>Tanker Exp</b></td>");
                    counter += 2;
                }
                Response.Write("</tr>");

                foreach (DataRow dr1 in dt1.Rows)
                {
                    DataTable dtTemp = ds.Tables[0];

                    DataRow[] drv = dtTemp.Select("FLDROWID = " + dr1["FLDROWID"].ToString());

                    Response.Write("<tr><td style='font-size:10px;' align='LEFT'>" + drv[0]["FLDROWHEADER"].ToString() + "</td>");

                    foreach (DataRow drTemp in drv)
                    {
                        Response.Write("<td style='font-size:10px;' align='LEFT'>");
                        Response.Write(drTemp["FLDRANKEXP"].ToString());
                        Response.Write("</td>");
                        Response.Write("<td style='font-size:10px;' align='LEFT'>");
                        Response.Write(drTemp["FLDTANKEREXP"].ToString());
                        Response.Write("</td>");
                    }
                    Response.Write("</tr>");
                }

                Response.Write("</table>");
                Response.Write("IP-Internal Promotion EP-External Promotion");
                Response.End();
            }
        }
    }
    private void ShowReport()
    {
        DataSet ds = new DataSet();

        ds = PhoenixCrewReportsJuniorOfficerReport.CrewJuniorsOfficersReport((ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : ucPrincipal.SelectedAddress.ToString(),
                        (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                        (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                        General.GetNullableInteger(ddlMonthlist.SelectedHard),
                        General.GetNullableInteger(ddlYearlist.SelectedQuick));

        //General.SetPrintOptions("gvCrew", "Crew List With Wages", alCaptions, alColumns, ds);

        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                DataTable dt1 = ds.Tables[1];
                sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;overflow:auto;width:100%;border-collapse:collapse;\"> ");

                DataTable dtTempHeader = ds.Tables[0];
               
                DataRow[] drvHeader = dtTempHeader.Select("FLDROWID = " + dt1.Rows[0]["FLDROWID"].ToString());
                sb.Append("<tr class=\"DataGrid-HeaderStyle\"><td>Vessel Name</td>");

                foreach (DataRow drTempHeader in drvHeader)
                {
                    sb.Append("<td colspan='2' align='CENTER'><b>");
                    sb.Append(drTempHeader["FLDCOLUMNHEADER"].ToString());
                    sb.Append("</b></td>");
                }

                sb.Append("</tr><tr><td></td>");
                int counter = 0;
                foreach (DataRow drTempHeader in drvHeader)
                {
                    sb.Append("<td align='CENTER'><b>Rank Exp</b></td><td align='CENTER'><b>Tkr Exp</b></td>");
                    counter += 2;
                }
                sb.Append("</tr>");

                //PRINTING
                foreach (DataRow dr1 in dt1.Rows)
                {
                    DataTable dtTemp = ds.Tables[0];

                    DataRow[] drv = dtTemp.Select("FLDROWID = " + dr1["FLDROWID"].ToString());

                    sb.Append("<td align='left'>" + drv[0]["FLDROWHEADER"].ToString() + "</td>");

                    foreach (DataRow drTemp in drv)
                    {
                        sb.Append("<td align='center'>");
                        sb.Append(drTemp["FLDRANKEXP"].ToString());
                        sb.Append("</td>");
                        sb.Append("<td align='center'>");
                        sb.Append(drTemp["FLDTANKEREXP"].ToString());
                        sb.Append("</td>");
                    }
                    sb.Append("</tr>");
                }
                sb.Append("</table>");
                sb.Append("<br/><table width='20%'>");
                sb.Append("<tr><td style='color:Green'>IP</td><td>Internal Promotion</td><td style='color:Red'>EP</td><td>External Promotion</td></tr></table>");
                

                ltGrid.Text = sb.ToString();

            }
            else
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("<table cellspacing=\"0\" cellpadding=\"3\" rules=\"all\" border=\"1\" style=\"font-size:11px;width:100%;border-collapse:collapse;\"> ");

                sb.Append("<tr style=\"height:10px;\" class=\"DataGrid-HeaderStyle\"><td style=\"height:15px;\"></td></tr>");
                sb.Append("<tr style=\"height:10px;\"><td align=\"center\" colspan=\"6\" style=\"color:Red;font-weight:bold;\">NO RECORDS FOUND</td></tr>");

                sb.Append("</table>");

                ltGrid.Text = sb.ToString();
            }
        }

    }
    public bool IsValidFilter(string month,string year)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //GridView _gridview = gvCrew;

        if (month.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Month is Required";
        }
        if (year.Equals("Dummy"))
        {
            ucError.ErrorMessage = "Year is Required";
        }
        return (!ucError.IsError);

    }
}