using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.VesselAccounts;

public partial class AccountsPhoneCardRequisition : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsPhoneCardRequisition.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvPhoneCards')", "Print Grid", "icon_print.png", "PRINT");
            MenuOffice.AccessRights = this.ViewState;
            MenuOffice.MenuList = toolbar.Show();
            MenuOffice.SetTrigger(pnlPhoneCardEntry);

            if (!Page.IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = "AccountsPhoneCardRequisitionLineItem.aspx";
                ViewState["ORDERID"] = null;
                ViewState["REQUESTID"] = null;
            }

            BindData();
            SetPageNavigator();            
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"] + "?orderid=" + (ViewState["ORDERID"]==null?null:ViewState["ORDERID"].ToString()) + "&requestid=" + (ViewState["REQUESTID"]==null?null:ViewState["REQUESTID"].ToString());
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
            ViewState["ORDERID"] = string.Empty;
            ViewState["REQUESTID"] = string.Empty;
            gvPhoneCards.EditIndex = -1;
            gvPhoneCards.SelectedIndex = -1;
            BindData();
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"] + "?orderid=" + ViewState["ORDERID"] + "&requestid=" + ViewState["REQUESTID"];
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

        string[] alColumns = { "FLDVESSELNAME", "FLDREFERENCENO", "FLDREQUESTDATE", "FLDSENTDATE", "FLDSUPPLIER", "FLDBUDGETCODE", "FLDBILLTOCOMPANYNAME" };
        string[] alCaptions = { "Vessel", "Requisition Number", "Order Date", "Sent Date", "Supplier", "Budget Code", "Bill to Company" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
       
        //shows only pending status requisitions

        DataSet ds = PhoenixAccountsVesselAccounting.PhoneCardRequisitionSearch(null
                            , null
                            , null
                            , null                            
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvPhoneCards", "Phone Card Requisition", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["ORDERID"] == null && ViewState["REQUESTID"] == null)
            {
                ViewState["ORDERID"] = ds.Tables[0].Rows[0]["FLDORDERID"].ToString();
                ViewState["REQUESTID"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();
                gvPhoneCards.SelectedIndex = 0;
            }
            gvPhoneCards.DataSource = ds;
            gvPhoneCards.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ifMoreInfo.Attributes["src"] = "AccountsPhoneCardRequisitionLineItem.aspx";
            ShowNoRecordsFound(dt, gvPhoneCards);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvPhoneCards_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvPhoneCards_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
        
        if (e.CommandName.ToString().ToUpper() == "SELECT")
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            _gridView.SelectedIndex = nCurrentRow;

            Label lblorderid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderid"));
            Label lblRequestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestid"));
            ViewState["ORDERID"] = lblorderid.Text;
            ViewState["REQUESTID"] = lblRequestid.Text;
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"] + "?orderid=" + ViewState["ORDERID"] + "&requestid=" + ViewState["REQUESTID"];
            BindData();
            SetPageNavigator();
        }
    }

    protected void gvPhoneCards_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
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

    protected void gvPhoneCards_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            _gridView.SelectedIndex = e.NewEditIndex;

            Label lblOrderid = ((Label)_gridView.Rows[e.NewEditIndex].FindControl("lblOrderid"));
            Label lblRequestid = ((Label)_gridView.Rows[e.NewEditIndex].FindControl("lblRequestid"));
            ViewState["ORDERID"] = lblOrderid.Text;
            ViewState["REQUESTID"] = lblRequestid.Text;
            ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"] + "?orderid=" + ViewState["ORDERID"] + "&requestid=" + ViewState["REQUESTID"];

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

    protected void gvPhoneCards_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;

        try
        {
            string lblRequestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestid")).Text;
            string budgetid = ((UserControlBudgetCode)_gridView.Rows[nCurrentRow].FindControl("ucBudgetCodeEdit")).SelectedBudgetCode;
            string compaddr = ((UserControlCompany)_gridView.Rows[nCurrentRow].FindControl("ddlCompany")).SelectedCompany;
            string vendorid = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtVendorId")).Text;

            if (!IsValidPhoneCardMapping(vendorid,budgetid,compaddr))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixAccountsVesselAccounting.PhoneCardRequestMappingInsert(new Guid(lblRequestid.ToString()),
                General.GetNullableInteger(budgetid.ToString()), General.GetNullableInteger(compaddr.ToString()), General.GetNullableInteger(vendorid));            
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

    protected void gvPhoneCards_RowDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            UserControlBudgetCode ucBudgetCodeEdit = (UserControlBudgetCode)e.Row.FindControl("ucBudgetCodeEdit");
            DataRowView drv = (DataRowView)e.Row.DataItem;

            UserControlCompany ucCompany = (UserControlCompany)e.Row.FindControl("ddlCompany");
            if (ucCompany != null && drv["FLDBILLTOCOMPANY"].ToString() != "")
                ucCompany.SelectedCompany = drv["FLDBILLTOCOMPANY"].ToString();
            else if (ucCompany != null && drv["FLDBILLTOCOMPANY"].ToString() == "")
                ucCompany.SelectedCompany = "16";

            if (ucBudgetCodeEdit != null && drv["FLDBUDGETCODE"].ToString() != "")
                ucBudgetCodeEdit.SelectedBudgetSubAccount = drv["FLDBUDGETCODE"].ToString();
            else if (ucBudgetCodeEdit != null && drv["FLDBUDGETCODE"].ToString() == "")
                ucBudgetCodeEdit.SelectedBudgetSubAccount = "2097";

            TextBox vendorid = (TextBox)e.Row.FindControl("txtVendorId");
            if (vendorid != null)
            {
                vendorid.Attributes.Add("style", "visibility:hidden");
                vendorid.Text = drv["FLDADDRESSCODE"].ToString();
            }
            TextBox txtVendorCode = (TextBox)e.Row.FindControl("txtVendorCode");
            if (txtVendorCode != null) txtVendorCode.Text = drv["FLDSUPPLIERCODE"].ToString();
            TextBox txtVendorName = (TextBox)e.Row.FindControl("txtVendorName");
            if (txtVendorName != null) txtVendorName.Text = drv["FLDSUPPLIER"].ToString();            
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDREFERENCENO", "FLDREQUESTDATE", "FLDSENTDATE", "FLDSUPPLIER", "FLDBUDGETCODE", "FLDBILLTOCOMPANYNAME" };
        string[] alCaptions = { "Vessel", "Requisition Number", "Order Date", "Sent Date", "Supplier", "Budget Code", "Bill to Company" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //shows only pending status requisitions

        DataSet ds = PhoenixAccountsVesselAccounting.PhoneCardRequisitionSearch(null
                            , null
                            , null
                            , null
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=PhoneCardRequisition.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Phone Card Requisition</h3></td>");
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
        gvPhoneCards.SelectedIndex = -1;
        gvPhoneCards.EditIndex = -1;
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

    private bool IsValidPhoneCardMapping(string supplier, string budgetid, string billtocomp)
    {
        ucError.HeaderMessage = "Please provide the following information.";

        if (General.GetNullableInteger(supplier) == null)
            ucError.ErrorMessage = "Supplier is required.";

        if (General.GetNullableInteger(budgetid) == null)
            ucError.ErrorMessage = "Budget code is required.";

        if (General.GetNullableInteger(billtocomp) == null)
            ucError.ErrorMessage = "Bill to company is required.";

        return (!ucError.IsError);
    }
}
