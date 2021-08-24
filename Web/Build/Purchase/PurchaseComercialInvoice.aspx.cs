using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Reports;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PurchaseComercialInvoice : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Purchase/PurchaseComercialInvoice.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Purchase/PurchaseComercialInvoice.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbar.AddImageButton("../Purchase/PurchaseComercialInvoice.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('rgvForm')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("javascript:return showPickList('codehelp1', 'codehelp1', 'Invoice', '../Purchase/PurchaseComercialInvoiceAdd.aspx'); return true;", "Add", "Add.png", "ADD");

            MenuNewRequisition.AccessRights = this.ViewState;
            MenuNewRequisition.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                MenuNewRequisition.SelectedMenuIndex = 0;
                if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
                    Response.Redirect("PhoenixLogout.aspx");
                ViewState["PAGENUMBER"] = 1;

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


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDINVOICENUMBER", "FLDINVOICEDATE", "FLDCOMPANYNAME", "FLDCONSIGNY" };
        string[] alCaptions = { "Invoice Number", "Invoice Date", "Company Name", "Consigny" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseComercialInvoice.ComercialInvoiceSearch(txtFormNo.Text.Trim()
                                                                , General.GetNullableDateTime(txtFromDate.Text)
                                                                , General.GetNullableDateTime(txtToDate.Text)
                                                                , General.GetNullableInteger(ucAddrAgent.SelectedValue)
                                                                , General.GetNullableInteger(ucCompany.SelectedCompany)
                                                                , sortexpression
                                                                , sortdirection
                                                                , 1
                                                                , rgvForm.VirtualItemCount > 0 ? rgvForm.VirtualItemCount : 10
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=CommercialInvoice.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Commercial Invoice</center></h3></td>");
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
    protected void MenuNewRequisition_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                rgvForm.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFormNo.Text = "";
                ucCompany.SelectedCompany = "";
                ucAddrAgent.SelectedValue = "";
                ucAddrAgent.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                rgvForm.Rebind();
            }
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

    protected void ddlStockType_TextChanged(object sender, EventArgs e)
    {
        rgvForm.Rebind();
    }

    protected void rgvForm_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDINVOICENUMBER", "FLDINVOICEDATE", "FLDCOMPANYNAME", "FLDCONSIGNY" };
        string[] alCaptions = { "Invoice Number", "Invoice Date", "Company Name", "Consigny"};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : ViewState["SORTEXPRESSION"].ToString();


        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : rgvForm.CurrentPageIndex + 1;

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseComercialInvoice.ComercialInvoiceSearch(txtFormNo.Text.Trim()
                                                                , General.GetNullableDateTime(txtFromDate.Text)
                                                                , General.GetNullableDateTime(txtToDate.Text)
                                                                , General.GetNullableInteger(ucAddrAgent.SelectedValue)
                                                                , General.GetNullableInteger(ucCompany.SelectedCompany)
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , rgvForm.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        rgvForm.DataSource = ds;
        rgvForm.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("rgvForm", "Commercial Invoice", alCaptions, alColumns, ds);
    }

    protected void rgvForm_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;
  
            DataRowView drv = (DataRowView)item.DataItem;
            ImageButton cmdEdit = (ImageButton)item["ACTION"].FindControl("cmdEdit");
            if (cmdEdit != null)
                cmdEdit.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'Purchase/PurchaseComercialInvoiceAdd.aspx?INVOICEID="+ drv["FLDCOMRTIALINVOICEID"].ToString()+ "'); return true;");
            if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton db = (ImageButton)item["ACTION"].FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

            LinkButton lblInvoiceNo = (LinkButton)item["NUMBER"].FindControl("lblInvoiceNo");
            if (lblInvoiceNo != null)
                lblInvoiceNo.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'Purchase/PurchaseComercialInvoiceAdd.aspx?INVOICEID=" + drv["FLDCOMRTIALINVOICEID"].ToString() + "'); return true;");
            if (!SessionUtil.CanAccess(this.ViewState, lblInvoiceNo.CommandName)) cmdEdit.Enabled = false;
        }
    }

    protected void rgvForm_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {

            PhoenixPurchaseComercialInvoice.DeleteComercialInvoice(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblInoviceId")).Text));
            rgvForm.DataSource = null;
            rgvForm.Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("PDF"))
        {
            string Tmpfilelocation = string.Empty; string[] reportfile = new string[0];
            Guid? invoiceid = General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblInoviceId")).Text);
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = PhoenixPurchaseComercialInvoice.ComercialInvoiceLineitemSearch(invoiceid,null,null,1,100000,ref iRowCount,ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0 && PhoenixGeneralSettings.CurrentGeneralSetting.ReportType == "1")
            {
                string InvoiceNumber = ds.Tables[0].Rows[0]["FLDINVOICENUMBER"].ToString();

                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("applicationcode", "3");
                nvc.Add("reportcode", "COMMERCIALINVOICE");
                nvc.Add("CRITERIA", "");
                Session["PHOENIXREPORTPARAMETERS"] = nvc;

                Tmpfilelocation = System.Web.HttpContext.Current.Request.MapPath("~/");
                string filename = "CommercialInvoice.pdf";
                Tmpfilelocation = HttpContext.Current.Server.MapPath("../Attachments/TEMP/" + filename);

                PhoenixSSRSReportClass.ExportSSRSReport(reportfile, ds, PhoenixSSRSReportClass.ExportFileFormat.PDF, null, ref Tmpfilelocation);
                Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + Tmpfilelocation + "&type=pdf");
            }
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
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

    protected void rgvForm_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            rgvForm.DataSource = null;
            rgvForm.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
