using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Elog;
using Telerik.Web.UI;

public partial class ElectricLogEngineAttributesRevisionHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["RevisionId"] = "";
            if (Request.QueryString["RevisionId"] != null && Request.QueryString["RevisionId"].ToString() != "")
            {
                ViewState["RevisionId"] = Request.QueryString["RevisionId"].ToString();
            }

            BindRevDetails();
        }
    }

    protected void gvOperationalHazard_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindGrid();
    }

    protected void gvtank_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindTank();
    }

    private void BindGrid()
    {
        DataSet ds = new DataSet();
        ds = PhoenixEngineLogAttributes.EngineLogAttributesHistoryList(General.GetNullableGuid(ViewState["RevisionId"].ToString()), 0);
        gvOperationalHazard.DataSource = ds;
    }

    private void BindTank()
    {
        DataSet ds = new DataSet();
        ds = PhoenixEngineLogAttributes.EngineLogAttributesHistoryList(General.GetNullableGuid(ViewState["RevisionId"].ToString()), 1);
        gvtank.DataSource = ds;
    }

    private void BindRevDetails()
    {
        DataSet ds = new DataSet();
        ds = PhoenixEngineLogAttributes.EngineLogAttributesHistoryList(General.GetNullableGuid(ViewState["RevisionId"].ToString()), 1);
        if (ds.Tables[1].Rows.Count > 0)
        {
            txtrevno.Text = ds.Tables[1].Rows[0]["FLDREVISIONNO"].ToString();
            ucdate.Text = ds.Tables[1].Rows[0]["FLDREVISEDDATE"].ToString();
            if (ds.Tables[1].Rows[0]["FLDACTIVEYN"].ToString().Equals("1"))
                ChkPublishedYN.Checked = true;
            else
                ChkPublishedYN.Checked = false;
        }
    }
}