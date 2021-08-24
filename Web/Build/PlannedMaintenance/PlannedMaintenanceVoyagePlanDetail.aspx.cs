using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenance_PlannedMaintenanceVoyagePlanDetail : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVoyagePlanDetail.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvVoyagePlan')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");        
        MenuPartB.Title = "Part B: Planned Maintenance - Engine";
        MenuPartB.AccessRights = this.ViewState;
        MenuPartB.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVoyagePlanDetail.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMPartC')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuPartC.Title = "Part C: Planned Maintenance - Deck";
        MenuPartC.AccessRights = this.ViewState;
        MenuPartC.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceVoyagePlanDetail.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvActivity')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:showDialog();", "Add Operations", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MenuPartA.Title = "Part A: Operations";
        MenuPartA.AccessRights = this.ViewState;
        MenuPartA.MenuList = toolbar.Show();        

        if (!IsPostBack)
        {
            ViewState["PAGENUMBERA"] = 1;
            ViewState["SORTEXPRESSIONA"] = null;
            ViewState["SORTDIRECTIONA"] = null;

            ViewState["PAGENUMBER"] = 1;           
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            ViewState["PAGENUMBERC"] = 1;
            ViewState["SORTEXPRESSIONC"] = null;
            ViewState["SORTDIRECTIONC"] = null;
            ViewState["PLANID"] = Request.QueryString["p"];
            ViewState["PROCESSID"] = string.Empty;
            DataSet ds = PhoenixRegistersHard.EditHardCode(1, 51, "DEK");
            if(ds.Tables.Count>0 && ds.Tables[0].Rows.Count>0)
            {
                ViewState["DEK"] = ds.Tables[0].Rows[0]["FLDHARDCODE"].ToString();
            }
            ds = PhoenixRegistersHard.EditHardCode(1, 51, "ENG");
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ViewState["ENG"] = ds.Tables[0].Rows[0]["FLDHARDCODE"].ToString();
            }
            if (!string.IsNullOrEmpty(Request.QueryString["eid"]))
            {
                ViewState["PROCESSID"] = Request.QueryString["eid"];
            }
            gvVoyagePlan.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvMPartC.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvActivity.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            SetHeading();
            PopuldateElement();
        }
    }
    protected void MenuPartA_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLEAR"))
            {

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
    protected void MenuPartB_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLEAR"))
            {

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
    protected void MenuPartC_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLEAR"))
            {

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
    protected void MainMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("~/PlannedMaintenance/PlannedMaintenanceVoyagePlan.aspx");
            }
            else if (CommandName.ToUpper().Equals("ISSUE"))
            {
                PhoenixPlannedMaintenanceVoyagePlan.Issue(new Guid(ViewState["PLANID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                SetHeading();
                ucStatus.Text = "Plan Issued Successfully";
            }
            else if (CommandName.ToUpper().Equals("REFRESH"))
            {
                PhoenixPlannedMaintenanceVoyagePlan.InsertWorkOrder(new Guid(ViewState["PLANID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                BindData();
                gvVoyagePlan.Rebind();
                BindDataPartC();
                gvMPartC.Rebind();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDWORKORDERNUMBER", "FLDCATEGORY", "FLDPLANNINGDUEDATE", "FLDPLANINGDURATIONINDAYS", "FLDPLANNINGESTIMETDURATION", "FLDDISCIPLINENAME", "FLDSTATUS" };
        string[] alCaptions = { "Work Order No.", "Category", "Planned Date", "Duration Days", "Duration Hours", "Assigned To", "Status" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixPlannedMaintenanceVoyagePlan.SearchWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                        , General.GetNullableGuid(ViewState["PLANID"].ToString())
                                        , General.GetNullableInteger(ViewState["ENG"].ToString())
                                         , sortexpression, sortdirection
                                         , 1
                                         , iRowCount
                                         , ref iRowCount
                                         , ref iTotalPageCount);

        General.ShowExcel("Part B: Maintenance - Engine", dt, alColumns, alCaptions, null, null);
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDCATEGORY", "FLDPLANNINGDUEDATE", "FLDPLANINGDURATIONINDAYS", "FLDPLANNINGESTIMETDURATION", "FLDDISCIPLINENAME", "FLDSTATUS" };
            string[] alCaptions = { "Work Order No.", "Category", "Planned Date", "Duration Days", "Duration Hours", "Assigned To", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixPlannedMaintenanceVoyagePlan.SearchWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                        , General.GetNullableGuid(ViewState["PLANID"].ToString())
                                        , General.GetNullableInteger(ViewState["ENG"].ToString())
                                         , sortexpression, sortdirection
                                         , int.Parse(ViewState["PAGENUMBER"].ToString())
                                         , gvVoyagePlan.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvVoyagePlan", "Part B: Maintenance - Engine", alCaptions, alColumns, ds);

            gvVoyagePlan.DataSource = dt;
            gvVoyagePlan.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataPartA()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDELEMENT", "FLACTIVITY" };
            string[] alCaptions = { "Element", "Activity" };

            string sortexpression = (ViewState["SORTEXPRESSIONA"] == null) ? null : (ViewState["SORTEXPRESSIONA"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONA"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONA"].ToString());

            DataTable dt = PhoenixPlannedMaintenanceVoyagePlan.SearchActivty(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                        , General.GetNullableGuid(ViewState["PLANID"].ToString())
                                        , General.GetNullableInteger(ViewState["PROCESSID"].ToString())
                                         , sortexpression, sortdirection
                                         , int.Parse(ViewState["PAGENUMBERA"].ToString())
                                         , gvActivity.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvActivity", "Part A: Operations", alCaptions, alColumns, ds);

            gvActivity.DataSource = dt;
            gvActivity.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNTA"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataPartC()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDWORKORDERNUMBER", "FLDCATEGORY", "FLDPLANNINGDUEDATE", "FLDPLANINGDURATIONINDAYS", "FLDPLANNINGESTIMETDURATION", "FLDDISCIPLINENAME", "FLDSTATUS" };
            string[] alCaptions = { "Work Order No.", "Category", "Planned Date", "Duration Days", "Duration Hours", "Assigned To", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSIONC"] == null) ? null : (ViewState["SORTEXPRESSIONC"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTIONC"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTIONC"].ToString());

            DataTable dt = PhoenixPlannedMaintenanceVoyagePlan.SearchWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                        , General.GetNullableGuid(ViewState["PLANID"].ToString())
                                        , General.GetNullableInteger(ViewState["DEK"].ToString())
                                         , sortexpression, sortdirection
                                         , int.Parse(ViewState["PAGENUMBERC"].ToString())
                                         , gvMPartC.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvMPartC", "Part C: Maintenance - Deck", alCaptions, alColumns, ds);

            gvMPartC.DataSource = dt;
            gvMPartC.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNTC"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVoyagePlan_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "PAGE")
            {
                ViewState["PAGENUMBER"] = null;
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVoyagePlan_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;           
            if (e.Item.ItemType == GridItemType.EditItem)
            {

            }
            if (e.Item.ItemType == GridItemType.Footer)
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVoyagePlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVoyagePlan.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMPartC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERC"] = ViewState["PAGENUMBERC"] != null ? ViewState["PAGENUMBERC"] : gvMPartC.CurrentPageIndex + 1;
            BindDataPartC();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvVoyagePlan_SortCommand(object sender, GridSortCommandEventArgs e)
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
    protected void gvMPartC_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSIONC"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTIONC"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTIONC"] = "1";
                break;
        }
    }
        
    private void SetHeading()
    {
        string heading = "Voyage No. : {no} | Port : {port} | Date From: {from} | Date To : {to} | Status : {status}  | Rev. No. : {revno}";
        DataTable dt = PhoenixPlannedMaintenanceVoyagePlan.Edit(new Guid(ViewState["PLANID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if(dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            heading = heading.Replace("{no}", dr["FLDVOYAGENUMBER"].ToString())
                                .Replace("{port}", dr["FLDVOYAGETITLE"].ToString())
                                .Replace("{from}", General.GetDateTimeToString(dr["FLDFROMDATE"].ToString()))
                                .Replace("{to}", General.GetDateTimeToString(dr["FLDTODATE"].ToString()))
                                .Replace("{status}", dr["FLDSTATUS"].ToString())
                                .Replace("{revno}", dr["FLDREVISIONNUMBER"].ToString());

            PhoenixToolbar toolbar = new PhoenixToolbar();
            if (dr["FLDISSUED"].ToString() == "1")
            {
                toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            }
            else
            {
                toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
                toolbar.AddButton("Issue", "ISSUE", ToolBarDirection.Right);
                toolbar.AddButton("Refresh", "REFRESH", ToolBarDirection.Right);
            }
            MenuMain.AccessRights = this.ViewState;
            MenuMain.MenuList = toolbar.Show();
        }
        lblHeading.Text = heading;        
    }

    protected void gvActivity_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item.ItemType == GridItemType.EditItem)
            {

            }
            if (e.Item.ItemType == GridItemType.Footer)
            {
                                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvActivity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBERA"] = ViewState["PAGENUMBERA"] != null ? ViewState["PAGENUMBERA"] : gvMPartC.CurrentPageIndex + 1;
            BindDataPartA();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvActivity_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper() == "PAGE")
        {
            ViewState["PAGENUMBERA"] = null;
        }
        else if (e.CommandName.ToUpper() == "ADD")
        {
            string element = ((UserControlRACategory)e.Item.FindControl("ddlElement")).SelectedCategory;
            var collection = ((RadComboBox)e.Item.FindControl("ddlActivity")).CheckedItems;
            string csvActivity = string.Empty;
            if (collection.Count != 0)
            {
                csvActivity = ",";

                foreach (var item in collection)
                    csvActivity = csvActivity + item.Value + ",";
            }
            if (!(IsValidActivity(element,csvActivity)))
            {
                ucError.Visible = true;                
                return;
            }
            //PhoenixPlannedMaintenanceVoyagePlan.InsertActivity(new Guid(ViewState["PLANID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID
            //    , int.Parse(element), csvActivity);
            BindDataPartA();
            gvActivity.Rebind();
        }
    }

    protected void gvActivity_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSIONA"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTIONA"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTIONA"] = "1";
                break;
        }
    }
    private DataTable GetActivity(int ElementId)
    {
        string companyid = string.Empty;
        NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
        if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
        {
            companyid = nvc.Get("QMS");
        }
        DataSet ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(ElementId, General.GetNullableInteger(companyid));
        return ds.Tables[0];
    }

    protected void ddlElement_TextChangedEvent(object sender, EventArgs e)
    {
        UserControlRACategory category = sender as UserControlRACategory;
        GridFooterItem item = (GridFooterItem)category.NamingContainer;
        if (General.GetNullableInteger(category.SelectedCategory).HasValue)
        {
            RadComboBox activity = (RadComboBox)item.FindControl("ddlActivity");
            activity.DataSource = GetActivity(int.Parse(category.SelectedCategory));
            activity.DataBind();
        }
    }
    private bool IsValidActivity(string elementid, string csvActivity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (csvActivity.Trim().Equals(""))
            ucError.ErrorMessage = "Activity is required.";

        if (General.GetNullableInteger(elementid) == null)
            ucError.ErrorMessage = "Element is required.";        

        return (!ucError.IsError);
    }
    protected void RadAjaxPanel2_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        string id = e.Argument;
       
    }
    private void PopuldateElement()
    {
        DataSet ds = PhoenixInspectionRiskAssessmentCategory.ListRiskAssessmentCategory();
        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            foreach(DataRow dr in dt.Rows)
            {
                cblElement.Items.Add(new ButtonListItem(dr["FLDNAME"].ToString(), dr["FLDCATEGORYID"].ToString()));
            }
        }
    }

    protected void cblElement_SelectedIndexChanged(object sender, EventArgs e)
    {
        cblActivity.DataSource = null;
        cblActivity.Items.Clear();
        foreach (var item in cblElement.SelectedItems)
        {
            DataTable dt = GetActivity(int.Parse(item.Value));
            foreach (DataRow dr in dt.Rows)
            {
                ButtonListItem itm = new ButtonListItem(dr["FLDNAME"].ToString(), dr["FLDACTIVITYID"].ToString() + "~" + dr["FLDCATEGORYID"].ToString());
                cblActivity.Items.Add(itm);
            }
        }
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                string csvActivity = string.Empty;
                foreach(var item in cblActivity.SelectedItems)
                {
                    csvActivity = csvActivity + item.Value + ",";
                }
                csvActivity = csvActivity.Trim(',');
                PhoenixPlannedMaintenanceVoyagePlan.InsertActivity(new Guid(ViewState["PLANID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , csvActivity);
                cblActivity.DataSource = null;
                cblActivity.Items.Clear();
                foreach (var item in cblElement.SelectedItems)
                {
                    item.Selected = false;
                }
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