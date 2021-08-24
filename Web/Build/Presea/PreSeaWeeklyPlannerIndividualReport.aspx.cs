using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Framework;



public partial class PreSeaWeeklyPlannerIndividualReport : PhoenixBasePage
{
    private DataSet dsGrid = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {

                ViewState["WEEKID"] = "";
                ViewState["HOURID"] = "";
                ViewState["BATCHID"] = "";
               
                PhoenixToolbar toolbarPlannerList = new PhoenixToolbar();
                toolbarPlannerList.AddImageButton("../PreSea/PreSeaWeeklyPlannerIndividualReport.aspx?", "Export to Excel", "icon_xls.png", "Excel");
                //toolbarPlannerList.AddImageButton("../PreSea/PreSeaWeeklyPlannerIndividualReport.aspx?", "Search", "search.png", "FIND");
                toolbarPlannerList.AddImageButton("../PreSea/PreSeaWeeklyPlannerIndividualReport.aspx?", "Clear Filter", "clear-filter.png", "CLEAR");
                MenuPreSeaWeekPlannerReport.AccessRights = this.ViewState;
                MenuPreSeaWeekPlannerReport.MenuList = toolbarPlannerList.Show();               

                ddlFaculty.Items.Clear();
                ListItem li = new ListItem("--Select--", "DUMMY");
                ddlFaculty.Items.Add(li);
                ddlFaculty.DataBind();     

            }
                BindWeeklyPlan();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    # region : Methods : 
    protected void BindFaculty(DropDownList ddl)
    {
        if (General.GetNullableInteger(ucBatch.SelectedBatch).HasValue)
        {
            ddl.Items.Clear();
            ListItem li = new ListItem("--Select--", "DUMMY");
            ddl.Items.Add(li);

            DataSet ds = PhoenixPreSeaBatchAdmissionContact.ListPreSeaBatchAdmissionContact(Convert.ToInt32(ucBatch.SelectedBatch));

            ddl.DataSource = ds.Tables[0];
            ddl.DataTextField = "FLDCONTACTNAME";
            ddl.DataValueField = "FLDUSERCODE";
            ddl.DataBind();
        }
    }
    private void BindWeeklyPlan()
    {

        try
        {
            gvPreseaWeeklyPlannerReport.Columns.Clear();
            dsGrid = PhoenixPreSeaWeeklyPlannerReport.WeekIndividualTimeTableReport(General.GetNullableInteger(ucBatch.SelectedBatch.ToString())
                                                                , General.GetNullableInteger(ucWeek.SelectedWeek.ToString())
                                                                , General.GetNullableInteger(ddlFaculty.SelectedValue));


            if (dsGrid.Tables.Count > 0 && dsGrid.Tables[0].Rows.Count > 0)
            {
                DataTable dt = dsGrid.Tables[0];
                DataTable dt2 = dsGrid.Tables[1];

                if (dt.Rows.Count > 0 && dt.Rows[0]["FLDWEEKID"].ToString() != "" && dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i != 1)
                        {
                            BoundField field = new BoundField();
                            field.HtmlEncode = false;
                            field.HeaderText = dt.Rows[0][i].ToString();
                            field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                            if (i == 0)
                                field.DataField = "FLDHOURSLOTS";
                            field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                            field.ItemStyle.Wrap = false;
                            gvPreseaWeeklyPlannerReport.Columns.Add(field);
                        }
                    }

                    gvPreseaWeeklyPlannerReport.DataSource = dsGrid.Tables[1];
                    gvPreseaWeeklyPlannerReport.DataBind();
                }
                else
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (i != 1)
                        {
                            BoundField field = new BoundField();
                            field.HtmlEncode = false;
                            field.HeaderText = dt.Rows[0][i].ToString();
                            field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

                            field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                            field.ItemStyle.Wrap = false;
                            gvPreseaWeeklyPlannerReport.Columns.Add(field);
                        }
                    }

