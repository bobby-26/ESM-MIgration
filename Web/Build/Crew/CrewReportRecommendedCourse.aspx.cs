using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewReportRecommendedCourse : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "GO", ToolBarDirection.Right);
            MenuRecommendedCourseReport.AccessRights = this.ViewState;
            MenuRecommendedCourseReport.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewReportRecommendedCourse.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewRecommendedCourse')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewReportRecommendedCourse.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuCrewRecommendedCourse.AccessRights = this.ViewState;
            MenuCrewRecommendedCourse.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                Filter.CurrentRecommendedCourseReportFilter = null;

                BindCourse();
                txtRecommendedToDate.Text = DateTime.Now.ToShortDateString();
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
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
        lstCourse.DataSource = PhoenixRegistersDocumentCourse.ListPostSeaCourse(null);
        lstCourse.DataTextField = "FLDDOCUMENTNAME";
        lstCourse.DataValueField = "FLDDOCUMENTID";
        lstCourse.DataBind();
    }


    protected void MenuRecommendedCourseReport_TabStripCommand(object sender, EventArgs e)
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
                if (!IsValidCourseFilter(txtRecommendedFromDate.Text, txtRecommendedFromDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("fileno", txtFileNo.Text);
                criteria.Add("status", ddlStatus.SelectedValue);
                criteria.Add("ranklist", ucRank.selectedlist);
                criteria.Add("course", strCourse.ToString());
                criteria.Add("poollist", ucPool.SelectedPool);
                criteria.Add("zonelist", (ucZone.selectedlist) == "Dummy" ? null : ucZone.selectedlist);
                criteria.Add("batchlist", ucBatch.SelectedList);
                criteria.Add("manager", ucManager.SelectedAddress);
                criteria.Add("recfromdate", txtRecommendedFromDate.Text);
                criteria.Add("rectodate", txtRecommendedToDate.Text);
                criteria.Add("signofffromdate", ucSignoffFromDate.Text);
                criteria.Add("signofftodate", ucSignoffToDate.Text);
                criteria.Add("completedyn", ddlCompletedyn.SelectedValue);
                criteria.Add("ucVesselType", ucVesselType.SelectedVesseltype);
                criteria.Add("ucPrincipal", ucPrincipal.SelectedAddress);
                Filter.CurrentRecommendedCourseReportFilter = criteria;

                BindData();
                gvCrew.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidCourseFilter(string recfromdate, string rectodate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        DateTime resultdate;



        if (!string.IsNullOrEmpty(recfromdate))
        {
            if (DateTime.TryParse(recfromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
            {
                ucError.ErrorMessage = "Recommended From date should be earlier than current date.";
            }
        }
        else
        {
            ucError.ErrorMessage = "Recommended From date is required.";
        }

        if (string.IsNullOrEmpty(rectodate))
        {
            ucError.ErrorMessage = "Recommended To is required.";
        }
        else if (!string.IsNullOrEmpty(recfromdate)
            && DateTime.TryParse(rectodate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(recfromdate)) < 0)
        {
            ucError.ErrorMessage = "Recommended To should be later than 'Recommended From'.";
        }

        return (!ucError.IsError);
    }


    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE","FLDBATCH", "FLDLASTVESSEL"
                                 , "FLDSIGNOFFDATE", "FLDPRESENTVESSEL", "FLDSIGNONDATE", "FLDCOURSENAME", "FLDRECOMMENDEDBY","FLDRECOMMENDEDDATE","FLDCOURSESTATUS","FLDAPPROVEDBY","FLDAPPROVEDDATE" };
            string[] alCaptions = { "S.No.", "File No", "Name", "Rank","Batch", "Last Vessel", "SignOff Date" 
                                 , "Onboard", "SignOn Date", "Course Name", " Recommended By","Recommended Date","Status"," Approved By","Approved Date" };
            string[] FilterColumns = { "FLDSELECTEDCOURSES", "FLDSELECTEDFROMDATE", "FLDSELECTEDTODATE" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentRecommendedCourseReportFilter;
            DataSet ds = new DataSet();


            ds = PhoenixCrewReports.RecommendedCoursesDoneReport((nvc != null ? General.GetNullableString(nvc.Get("fileno")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("status")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("ranklist")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("course")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("poollist")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("zonelist")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("batchlist")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("manager")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("recfromdate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("rectodate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("signofffromdate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("signofftodate")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("completedyn")) : null)
                                , sortexpression, sortdirection
                                , (int)ViewState["PAGENUMBER"]
                                , gvCrew.PageSize
                                , ref iRowCount
                                , ref iTotalPageCount
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("ucPrincipal")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("ucVesselType")) : null)
                                );

            General.SetPrintOptions("gvCrewRecommendedCourse", "Recommended Courses", alCaptions, alColumns, ds);

            gvCrew.DataSource = ds;
            gvCrew.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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

        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE","FLDBATCH", "FLDLASTVESSEL"
                                 , "FLDSIGNOFFDATE", "FLDPRESENTVESSEL", "FLDSIGNONDATE", "FLDCOURSENAME", "FLDRECOMMENDEDBY","FLDRECOMMENDEDDATE","FLDCOURSESTATUS","FLDAPPROVEDBY","FLDAPPROVEDDATE" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank","Batch", "Last Vessel", "SignOff Date" 
                                 , "Onboard", "SignOn Date", "Course Name", " Recommended By","Recommended Date","Status"," Approved By","Approved Date" };

        string[] FilterColumns = { "FLDSELECTEDFILENO", "FLDSELECTEDRANK", "FLDSELECTEDBATCH",
		                             "FLDSELECTEDCOURSES", "FLDSELECTEDFROMDATE", "FLDSELECTEDTODATE" };
        string[] FilterCaptions = { "File No", "Rank", "Batch", "Course", "Recommended From Date", "Recommended To Date" };

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

        NameValueCollection nvc = Filter.CurrentRecommendedCourseReportFilter;

        DataSet ds = new DataSet();
        ds = PhoenixCrewReports.RecommendedCoursesDoneReport((nvc != null ? General.GetNullableString(nvc.Get("fileno")) : null)
                                        , (nvc != null ? General.GetNullableInteger(nvc.Get("status")) : null)
                                        , (nvc != null ? General.GetNullableString(nvc.Get("ranklist")) : null)
                                        , (nvc != null ? General.GetNullableString(nvc.Get("course")) : null)
                                        , (nvc != null ? General.GetNullableString(nvc.Get("poollist")) : null)
                                        , (nvc != null ? General.GetNullableString(nvc.Get("zonelist")) : null)
                                        , (nvc != null ? General.GetNullableString(nvc.Get("batchlist")) : null)
                                        , (nvc != null ? General.GetNullableInteger(nvc.Get("manager")) : null)
                                        , (nvc != null ? General.GetNullableDateTime(nvc.Get("recfromdate")) : null)
                                        , (nvc != null ? General.GetNullableDateTime(nvc.Get("rectodate")) : null)
                                        , (nvc != null ? General.GetNullableDateTime(nvc.Get("signofffromdate")) : null)
                                        , (nvc != null ? General.GetNullableDateTime(nvc.Get("signofftodate")) : null)
                                        , (nvc != null ? General.GetNullableInteger(nvc.Get("completedyn")) : null)
                                        , sortexpression, sortdirection
                                        , (int)ViewState["PAGENUMBER"]
                                        , iRowCount
                                        , ref iRowCount
                                        , ref iTotalPageCount
                                        , (nvc != null ? General.GetNullableInteger(nvc.Get("ucPrincipal")) : null)
                                        , (nvc != null ? General.GetNullableString(nvc.Get("ucVesselType")) : null)
                                        );

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewRecommendedCoursesReport.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Recommended Courses</center></h5></td></tr>");
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

    }

    protected void CrewRecommendedCourse_TabStripCommand(object sender, EventArgs e)
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
                Filter.CurrentRecommendedCourseReportFilter = null;
                txtFileNo.Text = "";
                ddlStatus.SelectedIndex = 0;
                ucRank.selectedlist = "";
                ucBatch.SelectedList = "";
                txtRecommendedFromDate.Text = "";
                ucManager.SelectedAddress = "";
                ucPool.SelectedPool = "";
                ucZone.SelectedZoneValue = "";
                ucSignoffFromDate.Text = "";
                ucSignoffToDate.Text = "";
                ddlCompletedyn.SelectedIndex = -1;
                txtRecommendedToDate.Text = DateTime.Now.ToShortDateString();
                BindCourse();
                lstCourse.SelectedIndex = -1;
                ucVesselType.SelectedVesseltype = "";
                ddlStatus.SelectedIndex = -1;
                ucPrincipal.SelectedAddress = "";
                ddlCompletedyn.SelectedValue = "";
                BindData();
                gvCrew.Rebind();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
                gvCrew.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        BindData();
 
    }
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {

            RadLabel lblEmpid = (RadLabel)e.Item.FindControl("lblEmpid");
            RadLabel lblEmpcode = (RadLabel)e.Item.FindControl("lblEmployeeCode");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkEmployeeName");
            if (lblEmpcode.Text != "")
            {
                lbr.Attributes.Add("onclick", "javascript:Openpopup('chml','','CrewPersonalGeneral.aspx?empid=" + lblEmpid.Text + "&recommendedcourse=1'); return false;");
            }
            else
            {
                lbr.Attributes.Add("onclick", "javascript:Openpopup('chml','','CrewNewApplicantPersonalGeneral.aspx?empid=" + lblEmpid.Text + "&recommendedcourse=1'); return false;");
            }
            RadLabel lblCourseName = (RadLabel)e.Item.FindControl("lblCourseName");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipCourse");
            lblCourseName.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lblCourseName.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

        }
    }


}
