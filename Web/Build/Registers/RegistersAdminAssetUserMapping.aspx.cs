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
using SouthNests.Phoenix.AdminAssetUserMapping;

public partial class Registers_RegistersAdminAssetUserMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    { 
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersAdminAssetUserMapping.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvUserMapping')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersAdminAssetUserMapping.aspx", "Search User", "search.png", "Search");
            UserMenuList.AccessRights = this.ViewState;
            UserMenuList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["ASSETID"] != null && Request.QueryString["ASSETNAME"] != null)
                {
                    ViewState["ASSETID"] = Request.QueryString["ASSETID"].ToString();
                    ViewState["FLDNAME"] = Request.QueryString["ASSETNAME"].ToString();
                    txtAssetType.Text = ViewState["FLDNAME"].ToString();
                }
                if (Request.QueryString["ZONEID"] != null)
                {
                    ViewState["ZONEID"] = Request.QueryString["ZONEID"].ToString();
                    ucZone.SelectedZone = ViewState["ZONEID"].ToString();
                }
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

    protected void UserMenuList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                gvUserMapping.EditIndex = -1;
                gvUserMapping.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvUserMapping.EditIndex = -1;
                gvUserMapping.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
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

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDNAME", "FLDASSIGNEDDATE", "FLDUNASSIGNEDDATE" };
            string[] alCaptions = { "User", "Assigned Date", "Unassigned Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixAdministrationAssetUserMapping.AdminAssetUserMappingSearch(General.GetNullableInteger(ViewState["ZONEID"].ToString())
                , General.GetNullableGuid(ViewState["ASSETID"].ToString())
                , sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

            General.ShowExcel("User Mapping", dt, alColumns, alCaptions, null, string.Empty);
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

            string[] alColumns = { "FLDNAME", "FLDASSIGNEDDATE", "FLDUNASSIGNEDDATE" };
            string[] alCaptions = { "User", "Assigned Date", "Unassigned Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixAdministrationAssetUserMapping.AdminAssetUserMappingSearch(
                General.GetNullableInteger(ViewState["ZONEID"].ToString())
                , General.GetNullableGuid(ViewState["ASSETID"].ToString())
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , General.ShowRecords(null)
                , ref iRowCount
                , ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvUserMapping.DataSource = dt;
                gvUserMapping.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvUserMapping);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            General.SetPrintOptions("gvUserMapping", "User Mapping", alCaptions, alColumns, ds);
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvUserMapping_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvUserMapping_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvUserMapping_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridview = (GridView)sender;
            int nCurrentRow = e.RowIndex;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvUserMapping_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridview = (GridView)sender;
            int nCurrentRow = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidUser(ucZone.SelectedZone
                    ,((UserControlDate)_gridview.FooterRow.FindControl("ucAssignedDateAdd")).Text
                    , ((DropDownList)_gridview.FooterRow.FindControl("ddlUserNameAdd")).SelectedValue
                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAdministrationAssetUserMapping.AdminAssetUserMappingInsert
                    (
                    new Guid (ViewState["ASSETID"].ToString())
                    , int.Parse(ucZone.SelectedZone)
                    , int.Parse(((DropDownList)_gridview.FooterRow.FindControl("ddlUserNameAdd")).SelectedValue)
                    , General.GetNullableDateTime(((UserControlDate)_gridview.FooterRow.FindControl("ucAssignedDateAdd")).Text)
                    );
            }

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidUser(ucZone.SelectedZone
                    , ((UserControlDate)_gridview.Rows[nCurrentRow].FindControl("ucAssignedDateEdit")).Text
                    , ((DropDownList)_gridview.Rows[nCurrentRow].FindControl("ddlUserNameEdit")).SelectedValue
                    ))
                {
                    ucError.Visible = true;
                    return;
                }

               PhoenixAdministrationAssetUserMapping.AdminAssetUserMappingUpdate
                    (
                      new Guid(((Label)_gridview.Rows[nCurrentRow].FindControl("lblAdminAssetUserMappingIdEdit")).Text)
                    , new Guid(ViewState["ASSETID"].ToString())
                    , int.Parse(ucZone.SelectedZone)
                    , int.Parse(((DropDownList)_gridview.Rows[nCurrentRow].FindControl("ddlUserNameEdit")).SelectedValue)
                    , General.GetNullableDateTime(((UserControlDate)_gridview.Rows[nCurrentRow].FindControl("ucAssignedDateEdit")).Text)
                    , General.GetNullableDateTime(((UserControlDate)_gridview.Rows[nCurrentRow].FindControl("ucUnAssignedDateEdit")).Text)
                    );
            }

            _gridview.EditIndex = -1;
            BindData();
            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidUser(string Zone, string AssignedDate, string OfficeUserId)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(Zone) == null)
            ucError.ErrorMessage = "Zone is required.";
        if (General.GetNullableDateTime(AssignedDate) == null)
            ucError.ErrorMessage = "Assigned Date is required.";
        if (General.GetNullableInteger(OfficeUserId) == null)
            ucError.ErrorMessage = "User is required.";

        return (!ucError.IsError);
    }
    private bool IsValidZone(string Zone)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(Zone) == null)
            ucError.ErrorMessage = "Zone is required.";
        return (!ucError.IsError);
    }

    protected void gvUserMapping_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);
            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvUserMapping_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        try
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
                ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

                DropDownList UserMapEdit = (DropDownList)e.Row.FindControl("ddlUserNameEdit");
                Label UserStaffid = (Label)e.Row.FindControl("lblUserStaffId");
                if (UserMapEdit != null && UserMapEdit != null)
                {
                    UserMapEdit.DataSource = PhoenixAdministrationAssetUserMapping.OfficeStaffList(General.GetNullableInteger(ViewState["ZONEID"].ToString()));
                    UserMapEdit.DataBind();
                    UserMapEdit.Items.Insert(0, new ListItem("--Select--", "Dummy"));
                    UserMapEdit.SelectedValue = UserStaffid.Text.ToString();
                }

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
                DropDownList ddlUserNameAdd = (DropDownList)e.Row.FindControl("ddlUserNameAdd");
                ddlUserNameAdd.Items.Clear();
                ddlUserNameAdd.DataSource = PhoenixAdministrationAssetUserMapping.OfficeStaffList(General.GetNullableInteger(ViewState["ZONEID"].ToString()));
                ddlUserNameAdd.DataTextField = "FLDNAME";
                ddlUserNameAdd.DataValueField = "FLDOFFICESTAFFID";
                ddlUserNameAdd.DataBind();
                ddlUserNameAdd.Items.Insert(0, new ListItem("--Select--", "Dummy"));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvUserMapping_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentrow = de.RowIndex;
            string mappingid = _gridView.DataKeys[nCurrentrow].Value.ToString();

            PhoenixAdministrationAssetUserMapping.AdminAssetUserMappingDelete(new Guid(mappingid));
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvUserMapping.SelectedIndex = -1;
        gvUserMapping.EditIndex = -1;
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvUserMapping.SelectedIndex = -1;
        gvUserMapping.EditIndex = -1;
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
        gvUserMapping.SelectedIndex = -1;
        gvUserMapping.EditIndex = -1;
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
}
