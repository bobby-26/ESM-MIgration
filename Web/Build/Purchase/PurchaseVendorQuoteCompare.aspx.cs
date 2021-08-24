using System;
using System.Data;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Registers;

public partial class PurchaseVendorQuoteCompare : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            DataSet ds = PhoenixRegistersAddress.ListSpecificAddress(",SUP-000003,SUP-002791,");

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlVendor.DataSource = ds;
                ddlVendor.DataTextField = "FLDNAME";
                ddlVendor.DataValueField = "FLDADDRESSCODE";
                ddlVendor.DataBind();
                ddlVendor.SelectedValue = "211";
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Purchase/PurchaseVendorQuoteCompare.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvPOList')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Purchase/PurchaseVendorQuoteCompare.aspx", "Find", "search.png", "FIND");

            MenuPOList.MenuList = toolbar.Show();
            MenuPOList.SetTrigger(pnlDashboard);

            toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Purchase/PurchaseVendorQuoteCompare.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvLineItem')", "Print Grid", "icon_print.png", "PRINT");

            MenuLineItemList.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Quote Compare", "QUOTECOMPARE");

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbar.Show();
            MenuOrderFormMain.SelectedMenuIndex = 0;

            ViewState["SELECTEDORDERID"] = "";
            ViewState["SELECTEDVESSELID"] = "";
            ViewState["SELECTEDQUOTATIONID"] = "";

            ViewState["PAGENUMBER"] = 1;
            ViewState["PAGENUMBER1"] = 1;
        }

        BindPOListData();
        SetRowSelection();
        SetPageNavigator1();

        BindData();
        SetPageNavigator();
    }

    protected void MenuPOList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowPOListExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuLineItemList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowLineItemExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHEET"))
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowPOListExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDFORMNO", "FLDTITLE" };
        string[] alCaptions = { "Vessel", "Number", "Form Title" };
        
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION1"] == null) ? null : (ViewState["SORTEXPRESSION1"].ToString());

        if (ViewState["SORTDIRECTION1"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION1"].ToString());

        if (ViewState["ROWCOUNT1"] == null || Int32.Parse(ViewState["ROWCOUNT1"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT1"].ToString());

        ds = PhoenixPurchaseOrderForm.PurchaseQuotationCompareVendorPOList(
            General.GetNullableInteger(ucVessel.SelectedVessel),
            int.Parse(ddlVendor.SelectedValue),
            General.GetNullableString(ddlStockType.SelectedValue),
            General.GetNullableDateTime(txtDateFrom.Text),
            General.GetNullableDateTime(txtDateTo.Text),
            General.GetNullableString(txtPartNumber.Text),
            General.GetNullableString(txtPartName.Text),
            General.GetNullableString(txtMakerRef.Text),
            null,    // sort exp
            null,   // sort dir
            Int32.Parse(ViewState["PAGENUMBER1"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=FormList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Form List </center></h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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

    protected void ShowLineItemExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDMAKER", "FLDVENDORNAME", "FLDRECEIVEDDATE", "FLDQUANTITY", "FLDCURRENCYCODE", "FLDQUOTEDPRICE", "FLDDISCOUNT" };
        string[] alCaptions = { "Number", "Name", "Maker", "Vendor", "Received Date", "Qty", "Currency", "Price", "Discount (%)" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseOrderForm.PurchaseQuotationCompareLineItemList(
            General.GetNullableInteger(ViewState["SELECTEDVESSELID"].ToString()),
            General.GetNullableInteger(ddlVendor.SelectedValue),
            General.GetNullableGuid(ViewState["SELECTEDORDERID"].ToString()),
            General.GetNullableGuid(ViewState["SELECTEDQUOTATIONID"].ToString()),
            null, null,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=LineItemList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Line Item List </center></h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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

    private void BindPOListData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        if (ViewState["ROWCOUNT1"] == null || Int32.Parse(ViewState["ROWCOUNT1"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT1"].ToString());

        string[] alColumns = { "FLDVESSELNAME", "FLDFORMNO", "FLDTITLE" };
        string[] alCaptions = { "Vessel", "Number", "Form Title" };

        DataSet ds = PhoenixPurchaseOrderForm.PurchaseQuotationCompareVendorPOList(
            General.GetNullableInteger(ucVessel.SelectedVessel),
            int.Parse(ddlVendor.SelectedValue),
            General.GetNullableString(ddlStockType.SelectedValue),
            General.GetNullableDateTime(txtDateFrom.Text),
            General.GetNullableDateTime(txtDateTo.Text),
            General.GetNullableString(txtPartNumber.Text),
            General.GetNullableString(txtPartName.Text),
            General.GetNullableString(txtMakerRef.Text),
            null,    // sort exp
            null,   // sort dir
            Int32.Parse(ViewState["PAGENUMBER1"].ToString()),
            10,
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPOList.DataSource = ds;
            gvPOList.DataBind();
        }
        else
        {
            ShowNoRecordsFound(ds.Tables[0], gvPOList);
        }

        General.SetPrintOptions("gvPOList", "Order Form List", alCaptions, alColumns, ds);

        ViewState["ROWCOUNT1"] = iRowCount;
        ViewState["TOTALPAGECOUNT1"] = iTotalPageCount;
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDMAKER", "FLDVENDORNAME", "FLDRECEIVEDDATE", "FLDQUANTITY", "FLDCURRENCYCODE", "FLDQUOTEDPRICE", "FLDDISCOUNT" };
        string[] alCaptions = { "Number", "Name", "Maker", "Vendor", "Received Date", "Qty", "Currency", "Price", "Discount (%)" };

        DataSet ds = PhoenixPurchaseOrderForm.PurchaseQuotationCompareLineItemList(
            General.GetNullableInteger(ViewState["SELECTEDVESSELID"].ToString()),
            General.GetNullableInteger(ddlVendor.SelectedValue),
            General.GetNullableGuid(ViewState["SELECTEDORDERID"].ToString()),
            General.GetNullableGuid(ViewState["SELECTEDQUOTATIONID"].ToString()),   
            null, null,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvLineItem.DataSource = ds;
            gvLineItem.DataBind();
        }
        else
        {
            ShowNoRecordsFound(ds.Tables[0], gvLineItem);
        }

        General.SetPrintOptions("gvLineItem", "Line Item List", alCaptions, alColumns, ds);

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvLineItem_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    if (ViewState["SORTEXPRESSION"] != null)
        //    {
        //        HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
        //        if (img != null)
        //        {
        //            if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
        //                img.Src = Session["images"] + "/arrowUp.png";
        //            else
        //                img.Src = Session["images"] + "/arrowDown.png";

        //            img.Visible = true;
        //        }
        //    }
        //}
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
               && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;

                if (drv["FLDISCOMPARINGVENDOR"].ToString().Equals("1"))
                {
                    e.Row.CssClass = "datagrid_selectedstyle";
                }
            }
        }
    }

    protected void gvLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        int currentRow = int.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("REPLY"))
        {
        }
    }

    private void SetRowSelection()
    {
        gvPOList.SelectedIndex = -1;

        if (ViewState["SELECTEDORDERID"] != null && !ViewState["SELECTEDORDERID"].ToString().Equals(""))
        {
            for (int i = 0; i < gvPOList.Rows.Count; i++)
            {
                if (gvPOList.DataKeys[i].Value.ToString().Equals(ViewState["SELECTEDORDERID"].ToString()))
                {
                    gvPOList.SelectedIndex = i;
                }
            }
        }

        if (gvPOList.SelectedIndex == -1 && gvPOList.Rows.Count > 0)
        {
            gvPOList.SelectedIndex = 0;
            ViewState["SELECTEDORDERID"] = gvPOList.DataKeys[0].Value.ToString().Equals("") ? "" : gvPOList.DataKeys[0].Value.ToString();

            if (!ViewState["SELECTEDORDERID"].ToString().Equals(""))
            {
                Label lblVesselId = (Label)gvPOList.Rows[0].FindControl("lblVesselId");
                Label lblQuotationId = (Label)gvPOList.Rows[0].FindControl("lblQuotationId");
                ViewState["SELECTEDVESSELID"] = lblVesselId.Text;
                ViewState["SELECTEDQUOTATIONID"] = lblQuotationId.Text;
                Filter.CurrentPurchaseStockType = ddlStockType.SelectedValue;
            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindPOListData();
    }

    protected void gvPOList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        int currentRow = int.Parse(e.CommandArgument.ToString());

        if (e.CommandName.ToUpper().Equals("SELECTFORMNO"))
        {
            Label lblOrderId = (Label)_gridview.Rows[currentRow].FindControl("lblOrderId");
            Label lblVesselId = (Label)_gridview.Rows[currentRow].FindControl("lblVesselId");
            Label lblQuotationId = (Label)_gridview.Rows[currentRow].FindControl("lblQuotationId");

            ViewState["SELECTEDORDERID"] = lblOrderId.Text;
            ViewState["SELECTEDVESSELID"] = lblVesselId.Text;
            ViewState["SELECTEDQUOTATIONID"] = lblQuotationId.Text;

            SetRowSelection();
            Filter.CurrentPurchaseStockType = ddlStockType.SelectedValue;

            BindData();
            SetPageNavigator();
        }
    }

    protected void gvPOList_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }

    protected void gvPOList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
        //       && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
        //    {
        //        DataRowView drv = (DataRowView)e.Row.DataItem;                
        //    }
        //}
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvLineItem.SelectedIndex = -1;
            gvLineItem.EditIndex = -1;
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

    protected void PagerButtonClick1(object sender, CommandEventArgs ce)
    {
        try
        {
            gvPOList.SelectedIndex = -1;
            gvPOList.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER1"] = (int)ViewState["PAGENUMBER1"] - 1;
            else
                ViewState["PAGENUMBER1"] = (int)ViewState["PAGENUMBER1"] + 1;

            BindPOListData();
            SetPageNavigator1();
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

    private void SetPageNavigator1()
    {
        cmdPrevious1.Enabled = IsPreviousEnabled1();
        cmdNext1.Enabled = IsNextEnabled1();
        lblPagenumber1.Text = "Page " + ViewState["PAGENUMBER1"].ToString();
        lblPages1.Text = " of " + ViewState["TOTALPAGECOUNT1"].ToString() + " Pages. ";
        lblRecords1.Text = "(" + ViewState["ROWCOUNT1"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled1()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER1"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT1"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }

    private Boolean IsNextEnabled1()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER1"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT1"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
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

    protected void cmdGo_Click1(object sender, EventArgs e)
    {
        int result;
        try
        {
            if (Int32.TryParse(txtnopage1.Text, out result))
            {
                ViewState["PAGENUMBER1"] = Int32.Parse(txtnopage1.Text);

                if ((int)ViewState["TOTALPAGECOUNT1"] < Int32.Parse(txtnopage1.Text))
                    ViewState["PAGENUMBER1"] = ViewState["TOTALPAGECOUNT1"];

                if (0 >= Int32.Parse(txtnopage1.Text))
                    ViewState["PAGENUMBER1"] = 1;

                if ((int)ViewState["PAGENUMBER1"] == 0)
                    ViewState["PAGENUMBER1"] = 1;

                txtnopage1.Text = ViewState["PAGENUMBER1"].ToString();
            }
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
