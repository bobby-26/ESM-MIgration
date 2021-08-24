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
public partial class Inspection_InspectionDashBoardAuditRecordList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashBoardAuditRecordList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAuditRecordList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','', '" + Session["sitepath"] + "/Inspection/InspectionDashBoardAuditRecordListFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashBoardAuditRecordList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuInspectionScheduleSearch.AccessRights = this.ViewState;
            MenuInspectionScheduleSearch.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                InspectionFilter.CurrentAuditInspectionCategoryDashboardFilter = null;

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
                ViewState["STATUS"] = "";
                ViewState["VESSELID"] = "";
                ViewState["Type"] = "";
                ViewState["ExternalYN"] = "";

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;

                if (Request.QueryString["STATUS"] != null)
                    ViewState["STATUS"] = Request.QueryString["STATUS"].ToString();

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
        int Company = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
        NameValueCollection Dashboardnvc = InspectionFilter.CurrentAuditInspectionCategoryDashboardFilter;

        DataSet ds = PhoenixInspectionOfficeDashboard.DashBoardAuditInspectionShipSearch(General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null))
                      , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["Owner"] : null))
                                                                                            , General.GetNullableString(ViewState["STATUS"].ToString())
                                                                                            , General.GetNullableInteger(null)
                                                                                            , sortexpression
                                                                                            , sortdirection
                                                                                            ,1
                                                                                            , iRowCount
                                                                                            , ref iRowCount
                                                                                            ,ref iTotalPageCount
                                                                                            , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucAuditCategory"] : null)
                                                                                            , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucAudit"] : null)
                                                                                            , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucFromPort"] : null)
                                                                                            , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucToPort"] : null)
                                                                                            , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["ReferenceNo"] : null)
                                                                                            , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucFrom"] : null)
                                                                                            , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucTo"] : null)
                                                                                            , General.GetNullableString(ViewState["Type"].ToString())
                                                                                            , General.GetNullableInteger(ViewState["ExternalYN"].ToString())
                                                                                            , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

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
            InspectionFilter.CurrentAuditInspectionCategoryDashboardFilter = null;
            ViewState["PAGENUMBER"] = 1;
            ReBind();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
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


        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
        {
            gvAuditRecordList.Columns[5].Visible = false;
            gvAuditRecordList.Columns[10].Visible = false;
        }

        int Company = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
        NameValueCollection Dashboardnvc = InspectionFilter.CurrentAuditInspectionCategoryDashboardFilter;        

        DataSet ds = PhoenixInspectionOfficeDashboard.DashBoardAuditInspectionShipSearch(General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null))
                        , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null))
                        , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null))
                        , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (Dashboardnvc != null ? Dashboardnvc["Owner"] : null))
                        , General.GetNullableString(ViewState["STATUS"].ToString())
                        , General.GetNullableInteger(null)
                        , sortexpression
                        , sortdirection
                        , (int)ViewState["PAGENUMBER"]
                        , gvAuditRecordList.PageSize
                        , ref iRowCount
                        , ref iTotalPageCount
                        , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucAuditCategory"] : null)
                        , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucAudit"] : null)
                        , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucFromPort"] : null)
                        , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucToPort"] : null)
                        , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["ReferenceNo"] : null)
                        , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucFrom"] : null)
                        , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucTo"] : null)
                        , General.GetNullableString(ViewState["Type"].ToString())
                        , General.GetNullableInteger(ViewState["ExternalYN"].ToString())
                        , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

        General.SetPrintOptions("gvAuditRecordList", "Audit / Inspection Log", alCaptions, alColumns, ds);

        gvAuditRecordList.DataSource = ds;
        gvAuditRecordList.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
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
    protected void ReBind()
    {
        gvAuditRecordList.SelectedIndexes.Clear();
        gvAuditRecordList.EditIndexes.Clear();
        gvAuditRecordList.DataSource = null;
        gvAuditRecordList.Rebind();
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
            DataRowView dr = (DataRowView)e.Item.DataItem;

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
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-eye-na\"></i></span>";
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
            LinkButton lnkvessel = (LinkButton)e.Item.FindControl("lnkVessel");
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblVesselid");
            if (lnkvessel != null)
            {
                lnkvessel.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Details','Dashboard/DashboardVesselDetails.aspx?vesselid=" + lblvesselid.Text + "');");
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
        ViewState["PAGENUMBER"] = 1;
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
