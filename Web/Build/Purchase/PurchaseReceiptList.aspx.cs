using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.Data;

public partial class PurchaseReceiptList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Purchase/PurchaseReceiptList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvReceipt')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Purchase/PurchaseReceiptList.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Purchase/PurchaseReceiptList.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            toolbar.AddImageLink("javascript:return showPickList('codehelp1', 'codehelp1', 'Invoice', '../Purchase/PurchaseReceiptAdd.aspx'); return true;", "Add", "Add.png", "ADD");

            MenuReceipt.AccessRights = this.ViewState;
            MenuReceipt.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                MenuReceipt.SelectedMenuIndex = 0;
                if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
                    Response.Redirect("PhoenixLogout.aspx");
                ViewState["PAGENUMBER"] = 1;
                ViewState["RECEIPTID"] = "";

                gvReceipt.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() == "0")
                {
                    ucVessel.Enabled = true;
                }
                else
                {
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvReceipt_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDRECEIPTNO","FLDSTOCKTYPE", "FLDTITLE", "FLDRECEIPTDATE", "FLDPORT", "FLDRECEIPTSTATUSNAME" };
        string[] alCaptions = { "Receipt No.","Stock Type", "Title", "Receipt Date", "Port", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : ViewState["SORTEXPRESSION"].ToString();


        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvReceipt.CurrentPageIndex + 1;

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseReceipt.ReceiptSearch(txtReceiptNo.Text.Trim()
        , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) == 0 ? General.GetNullableInteger(ucVessel.SelectedVessel) : General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                                , General.GetNullableInteger(ucPort.SelectedValue)
                                                                , General.GetNullableDateTime(ucReceiptDate.Text)
                                                                , General.GetNullableString(txtTitle.Text.Trim())
                                                                , General.GetNullableInteger(ddlStatus.SelectedValue)
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , gvReceipt.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        gvReceipt.DataSource = ds;
        gvReceipt.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvReceipt", "Receipt", alCaptions, alColumns, ds);
    }

    protected void gvReceipt_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem item = (GridDataItem)e.Item;

            DataRowView drv = (DataRowView)item.DataItem;
            ImageButton cmdEdit = (ImageButton)item["ACTION"].FindControl("cmdEdit");
            if (cmdEdit != null)
                cmdEdit.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'Purchase/PurchaseReceiptAdd.aspx?RECEIPTID=" + drv["FLDRECEIPTID"].ToString() + "'); return true;");
            if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

            ImageButton db = (ImageButton)item["ACTION"].FindControl("cmdComplete");
            if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

            LinkButton lblReceiptNo = (LinkButton)item["NUMBER"].FindControl("lblReceiptNo");
            if (lblReceiptNo != null)
                lblReceiptNo.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'Purchase/PurchaseReceiptAdd.aspx?RECEIPTID=" + drv["FLDRECEIPTID"].ToString() + "'); return true;");
            if (!SessionUtil.CanAccess(this.ViewState, lblReceiptNo.CommandName)) cmdEdit.Enabled = false;
        }
    }

    protected void gvReceipt_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        gvReceipt.Rebind();
    }

    protected void gvReceipt_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper() == "COMPLETE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                string receiptId = ((RadLabel)item.FindControl("lblReceiptId")).Text;
                string stocktype = ((RadLabel)item.FindControl("lblStockType")).Text;

                PhoenixPurchaseReceipt.ReceiptComplete(new Guid(receiptId), stocktype);

                gvReceipt.Rebind();

            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvReceipt_DeleteCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void MenuReceipt_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvReceipt.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtReceiptNo.Text = "";
                ucVessel.SelectedValue = 0;
                ucPort.SelectedValue = "";
                ddlStatus.SelectedValue = "";
                ddlStatus.SelectedValue = "0";
                txtTitle.Text = "";
                ucReceiptDate.Text = "";
                gvReceipt.Rebind();
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvReceipt.Rebind();
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
        string[] alColumns = { "FLDRECEIPTNO", "FLDSTOCKTYPE", "FLDTITLE", "FLDRECEIPTDATE", "FLDPORT", "FLDRECEIPTSTATUSNAME" };
        string[] alCaptions = { "Receipt No.", "Stock Type","Title", "Receipt Date", "Port", "Status" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseReceipt.ReceiptSearch(txtReceiptNo.Text.Trim()
        , General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) == null ? General.GetNullableInteger(ucVessel.SelectedVessel) : General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                                , General.GetNullableInteger(ucPort.SelectedValue)
                                                                , General.GetNullableDateTime(ucReceiptDate.Text)
                                                                , General.GetNullableString(txtTitle.Text.Trim())
                                                                , General.GetNullableInteger(ddlStatus.SelectedValue)
                                                                , sortexpression
                                                                , sortdirection
                                                                , 1
                                                                , gvReceipt.VirtualItemCount > 0 ? gvReceipt.VirtualItemCount : 10
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=PurchaseReceipt.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Purchase Receipt</center></h3></td>");
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
}