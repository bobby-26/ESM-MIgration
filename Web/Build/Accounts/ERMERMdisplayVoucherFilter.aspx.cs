using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;

public partial class ERMERMdisplayVoucherFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Go", "GO");
        toolbar.AddButton("Cancel", "CANCEL");

        MenuOfficeFilterMain.AccessRights = this.ViewState;
        MenuOfficeFilterMain.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
           
            txtErmvoucherno.Focus();
            //ucInvoiceStatus.HardTypeCode = ((int)PhoenixHardTypeCode.VOUCHERSTATUS).ToString();
        }
    }


    protected void OfficeFilterMain_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
        Script += "fnReloadList();";
        Script += "</script>" + "\n";

        NameValueCollection criteria = new NameValueCollection();

        if (dce.CommandName.ToUpper().Equals("GO"))
        {

            criteria.Clear();
            criteria.Add("txtErmvoucherno", txtErmvoucherno.Text);
            criteria.Add("txtEsmvoucherno", txtEsmvoucherno.Text);
            criteria.Add("txtVoucherFromdateSearch", txtVoucherFromdateSearch.Text);
            criteria.Add("txtVoucherTodateSearch", txtVoucherTodateSearch.Text);
            criteria.Add("txtReferenceNumberSearch", txtReferenceNumberSearch.Text);
            //criteria.Add("txtVoucherNumber", txtVoucherNumber.Text);


            Filter.CurrentSelectedERMVoucherfilter = criteria;

            String script = String.Format("javascript:fnReloadList('codehelp1',null,null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

        }
        else if (dce.CommandName.ToUpper().Equals("CANCEL"))
        {
            criteria.Clear();
            Filter.CurrentInvoiceSelection = criteria;
        }

        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }
}
