using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class AccountsOwnerOfficeSingleDepartmentPost : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
                      
            toolbar.AddButton("Post", "POST",ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);

            MenuOfficeFund.AccessRights = this.ViewState;
            MenuOfficeFund.MenuList = toolbar.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsOwnerOfficeSingleDepartmentPost.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvBankCharge')", "Print Grid", "icon_print.png", "PRINT");
            MenuOfficeFundGrid.AccessRights = this.ViewState;
            MenuOfficeFundGrid.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["OwnerOfficeFundId"] = "";
                if (Request.QueryString["OwnerOfficeFundId"] != null)
                    ViewState["OwnerOfficeFundId"] = Request.QueryString["OwnerOfficeFundId"];
                ViewState["PAGENUMBER"] = 1;

                ddlBindAccount();
                ViewState["AccountCode"] = "";
                ViewState["VesselId"] = "";
                ViewState["BudgetCode"] = "";
                ViewState["OwnerBudgetCode"] = "";

                BindProjectCode();
            }
            BindOwnerBudgetCode();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlBindAccount()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = PhoenixRegistersAccount.AccountListforReport(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                       , txtAccountSearch.Text
                       , null
                       , null
                       , null
                       , null
                       , 1
                       , null, null,
                       1,
                       1000,
                       ref iRowCount,
                       ref iTotalPageCount);

            ds.Tables[0].Columns.Add("FLDACCOUNTDESC");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                dr["FLDACCOUNTDESC"] = dr["FLDACCOUNTCODE"] + "-" + dr["FLDDESCRIPTION"];
            }

            rblAccount.DataSource = ds;
            rblAccount.DataTextField = "FLDACCOUNTDESC";
            rblAccount.DataValueField = "FLDACCOUNTID";
            rblAccount.DataBind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void rblAccount_Changed(object sender, EventArgs e)
    {
        DataSet ds = new DataSet();
        try
        {
            ddlBudgetCode.Items.Clear();

            ViewState["AccountCode"] = rblAccount.SelectedItem.ToString().Substring(0, 7);
            txtAccountSearch.Text = rblAccount.SelectedItem.ToString();
            ds = PhoenixAccountsOfficeDebitCreditNoteGenerate.SubAccountList(General.GetNullableInteger(rblAccount.SelectedValue));

            if (ds.Tables.Count > 0)
            {
                ddlBudgetCode.DataSource = ds;
                ddlBudgetCode.DataTextField = "FLDSUBACCOUNTCODE";
                ddlBudgetCode.DataValueField = "FLDBUDGETID";
                ddlBudgetCode.DataBind();
            }
            ddlBudgetCode.Items.Insert(0, new RadComboBoxItem("--Select--", ""));

            ViewState["BudgetCode"] = ddlBudgetCode.SelectedValue;
            Getprincipal(Convert.ToInt32(rblAccount.SelectedValue));
            BindProjectCode();

            //BindOwnerBudgetCode();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindProjectCode()
    {
        ucProjectcode.bind(General.GetNullableInteger(rblAccount.SelectedValue),
            General.GetNullableInteger(ddlBudgetCode.SelectedValue));
    }


    protected void MenuOfficeFund_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("NEW"))
            {
                Clearcontrol();
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidLineItem(rblAccount.SelectedValue, ddlBudgetCode.SelectedValue, ucOwnerBudgetCode.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                if (lblFundBankChargeId.Text != "")
                {

                    PhoenixAccountsOwnerOfficeSingleDepartment.FundBankChargeUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                    , General.GetNullableGuid(lblFundBankChargeId.Text)
                                                                                    , int.Parse(rblAccount.SelectedValue)
                                                                                    , General.GetNullableInteger(ddlBudgetCode.SelectedValue)
                                                                                    , General.GetNullableGuid(ucOwnerBudgetCode.SelectedValue),General.GetNullableInteger(ucProjectcode.SelectedProjectCode)
                                                                                    );
                    ucStatus.Text = "Bank Charges Apportionment line item updated";
                    Rebind();

                }
                else
                {
                    PhoenixAccountsOwnerOfficeSingleDepartment.FundBankChargeInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                     , General.GetNullableGuid(ViewState["OwnerOfficeFundId"].ToString())
                                                                                     , int.Parse(rblAccount.SelectedValue)
                                                                                     , General.GetNullableInteger(ddlBudgetCode.SelectedValue)
                                                                                     , General.GetNullableGuid(ucOwnerBudgetCode.SelectedValue),General.GetNullableInteger(ucProjectcode.SelectedProjectCode));
                    ucStatus.Text = "Bank Charges Apportionment line item Added";
                    Rebind();

                }

                String script = "javascript:fnReloadList('codehelp1',null,'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            if (CommandName.ToUpper().Equals("POST"))
            {
                PhoenixAccountsOwnerOfficeSingleDepartment.FundBankChargePost(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , General.GetNullableGuid(ViewState["OwnerOfficeFundId"].ToString()));
                ucStatus.Text = "Voucher has been successfully posted.";
                Rebind();
                String script = "javascript:fnReloadList('codehelp1');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void Clearcontrol()
    {
        lblFundBankChargeId.Text = "";
        rblAccount.ClearSelection();
        txtAccountSearch.Text = "";
        ddlBudgetCode.SelectedValue = "";
        ucOwnerBudgetCode.SelectedValue = "";
        ucOwnerBudgetCode.Text = "";
        txtAmount.Text = "";
        ucProjectcode.SelectedProjectCode = "";
        ViewState["PRINCIPALID"] = null;
        ViewState["BudgetCode"] = null;
    }
    protected void MenuOfficeFundGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ViewState["PAGENUMBER"] = 1;
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidLineItem(string account, string SubAccount, string OwnerBudgetCode)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (account.Trim().Equals(""))
            ucError.ErrorMessage = "Account is required.";
        if (SubAccount.Trim().Equals(""))
            ucError.ErrorMessage = "SubAccount is required.";
        if (OwnerBudgetCode.Trim().Equals(""))
            ucError.ErrorMessage = "OwnerBudgetCode is required.";

        return (!ucError.IsError);
    }
    private bool ValidateBankCharges()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadLabel lblOwner;
        for (int i = 0; i < gvBankCharge.Items.Count; i++)
        {
            lblOwner = (RadLabel)gvBankCharge.Items[i].FindControl("lblOwnerBudgetCode");
            if (string.IsNullOrEmpty(lblOwner.Text))
                ucError.ErrorMessage = "OwnerBudgetCode is manadatory.";
        }
        return (!ucError.IsError);
    }

    protected void gvBankCharge_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton cmdEditCancel = (ImageButton)e.Item.FindControl("cmdEditCancel");
            if (cmdEditCancel != null) cmdEditCancel.Visible = SessionUtil.CanAccess(this.ViewState, cmdEditCancel.CommandName);

            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);

            ImageButton cmdSave = (ImageButton)e.Item.FindControl("cmdSave");
            if (cmdSave != null) cmdSave.Visible = SessionUtil.CanAccess(this.ViewState, cmdSave.CommandName);

            ImageButton cmdDelete = (ImageButton)e.Item.FindControl("cmdDelete");
        }

    }

    protected void gvBankCharge_RowEditing(object sender, GridViewEditEventArgs e)
    {
        RadGrid _gridView = (RadGrid)sender;
        //_gridView.EditIndex = e.NewEditIndex;
        //_gridView.SelectedIndex = e.NewEditIndex;
        int nCurrentRow = Int32.Parse(e.NewEditIndex.ToString());
        RadLabel FundBankChargeId = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblFundBankChargeId");
        RadLabel lblAccountid = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblAccountid");
        ViewState["BudgetCode"] = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblBudgetId")).Text;
        RadLabel lblOwnerBudgetCode = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblOwnerBudgetCode");
        ViewState["OwnerBudgetCode"] = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblOwnerBudgetCodeId")).Text;
        RadLabel lblAmount = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblAmount");

        lblFundBankChargeId.Text = FundBankChargeId.Text;
        rblAccount.SelectedValue = lblAccountid.Text;
        Getprincipal(Convert.ToInt32(rblAccount.SelectedValue));
        ddlBudgetCode.Items.Clear();
        DataSet ds1 = PhoenixAccountsOfficeDebitCreditNoteGenerate.SubAccountList(General.GetNullableInteger(rblAccount.SelectedValue));
        if (ds1.Tables.Count > 0)
        {
            ddlBudgetCode.DataSource = ds1;
            ddlBudgetCode.DataTextField = "FLDSUBACCOUNTCODE";
            ddlBudgetCode.DataValueField = "FLDBUDGETID";
            ddlBudgetCode.DataBind();
        }
        ddlBudgetCode.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        ddlBudgetCode.SelectedValue = ViewState["BudgetCode"].ToString();

        DataSet ds2 = PhoenixRegistersAccount.VesselAccountSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                            , (rblAccount.SelectedItem.ToString().Substring(0, 7)), "", null, null, null, 1, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        if (ds2.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds2.Tables[0].Rows[0];
            ViewState["VesselId"] = dr["FLDVESSELID"].ToString();
        }

        ucOwnerBudgetCode.Text = lblOwnerBudgetCode.Text;
        ucOwnerBudgetCode.SelectedValue = ViewState["OwnerBudgetCode"].ToString();

        txtAmount.Text = lblAmount.Text;
        txtAccountSearch.Text = rblAccount.SelectedItem.ToString();
        BindOwnerBudgetCode();
        Rebind();
     //   SetPageNavigator();
    }

    protected void gvBankCharge_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.ItemIndex;

            //if (e.CommandName.ToUpper().Equals("ADD"))
            //{
            //    if (!IsValidLineItem(rblAccount.SelectedValue))
            //    {
            //        ucError.Visible = true;
            //        return;
            //    }
            //    PhoenixAccountsOwnerOfficeSingleDepartment.FundBankChargeInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            //         , General.GetNullableGuid(ViewState["OwnerOfficeFundId"].ToString())
            //         , int.Parse(rblAccount.SelectedValue)
            //         , General.GetNullableInteger(ddlBudgetCode.SelectedValue));
            //    ucStatus.Text = "Bank Charges Apportionment line item updated";

            //    String script = "javascript:fnReloadList('codehelp1',null,'true');";
            //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            //    BindData();
            //    SetPageNavigator();
            //}

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                RadLabel FundBankChargeId = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblFundBankChargeId");
                RadLabel lblAccountid = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblAccountid");
                ViewState["BudgetCode"] = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblBudgetId")).Text;
                RadLabel lblOwnerBudgetCode = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblOwnerBudgetCode");
                ViewState["OwnerBudgetCode"] = ((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblOwnerBudgetCodeId")).Text;
                RadLabel lblAmount = (RadLabel)_gridView.Items[nCurrentRow].FindControl("lblAmount");

                lblFundBankChargeId.Text = FundBankChargeId.Text;
                rblAccount.SelectedValue = lblAccountid.Text;
                Getprincipal(Convert.ToInt32(rblAccount.SelectedValue));
                ddlBudgetCode.Items.Clear();
                DataSet ds1 = PhoenixAccountsOfficeDebitCreditNoteGenerate.SubAccountList(General.GetNullableInteger(rblAccount.SelectedValue));
                if (ds1.Tables.Count > 0)
                {
                    ddlBudgetCode.DataSource = ds1;
                    ddlBudgetCode.DataTextField = "FLDSUBACCOUNTCODE";
                    ddlBudgetCode.DataValueField = "FLDBUDGETID";
                    ddlBudgetCode.DataBind();
                }
                ddlBudgetCode.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
                ddlBudgetCode.SelectedValue = ViewState["BudgetCode"].ToString();

                DataSet ds2 = PhoenixRegistersAccount.VesselAccountSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                    , (rblAccount.SelectedItem.ToString().Substring(0, 7)), "", null, null, null, 1, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                if (ds2.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds2.Tables[0].Rows[0];
                    ViewState["VesselId"] = dr["FLDVESSELID"].ToString();
                }

                ucOwnerBudgetCode.Text = lblOwnerBudgetCode.Text;
                ucOwnerBudgetCode.SelectedValue = ViewState["OwnerBudgetCode"].ToString();

                txtAmount.Text = lblAmount.Text;
                txtAccountSearch.Text = rblAccount.SelectedItem.ToString();
                BindOwnerBudgetCode();
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsOwnerOfficeSingleDepartment.FundBankChargeDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                     , new Guid(((RadLabel)_gridView.Items[nCurrentRow].FindControl("lblFundBankChargeId")).Text)
                     , General.GetNullableGuid(ViewState["OwnerOfficeFundId"].ToString()));
                String script = "javascript:fnReloadList('codehelp1',null,'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                Rebind();
                ucStatus.Text = "Deleted successfully.";
                // SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDDESCRIPTION", "FLDSUBACCOUNT", "FLDOWNERBUDGETCODE", "FLDPROJECTCODE", "FLDAMOUNT" };
            string[] alCaptions = { "Account Code/ Description", "Sub Account", "Owner Budget Code","Project Code", "Amount" };

            DataSet ds = PhoenixAccountsOwnerOfficeSingleDepartment.FundBankChargeList(General.GetNullableGuid(ViewState["OwnerOfficeFundId"].ToString()),
                                                                                                        (int)ViewState["PAGENUMBER"],
                                                                                                        gvBankCharge.PageSize,
                                                                                                        ref iRowCount,
                                                                                                        ref iTotalPageCount);

            General.SetPrintOptions("gvBankCharge", "Bank Charge", alCaptions, alColumns, ds);

            gvBankCharge.DataSource = ds;
            gvBankCharge.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDESCRIPTION", "FLDSUBACCOUNT", "FLDOWNERBUDGETCODE", "FLDPROJECTCODE", "FLDAMOUNT" };
        string[] alCaptions = { "Account Code/ Description", "Sub Account", "Owner Budget Code", "Project Code","Amount" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixAccountsOwnerOfficeSingleDepartment.FundBankChargeList(General.GetNullableGuid(ViewState["OwnerOfficeFundId"].ToString()),
                                                                                                    (int)ViewState["PAGENUMBER"],
                                                                                                    iRowCount,
                                                                                                    ref iRowCount,
                                                                                                    ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=BankCharge.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Bank Charge</h3></td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString());
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        Rebind();

    }

    protected void cmdSearchAccount_Click(object sender, EventArgs e)
    {
        ddlBindAccount();
    }

    protected void ddlBudgetCode_Changed(object sender, EventArgs e)
    {
        ucOwnerBudgetCode.SelectedValue = "";
        ucOwnerBudgetCode.Text = "";

        ViewState["BudgetCode"] = ddlBudgetCode.SelectedValue;
        BindOwnerBudgetCode();
        BindProjectCode();
    }

    private void BindOwnerBudgetCode()
    {
        if (Convert.ToString(ViewState["PRINCIPALID"]) != "" && ViewState["BudgetCode"].ToString() != "")
        {
            ucOwnerBudgetCode.OwnerId = ViewState["PRINCIPALID"].ToString();
            ucOwnerBudgetCode.BudgetId = ViewState["BudgetCode"].ToString();

            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? iownerid = 0;
            DataSet ds1 = PhoenixCommonRegisters.InternalBillingOwnerBudgetCodeSearch(null
                                                                                     , null
                                                                                     , General.GetNullableInteger(Convert.ToString(ViewState["PRINCIPALID"]))
                                                                                     , null
                                                                                     , General.GetNullableInteger(ViewState["BudgetCode"].ToString())
                                                                                     , null, null
                                                                                     , 1
                                                                                     , General.ShowRecords(null)
                                                                                     , ref iRowCount
                                                                                     , ref iTotalPageCount
                                                                                     , ref iownerid);

            if (ds1.Tables[0].Rows.Count > 0)
                ViewState["OwnerBudgetCode"] = "1";
            else
                ViewState["OwnerBudgetCode"] = "";
            if (ds1.Tables[0].Rows.Count == 1)
            {
                DataRow dr = ds1.Tables[0].Rows[0];
                ucOwnerBudgetCode.SelectedValue = dr["FLDOWNERBUDGETCODEID"].ToString();
                ucOwnerBudgetCode.Text = dr["FLDOWNERBUDGETCODE"].ToString();
            }
        }
    }

    public void Getprincipal(int accountid)
    {
        try
        {
            DataSet ds = null;
            ds = PhoenixRegistersAccount.EditAccount(accountid);
            DataRow dr = ds.Tables[0].Rows[0];
            ViewState["PRINCIPALID"] = Convert.ToString(dr["FLDPRINCIPALID"]);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvBankCharge.SelectedIndexes.Clear();
        gvBankCharge.EditIndexes.Clear();
        gvBankCharge.DataSource = null;
        gvBankCharge.Rebind();
    }

    protected void gvBankCharge_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvBankCharge.CurrentPageIndex + 1;
        BindData();
    }
}
