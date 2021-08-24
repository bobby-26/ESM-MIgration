using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportsSeniorOfficerAnalysisReport : PhoenixBasePage
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
            
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsSeniorOfficerAnalysisReport.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsSeniorOfficerAnalysisReport.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucDate.Text = DateTime.Now.ToShortDateString();
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

                ddlOilMajor.SelectedHard = "";
                ucPrincipal.SelectedAddress = "";
                ucVessel.SelectedVessel = "";
                ucVesselType.SelectedVesseltype = "";
                ddlContract.SelectedHard = "";
                ucDate.Text = DateTime.Now.ToShortDateString();
                ViewState["PAGENUMBER"] = 1;
                gvCrew.CurrentPageIndex = 0;
                
                gvCrew.SelectedIndexes.Clear();
                gvCrew.EditIndexes.Clear();
                gvCrew.DataSource = null;
                gvCrew.Rebind();             
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
                if (!IsValidFilter(ddlOilMajor.SelectedHard.ToString(), ddlContract.SelectedHard.ToString(), ucDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    gvCrew.CurrentPageIndex = 0;
                    ShowReport();
                    //  gvCrew.Rebind();                   
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();       

        string[] alColumns = { "FLDVESSELNAME", "FLDRANKCODE", "FLDNAME",  "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDTANKEREXP", "FLDRANKEXP", "FLDESMEXP", "FLDAGGREGATETANKEREXPINYEARS", "FLDAGGREGATERANKEXPINYEARS", "FLDAGGREGATEESMEXPINYEARS", "FLDCOMPLIESWITHTANKEREXP", "FLDCOMPLIESWITHRANKEXP", "FLDCOMPLIESWITHESMEXP", };
        string[] alCaptions = { "Vessel", "Rank", "Name",  "S/On Date", "Relief Due", "Time on", "Time in", "Time with", "Time on", "Time in", "Time with", "Time on", "Time in", "Time with" };
        string[] alCaptions1 = { "", "", "Name", "", "", "Tanker", "Rank", "Company", "Tanker", "Rank", "Company", "Tanker", "Rank", "Company" };
        string[] alCaptions2 = { "", "", "", "", "", "(months)", "(months)", "(months)", "(Agg", "(Agg", "(Agg", "(Complies", "(Complies", "(Complies" };
        string[] alCaptions4 = { "", "", "", "", "", "", "", "", "Years)", "Years)", "Years)", "with", "with", "with" };
        string[] alCaptions3 = { "", "", "", "", "", "", "", "", "", "", "", "Matrix)", "Matrix)", "Matrix)" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        if (!IsPostBack)
        {
            ds = PhoenixCrewSeniorOfficerAnalysis.CrewSeniorOfficerAnalysis(0, 0, null, null, null,null, sortexpression, sortdirection,
                   1,
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount,
                    null,null);
        }
        else
        {
            if (ddlOilMajor.SelectedHard.ToString().Equals("") && ddlContract.SelectedHard.ToString().Equals(""))
            {
                return;
            }
            else
            {
                ds = PhoenixCrewSeniorOfficerAnalysis.CrewSeniorOfficerAnalysis(
                         General.GetNullableInteger(ddlOilMajor.SelectedHard.ToString()),
                        General.GetNullableInteger(ddlContract.SelectedHard.ToString()),
                        (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                        (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                        (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString()), 
                        General.GetNullableString(lstPool.SelectedPool),sortexpression, sortdirection,1,
                        iRowCount, ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(rblOfficers.SelectedValue), General.GetNullableDateTime(ucDate.Text)
                        );
            }
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewOfficersOnboardExperienceMatrix.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Officers Onboard Experience Matrix</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='10%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions1.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='10%'>");
            Response.Write("<b>" + alCaptions1[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions2.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='10%'>");
            Response.Write("<b>" + alCaptions2[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions4.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='10%'>");
            Response.Write("<b>" + alCaptions4[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions3.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='10%'>");
            Response.Write("<b>" + alCaptions3[i] + "</b>");
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELNAME", "FLDRANKCODE", "FLDNAME", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDTANKEREXP", "FLDRANKEXP", "FLDESMEXP", "FLDAGGREGATETANKEREXPINYEARS", "FLDAGGREGATERANKEXPINYEARS", "FLDAGGREGATEESMEXPINYEARS", "FLDCOMPLIESWITHTANKEREXP", "FLDCOMPLIESWITHRANKEXP", "FLDCOMPLIESWITHESMEXP", };
        string[] alCaptions = { "Vessel", "Rank", "Name",  "S/On Date", "Relief Due", "Time on Tanker(months)", "Time in Rank(months)", "Time with Company(months)", "Time on Tanker(Aggregate Years)", "Time in Rank(Aggregate Years)", "Time with Company(Aggregate Years)", "Time on Tanker(Complies with Matrix)", "Time in Rank(Complies with Matrix)", "Time with Company(Complies with Matrix)" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (!IsPostBack)
        {
            ds = PhoenixCrewSeniorOfficerAnalysis.CrewSeniorOfficerAnalysis(null, null, null, null, null, null, sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                  gvCrew.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount, null, null);
        }
        else
        {
            if (ddlOilMajor.SelectedHard.ToString().Equals("") && ddlContract.SelectedHard.ToString().Equals(""))
            {
                return;
            }
            else
            {
                ds = PhoenixCrewSeniorOfficerAnalysis.CrewSeniorOfficerAnalysis(
                         General.GetNullableInteger(ddlOilMajor.SelectedHard.ToString()),
                         General.GetNullableInteger(ddlContract.SelectedHard.ToString()),
                        (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                        (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                        (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                         General.GetNullableString(lstPool.SelectedPool), sortexpression, sortdirection,
                         Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                         gvCrew.PageSize,
                         ref iRowCount,
                         ref iTotalPageCount,
                         General.GetNullableInteger(rblOfficers.SelectedValue),
                         General.GetNullableDateTime(ucDate.Text));
            }
        }
        General.SetPrintOptions("gvCrew", "Officers Onboard Experience Matrix", alCaptions, alColumns, ds);
        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;
       
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
   
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {        
        if (e.Item is GridDataItem)
        {   
                RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
                LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
                lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");           
        }
    }
    public bool IsValidFilter(string oilmajor, string contract, string asondate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (oilmajor.Equals("0") || oilmajor.Equals("Dummy") || oilmajor.Equals(""))
        {
            ucError.ErrorMessage = "Oil Major is Required";
        }
        if (contract.Equals("0") || contract.Equals("Dummy") || contract.Equals(""))
        {
            ucError.ErrorMessage = "Contract is required";
        }
        if (General.GetNullableDateTime(asondate) == null)
        {
            ucError.ErrorMessage = "As on Date is required.";
        }

        return (!ucError.IsError);
    }
   
    protected void gvCrew_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        ShowReport();
        gvCrew.Rebind();
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
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
