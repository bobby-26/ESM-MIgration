using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Data;
using Telerik.Web.UI;

public partial class InspectionOpenReportsFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (General.IsTelerik() && Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper().Equals("MENU"))
        {
            Response.Redirect("../Inspection/InspectionDirectIncidentList.aspx", false);
        }

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
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

            DateTime now = DateTime.Now;
            ucFromDate.Text = now.Date.AddMonths(-6).ToShortDateString();
            ucToDate.Text = DateTime.Now.ToShortDateString();

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                //ucVessel.Enabled = false;
            }
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

        //string Script = "";
        //Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        //Script += "fnReloadList();";
        //Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            criteria.Clear();

            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("txtFromDate", ucFromDate.Text);
            criteria.Add("txtToDate", ucToDate.Text);
            criteria.Add("ddlStatus", ucORStatus.SelectedHard);
            criteria.Add("ucReviewCategory", ucReviewCategory.SelectedQuick);
            criteria.Add("ucReviewSubcategory", ucReviewSubcategory.SelectedQuick);
            criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            criteria.Add("ucAddrOwner", ucAddrOwner.SelectedAddress);
            criteria.Add("ucVesselType", ucVesselType.SelectedHard);
            criteria.Add("ucDept", ucDept.SelectedDepartment);
            criteria.Add("ucCompany", ucCompany.SelectedCompany);

            Filter.CurrentOpenReportsFilter  = criteria;
            //Response.Redirect("../Inspection/InspectionDirectIncidentList.aspx", false);
        }
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
}
