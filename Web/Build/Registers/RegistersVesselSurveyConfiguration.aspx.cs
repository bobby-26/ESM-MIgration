using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
public partial class Registers_RegistersVesselSurveyConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Registers/RegistersVesselSurveyConfiguration.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvSurveyConfig')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersSurveyCofig.AccessRights = this.ViewState;
            MenuRegistersSurveyCofig.MenuList = toolbar.Show();
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
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

        string[] alColumns = { "FLDVESSELNAME", "FLDANNIVERSARYDATE", "FLDSURVEYTYPENAME", "FLDLASTSURVEYDATE", "FLDSURVEYSEQUENCE" };
        string[] alCaptions = { "Vessel", "Anniversary Date", "Last Survey Type", "Last Survey Date", "Sequence" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegisterVesselSurveyConfiguration.SurveyConfigurationSearch(
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        General.ShowExcel("Survey Configuration", ds.Tables[0], alColumns, alCaptions, null, "");
    }

    protected void MenuRegistersSurveyCofig_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvSurveyConfig.EditIndex = -1;
                gvSurveyConfig.SelectedIndex = -1;
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

        string[] alColumns = { "FLDVESSELNAME", "FLDANNIVERSARYDATE", "FLDSURVEYTYPENAME", "FLDLASTSURVEYDATE", "FLDSURVEYSEQUENCE" };
        string[] alCaptions = { "Vessel", "Anniversary Date", "Last Survey Type", "Last Survey Date", "Sequence" };

        DataSet ds = PhoenixRegisterVesselSurveyConfiguration.SurveyConfigurationSearch(
            (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvSurveyConfig", "Survey Configuration", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvSurveyConfig.DataSource = ds;
            gvSurveyConfig.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvSurveyConfig);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvSurveyConfig_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
        SetPageNavigator();

    }

    protected void gvSurveyConfig_RowCreated(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow
        //    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        //    && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        //{
        //    e.Row.TabIndex = -1;
        //    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvSurveyConfig, "Edit$" + e.Row.RowIndex.ToString(), false);
        //}

        //SetKeyDownScroll(sender, e);
    }

    protected void gvSurveyConfig_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidSurveyConfig(((UserControlVessel)_gridView.FooterRow.FindControl("ucVesselAdd")).SelectedVessel
                , ((DropDownList)_gridView.FooterRow.FindControl("ddlSurveyTypeAdd")).SelectedValue
                , ((UserControlDate)_gridView.FooterRow.FindControl("ucLastSurveyDateAdd")).Text
                ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertSurveyConfig(
                    Int32.Parse(((UserControlVessel)_gridView.FooterRow.FindControl("ucVesselAdd")).SelectedVessel)
                    , Int32.Parse(((DropDownList)_gridView.FooterRow.FindControl("ddlSurveyTypeAdd")).SelectedValue)
                    , General.GetNullableDateTime(((UserControlDate)_gridView.FooterRow.FindControl("ucLastSurveyDateAdd")).Text)
                );
                BindData();
                SetPageNavigator();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteSurveyConfig(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text));
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvSurveyConfig_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvSurveyConfig_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;
        DataRowView drv = (DataRowView)_gridView.Rows[de.NewEditIndex].DataItem;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();
    }

    protected void gvSurveyConfig_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            if (!IsValidSurveyConfig(((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text
                , ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlSurveyTypeEdit")).SelectedValue
                , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucLastSurveyDateEdit")).Text
                ))
            {
                ucError.Visible = true;
                return;
            }

            UpdateSurveyConfig(
                     Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId")).Text)
                     , Int32.Parse(((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlSurveyTypeEdit")).SelectedValue)
                      , General.GetNullableDateTime(((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucLastSurveyDateEdit")).Text)
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

    protected void gvSurveyConfig_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {

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
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            DropDownList ddlSurveyTypeEdit = (DropDownList)e.Row.FindControl("ddlSurveyTypeEdit");
            Label lblSurveyTypeId = (Label)e.Row.FindControl("lblSurveyTypeId");
            if (ddlSurveyTypeEdit != null && lblSurveyTypeId != null) ddlSurveyTypeEdit.SelectedValue = lblSurveyTypeId.Text.Trim();
        }

        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvSurveyConfig.EditIndex = -1;
        gvSurveyConfig.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvSurveyConfig.EditIndex = -1;
        gvSurveyConfig.SelectedIndex = -1;
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
        gvSurveyConfig.SelectedIndex = -1;
        gvSurveyConfig.EditIndex = -1;
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

    private void InsertSurveyConfig(int VesselId, int SurveyTypeId, DateTime? LastSurveyDate)
    {
        PhoenixRegisterVesselSurveyConfiguration.InsertSurveyConfiguration(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             VesselId
             , SurveyTypeId
             , LastSurveyDate);
    }

    private void UpdateSurveyConfig(int VesselId, int SurveyTypeId, DateTime? LastSurveyDate)
    {
        PhoenixRegisterVesselSurveyConfiguration.UpdateSurveyConfiguration(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             VesselId
             , SurveyTypeId
             , LastSurveyDate);
    }
   
    private bool IsValidSurveyConfig(string VesselId, string SurveyTypeId, string LastSurveyDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(VesselId) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(SurveyTypeId) == null)
            ucError.ErrorMessage = "Last Survey Type is required.";

        if (LastSurveyDate == null || LastSurveyDate == "")
            ucError.ErrorMessage = "Last Survey Date is required.";

        if (General.GetNullableDateTime(LastSurveyDate) > DateTime.Today)
            ucError.ErrorMessage = "Last Survey Date  should be Earlier than Current Date";

        return (!ucError.IsError);
    }

    private void DeleteSurveyConfig(int VesselId)
    {
        PhoenixRegisterVesselSurveyConfiguration.DeleteSurveyConfiguration(PhoenixSecurityContext.CurrentSecurityContext.UserCode, VesselId);
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

}
