using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;
public partial class Inspection_InspectionUnsafeActsConditionsFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            if (General.IsTelerik() && Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper().Equals("MENU"))
            {
                Response.Redirect("../Inspection/InspectionUnsafeActsConditionsList.aspx", false);
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
            MenuOpenReportsFilter.AccessRights = this.ViewState;
            MenuOpenReportsFilter.MenuList = toolbar.Show();
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
                BindSubCategory();

                DateTime now = DateTime.Now;
                ucIncidentNearMissFromDate.Text = now.Date.AddMonths(-6).ToShortDateString();
                ucIncidentNearMissToDate.Text = DateTime.Now.ToShortDateString();
                ucStatusofUA.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 146, "OPN");

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

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
    protected void MenuOpenReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            //string Script = "";
            //Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            //Script += "fnReloadList();";
            //Script += "</script>" + "\n";

            NameValueCollection criteria = new NameValueCollection();

            if (CommandName.ToUpper().Equals("GO"))
            {
                criteria.Clear();

                criteria.Add("txtReferenceNumber", txtReferenceNumber.Text);
                criteria.Add("ucVessel", ucVessel.SelectedVessel);
                criteria.Add("txtFromDate", ucCreatedFromDate.Text);
                criteria.Add("txtToDate", ucCreatedToDate.Text);
                criteria.Add("ucIncidentNearMissFromDate", ucIncidentNearMissFromDate.Text);
                criteria.Add("ucIncidentNearMissToDate", ucIncidentNearMissToDate.Text);
                criteria.Add("ucCategory", ucCategory.SelectedHard);
                criteria.Add("ddlStatus", "0");
                criteria.Add("ucStatusofUA", ucStatusofUA.SelectedHard);
                criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
                criteria.Add("ucAddrOwner", ucAddrOwner.SelectedAddress);
                criteria.Add("ucVesselType", ucVesselType.SelectedHard);
                criteria.Add("ddlSubcategory", ddlSubcategory.SelectedValue);
                criteria.Add("chkIncidentNearMissRaisedYN", chkIncidentNearMissRaisedYN.Checked == true ? "1" : "0");

                Filter.CurrentUnsafeActsConditionsFilter = criteria;
                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void ucCategory_TextChanged(object sender, EventArgs e)
    {
        BindSubCategory();
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
}
