using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class CrewReportPreSeaCoursesDoneFormat2 : PhoenixBasePage
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
            MenuPreSeaCourseDoneReport.AccessRights = this.ViewState;
            MenuPreSeaCourseDoneReport.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Crew/CrewReportPreSeaCoursesDoneFormat2.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewPreSeaCoursesDone')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewReportPreSeaCoursesDoneFormat2.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

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
                BindInstitute();              
                gvCrewPreSeaCoursesDone.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                Status_Bind();
                Manager_Bind();
                ucStatus.HardTypeCode = "191";
                
            }
            MenuReports.SelectedMenuIndex = 0;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Status_Bind()
    {
        DataTable dt = PhoenixRegistersHard.CrewStatusListHard(1);

        ddlStatus.DataSource = dt;
        ddlStatus.DataBind();

    }
    protected void Manager_Bind()
    {
        DataSet ds = PhoenixRegistersAddress.ListAddress("126");

        ucManager.DataSource = ds;
        ucManager.DataBind();

    }

    protected void BindCourse()
    {
        lstCourse.Items.Clear();
        RadListBoxItem items = new RadListBoxItem();
        lstCourse.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 103, "3")));
        lstCourse.DataTextField = "FLDDOCUMENTNAME";
        lstCourse.DataValueField = "FLDDOCUMENTID";
        lstCourse.DataBind();
        //lstCourse.Items.Insert(0, new ListItem("Direct Entry", "0"));
    }

    protected void MenuReports_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("POSTSEA"))
            {
                Response.Redirect("../Crew/CrewReportCoursesDone.aspx", false);
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

        BindCourse();
    }
    private string CourseSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadListBoxItem item in lstCourse.Items)
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

    private string InstitutionSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (RadComboBoxItem item in lstInstitution.Items)
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

    protected void MenuPreSeaCourseDoneReport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GO"))
            {

                string Institutionlist = GetCsvValue(lstInstitution);
                string Managerlist = GetCsvValue(ucManager);
                string statuslist = GetCsvValue(ddlStatus);

                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("course", lstCourse.ToString());
                criteria.Add("fromdate", txtFromDate.Text);
                criteria.Add("todate", txtToDate.Text);
                criteria.Add("fileno", txtFileNo.Text);
                criteria.Add("status", statuslist);
                criteria.Add("ranklist", ucRank.selectedlist);
                criteria.Add("batchlist", ucBatch.SelectedList);
                criteria.Add("institution", Institutionlist);
                criteria.Add("vessellist", ucVessel.SelectedVessel);                
                criteria.Add("poollist", ucPool.SelectedPool);
                criteria.Add("manager", Managerlist);
                criteria.Add("IncludeNewApp", chkIncludeNewApp.Checked == true ? "1" : "0");
                criteria.Add("InstituteStatus", ucStatus.SelectedHard);
                Filter.CurrentCoursesDoneReportFilter = criteria;

                ViewState["PAGENUMBER"] = 1;
                gvCrewPreSeaCoursesDone.CurrentPageIndex = 0;

                BindData();
                gvCrewPreSeaCoursesDone.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();

    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDBATCH","FLDOALLRANK", "FLDSTATUS", "FLDLASTVESSEL","FLDPRESENTVESSEL"
                                 , "FLDSIGNOFFDATE", "FLDPRESENTVESSEL", "FLDSIGNONDATE","FLDFROMDATE","FLDTODATE","FLDPASSDATE", "FLDINSTITUTIONNAME","FLDINSTITUTIONSTATUS","FLDPLACEOFINSTITUTION","FLDEXPLANATION", "FLDCOURSENAME","FLDPERCENTAGE","FLDGRADE","FLDREMARKS", "FLDDATEOFISSUE" };
            string[] alCaptions = { "S.No.", "Emp Code", "Name", "Rank", "Batch", "O'all Rank","Status", "Last Vessel","Present vessel", "S/off Date" ,"From","TO","Date Of Passing"
                                 , "Onboard", "S/on Date", "Institution","Institution Status","Place of Issue","Qualification", "Course Name","% in Academics","Grade","Remarks" ,"Issue Date" };

            string[] FilterColumns = { "FLDSELECTEDCOURSES", "FLDSELECTEDFROMDATE", "FLDSELECTEDTODATE" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)

                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentCoursesDoneReportFilter;
            DataSet ds = new DataSet();


            ds = PhoenixCrewReports.PreseaCoursesDoneFormat2((nvc != null ? General.GetNullableString(nvc.Get("fileno")) : null)
                                , (nvc != null ? General.GetNullableString(GetCsvValue(ddlStatus)) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("ranklist")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("batchlist")) : null)
                                , (CourseSelectedList()) == "" ? null : General.GetNullableString(CourseSelectedList())
                                , (nvc != null ? General.GetNullableString(GetCsvValue(lstInstitution)) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("vessellist")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("fromdate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("todate")) : null)
                                , sortexpression, sortdirection
                                , (int)ViewState["PAGENUMBER"]
                                , gvCrewPreSeaCoursesDone.PageSize
                                , ref iRowCount
                                , ref iTotalPageCount
                                , (nvc != null ? General.GetNullableString(nvc.Get("poollist")) : null)
                                , (nvc != null ? General.GetNullableString(GetCsvValue(ucManager)) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("IncludeNewApp")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("InstituteStatus")) : null)
                                );


            General.SetPrintOptions("gvCrewPreSeaCoursesDone", "Crew PreSea Courses Done", alCaptions, alColumns, ds);

            gvCrewPreSeaCoursesDone.DataSource = ds.Tables[0];
            gvCrewPreSeaCoursesDone.VirtualItemCount = iRowCount;

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

        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDBATCH","FLDOALLRANK", "FLDSTATUS", "FLDLASTVESSEL","FLDPRESENTVESSEL"
                                 , "FLDSIGNOFFDATE", "FLDPRESENTVESSEL", "FLDSIGNONDATE","FLDFROMDATE","FLDTODATE","FLDPASSDATE", "FLDINSTITUTIONNAME","FLDINSTITUTIONSTATUS","FLDPLACEOFINSTITUTION","FLDEXPLANATION", "FLDCOURSENAME","FLDPERCENTAGE","FLDGRADE","FLDREMARKS", "FLDDATEOFISSUE" };
        string[] alCaptions = { "S.No.", "Emp Code", "Name", "Rank", "Batch", "O'all Rank","Status", "Last Vessel","Present vessel", "S/off Date" ,"From","TO","Date Of Passing"
                                 , "Onboard", "S/on Date", "Institution","Institution Status","Place of Issue","Qualification", "Course Name","% in Academics","Grade","Remarks" ,"Issue Date" };

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

        ds = PhoenixCrewReports.PreseaCoursesDoneFormat2((nvc != null ? General.GetNullableString(nvc.Get("fileno")) : null)
                                , (nvc != null ? General.GetNullableString(GetCsvValue(ddlStatus)) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("ranklist")) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("batchlist")) : null)
                                , (CourseSelectedList()) == "" ? null : General.GetNullableString(CourseSelectedList())
                                , (nvc != null ? General.GetNullableString(GetCsvValue(lstInstitution)) : null)
                                , (nvc != null ? General.GetNullableString(nvc.Get("vessellist")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("fromdate")) : null)
                                , (nvc != null ? General.GetNullableDateTime(nvc.Get("todate")) : null)
                                , sortexpression, sortdirection
                                , 1
                                , iRowCount
                                , ref iRowCount
                                , ref iTotalPageCount
                                , (nvc != null ? General.GetNullableString(nvc.Get("poollist")) : null)
                                , (nvc != null ? General.GetNullableString(GetCsvValue(ucManager)) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("IncludeNewApp")) : null)
                                , (nvc != null ? General.GetNullableInteger(nvc.Get("InstituteStatus")) : null)
                                );

        General.ShowExcel("Presea Course Done", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

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
                ViewState["PAGENUMBER"] = 1;
                txtFileNo.Text = "";
                foreach (RadComboBoxItem item in ddlStatus.Items)
                    item.Checked = false;
                ucRank.selectedlist = "";
                ucBatch.SelectedList = "";
                foreach (RadComboBoxItem item in lstInstitution.Items)
                    item.Checked = false;
                ucVessel.SelectedVessel = "";
                txtFromDate.Text = "";
                foreach (RadComboBoxItem item in ucManager.Items)
                    item.Checked = false;
                ucPool.SelectedPool = "";
                txtToDate.Text = "";
                ucStatus.SelectedHard = "";
                BindCourse();
                lstCourse.SelectedIndex = -1;
                gvCrewPreSeaCoursesDone.CurrentPageIndex = 0;
                BindData();
                gvCrewPreSeaCoursesDone.Rebind();

            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
                gvCrewPreSeaCoursesDone.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewPreSeaCoursesDone_ItemCommand(object sender, GridCommandEventArgs e)
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

    protected void gvCrewPreSeaCoursesDone_Sorting(object sender, GridSortCommandEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvCrewPreSeaCoursesDone_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton dbwl = (ImageButton)e.Item.FindControl("cmdNL");
            if (dbwl != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, dbwl.CommandName)) dbwl.Visible = false;
                dbwl.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to move this record to Nomination List?'); return false;");
            }
            RadLabel lblEmpid = (RadLabel)e.Item.FindControl("lblEmpid");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkEmployeeName");
            lbr.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewPersonalGeneral.aspx?empid=" + lblEmpid.Text + "'); return false;");

            RadLabel lblCourseName = (RadLabel)e.Item.FindControl("lblCourseName");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipCourse");
            lblCourseName.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lblCourseName.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");


            RadLabel lbtnqua = (RadLabel)e.Item.FindControl("lblQualification");
            UserControlToolTip uctQul = (UserControlToolTip)e.Item.FindControl("ucToolTipQualification");
            if (lbtnqua != null)
            {
                uctQul.Position = ToolTipPosition.TopCenter;
                uctQul.TargetControlId = lbtnqua.ClientID;
            }

            RadLabel lbtnIns = (RadLabel)e.Item.FindControl("lblInstitution");
            UserControlToolTip uctIns = (UserControlToolTip)e.Item.FindControl("ucToolTipInstitution");
            if (lbtnIns != null)
            {
                uctIns.Position = ToolTipPosition.TopCenter;
                uctIns.TargetControlId = lbtnIns.ClientID;
            }

            RadLabel lbtnCourse = (RadLabel)e.Item.FindControl("lblCourseName");
            UserControlToolTip uctCourse = (UserControlToolTip)e.Item.FindControl("ucToolTipCourse");
            if (lbtnCourse != null)
            {
                uctCourse.Position = ToolTipPosition.TopCenter;
                uctCourse.TargetControlId = lbtnCourse.ClientID;
            }
        }
    }

    protected void gvCrewPreSeaCoursesDone_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        BindData();
        gvCrewPreSeaCoursesDone.Rebind();

    }
    protected void BindInstitute()
    {
        lstInstitution.Items.Clear();
        RadComboBoxItem items = new RadComboBoxItem();
        lstInstitution.DataSource = PhoenixRegistersAddress.ListAddress("138");
        lstInstitution.DataTextField = "FLDNAME";
        lstInstitution.DataValueField = "FLDADDRESSCODE";
        lstInstitution.DataBind();
    }

    private string GetCsvValue(RadComboBox radComboBox)
    {
        var list = radComboBox.CheckedItems;
        string csv = string.Empty;
        if (list.Count != 0)
        {
            csv = ",";
            foreach (var item in list)
            {
                csv = csv + item.Value + ",";
            }
        }
        return csv;
    }
    protected void gvCrewPreSeaCoursesDone_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewPreSeaCoursesDone.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}