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
public partial class InspectionIncidentAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuInspectionIncident.AccessRights = this.ViewState;
            if (Filter.CurrentSelectedIncidentMenu == null)
                MenuInspectionIncident.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ucConfirmComplete.Visible = false;
                txtReportedByShipId.Attributes.Add("style", "visibility:hidden");

                if (Request.QueryString["DefectId"] != null && Request.QueryString["DefectId"].ToString() != "")
                    ViewState["DefectId"] = Request.QueryString["DefectId"].ToString();
                else
                    ViewState["DefectId"] = "";

                Filter.CurrentIncidentID = null;
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }
                Reset();
                if (Request.QueryString["module"] == "DMR")
                    txtDateOfReport.Text = Request.QueryString["date"];
                BindVesselEmployee();
                BindCategories();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindVesselEmployee()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? vesselid = 0;
        vesselid = General.GetNullableInteger(ucVessel.SelectedVessel);
        DataSet ds = PhoenixCommonInspection.SearchVesselEmployeeByDate(vesselid
            , General.GetNullableDateTime(txtDateOfIncident.Text)
            , null
            , null
            , null
            , 1
            , 100
            , ref iRowCount
            , ref iTotalPageCount);

        ddlPersonIncharge.DataSource = ds;
        ddlPersonIncharge.DataTextField = "FLDNAMEANDRANK";
        ddlPersonIncharge.DataValueField = "FLDEMPLOYEEID";
        ddlPersonIncharge.DataBind();
        ddlPersonIncharge.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void InspectionIncident_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidInspectionIncident())
                {
                    Guid? newinsertedid = null;

                    int? incidentyn = General.GetNullableInteger(rblIncidentNearmiss.SelectedValue);

                    PhoenixInspectionIncident.InsertInspectionIncident(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableInteger(rblIncidentNearmiss.SelectedValue),
                        General.GetNullableString(txtTitle.Text),
                        General.GetNullableDateTime(txtDateOfIncident.Text + " " + txtTimeOfIncident.SelectedTime.Value), 
                        General.GetNullableInteger(ucQuickVesselActivity.SelectedQuick),
                        General.GetNullableGuid(ucCategory.SelectedCategory),
                        General.GetNullableGuid(ucSubcategory.SelectedSubCategory),
                        General.GetNullableString(txtCurrent.Text),
                        General.GetNullableString(txtVisibility.Text),
                        General.GetNullableString(ucLatitude.Text),
                        General.GetNullableString(ucLongitude.Text),
                        General.GetNullableInteger(ucPort.SelectedSeaport),
                        General.GetNullableString(txtSeaCondition.Text),
                        General.GetNullableInteger(ucWindCondionScale.SelectedQuick),
                        General.GetNullableString(txtWindDirection.Text),
                        General.GetNullableInteger(ucSwellLength.SelectedQuick),
                        General.GetNullableInteger(ucSwellHeight.SelectedQuick),
                        General.GetNullableString(txtSwellDirection.Text),
                        General.GetNullableInteger(ucOnboardLocation.SelectedQuick),
                        General.GetNullableInteger(ucActivity.SelectedHard),
                        General.GetNullableString(txtOtherActivity.Text),
                        General.GetNullableInteger(ddlPersonIncharge.SelectedIndex == 0 ? null : ddlPersonIncharge.SelectedValue),
                        General.GetNullableString(txtDescription.Text),
                        General.GetNullableString(txtRemarks.Text),
                        General.GetNullableString(txtComprehensiveDescription.Text),
                        int.Parse(ucVessel.SelectedVessel),
                        ref newinsertedid,
                        General.GetNullableInteger(ucCompany.SelectedCompany), General.GetNullableDateTime(txtDateOfReport.Text)
                        );

                    if (General.GetNullableGuid(ViewState["DefectId"].ToString()) != null && newinsertedid != null && General.GetNullableInteger(ucVessel.SelectedVessel) != null)
                    {
                        PhoenixInspectionIncident.MapIncidenttoDefectList(new Guid(ViewState["DefectId"].ToString()), new Guid(newinsertedid.ToString()), int.Parse(ucVessel.SelectedVessel));

                        string script = "parent.CloseModelWindow();";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    }
                        

                    ucStatus.Text = "Incident details added.";
                    ucStatus.Visible = true;

                    Filter.CurrentIncidentID = newinsertedid.ToString(); if (Request.QueryString["module"] == "DMR")
                    {
                        Response.Redirect("../Inspection/InspectionIncidentInjuryList.aspx", false);
                    }
                    else
                    {
                        String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                    }
                }

                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Activity_Changed(object sender, EventArgs e)
    {
        try
        {
            UserControlHard ucActivity = (UserControlHard)sender;

            if (ucActivity.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 170, "OTH"))
            {
                txtOtherActivity.CssClass = "input_mandatory";
                txtOtherActivity.ReadOnly = false;
            }
            else
            {
                txtOtherActivity.CssClass = "readonlytextbox";
                txtOtherActivity.ReadOnly = true;
                txtOtherActivity.Text = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidInspectionIncident()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
        {
            if (ucVessel.SelectedVesselName.ToUpper().Equals("-- OFFICE --"))
            {
                if (General.GetNullableInteger(ucCompany.SelectedCompany) == null)
                    ucError.ErrorMessage = "'Company' is required";
            }
            if (!ucVessel.SelectedVesselName.ToUpper().Equals("-- OFFICE --"))
            {
                if (General.GetNullableInteger(ucOnboardLocation.SelectedQuick) == null)
                    ucError.ErrorMessage = "'Onboard Location' is required.";

                if (General.GetNullableInteger(ucActivity.SelectedHard) == null)
                    ucError.ErrorMessage = "'Activity relevent to the Event' is required.";
            }
        }
        else
        {
            ucError.ErrorMessage = "'Vessel' is required.";
        }

        if (txtTitle.Text.Trim().Equals(""))
            ucError.ErrorMessage = "'Title' is required.";

        if (General.GetNullableDateTime(txtDateOfIncident.Text) == null)
            ucError.ErrorMessage = "'Date of Incident' is required.";

        if (txtTimeOfIncident.SelectedTime == null)
            ucError.ErrorMessage = "'Time of Incident' is required.";
        else
        {
            if (General.GetNullableDateTime(txtDateOfIncident.Text + " " + txtTimeOfIncident.SelectedTime) == null)
                ucError.ErrorMessage = "'Time of Incident' is not a valid time.";
        }

        if (General.GetNullableDateTime(txtDateOfIncident.Text) > System.DateTime.Now)
            ucError.ErrorMessage = "'Date of Incident' should not be the future date.";

        if (General.GetNullableGuid(ucCategory.SelectedCategory) == null)
            ucError.ErrorMessage = "'Incident Category' is required.";

        if (General.GetNullableGuid(ucSubcategory.SelectedSubCategory) == null)
            ucError.ErrorMessage = "'Incident Subcategory' is required.";

        if (!string.IsNullOrEmpty(ucLatitude.TextDegree) && (Convert.ToInt32(ucLatitude.TextDegree) < 0 || Convert.ToInt32(ucLatitude.TextDegree) > 90))
            ucError.ErrorMessage = "Latitude Degree should not exceed 90&deg";

        if (!string.IsNullOrEmpty(ucLatitude.TextMinute) && (Convert.ToInt32(ucLatitude.TextMinute) < 0 || Convert.ToInt32(ucLatitude.TextMinute) > 59))
            ucError.ErrorMessage = "Latitude Minutes should not equal to or exceed 60 minutes";

        if (!string.IsNullOrEmpty(ucLatitude.TextSecond) && (Convert.ToInt32(ucLatitude.TextSecond) < 0 || Convert.ToInt32(ucLatitude.TextSecond) > 59))
            ucError.ErrorMessage = "Latitude Seconds should not equal to or exceed 60 seconds";

        if (!string.IsNullOrEmpty(ucLongitude.TextDegree) && (Convert.ToInt32(ucLongitude.TextDegree) < 0 || Convert.ToInt32(ucLongitude.TextDegree) > 180))
            ucError.ErrorMessage = "Longitude Degree should not exceed 180&deg";

        if (!string.IsNullOrEmpty(ucLongitude.TextMinute) && (Convert.ToInt32(ucLongitude.TextMinute) < 0 || Convert.ToInt32(ucLongitude.TextMinute) > 59))
            ucError.ErrorMessage = "Longitude Minutes should not equal to or exceed 60 minutes";

        if (!string.IsNullOrEmpty(ucLongitude.TextSecond) && (Convert.ToInt32(ucLongitude.TextSecond) < 0 || Convert.ToInt32(ucLongitude.TextSecond) > 59))
            ucError.ErrorMessage = "Longitude Seconds should not equal to or exceed 60 seconds";

        if (txtOtherActivity.CssClass.Equals("input_mandatory") && txtOtherActivity.Text.Trim().Equals(""))
            ucError.ErrorMessage = "'Other Activity relevent' is required.";

        if (General.GetNullableString(txtDescription.Text.Trim()) == null)
            ucError.ErrorMessage = "'Brief Description' is required.";

        return (!ucError.IsError);
    }

    private void Reset()
    {
        ucVessel.Enabled = true;
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            ucVessel.bind();
            ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            ucVessel.Enabled = false;
        }
        else
        {
            ucVessel.SelectedVessel = "";
        }
        ucCompany.SelectedCompany = "";
        txtRefNo.Text = "";
        txtTitle.Text = "";
        txtDateOfIncident.Text = "";
        txtTimeOfIncident.SelectedTime = null;
        txtReportedbyDesignation.Text = "";
        txtReportedByShipId.Text = "";
        txtReportedByShipName.Text = "";
        txtDescription.Text = "";
        txtSeaCondition.Text = "";
        ucOnboardLocation.SelectedQuick = "";
        ucWindCondionScale.SelectedQuick = "";
        txtWindDirection.Text = "";
        ucSwellLength.SelectedQuick = "";
        ucSwellHeight.SelectedQuick = "";
        txtSwellDirection.Text = "";
        txtDateOfReport.Text = "";
        ucActivity.SelectedHard = "";
        txtOtherActivity.Text = "";
        txtOtherActivity.CssClass = "readonlytextbox";
        txtOtherActivity.ReadOnly = true;
        ucPort.SelectedSeaport = "";
        ucLatitude.Clear();
        ucLongitude.Clear();
        ucLatitude.TextSecond = "00";
        ucLongitude.TextSecond = "00";
        ucQuickVesselActivity.SelectedQuick = "";
        txtCurrent.Text = "";
        txtCurrent.CssClass = "input";
        txtVisibility.Text = "";
        txtVisibility.CssClass = "input";
        rblIncidentNearmiss.SelectedValue = "1";
    }

    protected void ucConsequenceCategory_Changed(object sender, EventArgs e)
    {

    }

    protected void btnComplete_Click(object sender, EventArgs e)
    {

    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        BindVesselEmployee();
        if (ucVessel.SelectedVesselName.ToUpper().Equals("-- OFFICE --"))
        {
            ucCompany.Enabled = true;
            ucCompany.CssClass = "input_mandatory";
            ucOnboardLocation.CssClass = "input";
            ucActivity.CssClass = "input";

        }
        else
        {
            ucCompany.Enabled = false;
            ucCompany.CssClass = "input";
            ucOnboardLocation.CssClass = "dropdown_mandatory";
            ucActivity.CssClass = "dropdown_mandatory";
        }
    }

    protected void ucCompany_changed(object sender, EventArgs e)
    {

    }

    protected void rblIncidentNearmiss_Changed(object sender, EventArgs e)
    {
        BindCategories();

        if (rblIncidentNearmiss.SelectedValue == "1")
        {
            ucCategory.CssClass = "input_mandatory";
            ucSubcategory.CssClass = "input_mandatory";
        }
        if (rblIncidentNearmiss.SelectedValue == "2" || rblIncidentNearmiss.SelectedValue == "3")
        {
        }
    }

    protected void ucCategory_Changed(object sender, EventArgs e)
    {
        ucSubcategory.CategoryId = ucCategory.SelectedCategory;
        ucSubcategory.SelectedSubCategory = "";
        ucSubcategory.SubCategoryList = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(ucCategory.SelectedCategory));
        ucSubcategory.DataBind();
    }

    protected void BindCategories()
    {
        ucCategory.TypeId = rblIncidentNearmiss.SelectedValue;
        ucCategory.CategoryList = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(General.GetNullableInteger(rblIncidentNearmiss.SelectedValue));
        ucCategory.DataBind();

        ucSubcategory.CategoryId = ucCategory.SelectedCategory;
        ucSubcategory.SubCategoryList = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(ucCategory.SelectedCategory));
        ucSubcategory.DataBind();
    }

    protected void txtDateOfIncident_TextChanged(object sender, EventArgs e)
    {
        try
        {
            BindVesselEmployee();           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
