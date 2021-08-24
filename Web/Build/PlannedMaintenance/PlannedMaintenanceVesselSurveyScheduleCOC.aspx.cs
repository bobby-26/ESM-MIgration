using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyScheduleCOC : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            ViewState["ScheduleId"] = Request.QueryString["ScheduleId"];
            ViewState["VesselId"] = Request.QueryString["VesselId"];
            ViewState["CertificateId"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["IsLog"] = string.IsNullOrEmpty(Request.QueryString["IsLog"]) ? "" : Request.QueryString["IsLog"];
            PopulateDate(Request.QueryString["VesselId"], Request.QueryString["ScheduleId"]);
        }
        BindData();
        SetPageNavigator();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
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
    private void PopulateDate(string sVesselId, string sScheduleId)
    {
        DataSet ds = PhoenixPlannedMaintenanceSurveySchedule.SurveyScheduleEdit(General.GetNullableInteger(sVesselId), General.GetNullableGuid(sScheduleId));
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtSurveyNumber.Text = ds.Tables[0].Rows[0]["FLDSURVEYNUMBER"].ToString();
            txtSurvey.Text = ds.Tables[0].Rows[0]["FLDSURVEYNAME"].ToString();
            txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
            txtSurveyType.Text = ds.Tables[0].Rows[0]["FLDSURVEYTYPENAME"].ToString();
            txtSurveyorName.Text = ds.Tables[0].Rows[0]["FLDSURVEYORNAME"].ToString();
            txtSurveyorDesignation.Text = ds.Tables[0].Rows[0]["FLDSURVEYORDESIG"].ToString();
            txtSuperdentName.Text = ds.Tables[0].Rows[0]["FLDATTENDINGSUPTNAME"].ToString();
            txtCompanyName.Text = ds.Tables[0].Rows[0]["FLDCOMPANYNAME"].ToString();
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        try
        {

        DataSet ds = PhoenixPlannedMaintenanceVesselCertificateCOC.CertificateCOCSearch(
            General.GetNullableInteger(ViewState["VesselId"].ToString())
            , General.GetNullableInteger(ViewState["CertificateId"].ToString())
             , General.GetNullableGuid(ViewState["ScheduleId"].ToString())
           , (int)ViewState["PAGENUMBER"]
           , General.ShowRecords(null)
           , ref iRowCount
           , ref iTotalPageCount
           );
        //General.SetPrintOptions("gvCertificatesCOC", "Certificate COC", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCertificatesCOC.DataSource = ds.Tables[0];
            gvCertificatesCOC.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCertificatesCOC);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        if (!IsPostBack)
        {
            ((TextBox)gvCertificatesCOC.FooterRow.FindControl("txtItemAdd")).Focus();
        }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCertificatesCOC_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }
    protected void gvCertificatesCOC_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvCertificatesCOC.SelectedIndex = se.NewSelectedIndex;
    }
    protected void gvCertificatesCOC_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCertificatesCOC, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }
    protected void gvCertificatesCOC_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            if (!IsValidCOC(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtItemEdit")).Text
                  , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescEdit")).Text
                  , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucDueDateEdit")).Text))
            {
                ucError.Visible = true;
                return;
            }
            UpdateVesselCertificatesCOC(
                  ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCocID")).Text
                  , ViewState["VesselId"].ToString()
                  , ViewState["CertificateId"].ToString()
                  , ViewState["ScheduleId"].ToString()
                  , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtItemEdit")).Text
                  , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescEdit")).Text
                  , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucDueDateEdit")).Text
                  , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStatus")).Text
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
    protected void gvCertificatesCOC_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();

            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtItemEdit")).Focus();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCertificatesCOC_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvCertificatesCOC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
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

                ImageButton cp = (ImageButton)e.Row.FindControl("cmdComplete");
                if (cp != null) cp.Visible = SessionUtil.CanAccess(this.ViewState, cp.CommandName);

                if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
                {
                    ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                }
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                Label lblItem = (Label)e.Row.FindControl("lblItem");
                Label lblDescription = (Label)e.Row.FindControl("lblDescription");



                if (cp != null)
                    cp.Visible = !(lblStatus.Text.Trim().Equals("3") || ViewState["IsLog"].ToString().Equals("1"));
                if (del != null)
                    del.Visible = !(lblStatus.Text.Trim().Equals("3") || ViewState["IsLog"].ToString().Equals("1"));
                if (eb != null)
                    eb.Visible = !(lblStatus.Text.Trim().Equals("3") || ViewState["IsLog"].ToString().Equals("1"));

                UserControlToolTip ucItem = (UserControlToolTip)e.Row.FindControl("ucItem");
                if (lblItem != null)
                {
                    lblItem.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucItem.ToolTip + "', 'visible');");
                    lblItem.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucItem.ToolTip + "', 'hidden');");
                }
                UserControlToolTip ucDescription = (UserControlToolTip)e.Row.FindControl("ucDescription");
                if (lblDescription != null)
                {
                    lblDescription.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucDescription.ToolTip + "', 'visible');");
                    lblDescription.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucDescription.ToolTip + "', 'hidden');");
                }

                Label lblDtkey = (Label)e.Row.FindControl("lblDtkey");
                Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");
                ImageButton cmdSvyAtt = (ImageButton)e.Row.FindControl("cmdSvyAtt");
                if (lblIsAtt != null && lblIsAtt.Text != "YES")
                    cmdSvyAtt.ImageUrl = Session["images"] + "/no-attachment.png";

                if (cmdSvyAtt != null && lblDtkey != null)
                    cmdSvyAtt.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text.Trim() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&type=SURVEYCOC'); ");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                ImageButton ad = (ImageButton)e.Row.FindControl("cmdAdd");
                if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);

                if (ad != null)
                    ad.Visible = !ViewState["IsLog"].ToString().Equals("1");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCertificatesCOC_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidCOC(((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtItemEdit")).Text
                  , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescEdit")).Text
                  , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucDueDateEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateVesselCertificatesCOC(
                   ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCocID")).Text
                   , ViewState["VesselId"].ToString()
                   , ViewState["CertificateId"].ToString()
                   , ViewState["ScheduleId"].ToString()
                   , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtItemEdit")).Text
                   , ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtDescEdit")).Text
                   , ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucDueDateEdit")).Text
                    , ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStatus")).Text
                   );

                _gridView.EditIndex = -1;
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidCOC(((TextBox)_gridView.FooterRow.FindControl("txtItemAdd")).Text
                  , ((TextBox)_gridView.FooterRow.FindControl("txtDescAdd")).Text
                  , ((UserControlDate)_gridView.FooterRow.FindControl("ucDueDateAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertVesselCertificatesCOC(ViewState["VesselId"].ToString()
                                 , ViewState["CertificateId"].ToString()
                                 , ViewState["ScheduleId"].ToString()
                                 , ((TextBox)_gridView.FooterRow.FindControl("txtItemAdd")).Text
                                 , ((TextBox)_gridView.FooterRow.FindControl("txtDescAdd")).Text
                                 , ((UserControlDate)_gridView.FooterRow.FindControl("ucDueDateAdd")).Text
                                 );

                _gridView.EditIndex = -1;
            }
            else if (e.CommandName.ToUpper().Equals("COMPLETE"))
            {
                Label lblCocID = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCocID");

                string sScript = "javascript:parent.Openpopup('codehelp','','PlannedMaintenanceVesselSurveyCOCComplete.aspx?CertificateCOCId=" + lblCocID.Text.Trim()
                       + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
            }
            else if (e.CommandName.Trim().ToUpper().Equals("DELETE"))
            {
                PhoenixPlannedMaintenanceVesselCertificateCOC.DeleteCertificateCOC(new Guid(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCocID")).Text));
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

    private void UpdateVesselCertificatesCOC(string COCId, string VesselId, string CertificateId, string ScheduleId
        , string Item, string Description, string DueDate, string Status)
    {
        PhoenixPlannedMaintenanceVesselCertificateCOC.UpdateCertificateCOC(General.GetNullableGuid(COCId)
            , General.GetNullableInteger(VesselId)
            , General.GetNullableInteger(CertificateId)
            , General.GetNullableGuid(ScheduleId)
            , General.GetNullableString(Item)
            , General.GetNullableString(Description)
            , General.GetNullableDateTime(DueDate)
            , General.GetNullableInteger(Status));
    }
    private void InsertVesselCertificatesCOC(string VesselId, string CertificateId, string ScheduleId
        , string Item, string Description, string DueDate)
    {
        PhoenixPlannedMaintenanceVesselCertificateCOC.InsertCertificateCOC(
            General.GetNullableInteger(VesselId)
           , General.GetNullableInteger(CertificateId)
           , General.GetNullableGuid(ScheduleId)
           , General.GetNullableString(Item)
           , General.GetNullableString(Description)
           , General.GetNullableDateTime(DueDate));
    }
    private bool IsValidCOC(string Item, string Description, string DueDate)
    {
        DateTime resultDate;
        ucError.HeaderMessage = "Please provide the following required information";
        if (Item.Trim().Equals(""))
            ucError.ErrorMessage = "Item is required.";
        if (Description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";
        if (DueDate == null)
            ucError.ErrorMessage = "Due Date is required.";
        else if (DateTime.TryParse(DueDate, out resultDate) && DateTime.Parse(DueDate) < DateTime.Today)
        {
            ucError.ErrorMessage = "Due Date Should be Later then Current Date";
        }
        return (!ucError.IsError);
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
            gvCertificatesCOC.SelectedIndex = -1;
            gvCertificatesCOC.EditIndex = -1;
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

}
