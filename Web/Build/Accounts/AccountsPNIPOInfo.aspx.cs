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

public partial class Accounts_AccountsPNIPOInfo : PhoenixBasePage
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

            BindData();
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }

    protected void BindData()
    {
        string[] alColumns = { "FLDFORMNO","FLDINVOICENUMBER", "FLDAMOUNT" };
        string[] alCaptions = { "PO Num","INV Num", "Amount" };
        DataTable dt = PhoenixAccountsPNIIntergeration.PoInfoList(General.GetNullableGuid(ViewState["orderid"].ToString()), General.GetNullableGuid(ViewState["PNIID"].ToString()));
        lblGrid.Text = General.ShowGrid(dt, alColumns, alCaptions);
    }
}
