using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using SouthNests.Phoenix.Inspection;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Text;

public partial class PlannedMaintenanceDailyWorkPlanDetailActivity : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceDailyWorkPlanDetailActivity.aspx?" + Request.QueryString.ToString(), "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuActivity.AccessRights = this.ViewState;
            MenuActivity.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                ViewState["PLANID"] = Request.QueryString["p"] ?? string.Empty;
                ViewState["PROCESS"] = string.Empty;
                ViewState["ACTIVITY"] = string.Empty;
                PopuldateElement();
                gvActivty.PageSize = General.ShowRecords(null);
                //MenuDivWorkOrder.SelectedMenuIndex = 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuActivity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string csvActivity = GetSelectedActivity();
                if (!IsValidActivity(csvActivity))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixPlannedMaintenanceDailyWorkPlan.InsertFrequentlyUsedActivty(PhoenixSecurityContext.CurrentSecurityContext.VesselID, csvActivity);
                string script = "function f(){CloseUrlModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["PROCESS"] = string.Empty;
                ViewState["ACTIVITY"] = string.Empty;
                gvActivty.CurrentPageIndex = 0;
                gvActivty.MasterTableView.GetColumn("FLDELEMENTNAME").CurrentFilterValue = "";
                gvActivty.MasterTableView.GetColumn("FLDACTIVITYNAME").CurrentFilterValue = "";
                gvActivty.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
                ButtonListItem itm = new ButtonListItem(item.Text + " - " + dr["FLDNAME"].ToString(), dr["FLDACTIVITYID"].ToString() + "~" + dr["FLDCATEGORYID"].ToString());
                cblActivity.Items.Add(itm);
            }
        }
    }
    protected void btnCreate_Click(object sender, EventArgs e)
    {

        try
        {
            string csvActivity = string.Empty;
            foreach (var item in cblActivity.SelectedItems)
            {
                csvActivity = csvActivity + item.Value + ",";
            }
            csvActivity = csvActivity.Trim(',');
            if (!IsValidActivity(csvActivity))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixPlannedMaintenanceDailyWorkPlan.InsertActivity(General.GetNullableGuid(ViewState["PLANID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , csvActivity);
            cblActivity.DataSource = null;
            cblActivity.Items.Clear();
            foreach (var item in cblElement.SelectedItems)
            {
                item.Selected = false;
            }

            string script = "function f(){CloseUrlModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

        }
        catch (Exception ex)
        {
            ucError.Visible = true;
            ucError.ErrorMessage = ex.Message;
        }

    }
    private bool IsValidActivity(string activity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(activity) == null)
            ucError.ErrorMessage = "Select atleast one activity";

        return (!ucError.IsError);
    }
    private DataTable GetActivity(int ElementId)
    {
        DataSet ds = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentActivity(ElementId);
        return ds.Tables[0];
    }
    private void PopuldateElement()
    {
        DataTable dt = PhoenixInspectionRiskAssessmentCategoryExtn.ListDailyWorkPlanProcess(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                cblElement.Items.Add(new ButtonListItem(dr["FLDNAME"].ToString(), dr["FLDCATEGORYID"].ToString()));
            }
        }
    }

    protected void gvActivty_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDWORKORDERNUMBER", "FLDJOBCATEGORY", "FLDPLANNINGDUEDATE", "FLDDURATION", "FLDDISCIPLINENAME", "FLDSTATUS" };
        string[] alCaptions = { "Work Order No.", "Category", "Planned Date", "Duration", "Assigned To", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataTable dt = PhoenixPlannedMaintenanceDailyWorkPlan.SearchFrequentlyUsedActivty(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , null
                                                                            , sortexpression, sortdirection
                                                                            , gvActivty.CurrentPageIndex + 1
                                                                            , gvActivty.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount
                                                                            , ViewState["PROCESS"].ToString()
                                                                            , ViewState["ACTIVITY"].ToString());
        //DataSet ds = new DataSet();
        //ds.Tables.Add(dt.Copy());
        //General.SetPrintOptions("gvActivty", "Activity", alCaptions, alColumns, ds);

        gvActivty.DataSource = dt;
        gvActivty.VirtualItemCount = iRowCount;
    }
    private string GetSelectedActivity()
    {
        StringBuilder strlist = new StringBuilder();
        strlist.Append(",");
        if (gvActivty.Items.Count > 0)
        {
            foreach (GridDataItem gv in gvActivty.SelectedItems)
            {
                strlist.Append(gv.GetDataKeyValue("FLDDAILYPLANACTIVITYID").ToString() + ", ");
            }
        }
        if (strlist.Length == 1)
        {
            return "";
        }
        return strlist.ToString();
    }


    protected void gvActivty_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid grid = (RadGrid)sender;
            if (e.CommandName == RadGrid.FilterCommandName)
            {
                grid.CurrentPageIndex = 0;

                ViewState["PROCESS"] = grid.MasterTableView.GetColumn("FLDELEMENTNAME").CurrentFilterValue;
                ViewState["ACTIVITY"] = grid.MasterTableView.GetColumn("FLDACTIVITYNAME").CurrentFilterValue;                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
