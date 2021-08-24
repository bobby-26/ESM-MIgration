using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Configuration;

public partial class PlannedMaintenanceWorkOrderRequisition : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        imgComponent.Attributes.Add("onclick", "javascript:return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx', true);");
        imgJob.Attributes.Add("onclick", "javascript:return showPickList('spnPickListJob', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJob.aspx', true);");

        if (!IsPostBack)
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);

            MenuWorkOrderRequestion.MenuList = toolbarmain.Show();
            txtComponentId.Attributes.Add("style", "visibility:hidden");
            txtJobId.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolmain = new PhoenixToolbar();
            toolmain.AddButton("General", "GENERAL");
            if (Request.QueryString["module"] != "DMR")
            {
                toolmain.AddButton("Work Order", "WORKORDER");
            }

            MenuWorkOrderRequestionTop.MenuList = toolmain.Show();
            ResetTextBox();
            ViewState["OPERATIONMODE"] = "NEW";
            MenuWorkOrderRequestionTop.SelectedMenuIndex = 0;
            BindCheckBox();
            ucMaintClass.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTCLASS)).ToString();
            ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();
            if (Request.QueryString["module"] == "DMR")
            {
                chkUnexpected.Checked = true;
                ucMaintClass.SelectedText = "MACHINERY DAMAGE";
                ucMainType.SelectedText = "BREAKDOWN";
                ucMaintClass.Enabled = false;
                ucMainType.Enabled = false;
                txtPlannedStartDate.Text = Request.QueryString["date"];
                txtComponentCode.Visible = false;
                txtComponentName.Visible = false;
                txtComponentId.Visible = false;
                imgComponent.Visible = false;
                ucComponent.Visible = true;
                cmdClear.Visible = false;

                txtJobCode.Visible = false;
                txtJobName.Visible = false;
                txtJobId.Visible = false;
                imgJob.Visible = false;
                ucJob.Visible = true;
                ImageButton1.Visible = false;
            }
        }
    }
    private void BindFields()
    {

        DataSet ds = PhoenixPlannedMaintenanceWorkOrder.EditWorkRequest(new Guid(ViewState["WORKORDERID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        DataRow dr = ds.Tables[0].Rows[0];
        lblWorkOrderID.Text = dr["FLDWORKORDERID"].ToString();
        txtWorkOrderNumber.Text = dr["FLDWORKORDERNUMBER"].ToString();
        txtTitle.Text = dr["FLDWORKORDERNAME"].ToString();
        txtJobDescription.Text = dr["FLDDETAILS"].ToString();
        if (Request.QueryString["module"] != "DMR")
        {
            txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
            txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
            txtComponentId.Text = dr["FLDCOMPONENTID"].ToString();
            txtJobId.Text = dr["FLDJOBID"].ToString();
            txtJobName.Text = dr["FLDJOBTITLE"].ToString();
            txtJobCode.Text = dr["FLDJOBCODE"].ToString();
        }
        if (Request.QueryString["module"] == "DMR")
        {
            ucComponent.SelectedValue = dr["FLDCOMPONENTID"].ToString();
            ucJob.SelectedValue = dr["FLDJOBID"].ToString();
        }
        txtCreatedDate.Text = General.GetDateTimeToString(dr["FLDCREATEDDATE"].ToString());
        if (dr["FLDWORKISUNEXPECTED"].ToString().Equals("1"))
            chkUnexpected.Checked = true;
        else
            chkUnexpected.Checked = false;
        txtDuration.Text = dr["FLDPLANNINGESTIMETDURATION"].ToString();
        txtPlannedStartDate.Text = General.GetDateTimeToString(dr["FLDPLANNINGDUEDATE"].ToString());
        txtPriority.Text = dr["FLDPLANINGPRIORITY"].ToString();
        ucDiscipline.SelectedDiscipline = dr["FLDPLANNINGDISCIPLINE"].ToString();
        ucWTOApproval.SelectedHard = dr["FLDAPPROVALTYPE"].ToString();
        BindCheckBoxList(cblEnclosed, dr["FLDENCLOSED"].ToString());
        BindCheckBoxList(cblMaterial, dr["FLDMATERIAL"].ToString());
        BindCheckBoxList(cblWorkSurvey, dr["FLDWORKSURVEYBY"].ToString());
        BindCheckBoxList(cblInclude, dr["FLDINCLUDE"].ToString());
        ucMaintClass.SelectedQuick = dr["FLDWORKMAINTNANCECLASS"].ToString();
        txtWorkDuration.Text = dr["FLDWORKDURATION"].ToString();
        txtWorkDoneDate.Text = dr["FLDWORKDONEDATE"].ToString();
        ViewState["OPERATIONMODE"] = "EDIT";

    }
    protected void BindCheckBox()
    {
        string type = string.Empty;

        cblEnclosed.DataSource = PhoenixPlannedMaintenanceWorkOrder.ListWorkOrderMultiSpec(4);
        cblEnclosed.DataTextField = "FLDNAME";
        cblEnclosed.DataValueField = "FLDMULTISPECID";
        cblEnclosed.DataBind();

        cblMaterial.DataSource = PhoenixPlannedMaintenanceWorkOrder.ListWorkOrderMultiSpec(1);
        cblMaterial.DataTextField = "FLDNAME";
        cblMaterial.DataValueField = "FLDMULTISPECID";
        cblMaterial.DataBind();

        cblInclude.DataSource = PhoenixPlannedMaintenanceWorkOrder.ListWorkOrderMultiSpec(3);
        cblInclude.DataTextField = "FLDNAME";
        cblInclude.DataValueField = "FLDMULTISPECID";
        cblInclude.DataBind();

        cblWorkSurvey.DataSource = PhoenixPlannedMaintenanceWorkOrder.ListWorkOrderMultiSpec(2);
        cblWorkSurvey.DataTextField = "FLDNAME";
        cblWorkSurvey.DataValueField = "FLDMULTISPECID";
        cblWorkSurvey.DataBind();

        ddlJobType.DataSource = PhoenixPlannedMaintenanceWorkOrder.ListWorkOrderMultiSpec(7);
        ddlJobType.DataTextField = "FLDNAME";
        ddlJobType.DataValueField = "FLDMULTISPECID";
        ddlJobType.DataBind();

    }

    private void BindCheckBoxList(RadListBox cbl, string list)
    {
        foreach (RadListBoxItem li in cbl.Items)
        {
            li.Checked = false;
        }
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                if (cbl.FindItemByValue(item) != null)
                    cbl.FindItemByValue(item).Checked = true;
            }
        }
    }
   
    protected void MenuWorkOrderRequestionTop_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("WORKORDER"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtWorkOrderNumber", txtWorkOrderNumber.Text);
                criteria.Add("txtWorkOrderName", txtTitle.Text);
                criteria.Add("txtComponentNumber", txtComponentCode.Text.TrimEnd("000.00.00".ToCharArray()));
                criteria.Add("txtComponentName", txtComponentName.Text.Trim());
                criteria.Add("ucRank", string.Empty);
                criteria.Add("txtDateFrom", string.Empty);
                criteria.Add("txtDateTo", string.Empty);
                criteria.Add("status", string.Empty);
                criteria.Add("planning", string.Empty);
                criteria.Add("jobclass", string.Empty);
                criteria.Add("ucMainType", string.Empty);
                criteria.Add("ucMainCause", string.Empty);
                criteria.Add("ucMaintClass", string.Empty);
                criteria.Add("chkUnexpected", string.Empty);
                criteria.Add("txtPriority", string.Empty);
                Filter.CurrentWorkOrderFilter = criteria;
                Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrder.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkOrderRequestion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string componentId = "", jobId = "";
                if (Request.QueryString["module"] != "DMR")
                    componentId = txtComponentId.Text;
                else if (Request.QueryString["module"] == "DMR")
                    componentId = (ucComponent.SelectedValue).ToString();

                if (Request.QueryString["module"] != "DMR")
                    jobId = txtJobId.Text;
                else if (Request.QueryString["module"] == "DMR")
                    jobId = (ucJob.SelectedValue).ToString();

                if (!IsValidRequisition(componentId, txtTitle.Text, txtPlannedStartDate.Text, jobId, txtJobDescription.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if ((String)ViewState["OPERATIONMODE"] == "NEW")
                {
                    string workorderid = null;

                    byte? isDefect = null;

                    if (chkIsDefect.Checked == true)
                        isDefect = byte.Parse("1");
                    string strEnc = string.Empty;
                    string strMat = string.Empty;
                    string strWrk = string.Empty;
                    string strInc = string.Empty;

                    foreach (RadListBoxItem item in cblMaterial.Items)
                    {
                        if (item.Selected == true)
                        {
                            strMat = strMat + item.Value.ToString() + ",";
                        }
                    }
                    strMat = strMat.TrimEnd(',');

                    foreach (RadListBoxItem item in cblEnclosed.Items)
                    {
                        if (item.Selected == true)
                        {
                            strEnc = strEnc + item.Value.ToString() + ",";
                        }
                    }
                    strEnc = strEnc.TrimEnd(',');

                    foreach (RadListBoxItem item in cblWorkSurvey.Items)
                    {
                        if (item.Selected == true)
                        {
                            strWrk = strWrk + item.Value.ToString() + ",";
                        }
                    }
                    strWrk = strWrk.TrimEnd(',');

                    foreach (RadListBoxItem item in cblInclude.Items)
                    {
                        if (item.Selected == true)
                        {
                            strInc = strInc + item.Value.ToString() + ",";
                        }
                    }
                    strInc = strInc.TrimEnd(',');
                    if (ConfigurationManager.AppSettings.Get("PhoenixTelerik") != null && ConfigurationManager.AppSettings.Get("PhoenixTelerik").ToString() == "1")
                    {
                        PhoenixPlannedMaintenanceWorkOrder.NewWorkrequestInsert(
                        PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                        General.GetNullableString(txtTitle.Text.Trim()), General.GetNullableGuid(componentId),
                        General.GetNullableGuid(txtJobId.Text),
                        General.GetNullableInteger(ucWTOApproval.SelectedHard).HasValue ? 24 : 501, General.GetNullableInteger(ucMainType.SelectedQuick),
                        General.GetNullableInteger(txtPriority.Text), General.GetNullableInteger(txtDuration.Text),
                        General.GetNullableDateTime(txtPlannedStartDate.Text),
                        General.GetNullableInteger(ucDiscipline.SelectedDiscipline), General.GetNullableInteger(ucMainType.SelectedQuick), General.GetNullableInteger(ucMaintClass.SelectedQuick), null,
                        chkUnexpected.Checked == true ? "1" : "0", General.GetNullableInteger(ucWTOApproval.SelectedHard),
                        isDefect, ref workorderid, strWrk, strInc, strMat, strEnc, General.GetNullableInteger(ddlJobType.SelectedValue), txtJobDescription.Text,
                        General.GetNullableDateTime(txtWorkDoneDate.Text), General.GetNullableDecimal(txtWorkDuration.Text));
                    }
                    else
                    {
                        PhoenixPlannedMaintenanceWorkOrder.WorkrequestInsert(
                        PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                        General.GetNullableString(txtTitle.Text.Trim()), General.GetNullableGuid(componentId),
                        General.GetNullableGuid(txtJobId.Text),
                        General.GetNullableInteger(ucWTOApproval.SelectedHard).HasValue ? 24 : 501, General.GetNullableInteger(ucMainType.SelectedQuick),
                        General.GetNullableInteger(txtPriority.Text), General.GetNullableInteger(txtDuration.Text),
                        General.GetNullableDateTime(txtPlannedStartDate.Text),
                        General.GetNullableInteger(ucDiscipline.SelectedDiscipline), General.GetNullableInteger(ucMainType.SelectedQuick), General.GetNullableInteger(ucMaintClass.SelectedQuick), null,
                        chkUnexpected.Checked == true ? "1" : "0", General.GetNullableInteger(ucWTOApproval.SelectedHard),
                        isDefect, ref workorderid, strWrk, strInc, strMat, strEnc, General.GetNullableInteger(ddlJobType.SelectedValue), txtJobDescription.Text,
                        General.GetNullableDateTime(txtWorkDoneDate.Text), General.GetNullableDecimal(txtWorkDuration.Text));
                    }

                    if (General.GetNullableDateTime(txtWorkDoneDate.Text) != null && chkUnexpected.Checked == true)
                    {
                        PhoenixToolbar toolmain = new PhoenixToolbar();
                        toolmain.AddButton("General", "GENERAL");
                        MenuWorkOrderRequestionTop.AccessRights = this.ViewState;
                        MenuWorkOrderRequestionTop.MenuList = toolmain.Show();
                        MenuWorkOrderRequestionTop.SelectedMenuIndex = 0;
                    }

                    ViewState["WORKORDERID"] = workorderid;
                    ViewState["OPERATIONMODE"] = "NEW";
                    PhoenixToolbar toolbarmain = new PhoenixToolbar();
                    toolbarmain.AddButton("New", "NEW");
                    MenuWorkOrderRequestion.AccessRights = this.ViewState;
                    MenuWorkOrderRequestion.MenuList = toolbarmain.Show();
                    ResetTextBox();
                    BindFields();

                    String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                ResetTextBox();
                ViewState["WORKORDERID"] = null;
                ViewState["OPERATIONMODE"] = "NEW";
                PhoenixToolbar toolmain = new PhoenixToolbar();
                if (Request.QueryString["module"] != "DMR")
                {
                    toolmain.AddButton("Work Order", "WORKORDER");
                }
                toolmain.AddButton("General", "GENERAL");
                MenuWorkOrderRequestionTop.MenuList = toolmain.Show();
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
                MenuWorkOrderRequestion.MenuList = toolbarmain.Show();
            }
            //if(dce.CommandName.ToUpper().Equals("WORKORDER"))
            //    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrder.aspx?COMPONENTID=" +txtComponentId.Text + "?WORKORDERID=" + lblWorkOrderID.Text);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void ResetTextBox()
    {
        txtWorkOrderNumber.Text = "";
        txtTitle.Text = "";
        txtComponentCode.Text = "";
        txtComponentName.Text = "";
        txtComponentId.Text = "";
        ucComponent.Text = "";
        //txtCreatedDate.Text =General.GetDateTimeToString(DateTime.Now.ToString());
        chkUnexpected.Checked = true;
        txtDuration.Text = "";
        txtPlannedStartDate.Text = "";
        txtPriority.Text = "3";
        ucDiscipline.SelectedDiscipline = "";
        txtJobDescription.Text = "";
        txtJobId.Text = "";
        txtJobName.Text = "";
        txtJobCode.Text = "";
        ucJob.Text = "";
        ddlJobType.SelectedIndex = -1;
        ucWTOApproval.SelectedHard = "530";
        txtWorkDoneDate.Text = "";
        txtWorkDuration.Text = "";
        txtCreatedDate.Text = "";

    }
    private bool IsValidRequisition(string componentid, string title, string plannedstartdate, string jobid, string workdetails)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (componentid.Trim().Equals(""))
            ucError.ErrorMessage = "Component is required";

        if (title.Trim().Equals(""))
            ucError.ErrorMessage = "Title is required";

        if (!General.GetNullableDateTime(plannedstartdate).HasValue)
            ucError.ErrorMessage = "Planned Start Date is required";

        if (string.IsNullOrEmpty(jobid) && string.IsNullOrEmpty(workdetails))
            ucError.ErrorMessage = "Either Job Description or Work Details is required";

        //if (ucMaintClass.SelectedText.ToLower().Contains("docking") && !General.GetNullableInteger(ddlJobType.SelectedValue).HasValue)
        if (!General.GetNullableInteger(ddlJobType.SelectedValue).HasValue)
            ucError.ErrorMessage = "Job Type is required";

        return (!ucError.IsError);
    }

    protected void cmdComponentClear_Click(object sender, ImageClickEventArgs e)
    {
        txtComponentCode.Text = "";
        txtComponentName.Text = "";
        txtComponentId.Text = "";
    }

    protected void cmdJobClear_Click(object sender, ImageClickEventArgs e)
    {
        txtJobCode.Text = "";
        txtJobName.Text = "";
        txtJobId.Text = "";
    }

    protected void chkUnplanned_CheckedChanged(Object sender, EventArgs args)
    {
        if (chkUnexpected.Checked == true)
        {
            lblTotalManHours.Visible = true;
            txtWorkDuration.Visible = true;
            lblDateDone.Visible = true;
            txtWorkDoneDate.Visible = true;
        }
        else
        {
            lblTotalManHours.Visible = false;
            txtWorkDuration.Visible = false;
            lblDateDone.Visible = false;
            txtWorkDoneDate.Visible = false;
        }
    }



    protected void ucMaintClass_TextChangedEvent(object sender, EventArgs e)
    {
        if (ucMaintClass.SelectedText == "DOCKING")
        {
            chkUnexpected.Checked = false;
        }
        else
        {
            chkUnexpected.Checked = true;
        }
    }
}
