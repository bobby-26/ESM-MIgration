using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderGroupList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('md','Jobs','" + Session["sitepath"] + "/Dashboard/DashboardTechnicalPmsMaitenanceDue.aspx?frm=wo');", "Create WO", "<i class=\"fas fa-plus-circle\"></i>", "CREATE");

            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();
            ResetMenu();
            if (!IsPostBack)
            {
                if ((ConfigurationManager.AppSettings.Get("PhoenixTelerik") != null && ConfigurationManager.AppSettings.Get("PhoenixTelerik").ToString() == "0")
                    || ConfigurationManager.AppSettings.Get("PhoenixTelerik") == null)
                {
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderFilter.aspx");
                }
                gvWorkOrder.PageSize = General.ShowRecords(gvWorkOrder.PageSize);

                ViewState["GroupId"] = null;
                ViewState["WONUMBER"] = string.Empty;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["WONOFILTER"] = string.Empty;
                ViewState["WONAME"] = string.Empty;
                ViewState["filterDiscipline"] = string.Empty;
                ViewState["filterStatus"] = string.Empty;
                ViewState["JobCategoryFilter"] = string.Empty;
                ViewState["CompCategory"] = string.Empty;
                ViewState["UNPLANNEDJOB"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                ViewState["IsCritical"] = string.Empty;
                ViewState["workorderId"] = string.Empty;

                if (Request.QueryString["d"] != null)
                    ViewState["TDATE"] = DateTime.Now.AddDays(-1);
                if (Request.QueryString["FDATE"] != null)
                    ViewState["FDATE"] = Request.QueryString["FDATE"];
                if (Request.QueryString["TDATE"] != null)
                    ViewState["TDATE"] = Request.QueryString["TDATE"];
                if (Request.QueryString["wono"] != null)
                    ViewState["WONOFILTER"] = Request.QueryString["wono"];
                if (Request.QueryString["jc"] != null)
                    ViewState["JobCategoryFilter"] = Request.QueryString["jc"];
                if (Request.QueryString["cc"] != null)
                    ViewState["CompCategory"] = Request.QueryString["cc"];
                if (Request.QueryString["status"] != null)
                    ViewState["filterStatus"] = Request.QueryString["status"];
                if (Request.QueryString["res"] != null)
                    ViewState["filterDiscipline"] = Request.QueryString["res"];
                if (Request.QueryString["cw"] != null)
                    ViewState["IsCritical"] = Request.QueryString["cw"];

                //if (Request.QueryString["FromJob"] == null)
                //{
                    SetFilter();
                //}


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("WOORDER"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx?groupId=" + ViewState["GroupId"]);
            }
            if (CommandName.ToUpper().Equals("JOBS"))
            {
                if (ViewState["GroupId"] != null)
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + ViewState["GroupId"] + "&WONUMBER=" + ViewState["WONUMBER"]);
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
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDJOBCATEGORY", "FLDPLANNINGDUEDATE", "FLDDURATION", "FLDDISCIPLINENAME", "FLDSTATUS" };
            string[] alCaptions = { "Work Order No.", "Category", "Planned Date", "Duration", "Assigned To", "Status" };
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

            NameValueCollection nvc = Filter.CurrentWorkorderGroupFilter;
            ds = PhoenixPlannedMaintenanceWorkOrderGroup.GroupList(
                                        nvc.Get("WorkorderId") != null ? General.GetNullableGuid(nvc.Get("WorkorderId").ToString()) : General.GetNullableGuid(ViewState["workorderId"].ToString()),
                                        nvc.Get("CompCategory") != null ? General.GetNullableInteger(nvc.Get("CompCategory").ToString()) : General.GetNullableInteger(ViewState["CompCategory"].ToString()),
                                        nvc.Get("Discipline") != null ? General.GetNullableInteger(nvc.Get("Discipline").ToString()) : General.GetNullableInteger(ViewState["filterDiscipline"].ToString()),
                                        nvc.Get("Status") != null ? General.GetNullableString(nvc.Get("Status").ToString()) : General.GetNullableString(ViewState["filterStatus"].ToString()),
                                        nvc.Get("FromDate") != null ? General.GetNullableDateTime(nvc.Get("FromDate").ToString()) : General.GetNullableDateTime(ViewState["FDATE"].ToString()),
                                        nvc.Get("ToDate") != null ? General.GetNullableDateTime(nvc.Get("ToDate").ToString()) : General.GetNullableDateTime(ViewState["TDATE"].ToString()),
                                        sortexpression, sortdirection,
                                        1, iRowCount, ref iRowCount, ref iTotalPageCount,
                                        nvc.Get("Unplanned") != null ? General.GetNullableInteger(nvc.Get("Unplanned").ToString()) : General.GetNullableInteger(ViewState["UNPLANNEDJOB"].ToString()),
                                        nvc.Get("JobCategory") != null ? nvc.Get("JobCategory").ToString() : ViewState["JobCategoryFilter"].ToString(),
                                        nvc.Get("IsCritical") != null ? General.GetNullableInteger(nvc.Get("IsCritical").ToString()) : General.GetNullableInteger(ViewState["IsCritical"].ToString()),
                                        nvc.Get("Wonumber") != null ? General.GetNullableString(nvc.Get("Wonumber")) :  General.GetNullableString(ViewState["WONOFILTER"].ToString()),
                                        nvc.Get("") != null ? General.GetNullableString(nvc.Get("WOName")) : General.GetNullableString(ViewState["WONAME"].ToString()));

            General.ShowExcel("Work Order", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

            //ds = PhoenixPlannedMaintenanceWorkOrderGroup.GroupList(General.GetNullableGuid(ViewState["workorderId"].ToString()), General.GetNullableInteger(ViewState["CompCategory"].ToString()), 
            //                General.GetNullableInteger(ViewState["filterDiscipline"].ToString()), 
            //                General.GetNullableString(ViewState["filterStatus"].ToString()),
            //                General.GetNullableDateTime(ViewState["FDATE"].ToString()),
            //                General.GetNullableDateTime(ViewState["TDATE"].ToString()),
            //                sortexpression, sortdirection,
            //                1, iRowCount, ref iRowCount, ref iTotalPageCount, General.GetNullableInteger(ViewState["UNPLANNEDJOB"].ToString()),
            //                ViewState["JobCategoryFilter"].ToString(),
            //                General.GetNullableInteger(ViewState["IsCritical"].ToString()));

            //General.ShowExcel("Work Order", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuDivWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvWorkOrder.CurrentPageIndex = 0;
                gvWorkOrder.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CREATE"))
            {
                //string sScript = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobListForPlanWorkOrder.aspx?UnPlannedJob=" + ViewState["UNPLANNEDJOB"]+"');";
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["filterDiscipline"] = string.Empty;
                ViewState["filterStatus"] = string.Empty;
                ViewState["JobCategoryFilter"] = string.Empty;
                ViewState["UNPLANNEDJOB"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;
                ViewState["workorderId"] = string.Empty;
                ViewState["WONOFILTER"] = string.Empty;
                ViewState["WONAME"] = string.Empty;
                Filter.CurrentWorkorderGroupFilter.Clear();

                gvWorkOrder.CurrentPageIndex = 0;
                gvWorkOrder.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue = ViewState["WONAME"].ToString();
                gvWorkOrder.MasterTableView.GetColumn("FLDJOBCATEGORYID").CurrentFilterValue = ViewState["JobCategoryFilter"].ToString();
                gvWorkOrder.MasterTableView.GetColumn("FLDPLANNINGDISCIPLINE").CurrentFilterValue = ViewState["filterDiscipline"].ToString();
                gvWorkOrder.MasterTableView.GetColumn("FLDSTATUSCODE").CurrentFilterValue = ViewState["filterStatus"].ToString();
                gvWorkOrder.MasterTableView.GetColumn("FLDPLANNINGDUEDATE").CurrentFilterValue = string.Empty;
                gvWorkOrder.MasterTableView.GetColumn("FLDWORKORDERNUMBER").CurrentFilterValue = ViewState["WONOFILTER"].ToString();
                gvWorkOrder.Rebind();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDJOBCATEGORY", "FLDPLANNINGDUEDATE", "FLDDURATION", "FLDDISCIPLINENAME", "FLDSTATUS" };
            string[] alCaptions = { "Work Order No.", "Category", "Planned Date", "Duration", "Assigned To", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            NameValueCollection nvc = Filter.CurrentWorkorderGroupFilter;
            ds = PhoenixPlannedMaintenanceWorkOrderGroup.GroupList(
                                        nvc != null && nvc.Get("WorkorderId") != null ? General.GetNullableGuid(nvc.Get("WorkorderId").ToString()) : General.GetNullableGuid(ViewState["workorderId"].ToString()),
                                        nvc != null && nvc.Get("CompCategory") != null ? General.GetNullableInteger(nvc.Get("CompCategory").ToString()) : General.GetNullableInteger(ViewState["CompCategory"].ToString()),
                                        nvc != null && nvc.Get("Discipline") != null ? General.GetNullableInteger(nvc.Get("Discipline").ToString()) : General.GetNullableInteger(ViewState["filterDiscipline"].ToString()),
                                        nvc != null && nvc.Get("Status") != null ? General.GetNullableString(nvc.Get("Status").ToString()) : General.GetNullableString(ViewState["filterStatus"].ToString()),
                                        nvc != null && nvc.Get("FromDate") != null ? General.GetNullableDateTime(nvc.Get("FromDate").ToString()) : General.GetNullableDateTime(ViewState["FDATE"].ToString()),
                                        nvc != null && nvc.Get("ToDate") != null ? General.GetNullableDateTime(nvc.Get("ToDate").ToString()) : General.GetNullableDateTime(ViewState["TDATE"].ToString()),
                                        sortexpression, sortdirection,
                                        gvWorkOrder.CurrentPageIndex + 1,
                                        gvWorkOrder.PageSize,
                                        ref iRowCount, ref iTotalPageCount,
                                        nvc != null && nvc.Get("Unplanned") != null ? General.GetNullableInteger(nvc.Get("Unplanned").ToString()) : General.GetNullableInteger(ViewState["UNPLANNEDJOB"].ToString()),
                                        nvc != null && nvc.Get("JobCategory") != null ? nvc.Get("JobCategory").ToString() : ViewState["JobCategoryFilter"].ToString(),
                                        nvc != null && nvc.Get("IsCritical") != null ? General.GetNullableInteger(nvc.Get("IsCritical").ToString()) : General.GetNullableInteger(ViewState["IsCritical"].ToString()),
                                        nvc != null && nvc.Get("Wonumber") != null ? General.GetNullableString(nvc.Get("Wonumber")) : General.GetNullableString(ViewState["WONOFILTER"].ToString()),
                                        nvc != null && nvc.Get("") != null ? General.GetNullableString(nvc.Get("WOName")) : General.GetNullableString(ViewState["WONAME"].ToString()));

            //ds = PhoenixPlannedMaintenanceWorkOrderGroup.GroupList(General.GetNullableGuid(ViewState["workorderId"].ToString()), 
            //                            General.GetNullableInteger(ViewState["CompCategory"].ToString()), 
            //                            General.GetNullableInteger(ViewState["filterDiscipline"].ToString()), 
            //                            General.GetNullableString(ViewState["filterStatus"].ToString()),
            //                            General.GetNullableDateTime(ViewState["FDATE"].ToString()),
            //                            General.GetNullableDateTime(ViewState["TDATE"].ToString()),
            //                            sortexpression, sortdirection,
            //                            gvWorkOrder.CurrentPageIndex + 1, gvWorkOrder.PageSize, ref iRowCount, ref iTotalPageCount,
            //                            General.GetNullableInteger(ViewState["UNPLANNEDJOB"].ToString()),
            //                            ViewState["JobCategoryFilter"].ToString(),
            //                            General.GetNullableInteger(ViewState["IsCritical"].ToString()));

            General.SetPrintOptions("gvWorkOrder", "Work Order", alCaptions, alColumns, ds);

            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["GroupId"] = ds.Tables[0].Rows[0]["FLDWORKORDERGROUPID"].ToString();
                ViewState["WONUMBER"] = ds.Tables[0].Rows[0]["FLDWORKORDERNUMBER"].ToString();
            }
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
            gvWorkOrder.SelectedIndexes.Clear();
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            //if (e.CommandName.ToUpper().Equals("ROWCLICK") || e.CommandName.ToUpper().Equals("SELECT"))
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                ViewState["GroupId"] = ((RadLabel)item.FindControl("lblGroupId")).Text;
                ViewState["WONUMBER"] = ((LinkButton)item.FindControl("lblWorkorderNumber")).Text;
                ViewState["WOPID"] = ((RadLabel)item.FindControl("lblWOPID")).Text;
                SetFilter();
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + ViewState["GroupId"] + "&WONUMBER=" + ViewState["WONUMBER"] + "&wopid=" + ViewState["WOPID"]);
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string groupId = ((RadLabel)e.Item.FindControl("lblGroupId")).Text;
                string dueDate = ((RadLabel)e.Item.FindControl("lblDuedate")).Text;
                string discipline = ((UserControlDiscipline)e.Item.FindControl("ucDisciplineEdit")).SelectedDiscipline;
                string durationInDay = ((RadMaskedTextBox)e.Item.FindControl("txtDurationInDayEdit")).Text;
                string duration = ((RadMaskedTextBox)e.Item.FindControl("txtDurationEdit")).Text;
                string title = ((RadTextBox)e.Item.FindControl("txtTitleEdit")).Text;
                string isUnplanned = ((RadCheckBox)e.Item.FindControl("chkIsPlannedEdit")).Checked == true ? "0" : "1";

                if (!IsValidWorkorder(dueDate, discipline, title))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixPlannedMaintenanceWorkOrderGroup.GroupDetailUpdate(new Guid(groupId), title, General.GetNullableDateTime(dueDate),
                                                                        General.GetNullableInteger(discipline)
                                                                        , General.GetNullableInteger(duration)
                                                                        , General.GetNullableInteger(durationInDay)
                                                                        , General.GetNullableInteger(isUnplanned));
                gvWorkOrder.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string groupId = ((RadLabel)item.FindControl("lblGroupId")).Text;
                PhoenixPlannedMaintenanceWorkOrderGroup.GroupWoDelete(new Guid(groupId));

                gvWorkOrder.Rebind();
            }
            //else if (e.CommandName.ToUpper().Equals("TOOLBOX"))
            //{

            //    GridDataItem item = (GridDataItem)e.Item;
            //    string groupId = ((RadLabel)item.FindControl("lblGroupId")).Text;
            //    PhoenixPlannedMaintenanceWorkOrderGroup.PlannedJobToolboxCheck(new Guid(groupId));

            //    gvWorkOrder.Rebind();
            //    ucStatus.Text = "Tool box meet generated";

            //}
            else if (e.CommandName.ToUpper().Equals("TYPECHANGE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string groupId = ((RadLabel)item.FindControl("lblGroupId")).Text;
                PhoenixPlannedMaintenanceWorkOrderGroup.GroupCategoryChange(new Guid(groupId), 0);
                ucStatus.Text = "Type changed successfully";

                gvWorkOrder.Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("ISSUE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string groupId = ((RadLabel)item.FindControl("lblGroupId")).Text;
                PhoenixPlannedMaintenanceWorkOrderGroup.GroupWoGenerate(new Guid(groupId));

                gvWorkOrder.Rebind();
                ucStatus.Text = "Work order issued";
            }
            else if (e.CommandName == RadGrid.FilterCommandName)
            {
                ViewState["WONAME"] = gvWorkOrder.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue;
                ViewState["JobCategoryFilter"] = gvWorkOrder.MasterTableView.GetColumn("FLDJOBCATEGORYID").CurrentFilterValue;
                ViewState["filterDiscipline"] = gvWorkOrder.MasterTableView.GetColumn("FLDPLANNINGDISCIPLINE").CurrentFilterValue;
                ViewState["filterStatus"] = gvWorkOrder.MasterTableView.GetColumn("FLDSTATUSCODE").CurrentFilterValue;
                string daterange = gvWorkOrder.MasterTableView.GetColumn("FLDPLANNINGDUEDATE").CurrentFilterValue;
                if (daterange != "")
                {
                    ViewState["FDATE"] = daterange.Split('~')[0];
                    ViewState["TDATE"] = daterange.Split('~')[1];
                }

                ViewState["WONOFILTER"] = gvWorkOrder.MasterTableView.GetColumn("FLDWORKORDERNUMBER").CurrentFilterValue;
                SetFilter();
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
    private void ResetMenu()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("List", "WOORDER");
        toolbarmain.AddButton("Jobs", "JOBS");

        if (Request.QueryString["from"] != null)
        {
            toolbarmain.AddButton("Back", "BACK");
        }
        MenuWorkOrder.AccessRights = this.ViewState;
        MenuWorkOrder.MenuList = toolbarmain.Show();
        //MenuWorkOrder.SetTrigger(pnlComponent);
        MenuWorkOrder.SelectedMenuIndex = 0;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
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
    private bool IsValidWorkorder(string dueDate, string responsible, string title)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(dueDate))
            ucError.ErrorMessage = "Due Date is required.";

        if (string.IsNullOrEmpty(responsible) || responsible == "Dummy")
            ucError.ErrorMessage = "Responsibility is required.";

        if (title.Trim() == "")
            ucError.ErrorMessage = "Title is required.";

        return (!ucError.IsError);
    }

    protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        RadGrid grid = (RadGrid)sender;
        if (e.Item is GridFilteringItem)
        {
            grid.MasterTableView.GetColumn("FLDWORKORDERNUMBER").CurrentFilterValue = GetFilter("Wonumber");
            grid.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue = GetFilter("WOName");
            grid.MasterTableView.GetColumn("FLDJOBCATEGORYID").CurrentFilterValue = GetFilter("JobCategoryFilter");
            grid.MasterTableView.GetColumn("FLDPLANNINGDISCIPLINE").CurrentFilterValue = GetFilter("Discipline");
            grid.MasterTableView.GetColumn("FLDSTATUSCODE").CurrentFilterValue = GetFilter("filterStatus");
            grid.MasterTableView.GetColumn("FLDPLANNINGDUEDATE").CurrentFilterValue = GetFilter("FromDate") + '~' + GetFilter("ToDate");

        }
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;
            string status = drv["FLDSTATUSCODE"].ToString();
            LinkButton wono = (LinkButton)e.Item.FindControl("lblWorkorderNumber");
            if (wono != null)
            {
                if (drv["FLDWODETAILID"].ToString().Equals(""))
                {
                    wono.Font.Italic = true;
                    wono.Font.Bold = true;
                }
            }
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
                if (status != "ISS")  //issue
                {
                    edit.Attributes.Add("style", "display:none");
                }
            }

            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                if (status != "ISS")  //Issue
                {
                    del.Attributes.Add("style", "display:none");
                }
            }

            LinkButton tb = (LinkButton)e.Item.FindControl("cmdToolBox");
            if (tb != null)
            {
                tb.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupToolBoxMeet.aspx?groupId=" + drv["FLDWORKORDERGROUPID"] + "&WONUMBER=" + drv["FLDWORKORDERNUMBER"] + "&wopid="+ drv["FLDWODETAILID"] +"'; showDialog('Toolbox Meet - [" + drv["FLDWORKORDERNUMBER"].ToString() + " " + drv["FLDWORKORDERNAME"].ToString() + "]', true);");
                //tb.Attributes.Add("onclick", "javascript:openNewWindow('TOOLBOX', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupToolBoxMeet.aspx?groupId=" + drv["FLDWORKORDERGROUPID"] + "&WONUMBER=" + drv["FLDWORKORDERNUMBER"] + "'); ");
                tb.Visible = SessionUtil.CanAccess(this.ViewState, tb.CommandName);
                if (drv["FLDTOOLBOXMET"].ToString() == "0")
                {
                    tb.ToolTip = "Toolbox meet not done or conditions (e.g. RA, PTW, Spares) not met";
                    tb.Attributes["style"] = "color:red !important";
                }
                else if (drv["FLDTOOLBOXMET"].ToString() == "1")
                {
                    tb.ToolTip = "Conditions met, Toolbox meet can be conducted but not yet done";
                    tb.Attributes["style"] = "color:#9b870c !important";
                }
                else if (drv["FLDTOOLBOXMET"].ToString() == "2")
                {
                    tb.ToolTip = "Toolbox meet done";
                    tb.Attributes["style"] = "color:green !important";
                }
            }
            LinkButton Report = (LinkButton)e.Item.FindControl("cmdReport");
            if (Report != null)
            {
                Report.Visible = SessionUtil.CanAccess(this.ViewState, Report.CommandName);
                if (status == "ISS" || status == "POP")   //ISSUE OR POSTPONE
                {
                    Report.Attributes.Add("style", "display:none");
                }
                Report.Attributes.Add("onclick", "javascript:$modalWindow.modalWindowUrl='" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenancesSubWorkOrderReport.aspx?attachment="+ drv["FLDATTACHMENTYN"].ToString() +"&groupId=" + drv["FLDWORKORDERGROUPID"] + "&UnplannedJob=" + drv["FLDISUNPLANNEDJOB"] + "'; showDialog('Report - [" + drv["FLDWORKORDERNUMBER"].ToString() + " " + drv["FLDWORKORDERNAME"].ToString() + "]', true);");
                if(PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                {
                    Report.Attributes["title"] = "View Work Order Report";
                }
            }
            LinkButton reschedule = (LinkButton)e.Item.FindControl("cmdReschedule");
            if (reschedule != null)
            {
                reschedule.Visible = SessionUtil.CanAccess(this.ViewState, reschedule.CommandName);
                //reschedule.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupReschedule.aspx?groupId=" + drv["FLDWORKORDERGROUPID"] + "'); ");
                reschedule.Attributes.Add("onclick", "javascript:openNewWindow('postpone', 'Reschedule/Cancel WO - " + drv["FLDWORKORDERNAME"].ToString() + "', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkOrderGroupReschedule.aspx?groupId=" + drv["FLDWORKORDERGROUPID"] + "',false,500,400); ");
                //reschedule.Attributes["onclick"] = string.Format("return ShowRescheduleForm('{0}');", drv["FLDWORKORDERGROUPID"].ToString());
                if (status != "ISS" && status != "POP")   //Issue && posponed
                {
                    reschedule.Attributes.Add("style", "display:none");
                }
                reschedule.Visible = false;
                if (drv["FLDISPLANNED"].ToString().Equals("1") && drv["FLDISPLANCARRYOVER"].ToString().Equals("0") || drv["FLDWODETAILID"].ToString().Equals(""))
                    reschedule.Visible = true;
            }
            LinkButton tc = (LinkButton)e.Item.FindControl("cmdTypeChange");              //planned or unplanned
            if (tc != null)
            {
                tc.Visible = SessionUtil.CanAccess(this.ViewState, tc.CommandName);
                if (status == "IPG")  //In progress
                {
                    tc.Attributes.Add("style", "display:none");
                }
            }
            LinkButton Communication = (LinkButton)e.Item.FindControl("lnkCommunication");
            if (Communication != null)
            {
                int vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                Communication.Visible = SessionUtil.CanAccess(this.ViewState, Communication.CommandName);
                Communication.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','Communication - " + drv["FLDWORKORDERNAME"].ToString() + "','" + Session["sitepath"] + "/Common/CommonCommunication.aspx?Type=WORKORDER" + "&Referenceid=" + drv["FLDWORKORDERGROUPID"] + "&Vesselid=" + vesselid + "');");
            }
            LinkButton history = (LinkButton)e.Item.FindControl("cmdhistory");
            if (history != null)
            {
                history.Visible = SessionUtil.CanAccess(this.ViewState, history.CommandName);
                if (drv["FLDISVERIFICATION"].ToString() != "1")
                    history.Visible = false;

                history.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','Verification - " + drv["FLDWORKORDERNAME"].ToString() + "','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWorkorderVerificationHistory.aspx?groupid=" + drv["FLDWORKORDERGROUPID"] + "&Vesselid=" + drv["FLDVESSELID"] + "');");
            }
            ImageButton cmdAtt = (ImageButton)e.Item.FindControl("cmdAtt");
            if (cmdAtt != null)
            {
                cmdAtt.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?mimetype=.pdf&dtkey=" + drv["FLDWORKORDERGROUPID"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&DocSource=WOGRP'); return false;");
                cmdAtt.Visible = SessionUtil.CanAccess(this.ViewState, cmdAtt.CommandName);
                if (drv["FLDATTACHMENTYN"].ToString() == "0" || drv["FLDATTACHMENTYN"].ToString() == "") cmdAtt.ImageUrl = Session["images"] + "/no-attachment.png";
            }
            TableCell cell = dataItem["RA"];
            if (drv["FLDISRA"].ToString().ToLower() == "yes")
                cell.ForeColor = Color.Green;
            else if (drv["FLDISRA"].ToString().ToLower() == "no")
                cell.ForeColor = Color.Red;

            cell = dataItem["PTW"];
            if (drv["FLDISPTW"].ToString().ToLower() == "yes")
                cell.ForeColor = Color.Green;
            else if (drv["FLDISPTW"].ToString().ToLower() == "no")
                cell.ForeColor = Color.Red;

            cell = dataItem["SPARES"];
            if (drv["FLDISSPARES"].ToString().ToLower() == "yes")
                cell.ForeColor = Color.Green;
            else if (drv["FLDISSPARES"].ToString().ToLower() == "no")
                cell.ForeColor = Color.Red;

            LinkButton start = (LinkButton)e.Item.FindControl("cmdStart");
            if (start != null)
            {
                start.Visible = SessionUtil.CanAccess(this.ViewState, start.CommandName);
                start.Attributes.Add("onclick", "javascript:top.openNewWindow('activity','Start Maintenance','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceTimeSheet.aspx?wo=" + drv["FLDWORKORDERGROUPID"].ToString() + "&wopi=" + drv["FLDWODETAILID"].ToString() + "&a=2'); return false;");
                if (drv["FLDISINPROGRESS"].ToString().Equals("1") || drv["FLDISREPORTPENDING"].ToString().Equals("1")
                    || drv["FLDISCOMPLETED"].ToString().Equals("1") || drv["FLDISPLANCARRYOVER"].ToString().Equals("1") || drv["FLDWODETAILID"].ToString().Equals(""))
                    start.Visible = false;
            }

            RadRadioButtonList divProgress = (RadRadioButtonList)e.Item.FindControl("rblInProgress");
            if (divProgress != null)
            {
                if (drv["FLDISCOMPLETED"].ToString() == "1" || drv["FLDISPLANNED"].ToString().Equals("1") ||
                     (drv["FLDISPLANCARRYOVER"].ToString().Equals("0") && drv["FLDISINPROGRESS"].ToString().Equals("0")))
                    divProgress.Visible = false;
            }
            RadRadioButtonList divPlanned = (RadRadioButtonList)e.Item.FindControl("rblPlanned");
            if (divPlanned != null)
            {
                if (drv["FLDISCOMPLETED"].ToString() == "1" || drv["FLDISINPROGRESS"].ToString().Equals("1") || drv["FLDISPLANCARRYOVER"].ToString().Equals("0"))
                    divPlanned.Visible = false;
            }
            if (drv != null && divProgress != null)
            {
                if (drv["FLDISREPORTPENDING"].ToString() == "1")
                {
                    divProgress.Visible = false;
                    divPlanned.Visible = false;
                }                
            }
            if(divProgress != null && !divProgress.Visible && !divPlanned.Visible)
            {
                divProgress.Visible = true;
                divProgress.Attributes["style"] = "visibility:hidden";
            }
        }
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            UserControlDiscipline dsc = (UserControlDiscipline)e.Item.FindControl("ucDisciplineEdit");
            if (dsc != null)
            {
                dsc.SelectedDiscipline = drv["FLDPLANNINGDISCIPLINE"].ToString();
            }
        }
    }



    protected void cmbIsunPlanned_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvWorkOrder.CurrentPageIndex = 0;

        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDISUNPLANNEDJOB").CurrentFilterValue = e.Value;
        ViewState["UNPLANNEDJOB"] = e.Value;

        SetFilter();

        gvWorkOrder.Rebind();
    }
    protected void cblJobCategory_DataBinding(object sender, EventArgs e)
    {
        RadComboBox jobCategory = sender as RadComboBox;
        jobCategory.DataSource = PhoenixRegistersQuick.ListQuick(1, 165);
        jobCategory.DataTextField = "FLDQUICKNAME";
        jobCategory.DataValueField = "FLDQUICKCODE";
    }
    protected void cblDiscipline_DataBinding(object sender, EventArgs e)
    {
        RadComboBox discipline = sender as RadComboBox;
        discipline.DataSource = PhoenixRegistersDiscipline.ListDiscipline();
        discipline.DataTextField = "FLDDISCIPLINENAME";
        discipline.DataValueField = "FLDDISCIPLINEID";

        discipline.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }
    protected void cmbStatus_DataBinding(object sender, EventArgs e)
    {
        RadComboBox status = sender as RadComboBox;
        status.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 10, 0, "ISS,POP,IPG,RPP,CVR,SVP,CAN");
        status.DataTextField = "FLDHARDNAME";
        status.DataValueField = "FLDHARDCODE";

        status.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }    
    protected void ucToDate_TextChangedEvent(object sender, EventArgs e)
    {

    }
    protected void ucFromDate_TextChangedEvent(object sender, EventArgs e)
    {

    }
    protected void btnCreateWO_Click(object sender, EventArgs e)
    {
        //PhoenixPlannedMaintenanceWorkOrderGroup.WorkordergroupReschedule(new Guid(Request.QueryString["groupId"].ToString())
        //                                                         , DateTime.Parse(txtPostponeDate.SelectedDate.ToString())
        //                                                         , txtRemarks.Text);

        string script = "function f(){CloseModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
    }

    private void SetFilter()
    {
        NameValueCollection criteria = new NameValueCollection
        {
            { "Discipline", ViewState["filterDiscipline"].ToString() },
            { "Status", ViewState["filterStatus"].ToString() },
            { "JobCategory", ViewState["JobCategoryFilter"].ToString() },
            { "CompCategory", ViewState["CompCategory"].ToString() },
            { "Unplanned", ViewState["UNPLANNEDJOB"].ToString() },
            { "FromDate", ViewState["FDATE"].ToString() },
            { "ToDate", ViewState["TDATE"].ToString() },
            { "IsCritical", ViewState["IsCritical"].ToString() },
            { "WorkorderId", string.Empty },
            { "Wonumber", ViewState["WONOFILTER"].ToString() },
            { "WOName", ViewState["WONAME"].ToString() }
        };
        Filter.CurrentWorkorderGroupFilter = criteria;
    }

    protected string GetFilter(string filter)
    {
        string value = string.Empty;
        NameValueCollection nvc = Filter.CurrentWorkorderGroupFilter;
        if (nvc != null)
        {
            value = nvc[filter];
        }
        return value;
    }
    protected void SetFilterValue(string key, string value)
    {
        NameValueCollection nvc = Filter.CurrentWorkorderGroupFilter;
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
        Filter.CurrentWorkorderGroupFilter = nvc;
    }
    protected void cmdHiddenActivity_Click(object sender, EventArgs e)
    {
        try
        {
            RadButton btn = (RadButton)sender;
            string args = btn.CommandArgument;
            if (args.Trim().Equals("")) return;
            string[] val = args.Split('~');
            string completionstatus = val[0];
            string id = val[1];
            string status = val[2];
            Guid? ActivityId = General.GetNullableGuid(id);
            int? Status = General.GetNullableInteger(status);

            PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWorkOrderStatus(new Guid(id)
                           , PhoenixSecurityContext.CurrentSecurityContext.VesselID, int.Parse(status), DateTime.Now, General.GetNullableInteger(completionstatus), null, null);
            PhoenixPlannedMaintenanceDailyWorkPlan.UpdateActivityMemberActualTimeLine(ActivityId, 2, General.GetNullableInteger(completionstatus), null, null);

            PhoenixPlannedMaintenanceDailyWorkPlan.UpdateWRH(ActivityId, 2, General.GetNullableInteger(completionstatus));

            if (completionstatus == "5")
            {
                PhoenixPlannedMaintenanceDailyWorkPlan.CarryForwardWorkOrder(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            }
            gvWorkOrder.Rebind();
        }
        catch (Exception ex)
        {
            RadWindowManager1.RadAlert("<span class=\"bold-red\">" + ex.Message.Replace("'", "&lsquo;") + "<span>", 330, 180, "Message", null);
        }
    }
}
