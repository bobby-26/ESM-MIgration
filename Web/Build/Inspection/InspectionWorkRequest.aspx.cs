using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class InspectionWorkRequest : PhoenixBasePage
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        
        txtComponentId.Attributes.Add("style", "visibility:hidden");
        txtJobId.Attributes.Add("style", "visibility:hidden");

        if (!IsPostBack)
        {
            ucMainType.QuickTypeCode = ((int)(PhoenixQuickTypeCode.MAINTTYPE)).ToString();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["RecordAndResponseId"] = null;
            ViewState["REFFROM"] = null;      
            ViewState["NEW"] = "false";
            gvWorkOrder.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            if (Request.QueryString["RecordResponseId"] != null && Request.QueryString["RecordResponseId"].ToString() != "")
                ViewState["RecordAndResponseId"] = Request.QueryString["RecordResponseId"].ToString();

            if (Request.QueryString["OBSID"] != null && Request.QueryString["OBSID"].ToString() != "")
                ViewState["OBSID"] = Request.QueryString["OBSID"].ToString();

            if (Request.QueryString["reffrom"] != null && !string.IsNullOrEmpty(Request.QueryString["reffrom"].ToString()))
            {
                ViewState["REFFROM"] = Request.QueryString["reffrom"].ToString();
            }
        }

        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
        if (Filter.CurrentInspectionMenu == null)
        {
            MenuWorkOrderRequestion.AccessRights = this.ViewState;
            MenuWorkOrderRequestion.MenuList = toolbarmain.Show();
        }
        else if (Filter.CurrentInspectionMenu == "directobs")
        {
            if (ViewState["OBSID"] != null && ViewState["OBSID"].ToString() != string.Empty)
            {
                DataSet ds = PhoenixInspectionObservation.EditDirectObservation(new Guid(ViewState["OBSID"].ToString()));

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    if (dr["FLDSTATUS"].ToString() != PhoenixCommonRegisters.GetHardCode(1, 146, "CLD"))
                        MenuWorkOrderRequestion.MenuList = toolbarmain.Show();
                }
            }
        }

        if (Filter.CurrentInspectionScheduleId != null && Filter.CurrentInspectionScheduleId.ToString() != string.Empty && ViewState["RecordAndResponseId"] != null && ViewState["RecordAndResponseId"].ToString() != string.Empty)
        {
            ViewState["scheduleid"] = Filter.CurrentInspectionScheduleId.ToString();  
        }
        imgJob.Attributes.Add("onclick", "return showPickList('spnPickListJob', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJob.aspx', true); ");
        BindInspectionShortCode(); 
    }
    private void BindFields()
    {
        if (ViewState["WORKORDERID"] != null)
        {
            DataSet ds = PhoenixInspectionWorkOrder.EditWorkOrder(new Guid(ViewState["WORKORDERID"].ToString()));
            DataRow dr = ds.Tables[0].Rows[0];
            lblWorkOrderID.Text = dr["FLDWORKORDERID"].ToString();
            txtWorkOrderNumber.Text = dr["FLDWORKORDERNUMBER"].ToString();
            txtTitle.Text = dr["FLDWORKORDERNAME"].ToString();
            txtJobDescription.Text = dr["FLDDETAILS"].ToString();
            txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
            txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
            txtComponentId.Text = dr["FLDCOMPONENTID"].ToString();
            txtCreatedDate.Text = General.GetDateTimeToString(dr["FLDCREATEDDATE"].ToString());
            if (dr["FLDWORKISUNEXPECTED"].ToString().Equals("1"))
                chkUnexpected.Checked = true;
            txtDuration.Text = dr["FLDPLANNINGESTIMETDURATION"].ToString();
            txtPlannedStartDate.Text = General.GetDateTimeToString(dr["FLDPLANNINGDUEDATE"].ToString());
            txtPriority.Text = dr["FLDPLANINGPRIORITY"].ToString();
            ucDiscipline.SelectedDiscipline = dr["FLDPLANNINGDISCIPLINE"].ToString();
            ucWTOApproval.SelectedHard = dr["FLDAPPROVALTYPE"].ToString();
            txtJobId.Text = dr["FLDJOBID"].ToString();
            txtJobName.Text = dr["FLDJOBTITLE"].ToString();
            txtJobCode.Text = dr["FLDJOBCODE"].ToString();
            ucMainType.SelectedQuick = dr["FLDWORKMAINTNANCETYPE"].ToString();
        }
        else
        {
            ViewState["NEW"] = true;
            ResetTextBox();
        }
    }

    private void BindInspectionShortCode()
    {
        if (ViewState["scheduleid"] != null)
        {
            DataSet ds = PhoenixInspectionSchedule.EditInspectionSchedule(new Guid(ViewState["scheduleid"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["SHORTCODE"] = ds.Tables[0].Rows[0]["FLDINSPECTIONSHORTCODE"].ToString();
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();

                imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?vesselid=" + ViewState["VESSELID"].ToString() + "', true);");

                DataSet dsmain = PhoenixInspectionRecordAndResponse.WorkRequestMainType(new Guid(ViewState["scheduleid"].ToString()), General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 148, "INS")));
                if (dsmain.Tables[0].Rows.Count > 0)
                {
                    if (dsmain.Tables[0].Rows[0]["FLDQUICKCODE"] != null && dsmain.Tables[0].Rows[0]["FLDQUICKCODE"].ToString() != string.Empty)
                    {
                        ucMainType.SelectedQuick = (dsmain.Tables[0].Rows[0]["FLDQUICKCODE"] != null ? dsmain.Tables[0].Rows[0]["FLDQUICKCODE"].ToString() : null);
                    }
                }
            }
        }
        else if (ViewState["REFFROM"]!=null)
        {
            DataSet ds = PhoenixInspectionObservation.EditDirectObservation(new Guid(ViewState["OBSID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?vesselid=" + ViewState["VESSELID"] + "&framename=ifMoreInfo', true); ");
            }
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
                int? maintype = General.GetNullableInteger(ucMainType.SelectedQuick);
                if (ViewState["OBSID"] == null)
                {
                    ucError.ErrorMessage = "Please record observation details to generate a work order";
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["NEW"].ToString().ToUpper() != "TRUE")
                {
                    ucError.ErrorMessage = "Sorry,You can not make any more changes here.";
                    ucError.Visible = true;
                    return;
                }
                if (!IsValidRequisition(txtComponentId.Text, txtTitle.Text, txtPlannedStartDate.Text, txtJobId.Text, txtJobDescription.Text))
                {
                    ucError.Visible = true;
                    return;
                }                

                string workorderid = null;

                byte? isDefect = null;

                if (chkIsDefect.Checked.Equals(true))
                    isDefect = byte.Parse("1");

                PhoenixInspectionWorkOrder.InsertWorkOrder(
                    int.Parse(ViewState["VESSELID"].ToString()), null,
                    General.GetNullableString(txtTitle.Text), General.GetNullableGuid(txtComponentId.Text), null,
                    General.GetNullableGuid(txtJobId.Text), null, null, null,
                    General.GetNullableInteger(ucWTOApproval.SelectedHard).HasValue ? 24 : 501, null,
                    General.GetNullableInteger(txtPriority.Text), General.GetNullableInteger(txtDuration.Text),
                    General.GetNullableDateTime(txtPlannedStartDate.Text),
                    General.GetNullableInteger(ucDiscipline.SelectedDiscipline), null, null, null, maintype, null, null,
                    chkUnexpected.Checked.Equals(true)? "1" : "0", General.GetNullableInteger(ucWTOApproval.SelectedHard),
                    isDefect, ref workorderid);
                if (txtJobDescription.Text != "")
                    PhoenixInspectionWorkOrder.UpdateDetailsWorkOrder(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(workorderid), General.GetNullableString(txtJobDescription.Text));
                ViewState["WORKORDERID"] = workorderid;

                PhoenixInspectionRecordAndResponse.NCWorkOrderInsert
                                               (PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                              , new Guid(txtComponentId.Text)
                                              , ViewState["RecordAndResponseId"] == null? null : General.GetNullableGuid(ViewState["RecordAndResponseId"].ToString())
                                              , ViewState["scheduleid"] == null? null : General.GetNullableGuid(ViewState["scheduleid"].ToString())
                                              , new Guid(workorderid)
                                              , 1
                                              , int.Parse(ViewState["VESSELID"].ToString())
                                              , new Guid(ViewState["OBSID"].ToString()));

                ViewState["NEW"] = "false";
                Rebind();
                BindFields();
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                ResetTextBox();
                ViewState["WORKORDERID"] = null;
                ViewState["NEW"] = "true";
            }
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
        txtCreatedDate.Text = General.GetDateTimeToString(DateTime.Now.ToString());
        chkUnexpected.Checked = true;
        txtDuration.Text = "";
        txtPlannedStartDate.Text = "";
        txtPriority.Text = "1";
        ucDiscipline.SelectedDiscipline = "";
        txtJobDescription.Text = "";
        txtJobId.Text = "";
        txtJobName.Text = "";
        txtJobCode.Text = "";
        ucWTOApproval.SelectedHard = "";
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

        return (!ucError.IsError);
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? vesselid = null;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if ((Filter.CurrentVesselConfiguration != null) && (!Filter.CurrentVesselConfiguration.ToString().Equals("0")))
                vesselid = int.Parse(Filter.CurrentVesselConfiguration.ToString());

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            DataSet ds = PhoenixInspectionRecordAndResponse.InspectionWorkRequestSearch(null,
                             null,null,
                             ViewState["OBSID"] != null ? General.GetNullableGuid(ViewState["OBSID"].ToString()) : null,
                             vesselid,
                             sortexpression, sortdirection,
                             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                             gvWorkOrder.PageSize,
                             ref iRowCount,
                             ref iTotalPageCount);

            gvWorkOrder.DataSource = ds;
            gvWorkOrder.VirtualItemCount = iRowCount;

            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["OPERATIONMODE"] = "EDIT";
                gvWorkOrder.SelectedIndexes.Clear();

                if (ViewState["WORKORDERID"] == null && ViewState["NEW"] != null && ViewState["NEW"].ToString().ToUpper() != "TRUE")
                {
                    ViewState["WORKORDERID"] = ds.Tables[0].Rows[0]["FLDWORKORDERID"].ToString();
                    BindFields();
                }                                   
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ViewState["NEW"] = "true";
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
    protected void gvWorkOrder_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            RadLabel lblWorkOrderId = ((RadLabel)e.Item.FindControl("lblWorkOrderId"));
            if (lblWorkOrderId != null)
                ViewState["WORKORDERID"] = lblWorkOrderId.Text;
            else
                ViewState["WORKORDERID"] = null;
            BindFields();
        }
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
}
