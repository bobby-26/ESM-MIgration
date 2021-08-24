using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceComponentSearchAcrossVessel : PhoenixBasePage
{
    string ComponentNo = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentSearchAcrossVessel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSpareItemList')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            MenuComponentList.AccessRights = this.ViewState;
            MenuComponentList.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["partno"] = "";
                ViewState["Vesselid"] = "";
                if (Request.QueryString["ItemNo"] != null && Request.QueryString["ItemNo"] != "")
                {
                    ViewState["partno"] = Request.QueryString["ItemNo"].ToString();
                    this.Page.Title = "Component [" + ViewState["partno"].ToString() +"]";
                }
                //if (Request.QueryString["Vesselid"] != null && Request.QueryString["Vesselid"] != "")
                //{
                //    ViewState["Vesselid"] = Request.QueryString["Vesselid"].ToString();
                //}
                //lblComponentList.Text = lblComponentList.Text + "[" + ViewState["partno"].ToString() + "]";
                gvSpareItemList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            ComponentNo = ViewState["partno"].ToString();
            //Vesselid = Int32.Parse(ViewState["Vesselid"].ToString());

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuComponentList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDVESSELNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDSERIALNUMBER", "FLDCOMPONENTSTATUSNAME", "FLDTYPE", "FLDCLASSCODE" };
        string[] alCaptions = { "Vessel", "Component No.", "Name", "Serial Number", "Status", "Type", "Class Code" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = new DataSet();
        ds = PhoenixPlannedMaintenanceComponentSearch.ComponentSearchByReference(ComponentNo, PhoenixSecurityContext.CurrentSecurityContext.VesselID, sortexpression, sortdirection, gvSpareItemList.CurrentPageIndex + 1,
                                                               gvSpareItemList.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvSpareItemList", "Component", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvSpareItemList.DataSource = ds;
            gvSpareItemList.VirtualItemCount = iRowCount;
        }
        else
        {
            DataTable dt = ds.Tables[0];
            gvSpareItemList.DataSource = "";
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
   
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
   
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        DataTable dt;
        string[] alColumns = { "FLDVESSELNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDSERIALNUMBER", "FLDCOMPONENTSTATUSNAME", "FLDTYPE", "FLDCLASSCODE" };
        string[] alCaptions = { "Vessel", "Number", "Name", "Serial Number", "Status", "Type", "Class Code" };
        int iRowCount;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixPlannedMaintenanceComponentSearch.ComponentSearchByReference(ComponentNo, PhoenixSecurityContext.CurrentSecurityContext.VesselID, sortexpression, sortdirection, gvSpareItemList.CurrentPageIndex + 1,
                                                               gvSpareItemList.PageSize, ref iRowCount, ref iTotalPageCount);
        dt = ds.Tables[0];
        General.ShowExcel("Component", dt, alColumns, alCaptions, null, null);

    }

    protected void gvSpareItemList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSpareItemList_ItemDataBound(object sender, GridItemEventArgs e)
    {
      
    }

    protected void gvSpareItemList_ItemCommand(object sender, GridCommandEventArgs e)
    {
       
    }
}