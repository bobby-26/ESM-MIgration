using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class Crew_CrewLicenceDepositUtilization : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewLicenceDepositUtilization.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvRequestsMade')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Crew/CrewLicenceDepositUtilization.aspx", "Find", "search.png", "FIND");
            DepositUtilization.AccessRights = this.ViewState;
            DepositUtilization.MenuList = toolbar.Show();

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                NameValueCollection nvc = Filter.CurrentLicenceDepositFilter;

                if (nvc != null)
                {
                    ucConsulate.SelectedAddress = nvc.Get("ucConsulate");
                    ucCurrency.SelectedCurrency = nvc.Get("ucCurrency").ToString() == "Dummy" ? "0" : nvc.Get("ucCurrency");
                    chkShowAll.Checked = nvc.Get("chkShowAll") == "1" ? true : false;
                }
            }
        }

        BindData();
        SetPageNavigator();
    }

    private Boolean IsPreviousEnabled1()
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

    private Boolean IsNextEnabled1()
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

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDREFNUMBER", "FLDNAME", "FLDCURRENCYCODE", "FLDRANKCREWNAME", "FLDREQUSTAMOUNT", "FLDOFFSETAMOUNT", "FLDBALANCE", "FLDSTATUS" };
        string[] alCaptions = { "Request Number", "Consulate", "Currency", "Rank/Crew Name", "Request Amount", "Offset Amount", "Balance", "Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentLicenceDepositFilter;

        ds = PhoenixCrewLicenceDepositUtilization.LicenceRequestsMade(
           General.GetNullableInteger(nvc != null ? nvc.Get("ucConsulate") : null)
           , General.GetNullableInteger(nvc != null ? nvc.Get("ucCurrency") : null),
           sortexpression, sortdirection,
           Int32.Parse(ViewState["PAGENUMBER"].ToString()),
           iRowCount,
           ref iRowCount,
           ref iTotalPageCount,
           General.GetNullableInteger(nvc != null ? nvc.Get("chkShowAll") : "0"));

        Response.AddHeader("Content-Disposition", "attachment; filename=DepositUtilization.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Deposit Utilization</h3></td>");
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

    protected void DepositUtilization_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;

            NameValueCollection criteria = new NameValueCollection();

            criteria.Clear();

            criteria.Add("ucConsulate", ucConsulate.SelectedAddress);
            criteria.Add("chkShowAll", chkShowAll.Checked == true ? "1" : "0");
            criteria.Add("ucCurrency", ucCurrency.SelectedCurrency);

            Filter.CurrentLicenceDepositFilter = criteria;

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

        string[] alColumns = { "FLDREFNUMBER", "FLDNAME", "FLDCURRENCYCODE", "FLDRANKCREWNAME", "FLDREQUSTAMOUNT", "FLDOFFSETAMOUNT", "FLDBALANCE", "FLDSTATUS" };
        string[] alCaptions = { "Request Number", "Consulate", "Currency", "Rank/Crew Name", "Request Amount", "Offset Amount", "Balance", "Status" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentLicenceDepositFilter;

        ds = PhoenixCrewLicenceDepositUtilization.LicenceRequestsMade(
            General.GetNullableInteger(nvc != null ? nvc.Get("ucConsulate") : null)
            ,General.GetNullableInteger(nvc != null ? nvc.Get("ucCurrency") : null),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(nvc != null ? nvc.Get("chkShowAll") : "0"));

        General.SetPrintOptions("gvRequestsMade", "Deposit Utilization", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvRequestsMade.DataSource = ds;
            gvRequestsMade.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvRequestsMade);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        ViewState["SORTEXPRESSION"] = ib.CommandName;
        ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
    }

    protected void gvRequestsMade_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvRequestsMade_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["REQUESTID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblProcessId")).Text;
                ViewState["CONSULATEID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblConsulateId")).Text;
                ViewState["CURRENCYID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCurrency")).Text;

                Response.Redirect("../Crew/CrewLicenceDepositAdvanceRequest.aspx?CONSULATEID=" + ViewState["CONSULATEID"].ToString() + "&CURRENCYID=" + ViewState["CURRENCYID"] + "&REQUESTID=" + ViewState["REQUESTID"].ToString(), true);

                //LinkButton lnk = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkRequestNumber");
                //lnk.Attributes.Add("onclick", "parent.Openpopup('codehelp', '', 'CrewLicenceDepositAdvanceRequest.aspx?CONSULATEID=" + ViewState["CONSULATEID"].ToString() + "&CURRENCYID=" + ViewState["CURRENCYID"].ToString() + "');return false;");

                //String scriptpopup = String.Format(
                //        "javascript:Openpopup('codehelp1', '', 'CrewLicenceDepositAdvanceRequest.aspx?CONSULATEID=" + ViewState["CONSULATEID"].ToString() + "&CURRENCYID=" + ViewState["CURRENCYID"] + "&REQUESTID=" + ViewState["REQUESTID"].ToString() + "');");
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRequestsMade_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvRequestsMade.SelectedIndex = -1;
        gvRequestsMade.EditIndex = -1;
        ViewState["CURRENTROW"] = -1;

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvRequestsMade_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvRequestsMade_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindData();
        SetPageNavigator();
    }

    protected void gvRequestsMade_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            string lblprocessid = ((Label)e.Row.FindControl("lblProcessId")).Text;

            ImageButton history = (ImageButton)e.Row.FindControl("cmdHistory");
            if (history != null)
            {
                history.Visible = SessionUtil.CanAccess(this.ViewState, history.CommandName);
                history.Attributes.Add("onclick", "Openpopup('history', '', '../Crew/CrewLicenceDepositUtilizationView.aspx?processid=" + lblprocessid + "');return false;");
            }
        }


        if (e.Row.RowType == DataControlRowType.Footer)
        {
           
        }

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvRequestsMade.SelectedIndex = -1;
        gvRequestsMade.EditIndex = -1;
        ViewState["CURRENTROW"] = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvRequestsMade.SelectedIndex = -1;
        gvRequestsMade.EditIndex = -1;
        ViewState["CURRENTROW"] = -1;

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
        gvRequestsMade.SelectedIndex = -1;
        gvRequestsMade.EditIndex = -1;
        ViewState["CURRENTROW"] = -1;
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
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
}
