using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using System.Web.UI;
using Telerik.Web.UI;

public partial class DryDockStandardUnitAdd : PhoenixBasePage
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
				ViewState["StandardUnitID"] = null;
				if (Request.QueryString["StandardUnitID"] != null)
				{
					ViewState["StandardUnitID"] = Request.QueryString["StandardUnitID"];
				}
                toolbar.AddButton("Details", "DETAIL", ToolBarDirection.Right);
                toolbar.AddButton("List", "LIST",ToolBarDirection.Right);
				
				MenuHeader.AccessRights = this.ViewState;
				MenuHeader.MenuList = toolbar.Show();

				toolbar = new PhoenixToolbar();
				toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
				MenuStandardUnitsSpecification.AccessRights = this.ViewState;
				MenuStandardUnitsSpecification.MenuList = toolbar.Show();
				MenuHeader.SelectedMenuIndex = 0;

			}


		}
		catch (Exception ex)
		{
			ucError.ErrorMessage = ex.Message;
			ucError.Visible = true;
		}
	}

	protected void StandardUnitsSpecification_TabStripCommand(object sender, EventArgs e)
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

                Guid? jobid = null;
                jobid = PhoenixDryDockStandardCost.InsertDryDockStandardUnit(
                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                    4,//StandardUnitid
                                                    General.GetNullableString(txtNumber.Text).Trim(),
                                                    txtTitle.Text.Trim(),
                                                    General.GetNullableString(txtJobDescription.Text).Trim(),
                                                    ref jobid
                                                    );
                ViewState["StandardUnitID"] = jobid;

                ucStatus.Text = "Details Updated.";
                String script = "javascript:fnJobEdit('" + ViewState["StandardUnitID"].ToString() + "'); javascript:fnReloadList('code1');";
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

		if (txtNumber.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Number cannot be blank.";

		if (txtTitle.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Title cannot be blank.";

		if (txtJobDescription.Text.Trim().Equals(""))
			ucError.ErrorMessage = "Job Description cannot be blank.";

		return (!ucError.IsError);
	}
	protected void MenuHeader_TabStripCommand(object sender, EventArgs e)
	{

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("DETAIL"))
		{
            Response.Redirect("../DryDock/DryDockStandardUnitAdd.aspx", false);
		}
		else if (CommandName.ToUpper().Equals("LIST"))
		{
            Response.Redirect("../DryDock/DryDockStandardUnitList.aspx", false);
		}

	}
	protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
	{

	}
}
