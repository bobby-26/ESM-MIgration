using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using SouthNests.Phoenix.PreSea;
using System.Collections.Specialized;

public partial class PreSeaEntranceScoreCardSummary : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                PhoenixToolbar Maintoolbar = new PhoenixToolbar();
                Maintoolbar.AddButton("All", "ALL");
                Maintoolbar.AddButton("Confirmed", "CONFIRMED");
                Maintoolbar.AddButton("Waitlisted", "WAITLISTED");
                Maintoolbar.AddButton("Rejected", "REJECTED");
                MenuPreSeaScoreCradSummary.AccessRights = this.ViewState;
                MenuPreSeaScoreCradSummary.MenuList = Maintoolbar.Show();
                MenuPreSeaScoreCradSummary.SelectedMenuIndex = 0;

                PhoenixToolbar toolbarsub = new PhoenixToolbar();
                toolbarsub.AddImageButton("../PreSea/PreSeaEntranceScoreCardSummary.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
                toolbarsub.AddImageButton("../PreSea/PreSeaEntranceScoreCardSummary.aspx", "Filter", "search.png", "FIND");
                toolbarsub.AddImageButton("../PreSea/PreSeaEntranceScoreCardSummary.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
                toolbarsub.AddImageButton("../PreSea/PreSeaEntranceScoreCardSummary.aspx", "Move to Confirmed", "move-confirm.png", "BULKSELECTION");
                MenuPreSeaGrid.AccessRights = this.ViewState;
                MenuPreSeaGrid.MenuList = toolbarsub.Show();

                if (Filter.PreSeaScoreCardSummaryFilter  != null)
                    SetFilterValues();

            }
            BindData();
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
            if (dce.CommandName.ToUpper().Equals("ALL"))
            {
                Response.Redirect(Request.RawUrl);
            }
            else if (General.GetNullableInteger(ucBatch.SelectedBatch).HasValue)
            {
                if (dce.CommandName.ToUpper().Equals("CONFIRMED"))
                {
                    Response.Redirect("../PreSea/PreSeaSelectedCandidates.aspx?show=CONFIRM");
                }
                else if (dce.CommandName.ToUpper().Equals("WAITLISTED"))
                {
                    Response.Redirect("../PreSea/PreSeaSelectedCandidates.aspx?show=WAIT");
                }
                else if (dce.CommandName.ToUpper().Equals("REJECTED"))
                {
                    Response.Redirect("../PreSea/PreSeaSelectedCandidates.aspx?show=REJECT");
                }
            }
            else if (!General.GetNullableInteger(ucBatch.SelectedBatch).HasValue)
            {
                ucError.ErrorMessage = "Please select batch to see the Confirmed/Waitlised/Rejected Candidate's list";
                ucError.Visible = true;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuPreSeaGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                Response.ClearContent();
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment;filename=PreSea.xls");
                Response.Charset = "";
                System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                stringwriter.Write("<table><tr><td colspan=\"" + (gvPreSea.Columns.Count) + "\"></td></tr></table>");
                HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
                gvPreSea.RenderControl(htmlwriter);
                Response.Write(stringwriter.ToString());
                Response.End();
            }
            else if (dce.CommandName.ToUpper().Equals("SEARCH"))
            {
                if (!General.GetNullableInteger(ucBatch.SelectedBatch).HasValue)
                {
                    ucError.ErrorMessage = "Select Applied batch for filter the candidates";
                    ucError.Visible = true;
                }
                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("CLEAR"))
            {
                ucExamVenue.SelectedExamVenue = "";
                ucBatch.SelectedBatch = "";
                txtName.Text = "";
                txtScoreFrom.Text = "";
                txtScoreTo.Text = "";
                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("BULKSELECTION"))
            {
                string csvinterviewid = "";
                string csvcandidateid = "";
                string batch = "";

                foreach (GridViewRow row in gvPreSea.Rows)
                {
                    CheckBox chk = (CheckBox)row.FindControl("chkConfirm");
                    if (chk != null && chk.Checked)
                    {
                        Label lblInetrview = (Label)row.FindControl("LblInterviewId");
                        Label lblBatch = (Label)row.FindControl("LblBatchId");
                        Label lblCandidate = (Label)row.FindControl("LblCandidateId");

                        csvcandidateid += lblCandidate.Text;
                        csvinterviewid += lblInetrview.Text;
                        batch = lblBatch.Text;

                    }
                }
                PhoenixPreSeaCandidateConfirmation.PreSeaBulkCandidateConfirm(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                               , csvinterviewid
                                                                               , csvcandidateid
                                                                               , General.GetNullableInteger(batch));

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    /// <summary>
    /// This event is used to verify the form control is rendered
    /// It is used to remove the error occuring while exporting to export
    /// The Error is : Control 'XXX' of type 'GridView' must be placed inside a form tag with runat=server.
    /// </summary>
    /// <param name="control"></param>
    public override void VerifyRenderingInServerForm(Control control)
    {
        return;
    }

    protected void SetFilterValues()
    {
        try
        {
            if (Filter.PreSeaScoreCardSummaryFilter != null)
            {
                NameValueCollection Filters = (NameValueCollection)Filter.PreSeaScoreCardSummaryFilter;
                ucBatch.SelectedBatch = Filters["batch"].ToString();
                ucExamVenue.SelectedExamVenue = Filters["venue"].ToString();
                txtRollNumber.Text = Filters["rollno"].ToString();
                txtName.Text = Filters["name"].ToString();
                txtScoreFrom.Text = Filters["scorefrom"].ToString();
                txtScoreTo.Text = Filters["scoreto"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void GetFilterValues()
    {
        try
        {
            NameValueCollection Filters = new NameValueCollection();
            Filters.Add("batch", ucBatch.SelectedBatch);
            Filters.Add("venue", ucExamVenue.SelectedExamVenue.ToUpper().Equals("DUMMY") ? "" : ucExamVenue.SelectedExamVenue);
            Filters.Add("rollno", txtRollNumber.Text.Trim());
            Filters.Add("name", txtName.Text.Trim());
            Filters.Add("scorefrom", txtScoreFrom.Text.Trim());
            Filters.Add("scoreto", txtScoreTo.Text.Trim());

            Filter.PreSeaScoreCardSummaryFilter = Filters;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {

        try
        {
            GetFilterValues();

            NameValueCollection Filters = (NameValueCollection)Filter.PreSeaScoreCardSummaryFilter ;

            DataSet ds = PhoenixPreSeaEntranceExam.SearchInterviewScoreCardSummary(General.GetNullableInteger(Filters["batch"].ToString())
             , General.GetNullableInteger(Filters["venue"].ToString())
             , General.GetNullableString(Filters["rollno"].ToString())
             , General.GetNullableString(Filters["name"].ToString())
             , General.GetNullableDecimal(Filters["scorefrom"].ToString())
             , General.GetNullableDecimal(Filters["scoreto"].ToString())
             , General.GetNullableString("ALL"));
            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[1];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    BoundField field = new BoundField();
                    field.HeaderText = dt.Rows[i]["FLDCOLUMNAME"].ToString();
                    field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                    field.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    field.ItemStyle.Wrap = false;
                    gvPreSea.Columns.Add(field);
                }
                gvPreSea.DataSource = ds;
                gvPreSea.DataBind();
            }
            else
            {
                ShowNoRecordsFound(ds.Tables[1], gvPreSea);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPreSea_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            GridView gv = (GridView)sender;
            DataTable header;
            DataTable data;

            if (gv.DataSource.GetType().Equals(typeof(DataSet)))
            {
                DataSet ds = (DataSet)gv.DataSource;
                header = ds.Tables[1];
                data = ds.Tables[2];
            }
            else
            {
                header = (DataTable)gv.DataSource;
                data = (DataTable)gv.DataSource;
            }

            DataRowView drv = (DataRowView)e.Row.DataItem;
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (e.Row.Cells.Count > 2)
                {
                    for (int i = 0; i < header.Rows.Count; i++)
                    {
                        e.Row.Cells[i + 2].Attributes.Add("title", header.Rows[i]["FLDFIELDDESCRIPION"].ToString());
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.DataRow && gv.DataSource.GetType().Equals(typeof(DataSet)))
            {
                string interviewid = drv["FLDINTERVIEWID"].ToString();
                string candidate = drv["FLDCANDIDATEID"].ToString();
                string batch = drv["FLDBATCHID"].ToString();
                string course = drv["FLDPRESEACOURSE"].ToString();

                LinkButton lbtn = (LinkButton)e.Row.FindControl("lnkCrew");
                lbtn.CommandName = "SELECTION";
                if (lbtn != null)
                {
                    lbtn.Attributes.Add("onclick", "Openpopup('CandidateConfirm', '', '../PreSea/PreSeaCandidateConfirmation.aspx?candidateId=" + candidate + "&batch=" + batch + "&interviewid=" + interviewid + "&course=" + course + "'); return false;");
                    lbtn.Enabled = SessionUtil.CanAccess(this.ViewState, lbtn.CommandName);
                }

                for (int i = 0; i < header.Rows.Count; i++)
                {
                    DataRow[] dr = data.Select("FLDINTERVIEWID = " + interviewid + " AND FLDCOLUMNAME = '" + header.Rows[i]["FLDCOLUMNAME"].ToString() + "' AND FLDFIELDID = " + header.Rows[i]["FLDFIELDID"].ToString());
                    if (dr.Length > 0)
                        e.Row.Cells[i + 2].Text = (String.IsNullOrEmpty(dr[0]["FLDACADEMICSUBJECT"].ToString()) ? dr[0]["FLDALLOTEDSCORE"].ToString() : dr[0]["FLDSCOREDPERCENTAGE"].ToString());

                }
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
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            gv.HeaderRow.Cells[0].Text = "&nbsp;";

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
}
