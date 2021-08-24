using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Purchase;

public partial class PurchaseShippedQuantityReportFilter : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvQuery.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);


            }
            Toolbar();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void Toolbar()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Purchase/PurchaseShippedQuantityReportFilter.aspx", "Find", "search.png", "FIND");
        toolbar.AddImageButton("../Purchase/PurchaseShippedQuantityReportFilter.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
        toolbar.AddImageButton("../Purchase/PurchaseShippedQuantityReportFilter.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
        MenuPhoenixQuery.AccessRights = this.ViewState;

        MenuPhoenixQuery.MenuList = toolbar.Show();

    }
    protected void MenuPhoenixQuery_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;



        if (CommandName.ToUpper().Equals("FIND"))
        {
            //if (!IsValidSearchDetails())
            //{
            //    ucError.Visible = true;
            //    return;
            //}
            gvQuery.Rebind();
        }

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();

        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            txtfromnumber.Text = "";
            radfromdate.Text = "";
            radtodate.Text = "";
            ucVessel.SelectedVessel = "";
            txtVendorId.Text = "";
            txtVendorCode.Text = "";
            txtVendorName.Text = "";
            Rebind();
            // ShowReasonsExcel();

        }

    }
    protected void Rebind()
    {
        gvQuery.SelectedIndexes.Clear();
        gvQuery.EditIndexes.Clear();
        gvQuery.DataSource = null;
        gvQuery.Rebind();
    }
    protected void gvQuery_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvQuery.CurrentPageIndex + 1;
            BindData();
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

        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        

            ds = PhoenixPurchaseOrderLine.ShippedQtyReport(General.GetNullableString(txtfromnumber.Text),General.GetNullableDateTime(radfromdate.Text),General.GetNullableDateTime(radtodate.Text), General.GetNullableInteger(txtVendorId.Text),
                General.GetNullableString(ucVessel.SelectedVessel.ToString()),
              sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
               gvQuery.PageSize, ref iRowCount, ref iTotalPageCount);

        

        gvQuery.DataSource = ds;
        gvQuery.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;



    }
    protected void ShowExcel()
    {

        string[] alColumns = { "FLDFORMNUMBER", "FLDVESSELNAME", "FLDVENDORNAME", "FLDSHIPPEDDDATE", "FLDSHIPPEDAGINGDAYS", "FLDPOAMOUNTBYSHIPPEDQYT", "FLDINVOICENUMBER", "FLDINVOICEDATE" };
        string[] alCaptions = { "Form Number","Vessel Name","Vendor","Shipped Date", "No of Days lying with Forwarder", "PO Total by Shipped Qty (USD)", "Invoice Number","Invoice Date" };

        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());



        ds = PhoenixPurchaseOrderLine.ShippedQtyReport(General.GetNullableString(txtfromnumber.Text), General.GetNullableDateTime(radfromdate.Text), General.GetNullableDateTime(radtodate.Text), General.GetNullableInteger(txtVendorId.Text),
            General.GetNullableString(ucVessel.SelectedVessel.ToString()),
          sortexpression, sortdirection, Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Shipped Qty-Invoice Clearance Aging Report.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Shipped Qty - Invoice Clearance Aging Report</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        //Response.Write("<tr><td> From </td><td>");
        //Response.Write(radfromdate.Text + "</td>");
        //Response.Write("<td> To </td><td>");
        //Response.Write(radtodate.Text + "</td>");
        //Response.Write("</tr>");
        Response.Write("<tr>");
       // Response.Write("<td> Port </td><td>");
       // Response.Write(ddlmulticolumnport.Text + "</td>");
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
       // Response.Write("<tr>");
       // Response.Write("<td>");
       // Response.Write("Total");
       // Response.Write("</td>");
       // Response.Write("<td>");
       //// Response.Write(Total.ToString());
       // Response.Write("</td>");
       // Response.Write("<td>");
       // Response.Write("");
       // Response.Write("</td>");
       // Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void gvQuery_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
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


}