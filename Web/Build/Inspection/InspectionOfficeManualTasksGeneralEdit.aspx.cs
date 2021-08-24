using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using System.Text;
using Telerik.Web.UI;

public partial class Inspection_InspectionOfficeManualTasksGeneralEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PREVENTIVEACTIONID"] = "";
                ViewState["COMPANYID"] = "";
                ViewState["MANUALTASKSTATE"] = "";

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }

                if (Request.QueryString["preventiveactionid"] != null)
                {
                    ViewState["PREVENTIVEACTIONID"] = Request.QueryString["preventiveactionid"].ToString();
                }
                ViewState["QUALITYCOMPANYID"] = "";
                NameValueCollection qualnvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (qualnvc.Get("QMS") != null && qualnvc.Get("QMS") != "")
                {
                    ViewState["QUALITYCOMPANYID"] = qualnvc.Get("QMS");
                    ucCompany.SelectedCompany = ViewState["QUALITYCOMPANYID"].ToString();
                    ucCompany.Enabled = false;
                }
                else
                    ucCompany.Enabled = true;

                BindCompany();
                BindCategory();
                BindSubcategory();
                BindVesselTypeList();
                BindVesselList();
                BindOfficeTask();
            }
            BindMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindVesselTypeList()
    {
        chkVesselType.Items.Clear();
        chkVesselType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 81);
        chkVesselType.DataTextField = "FLDHARDNAME";
        chkVesselType.DataValueField = "FLDHARDCODE";
        chkVesselType.DataBind();
    }
    protected void BindVesselList()
    {
        int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        chkVessel.Items.Clear();
        chkVessel.DataSource = PhoenixRegistersVessel.ListVessel(null, null, null, null, null, companyid);
        chkVessel.DataTextField = "FLDVESSELNAME";
        chkVessel.DataValueField = "FLDVESSELID";
        chkVessel.DataBind();
    }
    protected void chkVesselType_Changed(object sender, EventArgs e)
    {
        string selectedcategorylist = GetSelectedVesselType();

        foreach (ListItem item in chkVessel.Items)
            item.Selected = false;

        int? companyid = General.GetNullableInteger(ViewState["COMPANYID"].ToString());
        DataSet ds = PhoenixRegistersVessel.ListVessel(null, null, null, null, null, companyid, General.GetNullableString(selectedcategorylist));

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                foreach (ListItem item in chkVessel.Items)
                {
                    string vesselid = dr["FLDVESSELID"].ToString();

                    if (item.Value == vesselid && !item.Selected && General.GetNullableString(selectedcategorylist) != null)
                    {
                        item.Selected = true;
                        break;
                    }
                }
            }
        }
    }
    protected void chkVesselTypeAll_Changed(object sender, EventArgs e)
    {
        try
        {
            if (chkVesselTypeAll.Checked == true)
            {
                foreach (ListItem li in chkVesselType.Items)
                    li.Selected = true;

                foreach (ListItem li in chkVessel.Items)
                    li.Selected = true;
            }
            else
            {
                foreach (ListItem li in chkVesselType.Items)
                    li.Selected = false;

                foreach (ListItem li in chkVessel.Items)
                    li.Selected = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected string GetSelectedVesselType()
    {
        StringBuilder strvesseltype = new StringBuilder();
        foreach (ListItem item in chkVesselType.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strvesseltype.Append(item.Value.ToString());
                strvesseltype.Append(",");
            }
        }

        if (strvesseltype.Length > 1)
            strvesseltype.Remove(strvesseltype.Length - 1, 1);

        string vesseltype = strvesseltype.ToString();
        return vesseltype;
    }
    protected string GetSelectedVessels()
    {
        StringBuilder strVessel = new StringBuilder();
        foreach (ListItem item in chkVessel.Items)
        {
            if (item.Selected == true)
            {
                strVessel.Append(item.Value.ToString());
                strVessel.Append(",");
            }
        }

        if (strVessel.Length > 1)
            strVessel.Remove(strVessel.Length - 1, 1);

        string categoryList = strVessel.ToString();
        return categoryList;
    }
    protected void BindCategory()
    {
        ddlCategory.DataSource = PhoenixInspectionTaskCategory.InspectionTaskCategoryList();
        ddlCategory.DataTextField = "FLDCATEGORYNAME";
        ddlCategory.DataValueField = "FLDCATEGORYID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    protected void BindSubcategory()
    {
        ddlSubcategory.DataSource = PhoenixInspectionTaskCategory.InspectionTaskSubCategoryList(General.GetNullableGuid(ddlCategory.SelectedValue));
        ddlSubcategory.DataTextField = "FLDSUBCATEGORYNAME";
        ddlSubcategory.DataValueField = "FLDSUBCATEGORYID";
        ddlSubcategory.DataBind();
        ddlSubcategory.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    protected void ddlCategory_Changed(object sender, EventArgs e)
    {
        BindSubcategory();
    }
    protected void BindOfficeTask()
    {
        DataSet ds = PhoenixInspectionOfficeManualTasks.OfficeManualTasksEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableGuid(ViewState["PREVENTIVEACTIONID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["PREVENTIVEACTIONID"] = dr["FLDINSPECTIONPREVENTIVEACTIONID"].ToString();
            txtReferenceNo.Text = dr["FLDTASKREFERENCENO"].ToString();
            txtReportedBy.Text = dr["FLDMANUALTASKREPORTEDBYNAME"].ToString();
            txtReportedDate.Text = dr["FLDCREATEDDATE"].ToString();
            ddlTaskStatus.SelectedValue = dr["FLDSTATUS"].ToString();
            txtCAN.Text = dr["FLDCONTROLACTIONNEEDS"].ToString();
            txtPreventiveAction.Text = dr["FLDPREVENTIVEACTION"].ToString();
            if (Convert.ToInt32(dr["FLDMANUALGENERATETASKSYN"].ToString()) == 1)
            {
                ucTargetDate.Text = dr["FLDTARGETDATE"].ToString();
                ucpaTargetDate.Text = dr["FLDTARGETDATE"].ToString();
            }
            else
            {
                ucpaTargetDate.Text = dr["FLDTARGETDATE"].ToString();
            }
            ucVerficationLevel.SelectedHard = dr["FLDVERIFICATIONLEVEL"].ToString();
            ddlCategory.SelectedValue = dr["FLDTASKCATEGORY"].ToString();
            ucCompletionDate.Text = dr["FLDCOMPLETIONDATE"].ToString();
            txtCompletionRemarks.Text = dr["FLDTASKCOMPLETIONREMARKS"].ToString();

            ddlSubcategory.DataSource = PhoenixInspectionTaskCategory.InspectionTaskSubCategoryList(General.GetNullableGuid(dr["FLDTASKCATEGORY"].ToString()));
            ddlSubcategory.DataTextField = "FLDSUBCATEGORYNAME";
            ddlSubcategory.DataValueField = "FLDSUBCATEGORYID";
            ddlSubcategory.DataBind();
            ddlSubcategory.Items.Insert(0, new ListItem("--Select--", "Dummy"));

            ddlSubcategory.SelectedValue = General.GetNullableString(dr["FLDTASKSUBCATEGORY"].ToString());
            ucDept.DepartmentList = PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
            ucDept.SelectedDepartment = dr["FLDDEPARTMENT"].ToString();
            BindSubdepartment();
            ucSubDept.SelectedSubDepartment = dr["FLDSUBDEPARTMENTMAPPINGID"].ToString();


            ViewState["MANUALTASKSTATE"] = dr["FLDMANUALTASKSTATE"].ToString();
            txtApprovalRemarks.Text = dr["FLDMANUALTASKAPPROVALREMARKS"].ToString();
            txtApprovedDate.Text = dr["FLDMANUALTASKAPPROVEDDATE"].ToString();
            txtApprovedBy.Text = dr["FLDMANUALTASKAPPROVEDBYNAME"].ToString();
            txtCorrectiveAction.Text = dr["FLDMANUALTASKCORRECTIVEACTION"].ToString();
            txtDeficiencyDetails.Text = dr["FLDMANUALTASKDEFICIENCYDETAILS"].ToString();
            if (Convert.ToInt32(dr["FLDMANUALGENERATETASKSYN"].ToString()) == 1)
                chktaskgenerate.Checked = true;
            else
                chktaskgenerate.Checked = false;

            txtCloseOutRemarks.Text = dr["FLDREMARKS"].ToString();
            ucCloseOutDate.Text = dr["FLDPACLOSEOUTVERIFIEDDATE"].ToString();
            txtCloseOutByName.Text = dr["FLDVERIFIEDBYNAME"].ToString();
            txtCompletedByDesignation.Text = dr["FLDVERIFIEDDESIGNATIONNAME"].ToString();
            txtCompletedByName.Text = dr["FLDCOMPLETEDBYNAME"].ToString();
            txtCompletedByDesignation.Text = dr["FLDCOMPLETEDBYDESIGNATION"].ToString();
            string vessellist = dr["FLDMANUALTASKVESSELLIST"].ToString();
            General.BindCheckBoxList(chkVessel, vessellist);
            SetRights();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                ddlTaskStatus.Enabled = false;
            }

            if (!string.IsNullOrEmpty(dr["FLDTASKCOMPANYID"].ToString()))
            {
                ucCompany.SelectedCompany = dr["FLDTASKCOMPANYID"].ToString();                
            }

            ucCompany.Enabled = false;
        }
        BindMenu();
        EnableDisableControls();
    }
    private void SetRights()
    {
        ddlTaskStatus.Enabled = SessionUtil.CanAccess(this.ViewState, "TASKSTATUS");
        ucCompletionDate.Enabled = SessionUtil.CanAccess(this.ViewState, "COMPLETIONDATE");
        txtCompletionRemarks.Enabled = SessionUtil.CanAccess(this.ViewState, "COMPLETIONREMARKS");
        txtCloseOutRemarks.Enabled = SessionUtil.CanAccess(this.ViewState, "CLOSEOUTREMARKS");
    }

    protected void ucDept_TextChangedEvent(object sender, EventArgs e)
    {
        BindSubdepartment();
    }

    protected void BindSubdepartment()
    {
        ucSubDept.DepartmentFilter = ucDept.SelectedDepartment;
        ucSubDept.bind();
    }


    protected void BindMenu()
    {
        if (Request.QueryString["VIEWONLY"] == null)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "LIST",ToolBarDirection.Right);
            if (General.GetNullableGuid(ViewState["PREVENTIVEACTIONID"].ToString()) == null)
            {
                toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            }
            else
            {
                if (ViewState["MANUALTASKSTATE"].ToString() == "2")
                {
                    toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
                    if (chktaskgenerate.Checked)
                    {
                        toolbar.AddButton("Approval Request", "APPROVALREQUEST", ToolBarDirection.Right);
                    }
                }
                if (ViewState["MANUALTASKSTATE"].ToString() == "3")
                    toolbar.AddButton("Approve", "APPROVE", ToolBarDirection.Right);
                if (ViewState["MANUALTASKSTATE"].ToString() == "4")
                    toolbar.AddButton("Prepare Shipboard Task", "PREPARATION", ToolBarDirection.Right);
                if (ViewState["MANUALTASKSTATE"].ToString() == "5")
                {
                    toolbar.AddButton("Generate Shipboard Tasks", "GENERATESHIPBOARDTASKS", ToolBarDirection.Right);
                    toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                }

            }
            MenuOfficeTasksGeneral.AccessRights = this.ViewState;
            MenuOfficeTasksGeneral.MenuList = toolbar.Show();
        }
        else
        {
            ucTitle.ShowMenu = "false";
        }
    }
    protected void EnableDisableControls()
    {
        if (General.GetNullableGuid(ViewState["PREVENTIVEACTIONID"].ToString()) == null)
        {
            ddlCategory.Enabled = true;
            ddlSubcategory.Enabled = true;
            txtPreventiveAction.Enabled = true;
            txtCAN.Enabled = true;
            ddlTaskStatus.Enabled = false;

            txtCorrectiveAction.Enabled = false;
            txtDeficiencyDetails.Enabled = false;
            ucTargetDate.Enabled = false;
            ucVerficationLevel.Enabled = false;

            ucCompletionDate.Enabled = false;
            txtCompletionRemarks.Enabled = false;
            txtCloseOutRemarks.Enabled = false;

            chkVesselType.Enabled = false;
            chkVessel.Enabled = false;

        }
        if (ViewState["MANUALTASKSTATE"].ToString() == "2")
        {
            ddlCategory.Enabled = true;
            ddlSubcategory.Enabled = true;
            txtPreventiveAction.Enabled = true;
            txtCAN.Enabled = true;

            txtCorrectiveAction.Enabled = false;
            txtDeficiencyDetails.Enabled = false;
            ucTargetDate.Enabled = false;
            ucVerficationLevel.Enabled = false;

            ucCompletionDate.Enabled = true;
            txtCompletionRemarks.Enabled = true;
            txtCloseOutRemarks.Enabled = true;

            chkVesselType.Enabled = false;
            chkVessel.Enabled = false;
        }

        if (ViewState["MANUALTASKSTATE"].ToString() == "3")
        {
            ddlCategory.Enabled = false;
            ddlSubcategory.Enabled = false;
            txtPreventiveAction.Enabled = false;
            txtCAN.Enabled = false;

            txtCorrectiveAction.Enabled = false;
            txtDeficiencyDetails.Enabled = false;
            ucTargetDate.Enabled = false;
            ucVerficationLevel.Enabled = false;

            ucCompletionDate.Enabled = false;
            txtCompletionRemarks.Enabled = false;
            txtCloseOutRemarks.Enabled = false;

            chkVesselType.Enabled = false;
            chkVessel.Enabled = false;

        }
        if (ViewState["MANUALTASKSTATE"].ToString() == "4")
        {
            ddlCategory.Enabled = false;
            ddlSubcategory.Enabled = false;
            txtPreventiveAction.Enabled = false;
            txtCAN.Enabled = false;

            txtCorrectiveAction.Enabled = true;
            txtDeficiencyDetails.Enabled = true;
            ucTargetDate.Enabled = true;
            ucVerficationLevel.Enabled = true;

            ucCompletionDate.Enabled = false;
            txtCompletionRemarks.Enabled = false;
            txtCloseOutRemarks.Enabled = false;

            chkVesselType.Enabled = false;
            chkVessel.Enabled = false;

        }
        if (ViewState["MANUALTASKSTATE"].ToString() == "5")
        {
            ddlCategory.Enabled = false;
            ddlSubcategory.Enabled = false;
            txtPreventiveAction.Enabled = false;
            txtCAN.Enabled = false;

            txtCorrectiveAction.Enabled = false;
            txtDeficiencyDetails.Enabled = false;
            ucTargetDate.Enabled = false;
            ucVerficationLevel.Enabled = false;

            ucCompletionDate.Enabled = true;
            txtCompletionRemarks.Enabled = true;
            txtCloseOutRemarks.Enabled = true;

            chkVesselType.Enabled = true;
            chkVessel.Enabled = true;
        }
    }
    protected void MenuOfficeTasksGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("LIST"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
                {
                    Response.Redirect("../Inspection/InspectionLongTermActionOfficeList.aspx", true);
                }
                else
                    Response.Redirect("../Inspection/InspectionLongTermActionOfficeList.aspx", true);
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (General.GetNullableGuid(ViewState["PREVENTIVEACTIONID"].ToString()) == null)
                    InsertPreventiveAction();
                else
                    UpdatePreventiveAction();
                BindOfficeTask();
            }
            if (CommandName.ToUpper().Equals("APPROVALREQUEST"))
            {
                SendMailForApproval();

                PhoenixInspectionOfficeManualTasks.OfficeManualTasksApprovalRequest(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["PREVENTIVEACTIONID"].ToString()));
                ucStatus.Visible = true;
                ucStatus.Text = "Approval requested";
                //ucStatus.Text = "Approval request has been sent";
                BindOfficeTask();
            }
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                string script = "parent.Openpopup('Bank','','../Common/CommonApproval.aspx?docid=" + ViewState["PREVENTIVEACTIONID"].ToString() + "&mod=" + PhoenixModule.QUALITY + "&type=" + PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 98, "OMA").ToString() + "');";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
            }
            if (CommandName.ToUpper().Equals("PREPARATION"))
            {
                if (!IsValidCorrectiveAction())
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionOfficeManualTasks.OfficeManualTasksPrepareCorrectiveTask(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["PREVENTIVEACTIONID"].ToString())
                    , txtCorrectiveAction.Text
                    , General.GetNullableDateTime(ucTargetDate.Text)
                    , General.GetNullableInteger(ucVerficationLevel.SelectedHard)
                    , null
                    , General.GetNullableString(txtDeficiencyDetails.Text));

                ucStatus.Visible = true;
                ucStatus.Text = "Shipboard task prepared";

                BindOfficeTask();
            }
            if (CommandName.ToUpper().Equals("GENERATESHIPBOARDTASKS"))
            {
                string strVessels = GetSelectedVessels();
                if (!IsValidShipboardTaskGeneration(strVessels))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionOfficeManualTasks.OfficeManualTasksGenerateCorrectiveTask(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableGuid(ViewState["PREVENTIVEACTIONID"].ToString())
                    , General.GetNullableString(strVessels));

                ucStatus.Visible = true;
                ucStatus.Text = "Shipboard tasks generated";
                BindOfficeTask();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void SendMailForApproval()
    {
        try
        {
            StringBuilder toMailId = new StringBuilder();
            string fromMailId = "";
            DataTable dt = PhoenixInspectionOfficeManualTasks.OfficeManualTasksApprovalMailGet(PhoenixSecurityContext.CurrentSecurityContext.UserCode);

            if (dt.Rows.Count > 0)
            {
                if (General.IsvalidEmail(dt.Rows[0]["FLDFROMMAIL"].ToString()))
                {
                    fromMailId = dt.Rows[0]["FLDFROMMAIL"].ToString();
                }
                foreach (DataRow dr in dt.Rows)
                {
                    if (General.IsvalidEmail(dr["FLDTOMAIL"].ToString()))
                    {
                        toMailId.Append(dr["FLDTOMAIL"].ToString().ToString());
                        toMailId.Append(",");
                    }
                }
            }
            if (toMailId.Length > 0)
            {
                toMailId.Remove(toMailId.Length - 1, 1);
            }

            if (General.IsvalidEmail(toMailId.ToString()) == false)
            {
                ucError.ErrorMessage = "Invalid Email/Email not configured in Office Manual Task Approval Configuration";
                ucError.Visible = true;
                return;
            }
            string emailbodytext = "";
            emailbodytext = PrepareEmailBodyText();
            PhoenixMail.SendMail(toMailId.ToString(), fromMailId, null, "Office Manual Task Approval", emailbodytext, true, System.Net.Mail.MailPriority.Normal, "");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private string PrepareEmailBodyText()
    {
        StringBuilder sbemailbody = new StringBuilder();

        sbemailbody.Append(txtReferenceNo.Text);
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append("Dear Sir,");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append("We are requesting you to approve the Manual office Task for " + txtReferenceNo.Text + " in Phoenix");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Thank you,");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine("Regrads,");
        sbemailbody.AppendLine();
        sbemailbody.AppendLine();
        sbemailbody.Append(PhoenixSecurityContext.CurrentSecurityContext.FirstName);
        sbemailbody.Append(" ");
        sbemailbody.Append(PhoenixSecurityContext.CurrentSecurityContext.MiddleName);
        sbemailbody.Append(" ");
        sbemailbody.Append(PhoenixSecurityContext.CurrentSecurityContext.LastName);
        sbemailbody.AppendLine();

        return sbemailbody.ToString();
    }
    protected void InsertPreventiveAction()
    {
        if (!IsValidPreventiveAction())
        {
            ucError.Visible = true;
            return;
        }
        Guid? preventiveactionidout;
        preventiveactionidout = null;
        int? manualtaskgenerate = null;
        if (chktaskgenerate.Checked)
            manualtaskgenerate = 1;
        else
            manualtaskgenerate = 0;

        PhoenixInspectionOfficeManualTasks.OfficeManualTasksInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , null
            , General.GetNullableString(txtPreventiveAction.Text)
            , General.GetNullableString(txtCAN.Text)
            , General.GetNullableGuid(ddlCategory.SelectedValue)
            , General.GetNullableGuid(ddlSubcategory.SelectedValue)
            , null
            , General.GetNullableInteger(ViewState["COMPANYID"].ToString())
            , ref preventiveactionidout
            , General.GetNullableInteger(ucDept.SelectedDepartment)
            , General.GetNullableInteger(ucSubDept.SelectedSubDepartment)
            , General.GetNullableInteger(manualtaskgenerate.ToString())
            , General.GetNullableDateTime(ucpaTargetDate.Text)
            , General.GetNullableInteger(ucCompany.SelectedCompany));

        ViewState["PREVENTIVEACTIONID"] = preventiveactionidout.ToString();

        ucStatus.Text = "Information Updated.";
        ucStatus.Visible = true;
    }
    protected void UpdatePreventiveAction()
    {
        if (!IsValidPreventiveAction())
        {
            ucError.Visible = true;
            return;
        }
        int? manualtaskgenerate = null;
        if (chktaskgenerate.Checked)
            manualtaskgenerate = 1;
        else
            manualtaskgenerate = 0;

        PhoenixInspectionOfficeManualTasks.OfficeManualTasksUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableGuid(ViewState["PREVENTIVEACTIONID"].ToString())
            , null
            , General.GetNullableString(txtPreventiveAction.Text)
            , General.GetNullableString(txtCAN.Text)
            , General.GetNullableGuid(ddlCategory.SelectedValue)
            , General.GetNullableGuid(ddlSubcategory.SelectedValue)
            , General.GetNullableDateTime(ucpaTargetDate.Text)
            , null
            , General.GetNullableInteger(ucVerficationLevel.SelectedHard)
            , int.Parse(ddlTaskStatus.SelectedValue)
            , General.GetNullableString(txtCompletionRemarks.Text)
            , General.GetNullableDateTime(ucCompletionDate.Text)
            , General.GetNullableString(txtCloseOutRemarks.Text)
            , General.GetNullableInteger(ucDept.SelectedDepartment)
            , General.GetNullableInteger(ucSubDept.SelectedSubDepartment)
            , General.GetNullableInteger(manualtaskgenerate.ToString())
            , General.GetNullableInteger(ucCompany.SelectedCompany));
        ucStatus.Text = "Inofrmation Updated.";
        BindOfficeTask();
    }
    private bool IsValidPreventiveAction()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ddlCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Task category is required.";

        if (string.IsNullOrEmpty(txtPreventiveAction.Text))
            ucError.ErrorMessage = "Preventive action is required.";

        //if (General.GetNullableDateTime(ucCompletionDate.Text) != null && General.GetNullableDateTime(ucCompletionDate.Text) > DateTime.Today)
        //    ucError.ErrorMessage = "Completion Date cannot be the future date.";

        return (!ucError.IsError);
    }
    private bool IsValidCorrectiveAction()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtCorrectiveAction.Text) == null)
            ucError.ErrorMessage = "Corrective action is required.";
        if (General.GetNullableString(txtDeficiencyDetails.Text) == null)
            ucError.ErrorMessage = "Deficiency detail is required.";
        if (General.GetNullableDateTime(ucTargetDate.Text) == null)
            ucError.ErrorMessage = "Target date is required.";
        if (General.GetNullableDateTime(ucTargetDate.Text) != null && General.GetNullableDateTime(ucTargetDate.Text) < DateTime.Today)
            ucError.ErrorMessage = "Target date should be future date.";

        return (!ucError.IsError);
    }
    private bool IsValidShipboardTaskGeneration(string strVessels)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(strVessels) == null)
            ucError.ErrorMessage = "Vessel is required.";
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindOfficeTask();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindCompany()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            DataSet ds = PhoenixInspectionRiskAssessmentProcessExtn.MappedVesselCompany(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (ds.Tables[0].Rows.Count > 0)
                ucCompany.SelectedCompany = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
        }
    }
}
