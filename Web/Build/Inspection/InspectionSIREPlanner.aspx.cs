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

public partial class InspectionSIREPlanner : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionSIREPlanner.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvScheduleForCompany')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionSIREPlannerFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionSIREPlanner.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuScheduleGroup.AccessRights = this.ViewState;
            MenuScheduleGroup.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.Visible = false;

            if (!IsPostBack)
            {

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                VesselConfiguration();
                ucConfirm.Attributes.Add("style", "display:none");
                //ucConfirmDelete.Attributes.Add("style", "display:none");

                gvScheduleForCompany.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGEURL"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["SECTIONID"] = "";
            }
            // BindData();
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
            Response.Redirect("../Inspection/InspectionSIREPlannerFilter.aspx");
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int vesselid = -1;
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDCOMPANYNAME", "FLDINSPECTIONSHORTCODE", "FLD3RDINSPECTION", "FLD2NDINSPECTION",
                                 "FLD1STINSPECTION", "FLDDUEDATE", "FLDPLANNEDDATE", "FLDSEAPORTNAME", "FLDNAMEOFINSPECTOR",
                                 "FLDSCHEDULESTATUS","FLDACTIVEYN" };
        string[] alCaptions = { "Vessel", "Company", "Type", "3rd Last", "2nd Last", "Last","Due", "Planned",
                                  "Planned Port", "Inspector", "Status", "Active" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentScheduleByCompanyFilter;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentScheduleByCompanyFilter == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }

        ds = PhoenixInspectionSchedule.InspectionSirePlannerSearch(
                                                    vesselid,
                                                    nvc != null ? General.GetNullableGuid(nvc.Get("ddlCompany")) : null,
                                                    chkShowAll.Checked == true ? 1 : 0,//General.GetNullableInteger(null),
                                                    sortexpression, sortdirection,
                                                    1,
                                                    gvScheduleForCompany.PageSize,
                                                    ref iRowCount,
                                                    ref iTotalPageCount,
                                                    nvc != null ? General.GetNullableGuid(nvc.Get("ucVetting")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucPort")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ddlPlanned")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucDoneFrom")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucDoneTo")) : null,
                                                    nvc != null ? General.GetNullableString(nvc.Get("txtInspector")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucAddrOwner")) : null,
                                                    nvc != null ? General.GetNullableGuid(nvc.Get("ddlBasis")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucPlannedFrom")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucPlannedTo")) : null,
                                                    General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=Schedule.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>CDI / SIRE Schedule</h3></td>");
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
                gvScheduleForCompany.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentScheduleByCompanyFilter = null;
                ViewState["PAGENUMBER"] = 1;
                // BindData();
                gvScheduleForCompany.Rebind();
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
        int vesselid = -1;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alColumns = { "FLDVESSELNAME", "FLDCOMPANYNAME", "FLDINSPECTIONSHORTCODE", "FLD3RDINSPECTION", "FLD2NDINSPECTION",
                                 "FLD1STINSPECTION", "FLDDUEDATE", "FLDPLANNEDDATE", "FLDSEAPORTNAME", "FLDNAMEOFINSPECTOR",
                                 "FLDSCHEDULESTATUS","FLDACTIVEYN" };
        string[] alCaptions = { "Vessel", "Company", "Type", "3rd Last", "2nd Last", "Last","Due", "Planned",
                                  "Planned Port", "Inspector", "Status","Active" };

        NameValueCollection nvc = Filter.CurrentScheduleByCompanyFilter;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel")) == null) ? -1 : int.Parse(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentScheduleByCompanyFilter == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }

        DataSet ds = PhoenixInspectionSchedule.InspectionSirePlannerSearch(
                                                    vesselid,
                                                    nvc != null ? General.GetNullableGuid(nvc.Get("ddlCompany")) : null,
                                                    chkShowAll.Checked == true ? 1 : 0,//General.GetNullableInteger(null),
                                                    sortexpression, sortdirection,
                                                    gvScheduleForCompany.CurrentPageIndex + 1,
                                                    gvScheduleForCompany.PageSize,
                                                    //General.ShowRecords(null),
                                                    ref iRowCount,
                                                    ref iTotalPageCount,
                                                    nvc != null ? General.GetNullableGuid(nvc.Get("ucVetting")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucPort")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ddlPlanned")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucDoneFrom")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucDoneTo")) : null,
                                                    nvc != null ? General.GetNullableString(nvc.Get("txtInspector")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null,
                                                    nvc != null ? General.GetNullableInteger(nvc.Get("ucAddrOwner")) : null,
                                                    nvc != null ? General.GetNullableGuid(nvc.Get("ddlBasis")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucPlannedFrom")) : null,
                                                    nvc != null ? General.GetNullableDateTime(nvc.Get("ucPlannedTo")) : null,
                                                    General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        General.SetPrintOptions("gvScheduleForCompany", "CDI / SIRE Schedule", alCaptions, alColumns, ds);

        gvScheduleForCompany.DataSource = ds;
        gvScheduleForCompany.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

        gvScheduleForCompany.Rebind();
    }

    protected void gvScheduleForCompany_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvScheduleForCompany_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            //RadGrid _gridView = (RadGrid)sender;
            //int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("CREATESCHEDULE"))
            {
                ViewState["CURRENTROW"] = e.Item.ItemIndex;

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
                    RadWindowManager1.RadConfirm("Are you sure to plan this inspection?", "Confirm", 320, 150, null, "Audit/Inspection Plan");
                }

            }
            if (e.CommandName.ToUpper().Equals("NEWSCHEDULE"))
            {
                RadLabel lblScheduleByCompanyId = (RadLabel)e.Item.FindControl("lblScheduleByCompanyId");
                PhoenixInspectionSchedule.InsertInspectionSchedulebyBasis(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(lblScheduleByCompanyId.Text));

                ucStatus.Text = "Inspection is planned successfully.";
                gvScheduleForCompany.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UNPLAN"))
            {
                RadLabel lblScheduleByCompanyId = (RadLabel)e.Item.FindControl("lblScheduleByCompanyId");
                RadLabel lblSchedule = (RadLabel)e.Item.FindControl("lblScheduleId");

                PhoenixInspectionSchedule.UnplanInspectionSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(lblScheduleByCompanyId.Text)
                                            , new Guid(lblSchedule.Text));

                ucStatus.Text = "Inspection is un-planned successfully.";
                gvScheduleForCompany.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                RadLabel lblScheduleId = (RadLabel)e.Item.FindControl("lblScheduleId");

                if (!IsValidPlan(((RadLabel)e.Item.FindControl("lblStatusId")).Text,
                    ((UserControlDate)e.Item.FindControl("ucPlannedDateEdit")).Text
                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertScheduleDetails(
                           int.Parse(((RadLabel)e.Item.FindControl("lblVesselId")).Text)
                         , ((UserControlDate)e.Item.FindControl("ucLastDoneDateEdit")).Text
                         , ((UserControlDate)e.Item.FindControl("ucDueDateEdit")).Text
                         , ((RadLabel)e.Item.FindControl("lblCompanyId")).Text
                         , ((TextBox)e.Item.FindControl("txtBasisScheduleId")).Text
                         , ((RadLabel)e.Item.FindControl("lblScheduleId")).Text
                         , ((RadLabel)e.Item.FindControl("lblScheduleByCompanyId")).Text
                         , ((UserControlDate)e.Item.FindControl("ucPlannedDateEdit")).Text
                         , ((UserControlSeaport)e.Item.FindControl("ucSeaportEdit")).SelectedSeaport
                         , ((TextBox)e.Item.FindControl("txtInspectorEdit")).Text
                         , ((RadLabel)e.Item.FindControl("lblIsManual")).Text
                         , ((RadLabel)e.Item.FindControl("lblInspectionId")).Text);

                gvScheduleForCompany.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvScheduleForCompany_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                DataRowView dv = (DataRowView)e.Item.DataItem;
                Image imgFlag = e.Item.FindControl("imgFlag") as Image;

                RadLabel DueDate = (RadLabel)e.Item.FindControl("lblDueDate");
                if (DueDate != null)
                {
                    if (imgFlag != null && dv["FLDSIREDUE"].ToString().Equals("1"))
                    {
                        //DueDate.Attributes.Add("style", "background-color:#00FF00;font-weight:bold;");
                        //DueDate.ToolTip = "Due within 60 days";
                        DueDate.Attributes["style"] = "color:Green !important";
                        DueDate.ToolTip = "Due within 60 days";
                        DueDate.Font.Bold = true;
                    }
                    else if (imgFlag != null && dv["FLDSIREDUE"].ToString().Equals("2"))
                    {
                        //DueDate.Attributes.Add("style", "background-color:#FFFF00;font-weight:bold;");
                        //DueDate.ToolTip = "Due within 30 days";
                        DueDate.Attributes["style"] = "color:darkorange !important";
                        DueDate.ToolTip = "Due within 30 days";
                        DueDate.Font.Bold = true;
                    }
                    else if (imgFlag != null && dv["FLDSIREDUE"].ToString().Equals("3"))
                    {
                        //DueDate.Attributes.Add("style", "background-color:#FF0000;font-weight:bold;");
                        //DueDate.ToolTip = "Overdue";
                        DueDate.Attributes["style"] = "color:Red !important";
                        DueDate.ToolTip = "Overdue";
                        DueDate.Font.Bold = true;
                    }
                    else
                    {
                        if (imgFlag != null) imgFlag.Visible = false;
                    }
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

                ImageButton imgNewSchedule = (ImageButton)e.Item.FindControl("imgNewSchedule");
                if (imgNewSchedule != null)
                {
                    if (General.GetNullableGuid(dv["FLDBASISID"].ToString()) != null && General.GetNullableGuid(dv["FLDSCHEDULEBYCOMPANYIDFORBASIS"].ToString()) == null)
                        imgNewSchedule.Visible = true;
                }
                LinkButton imgCreateSchedule = (LinkButton)e.Item.FindControl("imgCreateSchedule");
                if (imgCreateSchedule != null && General.GetNullableGuid(dv["FLDBASISID"].ToString()) != null)
                {
                    imgCreateSchedule.Visible = false;
                }

                LinkButton cmdReport = (LinkButton)e.Item.FindControl("cmdReport");
                if (cmdReport != null)
                {
                    if (dv["FLDSHEDULEID"] != null && dv["FLDSHEDULEID"].ToString() != "")
                        cmdReport.Attributes.Add("onclick", "javascript:openNewWindow('Report','','" + Session["sitepath"] + "//Inspection/InspectionVettingReport.aspx?SCHEDULEID=" + dv["FLDSHEDULEID"].ToString() + "'); return true;");
                }

                HtmlImage imgBasis = (HtmlImage)e.Item.FindControl("imgBasis");
                RadLabel lblVesselId = (RadLabel)e.Item.FindControl("lblVesselId");
                RadLabel lblCompanyId = (RadLabel)e.Item.FindControl("lblCompanyId");
                RadLabel lblInspectionId = (RadLabel)e.Item.FindControl("lblInspectionId");

                if (imgBasis != null)
                    imgBasis.Attributes.Add("onclick", "return showPickList('spanBasisInspectionSchedule', 'codehelp1', '', '../Common/CommonPickListBasisInspectionSchedule.aspx?VESSELID=" + lblVesselId.Text + "&COMPANYID=" + lblCompanyId.Text + "&INSPECTIONID=" + lblInspectionId.Text + "', true);");

                UserControlDate ucPlannedDateEdit = (UserControlDate)e.Item.FindControl("ucPlannedDateEdit");
                UserControlSeaport ucSeaportEdit = (UserControlSeaport)e.Item.FindControl("ucSeaportEdit");
                TextBox txtInspectorEdit = (TextBox)e.Item.FindControl("txtInspectorEdit");
                RadLabel lblPlannedDateEdit = (RadLabel)e.Item.FindControl("lblPlannedDateEdit");
                RadLabel lblPlannedPortEdit = (RadLabel)e.Item.FindControl("lblPlannedPortEdit");
                RadLabel lblInspectorEdit = (RadLabel)e.Item.FindControl("lblInspectorEdit");

                RadLabel lblStatusId = (RadLabel)e.Item.FindControl("lblStatusId");
                if (lblStatusId != null && lblStatusId.Text != "")
                {
                    if (imgCreateSchedule != null) imgCreateSchedule.Visible = false;
                }
                else
                {
                    if (cmdReport != null) cmdReport.Visible = false;
                }
                if (ucSeaportEdit != null)
                {
                    ucSeaportEdit.SeaportList = PhoenixRegistersSeaport.ListSeaport();
                    ucSeaportEdit.DataBind();
                    ucSeaportEdit.SelectedSeaport = dv["FLDPORTID"].ToString();
                }
                if (lblStatusId != null && lblStatusId.Text == PhoenixCommonRegisters.GetHardCode(1, 146, "ASG"))
                {
                    if (ucPlannedDateEdit != null) ucPlannedDateEdit.Visible = true;
                    if (ucSeaportEdit != null) ucSeaportEdit.Visible = true;
                    if (txtInspectorEdit != null) txtInspectorEdit.Visible = true;
                    if (lblPlannedDateEdit != null) lblPlannedDateEdit.Visible = false;
                    if (lblPlannedPortEdit != null) lblPlannedPortEdit.Visible = false;
                    if (lblInspectorEdit != null) lblInspectorEdit.Visible = false;
                }
                else
                {
                    if (ucPlannedDateEdit != null) ucPlannedDateEdit.Visible = false;
                    if (ucSeaportEdit != null) ucSeaportEdit.Visible = false;
                    if (txtInspectorEdit != null) txtInspectorEdit.Visible = false;
                    if (lblPlannedDateEdit != null) lblPlannedDateEdit.Visible = true;
                    if (lblPlannedPortEdit != null) lblPlannedPortEdit.Visible = true;
                    if (lblInspectorEdit != null) lblInspectorEdit.Visible = true;
                }

                LinkButton lnkScheduleNumber = (LinkButton)e.Item.FindControl("lnkScheduleNumber");
                if (lnkScheduleNumber != null)
                {
                    if (dv["FLDPREVIOUSSCHEDULEID"] != null && dv["FLDPREVIOUSSCHEDULEID"].ToString() != "")
                        lnkScheduleNumber.Attributes.Add("onclick", "javascript:openNewWindow('Details','','" + Session["sitepath"] + "/Inspection/InspectionVettingReport.aspx?SCHEDULEID=" + dv["FLDPREVIOUSSCHEDULEID"].ToString() + "&viewonly=1'); return true;");
                }

                LinkButton lnkBasisDetails = (LinkButton)e.Item.FindControl("lnkBasisDetails");
                if (lnkBasisDetails != null)
                {
                    if (dv["FLDBASISID"] != null && dv["FLDBASISID"].ToString() != "")
                        lnkBasisDetails.Attributes.Add("onclick", "javascript:openNewWindow('Details','','" + Session["sitepath"] + "/Inspection/InspectionVettingReport.aspx?SCHEDULEID=" + dv["FLDBASISID"].ToString() + "&viewonly=1'); return true;");
                }

                RadLabel lblPlannedPort = (RadLabel)e.Item.FindControl("lblPlannedPort");
                if (lblPlannedPort != null)
                {
                    UserControlToolTip ucToolTipPort = (UserControlToolTip)e.Item.FindControl("ucToolTipPort");
                    lblPlannedPort.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipPort.ToolTip + "', 'visible');");
                    lblPlannedPort.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipPort.ToolTip + "', 'hidden');");
                }

                RadLabel lblInspector = (RadLabel)e.Item.FindControl("lblInspector");
                if (lblInspector != null)
                {
                    UserControlToolTip ucToolTipInspector = (UserControlToolTip)e.Item.FindControl("ucToolTipInspector");
                    lblInspector.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipInspector.ToolTip + "', 'visible');");
                    lblInspector.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipInspector.ToolTip + "', 'hidden');");
                }

                LinkButton imgUnPlan = (LinkButton)e.Item.FindControl("imgUnPlan");

                if (dv["FLDSHEDULEID"] != null && dv["FLDSHEDULEID"].ToString() != "" && imgUnPlan != null)
                {
                    imgUnPlan.Visible = true;

                    if (!SessionUtil.CanAccess(this.ViewState, imgUnPlan.CommandName))
                        imgUnPlan.Visible = false;

                    imgUnPlan.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you Sure you want to UnPlan?'); return false;");
                }

                if (imgNewSchedule != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, imgNewSchedule.CommandName))
                        imgNewSchedule.Visible = false;
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
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvScheduleForCompany.Rebind();
    }

    private void InsertScheduleDetails(int vesselid, string lastdonedate, string duedate, string company, string basis,
        string scheduleid, string schedulebycompanyid, string planneddate, string plannedport, string nameofinspector, string ismanualinspection, string inspectionid)
    {
        try
        {
            PhoenixInspectionSchedule.InsertInspectionScheduleByCompany(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , vesselid
                , General.GetNullableDateTime(lastdonedate)
                , General.GetNullableDateTime(duedate)
                , General.GetNullableGuid(company)
                , General.GetNullableGuid(basis)
                , General.GetNullableGuid(scheduleid)
                , General.GetNullableGuid(schedulebycompanyid)
                , General.GetNullableDateTime(planneddate)
                , General.GetNullableInteger(plannedport)
                , General.GetNullableString(nameofinspector)
                , General.GetNullableInteger(ismanualinspection)
                , General.GetNullableGuid(inspectionid)
                );
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void chkShowAll_Changed(object sender, EventArgs e)
    {
        gvScheduleForCompany.Rebind();
    }

    private bool IsValidDetails(string vesselid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(vesselid) == null)
            ucError.ErrorMessage = "Vessel is required";

        return (!ucError.IsError);
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

            RadLabel lblDueDate = (RadLabel)gvScheduleForCompany.Items[nCurrentRow].FindControl("lblDueDate");
            string strduedate = lblDueDate.Text;

            RadLabel lblBasisCompanyId = (RadLabel)gvScheduleForCompany.Items[nCurrentRow].FindControl("lblBasisCompanyId");
            RadLabel lblLastDoneDate = (RadLabel)gvScheduleForCompany.Items[nCurrentRow].FindControl("lblLastDoneDate");
            RadLabel lblScheduleByCompanyId = (RadLabel)gvScheduleForCompany.Items[nCurrentRow].FindControl("lblScheduleByCompanyId");
            RadLabel lblCompanyId = (RadLabel)gvScheduleForCompany.Items[nCurrentRow].FindControl("lblCompanyId");
            RadLabel lblVesselId = (RadLabel)gvScheduleForCompany.Items[nCurrentRow].FindControl("lblVesselId");
            RadLabel lblInspectionId = (RadLabel)gvScheduleForCompany.Items[nCurrentRow].FindControl("lblInspectionId");

            PhoenixInspectionSchedule.InspectionForCompanyInsert(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , new Guid(lblInspectionId.Text)
                            , Int16.Parse(lblVesselId.Text)
                            , lblLastDoneDate != null ? General.GetNullableDateTime(lblLastDoneDate.Text) : null
                            , null
                            , null
                            , null
                            , DateTime.Parse(strduedate)
                            , lblScheduleByCompanyId != null ? General.GetNullableGuid(lblScheduleByCompanyId.Text) : null
                            , General.GetNullableGuid(lblCompanyId.Text)
                            );

            ucStatus.Text = "Inspection is planned successfully.";
            gvScheduleForCompany.Rebind();

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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Text_Changed(object sender, EventArgs e)
    {
        gvScheduleForCompany.Rebind();
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void ucLastDoneDateEdit_TextChanged(object sender, EventArgs e)
    {
        UserControlDate ucLastDoneDate = (UserControlDate)sender;
        GridViewRow row = (GridViewRow)ucLastDoneDate.Parent.Parent;
        ucLastDoneDate = (UserControlDate)row.FindControl("ucLastDoneDateEdit");
        UserControlDate ucDueDate = (UserControlDate)row.FindControl("ucDueDateEdit");
        RadLabel lblInspectionIdEdit = (RadLabel)row.FindControl("lblInspectionIdEdit");
        int frequency = 0;
        DataSet ds = PhoenixInspection.EditInspection(new Guid(lblInspectionIdEdit.Text));
        if (ds.Tables[0].Rows.Count > 0)
            frequency = int.Parse(ds.Tables[0].Rows[0]["FLDFREQUENCYINMONTHS"].ToString());

        if (ucLastDoneDate != null && General.GetNullableDateTime(ucLastDoneDate.Text) != null)
        {
            DateTime dtLastDoneDate = Convert.ToDateTime(ucLastDoneDate.Text);
            DateTime dtDueDate = dtLastDoneDate.AddMonths(frequency);
            ucDueDate.Text = dtDueDate.ToString();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvScheduleForCompany.Rebind();
    }

    protected void ClearBasis(object sender, EventArgs e)
    {
        ImageButton imgClearBasis = (ImageButton)sender;
        GridViewRow row = (GridViewRow)imgClearBasis.Parent.Parent;
        imgClearBasis = (ImageButton)row.FindControl("imgClearBasis");
        TextBox txtCompany = (TextBox)row.FindControl("txtCompany");
        TextBox txtBasis = (TextBox)row.FindControl("txtBasis");
        TextBox txtBasisScheduleId = (TextBox)row.FindControl("txtBasisScheduleId");
        if (txtCompany != null) txtCompany.Text = "";
        if (txtBasis != null) txtBasis.Text = "";
        if (txtBasisScheduleId != null) txtBasisScheduleId.Text = "";
    }

    protected void gvScheduleForCompany_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvScheduleForCompany.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
