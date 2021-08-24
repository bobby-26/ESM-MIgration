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
using System.Text;
using SouthNests.Phoenix.Registers;

public partial class PlannedMaintenanceJobListForPlanWorkOrder : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Close", "CLOSE", ToolBarDirection.Right);
            toolbarmain.AddLinkButton("javascript:showDialog();", "Create Work Order", "ADD", ToolBarDirection.Right);
            toolbarmain.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobListForPlanWorkOrder.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            //toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
            MenuWorkOrderRequestion.MenuList = toolbarmain.Show();

            PhoenixToolbar toolWoCreate = new PhoenixToolbar();
            toolWoCreate.AddButton("Save", "SAVE", ToolBarDirection.Right);

            menuWorkorderCreate.MenuList = toolWoCreate.Show();


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

                txtTitle.Text = "";
                ddlResponsible.SelectedDiscipline = "";

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

            ds = PhoenixPlannedMaintenanceWorkOrderGroup.UnplannedWorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    , ViewState["CompNoFilter"].ToString() != string.Empty ? ViewState["CompNoFilter"].ToString() : null
                    , ViewState["CompNameFilter"].ToString() != string.Empty ? ViewState["CompNameFilter"].ToString() : null
                    , General.GetNullableDateTime(ViewState["FilterFromDate"].ToString())
                    , General.GetNullableDateTime(ViewState["FilterToDate"].ToString())
                    , null, null
                    , ViewState["Discipline"] != null ? General.GetNullableInteger(ViewState["Discipline"].ToString()) : General.GetNullableInteger(ViewState["filterDiscipline"].ToString())
                    , ViewState["compCategoryId"] != null ? General.GetNullableInteger(ViewState["compCategoryId"].ToString()) : General.GetNullableInteger(ViewState["CompCategoryFilter"].ToString())
                    , sortexpression, sortdirection
                    , gvWorkOrder.CurrentPageIndex + 1
                    , gvWorkOrder.PageSize
                    , ref iRowCount
                    , ref iTotalPageCount
                    , ViewState["frequency"] != null ? General.GetNullableInteger(ViewState["frequency"].ToString()) : null
                    , ViewState["frequencytype"] != null ? General.GetNullableInteger(ViewState["frequencytype"].ToString()) : General.GetNullableInteger(ViewState["FilterFrequencyType"].ToString())
                    , 0
                    , ViewState["JobCategoryFilter"].ToString());

            General.SetPrintOptions("gvWorkOrder", "Work Order", alCaptions, alColumns, ds);

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



    protected void MenuWorkOrderRequestion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

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

                string refreshname = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                            "BookMarkScript", "top.closeTelerikWindow('codehelp1'," + (refreshname == string.Empty ? "null" : "'" + refreshname + "'") + ");", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["compCategoryId"] = ((RadLabel)e.Item.FindControl("lblCompCategory")).Text;
            ViewState["Discipline"] = ((RadLabel)e.Item.FindControl("lblDisplineId")).Text;
            ViewState["frequencytype"] = ((RadLabel)e.Item.FindControl("lblFrequencyType")).Text;
            ViewState["frequency"] = ((RadLabel)e.Item.FindControl("lblFrequency")).Text;

            //BindData();
            gvWorkOrder.Rebind();
        }
        else if (e.CommandName == RadGrid.FilterCommandName)
        {
            GridFilteringItem filterItem = (GridFilteringItem)e.Item;
            ViewState["CompNoFilter"] = gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue.ToString();
            ViewState["CompNameFilter"] = gvWorkOrder.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue.ToString();

            string daterange = gvWorkOrder.MasterTableView.GetColumn("FLDPLANNINGDUEDATE").CurrentFilterValue.ToString();
            if (daterange != string.Empty)
            {
                ViewState["FilterFromDate"] = daterange.Split('~')[0];
                ViewState["FilterToDate"] = daterange.Split('~')[1];
            }
            gvWorkOrder.Rebind();
        }

    }

    protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }

    protected void cblCompCategory_DataBinding(object sender, EventArgs e)
    {
        RadComboBox compCategory = sender as RadComboBox;
        compCategory.DataSource = PhoenixRegistersQuick.ListQuick(1, 166);
        compCategory.DataTextField = "FLDQUICKNAME";
        compCategory.DataValueField = "FLDQUICKCODE";

        compCategory.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }
    protected void cblCompCategory_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvWorkOrder.CurrentPageIndex = 0;
        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDCOMPCATEGORY").CurrentFilterValue = e.Value;
        //filterItem.FireCommandEvent("Filter", new Pair("EqualTo", "OrderIDServer"));

        //UserControlQuick compCategory = sender as UserControlQuick;
        ViewState["CompCategoryFilter"] = e.Value;

        gvWorkOrder.Rebind();
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

    protected void cblDiscipline_DataBinding(object sender, EventArgs e)
    {
        RadComboBox discipline = sender as RadComboBox;
        discipline.DataSource = PhoenixRegistersDiscipline.ListDiscipline();
        discipline.DataTextField = "FLDDISCIPLINENAME";
        discipline.DataValueField = "FLDDISCIPLINEID";

        discipline.Items.Insert(0, new RadComboBoxItem("All", string.Empty));
    }

    protected void cblDiscipline_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvWorkOrder.CurrentPageIndex = 0;

        GridFilteringItem filterItem = (sender as RadComboBox).NamingContainer as GridFilteringItem;
        filterItem.OwnerTableView.GetColumn("FLDPLANNINGDISCIPLINE").CurrentFilterValue = e.Value;
        ViewState["filterDiscipline"] = e.Value;

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

            if(string.IsNullOrEmpty(PlannedDate))
            {
               RequiredFieldValidator validator = new RequiredFieldValidator();
                validator.ErrorMessage = "Planned date is required";
                validator.IsValid = false;
                validator.Visible = false;
                Page.Form.Controls.Add(validator);
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
                    //PhoenixPlannedMaintenanceWorkOrderGroup.GroupDetailUpdate(groupId.Value, txtTitle.Text, txtDueDate.SelectedDate, General.GetNullableInteger(ddlResponsible.SelectedDiscipline), null, null);
                    ViewState["GROUPID"] = groupId.ToString();
                    PhoenixPlannedMaintenanceWorkOrderGroup.RefreshToolboxMeet(new Guid(ViewState["GROUPID"].ToString()));
                    gvWorkOrder.Rebind();
                    string script = "function f(){CloseModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
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
