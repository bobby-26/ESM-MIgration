using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Xml.Linq;

public partial class PreSeaBatchTimeSlot : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar MainToolbar = new PhoenixToolbar();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../PreSea/PreSeaBatchTimeSlot.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvExamDetails')", "Print Grid", "icon_print.png", "PRINT");

            MenuPreSeaExam.AccessRights = this.ViewState;
            MenuPreSeaExam.MenuList = toolbar.Show();
            if (!IsPostBack)
            {   
                ViewState["PAGENUMBER"] = 1;
                ViewState["SEMESTERID"] = "";
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["BATCHID"] = "";

                ddlCourse.SelectedCourse = Filter.CurrentPreSeaCourseMasterSelection;
                ddlCourse.Enabled = false;
               
                if (Request.QueryString["batchid"].ToString() != null && Request.QueryString["batchid"].ToString() != "")
                {
                    ucBatch.SelectedBatch = Request.QueryString["batchid"].ToString();
                    ViewState["BATCHID"] = Request.QueryString["batchid"].ToString();
                    ucBatch.Enabled = false;
                }
            }           
            BindExamDetails();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();


        string[] alColumns = { "FLDROWNUMBER", "FLDSTARTTIME", "FLDENDTIME", "FLDBREAK" };
        string[] alCaptions = { "S.No.", "Start Time", "End Time", "Break" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixPreSeaBatch.SearchPreSeaTimeSlot(PhoenixSecurityContext.CurrentSecurityContext.UserCode
           , General.GetNullableInteger(ucBatch.SelectedBatch)
           , General.GetNullableInteger(ddlSemester.SelectedValue)           
           , sortexpression
           , sortdirection
           , (int)ViewState["PAGENUMBER"]
           , General.ShowRecords(null)
           , ref iRowCount
           , ref iTotalPageCount
           );

        Response.AddHeader("Content-Disposition", "attachment; filename=BatchTimeSlot.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/sims1.png" + "' /></td>");
        Response.Write("<td><h3Batch Time Slot</h3></td>");
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
                ViewState["PAGENUMBER"] = 1;             
               
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

   
    private void clearfilters()
    {
      
        ViewState["PAGENUMBER"] = 1;
       
    }
    private void BindExamDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROWNUMBER", "FLDSTARTTIME", "FLDENDTIME", "FLDBREAK" };
        string[] alCaptions = { "S.No.", "Start Time", "End Time", "Break" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = new DataSet();
        ds = PhoenixPreSeaBatch.SearchPreSeaTimeSlot(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableInteger(ViewState["BATCHID"].ToString())
                , General.GetNullableInteger(ddlSemester.SelectedValue)
                , sortexpression
                , sortdirection
                , (int)ViewState["PAGENUMBER"]
                , General.ShowRecords(null)
                , ref iRowCount
                , ref iTotalPageCount
             );

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvExamDetails.DataSource = ds;
            gvExamDetails.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvExamDetails);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        General.SetPrintOptions("gvExamDetails", "Time Slot", alCaptions, alColumns, ds);
    }

    protected void gvExamDetails_RowDataBound(object sender, GridViewRowEventArgs e)
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
            DataRowView drv = (DataRowView)e.Row.DataItem;

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
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }

    protected void gvExamDetails_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("ADD"))
            {


                string examdate = "01/01/2012";
                string starttime = ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtStartTimeAdd")).Text;
                string endtime = ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtEndTimeAdd")).Text;
                string semesterId = ddlSemester.SelectedValue;
                string examstarttime = (starttime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : starttime;
                string examendtime = (endtime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : endtime;
                string isbrkyn = ((CheckBox)_gridView.FooterRow.FindControl("chkBrkAdd")).Checked ? "1" : "0";

                if (!IsValidTimeSlot(semesterId,examdate
                    , examstarttime
                    , examendtime                   
                    ))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertPreSeaTimeSlot(General.GetNullableInteger(ddlCourse.SelectedCourse)
                , General.GetNullableInteger(ucBatch.SelectedBatch)
                , General.GetNullableInteger(ddlSemester.SelectedValue)
                , General.GetNullableDecimal(examstarttime)
                , General.GetNullableDecimal(examendtime)
                , General.GetNullableByte(isbrkyn));

               
                BindExamDetails();
              
            }

            else if (e.CommandName.ToUpper().Equals("SAVE"))
            {

                string examdate = "01/01/2012";
                string starttime = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtStartTimeEdit")).Text;
                string endtime = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtEndTimeEdit")).Text;
                string semesterId = ddlSemester.SelectedValue;

                string examstarttime = (starttime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : starttime;
                string examendtime = (endtime.Replace("AM", "").Replace("PM", "").Trim() == "__:__") ? string.Empty : endtime;

                string isbrkyn = ((CheckBox)_gridView.Rows[nCurrentRow].FindControl("chkIsBrkEdit")).Checked? "1":"0";

                if (!IsValidTimeSlot(semesterId,examdate
                  , examstarttime
                  , examendtime                 
                  ))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdatePreSeaTimeSlot(General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblTimeSlotId")).Text)
                    , General.GetNullableInteger(ddlCourse.SelectedCourse)
                    , General.GetNullableInteger(ucBatch.SelectedBatch)
                    , General.GetNullableInteger(ddlSemester.SelectedValue)
                    , General.GetNullableDecimal(starttime)
                    , General.GetNullableDecimal(endtime)
                    , General.GetNullableByte(isbrkyn));

                _gridView.EditIndex = -1;
              
                BindExamDetails();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeletePreSeaTimeSlot(int.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblTimeSlotId")).Text));
                _gridView.EditIndex = -1;                
                BindExamDetails();
               
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvExamDetails_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = e.NewEditIndex;
        _gridView.SelectedIndex = e.NewEditIndex;

        BindExamDetails();
    }

    protected void gvExamDetails_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
      
    }

    protected void gvExamDetails_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void gvExamDetails_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {

    }

    private bool IsValidTimeSlot(string semesterId,string examdate, string starttime, string endtime)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (General.GetNullableInteger(semesterId) == null)
            ucError.ErrorMessage = "Semester is required.";        
        if (General.GetNullableString(starttime) == null)
            ucError.ErrorMessage = "Start time is required";
        if (General.GetNullableString(endtime) == null)
            ucError.ErrorMessage = "End time is required";
        else
        {
            if (decimal.Parse(starttime) > 24)
                ucError.ErrorMessage = "Start time is not valid time.";
            if (decimal.Parse(endtime) > 24)
                ucError.ErrorMessage = "End time is not valid time.";
            if (starttime.Split('.').Length == 2)
            {
                string time = starttime.Split('.')[1].Substring(0, starttime.Split('.')[1].Length);
                if(int.Parse(time) >= 60)
                ucError.ErrorMessage = "Start time is not valid time.";
            }
            if (endtime.Split('.').Length == 2)
            {
                string time = endtime.Split('.')[1].Substring(0, endtime.Split('.')[1].Length);
                if (int.Parse(time) >= 60)
                    ucError.ErrorMessage = "End time is not valid time.";
            }
            if((decimal.Parse(starttime)- decimal.Parse(endtime)) > 0)
                ucError.ErrorMessage = "Start time should be earlier than end time";            
                    
        }    

        return (!ucError.IsError);
    }

    private void InsertPreSeaTimeSlot(int? courseID,int? batchid,int? semesterId, decimal? starttime, decimal? endtime, byte? isbrkyn)
    {
        PhoenixPreSeaBatch.InsertPreSeaTimeSlot(PhoenixSecurityContext.CurrentSecurityContext.UserCode, courseID, batchid, semesterId, starttime, endtime,isbrkyn);
    }

    private void UpdatePreSeaTimeSlot(int? TimeSlotId, int? courseID, int? batchId, int? SemesterID, decimal? starttime, decimal? endtime, byte? isbrkyn)
    {
        PhoenixPreSeaBatch.UpdatePreSeaTimeSlot(PhoenixSecurityContext.CurrentSecurityContext.UserCode, TimeSlotId, courseID, batchId, SemesterID, starttime, endtime,isbrkyn);
    }

    private void DeletePreSeaTimeSlot(int TimeSlotId)
    {
        PhoenixPreSeaBatch.DeletePreSeaTimeSlot(PhoenixSecurityContext.CurrentSecurityContext.UserCode, TimeSlotId);
    }
}

