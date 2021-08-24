using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web.UI;
using SouthNests.Phoenix.Inspection;

public partial class Inspection_InspectionDefectJobVerification : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarNoonReporttap = new PhoenixToolbar();

            toolbarNoonReporttap.AddButton("Exit", "EXIT", ToolBarDirection.Right);
            toolbarNoonReporttap.AddButton("Submit", "SUBMIT", ToolBarDirection.Right);
            MenuSave.AccessRights = this.ViewState;
            MenuSave.MenuList = toolbarNoonReporttap.Show();
            if (!IsPostBack)
            {

                ViewState["Defectjobid"] = string.Empty;
                ViewState["Defectno"] = string.Empty;

                if (Request.QueryString["defectjobId"] != null)
                    ViewState["Defectjobid"] = Request.QueryString["defectjobId"].ToString();

                if (Request.QueryString["defectno"] != null)
                    ViewState["Defectno"] = Request.QueryString["defectno"].ToString();

                lbldefectjobid.Text = ViewState["Defectjobid"].ToString();
                txtdefectno.Text = ViewState["Defectno"].ToString();
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

            if (CommandName.ToUpper().Equals("SUBMIT"))
            {
                if (!string.IsNullOrEmpty(ViewState["Defectjobid"].ToString()))
                {
                    int workorderrequired = int.Parse(RadioButtonlistwork.SelectedValue);
                    PhoenixInspectionWorkOrder.DefectJobupdateverified(Guid.Parse(ViewState["Defectjobid"].ToString())
                                                                               , workorderrequired);
                    string script = "parent.CloseModelWindow();";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                }
            }
            else if (CommandName.ToUpper().Equals("EXIT"))
            {
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
}
