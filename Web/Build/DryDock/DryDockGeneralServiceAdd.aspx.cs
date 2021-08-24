using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Telerik.Web.UI;


public partial class DryDockGeneralServiceAdd : PhoenixBasePage
{

	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
			PhoenixToolbar toolbar = new PhoenixToolbar();

			if (!IsPostBack)
			{
				ViewState["GENERALSERVICEID"] = null;
				if (Request.QueryString["GENERALSERVICEID"] != null)
				{
					ViewState["GENERALSERVICEID"] = Request.QueryString["GENERALSERVICEID"];
				}
                toolbar.AddButton("Details", "DETAIL", ToolBarDirection.Right);
                toolbar.AddButton("List", "LIST",ToolBarDirection.Right);
				
				MenuHeader.AccessRights = this.ViewState;
				MenuHeader.MenuList = toolbar.Show();

				toolbar = new PhoenixToolbar();
				toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
				MenuGeneralServicesSpecification.AccessRights = this.ViewState;
				MenuGeneralServicesSpecification.MenuList = toolbar.Show();
				MenuHeader.SelectedMenuIndex = 0;
                ucJobType.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(7);
                ucJobType.DataTextField = "FLDNAME";
                ucJobType.DataValueField = "FLDMULTISPECID";
                ucJobType.DataBind();
				BindCheckBox();

			}


		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void BindCheckBox()
	{
		string type = string.Empty;
     
        cblEnclosed.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(4);
		cblEnclosed.DataBindings.DataTextField = "FLDNAME";
		cblEnclosed.DataBindings.DataTextField = "FLDMULTISPECID";
		cblEnclosed.DataBind();

		cblMaterial.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(1);
		cblMaterial.DataBindings.DataTextField = "FLDNAME";
		cblMaterial.DataBindings.DataValueField = "FLDMULTISPECID";
		cblMaterial.DataBind();


        cblInclude.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(3);
        cblInclude.DataBindings.DataTextField = "FLDNAME";
        cblInclude.DataBindings.DataValueField = "FLDMULTISPECID";
        cblInclude.DataBind();


        cblWorkSurvey.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(2);
        cblWorkSurvey.DataBindings.DataTextField = "FLDNAME";
        cblWorkSurvey.DataBindings.DataValueField = "FLDMULTISPECID";
        cblWorkSurvey.DataBind();

        ddlResponsibilty.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(5);
        ddlResponsibilty.DataTextField = "FLDNAME";
        ddlResponsibilty.DataValueField = "FLDMULTISPECID";
        ddlResponsibilty.DataBind();

        ddlCostClassification.DataSource = PhoenixDryDockMultiSpec.ListDryDockMultiSpec(8);
        ddlCostClassification.DataTextField = "FLDNAME";
        ddlCostClassification.DataValueField = "FLDMULTISPECID";
        ddlCostClassification.DataBind();

        radddlbudgetcode.DataSource = PhoenixDryDockOrder.DrydockBudgetcodeList(null);
        radddlbudgetcode.DataTextField = "FLDBUDGETCODE";
        radddlbudgetcode.DataValueField = "FLDBUDGETID";
        radddlbudgetcode.DataBind();
    }
	protected void GeneralServicesSpecification_TabStripCommand(object sender, EventArgs e)
	{
        try
        {
            
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidRepairSpec())
                {
                    ucError.Visible = true;
                    return;
                }
                string strEnc = string.Empty;
                string strMat = string.Empty;
                string strWrk = string.Empty;
                string strInc = string.Empty;

                foreach (ButtonListItem item in cblMaterial.Items)
                {
                    if (item.Selected == true)
                    {
                        strMat = strMat + item.Value.ToString() + ",";
                    }
                }
                strMat = strMat.TrimEnd(',');

                foreach (ButtonListItem item in cblEnclosed.Items)
                {
                    if (item.Selected == true)
                    {
                        strEnc = strEnc + item.Value.ToString() + ",";
                    }
                }
                strEnc = strEnc.TrimEnd(',');

                foreach (ButtonListItem item in cblWorkSurvey.Items)
                {
                    if (item.Selected == true)
                    {
                        strWrk = strWrk + item.Value.ToString() + ",";
                    }
                }
                strWrk = strWrk.TrimEnd(',');

                foreach (ButtonListItem item in cblWorkSurvey.Items)
                {
                    if (item.Selected == true)
                    {
                        strWrk = strWrk + item.Value.ToString() + ",";
                    }
                }
                strWrk = strWrk.TrimEnd(',');


                foreach (ButtonListItem item in cblInclude.Items)
                {
                    if (item.Selected == true)
                    {
                        strInc = strInc + item.Value.ToString() + ",";
                    }
                }
                strInc = strInc.TrimEnd(',');


                Guid? jobid = null;
                jobid = PhoenixDryDockJobGeneral.InsertDryDockJobGeneral(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                    1,//generalserviceid
                                                    General.GetNullableString(txtNumber.Text).Trim(),
                                                    txtTitle.Text.Trim(),
                                                    ddlResponsibilty.SelectedValue,
                                                    General.GetNullableDecimal(txtDuration.Text),
                                                    null,
                                                    General.GetNullableInteger(ucJobType.SelectedValue),
                                                    General.GetNullableString(txtJobDescription.Text).Trim(),
                                                    null, strWrk,
                                                    strInc, strMat, strEnc, General.GetNullableInteger(ddlCostClassification.SelectedValue),
                                                    ref jobid
                                                    );
                ViewState["GENERALSERVICEID"] = jobid;
                PhoenixDryDockJobGeneral.DrydockJobGenralBudgetcodeInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                  , jobid
                  , General.GetNullableInteger(radddlbudgetcode.SelectedValue));

                ucStatus.Text = "Details Updated.";
                String script = "javascript:fnJobEdit('" + ViewState["GENERALSERVICEID"].ToString() + "'); javascript:fnReloadList('code1');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

	}
	private bool IsValidRepairSpec()
	{

		ucError.HeaderMessage = "Provide the following required information.";

        if (General.GetNullableInteger(ucJobType.SelectedValue) == null)
            ucError.ErrorMessage = "Job Type cannot be blank.";

		if (txtNumber.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Job Number cannot be blank.";

		if (txtTitle.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Title cannot be blank.";

		if (txtJobDescription.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Job Description cannot be blank.";

        if (General.GetNullableInteger(ddlCostClassification.SelectedValue) == null)
            ucError.ErrorMessage = "Cost Classification cannot be blank.";

		return (!ucError.IsError);
	}
	protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
	{


        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("DETAIL"))
		{
			//Response.Redirect("../DryDock/DryDockGeneralService.aspx?GENERALSERVICEID=" + ViewState["GENERALSERVICEID"].ToString(), false);
		}
		else if (CommandName.ToUpper().Equals("LIST"))
		{
            Response.Redirect("../DryDock/DryDockGeneralServiceList.aspx?GENERALSERVICEID=" + ViewState["GENERALSERVICEID"].ToString(), false);
		}

	}
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{

	}
	private bool IsValidRepairJobDetail(string jobdetail, string GENERALSERVICEID)
	{

		ucError.HeaderMessage = "Provide the following required information";

		if (General.GetNullableGuid(GENERALSERVICEID) == null)
			ucError.ErrorMessage = "Create Standard job to amend the details";

		if (jobdetail.Trim().Equals(""))
			ucError.ErrorMessage = "Job Detail is required.";

		return (!ucError.IsError);
	}
}
