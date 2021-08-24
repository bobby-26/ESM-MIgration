using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.Purchase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PurchaseOrderLineItemSelectService : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        try
        {

            //foreach (GridViewRow r in gvItemList.Rows)
            //{
            //    if (r.RowType == DataControlRowType.DataRow)
            //    {
            //        Page.ClientScript.RegisterForEventValidation(gvItemList.UniqueID, "Edit$" + r.RowIndex.ToString());
            //    }
            //}
            //if (!IsPostBack && ViewState["COMPONENTID"] != null)
            //{
            //    tvwComponent.FindNodeByValue(tvwComponent.ThisTreeView.Nodes, ViewState["COMPONENTID"].ToString());
            //}
            base.Render(writer);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Close", "CLOSE", ToolBarDirection.Right);
            toolbarmain.AddButton("Search", "SEARCH",ToolBarDirection.Right);       
            MenuStoreItemInOutTransaction.AccessRights = this.ViewState;
            MenuStoreItemInOutTransaction.MenuList = toolbarmain.Show();
            //txtComponentID.Attributes.Add("style", "visibility:hidden"); 
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Purchase/PurchaseOrderLineItemSelectService.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('divGrid')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Purchase/PurchaseOrderLineItemSelectService.aspx", "<b>Find</b>", "search.png", "FIND");

            if (!IsPostBack)
            {
                gvItemList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["orderid"] = "0";
                ViewState["COMPONENTID"] = null;
                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                }
                if (Request.QueryString["COMPONENTID"] != null)
                {
                    ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"].ToString();
                }
                if (Request.QueryString["storeitemnumber"] != null)
                    txtPartNumber.Text = Request.QueryString["storeitemnumber"].ToString();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["FIRSTINITIALIZED"] = true;
                BindTreeData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void BindTreeData()
    {
        try
        {
            DataSet ds = new DataSet();

            int vesselid = -1;
            if (Request.QueryString["vesselid"] != null)
                vesselid = int.Parse(Request.QueryString["vesselid"].ToString());
            else
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            if (!string.IsNullOrEmpty(ViewState["COMPONENTID"].ToString()))
                ds = PhoenixInventoryComponent.TreeComponentService(vesselid, new Guid(ViewState["COMPONENTID"].ToString()));
            else
                ds = PhoenixInventoryComponent.TreeComponent(vesselid);
            tvwComponent.DataTextField = "FLDCOMPONENT";
            tvwComponent.DataValueField = "FLDCOMPONENTID";
            tvwComponent.DataFieldParentID = "FLDPARENTID";
            tvwComponent.RootText = "Component";
            tvwComponent.PopulateTree(ds.Tables[0]);

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
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList();";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }

            if (CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvItemList.Rebind();

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
            DataSet ds = PhoenixPurchaseOrderLine.SearchServiceLineItems(General.GetNullableGuid(ViewState["COMPONENTID"].ToString()), Filter.CurrentPurchaseVesselSelection, General.GetNullableGuid(ViewState["orderid"].ToString())
                             , txtPartNumber.Text, txtItemName.Text
                             , sortexpression, 1
                             , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
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
                gvItemList.Rebind();
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
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvItemList.CurrentPageIndex + 1;
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixPurchaseOrderLine.SearchServiceLineItems(General.GetNullableGuid(ViewState["COMPONENTID"].ToString()), Filter.CurrentPurchaseVesselSelection, General.GetNullableGuid(ViewState["orderid"].ToString())
                            , txtPartNumber.Text, txtItemName.Text
                           , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , gvItemList.PageSize
                            , ref iRowCount
                            , ref iTotalPageCount);



            gvItemList.DataSource = ds;
            gvItemList.VirtualItemCount = iRowCount;
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    if (Filter.CurrentPurchaseStockType.ToUpper().Equals("SERVICE"))
            //    {
            //        txtComponent.Text = ds.Tables[0].Rows[0]["FLDCOMPONENTNAME"].ToString();
            //        txtComponentNo.Text = ds.Tables[0].Rows[0]["FLDCOMPONENTNUMBER"].ToString();
            //    }

            //}

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvItemList_Sorting(object sender, GridSortCommandEventArgs e)
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
        gvItemList.Rebind();
    }

 

    protected void gvItemList_RowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {

            string unit = ((UserControlPurchaseUnit)e.Item.FindControl("ucUnit")).SelectedUnit;
            string quantity = ((RadTextBox)e.Item.FindControl("txtQuantityEdit")).Text;

            if (!IsValidLineItemEdit(quantity, unit))
            {
                ucError.Visible = true;
                return;
            }
            else
            {
                InsertOrderLineItem(
                       ((RadLabel)e.Item.FindControl("lblWorkOrderId")).Text,
                       ((RadLabel)e.Item.FindControl("lblOrderLineId")).Text,
                       ((RadLabel)e.Item.FindControl("lblComponentId")).Text,
                       ((RadTextBox)e.Item.FindControl("txtQuantityEdit")).Text,
                       ((UserControlPurchaseUnit)e.Item.FindControl("ucUnit")).SelectedUnit
                    );
            }
            gvItemList.DataSource = null;
            gvItemList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvItemList_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    private void InsertOrderLineItem(string storeitemid, string orderlineid, string componentid, string quantity, string unit)
    {
        try
        {
            if (quantity.Trim() == "")
            {
                return;
            }
            if (!IsValidLineItemEdit(quantity, unit))
            {
                ucError.Visible = true;
                return;
            }
            else
            {
                if (General.GetNullableGuid(orderlineid) == null)
                {
                    PhoenixPurchaseOrderLine.InsertServiceOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(storeitemid), General.GetNullableGuid(componentid), new Guid(ViewState["orderid"].ToString())
                         , General.GetNullableDecimal(quantity), Filter.CurrentPurchaseStockType.ToString(), General.GetNullableInteger(unit.ToString()));
                }
                else
                {
                    PhoenixPurchaseOrderLine.UpdateOrderLine(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(orderlineid)
                        , General.GetNullableDecimal(quantity), General.GetNullableInteger(unit.ToString()));
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsValidLineItemEdit(string quantity, string unit)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        decimal result;

        if (quantity.Trim() != "")
        {
            if (decimal.TryParse(quantity, out result) == false)
                ucError.ErrorMessage = "Item requested quantity should be a valid numeric value.";
        }
        if (unit.ToUpper().Trim() == "DUMMY" || unit.Trim() == "")
            ucError.ErrorMessage = "Unit is Required.";
        return (!ucError.IsError);
    }


    protected void gvItemList_RowEditing(object sender, GridCommandEventArgs de)
    {
        try
        {

            ((RadTextBox)de.Item.FindControl("txtQuantityEdit")).Focus();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvItemList_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridEditableItem)
            {
                UserControlPurchaseUnit unit = (UserControlPurchaseUnit)e.Item.FindControl("ucUnit");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (unit != null)
                {
                    unit.VesslId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                    unit.SelectedUnit = drv["FLDUNITID"].ToString();
                }
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
            gvItemList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetKeyDownScroll(object sender, GridCommandItem e)
    {
        //int nextRow = 0;
        //GridView _gridView = (GridView)sender;

        //if (e.Row.RowType == DataControlRowType.DataRow
        //    && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        //    || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        //{

        //    int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

        //    String script = "var keyValue = SelectSibling(event); ";
        //    script += " if(keyValue == 38) {";  //Up Arrow
        //    nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

        //    script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
        //    script += "}";

        //    script += " if(keyValue == 40) {";  //Down Arrow
        //    nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

        //    script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
        //    script += "}";
        //    script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
        //    e.Row.Attributes["onkeydown"] = script;
        //}
    }

    protected void ucTree_SelectNodeEvent(object sender, EventArgs e)
    {
        try
        {
            RadTreeNodeEventArgs args = (RadTreeNodeEventArgs)e;
            txtPartNumber.Text = "";
            txtItemName.Text = "";
            ViewState["COMPONENTID"] = "";

            if (args.Node.Value != "Root")
            {
                string selectednode = args.Node.Value.ToString();
                string selectedvalue = args.Node.Text.ToString();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["COMPONENTID"] = selectednode;
                txtComponent.Text = selectedvalue;

                if (General.GetNullableGuid(selectednode) != null)
                {
                    DataTable dt = PhoenixInventoryComponent.EditComponent(new Guid(selectednode), Filter.CurrentPurchaseVesselSelection);
                    if (dt.Rows.Count > 0)
                    {
                        txtComponentNo.Text = dt.Rows[0]["FLDCOMPONENTNUMBER"].ToString();
                    }

                }
                

            }
            //string script = "var ar = document.getElementById(\"tvwComponent_tvwTreet0\"); ar.href=\"#\"; ar.onclick=null;\r\n;resizew();";
            //ScriptManager.RegisterStartupScript(this, typeof(Page), "disableScript", script, true);
            gvItemList.DataSource = null;
            gvItemList.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvItemList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
