using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOffshore;

public partial class CrewOffshoreQueryActivity : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("New", "NEW");
            CrewQuery.AccessRights = this.ViewState;
            CrewQuery.MenuList = toolbarmain.Show();

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddImageButton("../CrewOffshore/CrewOffshoreQueryActivity.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Export to Excel", "icon_xls.png", "Excel");
            toolbarsub.AddImageLink("javascript:Openpopup('Filter','','CrewOffshoreQueryActivityFilter.aspx'); return false;", "Filter", "search.png", "FIND");
            toolbarsub.AddImageButton("../CrewOffshore/CrewOffshoreQueryActivity.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Clear Filter", "clear-filter.png", "CLEAR");
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                //ddlGrade.QuickTypeCode = ((int)PhoenixQuickTypeCode.MISCELLANEOUSGRADE).ToString();
                ViewState["LAUNCHEDFROM"] = "";
                ViewState["pl"] = "";

                if (Request.QueryString["p"] != null && Request.QueryString["p"].ToString() != string.Empty)
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["p"]);
                else
                    ViewState["PAGENUMBER"] = 1;

                if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString() != "")
                    ViewState["LAUNCHEDFROM"] = Request.QueryString["launchedfrom"].ToString();

                if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
                    ViewState["pl"] = Request.QueryString["pl"].ToString();

                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void SetPageNavigator()
    {
        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
        {
            int result;
            if (Int32.TryParse(txtnopage.Text, out result))
            {
                ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

                if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


                if (0 >= Int32.Parse(txtnopage.Text))
                    ViewState["PAGENUMBER"] = 1;

                if ((int)ViewState["PAGENUMBER"] == 0)
                    ViewState["PAGENUMBER"] = 1;

                txtnopage.Text = ViewState["PAGENUMBER"].ToString();
            }
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvCrewSearch.SelectedIndex = -1;
            gvCrewSearch.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewQueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentOffshoreEmployeeFilterCriteria = null;
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        Filter.CurrentNewApplicantSelection = null;
        Response.Redirect("..\\Crew\\CrewNewApplicantPersonalGeneral.aspx?t=n" + (Request.QueryString.ToString() != string.Empty ? ("&" + Request.QueryString.ToString()) : string.Empty));
    }

    protected void gvCrewSearch_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void gvCrewSearch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "SORT") return;
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper() == "GETEMPLOYEE")
            {
                string familyid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblfamlyid")).Text;
                string newapp = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblNewApp")).Text;
                Session["REFRESHFLAG"] = null;

                if (newapp.Equals("1"))
                {
                    Filter.CurrentNewApplicantSelection = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeid")).Text;
                    Response.Redirect("..\\Crew\\CrewNewApplicantPersonalGeneral.aspx?p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes&launchedfrom=" + ViewState["LAUNCHEDFROM"].ToString() + "&pl=" + ViewState["pl"].ToString());
                }
                else
                {
                    Filter.CurrentCrewSelection = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeid")).Text;
                    if (familyid == "")
                    {
                        Response.Redirect("..\\Crew\\CrewPersonalGeneral.aspx?p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes&launchedfrom=" + ViewState["LAUNCHEDFROM"].ToString() + "&pl=" + ViewState["pl"].ToString());
                    }
                    else
                    {
                        Response.Redirect("..\\Crew\\CrewPersonalGeneral.aspx?empid=" + Filter.CurrentCrewSelection + "&familyid=" + familyid + "&p=" + ViewState["PAGENUMBER"].ToString() + "&back=yes&launchedfrom=" + ViewState["LAUNCHEDFROM"].ToString() + "&pl=" + ViewState["pl"].ToString(), false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCrewSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                Label empid = (Label)e.Row.FindControl("lblEmployeeid");
                Label status = (Label)e.Row.FindControl("lblStatus");
                ImageButton sg = (ImageButton)e.Row.FindControl("imgActivity");

                if (drv["FLDNEWAPP"] != null && drv["FLDNEWAPP"].ToString().Equals("1"))
                {
                    sg.Attributes.Add("onclick", "parent.Openpopup('codehelpactivity', '', '../Crew/CrewNewApplicantActivitylGeneral.aspx?empid=" + empid.Text + "');return false;");
                }
                else
                {
                    if (status != null && (status.Text.Contains("ONB") || status.Text.Contains("OBP")))
                    {
                        sg.Attributes.Add("onclick", "parent.Openpopup('codehelpactivity', '', '../Crew/CrewActivityGeneral.aspx?empid=" + empid.Text + "&sg=0&ntbr=0&ds=" + drv["FLDDIRECTSIGNON"].ToString() + "&launchedfrom=offshore');return false;");
                    }
                    else
                    {
                        sg.Attributes.Add("onclick", "parent.Openpopup('codehelpactivity', '', '../Crew/CrewActivityGeneral.aspx?empid=" + empid.Text + "&sg=0&ds=" + drv["FLDDIRECTSIGNON"].ToString() + "&launchedfrom=offshore');return false;");
                    }
                }

                ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                Label lbtn = (Label)e.Row.FindControl("lblNextVessel");
                UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("ucToolTipVesselNames");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");

                ImageButton pd = (ImageButton)e.Row.FindControl("cmdPDForm");               

                if (pd != null)
                {
                    pd.Visible = SessionUtil.CanAccess(this.ViewState, pd.CommandName);
                    pd.Attributes.Add("onclick", "Openpopup('PDForm', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + empid.Text + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
                }
            }
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME", "FLDRANKPOSTEDNAME", "FLDEMPLOYEECODE", "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE", 
                               "FLDPRESENTVESSELNAME", "FLDPRESENTSIGNONDATE", "FLDPLANNEDVESSELCODES", "FLDDOA", "FLDSTATUSDESCRIPTION","FLDZONE","FLDBATCHNO","FLDDECIMALEXPERIENCE" };
        string[] alCaptions = { "Name", "Rank", "File No", "Last Vessel", "Sign-Off Date", "Present Vessel",
                                  "Sign-On Date","Next Vessel","DOA","Status","Zone","Batch No","Exp(M)"  };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentOffshoreEmployeeFilterCriteria;
        if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
        {
            if (nvc == null)
            {
                nvc = new NameValueCollection();
                nvc.Add("txtName", string.Empty);
                nvc.Add("txtFileNumber", string.Empty);
                nvc.Add("txtRefNumber", string.Empty);
                nvc.Add("txtPassortNo", string.Empty);
                nvc.Add("txtSeamanbookNo", string.Empty);
                nvc.Add("ddlSailedRank", string.Empty);
                nvc.Add("ddlVesselType", string.Empty);
                nvc.Add("txtAppliedStartDate", string.Empty);
                nvc.Add("txtAppliedEndDate", string.Empty);
                nvc.Add("txtDOAStartDate", string.Empty);
                nvc.Add("txtDOAEndDate", string.Empty);
                nvc.Add("txtDOBStartDate", string.Empty);
                nvc.Add("txtDOBEndDate", string.Empty);
                nvc.Add("ddlCourse", string.Empty);
                nvc.Add("ddlLicences", string.Empty);
                nvc.Add("ddlEngineType", string.Empty);
                nvc.Add("ddlVisa", string.Empty);
                nvc.Add("ddlZone", string.Empty);
                nvc.Add("lstRank", string.Empty);
                nvc.Add("lstNationality", string.Empty);
                nvc.Add("ddlPrevCompany", string.Empty);
                nvc.Add("ddlActiveYN", string.Empty);
                nvc.Add("ddlStatus", string.Empty);
                nvc.Add("ddlCountry", string.Empty);
                nvc.Add("ddlState", string.Empty);
                nvc.Add("ddlCity", string.Empty);
                nvc.Add("txtNOKName", string.Empty);
                nvc.Add("chkIncludepastexp", "0");
                nvc.Add("ddlPool", Request.QueryString["pl"]);
                nvc.Add("ddlVessel", string.Empty);
                nvc.Add("lstBatch", string.Empty);
                nvc.Add("ddlFlag", string.Empty);
                nvc.Add("ucPrincipal", string.Empty);
            }
            else
            {
                nvc["ddlPool"] = Request.QueryString["pl"];
            }
        }

        DataTable dt = PhoenixCrewOffshoreEmployee.QueryActivity(nvc != null ? nvc.Get("lstNationality") : string.Empty
                                                                   , nvc != null ? nvc.Get("txtPassortNo") : string.Empty
                                                                   , nvc != null ? nvc.Get("txtSeamanbookNo") : string.Empty
                                                                   , nvc != null ? nvc.Get("txtFileNumber") : string.Empty
                                                                   , nvc != null ? nvc.Get("lstRank") : string.Empty
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlZone") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlSailedRank") : string.Empty)
                                                                   , nvc != null ? nvc.Get("ddlVesselType") : string.Empty
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlEngineType") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCourse") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlLicences") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlVisa") : string.Empty)
                                                                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedStartDate") : string.Empty)
                                                                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedEndDate") : string.Empty)
                                                                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOAStartDate") : string.Empty)
                                                                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOAEndDate") : string.Empty)
                                                                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBStartDate") : string.Empty)
                                                                   , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBEndDate") : string.Empty)
                                                                   , nvc != null ? nvc.Get("txtRefNumber") : string.Empty, nvc != null ? nvc.Get("txtName") : string.Empty
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlPrevCompany") : string.Empty)
                                                                   , nvc != null ? nvc.Get("ddlStatus") : string.Empty
                                                                   , nvc != null ? (byte?)General.GetNullableInteger(nvc.Get("ddlActiveYN")) : null
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlState") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCity") : string.Empty)
                                                                   , sortexpression, sortdirection
                                                                   , 1, iRowCount
                                                                   , ref iRowCount, ref iTotalPageCount
                                                                   , nvc != null ? nvc.Get("txtNOKName") : string.Empty, null
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("chkIncludepastexp") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlVessel") : string.Empty)
                                                                   , nvc != null ? nvc.Get("ddlPool") : string.Empty
                                                                   , nvc != null ? nvc.Get("lstBatch") : string.Empty
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ddlFlag") : string.Empty)
                                                                   , General.GetNullableInteger(nvc != null ? nvc.Get("ucPrincipal") : string.Empty));

        General.ShowExcel("PersonnelMaster", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {

            NameValueCollection nvc = Filter.CurrentOffshoreEmployeeFilterCriteria;
            if (Request.QueryString["pl"] != null && Request.QueryString["pl"].ToString() != "")
            {
                if (nvc == null)
                {
                    nvc = new NameValueCollection();
                    nvc.Add("txtName", string.Empty);
                    nvc.Add("txtFileNumber", string.Empty);
                    nvc.Add("txtRefNumber", string.Empty);
                    nvc.Add("txtPassortNo", string.Empty);
                    nvc.Add("txtSeamanbookNo", string.Empty);
                    nvc.Add("ddlSailedRank", string.Empty);
                    nvc.Add("ddlVesselType", string.Empty);
                    nvc.Add("txtAppliedStartDate", string.Empty);
                    nvc.Add("txtAppliedEndDate", string.Empty);
                    nvc.Add("txtDOAStartDate", string.Empty);
                    nvc.Add("txtDOAEndDate", string.Empty);
                    nvc.Add("txtDOBStartDate", string.Empty);
                    nvc.Add("txtDOBEndDate", string.Empty);
                    nvc.Add("ddlCourse", string.Empty);
                    nvc.Add("ddlLicences", string.Empty);
                    nvc.Add("ddlEngineType", string.Empty);
                    nvc.Add("ddlVisa", string.Empty);
                    nvc.Add("ddlZone", string.Empty);
                    nvc.Add("lstRank", string.Empty);
                    nvc.Add("lstNationality", string.Empty);
                    nvc.Add("ddlPrevCompany", string.Empty);
                    nvc.Add("ddlActiveYN", string.Empty);
                    nvc.Add("ddlStatus", string.Empty);
                    nvc.Add("ddlCountry", string.Empty);
                    nvc.Add("ddlState", string.Empty);
                    nvc.Add("ddlCity", string.Empty);
                    nvc.Add("txtNOKName", string.Empty);
                    nvc.Add("chkIncludepastexp", "0");
                    nvc.Add("ddlPool", Request.QueryString["pl"]);
                    nvc.Add("ddlVessel", string.Empty);
                    nvc.Add("lstBatch", string.Empty);
                    nvc.Add("ddlFlag", string.Empty);
                    nvc.Add("ucPrincipal", string.Empty);
                }
                else
                {
                    nvc["ddlPool"] = Request.QueryString["pl"];
                }
            }
            DataTable dt = PhoenixCrewOffshoreEmployee.QueryActivity(nvc != null ? nvc.Get("lstNationality") : string.Empty
                                                                       , nvc != null ? nvc.Get("txtPassortNo") : string.Empty
                                                                       , nvc != null ? nvc.Get("txtSeamanbookNo") : string.Empty
                                                                       , nvc != null ? nvc.Get("txtFileNumber") : string.Empty
                                                                       , nvc != null ? nvc.Get("lstRank") : string.Empty
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlZone") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlSailedRank") : string.Empty)
                                                                       , nvc != null ? nvc.Get("ddlVesselType") : string.Empty
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlEngineType") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCourse") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlLicences") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlVisa") : string.Empty)
                                                                       , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedStartDate") : string.Empty)
                                                                       , General.GetNullableDateTime(nvc != null ? nvc.Get("txtAppliedEndDate") : string.Empty)
                                                                       , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOAStartDate") : string.Empty)
                                                                       , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOAEndDate") : string.Empty)
                                                                       , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBStartDate") : string.Empty)
                                                                       , General.GetNullableDateTime(nvc != null ? nvc.Get("txtDOBEndDate") : string.Empty)
                                                                       , nvc != null ? nvc.Get("txtRefNumber") : string.Empty, nvc != null ? nvc.Get("txtName") : string.Empty
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlPrevCompany") : string.Empty)
                                                                       , nvc != null ? nvc.Get("ddlStatus") : string.Empty
                                                                       , nvc != null ? (byte?)General.GetNullableInteger(nvc.Get("ddlActiveYN")) : null
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCountry") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlState") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlCity") : string.Empty)
                                                                       , sortexpression, sortdirection
                                                                       , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                                       , ref iRowCount, ref iTotalPageCount
                                                                       , nvc != null ? nvc.Get("txtNOKName") : string.Empty, null
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("chkIncludepastexp") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlVessel") : string.Empty)
                                                                       , nvc != null ? nvc.Get("ddlPool") : string.Empty
                                                                       , nvc != null ? nvc.Get("lstBatch") : string.Empty
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ddlFlag") : string.Empty)
                                                                       , General.GetNullableInteger(nvc != null ? nvc.Get("ucPrincipal") : string.Empty));



            if (dt.Rows.Count > 0)
            {
                gvCrewSearch.DataSource = dt;
                gvCrewSearch.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvCrewSearch);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void ResetFormControlValues(Control parent)
    {
        try
        {
            foreach (Control c in parent.Controls)
            {
                if (c.Controls.Count > 0)
                {
                    ResetFormControlValues(c);
                }
                else
                {
                    switch (c.GetType().ToString())
                    {
                        case "System.Web.UI.WebControls.TextBox":
                            ((TextBox)c).Text = "";
                            break;
                        case "System.Web.UI.WebControls.CheckBox":
                            ((CheckBox)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.RadioButton":
                            ((RadioButton)c).Checked = false;
                            break;
                        case "System.Web.UI.WebControls.DropDownList":
                            ((DropDownList)c).SelectedIndex = 0;
                            break;
                        case "System.Web.UI.WebControls.ListBox":
                            ((ListBox)c).SelectedIndex = 0;
                            break;

                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
