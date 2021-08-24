using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaTraineeAcademicQualification : PhoenixBasePage
{
	protected override void Render(HtmlTextWriter writer)
	{
		foreach (GridViewRow r in gvPreSeaAcademics.Rows)
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
			PhoenixToolbar toolbargrid = new PhoenixToolbar();
			toolbargrid.AddImageButton("../PreSea/PreSeaTraineeAcademicQualification.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbargrid.AddImageLink("javascript:CallPrint('gvPreSeaAcademics')", "Print Grid", "icon_print.png", "PRINT");
			toolbargrid.AddImageLink("javascript:parent.Openpopup('codehelp1','','PreSeaNewApplicantAcademic.aspx?empid=" + Filter.CurrentPreSeaTraineeSelection + "')", "Add", "add.png", "ADDPRESEAACADEMIC");
			MenuPreSeaAcademic.AccessRights = this.ViewState;
			MenuPreSeaAcademic.MenuList = toolbargrid.Show();
			MenuPreSeaAcademic.SetTrigger(pnlPreSeaTraineeAcademicEntry);

			toolbargrid = new PhoenixToolbar();
			toolbargrid.AddImageButton("../PreSea/PreSeaTraineeAcademicQualification.aspx", "Export to Excel", "icon_xls.png", "Excel");
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

			toolbargrid = new PhoenixToolbar();
			toolbargrid.AddImageButton("../PreSea/PreSeaTraineeAcademicQualification.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbargrid.AddImageLink("javascript:CallPrint('gvAwardAndCertificate')", "Print Grid", "icon_print.png", "PRINT");
			PreSeaAwardandCertificate.AccessRights = this.ViewState;
			PreSeaAwardandCertificate.MenuList = toolbargrid.Show();

			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;


				ViewState["PAGENUMBERSUB"] = 1;
				ViewState["SORTEXPRESSIONSUB"] = null;
				ViewState["SORTDIRECTIONSUB"] = null;
				ViewState["CURRENTINDEXSUB"] = 1;

				ViewState["PAGENUMBERCERT"] = 1;
				ViewState["SORTEXPRESSIONCERT"] = null;
				ViewState["SORTDIRECTIONCERT"] = null;
				ViewState["CURRENTINDEXCERT"] = 1;

				SetPreSeaNewApplicantPrimaryDetails();

			}
			BindData();
			SetPageNavigator();
			BindAwardCertificate();
			SetPageNavigatorCert();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;

		}
	}
	protected void PreSeaAcademicMenu_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{
				string[] alColumns = { "FLDQUALIFICATIONNAME", "FLDPERCENTAGE", "FLDDATEOFPASS" };
				string[] alCaptions = { "Certificate", "Percentage", "Passed Date" };
				string sortexpression;
				int? sortdirection = null;

				sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
				if (ViewState["SORTDIRECTION"] != null)
					sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
				DataTable dt = PhoenixPreSeaNewApplicantAcademicQualification.ListPreSeaNewApplicantAcademic(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection), null);
				General.ShowExcel("Academics Qualification", dt, alColumns, alCaptions, sortdirection, sortexpression);
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
			string[] alColumns = { "FLDQUALIFICATIONNAME", "FLDPERCENTAGE", "FLDDATEOFPASS" };
			string[] alCaptions = { "Certificate", "Percentage", "Passed Date" };
			string academicid = null;
			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

			if (ViewState["EMPLOYEEACADEMICID"] != null)
			{
				academicid = ViewState["EMPLOYEEACADEMICID"].ToString();
			}
			DataTable dt = PhoenixPreSeaNewApplicantAcademicQualification.ListPreSeaNewApplicantAcademic(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection), null);
			iRowCount = dt.Rows.Count;
			iTotalPageCount = 1;

			DataSet ds = new DataSet();
			ds.Tables.Add(dt.Copy());
			General.SetPrintOptions("gvPreSeaAcademics", "Academics Qualification", alCaptions, alColumns, ds);

			if (dt.Rows.Count > 0)
			{

				gvPreSeaAcademics.DataSource = dt;
				gvPreSeaAcademics.DataBind();
			}
			else
			{
				ShowNoRecordsFound(dt, gvPreSeaAcademics);
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
	public void SetPreSeaNewApplicantPrimaryDetails()
	{
		try
		{
			DataTable dt = PhoenixPreSeaTrainee.PreSeaTraineeEdit(General.GetNullableInteger(Filter.CurrentPreSeaTraineeSelection));
			if (dt.Rows.Count > 0)
			{
				txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
				txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
				txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
				txtBatch.Text = dt.Rows[0]["FLDBATCHNAME"].ToString();
				ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	protected void gvPreSeaAcademics_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
			string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
			e.Row.Attributes["onclick"] = _jsDouble;
		}
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
			{
				ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
				db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
				Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
				ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
				if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
				ImageButton cme = (ImageButton)e.Row.FindControl("cmdEdit");
				if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
				Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");
				if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
				att.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
					+ PhoenixModule.PRESEA + "&type=" + PhoenixPreSeaAttachmentType.ACADEMICS + "&cmdname=COURSEUPLOAD'); return false;");

				Label l = (Label)e.Row.FindControl("lblAcademicsId");
				Label lblAcademictype = (Label)e.Row.FindControl("lblAcademicTYPE");


				LinkButton lb = (LinkButton)e.Row.FindControl("lblAcademicsname");
				lb.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', 'PreSeaNewApplicantAcademicDetail.aspx?empid=" + Filter.CurrentPreSeaTraineeSelection + "&ACADEMICID=" + l.Text + "&ACADEMICTYPE=" + lblAcademictype.Text + "');return false;");

				ImageButton db1 = (ImageButton)e.Row.FindControl("cmdEdit");
				db1.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '','PreSeaNewApplicantAcademicDetail.aspx?empid=" + Filter.CurrentPreSeaTraineeSelection + "&ACADEMICID=" + l.Text + "&ACADEMICTYPE=" + lblAcademictype.Text + "');return false;");

			}
		}

	}
	protected void gvPreSeaAcademics_RowEditing(object sender, GridViewEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.NewEditIndex;
			string academicid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAcademicsId")).Text;

			_gridView.SelectedIndex = nCurrentRow;

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
	private void ResetFormControlValues(Control parent)
	{

		try
		{
			ViewState["EMPLOYEEACADEMICID"] = null;

			foreach (Control c in parent.Controls)
			{
				if (c.Controls.Count > 0)
				{
					ResetFormControlValues(c);
				}
				else
				{
					switch (c.GetType().ToString())
					{
						case "System.Web.UI.WebControls.TextBox":
							((TextBox)c).Text = "";
							break;
						case "System.Web.UI.WebControls.CheckBox":
							((CheckBox)c).Checked = false;
							break;
						case "System.Web.UI.WebControls.RadioButton":
							((RadioButton)c).Checked = false;
							break;
						case "System.Web.UI.WebControls.DropDownList":
							((DropDownList)c).SelectedIndex = 0;
							break;
						case "System.Web.UI.WebControls.ListBox":
							((ListBox)c).SelectedIndex = 0;
							break;

					}
				}
			}
			SetPreSeaNewApplicantPrimaryDetails();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvPreSeaAcademics_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			string academicsid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAcademicsId")).Text;
			string academicstype = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAcademictype")).Text;

			PhoenixPreSeaNewApplicantAcademicQualification.DeletePreSeaNewApplicantAcademic(Convert.ToInt32(PhoenixSecurityContext.CurrentSecurityContext.UserCode)
															, Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
															, Convert.ToInt32(academicsid)
															, General.GetNullableInteger(academicstype)
															);
			BindData();
			_gridView.SelectedIndex = -1;
			ResetFormControlValues(this);
			SetPreSeaNewApplicantPrimaryDetails();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;

		}
	}
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();
		SetPageNavigator();
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
			gvPreSeaAcademics.SelectedIndex = -1;
			gvPreSeaAcademics.EditIndex = -1;
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
	private Boolean IsPreviousEnabledSub()
	{

		int iCurrentPageNumber;
		int iTotalPageCount;

		iCurrentPageNumber = (int)ViewState["PAGENUMBERSUB"];
		iTotalPageCount = (int)ViewState["TOTALPAGECOUNTSUB"];

		if (iTotalPageCount == 0)
			return false;

		if (iCurrentPageNumber > 1)
		{
			return true;
		}

		return false;

	}
	private Boolean IsNextEnabledSub()
	{

		int iCurrentPageNumber;
		int iTotalPageCount;

		iCurrentPageNumber = (int)ViewState["PAGENUMBERSUB"];
		iTotalPageCount = (int)ViewState["TOTALPAGECOUNTSUB"];

		if (iCurrentPageNumber < iTotalPageCount)
		{
			return true;
		}
		return false;
	}
	protected void AwardandCertificateMenu_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{
				int iRowCount = 0;
				int iTotalPageCount = 0;
				string[] alColumns = { "FLDROWNUMBER", "FLDCERTIFICATENAME", "FLDISSUEDATE", "FLDREMARKS" };
				string[] alCaptions = { "Sl No", "Award/Certificate", "Issue Date", "Remarks" };


				string sortexpression = (ViewState["SORTEXPRESSIONCERT"] == null) ? null : (ViewState["SORTEXPRESSIONCERT"].ToString());
				int? sortdirection = null;

				if (ViewState["SORTDIRECTIONCERT"] != null)
					sortdirection = Int32.Parse(ViewState["SORTDIRECTIONCERT"].ToString());


				if (ViewState["ROWCOUNTCERT"] == null || Int32.Parse(ViewState["ROWCOUNTCERT"].ToString()) == 0)
					iRowCount = 10;
				else
					iRowCount = Int32.Parse(ViewState["ROWCOUNTCERT"].ToString());

				DataSet ds = PhoenixPreSeaAwardAndCertificate.PreSeaAwardAndCertificateSearch(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
					 , sortexpression, sortdirection, (int)ViewState["PAGENUMBERCERT"], iRowCount, ref iRowCount, ref iTotalPageCount);

				General.ShowExcel("Award and Certificate", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}
	private void BindAwardCertificate()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;
			string[] alColumns = { "FLDROWNUMBER", "FLDCERTIFICATENAME", "FLDISSUEDATE", "FLDREMARKS" };
			string[] alCaptions = { "Sl No", "Award/Certificate", "Issue Date", "Remarks" };


			string sortexpression = (ViewState["SORTEXPRESSIONCERT"] == null) ? null : (ViewState["SORTEXPRESSIONCERT"].ToString());
			int? sortdirection = null;
			if (ViewState["SORTDIRECTIONCERT"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTIONCERT"].ToString());


			DataSet ds = PhoenixPreSeaAwardAndCertificate.PreSeaAwardAndCertificateSearch(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
					 , sortexpression, sortdirection, (int)ViewState["PAGENUMBERCERT"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

			General.SetPrintOptions("gvAwardAndCertificate", "Award and Certificate", alCaptions, alColumns, ds);

			if (ds.Tables[0].Rows.Count > 0)
			{

				gvAwardAndCertificate.DataSource = ds.Tables[0];
				gvAwardAndCertificate.DataBind();
			}
			else
			{
				ShowNoRecordsFound(ds.Tables[0], gvAwardAndCertificate);
			}

			ViewState["ROWCOUNTCERT"] = iRowCount;
			ViewState["TOTALPAGECOUNTCERT"] = iTotalPageCount;

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvAwardAndCertificate_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
		try
		{
			if (e.CommandName.ToString().ToUpper() == "ADD")
			{

				_gridView.EditIndex = -1;
				string certificate = ((DropDownList)_gridView.FooterRow.FindControl("ddlCertificateAdd")).SelectedValue;
				string dateofissue = ((UserControlDate)_gridView.FooterRow.FindControl("txtIssueDateAdd")).Text;
				string remarks = ((TextBox)_gridView.FooterRow.FindControl("txtRemarksAdd")).Text;

				if (!IsValidCertificate(certificate, dateofissue, remarks))
				{
					ucError.Visible = true;
					return;
				}
				PhoenixPreSeaAwardAndCertificate.InsertPreSeaAwardAndCertificate(
					  PhoenixSecurityContext.CurrentSecurityContext.UserCode
					, Convert.ToInt32(certificate)
					, General.GetNullableDateTime(dateofissue)
					, General.GetNullableString(remarks)
					, Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
					);

				BindAwardCertificate();
				SetPageNavigatorCert();
			}

		}
		catch (Exception ex)
		{
			ucError.HeaderMessage = "Please make the required correction";
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;

		}
	}
	protected void gvAwardAndCertificate_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.RowIndex;
		try
		{
			string awardid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAwardIdEdit")).Text;
			string certificate = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlCertificateEdit")).SelectedValue;
			string dateofissue = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtIssueDateEdit")).Text;
			string remarks = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtRemarksEdit")).Text;

			if (!IsValidCertificate(certificate, dateofissue, remarks))
			{
				ucError.Visible = true;
				return;
			}

			PhoenixPreSeaAwardAndCertificate.UpdatePreSeaAwardAndCertificate(
					  PhoenixSecurityContext.CurrentSecurityContext.UserCode
					, Convert.ToInt32(awardid)
					, Convert.ToInt32(certificate)
					, General.GetNullableDateTime(dateofissue)
					, General.GetNullableString(remarks)
					, Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
					);

			_gridView.EditIndex = -1;
			BindAwardCertificate();
			SetPageNavigatorCert();
		}
		catch (Exception ex)
		{
			ucError.HeaderMessage = "Please make the required correction";
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;

		}
	}
	private bool IsValidCertificate(string certificate, string issuedate, string remarks)
	{

		ucError.HeaderMessage = "Please provide the following required information";

		if (General.GetNullableInteger(certificate) == null)
			ucError.ErrorMessage = "Award/Certificate is required";

		if (General.GetNullableDateTime(issuedate) == null)
			ucError.ErrorMessage = "Issue Date is required";

		if (remarks.Trim() == "")
			ucError.ErrorMessage = "Remarks is required";

		return (!ucError.IsError);
	}
	protected void gvAwardAndCertificate_RowDataBound(object sender, GridViewRowEventArgs e)
	{

		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
			   && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
			{
				ImageButton db = (ImageButton)e.Row.FindControl("cmdXDelete");
				db.Attributes.Add("onclick", "return fnConfirmDelete(event)");

				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
				Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
				ImageButton att = (ImageButton)e.Row.FindControl("cmdXAtt");
				if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
				ImageButton cme = (ImageButton)e.Row.FindControl("cmdXEdit");
				if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
				Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");
				if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
				att.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
					+ PhoenixModule.PRESEA + "&type=" + PhoenixPreSeaAttachmentType.ACADEMICS + "&cmdname=AWARDANDCERTIFICATE'); return false;");

				Label lblawardid = (Label)e.Row.FindControl("lblAwardId");

			}
			DropDownList ddlcertificate = (DropDownList)e.Row.FindControl("ddlCertificateEdit");
			DataRowView drvCertificate = (DataRowView)e.Row.DataItem;
			if (ddlcertificate != null) ddlcertificate.SelectedValue = drvCertificate["FLDCERTIFICATE"].ToString();
		}

	}
	protected void gvAwardAndCertificate_RowEditing(object sender, GridViewEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;

			_gridView.EditIndex = e.NewEditIndex;
			_gridView.SelectedIndex = e.NewEditIndex;

			BindAwardCertificate();
			SetPageNavigatorCert();

		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvAwardAndCertificate_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			string awardid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblAwardId")).Text;

			PhoenixPreSeaAwardAndCertificate.DeletePreSeaAwardAndCertificate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
									Convert.ToInt32(awardid));
			BindAwardCertificate();
			SetPageNavigatorCert();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;

		}
	}
	protected void gvAwardAndCertificate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindAwardCertificate();
	}
	protected void cmdGoCert_Click(object sender, EventArgs e)
	{
		try
		{
			int result;
			if (Int32.TryParse(txtnopageCert.Text, out result))
			{
				ViewState["PAGENUMBERCERT"] = Int32.Parse(txtnopageCert.Text);

				if ((int)ViewState["TOTALPAGECOUNTCERT"] < Int32.Parse(txtnopageCert.Text))
					ViewState["PAGENUMBERCERT"] = ViewState["TOTALPAGECOUNTCERT"];


				if (0 >= Int32.Parse(txtnopage.Text))
					ViewState["PAGENUMBERCERT"] = 1;

				if ((int)ViewState["PAGENUMBERCERT"] == 0)
					ViewState["PAGENUMBERCERT"] = 1;

				txtnopageCert.Text = ViewState["PAGENUMBERCERT"].ToString();
			}
			BindAwardCertificate();
			SetPageNavigatorCert();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void PagerButtonClickCert(object sender, CommandEventArgs ce)
	{
		try
		{
			gvAwardAndCertificate.SelectedIndex = -1;
			gvAwardAndCertificate.EditIndex = -1;
			if (ce.CommandName == "prev")
				ViewState["PAGENUMBERCERT"] = (int)ViewState["PAGENUMBERCERT"] - 1;
			else
				ViewState["PAGENUMBERCERT"] = (int)ViewState["PAGENUMBERCERT"] + 1;

			BindAwardCertificate();
			SetPageNavigatorCert();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private void SetPageNavigatorCert()
	{
		try
		{
			cmdPreviousCert.Enabled = IsPreviousEnabledCert();
			cmdNextCert.Enabled = IsNextEnabledCert();
			lblPagenumberCert.Text = "Page " + ViewState["PAGENUMBERCERT"].ToString();
			lblPagesCert.Text = " of " + ViewState["TOTALPAGECOUNTCERT"].ToString() + " Pages. ";
			lblRecordsCert.Text = "(" + ViewState["ROWCOUNTCERT"].ToString() + " records found)";
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private Boolean IsPreviousEnabledCert()
	{

		int iCurrentPageNumber;
		int iTotalPageCount;

		iCurrentPageNumber = (int)ViewState["PAGENUMBERCERT"];
		iTotalPageCount = (int)ViewState["TOTALPAGECOUNTCERT"];

		if (iTotalPageCount == 0)
			return false;

		if (iCurrentPageNumber > 1)
		{
			return true;
		}

		return false;

	}
	private Boolean IsNextEnabledCert()
	{

		int iCurrentPageNumber;
		int iTotalPageCount;

		iCurrentPageNumber = (int)ViewState["PAGENUMBERCERT"];
		iTotalPageCount = (int)ViewState["TOTALPAGECOUNTCERT"];

		if (iCurrentPageNumber < iTotalPageCount)
		{
			return true;
		}
		return false;
	}
}
