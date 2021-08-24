using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI;
using System.Text;
using SouthNests.Phoenix.Registers;

public partial class PlannedMaintenance_PlannedMaintenanceDefectClose : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarNoonReporttap = new PhoenixToolbar();
            toolbarNoonReporttap.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuSave.AccessRights = this.ViewState;
            MenuSave.MenuList = toolbarNoonReporttap.Show();

            if(!IsPostBack)
            {
                ViewState["DefectId"] = null;
                if(Request.QueryString["DefectId"] != null)
                {
                    ViewState["DefectId"] = Request.QueryString["DefectId"].ToString();
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
                string comments = txtComments.Text;
                string Donedate = ucDoneDate.Text;

                if (!IsValidDefectjob(comments, Donedate))
                {
                    ucError.Visible = true;
                    return;
                }
                Console.WriteLine(new Guid(ViewState["DefectId"].ToString()));
                PhoenixPlannedMaintenanceDefectJob.DefectJobClose(new Guid(ViewState["DefectId"].ToString())
                                                                   ,DateTime.Parse(Donedate), 
                                                                   txtComments.Text);
                
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
    private bool IsValidDefectjob(string Details, string Duedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(Duedate))
            ucError.ErrorMessage = "Done date is required.";

        if (string.IsNullOrEmpty(Details))
            ucError.ErrorMessage = "Comments is required.";

        return (!ucError.IsError);
    }
}