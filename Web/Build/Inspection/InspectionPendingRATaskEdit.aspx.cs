using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class InspectionPendingRATaskEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbartab = new PhoenixToolbar();
        toolbartab.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuMapping.AccessRights = this.ViewState;
        MenuMapping.MenuList = toolbartab.Show();

        if (!IsPostBack)
        {
            ViewState["RATYPE"] = "-1";
            ViewState["Vesselid"] = "";

            txtPICIdAdd.Attributes.Add("style", "visibility:hidden");
            txtShoreTeamAppointedByEmail.Attributes.Add("style", "visibility:hidden");

            ViewState["callfrom"] = string.IsNullOrEmpty(Request.QueryString["callfrom"]) ? "" : Request.QueryString["callfrom"];
            ViewState["RATYPE"] = string.IsNullOrEmpty(Request.QueryString["RATYPE"]) ? "" : Request.QueryString["RATYPE"];
            ViewState["RAId"] = string.IsNullOrEmpty(Request.QueryString["RAId"]) ? "" : Request.QueryString["RAId"];
            ViewState["Status"] = string.IsNullOrEmpty(Request.QueryString["Status"]) ? "" : Request.QueryString["Status"];
            ViewState["Taskid"] = string.IsNullOrEmpty(Request.QueryString["Taskid"]) ? "" : Request.QueryString["Taskid"];
            ViewState["Vesselid"] = string.IsNullOrEmpty(Request.QueryString["Vesselid"]) ? "" : Request.QueryString["Vesselid"];
            ViewState["TASKMODE"] = string.IsNullOrEmpty(Request.QueryString["TaskMode"]) ? "" : Request.QueryString["TaskMode"];


            if (Request.QueryString["RAMACHINERYID"] != null && Request.QueryString["RAMACHINERYID"].ToString() != string.Empty)
                ViewState["RAMACHINERYID"] = Request.QueryString["RAMACHINERYID"].ToString();

            Bind();
        }
    }

    protected void Bind()
    {
        if (ViewState["RAMACHINERYID"] != null && ViewState["RAMACHINERYID"].ToString() != string.Empty)
        {
            txtCompletionRemarks.Enabled = true;
            uccompletiondate.Enabled = true;
            DataTable ds = PhoenixInspectionRiskAssessmentMachineryExtn.EditRiskAssessmentMachineryTask(General.GetNullableGuid(ViewState["RAMACHINERYID"].ToString()));
            if (ds.Rows.Count > 0)
            {
                DataRow dr = ds.Rows[0];
                txtTask.Text = dr["FLDTASK"].ToString();
                txtCompletionRemarks.Text = dr["FLDREMARKS"].ToString();
                txtstatus.Text = dr["FLDSTATUSNAME"].ToString();
                uccompletiondate.Text = dr["FLDACTUALFINISHDATE"].ToString();
                uctargetdate.Text = dr["FLDESTIMATEDFINISHDATE"].ToString();
                txtPICIdAdd.Text = dr["FLDPIC"].ToString();
                txtPICNameAdd.Text = dr["FLDPICNAME"].ToString();
                txtPICRankAdd.Text = dr["FLDPICRANK"].ToString();
                rblDepartmentType.SelectedValue = dr["FLDDEPARTMENTTYPEID"].ToString();
            }
        }
    }

    protected void MenuMapping_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsTaskValid())
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["RAMACHINERYID"].ToString() == "")
                {
                    Guid? Taskid = General.GetNullableGuid(null);

                    PhoenixInspectionRiskAssessmentMachineryExtn.InsertMachinerySafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["RAId"].ToString())
                    , ref Taskid
                    , General.GetNullableString(txtTask.Text.Trim())
                    , General.GetNullableInteger(ViewState["Vesselid"].ToString())
                    , General.GetNullableInteger(txtPICIdAdd.Text)
                    , General.GetNullableString(txtPICNameAdd.Text.Trim())
                    , General.GetNullableString(txtPICRankAdd.Text.Trim())
                    , General.GetNullableDateTime(uctargetdate.Text)
                    , General.GetNullableString(txtCompletionRemarks.Text.Trim())
                    , General.GetNullableDateTime(uccompletiondate.Text)
                    , General.GetNullableInteger(ViewState["TASKMODE"].ToString())
                    , General.GetNullableInteger(ViewState["RATYPE"].ToString())
                    , General.GetNullableInteger(rblDepartmentType.SelectedValue));

                    ViewState["RAMACHINERYID"] = Taskid;

                    Bind();
                    ucStatus.Text = "Information Updated";

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Task', 'PendingTask');", true);
                }
                else
                {
                    Guid? Taskid = General.GetNullableGuid(ViewState["RAMACHINERYID"].ToString());

                    PhoenixInspectionRiskAssessmentMachineryExtn.InsertMachinerySafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["RAId"].ToString())
                    , ref Taskid
                    , General.GetNullableString(txtTask.Text.Trim())
                    , General.GetNullableInteger(ViewState["Vesselid"].ToString())
                    , General.GetNullableInteger(txtPICIdAdd.Text)
                    , General.GetNullableString(txtPICNameAdd.Text.Trim())
                    , General.GetNullableString(txtPICRankAdd.Text.Trim())
                    , General.GetNullableDateTime(uctargetdate.Text)
                    , General.GetNullableString(txtCompletionRemarks.Text.Trim())
                    , General.GetNullableDateTime(uccompletiondate.Text)
                    , General.GetNullableInteger(ViewState["TASKMODE"].ToString())
                    , General.GetNullableInteger(ViewState["RATYPE"].ToString())
                    , General.GetNullableInteger(rblDepartmentType.SelectedValue));

                    Bind();
                    ucStatus.Text = "Information Updated";

                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('Task', 'PendingTask');", true);
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsTaskValid()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if (General.GetNullableGuid(ViewState["RAId"].ToString()) == null)
        //    ucError.ErrorMessage = "RA is required";

        if (General.GetNullableDateTime(uctargetdate.Text) == null)
            ucError.ErrorMessage = "Target Date is required";

        if (General.GetNullableString(txtCompletionRemarks.Text.Trim()) == null)
            ucError.ErrorMessage = "Completion Remarks";

        if (General.GetNullableDateTime(uccompletiondate.Text) == null)
            ucError.ErrorMessage = "Completion Date is required";

        //if (General.GetNullableInteger(txtPICIdAdd.Text) == null)
        //    ucError.ErrorMessage = "Responsiblity is required";

        return (!ucError.IsError);
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