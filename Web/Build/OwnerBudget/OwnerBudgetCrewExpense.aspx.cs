using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.OwnerBudget;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class OwnerBudgetCrewExpense : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Report", "REPORT", ToolBarDirection.Right);
            toolbar.AddButton("Technical", "TECHNICAL", ToolBarDirection.Right);
            toolbar.AddButton("Lub Oil", "LUBOIL", ToolBarDirection.Right);
            toolbar.AddButton("Crew Expense", "EXPENSE", ToolBarDirection.Right);
            toolbar.AddButton("Crew Wages", "CREWWAGE", ToolBarDirection.Right);
            toolbar.AddButton("Particulars", "PARTICULARS", ToolBarDirection.Right);
            toolbar.AddButton("Revisions", "REVISIONS", ToolBarDirection.Right);
            toolbar.AddButton("Proposals", "PROPOSALS", ToolBarDirection.Right);


            MenuCrewExpense.AccessRights = this.ViewState;
            MenuCrewExpense.MenuList = toolbar.Show();
            MenuCrewExpense.SelectedMenuIndex = 3;

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Show", "SHOW", ToolBarDirection.Right);
            MenuShowExpenses.AccessRights = this.ViewState;
            MenuShowExpenses.MenuList = toolbarmain.Show();


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
            BindDataCrewExpense();
            BindDataTravelExpense();
            BindDataOtherCrewExpense();
            BindDataOtherExpenses();
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
        BindDataCrewExpense();
    }

    protected void MenuCrewExpense_TabStripCommand(object sender, EventArgs e)
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
        if (CommandName.ToUpper().Equals("TECHNICAL"))
        {
            Response.Redirect("../OwnerBudget/OwnerBudgetTechnicalProposal.aspx?proposalid=" + ViewState["PROPOSALID"] + "&revisionid=" + ViewState["REVISIONID"], false);
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

    protected void MenuShowExpenses_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOW"))
            {
                if (ViewState["PROPOSALID"] != null)
                    PhoenixOwnerBudget.OwnerBudgetCrewExpenseInsert(General.GetNullableGuid(ViewState["PROPOSALID"].ToString()));
            }
            BindDataCrewExpense();
            BindDataTravelExpense();
            BindDataOtherCrewExpense();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY" };
        string[] alCaptions = { "Crew", "Nos", "Crew Wages Per Month", "Current Wages Per Day", "Proposed Wages Per Month", "Proposed Wages Per Day" };


        ds = PhoenixOwnerBudget.OwnerBudgetExpenseEdit(new Guid(ViewState["REVISIONID"].ToString()), 1);

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewExpense.xls");
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

    private void BindDataCrewExpense()
    {
        string[] alColumns = { "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY" };
        string[] alCaptions = { "Crew", "Nos", "Crew Wages Per Month", "Current Wages Per Day", "Proposed Wages Per Month", "Proposed Wages Per Day" };



        DataSet ds = PhoenixOwnerBudget.OwnerBudgetExpenseEdit(new Guid(ViewState["PROPOSALID"].ToString()), 112);

        General.SetPrintOptions("gvCrewExpense", "CrewExpense", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {

            //  gvCrewExpense.DataBind();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Refresh", "SHOW", ToolBarDirection.Right);
            MenuShowExpenses.AccessRights = this.ViewState;
            MenuShowExpenses.MenuList = toolbarmain.Show();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            // ShowNoRecordsFound(dt, gvCrewExpense);
        }
        gvCrewExpense.DataSource = ds;

    }

    private void BindDataTravelExpense()
    {
        string[] alColumns = { "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY" };
        string[] alCaptions = { "Crew", "Nos", "Crew Wages Per Month", "Current Wages Per Day", "Proposed Wages Per Month", "Proposed Wages Per Day" };



        DataSet ds = PhoenixOwnerBudgetTravelCost.OwnerBudgetTravelCostList(new Guid(ViewState["PROPOSALID"].ToString()));

        General.SetPrintOptions("gvTravelExpense", "TravelExpense", alCaptions, alColumns, ds);

        gvTravelExpense.DataSource = ds;

    }

    private void BindDataOtherCrewExpense()
    {
        string[] alColumns = { "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY", "FLDDTKEY" };
        string[] alCaptions = { "Crew", "Nos", "Crew Wages Per Month", "Current Wages Per Day", "Proposed Wages Per Month", "Proposed Wages Per Day" };



        DataSet ds = PhoenixOwnerBudget.OwnerBudgetExpenseEdit(new Guid(ViewState["PROPOSALID"].ToString()), 113);

        General.SetPrintOptions("gvCrewExpense", "CrewExpense", alCaptions, alColumns, ds);

        gvOtherCrewExpense.DataSource = ds;
    }

    protected void gvCrewExpense_Sorting(object sender, GridViewSortEventArgs se)
    {

        BindDataCrewExpense();
    }

    protected void gvCrewExpense_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCrewExpense, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvCrewExpense_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    string categoryid = ((UserControlExpenseType)e.Item.FindControl("ucCategoryAdd")).SelectedExpenseType;
                    string officertypeid = ((UserControlHard)e.Item.FindControl("ucOfficerTypeAdd")).SelectedHard;
                    if (!IsValidExpense(categoryid, officertypeid))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixOwnerBudget.OwnerBudgetExpenseAdd(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                           new Guid(ViewState["PROPOSALID"].ToString()),
                                                           General.GetNullableGuid(categoryid),
                                                           General.GetNullableInteger(officertypeid),
                                                           General.GetNullableInteger("1")
                                                         );
                    BindDataCrewExpense();
                    gvCrewExpense.Rebind();

                }
                else if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    PhoenixOwnerBudget.OwnerBudgetExpenseDelete((PhoenixSecurityContext.CurrentSecurityContext.UserCode), new Guid(((RadLabel)e.Item.FindControl("lblDTKey")).Text));
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewExpense_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindDataCrewExpense();
    }

    protected void gvCrewExpense_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            string contractperiod = (((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucContractPeriodEdit")).Text);
            string manrequired = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucManRequiredEdit")).Text;

            PhoenixOwnerBudget.OwnerBudgetExpenseUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDTkey")).Text)
                                                            , Int32.Parse(contractperiod)
                                                            , Int32.Parse(manrequired)
                                                            );
            _gridView.EditIndex = -1;
            BindDataCrewExpense();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewExpense_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            if (General.GetNullableGuid(DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString()) == null)
            {
                RadLabel lblExpense = (RadLabel)e.Item.FindControl("lblExpense");
                if (General.GetNullableGuid(DataBinder.Eval(e.Item.DataItem, "FLDCATEGORYID").ToString()) == null && lblExpense != null)
                {
                    lblExpense.Text = "Grand Total";
                }
                else
                {
                    lblExpense.Text = "Sub Total";
                }
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                    db.Visible = false;
                ImageButton dbEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (dbEdit != null)
                    dbEdit.Visible = false;
                e.Item.Font.Bold = true;
                //e.Row.Attributes.Add("style", "f:pointer;");
            }
        }
        if (e.Item is GridFooterItem)
        {
            UserControlExpenseType ucExpenseType = (UserControlExpenseType)e.Item.FindControl("ucCategoryAdd");
            if (ucExpenseType != null)
            {
                //ucExpenseType.ExpenseTypeList = PhoenixOwnerBudget.OwnerBudgetExpenseTypeList(1, null);
                ucExpenseType.ExpenseType = "1";
                ucExpenseType.bind();
            }
            UserControlHard ucOfficerType = (UserControlHard)e.Item.FindControl("ucOfficerTypeAdd");
            if (ucOfficerType != null)
            {
                ucOfficerType.HardTypeCode = "90";
                ucOfficerType.ShortNameFilter = "SOF,JOF,TRA,RAT";
                ucOfficerType.bind();
            }

        }
    }

    protected void gvTravelExpense_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            if (General.GetNullableGuid(DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString()) == null)
            {
                //RadLabel lblExpense = (RadLabel)e.Row.FindControl("lblExpense");
                //if (General.GetNullableGuid(DataBinder.Eval(e.Row.DataItem, "FLDCREWAIRFAREID").ToString()) == null && lblExpense != null)
                //{
                //    lblExpense.Text = "Grand Total";
                //}
                //else
                //{
                //    lblExpense.Text = "Sub Total";
                //}
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                    db.Visible = false;
                ImageButton dbEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (dbEdit != null)
                    dbEdit.Visible = false;
                e.Item.Font.Bold = true;
                //e.Row.Attributes.Add("style", "f:pointer;");
            }
        }
        if (e.Item is GridFooterItem)
        {
            UserControlExpenseType ucExpenseType = (UserControlExpenseType)e.Item.FindControl("ucCategoryAdd");
            if (ucExpenseType != null)
            {
                //ucExpenseType.ExpenseTypeList = PhoenixOwnerBudget.OwnerBudgetExpenseTypeList(2, null);
                ucExpenseType.ExpenseType = "2";
                ucExpenseType.bind();
            }
            UserControlHard ucOfficerType = (UserControlHard)e.Item.FindControl("ucOfficerTypeAdd");
            if (ucOfficerType != null)
            {
                ucOfficerType.HardTypeCode = "90";
                ucOfficerType.ShortNameFilter = "SOF,JOF,TRA,RAT";
                ucOfficerType.bind();
            }
        }
    }

    protected void gvTravelExpense_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            string contractperiod = (((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucContractPeriodEdit")).Text);
            string manrequired = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucManRequiredEdit")).Text;

            PhoenixOwnerBudgetTravelCost.OwnerBudgetTravelCostUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDTkey")).Text)
                                                            , Int32.Parse(contractperiod)
                                                            , Int32.Parse(manrequired)
                                                            );
            _gridView.EditIndex = -1;
            BindDataTravelExpense();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelExpense_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvTravelExpense, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvTravelExpense_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                if (e.CommandName.ToUpper().Equals("ADD"))
                {
                    string categoryid = ((UserControlExpenseType)e.Item.FindControl("ucCategoryAdd")).SelectedExpenseType;
                    string officertypeid = ((UserControlHard)e.Item.FindControl("ucOfficerTypeAdd")).SelectedHard;

                    if (!IsValidExpense(categoryid, officertypeid))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    PhoenixOwnerBudget.OwnerBudgetExpenseAdd(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                           new Guid(ViewState["PROPOSALID"].ToString()),
                                                           General.GetNullableGuid(categoryid),
                                                           General.GetNullableInteger(officertypeid),
                                                           General.GetNullableInteger("2")
                                                         );
                    BindDataTravelExpense();

                }
                else if (e.CommandName.ToUpper().Equals("DELETE"))
                {
                    PhoenixOwnerBudget.OwnerBudgetExpenseDelete((PhoenixSecurityContext.CurrentSecurityContext.UserCode), new Guid(((RadLabel)e.Item.FindControl("lblDTKey")).Text));
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravelExpense_Sorting(object sender, GridViewSortEventArgs se)
    {
        BindDataTravelExpense();
    }

    protected void gvTravelExpense_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindDataTravelExpense();
    }

    protected void gvOtherCrewExpense_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            if (General.GetNullableGuid(DataBinder.Eval(e.Item.DataItem, "FLDDTKEY").ToString()) == null)
            {
                RadLabel lblExpense = (RadLabel)e.Item.FindControl("lblExpense");
                if (General.GetNullableGuid(DataBinder.Eval(e.Item.DataItem, "FLDCATEGORYID").ToString()) == null && lblExpense != null)
                {
                    lblExpense.Text = "Grand Total";
                }
                else
                {
                    lblExpense.Text = "Sub Total";
                }
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                    db.Visible = false;
                ImageButton dbEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (dbEdit != null)
                    dbEdit.Visible = false;
                e.Item.Font.Bold = true;
                //e.Row.Attributes.Add("style", "f:pointer;");
            }
        }
        if (e.Item is GridFooterItem)
        {
            UserControlExpenseType ucExpenseType = (UserControlExpenseType)e.Item.FindControl("ucCategoryAdd");
            if (ucExpenseType != null)
            {
                //ucExpenseType.ExpenseTypeList = PhoenixOwnerBudget.OwnerBudgetExpenseTypeList(3, null);
                ucExpenseType.ExpenseType = "3";
                ucExpenseType.bind();
            }
            UserControlHard ucOfficerType = (UserControlHard)e.Item.FindControl("ucOfficerTypeAdd");
            if (ucOfficerType != null)
            {
                ucOfficerType.HardTypeCode = "90";
                ucOfficerType.ShortNameFilter = "SOF,JOF,TRA,RAT";
                ucOfficerType.bind();
            }
        }
    }

    protected void gvOtherCrewExpense_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindDataOtherCrewExpense();
    }

    protected void gvOtherCrewExpense_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            string contractperiod = (((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucContractPeriodEdit")).Text);
            string manrequired = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("ucManRequiredEdit")).Text;

            PhoenixOwnerBudget.OwnerBudgetExpenseUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , new Guid(((RadLabel)_gridView.Rows[nCurrentRow].FindControl("lblDTkey")).Text)
                                                            , Int32.Parse(contractperiod)
                                                            , Int32.Parse(manrequired)
                                                            );
            _gridView.EditIndex = -1;
            BindDataOtherCrewExpense();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOtherCrewExpense_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            // e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvOtherCrewExpense, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvOtherCrewExpense_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string categoryid = ((UserControlExpenseType)e.Item.FindControl("ucCategoryAdd")).SelectedExpenseType;
                string officertypeid = ((UserControlHard)e.Item.FindControl("ucOfficerTypeAdd")).SelectedHard;

                if (!IsValidExpense(categoryid, officertypeid))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixOwnerBudget.OwnerBudgetExpenseAdd(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                       new Guid(ViewState["PROPOSALID"].ToString()),
                                                       General.GetNullableGuid(categoryid),
                                                       General.GetNullableInteger(officertypeid),
                                                       General.GetNullableInteger("3")
                                                       );
                BindDataOtherCrewExpense();

            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixOwnerBudget.OwnerBudgetExpenseDelete((PhoenixSecurityContext.CurrentSecurityContext.UserCode), new Guid(((RadLabel)e.Item.FindControl("lblDTKey")).Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOtherCrewExpense_Sorting(object sender, GridViewSortEventArgs se)
    {
        BindDataOtherCrewExpense();
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private bool IsValidExpense(string expensetypeid, string officertypeid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(expensetypeid) == null)
            ucError.ErrorMessage = "Expense Type is required.";

        if (General.GetNullableInteger(officertypeid) == null)
            ucError.ErrorMessage = "Officer Type is required.";

        return (!ucError.IsError);
    }

    protected void gvOtherExpenses_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            if (General.GetNullableString(DataBinder.Eval(e.Item.DataItem, "FLDLEVEL").ToString()) == null)
            {
                RadLabel lblComponent = (RadLabel)e.Item.FindControl("lblComponent");
                if (General.GetNullableString(DataBinder.Eval(e.Item.DataItem, "FLDEXPENSES").ToString()) == null && lblComponent != null)
                {
                    //lblComponent.Text = "Grand Total";
                }
                else
                {
                    //lblComponent.Text = "Sub Total";
                }
                e.Item.Font.Bold = true;
            }
        }
    }

    private void BindDataOtherExpenses()
    {


        DataSet ds = PhoenixOwnerBudget.OwnerBudgetOtherExpenses(new Guid(ViewState["PROPOSALID"].ToString()));

        gvOtherExpenses.DataSource = ds;
    }

    protected void gvCrewExpense_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataCrewExpense();
    }

    protected void gvOtherExpenses_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataOtherExpenses();
    }

    protected void gvOtherCrewExpense_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataOtherCrewExpense();
    }

    protected void gvTravelExpense_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataTravelExpense();
    }
}
