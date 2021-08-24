using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceCompJobMainSummaryPublish : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["TITLE"] = string.Empty;
                ViewState["JOBSUMMARYID"] = null;

                if (!string.IsNullOrEmpty(Request.QueryString["JOBSUMMARYID"]))
                {
                    ViewState["JOBSUMMARYID"] = Request.QueryString["JOBSUMMARYID"];
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

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (txtTitle.Text == string.Empty)
        {
            RequiredFieldValidator Validator = new RequiredFieldValidator();
            Validator.ErrorMessage = "Title is required";
            //Validator.ValidationGroup = "Group1";
            Validator.IsValid = false;
            Validator.Visible = false;
            Page.Form.Controls.Add(Validator);
        }

        if (Page.IsValid)
        {
            try
            {
                PhoenixPlannedMaintenanceCompJobSummary.CompjobSummaryPublish(new Guid(ViewState["JOBSUMMARYID"].ToString()), txtTitle.Text);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                         "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
            catch (Exception ex)
            {
                RequiredFieldValidator Valid = new RequiredFieldValidator();
                Valid.ErrorMessage = "* " + ex.Message;
                //Validator.ValidationGroup = "Group1";
                Valid.IsValid = false;
                Valid.Visible = false;
                Page.Form.Controls.Add(Valid);
            }
        }
    }
}