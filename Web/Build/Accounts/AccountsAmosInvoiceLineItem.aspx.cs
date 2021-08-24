using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
public partial class AccountsAmosInvoiceLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        
        Title.AccessRights = this.ViewState;
        Title.Title = "Purchase Order Details";
        Title.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["ACTIVITYID"] = null;
            gvInvoice.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            if (ViewState["TOTALPAGECOUNT"] == null)
                ViewState["TOTALPAGECOUNT"] = 1;
            if (Request.QueryString["QINVOICELINEITEMCODE"] != null && Request.QueryString["qinvoicelineitemcode"] != string.Empty)
                ViewState["INVOICELINEITEMCODE"] = Request.QueryString["qinvoicelineitemcode"];
            if (Request.QueryString["qinvoicecode"] != null && Request.QueryString["qinvoicecode"] != string.Empty)
                ViewState["INVOICECODE"] = Request.QueryString["qinvoicecode"];
            InvoiceEdit();
        }
        //BindData();
        txtVendorId.Attributes.Add("style", "visibility:hidden");
        txtCurrencyId.Attributes.Add("style", "visibility:hidden");
    }

  
    protected void gvInvoice_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvInvoice_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblDescription = (RadLabel)e.Item.FindControl("lblDescription");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTip");
            lblDescription.Attributes.Add("onmouseover", "showTooltip(ev,'" + uct.ToolTip + "', 'visible');");
            lblDescription.Attributes.Add("onmouseout", "showTooltip(ev,'" + uct.ToolTip + "', 'hidden');");
        }

    }

    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (txtPOnumber.Text.Trim() != string.Empty)
            ViewState["PAGENUMBER"] = 1;

        ds = PhoenixAccountsInvoice.AmosFormSearch(General.GetNullableString(txtPOnumber.Text.Trim())
                                                            , General.GetNullableInteger(txtVendorId.Text)
                                                            , int.Parse(txtCurrencyId.Text)
                                                            , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                            , sortexpression
                                                            , sortdirection
                                                            , (int)ViewState["PAGENUMBER"]
                                                            , gvInvoice.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount);
        gvInvoice.DataSource = ds;
        gvInvoice.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void InvoiceEdit()
    {
        if (ViewState["INVOICECODE"] != null)
        {

            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["INVOICECODE"].ToString()));
            if (dsInvoice.Tables.Count > 0)
            {
                DataRow drInvoice = dsInvoice.Tables[0].Rows[0];
                txtInvoiceNumber.Text = drInvoice["FLDINVOICENUMBER"].ToString();
                txtSupplierRefEdit.Text = drInvoice["FLDINVOICESUPPLIERREFERENCE"].ToString();
                txtVendorId.Text = drInvoice["FLDADDRESSCODE"].ToString();
                txtVendorCode.Text = drInvoice["FLDCODE"].ToString();
                txtVenderName.Text = drInvoice["FLDNAME"].ToString();
                txtCurrencyId.Text = drInvoice["FLDCURRENCY"].ToString();
                txtCurrencyName.Text = drInvoice["FLDCURRENCYCODE"].ToString();
                ViewState["SHORTNAME"] = drInvoice["FLDSHORTNAME"].ToString();
            }
        }
    }




    protected void Invoice_SetVessel(object sender, EventArgs e)
    {
        if (ucVessel.SelectedVessel.ToUpper() != "DUMMY")
        {
            Rebind();
        }
    }


    public bool IsValidLineItem()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        return (!ucError.IsError);
    }

    protected void InvoiceLineItemSelected(object sender, EventArgs e)
    {
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1', null, true);";
        Script += "</script>" + "\n";

        LinkButton lk = (LinkButton)sender;

        string strOrderFormNo = lk.Text;
        string strOrderId = lk.CommandArgument;
        try
        {
            //if (ViewState["SHORTNAME"].ToString() == "SPI")
            //{
            PhoenixAccountsInvoice.InvoiceLineItemInsertAMOS(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["INVOICECODE"].ToString()), new Guid(strOrderId), strOrderFormNo);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            Rebind();
            //}

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void gvInvoice_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInvoice.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvInvoice.SelectedIndexes.Clear();
        gvInvoice.EditIndexes.Clear();
        gvInvoice.DataSource = null;
        gvInvoice.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["ORDERID"] = null;
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
