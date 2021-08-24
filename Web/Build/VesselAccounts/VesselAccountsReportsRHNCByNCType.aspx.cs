using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Export2XL;

public partial class VesselAccountsReportsRHNCByNCType :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                //cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            string jvscript = "javascript:parent.Openpopup('codehelp1','','../VesselAccounts/VesselAccountsExport2XL.aspx?fleetid=" + ucFleet.SelectedFleet + "&fromdate=" + ucFromDate.Text + "&todate=" + ucToDate.Text + "&for=report3'); return false;";
            //toolbar.AddImageLink(jvscript, "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("../VesselAccounts/VesselAccountsReportsRHNCByNCType.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbar.AddImageLink("javascript:CallPrint('gvNC')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../VesselAccounts/VesselAccountsReportsRHNCByNCType.aspx", "Search", "search.png", "FIND");
            toolbar.AddImageButton("../VesselAccounts/VesselAccountsReportsRHNCByNCType.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuNC.AccessRights = this.ViewState;
            MenuNC.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("NC/Rank", "NCRANK");
            toolbar.AddButton("NC/Vessel Type", "NCVESSELTYPE");
            toolbar.AddButton("NC/NC Type", "NCTYPE");
            toolbar.AddButton("NC/System Cause", "NCSYSTEMCAUSE");
            toolbar.AddButton("NC/Reason", "NCREASON");
            toolbar.AddButton("NC/Action", "NCACTION");
            toolbar.AddButton("NC/Owner", "NCOWNER");
            MenuNCGeneral.AccessRights = this.ViewState;
            MenuNCGeneral.MenuList = toolbar.Show();
            MenuNCGeneral.SelectedMenuIndex = 2;

            BindData();
            SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void ShowExcel()
    {

    }

    protected void MenuNC_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidInput())
                {
                    ucError.Visible = true;
                    return;
                }
                gvNC.EditIndex = -1;
                gvNC.SelectedIndex = -1;
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                //ShowExcel();
                PhoenixVesselAccounts2XL.Export2XLVesselAccountsRHNCCountByNCtype(General.GetNullableInteger(ucFleet.SelectedFleet),
                    General.GetNullableDateTime(ucFromDate.Text), General.GetNullableDateTime(ucToDate.Text));
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ucFleet.SelectedFleet = "";
                ucFromDate.Text = "";
                ucToDate.Text = "";
                BindData();
                SetPageNavigator();
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

        string[] alColumns = { "FLDMONTH", "FLDS1", "FLDS2", "FLDS3", "FLDS4", "FLDS5", "FLDO1", "FLDO2" };
        string[] alCaptions = { "Month", "S1", "S2", "S3", "S4", "S5", "O1", "O2" };

        DataSet ds = new DataSet();

        ds = PhoenixVesselAccountsRHReports.SearchRHNCByNCtype(
                                                               General.GetNullableInteger(ucFleet.SelectedFleet)
                                                             , General.GetNullableDateTime(ucFromDate.Text)
                                                             , General.GetNullableDateTime(ucToDate.Text)
                                                             , (int)ViewState["PAGENUMBER"]
                                                             , General.ShowRecords(null)
                                                             , ref iRowCount
                                                             , ref iTotalPageCount);

        General.SetPrintOptions("gvNC", "NCs by NC Type", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvNC.DataSource = ds;
            gvNC.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvNC);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvNC_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvNC.EditIndex = -1;
        gvNC.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvNC.EditIndex = -1;
        gvNC.SelectedIndex = -1;
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
        gvNC.SelectedIndex = -1;
        gvNC.EditIndex = -1;
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

    private bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if (General.GetNullableInteger(ucFleet.SelectedFleet) == null)
        //    ucError.ErrorMessage = "Fleet is required.";

        if (General.GetNullableDateTime(ucFromDate.Text) == null)
            ucError.ErrorMessage = "From Date is required.";

        if (General.GetNullableDateTime(ucToDate.Text) == null)
            ucError.ErrorMessage = "To Date is required.";

        if (General.GetNullableDateTime(ucFromDate.Text) != null && General.GetNullableDateTime(ucToDate.Text) != null &&
            General.GetNullableDateTime(ucToDate.Text) < General.GetNullableDateTime(ucFromDate.Text))
            ucError.ErrorMessage = "To Date should be greater than From Date.";

        //if (General.GetNullableDateTime(ucFromDate.Text) != null && General.GetNullableDateTime(ucToDate.Text) != null &&
        //    DateTime.Parse(ucToDate.Text).Year != DateTime.Parse(ucFromDate.Text).Year)
        //    ucError.ErrorMessage = "Please select the range in same year.";

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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvNC.EditIndex = -1;
        gvNC.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void MenuNCGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("NCVESSELTYPE"))
        {
            Response.Redirect("../VesselAccounts/VesselAccountsReportsRHNCByVesselType.aspx");
        }
        else if (dce.CommandName.ToUpper().Equals("NCRANK"))
        {
            Response.Redirect("../VesselAccounts/VesselAccountsReportsRHNCByRank.aspx");
        }
        else if (dce.CommandName.ToUpper().Equals("NCSYSTEMCAUSE"))
        {
            Response.Redirect("../VesselAccounts/VesselAccountsReportsRHNCBySystemCause.aspx");
        }
        else if (dce.CommandName.ToUpper().Equals("NCREASON"))
        {
            Response.Redirect("../VesselAccounts/VesselAccountsReportsRHNCByReason.aspx");
        }
        else if (dce.CommandName.ToUpper().Equals("NCACTION"))
        {
            Response.Redirect("../VesselAccounts/VesselAccountsReportsRHNCByAction.aspx");
        }
        else if (dce.CommandName.ToUpper().Equals("NCOWNER"))
        {
            Response.Redirect("../VesselAccounts/VesselAccountsReportsRHNCByOwner.aspx");
        }
    }
}
