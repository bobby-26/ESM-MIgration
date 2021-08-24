using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Registers;

public partial class CrewOffshoreWageScaleRevision : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreWageScaleRevision.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvWageScaleRevision')", "Print Grid", "icon_print.png", "PRINT");
            MenuOffshoreWageScaleRevision.AccessRights = this.ViewState;
            MenuOffshoreWageScaleRevision.MenuList = toolbar.Show();

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

        string[] alColumns = { "FLDDESCRIPTION", "FLDNATIONALITYNAME", "FLDEFFECTIVEDATE" };
        string[] alCaptions = { "Description", "Nationality", "Effective Date" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreWageScale.SearchWageScaleRevision(General.GetNullableInteger(ucNationality.SelectedNationality),
                null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

        General.ShowExcel("Wage Scale Revision", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenuOffshoreWageScaleRevision_TabStripCommand(object sender, EventArgs e)
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDESCRIPTION", "FLDNATIONALITYNAME", "FLDEFFECTIVEDATE" };
        string[] alCaptions = { "Description", "Nationality", "Effective Date" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixCrewOffshoreWageScale.SearchWageScaleRevision(General.GetNullableInteger(ucNationality.SelectedNationality),
                null, sortexpression, sortdirection,
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvWageScaleRevision", "Wage Scale Revision", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvWageScaleRevision.DataSource = ds;
            gvWageScaleRevision.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvWageScaleRevision);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }

    protected void gvWageScaleRevision_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtDescEdit")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWageScaleRevision_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvWageScaleRevision_RowDataBound(object sender, GridViewRowEventArgs e)
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
            DataRowView dr = (DataRowView )e.Row.DataItem;
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;

                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;
            }

            UserControlNationality ucNationalityEdit = (UserControlNationality)e.Row.FindControl("ucNationalityEdit");
            if (ucNationalityEdit != null)
            {
                ucNationalityEdit.NationalityList = PhoenixRegistersCountry.ListNationality();
                ucNationalityEdit.DataBind();
                ucNationalityEdit.SelectedNationality = dr["FLDNATIONALITY"].ToString();
            }
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton ab = (ImageButton)e.Row.FindControl("cmdAdd");
            if (ab != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName))
                    ab.Visible = false;
            }

            UserControlNationality ucNationalityAdd = (UserControlNationality)e.Row.FindControl("ucNationalityAdd");
            if (ucNationalityAdd != null)
            {
                ucNationalityAdd.NationalityList = PhoenixRegistersCountry.ListNationality();
                ucNationalityAdd.DataBind();
            }
        }
    }

    protected void gvWageScaleRevision_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((TextBox)_gridView.FooterRow.FindControl("txtDescAdd")).Text,
                    ((UserControlNationality)_gridView.FooterRow.FindControl("ucNationalityAdd")).SelectedNationality,
                    ((UserControlDate)_gridView.FooterRow.FindControl("ucEffectiveDateAdd")).Text)
                )
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewOffshoreWageScale.InsertWageScaleRevision(
                    General.GetNullableString(((TextBox)_gridView.FooterRow.FindControl("txtDescAdd")).Text)
                    , int.Parse(((UserControlNationality)_gridView.FooterRow.FindControl("ucNationalityAdd")).SelectedNationality)
                    , DateTime.Parse(((UserControlDate)_gridView.FooterRow.FindControl("ucEffectiveDateAdd")).Text)
                    );

                BindData();
                ((TextBox)_gridView.FooterRow.FindControl("txtDescAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid revisionid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());
                PhoenixCrewOffshoreWageScale.DeleteWageScaleRevision(revisionid);
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("NAVIGATE"))
            {
                string revisionid = _gridView.DataKeys[nCurrentRow].Value.ToString();
                Response.Redirect("../CrewOffshore/CrewOffshoreWageScale.aspx?revisionid=" + revisionid);
            }

            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWageScaleRevision_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidData(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescEdit")).Text,
                    ((UserControlNationality)_gridView.Rows[nCurrentRow].FindControl("ucNationalityEdit")).SelectedNationality,
                    ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucEffectiveDateEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            Guid revisionid = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());

            PhoenixCrewOffshoreWageScale.UpdateWageScaleRevision(revisionid
                , General.GetNullableString(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescEdit")).Text)
                , int.Parse(((UserControlNationality)_gridView.Rows[nCurrentRow].FindControl("ucNationalityEdit")).SelectedNationality)
                , DateTime.Parse(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucEffectiveDateEdit")).Text)
                );

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
    protected void gvWageScaleRevision_OnRowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }
    private bool IsValidData(string description, string nationality, string effectivedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(description) == null)
            ucError.ErrorMessage = "Description is required.";

        if (General.GetNullableInteger(nationality) == null)
            ucError.ErrorMessage = "Nationality is required.";

        if (General.GetNullableDateTime(effectivedate) == null)
            ucError.ErrorMessage = "Effective Date is required.";

        return (!ucError.IsError);
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvWageScaleRevision.SelectedIndex = -1;
        gvWageScaleRevision.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvWageScaleRevision.SelectedIndex = -1;
        gvWageScaleRevision.EditIndex = -1;
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
        gvWageScaleRevision.SelectedIndex = -1;
        gvWageScaleRevision.EditIndex = -1;
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
