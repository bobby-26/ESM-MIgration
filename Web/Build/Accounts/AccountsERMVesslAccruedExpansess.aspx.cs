using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsERMVesslAccruedExpansess : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvERMVesselAccruedExpensess.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvERMVesselAccruedExpensess.UniqueID, "Edit$" + r.RowIndex.ToString());
    //        }
    //    }
    //
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsERMVesslAccruedExpansess.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvERMVesselAccruedExpensess')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("../Accounts/AccountsERMVesslAccruedExpansess.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Accounts/AccountsERMVesslAccruedExpansess.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuERMOtherSubAccount.AccessRights = this.ViewState;
            MenuERMOtherSubAccount.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvERMVesselAccruedExpensess.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ERMOtherSubAccount_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtaccountid.Text = string.Empty;
                txtcode.Text = string.Empty;
                txtAccountdescription.Text = string.Empty;
                txtBudgetId.Text = string.Empty;
                txtXAccount.Text = string.Empty;
                txtXAccountDescription.Text = string.Empty;
                Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvERMVesselAccruedExpensess_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                {
                    PhoenixAccountsERMVesselAccruedExpensess.ERMVesselAccruedExpensessInsert((((RadTextBox)e.Item.FindControl("txtlAccountIdAdd")).Text.Trim()),
                                                            (((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text.Trim()),
                                                            (((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text.Trim()),
                                                            (((RadTextBox)e.Item.FindControl("txtBudgetIdadd")).Text.Trim()),
                                                            (((RadTextBox)e.Item.FindControl("txtXAccountAdd")).Text.Trim()),
                                                            (((RadTextBox)e.Item.FindControl("txtXAccountDescriptionAdd")).Text.Trim())
                                                          );
                    ucStatus.Text = "Account Saved Successfully";
                    gvERMVesselAccruedExpensess.Rebind();
                }

            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                {

                    PhoenixAccountsERMVesselAccruedExpensess.ERMVesselAccruedExpensessUpdate((((RadTextBox)e.Item.FindControl("txtlAccountIdEdit")).Text.Trim()),
                                                                (((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text.Trim()),
                                                                (((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text.Trim()),
                                                                (((RadTextBox)e.Item.FindControl("txtBudgetIdEdit")).Text.Trim()),
                                                                (((RadTextBox)e.Item.FindControl("txtXAccountEdit")).Text.Trim()),
                                                                (((RadTextBox)e.Item.FindControl("txtXAccountDescriptionEdit")).Text.Trim()),
                                                                General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbldtkeyEdit")).Text.Trim())
                                                              );

                    ucStatus.Text = "Account Updated Successfully";
                    gvERMVesselAccruedExpensess.Rebind();
                }
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsERMVesselAccruedExpensess.ERMVesselAccruedExpensessDelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbldtkey")).Text));
                gvERMVesselAccruedExpensess.Rebind();
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDACCOUNTID", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDBUDGETID", "FLDXACC", "FLDXACCOUNTDESCRIPTION" };
        string[] alCaptions = { "Account Id", "Account Code", "Description", "Budget Id", "X Account", "X Account Description" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixAccountsERMVesselAccruedExpensess.ERMVesselAccruedExpensessSearch(General.GetNullableString(txtaccountid.Text.Trim())
                                                                                    , General.GetNullableString(txtcode.Text.Trim())
                                                                                    , General.GetNullableString(txtAccountdescription.Text.Trim())
                                                                                    , General.GetNullableString(txtBudgetId.Text.Trim())
                                                                                    , General.GetNullableString(txtXAccount.Text.Trim())
                                                                                    , General.GetNullableString(txtXAccountDescription.Text.Trim())
                                                                                    , (int)ViewState["PAGENUMBER"]
                                                                                    , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount
                                                                                    );

        General.SetPrintOptions("gvERMVesselAccruedExpensess", "ERM Vessel Accrued Expensess", alCaptions, alColumns, ds);

        gvERMVesselAccruedExpensess.DataSource = ds;
        gvERMVesselAccruedExpensess.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvERMVesselAccruedExpensess_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancel = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvERMVesselAccruedExpensess_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvERMVesselAccruedExpensess.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void Rebind()
    {
        gvERMVesselAccruedExpensess.EditIndexes.Clear();
        gvERMVesselAccruedExpensess.SelectedIndexes.Clear();
        gvERMVesselAccruedExpensess.DataSource = null;
        gvERMVesselAccruedExpensess.Rebind();
    }

    protected void gvERMVesselAccruedExpensess_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvERMVesselAccruedExpensess, "Edit$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void gvERMVesselAccruedExpensess_Sorting(object sender, GridSortCommandEventArgs e)
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDACCOUNTID", "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDBUDGETID", "FLDXACC", "FLDXACCOUNTDESCRIPTION" };
        string[] alCaptions = { "Account Id", "Account Code", "Description", "Budget Id", "X Account", "X Account Description" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsERMVesselAccruedExpensess.ERMVesselAccruedExpensessSearch(General.GetNullableString(txtaccountid.Text.Trim())
                                                                            , General.GetNullableString(txtcode.Text.Trim())
                                                                            , General.GetNullableString(txtAccountdescription.Text.Trim())
                                                                            , General.GetNullableString(txtBudgetId.Text.Trim())
                                                                            , General.GetNullableString(txtXAccount.Text.Trim())
                                                                            , General.GetNullableString(txtXAccountDescription.Text.Trim())
                                                                            , (int)ViewState["PAGENUMBER"]
                                                                            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount
                                                                            );

        Response.AddHeader("Content-Disposition", "attachment; filename=\"ERMVesslAccruedExpansess.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td colspan=2><h3>ERM Vessel Accrued Expensess</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");

        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td >");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td align=left>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
}
