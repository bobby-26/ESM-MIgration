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

public partial class PurchaseDeliveryItems : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvDeliveryItem.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvDeliveryItem.UniqueID, "Edit$" + r.RowIndex.ToString());
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Purchase/PurchaseDeliveryItems.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvDeliveryItem')", "Print Grid", "icon_print.png", "");

            MenuDeliveryItemList.MenuList = toolbargrid.Show();
            //MenuDeliveryItemList.SetTrigger(pnlLineItemEntry);

            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                
                toolbarmain.AddButton("Line Items", "LINEITEM");
                toolbarmain.AddButton("Delivery", "DELIVERY");

                MenuDeliveryLineItem.MenuList = toolbarmain.Show();
                MenuDeliveryLineItem.SelectedMenuIndex = 0;

                //ucTitle.Text = "Delivery Lineitem [" + PhoenixPurchaseOrderForm.FormNumber + "   Vendor :   " + PhoenixPurchaseQuotation.VendorName + "     ]";
               
                if (Request.QueryString["deliveryid"] != null)
                {
                    ViewState["deliveryid"] = Request.QueryString["deliveryid"].ToString();
                }
                else
                {
                    ViewState["deliveryid"] = "";
                }
                if (Request.QueryString["orderid"] != null)
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                bindDeliveryLine();
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

    private void bindDeliveryLine()
    {
        DataSet ds = new DataSet();
        ds = PhoenixPurchaseDelivery.Editdelivery(new Guid(ViewState["deliveryid"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ucTitle.Text = " Delivery Items [  " + PhoenixPurchaseOrderForm.FormNumber + "   ,   Delivery Number:   " + ds.Tables[0].Rows[0]["FLDDELIVERYNUMBER"].ToString() + "  ]";
           // ucTitle.Text = "Delivery Number: " + ds.Tables[0].Rows[0]["FLDDELIVERYNUMBER"].ToString();
        }
    }

    protected void MenuDeliveryItemList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        
        string[] alColumns = { "FLDPARTNUMBER", "FLDPARTNAME", "FLDQUANTITY", "FLDORDEREDQUANTITY", "FLDLOCATIONNAME" };
        string[] alCaptions = { "Part Number", "Part Name", "Quantity", "Purchased", "Located" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseDeliveryLine.DeliveryLineSearch(
            new Guid(Request.QueryString["orderid"].ToString()),
            new Guid(ViewState["deliveryid"].ToString())
            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=DeliveryLineItem.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Delivery Line Items</h3></td>");
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

    protected void MenuDeliveryLineItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("DELIVERY"))
            {
                if (ViewState["orderid"] != null)
                    Response.Redirect("../Purchase/PurchaseDeliveryDetail.aspx?orderid=" + ViewState["orderid"].ToString());
                else
                    Response.Redirect("../Purchase/PurchaseDeliveryDetail.aspx");
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = {"FLDPARTNUMBER", "FLDPARTNAME","FLDQUANTITY","FLDORDEREDQUANTITY","FLDLOCATIONNAME" };
        string[] alCaptions = {"Part Number", "Part Name", "Quantity","Purchased","Located" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseDeliveryLine.DeliveryLineSearch(
            new Guid(Request.QueryString["orderid"].ToString()),
            new Guid(ViewState["deliveryid"].ToString())
            , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDeliveryItem.DataSource = ds;
            gvDeliveryItem.DataBind();

            if (ViewState["CURRENTROW"] == null)
            {
                gvDeliveryItem.SelectedIndex = 0;
                ViewState["CURRENTROW"] = 0;
                bindDeliveryLine(ds.Tables[0].Rows[0]["FLDDELIVERYLINEID"].ToString());
            }
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvDeliveryItem);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        General.SetPrintOptions("gvDeliveryItem", "Delivery Line item", alCaptions, alColumns, ds);
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {
            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvDeliveryItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        gvDeliveryItem.SelectedIndex = e.NewSelectedIndex;
        ViewState["CURRENTROW"] = e.NewSelectedIndex;

        ViewState["deliverylineid"] = ((Label)gvDeliveryItem.Rows[e.NewSelectedIndex].FindControl("lblDeliveryLineId")).Text;

        bindDeliveryLine(((Label)gvDeliveryItem.Rows[e.NewSelectedIndex].FindControl("lblDeliveryLineId")).Text);        

        if (gvDeliveryItem.EditIndex > -1)
            gvDeliveryItem.UpdateRow(gvDeliveryItem.EditIndex, false);

        gvDeliveryItem.EditIndex = -1;
        BindData();
    }

    protected void gvDeliveryItem_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvDeliveryItem.SelectedIndex = -1;
        ViewState["CURRENTROW"] = -1;
        gvDeliveryItem.EditIndex = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvDeliveryItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeliveryItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixPurchaseDeliveryLine.DeleteDeliveryLine(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblDeliveryLineId")).Text));
                _gridView.EditIndex = -1;
                bindDeliveryLine(((Label)_gridView.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblDeliveryLineId")).Text);
                BindData();
            }
            else if (e.CommandName.ToUpper().Equals("CANCEL"))
            {
                _gridView.EditIndex = -1;
                BindData();
            }
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeliveryItem_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvDeliveryItem, "Edit$" + e.Row.RowIndex.ToString(), false);
        }

        SetKeyDownScroll(sender, e);
    }

    protected void gvDeliveryItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;

        if (!IsValidQuantity(((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtQuantityEdit")).Text, ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPurchasedEdit")).Text))
        {
            ucError.Visible = true;
            return;
        }
        if (((Label)_gridView.Rows[nCurrentRow].FindControl("lblDeliveryLineIdEdit")).Text != "")
        {
            UpdateDeliveryLineItem(
                ((Label)(_gridView.Rows[nCurrentRow].FindControl("lblDeliveryLineIdEdit"))).Text,
                ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderLineIdEdit")).Text,
                ((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtQuantityEdit")).Text,
                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtLocated")).Text);
        }
        else
        {
            InsertDeliveryLineItem(
                ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderLineIdEdit")).Text,
                ((UserControlDecimal)_gridView.Rows[nCurrentRow].FindControl("txtQuantityEdit")).Text,
                ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtLocated")).Text);
        }
        
        _gridView.EditIndex = -1;
        BindData();
        bindDeliveryLine(((Label)_gridView.Rows[nCurrentRow].FindControl("lblDeliveryLineId")).Text);
        SetPageNavigator();
    }

    protected void gvDeliveryItem_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        try
        {
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void InsertDeliveryLineItem(string orderlineid, string quantity, string location)
    {
        try
        {
            PhoenixPurchaseDeliveryLine.InsertDeliveryLine(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(ViewState["deliveryid"].ToString())
                , new Guid(orderlineid), General.GetNullableInteger(quantity.Replace(",", "")), null, location);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateDeliveryLineItem(string deliverylineid, string orderlineid, string quantity, string location)
    {
        try
        {
            PhoenixPurchaseDeliveryLine.UpdateDeliveryLine(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , new Guid(deliverylineid), new Guid(ViewState["deliveryid"].ToString())
                , new Guid(orderlineid), General.GetNullableInteger(quantity.Replace(",","")), null, location);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidQuantity(string quantity, string purchased)
    {
        if (quantity.Trim().Equals("") || quantity == "0")
            ucError.ErrorMessage = "Quantity  is required.";

        if (quantity.Trim() != "" && purchased.Trim() != "")
            if (decimal.Parse(quantity) > decimal.Parse(purchased))
                ucError.ErrorMessage = "Delivered quantity should be less than or equal to Purchase quantity.";

        if (General.GetNullableGuid(ViewState["deliveryid"].ToString()) == null)
            ucError.ErrorMessage = "DeliveryId is required.";

        return (!ucError.IsError);
    }

    protected void gvDeliveryItem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            ViewState["CURRENTROW"] = de.NewEditIndex;
                        
            BindData();

            bindDeliveryLine(((Label)_gridView.Rows[de.NewEditIndex].FindControl("lblDeliveryLineId")).Text);            

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeliveryItem_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }

            Label lblComponentName = (Label)e.Row.FindControl("lblComponentName");

            if (Filter.CurrentPurchaseStockType.Equals("STORE"))
            {
                lblComponentName.Visible = false;
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvDeliveryItem.SelectedIndex = -1;
        ViewState["CURRENTROW"] = -1;
        gvDeliveryItem.EditIndex = -1;

        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        try
        {
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
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvDeliveryItem.SelectedIndex = -1;
            ViewState["CURRENTROW"] = -1;
            gvDeliveryItem.EditIndex = -1;
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    protected void onPurchaseDeliveryLine(object sender, CommandEventArgs e)
    {
        try
        {
            gvDeliveryItem.SelectedIndex = Int32.Parse(e.CommandArgument.ToString());
            ViewState["CURRENTROW"] = gvDeliveryItem.SelectedIndex;

            bindDeliveryLine(((Label)gvDeliveryItem.Rows[Int32.Parse(e.CommandArgument.ToString())].FindControl("lblDeliveryLineId")).Text);
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    private void bindDeliveryLine(string deliverylineid)
    {
        if (deliverylineid  != "")
        {
            DataSet ds = PhoenixPurchaseDeliveryLine.EditDeliveryLine(new Guid(deliverylineid));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtPartName.Text = dr["FLDPARTNAME"].ToString();
                txtPartNumber.Text = dr["FLDPARTNUMBER"].ToString();
                txtMakerReference.Text = dr["FLDMAKERREFERENCE"].ToString();
                txtPurchased.Text = String.Format("{0:##,###,##0}", dr["FLDORDEREDQUANTITY"]);
                txtQuantity.Text = String.Format("{0:##,###,##0}", dr["FLDQUANTITY"]);
                txtLocation.Text = dr["FLDLOCATIONNAME"].ToString();
                ucUnit.SelectedUnit = dr["FLDUNITID"].ToString();
            }
            else
            {
                txtPartName.Text = ((Label)gvDeliveryItem.Rows[Int32.Parse(ViewState["CURRENTROW"].ToString())].FindControl("lblPartName")).Text;
                txtPartNumber.Text = ((LinkButton)gvDeliveryItem.Rows[Int32.Parse(ViewState["CURRENTROW"].ToString())].FindControl("lnkPartNumber")).Text; 
                txtPurchased.Text = ((Label)gvDeliveryItem.Rows[Int32.Parse(ViewState["CURRENTROW"].ToString())].FindControl("lblPurchased")).Text; 
                txtMakerReference.Text = ((Label)gvDeliveryItem.Rows[Int32.Parse(ViewState["CURRENTROW"].ToString())].FindControl("lblMakerRef")).Text;
                ucUnit.SelectedUnit = ((Label)gvDeliveryItem.Rows[Int32.Parse(ViewState["CURRENTROW"].ToString())].FindControl("lblUnitId")).Text; 
                txtQuantity.Text = "";
                txtLocation.Text = "";
            }
        }
        else
        {
            txtPartName.Text = ((Label)gvDeliveryItem.Rows[Int32.Parse(ViewState["CURRENTROW"].ToString())].FindControl("lblPartName")).Text;
            txtPartNumber.Text = ((LinkButton)gvDeliveryItem.Rows[Int32.Parse(ViewState["CURRENTROW"].ToString())].FindControl("lnkPartNumber")).Text; 
            txtPurchased.Text = ((Label)gvDeliveryItem.Rows[Int32.Parse(ViewState["CURRENTROW"].ToString())].FindControl("lblPurchased")).Text; 
            txtMakerReference.Text = ((Label)gvDeliveryItem.Rows[Int32.Parse(ViewState["CURRENTROW"].ToString())].FindControl("lblMakerRef")).Text;
            ucUnit.SelectedUnit = ((Label)gvDeliveryItem.Rows[Int32.Parse(ViewState["CURRENTROW"].ToString())].FindControl("lblUnitId")).Text; 
            txtQuantity.Text = "";
            txtLocation.Text = "";
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
