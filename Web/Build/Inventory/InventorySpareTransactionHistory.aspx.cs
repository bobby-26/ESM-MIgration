using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using System;
using System.Collections.Specialized;
using System.Data;
using Telerik.Web.UI;

public partial class InventorySpareTransactionHistory : PhoenixBasePage
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareTransactionHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSpareTransactionEntry')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Inventory/InventorySpareTransactionHistory.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuGridStoreInOut.AccessRights = this.ViewState; 
            MenuGridStoreInOut.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                gvSpareEntryDetail.PageSize = General.ShowRecords(null);
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SPAREITEMDISPOSITIONID"] = null;
                ddlDispositionType.HardTypeCode = ((int)PhoenixHardTypeCode.TRANSACTIONTYPE).ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDDISPOSITIODATE", "FLDNUMBER", "FLDNAME", "FLDTRANSACTIONTYPENAME", "FLDCOMPONENTNUMBER", "FLDWORKORDERTYPENAME", "FLDREPORTEDBY" };
            string[] alCaptions = { "Transaction Date", "Item Number", "Item Name", "Transaction Type", "Component Number", "Work Order", "Reported By." };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixInventorySpareItemTransaction.SpareItemTransactionHistorySearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                             string.IsNullOrEmpty(txtItemNumber.Text) ? txtItemNumber.Text : txtItemNumber.TextWithLiterals,
                             string.IsNullOrEmpty(txtItemNumberTo.Text) ? txtItemNumberTo.Text : txtItemNumberTo.TextWithLiterals,
                             txtItemName.Text,
                             string.IsNullOrEmpty(txtComponentNumber.Text) ? txtComponentNumber.Text : txtComponentNumber.TextWithLiterals,
                             string.IsNullOrEmpty(txtComponentNumberTo.Text) ? txtComponentNumberTo.Text : txtComponentNumberTo.TextWithLiterals,
                             txtComponentName.Text, General.GetNullableInteger(ddlDispositionType.SelectedHard), txtWorkOrderNo.Text, General.GetNullableDateTime(txtDispositionDate.Text), General.GetNullableDateTime(txtDispositionTodate.Text),
                              General.GetNullableInteger(chkCritical.Checked == true ? "1" : null), sortexpression, sortdirection, gvSpareEntryDetail.CurrentPageIndex + 1,
                            gvSpareEntryDetail.PageSize,
                            ref iRowCount,
                            ref iTotalPageCount);

            General.SetPrintOptions("gvSpareTransactionEntry", "Inventory Spare Item Transaction History Details", alCaptions, alColumns, ds);

            gvSpareEntryDetail.DataSource = ds;
            gvSpareEntryDetail.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSpareEntryDetail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    private bool IsValidSpareItemSearch(string numberfrom, string numberto, string componentnumberfrom, string componentnumberto)
    {
        ucError.HeaderMessage = "Please provide the following required information";


        if (numberfrom.Equals("") && !numberto.Equals(""))
            ucError.ErrorMessage = "Please Enter Part From Number ";
        if (!numberfrom.Equals("") && numberto.Equals(""))
            ucError.ErrorMessage = "Please Enter Part To Number";

        if (componentnumberfrom.Equals("") && !componentnumberto.Equals(""))
            ucError.ErrorMessage = "Please Enter Component From Number ";
        if (!componentnumberfrom.Equals("") && componentnumberto.Equals(""))
            ucError.ErrorMessage = "Please Enter Component To Number";

        return (!ucError.IsError);
    }
    protected void MenuGridStoreInOut_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            ; if (CommandName.ToUpper().Equals("FIND"))
            {
                string itemnumber = txtItemNumber.TextWithLiterals.Replace("_", "").TrimEnd('.');
                string itemnumberto = txtItemNumberTo.TextWithLiterals.Replace("_", "").TrimEnd('.');
                string componentnumber = txtComponentNumber.TextWithLiterals.Replace("_", "").TrimEnd('.');
                string componentnumberfrom = txtComponentNumberTo.TextWithLiterals.Replace("_", "").TrimEnd('.');

                if (!IsValidSpareItemSearch(itemnumber, itemnumberto, componentnumber, componentnumberfrom))
                {
                    ucError.Visible = true;
                    return;
                }
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            BindData();
            gvSpareEntryDetail.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {

            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDDISPOSITIODATE", "FLDNUMBER", "FLDNAME", "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "FLDTRANSACTIONTYPENAME", "FLDDISPOSITIONQUANTITY", "FLDROB", "FLDPURCHASEPRICE", "FLDFORMNUMBER", "FLDWORKORDERTYPENAME", "FLDREPORTEDBY" };
            string[] alCaptions = { "Transaction Date", "Item Number", "Item Name", "Component Number", "Component Name", "Transaction Type", "Quantity", "ROB", "Purchase Price", "Order Number", "Work Order", "Reported By" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = gvSpareEntryDetail.PageSize;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            string dispositionheaderid = "";

            string spareitemid = (Request.QueryString["SPAREITEMID"] == null) ? null : (Request.QueryString["SPAREITEMID"].ToString());

            if (Filter.CurrentStoreItemDispositionHeaderId != null)
            {
                NameValueCollection nvc = Filter.CurrentStoreItemDispositionHeaderId;
                dispositionheaderid = nvc.Get("DISPOSITIONHEADERID").ToString();
            }
            DataSet ds = PhoenixInventorySpareItemTransaction.SpareItemTransactionHistorySearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                             string.IsNullOrEmpty(txtItemNumber.Text) ? txtItemNumber.Text : txtItemNumber.TextWithLiterals,
                             string.IsNullOrEmpty(txtItemNumberTo.Text) ? txtItemNumberTo.Text : txtItemNumberTo.TextWithLiterals,
                             string.IsNullOrEmpty(txtComponentNumber.Text) ? txtComponentNumber.Text : txtComponentNumber.TextWithLiterals,
                             string.IsNullOrEmpty(txtComponentNumberTo.Text) ? txtComponentNumberTo.Text : txtComponentNumberTo.TextWithLiterals,
                             txtItemName.Text,
                             txtComponentName.Text,General.GetNullableInteger( ddlDispositionType.SelectedHard), txtWorkOrderNo.Text, General.GetNullableDateTime(txtDispositionDate.Text), General.GetNullableDateTime(txtDispositionTodate.Text),
                             General.GetNullableInteger(chkCritical.Checked == true ? "1" : null), sortexpression, sortdirection, 1, iRowCount,
                        ref iRowCount,
                        ref iTotalPageCount);
            Response.AddHeader("Content-Disposition", "attachment; filename=SpareItemTransactionDetails.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Inventory Spare Item Transaction History Details</h3></td>");
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvSpareEntryDetail.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSpareEntryDetail_SortCommand(object sender, GridSortCommandEventArgs e)
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
