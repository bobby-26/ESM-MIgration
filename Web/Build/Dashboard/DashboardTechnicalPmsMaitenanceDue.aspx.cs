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

public partial class DashboardTechnicalPmsMaitenanceDue : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);                        

            if (!IsPostBack)
            {                
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;               
                ViewState["CATEGORY"] = Request.QueryString["cc"] ?? string.Empty;
                ViewState["FROM"] = Request.QueryString["frm"] ?? string.Empty;
                ViewState["GROUPID"] = Request.QueryString["groupId"] ?? string.Empty;
                if (Request.QueryString["JobNotPlan"] != null)
                    SetFilter("JOBNOTPLAN", Request.QueryString["JobNotPlan"]);
                if (Request.QueryString["resp"] != null)
                    SetFilter("ucRank", Request.QueryString["resp"]);
                if (Request.QueryString["d"] != null)
                    SetFilter("DUE", Request.QueryString["d"].Replace("D", "").Trim());
                if (Request.QueryString["iscr"] != null)
                    SetFilter("ISCRITICAL", "1");
                if (Request.QueryString["p"] != null)
                    SetFilter("txtPriority", Request.QueryString["p"]);
                if (Request.QueryString["jcat"] != null)
                    SetFilter("JCATNAME", Request.QueryString["jcat"]);
                if (Request.QueryString["jcls"] != null)
                    SetFilter("jobclass", Request.QueryString["jcls"]);
                //ViewState["DAYS"] = ViewState["DUE"].ToString() == "0"? string.Empty : ViewState["DUE"].ToString();
                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                txtDueDate.SelectedDate = DateTime.Now;                
                //txtDueDate.MinDate = DateTime.Now;
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddFontAwesomeButton("../Dashboard/DashboardTechnicalPmsMaitenanceDue.aspx?"+Request.QueryString.ToString(), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarmain.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbarmain.AddFontAwesomeButton("../Dashboard/DashboardTechnicalPmsMaitenanceDue.aspx?" + Request.QueryString.ToString(), "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbarmain.AddFontAwesomeButton("../Dashboard/DashboardTechnicalPmsMaitenanceDue.aspx?" + Request.QueryString.ToString(), "Search", "<i class=\"fas fa-search\"></i>", "FIND");
            if(ViewState["FROM"].ToString().ToUpper().Equals("WOJOB"))
            {
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            else
            { 
                toolbarmain.AddLinkButton("javascript:showDialog();", "Create Work Order", "CREATE", ToolBarDirection.Right);
            }
            MenuWorkOrder.AccessRights = this.ViewState;
            MenuWorkOrder.MenuList = toolbarmain.Show();

            PhoenixToolbar toolWoCreate = new PhoenixToolbar();
            toolWoCreate.AddButton("Save", "SAVE", ToolBarDirection.Right);
            menuWorkorderCreate.AccessRights = this.ViewState;
            menuWorkorderCreate.MenuList = toolWoCreate.Show();
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
            string[] alColumns = { "FLDVESSELNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDWORKORDERNAME", "FLDJOBCATEGORY", "FLDFREQUENCYNAME", "FLDDISCIPLINENAME", "FLDDUEDATE", "FLDLASTDONEDATE", "FLDWORKORDERSTATUS", "FLDPLANINGPRIORITY", "FLDWORKORDERGROUPNO" };
            string[] alCaptions = { "Vessel", "Component Number", "Component Name", "Job Code & Title", "Job Category", "Maintenance Interval", "Responsibility", "Due On", "Last Done On", "Status", "Priority", "Work Order No." };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            NameValueCollection nvc = Filter.CurrentWorkOrderFilter;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
            }
            int? vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            if(vesselid == 0)
            {
                vesselid = null;
            }
            DataTable dt = PhoenixDashboardTechnical.DashboardMaitenanceDue(vesselid
                    , null
                    , General.GetNullableInteger(nvc["JCATNAME"] ?? null)
                    , General.GetNullableInteger(nvc["DUE"] ?? null)
                    , General.GetNullableInteger(nvc["ucRank"] ?? null)
                    , General.GetNullableInteger(nvc["CATEGORY"] ?? null)
                    , nvc["txtComponentNumber"] ?? null
                    , nvc["txtWorkOrderName"] ?? null
                    , nvc["txtComponentName"] ?? null
                    , null
                    , null
                    , (byte?)General.GetNullableInteger(nvc["ISCRITICAL"] ?? null)
                    , sortexpression, sortdirection
                    , 1
                    , iRowCount
                    , ref iRowCount
                    , ref iTotalPageCount
                    , null
                    , (byte?)General.GetNullableInteger(nvc["JOBNOTPLAN"] ?? null)
                    , General.GetNullableInteger(nvc["FREQUENCYTYPE"] ?? null)
                    , General.GetNullableInteger(nvc["FREQUENCY"] ?? null)
                    , nvc["status"] ?? null
                    , General.GetNullableInteger(nvc["txtPriority"] ?? null)
                    , General.GetNullableInteger(nvc["chkDefect"] ?? null)
                    , nvc["jobclass"] ?? null
                    , nvc["txtClassCode"] ?? null
                    , General.GetNullableInteger(nvc["ucMainType"] ?? null)
                    , General.GetNullableInteger(nvc["ucMaintClass"] ?? null)
                    , General.GetNullableInteger(nvc["ucMainCause"] ?? null)                    
                   );
            string heading = Request.QueryString["title"];
            if (string.IsNullOrEmpty(heading))
            {
                heading = "Maintenance Due";
            }
            General.ShowExcel(heading, dt, alColumns, alCaptions, sortdirection, sortexpression);

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
            gvWorkOrder.Rebind();
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

            string[] alColumns = { "FLDVESSELNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDWORKORDERNAME", "FLDJOBCATEGORY", "FLDFREQUENCYNAME", "FLDDISCIPLINENAME", "FLDDUEDATE", "FLDLASTDONEDATE", "FLDWORKORDERSTATUS", "FLDPLANINGPRIORITY", "FLDWORKORDERGROUPNO" };
            string[] alCaptions = { "Vessel", "Component Number", "Component Name", "Job Code & Title", "Job Category", "Maintenance Interval", "Responsibility", "Due On", "Last Done On", "Status", "Priority", "Work Order No." };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentWorkOrderFilter;
            if (nvc == null)
            {
                nvc = new NameValueCollection();
            }
            int? vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            if (vesselid == 0)
            {
                gvWorkOrder.MasterTableView.GetColumn("FLDVESSELNAME").Visible = true;
                vesselid = null;
            }
            DataTable dt = PhoenixDashboardTechnical.DashboardMaitenanceDue(vesselid
                    , null
                    , General.GetNullableInteger(nvc["JCATNAME"] ?? null)
                    , General.GetNullableInteger(nvc["DUE"] ?? null)
                    , General.GetNullableInteger(nvc["ucRank"] ?? null)
                    , General.GetNullableInteger(nvc["CATEGORY"] ?? null)
                    , nvc["txtComponentNumber"] ?? null
                    , nvc["txtWorkOrderName"] ?? null
                    , nvc["txtComponentName"] ?? null
                    , null
                    , null
                    , (byte?)General.GetNullableInteger(nvc["ISCRITICAL"] ?? null)
                    , sortexpression, sortdirection
                    , gvWorkOrder.CurrentPageIndex + 1
                    , gvWorkOrder.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , null
                    , (byte?)General.GetNullableInteger(nvc["JOBNOTPLAN"] ?? null)
                    , General.GetNullableInteger(nvc["FREQUENCYTYPE"] ?? null)
                    , General.GetNullableInteger(nvc["FREQUENCY"] ?? null)
                    , nvc["status"] ?? null
                    , General.GetNullableInteger(nvc["txtPriority"] ?? null)
                    , General.GetNullableInteger(nvc["chkDefect"] ?? null)
                    , nvc["jobclass"] ?? null
                    , nvc["txtClassCode"] ?? null
                    , General.GetNullableInteger(nvc["ucMainType"] ?? null)
                    , General.GetNullableInteger(nvc["ucMaintClass"] ?? null)
                    , General.GetNullableInteger(nvc["ucMainCause"] ?? null)
                    );

            DataSet ds = new DataSet();
            ds.Merge(dt);
            string heading = Request.QueryString["title"];
            if (string.IsNullOrEmpty(heading))
            {
                heading = "Maintenance Due";
            }
            General.SetPrintOptions("gvWorkOrder", heading, alCaptions, alColumns, ds);

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
    protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.Item is GridFilteringItem)
        {
            grid.MasterTableView.GetColumn("FLDISCRITICAL").CurrentFilterValue = GetFilter("ISCRITICAL");
            grid.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue = GetFilter("txtComponentNumber");
            grid.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue = GetFilter("txtComponentName");
            grid.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue = GetFilter("txtWorkOrderName");

            grid.MasterTableView.GetColumn("FLDJOBCATEGORY").CurrentFilterValue = GetFilter("JCATNAME");
            string type = GetFilter("FREQUENCYTYPE");
            grid.MasterTableView.GetColumn("FLDFREQUENCYTYPE").CurrentFilterValue = GetFilter("FREQUENCY") + '~' + GetFilter("FREQUENCYTYPE");
            grid.MasterTableView.GetColumn("FLDPLANNINGDISCIPLINE").CurrentFilterValue = GetFilter("ucRank");
            grid.MasterTableView.GetColumn("FLDDUEDATE").CurrentFilterValue = GetFilter("DUE");
            grid.MasterTableView.GetColumn("FLDISJOBPLANNED").CurrentFilterValue = GetFilter("JOBNOTPLAN");
        }

        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (drv["FLDWORKORDERSTATUSCODE"].ToString().ToUpper() == drv["FLDSECONDPOSTPONESTATUS"].ToString() || drv["FLDWORKORDERSTATUSCODE"].ToString() == drv["FLDPOSTPONESTATUS"].ToString())
            {
                //item.BackColor = System.Drawing.Color.Yellow;
                item.Attributes["style"] = "background-color: yellow !important;";
            }

            if (drv["FLDWORKORDERGROUPID"].ToString() != "")
            {
                CheckBox checkBox = (CheckBox)item["ClientSelectColumn"].Controls[0];
                checkBox.Enabled = false;
                item.SelectableMode = GridItemSelectableMode.None;
                //item["ClientSelectColumn"].Attributes.Add( = GridItemSelectableMode.None;
            }
            LinkButton lblGroupNo = (LinkButton)e.Item.FindControl("lnkGroupNo");
            if (lblGroupNo != null)
            {
                string vslid = "";
                if(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                {
                    vslid = "&vslid=" + drv["FLDVESSELID"].ToString();
                }
                if (drv["FLDWORKORDERGROUPID"] != null)
                {
                    lblGroupNo.Attributes.Add("onclick", "javascript:openNewWindow('maint','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + drv["FLDWORKORDERGROUPID"] + vslid + "'); return false;");
                }
            }
            LinkButton lnkpostpone = (LinkButton)e.Item.FindControl("cmdReschedule");
            if (lnkpostpone != null)
            {
                lnkpostpone.Attributes.Add("onclick", "javascript:openNewWindow('detail','" + drv["FLDWORKORDERNUMBER"].ToString() + " " + drv["FLDWORKORDERNAME"].ToString().Replace("'", "&lsquo;") + "','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRA.aspx?WORKORDERID=" + drv["FLDWORKORDERID"] + "', false,'760px','350px'); return false;");
            }
            LinkButton lnkTitle = (LinkButton)e.Item.FindControl("lnktitle");
            if (lnkTitle != null)
            {
                string cjid = drv["FLDCOMPONENTJOBID"].ToString();
                if (General.GetNullableGuid(cjid).HasValue && General.GetNullableGuid(cjid).Value != Guid.Empty)
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&Cancelledjob=0'); ");
                else
                    lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderDetail.aspx?WORKORDERID=" + drv["FLDWORKORDERID"].ToString() + "','','1200','600');return false");

                //lnkTitle.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?tv=1&COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&COMPONENTID=" + drv["FLDCOMPONENTID"] + "&hierarchy=1&Cancelledjob=0'); ");
                if (General.GetNullableInteger(drv["FLDISCRITICAL"].ToString()) == 1)
                    lnkTitle.Text = "<font color=red>" + drv["FLDWORKORDERNUMBER"].ToString() + " - " + drv["FLDWORKORDERNAME"].ToString() + "</font>";
                else
                    lnkTitle.Text = drv["FLDWORKORDERNUMBER"].ToString() + " - " + drv["FLDWORKORDERNAME"].ToString();
            }
            LinkButton docking = (LinkButton)e.Item.FindControl("cmdDocking");
            if (docking != null)
            {
                docking.Attributes.Add("onclick", "javascript:openNewWindow('DOCING','Add To Drydock','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWODocking.aspx?woid=" + drv["FLDWORKORDERID"] + "'); return false;");
            }
            ImageButton au = (ImageButton)e.Item.FindControl("cmdView");
            if (au != null)
            {
                au.Visible = SessionUtil.CanAccess(this.ViewState, au.CommandName);
                string workorderid = drv["FLDWORKORDERID"].ToString();
                string vesselid = drv["FLDVESSELID"].ToString();
                au.Attributes.Add("onclick", "javascript:openNewWindow('jobaudit','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderRescheduleDetail.aspx?WORKORDERID=" + workorderid + "&vesselid=" + vesselid + "'); return false;");
                if (drv["FLDPOSTPONECOUNT"].ToString().Equals("0"))
                    au.Visible = false;
            }
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
                NameValueCollection criteria = new NameValueCollection();

                foreach (GridColumn column in gvWorkOrder.MasterTableView.Columns)
                {

                    column.ListOfFilterValues = null; // CheckList values set to null will uncheck all the checkboxes

                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;

                    column.AndCurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.AndCurrentFilterValue = string.Empty;
                }

                ViewState["CATEGORY"] = ((RadLabel)item.FindControl("lblCategoryId")).Text;
                criteria.Add("txtWorkOrderName", string.Empty);
                criteria.Add("txtComponentNumber", string.Empty);
                criteria.Add("txtComponentName", string.Empty);
                criteria.Add("ucRank", ((RadLabel)item.FindControl("lblRespId")).Text);
                criteria.Add("CATEGORY", ((RadLabel)item.FindControl("lblCategoryId")).Text);
                criteria.Add("JCATNAME", string.Empty);
                criteria.Add("txtDateFrom", string.Empty);
                criteria.Add("txtDateTo", string.Empty);
                criteria.Add("txtPriority", string.Empty);
                criteria.Add("chkDefect", string.Empty);
                criteria.Add("ISCRITICAL", string.Empty);
                criteria.Add("status", string.Empty);
                criteria.Add("FREQUENCY", string.Empty);
                criteria.Add("FREQUENCYTYPE", string.Empty);

                Filter.CurrentWorkOrderFilter = criteria;
                gvWorkOrder.Rebind();
            }            
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                NameValueCollection criteria = new NameValueCollection();
               
                gvWorkOrder.CurrentPageIndex = 0;
                
                criteria.Add("txtWorkOrderName", gvWorkOrder.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue);
                criteria.Add("txtComponentNumber", gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue);
                criteria.Add("txtComponentName", gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue);
                criteria.Add("ucRank", gvWorkOrder.MasterTableView.GetColumn("FLDPLANNINGDISCIPLINE").CurrentFilterValue);
                criteria.Add("JCATNAME", gvWorkOrder.MasterTableView.GetColumn("FLDJOBCATEGORY").CurrentFilterValue);

                string freqfilter = gvWorkOrder.MasterTableView.GetColumn("FLDFREQUENCYTYPE").CurrentFilterValue.ToString();
                if (freqfilter != "")
                {
                    
                    criteria.Add("FREQUENCY", freqfilter.Split('~')[0]);
                    criteria.Add("FREQUENCYTYPE", freqfilter.Split('~')[1]);
                }
                
                
                criteria.Add("CATEGORY", string.Empty);
                //string daterange = gvWorkOrder.MasterTableView.GetColumn("FLDDUEDATE").CurrentFilterValue;
                //string[] dates = daterange.Split('~');

                //criteria.Add("txtDateFrom", (dates.Length > 0 ? dates[0] : string.Empty));
                //criteria.Add("txtDateTo", (dates.Length > 1 ? dates[1] : string.Empty));
                //DateTime? frmDate = General.GetNullableDateTime(criteria["txtDateFrom"].ToString());
                //DateTime? toDate = General.GetNullableDateTime(criteria["txtDateTo"].ToString());
                //if(frmDate.HasValue && toDate.HasValue)
                //{                    
                //    criteria.Add("DUE", (toDate.Value - frmDate.Value).TotalDays.ToString());
                //    if (criteria["DUE"].ToString() == "0")
                //    {                        
                //        criteria.Add("txtDateFrom", string.Empty);
                //        criteria.Add("txtDateTo", string.Empty);
                //    }
                //}
                //else if(dates.Length > 1)           // only in due fitler change
                //{

                //    criteria.Add("DUE", string.Empty);
                //}
                criteria.Add("DUE", gvWorkOrder.MasterTableView.GetColumn("FLDDUEDATE").CurrentFilterValue);
                criteria.Add("txtPriority", gvWorkOrder.MasterTableView.GetColumn("FLDPLANINGPRIORITY").CurrentFilterValue);
                criteria.Add("chkDefect", gvWorkOrder.MasterTableView.GetColumn("FLDDEFECT").CurrentFilterValue);
                criteria.Add("ISCRITICAL", gvWorkOrder.MasterTableView.GetColumn("FLDISCRITICAL").CurrentFilterValue);
                criteria.Add("status", gvWorkOrder.MasterTableView.GetColumn("FLDWORKORDERSTATUS").CurrentFilterValue);
                Filter.CurrentWorkOrderFilter = criteria;                
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
            if (CommandName.ToUpper() == "FIND")
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderFilter.aspx?frm=md");
            }
            else if (CommandName.ToUpper().Equals("CREATE"))
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
            else if (CommandName.ToUpper().Equals("SAVE"))
            {
                string csvjobList = GetSelectedJobList();
                if (csvjobList.Trim().Equals(""))
                {
                    ucError.ErrorMessage = "Select atleast one job";
                    ucError.Visible = true;
                    return;
                }
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                {
                    ucError.ErrorMessage = "Cannot add job in office context";
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceWorkOrderGroup.InsertSubJob(new Guid(ViewState["GROUPID"].ToString()),csvjobList,PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                            "BookMarkScript", "top.closeTelerikWindow('md','dsd')", true);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                gvWorkOrder.CurrentPageIndex = 0;                
                Filter.CurrentWorkOrderFilter = null;
                foreach (GridColumn column in gvWorkOrder.MasterTableView.Columns)
                {

                    column.ListOfFilterValues = null; // CheckList values set to null will uncheck all the checkboxes

                    column.CurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.CurrentFilterValue = string.Empty;

                    column.AndCurrentFilterFunction = GridKnownFunction.NoFilter;
                    column.AndCurrentFilterValue = string.Empty;
                }
                gvWorkOrder.MasterTableView.FilterExpression = string.Empty;
                gvWorkOrder.MasterTableView.Rebind();
            }
            if (CommandName.ToUpper() == "EXCEL")
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

    protected void btnCreateWO_Click(object sender, EventArgs e)
    {

    }   
    protected void ddlResponsibility_DataBinding(object sender, EventArgs e)
    {
        RadComboBox ddlDiscipline = sender as RadComboBox;
        ddlDiscipline.DataSource = PhoenixRegistersDiscipline.ListDiscipline();
        ddlDiscipline.DataTextField = "FLDDISCIPLINENAME";
        ddlDiscipline.DataValueField = "FLDDISCIPLINEID";
        ddlDiscipline.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
        ddlDiscipline.SelectedValue = GetFilter("ucRank");
    }

    protected void ddlJobCategory_DataBinding(object sender, EventArgs e)
    {
        RadComboBox jobCategory = sender as RadComboBox;
        jobCategory.DataSource = PhoenixRegistersQuick.ListQuick(1, 165);
        jobCategory.DataTextField = "FLDQUICKNAME";
        jobCategory.DataValueField = "FLDQUICKCODE";

        jobCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }
    
    protected void chkIsCritical_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox chk = (RadCheckBox)sender;
        string iscritical = chk.Checked.HasValue && chk.Checked.Value ? "1" : string.Empty;
        NameValueCollection nvc = Filter.CurrentWorkOrderFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection
            {
                { "ISCRITICAL", iscritical }
            };
        }
        else
        {
            nvc["ISCRITICAL"] = iscritical;
        }        
        Filter.CurrentWorkOrderFilter = nvc;
        gvWorkOrder.DataSource = null;
        gvWorkOrder.MasterTableView.Rebind();
    }
    protected void ChkNotPlanned_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox chk = (RadCheckBox)sender;        
        string jobnotplaned = chk.Checked.HasValue && chk.Checked.Value ? "1" : string.Empty;
        NameValueCollection nvc = Filter.CurrentWorkOrderFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection
            {
                { "JOBNOTPLAN", jobnotplaned }
            };
        }
        else
        {
            nvc["JOBNOTPLAN"] = jobnotplaned;
        }
        Filter.CurrentWorkOrderFilter = nvc;
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
        frequency.Items.Add(new RadComboBoxItem("Hours", "-1"));
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
                    PhoenixPlannedMaintenanceWorkOrderGroup.ValidateJob(csvjobList, PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableInteger(ddlResponsible.SelectedDiscipline).Value);
                    int isUnplanned = int.Parse(rblPlannedJob.SelectedValue);
                    PhoenixPlannedMaintenanceWorkOrderGroup.GroupCreate(csvjobList, null, ref groupId, isUnplanned, txtDueDate.SelectedDate, txtTitle.Text, General.GetNullableInteger(ddlResponsible.SelectedDiscipline));
                    ViewState["GROUPID"] = groupId.ToString();
                    PhoenixPlannedMaintenanceWorkOrderGroup.RefreshToolboxMeet(new Guid(ViewState["GROUPID"].ToString()));
                    if (Request.QueryString["frm"] != null && Request.QueryString["frm"] == "wo")
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                            "BookMarkScript", "top.closeTelerikWindow('md','dsd')", true);
                    }
                    else
                    {
                        Response.Redirect("../PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + ViewState["GROUPID"]);
                    }
                }
                catch (Exception ex)
                {
                    RequiredFieldValidator Validator = new RequiredFieldValidator
                    {
                        ErrorMessage = "* " + ex.Message,
                        //Validator.ValidationGroup = "Group1";
                        IsValid = false,
                        Visible = false
                    };
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
    protected string GetFilter(string filter)
    {
        string value = string.Empty;
        NameValueCollection nvc = Filter.CurrentWorkOrderFilter;
        if (nvc != null)
        {
            value = nvc[filter];
        }
        return value;
    }
    protected void SetFilter(string key, string value)
    {        
        NameValueCollection nvc = Filter.CurrentWorkOrderFilter;
        if (nvc == null)
        {
            nvc = new NameValueCollection
            {
                { key, value }
            };
        }
       else
        {
            nvc[key] = value;
        }
        Filter.CurrentWorkOrderFilter = nvc;
    }

    protected void gvWorkOrder_PreRender(object sender, EventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (!IsPostBack)
        {            
            grid.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue = GetFilter("txtComponentNumber");
            grid.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue = GetFilter("txtComponentName");
            grid.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue = GetFilter("txtWorkOrderName");
            grid.MasterTableView.GetColumn("FLDPLANINGPRIORITY").CurrentFilterValue = GetFilter("txtPriority");
            grid.Rebind();
        }
    }
}
