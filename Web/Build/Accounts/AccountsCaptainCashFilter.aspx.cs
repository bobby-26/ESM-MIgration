using System;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class AccountsCaptainCashFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Go", "GO");
            MenuOfficeFilterMain.MenuList = toolbar.Show();
            txtVoucherNo.Focus();
            BindYear();

            ddlAccountDetails.DataSource = PhoenixRegistersVesselAccount.ListUniqueVesselAccount(null, 1);
            ddlAccountDetails.DataBind();
        }
    }

    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year); i++)
        {
            ListItem li = new ListItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
    }

    protected void ddlAccountDetails_DataBound(object sender, EventArgs e)
    {
        ddlAccountDetails.Items.Insert(0, new ListItem("--Select--", ""));
    }

    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("GO"))
        {
            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("ddlVessel", ddlVessel.SelectedVessel);
            criteria.Add("ddlVesselAccountCode", ddlVessel.SelectedVessel);
            criteria.Add("txtVoucherNo", txtVoucherNo.Text);
            criteria.Add("ddlYear", ddlVessel.SelectedVessel);
            criteria.Add("ddlMonth", ddlVessel.SelectedVessel);
            criteria.Add("txtFromdate", txtFromDate.Text);
            criteria.Add("txtToDate", txtToDate.Text);

            Filter.CurrentCaptainCashOfficeFilter = criteria;
        }
        Response.Redirect("AccountsCaptainCash.aspx", false);
    }
}
