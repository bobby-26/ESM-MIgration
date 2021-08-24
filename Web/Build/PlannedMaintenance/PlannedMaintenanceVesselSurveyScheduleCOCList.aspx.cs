using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyScheduleCOCList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["ScheduleId"] = Request.QueryString["ScheduleId"];
            ViewState["VesselId"] = Request.QueryString["VesselId"];
            ViewState["PAGENUMBER"] = 1;
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
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCertificatesCOC_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label lblItem = (Label)e.Row.FindControl("lblItem");
                Label lblDescription = (Label)e.Row.FindControl("lblDescription");

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
