using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class InspectionMOCTrainingAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbartab = new PhoenixToolbar();
        toolbartab.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbartab.AddButton("Back", "BACK", ToolBarDirection.Right);
        MenuMapping.AccessRights = this.ViewState;
        MenuMapping.MenuList = toolbartab.Show();

        if (!IsPostBack)
        {
            ViewState["Vesselid"] = "";
            ViewState["MOCID"] = "";
            ViewState["TrainingID"] = "";

            txtPICIdAdd.Attributes.Add("style", "visibility:hidden");
            txtShoreTeamAppointedByEmail.Attributes.Add("style", "visibility:hidden");

            ViewState["MOCID"] = Request.QueryString["MOCID"];
            ViewState["VESSELID"] = Request.QueryString["VESSELID"];
            ViewState["TrainingID"] = string.IsNullOrEmpty(Request.QueryString["TrainingID"]) ? "" : Request.QueryString["TrainingID"];

            Bind();

            if (rblDepartmentType.SelectedValue.Equals("2"))
            {
                imgReportedByShip.Attributes.Add("onclick", "return showPickList('spnReportedByShip3', 'codehelp1', '','../Common/CommonPickListUser.aspx?', true); ");
            }
            else
            {
                imgReportedByShip.Attributes.Add("onclick",
                       "return showPickList('spnReportedByShip3', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                       + ViewState["Vesselid"].ToString() + "', true); ");
            }
        }
    }

    protected void Bind()
    {
        if (ViewState["TrainingID"] != null && ViewState["TrainingID"].ToString() != string.Empty)
        {
            DataTable ds = PhoenixInspectionMOCRequestForChange.MOCTrainingRequiredEdit(new Guid(ViewState["TrainingID"].ToString()));

            if (ds.Rows.Count > 0)
            {
                DataRow dr = ds.Rows[0];
                uctargetdate.Text = dr["FLDTARGETDATE"].ToString();
                txtTrainingRequired.Text = dr["FLDTRAININGREQUIRED"].ToString();
                txtTrained.Text = dr["FLDDEPARTMENTORPERSONTOBETRAINED"].ToString();
                txtPICIdAdd.Text = dr["FLDPIC"].ToString();
                txtPICNameAdd.Text = dr["FLDPICNAME"].ToString();
                txtPICRankAdd.Text = dr["FLDPICRANK"].ToString();
                rblDepartmentType.SelectedValue = dr["FLDTRAININGTYPE"].ToString();
                ucDept.SelectedDepartment = dr["FLDDEPARTMENTID"].ToString();
            }
        }
    }

    private bool IsValidMOCTrainingRequired(string TrainingRequired, string Department, string Targetdate, string resperson)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (TrainingRequired.Equals(""))
            ucError.ErrorMessage = "Training required";

        if (Department.Equals(""))
            ucError.ErrorMessage = "Dept./Persons to be trained is Required";

        if (General.GetNullableInteger(resperson)==null)
            ucError.ErrorMessage = "Department is Required";

        if (General.GetNullableDateTime(Targetdate) == null)
            ucError.ErrorMessage = "Target date Required";

        return (!ucError.IsError);
    }

    protected void MenuMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidMOCTrainingRequired(txtTrainingRequired.Text
                                                   , txtTrained.Text
                                                   , uctargetdate.Text
                                                   , ucDept.SelectedDepartment))
                {
                    ucError.Visible = true;
                    return;
                }

                Guid? Taskid = null;
                if (ViewState["TrainingID"].ToString() == "")
                {
                    General.GetNullableGuid(null);

                    PhoenixInspectionMOCRequestForChange.MOCTrainingRequiredInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                     , ref Taskid
                                                     , new Guid((ViewState["MOCID"]).ToString())
                                                     , int.Parse(ViewState["VESSELID"].ToString())
                                                     , Int32.Parse(rblDepartmentType.SelectedValue)
                                                     , txtTrainingRequired.Text
                                                     , txtTrained.Text
                                                     , General.GetNullableInteger(txtPICIdAdd.Text)
                                                     , txtPICNameAdd.Text.Trim()
                                                     , txtPICRankAdd.Text.Trim()
                                                     , General.GetNullableDateTime(uctargetdate.Text)
                                                     , General.GetNullableInteger(ucDept.SelectedDepartment));

                    ViewState["TrainingID"] = Taskid;

                    Bind();
                    ucStatus.Text = "Information Updated";
                }
                else
                {
                    Taskid = new Guid(ViewState["TrainingID"].ToString());

                    PhoenixInspectionMOCRequestForChange.MOCTrainingRequiredInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                 , ref Taskid
                                 , new Guid((ViewState["MOCID"]).ToString())
                                 , int.Parse(ViewState["VESSELID"].ToString())
                                 , Int32.Parse(rblDepartmentType.SelectedValue)
                                 , txtTrainingRequired.Text
                                 , txtTrained.Text
                                 , General.GetNullableInteger(txtPICIdAdd.Text)
                                 , txtPICNameAdd.Text.Trim()
                                 , txtPICRankAdd.Text.Trim()
                                 , General.GetNullableDateTime(uctargetdate.Text)
                                 , General.GetNullableInteger(ucDept.SelectedDepartment));

                    Bind();
                    ucStatus.Text = "Information Updated";
                }
            }

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestAdd.aspx?&VESSELID=" + ViewState["VESSELID"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString());
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

    }


    protected void rblDepartmentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblDepartmentType.SelectedValue.Equals("2"))
        {
            txtPICIdAdd.Text = "";
            txtPICNameAdd.Text = "";
            txtPICRankAdd.Text = "";
            imgReportedByShip.Attributes.Add("onclick", "return showPickList('spnReportedByShip3', 'codehelp1', '','../Common/CommonPickListUser.aspx?', true); ");
        }
        else
        {
            txtPICIdAdd.Text = "";
            txtPICNameAdd.Text = "";
            txtPICRankAdd.Text = "";
            imgReportedByShip.Attributes.Add("onclick",
                   "return showPickList('spnReportedByShip3', 'codehelp1', '','../Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
                   + ViewState["Vesselid"].ToString() + "', true); ");
        }
    }
}