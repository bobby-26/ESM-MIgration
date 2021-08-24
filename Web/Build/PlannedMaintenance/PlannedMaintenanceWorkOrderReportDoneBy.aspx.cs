using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderReportDoneBy : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportDoneBy.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvResources')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenugvResources.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["WORKORDERID"] = null;
            ViewState["TOTALDURATION"] = null;
            ViewState["SPENTHOURS"] = null;
            ViewState["WORKORDERREPORTID"] = "";
            if(Request.QueryString["WORKORDERID"]!=null)
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
            getReportId();
            
        }
        BindData();
    }
    private void getReportId()
    {
        try
        {
            if ((Request.QueryString["WORKORDERID"] != null) && (Request.QueryString["WORKORDERID"] != ""))
            {
                DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.EditWorkOrderReport(new Guid(Request.QueryString["WORKORDERID"]));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["ISREPORTRESOURCES"] = dr["FLDREPORTUSEDRESOURCES"].ToString();
                    ViewState["WORKORDERREPORTID"] = dr["FLDWORKORDERREPORTID"].ToString();
                    ViewState["TOTALDURATION"] = dr["FLDWORKDURATION"].ToString();
                    ViewState["SPENTHOURS"] = dr["FLDSPENTHOURS"].ToString();
                    ViewState["OPERATIONMODE"] = "EDIT";
                }
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
        string[] alColumns = { "FLDDISCIPLINENAME", "FLDSPENTHOURS" };
        string[] alCaptions = { "Discipline", "Spent Hours"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.WorkOrderDoneBySearch(General.GetNullableGuid(ViewState["WORKORDERREPORTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID, sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()), gvResources.PageSize, ref iRowCount, ref iTotalPageCount);
        General.SetPrintOptions("gvResources", "Resources", alCaptions, alColumns, ds);
        
        gvResources.DataSource = ds;
        gvResources.VirtualItemCount = iRowCount;
      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvResources_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridFooterItem)
        {
            GridFooterItem footerItem = (GridFooterItem)gvResources.MasterTableView.GetItems(GridItemType.Footer)[0];
            LinkButton db = (LinkButton)footerItem.FindControl("cmdAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }

        if (e.Item is GridDataItem)
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
            }

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null)
            {
                save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
            }

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null)
            {
                cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
            }
            LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
            {
                cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                cmdDelete.Visible = SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName);
            }
            UserControlDiscipline ucDiscipline = (UserControlDiscipline)e.Item.FindControl("ucRankAdd");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucDiscipline != null) ucDiscipline.SelectedDiscipline = drv["FLDDISCIPLINENAME"].ToString();
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDDISCIPLINENAME", "FLDSPENTHOURS" };
        string[] alCaptions = { "Discipline", "Spent Hours" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixPlannedMaintenanceWorkOrderReport.WorkOrderDoneBySearch(General.GetNullableGuid(ViewState["WORKORDERREPORTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID, sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
        General.ShowExcel("Resources", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void gvResources_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvResources.SelectedIndexes.Clear();
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvResources.Rebind();
            }
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
  
    private bool IsValidResources(string workorderid, string rankid, string spenttime)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        decimal resultDecimal;
        if (workorderid.Trim().Equals(""))
            ucError.ErrorMessage = "Work Order is required.";
        if (rankid.Trim().Equals("") || General.GetNullableInteger(rankid) == null)
            ucError.ErrorMessage = "Discipline is required.";
        if ((spenttime.Trim().Equals("")) || General.GetNullableDecimal(spenttime) == null || General.GetNullableDecimal(spenttime) <= 0)
            ucError.ErrorMessage = "Spent Hours is required";
        if (ViewState["WORKORDERREPORTID"] == null || ViewState["WORKORDERREPORTID"].ToString() == "")
            ucError.ErrorMessage = "General information is required";
        if (ViewState["TOTALDURATION"] != null && ViewState["TOTALDURATION"].ToString() != string.Empty
            && decimal.TryParse(spenttime, out resultDecimal) && resultDecimal > decimal.Parse(ViewState["TOTALDURATION"].ToString()))
            ucError.ErrorMessage = "Spent Hours cannot be greater than Total Duration(Hrs.)";
        return (!ucError.IsError);
    }
    private bool IsValidResourcesUpdate(string workorderid, string spenttime)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (workorderid.Trim().Equals(""))
            ucError.ErrorMessage = "Work Order is required.";
        if ((spenttime.Trim().Equals("")) || General.GetNullableDecimal(spenttime) == null || General.GetNullableDecimal(spenttime) <= 0)
            ucError.ErrorMessage = "Spent Hours is required";      
        return (!ucError.IsError);
    }
   
    private void InsertResources(string workorderid,string rainkid, string spenthours )
    {
        PhoenixPlannedMaintenanceWorkOrderReport.InsertWorkOrderDoneBy(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableGuid(ViewState["WORKORDERREPORTID"].ToString()),new Guid(workorderid),  General.GetNullableInteger(rainkid), General.GetNullableDecimal(spenthours));
    }
    private void UpdateResources(string workorderlineid, string workorderid, string spenthours)
    {
        PhoenixPlannedMaintenanceWorkOrderReport.UpdateWorkOrderDoneBy(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(workorderlineid),new Guid(ViewState["WORKORDERREPORTID"].ToString()),  General.GetNullableDecimal(spenthours));
    }
    private void DeleteResources(string workorderreportdoneby)
    {
        PhoenixPlannedMaintenanceWorkOrderReport.DeleteWorkOrderDoneBy(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(workorderreportdoneby));
    }

    protected void gvResources_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvResources_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridFooterItem footerItem = (GridFooterItem)gvResources.MasterTableView.GetItems(GridItemType.Footer)[0];
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidResources(ViewState["WORKORDERID"].ToString(), ((UserControlDiscipline)footerItem.FindControl("ucRankAdd")).SelectedDiscipline,
                      ((UserControlDecimal)footerItem.FindControl("txtSpentHoursAdd")).Text
                   ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertResources(ViewState["WORKORDERID"].ToString(), ((UserControlDiscipline)footerItem.FindControl("ucRankAdd")).SelectedDiscipline,
                      ((UserControlDecimal)footerItem.FindControl("txtSpentHoursAdd")).Text);
            }
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidResourcesUpdate(((Label)eeditedItem.FindControl("lblWorkOrderID")).Text
                    , ((UserControlDecimal)eeditedItem.FindControl("txtSpentHoursEdit")).Text
                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateResources
                    (((Label)eeditedItem.FindControl("lblWorkDoneByID")).Text
                   , ((Label)eeditedItem.FindControl("lblWorkOrderID")).Text
                   , ((UserControlDecimal)eeditedItem.FindControl("txtSpentHoursEdit")).Text

                     );
                gvResources.EditIndexes.Clear();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteResources(((Label)eeditedItem.FindControl("lblWorkDoneByID")).Text);
            }
            BindData();
            gvResources.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvResources_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (!IsValidResourcesUpdate(((Label)e.Item.FindControl("lblWorkOrderID")).Text
                    , ((UserControlDecimal)e.Item.FindControl("txtSpentHoursEdit")).Text
                    ))
            {
                ucError.Visible = true;
                return;
            }

            UpdateResources
                    (((Label)e.Item.FindControl("lblWorkDoneByID")).Text
                   , ((Label)e.Item.FindControl("lblWorkOrderID")).Text
                   , ((UserControlDecimal)e.Item.FindControl("txtSpentHoursEdit")).Text

                     );
            gvResources.EditIndexes.Clear();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvResources_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            DeleteResources(((Label)eeditedItem.FindControl("lblWorkDoneByID")).Text);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
