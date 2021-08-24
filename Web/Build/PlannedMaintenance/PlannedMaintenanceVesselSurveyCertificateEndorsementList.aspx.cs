using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using System.Web;

public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyCertificateEndorsementList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateEndorsementList.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
        toolbar.AddImageLink("javascript:CallPrint('gvSurveyCertificate')", "Print Grid", "icon_print.png", "PRINT");
        toolbar.AddImageLink("../PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateEndorsementList.aspx", "Filter", "search.png", "FIND");
        MenuSurveyCertificate.AccessRights = this.ViewState;
        MenuSurveyCertificate.MenuList = toolbar.Show();

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        if (!IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Certificates", "CERTIFICATES");
            toolbarmain.AddButton("Endorsements", "ENDORSEMENTS");

            MenuSurveyScheduleHeader.AccessRights = this.ViewState;
            MenuSurveyScheduleHeader.MenuList = toolbarmain.Show();
            MenuSurveyScheduleHeader.SelectedMenuIndex = 1;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SELECTEDINDEX"] = 0;
        }
        BindData();
        SetPageNavigator();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCERTIFICATECODE", "FLDCERTIFICATENAME", "FLDCERTIFICATENO", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDSURVEYNUMBER", "FLDISSUINGAUTHORITYNAME", "FLDSEAPORTNAME", "FLDENDORSEDATE" };
        string[] alCaptions = { "Certificate Code", "Certificate", "Certificate.No", "Date of Issue", "Date of Expiry", "Survey", "Issuing Authority", "Place of Issue", "Endorsement Date" };

        string date = DateTime.Now.ToShortDateString();

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        NameValueCollection nvc = Filter.VesselSurveyCertificateFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection();
            nvc.Add("VesselId", PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
            nvc.Add("IssuedFrom", string.Empty);
            nvc.Add("IssuedTo", string.Empty);
            nvc.Add("SurveyType", string.Empty);
            nvc.Add("SurveyNumber", string.Empty);
            nvc.Add("EndorsementsYN", string.Empty);
        }
        DataSet ds = PhoenixPlannedMaintenanceVesselCertificateCOC.VesselSurveyCertificateList(General.GetNullableInteger(nvc.Get("VesselId"))
           , General.GetNullableInteger(nvc.Get("SurveyType"))
           , General.GetNullableDateTime(nvc.Get("IssuedFrom"))
           , General.GetNullableDateTime(nvc.Get("IssuedTo"))
           , General.GetNullableString(nvc.Get("SurveyNumber"))
               , (int)ViewState["PAGENUMBER"]
               , General.ShowRecords(null)
               , ref iRowCount
               , ref iTotalPageCount
               , General.GetNullableInteger("1")
               );
        Response.AddHeader("Content-Disposition", "attachment; filename=Endorsements.xls");
        Response.ContentType = "application/x-excel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>"+ HttpContext.Current.Session["companyname"].ToString() + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>RECORD OF SHIPS' CERTIFICATES, SURVEYS AND INSPECTIONS</center></h5></td></tr>");
        Response.Write("<tr>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td align='left' style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length - 3).ToString() + "' align='left'>Vessel:" + ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString() + "</td>");
        Response.Write("<td align='left' style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length - 4).ToString() + "' align='left'>Date:" + date + "</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        //General.ShowFilterCriteriaInExcel(ds, FilterCaptions, FilterColumns);
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        ViewState["CategoryId"] = "";
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (ViewState["CategoryId"].ToString().Trim().Equals("") || !ViewState["CategoryId"].ToString().Trim().Equals(dr["FLDCATEGORYID"].ToString()))
            {
                ViewState["CategoryId"] = dr["FLDCATEGORYID"].ToString();
                Response.Write("<tr>");
                Response.Write("<td style='font-family:Arial; font-size:12px; background-color:#FAEBD7; font-weight:bold;' colspan='" + (alColumns.Length).ToString() + "' align='center'>" + dr["FLDCATEGORYNAME"].ToString() + "</td>");
                Response.Write("</tr>");
            }
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                string sValue = dr[alColumns[i]].GetType().ToString().Equals("System.DateTime") ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString();
                Response.Write(sValue);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
        //General.ShowExcel("Survey Certificate List", ds.Tables[0], alColumns, alCaptions, null, "");
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCERTIFICATECODE", "FLDCERTIFICATENAME", "FLDCERTIFICATENO", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDSURVEYNUMBER", "FLDISSUINGAUTHORITYNAME", "FLDSEAPORTNAME", "FLDENDORSEDATE" };
        string[] alCaptions = { "Certificate Code", "Certificate", "Certificate.No", "Date of Issue", "Date of Expiry", "Survey", "Issuing Authority", "Place of Issue", "Endorsement Date" };

        try
        {
            NameValueCollection nvc = Filter.VesselSurveyCertificateFilter;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
                nvc.Add("VesselId", PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
                nvc.Add("IssuedFrom", string.Empty);
                nvc.Add("IssuedTo", string.Empty);
                nvc.Add("SurveyType", string.Empty);
                nvc.Add("SurveyNumber", string.Empty);
                nvc.Add("EndorsementsYN", "1");
            }
            DataSet ds = PhoenixPlannedMaintenanceVesselCertificateCOC.VesselSurveyCertificateList(General.GetNullableInteger(nvc.Get("VesselId"))
               , General.GetNullableInteger(nvc.Get("SurveyType"))
               , General.GetNullableDateTime(nvc.Get("IssuedFrom"))
               , General.GetNullableDateTime(nvc.Get("IssuedTo"))
               , General.GetNullableString(nvc.Get("SurveyNumber"))
               , (int)ViewState["PAGENUMBER"]
               , General.ShowRecords(null)
               , ref iRowCount
               , ref iTotalPageCount
               , General.GetNullableInteger("1")
               );
            General.SetPrintOptions("gvSurveyCertificate", "Endorsements", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //int SelectedIndex = int.Parse(ViewState["SELECTEDINDEX"] != null ? ViewState["SELECTEDINDEX"].ToString() : "0");
                gvSurveyCertificate.DataSource = ds.Tables[0];
                gvSurveyCertificate.DataBind();
                //gvSurveyCertificate.SelectedIndex = SelectedIndex;
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvSurveyCertificate);
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
    protected void MenuSurveyScheduleHeader_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName == "CERTIFICATES")
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateQueryList.aspx");
            }
            else if (dce.CommandName.ToUpper().Equals("ENDORSEMENTS"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateEndorsementList.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuSurveyCertificate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName == "EXCEL")
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.VesselSurveyFilter = null;
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateQueryListFilter.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSurveyCertificate_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lblSurveyNo = (LinkButton)e.Row.FindControl("lblSurveyNo");
                Label lblVesselId = (Label)e.Row.FindControl("lblVesselId");
                Label lblSheduleId = (Label)e.Row.FindControl("lblSheduleId");
                ImageButton cmdSelect = (ImageButton)e.Row.FindControl("cmdSelect");
                Label lblDtkey = (Label)e.Row.FindControl("lblDtkey");
                Label lblCertificateId = (Label)e.Row.FindControl("lblCertificateId");
                string sVesselId = lblVesselId.Text.Trim().Equals("") ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : lblVesselId.Text.Trim();
                lblSurveyNo.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1','','PlannedMaintenanceVesselSurveyScheduleGeneral.aspx?ScheduleId=" + lblSheduleId.Text.Trim() + "&VesselId=" + sVesselId + "&AllowEdit=0" + "');");
                string sScript = "javascript:parent.Openpopup('codehelp1','','PlannedMaintenanceVesselSurveyCertificateGeneral.aspx?ScheduleId=" + lblSheduleId.Text.Trim() + "&CertificateId=" + lblCertificateId.Text.Trim() + "&VesselId=" + sVesselId + "&Dtkey=" + lblDtkey.Text.Trim() + "&ShowOnlyEndorse=1" + "');";
                if (cmdSelect != null)
                    cmdSelect.Attributes.Add("onclick", sScript);
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
            gvSurveyCertificate.SelectedIndex = -1;
            gvSurveyCertificate.EditIndex = -1;
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
    protected void gvSurveyCertificate_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvSurveyCertificate.SelectedIndex = se.NewSelectedIndex;
    }
}
