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


public partial class PreSeaTraineeOtherDocument : PhoenixBasePage
{
	protected override void Render(HtmlTextWriter writer)
	{
		foreach (GridViewRow r in gvOtherDocument.Rows)
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
			toolbargrid.AddImageButton("../PreSea/PreSeaTraineeOtherDocument.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbargrid.AddImageLink("javascript:CallPrint('gvOtherDocument')", "Print Grid", "icon_print.png", "PRINT");
			//toolbargrid.AddImageLink("javascript:parent.Openpopup('NAFA','','CrewTravelDocumentArchived.aspx?empid=" + Filter.CurrentPreSeaTraineeSelection + "&type=2&t=p'); return false;", "Show Archived", "show-archive.png", "ARCHIVED");
			MenuPreSeaDocumentOther.AccessRights = this.ViewState;
			MenuPreSeaDocumentOther.MenuList = toolbargrid.Show();
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
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void PreSeaDocumentOtherMenu_TabStripCommand(object sender, EventArgs e)
	{
		try
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

		string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDPLACEOFISSUE", "FLDISSUINGAUTHORITY", "FLDREMARKS" };
		string[] alCaptions = { "Document Name", "Number", "Date of Issue", "Date of Expiry", "Place of Issue", "Issuing Authority", "Remarks" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		DataSet ds = PhoenixPreSeaTraineeTravelDocument.SearchTraineeOtherDocument(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection), null, 1,
																			  sortexpression, sortdirection,
																			(int)ViewState["PAGENUMBER"],
																			General.ShowRecords(null),
																			ref iRowCount,
																			ref iTotalPageCount);
		General.ShowExcel("Other Document", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
	}

	private void BindData()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;
			string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDDATEOFISSUE", "FLDDATEOFEXPIRY", "FLDPLACEOFISSUE", "FLDISSUINGAUTHORITY", "FLDREMARKS" };
			string[] alCaptions = { "Document Name", "Number", "Date of Issue", "Date of Expiry", "Place of Issue", "Issuing Authority", "Remarks" };

			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = null;
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


			DataSet ds = PhoenixPreSeaTraineeTravelDocument.SearchTraineeOtherDocument(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection), null, 1,
																			   sortexpression, sortdirection,
																			   (int)ViewState["PAGENUMBER"],
																			   General.ShowRecords(null),
																			   ref iRowCount,
																			   ref iTotalPageCount);

			General.SetPrintOptions("gvOtherDocument", "Other Documnet", alCaptions, alColumns, ds);


			if (ds.Tables[0].Rows.Count > 0)
			{

				gvOtherDocument.DataSource = ds;
				gvOtherDocument.DataBind();
			}
			else
			{

				DataTable dt = ds.Tables[0];
				ShowNoRecordsFound(dt, gvOtherDocument);
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

	protected void gvOtherDocument_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
	}

	protected void gvOtherDocument_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToString().ToUpper() == "SORT") return;
			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
			if (e.CommandName.ToString().ToUpper() == "ADD")
			{

				_gridView.EditIndex = -1;
				_gridView.SelectedIndex = -1;
				string documenttype = ((UserControlDocumentType)_gridView.FooterRow.FindControl("ucDocumentTypeAdd")).SelectedDocumentType;
				string documentnumber = ((TextBox)_gridView.FooterRow.FindControl("txtNumberAdd")).Text;
				string dateofissue = ((UserControlDate)_gridView.FooterRow.FindControl("ucDateOfIssueAdd")).Text;
				string placeofissue = ((TextBox)_gridView.FooterRow.FindControl("txtPlaceIssueAdd")).Text;
				UserControlDate dateofexpiry = ((UserControlDate)_gridView.FooterRow.FindControl("ucDateExpiryAdd"));
				string issuingauthority = ((TextBox)_gridView.FooterRow.FindControl("txtIssuingAuthorityAdd")).Text;
				if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry))
				{
					ucError.Visible = true;
					if (_gridView.EditIndex > -1)
					{
						_gridView.EditIndex = -1;
						BindData();
					}
					return;
				}
				InsertTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry.Text, issuingauthority);
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
	protected void gvOtherDocument_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		try
		{

			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			string traveldocumentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lbltraveldocumentid")).Text;

			PhoenixPreSeaTraineeTravelDocument.DeleteTraineeTravelDocument(PhoenixSecurityContext.CurrentSecurityContext.UserCode
																, Convert.ToInt32(traveldocumentid)
																);
			_gridView.EditIndex = -1;
			_gridView.SelectedIndex = -1;
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;

		}
	}

	protected void gvOtherDocument_RowEditing(object sender, GridViewEditEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			if (_gridView.EditIndex >= 0)
			{
				int nCurrentRow = _gridView.EditIndex;
				string documenttype = ((UserControlDocumentType)_gridView.Rows[nCurrentRow].FindControl("ucDocumentTypeEdit")).SelectedDocumentType;
				string documentnumber = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNumberEdit")).Text;
				string dateofissue = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucDateOfIssueEdit")).Text;
				string placeofissue = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPlaceOfIssue")).Text;
				UserControlDate dateofexpiry = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucDateExpiryEdit"));
				string traveldocumentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lbltraveldocumentidEdit")).Text;

				if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry))
				{
					ucError.Visible = true;
					return;
				}
			}
			if (_gridView.EditIndex > -1)
				_gridView.UpdateRow(_gridView.EditIndex, false);

			_gridView.EditIndex = e.NewEditIndex;
			_gridView.SelectedIndex = e.NewEditIndex;
			BindData();
			//((TextBox)_gridView.Rows[e.NewEditIndex].FindControl("txtStockItemWantedQuantityEdit")).Focus();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvOtherDocument_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			
			LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
			string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
			e.Row.Attributes["onclick"] = _jsDouble;
		}
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
			  && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
			{
				ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
				if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

				ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
				db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
				if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
				Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
				ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
				if (att != null) att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
				Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");
				if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
				att.Attributes.Add("onclick", "javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
					+ PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=OTHERDOCUPLOAD'); return false;");

				Label lbl = (Label)e.Row.FindControl("lbltraveldocumentid");
				HtmlImage img = (HtmlImage)e.Row.FindControl("imgRemarks");
				img.Attributes.Add("onclick", "parent.Openpopup('MoreInfo','','PreSeaMoreInfo.aspx?id=" + lbl.Text + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "','xlarge')");
				Label lblR = (Label)e.Row.FindControl("lblRemarks");
				if (string.IsNullOrEmpty(lblR.Text.Trim())) img.Src = Session["images"] + "/no-remarks.png";

				System.Web.UI.WebControls.Image imgFlag = e.Row.FindControl("imgFlag") as System.Web.UI.WebControls.Image;
				Label expdate = e.Row.FindControl("lblDateofExpiry") as Label;
				DateTime? d = General.GetNullableDateTime(expdate.Text);
				if (d.HasValue)
				{
					TimeSpan t = d.Value - DateTime.Now;
					if (t.Days >= 0 && t.Days < 120)
					{
						
						imgFlag.Visible = true;
						imgFlag.ImageUrl = Session["images"] + "/yellow.png";
					}
					else if (t.Days < 0)
					{
						
						imgFlag.Visible = true;
						imgFlag.ImageUrl = Session["images"] + "/red.png";
					}
				}
			}
			else
			{
				e.Row.Attributes["onclick"] = "";
			}

			UserControlDocumentType ucDocumentType = (UserControlDocumentType)e.Row.FindControl("ucDocumentTypeEdit");
			DataRowView drv = (DataRowView)e.Row.DataItem;
			if (ucDocumentType != null) ucDocumentType.SelectedDocumentType = drv["FLDDOCUMENTTYPE"].ToString();

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
		if (e.Row.RowType == DataControlRowType.Footer)
		{
			ImageButton ad = (ImageButton)e.Row.FindControl("cmdAdd");
			if (ad != null) ad.Visible = SessionUtil.CanAccess(this.ViewState, ad.CommandName);
		}

	}

	protected void gvOtherDocument_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		int nCurrentRow = e.RowIndex;
		try
		{
			string documenttype = ((UserControlDocumentType)_gridView.Rows[nCurrentRow].FindControl("ucDocumentTypeEdit")).SelectedDocumentType;
			string documentnumber = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtNumberEdit")).Text;
			string dateofissue = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucDateOfIssueEdit")).Text;
			string placeofissue = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtPlaceOfIssue")).Text;
			UserControlDate dateofexpiry = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("ucDateExpiryEdit"));
			string issuingauthority = ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtIssuingAuthorityEdit")).Text;
			string traveldocumentid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lbltraveldocumentidEdit")).Text;
			if (!IsValidTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry))
			{
				ucError.Visible = true;
				return;
			}
			UpdateTravelDocument(documenttype, documentnumber, dateofissue, placeofissue, dateofexpiry.Text, issuingauthority, traveldocumentid);
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
		_gridView.EditIndex = -1;
		SetPageNavigator();
		BindData();
	}

	protected void gvOtherDocument_Sorting(object sender, GridViewSortEventArgs se)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		_gridView.SelectedIndex = -1;
		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
		SetPageNavigator();
	}
	protected void gvOtherDocument_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.SelectedIndex = e.NewSelectedIndex;
		_gridView.EditIndex = -1;
		BindData();
		SetPageNavigator();
	}
	public void InsertTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, string dateofexpiry, string issuingauthority)
	{
		PhoenixPreSeaTraineeTravelDocument.InsertTraineeOtherDocument(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
															, Convert.ToInt32(documenttype)
															, documentnumber
															, Convert.ToDateTime(dateofissue)
															, placeofissue
															, General.GetNullableDateTime(dateofexpiry)
															, issuingauthority
															);
	}
	public void UpdateTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, string dateofexpiry, string issuingauthority, string traveldocumentid)
	{
		PhoenixPreSeaTraineeTravelDocument.UpdateTraineeOtherDocument(Convert.ToInt32(Filter.CurrentPreSeaTraineeSelection)
															, Convert.ToInt32(traveldocumentid)
															, Convert.ToInt32(documenttype)
															, documentnumber
															, Convert.ToDateTime(dateofissue)
															, placeofissue
															, General.GetNullableDateTime(dateofexpiry)
															, issuingauthority
															);
	}

	private bool IsValidTravelDocument(string documenttype, string documentnumber, string dateofissue, string placeofissue, UserControlDate dateofexpiry)
	{

		ucError.HeaderMessage = "Please provide the following required information";
		Int16 result;
		DateTime resultdate;

		if (documenttype.Equals("") || !Int16.TryParse(documenttype, out result))
			ucError.ErrorMessage = "Document Type  is required";

		if (documentnumber.Trim() == "")
			ucError.ErrorMessage = "Document Number is required";

		if (!DateTime.TryParse(dateofissue, out resultdate))
			ucError.ErrorMessage = "Date of Issue is required";
		else if (DateTime.TryParse(dateofissue, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
		{
			ucError.ErrorMessage = "Date of Issue should be earlier than current date";
		}

		if (placeofissue.Trim() == "")
			ucError.ErrorMessage = "Place of Issue is required";

		if (!DateTime.TryParse(dateofexpiry.Text, out resultdate) && dateofexpiry.CssClass == "input_mandatory")
			ucError.ErrorMessage = "Expiry Date is required";
		else if (DateTime.TryParse(dateofissue, out resultdate)
			&& DateTime.TryParse(dateofexpiry.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(dateofissue)) < 0)
		{
			ucError.ErrorMessage = "Date of Expiry should be later than 'Date of Issue'";
		}

		return (!ucError.IsError);
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
			gvOtherDocument.SelectedIndex = -1;
			gvOtherDocument.EditIndex = -1;
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

	protected void ddlDocumentType_TextChanged(object sender, EventArgs e)
	{
		UserControlDocumentType dc = (UserControlDocumentType)sender;
		DataSet ds = new DataSet();
		UserControlDate date;
		if (dc.SelectedDocumentType != "Dummy")
		{
			ds = PhoenixRegistersDocumentOther.EditDocumentOther(General.GetNullableInteger(dc.SelectedDocumentType).Value);
		}
		if (dc.ID == "ucDocumentTypeAdd" && ds.Tables.Count > 0)
		{
			date = (UserControlDate)gvOtherDocument.FooterRow.FindControl("ucDateExpiryAdd");

		}
		else
		{
			GridViewRow row = ((GridViewRow)dc.Parent.Parent);
			date = (UserControlDate)gvOtherDocument.Rows[row.RowIndex].FindControl("ucDateExpiryEdit");
		}
		if (ds.Tables[0].Rows[0]["FLDHAVINGEXPIRY"].ToString() == "1")
			date.CssClass = "input_mandatory";
		else
			date.CssClass = "input";
	}
}
