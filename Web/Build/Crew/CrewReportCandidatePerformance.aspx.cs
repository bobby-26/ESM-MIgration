using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using Telerik.Web.UI;

public partial class CrewReportCandidatePerformance : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddFontAwesomeButton("../Crew/CrewReportCandidatePerformance.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
			toolbar.AddFontAwesomeButton("../Crew/CrewReportCandidatePerformance.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
			MenuGridCandidatePerformnace.AccessRights = this.ViewState;
			MenuGridCandidatePerformnace.MenuList = toolbar.Show();
			PhoenixToolbar toolbartop = new PhoenixToolbar();
			toolbartop.AddButton("Show Report", "GO",ToolBarDirection.Right);
			MenuCandidatePerformanceReport.AccessRights = this.ViewState;
			MenuCandidatePerformanceReport.MenuList = toolbartop.Show();    
			if (!IsPostBack)
			{

				BindCourse();
				lstBatch.Items.Clear();
				lstBatch.DataSource = PhoenixRegistersBatch.ListPostSeaBatch(null);
				lstBatch.DataBind();
				lstBatch.Items.Insert(0, new RadListBoxItem("--Select--", "Dummy"));
			}
			BindCandidatePerfarmanceList();
			txtEmployeeId.Attributes.Add("style", "visibility:hidden");
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void BindBatch(object sender, EventArgs e)
	{
		lstBatch.Items.Clear();
		lstBatch.DataSource = PhoenixRegistersBatch.ListPostSeaBatch(null);
		lstBatch.DataBind();
		lstBatch.Items.Insert(0, new RadListBoxItem("--Select--", "Dummy"));
	}
	protected void BindCourse()
	{
		lstCourse.Items.Clear();
		RadListBoxItem items = new RadListBoxItem();
		lstCourse.DataSource = PhoenixRegistersDocumentCourse.ListPostSeaCourse(null);
		lstCourse.DataTextField = "FLDDOCUMENTNAME";
		lstCourse.DataValueField = "FLDDOCUMENTID";
		lstCourse.DataBind();
	}
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindCandidatePerfarmanceList();
	}
	public void BindCandidatePerfarmanceList()
	{

		try
		{
			gvCandidatePerformnace.Columns.Clear();

			StringBuilder strbatchlist = new StringBuilder();
			StringBuilder strCourse = new StringBuilder();
			foreach (RadListBoxItem item in lstBatch.Items)
			{
				if (item.Selected == true)
				{
                    if (item.Value !="Dummy")
                    {
                        strbatchlist.Append(item.Value.ToString());
                    }
                    strbatchlist.Append(",");
                }

			}
			if (strbatchlist.Length > 1)
			{
				strbatchlist.Remove(strbatchlist.Length - 1, 1);
			}
			foreach (RadListBoxItem item in lstCourse.Items)
			{
				if (item.Selected == true)
				{

					strCourse.Append(item.Value.ToString());
					strCourse.Append(",");
				}

			}
			if (strCourse.Length > 1)
			{
				strCourse.Remove(strCourse.Length - 1, 1);
			}
			
			DataSet ds = PhoenixCrewCourseAssessment.ListCandidateCombinedPerformance(General.GetNullableString(strbatchlist.ToString()),
							General.GetNullableString(strCourse.ToString()), General.GetNullableInteger(txtEmployeeId.Text));

			string[] nonaggcol = { "No", "Course", "Name", "Batch", "Rank", "Written Marks", "Written %", "CBT Marks", "CBT %", "Result", "Faculty", "Remarks" };
			if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Rows[0]["Batch"].ToString()!="") 
			{

				for (int i = 0; i < nonaggcol.Length - 1; i++)
				{
					if (nonaggcol[i] == "Name")
					{

                        GridHyperLinkColumn lnk = new GridHyperLinkColumn();
						lnk.HeaderText = nonaggcol[i];
						lnk.DataTextField = nonaggcol[i];

						gvCandidatePerformnace.Columns.Add(lnk);
					}
					else
					{
                        GridBoundColumn field = new GridBoundColumn();

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
                        GridBoundColumn field = new GridBoundColumn();

						field = new GridBoundColumn();
						field.DataField = (i + 1).ToString();
						field.HeaderText = dt.Rows[i]["FLDNAME"].ToString();
						field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
						field.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

						gvCandidatePerformnace.Columns.Add(field);

					}
					for (int i = nonaggcol.Length; i <= nonaggcol.Length; i++)
					{

                        GridBoundColumn field = new GridBoundColumn();

						field.DataField = nonaggcol[i - 1];
						field.HeaderText = nonaggcol[i - 1];
						field.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
						gvCandidatePerformnace.Columns.Add(field);

					}
				}
				gvCandidatePerformnace.DataSource = ds;
				gvCandidatePerformnace.DataBind();
                GridTableView tableview = new GridTableView();
                GridDataItem row = new GridDataItem(tableview, 0, 0, GridItemType.Header);
                row.Attributes.Add("style", "position:static");
                GridTableCell cell = new GridTableCell();
				row.Cells.Add(cell);
				cell.ColumnSpan = 5;

				cell = new GridTableCell();
				cell.ColumnSpan = 2;
				cell.HorizontalAlign = HorizontalAlign.Center;
				cell.Text = "Written";
				row.Cells.Add(cell);

				cell = new GridTableCell();
				cell.ColumnSpan = 2;
				cell.HorizontalAlign = HorizontalAlign.Center;
				cell.Text = "CBT";
				row.Cells.Add(cell);
				cell = new GridTableCell();
				cell.ColumnSpan = 1;
				cell.HorizontalAlign = HorizontalAlign.Center;
				cell.Text = "Result";
				row.Cells.Add(cell);

				cell = new GridTableCell();
				cell.ColumnSpan = 1;
				cell.HorizontalAlign = HorizontalAlign.Center;
				cell.Text = "Faculty";
				row.Cells.Add(cell);

				cell = new GridTableCell();
				cell.ColumnSpan = 11;
				cell.HorizontalAlign = HorizontalAlign.Center;
				cell.Text = "Behavioural Aspects";
				row.Cells.Add(cell);

				gvCandidatePerformnace.Controls[0].Controls.AddAt(0, row);
			}
			else
			{
				ViewState["row"] = "";
                GridBoundColumn field = new GridBoundColumn();
				field.HeaderText = "";
				gvCandidatePerformnace.Columns.Add(field);
				DataTable dt = new DataTable();
			}

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void MenuCandidatePerformanceReport_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GO"))
			{
				BindCandidatePerfarmanceList();
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
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
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
				Response.Write("</TABLE>");
				stringwriter.Write("<table><tr><td colspan=\"" + gvCandidatePerformnace.Columns.Count + "\"></td></tr></table>");
				HtmlTextWriter htmlwriter = new HtmlTextWriter(stringwriter);
				gvCandidatePerformnace.RenderControl(htmlwriter);
				Response.Write(stringwriter.ToString());
				Response.End();
			}
			if (CommandName.ToUpper().Equals("CLEAR"))
			{
				lstCourse.SelectedIndex = -1;
				lstBatch.SelectedIndex = -1;
				txtEmployeeId.Text = "";
				txtEmployeeName.Text = "";
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvCandidatePerformnace_ItemDataBound(object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (e.Item.Cells.Count > 1)
			{
                LinkButton hl = new LinkButton();
				if (hl != null)
				{
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
    protected void gvCandidatePerformnace_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCandidatePerformnace.CurrentPageIndex + 1;

        BindCandidatePerfarmanceList();
    }
    public override void VerifyRenderingInServerForm(Control control)
	{
		return;
	}

	public StateBag ReturnViewState()
	{
		return ViewState;
	}
}
