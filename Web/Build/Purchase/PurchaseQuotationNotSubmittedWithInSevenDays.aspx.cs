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

public partial class PurchaseQuotationNotSubmittedWithInSevenDays : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddImageButton("../Purchase/PurchaseQuotationNotSubmittedWithInSevenDays.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Purchase/PurchaseQuotationNotSubmittedWithInSevenDays.aspx", "Clear Filter", "clear-filter.png", "CLEAR");

            MenuPurchasequotationnotsub.AccessRights = this.ViewState;
            MenuPurchasequotationnotsub.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucnoofdays.Text = "7";
                gvPurchasequotationnotsub.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void Purchasequotationnotsub_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {

                ViewState["PAGENUMBER"] = 1;
                gvPurchasequotationnotsub.Rebind();

            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                UcVesselSearch.SelectedVessel = "";
                ucfromdatesearch.Text = "";
                uctodatesearch.Text = "";
                ddlStockType.SelectedValue = "";
                ViewState["PAGENUMBER"] = 1;
                gvPurchasequotationnotsub.Rebind();
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

        DataSet ds = PheonixPurchaseQuotationNotSubmittedWithInSevenDays.SearchpurchaseQuotationNotSubmitted(General.GetNullableInteger(UcVesselSearch.SelectedVessel),
                    General.GetNullableDateTime(ucfromdatesearch.Text),
                    General.GetNullableDateTime(uctodatesearch.Text),
                    General.GetNullableString(ddlStockType.SelectedValue),
                    int.Parse(ucnoofdays.Text),
                    General.GetNullableInteger(ucZone.SelectedZone),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvPurchasequotationnotsub.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);


        gvPurchasequotationnotsub.DataSource = ds;
        gvPurchasequotationnotsub.VirtualItemCount = iRowCount;



        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void gvPurchasequotationnotsub_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPurchasequotationnotsub.CurrentPageIndex + 1;
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
        gvPurchasequotationnotsub.Rebind();
    }
    protected void ucZone_Changed(object sender, EventArgs e)
    {

        gvPurchasequotationnotsub.Rebind();
    }
    protected void gvPurchasequotationnotsub_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvPurchasequotationnotsub_ItemDataBound(object sender, GridItemEventArgs e)
    {
    }
    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        gvPurchasequotationnotsub.Rebind();
    }
    protected void Rebind()
    {
        gvPurchasequotationnotsub.SelectedIndexes.Clear();
        gvPurchasequotationnotsub.EditIndexes.Clear();
        gvPurchasequotationnotsub.DataSource = null;
        gvPurchasequotationnotsub.Rebind();
    }
}