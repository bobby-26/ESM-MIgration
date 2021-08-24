using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;

public partial class RegistersCourseCertificates : PhoenixBasePage
{
	protected override void Render(HtmlTextWriter writer)
	{
		foreach (GridViewRow r in gvCourseCertificates.Rows)
		{
			if (r.RowType == DataControlRowType.DataRow)
			{
				Page.ClientScript.RegisterForEventValidation(gvCourseCertificates.UniqueID, "Select$" + r.RowIndex.ToString());
			}
		}
		base.Render(writer);
	}

	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
		PhoenixToolbar toolbar = new PhoenixToolbar();
		toolbar.AddImageButton("../Registers/RegistersCourseCertificates.aspx", "Export to Excel", "icon_xls.png", "Excel");
		toolbar.AddImageLink("javascript:CallPrint('gvCourseCertificates')", "Print Grid", "icon_print.png", "PRINT");
		MenuRegistersCourseCertificates.AccessRights = this.ViewState;
		MenuRegistersCourseCertificates.MenuList = toolbar.Show();

		if (!IsPostBack)
		{
	
			ViewState["PAGENUMBER"] = 1;
			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
			ViewState["CURRENTINDEX"] = 1;
			if (Request.QueryString["courseid"] != null)
			{
				Session["COURSEID"] = Request.QueryString["courseid"];
			}
			if (Session["COURSEID"] != null)
			{
				EditCourse();
			}
		}
		BindData();
		SetPageNavigator();
	}
	protected void EditCourse()
	{
		try
		{
			int courseid = Convert.ToInt32(Session["COURSEID"].ToString());
			DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(courseid);
			if (ds.Tables[0].Rows.Count > 0)
			{
				ucCourse.SelectedCourse = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
				ucCourseType.SelectedHard = ds.Tables[0].Rows[0]["FLDDOCUMENTTYPE"].ToString();
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

		DataSet ds = new DataSet();
		string[] alColumns = { "FLDCERTIFICATE", "FLDCERTIFICATENO", "FLDISSUEDATE", "FLDEXPIRYDATE" };
		string[] alCaptions = { "Certificate", "Number", "Issue Date", "Expiry Date" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		ds = PhoenixRegistersCourseCertificate.CourseCertificateSearch(
			 Convert.ToInt32(Session["COURSEID"].ToString()),
			 null,null,null,
			sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
			iRowCount,
			ref iRowCount,
			ref iTotalPageCount);

		Response.AddHeader("Content-Disposition", "attachment; filename=CourseCertificates.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Course Certificates</h3></td>");
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

	protected void RegistersCourseCertificates_TabStripCommand(object sender, EventArgs e)
	{
		DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
		if (dce.CommandName.ToUpper().Equals("EXCEL"))
		{
			ShowExcel();
		}
	}

	private void BindData()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;

		string[] alColumns = { "FLDCERTIFICATE", "FLDCERTIFICATENO", "FLDISSUEDATE", "FLDEXPIRYDATE" };
		string[] alCaptions = { "Certificate", "Number", "Issue Date", "Expiry Date"};

		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		DataSet ds = PhoenixRegistersCourseCertificate.CourseCertificateSearch(
				 Convert.ToInt32(Session["COURSEID"].ToString()),
				 null, null, null,
				sortexpression, sortdirection,
				Int32.Parse(ViewState["PAGENUMBER"].ToString()),
				General.ShowRecords(null),
				ref iRowCount,
				ref iTotalPageCount);

		

		General.SetPrintOptions("gvCourseCertificates", "Certificates", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvCourseCertificates.DataSource = ds;
			gvCourseCertificates.DataBind();
		}
		else
		{
			DataTable dt = ds.Tables[0];
			ShowNoRecordsFound(dt, gvCourseCertificates);
		}
		DropDownList ddlCertificate = (DropDownList)gvCourseCertificates.FooterRow.FindControl("ucCertificateAdd");
		ddlCertificate.DataSource= PhoenixRegistersCourseCertificate.ListCourseCertificate(null);
		ddlCertificate.DataTextField = "FLDNAME";
		ddlCertificate.DataValueField = "FLDCERTIFICATEID";
		ddlCertificate.DataBind();

		ViewState["ROWCOUNT"] = iRowCount;
		ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

	}

	protected void cmdSort_Click(object sender, EventArgs e)
	{
		ImageButton ib = (ImageButton)sender;

		ViewState["SORTEXPRESSION"] = ib.CommandName;
		ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);

	}

	protected void gvCourseCertificates_RowCreated(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
			&& e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
		{
			e.Row.TabIndex = -1;
			e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCourseCertificates, "Select$" + e.Row.RowIndex.ToString(), false);
		}
	}

	protected void gvCourseCertificates_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvCourseCertificates.SelectedIndex = -1;
		gvCourseCertificates.EditIndex = -1;

		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		BindData();
	}

	protected void gvCourseCertificates_RowUpdating(object sender, GridViewUpdateEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			string type = "0";
			string certificateid = ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ucCertificateEdit")).SelectedValue;
			DataSet dscer = PhoenixRegistersCourseCertificate.ListCourseCertificate(General.GetNullableInteger(certificateid));
			if (dscer.Tables[0].Rows.Count > 0)
			{
				 type = dscer.Tables[0].Rows[0]["FLDTYPE"].ToString();
			}

			UpdateCourseCertificate(
					((Label)_gridView.Rows[nCurrentRow].FindControl("lblCourseCertificateIdEdit")).Text
					, Session["COURSEID"].ToString()
					, ((DropDownList)_gridView.Rows[nCurrentRow].FindControl("ucCertificateEdit")).SelectedValue
					, ((TextBox)_gridView.Rows[nCurrentRow].FindControl("txtCertificateNumberEdit")).Text
					, ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDateOfIssueEdit")).Text
					, ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtDateOfExpiryEdit")).Text
					, Convert.ToInt32(type));

			_gridView.EditIndex = -1;
			BindData();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvCourseCertificates_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
	{
		gvCourseCertificates.SelectedIndex = e.NewSelectedIndex;
	}

	protected void gvCourseCertificates_RowEditing(object sender, GridViewEditEventArgs de)
	{
		try
		{
			GridView _gridView = (GridView)sender;

			_gridView.EditIndex = de.NewEditIndex;
			_gridView.SelectedIndex = de.NewEditIndex;

			BindData();
			((TextBox)_gridView.Rows[de.NewEditIndex].FindControl("txtCertificateNumberEdit")).Focus();
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvCourseCertificates_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		try
		{
			if (e.CommandName.ToUpper().Equals("SORT"))
				return;

			GridView _gridView = (GridView)sender;
			int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

			if (e.CommandName.ToUpper().Equals("ADD"))
			{
				string type = "0";
				string certificateid= ((DropDownList)_gridView.FooterRow.FindControl("ucCertificateAdd")).SelectedValue;
				DataSet dscer = PhoenixRegistersCourseCertificate.ListCourseCertificate(General.GetNullableInteger(certificateid));
				if (dscer.Tables[0].Rows.Count > 0)
				{
					type = dscer.Tables[0].Rows[0]["FLDTYPE"].ToString();
				}

				InsertCourseCertificate(
					    Session["COURSEID"].ToString()
					,  ((DropDownList)_gridView.FooterRow.FindControl("ucCertificateAdd")).SelectedValue
					, ((TextBox)_gridView.FooterRow.FindControl("txtCertificateNumberAdd")).Text
					, ((UserControlDate)_gridView.FooterRow.FindControl("txtDateOfIssueAdd")).Text
					, ((UserControlDate)_gridView.FooterRow.FindControl("txtDateOfExpiryAdd")).Text
					, Convert.ToInt32(type)
					);

				BindData();
				((DropDownList)_gridView.FooterRow.FindControl("ucCertificateAdd")).Focus();
			}

			else if (e.CommandName.ToUpper().Equals("DELETE"))
			{
				DeleteCourseCertificate(Int32.Parse(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCertificateId")).Text));
			}
			
			SetPageNavigator();
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void gvCourseCertificates_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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

		if (e.Row.RowType == DataControlRowType.Footer)
		{
			ImageButton db = (ImageButton)e.Row.FindControl("cmdAdd");
			if (db != null)
			{
				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
					db.Visible = false;
			}
		}

		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			ImageButton del = (ImageButton)e.Row.FindControl("cmdDelete");
			if (del != null)
			{
				del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
			}

			ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
			if (edit != null)
			{
				edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);
			}

			ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
			if (save != null)
			{
				save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);
			}

			ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
			if (cancel != null)
			{
				cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);
			}

			ImageButton add = (ImageButton)e.Row.FindControl("cmdAtt");
			if (add != null)
			{
				add.Visible = SessionUtil.CanAccess(this.ViewState, add.CommandName);
			}

			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
				&& !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
			{
				ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
				if (db != null)
				{
					db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
				}

				LinkButton lb = (LinkButton)e.Row.FindControl("lnkCertificateName");
				if (lb != null)
				{
					if (!SessionUtil.CanAccess(this.ViewState, lb.CommandName))
						lb.CommandName = "";
				}

				Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
				ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
				Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");
				if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
				att.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
					+ PhoenixModule.REGISTERS + "'); return false;");

				Label expdate = e.Row.FindControl("lblDateOfExpiry") as Label;
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
		
			DropDownList ucCertificate = (DropDownList)e.Row.FindControl("ucCertificateEdit");
			DataRowView drv = (DataRowView)e.Row.DataItem;
			if (ucCertificate != null) ucCertificate.SelectedValue = drv["FLDCERTIFICATEID"].ToString();

			
		}
	}

	protected void gvCourseCertificates_RowDeleting(object sender, GridViewDeleteEventArgs de)
	{
		BindData();
		SetPageNavigator();
	}

	protected void gvCourseCertificates_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
	{
		GridView _gridView = (GridView)sender;
		_gridView.EditIndex = -1;
		BindData();
	}

	protected void cmdSearch_Click(object sender, EventArgs e)
	{
		gvCourseCertificates.SelectedIndex = -1;
		gvCourseCertificates.EditIndex = -1;
		BindData();
		SetPageNavigator();
	}

	protected void cmdGo_Click(object sender, EventArgs e)
	{
		int result;

		gvCourseCertificates.SelectedIndex = -1;
		gvCourseCertificates.EditIndex = -1;

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

	public void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		BindData();
		SetPageNavigator();
	}

	protected void PagerButtonClick(object sender, CommandEventArgs ce)
	{
		gvCourseCertificates.SelectedIndex = -1;
		gvCourseCertificates.EditIndex = -1;

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

	private void InsertCourseCertificate(
		  string courseid
		, string certificatesid
		, string certificatesno
		, string dateofissue
		, string dateofexpiry
		, int	 type
		)
	{

		if (!IsValidCourseCertificate(certificatesid, certificatesno, dateofissue, dateofexpiry))
		{
			ucError.Visible = true;
			return;
		}
		else
		{
			
			PhoenixRegistersCourseCertificate.InsertCourseCertificate(
				 Convert.ToInt32(Session["COURSEID"].ToString())
				, Convert.ToInt64(certificatesid)
				, null
				, certificatesno
				, General.GetNullableDateTime(dateofissue)
				, General.GetNullableDateTime(dateofexpiry)
				, type
				);
		}
	}

	private void UpdateCourseCertificate(
		  string coursecertificateid
		, string courseid
		, string certificatesid
		, string certificatesno
		, string dateofissue
		, string dateofexpiry
		, int	 type )
	{

		if (!IsValidCourseCertificate(certificatesid, certificatesno, dateofissue, dateofexpiry))
		{
			ucError.Visible = true;
			return;
		}
		PhoenixRegistersCourseCertificate.UpdateCourseCertificate(
				 Convert.ToInt16(coursecertificateid)
				, Convert.ToInt32(Session["COURSEID"].ToString())
				, Convert.ToInt64(certificatesid)
				, null
				, certificatesno
				, General.GetNullableDateTime(dateofissue)
				, General.GetNullableDateTime(dateofexpiry)
				, type);

		ucStatus.Text = "Course Certificate information updated successfully";
	}

	private bool IsValidCourseCertificate(
		  string certificatesid
		, string certificateno
		, string dateofissue
		, string dateofexpiry)
	{
		DateTime resultDate;
		Int16 resultInt;

		ucError.HeaderMessage = "Please provide the following required information";

		if (!Int16.TryParse(certificatesid, out resultInt))
			ucError.ErrorMessage = "Certificate  is required.";

		if (certificateno.Trim().Equals(""))
			ucError.ErrorMessage = " Number is required.";

		if (!DateTime.TryParse(dateofissue, out resultDate))
			ucError.ErrorMessage = "Valid Issue Date is required.";

		if (dateofexpiry != null && !DateTime.TryParse(dateofexpiry, out resultDate))
			ucError.ErrorMessage = "Valid Expiry Date is required.";

		if (dateofissue != null && dateofexpiry != null)
		{
			if ((DateTime.TryParse(dateofissue, out resultDate)) && (DateTime.TryParse(dateofexpiry, out resultDate)))
				if ((DateTime.Parse(dateofissue)) >= (DateTime.Parse(dateofexpiry)))
					ucError.ErrorMessage = "'Expiry Date' should be greater than 'Issue Date'";
		}

		return (!ucError.IsError);
	}

	private void DeleteCourseCertificate(int certificatesid)
	{
		PhoenixRegistersCourseCertificate.DeleteCourseCertificate(certificatesid);
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
