using System;
using System.Web.UI;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;


public partial class InspectionOfficeAuditScheduleUpdate : PhoenixBasePage
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
            
            if (Request.QueryString["STATUS"] != null && Request.QueryString["STATUS"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 146, "ASG"))
            {
                txtplandate.Enabled = true;
             
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
            
                ucInspector.Enabled = false;
                txtExternalAuditor.Enabled = false;
                txtOrganizationStatus.Enabled = false;


            }

            BindInfo();
        }
    }
    public void BindInfo()
    {
        DataTable dt = PhoenixInspectionAuditOfficeSchedule.OfficeScheduleEdit(
               PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , General.GetNullableGuid(Request.QueryString["INSID"].ToString())
               , int.Parse(Request.QueryString["CID"].ToString())
               , General.GetNullableGuid(Request.QueryString["PSCHEID"].ToString()));

        if (dt.Rows.Count > 0)
        {
            txtvesselname.Text = dt.Rows[0]["FLDCOMPANYNAME"].ToString();
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

         
            ucInspector.SelectedValue = dt.Rows[0]["FLDINTERNALINSPECTORID"].ToString();
            ucInspector.Text = dt.Rows[0]["FLDNAMEOFINSPECTOR"].ToString();
            txtOrganizationStatus.Text = dt.Rows[0]["FLDEXTERNALINSPECTORORGANISATION"].ToString();
            txtExternalAuditor.Text = dt.Rows[0]["FLDEXTERNALINSPECTORNAME"].ToString();

        }
    }

    protected void MenuScheduleFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            string category = ViewState["CATEGORY"].ToString();
            PhoenixInspectionAuditOfficeSchedule.InsertReviewOfficePlanner(
                          PhoenixSecurityContext.CurrentSecurityContext.UserCode
                          , int.Parse(Request.QueryString["CID"].ToString())
                          , General.GetNullableGuid(Request.QueryString["INSID"].ToString())
                          , General.GetNullableGuid(ViewState["SCHEDULEID"].ToString())
                          , General.GetNullableGuid(Request.QueryString["PSCHEID"].ToString())
                          , General.GetNullableDateTime(txtlastdone.Text)
                          , General.GetNullableDateTime(txtduedate.Text)
                          , General.GetNullableDateTime(txtplandate.Text)
                          , (category == PhoenixCommonRegisters.GetHardCode(1, 144, "INT")) ? General.GetNullableInteger(ucInspector.SelectedValue) : null
                          , General.GetNullableString(txtExternalAuditor.Text)
                          , null
                          , null
                          , (category == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")) ? General.GetNullableInteger(ucInspector.SelectedValue) : null
                          , General.GetNullableInteger(ViewState["ISMANUALINSPECTION"].ToString())
                          , General.GetNullableGuid(ViewState["PLANINSPECTINGCOMPANYID"].ToString())
                      );
        }

        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo','','');");
        RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }
    protected void txtlastdone_TextChangedEvent(object sender, EventArgs e)
    {
        int frequency = 0;
        DataSet ds = PhoenixInspection.EditInspection(new Guid(Request.QueryString["INSID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
            frequency = int.Parse(ds.Tables[0].Rows[0]["FLDFREQUENCYINMONTHS"].ToString());

        if (txtlastdone != null && General.GetNullableDateTime(txtlastdone.Text) != null && frequency != 0)
        {
            DateTime dtLastDoneDate = Convert.ToDateTime(txtlastdone.Text);
            DateTime dtDueDate = dtLastDoneDate.AddMonths(frequency);
            txtduedate.Text = dtDueDate.ToString();
        }
    }
}