using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web;
using Telerik.Web.UI;
public partial class CrewPreSeaCourse : PhoenixBasePage
{
	string empid = string.Empty;
	protected void Page_Load(object sender, EventArgs e)
	{
        SessionUtil.PageAccessRights(this.ViewState);
		empid = Request.QueryString["empid"].ToString();
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuCrewPreSeaCourse.AccessRights = this.ViewState;
        MenuCrewPreSeaCourse.MenuList = toolbar.Show();

		if (!IsPostBack)
		{
			ViewState["PRESEARCOURSEID"] = Request.QueryString["PRESEARCOURSEID"];
			
			if (Request.QueryString["PRESEARCOURSEID"] != null)
			{
				EditCrewPreSeaCourses(Int16.Parse(Request.QueryString["PRESEARCOURSEID"].ToString()));

                ((RadComboBox)ucCourse.FindControl("ddlCourse")).Focus();
			}
            //if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";
            //att.Attributes.Add("onclick", "javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
            //    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.ACADEMICS + "&cmdname=COURSEUPLOAD'); return false;");

		}
	}

	protected void EditCrewPreSeaCourses(int preseacourseid)
	{
		DataTable dt = PhoenixCrewAcademicQualification.EditEmployeePreSeaCourse(Convert.ToInt32(empid), preseacourseid);

		if (dt.Rows.Count > 0)
		{

			ucInstitution.SelectedAddress = dt.Rows[0]["FLDINSTITUTIONID"].ToString();
			txtPlaceOfInstittution.Text = dt.Rows[0]["FLDPLACEOFINSTITUTION"].ToString();
			ucCourse.SelectedCourse = dt.Rows[0]["FLDQUALIFICATIONID"].ToString();
			txtFromDate.Text = dt.Rows[0]["FLDFROMDATE"].ToString();
			txtToDate.Text = dt.Rows[0]["FLDTODATE"].ToString();
			txtPassDate.Text = dt.Rows[0]["FLDPASSDATE"].ToString();
			txtPercentage.Text = dt.Rows[0]["FLDPERCENTAGE"].ToString();
			txtGrade.Text = dt.Rows[0]["FLDGRADE"].ToString();
            ucOallRank.Text = dt.Rows[0]["FLDOALLRANK"].ToString();
            txtRemarks.Text = dt.Rows[0]["FLDREMARKS"].ToString();
            //lblAttachment.Visible = false;
            lblFileSelect.Visible = false;
            txtFileUpload.Visible = false;            
		}
	}
	protected void DisablePreSea(object sender,EventArgs e)
	{
		if (ucCourse.SelectedCourse == "")
		{
			ViewState["ENABLE"] = "1";
			txtPlaceOfInstittution.CssClass = "input";
			txtFromDate.CssClass = "input";
			txtToDate.CssClass = "input";
			ucInstitution.CssClass = "input";
			ucCourse.CssClass = "input";
			txtPassDate.CssClass = "input";
            txtFileUpload.CssClass = "input";
		}
		else
		{
			ViewState["ENABLE"] = "";
			txtPlaceOfInstittution.CssClass = "input_mandatory";
			txtFromDate.CssClass = "input_mandatory";
			txtToDate.CssClass = "input_mandatory";
            ucInstitution.CssClass = "dropdown_mandatory";
			ucCourse.CssClass = "dropdown_mandatory";
			txtPassDate.CssClass = "input_mandatory";
            txtFileUpload.CssClass = "input_mandatory";
		}

	}
	protected void ResetCrewPreSeaCourse()
	{
		ucInstitution.SelectedAddress = "";
		txtPlaceOfInstittution.Text = "";
		ucCourse.SelectedCourse = "";
		txtFromDate.Text = "";
		txtToDate.Text = "";
		txtPassDate.Text = "";
		txtPercentage.Text = "";
		txtGrade.Text = "";
	}

