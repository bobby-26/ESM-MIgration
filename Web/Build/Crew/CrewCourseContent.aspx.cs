using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using System.Web;
using System.Text;
using System.IO;
using Telerik.Web.UI;
public partial class CrewCourseContent : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);      
			PhoenixToolbar toolbar = new PhoenixToolbar();
			toolbar.AddButton("New", "NEW");
			toolbar.AddButton("Save", "SAVE");
			toolbar.AddButton("Export To Excel", "EXPORTTOEXCEL");
			MenuCourseContent.AccessRights = this.ViewState;
			MenuCourseContent.MenuList = toolbar.Show();

			if (!IsPostBack)
			{
				if (Session["COURSEID"] != null)
				{
					EditCourseContent();
				}
				imgRevision.Attributes.Add("onclick", "parent.Openpopup('codehelp1', '', '../Crew/CrewCourseReview.aspx?courseid=" + Session["COURSEID"] + "');return false;");
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
		Response.ClearContent();
		Response.AddHeader("Content-Disposition", "attachment; filename=CourseContent.xls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3>Course Content</h3></td>");
		Response.Write("</tr>");
		Response.Write("</TABLE>");
		Response.Write("<br />");
		Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><b>Course</b></td>");
		Response.Write("<td width='220px'>" + ucCourse.SelectedName + "</td>");
		Response.Write("<td><b>Course Type</b></td>");
		Response.Write("<td width='220px'>" + ucCourseType.SelectedName + "</td>");
		Response.Write("</tr>");
		Response.Write("<tr>");
		Response.Write("<td><b>Description</b></td>");
		Response.Write("<td>" + txtDescription.Text + "</td>");
		Response.Write("<td><b>Subject Matter</b></td>");
		Response.Write("<td>" + txtSubjectMatter.Text + "</td>");
		Response.Write("</tr>");
		Response.Write("<tr>");
		Response.Write("<td><b>Learning Objective</b></td>");
		Response.Write("<td >" + txtLearningTarget.Text + "</td>");
		Response.Write("<td><b>Methodology</b></td>");
		Response.Write("<td>" + txtMethodology.Text + "</td>");
		Response.Write("</tr>");
		Response.Write("<tr>");
		Response.Write("<td><b>Minimum Requirements for candidates</b></td>");
		Response.Write("<td >" + txtRequirement.Text + "</td>");
		Response.Write("<td><b>Notes</b></td>");
		Response.Write("<td>" + txtNotes.Text + "</td>");
		Response.Write("</tr>");
		Response.Write("<tr>");
		Response.Write("</TABLE>");
		Response.End();
	}

	public override void VerifyRenderingInServerForm(Control control)
	{
		return;
	}
	protected void EditCourseContent()
	{
		try
		{

		    int courseid = Convert.ToInt32(Session["COURSEID"].ToString());
			DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(courseid);
			if (ds.Tables[0].Rows.Count > 0)
			{

				ucCourse.SelectedCourse = ds.Tables[0].Rows[0]["FLDDOCUMENTID"].ToString();
				ucCourseType.SelectedHard = ds.Tables[0].Rows[0]["FLDDOCUMENTTYPE"].ToString();
				txtDescription.Text = ds.Tables[0].Rows[0]["FLDDESCRITPTION"].ToString();
				txtSubjectMatter.Text = ds.Tables[0].Rows[0]["FLDSUBJECTMATTER"].ToString();
				txtLearningTarget.Text = ds.Tables[0].Rows[0]["FLDLEARNINGTARGET"].ToString();
				txtMethodology.Text = ds.Tables[0].Rows[0]["FLDMETHODOLOGY"].ToString();
				txtRequirement.Text = ds.Tables[0].Rows[0]["FLDREQUIREMENT"].ToString();
				txtNotes.Text = ds.Tables[0].Rows[0]["FLDNOTE"].ToString();
				txtCourseOutline.Content = ds.Tables[0].Rows[0]["FLDCOURSEOUTLINE"].ToString();
				txtReviewedBy.Text = ds.Tables[0].Rows[0]["FLDLASTREVIEWEDBY"].ToString();
                txtAdditionaltext.Text = ds.Tables[0].Rows[0]["FLDADDITIONALTEXT"].ToString();
			}
			
		
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}

	protected void Reset()
	{
	
		txtDescription.Text = "";
		txtDescription.Text = "";
		txtSubjectMatter.Text = "";
		txtLearningTarget.Text = "";
		txtMethodology.Text = "";
		txtRequirement.Text = "";
		txtNotes.Text = "";
		txtCourseOutline.Content = "";
		ucRevisionDate.Text = "";

	}
	protected void btnInsertPic_Click(object sender, EventArgs e)
	{
		try
		{
			if (Request.Files.Count > 0 && txtCourseOutline.ActiveMode == AjaxControlToolkit.HTMLEditor.ActiveModeType.Design)
			{
				Guid gFileName = PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.INVENTORY, null, ".jpg,.png,.gif");
				DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["dtkey"].ToString()));
				DataRow[] dr = dt.Select("FLDDTKEY = '" + gFileName.ToString() + "'");
				if (dr.Length > 0)
					txtCourseOutline.Content = txtCourseOutline.Content + "<img src=\"" + HttpContext.Current.Session["sitepath"] + "/attachments/" + dr[0]["FLDFILEPATH"].ToString() + "\" />";
			}
			else
			{
				ucError.Text = Request.Files.Count > 0 ? "You are not in design mode" : "No Picture selected.";
				ucError.Visible = true;
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void CourseContent_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
				SaveCourseContent();
				EditCourseContent();
			}
			if (dce.CommandName.ToUpper().Equals("NEW"))
			{
				Reset();

			}
			if (dce.CommandName.ToUpper().Equals("EXPORTTOEXCEL"))
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


	protected void SaveCourseContent()
	{
		try
		{
			if (!IsValidCourseEvaluationItem())
			{
				ucError.Visible = true;
				return;
			}
			if (General.GetNullableDateTime(ucRevisionDate.Text) == null)
			{
				PhoenixRegistersDocumentCourse.UpdateCourseContent(
				PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt32(Session["COURSEID"].ToString()),
				General.GetNullableString(txtDescription.Text),
				General.GetNullableString(txtSubjectMatter.Text),
				General.GetNullableString(txtLearningTarget.Text),
				General.GetNullableString(txtMethodology.Text),
				General.GetNullableString(txtRequirement.Text),
				General.GetNullableString(txtNotes.Text),
				General.GetNullableString(txtCourseOutline.Content),
                General.GetNullableString(txtAdditionaltext.Text));

				ucStatus.Text = "Course Information updated.";
				txtReviewedBy.Text = "";
				ucRevisionDate.Text = "";
			}
			else
			{
				PhoenixRegistersDocumentCourse.InsertCourseReview(
				PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				Convert.ToInt32(Session["COURSEID"].ToString()),
				Convert.ToDateTime(ucRevisionDate.Text),
				General.GetNullableString(txtDescription.Text),
				General.GetNullableString(txtSubjectMatter.Text),
				General.GetNullableString(txtLearningTarget.Text),
				General.GetNullableString(txtMethodology.Text),
				General.GetNullableString(txtRequirement.Text),
				General.GetNullableString(txtNotes.Text),
				General.GetNullableString(txtCourseOutline.Content),
				General.GetNullableString(txtReviewedBy.Text),
                General.GetNullableString(txtAdditionaltext.Text));

				ucStatus.Text = "Course details Revised.";
				txtReviewedBy.Text = "";
				ucRevisionDate.Text = "";
			}
			
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private bool IsValidCourseEvaluationItem()
	{
	
		ucError.HeaderMessage = "Please provide the following required information";

		if (string.IsNullOrEmpty(txtDescription.Text))
			ucError.ErrorMessage = "Description  is required.";

		if (string.IsNullOrEmpty(txtRequirement.Text))
			ucError.ErrorMessage = "Minimum requirements for candidates  is required.";

		if (General.GetNullableDateTime(ucRevisionDate.Text) != null)
		{
			if (string.IsNullOrEmpty(txtReviewedBy.Text))
				ucError.ErrorMessage = "Last Reviewed by is required when revision is done.";

		}

		return (!ucError.IsError);
	}

	public StateBag ReturnViewState()
	{
		return ViewState;
	}


}
