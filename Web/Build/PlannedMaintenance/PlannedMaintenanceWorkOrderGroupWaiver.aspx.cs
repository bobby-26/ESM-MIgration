using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderGroupWaiver : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuWorkOrderRequestion.MenuList = toolbarmain.Show();
            //txtJobId.Attributes.Add("style", "visibility:hidden");
            ViewState["WaiverType"] = null;
            ViewState["WorkorderGroupId"] = null;
            if (Request.QueryString["WaiverType"] != null)
            {
                ViewState["WaiverType"] = Request.QueryString["WaiverType"].ToString();
            }

            if (Request.QueryString["WorkorderGroupId"] != null)
            {
                ViewState["WorkorderGroupId"] = Request.QueryString["WorkorderGroupId"].ToString();
            }
            string woid = Request.QueryString["WorkOrderId"];
            ViewState["WORKORDERID"] = string.IsNullOrEmpty(woid) ? "" : woid;
        }
    }

    
    protected void MenuWorkOrderRequestion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRequisition(txtRemarks.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string waiverChk = chkWaiver.Checked == true ? "1" : "0";
                if (ViewState["WorkorderGroupId"] != null && ViewState["WaiverType"] != null)
                {
                    PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderGroupWaiverInsert(new Guid(ViewState["WorkorderGroupId"].ToString())
                                                                                       , int.Parse(waiverChk)
                                                                                       , int.Parse(ViewState["WaiverType"].ToString())
                                                                                       , txtRemarks.Text
                                                                                       , General.GetNullableGuid(ViewState["WORKORDERID"].ToString()));
                }
                string inp = Request.QueryString["inp"];
                if (string.IsNullOrEmpty(inp))
                {
                    string refreshname = "codehelp1";
                    string Script = string.Empty;
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "closeTelerikWindow('WAIVE'," + (refreshname == string.Empty ? "null" : "'" + refreshname + "'") + ");";
                    Script += "</script>" + "\n";

                    Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                               "BookMarkScript", "refreshParent()", true);
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidRequisition(string workdetails)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (string.IsNullOrEmpty(workdetails))
            ucError.ErrorMessage = "Remarks is required";

        return (!ucError.IsError);
    }

    protected void gvWaiver_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixPlannedMaintenanceWorkOrderGroup.WorkorderGroupWaiverList(new Guid(ViewState["WorkorderGroupId"].ToString()), int.Parse(ViewState["WaiverType"].ToString()));
        gvWaiver.DataSource = dt;
    }
}
