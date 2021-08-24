using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class VesselAccountsStoreIssueStoreItemSelection : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        MenuStockItem.AccessRights = this.ViewState;
        MenuStockItem.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            if ((Request.QueryString["txtnumber"] != null) && (Request.QueryString["txtnumber"] != ""))
                txtNumberSearch.Text = Request.QueryString["txtnumber"].ToString().TrimEnd("00.00.00".ToCharArray()).TrimEnd("__.__.__".ToCharArray());
            if ((Request.QueryString["txtname"] != null) && (Request.QueryString["txtname"] != ""))
                txtStockItemNameSearch.Text = Request.QueryString["txtname"].ToString();
            ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();
            ddlStockClass.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)PhoenixHardTypeCode.STORETYPE);
            if (Request.QueryString["storeclass"] != null)
            {
                ddlStockClass.SelectedHard = Request.QueryString["storeclass"];
                ddlStockClass.Enabled = false;
            }
            if (Request.QueryString["storetype"] != null)
                ddlStockClass.SelectedHard = Request.QueryString["storetype"];
            if (Request.QueryString["accountfor"] != null)
            {
                if (Request.QueryString["accountfor"].ToString() == "")
                {
                    ucError.ErrorMessage = "Select Staff name for issue Bonded Store item";
                    ucError.Visible = true;
                }
                ViewState["ACCOUNTFOR"] = Request.QueryString["accountfor"];
            }
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvStoreItem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }
    protected void Rebind()
    {
        gvStoreItem.SelectedIndexes.Clear();
        gvStoreItem.EditIndexes.Clear();
        gvStoreItem.DataSource = null;
        gvStoreItem.Rebind();
    }
    protected void MenuStockItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
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
            int iTotalPageCount = 10;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixVesselAccountsOrderForm.SearchOrderFormStoreItem(null,
                PhoenixSecurityContext.CurrentSecurityContext.VesselID, txtNumberSearch.Text, txtStockItemNameSearch.Text,
                null, null, General.GetNullableInteger(ddlStockClass.SelectedHard), 0, sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), gvStoreItem.PageSize, ref iRowCount, ref iTotalPageCount);
            gvStoreItem.DataSource = dt;
            gvStoreItem.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStoreItem_PreRender(object sender, EventArgs e)
    {

    }
    protected void gvStoreItem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvStoreItem.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStoreItem_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem item = (GridEditableItem)e.Item;

            string quantity = ((RadNumericTextBox)item.FindControl("txtQuantity")).Text;
            string storeitem = ((RadLabel)item.FindControl("lblIssueItemid")).Text;
            string date = ((UserControlDate)item.FindControl("txtIssueDate")).Text;
            string remarks = ((RadTextBox)item.FindControl("txtRemarks")).Text;
            string rob = ((RadLabel)item.FindControl("lblRob")).Text;
            if (IsValidIssue(date, quantity, rob, remarks))
            {
                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("StoreItemId", storeitem);
                nvc.Add("Quantity", quantity);
                nvc.Add("IssueDate", date);
                nvc.Add("Remarks", remarks);
                Filter.CurrentPickListSelection = nvc;
                string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
            }
            else if (!string.IsNullOrEmpty(date) && !string.IsNullOrEmpty(quantity))
            {
                ucError.Visible = true;
                return;
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStoreItem_EditCommand(object sender, GridCommandEventArgs e)
    {
        gvStoreItem.SelectedIndexes.Clear();
        GridDataItem item = (GridDataItem)e.Item;
        gvStoreItem.SelectedIndexes.Add(e.Item.ItemIndex);
    }
    protected void gvStoreItem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                RadLabel lblStockItemNumber = (RadLabel)e.Item.FindControl("lblStockItemNumber");
                RadLabel lblNamedesc = (RadLabel)e.Item.FindControl("lblNamedesc");
                RadLabel lblMakerdesc = (RadLabel)e.Item.FindControl("lblMakerdesc");

                Int64 result = 0;
                if (Int64.TryParse(drv["FLDISINMARKET"].ToString(), out result))
                {
                    //  e.Item.ForeColor = (result == 0) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                    if (result == 0)
                    {
                        lblMakerdesc.Attributes.Add("style", "color:red !important");
                        lblStockItemNumber.Attributes.Add("style", "color:red !important");
                        lblNamedesc.Attributes.Add("style", "color:red !important");
                    }
                    else
                    {
                        lblMakerdesc.Attributes.Add("style", "color:Black !important"); 
                        lblStockItemNumber.Attributes.Add("style", "color:Black !important");
                        lblNamedesc.Attributes.Add("style", "color:Black !important");
                    }
                }
                RadTextBox txtremarks = (RadTextBox)e.Item.FindControl("txtRemarks");
                if (txtremarks != null)
                {
                    if (ViewState["ACCOUNTFOR"].ToString() == "-1" || ViewState["ACCOUNTFOR"].ToString() == "-2")
                        txtremarks.Attributes.Add("class", "input_mandatory");
                    else
                        txtremarks.Attributes.Add("class", "input");
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStoreItem_ItemCommand(object sender, GridCommandEventArgs e)
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
    private bool IsValidIssue(string date, string quantity, string rob, string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        decimal resultDecimal;
        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Sold Quantity is required.";
        }
        else if (decimal.Parse(quantity) <= 0)
        {
            ucError.ErrorMessage = "Sold Quantity should not be zero or negative";
        }
        if (General.GetNullableDecimal(quantity).HasValue && !General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Issue Date is required.";
        }
        else if (General.GetNullableDecimal(quantity).HasValue && DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Issue Date should be earlier than current date";
        }
        if (General.GetNullableDecimal(quantity).HasValue && decimal.TryParse(quantity, out resultDecimal) && resultDecimal > decimal.Parse(rob == string.Empty ? "0" : rob))
            ucError.ErrorMessage = "Please check your stock. Quantity cannot be greater than ROB.";

        if (ViewState["ACCOUNTFOR"].ToString() == "-1" || ViewState["ACCOUNTFOR"].ToString() == "-2")
        {
            if (string.IsNullOrEmpty(remarks))
                ucError.ErrorMessage = "Remarks is Required.";
        }
        return (!ucError.IsError);
    }

}
