using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;

public partial class PurchaseSchedulePOLineItemList : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvSchedulePOLineItem.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (!IsPostBack)
        {
            Session["NEWSPOLINEITEM"] = "N";
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            toolbar.AddImageButton("../Purchase/PurchaseSchedulePOLineItemList.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvSchedulePOLineItem')", "Print Grid", "icon_print.png", "PRINT");

            MenuSchedulePO.AccessRights = this.ViewState;
            MenuSchedulePO.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Schedule PO", "SCHEDULEPO");
            toolbar.AddButton("Line Items", "LINEITEM");
            //toolbar.AddButton("Vessels", "VESSEL");
            toolbar.AddButton("History", "HISTORY");

            MenuMainSchedulePO.AccessRights = this.ViewState;
            MenuMainSchedulePO.MenuList = toolbar.Show();
            MenuMainSchedulePO.SelectedMenuIndex = 1;

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            if (Request.QueryString["SCHEDULEPOID"] != null && Request.QueryString["SCHEDULEPOID"].ToString() != "")
                ViewState["SCHEDULEPOID"] = Request.QueryString["SCHEDULEPOID"].ToString();
            else
                ViewState["SCHEDULEPOID"] = "";
        }
        BindData();
        SetPageNavigator();
    }

    protected void MenuMainSchedulePO_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SCHEDULEPO"))
        {
            Response.Redirect("../Purchase/PurchaseSchedulePOList.aspx?SCHEDULEPOID=" + ViewState["SCHEDULEPOID"], false);
        }
        if (dce.CommandName.ToUpper().Equals("LINEITEM"))
        {
            Response.Redirect("../Purchase/PurchaseSchedulePOLineItemList.aspx", false);
        }
        if (dce.CommandName.ToUpper().Equals("VESSEL"))
        {
            Response.Redirect("../Purchase/PurchaseSchedulePOLineItemByVesselList.aspx");
        }
        if (dce.CommandName.ToUpper().Equals("HISTORY"))
        {
            Response.Redirect("../Purchase/PurchaseSchedulePOHistory.aspx?SCHEDULEPOID=" + ViewState["SCHEDULEPOID"]);
        }
    }

    protected void MenuSchedulePO_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDLINEITEMNAME", "FLDBUDGETCODE", "FLDUNITNAME", "FLDPRICE", "FLDREQUESTEDQUANTITY" };
        string[] alCaptions = { "Item Name", "Budget Code", "Unit", "Unit Price", "Total Order Quantity" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixPurchaseSchedulePO.SchedulePOLineItemSearch(
                                                                 General.GetNullableGuid(ViewState["SCHEDULEPOID"].ToString())
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvSchedulePOLineItem", "Schedule PO Line Item list", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvSchedulePOLineItem.DataSource = ds;
            gvSchedulePOLineItem.DataBind();

            if (ViewState["LINEITEMID"] == null)
            {
                ViewState["LINEITEMID"] = ds.Tables[0].Rows[0]["FLDLINEITEMID"].ToString();
                gvSchedulePOLineItem.SelectedIndex = 0;
            }
            SetRowSelection();
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseSchedulePOLineItemGeneral.aspx?LINEITEMID=" + ViewState["LINEITEMID"];
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvSchedulePOLineItem);
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseSchedulePOLineItemGeneral.aspx?LINEITEMID=";
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDLINEITEMNAME", "FLDBUDGETCODE", "FLDUNITNAME", "FLDPRICE", "FLDREQUESTEDQUANTITY" };
        string[] alCaptions = { "Item Name", "Budget Code", "Unit", "Unit Price", "Total Order Quantity" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseSchedulePO.SchedulePOLineItemSearch(
                                                         General.GetNullableGuid(ViewState["SCHEDULEPOID"].ToString())
                                                        , sortexpression
                                                        , sortdirection
                                                        , (int)ViewState["PAGENUMBER"]
                                                        , iRowCount
                                                        , ref iRowCount
                                                        , ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=SchedulePOLineItemList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Schedule PO Line Item list</h3></td>");
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

    private void SetRowSelection()
    {
        gvSchedulePOLineItem.SelectedIndex = -1;

        for (int i = 0; i < gvSchedulePOLineItem.Rows.Count; i++)
        {
            if (gvSchedulePOLineItem.DataKeys[i].Value.ToString().Equals(ViewState["LINEITEMID"].ToString()))
            {
                gvSchedulePOLineItem.SelectedIndex = i;
            }
        }
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;

        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

    }

    protected void gvSchedulePOLineItem_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvSchedulePOLineItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvSchedulePOLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                BindPageURL(nCurrentRow);
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                BindPageURL(nCurrentRow);
                if (General.GetNullableGuid(ViewState["LINEITEMID"].ToString()) != null)
                {                    
                    PhoenixPurchaseSchedulePO.SchedulePOLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["LINEITEMID"].ToString()));
                    ViewState["LINEITEMID"] = null;
                    Filter.CurrentSelectedSchedulePOLineItemId = null;
                }
            }

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSchedulePOLineItem_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvSchedulePOLineItem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvSchedulePOLineItem_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
        }
    }

    protected void gvSchedulePOLineItem_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        gvSchedulePOLineItem.SelectedIndex = se.NewSelectedIndex;
        BindPageURL(se.NewSelectedIndex);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["LINEITEMID"] = ((Label)gvSchedulePOLineItem.Rows[rowindex].FindControl("lblLineItemId")).Text;
            ifMoreInfo.Attributes["src"] = "../Purchase/PurchaseSchedulePOLineItemGeneral.aspx?LINEITEMID=" + ViewState["LINEITEMID"].ToString();
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
            BindData();
            SetPageNavigator();
            if (Session["NEWBULKLINEITEM"] != null && Session["NEWBULKLINEITEM"].ToString() == "Y")
            {
                gvSchedulePOLineItem.SelectedIndex = 0;
                Session["NEWBULKLINEITEM"] = "N";
                BindPageURL(gvSchedulePOLineItem.SelectedIndex);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
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
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvSchedulePOLineItem.SelectedIndex = -1;
        gvSchedulePOLineItem.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
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

    private void DeleteBulkPOLineItem(Guid lineitemid)
    {
        PhoenixPurchaseBulkPurchase.BulkPOLineItemDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, lineitemid);
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
