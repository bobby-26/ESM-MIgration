using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using System.Collections;
using Telerik.Web.UI;

public partial class InspectionUnsafeActsConditionsList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Inspection/InspectionUnsafeActsConditionsAdd.aspx')", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionUnsafeActsConditionsList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDirectIncident')", "Print Grid", "<i class=\"fas fa-print\"></i>","PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionUnsafeActsConditionsFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionUnsafeActsConditionsList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Inspection/InspectionBulkUnSafeActContionsClose.aspx')", "Bulk Closure", "<i class=\"fas fa-tasks\"></i>", "BULKOFFICECOMMENTS");
            }
            MenuRegistersInspection.AccessRights = this.ViewState;
            MenuRegistersInspection.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
            MenuRegistersInspectionGeneral.AccessRights = this.ViewState;
            MenuRegistersInspectionGeneral.Visible = false;
            

            if (!IsPostBack)
            {
                VesselConfiguration();

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

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvDirectIncident.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        int? vesselid = -1;

        string[] alColumns = { "FLDVESSELNAME", "FLDINCIDENTNEARMISSREFNO", "FLDREPORTEDDATE", "FLDINCIDENTDATE", "FLDINCIDENTTIME", "FLDICCATEGORYNAME", "FLDICSUBCATEGORYNAME", "FLDSUMMARY", "FLDLOCATION", "FLDSTATUSNAME", "FLDCORRECTIVEACTION", "FLDROOTCAUSENAMELIST" };
        string[] alCaptions = { "Vessel", "Reference No.", "Reported", "Incident", "Time", "Category", "Sub-category", "Details", "Location", "Status", "Action Taken", "Root Cause" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentUnsafeActsConditionsFilter;
        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        DataSet ds = PhoenixInspectionUnsafeActsConditions.DirectIncidentNearMissSearch(
                                                                    General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : null)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : null)
                                                                    , vesselid
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                    , gvDirectIncident.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ucCategory"] : null)
                                                                    , General.GetNullableGuid(nvc != null ? nvc["ddlSubcategory"] : null)
                                                                    , General.GetNullableInteger(nvc != null ? (nvc["ddlStatus"] == "0" ? null : nvc["ddlStatus"].ToString()) : null)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["ucIncidentNearMissFromDate"] : ViewState["FROMDATE"].ToString())
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["ucIncidentNearMissToDate"] : ViewState["TODATE"].ToString())
                                                                    , General.GetNullableString(nvc != null ? nvc["txtReferenceNumber"] : null)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ucStatusofUA"] : ViewState["Status"].ToString())
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ucTechFleet"] : null)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ucAddrOwner"] : null)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ucVesselType"] : null)
                                                                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                    , General.GetNullableByte(nvc != null ? nvc["chkIncidentNearMissRaisedYN"] : null)
                                                                    );

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
        int? vesselid = -1;

        string[] alColumns = { "FLDVESSELNAME", "FLDINCIDENTNEARMISSREFNO", "FLDREPORTEDDATE", "FLDINCIDENTDATE", "FLDINCIDENTTIME", "FLDICCATEGORYNAME", "FLDICSUBCATEGORYNAME", "FLDSUMMARY", "FLDLOCATION", "FLDSTATUSNAME", "FLDCORRECTIVEACTION", "FLDROOTCAUSENAMELIST" };
        string[] alCaptions = { "Vessel", "Reference No.", "Reported", "Incident", "Time", "Category", "Sub-category", "Details", "Location", "Status", "Action Taken", "Root Cause" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentUnsafeActsConditionsFilter;
        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        DataSet ds = PhoenixInspectionUnsafeActsConditions.DirectIncidentNearMissSearch(
                                                                    General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : null)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : null)
                                                                    , vesselid
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , 1
                                                                    , iRowCount
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ucCategory"] : null)
                                                                    , General.GetNullableGuid(nvc != null ? nvc["ddlSubcategory"] : null)
                                                                    , General.GetNullableInteger(nvc != null ? (nvc["ddlStatus"] == "0" ? null : nvc["ddlStatus"].ToString()) : null)
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["ucIncidentNearMissFromDate"] : ViewState["FROMDATE"].ToString())
                                                                    , General.GetNullableDateTime(nvc != null ? nvc["ucIncidentNearMissToDate"] : ViewState["TODATE"].ToString())
                                                                    , General.GetNullableString(nvc != null ? nvc["txtReferenceNumber"] : null)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ucStatusofUA"] : ViewState["Status"].ToString())
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ucTechFleet"] : null)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ucAddrOwner"] : null)
                                                                    , General.GetNullableInteger(nvc != null ? nvc["ucVesselType"] : null)
                                                                    , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                    , General.GetNullableByte(nvc != null ? nvc["chkIncidentNearMissRaisedYN"] : null)
                                                                    );

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
    protected void MenuRegistersInspectionGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Response.Redirect("../Inspection/InspectionUnsafeActsConditionsFilter.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
                Filter.CurrentUnsafeActsConditionsFilter = null;
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
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                RadLabel lblDirectIncidentId = (RadLabel)e.Item.FindControl("lblDirectIncidentId");
                BindPageURL(e.Item.ItemIndex);
                if (lblDirectIncidentId != null)
                {
                    ViewState["directincidentid"] = lblDirectIncidentId.Text;
                    Response.Redirect("../Inspection/InspectionUnsafeActsConditions.aspx?directincidentid=" + ViewState["directincidentid"], false);
                }
            }
            else if (e.CommandName.ToUpper().Equals("CANCELACTION"))
            {
                BindPageURL(e.Item.ItemIndex);
                SetRowSelection();
            }
            else if (e.CommandName == "Page")
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
    private bool IsValidInspection(string inspectiontypeid, string inspectiontypecategoryid, string inspectionname, string shortcode, string effectivedate, string externalaudittype, string officeyn, string companyid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (inspectiontypeid.Trim().Equals("Dummy") || inspectiontypeid.Trim().Equals(""))
            ucError.ErrorMessage = "Type is required.";
        if (inspectiontypecategoryid.Trim().Equals("Dummy") || inspectiontypecategoryid.Trim().Equals(""))
            ucError.ErrorMessage = "Category is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
    protected void gvDirectIncident_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridHeaderItem)
        {
            RadCheckBox chkSelectall = (RadCheckBox)e.Item.FindControl("chkAllUnsafe");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                chkSelectall.Visible = false;
            }
        }
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
            if(lblActionTaken.Text != "")
            {
                //lblActionTaken.CssClass = "tooltip";
            }
            UserControlToolTip uc = (UserControlToolTip)e.Item.FindControl("ucToolTipActionTaken");
            if (uc != null)
            {
                uc.Position = ToolTipPosition.TopCenter;
                uc.TargetControlId = lblActionTaken.ClientID;
            }

            LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
            RadLabel lblDirectIncidentId = (RadLabel)e.Item.FindControl("lblDirectIncidentId");
            if (cmdCancel != null && lblDirectIncidentId != null)
                cmdCancel.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','', '" + Session["sitepath"] + "/Inspection/InspectionCancelReasonUpdate.aspx?REFERENCEID=" + lblDirectIncidentId.Text + "&TYPE=1','small'); return true;");

            if (cmdCancel != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName))
                    cmdCancel.Visible = false;
            }

            RadCheckBox chkSelect = (RadCheckBox)e.Item.FindControl("chkSelect");
            RadLabel lblcompletedyn = (RadLabel)e.Item.FindControl("lblcompletedyn");

            if (lblcompletedyn != null && (lblcompletedyn.Text == "1"))
            {
                if (chkSelect != null)
                    chkSelect.Visible = true;
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                chkSelect.Visible = false;
            }
            
            UserControlCommonToolTip ttip = (UserControlCommonToolTip)e.Item.FindControl("ucCommonToolTip");
            if (ttip != null)
            {
                ttip.Screen = "Inspection/InspectionToolTipUnsafeActsConditions.aspx?directincidentid=" + drv["FLDDIRECTINCIDENTID"].ToString();
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
    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvDirectIncident$ctl00$ctl02$ctl01$chkAllUnsafe")
        {
            GridHeaderItem headerItem = gvDirectIncident.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            RadCheckBox chkAll = (RadCheckBox)headerItem.FindControl("chkAllUnsafe");
            foreach (GridDataItem row in gvDirectIncident.MasterTableView.Items)
            {
                RadCheckBox cbSelected = (RadCheckBox)row.FindControl("chkSelect");
                if (cbSelected != null)
                {
                    if (chkAll.Checked == true)
                    {
                        cbSelected.Checked = true;
                    }
                    else
                    {
                        cbSelected.Checked = false;
                    }
                }
            }
        }
    }
    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedPvs = new ArrayList();
        Guid index = new Guid();
        foreach (GridDataItem gvrow in gvDirectIncident.Items)
        {
            bool result = false;
            index = new Guid(gvrow.GetDataKeyValue("FLDDIRECTINCIDENTID").ToString());

            if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;
            }

            // Check in the Session
            if (Filter.CurrentSelectedBulkDirectIncident != null)
            {
                SelectedPvs = (ArrayList)Filter.CurrentSelectedBulkDirectIncident;
            }
            if (result)
            {
                if (!SelectedPvs.Contains(index))
                    SelectedPvs.Add(index);
            }
            else
                SelectedPvs.Remove(index);
        }
        if (SelectedPvs != null && SelectedPvs.Count > 0)
        {
            Filter.CurrentSelectedBulkDirectIncident = SelectedPvs;
        }
    }

    private void GetSelectedPvs()
    {
        if (Filter.CurrentSelectedBulkDirectIncident != null)
        {
            ArrayList SelectedPvs = (ArrayList)Filter.CurrentSelectedBulkDirectIncident;
            Guid index = new Guid();
            if (SelectedPvs != null && SelectedPvs.Count > 0)
            {
                foreach (GridDataItem row in gvDirectIncident.Items)
                {
                    RadCheckBox chk = (RadCheckBox)(row.Cells[0].FindControl("chkSelect"));
                    index = new Guid(gvDirectIncident.MasterTableView.Items[0].GetDataKeyValue("FLDDIRECTINCIDENTID").ToString());
                    if (SelectedPvs.Contains(index))
                    {
                        RadCheckBox myCheckBox = (RadCheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }
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

    protected void chkSelectall_CheckedChanged(object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvDirectIncident$ctl01$chkAllUnsafe")
        {
            GridHeaderItem headerItem = gvDirectIncident.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            RadCheckBox chkAll = (RadCheckBox)headerItem.FindControl("chkAllUnsafe");
            foreach (GridDataItem row in gvDirectIncident.Items)
            {
                RadCheckBox cbSelected = (RadCheckBox)row.FindControl("chkSelect");
                if (cbSelected != null)
                {
                    if (chkAll.Checked == true)
                    {
                        cbSelected.Checked = true;
                    }
                    else
                    {
                        cbSelected.Checked = false;
                    }
                }
            }
        }
    }
}
