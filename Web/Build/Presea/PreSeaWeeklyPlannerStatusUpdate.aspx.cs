using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Drawing;
using SouthNests.Phoenix.PreSea;
using System.Web;

public partial class PreSeaWeeklyPlannerStatusUpdate : PhoenixBasePage
{
    private DataSet dsGrid = new DataSet();

    #region :   Page load and render Events :

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
            SessionUtil.PageAccessRights(this.ViewState);


            if (!IsPostBack)
            {
                ViewState["BATCH"] = "";
                ViewState["WEEKID"] = "";
                ViewState["DAYID"] = "";
                ViewState["SECTIONID"] = "";

                ucSemester.Enabled = false;
                ucWeek.Enabled = false;

                PhoenixToolbar toolbarPlannerList = new PhoenixToolbar();
                toolbarPlannerList.AddImageButton("../PreSea/PreSeaWeeklyPlannerStatusUpdate.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbarPlannerList.AddImageButton("../PreSea/PreSeaWeeklyPlannerStatusUpdate.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
                MenuPreSeaWeekPlanner.AccessRights = this.ViewState;
                MenuPreSeaWeekPlanner.MenuList = toolbarPlannerList.Show();

                ListItem li = new ListItem("--Select--", "DUMMY");
                ddlSection.Items.Add(li);
                ddlSection.DataBind();
            }
            if (Request.Params.Get("__EVENTTARGET") != null)
            {
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

    #region :   Methods :

    private void BindWeeklyPlan()
    {

        try
        {
            gvPreseaWeeklyPlanner.Columns.Clear();

            dsGrid = PhoenixPreSeaWeeklyPlanner.ListTimeTableDetails(General.GetNullableInteger(ViewState["BATCH"].ToString())
                                                                        , General.GetNullableInteger(ViewState["WEEKID"].ToString())
                                                                        , General.GetNullableInteger(ViewState["SECTIONID"].ToString()));

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
                            gvPreseaWeeklyPlanner.Columns.Add(field);
                        }
                    }

                    gvPreseaWeeklyPlanner.DataSource = dsGrid.Tables[1];
                    gvPreseaWeeklyPlanner.DataBind();
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
                            gvPreseaWeeklyPlanner.Columns.Add(field);
                        }
                    }

