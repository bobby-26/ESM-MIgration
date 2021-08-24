using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Owners;

public partial class OwnersMoreInfoBillingParties : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        string ownerid = Request.QueryString["ownerid"].ToString();       //Session["OWNERID"] == null ? "0" : Session["OWNERID"].ToString();

        DataSet ds = PhoenixOwnersMapping.EditOwnerMapping(
            General.GetNullableInteger(ownerid) == null ? 0 : int.Parse(ownerid));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            string billingparties = dr["FLDBILLINGPARTY"].ToString() == "" ? "0" : dr["FLDBILLINGPARTY"].ToString();

            DataSet dsBillingParties = PhoenixOwnersMapping.ListBillingParty(billingparties);

            if (dsBillingParties.Tables[0].Rows.Count > 0)
            {
                gvBillingParties.DataSource = dsBillingParties.Tables[0];
                gvBillingParties.DataBind();
            }
            else
            {
                Response.Write("Owner doesn't have any Billing Party.");
            }
        }
        else
        {
            Response.Write("Owner doesn't have any Billing Party.");
        }
    }
}
