using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceOrderInformationAcknowledgeList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceOrderInformationAcknowledgeList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAck')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuOrderInformation.Title = "Acknowledged";
        MenuOrderInformation.AccessRights = this.ViewState;
        MenuOrderInformation.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceOrderInformationAcknowledgeList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "ExcelPending");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvAckPen')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuOrderInformationPending.Title = "Pending";
        MenuOrderInformationPending.AccessRights = this.ViewState;
        MenuOrderInformationPending.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["ID"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["o"]))
            {
                ViewState["ID"] = Request.QueryString["o"];
            }            
        }
    }
    protected void MenuOrderInformation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("EXCELPENDING"))
            {
                ShowExcelPending();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowExcel()
    {
        string[] alColumns = { "FLDFILENO", "FLDRANKCODE", "FLDEMPLOYEENAME", "FLDREADDATE" };
        string[] alCaptions = { "File No", "Rank", "Name", "Read On" };

        DataTable dt = PhoenixPlannedMaintenanceOrderInformation.ListAcknowledge(new Guid(ViewState["ID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        General.ShowExcel("Acknowledge List", dt, alColumns, alCaptions, null, null);
    }

    private void ShowExcelPending()
    {
        string[] alColumns = { "FLDFILENO", "FLDRANKCODE", "FLDEMPLOYEENAME" };
        string[] alCaptions = { "File No", "Rank", "Name" };

        DataTable dt = PhoenixPlannedMaintenanceOrderInformation.ListAcknowledgePending(new Guid(ViewState["ID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        General.ShowExcel("Acknowledge Pending List", dt, alColumns, alCaptions, null, null);
    }
    protected void gvAck_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

        try
        {
            RadGrid grid = (RadGrid)sender;          

            string[] alColumns = { "FLDFILENO","FLDRANKCODE", "FLDEMPLOYEENAME", "FLDREADDATE" };
            string[] alCaptions = { "File No", "Rank", "Name", "Read On" };            

            DataTable dt = PhoenixPlannedMaintenanceOrderInformation.ListAcknowledge(new Guid(ViewState["ID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvAck", "Acknowledge List", alCaptions, alColumns, ds);

            grid.DataSource = dt;           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvAckPen_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;

        string[] alColumns = { "FLDFILENO", "FLDRANKCODE", "FLDEMPLOYEENAME" };
        string[] alCaptions = { "File No", "Rank", "Name"};

        DataTable dt = PhoenixPlannedMaintenanceOrderInformation.ListAcknowledgePending(new Guid(ViewState["ID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvAckPen", "Acknowledge Pending List", alCaptions, alColumns, ds);

        grid.DataSource = dt;
    }

    protected void gvAckPen_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        LinkButton ack = (LinkButton)e.Item.FindControl("cmdAck");
        if (ack != null && drv != null)
        {
            ack.Visible = !drv["FLDISMST"].ToString().Equals("0") && SessionUtil.CanAccess(this.ViewState, ack.CommandName);
            ack.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event, 'Are you sure you want to acknowledge on behalf of " + drv["FLDRANKCODE"] + " " + drv["FLDEMPLOYEENAME"] + " ?'); return false;");
        }
    }

    protected void gvAckPen_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            if (e.CommandName.ToUpper() == "ACK")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string id = item.GetDataKeyValue("FLDORDERINFORMATIONID").ToString();
                string eid = item.GetDataKeyValue("FLDEMPLOYEEID").ToString();
                PhoenixPlannedMaintenanceOrderInformation.Acknowledge(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(eid));
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "refreshParent();", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}