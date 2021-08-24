using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Web.Profile;
using Telerik.Web.UI;

public partial class PurchaseOrderFormVendors : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddImageButton("../Purchase/PurchaseOrderFormVendors.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Purchase/PurchaseOrderFormVendors.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuPurchaseordervendor.AccessRights = this.ViewState;
            MenuPurchaseordervendor.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvPurchaseordervendor.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    UcVesselSearch.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    UcVesselSearch.Enabled = false;
                }
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Purchaseordervendor_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {

                ViewState["PAGENUMBER"] = 1;
                gvPurchaseordervendor.Rebind();

            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                UcVesselSearch.SelectedVessel = "";
                ucfromdatesearch.Text = "";
                uctodatesearch.Text = "";
                ddlStockType.SelectedValue = "";
                ucdecimalamount.Text = "";
                ViewState["PAGENUMBER"] = 1;
                gvPurchaseordervendor.Rebind();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ddlStockType.SelectedValue == "STORE")
        {
            DataSet ds = PhoenixPurchaseOrderFormVendors.Searchpurchaseorderstorevendorform(General.GetNullableInteger(UcVesselSearch.SelectedVessel),
                        General.GetNullableDateTime(ucfromdatesearch.Text),
                        General.GetNullableDateTime(uctodatesearch.Text),
                        General.GetNullableDecimal(ucdecimalamount.Text),
                        sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        gvPurchaseordervendor.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount);
            gvPurchaseordervendor.DataSource = ds;
            gvPurchaseordervendor.VirtualItemCount = iRowCount;
        }
        else if (ddlStockType.SelectedValue == "SERVICE")
        {
            DataSet ds = PhoenixPurchaseOrderFormVendors.Searchpurchaseorderservicevendorform(General.GetNullableInteger(UcVesselSearch.SelectedVessel),
                        General.GetNullableDateTime(ucfromdatesearch.Text),
                        General.GetNullableDateTime(uctodatesearch.Text),
                        General.GetNullableDecimal(ucdecimalamount.Text),
                        sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        gvPurchaseordervendor.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount);
            gvPurchaseordervendor.DataSource = ds;
            gvPurchaseordervendor.VirtualItemCount = iRowCount;
        }
        else
        {
            DataSet ds = PhoenixPurchaseOrderFormVendors.Searchpurchaseordervendorform(General.GetNullableInteger(UcVesselSearch.SelectedVessel),
                        General.GetNullableDateTime(ucfromdatesearch.Text),
                        General.GetNullableDateTime(uctodatesearch.Text),
                        General.GetNullableDecimal(ucdecimalamount.Text),
                        sortexpression, sortdirection,
                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                        gvPurchaseordervendor.PageSize,
                        ref iRowCount,
                        ref iTotalPageCount);
            gvPurchaseordervendor.DataSource = ds;
            gvPurchaseordervendor.VirtualItemCount = iRowCount;
        }





        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void Rebind()
    {
        gvPurchaseordervendor.SelectedIndexes.Clear();
        gvPurchaseordervendor.EditIndexes.Clear();
        gvPurchaseordervendor.DataSource = null;
        gvPurchaseordervendor.Rebind();
    }
    protected void gvPurchaseordervendor_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPurchaseordervendor.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlStockType_TextChanged(object sender, EventArgs e)
    {
        gvPurchaseordervendor.Rebind();
    }
    protected void gvPurchaseordervendor_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }
    protected void gvPurchaseordervendor_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }
    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        gvPurchaseordervendor.Rebind();
    }
}