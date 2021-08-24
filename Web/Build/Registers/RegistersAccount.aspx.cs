using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI;
using Telerik.Web.UI;
public partial class RegistersAccount : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbarmain.AddButton("New", "NEW", ToolBarDirection.Right);
        MenuAccount.AccessRights = this.ViewState;
        MenuAccount.MenuList = toolbarmain.Show();
        txtVendorId.Attributes.Add("style", "visibility:hidden");
        txtVenderName.Attributes.Add("style", "visibility:hidden");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersAccount.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRegisterAccount')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuRegisterAccount.AccessRights = this.ViewState;
        MenuRegisterAccount.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            if (General.GetNullableInteger(Convert.ToString(Session["ACCOUNTID"])) == 0)
                Session["ACCOUNTID"] = null;
            chkShowAllAccount.Checked = true;
            BindHard();
            BindAccountsTree();
            AccountEdit(Convert.ToString(Session["ACCOUNTID"]));
            EnableCurrency();
        }

        if (ddlAccountSource.SelectedValue == "73" || ddlAccountUsage.SelectedValue == "81")
        {
            txtVendorCode.Enabled = true;
            if (ddlAccountUsage.SelectedValue == "81")
                txtVendorCode.CssClass = "dropdown_mandatory";
            else
                txtVendorCode.CssClass = "input";
            ImgSupplierPickList.Enabled = true;
            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?addresstype=128&txtsupcode=" + txtVendorCode.Text + "&framename=ifMoreInfo', true); ");
        }
        else
        {
            txtVendorCode.Enabled = false;
            txtVendorCode.CssClass = "input";
            txtVendorCode.Text = "";
            txtVendorId.Text = "";
            txtVenderName.Text = "";
            ImgSupplierPickList.Enabled = false;
        }
    }

    private void BindAccountsTree()
    {
        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDACCOUNTTYPE", "FLDACCOUNTUSAGE", "FLDACCOUNTSOURCE" };
        string[] alCaptions = { "Account ", "Description", "Type", "Usage", "Source" };
        DataSet ds = new DataSet();
        int IsActive = 0;
        if (chkShowAllAccount.Checked == true)
            IsActive = 1;

        ds = PhoenixRegistersAccount.AccountTree(string.Empty, IsActive);
        tvwAccounts.DataTextField = "flddescription";
        tvwAccounts.DataValueField = "fldaccountid";
        tvwAccounts.DataFieldParentID = "fldparentid";
        tvwAccounts.RootText = "Accounts";
        tvwAccounts.PopulateTree(ds.Tables[0]);
        General.SetPrintOptions("gvRegisterAccount", "Master Chart of Accounts", alCaptions, alColumns, ds);
    }

    protected void BudgetSearch(object sender, EventArgs e)
    {
        EnableCurrency();
    }

    protected void ucTree_SelectNodeEvent(object sender, EventArgs args)
    {
        RadTreeNodeEventArgs tvsne = (RadTreeNodeEventArgs)args;
        Session["SELECTEDACCOUNTCODE"] = tvsne.Node;
        lblSelectedNode.Visible = false;
        lblSelectedNode.Text = tvsne.Node.Value.ToString();
        Session["ACCOUNTID"] = lblSelectedNode.Text;
        ViewState["PARENTACCOUNTID"] = lblSelectedNode.Text;

        AccountEdit(Convert.ToString(Session["ACCOUNTID"]));

        if (ddlAccountSource.SelectedValue == "73" || ddlAccountUsage.SelectedValue == "81")
        {
            txtVendorCode.Enabled = true;
            if (ddlAccountUsage.SelectedValue == "81")
                txtVendorCode.CssClass = "dropdown_mandatory";
            else
                txtVendorCode.CssClass = "input";
            ImgSupplierPickList.Enabled = true;
            ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?addresstype=128&txtsupcode=" + txtVendorCode.Text + "&framename=ifMoreInfo', true); ");
        }
        else
        {
            txtVendorCode.Enabled = false;
            txtVendorCode.CssClass = "input";
            txtVendorCode.Text = "";
            txtVendorId.Text = "";
            txtVenderName.Text = "";
            ImgSupplierPickList.Enabled = false;
        }
        string script = "resizeFrame(document.getElementById('divAccounts'));\r\n";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
    }

    private void Reset()
    {

        if (General.GetNullableInteger(Convert.ToString(Session["ACCOUNTID"])) != null)
        {
            DataSet dsaccount = PhoenixRegistersAccount.EditAccount(Convert.ToInt32(Session["ACCOUNTID"]));

            if (dsaccount.Tables[0].Rows.Count > 0)
            {
                DataRow draccount = dsaccount.Tables[0].Rows[0];
                ddlAccountType.SelectedValue = draccount["FLDACCOUNTTYPE"].ToString();
                ddlAccountUsage.SelectedValue = draccount["FLDACCOUNTUSAGE"].ToString();
                ddlAccountSource.SelectedValue = draccount["FLDACCOUNTSOURCE"].ToString();
                ucSecurityLevel.SelectedHard = draccount["FLDACCOUNTACCESSLEVEL"].ToString();
                ViewState["ACCOUNTLEVEL"] = draccount["FLDACCOUNTLEVEL"].ToString();
            }
        }
        if (General.GetNullableInteger(Convert.ToString(Session["ACCOUNTID"])) == null)
        {
            ddlAccountType.SelectedValue = "Dummy";
            ddlAccountUsage.SelectedValue = "Dummy";
            ddlAccountSource.SelectedValue = "Dummy";
            ucSecurityLevel.SelectedHard = "";
        }
        Session["ACCOUNTID"] = null;
        txtAccountCode.Text = "";
        txtDescription.Text = "";
        chkActive.Checked = true;
        txtVendorCode.Text = "";
        txtVendorId.Text = "";
        txtVenderName.Text = "";
        chkOpenProjecct.Checked = false;
        chkMonetaryItems.Checked = false;

        ucBankCurrency.SelectedCurrency = "";
        EnableCurrency();
    }

    protected void RegistersAccountMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
        }
    }

    private void EnableCurrency()
    {
        if (ddlAccountUsage.SelectedValue == "335" || ddlAccountUsage.SelectedValue == "460")  // usage = 'Bank', 'Cash'
        {
            if (Convert.ToString(ViewState["ACCOUNTLEVEL"]) == "3" || Convert.ToString(ViewState["ACCOUNTLEVEL"]) == "4")
            {
                //if (Convert.ToString(ViewState["ACCOUNTLEVEL"]) == "3" && General.GetNullableInteger(Convert.ToString(Session["ACCOUNTID"])) == null )
                {
                    ucBankCurrency.CssClass = "dropdown_mandatory";
                    ucBankCurrency.Enabled = true;
                    txtVPCurrencyCode.CssClass = "input_mandatory";
                    txtVPCurrencyCode.Enabled = true;
                }
            }
        }
        else
        {
            ucBankCurrency.SelectedCurrency = "";
            ucBankCurrency.CssClass = "input";
            ucBankCurrency.Enabled = false;
            txtVPCurrencyCode.Text = "";
            txtVPCurrencyCode.CssClass = "input";
            txtVPCurrencyCode.Enabled = false;
        }
    }

    protected void Account_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
        }
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidAccount())
            {
                ucError.Visible = true;
                return;
            }

            if (General.GetNullableInteger(Convert.ToString(Session["ACCOUNTID"])) == null)
            {
                try
                {
                    PhoenixRegistersAccount.InsertAccount(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                   string.Empty,
                                                                   txtDescription.Text,
                                                                   General.GetNullableInteger(ddlAccountType.SelectedValue),
                                                                   General.GetNullableInteger(ddlAccountUsage.SelectedValue),
                                                                   General.GetNullableInteger(ddlAccountSource.SelectedValue),
                                                                   99,
                                                                   General.GetNullableInteger(Convert.ToString(ViewState["PARENTACCOUNTID"])),
                                                                   chkActive.Checked.Equals(true) ? 1 : 0,
                                                                   General.GetNullableInteger(ucBankCurrency.SelectedCurrency),
                                                                   General.GetNullableString(txtVPCurrencyCode.Text),
                                                                   chkMonetaryItems.Checked.Equals(true) ? 1 : 0,
                                                                   txtVendorCode.Text,
                                                                   General.GetNullableInteger(ucSecurityLevel.SelectedHard),
                                                                   chkOpenProjecct.Checked.Equals(true) ? 1 : 0);
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }

                BindHard();
                BindAccountsTree();
                AccountEdit(Convert.ToString(Session["ACCOUNTID"]));
                EnableCurrency();
                Reset();
            }
            else if (General.GetNullableInteger(Convert.ToString(Session["ACCOUNTID"])) != null && General.GetNullableInteger(Convert.ToString(Session["ACCOUNTID"])) != 0)
            {
                try
                {
                    PhoenixRegistersAccount.UpdateAccount(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    Convert.ToInt32(Session["ACCOUNTID"]),
                                                                    txtAccountCode.Text, txtDescription.Text,
                                                                    General.GetNullableInteger(ddlAccountType.SelectedValue),
                                                                    General.GetNullableInteger(ddlAccountUsage.SelectedValue),
                                                                    General.GetNullableInteger(ddlAccountSource.SelectedValue),
                                                                    99,
                                                                    chkActive.Checked.Equals(true) ? 1 : 0,
                                                                    General.GetNullableInteger(ucBankCurrency.SelectedCurrency),
                                                                    General.GetNullableString(txtVPCurrencyCode.Text),
                                                                    chkMonetaryItems.Checked.Equals(true) ? 1 : 0,
                                                                    txtVendorCode.Text,
                                                                    General.GetNullableInteger(ucSecurityLevel.SelectedHard),
                                                                    chkOpenProjecct.Checked.Equals(true) ? 1 : 0);
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
            //ExpandTreeToNode();
            AccountEdit(Convert.ToString(Session["ACCOUNTID"]));
            ucStatus.Text = "Account Code Saved Successfully";
        }
    }
    //private void ExpandTreeToNode()
    //{
    //    if (Session["SELECTEDACCOUNTCODE"] == null)
    //    {
    //        BindAccountsTree();
    //        return;
    //    }

    //    TreeNode tn = (TreeNode)Session["SELECTEDACCOUNTCODE"];
    //    //RadTreeNode tn = (RadTreeNode)Session["SELECTEDACCOUNTCODE"];
    //    tvwAccounts.ThisTreeView.ExpandDepth = tn.Depth;
    //    BindAccountsTree();
    //    tn = tvwAccounts.ThisTreeView.FindNode(tn.ValuePath);
    //    if (tn != null)
    //    {
    //        while (tn.Depth > 0)
    //            tn = tn.Parent;
    //        tn.ExpandAll();
    //        tvwAccounts.ThisTreeView.FindNode(((TreeNode)Session["SELECTEDACCOUNTCODE"]).ValuePath).Select();
    //    }
    //}
    public bool IsValidAccount()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtDescription.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        if (General.GetNullableInteger(ddlAccountType.SelectedValue) == null)
            ucError.ErrorMessage = "Type is required";

        if (General.GetNullableInteger(ddlAccountUsage.SelectedValue) == null)
            ucError.ErrorMessage = "Usage is required";

        if (General.GetNullableInteger(ddlAccountSource.SelectedValue) == null)
            ucError.ErrorMessage = "Source is required";



        if (ddlAccountUsage.SelectedValue == "335")  // usage = 'Bank'
        {
            if (Convert.ToString(ViewState["ACCOUNTLEVEL"]) == "3" || Convert.ToString(ViewState["ACCOUNTLEVEL"]) == "4")
            {
                if (General.GetNullableInteger(ucBankCurrency.SelectedCurrency) == null)
                    ucError.ErrorMessage = "Bank / Cash Currency is required";
                if (txtVPCurrencyCode.Text.Trim().Equals(""))
                    ucError.ErrorMessage = "Voucher Prefix Currency Code is required.";
            }
        }

        if (ddlAccountUsage.SelectedValue == "460")  // usage = 'Cash'
        {
            if (Convert.ToString(ViewState["ACCOUNTLEVEL"]) == "3" || Convert.ToString(ViewState["ACCOUNTLEVEL"]) == "4")
            {
                if (General.GetNullableInteger(ucBankCurrency.SelectedCurrency) == null)
                    ucError.ErrorMessage = "Bank / Cash Currency is required";
                //if (txtVPCurrencyCode.Text.Trim().Equals(""))
                //    ucError.ErrorMessage = "Voucher Prefix Currency Code is required.";
            }
        }
        return (!ucError.IsError);
    }

    protected void AccountEdit(string AccountId)
    {
        if (General.GetNullableInteger(Convert.ToString(AccountId)) != null)
        {
            DataSet dsaccount = PhoenixRegistersAccount.EditAccount(Convert.ToInt32(AccountId));

            if (dsaccount.Tables[0].Rows.Count > 0)
            {
                DataRow draccount = dsaccount.Tables[0].Rows[0];
                txtAccountCode.Text = draccount["FLDACCOUNTCODE"].ToString();
                txtDescription.Text = draccount["FLDDESCRIPTION"].ToString();
                ucBankCurrency.SelectedCurrency = draccount["FLDBANKCURRENCYID"].ToString();
                chkActive.Checked = draccount["FLDACTIVE"].ToString() == "1" ? true : false;
                ddlAccountType.SelectedValue = draccount["FLDACCOUNTTYPE"].ToString();
                ddlAccountUsage.SelectedValue = draccount["FLDACCOUNTUSAGE"].ToString();
                ddlAccountSource.SelectedValue = draccount["FLDACCOUNTSOURCE"].ToString();
                txtVPCurrencyCode.Text = draccount["FLDVOUCHERPREFIXCURRENCYCODE"].ToString();
                ViewState["ACCOUNTLEVEL"] = draccount["FLDACCOUNTLEVEL"].ToString();
                txtVendorCode.Text = draccount["FLDPRINCIPAL"].ToString();
                chkMonetaryItems.Checked = (draccount["FLDMONETARYITEMSYN"].ToString() == "1") ? true : false;
                ucSecurityLevel.SelectedHard = draccount["FLDACCOUNTACCESSLEVEL"].ToString();
                chkOpenProjecct.Checked = (draccount["FLDISRESTRICTTOOPENPROJECT"].ToString() == "1") ? true : false;
                EnableCurrency();
                if (ddlAccountSource.SelectedValue == "73" || ddlAccountUsage.SelectedValue == "81")
                {
                    txtVendorCode.Enabled = true;
                    if (ddlAccountUsage.SelectedValue == "81")
                        txtVendorCode.CssClass = "dropdown_mandatory";
                    else
                        txtVendorCode.CssClass = "input";
                    ImgSupplierPickList.Enabled = true;
                    ImgSupplierPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '" + Session["sitepath"] + "/Common/CommonPickListAccountsSupplier.aspx?addresstype=128&framename=ifMoreInfo', true); ");
                }
                else
                {
                    txtVendorCode.Enabled = false;
                    txtVendorCode.CssClass = "input";
                    txtVendorCode.Text = "";
                    txtVendorId.Text = "";
                    txtVenderName.Text = "";
                    ImgSupplierPickList.Enabled = false;
                }
            }
        }
        else if (General.GetNullableInteger(Convert.ToString(Session["ACCOUNTID"])) == null || General.GetNullableInteger(Convert.ToString(Session["ACCOUNTID"])) == 0)
        {
            Reset();
        }
    }
    protected void BindHard()
    {
        ddlAccountType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                Convert.ToInt32(PhoenixHardTypeCode.ACCOUNTTYPE));
        ddlAccountType.DataTextField = "FLDHARDNAME";
        ddlAccountType.DataValueField = "FLDHARDCODE";
        ddlAccountType.DataBind();
        ddlAccountType.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));

        ddlAccountUsage.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                Convert.ToInt32(PhoenixHardTypeCode.ACCOUNTSOURCE));
        ddlAccountUsage.DataTextField = "FLDHARDNAME";
        ddlAccountUsage.DataValueField = "FLDHARDCODE";
        ddlAccountUsage.DataBind();
        ddlAccountUsage.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
        ddlAccountSource.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                Convert.ToInt32(PhoenixHardTypeCode.ACCOUNTUSAGE));
        ddlAccountSource.DataTextField = "FLDHARDNAME";
        ddlAccountSource.DataValueField = "FLDHARDCODE";
        ddlAccountSource.DataBind();
        ddlAccountSource.Items.Insert(0, new DropDownListItem("--Select--", "Dummy"));
    }
    protected void cmdDeleteAccount_Click(object sender, EventArgs e)
    {
        if (Session["ACCOUNTID"] == null)
            return;

        PhoenixRegistersAccount.DeleteAccount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(Session["ACCOUNTID"].ToString()));
    }
    protected void chkShowAllAccount_CheckedChanged(object sender, EventArgs e)
    {
        BindAccountsTree();
    }
    protected void MenuRegisterAccount_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            //if (txtAccountSearch.Text.Trim() != string.Empty)
            //{
            //    ShowExcel(txtAccountSearch.Text);
            //}
            //else
            //{
            ShowExcel(string.Empty);
            //}
        }
    }
    protected void ShowExcel(string Account)
    {
        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDACCOUNTTYPE", "FLDACCOUNTUSAGE", "FLDACCOUNTSOURCE", "FLDACTIVE" };
        string[] alCaptions = { "Account ", "Description", "Type", "Usage", "Source", "Active" };
        DataSet ds = new DataSet();

        int IsActive = 0;
        if (chkShowAllAccount.Checked == true)
            IsActive = 1;

        ds = PhoenixRegistersAccount.AccountTreeExcel(Account, IsActive);

        Response.AddHeader("Content-Disposition", "attachment; filename=MasterChartofAccounts.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Master Chart of Accounts</h3></td>");
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
