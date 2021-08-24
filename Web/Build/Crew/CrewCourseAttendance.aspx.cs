using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class CrewCourseAttendance : PhoenixBasePage
{
    string dtkey;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {
                ViewState["row"] = "";
				PhoenixToolbar toolbarAttendanceList = new PhoenixToolbar();
				toolbarAttendanceList.AddImageButton("../Crew/CrewCourseAttendance.aspx?batchid=" + Request.QueryString["batchid"], "Export to Excel", "icon_xls.png", "Excel");
				toolbarAttendanceList.AddImageButton("../Crew/CrewCourseAttendance.aspx?batchid=" + Request.QueryString["batchid"], "Attendance List", "icon_xls.png", "ATTENDENCE");
				MenuGridCourseAttendance.AccessRights = this.ViewState;
				MenuGridCourseAttendance.MenuList = toolbarAttendanceList.Show();
                if (Filter.CurrentCourseSelection != null)
                {
                    EditCourseDetails();
                }
                if (Request.QueryString["batchid"] != null)
                {
                    EditBatchDetails(Convert.ToInt32(Request.QueryString["batchid"]));
                }
                BindAttendanceDate();
            }
            BindAttendanceData();
            PhoenixToolbar toolbarAttendancet = new PhoenixToolbar();
            toolbarAttendancet.AddImageButton("../Crew/CrewCourseAttendance.aspx?batchid=" + Request.QueryString["batchid"], "Export to Excel", "icon_xls.png", "Excel");
            toolbarAttendancet.AddImageButton("javascript:parent.Openpopup('ATT','','../Common/CommonFileAttachment.aspx?dtkey=" + dtkey + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&cmdname=UPLOAD'); return false;", "Attachment", "attachment.png", "ATTACH");
            MenuAttendance.AccessRights = this.ViewState;
           // MenuAttendance.MenuList = toolbarAttendancet.Show();

            BindDataAttendanceList();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void EditCourseDetails()
    {
        try
        {

            int courseid = Convert.ToInt32(Filter.CurrentCourseSelection);
            DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(courseid);
            if (ds.Tables[0].Rows.Count > 0)
            {

                ucCourse.SelectedCourse = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
                ucCourseType.SelectedHard = ds.Tables[0].Rows[0]["FLDDOCUMENTTYPE"].ToString();

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void EditBatchDetails(int batchid)
    {
        try
        {

            DataSet ds = PhoenixRegistersBatch.EditBatch(batchid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtBatchNo.Text = ds.Tables[0].Rows[0]["FLDBATCHNAME"].ToString();

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void BindAttendanceData()
    {
        string date = "";
        string[] alColumns = { "Name", "FLDAM", "FLDPM" };
        string[] alCaptions = { "Employee Name", "AM", "PM" };
        if (ddlAttendanceDate.SelectedItem != null)
        {
            date = ddlAttendanceDate.SelectedItem.Text;
        }

        DataSet ds = PhoenixCrewCourseAttendance.SearchAttendancePerDay(General.GetNullableDateTime(date),
            Convert.ToInt32(Filter.CurrentCourseSelection),
            Convert.ToInt32(Request.QueryString["batchid"]));

        General.SetPrintOptions("gvAttendanceList", "Attendance", alCaptions, alColumns, ds);
	
        if (ds.Tables[0].Rows.Count > 0)
        {
			dtkey = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
            gvAttendanceList.DataSource = ds;
            gvAttendanceList.DataBind();
        }
        else
        {

            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvAttendanceList);
        }

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
 
    }

    protected void gvAttendanceList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            string date = "";
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                if (ddlAttendanceDate.SelectedItem != null)
                {
                    date = ddlAttendanceDate.SelectedItem.Text;
                }

                if (!IsValidAttendance(date))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewCourseAttendance.InsertAttendance(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Convert.ToInt32(Filter.CurrentCourseSelection),
                    General.GetNullableInteger(Request.QueryString["batchid"]),
                    Int64.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeId")).Text),
                   Convert.ToDateTime(ddlAttendanceDate.SelectedItem.Text),
                    (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkAMEdit")).Checked) ? 1 : 0,
                    (((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkPMEdit")).Checked) ? 1 : 0);

                _gridView.EditIndex = -1;
                BindAttendanceData();
                BindDataAttendanceList();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidAttendance(string attendancedate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(attendancedate))
        {
            ucError.ErrorMessage = "Attendance Date is required";
        }


        return (!ucError.IsError);
    }
    protected void gvAttendanceList_RowDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);
            Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
            Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");
            ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (lblIsAtt.Text == string.Empty)
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&cmdname=COURSEATTENDANCE'); return false;");
            }
        }

    }
    protected void ShowExcelAttendancePerDay()
    {
        string[] alColumns = { "Name", "FLDAM", "FLDPM", "FLDSIGNATURE" };
        string[] alCaptions = { "Employee Name", "AM", "PM", "Signature" };


        DataSet ds = PhoenixCrewCourseAttendance.SearchAttendancePerDay(General.GetNullableDateTime(ddlAttendanceDate.SelectedItem.Text),
            Convert.ToInt32(Filter.CurrentCourseSelection),
            Convert.ToInt32(Request.QueryString["batchid"]));

        Response.AddHeader("Content-Disposition", "attachment; filename=AttendancePerDayList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Attendance List</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length + 2).ToString() + "' align='left'><b>Date:" + ddlAttendanceDate.SelectedItem.Text + "</b></td>");
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

    protected void gvAttendanceList_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindAttendanceData();
    }
    protected void gvAttendanceList_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;
        BindAttendanceData();


    }
    protected void MenuAttendance_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvAttendanceList.EditIndex = -1;
                gvAttendanceList.SelectedIndex = -1;
                BindAttendanceData();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                string date = "";
                if (ddlAttendanceDate.SelectedItem != null)
                {
                    date = ddlAttendanceDate.SelectedItem.Text;
                }
                if (!IsValidAttendance(date))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ShowExcelAttendancePerDay();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGridCourseAttendance_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {

                BindDataAttendanceList();
				string[] nonaggcol = { "No", "Rank", "Name", "Vessel", "Post Assessment Remarks" };
				DataSet ds = PhoenixCrewCourseAttendance.ListAttendance(Convert.ToInt32(Convert.ToInt32(Session["COURSEID"] != null ? (Session["COURSEID"].ToString()) : Filter.CurrentCourseSelection)),
							Convert.ToInt32(Request.QueryString["batchid"]));
				 DataTable dt = ds.Tables[1];
				int a= Convert.ToInt32( nonaggcol.Length.ToString()) ;
				int b = Convert.ToInt32(dt.Rows.Count.ToString());
				
                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=AttendanceList.xls");

				Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
				Response.Write("<tr>");
				Response.Write("<td colspan=" + Convert.ToString(a+b) + " align='center'><h3>Candidate's Attendance Sheet</h3></td>");
				Response.Write("</tr>");
				Response.Write("<tr>");
				Response.Write("<td colspan='6'><b>Course : </b>"+ucCourse.SelectedName +" </td>");
				Response.Write("</tr>");
				Response.Write("<tr>");
				Response.Write("<td colspan='3'><b>Batch No :</b>" + txtBatchNo.Text + " </td>");
				Response.Write("</tr>");
				Response.Write("</TABLE>");

                Response.Charset = "";
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                stringwriter.Write("<table><tr><td colspan=\"" + gvAttendance.Columns.Count + "\"></td></tr></table>");
                HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
                gvAttendance.RenderControl(htmlwriter);
                Response.Write(stringwriter.ToString());
                Response.End();
            }
            if (dce.CommandName.ToUpper().Equals("ATTENDENCE"))
            {
                BindDataAttendanceList1();
				string[] nonaggcol = { "No", "Rank", "Name", "Vessel", "Post Assessment Remarks" };
				DataSet ds = PhoenixCrewCourseAttendance.ListAttendance(Convert.ToInt32(Convert.ToInt32(Session["COURSEID"] != null ? (Session["COURSEID"].ToString()) : Filter.CurrentCourseSelection)),
							Convert.ToInt32(Request.QueryString["batchid"]));
				DataTable dt = ds.Tables[1];
				int a = Convert.ToInt32(nonaggcol.Length.ToString());
				int b = Convert.ToInt32(dt.Rows.Count.ToString());

                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=Attendance.xls");
                Response.Charset = "";

				Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
				Response.Write("<tr>");
				Response.Write("<td colspan=" + Convert.ToString(a + b) + " align='center'><h3>Candidate's Attendance Sheet</h3></td>");
				Response.Write("</tr>");
				Response.Write("<tr>");
				Response.Write("<td colspan='6'><b>Course : </b>" + ucCourse.SelectedName + " </td>");
				Response.Write("</tr>");
				Response.Write("<tr>");
				Response.Write("<td colspan='3'><b>Batch No :</b>" + txtBatchNo.Text + " </td>");
				Response.Write("</tr>");
				Response.Write("</TABLE>");

                System.IO.StringWriter stringwriter1 = new System.IO.StringWriter();
                stringwriter1.Write("<table><tr><td colspan=\"" + gvAttendance1.Columns.Count + "\"></td></tr></table>");
                HtmlTextWriter htmlwriter1 = new HtmlTextWriter(stringwriter1);
                gvAttendance1.RenderControl(htmlwriter1);
                Response.Write(stringwriter1.ToString());
                Response.End();
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

    public void BindDataAttendanceList()
    {

        try
        {
            gvAttendance.Columns.Clear();

            DataSet ds = PhoenixCrewCourseAttendance.ListAttendance(Convert.ToInt32(Convert.ToInt32(Session["COURSEID"] != null ? (Session["COURSEID"].ToString()) : Filter.CurrentCourseSelection)),
                Convert.ToInt32(Request.QueryString["batchid"]));

            string[] nonaggcol = { "SR.NO", "RANK", "NAME OF CANDIDATE", "PRE COURSE ASSESSMENT MARKS", "WRITTEN MARKS", "PRACTICAL MARKS", "TOTAL", "P/F", "REMARKS", "CERTIFICATE NO", "Admin Remarks With Sign" };
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < 3; i++)
                {
                    if (nonaggcol[i] == "NAME OF CANDIDATE")
                    {

                        HyperLinkField lnk = new HyperLinkField();
                        lnk.HeaderText = nonaggcol[i];
                        lnk.DataTextField = nonaggcol[i];

                        gvAttendance.Columns.Add(lnk);
                    }
                    else
                    {
                        BoundField field = new BoundField();

                        field.DataField = nonaggcol[i];
                        field.HeaderText = nonaggcol[i];

                        gvAttendance.Columns.Add(field);
                    }
                }
                DataTable dt = ds.Tables[1];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BoundField field = new BoundField();

                    field = new BoundField();
                    field.DataField = (i + 1).ToString();
                    field.HeaderText = dt.Rows[i]["FLDDATE"].ToString();
                    field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    field.ItemStyle.Font.Bold = true;
                    gvAttendance.Columns.Add(field);


                }
                /*Post Assessment Remarks*/
                for (int i = 3; i <= nonaggcol.Length-1; i++)
                {

                    BoundField field = new BoundField();

                    field.DataField = nonaggcol[i];
                    field.HeaderText = nonaggcol[i];
                    field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    gvAttendance.Columns.Add(field);

                }

                DataTable dtrow = ds.Tables[3];
                if (dtrow.Rows.Count > 0)
                {
                    ViewState["row"] = dtrow.Rows[0]["FLDROW"];
                }
                gvAttendance.DataSource = ds;
                gvAttendance.DataBind();

                GridViewRow Toprow = new GridViewRow(0, 2, DataControlRowType.Header, DataControlRowState.Normal);
                Toprow.Attributes.Add("style", "position:static");
                TableCell Topcell = new TableCell();
                Topcell.ColumnSpan = 3 + ds.Tables[1].Rows.Count;
                Topcell.HorizontalAlign = HorizontalAlign.Center;
                Topcell.Text = "FOR CANDIDATES USE ONLY";
                Toprow.Cells.Add(Topcell);
                gvAttendance.Controls[0].Controls.AddAt(0, Toprow);

                Topcell = new TableCell();
                Topcell.ColumnSpan = 6;
                Topcell.HorizontalAlign = HorizontalAlign.Center;
                Topcell.Text = "FOR FACULTY USE ONLY";
                Toprow.Cells.Add(Topcell);
                gvAttendance.Controls[0].Controls.AddAt(0, Toprow);

                Topcell = new TableCell();
                Topcell.ColumnSpan = 2;
                Topcell.HorizontalAlign = HorizontalAlign.Center;
                Topcell.Text = "FOR OFFICE USE ONLY";
                Toprow.Cells.Add(Topcell);
                gvAttendance.Controls[0].Controls.AddAt(0, Toprow);

                GridViewRow row = new GridViewRow(1, 2, DataControlRowType.Header, DataControlRowState.Normal);
                row.Attributes.Add("style", "position:static");
                TableCell cell = new TableCell();
                cell.ColumnSpan = 3;
                row.Cells.Add(cell);



                DataTable dtheader = ds.Tables[2];
                int cnt = 0;
                for (int i = 0; i < dtheader.Rows.Count; i++)
                {
                    cell = new TableCell();
                    cell.ColumnSpan = cnt + 2;
                    cell.HorizontalAlign = HorizontalAlign.Center;
                    cell.Text = dtheader.Rows[i]["FLDDATE"].ToString();
                    row.Cells.Add(cell);
                    gvAttendance.Controls[0].Controls.AddAt(1, row);
                }
                cell = new TableCell();
                //cell.RowSpan = 2;
                cell.HorizontalAlign = HorizontalAlign.Center;
                //cell.Text = "PRE COURSE ASSESSMENT MARKS";
                row.Cells.Add(cell);
                gvAttendance.Controls[0].Controls.AddAt(1, row);

                cell = new TableCell();
                cell.ColumnSpan = 3;
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Text = "POST COURSE ASSESSMENT";
                row.Cells.Add(cell);
                gvAttendance.Controls[0].Controls.AddAt(1, row);

                cell = new TableCell();
                cell.ColumnSpan = 2;
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Text = "";
                row.Cells.Add(cell);
                gvAttendance.Controls[0].Controls.AddAt(1, row);

                cell = new TableCell();
                cell.ColumnSpan = 2;
                cell.HorizontalAlign = HorizontalAlign.Center;
                cell.Text = "";
                row.Cells.Add(cell);
                gvAttendance.Controls[0].Controls.AddAt(1, row);
            }
            else
            {
                ViewState["row"] = "";
                BoundField field = new BoundField();
                field.HeaderText = "";
                gvAttendance.Columns.Add(field);
                DataTable dt = new DataTable();
                ShowNoRecordsFound(dt, gvAttendance);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindDataAttendanceList1()
    {

        try
        {
            gvAttendance1.Columns.Clear();

            DataSet ds = PhoenixCrewCourseAttendance.ListAttendance(Convert.ToInt32(Convert.ToInt32(Session["COURSEID"] != null ? (Session["COURSEID"].ToString()) : Filter.CurrentCourseSelection)),
                Convert.ToInt32(Request.QueryString["batchid"]));

			string[] nonaggcol = { "No", "Rank", "Name", "Vessel","Post Assessment Remarks" };
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < nonaggcol.Length-1; i++)
                {
                    if (nonaggcol[i] == "Name")
                    {

                        HyperLinkField lnk = new HyperLinkField();
                        lnk.HeaderText = nonaggcol[i];
                        lnk.DataTextField = nonaggcol[i];

                        gvAttendance1.Columns.Add(lnk);
                    }
                    else
                    {
                        BoundField field = new BoundField();

                        field.DataField = nonaggcol[i];
                        field.HeaderText = nonaggcol[i];
                        gvAttendance1.Columns.Add(field);
                    }
                }
                DataTable dt = ds.Tables[1];

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BoundField field = new BoundField();

                    field = new BoundField();
                    field.HeaderText = dt.Rows[i]["FLDDATE"].ToString();
                    gvAttendance1.Columns.Add(field);


                }
				for (int i = nonaggcol.Length; i <= nonaggcol.Length; i++)
				{

					BoundField field = new BoundField();
					field.DataField = nonaggcol[i - 1];
					field.HeaderText = nonaggcol[i - 1];
					field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
					gvAttendance1.Columns.Add(field);

				}
                DataTable dtrow = ds.Tables[3];
                if (dtrow.Rows.Count > 0)
                {
                    ViewState["row"] = dtrow.Rows[0]["FLDROW"];
                }
                gvAttendance1.DataSource = ds;
                gvAttendance1.DataBind();
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                row.Attributes.Add("style", "position:static");
                TableCell cell = new TableCell();
                cell.ColumnSpan = 4;
                row.Cells.Add(cell);

                DataTable dtheader = ds.Tables[2];
                int cnt = 0;
                for (int i = 0; i < dtheader.Rows.Count; i++)
                {
                    cell = new TableCell();
                    cell.ColumnSpan = cnt + 2;
                    cell.HorizontalAlign = HorizontalAlign.Center;
                    cell.Text = dtheader.Rows[i]["FLDDATE"].ToString();
                    row.Cells.Add(cell);

                    gvAttendance1.Controls[0].Controls.AddAt(0, row);
                }
            }
            else
            {
                ViewState["row"] = "";
                BoundField field = new BoundField();
                field.HeaderText = "";
                gvAttendance1.Columns.Add(field);
                DataTable dt = new DataTable();
                ShowNoRecordsFound(dt, gvAttendance);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvAttendance_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

			for (int i = 1; i < e.Row.Cells.Count; i++)
            {
				if (e.Row.Cells[i].Text == "1")
                    e.Row.Cells[i].Text = "P";
                else if (e.Row.Cells[i].Text == "0")
                    e.Row.Cells[i].Text = "A";

            }

            if (ViewState["row"].ToString() != "")
            {
				for (int i = 0; i < e.Row.Cells.Count; i++)
                {
                    string[] s = ViewState["row"].ToString().Split(',');
                    for (int k = 0; k < s.Length; k++)
                    {
                        if (e.Row.Cells[3 + Convert.ToInt32(s[k])].Text == "&nbsp;")
                        {
                            e.Row.Cells[3 + Convert.ToInt32(s[k])].Text = "H";
                        }
                    }

                }
            }
        }

    }
    protected void gvAttendance1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (ViewState["row"].ToString() != "")
        {
            for (int i = 0; i < gvAttendance.Columns.Count; i++)
            {
                string[] s = ViewState["row"].ToString().Split(',');
                for (int k = 0; k < s.Length; k++)
                {
                    if (e.Row.Cells[3 + Convert.ToInt32(s[k])].Text == "&nbsp;")
                    {
                        e.Row.Cells[3 + Convert.ToInt32(s[k])].Text = "H";
                    }
                }

            }
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
    protected void MenuCourseAttendance_TabStripCommand(object sender, EventArgs e)
    {
        //try
        //{
        //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        //    if (dce.CommandName.ToUpper().Equals("FINALIZEPB"))
        //    {
        //        if (!IsValidPB(txtClosginDate.Text))
        //        {
        //            ucError.Visible = true;
        //            return;
        //        }
        //        ucConfirm.Visible = true;
        //        ucConfirm.Text = (LastDayOfMonthFromDateTime(DateTime.Parse(txtClosginDate.Text)).ToString("dd/MM/yyyy") == DateTime.Parse(txtClosginDate.Text).ToString("dd/MM/yyyy") ? string.Empty : "MID MONTH CLOSING!! ")
        //            + "Portage Bill will be Locked till " + txtClosginDate.Text + " and no further changes can be made. Please confirm you want to proceed ?";
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
        //BindData();
    }
    protected void ddlAttendanceDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //if (!General.GetNullableDateTime(ddlAttendanceDate.SelectedValue).HasValue)
            //    ddlAttendanceDate.Text = DateTime.Now.ToShortDateString();
            BindAttendanceData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindAttendanceDate()
    {
        try
        {
            DataSet ds = PhoenixCrewCourseAttendance.ListAttendance(Convert.ToInt32(Filter.CurrentCourseSelection),
                Convert.ToInt32(Request.QueryString["batchid"]));
            if (ds.Tables[4].Rows.Count > 0)
            {

                ddlAttendanceDate.DataSource = ds.Tables[4];
                ddlAttendanceDate.DataTextField = "FLDDATE";
				ddlAttendanceDate.DataValueField = "FLDDATE";
                ddlAttendanceDate.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
