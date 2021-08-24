using System;
using System.Data;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;

public partial class RegistersFacultyList : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
            SessionUtil.PageAccessRights(this.ViewState);
			if (!IsPostBack)
			{
				ViewState["FACULTYID"] = Request.QueryString["facultyid"];
				PhoenixToolbar toolbar = new PhoenixToolbar();
				toolbar.AddButton("New", "NEW");
				toolbar.AddButton("Save", "SAVE");
                MenuFacultyList.AccessRights = this.ViewState;
				MenuFacultyList.MenuList = toolbar.Show();
				ViewState["FACULTYID"] = "";
				BindCourseList(null,null);
				if (Request.QueryString["facultyid"] != null)
				{
					FacultyEdit();
				}
				
			}
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	protected void BindCourseList(object sender,EventArgs e)
	{
		cblCourse.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(null);
		cblCourse.DataTextField = "FLDCOURSE";
		cblCourse.DataValueField = "FLDDOCUMENTID";
		cblCourse.DataBind();

	}
	protected void FacultyList_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

			String scriptClosePopup = String.Format("javascript:fnReloadList('codehelp1');");

			String scriptKeepPopupOpen = String.Format("javascript:fnReloadList('codehelp1', null, 'yes');");


			if (dce.CommandName.ToUpper().Equals("NEW"))
			{
				ViewState["FACULTYID"] = "";
				Reset();
			}
			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
				if (!IsValidFaculty())
				{
					ucError.Visible = true;
					return;
				}
				StringBuilder strcourseid = new StringBuilder();
				foreach (ListItem item in cblCourse.Items)
				{
					if (item.Selected == true)
					{
						strcourseid.Append(item.Value.ToString());
						strcourseid.Append(",");
					}
				}

				if (strcourseid.Length > 1)
				{
					strcourseid.Remove(strcourseid.Length - 1, 1);
				}
				if (strcourseid.ToString() == "")
				{
					ucError.ErrorMessage = "Please Select Atleast One Course";
					ucError.Visible = true;
					return;
				}
				if (ViewState["FACULTYID"].ToString() == "")
				{

					PhoenixRegistersFaculty.InsertFaculty(
					PhoenixSecurityContext.CurrentSecurityContext.UserCode
					, txtCode.Text
					, txtFacultyName.Text
					, General.GetNullableString(strcourseid.ToString())
					, General.GetNullableInteger(chkActiveYesOrNo.Checked == true ? "1" : "0")
					);
					ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptClosePopup, true);
					Reset();

				}
				else
				{

					PhoenixRegistersFaculty.UpdateFaculty(
					PhoenixSecurityContext.CurrentSecurityContext.UserCode
					, Convert.ToInt32(ViewState["FACULTYID"].ToString())
					, txtCode.Text
					, txtFacultyName.Text
					, General.GetNullableString(strcourseid.ToString())
					, General.GetNullableInteger(chkActiveYesOrNo.Checked == true ? "1" : "0")
					);
					ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptClosePopup, true);

				}
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private void Reset()
	{
		ViewState["FACULTYID"] = "";
		txtCode.Text= "";
		txtFacultyName.Text = "";
		foreach (ListItem item in cblCourse.Items)
		{
			item.Selected = false;
		}
		chkActiveYesOrNo.Checked = false;

	}

	protected void FacultyEdit()
	{
		try
		{

			int facultyid = Convert.ToInt32(Request.QueryString["facultyid"]);
			DataTable dt = PhoenixRegistersFaculty.EditFaculty(facultyid);

			if (dt.Rows.Count > 0)
			{

				txtCode.Text = dt.Rows[0]["FLDFACULTYCODE"].ToString();
				txtFacultyName.Text = dt.Rows[0]["FLDFACULTYNAME"].ToString();
				chkActiveYesOrNo.Checked =  dt.Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
				ViewState["FACULTYID"] = dt.Rows[0]["FLDFACULTYID"].ToString();
				string[] courseid = dt.Rows[0]["FLDCOURSEID"].ToString().Split(',');
				foreach (string item in courseid)
				{
					if (item.Trim() != "")
					{
						cblCourse.Items.FindByValue(item).Selected = true;
					}
				}
			}
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private bool IsValidFaculty()
	{

		ucError.HeaderMessage = "Please provide the following required information";


		if (string.IsNullOrEmpty(txtCode.Text))
			ucError.ErrorMessage = "Faculty Code  is required.";

		if (string.IsNullOrEmpty(txtFacultyName.Text))
			ucError.ErrorMessage = "Faculty Name  is required.";


		return (!ucError.IsError);
	}

}
