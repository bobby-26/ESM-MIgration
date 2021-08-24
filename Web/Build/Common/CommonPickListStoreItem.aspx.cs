using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CommonPickListStoreItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
        if (Request.QueryString["mode"] == "multi")
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuStockItem.MenuList = toolbarmain.Show();
        if (!IsPostBack)
        {
            if ((Request.QueryString["txtnumber"] != null) && (Request.QueryString["txtnumber"] != ""))
                txtNumberSearch.Text = Request.QueryString["txtnumber"].ToString().TrimEnd("00.00.00".ToCharArray()).TrimEnd("__.__.__".ToCharArray());
            if ((Request.QueryString["txtname"] != null) && (Request.QueryString["txtname"] != ""))
                txtStockItemNameSearch.Text = Request.QueryString["txtname"].ToString();

            if ((Request.QueryString["makerref"] != null) && (Request.QueryString["makerref"] != ""))
                txtProductCode.Text = Request.QueryString["makerref"].ToString();

            ddlStockClass.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();
            ddlStockClass.HardList = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)PhoenixHardTypeCode.STORETYPE);
            if (Request.QueryString["storeclass"] != null)
            {
                ddlStockClass.SelectedHard = Request.QueryString["storeclass"];
                ddlStockClass.Enabled = false;
            }
            if (Request.QueryString["storetype"] != null)
            {
                ddlStockClass.SelectedHard = Request.QueryString["storetype"];
            }
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }
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
                BindData();
            }
            else if (CommandName.ToUpper().Equals("SAVE"))
            {
                NameValueCollection nvc = null;
                string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo');";
                Script += "</script>" + "\n";

                nvc = new NameValueCollection();
                string number = string.Empty, name = string.Empty, id = string.Empty;
                foreach (GridDataItem row in gvStoreItem.Items)
                {
                    UserControlMaskNumber lbl = (UserControlMaskNumber)row.FindControl("txtQuantity");
                    if (lbl == null)
                    {
                        ucError.ErrorMessage = "No Store Item to save";
                        ucError.Visible = true;
                        return;
                    }
                    decimal qty;
                    if (decimal.TryParse(lbl.Text, out qty) && qty > 0)
                    {
                        number += qty.ToString() + "¿";
                        LinkButton lb = (LinkButton)row.FindControl("lnkStockItemName");
                        name += lb.Text.ToString() + "¿";
                        RadLabel lblId = (RadLabel)row.FindControl("lblStockItemId");
                        id += lblId.Text + "¿";
                    }
                }
                nvc.Add("lblStockItemNumber", number.TrimEnd('¿'));
                nvc.Add("lnkStockItemName", name.TrimEnd('¿'));
                nvc.Add("lblStockItemId", id.TrimEnd('¿'));
                Filter.CurrentPickListSelection = nvc;                
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
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
            DataSet ds;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            int vesselid;
            if (Request.QueryString["vesselid"] != null)
                vesselid = int.Parse(Request.QueryString["vesselid"].ToString());
            else
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            ds = PhoenixCommonInventory.StoreItemSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                vesselid, txtNumberSearch.Text, txtStockItemNameSearch.Text,
                null, null, General.GetNullableInteger(ddlStockClass.SelectedHard), sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), gvStoreItem.PageSize,
               ref iRowCount,
               ref iTotalPageCount, 0, txtProductCode.Text);
            gvStoreItem.DataSource = ds;
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
        if (Request.QueryString["mode"] != "multi")
            gvStoreItem.MasterTableView.GetColumn("Quantity").Display = false;

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
    protected void gvStoreItem_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lb = (RadLabel)e.Item.FindControl("lblIsInMarket");

            LinkButton lnkbtn = (LinkButton)e.Item.FindControl("lnkStockItemName");
            Int64 result = 0;

            if (Int64.TryParse(lb.Text, out result))
            {
                e.Item.ForeColor = (result == 0) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
               // lnkbtn.ForeColor = (result == 0) ? System.Drawing.Color.Red : System.Drawing.Color.Black;

            }
            if (Request.QueryString["mode"] == "multi") lnkbtn.Enabled = false;
        }
    }
    protected void gvStoreItem_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string Script = "";
                NameValueCollection nvc = null;

                if (Request.QueryString["mode"] == "custom")
                {

                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnReloadList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";
                    nvc = new NameValueCollection();
                    RadLabel lbl = (RadLabel)e.Item.FindControl("lblStockItemNumber");
                    nvc.Add(lbl.ID, lbl.Text);
                    LinkButton lb = (LinkButton)e.Item.FindControl("lnkStockItemName");
                    nvc.Add(lb.ID, lb.Text.ToString());
                    RadLabel lblId = (RadLabel)e.Item.FindControl("lblStockItemId");
                    nvc.Add(lblId.ID, lblId.Text);

                }
                else
                {
                    Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                    Script += "fnClosePickList('codehelp1','ifMoreInfo');";
                    Script += "</script>" + "\n";
                    nvc = Filter.CurrentPickListSelection;

                    RadLabel lbl = (RadLabel)e.Item.FindControl("lblStockItemNumber");
                    nvc.Set(nvc.GetKey(1), lbl.Text);
                    LinkButton lb = (LinkButton)e.Item.FindControl("lnkStockItemName");
                    nvc.Set(nvc.GetKey(2), lb.Text.ToString());
                    RadLabel lblId = (RadLabel)e.Item.FindControl("lblStockItemId");
                    nvc.Set(nvc.GetKey(3), lblId.Text);
                }

                Filter.CurrentPickListSelection = nvc;
                
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
            }

            else if (e.CommandName == "Page")
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
