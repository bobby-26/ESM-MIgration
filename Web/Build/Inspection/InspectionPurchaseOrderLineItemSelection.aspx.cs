using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionPurchaseOrderLineItemSelection : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Close", "CLOSE", ToolBarDirection.Right);
            toolbarmain.AddButton("Search", "SEARCH", ToolBarDirection.Right);
            MenuStoreItemInOutTransaction.AccessRights = this.ViewState;
            MenuStoreItemInOutTransaction.MenuList = toolbarmain.Show();

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionPurchaseOrderLineItemSelection.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('divGrid')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionPurchaseOrderLineItemSelection.aspx", "<b>Find</b>", "<i class=\"fas fa-search\"></i>", "FIND");

            if (!IsPostBack)
            {
                ViewState["orderid"] = "0";
                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                }
                if (Request.QueryString["storeitemnumber"] != null)
                    txtPartNumber.Text = Request.QueryString["storeitemnumber"].ToString();
                ddlStockClassType.HardTypeCode = ((int)PhoenixHardTypeCode.STORETYPE).ToString();
                if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("STORE"))
                {
                    ddlStockClassType.Visible = true;
                    if (Filter.CurrentInspectionPurchaseStockClass == "")
                        Filter.CurrentInspectionPurchaseStockClass = "407";
                    ddlStockClassType.SelectedHard = Filter.CurrentInspectionPurchaseStockClass;
                    lblClassName.Text = "Store Type";
                }
                if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("SPARE"))
                {
                    ddlComponentClass.DataSource = PhoenixInspectionAuditPurchaseForm.GetComponentHeads(Filter.CurrentInspectionPurchaseVesselSelection);
                    ddlComponentClass.DataBind();
                    ddlComponentClass.Visible = true;
                    lblClassName.Text = "Component";
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["FIRSTINITIALIZED"] = true;
                ViewState["COMPONENTID"] = "";

                if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("STORE"))
                    lblMakerRef.Text = "Product Code";
                else
                    lblMakerRef.Text = "Maker Reference";

                gvItemList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void StoreItemInOutTransaction_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CLOSE"))
            {
                String script = String.Format("javascript:parent.fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["COMPONENTID"] = "";
                Rebind();
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "MAKER", "PREFERREDVENDOR", "FLDWANTED" };
            string[] alCaptions = { "Number", "Stock Item name", "Maker", "Preferred Vender", "Wanted" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            string stockclass = "";
            if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("STORE"))
                stockclass = ddlStockClassType.SelectedHard == "" ? Filter.CurrentInspectionPurchaseStockClass : ddlStockClassType.SelectedHard;


            DataSet ds = PhoenixInspectionAuditPurchaseForm.SearchWantedLineItems(
                            General.GetNullableGuid(ddlComponentClass.SelectedValue.ToString())
                             , Filter.CurrentInspectionPurchaseVesselSelection, General.GetNullableGuid(ViewState["orderid"].ToString())
                             , txtPartNumber.Text, txtItemName.Text, txtMakerReference.Text
                             , null
                             , null, General.GetNullableInteger(stockclass), sortexpression, 1
                             , 1
                             , iRowCount
                             , ref iRowCount
                             , ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=StockItemWanted.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='" + HttpContext.Current.Session["sitepath"] + "/images/esmlogo4_small.png' /></td>");
            Response.Write("<td><h3>Stock Item Wanted</h3></td>");
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

    protected void MenuGridStoreItemInOutTransaction_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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

    private void BindData()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            string stockclass = "";
            if (Filter.CurrentInspectionPurchaseStockType.ToUpper().Equals("STORE"))
                stockclass = ddlStockClassType.SelectedHard == "" ? Filter.CurrentInspectionPurchaseStockClass : ddlStockClassType.SelectedHard;

            DataSet ds = PhoenixInspectionAuditPurchaseForm.SearchWantedLineItems(General.GetNullableGuid(ddlComponentClass.SelectedValue.ToString()), Filter.CurrentInspectionPurchaseVesselSelection, General.GetNullableGuid(ViewState["orderid"].ToString())
                             , txtPartNumber.Text, txtItemName.Text, General.GetNullableString(txtMakerReference.Text)
                             , null
                             , null, General.GetNullableInteger(stockclass), sortexpression, sortdirection
                             , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                             , gvItemList.PageSize
                             , ref iRowCount
                             , ref iTotalPageCount);

            gvItemList.DataSource = ds;
            gvItemList.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void StockItemNameClick(object sender, CommandEventArgs e)
    {
       
    }
    protected void gvItemList_ItemCommand(object sender, GridCommandEventArgs e)
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
    private void InsertOrderLineItem(string storeitemid, string orderlineid, string componentid, string quantity)
    {
        try
        {
            if (quantity.Trim() == "")
            {
                return;
            }
            if (!IsValidLineItemEdit(quantity))
            { 
                ucError.Visible = true;
                return;
            }
            if (General.GetNullableGuid(orderlineid) == null)
            {
                PhoenixInspectionAuditPurchaseForm.InsertOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(storeitemid), General.GetNullableGuid(componentid), new Guid(ViewState["orderid"].ToString())
                     , General.GetNullableDecimal(quantity), Filter.CurrentInspectionPurchaseStockType.ToString());
                Rebind();
            }
            else
            {
                PhoenixInspectionAuditPurchaseForm.UpdateOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(orderlineid), General.GetNullableDecimal(quantity));
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidLineItemEdit(string quantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        RadGrid _gridView = gvItemList;
        decimal result;

        if (quantity.Trim() != "")
        {
            if (decimal.TryParse(quantity, out result) == false)
                ucError.ErrorMessage = "Item requested quantity should be a valid numeric value.";
        }
        return (!ucError.IsError);
    }
    protected void gvItemList_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadLabel lb = (RadLabel)e.Item.FindControl("lblIsInMarket");
                Int64 result = 0;

                if (Int64.TryParse(lb.Text, out result))
                {
                    e.Item.ForeColor = (result == 0) ? System.Drawing.Color.Red : System.Drawing.Color.Black;
                }

                RadTextBox tb = (RadTextBox)e.Item.FindControl("txtQuantityEdit");
                if (tb != null) tb.Focus();
            }

            RadLabel lbm = (RadLabel)e.Item.FindControl("lblMakerReferenceHeader");
            if (lbm != null && Filter.CurrentInspectionPurchaseStockType == "STORE")
            {
                lbm.Text = "Product Code";
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
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvItemList_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            InsertOrderLineItem(
                   ((RadLabel)e.Item.FindControl("lblStoreItemId")).Text,
                   ((RadLabel)e.Item.FindControl("lblOrderLineId")).Text,
                   ((RadLabel)e.Item.FindControl("lblComponentId")).Text,
                   ((RadTextBox)e.Item.FindControl("txtQuantityEdit")).Text
                );
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvItemList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvItemList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvItemList.SelectedIndexes.Clear();
        gvItemList.EditIndexes.Clear();
        gvItemList.DataSource = null;
        gvItemList.Rebind();
    }
}
