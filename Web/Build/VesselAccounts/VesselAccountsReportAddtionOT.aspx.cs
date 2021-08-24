using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;

public partial class VesselAccountsReportAddtionOT : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddImageButton("../VesselAccounts/VesselAccountsReportAddtionOT.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvOvertimeHolidays')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../VesselAccounts/VesselAccountsReportAddtionOT.aspx", "Search", "search.png", "FIND");
            toolbar.AddImageButton("../VesselAccounts/VesselAccountsReportAddtionOT.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuEarningdeductionList.AccessRights = this.ViewState;
            MenuEarningdeductionList.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    ddlVessel.Enabled = false;
                else
                    ddlVessel.Enabled = true;
                BindYear();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlEmployee_TextChangedEvent(object sender, EventArgs e)
    {
        if (rblEarningDeduction.SelectedValue.Equals("128"))
        {
            ddlEntryTypeEarning.Visible = true;
            ddlEntryTypeDeduction.Visible = false;
        }
        else
        {
            ddlEntryTypeDeduction.Visible = true;
            ddlEntryTypeEarning.Visible = false;
        }
        BindData();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();
            string[] alColumns = { "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDCOMPONENT", "FLDCURRENCY", "FLDAMOUNT" };
            string[] alCaptions = { "Vessel", "File No.", "Name", "Rank", "Entry Type", "Currency", "Amount" };
            //if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            //    iRowCount = 1;
            //else
            //    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            int? entrytype;

            if (rblEarningDeduction.SelectedValue == "128")
            {
                entrytype = General.GetNullableInteger(ddlEntryTypeEarning.SelectedHard);
            }
            else
            {
                entrytype = General.GetNullableInteger(ddlEntryTypeDeduction.SelectedHard);
            }
            ds = PhoenixVesselAccountsEarningsDeductions.ReportEarningDeductionOnboard(General.GetNullableInteger(ddlVessel.SelectedVessel)
                                        , entrytype
                                        , General.GetNullableInteger(ddlMonth.SelectedValue)
                                        , General.GetNullableInteger(ddlYear.SelectedValue)
                                        , int.Parse(rblEarningDeduction.SelectedValue)
                                        , int.Parse(ViewState["PAGENUMBER"].ToString())
                                        , General.ShowRecords(null)
                                        , ref iRowCount
                                        , ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {

                gvOvertimeHolidays.DataSource = ds.Tables[0];
                gvOvertimeHolidays.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvOvertimeHolidays);
            }
            General.SetPrintOptions("gvOvertimeHolidays", "Earning and Deduction", alCaptions, alColumns, ds);
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
    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year); i++)
        {
            ListItem li = new ListItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);
        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
    }

    protected void MenuEarningdeductionList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
            }
            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                ddlYear.SelectedValue = DateTime.Today.Year.ToString();
                ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                    ddlVessel.Enabled = false;
                else
                    ddlVessel.Enabled = true;
                rblEarningDeduction.SelectedValue = "128";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ddlEntryTypeEarning.Visible = true;
                ddlEntryTypeDeduction.Visible = false;
                ddlEntryTypeDeduction.SelectedHard = "";
                ddlEntryTypeEarning.SelectedHard = "";
                BindData();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string date = DateTime.Now.ToShortDateString();

                DataSet ds = new DataSet();
                string[] alColumns = { "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDCOMPONENT", "FLDCURRENCY", "FLDAMOUNT" };
                string[] alCaptions = { "Vessel", "File No.", "Name", "Rank", "Entry Type", "Currency", "Amount" };
                int? entrytype;

                if (rblEarningDeduction.SelectedValue == "128")
                    entrytype = General.GetNullableInteger(ddlEntryTypeEarning.SelectedHard);
                else
                    entrytype = General.GetNullableInteger(ddlEntryTypeDeduction.SelectedHard);
                string sortexpression;
                int? sortdirection = null;

                sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                ds = PhoenixVesselAccountsEarningsDeductions.ReportEarningDeductionOnboard(General.GetNullableInteger(ddlVessel.SelectedVessel)
                                     , entrytype
                                     , General.GetNullableInteger(ddlMonth.SelectedValue)
                                     , General.GetNullableInteger(ddlYear.SelectedValue)
                                     , int.Parse(rblEarningDeduction.SelectedValue)
                                     , int.Parse(ViewState["PAGENUMBER"].ToString())
                                     , iRowCount
                                     , ref iRowCount
                                     , ref iTotalPageCount);
                General.ShowExcel("Earning and Deduction", ds.Tables[0], alColumns, alCaptions, null, null);

            }
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
        lblPagenumber.Text = "Page " + (ViewState["TOTALPAGECOUNT"].ToString() == "0" ? ViewState["TOTALPAGECOUNT"].ToString() : ViewState["PAGENUMBER"].ToString());
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
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
