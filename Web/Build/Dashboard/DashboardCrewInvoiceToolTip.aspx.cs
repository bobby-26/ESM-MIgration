using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;

public partial class DashboardCrewInvoiceToolTip : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["INVOICENO"] != null)
                BindData(Request.QueryString["INVOICENO"].ToString());
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void BindData(string invoiceNO)
    {
        DataTable dt = new DataTable();
        dt = PhoenixDashboardTooltip.CrewInvoice(invoiceNO);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            lblInvNovalue.Text = dr["FLDINVOICENUMBER"].ToString();
            lblSupCodeValue.Text = dr["FLDCODE"].ToString();
            lblSupNameValue.Text = dr["FLDNAME"].ToString();
            lblVendorInvNoValue.Text = dr["VENDORINVNO"].ToString();
            lblInvStatusValue.Text = dr["FLDHARDNAME"].ToString();

        }
    }
}
