using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;

public partial class AccountsVesselVisitTravelClaimTravelAdvanceAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            Page.ClientScript.RegisterStartupScript(typeof(Page), "scroller", "<script>scrollToVal('divScroll', 'hdnScroll');</script>");
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsVesselVisitTravelClaimTravelAdvanceAdd.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvTravelAdvance')", "Print Grid", "icon_print.png", "PRINT");
            MenuTravelAdvance.AccessRights = this.ViewState;
            MenuTravelAdvance.MenuList = toolbargrid.Show();            

            if (!IsPostBack)
            {
                ViewState["VisitId"] = Request.QueryString["VisitId"];
                PhoenixToolbar toolbar = new PhoenixToolbar();                

                ViewState["PAGENUMBER"] = 1;
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTravelAdvanceMain_TabStripCommand(object sender, EventArgs e)
    {
    }
    protected void MenuTravelAdvance_TabStripCommand(object sender, EventArgs e)
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }


    protected void gvTravelAdvance_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
        }

    }

    protected void gvTravelAdvance_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((UserControlDate)_gridView.FooterRow.FindControl("txtPaidDateAdd")).Text,
                    ((UserControlCurrency)_gridView.FooterRow.FindControl("ucCurrencyAdd")).SelectedCurrency.ToString(),
                    ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtTakenAmountAdd")).Text,
                    ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtReturnAmountAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }

                //PhoenixAccountsVesselVIistTravelClaimRegister.TravelAdvanceManualInsert(new Guid(ViewState["VisitId"].ToString()),
                //    Convert.ToDateTime(((UserControlDate)_gridView.FooterRow.FindControl("txtPaidDateAdd")).Text),
                //    int.Parse(((UserControlCurrency)_gridView.FooterRow.FindControl("ucCurrencyAdd")).SelectedCurrency.ToString()),
                //    Convert.ToDecimal(((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtTakenAmountAdd")).Text),
                //    Convert.ToDecimal(((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtReturnAmountAdd")).Text),
                //    PhoenixSecurityContext.CurrentSecurityContext.UserCode);

                String script = "javascript:fnReloadList('codehelp1',null,'true');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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

    protected void gvTravelAdvance_ItemDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton cmdAdd = (ImageButton)e.Row.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName))
                    cmdAdd.Visible = false;
            }
        }
    }


    public void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDTRAVELADVANCENUMBER", "FLDPAIDDATE", "FLDCURRENCYCODE",  "FLDTAKENAMOUNT", "FLDRETURNAMOUNT", "FLDBALANCE", "FLDQUICKNAME" };
            string[] alCaptions = { "Travel Advance Number", "Paid Date", "Currency", "Taken Amount", "Return Amount", "Balance", "Advance Status" };

            DataSet ds = new DataSet();

            ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelAdvanceManualAddList(new Guid(ViewState["VisitId"].ToString()),
                                                                               (int)ViewState["PAGENUMBER"],
                                                                               General.ShowRecords(null),
                                                                               ref iRowCount,
                                                                               ref iTotalPageCount);

            General.SetPrintOptions("gvTravelAdvance", "Travel Advance Request", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvTravelAdvance.DataSource = ds;
                gvTravelAdvance.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvTravelAdvance);
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
        gv.Rows[0].Attributes["onclick"] = "";
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
        ViewState["ORDERID"] = null;
        BindData();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvTravelAdvance.SelectedIndex = -1;
        gvTravelAdvance.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;
        ViewState["ORDERID"] = null;
        BindData();
    }

    private bool IsValidData(string date, string currencyCode, string takenAmount, string returnAmount)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(date) == null)
            ucError.ErrorMessage = "Paid date is required.";

        if (currencyCode.Trim().Equals("Dummy"))
            ucError.ErrorMessage = "Currency code is required.";

        if (takenAmount.Trim().Equals(""))
            ucError.ErrorMessage = "Taken amount is required.";

        if (returnAmount.Trim().Equals(""))
            ucError.ErrorMessage = "Return amount is required.";

        return (!ucError.IsError);

    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDTRAVELADVANCENUMBER", "FLDPAIDDATE", "FLDCURRENCYCODE", "FLDTAKENAMOUNT", "FLDRETURNAMOUNT", "FLDBALANCE", "FLDQUICKNAME" };
        string[] alCaptions = { "Travel Advance Number", "Paid Date", "Currency", "Taken Amount", "Return Amount", "Balance", "Advance Status" };


        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelAdvanceManualAddList(new Guid(ViewState["VisitId"].ToString()),
                                                                              (int)ViewState["PAGENUMBER"],
                                                                              iRowCount,
                                                                              ref iRowCount,
                                                                              ref iTotalPageCount);
        Response.AddHeader("Content-Disposition", "attachment; filename=TravelAdvanceRequest.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Advance Request</h3></td>");
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
}
