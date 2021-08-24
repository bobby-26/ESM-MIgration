using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionDashboardUnsafeActsConditions : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();           
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardUnsafeActsConditions.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDirectIncident')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
           
            if (!IsPostBack)
            {
                VesselConfiguration();

                InspectionFilter.CurrentUnsafeActsConditionsDashboardFilter = null;

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                DateTime now = DateTime.Now;
                string FromDate = now.Date.AddMonths(-6).ToShortDateString();
                string ToDate = DateTime.Now.ToShortDateString();
                string Status = PhoenixCommonRegisters.GetHardCode(1, 146, "OPN");

                ViewState["FROMDATE"] = FromDate.ToString();
                ViewState["TODATE"] = ToDate.ToString();
                ViewState["Status"] = Status.ToString();
                ViewState["CODE"] = "";
                ViewState["OfficeDashboard"] = "";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["VESSELID"] = string.Empty;
                gvDirectIncident.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    ViewState["CODE"] = Request.QueryString["status"];
                }

                if (!string.IsNullOrEmpty(Request.QueryString["OfficeDashboard"]))
                {
                    ViewState["OfficeDashboard"] = Request.QueryString["OfficeDashboard"];
                }

                if (!string.IsNullOrEmpty(Request.QueryString["vslid"]))
                {
                    ViewState["VESSELID"] = Request.QueryString["vslid"];
                }

            }

            if (ViewState["OfficeDashboard"].ToString() == "1")
            {
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','', '" + Session["sitepath"] + "/Inspection/InspectionDashboardUnsafeActsConditionsFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
                toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardUnsafeActsConditions.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            }

            MenuRegistersInspection.AccessRights = this.ViewState;
            MenuRegistersInspection.MenuList = toolbar.Show();
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

        string[] alColumns = { "FLDVESSELNAME", "FLDINCIDENTNEARMISSREFNO", "FLDREPORTEDDATE", "FLDINCIDENTDATE", "FLDINCIDENTTIME", "FLDICCATEGORYNAME", "FLDICSUBCATEGORYNAME", "FLDSUMMARY", "FLDLOCATION", "FLDSTATUSNAME", "FLDCORRECTIVEACTION", "FLDROOTCAUSENAMELIST" };
        string[] alCaptions = { "Vessel", "Reference No.", "Reported", "Incident", "Time", "Category", "Sub-category", "Details", "Location", "Status", "Action Taken", "Root Cause" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds;

        if (ViewState["OfficeDashboard"].ToString().Equals("1"))
        {        
            NameValueCollection Dashboardnvc = InspectionFilter.CurrentUnsafeActsConditionsDashboardFilter;

                ds = PhoenixInspectionOfficeDashboard.DashboardOfficeDirectIncidentNearMissSearch(General.GetNullableString(ViewState["VESSELID"].ToString()== string.Empty? (Dashboardnvc != null ? Dashboardnvc["VesselList"]:null) : ViewState["VESSELID"].ToString())
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                                                                    , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
                                                                    , General.GetNullableString(ViewState["CODE"].ToString())
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                    , gvDirectIncident.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucCategory"] : null)
                                                                    , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ddlSubcategory"] : null)
                                                                    , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["txtReferenceNumber"] : null)
                                                                    , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["chkIncidentNearMissRaisedYN"] : null)
                                                                    , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucFrom"] : null)
                                                                    , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucTo"] : null)
                                                                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        }
        else
        {

            ds = PhoenixInspectionUnsafeActsConditions.DashboardDirectIncidentNearMissSearch(General.GetNullableString(ViewState["CODE"].ToString())
                                                                        , sortexpression
                                                                        , sortdirection
                                                                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                        , gvDirectIncident.PageSize
                                                                        , ref iRowCount
                                                                        , ref iTotalPageCount);
        }

        General.SetPrintOptions("gvDirectIncident", "Unsafe Acts / Conditions", alCaptions, alColumns, ds);

        gvDirectIncident.DataSource = ds;
        gvDirectIncident.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["directincidentid"] == null)
            {
                ViewState["directincidentid"] = ds.Tables[0].Rows[0]["FLDDIRECTINCIDENTID"].ToString();
                gvDirectIncident.SelectedIndexes.Clear();
            }
        }
        else
        {
            DataTable dt = ds.Tables[0];
        }

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME","FLDINCIDENTNEARMISSREFNO", "FLDREPORTEDDATE", "FLDINCIDENTDATE", "FLDINCIDENTTIME", "FLDICCATEGORYNAME", "FLDICSUBCATEGORYNAME", "FLDSUMMARY", "FLDLOCATION", "FLDSTATUSNAME", "FLDCORRECTIVEACTION", "FLDROOTCAUSENAMELIST" };
        string[] alCaptions = { "Vessel","Reference No.", "Reported", "Incident", "Time", "Category", "Sub-category", "Details", "Location", "Status", "Action Taken", "Root Cause" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds;

        if (ViewState["OfficeDashboard"].ToString().Equals("1"))
        {
            NameValueCollection Dashboardnvc = InspectionFilter.CurrentUnsafeActsConditionsDashboardFilter;

            ds = PhoenixInspectionOfficeDashboard.DashboardOfficeDirectIncidentNearMissSearch(General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null) : ViewState["VESSELID"].ToString())
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                                                                    , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
                                                                    , General.GetNullableString(ViewState["CODE"].ToString())
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                    , gvDirectIncident.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucCategory"] : null)
                                                                    , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ddlSubcategory"] : null)
                                                                    , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["txtReferenceNumber"] : null)
                                                                    , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["chkIncidentNearMissRaisedYN"] : null)
                                                                    , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucFrom"] : null)
                                                                    , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucTo"] : null)
                                                                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        }
        else
        {
            ds = PhoenixInspectionUnsafeActsConditions.DashboardDirectIncidentNearMissSearch(General.GetNullableString(ViewState["CODE"].ToString())
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                    , gvDirectIncident.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount);
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=UnsafeActs/ConditionsList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Unsafe Acts / Conditions</h3></td>");
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
    protected void RegistersInspection_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
               InspectionFilter.CurrentUnsafeActsConditionsDashboardFilter = null;
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
    private void SetRowSelection()
    {
        gvDirectIncident.SelectedIndexes.Clear();
        for (int i = 0; i < gvDirectIncident.Items.Count; i++)
        {
            if (gvDirectIncident.MasterTableView.Items[i].GetDataKeyValue("FLDDIRECTINCIDENTID").ToString().Equals(ViewState["directincidentid"].ToString()))
            {
                gvDirectIncident.SelectedIndexes.Clear();
                ViewState["directincidentid"] = ((RadLabel)gvDirectIncident.Items[i].FindControl("lblDirectIncidentId")).Text;
                gvDirectIncident.MasterTableView.Items[i].Selected = true;
                break;
            }
        }
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblDirectIncidentId = (RadLabel)gvDirectIncident.Items[rowindex].FindControl("lblDirectIncidentId");
            if (lblDirectIncidentId != null)
            {
                ViewState["directincidentid"] = lblDirectIncidentId.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvDirectIncident_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("CANCELACTION"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
    protected void gvDirectIncident_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblSummary = (RadLabel)e.Item.FindControl("lblSummaryFirstLine");
            if (lblSummary.Text != "")
            {
                //lblSummary.CssClass = "tooltip";
            }
            UserControlToolTip uctt = (UserControlToolTip)e.Item.FindControl("ucToolTipSummary");
            if (uctt != null)
            {
                uctt.Position = ToolTipPosition.TopCenter;
                uctt.TargetControlId = lblSummary.ClientID;
            }
            RadLabel lblActionTaken = (RadLabel)e.Item.FindControl("lblActionTaken");
            if (lblActionTaken.Text != "")
            {
                //lblActionTaken.CssClass = "tooltip";
            }
            UserControlToolTip uc = (UserControlToolTip)e.Item.FindControl("ucToolTipActionTaken");
            if (uc != null)
            {
                uc.Position = ToolTipPosition.TopCenter;
                uc.TargetControlId = lblActionTaken.ClientID;
            }

            LinkButton lnkRefno = (LinkButton)e.Item.FindControl("lnkRefno");
            RadLabel lblDirectIncidentId = (RadLabel)e.Item.FindControl("lblDirectIncidentId");
            if (lnkRefno != null)
            {
                lnkRefno.Visible = SessionUtil.CanAccess(this.ViewState, lnkRefno.CommandName);

                lnkRefno.Attributes.Add("onclick", "javascript:openNewWindow('uacts', '', '" + Session["sitepath"] + "/Inspection/InspectionUnsafeActsConditions.aspx?DashboardYN=1&directincidentid=" + lblDirectIncidentId.Text+ "&OfficeDashboard=" + ViewState["OfficeDashboard"] + "');");
            }

            LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cmdCancel != null && lblDirectIncidentId != null)
                cmdCancel.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionCancelReasonUpdate.aspx?REFERENCEID=" + lblDirectIncidentId.Text + "&TYPE=1','small'); return true;");

            if (cmdCancel != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName))
                    cmdCancel.Visible = false;
            }

            LinkButton lnkvessel = (LinkButton)e.Item.FindControl("lnkVessel");
            RadLabel lblvesselid = (RadLabel)e.Item.FindControl("lblVesselid");
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
    protected void gvDirectIncident_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDirectIncident.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvDirectIncident.SelectedIndexes.Clear();
        gvDirectIncident.EditIndexes.Clear();
        gvDirectIncident.DataSource = null;
        gvDirectIncident.Rebind();
    }    
}