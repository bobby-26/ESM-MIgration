using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PhoenixCrewQuickReports;
using System.Text;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewQuickReportCourse : PhoenixBasePage
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

            toolbar1.AddFontAwesomeButton("../Crew/CrewQuickReportCourse.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewQuickReportCourse.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                lstCourse.Items.Clear();
                RadListBoxItem items = new RadListBoxItem();
                lstCourse.DataSource = PhoenixRegistersDocumentCourse.ListPostSeaCourse(null);
                lstCourse.DataTextField = "FLDDOCUMENTNAME";
                lstCourse.DataValueField = "FLDDOCUMENTID";
                lstCourse.DataBind();
                BindInstitute();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SHOWREPORT"] = null;
                ucSignOnToDate.Text = DateTime.Now.ToShortDateString();
                gvCrew.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                
            }
            //ShowReport();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindInstitute()
    {
        lstInstitution.Items.Clear();
        RadListBoxItem items = new RadListBoxItem();
        lstInstitution.DataSource = PhoenixRegistersAddress.ListAddress("138");
        lstInstitution.DataTextField = "FLDNAME";
        lstInstitution.DataValueField = "FLDADDRESSCODE";
        lstInstitution.DataBind();
    }

    private string InstitutionSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstInstitution.Items)
        {
            if (item.Selected == true)
            {
                if (item.Value != "--Select--")
                {
                    strlist.Append(item.Value.ToString());
                }
                strlist.Append(",");
            }

        }
        return strlist.ToString().TrimEnd(',');
    }
    private string CourseSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstCourse.Items)
        {
            if (item.Selected == true && item.Value != "")
            {
                strlist.Append(item.Value.ToString());
                strlist.Append(",");
            }
        }
        return strlist.ToString().TrimEnd(',');
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
                ViewState["SHOWREPORT"] = null;
                ucDocumentType.SelectedHard = "";
                ucSignOnFromDate.Text = "";
                lstCourse.SelectedIndex = -1;
                ucSignOnToDate.Text = DateTime.Now.ToShortDateString();
                ShowReport();
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
                if (!IsValidFilter(ucSignOnFromDate.Text, ucSignOnToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string date = DateTime.Now.ToShortDateString();

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDROW", "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDBATCH", "FLDZONENAME", "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE", "FLDPRESENTVESSELNAME", "FLDPRESENTSIGNONDATE", "FLDLASTNOTESDESCRIPTION", "FLDDOA", "FLDLASTCONTACTDATE", "FLDSTATUS", "FLDSEAFARERREQUIREMENT", "FLDCOURSENAME", "FLDDATEOFISSUE", "FLDINSTITUTIONNAME", "FLDPLACEOFISSUE" };
            string[] alCaptions = { "SNo.", "File No.", "	Name", "Rank", "Batch", "Zone", "Last Vessel", "Sign-Off Date", "Present Vessel", "Sign-On Date", "Remarks", "Availability", "Last Contact", "Status", "Requirement", "Course Name", "Date of Issue", "Institution", "Place Of Issue" };
            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixCrewQuickReports.CrewQuickCourse(General.GetNullableString(txtname.Text.Trim())
                                                                , General.GetNullableString(txtFileNo.Text.Trim())
                                                                , General.GetNullableInteger(ucDocumentType.SelectedHard == null ? null : ucDocumentType.SelectedHard)
                                                                , General.GetNullableString(CourseSelectedList())
                                                                , General.GetNullableString(InstitutionSelectedList())
                                                                , General.GetNullableDateTime(ucSignOnFromDate.Text)
                                                                , General.GetNullableDateTime(ucSignOnToDate.Text)
                                                                , 1
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=CourseQuickReport.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
            Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Course</center></h5></td></tr>");
            Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
            Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowReport()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDROW", "FLDFILENO", "FLDNAME", "FLDRANKCODE", "FLDBATCH", "FLDZONENAME", "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE", "FLDPRESENTVESSELNAME", "FLDPRESENTSIGNONDATE", "FLDLASTNOTESDESCRIPTION", "FLDDOA", "FLDLASTCONTACTDATE", "FLDSTATUS", "FLDSEAFARERREQUIREMENT", "FLDCOURSENAME", "FLDDATEOFISSUE", "FLDINSTITUTIONNAME", "FLDPLACEOFISSUE" };
            string[] alCaptions = { "SNo.", "File No.", "	Name", "Rank", "Batch", "Zone", "Last Vessel", "Sign-Off Date", "Present Vessel", "Sign-On Date", "Remarks", "Availability", "Last Contact", "Status", "Requirement", "Course Name", "Date of Issue", "Institution", "Place Of Issue" };
            DataSet ds = PhoenixCrewQuickReports.CrewQuickCourse(General.GetNullableString(txtname.Text.Trim())
                                                                    , General.GetNullableString(txtFileNo.Text.Trim())
                                                                    , General.GetNullableInteger(ucDocumentType.SelectedHard == null ? null : ucDocumentType.SelectedHard)
                                                                    , General.GetNullableString(CourseSelectedList())
                                                                    , General.GetNullableString(InstitutionSelectedList())
                                                                    , General.GetNullableDateTime(ucSignOnFromDate.Text)
                                                                    , General.GetNullableDateTime(ucSignOnToDate.Text)
                                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                    , gvCrew.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);

            General.SetPrintOptions("gvCrew", "Course", alCaptions, alColumns, ds);
            
            gvCrew.DataSource = ds;
            gvCrew.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            if (iRowCount > 0)
            {
                ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
            }
            else
            {
                ViewState["ROWSINGRIDVIEW"] = 0;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Source;
            ucError.Visible = true;
        }
    }
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        { 
        if (e.Item is GridDataItem)
        {
        }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter(string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";
        if (string.IsNullOrEmpty(fromdate) || string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "Date is required";
        }
        if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (!string.IsNullOrEmpty(fromdate)
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
            if (e.CommandName.ToUpper().Equals("PAGE"))
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
