using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.VesselAccounts;

public partial class VesselAccountsAdminOrderFormLineItem :  PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCrewSearch.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvCrewSearch.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

                ViewState["ORDERID"] = Request.QueryString["ORDERID"];

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ACTIVE"] = "1";
                if (ViewState["ORDERID"] != null)
                    EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));

            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditBondProvision(Guid gOrderId)
    {
        DataTable dt = PhoenixVesselAccountsOrderForm.EditOrderForm(gOrderId, PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            ViewState["EXCHANGERATE"] = dr["FLDEXCHANGERATE"].ToString();
            ViewState["ACTIVE"] = dr["FLDACTIVEYN"].ToString();
            StoreMenu(dr["FLDSTOCKTYPE"].ToString());
        }
    }
    protected void MenuStoreItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
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
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY", "FLDUNITPRICE", "FLDTOTALPRICE" };
            string[] alCaptions = { "Number", "Name", "Unit", "Quantity", "Unit Price", "Total Price" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixVesselAccountsOrderForm.SearchOrderFormLineItem(General.GetNullableGuid(ViewState["ORDERID"] != null ? ViewState["ORDERID"].ToString() : string.Empty)
                , sortexpression, sortdirection,
               1, iRowCount,
               ref iRowCount,
               ref iTotalPageCount);
            string title = "Purchase of Bonded Stores and Provisions";
            if (ds.Tables[0].Rows.Count > 0)
            {
                title += "<br/> Order No : " + ds.Tables[0].Rows[0]["FLDREFERENCENO"].ToString() + "<br/> Order Date : " + DateTime.Parse(ds.Tables[0].Rows[0]["FLDORDERDATE"].ToString()).ToString("dd/MM/yyyy");
            }
            General.ShowExcel(title, ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDQUANTITY", "FLDUNITPRICE", "FLDTOTALPRICE" };
            string[] alCaptions = { "Number", "Name", "Unit", "Quantity", "Unit Price", "Total Price" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixVesselAccountsOrderForm.SearchOrderFormLineItem(General.GetNullableGuid(ViewState["ORDERID"] != null ? ViewState["ORDERID"].ToString() : string.Empty)
                , sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), 200,
               ref iRowCount,
               ref iTotalPageCount);
            string title = "Purchase of Bonded Stores and Provisions";
            if (ds.Tables[0].Rows.Count > 0)
            {
                title += "Order No : " + ds.Tables[0].Rows[0]["FLDREFERENCENO"].ToString() + "<br/> Order Date : " + DateTime.Parse(ds.Tables[0].Rows[0]["FLDORDERDATE"].ToString()).ToString("dd/MM/yyyy");
            }
            General.SetPrintOptions("gvCrewSearch", title, alCaptions, alColumns, ds);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvCrewSearch.DataSource = ds;
                gvCrewSearch.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCrewSearch);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrewSearch_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            PhoenixVesselAccountsCorrections.DeleteAdminOrderFormLineItem(id);
            EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
    }
    private decimal TotalPrice = 0, LocalPrice = 0, DiscountPrice = 0, ExchangePrice = 0;
    protected void gvCrewSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (ViewState["ACTIVE"].ToString() == "1")
            {
                if (db != null)
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCrewSearch, "Edit$" + e.Row.RowIndex.ToString(), false);
                SetKeyDownScroll(sender, e);
            }

            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            //if (ViewState["ACTIVE"].ToString() != "1") { db.Visible = false; ed.Visible = false; }
            decimal.TryParse(drv["FLDTOTALAMOUNT"].ToString(), out TotalPrice);
            decimal.TryParse(drv["FLDTOTALLOCALPRICE"].ToString(), out LocalPrice);
            decimal.TryParse(drv["FLDDISCOUNTLOCALPRICE"].ToString(), out DiscountPrice);
            decimal.TryParse(drv["FLDEXCHANGEPRICE"].ToString(), out ExchangePrice);
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[5].Text = LocalPrice.ToString() + "<br/>" + DiscountPrice.ToString("0.00") + "<br/>" + TotalPrice.ToString() + "<br/>" + ExchangePrice.ToString();
            e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Font.Bold = true;
        }
    }
    protected void gvCrewSearch_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvCrewSearch_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        if (_gridView.EditIndex > -1)
            _gridView.UpdateRow(_gridView.EditIndex, false);
        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;
        BindData();
        ((TextBox)((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("txtUnitPrice")).FindControl("txtNumber")).Focus();
    }
    protected void gvCrewSearch_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            string lquantity = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("lblQuantity")).Text;
            string lprice = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("lblUnitPrice")).Text;
            string quantity = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantity")).Text;
            string price = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtUnitPrice")).Text;
            if (!IsValidOrderLineItem(price, quantity))
            {
                ucError.Visible = true;
                return;
            }
            if (lquantity != quantity || lprice != price)
            {
                PhoenixVesselAccountsCorrections.UpdateAdminOrderFormLineItem(id, decimal.Parse(quantity), decimal.Parse(price));
            }
            EditBondProvision(new Guid(ViewState["ORDERID"].ToString()));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
    }

    private bool IsValidOrderLineItem(string price, string quantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDecimal(price).HasValue)
        {
            ucError.ErrorMessage = "Unit Price is required.";
        }
        else if (General.GetNullableDecimal(price).HasValue && General.GetNullableDecimal(price).Value <= 0)
        {
            ucError.ErrorMessage = "Unit Price should be greater than zero.";
        }

        if (!General.GetNullableDecimal(quantity).HasValue)
        {
            ucError.ErrorMessage = "Quantity is required.";
        }
        else if (General.GetNullableDecimal(quantity).HasValue && General.GetNullableDecimal(quantity).Value <= 0)
        {
            ucError.ErrorMessage = "Quantity should be greater than zero.";
        }

        return (!ucError.IsError);
    }
    private void StoreMenu(string storeclass)
    {
        if (ViewState["ACTIVE"].ToString() == "1")
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../VesselAccounts/VesselAccountsOrderFormLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCrewSearch')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("javascript:return showPickList('spnPickListStore', 'codehelp1', '', 'VesselAccountsOrderFormStoreItemSelection.aspx?storeclass=" + storeclass + "&orderid=" + ViewState["ORDERID"] + "', true);", "Store Item", "add.png", "ADDSTORE");
            MenuStoreItem.AccessRights = this.ViewState;
            MenuStoreItem.MenuList = toolbar.Show();
            MenuStoreItem.SetTrigger(pnlNTBRManager);
        }
        if (ViewState["ACTIVE"].ToString() == "0")
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../VesselAccounts/VesselAccountsOrderFormLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCrewSearch')", "Print Grid", "icon_print.png", "PRINT");
            MenuStoreItem.AccessRights = this.ViewState;
            MenuStoreItem.MenuList = toolbar.Show();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvCrewSearch.EditIndex = -1;
            gvCrewSearch.SelectedIndex = -1;
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvCrewSearch.SelectedIndex = -1;
        gvCrewSearch.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }
    }
}
