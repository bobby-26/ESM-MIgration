using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceStockItemWithLocation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "ADD", ToolBarDirection.Right);
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);       
        MenuStockItem.AccessRights = this.ViewState;
        MenuStockItem.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            gvStockItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            if (Request.QueryString["WORKORDERID"] != null)
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
            ViewState["ISCONSUMED"] = string.Empty;
            if (Request.QueryString["isconsume"] != null)
                ViewState["ISCONSUMED"] = Request.QueryString["isconsume"];
            if (Request.QueryString["COMPNO"] != null && Request.QueryString["COMPNO"].Length >= 6)
                txtComponentNo.Text =  Request.QueryString["COMPNO"].Substring(0,6);
            //txtComponentName.Text = Request.QueryString["COMPNAME"];
        }
    }

    protected void MenuStockItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvStockItem.Rebind();
            }
            else if (CommandName.ToUpper().Equals("ADD"))
            {
                string strItemNumber = string.Empty;
                string strItemName = string.Empty;
                string strItemId = string.Empty;
                string strLocation = string.Empty;
                string strSpareItemLocation = string.Empty;

                foreach (GridDataItem gr in gvStockItem.Items)
                {
                    CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        strItemNumber += ((RadLabel)gr.FindControl("lblStockItemNumber")).Text + ",";
                        strItemName += ((LinkButton)gr.FindControl("lnkStockItemName")).Text + ",";
                        strItemId += ((RadLabel)gr.FindControl("lblStockItemId")).Text + ",";
                        strLocation += ((RadLabel)gr.FindControl("lblLocationId")).Text + ",";
                        strSpareItemLocation += ((RadLabel)gr.FindControl("lblSpareItemLocationId")).Text + ",";
                    }
                }
                if (strItemId == string.Empty)
                {
                    ucError.ErrorMessage = "Select atleast one or more parts";
                    ucError.Visible = true;
                    return;
                }
                if ((Request.QueryString["WORKORDERID"] != null) && (Request.QueryString["WORKORDERID"] != ""))
                {
                    InsertUsesParts(ViewState["WORKORDERID"].ToString(), strItemId, strLocation, strSpareItemLocation);
                    string script = "refreshParent();";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                }
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (ViewState["ISCONSUMED"].ToString() != string.Empty)
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportPartsConsumed.aspx?WORKORDERID=" + ViewState["WORKORDERID"].ToString() + "&COMPNO=" + Request.QueryString["COMPNO"]);
                else
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderPartsRequired.aspx?WORKORDERID=" + ViewState["WORKORDERID"].ToString() + "&COMPNO=" + Request.QueryString["COMPNO"]);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InsertUsesParts(string WorkOrderId, string csvItemId, string csvlocationid, string csvspareitemlocationid)
    {
        if (ViewState["ISCONSUMED"].ToString() == string.Empty)
            PhoenixPlannedMaintenanceUsesParts.InsertUsesParts(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(WorkOrderId), csvItemId, csvlocationid, csvspareitemlocationid);
        PhoenixPlannedMaintenanceUsesParts.InsertWorkOrderPartConsumed(PhoenixSecurityContext.CurrentSecurityContext.VesselID, csvItemId, new Guid(WorkOrderId), csvlocationid, csvspareitemlocationid);
        ucStatus.Text = "Spare items added.";
        ucStatus.Visible = true;
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            DataSet ds;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            int vesselid;
            if (Request.QueryString["vesselid"] != null)
                vesselid = int.Parse(Request.QueryString["vesselid"].ToString());
            else
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            ds = PhoenixInventoryComponentSpareItem.ComponentStockItemComponentSearch(Request.QueryString["componentid"],
                PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableString(txtNumberSearch.TextWithLiterals.TrimEnd('.')), General.GetNullableString(txtStockItemNameSearch.Text),
                General.GetNullableString(txtComponentName.Text), General.GetNullableString(txtComponentNo.TextWithLiterals.TrimEnd('.')),
                sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), gvStockItem.PageSize,
               ref iRowCount,
               ref iTotalPageCount);

            gvStockItem.DataSource = ds;
            gvStockItem.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStockItem_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvStockItem_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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

    protected void gvStockItem_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvStockItem.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvStockItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                Image img = (Image)e.Item.FindControl("imgFlag");

                if (drv["MINQTYFLAGE"].ToString() == "1")
                {
                    img.Visible = true;
                    img.ToolTip = "ROB is less than Minimum Level";
                }
                else
                    img.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}