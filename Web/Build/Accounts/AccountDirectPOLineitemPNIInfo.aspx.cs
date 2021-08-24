using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Framework;
using System.Data;

public partial class Accounts_AccountDirectPOLineitemPNIInfo : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ViewState["orderid"] = "";
            if (Request.QueryString["orderid"] != null && Request.QueryString["orderid"] != null)
                ViewState["orderid"] = Request.QueryString["orderid"].ToString();
            if (Request.QueryString["PNIID"] != null && Request.QueryString["PNIID"] != null)
                ViewState["PNIID"] = Request.QueryString["PNIID"].ToString();
            if (Request.QueryString["PNIT"] != null && Request.QueryString["PNIT"] != null)
                ViewState["PNIT"] = Request.QueryString["PNIT"].ToString();

            BindData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    protected void BindData()
    {
        string[] alColumns = { "FLDPARTNAME", "FLDTOTALAMOUNT" };
        string[] alCaptions = { "Item Name", "Amount" };
        DataTable dt = PhoenixAccountsPNIIntergeration.OrderLineItemforPNIinfo(General.GetNullableGuid(ViewState["orderid"].ToString()), General.GetNullableGuid(ViewState["PNIID"].ToString()), ViewState["PNIT"].ToString(),0);
        lblGrid.Text = General.ShowGrid(dt, alColumns, alCaptions);
    }
}
