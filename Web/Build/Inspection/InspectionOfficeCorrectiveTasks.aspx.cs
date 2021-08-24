using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionOfficeCorrectiveTasks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionOfficeCorrectiveTasks.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvShipBoardTasks')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionOfficeTaskFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionOfficeCorrectiveTasks.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

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
            gvShipBoardTasks.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
                        
        }

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
            Filter.CurrentOfficeCorrectiveTaskFilter = null;
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
            Response.Redirect("../Inspection/InspectionOfficeTaskFilter.aspx", true);
        }
        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
        {
            if (CommandName.ToUpper().Equals("PATASK"))
            {
                Response.Redirect("../Inspection/InspectionLongTermActionOfficeList.aspx", true);
            }

            if (CommandName.ToUpper().Equals("MOCTASK"))
            {
                Response.Redirect("../Inspection/InspectionMOCActionPlanShipboardTaskList.aspx", false);
            }
        }
        else
        {
            if (CommandName.ToUpper().Equals("PATASK"))
            {
                Response.Redirect("../Inspection/InspectionLongTermActionOfficeList.aspx", true);
            }

            if (CommandName.ToUpper().Equals("MOCTASK"))
            {
                Response.Redirect("../Inspection/InspectionMOCActionPlanOfficeTaskList.aspx", false);
            }

            if (CommandName.ToUpper().Equals("RATASK"))
            {
                Response.Redirect("../Inspection/InspectionOfficeCorrectiveRATasks.aspx", false);
            }
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;
        int? departmenttype = 2;

        string[] alColumns = { "FLDVESSELNAME", "FLDCREATEDFROMNAME", "FLDSOURCENAME", "FLDHARDNAME", "FLDITEMNAME", "FLDCORRECTIVEACTION", "FLDDEPARTMENTNAME", "FLDTARGETDATE", "FLDCOMPLETIONDATE", "FLDCACLOSEOUTVERIFIEDDATE", "FLDSHIPBOARDSTATUS" };
        string[] alCaptions = { "Vessel", "Source Reference number", "Source", "Type", "Item", "Task", "Assigned Department", "Target On", "Completion On", "Closed On", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentOfficeCorrectiveTaskFilter;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentOfficeCorrectiveTaskFilter == null)
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
            //gvShipBoardTasks.DataSource = ds;
            //gvShipBoardTasks.DataBind();

            if (ViewState["TASKID"] == null)
            {
                ViewState["TASKID"] = ds.Tables[0].Rows[0]["FLDINSPECTIONCORRECTIVEACTIONID"].ToString();
                ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                //gvShipBoardTasks.SelectedIndex = 0;
            }
            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            //ShowNoRecordsFound(dt, gvShipBoardTasks);
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
        int? departmenttype = 2;

        string[] alColumns = { "FLDVESSELNAME", "FLDCREATEDFROMNAME", "FLDSOURCENAME", "FLDHARDNAME", "FLDITEMNAME", "FLDCORRECTIVEACTION", "FLDDEPARTMENTNAME", "FLDTARGETDATE", "FLDCOMPLETIONDATE", "FLDCACLOSEOUTVERIFIEDDATE", "FLDSHIPBOARDSTATUS" };
        string[] alCaptions = { "Vessel", "Source Reference number", "Source", "Type", "Item", "Task", "Assigned Department", "Target On", "Completion On", "Closed On", "Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentOfficeCorrectiveTaskFilter;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentOfficeCorrectiveTaskFilter == null)
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



        Response.AddHeader("Content-Disposition", "attachment; filename=OfficeCorrectiveTaskList.xls");
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
        //gvShipBoardTasks.SelectedIndex = -1;

        //for (int i = 0; i < gvShipBoardTasks.Rows.Count; i++)
        //{
        //    if (gvShipBoardTasks.DataKeys[i].Value.ToString().Equals(ViewState["TASKID"].ToString()))
        //    {
        //        gvShipBoardTasks.SelectedIndex = i;
        //    }
        //}
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvShipBoardTasks_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        //BindData();
        gvShipBoardTasks.Rebind();
    }

    protected void gvShipBoardTasks_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        //BindData();
        gvShipBoardTasks.Rebind();
    }

    protected void gvShipBoardTasks_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nCurrentRow = e.Item.ItemIndex;

                if (e.CommandName.ToUpper().Equals("SELECT"))
                {
                    BindPageURL(nCurrentRow);
                    SetRowSelection();
                    RadLabel lblCorrectiveActionid = (RadLabel)e.Item.FindControl("lblCorrectiveActionid");
                    if (lblCorrectiveActionid != null)
                        Response.Redirect("../Inspection/InspectionShipBoardTasksDetails.aspx?Officetask=yes&correctiveactionid=" + lblCorrectiveActionid.Text);
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
                    //BindData();
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

    //protected void gvShipBoardTasks_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    BindData();
    //}

    //protected void gvShipBoardTasks_RowEditing(object sender, GridViewEditEventArgs de)
    //{
    //    GridView _gridView = (GridView)sender;

    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;
    //    BindPageURL(de.NewEditIndex);

    //    BindData();
    //}

    protected void gvShipBoardTasks_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            LinkButton Communication = (LinkButton)e.Item.FindControl("lnkCommunication");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblVesselsid = (RadLabel)e.Item.FindControl("lblVesselId");

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

            RadLabel lblPreventiveActionid = (RadLabel)e.Item.FindControl("lblLongTermActionId");
            if (Communication != null)
            {
                Communication.Visible = SessionUtil.CanAccess(this.ViewState, Communication.CommandName);
                Communication.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonCommunication.aspx?Type=TASK" + "&Referenceid=" + lblPreventiveActionid.Text + "&Vesselid=" + lblVesselsid.Text + "','large'); return true;");
            }
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
                ucToolTipItem.Position = ToolTipPosition.Center;
                ucToolTipItem.TargetControlId = lblItem.ClientID;
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
            RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
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
                //if (drvCompletedDate["FLDATTACHMENTDELETELOCKYN"].ToString() == "1")
                //    att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?U=1&ratingyn=1&dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                //    + PhoenixModule.QUALITY + "&type=SHIPBOARDEVIDENCE&cmdname=SHIPBOARDEVIDENCEUPLOAD&VESSELID=" + lblVesselId.Text + "'); return true;");
                //else

                att.Attributes.Add("onclick", "javascript:openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.QUALITY + "&type=SHIPBOARDEVIDENCE&cmdname=SHIPBOARDEVIDENCEUPLOAD&VESSELID=" + lblVesselId.Text + "'); return true;");


            }


            RadLabel lblCorrectiveActionid = (RadLabel)e.Item.FindControl("lblCorrectiveActionid");
            LinkButton imgSuperintendentComments = (LinkButton)e.Item.FindControl("imgSuperintendentComments");
            if (lblCorrectiveActionid != null) imgSuperintendentComments.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Inspection/InspectionRescheduleTaskSuperintendentApproval.aspx?CORRECTIVEACTIONID=" + lblCorrectiveActionid.Text + "','large'); return true;");

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
        // gvShipBoardTasks.SelectedIndex = se.NewSelectedIndex;
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
           // ViewState["PAGENUMBER"] = 1;
            gvShipBoardTasks.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void cmdSearch_Click(object sender, EventArgs e)
    //{
    //    BindData();
    //    SetPageNavigator();
    //}

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
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
    //    //gvShipBoardTasks.SelectedIndex = -1;
    //    //gvShipBoardTasks.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindData();
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

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void selection_Changed(Object sender, EventArgs e)
    {
        //BindData();
        gvShipBoardTasks.Rebind();
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
