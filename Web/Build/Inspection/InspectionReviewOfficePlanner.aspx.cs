using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionReviewOfficePlanner : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                ViewState["COMPANYID"] = nvc.Get("QMS");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionReviewOfficePlanner.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPlanner')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionReviewOfficePlannerFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionReviewOfficePlanner.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Inspection/InspectionReviewOfficePlannerManual.aspx')", "Manual Office Audit/Inspection", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuScheduleGroup.AccessRights = this.ViewState;
            MenuScheduleGroup.MenuList = toolbar.Show();
            // MenuScheduleGroup.SetTrigger(pnlBudgetGroup);


            if (!IsPostBack)
            {
                ucConfirm.Attributes.Add("style", "display:none");
                ucConfirmDelete.Attributes.Add("style", "display:none");
                gvPlanner.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            {
                //  MenuGeneral.TabStrip = "true";                
                toolbarmain.AddButton("Office Audit/Inspection", "OFFICEAUDIT", ToolBarDirection.Right);
                toolbarmain.AddButton("Audit/Inspection", "VESSELAUDIT", ToolBarDirection.Right);
                MenuGeneral.AccessRights = this.ViewState;
                MenuGeneral.MenuList = toolbarmain.Show();
                MenuGeneral.SelectedMenuIndex = 0;
            }
            else
            {
                MenuGeneral.AccessRights = this.ViewState;
                MenuGeneral.Visible = false;
            }
            //if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            //{
                
            //}
           // BindData();
            // SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            if ((PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE")))
            {
                Response.Redirect("../Inspection/InspectionAuditReviewPlannerFilter.aspx");
            }
            else
            {
                Response.Redirect("../Inspection/InspectionReviewOfficePlannerFilter.aspx");
            }
        }
        if (CommandName.ToUpper().Equals("VESSELAUDIT"))
        {
            Response.Redirect("../Inspection/InspectionReviewPlanner.aspx");
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int companyid = -1;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCOMPANYNAME", "FLDISMANUAL", "FLDINSPECTIONSHORTCODE", "FLDINSPECTIONCATEGORY", "FLDLASTDONEDATE", "FLDDUEDATE",
                                 "FLDPLANNEDDATE", "FLDATTENDINGSUPT", "FLDEXTERNALINSPECTORNAME", "FLDEXTERNALINSPECTORORGANISATION", "FLDSTATUSNAME" };
        string[] alCaptions = { "Company", "M/C", "Audit/Inspection", "Category", "Last Done", "Due", "Planned",
                                  "Attending Supt", "External Auditor", "Organization", "Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentReviewOfficePlannerFilter;

        if (nvc != null)
            companyid = (General.GetNullableInteger(nvc.Get("ucCompanySelect")) == null) ? -1 : int.Parse(nvc.Get("ucCompanySelect").ToString());


        ds = PhoenixInspectionAuditOfficeSchedule.ReviewOfficePlannerSearch(
                                                   nvc != null ? General.GetNullableInteger(nvc.Get("ucCompanySelect")) : null,
                                                   nvc != null ? General.GetNullableGuid(nvc.Get("ucAudit")) : null,
                                                   sortexpression, sortdirection,
                                                    gvPlanner.CurrentPageIndex + 1,
                                                    gvPlanner.PageSize,
                                                   ref iRowCount,
                                                   ref iTotalPageCount,
                                                   nvc != null ? General.GetNullableInteger(nvc.Get("ucAuditCategory")) : null,
                                                   nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : null,
                                                   nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : null,
                                                   nvc != null ? General.GetNullableInteger(nvc.Get("ddlPlanned")) : null,
                                                   nvc != null ? General.GetNullableInteger(nvc.Get("ucCharterer")) : null,
                                                   nvc != null ? General.GetNullableString(nvc.Get("txtExternalInspector")) : null,
                                                   nvc != null ? General.GetNullableString(nvc.Get("txtExternalOrganization")) : null,
                                                   nvc != null ? General.GetNullableInteger(nvc.Get("ddlInspectorName")) : null,
                                                   nvc != null ? General.GetNullableDateTime(nvc.Get("txtPlannedFrom")) : null,
                                                   nvc != null ? General.GetNullableDateTime(nvc.Get("txtPlannedTo")) : null,
                                                   General.GetNullableInteger(ViewState["COMPANYID"].ToString()));


        Response.AddHeader("Content-Disposition", "attachment; filename=Office_Audit_Inspection_Schedule.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Office Audit / Inspection Schedule</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void BudgetGroup_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvPlanner.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentReviewOfficePlannerFilter = null;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvPlanner.Rebind();
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
        int companyid = -1;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDCOMPANYNAME", "FLDISMANUAL", "FLDINSPECTIONSHORTCODE", "FLDINSPECTIONCATEGORY", "FLDLASTDONEDATE", "FLDDUEDATE",
                                 "FLDPLANNEDDATE", "FLDATTENDINGSUPT", "FLDEXTERNALINSPECTORNAME", "FLDEXTERNALINSPECTORORGANISATION", "FLDSTATUSNAME" };
        string[] alCaptions = { "Company", "M/C", "Audit/Inspection", "Category", "Last Done", "Due", "Planned",
                                  "Attending Supt", "External Auditor", "Organization", "Status" };

        NameValueCollection nvc = Filter.CurrentReviewOfficePlannerFilter;

        if (nvc != null)
            companyid = (General.GetNullableInteger(nvc.Get("ucCompanySelect")) == null) ? -1 : int.Parse(nvc.Get("ucCompanySelect").ToString());


        DataSet ds = PhoenixInspectionAuditOfficeSchedule.ReviewOfficePlannerSearch(
                                                   nvc != null ? General.GetNullableInteger(nvc.Get("ucCompanySelect")) : null,
                                                   nvc != null ? General.GetNullableGuid(nvc.Get("ucAudit")) : null,
                                                   sortexpression, sortdirection,
                                                  gvPlanner.CurrentPageIndex + 1,
                                                    gvPlanner.PageSize,
                                                   ref iRowCount,
                                                   ref iTotalPageCount,
                                                   nvc != null ? General.GetNullableInteger(nvc.Get("ucAuditCategory")) : null,
                                                   nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : null,
                                                   nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : null,
                                                   nvc != null ? General.GetNullableInteger(nvc.Get("ddlPlanned")) : null,
                                                   nvc != null ? General.GetNullableInteger(nvc.Get("ucCharterer")) : null,
                                                   nvc != null ? General.GetNullableString(nvc.Get("txtExternalInspector")) : null,
                                                   nvc != null ? General.GetNullableString(nvc.Get("txtExternalOrganization")) : null,
                                                   nvc != null ? General.GetNullableInteger(nvc.Get("ddlInspectorName")) : null,
                                                   nvc != null ? General.GetNullableDateTime(nvc.Get("txtPlannedFrom")) : null,
                                                   nvc != null ? General.GetNullableDateTime(nvc.Get("txtPlannedTo")) : null,
                                                   General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        General.SetPrintOptions("gvPlanner", "Office Audit / Inspection Schedule", alCaptions, alColumns, ds);

        gvPlanner.DataSource = ds;
        gvPlanner.VirtualItemCount = iRowCount;

    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        gvPlanner.Rebind();
    }

    protected void gvPlanner_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("CREATESCHEDULE"))
            {
                RadLabel lblDueDate = (RadLabel)e.Item.FindControl("lblDueDate");
                string strduedate = lblDueDate.Text;

                if (General.GetNullableDateTime(strduedate) == null)
                {
                    ucError.ErrorMessage = "Due date is required to create the schedule.";
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ViewState["CURRENTROW"] = e.Item.ItemIndex;
                    RadWindowManager1.RadConfirm("Are you sure to plan this Office audit/inspection?", "Confirm", 320, 150, null, "Audit/Inspection Plan");
                }
            }

            else if (e.CommandName.ToUpper().Equals("UNPLAN"))
            {
                RadLabel lblPlannerid = (RadLabel)e.Item.FindControl("lblPlannerId");
                RadLabel lblScheduleId = (RadLabel)e.Item.FindControl("lblScheduleId");

                PhoenixInspectionAuditSchedule.UnPlanInspection(General.GetNullableGuid(lblPlannerid.Text), General.GetNullableGuid(lblScheduleId.Text));

                ucStatus.Text = "Office Audit/Inspection is un-planned successfully.";

                gvPlanner.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["CURRENTROW"] = e.Item.ItemIndex;
                //ucConfirmDelete.Visible = true;
                RadWindowManager1.RadConfirm("When the schedule is deleted then all the deficiencies will also be deleted if recorded. Do you want to continue..?", "ConfirmDelete", 320, 150, null, "Delete");
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadLabel lblScheduleId = (RadLabel)e.Item.FindControl("lblScheduleId");
                RadLabel lblCategoryId = (RadLabel)e.Item.FindControl("lblCategoryId");
                RadLabel lblInspectingCompanyid = (RadLabel)e.Item.FindControl("lblInspectingCompanyid");
                RadComboBox ddlInternalInspectorEdit = (RadComboBox)e.Item.FindControl("ddlInternalInspectorEdit");

                if (!IsValidPlan(((RadLabel)e.Item.FindControl("lblStatusId")).Text,
                    ((UserControlDate)e.Item.FindControl("ucPlannedDateEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                string category = "";
                string organisation = "";
                if (lblCategoryId != null && lblCategoryId.Text != "")
                    category = lblCategoryId.Text;

                organisation = General.GetNullableString(((RadTextBox)e.Item.FindControl("txtOrganisationEdit")).Text);

                PhoenixInspectionAuditOfficeSchedule.InsertReviewOfficePlanner(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , int.Parse(((RadLabel)e.Item.FindControl("lblCompanyId")).Text)
                            , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblInspectionId")).Text)
                            , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblScheduleId")).Text)
                            , General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblPlannerId")).Text)
                            , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucLastDoneDateEdit")).Text)
                            , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucDueDateEdit")).Text)
                            , General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("ucPlannedDateEdit")).Text)
                            , (category == PhoenixCommonRegisters.GetHardCode(1, 144, "INT")) ? General.GetNullableInteger(ddlInternalInspectorEdit.SelectedValue) : null
                            , General.GetNullableString(((RadTextBox)e.Item.FindControl("txtExternalAuditorEdit")).Text)
                            , (category == PhoenixCommonRegisters.GetHardCode(1, 144, "INT")) ? "" : null
                            , (category == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")) ? organisation : null
                            , (category == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")) ? General.GetNullableInteger(ddlInternalInspectorEdit.SelectedValue) : null
                            , General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblIsManual")).Text)
                            , General.GetNullableGuid(lblInspectingCompanyid.Text)
                        );
                BindData();
                gvPlanner.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPlanner_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dv = (DataRowView)e.Item.DataItem;
            Image imgFlag = e.Item.FindControl("imgFlag") as Image;
            RadLabel lblDueDate = (RadLabel)e.Item.FindControl("lblDueDate");
            UserControlToolTip ucToolTipDueDate = (UserControlToolTip)e.Item.FindControl("ucToolTipDueDate");
            if (dv["FLDDUEOVERDUEYN"].ToString().Equals("3"))
            {
                lblDueDate.Attributes["style"] = "color:Red !important";
                lblDueDate.ToolTip = "Overdue";
                lblDueDate.Font.Bold = true;
            }
            else if (dv["FLDDUEOVERDUEYN"].ToString().Equals("2"))
            {
                lblDueDate.Attributes["style"] = "color:darkorange !important";
                lblDueDate.ToolTip = "Due within 30 days";
                lblDueDate.Font.Bold = true;
            }
            else if (dv["FLDDUEOVERDUEYN"].ToString().Equals("1"))
            {
                lblDueDate.Attributes["style"] = "color:Green !important";
                lblDueDate.ToolTip = "Due within 60 days";
                lblDueDate.Font.Bold = true;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            RadLabel lblPlanStatusId = (RadLabel)e.Item.FindControl("lblStatusId");
            RadLabel lblPlanCompany = (RadLabel)e.Item.FindControl("lblCompanyId");
            RadLabel lblPlanInspId = (RadLabel)e.Item.FindControl("lblInspectionId");
            if (eb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                    eb.Visible = false;

                eb.Attributes.Add("onclick", "javascript:openNewWindow('Report','','" + Session["sitepath"] + "/Inspection/InspectionOfficeAuditScheduleUpdate.aspx?PSCHEID=" + dv["FLDREVIEWPLANNERID"].ToString() + "&STATUS=" + lblPlanStatusId.Text + "&CID=" + lblPlanCompany.Text + "&INSID=" + lblPlanInspId.Text + "'); return true;");
            }

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, sb.CommandName))
                    sb.Visible = false;
            }

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cb.CommandName))
                    cb.Visible = false;
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    db.Visible = false;
                if (string.IsNullOrEmpty(dv["FLDREVIEWPLANNERID"].ToString()))
                    db.Visible = false;
            }

            LinkButton imgCreateSchedule = (LinkButton)e.Item.FindControl("imgCreateSchedule");

            LinkButton cmdReport = (LinkButton)e.Item.FindControl("cmdReport");
            if (cmdReport != null)
            {
                if (dv["FLDSCHEDULEID"] != null && dv["FLDSCHEDULEID"].ToString() != "")
                    cmdReport.Attributes.Add("onclick", "javascript:openNewWindow('Report','','" + Session["sitepath"] + "/Inspection/InspectionAuditOfficeRecordGeneral.aspx?AUDITSCHEDULEID=" + dv["FLDSCHEDULEID"].ToString() + "'); return true;");
            }

            RadLabel lblCompanyId = (RadLabel)e.Item.FindControl("lblCompanyId");
            UserControlDate ucPlannedDateEdit = (UserControlDate)e.Item.FindControl("ucPlannedDateEdit");
            RadComboBox ddlInternalInspectorEdit = (RadComboBox)e.Item.FindControl("ddlInternalInspectorEdit");
            RadTextBox txtExternalAuditorEdit = (RadTextBox)e.Item.FindControl("txtExternalAuditorEdit");
            RadTextBox txtOrganisationEdit = (RadTextBox)e.Item.FindControl("txtOrganisationEdit");

            RadLabel lblPlannedDateEdit = (RadLabel)e.Item.FindControl("lblPlannedDateEdit");
            RadLabel lblInspectorEdit = (RadLabel)e.Item.FindControl("lblInspectorEdit");
            RadLabel lblExternalAuditorEdit = (RadLabel)e.Item.FindControl("lblExternalAuditorEdit");
            RadLabel lblOrganisationEdit = (RadLabel)e.Item.FindControl("lblOrganisationEdit");
            RadLabel lblCategoryId = (RadLabel)e.Item.FindControl("lblCategoryId");

            if (ddlInternalInspectorEdit != null)
            {
                DataSet ds = PhoenixInspectionAuditSchedule.AuditInternalInspectorSearch(null);
                ddlInternalInspectorEdit.DataSource = ds.Tables[0];
                ddlInternalInspectorEdit.DataTextField = "FLDDESIGNATIONNAME";
                ddlInternalInspectorEdit.DataValueField = "FLDUSERCODE";
                ddlInternalInspectorEdit.DataBind();
                ddlInternalInspectorEdit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                ddlInternalInspectorEdit.SelectedValue = dv["FLDINTERNALINSPECTORID"].ToString();
            }

            RadLabel lblStatusId = (RadLabel)e.Item.FindControl("lblStatusId");
            if (lblStatusId != null && lblStatusId.Text != "")
            {
                if (imgCreateSchedule != null) imgCreateSchedule.Visible = false;
            }
            else
            {
                if (cmdReport != null) cmdReport.Visible = false;
            }

            if (lblStatusId != null && lblStatusId.Text == PhoenixCommonRegisters.GetHardCode(1, 146, "ASG"))
            {
                if (ucPlannedDateEdit != null) ucPlannedDateEdit.Visible = true;
                if (ddlInternalInspectorEdit != null) ddlInternalInspectorEdit.Visible = true;
                if (txtExternalAuditorEdit != null) txtExternalAuditorEdit.Visible = true;
                if (txtOrganisationEdit != null) txtOrganisationEdit.Visible = true;

                if (lblPlannedDateEdit != null) lblPlannedDateEdit.Visible = false;

                if (lblInspectorEdit != null) lblInspectorEdit.Visible = false;
                if (lblExternalAuditorEdit != null) lblExternalAuditorEdit.Visible = false;
                if (lblOrganisationEdit != null) lblOrganisationEdit.Visible = false;
            }
            else
            {
                if (ucPlannedDateEdit != null) ucPlannedDateEdit.Visible = false;

                if (ddlInternalInspectorEdit != null) ddlInternalInspectorEdit.Visible = false;
                if (txtExternalAuditorEdit != null) txtExternalAuditorEdit.Visible = false;
                if (txtOrganisationEdit != null) txtOrganisationEdit.Visible = false;

                if (lblPlannedDateEdit != null) lblPlannedDateEdit.Visible = true;

                if (lblInspectorEdit != null) lblInspectorEdit.Visible = true;
                if (lblExternalAuditorEdit != null) lblExternalAuditorEdit.Visible = true;
                if (lblOrganisationEdit != null) lblOrganisationEdit.Visible = true;
            }

            RadLabel lblInspector = (RadLabel)e.Item.FindControl("lblInspector");
            if (lblInspector != null)
            {
                UserControlToolTip ucToolTipInspector = (UserControlToolTip)e.Item.FindControl("ucToolTipInspector");
                ucToolTipInspector.Position = ToolTipPosition.TopCenter;
                ucToolTipInspector.TargetControlId = lblInspector.ClientID;
            }

            RadLabel lblExternalAuditor = (RadLabel)e.Item.FindControl("lblExternalAuditor");
            if (lblExternalAuditor != null)
            {
                UserControlToolTip ucToolTipExternalAuditor = (UserControlToolTip)e.Item.FindControl("ucToolTipExternalAuditor");
                ucToolTipExternalAuditor.Position = ToolTipPosition.TopCenter;
                ucToolTipExternalAuditor.TargetControlId = lblExternalAuditor.ClientID;
            }

            RadLabel lblOrganisation = (RadLabel)e.Item.FindControl("lblOrganisation");
            if (lblOrganisation != null)
            {
                UserControlToolTip ucToolTipOrganisation = (UserControlToolTip)e.Item.FindControl("ucToolTipOrganisation");
                ucToolTipOrganisation.Position = ToolTipPosition.TopCenter;
                ucToolTipOrganisation.TargetControlId = lblOrganisation.ClientID;
            }

            LinkButton imgUnPlan = (LinkButton)e.Item.FindControl("imgUnPlan");

            if (dv["FLDSCHEDULEID"] != null && dv["FLDSCHEDULEID"].ToString() != "" && imgUnPlan != null)
            {
                imgUnPlan.Visible = true;

                if (!SessionUtil.CanAccess(this.ViewState, imgUnPlan.CommandName))
                    imgUnPlan.Visible = false;

                imgUnPlan.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you Sure you want to UnPlan?'); return false;");
            }

            if (imgCreateSchedule != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgCreateSchedule.CommandName))
                    imgCreateSchedule.Visible = false;
            }

            if (cmdReport != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdReport.CommandName))
                    cmdReport.Visible = false;
            }

            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    private bool IsValidPlan(string status, string planneddate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (status == PhoenixCommonRegisters.GetHardCode(1, 146, "ASG"))
        {
            //if (General.GetNullableDateTime(planneddate) == null)
            //    ucError.ErrorMessage = "Planned Date is required.";            
        }

        return (!ucError.IsError);
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            int nCurrentRow = int.Parse(ViewState["CURRENTROW"].ToString());

            RadLabel lblDueDate = (RadLabel)gvPlanner.Items[nCurrentRow].FindControl("lblDueDate");
            string strduedate = lblDueDate.Text;

            RadLabel lblLastDoneDate = (RadLabel)gvPlanner.Items[nCurrentRow].FindControl("lblLastDoneDate");
            RadLabel lblPlannerId = (RadLabel)gvPlanner.Items[nCurrentRow].FindControl("lblPlannerId");
            RadLabel lblInspectionId = (RadLabel)gvPlanner.Items[nCurrentRow].FindControl("lblInspectionId");
            RadLabel lblCompanyID = (RadLabel)gvPlanner.Items[nCurrentRow].FindControl("lblCompanyId");
            RadLabel lblInspectingCompanyid = (RadLabel)gvPlanner.Items[nCurrentRow].FindControl("lblInspectingCompanyid");

            PhoenixInspectionAuditOfficeSchedule.ReviewOfficeScheduleInsert(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , int.Parse(lblCompanyID.Text)
                            , new Guid(lblInspectionId.Text)
                            , lblLastDoneDate != null ? General.GetNullableDateTime(lblLastDoneDate.Text) : null
                            , DateTime.Parse(strduedate)
                            , null
                            , null
                            , null
                            , null
                            , null
                            , lblPlannerId != null ? General.GetNullableGuid(lblPlannerId.Text) : null
                            , null
                            , General.GetNullableGuid(lblInspectingCompanyid.Text)
                            );

            ucStatus.Text = " Office Audit/Inspection is planned successfully.";
            gvPlanner.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int nCurrentRow = int.Parse(ViewState["CURRENTROW"].ToString());

            RadLabel lblPlannerid = (RadLabel)gvPlanner.Items[nCurrentRow].FindControl("lblPlannerId");
            RadLabel lblScheduleId = (RadLabel)gvPlanner.Items[nCurrentRow].FindControl("lblScheduleId");
            RadLabel lblCompanyId = (RadLabel)gvPlanner.Items[nCurrentRow].FindControl("lblCompanyId");

            PhoenixInspectionAuditSchedule.DeleteAuditPlanAndSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
               0, General.GetNullableGuid(lblPlannerid.Text), General.GetNullableGuid(lblScheduleId.Text));

            ucStatus.Text = "Office Audit/Planner is deleted successfully.";
            gvPlanner.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void ucLastDoneDateEdit_TextChanged(object sender, EventArgs e)
    {
        //UserControlDate ucLastDoneDate = (UserControlDate)sender;
        //GridViewRow row = (GridViewRow)ucLastDoneDate.Parent.Parent;
        //ucLastDoneDate = (UserControlDate)row.FindControl("ucLastDoneDateEdit");
        //UserControlDate ucDueDate = (UserControlDate)row.FindControl("ucDueDateEdit");
        //RadLabel lblInspectionIdEdit = (RadLabel)row.FindControl("lblInspectionIdEdit");
        //int frequency = 0;
        //DataSet ds = PhoenixInspection.EditInspection(new Guid(lblInspectionIdEdit.Text));
        //if (ds.Tables[0].Rows.Count > 0)
        //    frequency = int.Parse(ds.Tables[0].Rows[0]["FLDFREQUENCYINMONTHS"].ToString());

        //if (ucLastDoneDate != null && General.GetNullableDateTime(ucLastDoneDate.Text) != null)
        //{
        //    DateTime dtLastDoneDate = Convert.ToDateTime(ucLastDoneDate.Text);
        //    DateTime dtDueDate = dtLastDoneDate.AddMonths(frequency);
        //    ucDueDate.Text = dtDueDate.ToString();
        //}
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvPlanner.Rebind();
    }

    protected void gvPlanner_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPlanner.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPlanner_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
