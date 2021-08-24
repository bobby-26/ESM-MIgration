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
using System.Text;
using Telerik.Web.UI;
public partial class PurchaseContractPriceList : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);

        
        if (!IsPostBack)
        {
            if (Request.QueryString["quotationid"] != null)
            {
                ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
            }
            else ViewState["quotationid"] = "";




            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            SetDropDownValues();
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Purchase/PurchaseContractPriceList.aspx?quotationid=" + ViewState["quotationid"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('rgvLine')", "Print Grid", "icon_print.png", "PRINT");
        MenuRegistersVendorProductZone.AccessRights = this.ViewState;
        MenuRegistersVendorProductZone.MenuList = toolbar.Show();

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Apply Price", "APPLYPRICE", ToolBarDirection.Right);
        //toolbarsub.AddButton("Item", "MAPPING");
        //toolbarsub.AddButton("Delivery", "DELIVERY");
        MenuContractItem.MenuList = toolbarsub.Show();
    }

    private void SetDropDownValues()
    {
        if (Request.QueryString["vendorid"] != null)
        {
            ViewState["vendorid"] = Request.QueryString["vendorid"].ToString();
            //ucVendorZone.SelectedVendor = int.Parse(ViewState["vendorid"].ToString());

            ucVendorZone.DataSource = PhoenixPurchaseVendorZone.VendorZoneList(0, int.Parse(ViewState["vendorid"].ToString()));
            ucVendorZone.DataBind();

            ucPort.DataSource = PhoenixPurchaseZonePortMapping.ZonePortMappingList(
                General.GetNullableInteger(ucVendorZone.SelectedValue),
                General.GetNullableInteger(ViewState["vendorid"].ToString()));
            ucPort.DataBind();

            if (Request.QueryString["portid"] != null && General.GetNullableInteger(Request.QueryString["portid"].ToString()) != null)
            {
                
                ucPort.SelectedIndex = -1;
                foreach (RadComboBoxItem item in ucPort.Items)
                {
                    if (item.Value == Request.QueryString["portid"].ToString())
                    {
                        item.Selected = true;
                        break;
                    }
                }

                DataSet ds = PhoenixPurchaseZonePortMapping.ZonePortMappingEdit(
                    General.GetNullableInteger(ucVendorZone.SelectedValue)
                    , General.GetNullableInteger(ucPort.SelectedValue)
                    , General.GetNullableInteger(ViewState["vendorid"].ToString())
                    );

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    if (General.GetNullableInteger(dr["FLDVENDORZONEID"].ToString()) != null)
                    {
                        ucVendorZone.SelectedValue = dr["FLDVENDORZONEID"].ToString();

                        ucPort.DataSource = null;
                        ucPort.DataSource = PhoenixPurchaseZonePortMapping.ZonePortMappingList(
                            General.GetNullableInteger(ucVendorZone.SelectedValue),
                            General.GetNullableInteger(ViewState["vendorid"].ToString()));
                        ucPort.DataBind();

                        if (General.GetNullableInteger(dr["FLDPORTID"].ToString()) != null)
                        {
                            ucPort.SelectedValue = dr["FLDPORTID"].ToString();
                        }
                    }
                    else
                        ucPort.SelectedIndex = 0;
                }
            }
        }
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

    protected void ucVendorZone_DataBound(object sender, EventArgs e)
    {
        ucVendorZone.Items.Insert(0, new RadComboBoxItem("--select--", "Dummy"));
    }

    protected void ucSeaport_DataBound(object sender, EventArgs e)
    {
        ucPort.Items.Insert(0, new RadComboBoxItem("--select--", "Dummy"));
    }

    protected void MenuContractItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("APPLYPRICE"))
            {
                if (rgvLine.Items.Count > 0)
                {
                    StringBuilder lblZoneProductMappingId = new StringBuilder();
                    StringBuilder lblQuotationLineId = new StringBuilder();

                    lblZoneProductMappingId.Append(",");

                    foreach (GridDataItem item in rgvLine.Items)
                    {
                        if (item.GetDataKeyValue("FLDZONEPRODUCTMAPPINGID") != null)
                        {
                            lblZoneProductMappingId.Append(item.GetDataKeyValue("FLDZONEPRODUCTMAPPINGID").ToString());
                            lblZoneProductMappingId.Append(",");
                        }
                    }

                    string strZoneProductMappingId = lblZoneProductMappingId.ToString();

                    lblQuotationLineId.Append(",");

                    foreach (GridDataItem item in rgvLine.Items)
                    {
                        if (item.GetDataKeyValue("FLDQUOTATIONLINEID") != null)
                        {
                            lblQuotationLineId.Append(item.GetDataKeyValue("FLDQUOTATIONLINEID").ToString());
                            lblQuotationLineId.Append(",");
                        }
                    }

                    string strQuotationLineId = lblQuotationLineId.ToString();

                    if (strQuotationLineId.Length > 1)
                    {
                        PhoenixPurchaseQuotation.ApplyContractPrice(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , new Guid(ViewState["quotationid"].ToString())
                            , strZoneProductMappingId
                            , strQuotationLineId);

                        ucStatus.Show("Contract Price is updated to the below line items.");

                        string Script = "";
                        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                        Script += " fnReloadList();";
                        Script += "</script>" + "\n";
                        Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                    }
                    else
                    {
                        ucError.ErrorMessage = "No Line items available to apply the price.";
                        ucError.Visible = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void VendorZone_Changed(object sender, EventArgs e)
    {
        ucPort.DataSource = PhoenixPurchaseZonePortMapping.ZonePortMappingList(
            General.GetNullableInteger(ucVendorZone.SelectedValue),
            General.GetNullableInteger(ViewState["vendorid"].ToString()));
        ucPort.DataBind();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDPRODUCTCODE", "FLDPRODUCTNAME", "FLDZONECODE", "FLDSEAPORTNAME", "FLDUNITNAME", "FLDPRICE", "FLDDISCOUNT" };
        string[] alCaptions = { "Product Code", "Product Name", "Zone", "Port", "Contract Unit", "Price", "Discount" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseQuotation.ShowContractPrice(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(ViewState["quotationid"].ToString()),
            General.GetNullableInteger(ucVendorZone.SelectedValue),
            General.GetNullableInteger(ucPort.SelectedValue),
            General.GetNullableInteger(ucPackage.SelectedHard),
            sortexpression, sortdirection, 1,
            rgvLine.VirtualItemCount > 0 ? rgvLine.VirtualItemCount : 10, ref iRowCount, ref iTotalPageCount);

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

    

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }

    protected void rgvLine_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDPRODUCTCODE", "FLDPRODUCTNAME", "FLDZONECODE", "FLDSEAPORTNAME", "FLDUNITNAME", "FLDPRICE", "FLDDISCOUNT" };
        string[] alCaptions = { "Product Code", "Product Name", "Zone", "Port", "Contract Unit", "Price", "Discount" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseQuotation.ShowContractPrice(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(ViewState["quotationid"].ToString()),
            General.GetNullableInteger(ucVendorZone.SelectedValue),
            General.GetNullableInteger(ucPort.SelectedValue),
            General.GetNullableInteger(ucPackage.SelectedHard),
            sortexpression, sortdirection, rgvLine.CurrentPageIndex + 1,
            rgvLine.PageSize, ref iRowCount, ref iTotalPageCount);

        rgvLine.DataSource = ds;
        rgvLine.VirtualItemCount = iRowCount;

        General.SetPrintOptions("gvVendorProductZone", "Product Price", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            
            if (!IsPostBack)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ViewState["contractid"] = dr["FLDCONTRACTID"].ToString();

                EnablePackage();
            }
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void rgvLine_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

    }

    protected void rgvLine_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
    }

    protected void rgvLine_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
           
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                GridDataItem item = (GridDataItem)e.Item;

                string lblZoneProductMappingId = item.GetDataKeyValue("FLDZONEPRODUCTMAPPINGID").ToString();
                string lblQuotationLineId = item.GetDataKeyValue("FLDQUOTATIONLINEID").ToString();

                PhoenixPurchaseQuotation.ApplyContractPrice(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["quotationid"].ToString())
                        , lblZoneProductMappingId
                        , lblQuotationLineId);

                ucStatus.Show("Contract Price is updated to the line item.");

                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList();";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucPackage_TextChangedEvent(object sender, EventArgs e)
    {
        rgvLine.Rebind();
    }

    protected void ucPort_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        rgvLine.Rebind();
    }
}
