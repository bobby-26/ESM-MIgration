using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI.HtmlControls;
using System.Drawing;
using Telerik.Web.UI;
public partial class CrewEarningdeductionSummary : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        Table gridTable1 = (Table)gvCrewSearch.Controls[0];
        ViewState["Category"] = "";

        foreach (GridViewRow gv in gvCrewSearch.Rows)
        {
            Label lblType = (Label)gv.FindControl("lblType");
            if (lblType != null)
            {
                if (ViewState["Category"].ToString().Trim().Equals("") || !ViewState["Category"].ToString().Trim().Equals(lblType.Text.Trim()))
                {

                    ViewState["Category"] = lblType.Text.Trim();
                    int rowIndex = gridTable1.Rows.GetRowIndex(gv);
                    // Add new group header row  
                    GridViewRow headerRow = new GridViewRow(rowIndex, rowIndex, DataControlRowType.DataRow, DataControlRowState.Normal);
                    TableCell headerCell = new TableCell();
                    headerCell.ColumnSpan = gvCrewSearch.Columns.Count;
                    headerCell.Text = @"<font size=""2"" ><b>" + string.Format("{0}", lblType != null ? lblType.Text.Trim() : "") + "</b></font>";
                    headerCell.CssClass = "GroupHeaderRowStyle";
                    // Add header Cell to header Row, and header Row to gridTable  
                    headerRow.Cells.Add(headerCell);
                    headerRow.HorizontalAlign = HorizontalAlign.Left;
                    gridTable1.Controls.AddAt(rowIndex, headerRow);
                }
            }
        }
        base.Render(writer);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Summary", "LIST");
            toolbar1.AddButton("Bond Issue & Phone Cards", "BONDPHONECARD");
            toolbar1.AddButton("Earnings & Deductions", "EARNINGDEDUCTION");
            toolbar1.AddButton("List", "BACK");
            MenuPettyCash.AccessRights = this.ViewState;
            MenuPettyCash.MenuList = toolbar1.Show();
            MenuPettyCash.SelectedMenuIndex = 2;
            if (!IsPostBack)
            {
                Title1.Text = "Earnings & Deductions";
                SetEmployeePrimaryDetails(Request.QueryString["empid"].ToString());
                PortageDetailsBindData();
            }
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Crew/CrewEarningdeductionSummary.aspx?empid=" + Request.QueryString["empid"].ToString() + "&Portagebillid=" + Request.QueryString["Portagebillid"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvCrewSearch')", "Print Grid", "icon_print.png", "PRINT");
            MenuCTM.MenuList = toolbar.Show();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ddlType_TextChangedEvent(object sender, EventArgs e)
    {
        BindData();
    }
    public void SetEmployeePrimaryDetails(string empid)
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(empid));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void PortageDetailsBindData()
    {
        DataSet ds1 = new DataSet();
        ds1 = PhoenixCrewPortagebill.CrewPortagebillBreakUp(int.Parse(Request.QueryString["empid"].ToString()), new Guid(Request.QueryString["Portagebillid"].ToString()));
        if (ds1.Tables[1].Rows.Count > 0)
        {
            ViewState["VESSELID"] = ds1.Tables[1].Rows[0]["FLDVESSELID"].ToString();
            txtvessel.Text = ds1.Tables[1].Rows[0]["FLDVESSELNAME"].ToString();
            txtTodate.Text = ds1.Tables[1].Rows[0]["FLDTODATE"].ToString();
            txtfromdate.Text = ds1.Tables[1].Rows[0]["FLDFROMDATE"].ToString();
            txtdays.Text = ds1.Tables[1].Rows[0]["FLDDAYS"].ToString();
            txtsignonRank.Text = ds1.Tables[1].Rows[0]["FLDRANKNAME"].ToString();
        }
    }
    public void BindData()
    {
        string[] alColumns = { "FLDCOMPONENTNAME", "FLDEARNINGDEDUCTION", "FLDPURPOSE", "FLDMONTHYEAR", "FLDDATE", "FLDAMOUNT" };
        string[] alCaptions = { "Component Name", "Earnings/Deduction", "Purpose", "Month & Year", "Date", "Amount" };
        try
        {
            DataTable dt = PhoenixCrewPortagebill.CrewPortageearningdeductionCrew(int.Parse(Request.QueryString["empid"].ToString()), int.Parse(ViewState["VESSELID"].ToString())
                                                                    , DateTime.Parse(txtfromdate.Text)
                                                                          , DateTime.Parse(txtTodate.Text), new Guid(Request.QueryString["Portagebillid"].ToString()));
            if (dt.Rows.Count > 0)
            {
                gvCrewSearch.DataSource = dt;
                gvCrewSearch.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvCrewSearch);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCrewSearch", "Earnings Deductions", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuPettyCash_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("BONDPHONECARD"))
            {
                Response.Redirect("../Crew/CrewBondSummary.aspx?empid=" + Request.QueryString["empid"].ToString() + "&Portagebillid=" + Request.QueryString["Portagebillid"].ToString(), false);
            }
            else if (dce.CommandName.ToUpper().Equals("EARNINGDEDUCTION"))
            {
                Response.Redirect("../Crew/CrewEarningdeductionSummary.aspx?empid=" + Request.QueryString["empid"].ToString() + "&Portagebillid=" + Request.QueryString["Portagebillid"].ToString(), false);
            }
            else if (dce.CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Crew/CrewPortagebillHistory.aspx?empid=" + Request.QueryString["empid"].ToString() + "&Portagebillid=" + Request.QueryString["Portagebillid"].ToString(), false);
            }
            else if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Crew/CrewEmployeePortageBillList.aspx?empid=" + Request.QueryString["empid"].ToString(), false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCTM_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDCOMPONENTNAME", "FLDEARNINGDEDUCTION", "FLDPURPOSE", "FLDMONTHYEAR", "FLDDATE", "FLDAMOUNT" };
                string[] alCaptions = { "Component Name", "Earnings/Deduction", "Purpose", "Month & Year", "Date", "Amount" };
                DataTable dt = PhoenixCrewPortagebill.CrewPortageearningdeductionCrew(int.Parse(Request.QueryString["empid"].ToString()), int.Parse(ViewState["VESSELID"].ToString())
                                                                        , DateTime.Parse(txtfromdate.Text)
                                                                              , DateTime.Parse(txtTodate.Text), new Guid(Request.QueryString["Portagebillid"].ToString()));
                General.ShowExcel("Earnings Deduction", dt, alColumns, alCaptions, null, null);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
   
    decimal qtyTotal = 0;
    decimal grQtyTotal = 0;
    string storid = string.Empty;
    int rowIndex = 1;
    protected void gvCrewSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            storid = DataBinder.Eval(e.Row.DataItem, "FLDCOMPONENTNAME").ToString();
            decimal tmpTotal = decimal.Parse(DataBinder.Eval(e.Row.DataItem, "FLDAMOUNT").ToString()) * decimal.Parse(DataBinder.Eval(e.Row.DataItem, "FLDEARDEDTYPE").ToString());
            qtyTotal += tmpTotal;
            grQtyTotal += tmpTotal;
        }
      
    }
    protected void gvCrewSearch_RowCreated(object sender, GridViewRowEventArgs e)
    {
        bool newRow = false;
        if ((storid != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "FLDCOMPONENTNAME") != null))
        {
            if (storid != DataBinder.Eval(e.Row.DataItem, "FLDCOMPONENTNAME").ToString())
                newRow = true;
        }
        if ((storid != string.Empty) && (DataBinder.Eval(e.Row.DataItem, "FLDCOMPONENTNAME") == null))
        {
            newRow = true;
            rowIndex = 0;
        }
        if (newRow)
        {
            GridView gvCrewSearch = (GridView)sender;
            GridViewRow NewTotalRow = new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Insert);
            NewTotalRow.Font.Bold = true;
            TableCell HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Left;
            HeaderCell.ColumnSpan = 3;
            NewTotalRow.Cells.Add(HeaderCell); HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Right;
            HeaderCell.Text = "Sub Total";
            NewTotalRow.Cells.Add(HeaderCell);
            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Right;
            HeaderCell.Text = (Math.Abs(qtyTotal)).ToString();
            NewTotalRow.Cells.Add(HeaderCell);
            gvCrewSearch.Controls[0].Controls.AddAt(e.Row.RowIndex + rowIndex, NewTotalRow);
            rowIndex++;
            qtyTotal = 0;
        }
    }
}