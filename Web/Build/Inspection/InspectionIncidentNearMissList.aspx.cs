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

public partial class InspectionIncidentNearMissList : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionIncidentNearMissList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvInspectionIncidentSearch')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionIncidentNearMissList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionIncidentAdd.aspx')", "Add New", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MenuIncidentSearch.AccessRights = this.ViewState;
            MenuIncidentSearch.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            //toolbar.AddButton("Search", "SEARCH", ToolBarDirection.Right);

            if ((PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE")) && (PhoenixSecurityContext.CurrentSecurityContext.InstallCode.Equals(0)))
            {
                toolbar.AddButton("Office Incident", "OFFICELIST", ToolBarDirection.Right);
                toolbar.AddButton("Incident", "LIST", ToolBarDirection.Right);
            }

            if ((PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE")) && (PhoenixSecurityContext.CurrentSecurityContext.InstallCode.Equals(0)))
            {
                MenuIncidentGeneral.AccessRights = this.ViewState;
                MenuIncidentGeneral.MenuList = toolbar.Show();
                MenuIncidentGeneral.SelectedMenuIndex = 1;
            }
            else
                MenuIncidentGeneral.Visible = false;

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

                if (Filter.CurrentIncidentFilterCriteria != null)
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

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        //if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
        //    vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentIncidentFilterCriteria == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }

        ds = PhoenixInspectionIncident.InspectionIncidentSearch(
              nvc != null ? General.GetNullableString(nvc.Get("txtRefNo")) : string.Empty
            , nvc != null ? General.GetNullableString(nvc.Get("txtTitle")) : string.Empty
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : General.GetNullableInteger(ViewState["Status"].ToString())
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : General.GetNullableDateTime(ViewState["FDATE"].ToString())
            , nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : General.GetNullableDateTime(ViewState["TDATE"].ToString())
            , vesselid
            , sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucConsequenceCategory")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucPotentialCategory")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucActivity")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlIncidentNearmiss")) : null
            , nvc != null ? General.GetNullableGuid(nvc.Get("ucCategory")) : null
            , nvc != null ? General.GetNullableGuid(nvc.Get("ucSubCategory")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucAddressType")) : null
            , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
            , nvc != null ? General.GetNullableInteger(nvc.Get("chkOfficeAuditIncident")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("ucCompany")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReportedDateFrom")) : null
            , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReportedDateTo")) : null
            , nvc != null ? General.GetNullableInteger(nvc.Get("chkContractedRelatedIncidentYN")) : null
            );

        Response.AddHeader("Content-Disposition", "attachment; filename=Incident/NearMissList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Incident/Near Miss List</h3></td>");
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
                Filter.CurrentIncidentFilterCriteria = null;
                SetFilter();
                ViewState["PAGENUMBER"] = 1;
                foreach(GridColumn column in gvInspectionIncidentSearch.MasterTableView.Columns)
                {

                    column.ListOfFilterValues = null; // CheckList values set to null will uncheck all the checkboxes

                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;

                    column.AndCurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.AndCurrentFilterValue = string.Empty;
                }
                gvInspectionIncidentSearch.MasterTableView.FilterExpression = string.Empty;
                gvInspectionIncidentSearch.MasterTableView.Rebind();
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

            if (Filter.CurrentIncidentFilterCriteria == null)
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                }
            }

            ds = PhoenixInspectionIncident.InspectionIncidentSearch(
                  nvc != null ? General.GetNullableString(nvc.Get("txtRefNo")) : string.Empty
                , nvc != null ? General.GetNullableString(nvc.Get("txtTitle")) : string.Empty
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucStatus")) : General.GetNullableInteger(ViewState["Status"].ToString())
                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtFromDate")) : General.GetNullableDateTime(ViewState["FDATE"].ToString())
                , nvc != null ? General.GetNullableDateTime(nvc.Get("txtToDate")) : General.GetNullableDateTime(ViewState["TDATE"].ToString())
                , vesselid
                , sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                gvInspectionIncidentSearch.PageSize,
                ref iRowCount,
                ref iTotalPageCount
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucConsequenceCategory")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucPotentialCategory")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucActivity")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ddlIncidentNearmiss")) : null
                , nvc != null ? General.GetNullableGuid(nvc.Get("ucCategory")) : null
                , nvc != null ? General.GetNullableGuid(nvc.Get("ucSubCategory")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucAddressType")) : null
                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                , nvc != null ? General.GetNullableInteger(nvc.Get("chkOfficeAuditIncident")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("ucCompany")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReportedDateFrom")) : null
                , nvc != null ? General.GetNullableDateTime(nvc.Get("ucReportedDateTo")) : null
                , nvc != null ? General.GetNullableInteger(nvc.Get("chkContractedRelatedIncidentYN")) : null
                );

            General.SetPrintOptions("gvInspectionIncidentSearch", "Incident/Near Miss List", alCaptions, alColumns, ds);

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
                ViewState["txtRefNo"] = gvInspectionIncidentSearch.MasterTableView.GetColumn("FLDINCIDENTREFNO").CurrentFilterValue;
                ViewState["txtTitle"] = gvInspectionIncidentSearch.MasterTableView.GetColumn("FLDINCIDENTTITLE").CurrentFilterValue;

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
        int vesselsonly = 1;

        if(PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            vesselsonly = 0;
        }

        dt = PhoenixRegistersVessel.ListOwnerAssignedVessel(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(null), General.GetNullableInteger(ViewState["COMPANYID"].ToString()), General.GetNullableInteger(null), vesselsonly);
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
        
        criteria.Add("txtRefNo", ViewState["txtRefNo"].ToString());
        criteria.Add("txtTitle", ViewState["txtTitle"].ToString());
        criteria.Add("txtFromDate", ViewState["FDATE"].ToString());
        criteria.Add("txtToDate", ViewState["TDATE"].ToString());
        criteria.Add("ucStatus", ViewState["Status"].ToString());
        criteria.Add("ucVessel", ViewState["VesselID"].ToString());
        criteria.Add("ddlIncidentNearmiss", ViewState["ddlType"].ToString());
        criteria.Add("ucCategory", ViewState["ddlCategory"].ToString());
        criteria.Add("ucSubCategory", ViewState["ddlSubCategory"].ToString());
        criteria.Add("ucReportedDateFrom", ViewState["FRDATE"].ToString());
        criteria.Add("ucReportedDateTo", ViewState["TRDATE"].ToString());
        criteria.Add("ucConsequenceCategory", ViewState["ddlCon"].ToString());

        Filter.CurrentIncidentFilterCriteria = criteria;
    }

    private void GetFilter()
    {
        NameValueCollection nvc = Filter.CurrentIncidentFilterCriteria;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            ViewState["VesselID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }

        ViewState["txtRefNo"] = General.GetNullableString(nvc.Get("txtRefNo")) == null ? string.Empty : General.GetNullableString(nvc.Get("txtRefNo"));
        ViewState["txtTitle"] = General.GetNullableString(nvc.Get("txtTitle")) == null ? string.Empty : General.GetNullableString(nvc.Get("txtTitle"));
        ViewState["FDATE"] = General.GetNullableString(nvc.Get("txtFromDate")) == null ? string.Empty : General.GetNullableString(nvc.Get("txtFromDate"));
        ViewState["TDATE"] = General.GetNullableString(nvc.Get("txtToDate")) == null ? string.Empty : General.GetNullableString(nvc.Get("txtToDate"));
        ViewState["Status"] = General.GetNullableString(nvc.Get("ucStatus")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucStatus"));
        ViewState["VesselID"] = General.GetNullableString(nvc.Get("ucVessel")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucVessel"));
        ViewState["ddlType"] = General.GetNullableString(nvc.Get("ddlIncidentNearmiss")) == null ? string.Empty : General.GetNullableString(nvc.Get("ddlIncidentNearmiss"));
        ViewState["ddlCategory"] = General.GetNullableString(nvc.Get("ucCategory")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucCategory"));
        ViewState["ddlSubCategory"] = General.GetNullableString(nvc.Get("ucSubCategory")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucSubCategory"));
        ViewState["FRDATE"] = General.GetNullableString(nvc.Get("ucReportedDateFrom")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucReportedDateFrom"));
        ViewState["TRDATE"] = General.GetNullableString(nvc.Get("ucReportedDateTo")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucReportedDateTo"));
        ViewState["ddlCon"] = General.GetNullableString(nvc.Get("ucConsequenceCategory")) == null ? string.Empty : General.GetNullableString(nvc.Get("ucConsequenceCategory"));
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
        dt = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(General.GetNullableInteger(ViewState["ddlType"].ToString()));
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
