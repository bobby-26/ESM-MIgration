using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using SouthNests.Phoenix.Export2XL;
using Telerik.Web.UI;

public partial class InspectionAuditRecordList : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionAuditRecordList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAuditRecordList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            if (Request.QueryString["FMSYN"] == null)
            {
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionAuditScheduleFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
                toolbar.AddFontAwesomeButton("../Inspection/InspectionAuditRecordList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Inspection/InspectionAuditManualAdd.aspx')", "Add Manual Audit/Inspection", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            }

            MenuInspectionScheduleSearch.AccessRights = this.ViewState;
            MenuInspectionScheduleSearch.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                VesselConfiguration();

                toolbar = new PhoenixToolbar();

                if (Request.QueryString["ShowNavigationError"] != null)
                    ShowNavigationError();

                ucConfirm.Attributes.Add("style", "display:none");
                ucConfirmDelete.Attributes.Add("style", "display:none");
                gvAuditRecordList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["SECTIONID"] = "";
                ViewState["AUDITSCHEDULID"] = null;
                ViewState["TYPE"] = "";
                ViewState["EXTERNALYN"] = "";

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvc.Get("QMS");


                if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() == "log")
                {
                    Filter.CurrentAuditMenu = "log";
                    ucTitle.Text = "Audit / Inspection Log";
                    Session["INSPECTIONDTKEY"] = null;
                }
                else if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToString() == "record")
                {
                    Session["INSPECTIONDTKEY"] = null;
                    Filter.CurrentAuditMenu = null;
                }
                if (Request.QueryString["menu"] != null && Request.QueryString["menu"].ToString() != string.Empty)
                    Filter.CurrentAuditScheduleId = null;

                if (Request.QueryString["TYPE"] != null && Request.QueryString["TYPE"].ToString() != string.Empty)
                    ViewState["TYPE"] = Request.QueryString["TYPE"].ToString();

                if (Request.QueryString["EXTERNALYN"] != null && Request.QueryString["EXTERNALYN"].ToString() != string.Empty)
                    ViewState["EXTERNALYN "] = Request.QueryString["EXTERNALYN"].ToString();

                if (Request.QueryString["FMSYN"] != null && Request.QueryString["FMSYN"].ToString() != string.Empty)
                    ViewState["FMSYN"] = Request.QueryString["FMSYN"].ToString();

            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);

            if ((PhoenixSecurityContext.CurrentSecurityContext.InstallCode.Equals(0)) && (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE")))
            {
                //      MenuGeneral.TabStrip = "true";                
                toolbarmain.AddButton("Office Audit / Inspection", "OFFICELIST", ToolBarDirection.Right);
                toolbarmain.AddButton("Audit / Inspection", "LIST", ToolBarDirection.Right);
                MenuGeneral.MenuList = toolbarmain.Show();
            }
            else
            {
                MenuGeneral.Visible = false;
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            {
                MenuGeneral.SelectedMenuIndex = 1;
            }
            Filter.CurrentAuditRecordResponseFilter = null;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void ShowNavigationError()
    {
        ucError.HeaderMessage = "Navigation Error";
        ucError.ErrorMessage = "Please select an 'Audit' and navigate to other Tabs.";
        ucError.Visible = true;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int vesselid = -1;
        string doctitle = null;
        doctitle = "Audit / Inspection Log";
        string[] alColumns;
        string[] alCaptions;
        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
        {
            alColumns = new string[]{ "FLDVESSELNAME", "FLDISMANUAL", "FLDREFERENCENUMBER", "FLDSHORTCODE", "FLDCOMPLETIONDATE",
                                        "FLDFROMPORT", "FLDTOPORT", "FLDSTATUSNAME", "FLDNAMEOFINSPECTOR", "FLDORGANISATION","FLDDEFICIENCYCOUNT",
                                        "FLDMAJORNCCOUNT", "FLDNCCOUNT", "FLDOBSCOUNT" };
            alCaptions = new string[]{ "Vessel", "M/C", "Reference Number", "Audit / Inspection", "Last Done",
                                        "From Port", "To Port", "Status", "Auditor / Inspector",  "Organization","Def Count", "MNC", "NC", "OBS" };
        }
        else
        {
            alColumns = new string[]{  "FLDVESSELNAME","FLDISMANUAL", "FLDREFERENCENUMBER", "FLDSHORTCODE", "FLDCOMPLETIONDATE","FLDCREDITEDYN",
                                        "FLDFROMPORT", "FLDTOPORT", "FLDSTATUSNAME", "FLDNAMEOFINSPECTOR", "FLDORGANISATION","FLDATTACHMENTCREATEDBY","FLDDEFICIENCYCOUNT",
                                        "FLDMAJORNCCOUNT", "FLDNCCOUNT", "FLDOBSCOUNT" };
            alCaptions = new string[]{  "Vessel","M/C", "Reference Number", "Audit / Inspection", "Last Done","Credit",
                                        "From Port", "To Port", "Status", "Auditor / Inspector",  "Organization","Attached By","Def Count", "MNC", "NC", "OBS" };
        }

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentAuditScheduleFilterCriteria;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentAuditScheduleFilterCriteria == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        DataSet ds = PhoenixInspectionAuditSchedule.AuditScheduleSearch(
          PhoenixSecurityContext.CurrentSecurityContext.UserCode
        , nvc != null ? General.GetNullableInteger(nvc.Get("ucAuditType")) : null
        , nvc != null ? General.GetNullableInteger(nvc.Get("ucAuditCategory")) : null
        , nvc != null ? General.GetNullableGuid(nvc.Get("ucAudit")) : null
        , vesselid
        , nvc != null ? General.GetNullableInteger(nvc.Get("ucPort")) : null
        , nvc != null ? General.GetNullableString(nvc.Get("ucStatus")) : null
        , null
        , null
        , sortexpression, sortdirection,
        (int)ViewState["PAGENUMBER"],
        gvAuditRecordList.PageSize,
        ref iRowCount,
        ref iTotalPageCount,
        nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
        , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneFrom")) : null
        , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneTo")) : null
        , nvc != null ? General.GetNullableString(nvc.Get("txtRefNo")) : null
        , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
        , nvc != null ? General.GetNullableInteger(nvc.Get("ucAddrOwner")) : null
        , nvc != null ? General.GetNullableInteger(nvc.Get("ucCharterer")) : null
        , nvc != null ? General.GetNullableString(nvc.Get("txtExternalInspector")) : null
        , nvc != null ? General.GetNullableString(nvc.Get("txtExternalOrganization")) : null
        , nvc != null ? General.GetNullableInteger(nvc.Get("ddlInspectorName")) : null
        , nvc != null ? General.GetNullableInteger(nvc.Get("ucPortTo")) : null
        , nvc != null ? General.GetNullableInteger(nvc.Get("ddlDefType")) : null
        , nvc != null ? General.GetNullableInteger(nvc.Get("txtKey")) : null
        , nvc != null ? General.GetNullableGuid(nvc.Get("ucChapter")) : null
        , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
        , nvc != null ? General.GetNullableInteger(nvc.Get("chkAtSea")) : null
        , nvc != null ? General.GetNullableInteger(nvc.Get("ChkCredited")) : null
        , nvc != null ? General.GetNullableInteger(nvc.Get("isdetention")) : null
        , General.GetNullableString(ViewState["TYPE"].ToString())
        , General.GetNullableInteger(ViewState["EXTERNALYN"].ToString())
        );

        Response.AddHeader("Content-Disposition", "attachment; filename=Audit_Inspection_Log.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + doctitle + "</h3></td>");
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

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            if ((PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE")))
            {
                Response.Redirect("../Inspection/InspectionAuditOfficeVesselScheduleFilter.aspx");
            }
            else
            {
                Response.Redirect("../Inspection/InspectionAuditScheduleFilter.aspx");
            }
        }
        if (CommandName.ToUpper().Equals("OFFICELIST"))
        {
            Response.Redirect("../Inspection/InspectionAuditOfficeRecordList.aspx");
        }
    }

    protected void InspectionScheduleSearch_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvAuditRecordList.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentAuditScheduleFilterCriteria = null;
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvAuditRecordList.Rebind();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int vesselid = -1;
        string[] alColumns;
        string[] alCaptions;
        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
        {
            alColumns = new string[]{ "FLDVESSELNAME", "FLDISMANUAL", "FLDREFERENCENUMBER", "FLDSHORTCODE", "FLDCOMPLETIONDATE",
                                        "FLDFROMPORT", "FLDTOPORT", "FLDSTATUSNAME", "FLDNAMEOFINSPECTOR", "FLDORGANISATION","FLDDEFICIENCYCOUNT",
                                        "FLDMAJORNCCOUNT", "FLDNCCOUNT", "FLDOBSCOUNT" };
            alCaptions = new string[]{ "Vessel", "M/C", "Reference Number", "Audit / Inspection", "Last Done",
                                        "From Port", "To Port", "Status", "Auditor / Inspector",  "Organization","Def Count", "MNC", "NC", "OBS" };
        }
        else
        {
            alColumns = new string[]{  "FLDVESSELNAME","FLDISMANUAL", "FLDREFERENCENUMBER", "FLDSHORTCODE", "FLDCOMPLETIONDATE","FLDCREDITEDYN",
                                        "FLDFROMPORT", "FLDTOPORT", "FLDSTATUSNAME", "FLDNAMEOFINSPECTOR", "FLDORGANISATION","FLDATTACHMENTCREATEDBY","FLDDEFICIENCYCOUNT",
                                        "FLDMAJORNCCOUNT", "FLDNCCOUNT", "FLDOBSCOUNT" };
            alCaptions = new string[]{  "Vessel","M/C", "Reference Number", "Audit / Inspection", "Last Done","Credit",
                                        "From Port", "To Port", "Status", "Auditor / Inspector",  "Orzanisation","Attached By","Def Count", "MNC", "NC", "OBS" };
        }

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentAuditScheduleFilterCriteria;


        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
        {
            gvAuditRecordList.Columns[5].Visible = false;
            gvAuditRecordList.Columns[10].Visible = false;
        }

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentAuditScheduleFilterCriteria == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
        DataSet ds = PhoenixInspectionAuditSchedule.AuditScheduleSearch(
              PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucAuditType")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucAuditCategory")) : null
            , nvc != null ? General.GetNullableGuid(nvc.Get("ucAudit")) : null
            , vesselid
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucPort")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("ucStatus")) : null
            , null
            , null
            , sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"]
            , gvAuditRecordList.PageSize
            , ref iRowCount
            , ref iTotalPageCount
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneFrom")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneTo")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtRefNo")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucAddrOwner")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucCharterer")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtExternalInspector")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtExternalOrganization")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlInspectorName")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucPortTo")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlDefType")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("txtKey")) : null
            , nvc != null ? General.GetNullableGuid(nvc.Get("ucChapter")) : null
            , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
            , nvc != null ? General.GetNullableInteger(nvc.Get("chkAtSea")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ChkCredited")) : null
             , nvc != null ? General.GetNullableInteger(nvc.Get("isdetention")) : null
             , General.GetNullableString(ViewState["TYPE"].ToString())
             , General.GetNullableInteger(ViewState["EXTERNALYN"].ToString()));

        General.SetPrintOptions("gvAuditRecordList", "Audit / Inspection Log", alCaptions, alColumns, ds);

        gvAuditRecordList.DataSource = ds;
        gvAuditRecordList.VirtualItemCount = iRowCount;


    }

    private void SetRowSelection()
    {
        //string a = Filter.CurrentAuditScheduleId.ToString();
        //gvAuditRecordList.SelectedIndex = -1;
        //for (int i = 0; i < gvAuditRecordList.Rows.Count; i++)
        //{
        //    if (gvAuditRecordList.DataKeys[i].Value.ToString().Equals(Filter.CurrentAuditScheduleId.ToString()))
        //    {
        //        gvAuditRecordList.SelectedIndex = i;
        //    }
        //}
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvAuditRecordList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //RadGrid _gridView = (RadGrid)sender;
            //int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("NAVIGATE"))
            {
               // BindPageURL(nCurrentRow);
                SetRowSelection();
            }
            else if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
            {
               //  BindPageURL(nCurrentRow);
                SetRowSelection();
            }
            else if (e.CommandName.ToUpper().Equals("DEFICIENCYSUMMARY"))
            {
              //   BindPageURL(nCurrentRow);
                SetRowSelection();
            }
            else if (e.CommandName.ToUpper().Equals("EXPORT2XL"))
            {
               // BindPageURL(nCurrentRow);
                SetRowSelection();
                RadLabel lblInspectionScheduleId = (RadLabel)e.Item.FindControl("lblInspectionScheduleId");
                RadLabel lblInspectionShortcode = (RadLabel)e.Item.FindControl("lblInspectionShortcode");
                if (lblInspectionShortcode != null && lblInspectionShortcode.Text.ToUpper().Contains("VIR"))
                    PhoenixInspection2XL.Export2XLVIRInspection(new Guid(lblInspectionScheduleId.Text));
                else
                    PhoenixInspection2XL.Export2XLInspection(new Guid(lblInspectionScheduleId.Text));
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["CURRENTROW"] = e.Item.ItemIndex;

                RadLabel lblPlannerid = (RadLabel)e.Item.FindControl("lblPlannerId");
                RadLabel lblScheduleId = (RadLabel)e.Item.FindControl("lblInspectionScheduleId");
                RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselid");

                RadWindowManager1.RadConfirm("When the schedule is deleted then all the deficiencies will also be deleted if recorded. Do you want to continue..?", "ConfirmDelete", 320, 150, null, "Delete");

                //PhoenixInspectionAuditSchedule.DeleteAuditPlanAndSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                //    int.Parse(lblVesselId.Text), General.GetNullableGuid(lblPlannerid.Text), General.GetNullableGuid(lblScheduleId.Text));
            }
            else if (e.CommandName.ToUpper().Equals("CHECKLIST"))
            {
                //BindPageURL(nCurrentRow);
                SetRowSelection();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            gvAuditRecordList.Rebind();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (Filter.CurrentAuditScheduleId != null && Filter.CurrentAuditScheduleId.ToString() != string.Empty)
            {
                PhoenixInspectionAuditSchedule.UpdateAuditScheduleStatus(
                   PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                   new Guid(Filter.CurrentAuditScheduleId.ToString()));

                ucStatus.Text = "Audit is Completed";
                gvAuditRecordList.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void btnConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            int nCurrentRow = int.Parse(ViewState["CURRENTROW"].ToString());

            RadLabel lblPlannerid = (RadLabel)gvAuditRecordList.Items[nCurrentRow].FindControl("lblPlannerId");
            RadLabel lblScheduleId = (RadLabel)gvAuditRecordList.Items[nCurrentRow].FindControl("lblInspectionScheduleId");
            RadLabel lblVesselId = (RadLabel)gvAuditRecordList.Items[nCurrentRow].FindControl("lblVesselid");

            PhoenixInspectionAuditSchedule.DeleteAuditPlanAndSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(lblVesselId.Text), General.GetNullableGuid(lblPlannerid.Text), General.GetNullableGuid(lblScheduleId.Text));

            ucStatus.Text = "Planner is deleted successfully.";
            gvAuditRecordList.Rebind();
            //BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DeleteAuditSchedule(Guid inspectionscheduleid)
    {
        PhoenixInspectionAuditSchedule.DeleteAuditSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, inspectionscheduleid);
    }

    protected void gvAuditRecordList_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvAuditRecordList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            string refno = ((RadLabel)e.Item.FindControl("lblInspectionRefNo")).Text;
            string auditscheduleid = ((RadLabel)e.Item.FindControl("lblInspectionScheduleId")).Text;
            LinkButton lnkInspection = (LinkButton)e.Item.FindControl("lnkInspection");
            LinkButton cmdExport2XL = (LinkButton)e.Item.FindControl("cmdExport2XL");
            RadLabel lblCompletionDate  = (RadLabel)e.Item.FindControl("lblInspectionCompletionDate");
            DataRowView dr = (DataRowView)e.Item.DataItem;

            if (dr["FLDREVIEWOVERDUEYN"].ToString().Equals("1"))
            {
                lblCompletionDate.Attributes["style"] = "color:Red !important";
                lblCompletionDate.ToolTip = "Overdue for Review";
                lblCompletionDate.Font.Bold = true;
            }
            else if (dr["FLDREVIEWOVERDUEYN"].ToString().Equals("2"))
            {
                lblCompletionDate.Attributes["style"] = "color:darkorange !important";
                lblCompletionDate.ToolTip = "Overdue for Closure";
                lblCompletionDate.Font.Bold = true;
            }

            if (lnkInspection != null)
            {
                lnkInspection.Attributes.Add("onclick", "javascript:openNewWindow('Report','','" + Session["sitepath"] + "/Inspection/InspectionAuditRecordGeneral.aspx?AUDITSCHEDULEID=" + auditscheduleid + "&reffrom=log'); return true;");
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

            RadLabel lblNameOfInspector = (RadLabel)e.Item.FindControl("lblNameOfInspector");
            UserControlToolTip ucToolTipInspector = (UserControlToolTip)e.Item.FindControl("ucToolTipInspector");
            ucToolTipInspector.Position = ToolTipPosition.TopCenter;
            ucToolTipInspector.TargetControlId = lblNameOfInspector.ClientID;

            RadLabel lblInspectionRefNo = (RadLabel)e.Item.FindControl("lblInspectionRefNo");
            UserControlToolTip ucToolTipInspectionRefNo = (UserControlToolTip)e.Item.FindControl("ucToolTipInspectionRefNo");
            ucToolTipInspectionRefNo.Position = ToolTipPosition.TopCenter;
            ucToolTipInspectionRefNo.TargetControlId = lblInspectionRefNo.ClientID;

            RadLabel lblInspectionShortcode = (RadLabel)e.Item.FindControl("lblInspectionShortcode");
            UserControlToolTip ucToolTipName = (UserControlToolTip)e.Item.FindControl("ucToolTipName");
            ucToolTipName.Position = ToolTipPosition.TopCenter;
            ucToolTipName.TargetControlId = lnkInspection.ClientID;


            RadLabel lblOrganisation = (RadLabel)e.Item.FindControl("lblOrganisation");
            UserControlToolTip ucToolTipOrganisation = (UserControlToolTip)e.Item.FindControl("ucToolTipOrganisation");
            ucToolTipOrganisation.Position = ToolTipPosition.TopCenter;
            ucToolTipOrganisation.TargetControlId = lblOrganisation.ClientID;

            RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
            UserControlToolTip ucToolTipStatus = (UserControlToolTip)e.Item.FindControl("ucToolTipStatus");
            ucToolTipStatus.Position = ToolTipPosition.TopCenter;
            ucToolTipStatus.TargetControlId = lblStatus.ClientID;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
            RadLabel lblDTkey = (RadLabel)e.Item.FindControl("lblDTkey");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();
                if (dr["FLDISATTACHMENT"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                else
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                //    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTkey.Text
                            + "&mod=" + PhoenixModule.QUALITY
                            + "&type=AUDITINSPECTION"
                            + "&cmdname=AUDITINSPECTIONUPLOAD"
                            + "&VESSELID=" + lblVesselid.Text
                            + "'); return true;");
            }

            LinkButton cmdDeficiencySummary = (LinkButton)e.Item.FindControl("cmdDeficiencySummary");
            if (cmdDeficiencySummary != null)
            {
                cmdDeficiencySummary.Visible = SessionUtil.CanAccess(this.ViewState, cmdDeficiencySummary.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();
                if (dr["FLDDEFICIENCYRECORDEDYN"].ToString() == "0")
                {
                    cmdDeficiencySummary.Controls.Remove(html);
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-eye-na\"></i></span>";
                    cmdDeficiencySummary.Controls.Add(html);
                }
                //    cmdDeficiencySummary.ImageUrl = Session["images"] + "/deficiency-noaction.png";
                cmdDeficiencySummary.Attributes.Add("onclick", "javascript:openNewWindow('Summary','','" + Session["sitepath"] + "/Inspection/InspectionDeficiencySummary.aspx?SOURCEID=" + auditscheduleid + "&VESSELID=" + lblVesselid.Text + "'); return true;");
            }

            if (cmdExport2XL != null)
            {
                cmdExport2XL.Visible = true;

                if (dr["FLDINSTALLCODE"] != null && dr["FLDINSTALLCODE"].ToString() != "" && int.Parse(dr["FLDINSTALLCODE"].ToString()) > 0)
                    cmdExport2XL.Visible = false;

                if (!SessionUtil.CanAccess(this.ViewState, cmdExport2XL.CommandName))
                {
                    cmdExport2XL.Visible = false;
                }
            }

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    db.Visible = false;
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            LinkButton cmdChecklist = (LinkButton)e.Item.FindControl("cmdChecklist");
            if (cmdChecklist != null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    cmdChecklist.Visible = false;
                if (!SessionUtil.CanAccess(this.ViewState, cmdChecklist.CommandName))
                    cmdChecklist.Visible = false;
                cmdChecklist.Attributes.Add("onclick", "javascript:openNewWindow('Checklist','','" + Session["sitepath"] + "/Inspection/InspectionAuditChecklistVerification.aspx?REVIEWSCHEDULEID=" + auditscheduleid + "&VESSELID=" + lblVesselid.Text + "'); return true;");
            }
            LinkButton Report = (LinkButton)e.Item.FindControl("cmdReport");

            if (Report != null && dr["FLDQUESTIONTYPE"].ToString()=="189")
            {
                Report.ToolTip = "Report";
                Report.Visible = SessionUtil.CanAccess(this.ViewState, Report.CommandName);
                Report.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Report','Inspection/InspectionAuditInterfaceReport.aspx?ReviewScheduleId=" + auditscheduleid + "&Type=" + dr["FLDQUESTIONTYPE"].ToString() + "&VesselId=" + lblVesselid.Text + "','false','1400px','600px');return false");
            }
            else
            {
                Report.ToolTip = "Summary of Findings";
                Report.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters','Summary','Inspection/InspectionAuditInterfaceSummary.aspx?ReviewScheduleId=" + auditscheduleid + "&Type=" + dr["FLDQUESTIONTYPE"].ToString() + "&VesselId=" + lblVesselid.Text + "','false','1400px','600px');return false");
            }
        }

    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblInspectionScheduleId = (RadLabel)gvAuditRecordList.Items[rowindex].FindControl("lblInspectionScheduleId");
            if (lblInspectionScheduleId != null)
            {
                Filter.CurrentAuditScheduleId = lblInspectionScheduleId.Text;
                RadLabel lblInspectionDtKey = (RadLabel)gvAuditRecordList.Items[rowindex].FindControl("lblInspectionDtKey");
                if (lblInspectionDtKey != null)
                    Session["INSPECTIONDTKEY"] = lblInspectionDtKey.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //ViewState["PAGENUMBER"] = 1;
        gvAuditRecordList.Rebind();
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvAuditRecordList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAuditRecordList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
}
