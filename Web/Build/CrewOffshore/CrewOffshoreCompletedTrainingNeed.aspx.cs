using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;

public partial class CrewOffshoreCompletedTrainingNeed : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["employeeid"] = "";
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["coursetype"] = "";
                if (Request.QueryString["coursetype"] != null && Request.QueryString["coursetype"].ToString() != "")
                    ViewState["coursetype"] = Request.QueryString["coursetype"].ToString();
                else
                    ViewState["coursetype"] = PhoenixCommonRegisters.GetHardCode(1, 103, "4");   
            }

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Pending Training Needs", "PENDINGTRAININGNEED");
            toolbarsub.AddButton("Completed Training Needs", "COMPLETEDTRAININGNEED");
            toolbarsub.AddButton("Overridden Training Needs", "OVERRIDDENTRAININGNEED");
            TrainingNeed.AccessRights = this.ViewState;
            TrainingNeed.MenuList = toolbarsub.Show();
            TrainingNeed.SelectedMenuIndex = 1;

            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreCompletedTrainingNeed.aspx?coursetype=" + ViewState["coursetype"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvOffshoreTraining')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreTrainingNeedsCompletedSearch.aspx?PendingNeedsYN=0&Vessel=0&Override=0", "Filter", "search.png", "SEARCH");
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreCompletedTrainingNeed.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuOffshoreTraining.AccessRights = this.ViewState;
            MenuOffshoreTraining.MenuList = toolbar.Show();

            PhoenixToolbar toolbarcourse = new PhoenixToolbar();
            toolbarcourse.AddButton("CBT", "CBT");
            toolbarcourse.AddButton("Training Course", "HSEQA");
            CourseRequest.AccessRights = this.ViewState;
            CourseRequest.MenuList = toolbarcourse.Show();
            if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
                CourseRequest.SelectedMenuIndex = 0;
            else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
                CourseRequest.SelectedMenuIndex = 1;
            else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "8"))
                CourseRequest.SelectedMenuIndex = 2;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CourseRequest_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        string coursetype = "";

        if (dce.CommandName.ToUpper().Equals("CBT"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "4");
            Response.Redirect("../CrewOffshore/CrewOffshoreCompletedTrainingNeed.aspx?coursetype=" + coursetype, true);
        }
        else if (dce.CommandName.ToUpper().Equals("HSEQA"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "7");
            Response.Redirect("../CrewOffshore/CrewOffshoreCompletedTrainingNeed.aspx?coursetype=" + coursetype, true);
        }
        else if (dce.CommandName.ToUpper().Equals("SEAGULL"))
        {
            coursetype = PhoenixCommonRegisters.GetHardCode(1, 103, "8");
            Response.Redirect("../CrewOffshore/CrewOffshoreCompletedTrainingNeed.aspx?coursetype=" + coursetype, true);
        }
    }
    protected void gvOffshoreTraining_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = new string[0];
        string[] alCaptions = new string[0];
        if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
        {
            alColumns = new string[] { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO"};
            alCaptions = new string[] { "Name", "Rank", "File No"};
        }
        else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
        {
            alColumns = new string[] { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO"};
            alCaptions = new string[] { "Name", "Rank", "File No"};
        }
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;
        int? vesselid = null;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        else if (nvc != null)
        {
            if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
        }

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchGroupTrainingNeeds(null,
                            null,
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount,
                            nvc != null ? nvc["txtName"] : "",
                            nvc != null ? nvc["txtFileNo"] : "",
                            General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : ""),
                            0,
                            General.GetNullableInteger(ViewState["coursetype"].ToString()),
                            General.GetNullableInteger(nvc != null ? (nvc["chkShowArchived"].ToString() == "1" ? "0" : "1") : ""),
                            vesselid,
                            General.GetNullableInteger(nvc != null ? nvc["ddlCourse"] : ""),
                            General.GetNullableInteger(nvc != null ? nvc["ddlCatergory"] : ""),
                            General.GetNullableInteger(nvc != null ? nvc["ddlSubCatergory"] : ""),
                            General.GetNullableInteger(nvc != null ? nvc["ucLevelOfImprovement"] : ""),
                            General.GetNullableInteger(nvc != null ? nvc["ucTypeofTraining"] : ""),
                            General.GetNullableInteger(nvc != null ? nvc["ucTobedoneby"] : ""),
                            General.GetNullableDateTime(nvc != null ? nvc["ucCompletedFrom"] : ""),
                            General.GetNullableDateTime(nvc != null ? nvc["ucCompletedTo"] : ""));

        Response.AddHeader("Content-Disposition", "attachment; filename=OffshoreCompletedTrainingNeeds.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Training Needs</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void TrainingNeed_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("COMPLETEDTRAININGNEED"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreCompletedTrainingNeed.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("PENDINGTRAININGNEED"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreTrainingNeeds.aspx", true);
        }
        else if (dce.CommandName.ToUpper().Equals("OVERRIDDENTRAININGNEED"))
        {
            Response.Redirect("../CrewOffshore/CrewOffshoreOverrideTrainingNeed.aspx", true);
        }
    }

    protected void MenuOffshoreTraining_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentOffshoreTrainingNeedSearch = null;
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = new string[0];
        string[] alCaptions = new string[0];
        if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
        {
            alColumns = new string[] { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO"};
            alCaptions = new string[] { "Name", "Rank", "File No"};
        }
        else if (ViewState["coursetype"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 103, "7"))
        {
            alColumns = new string[] { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO"};
            alCaptions = new string[] { "Name", "Rank", "File No"};
        }
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentOffshoreTrainingNeedSearch;
        int? vesselid = null;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        else if (nvc != null)
        {
            if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
        }

        DataTable dt = PhoenixCrewOffshoreTrainingNeeds.SearchGroupTrainingNeedsCompleted(null,
                null,
                sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount,
                nvc != null ? nvc["txtName"] : "",
                nvc != null ? nvc["txtFileNo"] : "",
                General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : ""),
                General.GetNullableInteger(ViewState["coursetype"].ToString())
               
                );
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvOffshoreTraining", "Completed Training Needs", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvOffshoreTraining.DataSource = ds;
            gvOffshoreTraining.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvOffshoreTraining);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }

    protected void gvOffshoreTraining_RowDataBound(object sender, GridViewRowEventArgs e)
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
    }
    protected void gvOffshoreTraining_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("GETEMPLOYEE"))
            {
                GridView _gridView = (GridView)sender;
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                string lblEmployeeid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeid")).Text;

                Response.Redirect("../CrewOffshore/CrewOffshoreCompletedTrainingNeedDetail.aspx?employeeid=" + lblEmployeeid + "&coursetype=" + ViewState["coursetype"].ToString(), true);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshoreTraining_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvOffshoreTraining_OnRowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvOffshoreTraining.SelectedIndex = -1;
        gvOffshoreTraining.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvOffshoreTraining.SelectedIndex = -1;
        gvOffshoreTraining.EditIndex = -1;
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvOffshoreTraining.SelectedIndex = -1;
        gvOffshoreTraining.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
