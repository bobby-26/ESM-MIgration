using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionOfficeDashboardShipBoardTasks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionOfficeDashboardShipBoardTasks.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvShipBoardTasks')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        
        if (!IsPostBack)
        {
            InspectionFilter.CurrentShipTaskDashboardFilter = null;

            Session["New"] = "N";
            ViewState["STATUS"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["CARYN"] = "";

            if (Request.QueryString["STATUS"] != null && Request.QueryString["STATUS"].ToString() != "")
                ViewState["STATUS"] = Request.QueryString["STATUS"].ToString();
            else
                ViewState["STATUS"] = "";

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
            ViewState["VESSELID"] = string.Empty;

            gvShipBoardTasks.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            if (Request.QueryString["TASKID"] != null && Request.QueryString["TASKID"].ToString() != "")
                ViewState["TASKID"] = Request.QueryString["TASKID"].ToString();
            else
                ViewState["TASKID"] = null;

            if (Request.QueryString["DTKEY"] != null && Request.QueryString["DTKEY"].ToString() != null)
                ViewState["DTKEY"] = Request.QueryString["DTKEY"];
            else
                ViewState["DTKEY"] = null;

            if (Request.QueryString["TASK"] != null && Request.QueryString["TASK"].ToString() != "")
                ViewState["TASK"] = Request.QueryString["TASK"].ToString();
            else
                ViewState["TASK"] = "";

            if (!string.IsNullOrEmpty(Request.QueryString["vslid"]))
            {
                ViewState["VESSELID"] = Request.QueryString["vslid"];
            }

        }
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','', '" + Session["sitepath"] + "/Inspection/InspectionOfficeDashboardShipBoardTasksFilter.aspx?TASK=" + ViewState["TASK"].ToString() + "')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
        toolbar.AddFontAwesomeButton("../Inspection/InspectionOfficeDashboardShipBoardTasks.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        MenuShipBoardTasks.AccessRights = this.ViewState;
        MenuShipBoardTasks.MenuList = toolbar.Show();
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
            InspectionFilter.CurrentShipTaskDashboardFilter = null;
            ViewState["PAGENUMBER"] = 1;
            ReBind();
        }
    }
    protected void ReBind()
    {
        gvShipBoardTasks.SelectedIndexes.Clear();
        gvShipBoardTasks.EditIndexes.Clear();
        gvShipBoardTasks.DataSource = null;
        gvShipBoardTasks.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDCREATEDFROMNAME", "FLDSOURCENAME", "FLDHARDNAME", "FLDDEPARTMENTNAME", "FLDITEMNAME", "FLDDEFICIENCYDETAILS", "FLDTASK", "FLDTARGETDATE" };
        string[] alCaptions = { "Vessel", "Source Reference", "Source", "Type", "Assigned Department", "Item", "Deficiency Details", "Task", "Target" };

        DataSet ds;
        NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
        NameValueCollection Dashboardnvc = InspectionFilter.CurrentShipTaskDashboardFilter;

        if (ViewState["TASK"].ToString().Equals("CAR"))
        {
            ds = ds = PhoenixInspectionOfficeDashboard.InspectionDashboardCorrectiveTaskList(General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null) : ViewState["VESSELID"].ToString())
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                                                                    , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
                                                                , General.GetNullableString(ViewState["STATUS"].ToString())
                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvShipBoardTasks.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ucInspectionType")) : null
                                                                , Dashboardnvc != null ? General.GetNullableGuid(Dashboardnvc.Get("ucInspection")) : null
                                                                , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ddlDefType")) : null
                                                                , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ucNonConformanceCategory")) : null
                                                                , Dashboardnvc != null ? General.GetNullableGuid(Dashboardnvc.Get("ucChapter")) : null
                                                                , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ddlSourceType")) : null
                                                                , Dashboardnvc != null ? General.GetNullableString(Dashboardnvc.Get("txtSourceRefNo")) : null
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc.Get("ucFrom") : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc.Get("ucTo") : null)
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            General.SetPrintOptions("gvShipBoardTasks", "Corrective Tasks", alCaptions, alColumns, ds);
            gvShipBoardTasks.DataSource = ds;
            gvShipBoardTasks.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        if (ViewState["TASK"].ToString().Equals("PAR"))
        {
            ds = ds = PhoenixInspectionOfficeDashboard.InspectionDashboardPreventiveTaskList(General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null) : ViewState["VESSELID"].ToString())
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                                                                    , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
                                                                           , General.GetNullableString(ViewState["STATUS"].ToString())
                                                                           , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                           , gvShipBoardTasks.PageSize
                                                                           , ref iRowCount
                                                                           , ref iTotalPageCount
                                                                           , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ucInspectionType")) : null
                                                                           , Dashboardnvc != null ? General.GetNullableGuid(Dashboardnvc.Get("ucInspection")) : null
                                                                           , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ddlDefType")) : null
                                                                           , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ucNonConformanceCategory")) : null
                                                                           , Dashboardnvc != null ? General.GetNullableGuid(Dashboardnvc.Get("ucChapter")) : null
                                                                           , General.GetNullableInteger(Dashboardnvc !=null ? Dashboardnvc["ddlSourceType"] : null )
                                                                           , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["txtSourceRefNo"] : null)
                                                                           , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc.Get("ucFrom") : null)
                                                                           , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc.Get("ucTo") : null)
                                                                           , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            General.SetPrintOptions("gvShipBoardTasks", "Preventive Tasks", alCaptions, alColumns, ds);
            gvShipBoardTasks.DataSource = ds;
            gvShipBoardTasks.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }

        if (ViewState["TASK"].ToString().Equals("VIR"))
        {
            ds = ds = PhoenixInspectionOfficeDashboard.InspectionDashboardVIRTaskList(General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null) : ViewState["VESSELID"].ToString())
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                                                                    , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
                                                                , General.GetNullableString(ViewState["STATUS"].ToString())
                                                                , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvShipBoardTasks.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ddlDefType")) : null
                                                                , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ucNonConformanceCategory")) : null
                                                                , Dashboardnvc != null ? General.GetNullableGuid(Dashboardnvc.Get("ucChapter")) : null
                                                                , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ddlSourceType")) : null
                                                                , Dashboardnvc != null ? General.GetNullableString(Dashboardnvc.Get("txtSourceRefNo")) : null
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc.Get("ucFrom") : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc.Get("ucTo") : null)
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            General.SetPrintOptions("gvShipBoardTasks", "VIR Tasks", alCaptions, alColumns, ds);
            gvShipBoardTasks.DataSource = ds;
            gvShipBoardTasks.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }





    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDCREATEDFROMNAME", "FLDSOURCENAME", "FLDHARDNAME", "FLDDEPARTMENTNAME", "FLDITEMNAME", "FLDDEFICIENCYDETAILS", "FLDTASK", "FLDTARGETDATE" };
        string[] alCaptions = { "Vessel", "Source Reference", "Source", "Type", "Assigned Department", "Item", "Deficiency Details", "Task", "Target" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
        NameValueCollection Dashboardnvc = InspectionFilter.CurrentShipTaskDashboardFilter;
        DataSet ds;

        if (ViewState["TASK"].ToString().Equals("CAR"))
        {
           ds = PhoenixInspectionOfficeDashboard.InspectionDashboardCorrectiveTaskList(General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null) : ViewState["VESSELID"].ToString())
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                                                                    , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
                                                                , General.GetNullableString(ViewState["STATUS"].ToString())
                                                                , 1
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ucInspectionType")) : null
                                                                , Dashboardnvc != null ? General.GetNullableGuid(Dashboardnvc.Get("ucInspection")) : null
                                                                , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ddlDefType")) : null
                                                                , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ucNonConformanceCategory")) : null
                                                                , Dashboardnvc != null ? General.GetNullableGuid(Dashboardnvc.Get("ucChapter")) : null
                                                                , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ddlSourceType")) : null
                                                                , Dashboardnvc != null ? General.GetNullableString(Dashboardnvc.Get("txtSourceRefNo")) : null
                                                                , Dashboardnvc != null ? General.GetNullableDateTime(Dashboardnvc.Get("ucFrom")) : null
                                                                , Dashboardnvc != null ? General.GetNullableDateTime(Dashboardnvc.Get("ucTo")) : null
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
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

        if (ViewState["TASK"].ToString().Equals("PAR"))
        {
            ds = PhoenixInspectionOfficeDashboard.InspectionDashboardPreventiveTaskList(General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null) : ViewState["VESSELID"].ToString())
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                                                                    , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
                                                                               , General.GetNullableString(ViewState["STATUS"].ToString())
                                                                               , 1
                                                                               , iRowCount
                                                                               , ref iRowCount
                                                                               , ref iTotalPageCount
                                                                               , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ucInspectionType")) : null
                                                                               , Dashboardnvc != null ? General.GetNullableGuid(Dashboardnvc.Get("ucInspection")) : null
                                                                               , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ddlDefType")) : null
                                                                               , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ucNonConformanceCategory")) : null
                                                                               , Dashboardnvc != null ? General.GetNullableGuid(Dashboardnvc.Get("ucChapter")) : null
                                                                               , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ddlSourceType"] : null)
                                                                               , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["txtSourceRefNo"] : null)
                                                                               , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc.Get("ucFrom") : null)
                                                                               , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc.Get("ucTo") : null)
                                                                               , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
            Response.AddHeader("Content-Disposition", "attachment; filename=TaskList.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Preventive Tasks</h3></td>");
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
        if(ViewState["TASK"].ToString().Equals("VIR"))
        {
            ds = PhoenixInspectionOfficeDashboard.InspectionDashboardVIRTaskList(General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null) : ViewState["VESSELID"].ToString())
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                                                                    , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
                                                                , General.GetNullableString(ViewState["STATUS"].ToString())
                                                                , 1
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ddlDefType")) : null
                                                                , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ucNonConformanceCategory")) : null
                                                                , Dashboardnvc != null ? General.GetNullableGuid(Dashboardnvc.Get("ucChapter")) : null
                                                                , Dashboardnvc != null ? General.GetNullableInteger(Dashboardnvc.Get("ddlSourceType")) : null
                                                                , Dashboardnvc != null ? General.GetNullableString(Dashboardnvc.Get("txtSourceRefNo")) : null
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc.Get("ucFrom") : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc.Get("ucTo") : null)
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            Response.AddHeader("Content-Disposition", "attachment; filename=TaskList.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>VIR Tasks</h3></td>");
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

        
    }
    protected void gvShipBoardTasks_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nRow = e.Item.ItemIndex;

                if (e.CommandName.ToUpper().Equals("SELECT"))
                {
                    RadLabel lblCorrectiveActionid = (RadLabel)_gridView.Items[nRow].FindControl("lblLongTermActionId");
                    RadLabel lbl = (RadLabel)_gridView.Items[nRow].FindControl("lblcaryntext");

                    
                }
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
    protected void gvShipBoardTasks_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton lnkRefno = (LinkButton)e.Item.FindControl("lnkTaskDetails");
            RadLabel lblCorrectiveActionid = (RadLabel)e.Item.FindControl("lblCorrectiveActionid");
            RadLabel lblcaryntext = (RadLabel)e.Item.FindControl("lblcaryntext");
            if (lnkRefno != null)
            {
                lnkRefno.Visible = SessionUtil.CanAccess(this.ViewState, lnkRefno.CommandName);

                if (lblcaryntext.Text.Equals("1"))
                {
                    lnkRefno.Attributes.Add("onclick", "javascript:openNewWindow('Tasks', '', '" + Session["sitepath"] + "/Inspection/InspectionShipBoardTasksDetails.aspx?correctiveactionid=" + lblCorrectiveActionid.Text + "&DASHBOARD=1" + "');");
                }
                if (lblcaryntext.Text.Equals("0"))
                {
                    lnkRefno.Attributes.Add("onclick", "javascript:openNewWindow('Tasks', '', '" + Session["sitepath"] + "/Inspection/InspectionPreventiveTasksDetails.aspx?preventiveactionid=" + lblCorrectiveActionid.Text + "&DASHBOARD=1" + "');");
                }

                LinkButton lnkvessel = (LinkButton)e.Item.FindControl("lnkVessel");
                RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblvesselid");
                if (lnkvessel != null)
                {
                    lnkvessel.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Details','Dashboard/DashboardVesselDetails.aspx?vesselid=" + lblvesselid.Text + "');");
                }
            }
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