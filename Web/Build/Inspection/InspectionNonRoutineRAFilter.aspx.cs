using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Inspection_InspectionNonRoutineRAFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO", ToolBarDirection.Right);
        MenuRAGenericFilter.AccessRights = this.ViewState;
        MenuRAGenericFilter.MenuList = toolbar.Show();
        if (!IsPostBack)
        {            
            txtRefNo.Focus();
            BindStatus();

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
                ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                ucVessel.Enabled = false;
            }
        }
    }

    private void BindStatus()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("FLDSTATUSID", typeof(string));
        dt.Columns.Add("FLDSTATUSNAME", typeof(string));
        dt.Rows.Add("1", "Draft");
        dt.Rows.Add("2", "Approved");
        dt.Rows.Add("3", "Issued");
        dt.Rows.Add("4", "Pending approval");

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
        {
            dt.Rows.Add("5", "Approved for use, subject to completion of Tasks");
        }
        else
        {
            dt.Rows.Add("5", "Approved for use");
        }
        dt.Rows.Add("6", "Not Approved");
        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper() == "OFFSHORE")
        {
            dt.Rows.Add("7", "Approved for use");
        }
        else
        {
            dt.Rows.Add("7", "Completed");
        }

        ddlStatus.DataSource = dt;
        ddlStatus.DataTextField = "FLDSTATUSNAME";
        ddlStatus.DataValueField = "FLDSTATUSID";
        ddlStatus.DataBind();
        ddlStatus.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ddlRAType_Changed(object sender, EventArgs e)
    {
        if (ddlRAType.SelectedValue == "0")
        {

        }
    }

    protected void MenuRAGenericFilter_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (ddlRAType.SelectedValue == "2")
        {
            if (CommandName.ToUpper().Equals("GO"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ddlRAType", ddlRAType.SelectedValue);
                criteria.Add("txtRefNo", txtRefNo.Text);
                criteria.Add("txtActivityConditions", txtActivityConditions.Text.ToString());
                criteria.Add("ucVessel", ucVessel.SelectedVessel);
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                criteria.Add("ucDatePreparedFrom", ucDatePreparedFrom.Text);
                criteria.Add("ucDatePreparedTo", ucDatePreparedTo.Text);
                criteria.Add("ucDateIntendedWorkFrom", ucDateIntendedWorkFrom.Text);
                criteria.Add("ucDateIntendedWorkTo", ucDateIntendedWorkTo.Text);
                criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
                Filter.CurrentGenericRAFilter = criteria;
            }
            Response.Redirect("../Inspection/InspectionRAGenericList.aspx", false);
        }

        if (ddlRAType.SelectedValue == "3")
        {
            if (CommandName.ToUpper().Equals("GO"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtRefNo", txtRefNo.Text);
                criteria.Add("txtActivityConditions", txtActivityConditions.Text.ToString());
                criteria.Add("ucVessel", ucVessel.SelectedVessel);
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                criteria.Add("ucDatePreparedFrom", ucDatePreparedFrom.Text);
                criteria.Add("ucDatePreparedTo", ucDatePreparedTo.Text);
                criteria.Add("ucDateIntendedWorkFrom", ucDateIntendedWorkFrom.Text);
                criteria.Add("ucDateIntendedWorkTo", ucDateIntendedWorkTo.Text);
                criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);
                Filter.CurrentMachineryRAFilter = criteria;
            }
            Response.Redirect("../Inspection/InspectionRAMachineryList.aspx", false);
        }

        if (ddlRAType.SelectedValue == "1")
        {
            if (CommandName.ToUpper().Equals("GO"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("txtRefNo", txtRefNo.Text);
                criteria.Add("txtActivityConditions", txtActivityConditions.Text.ToString());
                criteria.Add("ucVessel", ucVessel.SelectedVessel);
                criteria.Add("ddlStatus", ddlStatus.SelectedValue);
                criteria.Add("ucDatePreparedFrom", ucDatePreparedFrom.Text);
                criteria.Add("ucDatePreparedTo", ucDatePreparedTo.Text);
                criteria.Add("ucDateIntendedWorkFrom", ucDateIntendedWorkFrom.Text);
                criteria.Add("ucDateIntendedWorkTo", ucDateIntendedWorkTo.Text);
                criteria.Add("ucTechFleet", ucTechFleet.SelectedFleet);

                Filter.CurrentNavigationRAFilter = criteria;
            }
            Response.Redirect("../Inspection/InspectionRANavigationList.aspx", false);
        }        
    }
}
