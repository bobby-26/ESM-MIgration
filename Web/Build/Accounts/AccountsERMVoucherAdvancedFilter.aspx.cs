using System;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Web.UI;
using Telerik.Web.UI;

public partial class AccountsERMVoucherFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Cancel", "CANCEL",ToolBarDirection.Right);
        toolbar.AddButton("Go", "GO",ToolBarDirection.Right);
        

        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.Title = "Vouchers Advance Filter";
        MenuOfficeFilterMain.MenuList = toolbar.Show();
       
       

        imgShowAccount.Attributes.Add("onclick", "return showPickList('spnPickListCreditAccount', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCompanyAccount.aspx', true); ");
    }
    
    
    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;


        //string Script = "";
        //Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        //Script += "fnReloadList();";
        //Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (CommandName.ToUpper().Equals("GO"))
        {
 
            criteria.Clear();
            criteria.Add("txtEsmBudgetCode", txtEsmBudgetCode.Text);
            criteria.Add("txtOwnerBudgetCode", txtOwnerBudgetCode.Text);
            criteria.Add("txtVoucherDateFrom", txtVoucherDateFrom.Text);
            criteria.Add("txtVoucherDateTo", txtVoucherDateTo.Text);
            criteria.Add("txtAccountCode", txtAccountCode.Text);
            criteria.Add("txtDebitNoteReference", txtDebitNoteReference.Text);
            criteria.Add("txtVoucherNumber", txtVoucherNumber.Text);
            criteria.Add("chkShowAll", chkShowAll.Checked == true ? "1" : "0");
            criteria.Add("SearchType", "FINDADVANCED");

            Filter.CurrentSelectedERMVoucher = criteria;

            //String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

        }
        else if (CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            criteria.Add("SearchType", "FINDADVANCED");
            Filter.CurrentSelectedERMVoucher = criteria;
            
            //String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }

        if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper() == "UNBILLEDADVANCED")
        {
            Response.Redirect("../Accounts/AccountsERMVoucherDetail.aspx");
        }
        else if (Request.QueryString["callfrom"] != null && Request.QueryString["callfrom"].ToUpper() == "BILLEDADVANCED")
        {
            Response.Redirect("../Accounts/AccountsERMVoucherDetailDebitNoteReference.aspx");
        }
    }    
}



