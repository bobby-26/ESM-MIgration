using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderReportPartsConsumed : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["WORKORDERID"] = null;
            ViewState["COMPNO"] = null;
            ViewState["PARTUSED"] = null;
            ViewState["WORKORDERLINEITEMID"] = null;
            ViewState["WORKORDERID"] = "";
            if (Request.QueryString["WORKORDERID"] != null)
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
            //getReportId();
            gvUsesParts.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportPartsConsumed.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvUsesParts')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderReportPartsConsumed.aspx?COMPNO=" + Request.QueryString["COMPNO"] + "&COMPNAME=" + Request.QueryString["COMPNAME"], "Part", "<i class=\"fas fa-cogs\"></i>", "ADDPART");
        MenugvUsesParts.AccessRights = this.ViewState;
        MenugvUsesParts.MenuList = toolbar.Show();
        //MenugvUsesParts.SetTrigger(pnlgvUsesParts);
    }
    //private void getReportId()
    //{
    //    try
    //    {
    //        if ((Request.QueryString["WORKORDERID"] != null) && (Request.QueryString["WORKORDERID"] != ""))
    //        {
    //            DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.EditWorkOrderReport(new Guid(Request.QueryString["WORKORDERID"]));
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                DataRow dr = ds.Tables[0].Rows[0];
    //                ViewState["ISREPORTPATRS"] = dr["FLDREPORTUSEDPARTS"].ToString();
    //                ViewState["WORKORDERID"] = dr["FLDWORKORDERID"].ToString();
    //                ViewState["OPERATIONMODE"] = "EDIT";
    //                if (dr["FLDCOMPONENTNUMBER"].ToString().Length > 6)
    //                {
    //                    ViewState["COMPNO"] = dr["FLDCOMPONENTNUMBER"].ToString().Substring(0, 6);
    //                }
    //                else
    //                {
    //                    ViewState["COMPNO"] = dr["FLDCOMPONENTNUMBER"].ToString();
    //                }
    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
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

        DataSet ds = PhoenixPlannedMaintenanceUsesParts.WorkOrderPartConsumedSearch(General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
                                                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                                , sortexpression, sortdirection
                                                                                                , gvUsesParts.CurrentPageIndex + 1
                                                                                                , gvUsesParts.PageSize
                                                                                                , ref iRowCount, ref iTotalPageCount);
        General.SetPrintOptions("gvUsesParts", "Stock Used", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvUsesParts.DataSource = ds;
            gvUsesParts.VirtualItemCount = iRowCount;
        }
        else
        {
            DataTable dt = ds.Tables[0];
            gvUsesParts.DataSource = "";
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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

        ds = PhoenixPlannedMaintenanceUsesParts.WorkOrderPartConsumedSearch(General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                        , sortexpression
                                                                                        , sortdirection
                                                                                        , 1
                                                                                        , iRowCount
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
            if (CommandName.ToUpper().Equals("ADDPART"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceStockItemWithLocation.aspx?WORKORDERID=" + ViewState["WORKORDERID"].ToString() + "&COMPNO=" + Request.QueryString["COMPNO"] + "&isconsume=1");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
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
    
    private void UpdategvUsesParts(string workorderlineid, string partused, string location)
    {
        try
        {
            int iMessageCode = 0;
            string iMessageText = "";

            PhoenixPlannedMaintenanceUsesParts.UpdateWorkOrderPartConsumed(new Guid(workorderlineid)
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
    protected void gvUsesParts_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        foreach (GridBatchEditingCommand cmd in e.Commands)
        {
            string workorderlineitemid = cmd.NewValues["FLDWORKORDERLINEITEMID"].ToString();
            string partused = cmd.NewValues["FLDQUANTITY"].ToString();
            string location = cmd.NewValues["FLDLOCATION"].ToString();
            int iMessageCode = 0;
            string iMessageText = string.Empty;

            PhoenixPlannedMaintenanceUsesParts.UpdateWorkOrderPartConsumed(new Guid(workorderlineitemid)
                , General.GetNullableDecimal(partused), 1, int.Parse(location), ref iMessageCode, ref iMessageText);            
        }    
    }
    protected void ucConfirm_OnClick(object sender, EventArgs e)
    {
        UpdategvUsesParts(ViewState["WORKORDERLINEITEMID"].ToString(), ViewState["PARTUSED"].ToString(), ViewState["LOCATION"].ToString());
        BindData();
    }
    private void DeletegvUsesPartss(string workorderlineid)
    {
        PhoenixPlannedMaintenanceUsesParts.DeleteWorkOrderPartConsumed(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(workorderlineid));
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
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

            //if (e.CommandName.ToUpper().Equals("UPDATE"))
            //{
            //    ViewState["LOCATION"] = ((RadDropDownList)e.Item.FindControl("ddlLocation")).SelectedValue;
            //    if (!IsValidUsesParts(ViewState["WORKORDERID"].ToString()
            //            , ((RadLabel)e.Item.FindControl("lblSpareItemId")).Text
            //            , ((UserControlDecimal)e.Item.FindControl("txtQuantityEdit")).Text
            //            ))
            //    {
            //        ucError.Visible = true;
            //        return;
            //    }

            //    ViewState["WORKORDERLINEITEMID"] = ((RadLabel)e.Item.FindControl("lblWorkOrderLineID")).Text;
            //    ViewState["PARTUSED"] = ((UserControlDecimal)e.Item.FindControl("txtQuantityEdit")).Text;
            //    UpdategvUsesParts(ViewState["WORKORDERLINEITEMID"].ToString(), ViewState["PARTUSED"].ToString(), ViewState["LOCATION"].ToString());
            //}
            //gvUsesParts.Rebind();

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
                    db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                }
            }
            if (e.Item is GridEditableItem)
            {
                //RadLabel lblSpareItemId = (RadLabel)e.Item.FindControl("lblSpareItemId");
                //RadDropDownList ddlLocation = (RadDropDownList)e.Item.FindControl("ddlLocation");
                //DataRowView drv = (DataRowView)e.Item.DataItem;
                //if (ddlLocation != null)
                //{
                //    ddlLocation.DataSource = PhoenixInventorySpareItemLocation.ListSpareItemLocation(new Guid(lblSpareItemId.Text), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

                //    ddlLocation.DataBind();
                //    ddlLocation.SelectedValue = drv["FLDLOCATION"].ToString();
                //}
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}