using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderMappingList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("History", "HISTORY", ToolBarDirection.Right);
            MenuMappedWorkOrder.AccessRights = this.ViewState;
            MenuMappedWorkOrder.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["REPORTID"] = Request.QueryString["REPORTID"].ToString();
                ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
                ViewState["STORETYPEID"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuMappedWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        Response.Redirect("../PlannedMaintenance/PlannedMaintenanceHistoryTemplateReports.aspx?FORMID=" + ViewState["FORMID"].ToString(), true);
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds;
            ds = PhoenixCommonPlannedMaintenance.WorkOrderListSearch1(General.GetNullableGuid(ViewState["REPORTID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                        , sortexpression, sortdirection,
                                                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                        General.ShowRecords(null),
                                                                        ref iRowCount,
                                                                        ref iTotalPageCount);


            RadGrid1.DataSource = ds;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_PreRender(object sender, EventArgs e)
    {
        //RadGrid1.MasterTableView.GetColumn("Activeyn").Visible = false;
        //RadGrid1.Rebind();
    }

    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = new RadGrid();
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                int nCurrentRow = e.Item.RowIndex;

                RadLabel lblWorkOrderId = (RadLabel)e.Item.FindControl("lblWorkOrderId");
                RadLabel lblWorkOrderNo = (RadLabel)e.Item.FindControl("lblWorkorderNumber");
                RadLabel lblComponentid = (RadLabel)e.Item.FindControl("lblComponentId");
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportList.aspx?IsMaintenanceForm=1&WORKORDERID=" + lblWorkOrderId.Text.Trim() + "&WORKORDERNO=" + lblWorkOrderNo.Text.Trim() + "&COMPONENTID=" + lblComponentid.Text.Trim() + "&REPORTID=" + ViewState["REPORTID"].ToString() + "&FORMID=" + ViewState["FORMID"].ToString(), true);
            }
            if (e.CommandName == "DELETE")
            {
                RadLabel lblreportmappingid = (RadLabel)e.Item.FindControl("lblreportmappingid");
                PhoenixPlannedMaintenanceHistoryTemplate.DeleteWorkorderMapping(new Guid(lblreportmappingid.Text));
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}

