using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using System.Data;
using Telerik.Web.UI;

public partial class InspectionPreventiveTaskFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuScheduleFilter.AccessRights = this.ViewState;
        MenuScheduleFilter.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
                ucVessel.Company = nvc.Get("QMS");
                ucVessel.bind();
            }

            DateTime now = DateTime.Now;
            txtPreFrom.Text = now.Date.AddMonths(-6).ToShortDateString();
            txtPreTo.Text = DateTime.Now.ToShortDateString();

            VesselConfiguration();
            ucVessel.Enabled = true;
            ucTechFleet.Enabled = true;

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                //ucVessel.Enabled = false;
                ucTechFleet.SelectedFleet = "";
                ucTechFleet.Enabled = false;
                //chkOfficeAuditDeficiencies.Enabled = false;
            }

            BindCategory();
            BindSubcategory();
            BindUser();
        }
    }
    protected void MenuScheduleFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        //string Script = "";
        //Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        //Script += "fnReloadList();";
        //Script += "</script>" + "\n";

        if (CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection precriteria = new NameValueCollection();

            precriteria.Clear();

            precriteria.Add("txtFrom", txtPreFrom.Text);
            precriteria.Add("txtTo", txtPreTo.Text);
            precriteria.Add("ddlAcceptance", ddlAcceptance.SelectedValue);
            precriteria.Add("txtDoneFrom", txtPreDoneDateFrom.Text);
            precriteria.Add("txtDoneTo", txtPreDoneDateTo.Text);
            precriteria.Add("ucDepartment", ucDepartment.SelectedDepartment);
            precriteria.Add("ddlCategory", ddlCategory.SelectedValue);
            precriteria.Add("ddlSubcategory", ddlSubcategory.SelectedValue);
            precriteria.Add("ucVessel", ucVessel.SelectedVessel);
            precriteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            precriteria.Add("ucVesselType", ucVesselType.SelectedHard);
            precriteria.Add("ucAddrOwner", ucAddrOwner.SelectedAddress);
            precriteria.Add("ddlSourceType", ddlPreSourceType.SelectedValue == "0" ? null : ddlPreSourceType.SelectedValue);
            precriteria.Add("txtSourceRefNo", txtPreSourceRefNo.Text);
            precriteria.Add("ddlAcceptedBy", ddlAcceptedBy.SelectedValue);

            Filter.CurrentPreventiveTaskFilter = precriteria;
        }
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

    protected void ucAddrOwner_Changed(object sender, EventArgs e)
    {
        if (ViewState["COMPANYID"] != null && ViewState["COMPANYID"].ToString() != "")
        {
            ucVessel.Company = ViewState["COMPANYID"].ToString();
            ucVessel.Owner = General.GetNullableString(ucAddrOwner.SelectedAddress);
            ucVessel.bind();
        }
    }

    protected void BindCategory()
    {
        ddlCategory.DataSource = PhoenixInspectionTaskCategory.InspectionTaskCategoryList();
        ddlCategory.DataTextField = "FLDCATEGORYNAME";
        ddlCategory.DataValueField = "FLDCATEGORYID";
        ddlCategory.DataBind();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindSubcategory()
    {
        ddlSubcategory.DataSource = PhoenixInspectionTaskCategory.InspectionTaskSubCategoryList(General.GetNullableGuid(ddlCategory.SelectedValue));
        ddlSubcategory.DataTextField = "FLDSUBCATEGORYNAME";
        ddlSubcategory.DataValueField = "FLDSUBCATEGORYID";
        ddlSubcategory.DataBind();
        ddlSubcategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void chkOfficeAuditDeficiencies_CheckedChanged(object sender, EventArgs e)
    {
        //if (chkOfficeAuditDeficiencies.Checked)
        //    ucCompany.Enabled = true;
        //else
        //    ucCompany.Enabled = false;
    }
    protected void ddlCategory_Changed(object sender, EventArgs e)
    {
        BindSubcategory();
    }

    protected void BindUser()
    {
        DataSet ds = PhoenixInspection.UserList(null);
        ddlAcceptedBy.DataSource = ds.Tables[0];
        ddlAcceptedBy.DataTextField = "FLDDESIGNATIONNAME";
        ddlAcceptedBy.DataValueField = "FLDUSERCODE";
        ddlAcceptedBy.DataBind();
        ddlAcceptedBy.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
