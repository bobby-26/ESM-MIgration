using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
using System.Globalization;
using System.Drawing;
using Telerik.Web.UI;

public partial class CrewCourseCandidatePerformance : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddImageButton("../Crew/CrewCourseCandidatePerformance.aspx?batchid=" + Request.QueryString["batchid"], "Export to Excel", "icon_xls.png", "Excel");
			MenuGridCandidatePerformnace.AccessRights = this.ViewState;
			MenuGridCandidatePerformnace.MenuList = toolbar.Show();
			if (!IsPostBack)
			{
				if (General.GetNullableInteger(Request.QueryString["batchid"]) != null)
				{
					EditBatchDetails(Convert.ToInt32(Request.QueryString["batchid"]));
					ucBatch.Enabled = false;
					BindCandidatePerfarmanceList();
				}
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
				txtBatchNo.Text = ds.Tables[0].Rows[0]["FLDBATCH"].ToString();
				DateTime dtToDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["FLDTODATE"].ToString());
				txtToDate.Text = dtToDate.ToShortDateString();
				DateTime dtFromDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["FLDFROMDATE"].ToString());
				txtFromDate.Text = dtFromDate.ToShortDateString();
				ucBatch.SelectedBatch = ds.Tables[0].Rows[0]["FLDBATCHID"].ToString();
			}
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	public void BindCandidatePerfarmanceList()
	{

		try
		{
			gvCandidatePerformnace.Columns.Clear();

			DataSet ds = PhoenixCrewCourseAssessment.ListCandidatePerformance(Convert.ToInt32(Request.QueryString["batchid"]));

			string[] nonaggcol = { "No", "Name", "Rank", "Written Marks", "Written %", "CBT Marks", "CBT %", "Result", "Remarks" };
			if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
			{

				for (int i = 0; i < nonaggcol.Length-1; i++)
				{
					if (nonaggcol[i] == "Name")
					{

						HyperLinkField lnk = new HyperLinkField();
						lnk.HeaderText = nonaggcol[i];
						lnk.DataTextField = nonaggcol[i];

						gvCandidatePerformnace.Columns.Add(lnk);
					}
					else
					{
						BoundField field = new BoundField();

						field.DataField = nonaggcol[i];
						field.HeaderText = nonaggcol[i];
						gvCandidatePerformnace.Columns.Add(field);
					}
				}
				if (ds.Tables[1].Rows.Count > 0)
				{
					DataTable dt = ds.Tables[1];

					for (int i = 0; i < dt.Rows.Count; i++)
					{
						BoundField field = new BoundField();

						field = new BoundField();
						field.DataField = dt.Rows[i]["FLDNAME"].ToString();
						field.HeaderText = dt.Rows[i]["FLDNAME"].ToString();
						field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
						field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

						gvCandidatePerformnace.Columns.Add(field);

					}
					for (int i = nonaggcol.Length; i <= nonaggcol.Length; i++)
					{

						BoundField field = new BoundField();

						field.DataField = nonaggcol[i - 1];
						field.HeaderText = nonaggcol[i - 1];
						field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
						gvCandidatePerformnace.Columns.Add(field);

					}
				}
				gvCandidatePerformnace.DataSource = ds;
				gvCandidatePerformnace.DataBind();
				GridViewRow row = new GridViewRow(0, 2, DataControlRowType.Header, DataControlRowState.Normal);
				row.Attributes.Add("style", "position:static");
				TableCell cell = new TableCell();
				row.Cells.Add(cell);
				cell.ColumnSpan = 3;

				cell = new TableCell();
				cell.ColumnSpan = 2;
				cell.HorizontalAlign = HorizontalAlign.Center;
				cell.Text ="Written";
				row.Cells.Add(cell);

				cell = new TableCell();
				cell.ColumnSpan = 2;
				cell.HorizontalAlign = HorizontalAlign.Center;
				cell.Text = "CBT";
				row.Cells.Add(cell);
				cell = new TableCell();
				cell.ColumnSpan = 1;
				cell.HorizontalAlign = HorizontalAlign.Center;
				cell.Text = "Result";
				row.Cells.Add(cell);
				gvCandidatePerformnace.Controls[0].Controls.AddAt(0, row);

				if (ds.Tables[2].Rows.Count > 0)
				{
					DataTable dtheader = ds.Tables[2];
					cell = new TableCell();
					cell.ColumnSpan = Convert.ToInt32(dtheader.Rows[0]["FLDCOUNT"].ToString())+1;
					cell.HorizontalAlign = HorizontalAlign.Center;
					cell.Text = "Behavioural Aspects";
					row.Cells.Add(cell);
					gvCandidatePerformnace.Controls[0].Controls.AddAt(0, row);
				}

			}
			else
			{
				ViewState["row"] = "";
				BoundField field = new BoundField();
				field.HeaderText = "";
				gvCandidatePerformnace.Columns.Add(field);
				DataTable dt = new DataTable();
				ShowNoRecordsFound(dt, gvCandidatePerformnace);
			}

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void MenuCandidatePerformnace_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{

				BindCandidatePerfarmanceList();
				Response.ClearContent();
				Response.ContentType = "application/ms-excel";
				Response.AddHeader("content-disposition", "attachment;filename=CandidatePerformance.xls");
				Response.Charset = "";
				System.IO.StringWriter stringwriter = new System.IO.StringWriter();
				Response.Write("<br>");
				Response.Write("<TABLE BORDER='1' CELLPADDING='1' CELLSPACING='1' width='100%'>");
				Response.Write("<tr>");
				Response.Write("</tr>");
				Response.Write("<tr>");
				Response.Write("<td align='center' colspan='13'>");
				Response.Write("<b>Candidate Performance Report</b>");
				Response.Write("</td>");
				Response.Write("</tr>");
				Response.Write("<td><b>Batch No</b> </td>");
				Response.Write("<td >" + txtBatchNo.Text + " </td>");
				Response.Write("<td><b>From Date</b> </td>");
				Response.Write("<td colspan='2'>" + txtFromDate.Text + " </td>");
				Response.Write("<td><b>To Date</b> </td>");
				Response.Write("<td >" + txtToDate.Text + " </td>");
				Response.Write("</tr>");
				Response.Write("</TABLE>");
				stringwriter.Write("<table><tr><td colspan=\"" + gvCandidatePerformnace.Columns.Count + "\"></td></tr></table>");
				HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
				gvCandidatePerformnace.RenderControl(htmlwriter);
				Response.Write(stringwriter.ToString());
				Response.End();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvCandidatePerformnace_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		DataRowView drv = (DataRowView)e.Row.DataItem;
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (e.Row.Cells.Count > 1)
			{
				HyperLink hl = (HyperLink)e.Row.Cells[1].Controls[0];
				if (hl != null)
				{
					hl.NavigateUrl = "#";
					if (drv["FLDNEWAPPYN"].ToString() == "1")
					{
						hl.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewNewApplicantPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");
					}
					else
					{
						hl.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewPersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "'); return false;");
					}
				}
			}
		}
		
	}
	
	public override void VerifyRenderingInServerForm(Control control)
	{
		return;
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
