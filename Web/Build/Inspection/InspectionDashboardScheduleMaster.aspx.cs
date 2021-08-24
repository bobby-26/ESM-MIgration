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
using Telerik.Web.UI;

public partial class InspectionDashboardScheduleMaster : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            ucConfirmComplete.Visible = false;
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardScheduleMaster.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInspectionSchedule')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','', '" + Session["sitepath"] + "/Inspection/InspectionDashboardScheduleFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardScheduleMaster.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuInspectionScheduleSearch.AccessRights = this.ViewState;
            MenuInspectionScheduleSearch.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
            MenuGeneral.AccessRights = this.ViewState;
            //MenuGeneral.MenuList = toolbarmain.Show();
            MenuGeneral.Visible = false;

            if (!IsPostBack)
            {
                InspectionFilter.CurrentVettingSIRELOGDashboardFilter = null;
                VesselConfiguration();

                if (Request.QueryString["ShowNavigationError"] != null)
                    ShowNavigationError();

                gvInspectionSchedule.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGEURL"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["SECTIONID"] = "";
                ViewState["VESSELID"] = "";

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }
                string status = PhoenixCommonRegisters.GetHardCode(1, 146, "CMP");
                ViewState["Status"] = status.ToString();

                if (!string.IsNullOrEmpty(Request.QueryString["STATUS"]))
                {
                    ViewState["STATUS"] = Request.QueryString["STATUS"];
                }
                else
                    ViewState["STATUS"] = "";

                if (Request.QueryString["vslid"] != null && Request.QueryString["vslid"].ToString() != string.Empty)
                    ViewState["VESSELID"] = Request.QueryString["vslid"].ToString();
            }
            //   BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Inspection/InspectionScheduleFilter.aspx");
        }

        else
          if (CommandName.ToUpper().Equals("SCHEDULE"))
        {
            if (Filter.CurrentSelectedInspectionSchedule == null)
            {
                ucError.ErrorMessage = "Please select a 'Vetting' from the scroll list and then navigate to other tabs.";
                ucError.Visible = true;
                return;
            }
            Response.Redirect("../Inspection/InspectionScheduleGeneral.aspx?INSPECTIONSCHEDULEID=" + Filter.CurrentSelectedInspectionSchedule);
        }
        else if (CommandName.ToUpper().Equals("RECORD"))
        {
            if (Filter.CurrentSelectedInspectionSchedule == null)
            {
                ucError.ErrorMessage = "Please select a 'Vetting' from the scroll list and then navigate to other tabs.";
                ucError.Visible = true;
                return;
            }
            DataSet ds = PhoenixInspectionSchedule.EditInspectionSchedule(new Guid(Filter.CurrentSelectedInspectionSchedule));
            string refno = "";
            string status = "";
            if (ds.Tables[0].Rows.Count > 0)
            {
                refno = ds.Tables[0].Rows[0]["FLDREFERENCENUMBER"].ToString();
                status = ds.Tables[0].Rows[0]["FLDSTATUS"].ToString();
            }

            if (status == PhoenixCommonRegisters.GetHardCode(1, 146, "PLA"))
            {
                ucError.ErrorMessage = "Scheduled 'Vetting' can not start recording.";
                ucError.Visible = true;
                return;
            }

            if (!string.IsNullOrEmpty(refno))
                Response.Redirect("../Inspection/InspectionRecordAndResponse.aspx?", false);
        }
    }

    private void ShowNavigationError()
    {
        ucError.HeaderMessage = "Navigation Error";
        ucError.ErrorMessage = "Please Select a Inspection and Navigate to other Tabs";
        ucError.Visible = true;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDREFERENCENUMBER", "FLDINSPECTIONCOMPANYNAME", "FLDSHORTCODE",
                                 "FLDINSPECTIONSCREENING", "FLDBASISREFERENCENUMBER", "FLDCOMPLETIONDATE",
                                 "FLDSEAPORTNAME", "FLDNAMEOFINSPECTOR", "FLDATTENDINGSUPT","FLDSTATUSNAME", "FLDDEFICIENCYCOUNT",
                                 "FLDOBSCOUNT", "FLDHIRISKOBSCOUNT"};
        string[] alCaptions = { "Vessel", "Reference Number", "Company", "Vetting", "I/S", "Basis",
                                  "Last Done", "Port", "Inspector", "Attending Supt","Status", "Deficiency Count", "OBS", "HR OBS" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        

        NameValueCollection nvc = InspectionFilter.CurrentVettingSIRELOGDashboardFilter;

        DataSet ds = PhoenixInspectionOfficeDashboard.InspectionScheduleSearch(
                        General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (nvc != null ? nvc["VesselList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (nvc != null ? nvc["VesselTypeList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (nvc != null ? nvc["FleetList"] : null))
                      , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (nvc != null ? nvc["Owner"] : null))
                      , General.GetNullableString(ViewState["STATUS"].ToString())
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucInspectionType")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucInspectionCategory")) : null
            , nvc != null ? General.GetNullableGuid(nvc.Get("ucInspection")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucPort")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : null
            , sortexpression, sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvInspectionSchedule.PageSize,
            ref iRowCount,
            ref iTotalPageCount
            , nvc != null ? General.GetNullableGuid(nvc.Get("ddlCompany")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtInspector")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtRefNo")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlAttendingSupt")) : null
            , nvc != null ? General.GetNullableGuid(nvc.Get("ucChapter")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("Isrejected")) : null
            , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

        Response.AddHeader("Content-Disposition", "attachment; filename=VettingLog.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vetting Log</h3></td>");
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
            gvInspectionSchedule.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            InspectionFilter.CurrentVettingSIRELOGDashboardFilter = null;
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvInspectionSchedule.Rebind();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDREFERENCENUMBER", "FLDINSPECTIONCOMPANYNAME", "FLDSHORTCODE",
                                 "FLDINSPECTIONSCREENING", "FLDBASISREFERENCENUMBER", "FLDCOMPLETIONDATE",
                                 "FLDSEAPORTNAME", "FLDNAMEOFINSPECTOR", "FLDATTENDINGSUPT","FLDSTATUSNAME","FLDDEFICIENCYCOUNT",
                                 "FLDOBSCOUNT", "FLDHIRISKOBSCOUNT"};
        string[] alCaptions = { "Vessel", "Reference Number", "Company", "Vetting", "I/S", "Basis",
                                  "Last Done", "Port", "Inspector", "Attending Supt","Status", "Deficiency Count", "OBS", "HR OBS" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = InspectionFilter.CurrentVettingSIRELOGDashboardFilter;

        DataSet ds = PhoenixInspectionOfficeDashboard.InspectionScheduleSearch(General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? ViewState["VESSELID"].ToString() : (nvc != null ? nvc["VesselList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (nvc != null ? nvc["VesselTypeList"] : null))
                      , General.GetNullableString(ViewState["VESSELID"].ToString() != string.Empty ? null : (nvc != null ? nvc["FleetList"] : null))
                      , General.GetNullableInteger(ViewState["VESSELID"].ToString() != string.Empty ? null : (nvc != null ? nvc["Owner"] : null))
                      , General.GetNullableString(ViewState["STATUS"].ToString())
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucInspectionType")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucInspectionCategory")) : null
            , nvc != null ? General.GetNullableGuid(nvc.Get("ucInspection")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucPort")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFrom")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtTo")) : null
            , sortexpression, sortdirection,
           gvInspectionSchedule.CurrentPageIndex + 1,
            gvInspectionSchedule.PageSize,
            ref iRowCount,
            ref iTotalPageCount
            , nvc != null ? General.GetNullableGuid(nvc.Get("ddlCompany")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtInspector")) : null
            , nvc != null ? General.GetNullableString(nvc.Get("txtRefNo")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlAttendingSupt")) : null
            , nvc != null ? General.GetNullableGuid(nvc.Get("ucChapter")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("Isrejected")) : null
            , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

        General.SetPrintOptions("gvInspectionSchedule", "Vetting Log", alCaptions, alColumns, ds);

        gvInspectionSchedule.DataSource = ds;
        gvInspectionSchedule.VirtualItemCount = iRowCount;
    }

    private void SetRowSelection()
    {
        //string a = Filter.CurrentSelectedInspectionSchedule.ToString();
        //gvInspectionSchedule.SelectedIndex = -1;
        //for (int i = 0; i < gvInspectionSchedule.Rows.Count; i++)
        //{
        //    if (gvInspectionSchedule.DataKeys[i].Value.ToString().Equals(Filter.CurrentSelectedInspectionSchedule.ToString()))
        //    {
        //        gvInspectionSchedule.SelectedIndex = i;

        //    }
        //}
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblInspectionScheduleId = (RadLabel)gvInspectionSchedule.Items[rowindex].FindControl("lblInspectionScheduleId");
            if (lblInspectionScheduleId != null)
            {
                Filter.CurrentSelectedInspectionSchedule = lblInspectionScheduleId.Text;
                //ifMoreInfo.Attributes["src"] = "../Inspection/InspectionScheduleGeneral.aspx?INSPECTIONSCHEDULEID=" + Filter.CurrentSelectedInspectionSchedule;
            }
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

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvInspectionSchedule_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("NAVIGATE"))
            {
                //  BindPageURL(nCurrentRow);
                SetRowSelection();
            }
            else if (e.CommandName.ToUpper().Equals("ATTACHMENT"))
            {
                // BindPageURL(nCurrentRow);
                SetRowSelection();
            }
            else if (e.CommandName.ToUpper().Equals("DEFICIENCYSUMMARY"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void DeleteInspectionSchedule(Guid inspectionscheduleid)
    {
        PhoenixInspectionSchedule.DeleteInspectionSchedule(PhoenixSecurityContext.CurrentSecurityContext.UserCode, inspectionscheduleid);
    }

    protected void gvInspectionSchedule_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvInspectionSchedule_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = sender as GridView;
        int nCurrentRow = e.RowIndex;

        RadLabel lblInspectionScheduleId = (RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblInspectionScheduleId");
        if (lblInspectionScheduleId != null)
            ViewState["SCHEDULEID"] = lblInspectionScheduleId.Text;
        ucConfirmComplete.Visible = true;
        ucConfirmComplete.Text = "Are you sure you want to Complete the 'Inspection'?";
    }


    protected void gvInspectionSchedule_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            string scheduleid = ((RadLabel)e.Item.FindControl("lblInspectionScheduleId")).Text;
            LinkButton lnkInspection = (LinkButton)e.Item.FindControl("lnkInspection");
            DataRowView dr = (DataRowView)e.Item.DataItem;
            LinkButton Communication = (LinkButton)e.Item.FindControl("lnkCommunication");
            RadLabel lblVesselsid = (RadLabel)e.Item.FindControl("lblVesselId");
            if (Communication != null)
            {
                Communication.Visible = SessionUtil.CanAccess(this.ViewState, Communication.CommandName);
                Communication.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonCommunication.aspx?Type=SIRECDI" + "&Referenceid=" + scheduleid + "&Vesselid=" + lblVesselsid.Text + "','large'); return true;");
            }
            if (lnkInspection != null)
            {
                lnkInspection.Attributes.Add("onclick", "javascript:openNewWindow('Report','','" + Session["sitepath"] + "/Inspection/InspectionVettingReport.aspx?SCHEDULEID=" + scheduleid + "&reffrom=log'); return true;");
            }

            RadLabel lblStatus = ((RadLabel)e.Item.FindControl("lblStatus"));
            UserControlToolTip ucToolTipStatus = (UserControlToolTip)e.Item.FindControl("ucToolTipStatus");
            ucToolTipStatus.Position = ToolTipPosition.TopCenter;
            ucToolTipStatus.TargetControlId = lblStatus.ClientID;

            LinkButton lnkBasis = (LinkButton)e.Item.FindControl("lnkBasis");
            RadLabel lblBasisId = (RadLabel)e.Item.FindControl("lblBasisId");
            if (lnkBasis != null)
                lnkBasis.Attributes.Add("onclick", "javascript:openNewWindow('Report','','" + Session["sitepath"] + "/Inspection/InspectionVettingReport.aspx?SCHEDULEID=" + lblBasisId.Text + "&reffrom=log&viewonly=1'); return true;");

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
                // att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionVettingAttachment.aspx?dtkey=" + lblDTkey.Text
                        + "&VESSELID=" + lblVesselid.Text
                        + "&viewonly=0"
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
                //   cmdDeficiencySummary.ImageUrl = Session["images"] + "/deficiency-noaction.png";
                cmdDeficiencySummary.Attributes.Add("onclick", "javascript:openNewWindow('Summary','','" + Session["sitepath"] + "/Inspection/InspectionDeficiencySummary.aspx?SOURCEID=" + scheduleid + "&VESSELID=" + lblVesselid.Text + "'); return true;");
            }

            LinkButton lnkvessel = (LinkButton)e.Item.FindControl("lnkVessel");
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblVesselId");
            if (lnkvessel != null)
            {
                lnkvessel.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Details','Dashboard/DashboardVesselDetails.aspx?vesselid=" + lblvesselid.Text + "');");
            }
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
        gvInspectionSchedule.Rebind();
        if (Session["NewSchedule"] != null && Session["NewSchedule"].ToString() == "Y")
        {
            Session["NewSchedule"] = "N";
            //BindPageURL(gvInspectionSchedule.SelectedIndex);
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void btnComplete_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                if (ViewState["SCHEDULEID"] != null)
                {
                    PhoenixInspectionSchedule.UpdateInspectionScheduleStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["SCHEDULEID"].ToString()));
                    gvInspectionSchedule.Rebind();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void gvInspectionSchedule_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInspectionSchedule.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}