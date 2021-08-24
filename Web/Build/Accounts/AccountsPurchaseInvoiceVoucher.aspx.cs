using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsPurchaseInvoiceVoucher : PhoenixBasePage
{
    public int iUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "Display:None;");
       
      //  MenuVoucher.SetTrigger(pnlVoucher);

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
                ViewState["VOUCHERID1"] = ViewState["VOUCHERID"];
            }
            VoucherEdit();
            if (ViewState["VOUCHERID"] != null)
            {
         //       ttlVoucher.Text = "Purchase Invoice Voucher      (" + PhoenixAccountsVoucher.VoucherNumber + ")     ";
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
        MenuVoucher.Title = "Purchase Invoice Voucher      (" + txtVoucherNumber.Text + ")     ";
        MenuVoucher.MenuList = toolbar1.Show();
    }    

    protected void Voucher_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Accounts/AccountsPurchaseInvoiceVoucherList.aspx");
        }
        if (CommandName.ToUpper().Equals("DETAILS"))
        {
            Response.Redirect("../Accounts/AccountsPurchaseInvoiceVoucherLineItem.aspx?qvouchercode=" + ViewState["VOUCHERID"].ToString());
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
                        DataSet ds = PhoenixAccountsVoucherNumberSetup.VoucherNumberFormatEdit(Convert.ToInt32(ViewState["SubVoucherTypeId"].ToString()));
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = ds.Tables[0].Rows[0];
                            ViewState["VoucherNumberNameValuelist"] = ",CURRENCYCODE=" + dr["FLDCURRENCYSHORTNAME"].ToString() + ",TRANSACTIONCODE=" + dr["FLDTRANSACTIONCODE"].ToString() + ",";
                        }
                    }
                    PhoenixAccountsVoucher.PuchaseInvoiceVoucherInsert(
                                                       PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                       72,
                                                       0,
                                                       DateTime.Parse(txtVoucherDate.Text),
                                                       txtReferenceNumber.Text,
                                                       chkLocked.Checked ? 1 : 0,
                                                       txtLongDescription.Text,
                                                       General.GetNullableDateTime(ucDueDate.Text),
                                                       PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                       ref iVoucherId,
                                                       string.Empty ,
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
                if (ex.Message == "INVOICE REFERENCENUMBER ALREADY EXISTS")
                    {
                        System.Collections.Hashtable ht = new System.Collections.Hashtable();
                        ht.Add("REFERENCENUMBER", txtReferenceNumber.Text);
                        ht.Add("COMPANYID", PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                        ht.Add("VOUCHERDATE", txtVoucherDate.Text);
                        ht.Add("LOCKED", chkLocked.Checked);
                        ht.Add("LONGDESCRIPTION", txtLongDescription.Text);
                        ht.Add("DUEDATE", ucDueDate.Text);
                        ht.Add("USERCODE", PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                        ht.Add("VOUCHERID", ViewState["VOUCHERID1"]);
                        ht.Add("TASK", "ADD");
                        Session["VOUCHERDETAILS"] = ht;
                        Response.Redirect("../Accounts/AccountsReferenceNumberPurchaseInvoiceList.aspx");
                    }
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
                    PhoenixAccountsVoucher.PuchaseInvoiceVoucherInsert(
                                                       PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                       72,
                                                       0,
                                                       DateTime.Parse(txtVoucherDate.Text),
                                                       txtReferenceNumber.Text,
                                                       chkLocked.Checked ? 1 : 0,
                                                       txtLongDescription.Text,
                                                       General.GetNullableDateTime(ucDueDate.Text),
                                                       PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                       ref iVoucherId,
                                                       string.Empty  ,
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
                    if (ex.Message == "INVOICE REFERENCENUMBER ALREADY EXISTS")
                    {
                        System.Collections.Hashtable ht = new System.Collections.Hashtable();
                        ht.Add("REFERENCENUMBER", txtReferenceNumber.Text.Replace(" ", ""));
                        ht.Add("COMPANYID", PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                        ht.Add("VOUCHERDATE", txtVoucherDate.Text);
                        ht.Add("LOCKED", chkLocked.Checked);
                        ht.Add("LONGDESCRIPTION", txtLongDescription.Text);
                        ht.Add("DUEDATE", ucDueDate.Text);
                        ht.Add("USERCODE", PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                        ht.Add("VOUCHERID", ViewState["VOUCHERID1"]);
                        ht.Add("TASK", "ADD");
                        Session["VOUCHERDETAILS"] = ht;
                        Response.Redirect("../Accounts/AccountsReferenceNumberPurchaseInvoiceList.aspx");
                    }
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

                    PhoenixAccountsVoucher.PurchaseInvoiceVoucherUpdate(int.Parse(ViewState["VOUCHERID"].ToString()), DateTime.Parse(txtVoucherDate.Text),
                                                            txtReferenceNumber.Text,
                                                            chkLocked.Checked ? 1 : 0,
                                                            txtLongDescription.Text,
                                                            General.GetNullableDateTime(ucDueDate.Text),
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                              txtRemarks.Text.Trim()
                                                          );
                    ucStatus.Text = "Voucher information updated";
                }
                catch (Exception ex)
                {
                    string errormessage = "";
                    errormessage = ex.Message;
                    errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                    RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                    if (ex.Message == "INVOICE REFERENCENUMBER ALREADY EXISTS")
                    {
                        System.Collections.Hashtable ht = new System.Collections.Hashtable();
                        ht.Add("REFERENCENUMBER", txtReferenceNumber.Text.Replace(" ", ""));
                        ht.Add("COMPANYID", PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                        ht.Add("VOUCHERDATE", txtVoucherDate.Text);
                        ht.Add("LOCKED", chkLocked.Checked);
                        ht.Add("LONGDESCRIPTION", txtLongDescription.Text);
                        ht.Add("DUEDATE", ucDueDate.Text);
                        ht.Add("USERCODE", PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                        ht.Add("VOUCHERID", ViewState["VOUCHERID1"]);
                        ht.Add("TASK", "UPDATE");
                        Session["VOUCHERDETAILS"] = ht;
                        Response.Redirect("../Accounts/AccountsReferenceNumberPurchaseInvoiceList.aspx");
                    }
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
            txtRemarks.Text = "";
            //  ttlVoucher.Text = "Purchase Invoice Voucher      ()     ";
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
                    ViewState["SubVoucherTypeId"] = dr["FLDSUBVOUCHERTYPEID"].ToString();
                    if (int.Parse(dr["FLDISPERIODLOCKED"].ToString()) == 1)
                        AddRemoveSaveButton();
                    txtRemarks.Text = dr["FLDREMARKS"].ToString();
                    //if (dr["FLDISDUPLICATEYN"] != null && dr["FLDISDUPLICATEYN"].ToString() == "1")
                    //{
                    //    HlinkRefDuplicate.NavigateUrl = "~/Accounts/AccountsVoucherDuplicateList.aspx?voucherid=" + ViewState["VOUCHERID"].ToString();
                    //    HlinkRefDuplicate.Visible = true;
                    //}
                }
            }
        }
    }

    private void AddRemoveSaveButton()
    {
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("New", "NEW",ToolBarDirection.Right);
        toolbar1.AddButton("Add", "ADD",ToolBarDirection.Right);


        MenuVoucher.AccessRights = this.ViewState;
        MenuVoucher.MenuList = toolbar1.Show();
    }

    protected bool IsValidVoucher()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        ucError.clear = "";
        DateTime dtduedate = new DateTime();

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
}
