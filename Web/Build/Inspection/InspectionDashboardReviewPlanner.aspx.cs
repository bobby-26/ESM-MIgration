using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionDashboardReviewPlanner : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardReviewPlanner.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPlanner')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','', '" + Session["sitepath"] + "/Inspection/InspectionDashboardReviewPlannerFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardReviewPlanner.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuScheduleGroup.AccessRights = this.ViewState;
            MenuScheduleGroup.MenuList = toolbar.Show();            

            if (!IsPostBack)
            {
                InspectionFilter.CurrentAuditInspectionDashboardFilter = null;

                VesselConfiguration();
                ViewState["ASG"] = PhoenixCommonRegisters.GetHardCode(1, 146, "ASG");

                ViewState["COMPANYID"] = "";
                ViewState["VESSELID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvc.Get("QMS");

                ucConfirm.Attributes.Add("style", "display:none");
                ucConfirmDelete.Attributes.Add("style", "display:none");
                gvPlanner.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["Type"] = "";
                ViewState["ExternalYN"] = "";

                if (!string.IsNullOrEmpty(Request.QueryString["STATUS"]))
                {
                    ViewState["STATUS"] = Request.QueryString["STATUS"];
                }
                else
                    ViewState["STATUS"] = "";

                if (Request.QueryString["vslid"] != null && Request.QueryString["vslid"].ToString() != string.Empty)
                    ViewState["VESSELID"] = Request.QueryString["vslid"].ToString();

                if (Request.QueryString["Type"] != null && Request.QueryString["Type"].ToString() != string.Empty)
                    ViewState["Type"] = Request.QueryString["Type"].ToString();

                if (Request.QueryString["ExternalYN"] != null && Request.QueryString["ExternalYN"].ToString() != string.Empty)
                    ViewState["ExternalYN"] = Request.QueryString["ExternalYN"].ToString();
            }
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
    
    }
    protected void Rebind()
    {
        gvPlanner.SelectedIndexes.Clear();
        gvPlanner.EditIndexes.Clear();
        gvPlanner.DataSource = null;
        gvPlanner.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDISMANUAL", "FLDINSPECTIONSHORTCODE", "FLDINSPECTIONCATEGORY", "FLDLASTDONEDATE", "FLDDUEDATE",
                                 "FLDPLANNEDDATE", "FLDFROMPORT", "FLDTOPORT", "FLDATTENDINGSUPT", "FLDEXTERNALINSPECTORNAME", "FLDEXTERNALINSPECTORORGANISATION", "FLDSTATUSNAME" };
        string[] alCaptions = { "Vessel", "M/C", "Audit/Inspection", "Category", "Last Done", "Due","Planned", "From Port", "To Port",
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

        NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
        NameValueCollection Dashboardnvc = InspectionFilter.CurrentAuditInspectionDashboardFilter;

        ds = PhoenixInspectionOfficeDashboard.DashboardShipAuditPlannerSearch(General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null))
                      , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["Owner"] : null))
                      , General.GetNullableString(ViewState["STATUS"].ToString())
                      , sortexpression
                      , sortdirection
                      , 1
                      , iRowCount
                      , ref iRowCount
                      , ref iTotalPageCount
                      , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucAudit"] : null)
                      , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucFromPort"] : null)
                      , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucToPort"] : null)
                      , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["chkAtSea"] : null)
                      , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucFrom"] : null)
                      , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucTo"] : null)
                      , General.GetNullableString(ViewState["Type"].ToString())
                      , General.GetNullableInteger(ViewState["ExternalYN"].ToString())
                      , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

        Response.AddHeader("Content-Disposition", "attachment; filename=Audit/Inspection_Schedule.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Audit / Inspection Schedule</h3></td>");
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
                InspectionFilter.CurrentAuditInspectionDashboardFilter = null;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDVESSELNAME", "FLDISMANUAL", "FLDINSPECTIONSHORTCODE", "FLDINSPECTIONCATEGORY", "FLDLASTDONEDATE", "FLDDUEDATE",
                                 "FLDPLANNEDDATE", "FLDFROMPORT", "FLDTOPORT", "FLDATTENDINGSUPT", "FLDEXTERNALINSPECTORNAME", "FLDEXTERNALINSPECTORORGANISATION", "FLDSTATUSNAME" };
        string[] alCaptions = { "Vessel", "M/C", "Audit/Inspection", "Category", "Last Done", "Due","Planned", "From Port", "To Port",
                                  "Attending Supt", "External Auditor", "Organization", "Status" };

        NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
        NameValueCollection Dashboardnvc = InspectionFilter.CurrentAuditInspectionDashboardFilter;

        DataSet ds = PhoenixInspectionOfficeDashboard.DashboardShipAuditPlannerSearch(General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null))
                      , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["Owner"] : null))
                      , General.GetNullableString(ViewState["STATUS"].ToString())
                      , sortexpression
                      , sortdirection
                      , gvPlanner.CurrentPageIndex+1
                      , gvPlanner.PageSize
                      , ref iRowCount
                      , ref iTotalPageCount
                      , General.GetNullableGuid(Dashboardnvc != null? Dashboardnvc["ucAudit"] : null)
                      , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucFromPort"] : null)
                      , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucToPort"] : null)
                      , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["chkAtSea"] : null)
                      , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucFrom"] : null)
                      , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucTo"] : null)
                      , General.GetNullableString(ViewState["Type"].ToString())
                      , General.GetNullableInteger(ViewState["ExternalYN"].ToString())
                      , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

        General.SetPrintOptions("gvPlanner", "Audit / Inspection Schedule", alCaptions, alColumns, ds);

        gvPlanner.DataSource = ds;
        gvPlanner.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
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
                    RadWindowManager1.RadConfirm("Are you sure to plan this audit/inspection?", "Confirm", 320, 150, null, "Audit/Inspection Plan");
                }
                gvPlanner.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UNPLAN"))
            {
                RadLabel lblPlannerid = (RadLabel)e.Item.FindControl("lblPlannerId");
                RadLabel lblScheduleId = (RadLabel)e.Item.FindControl("lblScheduleId");

                PhoenixInspectionAuditSchedule.UnPlanInspection(General.GetNullableGuid(lblPlannerid.Text), General.GetNullableGuid(lblScheduleId.Text));

                ucStatus.Text = "Audit/Inspection is un-planned successfully.";
                gvPlanner.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;
                RadLabel lblPlannerid = (RadLabel)eeditedItem.FindControl("lblPlannerId");
                RadLabel lblScheduleId = (RadLabel)eeditedItem.FindControl("lblScheduleId");
                RadLabel lblVesselId = (RadLabel)eeditedItem.FindControl("lblVesselId");

                ViewState["CURRENTROW"] = e.Item.ItemIndex;

                RadWindowManager1.RadConfirm("When the schedule is deleted then all the deficiencies will also be deleted if recorded. Do you want to continue..?", "ConfirmDelete", 320, 150, null, "Delete");


                //PhoenixInspectionAuditSchedule.DeleteAuditPlanAndSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                //        int.Parse(lblVesselId.Text), General.GetNullableGuid(lblPlannerid.Text), General.GetNullableGuid(lblScheduleId.Text));
                gvPlanner.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                RadLabel lblScheduleId = (RadLabel)eeditedItem.FindControl("lblScheduleId");
                RadLabel lblCategoryId = (RadLabel)eeditedItem.FindControl("lblCategoryId");
                RadLabel lblInspectingCompanyid = (RadLabel)eeditedItem.FindControl("lblInspectingCompanyid");
                RadComboBox ddlInternalInspectorEdit = (RadComboBox)eeditedItem.FindControl("ddlInternalInspectorEdit");

                string category = "";
                string organisation = "";
                string internalorganization = "";

                if (!IsValidPlan(((RadLabel)eeditedItem.FindControl("lblStatusId")).Text,
                    ((UserControlDate)eeditedItem.FindControl("ucPlannedDateEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                if (lblCategoryId != null && lblCategoryId.Text != "")
                    category = lblCategoryId.Text;

                organisation = General.GetNullableString(((RadTextBox)eeditedItem.FindControl("txtOrganisationEdit")).Text);

                PhoenixInspectionAuditSchedule.InsertReviewPlanner(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , int.Parse(((RadLabel)eeditedItem.FindControl("lblVesselId")).Text)
                            , General.GetNullableGuid(((RadLabel)eeditedItem.FindControl("lblInspectionId")).Text)
                            , General.GetNullableGuid(((RadLabel)eeditedItem.FindControl("lblScheduleId")).Text)
                            , General.GetNullableGuid(((RadLabel)eeditedItem.FindControl("lblPlannerId")).Text)
                            , General.GetNullableDateTime(((UserControlDate)eeditedItem.FindControl("ucLastDoneDateEdit")).Text)
                            , General.GetNullableDateTime(((UserControlDate)eeditedItem.FindControl("ucDueDateEdit")).Text)
                            , General.GetNullableDateTime(((UserControlDate)eeditedItem.FindControl("ucPlannedDateEdit")).Text)
                            , General.GetNullableInteger(((UserControlSeaport)eeditedItem.FindControl("ucFromPortEdit")).SelectedSeaport)
                            , General.GetNullableInteger(((UserControlSeaport)eeditedItem.FindControl("ucToPortEdit")).SelectedSeaport)
                            , (category == PhoenixCommonRegisters.GetHardCode(1, 144, "INT")) ? General.GetNullableInteger(ddlInternalInspectorEdit.SelectedValue) : null
                            , General.GetNullableString(((RadTextBox)eeditedItem.FindControl("txtExternalAuditorEdit")).Text)
                            , (category == PhoenixCommonRegisters.GetHardCode(1, 144, "INT")) ? internalorganization : null
                            , (category == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")) ? organisation : null
                            , (category == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")) ? General.GetNullableInteger(ddlInternalInspectorEdit.SelectedValue) : null
                            , General.GetNullableInteger(((RadLabel)eeditedItem.FindControl("lblIsManual")).Text)
                            , General.GetNullableGuid(lblInspectingCompanyid.Text)
                        );

                gvPlanner.Rebind();
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
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
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = (GridDataItem)e.Item;
                DataRowView dv = (DataRowView)e.Item.DataItem;

                Image imgFlag = e.Item.FindControl("imgFlag") as Image;
                if (imgFlag != null && dv["FLDDUEOVERDUEYN"].ToString().Equals("3"))
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/" + "red-symbol.png";
                    imgFlag.ToolTip = "Overdue";
                }
                else if (imgFlag != null && dv["FLDDUEOVERDUEYN"].ToString().Equals("2"))
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/" + "yellow-symbol.png";
                    imgFlag.ToolTip = "Due within 30 days";
                }
                else if (imgFlag != null && dv["FLDDUEOVERDUEYN"].ToString().Equals("1"))
                {
                    imgFlag.Visible = true;
                    imgFlag.ImageUrl = Session["images"] + "/" + "green-symbol.png";
                    imgFlag.ToolTip = "Due within 60 days";
                }
                else
                {
                    if (imgFlag != null) imgFlag.Visible = false;
                }
                LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                if (eb != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, eb.CommandName))
                        eb.Visible = false;
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

                LinkButton imgCreateSchedule = (LinkButton)e.Item.FindControl("imgCreateSchedule");

                LinkButton cmdReport = (LinkButton)e.Item.FindControl("cmdReport");
                if (cmdReport != null)
                {
                    if (dv["FLDSCHEDULEID"] != null && dv["FLDSCHEDULEID"].ToString() != "")
                        cmdReport.Attributes.Add("onclick", "javascript:openNewWindow('Report','','" + Session["sitepath"] + "/Inspection/InspectionAuditRecordGeneral.aspx?AUDITSCHEDULEID=" + dv["FLDSCHEDULEID"].ToString() + "'); return true;");
                }

                RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
                UserControlDate ucPlannedDateEdit = (UserControlDate)e.Item.FindControl("ucPlannedDateEdit");
                UserControlSeaport ucFromPortEdit = (UserControlSeaport)e.Item.FindControl("ucFromPortEdit");
                UserControlSeaport ucToPortEdit = (UserControlSeaport)e.Item.FindControl("ucToPortEdit");
                RadComboBox ddlInternalInspectorEdit = (RadComboBox)e.Item.FindControl("ddlInternalInspectorEdit");
                RadTextBox txtExternalAuditorEdit = (RadTextBox)e.Item.FindControl("txtExternalAuditorEdit");
                RadTextBox txtOrganisationEdit = (RadTextBox)e.Item.FindControl("txtOrganisationEdit");

                RadLabel lblPlannedDateEdit = (RadLabel)e.Item.FindControl("lblPlannedDateEdit");
                RadLabel lblFromPortEdit = (RadLabel)e.Item.FindControl("lblFromPortEdit");
                RadLabel lblToPortEdit = (RadLabel)e.Item.FindControl("lblToPortEdit");
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
                if (ucFromPortEdit != null)
                {
                    ucFromPortEdit.SeaportList = PhoenixRegistersSeaport.ListSeaport();
                    ucFromPortEdit.DataBind();
                    ucFromPortEdit.SelectedSeaport = dv["FLDFROMPORTID"].ToString();
                }
                if (ucToPortEdit != null)
                {
                    ucToPortEdit.SeaportList = PhoenixRegistersSeaport.ListSeaport();
                    ucToPortEdit.DataBind();
                    ucToPortEdit.SelectedSeaport = dv["FLDTOPORTID"].ToString();
                }
                if (lblStatusId != null && lblStatusId.Text == ViewState["ASG"].ToString())
                {
                    if (ucPlannedDateEdit != null) ucPlannedDateEdit.Visible = true;
                    if (ucFromPortEdit != null) ucFromPortEdit.Visible = true;
                    if (ucToPortEdit != null) ucToPortEdit.Visible = true;

                    if (ddlInternalInspectorEdit != null)
                    {
                        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                            ddlInternalInspectorEdit.Visible = false;
                        else
                            ddlInternalInspectorEdit.Visible = true;
                    }
                    if (txtExternalAuditorEdit != null) txtExternalAuditorEdit.Visible = true;
                    if (txtOrganisationEdit != null) txtOrganisationEdit.Visible = true;

                    if (lblPlannedDateEdit != null) lblPlannedDateEdit.Visible = false;
                    if (lblFromPortEdit != null) lblFromPortEdit.Visible = false;
                    if (lblToPortEdit != null) lblToPortEdit.Visible = false;
                    if (lblInspectorEdit != null)
                    {
                        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                            lblInspectorEdit.Visible = true;
                        else
                            lblInspectorEdit.Visible = false;
                    }
                    if (lblExternalAuditorEdit != null) lblExternalAuditorEdit.Visible = false;
                    if (lblOrganisationEdit != null) lblOrganisationEdit.Visible = false;
                }
                else
                {
                    if (ucPlannedDateEdit != null) ucPlannedDateEdit.Visible = false;
                    if (ucFromPortEdit != null) ucFromPortEdit.Visible = false;
                    if (ucToPortEdit != null) ucToPortEdit.Visible = false;
                    if (ddlInternalInspectorEdit != null) ddlInternalInspectorEdit.Visible = false;
                    if (txtExternalAuditorEdit != null) txtExternalAuditorEdit.Visible = false;
                    if (txtOrganisationEdit != null) txtOrganisationEdit.Visible = false;

                    if (lblPlannedDateEdit != null) lblPlannedDateEdit.Visible = true;
                    if (lblFromPortEdit != null) lblFromPortEdit.Visible = true;
                    if (lblToPortEdit != null) lblToPortEdit.Visible = true;
                    if (lblInspectorEdit != null) lblInspectorEdit.Visible = true;
                    if (lblExternalAuditorEdit != null) lblExternalAuditorEdit.Visible = true;
                    if (lblOrganisationEdit != null) lblOrganisationEdit.Visible = true;
                }

                RadLabel lblFromPort = (RadLabel)e.Item.FindControl("lblFromPort");
                if (lblFromPort != null)
                {
                    UserControlToolTip ucToolTipFromPort = (UserControlToolTip)e.Item.FindControl("ucToolTipFromPort");
                    ucToolTipFromPort.Position = ToolTipPosition.TopCenter;
                    ucToolTipFromPort.TargetControlId = lblFromPort.ClientID;
                }

                RadLabel lblToPort = (RadLabel)e.Item.FindControl("lblToPort");
                if (lblToPort != null)
                {
                    UserControlToolTip ucToolTipToPort = (UserControlToolTip)e.Item.FindControl("ucToolTipToPort");
                    ucToolTipToPort.Position = ToolTipPosition.TopCenter;
                    ucToolTipToPort.TargetControlId = lblToPort.ClientID;
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
                    lblOrganisation.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipOrganisation.ToolTip + "', 'visible');");
                    lblOrganisation.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipOrganisation.ToolTip + "', 'hidden');");
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
                LinkButton lnkvessel = (LinkButton)e.Item.FindControl("lnkvessel");
                RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblVesselId");
                if (lnkvessel != null)
                {
                    lnkvessel.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Details','Dashboard/DashboardVesselDetails.aspx?vesselid=" + lblvesselid.Text + "');");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
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
            RadLabel lblVesselId = (RadLabel)gvPlanner.Items[nCurrentRow].FindControl("lblVesselId");
            RadLabel lblInspectingCompanyid = (RadLabel)gvPlanner.Items[nCurrentRow].FindControl("lblInspectingCompanyid");

            PhoenixInspectionAuditSchedule.ReviewScheduleInsert(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , int.Parse(lblVesselId.Text)
                            , new Guid(lblInspectionId.Text)
                            , lblLastDoneDate != null ? General.GetNullableDateTime(lblLastDoneDate.Text) : null
                            , DateTime.Parse(strduedate)
                            , null
                            , null
                            , null
                            , null
                            , null
                            , null
                            , null
                            , lblPlannerId != null ? General.GetNullableGuid(lblPlannerId.Text) : null
                            , null
                            , General.GetNullableGuid(lblInspectingCompanyid.Text)
                            , 0
                            );

            ucStatus.Text = "Audit/Inspection is planned successfully.";
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
            RadLabel lblVesselId = (RadLabel)gvPlanner.Items[nCurrentRow].FindControl("lblVesselId");

            PhoenixInspectionAuditSchedule.DeleteAuditPlanAndSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(lblVesselId.Text), General.GetNullableGuid(lblPlannerid.Text), General.GetNullableGuid(lblScheduleId.Text));

            ucStatus.Text = "Planner is deleted successfully.";
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
}