                    ShowNoRecordsFound(dsGrid.Tables[0], gvPreseaWeeklyPlanner);
                }
            }
            else
            {
                ShowNoRecordsFound(dsGrid.Tables[0], gvPreseaWeeklyPlanner);
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

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void BindSection()
    {
        ddlSection.Items.Clear();

        DataSet ds = PhoenixPreSeaTrainee.ListPreSeaTraineeSection(General.GetNullableInteger(ucBatch.SelectedBatch));
        if (ds.Tables.Count > 0)
        {
            ListItem li = new ListItem("--Select--", "DUMMY");

            ddlSection.Items.Add(li);
            ddlSection.DataBind();

            ddlSection.DataSource = ds.Tables[0];
            ddlSection.DataBind();
        }
    }

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

    private void PrepareControlForExport(Control control)
    {

        for (int i = 0; i < control.Controls.Count; i++)
        {
            Control current = control.Controls[i];
            if (current is TableCell || current is Literal || current is LiteralControl)
            {
                PrepareControlForExport(current);
            }
            else
            {
                control.Controls.Remove(current);
            }
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

                    // Get Current date day id
                    if (General.GetNullableInteger(ddlSection.SelectedValue).HasValue)
                    {
                        ViewState["DAYID"] = "";
                        DataTable dtDay = PhoenixPreSeaWeeklyPlanner.ListDayEntry(General.GetNullableInteger(ucBatch.SelectedBatch)
                                                                , General.GetNullableInteger(dt.Rows[0]["FLDSEMESTERID"].ToString())
                                                                , General.GetNullableInteger(ddlSection.SelectedValue)
                                                                , General.GetNullableInteger(dt.Rows[0]["FLDWEEKID"].ToString())
                                                                , General.GetNullableDateTime(DateTime.Today.Date.ToShortDateString()));
                        if (dtDay.Rows.Count > 0)
                            ViewState["DAYID"] = dtDay.Rows[0]["FLDDAYID"].ToString();
                    }
                }
            }

            BindSection();
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

    protected void ddlSection_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ddlSection.SelectedValue).HasValue)
        {
            ViewState["SECTIONID"] = ddlSection.SelectedValue;
            BindWeeklyPlan();
        }
        BindWeeklyPlan();
    }

    # endregion

    # region :  Tabstrip Events :

    protected void MenuPreSeaWeekPlanner_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                BindWeeklyPlan();

                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=PlannerList.xls");
                Response.Charset = "";
                if (dsGrid.Tables.Count > 3 && dsGrid.Tables[3].Rows.Count > 0)
                {
                    string Header = "<table>";
                    Header += "<tr><td colspan='2' rowspan='5'><img width='99' height='99' src='http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Session["images"] + "/sims.png" + "' /></td>";
                    Header += "<td colspan='6' align='center'  style='font-weight:bold;'>Samundra Institute of Maritime Studies, Lonavla</td></tr>";
                    Header += "<tr><td colspan='6'  align='center'  style='font-weight:bold;'>";
                    Header += dsGrid.Tables[3].Rows[0]["FLDBATCH"].ToString();
                    Header += "</td></tr>";
                    Header += "<tr><td colspan='6'  align='center'  style='font-weight:bold;'>";
                    Header += dsGrid.Tables[3].Rows[0]["FLDSEMESTERNAME"].ToString();
                    Header += "</td></tr>";
                    Header += "<tr><td colspan='6'  align='center'  style='font-weight:bold;'>";
                    Header += dsGrid.Tables[3].Rows[0]["FLDSECTIONNAME"].ToString();
                    Header += "</td></tr>";
                    Header += "<tr><td colspan='6'  align='center'  style='font-weight:bold;'>";
                    Header += dsGrid.Tables[3].Rows[0]["FLDWEEKPERIOD"].ToString();
                    Header += "</td></tr><tr><td colspan='8'>&nbsp;</td></tr>";
                    Header += "</table>";
                    Response.Write(Header);
                }
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();

                HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
                Table table = new Table();
                table.CellPadding = 3;
                table.CellSpacing = 2;
                table.GridLines = GridLines.Both;
                if (gvPreseaWeeklyPlanner.HeaderRow != null)
                {
                    GridViewRow HdrRow = gvPreseaWeeklyPlanner.HeaderRow;

                    TableCell HdrCell = new TableCell();
                    HdrCell.Text = "SIGN";
                    HdrCell.Font.Bold = true;
                    HdrRow.Cells.Add(HdrCell);

                    PrepareControlForExport(HdrRow);
                    table.Rows.Add(HdrRow);
                }
                foreach (GridViewRow row in gvPreseaWeeklyPlanner.Rows)
                {
                    TableCell cell = new TableCell();
                    cell.Text = "";
                    cell.Font.Bold = true;
                    row.Cells.Add(cell);

                    PrepareControlForExport(row);
                    table.Rows.Add(row);
                }

                TableRow tr = new TableRow();
                table.Rows.Add(tr);

                TableRow FooterRow = new TableRow();
                TableCell Ftrcell = new TableCell();
                Ftrcell.ColumnSpan = 8;
                Ftrcell.Text = dsGrid.Tables[4].Rows[0][0].ToString();
                FooterRow.Cells.Add(Ftrcell);

                table.Rows.Add(FooterRow);

                table.RenderControl(htmlwriter);
                Response.Write(stringwriter.ToString());
                Response.End();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ucWeek.SelectedWeek = "";
                ucSemester.SelectedSemester = "";
                ddlSection.SelectedValue = "DUMMY";
                ucBatch.SelectedBatch = "";

                ViewState["BATCH"] = "";
                ViewState["WEEKID"] = "";
                ViewState["SECTIONID"] = "";

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

    # region :  Other Control Events :

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindWeeklyPlan();
    }

    protected void PrevClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = PhoenixPreSeaWeeklyPlanner.GetPrevNextWeekofBatch(Convert.ToInt32(ucBatch.SelectedBatch), Convert.ToInt32(ucSemester.SelectedSemester), Convert.ToInt32(ViewState["WEEKID"].ToString()), -1);
            if (dt.Rows.Count > 0)
            {
                ViewState["WEEKID"] = dt.Rows[0]["FLDWEEKID"].ToString();
            }

            BindWeeklyPlan();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void NextClick(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = PhoenixPreSeaWeeklyPlanner.GetPrevNextWeekofBatch(Convert.ToInt32(ucBatch.SelectedBatch), Convert.ToInt32(ucSemester.SelectedSemester), Convert.ToInt32(ViewState["WEEKID"].ToString()), 1);
            if (dt.Rows.Count > 0)
            {
                ViewState["WEEKID"] = dt.Rows[0]["FLDWEEKID"].ToString();
            }
            BindWeeklyPlan();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    #endregion

    #region :   Grid Events :

    protected void gvPreseaWeeklyPlanner_RowDataBound(object sender, GridViewRowEventArgs e)
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

                    e.Row.Cells[0].HorizontalAlign = HorizontalAlign.Center;
                    e.Row.Cells[0].Style.Add("font-weight", "bold");
                    e.Row.Cells[0].Text = timeslots.Rows[e.Row.RowIndex]["FLDHOURSLOTS"].ToString();

                    for (int i = 0; i < e.Row.Cells.Count; i++)
                    {
                        if (i > 0)
                        {

                            Literal ltrl = new Literal();
                            CheckBox ChkDone = new CheckBox();
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
                                        ChkDone.ID = dr[0]["FLDHOURID"].ToString();
                                        ChkDone.AutoPostBack = true;
                                        ChkDone.CheckedChanged += new EventHandler(ChkDone_CheckedChanged);

                                        if (!String.IsNullOrEmpty(dr[0]["FLDSUBJECT"].ToString()))
                                            ltrl.Text = dr[0]["FLDTHEORYDATA"].ToString();
                                        else if (!String.IsNullOrEmpty(dr[0]["FLDPRACTICALDETAILS"].ToString()))
                                        {
                                            if (dr[0]["FLDPRACTICALDETAILS"].ToString().Contains("- ()"))
                                                ltrl.Text = dr[0]["FLDPRACTICALDETAILS"].ToString().Replace("- ()", "");
                                            else
                                                ltrl.Text = dr[0]["FLDPRACTICALDETAILS"].ToString();
                                        }
                                        else
                                            ltrl.Text = dr[0]["FLDBREAKDETAILS"].ToString();

                                        lblIsBreak.Text = dr[0]["FLDISBREAKHOURS"].ToString();
                                        lblIsBreak.Visible = false;

                                        ltrl.Text += "<br/>";
                                        e.Row.Cells[i].HorizontalAlign = HorizontalAlign.Center;
                                        e.Row.Cells[i].Style.Add("font-weight", "bold");

                                        e.Row.Cells[i].Controls.Add(lblIsBreak);
                                        e.Row.Cells[i].Controls.Add(ltrl);

                                        if ((!String.IsNullOrEmpty(dr[0]["FLDTHEORYDATA"].ToString()) || !String.IsNullOrEmpty(dr[0]["FLDPRACTICALDETAILS"].ToString())) && Convert.ToDateTime(timeslots.Rows[e.Row.RowIndex]["FLDDAY" + i.ToString()]) <= DateTime.Today.Date) //String.IsNullOrEmpty(dr[0]["FLDBREAKDETAILS"].ToString()) &&
                                        {
                                            ChkDone.Checked = dr[0]["FLDISCLASSCONDUCTED"].ToString().Equals("1");
                                            e.Row.Cells[i].Controls.Add(ChkDone);
                                        }
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

    protected void gvPreseaWeeklyPlanner_DataBound(object sender, EventArgs e)
    {
        try
        {

            foreach (GridViewRow gvRow in gvPreseaWeeklyPlanner.Rows)
            {

                string CellText = ""; bool AllCellSame = false;
                for (int cellCount = 1; cellCount < 7; cellCount++)
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

    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void ChkDone_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        string hourid = chk.ID;

        if (General.GetNullableGuid(hourid).HasValue)
        {
            PhoenixPreSeaWeeklyPlanner.UpdateHourlyEntryStatus(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(hourid), General.GetNullableByte(chk.Checked ? "1" : "0"));
        }

    }

    #endregion

}
