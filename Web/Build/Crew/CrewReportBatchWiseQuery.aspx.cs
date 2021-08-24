using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportBatchWiseQuery : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();

            toolbar1.AddFontAwesomeButton("../Crew/CrewReportBatchWiseQuery.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvBatch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportBatchWiseQuery.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["DOCTYPE"] = "";
                ucToDate.Text = DateTime.Now.ToShortDateString();
                DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                       , 54, 1, string.Empty);
                ds.Tables[0].Merge(PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , 53, 1, string.Empty).Tables[0]);
                lstStatus.DataSource = ds;

                lstStatus.DataBind();
                lstStatus.Items.Insert(0, new RadListBoxItem("--Select--", ""));
                gvBatch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindCourse();
            }
            //ShowReport();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindCourse()
    {
        lstCourse.Items.Clear();
        RadListBoxItem items = new RadListBoxItem();
        if (ucDocumentType.SelectedHard.Trim().Equals(PhoenixCommonRegisters.GetHardCode(1, 103, "3")))
            lstCourse.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 103, "3")));
        else
            lstCourse.DataSource = PhoenixRegistersDocumentCourse.ListPostSeaCourse(General.GetNullableString(ViewState["DOCTYPE"].ToString()));
        lstCourse.DataTextField = "FLDDOCUMENTNAME";
        lstCourse.DataValueField = "FLDDOCUMENTID";
        lstCourse.DataBind();
        //lstCourse.Items.Insert(0, new ListItem("Direct Entry", "0"));
    }

    private string StatusSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstStatus.Items)
        {
            if (item.Selected == true && item.Value != "")
            {
                strlist.Append(item.Value.ToString());
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
    protected void DocumentTypeSelection(object sender, EventArgs e)
    {
        ViewState["DOCTYPE"] = ucDocumentType.SelectedHard;
        BindCourse();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();


        string[] alColumns = { "FLDROWNUMBER", "FLDCOURSE", "FLDBATCH", "FLDFROMDATE", "FLDTODATE", "FLDEMPLOYEECOUNT" };
        string[] alCaptions = { "Sr.No", "Course", "Batch", "From Date", "To Date", "No.of Participants" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;
        NameValueCollection nvc = Filter.CurrentCrewBatchWiseQueryReportFilters;
        if (nvc == null)
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("ucZone", ucZone.selectedlist);
            criteria.Add("ucVesselType", ucVesselType.SelectedVesseltype);
            criteria.Add("ucFromDate", ucFromDate.Text);
            criteria.Add("ucToDate", ucToDate.Text);
            criteria.Add("ucPool", ucPool.SelectedPool);
            criteria.Add("lstCourse", CourseSelectedList());
            criteria.Add("ucDocumentType", ucDocumentType.SelectedHard);
            criteria.Add("lstStatus", StatusSelectedList());
            criteria.Add("ucInstitution", ucInstitution.SelectedAddress);

            Filter.CurrentCrewBatchWiseQueryReportFilters = criteria;
        }
        DataSet ds = new DataSet();
        ds = PhoenixCrewBatchWiseQuery.CrewBatchWiseQuerySearch((nvc != null ? General.GetNullableDateTime(nvc.Get("ucFromDate")) : null)
                , (nvc != null ? General.GetNullableDateTime(nvc.Get("ucToDate")) : null)
                , (nvc != null ? General.GetNullableString(nvc.Get("ucVesselType")) : null)
                , (nvc != null ? General.GetNullableString(nvc.Get("ucPool")) : null)
                , (nvc != null ? General.GetNullableString(nvc.Get("ucZone")) : null)
                , (nvc != null ? General.GetNullableString(nvc.Get("lstStatus")) : null)
                , (nvc != null ? General.GetNullableString(nvc.Get("lstCourse")) : null)
                , (nvc != null ? General.GetNullableInteger(nvc.Get("ucInstitution")) : null)
                , (nvc != null ? General.GetNullableInteger(nvc.Get("ucDocumentType")) : null)
                , 1
                , iRowCount
                , ref iRowCount
                , ref iTotalPageCount);

        string fromdates = ucFromDate.Text;
        string todatess = ucToDate.Text;

        Response.AddHeader("Content-Disposition", "attachment; filename=Batch_Wise_Query.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Batch Wise Query</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>From:" + fromdates + "To:" + todatess + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
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
        Response.Write(string.IsNullOrEmpty(ConfigurationManager.AppSettings["softwarename"]) ? "" : ConfigurationManager.AppSettings["softwarename"]);
        Response.End();
    }

    private void ShowReport()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDROWNUMBER", "FLDCOURSE", "FLDBATCH", "FLDFROMDATE", "FLDTODATE", "FLDEMPLOYEECOUNT" };
            string[] alCaptions = { "Sr.No", "Course", "Batch", "From Date", "To Date", "No.of Participants" };


            ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
            ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
            ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;

            NameValueCollection nvc = Filter.CurrentCrewBatchWiseQueryReportFilters;
            if (nvc == null)
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucZone", ucZone.selectedlist);
                criteria.Add("ucVesselType", ucVesselType.SelectedVesseltype);
                criteria.Add("ucFromDate", ucFromDate.Text);
                criteria.Add("ucToDate", ucToDate.Text);
                criteria.Add("ucPool", ucPool.SelectedPool);
                criteria.Add("lstCourse", CourseSelectedList());
                criteria.Add("ucDocumentType", ucDocumentType.SelectedHard);
                criteria.Add("lstStatus", StatusSelectedList());
                criteria.Add("ucInstitution", ucInstitution.SelectedAddress);

                Filter.CurrentCrewBatchWiseQueryReportFilters = criteria;
            }
            DataSet ds = new DataSet();

            ds = PhoenixCrewBatchWiseQuery.CrewBatchWiseQuerySearch((nvc != null ? General.GetNullableDateTime(nvc.Get("ucFromDate")) : null)
                , (nvc != null ? General.GetNullableDateTime(nvc.Get("ucToDate")) : null)
                , (nvc != null ? General.GetNullableString(nvc.Get("ucVesselType")) : null)
                , (nvc != null ? General.GetNullableString(nvc.Get("ucPool")) : null)
                , (nvc != null ? General.GetNullableString(nvc.Get("ucZone")) : null)
                , (nvc != null ? General.GetNullableString(nvc.Get("lstStatus")) : null)
                , (nvc != null ? General.GetNullableString(nvc.Get("lstCourse")) : null)
                , (nvc != null ? General.GetNullableInteger(nvc.Get("ucInstitution")) : null)
                , (nvc != null ? General.GetNullableInteger(nvc.Get("ucDocumentType")) : null)
                , gvBatch.CurrentPageIndex + 1
                , gvBatch.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

            General.SetPrintOptions("gvBatch", "Batch Wise Query", alCaptions, alColumns, ds);

            gvBatch.DataSource = ds;
            gvBatch.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
                Filter.CurrentCrewBatchWiseQueryReportFilters = null;
                ucVesselType.SelectedVesselTypeValue = "";
                ucZone.SelectedZoneValue = "";
                ucPool.SelectedPoolValue = "";
                ucFromDate.Text = null;
                ucToDate.Text = DateTime.Now.ToShortDateString();
                ShowReport();
                gvBatch.Rebind();
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
                if (!IsValidFilter(ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucZone", ucZone.selectedlist);
                criteria.Add("ucVesselType", ucVesselType.SelectedVesseltype);
                criteria.Add("ucFromDate", ucFromDate.Text);
                criteria.Add("ucToDate", ucToDate.Text);
                criteria.Add("ucPool", ucPool.SelectedPool);
                criteria.Add("lstCourse", CourseSelectedList());
                criteria.Add("ucDocumentType", ucDocumentType.SelectedHard);
                criteria.Add("lstStatus", StatusSelectedList());
                criteria.Add("ucInstitution", ucInstitution.SelectedAddress);

                Filter.CurrentCrewBatchWiseQueryReportFilters = criteria;
                gvBatch.CurrentPageIndex = 0;
                ShowReport();
                gvBatch.Rebind();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBatch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
                ShowReport();
                gvBatch.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvBatch_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadLabel lblBatchId = (RadLabel)e.Item.FindControl("lblBatchId");
                RadLabel lblCourseId = (RadLabel)e.Item.FindControl("lblCourseId");
                LinkButton lblParticipants = (LinkButton)e.Item.FindControl("lblParticipants");
                if (lblBatchId != null && lblParticipants != null && lblCourseId != null)
                    lblParticipants.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewReportBatchWiseQueryEmployeeList.aspx?BatchId=" + lblBatchId.Text + "&CourseId=" + lblCourseId.Text + "'); return false;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBatch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBatch.CurrentPageIndex + 1;
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}