using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Text;

public partial class InspectionScheduleByCompanyManual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE");
                MenuGeneral.AccessRights = this.ViewState;
                MenuGeneral.MenuList = toolbar.Show();
                VesselConfiguration();
                ucVessel.Enabled = true;
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                BindCompanies();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void BindCompanies()
    {
        DataSet ds = PhoenixInspectionInspectingCompany.ListInspectionCompany();
        ddlCompany.DataSource = ds;
        ddlCompany.DataTextField = "FLDCOMPANYINSPECTION";
        ddlCompany.DataValueField = "FLDCOMPANYID";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new ListItem("--Select--","Dummy"));
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidSchedule(ucVessel.SelectedVessel, ddlCompany.SelectedValue, txtDueDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                InsertScheduleDetails(int.Parse(ucVessel.SelectedVessel),
                                        txtLastDoneDate.Text,
                                        txtDueDate.Text,
                                        ddlCompany.SelectedValue);

                ucStatus.Text = "Schedule created successfully.";

                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidSchedule(string vesselid, string companyid, string duedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(vesselid) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableString(companyid) == null)
            ucError.ErrorMessage = "Company is required.";

        if (General.GetNullableDateTime(duedate) == null)
            ucError.ErrorMessage = "Due Date is required.";

        return (!ucError.IsError);
    }

    private void InsertScheduleDetails(int vesselid, string lastdonedate, string duedate, string company)
    {
        try
        {
            PhoenixInspectionSchedule.InsertInspectionScheduleByCompany(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , vesselid
                , General.GetNullableDateTime(lastdonedate)
                , General.GetNullableDateTime(duedate)
                , General.GetNullableGuid(company)
                , null
                , null
                , null
                , null
                , null
                , null
                , 1
                );
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void txtLastDoneDate_Changed(object sender, EventArgs e)
    {
        int frequency = 0;
        if (General.GetNullableGuid(lblInspectionId.Text) != null)
        {
            DataSet ds = PhoenixInspection.EditInspection(new Guid(lblInspectionId.Text));
            if (ds.Tables[0].Rows.Count > 0)
                frequency = int.Parse(ds.Tables[0].Rows[0]["FLDFREQUENCYINMONTHS"].ToString());
        }

        if (txtLastDoneDate != null && General.GetNullableDateTime(txtLastDoneDate.Text) != null)
        {
            DateTime dtLastDoneDate = Convert.ToDateTime(txtLastDoneDate.Text);
            DateTime dtDueDate = dtLastDoneDate.AddMonths(frequency);
            txtDueDate.Text = dtDueDate.ToString();
        }
    }

    protected void ddlCompany_Changed(object sender, EventArgs e)
    {
        lblInspectionId.Text = "";
        if (General.GetNullableGuid(ddlCompany.SelectedValue) != null)
        {
            DataTable dt = PhoenixInspectionInspectingCompany.EditInspectionCompany(General.GetNullableGuid(ddlCompany.SelectedValue));
            if(dt.Rows.Count > 0)
                lblInspectionId.Text = dt.Rows[0]["FLDVETTINGINSPECTIONID"].ToString();
        }
    }
}
