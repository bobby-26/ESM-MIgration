using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;

public partial class PurchaseSchedulePOHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {               
                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddImageButton("../Purchase/PurchaseSchedulePOHistory.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvSchedulePOHistory')", "Print Grid", "icon_print.png", "PRINT");

                MenuOrderLineItem.AccessRights = this.ViewState;
                MenuOrderLineItem.MenuList = toolbargrid.Show();
                MenuOrderLineItem.SetTrigger(pnlStockItemEntry);

                toolbargrid = new PhoenixToolbar();
                toolbargrid.AddButton("Schedule PO", "SCHEDULEPO");
                toolbargrid.AddButton("Line Items", "LINEITEM");
                //toolbargrid.AddButton("Vessels", "VESSEL");
                toolbargrid.AddButton("History", "HISTORY");

                MenuSchedulePO.AccessRights = this.ViewState;
                MenuSchedulePO.MenuList = toolbargrid.Show();
                MenuSchedulePO.SelectedMenuIndex = 2;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
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

    protected void MenuSchedulePO_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("LINEITEM"))
        {
            Response.Redirect("../Purchase/PurchaseSchedulePOLineItemList.aspx", false);
        }
        if (dce.CommandName.ToUpper().Equals("VESSEL"))
        {
            Response.Redirect("../Purchase/PurchaseSchedulePOLineItemByVesselList.aspx", false);
        }
        if (dce.CommandName.ToUpper().Equals("SCHEDULEPO"))
        {
            Response.Redirect("../Purchase/PurchaseSchedulePOList.aspx", false);
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCREATEDATE", "FLDFORMNO", "FLDVESSELNAME" };
        string[] alCaptions = { "Created Date", "Form Number", "Vessel Name" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixPurchaseSchedulePO.SchedulePOHistorySearch(  General.GetNullableGuid(Filter.CurrentSelectedSchedulePO)
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , General.ShowRecords(null)
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        General.SetPrintOptions("gvSchedulePOHistory", "Schedule PO History", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvSchedulePOHistory.DataSource = ds;
            gvSchedulePOHistory.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvSchedulePOHistory);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;       
    }

    protected void MenuOrderLineItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }   

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCREATEDATE", "FLDFORMNO", "FLDVESSELNAME" };
        string[] alCaptions = { "Created Date", "Form Number", "Vessel Name" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPurchaseSchedulePO.SchedulePOHistorySearch( General.GetNullableGuid(Filter.CurrentSelectedSchedulePO)
                                                                , sortexpression
                                                                , sortdirection
                                                                , (int)ViewState["PAGENUMBER"]
                                                                , iRowCount
                                                                , ref iRowCount
                                                                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=SchedulePOHistory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Schedule PO History</h3></td>");
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

    protected void gvSchedulePOHistory_RowDataBound(object sender, GridViewRowEventArgs e)
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
    }

    protected void gvSchedulePOHistory_RowEditing(object sender, GridViewEditEventArgs de)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);
            _gridView.EditIndex = de.NewEditIndex;
            _gridView.SelectedIndex = de.NewEditIndex;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSchedulePOHistory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }   

    protected void gvSchedulePOHistory_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvSchedulePOHistory_PreRender(object sender, EventArgs e)
    {
        GridDecorator.MergeRows(gvSchedulePOHistory);
    }

    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];               

                Label lblCreatedDate = (Label)gridView.Rows[rowIndex].FindControl("lblCreatedDate");
                Label lblPreviousCreatedDate = ((Label)gridView.Rows[rowIndex + 1].FindControl("lblCreatedDate"));

                if (lblCreatedDate != null && lblPreviousCreatedDate != null && General.GetNullableDateTime(lblCreatedDate.Text) != null && General.GetNullableDateTime(lblPreviousCreatedDate.Text) != null)
                {
                    DateTime createddate = DateTime.Parse(lblCreatedDate.Text);
                    DateTime previouscreateddate = DateTime.Parse(lblPreviousCreatedDate.Text);

                    if (createddate == previouscreateddate)
                    {
                        row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                               previousRow.Cells[0].RowSpan + 1;
                        previousRow.Cells[0].Visible = false;

                    }
                }
            }
        }
    }   

    private bool IsValidExchangeRate(string strRef, string strExchangeRate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (strRef.Trim().ToUpper().Equals(""))
            ucError.ErrorMessage = "FX Contract Ref is required.";

        if (strExchangeRate.Trim().Equals(""))
            ucError.ErrorMessage = "Exchange Rate is required.";
        return (!ucError.IsError);
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvSchedulePOHistory.SelectedIndex = -1;
        gvSchedulePOHistory.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;
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
}
