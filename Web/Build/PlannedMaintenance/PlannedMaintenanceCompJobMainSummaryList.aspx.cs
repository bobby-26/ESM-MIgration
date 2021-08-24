using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceCompJobMainSummaryList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            ViewState["JOBSUMMARYID"] = null;
            if (ViewState["JOBSUMMARYID"] == null)
            {
                ViewState["JOBSUMMARYID"] = Guid.NewGuid().ToString();
            }
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceCompJobMainSummaryList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceCompJobMainSummaryInsert.aspx?JOBSUMMARYID=" + ViewState["JOBSUMMARYID"] + "", "Create Summary", "<i class=\"fas fa-plus-circle\"></i>", "CREATE");

            MenuDivWorkOrder.AccessRights = this.ViewState;
            MenuDivWorkOrder.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                gvWorkOrder.PageSize = General.ShowRecords(gvWorkOrder.PageSize);
                ViewState["TITLE"] = string.Empty;
                ViewState["FDATE"] = string.Empty;
                ViewState["TDATE"] = string.Empty;

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

    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDTITLE", "FLDTYPE", "FLDCREATEDDATE", "FLDCREATEDBYNAME", "FLDVESSELNAMELIST", };
            string[] alCaptions = { "Title", "Type", "Created Date", "Created by", "Vessels Applicable" };
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

            ds = PhoenixPlannedMaintenanceCompJobSummary.CompjobSummarySearch(ViewState["TITLE"].ToString(), General.GetNullableDateTime(ViewState["FDATE"].ToString())
                                                                            , General.GetNullableDateTime(ViewState["TDATE"].ToString()), null
                                                                            , sortexpression, sortdirection
                                                                            , 1
                                                                            , iRowCount, ref iRowCount
                                                                            , ref iTotalPageCount
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , 1);

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
                gvWorkOrder.CurrentPageIndex = 0;
                gvWorkOrder.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
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

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDTITLE", "FLDTYPE", "FLDCREATEDDATE", "FLDCREATEDBYNAME", "FLDVESSELNAMELIST", };
            string[] alCaptions = { "Title", "Type", "Created Date", "Created by", "Vessels Applicable" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            ds = PhoenixPlannedMaintenanceCompJobSummary.CompjobSummarySearch(ViewState["TITLE"].ToString(), General.GetNullableDateTime(ViewState["FDATE"].ToString())
                                                                            , General.GetNullableDateTime(ViewState["TDATE"].ToString()), null
                                                                            , sortexpression, sortdirection
                                                                            , gvWorkOrder.CurrentPageIndex + 1
                                                                            , gvWorkOrder.PageSize, ref iRowCount
                                                                            , ref iTotalPageCount
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , 1);



            General.SetPrintOptions("gvWorkOrder", "Job Summary", alCaptions, alColumns, ds);

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
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
                edit.Attributes.Add("onclick", "window.location.href='../PlannedMaintenance/PlannedMaintenanceCompJobMainSummaryInsert.aspx?JOBSUMMARYID=" + drv["FLDCOMPJOBSUMMARYID"].ToString() + "'");
                //edit.Attributes.Add("onclick", "javascript: openNewWindow('codehelp1', '', 'PlannedMaintenance/PlannedMaintenanceCompJobMainSummaryInsert.aspx?JOBSUMMARYID=" + drv["FLDCOMPJOBSUMMARYID"].ToString() + "'); return false;");
            }
            LinkButton title = (LinkButton)e.Item.FindControl("lnkTitle");
            if(title != null)
            {
                title.Visible = SessionUtil.CanAccess(this.ViewState, title.CommandName);
                title.Attributes.Add("onclick", "javascript: openNewWindow('codehelp1', '', 'PlannedMaintenance/PlannedMaintenanceCompJobSummaryDetail.aspx?JOBSUMMARYID=" + drv["FLDCOMPJOBSUMMARYID"].ToString() + "'); return false;");

            }
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }
            LinkButton pub = (LinkButton)e.Item.FindControl("cmdPublish");
            if (pub != null)
            {
                pub.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event,'Do you want to publish this'); return false;");
                pub.Visible = SessionUtil.CanAccess(this.ViewState, pub.CommandName);
            }
        }
    }



    protected void gvWorkOrder_ItemCommand1(object sender, GridCommandEventArgs e)
    {
        gvWorkOrder.SelectedIndexes.Clear();

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            string summaryId = item.GetDataKeyValue("FLDCOMPJOBSUMMARYID").ToString();
            PhoenixPlannedMaintenanceCompJobSummary.CompjobSummaryDelete(new Guid(summaryId));

            gvWorkOrder.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("PUBLISH"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            string summaryId = item.GetDataKeyValue("FLDCOMPJOBSUMMARYID").ToString();
            PhoenixPlannedMaintenanceCompJobSummary.CompjobSummaryPublish(new Guid(summaryId));

            gvWorkOrder.Rebind();
        }
        else if (e.CommandName == RadGrid.FilterCommandName)
        {
            ViewState["TITLE"] = gvWorkOrder.MasterTableView.GetColumn("FLDTITLE").CurrentFilterValue.ToString();
            string daterange = gvWorkOrder.MasterTableView.GetColumn("FLDCREATEDDATE").CurrentFilterValue.ToString();
            if (daterange != "")
            {
                ViewState["FDATE"] = daterange.Split('~')[0];
                ViewState["TDATE"] = daterange.Split('~')[1];
            }

            gvWorkOrder.Rebind();

        }
        }
    }
