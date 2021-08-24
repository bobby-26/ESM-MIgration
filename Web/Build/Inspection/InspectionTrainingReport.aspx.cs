using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class Inspection_InspectionTrainingReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {


        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar menu = new PhoenixToolbar();
        menu.AddButton("Training Planner & Record", "Toggle4", ToolBarDirection.Right);
        menu.AddButton("History", "Toggle3", ToolBarDirection.Right);
        menu.AddButton("Training Schedule", "Toggle2", ToolBarDirection.Right);



        TabstripTrainingreport.MenuList = menu.Show();

        TabstripTrainingreport.SelectedMenuIndex = 0;

        if (!IsPostBack)
        {
            

            DataTable dt = PhoenixInspectionTrainingLog.year();
            radcomboyear.DataSource = dt;
            radcomboyear.DataTextField = "FLDYEAR";
            radcomboyear.DataValueField = "FLDYEAR";
            radcomboyear.DataBind();

    

            int currentvesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            int currentYear = DateTime.Now.Year;

            int result = 0;

            PhoenixInspectionTrainingLog.result(currentYear, ref result);

            if (result == 1)
            {
                radcomboyear.SelectedValue = currentYear.ToString();
            }

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
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionTrainingReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvMandatoryTrainingReport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionTrainingReport.aspx", "Search", "<i class=\"fa fa-search\"></i>", "SEARCH");
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionTrainingReport.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=MANDATORYTRAINING&vid=" + ViewState["VESSELID"].ToString() + "&y=" + radcomboyear.SelectedValue+ "&showmenu=0&showword=NO&showexcel=NO'); return false;", "Report", "<i class=\"fas fa-chart-bar\"></i>", "REPORTS");

        TabstripTrainingreportmenu.MenuList = toolbargrid.Show();


        PhoenixToolbar toolbargrid1 = new PhoenixToolbar();
        toolbargrid1.AddFontAwesomeButton("../Inspection/InspectionTrainingReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid1.AddFontAwesomeButton("javascript:CallPrint('gvCompanySpecifiedTrainingReport')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbargrid1.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=COMPANYSPECIFIEDTRAINING&vid=" + ViewState["VESSELID"].ToString() + "&y=" + radcomboyear.SelectedValue+ "&showmenu=0&showword=NO&showexcel=NO'); return false;", "Report", "<i class=\"fas fa-chart-bar\"></i>", "REPORTS");

        TabstripTrainingreportmenu_1.MenuList = toolbargrid1.Show();
    }

    protected void Trainingreportmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());

                int? year = General.GetNullableInteger(radcomboyear.SelectedValue.ToString());

                if (!IsValidTrainingReportDetails(vesselid, year))
                {
                    ucError.Visible = true;
                    return;
                }

                if (ddlvessellist.SelectedVessel != null)
                {

                    ViewState["VESSELID"] = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
                }
                gvMandatoryTrainingReport.Rebind();
                gvCompanySpecifiedTrainingReport.Rebind();


            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlvessellist.SelectedVessel = string.Empty;
                
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    protected void Trainingreportmenu1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel_1();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    protected void Trainingreport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;



            if (CommandName.ToUpper().Equals("TOGGLE2"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionTrainingSchedule.aspx?vesselid=" + vesselid);
                }
                else
                    Response.Redirect("../Inspection/InspectionTrainingSchedule.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE3"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionTrainingHistory.aspx?vesselid=" + vesselid);
                }
                else
                    Response.Redirect("../Inspection/InspectionTrainingHistory.aspx");

            }
            if (CommandName.ToUpper().Equals("TOGGLE4"))
            {
                if (ViewState["VESSELID"] != null)
                {
                    string vesselid = ViewState["VESSELID"].ToString();

                    Response.Redirect("~/Inspection/InspectionTrainingReport.aspx?vesselid=" + vesselid);
                }
                else
                    Response.Redirect("../Inspection/InspectionTrainingReport.aspx");

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }



    protected void gvMandatoryTrainingReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
        string vesselname = string.Empty;
        int? year = General.GetNullableInteger(radcomboyear.SelectedValue.ToString());

        DataTable dt = PhoenixInspectionTrainingLog.TrainingMandatoryReport(vesselid, year, ref vesselname);
        gvMandatoryTrainingReport.DataSource = dt;
        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDTRAININGNAME", "FLDSCENARIO", "FLDINTERVAL", "FLDJAN", "FLDFEB", "FLDMAR", "FLDAPR", "FLDMAY", "FLDJUN", "FLDJUL", "FLDAUG", "FLDSEP", "FLDOCT", "FLDNOV", "FLDDEC" };
        string[] alCaptions = { "Mandatory Trainings/ Exercise", "Scenario", "Interval", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        General.SetPrintOptions("gvMandatoryTrainingReport", " Training Log", alCaptions, alColumns, ds);

    }


    protected void gvMandatoryTrainingReport_ItemDataBound(object sender, GridItemEventArgs e)
    {

        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel Trainingidentry = (RadLabel)item.FindControl("Radlbltrainingid");
                RadLabel scenariolabel = (RadLabel)item.FindControl("RadlblScenario");
                string scenario = scenariolabel.Text;
                string Training = Trainingidentry.Text;
                int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel);
                int? year = General.GetNullableInteger(radcomboyear.SelectedValue.ToString());

                HtmlAnchor jan = (HtmlAnchor)item.FindControl("Jananchor");
                jan.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=1" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor feb = (HtmlAnchor)item.FindControl("Febanchor");
                feb.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=2" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor mar = (HtmlAnchor)item.FindControl("Maranchor");
                mar.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=3" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor apr = (HtmlAnchor)item.FindControl("Apranchor");
                apr.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=4" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor may = (HtmlAnchor)item.FindControl("Mayanchor");
                may.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=5" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor jun = (HtmlAnchor)item.FindControl("Junanchor");
                jun.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=6" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor jul = (HtmlAnchor)item.FindControl("Julanchor");
                jul.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=7" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor aug = (HtmlAnchor)item.FindControl("Auganchor");
                aug.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=8" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor sep = (HtmlAnchor)item.FindControl("Sepanchor");
                sep.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=9" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor oct = (HtmlAnchor)item.FindControl("Octanchor");
                oct.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=10" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor nov = (HtmlAnchor)item.FindControl("Novanchor");
                nov.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=11" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor dec = (HtmlAnchor)item.FindControl("Decanchor");
                dec.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=12" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCompanySpecifiedTrainingReport_ItemDataBound(object sender, GridItemEventArgs e)
    {

        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                RadLabel Trainingidentry = (RadLabel)item.FindControl("Radlbltrainingid1");
                RadLabel scenariolabel = (RadLabel)item.FindControl("RadlblScenario1");
                string scenario = scenariolabel.Text;
                string Training = Trainingidentry.Text;
                int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel);
                int? year = General.GetNullableInteger(radcomboyear.SelectedValue.ToString());

                HtmlAnchor jan = (HtmlAnchor)item.FindControl("Jananchor1");
                jan.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=1" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor feb = (HtmlAnchor)item.FindControl("Febanchor1");
                feb.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=2" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor mar = (HtmlAnchor)item.FindControl("Maranchor1");
                mar.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=3" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor apr = (HtmlAnchor)item.FindControl("Apranchor1");
                apr.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=4" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor may = (HtmlAnchor)item.FindControl("Mayanchor1");
                may.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=5" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor jun = (HtmlAnchor)item.FindControl("Junanchor1");
                jun.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=6" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor jul = (HtmlAnchor)item.FindControl("Julanchor1");
                jul.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=7" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor aug = (HtmlAnchor)item.FindControl("Auganchor1");
                aug.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=8" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor sep = (HtmlAnchor)item.FindControl("Sepanchor1");
                sep.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=9" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor oct = (HtmlAnchor)item.FindControl("Octanchor1");
                oct.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=10" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor nov = (HtmlAnchor)item.FindControl("Novanchor1");
                nov.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=11" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

                HtmlAnchor dec = (HtmlAnchor)item.FindControl("Decanchor1");
                dec.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Individual Training Report','Inspection/InspectionTrainingIndividualReport.aspx?year=" + year + "&month=12" + "&vessel=" + vesselid + "&Training=" + Training + "&scenario=" + scenario + "','large');return false");

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCompanySpecifiedTrainingReport_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
        int? year = General.GetNullableInteger(radcomboyear.SelectedValue.ToString());
        string vesselname = string.Empty;
        DataTable dt = PhoenixInspectionTrainingLog.TrainingCompanySpecifiedReport(vesselid, year, ref vesselname);

        gvCompanySpecifiedTrainingReport.DataSource = dt;
        DataSet ds = dt.DataSet;
        string[] alColumns = { "FLDTRAININGNAME", "FLDSCENARIO", "FLDINTERVAL", "FLDJAN", "FLDFEB", "FLDMAR", "FLDAPR", "FLDMAY", "FLDJUN", "FLDJUL", "FLDAUG", "FLDSEP", "FLDOCT", "FLDNOV", "FLDDEC" };
        string[] alCaptions = { "Company Specified Trainings/ Exercise", "Scenario", "Interval", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        General.SetPrintOptions("gvCompanySpecifiedTrainingReport", "Training Log", alCaptions, alColumns, ds);

    }
    protected void ShowExcel_1()
    {
        int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
        int? year = General.GetNullableInteger(radcomboyear.SelectedValue.ToString());
        string vesselname = string.Empty;
        DataTable dt = PhoenixInspectionTrainingLog.TrainingCompanySpecifiedReport(vesselid, year, ref vesselname);

        string[] alColumns = { "FLDTRAININGNAME", "FLDSCENARIO", "FLDINTERVAL", "FLDJAN", "FLDFEB", "FLDMAR", "FLDAPR", "FLDMAY", "FLDJUN", "FLDJUL", "FLDAUG", "FLDSEP", "FLDOCT", "FLDNOV", "FLDDEC" };
        string[] alCaptions = { "Company Specified Trainings/ Exercise", "Scenario", "Interval", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        General.ShowExcel( "Company Specified Training Log",dt, alCaptions, alColumns, null,null);
    }
    protected void ShowExcel()
    {
        int? vesselid = General.GetNullableInteger(ddlvessellist.SelectedVessel.ToString());
        string vesselname = string.Empty;
        int? year = General.GetNullableInteger(radcomboyear.SelectedValue.ToString());

        DataTable dt = PhoenixInspectionTrainingLog.TrainingMandatoryReport(vesselid, year, ref vesselname);
       
        string[] alColumns = { "FLDTRAININGNAME", "FLDSCENARIO", "FLDINTERVAL", "FLDJAN", "FLDFEB", "FLDMAR", "FLDAPR", "FLDMAY", "FLDJUN", "FLDJUL", "FLDAUG", "FLDSEP", "FLDOCT", "FLDNOV", "FLDDEC" };
        string[] alCaptions = { "Mandatory Trainings/ Exercise", "Scenario", "Interval", "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
        General.ShowExcel("Mandatory Training Log", dt, alCaptions, alColumns, null, null);
    }

    private bool IsValidTrainingReportDetails(int? vesselid, int? year)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if ( year == null)
        {
            ucError.ErrorMessage = " Year of the Training Log.";
        }
        if (vesselid == null )
        {
            ucError.ErrorMessage = " Vessel Name.";
        }
        return (!ucError.IsError);
    }
}