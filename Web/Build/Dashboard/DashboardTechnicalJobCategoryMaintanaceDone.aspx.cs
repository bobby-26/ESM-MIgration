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

public partial class DashboardTechnicalJobCategoryMaintanaceDone : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("../Dashboard/DashboardTechnicalJobCategoryMaintanaceDone.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            //toolbarmain.AddLinkButton("javascript:showDialog();", "Create Work Order", "CREATE", ToolBarDirection.Right);
            MenuWorkOrder.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["RESP"] = string.Empty;
                ViewState["CNO"] = string.Empty;
                ViewState["WONAME"] = string.Empty;
                ViewState["CNAME"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                ViewState["MDUE"] = string.Empty;
                ViewState["JCATNAME"] = string.Empty;
                ViewState["DUE"] = string.Empty;
                ViewState["JCAT"] = Request.QueryString["jc"] == null ? string.Empty : Request.QueryString["jc"];
                ViewState["CATEGORY"] = Request.QueryString["cc"] == null ? string.Empty : Request.QueryString["cc"];
                ViewState["ISCRITICAL"] = Request.QueryString["iscr"] == null ? string.Empty : Request.QueryString["iscr"];
                if (Request.QueryString["resp"] != null)
                    ViewState["RESP"] = Request.QueryString["resp"];
                if (Request.QueryString["d"] != null)
                    ViewState["DUE"] = Request.QueryString["d"].Replace("D", "").Trim();
                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                txtDueDate.SelectedDate = DateTime.Now;
                //txtDueDate.MinDate = DateTime.Now;
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
            //int iTotalPageCount = 0;
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

            DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.ReportMaintenanceDone(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , ViewState["CNO"].ToString()
                    , ViewState["CNAME"].ToString()
                    , null
                    , General.GetNullableDateTime(ViewState["FDATE"].ToString())
                    , General.GetNullableDateTime(ViewState["TDATE"].ToString())
                    , null
                    , null
                    , null
                    , null
                    , null
                    , null
                    , null
                    , null
                    , null
                    , null
                    , null);

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
            //int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixPlannedMaintenanceWorkOrderReport.ReportMaintenanceDone(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , ViewState["CNO"].ToString()
                    , ViewState["CNAME"].ToString()
                    , null
                    , General.GetNullableDateTime(ViewState["FDATE"].ToString())
                    , General.GetNullableDateTime(ViewState["TDATE"].ToString())
                    , null
                    , null
                    , null
                    , null
                    , null
                    , null
                    , null
                    , null
                    , null
                    , null
                    , null);
                  

            gvWorkOrder.DataSource = ds.Tables[0];
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
    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper() == "FIND")
            {
                GridDataItem item = (GridDataItem)e.Item;
                ViewState["PAGENUMBER"] = "1";
                ViewState["RESP"] = ((RadLabel)item.FindControl("lblRespId")).Text;
                ViewState["CATEGORY"] = ((RadLabel)item.FindControl("lblCategoryId")).Text;
                ViewState["CNO"] = string.Empty;
                ViewState["WONAME"] = string.Empty;
                ViewState["CNAME"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                ViewState["JCATNAME"] = string.Empty;
                gvWorkOrder.Rebind();
            }
            else if (e.CommandName.ToUpper() == "PAGE")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                Pair filterPair = (Pair)e.CommandArgument;
                string value = filterPair.First.ToString();//accessing function name                
                ViewState["PAGENUMBER"] = "1";
                ViewState["CNO"] = gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue;
                ViewState["CNAME"] = gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue;
                ViewState["WONAME"] = gvWorkOrder.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue;
                ViewState["RESP"] = gvWorkOrder.MasterTableView.GetColumn("FLDPLANNINGDISCIPLINE").CurrentFilterValue;
                ViewState["JCATNAME"] = gvWorkOrder.MasterTableView.GetColumn("FLDJOBCATEGORY").CurrentFilterValue;
                ViewState["CATEGORY"] = string.Empty;
                string daterange = gvWorkOrder.MasterTableView.GetColumn("FLDDUEDATE").CurrentFilterValue;
                string[] dates = daterange.Split('~');
                ViewState["FDATE"] = (dates.Length > 0 ? dates[0] : string.Empty);
                ViewState["TDATE"] = (dates.Length > 1 ? dates[1] : string.Empty);
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
        DateTime? MinDate = null;
        DateTime? temp = null;
        StringBuilder strlist = new StringBuilder();
        if (gvWorkOrder.Items.Count > 0)
        {
            foreach (GridDataItem gv in gvWorkOrder.SelectedItems)
            {
                temp = General.GetNullableDateTime(((RadLabel)gv.FindControl("lblDue")).Text);
                if (!MinDate.HasValue)
                    MinDate = temp;
                else if (DateTime.Compare(MinDate.Value, temp.Value) == 1)
                {
                    MinDate = temp;
                }
                RadLabel lblworkorId = (RadLabel)gv.FindControl("lblWorkOrderId");
                strlist.Append(lblworkorId.Text + ",");
            }
        }
        ViewState["MDUE"] = MinDate;
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
                //string csvjobList = GetSelectedJobList();
                //if (csvjobList.Trim().Equals(""))
                //{
                //    ucError.ErrorMessage = "Select atleast one job";
                //    ucError.Visible = true;
                //    return;
                //}
                //PhoenixPlannedMaintenanceWorkOrderGroup.GroupCreate(csvjobList, null, ref groupId, 1);
                //ViewState["GROUPID"] = groupId.ToString();
                //Response.Redirect("../PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + ViewState["GROUPID"]);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = "1";
                ViewState["CNO"] = string.Empty;
                ClearGridFilter(gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNUMBER"));

                ViewState["CNAME"] = string.Empty;
                ClearGridFilter(gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNAME"));

                ViewState["WONAME"] = string.Empty;
                ClearGridFilter(gvWorkOrder.MasterTableView.GetColumn("FLDWORKORDERNAME"));

                ViewState["CATEGORY"] = string.Empty;

                ViewState["RESP"] = string.Empty;
                ClearGridFilter(gvWorkOrder.MasterTableView.GetColumn("FLDPLANNINGDISCIPLINE"));

                ViewState["JCATNAME"] = string.Empty;
                ClearGridFilter(gvWorkOrder.MasterTableView.GetColumn("FLDJOBCATEGORY"));

                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                ClearGridFilter(gvWorkOrder.MasterTableView.GetColumn("FLDDUEDATE"));

                gvWorkOrder.Rebind();
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
        DateTime? MinDate = General.GetNullableDateTime(ViewState["MDUE"] == null ? string.Empty : ViewState["MDUE"].ToString());
        if (MinDate.HasValue && txtDueDate.SelectedDate.HasValue && DateTime.Compare(MinDate.Value, txtDueDate.SelectedDate.Value) < 0)
        {
            RequiredFieldValidator Validator = new RequiredFieldValidator();
            Validator.ErrorMessage = "*  Planned date can't be greater than " + General.GetDateTimeToString(MinDate);
            //Validator.ValidationGroup = "Group1";
            Validator.IsValid = false;
            Validator.Visible = false;
            Page.Form.Controls.Add(Validator);
        }
        if (Page.IsValid)
        {
            try
            {
                PhoenixPlannedMaintenanceWorkOrderGroup.GroupCreate(csvjobList, null, ref groupId, 1, txtDueDate.SelectedDate, txtTitle.Text, General.GetNullableInteger(ddlResponsible.SelectedDiscipline));
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
    }
}