using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsAdvancePaymentVoucherGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        cmdHiddenSubmit.Attributes.Add("style", "display:none;");
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        //toolbar1.AddButton("New", "NEW");
        //toolbar1.AddButton("Save", "SAVE");
        MenuVoucher.MenuList = toolbar1.Show();
        //MenuVoucher.SetTrigger(pnlVoucher);
        txtVendorId.Attributes.Add("style", "display:none");
        txtVendorId.Attributes.Add("onkeydown", "return false;");

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            if (Request.QueryString["VOUCHERID"] != string.Empty)
            {
                ViewState["VOUCHERID"] = Request.QueryString["VOUCHERID"];
            }
            InvoicePaymentVoucherEdit();            
        }
    }

    protected void Voucher_TabStripCommand(object sender, EventArgs e)
    {
        //DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        //if (dce.CommandName.ToUpper().Equals("NEW"))
        //{
        //    Reset();
        //}
        //if (dce.CommandName.ToUpper().Equals("SAVE"))
        //{
        //    string iVoucherNumber = "";
        //    if (!IsValidVoucher())
        //    {
        //        ucError.Visible = true;
        //        return;
        //    }
        //    if (ViewState["VOUCHERID"] == null)
        //    {
        //        try
        //        {
        //            PhoenixAccountsAdvancePaymentVoucher.PaymentVoucherInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
        //                                                General.GetNullableInteger(txtVendorId.Text),
        //                                               General.GetNullableInteger(ddlCurrency.SelectedCurrency), 
        //                                               ref iVoucherNumber
        //                                             );
        //            ucStatus.Text = "Voucher information added";
        //        }
        //        catch (Exception ex)
        //        {
        //            ucError.HeaderMessage = "";
        //            ucError.ErrorMessage = ex.Message;
        //            ucError.Visible = true;
        //            return;
        //        }
        //        String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
        //        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
        //    }
        //    else
        //    {
        //        iVoucherNumber = ViewState["VOUCHERID"].ToString();
        //        PhoenixAccountsInvoicePaymentVoucher.PaymentVoucherUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
        //           General.GetNullableInteger(txtVendorId.Text), General.GetNullableInteger(ddlCurrency.SelectedCurrency.ToString()), iVoucherNumber);
        //        ucStatus.Text = "Voucher information updated";
        //    }
        //    String scriptupdate = String.Format("javascript:parent.fnReloadList('code1');");
        //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
        //    Session["New"] = "Y";
       // }
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
            txtVendorId.Text = "";
            txtVendorCode.Text = "";
            txtVendorName.Text = "";
            ddlCurrency.SelectedCurrency = "";           
        }
    }

    protected void InvoicePaymentVoucherEdit()
    {
        if (ViewState["VOUCHERID"] != null)
        {
            DataSet ds = PhoenixAccountsAdvancePaymentVoucher.PaymentVoucherEdit(ViewState["VOUCHERID"].ToString());
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtVoucherNumber.Text = dr["FLDVOUCHERNUMBER"].ToString();
                    txtVendorId.Text = dr["FLDSUPPLIERCODE"].ToString();
                    txtVendorCode.Text = dr["FLDCODE"].ToString();
                    txtVendorName.Text = dr["FLDNAME"].ToString();
                    ddlCurrency.SelectedCurrency = dr["FLDCURRENCY"].ToString();
                    txtStatus.Text = dr["FLDPAYMENTVOUCHERSTATUS"].ToString();                   
                    txtAmount.Text = dr["FLDAMOUNT"].ToString();

                }
            }
        }
    }

    protected bool IsValidVoucher()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ddlCurrency.SelectedCurrency.ToUpper() == "DUMMY")
            ucError.ErrorMessage = "Currency is required.";
        if (txtVendorId.Text.ToString() == "")
            ucError.ErrorMessage = "Supplier is required.";

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
