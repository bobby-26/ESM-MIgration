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

public partial class Inspection_InspectionShipBoardTasks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionShipBoardTasks.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvShipBoardTasks')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionShipBoardTaskFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionShipBoardTasks.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        MenuShipBoardTasks.AccessRights = this.ViewState;
        MenuShipBoardTasks.MenuList = toolbar.Show();

        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("RA Task", "RATASK", ToolBarDirection.Right);
        toolbarmain.AddButton("MOC Task", "MOCTASK", ToolBarDirection.Right);
        toolbarmain.AddButton("Preventive Task", "PATASK", ToolBarDirection.Right);
        toolbarmain.AddButton("Corrective Task", "CARTASK", ToolBarDirection.Right);
        //toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuOrderFormMain.AccessRights = this.ViewState;
        MenuOrderFormMain.MenuList = toolbarmain.Show();
        MenuOrderFormMain.SelectedMenuIndex = 3;

        if (!IsPostBack)
        {
            Session["New"] = "N";
            if (Session["CHECKED_ITEMS"] != null)
                Session.Remove("CHECKED_ITEMS");

            VesselConfiguration();


            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }

            DateTime now = DateTime.Now;
            string FromDate = now.Date.AddMonths(-3).ToShortDateString();
            string ToDate = now.Date.AddMonths(+3).ToShortDateString();
            string Status = PhoenixCommonRegisters.GetHardCode(1, 146, "OPN");

            ViewState["FROMDATE"] = FromDate.ToString();
            ViewState["TODATE"] = ToDate.ToString();
            ViewState["Status"] = Status.ToString();

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

            gvShipBoardTasks.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        // BindData();
    }

    protected void MenuShipBoardTasks_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvShipBoardTasks.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentShipBoardTaskFilter = null;
            BindData();
            gvShipBoardTasks.Rebind();

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
        if (CommandName.ToUpper().Equals("PATASK"))
        {
            Response.Redirect("../Inspection/InspectionPreventiveTaskList.aspx", true);
        }
        if (CommandName.ToUpper().Equals("MOCTASK"))
        {
            Response.Redirect("../Inspection/InspectionMOCActionPlanShipboardTaskList.aspx", false);
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
        int? departmenttype = 1;

        string[] alColumns = { "FLDVESSELNAME", "FLDCREATEDFROMNAME", "FLDSOURCENAME", "FLDHARDNAME", "FLDDEPARTMENTNAME", "FLDITEMNAME", "FLDDEFICIENCYDETAILS", "FLDCORRECTIVEACTION", "FLDTARGETDATE", "FLDCOMPLETIONDATE", "FLDCACLOSEOUTVERIFIEDDATE", "FLDSHIPBOARDSTATUS" };
        string[] alCaptions = { "Vessel", "Source Reference", "Source", "Type", "Assigned Department", "Item", "Deficiency Details", "Task", "Target", "Completion", "Closed", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentShipBoardTaskFilter;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentShipBoardTaskFilter == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }

        DataSet ds = PhoenixInspectionLongTermAction.ShipBoardTasksSearch(
                                                                  null
                                                                , vesselid
                                                                , null
                                                                , null
                                                                , null //General.GetNullableInteger(ddlAcceptance.SelectedValue)
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvShipBoardTasks.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucAddrOwner")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : General.GetNullableInteger(ViewState["Status"].ToString())
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneFrom")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneTo")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : General.GetNullableDateTime(ViewState["FROMDATE"].ToString())
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : General.GetNullableDateTime(ViewState["TODATE"].ToString())
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucInspectionType")) : null
                                                                , nvc != null ? General.GetNullableGuid(nvc.Get("ucInspection")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("chkExcludeVIR")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlSourceType")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtSourceRefNo")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("chkShowRescheduledTasks")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtItem")) : null
                                                                , nvc != null ? General.GetNullableGuid(nvc.Get("ucChapter")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlDefType")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucNonConformanceCategory")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucVerficationLevel")) : null
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("chkOfficeAuditDeficiencies")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucCompany")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("chkPendingRescheduleTask")) : null
                                                                , departmenttype
                                                                );

        General.SetPrintOptions("gvShipBoardTasks", "Corrective Tasks", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["TASKID"] == null)
            {
                ViewState["TASKID"] = ds.Tables[0].Rows[0]["FLDINSPECTIONCORRECTIVEACTIONID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
        }
        gvShipBoardTasks.DataSource = ds;
        gvShipBoardTasks.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;
        int? departmenttype = 1;

        string[] alColumns = { "FLDVESSELNAME", "FLDCREATEDFROMNAME", "FLDSOURCENAME", "FLDHARDNAME", "FLDDEPARTMENTNAME", "FLDITEMNAME", "FLDDEFICIENCYDETAILS", "FLDCORRECTIVEACTION", "FLDTARGETDATE", "FLDCOMPLETIONDATE", "FLDCACLOSEOUTVERIFIEDDATE", "FLDSHIPBOARDSTATUS" };
        string[] alCaptions = { "Vessel", "Source Reference", "Source", "Type", "Assigned Department", "Item", "Deficiency Details", "Task", "Target", "Completion", "Closed", "Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentShipBoardTaskFilter;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentShipBoardTaskFilter == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }

        DataSet ds = PhoenixInspectionLongTermAction.ShipBoardTasksSearch(
                                                                  null
                                                                , vesselid
                                                                , null
                                                                , null
                                                                , null //General.GetNullableInteger(ddlAcceptance.SelectedValue)
                                                                , sortexpression
                                                                , sortdirection
                                                                , 1
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucAddrOwner")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : General.GetNullableInteger(ViewState["Status"].ToString())
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneFrom")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtDoneTo")) : null
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : General.GetNullableDateTime(ViewState["FROMDATE"].ToString())
                                                                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : General.GetNullableDateTime(ViewState["TODATE"].ToString())
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucInspectionType")) : null
                                                                , nvc != null ? General.GetNullableGuid(nvc.Get("ucInspection")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("chkExcludeVIR")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlSourceType")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtSourceRefNo")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("chkShowRescheduledTasks")) : null
                                                                , nvc != null ? General.GetNullableString(nvc.Get("txtItem")) : null
                                                                , nvc != null ? General.GetNullableGuid(nvc.Get("ucChapter")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlDefType")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucNonConformanceCategory")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucVerficationLevel")) : null
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("chkOfficeAuditDeficiencies")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucCompany")) : null
                                                                , nvc != null ? General.GetNullableInteger(nvc.Get("chkPendingRescheduleTask")) : null
                                                                , departmenttype);


        Response.AddHeader("Content-Disposition", "attachment; filename=TaskList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Corrective Tasks</h3></td>");
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
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }


    protected void gvShipBoardTasks_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nCurrentRow = e.Item.ItemIndex;
                
                if (e.CommandName.ToUpper().Equals("SELECT"))
                {
                    BindPageURL(nCurrentRow);
                    SetRowSelection();
                    RadLabel lblCorrectiveActionid = (RadLabel)e.Item.FindControl("lblCorrectiveActionid");
                    if (lblCorrectiveActionid != null)
                        Response.Redirect("../Inspection/InspectionShipBoardTasksDetails.aspx?correctiveactionid=" + lblCorrectiveActionid.Text);
                }
                if (e.CommandName.ToUpper().Equals("COMMUNICATION"))
                {
                    LinkButton lnkCommunication = (LinkButton)e.Item.FindControl("lnkCommunication");
                    
                }
                else if (e.CommandName.ToUpper().Equals("SAVE"))
                {
                    if (!IsValidShipBoardTask(((UserControlDate)e.Item.FindControl("ucCompletedDate")).Text
                           , ((TextBox)e.Item.FindControl("txtRemarksEdit")).Text))
                    {
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionLongTermAction.UpdateShipBoardTasks(
                          PhoenixSecurityContext.CurrentSecurityContext.UserCode
                          , new Guid(((RadLabel)e.Item.FindControl("lblLongTermActionIdEdit")).Text)
                          , DateTime.Parse(((UserControlDate)e.Item.FindControl("ucCompletedDate")).Text)
                          , ((TextBox)e.Item.FindControl("txtRemarksEdit")).Text
                     );
                    gvShipBoardTasks.Rebind();
                }
                else if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
                {
                    BindPageURL(nCurrentRow);
                    SetRowSelection();
                }
                else if (e.CommandName.ToUpper().Equals("RESCHEDULE"))
                {
                    BindPageURL(nCurrentRow);
                    SetRowSelection();
                }
                else if (e.CommandName.ToUpper().Equals("SUPERINTENDENTAPPROVAL"))
                {
                    BindPageURL(nCurrentRow);
                    SetRowSelection();
                }
            }
            if (e.CommandName.ToUpper().Equals("PAGE"))
            {
                ViewState["PAGENUMBER"] = null;
                gvShipBoardTasks.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvShipBoardTasks_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton Communication = (LinkButton)e.Item.FindControl("lnkCommunication");
            RadLabel lblCorrectiveActionid = (RadLabel)e.Item.FindControl("lblCorrectiveActionid");
            RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblTargetDate = (RadLabel)e.Item.FindControl("lblTargetDate");

            if (drv["FLDOVERDUEYN"].ToString().Equals("1"))
            {
                //lblTargetDate.BackColor = System.Drawing.Color.Red;
                lblTargetDate.Attributes["style"] = "color:Red !important";
                lblTargetDate.Font.Bold = true;
                lblTargetDate.ToolTip = "Overdue";
            }
            else if (drv["FLDOVERDUEYN"].ToString().Equals("2"))
            {
                //lblTargetDate.BackColor = System.Drawing.Color.Brown;
                lblTargetDate.Attributes["style"] = "color:darkviolet !important";
                lblTargetDate.Font.Bold = true;
                lblTargetDate.ToolTip = "Postponed";
            }

            if (Communication != null)
            {
                Communication.Visible = SessionUtil.CanAccess(this.ViewState, Communication.CommandName);                           
                Communication.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonCommunication.aspx?Type=TASK" + "&Referenceid=" + lblCorrectiveActionid.Text + "&Vesselid=" + lblVesselId.Text + "','large'); return true;");
            }
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            LinkButton lnkTaskDetails = (LinkButton)e.Item.FindControl("lnkTaskDetails");
            if (lnkTaskDetails != null)
            {
                UserControlToolTip ucToolTip = (UserControlToolTip)e.Item.FindControl("ucToolTip");
                ucToolTip.Position = ToolTipPosition.TopCenter;
                ucToolTip.TargetControlId = lnkTaskDetails.ClientID;
            }

            RadLabel lblTaskEdit = (RadLabel)e.Item.FindControl("lblTaskEdit");
            if (lblTaskEdit != null)
            {
                UserControlToolTip ucToolTip1 = (UserControlToolTip)e.Item.FindControl("ucToolTipEdit");
                ucToolTip1.Position = ToolTipPosition.TopCenter;
                ucToolTip1.TargetControlId = lblTaskEdit.ClientID;
            }

            RadLabel lblItem = (RadLabel)e.Item.FindControl("lblItem");
            if (lblItem != null)
            {
                UserControlToolTip ucToolTipItem = (UserControlToolTip)e.Item.FindControl("ucToolTipItem");
                ucToolTipItem.Position = ToolTipPosition.TopCenter;
                ucToolTipItem.TargetControlId = lblItem.ClientID;
            }

            RadLabel lblDeficiencyDetails = (RadLabel)e.Item.FindControl("lblDeficiencyDetails");
            if (lblDeficiencyDetails != null)
            {
                UserControlToolTip ucToolTipDetails = (UserControlToolTip)e.Item.FindControl("ucToolTipDetails");
                ucToolTipDetails.Position = ToolTipPosition.TopCenter;
                ucToolTipDetails.TargetControlId = lblDeficiencyDetails.ClientID;
            }

            RadLabel lbldepartment = (RadLabel)e.Item.FindControl("lbldepartment");
            if (lbldepartment != null)
            {
                UserControlToolTip ucToolTipdepartment = (UserControlToolTip)e.Item.FindControl("ucToolTipdepartment");
                ucToolTipdepartment.Position = ToolTipPosition.TopCenter;
                ucToolTipdepartment.TargetControlId = lbldepartment.ClientID;
            }

            UserControlDate ucCompletedDate = (UserControlDate)e.Item.FindControl("ucCompletedDate");
            DataRowView drvCompletedDate = (DataRowView)e.Item.DataItem;
            if (ucCompletedDate != null)
            {
                ucCompletedDate.Text = drvCompletedDate["FLDCOMPLETIONDATE"].ToString();
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

                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.QUALITY + "&type=SHIPBOARDEVIDENCE&cmdname=SHIPBOARDEVIDENCEUPLOAD&VESSELID=" + lblVesselId.Text + "'); return true;");
            }
           
            LinkButton imgSuperintendentComments = (LinkButton)e.Item.FindControl("imgSuperintendentComments");
            if (drv["FLDRESCHEDULETASKYN"].ToString() == "1" && drv["FLDSUPERINTENDENTAPPROVEDYN"].ToString() == "1" && drv["FLDSECONDARYAPPROVALREQUIREDYN"].ToString() == "1")
            {
                imgSuperintendentComments.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Inspection/InspectionRescheduleTaskSuperintendentApproval.aspx?viewonly=1&CORRECTIVEACTIONID=" + lblCorrectiveActionid.Text + "','large'); return true;");
            }
            else if (lblCorrectiveActionid != null)
            {
                imgSuperintendentComments.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Inspection/InspectionRescheduleTaskSuperintendentApproval.aspx?CORRECTIVEACTIONID=" + lblCorrectiveActionid.Text + "','large'); return true;");
            }

            LinkButton imgReschedule = (LinkButton)e.Item.FindControl("imgReschedule");
            if (lblCorrectiveActionid != null) imgReschedule.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Inspection/InspectionShipboardtaskRescheduleReason.aspx?CORRECTIVEACTIONID=" + lblCorrectiveActionid.Text + "','large'); return true;");

            if (drv["FLDRESCHEDULETASKYN"].ToString() == "1" && drv["FLDSUPERINTENDENTAPPROVEDYN"].ToString() == "0")
            {
                imgSuperintendentComments.Visible = true;
            }

            if (drv["FLDRESCHEDULETASKYN"].ToString() == "1" && drv["FLDSUPERINTENDENTAPPROVEDYN"].ToString() == "1" && drv["FLDSECONDARYAPPROVALREQUIREDYN"].ToString() == "1")
            {
                imgSuperintendentComments.Visible = true;
                imgSuperintendentComments.ToolTip = "Secondary Approval";
            }

            if (drv["FLDSUPERINTENDENTAPPROVEDYN"].ToString() == "1" && drv["FLDSECONDARYAPPROVALREQUIREDYN"].ToString() == "0")
            {
                imgSuperintendentComments.Visible = false;
            }

            if (drv["FLDSUPERINTENDENTAPPROVEDYN"].ToString() == "1" && drv["FLDSECONDARYAPPROVEDYN"].ToString() == "1")
            {
                imgSuperintendentComments.Visible = false;
            }


            LinkButton lnkTaskSource = (LinkButton)e.Item.FindControl("lnkTaskSource");

            RadLabel lblSourceId = (RadLabel)e.Item.FindControl("lblSourceId");
            RadLabel lblSourceType = (RadLabel)e.Item.FindControl("lblSourceType");
            RadLabel lblVesslName = (RadLabel)e.Item.FindControl("lblVesselId");

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
        }
    }

    protected void gvShipBoardTasks_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        //  gvShipBoardTasks.SelectedIndex = se.NewSelectedIndex;
        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {

            RadLabel lblLongTermActionId = ((RadLabel)gvShipBoardTasks.Items[rowindex].FindControl("lblLongTermActionId"));
            RadLabel lblDTKey = ((RadLabel)gvShipBoardTasks.Items[rowindex].FindControl("lblDTKey"));
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
            gvShipBoardTasks.Rebind();
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

    private bool IsValidShipBoardTask(string completeddate, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableDateTime(completeddate).Equals("") || General.GetNullableDateTime(completeddate).Equals(null))
            ucError.ErrorMessage = "Completion date is required.";

        if (General.GetNullableDateTime(completeddate) > DateTime.Today)
            ucError.ErrorMessage = "Completion date should be greater than current date.";

        if (remarks.Equals("") || remarks.Equals(null))
            ucError.ErrorMessage = "Remarks is required.";

        return (!ucError.IsError);
    }

    protected void gvShipBoardTasks_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvShipBoardTasks_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvShipBoardTasks.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
