using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI;
using System.Text;
using SouthNests.Phoenix.Registers;

public partial class PlannedMaintenanceJobsLink : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Close", "CLOSE", ToolBarDirection.Right);
            toolbarmain.AddButton("Save", "ADDJOBS", ToolBarDirection.Right);
            toolbarmain.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobsLink.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            //toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
            MenuWorkOrderRequestion.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                ViewState["COMPONENTID"] = null;
                ViewState["WORKORDERID"] = null;

                ViewState["MDUE"] = string.Empty;
                ViewState["GROUPID"] = null;

                ViewState["compCategoryId"] = null;
                ViewState["Discipline"] = null;
                ViewState["frequencytype"] = null;
                ViewState["frequency"] = null;

                ViewState["CompNoFilter"] = string.Empty;
                ViewState["CompNameFilter"] = string.Empty;
                ViewState["CompCategoryFilter"] = string.Empty;
                ViewState["JobCategoryFilter"] = string.Empty;
                ViewState["filterDiscipline"] = string.Empty;
                ViewState["FilterFrequencyType"] = string.Empty;
                ViewState["FilterFromDate"] = string.Empty;
                ViewState["FilterToDate"] = string.Empty;

                if (Request.QueryString["COMPONENT"] != null && Request.QueryString["COMPONENT"].ToString() != "")
                    ViewState["CompNameFilter"] = Request.QueryString["COMPONENT"].ToString();

                if (Request.QueryString["DefectId"] != null && Request.QueryString["DefectId"].ToString() != "")
                    ViewState["DefectId"] = Request.QueryString["DefectId"].ToString();

                if (Request.QueryString["Luboilid"] != null && Request.QueryString["Luboilid"].ToString() != "")
                {
                    ViewState["LUBOILID"] = Request.QueryString["Luboilid"].ToString();
                }



                gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

            //ds = PhoenixPlannedMaintenanceDefectJob.UnplannedWorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
            //        , ViewState["CompNoFilter"].ToString() != string.Empty ? ViewState["CompNoFilter"].ToString() : null
            //        , ViewState["CompNameFilter"].ToString() != string.Empty ? ViewState["CompNameFilter"].ToString() : null
            //        , General.GetNullableDateTime(ViewState["FilterFromDate"].ToString())
            //        , General.GetNullableDateTime(ViewState["FilterToDate"].ToString())
            //        , null, null
            //        , ViewState["Discipline"] != null ? General.GetNullableInteger(ViewState["Discipline"].ToString()) : General.GetNullableInteger(ViewState["filterDiscipline"].ToString())
            //        , ViewState["compCategoryId"] != null ? General.GetNullableInteger(ViewState["compCategoryId"].ToString()) : General.GetNullableInteger(ViewState["CompCategoryFilter"].ToString())
            //        , sortexpression, sortdirection
            //        , gvWorkOrder.CurrentPageIndex + 1
            //        , gvWorkOrder.PageSize
            //        , ref iRowCount
            //        , ref iTotalPageCount
            //        , ViewState["frequency"] != null ? General.GetNullableInteger(ViewState["frequency"].ToString()) : null
            //        , ViewState["frequencytype"] != null ? General.GetNullableInteger(ViewState["frequencytype"].ToString()) : General.GetNullableInteger(ViewState["FilterFrequencyType"].ToString())
            //        , 0
            //        , ViewState["JobCategoryFilter"].ToString()
            //        , General.GetNullableGuid(ViewState["DefectId"].ToString()));

            ds = PhoenixPlannedMaintenanceLubOilStoreAnalysis.Luboiljobssearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                  , Guid.Parse(ViewState["LUBOILID"].ToString())
                  , null
                  , null
                  , null
                  , sortexpression, sortdirection
                  , gvWorkOrder.CurrentPageIndex + 1
                  , gvWorkOrder.PageSize
                  , ref iRowCount
                  , ref iTotalPageCount
                   );

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
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkOrder.CurrentPageIndex + 1;
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
        //BindData();
        //gvWorkOrder.Rebind();
    }


    protected void MenuWorkOrderRequestion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            String scriptClosePopup = String.Format("javascript:fnReloadList('codehelp1');");

            String scriptKeepPopupOpen = String.Format("javascript:fnReloadList('codehelp1', null, 'yes');");

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["compCategoryId"] = null;
                ViewState["Discipline"] = null;
                ViewState["frequencytype"] = null;
                ViewState["frequency"] = null;
                ViewState["filterDiscipline"] = string.Empty;
                ViewState["FilterFrequencyType"] = string.Empty;
                ViewState["CompCategoryFilter"] = string.Empty;
                ViewState["CompNameFilter"] = string.Empty;
                ViewState["CompNoFilter"] = string.Empty;
                ViewState["FilterFromDate"] = string.Empty;
                ViewState["FilterToDate"] = string.Empty;
                ViewState["JobCategoryFilter"] = string.Empty;

                ViewState["PAGENUMBER"] = 1;

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

                gvWorkOrder.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLOSE"))
            {

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                 "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
            else if (CommandName.ToUpper().Equals("ADDJOBS"))
            {

                string source = GetAssignedTo();
                PhoenixPlannedMaintenanceLubOilStoreAnalysis.LuboilStoreJOBSUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(Request.QueryString["Luboilid"].ToString()), source);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptClosePopup, true);


            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private string GetAssignedTo()
    {
        string list = string.Empty;

        foreach (GridDataItem gvr in gvWorkOrder.Items)
        {
            CheckBox chk = (CheckBox)gvr.FindControl("chkSelect");
            RadLabel lblWorkOrderId = (RadLabel)gvr.FindControl("lblWorkOrderId");

            if (chk != null && lblWorkOrderId != null)
            {
                if (chk.Checked == true)
                {
                    list = list + lblWorkOrderId.Text.Trim() + ",";
                }
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {


            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName == RadGrid.FilterCommandName)
            {
                GridFilteringItem filterItem = (GridFilteringItem)e.Item;
                ViewState["CompNoFilter"] = gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue.ToString();
                ViewState["CompNameFilter"] = gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue.ToString();

                //string daterange = gvWorkOrder.MasterTableView.GetColumn("FLDPLANNINGDUEDATE").CurrentFilterValue.ToString();
                //if (daterange != string.Empty)
                //{
                //    ViewState["FilterFromDate"] = daterange.Split('~')[0];
                //    ViewState["FilterToDate"] = daterange.Split('~')[1];
                //}
                gvWorkOrder.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("MAP"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (General.GetNullableGuid(ViewState["DefectId"].ToString()) != null)
                    PhoenixPlannedMaintenanceDefectJob.MapJobstoDefectList(new Guid(ViewState["DefectId"].ToString()), new Guid(item.GetDataKeyValue("FLDWORKORDERID").ToString()));

                gvWorkOrder.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            e.Canceled = true;
        }
    }

    protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            CheckBox ddl = (CheckBox)e.Item.FindControl("chkSelect");

            if (drv["FLDALREADYEXIST"].ToString() == "1")
            {
                ddl.Checked = true;
            }
            else
            {
                ddl.Checked = false;
            }
        }
    }

    protected void cblJobCategory_DataBinding(object sender, EventArgs e)
    {
        RadComboBox jobCategory = sender as RadComboBox;
        jobCategory.DataSource = PhoenixRegistersQuick.ListQuick(1, 165);
        jobCategory.DataTextField = "FLDQUICKNAME";
        jobCategory.DataValueField = "FLDQUICKCODE";

        jobCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }

    protected void cblJobCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvWorkOrder.CurrentPageIndex = 0;

        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDJOBCATEGORYID").CurrentFilterValue = e.Value;
        ViewState["JobCategoryFilter"] = e.Value;

        gvWorkOrder.Rebind();

    }


    protected void cblFrequencyType_DataBinding(object sender, EventArgs e)
    {
        RadComboBox frequency = sender as RadComboBox;
        frequency.DataSource = PhoenixRegistersHard.ListHard(1, 7);
        frequency.DataTextField = "FLDHARDNAME";
        frequency.DataValueField = "FLDHARDCODE";

        frequency.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }

    protected void cblFrequencyType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvWorkOrder.CurrentPageIndex = 0;

        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDFREQUENCYTYPE").CurrentFilterValue = e.Value;
        ViewState["FilterFrequencyType"] = e.Value;

        gvWorkOrder.Rebind();
    }

}
