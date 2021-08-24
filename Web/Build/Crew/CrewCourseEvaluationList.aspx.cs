using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class CrewCourseEvaluationList : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{
		PhoenixToolbar toolbar = new PhoenixToolbar();
		toolbar.AddButton("Save", "SAVE");
		MenuCourseEvaluationList.MenuList = toolbar.Show();
		
		if (!IsPostBack)
		{
			
			if (Request.QueryString["evaluationid"] != "")
			{

				EditCourseEvaluationItem(Convert.ToInt32(Request.QueryString["evaluationid"]));
			}
		}
		
	}
	protected void EditCourseEvaluationItem(int evaluationid)
	{
		try
		{

			DataTable dt = PhoenixCrewCourseEvaluation.EditCourseEvaluation(evaluationid);
			if (dt.Rows.Count > 0)
			{
				ucCourse.SelectedCourse = dt.Rows[0]["FLDCOURSEID"].ToString();
				ucCourseType.SelectedHard = dt.Rows[0]["FLDDOCUMENTTYPE"].ToString();
			    txtItemCode.Text = dt.Rows[0]["FLDITEMCODE"].ToString();
			    txtItemDescription.Text = dt.Rows[0]["FLDITEMDESCRIPTION"].ToString();
			    ucItemName.SelectedHard = dt.Rows[0]["FLDITEMID"].ToString();
			    ucRatingScale.SelectedHard = dt.Rows[0]["FLDRATINGSCALE"].ToString();
				txtSortOrder.Text= dt.Rows[0]["FLDSORTORDER"].ToString();
			    txtDescriptor1.Text = dt.Rows[0]["FLDDEFAULTDESCRIPTOR1"].ToString();
			    txtDescriptor2.Text = dt.Rows[0]["FLDDEFAULTDESCRIPTOR2"].ToString();
			    txtDescriptor3.Text = dt.Rows[0]["FLDDEFAULTDESCRIPTOR3"].ToString();
			    txtDescriptor4.Text = dt.Rows[0]["FLDDEFAULTDESCRIPTOR4"].ToString();
			    txtDescriptor5.Text = dt.Rows[0]["FLDDEFAULTDESCRIPTOR5"].ToString();
			    txtDescriptor6.Text = dt.Rows[0]["FLDDEFAULTDESCRIPTOR6"].ToString();
			    txtDescriptor7.Text = dt.Rows[0]["FLDDEFAULTDESCRIPTOR7"].ToString();
				ViewState["ratingscale"]=dt.Rows[0]["FLDRATINGSCALE"].ToString();
				ViewState["evaluationitemid"] = dt.Rows[0]["FLDEVALUATIONITEMID"].ToString();
				ViewState["evaluationid"] = dt.Rows[0]["FLDEVALUATIONID"].ToString();
				if (dt.Rows[0]["FLDRATINGSCALE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 150, "PAC"))
				{
					EnableComment();
				}
				else
				{
					EnableDescriptor();
				}
			}
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}

	}

	protected void EnableDescriptor(object sender, EventArgs e)
	{
		if (chkEditDescriptors.Checked == true)
		{
			MenuCourseEvaluationList.Visible = true;
			txtDescriptor1Update.Enabled = true;
			txtDescriptor2Update.Enabled = true;
			txtDescriptor3Update.Enabled = true;
			txtDescriptor4Update.Enabled = true;
			txtDescriptor5Update.Enabled = true;
			txtDescriptor6Update.Enabled = true;
			txtDescriptor7Update.Enabled = true;
			txtDescriptor1Update.CssClass = "input";
			txtDescriptor2Update.CssClass = "input";
			txtDescriptor3Update.CssClass = "input";
			txtDescriptor4Update.CssClass = "input";
			txtDescriptor5Update.CssClass = "input";
			txtDescriptor6Update.CssClass = "input";
			txtDescriptor7Update.CssClass = "input";
		}
		else
		{
			MenuCourseEvaluationList.Visible = false;
			txtDescriptor1Update.Enabled = false;
			txtDescriptor2Update.Enabled = false;
			txtDescriptor3Update.Enabled = false;
			txtDescriptor4Update.Enabled = false;
			txtDescriptor5Update.Enabled = false;
			txtDescriptor6Update.Enabled = false;
			txtDescriptor7Update.Enabled = false;
			txtDescriptor1Update.CssClass = "readonlytextbox";
			txtDescriptor2Update.CssClass = "readonlytextbox";
			txtDescriptor3Update.CssClass = "readonlytextbox";
			txtDescriptor4Update.CssClass = "readonlytextbox";
			txtDescriptor5Update.CssClass = "readonlytextbox";
			txtDescriptor6Update.CssClass = "readonlytextbox";
			txtDescriptor7Update.CssClass = "readonlytextbox";
		}
	}
	protected void EnableDescriptor()
	{
		txtDescriptor1.Visible = true;
		txtDescriptor2.Visible = true;
		txtDescriptor3.Visible = true;
		txtDescriptor4.Visible = true;
		txtDescriptor5.Visible = true;
		txtDescriptor6.Visible = true;
		txtDescriptor7.Visible = false;
		txtDescriptor1Update.Visible = true;
		txtDescriptor2Update.Visible = true;
		txtDescriptor3Update.Visible = true;
		txtDescriptor4Update.Visible = true;
		txtDescriptor5Update.Visible = true;
		txtDescriptor6Update.Visible = true;
		txtDescriptor7Update.Visible = false;
		lblDescriptor7.Visible = false;

	}
	protected void EnableComment()
	{
		txtDescriptor1.Visible = false;
		txtDescriptor2.Visible = false;
		txtDescriptor3.Visible = false;
		txtDescriptor4.Visible = false;
		txtDescriptor5.Visible = false;
		txtDescriptor6.Visible = false;
		txtDescriptor7.Visible = true;
		txtDescriptor7.CssClass = "readonlytextbox";
		txtDescriptor1Update.Visible = false;
		txtDescriptor2Update.Visible = false;
		txtDescriptor3Update.Visible = false;
		txtDescriptor4Update.Visible = false;
		txtDescriptor5Update.Visible = false;
		txtDescriptor6Update.Visible = false;
		txtDescriptor7Update.Visible = true;
		lblDescriptor1.Visible = false;
		lblDescriptor2.Visible = false;
		lblDescriptor3.Visible = false;
		lblDescriptor4.Visible = false;
		lblDescriptor5.Visible = false;
		lblDescriptor6.Visible = false;

	}

	
	protected void CourseEvaluationList_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
			DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
			if (dce.CommandName.ToUpper().Equals("SAVE"))
			{
				SaveCourseEvaluation();
			}
			
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}


	protected void SaveCourseEvaluation()
	{
		try
		{

			if (Request.QueryString["evaluationid"] != "")
			{
				if (!IsValidEvaluation())
				{
					ucError.Visible = true;
					return;
				}
				PhoenixCrewCourseEvaluation.UpdateCourseEvaluation(
					PhoenixSecurityContext.CurrentSecurityContext.UserCode,
					Convert.ToInt32(ViewState["evaluationitemid"]),
					Convert.ToInt32(ViewState["ratingscale"].ToString()),
					General.GetNullableString(txtDescriptor1Update.Text),
					General.GetNullableString(txtDescriptor2Update.Text),
					General.GetNullableString(txtDescriptor3Update.Text),
					General.GetNullableString(txtDescriptor4Update.Text),
					General.GetNullableString(txtDescriptor5Update.Text),
					General.GetNullableString(txtDescriptor6Update.Text),
					General.GetNullableString(txtDescriptor7Update.Text),
					Convert.ToInt32(txtSortOrder.Text),
					Convert.ToInt32(ViewState["evaluationid"].ToString()));

				ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('codehelp1','ifMoreInfo'," + (("'keepopen'")) + ");", true);

				EditCourseEvaluationItem(Convert.ToInt32(Request.QueryString["evaluationid"]));
				txtDescriptor1Update.CssClass = "readonlytextbox";
				txtDescriptor2Update.CssClass = "readonlytextbox";
				txtDescriptor3Update.CssClass = "readonlytextbox";
				txtDescriptor4Update.CssClass = "readonlytextbox";
				txtDescriptor5Update.CssClass = "readonlytextbox";
				txtDescriptor6Update.CssClass = "readonlytextbox";
				txtDescriptor7Update.CssClass = "readonlytextbox";
				txtDescriptor1Update.Enabled = false;
				txtDescriptor2Update.Enabled = false;
				txtDescriptor3Update.Enabled = false;
				txtDescriptor4Update.Enabled = false;
				txtDescriptor5Update.Enabled = false;
				txtDescriptor6Update.Enabled = false;
				txtDescriptor7Update.Enabled = false;
				chkEditDescriptors.Checked = false;
				ucStatus.Text = "Course Evaluation Information Updated";

			}
			
		}

		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}
	private bool IsValidEvaluation()
	{
		ucError.HeaderMessage = "Please provide the following required information";

		if (string.IsNullOrEmpty(txtSortOrder.Text))
			ucError.ErrorMessage = "General Remarks  is required.";
		
		
		


		return (!ucError.IsError);
	}
	public StateBag ReturnViewState()
	{
		return ViewState;
	}

	



}
