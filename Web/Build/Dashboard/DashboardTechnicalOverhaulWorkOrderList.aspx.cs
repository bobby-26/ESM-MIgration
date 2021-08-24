using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class DashboardTechnicalOverhaulWorkOrderList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddLinkButton("javascript:showDialog();", "Create Work Order", "CREATE", ToolBarDirection.Right);
            MenuWorkOrder.AccessRights = this.ViewState;
            MenuWorkOrder.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["RESP"] = string.Empty;                
                ViewState["CNO"] = string.Empty;
                ViewState["CNAME"] = string.Empty;
                ViewState["DUE"] = string.Empty;
                ViewState["JCAT"] = string.Empty;
                ViewState["OVERDUE"] = string.Empty;
                ViewState["JOBCLASS"] = string.Empty;
                ViewState["COMPONENTID"] = string.Empty;
                if (Request.QueryString["d"] != null)
                    ViewState["DUE"] = Request.QueryString["d"];
                if (Request.QueryString["jobclass"] != null)
                    ViewState["JOBCLASS"] = Request.QueryString["jobclass"];
                if (Request.QueryString["componentId"] != null)
                    ViewState["COMPONENTID"] = Request.QueryString["componentId"];


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
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME","FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataTable dt = PhoenixDashboardTechnical.DashboardOverhaulWorkorderList(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                   , General.GetNullableInteger(ViewState["JCAT"].ToString())
                   , General.GetNullableInteger(ViewState["DUE"].ToString())
                   , General.GetNullableInteger(ViewState["OVERDUE"].ToString())
                   , General.GetNullableInteger(ViewState["RESP"].ToString())
                   , ViewState["CNO"].ToString()
                   , ViewState["CNAME"].ToString()
                   , sortexpression, sortdirection
                   , 1
                   , iRowCount
                   , ref iRowCount
                   , ref iTotalPageCount
                   , ViewState["JOBCLASS"].ToString()
                   , new Guid(ViewState["COMPONENTID"].ToString()));

            General.ShowExcel("Jobs", dt, alColumns, alCaptions, sortdirection, sortexpression);

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

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME","FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name","Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixDashboardTechnical.DashboardOverhaulWorkorderList(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                   , General.GetNullableInteger(ViewState["JCAT"].ToString())
                   , General.GetNullableInteger(ViewState["DUE"].ToString())
                   , General.GetNullableInteger(ViewState["OVERDUE"].ToString())
                   , General.GetNullableInteger(ViewState["RESP"].ToString())
                   , ViewState["CNO"].ToString()
                   , ViewState["CNAME"].ToString()
                   , sortexpression, sortdirection
                   , gvWorkOrder.CurrentPageIndex + 1
                   , gvWorkOrder.PageSize
                   , ref iRowCount
                   , ref iTotalPageCount
                   , ViewState["JOBCLASS"].ToString()
                   , new Guid(ViewState["COMPONENTID"].ToString()));

            gvWorkOrder.DataSource = dt;
            gvWorkOrder.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;

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
    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper() == "FIND")
            {
                GridDataItem item = (GridDataItem)e.Item;
                gvWorkOrder.CurrentPageIndex = 0;
                ViewState["RESP"] = ((RadLabel)item.FindControl("lblRespId")).Text;
                ViewState["CNO"] = string.Empty;
                ViewState["CNAME"] = string.Empty;
                gvWorkOrder.Rebind();
            }
            if (e.CommandName == RadGrid.FilterCommandName)
            {                
                Pair filterPair = (Pair)e.CommandArgument;
                string value = filterPair.First.ToString();//accessing function name                
                gvWorkOrder.CurrentPageIndex = 0;
                ViewState["RESP"] = gvWorkOrder.MasterTableView.GetColumn("FLDPLANNINGDISCIPLINE").CurrentFilterValue;
                ViewState["JCAT"] = gvWorkOrder.MasterTableView.GetColumn("FLDJOBCATEGORY").CurrentFilterValue;

            }            
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvWorkOrder_SortCommand(object sender, GridSortCommandEventArgs e)
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
    private string GetSelectedJobList()
    {
        StringBuilder strlist = new StringBuilder();
        if (gvWorkOrder.Items.Count > 0)
        {
            foreach (GridDataItem gv in gvWorkOrder.SelectedItems)
            {
                RadLabel lblworkorId = (RadLabel)gv.FindControl("lblWorkOrderId");
                strlist.Append(lblworkorId.Text + ",");
            }
        }
        return strlist.ToString();       
    }

    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;           

            if (CommandName.ToUpper().Equals("CREATE"))
            {
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnCreateWO_Click(object sender, EventArgs e)
    {
        Guid? groupId = Guid.Empty;        
        string csvjobList = GetSelectedJobList();
        if (csvjobList.Trim().Equals(""))
        {                        
            RequiredFieldValidator Validator = new RequiredFieldValidator();
            Validator.ErrorMessage = "* Select atleast one job";
            //Validator.ValidationGroup = "Group1";
            Validator.IsValid = false;
            Validator.Visible = false;
            Page.Form.Controls.Add(Validator);
        }
        if (Page.IsValid)
        {
            try
            {
                int isUnplanned = int.Parse(rblPlannedJob.SelectedValue);
                PhoenixPlannedMaintenanceWorkOrderGroup.GroupCreate(csvjobList, null, ref groupId, isUnplanned, txtDueDate.SelectedDate, txtTitle.Text, General.GetNullableInteger(ddlResponsible.SelectedDiscipline));                
                ViewState["GROUPID"] = groupId.ToString();
                PhoenixPlannedMaintenanceWorkOrderGroup.RefreshToolboxMeet(new Guid(ViewState["GROUPID"].ToString()));
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + ViewState["GROUPID"]);
            }
            catch (Exception ex)
            {
                RequiredFieldValidator Validator = new RequiredFieldValidator();
                Validator.ErrorMessage = "* " + ex.Message;
                //Validator.ValidationGroup = "Group1";
                Validator.IsValid = false;
                Validator.Visible = false;
                Page.Form.Controls.Add(Validator);
            }
        }
    }
    private void ClearGridFilter(GridColumn column)
    {        
        column.ListOfFilterValues = null;
        column.CurrentFilterFunction = GridKnownFunction.NoFilter;
        column.CurrentFilterValue = string.Empty;
    }
    protected void ddlResponsibility_DataBinding(object sender, EventArgs e)
    {
        RadComboBox ddlDiscipline = sender as RadComboBox;
        ddlDiscipline.DataSource = PhoenixRegistersDiscipline.ListDiscipline();
        ddlDiscipline.DataTextField = "FLDDISCIPLINENAME";
        ddlDiscipline.DataValueField = "FLDDISCIPLINEID";
        ddlDiscipline.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ddlDiscipline.SelectedValue = ViewState["RESP"].ToString();
    }

    protected void ddlJobCategory_DataBinding(object sender, EventArgs e)
    {
        RadComboBox jobCategory = sender as RadComboBox;
        jobCategory.DataSource = PhoenixRegistersQuick.ListQuick(1, 165);
        jobCategory.DataTextField = "FLDQUICKNAME";
        jobCategory.DataValueField = "FLDQUICKCODE";

        jobCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }

    protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (drv["FLDWORKORDERGROUPID"].ToString() != "")
            {
                CheckBox checkBox = (CheckBox)item["ClientSelectColumn"].Controls[0];
                checkBox.Enabled = false;
            }
            LinkButton lblGroupNo = (LinkButton)e.Item.FindControl("lnkGroupNo");
            if(lblGroupNo != null)
            {
                if(drv["FLDWORKORDERGROUPID"] != null)
                {
                    lblGroupNo.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId="+ drv["FLDWORKORDERGROUPID"] +"'); return false;");
                }
            }
            LinkButton lnkpostpone = (LinkButton)e.Item.FindControl("cmdReschedule");
            if(lnkpostpone != null)
            {
                lnkpostpone.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "'); return false;");
            }
            LinkButton lnkTitle = (LinkButton)e.Item.FindControl("lnktitle");
            if (lnkTitle != null)
            {
                lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('DETAIL','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGeneral.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "'); return false;");
            }
        }
    }
}
