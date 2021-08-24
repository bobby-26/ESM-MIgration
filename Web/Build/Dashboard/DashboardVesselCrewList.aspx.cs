using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;

public partial class Dashboard_DashboardVesselCrewList : PhoenixBasePage
{
    int vesselid;

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Dashboard/DashboardVesselCrewList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCrewSearch')", "Print Grid", "icon_print.png", "PRINT");
            //MenuCrewList.MenuList = toolbar.Show();

            //PhoenixToolbar toolbarmain = new PhoenixToolbar();

            //toolbarmain.AddButton("Vessel Overview", "PARTICULARS");
            //toolbarmain.AddButton("Crew List", "CREWLIST");
            //toolbarmain.AddButton("Certificates", "CERTIFICATES");
            //toolbarmain.AddButton("Admin", "ADMIN");
            //toolbarmain.AddButton("Office Admin", "OFFICE");
            //toolbarmain.AddButton("Manning", "MANNING");
            //toolbarmain.AddButton("Travel", "TRAVEL");
            //toolbarmain.AddButton("Attachments", "ATTACHMENTS");
            //toolbarmain.AddButton("Summary", "SUMMARY");
            //toolbarmain.AddButton("Sync Status", "OLD");
            //toolbarmain.AddButton("Alerts", "ALERTS");

            if (Request.QueryString["launchedfrom"] != null)
            {
                ucTitle.ShowMenu = "false";

            }

            if (Request.QueryString["vesselid"] != null)
            {
                ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
                vesselid = int.Parse(ViewState["vesselid"].ToString());
            }
            else
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

                BindToolBar();

                //MenuDdashboradVesselParticulrs.MenuList = toolbarmain.Show();
                //MenuDdashboradVesselParticulrs.SelectedMenuIndex = 1;

                MenuCrewList.MenuList = toolbar.Show();
            }

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
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

    protected void BindToolBar()
    {
        DataSet ds = new DataSet();

        ds = PhoenixCommonDashboard.DashboardPreferencesList(PhoenixSecurityContext.CurrentSecurityContext.UserType
                                                , vesselid
                                                , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , null);

        if (ds.Tables[0].Rows.Count > 0)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            int index = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                toolbar.AddButton(dr["FLDURLDESCRIPTION"].ToString(), dr["FLDCOMMAND"].ToString());
            }

            DataRow[] drindex = ds.Tables[0].Select("FLDCOMMAND='CREWLIST'");
            if (drindex != null)
            {
                index = int.Parse(drindex[0]["FLDSEQUENCE"].ToString()) - 1;
            }
            MenuDdashboradVesselParticulrs.MenuList = toolbar.Show();
            MenuDdashboradVesselParticulrs.SelectedMenuIndex = index;
        }
    }

    protected void CrewList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDNAME", "FLDSIGNONRANKNAME", "FLDFILENO", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDSIGNONDATE", "FLDRELIEFDUEDATE" };
                string[] alCaptions = { "First Name", "Sign on Rank", "File No", "Passport No", "CDC No", "Sign on Date", "Relief Due Date" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataSet ds = PhoenixCommonDashboard.DashboardVesselCrewList(vesselid  
                                                                        , sortexpression, sortdirection
                                                                       , (int)ViewState["PAGENUMBER"], iRowCount
                                                                       , ref iRowCount, ref iTotalPageCount);

                General.ShowExcel("Crew List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDNAME", "FLDSIGNONRANKNAME", "FLDFILENO", "FLDPASSPORTNO", "FLDSEAMANBOOKNO", "FLDSIGNONDATE", "FLDRELIEFDUEDATE" };
        string[] alCaptions = { "First Name", "Sign on Rank", "File No", "Passport No", "CDC No", "Sign on Date", "Relief Due Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {

            DataSet ds = PhoenixCommonDashboard.DashboardVesselCrewList(vesselid
                                                                         , sortexpression, sortdirection
                                                                        , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                                        , ref iRowCount, ref iTotalPageCount);

            DataTable dt = ds.Tables[0];
            General.SetPrintOptions("gvCrewSearch", "Crew List", alCaptions, alColumns, ds);
            if (dt.Rows.Count > 0)
            {
                gvCrewSearch.DataSource = ds;
                gvCrewSearch.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvCrewSearch);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            DataSet dsedit = PhoenixCommonDashboard.DashboardVesselAdminEdit(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            if (dsedit.Tables[0].Rows.Count > 0)
                ViewState["FLDDTKEY"] = dsedit.Tables[0].Rows[0]["FLDDTKEY"].ToString();
            else
                ViewState["FLDDTKEY"] = "";

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

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
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
               && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                Filter.CurrentMenuCodeSelection = "CRW-PER-PER";

                Label lblName = (Label)e.Row.FindControl("lblName");
                Label empid = (Label)e.Row.FindControl("lblEmployeeId");
                LinkButton lbr = (LinkButton)e.Row.FindControl("lnkName");

                lbr.Attributes.Add("onclick", "parent.Openpopup('BioData','','../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=BIODATA&empid="
                            + empid.Text + "&showmenu=1'); return false;");

                //lbr.Attributes.Add("onclick", "javascript:Openpopup('Crew','','../Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                    lbr.Visible = true;
                else
                    lblName.Visible = true;
                
            }
        }
    }
    protected void gvCrewSearch_RowEditing(object sender, GridViewEditEventArgs e)
    {
       
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    protected void MenuDdashboradVesselParticulrs_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("PARTICULARS"))
        {
            Response.Redirect("../Dashboard/DashboardVesselParticulars.aspx", true);
        }

        if (dce.CommandName.ToUpper().Equals("CERTIFICATES"))
        {
            Response.Redirect("../Dashboard/DashboardVesselCertificates.aspx", true);
        }

        if (dce.CommandName.ToUpper().Equals("ADMIN"))
        {
            Response.Redirect("../Dashboard/DashboardAdmin.aspx", true);
        }

        if (dce.CommandName.ToUpper().Equals("OFFICE"))
        {
            Response.Redirect("../Dashboard/DashboardOfficeAdmin.aspx", true);
        }
        if (dce.CommandName.ToUpper().Equals("MANNING"))
        {
            Response.Redirect("../Dashboard/DashboardManning.aspx", true);
        }
        if (dce.CommandName.ToUpper().Equals("ATTACHMENTS"))
        {
            Response.Redirect("../Dashboard/DashboardAttachments.aspx?dtkey=" + ViewState["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.REGISTERS, true);
        }
        if (dce.CommandName.ToUpper().Equals("TRAVEL"))
        {
            Response.Redirect("../Dashboard/DashboardTravelSingOnOffList.aspx", true);
        }
        if (dce.CommandName.ToUpper().Equals("SUMMARY"))
        {
            Response.Redirect("../Dashboard/DashboardSummary.aspx", true);
        }
        if (dce.CommandName.ToUpper().Equals("NEW"))
        {
            Response.Redirect("../Dashboard/DashboardBlank.aspx", true);
        }

        if (dce.CommandName.ToUpper().Equals("OLD"))
        {
            Response.Redirect("../Dashboard/DashboardVesselSynchronizationStatus.aspx", false);
        }

        if (dce.CommandName.ToUpper().Equals("ALERTS"))
        {
            Response.Redirect("../Dashboard/DashboardAlerts.aspx", false);
        }
    }
}
