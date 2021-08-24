using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceComponentJobManual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["vesselid"] != null)
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobManual.aspx?vesselid=" + Request.QueryString["vesselid"] + "", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        else
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobManual.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvManual')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuPMSManual.AccessRights = this.ViewState;
        MenuPMSManual.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CJID"] = Request.QueryString["COMPONENTJOBID"];
            ViewState["VESSELID"] = null;
            if (Request.QueryString["vesselid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vesselid"];
            else
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            if (Request.QueryString["JOBYN"] != null)
                ViewState["JOBYN"] = Request.QueryString["JOBYN"].ToString();
            else
                ViewState["JOBYN"] = "0";

            gvManual.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);            
        }
    }
    protected void MenuPMSManual_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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

        string[] alColumns = { "FLDMANUALNAME" };
        string[] alCaptions = { "Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixPlannedMaintenanceComponentJobManual.Search(int.Parse(ViewState["VESSELID"].ToString())
                                     , General.GetNullableGuid(ViewState["CJID"].ToString())
                                     , sortexpression, sortdirection
                                     , int.Parse(ViewState["PAGENUMBER"].ToString())
                                     , gvManual.PageSize
                                     , ref iRowCount
                                     , ref iTotalPageCount
                                     ,General.GetNullableInteger(ViewState["JOBYN"].ToString()));

        General.ShowExcel("Manual", dt, alColumns, alCaptions, null, null);
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDMANUALNAME" };
            string[] alCaptions = { "Name" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixPlannedMaintenanceComponentJobManual.Search(int.Parse(ViewState["VESSELID"].ToString())
                                         , General.GetNullableGuid(ViewState["CJID"].ToString())
                                         , sortexpression, sortdirection
                                         , int.Parse(ViewState["PAGENUMBER"].ToString())
                                         , gvManual.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount
                                         , General.GetNullableInteger(ViewState["JOBYN"].ToString()));

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvManual", "Manual", alCaptions, alColumns, ds);

            gvManual.DataSource = dt;
            gvManual.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvManual_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "PAGE")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper() =="ADD")
            {
                RadTextBox filename = (RadTextBox)e.Item.FindControl("fileName");              
                if(!IsValidManual(filename.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string file = filename.Text.Replace("PMSManuals/", "/");
                FileInfo fi = new FileInfo(PhoenixGeneralSettings.CurrentGeneralSetting.PMSManualsPath + file);
                PhoenixPlannedMaintenanceComponentJobManual.Insert(int.Parse(ViewState["VESSELID"].ToString())
                                        , new Guid(ViewState["CJID"].ToString())
                                        , file
                                        , fi.Name);
                BindData();
                gvManual.Rebind();
            }           
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string id = drv["FLDMANUALID"].ToString();
                PhoenixPlannedMaintenanceComponentJobManual.Delete(int.Parse(ViewState["VESSELID"].ToString()), new Guid(id));
                BindData();
                gvManual.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvManual_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
            }
            LinkButton lnkManualName = (LinkButton)e.Item.FindControl("lnkManualName");
            if (lnkManualName != null)
            {
                lnkManualName.Attributes.Add("onclick", "javascript:return OpenPDF('../Common/Download.aspx?manualid="+drv["FLDMANUALID"].ToString()+"','"+drv["FLDMANUALNAME"].ToString() +"');");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvManual_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvManual.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvManual_SortCommand(object sender, GridSortCommandEventArgs e)
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
    private bool IsValidManual(string manual)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (manual.Trim().Equals(""))
            ucError.ErrorMessage = "Manual is required.";
        
        return (!ucError.IsError);
    }
}