using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class InspectionRiskAssessmentTask : PhoenixBasePage
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
            ViewState["RATYPE"] = "-1";
            ViewState["Vesselid"] = "";
            
            ViewState["callfrom"] = string.IsNullOrEmpty(Request.QueryString["callfrom"]) ? "" : Request.QueryString["callfrom"];
            ViewState["RATYPE"] = string.IsNullOrEmpty(Request.QueryString["RATYPE"]) ? "" : Request.QueryString["RATYPE"];
            ViewState["RAId"] = string.IsNullOrEmpty(Request.QueryString["RAId"]) ? "" : Request.QueryString["RAId"];
            ViewState["Status"] = string.IsNullOrEmpty(Request.QueryString["Status"]) ? "" : Request.QueryString["Status"];
            ViewState["Taskid"] = string.IsNullOrEmpty(Request.QueryString["Taskid"]) ? "" : Request.QueryString["Taskid"];
            ViewState["Vesselid"] = string.IsNullOrEmpty(Request.QueryString["Vesselid"]) ? "" : Request.QueryString["Vesselid"];
            ViewState["TASKMODE"] = string.IsNullOrEmpty(Request.QueryString["TaskMode"]) ? "" : Request.QueryString["TaskMode"];

            if (ViewState["Vesselid"].ToString()=="0")
            {
                ucUser.Enabled = false;
                ucCrew.Enabled = false;
                uctargetdate.Enabled = false;
            }
            ucCrew.VesselId = ViewState["Vesselid"].ToString();
            ucCrew.DataBind();
            ucUser.DataBind();
            ucUser.Visible = false;
            Bind();
        }
    }

    protected void Bind()
    {
        if (General.GetNullableGuid(ViewState["Taskid"].ToString()).Equals(null))
        {
            txtCompletionRemarks.Enabled = false;
            uccompletiondate.Enabled = false;
        }

        if (ViewState["Taskid"] != null && ViewState["Taskid"].ToString() != string.Empty)
        {
            txtCompletionRemarks.Enabled = true;
            uccompletiondate.Enabled = true;
            DataTable ds = PhoenixInspectionRiskAssessmentMachineryExtn.EditRiskAssessmentMachineryTask(General.GetNullableGuid(ViewState["Taskid"].ToString()));
            if (ds.Rows.Count > 0)
            {
                ucUser.DataBind();
                ucCrew.DataBind();
                DataRow dr = ds.Rows[0];
                txtTask.Text = dr["FLDTASK"].ToString();
                txtCompletionRemarks.Text = dr["FLDREMARKS"].ToString();
                txtstatus.Text = dr["FLDSTATUSNAME"].ToString();
                uccompletiondate.Text = dr["FLDACTUALFINISHDATE"].ToString();
                uctargetdate.Text = dr["FLDESTIMATEDFINISHDATE"].ToString();
                rblDepartmentType.SelectedValue = dr["FLDDEPARTMENTTYPEID"].ToString();

                if (dr["FLDDEPARTMENTTYPEID"].ToString().Equals("2"))
                {
                    ucUser.Visible = true;
                    ucCrew.Visible = false;                    
                }
                else
                {
                    ucUser.Visible = false;
                    ucCrew.Visible = true;
                }
                ucUser.SelectedValue = dr["FLDPIC"].ToString();
                ucUser.Text = dr["FLDPICNAME"].ToString();
                ucCrew.SelectedCrew = dr["FLDPIC"].ToString();
                ucCrew.Text = dr["FLDPICNAME"].ToString();
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

                int? pic;
                string picname;

                if (rblDepartmentType.SelectedValue.Equals("2"))
                {
                    pic = General.GetNullableInteger(ucUser.SelectedValue);
                    picname = General.GetNullableString(ucUser.Text.ToString());
                }
                else
                {
                    pic = General.GetNullableInteger(ucCrew.SelectedCrew);
                    picname = General.GetNullableString(ucCrew.Text.ToString());
                }

                if (ViewState["Taskid"].ToString() == "")
                {
                    Guid? Taskid = General.GetNullableGuid(null);


                    PhoenixInspectionRiskAssessmentMachineryExtn.InsertMachinerySafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["RAId"].ToString())
                    , ref Taskid
                    , General.GetNullableString(txtTask.Text.Trim())
                    , General.GetNullableInteger(ViewState["Vesselid"].ToString())
                    , General.GetNullableInteger(pic.ToString())
                    , General.GetNullableString(picname)
                    , General.GetNullableString(null)
                    , General.GetNullableDateTime(uctargetdate.Text)
                    , General.GetNullableString(txtCompletionRemarks.Text.Trim())
                    , General.GetNullableDateTime(uccompletiondate.Text)
                    , General.GetNullableInteger(ViewState["TASKMODE"].ToString())
                    , General.GetNullableInteger(ViewState["RATYPE"].ToString())
                    , General.GetNullableInteger(rblDepartmentType.SelectedValue));

                    ViewState["Taskid"] = Taskid;

                    Bind();
                    ucStatus.Text = "Information Updated";
                }
                else
                {
                    Guid? Taskid = General.GetNullableGuid(ViewState["Taskid"].ToString());

                    PhoenixInspectionRiskAssessmentMachineryExtn.InsertMachinerySafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["RAId"].ToString())
                    , ref Taskid
                    , General.GetNullableString(txtTask.Text.Trim())
                    , General.GetNullableInteger(ViewState["Vesselid"].ToString())
                    , General.GetNullableInteger(pic.ToString())
                    , General.GetNullableString(picname)
                    , General.GetNullableString(null)
                    , General.GetNullableDateTime(uctargetdate.Text)
                    , General.GetNullableString(txtCompletionRemarks.Text.Trim())
                    , General.GetNullableDateTime(uccompletiondate.Text)
                    , General.GetNullableInteger(ViewState["TASKMODE"].ToString())
                    , General.GetNullableInteger(ViewState["RATYPE"].ToString())
                    , General.GetNullableInteger(rblDepartmentType.SelectedValue));

                    ucStatus.Text = "Information Updated";
                }
            }

            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (ViewState["callfrom"].ToString() == "OfficeTask")
                {
                    Response.Redirect("../Inspection/InspectionRAOfficeTask.aspx?", false);
                }
                else if (ViewState["callfrom"].ToString() == "ShipTask")
                {
                    Response.Redirect("../Inspection/InspectionRAShipTask.aspx?", false);
                }
                else if (ViewState["RATYPE"].ToString() == "5")
                {
                    Response.Redirect("../Inspection/InspectionRAMachineryExtn.aspx?machineryid=" + ViewState["RAId"].ToString() + "&status=" + ViewState["Status"].ToString(), false);
                }
                else if (ViewState["RATYPE"].ToString() == "4")
                {
                    Response.Redirect("../Inspection/InspectionRANavigationExtn.aspx?navigationid=" + ViewState["RAId"].ToString() + "&status=" + ViewState["Status"].ToString(), false);
                }
                else if (ViewState["RATYPE"].ToString() == "3")
                {
                    Response.Redirect("../Inspection/InspectionRAGenericExtn.aspx?genericid=" + ViewState["RAId"].ToString() + "&status=" + ViewState["Status"].ToString(), false);
                }
                else if (ViewState["RATYPE"].ToString() == "6")
                {
                    Response.Redirect("../Inspection/InspectionRACargoExtn.aspx?genericid=" + ViewState["RAId"].ToString() + "&status=" + ViewState["Status"].ToString(), false);
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

        int? pic;

        if (rblDepartmentType.SelectedValue.Equals("2"))
            pic = General.GetNullableInteger(ucUser.SelectedValue);
        else
            pic = General.GetNullableInteger(ucCrew.SelectedCrew);

        if (General.GetNullableGuid(ViewState["RAId"].ToString()) == null)
            ucError.ErrorMessage = "RA is required";

        if (General.GetNullableString(txtTask.Text.Trim()) == null)
            ucError.ErrorMessage = "Task is required";

        if (ViewState["Vesselid"].ToString()!="0")
        {
            if (General.GetNullableDateTime(uctargetdate.Text) == null)
                ucError.ErrorMessage = "Target Date is required";

            if (General.GetNullableInteger(pic.ToString()) == null)
                ucError.ErrorMessage = "Responsiblity is required";
        }

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }


    protected void rblDepartmentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblDepartmentType.SelectedValue.Equals("2"))
        {
            ucUser.Visible = true;
            ucCrew.Visible = false;
        }
        else
        {
            ucUser.Visible = false;
            ucCrew.Visible = true;
            ucCrew.VesselId = ViewState["Vesselid"].ToString();
            ucCrew.DataBind();
        }
    }
}