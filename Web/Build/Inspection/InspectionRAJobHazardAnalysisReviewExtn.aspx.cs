using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Data;

public partial class InspectionRAJobHazardAnalysisReviewExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
		PhoenixToolbar toolbar = new PhoenixToolbar();

		toolbar.AddImageButton("../Inspection/InspectionRAJobHazardAnalysisReviewExtn.aspx?jobhazardid=" + Request.QueryString["jobhazardid"], "Export to Excel", "icon_xls.png", "Excel");
		toolbar.AddImageLink("javascript:CallPrint('gvRiskAssessmentJobHazardAnalysisReview')", "Print Grid", "icon_print.png", "PRINT");

		MenuJobHazardAnalysisReview.MenuList = toolbar.Show();

		if (!IsPostBack)
		{

			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
			EditJobHazard(new Guid (Request.QueryString["jobhazardid"]));

		}
		BindData();
    }
	protected void MenuJobHazardAnalysisReview_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

		if (dce.CommandName.ToUpper().Equals("EXCEL"))
		{
			ShowExcel();
		}

	}
	protected void EditJobHazard(Guid jobhazardid)
	{
		try
		{
			DataSet ds = PhoenixInspectionRiskAssessmentJobHazard.EditRiskAssessmentJobHazard(jobhazardid);
			if (ds.Tables[0].Rows.Count > 0)
			{
		
				ddlCategory.SelectedCategory = ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString();
				if (ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString() != "")
				{
					BindJob_OnSelectedIndexChanged(null, null);
					ddlJob.SelectedValue = ds.Tables[0].Rows[0]["FLDJOBID"].ToString();
				}
				txtJob.Text = ds.Tables[0].Rows[0]["FLDJOB"].ToString();
				txtRevisionno.Text = ds.Tables[0].Rows[0]["FLDREVISIONNO"].ToString();
				txtIssuedDate.Text = ds.Tables[0].Rows[0]["FLDISSUEDDATE"].ToString();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void BindJob_OnSelectedIndexChanged(object sender, EventArgs e)
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;
		DataSet ds = new DataSet();

		ds = PhoenixInspectionRiskAssessmentActivity.RiskAssessmentActivitySearch(null,
			General.GetNullableInteger(ddlCategory.SelectedCategory),
			null, null,
			1,
			General.ShowRecords(null),
			ref iRowCount,
			ref iTotalPageCount);

		if (ds.Tables[0].Rows.Count > 0)
		{
			ddlJob.Items.Clear();
			ddlJob.Items.Insert(0, new ListItem("--Select--", "Dummy"));
			ddlJob.DataSource = ds.Tables[0];
			ddlJob.DataBind();
		}
	}
	private void BindData()
	{

        string[] alColumns = { "FLDOPERATIONALHAZARD", "FLDHEALTHANDSAFETY", "FLDENVIRONMENTALHAZARD", "FLDECONOMICHAZARD", "FLDOTHERHAZARD", "FLDCONTROLS", "FLDRECOMMENDEPPE", "FLDCOMPETENCYLEVEL" };
        string[] alCaptions = { "Operational Hazards / Aspects", "Health and Safety Hazards", "Environmental Impact", "Economic / Process Loss", "Worst Case Scenario", "Controls/Precautions", "Recommended PPE", "Competency Level for Supervision" };

		DataSet ds = PhoenixInspectionRiskAssessmentJobHazard.ListRiskAssessmentJobHazardAnalysis(new Guid(Request.QueryString["jobhazardid"]));

		General.SetPrintOptions("gvRiskAssessmentJobHazardAnalysisReview", "Job Hazard Analysis", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvRiskAssessmentJobHazardAnalysisReview.DataSource = ds;
			gvRiskAssessmentJobHazardAnalysisReview.DataBind();
		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvRiskAssessmentJobHazardAnalysisReview);
		}
	}
    protected void gvRiskAssessmentJobHazardAnalysisReview_PreRender(object sender, EventArgs e)
    {
        MergeRows(gvRiskAssessmentJobHazardAnalysisReview);
    }

    protected void MergeRows(GridView g)
    {
        for (int rowIndex = g.Rows.Count - 2;
                                     rowIndex >= 0; rowIndex--)
        {
            GridViewRow gvRow = g.Rows[rowIndex];
            GridViewRow gvPreviousRow = g.Rows[rowIndex + 1];

            if (gvRow.Cells[0].Text == gvPreviousRow.Cells[0].Text)
            {
                if (gvPreviousRow.Cells[0].RowSpan < 2)                
                    gvRow.Cells[0].RowSpan = 2;                
                else                
                    gvRow.Cells[0].RowSpan = gvPreviousRow.Cells[0].RowSpan + 1;                
                gvPreviousRow.Cells[0].Visible = false;
            }
            if (gvRow.Cells[5].Text == gvPreviousRow.Cells[5].Text)
            {
                if (gvPreviousRow.Cells[5].RowSpan < 2)
                    gvRow.Cells[5].RowSpan = 2;
                else
                    gvRow.Cells[5].RowSpan = gvPreviousRow.Cells[5].RowSpan + 1;
                gvPreviousRow.Cells[5].Visible = false;
            }
        }
    }

	protected void gvRiskAssessmentJobHazardAnalysisReview_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        
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
	protected void ShowExcel()
	{
		try
		{
            string[] alColumns = { "FLDOPERATIONALHAZARD", "FLDHEALTHANDSAFETY", "FLDENVIRONMENTALHAZARD", "FLDECONOMICHAZARD", "FLDOTHERHAZARD", "FLDCONTROLS", "FLDRECOMMENDEPPE", "FLDCOMPETENCYLEVEL" };
            string[] alCaptions = { "Operational Hazards / Aspects", "Health and Safety Hazards", "Environmental Impact", "Economic / Process Loss", "Worst Case Scenario", "Controls/Precautions", "Recommended PPE", "Competency Level for Supervision" };

			DataSet ds = PhoenixInspectionRiskAssessmentJobHazard.ListRiskAssessmentJobHazardAnalysis(new Guid(Request.QueryString["jobhazardid"]));

            int rowcount;
            rowcount = ds.Tables[0].Rows.Count;
			//General.ShowExcel("Job Hazard Analysis", ds.Tables[0], alColumns, alCaptions, null, null);
            Response.AddHeader("Content-Disposition", "attachment; filename=JobHazardAnalysis.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Job Hazard Analysis</h3></td>");
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
            int r = 0;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    if (alColumns[i].ToString().Equals("FLDOPERATIONALHAZARD") || alColumns[i].ToString().Equals("FLDCONTROLS")) 
                    {
                        if (r == 0)
                        {
                            Response.Write("<td rowspan=" + rowcount.ToString() + ">");
                            Response.Write(Server.HtmlDecode(dr[alColumns[i]].ToString()));
                            Response.Write("</td>");
                        }
                    }
                    else
                    {
                        Response.Write("<td>");
                        Response.Write(Server.HtmlDecode(dr[alColumns[i]].ToString()));
                        Response.Write("</td>");
                    }
                }
                Response.Write("</tr>");
                r++;
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
