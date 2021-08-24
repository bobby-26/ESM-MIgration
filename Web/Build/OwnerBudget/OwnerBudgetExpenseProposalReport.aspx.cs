using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.OwnerBudget;


public partial class OwnerBudgetExpenseProposalReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (Request.QueryString["proposalid"] != null)
            {
                ViewState["PROPOSALID"] = Request.QueryString["proposalid"].ToString();
            }
            if (Request.QueryString["revisionid"] != null)
            {
                ViewState["REVISIONID"] = Request.QueryString["revisionid"].ToString();
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
           
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Back", "REVISIONS");
            
            MenuClose.AccessRights = this.ViewState;
            MenuClose.MenuList = toolbar.Show();

            GetData();
        }
    }

    private void GetData()
    {
        string htmltext="";   
        PhoenixOwnerBudgetRegisters.ProposalReport(new Guid(ViewState["PROPOSALID"].ToString())
                                            ,ref htmltext);

        span2.InnerHtml = span2.InnerHtml + htmltext;
       
    }

    protected void MenuClose_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("REVISIONS"))
            {
                Response.Redirect("OwnerBudgetProposalRevision.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"]);
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void MenuFind_TabStripCommand(object sender, EventArgs e)
    {
 
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;        
    }
}
