using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportsCrewJoinningFeedBack : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbarmain.AddButton("Feedback Analysis", "FEEDBACKANALYSIS", ToolBarDirection.Right);
            toolbarmain.AddButton("Crew Feedback", "CREWFEEDBACK", ToolBarDirection.Right);

            MenuCrewFeedback.AccessRights = this.ViewState;
            MenuCrewFeedback.MenuList = toolbarmain.Show();
            MenuCrewFeedback.SelectedMenuIndex = 1;

            toolbarsub.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReport.AccessRights = this.ViewState;
            MenuReport.MenuList = toolbarsub.Show();

            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportsCrewJoinningFeedBack.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewReportsCrewJoinningFeedBack.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCrew.SelectedIndexes.Clear();
        gvCrew.EditIndexes.Clear();
        gvCrew.DataSource = null;
        gvCrew.Rebind();
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
                NameValueCollection criteria = Filter.CurrentFeedBackFilter;
                if (criteria != null)
                {
                    criteria.Set("ddlFeedbackStatus", "");
                    criteria.Set("ucManager", "");
                    criteria.Set("ucFromDate", "");
                    criteria.Set("ucToDate", "");
                    criteria.Set("ucVessel", "");
                    criteria.Set("lstPool", "");
                    criteria.Set("ucVesselType", "");
                    criteria.Set("ucRank", "");
                    criteria.Set("ucZone", "");
                }
                ViewState["SHOWREPORT"] = null;
                ucFromDate.Text = null;
                ucToDate.Text = null;
                ucZone.SelectedZoneValue = "";
                lstPool.SelectedPool = "";
                ucVesselType.SelectedVesselTypeValue = "";
                ucVessel.SelectedVessel = "";
                ucRank.SelectedRankValue = "";
                ucManager.SelectedAddress = "";
                ddlFeedbackStatus.SelectedValue = "";
                Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Report_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();
                    criteria.Clear();
                    criteria.Add("ddlFeedbackStatus", ddlFeedbackStatus.SelectedValue);
                    criteria.Add("ucManager", ucManager.SelectedAddress);
                    criteria.Add("ucFromDate", ucFromDate.Text);
                    criteria.Add("ucToDate", ucToDate.Text);
                    criteria.Add("ucVessel", ucVessel.SelectedVessel);
                    criteria.Add("lstPool", lstPool.SelectedPool);
                    criteria.Add("ucVesselType", ucVesselType.SelectedVesselTypeValue);
                    criteria.Add("ucRank", ucRank.selectedlist);
                    criteria.Add("ucZone", ucZone.selectedlist);
                    Filter.CurrentFeedBackFilter = criteria;
                    ViewState["PAGENUMBER"] = 1;
                    Rebind();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCrewFeedback_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FEEDBACKANALYSIS"))
            {
                Response.Redirect("../Crew/CrewReportsCrewJoinningFBAnalysis.aspx?VesselList=" + General.GetNullableString(ucVessel.SelectedVessel), false);
                MenuCrewFeedback.SelectedMenuIndex = 0;
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
        string[] alColumns = { "FLDROWNUMBER", "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDFEEDBACKYNNAME", "FLDSIGNONDATE" };
        string[] alCaptions = { "Serial No.", "File No", "Employee Name", "Rank", "Feedback Vessel", "Feedback", "SignOn Date" };

        string[] FilterColumns = { "FLDSELECTEDVESSEL", "FLDSELECTEDVESSELTYPE", "FLDSELECTEDMANAGER", "FLDSELECTEDRANK", "FLDSELECTEDPOOL", "FLDSELECTEDZONE", "FLDSELECTEDSTATUS", "FLDFROMDATE", "FLDTODATE" };
        string[] FilterCaptions = { "Vessel List", "Vessel Type", "Manager", "Rank", "Pool", "Zone", "Feedback Status", "FromDate", "ToDate" };

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
        ucVessel.SelectedVessel = ucVessel.SelectedVessel.ToString().Contains("Dummy,") ? ucVessel.SelectedVessel.ToString().Replace("Dummy,", "") : ucVessel.SelectedVessel;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
        lstPool.SelectedPool = lstPool.SelectedPool.ToString().Contains("Dummy,") ? lstPool.SelectedPool.ToString().Replace("Dummy,", "") : lstPool.SelectedPool;

        ds = PhoenixVesselAccountsCrewFeedback.SearchCrewJoinningFeedBackSummary(
                       PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                       (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                       (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                       (General.GetNullableInteger(ucManager.SelectedAddress)),
                       (lstPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(lstPool.SelectedPool),
                       (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                       (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                        General.GetNullableDateTime(ucFromDate.Text),
                        General.GetNullableDateTime(ucToDate.Text),
                        General.GetNullableInteger(ddlFeedbackStatus.SelectedValue),
                       sortexpression, sortdirection,
                       Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                       General.ShowRecords(null),
                       ref iRowCount,
                       ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=Seafarer_Joining_Feedback_List.xls");
        Response.ContentType = "application/msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Seafarer Joining Feedback List</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        General.ShowFilterCriteriaInExcel(ds, FilterCaptions, FilterColumns);
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td align='left'>");
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

        string[] alColumns = { "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDVESSELNAME", "FLDSIGNONDATE" };
        string[] alCaptions = { "File No", "Employee Name", "Rank", "FeedBack Vessel", "SignOn Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucVessel.SelectedVessel = ucVessel.SelectedVessel.ToString().Contains("Dummy,") ? ucVessel.SelectedVessel.ToString().Replace("Dummy,", "") : ucVessel.SelectedVessel;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
        lstPool.SelectedPool = lstPool.SelectedPool.ToString().Contains("Dummy,") ? lstPool.SelectedPool.ToString().Replace("Dummy,", "") : lstPool.SelectedPool;

        DataSet ds = new DataSet();

        ds = PhoenixVesselAccountsCrewFeedback.SearchCrewJoinningFeedBackSummary(
                     PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                     (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                     (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                     (General.GetNullableInteger(ucManager.SelectedAddress)),
                     (lstPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(lstPool.SelectedPool),
                     (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                     (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                      General.GetNullableDateTime(ucFromDate.Text),
                      General.GetNullableDateTime(ucToDate.Text),
                      General.GetNullableInteger(ddlFeedbackStatus.SelectedValue),
                     sortexpression, sortdirection,
                     Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                     gvCrew.PageSize,
                     ref iRowCount,
                     ref iTotalPageCount);

        General.SetPrintOptions("gvCrew", "Crew Short Contract", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

           
                LinkButton imgRep = (LinkButton)e.Item.FindControl("cmdShowReport");

                if (imgRep != null)
                {
                if(drv["FLDFEEDBACKYN"].ToString()!="1")
                {
                    imgRep.Visible = false;
                }
                    
                    string employeeid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
                    string signonoffid = ((RadLabel)e.Item.FindControl("lblSignOnOffId")).Text;
                    string vesselid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;

                    string prams = "";
                    prams += "&vesselid=" + vesselid;
                    prams += "&employeeid=" + employeeid;
                    prams += "&signonoffid=" + signonoffid;

                    imgRep.Attributes.Add("onclick", "javascript:openNewWindow('chml', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=7&reportcode=VESSELCREWFEEDBACK" + prams + "'); return false;");
                }
            }

    }
    protected void gvCrew_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
    }

    public bool IsValidFilter(string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ucVessel.SelectedVessel) == null)
        {
            ucError.ErrorMessage = "Vessel required";
        }
        if (General.GetNullableDateTime(fromdate) != null && General.GetNullableDateTime(todate) != null)
        {
            if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
                ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        return (!ucError.IsError);

    }
}
