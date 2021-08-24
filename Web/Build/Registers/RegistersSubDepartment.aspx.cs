using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;

public partial class RegistersSubDepartment : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvSubDepartment.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvSubDepartment.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersSubDepartment.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvSubDepartment')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersSubDepartment.aspx", "Find", "search.png", "FIND");
            MenuRegistersSubDepartment.AccessRights = this.ViewState;
            MenuRegistersSubDepartment.MenuList = toolbar.Show();
            MenuRegistersSubDepartment.SetTrigger(pnlSubDepartmentEntry);

            if (!IsPostBack)
            {
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
        string[] alColumns = { "FLDDEPARTMENTNAME", "FLDSUBDEPARTMENTNAME" };
        string[] alCaptions = { "Department", "Sub Department " };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersSubDepartment.SubDepartmentSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableInteger(ucDepartment.SelectedDepartment) 
                        , General.GetNullableString(txtSearch.Text), sortexpression, sortdirection
                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                        , iRowCount
                        , ref iRowCount
                        , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Department.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Sub Department</h3></td>");
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

    protected void RegistersSubDepartment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvSubDepartment.SelectedIndex = -1;
                gvSubDepartment.EditIndex = -1;

                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDDEPARTMENTNAME", "FLDSUBDEPARTMENTNAME" };
        string[] alCaptions = { "Department", "Sub Department " };

        DataSet ds = PhoenixRegistersSubDepartment.SubDepartmentSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableInteger(ucDepartment.SelectedDepartment)
                        , General.GetNullableString(txtSearch.Text), sortexpression, sortdirection
                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                        , General.ShowRecords(null)
                        , ref iRowCount
                        , ref iTotalPageCount);

        General.SetPrintOptions("gvSubDepartment", "Sub Department", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvSubDepartment.DataSource = ds;
            gvSubDepartment.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvSubDepartment);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        BindData();
        SetPageNavigator();
    }

    protected void gvSubDepartment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvSubDepartment_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvSubDepartment, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvSubDepartment_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvSubDepartment.SelectedIndex = -1;
        gvSubDepartment.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvSubDepartment_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidSubDepartment(((UserControlDepartment)_gridView.Rows[nCurrentRow].FindControl("ucDepartmentEdit")).SelectedDepartment,
                                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSubDepartmentNameEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersSubDepartment.UpdateSubDepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMappingIdEdit")).Text),
                int.Parse(((UserControlDepartment)_gridView.Rows[nCurrentRow].FindControl("ucDepartmentEdit")).SelectedDepartment),
                General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSubDepartmentNameEdit")).Text));

            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSubDepartment_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvSubDepartment.SelectedIndex = e.NewSelectedIndex;
    }

    protected void gvSubDepartment_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            ((UserControlDepartment)_gridView.Rows[de.NewEditIndex].FindControl("ucDepartmentEdit")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSubDepartment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidSubDepartment(((UserControlDepartment)_gridView.FooterRow.FindControl("ucDepartmentAdd")).SelectedDepartment,
                                    ((TextBox)_gridView.FooterRow.FindControl("txtSubDepartmentNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersSubDepartment.InsertSubDepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(((UserControlDepartment)_gridView.FooterRow.FindControl("ucDepartmentAdd")).SelectedDepartment),
                    General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtSubDepartmentNameAdd")).Text));
         
                BindData();
                ((UserControlDepartment)_gridView.FooterRow.FindControl("ucDepartmentAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidSubDepartment(((UserControlDepartment)_gridView.Rows[nCurrentRow].FindControl("ucDepartmentEdit")).SelectedDepartment,
                                    ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSubDepartmentNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersSubDepartment.UpdateSubDepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMappingIdEdit")).Text),
                    int.Parse(((UserControlDepartment)_gridView.Rows[nCurrentRow].FindControl("ucDepartmentEdit")).SelectedDepartment),
                    General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtSubDepartmentNameEdit")).Text));

                _gridView.EditIndex = -1;
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersSubDepartment.DeleteSubDepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblMappingId")).Text));
            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSubDepartment_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvSubDepartment_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            UserControlDepartment ucDepartmentEdit = (UserControlDepartment)e.Row.FindControl("ucDepartmentEdit");
            DataRowView dr = (DataRowView)e.Row.DataItem;
            if (ucDepartmentEdit != null)
            {                
                ucDepartmentEdit.DepartmentList = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                ucDepartmentEdit.SelectedDepartment = dr["FLDDEPARTMENTID"].ToString();
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

            UserControlDepartment ucDepartmentAdd = (UserControlDepartment)e.Row.FindControl("ucDepartmentAdd");
            if (ucDepartmentAdd != null)
            {
                ucDepartmentAdd.DepartmentList = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
                ucDepartmentAdd.DataBind();
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvSubDepartment.SelectedIndex = -1;
        gvSubDepartment.EditIndex = -1;

        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvSubDepartment.SelectedIndex = -1;
        gvSubDepartment.EditIndex = -1;

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
        gvSubDepartment.SelectedIndex = -1;
        gvSubDepartment.EditIndex = -1;
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

    private bool IsValidSubDepartment(string departmentid, string subdepartmentname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(departmentid) == null)
            ucError.ErrorMessage = "Department is required.";

        if (General.GetNullableString(subdepartmentname) == null)
            ucError.ErrorMessage = "Sub Department Name is required.";

        return (!ucError.IsError);
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
