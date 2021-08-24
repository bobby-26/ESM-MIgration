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

public partial class Inspection_InspectionDashBoardMachineryDamageList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashBoardMachineryDamageList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMachineryDamage')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('Filter','', '" + Session["sitepath"] + "/Inspection/InspectionDashBoardMachineryDamageListFilter.aspx?')", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDashBoardMachineryDamageList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            MenuDamageList.AccessRights = this.ViewState;
            MenuDamageList.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Search", "SEARCH", ToolBarDirection.Right);

            MenuDamageListMain.AccessRights = this.ViewState;
            // MenuDamageListMain.MenuList = toolbar.Show();
            MenuDamageListMain.Visible = false;

            if (!IsPostBack)
            {

                InspectionFilter.CurrentMachineryDamageDashboardFilter = null;
                ViewState["COMPANYID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["MACHINERYDAMAGEID"] = "";
                ViewState["STATUS"] = "";
                ViewState["VESSELID"] = "";

                if (Request.QueryString["STATUS"] != null)
                    ViewState["STATUS"] = Request.QueryString["STATUS"].ToString();                

                gvMachineryDamage.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvc.Get("QMS");

                if (Request.QueryString["MACHINERYDAMAGEID"] != null && Request.QueryString["MACHINERYDAMAGEID"].ToString() != "")
                    ViewState["MACHINERYDAMAGEID"] = Request.QueryString["MACHINERYDAMAGEID"].ToString();
                else
                    ViewState["MACHINERYDAMAGEID"] = "";

                if (!string.IsNullOrEmpty(Request.QueryString["vslid"]))
                {
                    ViewState["VESSELID"] = Request.QueryString["vslid"];
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


        NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
        NameValueCollection Dashboardnvc = InspectionFilter.CurrentMachineryDamageDashboardFilter;

        ds = PhoenixInspectionOfficeDashboard.DashBoardMachineryDamageSearch(General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null) : ViewState["VESSELID"].ToString())
                                                                , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                                                                , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                                                                , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
                                                                , General.GetNullableString(ViewState["STATUS"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvMachineryDamage.PageSize
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

    protected void MenuDamageList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

           if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
           if(CommandName.ToUpper().Equals("CLEAR"))
            {
                InspectionFilter.CurrentMachineryDamageDashboardFilter = null;
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

        //if (Filter.CurrentMachineryDamageFilter == null)
        //{
        //    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
        //    {
        //        vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        //    }
        //}

        NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
        NameValueCollection Dashboardnvc = InspectionFilter.CurrentMachineryDamageDashboardFilter;

        ds = PhoenixInspectionOfficeDashboard.DashBoardMachineryDamageSearch(General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselList"] : null) : ViewState["VESSELID"].ToString())
                                                                , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["VesselTypeList"] : null) : null)
                                                                , General.GetNullableString(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["FleetList"] : null) : null)
                                                                , General.GetNullableInteger(ViewState["VESSELID"].ToString() == string.Empty ? (Dashboardnvc != null ? Dashboardnvc["Owner"] : null) : null)
                                                                , General.GetNullableString(ViewState["STATUS"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                , gvMachineryDamage.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , General.GetNullableGuid(Dashboardnvc !=null ? Dashboardnvc["ddlCategory"] : null ) 
                                                                , General.GetNullableGuid(Dashboardnvc !=null ? Dashboardnvc["ddlSubCategory"] : null )
                                                                , General.GetNullableGuid(Dashboardnvc !=null ? Dashboardnvc["ddlProcessSubHazardId"] : null )
                                                                , General.GetNullableGuid(Dashboardnvc !=null ? Dashboardnvc["ddlPropertySubHazardId"] : null )
                                                                , General.GetNullableString(Dashboardnvc !=null ? Dashboardnvc["txtRefno"] : null )
                                                                , General.GetNullableString(Dashboardnvc !=null ? Dashboardnvc["txtTitle"] : null )
                                                                , General.GetNullableInteger(Dashboardnvc !=null ? Dashboardnvc["ddlConsequenceCategory"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucIncidentFrom"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucIncidentTo"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucReportedFrom"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucReportedTo"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucClosedFrom"] : null)
                                                                , General.GetNullableDateTime(Dashboardnvc != null ? Dashboardnvc["ucClosedTo"] : null)
                                                                , PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

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
            if (e.CommandName.ToUpper().Equals("NAVIGATE"))
            {
                e.Item.Selected = true;
                GetMachineryDamageId(e.Item.ItemIndex);
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                e.Item.Selected = true;
                GetMachineryDamageId(e.Item.ItemIndex);
                DeleteMachineryDamage(ViewState["MACHINERYDAMAGEID"].ToString());
                ucStatus.Text = "Machinery Damage has been deleted.";

                ViewState["MACHINERYDAMAGEID"] = "";
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("MACHINERY"))
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

            //LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            //if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton riskCreate = (LinkButton)e.Item.FindControl("lnkReferenceNumber");
            if (riskCreate != null)
            {
                riskCreate.Visible = SessionUtil.CanAccess(this.ViewState, riskCreate.CommandName);
                riskCreate.Attributes.Add("onclick", "javascript:openNewWindow('ra', '', '" + Session["sitepath"] + "/Inspection/InspectionMachineryDamageGeneral.aspx?DashboardYN=1&MACHINERYDAMAGEID=" + ViewState["MACHINERYDAMAGEID"].ToString() + "&PageNo=" + ViewState["PAGENUMBER"] + "');");
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

            RadLabel lblMachineryDamageId = (RadLabel)e.Item.FindControl("lblMachineryDamageId");

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

