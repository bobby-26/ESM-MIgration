using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class InspectionUnsafeActsConditionsAdd : PhoenixBasePage
{
    protected string Code = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ddlVessel.Enabled = true;
            ViewState["COMPANYID"] = "";

            if(PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
            {
                txtName.CssClass = "input_mandatory";
                txtRank.CssClass = "input_mandatory";
                lblNote.Text = "Note: All crew members should participate in reporting of near-misses/ unsafe acts-conditions. This reporting should not only be limited to senior officers or only one department.";
            }

            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
                ddlVessel.Company = nvc.Get("QMS");
                ddlVessel.bind();
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ddlVessel.bind();
                ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ddlVessel.Enabled = false;
            }
            else
            {
                ddlVessel.SelectedVessel = "";
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuInspectionIncident.AccessRights = this.ViewState;
            MenuInspectionIncident.MenuList = toolbar.Show();
            BindSubCategory();
        }
    }

    protected void ucCategory_TextChanged(object sender, EventArgs e)
    {
        BindSubCategory();
    }

    protected int Getinstallcode()
    {
        DataSet ds = PhoenixInspectionIncident.GetInstallCode();
        int installcode = Convert.ToInt32(ds.Tables[0].Rows[0]["FLDINSTALLCODE"]);
        return installcode;
    }

    private void BindSubCategory()
    {
        DataTable dt = PhoenixInspectionUnsafeActsConditions.OpenReportSubcategoryList(General.GetNullableInteger(ucCategory.SelectedHard));
        ddlSubcategory.DataSource = dt;
        ddlSubcategory.DataTextField = "FLDIMMEDIATECAUSE";
        ddlSubcategory.DataValueField = "FLDIMMEDIATECAUSEID";
        ddlSubcategory.DataBind();
        ddlSubcategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void InspectionIncident_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidEntry())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionUnsafeActsConditions.DirectIncidentNearmissInsert(null, txtInvestigationAndEvidence.Text.Trim(), 1, int.Parse(ddlVessel.SelectedVessel)
                    , General.GetNullableString(txtLocation.Text), DateTime.Parse(ucDate.Text + " " + rtpunsafetime.SelectedTime.Value), int.Parse(ucCategory.SelectedHard)
                    , new Guid(ddlSubcategory.SelectedValue), General.GetNullableString(txtRank.Text), General.GetNullableString(txtName.Text));
                ucStatus.Text = "Information is recorded.";
                txtInvestigationAndEvidence.Text = "";

                String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (!IsValidEntry())
            {
                ucError.Visible = true;
                return;
            }
            PhoenixInspectionUnsafeActsConditions.DirectIncidentNearmissInsert(null, txtInvestigationAndEvidence.Text.Trim(), int.Parse(Request.QueryString["category"].ToString()), int.Parse(ddlVessel.SelectedVessel)
                , General.GetNullableString(txtLocation.Text), DateTime.Parse(ucDate.Text + " " + rtpunsafetime.SelectedTime.Value), int.Parse(ucCategory.SelectedHard)
                , new Guid(ddlSubcategory.SelectedValue), General.GetNullableString(txtRank.Text), General.GetNullableString(txtName.Text));
            ucStatus.Text = "Information is recorded.";
            txtInvestigationAndEvidence.Text = "";

            Response.Redirect("~/Default.aspx");
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Report Incident/Near Miss";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidEntry()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (txtLocation.Text.Trim().Length == 0)
            ucError.ErrorMessage = "Location is required.";

        if (General.GetNullableDateTime(ucDate.Text + " " + rtpunsafetime.SelectedTime) == null)
            ucError.ErrorMessage = "Date is required.";

        if (rtpunsafetime.SelectedTime == null)
            ucError.ErrorMessage = "Time is required.";
        else
        {
            if (General.GetNullableDateTime(ucDate.Text + " " + rtpunsafetime.SelectedTime) == null)
                ucError.ErrorMessage = "Time is not a valid time.";
        }

        if (General.GetNullableDateTime(ucDate.Text + " " + rtpunsafetime.SelectedTime) > System.DateTime.Now)
            ucError.ErrorMessage = "Date should not be the future date.";

        if (General.GetNullableInteger(ucCategory.SelectedHard) == null)
            ucError.ErrorMessage = "Category is required.";
    
        if (ddlSubcategory.SelectedIndex == 0)
            ucError.ErrorMessage = "Sub-category is required.";

        if (txtInvestigationAndEvidence.Text.Trim().Length == 0)
            ucError.ErrorMessage = "Comprehensive Description is required.";

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("PHOENIX"))
        {
            if (General.GetNullableString(txtRank.Text.Trim()) == null)
                ucError.ErrorMessage = "Rank is required.";

            if (General.GetNullableString(txtName.Text.Trim()) == null)
                ucError.ErrorMessage = "Name is required.";
        }

        return (!ucError.IsError);
    }    
}
