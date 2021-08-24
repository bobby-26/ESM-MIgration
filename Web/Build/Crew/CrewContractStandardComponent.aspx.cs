using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewContractStandardComponent : PhoenixBasePage
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
            MenuCrewContract.SelectedMenuIndex = 2;

            if (!IsPostBack)
                EditContractDetails();
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddImageButton("../Crew/CrewContractStandardComponent.aspx?Contractid=" + Request.QueryString["Contractid"].ToString(), "Export to Excel", "icon_xls.png", "Excel");
            toolbar1.AddImageLink("javascript:CallPrint('gvCrew')", "Print Grid", "icon_print.png", "PRINT");
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
                string[] alColumns = { "FLDCOMPONENTNAME", "FLDCALCULATIONBASISNAME", "FLDPAYABLEBASISNAME" };
                string[] alCaptions = { "Component", "Calculation Basis", "Payable Basis" };
                DataTable dt = PhoenixCrewManagement.CrewContractStandardCompnent(new Guid(Request.QueryString["Contractid"].ToString()));
                General.ShowExcel("Standard", dt, alColumns, alCaptions, null, null);
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
        string[] alColumns = { "FLDCOMPONENTNAME", "FLDCALCULATIONBASISNAME", "FLDPAYABLEBASISNAME" };
        string[] alCaptions = { "Component", "Calculation Basis", "Payable Basis" };
        DataTable dt = PhoenixCrewManagement.CrewContractStandardCompnent(new Guid(Request.QueryString["Contractid"].ToString()));
        if (dt.Rows.Count > 0)
        {
            gvCrew.DataSource = dt;
            gvCrew.DataBind();
        }
        else
            ShowNoRecordsFound(dt, gvCrew);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvCrew", "Standard", alCaptions, alColumns, ds);
    }
    private void EditContractDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EditCrewContractDetails(new Guid(Request.QueryString["Contractid"].ToString()));
       
            if (dt.Rows.Count > 0)
            {
                ViewState["EMPLOYEEID"] = dt.Rows[0]["FLDEMPLOYEEID"].ToString();
                ViewState["STANDARDWAGECODE"] = dt.Rows[0]["FLDSTANDARDWAGECODE"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddlename.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtStandardWageComponents.Text = dt.Rows[0]["FLDSTANDARDWAGECODENAME"].ToString();
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
