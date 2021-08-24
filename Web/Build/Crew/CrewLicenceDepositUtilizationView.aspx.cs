using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class Crew_CrewLicenceDepositUtilizationView : PhoenixBasePage
{
  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewLicenceDepositUtilizationView.aspx?advancepaymentid=" + ViewState["advancepaymentid"] + "&processid="+ ViewState["processid"], "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvDepositUtilized')", "Print Grid", "icon_print.png", "PRINT");
            //toolbar.AddImageButton("../Crew/CrewLicenceDepositUtilizationView.aspx", "Find", "search.png", "FIND");
            MenuDepositUtilized.AccessRights = this.ViewState;
            MenuDepositUtilized.MenuList = toolbar.Show();
            //MenuRegistersAirlines.SetTrigger(pnlAirlinesEntry);

            if (Request.QueryString["advancepaymentid"] != null)
                ViewState["advancepaymentid"] = Request.QueryString["advancepaymentid"].ToString();
            else
                ViewState["advancepaymentid"] = "";

            if (Request.QueryString["processid"] != null)
                ViewState["processid"] = Request.QueryString["processid"].ToString();
            else
                ViewState["processid"] = "";
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
            AmountDetailsEdit();
            BindData();
            SetPageNavigator();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void AmountDetailsEdit()
    {
        DataSet ds = new DataSet();

        ds = PhoenixCrewLicenceDepositUtilization.CrewDepositDetailsEdit(General.GetNullableGuid(ViewState["advancepaymentid"].ToString())
                                                                            , General.GetNullableGuid(ViewState["processid"].ToString()));

        txtDepositAmount.Text = ds.Tables[0].Rows[0]["FLDAMOUNT"].ToString();
        txtDepositCurrency.Text = ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString();
        txtBalance.Text = ds.Tables[0].Rows[0]["FLDREMAININGAMT"].ToString();
        
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDADVANCEPAYMENTNUMBER", "FLDREFNUMBER", "FLDAMOUNT", "FLDDEPOSITDATE" };
        string[] alCaptions = { "Advance Payment Number", "Request Number", "Request Amount", "Request Date" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewLicenceDepositUtilization.UtilizedAmountSearch(General.GetNullableGuid(ViewState["advancepaymentid"].ToString())
                                                     , General.GetNullableGuid(ViewState["processid"].ToString())
                                                     , sortexpression, sortdirection
                                                     , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                     , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Utilized Deposit.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Utilized Deposit</h3></td>");
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

    protected void MenuDepositUtilized_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvDepositUtilized.SelectedIndex = -1;
                gvDepositUtilized.EditIndex = -1;

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
        string[] alColumns = { "FLDADVANCEPAYMENTNUMBER", "FLDREFNUMBER", "FLDAMOUNT", "FLDDEPOSITDATE" };
        string[] alCaptions = { "Advance Payment Number", "Request Number", "Request Amount", "Request Date" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixCrewLicenceDepositUtilization.UtilizedAmountSearch(General.GetNullableGuid(ViewState["advancepaymentid"].ToString())
                                                    , General.GetNullableGuid(ViewState["processid"].ToString())
                                                    , sortexpression, sortdirection
                                                    , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                    , ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvDepositUtilized", "Utilized Deposit", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvDepositUtilized.DataSource = ds;
            gvDepositUtilized.DataBind();
            ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvDepositUtilized);
            ViewState["ROWSINGRIDVIEW"] = 0;
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    public void gvDepositUtilized_OnRowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("REMOVE"))
            {
                string depositid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblDepositUtilizationid")).Text;

                PhoenixCrewLicenceDepositUtilization.DeleteDepositAllocation(General.GetNullableGuid(depositid));

                BindData();
                SetPageNavigator();

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null, true);", true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void gvDepositUtilized_ItemDataBound(object sender,GridViewRowEventArgs e)
    {
        if (ViewState["advancepaymentid"].ToString() == "")
            e.Row.Cells[4].Visible = false;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string balanceremaining = ((Label)e.Row.FindControl("lblRequestBalance")).Text;

            if (balanceremaining == "0")
            {
                ImageButton remove = (ImageButton)e.Row.FindControl("cmdRemove");
                remove.Visible = false;
            }
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvDepositUtilized.SelectedIndex = -1;
        gvDepositUtilized.EditIndex = -1;

        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        gvDepositUtilized.SelectedIndex = -1;
        gvDepositUtilized.EditIndex = -1;

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
        gvDepositUtilized.SelectedIndex = -1;
        gvDepositUtilized.EditIndex = -1;
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
