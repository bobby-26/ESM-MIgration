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
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionReviewPlannerManual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                VesselConfiguration();
                ucVessel.Enabled = true;

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.bind();
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                BindCompany();
                Bind_UserControls(sender, new EventArgs());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Bind_UserControls(object sender, EventArgs e)
    {
        string type = PhoenixCommonRegisters.GetHardCode(1, 148, "AUD");

        string externalaudit = PhoenixCommonRegisters.GetHardCode(1, 144, "EXT");

        ddlAudit.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                        , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                        , null
                                        , 1
                                        , 0
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlAudit.DataTextField = "FLDSHORTCODE";
        ddlAudit.DataValueField = "FLDINSPECTIONID";
        ddlAudit.DataBind();
        ddlAudit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

        if (ucAuditCategory.SelectedHard != externalaudit)
        {
            ddlCompany.SelectedValue = "Dummy";
            ddlCompany.Enabled = false;
        }
        else if (ucAuditCategory.SelectedHard == externalaudit)
        {
            BindCompany();
            ddlCompany.Enabled = true;
        }
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidSchedule(ucVessel.SelectedVessel, ddlAudit.SelectedValue, txtDueDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionAuditSchedule.InsertReviewPlanner(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ucVessel.SelectedVessel), General.GetNullableGuid(ddlAudit.SelectedValue), null, null,
                    General.GetNullableDateTime(txtLastDoneDate.Text), General.GetNullableDateTime(txtDueDate.Text),
                    null, null, null, null, null, null, null, null, 1, General.GetNullableGuid(ddlCompany.SelectedValue));

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

    private bool IsValidSchedule(string vesselid, string inspectionid, string duedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(vesselid) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableString(inspectionid) == null)
            ucError.ErrorMessage = "Audit/Inspection is required.";

        if (General.GetNullableDateTime(duedate) == null)
            ucError.ErrorMessage = "Due Date is required.";

        return (!ucError.IsError);
    }

    protected void txtLastDoneDate_Changed(object sender, EventArgs e)
    {
        int frequency = 0;
        if (General.GetNullableGuid(ddlAudit.SelectedValue) != null)
        {
            DataSet ds = PhoenixInspection.EditInspection(new Guid(ddlAudit.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
                frequency = int.Parse(ds.Tables[0].Rows[0]["FLDAUDITFREQUENCYINMONTHS"].ToString());
        }

        if (txtLastDoneDate != null && General.GetNullableDateTime(txtLastDoneDate.Text) != null)
        {
            DateTime dtLastDoneDate = Convert.ToDateTime(txtLastDoneDate.Text);
            DateTime dtDueDate = dtLastDoneDate.AddMonths(frequency);
            txtDueDate.Text = dtDueDate.ToString();
        }
    }

    protected void BindCompany()
    {
        ddlCompany.DataSource = PhoenixInspectionInspectingCompany.ListAuditInspectionCompany(null, General.GetNullableGuid(ddlAudit.SelectedValue));
        ddlCompany.DataTextField = "FLDCOMPANYNAME";
        ddlCompany.DataValueField = "FLDCOMPANYID";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ddlAudit_SelectedIndexChanged(object sender, EventArgs e)
    {
        string type = "";
        if (General.GetNullableGuid(ddlAudit.SelectedValue) != null)
        {
            DataSet ds = PhoenixInspection.EditInspection(new Guid(ddlAudit.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
                type = ds.Tables[0].Rows[0]["FLDINSPECTIONCATEGORYID"].ToString();
        }
        if (type == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
        {
            ddlCompany.Enabled = true;
        }
        else
        {
            ddlCompany.Enabled = false;
        }
        BindCompany();

    }
}
