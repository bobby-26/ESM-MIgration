using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web.UI;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;

public partial class Inspection_InspectionMachineryDamageDefectWorkRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Inspection/InspectionMachineryDamageDefectWorkRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvWorkOrder')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuInspectionWorkRequest.AccessRights = this.ViewState;
        MenuInspectionWorkRequest.MenuList = toolbar.Show();

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Create Job", "WORKORDER", ToolBarDirection.Right);
        toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar1.AddButton("New", "NEW", ToolBarDirection.Right);
        MenuWOWorkRequest.AccessRights = this.ViewState;
        MenuWOWorkRequest.MenuList = toolbar1.Show();

        if (!IsPostBack)
        {

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["REVIEWDNC"] = string.Empty;
            ViewState["scheduleid"] = null;
            ViewState["VESSELID"] = 0;
            ViewState["NEW"] = "false";
            ViewState["REVIEWEDYN"] = 0;
            ViewState["VESSELID"] = 0;
            ViewState["DEFICIENCYTYPE"] = 0;
            if (Request.QueryString["MACID"] != null && Request.QueryString["MACID"].ToString() != "")
            {
                ViewState["REVIEWDNC"] = Request.QueryString["MACID"].ToString();
            }

            if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != "")
            {
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            }

            if (Request.QueryString["scheduleid"] != null && Request.QueryString["scheduleid"].ToString() != "")
                ViewState["scheduleid"] = Request.QueryString["scheduleid"].ToString();

            ViewState["DefectId"] = null;
            ViewState["WORKORDERID"] = string.Empty;
            ViewState["GROUPID"] = string.Empty;

            if (!string.IsNullOrEmpty(Request.QueryString["DefectId"]))
                ViewState["DefectId"] = Request.QueryString["DefectId"].ToString();

            if (!string.IsNullOrEmpty(Request.QueryString["WORKORDERID"]))
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();

            ViewState["JOBS"] = "";
            pnlDryDock.Visible = false;
            ucMaintClass.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCLASS)).ToString();
            ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
            //GetVesselId();
            BindCheckBox();
            BindFields();
            if ((ViewState["VESSELID"] != null) && (ViewState["VESSELID"].ToString() != ""))
            {
                ucComponent.selectedvesselid = int.Parse(ViewState["VESSELID"].ToString());
            }
        }
    }
    private void BindFields()
    {
        rbtSpare.SelectedIndex = -1;
        rbtAffectNavigation.SelectedIndex = -1;
        rbtEquipmentTaken.SelectedIndex = -1;
        rbtRaRequired.SelectedIndex = -1;
        rbtShoreAssistant.SelectedIndex = -1;
        rbtDrydock.SelectedIndex = -1;
        rbtIncident.SelectedIndex = -1;
        rbtInternalDefect.SelectedIndex = -1;
        lblPriority.Visible = false;
        txtPriority.Visible = false;

        ucMaintClass.SelectedQuick = "";
        ucMainType.SelectedQuick = "";

        lblType.Visible = true;
        rdType.Visible = true;
        rdType.SelectedIndex = -1;


        if ((ViewState["DefectId"] != null) && (ViewState["DefectId"].ToString() != ""))
        {
            DataSet ds = PhoenixInspectionWorkOrder.DefectJobedit(new Guid(ViewState["DefectId"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ImgLink.Visible = false;

                tblExtraQuestion.Visible = true;
                lblPriority.Visible = true;
                txtPriority.Visible = true;

                lblType.Visible = false;
                rdType.Visible = false;

                DataRow dr = ds.Tables[0].Rows[0];

                ucComponent.Text = dr["FLDCOMPONENTNAME"].ToString();
                ucComponent.SelectedValue = dr["FLDCOMPONENTID"].ToString();
                txtTitle.Text = dr["FLDTITLE"].ToString();
                txtWorkDetails.Text = dr["FLDDETAILS"].ToString();
                txtPlannedStartDate.Text = General.GetDateTimeToString(dr["FLDDUEDATE"].ToString());
                txtPriority.Text = dr["FLDPRIORITY"].ToString();
                ucDiscipline.SelectedDiscipline = dr["FLDRESPONSIBILITY"].ToString();
                //ucVesselJob.SelectedValue = "";
                txtReason.Text = dr["FLDPOSSIBLEREASON"].ToString();
                txtActionTaken.Text = dr["FLDACTIONTAKEN"].ToString();

                if (General.GetNullableInteger(dr["FLDSPARESAVAILABLE"].ToString()) != null)
                    rbtSpare.SelectedValue = dr["FLDSPARESAVAILABLE"].ToString();
                if (General.GetNullableInteger(dr["FLDAFFECTNAVIGATION"].ToString()) != null)
                    rbtAffectNavigation.SelectedValue = dr["FLDAFFECTNAVIGATION"].ToString();
                if (General.GetNullableInteger(dr["FLDEQUIPMENTREQUIRED"].ToString()) != null)
                    rbtEquipmentTaken.SelectedValue = dr["FLDEQUIPMENTREQUIRED"].ToString();
                if (General.GetNullableInteger(dr["FLDRAREQUIRED"].ToString()) != null)
                    rbtRaRequired.SelectedValue = dr["FLDRAREQUIRED"].ToString();
                if (General.GetNullableInteger(dr["FLDASSISTANTREQUIRED"].ToString()) != null)
                    rbtShoreAssistant.SelectedValue = dr["FLDASSISTANTREQUIRED"].ToString();
                if (General.GetNullableInteger(dr["FLDDDJOB"].ToString()) != null)
                    rbtDrydock.SelectedValue = dr["FLDDDJOB"].ToString();
                if (General.GetNullableInteger(dr["FLDMACHINERYFAILURE"].ToString()) != null)
                    rbtIncident.SelectedValue = dr["FLDMACHINERYFAILURE"].ToString();

                if (General.GetNullableInteger(dr["FLDINTERNALDEFECTYN"].ToString()) != null)
                    rbtInternalDefect.SelectedValue = dr["FLDINTERNALDEFECTYN"].ToString();

                ViewState["JOBS"] = dr["FLDLINKEDJOBS"].ToString();
                txtRequisitionNo.Text = dr["FLDREQUISITIONNO"].ToString();
                hdnRequisitionID.Value = dr["FLDREQUISITIONID"].ToString();
                hdnWorkOrderID.Value = dr["FLDWORKORDERID"].ToString();
                hdnGroupID.Value = dr["FLDWOGROUPID"].ToString();

                ViewState["REVIEWEDYN"] = dr["FLDREVIEWEDYN"].ToString();

                if (rbtSpare.SelectedValue == "1")
                    lnkRequisitionCreate.Visible = false;
                else
                    lnkRequisitionCreate.Visible = true;

                if (rbtIncident.SelectedValue == "1")
                    lnkIncidentCreate.Visible = true;
                else
                    lnkIncidentCreate.Visible = false;
                if (rbtRaRequired.SelectedValue == "1")
                    lnkRaCreate.Visible = true;
                else
                    lnkRaCreate.Visible = false;

                txtWorkOrder.Text = dr["FLDWORKGROUPNO"].ToString();
                txtIncident.Text = dr["FLDMACHINERYFAILURENO"].ToString();
                hdnIncidentID.Value = dr["FLDMACHILERYFAILUREID"].ToString();
                if (General.GetNullableGuid(dr["FLDREQUISITIONID"].ToString()) != null)
                {
                    lnkRequisitionCreate.Visible = false;
                    //lnkRequisitionCreate.Text = "View Requisition";
                }

                txtRANumber.Text = dr["FLDRANUMBER"].ToString();
                hdnRAID.Value = dr["FLDRAID"].ToString();

                if (General.GetNullableGuid(dr["FLDRAID"].ToString()) != null)
                {
                    //lnkRaCreate.Visible = false;
                    lnkRaCreate.Text = "View RA";
                }
                ucMaintClass.SelectedQuick = dr["FLDMAINTENANCECLASS"].ToString();
                ucMainType.SelectedQuick = dr["FLDMAINTENANCETYPE"].ToString();

                if (General.GetNullableGuid(dr["FLDWORKORDERID"].ToString()) != null)
                {
                    DataSet ds1 = PhoenixPlannedMaintenanceWorkOrder.EditWorkRequest(new Guid(dr["FLDWORKORDERID"].ToString()), int.Parse(ViewState["VESSELID"].ToString()));

                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr1 = ds1.Tables[0].Rows[0];
                        ucWTOApproval.SelectedHard = dr1["FLDAPPROVALTYPE"].ToString();
                       
                        RadBindCheckBoxList(cblEnclosed, dr1["FLDENCLOSED"].ToString());
                        RadBindCheckBoxList(cblMaterial, dr1["FLDMATERIAL"].ToString());
                        RadBindCheckBoxList(cblWorktobeSurveyedBy, dr1["FLDWORKSURVEYBY"].ToString());
                        RadBindCheckBoxList(cblInclude, dr1["FLDINCLUDE"].ToString());
                        ViewState["WORKORDERID"] = dr["FLDWORKORDERID"].ToString();
                    }
                }
                if (ds.Tables[1].Rows.Count > 0)
                {
                    BindLinkedJobs(ds.Tables[1]);
                }


                if (General.GetNullableInteger(dr["FLDDDJOB"].ToString()) == 1)
                    pnlDryDock.Visible = true;
                else
                    pnlDryDock.Visible = false;

            }
            else
            {
                ResetTextBox();
            }
        }
        else
        {
            ImgLink.Visible = false;
            tblExtraQuestion.Visible = false;
        }
    }
    protected void MenuWOWorkRequest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                SaveDefect();
                //BindFields();
                Rebind();
                ViewState["NEW"] = "false";
            }
            if (CommandName.ToUpper().Equals("WORKORDER"))
            {
                if ((ViewState["DefectId"] == null) || (ViewState["DefectId"].ToString() == ""))
                {
                    ucError.ErrorMessage = "Save Defect job first.";
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["REVIEWEDYN"].ToString() == "0")
                {
                    ucError.ErrorMessage = "Please Review the Defect then only you can create work order or work order already created.";
                    ucError.Visible = true;
                    return;
                }

                if (!IsValidWorkorder())
                {
                    ucError.Visible = true;
                    return;
                }

                SaveDefect();

                string componentid = (ucComponent.SelectedValue).ToString();
                string Details = txtWorkDetails.Text;
                string Duedate = txtPlannedStartDate.Text;
                string Responsibility = ucDiscipline.SelectedDiscipline;

                if (rbtRaRequired.SelectedValue == "1" && General.GetNullableGuid(hdnRAID.Value) == null)
                {
                    ucError.ErrorMessage = "RA is required";
                    ucError.Visible = true;
                    return;
                }

                if (rbtRaRequired.SelectedValue == "1" && General.GetNullableGuid(hdnRAID.Value) != null && txtRANumber.Text.ToUpper().Equals("UNDEFINED"))
                {
                    ucError.ErrorMessage = "RA approval required";
                    ucError.Visible = true;
                    return;
                }

                string workorderid = ViewState["WORKORDERID"].ToString();
                byte? isDefect = byte.Parse("1");
                string strEnc = string.Empty;
                string strMat = string.Empty;
                string strWrk = string.Empty;
                string strInc = string.Empty;

                foreach (ButtonListItem item in cblMaterial.Items)
                {
                    if (item.Selected == true)
                    {
                        strMat = strMat + item.Value.ToString() + ",";
                    }
                }
                strMat = strMat.TrimEnd(',');

                foreach (ButtonListItem item in cblEnclosed.Items)
                {
                    if (item.Selected == true)
                    {
                        strEnc = strEnc + item.Value.ToString() + ",";
                    }
                }
                strEnc = strEnc.TrimEnd(',');

                foreach (ButtonListItem item in cblWorktobeSurveyedBy.Items)
                {
                    if (item.Selected == true)
                    {
                        strWrk = strWrk + item.Value.ToString() + ",";
                    }
                }
                strWrk = strWrk.TrimEnd(',');

                foreach (ButtonListItem item in cblInclude.Items)
                {
                    if (item.Selected == true)
                    {
                        strInc = strInc + item.Value.ToString() + ",";
                    }
                }
                strInc = strInc.TrimEnd(',');

                byte? raRequired = byte.Parse("0");
                byte? ddJob = rbtDrydock.SelectedValue == "1" ? byte.Parse("1") : byte.Parse("0");

                PhoenixPlannedMaintenanceDefectJob.WorkrOrderCreate(
                int.Parse(ViewState["VESSELID"].ToString()),
                General.GetNullableString(txtTitle.Text.Trim()), General.GetNullableGuid(ucComponent.SelectedValue),
                General.GetNullableGuid(""),
                null, General.GetNullableInteger(ucMainType.SelectedQuick),
                General.GetNullableInteger(""), General.GetNullableInteger(""),
                General.GetNullableDateTime(txtPlannedStartDate.Text),
                General.GetNullableInteger(ucDiscipline.SelectedDiscipline), General.GetNullableInteger(ucMainType.SelectedQuick), General.GetNullableInteger(ucMaintClass.SelectedQuick), null,
                "0", General.GetNullableInteger(ucWTOApproval.SelectedHard),
                isDefect, ref workorderid, strWrk, strInc, strMat, strEnc, General.GetNullableInteger(ddlJobType.SelectedValue), txtWorkDetails.Text
                , raRequired, ddJob
                 );

                ViewState["WORKORDERID"] = workorderid;
                PhoenixPlannedMaintenanceDefectJob.DefectWorkOrderMap(new Guid(ViewState["DefectId"].ToString()), new Guid(ViewState["WORKORDERID"].ToString()));
                if (!string.IsNullOrEmpty(ViewState["WORKORDERID"].ToString())
                    && string.IsNullOrEmpty(ViewState["GROUPID"].ToString())
                    && !string.IsNullOrEmpty(ViewState["DefectId"].ToString()))
                {
                    string strLinkedJobs = string.Empty;
                    foreach (ButtonListItem item in chkLinkedJobs.Items)
                    {
                        if (item.Selected == true)
                        {
                            strLinkedJobs = strLinkedJobs + item.Value.ToString() + ",";
                        }
                    }

                    Guid? groupId = null;
                    string csvjobList = "," + strLinkedJobs + ViewState["WORKORDERID"].ToString() + ",";
                    PhoenixInspectionWorkOrder.GroupCreate(csvjobList, null, ref groupId, 1, General.GetNullableDateTime(txtPlannedStartDate.Text), txtTitle.Text, General.GetNullableInteger(ucDiscipline.SelectedDiscipline), int.Parse(ViewState["VESSELID"].ToString()));
                    ViewState["GROUPID"] = groupId.ToString();
                    if (General.GetNullableGuid(ViewState["GROUPID"].ToString()) != null)
                        PhoenixPlannedMaintenanceDefectJob.DefectWorkOrderMap(new Guid(ViewState["DefectId"].ToString()), new Guid(ViewState["WORKORDERID"].ToString()), General.GetNullableGuid(ViewState["GROUPID"].ToString()));
                }

                if ((General.GetNullableGuid(ViewState["GROUPID"].ToString()) != null) && (General.GetNullableGuid(ViewState["DefectId"].ToString()) != null) && (General.GetNullableGuid(ViewState["WORKORDERID"].ToString()) != null))
                    PhoenixInspectionWorkOrder.DefectWorkOrderMap(new Guid(ViewState["DefectId"].ToString()), new Guid(ViewState["WORKORDERID"].ToString()), new Guid(ViewState["GROUPID"].ToString()));


                gvWorkOrder.Rebind();
                BindFields();
                Rebind();
            }
            if (CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["DefectId"] = "";

                ResetTextBox();
                rbtSpare.SelectedIndex = -1;
                rbtAffectNavigation.SelectedIndex = -1;
                rbtEquipmentTaken.SelectedIndex = -1;
                rbtRaRequired.SelectedIndex = -1;
                rbtShoreAssistant.SelectedIndex = -1;
                rbtDrydock.SelectedIndex = -1;
                rbtIncident.SelectedIndex = -1;
                rbtInternalDefect.SelectedIndex = -1;
                ImgLink.Visible = false;
                lblPriority.Visible = false;
                txtPriority.Visible = false;
                lblType.Visible = true;
                rdType.Visible = true;
                rdType.SelectedIndex = -1;
                tblExtraQuestion.Visible = false;
                txtActionTaken.Text = "";
                txtReason.Text = "";
                ucMaintClass.SelectedQuick = "";
                ucMainType.SelectedQuick = "";
                ViewState["NEW"] = "true";
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidDefectjob(string Details, string Duedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(ViewState["REVIEWDNC"].ToString()))
            ucError.ErrorMessage = "Record Machinery Damage First.";

        if (string.IsNullOrEmpty(txtWorkDetails.Text))
            ucError.ErrorMessage = "Defect Details is required.";

        if (string.IsNullOrEmpty(ucComponent.SelectedValue))
            ucError.ErrorMessage = "Component is required.";

        if (string.IsNullOrEmpty(txtPlannedStartDate.Text))
            ucError.ErrorMessage = "Due Date is required.";

        if (string.IsNullOrEmpty(txtTitle.Text))
            ucError.ErrorMessage = "Title is required.";

        if (string.IsNullOrEmpty(ucDiscipline.SelectedDiscipline))
            ucError.ErrorMessage = "Responsiblity is required.";

        if (General.GetNullableInteger(rdType.SelectedValue).Equals(null))
            ucError.ErrorMessage = "Type Is Required.";

        if (General.GetNullableInteger(ucMaintClass.SelectedQuick) == null)
            ucError.ErrorMessage = "Maintenance Class is required";

        if (General.GetNullableInteger(ucMainType.SelectedQuick) == null)
            ucError.ErrorMessage = "Maintenance Type is required";

        return (!ucError.IsError);
    }

    private bool IsValidWorkorder()
    {
        ucError.HeaderMessage = "Please answer following questions.";

        if (General.GetNullableInteger(rbtSpare.SelectedValue).Equals(null))
            ucError.ErrorMessage = "Are Spares/Stores Available Onboard?";

        if (General.GetNullableInteger(rbtAffectNavigation.SelectedValue).Equals(null))
            ucError.ErrorMessage = "Is it Significant defect affecting Navigation, Safety, or Pollution prevention?";

        if (General.GetNullableInteger(rbtEquipmentTaken.SelectedValue).Equals(null))
            ucError.ErrorMessage = "Is Equipment required to be taken out of service?";

        if (General.GetNullableInteger(rbtRaRequired.SelectedValue).Equals(null))
            ucError.ErrorMessage = "RA Required?";

        if (General.GetNullableInteger(rbtShoreAssistant.SelectedValue).Equals(null))
            ucError.ErrorMessage = "Is Shore Assistance Required?";

        if (General.GetNullableInteger(rbtDrydock.SelectedValue).Equals(null))
            ucError.ErrorMessage = "Is the Job to be added to Dry Dock List?";

        if (General.GetNullableInteger(rbtIncident.SelectedValue).Equals(null))
            ucError.ErrorMessage = "Do you want to raise Machinery Incident?";

        if (General.GetNullableInteger(rbtInternalDefect.SelectedValue).Equals(null))
            ucError.ErrorMessage = "Is Internal Purpose?";



        return (!ucError.IsError);
    }


    protected void SaveDefect()
    {
        if ((ViewState["DefectId"] == null) || (ViewState["DefectId"].ToString() == ""))
        {
            string componentid = (ucComponent.SelectedValue).ToString();
            string Details = txtWorkDetails.Text;
            string Duedate = txtPlannedStartDate.Text;
            string Responsibility = ucDiscipline.SelectedDiscipline;

            if (!IsValidDefectjob(Details, Duedate))
            {
                ucError.Visible = true;
                return;
            }
            Guid defectJobId = Guid.Empty;
            PhoenixInspectionWorkOrder.DefectJobInsert(General.GetNullableGuid(ucComponent.SelectedValue), Details,
                                                               DateTime.Parse(Duedate),
                                                               General.GetNullableInteger(Responsibility),
                                                               ref defectJobId, int.Parse(ViewState["VESSELID"].ToString())
                                                               , General.GetNullableString(txtTitle.Text)
                                                               , General.GetNullableString(txtReason.Text)
                                                               , General.GetNullableString(txtActionTaken.Text)
                                                               , General.GetNullableInteger(rdType.SelectedValue)
                                                               , General.GetNullableInteger(ucMaintClass.SelectedValue)
                                                               , General.GetNullableInteger(ucMainType.SelectedValue)
                                                               );
            PhoenixInspectionWorkOrder.InspectionDefectJobMap(defectJobId, new Guid(ViewState["REVIEWDNC"].ToString()), int.Parse(ViewState["DEFICIENCYTYPE"].ToString()), null, int.Parse(ViewState["VESSELID"].ToString()), General.GetNullableString("MACH"));

            ViewState["DefectId"] = defectJobId;

            if ((General.GetNullableGuid(ViewState["REVIEWDNC"].ToString()) != null) && (General.GetNullableGuid(ViewState["DefectId"].ToString()) != null))
                PhoenixInspectionWorkOrder.DefectMachineryMap(new Guid(ViewState["DefectId"].ToString()), new Guid(ViewState["REVIEWDNC"].ToString()));

            //BindFields();
            //Rebind();
        }

        string componentId = "", jobId = "";

        componentId = (ucComponent.SelectedValue).ToString();

        if (!IsValidRequisition(componentId, txtTitle.Text, txtPlannedStartDate.Text, jobId, txtWorkDetails.Text))
        {
            ucError.Visible = true;
            return;
        }

        if (!IsValidWorkorder())
        {
            ucError.Visible = true;
            return;
        }

        string workorderid = ViewState["WORKORDERID"].ToString();
        byte? isDefect = byte.Parse("1");

        string strLinkedJobs = string.Empty;
        foreach (ButtonListItem item in chkLinkedJobs.Items)
        {
            if (item.Selected == true)
            {
                strLinkedJobs = strLinkedJobs + item.Value.ToString() + ",";
            }
        }

        if (!string.IsNullOrEmpty(strLinkedJobs))
            strLinkedJobs = "," + strLinkedJobs;

        PhoenixPlannedMaintenanceDefectJob.DefectJobUpdateDetails(new Guid(ViewState["DefectId"].ToString())
                                                                , General.GetNullableString(txtWorkDetails.Text)
                                                                , General.GetNullableDateTime(txtPlannedStartDate.Text)
                                                                , General.GetNullableInteger(ucDiscipline.SelectedDiscipline)
                                                                , int.Parse(ViewState["VESSELID"].ToString())
                                                                , General.GetNullableGuid(ucComponent.SelectedValue)
                                                                , General.GetNullableString(txtTitle.Text)
                                                                , General.GetNullableString(strLinkedJobs)
                                                                , General.GetNullableString(txtReason.Text)
                                                                , General.GetNullableString(txtActionTaken.Text)
                                                                , General.GetNullableInteger(rbtSpare.SelectedValue)
                                                                , General.GetNullableInteger(rbtAffectNavigation.SelectedValue)
                                                                , General.GetNullableInteger(rbtEquipmentTaken.SelectedValue)
                                                                , General.GetNullableInteger(rbtRaRequired.SelectedValue)
                                                                , General.GetNullableInteger(rbtShoreAssistant.SelectedValue)
                                                                , General.GetNullableInteger(rbtDrydock.SelectedValue)
                                                                , General.GetNullableInteger(rbtIncident.SelectedValue)
                                                                , General.GetNullableInteger(rbtInternalDefect.SelectedValue)
                                                                , General.GetNullableInteger(ucMaintClass.SelectedValue)
                                                                , General.GetNullableInteger(ucMainType.SelectedValue)
                                                                );
        //Rebind();
        //BindFields();

    }
    private bool IsValidRequisition(string componentid, string title, string plannedstartdate, string jobid, string workdetails)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (componentid.Trim().Equals(""))
            ucError.ErrorMessage = "Component is required";

        if (title.Trim().Equals(""))
            ucError.ErrorMessage = "Title is required";

        if (!General.GetNullableDateTime(plannedstartdate).HasValue)
            ucError.ErrorMessage = "Due Date is required";

        if (string.IsNullOrEmpty(jobid) && string.IsNullOrEmpty(workdetails))
            ucError.ErrorMessage = "Either Job Description or Work Details is required";

        if (General.GetNullableInteger(ucDiscipline.SelectedDiscipline) == null)
            ucError.ErrorMessage = "Responsiblity is required.";

        if (General.GetNullableInteger(ucMaintClass.SelectedQuick) == null)
            ucError.ErrorMessage = "Maintenance Class is required";

        if (General.GetNullableInteger(ucMainType.SelectedQuick) == null)
            ucError.ErrorMessage = "Maintenance Type is required";

        return (!ucError.IsError);
    }
    protected void BindCheckBox()
    {
        cblWorktobeSurveyedBy.DataSource = PhoenixPlannedMaintenanceWorkOrder.ListWorkOrderMultiSpec(2);
        cblWorktobeSurveyedBy.DataBindings.DataTextField = "FLDNAME";
        cblWorktobeSurveyedBy.DataBindings.DataValueField = "FLDMULTISPECID";
        cblWorktobeSurveyedBy.DataBind();

        cblMaterial.DataSource = PhoenixPlannedMaintenanceWorkOrder.ListWorkOrderMultiSpec(1);
        cblMaterial.DataBindings.DataTextField = "FLDNAME";
        cblMaterial.DataBindings.DataValueField = "FLDMULTISPECID";
        cblMaterial.DataBind();

        cblEnclosed.DataSource = PhoenixPlannedMaintenanceWorkOrder.ListWorkOrderMultiSpec(4);
        cblEnclosed.DataBindings.DataTextField = "FLDNAME";
        cblEnclosed.DataBindings.DataValueField = "FLDMULTISPECID";
        cblEnclosed.DataBind();

        cblInclude.DataSource = PhoenixPlannedMaintenanceWorkOrder.ListWorkOrderMultiSpec(3);
        cblInclude.DataBindings.DataTextField = "FLDNAME";
        cblInclude.DataBindings.DataValueField = "FLDMULTISPECID";
        cblInclude.DataBind();

        ddlJobType.DataSource = PhoenixPlannedMaintenanceWorkOrder.ListWorkOrderMultiSpec(7);
        ddlJobType.DataTextField = "FLDNAME";
        ddlJobType.DataValueField = "FLDMULTISPECID";
        ddlJobType.DataBind();
    }
    protected void BindLinkedJobs(DataTable dt)
    {
        chkLinkedJobs.DataSource = dt;
        chkLinkedJobs.DataBindings.DataTextField = "FLDWORKORDERNAME";
        chkLinkedJobs.DataBindings.DataValueField = "FLDWORKORDERID";
        chkLinkedJobs.DataBind();

        foreach (ButtonListItem item in chkLinkedJobs.Items)
        {
            item.Selected = true;
        }
    }
    public static void RadBindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                cbl.SelectedValue = item;
            }
        }
    }

    private void ResetTextBox()
    {
        txtTitle.Text = "";
        ucComponent.Text = "";
        txtPlannedStartDate.Text = "";
        ucDiscipline.SelectedDiscipline = "";
        txtWorkDetails.Text = "";
        ddlJobType.SelectedIndex = -1;
        ucWTOApproval.SelectedHard = "530";
        rbtDrydock.SelectedValue = "0";
        pnlDryDock.Visible = false;
        cblMaterial.SelectedItems.Clear();
        cblEnclosed.SelectedItems.Clear();
        cblInclude.SelectedItems.Clear();
        cblWorktobeSurveyedBy.SelectedItems.Clear();
    }

    protected void rbtDrydock_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtDrydock.SelectedValue == "1")
        {
            pnlDryDock.Visible = true;
            ucMaintClass.SelectedText = "DOCKING";
        }
        else
        {
            pnlDryDock.Visible = false;
        }
    }
    protected void lnkRequisitionCreate_Click(object sender, EventArgs e)
    {
        Filter.CurrentVesselConfiguration = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "  setTimeout(function(){ top.openNewWindow('req', 'Requisition', '" + Session["sitepath"] + "/Purchase/PurchaseFormType.aspx?cID=" + ucComponent.SelectedValue + "&DefectId=" + ViewState["DefectId"].ToString() + "&Title=" + txtTitle.Text + "')},1000)";
        Script += "</script>" + "\n";
        RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);

    }

    protected void lnkRaCreate_Click(object sender, EventArgs e)
    {
        if (General.GetNullableGuid(hdnRAID.Value) == null)
            Response.Redirect(Session["sitepath"] + "/Inspection/InspectionRAMachineryExtn.aspx?DefectId=" + ViewState["DefectId"].ToString() + "&DueDate=" + txtPlannedStartDate.Text);
        else
            Response.Redirect(Session["sitepath"] + "/Inspection/InspectionRAMachineryExtn.aspx?machineryid=" + hdnRAID.Value + "&DefectId=" + ViewState["DefectId"].ToString() + "&DueDate=" + txtPlannedStartDate.Text);
    }

    protected void lnkIncidentCreate_Click(object sender, EventArgs e)
    {
        if (rbtIncident.SelectedValue == "1")
        {

            string script = "$modalWindow.modalWindowUrl = '../Inspection/InspectionIncidentAdd.aspx?DefectId=" + ViewState["DefectId"].ToString() + "&Title=" + txtTitle.Text + "';showDialog('Incident');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

        }

    }

    protected void MenuNavigate_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect(Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectListRegister.aspx?DefectId=" + ViewState["DefectId"].ToString());
        }
    }

    protected void ImgLink_Click(object sender, ImageClickEventArgs e)
    {
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "  setTimeout(function(){ openNewWindow('spnPickReason', 'codehelp1', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectLinkJobs.aspx?DefectId=" + ViewState["DefectId"].ToString() + "&COMPONENT=" + ucComponent.Text + "','false')},1000)";
        Script += "</script>" + "\n";
        RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindFields();
            gvWorkOrder.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void rbtSpare_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtSpare.SelectedValue == "1")
            lnkRequisitionCreate.Visible = false;
        else
            lnkRequisitionCreate.Visible = true;

        if (General.GetNullableGuid(hdnRequisitionID.Value) != null)
            lnkRequisitionCreate.Visible = false;
    }
    protected void rbtIncident_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtIncident.SelectedValue == "1")
            lnkIncidentCreate.Visible = true;
        else
            lnkIncidentCreate.Visible = false;
    }
    protected void rbtRaRequired_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rbtRaRequired.SelectedValue == "1")
            lnkRaCreate.Visible = true;
        else
            lnkRaCreate.Visible = false;

    }

    protected void MenuInspectionWorkRequest_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDDEFECTNO", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDRANUMBER" };
            string[] alCaptions = { "Defect No", "Component Number", "Component Name", "Work Order Number", "Work Order Title", "Resp Discipline", "Status", "RA Number" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixInspectionAuditDirectNonConformity.InspectionDirectNCDefectWorkRequestSearch(General.GetNullableGuid(ViewState["REVIEWDNC"].ToString()),
                             null, Convert.ToInt32(ViewState["VESSELID"].ToString()),
                             sortexpression, sortdirection,
                             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                             General.ShowRecords(iRowCount),
                             ref iRowCount,
                             ref iTotalPageCount);

            General.SetPrintOptions("gvWorkOrder", "Defect Work Request", alCaptions, alColumns, ds);

            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["OPERATIONMODE"] = "EDIT";
                gvWorkOrder.SelectedIndexes.Clear();

                //if (ViewState["NEW"] != null && ViewState["NEW"].ToString().ToUpper() != "TRUE")
                //{
                //    ViewState["WORKORDERID"] = ds.Tables[0].Rows[0]["FLDWORKORDERID"].ToString();
                //    ViewState["DefectId"] = ds.Tables[0].Rows[0]["FLDDEFECTJOBID"].ToString();
                //    BindFields();                   
                //}
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ViewState["NEW"] = "true";
                ViewState["DefectId"] = "";
                tblExtraQuestion.Visible = false;

            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDEFECTNO", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDWORKORDERNUMBER", "FLDWORKORDERNAME", "FLDDISCIPLINENAME", "FLDHARDNAME", "FLDRANUMBER" };
        string[] alCaptions = { "Defect No", "Component Number", "Component Name", "Work Order Number", "Work Order Title", "Resp Discipline", "Status", "RA Number" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionAuditDirectNonConformity.InspectionDirectNCDefectWorkRequestSearch(General.GetNullableGuid(ViewState["REVIEWDNC"].ToString()),
                              null, Convert.ToInt32(ViewState["VESSELID"].ToString()),
                              sortexpression, sortdirection,
                              Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                              General.ShowRecords(iRowCount),
                              ref iRowCount,
                              ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DefectWorkRequest.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>NC Work Request</center></h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void gvWorkOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkOrder.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvWorkOrder.SelectedIndexes.Clear();
        gvWorkOrder.EditIndexes.Clear();
        gvWorkOrder.DataSource = null;
        gvWorkOrder.Rebind();
    }
    protected void gvWorkOrder_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            RadLabel lblWorkOrderId = ((RadLabel)e.Item.FindControl("lblWorkOrderId"));
            RadLabel lbldefectid = ((RadLabel)e.Item.FindControl("lbldefectid"));

            if (lblWorkOrderId != null)
            {
                ViewState["WORKORDERID"] = lblWorkOrderId.Text;
                ViewState["DefectId"] = lbldefectid.Text;
            }
            else
            {
                ViewState["WORKORDERID"] = null;
                ViewState["DefectId"] = lbldefectid.Text;
            }

            BindFields();
        }
        else if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            ViewState["DefectId"] = (((RadLabel)e.Item.FindControl("lbldefectjobid")).Text);
            RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
            return;
        }
        //  else if (e.CommandName.ToUpper().Equals("VERIFY"))
        //  {
        //      GridDataItem item = (GridDataItem)e.Item;
        //      string id = item.GetDataKeyValue("FLDDEFECTJOBID").ToString();
        //      string no = ((RadLabel)item.FindControl("lbldefectno")).Text;
        //      string script = "$modalWindow.modalWindowUrl = '../Inspection/InspectionDefectJobVerification.aspx?defectjobId=" + id + "&defectno=" + no + "';showDialog('Edit');";
        //      ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        //  }
        else if (e.CommandName.ToUpper().Equals("COMPLETE"))
        {

            GridDataItem item = (GridDataItem)e.Item;
            string id = item.GetDataKeyValue("FLDDEFECTJOBID").ToString();
            string script = "$modalWindow.modalWindowUrl = '../PlannedMaintenance/PlannedMaintenanceDefectClose.aspx?DefectId=" + id + "';showDialog('Edit');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        }
        else if (e.CommandName.ToUpper().Equals("WORKORDER"))
        {
            RadLabel lblWorkOrderId = ((RadLabel)e.Item.FindControl("lblWorkOrderId"));
            RadLabel lbldefectid = ((RadLabel)e.Item.FindControl("lbldefectid"));

            if (lblWorkOrderId != null)
            {
                ViewState["WORKORDERID"] = lblWorkOrderId.Text;
                ViewState["DefectId"] = lbldefectid.Text;
            }
            else
            {
                ViewState["WORKORDERID"] = null;
                ViewState["DefectId"] = lbldefectid.Text;
            }

            BindFields();
        }
    }

    protected void gvWorkOrder_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            string Defectjobid = drv["FLDDEFECTJOBID"].ToString();

            ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
            ImageButton verify = (ImageButton)e.Item.FindControl("cmdverify");
            ImageButton workorder = (ImageButton)e.Item.FindControl("cmdWorkorder");
            ImageButton complete = (ImageButton)e.Item.FindControl("cmdComplete");

            if (edit != null)
            {
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
                //edit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectListUpdate.aspx?defectjobId=" + drv["FLDDEFECTJOBID"] + "',false,700,500); ");
            }
            if (verify != null)
            {
                verify.Visible = SessionUtil.CanAccess(this.ViewState, verify.CommandName);
                verify.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectJobVerification.aspx?defectjobId=" + drv["FLDDEFECTJOBID"] + "&defectno=" + drv["FLDDEFECTNO"] + "'); return false;");
            }

            if (drv["FLDWORKORDERREQUIRED"].ToString() == "0")
            {
                if (verify != null)
                {
                    verify.Visible = false;
                }
                if (complete != null)
                {
                    complete.Visible = true;
                    complete.Visible = SessionUtil.CanAccess(this.ViewState, complete.CommandName);
                    //complete.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectClose.aspx?DefectId=" + drv["FLDDEFECTJOBID"] + "',false,500,300); ");
                }
            }
            if (drv["FLDWORKORDERREQUIRED"].ToString() == "1")
            {
                if (verify != null)
                {
                    verify.Visible = false;
                }
                if (workorder != null)
                {
                    workorder.Visible = SessionUtil.CanAccess(this.ViewState, workorder.CommandName);
                    //workorder.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceWODefectJobDetails.aspx?DefectId=" + drv["FLDDEFECTJOBID"] + "&DefectNo=" + drv["FLDDEFECTNO"] + "&ComponentId=" + drv["FLDCOMPONENTID"] + "&Res=" + drv["FLDRESPONSIBILITYID"] + "&Due=" + drv["FLDDUEDATE"] + "',false,800,500); ");
                }
            }

            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null)
            {
                //del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            }


            if (!string.IsNullOrEmpty(drv["FLDDONEBY"].ToString()))
            {
                //edit.Visible = false;
                del.Visible = false;
                verify.Visible = false;
                //workorder.Visible = false;
                complete.Visible = false;

            }
            LinkButton Communication = (LinkButton)e.Item.FindControl("lnkCommunication");
            if (Communication != null)
            {
                int vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                Communication.Visible = SessionUtil.CanAccess(this.ViewState, Communication.CommandName);
                Communication.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonCommunication.aspx?Type=DEFECT" + "&Referenceid=" + drv["FLDDEFECTJOBID"] + "&Vesselid=" + vesselid + "');");
            }
            ImageButton reschedule = (ImageButton)e.Item.FindControl("cmdPostpone");
            if (reschedule != null)
            {
                reschedule.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectReschedule.aspx?dueDate=" + drv["FLDDUEDATE"] + "&defectId=" + drv["FLDDEFECTJOBID"] + "',false,500,400); ");
                reschedule.Visible = SessionUtil.CanAccess(this.ViewState, reschedule.CommandName);
            }
        }
    }

    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixInspectionWorkOrder.DefectJobDelete(new Guid(ViewState["DefectId"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucMaintClass_TextChangedEvent(object sender, EventArgs e)
    {

        DataSet ds = PhoenixRegistersQuick.GetQuickCode(30, "DOK");
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["FLDQUICKCODE"].ToString() == ucMaintClass.SelectedQuick)
            {
                rbtDrydock.SelectedValue = "1";
                pnlDryDock.Visible = true;
                tblExtraQuestion.Visible = true;

            }
            else
            {
                rbtDrydock.SelectedValue = "0";
                pnlDryDock.Visible = false;
                tblExtraQuestion.Visible = false;

            }
        }
    }

}