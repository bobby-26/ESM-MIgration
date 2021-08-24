using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Text;
using Telerik.Web.UI;
public partial class CrewRecommendedCourseAdd : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		SessionUtil.PageAccessRights(this.ViewState);
		
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuCrewRecommendedCourse.AccessRights = this.ViewState;
        MenuCrewRecommendedCourse.MenuList = toolbar.Show();

        if (!IsPostBack)
		{
            ViewState["empid"] = Request.QueryString["empid"].ToString();

            BindCheckBoxCourse();
			BindDropdownCourse();

			if (Request.QueryString["dtkey"] != null)
			{
				ddlCourse.Visible = true;
				lstCourse.Visible = false;
				EditCrewRecommendedCourse(new Guid(Request.QueryString["dtkey"]));
			}
			else
			{
				ddlCourse.Visible = false;
				lstCourse.Visible = true;
			}

		}
	}

	protected void BindCheckBoxCourse()
	{
		lstCourse.Items.Clear();
		ListItem items = new ListItem();
		lstCourse.DataSource = PhoenixRegistersDocumentCourse.ListPostSeaCourse(null);
		lstCourse.DataTextField = "FLDDOCUMENTNAME";
		lstCourse.DataValueField = "FLDDOCUMENTID";
		lstCourse.DataBind();
	}
	protected void BindDropdownCourse()
	{

		ddlCourse.DataSource = PhoenixRegistersDocumentCourse.ListPostSeaCourse(null);
		ddlCourse.DataTextField = "FLDDOCUMENTNAME";
		ddlCourse.DataValueField = "FLDDOCUMENTID";
		ddlCourse.DataBind();
	}

	protected void EditCrewRecommendedCourse(Guid dtkey)
	{
		DataSet ds = PhoenixCrewRecommendedCourse.EditCrewRecommendedCourse(dtkey);

		if (ds.Tables[0].Rows.Count > 0)
		{
			ddlCourse.SelectedValue = ds.Tables[0].Rows[0]["FLDCOURSEID"].ToString();
			txtRecommendedDate.Text = ds.Tables[0].Rows[0]["FLDRECOMMENDEDDATE"].ToString();
			ddlCreatedBy.SelectedUser = ds.Tables[0].Rows[0]["FLDRECOMMENDEDBY"].ToString();
			txtRemarks.Text = ds.Tables[0].Rows[0]["FLDREMARKS"].ToString();
			
		}
	}
	protected void ddlCourse_DataBound(object sender, EventArgs e)
	{
		ddlCourse.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
	}

	protected void CrewRecommendedCourse_TabStripCommand(object sender, EventArgs e)
	{
		String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo');");
		String scriptpopupopen = String.Format("javascript:fnReloadList('codehelp1', 'ifMoreInfo', 'keeppopupopen');");

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
		{
			if (CommandName.ToUpper().Equals("SAVE"))
			{
				StringBuilder strCourse = new StringBuilder();
				foreach (RadListBoxItem item in lstCourse.Items)
				{
					if (item.Selected == true)
					{

						strCourse.Append(item.Value.ToString());
						strCourse.Append(",");
					}
				}
				if (strCourse.Length > 1)
				{
					strCourse.Remove(strCourse.Length - 1, 1);
				}
				if (!IsValidRecommendedCourse(strCourse.ToString(), txtRecommendedDate.Text, ddlCreatedBy.SelectedUser))
				{
					ucError.Visible = true;
					return;
				}
				if (Request.QueryString["recommendedid"] != null)
				{

					PhoenixCrewRecommendedCourse.UpdateCrewRecommendedCourse(
						    PhoenixSecurityContext.CurrentSecurityContext.UserCode
						  ,	new Guid(Request.QueryString["dtkey"])
						  , Convert.ToInt32(ddlCourse.SelectedValue)
						  , Convert.ToInt32(ViewState["empid"].ToString())
						  , Convert.ToDateTime(txtRecommendedDate.Text)
						  , General.GetNullableInteger(ddlCreatedBy.SelectedUser)
						  , null
						  , General.GetNullableString(txtRemarks.Text)
						  , null
						  );

					ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);

				}
				else
				{
					PhoenixCrewRecommendedCourse.InsertCrewRecommendedCourse(
							PhoenixSecurityContext.CurrentSecurityContext.UserCode
						  , strCourse.ToString()
						  , Convert.ToInt32(ViewState["empid"].ToString())
						  , Convert.ToDateTime(txtRecommendedDate.Text)
						  , General.GetNullableInteger(ddlCreatedBy.SelectedUser)
						  , null 
						  , General.GetNullableString(txtRemarks.Text)
						  , null
						  );
					ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopupclose, true);

				}
			}
		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	private bool IsValidRecommendedCourse(string courseid, string recommendeddate, string recommendedby)
	{

		ucError.HeaderMessage = "Please provide the following required information";

		if (Request.QueryString["recommendedid"] != null)
		{
			if (General.GetNullableInteger(ddlCourse.SelectedValue) == null)
				ucError.ErrorMessage = "Course is required.";
		}
		else
		{
			if (General.GetNullableString(courseid) == null)
				ucError.ErrorMessage = "Select atleast 1 course to be recommeded.";
		}
		if (General.GetNullableDateTime(recommendeddate) == null)
			ucError.ErrorMessage = "Recommeded date is required.";

		if (General.GetNullableInteger(recommendedby) == null)
			ucError.ErrorMessage = "Recommeded by is required.";

		return (!ucError.IsError);
	}

}
