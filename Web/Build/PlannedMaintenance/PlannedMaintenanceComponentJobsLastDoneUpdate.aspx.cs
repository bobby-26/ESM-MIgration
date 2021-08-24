using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Collections.Specialized;
using Telerik.Web.UI;
using SouthNests.Phoenix.Registers;
using System.Text;

public partial class PlannedMaintenanceComponentJobsLastDoneUpdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentJobsLastDoneUpdate.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvComponentJob')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuDivComponentJob.AccessRights = this.ViewState;
        MenuDivComponentJob.MenuList = toolbargrid.Show();

        PhoenixToolbar toolbarMain = new PhoenixToolbar();
        toolbarMain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MainMenu.AccessRights = this.ViewState;
        MainMenu.MenuList = toolbarMain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["FLDCOMPONENTNUMBER"] = string.Empty;
            ViewState["COMPONENTNAME"] = string.Empty;
            ViewState["FLDJOBCODE"] = string.Empty;
            ViewState["FLDJOBTITLE"] = string.Empty;
            ViewState["FLDPRIORITY"] = string.Empty;
            ViewState["FLDPRIORITY"] = string.Empty;
            ViewState["FLDJOBCATEGORY"] = string.Empty;
            ViewState["FLDDISCIPLINENAME"] = string.Empty;



            gvComponentJob.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }

    protected void MenuDivComponentJob_TabStripCommand(object sender, EventArgs e)
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
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PAGENUMBER"] = 1;
                Filter.CurrentComponentJobFilter = null;
                ViewState["COMPONENTNAME"] = string.Empty;
                ViewState["FLDCOMPONENTNUMBER"] = string.Empty;
                ViewState["FLDJOBCODE"] = string.Empty;
                ViewState["FLDJOBTITLE"] = string.Empty;
                ViewState["FLDPRIORITY"] = string.Empty;
                ViewState["FLDJOBCATEGORY"] = string.Empty;
                ViewState["FLDDISCIPLINENAME"] = string.Empty;

                BindData();
                gvComponentJob.Rebind();
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
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDJOBLASTDONEDATE", "FLDLASTDONEHOURS", "FLDPRIORITY","FLDDISCIPLINENAME"  };
            string[] alCaptions = { "Component.No", "Component Name", "Job Code", "Job Title", "Frequency", "Last Done date", "Last Done Hour", "Priority","Resp Discipline" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = gvComponentJob.PageSize;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            SetFilter();



            NameValueCollection nvc = Filter.CurrentComponentJobFilter;

            DataTable dt = PhoenixPlannedMaintenanceComponentJob.ComponentJobsLastDoneSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , nvc != null ? nvc.Get("txtCompNumber") : null
                        , nvc != null ? nvc.Get("txtCompName") : null
                        , nvc != null ? nvc.Get("txtJobcode") : null
                       , nvc != null ? nvc.Get("txtJobTitle") : null
                       , General.GetNullableInteger(nvc != null ? nvc["ucDiscipline"] : null)
                        , General.GetNullableInteger(nvc != null ? nvc.Get("txtPriority") : string.Empty)
                        , sortexpression, sortdirection,
                         1,
                        iRowCount,
                        ref iRowCount,
                        ref iTotalPageCount,
                        General.GetNullableInteger(nvc != null ? nvc.Get("chkCancelledjob") : string.Empty),
                       nvc != null ? General.GetNullableString(nvc.Get("txtClasscode")) : null,
                         General.GetNullableInteger(nvc != null ? nvc["txtCategory"] : null), 1

                        );


            General.ShowExcel("Component - Job", dt, alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponentJob_PreRender(object sender, EventArgs e)
    {
        //gvComponentJob.MasterTableView.GetColumn("Activeyn").Visible = false;
        //gvComponentJob.Rebind();
    }
    protected void ddlResponsibility_DataBinding(object sender, EventArgs e)
    {
        RadComboBox ddlDiscipline = sender as RadComboBox;
        ddlDiscipline.DataSource = PhoenixRegistersDiscipline.ListDiscipline();
        ddlDiscipline.DataTextField = "FLDDISCIPLINENAME";
        ddlDiscipline.DataValueField = "FLDDISCIPLINEID";
        ddlDiscipline.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }

    private void SetFilter()
    {

        NameValueCollection nvc = new NameValueCollection();

        if (ViewState["FLDCOMPONENTNUMBER"].ToString() != string.Empty) nvc.Add("txtCompNumber", ViewState["FLDCOMPONENTNUMBER"].ToString());
        if (ViewState["COMPONENTNAME"].ToString() != string.Empty) nvc.Add("txtCompName", ViewState["COMPONENTNAME"].ToString());
        if (ViewState["FLDJOBCODE"].ToString() != string.Empty) nvc.Add("txtJobcode", ViewState["FLDJOBCODE"].ToString());
        if (ViewState["FLDJOBTITLE"].ToString() != string.Empty) nvc.Add("txtJobTitle", ViewState["FLDJOBTITLE"].ToString());
        if (ViewState["FLDPRIORITY"].ToString() != string.Empty) nvc.Add("txtPriority", ViewState["FLDPRIORITY"].ToString());
        if (ViewState["FLDJOBCATEGORY"].ToString() != string.Empty) nvc.Add("txtCategory", ViewState["FLDJOBCATEGORY"].ToString());
        if (ViewState["FLDDISCIPLINENAME"].ToString() != string.Empty) nvc.Add("ucDiscipline", ViewState["FLDDISCIPLINENAME"].ToString());


        Filter.CurrentComponentJobFilter = nvc;

    }
    private void CollapseAllRows()
    {
        foreach (GridItem item in gvComponentJob.MasterTableView.Items)
        {
            item.Expanded = false;
        }
    }

    protected void gvComponentJob_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        // ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvComponentJob.CurrentPageIndex + 1;
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDJOBCODE", "FLDJOBTITLE", "FLDFREQUENCYNAME", "FLDJOBLASTDONEDATE", "FLDLASTDONEHOURS", "FLDPRIORITY", "FLDDISCIPLINENAME" };
        string[] alCaptions = { "Component.No", "Component Name", "Job Code", "Job Title", "Frequency", "Last Done date", "Last Done Hour", "Priority", "Resp Discipline" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        SetFilter();

        NameValueCollection nvc = Filter.CurrentComponentJobFilter;




        DataTable dt = PhoenixPlannedMaintenanceComponentJob.ComponentJobsLastDoneSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , nvc != null ? nvc.Get("txtCompNumber") : null
                        , nvc != null ? nvc.Get("txtCompName") : null
                        , nvc != null ? nvc.Get("txtJobcode") : null
                       , nvc != null ? nvc.Get("txtJobTitle") : null
                       , General.GetNullableInteger(nvc != null ? nvc["ucDiscipline"] : null)
                        , General.GetNullableInteger(nvc != null ? nvc.Get("txtPriority") : string.Empty)
                        , sortexpression, sortdirection,
                         gvComponentJob.CurrentPageIndex + 1,
                        gvComponentJob.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount,
                        General.GetNullableInteger(nvc != null ? nvc.Get("chkCancelledjob") : string.Empty),
                       nvc != null ? General.GetNullableString(nvc.Get("txtClasscode")) : null,
                         General.GetNullableInteger(nvc != null ? nvc["txtCategory"] : null), 1

                        );
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        General.SetPrintOptions("gvComponentJob", "Component - Job", alCaptions, alColumns, ds);

        gvComponentJob.DataSource = ds;
        gvComponentJob.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void cblJobCategory_DataBinding(object sender, EventArgs e)
    {
        RadComboBox jobCategory = sender as RadComboBox;
        jobCategory.DataSource = PhoenixRegistersQuick.ListQuick(1, 165);
        jobCategory.DataTextField = "FLDQUICKNAME";
        jobCategory.DataValueField = "FLDQUICKCODE";

        jobCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }
    protected void gvComponentJob_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void MainMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string lastDonedate = dtDonedate.SelectedDate.ToString();
                if (string.IsNullOrEmpty(lastDonedate))
                {
                    ucError.ErrorMessage = "Enter the last done date";
                    ucError.Visible = true;
                    return;
                }
                if (DateTime.Compare(Convert.ToDateTime(lastDonedate), DateTime.Now.Date) > 0)
                {
                    ucError.ErrorMessage = "Last done should not be later than current date";
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceComponentJob.JobLastDoneUpdate(PhoenixSecurityContext.CurrentSecurityContext.VesselID,null, dtDonedate.SelectedDate);
                RadNotification1.Show("Update successfully..");
                gvComponentJob.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private string GetSelectedJobList()
    {
        StringBuilder strlist = new StringBuilder();
        if (gvComponentJob.Items.Count > 0)
        {
            foreach (GridDataItem gv in gvComponentJob.Items)
            {
                RadLabel lblworkorId = (RadLabel)gv.FindControl("lblComponentJobId");
                strlist.Append(lblworkorId.Text + ",");
            }
        }
        return strlist.ToString();
    }

    protected void gvComponentJob_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == RadGrid.FilterCommandName)
            {

                NameValueCollection nvc = Filter.CurrentComponentJobFilter;
                ViewState["FLDCOMPONENTNUMBER"] = gvComponentJob.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue.ToString();
                if (ViewState["FLDCOMPONENTNUMBER"] != null)
                {
                    nvc.Set("txtCompNumber", ViewState["FLDCOMPONENTNUMBER"].ToString());
                }
                ViewState["COMPONENTNAME"] = gvComponentJob.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue.ToString();
                if (ViewState["COMPONENTNAME"] != null)
                {
                    nvc.Set("txtCompName", ViewState["COMPONENTNAME"].ToString());
                }
                ViewState["FLDJOBCODE"] = gvComponentJob.MasterTableView.GetColumn("FLDJOBCODE").CurrentFilterValue.ToString();
                if (ViewState["FLDJOBCODE"] != null)
                {
                    nvc.Set("txtJobcode", ViewState["FLDJOBCODE"].ToString());
                }
                ViewState["FLDJOBTITLE"] = gvComponentJob.MasterTableView.GetColumn("FLDJOBTITLE").CurrentFilterValue.ToString();
                if (ViewState["FLDJOBTITLE"] != null)
                {
                    nvc.Set("txtJobTitle", ViewState["FLDJOBTITLE"].ToString());
                }
                ViewState["FLDPRIORITY"] = gvComponentJob.MasterTableView.GetColumn("FLDPRIORITY").CurrentFilterValue.ToString();
                if (ViewState["FLDPRIORITY"] != null)
                {
                    nvc.Set("txtPriority", ViewState["FLDPRIORITY"].ToString());
                }
                ViewState["FLDJOBCATEGORY"] = gvComponentJob.MasterTableView.GetColumn("FLDJOBCATEGORY").CurrentFilterValue.ToString();
                if (ViewState["FLDJOBCATEGORY"] != null)
                {
                    nvc.Set("txtCategory", ViewState["FLDJOBCATEGORY"].ToString());
                }
                ViewState["FLDDISCIPLINENAME"] = gvComponentJob.MasterTableView.GetColumn("FLDDISCIPLINENAME").CurrentFilterValue.ToString();
                if (ViewState["FLDDISCIPLINENAME"] != null)
                {
                    nvc.Set("ucDiscipline", ViewState["FLDDISCIPLINENAME"].ToString());
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}