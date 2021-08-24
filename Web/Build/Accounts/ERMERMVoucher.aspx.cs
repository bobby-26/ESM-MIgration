using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class ERMERMVoucher : PhoenixBasePage
{
    public int iUserCode;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        MenuVoucher.AccessRights = this.ViewState;
        MenuVoucher.Title= "ERM Voucher      (" + PhoenixAccountsVoucher.VoucherNumber + ")     ";
        MenuVoucher.MenuList = toolbar.Show();

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
            VoucherEdit();
            if (ViewState["VOUCHERID"] != null)
            {
               // ttlVoucher.Text = "ERM Voucher      (" + PhoenixAccountsVoucher.VoucherNumber + ")     ";
            }
        }
        for (int i = 0; i < Session.Contents.Count; i++)
        {
            if (Session.Keys[i].ToString().StartsWith("VOUCHERCURRENCYID"))
                Session.Remove(Session.Keys[i].ToString());
        }
    }

    protected void Voucher_TabStripCommand(object sender, EventArgs e)
    {

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void VoucherEdit()
    {
        if (ViewState["VOUCHERID"] != null)
        {

            DataSet ds = PhoenixAccountsVoucher.ERMVoucherEdit(ViewState["VOUCHERID"].ToString());

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    txtERMVoucherNumber.Text = dr["FLDXVOUCHER"].ToString();
                    txtVoucherNumber.Text = dr["FLDVOUCHERNUMBER"].ToString();
                    txtReferenceNumber.Text = dr["FLDREFERENCEDOCUMENTNO"].ToString();
                    txtUpdatedBy.Text = dr["FLDLASTUPDATEBYUSERNAME"].ToString();
                    txtUpdatedDate.Text = dr["FLDUPDATEDDATE"].ToString();
                    txtCreatedby.Text = dr["FLDCREATEDBY"].ToString();
                    txtStatus.Text = "BALANCED";
                    txtzid.Text = dr["FLDZID"].ToString();
                    txtCreatedDate.Text = dr["FLDCREATEDDATE"].ToString();
                    txtERMPer.Text = dr["FLDXPER"].ToString();
                    txtERMYear.Text = dr["FLDXYEAR"].ToString();
                    txtERMDateDue.Text = dr["FLDXDATEDUE"].ToString();
                    txtERMTeam.Text = dr["FLDXTEAM"].ToString();
                    txtERMAccses.Text = dr["FLDXACCESS"].ToString();
                    txtERMMember.Text = dr["FLDXMEMBER"].ToString();
                    txtERMManager.Text = dr["FLDXMANAGER"].ToString();
                    txtVoucherPay.Text = dr["FLDXVOUCHERPAY"].ToString();
                    txtCompanyname.Text = dr["FLDCOMPANYNAME"].ToString();
                    txtERMLongDescription.Text = dr["FLDXLONG"].ToString();
                    txtLongDescription.Text = dr["FLDLONGDESCRIPTION"].ToString();
                }
            }
        }
    }



    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
