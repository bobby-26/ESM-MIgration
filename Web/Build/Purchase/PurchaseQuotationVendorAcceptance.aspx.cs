using System;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Data;
using System.Text;

public partial class PurchaseQuotationVendorAcceptance : PhoenixBasePage
{
	string sessionId = "";
    string flag = "";
	protected void Page_Load(object sender, EventArgs e)
	{
		
		if(!IsPostBack)
		{
			sessionId = Request.QueryString["sessionId"].ToString();
            flag = Request.QueryString["flag"].ToString();

            sessionValidation();
		}
	}
	private void sessionValidation()
	{
		try
		{
            PhoenixPurchaseOrderForm.UpdateAcceptanceflag(new Guid(sessionId), int.Parse(flag), 1);
            if (flag == "1")
			{
				//string result;
				Response.Write("Order Accepted Successfully.");
				//Response.Write(result);
			}
			else
			{
				Response.Write("Order Rejected.");
			}
		}
		catch(Exception ex)
		{
			Response.Write(ex.Message);
		}
	}

}
