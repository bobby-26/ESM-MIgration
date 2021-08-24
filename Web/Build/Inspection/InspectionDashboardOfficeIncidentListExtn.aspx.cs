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

public partial class InspectionDashboardOfficeIncidentListExtn : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardOfficeIncidentListExtn.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInspectionIncidentSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionDashboardOfficeIncidentNearMissListFilterExtn.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardOfficeIncidentListExtn.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            
            MenuIncidentSearch.AccessRights = this.ViewState;
            MenuIncidentSearch.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Accidents", "ACCIDENTS");
            toolbarmain.AddButton("Near Miss", "NEARMISS");
            toolbarmain.AddButton("Machinery Damage", "MACHINERYDAMAGE");
            toolbarmain.AddButton("UC/ UACT", "UCUACT");
            MenuARSubTab.AccessRights = this.ViewState;
            MenuARSubTab.MenuList = toolbarmain.Show();
            MenuARSubTab.SelectedMenuIndex = 0;

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
                ViewState["Status"] = PhoenixCommonRegisters.GetHardCode(1, 168, "S1");
                ViewState["ddlCategory"] = string.Empty;
                ViewState["ddlSubCategory"] = string.Empty;
                ViewState["ddlCon"] = string.Empty;
                ViewState["FRDATE"] = string.Empty;
                ViewState["TRDATE"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                ViewState["CODE"] = "";
                ViewState["VESSELID"] = "";
                ViewState["FleetList"] = string.Empty;
                ViewState["Owner"] = string.Empty;
                ViewState["VesselTypeList"] = string.Empty;
                ViewState["VesselList"] = string.Empty;
                ViewState["ucActivity"] = string.Empty;
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

                string Status = PhoenixCommonRegisters.GetHardCode(1, 168, "S1");
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
                ViewState["Status"] = Status.ToString();

                //Filter.CurrentSelectedIncidentMenu = null;
                if (Filter.CurrentSelectedIncidentMenu == null)
                {
                  //SetFilter();
                }

                if (InspectionFilter.CurrentIncidentnearmissDashboardFilter != null)
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
        if (CommandName.ToUpper().Equals("NEARMISS"))
        {
            Response.Redirect("../Inspection/InspectionDashboardOfficeNearmissListExtn.aspx?STATUS=S1");
        }
        if (CommandName.ToUpper().Equals("MACHINERYDAMAGE"))
        {
            Response.Redirect("../Inspection/InspectionDashBoardMachineryDamageDBListExtn.aspx?STATUS=OPN");
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

        string[] alColumns = { "FLDVESSELNAME", "FLDINCIDENTREFNO", "FLDINCIDENTCLASSIFICATION", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDCONSEQUENCECATEGORY", "FLDINCIDENTTITLE", "FLDREPORTEDDATE", "FLDINCIDENTDATE", "FLDSTATUSOFINCIDENTNAME" };
        string[] alCaptions = { "Vessel", "Ref. No", "Type", "Category", "Subcategory", "Consequence Category", "Title", "Reported", "Incident", "Status" };

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
        NameValueCollection Dashboardnvc = InspectionFilter.CurrentIncidentnearmissDashboardFilter;
   
        //if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
        //    vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Dashboardnvc == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }

        ds = PhoenixInspectionOfficeDashboard.InspectionOfficeDashboardIncidentnearmissSearch(
                General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null) : ViewState["VESSELID"].ToString())
                , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
            , Dashboardnvc != null ? General.GetNullableString(Dashboardnvc.Get("ucStatus")) : General.GetNullableString(ViewState["Status"].ToString())
            , sortexpression, sortdirection,
            1, // accidant
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount
            , 1//General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ddlIncidentNearmiss"] : "1")
            , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucActivity"] : null)
            , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucCategory"] : null)
            , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucSubCategory"] : null)
            , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["txtRefNo"] : null)
            , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["txtTitle"] : null)
            , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucConsequenceCategory"] : null)
            , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucIncidentFrom"] : null)
            , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucIncidentTo"] : null)
            , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucReportedFrom"] : null)
            , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucReportedTo"] : null)
            , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

        General.ShowExcel("Accident", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        //Response.AddHeader("Content-Disposition", "attachment; filename=Accident.xls");
        //Response.ContentType = "application/vnd.msexcel";
        //Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        //Response.Write("<tr>");
        //Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        //Response.Write("<td><h3>Accident</h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        //Response.Write("</tr>");
        //Response.Write("</TABLE>");
        //Response.Write("<br />");
        //Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        //Response.Write("<tr>");
        //for (int i = 0; i < alCaptions.Length; i++)
        //{
        //    Response.Write("<td width='20%'>");
        //    Response.Write("<b>" + alCaptions[i] + "</b>");
        //    Response.Write("</td>");
        //}
        //Response.Write("</tr>");
        //foreach (DataRow dr in ds.Tables[0].Rows)
        //{
        //    Response.Write("<tr>");
        //    for (int i = 0; i < alColumns.Length; i++)
        //    {
        //        Response.Write("<td>");
        //        Response.Write(dr[alColumns[i]]);
        //        Response.Write("</td>");
        //    }
        //    Response.Write("</tr>");
        //}
        //Response.Write("</TABLE>");
        //Response.End();
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
                ViewState["Status"] = PhoenixCommonRegisters.GetHardCode(1, 168, "S1");
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

                InspectionFilter.CurrentIncidentnearmissDashboardFilter = null;
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

            string[] alColumns = { "FLDVESSELNAME", "FLDINCIDENTREFNO", "FLDINCIDENTCLASSIFICATION", "FLDCATEGORYNAME", "FLDSUBCATEGORYNAME", "FLDCONSEQUENCECATEGORY", "FLDINCIDENTTITLE", "FLDREPORTEDDATE", "FLDINCIDENTDATE", "FLDSTATUSOFINCIDENTNAME" };
            string[] alCaptions = { "Vessel", "Ref. No", "Type", "Category", "Subcategory", "Consequence Category", "Title", "Reported", "Incident", "Status" };

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

            NameValueCollection Dashboardnvc = InspectionFilter.CurrentIncidentnearmissDashboardFilter;
            if (Dashboardnvc == null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                }
            }
            
            ds = PhoenixInspectionOfficeDashboard.InspectionOfficeDashboardIncidentnearmissSearch(General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null) : ViewState["VESSELID"].ToString())
                                                                     , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                                                                     , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                                                                     , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
                                                                     , Dashboardnvc != null ? General.GetNullableString(Dashboardnvc.Get("ucStatus")) : General.GetNullableString(ViewState["Status"].ToString())
                                                                     , sortexpression, sortdirection,
                                                                     (int)ViewState["PAGENUMBER"],
                                                                     gvInspectionIncidentSearch.PageSize,
                                                                     ref iRowCount,
                                                                     ref iTotalPageCount
                                                                     , 1//General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ddlIncidentNearmiss"] : "1")
                                                                     , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucActivity"] : null)
                                                                     , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucCategory"] : null)
                                                                     , General.GetNullableGuid(Dashboardnvc != null ? Dashboardnvc["ucSubCategory"] : null)
                                                                     , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["txtRefNo"] : null)
                                                                     , General.GetNullableString(Dashboardnvc != null ? Dashboardnvc["txtTitle"] : null)
                                                                     , General.GetNullableInteger(Dashboardnvc != null ? Dashboardnvc["ucConsequenceCategory"] : null)
                                                                     , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucIncidentFrom"] : null)
                                                                     , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucIncidentTo"] : null)
                                                                     , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucReportedFrom"] : null)
                                                                     , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucReportedTo"] : null)
                                                                     , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            General.SetPrintOptions("gvInspectionIncidentSearch", "Accident", alCaptions, alColumns, ds);

            gvInspectionIncidentSearch.DataSource = ds;
            gvInspectionIncidentSearch.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (Filter.CurrentIncidentID == null)
                {
                    gvInspectionIncidentSearch.SelectedIndexes.Clear();
                    Filter.CurrentIncidentVesselID = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                    Filter.CurrentIncidentID = ds.Tables[0].Rows[0]["FLDINSPECTIONINCIDENTID"].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
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

               // Response.Redirect("../Inspection/InspectionIncidentList.aspx?callfrom=irecord", false);
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
                ViewState["txtRefNo"] = gvInspectionIncidentSearch.MasterTableView.GetColumn("FLDINCIDENTREFNO").CurrentFilterValue;
                ViewState["txtTitle"] = gvInspectionIncidentSearch.MasterTableView.GetColumn("FLDINCIDENTTITLE").CurrentFilterValue;

                SetFilter();
                ViewState["PAGENUMBER"] = 1;
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
                if (item.GetDataKeyValue("FLDINSPECTIONINCIDENTID").ToString() == Filter.CurrentIncidentID)
                {
                    Filter.CurrentIncidentVesselID = (item["FLDINCIDENTREFNO"].FindControl("lblVesselID") as RadLabel).Text;
                    ViewState["DTKEY"] = (item["FLDINCIDENTREFNO"].FindControl("lbldtkey") as RadLabel).Text;
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
                LinkButton Communication = (LinkButton)e.Item.FindControl("lnkCommunication");
                RadLabel lblVesselsid = (RadLabel)e.Item.FindControl("lblVesselID");
                RadLabel lblInspectionIncidentId = (RadLabel)e.Item.FindControl("lblInspectionIncidentId");
                if (Communication != null)
                {
                    Communication.Visible = SessionUtil.CanAccess(this.ViewState, Communication.CommandName);
                    Communication.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonCommunication.aspx?Type=INCIDENT" + "&Referenceid=" + lblInspectionIncidentId.Text + "&Vesselid=" + lblVesselsid.Text + "','large'); return true;");
                }
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

                LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cb != null && lblInspectionIncidentId != null) cb.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionCancelReasonUpdate.aspx?REFERENCEID=" + lblInspectionIncidentId.Text + "&TYPE=2','medium'); return true;");
                if (!SessionUtil.CanAccess(this.ViewState, cb.CommandName)) cb.Visible = false;
                LinkButton cmdUnlockIncident = (LinkButton)e.Item.FindControl("cmdUnlockIncident");
                if (!SessionUtil.CanAccess(this.ViewState, cmdUnlockIncident.CommandName)) cmdUnlockIncident.Visible = false;

                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToString().ToUpper().Equals("OFFSHORE"))
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    LinkButton cmdClose = (LinkButton)e.Item.FindControl("cmdApprove");
                    if (drv["FLDSTATUSOFINCIDENT"].ToString().Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 168, "S3"))
                        || drv["FLDSTATUSOFINCIDENT"].ToString().Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 168, "S4")))
                    {
                        //cmdClose.ImageUrl = Session["images"] + "/approve.png";
                        cmdClose.Visible = SessionUtil.CanAccess(this.ViewState, cmdClose.CommandName);
                        if (cmdClose.Visible == true)
                        {
                            cmdClose.Visible = true;
                        }
                    }
                    else
                    {
                        //cmdClose.ImageUrl = Session["images"] + "/spacer.gif";
                        cmdClose.Visible = false;
                    }

                    cmdClose.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentCloseComment.aspx?REFERENCEID=" + lblInspectionIncidentId.Text + "&type=" + drv["FLDAPPROVALTYPE"].ToString() + "&user=" + drv["FLDQUALITYGENERALMANAGER"].ToString() + "," + drv["FLDQUALITYDIRECTOR"].ToString() + "," + drv["FLDQAMANAGER"].ToString() + "," + drv["FLDQUALITYINCHARGE"].ToString() + "','large'); return true;");
                }
                else
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    LinkButton cmdClose = (LinkButton)e.Item.FindControl("cmdApprove");
                    if (drv["FLDSTATUSOFINCIDENT"].ToString().Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 168, "S3"))
                      || drv["FLDSTATUSOFINCIDENT"].ToString().Equals(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 168, "S4")))
                    {
                        //cmdClose.ImageUrl = Session["images"] + "/approve.png";
                        cmdClose.Visible = SessionUtil.CanAccess(this.ViewState, cmdClose.CommandName);
                        if (cmdClose.Visible == true)
                        {
                            cmdClose.Visible = true;
                        }
                    }
                    else
                    {
                        //cmdClose.ImageUrl = Session["images"] + "/spacer.gif";
                        cmdClose.Visible = false;
                    }
                    cmdClose.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentCloseComment.aspx?REFERENCEID=" + lblInspectionIncidentId.Text + "&type=" + drv["FLDAPPROVALTYPE"].ToString() + "&user=" + drv["FLDQUALITYGENERALMANAGER"].ToString() + "," + drv["FLDQUALITYDIRECTOR"].ToString() + "," + drv["FLDQAMANAGER"].ToString() + "," + drv["FLDQUALITYINCHARGE"].ToString() + "','large'); return true;");
                }

                LinkButton cmdCar = (LinkButton)e.Item.FindControl("cmdCorrectiveActions");
                if (cmdCar != null)
                {
                    cmdCar.Visible = SessionUtil.CanAccess(this.ViewState, cmdCar.CommandName);
                    HtmlGenericControl html = new HtmlGenericControl();

                    if (drv["FLDCARPARRECORDEDYN"].ToString() == "0")
                    {
                        cmdCar.Controls.Remove(html);
                        html.InnerHtml = "<span class=\"icon\"><i class=\"fa-eye-na\"></i></span>";
                        cmdCar.Controls.Add(html);
                    }
                }

                LinkButton cmdReport = (LinkButton)e.Item.FindControl("cmdReport");

                if (cmdReport != null)
                {
                    cmdReport.Attributes.Add("onclick", "openNewWindow('Report', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=INCIDENTNEARMISSREPORT&inspectionincidentid=" + lblInspectionIncidentId.Text + "&vesselid=" + drv["FLDVESSELID"].ToString() + "&showmenu=0&showexcel=NO');return true;");
                }

                if (cmdReport != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdReport.CommandName)) cmdReport.Visible = false;
                }

                LinkButton cmdThirdPartyReport = (LinkButton)e.Item.FindControl("cmdThirdPartyReport");
                if (cmdThirdPartyReport != null)
                {
                    cmdThirdPartyReport.Attributes.Add("onclick", "openNewWindow('Report', '', '" + Session["sitepath"] + "/Inspection/InspectionIncidentReport.aspx?INCIDENTID=" + lblInspectionIncidentId.Text + "&VESSELID=" + drv["FLDVESSELID"].ToString() + "');return true;");
                }

                if (cmdThirdPartyReport != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, cmdThirdPartyReport.CommandName)) cmdThirdPartyReport.Visible = false;
                }

                RadLabel lblTitle = (RadLabel)e.Item.FindControl("lblTitle");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucIncidentTitle");
                if (uct != null)
                {
                    uct.Position = ToolTipPosition.TopCenter;
                    uct.TargetControlId = lblTitle.ClientID;
                }

                LinkButton cmdDirectorComment = (LinkButton)e.Item.FindControl("imgDirectorComments");
                if (lblInspectionIncidentId != null) cmdDirectorComment.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentDirectorComment.aspx?REFERENCEID=" + lblInspectionIncidentId.Text + "','large'); return true;");
                if (drv["FLDSTATUSOFINCIDENT"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 168, "S2") && drv["FLDINCIDENTLOCKFORVESSELYN"].ToString() == "1")
                {
                    if (cmdUnlockIncident != null)
                    {
                        cmdUnlockIncident.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to Unlock this Investigation in Vessel?'); return false;");
                        cmdUnlockIncident.Visible = true;
                    }
                }

                LinkButton lnkRefno = (LinkButton)e.Item.FindControl("lnkIncidentRefNo");
                if (lnkRefno != null)
                {


                    lnkRefno.Visible = SessionUtil.CanAccess(this.ViewState, lnkRefno.CommandName);

                    lnkRefno.Attributes.Add("onclick", "javascript:openNewWindow('code1', '', '" + Session["sitepath"] + "/Inspection/InspectionIncidentList.aspx?DashboardYN=1&IncidentId=" + lblInspectionIncidentId.Text + "');");

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
        criteria.Add("ddlIncidentNearmiss", ViewState["ddlType"].ToString());
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

        InspectionFilter.CurrentIncidentnearmissDashboardFilter = criteria;
    }

    private void GetFilter()
    {
        NameValueCollection nvc = InspectionFilter.CurrentIncidentnearmissDashboardFilter;

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

    protected void ddlCategory_DataBinding(object sender, EventArgs e)
    {
        DataSet dt = new DataSet();
        dt = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(1);
        RadComboBox ddlCategory = sender as RadComboBox;
        ddlCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ddlCategory.DataSource = dt;
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDCATEGORY").CurrentFilterValue = e.Value;
        ViewState["ddlCategory"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
    protected void ddlSubCategory_DataBinding(object sender, EventArgs e)
    {
        DataSet dt = new DataSet();
        dt = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(ViewState["ddlCategory"].ToString()));
        RadComboBox ddlSubCategory = sender as RadComboBox;
        ddlSubCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ddlSubCategory.DataSource = dt;
    }
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
        dst = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,168, 0, null);
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
        filterItem.OwnerTableView.GetColumn("FLDINCIDENTCATEGORY").CurrentFilterValue = e.Value;
        ViewState["ddlCon"] = e.Value;

        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
}
