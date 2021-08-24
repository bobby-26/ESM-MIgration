using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;

public partial class PurchaseVendorProductZone : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        

        if (!IsPostBack)
        {
            gvVendorProductZone.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            if (Request.QueryString["contractid"] != null && Request.QueryString["contractid"].ToString() != null)
            {
                ViewState["contractid"] = Request.QueryString["contractid"].ToString();
            }

            if (Request.QueryString["vendorid"].ToString() != null)
            {
                ViewState["vendorid"] = Request.QueryString["vendorid"].ToString();
                //ucVendorZone.SelectedVendor = int.Parse(ViewState["vendorid"].ToString());

                ucVendorZone.DataSource = PhoenixPurchaseVendorZone.VendorZoneList(0, int.Parse(ViewState["vendorid"].ToString()));
                ucVendorZone.DataBind();

                ucPort.DataSource = PhoenixPurchaseZonePortMapping.ZonePortMappingList(General.GetNullableInteger(ucVendorZone.SelectedValue));
                ucPort.DataBind();
            }

            

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            EnablePackage();
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Purchase/PurchaseVendorProductZone.aspx?contractid=" + ViewState["contractid"].ToString() + "&vendorid=" + ViewState["vendorid"].ToString() + "", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvVendorProductZone')", "Print Grid", "icon_print.png", "PRINT");
        MenuRegistersVendorProductZone.AccessRights = this.ViewState;
        MenuRegistersVendorProductZone.MenuList = toolbar.Show();

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("General", "CONTRACT");
        toolbarsub.AddButton("Item", "MAPPING");
        //toolbarsub.AddButton("Delivery", "DELIVERY");
        MenuContractItem.MenuList = toolbarsub.Show();
        MenuContractItem.SelectedMenuIndex = 1;
        //BindData();
        //SetPageNavigator();
    }

    private void EnablePackage()
    {
        DataSet ds = PhoenixPurchaseContract.EditContract(new Guid(ViewState["contractid"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ucPackage.Visible = dr["FLDPACKAGEYESNO"].ToString() == "1" ? true : false;
            lblPackage.Visible = dr["FLDPACKAGEYESNO"].ToString() == "1" ? true : false;
        }
    }

    protected void MenuContractItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CONTRACT"))
            {
                if (ViewState["contractid"].ToString() == "0")
                    Response.Redirect("../Purchase/PurchaseContract.aspx");
                else
                    Response.Redirect("../Purchase/PurchaseContract.aspx?contractid=" + ViewState["contractid"].ToString() + "&vendorid=" + ViewState["vendorid"].ToString());
            }
            if (CommandName.ToUpper().Equals("DELIVERY"))
            {
                if (ViewState["contractid"].ToString() == "0")
                    Response.Redirect("../Purchase/PurchaseContractDeliveryLocation.aspx");
                else
                    Response.Redirect("../Purchase/PurchaseContractDeliveryLocation.aspx?contractid=" + ViewState["contractid"].ToString() + "&vendorid=" + ViewState["vendorid"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucSeaport_DataBound(object sender, EventArgs e)
    {
        ucPort.Items.Insert(0, new RadComboBoxItem("--select--", "Dummy"));
    }

    protected void VendorZone_Changed(object sender, EventArgs e)
    {
        ucPort.DataSource = PhoenixPurchaseZonePortMapping.ZonePortMappingList(General.GetNullableInteger(ucVendorZone.SelectedValue));
        ucPort.DataBind();
        gvVendorProductZone.DataSource = null;
        gvVendorProductZone.Rebind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDPRODUCTCODE", "FLDPRODUCTNAME", "FLDPRODUCTGROUP", "FLDDESCRIPTION", "FLDUNITNAME", "FLDPRICE", "FLDDISCOUNT" };
        string[] alCaptions = { "Product Code", "Product Name", "Product Group", "Description", "Unit", "Price", "Discount" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseVendorProductZone.VendorZoneProductSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(ViewState["contractid"].ToString()),
            General.GetNullableInteger(ViewState["vendorid"].ToString()),
            General.GetNullableInteger(ucVendorZone.SelectedValue),
            General.GetNullableInteger(ucPort.SelectedValue),
            General.GetNullableInteger(ucPackage.SelectedHard),
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ProductPrice.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Product Price</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void RegistersVendorProductZone_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVendorProductZone.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDPRODUCTCODE", "FLDPRODUCTNAME", "FLDPRODUCTGROUP", "FLDDESCRIPTION", "FLDUNITNAME", "FLDPRICE", "FLDDISCOUNT" };
        string[] alCaptions = { "Product Code", "Product Name", "Product Group", "Description", "Unit", "Price", "Discount" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixPurchaseVendorProductZone.VendorZoneProductSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(ViewState["contractid"].ToString()),
            General.GetNullableInteger(ViewState["vendorid"].ToString()),
            General.GetNullableInteger(ucVendorZone.SelectedValue),
            General.GetNullableInteger(ucPort.SelectedValue),
            General.GetNullableInteger(ucPackage.SelectedHard),
            sortexpression, sortdirection, (int)ViewState["PAGENUMBER"], gvVendorProductZone.PageSize, ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvVendorProductZone", "Product Price", alCaptions, alColumns, ds);

        gvVendorProductZone.DataSource = ds;
        gvVendorProductZone.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvVendorProductZone_Sorting(object sender, GridSortCommandEventArgs e)
    {

        
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
        gvVendorProductZone.Rebind();
    }

  
    protected void gvVendorProductZone_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string ss1 = "";
                RadLabel ss = (RadLabel)e.Item.FindControl("lblZoneProductMappingId");
                if (ss != null)
                    ss1 = ss.Text;
                DeleteZoneProductPrice(((RadLabel)e.Item.FindControl("lblZoneProductMappingId")).Text);
                gvVendorProductZone.DataSource = null;
                gvVendorProductZone.Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVendorProductZone_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (((RadLabel)e.Item.FindControl("lblZoneProductMappingIdEdit")).Text == "")
            {
                InsertZoneProductPrice(ViewState["vendorid"].ToString(), ucVendorZone.SelectedValue,
                    ((RadLabel)e.Item.FindControl("lblVendorProductIdEdit")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("ucPriceEdit")).Text,
                    ((UserControlMaskNumber)e.Item.FindControl("ucDiscountEdit")).Text
                    );
            }
            else
            {
                UpdateZoneProductPrice(
                        ((RadLabel)e.Item.FindControl("lblZoneProductMappingIdEdit")).Text,
                       ViewState["vendorid"].ToString(), ucVendorZone.SelectedValue,
                        ((UserControlMaskNumber)e.Item.FindControl("ucPriceEdit")).Text,
                        ((UserControlMaskNumber)e.Item.FindControl("ucDiscountEdit")).Text
                        );
            }
            gvVendorProductZone.DataSource = null;
            gvVendorProductZone.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }


    }

    protected void gvVendorProductZone_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

                if (General.GetNullableGuid(dr["FLDZONEPRODUCTMAPPINGID"].ToString()) == null)
                    db.Visible = false;
            }
            

            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }
        if (e.Item is GridFooterItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }

    }
    private void InsertZoneProductPrice(string vendorid, string vendorzoneid, string vendorproductid, string price, string discount)
    {
        if (!IsValidZonePrice(price))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixPurchaseVendorProductZone.VendorZoneProductInsert(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableInteger(vendorid), General.GetNullableInteger(vendorzoneid),
            General.GetNullableGuid(vendorproductid)
            , decimal.Parse(price), decimal.Parse(discount)
            , new Guid(ViewState["contractid"].ToString())
            , General.GetNullableInteger(ucPort.SelectedValue)
            , General.GetNullableInteger(ucPackage.SelectedHard));
    }

    private void UpdateZoneProductPrice(string zoneproductmappingid, string vendorid, string vendorzoneid, string price, string discount)
    {
        if (!IsValidZonePrice(price))
        {
            ucError.Visible = true;
            return;
        }

        PhoenixPurchaseVendorProductZone.VendorZoneProductUpdate(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableInteger(vendorid), General.GetNullableInteger(vendorzoneid),
            General.GetNullableGuid(zoneproductmappingid), decimal.Parse(price), decimal.Parse(discount),
            new Guid(ViewState["contractid"].ToString()),
            General.GetNullableInteger(ucPort.SelectedValue),
            General.GetNullableInteger(ucPackage.SelectedHard)
            );

        ucStatus.Text = "Information updated";
    }

    private bool IsValidZonePrice(string price)
    {
        ucError.HeaderMessage = "Please provide the following required informaltion";

        if (price.Trim().Equals(""))
            ucError.ErrorMessage = "Price is required.";

        return (!ucError.IsError);
    }

    private void DeleteZoneProductPrice(string zoneproductmappingid)
    {
        PhoenixPurchaseVendorProductZone.VendorZoneProductDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(zoneproductmappingid));
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

        //if (Filter.CurrentPickListSelection.Keys[3].ToString() == "txtVenderID")
        //{
        //    ucVendorZone.VendorZoneList = PhoenixPurchaseVendorZone.VendorZoneList(0, int.Parse(Filter.CurrentPickListSelection.Get(3)));
        //}
    }

    private bool IsValidVendorPort(string vendor, string zone)
    {
        ucError.HeaderMessage = "Please provide the following required informaltion";

        if (vendor.Trim().Equals(""))
            ucError.ErrorMessage = "Vendor is required.";

        if (zone.Trim().Equals("") || zone.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Zone is required.";

        return (!ucError.IsError);
    }

    protected void gvVendorProductZone_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void ucPort_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        gvVendorProductZone.DataSource = null;
        gvVendorProductZone.Rebind();
    }
}
