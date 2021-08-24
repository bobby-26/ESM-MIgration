using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web;

public partial class RegistersWorkingGearItemLocation : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvRegistersworkgearitemlocation.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation(gvRegistersworkgearitemlocation.UniqueID, "Edit$" + r.RowIndex.ToString());
            }

        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        try
        {
            toolbar.AddImageButton("../Registers/RegistersWorkingGearItemLocation.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvRegistersworkgearitemlocation')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Registers/RegistersWorkingGearItemLocation.aspx", "<b>Find</b>", "search.png", "FIND");
            MenuRegistersWorkingGearItem.AccessRights = this.ViewState;
            MenuRegistersWorkingGearItem.MenuList = toolbar.Show();
            MenuRegistersWorkingGearItem.SetTrigger(pnlWorkingGearItem);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["ITEMID"] = null;

                ViewState["ITEMID"] = Request.QueryString["itemid"];
            }
            BindData();
            SetPageNavigator();
            ucDate.Text = DateTime.Now.ToString();
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
        string[] alColumns = { "FLDSIZENAME", "FLDSTOCKQUANTITY", "FLDSTOCKVALUE" };
        string[] alCaptions = { "Size", "Stock in Hand(Qty)", "Stock Value (INR)" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixRegistersWorkingGearItem.WorkingGearItemLocationList(General.GetNullableGuid(ViewState["ITEMID"].ToString()),General.GetNullableInteger(ucZone.SelectedZone), Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                        iRowCount, ref iRowCount, ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=WorkingGeraItemLocation.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' nowrap><h4><center>Working Gear Item Location</center></h4></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' nowrap>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");

        DataRow drHeader = ds.Tables[0].Rows[0];

        HttpContext.Current.Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'><tr>");
        HttpContext.Current.Response.Write("<td colspan=" + 3 + " align='left' nowrap ><b>Item Name : </b>" + drHeader["FLDWORKINGGEARITEMNAME"].ToString() + "</td>");
        HttpContext.Current.Response.Write("<tr>");

        HttpContext.Current.Response.Write("</TABLE>");

        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td nowrap>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td nowrap>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void RegistersWorkingGearItem_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSIZENAME", "FLDSTOCKQUANTITY", "FLDSTOCKVALUE" };
        string[] alCaptions = {"Size", "Stock in Hand(Qty)", "Stock Value (INR)" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersWorkingGearItem.WorkingGearItemLocationList(General.GetNullableGuid(ViewState["ITEMID"].ToString()),General.GetNullableInteger(ucZone.SelectedZone), Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                        General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {

            gvRegistersworkgearitemlocation.DataSource = ds;
            gvRegistersworkgearitemlocation.DataBind();
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvRegistersworkgearitemlocation);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        StringBuilder title = new StringBuilder();
        title.Append("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        title.Append("<tr><td colspan='" + (alColumns.Length).ToString() + "' nowrap><h4><center>Working Gear Set Item Details</center></h4></td></tr>");
        title.Append("<tr><td colspan='" + (alColumns.Length).ToString() + "' nowrap>&nbsp;</td>");
        title.Append("</tr>");
        title.Append("</TABLE>");
        DataRow drHeader = ds.Tables[0].Rows[0];
        title.Append("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'><tr>");
        title.Append("<td colspan=" + 3 + " align='left' nowrap ><b>Item Name : </b>" + drHeader["FLDWORKINGGEARITEMNAME"].ToString() + "</td>");
        title.Append("<tr>");
        title.Append("</TABLE>");

        General.SetPrintOptions("gvRegistersworkgearitemlocation", title.ToString(), alCaptions, alColumns, ds);

        FillItemDetails();
    }

    private void FillItemDetails()
    {
        DataSet ds = PhoenixRegistersWorkingGearItem.EditWorkingGearItem(General.GetNullableGuid(ViewState["ITEMID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            //DataRow dr = ds.Tables[0].Rows[0];
            //txtItemName.Text = dr["FLDWORKINGGEARITEMNAME"].ToString();
            ////txtPrice.Text = dr["FLDUNITPRICE"].ToString();
            //txtStockinHand.Text = dr["FLDSTOCKQUANTITY"].ToString();
            //txtStockValue.Text = dr["FLDSTOCKVALUE"].ToString();
        }
    }

    protected void gvRegistersworkgearitemlocation_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvRegistersworkgearitemlocation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {

            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string A = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblWorkGearitemLocIdEdit")).Text;
                
                UpdateWorkingGearItemLocationStock(
                    ((Label)_gridView.Rows[nCurrentRow].FindControl("lblWorkGearitemLocIdEdit")).Text,
                    ((Label)_gridView.Rows[nCurrentRow].FindControl("lblWorkGearItemIdEdit")).Text,
                    Convert.ToInt32(ucZone.SelectedZone),
                   ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtStockinHandEdit")).Text,
                   ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPriceEdit")).Text,
                    ucDate.Text,
                   ((Label)_gridView.Rows[nCurrentRow].FindControl("lblSizeCode")).Text);

                _gridView.EditIndex = -1;
                BindData();


            }
            SetPageNavigator();

            string Script = "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1',null,'keepopen');";
            Script += "</script>" + "\n";
            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvRegistersworkgearitemlocation_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.EditIndex = de.NewEditIndex;
            BindData();
            SetPageNavigator();
        }
        catch (Exception)
        {
            
            throw;
        }

    }

    protected void gvRegistersworkgearitemlocation_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);


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
            // Get the LinkButton control in the first cell
            LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Row.Attributes["ondblclick"] = _jsDouble;


            if (General.GetNullableDecimal(drv["FLDSTOCKQUANTITY"].ToString()) != null)
                edit.Visible = false;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
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
            gvRegistersworkgearitemlocation.SelectedIndex = -1;
            gvRegistersworkgearitemlocation.EditIndex = -1;
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

    private void UpdateWorkingGearItemLocationStock(string locationid, string itemid, int zoneid, string qty, string price, string opendate,string sizeid)
    {
        if (!IsValidWorkingGearItemStockUpdate(qty, opendate))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersWorkingGearItem.UpdateWorkingGearItemOpeningStock(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(locationid), new Guid(itemid), zoneid, decimal.Parse(qty), decimal.Parse(price), General.GetNullableDateTime(opendate),int.Parse(sizeid));
    }

    private bool IsValidWorkingGearItemStockUpdate(string qty, string opendate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (qty == "")
        {
            ucError.ErrorMessage = "Stock in hand is required";
        }
        if (General.GetNullableDecimal(qty).HasValue)
        {
            if (General.GetNullableDecimal(qty) < 0)
                ucError.ErrorMessage = "Stock in hand should not be Negative";
        }
        if (!General.GetNullableDateTime(opendate).HasValue)
        {
            ucError.ErrorMessage = "Opening date is required";
        }
        else if (General.GetNullableDateTime(opendate).HasValue)
        {
            if (DateTime.Parse(opendate) > DateTime.Today.Date)
                ucError.ErrorMessage = "Opening date should not be greater than today.";
        }
        return (!ucError.IsError);
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
}
