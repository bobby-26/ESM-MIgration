using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;

public partial class CrewWorkingGearOrderFormGeneral : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvWorkGearItem.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvWorkGearItem.UniqueID, "Edit$" + r.RowIndex.ToString());
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
               
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ORDERSTATUS"] = "";

                DataSet dsCurrency = PhoenixRegistersCurrency.ListCurrency(1, "INR");
                if (dsCurrency.Tables[0].Rows[0]["FLDCURRENCYID"] != null)
                {
                    ddlCurrency.SelectedCurrency = dsCurrency.Tables[0].Rows[0]["FLDCURRENCYID"].ToString();
                    ViewState["LOCALCURRENCY"] = dsCurrency.Tables[0].Rows[0]["FLDCURRENCYID"].ToString();
                    
                }
                else
                {
                    ViewState["LOCALCURRENCY"] = "";
                }

                
                ViewState["ORDERID"] = Request.QueryString["ORDERID"];
                ViewState["ACTIVE"] = "1";

                if (ViewState["ORDERID"] != null)
                    EditWorkGearOrder(new Guid(ViewState["ORDERID"].ToString()));
      
            }
            if (ViewState["ORDERID"] != null)
                divItems.Visible = true;
            else
                divItems.Visible = false;
            MainMenu();

            BindData();


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuWorkGearGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidOrder(ucMultiAddr.SelectedValue, txtOrderDate.Text, txtDiscount.Text, txtReceivedDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["ORDERID"] != null)
                {
                    PhoenixCrewWorkingGearOrderForm.UpdateOrderForm(new Guid(ViewState["ORDERID"].ToString())
                        , DateTime.Parse(txtOrderDate.Text)
                        , int.Parse(ucMultiAddr.SelectedValue)
                        , General.GetNullableInteger(ddlCurrency.SelectedCurrency)
                        ,null//, General.GetNullableDecimal(txtExchangeRate.Text)
                        , General.GetNullableDecimal("")
                        , null
                        , General.GetNullableDecimal(txtDiscount.Text)
                        , General.GetNullableDateTime(txtReceivedDate.Text)
                        , General.GetNullableString(txtRemarks.Text)
                        , General.GetNullableInteger(ucZone.SelectedZone));

                    ucStatus.Text = "Requistion Details updated successfully.";
                }
                else
                {
                    Guid outorderid = new Guid();
                    PhoenixCrewWorkingGearOrderForm.InsertOrderForm(DateTime.Parse(txtOrderDate.Text)
                                                                    , int.Parse(ucMultiAddr.SelectedValue)
                                                                    , General.GetNullableInteger(ddlCurrency.SelectedCurrency)
                                                                    , null, null, null
                                                                    , General.GetNullableInteger(ucZone.SelectedZone)
                                                                    , ref outorderid );
                    ucStatus.Text = "New Requistion saved successfully.";

                    if (outorderid != null)
                    {
                        ViewState["ORDERID"] = outorderid.ToString();
                        divItems.Visible = true;
                    }
                }
                EditWorkGearOrder(new Guid(ViewState["ORDERID"].ToString()));

                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("CrewWorkingGearOrderForm.aspx", false);
            }
            if (dce.CommandName.ToUpper().Equals("CONFIRM"))
            {
                if (!IsValidConfirm(ucMultiAddr.SelectedValue, txtOrderDate.Text, txtDiscount.Text, txtReceivedDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewWorkingGearOrderForm.UpdateOrderForm(new Guid(ViewState["ORDERID"].ToString())
                    , DateTime.Parse(txtOrderDate.Text)
                    , int.Parse(ucMultiAddr.SelectedValue)
                    , General.GetNullableInteger(ddlCurrency.SelectedCurrency)
                    ,null//, General.GetNullableDecimal(txtExchangeRate.Text)
                    , General.GetNullableDecimal("")
                    , null
                    , General.GetNullableDecimal(txtDiscount.Text)
                    , General.GetNullableDateTime(txtReceivedDate.Text)
                    , General.GetNullableString(txtRemarks.Text)
                    , General.GetNullableInteger(ucZone.SelectedZone));

                //PhoenixCrewWorkingGearOrderForm.ConfirmOrderFormReceive(new Guid(ViewState["ORDERID"].ToString()));

                ucStatus.Text = "Requistion Status is Changed as Received.";

                EditWorkGearOrder(new Guid(ViewState["ORDERID"].ToString()));

                BindData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditWorkGearOrder(Guid gOrderId)
    {
        DataTable dt = PhoenixCrewWorkingGearOrderForm.EditOrderForm(gOrderId);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtRefNo.Text = dr["FLDREFERENCENO"].ToString();
            ucMultiAddr.SelectedValue = dr["FLDADDRESSCODE"].ToString();
            ucMultiAddr.Text = dr["FLDNAME"].ToString();
            txtOrderDate.Text = dr["FLDORDERDATE"].ToString();
            ddlCurrency.SelectedCurrency = dr["FLDCURRENCYID"].ToString();
            //txtExchangeRate.Text = dr["FLDEXCHANGERATE"].ToString();
            txtTotalAmount.Text = dr["FLDTOTALAMOUNT"].ToString();
            txtDiscount.Text = dr["FLDDISCOUNT"].ToString();
            txtReceivedDate.Text = dr["FLDRECEIVEDDATE"].ToString();
            txtRemarks.Text = dr["FLDRECEIPTREMARKS"].ToString();
            ViewState["ACTIVE"] = dr["FLDACTIVEYN"].ToString();
            ViewState["ORDERSTATUS"] = dr["FLDORDERSTATUSNAME"].ToString();


            if (String.IsNullOrEmpty(dr["FLDCURRENCYID"].ToString()))
                ddlCurrency.SelectedCurrency = ViewState["LOCALCURRENCY"].ToString();

            if (!String.IsNullOrEmpty(dr["FLDRECEIVEDZONE"].ToString()))
                ucZone.SelectedZone = dr["FLDRECEIVEDZONE"].ToString();

            if (ViewState["ACTIVE"].ToString() == "1")
                ViewState["ACTIVE"] = dr["FLDREQUESTTYPE"].ToString().Equals("2") ? "0" : "1";

            
            WorkGearItemMenu();
            EnablePage();
        }
    }

    private void EnablePage()
    {
        bool editable = ViewState["ACTIVE"].Equals("0") ? false : true; //Enable or disable all controls

        txtOrderDate.Enabled = editable;
        ddlCurrency.Enabled = editable;
        //txtExchangeRate.Enabled = editable;
        txtTotalAmount.Enabled = editable;
        txtDiscount.Enabled = editable;
        txtReceivedDate.Enabled = editable;
        txtRemarks.Enabled = editable;
        ucZone.Enabled = editable;
    }

    protected void MenuWorkGearItem_TabStripCommand(object sender, EventArgs e)
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
            string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDUNITNAME", "FLDQUANTITY", "FLDUNITPRICE", "FLDTOTALPRICE" };
            string[] alCaptions = { "Name", "Unit", "Quantity", "Unit Price", "Total Price" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixCrewWorkingGearOrderForm.SearchOrderFormLineItem(General.GetNullableGuid(ViewState["ORDERID"] != null ? ViewState["ORDERID"].ToString() : string.Empty)
                , sortexpression, sortdirection,
               1, iRowCount,
               ref iRowCount,
               ref iTotalPageCount);
            string title = "Requisition Details";
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

            string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDUNITNAME", "FLDQUANTITY", "FLDUNITPRICE", "FLDTOTALPRICE" };
            string[] alCaptions = { "Name", "Unit", "Quantity", "Unit Price", "Total Price" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewWorkingGearOrderForm.SearchOrderFormLineItem(General.GetNullableGuid(ViewState["ORDERID"] != null ? ViewState["ORDERID"].ToString() : string.Empty)
                , sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), 50,
               ref iRowCount,
               ref iTotalPageCount);
            string title = "Requisition of Working Gear";
            if (ds.Tables[0].Rows.Count > 0)
            {
                title += "Order No : " + ds.Tables[0].Rows[0]["FLDREFERENCENO"].ToString() + "<br/> Order Date : " + DateTime.Parse(ds.Tables[0].Rows[0]["FLDORDERDATE"].ToString()).ToString("dd/MM/yyyy");
            }
            General.SetPrintOptions("Requisition Details", title, alCaptions, alColumns, ds);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvWorkGearItem.DataSource = ds;
                gvWorkGearItem.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvWorkGearItem);
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

    protected void gvWorkGearItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            PhoenixCrewWorkingGearOrderForm.DeleteOrderFormLineItem(id);
            EditWorkGearOrder(new Guid(ViewState["ORDERID"].ToString()));
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
    protected void gvWorkGearItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (ViewState["ACTIVE"].ToString() == "1")
            {
                if (db != null)
                    e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvWorkGearItem, "Edit$" + e.Row.RowIndex.ToString(), false);
                SetKeyDownScroll(sender, e);
            }

            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            if (ViewState["ACTIVE"].ToString() != "1") { db.Visible = false; ed.Visible = false; }
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

    protected void gvWorkGearItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvWorkGearItem_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        if (_gridView.EditIndex > -1)
            _gridView.UpdateRow(_gridView.EditIndex, false);
        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;
        BindData();
        ((TextBox)((UserControlMaskNumber)_gridView.Rows[e.NewEditIndex].FindControl("txtUnitPrice")).FindControl("txtNumber")).Focus();
    }

    protected void gvWorkGearItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            string quantity = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantity")).Text;
            string price = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtUnitPrice")).Text;
            //string sizeid=((Label)_gridView.Rows[nCurrentRow].FindControl("lblSizeid")).Text;
            if (!IsValidOrderLineItem(price, quantity))
            {
                ucError.Visible = true;
                return;
            }
            //PhoenixCrewWorkingGearOrderForm.UpdateOrderFormLineItem(id, decimal.Parse(quantity), decimal.Parse(price));
            EditWorkGearOrder(new Guid(ViewState["ORDERID"].ToString()));
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

    private bool IsValidOrder(string supplier, string orderdate, string discount, string receiveddate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(orderdate).HasValue)
        {
            ucError.ErrorMessage = "Order Date is required.";
        }
        else if (DateTime.TryParse(orderdate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Order Date should not be later than current date";
        }
        if (!General.GetNullableInteger(supplier).HasValue)
        {
            ucError.ErrorMessage = "Supplier Name is required.";
        }
        if (General.GetNullableDecimal(discount).HasValue && General.GetNullableDecimal(discount).Value > 100)
        {
            ucError.ErrorMessage = "Discount should be between 0 and 100";
        }
        if (General.GetNullableDateTime(receiveddate).HasValue && General.GetNullableDateTime(orderdate).HasValue)
        {
            if (DateTime.Parse(receiveddate) > DateTime.Today)
                ucError.ErrorMessage = "Received Date should not be greater than today Date.";

            if (DateTime.Parse(orderdate) > DateTime.Parse(receiveddate))
                ucError.ErrorMessage = "Received Date should not be less than Order Date.";
        }
        //if (ddlCurrency.SelectedCurrency != ViewState["LOCALCURRENCY"].ToString() && !String.IsNullOrEmpty(ViewState["LOCALCURRENCY"].ToString()))
        //{
        //    //if (String.IsNullOrEmpty(txtExchangeRate.Text.Trim()))
        //    //    ucError.ErrorMessage = "Exchange rate should not be empty other than INR.";
        //}
        return (!ucError.IsError);
    }

    private bool IsValidConfirm(string supplier, string orderdate, string discount, string receiveddate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(orderdate).HasValue)
        {
            ucError.ErrorMessage = "Order Date is required.";
        }
        else if (DateTime.TryParse(orderdate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Order Date should be earlier than current date";
        }
        if (!General.GetNullableInteger(supplier).HasValue)
        {
            ucError.ErrorMessage = "Supplier Name is required.";
        }
        if (General.GetNullableDecimal(discount).HasValue && General.GetNullableDecimal(discount).Value > 100)
        {
            ucError.ErrorMessage = "Discount should be between 0 and 100";
        }
        if (!General.GetNullableDateTime(receiveddate).HasValue)
        {
            ucError.ErrorMessage = "Received Date is required.";
        }
        if (General.GetNullableDateTime(receiveddate).HasValue && General.GetNullableDateTime(orderdate).HasValue)
        {
            if (DateTime.Parse(receiveddate) > DateTime.Today)
                ucError.ErrorMessage = "Received Date should not be greater than today Date.";

            if (DateTime.Parse(orderdate) > DateTime.Parse(receiveddate))
                ucError.ErrorMessage = "Received Date should not be less than Order Date.";
        }
        //if (ddlCurrency.SelectedCurrency != ViewState["LOCALCURRENCY"].ToString() && !String.IsNullOrEmpty(ViewState["LOCALCURRENCY"].ToString()))
        //{
        //    if (String.IsNullOrEmpty(txtExchangeRate.Text.Trim()))
        //        ucError.ErrorMessage = "Exchange rate should not be empty other than INR.";
        //}
        if (!General.GetNullableInteger(ucZone.SelectedZone).HasValue)
            ucError.ErrorMessage = "Requisition for the Zone should not be empty before confirm.";

        return (!ucError.IsError);
    }

    private void MainMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Back", "BACK");

        if (ViewState["ORDERID"] != null)
        {

            if (ViewState["ACTIVE"] == null || ViewState["ACTIVE"].ToString() == "1")
            {
                toolbar.AddButton("Save", "SAVE");
                toolbar.AddButton("Confirm Received", "CONFIRM");
            }
        }
        else
        {
            toolbar.AddButton("Save", "SAVE");
        }

        MenuWorkGearGeneral.AccessRights = this.ViewState;
        MenuWorkGearGeneral.MenuList = toolbar.Show();
    }

    private void WorkGearItemMenu()
    {
        if (ViewState["ACTIVE"].ToString() == "1")
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewWorkingGearOrderFormGeneral.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvWorkGearItem')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("javascript:return showPickList('spnPickListStore', 'codehelp1', '', 'CrewWorkingGearOrderFormStoreItemSelection.aspx?orderid=" + ViewState["ORDERID"] + "', true);", "Working Gear Item", "add.png", "ADDITEM");

            MenuWorkGearItem.AccessRights = this.ViewState;
            MenuWorkGearItem.MenuList = toolbar.Show();
            MenuWorkGearItem.SetTrigger(pnlNTBRManager);
        }
        if (ViewState["ACTIVE"].ToString() == "0")
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewWorkingGearOrderFormGeneral.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvWorkGearItem')", "Print Grid", "icon_print.png", "PRINT");
            MenuWorkGearItem.AccessRights = this.ViewState;
            MenuWorkGearItem.MenuList = toolbar.Show();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            gvWorkGearItem.EditIndex = -1;
            gvWorkGearItem.SelectedIndex = -1;
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
        gvWorkGearItem.SelectedIndex = -1;
        gvWorkGearItem.EditIndex = -1;
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

    protected void CurrencySelection(object sender, EventArgs e)
    {
        //if (!string.IsNullOrEmpty(ViewState["LOCALCURRENCY"].ToString()) && ddlCurrency.SelectedCurrency != ViewState["LOCALCURRENCY"].ToString())
        //    txtExchangeRate.CssClass = "input_mandatory";
        //else
        //    txtExchangeRate.CssClass = "input";
    }

}
