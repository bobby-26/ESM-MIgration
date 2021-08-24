using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class AccountsERMSubAccount : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvERMSubAccount.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation(gvERMSubAccount.UniqueID, "Edit$" + r.RowIndex.ToString());
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
            toolbar.AddImageButton("../Accounts/AccountsERMSubAccount.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvERMSubAccount')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("../Accounts/AccountsERMSubAccount.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Accounts/AccountsERMSubAccount.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuERMSubAccount.AccessRights = this.ViewState;
            MenuERMSubAccount.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["SHOWYN"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvERMSubAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                BindData();

            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuERMSubAccount_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                subaccount.Text = string.Empty;
                AccountName.Text = string.Empty;
                SubAccountDescription.Text = string.Empty;
                Accountdescription.Text = string.Empty;
                MappedAccount.Text = string.Empty;
                Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvERMSubAccount_ItemCommand(object sender, GridCommandEventArgs e)
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
                if (Validate((((RadTextBox)e.Item.FindControl("txtSubAccountIdAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtSubAccountAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtSubAccDescAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtAccountIdadd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtAccDescAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtMappedERMCodeAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtERMAccountNameAdd")).Text)))
                {
                    PhoenixAccountsERMSubAccount.ERMSubAccInsert((((RadTextBox)e.Item.FindControl("txtSubAccountIdAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtSubAccountAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtSubAccDescAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtAccountIdadd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtAccDescAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtMappedERMCodeAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtERMAccountNameAdd")).Text)
                                                          );
                    ucStatus.Text = "Account Saved Successfully";
                    gvERMSubAccount.Rebind();
                }

            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (Validate((((RadTextBox)e.Item.FindControl("txtSubAccountIdEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtSubAccountEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtSubAccDescEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtAccountIdEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtAccDescEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtMappedERMCodeEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtERMAccountNameEdit")).Text)))
                {

                    PhoenixAccountsERMSubAccount.ERMSubAccUpdate((((RadTextBox)e.Item.FindControl("txtSubAccountIdEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtSubAccountEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtSubAccDescEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtAccountIdEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtAccDescEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtMappedERMCodeEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtERMAccountNameEdit")).Text),
                                                                General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbldtkeyEdit")).Text)
                                                              );

                    ucStatus.Text = "Account Updated Successfully";
                    gvERMSubAccount.Rebind();
                }
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsERMSubAccount.ERMSubAccDelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbldtkey")).Text));
                gvERMSubAccount.Rebind();
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool Validate(string SubAccountId
                                                    , string SubAccount
                                                    , string SubAccountDescription
                                                    , string Accountid
                                                    , string AccountDescription
                                                    , string MappedCode
                                                    , string ERMAccName)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if (string.IsNullOrEmpty(SubAccountId))
        //    ucError.ErrorMessage = "SubAccount Id is required.";
        //if (string.IsNullOrEmpty(SubAccount))
        //    ucError.ErrorMessage = "Sub Account is required.";
        //if (string.IsNullOrEmpty(SubAccountDescription))
        //    ucError.ErrorMessage = "Sub Account Description is required.";
        //if (string.IsNullOrEmpty(Accountid))
        //    ucError.ErrorMessage = "Account Id is required.";
        //if (string.IsNullOrEmpty(AccountDescription))
        //    ucError.ErrorMessage = "Account Description is required.";
        //if (string.IsNullOrEmpty(MappedCode))
        //    ucError.ErrorMessage = "Mapped Code is required.";
        //if (string.IsNullOrEmpty(ERMAccName))
        //    ucError.ErrorMessage = "ERM Account Name is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSUBACCOUNTID", "FLDSUBACCOUNT", "FLDDBOTBLSUBACCOUNTFLDDESCRIPTION", "FLDACCOUNTID", "FLDDBOTBLACCOUNTFLDDESCRIPTION", "FLDMAPPEDERMCODE", "FLDERMACCOUNTNAME" };
        string[] alCaptions = { "Sub Account Id", "Sub Account", "Sub Account Description", "Account Id", "Account Description", "Mapped ERM Code", "ERM Account Name" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixAccountsERMSubAccount.ERMSubAccSearch(subaccount.Text, AccountName.Text, SubAccountDescription.Text, Accountdescription.Text, MappedAccount.Text, (int)ViewState["PAGENUMBER"],
                                                    gvERMSubAccount.PageSize, ref iRowCount, ref iTotalPageCount
                                                   );

        General.SetPrintOptions("gvERMSubAccount", "ERM Sub Account", alCaptions, alColumns, ds);
        gvERMSubAccount.DataSource = ds;
        gvERMSubAccount.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvERMSubAccount_ItemDataBound(Object sender, GridItemEventArgs e)
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

    private void Rebind()
    {
        gvERMSubAccount.EditIndexes.Clear();
        gvERMSubAccount.SelectedIndexes.Clear();
        gvERMSubAccount.DataSource = null;
        gvERMSubAccount.Rebind();
    }

    protected void gvERMSubAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvERMSubAccount.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvERMSubAccount_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvERMSubAccount, "Edit$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    protected void gvERMSubAccount_Sorting(object sender, GridSortCommandEventArgs e)
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
        string[] alColumns = { "FLDSUBACCOUNTID", "FLDSUBACCOUNT", "FLDDBOTBLSUBACCOUNTFLDDESCRIPTION", "FLDACCOUNTID", "FLDDBOTBLACCOUNTFLDDESCRIPTION", "FLDMAPPEDERMCODE", "FLDERMACCOUNTNAME" };
        string[] alCaptions = { "Sub Account Id", "Sub Account", "Sub Account Description", "Account Id", "Account Description", "Mapped ERM Code", "ERM Account Name" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsERMSubAccount.ERMSubAccSearch(subaccount.Text, AccountName.Text, SubAccountDescription.Text, Accountdescription.Text, MappedAccount.Text, (int)ViewState["PAGENUMBER"],
                                                   PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount
                                                  );

        Response.AddHeader("Content-Disposition", "attachment; filename=\"ERMSubAccount.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>ERM Sub Account</h3></td>");
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
}
