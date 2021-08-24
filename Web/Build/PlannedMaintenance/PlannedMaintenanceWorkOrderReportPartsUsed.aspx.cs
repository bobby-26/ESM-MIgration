using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Collections.Specialized;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inventory;

public partial class PlannedMaintenanceWorkOrderReportPartsUsed : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["WORKORDERID"] = null;
            ViewState["COMPNO"] = null;
            ViewState["PARTUSED"] = null;
            ViewState["WORKORDERLINEITEMID"] = null;
            ViewState["WORKORDERREPORTID"] = "";
            if (Request.QueryString["WORKORDERID"] != null)
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
            getReportId();
            gvUsesParts.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportPartsUsed.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvUsesParts')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListStockItemWithLocation.aspx?mode=multi&stockno=" + ViewState["COMPNO"] + "&WORKORDERREPORTID="+ ViewState["WORKORDERREPORTID"] + "', true);", "Part", "<i class=\"fas fa-cogs\"></i>", "ADDPART");

        MenugvUsesParts.MenuList = toolbar.Show();
        //MenugvUsesParts.SetTrigger(pnlgvUsesParts);
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
                    ViewState["ISREPORTPATRS"] = dr["FLDREPORTUSEDPARTS"].ToString();
                    ViewState["WORKORDERREPORTID"] = dr["FLDWORKORDERREPORTID"].ToString();
                    ViewState["OPERATIONMODE"] = "EDIT";
                    if (dr["FLDCOMPONENTNUMBER"].ToString().Length > 6)
                    {
                        ViewState["COMPNO"] = dr["FLDCOMPONENTNUMBER"].ToString().Substring(0, 6);
                    }
                    else
                    {
                        ViewState["COMPNO"] = dr["FLDCOMPONENTNUMBER"].ToString();
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
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDLOCATIONNAME", "FLDQUANTITY", "FLDUNITNAME" };
        string[] alCaptions = { "Part Number", "Part Name", "Location", "Quantity", "Unit" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.WorkOrderReportPartConsumedSearch(General.GetNullableGuid(ViewState["WORKORDERREPORTID"].ToString())
                                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                                , sortexpression, sortdirection
                                                                                                , gvUsesParts.CurrentPageIndex + 1
                                                                                                , gvUsesParts.PageSize
                                                                                                , ref iRowCount, ref iTotalPageCount);
        General.SetPrintOptions("gvUsesParts", "Stock Used", alCaptions, alColumns, ds);

        gvUsesParts.DataSource = ds;
        gvUsesParts.VirtualItemCount = iRowCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDLOCATIONNAME", "FLDQUANTITY", "FLDUNITNAME" };
        string[] alCaptions = { "Part Number", "Part Name", "Location", "Quantity", "Unit" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPlannedMaintenanceWorkOrderReport.WorkOrderReportPartConsumedSearch(General.GetNullableGuid(ViewState["WORKORDERREPORTID"].ToString())
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                        , sortexpression
                                                                                        , sortdirection
                                                                                        , gvUsesParts.CurrentPageIndex + 1
                                                                                        , gvUsesParts.PageSize
                                                                                        , ref iRowCount
                                                                                        , ref iTotalPageCount);
        General.ShowExcel("Stock Used", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void gvUsesParts_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvUsesParts.CurrentPageIndex = 0;
                BindData();
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
    protected void gvUsesParts_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvUsesParts, "Edit$" + e.Row.RowIndex.ToString(), false);
        }

        SetKeyDownScroll(sender, e);
    }

    private bool IsValidUsesParts(string workorderid, string sparepartiid, string requiredvalue)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (workorderid.Trim().Equals(""))
            ucError.ErrorMessage = "Please first fill general information.";

        if (sparepartiid.Trim().Equals(""))
            ucError.ErrorMessage = "Spare Item is required.";

        if ((requiredvalue.Trim().Equals("")) && General.GetNullableDecimal(requiredvalue) == null)
            ucError.ErrorMessage = "Used Quantity is required";
        return (!ucError.IsError);
    }
    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }
    }
    private void UpdategvUsesParts(string workorderlineid, string partused, string location)
    {
        try
        {
            int iMessageCode = 0;
            string iMessageText = "";

            PhoenixPlannedMaintenanceWorkOrderReport.UpdateWorkOrderReportPartConsumed(new Guid(workorderlineid)
                , General.GetNullableDecimal(partused), ucConfirm.confirmboxvalue, int.Parse(location), ref iMessageCode, ref iMessageText);

            if (iMessageCode == 1)
                throw new ApplicationException(iMessageText);

        }
        catch (ApplicationException aex)
        {
            ucConfirm.HeaderMessage = "Please Confirm";
            ucConfirm.ErrorMessage = aex.Message;
            ucConfirm.Visible = true;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void ucConfirm_OnClick(object sender, EventArgs e)
    {
        UpdategvUsesParts(ViewState["WORKORDERLINEITEMID"].ToString(), ViewState["PARTUSED"].ToString(), ViewState["LOCATION"].ToString());
        BindData();
    }
    private void DeletegvUsesPartss(string workorderlineid)
    {
        PhoenixPlannedMaintenanceWorkOrderReport.DeleteWorkOrderReportPartConsumed(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(workorderlineid));
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            string stockitemid = nvc.Get("lblStockItemId").ToString();
            string location = nvc.Get("lblLocationId").ToString();
            string spareitemlocationid = nvc.Get("lblSpareItemLocationId").ToString();

            if ((ViewState["WORKORDERREPORTID"] != null) && (ViewState["WORKORDERREPORTID"].ToString() != ""))
            {
                PhoenixPlannedMaintenanceWorkOrderReport.InsertWorkOrderReportPartConsumed(PhoenixSecurityContext.CurrentSecurityContext.VesselID, stockitemid, new Guid(ViewState["WORKORDERREPORTID"].ToString()), location, spareitemlocationid);
            }
            BindData();
            gvUsesParts.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvUsesParts_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeletegvUsesPartss(((RadLabel)e.Item.FindControl("lblWorkOrderLineID")).Text);
            }

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                ViewState["LOCATION"] = ((RadDropDownList)e.Item.FindControl("ddlLocation")).SelectedValue;
                if (!IsValidUsesParts(ViewState["WORKORDERREPORTID"].ToString()
                        , ((RadLabel)e.Item.FindControl("lblSpareItemId")).Text
                        , ((UserControlDecimal)e.Item.FindControl("txtQuantityEdit")).Text
                        ))
                {
                    ucError.Visible = true;
                    return;
                }

                ViewState["WORKORDERLINEITEMID"] = ((RadLabel)e.Item.FindControl("lblWorkOrderLineID")).Text;
                ViewState["PARTUSED"] = ((UserControlDecimal)e.Item.FindControl("txtQuantityEdit")).Text;
                UpdategvUsesParts(ViewState["WORKORDERLINEITEMID"].ToString(), ViewState["PARTUSED"].ToString(), ViewState["LOCATION"].ToString());
            }
            gvUsesParts.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvUsesParts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvUsesParts_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                }
            }
            if(e.Item is GridEditableItem)
            {
                RadLabel lblSpareItemId = (RadLabel)e.Item.FindControl("lblSpareItemId");
                RadDropDownList ddlLocation = (RadDropDownList)e.Item.FindControl("ddlLocation");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (ddlLocation != null)
                {
                    ddlLocation.DataSource = PhoenixInventorySpareItemLocation.ListSpareItemLocation(new Guid(lblSpareItemId.Text), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

                    ddlLocation.DataBind();
                    ddlLocation.SelectedValue = drv["FLDLOCATION"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvUsesParts_PreRender(object sender, EventArgs e)
    {
        gvUsesParts.Rebind();
    }
}
