using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsBankPaymentVoucher : PhoenixBasePage
{
    public int iUserCode;
    public int iCompanyId;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        iUserCode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        iCompanyId = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            txtVoucherDate.Text = General.GetDateTimeToString(DateTime.Now.ToString());
            if (Request.QueryString["VOUCHERID"] != string.Empty)
            {
                ViewState["VOUCHERID"] = Request.QueryString["VOUCHERID"];
            }
            if (Request.QueryString["voucherid"] != string.Empty)
            {
                ViewState["voucherid"] = Request.QueryString["voucherid"];
            }
            if (Request.QueryString["type"] != null)
            {
                ViewState["type"] = Request.QueryString["type"].ToString();
            }
            else
            {
                ViewState["type"] = "";
            }

            if (Request.QueryString["type"] == "1" && Request.QueryString["Lineitemid"] != null)
            {
                ViewState["Lineitemid"] = Request.QueryString["Lineitemid"];
                ViewState["ValueDate"] = Request.QueryString["ValueDate"];
                ViewState["TTRef"] = Request.QueryString["TTRef"];
                ViewState["Narrative"] = Request.QueryString["Narrative"];
                ViewState["BankAccount"] = Request.QueryString["BankAccount"];
                ViewState["Amount"] = Request.QueryString["Amount"];
                Reset();
            }

            VoucherEdit();
            //if (ViewState["VOUCHERID"] != null)
            //{
            //    ttlVoucher.Text = "Bank Payment Voucher      (" + PhoenixAccountsVoucher.VoucherNumber + ")     ";
            //}
        }
        for (int i = 0; i < Session.Contents.Count; i++)
        {
            if (Session.Keys[i].ToString().StartsWith("VOUCHERCURRENCYID"))
                Session.Remove(Session.Keys[i].ToString());
        }

        if (Request.QueryString["type"] == "1" && Request.QueryString["Lineitemid"] != null)
        {
            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            toolbar2.AddButton("Add", "ADD");
            toolbar2.AddButton("New", "NEW");
            MenuVoucher.AccessRights = this.ViewState;
            MenuVoucher.MenuList = toolbar2.Show();
        }

        if (Request.QueryString["type"] == "2")
        {
            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            toolbar2.AddButton("Save", "SAVE");
            MenuVoucher.AccessRights = this.ViewState;
            MenuVoucher.MenuList = toolbar2.Show();
        }
        PhoenixToolbar toolbar1 = new PhoenixToolbar();

        toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar1.AddButton("New", "NEW", ToolBarDirection.Right);
        toolbar1.AddButton("Add", "ADD", ToolBarDirection.Right);
        MenuVoucher.Title = "Bank Payment Voucher (" + txtVoucherNumber.Text + ")";
        MenuVoucher.AccessRights = this.ViewState;
        MenuVoucher.MenuList = toolbar1.Show();

    }

    protected void Voucher_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Accounts/AccountsBankPaymentVoucherList.aspx");
        }
        if (CommandName.ToUpper().Equals("DETAILS"))
        {
            Response.Redirect("../Accounts/AccountsBankPaymentVoucherLineItem.aspx?qvouchercode=" + ViewState["VOUCHERID"].ToString());
        }
        if (CommandName.ToUpper().Equals("NEW"))
        {
            Reset();
        }

        if (CommandName.ToUpper().Equals("ADD"))
        {
            int iVoucherId = 0;
            if (!IsValidVoucher())
            {
                string errormessage = "";
                errormessage = ucError.ErrorMessage;
                errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                return;
            }
            try
            {

                if (ViewState["SubVoucherTypeId"] != null)
                {
                    DataSet ds = PhoenixRegistersAccount.ListBankAccount(Convert.ToInt32(ddlBankAccount.SelectedBankAccount.ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        ViewState["VoucherNumberNameValuelist"] = ",CURRENCYCODE=" + dr["FLDCURRENCYCHAR"].ToString() + ",ACCOUNT=" + dr["FLDACCOUNTCHAR"].ToString() + ",";
                    }
                }

                if (Request.QueryString["type"] == "1" && Request.QueryString["Lineitemid"] != null)
                {

                    PhoenixAccountsVoucher.BankVoucherInsertForBankRecon(
                                                        PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                        68,
                                                        int.Parse(ddlBankAccount.SelectedBankAccount),
                                                        DateTime.Parse(txtVoucherDate.Text),
                                                        txtReferenceNumber.Text,
                                                        chkLocked.Checked == true ? 1 : 0,
                                                        txtLongDescription.Text,
                                                        General.GetNullableDateTime(ucDueDate.Text),
                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        ref iVoucherId,
                                                        ViewState["VoucherNumberNameValuelist"].ToString(),
                                                        General.GetNullableGuid(ViewState["Lineitemid"].ToString())
                                                      );
                    ucStatus.Text = "Voucher information added";
                    Response.Redirect("../Accounts/AccountsBankPaymentVoucherMaster.aspx", false);
                }
                else
                {

                    PhoenixAccountsVoucher.BankVoucherInsert(
                                                        PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                        68,
                                                        int.Parse(ddlBankAccount.SelectedBankAccount),
                                                        DateTime.Parse(txtVoucherDate.Text),
                                                        txtReferenceNumber.Text,
                                                        chkLocked.Checked == true ? 1 : 0,
                                                        txtLongDescription.Text,
                                                        General.GetNullableDateTime(ucDueDate.Text),
                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        ref iVoucherId,
                                                        ViewState["VoucherNumberNameValuelist"].ToString(),
                                                        txtRemarks.Text.Trim()
                                                      );
                    ucStatus.Text = "Voucher information added";
                }

            }
            catch (Exception ex)
            {
                string errormessage = "";
                errormessage = ex.Message;
                errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                return;
            }
            String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
            Session["New"] = "Y";
        }

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            int iVoucherId = 0;
            if (!IsValidVoucher())
            {
                string errormessage = "";
                errormessage = ucError.ErrorMessage;
                errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                return;
            }
            if (ViewState["VOUCHERID"] == null)
            {
                try
                {

                    PhoenixAccountsVoucher.BankVoucherInsert(
                                                       PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                       68,
                                                       int.Parse(ddlBankAccount.SelectedBankAccount),
                                                       DateTime.Parse(txtVoucherDate.Text),
                                                       txtReferenceNumber.Text,
                                                       chkLocked.Checked == true ? 1 : 0,
                                                       txtLongDescription.Text,
                                                       General.GetNullableDateTime(ucDueDate.Text),
                                                       PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                       ref iVoucherId,
                                                       ViewState["VoucherNumberNameValuelist"].ToString(),
                                                       txtRemarks.Text.Trim()
                                                     );
                    ucStatus.Text = "Voucher information added";
                }
                catch (Exception ex)
                {
                    string errormessage = "";
                    errormessage = ex.Message;
                    errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                    RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                    return;
                }
                String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
                Session["New"] = "Y";
            }
            else
            {
                try
                {
                    iVoucherId = int.Parse(ViewState["VOUCHERID"].ToString());

                    PhoenixAccountsVoucher.VoucherUpdate(int.Parse(ViewState["VOUCHERID"].ToString()),
                                                            DateTime.Parse(txtVoucherDate.Text),
                                                            txtReferenceNumber.Text,
                                                            chkLocked.Checked == true ? 1 : 0,
                                                            txtLongDescription.Text,
                                                            General.GetNullableDateTime(ucDueDate.Text),
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            txtRemarks.Text.Trim()
                                                          );
                    ucStatus.Text = "Voucher information updated";
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
            String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void Reset()
    {
        ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyId, 0);
        if (ViewState["VOUCHERID"] != null)
        {
            ViewState["VOUCHERID"] = null;
            txtVoucherNumber.Text = "";
            txtVoucherDate.Text = General.GetDateTimeToString(DateTime.Now.ToString());
            txtReferenceNumber.Text = "";
            chkLocked.Checked = false;
            txtUpdatedBy.Text = "";
            txtUpdatedDate.Text = "";
            txtLongDescription.Text = "";
            txtStatus.Text = "";
            ucDueDate.Text = "";
            ddlBankAccount.Enabled = true;
            ddlBankAccount.SelectedBankAccount = null;
            txtRemarks.Text = "";
            //    ttlVoucher.Text = "Bank Payment Voucher      ()     ";
        }
        else
        {
            txtVoucherNumber.Text = "";
            txtVoucherDate.Text = General.GetDateTimeToString(DateTime.Now.ToString());
            txtReferenceNumber.Text = "";
            chkLocked.Checked = false;
            txtUpdatedBy.Text = "";
            txtUpdatedDate.Text = "";
            txtLongDescription.Text = "";
            txtStatus.Text = "";
            ucDueDate.Text = "";
            ddlBankAccount.Enabled = true;
            ddlBankAccount.SelectedBankAccount = null;
            txtRemarks.Text = "";
            //  ttlVoucher.Text = "Bank Payment Voucher      ()     ";
        }
    }

    protected void VoucherEdit()
    {


        if (ViewState["VOUCHERID"] != null)
        {

            DataSet ds = PhoenixAccountsVoucher.BankVoucherEdit(int.Parse(ViewState["VOUCHERID"].ToString()));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtVoucherNumber.Text = dr["FLDVOUCHERNUMBER"].ToString();
                    txtVoucherDate.Text = General.GetDateTimeToString(dr["FLDVOUCHERDATE"].ToString());
                    MenuVoucher.Title = "Bank Payment Voucher (" + dr["FLDVOUCHERNUMBER"].ToString() + ")";
                    txtReferenceNumber.Text = dr["FLDREFERENCEDOCUMENTNO"].ToString();
                    if (dr["FLDLOCKEDYN"].ToString() == "1")
                        chkLocked.Checked = true;
                    txtUpdatedBy.Text = dr["FLDLASTUPDATEBYUSERNAME"].ToString();
                    txtUpdatedDate.Text = dr["FLDUPDATEDDATE"].ToString();
                    txtLongDescription.Text = dr["FLDLONGDESCRIPTION"].ToString().Replace("^", "\n");
                    txtStatus.Text = dr["FLDVOUCHERSTATUSNAME"].ToString();
                    ucDueDate.Text = General.GetDateTimeToString(dr["FLDDUEDATE"].ToString());
                    if (dr["FLDISACTIVEBANKACCOUNT"].ToString() == "0")
                    {
                        ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyId, 1);
                    }
                    else
                    {
                        ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyId, 0);
                    }
                    ddlBankAccount.SelectedBankAccount = dr["FLDSUBVOUCHERTYPEID"].ToString();
                    ddlBankAccount.Enabled = false;
                    ViewState["SubVoucherTypeId"] = dr["FLDSUBVOUCHERTYPEID"].ToString();
                    if (int.Parse(dr["FLDISPERIODLOCKED"].ToString()) == 1)
                        RemoveSaveButton();
                    txtRemarks.Text = dr["FLDREMARKS"].ToString();
                }
            }
        }
        else
        {

            ddlBankAccount.BankAccountList = PhoenixRegistersAccount.ListBankAccount(null, null, iCompanyId, 0);
        }
        if (Request.QueryString["type"] == "1" && ViewState["Lineitemid"] != null)
        {
            ViewState["Lineitemid"] = Request.QueryString["Lineitemid"];
            ViewState["SubVoucherTypeId"] = 1;
            string date = ViewState["ValueDate"].ToString();
            txtVoucherDate.Text = General.GetDateTimeToString(ViewState["ValueDate"].ToString());
            txtReferenceNumber.Text = ViewState["TTRef"].ToString();
            txtLongDescription.Text = ViewState["Narrative"].ToString();
            ddlBankAccount.SelectedBankAccount = ViewState["BankAccount"].ToString();
        }
    }

    private void RemoveSaveButton()
    {
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Add", "ADD");
        toolbar1.AddButton("New", "NEW");
        MenuVoucher.MenuList = toolbar1.Show();
        //  MenuVoucher.SetTrigger(pnlVoucher);
    }

    protected bool IsValidVoucher()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        ucError.clear = "";
        DateTime dtduedate = new DateTime();

        if (txtVoucherDate.Text == null)
            ucError.ErrorMessage = "Voucher date is required.";

        if (ddlBankAccount.SelectedBankAccount.ToUpper() == "DUMMY" || ddlBankAccount.SelectedBankAccount.ToString() == "")
            ucError.ErrorMessage = "Bank A/C is required.";

        if (txtReferenceNumber.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Reference number is required.";

        if (ucDueDate.Text != null && ucDueDate.Text.Trim().Length > 0)
        {
            dtduedate = DateTime.Parse(ucDueDate.Text);

            if (DateTime.Parse(dtduedate.ToShortDateString()) < DateTime.Parse(DateTime.Now.ToShortDateString()))
            {
                ucError.ErrorMessage = "Due date should be greater than or equal to current date.";
            }
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
