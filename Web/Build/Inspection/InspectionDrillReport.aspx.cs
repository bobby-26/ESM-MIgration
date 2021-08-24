using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class Registers_DrillReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar menu = new PhoenixToolbar();
        menu.AddButton("Drill Planner & Record", "Toggle4", ToolBarDirection.Right);
        menu.AddButton("History", "Toggle3", ToolBarDirection.Right);
        menu.AddButton("Drill Schedule", "Toggle2", ToolBarDirection.Right);
  
        

        Tabstripdrillreport.MenuList = menu.Show();

        Tabstripdrillreport.SelectedMenuIndex = 0;

        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
          
            ViewState["PAGENUMBER"]     = 1;

            DataTable   dt              = PhoenixInspectionDrillLog.year();
            radcomboyear.DataSource     = dt;
            radcomboyear.DataTextField  = "FLDYEAR";
            radcomboyear.DataValueField = "FLDYEAR";
            radcomboyear.DataBind();

            
            int currentYear = DateTime.Now.Year;

            int result = 0;

            PhoenixInspectionDrillLog.result(currentYear, ref result);

            if (result == 1)
            {
                radcomboyear.SelectedValue = currentYear.ToString();
            }

            int currentvesselid = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;

            ViewState["CURRENTVESSELID"] = currentvesselid.ToString();
            if (currentvesselid == 0)
            {

                int? vesselid = General.GetNullableInteger(Request.QueryString["vesselid"]);
                if (vesselid == null)
                {
                    ViewState["VESSELID"] = 0;
                }
                else
                {
                    ViewState["VESSELID"] = vesselid;
                    ddlvessellist.SelectedVessel = vesselid.ToString();
                }
            }
            else
            {
                ViewState["VESSELID"] = currentvesselid.ToString();
                ddlvessellist.SelectedVessel = currentvesselid.ToString();
                ddlvessellist.Enabled = false;
            }


        }
        if (General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString()) != null)
        { ViewState["VESSELID"] = ddlvessellist.SelectedVessel; }
        
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvMandatoryDrillReport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillReport.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionDrillReport.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=MANDATORYDRILL&vid=" + ViewState["VESSELID"].ToString() + "&y=" + radcomboyear.SelectedValue + "&showmenu=0&showword=NO&showexcel=NO'); return false;", "Report", "<i class=\"fas fa-chart-bar\"></i>", "REPORTS");

        Tabstripdrillreportmenu.MenuList = toolbargrid.Show();

        PhoenixToolbar toolbargrid1 = new PhoenixToolbar();
        toolbargrid1.AddFontAwesomeButton("../Inspection/InspectionDrillReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid1.AddFontAwesomeButton("javascript:CallPrint('gvCompanySpecifiedDrillReport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid1.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=COMPANYSPECIFIED&vid=" + ViewState["VESSELID"].ToString() + "&y=" + radcomboyear.SelectedValue + "&showmenu=0&showword=NO&showexcel=NO'); return false;", "Report", "<i class=\"fas fa-chart-bar\"></i>", "REPORTS");

        Tabstripdrillreportmenu_1.MenuList = toolbargrid1.Show();

    }

    protected void drillreportmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce         = (RadToolBarEventArgs)e;
            string              CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
                
                int? year = General.GetNullableInteger(radcomboyear.SelectedValue);

                if (!IsValidDrillReportDetails(vesselid, year))
                {
                    ucError.Visible = true;
                    return;
                }

                if (ddlvessellist.SelectedVessel != null)
                {

                    ViewState["VESSELID"] = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
                }
                gvMandatoryDrillReport.Rebind();
                gvCompanySpecifiedDrillReport.Rebind();

                
            } 
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlvessellist.SelectedVessel    = string.Empty;
                radcomboyear.ClearSelection();
                radcomboyear.Text               = string.Empty;
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage    = ex.Message;
            ucError.Visible         = true;

        }

    }

    protected void drillreportmenu1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce         = (RadToolBarEventArgs)e;
            string              CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel_1();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage    = ex.Message;
            ucError.Visible         = true;

        }

    }

    protected void drillreport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs         dce         = (RadToolBarEventArgs)e;
            string                      CommandName = ((RadToolBarButton)dce.Item).CommandName;


           
            if (CommandName.ToUpper().Equals("TOGGLE2"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionDrillSchedule.aspx?vesselid=" + vesselid);
                }
                else 
                Response.Redirect("../Inspection/InspectionDrillSchedule.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE3"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionDrillHistory.aspx?vesselid=" + vesselid);
                }
                else 
                Response.Redirect("../Inspection/InspectionDrillHistory.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE4"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionDrillReport.aspx?vesselid=" + vesselid);
                }
                else 
                Response.Redirect("../Inspection/InspectionDrillReport.aspx");

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage    = ex.Message;
            ucError.Visible         = true;

        }
    }

    

    protected void gvMandatoryDrillReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int?        vesselid        = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
        string      vesselname      = string.Empty;
        int?        year            = General.GetNullableInteger(radcomboyear.SelectedValue);

        DataTable   dt              = PhoenixInspectionDrillLog.drillmandatoryreport(vesselid, year, ref vesselname);
        gvMandatoryDrillReport.DataSource = dt;
        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDDRILLNAME", "FLDSCENARIO", "FLDINTERVAL", "FLDJAN", "FLDFEB", "FLDMAR", "FLDAPR", "FLDMAY", "FLDJUN", "FLDJUL", "FLDAUG", "FLDSEP", "FLDOCT", "FLDNOV", "FLDDEC" };
        string[] alCaptions = { "Mandatory Drills/ Exercise","Scenario", "Interval", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        General.SetPrintOptions("gvMandatoryDrillReport", " Drill Log", alCaptions, alColumns, ds);

    }


    protected void gvMandatoryDrillReport_ItemDataBound(object sender, GridItemEventArgs e)
    {

        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem    item            = (GridDataItem)e.Item;
                RadLabel        drillidentry       = (RadLabel)item.FindControl("Radlbldrillid");
                RadLabel        scenariolabel   = (RadLabel)item.FindControl("RadlblScenario");
                string          scenario        = scenariolabel.Text;
                string          drillid           = drillidentry.Text;
                int?            vesselid        = General.GetNullableInteger(ddlvessellist.SelectedVessel);
                int?            year            = General.GetNullableInteger(radcomboyear.SelectedValue);

                HtmlAnchor jan = (HtmlAnchor)item.FindControl("Jananchor");
                jan.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=1" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor feb = (HtmlAnchor)item.FindControl("Febanchor");
                feb.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=2" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor mar = (HtmlAnchor)item.FindControl("Maranchor");
                mar.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=3" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor apr = (HtmlAnchor)item.FindControl("Apranchor");
                apr.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=4" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor may = (HtmlAnchor)item.FindControl("Mayanchor");
                may.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=5" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor jun = (HtmlAnchor)item.FindControl("Junanchor");
                jun.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=6" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor jul = (HtmlAnchor)item.FindControl("Julanchor");
                jul.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=7" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor aug = (HtmlAnchor)item.FindControl("Auganchor");
                aug.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=8" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");
                
                HtmlAnchor sep = (HtmlAnchor)item.FindControl("Sepanchor");
                sep.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=9" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor oct = (HtmlAnchor)item.FindControl("Octanchor");
                oct.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=10" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor nov = (HtmlAnchor)item.FindControl("Novanchor");
                nov.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=11" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor dec = (HtmlAnchor)item.FindControl("Decanchor");
                dec.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=12" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCompanySpecifiedDrillReport_ItemDataBound(object sender, GridItemEventArgs e)
    {

        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel drillidentry = (RadLabel)item.FindControl("Radlbldrillid1");
                RadLabel scenariolabel = (RadLabel)item.FindControl("RadlblScenario1");
                string scenario = scenariolabel.Text;
                string drillid = drillidentry.Text;
                int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel);
                int? year = General.GetNullableInteger(radcomboyear.SelectedValue);

                HtmlAnchor jan = (HtmlAnchor)item.FindControl("Jananchor1");
                jan.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=1" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor feb = (HtmlAnchor)item.FindControl("Febanchor1");
                feb.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=2" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor mar = (HtmlAnchor)item.FindControl("Maranchor1");
                mar.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=3" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor apr = (HtmlAnchor)item.FindControl("Apranchor1");
                apr.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=4" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor may = (HtmlAnchor)item.FindControl("Mayanchor1");
                may.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=5" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor jun = (HtmlAnchor)item.FindControl("Junanchor1");
                jun.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=6" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor jul = (HtmlAnchor)item.FindControl("Julanchor1");
                jul.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=7" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor aug = (HtmlAnchor)item.FindControl("Auganchor1");
                aug.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=8" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor sep = (HtmlAnchor)item.FindControl("Sepanchor1");
                sep.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=9" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor oct = (HtmlAnchor)item.FindControl("Octanchor1");
                oct.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=10" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor nov = (HtmlAnchor)item.FindControl("Novanchor1");
                nov.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=11" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor dec = (HtmlAnchor)item.FindControl("Decanchor1");
                dec.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Drill Report','Inspection/InspectionDrillIndividualReport.aspx?year=" + year + "&month=12" + "&vessel=" + vesselid + "&drillid=" + drillid + "&scenario=" + scenario + "','large');return false");

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCompanySpecifiedDrillReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int?        vesselid    = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
        int?        year        = General.GetNullableInteger(radcomboyear.SelectedValue);
        string      vesselname  = string.Empty;
        DataTable   dt          = PhoenixInspectionDrillLog.drillcompanyspecifiedreport(vesselid, year, ref vesselname);

        gvCompanySpecifiedDrillReport.DataSource = dt;
        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDDRILLNAME", "FLDSCENARIO", "FLDINTERVAL", "FLDJAN", "FLDFEB", "FLDMAR", "FLDAPR", "FLDMAY", "FLDJUN", "FLDJUL", "FLDAUG", "FLDSEP", "FLDOCT", "FLDNOV", "FLDDEC" };
        string[] alCaptions = { "Company Specified Drills/ Exercise","Scenario", "Interval", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        General.SetPrintOptions("gvCompanySpecifiedDrillReport", "Drill Log", alCaptions, alColumns, ds);

    }

    protected void ShowExcel()
    {

        string[]    alColumns       = { "FLDDRILLNAME", "FLDSCENARIO", "FLDINTERVAL", "FLDJAN", "FLDFEB", "FLDMAR", "FLDAPR", "FLDMAY", "FLDJUN", "FLDJUL", "FLDAUG", "FLDSEP", "FLDOCT", "FLDNOV", "FLDDEC" };
        string[]    alCaptions      = { "Mandatory Drills/ Exercise","Scenario", "Interval", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        string      vesselname      = string.Empty;
        int?        vesselid        = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
        int?        year            = General.GetNullableInteger(radcomboyear.SelectedValue);

        DataTable dt = PhoenixInspectionDrillLog.drillmandatoryreport(vesselid, year,
                                               ref vesselname);
        Response.AddHeader("Content-Disposition", "attachment; filename=Mandatorydillsreport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 1).ToString() + "'><h3>EMERGENCY AND CONTINGENCY DRILL PLANNING AND RECORD</h3></td>");
        Response.Write("</tr>");
       
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void ShowExcel_1()
    {

        string[]    alColumns   = { "FLDDRILLNAME", "FLDSCENARIO", "FLDINTERVAL", "FLDJAN", "FLDFEB", "FLDMAR", "FLDAPR", "FLDMAY", "FLDJUN", "FLDJUL", "FLDAUG", "FLDSEP", "FLDOCT", "FLDNOV", "FLDDEC" };
        string[]    alCaptions  = { "Company Specified Drills/ Exercise","Scenario", "Interval", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        string      vesselname  = string.Empty;
        int?        vesselid    = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
        int?        year        = General.GetNullableInteger(radcomboyear.SelectedValue);
        DataTable dt = PhoenixInspectionDrillLog.drillcompanyspecifiedreport(vesselid, year,
                                               ref vesselname);


        Response.AddHeader("Content-Disposition", "attachment; filename=CompanySpecifieddillsreport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 1).ToString() + "'><h3>EMERGENCY AND CONTINGENCY DRILL PLANNING AND RECORD</h3></td>");

        Response.Write("</tr>");
   
   
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }


    private bool IsValidDrillReportDetails(int? vesselid, int? year)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if ( year == null)
        {
            ucError.ErrorMessage = " Year of the Drill Log.";
        }
        if (vesselid == null )
        {
            ucError.ErrorMessage = " Vessel Name.";
        }
        return (!ucError.IsError);
    }

}
