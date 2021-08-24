using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PurchaseComercialPOAdd : PhoenixBasePage
{
    string vesselname;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Purchase/PurchaseComercialPOAdd.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Purchase/PurchaseComercialPOAdd.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbar.AddImageButton("../Purchase/PurchaseComercialPOAdd.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('rgvForm')", "Print Grid", "icon_print.png", "PRINT");
            MenuNewRequisition.AccessRights = this.ViewState;
            MenuNewRequisition.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                VesselConfiguration();
                MenuNewRequisition.SelectedMenuIndex = 0;
                if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
                    Response.Redirect("PhoenixLogout.aspx");
                if (Request.QueryString["pageno"] != null)
                {
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["pageno"].ToString());
                    rgvForm.CurrentPageIndex = int.Parse(Request.QueryString["pageno"].ToString()) - 1;
                }
                else
                    ViewState["PAGENUMBER"] = 1;//INVOICEID
                ViewState["INVOICENO"] = "";
                if (Request.QueryString["INVOICEID"] != null)
                    ViewState["INVOICENO"] = Request.QueryString["INVOICEID"];

                    rgvForm.PageSize = General.ShowRecords(null);

                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixCommonPurchase.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            rgvForm.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuNewRequisition_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                rgvForm.CurrentPageIndex = 0;
                rgvForm.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFormNo.Text = "";
                txtTitle.Text = "";
                ucVessel.SelectedVessel = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                ddlStockType.SelectedValue = "";
                ucAddrAgent.SelectedValue = "";
                ucAddrAgent.Text = "";
                rgvForm.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlStockType_TextChanged(object sender, EventArgs e)
    {
        rgvForm.Rebind();
    }

    protected void rgvForm_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSTOCKTYPE", "FLDFORMNO", "FLDTITLE", "FLDSUBACCOUNT", "FLDVENDORNAME", "FLDACTUALTOTAL", "FLDAMOUNTINUSD" };
        string[] alCaptions = { "Type", "Number", "Title", "Budget Code", "Vendor", "Quoted Price", "Committed(USD)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : ViewState["SORTEXPRESSION"].ToString();


        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            vesselname = ucVessel.SelectedVessel.ToString();

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseComercialInvoice.PoIssuedComercialSearch(General.GetNullableInteger(vesselname), txtFormNo.Text.Trim(), txtTitle.Text.Trim()
                                                                , General.GetNullableDateTime(txtFromDate.Text)
                                                                , General.GetNullableDateTime(txtToDate.Text)
                                                                , General.GetNullableInteger( ucAddrAgent.SelectedValue)
                                                                , General.GetNullableString(ddlStockType.SelectedValue)
                                                                , sortexpression
                                                                , sortdirection
                                                                , rgvForm.CurrentPageIndex + 1
                                                                , rgvForm.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        rgvForm.DataSource = ds;
        rgvForm.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("rgvForm", "PO Issued", alCaptions, alColumns, ds);
    }

    protected void rgvForm_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
           
        }
    }

    protected void rgvForm_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("ADD"))
        {
            GridDataItem item = (GridDataItem)e.Item;
            Guid? orderid = General.GetNullableGuid(e.CommandArgument.ToString());
            PhoenixPurchaseComercialInvoice.InsertComercialInvoiceLineItem(General.GetNullableGuid(ViewState["INVOICENO"].ToString()), orderid);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('codehelp3', 'codehelp1');", true);
            rgvForm.Rebind();
        }
    }

    protected void rgvForm_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        rgvForm.Rebind();
    }

    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        rgvForm.Rebind();
    }
}