                    ShowNoRecordsFound(dsGrid.Tables[0], gvPreseaWeeklyPlannerReport);
                }
            }
            else
            {
                ShowNoRecordsFound(dsGrid.Tables[0], gvPreseaWeeklyPlannerReport);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidWeeklyPlan()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ucBatch.SelectedBatch) == null)
            ucError.ErrorMessage = "Batch is required";

        if (General.GetNullableString(ucSemester.SelectedSemester) == null)
            ucError.ErrorMessage = "Semester is required";

        if (General.GetNullableString(ucWeek.SelectedWeek) == null)
            ucError.ErrorMessage = "Week is required";

        if (General.GetNullableString(ddlFaculty.SelectedValue) == null)
            ucError.ErrorMessage = "Faculty is required";

        return (!ucError.IsError);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        if (dt.Rows.Count <= 0)
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
    protected void ShowExcel()
    {
        BindWeeklyPlan();

        Response.ClearContent();
        Response.ContentType = "application/ms-excel";
        Response.AddHeader("content-disposition", "attachment;filename=WeeklyPlannerIndividualReport.xls");
        Response.Charset = "";
        if (dsGrid.Tables.Count > 1 && dsGrid.Tables[2].Rows.Count > 0)
        {
            string Header = "<table>";
            Header += "<tr><td colspan='2' rowspan='5'><img width='99' height='99' src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/sims.png" + "' /></td>";
            Header += "<td colspan='6' align='center'  style='font-weight:bold;'>Samundra Institute of Maritime Studies, Lonavla</td></tr>";
            Header += "<tr><td colspan='6'  align='center'  style='font-weight:bold;'>";
            Header += dsGrid.Tables[2].Rows[0]["FLDWEEKPERIOD"].ToString();
            Header += "</td></tr>";
            Header += "<tr><td colspan='6'  align='center'  style='font-weight:bold;'>";
            Header += dsGrid.Tables[2].Rows[0]["FLDFACULTYNAME"].ToString();
            Header += "</td></tr><tr><td colspan='6'>&nbsp;</td></tr>";
            Header += "</table>";
            Response.Write(Header);
        }
        System.IO.StringWriter stringwriter = new System.IO.StringWriter();

        HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
        Table table = new Table();
        table.CellPadding = 3;
        table.CellSpacing = 2;
        table.GridLines = GridLines.Both;

        GridViewRow HdrRow;//= new GridViewRow();
        if (gvPreseaWeeklyPlannerReport.HeaderRow != null)
        {
            HdrRow = gvPreseaWeeklyPlannerReport.HeaderRow;
            table.Rows.Add(HdrRow);

            foreach (GridViewRow row in gvPreseaWeeklyPlannerReport.Rows)
            {
                TableCell cell = new TableCell();
                cell.Text = "";
                cell.Font.Bold = true;
                row.Cells.Add(cell);
                table.Rows.Add(row);
            }
            TableRow FooterRow = new TableRow();
            TableCell Ftrcell;
            DataTable dt = dsGrid.Tables[3];
            for (int i = 1; i <= HdrRow.Cells.Count; i++)
            {
                Ftrcell = new TableCell();
                Ftrcell.Text = "";
                if (i == 1)
                {
                    Ftrcell.Text = "Total";
                    if (dt.Rows.Count > 0)
                        Ftrcell.Text = Ftrcell.Text + " [ " + dt.Rows[0]["FLDTOTALHOURS"].ToString() + "]";
                }
                else
                {
                    for (int x = 0; x < dt.Rows.Count; x++)
                    {
                        if (HdrRow.Cells[i - 1].Text.Contains(dt.Rows[x]["FLDDATE"].ToString()))
                        {
                            Ftrcell.Text = dt.Rows[x]["FLDHOURS"].ToString();
                            break;
                        }
                    }
                }
                Ftrcell.Font.Bold = true;
                Ftrcell.HorizontalAlign = HorizontalAlign.Center;
                FooterRow.Cells.Add(Ftrcell);
            }
            table.Rows.Add(FooterRow);
            table.RenderControl(htmlwriter);
            Response.Write(stringwriter.ToString());
            Response.End();
        }
    }
    #endregion

    #region : Tabstrip Events :
    protected void MenuPreSeaWeekPlannerReport_TabStripCommand(object sender, EventArgs e)
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
                ucBatch.SelectedBatch = "";
                ucSemester.SelectedSemester = "";
                ucWeek.SelectedWeek = "";
                ddlFaculty.SelectedValue = "DUMMY";

                BindWeeklyPlan();
            }
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidWeeklyPlan())
                {
                    ucError.Visible = true;
                    //return
                }
                BindWeeklyPlan();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion
  
    # region :  Dropdown Change events  :

    protected void Batch_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucBatch.SelectedBatch).HasValue)
        {
            ViewState["BATCH"] = ucBatch.SelectedBatch;

            ucSemester.Enabled = true;
            ucSemester.Batch = ucBatch.SelectedBatch;
            ucSemester.bind();

            if (String.IsNullOrEmpty(ViewState["WEEKID"].ToString()))
            {
                DataTable dt = PhoenixPreSeaWeeklyPlanner.GetCurrentWeekofBatch(General.GetNullableInteger(ucBatch.SelectedBatch));
                if (dt.Rows.Count > 0)
                {
                    ViewState["WEEKID"] = dt.Rows[0]["FLDWEEKID"].ToString();

                    ucSemester.SelectedSemester = dt.Rows[0]["FLDSEMESTERID"].ToString();

                    ucWeek.Enabled = true;
                    ucWeek.Batch = ucBatch.SelectedBatch;
                    ucWeek.Semester = dt.Rows[0]["FLDSEMESTERID"].ToString();
                    ucWeek.bind();


                    ucWeek.SelectedWeek = dt.Rows[0]["FLDWEEKID"].ToString();                                      
                    
                }
            }          
            BindFaculty(ddlFaculty);
        }
        BindWeeklyPlan();

    }

    protected void Semester_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucSemester.SelectedSemester).HasValue)
        {
            ucWeek.Enabled = true;
            ucWeek.Batch = ucBatch.SelectedBatch;
            ucWeek.Semester = ucSemester.SelectedSemester;
            ucWeek.bind();
        }
        BindWeeklyPlan();
    }

    protected void Week_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucWeek.SelectedWeek).HasValue)
        {
            ViewState["WEEKID"] = ucWeek.SelectedWeek;
        }
       BindWeeklyPlan();
    }

    # endregion

    #region : Grid Events :
    protected void gvPreseaWeeklyPlannerReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        try
        {
            GridView gv = (GridView)sender;

            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.DataRow && gv.DataSource.GetType().Equals(typeof(DataTable)))
            {

                DataTable timeslots = (DataTable)gv.DataSource;
                DataTable header = dsGrid.Tables[0];
                DataTable data = dsGrid.Tables[2];
                if (timeslots.Rows.Count > 0 && timeslots.Columns.Contains("FLDHOURSLOTS"))
                {
                    string holidays = timeslots.Rows[0]["FLDHOLIDAYS"].ToString();

                    Literal ltrlHRs = new Literal();
                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[0].VerticalAlign = VerticalAlign.Middle;
                    e.Row.Cells[0].Style.Add("font-weight", "bold");
                    ltrlHRs.Text = timeslots.Rows[e.Row.RowIndex]["FLDHOURSLOTS"].ToString() + "&nbsp;&nbsp;";

                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        if (i > 0)
                        {

                            Literal ltrl = new Literal();
                            Label lblIsBreak = new Label();

                            ltrl.ID = "ltrlDay" + i.ToString();
                            lblIsBreak.ID = "lblIsBreak" + i.ToString();

                            if (!holidays.Contains(Convert.ToDateTime(timeslots.Rows[e.Row.RowIndex]["FLDDAY" + i.ToString()]).ToShortDateString()))
                            {
                                DataRow[] dr = data.Select("FLDDATE = '" + timeslots.Rows[e.Row.RowIndex]["FLDDAY" + i.ToString()].ToString() + "' AND FLDHOURFROM = '" + timeslots.Rows[e.Row.RowIndex]["FLDHOURFROM"].ToString() + "' AND FLDHOURTO = " + timeslots.Rows[e.Row.RowIndex]["FLDHOURTO"].ToString());
                                if (dr != null && dr.Length > 0)
                                {
                                    if (dr[0]["FLDISHOLIDAY"].ToString() != "1")
                                    {
                                        if (!String.IsNullOrEmpty(dr[0]["FLDSUBJECT"].ToString()))
                                            ltrl.Text = dr[0]["FLDBATCHSECTION"].ToString() + dr[0]["FLDTHEORYDATA"].ToString();
                                        else if (!String.IsNullOrEmpty(dr[0]["FLDPRACTICALDETAILS"].ToString()))
                                            ltrl.Text = dr[0]["FLDBATCHSECTION"].ToString() + dr[0]["FLDPRACTICALDETAILS"].ToString();
                                        else
                                            ltrl.Text = dr[0]["FLDBREAKDETAILS"].ToString();

                                        lblIsBreak.Text = dr[0]["FLDISBREAKHOURS"].ToString();
                                        lblIsBreak.Visible = false;

                                        ltrl.Text += "<br/>";
                                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                                        e.Row.Cells[i].Style.Add("font-weight", "bold");

                                        e.Row.Cells[i].Controls.Add(ltrl);

                                        e.Row.Cells[i].Controls.Add(lblIsBreak);
                                    }

                                }
                            }
                            else if (holidays.Contains(Convert.ToDateTime(timeslots.Rows[e.Row.RowIndex]["FLDDAY" + i.ToString()]).ToShortDateString()))
                            {
                                DataTable dt = PhoenixPreSeaHoliday.ListHoliday(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableDateTime(timeslots.Rows[e.Row.RowIndex]["FLDDAY" + i.ToString()].ToString()));
                                e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                                e.Row.Cells[i].Style.Add("font-weight", "bold");
                                if (dt.Rows.Count > 0)
                                    e.Row.Cells[i].Text = "HOLIDAY" + "<br/>" + dt.Rows[0]["FLDREASON"].ToString();
                                else
                                    e.Row.Cells[i].Text = "HOLIDAY";
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreseaWeeklyPlannerReport_DataBound(object sender, EventArgs e)
    {
        try
        {

            foreach (GridViewRow gvRow in gvPreseaWeeklyPlannerReport.Rows)
            {

                string CellText = ""; bool AllCellSame = false;
                for (int cellCount = 1; cellCount < 6; cellCount++)
                {
                    Label lblIsBreak = (Label)gvRow.Cells[cellCount].FindControl("lblIsBreak" + cellCount.ToString());
                    Literal ltrl = (Literal)gvRow.Cells[cellCount].FindControl("ltrlDay" + cellCount.ToString());

                    if (lblIsBreak != null && lblIsBreak.Text.ToUpper() == "YES")
                    {
                        if (ltrl != null && cellCount == 1)
                            CellText = ltrl.Text;
                        else if (ltrl != null && cellCount > 1)
                        {
                            if (CellText == ltrl.Text)
                            {
                                gvRow.Cells[cellCount].Visible = false;
                                AllCellSame = true;
                            }
                            else
                            {
                                AllCellSame = false;
                            }
                        }
                        else
                        {
                            AllCellSame = false;
                        }
                    }
                }
                if (AllCellSame)
                {
                    gvRow.Cells[1].ColumnSpan = 6;
                    gvRow.Style.Add("background-color", "#CCCCCC");
                    foreach (TableCell cl in gvRow.Cells)
                    {
                        if (cl.Text.Contains("HOLIDAY"))
                            cl.Visible = false;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvPreseaWeeklyPlannerReport_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    #endregion

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }
       
}

