using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaCourseMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../PreSea/PreSeaCourseMaster.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvPreSea')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageLink("javascript:Openpopup('AddCourse','','../PreSea/PreSeaCoursegeneral.aspx'); return false;", "Add", "add.png", "ADDCOURSE");
            MenuPreSea.AccessRights = this.ViewState;
            MenuPreSea.MenuList = toolbargrid.Show();
            MenuPreSea.SetTrigger(pnlComponent);

            PhoenixToolbar Maintoolbar = new PhoenixToolbar();
            Maintoolbar.AddButton("Course", "COURSE");            
            Maintoolbar.AddButton("Batch", "BATCH");               
            Maintoolbar.AddButton("Subjects", "SUBJECTS");
            MenuCourseMaster.AccessRights = this.ViewState;
            //MenuCourseMaster.MenuList = Maintoolbar.Show();

            //MenuCourseMaster.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
                ViewState["PRESEACOURSEID"] = String.Empty;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
              
            }
            BindData();

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
            string[] alColumns = { "FLDSHORTNAME", "FLDPRESEACOURSENAME", "FLDDURATION", "FLDNOOFSEMESTERS" };
            string[] alCaptions = {"Abbreviation", "Course Name", "Duration (Yrs)", "No Of Semesters" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixPreSeaCourse.SearchPreSeaCourse(General.GetNullableString(""), null
                                , sortexpression, sortdirection
                                , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            //General.ShowExcel("Pre-Sea Course", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
            Response.AddHeader("Content-Disposition", "attachment; filename=PreSea Course Subjects.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/sims1.png" + "' /></td>");
            Response.Write("<td><h3>PreSea Courses </h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CourseMaster_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (Filter.CurrentPreSeaCourseMasterSelection == null)
            {
                ucError.ErrorMessage = "Please select Course to view/modify details.";
                return;
            }
            if (dce.CommandName.ToUpper().Equals("COURSE"))
            {
                Response.Redirect("../PreSea/PreSeaCourseMaster.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("BATCH"))
            {
                Response.Redirect("../PreSea/PreSeaBatch.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("SUBJECTS"))
            {
                Response.Redirect("../PreSea/PreSeaCourseSubjects.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDSHORTNAME", "FLDPRESEACOURSENAME", "FLDDURATION", "FLDNOOFSEMESTERS" };
            string[] alCaptions = { "Abbreviation", "Course Name", "Duration (Yrs)", "No Of Semesters" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixPreSeaCourse.SearchPreSeaCourse(General.GetNullableString(""), null
                                , sortexpression, sortdirection
                                , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvPreSea", "Pre-Sea Course", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPreSea.DataSource = ds;
                gvPreSea.DataBind();
                if (String.IsNullOrEmpty(ViewState["PRESEACOURSEID"].ToString()))
                {
                    ViewState["PRESEACOURSEID"] = ds.Tables[0].Rows[0]["FLDPRESEACOURSEID"].ToString();
                    gvPreSea.SelectedIndex = 0;
                }
                if (Filter.CurrentPreSeaCourseMasterSelection == null)
                {
                    gvPreSea.SelectedIndex = 0;
                    Filter.CurrentPreSeaCourseMasterSelection = ((Label)gvPreSea.Rows[0].FindControl("lblPreSeaCourseId")).Text;
                }
                SetRowSelection();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvPreSea);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        gvPreSea.SelectedIndex = -1;
        for (int i = 0; i < gvPreSea.Rows.Count; i++)
        {
            if (gvPreSea.DataKeys[i].Value.ToString().Equals(Filter.CurrentPreSeaCourseMasterSelection))
            {
                gvPreSea.SelectedIndex = i;

            }
        }
    }

    protected void gvPreSea_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToString().ToUpper() == "DELETE")
            {
                string VenueId = _gridView.DataKeys[nCurrentRow].Value.ToString();
                PhoenixPreSeaCourse.DeletePreSeaCourse(int.Parse(VenueId));
                ViewState["PRESEACOURSEID"] = String.Empty;
                BindData();
                ucStatus.Text = "Pre Sea Course is Deleted Successfully.";
            }
            if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                string PreSeaCourseId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPreSeaCourseId")).Text;
                ViewState["PRESEACOURSEID"] = PreSeaCourseId;
                Filter.CurrentPreSeaCourseMasterSelection = PreSeaCourseId;
                Response.Redirect("../PreSea/PreSeaCourseMasterGeneral.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreSea_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["STORETYPEID"] = null;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        ViewState["PRESEACOURSEID"] = String.Empty;
        BindData();
    }

    protected void gvPreSea_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        GridView _gridView = sender as GridView;
        gvPreSea.SelectedIndex = se.NewSelectedIndex;
        string PreSeaCourseId = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblPreSeaCourseId")).Text;
        ViewState["PRESEACOURSEID"] = PreSeaCourseId;
        Filter.CurrentPreSeaCourseMasterSelection = PreSeaCourseId;
        BindData();
        //Response.Redirect("../PreSea/PreSeaCourseMasterGeneral.aspx");
    }

    protected void gvPreSea_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
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
                ImageButton del = (ImageButton)e.Row.FindControl("cmdDel");
                if (del != null)
                {
                    del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                    del.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Delete ?'); return false;");
                }

                LinkButton lbtn = (LinkButton)e.Row.FindControl("lnkCourseName");
                lbtn.Attributes.Add("onclick", "Openpopup('AddAddress', '', '../PreSea/PreSeaCoursegeneral.aspx?courseid=" + lbtn.CommandArgument + "'); return false;");

                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete()");

                ImageButton db1 = (ImageButton)e.Row.FindControl("cmdEdit");
                if (db1 != null)
                {
                    db1.Attributes.Add("onclick", "Openpopup('AddAddress', '', '../PreSea/PreSeaCoursegeneral.aspx?courseid=" + lbtn.CommandArgument + "'); return false;");
                }

                ImageButton ib = (ImageButton)e.Row.FindControl("cmdSelect");
                //if (ib != null)
                //{
                //    ib.Attributes.Add("onclick", "Openpopup('AddAddress', '', '../PreSea/PreSeaCourseSubjects.aspx?courseid=" + lbtn.CommandArgument + "'); return false;");
                //}

                Label lblCourseId = (Label)e.Row.FindControl("lblPreSeaCourseId");


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

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
            ViewState["EXAMVENUEID"] = String.Empty;
            BindData();
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
            gvPreSea.SelectedIndex = -1;
            gvPreSea.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            ViewState["EXAMVENUEID"] = String.Empty;
            BindData();
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
            return true;

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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

        ViewState["PAGENUMBER"] = 1;
        gvPreSea.SelectedIndex = -1;
        BindData();
        SetPageNavigator();

    }

    protected void gvPreSea_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreSea_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;
        Filter.CurrentPreSeaCourseMasterSelection = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPreSeaCourseId")).Text;
    }
}

