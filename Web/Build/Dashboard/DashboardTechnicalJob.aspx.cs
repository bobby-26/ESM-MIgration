using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Dashboard_DashboardTechnicalJob : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("../Dashboard/DashboardTechnicalJob.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbarmain.AddLinkButton("javascript:showDialog();", "Create Work Order", "CREATE", ToolBarDirection.Right);
            MenuWorkOrder.AccessRights = this.ViewState;
            MenuWorkOrder.MenuList = toolbarmain.Show();

            PhoenixToolbar toolWoCreate = new PhoenixToolbar();
            toolWoCreate.AddButton("Save", "SAVE", ToolBarDirection.Right);

            menuWorkorderCreate.MenuList = toolWoCreate.Show();

            if (!IsPostBack)
            {
                ViewState["RESP"] = string.Empty;
                ViewState["CATEGORY"] = string.Empty;
                ViewState["JOBCATEGORY"] = string.Empty;
                ViewState["FREQUENCY"] = string.Empty;
                ViewState["FREQUENCYTYPE"] = string.Empty;
                ViewState["CNO"] = string.Empty;
                ViewState["CNAME"] = string.Empty;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CODE"] = string.Empty;
                if (Request.QueryString["code"] != null)
                {
                    ViewState["CODE"] = Request.QueryString["code"];
                }
                ViewState["VESSELNAME"] = string.Empty;
                if (Request.QueryString["vslname"] != null)
                {
                    ViewState["VESSELNAME"] = Request.QueryString["vslname"];
                    gvWorkOrder.MasterTableView.GetColumn("FLDVESSELNAME").CurrentFilterValue = ViewState["VESSELNAME"].ToString();
                }
                
               
                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if(PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    gvWorkOrder.MasterTableView.GetColumn("FLDVESSELNAME").Visible = false;
                }
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
            NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
                nvc["VesselList"] = string.Empty;
                nvc["FleetList"] = string.Empty;
                nvc["Owner"] = string.Empty;
                nvc["VesselTypeList"] = string.Empty;
                nvc["RankList"] = string.Empty;
            }
            string vesselId = "";
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();

            DataTable dt = PhoenixDashboardTechnical.DashboardPMSJobSearch(ViewState["CODE"].ToString(), General.GetNullableString(nvc["VesselList"])
                                                               , General.GetNullableString(nvc["VesselTypeList"])
                                                               , General.GetNullableString(nvc["FleetList"])
                                                               , General.GetNullableInteger(nvc["Owner"])
                                                               , ViewState["VESSELNAME"].ToString()
                                                               , sortexpression, sortdirection
                                                               , 1
                                                               , iRowCount
                                                               , ref iRowCount
                                                               , ref iTotalPageCount
                                                               , General.GetNullableInteger(vesselId)
                                                               , General.GetNullableInteger(ViewState["RESP"].ToString())
                                                               , General.GetNullableInteger(ViewState["CATEGORY"].ToString())
                                                               , General.GetNullableInteger(ViewState["FREQUENCYTYPE"].ToString())
                                                               , General.GetNullableInteger(ViewState["FREQUENCY"].ToString())
                                                               , General.GetNullableInteger(ViewState["JOBCATEGORY"].ToString())
                                                               , ViewState["CNO"].ToString()                                                                
                                                               , ViewState["CNAME"].ToString());

            General.ShowExcel("Jobs", dt, alColumns, alCaptions, sortdirection, sortexpression);

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
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = FilterDashboard.OfficeDashboardFilterCriteria;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
                nvc["VesselList"] = string.Empty;
                nvc["FleetList"] = string.Empty;
                nvc["Owner"] = string.Empty;
                nvc["VesselTypeList"] = string.Empty;
                nvc["RankList"] = string.Empty;
            }
            string vesselId = "";
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            DataTable dt = PhoenixDashboardTechnical.DashboardPMSJobSearch(ViewState["CODE"].ToString(), General.GetNullableString(nvc["VesselList"])
                                                                , General.GetNullableString(nvc["VesselTypeList"])
                                                                , General.GetNullableString(nvc["FleetList"])
                                                                , General.GetNullableInteger(nvc["Owner"])
                                                                , ViewState["VESSELNAME"].ToString()
                                                                , sortexpression, sortdirection
                                                                , gvWorkOrder.CurrentPageIndex + 1
                                                                , gvWorkOrder.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount
                                                                , General.GetNullableInteger(vesselId)
                                                                , General.GetNullableInteger(ViewState["RESP"].ToString())
                                                                , General.GetNullableInteger(ViewState["CATEGORY"].ToString())
                                                                , General.GetNullableInteger(ViewState["FREQUENCYTYPE"].ToString())
                                                                , General.GetNullableInteger(ViewState["FREQUENCY"].ToString())
                                                                , General.GetNullableInteger(ViewState["JOBCATEGORY"].ToString())
                                                                , ViewState["CNO"].ToString()
                                                                , ViewState["CNAME"].ToString());

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
    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                grid.CurrentPageIndex = 0;

                ViewState["VESSELNAME"] = grid.MasterTableView.GetColumn("FLDVESSELNAME").CurrentFilterValue;               
                ViewState["CNO"] = gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue;
                ViewState["CNAME"] = gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue;
                ViewState["CATEGORY"] = string.Empty;
                ViewState["RESP"] = grid.MasterTableView.GetColumn("FLDPLANNINGDISCIPLINE").CurrentFilterValue;
                ViewState["JOBCATEGORY"] = grid.MasterTableView.GetColumn("FLDJOBCATEGORY").CurrentFilterValue;
                string freqfilter = gvWorkOrder.MasterTableView.GetColumn("FLDFREQUENCYTYPE").CurrentFilterValue.ToString();
                if (freqfilter != "")
                {
                    ViewState["FREQUENCY"] = freqfilter.Split('~')[0];
                    ViewState["FREQUENCYTYPE"] = freqfilter.Split('~')[1];
                }
                //grid.Rebind();
            }
            else if (e.CommandName.ToUpper() == "FIND")
            {
                GridDataItem item = (GridDataItem)e.Item;
                gvWorkOrder.CurrentPageIndex = 0;
                ViewState["RESP"] = ((RadLabel)item.FindControl("lblRespId")).Text;
                ViewState["CATEGORY"] = ((RadLabel)item.FindControl("lblCategoryId")).Text;
                ViewState["JOBCATEGORY"] = gvWorkOrder.MasterTableView.GetColumn("FLDJOBCATEGORY").CurrentFilterValue;
                grid.Rebind();
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

    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;           

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["RESP"] = string.Empty;
                ViewState["CATEGORY"] = string.Empty;
                ViewState["JOBCATEGORY"] = string.Empty;
                ViewState["FREQUENCY"] = string.Empty;
                ViewState["FREQUENCYTYPE"] = string.Empty;
                ViewState["CNO"] = string.Empty;
                ViewState["CNAME"] = string.Empty;
                gvWorkOrder.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
    protected void cblFrequencyType_DataBinding(object sender, EventArgs e)
    {
        RadComboBox frequency = sender as RadComboBox;
        frequency.DataSource = PhoenixRegistersHard.ListHard(1, 7);
        frequency.DataTextField = "FLDHARDNAME";
        frequency.DataValueField = "FLDHARDCODE";

        frequency.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
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
            if (lnkpostpone != null)
            {
                lnkpostpone.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','Postpone','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRA.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "'); return false;");
            }
            LinkButton lnkTitle = (LinkButton)e.Item.FindControl("lnktitle");
            if (lnkTitle != null)
            {
                lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGeneral.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "'); return false;");
            }
            ImageButton au = (ImageButton)e.Item.FindControl("cmdView");
            if (au != null)
            {
                au.Visible = SessionUtil.CanAccess(this.ViewState, au.CommandName);
                string workorderid = ((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text;
                int vesselid = Convert.ToInt16(((RadLabel)e.Item.FindControl("lblVesselId")).Text);

                au.Attributes.Add("onclick", "javascript:openNewWindow('jobaudit','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?WORKORDERID=" + workorderid + "&vesselid=" + vesselid + "'); return false;");
                if (!ViewState["CODE"].ToString().Equals("TECH-PMS-OPJO"))
                    au.Visible = false;
            }
        }
    }
    private string GetSelectedJobList()
    {
        StringBuilder strlist = new StringBuilder();
        if (gvWorkOrder.Items.Count > 0)
        {
            foreach (GridDataItem gv in gvWorkOrder.SelectedItems)
            {
                RadLabel lblgroupid = (RadLabel)gv.FindControl("lblGroupId");

                if (!lblgroupid.Text.Trim().Equals("")) continue;
                RadLabel lblworkorId = (RadLabel)gv.FindControl("lblWorkOrderId");
                strlist.Append(lblworkorId.Text + ",");
            }
        }
        return strlist.ToString();
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
}
