using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
using SouthNests.Phoenix.VesselAccounts;
public partial class PlannedMaintenance_PlannedMaintenanceOrderInformationAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuWorkOrder.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {            
            ViewState["ID"] = string.Empty;
            ViewState["EID"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                ViewState["ID"] = Request.QueryString["id"];
                //EditOrderInformation();
            }            
            ViewState["TYPE"] = string.Empty;
            if (!string.IsNullOrEmpty(Request.QueryString["type"]))
            {
                ViewState["TYPE"] = Request.QueryString["type"];
            }
            PopulateRegisters();
            txtDate.SelectedDate = DateTime.Now;            
        }
    }
    protected void MenuWorkOrder_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string detail = txtDetail.Text;
                var collection = ddlCrewList.SelectedItems;
                string csvapplicableto = string.Empty;
                string csvapplicabletoname = string.Empty;
                if (collection.Count != 0)
                {
                    csvapplicableto = ",";
                    csvapplicabletoname = ",";
                    foreach (var item in collection)
                    {
                        csvapplicableto = csvapplicableto + item.Value + ",";
                        csvapplicabletoname = csvapplicabletoname + item.Text + ",";
                    }
                }
                if (!IsValidOrderInformation(detail, csvapplicableto))
                {
                    ucError.Visible = true;
                    return;
                }
                string id = ViewState["ID"].ToString();
                if (!General.GetNullableGuid(id).HasValue)
                {
                    PhoenixPlannedMaintenanceOrderInformation.Insert(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , txtDate.SelectedDate.Value, detail, int.Parse(ViewState["TYPE"].ToString()), csvapplicableto, csvapplicabletoname);
                }
                else
                {
                    PhoenixPlannedMaintenanceOrderInformation.Update(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                        , detail, csvapplicableto, csvapplicabletoname, txtDate.SelectedDate.Value);
                }
                string script = "parent.CloseModelWindow();";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditOrderInformation()
    {
        string id = ViewState["ID"].ToString();
        int? EmployeeId = null;
        DataTable dt = PhoenixPlannedMaintenanceOrderInformation.Edit(new Guid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID, ref EmployeeId);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtDate.SelectedDate = General.GetNullableDateTime(dr["FLDDATE"].ToString());
            txtDetail.Text = dr["FLDDETAIL"].ToString();
            foreach (ButtonListItem item in ddlCrewList.Items)
            {
                item.Selected = false;
                if (dr["FLDAPPLICABLETO"].ToString().Contains("," + item.Value + ","))
                {
                    item.Selected = true;
                }
            }            
        }
        ViewState["EID"] = EmployeeId.HasValue ? EmployeeId.ToString() : string.Empty;
    }
    private bool IsValidOrderInformation(string details, string applicablerank)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (details.Trim().Equals(""))
            ucError.ErrorMessage = "Details is required.";

        if (details.Trim().Equals(""))
            ucError.ErrorMessage = "Applicable To is required.";

        return (!ucError.IsError);
    }

    protected void PopulateRegisters()
    {
        string id = ViewState["ID"].ToString();
        int? EmployeeId = null;
        DataTable dt = PhoenixPlannedMaintenanceOrderInformation.Edit(General.GetNullableGuid(id), PhoenixSecurityContext.CurrentSecurityContext.VesselID, ref EmployeeId);
        ViewState["EID"] = EmployeeId.HasValue ? EmployeeId.ToString() : string.Empty;
        ddlCrewList.DataSource = GetCrewList();
        ddlCrewList.DataBind();

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtDate.SelectedDate = General.GetNullableDateTime(dr["FLDDATE"].ToString());
            txtDetail.Text = dr["FLDDETAIL"].ToString();
            foreach (ButtonListItem item in ddlCrewList.Items)
            {
                item.Selected = false;
                if (dr["FLDAPPLICABLETO"].ToString().Contains("," + item.Value + ","))
                {
                    item.Selected = true;
                }
            }
        }
    }
    private DataTable GetCrewList()
    {
        return PhoenixVesselAccountsEmployee.ListVesselCrew(PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtDate.SelectedDate);
    }


    protected void ddlCrewList_ItemDataBound(object sender, ButtonListEventArgs e)
    {
        if (e.Item.Value == ViewState["EID"].ToString())
        {
            e.Item.Enabled = false;
        }
        if (ViewState["TYPE"].ToString() == "2" && (e.Item.Text.ToUpper().StartsWith("MST") || e.Item.Text.ToUpper().StartsWith("CO")))
        {
            e.Item.Enabled = false;
        }
        if (ViewState["TYPE"].ToString() == "3" && (e.Item.Text.ToUpper().StartsWith("MST") || e.Item.Text.ToUpper().StartsWith("CE")))
        {
            e.Item.Enabled = false;
        }
    }
}