using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderPartsRequired : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["WORKORDERID"] = null;
            if (Request.QueryString["WORKORDERID"] != null)
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"];
            ViewState["WORKORDERGROUPID"] = string.Empty;
            if (Request.QueryString["WORKORDERGROUPID"] != null)
                ViewState["WORKORDERGROUPID"] = Request.QueryString["WORKORDERGROUPID"];
            LoadCopyToList();
            gvUsesParts.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();

        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderPartsRequired.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvUsesParts')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT"); 
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderPartsRequired.aspx?COMPNO="+ Request.QueryString["COMPNO"] + "&COMPNAME="+Request.QueryString["COMPNAME"], "Part", "<i class=\"fas fa-cogs\"></i>", "ADDPART");
        toolbar.AddFontAwesomeButton("javascript:showDialog();", "Copy", "<i class=\"fas fa-copy\"></i>", "COPY");
        MenugvUsesParts.AccessRights = this.ViewState;
        MenugvUsesParts.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Copy", "COPY", ToolBarDirection.Right);
        menuWorkorderCopy.AccessRights = this.ViewState;
        menuWorkorderCopy.MenuList = toolbar.Show();

    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "FLDROB" };
        string[] alCaptions = { "Part Number", "Part Name", "Maker Reference", "ROB" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPlannedMaintenanceUsesParts.UsesPartsSearch(General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
            , PhoenixSecurityContext.CurrentSecurityContext.VesselID, sortexpression, sortdirection
            , gvUsesParts.CurrentPageIndex + 1, gvUsesParts.PageSize, ref iRowCount, ref iTotalPageCount);
        General.SetPrintOptions("gvUsesParts", "Parts Required", alCaptions, alColumns, ds);
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
        string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDMAKERREFERENCE", "FLDROB" };
        string[] alCaptions = { "Part Number", "Part Name", "Maker Reference", "ROB" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPlannedMaintenanceUsesParts.UsesPartsSearch(General.GetNullableGuid(ViewState["WORKORDERID"].ToString())
                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                , sortexpression, sortdirection
                                                                , gvUsesParts.CurrentPageIndex + 1
                                                                , gvUsesParts.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);
        General.ShowExcel("Parts Required", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
                gvUsesParts.Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if(CommandName.ToUpper().Equals("ADDPART"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceStockItemWithLocation.aspx?WORKORDERID="+ ViewState["WORKORDERID"].ToString()+ "&COMPNO=" + Request.QueryString["COMPNO"] + "&COMPNAME=" + Request.QueryString["COMPNAME"]);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private bool IsValidCopy()
    {
        ucError2.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ddlWorkOrder.SelectedValue) == null)
            ucError2.ErrorMessage = "Copy To is required.";

        return (!ucError2.IsError);
    }
    protected void gvUsesParts_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        foreach (GridBatchEditingCommand cmd in e.Commands)
        {
            string workorderlineitemid = cmd.NewValues["FLDWORKORDERLINEITEMID"].ToString();
            string partused = cmd.NewValues["FLDQUANTITY"].ToString();
            string spareitem = cmd.NewValues["FLDSPAREITEMID"].ToString();
            string woid = cmd.NewValues["FLDWORKORDERID"].ToString();

            PhoenixPlannedMaintenanceUsesParts.UpdateUsesParts(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , new Guid(workorderlineitemid)
                                                                   , new Guid(woid)
                                                                   , new Guid(spareitem)
                                                                   , Convert.ToDecimal(partused)
                                                                   , null, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            PhoenixPlannedMaintenanceWorkOrderGroup.RefreshJob(new Guid(woid), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            string script = "CloseModelWindow();";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        }
    }   

    private void DeletegvUsesPartss(string workorderlineid)
    {
        PhoenixPlannedMaintenanceUsesParts.DeleteUsesParts(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(workorderlineid));
    }

    private void DeletePartsConsumed(string spareitemid, string locationid, string workorderid)
    {
        PhoenixPlannedMaintenanceUsesParts.DeletePartsConsumed(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(spareitemid),int.Parse(locationid),new Guid(workorderid));
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //NameValueCollection nvc = Filter.CurrentPickListSelection;
            //string stockitemid = nvc.Get("lblStockItemId").ToString();
            //string location = nvc.Get("lblLocationId").ToString();
            //string spareitemlocationid = nvc.Get("lblSpareItemLocationId").ToString();

            //if ((Request.QueryString["WORKORDERID"] != null) && (Request.QueryString["WORKORDERID"] != ""))
            //{
            //    InsertUsesParts(ViewState["WORKORDERID"].ToString(), stockitemid, location, spareitemlocationid);
            //}
           
            gvUsesParts.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RadGrid1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string workorderid = ((RadLabel)e.Item.FindControl("lblWorkOrderID")).Text;
                DeletegvUsesPartss(((RadLabel)e.Item.FindControl("lblWorkOrderLineID")).Text);
                DeletePartsConsumed(((RadLabel)e.Item.FindControl("lblSpareItemId")).Text, ((RadLabel)e.Item.FindControl("lblLocationId")).Text, workorderid);
                int count = gvUsesParts.Items.Count;
                PhoenixPlannedMaintenanceWorkOrderGroup.RefreshJob(new Guid(workorderid), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                if (General.GetNullableGuid(ViewState["WORKORDERGROUPID"].ToString()).HasValue && count <= 1)
                    PhoenixPlannedMaintenanceWorkOrderGroup.RefreshToolboxMeet(new Guid(ViewState["WORKORDERGROUPID"].ToString()));
                string script = "CloseModelWindow();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
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
            if (e.Item is GridDataItem)
            {
                RadLabel minQTY = (RadLabel)e.Item.FindControl("lblminqtyflag");
                Image img = (Image)e.Item.FindControl("imgFlag");
                if (minQTY.Text.ToString() == "1")
                {
                    img.Visible = true;
                    img.ToolTip = "ROB is less than Minimum Level";
                }
                else
                    img.Visible = false;
            }

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

    protected void menuWorkorderCopy_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("COPY"))
            {
                if(!IsValidCopy())
                {
                    ucError2.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceUsesParts.CopyUsesParts(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["WORKORDERID"].ToString())
                    , new Guid(ddlWorkOrder.SelectedValue));
                string script = "CloseModelWindow();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }            
        }
        catch (Exception ex)
        {
            ucError2.ErrorMessage = ex.Message;
            ucError2.Visible = true;

        }
    }
    private void LoadCopyToList()
    {
        DataTable dt = PhoenixPlannedMaintenanceUsesParts.ListWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(ViewState["WORKORDERID"].ToString()));
        ddlWorkOrder.DataTextField = "FLDNAME";
        ddlWorkOrder.DataValueField = "FLDWORKORDERID";
        ddlWorkOrder.DataSource = dt;
        ddlWorkOrder.DataBind();
        ddlWorkOrder.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
}