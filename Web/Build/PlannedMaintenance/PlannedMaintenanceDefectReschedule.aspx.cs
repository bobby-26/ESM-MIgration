using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PlannedMaintenanceDefectReschedule : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        menuReschedule.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request.QueryString["dueDate"]))
            {
                txtPlannedDate.SelectedDate = DateTime.Parse(Request.QueryString["dueDate"].ToString());
            }
        }
    }

    private bool IsValidRequisition(string date, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(date))
            ucError.ErrorMessage = "Postpone date is required";

        if (remarks.Trim().Equals(""))
            ucError.ErrorMessage = "Postpone remarks is required";

        return (!ucError.IsError);
    }


    protected void btnpostpone_Click(object sender, EventArgs e)
    {

    }
    protected void menuReschedule_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string commandname = ((RadToolBarButton)dce.Item).CommandName;
            if (commandname.ToUpper().Equals("SAVE"))
            {
                if (Page.IsValid)
                {
                    if (Request.QueryString["defectId"] != null)
                    {

                        PhoenixPlannedMaintenanceDefectJob.DefectReschedule(new Guid(Request.QueryString["defectId"].ToString())
                                                                                         , DateTime.Parse(txtpostponeDate.SelectedDate.ToString())
                                                                                         , txtRemarks.Text);
                    }

                    //string Script = "";
                    //Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
                    //Script += "fnReloadList();";
                    //Script += "</script>" + "\n";

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                    "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
                }
            }

        }
        catch (Exception ex)
        {
            RequiredFieldValidator Validator = new RequiredFieldValidator();
            Validator.ErrorMessage = ex.Message;
            //Validator.ValidationGroup = "Group1";
            Validator.IsValid = false;
            Validator.Visible = false;
            Page.Form.Controls.Add(Validator);
        }
    }

    protected void gvReschedule_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    private void BindData()
    {
        if (Request.QueryString["defectId"] != null)
        {
            DataTable dt;
            if (Request.QueryString["VesselId"] != null)
                dt = PhoenixPlannedMaintenanceDefectJob.DefectRescheduleList(new Guid(Request.QueryString["defectId"].ToString()),General.GetNullableInteger(Request.QueryString["VesselId"].ToString()));
            else
                 dt = PhoenixPlannedMaintenanceDefectJob.DefectRescheduleList(new Guid(Request.QueryString["defectId"].ToString()));
            gvReschedule.DataSource = dt;
                                                                           
        }
    }
}
