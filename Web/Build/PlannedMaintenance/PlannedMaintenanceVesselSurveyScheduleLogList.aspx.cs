using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;

public partial class PlannedMaintenanceVesselSurveyScheduleLogList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            gvSurvey.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            if (Filter.CurrentCertificateSurveyVesselFilter == string.Empty)
                Filter.CurrentCertificateSurveyVesselFilter = ddlVessel.SelectedVessel;
            BindData();
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleLogList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSurvey')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleLogFilter.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVesselSurveyCOCList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuSurveyScheduleLog.AccessRights = this.ViewState;
        MenuSurveyScheduleLog.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["VesselId"] = "";
            ViewState["SheduleId"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["CURRENTINDEX"] = 0;
            ViewState["SELECTEDINDEX"] = 0;
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                Filter.CurrentCertificateSurveyVesselFilter = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ddlVessel.Visible = false;
                lblVesselName.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                lblVesselName.Visible = true;
            }
            else
            {                
                ddlVessel.Visible = true;                
                lblVesselName.Visible = false;
            }
            
            if (Filter.CurrentCertificateSurveyVesselFilter != null && Filter.CurrentCertificateSurveyVesselFilter != string.Empty)
                ddlVessel.SelectedVessel = Filter.CurrentCertificateSurveyVesselFilter;
        }
        //BindData();       
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCERTIFICATENAME", "FLDCATEGORYNAME", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME", "FLDSURVEYTYPENAME", "FLDDONEDATE", "FLDSEAPORTNAME", "FLDSURVEYORNAME", "FLDFULLREMARKS", "FLDCOCCOUNT", "FLDCOCSTATUS", "FLDCOMPLETEDBY" };
        string[] alCaptions = { "Certificate", "Category", "Issued", "Expiry", "Issued By", "Type", "Done", "Port", "Surveyor", "Remarks", "COC Count", "COC Status", "Completed By" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        NameValueCollection nvc = Filter.VesselSurveyLogFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection();            
            nvc.Add("IssuedFrom", string.Empty);
            nvc.Add("IssuedTo", string.Empty);
            nvc.Add("IssueFrom", string.Empty);
            nvc.Add("IssueTo", string.Empty);
            nvc.Add("CategoryList", string.Empty);
        }
        DataSet ds = PhoenixPlannedMaintenanceSurveySchedule.SurveyScheduleLogSearch(General.GetNullableInteger(Filter.CurrentCertificateSurveyVesselFilter)
               , General.GetNullableDateTime(nvc.Get("IssuedFrom"))
               , General.GetNullableDateTime(nvc.Get("IssuedTo"))
               , General.GetNullableString(nvc.Get("CategoryList"))
               , 1
               , iRowCount
               , ref iRowCount
               , ref iTotalPageCount
               , General.GetNullableDateTime(nvc.Get("IssueFrom"))
               , General.GetNullableDateTime(nvc.Get("IssueTo"))
               );

        General.ShowExcel("Certificate Log", ds.Tables[0], alColumns, alCaptions, null, "");
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDCERTIFICATENAME", "FLDCATEGORYNAME", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDISSUINGAUTHORITYNAME", "FLDSURVEYTYPENAME", "FLDDONEDATE", "FLDSEAPORTNAME", "FLDSURVEYORNAME", "FLDFULLREMARKS", "FLDCOCCOUNT", "FLDCOCSTATUS", "FLDCOMPLETEDBY" };
        string[] alCaptions = { "Certificate", "Category", "Issued", "Expiry", "Issued By", "Type", "Done", "Port", "Surveyor", "Remarks", "COC Count", "COC Status", "Completed By" };

        try
        {
            NameValueCollection nvc = Filter.VesselSurveyLogFilter;
            if (nvc == null)
            {
                nvc = new NameValueCollection();              
                nvc.Add("CompletedFrom", string.Empty);
                nvc.Add("CompletedTo", string.Empty);
                nvc.Add("IssueFrom", string.Empty);
                nvc.Add("IssueTo", string.Empty);
                nvc.Add("CategoryList", string.Empty);
            }
            DataSet ds = PhoenixPlannedMaintenanceSurveySchedule.SurveyScheduleLogSearch(General.GetNullableInteger(Filter.CurrentCertificateSurveyVesselFilter)
                   , General.GetNullableDateTime(nvc.Get("CompletedFrom"))
                   , General.GetNullableDateTime(nvc.Get("CompletedTo"))
                   , General.GetNullableString(nvc.Get("CategoryList"))
                   , gvSurvey.CurrentPageIndex + 1
                   , gvSurvey.PageSize
                   , ref iRowCount
                   , ref iTotalPageCount
                   , General.GetNullableDateTime(nvc.Get("IssueFrom"))
                   , General.GetNullableDateTime(nvc.Get("IssueTo"))
                   );
            General.SetPrintOptions("gvSurvey", "Certificate Log", alCaptions, alColumns, ds);
            gvSurvey.DataSource = ds;
            gvSurvey.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuSurveyScheduleLog_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName == "EXCEL")
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.VesselSurveyLogFilter = null;
                BindData();
                gvSurvey.Rebind();
                //SetPageNavigator();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceVesselSurveyScheduleFilterLog.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Filter.CurrentCertificateSurveyVesselFilter = PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : ddlVessel.SelectedVessel;
        NameValueCollection nvc = Filter.VesselSurveyLogFilter;
        if (nvc != null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                nvc["VesselId"] = ddlVessel.SelectedVessel;
            else
                nvc["VesselId"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }
    //    BindData();
    //    SetPageNavigator();
    }
    //private void SetPageNavigator()
    //{
    //    try
    //    {
    //        cmdPrevious.Enabled = IsPreviousEnabled();
    //        cmdNext.Enabled = IsNextEnabled();
    //        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

        //private Boolean IsPreviousEnabled()
        //{
        //    int iCurrentPageNumber;
        //    int iTotalPageCount;

        //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        //    if (iTotalPageCount == 0)
        //        return false;

        //    if (iCurrentPageNumber > 1)
        //    {
        //        return true;
        //    }

        //    return false;
        //}

        //private Boolean IsNextEnabled()
        //{
        //    int iCurrentPageNumber;
        //    int iTotalPageCount;

        //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        //    if (iCurrentPageNumber < iTotalPageCount)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        //protected void cmdGo_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int result;
        //        if (Int32.TryParse(txtnopage.Text, out result))
        //        {
        //            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

        //            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
        //                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


        //            if (0 >= Int32.Parse(txtnopage.Text))
        //                ViewState["PAGENUMBER"] = 1;

        //            if ((int)ViewState["PAGENUMBER"] == 0)
        //                ViewState["PAGENUMBER"] = 1;

        //            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        //        }
        //        BindData();
        //        SetPageNavigator();
        //    }
        //    catch (Exception ex)
        //    {
        //        ucError.ErrorMessage = ex.Message;
        //        ucError.Visible = true;
        //    }
        //}

        //protected void PagerButtonClick(object sender, CommandEventArgs ce)
        //{
        //    try
        //    {
        //        gvSurvey.SelectedIndex = -1;
        //        gvSurvey.EditIndex = -1;
        //        if (ce.CommandName == "prev")
        //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        //        else
        //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        //        BindData();
        //        SetPageNavigator();
        //    }
        //    catch (Exception ex)
        //    {
        //        ucError.ErrorMessage = ex.Message;
        //        ucError.Visible = true;
        //    }
        //}
        //private void ShowNoRecordsFound(DataTable dt, GridView gv)
        //{
        //    try
        //    {
        //        dt.Rows.Add(dt.NewRow());
        //        gv.DataSource = dt;
        //        gv.DataBind();

        //        int colcount = gv.Columns.Count;
        //        gv.Rows[0].Cells.Clear();
        //        gv.Rows[0].Cells.Add(new TableCell());
        //        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        //        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        //        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        //        gv.Rows[0].Cells[0].Font.Bold = true;
        //        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        //    }
        //    catch (Exception ex)
        //    {
        //        ucError.ErrorMessage = ex.Message;
        //        ucError.Visible = true;
        //    }
        //}
        //protected void gvSurvey_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
        //{
        //    gvSurvey.SelectedIndex = se.NewSelectedIndex;
        //}

    protected void gvSurvey_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvSurvey_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                LinkButton cmdSvyAtt = (LinkButton)e.Item.FindControl("cmdSvyAtt");
                RadLabel lblAttachmentyn = (RadLabel)e.Item.FindControl("lblAttachmentyn");
                RadLabel lblDtkey = (RadLabel)e.Item.FindControl("lblDtkey");
                RadLabel lblRemarks = (RadLabel)e.Item.FindControl("lblRemarks");
                RadLabel lblCertificate = (RadLabel)e.Item.FindControl("lblCertificate");

                if (lblAttachmentyn != null && lblAttachmentyn.Text != "YES")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    cmdSvyAtt.Controls.Add(html);
                }

                UserControlToolTip ucRemarks = (UserControlToolTip)e.Item.FindControl("ucRemarks");
                if (lblCertificate != null && lblRemarks != null)
                {
                    //lblRemarks.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucRemarks.ToolTip + "', 'visible');");
                    //lblRemarks.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucRemarks.ToolTip + "', 'hidden');");
                    ucRemarks.Position = ToolTipPosition.TopCenter;
                    ucRemarks.TargetControlId = lblRemarks.ClientID;
                }

                if (cmdSvyAtt != null && lblDtkey != null)
                    cmdSvyAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text.Trim() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&type=SURVEYCERTIFICATE'); return false;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
