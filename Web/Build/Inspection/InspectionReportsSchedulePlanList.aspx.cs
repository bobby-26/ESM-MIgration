using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionReportsSchedulePlanList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Inspection/InspectionReportsSchedulePlanList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSchedulePlan')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionReportsSchedulePlanList.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionReportsSchedulePlanList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuSchedulePlan.AccessRights = this.ViewState;
            MenuSchedulePlan.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["COMPANYID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                VesselConfiguration();
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvc.Get("QMS");

                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                Bind_UserControls(sender, new EventArgs());
            }
        //    BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDREFERENCENO", "FLDINSPECTIONNAME", "FLDLASTDONEDATE", "FLDDUEDATE", "FLDRANGEFROMDATE", "FLDCOMPLETIONDATE", "FLDPORTFROM", "FLDPORTTO", "FLDAUDITOR", "FLDATTENDINGSUPDT", "FLDDEFICIENCYCOUNT", "FLDMAJORNCCOUNT", "FLDNCCOUNT", "FLDOBSCOUNT", "FLDSTATUSNAME", };
        string[] alCaptions = { "Vessel", "Reference Number", "Audit/Inspection", "Last Done Date", "Due Date", "Planned Date", "Completed Date", "Port From", "Port To", "Auditor", "Attending Supdt.", "Deficiency Count", "Major NC Count", "NC Count", "Observations Count", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        string vesselid = "";

        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.InstallCode.ToString();
        else
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

        ds = PhoenixInspectionReportAuditInspection.ReportAuditInspection(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , null
                                                    , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                                    , General.GetNullableGuid(ddlAudit.SelectedValue)
                                                    , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                    , General.GetNullableInteger(ucFromPort.SelectedSeaport)
                                                    , General.GetNullableInteger(ucToPort.SelectedSeaport)
                                                    , General.GetNullableInteger(rblDueOverdue.SelectedValue)
                                                    , General.GetNullableInteger(chkRCANotcompletedYN.Checked == true ? "1" : "0")
                                                    , General.GetNullableInteger(chkCARNotCompletedYN.Checked == true ? "1" : "0")
                                                    , General.GetNullableInteger(ddlPastDateRange.SelectedValue)
                                                    , General.GetNullableInteger(ddlFutureDateRange.SelectedValue)
                                                    , General.GetNullableInteger(ddlStatus.SelectedValue)
                                                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                    , sortexpression
                                                    , sortdirection
                                                    , (int)ViewState["PAGENUMBER"]
                                                    , gvSchedulePlan.PageSize
                                                    , ref iRowCount
                                                    , ref iTotalPageCount);

        General.ShowExcel("Audit/Inspection", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenuSchedulePlan_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["PAGENUMBER"] = 1;
                    BindData();
                }
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                else
                {
                    ucVessel.Enabled = true;
                    ucVessel.SelectedVessel = "";
                }
                ucAuditCategory.SelectedHard = "";
                ddlAudit.SelectedValue = "Dummy";

                rblDueOverdue.SelectedValue = null;

                ddlPastDateRange.SelectedValue = "DUMMY";
                ddlFutureDateRange.SelectedValue = "DUMMY";

                chkRCANotcompletedYN.Checked = false;
                chkCARNotCompletedYN.Checked = false;

                ucFromPort.SelectedSeaport = "";
                ucToPort.SelectedSeaport = "";

                ddlStatus.SelectedValue = "DUMMY";
                BindData();
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

        string[] alColumns = { "FLDVESSELNAME", "FLDREFERENCENO", "FLDINSPECTIONNAME", "FLDLASTDONEDATE", "FLDDUEDATE", "FLDRANGEFROMDATE", "FLDCOMPLETIONDATE", "FLDPORTFROM", "FLDPORTTO", "FLDAUDITOR", "FLDATTENDINGSUPDT", "FLDDEFICIENCYCOUNT", "FLDMAJORNCCOUNT", "FLDNCCOUNT", "FLDOBSCOUNT", "FLDSTATUSNAME", };
        string[] alCaptions = { "Vessel", "Reference Number", "Audit/Inspection", "Last Done Date", "Due Date", "Planned Date", "Completed Date", "Port From", "Port To", "Auditor", "Attending Supdt.", "Deficiency Count", "Major NC Count", "NC Count", "Observations Count", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        string vesselid = "";

        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.InstallCode.ToString();
        else
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        ds = PhoenixInspectionReportAuditInspection.ReportAuditInspection(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , null
                                                                , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                                                , General.GetNullableGuid(ddlAudit.SelectedValue)
                                                                , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                , General.GetNullableInteger(ucFromPort.SelectedSeaport)
                                                                , General.GetNullableInteger(ucToPort.SelectedSeaport)
                                                                , General.GetNullableInteger(rblDueOverdue.SelectedValue)
                                                                , General.GetNullableInteger(chkRCANotcompletedYN.Checked == true ? "1" : "0")
                                                                , General.GetNullableInteger(chkCARNotCompletedYN.Checked == true ? "1" : "0")
                                                                , General.GetNullableInteger(ddlPastDateRange.SelectedValue)
                                                                , General.GetNullableInteger(ddlFutureDateRange.SelectedValue)
                                                                , General.GetNullableInteger(ddlStatus.SelectedValue)
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , gvSchedulePlan.CurrentPageIndex + 1
                                                                , gvSchedulePlan.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvSchedulePlan", "Audit/Inspection", alCaptions, alColumns, ds);

        gvSchedulePlan.DataSource = ds;
        gvSchedulePlan.VirtualItemCount = iRowCount;

    }

    private bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information.";
        bool flag = false;
        if (chkRCANotcompletedYN.Checked == false && chkCARNotCompletedYN.Checked == false)
        {
            if (ddlStatus.SelectedValue == "3" || rblDueOverdue.SelectedValue == "1" || rblDueOverdue.SelectedValue == "2")
            {
                if (ddlStatus.SelectedValue == "3" || rblDueOverdue.SelectedValue == "2")
                {
                    if (General.GetNullableInteger(ddlPastDateRange.SelectedValue) == null)
                        flag = true;
                }
                else if (rblDueOverdue.SelectedValue == "1")
                {
                    if (General.GetNullableInteger(ddlFutureDateRange.SelectedValue) == null)
                        flag = true;
                }
            }
            if (flag == true)
                ucError.ErrorMessage = "Duration is required.";
        }
        return (!ucError.IsError);
    }
    protected void gvSchedulePlan_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvSchedulePlan_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvSchedulePlan, "Select$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvSchedulePlan_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvSchedulePlan_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
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

        if (e.Item is GridDataItem)
        {
            DataRowView dv = (DataRowView)e.Item.DataItem;

            UserControlToolTip ucReferenceNo = (UserControlToolTip)e.Item.FindControl("ucReferenceNo");
            UserControlToolTip ucAuditInspection = (UserControlToolTip)e.Item.FindControl("ucAuditInspection");
            UserControlToolTip ucPortFrom = (UserControlToolTip)e.Item.FindControl("ucPortFrom");
            UserControlToolTip ucPortTo = (UserControlToolTip)e.Item.FindControl("ucPortTo");
            UserControlToolTip ucAuditor = (UserControlToolTip)e.Item.FindControl("ucAuditor");
            UserControlToolTip ucAttendingSupdt = (UserControlToolTip)e.Item.FindControl("ucAttendingSupdt");


            RadLabel lblReferenceNumber = (RadLabel)e.Item.FindControl("lblReferenceNumber");
            RadLabel lblAuditInspection = (RadLabel)e.Item.FindControl("lblAuditInspection");
            RadLabel lblPortFrom = (RadLabel)e.Item.FindControl("lblPortFrom");
            RadLabel lblPortTo = (RadLabel)e.Item.FindControl("lblPortTo");
            RadLabel lblAuditor = (RadLabel)e.Item.FindControl("lblAuditor");
            RadLabel lblAttendingSupdt = (RadLabel)e.Item.FindControl("lblAttendingSupdt");

            if (ucReferenceNo != null && lblReferenceNumber != null)
            {
                lblReferenceNumber.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucReferenceNo.ToolTip + "', 'visible');");
                lblReferenceNumber.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucReferenceNo.ToolTip + "', 'hidden');");
            }
            if (ucAuditInspection != null && lblAuditInspection != null)
            {
                lblAuditInspection.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucAuditInspection.ToolTip + "', 'visible');");
                lblAuditInspection.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucAuditInspection.ToolTip + "', 'hidden');");
            }
            if (ucPortFrom != null && lblPortFrom != null)
            {
                lblPortFrom.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucPortFrom.ToolTip + "', 'visible');");
                lblPortFrom.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucPortFrom.ToolTip + "', 'hidden');");
            }
            if (ucPortTo != null && lblPortTo != null)
            {
                lblPortTo.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucPortTo.ToolTip + "', 'visible');");
                lblPortTo.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucPortTo.ToolTip + "', 'hidden');");
            }
            if (ucAuditor != null && lblAuditor != null)
            {
                lblAuditor.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucAuditor.ToolTip + "', 'visible');");
                lblAuditor.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucAuditor.ToolTip + "', 'hidden');");
            }
            if (ucAttendingSupdt != null && lblAttendingSupdt != null)
            {
                lblAttendingSupdt.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucAttendingSupdt.ToolTip + "', 'visible');");
                lblAttendingSupdt.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucAttendingSupdt.ToolTip + "', 'hidden');");
            }
            LinkButton cmdDeficiencySummary = (LinkButton)e.Item.FindControl("cmdDeficiencySummary");
            RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel lblReviewScheduleId = (RadLabel)e.Item.FindControl("lblReviewScheduleId");

            if (cmdDeficiencySummary != null && lblVesselId != null && lblReviewScheduleId != null)
            {
                cmdDeficiencySummary.Visible = SessionUtil.CanAccess(this.ViewState, cmdDeficiencySummary.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();

                if (dv["FLDCOMPLETEDYN"].ToString() == "0")
                {
                    cmdDeficiencySummary.Controls.Remove(html);
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-eye\"></i></span>";
                    cmdDeficiencySummary.Controls.Add(html);
                }

                cmdDeficiencySummary.Attributes.Add("onclick", "javascript:openNewWindow('Summary','','" + Session["sitepath"] + "/Inspection/InspectionDeficiencySummary.aspx?SOURCEID=" + lblReviewScheduleId.Text + "&VESSELID=" + lblVesselId.Text + "'); return true;");
                if (dv["FLDCOMPLETEDYN"].ToString() == "1")
                {
                    cmdDeficiencySummary.Visible = true;
                }
                else
                {
                    cmdDeficiencySummary.Visible = false;
                }
            }
        }
    }

    protected void gvSchedulePlan_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        // gvSchedulePlan.SelectedIndex = se.NewSelectedIndex;
    }

    protected void Bind_UserControls(object sender, EventArgs e)
    {
        string type = PhoenixCommonRegisters.GetHardCode(1, 148, "AUD");
        ddlAudit.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                        , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                        , null
                                        , 1
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlAudit.DataTextField = "FLDSHORTCODE";
        ddlAudit.DataValueField = "FLDINSPECTIONID";
        ddlAudit.DataBind();
        ddlAudit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    gvSchedulePlan.EditIndex = -1;
    //    gvSchedulePlan.SelectedIndex = -1;
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    gvSchedulePlan.EditIndex = -1;
    //    gvSchedulePlan.SelectedIndex = -1;
    //    int result;
    //    if (Int32.TryParse(txtnopage.Text, out result))
    //    {
    //        ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //        if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //        if (0 >= Int32.Parse(txtnopage.Text))
    //            ViewState["PAGENUMBER"] = 1;

    //        if ((int)ViewState["PAGENUMBER"] == 0)
    //            ViewState["PAGENUMBER"] = 1;

    //        txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //    }
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvSchedulePlan.SelectedIndex = -1;
    //    gvSchedulePlan.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
    //    SetPageNavigator();
    //}

    //private void SetPageNavigator()
    //{
    //    cmdPrevious.Enabled = IsPreviousEnabled();
    //    cmdNext.Enabled = IsNextEnabled();
    //    lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //    lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //    lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
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
    //        return true;

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

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    dt.Rows.Add(dt.NewRow());
    //    gv.DataSource = dt;
    //    gv.DataBind();

    //    int colcount = gv.Columns.Count;
    //    gv.Rows[0].Cells.Clear();
    //    gv.Rows[0].Cells.Add(new TableCell());
    //    gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //    gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //    gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //    gv.Rows[0].Cells[0].Font.Bold = true;
    //    gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //    gv.Rows[0].Attributes["onclick"] = "";
    //}

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    //private void SetRowSelection()
    //{
    //    gvMachineryDamage.SelectedIndex = -1;

    //    for (int i = 0; i < gvMachineryDamage.Rows.Count; i++)
    //    {
    //        if (gvMachineryDamage.DataKeys[i].Value.ToString().Equals(ViewState["MACHINERYDAMAGEID"].ToString()))
    //        {
    //            gvMachineryDamage.SelectedIndex = i;
    //        }
    //    }
    //}

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        // gvSchedulePlan.EditIndex = -1;
        // gvSchedulePlan.SelectedIndex = -1;
        BindData();
        //    SetPageNavigator();
    }

    protected void gvSchedulePlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}

