using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class ERMERMVoucherLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        
        PhoenixToolbar toolbar = new PhoenixToolbar();

        //txtAccountId.Attributes.Add("style", "visibility:hidden;");
        //txtAccountSource.ReadOnly = true;
        //txtAccountUsage.ReadOnly = true;
        if (!IsPostBack)
        {
            Reset();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            if (Request.QueryString["voucherlineitemcode"] != null && Request.QueryString["voucherlineitemcode"] != string.Empty)
            {
                ViewState["VOUCHERLINEITEMCODE"] = Request.QueryString["voucherlineitemcode"];
                VoucherLineEdit(new Guid(ViewState["VOUCHERLINEITEMCODE"].ToString()));
            }
            if (Request.QueryString["lastAddedlineitemid"] != null && Request.QueryString["lastAddedlineitemid"] != string.Empty)
            {
                ViewState["lastAddedlineitemid"] = Request.QueryString["lastAddedlineitemid"];
            }
            //btnShowBudget.Attributes.Add("onclick", "return showSubAccountPickList('spnPickListExpenseAccount', 'codehelp1', '','../Common/CommonPickListSubAccount.aspx', true);");
            //ttlVoucher.Text = "Row Number (" + txtRowNumber.Text + ")";
            ViewState["VOUCHERID"] = Request.QueryString["qvouchercode"];
            VoucherEdit();
        }
        //if (ViewState["ISACCOUNTACTIVE"] != null)
        //{
        //    if (ViewState["ISACCOUNTACTIVE"].ToString() == "1")
        //    {
        //        toolbar.AddButton("Add", "ADD");
        //        toolbar.AddButton("New", "NEW");
        //        toolbar.AddButton("Save", "SAVE");
        //    }
        //    else
        //    { toolbar.AddButton("New", "NEW"); }
        //}
        //else
        //{
        //    toolbar.AddButton("Add", "ADD");
        //    toolbar.AddButton("New", "NEW");
        //    toolbar.AddButton("Save", "SAVE");
        //}
        MenuVoucherLineItem.AccessRights = this.ViewState;
        MenuVoucherLineItem.Title = "Row Number (" + txtRowNumber.Text + ")";
        MenuVoucherLineItem.MenuList = toolbar.Show();
    }

    protected void VoucherEdit()
    {
        if (ViewState["VOUCHERID"] != null && ViewState["VOUCHERID"].ToString() != string.Empty)
        {

            DataSet ds = PhoenixAccountsVoucher.ERMVoucherEdit(ViewState["VOUCHERID"].ToString());
            if (ds.Tables.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtVoucherNumber.Text = dr["FLDVOUCHERNUMBER"].ToString();

                ViewState["VoucherDate"] = dr["FLDVOUCHERDATE"].ToString();
                //if (int.Parse(dr["FLDISPERIODLOCKED"].ToString()) == 1)
                //    MenuVoucherLineItem.Visible = false;
            }


        }
    }

    protected void Voucher_SetExchangeRate(object sender, EventArgs e)
    {
        //decimal dTransactionExchangerate = 0;
        //if (ddlCurrencyCode.SelectedCurrency.ToUpper() != "DUMMY")
        //{
        //    DataSet dsInvoice = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(ddlCurrencyCode.SelectedCurrency), DateTime.Parse(ViewState["VoucherDate"].ToString()));
        //    if (dsInvoice.Tables.Count > 0)
        //    {
        //        DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
        //        dTransactionExchangerate = decimal.Parse(drInvoice["FLDEXCHANGERATE"].ToString());
        //    }
        //}

        //if (ddlCurrencyCode.SelectedCurrency.ToUpper() != "DUMMY")
        //{
        //    DataSet ds = PhoenixRegistersCompany.EditCompany(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        //    if (ds.Tables.Count > 0)
        //    {
        //        DataRow dr = ds.Tables[0].Rows[0];

        //        DataSet dsbasecurrency = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(dr["FLDBASECURRENCY"].ToString()), DateTime.Parse(ViewState["VoucherDate"].ToString()));
        //        DataSet dsReportcurrency = PhoenixAccountsExchangeRateHistory.EditExchangeRateHistory(int.Parse(dr["FLDREPORTINGCURRENCY"].ToString()), DateTime.Parse(ViewState["VoucherDate"].ToString()));

        //        DataRow drbasecurrency = dsbasecurrency.Tables[0].Rows[0];
        //        DataRow drreportcurrency = dsReportcurrency.Tables[0].Rows[0];
        //        if (Session["VOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "Rate"] != null)
        //        {
        //            System.Collections.Hashtable ht = new System.Collections.Hashtable();
        //            ht = (System.Collections.Hashtable)Session["VOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "Rate"];
        //            txtExchangeRate.Text = ht["EXCHANGERATE"].ToString();
        //            txt2ndExchangeRate.Text = ht["2ndEXCHANGERATE"].ToString();
        //        }
        //        else
        //        {
        //            txtExchangeRate.Text = string.Format(String.Format("{0:#####.00000000000000000}", dTransactionExchangerate / decimal.Parse(drbasecurrency["FLDEXCHANGERATE"].ToString())));
        //            txt2ndExchangeRate.Text = string.Format(String.Format("{0:#####.00000000000000000}", dTransactionExchangerate / decimal.Parse(drreportcurrency["FLDEXCHANGERATE"].ToString())));
        //            System.Collections.Hashtable htInitialRate = new System.Collections.Hashtable();
        //            htInitialRate["INITIALEXCHANGERATE"] = decimal.Parse(txtExchangeRate.Text) < 1 ? '0' + txtExchangeRate.Text : txtExchangeRate.Text; ;
        //            htInitialRate["INITIAL2ndEXCHANGERATE"] = decimal.Parse(txt2ndExchangeRate.Text) < 1 ? '0' + txtExchangeRate.Text : txtExchangeRate.Text;
        //            Session["VOUCHERCURRENCYID" + ddlCurrencyCode.SelectedCurrency + "INITIALRate"] = htInitialRate;
        //        }
        //    }
        //}
        //else
        //{
        //    txtExchangeRate.Text = "";
        //    txt2ndExchangeRate.Text = "";
        //}
    }

    protected void Voucher_TabStripCommand(object sender, EventArgs e)
    {

    }

    protected void Reset()
    {

    }

    protected void EditVoucherLineItem(object sender, CommandEventArgs e)
    {
        string[] strValues = new string[2];
        strValues = e.CommandArgument.ToString().Split('^');
        VoucherLineEdit(new Guid(strValues[0]));
    }

    protected void VoucherLineEdit(Guid gLineId)
    {
        ViewState["VOUCHERLINEITEMCODE"] = gLineId.ToString();
        DataSet ds = PhoenixAccountsVoucher.ERMVoucherLineitemEdit(gLineId);
        if (ds.Tables.Count > 0)
        {
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtRowNumber.Text = dr["FLDVOUCHERLINEITEMNO"].ToString();
                txtAccountId.Text = dr["FLDXACC"].ToString();
                txtAccountCocde.Text = dr["FLDACCOUNTCODE"].ToString();
                txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
                txtERMVoucherNumber.Text = dr["FLDXVOUCHER"].ToString();
                txtAccountSource.Text = dr["FLDXACCSOURCE"].ToString();
                txtAccountUsage.Text = dr["FLDXACCUSAGE"].ToString();
                txtCurrency.Text = dr["FLDCURRENCYCODE"].ToString();
                txtBaseexchange.Text = string.Format(String.Format("{0:#####.00000000000000000}", decimal.Parse(dr["FLDBASEEXCHANGERATE"].ToString())));//
                txtReportexchange.Text = string.Format(String.Format("{0:#####.00000000000000000}", decimal.Parse(dr["FLDREPORTEXCHANGERATE"].ToString())));
                txtPrimeAmoutEdit.Text = string.Format(String.Format("{0:#####.00}", dr["FLDAMOUNT"]));
                txtChequeno.Text = dr["FLDXCHEQUENO"].ToString();
                txtLongDescription.Text = dr["FLDLONGDESCRITION"].ToString();
                txtUpdatedBy.Text = dr["FLDUPDATEDBYUSERNAME"].ToString();
                txtCreatedby.Text = dr["FLDCREATEDBY"].ToString();
                txtUpdatedDate.Text = dr["FLDUPDATEDDATE"].ToString();
                txtCreatedDate.Text = dr["FLDCREATEDDATE"].ToString();
                txtzid.Text = dr["FLDZID"].ToString();
                txtERMcurrency.Text = dr["FLDXCUR"].ToString();
                txtSubAccount.Text = dr["FLDXSUB"].ToString();
                txtERMlongdescription.Text = dr["FLDXLONG"].ToString();
                txtpayact.Text = dr["FLDXPAYACT"].ToString();
                txtERMSec.Text = dr["FLDXSEC"].ToString();
                txtBase.Text = dr["FLDBASEAMOUNT"].ToString();
                txtERMexchange.Text = string.Format(String.Format("{0:#####.00000000000000000}", decimal.Parse(dr["FLDXEXCH"].ToString())));
                txtERMexchangereport.Text = string.Format(String.Format("{0:#####.00000000000000000}", decimal.Parse(dr["FLDXEXCHR"].ToString())));
                txtERMPrime.Text = string.Format(String.Format("{0:#####.00}", dr["FLDXPRIME"]));
                txtBudget.Text = dr["FLDBUDGETID"].ToString();
                txtBaseBalance.Text = dr["FLDBASEBALANCE"].ToString();
                txtReportBalance.Text = dr["FLDREPORTBALANCE"].ToString();
                txtERMBase.Text = dr["FLDERMBASE"].ToString();
                txtERMReport.Text = dr["FLDERMREPORT"].ToString();
                txtERMTypeTransaction.Text = dr["FLDXTYPETRN"].ToString();
                txtERMManager.Text = dr["FLDXMANAGER"].ToString();
                txtERMStatus.Text = dr["FLDXSTATUS"].ToString();
                txtERMInvoiceNo.Text = dr["FLDXINVNUM"].ToString();
                txtERMReference.Text = dr["FLDXREF"].ToString();
                txtERMDate.Text = dr["FLDXDATE"].ToString();
                txtERMDateDue.Text = dr["FLDXDATEDUE"].ToString();
                txtCheque.Text = dr["FLDXCHEQUE"].ToString();
                txtERMDiv.Text = dr["FLDXDIV"].ToString();
                txtERMPer.Text = dr["FLDXPER"].ToString();
                txtERMYear.Text = dr["FLDXYEAR"].ToString();
                txtProject.Text = dr["FLDXPROJ"].ToString();
            }
     
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }



    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
       
    }
}



