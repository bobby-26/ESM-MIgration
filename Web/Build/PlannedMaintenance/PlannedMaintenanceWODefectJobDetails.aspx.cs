using System;
using System.Web.UI;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Configuration;
using System.Data;
using SouthNests.Phoenix.Common;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using SouthNests.Phoenix.Registers;

public partial class PlannedMaintenance_PlannedMaintenanceWODefectJobDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Create Job", "WOORDER", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

        MenuWOWorkRequest.MenuList = toolbarmain.Show();

        PhoenixToolbar toolbarnavigate = new PhoenixToolbar();
        toolbarnavigate.AddButton("List", "LIST", ToolBarDirection.Left);
        toolbarnavigate.AddButton("Defect Job", "DETAILS", ToolBarDirection.Left);
        

        MenuNavigate.MenuList = toolbarnavigate.Show();
        MenuNavigate.SelectedMenuIndex = 1;

        if (!IsPostBack)
        {
            ViewState["DefectId"] = null;
            ViewState["WORKORDERID"] = string.Empty;
            ViewState["GROUPID"] = string.Empty;

            if (!string.IsNullOrEmpty(Request.QueryString["DefectId"]))
                ViewState["DefectId"] = Request.QueryString["DefectId"].ToString();

            if(!string.IsNullOrEmpty(Request.QueryString["WORKORDERID"]))
                ViewState["WORKORDERID"] = Request.QueryString["WORKORDERID"].ToString();

            ViewState["JOBS"] = "";
            pnlDryDock.Visible = false;
            ucMaintClass.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCLASS)).ToString();
            ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
            BindCheckBox();
            BindFields();
        }
    }
    private void BindFields()
    {
        DataSet ds = PhoenixPlannedMaintenanceDefectJob.DefectJobedit(new Guid(ViewState["DefectId"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ucComponent.Text = dr["FLDCOMPONENTNAME"].ToString();
            ucComponent.SelectedValue = dr["FLDCOMPONENTID"].ToString();
            txtTitle.Text = dr["FLDTITLE"].ToString();
            txtWorkDetails.Text = dr["FLDDETAILS"].ToString();
            txtPlannedStartDate.Text = General.GetDateTimeToString(dr["FLDDUEDATE"].ToString());

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
            ViewState["WORKORDERID"] = dr["FLDWORKORDERID"].ToString();
            hdnGroupID.Value = dr["FLDWOGROUPID"].ToString();

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

            txtPriority.Text = dr["FLDPRIORITY"].ToString();
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
                DataSet ds1 = PhoenixPlannedMaintenanceWorkOrder.EditWorkRequest(new Guid(dr["FLDWORKORDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);

                DataRow dr1 = ds1.Tables[0].Rows[0];
                ucWTOApproval.SelectedHard = dr1["FLDAPPROVALTYPE"].ToString();
                ucMaintClass.SelectedQuick = dr1["FLDWORKMAINTNANCECLASS"].ToString();
                ucMainType.SelectedQuick = dr1["FLDWORKMAINTNANCETYPE"].ToString();

                RadBindCheckBoxList(cblEnclosed, dr1["FLDENCLOSED"].ToString());
                RadBindCheckBoxList(cblMaterial, dr1["FLDMATERIAL"].ToString());
                RadBindCheckBoxList(cblWorktobeSurveyedBy, dr1["FLDWORKSURVEYBY"].ToString());
                RadBindCheckBoxList(cblInclude, dr1["FLDINCLUDE"].ToString());
                ViewState["WORKORDERID"] = dr["FLDWORKORDERID"].ToString();
            }
            if (ds.Tables[1].Rows.Count > 0)
            {
                BindLinkedJobs(ds.Tables[1]);
            }


            if (General.GetNullableInteger(dr["FLDDDJOB"].ToString()) == 1)
                pnlDryDock.Visible = true;
            else
                pnlDryDock.Visible = false;

            txtActionRequired.Text = dr["FLDACTIONREQUIRED"].ToString();
            ucVIQCode.Text = dr["FLDVIQCODE"].ToString();
            if(dr["FLDSTATUSID"].ToString() == "6")
            {
                MenuWOWorkRequest.Visible = false;
            }
        }
        else
        {
            ResetTextBox();
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
                string componentId = "", jobId = "";

                componentId = (ucComponent.SelectedValue).ToString();

                if (!IsValidRequisition(componentId, txtTitle.Text, txtPlannedStartDate.Text, jobId, txtWorkDetails.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                SaveDefect();
                ucStatus.Show("Saved Successfully");
                BindFields();
            }
            else if(CommandName.ToUpper().Equals("WOORDER"))
            {
                if (!IsValidWO())
                {
                    ucError.Visible = true;
                    return;
                }

                
                SaveDefect();

                Guid? workorderid = General.GetNullableGuid(ViewState["WORKORDERID"].ToString());
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

                byte? raRequired = rbtRaRequired.SelectedValue == "1" ? byte.Parse("1") : byte.Parse("0");
                byte? ddJob = rbtDrydock.SelectedValue == "1" ? byte.Parse("1"): byte.Parse("0");
                
                PhoenixPlannedMaintenanceDefectJob.WorkrOrderCreate(
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                General.GetNullableString(txtTitle.Text.Trim()), General.GetNullableGuid(ucComponent.SelectedValue),
                General.GetNullableGuid(""),
                null, General.GetNullableInteger(ucMainType.SelectedQuick),
                General.GetNullableInteger(txtPriority.Text), General.GetNullableInteger(""),
                General.GetNullableDateTime(txtPlannedStartDate.Text),
                General.GetNullableInteger(ucDiscipline.SelectedDiscipline), General.GetNullableInteger(ucMainType.SelectedQuick), General.GetNullableInteger(ucMaintClass.SelectedQuick), null,
                "0", General.GetNullableInteger(ucWTOApproval.SelectedHard),
                isDefect, ref workorderid, strWrk, strInc, strMat, strEnc, General.GetNullableInteger(ddlJobType.SelectedValue), txtWorkDetails.Text
                , raRequired, ddJob
            );

                ViewState["WORKORDERID"] = workorderid;
                //PhoenixPlannedMaintenanceDefectJob.DefectWorkOrderMap(new Guid(ViewState["DefectId"].ToString()), new Guid(ViewState["WORKORDERID"].ToString()));
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
                    PhoenixPlannedMaintenanceDefectJob.DefectWorkOrderMap(new Guid(ViewState["DefectId"].ToString()), new Guid(ViewState["WORKORDERID"].ToString()), null);
                    //if (!string.IsNullOrEmpty(strLinkedJobs))
                    //    strLinkedJobs = "," + strLinkedJobs;

                    //Guid? groupId = null;
                    //string csvjobList = "," + strLinkedJobs + ViewState["WORKORDERID"].ToString() + ",";
                    //PhoenixPlannedMaintenanceWorkOrderGroup.GroupCreate(csvjobList, null, ref groupId, 1, General.GetNullableDateTime(txtPlannedStartDate.Text), txtTitle.Text, General.GetNullableInteger(ucDiscipline.SelectedDiscipline));
                    //ViewState["GROUPID"] = groupId.ToString();
                    //if (General.GetNullableGuid(ViewState["GROUPID"].ToString()) != null)
                    ucStatus.Show("Job Created");
                }

                BindFields();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SaveDefect()
    {
        string componentId = "", jobId = "";

        componentId = (ucComponent.SelectedValue).ToString();

        if (!IsValidRequisition(componentId, txtTitle.Text, txtPlannedStartDate.Text, jobId, txtWorkDetails.Text))
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
                                                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
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
                                                                ,General.GetNullableString(txtActionRequired.Text)
                                                                ,General.GetNullableDecimal(ucVIQCode.Text)
                                                                );

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
        if (General.GetNullableInteger(ucMaintClass.SelectedQuick) == null)
            ucError.ErrorMessage = "Maintenance Class is required";
        if (General.GetNullableInteger(ucMainType.SelectedQuick) == null)
            ucError.ErrorMessage = "Maintenance Type is required";

        return (!ucError.IsError);
    }
    private bool IsValidWO()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(rbtSpare.SelectedValue) == null || General.GetNullableInteger(rbtAffectNavigation.SelectedValue) == null
            || General.GetNullableInteger(rbtDrydock.SelectedValue) == null || General.GetNullableInteger(rbtEquipmentTaken.SelectedValue) == null
            || General.GetNullableInteger(rbtIncident.SelectedValue) == null || General.GetNullableInteger(rbtInternalDefect.SelectedValue) == null
            || General.GetNullableInteger(rbtRaRequired.SelectedValue) == null || General.GetNullableInteger(rbtShoreAssistant.SelectedValue) == null
            || General.GetNullableInteger(rbtSpare.SelectedValue) == null)
            ucError.ErrorMessage = "All the questions must be answered to create Work Order";

         if (rbtRaRequired.SelectedValue == "1" && General.GetNullableGuid(hdnRAID.Value) == null)
        {
            ucError.ErrorMessage = "RA is required";
        
        }

        if (rbtRaRequired.SelectedValue == "1" && General.GetNullableGuid(hdnRAID.Value) != null && txtRANumber.Text.ToUpper().Equals("UNDEFINED"))
        {
            ucError.ErrorMessage = "RA approval required";

        }
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
        txtActionRequired.Text = "";
        ucVIQCode.Text = "";
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
        //Response.Redirect("../Purchase/PurchaseFormType.aspx?", false);

        //if (rbtSpare.SelectedValue == "0")
        //{
        //    PhoenixPlannedMaintenanceDefectJob.CreateRequisition(new Guid(ViewState["DefectId"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        //    ucStatus.Show("Requisition Created Successfully");
        //    BindFields();
        //}
        Filter.CurrentVesselConfiguration = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        //Script += "  setTimeout(function(){ top.openNewWindow('req', 'Requisition', '" + Session["sitepath"] + "/Purchase/PurchaseFormType.aspx?cID="+ucComponent.SelectedValue+"&DefectId=" + ViewState["DefectId"].ToString() + "&Title=" + txtTitle.Text + "')},1000)";
        Script += "  setTimeout(function(){ top.openNewWindow('req', 'Requisitions', '" + Session["sitepath"] + "/Purchase/PurchaseDefectJobRequisitionMap.aspx?cID=" + ucComponent.SelectedValue + "&DEFECTID=" + ViewState["DefectId"].ToString() + "&Title=" + txtTitle.Text + "&VESSELID=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "')},1000)";
        Script += "</script>" + "\n";
        RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);

        //string script = "$modalWindow.modalWindowUrl = '../Purchase/PurchaseFormType.aspx?DefectId=" + ViewState["DefectId"].ToString() + "&Title=" + txtTitle.Text + "';showDialog('Requisition');";
        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);

    }

    protected void lnkRaCreate_Click(object sender, EventArgs e)
    {
        if (General.GetNullableGuid(hdnRAID.Value) == null)
            Response.Redirect(Session["sitepath"] + "/Inspection/InspectionRAMachineryExtn.aspx?DefectId=" + ViewState["DefectId"].ToString() + "&DueDate=" + txtPlannedStartDate.Text);
        else
            Response.Redirect(Session["sitepath"] + "/Inspection/InspectionRAMachineryExtn.aspx?machineryid="+hdnRAID.Value+"&DefectId=" + ViewState["DefectId"].ToString() + "&DueDate=" + txtPlannedStartDate.Text);
    }

    protected void lnkIncidentCreate_Click(object sender, EventArgs e)
    {
        if(rbtIncident.SelectedValue == "1")
        {
            //string Script = "";
            //Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            //Script += "  setTimeout(function(){ openNewWindow('spnPickReason', 'codehelp1', '" + Session["sitepath"] + "/Inspection/InspectionIncidentAdd.aspx?DefectId=" + ViewState["DefectId"].ToString() + "&Title=" + txtTitle.Text + "','false')},1000)";
            //Script += "</script>" + "\n";
            //RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);

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

    //protected void RadWorkorder_ItemsRequested(object sender, RadComboBoxItemsRequestedEventArgs e)
    //{
    //    int iRowCount = 0;
    //    int iTotalPageCount = 0;
    //    int pageNumber = 1;
    //    int itemOffset = e.NumberOfItems;
    //    int count = 0;
    //    int ItemsPerRequest = 10;
    //    ItemsPerRequest = General.ShowRecords(null);
    //    DataSet ds = new DataSet();
    //    if (e.NumberOfItems > 0)
    //    {
    //        pageNumber = ((int)Math.Ceiling((decimal)itemOffset / ItemsPerRequest)) + 1;
    //    }
    //    e.Text = e.Text != null ? ((e.Text != string.Empty && e.Text.Contains("-")) ? e.Text.Substring(e.Text.IndexOf("-") + 1).Trim() : e.Text) : "";
    //    ds = PhoenixCommonPlannedMaintenance.WorkOrderSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, null, General.GetNullableGuid("")
    //        , null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 1,
    //           pageNumber,
    //           ItemsPerRequest,
    //           ref iRowCount,
    //           ref iTotalPageCount);

    //    if (ds.Tables.Count > 0)
    //    {
    //        DataTable dt = ds.Tables[0];
    //        count = dt.Rows.Count;
    //        e.EndOfItems = (itemOffset + count) == iRowCount;
    //        RadComboBoxItem Item;
    //        foreach (DataRow dr in dt.Rows)
    //        {
    //            Item = new RadComboBoxItem();

    //            Item.Text = dr["FLDTITLE"].ToString();
    //            Item.Value = dr["FLDWORKORDERID"].ToString();
    //            RadWorkorder.Items.Add(Item);
    //        }
    //    }
    //    RadWorkorder.DataSource = ds;
    //    RadWorkorder.DataBind();
    //    //RadWorkorder.SelectedValue = SelectedValue;

    //    foreach(RadComboBoxItem item in RadWorkorder.Items)
    //    {
    //        if (ViewState["JOBS"].ToString().Contains(item.Value))
    //            item.Checked = true;
    //    }

    //    string message = string.Empty;
    //    if (iRowCount <= 0)
    //        message = "No matches";
    //    else
    //        message = String.Format("Items <b>1</b>-<b>{0}</b> out of <b>{1}</b>", (itemOffset + count), iRowCount);
    //    e.Message = message;
    //}

    //protected void RadWorkorder_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
    //{

    //}

    //protected void RadWorkorder_ItemChecked(object sender, RadComboBoxItemEventArgs e)
    //{
    //    List<Guid> jobs = ViewState["JOBS"].ToString().Split(',').Select(Guid.Parse).ToList();

    //    if (e.Item.Checked)
    //    {
    //        if (!jobs.Contains(new Guid(e.Item.Value)))
    //            jobs.Add(new Guid(e.Item.Value));

    //    }
    //    else
    //    {
    //        if (jobs.Contains(new Guid(e.Item.Value)))
    //            jobs.Remove(new Guid(e.Item.Value));
    //    }

    //    if (jobs.Count > 0)
    //        ViewState["JOBS"] = string.Join(",", jobs.ToArray());
    //    else
    //        ViewState["JOBS"] = "";

    //}

    protected void ImgLink_Click(object sender, ImageClickEventArgs e)
    {
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "  setTimeout(function(){ openNewWindow('spnPickReason', 'codehelp1', '" + Session["sitepath"] + "/PlannedMaintenance/PlannedMaintenanceDefectLinkJobs.aspx?DefectId="+ViewState["DefectId"].ToString()+"&COMPONENT=" + ucComponent.Text + "','false')},1000)";
        Script += "</script>" + "\n";
        RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindFields();
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

    protected void ucMaintClass_TextChangedEvent(object sender, EventArgs e)
    {
        DataSet ds = PhoenixRegistersQuick.GetQuickCode(30,"DOK");
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ds.Tables[0].Rows[0]["FLDQUICKCODE"].ToString() == ucMaintClass.SelectedQuick)
            {
                rbtDrydock.SelectedValue = "1";
                pnlDryDock.Visible = true;
            }
            else
            {
                pnlDryDock.Visible = false;
                rbtDrydock.SelectedValue = "0";
            }
            
            
        }
        
    }

    //protected void lnkRequisitionMap_Click(object sender, EventArgs e)
    //{
    //    Filter.CurrentVesselConfiguration = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
    //    string Script = "";
    //    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
    //    Script += "  setTimeout(function(){ top.openNewWindow('req', 'ReqMap', '" + Session["sitepath"] + "/Purchase/PurchaseDefectJobRequisitionMap.aspx?cID=" + ucComponent.SelectedValue + "&DEFECTID=" + ViewState["DefectId"].ToString() + "&Title=" + txtTitle.Text + "&VESSELID="+ PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() + "')},1000)";
    //    Script += "</script>" + "\n";
    //    RadScriptManager.RegisterClientScriptBlock(Page, typeof(Page), "BookMarkScript", Script, false);
    //}
}