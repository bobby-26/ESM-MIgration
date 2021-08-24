using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaAgreementsAttachment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["qfrom"] != null && Request.QueryString["qfrom"] != string.Empty)
        {
            toolbar.AddButton("Address", "ADDRESS");
        }
        toolbar.AddButton("Agreements", "AGREEMENTSATTACHMENT");

        MenuAgreementsAttachment.AccessRights = this.ViewState;
        MenuAgreementsAttachment.MenuList = toolbar.Show();
        MenuAgreementsAttachment.SetTrigger(pnlAgreementsAttachment);

        if (Request.QueryString["qfrom"] != null && Request.QueryString["qfrom"] != string.Empty)
        {
            MenuAgreementsAttachment.SelectedMenuIndex = 1;
        }
        else
        {
            MenuAgreementsAttachment.SelectedMenuIndex = 0;
        }
        if (!IsPostBack)
        {
            if (Request.QueryString["ADDRESSCODE"] != null && Request.QueryString["ADDRESSCODE"] != string.Empty)
            {
                DataSet ds = PhoenixPreSeaAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    long.Parse(Request.QueryString["ADDRESSCODE"].ToString()));

                ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"].ToString();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["DTKey"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                }
            }
            else
            {
                Response.Redirect("../PreSea/PreSeaOffice.aspx", true);
            }
        }
        ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.PRESEA + "&type=AGREEMENTS";
    }
    protected void AgreementsAttachment_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("ADDRESS"))
        {
            Response.Redirect("../PreSea/PreSeaOffice.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"].ToString());
        }
    }
}
