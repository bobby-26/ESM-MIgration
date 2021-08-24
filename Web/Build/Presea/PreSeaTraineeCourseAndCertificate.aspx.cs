using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using System.Text;
using SouthNests.Phoenix.PreSea;
using Telerik.Web.UI;

public partial class PreSeaTraineeCourseAndCertificate : PhoenixBasePage
{
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
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddImageButton("../PreSea/PreSeaTraineeCourseAndCertificate.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbar.AddImageLink("javascript:CallPrint('gvTraineeCourseCertificate')", "Print Grid", "icon_print.png", "PRINT");
			toolbar.AddImageLink("javascript:parent.Openpopup('NAFA','','PreSeaTraineeCourseandCertificateArchived.aspx?empid=" + Filter.CurrentPreSeaTraineeSelection + "&type=1&t=p'); return false;", "Show Archived", "show-archive.png", "ARCHIVED");
			toolbar.AddImageButton("../PreSea/PreSeaTraineeCourseAndCertificate.aspx", "Email", "email.png", "CEmail");
			MenuPreSeaTraineeCourseCertificate.AccessRights = this.ViewState;
			MenuPreSeaTraineeCourseCertificate.MenuList = toolbar.Show();
			MenuPreSeaTraineeCourseCertificate.SetTrigger(pnlPreSeaTraineeCourseCertificateEntry);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");


			if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = 1;

				SetEmployeePrimaryDetails();
			}
			BindData();
			SetPageNavigator();
			SetAttachmentMarking();
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
	private void BindData()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;
			string[] alColumns = { "FLDCOURSENAME", "FLDCOURSENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDNAME", "FLDNATIONALITY", "FLDAUTHORITY", "FLDREMARKS" };
			string[] alCaptions = { "Course", "Certificate Number", "Place Of Issue", "Issue Date", "Expiry Date", "Institution", "Certificate Issuing Country", "Issuing Authority", "Remarks" };
			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;

			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

			DataSet ds = PhoenixPreSeaTraineeCourseAndCertificate.TraineeCourseCertificateSearch(
						Int32.Parse(Filter.CurrentPreSeaTraineeSelection.ToString()), 1
						, 0  // list courses other than cbt's
						, sortexpression, sortdirection
						, (int)ViewState["PAGENUMBER"]
						, General.ShowRecords(null)
						, ref iRowCount
						, ref iTotalPageCount);

