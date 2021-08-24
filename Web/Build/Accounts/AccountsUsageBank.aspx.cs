using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class AccountsUsageBank : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gdBudget.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl00");
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl01");
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsUsageBank.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvBudget')", "Print Grid", "icon_print.png", "PRINT");
        toolbargrid.AddImageLink("javascript:parent.Openpopup('codehelp1','','AccountsSubAccountFilter.aspx')", "Find", "search.png", "FIND");

        MenuRegistersBudget.AccessRights = this.ViewState;
        MenuRegistersBudget.MenuList = toolbargrid.Show();
        //  MenuRegistersBudget.SetTrigger(pnlBudget);

        EditAccount();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["BUDGETID"] = null;
            gdBudget.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //  BindData();
    }

    private void EditAccount()
    {
        DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(Session["ACCOUNTID"].ToString()));
        DataRow dr = ds.Tables[0].Rows[0];
        txtAccountCode.Text = dr["FLDACCOUNTCODE"].ToString();
        txtAccountDescription.Text = dr["FLDDESCRIPTION"].ToString();
        txtAccountUsage.Text = dr["FLDACCOUNTUSAGENAME"].ToString();
        ViewState["USAGE"] = dr["FLDACCOUNTUSAGE"].ToString();
        EnableControls();
    }

    private void EnableControls()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);


        MenuBudget.AccessRights = this.ViewState;
        MenuBudget.Title = "Sub Account";
        MenuBudget.MenuList = toolbarmain.Show();
        // MenuBudget.SetTrigger(pnlBudget);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void Reset()
    {
        ViewState["BUDGETID"] = null;
        txtSubAccount.Text = "";
        txtDescription.Text = "";
        txtVoucherPrefixAccountCode.Text = "";
        chkActive.Checked = true;
    }

    protected void RegistersBudgetMenu_TabStripCommand(object sender, EventArgs e)
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
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDVOUCHERPREFIXACCOUNTCODE", "FLDACTIVE" };
        string[] alCaptions = { "Budget Code", "Description", "Voucher Prefix AccountCode", "Active" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersBudget.BudgetSearch(
                          General.GetNullableInteger(ViewState["USAGE"].ToString())
                          , ""
                          , ""
                           , null
                           , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString()) //PhoenixSecurityContext.CurrentSecurityContext == null ? General.GetNullableInteger("") : PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                           , null
                        , sortexpression, sortdirection
                        , 1, PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                        , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SubAccount.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Sub Account</h3></td>");
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

    protected void Budget_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("NEW"))
        {
            ViewState["BUDGETID"] = null;
            Reset();
        }

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidBudget())
            {
                ucError.Visible = true;
                return;
            }
            if (ViewState["BUDGETID"] == null)
            {
                int? active = chkActive.Checked == true ? 1 : 0;

                try
                {
                    PhoenixRegistersSubAccount.InsertSubAccount(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , txtSubAccount.Text, txtDescription.Text
                        , null
                        , null
                        , null
                        , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
                        , General.GetNullableInteger(Session["ACCOUNTID"].ToString())
                        , General.GetNullableInteger(ViewState["USAGE"].ToString())
                        , General.GetNullableString(txtVoucherPrefixAccountCode.Text)
                        , active);
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
                Reset();
            }
            else
            {
                try
                {
                    int? active = chkActive.Checked == true ? 1 : 0;

                    PhoenixRegistersSubAccount.UpdateSubAccount(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , Convert.ToInt32(ViewState["BUDGETID"])
                        , txtSubAccount.Text, txtDescription.Text
                        , null
                        , null
                        , null
                        , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString())
                        , General.GetNullableInteger(Session["ACCOUNTID"].ToString())
                        , General.GetNullableInteger(ViewState["USAGE"].ToString())
                        , General.GetNullableString(txtVoucherPrefixAccountCode.Text)
                        , active);
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
            Rebind();
        }
    }
    protected void Rebind()
    {
        gdBudget.SelectedIndexes.Clear();
        gdBudget.EditIndexes.Clear();
        gdBudget.DataSource = null;
        gdBudget.Rebind();
    }
    public bool IsValidBudget()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtSubAccount.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Budget code is required.";
        if (txtDescription.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";
        if (txtVoucherPrefixAccountCode.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Voucher prefix account code is required.";
        return (!ucError.IsError);
    }

    public bool IsValidSubAccount()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (lblSubAccountMapId.Text.Trim() == "")
            ucError.ErrorMessage = "Budget code is required.";

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? sortdirection = null;
        int? accountid = null;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        string[] alColumns = { "FLDSUBACCOUNT", "FLDDESCRIPTION", "FLDVOUCHERPREFIXACCOUNTCODE", "FLDACTIVE" };
        string[] alCaptions = { "Budget Code", "Description", "Voucher Prefix AccountCode", "Active" };

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["USAGE"].ToString() == "79" || ViewState["USAGE"].ToString() == "335")
        {
            accountid = General.GetNullableInteger(Session["ACCOUNTID"].ToString());
        }

        NameValueCollection nvc = Filter.CurrentBudgetFilterSelection;

        DataSet ds = PhoenixRegistersBudget.BudgetSearch(
                           General.GetNullableInteger(ViewState["USAGE"].ToString())
                           , nvc != null ? General.GetNullableString(nvc.Get("txtAccountCodeSearch")) : string.Empty
                           , nvc != null ? General.GetNullableString(nvc.Get("txtDescription")) : string.Empty
                           , nvc != null ? General.GetNullableInteger(nvc.Get("ucGroupSearch")) : null
                           , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString()) //PhoenixSecurityContext.CurrentSecurityContext == null ? General.GetNullableInteger("") : PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                           , accountid
                           , sortexpression, sortdirection
                           , int.Parse(ViewState["PAGENUMBER"].ToString()), gdBudget.PageSize
                           , ref iRowCount
                           , ref iTotalPageCount);

        General.SetPrintOptions("gvBudget", "Budget Account", alCaptions, alColumns, ds);


        gdBudget.DataSource = ds;
        gdBudget.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void BudgetEdit()
    {
        if (ViewState["BUDGETID"] != null)
        {
            DataSet dsBudget = PhoenixRegistersBudget.EditSubAccountMap
                (int.Parse(ViewState["USAGE"].ToString()), Convert.ToInt32(ViewState["BUDGETID"].ToString()));

            if (dsBudget.Tables.Count > 0)
            {
                DataRow drBudget = dsBudget.Tables[0].Rows[0];
                txtSubAccount.Text = drBudget["FLDSUBACCOUNT"].ToString();
                txtDescription.Text = drBudget["FLDDESCRIPTION"].ToString();
                lblSubAccountMapId.Text = drBudget["FLDSUBACCOUNTMAPID"].ToString();
                txtVoucherPrefixAccountCode.Text = drBudget["FLDVOUCHERPREFIXACCOUNTCODE"].ToString();
                chkActive.Checked = drBudget["FLDACTIVEYN"].ToString() == "0" ? false : true;
                Rebind();
            }
        }
    }

    protected void gdBudget_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            PhoenixRegistersSubAccount.DeleteSubAccount(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            Convert.ToInt32(((RadLabel)e.Item.FindControl("lblBudgetid")).Text));
        }
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            ViewState["BUDGETID"] = Convert.ToInt32(((RadLabel)e.Item.FindControl("lblBudgetid")).Text);
            BudgetEdit();
        }
    }

    protected void gdBudget_Sorting(object sender, GridSortCommandEventArgs e)
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


    protected void gdBudget_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton imgEditBank = (ImageButton)e.Item.FindControl("imgEditBank");
            if (imgEditBank != null)
                if (!SessionUtil.CanAccess(this.ViewState, imgEditBank.CommandName)) imgEditBank.Visible = false;

            ImageButton imgEditTemplate = (ImageButton)e.Item.FindControl("imgEditTemplate");
            if (imgEditTemplate != null)
                if (!SessionUtil.CanAccess(this.ViewState, imgEditTemplate.CommandName)) imgEditTemplate.Visible = false;

            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
            if (cmdDelete != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;

            RadLabel lblBudgetid = (RadLabel)e.Item.FindControl("lblBudgetid");

            if (Session["BANKCURRENCYID"].ToString().Trim().Equals(""))
            {
                if (imgEditBank != null) imgEditBank.Visible = false;
                if (imgEditTemplate != null) imgEditTemplate.Visible = false;
            }
            else
            {
                if (imgEditBank != null)
                    imgEditBank.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Registers/RegistersBankAddress.aspx?subaccountid=" + lblBudgetid.Text + "');return false;");
                if (imgEditTemplate != null)
                    imgEditTemplate.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Accounts/AccountsBankDownloadMappedTemplates.aspx?bankid=" + lblBudgetid.Text + "');return false;");
            }
            if (cmdDelete != null) cmdDelete.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

        }

    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gdBudget_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gdBudget.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
