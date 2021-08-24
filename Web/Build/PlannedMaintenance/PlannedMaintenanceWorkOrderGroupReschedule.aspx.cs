using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderGroupReschedule : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        if (Request.QueryString["unplanned"] == "1")
        {
            toolbarmain.AddButton("Cancel WO", "CANCELWO", ToolBarDirection.Right);
            trpostponecancel.Visible = true;
        }
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuPostpone.AccessRights = this.ViewState;
        MenuPostpone.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {

        }
    }

    private bool IsValidRequisition()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtpostponeDate.SelectedDate == null)
            ucError.ErrorMessage = "Reschedule date is required";
        else if (DateTime.Compare(txtpostponeDate.SelectedDate.Value, DateTime.Now) < 0)
        {
            ucError.ErrorMessage = "Reschedule Date should be later than current date";
        }
        if (txtRemarks.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Remarks is required";

        return (!ucError.IsError);
    }

    protected void gvJobList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDPLANNINGDUEDATE", "FLDWORKORDERSTATUS" };
            string[] alCaptions = { "Comp. No", "Comp. Name", "Job No", "Job Title", "Due Date", "Status" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;

            ds = PhoenixPlannedMaintenanceWorkOrderGroup.SubWorkOrderList(new Guid(Request.QueryString["groupId"].ToString()), sortexpression, sortdirection,
                            gvJobList.CurrentPageIndex + 1, gvJobList.PageSize, ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvWorkOrder", "Work Order", alCaptions, alColumns, ds);

            gvJobList.DataSource = ds;
            gvJobList.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;

            txtPlannedDate.SelectedDate = DateTime.Parse(ds.Tables[1].Rows[0]["FLDPLANNINGDUEDATE"].ToString());

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPostpone_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                if (!IsValidRequisition())
                {
                    ucError.Visible = true;
                    return;
                }

                if (Request.QueryString["groupId"] != null)
                {

                    PhoenixPlannedMaintenanceWorkOrderGroup.WorkordergroupReschedule(new Guid(Request.QueryString["groupId"])
                                                                                     , txtpostponeDate.SelectedDate.Value
                                                                                     , txtRemarks.Text);
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                "BookMarkScript", "refresh();", true);
            }
            else if (CommandName.ToUpper().Equals("CANCELWO"))
            {
                string radalertscript = "<script language='javascript'>function f(){callConfirm(); Sys.Application.remove_load(f);}; Sys.Application.add_load(f);</script>";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "radalert", radalertscript);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void RadAjaxManager1_AjaxRequest(object sender, AjaxRequestEventArgs e)
    {
        try
        {
            if (e.Argument.ToString() == "ok")
            {
                string groupId = Request.QueryString["groupId"];
                string id = Request.QueryString["woplanid"];
                if (!string.IsNullOrEmpty(groupId))
                    PhoenixPlannedMaintenanceWorkOrderGroup.GroupWoDelete(new Guid(groupId));
                if (!string.IsNullOrEmpty(id))
                    PhoenixPlannedMaintenanceDailyWorkPlan.DeleteWorkOrder(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                    "BookMarkScript", "refresh();", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
}
