using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaTraineeCorrespondence : PhoenixBasePage
{
	protected override void Render(HtmlTextWriter writer)
	{
		foreach (GridViewRow r in gvCorrespondence.Rows)
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
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

			PhoenixToolbar toolbargrid = new PhoenixToolbar();
			toolbargrid.AddImageButton("../PreSea/PreSeaTraineeCorrespondence.aspx", "Export to Excel", "icon_xls.png", "Excel");
			toolbargrid.AddImageLink("javascript:CallPrint('gvCorrespondence')", "Print Grid", "icon_print.png", "PRINT");			
			MenuPreSeaCorrespondence.AccessRights = this.ViewState;
			MenuPreSeaCorrespondence.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("New", "NEW");            
            MenuCorrespondence.AccessRights = this.ViewState;
            MenuCorrespondence.MenuList = toolbar.Show();

            if (!IsPostBack)
			{				
				string strTraineeId = string.Empty;
				if (string.IsNullOrEmpty(Filter.CurrentPreSeaTraineeSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
					strTraineeId = string.Empty;
				else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
					strTraineeId = Request.QueryString["empid"];
				else if (!string.IsNullOrEmpty(Filter.CurrentPreSeaTraineeSelection))
					strTraineeId = Filter.CurrentPreSeaTraineeSelection;

				ViewState["EMPID"] = strTraineeId;
				DataTable dt = PhoenixPreSeaTraineePersonal.ListPreSeaTraineeAddress(int.Parse(strTraineeId));
				ViewState["EMPEMAIL"] = dt.Rows.Count > 0 ? dt.Rows[0]["FLDEMAIL"].ToString() : string.Empty;

				if (General.GetNullableInteger(ViewState["EMPID"].ToString()) == null)
				{
					ucError.ErrorMessage = "Select a Employee from Query Activity";
					ucError.Visible = true;
					return;
				}
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
				ViewState["CURRENTINDEX"] = -1;
				ViewState["CORRESPONDENCEID"] = string.Empty;
				ViewState["mailsessionid"] = string.Empty;
                //SetEmployeePrimaryDetails();
                SetPreSeaNewApplicantPrimaryDetails();

                trBCC.Visible = false;
				trCC.Visible = false;
				trAtt.Visible = false;
				trFrom.Visible = false;
				trTO.Visible = false;
				txtTO.CssClass = "input";
				txtDate.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);

				txtSubject.CssClass = "readonlytextbox";
				txtSubject.ReadOnly = true;
				ddlCorrespondenceType.Enabled = false;
				txtBodyDiv.Visible = true;
				txtBodyDiv.InnerText = "";
				txtBody.Visible = false;
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

	protected void PreSeaCorrespondence_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("FIND"))
			{
				BindData();
			}
			else if (dce.CommandName.ToUpper().Equals("EXCEL"))
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

	protected void Correspondence_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
				if (!IsValidCorrespondence(txtSubject.Text, ddlCorrespondenceType.SelectedQuick))
				{
					ucError.Visible = true;
					return;
				}
				if (ViewState["CORRESPONDENCEID"].ToString() == string.Empty)
				{
					PhoenixPreSeaTraineeCorrespondence.InsertCorrespondence(int.Parse(ViewState["EMPID"].ToString()), int.Parse(ddlCorrespondenceType.SelectedQuick)
																	, txtFrom.Text, txtTO.Text, txtSubject.Text, txtBody.Content, 1);
				}
				else
				{
					PhoenixPreSeaTraineeCorrespondence.UpdateCorrespondence(int.Parse(ViewState["CORRESPONDENCEID"].ToString()), int.Parse(ViewState["EMPID"].ToString()), int.Parse(ddlCorrespondenceType.SelectedQuick)
																		, txtFrom.Text, txtTO.Text, txtSubject.Text, txtBody.Content, 1);
					ucStatus.Text = "Correspondence Information Updated";
				}
				BindData();
				SetPageNavigator();
			}
			else if (dce.CommandName.ToUpper().Equals("SEND"))
			{
				if (!IsValidCorrespondence(txtSubject.Text, ddlCorrespondenceType.SelectedQuick))
				{
					ucError.Visible = true;
					return;
				}
				string dtkey = string.Empty;
				string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/EmailAttachments/" + ViewState["mailsessionid"].ToString();
				DataSet ds = PhoenixPreSeaTraineeCorrespondence.InsertCorrespondence(int.Parse(ViewState["EMPID"].ToString()), int.Parse(ddlCorrespondenceType.SelectedQuick)
																, txtFrom.Text, txtTO.Text, txtSubject.Text, txtBody.Content, null);
				if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
				{
					dtkey = ds.Tables[0].Rows[0][0].ToString();
					if (Directory.Exists(path))
					{
						DirectoryInfo di = new DirectoryInfo(path);
						FileInfo[] fi = di.GetFiles();
						foreach (FileInfo f in fi)
						{

							string tempkey = PhoenixCommonFileAttachment.GenerateDTKey();
							PhoenixCommonFileAttachment.InsertAttachment(new Guid(dtkey), f.Name, "Crew/" + tempkey + f.Extension, f.Length
								, PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, PhoenixCrewAttachmentType.CORRESPONDENCE.ToString(), new Guid(tempkey));
							string desgpath = HttpContext.Current.Request.MapPath("~/");
							desgpath = desgpath + "Attachments/Crew/" + tempkey + f.Extension;
							f.CopyTo(desgpath, true);

						}
					}
				}
				PhoenixMail.SendMail(txtTO.Text, txtCC.Text, txtCC.Text, txtSubject.Text, txtBody.Content
							   , true, System.Net.Mail.MailPriority.Normal, ViewState["mailsessionid"].ToString(), General.GetNullableInteger(ViewState["EMPID"].ToString()));
				gvCorrespondence.SelectedIndex = -1;
				BindData();
				SetPageNavigator();
			}

			if (!dce.CommandName.ToUpper().Equals("NEWMAIL"))
			{
				ViewState["CORRESPONDENCEID"] = string.Empty;
				ResetFields();
				txtDate.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
				gvCorrespondence.SelectedIndex = -1;
				trBCC.Visible = false;
				trCC.Visible = false;
				trAtt.Visible = false;
				trFrom.Visible = false;
				trTO.Visible = false;
				txtTO.CssClass = "input";

				if (dce.CommandName.ToUpper().Equals("DISCARD"))
				{
					if (lstAttachments.Items.Count > 0)
					{
						string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/EmailAttachments/" + ViewState["mailsessionid"].ToString();
						if (Directory.Exists(path))
							Directory.Delete(path, true);
						lstAttachments.Items.Clear();
					}
				}
				ViewState["mailsessionid"] = string.Empty;

				PhoenixToolbar toolbar = new PhoenixToolbar();
				toolbar.AddButton("New", "NEW");
				toolbar.AddButton("Save", "SAVE");
				MenuCorrespondence.MenuList = toolbar.Show();

				txtSubject.CssClass = "input_mandatory";
				txtSubject.ReadOnly = false;
				ddlCorrespondenceType.Enabled = true;
				txtBodyDiv.Visible = false;
				txtBody.Visible = true;
				txtBody.ActiveMode = AjaxControlToolkit.HTMLEditor.ActiveModeType.Design;
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private void EditCorrespondence(string correspondenceid, string employeeid)
	{
		DataTable dt = PhoenixPreSeaTraineeCorrespondence.EditCorrespondence(General.GetNullableInteger(correspondenceid).Value, General.GetNullableInteger(employeeid));

		if (dt.Rows.Count > 0)
		{
			ViewState["CORRESPONDENCEID"] = dt.Rows[0]["FLDCORRESPONDENCEID"].ToString();
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("New", "NEW");
			//toolbar.AddButton("Save", "SAVE");            
			toolbar.AddImageLink("javascript:parent.Openpopup('CI','','../Crew/CrewCorrespondenceLockAndUnlock.aspx?id=" + dt.Rows[0]["FLDCORRESPONDENCEID"].ToString() + "&empid=" + ViewState["EMPID"].ToString() + "','medium'); return false;", dt.Rows[0]["FLDLOCKEDYN"].ToString() == "0" ? "Lock" : "UnLock", "", dt.Rows[0]["FLDLOCKEDYN"].ToString() == "0" ? "LOCK" : "UNLOCK");
			MenuCorrespondence.MenuList = toolbar.Show();
			txtFrom.Text = dt.Rows[0]["FLDFROM"].ToString();
			txtTO.Text = dt.Rows[0]["FLDTO"].ToString();
			txtSubject.Text = dt.Rows[0]["FLDSUBJECT"].ToString();
			txtBody.Content = dt.Rows[0]["FLDBODY"].ToString();
			txtBodyDiv.InnerHtml = dt.Rows[0]["FLDBODY"].ToString();
			txtDate.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDCREATEDDATE"].ToString()));
			ddlCorrespondenceType.SelectedQuick = dt.Rows[0]["FLDCORRESPONDENCETYPE"].ToString();
			ViewState["DTKEY"] = dt.Rows[0]["FLDDTKEY"].ToString();
		}

		txtSubject.CssClass = "readonlytextbox";
		txtSubject.ReadOnly = true;
		ddlCorrespondenceType.Enabled = false;
		//ddlEmailTemplate.Enabled = false;
		txtBody.Visible = false;
		txtBodyDiv.Visible = true;
	}
	protected void ShowExcel()
	{
		int iRowCount = 0;
		int iTotalPageCount = 0;
		string[] alColumns = { "FLDCREATEDDATE", "FLDSUBJECT", "FLDCORRESPONDENCETYPENAME", "FLDCREATEDBYNAME", "FLDTYPEOF" };
		string[] alCaptions = { "Created Date", "Subject", "Correspondence Type", "Created By", "Type Of" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
		NameValueCollection nvc = Filter.CurrentCorrespondenceFilter;
		DataTable dt = new DataTable();
		if (Filter.CurrentCorrespondenceFilter != null)
		{
			nvc["filter"] = "0";
			dt = PhoenixPreSeaTraineeCorrespondence.SearchCorrespondence(Convert.ToInt32(ViewState["EMPID"].ToString()),
																						   sortexpression, sortdirection,
																					 (int)ViewState["PAGENUMBER"],
																					 General.ShowRecords(null),
																					 ref iRowCount,
																					 ref iTotalPageCount,
																					 General.GetNullableString(nvc.Get("txtSubject").ToString()),
																					 General.GetNullableInteger(nvc.Get("ddlCorrespondenceType").ToString()));


		}
		else
		{
			dt = PhoenixPreSeaTraineeCorrespondence.SearchCorrespondence(Convert.ToInt32(ViewState["EMPID"].ToString()),
																			   sortexpression, sortdirection,
																		 (int)ViewState["PAGENUMBER"],
																		 General.ShowRecords(null),
																		 ref iRowCount,
																		 ref iTotalPageCount,
																		 null, null);
		}

		General.ShowExcel("Crew Correspondence", dt, alColumns, alCaptions, sortdirection, sortexpression);
	}
	private void BindData()
	{
		try
		{
			int iRowCount = 0;
			int iTotalPageCount = 0;
			string[] alColumns = { "FLDCREATEDDATE", "FLDSUBJECT", "FLDCORRESPONDENCETYPENAME", "FLDCREATEDBYNAME", "FLDTYPEOF" };
			string[] alCaptions = { "Created Date", "Subject", "Correspondence Type", "Created By", "Type Of" };
			NameValueCollection nvc = Filter.CurrentCorrespondenceFilter;
			string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
			int? sortdirection = 1;
			if (ViewState["SORTDIRECTION"] != null)
				sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
			DataTable dt = new DataTable();
			if (Filter.CurrentCorrespondenceFilter != null)
			{
				nvc["filter"] = "0";
				dt = PhoenixPreSeaTraineeCorrespondence.SearchCorrespondence(Convert.ToInt32(ViewState["EMPID"].ToString()),
																							   sortexpression, sortdirection,
																						 (int)ViewState["PAGENUMBER"],
																						 General.ShowRecords(null),
																						 ref iRowCount,
																						 ref iTotalPageCount,
																						 General.GetNullableString(nvc.Get("txtSubject").ToString()),
																						 General.GetNullableInteger(nvc.Get("ddlCorrespondenceType").ToString()));


			}
			else
			{
				dt = PhoenixPreSeaTraineeCorrespondence.SearchCorrespondence(Convert.ToInt32(ViewState["EMPID"].ToString()),
																				   sortexpression, sortdirection,
																			 (int)ViewState["PAGENUMBER"],
																			 General.ShowRecords(null),
																			 ref iRowCount,
																			 ref iTotalPageCount,
																			 null, null);
			}
			DataSet ds = new DataSet();
			ds.Tables.Add(dt.Copy());
			ds.AcceptChanges();
			General.SetPrintOptions("gvCorrespondence", "Crew Correspondence", alCaptions, alColumns, ds);
			if (dt.Rows.Count > 0)
			{

				gvCorrespondence.DataSource = dt;
				gvCorrespondence.DataBind();
			}
			else
			{
				ShowNoRecordsFound(dt, gvCorrespondence);
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

	protected void gvCorrespondence_RowDataBound(object sender, GridViewRowEventArgs e)
	{
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
			   && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
			{
				DataRowView drv = (DataRowView)e.Row.DataItem;
				ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
				db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
				if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
				Label lblDTKey = (Label)e.Row.FindControl("lblDTKey");
				ImageButton att = (ImageButton)e.Row.FindControl("cmdAtt");
				if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;
				Label lblIsAtt = (Label)e.Row.FindControl("lblIsAtt");
				string strlock = ((Label)e.Row.FindControl("lblLockYN")).Text;
				if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
				att.Attributes.Add("onclick", "javascript:parent.Openpopup('CI','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
					+ PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.CORRESPONDENCE + (strlock == "1" ? "&U=N" : string.Empty) + "&cmdname=CORRESUPLOAD'); return false;");

				HtmlAnchor mail = (HtmlAnchor)e.Row.FindControl("cmdMail");
				mail.HRef = "CrewPersonalGeneral.aspx?email=true&cid=" + drv["FLDCORRESPONDENCEID"].ToString();

				LinkButton chkMail = (LinkButton)e.Row.FindControl("lnkCorrespondence");

				if (drv["FLDTYPE"].ToString() != "1")
				{
					chkMail.Attributes["target"] = "filterandsearch";
					chkMail.Attributes["href"] = "PreSeaTraineePersonalGeneral.aspx?email=true&cid=" + drv["FLDCORRESPONDENCEID"].ToString();
				}
			}
		}
		if (e.Row.RowType == DataControlRowType.DataRow)
		{
			// Get the LinkButton control in the first cell
			LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
			// Get the javascript which is assigned to this LinkButton
			string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
			// Add this javascript to the onclick Attribute of the row
			e.Row.Attributes["onclick"] = _jsDouble;
		}
	}
	protected void gvCorrespondence_RowEditing(object sender, GridViewEditEventArgs e)
	{
		try
		{

			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.NewEditIndex;
			string id = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCorrespondenceId")).Text;
			string empid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTraineeId")).Text;
			_gridView.SelectedIndex = nCurrentRow;

			EditCorrespondence(id, empid);
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void gvCorrespondence_RowDeleting(object sender, GridViewDeleteEventArgs e)
	{
		try
		{
			GridView _gridView = (GridView)sender;
			int nCurrentRow = e.RowIndex;
			string id = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblCorrespondenceId")).Text;

			PhoenixPreSeaTraineeCorrespondence.DeleteCorrespondence(Convert.ToInt32(id), int.Parse(ViewState["EMPID"].ToString()));
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

	private void ResetFields()
	{
		ViewState["CORRESPONDENCEID"] = string.Empty;
		txtFrom.Text = string.Empty;
		txtTO.Text = string.Empty;
		txtSubject.Text = string.Empty;
		txtBody.Content = string.Empty;
		ddlCorrespondenceType.SelectedQuick = string.Empty;
	}
	public bool IsValidCorrespondence(string subject, string type)
	{
		ucError.HeaderMessage = "Please provide the following required information";
		int resultInt;

		if (string.IsNullOrEmpty(subject))
			ucError.ErrorMessage = "Subject is required.";

		if (!int.TryParse(type, out resultInt))
			ucError.ErrorMessage = "Correspondence Type is required.";

		if (txtFrom.Text.Trim() != string.Empty && !General.IsvalidEmail(txtFrom.Text))
			ucError.ErrorMessage = "Please enter valid From E-Mail Address";

		if ((txtTO.Text.Trim() != string.Empty || txtTO.CssClass == "input_mandatory") && !General.IsvalidEmail(txtTO.Text))
			ucError.ErrorMessage = "Please enter valid To E-Mail Address";

		return (!ucError.IsError);

	}

	//private void SetEmployeePrimaryDetails()
	//{
	//	try
	//	{
	//		DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(ViewState["EMPID"].ToString()));

	//		if (dt.Rows.Count > 0)
	//		{
	//			txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
	//			txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
	//			txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
	//			txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
	//		}
	//	}
	//	catch (Exception ex)
	//	{
	//		ucError.ErrorMessage = ex.Message;
	//		ucError.Visible = true;
	//	}
	//}

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
			gvCorrespondence.SelectedIndex = -1;
			gvCorrespondence.EditIndex = -1;
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
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{
		if (Session["AttachFiles"] != null)
		{
			lstAttachments.Items.Clear();
			DataTable dt = (DataTable)Session["AttachFiles"];
			for (int i = 0; i < dt.Rows.Count; i++)
			{
				lstAttachments.Items.Add(new ListItem(dt.Rows[i]["Text"].ToString(), dt.Rows[i]["Value"].ToString()));
			}
			Session["AttachFiles"] = null;
		}
		else if (!trFrom.Visible && Session["corres"].ToString() == "1")
		{
			EditCorrespondence(ViewState["CORRESPONDENCEID"].ToString(), ViewState["EMPID"].ToString());
		}
		else
			ResetFields();
		BindData();
	}
}
