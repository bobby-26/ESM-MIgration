using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewReports;
using System.Text;
using Telerik.Web.UI;
public partial class CrewSignOnOffFlaggedVesselsReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../Crew/CrewSignOnOffFlaggedVesselsReport.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvSignOnOff')", "Print Grid", "icon_print.png", "PRINT");
                toolbar.AddImageButton("../Crew/CrewSignOnOffFlaggedVesselsReport.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
                MenuCrewSignonoffSporeFlagged.AccessRights = this.ViewState;
                MenuCrewSignonoffSporeFlagged.MenuList = toolbar.Show();

                PhoenixToolbar toolbar1 = new PhoenixToolbar();
                toolbar1.AddButton("Show Report", "SHOWREPORT");
                MenuReportsFilter.AccessRights = this.ViewState;
                MenuReportsFilter.MenuList = toolbar1.Show();

                BindVessel(General.GetNullableInteger(ucFlag.SelectedFlag.ToString()));
            }
            
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindVessel(int? flag)
    {    
        lstVessel.Items.Clear();
        lstVessel.Items.Insert(0, "--Select--");  
        lstVessel.AppendDataBoundItems = true;
        lstVessel.DataSource = PhoenixCrewReportsSignOnOffFlagBased.FlagBasedVesselList(flag);
        lstVessel.DataBind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDOFFSIGNERNAME", "FLDOFFSIGNERRANK", "FLDVESSELNAME", "FLDOFFSIGNERSIGNOFFDATE", "FLDOFFSIGNERSIGNOFFPORT", "FLDRELIEVERNAME", "FLDRELATIONSHIP", "FLDADDRESS", "FLDDATEOFBIRTH", "FLDNATIONALITY", "FLDRELIEVERSIGNONDATE", "FLDRELIEVERPORT", "FLDRELIEVERWAGES" };
        string[] alCaptions = { "Off Signer", "Rank", "Vessel", "Sign Off Date", "Sign Off Port", "Joiner", "Relationship", "Address", "Date of Birth", "Nationality", "SignOn Date", "SignOn Port", "Wages" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixCrewReportsSignOnOffFlagBased.CrewSignOnOffFlagBasedVesselReport(
            General.GetNullableInteger(ucFlag.SelectedFlag), General.GetNullableString(VesselSelectedList()), General.GetNullableDateTime(ucFromDate.Text)
           , General.GetNullableDateTime(ucToDate.Text)
           , sortexpression
           , sortdirection
           , 50//(int)ViewState["PAGENUMBER"]
           , General.ShowRecords(null)
           , ref iRowCount
           , ref iTotalPageCount
           );

        General.ShowExcel("Crew SignOnOff List", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void MenuCrewSignonoffSporeFlagged_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ucFromDate.Text = "";
                ucToDate.Text = "";
               
                //ucVessel.SelectedVessel = "";                

                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                BindData();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvSignOnOff_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        Label lblAddress1 = (Label)e.Row.FindControl("lblAddress1");
        UserControlToolTip uct = (UserControlToolTip)e.Row.FindControl("lblAddress");
        if (lblAddress1 != null)
        {
            lblAddress1.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            lblAddress1.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
        }
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
                    && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
                {
                    Label empid ;
                    empid = (Label)e.Row.FindControl("lblOffSignerId");
                    if (empid != null)
                    {
                        LinkButton lbr = (LinkButton)e.Row.FindControl("lnkName");
                        lbr.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewSignOff','','../Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
                    }
                    empid = (Label)e.Row.FindControl("lblOnSignerId");
                    if (empid != null)
                    {
                        LinkButton lnkemp = (LinkButton)e.Row.FindControl("lnkSignOnName");
                        if (lnkemp != null)
                        {
                            lnkemp.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewSignOff','','../Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
                        }
                    }
                }
            }
        }
    }
    protected void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDOFFSIGNERNAME", "FLDOFFSIGNERRANK", "FLDVESSELNAME", "FLDOFFSIGNERSIFNOFFDATE", "FLDOFFSIGNERSIGNOFFPORT", "FLDRELIEVERNAME", "FLDRELATIONSHIP", "FLDADDRESS", "FLDDATEOFBIRTH", "FLDNATIONALITY", "FLDRELIEVERSIGNONDATE", "FLDRELIEVERPORT", "FLDRELIEVERWAGES" };
        string[] alCaptions = { "Off Signer", "Rank", "Vessel", "Sign Off Date", "Sign Off Port", "Joiner","Relationship","Address","Date of Birth","Nationality","SignOn Date","SignOn Port","Wages" };
        int? sortdirection = 1; //DEFAULT DESC SORT
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string sortexpression;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        try
        {
            DataSet ds = PhoenixCrewReportsSignOnOffFlagBased.CrewSignOnOffFlagBasedVesselReport(
                        General.GetNullableInteger(ucFlag.SelectedFlag),General.GetNullableString(VesselSelectedList()), General.GetNullableDateTime(ucFromDate.Text)
                      , General.GetNullableDateTime(ucToDate.Text)  
                      , sortexpression
                      , sortdirection
                      , (int)ViewState["PAGENUMBER"]
                      , 50//General.ShowRecords(null)        
                      , ref iRowCount
                      , ref iTotalPageCount
                      );

            General.SetPrintOptions("gvSignOnOff", "Crew Appraisal", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvSignOnOff.DataSource = ds.Tables[0];
                gvSignOnOff.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvSignOnOff);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetPageNavigator()
    {
        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvSignOnOff.SelectedIndex = -1;
            gvSignOnOff.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
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
    private bool IsValidFilter(string fromdate, string todate)
    {
        ucError.HeaderMessage = "Please provide the following filter criteria";
        DateTime resultdate;

        if (fromdate == null || !DateTime.TryParse(fromdate, out  resultdate))
            ucError.ErrorMessage = "From Date is required";
        if (todate == null || !DateTime.TryParse(todate, out resultdate))
            ucError.ErrorMessage = "To Date is required";
        else if (!string.IsNullOrEmpty(fromdate)
              && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "'To Date' should be later than 'From Date'";
        }
        if (!string.IsNullOrEmpty(fromdate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "'From date' should not be future date";
        }
        if (!string.IsNullOrEmpty(todate)
            && DateTime.Compare(DateTime.Today, DateTime.Parse(todate)) < 0)
        {
            ucError.ErrorMessage = "'To date' should not be future date";
        }
        return (!ucError.IsError);
    }
    private string VesselSelectedList()
    {
        StringBuilder strlist = new StringBuilder();
        foreach (ListItem item in lstVessel.Items)
        {
            if (item.Selected == true)
            {
                if (item.Text == "--Select--")
                    strlist.Append("DUMMY");
                else
                {
                    strlist.Append(item.Value.ToString());
                    strlist.Append(",");
                }
            }

        }
        return strlist.ToString().TrimEnd(',');
    }
    protected void ucFlag_OnTextChangedEvent(object sender,EventArgs args)
    {
        BindVessel(General.GetNullableInteger(ucFlag.SelectedFlag.ToString()));
    }
}