	protected void CrewPreSeaCourse_TabStripCommand(object sender, EventArgs e)
	{
		String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
		String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
		{
			if (CommandName.ToUpper().Equals("SAVE"))
			{
				if (IsValidAcademic(txtPlaceOfInstittution.Text, txtFromDate.Text, txtToDate.Text, ucCourse.SelectedCourse,
					txtPassDate.Text, txtPercentage.Text, txtGrade.Text))
				{


					if (Request.QueryString["PRESEARCOURSEID"] != null)
					{
						PhoenixCrewAcademicQualification.UpdateEmployeePreSeaCourse(PhoenixSecurityContext.CurrentSecurityContext.UserCode						
														, Convert.ToInt32(ViewState["PRESEARCOURSEID"].ToString())
														, Convert.ToInt32(empid)
														, General.GetNullableInteger(ucInstitution.SelectedAddress) 
														, txtPlaceOfInstittution.Text
														, General.GetNullableInteger(ucCourse.SelectedCourse)
														, General.GetNullableDateTime(txtFromDate.Text)
														, General.GetNullableDateTime(txtToDate.Text)
														, General.GetNullableDateTime(txtPassDate.Text)
														, General.GetNullableDecimal(txtPercentage.Text)
														, txtGrade.Text
														, General.GetNullableString(txtRemarks.Text)
                                                        , General.GetNullableInteger(ucOallRank.Text)
														);
						ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
						ResetCrewPreSeaCourse();
					}
					else
					{
                        //if (!(txtFileUpload.HasFile && Request.Files["txtFileUpload"].ContentLength > 0 || ucCourse.SelectedCourse == ""))
                        if(Request.Files.Count==0)
                        {
                            ucError.ErrorMessage = "Attachment Required";
                            ucError.Visible = true;
                            return;
                        }
                        else
                        {
                            DataTable dt= PhoenixCrewAcademicQualification.InsertEmployeePreSeaCourse(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                          , Convert.ToInt32(empid)
                                                            , General.GetNullableInteger(ucInstitution.SelectedAddress)
                                                            , txtPlaceOfInstittution.Text
                                                            , General.GetNullableInteger(ucCourse.SelectedCourse)
                                                            , General.GetNullableDateTime(txtFromDate.Text)
                                                            , General.GetNullableDateTime(txtToDate.Text)
                                                            , General.GetNullableDateTime(txtPassDate.Text)
                                                            , General.GetNullableDecimal(txtPercentage.Text)
                                                            , General.GetNullableString(txtGrade.Text)
															, General.GetNullableString(txtRemarks.Text)
                                                            , General.GetNullableInteger(ucOallRank.Text)
                                                          );
                            ViewState["attachmentcode"] = dt.Rows[0][1].ToString();
                            if (Request.Files.Count > 0)
                            {
                                foreach (UploadedFile postedFile in txtFileUpload.UploadedFiles)
                                {
                                    if (!Object.Equals(postedFile, null))
                                    {
                                        if (postedFile.ContentLength > 0)
                                        {

                                            if (ViewState["attachmentcode"] != null || ViewState["attachmentcode"].ToString() != string.Empty)
                                            {
                                                PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["attachmentcode"].ToString()), PhoenixModule.CREW, null, ".jpg,.png,.gif,.doc,.txt,.xls,.pdf,.htm,.html,.gif,.jpeg,.bmp,.csv", string.Empty, "ACADEMICS");
                                            }

                                        }
                                    }
                                }
                            }

                            //if (txtFileUpload.UploadedFiles)
                            //{
                            //    if (Request.Files["txtFileUpload"].ContentLength > 0)
                            //    {

                            //        if (ViewState["attachmentcode"] != null || ViewState["attachmentcode"].ToString() != string.Empty)
                            //        {
                            //            PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["attachmentcode"].ToString()), PhoenixModule.CREW, null, ".jpg,.png,.gif,.doc,.txt,.xls,.pdf,.htm,.html,.gif,.jpeg,.bmp,.csv", string.Empty, "ACADEMICS");
                            //        }
                            //    }
                            //}
                        }
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);
                        ResetCrewPreSeaCourse();
					}
				}
				else
				{
					ucError.Visible = true;
					return;
				}
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private bool IsValidAcademic(string placeofinstitution, string fromdate, string todate, string qualification, string passingdate, string percentage, string grade)
	{
		ucError.HeaderMessage = "Please provide the following required information";
		DateTime resultdate;
		int resultInt;

		if ((string)ViewState["ENABLE"] != "1")
		{
			if (!int.TryParse(qualification, out resultInt))
				ucError.ErrorMessage = "Qualification is required";

			if (!int.TryParse(ucInstitution.SelectedAddress, out resultInt))
				ucError.ErrorMessage = "Institution is required";

			if (string.IsNullOrEmpty(placeofinstitution))
			{
				ucError.ErrorMessage = "Place of institution is required";
			}

			if (string.IsNullOrEmpty(fromdate))
			{
				ucError.ErrorMessage = "From Date is required";
			}
			else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
			{
				ucError.ErrorMessage = "From Date should be earlier than current date";
			}

			if (string.IsNullOrEmpty(todate))
			{
				ucError.ErrorMessage = "To Date is required";
			}
			else if (!string.IsNullOrEmpty(fromdate)
				&& DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
			{
				ucError.ErrorMessage = "To Date should be later than 'From Date'";
			}

			if (string.IsNullOrEmpty(passingdate))
			{
				ucError.ErrorMessage = "Pass Date is required";
			}
			else if (DateTime.TryParse(passingdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
			{
				ucError.ErrorMessage = "Pass Date should be earlier than current date";
			}
			else if (!string.IsNullOrEmpty(todate)
				&& DateTime.TryParse(passingdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(todate)) < 0)
			{
				ucError.ErrorMessage = "Pass Date should be later than 'To Date'";
			}

            //if (string.IsNullOrEmpty(percentage) && string.IsNullOrEmpty(grade))
            //{
            //    ucError.ErrorMessage = "Either Percentage or Grade  is required";
            //}
		}
		return (!ucError.IsError);

	}
}
