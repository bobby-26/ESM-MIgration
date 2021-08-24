using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Purchase_PurchaseQuotationComparisonBulkSave : PhoenixBasePage
{
    ArrayList arrayUser = new ArrayList();
    ArrayList arrayDiscount = new ArrayList();
    ArrayList arrayFooter = new ArrayList();
    ArrayList arrayvendorstatus = new ArrayList();
    DataSet ds;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                pnlErrorMessage.Style["position"] = "absolute";
                pnlErrorMessage.Style["left"] = "350px";
                pnlErrorMessage.Style["filter"] = "alpha(opacity=95)";
                pnlErrorMessage.Style["-moz-opacity"] = ".95";
                pnlErrorMessage.Style["opacity"] = ".95";
                pnlErrorMessage.Style["z-index"] = "99";
                pnlErrorMessage.Style["background-color"] = "White";
                pnlErrorMessage.Style["display"] = "none";

                PhoenixToolbar toolSave = new PhoenixToolbar();
                //toolSave.AddButton("Save", "SAVE");
                toolSave.AddImageLink("javascript:bulkSaveOfOrderLine(); return false;", "Save", "", "BULKSAVE");
                toolSave.AddButton("Cancel", "CANCEL");
                menuSaveDetails.MenuList = toolSave.Show();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (Request.QueryString["orderid"] != null)
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                    ViewState["selectvendors"] = Request.QueryString["vendors"].ToString();

                    Page.ClientScript.RegisterHiddenField("hidOrderId", ViewState["orderid"].ToString());
                }
                else
                {
                    ViewState["orderid"] = "0";
                    ViewState["selectvendors"] = "";
                }


            }

            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void menuSaveDetails_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnClosePickList('codehelp1', 'ifMoreInfo');";
            Script += "</script>" + "\n";

            if (dce.CommandName.ToUpper().Equals("CANCEL"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "CloseWindow('codehelp1');", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void pnlErrorMessage_Load(object sender, EventArgs e)
    {
        string str = "var dg = document.getElementById(\"" + pnlErrorMessage.ClientID + "\");";
        str += "if (dg != null)";
        str += "{";
        str += "dg.style.top = Number((window.pageYOffset ? window.pageYOffset : (document.documentElement ? document.documentElement.scrollTop : (document.body ? document.body.scrollTop : 0)))) + \"px\";";
        str += "}";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", str, true);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixPurchaseQuotation.QuotationComparison(General.GetNullableGuid(ViewState["orderid"].ToString()), ViewState["selectvendors"].ToString(), int.Parse(Filter.CurrentPurchaseVesselSelection.ToString()));
        arrayUser.Clear();
        arrayFooter.Clear();
        arrayDiscount.Clear();
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            arrayFooter.Add(dr["FLDTOTALAMOUNT"].ToString());
            arrayFooter.Add(dr["FLDTOTALUSDAMOUNT"].ToString());
            arrayDiscount.Add(dr["FLDDISCOUNT"].ToString());
            arrayvendorstatus.Add(dr["FLDQUOTATIONSTATUS"].ToString());
        }
        if (ds.Tables[1].Rows.Count > 0)
        {
            gvVendor.DataSource = ds.Tables[1];
            gvVendor.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvVendor);
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            if (gvVendor.Columns.Count == 10)
            {
                AddColumnsInGrid(ds);
            }
            DataTable dt = ds.Tables[0];
            GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
            row.Attributes.Add("style", "position:static");
            TableCell cell = new TableCell();
            cell.ColumnSpan = 7;
            row.Cells.Add(cell);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cell = new TableCell();
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Text = dt.Rows[i]["FLDNAME"].ToString();
                cell.ColumnSpan = 4;
                row.Cells.Add(cell);

                gvVendor.Controls[0].Controls.AddAt(0, row);
            }
            cell = new TableCell();
            cell.HorizontalAlign = HorizontalAlign.Center;
            cell.Text = "";
            row.Cells.Add(cell);

            gvVendor.Controls[0].Controls.AddAt(0, row);
        }
        AddFooter();
        CheckLowestPrice();
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void AddColumnsInGrid(DataSet ds)
    {

        DataTable datatable = new DataTable();
        datatable = ds.Tables[0];
        DataTable detailsdt = new DataTable();
        detailsdt = ds.Tables[2];
        int k = 0;
        for (int i = 0; i < datatable.Rows.Count; i++)
        {
            BoundField UnitPrice = new BoundField();
            UnitPrice.HeaderText = "Unit Price";
            gvVendor.Columns.Insert(gvVendor.Columns.Count - 1, UnitPrice);

            BoundField ItemPrice = new BoundField();
            ItemPrice.HeaderText = "Item Price";
            gvVendor.Columns.Insert(gvVendor.Columns.Count - 1, ItemPrice);

            BoundField Discount = new BoundField();
            Discount.HeaderText = "Discount Price [ " + arrayDiscount[k].ToString() + "% ]";
            gvVendor.Columns.Insert(gvVendor.Columns.Count - 1, Discount);

            BoundField Delay = new BoundField();
            Delay.HeaderText = "Del. Time(Days)";
            gvVendor.Columns.Insert(gvVendor.Columns.Count - 1, Delay);

            gvVendor.DataBind();

            k++;
        }
    }

    private void CheckLowestPrice()
    {
        if (gvVendor.Columns.Count > 12)
        {
            foreach (GridViewRow gvr in gvVendor.Rows)
            {
                decimal lowprice = CheckDecimal(gvr.Cells[9].Text);
                int j = 9;
                for (int i = 13; i < gvVendor.Columns.Count - 1; i += 4)
                {
                    if (lowprice > CheckDecimal(gvr.Cells[i].Text))
                    {
                        j = i;
                        lowprice = CheckDecimal(gvr.Cells[i].Text);
                    }
                }
                if (!string.IsNullOrEmpty(gvr.Cells[j].Text))
                {
                    gvr.Cells[j].BackColor = System.Drawing.Color.Yellow;
                }
            }
            GridViewRow gvfr = gvVendor.FooterRow;
            int m = 0;
            int k = 10;
            decimal lowtotalprice = Decimal.Parse("0.00");
            for (int a = 0; a < arrayvendorstatus.Count; a++)
            {
                if (arrayvendorstatus[a].ToString() == "FULL")
                {
                    k = 10 + (a * 4);
                    lowtotalprice = CheckDecimal(gvfr.Cells[k].Text);
                    break;
                }

            }
            m = 0;
            for (int i = 10; i < gvVendor.Columns.Count - 1; i += 4)
            {
                if (arrayvendorstatus[m].ToString() == "FULL")
                {
                    if (lowtotalprice > CheckDecimal(gvfr.Cells[i].Text))
                    {
                        k = i;
                        lowtotalprice = CheckDecimal(gvfr.Cells[i].Text);
                    }
                }
                m++;
            }
            gvfr.Cells[k].BackColor = System.Drawing.Color.GreenYellow;
            gvfr.Cells[k].ForeColor = System.Drawing.Color.Red;

            m = 0;
            k = 11;

            for (int a = 0; a < arrayvendorstatus.Count; a++)
            {
                if (arrayvendorstatus[a].ToString() == "FULL")
                {
                    k = 11 + (a * 4);
                    lowtotalprice = CheckDecimal(gvfr.Cells[k].Text);
                    break;

                }
            }
            m = 0;

            for (int i = 11; i < gvVendor.Columns.Count - 1; i += 4)
            {
                if (arrayvendorstatus[m].ToString() == "FULL")
                {
                    if (lowtotalprice > CheckDecimal(gvfr.Cells[i].Text) && arrayvendorstatus[m].ToString() == "FULL")
                    {
                        k = i;
                        lowtotalprice = CheckDecimal(gvfr.Cells[i].Text);
                    }
                }
                m++;
            }
            gvfr.Cells[k].ForeColor = System.Drawing.Color.Red;
            gvfr.Cells[k].BackColor = System.Drawing.Color.GreenYellow;
        }
    }

    private void AddFooter()
    {
        int j = 0;
        GridViewRow gvfr = gvVendor.FooterRow;
        try
        {
            for (int i = 10; i < gvVendor.Columns.Count - 1; i += 4)
            {
                gvfr.Cells[i].Text = arrayFooter[j].ToString();
                gvfr.Cells[i].HorizontalAlign = HorizontalAlign.Right;
                gvfr.Cells[i + 1].Text = arrayFooter[j + 1].ToString();
                gvfr.Cells[i + 1].HorizontalAlign = HorizontalAlign.Right;
                j = j + 2;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private decimal CheckDecimal(string decimalvalue)
    {
        if (General.GetNullableDecimal(decimalvalue) != null)
            return decimal.Parse(decimalvalue);
        else
            return decimal.Parse("999999999999999.00");
    }

    protected void gvVendor_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvVendor_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = int.Parse(e.CommandArgument.ToString());

        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                UserControlMaskNumber txtOrderQtyEdit = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtOrderQtyEdit"));
                Label lblOrderLineIdEdit = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderLineIdEdit"));

                if (txtOrderQtyEdit != null)
                {
                    PhoenixPurchaseOrderLine.UpdateOrderLineFromCompareScreen(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(lblOrderLineIdEdit.Text),
                        General.GetNullableGuid(ViewState["orderid"].ToString()),
                        Filter.CurrentPurchaseVesselSelection,
                        General.GetNullableDecimal(txtOrderQtyEdit.Text));
                }

                _gridView.EditIndex = -1;
                BindData();
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVendor_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        
    }

    protected void gvVendor_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();

            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVendor_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (Filter.CurrentPurchaseStockType == "STORE")
            {
                Label lblComponentHeader = (Label)e.Row.FindControl("lblComponentHeader");
                if (lblComponentHeader != null)
                {
                    lblComponentHeader.Visible = false;
                }
            }
        }

        DataRowView drv = (DataRowView)e.Row.DataItem;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            DataTable header = ds.Tables[0];

            int NewColumnCount = header.Rows.Count * 4;
            if (Filter.CurrentPurchaseStockType == "STORE")
            {
                Label lblComponentNo = (Label)e.Row.FindControl("lblComponentNo");
                if (lblComponentNo != null)
                {
                    lblComponentNo.Visible = false;
                }
            }
            ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
            UserControl txtOrderQty = (UserControl)e.Row.FindControl("txtOrderQty");
            Label lblId = (Label)e.Row.FindControl("lblId");
            if (lblId != null)
            {
                if (string.IsNullOrEmpty(lblId.Text))
                {
                    if (cmdEdit != null)
                        cmdEdit.Visible = false;
                    if (txtOrderQty != null)
                        txtOrderQty.Visible = false;
                }
            }
            if (gvVendor.Columns.Count == (10 + NewColumnCount))
            {
                try
                {
                    DataTable data = ds.Tables[2];

                    if (drv.Row.Table.Columns.Count > 0)
                    {
                        int j = 9;
                        if (j < gvVendor.Columns.Count - 1)
                        {
                            for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                            {
                                if (drv["FLDORDERLINEID"].ToString() == data.Rows[i]["FLDORDERLINEID"].ToString())
                                {
                                    e.Row.Cells[j].Text = data.Rows[i]["FLDUSDPRICE"].ToString();
                                    e.Row.Cells[j].CssClass = "txtNumber";

                                    e.Row.Cells[j + 1].Text = data.Rows[i]["FLDUSDTOTAL"].ToString();
                                    e.Row.Cells[j + 1].CssClass = "txtNumber";

                                    e.Row.Cells[j + 2].Text = data.Rows[i]["FLDDISCOUNTTOTAL"].ToString();
                                    e.Row.Cells[j + 2].CssClass = "txtNumber";

                                    e.Row.Cells[j + 3].Text = data.Rows[i]["FLDDELIVERYTIME"].ToString();
                                    e.Row.Cells[j + 3].CssClass = "txtNumber";
                                    j = j + 4;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
        }
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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
