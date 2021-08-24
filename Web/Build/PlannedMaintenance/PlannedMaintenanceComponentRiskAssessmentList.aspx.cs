using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI;

public partial class PlannedMaintenanceComponentRiskAssessmentList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
             
            if (!IsPostBack)
            {
                ViewState["ComponentId"] = string.Empty;
                ViewState["WorkorderId"] = string.Empty;
                ViewState["ISPOPUP"] = string.Empty;
                ViewState["GroupId"] = string.Empty;
                if (Request.QueryString["ComponentId"] != null)
                    ViewState["ComponentId"] = Request.QueryString["ComponentId"];
                if (Request.QueryString["WorkorderId"] != null)
                    ViewState["WorkorderId"] = Request.QueryString["WorkorderId"];
                if (!string.IsNullOrEmpty(Request.QueryString["ispopup"]))
                    ViewState["ISPOPUP"] = Request.QueryString["ispopup"];

                if (!string.IsNullOrEmpty(Request.QueryString["WORescheduleID"]))
                    ViewState["WORescheduleID"] = Request.QueryString["WORescheduleID"];

                if (!string.IsNullOrEmpty(Request.QueryString["WORescheduleDTKey"]))
                    ViewState["WORescheduleDTKey"] = Request.QueryString["WORescheduleDTKey"];

                if (!string.IsNullOrEmpty(Request.QueryString["GroupId"]))
                    ViewState["GroupId"] = Request.QueryString["GroupId"];

                if (!string.IsNullOrEmpty(Request.QueryString["GroupId"]))
                    ViewState["GroupId"] = Request.QueryString["GroupId"];

                if (!string.IsNullOrEmpty(Request.QueryString["ispostpone"]))
                    ViewState["ispostpone"] = Request.QueryString["ispostpone"];
                else
                    ViewState["ispostpone"] = "0";
                if (!string.IsNullOrEmpty(Request.QueryString["inlp"]))
                    ViewState["inlp"] = Request.QueryString["inlp"];
                else
                    ViewState["inlp"] = "0";
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

            ds = PhoenixPlannedMaintenanceWorkOrderGroup.ComponentRAList(new Guid(ViewState["ComponentId"].ToString())
                                                                        ,PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            gvRiskAssessment.DataSource = ds;
            gvRiskAssessment.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRiskAssessment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvRiskAssessment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                string raId = (item.FindControl("lblRaId") as RadLabel).Text;

                if (General.GetNullableInteger(ViewState["ispostpone"].ToString()) == 1)
                {
                    PhoenixPlannedMaintenanceWorkOrderReschedule.WorkOrderRescheduleMapRA(
                            PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , new Guid(ViewState["WorkorderId"].ToString())
                            , new Guid(raId)
                            , new Guid(ViewState["WORescheduleID"].ToString())
                            , new Guid(ViewState["WORescheduleDTKey"].ToString())
                        );
                }
                else
                {
                    PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderRAMap(new Guid(ViewState["ComponentId"].ToString()),
                                                                        new Guid(raId),
                                                                        General.GetNullableGuid(ViewState["WorkorderId"].ToString()),
                                                                        PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                                        General.GetNullableGuid(""),
                                                                        General.GetNullableGuid(ViewState["GroupId"].ToString()));
                }
                if (ViewState["inlp"].ToString().Equals("1"))
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                "BookMarkScript", "refreshParent()", true);
                }
                else
                {
                    if (ViewState["ISPOPUP"].ToString().Equals(""))
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                    "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                    }
                    else if (!ViewState["ISPOPUP"].ToString().Equals(""))
                    {
                        string[] popup = ViewState["ISPOPUP"].ToString().Split(',');
                        string refreshname = popup.Length > 1 ? popup[1] : string.Empty;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                    "BookMarkScript", "top.closeTelerikWindow('" + popup[0] + "'," + (refreshname == string.Empty ? "null" : "'" + refreshname + "'") + ");", true);
                    }
                }
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
}
