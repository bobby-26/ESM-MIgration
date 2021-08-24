using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase ;


public partial class PurchaseServiceDetail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        Tab.MenuList = toolbar.Show();

        if (!IsPostBack)
        {         
            BindFields();
        }
    }

    private void BindFields()
    {
        try
        {

            if ((Request.QueryString["WORKORDERID"] != null) && (Request.QueryString["WORKORDERID"] != ""))
            {
                DataSet ds = PhoenixPurchaseOrderLine.EditComponentJob(new Guid(Request.QueryString["WORKORDERID"].ToString()));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtComponentDetail.Content = dr["FLDJOBDETAILS"].ToString();
                    ViewState["dtkey"] = dr["FLDDTKEY"].ToString();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
