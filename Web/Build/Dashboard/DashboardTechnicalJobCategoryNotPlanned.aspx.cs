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

public partial class DashboardTechnicalJobCategoryNotPlanned : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddFontAwesomeButton("../Dashboard/DashboardTechnicalJobCategoryNotPlanned.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbarmain.AddLinkButton("javascript:showDialog();", "Create Work Order", "CREATE", ToolBarDirection.Right);
            MenuWorkOrder.AccessRights = this.ViewState;
            MenuWorkOrder.MenuList = toolbarmain.Show();

            PhoenixToolbar toolWoCreate = new PhoenixToolbar();
            toolWoCreate.AddButton("Save", "SAVE", ToolBarDirection.Right);

            menuWorkorderCreate.MenuList = toolWoCreate.Show();

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
                ViewState["DAYS"] = string.Empty;
                ViewState["JCAT"] = Request.QueryString["jc"] == null ? string.Empty : Request.QueryString["jc"];
                ViewState["CATEGORY"] = Request.QueryString["cc"] == null ? string.Empty : Request.QueryString["cc"];
                ViewState["ISCRITICAL"] = Request.QueryString["iscr"] == null ? string.Empty : Request.QueryString["iscr"];
                ViewState["JOBNOTPLAN"] = Request.QueryString["JobNotPlan"] == null ? string.Empty : Request.QueryString["JobNotPlan"];
                ViewState["FREQUENCYTYPE"] = string.Empty;
                ViewState["FREQUENCY"] = string.Empty;
                ViewState["OVERDUE"] = string.Empty;
                ViewState["STATUS"] = string.Empty;
                ViewState["PRIORITY"] = string.Empty;
                if (Request.QueryString["resp"] != null)
                    ViewState["RESP"] = Request.QueryString["resp"];
                if (Request.QueryString["d"] != null)
                    ViewState["DUE"] = Request.QueryString["d"].Replace("D", "").Trim();
                ViewState["DAYS"] = ViewState["DUE"].ToString() == "0"? string.Empty : ViewState["DUE"].ToString();
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

            DataTable dt = PhoenixDashboardTechnical.DashboardJobCategoryList(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                   , ViewState["JCAT"].ToString()
                   , General.GetNullableInteger(ViewState["JCATNAME"].ToString())
                   , General.GetNullableInteger(ViewState["DUE"].ToString())
                   , General.GetNullableInteger(ViewState["RESP"].ToString())
                   , General.GetNullableInteger(ViewState["CATEGORY"].ToString())
                   , ViewState["CNO"].ToString()
                   , ViewState["WONAME"].ToString()
                   , ViewState["CNAME"].ToString()
                   , null
                   , null
                   , (byte?)General.GetNullableInteger(ViewState["ISCRITICAL"].ToString())
                   , sortexpression, sortdirection
                   , 1
                   , iRowCount
                   , ref iRowCount
                   , ref iTotalPageCount
                   , (byte?)General.GetNullableInteger(ViewState["OVERDUE"].ToString())
                   , (byte?)General.GetNullableInteger(ViewState["JOBNOTPLAN"].ToString())
                   , General.GetNullableInteger(ViewState["FREQUENCYTYPE"].ToString())
                   , General.GetNullableInteger(ViewState["FREQUENCY"].ToString())
                   , ViewState["STATUS"].ToString() == string.Empty ? null : ViewState["STATUS"].ToString()
                   , ViewState["PRIORITY"].ToString()== string.Empty ? null : General.GetNullableInteger(ViewState["PRIORITY"].ToString())
                   );

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

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixDashboardTechnical.DashboardJobCategoryList(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , ViewState["JCAT"].ToString()
                    , General.GetNullableInteger(ViewState["JCATNAME"].ToString())
                    , General.GetNullableInteger(ViewState["DUE"].ToString())
                    , General.GetNullableInteger(ViewState["RESP"].ToString())
                    , General.GetNullableInteger(ViewState["CATEGORY"].ToString())
                    , ViewState["CNO"].ToString()
                    , ViewState["WONAME"].ToString()
                    , ViewState["CNAME"].ToString()
                    , null
                    , null
                    , (byte?)General.GetNullableInteger(ViewState["ISCRITICAL"].ToString())
                    , sortexpression, sortdirection
                    , gvWorkOrder.CurrentPageIndex + 1
                    , gvWorkOrder.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , (byte?)General.GetNullableInteger(ViewState["OVERDUE"].ToString())
                    , (byte?)General.GetNullableInteger(ViewState["JOBNOTPLAN"].ToString())
                    , General.GetNullableInteger(ViewState["FREQUENCYTYPE"].ToString())
                    , General.GetNullableInteger(ViewState["FREQUENCY"].ToString())
                    , ViewState["STATUS"].ToString() == string.Empty ? null : ViewState["STATUS"].ToString()
                    , ViewState["PRIORITY"].ToString() == string.Empty ? null : General.GetNullableInteger(ViewState["PRIORITY"].ToString())
                    );

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
                gvWorkOrder.CurrentPageIndex = 0;
                ViewState["RESP"] = ((RadLabel)item.FindControl("lblRespId")).Text;
                ViewState["CATEGORY"] = ((RadLabel)item.FindControl("lblCategoryId")).Text;
                ViewState["CNO"] = string.Empty;
                ViewState["WONAME"] = string.Empty;
                ViewState["CNAME"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                ViewState["JCATNAME"] = string.Empty;
                ViewState["ISCRITICAL"] = string.Empty;
                ViewState["DAYS"] = string.Empty;
                ViewState["OVERDUE"] = string.Empty;
                ViewState["PRIORITY"] = string.Empty;
                ViewState["STATUS"] = string.Empty;

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
                gvWorkOrder.CurrentPageIndex = 0;
                ViewState["CNO"] = gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue;
                ViewState["CNAME"] = gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue;
                ViewState["WONAME"] = gvWorkOrder.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue;
                ViewState["RESP"] = gvWorkOrder.MasterTableView.GetColumn("FLDPLANNINGDISCIPLINE").CurrentFilterValue;
                ViewState["JCATNAME"] = gvWorkOrder.MasterTableView.GetColumn("FLDJOBCATEGORY").CurrentFilterValue;

                string freqfilter = gvWorkOrder.MasterTableView.GetColumn("FLDFREQUENCYTYPE").CurrentFilterValue.ToString();
                if (freqfilter != "")
                {
                    ViewState["FREQUENCY"] = freqfilter.Split('~')[0];
                    ViewState["FREQUENCYTYPE"] = freqfilter.Split('~')[1];
                }

                //ViewState["FREQUENCYTYPE"] = gvWorkOrder.MasterTableView.GetColumn("FLDFREQUENCYTYPE").CurrentFilterValue; 
                ViewState["CATEGORY"] = string.Empty;
                string daterange = gvWorkOrder.MasterTableView.GetColumn("FLDDUEDATE").CurrentFilterValue;
                string[] dates = daterange.Split('~');
                ViewState["FDATE"] = (dates.Length > 0 ? dates[0] : string.Empty);
                ViewState["TDATE"] = (dates.Length > 1 ? dates[1] : string.Empty);
                DateTime? frmDate = General.GetNullableDateTime(ViewState["FDATE"].ToString());
                DateTime? toDate = General.GetNullableDateTime(ViewState["TDATE"].ToString());
                if(frmDate.HasValue && toDate.HasValue)
                {
                    ViewState["DAYS"] = (toDate.Value - frmDate.Value).TotalDays;
                    ViewState["DUE"] = (toDate.Value - frmDate.Value).TotalDays;

                    if(ViewState["DUE"].ToString() == "0")
                    {
                        ViewState["FDATE"] = string.Empty;
                        ViewState["TDATE"] = string.Empty;
                    }
                }
                else if(dates.Length > 1)           // only in due fitler change
                {
                    ViewState["DAYS"] = string.Empty;
                    ViewState["DUE"] = string.Empty;
                }
                ViewState["ISCRITICAL"] = gvWorkOrder.MasterTableView.GetColumn("FLDISCRITICAL").CurrentFilterValue;
                ViewState["STATUS"] = gvWorkOrder.MasterTableView.GetColumn("FLDWORKORDERSTATUS").CurrentFilterValue;
                ViewState["PRIORITY"] = gvWorkOrder.MasterTableView.GetColumn("FLDPLANINGPRIORITY").CurrentFilterValue;
                //ViewState["OVERDUE"] = gvWorkOrder.MasterTableView.GetColumn("FLDOVERDUE").CurrentFilterValue;
                gvWorkOrder.Rebind();
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
                gvWorkOrder.CurrentPageIndex = 0;
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
                ViewState["DAYS"] = string.Empty;
                ViewState["ISCRITICAL"] = string.Empty;
                ViewState["JOBNOTPLAN"] = string.Empty;
                ViewState["STATUS"] = string.Empty;
                ViewState["PRIORITY"] = string.Empty;
                
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
            if (drv["FLDWORKORDERSTATUS"].ToString().ToUpper() == "2ND POSTPONED" || drv["FLDWORKORDERSTATUS"].ToString().ToUpper() == "POSTPONED")
            {
                //item.BackColor = System.Drawing.Color.Yellow;
                item.Attributes["style"] = "background-color: yellow !important;";
            }

            if (drv["FLDWORKORDERGROUPID"].ToString() != "")
            {
                CheckBox checkBox = (CheckBox)item["ClientSelectColumn"].Controls[0];
                checkBox.Enabled = false;
                //item.SelectableMode = GridItemSelectableMode.None;
                //item["ClientSelectColumn"].Attributes.Add( = GridItemSelectableMode.None;
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
                lnkpostpone.Attributes.Add("onclick", "javascript:openNewWindow('PPLIST','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRA.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "'); return false;");
            }
            LinkButton lnkTitle = (LinkButton)e.Item.FindControl("lnktitle");
            if (lnkTitle != null)
            {
                lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?tv=1&COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&hierarchy=1&Cancelledjob=0'); ");
                if (General.GetNullableInteger(drv["FLDISCRITICAL"].ToString()) == 1)
                    lnkTitle.Text = "<html><font color=red>" + drv["FLDWORKORDERNAME"].ToString() + "</font>";
                else
                    lnkTitle.Text = drv["FLDWORKORDERNAME"].ToString();


            }

        }
    }

    protected void chkIsCritical_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox chk = (RadCheckBox)sender;
        ViewState["ISCRITICAL"] = chk.Checked.HasValue && chk.Checked.Value ? "1" : string.Empty;        
        gvWorkOrder.DataSource = null;
        gvWorkOrder.MasterTableView.Rebind();
    }
    protected void ChkNotPlanned_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox chk = (RadCheckBox)sender;
        ViewState["JOBNOTPLAN"] = chk.Checked.HasValue && chk.Checked.Value ? "1" : string.Empty;
        gvWorkOrder.DataSource = null;
        gvWorkOrder.MasterTableView.Rebind();
    }
    protected void cblFrequencyType_DataBinding(object sender, EventArgs e)
    {
        RadComboBox frequency = sender as RadComboBox;
        frequency.DataSource = PhoenixRegistersHard.ListHard(1, 7);
        frequency.DataTextField = "FLDHARDNAME";
        frequency.DataValueField = "FLDHARDCODE";

        frequency.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }


    protected void menuWorkorderCreate_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            Guid? groupId = Guid.Empty;
            string csvjobList = GetSelectedJobList();
            if (csvjobList.Trim().Equals(""))
            {
                RequiredFieldValidator Validator = new RequiredFieldValidator();
                Validator.ErrorMessage = "Select atleast one job";
                //Validator.ValidationGroup = "Group1";
                Validator.IsValid = false;
                Validator.Visible = false;
                Page.Form.Controls.Add(Validator);
            }

            if (txtTitle.Text == string.Empty)
            {
                RequiredFieldValidator Validator = new RequiredFieldValidator();
                Validator.ErrorMessage = "Title is required";
                //Validator.ValidationGroup = "Group1";
                Validator.IsValid = false;
                Validator.Visible = false;
                Page.Form.Controls.Add(Validator);
            }

            string PlannedDate = txtDueDate.SelectedDate.ToString();

            if (string.IsNullOrEmpty(PlannedDate))
            {
                RequiredFieldValidator Validator = new RequiredFieldValidator();
                Validator.ErrorMessage = "Planned date is required";
                Validator.IsValid = false;
                Validator.Visible = false;
                Page.Form.Controls.Add(Validator);
            }
            if (General.GetNullableInteger(ddlResponsible.SelectedDiscipline) == null)
            {
                RequiredFieldValidator Validator = new RequiredFieldValidator();
                Validator.ErrorMessage = "Assigned To is required";
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
    }


    protected void ddlStatus_DataBinding(object sender, EventArgs e)
    {
        RadComboBox status = sender as RadComboBox;
        status.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 10, 0, "PPP,POP,2PP");
        status.DataTextField = "FLDHARDNAME";
        status.DataValueField = "FLDSHORTNAME";

        status.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        status.Items.Insert(1, new RadComboBoxItem("Overdue", "OVERDUE"));
        status.Items.Insert(2, new RadComboBoxItem("Due", "DUE"));
    }
}
