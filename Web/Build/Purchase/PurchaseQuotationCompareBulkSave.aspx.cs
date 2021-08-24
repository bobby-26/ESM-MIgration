using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using System.IO;

public partial class PurchaseQuotationCompareBulkSave : PhoenixBasePage
{
    ArrayList arrayUser = new ArrayList();
    ArrayList arrayDiscount = new ArrayList();
    ArrayList arrayFooter = new ArrayList();
    ArrayList arrayvendorstatus = new ArrayList(); 
    
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
            SetPageNavigator();
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
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdClose_Click(object sender, EventArgs e)
    {
        spnErrorMessage.InnerHtml = "";
        this.Visible = false;
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


        DataSet ds = PhoenixPurchaseQuotation.QuotationCompare(General.GetNullableGuid(ViewState["orderid"].ToString()), ViewState["selectvendors"].ToString());
        DataSet dsvendorlist = PhoenixPurchaseQuotation.ListQuotatedVendor(General.GetNullableGuid(ViewState["orderid"].ToString()), ViewState["selectvendors"].ToString());
        arrayUser.Clear();
        arrayFooter.Clear();
        arrayDiscount.Clear(); 
        foreach (DataRow dr in dsvendorlist.Tables[0].Rows)
        {
           // arrayUser.Add(dr["FLDNAME"].ToString()+"(" + dr["FLDCURRENCYCODE"].ToString() +")" );
            arrayUser.Add(dr["FLDNAME"].ToString());
            arrayFooter.Add(dr["FLDTOTALAMOUNT"].ToString());
            arrayFooter.Add(dr["FLDTOTALUSDAMOUNT"].ToString());
            arrayDiscount.Add(dr["FLDDISCOUNT"].ToString()); 
            arrayvendorstatus.Add(dr["STATUS"].ToString());  
  

        }
       if (ds.Tables[0].Rows.Count > 0)
       {
           AddCoumnsInGrid(ds.Tables[0], dsvendorlist.Tables[0]);
           gvVender.DataSource = ds;
           gvVender.DataBind();
           AddFooter();
           CheckLowestPrice();
           //CheckDiffQuontity();
       }
       else
       {
           DataTable dt = ds.Tables[0];
           ShowNoRecordsFound(dt, gvVender);
       }
      
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        
    }

    private void CheckLowestPrice()
    {
        if (gvVender.Columns.Count > 9)
        {
            foreach (GridViewRow gvr in gvVender.Rows)
            {
                decimal lowprice = CheckDecimal(gvr.Cells[8].Text);
                int j = 8;
                for (int i = 11; i < gvVender.Columns.Count - 1; i += 3)
                {
                    if (lowprice > CheckDecimal(gvr.Cells[i].Text))
                    {
                        j = i;
                        lowprice = CheckDecimal(gvr.Cells[i].Text);
                    }
                }
                //gvr.Cells[7].Text = lowprice.ToString();
                gvr.Cells[j].BackColor = System.Drawing.Color.Yellow;
            }
            GridViewRow gvfr = gvVender.FooterRow;

            decimal lowtotalprice = CheckDecimal(gvfr.Cells[9].Text);
            int k = 9;
            for (int i = 12; i < gvVender.Columns.Count - 1; i += 3)
            {
                if (lowtotalprice > CheckDecimal(gvfr.Cells[i].Text) )
                {
                    k = i;
                    lowtotalprice = CheckDecimal(gvfr.Cells[i].Text);
                }

            }
            //gvr.Cells[7].Text = lowprice.ToString();
            gvfr.Cells[k].BackColor = System.Drawing.Color.GreenYellow;
            gvfr.Cells[k].ForeColor  = System.Drawing.Color.Red;
            lowtotalprice = CheckDecimal(gvfr.Cells[10].Text);
            bool status=false; 
            for (int l = 0; l < arrayvendorstatus.Count; l++)
            {
                if (arrayvendorstatus[l].ToString() == "FULL")
                    status = true; 
            }
 
            k = 10;
            int m   =   0;
            if (status)
            {
                for (int i = 13; i < gvVender.Columns.Count - 1; i += 3)
                {
                    if (lowtotalprice > CheckDecimal(gvfr.Cells[i].Text) && arrayvendorstatus[m].ToString() == "FULL")
                    {
                        k = i;
                        lowtotalprice = CheckDecimal(gvfr.Cells[i].Text);
                    }
                    m++;
                }
            }
            else
            {
                  for (int i = 13; i < gvVender.Columns.Count - 1; i += 3)
                {
                    if (lowtotalprice > CheckDecimal(gvfr.Cells[i].Text))
                    {
                        k = i;
                        lowtotalprice = CheckDecimal(gvfr.Cells[i].Text);
                    }
                }
            }
            //gvr.Cells[7].Text = lowprice.ToString();
            gvfr.Cells[k].ForeColor = System.Drawing.Color.Red;
            gvfr.Cells[k].BackColor = System.Drawing.Color.GreenYellow;
        }
    }

    private void CheckDiffQuontity()
    {
        if (gvVender.Columns.Count > 8)
        {
            foreach (GridViewRow gvr in gvVender.Rows)
            {
                decimal diffquantity = CheckDecimal(((Label)gvr.FindControl("lblQuantity")).Text);
                for (int i = 8; i < gvVender.Columns.Count; i += 5)
                {
                    if (diffquantity != CheckDecimal(gvr.Cells[i].Text))
                    {
                        gvr.Cells[i].ForeColor = System.Drawing.Color.Red;
                    }
                }                
            }
        }
    }

