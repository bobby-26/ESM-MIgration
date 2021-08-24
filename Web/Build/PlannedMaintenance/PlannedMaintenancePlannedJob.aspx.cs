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

public partial class PlannedMaintenancePlannedJob : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenancePlannedJob.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenancePlannedJob.aspx", "Add Job", "<i class=\"fas fa-plus-circle\"></i>", "CREATE");

            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();
            ResetMenu();
            if (!IsPostBack)
            {
                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["GroupId"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;


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

            if (CommandName.ToUpper().Equals("JOBGROUP"))
            {
                Response.Redirect("../PlannedMaintenance/PlannedMaintenancePlannedJobGroup.aspx?groupId=" + ViewState["GroupId"]);
            }
            if (CommandName.ToUpper().Equals("JOBS"))
            {
                if (ViewState["GroupId"] != null)
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenancePlannedJob.aspx?groupId=" + ViewState["GroupId"]);
            }
            if (CommandName.ToUpper().Equals("WORKORDER"))
            {
                if (ViewState["GroupId"] != null)
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenancePlannedWorkOrder.aspx?groupId=" + ViewState["GroupId"]);
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
            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDPLANNINGDUEDATE", "FLDDISCIPLINENAME", "FLDSTATUS" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Due Date", "Responsibility", "Status" };
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

            ds = PhoenixPlannedMaintenanceWorkOrderGroup.GroupList(null, null, null, General.GetNullableString(""), null, null, sortexpression, sortdirection,
                            int.Parse(ViewState["PAGENUMBER"].ToString()), gvWorkOrder.PageSize, ref iRowCount, ref iTotalPageCount,0);

            General.ShowExcel("Work Order", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

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
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CREATE"))
            {
                string sScript = "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobListForPlanWorkOrder.aspx?UnPlannedJob=" + ViewState["UNPLANNEDJOB"] + "');";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "", sScript, true);
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

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDCLASSCODE", "FLDFREQUENCYNAME", "FLDPLANINGPRIORITY", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTARTEDDATE", "FLDWORKORDERCOMPLETEDDATE" };
            string[] alCaptions = { "Work Order Number", "Work Order Title", "Component Number", "Component Name", "Class Code", "Frequency", "Priority", "Resp Discipline", "Status", "Due Date", "Started", "Completed" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            ds = PhoenixPlannedMaintenanceWorkOrderGroup.GroupList(null, null, null, General.GetNullableString(""), null, null, sortexpression, sortdirection
                                         , int.Parse(ViewState["PAGENUMBER"].ToString()), gvWorkOrder.PageSize, ref iRowCount, ref iTotalPageCount,0);


            General.SetPrintOptions("gvWorkOrder", "Work Order", alCaptions, alColumns, ds);

            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["GroupId"] = ds.Tables[0].Rows[0]["FLDWORKORDERGROUPID"].ToString();

                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenancePlannedWorkOrder.aspx?groupId=" + ViewState["GroupId"];

            }

                

            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkOrder_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        //      gvWorkOrder.SelectedIndex = se.NewSelectedIndex;

        //      ViewState["COMPONENTID"] = ((Label)gvWorkOrder.Rows[se.NewSelectedIndex].FindControl("lblComponentId")).Text;
        //      ViewState["WORKORDERID"] = ((Label)gvWorkOrder.Rows[se.NewSelectedIndex].FindControl("lblWorkOrderId")).Text;
        //      ViewState["JOBID"] = ((Label)gvWorkOrder.Rows[se.NewSelectedIndex].FindControl("lblJobID")).Text;
        //      ViewState["DTKEY"] = ((Label)gvWorkOrder.Rows[se.NewSelectedIndex].FindControl("lbldtkey")).Text;
        //ViewState["WORKORDERNO"] = ((Label)gvWorkOrder.Rows[se.NewSelectedIndex].FindControl("lblWorkorderNo")).Text;
        //ResetMenu();
        //      BindData();
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
            if (e.CommandName == "RowClick")
            {
                GridDataItem item = (GridDataItem)e.Item;
                ViewState["GroupId"] = ((RadLabel)item.FindControl("lblGroupId")).Text;
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenancePlannedWorkOrder.aspx?groupId=" + ViewState["GroupId"];
            }
            else if (e.CommandName == "Select")
            {
                GridDataItem item = (GridDataItem)e.Item;
                ViewState["GroupId"] = ((RadLabel)item.FindControl("lblGroupId")).Text;
                ifMoreInfo.Attributes["src"] = "../PlannedMaintenance/PlannedMaintenancePlannedWorkOrder.aspx?groupId=" + ViewState["GroupId"];
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string groupId = ((RadLabel)e.Item.FindControl("lblGroupId")).Text;
                string title = ((RadTextBox)e.Item.FindControl("txtWorkOrderNameEdit")).Text;
                string dueDate = ((UserControlDate)e.Item.FindControl("ucFromDateEdit")).Text;
                string discipline =((UserControlDiscipline)e.Item.FindControl("ucDisciplineEdit")).SelectedDiscipline;

                if (!IsValidWorkorder(title, dueDate,discipline))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixPlannedMaintenanceWorkOrderGroup.GroupDetailUpdate(new Guid(groupId), title, General.GetNullableDateTime(dueDate),General.GetNullableInteger(discipline),null,null);
                BindData();
                gvWorkOrder.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string groupId = ((RadLabel)item.FindControl("lblGroupId")).Text;
                PhoenixPlannedMaintenanceWorkOrderGroup.GroupWoDelete(new Guid(groupId));

                BindData();
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

    }
    private void ResetMenu()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("General", "JOBGROUP");
        toolbarmain.AddButton("Jobs", "JOBS");
        toolbarmain.AddButton("Work Order", "WORKORDER");

        MenuWorkOrder.AccessRights = this.ViewState;
        MenuWorkOrder.MenuList = toolbarmain.Show();
        //MenuWorkOrder.SetTrigger(pnlComponent);
        MenuWorkOrder.SelectedMenuIndex = 1;
    }



    protected void gvWorkOrder_SelectedIndexChanged(object sender, EventArgs e)
    {
        //var dataItem = (gvWorkOrder.SelectedItems[0] as GridDataItem);
        //if(dataItem != null)
        //{
        //    RadLabel groupid = dataItem.FindControl("lblGroupId") as RadLabel;
        //    ViewState["GroupId"] = groupid.Text;
        //}
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvWorkOrder.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidWorkorder(string title, string dueDate, string responsible)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (title.Trim().Equals(""))
            ucError.ErrorMessage = "Title is required.";

        if (string.IsNullOrEmpty(dueDate))
            ucError.ErrorMessage = "Due Date is required.";

        if(string.IsNullOrEmpty(responsible) || responsible == "Dummy")
            ucError.ErrorMessage = "Responsibility is required.";

        return (!ucError.IsError);
    }

    protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
        }
        if(e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            UserControlDiscipline dsc = (UserControlDiscipline)e.Item.FindControl("ucDisciplineEdit");
            if(dsc != null)
            {
                dsc.SelectedDiscipline = drv["FLDPLANNINGDISCIPLINE"].ToString();
            }
        }
    }
}
