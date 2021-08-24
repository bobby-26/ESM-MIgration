using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inventory;
using System.Web.UI;
using System.Text;

public partial class PlannedMaintenanceUnplannedWorkOrder : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceUnplannedWorkOrder.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceUnplannedWorkOrder.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");

            menugridMenu.AccessRights = this.ViewState;
            menugridMenu.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Add", "ADD", ToolBarDirection.Right);
                //toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
                MenuWorkOrderRequestion.MenuList = toolbarmain.Show();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["COMPONENTID"] = null;
                ViewState["WORKORDERID"] = null;
                ViewState["groupId"] = null;

                if (Request.QueryString["groupId"] != null)
                    ViewState["groupId"] = Request.QueryString["groupId"].ToString();

                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds;
            ds = PhoenixPlannedMaintenanceWorkOrderGroup.UnplannedWorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null
                    , General.GetNullableDateTime(ucFromDAte.Text), General.GetNullableDateTime(ucToDAte.Text)
                    , null, General.GetNullableInteger(ucJobCategory.SelectedValue)
                    , General.GetNullableInteger(ucDiscipline.SelectedDiscipline)
                    , General.GetNullableInteger(ucCompCategory.SelectedValue.ToString())
                    , sortexpression, sortdirection
                    , int.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvWorkOrder.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount,null,null,1);

            General.ShowExcel("Jobs", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            ds = PhoenixPlannedMaintenanceWorkOrderGroup.UnplannedWorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null
                    , General.GetNullableDateTime(ucFromDAte.Text), General.GetNullableDateTime(ucToDAte.Text)
                    , null,General.GetNullableInteger(ucJobCategory.SelectedValue)
                    , General.GetNullableInteger(ucDiscipline.SelectedDiscipline)
                    , General.GetNullableInteger(ucCompCategory.SelectedValue)
                    , sortexpression, sortdirection
                    , int.Parse(ViewState["PAGENUMBER"].ToString())
                    , gvWorkOrder.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount,null,null,1);

            General.SetPrintOptions("gvWorkOrder", "Work Order", alCaptions, alColumns, ds);

            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
     
    protected void cmdSearch_Click(object sender, EventArgs e)
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

    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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
    protected void gvWorkOrder_SortCommand(object sender, GridSortCommandEventArgs e)
    {

    }
    private string GetSelectedScheduleList()
    {
        StringBuilder strlist = new StringBuilder();
        if (gvWorkOrder.Items.Count > 0)
        {
            foreach (GridDataItem gv in gvWorkOrder.Items)
            {
                CheckBox ChkPlan = (CheckBox)gv["ClientSelectColumn"].Controls[0];
                //CheckBox ChkPlan = (CheckBox)gv.FindControl("ChkPlan");

                if (ChkPlan.Checked && ChkPlan.Enabled)
                {
                    RadLabel lblworkorId = (RadLabel)gv.FindControl("lblWorkOrderId");
                    if (!string.IsNullOrEmpty(lblworkorId.Text))
                    {
                        strlist.Append(lblworkorId.Text);
                        strlist.Append(",");
                    }
                }
            }
        }
        return strlist.ToString();
    }



    protected void MenuWorkOrderRequestion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? groupId = Guid.Empty;

            if (CommandName.ToUpper().Equals("ADD"))
            {
                string csvScheduleList = GetSelectedScheduleList();
                if (csvScheduleList.Trim().Equals(""))
                {
                    ucError.ErrorMessage = "Select atleast one job";
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["groupId"] != null)
                {
                    PhoenixPlannedMaintenanceWorkOrderGroup.GroupUpdate(csvScheduleList, new Guid(ViewState["groupId"].ToString()));
                }
                else
                {
                    PhoenixPlannedMaintenanceWorkOrderGroup.GroupCreate(csvScheduleList, null, ref groupId, 0);
                    ViewState["groupId"] = groupId.ToString();
                    PhoenixPlannedMaintenanceWorkOrderGroup.RefreshToolboxMeet(new Guid(ViewState["groupId"].ToString()));
                }
                ucStatus.Text = "Jobs Added successfully";
                BindData();
                gvWorkOrder.Rebind();

                var Frame = string.IsNullOrEmpty(Request.QueryString["ifr"]) ? "null" : "'ifMoreInfo'";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1'," + Frame + ", true);", true);

            }
            if(CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["groupId"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }
    protected void menugridMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            Guid? groupId = Guid.Empty;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
                gvWorkOrder.Rebind();
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

}
