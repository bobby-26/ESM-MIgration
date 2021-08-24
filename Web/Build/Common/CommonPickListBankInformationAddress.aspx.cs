using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Drawing;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class CommonPickListBankInformationAddress : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuComponent.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["addresscode"] = Request.QueryString["addresscode"].ToString();
            if (Request.QueryString["currency"] != "" && Request.QueryString["currency"] != null)
            {
                ucCurrencySearch.DataBind();
                ucCurrencySearch.SelectedCurrency = Request.QueryString["currency"].ToString();
            }
        }
    }

    protected void MenuComponent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds;
            ds = PhoenixRegistersBankInformationAddress.BankInformationSupplierSearch(0, General.GetNullableString(txtBankNameSearch.Text), General.GetNullableInteger(ucCurrencySearch.SelectedCurrency), General.GetNullableString(txtAccountNoSearch.Text), General.GetNullableInteger(ViewState["addresscode"].ToString()), Int32.Parse(ViewState["PAGENUMBER"].ToString()),
               General.ShowRecords(null),
               ref iRowCount,
               ref iTotalPageCount);

            gvBank.DataSource = ds;
            gvBank.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvBank_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            string Script = "";
            NameValueCollection nvc;

            if (Request.QueryString["mode"] == "custom")
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();

                LinkButton lnkAccountNo = (LinkButton)e.Item.FindControl("lnkAccountNo");
                nvc.Add(lnkAccountNo.ID, lnkAccountNo.Text.ToString());
                RadLabel lblBankName = (RadLabel)e.Item.FindControl("lblBankName");
                nvc.Add(lnkAccountNo.ID, lnkAccountNo.Text.ToString());
                RadLabel lblBankId = (RadLabel)e.Item.FindControl("lblBankID");
                nvc.Add(lblBankId.ID, lblBankId.Text);


            }
            else
            {
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = Filter.CurrentPickListSelection;

                LinkButton lnkAccountNo = (LinkButton)e.Item.FindControl("lnkAccountNo");
                nvc.Set(nvc.GetKey(1), lnkAccountNo.Text.ToString());
                RadLabel lblBankName = (RadLabel)e.Item.FindControl("lblBankName");
                nvc.Set(nvc.GetKey(2), lblBankName.Text.ToString());
                RadLabel lblBankId = (RadLabel)e.Item.FindControl("lblBankID");
                nvc.Set(nvc.GetKey(3), lblBankId.Text.ToString());
            }
            //Filter.CurrentPickListSelection = nvc;
            //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            Filter.CurrentPickListSelection = nvc;
            RadScriptManager.RegisterStartupScript(Page, typeof(Page), "BookMarkScript", Script, false);
        }

    }

    protected void gvBank_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            int? n = General.GetNullableInteger(drv["FLDACTIVEYN"].ToString());
            LinkButton lnkAccountNo = (LinkButton)e.Item.FindControl("lnkAccountNo");
            RadLabel lblAccountNo = (RadLabel)e.Item.FindControl("lblAccountNo");

            if (n == 1 && lnkAccountNo != null)
            {
                //lnkAccountNo.Enabled = false;
                //lnkAccountNo.ForeColor = Color.Black;
                lblAccountNo.Visible = true;
                lnkAccountNo.Visible = false;
            }

        }
    }
    protected void gvBank_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
