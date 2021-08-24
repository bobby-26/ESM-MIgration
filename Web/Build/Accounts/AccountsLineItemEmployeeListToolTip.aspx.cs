using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web;
using System.Collections.Specialized;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class Accounts_AccountsLineItemEmployeeListToolTip : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
              if (Request.QueryString["employeeid"] != null)
                ViewState["employeeid"] = Request.QueryString["employeeid"].ToString();
              if (Request.QueryString["BANKACCOUNTID"] != null)
                    ViewState["BANKACCOUNTID"] = Request.QueryString["BANKACCOUNTID"].ToString();
              if (Request.QueryString["VESSELID"] != null)
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
              if (Request.QueryString["PVID"] != null)
                    ViewState["PVID"] = Request.QueryString["PVID"].ToString();

                BindData();
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
        try
        {
            //if (!IsPostBack)
            //{
            DataTable dt = PhoenixAccountsInvoicePaymentVoucher.LineItemEmployeeDetailsList(new Guid(ViewState["PVID"].ToString()), General.GetNullableInteger(ViewState["VESSELID"].ToString()), int.Parse(ViewState["employeeid"].ToString()), new Guid(ViewState["BANKACCOUNTID"].ToString()));
            gvPVEmployeeDetail.DataSource = dt;

            //}

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvPVEmployeeDetail.SelectedIndexes.Clear();
        gvPVEmployeeDetail.EditIndexes.Clear();
        gvPVEmployeeDetail.DataSource = null;
        gvPVEmployeeDetail.Rebind();
    }

    protected void gvPVEmployeeDetail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}