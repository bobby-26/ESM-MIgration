using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI;
using System.Text;
using SouthNests.Phoenix.Registers;

public partial class PlannedMaintenance_PlannedMaintenanceDefectListAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarNoonReporttap = new PhoenixToolbar();
            toolbarNoonReporttap.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuSave.AccessRights = this.ViewState;
            MenuSave.MenuList = toolbarNoonReporttap.Show();

            if (!IsPostBack)
            {
                txtcomponentNo.Attributes.Add("style", "display:none");
                if (Request.QueryString["WorkorderId"] != null && General.GetNullableGuid(Request.QueryString["WorkorderId"].ToString()) != null)
                    ViewState["WOID"] = Request.QueryString["WorkorderId"].ToString();
                else
                    ViewState["WOID"] = "";
                ViewState["refreshframe"] = string.Empty;
                if (Request.QueryString["refreshframe"] != null)
                {
                    ViewState["refreshframe"] = Request.QueryString["refreshframe"];
                }
                if (Request.QueryString["ComponentId"] != null && Request.QueryString["ComponentNo"] != null)
                {
                    txtcomponentNo.Attributes.Remove("style");
                    txtcomponentNo.Text = Request.QueryString["ComponentNo"].ToString();
                    ucComponent.Attributes.Add("style", "display:none");
                    ucComponent.Visible = false;
                }
                if (Request.QueryString["CALLFROM"] != null && Request.QueryString["CALLFROM"].ToString().ToUpper().Equals("WORKORDER"))           // called from popup
                {
                    rdType.Enabled = false;
                    rdType.SelectedValue = "1";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSave_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string componentid = Request.QueryString["ComponentId"] != null? Request.QueryString["ComponentId"].ToString(): ucComponent.SelectedValue;
                string Details = txtdetailsofthedefect.Text;
                string Duedate = ucDueDate.Text;
                string Responsibility = ucDisciplineResponsibility.SelectedDiscipline;

                if (!IsValidDefectjob(Details, Duedate))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixPlannedMaintenanceDefectJob.DefectJobInsert(General.GetNullableGuid(componentid), Details,
                                                                   DateTime.Parse(Duedate), 
                                                                   General.GetNullableInteger(Responsibility)
                                                                   ,General.GetNullableInteger(rdType.SelectedValue)
                                                                   ,General.GetNullableGuid(ViewState["WOID"].ToString())
                                                                   ,General.GetNullableString(txtActionRequired.Text)
                                                                   );

                //if (Request.QueryString["ComponentId"] != null)           // called from popup
                //{
                //    string refreshname = "";
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //                "BookMarkScript", "top.closeTelerikWindow('Defect'," + (refreshname == string.Empty ? "null" : "'" + refreshname + "'") + ");", true);
                //}
                //else
                //{
                //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //    "BokMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                //}


                if (Request.QueryString["CALLFROM"] != null && Request.QueryString["CALLFROM"].ToString().ToUpper().Equals("WORKORDER"))           // called from popup
                {
                    string refreshname = ViewState["refreshframe"].ToString();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                "BookMarkScript", "top.closeTelerikWindow('Defect'," + (refreshname == string.Empty ? "null" : "'" + refreshname + "'") + ");", true);
                }
                else
                {
                    string script = "parent.CloseModelWindow();";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                }
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private bool IsValidDefectjob(string Details, string Duedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
        if (string.IsNullOrEmpty(Details))
            ucError.ErrorMessage = "Defect Details is required.";

        if (string.IsNullOrEmpty(Duedate))
            ucError.ErrorMessage = "Due Date is required.";

        if (General.GetNullableInteger(rdType.SelectedValue) == null)
            ucError.ErrorMessage = "Type is required";

        return (!ucError.IsError);
    }
}