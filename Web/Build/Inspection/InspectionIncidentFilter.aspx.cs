using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class InspectionIncidentFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (General.IsTelerik() && Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper().Equals("MENU"))
        {
            Response.Redirect("../Inspection/InspectionIncidentNearMissList.aspx?callfrom=irecord", false);
        }

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuIncidentFilter.AccessRights = this.ViewState;
        MenuIncidentFilter.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            VesselConfiguration();
            ucVessel.Enabled = true;
            ViewState["OFFICEYN"] = "0";
            ViewState["COMPANYID"] = "";

            if (Request.QueryString["OFFICEYN"] != null && Request.QueryString["OFFICEYN"].ToString() != string.Empty)
                ViewState["OFFICEYN"] = Request.QueryString["OFFICEYN"].ToString();

            if(ViewState["OFFICEYN"].ToString().Equals("1"))
            {
                lblFleet.Visible = false;
                lblOwner.Visible = false;
                lblVessel.Visible = false;
                lblVesselType.Visible = false;
                ucTechFleet.Visible = false;
                ucAddrOwner.Visible = false;
                ucVessel.Visible = false;
                ucVesselType.Visible = false;
                chkOfficeAuditIncident.Checked = true;
                ucCompany.Enabled = true;
            }

            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
                ucVessel.Company = nvc.Get("QMS");
                ucVessel.bind();
            }

            ucStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 168, "S1");

            DateTime now = DateTime.Now;
            ucFromDate.Text = now.Date.AddMonths(-6).ToShortDateString();
            ucToDate.Text = DateTime.Now.ToShortDateString();

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                //ucVessel.Enabled = false;
                ucTechFleet.SelectedFleet = "";
                ucTechFleet.Enabled = false;
                chkOfficeAuditIncident.Enabled = false;
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            {
                chkOfficeAuditIncident.Enabled = false;
            }
        }
    }

    protected void MenuIncidentFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        //string Script = "";
        //Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        //Script += "fnReloadList();";
        //Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {            
            criteria.Clear();

            criteria.Add("txtRefNo", txtRefNo.Text.Trim());
            criteria.Add("txtTitle", txtTitle.Text.Trim());
            criteria.Add("txtFromDate", ucFromDate.Text);
            criteria.Add("txtToDate", ucToDate.Text);
            criteria.Add("ucStatus", ucStatus.SelectedHard);
            criteria.Add("ucVessel", ucVessel.SelectedVessel);            
            criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            criteria.Add("ucConsequenceCategory", ucConsequenceCategory.SelectedHard);
            criteria.Add("ucPotentialCategory", ucPotentialCategory.SelectedHard);
            criteria.Add("ucActivity", ucActivity.SelectedHard);
            criteria.Add("ddlIncidentNearmiss", ddlIncidentNearmiss.SelectedValue);
            criteria.Add("ucCategory", ucCategory.SelectedCategory);
            criteria.Add("ucSubCategory", ucSubcategory.SelectedSubCategory);
            criteria.Add("ucVesselType", ucVesselType.SelectedHard);
            criteria.Add("ucAddressType", ucAddrOwner.SelectedAddress);
            criteria.Add("chkOfficeAuditIncident", (chkOfficeAuditIncident.Checked.Equals(true) ? "1" : "0"));
            criteria.Add("ucCompany", ucCompany.SelectedCompany);
            criteria.Add("ucReportedDateFrom", ucReportedDateFrom.Text);
            criteria.Add("ucReportedDateTo", ucReportedDateTo.Text);
            criteria.Add("chkContractedRelatedIncidentYN", chkContractedRelatedIncidentYN.Checked.Equals(true) ?"1" :"0");
            Filter.CurrentIncidentFilterCriteria = criteria;
            //Response.Redirect("../Inspection/InspectionIncidentNearMissList.aspx?callfrom=irecord", false);
        }
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void ucCategory_Changed(object sender, EventArgs e)
    {
        BindSubCategory();
    }

    protected void ddlIncidentNearmiss_Changed(object sender, EventArgs e)
    {
        BindCategory();
        BindSubCategory();
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

    protected void chkOfficeAuditIncident_CheckedChanged(object sender, EventArgs e)
    {
        if (chkOfficeAuditIncident.Checked.Equals(true))
            ucCompany.Enabled = true;
        else
            ucCompany.Enabled = false;
    }

    protected void BindCategory()
    {
        ucCategory.TypeId = ddlIncidentNearmiss.SelectedValue;
        ucCategory.CategoryList = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(General.GetNullableInteger(ddlIncidentNearmiss.SelectedValue == "0" ? null : ddlIncidentNearmiss.SelectedValue));        
        ucCategory.DataBind();        
    }

    protected void BindSubCategory()
    {
        ucSubcategory.CategoryId = ucCategory.SelectedCategory;
        ucSubcategory.SubCategoryList = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(ucCategory.SelectedCategory));
        ucSubcategory.DataBind();
    }
}
