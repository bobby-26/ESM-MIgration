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


public partial class OwnerBudgetProposedCrewWages : PhoenixBasePage
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

            MenuCrewWages.AccessRights = this.ViewState;
            MenuCrewWages.MenuList = toolbar.Show();
            MenuCrewWages.SelectedMenuIndex = 4;
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Show", "SHOW",ToolBarDirection.Right);
            MenuShowCrewWages.AccessRights = this.ViewState;
            MenuShowCrewWages.MenuList = toolbarmain.Show();
            MenuShowCrewWages.AccessRights = this.ViewState;

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
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
    }

    private void BindData()
    {
        string[] alColumns = { "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY" };
        string[] alCaptions = { "Crew", "Nos", "Crew Wages Per Month", "Current Wages Per Day", "Proposed Wages Per Month", "Proposed Wages Per Day" };

        DataSet ds = PhoenixOwnerBudget.OwnerBudgetVessel(new Guid(ViewState["PROPOSALID"].ToString()));

        General.SetPrintOptions("gvCrewWages", "CrewWages", alCaptions, alColumns, ds);
            gvCrewWages.DataSource = ds;
            gvCrewWages.DataBind();
            
      
        if (ds.Tables[1].Rows.Count > 0)
        {
            span1.InnerHtml = @"<table width=""100%"">";
            span1.InnerHtml = span1.InnerHtml + @"<tr>
                                          <td align=""left"" width=""20%"" ><h2>Allowance</h2></td>
                                          <td align=""right"" width=""20%"" ><h2>Current Costs Per Month</h2></td>
                                          <td align=""right"" width=""20%""><h2>Current Costs Per Day</h2></td>
                                          <td align=""right"" width=""20%"" ><h2>Proposed Costs Per Month</h2></td>
                                          <td align=""right"" width=""20%""><h2>Proposed Costs Per Day</h2></td>                          
                                                    </tr>";
            foreach (DataRow dr in ds.Tables[1].Rows)
            {

                span1.InnerHtml = span1.InnerHtml + @"<tr>
                                          <td align=""left"" width=""20%"" ><h3>" + HttpUtility.HtmlDecode(dr["FLDHARDNAME"].ToString()) + @"</h3></td>
                                          <td align=""right"" width=""20%"" ><h3>" + HttpUtility.HtmlDecode(dr["FLDCURRENTAMOUNTPERMONTH"].ToString()) + @"</h3></td>
                                          <td align=""right"" width=""20%""><h3>" + HttpUtility.HtmlDecode(dr["FLDCURRENTAMOUNTPERDAY"].ToString()) + @"</h3></td>            
                                          <td align=""right"" width=""20%"" ><h3>" + HttpUtility.HtmlDecode(dr["FLDPROPOSEDAMOUNTPERMONTH"].ToString()) + @"</h3></td>
                                          <td align=""right"" width=""20%""><h3>" + HttpUtility.HtmlDecode(dr["FLDPROPOSEDAMOUNTPERDAY"].ToString()) + @"</h3></td>            
                                                    </tr>";



            }
            foreach (DataRow dr2 in ds.Tables[2].Rows)
            {

                span1.InnerHtml = span1.InnerHtml + @"<tr>
                                          <td align=""left"" width=""20%"" ><h3>" + HttpUtility.HtmlDecode(dr2["FLDHARDNAME"].ToString()) + @"</h3></td>
                                          <td align=""right"" width=""20%"" ><h3>" + HttpUtility.HtmlDecode(dr2["FLDCURRENTAMOUNTPERMONTH"].ToString()) + @"</h3></td>
                                          <td align=""right"" width=""20%""><h3>" + HttpUtility.HtmlDecode(dr2["FLDCURRENTAMOUNTPERDAY"].ToString()) + @"</h3></td>            
                                          <td align=""right"" width=""20%"" ><h3>" + HttpUtility.HtmlDecode(dr2["FLDPROPOSEDAMOUNTPERMONTH"].ToString()) + @"</h3></td>
                                          <td align=""right"" width=""20%""><h3>" + HttpUtility.HtmlDecode(dr2["FLDPROPOSEDAMOUNTPERDAY"].ToString()) + @"</h3></td>            
                                                    </tr>";



            }
            span1.InnerHtml = span1.InnerHtml + @"</table>";


        }
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY" };
        string[] alCaptions = { "Crew", "Nos", "Crew Wages Per Month", "Current Wages Per Day", "Proposed Wages Per Month", "Proposed Wages Per Day" };

        ds = PhoenixOwnerBudget.OwnerBudgetVessel(new Guid(ViewState["REVISIONID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewWages.xls");
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

    protected void MenuCrewWages_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("PROPOSALS"))
        {
            Response.Redirect("OwnerBudgetProposal.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
        }
        if (CommandName.ToUpper().Equals("REVISIONS"))
        {
            Response.Redirect("OwnerBudgetProposalRevision.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
        }
        if (CommandName.ToUpper().Equals("PARTICULARS"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetVesselParticulars.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
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
    }

    protected void gvCrewWages_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if(e.Item is GridHeaderItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        if(e.Item is GridEditableItem)
        {
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            e.Item.Font.Bold = false;
            if (General.GetNullableGuid(DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString()) == null)
            {
                RadLabel lblCrew = (RadLabel)e.Item.FindControl("lblCrew");
                if (General.GetNullableInteger(DataBinder.Eval(e.Item.DataItem, "FLDLEVEL").ToString()) == null && lblCrew != null)
                {
                    lblCrew.Text = "Grand Total";
                }
                else
                {
                    lblCrew.Text = "Sub Total";
                }
                e.Item.Font.Bold = true;
                LinkButton dbDelete = (LinkButton)e.Item.FindControl("cmdDelete");
                LinkButton dbEdit  = (LinkButton)e.Item.FindControl("cmdEdit");
                if (dbDelete != null)
                    dbDelete.Visible = false;
                if (dbEdit != null)
                    dbEdit.Visible = false;
            }
            
        }
    }

    protected void gvCrewWages_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
        }
    }

    protected void gvCrewWages_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

          
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string rank = ((UserControlGroupRank)e.Item.FindControl("ddlRankAdd")).SelectedRank;
                string ownerscale = ((UserControlMaskNumber)e.Item.FindControl("ucOwnerScaleAdd")).Text;
                string currentwage = ((UserControlMaskNumber)e.Item.FindControl("ucCurrentWagePerMonthAdd")).Text;
                string proposedwage = ((UserControlMaskNumber)e.Item.FindControl("ucProposedWagePerMonthAdd")).Text;

                if (!IsValidWages(rank,ownerscale,currentwage, proposedwage))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixOwnerBudget.BudgetCrewWageAdd(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                       new Guid(ViewState["PROPOSALID"].ToString()),
                                                       General.GetNullableInteger(rank),
                                                       General.GetNullableInteger(ownerscale),
                                                       General.GetNullableDecimal(currentwage),
                                                       General.GetNullableDecimal(proposedwage)
                                                     );
                BindData();
                ((UserControlGroupRank)e.Item.FindControl("ddlRankAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixOwnerBudget.BudgetCrewWageDelete((PhoenixSecurityContext.CurrentSecurityContext.UserCode), new Guid(((RadLabel)e.Item.FindControl("lblDTKey")).Text));
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewWages_DeleteCommand(object sender, GridCommandEventArgs de)
    {
        BindData();
    }

   
    protected void gvCrewWages_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;


            string rank = ((RadLabel)e.Item.FindControl("lblRankid")).Text;
            string ownerscale = ((UserControlMaskNumber)e.Item.FindControl("ucOwnerScaleEdit")).Text;
            string currentwage = ((UserControlMaskNumber)e.Item.FindControl("ucCurrentWagePerMonth")).Text;
            string proposedwage = ((UserControlMaskNumber)e.Item.FindControl("ucProposedWagePerMonth")).Text;

            if (!IsValidWages(rank,ownerscale,currentwage, proposedwage))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixOwnerBudget.BudgetCrewWageUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                       new Guid(((RadLabel)e.Item.FindControl("lblDTKey")).Text.ToString()),
                                                       General.GetNullableInteger(rank),
                                                       General.GetNullableInteger(ownerscale),
                                                       General.GetNullableDecimal(currentwage),
                                                       General.GetNullableDecimal(proposedwage)
                                                     );
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   

    protected void gvCrewWages_SortCommand(object sender, GridSortCommandEventArgs e)
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

   

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private bool IsValidWages(string rank,string ownerscale,string currentwage, string proposedwage)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableInteger(rank).HasValue)
            ucError.ErrorMessage = "Rank is required.";

        if (!General.GetNullableDecimal(ownerscale).HasValue)
            ucError.ErrorMessage = "Owner scale is required";

        if (!General.GetNullableDecimal(currentwage).HasValue)
            ucError.ErrorMessage = "Current wage is required";

        if (!General.GetNullableDecimal(proposedwage).HasValue)
            ucError.ErrorMessage = "Proposed wage is required";

        return (!ucError.IsError);
    }
    protected void MenuShowCrewWages_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SHOW"))
        {
            if (ViewState["PROPOSALID"] != null && ViewState["PROPOSALID"].ToString()!="")
            {
                PhoenixOwnerBudget.CrewWagesInsert(General.GetNullableGuid(ViewState["PROPOSALID"].ToString()));
            }
            BindData();
        }
    }

    protected void gvCrewWages_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewWages.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}

