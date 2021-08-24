using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class InspectionDashBoardMachineryDamageDBListExtn : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashBoardMachineryDamageDBListExtn.aspx?STATUS=OPN", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInspectionIncidentSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionDashBoardMachineryDamageListFilterExtn.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashBoardMachineryDamageDBListExtn.aspx?STATUS=OPN", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            
            MenuIncidentSearch.AccessRights = this.ViewState;
            MenuIncidentSearch.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Accidents", "ACCIDENTS");
            toolbarmain.AddButton("Near Miss", "NEARMISS");
            toolbarmain.AddButton("Machinery Damage", "MACHINERYDAMAGE");
            toolbarmain.AddButton("UC/ UACT", "UCUACT");
            MenuARSubTab.AccessRights = this.ViewState;
            MenuARSubTab.MenuList = toolbarmain.Show();
            MenuARSubTab.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {
                VesselConfiguration();

                ViewState["COMPANYID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["SECTIONID"] = "";
                ViewState["txtRefNo"] = string.Empty;
                ViewState["txtTitle"] = string.Empty;
                ViewState["VesselID"] = string.Empty;
                ViewState["ddlType"] = string.Empty;
                if (Request.QueryString["STATUS"] != null)
                    ViewState["Status"] = Request.QueryString["STATUS"].ToString();
                ViewState["ddlCategory"] = string.Empty;
                ViewState["ddlSubCategory"] = string.Empty;
                ViewState["ddlCon"] = string.Empty;
                ViewState["FRDATE"] = string.Empty;
                ViewState["TRDATE"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                
                ViewState["VESSELID"] = "";
                ViewState["FleetList"] = string.Empty;
                ViewState["Owner"] = string.Empty;
                ViewState["VesselTypeList"] = string.Empty;
                ViewState["VesselList"] = string.Empty;
                ViewState["ucActivity"] = string.Empty;
                ViewState["ddlProcessSubHazardId"] = string.Empty;
                ViewState["ddlPropertySubHazardId"] = string.Empty;
                ViewState["ucClosedFrom"] = string.Empty;
                ViewState["ucClosedTo"] = string.Empty;
                

                if (!string.IsNullOrEmpty(Request.QueryString["vslid"]))
                {
                    ViewState["VESSELID"] = Request.QueryString["vslid"];
                }
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ViewState["VesselID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                }

                string Status = PhoenixCommonRegisters.GetHardCode(1, 168, "OPN");
                if (!string.IsNullOrEmpty(Request.QueryString["STATUS"]))
                {
                    ViewState["CODE"] = Request.QueryString["STATUS"];
                }
                else
                    ViewState["CODE"] = Request.QueryString["code"];

                DateTime now = DateTime.Now;
                string FromDate = now.Date.AddMonths(-6).ToShortDateString();
                string ToDate = DateTime.Now.ToShortDateString();

                ViewState["FROMDATE"] = FromDate.ToString();
                ViewState["TODATE"] = ToDate.ToString();
                //ViewState["Status"] = Status.ToString();

                //Filter.CurrentSelectedIncidentMenu = null;
                if (Filter.CurrentSelectedIncidentMenu == null)
                {
                  //SetFilter();
                }

                if (InspectionFilter.CurrentMachineryDamageDashboardFilter != null)
                {
                    GetFilter();
                }

                if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"] != string.Empty)
                {
                    if (Request.QueryString["callfrom"] == "ilog")
                    {
                        Filter.CurrentIncidentID = null;
                        Filter.CurrentSelectedIncidentMenu = "ilog";
                    }
                    else if (Request.QueryString["callfrom"] == "irecord")
                    {
                        Filter.CurrentIncidentID = null;
                        Filter.CurrentSelectedIncidentMenu = null;
                    }
                }
                gvInspectionIncidentSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void Rebind()
    {
        gvInspectionIncidentSearch.SelectedIndexes.Clear();
        gvInspectionIncidentSearch.EditIndexes.Clear();
        gvInspectionIncidentSearch.DataSource = null;
        gvInspectionIncidentSearch.Rebind();
    }
    protected void IncidentGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Inspection/InspectionIncidentFilter.aspx");
        }
        if (CommandName.ToUpper().Equals("OFFICELIST"))
        {
            Response.Redirect("../Inspection/InspectionIncidentNearMissOfficelist.aspx");
        }
    }

    protected void MenuARSubTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("ACCIDENTS"))
        {
            Response.Redirect("../Inspection/InspectionDashboardOfficeIncidentListExtn.aspx?STATUS=S1");
        }
        if (CommandName.ToUpper().Equals("NEARMISS"))
        {
            Response.Redirect("../Inspection/InspectionDashboardOfficeNearmissListExtn.aspx?STATUS=S1");
        }
        if (CommandName.ToUpper().Equals("UCUACT"))
        {
            Response.Redirect("../Inspection/InspectionDashboardUnsafeActsAndConditions.aspx?OfficeDashboard=1&STATUS=OPN");
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = null;

        string[] alColumns = { "FLDVESSELNAME", "FLDREFERENCENUMBER", "FLDINCIDENTDATE", "FLDREPORTEDDATE", "FLDTITLE", "FLDCOMPONENTNAME", "FLDCONSEQUENCECATEGORYNAME", "FLDCARGONEARMISSYN", "FLDNAVIGATIONNEARMISSYN", "FLDINCIDENTREFNO", "FLDCLOSEDDATE", "FLDFLDSTATUSNAME" };
        string[] alCaptions = { "Vessel", "Reference No.", "Incident [LT]", "Reported", "Title", "Component", "Consequence Category", "Cargo Near Miss", "Navigation Near Miss", "Incident/Near Miss Ref.No", "Closed", "Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentIncidentFilterCriteria;
        NameValueCollection Dashboardnvc = InspectionFilter.CurrentMachineryDamageDashboardFilter;
   
        //if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
        //    vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Dashboardnvc == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }

        ds = PhoenixInspectionOfficeDashboard.DashBoardMachineryDamagelistSearch(General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null) : ViewState["VESSELID"].ToString())
                                                                , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                                                                , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                                                                , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
                                                                , General.GetNullableString(ViewState["Status"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvInspectionIncidentSearch.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ddlCategory"] : null)
                                                                , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ddlSubCategory"] : null)
                                                                , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ddlProcessSubHazardId"] : null)
                                                                , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ddlPropertySubHazardId"] : null)
                                                                , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["txtRefno"] : null)
                                                                , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["txtTitle"] : null)
                                                                , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ddlConsequenceCategory"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucIncidentFrom"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucIncidentTo"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucReportedFrom"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucReportedTo"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucClosedFrom"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucClosedTo"] : null)
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

        General.ShowExcel("Machinery Damage / Failure List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void IncidentSearch_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["VesselID"] = string.Empty;
                ViewState["txtRefNo"] = string.Empty;
                ViewState["txtTitle"] = string.Empty;
                ViewState["ddlType"] = string.Empty;
                if (Request.QueryString["STATUS"] != null)
                    ViewState["Status"] = Request.QueryString["STATUS"].ToString();
                //ViewState["Status"] = PhoenixCommonRegisters.GetHardCode(1, 168, "S1");
                ViewState["ddlCategory"] = string.Empty;
                ViewState["ddlSubCategory"] = string.Empty;
                ViewState["ddlCon"] = string.Empty;
                ViewState["FRDATE"] = string.Empty;
                ViewState["TRDATE"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;

                ViewState["FleetList"] = string.Empty;
                ViewState["Owner"] = string.Empty;
                ViewState["VesselTypeList"] = string.Empty;
                ViewState["VesselList"] = string.Empty;
                ViewState["ucActivity"] = string.Empty;

                InspectionFilter.CurrentMachineryDamageDashboardFilter = null;
                SetFilter();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? vesselid = null;

            string[] alColumns = { "FLDVESSELNAME", "FLDREFERENCENUMBER", "FLDINCIDENTDATE", "FLDREPORTEDDATE", "FLDTITLE", "FLDCOMPONENTNAME", "FLDCONSEQUENCECATEGORYNAME", "FLDCARGONEARMISSYN", "FLDNAVIGATIONNEARMISSYN", "FLDINCIDENTREFNO", "FLDCLOSEDDATE", "FLDFLDSTATUSNAME" };
            string[] alCaptions = { "Vessel", "Reference No.", "Incident [LT]", "Reported", "Title", "Component", "Consequence Category", "Cargo Near Miss", "Navigation Near Miss", "Incident/Near Miss Ref.No", "Closed", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null ? null : (ViewState["SORTEXPRESSION"].ToString()));
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = new DataSet();

            NameValueCollection nvc = Filter.CurrentIncidentFilterCriteria;

            if (nvc != null)
                vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

            //if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            //    vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

            NameValueCollection Dashboardnvc = InspectionFilter.CurrentMachineryDamageDashboardFilter;
            if (Dashboardnvc == null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                }
            }

            ds = PhoenixInspectionOfficeDashboard.DashBoardMachineryDamagelistSearch(General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null) : ViewState["VESSELID"].ToString())
                                                                , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                                                                , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                                                                , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
                                                                , General.GetNullableString(ViewState["Status"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvInspectionIncidentSearch.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ddlCategory"] : null)
                                                                , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ddlSubCategory"] : null)
                                                                , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ddlProcessSubHazardId"] : null)
                                                                , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ddlPropertySubHazardId"] : null)
                                                                , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["txtRefno"] : null)
                                                                , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["txtTitle"] : null)
                                                                , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ddlConsequenceCategory"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucIncidentFrom"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucIncidentTo"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucReportedFrom"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucReportedTo"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucClosedFrom"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucClosedTo"] : null)
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);


            General.SetPrintOptions("gvInspectionIncidentSearch", "Machinery Damage / Failure List", alCaptions, alColumns, ds);

            gvInspectionIncidentSearch.DataSource = ds;
            gvInspectionIncidentSearch.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Filter.CurrentIncidentID == null)
                {
                    gvInspectionIncidentSearch.SelectedIndexes.Clear();
                    Filter.CurrentIncidentVesselID = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                    Filter.CurrentIncidentID = ds.Tables[0].Rows[0]["FLDMACHINERYDAMAGEID"].ToString();
                   // ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                } 
                SetRowSelection();
            }
            else
            {
                ds.Tables[0].Rows.Clear();
                Filter.CurrentIncidentID = null;
            }
            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void gvInspectionIncidentSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                string lblStatusid = ((RadLabel)e.Item.FindControl("lblStatusid")).Text;
                Filter.CurrentIncidentID = ((RadLabel)e.Item.FindControl("lblInspectionIncidentId")).Text;
                Filter.CurrentIncidentTab = "INCIDENTDETAILS";
                BindPageURL(e.Item.ItemIndex);

                Response.Redirect("../Inspection/InspectionIncidentList.aspx?callfrom=irecord", false);
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string inspectionincidentid = ((RadLabel)e.Item.FindControl("lblInspectionIncidentId")).Text;
                Filter.CurrentIncidentID = null;
                BindPageURL(e.Item.ItemIndex);
                DeleteInspectionIncident(inspectionincidentid);
            }
            if (e.CommandName.ToUpper().Equals("CANCELINCIDENT"))
            {
                string inspectionincidentid = ((RadLabel)e.Item.FindControl("lblInspectionIncidentId")).Text;
                BindPageURL(e.Item.ItemIndex);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("CORRECTIVEACTIONS"))
            {
                string inspectionincidentid = ((RadLabel)e.Item.FindControl("lblInspectionIncidentId")).Text;
                Response.Redirect("../Inspection/InspectionIncidentActionsView.aspx?inspectionincidentid=" + inspectionincidentid, false);
                BindPageURL(e.Item.ItemIndex);
            }
            if (e.CommandName.ToUpper().Equals("INCIDENTREPORT"))
            {
                BindPageURL(e.Item.ItemIndex);
            }
            if (e.CommandName.ToUpper().Equals("THIRDPARTYINCIDENTREPORT"))
            {
                BindPageURL(e.Item.ItemIndex);
            }
            if (e.CommandName.ToUpper().Equals("DIRECTORCOMMENTS"))
            {
                string inspectionincidentid = ((RadLabel)e.Item.FindControl("lblInspectionIncidentId")).Text;
                BindPageURL(e.Item.ItemIndex);
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UNLOCK"))
            {
                if (Filter.CurrentIncidentID != null)
                {
                    PhoenixInspectionIncident.IncidentVesselUnlock(
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , new Guid(Filter.CurrentIncidentID)
                                                                    , 0
                                                             );
                    ucStatus.Text = "Investigation Unlocked in Vessel.";
                }
                BindPageURL(e.Item.ItemIndex);
                Rebind();
            }
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                Pair filterPair = (Pair)e.CommandArgument;
                ViewState["PAGENUMBER"] = 1;

                string daterange = gvInspectionIncidentSearch.MasterTableView.GetColumn("FLDREPORTEDDATE").CurrentFilterValue.ToString();
                if (daterange != string.Empty)
                {
                    ViewState["FRDATE"] = daterange.Split('~')[0];
                    ViewState["TRDATE"] = daterange.Split('~')[1];
                }
                string newdaterange = gvInspectionIncidentSearch.MasterTableView.GetColumn("FLDINCIDENTDATE").CurrentFilterValue.ToString();
                if (newdaterange != string.Empty)
                {
                    ViewState["FDATE"] = newdaterange.Split('~')[0];
                    ViewState["TDATE"] = newdaterange.Split('~')[1];
                }
                string closedaterange = gvInspectionIncidentSearch.MasterTableView.GetColumn("FLDCLOSEDDATE").CurrentFilterValue.ToString();
                if (closedaterange != string.Empty)
                {
                    ViewState["ucClosedFrom"] = newdaterange.Split('~')[0];
                    ViewState["ucClosedTo"] = newdaterange.Split('~')[1];
                }
                ViewState["txtRefNo"] = gvInspectionIncidentSearch.MasterTableView.GetColumn("FLDREFERENCENUMBER").CurrentFilterValue;
                ViewState["txtTitle"] = gvInspectionIncidentSearch.MasterTableView.GetColumn("FLDTITLE").CurrentFilterValue;

                SetFilter();
                Rebind();
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

    private void SetRowSelection()
    {
        try
        {
            gvInspectionIncidentSearch.SelectedIndexes.Clear();
            foreach (GridDataItem item in gvInspectionIncidentSearch.Items)
            {
                if (item.GetDataKeyValue("FLDMACHINERYDAMAGEID").ToString() == Filter.CurrentIncidentID)
                {
                    Filter.CurrentIncidentVesselID = (item["FLDVESSELID"].FindControl("lblVesselID") as RadLabel).Text;
                    //ViewState["DTKEY"] = (item["FLDINCIDENTREFNO"].FindControl("lbldtkey") as RadLabel).Text;
                    //break;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            Filter.CurrentIncidentID = ((RadLabel)gvInspectionIncidentSearch.Items[rowindex].FindControl("lblInspectionIncidentId")).Text;
            ViewState["DTKEY"] = ((RadLabel)gvInspectionIncidentSearch.Items[rowindex].FindControl("lbldtkey")).Text;
            Filter.CurrentIncidentVesselID = ((RadLabel)gvInspectionIncidentSearch.Items[rowindex].FindControl("lblVesselID")).Text;
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DeleteInspectionIncident(string inspectionincidentid)
    {
        PhoenixInspectionIncident.DeleteInspectionIncident(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(inspectionincidentid));
    }
    protected void gvInspectionIncidentSearch_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView dv = (DataRowView)e.Item.DataItem;

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                //LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
                //if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

                LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

                RadLabel lblMachineryDamageId = (RadLabel)e.Item.FindControl("lblMachineryDamageId");
                LinkButton riskCreate = (LinkButton)e.Item.FindControl("lnkReferenceNumber");
                if (riskCreate != null && lblMachineryDamageId!=null)
                {
                    riskCreate.Visible = SessionUtil.CanAccess(this.ViewState, riskCreate.CommandName);
                    riskCreate.Attributes.Add("onclick", "javascript:openNewWindow('ra', '', '" + Session["sitepath"] + "/Inspection/InspectionMachineryDamageGeneral.aspx?DashboardYN=1&MACHINERYDAMAGEID=" + lblMachineryDamageId.Text + "&PageNo=" + ViewState["PAGENUMBER"] + "');");
                }

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                    if (dv != null && General.GetNullableInteger(dv["FLDSHOWDELETEBUTTONYN"].ToString()) == 1)
                        db.Visible = true;
                    else
                        db.Visible = false;
                }

                

                LinkButton cmdMachinery = (LinkButton)e.Item.FindControl("cmdMachinery");
                if (cmdMachinery != null && lblMachineryDamageId != null)
                {
                    cmdMachinery.Attributes.Add("onclick", "openNewWindow('Report', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=MACHINERYDAMAGE&machinerydamageId=" + lblMachineryDamageId.Text + "&showmenu=0&showexcel=NO');return false;");
                }

                LinkButton lnkVessel = (LinkButton)e.Item.FindControl("lnkVessel");
                RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");
                if (lnkVessel != null)
                {
                    lnkVessel.Attributes.Add("onclick", "javascript:top.openNewWindow('maint','Details','Dashboard/DashboardVesselDetails.aspx?vesselid=" + lblVesselid.Text + "');");
                }
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
        if (Filter.CurrentIncidentFilterCriteria != null)
        {
            GetFilter();
        }
        Rebind();
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
    protected void gvInspectionIncidentSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInspectionIncidentSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvInspectionIncidentSearch_SortCommand1(object sender, GridSortCommandEventArgs e)
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
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
    protected void ucVessel_DataBinding(object sender, EventArgs e)
    {
        DataSet dt = new DataSet();
        dt = PhoenixRegistersVessel.ListOwnerAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(null), General.GetNullableInteger(ViewState["COMPANYID"].ToString()), General.GetNullableInteger(null), 0);
        RadComboBox ucVessel = sender as RadComboBox;
        ucVessel.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ucVessel.DataSource = dt;

        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = dt.Tables[0].Columns["FLDVESSELID"];
        dt.Tables[0].PrimaryKey = keyColumns;
       

        if (ViewState["VesselID"] != null && ViewState["VesselID"].ToString() != "")
        {
            if (dt.Tables[0].Rows.Contains(ViewState["VesselID"].ToString()))
            {
                ucVessel.SelectedValue = ViewState["VesselID"].ToString();
            }
        }

    }
    protected void ucVessel_DataBinding_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDVESSELID").CurrentFilterValue = e.Value;
        ViewState["VesselID"] = e.Value;
        ViewState["VESSELID"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
    protected void ddlStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDSTATUSOFINCIDENT").CurrentFilterValue = e.Value;
        ViewState["Status"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
    private void SetFilter()
    {
        NameValueCollection criteria = new NameValueCollection();

        //if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        //{
        //    ViewState["VesselID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        //}

        

        criteria.Add("FleetList", ViewState["FleetList"].ToString());
        criteria.Add("Owner", ViewState["Owner"].ToString());
        criteria.Add("VesselTypeList", ViewState["VesselTypeList"].ToString());
        criteria.Add("VesselList", ViewState["VesselList"].ToString());
        criteria.Add("ucActivity", ViewState["ucActivity"].ToString());
        criteria.Add("ucCategory", ViewState["ddlCategory"].ToString());
        criteria.Add("ucSubCategory", ViewState["ddlSubCategory"].ToString());
        criteria.Add("txtRefNo", ViewState["txtRefNo"].ToString());
        criteria.Add("txtTitle", ViewState["txtTitle"].ToString());
        criteria.Add("ucConsequenceCategory", ViewState["ddlCon"].ToString());
        criteria.Add("ucIncidentFrom", ViewState["FDATE"].ToString());
        criteria.Add("ucIncidentTo", ViewState["TDATE"].ToString());
        criteria.Add("ucReportedFrom", ViewState["FRDATE"].ToString());
        criteria.Add("ucReportedTo", ViewState["TRDATE"].ToString());
        criteria.Add("ucStatus", ViewState["Status"].ToString());
        criteria.Add("ddlProcessSubHazardId", ViewState["ddlProcessSubHazardId"].ToString());
        criteria.Add("ddlPropertySubHazardId", ViewState["ddlPropertySubHazardId"].ToString());
        criteria.Add("ucClosedFrom", ViewState["ucClosedFrom"].ToString());
        criteria.Add("ucClosedTo", ViewState["ucClosedTo"].ToString());

        InspectionFilter.CurrentMachineryDamageDashboardFilter = criteria;
    }

    private void GetFilter()
    {
        NameValueCollection nvc = InspectionFilter.CurrentMachineryDamageDashboardFilter;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            ViewState["VesselID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }

        ViewState["txtRefNo"] = General.GetNullableString(nvc.Get("txtRefNo")) == null ? string.Empty : General.GetNullableString(nvc.Get("txtRefNo"));
        ViewState["txtTitle"] = General.GetNullableString(nvc.Get("txtTitle")) == null ? string.Empty : General.GetNullableString(nvc.Get("txtTitle"));
        ViewState["FDATE"] = General.GetNullableString(nvc.Get("ucIncidentFrom")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucIncidentFrom"));
        ViewState["TDATE"] = General.GetNullableString(nvc.Get("ucIncidentTo")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucIncidentTo"));
        ViewState["ucActivity"] = General.GetNullableString(nvc.Get("ucActivity")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucActivity"));
        ViewState["VesselList"] = General.GetNullableString(nvc.Get("VesselList")) == null ? string.Empty : General.GetNullableString(nvc.Get("VesselList"));
        ViewState["ddlType"] = General.GetNullableString(nvc.Get("ddlIncidentNearmiss")) == null ? string.Empty : General.GetNullableString(nvc.Get("ddlIncidentNearmiss"));
        ViewState["ddlCategory"] = General.GetNullableString(nvc.Get("ucCategory")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucCategory"));
        ViewState["ddlSubCategory"] = General.GetNullableString(nvc.Get("ucSubCategory")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucSubCategory"));
        ViewState["FRDATE"] = General.GetNullableString(nvc.Get("ucReportedFrom")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucReportedFrom"));
        ViewState["TRDATE"] = General.GetNullableString(nvc.Get("ucReportedTo")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucReportedTo"));
        ViewState["ddlCon"] = General.GetNullableString(nvc.Get("ucConsequenceCategory")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucConsequenceCategory"));
        ViewState["FleetList"]= General.GetNullableString(nvc.Get("FleetList")) == null ? string.Empty : General.GetNullableString(nvc.Get("FleetList"));
        ViewState["Owner"]= General.GetNullableString(nvc.Get("Owner")) == null ? string.Empty : General.GetNullableString(nvc.Get("Owner"));
        ViewState["VesselTypeList"]= General.GetNullableString(nvc.Get("VesselTypeList")) == null ? string.Empty : General.GetNullableString(nvc.Get("VesselTypeList"));
        ViewState["Status"] = General.GetNullableString(nvc.Get("ucStatus")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucStatus"));
        ViewState["ddlProcessSubHazardId"]= General.GetNullableString(nvc.Get("ddlProcessSubHazardId")) == null ? string.Empty : General.GetNullableString(nvc.Get("ddlProcessSubHazardId"));
        ViewState["ddlPropertySubHazardId"]= General.GetNullableString(nvc.Get("ddlPropertySubHazardId")) == null ? string.Empty : General.GetNullableString(nvc.Get("ddlPropertySubHazardId"));
        ViewState["ucClosedFrom"]= General.GetNullableString(nvc.Get("ucClosedFrom")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucClosedFrom"));
        ViewState["ucClosedTo"]=General.GetNullableString(nvc.Get("ucClosedTo")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucClosedTo"));
    }


    protected void ddlIncidentNearmiss_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDISINCIDENTORNEARMISS").CurrentFilterValue = e.Value;
        ViewState["ddlType"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    //protected void ddlCategory_DataBinding(object sender, EventArgs e)
    //{
    //    DataSet dt = new DataSet();
    //    dt = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(General.GetNullableInteger(ViewState["ddlType"].ToString()));
    //    RadComboBox ddlCategory = sender as RadComboBox;
    //    ddlCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    //    ddlCategory.DataSource = dt;
    //}
    protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDCATEGORY").CurrentFilterValue = e.Value;
        ViewState["ddlCategory"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
    //protected void ddlSubCategory_DataBinding(object sender, EventArgs e)
    //{
    //    DataSet dt = new DataSet();
    //    dt = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(ViewState["ddlCategory"].ToString()));
    //    RadComboBox ddlSubCategory = sender as RadComboBox;
    //    ddlSubCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    //    ddlSubCategory.DataSource = dt;
    //}
    protected void ddlSubCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDSUBCATEGORY").CurrentFilterValue = e.Value;
        ViewState["ddlSubCategory"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void ddlStatus_DataBinding(object sender, EventArgs e)
    {
        DataSet dst = new DataSet();
        dst = PhoenixInspectionMachineryDamage.MachineryDamageStatusList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        RadComboBox ddlStatus = sender as RadComboBox;
        ddlStatus.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ddlStatus.DataSource = dst;
    }

    protected void ddlCon_DataBinding(object sender, EventArgs e)
    {
        DataSet dst = new DataSet();
        dst = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 169, 0, null);
        RadComboBox ddlCon = sender as RadComboBox;
        ddlCon.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ddlCon.DataSource = dst;
    }

    protected void ddlCon_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDCONSEQUENCECATEGORYNAME").CurrentFilterValue = e.Value;
        ViewState["ddlCon"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
}
