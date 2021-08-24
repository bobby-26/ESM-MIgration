using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionMOCTrainingRequiredAdd : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        imgPersonInCharge.Attributes.Add("onclick", "return showPickList('spnPersonInCharge', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId=" + ViewState["Vesselid"].ToString() + "', true); ");
        imgPersonInChargeOffice.Attributes.Add("onclick", "return showPickList('spnPersonInChargeOffice', 'codehelp1', '', '../Common/CommonPickListUser.aspx?departmentlist=&mod=', true);");

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        ViewState["MOCID"] = Request.QueryString["MOCID"].ToString();
        ViewState["Vesselid"] = Request.QueryString["Vesselid"].ToString();

        PhoenixToolbar toolbarsubmit = new PhoenixToolbar();
        toolbarsubmit.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuMOC.MenuList = toolbarsubmit.Show();

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            txtCrewId.Attributes.Add("style", "visibility:hidden");
            txtPersonInChargeOfficeId.Attributes.Add("style", "visibility:hidden");
            txtPersonInChargeOfficeEmail.Attributes.Add("style", "visibility:hidden");
        }
        BindMOCTraining();
    }

    private void BindMOCTraining()
    {
        if (ddltraining.SelectedValue == "2")
        {
            spnPersonInCharge.Visible = false;
            spnPersonInChargeOffice.Visible = true;
        }
        else
        {
            spnPersonInCharge.Visible = true;
            spnPersonInChargeOffice.Visible = false;
        }
    }

    protected void MOC_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidMOCTrainingRequired(ddltraining.SelectedValue
                                                  , txtTrainingRequired.Text
                                                  , txtDepartmentAdd.Text
                                                  , txtTargetdateAdd.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                Guid? Taskid = new Guid(ViewState["MOCREQUIREDID"].ToString());

                PhoenixInspectionMOCRequestForChange.MOCTrainingRequiredInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                     , ref Taskid
                                                                     , new Guid((ViewState["MOCID"]).ToString())
                                                                     , int.Parse(ViewState["Vesselid"].ToString())
                                                                     , Int32.Parse(ddltraining.SelectedValue)
                                                                     , txtTrainingRequired.Text
                                                                     , txtDepartmentAdd.Text
                                                                     , (ddltraining.SelectedValue == "1") ? General.GetNullableInteger(txtCrewId.Text) : General.GetNullableInteger(txtPersonInChargeOfficeId.Text)
                                                                     , (ddltraining.SelectedValue == "1") ? txtCrewName.Text : txtOfficePersonName.Text
                                                                     , (ddltraining.SelectedValue == "1") ? txtCrewRank.Text : txtOfficePersonDesignation.Text
                                                                     , General.GetNullableDateTime(txtTargetdateAdd.Text)
                                                                     , null);
            }
            else
            {
                ucError.Visible = true;
                return;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidMOCTrainingRequired(string TrainingType, string TrainingRequired, string Department, string Targetdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (TrainingType.Equals("Dummy"))
            ucError.ErrorMessage = "Training Type is required";

        if (TrainingRequired.Equals(""))
            ucError.ErrorMessage = "Training required";

        if (Department.Equals(""))
            ucError.ErrorMessage = "Dept./Persons to be trained is Required";

        if (General.GetNullableDateTime(Targetdate) == null)
            ucError.ErrorMessage = "Target date Required";

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    public void ddltraining_SelectedIndedChanged(object sender, EventArgs e)
    {
        if (ddltraining.SelectedValue == "2")
        {
            spnPersonInCharge.Visible = false;
            spnPersonInChargeOffice.Visible = true;
        }
        else
        {
            spnPersonInCharge.Visible = true;
            spnPersonInChargeOffice.Visible = false;
        }
    }
}
