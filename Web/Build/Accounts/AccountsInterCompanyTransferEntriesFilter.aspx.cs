using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using System.Data;
using System.Text;
using Telerik.Web.UI;

public partial class AccountsInterCompanyTransferEntriesFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
     
        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
        {           
            
            txtVoucherNumber.Focus();
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

           
            if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"] != string.Empty)
                ViewState["callfrom"] = Request.QueryString["callfrom"];
            else
                ViewState["callfrom"] = null;
            BindHard();
            BindUsers();
            BindAccountCheckBoxList();
            BindCurrency();


        }       
    }
    protected void BindHard()
    {
        ddlVoucherStatus.HardTypeCode = "155";
        ddlVoucherStatus.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,154);
    }
    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
            StringBuilder struserlist = new StringBuilder();
            StringBuilder strcurrencylist = new StringBuilder();
            StringBuilder straccountlist = new StringBuilder();

             //To get the selected Users list 

            foreach (ListItem item in chkUserList.Items)
            {
                if (item.Selected == true)
                {
                    struserlist.Append(item.Value.ToString());
                    struserlist.Append(",");
                }
            }
            if (struserlist.Length > 1)
            {
                struserlist.Remove(struserlist.Length - 1, 1);
            }
            if (struserlist.ToString().Contains("Dummy"))
            {
                struserlist = new StringBuilder();
                struserlist.Append("Dummy");

            }
            
            //To get the selected currency list 

            foreach (ListItem item in chkCurrencyList.Items)
            {
                if (item.Selected == true)
                {
                    strcurrencylist.Append(item.Value.ToString());
                    strcurrencylist.Append(",");
                }
            }
            if (strcurrencylist.Length > 1)
            {
                strcurrencylist.Remove(strcurrencylist.Length - 1, 1);
            }
            if (strcurrencylist.ToString().Contains("Dummy"))
            {
                strcurrencylist = new StringBuilder();
                strcurrencylist.Append("Dummy");

            }

            //To get the selected accounts list 
           
            foreach (ListItem item in chkAccountList.Items)
            {
                if (item.Selected == true)
                {
                    straccountlist.Append(item.Value.ToString());
                    straccountlist.Append(",");
                }
            }
            if (straccountlist.Length > 1)
            {
                straccountlist.Remove(straccountlist.Length - 1, 1);
            }
            if (straccountlist.ToString().Contains("Dummy"))
            {
                straccountlist = new StringBuilder();
                straccountlist.Append("Dummy");
            }

            criteria.Clear();
            criteria.Add("txtVoucherNumber", txtVoucherNumber.Text.Trim());
            criteria.Add("txtRefenceNumber", txtRefenceNumber.Text.Trim());
            criteria.Add("txtVoucherLongDescription", txtVoucherLongDescription.Text.Trim());
            criteria.Add("txtLineItemLongDescription", txtLineItemLongDescription.Text.Trim());
            criteria.Add("txtVoucherFromdate", txtVoucherFromdate.Text);
            criteria.Add("txtVoucherTodate", txtVoucherTodate.Text);
            criteria.Add("txtAmountFrom", txtAmountFrom.Text);
            criteria.Add("txtAmountTo", txtAmountTo.Text);
            criteria.Add("txtAccountCode", straccountlist.ToString());
            criteria.Add("userlist", struserlist.ToString());
            criteria.Add("ddlCurrencyCode", strcurrencylist.ToString());
            if (ddlVoucherStatus.SelectedHard.ToString() == "Dummy")
                criteria.Add("voucherstatus", null);
            else
                criteria.Add("voucherstatus", ddlVoucherStatus.SelectedHard);

            Filter.CurrentInterCompanyEntriesSelection = criteria;
            if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "pendingvouchers")
                Response.Redirect("../Accounts/AccountsInterCompanyTransferPendingVoucherList.aspx", false);
            else
                Response.Redirect("../Accounts/AccountsInterCompanyTransferEntriesList.aspx", false);
        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentInterCompanyEntriesSelection = criteria;
            if (ViewState["callfrom"] != null && ViewState["callfrom"].ToString() == "pendingvouchers")
                Response.Redirect("../Accounts/AccountsInterCompanyTransferPendingVoucherList.aspx", false);
            else
                Response.Redirect("../Accounts/AccountsInterCompanyTransferEntriesList.aspx", false);
        }

    }
    protected void BindAccountCheckBoxList()
    {
        DataSet ds = new DataSet();

            int iRowCount = 0;
            int iTotalPageCount = 0;
            
            ds = PhoenixRegistersAccount.AccountSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                    , null
                                                    , null
                                                    , null
                                                    , 74
                                                    , null
                                                    , 1
                                                    , null, null,
                                                    1,
                                                    1000,
                                                    ref iRowCount,
                                                    ref iTotalPageCount);
        
        ds.Tables[0].Columns.Add("FLDaccoandept");

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            dr["FLDaccoandept"] = dr["FLDACCOUNTCODE"] + "-" + dr["FLDDESCRIPTION"];

        }
        chkAccountList.Items.Add("select");
        chkAccountList.DataTextField = "FLDaccoandept";
        chkAccountList.DataValueField = "FLDACCOUNTID";
        chkAccountList.DataSource = ds;
        chkAccountList.DataBind();
        chkAccountList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    private void BindUsers()
    {
        DataSet ds = PhoenixAccountsVoucher.VoucherUserList();

        chkUserList.Items.Add("select");
        chkUserList.DataSource = ds;
        chkUserList.DataTextField = "FLDUSERNAME";
        chkUserList.DataValueField = "FLDUSERCODE";
        chkUserList.DataBind();
        chkUserList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    private void BindCurrency()
    {
        DataSet ds = PhoenixRegistersCurrency.ListCurrency(1);

        chkCurrencyList.Items.Add("select");
        chkCurrencyList.DataSource = ds;
        chkCurrencyList.DataTextField = "FLDCURRENCYCODE";
        chkCurrencyList.DataValueField = "FLDCURRENCYID";
        chkCurrencyList.DataBind();
        chkCurrencyList.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }     
}
