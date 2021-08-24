using System;
using System.Collections.Specialized;
using System.Text;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;

public partial class InspectionRAGenericFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
            MenuRAGenericFilter.AccessRights = this.ViewState;
            MenuRAGenericFilter.MenuList = toolbar.Show();
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
        ddlStatus.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void MenuRAGenericFilter_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        if (dce.CommandName.ToUpper().Equals("GO"))
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
            Filter.CurrentGenericRAFilter = criteria;            
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }   
}
