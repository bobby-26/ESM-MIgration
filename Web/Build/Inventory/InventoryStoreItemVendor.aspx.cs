using System;
using System.Data;
using System.Web.UI;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class InventoryStoreItemVendor : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridDataItem item in gvVendor.Items)
        {
            if (item is GridDataItem)
            {
                Page.ClientScript.RegisterForEventValidation
                        (item.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (item.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventoryStoreItemVendor.aspx?STOREITEMID=" + Request.QueryString["STOREITEMID"] + "", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvVendor')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbargrid.AddFontAwesomeButton("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?mode=custom&addresstype=130,131', true);", "Vendor", "vendor.png", "ADDVENDOR");
            MenuGridStoreItemVendor.AccessRights = this.ViewState;  
            MenuGridStoreItemVendor.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void StoreItemVendor_TabStripCommand(object sender, EventArgs e)
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
    protected void gvVendor_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvVendor_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        DeleteStockItemVendor(((RadLabel)eeditedItem.FindControl("lblStockItemVendorId")).Text);
        gvVendor.Rebind();
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDVENDORCODE", "FLDNAME", "FLDQAGRADING", "FLDSTOREITEMPRICE", "FLDFORMNO","FLDORDERED", "FLDORDEREDDATE", "FLDRECEIVEDDATE" };
            string[] alCaptions = { "Vendor Code", "Vendor Name", "QA Grading", "Last Purchased Price", "Form No","Order Placed" ,"Last Ordered Date", "Received" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhonixInventoryStoreItemVendor.StoreItemVendorSearch(null, Request.QueryString["STOREITEMID"].ToString(),
                General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), null, null, null, null, null,
                General.GetNullableDateTime(""), sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                General.ShowRecords(null),
                ref iRowCount,
                ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=StoreItemVendor.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Inventory StoreItem Vendor</h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVENDORCODE", "FLDNAME", "FLDQAGRADING", "FLDSTOREITEMPRICE", "FLDFORMNO", "FLDORDERED", "FLDORDEREDDATE", "FLDRECEIVEDDATE" };
            string[] alCaptions = { "Vendor Code", "Vendor Name", "QA Grading", "Last Purchased Price", "Form No", "Order Placed", "Last Ordered Date", "Received" };
            string strHeader = "Inventory Store Item Vendor";

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhonixInventoryStoreItemVendor.StoreItemVendorSearch(null, Request.QueryString["STOREITEMID"].ToString(),
                General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()), null, null, null, null, null, General.GetNullableDateTime(""),
                sortexpression, sortdirection,
                gvVendor.CurrentPageIndex + 1,
                gvVendor.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("gvVendor", strHeader, alCaptions, alColumns, ds);

            gvVendor.DataSource = ds;
            gvVendor.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DeleteStockItemVendor(string stockitemvendorid)
    {
        try
        {

            PhonixInventoryStoreItemVendor.DeleteStoreItemVendor(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(stockitemvendorid));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            string vendorid = nvc.Get("lblAddressCode").ToString();

            if ((Request.QueryString["STOREITEMID"] != null) && (Request.QueryString["STOREITEMID"] != ""))
            {
                PhonixInventoryStoreItemVendor.InsertStoreItemVendor
                (
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Convert.ToInt32(vendorid), new Guid(Request.QueryString["STOREITEMID"]), PhoenixSecurityContext.CurrentSecurityContext.VesselID
                );
            }

            gvVendor.Rebind();
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



    protected void gvVendor_SortCommand(object sender, GridSortCommandEventArgs e)
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
