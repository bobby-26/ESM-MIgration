using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsJournalVoucherVoucher : PhoenixBasePage
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
            }
            BindSubVoucherType();
            VoucherEdit();
            if (ViewState["VOUCHERID"] != null)
            {
                //ttlVoucher.Text = "Journal Voucher      (" + PhoenixAccountsVoucher.VoucherNumber + ")     ";
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
        MenuVoucher.Title = "Journal Voucher      (" + txtVoucherNumber.Text + ")     ";
        MenuVoucher.MenuList = toolbar1.Show();


    }

    protected void Voucher_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            Response.Redirect("../Accounts/AccountsJournalVoucherVoucherList.aspx");
        }
        if (CommandName.ToUpper().Equals("DETAILS"))
        {
            Response.Redirect("../Accounts/AccountsJournalVoucherVoucherLineItem.aspx?qvouchercode=" + ViewState["VOUCHERID"].ToString());
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
                if (ViewState["SubVoucherTypeId"] != null && ViewState["SubVoucherTypeId"].ToString() != string.Empty)
                {
                    DataSet ds = PhoenixAccountsVoucherNumberSetup.VoucherNumberFormatEdit(Convert.ToInt32(ViewState["SubVoucherTypeId"].ToString()));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow dr = ds.Tables[0].Rows[0];
                        ViewState["VoucherNumberNameValuelist"] = ",CURRENCYCODE=" + dr["FLDCURRENCYSHORTNAME"].ToString() + ",TRANSACTIONCODE=" + dr["FLDTRANSACTIONCODE"].ToString() + ",";
                    }
                }
                PhoenixAccountsVoucher.JournalVoucherInsert(
                                                   PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                   76,
                                                   General.GetNullableInteger(ucSubType.SelectedQuick)!=null?General.GetNullableInteger(ucSubType.SelectedQuick): 0,
                                                   DateTime.Parse(txtVoucherDate.Text),
                                                   txtReferenceNumber.Text,
                                                   chkLocked.Checked==true ? 1 : 0,
                                                   txtLongDescription.Text,
                                                   General.GetNullableDateTime(ucDueDate.Text),
                                                   PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                   ref iVoucherId,
                                                   string.Empty,
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
                    PhoenixAccountsVoucher.JournalVoucherInsert(
                                                       PhoenixSecurityContext.CurrentSecurityContext.CompanyID,
                                                       76,
                                                       General.GetNullableInteger(ucSubType.SelectedQuick) != null ? General.GetNullableInteger(ucSubType.SelectedQuick) : 0,
                                                       DateTime.Parse(txtVoucherDate.Text),
                                                       txtReferenceNumber.Text,
                                                       chkLocked.Checked==true ? 1 : 0,
                                                       txtLongDescription.Text,
                                                       General.GetNullableDateTime(ucDueDate.Text),
                                                       PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                       ref iVoucherId,
                                                       string.Empty,
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

                    PhoenixAccountsVoucher.JournalVoucherUpdate(int.Parse(ViewState["VOUCHERID"].ToString()), DateTime.Parse(txtVoucherDate.Text),
                                                            txtReferenceNumber.Text,
                                                            chkLocked.Checked==true ? 1 : 0,
                                                            txtLongDescription.Text,
                                                            General.GetNullableDateTime(ucDueDate.Text),
                                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                             General.GetNullableInteger(ucSubType.SelectedQuick),
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
        //    ttlVoucher.Text = "Journal Voucher      ()     ";
            ucSubType.SelectedText = "";
            txtRemarks.Text = "";
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

                    if (General.GetNullableInteger(dr["FLDSUBVOUCHERTYPEID"].ToString()) != null && General.GetNullableInteger(dr["FLDSUBVOUCHERTYPEID"].ToString())!=0 )
                    {
                        ucSubType.SelectedQuick = dr["FLDSUBVOUCHERTYPEID"].ToString();
                    }

                    if (int.Parse(dr["FLDISPERIODLOCKED"].ToString()) == 1)
                    AddRemoveSaveButton();
                    txtRemarks.Text = dr["FLDREMARKS"].ToString();
                }
            }
        }
    }

    private void AddRemoveSaveButton()
    {
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Add", "ADD");
        toolbar1.AddButton("New", "NEW");

        MenuVoucher.AccessRights = this.ViewState;
        MenuVoucher.MenuList = toolbar1.Show();
       // MenuVoucher.SetTrigger(pnlVoucher);
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
    private void BindSubVoucherType()
    {
        ucSubType.QuickTypeCode = "142";
        ucSubType.bind();
    }
         
}
