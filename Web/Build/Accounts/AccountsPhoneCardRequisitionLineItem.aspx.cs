using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;

public partial class AccountsPhoneCardRequisitionLineItem : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            if (!IsPostBack)
            {
                ViewState["ORDERID"] = null;
                ViewState["REQUESTID"] = null; 
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }

            if (Request.QueryString["orderid"] != null && !string.IsNullOrEmpty(Request.QueryString["orderid"].ToString()))
                ViewState["ORDERID"] = Request.QueryString["orderid"];
            if (Request.QueryString["requestid"] != null && !string.IsNullOrEmpty(Request.QueryString["requestid"].ToString()))
                ViewState["REQUESTID"] = Request.QueryString["requestid"];

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsPhoneCardRequisitionLineItem.aspx?orderid=" + ViewState["ORDERID"] + "&requestid=" + ViewState["REQUESTID"], "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvPhoneCardLineItems')", "Print Grid", "icon_print.png", "PRINT");
            MenuOffice.AccessRights = this.ViewState;
            MenuOffice.MenuList = toolbar.Show();
            MenuOffice.SetTrigger(pnlPhoneCardEntry);
                        
            BindSummary();
            BindData();
            SetPageNavigator();
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }

    private void BindSummary()
    {
        DataTable dt = PhoenixAccountsVesselAccounting.PhoneCardRequestSummarySearch(ViewState["ORDERID"] == null ? null : General.GetNullableGuid(ViewState["ORDERID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            gvSummary.DataSource = dt;
            gvSummary.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvSummary);
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDNAME", "FLDCARDNO", "FLDPINNO" };
        string[] alCaptions = { "Name", "Card Type", "Card Number", "PIN Number" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixAccountsVesselAccounting.PhoneCardLineItemSearch(ViewState["REQUESTID"]==null?null:General.GetNullableGuid(ViewState["REQUESTID"].ToString())
                , sortexpression, sortdirection
                , Int32.Parse(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null)
                , ref iRowCount
                , ref iTotalPageCount);

        General.SetPrintOptions("gvPhoneCardLineItems", "Phone Card Requisition Line Items", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvPhoneCardLineItems.DataSource = ds;
            gvPhoneCardLineItems.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvPhoneCardLineItems);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvPhoneCardLineItems_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvPhoneCardLineItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
    }

    protected void gvPhoneCardLineItems_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.SelectedIndex = e.NewEditIndex;            
            _gridView.EditIndex = e.NewEditIndex;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPhoneCardLineItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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

    protected void gvPhoneCardLineItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        string cardpinno = "";
        string lineitemid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestlineitemid")).Text;
        string cardnumber = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCardNo")).Text;
        string pinnumber = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPINNo")).Text;
        string rownumber = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRowNumber")).Text;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixAccountsVesselAccounting.PhoneCardLineItemSearch(ViewState["REQUESTID"] == null ? null : General.GetNullableGuid(ViewState["REQUESTID"].ToString())
                , sortexpression, sortdirection
                , 1, int.Parse(ViewState["ROWCOUNT"].ToString())
                , ref iRowCount
                , ref iTotalPageCount);

        try
        {
            if (!IsValidPhoneCardLineItem(cardnumber, pinnumber))
            {
                ucError.Visible = true;
                return;
            }

            for (int i = 0; i < int.Parse(ViewState["ROWCOUNT"].ToString()); i++)
            {
                if (i+1 == int.Parse(rownumber))
                {
                    cardpinno = cardpinno + cardnumber + "-" + pinnumber+",";
                }
                if (i+1 != int.Parse(rownumber))
                {
                    if (lineitemid == ds.Tables[0].Rows[i]["FLDREQUESTLINEID"].ToString())
                    {
                        string cardno = ds.Tables[0].Rows[i]["FLDCARDNO"].ToString();
                        string pinno = ds.Tables[0].Rows[i]["FLDPINNO"].ToString();
                        if(cardno + "-" + pinno != "-")
                              cardpinno = cardpinno + cardno + "-" + pinno + ",";
                    }
                }
            }            
            if (cardpinno.EndsWith(","))
                cardpinno = cardpinno.Substring(0, cardpinno.Length-1);
            PhoenixAccountsVesselAccounting.PhoneCardNumberUpdate(new Guid(lineitemid), cardpinno);           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

        _gridView.EditIndex = -1;
        SetPageNavigator();
        BindData();
    }

    protected void gvPhoneCardLineItems_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
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

        }
    }

    private bool IsValidPhoneCardLineItem(string cardno,string pinno)
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (string.IsNullOrEmpty(cardno))
            ucError.ErrorMessage = "Card number is required.";

        if (string.IsNullOrEmpty(pinno))
            ucError.ErrorMessage = "PIN number is required.";           

        return (!ucError.IsError);
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDEMPLOYEENAME", "FLDNAME", "FLDCARDNO", "FLDPINNO" };
        string[] alCaptions = { "Name", "Card Type", "Card Number", "PIN Number" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsVesselAccounting.PhoneCardLineItemSearch(General.GetNullableGuid(ViewState["REQUESTID"].ToString())
                , sortexpression, sortdirection
                , Int32.Parse(ViewState["PAGENUMBER"].ToString()), iRowCount
                , ref iRowCount
                , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=PhoneCardLineItems.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Phone Card Requisition Line Items</h3></td>");
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
        gvPhoneCardLineItems.SelectedIndex = -1;
        gvPhoneCardLineItems.EditIndex = -1;
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

    protected void MenuOffice_TabStripCommand(object sender, EventArgs e)
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

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
        BindData();
        SetPageNavigator();
    }
}
