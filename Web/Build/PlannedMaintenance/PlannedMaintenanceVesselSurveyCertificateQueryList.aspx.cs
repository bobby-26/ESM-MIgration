using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using System.Web;

public partial class PlannedMaintenance_PlannedMaintenanceVesselSurveyCertificateQueryList : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    Table gridTable = (Table)gvCertificates.Controls[0];
    //    ViewState["Category"] = "";
    //    foreach (GridViewRow gv in gvCertificates.Rows)
    //    {
    //        Label lblCertificateCategory = (Label)gv.FindControl("lblCertificateCategory");
    //        Label lblCertificateCategoryId = (Label)gv.FindControl("lblCertificateCategoryId");
    //        if (ViewState["Category"].ToString().Trim().Equals("") || !ViewState["Category"].ToString().Trim().Equals(lblCertificateCategoryId.Text.Trim()))
    //        {
    //            ViewState["Category"] = lblCertificateCategoryId.Text.Trim();
    //            int rowIndex = gridTable.Rows.GetRowIndex(gv);
    //            // Add new group header row  

    //            GridViewRow headerRow = new GridViewRow(rowIndex, rowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);

    //            TableCell headerCell = new TableCell();

    //            headerCell.ColumnSpan = gvCertificates.Columns.Count;

    //            headerCell.Text = @"<font size=""2"" ><b>" + string.Format("{0}", lblCertificateCategory.Text.Trim()) + "</b></font>";

    //            headerCell.CssClass = "GroupHeaderRowStyle";

    //            // Add header Cell to header Row, and header Row to gridTable  

    //            headerRow.Cells.Add(headerCell);
    //            headerRow.HorizontalAlign = HorizontalAlign.Left;
    //            gridTable.Controls.AddAt(rowIndex, headerRow);
    //        }
    //    }
    //    base.Render(writer);
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateQueryList.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbar.AddImageLink("javascript:CallPrint('gvCertificates')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("../PlannedMaintenance/PlannedMaintenanceVesselSurveyCertificateQueryListFilter.aspx", "Filter", "search.png", "FIND");
            MenuSurveyCertificate.AccessRights = this.ViewState;
            MenuSurveyCertificate.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                //PhoenixToolbar toolbarmain = new PhoenixToolbar();
                //toolbarmain.AddButton("Certificates", "CERTIFICATES");
                //toolbarmain.AddButton("Endorsements", "ENDORSEMENTS");

                //MenuSurveyScheduleHeader.AccessRights = this.ViewState;
                //MenuSurveyScheduleHeader.MenuList = toolbarmain.Show();
                //MenuSurveyScheduleHeader.SelectedMenuIndex = 0;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SELECTEDINDEX"] = 0;
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
        string[] alColumns = { "FLDEXCELORDER", "FLDCERTIFICATECODE", "FLDCERTIFICATENAME", "FLDCERTIFICATENO", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME", "FLDSEAPORTNAME" };
        string[] alCaptions = { "S.No", "Certificate Code", "Certificate", "Certificate.No", "Date of Issue", "Date of Expiry", "Issuing Authority", "Place of Issue" };

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
            nvc.Add("ShowNotApplicable", string.Empty);
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
               , null
               , General.GetNullableInteger(nvc.Get("ShowNotApplicable"))
               );
        Response.AddHeader("Content-Disposition", "attachment; filename=Certificates.xls");
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
                if (alColumns[i] == "FLDEXCELORDER")
                    Response.Write("<td align='center'>");
                else
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
        string[] alColumns = { "FLDCERTIFICATECODE", "FLDCERTIFICATENAME", "FLDCERTIFICATENO", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME", "FLDSEAPORTNAME", "FLDREMARKSNAME", "FLDCERTIFICATEREMARKS", "FLDUPDATEAUDITLOG", "FLDATTACHCORRECTYN", "FLDNOTAPPLICABLE", "FLDNOTAPPLICABLEREASON", "FLDNOEXPIRY" };
        string[] alCaptions = { "Certificate Code", "Certificates", "Number", "Issue Date", "Expiry Date", "Issuing By", "Place of Issue", "Full Term / Short term / Interim / Provisional", "Remarks", "Update Audit Log", "Is uploaded Attachment correct", "Not applicable for this vessel", "Reason why not applicable", "No Expiry" };

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
                nvc.Add("EndorsementsYN", string.Empty);
                nvc.Add("ShowNotApplicable", string.Empty);
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
               , null
               , General.GetNullableInteger(nvc.Get("ShowNotApplicable"))
               );
            General.SetPrintOptions("gvCertificates", "Certificates", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //int SelectedIndex = int.Parse(ViewState["SELECTEDINDEX"] != null ? ViewState["SELECTEDINDEX"].ToString() : "0");
                gvCertificates.DataSource = ds.Tables[0];
                gvCertificates.DataBind();
                //gvCertificates.SelectedIndex = SelectedIndex;
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

    protected void gvCertificates_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton cmdCoc = (ImageButton)e.Row.FindControl("cmdCoc");
                ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
                ImageButton cmdSvyAtt = (ImageButton)e.Row.FindControl("cmdSvyAtt");
                Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");
                Label lblScheduleId = (Label)e.Row.FindControl("lblScheduleId");
                Label lblCertificateId = (Label)e.Row.FindControl("lblCertificateId");
                Label lblDtkey = (Label)e.Row.FindControl("lblDtkey");
                UserControlAddressType ucIssuingAuthorityEdit = (UserControlAddressType)e.Row.FindControl("ucIssuingAuthorityEdit");
                UserControlSeaport ddlPort = (UserControlSeaport)e.Row.FindControl("ddlPort");
                UserControlQuick ucRemarks = (UserControlQuick)e.Row.FindControl("ucRemarks");
                Label lblRemarksTypeId = (Label)e.Row.FindControl("lblRemarksTypeId");
                Label lblSeaPortId = (Label)e.Row.FindControl("lblSeaPortId");
                Label lblIssuingAuthorityId = (Label)e.Row.FindControl("lblIssuingAuthorityId");
                UserControlDate txtDateOfExpiryEdit = (UserControlDate)e.Row.FindControl("txtDateOfExpiryEdit");
                CheckBox chkExpiryYN = (CheckBox)e.Row.FindControl("chkExpiryYN");
                CheckBox chkUploadAttachYN = (CheckBox)e.Row.FindControl("chkUploadAttachYN");

                if (cmdSvyAtt != null) cmdSvyAtt.Visible = lblDtkey.Text.Trim().Equals("") ? false : true;

                if (cmdSvyAtt != null)
                {
                    if (lblDtkey != null)
                        cmdSvyAtt.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text.Trim() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&type=SURVEYCERTIFICATE'); return false;");
                }
                if (lblIsAtt != null && lblIsAtt.Text != "YES" && cmdSvyAtt != null)
                    cmdSvyAtt.ImageUrl = Session["images"] + "/no-attachment.png";

                if (chkUploadAttachYN != null && lblIsAtt != null) 
                    chkUploadAttachYN.Enabled = lblIsAtt.Text != "YES" ? false : true;

                if (ucIssuingAuthorityEdit != null && lblIssuingAuthorityId != null)
                {
                    ucIssuingAuthorityEdit.SelectedAddress = lblIssuingAuthorityId.Text.Trim();
                }
                if (ddlPort != null && lblSeaPortId != null)
                {
                    ddlPort.SelectedSeaport = lblSeaPortId.Text.Trim();
                }
                if (ucRemarks != null && lblRemarksTypeId != null)
                {
                    ucRemarks.SelectedQuick = lblRemarksTypeId.Text.Trim();
                }

                if (chkExpiryYN != null && txtDateOfExpiryEdit != null)
                {
                    txtDateOfExpiryEdit.Text = chkExpiryYN.Checked ? "" : txtDateOfExpiryEdit.Text;
                    txtDateOfExpiryEdit.ReadOnly = chkExpiryYN.Checked;
                    txtDateOfExpiryEdit.CssClass = chkExpiryYN.Checked ? "readonlytextbox" : "input";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCertificates_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            ((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtCertificateNoEdit")).Focus();

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCertificates_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            Label lblVesselId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblVesselId");
            Label lblCertificateId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCertificateId");
            Label lblScheduleId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblScheduleId");
            Label lbllCertificateCode = (Label)_gridView.Rows[nCurrentRow].FindControl("lbllCertificateCode");
            Label lblCertificates = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCertificates");
            TextBox txtCertificateNoEdit = (TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCertificateNoEdit");
            UserControlDate txtDateOfIssueEdit = (UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDateOfIssueEdit");
            UserControlDate txtDateOfExpiryEdit = (UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDateOfExpiryEdit");
            UserControlAddressType ucIssuingAuthorityEdit = (UserControlAddressType)_gridView.Rows[nCurrentRow].FindControl("ucIssuingAuthorityEdit");
            UserControlSeaport ddlPort = (UserControlSeaport)_gridView.Rows[nCurrentRow].FindControl("ddlPort");
            UserControlQuick ucRemarks = (UserControlQuick)_gridView.Rows[nCurrentRow].FindControl("ucRemarks");
            TextBox txtRemarks = (TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarks");
            TextBox txtNotapplicableReason = (TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNotapplicableReason");
            CheckBox chkAuditLogYN = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkAuditLogYN");
            CheckBox chkUploadAttachYN = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkUploadAttachYN");
            CheckBox chkVesselapplicableYN = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkVesselapplicableYN");
            CheckBox chkExpiryYN = (CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkExpiryYN");

            if (!IsValidDetails(txtCertificateNoEdit.Text, txtDateOfIssueEdit.Text, txtDateOfExpiryEdit.Text, ucIssuingAuthorityEdit.SelectedAddress,
                       chkVesselapplicableYN.Checked, txtNotapplicableReason.Text.Trim()))
            {
                ucError.Visible = true;
                return;
            }
            
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
    protected void gvCertificates_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
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
    protected void chkExpiryYN_CheckedChange(object sender, EventArgs e)
    {
        try
        {
            CheckBox chkExpiryYN = (CheckBox)sender;
            GridViewRow gv = (GridViewRow)chkExpiryYN.NamingContainer;
            UserControlDate txtDateOfExpiryEdit = (UserControlDate)gv.FindControl("txtDateOfExpiryEdit");

            txtDateOfExpiryEdit.Text = chkExpiryYN.Checked ? "" : txtDateOfExpiryEdit.Text;
            txtDateOfExpiryEdit.ReadOnly = chkExpiryYN.Checked;
            txtDateOfExpiryEdit.CssClass = chkExpiryYN.Checked ? "readonlytextbox" : "input";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidDetails(
       string certificateno
       , string dateofissue
       , string dateofexpiry
       , string issuingauthority
       , bool VesselNotApplicable
       , string Reason)
    {
        DateTime resultDate;
        Int16 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (certificateno.Trim().Equals(""))
            ucError.ErrorMessage = "Certificate Number is required.";

        if (!DateTime.TryParse(dateofissue, out resultDate))
            ucError.ErrorMessage = "Issue Date is required.";
        if (General.GetNullableDateTime(dateofissue) > DateTime.Today)
            ucError.ErrorMessage = "Issue Date should be earlier then current date.";
        if (!Int16.TryParse(issuingauthority, out resultInt))
            ucError.ErrorMessage = "Issuing Authority is required.";

        if (dateofissue != null && dateofexpiry != null)
        {
            if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry, out resultDate)))
                if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry)))
                    ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
        }
        if (VesselNotApplicable && Reason.Trim().Equals(""))
        {
            ucError.ErrorMessage = "Reason for not applicable is required";
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

    //protected void txtDateOfIssueEdit_TextChange(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        UserControlDate txtDateOfIssueEdit = (UserControlDate)sender;
    //        GridViewRow gv = (GridViewRow)txtDateOfIssueEdit.NamingContainer;

    //        Label lblCertificateValidityEdit = (Label)gv.FindControl("lblCertificateValidityEdit");
    //        UserControlDate txtDateOfExpiryEdit = (UserControlDate)gv.FindControl("txtDateOfExpiryEdit");
    //        CheckBox chkExpiryYN = (CheckBox)gv.FindControl("chkExpiryYN");
    //        if (chkExpiryYN != null && chkExpiryYN.Checked == false)
    //        {
    //            if (txtDateOfIssueEdit != null && txtDateOfIssueEdit.Text != null)
    //            {
    //                if (lblCertificateValidityEdit != null && txtDateOfExpiryEdit != null)
    //                {
    //                    DateTime IssueDate = DateTime.Parse(txtDateOfIssueEdit.Text);

    //                    txtDateOfExpiryEdit.Text = IssueDate.AddMonths((lblCertificateValidityEdit.Text != "" ? int.Parse(lblCertificateValidityEdit.Text) : 0)).ToString();
    //                }
    //                txtDateOfExpiryEdit.Focus();
    //            }
    //        }
    //        txtDateOfIssueEdit.Focus();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

}
