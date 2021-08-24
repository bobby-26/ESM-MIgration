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
public partial class CrewCourseReviewHistory : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			

			if (!IsPostBack)
			{
				if (Request.QueryString["reviewid"] != null)
				{
					EditCourseContent();
				}
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}


	public override void VerifyRenderingInServerForm(Control control)
	{
		return;
	}
	protected void EditCourseContent()
	{
		try
		{

			int reviewid = Convert.ToInt32(Request.QueryString["reviewid"]);
			DataSet ds = PhoenixRegistersDocumentCourse.EditCourseReview(reviewid);
			if (ds.Tables[0].Rows.Count > 0)
			{

				ucCourse.SelectedCourse = ds.Tables[0].Rows[0]["FLDCOURSEID"].ToString();
				ucCourseType.SelectedHard = ds.Tables[0].Rows[0]["FLDDOCUMENTTYPE"].ToString();
				txtDescription.Text = ds.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString();
				txtSubjectMatter.Text = ds.Tables[0].Rows[0]["FLDSUBJECTMATTER"].ToString();
				txtLearningTarget.Text = ds.Tables[0].Rows[0]["FLDLEARNINGTARGET"].ToString();
				txtMethodology.Text = ds.Tables[0].Rows[0]["FLDMETHODOLOGY"].ToString();
				txtRequirement.Text = ds.Tables[0].Rows[0]["FLDREQUIREMENT"].ToString();
				txtNotes.Text = ds.Tables[0].Rows[0]["FLDNOTE"].ToString();
				txtCourseOutline.Content = ds.Tables[0].Rows[0]["FLDCOURSEOUTLINE"].ToString();
				txtLastEditedBy.Text = ds.Tables[0].Rows[0]["FLDCREATEDBY"].ToString();
				ucRevisionDate.Text = ds.Tables[0].Rows[0]["FLDREVIEWDATE"].ToString();

			}


		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

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
                {
                    txtCourseOutline.Content = txtCourseOutline.Content + "<img src=\"" + PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + dr[0]["FLDFILEPATH"].ToString() + "\" />";
                    //txtCourseOutline.Content = txtCourseOutline.Content + "<img src=\"" + HttpContext.Current.Session["sitepath"] + "/attachments/" + dr[0]["FLDFILEPATH"].ToString() + "\" />";
                }
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



	public StateBag ReturnViewState()
	{
		return ViewState;
	}


}
