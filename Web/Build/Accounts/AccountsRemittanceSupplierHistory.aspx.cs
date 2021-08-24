using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsRemittanceSupplierHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvInvoice.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            if (ViewState["TOTALPAGECOUNT"] == null)
				ViewState["TOTALPAGECOUNT"] = 1;
            if (Request.QueryString["REMITTANCEID"] != null)
            {
                ViewState["REMITTANCEID"] = Request.QueryString["REMITTANCEID"].ToString();
            }
        }      
        txtVendorId.Attributes.Add("style", "display:none");
    }

    protected void BindData()
    {
        if (ViewState["REMITTANCEID"] != null && ViewState["REMITTANCEID"].ToString() != "")
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataTable dt = new DataTable();
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            dt = PhoenixAccountsRemittance.SupplierRemittanceHistory(new Guid(ViewState["REMITTANCEID"].ToString())
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvInvoice.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtVendorCode.Text = dr["FLDCODE"].ToString();
                txtVenderName.Text = dr["FLDNAME"].ToString();
            }
            else
            {
              //  ShowNoRecordsFound(dt, gvInvoice);
            }

            gvInvoice.DataSource = dt;
            gvInvoice.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
    }
    protected void gvInvoice_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvInvoice_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInvoice.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
