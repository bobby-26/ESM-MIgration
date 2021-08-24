using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using System.Text;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportsSelectionCriteria : System.Web.UI.Page
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
          
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsSelectionCriteria.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsSelectionCriteria.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = 0;
                gvCrew.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }            
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                //ViewState["SHOWREPORT"] = null;
                //ucDateTo.Text = DateTime.Now.ToShortDateString();
                //ucDateFrom.Text = "";
                //ucDateTo.Text = DateTime.Now.ToShortDateString();
                //ucBatchList.SelectedValue = "";
                //ucVesselType.SelectedVesselTypeValue = "";
                //ShowReport();
                //SetPageNavigator();

                Response.Redirect("../Crew/CrewReportsSelectionCriteria.aspx");
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {                
                gvCrew.CurrentPageIndex = 0;
                ViewState["SHOWREPORT"] = 1;                
                gvCrew.Rebind();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEECODE", "FLDBATCHNO", "FLDNAME", "FLDRANKPOSTEDNAME", "FLDPRESENTVESSELNAME", "FLDPRESENTSIGNONDATE", "FLDEXHANDYN", "FLDFULLCONTRACTYN", "FLDAGE", "FLDCOMPANYCHANGED", "FLDCOMBINEDEXP" };
        string[] alCaptions = { "Sl.No", "Employee Code", "Batch", "Name", "Rank", "Vessel OnBoard", "SignOn Date", "Exhand(Y/N)", "Full Contract(Y/N)", "Age", "No.of Companies", "Combined Rank Experience"};
        string[] filtercolumns = { "FLDROWNUMBER", "FLDEMPLOYEECODE", "FLDBATCHNO", "FLDNAME", "FLDRANKPOSTEDNAME", "FLDPRESENTVESSELNAME", "FLDPRESENTSIGNONDATE", "FLDEXHANDYN", "FLDFULLCONTRACTYN", "FLDAGE", "FLDCOMPANYCHANGED", "FLDCOMBINEDEXP" };
        string[] filtercaptions = { "Sl.No", "Employee Code", "Batch", "Name", "Rank", "Vessel OnBoard", "SignOn Date", "Exhand(Y/N)", "Full Contract(Y/N)", "Age", "No.of Companies", "Combined Rank Experience" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;

        ds = PhoenixCrewSelectionCriteria.CrewSelectionCriteriaSearch((ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                        , General.GetNullableInteger(ddlExhand.SelectedValue)
                                                                        , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                        , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                                                        , General.GetNullableInteger(ucPrinicipal.SelectedAddress)
                                                                        , General.GetNullableInteger(txtAgeF.Text)
                                                                        , General.GetNullableInteger(txtAgeT.Text)
                                                                        , General.GetNullableInteger(txtCmpyChangedF.Text)
                                                                        , General.GetNullableInteger(txtCmpyChangedT.Text)
                                                                        , General.GetNullableInteger(txtRExp3e4eF.Text)
                                                                        , General.GetNullableInteger(txtRExp3e4eT.Text)
                                                                        , General.GetNullableInteger(txtRExpmstcoF.Text)
                                                                        , General.GetNullableInteger(txtRExpmstcoT.Text)
                                                                        , General.GetNullableInteger(txtRExp2o3oF.Text)
                                                                        , General.GetNullableInteger(txtRExp2o3oT.Text)
                                                                        , General.GetNullableInteger(txtRExpce2eF.Text)
                                                                        , General.GetNullableInteger(txtRExpce2eT.Text)
                                                                        , 1
                                                                        , sortexpression, sortdirection
                                                                        , 1
                                                                        , iRowCount
                                                                        , ref iRowCount
                                                                        , ref iTotalPageCount
                                                                        , General.GetNullableDateTime(ucSignOnFrom.Text)
                                                                        , General.GetNullableDateTime(ucSignOnTo.Text)
                                                                        , General.GetNullableInteger(ddlRatingExp.SelectedValue)
                                                                        );

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewSelectionCriteria.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Staff Recruited Upto:"+date+"</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        //General.ShowFilterCriteriaInExcel(ds, filtercaptions, filtercolumns);
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void ShowReport()
    {
        //  ViewState["SHOWREPORT"] = 1;
            //divTab1.Visible = true;
            //divPage.Visible = true;
            //
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROWNUMBER", "FLDEMPLOYEECODE", "FLDBATCHNO", "FLDNAME", "FLDRANKPOSTEDNAME", "FLDPRESENTVESSELNAME", "FLDPRESENTSIGNONDATE", "FLDEXHANDYN", "FLDFULLCONTRACTYN", "FLDAGE", "FLDCOMPANYCHANGED", "FLDCOMBINEDEXP" };
        string[] alCaptions = { "Sl.No", "Employee Code", "Batch", "Name", "Rank", "Vessel OnBoard", "SignOn Date", "Exhand(Y/N)", "Full Contract(Y/N)", "Age", "No.of Companies", "Combined Rank Experience" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;

        ds = PhoenixCrewSelectionCriteria.CrewSelectionCriteriaSearch((ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist)
                                                                        , General.GetNullableInteger(ddlExhand.SelectedValue)
                                                                        , (ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool)
                                                                        , (ucVesselType.SelectedVesseltype) == "," ? null : General.GetNullableString(ucVesselType.SelectedVesseltype)
                                                                        , General.GetNullableInteger(ucPrinicipal.SelectedAddress)
                                                                        , General.GetNullableInteger(txtAgeF.Text)
                                                                        , General.GetNullableInteger(txtAgeT.Text)
                                                                        , General.GetNullableInteger(txtCmpyChangedF.Text)
                                                                        , General.GetNullableInteger(txtCmpyChangedT.Text)
                                                                        , General.GetNullableInteger(txtRExp3e4eF.Text)
                                                                        , General.GetNullableInteger(txtRExp3e4eT.Text)
                                                                        , General.GetNullableInteger(txtRExpmstcoF.Text)
                                                                        , General.GetNullableInteger(txtRExpmstcoT.Text)
                                                                        , General.GetNullableInteger(txtRExp2o3oF.Text)
                                                                        , General.GetNullableInteger(txtRExp2o3oT.Text)
                                                                        , General.GetNullableInteger(txtRExpce2eF.Text)
                                                                        , General.GetNullableInteger(txtRExpce2eT.Text)
                                                                        , General.GetNullableInteger(ViewState["SHOWREPORT"].ToString())
                                                                        , sortexpression, sortdirection
                                                                        , gvCrew.CurrentPageIndex+1
                                                                        , gvCrew.PageSize
                                                                        , ref iRowCount
                                                                        , ref iTotalPageCount
                                                                        , General.GetNullableDateTime(ucSignOnFrom.Text)
                                                                        , General.GetNullableDateTime(ucSignOnTo.Text)
                                                                        , General.GetNullableInteger(ddlRatingExp.SelectedValue)
                                                                        );

        General.SetPrintOptions("gvCrew", "Crew Selection Criteria", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpId");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('Crew','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
        }
    }
    public bool IsValidFilter(string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }

        return (!ucError.IsError);

    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("PAGE"))
                ViewState["PAGENUMBER"] = null;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {            
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
