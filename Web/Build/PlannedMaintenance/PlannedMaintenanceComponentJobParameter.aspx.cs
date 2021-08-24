using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceComponentJobParameter : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["vesselid"] != null)
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobParameter.aspx?vesselid=" + Request.QueryString["vesselid"] + "", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        else
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobParameter.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "EXCEL");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvJobParameter')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        if (Request.QueryString["vesselid"] != null)
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobParameter.aspx?vesselid=" + Request.QueryString["vesselid"] + "", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
        else
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobParameter.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        MenuJobParameter.AccessRights = this.ViewState;
        MenuJobParameter.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CJID"] = Request.QueryString["COMPONENTJOBID"];
            gvJobParameter.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["VESSELID"] = null;
            if (Request.QueryString["vesselid"] != null)
                ViewState["VESSELID"] = Request.QueryString["vesselid"];
            else
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        }
    }
    protected void MenuJobParameter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;            
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
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

        string[] alColumns = { "FLDPARAMETERCODE", "FLDPARAMETERNAME", "FLDPARAMETERTYPENAME" };
        string[] alCaptions = { "Code", "Name", "Type" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixPlannedMaintenanceJobParameter.SearchParameterMapping(int.Parse(ViewState["VESSELID"].ToString())
                                          , General.GetNullableGuid(ViewState["CJID"].ToString())
                                          , sortexpression, sortdirection
                                          , 1
                                          , iRowCount
                                          , ref iRowCount
                                          , ref iTotalPageCount);

        General.ShowExcel("Job Parameter", dt, alColumns, alCaptions, null, null);
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDPARAMETERCODE", "FLDPARAMETERNAME", "FLDPARAMETERTYPENAME" };
            string[] alCaptions = { "Code", "Name", "Type" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixPlannedMaintenanceJobParameter.SearchParameterMapping(int.Parse(ViewState["VESSELID"].ToString())
                                         , General.GetNullableGuid(ViewState["CJID"].ToString())
                                         , sortexpression, sortdirection
                                         , int.Parse(ViewState["PAGENUMBER"].ToString())
                                         , gvJobParameter.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvJobParameter", "Componet Job Parameter", alCaptions, alColumns, ds);

            gvJobParameter.DataSource = dt;
            gvJobParameter.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvJobParameter_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "PAGE")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper() =="ADD")
            {
                UserControlJobParameter parameter = (UserControlJobParameter)e.Item.FindControl("ddlJobParameter");
                string parameterid = parameter.SelectedJobParameter;
                if (!IsValidParameter(parameterid))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceJobParameter.InsertParameterMapping(General.GetNullableGuid(parameterid).Value,new Guid(ViewState["CJID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()));
                BindData();
                gvJobParameter.Rebind();
            }           
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                string id = drv["FLDMAPPINGID"].ToString();
                PhoenixPlannedMaintenanceJobParameter.DeleteParameterMapping(new Guid(id), int.Parse(ViewState["VESSELID"].ToString()));
                BindData();
                gvJobParameter.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvJobParameter_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
            }                               
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvJobParameter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvJobParameter.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvJobParameter_SortCommand(object sender, GridSortCommandEventArgs e)
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
    private bool IsValidParameter(string parameterid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(parameterid) == null)
            ucError.ErrorMessage = "Parameter is required.";        

        return (!ucError.IsError);
    }
}