using System;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;


public partial class InspectionWeeklyReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageLink("../Inspection/InspectionWeeklyReport.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageButton("../Inspection/InspectionWeeklyReport.aspx", "Search", "search.png", "FIND");
            toolbar.AddImageButton("../Inspection/InspectionWeeklyReport.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuWeeklyReport.AccessRights = this.ViewState;
            MenuWeeklyReport.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                BindStatus();
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                }

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvc.Get("QMS");

                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuWeeklyReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        // Fleetwise Corrective Task 

        DataSet ds = PhoenixInspectionWeeklyReport.InspectionFleetWiseCorrectiveTaskWeeklyreport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                                 , General.GetNullableInteger(rblDueOverdue.SelectedValue)
                                                                                                 , General.GetNullableInteger(ddlPastDateRange.SelectedValue)
              
                                                                                                 , General.GetNullableInteger(ddlFutureDateRange.SelectedValue));

        gvshipboardtask.DataSource = ds;

        

        // Fleetwise Preventive Task 

        DataSet ds1 = PhoenixInspectionWeeklyReport.InspectionFleetWisePreventiveTaskWeeklyreport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                                 , General.GetNullableInteger(rblDueOverdue.SelectedValue)
                                                                                                 , General.GetNullableInteger(ddlPastDateRange.SelectedValue)
                                                                                                 , General.GetNullableInteger(ddlFutureDateRange.SelectedValue));

        gvpreventivetask.DataSource = ds1;
        // Department Wise Corrective Task

        DataSet ds2 = PhoenixInspectionWeeklyReport.InspectionDepartmentWiseOfficeTaskWeeklyreport(General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                                                    , General.GetNullableInteger(rblDueOverdue.SelectedValue));

        gvofficetask.DataSource = ds2;

        // Risk Assessment

        DataSet ds3 = PhoenixInspectionWeeklyReport.InspectionRiskAssessmentWeeklyreport(General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                                         ,General.GetNullableInteger(ddlStatus.SelectedValue)
                                                                                         ,General.GetNullableInteger(ddlPastDateRange.SelectedValue)
                                                                                         ,General.GetNullableInteger(ddlFutureDateRange.SelectedValue));

        gvRiskassessment.DataSource = ds3;
      
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        Response.AddHeader("Content-Disposition", "attachment; filename=WeeklyManagementReview_Report.xls");
        Response.ContentType = "application/vnd.msexcel";
        StringBuilder sb = new StringBuilder();
        string[] alColumns = { };
        string[] alCaptions = { };

        // Fleetwise Corrective Task 

        ds = PhoenixInspectionWeeklyReport.InspectionFleetWiseCorrectiveTaskWeeklyreport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                                 , General.GetNullableInteger(rblDueOverdue.SelectedValue)
                                                                                                 , General.GetNullableInteger(ddlPastDateRange.SelectedValue)
                                                                                                 , General.GetNullableInteger(ddlFutureDateRange.SelectedValue));

        alColumns = new string[] { "FLDFLEETNAME", "FLDDUE", "FLDOPENTASKCOUNT", "FLDCOMPLETEDCOUNT", "FLDTOTAL" };
        alCaptions = new string[] { "Fleet", "OVERDUE ", "OPEN", "COMPLETED", "TOTAL" };
        sb.Append(PrepareData("1) Fleet Wise Corrective Tasks", ds.Tables[0], alColumns, alCaptions));

        // Fleetwise Preventive Task 

        ds = PhoenixInspectionWeeklyReport.InspectionFleetWisePreventiveTaskWeeklyreport(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                                  , General.GetNullableInteger(rblDueOverdue.SelectedValue)
                                                                                                  , General.GetNullableInteger(ddlPastDateRange.SelectedValue)
                                                                                                  , General.GetNullableInteger(ddlFutureDateRange.SelectedValue));

        alColumns = new string[] { "FLDFLEETNAME", "FLDDUE", "FLDOPENTASKCOUNT", "FLDCOMPLETEDCOUNT", "FLDTOTAL" };
        alCaptions = new string[] { "Fleet", "OVERDUE ", "OPEN", "COMPLETED", "TOTAL" };
        sb.Append(PrepareData("2) Fleet Wise Preventive Tasks", ds.Tables[0], alColumns, alCaptions));

        // Department Wise Corrective Task

        ds = PhoenixInspectionWeeklyReport.InspectionDepartmentWiseOfficeTaskWeeklyreport(General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                                           , General.GetNullableInteger(rblDueOverdue.SelectedValue));

        alColumns = new string[] { "FLDDEPARTMENTNAME", "FLDTASKCOUNT"};
        alCaptions = new string[] { "Department", "Count" };
        sb.Append(PrepareData("3) Department Wise Office Tasks", ds.Tables[0], alColumns, alCaptions));

        // Risk Assessment

        ds = PhoenixInspectionWeeklyReport.InspectionRiskAssessmentWeeklyreport(General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                                         , General.GetNullableInteger(ddlStatus.SelectedValue)
                                                                                         , General.GetNullableInteger(ddlPastDateRange.SelectedValue)
                                                                                         , General.GetNullableInteger(ddlFutureDateRange.SelectedValue));

        alColumns = new string[] { "FLDRATYPENAME", "FLDRACOUNT"};
        alCaptions = new string[] { "Risk Assessment Type", "Count" };
        sb.Append(PrepareData("4) Risk Assessment", ds.Tables[0], alColumns, alCaptions));

        Response.Write(sb.ToString());
        Response.End();
    }

    protected StringBuilder PrepareData(string title, DataTable dt, string[] alColumns, string[] alCaptions)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        sb.Append("<tr>");
        sb.Append("<td colspan='" + (alColumns.Length - 2).ToString() + "'><h3>" + title + "</h3></td>");
        sb.Append("</tr>");
        sb.Append("</TABLE>");
        sb.Append("<br />");
        sb.Append("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        sb.Append("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            sb.Append("<td width='20%'>");
            sb.Append("<b>" + alCaptions[i] + "</b>");
            sb.Append("</td>");
        }
        sb.Append("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            sb.Append("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                sb.Append("<td>");
                sb.Append(dr[alColumns[i]]);
                sb.Append("</td>");

            }
            sb.Append("</tr>");
        }
        sb.Append("</TABLE>");
        return sb;
    }
    protected void gvRiskassessment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
    //    BindData();
    }
    protected void gvofficetask_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
    }
    protected void gvpreventivetask_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
    }
    protected void gvshipboardtask_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
    }
    protected void gvRiskassessment_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
    }
    protected void gvofficetask_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
    }
    protected void gvpreventivetask_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
    }
    protected void gvshipboardtask_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
    protected void ddlIncidentNearmiss_Changed(object sender, EventArgs e)
    {
        BindData();
    }
    private void BindStatus()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("FLDSTATUSID", typeof(string));
        dt.Columns.Add("FLDSTATUSNAME", typeof(string));
        dt.Rows.Add("4", "Pending approval");

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
        {
            dt.Rows.Add("5", "Approved for use, subject to completion of Tasks");
        }
        else
        {
            dt.Rows.Add("5", "Approved for use");
        }
        dt.Rows.Add("6", "Not Approved");
        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
        {
            dt.Rows.Add("7", "Approved for use");
        }
        else
        {
            dt.Rows.Add("7", "Completed");
        }

        ddlStatus.DataSource = dt;
        ddlStatus.DataTextField = "FLDSTATUSNAME";
        ddlStatus.DataValueField = "FLDSTATUSID";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        BindData();
    }
}
