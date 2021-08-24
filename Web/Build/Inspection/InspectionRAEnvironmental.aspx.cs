using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using System.Text;
using SouthNests.Phoenix.Inspection;


public partial class InspectionRAEnvironmental : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
			PhoenixToolbar toolbargrid = new PhoenixToolbar();
			toolbargrid.AddImageButton("../Inspection/InspectionRAEnvironmental.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbargrid.AddImageLink("javascript:CallPrint('gvEnvironmental')", "Print Grid", "icon_print.png", "PRINT");
			MenuGridEnvironmental.AccessRights = this.ViewState;
			MenuGridEnvironmental.MenuList = toolbargrid.Show();

			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				PhoenixToolbar toolbar = new PhoenixToolbar();
				toolbar.AddButton("Back", "BACK");
				toolbar.AddButton("Health and Safety", "HEALTHANDSAFETY");
				toolbar.AddButton("Environmental", "ENVIRONMENTAL");
				toolbar.AddButton("Operational", "OPERATIONAL");
				toolbar.AddButton("Other Risk", "OTHER");
				MenuHeader.AccessRights = this.ViewState;
				MenuHeader.MenuList = toolbar.Show();
				MenuHeader.SelectedMenuIndex = 2;

				toolbar = new PhoenixToolbar();
				toolbar.AddButton("New", "NEW");
				toolbar.AddButton("Save", "SAVE");
				MenuEnvironmental.AccessRights = this.ViewState;
				MenuEnvironmental.MenuList = toolbar.Show();
				ViewState["assessmentid"] = "";
				if (Request.QueryString["subcategoryid"] != null)
				{
					ViewState["subcategoryid"] = Request.QueryString["subcategoryid"];
					DataSet ds = PhoenixInspectionRiskAssessmentSubCategory.EditRiskAssessmentSubCategory(Convert.ToInt32(ViewState["subcategoryid"]));
					if (ds.Tables[0].Rows.Count > 0)
					{
						txtSubcategory.Text = ds.Tables[0].Rows[0]["FLDNAME"].ToString();
					}
				}
				if (Request.QueryString["categoryid"] != null)
				{
					ucCategory.SelectedCategory = Request.QueryString["categoryid"];
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


	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		DataSet ds = new DataSet();
		string[] alColumns = { "FLDACTIVITY", "FLDRISK", "FLDHAZARDNAME", "FLDPOC", "FLDLIKELYHOODOFHARM", "FLDLEVELOFCONTROL", "FLDLEVELOFRISK" };
		string[] alCaptions = { "Activity", "Risk", "Impact", "Probabilty of Occurance", "Likelyhood of Harm", "Level of Control", "Level of Risk" };

		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		ds = PhoenixInspectionRiskAssessment.RiskAssessmentSearch(Convert.ToInt32(ViewState["subcategoryid"]),
				2, sortexpression, sortdirection,
				Int32.Parse(ViewState["PAGENUMBER"].ToString()),
				iRowCount,
				ref iRowCount,
				ref iTotalPageCount);

		Response.AddHeader("Content-Disposition", "attachment; filename=Environmental.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3> Environmental</h3></td>");
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
	protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

		if (dce.CommandName.ToUpper().Equals("BACK"))
		{
			Response.Redirect("../Inspection/InspectionRASubCategory.aspx", false);
		}
		if (dce.CommandName.ToUpper().Equals("HEALTHANDSAFETY"))
		{
			Response.Redirect("../Inspection/InspectionRAHealthAndSafety.aspx?categoryid=" + Request.QueryString["categoryid"] + "&subcategoryid=" + ViewState["subcategoryid"], false);
		}
		if (dce.CommandName.ToUpper().Equals("OTHER"))
		{
			Response.Redirect("../Inspection/InspectionRAOtherRisk.aspx?categoryid=" + Request.QueryString["categoryid"] + "&subcategoryid=" + ViewState["subcategoryid"], false);
		} 
		if (dce.CommandName.ToUpper().Equals("OPERATIONAL"))
		{
			Response.Redirect("../Inspection/InspectionRAOperational.aspx?categoryid=" + Request.QueryString["categoryid"] + "&subcategoryid=" + ViewState["subcategoryid"], false);
		}
	}

	protected void Environmental_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		if (dce.CommandName.ToUpper().Equals("SAVE"))
		{
			if (!IsValidEnvironmental(txtActivity.Text, txtRisk.Text, ucHazardType.SelectedHazardType, ucFrequency.SelectedFrequency, ucActivityDuration.SelectedFrequency, ucRAControl.SelectedFrequency))
			{
				ucError.Visible = true;
				return;
			}
			if (ViewState["assessmentid"].ToString() == "")
			{

				PhoenixInspectionRiskAssessment.InsertRiskAssessment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					Convert.ToInt32(ViewState["subcategoryid"]),
					txtActivity.Text, txtRisk.Text,
					General.GetNullableInteger(ucHazardType.SelectedHazardType),
					null, General.GetNullableInteger(ucFrequency.SelectedFrequency),
					General.GetNullableInteger(ucActivityDuration.SelectedFrequency),
					General.GetNullableInteger(ucRAControl.SelectedFrequency), 2);
				Reset();
				BindData();
			}
			else
			{


				PhoenixInspectionRiskAssessment.UpdateRiskAssessment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					Convert.ToInt32(ViewState["assessmentid"]),
					Convert.ToInt32(ViewState["subcategoryid"]),
					txtActivity.Text, txtRisk.Text,
					General.GetNullableInteger(ucHazardType.SelectedHazardType),
					null, General.GetNullableInteger(ucFrequency.SelectedFrequency),
					General.GetNullableInteger(ucActivityDuration.SelectedFrequency),
					General.GetNullableInteger(ucRAControl.SelectedFrequency), 2);
				SetDetails(Convert.ToInt32(ViewState["assessmentid"]));
				BindData();
			}


		}
		if (dce.CommandName.ToUpper().Equals("NEW"))
		{
			Reset();
		}
	}
	protected void Reset()
	{
		txtActivity.Text = "";
		txtRisk.Text = "";
		ucHazardType.SelectedHazardType = "";
		ucFrequency.SelectedFrequency = "";
		ucActivityDuration.SelectedFrequency = "";
		ucRAControl.SelectedFrequency = "";
		txtPOC.Text = "";
		txtLOH.Text = "";
		txtLOC.Text = "";
		txtLevelOfRisk.Text = "";
		ViewState["assessmentid"] = "";
		gvEnvironmental.SelectedIndex = -1;
	}
	private bool IsValidEnvironmental(string activity, string risk, string hazardtype, string activityfrequency, string activityduration, string racontrol)
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (activity.Trim().Equals(""))
			ucError.ErrorMessage = "Activity is required.";

		if (risk.Trim().Equals(""))
			ucError.ErrorMessage = "Risk is required.";

		if (General.GetNullableInteger(hazardtype) == null)
			ucError.ErrorMessage = "Impact is required.";

		if (General.GetNullableInteger(activityfrequency) == null)
			ucError.ErrorMessage = "Activity Frequency is required.";

		if (General.GetNullableInteger(activityduration) == null)
			ucError.ErrorMessage = "Activity duration is required.";

		if (General.GetNullableInteger(racontrol) == null)
			ucError.ErrorMessage = "RA control is required.";

		return (!ucError.IsError);
	}
	public void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();

	}


	private void BindData()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;

			string[] alColumns = { "FLDACTIVITY", "FLDRISK", "FLDHAZARDNAME", "FLDPOC", "FLDLIKELYHOODOFHARM", "FLDLEVELOFCONTROL", "FLDLEVELOFRISK" };
			string[] alCaptions = { "Activity", "Risk", "Impact", "Probabilty of Occurance", "Likelyhood of Harm", "Level of Control", "Level of Risk" };

			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
			string a = ViewState["subcategoryid"].ToString();

			DataSet ds = PhoenixInspectionRiskAssessment.RiskAssessmentSearch(Convert.ToInt32(ViewState["subcategoryid"]),
					2, sortexpression, sortdirection,
					Int32.Parse(ViewState["PAGENUMBER"].ToString()),
					General.ShowRecords(null),
					ref iRowCount,
					ref iTotalPageCount);

			General.SetPrintOptions("gvEnvironmental", "Health And Safety List", alCaptions, alColumns, ds);

			if (ds.Tables[0].Rows.Count > 0)
			{
				gvEnvironmental.DataSource = ds;
				gvEnvironmental.DataBind();

			}
			else
			{
				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvEnvironmental);
				ViewState["batchid"] = "";
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
	private void SetRowSelection()
	{
		gvEnvironmental.SelectedIndex = -1;
		for (int i = 0; i < gvEnvironmental.Rows.Count; i++)
		{
			if (gvEnvironmental.DataKeys[i].Value.ToString().Equals(ViewState["assessmentid"]))
			{
				gvEnvironmental.SelectedIndex = i;
				ViewState["DTKEY"] = ((Label)gvEnvironmental.Rows[gvEnvironmental.SelectedIndex].FindControl("lbldtkey")).Text;

			}
		}
	}
	protected void GridEnvironmental_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
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
	}

	protected void gvEnvironmental_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;

			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				PhoenixInspectionRiskAssessment.DeleteRiskAssessment(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
							Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAssessmentId")).Text));
			}
			if (e.CommandName.ToUpper().Equals("SELECT"))
			{
				ViewState["assessmentid"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAssessmentId")).Text;
				SetDetails(Int32.Parse(ViewState["assessmentid"].ToString()));
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

	protected void gvEnvironmental_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		BindData();
		SetPageNavigator();
	}
	protected void gvEnvironmental_RowEditing(object sender, GridViewEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.NewEditIndex;
		_gridView.SelectedIndex = e.NewEditIndex;

		string assessmentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAssessmentId")).Text;
		ViewState["assessmentid"] = assessmentid;
		SetDetails(Convert.ToInt32(ViewState["assessmentid"]));
	}
	protected void SetDetails(int assessmentid)
	{
		DataSet ds = PhoenixInspectionRiskAssessment.EditRiskAssessment(assessmentid);
		if (ds.Tables[0].Rows.Count > 0)
		{
			txtActivity.Text = ds.Tables[0].Rows[0]["FLDACTIVITY"].ToString();
			txtRisk.Text = ds.Tables[0].Rows[0]["FLDRISK"].ToString();
			ucHazardType.SelectedHazardType = ds.Tables[0].Rows[0]["FLDHAZARD"].ToString();
			ucFrequency.SelectedFrequency = ds.Tables[0].Rows[0]["FLDNOOFPEOPLE"].ToString();
			ucActivityDuration.SelectedFrequency = ds.Tables[0].Rows[0]["FLDACTIVITYDURATION"].ToString();
			ucRAControl.SelectedFrequency = ds.Tables[0].Rows[0]["FLDRACONTROL"].ToString();
			txtPOC.Text = ds.Tables[0].Rows[0]["FLDPOC"].ToString();
			txtLOH.Text = ds.Tables[0].Rows[0]["FLDLIKELYHOODOFHARM"].ToString();
			txtLOC.Text = ds.Tables[0].Rows[0]["FLDLEVELOFCONTROL"].ToString();
			txtLevelOfRisk.Text = ds.Tables[0].Rows[0]["FLDLEVELOFRISK"].ToString();
		}
	}
	protected void gvEnvironmental_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
			{
				ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
				if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
				Label l = (Label)e.Row.FindControl("lblAssessmentId");


			}
		}


	}



	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		BindData();

	}
	protected void gvEnvironmental_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvEnvironmental.SelectedIndex = -1;
		gvEnvironmental.EditIndex = -1;

		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}



	protected void cmdGo_Click(object sender, EventArgs e)
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

	protected void PagerButtonClick(object sender, CommandEventArgs ce)
	{
		gvEnvironmental.SelectedIndex = -1;
		gvEnvironmental.EditIndex = -1;
		if (ce.CommandName == "prev")
			ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
		else
			ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

		BindData();
		SetPageNavigator();
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

}
