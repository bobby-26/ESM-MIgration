using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using System.Collections.Specialized;

public partial class PreSeaEntranceExamCandidates : PhoenixBasePage
{
    string batchid = string.Empty, venue = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");


            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddImageButton("../PreSea/PreSeaEntranceExamCandidates.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbarsub.AddImageButton("../PreSea/PreSeaEntranceExamCandidates.aspx", "Filter", "search.png", "FIND");
            toolbarsub.AddImageButton("../PreSea/PreSeaEntranceExamCandidates.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            PreSeaGridMenu.AccessRights = this.ViewState;
            PreSeaGridMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                if (Request.QueryString["batchid"] != null)
                {
                    batchid = Request.QueryString["batchid"];
                    ucBatch.SelectedBatch = Request.QueryString["batchid"];
                }
                if (Request.QueryString["venue"] != null)
                {
                    venue = Request.QueryString["venue"];
                    ucExamVenue.SelectedExamVenue = Request.QueryString["venue"];
                }
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
            gvPreSeaSearch.SelectedIndex = -1;
            gvPreSeaSearch.EditIndex = -1;
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
            gv.Rows[0].Attributes["onclick"] = "";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PreSeaGridMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                if (!General.GetNullableInteger(ucExamVenue.SelectedExamVenue).HasValue)
                {
                    ucError.ErrorMessage = "Select Exam Venue for filter the candidates";
                    ucError.Visible = true;
                    return;
                }

                ShowExcel();
            }
            else if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                if (!General.GetNullableInteger(ucBatch.SelectedBatch).HasValue)
                {
                    ucError.ErrorMessage = "Select Applied batch for filter the candidates";
                    ucError.Visible = true;
                }
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucExamVenue", ucExamVenue.SelectedExamVenue);
                criteria.Add("ucBatch", ucBatch.SelectedBatch);
                criteria.Add("txtName", txtName.Text);
                criteria.Add("ucDOB", ucDOB.Text);
                criteria.Add("txtRollNumber", txtRollNumber.Text);
                Filter.PreSeaIndividualScoreCardFilter = criteria;
                BindData();
                SetPageNavigator();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ucExamVenue.SelectedExamVenue = "";
                ucBatch.SelectedBatch = "";
                ucDOB.Text = "";
                txtName.Text = "";
                txtRollNumber.Text = "";
                Filter.PreSeaIndividualScoreCardFilter = null;
                BindData();
                SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreSeaSearch_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvPreSeaSearch_RowDataBound(object sender, GridViewRowEventArgs e)
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
            ImageButton ed = (ImageButton)e.Row.FindControl("imgReceipt");
            if (ed != null)
            {
                Label lblEmployeeid = (Label)e.Row.FindControl("lblEmployeeid");
                Label lblBatchId = (Label)e.Row.FindControl("lblBatchId");
                Label lblinterviewid = (Label)e.Row.FindControl("lblinterviewid");

                if (lblinterviewid.Text == "")
                {
                    ed.Visible = false;
                }
                else
                {
                    ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
                    ed.Attributes.Add("onclick", "Openpopup('PDForm', '', '../Presea/PreSeaCandidateFees.aspx?empid=" + lblEmployeeid.Text + "&batchid=" + lblBatchId.Text + "&interviewid=" + lblinterviewid.Text + "');return false;");
                }
            }


        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    protected void gvPreSeaSearch_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "SORT")
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = int.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                Label lblEmployeeid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeid");
                Label lblBatchId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchId");
                Label lblExamPlanId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblExamPlanId");
                Label lblinterviewid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblinterviewid");
                Label lblscorecardid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblscorecardid");

                string url = "../PreSea/PreSeaEntranceExamDetails.aspx";

                if (lblEmployeeid != null && lblEmployeeid.Text != String.Empty)
                    url += "?candidateId=" + lblEmployeeid.Text;
                if (lblBatchId != null && lblBatchId.Text != String.Empty)
                    url += "&batch=" + lblBatchId.Text;
                if (lblExamPlanId != null && lblExamPlanId.Text != String.Empty)
                    url += "&examplanid=" + lblExamPlanId.Text;
                if (lblinterviewid != null && lblinterviewid.Text != String.Empty)
                    url += "&interviewid=" + lblinterviewid.Text;
                if (lblscorecardid != null && lblscorecardid.Text != String.Empty)
                    url += "&scorecard=" + lblscorecardid.Text;

                Response.Redirect(url, false);
            }

            //if (e.CommandName.ToString().ToUpper().Equals("RECEIPT"))
            //{
            //    Label lblEmployeeid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeid");
            //    Label lblBatchId = (Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchId");
            //    Label lblinterviewid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblinterviewid");



            //}
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
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        NameValueCollection nvc = Filter.PreSeaIndividualScoreCardFilter;

        try
        {

            DataTable dt = PhoenixPreSeaNewApplicantManagement.PreSeaEntranceCandidatesSearch(nvc !=null ? General.GetNullableInteger(nvc.Get("ucExamVenue")):null
                                                                                               ,nvc != null ? General.GetNullableInteger(nvc.Get("ucBatch")): null
                                                                                               ,nvc != null ? General.GetNullableString(nvc.Get("txtName")) : null 
                                                                                               ,nvc != null ? General.GetNullableDateTime(nvc.Get("ucDOB")): null
                                                                                               ,nvc != null ? General.GetNullableInteger(nvc.Get("txtRollNumber")) : null
                                                                                               , sortexpression, sortdirection
                                                                                               , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                                                                                               , ref iRowCount, ref iTotalPageCount);

            if (dt.Rows.Count > 0)
            {
                gvPreSeaSearch.DataSource = dt;
                gvPreSeaSearch.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvPreSeaSearch);
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

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDFIRSTNAME", "FLDMIDDLENAME", "FLDLASTNAME", "FLDDATEOFBIRTH", "FLDCOURSENAME", "FLDBATCHNAME", "FLDEXAMVENUENAME" };
            string[] alCaptions = { "First Name", "Middle Name", "Last Name", "Date of Birth", "Course Name", "Batch Name", "Called Venue" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.PreSeaIndividualScoreCardFilter;

            DataTable dt = PhoenixPreSeaNewApplicantManagement.PreSeaEntranceCandidatesSearch(nvc != null ? General.GetNullableInteger(nvc.Get("ucExamVenue")) : null
                                                                                               , nvc != null ? General.GetNullableInteger(nvc.Get("ucBatch")) : null
                                                                                               , nvc != null ? General.GetNullableString(nvc.Get("txtName")) : null
                                                                                               , nvc != null ? General.GetNullableDateTime(nvc.Get("ucDOB")) : null
                                                                                               , nvc != null ? General.GetNullableInteger(nvc.Get("txtRollNumber")) : null
                                                                                                   , sortexpression, sortdirection
                                                                                                   , (int)ViewState["PAGENUMBER"], iRowCount
                                                                                                   , ref iRowCount, ref iTotalPageCount);

            Response.AddHeader("Content-Disposition", "attachment; filename=PersonnelMaster.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Personnel Master</h3></td>");
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
