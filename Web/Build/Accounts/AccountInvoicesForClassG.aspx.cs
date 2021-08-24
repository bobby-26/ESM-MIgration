using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class AccountsInvoicesForClassG : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountInvoicesForClassG.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbargrid.AddImageLink("javascript:CallPrint('gvSpareItemList')", "Print Grid", "icon_print.png", "Print");
          
            MenuSpareItemList.AccessRights = this.ViewState;
            MenuSpareItemList.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar= new PhoenixToolbar();
            Menutab.Title = "Class G Supplier Invoices";
            Menutab.AccessRights = this.ViewState;
            Menutab.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvSpareItemList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

           // BindData();
          
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSpareItemList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSpareItemList.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuSpareItemList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }


    protected void Rebind()
    {
        gvSpareItemList.SelectedIndexes.Clear();
        gvSpareItemList.EditIndexes.Clear();
        gvSpareItemList.DataSource = null;
        gvSpareItemList.Rebind();
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alCaptions = {
                                "Supplier Code",
                                "Supplier Name",
                                "Invoice Reference",
                                "Received Date",
                                "Invoice Amount",
                                "Currency Code",
                                "Invoice Status",
                                "Invoice Number",
                                "Invoice Date",
                                "Invoice Type",
                                 "Invoice PIC"
                              };

        string[] alColumns = {  "FLDCODE",
                                "FLDSUPPLIERNAME",
                                "FLDINVOICESUPPLIERREFERENCE",
                                "FLDINVOICERECEIVEDDATE",
                                "FLDINVOICEAMOUNT",
                                "FLDCURRENCYNAME",
                                "FLDINVOICESTATUSNAME",
                                "FLDINVOICENUMBER",
                                "FLDINVOICEDATE",
                                "FLDINVOICETYPENAME",
                                "FLDPICUSERNAME"
                             };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        //int suppliercode=0;
        //string suppliername = "";

        DataSet ds = new DataSet();
        ds = PhoenixAccountsInvoiceForClassG.InvoiceSearchForClassG(null, sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                                                              gvSpareItemList.PageSize, ref iRowCount,  ref iTotalPageCount);

        General.SetPrintOptions("gvSpareItemList", "Class G Supplier Invoices ", alCaptions, alColumns, ds);
       
            gvSpareItemList.DataSource = ds;
            gvSpareItemList.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
   
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
       
    }


    protected void gvSpareItemList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    protected void gvSpareItemList_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        DataTable dt = new DataTable();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = {
                                "Supplier Code",
                                "Supplier Name",
                                "Invoice Reference",
                                "Received Date",
                                "Invoice Amount",
                                "Vessel Name",
                                "Currency Code",
                                "Invoice Status",
                                "Invoice Number",
                                "Invoice Date",
                                "Invoice Type",
                                "Remarks",
                                "Invoice PIC",
                              };

        string[] alColumns = {
                                "FLDCODE",
                                "FLDSUPPLIERNAME",
                                "FLDINVOICESUPPLIERREFERENCE",
                                "FLDINVOICERECEIVEDDATE",
                                "FLDINVOICEAMOUNT",
                                "FLDVESSELLIST",
                                "FLDCURRENCYNAME",
                                "FLDINVOICESTATUSNAME",
                                "FLDINVOICENUMBER",
                                "FLDINVOICEDATE",
                                "FLDINVOICETYPENAME",
                                "FLDREMARKS",
                                "FLDPICUSERNAME",

                             };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsInvoiceForClassG.InvoiceSearchForClassG(null,sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                                                               PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);
        dt = ds.Tables[0];
        General.ShowExcel("Class G Supplier Invoices", dt, alColumns, alCaptions, null, null);

    }
    //protected void cmdSort_Click(object sender, EventArgs e)
    //{
    //    ImageButton ib = (ImageButton)sender;
    //    try
    //    {

    //        ViewState["SORTEXPRESSION"] = ib.CommandName;
    //        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    //        BindData();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }

    //}

    protected void gvSpareItemList_ItemDataBound(object sender, GridItemEventArgs e)
 
    {

        if (e.Item is GridDataItem)
        {
            RadTextBox txtInvoiceCode = (RadTextBox)e.Item.FindControl("txtInvoiceCode");
            ImageButton cmdMoreInfo = (ImageButton)e.Item.FindControl("cmdMoreInfo");
            if (cmdMoreInfo != null)
            {
                cmdMoreInfo.Attributes.Add("onclick", "javascript:openNewWindow('codeHelp1', '','" + Session["sitepath"] + "/Accounts/AccountsInvoiceMoreInfo.aspx?invoiceCode=" + txtInvoiceCode.Text + "',false);");


            }
        }
    }
    protected void gvSpareItemList_SortCommand(object sender, GridSortCommandEventArgs e)
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
    }


   
}
