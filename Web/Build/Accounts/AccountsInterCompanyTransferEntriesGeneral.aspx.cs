using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class AccountsInterCompanyTransferEntriesGeneral : PhoenixBasePage
{
    public int iUserCode;
    public int iCompanyid;
    PhoenixToolbar toolbarmain;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            iUserCode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyid, 0);

                toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Create Voucher", "CREATEVOUCHER");
                toolbarmain.AddButton("Post Voucher", "POSTVOUCHER");
                MenuOffSettingGeneral.AccessRights = this.ViewState;
                MenuOffSettingGeneral.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["poststatus"] = 0;
                LoadVoucherType();
                if (Request.QueryString["VOUCHERLINEITEMID"] != null && Request.QueryString["VOUCHERLINEITEMID"] != string.Empty)
                    ViewState["VOUCHERLINEITEMID"] = Request.QueryString["VOUCHERLINEITEMID"].ToString();
                BindFields();
                ddlBankAccount.Enabled = false;
            }
            if (Request.QueryString["TotalVoucherAmount"] != null && Request.QueryString["TotalVoucherAmount"] != string.Empty)
            {
                ViewState["TotalVoucherAmount"] = Request.QueryString["TotalVoucherAmount"];
            }
            if (Session["Voucher" + ViewState["VOUCHERLINEITEMID"]] != null)
                VoucherDataEdit();
            txtAccountId.Enabled = txtAccountCode.Enabled = txtAccountDescription.Enabled = false;
            txtAccountId.Attributes.Add("style", "visibility:hidden");
            imgShowAccount.Visible = false;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindFields()
    {
        try
        {
            if ((Request.QueryString["VOUCHERLINEITEMID"] != null) && (Request.QueryString["VOUCHERLINEITEMID"] != ""))
            {

                DataSet ds = PhoenixAccountsOffSettingEntries.OffSettingLineItemsList(
                                                   new Guid(ViewState["VOUCHERLINEITEMID"].ToString())
                                                   );
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtVoucherNumber.Text = dr["FLDVOUCHERNUMBER"].ToString();
                    txtLongDescription.Text = dr["FLDLONGDESCRIPTION"].ToString();
                    txtReferenceNo.Text = dr["FLDREFERENCEDOCUMENTNO"].ToString();
                    ddlCurrencyCode.SelectedCurrency = dr["FLDCURRENCYCODE"].ToString();
                    txtTargetCompany.Text = dr["FLDCOMPANYSHORTNAME"].ToString();
                    txtAmount.Text = dr["FLDAMOUNT"].ToString();

                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void LoadVoucherType()
    {
        try
        {
            string ShortCode = "BP,CP";
            DataSet ds = PhoenixAccountsOffSettingEntries.OffSettingVoucherType(ShortCode);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlVoucherType.DataSource = ds.Tables[0];
                ddlVoucherType.DataBind();
                ddlVoucherType.SelectedValue = "76";
            }
            txtDate.Text = DateTime.Now.ToShortDateString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuOffSettingGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        UserControlTabs ucTabs = (UserControlTabs)sender;
        int iVoucherId = 0;
        Guid iVoucherLineItemId = Guid.Empty;
        Guid contravoucherid = Guid.Empty;
        try
        {
            if (dce.CommandName.ToUpper().Equals("CREATEVOUCHER"))
            {
                if (!IsValidOffSetAdd())
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["VOUCHERLINEITEMID"] != null)
                {
                    if (Session["Voucher" + ViewState["VOUCHERLINEITEMID"].ToString()] == null)
                    {
                        DataTable dtVoucher = new DataTable();
                        dtVoucher.Columns.Add("VoucherType", System.Type.GetType("System.String"));
                        dtVoucher.Columns.Add("Date", System.Type.GetType("System.String"));
                        dtVoucher.Columns.Add("ReferenceNumber", System.Type.GetType("System.String"));
                        dtVoucher.Columns.Add("BankAccount", System.Type.GetType("System.String"));
                        dtVoucher.Columns.Add("VoucherId", System.Type.GetType("System.Int32"));
                        dtVoucher.Columns["VoucherId"].AutoIncrement = true;
                        dtVoucher.Columns.Add("LongDescription", System.Type.GetType("System.String"));
                        dtVoucher.Columns.Add("CashAccount", System.Type.GetType("System.String"));
                        dtVoucher.Columns.Add("CashAccountCode", System.Type.GetType("System.String"));
                        dtVoucher.Columns.Add("CashAccountDescription", System.Type.GetType("System.String"));

                        DataRow drVoucher = dtVoucher.NewRow();
                        drVoucher[0] = ddlVoucherType.SelectedValue.ToString();
                        drVoucher[1] = txtDate.Text;
                        drVoucher[2] = txtReferenceNumber.Text;
                        drVoucher[3] = ddlBankAccount.SelectedBankAccount.ToUpper() != "DUMMY" ? ddlBankAccount.SelectedBankAccount : string.Empty;
                        drVoucher[5] = txtContraLongDescription.Text;
                        drVoucher[6] = txtAccountId.Text != "" ? txtAccountId.Text : string.Empty;
                        drVoucher[7] = txtAccountCode.Text != "" ? txtAccountCode.Text : string.Empty;
                        drVoucher[8] = txtAccountDescription.Text != "" ? txtAccountDescription.Text : string.Empty;
                        dtVoucher.Rows.Add(drVoucher);

                        Session.Add("Voucher" + ViewState["VOUCHERLINEITEMID"].ToString(), dtVoucher);
                        ucStatus.Text = "Voucher is created";
                        if (ddlVoucherType.SelectedValue.ToString() == "69" || ddlVoucherType.SelectedValue.ToString() == "71")
                        {
                            DataTable dt = (DataTable)Session["VoucherData" + ViewState["VOUCHERLINEITEMID"].ToString()];
                            if (dt.Rows.Count > 0)
                            {
                                dt.ImportRow(dt.Rows[0]);
                                dt.AcceptChanges();
                                dt.Rows[1]["FLDAMOUNT"] = Convert.ToDecimal("0");
                                dt.Rows[1][2] = Convert.ToInt32(dt.Rows[0][2].ToString()) + 10;
                                dt.AcceptChanges();
                                Session["VoucherData" + ViewState["VOUCHERLINEITEMID"].ToString()] = dt;
                            }
                        }
                        String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                        Session["New"] = "Y";
                        ddlVoucherType.Enabled = false;
                        txtContraLongDescription.Enabled = false;
                        txtDate.Enabled = false;
                        txtReferenceNumber.Enabled = false;
                        ddlBankAccount.Enabled = false;
                        imgShowAccount.Visible = false;
                    }
                    else
                    {
                        ucError.HeaderMessage = "";
                        ucError.ErrorMessage = "Voucher is already created";
                        ucError.Visible = true;
                        return;
                    }
                }
            }

            if (dce.CommandName.ToUpper().Equals("POSTVOUCHER"))
            {
                DataTable dtVoucherPostData = new DataTable();
                DataTable dtVoucherPostLineItem = new DataTable();
                int status = 0;
                if (ViewState["VOUCHERLINEITEMID"] != null)
                {
                    if (Session["Voucher" + ViewState["VOUCHERLINEITEMID"].ToString()] != null && Session["VoucherData" + ViewState["VOUCHERLINEITEMID"].ToString()] != null)
                    {
                        if (Convert.ToDouble(ViewState["TotalVoucherAmount"]) == 0)
                        {
                            dtVoucherPostData = (DataTable)Session["Voucher" + ViewState["VOUCHERLINEITEMID"].ToString()];
                            dtVoucherPostLineItem = (DataTable)Session["VoucherData" + ViewState["VOUCHERLINEITEMID"].ToString()];
                            if (dtVoucherPostData.Rows[0][0].ToString() == "69")
                            {
                                DataSet ds = PhoenixRegistersAccount.ListBankAccount(Convert.ToInt32(ddlBankAccount.SelectedBankAccount.ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    DataRow dr = ds.Tables[0].Rows[0];
                                    ViewState["VoucherNumberNameValuelist"] = ",CURRENCYCODE=" + dr["FLDCURRENCYCHAR"].ToString() + ",ACCOUNT=" + dr["FLDACCOUNTCHAR"].ToString() + ",";
                                }
                                PhoenixAccountsVoucher.BankVoucherInsert(
                                                                      PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                      69,
                                                                      int.Parse(ddlBankAccount.SelectedBankAccount),
                                                                      DateTime.Parse(txtDate.Text),
                                                                      txtReferenceNumber.Text,
                                                                      0,
                                                                      txtContraLongDescription.Text,
                                                                      DateTime.Parse(txtDate.Text),
                                                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                      ref iVoucherId,
                                                                      ViewState["VoucherNumberNameValuelist"].ToString()
                                                                    );
                                PhoenixAccountsOffSettingEntries.ContraMapVoucherLineItemInsert((Guid)General.GetNullableGuid(ViewState["VOUCHERLINEITEMID"].ToString()), iVoucherId, decimal.Parse(dtVoucherPostLineItem.Rows[0][8].ToString()));
                                for (int i = 0; i < dtVoucherPostLineItem.Rows.Count; i++)
                                {
                                    PhoenixAccountsVoucher.VoucherLineItemInsert(iVoucherId,
                                                                                 int.Parse(dtVoucherPostLineItem.Rows[i][3].ToString()),
                                                                                 int.Parse(dtVoucherPostLineItem.Rows[i][5].ToString()),
                                                                                 decimal.Parse(dtVoucherPostLineItem.Rows[i][6].ToString()),
                                                                                 decimal.Parse(dtVoucherPostLineItem.Rows[i][7].ToString()),
                                                                                 decimal.Parse(dtVoucherPostLineItem.Rows[i][8].ToString()),
                                                                                 string.Empty,
                                                                                 string.Empty,
                                                                                 1,
                                                                                 1,
                                                                                 string.Empty,  //txtChequeno.Text
                                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                 dtVoucherPostData.Rows[0][2].ToString(),
                                                                                 dtVoucherPostLineItem.Rows[0][19].ToString(),
                                                                                 General.GetNullableGuid(dtVoucherPostLineItem.Rows[i][24].ToString()),
                                                                                 ref iVoucherLineItemId);

                                }
                            }
                            else if (dtVoucherPostData.Rows[0][0].ToString() == "71")
                            {
                                DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(txtAccountId.Text));
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    DataRow dr = ds.Tables[0].Rows[0];
                                    ViewState["VoucherNumberNameValuelist"] = ",CURRENCYCODE=" + dr["FLDVOUCHERPREFIXCURRENCYCODE"].ToString() + ",";
                                }
                                PhoenixAccountsVoucher.CashVoucherInsert(
                                                                      PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                      71,
                                                                      int.Parse(txtAccountId.Text),
                                                                      DateTime.Parse(txtDate.Text),
                                                                      txtReferenceNumber.Text,
                                                                      0,
                                                                      txtContraLongDescription.Text,
                                                                      General.GetNullableDateTime(string.Empty),
                                                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                      ref iVoucherId,
                                                                      ViewState["VoucherNumberNameValuelist"].ToString()
                                                                    );
                                PhoenixAccountsOffSettingEntries.ContraMapVoucherLineItemInsert((Guid)General.GetNullableGuid(ViewState["VOUCHERLINEITEMID"].ToString()), iVoucherId, decimal.Parse(dtVoucherPostLineItem.Rows[0][8].ToString()));
                                for (int i = 0; i < dtVoucherPostLineItem.Rows.Count; i++)
                                {
                                    PhoenixAccountsVoucher.VoucherLineItemInsert(iVoucherId,
                                                                                 int.Parse(dtVoucherPostLineItem.Rows[i][3].ToString()),
                                                                                 int.Parse(dtVoucherPostLineItem.Rows[i][5].ToString()),
                                                                                 decimal.Parse(dtVoucherPostLineItem.Rows[i][6].ToString()),
                                                                                 decimal.Parse(dtVoucherPostLineItem.Rows[i][7].ToString()),
                                                                                 decimal.Parse(dtVoucherPostLineItem.Rows[i][8].ToString()),
                                                                                 string.Empty,
                                                                                 string.Empty,
                                                                                 1,
                                                                                 1,
                                                                                 string.Empty,  //txtChequeno.Text
                                                                                 PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                 dtVoucherPostData.Rows[0][2].ToString(),
                                                                                 dtVoucherPostLineItem.Rows[0][19].ToString(),
                                                                                 General.GetNullableGuid(dtVoucherPostLineItem.Rows[i][24].ToString()),
                                                                                 ref iVoucherLineItemId);
                                }

                            }
                            if (dtVoucherPostData.Rows[0][0].ToString() == "76")
                            {
                                PhoenixAccountsVoucher.VoucherInsert(
                                                                   PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                   76,
                                                                   0,
                                                                   DateTime.Parse(txtDate.Text),
                                                                   txtReferenceNumber.Text,
                                                                   0,
                                                                   txtContraLongDescription.Text,
                                                                   General.GetNullableDateTime(string.Empty),
                                                                   PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                   ref iVoucherId,
                                                                   string.Empty
                                                                 );
                                PhoenixAccountsOffSettingEntries.ContraMapVoucherLineItemInsert((Guid)General.GetNullableGuid(ViewState["VOUCHERLINEITEMID"].ToString()), iVoucherId, decimal.Parse(dtVoucherPostLineItem.Rows[0][8].ToString()));
                                for (int i = 0; i < dtVoucherPostLineItem.Rows.Count; i++)
                                {
                                    PhoenixAccountsVoucher.VoucherLineItemInsert(iVoucherId,
                                                                        int.Parse(dtVoucherPostLineItem.Rows[i][3].ToString()),
                                                                        int.Parse(dtVoucherPostLineItem.Rows[i][5].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][6].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][7].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][8].ToString()),
                                                                        string.Empty,
                                                                        string.Empty,
                                                                        1,
                                                                        1,
                                                                        string.Empty,       //txtChequeno.Text,
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                        dtVoucherPostData.Rows[0][2].ToString(),
                                                                        dtVoucherPostLineItem.Rows[i][19].ToString(),
                                                                        General.GetNullableGuid(dtVoucherPostLineItem.Rows[i][24].ToString()),
                                                                        ref iVoucherLineItemId);
                                }

                            }
                            else if (dtVoucherPostData.Rows[0][0].ToString() == "72")
                            {
                                PhoenixAccountsVoucher.PuchaseInvoiceVoucherInsert(
                                                                   PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                   72,
                                                                   0,
                                                                   DateTime.Parse(txtDate.Text),
                                                                   txtReferenceNumber.Text,
                                                                   0,
                                                                   txtContraLongDescription.Text,
                                                                   General.GetNullableDateTime(string.Empty),
                                                                   PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                   ref iVoucherId,
                                                                   string.Empty
                                                                 );
                                PhoenixAccountsOffSettingEntries.ContraMapVoucherLineItemInsert((Guid)General.GetNullableGuid(ViewState["VOUCHERLINEITEMID"].ToString()), iVoucherId, decimal.Parse(dtVoucherPostLineItem.Rows[0][8].ToString()));
                                for (int i = 0; i < dtVoucherPostLineItem.Rows.Count; i++)
                                {
                                    PhoenixAccountsVoucher.VoucherLineItemInsert(iVoucherId,
                                                                        int.Parse(dtVoucherPostLineItem.Rows[i][2].ToString()),
                                                                        int.Parse(dtVoucherPostLineItem.Rows[i][5].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][6].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][7].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][8].ToString()),
                                                                        string.Empty,
                                                                        string.Empty,
                                                                        1,
                                                                        1,
                                                                        string.Empty,       //txtChequeno.Text,
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                        dtVoucherPostData.Rows[0][2].ToString(),
                                                                        dtVoucherPostLineItem.Rows[i][19].ToString(),
                                                                        General.GetNullableGuid(dtVoucherPostLineItem.Rows[i][24].ToString()),
                                                                        ref iVoucherLineItemId);
                                }

                            }
                            else if (dtVoucherPostData.Rows[0][0].ToString() == "74")
                            {
                                PhoenixAccountsVoucher.VoucherInsert(
                                                                   PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                   74,
                                                                   0,
                                                                   DateTime.Parse(txtDate.Text),
                                                                   txtReferenceNumber.Text,
                                                                   0,
                                                                   txtContraLongDescription.Text,
                                                                   General.GetNullableDateTime(string.Empty),
                                                                   PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                   ref iVoucherId,
                                                                   string.Empty
                                                                 );
                                PhoenixAccountsOffSettingEntries.ContraMapVoucherLineItemInsert((Guid)General.GetNullableGuid(ViewState["VOUCHERLINEITEMID"].ToString()), iVoucherId, decimal.Parse(dtVoucherPostLineItem.Rows[0][8].ToString()));
                                for (int i = 0; i < dtVoucherPostLineItem.Rows.Count; i++)
                                {
                                    string str = dtVoucherPostLineItem.Rows[i][24].ToString();
                                    PhoenixAccountsVoucher.VoucherLineItemInsert(iVoucherId,
                                                                       int.Parse(dtVoucherPostLineItem.Rows[i][2].ToString()),
                                                                      int.Parse(dtVoucherPostLineItem.Rows[i][5].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][6].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][7].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][8].ToString()),
                                                                        string.Empty,
                                                                        string.Empty,
                                                                        1,
                                                                        1,
                                                                        string.Empty,       //txtChequeno.Text,
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                        dtVoucherPostData.Rows[0][2].ToString(),
                                                                        dtVoucherPostLineItem.Rows[i][19].ToString(),
                                                                        General.GetNullableGuid(dtVoucherPostLineItem.Rows[i][24].ToString()),
                                                                        ref iVoucherLineItemId);
                                }

                            }
                            else if (dtVoucherPostData.Rows[0][0].ToString() == "73")
                            {
                                PhoenixAccountsVoucher.VoucherInsert(
                                                                      PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                      73,
                                                                      0,
                                                                      DateTime.Parse(txtDate.Text),
                                                                      txtReferenceNumber.Text,
                                                                      0,
                                                                      txtContraLongDescription.Text,
                                                                      General.GetNullableDateTime(string.Empty),
                                                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                      ref iVoucherId,
                                                                      string.Empty
                                                                    );
                                PhoenixAccountsOffSettingEntries.ContraMapVoucherLineItemInsert((Guid)General.GetNullableGuid(ViewState["VOUCHERLINEITEMID"].ToString()), iVoucherId, decimal.Parse(dtVoucherPostLineItem.Rows[0][8].ToString()));
                                for (int i = 0; i < dtVoucherPostLineItem.Rows.Count; i++)
                                {
                                    PhoenixAccountsVoucher.VoucherLineItemInsert(iVoucherId,
                                                                       int.Parse(dtVoucherPostLineItem.Rows[i][2].ToString()),
                                                                        int.Parse(dtVoucherPostLineItem.Rows[i][5].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][6].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][7].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][8].ToString()),
                                                                        string.Empty,
                                                                        string.Empty,
                                                                        1,
                                                                        1,
                                                                        string.Empty,       //txtChequeno.Text,
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                        dtVoucherPostData.Rows[0][2].ToString(),
                                                                        dtVoucherPostLineItem.Rows[i][19].ToString(),
                                                                        General.GetNullableGuid(dtVoucherPostLineItem.Rows[i][24].ToString()),
                                                                        ref iVoucherLineItemId);
                                }

                            }
                            else if (dtVoucherPostData.Rows[0][0].ToString() == "75")
                            {
                                PhoenixAccountsVoucher.VoucherInsert(
                                                                   PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                   75,
                                                                   0,
                                                                   DateTime.Parse(txtDate.Text),
                                                                   txtReferenceNumber.Text,
                                                                   0,
                                                                   txtContraLongDescription.Text,
                                                                   General.GetNullableDateTime(string.Empty),
                                                                   PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                   ref iVoucherId,
                                                                   string.Empty
                                                                 );
                                PhoenixAccountsOffSettingEntries.ContraMapVoucherLineItemInsert((Guid)General.GetNullableGuid(ViewState["VOUCHERLINEITEMID"].ToString()), iVoucherId, decimal.Parse(dtVoucherPostLineItem.Rows[0][8].ToString()));
                                for (int i = 0; i < dtVoucherPostLineItem.Rows.Count; i++)
                                {
                                    string str = dtVoucherPostLineItem.Rows[i][24].ToString();
                                    string str1 = dtVoucherPostLineItem.Rows[i][23].ToString();
                                    PhoenixAccountsVoucher.VoucherLineItemInsert(iVoucherId,
                                                                       int.Parse(dtVoucherPostLineItem.Rows[i][2].ToString()),
                                                                        int.Parse(dtVoucherPostLineItem.Rows[i][5].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][6].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][7].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][8].ToString()),
                                                                        string.Empty,
                                                                        string.Empty,
                                                                        1,
                                                                        1,
                                                                        string.Empty,       //txtChequeno.Text,
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                        dtVoucherPostData.Rows[0][2].ToString(),
                                                                        dtVoucherPostLineItem.Rows[i][19].ToString(),
                                                                        General.GetNullableGuid(dtVoucherPostLineItem.Rows[i][24].ToString()),
                                                                        ref iVoucherLineItemId);
                                }

                            }
                            else if (dtVoucherPostData.Rows[0][0].ToString() == "77")
                            {
                                PhoenixAccountsVoucher.VoucherInsert(
                                                                   PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                                   77,
                                                                   0,
                                                                   DateTime.Parse(txtDate.Text),
                                                                   txtReferenceNumber.Text,
                                                                   0,
                                                                   txtContraLongDescription.Text,
                                                                   General.GetNullableDateTime(string.Empty),
                                                                   PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                   ref iVoucherId,
                                                                   string.Empty
                                                                 );
                                PhoenixAccountsOffSettingEntries.ContraMapVoucherLineItemInsert((Guid)General.GetNullableGuid(ViewState["VOUCHERLINEITEMID"].ToString()), iVoucherId, decimal.Parse(dtVoucherPostLineItem.Rows[0][8].ToString()));
                                for (int i = 0; i < dtVoucherPostLineItem.Rows.Count; i++)
                                {
                                    PhoenixAccountsVoucher.VoucherLineItemInsert(iVoucherId,
                                                                       int.Parse(dtVoucherPostLineItem.Rows[i][2].ToString()),
                                                                        int.Parse(dtVoucherPostLineItem.Rows[i][5].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][6].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][7].ToString()),
                                                                        decimal.Parse(dtVoucherPostLineItem.Rows[i][8].ToString()),
                                                                        string.Empty,
                                                                        string.Empty,
                                                                        1,
                                                                        1,
                                                                        string.Empty,       //txtChequeno.Text,
                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                        dtVoucherPostData.Rows[0][2].ToString(),
                                                                        dtVoucherPostLineItem.Rows[i][19].ToString(),
                                                                        General.GetNullableGuid(dtVoucherPostLineItem.Rows[i][24].ToString()),
                                                                        ref iVoucherLineItemId);
                                }
                            }

                            if (ViewState["VOUCHERLINEITEMID"] != null)
                                PhoenixAccountsOffSettingEntries.ContraVoucherLineItemInsert((Guid)General.GetNullableGuid(ViewState["VOUCHERLINEITEMID"].ToString())
                                                                               , decimal.Parse(dtVoucherPostLineItem.Rows[0][8].ToString())
                                                                               , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , ref contravoucherid);
                            if (Session["VoucherAmount" + ViewState["VOUCHERLINEITEMID"]] != null)
                                PhoenixAccountsOffSettingEntries.ContraVoucherLineItemAmountUpdate(contravoucherid
                                                                , Convert.ToDecimal((Session["VoucherAmount" + ViewState["VOUCHERLINEITEMID"]].ToString()))
                                                                , PhoenixSecurityContext.CurrentSecurityContext.UserCode);

                            status = decimal.Parse(dtVoucherPostLineItem.Rows[0][8].ToString()) == Convert.ToDecimal(txtAmount.Text) == true ? 1 : 0;
                            // status = 1;
                            PhoenixAccountsOffSettingEntries.ContraVoucherLineItemStatusUpdate((Guid)General.GetNullableGuid(ViewState["VOUCHERLINEITEMID"].ToString())
                                                                                        , status
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                            Session.Remove("Voucher" + ViewState["VOUCHERLINEITEMID"].ToString());
                            Session.Remove("VoucherData" + ViewState["VOUCHERLINEITEMID"].ToString());
                            Session["Rows"] = 0;
                            ucStatus.Text = "Voucher information added";
                            String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                        }
                        else
                        {
                            ucError.HeaderMessage = "";
                            ucError.ErrorMessage = "Voucher should be balanced";
                            ucError.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        ucError.HeaderMessage = "";
                        ucError.ErrorMessage = "Voucher is mandatory to post";
                        ucError.Visible = true;
                        return;
                    }
                }
                else
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = "Voucher is mandatory to post";
                    ucError.Visible = true;
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void VoucherDataEdit()
    {
        if (Session["Voucher" + ViewState["VOUCHERLINEITEMID"].ToString()] != null)
        {
            DataTable dtVoucher = new DataTable();
            dtVoucher = (DataTable)Session["Voucher" + ViewState["VOUCHERLINEITEMID"].ToString()];
            ddlVoucherType.SelectedValue = dtVoucher.Rows[0][0].ToString();
            txtDate.Text = dtVoucher.Rows[0][1].ToString();
            txtReferenceNumber.Text = dtVoucher.Rows[0][2].ToString();
            ddlBankAccount.SelectedBankAccount = dtVoucher.Rows[0][3].ToString();
            txtContraLongDescription.Text = dtVoucher.Rows[0][5].ToString();
            if (dtVoucher.Rows[0][6] != null)
            {
                txtAccountId.Text = dtVoucher.Rows[0][6].ToString();
                txtAccountCode.Text = dtVoucher.Rows[0][7].ToString();
                txtAccountDescription.Text = dtVoucher.Rows[0][8].ToString();
                txtAccountId.Enabled = txtAccountCode.Enabled = txtAccountDescription.Enabled = false;
            }
        }
    }
    protected void Reset()
    {
        ddlVoucherType.SelectedValue = "Dummy";
        txtReferenceNumber.Text = "";
        txtContraLongDescription.Text = "";
        ddlBankAccount.SelectedBankAccount = "Dummy";
        txtAccountCode.Text = txtAccountDescription.Text = txtAccountId.Text = "";
    }
    protected void ddlVoucherType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlVoucherType.SelectedValue.ToString() == "69")
            ddlBankAccount.Enabled = true;
        if (ddlVoucherType.SelectedValue.ToString() == "76")
            ddlBankAccount.Enabled = false;
        if (ddlVoucherType.SelectedValue.ToString() == "72")
            ddlBankAccount.Enabled = false;
        if (ddlVoucherType.SelectedValue.ToString() == "74")
            ddlBankAccount.Enabled = false;
        if (ddlVoucherType.SelectedValue.ToString() == "73")
            ddlBankAccount.Enabled = false;
        if (ddlVoucherType.SelectedValue.ToString() == "75")
            ddlBankAccount.Enabled = false;
        if (ddlVoucherType.SelectedValue.ToString() == "77")
            ddlBankAccount.Enabled = false;
        if (ddlVoucherType.SelectedValue.ToString() == "71")
        {
            txtAccountId.Enabled = txtAccountCode.Enabled = txtAccountDescription.Enabled = true;
            imgShowAccount.Visible = true;
            ddlBankAccount.Enabled = false;
        }
    }
    private bool IsValidOffSetAdd()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (txtReferenceNumber.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Reference number is required.";
        if (txtContraLongDescription.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Contra voucher long description is required";
        if (ddlVoucherType.SelectedValue.ToString() == "69")
        {
            if (ddlBankAccount.SelectedBankAccount.ToUpper() == "DUMMY")
                ucError.ErrorMessage = "Bank account is required to create a bank payment voucher";
        }
        if (ddlVoucherType.SelectedValue.ToString() == "71")
        {
            if (txtAccountId.Text.Trim().Equals(""))
                ucError.ErrorMessage = "Cash account is required to create a cash payment voucher";
        }
        return (!ucError.IsError);
    }
    protected void ddlBankAccount_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBankAccount.SelectedBankAccount.ToUpper() != "DUMMY")
        {
            DataSet ds = PhoenixRegistersAccount.ListBankAccount(Convert.ToInt32(ddlBankAccount.SelectedBankAccount.ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["VoucherNumberNameValuelist"] = ",CURRENCYCODE=" + dr["FLDCURRENCYCHAR"].ToString() + ",ACCOUNT=" + dr["FLDACCOUNTCHAR"].ToString() + ",";
            }
        }
    }
    private bool IsValidContraVoucherData(string ReferenceNo, string VoucherType, string LongDescription)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ReferenceNo.Trim().Equals(""))
            ucError.ErrorMessage = "Reference number is required.";
        if (VoucherType.Trim().Equals(""))
            ucError.ErrorMessage = "Voucher type is required";
        if (LongDescription.Trim().Equals(""))
            ucError.ErrorMessage = "Long description is required";

        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
