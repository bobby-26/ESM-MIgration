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

public partial class Purchase_PurchaseContractVendor : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Vendors", "VENDOR",ToolBarDirection.Left);
            toolbarsub.AddButton("Zone", "ZONE",ToolBarDirection.Left);
            toolbarsub.AddButton("Product","PRODUCT",ToolBarDirection.Left);
            MenuPurchaseVendor.AccessRights = this.ViewState;
            MenuPurchaseVendor.MenuList = toolbarsub.Show();
            MenuPurchaseVendor.SelectedMenuIndex = 0;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Purchase/PurchaseContractVendor.aspx", "Export to Excel", "icon_xls.png", "Excel",ToolBarDirection.Left);
            toolbar.AddImageLink("javascript:CallPrint('rgvLine')", "", "icon_print.png", "PRINT",ToolBarDirection.Left);
            MenuPurchaseContractVendor.AccessRights = this.ViewState;
            MenuPurchaseContractVendor.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["vendorname"] = "";

                if (Request.QueryString["vendorid"] != null)

                    ViewState["vendorid"] = Request.QueryString["vendorid"].ToString();
                else
                    ViewState["vendorid"] = "";
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDNAME" };
        string[] alCaptions = { "Vendor" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseContractVendor.ContractVendorSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, sortexpression, sortdirection,
                                                                 (int)ViewState["PAGENUMBER"], iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=ContractVendor.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Contract Vendor</h3></td>");
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


    protected void PurchaseContractVendor_TabStripCommand(object sender, EventArgs e)
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

    protected void MenuPurchaseVendor_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("ZONE"))
            {
                if (!IsValidVendor(ViewState["vendorid"].ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("../Purchase/PurchaseVendorZone.aspx?vendorid=" + ViewState["vendorid"].ToString());
            }

            if (CommandName.ToUpper().Equals("PRODUCT"))
            {
                if (!IsValidVendor(ViewState["vendorid"].ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                Response.Redirect("../Purchase/PurchaseVendorProduct.aspx?vendorid=" + ViewState["vendorid"].ToString());
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        rgvLine.SelectedIndexes.Clear();
        foreach (GridDataItem item in rgvLine.Items)
        {
            if (item.GetDataKeyValue("FLDVENDORID").ToString().Equals(ViewState["vendorid"].ToString()))
            {
                rgvLine.SelectedIndexes.Add(item.ItemIndex);
            }
        }
    }

    private bool IsValidVendor(string vendor)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(vendor) == null)
            ucError.ErrorMessage = "Select Vendor";

        return (!ucError.IsError);
    }

    protected void rgvLine_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void rgvLine_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME" };
        string[] alCaptions = { "Vendor" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixPurchaseContractVendor.ContractVendorSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                , sortexpression, sortdirection
                                                                , rgvLine.CurrentPageIndex+1
                                                                , rgvLine.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);


        General.SetPrintOptions("rgvLine", "Contract Vendor", alCaptions, alColumns, ds);
        rgvLine.DataSource = ds;
        rgvLine.VirtualItemCount = iRowCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["vendorid"].ToString() == "")
            {
                rgvLine.SelectedIndexes.Add(0);
                ViewState["vendorid"] = ds.Tables[0].Rows[0]["FLDVENDORID"].ToString();
            }
            if (ViewState["vendorname"].ToString() == "")
                ViewState["vendorname"] = ds.Tables[0].Rows[0]["FLDNAME"].ToString();

            PhoenixPurchaseContractVendor.VendorName = ViewState["vendorname"].ToString();
        }
        SetRowSelection();
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void rgvLine_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if(e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;

            LinkButton db = (LinkButton)item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            
        }
        if(e.Item is GridFooterItem)
        {
            GridFooterItem item = (GridFooterItem)e.Item;
            RadTextBox tb1 = (RadTextBox)item.FindControl("txtSupplierIdAdd");
            if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

            ImageButton ib1 = (ImageButton)item.FindControl("btnSupplierAdd");
            if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListSupplierAdd', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=132,130,131')");
        }
    }

    protected void rgvLine_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ROWCLICK"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                ViewState["vendorid"] = item.GetDataKeyValue("FLDVENDORID").ToString();
                ViewState["vendorname"] = ((RadLabel)item.FindControl("lblVendorName")).Text;
                PhoenixPurchaseContractVendor.VendorName = ViewState["vendorname"].ToString();

            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                if (!IsValidVendor(((RadTextBox)item["VENDOR"].FindControl("txtSupplierIdAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixPurchaseContractVendor.ContractVendorInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        General.GetNullableInteger(((RadTextBox)item["VENDOR"].FindControl("txtSupplierIdAdd")).Text));

                rgvLine.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                PhoenixPurchaseContractVendor.ContractVendorDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , int.Parse(item.GetDataKeyValue("FLDCONTRACTVENDORID").ToString()));
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }

    protected void rgvLine_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
    }
}
