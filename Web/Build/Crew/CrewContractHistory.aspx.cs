using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Text;
using Telerik.Web.UI;
public partial class CrewContractHistory : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
        {
            tblCrew.Visible = true;
            tblvessel.Visible = false;
            Filter.CurrentCrewSelection = Request.QueryString["empid"];
        }
        else
        {    PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Employee wise", "EMP");
            toolbar1.AddButton("Vessel wise", "VESSEL");
            MenuProcedureDetailList.AccessRights = this.ViewState;
            MenuProcedureDetailList.MenuList = toolbar1.Show();
            MenuProcedureDetailList.SelectedMenuIndex = 1;
            tblCrew.Visible = false;
            tblvessel.Visible = true;
        }
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                SetEmployeePrimaryDetails();
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Crew/CrewContractHistory.aspx?" + Request.QueryString["empid"], "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvContractHistory')", "Print Grid", "icon_print.png", "PRINT");
        Menuexport.MenuList = toolbar.Show();
        BindData();
    }
    protected void MenuProcedureDetailList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EMP"))
            {
                Response.Redirect("..\\Crew\\CrewQuickReportContractDetails.aspx", false);
            }
            if (dce.CommandName.ToUpper().Equals("VESSEL"))
            {
                Response.Redirect("..\\Crew\\CrewContractHistory.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {

            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Request.QueryString["empid"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void dllChange(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
    }
    protected void Menuexport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDREFNUMBER", "FLDRANKCODE", "FLDVESSELNAME", "FLDPAYDATE" };
                string[] alCaptions = { "Ref.No.", "Rank", "Vessel", "Pay Commencement" };


                DataTable dt = PhoenixCrewManagement.CrewContractHistory(General.GetNullableInteger(Request.QueryString["empid"]), General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                        , General.GetNullableInteger(ddlRank.SelectedRank)
                                                                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                        , General.ShowRecords(null)
                                                                        , ref iRowCount
                                                                        , ref iTotalPageCount);
                General.ShowExcel("Contract History", dt, alColumns, alCaptions, null, null);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDREFNUMBER", "FLDRANKCODE", "FLDVESSELNAME", "FLDPAYDATE" };
        string[] alCaptions = { "Ref.No.", "Rank", "Vessel", "Pay Commencement" };
        DataTable dt = PhoenixCrewManagement.CrewContractHistory(General.GetNullableInteger(Request.QueryString["empid"]), General.GetNullableInteger(ddlVessel.SelectedVessel)
                                                                        , General.GetNullableInteger(ddlRank.SelectedRank)
                                                                        , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                        , General.ShowRecords(null)
                                                                        , ref iRowCount
                                                                        , ref iTotalPageCount);
        if (dt.Rows.Count > 0)
        {
            gvContractHistory.DataSource = dt;
            gvContractHistory.DataBind();
        }
        else
            ShowNoRecordsFound(dt, gvContractHistory);
        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());
        General.SetPrintOptions("gvContractHistory", "Contract History", alCaptions, alColumns, ds);
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
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
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvContractHistory_RowCommand(object sender, GridViewCommandEventArgs e)
    {

        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToUpper().Equals("CONTRACT"))
        {
            Label ConId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblContractId");
            if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                Response.Redirect("../Crew/CrewContractPersonal.aspx?Contractid=" + ConId.Text + "&empid=" + Request.QueryString["empid"].ToString(), false);
            else
                Response.Redirect("../Crew/CrewContractPersonal.aspx?Contractid=" + ConId.Text, false);
        }
        if (e.CommandName.ToUpper().Equals("DOWNLOADPDF"))
        {
            Label ConId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblContractId");
            Label vesselid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblvesselid");
            Label Paydate = (Label)_gridView.Rows[nCurrentRow].FindControl("lblPaydate"); Label rankid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblrankid");
            Response.Redirect("../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=CREWCONTRACT&contractid=" +
                       (new Guid(ConId.Text)) + "&showmenu=0&empid=" + Request.QueryString["empid"]
                       + "&rnkid=" + rankid.Text + "&vslid=" + vesselid.Text + "&date=" + Paydate.Text + "&history=" + Request.QueryString["history"] + "&accessfrom=1&showword=no&showexcel=no", false);
        }
    }
    protected void gvContractHistory_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Label ConId = (Label)e.Row.FindControl("lblContractId"); Label paydate = (Label)e.Row.FindControl("lblPaydate");
            //Label vesselid = (Label)e.Row.FindControl("lblvesselid");
            //ImageButton con = (ImageButton)e.Row.FindControl("cmdGenContract");
            //if (con != null) con.Visible = SessionUtil.CanAccess(this.ViewState, con.CommandName);
            //if (!string.IsNullOrEmpty(ConId.Text))
            //{
            //    con.Attributes.Add("onclick", "parent.Openpopup('chml', '', '../Crew/CrewContractPersonal.aspx?Contractid=" + ConId.Text + "');return false;");
            //}
            //else
            //{
            //    con.Visible = false;
            //}
        }
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvContractHistory.EditIndex = -1;
        gvContractHistory.SelectedIndex = -1;
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
        gvContractHistory.SelectedIndex = -1;
        gvContractHistory.EditIndex = -1;
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
}