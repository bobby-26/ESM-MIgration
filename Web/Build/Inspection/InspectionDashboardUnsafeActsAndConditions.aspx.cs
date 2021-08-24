using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web.UI;
using SouthNests.Phoenix.Registers;

public partial class InspectionDashboardUnsafeActsAndConditions : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();           
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardUnsafeActsAndConditions.aspx?OfficeDashboard=1&STATUS=OPN", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
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

                ViewState["FleetList"] = "";
                ViewState["Owner"] = "";
                ViewState["VesselTypeList"] = "";
                ViewState["VesselList"] = "";
                ViewState["ucCategory"] = "";
                ViewState["ddlSubcategory"] = "";
                ViewState["txtReferenceNumber"] = "";
                ViewState["chkIncidentNearMissRaisedYN"] = "";
                ViewState["ucFrom"] = "";
                ViewState["ucTo"] = "";
                if(InspectionFilter.CurrentUnsafeActsConditionsDashboardFilter!=null)
                {
                    GetFilter();
                }

            }

            if (ViewState["OfficeDashboard"].ToString() == "1")
            {
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionDashboardUnsafeActsAndConditionsFilter.aspx?base=1')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
                toolbar.AddFontAwesomeButton("../Inspection/InspectionDashboardUnsafeActsAndConditions.aspx?OfficeDashboard=1&STATUS=OPN", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            }

            MenuRegistersInspection.AccessRights = this.ViewState;
            MenuRegistersInspection.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Accidents", "ACCIDENTS");
            toolbarmain.AddButton("Near Miss", "NEARMISS");
            toolbarmain.AddButton("Machinery Damage", "MACHINERYDAMAGE");
            toolbarmain.AddButton("UC/ UACT", "UCUACT");
            MenuARSubTab.AccessRights = this.ViewState;
            MenuARSubTab.MenuList = toolbarmain.Show();
            MenuARSubTab.SelectedMenuIndex = 3;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
        if (CommandName.ToUpper().Equals("MACHINERYDAMAGE"))
        {
            Response.Redirect("../Inspection/InspectionDashBoardMachineryDamageDBListExtn.aspx?STATUS=OPN");
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

                ds = PhoenixInspectionOfficeDashboard.DashboardOfficeunsafeactsandconditionSearch(General.GetNullableString(ViewState["VESSELID"].ToString()== string.Empty? (Dashboardnvc != null ? Dashboardnvc["VesselList"]:null) : ViewState["VESSELID"].ToString())
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                                                                    , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
                                                                    , General.GetNullableString(ViewState["Status"].ToString())
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

            ds = PhoenixInspectionOfficeDashboard.DashboardOfficeunsafeactsandconditionSearch(General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null) : ViewState["VESSELID"].ToString())
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                                                                    , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                                                                    , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
                                                                    , General.GetNullableString(ViewState["Status"].ToString())
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

                ViewState["FleetList"] = "";
                ViewState["Owner"] = "";
                ViewState["VesselTypeList"] = "";
                ViewState["VesselList"] = "";
                ViewState["ucCategory"] = "";
                ViewState["ddlSubcategory"] = "";
                ViewState["txtReferenceNumber"] = "";
                ViewState["chkIncidentNearMissRaisedYN"] = "";
                ViewState["ucFrom"] = "";
                ViewState["ucTo"] = "";
                ViewState["Status"] = PhoenixCommonRegisters.GetHardCode(1, 146, "OPN");
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
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                Pair filterPair = (Pair)e.CommandArgument;
                ViewState["PAGENUMBER"] = 1;

                string daterange = gvDirectIncident.MasterTableView.GetColumn("FLDREPORTEDDATE").CurrentFilterValue.ToString();
                if (daterange != string.Empty)
                {
                    ViewState["ucFrom"] = daterange.Split('~')[0];
                    ViewState["ucTo"] = daterange.Split('~')[1];
                }
                //string newdaterange = gvDirectIncident.MasterTableView.GetColumn("FLDINCIDENTDATE").CurrentFilterValue.ToString();
                //if (newdaterange != string.Empty)
                //{
                //    ViewState["FDATE"] = newdaterange.Split('~')[0];
                //    ViewState["TDATE"] = newdaterange.Split('~')[1];
                //}
                //string closedaterange = gvDirectIncident.MasterTableView.GetColumn("FLDCLOSEDDATE").CurrentFilterValue.ToString();
                //if (closedaterange != string.Empty)
                //{
                //    ViewState["ucClosedFrom"] = newdaterange.Split('~')[0];
                //    ViewState["ucClosedTo"] = newdaterange.Split('~')[1];
                //}
                ViewState["txtReferenceNumber"] = gvDirectIncident.MasterTableView.GetColumn("FLDINCIDENTNEARMISSREFNO").CurrentFilterValue;
               // ViewState["txtTitle"] = gvDirectIncident.MasterTableView.GetColumn("FLDTITLE").CurrentFilterValue;

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
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (InspectionFilter.CurrentUnsafeActsConditionsDashboardFilter != null)
        {
            GetFilter();
        }
        SetFilter();
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
    private void SetFilter()
    {
        NameValueCollection criteria = new NameValueCollection();

        criteria.Add("FleetList", ViewState["FleetList"].ToString());
        criteria.Add("Owner", ViewState["Owner"].ToString());
        criteria.Add("VesselTypeList", ViewState["VesselTypeList"].ToString());
        criteria.Add("VesselList", ViewState["VesselList"].ToString());
        criteria.Add("ucCategory", ViewState["ucCategory"].ToString());
        criteria.Add("ddlSubcategory", ViewState["ddlSubcategory"].ToString());
        criteria.Add("txtReferenceNumber", ViewState["txtReferenceNumber"].ToString());
        criteria.Add("chkIncidentNearMissRaisedYN", ViewState["chkIncidentNearMissRaisedYN"].ToString());
        criteria.Add("ucFrom", ViewState["ucFrom"].ToString());
        criteria.Add("ucTo", ViewState["ucTo"].ToString());
        criteria.Add("Status", ViewState["Status"].ToString());

        InspectionFilter.CurrentUnsafeActsConditionsDashboardFilter = criteria;
    }
    private void GetFilter()
    {
        NameValueCollection nvc = InspectionFilter.CurrentUnsafeActsConditionsDashboardFilter;

        ViewState["FleetList"] = General.GetNullableString(nvc.Get("FleetList")) == null ? string.Empty : General.GetNullableString(nvc.Get("FleetList"));
        ViewState["Owner"] = General.GetNullableString(nvc.Get("Owner")) == null ? string.Empty : General.GetNullableString(nvc.Get("Owner"));
        ViewState["VesselTypeList"] = General.GetNullableString(nvc.Get("VesselTypeList")) == null ? string.Empty : General.GetNullableString(nvc.Get("VesselTypeList"));
        ViewState["VesselList"] = General.GetNullableString(nvc.Get("VesselList")) == null ? string.Empty : General.GetNullableString(nvc.Get("VesselList"));
        ViewState["ucCategory"] = General.GetNullableString(nvc.Get("ucCategory")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucCategory"));
        ViewState["ddlSubcategory"] = General.GetNullableString(nvc.Get("ddlSubcategory")) == null ? string.Empty : General.GetNullableString(nvc.Get("ddlSubcategory"));
        ViewState["txtReferenceNumber"] = General.GetNullableString(nvc.Get("txtReferenceNumber")) == null ? string.Empty : General.GetNullableString(nvc.Get("txtReferenceNumber"));
        ViewState["chkIncidentNearMissRaisedYN"] = General.GetNullableString(nvc.Get("chkIncidentNearMissRaisedYN")) == null ? string.Empty : General.GetNullableString(nvc.Get("chkIncidentNearMissRaisedYN"));
        ViewState["ucFrom"] = General.GetNullableString(nvc.Get("ucFrom")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucFrom"));
        ViewState["ucTo"] = General.GetNullableString(nvc.Get("ucTo")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucTo"));
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

    protected void ddlCategory_DataBinding(object sender, EventArgs e)
    {
        DataSet dst = new DataSet();
        dst = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 208, 0, null);
        RadComboBox ddlCategory = sender as RadComboBox;
        ddlCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ddlCategory.DataSource = dst;
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDICCATEGORYNAME").CurrentFilterValue = e.Value;
        ViewState["ucCategory"] = e.Value;
        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void ddlSubcategory_DataBinding(object sender, EventArgs e)
    {
        DataTable dt = PhoenixInspectionUnsafeActsConditions.OpenReportSubcategoryList(General.GetNullableInteger(ViewState["ucCategory"].ToString()));
        RadComboBox ddlSubcategory = sender as RadComboBox;
        ddlSubcategory.Items.Insert(0, new RadComboBoxItem("All", String.Empty));
        ddlSubcategory.DataSource = dt;
        
    }

    protected void ddlSubcategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDICSUBCATEGORYNAME").CurrentFilterValue = e.Value;
        ViewState["ddlSubcategory"] = e.Value;
        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }

    protected void ddlStatus_DataBinding(object sender, EventArgs e)
    {
        DataSet dst = new DataSet();
        //dst = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 146, 0, null);
        dst = PhoenixRegistersHard.ListHard(146, 1, "OPN,CMP,CLD,CAD", null);
        RadComboBox ddlStatus = sender as RadComboBox;
        ddlStatus.Items.Insert(0, new RadComboBoxItem("All", String.Empty));
        ddlStatus.DataSource = dst;
    }

    protected void ddlStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDSTATUSNAME").CurrentFilterValue = e.Value;
        ViewState["Status"] = e.Value;
        SetFilter();
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
}