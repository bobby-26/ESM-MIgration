using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class AccountsInvoiceLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        TitleTab.AccessRights = this.ViewState;
        TitleTab.Title = "Purchase Order Details";
        TitleTab.MenuList = toolbar.Show();
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["ACTIVITYID"] = null;

            if (ViewState["TOTALPAGECOUNT"] == null)
                ViewState["TOTALPAGECOUNT"] = 1;
            if (Request.QueryString["QINVOICELINEITEMCODE"] != null && Request.QueryString["qinvoicelineitemcode"] != string.Empty)
                ViewState["INVOICELINEITEMCODE"] = Request.QueryString["qinvoicelineitemcode"];
            if (Request.QueryString["qinvoicecode"] != null && Request.QueryString["qinvoicecode"] != string.Empty)
            {
                ViewState["INVOICECODE"] = Request.QueryString["qinvoicecode"];

                DataSet ds = PhoenixAccountsInvoice.InvoiceEdit(new Guid(ViewState["INVOICECODE"].ToString()));

                if (ds.Tables[0].Rows.Count > 0 && (ds.Tables[0].Rows[0]["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 59, "SPI")
                                                    || ds.Tables[0].Rows[0]["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 59, "BNP")
                                                    || ds.Tables[0].Rows[0]["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 59, "PCD")
                                                    || ds.Tables[0].Rows[0]["FLDINVOICETYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 59, "QTY"))
                                                    )
                {
                    ucStoreType.Enabled = true;
                }
                else
                {
                    ucStoreType.Enabled = false;
                }
            }

            InvoiceEdit();
        }
        // BindData();
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
        Rebind();
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

        if (ViewState["SHORTNAME"].ToString() == "SPI" || ViewState["SHORTNAME"].ToString() == "QTY" || ViewState["SHORTNAME"].ToString() == "CRW" || ViewState["SHORTNAME"].ToString() == "AGY")
        {
            ds = PhoenixAccountsInvoice.OrderFormSearch(General.GetNullableString(txtPOnumber.Text.Trim())
                                                                , General.GetNullableInteger(txtVendorId.Text)
                                                                , int.Parse(txtCurrencyId.Text)
                                                                , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                , General.GetNullableInteger(ucStoreType.SelectedHard)
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

        else if (ViewState["SHORTNAME"].ToString() == "AVN")
        {
            ds = PhoenixAccountsInvoice.AviationRequestSearch(General.GetNullableString(txtPOnumber.Text.Trim())
                                                               , General.GetNullableInteger(txtVendorId.Text)
                                                               , int.Parse(txtCurrencyId.Text)
                                                               , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                               , sortexpression
                                                               , sortdirection
                                                               , (int)ViewState["PAGENUMBER"], gvInvoice.PageSize
                                                               , ref iRowCount
                                                               , ref iTotalPageCount);


            gvInvoice.DataSource = ds;
            gvInvoice.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        else if (ViewState["SHORTNAME"].ToString() == "MDL")
        {
            ds = PhoenixAccountsInvoice.MedicalRequestSearch(General.GetNullableString(txtPOnumber.Text.Trim())
                                                               , General.GetNullableInteger(txtVendorId.Text)
                                                               , int.Parse(txtCurrencyId.Text)
                                                               , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                               , sortexpression
                                                               , sortdirection
                                                               , (int)ViewState["PAGENUMBER"], gvInvoice.PageSize
                                                               , ref iRowCount
                                                               , ref iTotalPageCount);


            gvInvoice.DataSource = ds;
            gvInvoice.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        else if (ViewState["SHORTNAME"].ToString() == "LIC")
        {
            ds = PhoenixAccountsInvoice.LicenceRequestSearch(General.GetNullableString(txtPOnumber.Text.Trim())
                                                               , General.GetNullableInteger(txtVendorId.Text)
                                                               , int.Parse(txtCurrencyId.Text)
                                                               , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                               , sortexpression
                                                               , sortdirection
                                                               , (int)ViewState["PAGENUMBER"], gvInvoice.PageSize
                                                               , ref iRowCount
                                                               , ref iTotalPageCount);


            gvInvoice.DataSource = ds;
            gvInvoice.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        else if (ViewState["SHORTNAME"].ToString() == "WGR")
        {
            ds = PhoenixAccountsInvoice.WorkingGearRequestSearch(General.GetNullableString(txtPOnumber.Text.Trim())
                                                               , General.GetNullableInteger(txtVendorId.Text)
                                                               , int.Parse(txtCurrencyId.Text)
                                                               , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                               , sortexpression
                                                               , sortdirection
                                                               , (int)ViewState["PAGENUMBER"], gvInvoice.PageSize
                                                               , ref iRowCount
                                                               , ref iTotalPageCount);


            gvInvoice.DataSource = ds;
            gvInvoice.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        else if (ViewState["SHORTNAME"].ToString() == "BNP")
        {
            ds = PhoenixAccountsInvoice.BondProvisionSearch(General.GetNullableString(txtPOnumber.Text.Trim())
                                                                , General.GetNullableInteger(txtVendorId.Text)
                                                                , int.Parse(txtCurrencyId.Text)
                                                                , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                , General.GetNullableInteger(ucStoreType.SelectedHard)
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"], gvInvoice.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

            gvInvoice.DataSource = ds;
            gvInvoice.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        else if (ViewState["SHORTNAME"].ToString() == "PCD")
        {
            ds = PhoenixAccountsInvoice.PhoneCardSearch(General.GetNullableString(txtPOnumber.Text.Trim())
                                                                , General.GetNullableInteger(txtVendorId.Text)
                                                                , int.Parse(txtCurrencyId.Text)
                                                                , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                , General.GetNullableInteger(ucStoreType.SelectedHard)
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
        else
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("FLDFORMNO");
            dt.Columns.Add("FLDVESSELNAME");
            dt.Columns.Add("FLDACTUALTOTAL");
            dt.Columns.Add("FLDPARTPAYMENT");
            dt.Columns.Add("FLDORDERID");
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
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
            BindData();
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
            if (ViewState["SHORTNAME"].ToString() == "SPI" || ViewState["SHORTNAME"].ToString() == "QTY" || ViewState["SHORTNAME"].ToString() == "CRW" || ViewState["SHORTNAME"].ToString() == "AGY")
            {
                PhoenixAccountsInvoice.InvoiceLineItemInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["INVOICECODE"].ToString()), new Guid(strOrderId));
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                BindData();
                //  PhoenixAccountsPOStaging.OrderFormStagingInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(strOrderId));
            }
            if (ViewState["SHORTNAME"].ToString() == "MDL")
            {
                PhoenixAccountsInvoice.InvoiceLineItemInsertMedical(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["INVOICECODE"].ToString()), new Guid(strOrderId));
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                gvInvoice.Rebind();
                PhoenixAccountsPOStaging.OrderFormStagingInsertMedical(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(strOrderId));
            }
            if (ViewState["SHORTNAME"].ToString() == "WGR")
            {
                PhoenixAccountsInvoice.InvoiceLineItemInsertWorkingGear(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["INVOICECODE"].ToString()), new Guid(strOrderId));
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                gvInvoice.Rebind();
                PhoenixAccountsPOStaging.OrderFormStagingInsertWorkingGear(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(strOrderId));
            }
            if (ViewState["SHORTNAME"].ToString() == "LIC")
            {
                PhoenixAccountsInvoice.InvoiceLineItemInsertLicence(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["INVOICECODE"].ToString()), new Guid(strOrderId));
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                gvInvoice.Rebind();
                PhoenixAccountsPOStaging.OrderFormStagingInsertLicence(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(strOrderId));
            }
            if (ViewState["SHORTNAME"].ToString() == "AVN")
            {
                PhoenixAccountsInvoice.InvoiceLineItemInsertAviation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["INVOICECODE"].ToString()), new Guid(strOrderId));
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                gvInvoice.Rebind();
                PhoenixAccountsPOStaging.OrderFormStagingInsertAviation(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(strOrderId));
            }
            if (ViewState["SHORTNAME"].ToString() == "BNP")
            {
                PhoenixAccountsInvoice.InvoiceLineItemInsertBondProvision(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["INVOICECODE"].ToString()), new Guid(strOrderId));
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                gvInvoice.Rebind();
                PhoenixAccountsPOStaging.OrderFormStagingInsertBondProvision(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(strOrderId));
            }
            if (ViewState["SHORTNAME"].ToString() == "PCD")
            {
                PhoenixAccountsInvoice.InvoiceLineItemInsertPhoneCard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["INVOICECODE"].ToString()), new Guid(strOrderId));
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                gvInvoice.Rebind();
                PhoenixAccountsPOStaging.OrderFormStagingInsertPhoneCard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(strOrderId));
            }
            Rebind();
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
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void Rebind()
    {
        gvInvoice.SelectedIndexes.Clear();
        gvInvoice.EditIndexes.Clear();
        gvInvoice.DataSource = null;
        gvInvoice.Rebind();
    }
}
