using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewContractCBAdetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Contract Information", "CONTRACTINFORMATION");
            toolbar.AddButton("CBA ", "CBACOMPONENT");
            toolbar.AddButton("Standard ", "STANDARDCOMPONENT");
            toolbar.AddButton("Crew Agreed", "CREWAGREECOMPONENT");
            toolbar.AddButton("Contract Letter", "CONTRACT");
            toolbar.AddButton("Back", "LIST");
            MenuCrewContract.AccessRights = this.ViewState;
            MenuCrewContract.MenuList = toolbar.Show();
            MenuCrewContract.SelectedMenuIndex = 1;
            if (!IsPostBack)
                EditContractDetails();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddImageButton("../Crew/CrewContractCBAdetails.aspx?Contractid=" + Request.QueryString["Contractid"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbar1.AddImageLink("javascript:CallPrint('gvCBA')", "Print Grid", "icon_print.png", "PRINT");
            Menuexport.MenuList = toolbar1.Show();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Menuexport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                string[] alColumns = { "FLDCOMPONENTNAME", "FLDINCLUDECONTEARDESC", "FLDINCLUDECONTDEDDESC", "FLDCALUNITBASISNAME", "FLDCALTIMEBASISNAME", "FLDONBPAYDEDNAME", "FLDCURRENCYCODE" };
                string[] alCaptions = { "Component", "Earnings", "Deductions", "Calculation Unit", "Calculation Unit", "Onboard Payable/Deduction", "Currency" };

                DataTable dt = PhoenixCrewManagement.CrewContractCBACompnent(new Guid(Request.QueryString["Contractid"].ToString()));
                General.ShowExcel("CBA", dt, alColumns, alCaptions, null, null);
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
        string[] alColumns = { "FLDCOMPONENTNAME", "FLDINCLUDECONTEARDESC", "FLDINCLUDECONTDEDDESC", "FLDCALUNITBASISNAME", "FLDCALTIMEBASISNAME", "FLDONBPAYDEDNAME", "FLDCURRENCYCODE" };
        string[] alCaptions = { "Component", "Earnings", "Deductions", "Calculation Unit", "Calculation Unit", "Onboard Payable/Deduction", "Currency" };
        DataTable dt = PhoenixCrewManagement.CrewContractCBACompnent(new Guid(Request.QueryString["Contractid"].ToString()));
        if (dt.Rows.Count > 0)
        {
            gvCBA.DataSource = dt;
            gvCBA.DataBind();
        }
        else
            ShowNoRecordsFound(dt, gvCBA);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvCBA", "CBA", alCaptions, alColumns, ds);
    }
    protected void gvCBA_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;
            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);
            HeaderCell = new TableCell();
            HeaderCell.Text = "Included in Contractual";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 2;
            HeaderGridRow.Cells.Add(HeaderCell);
            HeaderCell = new TableCell();
            HeaderCell.Text = "Calculation Basis";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 2;
            HeaderGridRow.Cells.Add(HeaderCell);
            HeaderCell = new TableCell();
            HeaderCell.ColumnSpan = 2;
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderGridRow.Cells.Add(HeaderCell);
            gvCBA.Controls[0].Controls.AddAt(0, HeaderGridRow);
            GridViewRow row1 = ((GridViewRow)gvCBA.Controls[0].Controls[0]);
            row1.Attributes.Add("style", "position:static");
        }
    }
    private void EditContractDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EditCrewContractDetails(new Guid(Request.QueryString["Contractid"].ToString()));
            if (dt.Rows.Count > 0)
            {
                ViewState["EMPLOYEEID"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                txtunion.Text = dt.Rows[0]["FLDCBAAPPLIED"].ToString();
                txtCBARevision.Text = dt.Rows[0]["FLDREVISIONNODESC"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddlename.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                ViewState["ADDRESSCODE"] = dt.Rows[0]["FLDADDRESSCODE"].ToString();
                ViewState["REVISIONNO"] = dt.Rows[0]["FLDREVISIONNO"].ToString();
                txtDate.Text = dt.Rows[0]["FLDPAYDATE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewContract_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("CONTRACTINFORMATION"))
            {
                Response.Redirect("CrewContractPersonal.aspx?Contractid=" + Request.QueryString["Contractid"].ToString() + "&empid = " + Request.QueryString["empid"], false);
            }
            if (dce.CommandName.ToUpper().Equals("CBACOMPONENT"))
            {
                Response.Redirect("CrewContractCBAdetails.aspx?Contractid=" + Request.QueryString["Contractid"].ToString() + "&empid = " + Request.QueryString["empid"], false);
            }
            if (dce.CommandName.ToUpper().Equals("STANDARDCOMPONENT"))
            {
                Response.Redirect("CrewContractStandardComponent.aspx?Contractid=" + Request.QueryString["Contractid"].ToString() + "&empid = " + Request.QueryString["empid"], false);
            }
            if (dce.CommandName.ToUpper().Equals("CREWAGREECOMPONENT"))
            {
                Response.Redirect("CrewContractAgreedComponent.aspx?Contractid=" + Request.QueryString["Contractid"].ToString() + "&empid = " + Request.QueryString["empid"], false);
            }
            if (dce.CommandName.ToUpper().Equals("CONTRACT"))
            {
                Response.Redirect("CrewContractDetails.aspx?Contractid=" + Request.QueryString["Contractid"].ToString() + "&empid = " + Request.QueryString["empid"] , false);
            }
            if (dce.CommandName.ToUpper().Equals("LIST"))
            {
                if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                    Response.Redirect("../Crew/CrewContractHistory.aspx?empid=" + Request.QueryString["empid"].ToString(), false);
                else
                    Response.Redirect("../Crew/CrewContractHistory.aspx", false);

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
