using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class InspectionMachineryDamageList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Inspection/InspectionMachineryDamageList.aspx?", "New Machinery Damage", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMachineryDamageList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMachineryDamage')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
           // toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionUnsafeActsConditionsFilter.aspx')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMachineryDamageFilter.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMachineryDamageList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuDamageList.AccessRights = this.ViewState;
            MenuDamageList.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Search", "SEARCH", ToolBarDirection.Right);

            MenuDamageListMain.AccessRights = this.ViewState;
            // MenuDamageListMain.MenuList = toolbar.Show();
            MenuDamageListMain.Visible = false;

            if (!IsPostBack)
            {
                ViewState["COMPANYID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["MACHINERYDAMAGEID"] = "";

                gvMachineryDamage.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvc.Get("QMS");

                DateTime now = DateTime.Now;
                string FromDate = now.Date.AddMonths(-6).ToShortDateString();
                string ToDate = DateTime.Now.ToShortDateString();

                ViewState["FROMDATE"] = FromDate.ToString();
                ViewState["TODATE"] = ToDate.ToString();

                if (Request.QueryString["MACHINERYDAMAGEID"] != null && Request.QueryString["MACHINERYDAMAGEID"].ToString() != "")
                    ViewState["MACHINERYDAMAGEID"] = Request.QueryString["MACHINERYDAMAGEID"].ToString();
                else
                    ViewState["MACHINERYDAMAGEID"] = "";

                if (Request.QueryString["PageNo"] != null && Request.QueryString["PageNo"] != "")
                {
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["PageNo"]);
                }
                else
                {
                    ViewState["PAGENUMBER"] = null;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

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

        NameValueCollection nvc = Filter.CurrentMachineryDamageFilter;
        int? vesselid = null;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentMachineryDamageFilter == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }


        ds = PhoenixInspectionMachineryDamage.MachineryDamageSearch(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(null)
                                                                , (nvc != null && nvc.Get("txtRefno") != null) ? General.GetNullableString(nvc.Get("txtRefno")) : null
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                , vesselid
                                                                , (nvc != null && nvc.Get("txtComponentId") != null) ? General.GetNullableGuid(nvc.Get("txtComponentId")) : null
                                                                , (nvc != null && nvc.Get("ucIncidentFromDate") != null) ? General.GetNullableDateTime(nvc.Get("ucIncidentFromDate")) : General.GetNullableDateTime(ViewState["FROMDATE"].ToString())
                                                                , (nvc != null && nvc.Get("ucIncidentToDate") != null) ? General.GetNullableDateTime(nvc.Get("ucIncidentToDate")) : General.GetNullableDateTime(ViewState["TODATE"].ToString())
                                                                , (nvc != null && nvc.Get("ucReportedFromDate") != null) ? General.GetNullableDateTime(nvc.Get("ucReportedFromDate")) : null
                                                                , (nvc != null && nvc.Get("ucReportedToDate") != null) ? General.GetNullableDateTime(nvc.Get("ucReportedToDate")) : null
                                                                , (nvc != null && nvc.Get("ucCompletedFromDate") != null) ? General.GetNullableDateTime(nvc.Get("ucCompletedFromDate")) : null
                                                                , (nvc != null && nvc.Get("ucCompletedToDate") != null) ? General.GetNullableDateTime(nvc.Get("ucCompletedToDate")) : null
                                                                , (nvc != null && nvc.Get("ucClosedFromDate") != null) ? General.GetNullableDateTime(nvc.Get("ucClosedFromDate")) : null
                                                                , (nvc != null && nvc.Get("ucClosedToDate") != null) ? General.GetNullableDateTime(nvc.Get("ucClosedToDate")) : null
                                                                , (nvc != null && nvc.Get("txtTitle") != null) ? General.GetNullableString(nvc.Get("txtTitle")) : null
                                                                , (nvc != null && nvc.Get("ddlConsequenceCategory") != null) ? General.GetNullableInteger(nvc.Get("ddlConsequenceCategory")) : null
                                                                , null
                                                                , (nvc != null && nvc.Get("ddlProcessSubHazardId") != null) ? General.GetNullableGuid(nvc.Get("ddlProcessSubHazardId")) : null
                                                                , (nvc != null && nvc.Get("ddlPropertySubHazardId") != null) ? General.GetNullableGuid(nvc.Get("ddlPropertySubHazardId")) : null
                                                                , (nvc != null && nvc.Get("txtReportedBy") != null) ? General.GetNullableInteger(nvc.Get("txtReportedBy")) : null
                                                                , (nvc != null && nvc.Get("txtReportedByName") != null) ? General.GetNullableString(nvc.Get("txtReportedByName")) : null
                                                                , (nvc != null && nvc.Get("ddlStatus") != null) ? General.GetNullableInteger(nvc.Get("ddlStatus")) : null
                                                                , null
                                                                , null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , 1
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , (nvc != null && nvc.Get("ddlCategory") != null) ? General.GetNullableGuid(nvc.Get("ddlCategory")) : null
                                                                , (nvc != null && nvc.Get("ddlSubCategory") != null) ? General.GetNullableGuid(nvc.Get("ddlSubCategory")) : null
                                                                , (nvc != null && nvc.Get("ucTechFleet") != null) ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
                                                                , (nvc != null && nvc.Get("ucVesselType") != null) ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
                                                                , (nvc != null && nvc.Get("ucAddressType") != null) ? General.GetNullableInteger(nvc.Get("ucAddressType")) : null
                                                                , (nvc != null && nvc.Get("chkCritical") != null) ? General.GetNullableInteger(nvc.Get("chkCritical")) : null
                                                                , (nvc != null && nvc.Get("rblnearmisscategory") != null) ? General.GetNullableGuid(nvc.Get("rblnearmisscategory")) : null);

        General.ShowExcel("Machinery Damage / Failure List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void MenuDamageList_TabStripCommand(object sender, EventArgs e)
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
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentMachineryDamageFilter = null;
                ViewState["MACHINERYDAMAGEID"] = "";
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("ADD"))
            {
                Response.Redirect("../Inspection/InspectionMachineryDamageGeneral.aspx?MACHINERYDAMAGEID=" + "&PageNo=" + ViewState["PAGENUMBER"], true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDamageListMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Response.Redirect("../Inspection/InspectionMachineryDamageFilter.aspx");
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

        string[] alColumns = { "FLDVESSELNAME", "FLDREFERENCENUMBER", "FLDINCIDENTDATE", "FLDREPORTEDDATE", "FLDTITLE", "FLDCOMPONENTNAME", "FLDCONSEQUENCECATEGORYNAME", "FLDCARGONEARMISSYN", "FLDNAVIGATIONNEARMISSYN", "FLDINCIDENTREFNO", "FLDCLOSEDDATE", "FLDFLDSTATUSNAME" };
        string[] alCaptions = { "Vessel", "Reference No.", "Incident [LT]", "Reported", "Title", "Component", "Consequence Category", "Cargo Near Miss", "Navigation Near Miss", "Incident/Near Miss Ref.No", "Closed", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentMachineryDamageFilter;
        int? vesselid = null;

        if (nvc != null)
            vesselid = (General.GetNullableInteger(nvc.Get("ucVessel").ToString()) == null) ? null : General.GetNullableInteger(nvc.Get("ucVessel").ToString());

        if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
            vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

        if (Filter.CurrentMachineryDamageFilter == null)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
        }

        ds = PhoenixInspectionMachineryDamage.MachineryDamageSearch(
                                                                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , General.GetNullableGuid(null)
                                                                , (nvc != null && nvc.Get("txtRefno") != null) ? General.GetNullableString(nvc.Get("txtRefno")) : null
                                                                , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
                                                                , vesselid
                                                                , (nvc != null && nvc.Get("txtComponentId") != null) ? General.GetNullableGuid(nvc.Get("txtComponentId")) : null
                                                                , (nvc != null ) ? General.GetNullableDateTime(nvc.Get("ucIncidentFromDate")) : General.GetNullableDateTime(ViewState["FROMDATE"].ToString())
                                                                , (nvc != null ) ? General.GetNullableDateTime(nvc.Get("ucIncidentToDate")) : General.GetNullableDateTime(ViewState["TODATE"].ToString())
                                                                , (nvc != null && nvc.Get("ucReportedFromDate") != null) ? General.GetNullableDateTime(nvc.Get("ucReportedFromDate")) : null
                                                                , (nvc != null && nvc.Get("ucReportedToDate") != null) ? General.GetNullableDateTime(nvc.Get("ucReportedToDate")) : null
                                                                , (nvc != null && nvc.Get("ucCompletedFromDate") != null) ? General.GetNullableDateTime(nvc.Get("ucCompletedFromDate")) : null
                                                                , (nvc != null && nvc.Get("ucCompletedToDate") != null) ? General.GetNullableDateTime(nvc.Get("ucCompletedToDate")) : null
                                                                , (nvc != null && nvc.Get("ucClosedFromDate") != null) ? General.GetNullableDateTime(nvc.Get("ucClosedFromDate")) : null
                                                                , (nvc != null && nvc.Get("ucClosedToDate") != null) ? General.GetNullableDateTime(nvc.Get("ucClosedToDate")) : null
                                                                , (nvc != null && nvc.Get("txtTitle") != null) ? General.GetNullableString(nvc.Get("txtTitle")) : null
                                                                , (nvc != null && nvc.Get("ddlConsequenceCategory") != null) ? General.GetNullableInteger(nvc.Get("ddlConsequenceCategory")) : null
                                                                , null
                                                                , (nvc != null && nvc.Get("ddlProcessSubHazardId") != null) ? General.GetNullableGuid(nvc.Get("ddlProcessSubHazardId")) : null
                                                                , (nvc != null && nvc.Get("ddlPropertySubHazardId") != null) ? General.GetNullableGuid(nvc.Get("ddlPropertySubHazardId")) : null
                                                                , (nvc != null && nvc.Get("txtReportedBy") != null) ? General.GetNullableInteger(nvc.Get("txtReportedBy")) : null
                                                                , (nvc != null && nvc.Get("txtReportedByName") != null) ? General.GetNullableString(nvc.Get("txtReportedByName")) : null
                                                                , (nvc != null && nvc.Get("ddlStatus") != null) ? General.GetNullableInteger(nvc.Get("ddlStatus")) : null
                                                                , null
                                                                , null
                                                                , null
                                                                , sortexpression
                                                                , sortdirection
                                                                , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvMachineryDamage.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , (nvc != null && nvc.Get("ddlCategory") != null) ? General.GetNullableGuid(nvc.Get("ddlCategory")) : null
                                                                , (nvc != null && nvc.Get("ddlSubCategory") != null) ? General.GetNullableGuid(nvc.Get("ddlSubCategory")) : null
                                                                , (nvc != null && nvc.Get("ucTechFleet") != null) ? General.GetNullableInteger(nvc.Get("ucTechFleet")) : null
                                                                , (nvc != null && nvc.Get("ucVesselType") != null) ? General.GetNullableInteger(nvc.Get("ucVesselType")) : null
                                                                , (nvc != null && nvc.Get("ucAddressType") != null) ? General.GetNullableInteger(nvc.Get("ucAddressType")) : null
                                                                , (nvc != null && nvc.Get("chkCritical") != null) ? General.GetNullableInteger(nvc.Get("chkCritical")) : null
                                                                , (nvc != null && nvc.Get("rblnearmisscategory") != null) ? General.GetNullableGuid(nvc.Get("rblnearmisscategory")) : null);

        General.SetPrintOptions("gvMachineryDamage", "Machinery Damage / Failure List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (General.GetNullableGuid(ViewState["MACHINERYDAMAGEID"].ToString()) == null)
            {
                ViewState["MACHINERYDAMAGEID"] = ds.Tables[0].Rows[0]["FLDMACHINERYDAMAGEID"].ToString();
                gvMachineryDamage.SelectedIndexes.Clear();
            }
        }

        gvMachineryDamage.DataSource = ds;
        gvMachineryDamage.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvMachineryDamage_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                e.Item.Selected = true;
                GetMachineryDamageId(e.Item.ItemIndex);
                Response.Redirect("../Inspection/InspectionMachineryDamageGeneral.aspx?MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&PageNo=" + ViewState["PAGENUMBER"], true);
            }
            else if (e.CommandName.ToUpper().Equals("NAVIGATE"))
            {
                e.Item.Selected = true;
                GetMachineryDamageId(e.Item.ItemIndex);
                Response.Redirect("../Inspection/InspectionMachineryDamageGeneral.aspx?MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&PageNo=" + ViewState["PAGENUMBER"], true);
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                e.Item.Selected = true;
                GetMachineryDamageId(e.Item.ItemIndex);
                DeleteMachineryDamage(ViewState["MACHINERYDAMAGEID"].ToString());
                ucStatus.Text = "Machinery Damage has been deleted.";

                ViewState["MACHINERYDAMAGEID"] = "";
                Rebind();
            }
            else if(e.CommandName.ToUpper().Equals("MACHINERY"))
            {
                gvMachineryDamage.SelectedIndexes.Add(e.Item.ItemIndex);
                BindPageURL(e.Item.ItemIndex);
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
    protected void gvMachineryDamage_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dv = (DataRowView)e.Item.DataItem;

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                if (dv != null && General.GetNullableInteger(dv["FLDSHOWDELETEBUTTONYN"].ToString()) == 1)
                    db.Visible = true;
                else
                    db.Visible = false;
            }

            RadLabel lblMachineryDamageId = (RadLabel)e.Item.FindControl("lblMachineryDamageId");

            LinkButton cmdMachinery = (LinkButton)e.Item.FindControl("cmdMachinery");
            if (cmdMachinery != null && lblMachineryDamageId != null)
            {
                cmdMachinery.Attributes.Add("onclick", "openNewWindow('Report', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=MACHINERYDAMAGE&machinerydamageId=" + lblMachineryDamageId.Text + "&showmenu=0&showexcel=NO');return false;");
            }
        }
    }
    private void GetMachineryDamageId(int rowindex)
    {
        try
        {
            ViewState["MACHINERYDAMAGEID"] = ((RadLabel)gvMachineryDamage.Items[rowindex].FindControl("lblMachineryDamageId")).Text;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void DeleteMachineryDamage(string machinerydamageid)
    {
        if (General.GetNullableGuid(machinerydamageid) != null)
            PhoenixInspectionMachineryDamage.MachineryDamageDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(machinerydamageid));
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
    protected void gvMachineryDamage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMachineryDamage.CurrentPageIndex + 1;
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
        gvMachineryDamage.SelectedIndexes.Clear();
        gvMachineryDamage.EditIndexes.Clear();
        gvMachineryDamage.DataSource = null;
        gvMachineryDamage.Rebind();
    }
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }
    private void SetRowSelection()
    {
        gvMachineryDamage.SelectedIndexes.Clear();
        foreach (GridDataItem item in gvMachineryDamage.Items)
        {
            if (item.GetDataKeyValue("FLDMACHINERYDAMAGEID").ToString() == ViewState["MACHINERYDAMAGEID"].ToString())
            {
                gvMachineryDamage.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }
    private void BindPageURL(int rowindex)
    {
        try
        {
            GridDataItem item = (GridDataItem)gvMachineryDamage.Items[rowindex];
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

