using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Accounts_AccountsDashboardOutStandingFundsRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["Days"] != null)
            ViewState["Days"] = Request.QueryString["Days"].ToString();

        ddlSubtype.DataBind();
    }

    protected void gvOutstandingFund_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindOutStandingFundData();
    }

    private void BindOutStandingFundData()

    {
        DataSet ds = PhoenixDashboardAccounts.DashboardOutstandingVesselFundRequestdetails(General.GetNullableInteger(ddlSubtype.SelectedValue));
        gvOutstandingFund.DataSource = ds.Tables[0];
        //  gvOutstandingFund.DataBind();
    }
    protected void ddlSubtype_TextChanged(object sender, EventArgs e)
    {
        gvOutstandingFund.Rebind();
    }
    protected void ddlSubtype_DataBound(object sender, EventArgs e)
    {
        ddlSubtype.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }

    protected void gvOutstandingFund_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        
        if (e.Item is GridDataItem)
        {
            if(ViewState["Days"].ToString() == "GTHREE")
            {
                gvOutstandingFund.MasterTableView.GetColumn("UniqueGThree").Visible = true;
                gvOutstandingFund.MasterTableView.GetColumn("UniqueGSix").Visible = false;
                gvOutstandingFund.MasterTableView.GetColumn("UniqueGNine").Visible = false;
            }

            if (ViewState["Days"].ToString() == "GSIX")
            {
                gvOutstandingFund.MasterTableView.GetColumn("UniqueGThree").Visible = false;
                gvOutstandingFund.MasterTableView.GetColumn("UniqueGSix").Visible = true;
                gvOutstandingFund.MasterTableView.GetColumn("UniqueGNine").Visible = false;
            }

            if (ViewState["Days"].ToString() == "GNINE")
            {
                gvOutstandingFund.MasterTableView.GetColumn("UniqueGThree").Visible = false;
                gvOutstandingFund.MasterTableView.GetColumn("UniqueGSix").Visible = false;
                gvOutstandingFund.MasterTableView.GetColumn("UniqueGNine").Visible = true;
            }

            RadLabel lblmeasure = (RadLabel)e.Item.FindControl("lblmeasure");

            RadLabel lblPrincipalid = (RadLabel)e.Item.FindControl("lblPrincipalid");

            RadLabel lbl30daysurl = (RadLabel)e.Item.FindControl("lbl30daysurl");
            LinkButton lnk33dayscount = (LinkButton)e.Item.FindControl("lnk33dayscount");

            RadLabel lbl90daysurl = (RadLabel)e.Item.FindControl("lbl90daysurl");
            LinkButton lnk90dayscount = (LinkButton)e.Item.FindControl("lnk90dayscount");

            RadLabel lblgrt90url = (RadLabel)e.Item.FindControl("lblgrt90url");
            LinkButton lnkgrt90count = (LinkButton)e.Item.FindControl("lnkgrt90count");

            RadLabel lbltotalurl = (RadLabel)e.Item.FindControl("lbltotalurl");
            LinkButton lnktotalcount = (LinkButton)e.Item.FindControl("lnktotalcount");

            if (lnk33dayscount != null)
            {
                lnk33dayscount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + "','" + lbl30daysurl.Text + "&principalid=" + lblPrincipalid.Text +"&Code=" + 1 + "'); return false;");
            }

            if (lnk90dayscount != null)
            {
                lnk90dayscount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + "','" + lbl90daysurl.Text + "&principalid=" + lblPrincipalid.Text + "'); return false;");
            }

            if (lnkgrt90count != null)
            {
                lnkgrt90count.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + "','" + lblgrt90url.Text + "&principalid=" + lblPrincipalid.Text +"&Code=" + 1 + "&Days="+ ViewState["Days"].ToString()+"'); return false;");
            }

            if (lnktotalcount != null)
            {
                lnktotalcount.Attributes.Add("onclick", "javascript:top.openNewWindow('wo','" + lblmeasure.Text + "','" + lbltotalurl.Text + "&principalid=" + lblPrincipalid.Text + "'); return false;");
            }
        }
    }
}