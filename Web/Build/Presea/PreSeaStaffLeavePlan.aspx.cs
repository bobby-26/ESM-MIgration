using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Registers;

public partial class PreSeaStaffLeavePlan : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../PreSea/PreSeaStaffLeavePlan.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvPreseaStaffLeavePlan')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../PreSea/PreSeaStaffLeavePlan.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../PreSea/PreSeaStaffLeavePlan.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuPreSeaStaffLeavePlan.AccessRights = this.ViewState;
            MenuPreSeaStaffLeavePlan.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ddlDepartment.DataSource = PhoenixRegistersTrainingDepartment.ListTrainingDepartment();
                ddlDepartment.DataTextField = "FLDDEPARTMENTNAME";
                ddlDepartment.DataValueField = "FLDDEPARTMENTID";
                ddlDepartment.DataBind();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDFACULTYNAME", "FLDLEAVEFROM", "FLDLEAVETO" };
        string[] alCaptions = {"Faculty Name", "Leave From", "To" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPreSeaStaffLeavePlan.StaffLeavePlanSearch(General.GetNullableInteger(ddlDepartment.SelectedValue)
            , General.GetNullableInteger(ddlFaculty.SelectedValue)
            , General.GetNullableDateTime(txtLeaveFrom.Text)
            , General.GetNullableDateTime(txtLeaveTo.Text)
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=PreSeaStaffLeavePlanner.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pre-Sea Staff Leave Planner</h3></td>");
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFACULTYNAME", "FLDLEAVEFROM", "FLDLEAVETO" };
        string[] alCaptions = { "Faculty Name", "Leave From", "To" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixPreSeaStaffLeavePlan.StaffLeavePlanSearch(General.GetNullableInteger(ddlDepartment.SelectedValue)
            , General.GetNullableInteger(ddlFaculty.SelectedValue)
            , General.GetNullableDateTime(txtLeaveFrom.Text)
            , General.GetNullableDateTime(txtLeaveTo.Text)
            , sortexpression
            , sortdirection
            , (int)ViewState["PAGENUMBER"]
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount
            );

        General.SetPrintOptions("gvPreseaStaffLeavePlan", "Pre-Sea Staff Leave Planner", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPreseaStaffLeavePlan.DataSource = ds;
            gvPreseaStaffLeavePlan.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPreseaStaffLeavePlan);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void PreSeaStaffLeavePlan_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvPreseaStaffLeavePlan.EditIndex = -1;
                gvPreseaStaffLeavePlan.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlDepartment.SelectedValue = "DUMMY";
                ddlFaculty.SelectedValue = "DUMMY";
                txtLeaveFrom.Text = "";
                txtLeaveTo.Text = "";

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

    private bool IsValidStaffLeavePlan(string facultyid, string fromdate, string todate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(facultyid) == null)
        {
            ucError.ErrorMessage = "Faculty is required.";
        }
        if (General.GetNullableString(fromdate) == null)
        {
            ucError.ErrorMessage = "From date is required.";
        }
        else if (!General.GetNullableDateTime(fromdate).HasValue)
        {
            ucError.ErrorMessage = "From date is in invalid format.";
        }
        if (General.GetNullableString(todate) == null)
        {
            ucError.ErrorMessage = "To date is required.";
        }
        else if (!General.GetNullableDateTime(todate).HasValue)
        {
            ucError.ErrorMessage = "To date is in invalid format.";
        }
        else if (General.GetNullableDateTime(todate).HasValue && General.GetNullableDateTime(fromdate).HasValue)
        {
            if(DateTime.Parse(fromdate) > DateTime.Parse(todate))
                ucError.ErrorMessage = "From date should not greater than todate.";
        }


        return (!ucError.IsError);
    }

    protected void gvPreseaStaffLeavePlan_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
        SetPageNavigator();
    }

    protected void gvPreseaStaffLeavePlan_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidStaffLeavePlan(((DropDownList)_gridView.FooterRow.FindControl("ddlFacultyAdd")).SelectedValue
                                            , ((UserControlDate)_gridView.FooterRow.FindControl("txtLeaveFromAdd")).Text
                                            , ((UserControlDate)_gridView.FooterRow.FindControl("txtLeaveToAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPreSeaStaffLeavePlan.InsertLeavePlan(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , int.Parse(((DropDownList)_gridView.FooterRow.FindControl("ddlFacultyAdd")).SelectedValue)
                                                            , DateTime.Parse(((UserControlDate)_gridView.FooterRow.FindControl("txtLeaveFromAdd")).Text)
                                                            , DateTime.Parse(((UserControlDate)_gridView.FooterRow.FindControl("txtLeaveToAdd")).Text));
                
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidStaffLeavePlan(((Label)_gridView.Rows[nCurrentRow].FindControl("lblFacultyIdEdit")).Text
                                            , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtLeaveFromEdit")).Text
                                            , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtLeaveToEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPreSeaStaffLeavePlan.UpadteLeavePlan(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , Convert.ToInt64(((Label)_gridView.Rows[nCurrentRow].FindControl("lblLeavePlanIdEdit")).Text)
                                                            , DateTime.Parse(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtLeaveFromEdit")).Text)
                                                            , DateTime.Parse(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtLeaveToEdit")).Text));
                _gridView.EditIndex = -1;
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPreSeaStaffLeavePlan.DeleteStaffLeavePlan(Convert.ToInt64(((Label)_gridView.Rows[nCurrentRow].FindControl("lblLeavePlanId")).Text));
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

    protected void gvPreseaStaffLeavePlan_RowDataBound(object sender, GridViewRowEventArgs e)
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
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            DropDownList gvddlFaculty = (DropDownList)e.Row.FindControl("ddlFacultyAdd");
            if (gvddlFaculty != null)
            {
                DataSet ds = PhoenixPreSeaCourseContact.ListPreSeaCourseContactUser(General.GetNullableString(ddlDepartment.SelectedValue));

                gvddlFaculty.Items.Clear();
                ListItem li = new ListItem("--Select--", "DUMMY");

                gvddlFaculty.Items.Add(li);
                gvddlFaculty.DataBind();

                gvddlFaculty.DataSource = ds.Tables[0];
                gvddlFaculty.DataTextField = "FLDCONTACTNAME";
                gvddlFaculty.DataValueField = "FLDUSERCODE";
                gvddlFaculty.DataBind();
            }
        }
    }

    protected void gvPreseaStaffLeavePlan_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gvPreseaStaffLeavePlan_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
        SetPageNavigator();
    }

    protected void gvPreseaStaffLeavePlan_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void gvPreseaStaffLeavePlan_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
        SetPageNavigator();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvPreseaStaffLeavePlan.EditIndex = -1;
        gvPreseaStaffLeavePlan.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvPreseaStaffLeavePlan.EditIndex = -1;
        gvPreseaStaffLeavePlan.SelectedIndex = -1;
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvPreseaStaffLeavePlan.SelectedIndex = -1;
        gvPreseaStaffLeavePlan.EditIndex = -1;
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

    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataSet ds = PhoenixPreSeaCourseContact.ListPreSeaCourseContactUser(ddlDepartment.SelectedValue);
        ddlFaculty.Items.Clear();

        ListItem li = new ListItem("--Select--", "DUMMY");
        ddlFaculty.Items.Add(li);
        ddlFaculty.DataBind();

        ddlFaculty.DataSource = ds.Tables[0];
        ddlFaculty.DataTextField = "FLDCONTACTNAME";
        ddlFaculty.DataValueField = "FLDUSERCODE";
        ddlFaculty.DataBind();

        if (gvPreseaStaffLeavePlan.FooterRow != null)
        {
            DropDownList gvddlFaculty = (DropDownList)gvPreseaStaffLeavePlan.FooterRow.FindControl("ddlFacultyAdd");
            if (gvddlFaculty != null)
            {
                gvddlFaculty.Items.Clear();
                gvddlFaculty.Items.Add(li);
                gvddlFaculty.DataBind();

                gvddlFaculty.DataSource = ds.Tables[0];
                gvddlFaculty.DataTextField = "FLDCONTACTNAME";
                gvddlFaculty.DataValueField = "FLDUSERCODE";
                gvddlFaculty.DataBind();
            }
        }

        BindData();
        SetPageNavigator();
    }
}