    private void AddFooter()
    {
        int j = 0;
        GridViewRow gvfr = gvVender.FooterRow;
       
        for (int i = 9; i < gvVender.Columns.Count - 1; i += 3)
        {
            gvfr.Cells[i].Text = arrayFooter[j].ToString();
            gvfr.Cells[i].HorizontalAlign = HorizontalAlign.Right; 
            gvfr.Cells[i+1].Text = arrayFooter[j + 1].ToString();
            gvfr.Cells[i+1].HorizontalAlign = HorizontalAlign.Right; 
            j = j + 2;
        }
    }

    private decimal CheckDecimal(string decimalvalue)
    {
        if (General.GetNullableDecimal(decimalvalue) != null)
          return decimal.Parse(decimalvalue);
        else
            return decimal.Parse("0.00");
    }

    private void AddCoumnsInGrid(DataTable datatable, DataTable vendortable)
    {
        int k = 0;
        if (datatable.Columns.Count > 7 && gvVender.Columns.Count < 10)
        {
            for (int i = 7; i < datatable.Columns.Count; i += 3)
            {
                //TemplateColumn tempcol = new TemplateColumn();

                BoundField priceboundfield = new BoundField();
                priceboundfield.DataField = datatable.Columns[i].ColumnName;
                priceboundfield.HeaderText = "Unit Price";
                //priceboundfield.ItemStyle.CssClass = "input";
                //priceboundfield.ItemStyle.e
                priceboundfield.DataFormatString = "{0:n2}";
                priceboundfield.ReadOnly = true;
                priceboundfield.ApplyFormatInEditMode = true;
                gvVender.Columns.Insert(i + 1, priceboundfield); // (priceboundfield);

                BoundField amountboundfield = new BoundField();
                amountboundfield.DataField = datatable.Columns[i + 1].ColumnName;
                amountboundfield.HeaderText = "Item Price";
                amountboundfield.ItemStyle.CssClass = "txtNumber";
                amountboundfield.DataFormatString = "{0:n2}";
                amountboundfield.ReadOnly = true;
                amountboundfield.ApplyFormatInEditMode = true;
                gvVender.Columns.Insert(i + 2, amountboundfield);

                BoundField amountusdboundfield = new BoundField();
                amountusdboundfield.DataField = datatable.Columns[i + 2].ColumnName;
                amountusdboundfield.HeaderText = "Discount Price [ " + arrayDiscount[k].ToString() + "% ]";
                amountusdboundfield.ItemStyle.CssClass = "txtNumber";
                amountusdboundfield.DataFormatString = "{0:n2}";
                amountusdboundfield.ApplyFormatInEditMode = true;
                amountusdboundfield.ReadOnly = true;
                gvVender.Columns.Insert(i + 3, amountusdboundfield);
                k++;
            }
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvVender_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvVender_RowCommand(object sender, GridViewCommandEventArgs e)
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
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVender_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvVender_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            //_gridView.Columns[7].Visible = false;

            BindData();

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvVender_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
       
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvVender.SelectedIndex = -1;
            gvVender.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPageNavigator()
    {
        //cmdPrevious.Enabled = IsPreviousEnabled();
        //cmdNext.Enabled = IsNextEnabled();
        //lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        //lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        //lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
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
            return true;

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

    protected void gvVender_RowCreated(object sender, GridViewRowEventArgs e)
    {

        if (e.Row.RowType == DataControlRowType.DataRow &&
            (e.Row.RowState == DataControlRowState.Normal ||
            e.Row.RowState == DataControlRowState.Alternate))
        {
            e.Row.TabIndex = 0;
            e.Row.Attributes["onclick"] =
              string.Format("javascript:SelectRow(this, {0}, null);", e.Row.RowIndex);
            e.Row.Attributes["onkeydown"] = "javascript:return SelectSibling(event);";
            e.Row.Attributes["onselectstart"] = "javascript:return false;";
        }
    
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (gvVender.Columns.Count > 8)
            {
                int j = 0;
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);
                for (int i = 0; i <= gvVender.Columns.Count - 1; i += 3)
                {
                    TableCell HeaderCell;
                    if (i < 6)
                    {
                        HeaderCell = new TableCell();
                        HeaderCell.Text = "";
                        HeaderCell.ColumnSpan = 7;
                        HeaderGridRow.Cells.Add(HeaderCell);
                        i = i + 3;
                    }
                    if (i >= 7)
                    {
                        HeaderCell = new TableCell();
                        if (j < arrayUser.Count)
                            HeaderCell.Text = arrayUser[j].ToString();
                        else
                            HeaderCell.Text = "";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        HeaderCell.ColumnSpan = 3;
                        HeaderGridRow.Cells.Add(HeaderCell);
                        j++;
                    }
                }
                TableCell HeaderCell1;
                HeaderCell1 = new TableCell();
                HeaderCell1.Text = "";
                HeaderCell1.ColumnSpan = 1;
                HeaderGridRow.Cells.Add(HeaderCell1);

                gvVender.Controls[0].Controls.AddAt(0, HeaderGridRow);
            }
        }
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
