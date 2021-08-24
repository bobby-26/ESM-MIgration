using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class PlannedMaintenanceJobsJHAlist : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("javascript: showDialog('Add');", "Add JHA", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MenuJHA.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

                ViewState["jobId"] = "";
                ViewState["workorderId"] = string.Empty;
                ViewState["groupid"] = string.Empty;
                if (Request.QueryString["jobId"] != null)
                    ViewState["jobId"] = Request.QueryString["jobId"].ToString();
                if (Request.QueryString["WORKORDERID"] != null)
                    ViewState["workorderId"] = Request.QueryString["WORKORDERID"].ToString();
                if (Request.QueryString["WORKORDERGROUPID"] != null)
                    ViewState["groupid"] = Request.QueryString["WORKORDERGROUPID"].ToString();
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                if (Request.QueryString["vesselid"] != null)
                    ViewState["VESSELID"] = Request.QueryString["vesselid"];
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

            DataSet ds;
            if (Request.QueryString["vesselid"] != null)
                ds = PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderJHAlist(new Guid(ViewState["workorderId"].ToString()),int.Parse(ViewState["VESSELID"].ToString()));
            else
                ds = PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderJHAlist(new Guid(ViewState["workorderId"].ToString()));

            gvJhaList.DataSource = ds;
            gvJhaList.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
       
    protected void gvJhaList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvJhaList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item is GridDataItem)
            {
                string hazardId = drv["FLDJOBHAZARDID"].ToString();
                LinkButton jha = (LinkButton)e.Item.FindControl("lnkJHA");
                if (jha != null)
                {
                    jha.Visible = SessionUtil.CanAccess(this.ViewState, jha.CommandName);
                    if (Request.QueryString["vesselid"] != null)
                        jha.Attributes.Add("onclick", "javascript:top.openNewWindow('JobHazard', '', '" + Session["sitepath"] + "/Inspection/InspectionRAJobHazardAnalysisExtn.aspx?jobhazardid=" + hazardId + "&vslid=" + ViewState["VESSELID"] + "');return false;");
                    else
                        jha.Attributes.Add("onclick", "javascript:top.openNewWindow('JobHazard', '', '" + Session["sitepath"] + "/Inspection/InspectionRAJobHazardAnalysisExtn.aspx?jobhazardid=" + hazardId + "');return false;");
                }
                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null)
                {
                    del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                    del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);                    
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string hazardId = Filter.CurrentPickListSelection["lblJobHazardid"];
            if (General.GetNullableGuid(hazardId) != null)
            {
                if (Request.QueryString["vesselid"] != null)
                    PhoenixPlannedMaintenanceWorkOrderGroupExtn.SubWorkorderJhaPtwInsert(new Guid(hazardId), new Guid(ViewState["workorderId"].ToString()), new Guid(ViewState["groupid"].ToString()),int.Parse(ViewState["VESSELID"].ToString()));
                else
                    PhoenixPlannedMaintenanceWorkOrderGroupExtn.SubWorkorderJhaPtwInsert(new Guid(hazardId), new Guid(ViewState["workorderId"].ToString()), new Guid(ViewState["groupid"].ToString()));
                string script = "if (typeof parent.CloseUrlModelWindow === \"function\") { parent.CloseUrlModelWindow(); };";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvJhaList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string jhaid = ((RadLabel)item.FindControl("lblJHAid")).Text;
                PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderJHADelete(new Guid(jhaid), new Guid(ViewState["groupid"].ToString())
                    , new Guid(ViewState["workorderId"].ToString()), int.Parse(ViewState["VESSELID"].ToString()));
                string script = "if (typeof parent.CloseUrlModelWindow === \"function\") { parent.CloseUrlModelWindow(); };";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
