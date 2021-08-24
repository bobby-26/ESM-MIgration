using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Registers;

public partial class PreSeaBatchExamSecheduleSubjects : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["EXAMSCHEDULEID"] = "";

                if (Request.QueryString["scheduleid"] != null)
                    ViewState["EXAMSCHEDULEID"] = Request.QueryString["scheduleid"].ToString();

                SessionUtil.PageAccessRights(this.ViewState);
                PhoenixToolbar toolbar = new PhoenixToolbar();
                //toolbar.AddImageButton("../PreSea/PreSeaBatchExamSecheduleSubjects.aspx", "Export to Excel", "icon_xls.png", "Excel");
                //toolbar.AddImageLink("javascript:CallPrint('gvPreSeaExam')", "Print Grid", "icon_print.png", "PRINT");
                //MenuPreSeaExam.AccessRights = this.ViewState;
                //MenuPreSeaExam.MenuList = toolbar.Show();

                BindData();               
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {

        DataTable dt = new DataTable();

        string[] alColumns = { "FLDSUBJECTNAME", "FLDTYPE", "FLDMAXMARKS", "FLDPASSMARKS", "FLDISACTIVEYN" };
        string[] alCaptions = { "Subject", "Type", "Max. marks", "Pass Marks", "Active YN" };
            
        dt = PhoenixPreSeaBatchExamSchedule.PreSeaBatchExamScheduleSubjectsList(General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString()));

        Response.AddHeader("Content-Disposition", "attachment; filename=DesignationinvoiceStatus.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Pre Sea Batch Exam Subjetcs</h3></td>");
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
        foreach (DataRow dr in dt.Rows)
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
    

    private void BindData()
    {

        string[] alColumns = { "FLDSUBJECTNAME", "FLDTYPE", "FLDMAXMARKS", "FLDPASSMARKS", "FLDISACTIVEYN"};
        string[] alCaptions = {"Subject","Type","Max. marks","Pass Marks","Active YN"};

        DataTable dt = new DataTable();
        dt = PhoenixPreSeaBatchExamSchedule.PreSeaBatchExamScheduleSubjectsList(General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString()));

        DataSet ds = new DataSet();
        ds.Tables.Add(dt.Copy());

        if (dt.Rows.Count > 0)
        {
            gvPreSeaExam.DataSource = dt;
            gvPreSeaExam.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, gvPreSeaExam);
        }

        General.SetPrintOptions("gvPreSeaExam", "Presea Batch Exam", alCaptions, alColumns, ds);
    }

    protected void PreSeaExam_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                gvPreSeaExam.EditIndex = -1;
                gvPreSeaExam.SelectedIndex = -1;
                BindData();
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    private void UpdatePreSeaExamScheduleSubjects(Guid? examsubjectid, decimal? maxmarks, decimal? passmarks)
    {
        PhoenixPreSeaBatchExamSchedule.UpdatePreSeaBatchExamScheduleSubjects(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString())
            , examsubjectid
            , maxmarks
            , passmarks
            );
    }

    private void DeletePreSeaExamScheduleSubjects(Guid? examsubjectid)
    {
        PhoenixPreSeaBatchExamSchedule.DeletePreSeaBatchExamScheduleSubjects(PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , General.GetNullableGuid(ViewState["EXAMSCHEDULEID"].ToString())
            , examsubjectid
            );
    }    

    protected void gvPreSeaExam_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();
    }

    protected void gvPreSeaExam_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string examsubjectid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblExamSubjectId")).Text;
                string maxmark = ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtMaxMarkEdit")).Text;
                string passmark = ((UserControlNumber)_gridView.Rows[nCurrentRow].FindControl("txtPassMarkEdit")).Text;

                if (!IsValidPreSeaBatchExamSubjects(maxmark
                     , passmark
                     ))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdatePreSeaExamScheduleSubjects(General.GetNullableGuid(examsubjectid)
                    , General.GetNullableDecimal(maxmark)
                    , General.GetNullableDecimal(passmark)
                    );

                _gridView.EditIndex = -1;
                BindData();                
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string examsubjectid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblExamSubjectId")).Text;
                DeletePreSeaExamScheduleSubjects(General.GetNullableGuid(examsubjectid));

                _gridView.EditIndex = -1;

                BindData();
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreSeaExam_RowDataBound(object sender, GridViewRowEventArgs e)
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

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Row.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Row.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

            }                 
        }        
    }

    protected void gvPreSeaExam_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    protected void gvPreSeaExam_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindData();
    }

    private bool IsValidPreSeaBatchExamSubjects(string maxmark, string passmark)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDecimal(maxmark) == null)
        {
            ucError.ErrorMessage = "Max mark is required.";
        }
        if (General.GetNullableDecimal(passmark) == null)
        {
            ucError.ErrorMessage = "Pass mark is required.";
        }
        else if (General.GetNullableDecimal(maxmark) < General.GetNullableDecimal(passmark))
        {
            ucError.ErrorMessage = "Pass mark should be less than or equal to max mark.";
        }

        return (!ucError.IsError);
    }

    protected void gvPreSeaExam_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }    

    protected void ddlBatchSemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ListItem li = new ListItem("--Select--", "DUMMY");

            GridView _gridView = gvPreSeaExam;

            DropDownList ddlBatchSemester = (DropDownList)_gridView.FooterRow.FindControl("ddlBatchSemesterAdd");
            DropDownList ddlBatchSubject = (DropDownList)_gridView.FooterRow.FindControl("ddlBatchSubjectsAdd");

            ddlBatchSubject.Items.Clear();
            ddlBatchSubject.Items.Add(li);
            string semesterid = ddlBatchSemester.SelectedValue;

            DataSet dsSub = PhoenixPreSeaBatchManager.ListPreSeaBatchSubject(General.GetNullableInteger(Filter.CurrentPreSeaBatchManagerSelection)
                , General.GetNullableInteger(semesterid));
            DataTable dt = dsSub.Tables[0];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dt.Rows[i];
                li = new ListItem(dr["FLDSUBJECTNAME"].ToString(), dr["FLDSUBJECTID"].ToString());
                ddlBatchSubject.Items.Add(li);
            }
            ddlBatchSubject.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }    
}

