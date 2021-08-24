using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class CrewCourseReview : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
		try
		{
			if (!IsPostBack)
			{

				if (Request.QueryString["courseid"] != null)
				{
					ViewState["courseid"] = Request.QueryString["courseid"];
				}
				PhoenixToolbar toolbar = new PhoenixToolbar();
				toolbar.AddImageButton("../Crew/CrewCourseReview.aspx?courseid=" + ViewState["courseid"], "Export to Excel", "icon_xls.png", "Excel");
				toolbar.AddImageLink("javascript:CallPrint('gvCourseReview')", "Print Grid", "icon_print.png", "PRINT");
				MenuCourseReview.AccessRights = this.ViewState;
				MenuCourseReview.MenuList = toolbar.Show();
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

		string[] alColumns = { "FLDREVISIONNO", "FLDREVIEWDATE", "FLDCREATEDBY" };
		string[] alCaptions = { "Sl No", "Date of last course review", "Done by"};

		DataSet ds = PhoenixRegistersDocumentCourse.ListCourseReview(General.GetNullableInteger(ViewState["courseid"].ToString()));

		General.SetPrintOptions("gvCourseReview", "Course Review List", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvCourseReview.DataSource = ds.Tables[0];
			gvCourseReview.DataBind();

		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvCourseReview);
		}


	}
	private void ShowExcel()
	{

		string[] alColumns = { "FLDREVISIONNO", "FLDREVIEWDATE", "FLDCREATEDBY" };
		string[] alCaptions = { "Sl No", "Date of last course review", "Done by" };

		DataSet ds = PhoenixRegistersDocumentCourse.ListCourseReview(General.GetNullableInteger(ViewState["courseid"].ToString()));

		General.SetPrintOptions("gvCourseReview", "Course Review List", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvCourseReview.DataSource = ds.Tables[0];
			gvCourseReview.DataBind();

		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvCourseReview);
		}
		
		Response.AddHeader("Content-Disposition", "attachment; filename=CrewCourseReview.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
		Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Course Review List</center></h5></td></tr>");
		Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Course Name:" + DateTime.Now.ToShortDateString() + "</td></tr>");
		Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
		Response.Write("</tr>");
		Response.Write("</TABLE>");
		Response.Write("<br />");
		Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
		Response.Write("<tr>");
		for (int i = 0; i < alCaptions.Length; i++)
		{
			Response.Write("<td>");
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

	protected void gvCourseReview_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;

			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());


			BindData();

		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvCourseReview_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
	{

		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
			{
				LinkButton lbr = (LinkButton)e.Row.FindControl("lnkReviewDate");
				Label lblReviewid = (Label)e.Row.FindControl("lblReviewid");
				lbr.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewCourseReviewHistory.aspx?reviewid=" + lblReviewid.Text + "'); return false;");
			}
		}

	}
	protected void CourseReview_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		if (dce.CommandName.ToUpper().Equals("FIND"))
		{
			ViewState["PAGENUMBER"] = 1;
			BindData();

		}
		if (dce.CommandName.ToUpper().Equals("EXCEL"))
		{
			ShowExcel();
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

	public StateBag ReturnViewState()
	{
		return ViewState;
	}

}
