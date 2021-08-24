using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Web.UI;

public partial class Accounts_AccountsSOAGenerationManualReportsDetails : PhoenixBasePage
{
    public decimal dSumReversed = 0;
    public decimal dSumCharged  = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            PhoenixToolbar toolbarsave = new PhoenixToolbar();
            toolbarsave.AddButton("Verify", "VERIFY");
            MenuCommittedcostpostTab.AccessRights = this.ViewState;
            MenuCommittedcostpostTab.MenuList = toolbarsave.Show();

            if (Request.QueryString["reportverificationid"] != null)
            {
                ViewState["reportverificationid"] = Request.QueryString["reportverificationid"].ToString();
            }     
            if (Request.QueryString["dtkey"] != null)
            {

                ifMoreInfo.Attributes["src"] = "../common/download.aspx?dtkey=" + Request.QueryString["dtkey"]; //Session["sitepath"] + "/attachments/" + Request.QueryString["filepath"].ToString();
            }

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["BUDGETID"] = "";
             }         
          }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
      }   

    protected void CommittedcostpostTab_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("VERIFY"))
            {
                PhoenixAccountsSOAGeneration.UpdateSOAGenerationManualReportsVerification(new Guid(ViewState["reportverificationid"].ToString()));
                ucStatus.Text = "Attachment has been verified";
                ucStatus.Visible = true;
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                             "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', 'keepopen');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
      }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
     
    }

}
