using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Text;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionReviewOfficePlannerManual : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbar.Show();

            if (!IsPostBack)
            {                
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    //ucCompany.Company = nvc.Get("QMS");
                    //ucCompany.bind();
                }

                Bind_UserControls(sender, new EventArgs());
                Bind_UcCompany();
                BindCompany();
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
        ddlAudit.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                        , General.GetNullableInteger(ucAuditCategory.SelectedHard)
                                        , null
                                        , 0
                                        , 0
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlAudit.DataTextField = "FLDSHORTCODE";
        ddlAudit.DataValueField = "FLDINSPECTIONID";
        ddlAudit.DataBind();
        ddlAudit.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidSchedule(ucCompany.SelectedValue, ddlAudit.SelectedValue, txtDueDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionAuditOfficeSchedule.InsertReviewOfficePlanner(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(ucCompany.SelectedValue), General.GetNullableGuid(ddlAudit.SelectedValue), null, null,
                    General.GetNullableDateTime(txtLastDoneDate.Text), General.GetNullableDateTime(txtDueDate.Text),
                    null,  null, null, null, null, null, 1, General.GetNullableGuid(ddlCompany.SelectedValue));

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

    private bool IsValidSchedule(string selcompanyid, string inspectionid, string duedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(selcompanyid) == null)
            ucError.ErrorMessage = "Company is required.";

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
        ddlCompany.DataSource = PhoenixInspectionInspectingCompany.ListAuditInspectionCompany();
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
            
            Bind_UcCompany();
        }
        if (type == PhoenixCommonRegisters.GetHardCode(1, 144, "EXT"))
        {
            ddlCompany.Enabled = true;
        }
        else
        {
            ddlCompany.Enabled = false;
            ddlCompany.SelectedIndex = 0;
        }
    }
    protected void Bind_UcCompany()
    {
         ucCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
         if (General.GetNullableGuid(ddlAudit.SelectedValue) != null)
         {
             ucCompany.Enabled = true;
             ucCompany.DataSource = PhoenixInspectionAuditOfficeSchedule.ListAuditOfficeManualInspectionCompany(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
                                                                                  General.GetNullableGuid(ddlAudit.SelectedValue));
             ucCompany.DataTextField = "FLDCOMPANYNAME";
             ucCompany.DataValueField = "FLDCOMPANYID";
             ucCompany.DataBind();
             ucCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
         }
    }

}
