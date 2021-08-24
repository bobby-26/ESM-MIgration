using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyCertificateList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCertificates.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvCertificates.UniqueID, "Select$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateList.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
        toolbar.AddImageLink("javascript:CallPrint('gvCertificates')", "Print Grid", "icon_print.png", "PRINT");
        MenuSurveyCertificates.AccessRights = this.ViewState;
        MenuSurveyCertificates.MenuList = toolbar.Show();
        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            ViewState["SheduleId"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["SELECTEDINDEX"] = 0;
            ViewState["SheduleId"] = Request.QueryString["ScheduleId"];
            ViewState["VesselId"] = Request.QueryString["VesselId"];
            ViewState["IsLog"] = string.IsNullOrEmpty(Request.QueryString["IsLog"]) ? "" : Request.QueryString["IsLog"];
            PopulateDate(Request.QueryString["VesselId"], Request.QueryString["ScheduleId"]);
        }
        BindData();
        SetPageNavigator();
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
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCERTIFICATECODE", "FLDCERTIFICATENAME", "FLDCERTIFICATENO", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME", "FLDSEAPORTNAME" };
        string[] alCaptions = { "Certificate Code", "Certificates", "Number", "Issue Date", "Expiry Date", "Issuing Authority", "Place of Issue" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataSet ds = PhoenixPlannedMaintenanceSurveySchedule.GetSurveyCertificatesList(General.GetNullableGuid(ViewState["SheduleId"].ToString())
                , General.GetNullableInteger(ViewState["VesselId"].ToString())
               , (int)ViewState["PAGENUMBER"]
               , General.ShowRecords(null)
               , ref iRowCount
               , ref iTotalPageCount
               );

        General.ShowExcel("Certificates List", ds.Tables[0], alColumns, alCaptions, null, "");
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCERTIFICATECODE", "FLDCERTIFICATENAME", "FLDCERTIFICATENO", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME", "FLDSEAPORTNAME" };
        string[] alCaptions = { "Certificate Code", "Certificates", "Number", "Issue Date", "Expiry Date", "Issuing Authority", "Place of Issue" };

        try
        {

            DataSet ds = PhoenixPlannedMaintenanceSurveySchedule.GetSurveyCertificatesList(
                    General.GetNullableGuid(ViewState["SheduleId"].ToString())
                , General.GetNullableInteger(ViewState["VesselId"].ToString())
               , (int)ViewState["PAGENUMBER"]
               , General.ShowRecords(null)
               , ref iRowCount
               , ref iTotalPageCount
               );
            General.SetPrintOptions("gvCertificates", "Certificates List", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCertificates.DataSource = ds.Tables[0];
                gvCertificates.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCertificates);
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }
    protected void MenuSurveyCertificates_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName == "EXCEL")
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
            gvCertificates.SelectedIndex = -1;
            gvCertificates.EditIndex = -1;
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
    protected void gvCertificates_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvCertificates.SelectedIndex = se.NewSelectedIndex;
    }
    protected void gvCertificates_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCertificates, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }


    protected void gvCertificates_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("CMDEDIT"))
            {
                Label lblCertificateId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCertificateId");
                Label lblDtkey = (Label)_gridView.Rows[nCurrentRow].FindControl("lblPrvDtkey");
                string sScheduleId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSheduleId")).Text;
                sScheduleId = sScheduleId.Trim().Equals("") ? ViewState["SheduleId"].ToString() : sScheduleId;
                string sScript = "javascript:parent.Openpopup('codehelp1','','PlannedMaintenanceVesselSurveyCertificateGeneral.aspx?ScheduleId=" + ViewState["SheduleId"].ToString() + "&CertificateId=" + lblCertificateId.Text.Trim() + "&DtKey=" + lblDtkey.Text + "&VesselId=" + ViewState["VesselId"].ToString() + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
            }
            else if (e.CommandName.ToUpper().Equals("CMDCOC"))
            {
                Label lblCertificateId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCertificateId");
                Label lblCertificateType = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCertificateType");
                Label lblDtkey = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDtkey");
                string sScheduleId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSheduleId")).Text;
                sScheduleId = sScheduleId.Trim().Equals("") ? ViewState["SheduleId"].ToString() : sScheduleId;
                string sScript = "javascript:parent.Openpopup('codehelp1','','PlannedMaintenanceVesselSurveyCertificateCOC.aspx?ScheduleId=" + ViewState["SheduleId"].ToString() + "&CertificateId=" + lblCertificateId.Text.Trim() + "&DtKey=" + lblDtkey.Text + "&VesselId=" + ViewState["VesselId"].ToString() + "&Type=" + lblCertificateType.Text + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
            }
            else if (e.CommandName.ToUpper().Equals("REMARKS"))
            {
                Label lblDtkey = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDtkey");
                if (lblDtkey != null)
                {
                    string sScript = "javascript:parent.Openpopup('codehelp1','','PlannedMaintenanceCertificatesRemarks.aspx?DtKey=" + lblDtkey.Text.Trim() + "');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
                }
            }


            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCertificates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton sel = (ImageButton)e.Row.FindControl("cmdSelect");
            if (sel != null) sel.Visible = SessionUtil.CanAccess(this.ViewState, sel.CommandName);

            ImageButton co = (ImageButton)e.Row.FindControl("cmdCoc");
            if (co != null) co.Visible = SessionUtil.CanAccess(this.ViewState, co.CommandName);

            if (sel != null)
                sel.Visible = !ViewState["IsLog"].ToString().Equals("1");
            if (co != null)
                co.Visible = !ViewState["IsLog"].ToString().Equals("1");
            UserControlSeaport ddlPort = (UserControlSeaport)e.Row.FindControl("ddlPort");
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (ddlPort != null) ddlPort.SelectedSeaport = drv["FLDISSUEDPORT"].ToString();

            UserControlAddressType ucIssuingAuthorityEdit = (UserControlAddressType)e.Row.FindControl("ucIssuingAuthorityEdit");
            DataRowView drview = (DataRowView)e.Row.DataItem;
            if (ucIssuingAuthorityEdit != null) ucIssuingAuthorityEdit.SelectedAddress = drview["FLDISSUINGAUTHORITY"].ToString();
        }
    }

    private bool IsValidDetails(
        string certificateno
        , string dateofissue
        , string dateofexpiry
        , string issuingauthority)
    {
        DateTime resultDate;
        Int16 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (certificateno.Trim().Equals(""))
            ucError.ErrorMessage = "Certificate Number is required.";

        if (!DateTime.TryParse(dateofissue, out resultDate))
            ucError.ErrorMessage = "Valid Date of Issue is required.";

        if (!Int16.TryParse(issuingauthority, out resultInt))
            ucError.ErrorMessage = "Issuing Authority is required.";

        if (dateofissue != null && dateofexpiry != null)
        {
            if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry, out resultDate)))
                if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry)))
                    ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
        }

        return (!ucError.IsError);
    }
}
