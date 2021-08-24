using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using Telerik.Web.UI;
public partial class PurchaseVendorProduct : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);


            if (PhoenixPurchaseContractVendor.VendorName != null)
            {
                ViewState["vendorname"] = PhoenixPurchaseContractVendor.VendorName;
                MenuPurchaseVendor.Title = PhoenixPurchaseContractVendor.VendorName;
            }
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Vendors", "VENDOR");
            toolbarsub.AddButton("Zone", "ZONE");
            toolbarsub.AddButton("Product", "PRODUCT");
            MenuPurchaseVendor.AccessRights = this.ViewState;
            MenuPurchaseVendor.MenuList = toolbarsub.Show();
            MenuPurchaseVendor.SelectedMenuIndex = 2;

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Purchase/PurchaseVendorProduct.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('rgvProduct')", "Print Grid", "icon_print.png", "PRINT");
            MenuRegistersVendorProduct.AccessRights = this.ViewState;
            MenuRegistersVendorProduct.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["vendorid"] != null)
                    ViewState["vendorid"] = Request.QueryString["vendorid"].ToString();

                if (PhoenixPurchaseContractVendor.VendorName != null)
                {
                    ViewState["vendorname"] = PhoenixPurchaseContractVendor.VendorName;
                    MenuPurchaseVendor.Title= PhoenixPurchaseContractVendor.VendorName;
                }
                rgvProduct.PageSize = General.ShowRecords(null);
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

        string[] alColumns = { "FLDSFINUMBER", "FLDPRODUCTCODE", "FLDPRODUCTNAME", "FLDPRODUCTGROUP", "FLDDESCRIPTION", "FLDUNITNAME" };
        string[] alCaptions = { "Code", "Product Code", "Product Name", "Product Group", "Description", "Unit" };
        
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseVendorProduct.VendorProductSearch(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ViewState["vendorid"].ToString()), sortexpression, sortdirection,
             1, rgvProduct.VirtualItemCount > 0 ? rgvProduct.VirtualItemCount : 10, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Vendor Product.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Vendor Product</h3></td>");
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

    protected void RegistersVendorProduct_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidVendor(ViewState["vendorid"].ToString()))
                {
                    ucError.Visible = true;
                    return;
                }
                rgvProduct.Rebind();
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
            if (CommandName.ToUpper().Equals("VENDOR"))
            {
                Response.Redirect("../Purchase/PurchaseContractVendor.aspx?vendorid=" + ViewState["vendorid"].ToString());
            }
            if (CommandName.ToUpper().Equals("ZONE"))
            {
                Response.Redirect("../Purchase/PurchaseVendorZone.aspx?vendorid=" + ViewState["vendorid"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InsertVendorProduct(
        string vendorid, string sfinumber, string productcode, string productname, string productgroup,string description,string unitid)
    {

        PhoenixPurchaseVendorProduct.VendorProductInsert(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableInteger(vendorid),
            sfinumber,productcode,productname,productgroup,description,General.GetNullableInteger(unitid));
    }

    private void UpdateVendorProduct(
        string vendorid,Guid? vendorproductid,string sfinumber, string productcode,string productname,string productgroup,string description,int? unitid)
    {
        PhoenixPurchaseVendorProduct.VendorProductUpdate(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, 
            General.GetNullableInteger(ViewState["vendorid"].ToString()),
            sfinumber, vendorproductid,
            productcode,productname,productgroup,description,unitid);

        ucStatus.Show("Product Details updated");
    }

    private bool IsValidProduct(
        string vendorid, string sfinumber, string productcode,
        string productname,string productgroup,string description,string unitid)
    {
        ucError.HeaderMessage = "Please provide the following required informaltion";

        if (vendorid.Trim().Equals(""))
            ucError.ErrorMessage = "Vendor is required.";

        if (sfinumber.Trim().Equals("__.__.__")|| sfinumber.Trim().Equals(""))
            ucError.ErrorMessage = "Number is required.";

        if (productcode.Trim().Equals(""))
            ucError.ErrorMessage = "Product Code is required.";

        if (productname.Trim().Equals(""))
            ucError.ErrorMessage = "Product Name is required.";

        if (productgroup.Trim().Equals(""))
            ucError.ErrorMessage = "Product Group is required.";

        if (description.Trim().Equals(""))
            ucError.ErrorMessage = "Description is required.";

        if (unitid.Trim().Equals("") || unitid.ToUpper().Equals("DUMMY"))
            ucError.ErrorMessage = "Unit is required.";


        return (!ucError.IsError);
    }

    private void VendorProductDelete(Guid? vendorproductid)
    {
        PhoenixPurchaseVendorProduct.VendorProductDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, vendorproductid);
    }

  
    private bool IsValidVendor(string vendor)
    {
        ucError.ErrorMessage = "Please provide the following required information";
        if (vendor.Trim().Equals(""))
            ucError.ErrorMessage = "Select Vendor";

        return (!ucError.IsError);
    }

    protected void rgvProduct_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSFINUMBER", "FLDPRODUCTCODE", "FLDPRODUCTNAME", "FLDPRODUCTGROUP", "FLDDESCRIPTION", "FLDUNITNAME" };
        string[] alCaptions = { "Code", "Product Code", "Product Name", "Product Group", "Description", "Unit" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixPurchaseVendorProduct.VendorProductSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableInteger(ViewState["vendorid"].ToString()), sortexpression, sortdirection,
                                                                 rgvProduct.CurrentPageIndex+1, rgvProduct.PageSize, ref iRowCount, ref iTotalPageCount);

        rgvProduct.DataSource = ds;
        rgvProduct.VirtualItemCount = iRowCount;
        General.SetPrintOptions("rgvProduct", "Vendor Product", alCaptions, alColumns, ds);
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void rgvProduct_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
            UserControlUnit uc = (UserControlUnit)item.FindControl("ucUnitEdit");
            if (uc != null) uc.UnitList = PhoenixRegistersUnit.ListUnit();

            ImageButton db = (ImageButton)item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            ImageButton eb = (ImageButton)item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            DataRowView drv = (DataRowView)item.DataItem;

            UserControlUnit ucUnit = (UserControlUnit)item.FindControl("ucUnitEdit");
            if (ucUnit != null) ucUnit.SelectedUnit = drv["FLDUNITID"].ToString();

        }
        if (e.Item is GridFooterItem)
        {
            GridFooterItem item = (GridFooterItem)e.Item;
            ImageButton db = (ImageButton)item.FindControl("cmdAdd");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            UserControlUnit u = (UserControlUnit)item.FindControl("ucUnitAdd");
            u.UnitList = PhoenixRegistersUnit.ListUnit();
        }
    }

    protected void rgvProduct_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem item = (GridFooterItem)e.Item;
                if (!IsValidProduct(ViewState["vendorid"].ToString(),
                    ((UserControlMaskedTextBox)item.FindControl("txtNumberAdd")).Text,
                    ((RadTextBox)item.FindControl("txtProductCodeAdd")).Text,
                    ((RadTextBox)item.FindControl("txtProductNameAdd")).Text,
                    ((RadTextBox)item.FindControl("txtProductGroupAdd")).Text,
                    ((RadTextBox)item.FindControl("txtDescriptionAdd")).Text,
                    ((UserControlUnit)item.FindControl("ucUnitAdd")).SelectedUnit
                    ))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                InsertVendorProduct(ViewState["vendorid"].ToString(),
                    ((UserControlMaskedTextBox)item.FindControl("txtNumberAdd")).Text,
                    ((RadTextBox)item.FindControl("txtProductCodeAdd")).Text,
                    ((RadTextBox)item.FindControl("txtProductNameAdd")).Text,
                    ((RadTextBox)item.FindControl("txtProductGroupAdd")).Text,
                    ((RadTextBox)item.FindControl("txtDescriptionAdd")).Text,
                    ((UserControlUnit)item.FindControl("ucUnitAdd")).SelectedUnit);
                rgvProduct.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                if (!IsValidProduct(ViewState["vendorid"].ToString(),
                    ((UserControlMaskedTextBox)item.FindControl("txtNumber")).Text,
                    ((RadTextBox)item.FindControl("txtProductCodeEdit")).Text,
                    ((RadTextBox)item.FindControl("txtProductNameEdit")).Text,
                    ((RadTextBox)item.FindControl("txtProductGroupEdit")).Text,
                    ((RadTextBox)item.FindControl("txtDescriptionEdit")).Text,
                    ((UserControlUnit)item.FindControl("ucUnitEdit")).SelectedUnit
                   ))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                UpdateVendorProduct(ViewState["vendorid"].ToString(),
                         General.GetNullableGuid(item.GetDataKeyValue("FLDVENDORPRODUCTID").ToString()),
                        ((UserControlMaskedTextBox)item.FindControl("txtNumber")).Text,
                         ((RadTextBox)item.FindControl("txtProductCodeEdit")).Text,
                         ((RadTextBox)item.FindControl("txtProductNameEdit")).Text,
                         ((RadTextBox)item.FindControl("txtProductGroupEdit")).Text,
                         ((RadTextBox)item.FindControl("txtDescriptionEdit")).Text,
                         General.GetNullableInteger(((UserControlUnit)item.FindControl("ucUnitEdit")).SelectedUnit)
                         );
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                GridDataItem item = (GridDataItem)e.Item;
                VendorProductDelete(General.GetNullableGuid(item.GetDataKeyValue("FLDVENDORPRODUCTID").ToString()));
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void rgvProduct_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
    }
}
