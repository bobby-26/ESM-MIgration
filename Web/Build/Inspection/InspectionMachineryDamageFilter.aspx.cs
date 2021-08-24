using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class InspectionMachineryDamageFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (General.IsTelerik() && Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper().Equals("MENU"))
        {
            Response.Redirect("../Inspection/InspectionMachineryDamageList.aspx", false);
        }

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuIncidentFilter.AccessRights = this.ViewState;
        MenuIncidentFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            txtComponentId.Attributes.Add("style", "visibility:hidden;");
            
            ucVessel.Enabled = true;
            ViewState["COMPANYID"] = "";

            Filter.CurrentVesselConfiguration = PhoenixSecurityContext.CurrentSecurityContext.InstallCode.ToString();           

            SetVessel();
            BindControls();
            BindNearMiss();
            DateTime now = DateTime.Now;
            ucIncidentToDate.Text = DateTime.Now.ToShortDateString();
            ucIncidentFromDate.Text = now.Date.AddMonths(-6).ToShortDateString();           
        }
    }

    protected void BindNearMiss()
    {
        try
        {
            rblNearmiss.DataSource = PhoenixInspectionIncidentNearMissCategory.MachineryDamageNearMissCategory();
            rblNearmiss.DataBindings.DataTextField = "FLDNAME";
            rblNearmiss.DataBindings.DataValueField = "FLDCATEGORYID";
            rblNearmiss.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuIncidentFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();

            criteria.Add("txtRefno", txtRefNo.Text.Trim());
            criteria.Add("txtTitle", txtTitle.Text.Trim());
            criteria.Add("txtComponentId", txtComponentId.Text.Trim());
            criteria.Add("ucIncidentFromDate", ucIncidentFromDate.Text);
            criteria.Add("ucIncidentToDate", ucIncidentToDate.Text);
            criteria.Add("ucReportedFromDate", ucReportedFromDate.Text);
            criteria.Add("ucReportedToDate", ucReportedToDate.Text);
            criteria.Add("ucCompletedFromDate", "");
            criteria.Add("ucCompletedToDate", "");
            criteria.Add("ucClosedFromDate", ucClosedFromDate.Text);
            criteria.Add("ucClosedToDate", ucClosedToDate.Text);
            criteria.Add("ddlConsequenceCategory", ucConsequenceCategory.SelectedHard);
            criteria.Add("ddlProcessSubHazardId", ddlProcessLoss.SelectedValue);
            criteria.Add("ddlPropertySubHazardId", ddlCost.SelectedValue);
            criteria.Add("ddlStatus", ddlStatus.SelectedValue);
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("ucVesselType", ucVesselType.SelectedHard);
            criteria.Add("ucAddressType", ucAddrOwner.SelectedAddress);
            criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            criteria.Add("ucActivity", ucQuickVesselActivity.SelectedQuick);                        
            criteria.Add("ddlCategory", ddlCategory.SelectedValue);
            criteria.Add("ddlSubCategory", ddlSubCategory.SelectedValue);
            criteria.Add("chkCritical", (chkCritical.Checked == true ? "1" : "0"));
            criteria.Add("txtReportedBy", "");
            criteria.Add("txtReportedByName", "");
            criteria.Add("rblnearmisscategory", rblNearmiss.SelectedValue);

            Filter.CurrentMachineryDamageFilter = criteria;
            Response.Redirect("../Inspection/InspectionMachineryDamageList.aspx", false);
        }
    }
    
    protected void BindControls()
    {        
        // Process Loss

        ddlProcessLoss.DataSource = PhoenixInspectionRiskAssessmentSubHazard.RASubHazardListForMachineryDamage(null, 1, null);
        ddlProcessLoss.DataTextField = "FLDNAME";
        ddlProcessLoss.DataValueField = "FLDSUBHAZARDID";
        ddlProcessLoss.DataBind();
        ddlProcessLoss.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

        // Cost

        ddlCost.DataSource = PhoenixInspectionRiskAssessmentSubHazard.RASubHazardListForMachineryDamage(null, null, 1);
        ddlCost.DataTextField = "FLDNAME";
        ddlCost.DataValueField = "FLDSUBHAZARDID";
        ddlCost.DataBind();
        ddlCost.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

        ddlStatus.DataSource = PhoenixInspectionMachineryDamage.MachineryDamageStatusList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlStatus.DataTextField = "FLDSTATUSNAME";
        ddlStatus.DataValueField = "FLDSTATUSID";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));

        ddlCategory.DataSource = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(4);
        ddlCategory.DataTextField = "FLDNAME";
        ddlCategory.DataValueField = "FLDINCIDENTNEARMISSCATEGORYID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlSubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ClearComponent(object sender, EventArgs e)
    {
        txtComponentCode.Text = "";
        txtComponentName.Text = "";
        txtComponentId.Text = "";
    }

    protected void SetVessel()
    {
        NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
        if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
        {
            ViewState["COMPANYID"] = nvc.Get("QMS");
            ucVessel.Company = nvc.Get("QMS");
            ucVessel.bind();
        }

        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.bind();
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                //ucVessel.Enabled = false;

                ucTechFleet.SelectedFleet = "";
                ucTechFleet.Enabled = false;
            }
            else
            {
                ucVessel.SelectedVessel = "";
                ucVessel.Enabled = true;
            }
        }
        else
        {
            ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.InstallCode.ToString();
            ucVessel.Enabled = false;

            ucTechFleet.SelectedFleet = "";
            ucTechFleet.Enabled = false;
        }

                // To bind the component picklist 

        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
            imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListComponent.aspx?vesselid=" + General.GetNullableInteger(ucVessel.SelectedVessel) +"', null); ");
        else
            imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListComponent.aspx?', null); ");
                
    }

    protected void ucAddrOwner_Changed(object sender, EventArgs e)
    {
        if (ViewState["COMPANYID"] != null && ViewState["COMPANYID"].ToString() != "")
        {
            ucVessel.Company = ViewState["COMPANYID"].ToString();
            ucVessel.Owner = General.GetNullableString(ucAddrOwner.SelectedAddress);
            ucVessel.bind();
        }
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
            imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListComponent.aspx?vesselid=" + General.GetNullableInteger(ucVessel.SelectedVessel) + "', null); ");
        else
            imgComponent.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListComponent.aspx?', null); ");
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubCategory.DataSource = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(ddlCategory.SelectedValue));
        ddlSubCategory.DataTextField = "FLDNAME";
        ddlSubCategory.DataValueField = "FLDINCIDENTNEARMISSSUBCATEGORYID";
        ddlSubCategory.DataBind();
        ddlSubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
