using System;
using System.Data;
using System.Web.UI;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Text;
using Telerik.Web.UI;

public partial class Crew_CrewTravelPlanRequestAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("ADD", "ADDPLAN",ToolBarDirection.Right);
            MenuBreakupCopy.AccessRights = this.ViewState;
            MenuBreakupCopy.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                if (Request.QueryString["VESSELID"] != null && Request.QueryString["TRAVELID"] != null)
                {
                    ViewState["Vesselid"] = Request.QueryString["VESSELID"].ToString();
                    ViewState["Travelid"] = Request.QueryString["TRAVELID"].ToString();
                }
            }
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuBreakupCopy_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper() == "ADDPLAN")
            {
                StringBuilder stremployeelist = new StringBuilder();
                StringBuilder strtravelrequestlist = new StringBuilder();
                if (gvPlan.MasterTableView.Items.Count > 0)
                {
                    foreach (GridDataItem gvr in gvPlan.MasterTableView.Items)
                    {
                        RadCheckBox chkAdd = (RadCheckBox)gvr.FindControl("chkAdd");

                        if (chkAdd.Checked==true)
                        {
                            RadLabel lblPlanId = (RadLabel)gvr.FindControl("lblRequestId");

                            strtravelrequestlist.Append(lblPlanId.Text);
                            strtravelrequestlist.Append(",");
                        }
                    }

                    if (strtravelrequestlist.Length > 1)
                    {
                        strtravelrequestlist.Remove(strtravelrequestlist.Length - 1, 1);
                    }
                    else
                    {
                        ucError.ErrorMessage = "Please select Employees to Add";
                        ucError.Visible = true;
                        return;
                    }
                }
                else
                {
                    ucError.ErrorMessage = "Please select Employees to Add";
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewTravelRequest.CrewTravelPlanRequestAdd(
                     General.GetNullableInteger(ViewState["Vesselid"].ToString())
                    , strtravelrequestlist.ToString()
                    , General.GetNullableGuid(ViewState["Travelid"].ToString())
                   );
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1','',true);", true);               
                BindData();
                gvPlan.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void BindData()
    {
        try
        {

            DataTable dt = PhoenixCrewTravelRequest.CrewTravelPlanRequestAddList(General.GetNullableInteger(ViewState["Vesselid"].ToString()));

            if (dt.Rows.Count > 0)
                gvPlan.DataSource = dt;
            else
                gvPlan.DataSource = "";
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
            BindData();
            gvPlan.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPlan_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}