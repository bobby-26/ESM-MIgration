using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class AccountsCashPaymentVoucher : PhoenixBasePage
{
    public int iUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "Display:None;");
       
     //   MenuVoucher.SetTrigger(pnlVoucher);
        txtAccountId.Attributes.Add("style", "visibility:hidden;");

        iUserCode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;

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
            BindCashAccountCurrency();
            VoucherEdit();
            if (ViewState["VOUCHERID"] != null)
            {
                //ttlVoucher.Text = "Cash Payment Voucher      (" + PhoenixAccountsVoucher.VoucherNumber + ")     ";
            }
        }

        for (int i = 0; i < Session.Contents.Count; i++)
        {
            if (Session.Keys[i].ToString().StartsWith("VOUCHERCURRENCYID"))
                Session.Remove(Session.Keys[i].ToString());
        }

        PhoenixToolbar toolbar1 = new PhoenixToolbar();

        toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar1.AddButton("New", "NEW", ToolBarDirection.Right);
        toolbar1.AddButton("Add", "ADD", ToolBarDirection.Right);

        MenuVoucher.AccessRights = this.ViewState;
        MenuVoucher.Title = "Cash Payment Voucher (" + txtVoucherNumber.Text + ")";
        MenuVoucher.MenuList = toolbar1.Show();

    }


    protected void Voucher_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Accounts/AccountsCashPaymentVoucherList.aspx");
        }
        if (CommandName.ToUpper().Equals("DETAILS"))
        {
            Response.Redirect("../Accounts/AccountsCashPaymentVoucherLineItem.aspx?qvouchercode=" + ViewState["VOUCHERID"].ToString());
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
                DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(txtAccountId.Text));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["VoucherNumberNameValuelist"] = ",CURRENCYCODE=" + dr["FLDVOUCHERPREFIXCURRENCYCODE"].ToString() + ",";
                }
                PhoenixAccountsVoucher.CashVoucherInsert(
                                                         PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                         70,
                                                         int.Parse(txtAccountId.Text),
                                                         DateTime.Parse(txtVoucherDate.Text),
                                                         txtReferenceNumber.Text,
                                                         chkLocked.Checked==true ? 1 : 0,
                                                         txtLongDescription.Text,
                                                         General.GetNullableDateTime(ucDueDate.Text),
                                                         PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                         ref iVoucherId,
                                                         ViewState["VoucherNumberNameValuelist"].ToString()
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
                    DataSet ds = PhoenixRegistersAccount.EditAccount(int.Parse(txtAccountId.Text));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        ViewState["VoucherNumberNameValuelist"] = ",CURRENCYCODE=" + dr["FLDVOUCHERPREFIXCURRENCYCODE"].ToString() + ",";
                    }


                    PhoenixAccountsVoucher.CashVoucherInsert(
                                                       PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                       70,
                                                       int.Parse(txtAccountId.Text),
                                                       DateTime.Parse(txtVoucherDate.Text),
                                                       txtReferenceNumber.Text,
                                                       chkLocked.Checked==true ? 1 : 0,
                                                       txtLongDescription.Text,
                                                       General.GetNullableDateTime(ucDueDate.Text),
                                                       PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                       ref iVoucherId,
                                                       ViewState["VoucherNumberNameValuelist"].ToString()
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

                    PhoenixAccountsVoucher.VoucherUpdate(int.Parse(ViewState["VOUCHERID"].ToString()), DateTime.Parse(txtVoucherDate.Text),
                                                            txtReferenceNumber.Text,
                                                            chkLocked.Checked==true ? 1 : 0,
                                                            txtLongDescription.Text,
                                                            General.GetNullableDateTime(ucDueDate.Text),
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                          );
                    ucStatus.Text = "Voucher information updated";
                }
                catch (Exception ex)
                {
                    string errormessage = "";
                    errormessage = ex.Message;
                    errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                    RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                    return;
                }
                String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void Reset()
    {
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
            txtAccountId.Text = "";
            txtAccountDescription.Text = "";
            txtAccountCode.Text = "";
          //  ttlVoucher.Text = "Cash Payment Voucher      ()     ";
            BindCashAccountCurrency();
        }
    }

    protected void VoucherEdit()
    {
        if (ViewState["VOUCHERID"] != null)
        {
            DataSet ds = PhoenixAccountsVoucher.VoucherEdit(int.Parse(ViewState["VOUCHERID"].ToString()));
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtVoucherNumber.Text = dr["FLDVOUCHERNUMBER"].ToString();
                    txtVoucherDate.Text = General.GetDateTimeToString(dr["FLDVOUCHERDATE"].ToString());
                    txtReferenceNumber.Text = dr["FLDREFERENCEDOCUMENTNO"].ToString();
                    if (dr["FLDLOCKEDYN"].ToString() == "1")
                        chkLocked.Checked = true;
                    txtUpdatedBy.Text = dr["FLDLASTUPDATEBYUSERNAME"].ToString();
                    txtUpdatedDate.Text = dr["FLDUPDATEDDATE"].ToString();
                    txtLongDescription.Text = dr["FLDLONGDESCRIPTION"].ToString();
                    txtStatus.Text = dr["FLDVOUCHERSTATUSNAME"].ToString();
                    ucDueDate.Text = General.GetDateTimeToString(dr["FLDDUEDATE"].ToString());
                    txtAccountId.Text = dr["FLDACCOUNTID"].ToString();
                    txtAccountCode.Text = dr["FLDACCOUNTCODE"].ToString();
                    txtAccountDescription.Text = dr["FLDACCOUNTDESCRIPTION"].ToString();
                    if (General.GetNullableInteger(dr["FLDBANKCURRENCYID"].ToString()) != null)
                        ddlCurrency.SelectedValue = dr["FLDBANKCURRENCYID"].ToString();

                    if (int.Parse(dr["FLDISPERIODLOCKED"].ToString()) == 1)
                        RemoveSaveButton();
                }
            }
        }
    }

    private void RemoveSaveButton()
    {
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("New", "NEW",ToolBarDirection.Right);
        toolbar1.AddButton("Add", "ADD", ToolBarDirection.Right);

        MenuVoucher.MenuList = toolbar1.Show();
        MenuVoucher.AccessRights = this.ViewState;
      //  MenuVoucher.SetTrigger(pnlVoucher);
    }

    protected bool IsValidVoucher()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        ucError.clear = "";
        DateTime dtduedate = new DateTime();

        if (General.GetNullableInteger(ddlCurrency.SelectedValue) == null)
            ucError.ErrorMessage = "Cash Account Currency is required.";

        if (txtAccountId.Text.Trim().Equals(""))
            ucError.ErrorMessage = "Cash account is required";
        if (txtVoucherDate.Text == null)
            ucError.ErrorMessage = "Voucher date is required";
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void BindCashAccountCurrency()
    {
        ddlCurrency.DataSource = PhoenixRegistersAccount.ListCashAccountCurrency();
        ddlCurrency.DataBind();

    }
    protected void ddlCurrency_DataBound(object sender, EventArgs e)
    {
        ddlCurrency.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ddlCurrency_TextChanged(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ddlCurrency.SelectedValue) != null)
        {
            DataSet ds = PhoenixAccountsVoucher.EditCashAccount(int.Parse(ddlCurrency.SelectedValue));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtAccountDescription.Text = dr["FLDDESCRIPTION"].ToString();
                txtAccountCode.Text = dr["FLDACCOUNTCODE"].ToString();
                txtAccountId.Text = dr["FLDACCOUNTID"].ToString();
            }
        }
    }
}
