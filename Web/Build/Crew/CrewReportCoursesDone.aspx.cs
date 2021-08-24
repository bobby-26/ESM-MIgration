using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewReportCoursesDone : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbartop = new PhoenixToolbar();

            toolbartop.AddButton("Pre Sea", "PRESEA", ToolBarDirection.Right);
            toolbartop.AddButton("Post Sea", "POSTSEA", ToolBarDirection.Right);
            MenuReports.AccessRights = this.ViewState;
            MenuReports.MenuList = toolbartop.Show();

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "GO", ToolBarDirection.Right);
            MenuCourseDoneReport.AccessRights = this.ViewState;
            MenuCourseDoneReport.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Crew/CrewReportCoursesDone.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewCoursesDone')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewReportCoursesDone.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuCrewCoursesDone.AccessRights = this.ViewState;
            MenuCrewCoursesDone.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["DOCTYPE"] = "";
                ViewState["RANK"] = "";
                ViewState["VESSELTYPE"] = "";
                Filter.CurrentCoursesDoneReportFilter = null;
                BindCourse();
                txtToDate.Text = DateTime.Now.ToShortDateString();
                gvCrewCoursesDone.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            MenuReports.SelectedMenuIndex = 1;
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindCourse()
    {
        lstCourse.Items.Clear();
        RadListBoxItem items = new RadListBoxItem();
        lstCourse.DataSource = PhoenixRegistersDocumentCourse.ListPostSeaCourse(General.GetNullableString(ViewState["DOCTYPE"].ToString()));
        lstCourse.DataTextField = "FLDDOCUMENTNAME";
        lstCourse.DataValueField = "FLDDOCUMENTID";
        lstCourse.DataBind();
    }
    protected void Rebind()
    {
        gvCrewCoursesDone.SelectedIndexes.Clear();
        gvCrewCoursesDone.EditIndexes.Clear();
        gvCrewCoursesDone.DataSource = null;
        gvCrewCoursesDone.Rebind();
    }
    protected void MenuReports_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("PRESEA"))
            {
                Response.Redirect("../Crew/CrewReportPreSeaCoursesDoneFormat2.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void DocumentTypeSelection(object sender, EventArgs e)
    {
        ViewState["DOCTYPE"] = ucDocumentType.SelectedHard;
        BindCourse();
    }

    protected void MenuCourseDoneReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GO"))
            {
                StringBuilder strCourse = new StringBuilder();
                foreach (RadListBoxItem item in lstCourse.Items)
                {
                    if (item.Selected == true)
                    {

                        strCourse.Append(item.Value.ToString());
                        strCourse.Append(",");
                    }

                }
                if (strCourse.Length > 1)
                {
                    strCourse.Remove(strCourse.Length - 1, 1);
                }

                if (!IsValidCourseFilter(strCourse.ToString(), txtFromDate.Text, txtToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("course", strCourse.ToString());
                criteria.Add("fromdate", txtFromDate.Text);
                criteria.Add("todate", txtToDate.Text);
                criteria.Add("fileno", txtFileNo.Text);
                criteria.Add("status", ddlStatus.SelectedValue);
                criteria.Add("ranklist", ucRank.selectedlist);
                criteria.Add("batchlist", ucBatch.SelectedList);
                criteria.Add("coursetype", ucDocumentType.SelectedHard);
                criteria.Add("institution", ucInstitution.SelectedAddress);
                criteria.Add("vessellist", ucVessel.SelectedVessel);
                criteria.Add("signonfromdate", ucSignonFromDate.Text);
                criteria.Add("signontodate", ucSignonToDate.Text);
                criteria.Add("signofffromdate", ucSignoffFromDate.Text);
                criteria.Add("signofftodate", ucSignoffToDate.Text);
                criteria.Add("poollist", ucPool.SelectedPool);
                criteria.Add("manager", ucManager.SelectedAddress);
                criteria.Add("showarchived", (chkShowArchived.Checked == true ? 0 : 1).ToString());
                criteria.Add("showainactive", (chkShowIncative.Checked == true ? 0 : 1).ToString());
                criteria.Add("IncludeNewApp", (chkIncludeNewApp.Checked == true ? 1 : 0).ToString());
                Filter.CurrentCoursesDoneReportFilter = criteria;

                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidCourseFilter(string course, string fromdate, string todate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;

        if (String.IsNullOrEmpty(course) && txtFileNo.Text == "" && ucSignonFromDate.Text == null && ucSignonToDate.Text == null)
            ucError.ErrorMessage = "Course is required.";

        if (!string.IsNullOrEmpty(fromdate))
        {
            if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "Issued From should be earlier than current date.";
            }
        }
        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "Issued To is required.";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "Issued To should be later than 'Issued From'.";
        }

        return (!ucError.IsError);
    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE","FLDZONE", "FLDBATCH", "FLDSTATUS", "FLDLASTVESSEL"
                                 , "FLDSIGNOFFDATE", "FLDPRESENTVESSEL", "FLDSIGNONDATE", "FLDINSTITUTIONNAME","FLDPLACEOFISSUE", "FLDCOURSENAME", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY" };
            string[] alCaptions = { "S.No.", "Emp Code", "Name", "Rank","Zone", "Batch", "Status", "Last Vessel", "S/off Date"
                                 , "Onboard", "S/on Date", "Institution","Place of Issue", "Course Name", "Issue Date", "Expiry Date" };
            string[] FilterColumns = { "FLDSELECTEDCOURSES", "FLDSELECTEDFROMDATE", "FLDSELECTEDTODATE" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentCoursesDoneReportFilter;
            DataSet ds = new DataSet();
            ucBatch.SelectedList = ucBatch.SelectedList.ToString().Contains("Dummy,") ? ucBatch.SelectedList.ToString().Replace("Dummy,", "") : ucBatch.SelectedList;
            ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
            ucVessel.SelectedVessel = ucVessel.SelectedVessel.ToString().Contains("Dummy,") ? ucVessel.SelectedVessel.ToString().Replace("Dummy,", "") : ucVessel.SelectedVessel;
            ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;

            ds = PhoenixCrewReports.CoursesDone((nvc != null ? General.GetNullableString(nvc.Get("fileno")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("status")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("ranklist")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("batchlist")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("coursetype")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("course")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("institution")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("vessellist")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("fromdate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("todate")) : null)
                                , sortexpression, sortdirection
                                , int.Parse(ViewState["PAGENUMBER"].ToString())
                                , gvCrewCoursesDone.PageSize
                                , ref iRowCount
                                , ref iTotalPageCount
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("signonfromdate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("signontodate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("signofffromdate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("signofftodate")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("poollist")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("manager")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("showarchived")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("showainactive")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("IncludeNewApp")) : null)
                                );

            General.SetPrintOptions("gvCrewCoursesDone", "Crew Courses Done", alCaptions, alColumns, ds);
            gvCrewCoursesDone.DataSource = ds.Tables[0];
            gvCrewCoursesDone.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            if (ddlStatus.SelectedValue == "1")
            {
                gvCrewCoursesDone.Columns[7].Visible = false;
                gvCrewCoursesDone.Columns[8].Visible = false;
            }
            if (ddlStatus.SelectedValue == "0")
            {
                gvCrewCoursesDone.Columns[9].Visible = false;
                gvCrewCoursesDone.Columns[10].Visible = false;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE","FLDZONE", "FLDBATCH", "FLDSTATUS", "FLDLASTVESSEL"
                                 , "FLDSIGNOFFDATE", "FLDPRESENTVESSEL", "FLDSIGNONDATE", "FLDINSTITUTIONNAME","FLDPLACEOFISSUE", "FLDCOURSENAME", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY" };
        string[] alCaptions = { "S.No.", "Emp Code", "Name", "Rank","Zone", "Batch", "Status", "Last Vessel", "S/off Date"
                                 , "Onboard", "S/on Date", "Institution","Place of Issue", "Course Name", "Issue Date", "Expiry Date" };

        string[] FilterColumns = { "FLDSELECTEDFILENO", "FLDSELECTEDSTATUS", "FLDSELECTEDRANK", "FLDSELECTEDBATCH", "FLDSELECTEDCOURSETYPE",
                                     "FLDSELECTEDCOURSES", "FLDSELECTEDINSTITUTION", "FLDSELECTEDVESSEL", "FLDSELECTEDFROMDATE", "FLDSELECTEDTODATE" };
        string[] FilterCaptions = { "File No", "Status", "Rank", "Batch", "Course Type", "Course", "Institution", "Vessel", "From Date", "To Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        ViewState["PAGENUMBER"] = 1;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentCoursesDoneReportFilter;

        DataSet ds = new DataSet();
        ucBatch.SelectedList = ucBatch.SelectedList.ToString().Contains("Dummy,") ? ucBatch.SelectedList.ToString().Replace("Dummy,", "") : ucBatch.SelectedList;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucVessel.SelectedVessel = ucVessel.SelectedVessel.ToString().Contains("Dummy,") ? ucVessel.SelectedVessel.ToString().Replace("Dummy,", "") : ucVessel.SelectedVessel;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;

        ds = PhoenixCrewReports.CoursesDone((nvc != null ? General.GetNullableString(nvc.Get("fileno")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("status")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("ranklist")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("batchlist")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("coursetype")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("course")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("institution")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("vessellist")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("fromdate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("todate")) : null)
                                , sortexpression, sortdirection
                                , 1
                                , iRowCount
                                , ref iRowCount
                                , ref iTotalPageCount
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("signonfromdate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("signontodate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("signofffromdate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("signofftodate")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("poollist")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("manager")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("showarchived")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("showainactive")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("IncludeNewApp")) : null)
                                );


        Response.AddHeader("Content-Disposition", "attachment; filename=CrewCoursesDoneReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Courses Done Report</center></h5></td></tr>");
        //Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
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
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();

        //if (ds.Tables.Count > 0)
        //    General.ShowExcel("Crew Courses Done", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void CrewCoursesDone_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentCoursesDoneReportFilter = null;
                txtFileNo.Text = "";
                ddlStatus.SelectedIndex = 0;
                ucRank.selectedlist = "";
                ucBatch.SelectedList = "";
                ucDocumentType.SelectedHard = "";
                ucInstitution.SelectedAddress = "";
                ucVessel.SelectedVessel = "";
                txtFromDate.Text = "";
                ucManager.SelectedAddress = "";
                ucPool.SelectedPool = "";
                txtToDate.Text = DateTime.Now.ToShortDateString();
                ViewState["DOCTYPE"] = ucDocumentType.SelectedHard;
                BindCourse();
                lstCourse.SelectedIndex = 0;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                
                Rebind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewCoursesDone_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
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

    protected void gvCrewCoursesDone_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        //BindData();
    }

    protected void gvCrewCoursesDone_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton dbwl = (ImageButton)e.Item.FindControl("cmdNL");
            if (dbwl != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, dbwl.CommandName)) dbwl.Visible = false;
                dbwl.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to move this record to Nomination List?'); return false;");

                //Label st = (Label)e.Row.FindControl("lblStatus");
                //if (st.Text.ToUpper().Equals("RECOMMENDED"))
                //    dbwl.Enabled = true;
                //else
                //    dbwl.Enabled = false;
            }
            RadLabel lblEmpid = (RadLabel)e.Item.FindControl("lblEmpid");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkEmployeeName");

            lbr.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblEmpid.Text + "'); return false;");

            RadLabel lblCourseName = (RadLabel)e.Item.FindControl("lblCourseName");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipCourse");
            lblCourseName.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lblCourseName.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

            RadLabel lblInstitution = (RadLabel)e.Item.FindControl("lblInstitution");
            UserControlToolTip uctt = (UserControlToolTip)e.Item.FindControl("ucToolTipInstitution");
            lblInstitution.Attributes.Add("onmouseover", "showTooltip(ev, '" + uctt.ToolTip + "', 'visible');");
            lblInstitution.Attributes.Add("onmouseout", "showTooltip(ev, '" + uctt.ToolTip + "', 'hidden');");
        }
    }
    protected void gvCrewCoursesDone_SelectedIndexChanging(object sender, GridSelectCommandEventArgs e)
    {
        Rebind();
        // SetPageNavigator();
    }
    protected void gvCrewCoursesDone_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewCoursesDone.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}