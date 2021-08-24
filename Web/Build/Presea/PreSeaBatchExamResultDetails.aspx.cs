using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using System.Web.UI;

public partial class PreSeaBatchExamResultDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            if (!IsPostBack)
            {
                PhoenixToolbar toolbargrid = new PhoenixToolbar();                

                toolbargrid.AddImageButton("../PreSea/PreSeaBatchExamResultDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvExamResults')", "Print Grid", "icon_print.png", "PRINT");
                toolbargrid.AddImageButton("../PreSea/PreSeaBatchExamResultDetails.aspx", "Filter", "search.png", "FIND");
                toolbargrid.AddImageButton("../PreSea/PreSeaBatchExamResultDetails.aspx", "Add", "Add.png", "ADD");

                MenuExamResults.AccessRights = this.ViewState;
                MenuExamResults.MenuList     = toolbargrid.Show();
                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["EXAMSCHEDULEID"] = "";
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

    protected void MenuBatchExam_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("PreSeaBatchExamResultDetails.aspx", false);
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void MenuExamResults_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                SetPageNavigator();
            }
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (dce.CommandName.ToUpper().Equals("ADD"))
            {
                if (General.GetNullableGuid(ucBatchExam.SelectedBatchExam) != null && General.GetNullableInteger(ucBatch.SelectedBatch) != null)
                    Response.Redirect("PreSeaBatchExamResults.aspx?examscheduleid=" + ucBatchExam.SelectedBatchExam + "&batchid=" + ucBatch.SelectedBatch, false);
                else
                {
                    ucError.ErrorMessage = "Plesea select batch exam";
                    ucError.Visible = true;
                }
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDBATCHROLLNUMBER", "FLDNAME", "FLDSEMESTERNAME", "FLDEXAMNAME", "FLDNOOFSUBJECTS", "FLDNOOFSUBPASS", "FLDNOOFABSENT", "FLDSTATUSNAME" };
            string[] alCaptions = { "Roll No.", "Name", "Semester", "Exam Type","Total Subjects", "Subjects Passed","Absent Subjects","Status" };

            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixPreSeaBatchExamResults.PreSeaBatchExamResultDetailsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                ,General.GetNullableInteger(ucBatch.SelectedBatch)
                ,General.GetNullableInteger(ucSemester.SelectedSemester)
                ,General.GetNullableInteger(ucExamType.SelectedExam)
                ,General.GetNullableGuid(ucBatchExam.SelectedBatchExam)
                , sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);
            
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                gvExamResults.Visible = true;

                gvExamResults.DataSource = ds;
                gvExamResults.DataBind();

                if (ViewState["RESULTDETAILID"] == null)
                {
                    ViewState["RESULTDETAILID"] = ds.Tables[0].Rows[0]["FLDRESULTDETAILID"].ToString();                   
                    gvExamResults.SelectedIndex = 0;

                    if (ViewState["RESULTDETAILID"] != null)
                        ViewState["PAGEURL"] = "../PreSea/PreSeaBatchExamResults.aspx";
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvExamResults);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            General.SetPrintOptions("gvExamResults", "Exam Results", alCaptions, alColumns, ds);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }  

    protected void gvExamResults_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
               && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;                
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowState != DataControlRowState.Edit && e.Row.RowState != (DataControlRowState.Alternate | DataControlRowState.Edit))
        {
            LinkButton _doubleClickButton = (LinkButton)e.Row.FindControl("lnkDoubleClick");
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
        }
    }
    protected void gvExamResults_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;

        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
    }
    protected void gvExamResults_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = sender as GridView;
        gvExamResults.SelectedIndex = e.NewSelectedIndex;
        string resultdetailid = ((Label)_gridView.Rows[e.NewSelectedIndex].FindControl("lblResultDetailId")).Text;
        string examscheduleid = ((Label)_gridView.Rows[e.NewSelectedIndex].FindControl("lblExamScheduleId")).Text;
        string studentid = ((Label)_gridView.Rows[e.NewSelectedIndex].FindControl("lblStudentId")).Text;

        ViewState["RESULTDETAILID"] = resultdetailid;

        Response.Redirect("PreSeaBatchExamResults.aspx?examscheduleid=" + examscheduleid + "&resultdetailid=" + resultdetailid+"&addyn=1" + "&studentid=" + studentid, false);
    }

    private void SetRowSelection()
    {
        gvExamResults.SelectedIndex = -1;
        for (int i = 0; i < gvExamResults.Rows.Count; i++)
        {
            if (gvExamResults.DataKeys[i].Value.ToString().Equals(ViewState["REQUESTDETAILID"].ToString()))
            {
                gvExamResults.SelectedIndex = i;
            }
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string[] alColumns = { "FLDBATCHROLLNUMBER", "FLDNAME", "FLDSEMESTERNAME", "FLDEXAMNAME", "FLDNOOFSUBJECTS", "FLDNOOFSUBPASS", "FLDNOOFABSENT", "FLDSTATUSNAME" };
            string[] alCaptions = { "Roll No.", "Name", "Semester", "Exam Type", "Total Subjects", "Subjects Passed", "Absent Subjects", "Status" };
      
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixPreSeaBatchExamResults.PreSeaBatchExamResultDetailsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableInteger(ucBatch.SelectedBatch)
                , General.GetNullableInteger(ucSemester.SelectedSemester)
                , General.GetNullableInteger(ucExamType.SelectedExam)
                , General.GetNullableGuid(ucBatchExam.SelectedBatchExam)
                , sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename= BatchExamResults.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
            Response.Write("<h3><center>Exam Results Details </center></h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucBatch_Changed(object sender, EventArgs e)
    {
        if (General.GetNullableInteger(ucBatch.SelectedBatch) != null)
        {
            ucSemester.Enabled = true;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            dt = PhoenixPreSeaBatchManager.ListBatchSemesters(int.Parse(ucBatch.SelectedBatch)
                , null);
            ds.Tables.Add(dt.Copy());
            ucSemester.SemesterList = ds;
            ucSemester.DataBind();

            BindBatchExam();            
        }
    }
    protected void ucSemester_Changed(object sender, EventArgs e)
    {
        BindBatchExam();
    }
    protected void ucExamType_Changed(object sender, EventArgs e)
    {
        BindBatchExam();
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }
    private void BindBatchExam()
    {
        ucBatchExam.Enabled = true;
         DataTable dt = new DataTable();
         DataSet ds = new DataSet();
         dt = PhoenixPreSeaBatchExamSchedule.ListPreSeaBatchExamSchedule(General.GetNullableInteger(ucBatch.SelectedBatch)
             , General.GetNullableInteger(ucSemester.SelectedSemester)
             , General.GetNullableInteger(ucExamType.SelectedExam)
             , null
             , null);

         ds.Tables.Add(dt.Copy());
         ucBatchExam.BatchExamList = ds;
         ucBatchExam.DataBind();

         ViewState["EXAMSCHEDULEID"] = General.GetNullableGuid(ucBatchExam.SelectedBatchExam);
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        try
        {
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
            gvExamResults.SelectedIndex = -1;
            gvExamResults.EditIndex = -1;
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        SetPageNavigator();
    }    
}
