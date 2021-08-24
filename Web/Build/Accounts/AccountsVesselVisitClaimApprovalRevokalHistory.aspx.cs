using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsVesselVisitClaimApprovalRevokalHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                string strno = "";
                
                if (Request.QueryString["visitId"] != null && Request.QueryString["visitId"] != string.Empty)
                    ViewState["voucherid"] = Request.QueryString["visitId"];
                else
                    ViewState["voucherid"] = "";
                if (Request.QueryString["Formnumber"] != null && Request.QueryString["Formnumber"] != string.Empty)
                    strno = Request.QueryString["Formnumber"];
               PhoenixToolbar header = new PhoenixToolbar();
               
                MenuHistory.Title = "Approval History " + " - " + strno;
                MenuHistory.MenuList = header.Show();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        DataSet ds = new DataSet();
        if (General.GetNullableGuid(ViewState["voucherid"].ToString()) != null)
        {
            ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimApprovalHistory(new Guid(ViewState["voucherid"].ToString()));

            gvRevokeHistory.DataSource = ds;
        }
    }
    protected void MenuHistory_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    }
    protected void gvRevokeHistory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void BindApprovalData()
    {
        DataSet ds = new DataSet();
        if (General.GetNullableGuid(ViewState["voucherid"].ToString()) != null)
        {
            ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimApprovalHistorySearch(new Guid(ViewState["voucherid"].ToString()));

            gvApprovaHistory.DataSource = ds;

        }
    }
    protected void gvApprovaHistory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvApprovaHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindApprovalData();
    }

    protected void gvRevokeHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
