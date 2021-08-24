using System;
using System.Web.UI;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;

public partial class InspectionAuditScheduleUpdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuScheduleFilter.AccessRights = this.ViewState;
        MenuScheduleFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            //DataSet ds = PhoenixInspectionAuditSchedule.AuditInternalInspectorSearch(null);
            //ddlAttendingSupt.DataSource = ds.Tables[0];
            //ddlAttendingSupt.DataTextField = "FLDDESIGNATIONNAME";
            //ddlAttendingSupt.DataValueField = "FLDUSERCODE";
            //ddlAttendingSupt.DataBind();
            //ddlAttendingSupt.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

            if (Request.QueryString["STATUS"] != null && Request.QueryString["STATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 146, "ASG"))
            {


                txtplandate.Enabled = true;
                ucFromPort.Enabled = true;
                ucToPort.Enabled = true;


                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    ucInspector.Enabled = false;
                else
                    ucInspector.Enabled = true;

                txtExternalAuditor.Enabled = true;
                txtOrganizationStatus.Enabled = true;


            }
            else
            {
                txtplandate.Enabled = false;
                ucFromPort.Enabled = false;
                ucToPort.Enabled = false;
                ucInspector.Enabled = false;
                txtExternalAuditor.Enabled = false;
                txtOrganizationStatus.Enabled = false;


            }

            BindInfo();
        }
    }
    public void BindInfo()
    {
        DataTable dt = PhoenixInspectionAuditSchedule.ReviewScheduleEdit(
               PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , General.GetNullableGuid(Request.QueryString["INSID"].ToString())
               , int.Parse(Request.QueryString["VID"].ToString())
               , General.GetNullableGuid(Request.QueryString["PSCHEID"].ToString()));

        if (dt.Rows.Count > 0)
        {
            txtvesselname.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
            txtinspectionname.Text = dt.Rows[0]["FLDSHORTCODE"].ToString();
            txtMC.Text = dt.Rows[0]["FLDISMANUAL"].ToString();
            txtcategoryname.Text = dt.Rows[0]["FLDINSPECTIONCATEGORY"].ToString();
            txtlastdone.Text = dt.Rows[0]["FLDLASTDONEDATE"].ToString();
            txtduedate.Text = dt.Rows[0]["FLDDUEDATE"].ToString();
            txtplandate.Text = dt.Rows[0]["FLDPLANNEDDATE"].ToString();
            ViewState["SCHEDULEID"] = dt.Rows[0]["FLDSCHEDULEID"].ToString();
            ViewState["CATEGORY"] = dt.Rows[0]["FLDINSPECTIONCATEGORYID"].ToString();
            ViewState["ISMANUALINSPECTION"] = dt.Rows[0]["FLDISMANUALINSPECTION"].ToString();
            ViewState["PLANINSPECTINGCOMPANYID"] = dt.Rows[0]["FLDPLANINSPECTINGCOMPANYID"].ToString();

            ucFromPort.SelectedValue = dt.Rows[0]["FLDFROMPORTID"].ToString();
            ucFromPort.Text = dt.Rows[0]["FLDFROMPORT"].ToString();
            ucToPort.SelectedValue = dt.Rows[0]["FLDTOPORTID"].ToString();
            ucToPort.Text = dt.Rows[0]["FLDTOPORT"].ToString();
            ucInspector.SelectedValue = dt.Rows[0]["FLDINTERNALINSPECTORID"].ToString();
            ucInspector.Text = dt.Rows[0]["FLDNAMEOFINSPECTOR"].ToString();
            txtOrganizationStatus.Text = dt.Rows[0]["FLDEXTERNALINSPECTORORGANISATION"].ToString();
            txtExternalAuditor.Text = dt.Rows[0]["FLDEXTERNALINSPECTORNAME"].ToString();

        }
    }

    protected void MenuScheduleFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string category = ViewState["CATEGORY"].ToString();
                PhoenixInspectionAuditSchedule.InsertReviewPlanner(
                               PhoenixSecurityContext.CurrentSecurityContext.UserCode
                               , int.Parse(Request.QueryString["VID"].ToString())
                               , General.GetNullableGuid(Request.QueryString["INSID"].ToString())
                               , General.GetNullableGuid(ViewState["SCHEDULEID"].ToString())
                               , General.GetNullableGuid(Request.QueryString["PSCHEID"].ToString())
                               , General.GetNullableDateTime(txtlastdone.Text)
                               , General.GetNullableDateTime(txtduedate.Text)
                               , General.GetNullableDateTime(txtplandate.Text)
                               , General.GetNullableInteger(ucFromPort.SelectedValue)
                               , General.GetNullableInteger(ucToPort.SelectedValue)
                               , (category == PhoenixCommonRegisters.GetHardCode(1, 144, "INT")) ? General.GetNullableInteger(ucInspector.SelectedValue) : null
                               , General.GetNullableString(txtExternalAuditor.Text)
                               , null
                               , null
                               , (category == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")) ? General.GetNullableInteger(ucInspector.SelectedValue) : null
                               , General.GetNullableInteger(ViewState["ISMANUALINSPECTION"].ToString())
                               , General.GetNullableGuid(ViewState["PLANINSPECTINGCOMPANYID"].ToString())
                           );

                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo','','');");
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}