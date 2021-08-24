using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;

public partial class CrewWorkingGearOrderFormStoreItemSelection : PhoenixBasePage
{
    string empid = string.Empty;
    string vslid = string.Empty;
    string orderid = string.Empty;
    string crewplanid = string.Empty;
    string zoneid = string.Empty;

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
        empid = Request.QueryString["empid"];
        vslid = Request.QueryString["vesslid"];
        crewplanid = Request.QueryString["crewplanid"];
        orderid = Request.QueryString["orderid"];
        zoneid = Request.QueryString["Zoneid"]; 

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Search", "SEARCH");
        if (Request.QueryString["back"] != null)
            toolbarmain.AddButton("Back", "BACK");
        MenuStockItem.MenuList = toolbarmain.Show();
        MenuStockItem.SetTrigger(pnlStockItem);

        if (!IsPostBack)
        {
            if ((Request.QueryString["txtname"] != null) && (Request.QueryString["txtname"] != ""))
                txtItemSearch.Text = Request.QueryString["txtname"].ToString();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }
        BindData();
    }

    protected void MenuStockItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                if (empid != null)
                {
                    Response.Redirect("CrewWorkingGearIndividualRequest.aspx?empid=" + empid + "&vesslid=" + vslid + "&crewplanid=" + crewplanid + "&orderid=" + Request.QueryString["orderid"] + "", false);
                }
                //else
                //{
                //    Response.Redirect("CrewWorkingGearVesselRequest.aspx?&vesslid=" + vslid + "&orderid=" + orderid + "", false);
                //}
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

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixCrewWorkingGearOrderForm.SearchOrderFormWorkGearItem(new Guid(Request.QueryString["ORDERID"]),
                txtItemSearch.Text
                , General.GetNullableInteger(ucWorkingGearType.SelectedGearType)
                ,General.GetNullableInteger(zoneid)
                ,sortexpression
                , sortdirection
               ,Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
               , General.ShowRecords(null),
               ref iRowCount,
               ref iTotalPageCount);

            if (dt.Rows.Count > 0)
            {
                gvWorkGearItem.DataSource = dt;
                gvWorkGearItem.DataBind();
            }
            else
            {
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

    protected void gvWorkGearItem_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            UserControlMaskNumber qty = (UserControlMaskNumber)e.Row.FindControl("txtQuantity");
            if (qty == null)
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvWorkGearItem, "Edit$" + e.Row.RowIndex.ToString(), false);
            SetKeyDownScroll(sender, e);
        }
    }

    protected void gvWorkGearItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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

    protected void gvWorkGearItem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {

            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);

            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;

            BindData();
            ((TextBox)((UserControlMaskNumber)_gridView.Rows[de.NewEditIndex].FindControl("txtQuantity")).FindControl("txtNumber")).Focus();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkGearItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;
            string quantity = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtQuantity")).Text;
            string unitprice = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtUnitPrice")).Text;
            int size = int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblSizeid")).Text);
            if (General.GetNullableDecimal(quantity).HasValue)
            {
                PhoenixCrewWorkingGearOrderForm.InsertOrderFormLineItem(new Guid(Request.QueryString["ORDERID"])
                                                                        , _gridView.DataKeys[nCurrentRow].Value.ToString()
                                                                        , quantity
                                                                        , unitprice
                                                                        , size);
                string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1','ifMoreInfo','keepopen');";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
            }
            _gridView.EditIndex = -1;
            BindData();
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

    protected void gvWorkGearItem_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

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
            int nRows = ((DataTable)_gridView.DataSource).Rows.Count - 1;

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
