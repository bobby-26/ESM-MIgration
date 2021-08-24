using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsInvoiceTypeByPO : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Invoice Type Basis PO Number", "TITLE");

            Title.AccessRights = this.ViewState;
            Title.MenuList = toolbarmain.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = null;
                if (Request.QueryString["ponumber"] != null)
                {
                    ViewState["ponumber"] = Request.QueryString["ponumber"].ToString().Trim();
                    //ucTitle.Text = ucTitle.Text + " (" + Request.QueryString["ponumber"].ToString().Trim() + ") ";
                }
                else
                    ViewState["ponumber"] = "";
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        DataSet ds = new DataSet();

        ds = PhoenixAccountsInvoice.InvoiceTypeByPOList(ViewState["ponumber"].ToString());

        gvInvoiceTypePO.DataSource = ds;
        gvInvoiceTypePO.DataBind();

    }

    protected void Title_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("TITLE"))
        {
        }
    }

}
