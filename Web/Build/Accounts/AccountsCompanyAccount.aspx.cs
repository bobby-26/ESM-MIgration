using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class AccountsCompanyAccount : PhoenixBasePage
{
    string straccountusage = "";
    string straccounttype = "";
    string straccountsource = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);

        MenuAccount.AccessRights = this.ViewState;
        MenuAccount.MenuList = toolbarmain.Show();
      //  MenuAccount.SetTrigger(pnlAccount);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Accounts/AccountsCompanyAccount.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvAccountsCompanyAccount')", "Print Grid", "icon_print.png", "PRINT");

        MenuAccountsCompanyAccount.AccessRights = this.ViewState;
        MenuAccountsCompanyAccount.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            chkShowAllAccount.Checked = true;
            BindHard();
            BindAccountsTree();
            AccountEdit();
            
        }
       
    }

    private void BindAccountsTree()
    {
        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION"};

        string[] alCaptions = { "Account Number", "Account Description" };

        DataSet ds = new DataSet();
        ds = PhoenixRegistersAccount.CompanyAccountTree(string.Empty, PhoenixSecurityContext.CurrentSecurityContext.CompanyID,chkShowAllAccount.Checked==true?1:0);
        tvwAccounts.DataTextField = "flddescription";
        tvwAccounts.DataValueField = "fldaccountid";
        tvwAccounts.DataFieldParentID = "fldparentid";
        // tvwAccounts.XPathField = "fldxpath";
        tvwAccounts.RootText = "Accounts";
        tvwAccounts.PopulateTree(ds.Tables[0]);
        General.SetPrintOptions("gvAccountsCompanyAccount", "Chart of Accounts", alCaptions, alColumns, ds);       
    }

    protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
    {
        RadTreeNodeEventArgs tvsne = (RadTreeNodeEventArgs)e;
        Session["SELECTEDACCOUNTCODE"] = tvsne.Node;
        lblSelectedNode.Visible = false;
        lblSelectedNode.Text = tvsne.Node.Value.ToString();
        Session["ACCOUNTID"] = lblSelectedNode.Text;
        ViewState["PARENTACCOUNTID"] = lblSelectedNode.Text;
        AccountEdit();
        string script = "resizeFrame(document.getElementById('divAccounts'));\r\n";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);

    }

    private void Reset()
    {
        Session["ACCOUNTID"] = null;
        txtAccountCode.Text = "";
        txtDescription.Text = "";
        chkActive.Checked = false;
        //ucHard.SelectedHard = "";
        rblAccountType.SelectedIndex = -1;
        rblAccountUsage.SelectedIndex = -1;
        rblAccountSource.SelectedIndex = -1;        
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
            int active = chkActive.Checked==true ? 1 : 0;
            if (!IsValidAccount())
            {
                ucError.Visible = true;
                return;
            }

            straccounttype = rblAccountType.SelectedItem.Value.ToString();
            straccountusage = rblAccountUsage.SelectedItem.Value.ToString();
            straccountsource = rblAccountSource.SelectedItem.Value.ToString();

            if (Session["ACCOUNTID"] == null)
            {                
                Reset();
            }
            else
            {
                try
                {
                    

                    PhoenixRegistersAccount.UpdateCompanyAccount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(txtCompanyAccountId.Text),
                                                                  active);
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
            AccountEdit();            
        }
    }

    public bool IsValidAccount()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtAccountCode.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Account code is required.";
        if(rblAccountType.SelectedItem == null)
            ucError.ErrorMessage = "Account type is required";
        if (rblAccountUsage.SelectedItem == null)
            ucError.ErrorMessage = "Account usage is required";
        if (rblAccountSource.SelectedItem == null)
            ucError.ErrorMessage = "Account source is required";
       
        if (ViewState["ACCOUNTSOURCE"] != null && ViewState["ACCOUNTSOURCE"].ToString() == "74")
        {
            AccountEdit();
        }
        return (!ucError.IsError);
    }

    protected void AccountEdit()
    {
        if (Session["ACCOUNTID"] != null && Session["ACCOUNTID"].ToString() != "0")
        {
            DataSet dsaccount = PhoenixRegistersAccount.EditCompanyAccount(Convert.ToInt32(Session["ACCOUNTID"].ToString()),PhoenixSecurityContext.CurrentSecurityContext.CompanyID) ;

            if (dsaccount.Tables.Count > 0)
            {
                DataRow draccount = dsaccount.Tables[0].Rows[0];
                txtAccountCode.Text = draccount["FLDACCOUNTCODE"].ToString();
                txtDescription.Text = draccount["FLDDESCRIPTION"].ToString();
                txtCompanyAccountId.Text = draccount["FLDCOMPANYACCOUNTID"].ToString();
                string accounttype = draccount["FLDACCOUNTTYPE"].ToString();
                string accountusage = draccount["FLDACCOUNTUSAGE"].ToString();
                string accountsource = draccount["FLDACCOUNTSOURCE"].ToString();
                ViewState["ACCOUNTSOURCE"] = draccount["FLDACCOUNTSOURCE"].ToString();
                //ucHard.SelectedHard = draccount["FLDACCOUNTGROUP"].ToString();
                ucBankCurrency.SelectedCurrency = draccount["FLDBANKCURRENCYID"].ToString();
                Session["BANKCURRENCYID"] = draccount["FLDBANKCURRENCYID"].ToString();
                chkActive.Checked = draccount["FLDCOMPANYACCOUNTACTIVE"].ToString() == "1" ? true : false;
                rblAccountType.SelectedValue = accounttype;                  
                rblAccountUsage.SelectedValue = accountusage;
                rblAccountSource.SelectedValue = accountsource;
                ViewState["ACCOUNTLEVEL"] = draccount["FLDACCOUNTLEVEL"].ToString();
                ViewState["ACCOUNTUSAGE"] = draccount["FLDACCOUNTUSAGE"].ToString();
            }
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    }

    protected void BindHard()
    {
        //ucHard.HardTypeCode = Convert.ToInt32(PhoenixHardTypeCode.ACCOUNTGROUP).ToString();
        rblAccountType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                Convert.ToInt32(PhoenixHardTypeCode.ACCOUNTTYPE));
        rblAccountType.DataBindings.DataTextField = "FLDHARDNAME";
        rblAccountType.DataBindings.DataValueField = "FLDHARDCODE";
        rblAccountType.DataBind();

        rblAccountUsage.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                Convert.ToInt32(PhoenixHardTypeCode.ACCOUNTSOURCE));
        rblAccountUsage.DataBindings.DataTextField = "FLDHARDNAME";
        rblAccountUsage.DataBindings.DataValueField = "FLDHARDCODE";
        rblAccountUsage.DataBind();

        rblAccountSource.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                Convert.ToInt32(PhoenixHardTypeCode.ACCOUNTUSAGE));
        rblAccountSource.DataBindings.DataTextField = "FLDHARDNAME";
        rblAccountSource.DataBindings.DataValueField = "FLDHARDCODE";
        rblAccountSource.DataBind();
    }

    //private void ExpandTreeToNode()
    //{
    //    if(Session["SELECTEDACCOUNTCODE"] == null)
    //        return;

    //    TreeNode tn = (TreeNode)Session["SELECTEDACCOUNTCODE"];
    //    tvwAccounts.ThisTreeView.ExpandDepth = tn.Depth;
    //    BindAccountsTree();
    //    tn = tvwAccounts.ThisTreeView.FindNode(tn.ValuePath);
    //    while (tn.Depth > 1)
    //        tn = tn.Parent;
    //    tn.ExpandAll();
    //    tvwAccounts.ThisTreeView.FindNode(((TreeNode)Session["SELECTEDACCOUNTCODE"]).ValuePath).Select();
    //}

    protected void cmdSearchAccount_Click(object sender, EventArgs e)
    {
        //if ((txtAccountSearch.Text != null && txtAccountSearch.Text != "") || chkShowAllAccount.Checked == true)
        //{
        //    DataSet ds = new DataSet();
        //    ds = PhoenixRegistersAccount.AccountTree(txtAccountSearch.Text.Trim(), 1);
        //    tvwAccounts.DataTextField = "flddescription";
        //    tvwAccounts.DataValueField = "fldaccountid";
        //    tvwAccounts.DataFieldParentID = "fldparentid";
        //   // tvwAccounts.XPathField = "fldxpath";
        //    tvwAccounts.RootText = "Accounts";
        //    tvwAccounts.PopulateTree(ds.Tables[0]);
        // //   tvwAccounts.ThisTreeView.FindNode("Root").ExpandAll();
        //    if (txtAccountSearch.Text != null && txtAccountSearch.Text != "")
        //        chkShowAllAccount.Checked = false;
        //}
    }

    protected void cmdDeleteAccount_Click(object sender, EventArgs e)
    {
        if (Session["ACCOUNTID"] == null)
            return;
        PhoenixRegistersAccount.DeleteAccount(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(Session["ACCOUNTID"].ToString()));
    }

    protected void chkShowAllAccount_CheckedChanged(object sender, EventArgs e)
    {
        if (chkShowAllAccount.Checked == true)
        {
            //txtAccountSearch.Text = "";
            BindAccountsTree();
        }
    }
    protected void MenuAccountsCompanyAccount_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void ShowExcel()
    {
        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDACTIVE" };

        string[] alCaptions = { "Account Number", "Account Description","Active" };

        DataSet ds = new DataSet();
        ds = PhoenixRegistersAccount.CompanyAccountTreeExcel(string.Empty, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, chkShowAllAccount.Checked==true ? 1 : 0);
       
        Response.AddHeader("Content-Disposition", "attachment; filename=ChartofAccounts.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Chart of Accounts</h3></td>");
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
