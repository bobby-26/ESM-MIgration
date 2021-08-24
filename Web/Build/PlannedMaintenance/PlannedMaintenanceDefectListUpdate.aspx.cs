using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using System.Web.UI;
using System.Text;
using SouthNests.Phoenix.Registers;

public partial class PlannedMaintenance_PlannedMaintenanceDefectListUpdate : PhoenixBasePage
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
                ViewState["defectjobid"] = string.Empty;
                if (Request.QueryString["defectjobId"] != null)
                {
                    ViewState["defectjobid"] = Request.QueryString["defectjobId"].ToString();
                    bindData();
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
                if (!string.IsNullOrEmpty(ViewState["defectjobid"].ToString()))
                {
                    string Details = txtdetailsofthedefect.Text;
                    string Duedate = ucDueDate.Text;
                    string Responsibility = ucDisciplineResponsibility.SelectedDiscipline;

                    if (!IsValidDefectjobupdate(Details, Duedate))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixPlannedMaintenanceDefectJob.DefectJobUpdate(Guid.Parse(ViewState["defectjobid"].ToString()), Details,
                                                                        DateTime.Parse(Duedate), General.GetNullableInteger(Responsibility)
                                                                        ,General.GetNullableString(txtActionRequired.Text));
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

    private void bindData()
    {
        DataSet ds = PhoenixPlannedMaintenanceDefectJob.DefectJobedit(new Guid(ViewState["defectjobid"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtdefectno.Text = dr["FLDDEFECTNO"].ToString();
            ucComponent.Text = (dr["FLDCOMPONENTNUMBER"].ToString() + "_" + dr["FLDCOMPONENTNAME"].ToString());
            txtdetailsofthedefect.Text = dr["FLDDETAILS"].ToString();
            ucDueDate.Text = dr["FLDDUEDATE"].ToString();
            ucDisciplineResponsibility.SelectedDiscipline = dr["FLDRESPONSIBILITY"].ToString();
            txtActionRequired.Text = dr["FLDACTIONREQUIRED"].ToString();
        }
    }
    private bool IsValidDefectjobupdate(string Details, string Duedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(Details))
            ucError.ErrorMessage = "Defect Details is required.";

        if (string.IsNullOrEmpty(Duedate))
            ucError.ErrorMessage = "Due Date is required.";

        return (!ucError.IsError);
    }
}