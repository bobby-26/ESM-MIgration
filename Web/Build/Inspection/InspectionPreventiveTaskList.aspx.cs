using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionPreventiveTaskList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionPreventiveTaskList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvLongTermAction')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionPreventiveTaskFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionPreventiveTaskList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        MenuLongTermAction.AccessRights = this.ViewState;
        MenuLongTermAction.MenuList = toolbar.Show();

        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("RA Task", "RATASK", ToolBarDirection.Right);
        toolbarmain.AddButton("MOC Task", "MOCTASK", ToolBarDirection.Right);
        toolbarmain.AddButton("Preventive Task", "PATASK", ToolBarDirection.Right);
        toolbarmain.AddButton("Corrective Task", "CARTASK", ToolBarDirection.Right);
        //toolbar.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuOrderFormMain.AccessRights = this.ViewState;
        MenuOrderFormMain.MenuList = toolbarmain.Show();

        MenuOrderFormMain.SelectedMenuIndex = 2;

        if (!IsPostBack)
        {
            Session["New"] = "N";
            if (Filter.CurrentSelectedOfficeTasks != null)
                Filter.CurrentSelectedOfficeTasks = null;

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }

            VesselConfiguration();

            DateTime now = DateTime.Now;
            string FromDate = now.Date.AddMonths(-6).ToShortDateString();
            string ToDate = DateTime.Now.ToShortDateString();

            ViewState["FROMDATE"] = FromDate.ToString();
            ViewState["TODATE"] = ToDate.ToString();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            if (Request.QueryString["TASKID"] != null && Request.QueryString["TASKID"].ToString() != "")
                ViewState["TASKID"] = Request.QueryString["TASKID"].ToString();
            else
                ViewState["TASKID"] = null;

            if (Request.QueryString["DTKEY"] != null && Request.QueryString["DTKEY"].ToString() != null)
                ViewState["DTKEY"] = Request.QueryString["DTKEY"];
            else
                ViewState["DTKEY"] = null;

            gvLongTermAction.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        // BindData();
    }

    protected void MenuLongTermAction_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvLongTermAction.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentPreventiveTaskFilter = null;
            BindData();
            gvLongTermAction.Rebind();
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Inspection/InspectionShipBoardTaskFilter.aspx", true);
        }
        if (CommandName.ToUpper().Equals("CARTASK"))
        {
            Response.Redirect("../Inspection/InspectionShipBoardTasks.aspx", true);
        }
        if (CommandName.ToUpper().Equals("MOCTASK"))
        {
            Response.Redirect("../Inspection/InspectionMOCActionPlanShipboardTaskList.aspx", true);
        }
        if (CommandName.ToUpper().Equals("RATASK"))
        {
            Response.Redirect("../Inspection/InspectionShipboardRATask.aspx", false);
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;

        string[] alColumns = { "FLDVESSELNAME", "FLDCREATEDFROMNAME", "FLDTYPE", "FLDPREVENTIVEACTION", "FLDTASKCATEGORYNAME", "FLDTASKSUBCATEGORYNAME",
                                 "FLDSTATUSNAME", "FLDDEPARTMENTNAME", "FLDSUBDEPARTMENTNAME", "FLDTARGETDATE", "FLDCOMPLETIONDATE" };
        string[] alCaptions = { "Vessel", "Source", "Type", "Task", "Category", "Sub Category", "Task Status", "Assigned Department", "Sub Department",
                                  "Target Date", "Completed Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentPreventiveTaskFilter;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentPreventiveTaskFilter == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }

        DataSet ds = PhoenixInspectionLongTermAction.PreventiveTasksSearch(
                                                                  null
                                                                , null
                                                                , vesselid
                                                                , nvc != null ? General.GetNullableGuid(nvc.Get("ddlCategory")) : null
                                                                , nvc != null ? General.GetNullableGuid(nvc.Get("ddlSubcategory")) : null
                                                                , null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucDepartment")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlAcceptance")) : null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : General.GetNullableDateTime(ViewState["FROMDATE"].ToString())
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : General.GetNullableDateTime(ViewState["TODATE"].ToString())
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneFrom")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneTo")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucAddrOwner")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlSourceType")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtSourceRefNo")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlAcceptedBy")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtWONoFrom")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtWONoTo")) : null
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("chkOfficeAuditDeficiencies")) : null
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucCompany"] : null)
                                                                );

        General.SetPrintOptions("gvLongTermAction", "Preventive Task List", alCaptions, alColumns, ds);

        gvLongTermAction.DataSource = ds;
        gvLongTermAction.VirtualItemCount = iRowCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;

        string[] alColumns = { "FLDVESSELNAME", "FLDCREATEDFROMNAME", "FLDTYPE", "FLDPREVENTIVEACTION", "FLDTASKCATEGORYNAME", "FLDTASKSUBCATEGORYNAME",
                                 "FLDSTATUSNAME", "FLDDEPARTMENTNAME", "FLDSUBDEPARTMENTNAME", "FLDTARGETDATE", "FLDCOMPLETIONDATE" };
        string[] alCaptions = { "Vessel", "Source", "Type", "Task", "Category", "Sub Category", "Task Status", "Assigned Department", "Sub Department",
                                  "Target Date", "Completed Date" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentPreventiveTaskFilter;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentPreventiveTaskFilter == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }

        DataSet ds = PhoenixInspectionLongTermAction.PreventiveTasksSearch(
                                                                  null
                                                                , null
                                                                , vesselid
                                                                , nvc != null ? General.GetNullableGuid(nvc.Get("ddlCategory")) : null
                                                                , nvc != null ? General.GetNullableGuid(nvc.Get("ddlSubcategory")) : null
                                                                , null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucDepartment")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlAcceptance")) : null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvLongTermAction.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : General.GetNullableDateTime(ViewState["FROMDATE"].ToString())
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : General.GetNullableDateTime(ViewState["TODATE"].ToString())
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneFrom")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneTo")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucAddrOwner")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlSourceType")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtSourceRefNo")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlAcceptedBy")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtWONoFrom")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtWONoTo")) : null
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("chkOfficeAuditDeficiencies")) : null
                                                                , General.GetNullableInteger(nvc != null ? nvc["ucCompany"] : null)
                                                                );

        Response.AddHeader("Content-Disposition", "attachment; filename=PreventiveTaskList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Preventive Task List</h3></td>");
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

    private void SetRowSelection()
    {
        //gvLongTermAction.SelectedIndex = -1;

        //for (int i = 0; i < gvLongTermAction.Rows.Count; i++)
        //{
        //    if (gvLongTermAction.DataKeys[i].Value.ToString().Equals(ViewState["TASKID"].ToString()))
        //    {
        //        gvLongTermAction.SelectedIndex = i;
        //    }
        //}
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvLongTermAction_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        gvLongTermAction.Rebind();
    }

    protected void gvLongTermAction_RowCreated(object sender, GridViewRowEventArgs e)
    {
    }

    protected void gvLongTermAction_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        gvLongTermAction.Rebind();
    }

    protected void gvLongTermAction_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nCurrentRow = e.Item.ItemIndex;

                if (e.CommandName.ToUpper().Equals("NAVIGATE"))
                {
                    BindPageURL(nCurrentRow);
                    SetRowSelection();
                    RadLabel lblLongTermActionId = (RadLabel)e.Item.FindControl("lblLongTermActionId");
                    if (lblLongTermActionId != null)
                        Response.Redirect("../Inspection/InspectionPreventiveTasksDetails.aspx?preventiveactionid=" + lblLongTermActionId.Text);
                }

                if (e.CommandName.ToUpper().Equals("SHOWSOURCE"))
                {
                    BindPageURL(nCurrentRow);
                    SetRowSelection();
                }

                else if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
                {
                    BindPageURL(nCurrentRow);
                    SetRowSelection();
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLongTermAction_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton Communication = (LinkButton)e.Item.FindControl("lnkCommunication");
            RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel lblPreventiveActionid = (RadLabel)e.Item.FindControl("lblLongTermActionId");

            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblTargetDate = (RadLabel)e.Item.FindControl("lblTargetDate");

            if (drv["FLDOVERDUEYN"].ToString().Equals("1"))
            {
                lblTargetDate.Attributes["style"] = "color:Red !important";
                lblTargetDate.Font.Bold = true;
                lblTargetDate.ToolTip = "Overdue";
            }
            else if (drv["FLDOVERDUEYN"].ToString().Equals("2"))
            {
                lblTargetDate.Attributes["style"] = "color:darkviolet !important";
                lblTargetDate.Font.Bold = true;
                lblTargetDate.ToolTip = "Postponed";
            }

            if (Communication != null)
            {
                Communication.Visible = SessionUtil.CanAccess(this.ViewState, Communication.CommandName);
                Communication.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonCommunication.aspx?Type=TASK" + "&Referenceid=" + lblPreventiveActionid.Text + "&Vesselid=" + lblVesselId.Text + "','large'); return true;");
            }
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadLabel lblWorkOrderId = (RadLabel)e.Item.FindControl("lblWorkOrderId");
            CheckBox chkSelect = (CheckBox)e.Item.FindControl("chkSelect");
            if (lblWorkOrderId != null && lblWorkOrderId.Text != "")
            {
                if (chkSelect != null)
                    chkSelect.Visible = false;
            }

            LinkButton lnkTask = (LinkButton)e.Item.FindControl("lnkTask");
            UserControlToolTip ucToolTip = (UserControlToolTip)e.Item.FindControl("ucToolTip");
            UserControlToolTip ucToolTipCategory = (UserControlToolTip)e.Item.FindControl("ucToolTipCategory");

            RadLabel lblCategory = (RadLabel)e.Item.FindControl("lblCategory");
            ucToolTipCategory.Position = ToolTipPosition.TopCenter;
            ucToolTipCategory.TargetControlId = lblCategory.ClientID;

            ucToolTip.Position = ToolTipPosition.TopCenter;
            ucToolTip.TargetControlId = lnkTask.ClientID;

            LinkButton lnkTaskSource = (LinkButton)e.Item.FindControl("lnkTaskSource");
            RadLabel lblSourceId = (RadLabel)e.Item.FindControl("lblSourceId");
            RadLabel lblSourceType = (RadLabel)e.Item.FindControl("lblSourceType");
            RadLabel lblVesslName = (RadLabel)e.Item.FindControl("lblVesselId");
            RadLabel lblLongTermActionId = (RadLabel)e.Item.FindControl("lblLongTermActionId");
            RadLabel lblCategoryShortCode = (RadLabel)e.Item.FindControl("lblCategoryShortCode");

            if (lnkTaskSource != null)
            {
                if (lblSourceType.Text == "1") //Open Reports
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionDirectIncidentGeneral.aspx?directincidentid=" + lblSourceId.Text + "&viewonly=1'); return true;");
                }
                if (lblSourceType.Text == "2") //Direct
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionDeficiencyEdit.aspx?DEFICIENCYID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                }
                if (lblSourceType.Text == "3") // Audit/Inspection
                {
                    if (lblVesslName.Text.ToUpper().Equals("OFFICE"))
                    {
                        lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionAuditOfficeRecordGeneral.aspx?AUDITSCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                    }
                    else
                    {
                        lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionAuditRecordGeneral.aspx?AUDITSCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                    }
                }
                if (lblSourceType.Text == "4") //Vetting
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionVettingReport.aspx?SCHEDULEID=" + lblSourceId.Text + "&viewonly=1'); return true;");
                }
                if (lblSourceType.Text == "5") //Incident
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionIncidentView.aspx?incidentid=" + lblSourceId.Text + "'); return true;");
                }
                if (lblSourceType.Text == "6") //machinery damage
                {
                    lnkTaskSource.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionMachineryDamageGeneral.aspx?VIEWONLY=1&MACHINERYDAMAGEID=" + lblSourceId.Text + "'); return true;");
                }
            }
            LinkButton lnkWorkOrder = (LinkButton)e.Item.FindControl("lnkWorkOrderNumber");
            RadLabel lblWorkOrderID = (RadLabel)e.Item.FindControl("lblWorkOrderId");

            if (lnkWorkOrder != null)
            {
                lnkWorkOrder.Attributes.Add("onclick", "javascript:openNewWindow('source','','" + Session["sitepath"] + "/Inspection/InspectionLongTermActionWorkOrderDetails.aspx?WORKORDERID=" + lblWorkOrderID.Text + "&TASKID=" + ViewState["TASKID"] + "&viewonly=1'); return true;");
            }

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                HtmlGenericControl html = new HtmlGenericControl();

                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                else
                {
                    html.InnerHtml = "<span class=\"icon\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                // att.ImageUrl = Session["images"] + "/no-attachment.png";

                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.QUALITY + "&type=PREVENTIVETASKS&cmdname=PREVENTIVETASKSUPLOAD&VESSELID=" + lblVesselId.Text + "'); return true;");
            }

            LinkButton cmdShowStatus = (LinkButton)e.Item.FindControl("cmdShowStatus");


            if (cmdShowStatus != null && lblCategoryShortCode.Text == "FWV")
            {
                cmdShowStatus.Visible = true;
                cmdShowStatus.Attributes.Add("onclick", "javascript:openNewWindow('status','','" + Session["sitepath"] + "/Inspection/InspectionShipboardTaskStatus.aspx?preventiveactionid=" + lblLongTermActionId.Text + "'); return true;");
                cmdShowStatus.Visible = SessionUtil.CanAccess(this.ViewState, cmdShowStatus.CommandName);
            }

        }

    }

    private void BindPageURL(int rowindex)
    {
        try
        {

            RadLabel lblLongTermActionId = ((RadLabel)gvLongTermAction.Items[rowindex].FindControl("lblLongTermActionId"));
            RadLabel lblDTKey = ((RadLabel)gvLongTermAction.Items[rowindex].FindControl("lblDTKey"));
            RadLabel lblWorkorderId = ((RadLabel)gvLongTermAction.Items[rowindex].FindControl("lblWorkOrderId"));
            if (lblLongTermActionId != null)
                ViewState["TASKID"] = lblLongTermActionId.Text;
            if (lblDTKey != null)
                ViewState["DTKEY"] = lblDTKey.Text;
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
            if (Session["NewTask"] != null && Session["NewTask"].ToString() == "Y")
            {
                ViewState["TASKID"] = null;
                Session["NewTask"] = "N";
            }
            ViewState["PAGENUMBER"] = 1;
            gvLongTermAction.Rebind();
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

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
    protected void gvLongTermAction_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLongTermAction.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLongTermAction_SortCommand(object sender, GridSortCommandEventArgs e)
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