			General.SetPrintOptions("gvTraineeCourseCertificate", "Crew Course And Certificate", alCaptions, alColumns, ds);

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
            RadComboBox institution = (RadComboBox)gvTraineeCourseCertificate.FooterRow.FindControl("ucInstitutionAdd").FindControl("ddlAddressType");
			institution.Attributes["style"] = "width:250px";
            RadComboBox course = (RadComboBox)gvTraineeCourseCertificate.FooterRow.FindControl("ddlCourseAdd").FindControl("ddlCourse");
			course.Attributes["style"] = "width:250px";
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void PreSeaTraineeCourseCertificate_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("EXCEL"))
			{
				int iRowCount = 0;
				int iTotalPageCount = 0;

				string[] alColumns = { "FLDCOURSENAME", "FLDCOURSENUMBER", "FLDPLACEOFISSUE", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDNAME", "FLDNATIONALITY", "FLDAUTHORITY", "FLDREMARKS" };
				string[] alCaptions = { "Course", "Certificate Number", "Place Of Issue", "Issue Date", "Expiry Date", "Institution", "Certificate Issuing Country", "Issuing Authority", "Remarks" };
				string sortexpression;
				int? sortdirection = null;

				sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

				if (ViewState["SORTDIRECTION"] != null)
					sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

				if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
					iRowCount = 10;
				else
					iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

				DataSet ds = PhoenixPreSeaTraineeCourseAndCertificate.TraineeCourseCertificateSearch(
								Int32.Parse(Filter.CurrentPreSeaTraineeSelection.ToString()), 1
								, 0  // list courses other than cbt's
								, sortexpression, sortdirection
								, 1
								, General.ShowRecords(null)
								, ref iRowCount
								, ref iTotalPageCount);
				if (ds.Tables.Count > 0)
					General.ShowExcel("Crew Course And Certificate", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
			}
			else if (dce.CommandName.ToUpper().Equals("CEMAIL"))
			{
				StringBuilder stbcourseid = new StringBuilder();

				CheckBox chk = (CheckBox)gvTraineeCourseCertificate.FindControl("chkChekedEmail");
				if (chk != null)
				{
					foreach (GridViewRow gvr in gvTraineeCourseCertificate.Rows)
					{

						if (((CheckBox)gvr.FindControl("chkChekedEmail")).Checked == true)
						{

							string courseid = ((Label)gvr.FindControl("lbldocid")).Text;
							stbcourseid.Append(courseid);
							stbcourseid.Append(",");

						}
					}
					if (stbcourseid.Length > 1)
					{
						stbcourseid.Remove(stbcourseid.Length - 1, 1);
					}
					string scriptRefreshDontClose = "";
					scriptRefreshDontClose += "<script language='javaScript' id='CrewEmail'>" + "\n";
					scriptRefreshDontClose += "parent.Openpopup('NAFA','','CrewEmail.aspx?csvcourseid=" + stbcourseid.ToString() + "&empid=" + Filter.CurrentPreSeaTraineeSelection + "&course=1')";
					scriptRefreshDontClose += "</script>" + "\n";
					ScriptManager.RegisterClientScriptBlock(this, typeof(Page), "CrewEmail", scriptRefreshDontClose, false);
				}
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvTraineeCourseCertificate_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
	}
	protected void gvTraineeCourseCertificate_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvTraineeCourseCertificate.EditIndex = -1;
		gvTraineeCourseCertificate.SelectedIndex = -1;
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}
	protected void gvTraineeCourseCertificate_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (e.CommandName.ToUpper().Equals("SORT"))
			return;
		GridView _gridView = (GridView)sender;
		int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
		try
		{
			if (e.CommandName.ToString().ToUpper() == "CADD")
			{
				_gridView.EditIndex = -1;
				string courseid = ((UserControlCourse)_gridView.FooterRow.FindControl("ddlCourseAdd")).SelectedCourse;
				string institutionid = ((UserControlAddressType)_gridView.FooterRow.FindControl("ucInstitutionAdd")).SelectedAddress;
				string certificatenumber = ((TextBox)_gridView.FooterRow.FindControl("txtCourseNumberAdd")).Text;
				string dateofissue = ((UserControlDate)_gridView.FooterRow.FindControl("txtIssueDateAdd")).Text;
				string placeofissue = ((TextBox)_gridView.FooterRow.FindControl("txtPlaceOfIssueAdd")).Text;
				UserControlDate dateofexpiry = ((UserControlDate)_gridView.FooterRow.FindControl("txtExpiryDateAdd"));
				string remarks = ((TextBox)_gridView.FooterRow.FindControl("txtRemarksAdd")).Text;
				string flagid = ((UserControlNationality)_gridView.FooterRow.FindControl("ddlFlagAdd")).SelectedNationality;
				string authority = ((TextBox)_gridView.FooterRow.FindControl("txtAuthorityAdd")).Text;

				if (!IsValidCourseCertificate(courseid, certificatenumber, dateofissue, dateofexpiry, placeofissue, institutionid, flagid))
				{
					ucError.Visible = true;
					return;
				}
				PhoenixPreSeaTraineeCourseAndCertificate.InsertTraineeCourseCertificate(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
					, Convert.ToInt32(courseid)
					, certificatenumber
					, General.GetNullableDateTime(dateofissue)
					, General.GetNullableDateTime(dateofexpiry.Text)
					, placeofissue
					, General.GetNullableInteger(institutionid)
					, General.GetNullableString(remarks)
					, General.GetNullableInteger(flagid)
					, General.GetNullableString(authority)
					);

				BindData();
				SetPageNavigator();
			}
			else if (e.CommandName.ToString().ToUpper() == "CARCHIVE")
			{
				string id = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTraineeCourseId")).Text;
				PhoenixPreSeaTraineeCourseAndCertificate.ArchiveTraineeCourseCertificate(int.Parse(Filter.CurrentPreSeaTraineeSelection), int.Parse(id), 0);
				_gridView.EditIndex = -1;
				_gridView.SelectedIndex = -1;
				BindData();
				SetPageNavigator();
			}
			else if (e.CommandName.ToString().ToUpper() == "CDELETE")
			{
				string coursecertificateid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTraineeCourseId")).Text;

				PhoenixPreSeaTraineeCourseAndCertificate.DeleteTraineeCourseCertificate(Convert.ToInt32(coursecertificateid)
																);
				BindData();
				SetPageNavigator();
			}
			else if (e.CommandName.ToString().ToUpper() == "CEDIT")
			{
				_gridView.SelectedIndex = nCurrentRow;
				_gridView.EditIndex = nCurrentRow;
				
				BindData();
                RadComboBox institution = (RadComboBox)_gridView.Rows[nCurrentRow].FindControl("ucInstitutionEdit").FindControl("ddlAddressType");
				institution.Attributes["style"] = "width:250px";
                RadComboBox course = (RadComboBox)_gridView.Rows[nCurrentRow].FindControl("ddlCourseEdit").FindControl("ddlCourse");
				course.Attributes["style"] = "width:250px";
				//((TextBox)_gridView.Rows[e.NewEditIndex].FindControl("lblVISAIdEdit")).Focus();
				SetPageNavigator();
			}
			else if (e.CommandName.ToString().ToUpper() == "CUPDATE")
			{
				try
				{
					string courseid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTraineeCourseIdEdit")).Text;
					string institutionid = ((UserControlAddressType)_gridView.Rows[nCurrentRow].FindControl("ucInstitutionEdit")).SelectedAddress;
					string certificatenumber = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCourseNumberEdit")).Text;
					string dateofissue = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtIssueDateEdit")).Text;
					string placeofissue = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPlaceOfIssueEdit")).Text;
					UserControlDate dateofexpiry = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtExpiryDateEdit"));
					string coursecertificateid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTraineeCourseIdEdit")).Text;
					string flagid = ((UserControlNationality)_gridView.Rows[nCurrentRow].FindControl("ddlFlagEdit")).SelectedNationality;
					string authority = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAuthorityEdit")).Text;
					if (!IsValidateGrid(_gridView, nCurrentRow))
					{
						ucError.Visible = true;
						return;
					}

					PhoenixPreSeaTraineeCourseAndCertificate.UpdateTraineeCourseCertificate(Convert.ToInt32(coursecertificateid)
						, Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
						, Convert.ToInt32(courseid)
						, certificatenumber
						, General.GetNullableDateTime(dateofissue)
						, General.GetNullableDateTime(dateofexpiry.Text)
						, placeofissue
						, General.GetNullableInteger(institutionid)
						, General.GetNullableInteger(flagid)
						, General.GetNullableString(authority)
					   );

					_gridView.EditIndex = -1;
					BindData();
					SetPageNavigator();
				}
				catch (Exception ex)
				{
					ucError.HeaderMessage = "Please make the required correction";
					ucError.ErrorMessage = ex.Message;
					ucError.Visible = true;
				}
			}
			else if (e.CommandName.ToString().ToUpper() == "CCANCEL")
			{

				_gridView.EditIndex = -1;
				BindData();
			}
		}
		catch (Exception ex)
		{
			ucError.HeaderMessage = "Please make the required correction";
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
			string lblTraineeCourseId = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTraineeCourseId")).Text;

			PhoenixPreSeaTraineeCourseAndCertificate.DeleteTraineeCourseCertificate(General.GetNullableInteger(lblTraineeCourseId).Value);
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvTraineeCourseCertificate_RowEditing(object sender, GridViewEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;

			if (_gridView.EditIndex > -1)
			{
				if (!IsValidateGrid(_gridView, _gridView.EditIndex))
				{
					ucError.Visible = true;
					return;
				}
				_gridView.UpdateRow(_gridView.EditIndex, false);
			}
			_gridView.SelectedIndex = e.NewEditIndex;
			_gridView.EditIndex = e.NewEditIndex;
			BindData();
            RadComboBox institution = (RadComboBox)_gridView.Rows[e.NewEditIndex].FindControl("ucInstitutionEdit").FindControl("ddlAddressType");
			institution.Attributes["style"] = "width:250px";
            RadComboBox course = (RadComboBox)_gridView.Rows[e.NewEditIndex].FindControl("ddlCourseEdit").FindControl("ddlCourse");
			course.Attributes["style"] = "width:250px";
			//((TextBox)_gridView.Rows[e.NewEditIndex].FindControl("lblVISAIdEdit")).Focus();
			SetPageNavigator();
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
			try
			{
				if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
					&& !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
				{

					ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
					db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
					if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
					ImageButton db1 = (ImageButton)e.Row.FindControl("cmdArchive");
					db1.Attributes.Add("onclick", "return fnConfirmDelete(event,'Archive selected document ?')");
					if (!SessionUtil.CanAccess(this.ViewState, db1.CommandName)) db1.Visible = false;
					Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
					ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
					if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
					ImageButton cme = (ImageButton)e.Row.FindControl("cmdEdit");
					if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
					Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");
					if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
					att.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
						+ PhoenixModule.PRESEA + "&type=" + PhoenixCrewAttachmentType.COURSE + "&cmdname=COURSEUPLOAD'); return false;");
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
					Label lbl = (Label)e.Row.FindControl("lblTraineeCourseId");
					HtmlImage img = (HtmlImage)e.Row.FindControl("imgRemarks");
					img.Attributes.Add("onclick", "parent.Openpopup('MoreInfo','','PreSeaMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.COURSE + "','xlarge')");
					Label lblR = (Label)e.Row.FindControl("lblRemarks");
					if (string.IsNullOrEmpty(lblR.Text.Trim())) img.Src = Session["images"] + "/no-remarks.png";

				}
				else
					e.Row.Attributes["onclick"] = "";

				UserControlAddressType ucAddressType = (UserControlAddressType)e.Row.FindControl("ucInstitutionEdit");
				DataRowView drv = (DataRowView)e.Row.DataItem;
				if (ucAddressType != null) ucAddressType.SelectedAddress = drv["FLDINSTITUTIONID"].ToString();

				UserControlCourse ucCourse = (UserControlCourse)e.Row.FindControl("ddlCourseEdit");
				DataRowView drvCourse = (DataRowView)e.Row.DataItem;
				if (ucCourse != null) ucCourse.SelectedCourse = drvCourse["FLDCOURSE"].ToString();

				UserControlNationality ucFlag = (UserControlNationality)e.Row.FindControl("ddlFlagEdit");
				DataRowView drvFlag = (DataRowView)e.Row.DataItem;
				if (ucFlag != null) ucFlag.SelectedNationality = drvFlag["FLDFLAGID"].ToString();
			}
			catch (Exception ex)
			{
				ucError.HeaderMessage = "Please make the required correction";
				ucError.ErrorMessage = ex.Message;
				ucError.Visible = true;

			}
		}
	
		if (e.Row.RowType == DataControlRowType.Footer)
		{
			ImageButton imgbt = (ImageButton)e.Row.FindControl("cmdAdd");
			if (!SessionUtil.CanAccess(this.ViewState, imgbt.CommandName)) imgbt.Visible = false;
		}
	}

	protected void gvTraineeCourseCertificate_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.RowIndex;
		try
		{
			string courseid = ((UserControlCourse)_gridView.Rows[nCurrentRow].FindControl("ddlCourseEdit")).SelectedCourse;
			string institutionid = ((UserControlAddressType)_gridView.Rows[nCurrentRow].FindControl("ucInstitutionEdit")).SelectedAddress;
			string certificatenumber = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCourseNumberEdit")).Text;
			string dateofissue = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtIssueDateEdit")).Text;
			string placeofissue = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPlaceOfIssueEdit")).Text;
			UserControlDate dateofexpiry = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtExpiryDateEdit"));
			string coursecertificateid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseIdEdit")).Text;
			string flagid = ((UserControlNationality)_gridView.Rows[nCurrentRow].FindControl("ddlFlagEdit")).SelectedNationality;
			string authority = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtAuthorityEdit")).Text;
			if (!IsValidateGrid(_gridView, nCurrentRow))
			{
				ucError.Visible = true;
				return;
			}

			PhoenixPreSeaTraineeCourseAndCertificate.UpdateTraineeCourseCertificate(Convert.ToInt32(coursecertificateid)
				, Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
				, Convert.ToInt32(courseid)
				, certificatenumber
				, General.GetNullableDateTime(dateofissue)
				, General.GetNullableDateTime(dateofexpiry.Text)
				, placeofissue
				, General.GetNullableInteger(institutionid)
				, General.GetNullableInteger(flagid)
				, General.GetNullableString(authority)
			   );

			_gridView.EditIndex = -1;
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.HeaderMessage = "Please make the required correction";
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;

		}
	}

	protected void gvTraineeCourseCertificate_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.SelectedIndex = e.NewSelectedIndex;
		_gridView.EditIndex = -1;
		//  BindData();
		// SetPageNavigator();
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
			gv.Rows[0].Attributes["onclick"] = "";
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void cmdSort_Click(object sender, EventArgs e)
	{
		try
		{
			ImageButton ib = (ImageButton)sender;

			ViewState["SORTEXPRESSION"] = ib.CommandName;
			ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
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

	private bool IsValidCourseCertificate(string courseid, string certificatenumber, string dateofissue, UserControlDate dateofexpiry, string placeofissue, string institutionid, string flagid)
	{
		Int16 resultInt;
		DateTime resultDate;

		ucError.HeaderMessage = "Please provide the following required information";

		if (!Int16.TryParse(courseid, out resultInt))
			ucError.ErrorMessage = "Course is required";


		if (!Int16.TryParse(institutionid, out resultInt))
			ucError.ErrorMessage = "Institution is required";

		if (!Int16.TryParse(flagid, out resultInt))
			ucError.ErrorMessage = "Certificate Issuing Country is required";

		if (certificatenumber.Trim() == "")
			ucError.ErrorMessage = "Certificate Number is required";

		if (placeofissue.Trim() == "")
			ucError.ErrorMessage = "Place of issue is required";

		if (string.IsNullOrEmpty(dateofissue))
			ucError.ErrorMessage = "Issue Date is required.";
		else if (DateTime.TryParse(dateofissue, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
		{
			ucError.ErrorMessage = "Issue Date should be earlier than current date";
		}

		if (string.IsNullOrEmpty(dateofexpiry.Text) && dateofexpiry.CssClass == "input_mandatory")
			ucError.ErrorMessage = "Expiry Date is required.";

		if (dateofissue != null && dateofexpiry.Text != null)
		{
			if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry.Text, out resultDate)))
				if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry.Text)))
					ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
		}

		return (!ucError.IsError);
	}

	private bool IsValidateGrid(GridView _gridView, int nCurrentRow)
	{
		string courseid = ((UserControlCourse)_gridView.Rows[nCurrentRow].FindControl("ddlCourseEdit")).SelectedCourse;
		string certificatenumber = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCourseNumberEdit")).Text;
		string dateofissue = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtIssueDateEdit")).Text;
		string placeofissue = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPlaceOfIssueEdit")).Text;
		UserControlDate dateofexpiry = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtExpiryDateEdit"));
		string coursecertificateid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTraineeCourseIdEdit")).Text;
		string institutionid = ((UserControlAddressType)_gridView.Rows[nCurrentRow].FindControl("ucInstitutionEdit")).SelectedAddress;
		string flagid = ((UserControlNationality)_gridView.Rows[nCurrentRow].FindControl("ddlFlagEdit")).SelectedNationality;
		return IsValidCourseCertificate(courseid, certificatenumber, dateofissue, dateofexpiry, placeofissue, institutionid, flagid);
	}

	public void SetEmployeePrimaryDetails()
	{
		try
		{
			DataTable dt = PhoenixPreSeaTrainee.PreSeaTraineeEdit(General.GetNullableInteger(Filter.CurrentPreSeaTraineeSelection));

			if (dt.Rows.Count > 0)
			{
				txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
				txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
				txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
				txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
				ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	public StateBag ReturnViewState()
	{
		return ViewState;
	}

	protected void ddlDocument_TextChanged(object sender, EventArgs e)
	{
		UserControlCourse dc = (UserControlCourse)sender;
		DataSet ds = new DataSet();
		UserControlDate expirydate;
		//UserControlDate issuedate;
		if (dc.SelectedCourse != "Dummy")
		{
			ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(General.GetNullableInteger(dc.SelectedCourse).Value);
		}
		if (ds.Tables.Count > 0)
		{
			if (dc.ID == "ddlCourseAdd")
			{
				expirydate = (UserControlDate)gvTraineeCourseCertificate.FooterRow.FindControl("txtExpiryDateAdd");
				//issuedate = (UserControlDate)gvTraineeCourseCertificate.FooterRow.FindControl("txtIssueDateAdd");
			}
			else
			{
				GridViewRow row = ((GridViewRow)dc.Parent.Parent);
				expirydate = (UserControlDate)gvTraineeCourseCertificate.Rows[row.RowIndex].FindControl("txtExpiryDateEdit");
				//issuedate = (UserControlDate)gvTraineeCourseCertificate.Rows[row.RowIndex].FindControl("txtIssueDateEdit");
			}
			if (ds.Tables[0].Rows[0]["FLDEXPIRY"].ToString() == "1")
			{
				expirydate.CssClass = "input_mandatory";
				//issuedate.CssClass = "input_mandatory";
			}
			else
			{
				expirydate.CssClass = "input";
				//issuedate.CssClass = "input";
			}
		}
	}

	private void SetAttachmentMarking()
	{
        string attachmentcode = "00000000-0000-0000-0000-000000000000";
        if (ViewState["attachmentcode"] != null)
            attachmentcode = ViewState["attachmentcode"].ToString();
        DataTable dt1 = PhoenixCommonFileAttachment.AttachmentList(new Guid(attachmentcode), PhoenixCrewAttachmentType.COURSE.ToString());
		if (dt1.Rows.Count > 0)
		{
			imgClip.Visible = true;
			imgClip.Attributes["onclick"] = "javascript:parent.Openpopup('NAA','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
				   + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&cmdname=COURSEUPLOAD'); return false;";
		}
		else
			imgClip.Visible = false;
	}

    protected void gvTraineeCourseCertificate_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;

            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 5;
            HeaderGridRow.Cells.Add(HeaderCell);
            HeaderCell = new TableCell();
            HeaderCell.Text = "Date of";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 2;
            HeaderGridRow.Cells.Add(HeaderCell);
            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 4;
            HeaderGridRow.Cells.Add(HeaderCell);
            HeaderCell.Text = "Certificate Issuing";

            HeaderCell = new TableCell();
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 2;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvTraineeCourseCertificate.Controls[0].Controls.AddAt(0, HeaderGridRow);
            GridViewRow row = ((GridViewRow)gvTraineeCourseCertificate.Controls[0].Controls[0]);
            row.Attributes.Add("style", "position:static");
        }
    }
}
