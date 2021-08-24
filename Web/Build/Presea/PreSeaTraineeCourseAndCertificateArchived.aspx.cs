using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.PreSea;


public partial class PreSeaTraineeCourseAndCertificateArchived : PhoenixBasePage
{
	string empid = string.Empty;
	protected override void Render(HtmlTextWriter writer)
	{
		foreach (GridViewRow r in gvTraineeCourseCertificate.Rows)
		{
			if (r.RowType == DataControlRowType.DataRow)
			{
				Page.ClientScript.RegisterForEventValidation
						(r.UniqueID + "$ctl00");
				Page.ClientScript.RegisterForEventValidation
						(r.UniqueID + "$ctl01");
			}
		}

		base.Render(writer);
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			empid = Request.QueryString["empid"];
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("Courses", "OC");
			MenuCrew.AccessRights = this.ViewState;
			MenuCrew.MenuList = toolbar.Show();

			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;
				MenuCrew.SelectedMenuIndex = 0;

			}

			if (Request.QueryString["type"] == "1")
			{
				BindData();
				SetPageNavigator();
			}
			else if (Request.QueryString["type"] == "2")
			{
				divTD.Visible = false;
			}

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void Crew_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("OC"))
			{
				Response.Redirect("PreSeaTraineeCourseAndCertificateArchived.aspx?empid=" + empid + "&type=1", false);
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

			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;

			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

			DataSet ds = PhoenixPreSeaTraineeCourseAndCertificate.TraineeCourseCertificateSearch(
						Int32.Parse(empid), 0
						, 0  // list courses other than cbt's
						, sortexpression, sortdirection
						, (int)ViewState["PAGENUMBER"]
						, General.ShowRecords(null)
						, ref iRowCount
						, ref iTotalPageCount);

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvTraineeCourseCertificate.DataSource = ds;
				gvTraineeCourseCertificate.DataBind();
			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvTraineeCourseCertificate);
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

	protected void gvTraineeCourseCertificate_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			string coursecertificateid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseId")).Text;

			PhoenixCrewCourseCertificate.DeleteCrewCourseCertificate(Convert.ToInt32(coursecertificateid)
															);
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;

		}
	}


	protected void gvTraineeCourseCertificate_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		try
		{

			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			if (e.CommandName.ToString().ToUpper() == "DEARCHIVE")
			{
				string id = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseId")).Text;
				if (Request.QueryString["t"] == "n")
                    PhoenixPreSeaTraineeCourseAndCertificate.ArchiveTraineeCourseCertificate(int.Parse(empid), int.Parse(id), 1);
				else
                    PhoenixPreSeaTraineeCourseAndCertificate.ArchiveTraineeCourseCertificate(int.Parse(empid), int.Parse(id), 1);
				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);
				_gridView.EditIndex = -1;
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

	protected void gvTraineeCourseCertificate_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			// Get the LinkButton control in the first cell
			LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
			// Get the javascript which is assigned to this LinkButton
			string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
			// Add this javascript to the onclick Attribute of the row
			e.Row.Attributes["onclick"] = _jsDouble;
		}
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
				&& !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
			{
				ImageButton db1 = (ImageButton)e.Row.FindControl("cmdArchive");
				db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'De-Archive selected document ?')");
				Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
				ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
				Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");
				if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
				att.Attributes.Add("onclick", "javascript:Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
					+ PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&u=n'); return false;");
				Label expdate = e.Row.FindControl("lblExpiryDate") as Label;
				System.Web.UI.WebControls.Image imgFlag = e.Row.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
				DateTime? d = General.GetNullableDateTime(expdate.Text);
				if (d.HasValue)
				{
					TimeSpan t = d.Value - DateTime.Now;
					if (t.Days >= 0 && t.Days < 120)
					{
						//e.Row.CssClass = "rowyellow";
						imgFlag.Visible = true;
						imgFlag.ImageUrl = Session["images"] + "/yellow.png";
					}
					else if (t.Days < 0)
					{
						//e.Row.CssClass = "rowred";
						imgFlag.Visible = true;
						imgFlag.ImageUrl = Session["images"] + "/red.png";
					}
				}
			}
			else
				e.Row.Attributes["onclick"] = "";
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
			gvTraineeCourseCertificate.SelectedIndex = -1;
			gvTraineeCourseCertificate.EditIndex = -1;
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
			gv.Rows[0].Attributes["ondblclick"] = "";
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
	


}
