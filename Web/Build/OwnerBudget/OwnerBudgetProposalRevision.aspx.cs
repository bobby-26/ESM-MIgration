using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.OwnerBudget;
using Telerik.Web.UI;

public partial class OwnerBudgetProposalRevision : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Report", "REPORT", ToolBarDirection.Right);
        toolbar.AddButton("Technical", "TECHNICAL", ToolBarDirection.Right);
        toolbar.AddButton("Luboil", "LUBOIL", ToolBarDirection.Right);
        toolbar.AddButton("Crew Expense", "EXPENSE", ToolBarDirection.Right);
        toolbar.AddButton("Crew Wages", "CREWWAGE", ToolBarDirection.Right);
        toolbar.AddButton("Particulars", "PARTICULARS", ToolBarDirection.Right);
        toolbar.AddButton("Revisions", "REVISION", ToolBarDirection.Right);
        toolbar.AddButton("Proposals", "PROPOSALS", ToolBarDirection.Right);

        MenuProposalRevision.AccessRights = this.ViewState;
        MenuProposalRevision.MenuList = toolbar.Show();
        if (!IsPostBack)

        {
        
          
            MenuProposalRevision.SelectedMenuIndex = 6;
            ViewState["REVISIONID"] = string.Empty;
            ViewState["PROPOSALID"] = string.Empty;

            if (Request.QueryString["proposalid"] != null)
            {
                ViewState["PROPOSALID"] = Request.QueryString["proposalid"].ToString();
            }

            if (Request.QueryString["revisionid"] != null)
            {
                ViewState["REVISIONID"] = Request.QueryString["revisionid"].ToString();
                ifMoreInfo.Attributes["src"] = "../OwnerBudget/OwnerBudgetProposalAddEdit.aspx?proposalid=" + ViewState["PROPOSALID"];
            }           
        }
        BindData();
    }

    private void BindData()
    {
        DataTable dt = new DataTable();
        dt = PhoenixOwnerBudget.BudgetProposalList(new Guid(ViewState["PROPOSALID"].ToString()));

            gvBudgetProposal.DataSource = dt;
            gvBudgetProposal.DataBind();
    }

    protected void MenuProposalRevision_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("PROPOSALS"))
        {
            Response.Redirect("OwnerBudgetProposal.aspx", false);
        }
        if (CommandName.ToUpper() == "PARTICULARS")
        {
            Response.Redirect("OwnerBudgetVesselParticulars.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
        }
        if (CommandName.ToUpper().Equals("CREWWAGE"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetProposedCrewWages.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
        }
        if (CommandName.ToUpper().Equals("TECHNICAL"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetTechnicalProposal.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
        }
        if (CommandName.ToUpper().Equals("EXPENSE"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetCrewExpense.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
        }
        if (CommandName.ToUpper().Equals("REPORT"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetExpenseReport.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
        }
        if (CommandName.ToUpper().Equals("LUBOIL"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetLubOil.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
        }
        if (CommandName.ToUpper().Equals("FINAL"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetExpenseProposalReport.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void gvBudgetProposal_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            
            if (e.CommandName.ToUpper() == "ADD")
            {
                ViewState["PROPOSALID"] = ((RadLabel)e.Item.FindControl("lblProposalId")).Text;
                if ((ViewState["PROPOSALID"] != null) && (ViewState["REVISIONID"] != null))
                    AddRevision(ViewState["PROPOSALID"].ToString());
            }

            if (e.CommandName.ToUpper() == "APPROVE")
            {
                ViewState["PROPOSALID"] = ((RadLabel)e.Item.FindControl("lblProposalId")).Text;
                if ((ViewState["PROPOSALID"] != null))
                    PhoenixOwnerBudget.ProposalRevisionApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , new Guid(((RadLabel)e.Item.FindControl("lblProposalId")).Text)
                                                          );              
            }


            if (e.CommandName.ToUpper() == "EDIT")
            {
                ViewState["PROPOSALID"] = ((RadLabel)e.Item.FindControl("lblProposalId")).Text;
                ifMoreInfo.Attributes["src"] = "../OwnerBudget/OwnerBudgetProposalAddEdit.aspx?proposalid=" + ViewState["PROPOSALID"].ToString();         
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixOwnerBudget.ProposalRevisionDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                           , new Guid(((RadLabel)e.Item.FindControl("lblProposalId")).Text)
                                                          );              
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void AddRevision(string sourceproposalid)
    {
        PhoenixOwnerBudget.ProposalRevisionInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                              , new Guid(sourceproposalid)
                                                              );
    }

    protected void gvBudgetProposal_DeleteCommand(object sender, GridCommandEventArgs de)
    {
        BindData();
    }

    protected void gvBudgetProposal_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
       if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            int? n = General.GetNullableInteger(drv["FLDAPPROVALSTATUS"].ToString());
            LinkButton cmdApprove = (LinkButton)e.Item.FindControl("cmdApprove");
            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdApprove != null && n == 1)
            {
                cmdApprove.Visible = false;
                if (cmdAdd != null)
                    cmdAdd.Visible = true;
            }
            else
            {
                if (cmdAdd != null)
                    cmdAdd.Visible = false;
            }

        }
    }

   
    protected void gvBudgetProposal_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBudgetProposal.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
