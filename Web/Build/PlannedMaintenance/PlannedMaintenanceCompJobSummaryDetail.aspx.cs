using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.Registers;

public partial class PlannedMaintenanceCompJobSummaryDetail : PhoenixBasePage
{
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceCompJobSummaryDetail.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            menuPms.AccessRights = this.ViewState;
            menuPms.MenuList = toolbar.Show();

            ViewState["JOBSUMMARYID"] = null;
            ViewState["GRIDCOMPJOBID"] = string.Empty;
            ViewState["VESSELLIST"] = string.Empty;
            ViewState["COMPONENT"] = string.Empty;
            ViewState["JOB"] = string.Empty;
            ViewState["CATEGORY"] = string.Empty;
            ViewState["FROM"] = string.Empty;
            ViewState["TO"] = string.Empty;
            ViewState["VESSEL"] = string.Empty;

            if (!string.IsNullOrEmpty(Request.QueryString["JOBSUMMARYID"]))
            {
                ViewState["JOBSUMMARYID"] = Request.QueryString["JOBSUMMARYID"];
            }
            if (!string.IsNullOrEmpty(Request.QueryString["VESSELLIST"]))
            {
                ViewState["VESSELLIST"] = Request.QueryString["VESSELLIST"];
            }

            GvPMS.PageSize = General.ShowRecords(null);
        }
    }

    protected void GvPMS_NeedDataSource1(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            if (ViewState["JOBSUMMARYID"] == null)
            {
                return;
            }
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDVESSELNAME", "FLDCOMPONENT", "FLDJOB", "FLDQUICKNAME", "FLDFREQUENCYNAME", "FLDJOBLASTDONEDATE", "FLDJOBNEXTDUEDATE" };
            string[] alCaptions = {"Vessel", "Component", "Job", "Category", "Frequency", "Last Done", "Next Due" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 25;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds;

            ds = PhoenixPlannedMaintenanceCompJobSummary.CompjobSummaryDetailSearch(new Guid(ViewState["JOBSUMMARYID"].ToString()), ViewState["VESSELLIST"].ToString()
                                                                , sortexpression, sortdirection
                                                                , GvPMS.CurrentPageIndex + 1
                                                                , GvPMS.PageSize, ref iRowCount
                                                                , ref iTotalPageCount
                                                                , ViewState["COMPONENT"].ToString()
                                                                , ViewState["JOB"].ToString()
                                                                , General.GetNullableInteger(ViewState["CATEGORY"].ToString())
                                                                , General.GetNullableDateTime(ViewState["FROM"].ToString())
                                                                , General.GetNullableDateTime(ViewState["TO"].ToString())
                                                                , ViewState["VESSEL"].ToString());


            General.SetPrintOptions("gvWorkOrder", "Job Summary", alCaptions, alColumns, ds);



            GvPMS.DataSource = ds;
            GvPMS.VirtualItemCount = iRowCount;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void GvPMS_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel due = (RadLabel)e.Item.FindControl("lblNextDue");

            if (due != null)
            {
                if (drv["FLDISOVERDUE"].ToString() == "1")
                {
                    due.Attributes["style"] = "color:red !important";
                }
            }

            LinkButton job = (LinkButton)e.Item.FindControl("lnkJobcode");
            if(job != null)
            {
                //job.Enabled = SessionUtil.CanAccess(this.ViewState, job.CommandName);
                job.Attributes.Add("onclick", "javascript:openNewWindow('na', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobSummaryComponentJobGeneral.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&VESSELID=" + drv["FLDVESSELID"] + "',false); ");
            }
            LinkButton rpt = (LinkButton)e.Item.FindControl("cmdTemplates");
            if(rpt != null)
            {
                rpt.Attributes.Add("onclick", "javascript:openNewWindow('na', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceComponentJobFormMapList.aspx?componentjobid=" + drv["FLDCOMPONENTJOBID"] + "&VESSELID=" + drv["FLDVESSELID"] + "',false); ");
            }
            ImageButton att = (ImageButton)e.Item.FindControl("cmdAttachments");
            if (att != null)
            {
                att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"] + "&mod=PLANNEDMAINTENANCE&u=n'); return false;");
            }
            LinkButton sp = (LinkButton)e.Item.FindControl("cmdParts");
            if (sp != null)
            {
                sp.Attributes.Add("onclick", "javascript:openNewWindow('na', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceJobSummaryComponentJobPartsRequired.aspx?COMPONENTJOBID=" + drv["FLDCOMPONENTJOBID"] + "&VESSELID=" + drv["FLDVESSELID"] + "',false); ");
            }

        }
    }
    protected void menuPms_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

        }
    private void ShowExcel()
    {
        try
        {
            if (ViewState["JOBSUMMARYID"] == null)
            {
                return;
            }
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDVESSELNAME", "FLDCOMPONENT", "FLDJOB", "FLDQUICKNAME", "FLDFREQUENCYNAME", "FLDJOBLASTDONEDATE", "FLDJOBNEXTDUEDATE" };
            string[] alCaptions = {"Vessel", "Component", "Job", "Category", "Frequency", "Last Done", "Next Due" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 25;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds;

            ds = PhoenixPlannedMaintenanceCompJobSummary.CompjobSummaryDetailSearch(new Guid(ViewState["JOBSUMMARYID"].ToString()), ViewState["VESSELLIST"].ToString()
                                                                , sortexpression, sortdirection
                                                                , GvPMS.CurrentPageIndex + 1
                                                                , GvPMS.VirtualItemCount, ref iRowCount
                                                                , ref iTotalPageCount
                                                                , ViewState["COMPONENT"].ToString()
                                                                , ViewState["JOB"].ToString()
                                                                , General.GetNullableInteger(ViewState["CATEGORY"].ToString())
                                                                , General.GetNullableDateTime(ViewState["FROM"].ToString())
                                                                , General.GetNullableDateTime(ViewState["TO"].ToString())
                                                                , ViewState["VESSEL"].ToString());


            General.ShowExcel("Job Summary", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void GvPMS_PreRender(object sender, EventArgs e)
    {
        
        foreach (GridDataItem dataItem in GvPMS.MasterTableView.Items)
        {
            
            int previousItemIndex = dataItem.ItemIndex - 1;

            if (previousItemIndex >= 0)
            {
                string current = ((RadLabel)dataItem["FLDVESSELNAME"].FindControl("lblVessel")).Text;
                string prev = ((RadLabel)dataItem.OwnerTableView.Items[previousItemIndex]["FLDVESSELNAME"].FindControl("lblVessel")).Text;

                string currentComp = ((RadLabel)dataItem["FLDCOMPONENT"].FindControl("lblCompNo")).Text;
                string prevComp = ((RadLabel)dataItem.OwnerTableView.Items[previousItemIndex]["FLDCOMPONENT"].FindControl("lblCompNo")).Text;

                if (current == prev)
                {
                    int tempindex = previousItemIndex;
                    int cnt = 1;
                    dataItem["FLDVESSELNAME"].Visible = false;
                    while (tempindex >= 0)
                    {
                        string tempprev = ((RadLabel)dataItem.OwnerTableView.Items[tempindex]["FLDVESSELNAME"].FindControl("lblVessel")).Text;
                        if (current != tempprev) break;
                        cnt++;
                        tempindex--;
                    }
                    dataItem.OwnerTableView.Items[tempindex + 1]["FLDVESSELNAME"].RowSpan = cnt;
                }

                if (currentComp == prevComp)
                {
                    int tempindex = previousItemIndex;
                    int cnt = 1;
                    dataItem["FLDCOMPONENT"].Visible = false;
                    while (tempindex >= 0)
                    {
                        string tempprev = ((RadLabel)dataItem.OwnerTableView.Items[tempindex]["FLDCOMPONENT"].FindControl("lblCompNo")).Text;
                        if (currentComp != tempprev) break;
                        cnt++;
                        tempindex--;
                    }
                    dataItem.OwnerTableView.Items[tempindex + 1]["FLDCOMPONENT"].RowSpan = cnt;
                }
            }

        }
    }
    protected void cblJobCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        GvPMS.CurrentPageIndex = 0;

        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDCATEGORY").CurrentFilterValue = e.Value;
        ViewState["CATEGORY"] = e.Value;

        GvPMS.Rebind();
    }

    protected void cblJobCategory_DataBinding(object sender, EventArgs e)
    {
        RadComboBox jobCategory = sender as RadComboBox;
        jobCategory.DataSource = PhoenixRegistersQuick.ListQuick(1, 165);
        jobCategory.DataTextField = "FLDQUICKNAME";
        jobCategory.DataValueField = "FLDQUICKCODE";

        jobCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }

    protected void GvPMS_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.FilterCommandName)
        {
            ViewState["VESSEL"] = GvPMS.MasterTableView.GetColumn("FLDVESSELNAME").CurrentFilterValue.ToString();
            ViewState["COMPONENT"] = GvPMS.MasterTableView.GetColumn("FLDCOMPONENT").CurrentFilterValue.ToString();
            ViewState["JOB"] = GvPMS.MasterTableView.GetColumn("FLDJOB").CurrentFilterValue.ToString();
            //ViewState["CATEGORY"] = GvPMS.MasterTableView.GetColumn("FLDWORKORDERNAME").CurrentFilterValue.ToString();
            string daterange = GvPMS.MasterTableView.GetColumn("FLDJOBNEXTDUEDATE").CurrentFilterValue.ToString();
            if (daterange != "")
            {
                ViewState["FROM"] = daterange.Split('~')[0];
                ViewState["TO"] = daterange.Split('~')[1];
            }
            GvPMS.Rebind();
        }
    }

    protected void GvPMS_SortCommand(object sender, GridSortCommandEventArgs e)
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
}