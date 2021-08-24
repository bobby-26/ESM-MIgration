using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.PreSea;
using System.Collections.Generic;
using System.Collections;


public partial class PreSeaEntryRequirementsAndEligibility : PhoenixBasePage
{
    DataSet dsGrid;
    ArrayList arrayBatch = new ArrayList();
    ArrayList arrayCourse = new ArrayList();
    ArrayList arrayCourseId = new ArrayList();
    ArrayList arrayBatchCount = new ArrayList();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["CURRENTINDEX"] = 1;
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {

        dsGrid = PhoenixPreSeaCourseMaster.BatchEligibilitySearch(1);
        arrayBatch.Clear();

        foreach (DataRow dr in dsGrid.Tables[1].Rows)
        {
            arrayBatch.Add(dr["FLDPRESEABATCHNAME"].ToString());
        }

        foreach (DataRow dr in dsGrid.Tables[2].Rows)
        {
            arrayCourse.Add(dr["FLDPRESEACOURSENAME"].ToString());
            arrayCourseId.Add(dr["FLDPRESEACOURSEID"].ToString());
            arrayBatchCount.Add(dr["FLDCOUNT"].ToString());
        }

        AddCoumnsInGrid(dsGrid.Tables[0], dsGrid.Tables[1]);
        gvEligibility.DataSource = dsGrid;

        gvEligibility.DataBind();

        if (dsGrid.Tables[0].Rows.Count <= 0)
        {
            ShowNoRecordsFound(dsGrid.Tables[0], gvEligibility);
        }
    }

    private void AddCoumnsInGrid(DataTable datatable, DataTable coursetable)
    {
        if (datatable.Columns.Count > 0 && gvEligibility.Columns.Count < 2)
        {
            for (int i = 1; i < datatable.Columns.Count; i++)
            {
                BoundField eligibilitydetail = new BoundField();
                eligibilitydetail.DataField = datatable.Columns[i].ColumnName;
                eligibilitydetail.HeaderText = coursetable.Rows[i - 1]["FLDPRESEABATCHNAME"].ToString();
                eligibilitydetail.ControlStyle.BorderColor = System.Drawing.Color.White;
                gvEligibility.Columns.Add(eligibilitydetail);
            }
        }
    }

    protected void gvEligibility_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (gvEligibility.Columns.Count > 1)
            {
                int j = 0;
                GridView HeaderGrid = (GridView)sender;
                GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

                for (int i = 0; i <= gvEligibility.Columns.Count; i++)
                {
                    TableCell HeaderCell;

                    if (i < 1)
                    {
                        HeaderCell = new TableCell();
                        HeaderCell.Text = "";
                        HeaderCell.ColumnSpan = 1;
                        HeaderGridRow.Cells.Add(HeaderCell);
                        i = i + 1;
                    }
                    if (i >= 2 && i <= gvEligibility.Columns.Count)
                    {
                        HeaderCell = new TableCell();

                        string course = arrayCourse[j].ToString().Trim();
                        string count = arrayBatchCount[j].ToString().Trim();

                        if (j < arrayCourse.Count)
                            HeaderCell.Text = course;
                        else
                            HeaderCell.Text = "";
                        HeaderCell.HorizontalAlign = HorizontalAlign.Center;
                        if (General.GetNullableInteger(count) != null)
                            HeaderCell.ColumnSpan = int.Parse(count);

                        HeaderGridRow.Cells.Add(HeaderCell);
                        j = j + 1;
                        i = i + int.Parse(count)-1;
                    }
                }
                gvEligibility.Controls[0].Controls.AddAt(0, HeaderGridRow);
            }
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
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        Response.Redirect("../PreSea/PreSeaOnlineApplication.aspx");
    }
    protected void btnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("../PreSea/PreSeaRegisterdCandidatesLogin.aspx");
    }

}
