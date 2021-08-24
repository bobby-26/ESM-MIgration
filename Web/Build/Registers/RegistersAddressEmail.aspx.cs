using System;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using System.Data;

public partial class RegistersAddressEmail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataSet ds = PhoenixRegistersAddress.EditAddress(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                long.Parse(Request.QueryString["addresscode"].ToString()));

            if (ds.Tables.Count > 0)
            {
                txtEmails.Text = ds.Tables[0].Rows[0]["FLDEMAIL1"].ToString();
            }
        }
    }
}
