using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Web.UI;

public partial class InspectionSIREPlannerFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (General.IsTelerik() && Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper().Equals("MENU"))
        {
            Response.Redirect("../Inspection/InspectionSIREPlanner.aspx", false);
        }  

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

            VesselConfiguration();
            ucVessel.Enabled = true;
            ucTechFleet.Enabled = true;

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.bind();
                //ucVessel.Enabled = false;
                ucTechFleet.SelectedFleet = "";
                ucTechFleet.Enabled = false;

            }
            if (Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration != "" && int.Parse(Filter.CurrentVesselConfiguration) > 0)
            {
                ddlPlanned.SelectedValue = "1";
                ddlPlanned.Enabled = false;
            }
            BindCompany();
            BindBasis();
            Bind_UserControls(sender, new EventArgs());
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
            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("ucVetting", ucVetting.SelectedValue);
            criteria.Add("ucVessel", ucVessel.SelectedVessel);
            criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
            criteria.Add("ucPort", ucPort.SelectedValue);
            criteria.Add("txtFrom", txtFrom.Text);
            criteria.Add("txtTo", txtTo.Text);
            criteria.Add("ddlCompany", ddlCompany.SelectedValue);
            criteria.Add("ddlPlanned", (ddlPlanned.SelectedValue == "2" ? null : ddlPlanned.SelectedValue));
            criteria.Add("ucDoneFrom", ucDoneFrom.Text);
            criteria.Add("ucDoneTo", ucDoneTo.Text);
            criteria.Add("txtInspector", txtInspector.Text);
            criteria.Add("ucVesselType", ucVesselType.SelectedHard);
            criteria.Add("ucAddrOwner", ucAddrOwner.SelectedAddress);
            criteria.Add("ucCharterer", ucCharterer.SelectedAddress);
            criteria.Add("ddlBasis", ddlBasis.SelectedValue);
            criteria.Add("ucPlannedFrom", ucPlannedFrom.Text);
            criteria.Add("ucPlannedTo", ucPlannedTo.Text);

            Filter.CurrentScheduleByCompanyFilter = criteria;
            //Response.Redirect("../Inspection/InspectionSIREPlanner.aspx", false);
        }
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

    protected void Bind_UserControls(object sender, EventArgs e)
    {
        string type = PhoenixCommonRegisters.GetHardCode(1, 148, "INS");
        ucVetting.DataSource = PhoenixInspection.ListInspectionByCompany(General.GetNullableInteger(type)
                                        , null
                                        , null
                                        , 1
                                        , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ucVetting.DataTextField = "FLDSHORTCODE";
        ucVetting.DataValueField = "FLDINSPECTIONID";
        ucVetting.DataBind();
        ucVetting.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void BindCompany()
    {
        DataSet ds = PhoenixInspectionSchedule.ListInspectionCompany(null);

        ddlCompany.DataSource = ds.Tables[0];
        ddlCompany.DataTextField = "FLDCOMPANYNAME";
        ddlCompany.DataValueField = "FLDCOMPANYID";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ucVessel_Changed(object sender, EventArgs e)
    {
        BindBasis();
    }

    protected void ucVetting_Changed(object sender, EventArgs e)
    {
        BindBasis();
    }

    protected void BindBasis()
    {
        ddlBasis.DataSource = null;
        DataSet ds = PhoenixCommonInspection.BasisInspectionScheduleList(General.GetNullableInteger(ucVessel.SelectedVessel)
            , General.GetNullableGuid(ucVetting.SelectedValue)
            , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        ddlBasis.DataSource = ds;
        ddlBasis.DataValueField = "FLDBASISSCHEDULEID";
        ddlBasis.DataTextField = "FLDBASISSDETAILS";
        ddlBasis.DataBind();
        ddlBasis.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
}
