using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.OwnerBudget;
using Telerik.Web.UI;


public partial class OwnerBudgetTechnicalProposal : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
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

            MenuTechnicalProposal.AccessRights = this.ViewState;
            MenuTechnicalProposal.MenuList = toolbar.Show();
            MenuTechnicalProposal.SelectedMenuIndex = 1;

            SessionUtil.PageAccessRights(this.ViewState);
          
        //    cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            ViewState["BudgetGroup"] = null;

            if (!IsPostBack)
            {
                if (Request.QueryString["proposalid"] != null)
                {
                    ViewState["PROPOSALID"] = Request.QueryString["proposalid"].ToString();
                }
                if (Request.QueryString["revisionid"] != null)
                {
                    ViewState["REVISIONID"] = Request.QueryString["revisionid"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void MenuTechnicalProposal_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("PROPOSALS"))
        {
            Response.Redirect("OwnerBudgetProposal.aspx");
        }
        if (CommandName.ToUpper().Equals("REVISIONS"))
        {
            Response.Redirect("OwnerBudgetProposalRevision.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"]);
        }
        if (CommandName.ToUpper().Equals("CREWWAGE"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetProposedCrewWages.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"]);
        }
        if (CommandName.ToUpper().Equals("PARTICULARS"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetVesselParticulars.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"]);
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
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY" };
        string[] alCaptions = { "Crew", "Nos", "Crew Wages Per Month", "Current Wages Per Day", "Proposed Wages Per Month", "Proposed Wages Per Day" };


        ds = PhoenixOwnerBudget.OwnerBudgetTechnicalProposalEdit(new Guid(ViewState["REVISIONID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=TechnicalProposal.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Country Register</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    private void BindData()
    {
        string[] alColumns = { "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY" };
        string[] alCaptions = { "Crew", "Nos", "Crew Wages Per Month", "Current Wages Per Day", "Proposed Wages Per Month", "Proposed Wages Per Day" };



        DataSet ds = PhoenixOwnerBudget.OwnerBudgetTechnicalProposalEdit(new Guid(ViewState["PROPOSALID"].ToString()));

        General.SetPrintOptions("gvTechnicalProposal", "TechnicalProposal", alCaptions, alColumns, ds);
       
        gvTechnicalProposal.DataSource = ds;
        gvTechnicalProposal.VirtualItemCount = ds.Tables[0].Rows.Count;
       
    }

   
    protected void gvTechnicalProposal_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void gvTechnicalProposal_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string budgetgroup = ((UserControlHard)e.Item.FindControl("ucBudgetGroup")).SelectedHard;
                string budgetid = ((RadTextBox)e.Item.FindControl("txtBudgetId")).Text;
                string currentbudget = "0.00";
                string proposedbudget = ((UserControlMaskNumber)e.Item.FindControl("ucProposedBudgetPerYearAdd")).Text;
                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarksAdd")).Text;
               // gvTechnicalProposal.Rebind();

                if (!IsValidBudget(budgetgroup, budgetid, currentbudget, proposedbudget))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixOwnerBudget.OwnerBudgetTechnicalProposalAdd(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                       new Guid(ViewState["PROPOSALID"].ToString()),
                                                       General.GetNullableInteger(budgetid),
                                                       General.GetNullableInteger(budgetgroup),
                                                       General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucCurrentBudgetPerYearAdd")).Text),
                                                       General.GetNullableDecimal(proposedbudget),
                                                       remarks
                                                     );


            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixOwnerBudget.OwnerBudgetTechnicalProposalDelete((PhoenixSecurityContext.CurrentSecurityContext.UserCode), new Guid(((RadLabel)e.Item.FindControl("lblDTKey")).Text));
            }
            BindData();
            //gvTechnicalProposal.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
 
    protected void gvTechnicalProposal_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;

            string budgetgroup = ((RadLabel)e.Item.FindControl("lblBudgetGroupId")).Text;
            string budgetid = ((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text;
            string currentbudget = "0.00";
            string proposedbudget = ((UserControlMaskNumber)e.Item.FindControl("ucProposedBudgetPerYear")).Text;

            if (!IsValidBudget(budgetgroup, budgetid, currentbudget, proposedbudget))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixOwnerBudget.OwnerBudgetTechnicalProposalUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                       new Guid(((RadLabel)e.Item.FindControl("lblDTKey")).Text.ToString()),
                                                       int.Parse(((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text),
                                                       General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucCurrentBudgetPerYear")).Text),
                                                       General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucProposedBudgetPerYear")).Text),
                                                       ((RadTextBox)e.Item.FindControl("txtRemarks")).Text
                                                     );
            BindData();
            gvTechnicalProposal.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTechnicalProposal_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridFooterItem)
        {
            LinkButton ImgShowBudgetAdd = (LinkButton)e.Item.FindControl("ImgShowBudgetAdd");
            RadTextBox txtBudgetId = (RadTextBox)e.Item.FindControl("txtBudgetId");
            RadTextBox txtBudgetgroupId = (RadTextBox)e.Item.FindControl("txtBudgetgroupId");
            string currentbudget = "";
            if (ImgShowBudgetAdd != null)
            {
                ImgShowBudgetAdd.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '','../Common/CommonPickListBudget.aspx?budgetgroup=" + currentbudget + "&hardtypecode=30', true); ");
            }
            txtBudgetId.Attributes.Add("style", "visibility:hidden");
            txtBudgetgroupId.Attributes.Add("style", "visibility:hidden");

            UserControlHard ucBudgetGroup = ((UserControlHard)e.Item.FindControl("ucBudgetGroup"));
            if (ucBudgetGroup != null)
            {
                ucBudgetGroup.ShortNameFilter = "66,69,72,75,78,81,84,85,90,93,10";
                ucBudgetGroup.bind();
                if (ViewState["BudgetGroup"] != null )
                {
                    ucBudgetGroup.SelectedHard = ViewState["BudgetGroup"].ToString();
                }
            }

        }
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            if (General.GetNullableInteger(DataBinder.Eval(e.Item.DataItem, "FLDBUDGETID").ToString()) == null)
            {
                db.Visible = false;
                LinkButton dbEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                dbEdit.Visible = false;
                e.Item.Font.Bold = true;
                e.Item.Attributes.Add("style", "f:pointer;");
            }

            LinkButton ImgShowBudgetEdit = (LinkButton)e.Item.FindControl("ImgShowBudgetEdit");
            RadLabel lblBudgetGroupId = (RadLabel)e.Item.FindControl("lblBudgetGroupId");
            if (ImgShowBudgetEdit != null && lblBudgetGroupId != null)
            {
                ImgShowBudgetEdit.Attributes.Add("onclick", "return showPickList('spnPickListMainBudgetEdit', 'codehelp1', '','../Common/CommonPickListBudget.aspx?budgetgroup=" + lblBudgetGroupId.Text + "&hardtypecode=30', true); ");
            }
        }

    }
 
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private bool IsValidBudget(string budgetgroup, string budgetid, string currentwage, string proposedwage)
    {
        ucError.HeaderMessage = "Please provide the following required information";

     

        RadGrid _gridView = gvTechnicalProposal;
        Int16 result;
        if (budgetgroup.Trim().Equals("")||General.GetNullableInteger(budgetgroup)==null)
            ucError.ErrorMessage = "Budget group is required.";

        if (currentwage.Trim().Equals(""))
            ucError.ErrorMessage = "Current budget is required";

        if (proposedwage.Trim().Equals(""))
            ucError.ErrorMessage = "Proposed budget is required";

        if (budgetid == null || !Int16.TryParse(budgetid, out result))
            ucError.ErrorMessage = "Country is required.";
        return (!ucError.IsError);
    }

    protected void ucBudgetGroupAdd_Changed(object sender, EventArgs e)
    {
        UserControlHard ucBudgetGroup = (UserControlHard)sender;
        GridFooterItem gv = (GridFooterItem)ucBudgetGroup.Parent.Parent;

        RadTextBox txtBudgetCode = (RadTextBox)gv.FindControl("txtBudgetCode");
        RadTextBox txtBudgetName = (RadTextBox)gv.FindControl("txtBudgetName");
        RadTextBox txtBudgetId = (RadTextBox)gv.FindControl("txtBudgetId");
        RadTextBox txtBudgetgroupId = (RadTextBox)gv.FindControl("txtBudgetgroupId");

        ViewState["BudgetGroup"] = ucBudgetGroup.SelectedHard;
        txtBudgetCode.Text = txtBudgetgroupId.Text = txtBudgetId.Text = txtBudgetName.Text = "";

        LinkButton ImgShowBudgetAdd = (LinkButton)gv.FindControl("ImgShowBudgetAdd");
        if (ImgShowBudgetAdd != null)
            ImgShowBudgetAdd.Attributes.Add("onclick", "return showPickList('spnPickListMainBudget', 'codehelp1', '','../Common/CommonPickListBudget.aspx?budgetgroup=" + ucBudgetGroup.SelectedHard + "&hardtypecode=30', true); ");
    }

    protected void gvTechnicalProposal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
