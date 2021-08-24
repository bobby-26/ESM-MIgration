using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PurchaseReceiptLineitemAdd : PhoenixBasePage
{
    string vesselname;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Purchase/PurchaseReceiptLineitemAdd.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Purchase/PurchaseReceiptLineitemAdd.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            //toolbar.AddImageButton("../Purchase/PurchaseReceiptLineitemAdd.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbar.AddImageLink("javascript:CallPrint('rgvForm')", "Print Grid", "icon_print.png", "PRINT");
            MenuNewRequisition.AccessRights = this.ViewState;
            MenuNewRequisition.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                VesselConfiguration();
                MenuNewRequisition.SelectedMenuIndex = 0;
                if (PhoenixGeneralSettings.CurrentGeneralSetting == null)
                    Response.Redirect("PhoenixLogout.aspx");
                if (Request.QueryString["pageno"] != null)
                {
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["pageno"].ToString());
                    rgvForm.CurrentPageIndex = int.Parse(Request.QueryString["pageno"].ToString()) - 1;
                }
                else
                    ViewState["PAGENUMBER"] = 1;
                ViewState["RECEIPTID"] = "";
                ViewState["VESSELID"] = "";
                if (Request.QueryString["RECEIPTID"] != null)
                    ViewState["RECEIPTID"] = Request.QueryString["RECEIPTID"];
                ViewState["STOCKTYPE"] = "";
                if (Request.QueryString["STOCKTYPE"] != null)
                {
                    ViewState["STOCKTYPE"] = Request.QueryString["STOCKTYPE"];
                    ddlStockType.SelectedValue = Request.QueryString["STOCKTYPE"];
                    ddlStockType.Enabled = false;
                }
                rgvForm.PageSize = General.ShowRecords(null);

                if (string.IsNullOrEmpty(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()) || PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() == "0")
                {
                    if (Request.QueryString["VESSELID"] != null)
                    {
                        ViewState["VESSELID"] = Request.QueryString["VESSELID"];
                        ucVessel.SelectedVessel = Request.QueryString["VESSELID"].ToString();
                    }
                    ucVessel.Enabled = false;
                }
                else
                {
                    ucVessel.Enabled = false;
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                }

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

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixCommonPurchase.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void rgvForm_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSTOCKTYPE", "FLDFORMNO", "FLDTITLE", "FLDORDEREDQUANTITY", "FLDRECIEVEDQUANTITY", "FLDBALANCEQUANTITY" };
        string[] alCaptions = { "Type", "Number", "Title", "Ordered Quantity", "Received Quantity", "Balanced Quantity" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : ViewState["SORTEXPRESSION"].ToString();


        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        vesselname = ucVessel.SelectedVessel.ToString();

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseReceipt.ReceiptPoLineItemAddSearch(General.GetNullableInteger(vesselname), txtFormNo.Text.Trim(), txtTitle.Text.Trim()
                                                                , General.GetNullableDateTime(txtFromDate.Text)
                                                                , General.GetNullableDateTime(txtToDate.Text)
                                                                , General.GetNullableString(ddlStockType.SelectedValue)
                                                                , new Guid(ViewState["RECEIPTID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , rgvForm.CurrentPageIndex + 1
                                                                , rgvForm.PageSize
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        rgvForm.DataSource = ds;
        rgvForm.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("rgvForm", "PO Issued", alCaptions, alColumns, ds);
    }

    protected void MenuNewRequisition_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                rgvForm.CurrentPageIndex = 0;
                rgvForm.Rebind();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtFormNo.Text = "";
                txtTitle.Text = "";
                txtFromDate.Text = "";
                txtToDate.Text = "";
                rgvForm.Rebind();
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

    protected void rgvForm_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
        }
    }

    protected void rgvForm_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "UPDATE")
            {
                GridEditableItem item = (GridEditableItem)e.Item;
                string receiptId = ViewState["RECEIPTID"].ToString();
                string orderlineid = ((RadLabel)item.FindControl("lblEditOrderLineId")).Text;
                string orderedqty = ((RadLabel)item.FindControl("txtOrderedQty")).Text;
                string receivedqty = ((UserControlMaskNumber)item.FindControl("txtReceivedQty")).Text;
                string totalrecvdqty = ((RadLabel)item.FindControl("txtTotalReceived")).Text;
                string balanceqty = ((RadLabel)item.FindControl("lblEditBalanceQty")).Text;

                PhoenixPurchaseReceipt.POReceiptLineitemAdd(new Guid(receiptId.ToString())
                                                             , new Guid(orderlineid.ToString())
                                                             , decimal.Parse(orderedqty)
                                                             , decimal.Parse(receivedqty)
                                                             , decimal.Parse(totalrecvdqty)
                                                             , decimal.Parse(balanceqty));
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "closeTelerikWindow('codehelp3', 'codehelp1');", true);
                rgvForm.Rebind();
            }
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
    private bool IsValidQantity(string receivedqty, string balanceqty)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.GetNullableDecimal(receivedqty).HasValue)
        {
            ucError.ErrorMessage = "Received quantity is required.";
        }
        else if (General.GetNullableDecimal(receivedqty) <= 0)
        {
            ucError.ErrorMessage = "Received quantity should be grater than 0.";
        }
        else if (General.GetNullableDecimal(receivedqty) > General.GetNullableDecimal(balanceqty))
        {
            ucError.ErrorMessage = "Received quantity should be less than balance quantity.";
        }
        return (!ucError.IsError);
    }

}