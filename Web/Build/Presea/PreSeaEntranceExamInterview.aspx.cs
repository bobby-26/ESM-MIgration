using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;

public partial class PreSeaEntranceExamInterview : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["CANDIDATEID"] = "";
                ViewState["SCORECARDID"] = "";
                ViewState["EXAMPLANID"] = "";
                ViewState["BATCH"] = "";
                ViewState["INTERVIEWID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (Request.QueryString["candidateid"] != null)
                    ViewState["CANDIDATEID"] = Request.QueryString["candidateid"].ToString();

                //type 0= nill,1=add,2=edit
                if (Request.QueryString["type"] != "0")
                {
                    PhoenixToolbar toolbargrid = new PhoenixToolbar();
                    toolbargrid.AddImageButton("../PreSea/PreSeaEntranceExamDetails.aspx?type=1&candidateid=" + ViewState["CANDIDATEID"].ToString(), "Add Interview", "Add.png", "ADD");


                    MenuPreSea.AccessRights = this.ViewState;
                    MenuPreSea.MenuList = toolbargrid.Show();
                    MenuPreSea.SetTrigger(pnlPreSeaEntranceExam);
                }
                SetPrimaryCandidatesDetails();              
            }
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNAME", "FLDDATEOFBIRTH", "FLDPRESEACOURSENAME", "FLDBATCHNAME", "FLDINTERVIEWDATE" };
            string[] alCaptions = { "Name", "Date of Birth", "Course", "Batch", "Interview Date" };

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

            ds = PhoenixPreSeaEntranceExam.SearchPreSeaEntranceExam(PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , int.Parse(ViewState["CANDIDATEID"].ToString())
               , sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Pre-Sea Entrance Exam", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPreSea_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
            string[] alColumns = { "FLDNAME", "FLDDATEOFBIRTH", "FLDPRESEACOURSENAME", "FLDBATCHNAME", "FLDINTERVIEWDATE" };
            string[] alCaptions = { "Name", "Date of Birth", "Course", "Batch", "Interview Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = new DataSet();

            ds = PhoenixPreSeaEntranceExam.SearchPreSeaEntranceExam(PhoenixSecurityContext.CurrentSecurityContext.UserCode
               , int.Parse(ViewState["CANDIDATEID"].ToString())
               , sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvPreSea", "Pre-Sea Entrance Exam", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvPreSea.DataSource = ds;
                gvPreSea.DataBind();

                if (ViewState["INTERVIEWID"] == null)
                {
                    ViewState["INTERVIEWID"] = ds.Tables[0].Rows[0]["INTERVIEWID"].ToString();

                    gvPreSea.SelectedIndex = 0;

                    if (ViewState["INTERVIEWID"] != null)
                        ViewState["PAGEURL"] = "../PreSea/PreSeaEntranceExamDetail.aspx";
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvPreSea);
            }
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreSea_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
            if (e.CommandName.ToString().ToUpper() == "INTERVIEW")
            {
                string interviewid = _gridView.DataKeys[nCurrentRow].Value.ToString();
                Label candidateid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblCandidateId");
                string type = "2"; //edit

                if (Request.QueryString["type"] != null)
                {
                    if (Request.QueryString["type"].ToString() == "0") // view                  
                        type = "0";
                }
                Response.Redirect("../PreSea/PreSeaEntranceExamDetails.aspx?type=" + type + "&candidateid=" + candidateid.Text + "&interviewid=" + interviewid, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreSea_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["STORETYPEID"] = null;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        ViewState["EXAMVENUEID"] = String.Empty;
        BindData();
    }

    protected void gvPreSea_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        GridView _gridView = sender as GridView;
        gvPreSea.SelectedIndex = e.NewSelectedIndex;
        string interviewid = ((Label)_gridView.Rows[e.NewSelectedIndex].FindControl("lblInterviewId")).Text;
        string candidateid = ((Label)_gridView.Rows[e.NewSelectedIndex].FindControl("lblEmployeeId")).Text;

        ViewState["INTERVIEWID"] = interviewid;

        Response.Redirect("PreSeaEntranceExam.aspx?interviewid=" + interviewid + "&candidateid=" + candidateid, false);
    }

    protected void gvPreSea_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.Header)
            {

            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton att = (ImageButton)e.Row.FindControl("cmdXAtt");
                Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
                Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");
                if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                    + PhoenixModule.PRESEA + "&type=" + PhoenixPreSeaAttachmentType.INTERVIEW + "&cmdname=INTERVIEWUPLOAD'); return false;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

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
            gvPreSea.SelectedIndex = -1;
            gvPreSea.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
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

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void SetRowSelection()
    {
        gvPreSea.SelectedIndex = -1;
        for (int i = 0; i < gvPreSea.Rows.Count; i++)
        {
            if (gvPreSea.DataKeys[i].Value.ToString().Equals(ViewState["FLDINTERVIREWID"].ToString()))
            {
                gvPreSea.SelectedIndex = i;
                ViewState["FLDINTERVIREWID"] = ((Label)gvPreSea.Rows[gvPreSea.SelectedIndex].FindControl("lblnterviewId")).Text;
            }
        }
    }
    private void SetPrimaryCandidatesDetails()
    {
        try
        {
            DataTable dt = PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantList(General.GetNullableInteger(ViewState["CANDIDATEID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtFirstName.Text = dr["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dr["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dr["FLDLASTNAME"].ToString();
                txtCourse.Text = dr["FLDCOURSENAME"].ToString();
                txtBatchApplied.Text = dr["FLDBATCHNAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
