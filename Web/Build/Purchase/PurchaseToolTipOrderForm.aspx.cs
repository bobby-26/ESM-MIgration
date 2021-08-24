using System;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;

public partial class PurchaseToolTipOrderForm : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["orderid"] != null && Request.QueryString["vesselid"] != null)
                BindData(Request.QueryString["orderid"].ToString(), Request.QueryString["vesselid"].ToString());
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    private void BindData(string orderid, string vesselid)
    {
        if (orderid == "")
            orderid = Guid.NewGuid().ToString();

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseOrderForm.EditOrderForm(new Guid(orderid), General.GetNullableInteger(vesselid));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            lbltNumber.Text = dr["FLDFORMNO"].ToString();
            lbltFormTitle.Text = dr["FLDTITLE"].ToString();
            lbltVendor.Text = dr["FLDVENDORNAME"].ToString();
            //lbltFormType.Text = dr["FLDFORMTYPE"].ToString();
            lbltFromStatus.Text = dr["FLDFORMSTATUSNAME"].ToString();
            lbltBudgetCode.Text = dr["FLDBUDGETNAME"].ToString();
            lbltApprovedDate.Text = General.GetDateTimeToString(dr["FLDPURCHASEAPPROVEDATE"].ToString());
            lbltOrderedDate.Text = General.GetDateTimeToString(dr["FLDORDEREDDATE"].ToString());
            lbltReceivedDate.Text = General.GetDateTimeToString(dr["FLDEXPORTIMPORTDATE"].ToString());
            lbltType.Text = dr["FLDFORMTYPENAME"].ToString();
            lbltVesselReceivedDate.Text = General.GetDateTimeToString(dr["FLDVENDORDELIVERYDATE"].ToString());
        }
    }
}
