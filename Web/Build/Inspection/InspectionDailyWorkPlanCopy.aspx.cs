using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI;
using Telerik.Web.UI;

public partial class Inspection_InspectionDailyWorkPlanCopy : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddButton("Copy", "COPY",ToolBarDirection.Right);
            MenuCopyGeneral.AccessRights = this.ViewState;
            MenuCopyGeneral.MenuList = toolbargrid.Show();

            if (Request.QueryString["vesselid"] != null)
                ucVessel.SelectedVessel = Request.QueryString["vesselid"].ToString();

            if (Request.QueryString["lblWorkPlanId"] != null)
                ViewState["lblWorkPlanId"] = Request.QueryString["lblWorkPlanId"].ToString();

            //VesselConfiguration();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void DeficiencyGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName; ;

            if (CommandName.ToUpper().Equals("COPY"))
            {
                if (!IsValidDailyWorkPlan())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    PhoenixInspectionDailyWorkPlanActivity.DailyWorkPlanCopy(General.GetNullableGuid(ViewState["lblWorkPlanId"].ToString()), General.GetNullableDateTime(ucDate.Text));

                    String scriptpopupclose = String.Format("javascript:fnReloadList('codehelp1', 'null');");

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

    private bool IsValidDailyWorkPlan()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;

        if (General.GetNullableDateTime(ucDate.Text) == null)
            ucError.ErrorMessage = "Intended Date of Work is Required.";
        else if (DateTime.TryParse(ucDate.Text, out resultdate) && DateTime.Compare(DateTime.Parse(DateTime.Now.ToLongDateString()), resultdate) > 0)
            ucError.ErrorMessage = "Intended Date of Work Should not be a Past Date";

        return (!ucError.IsError);
    }
}
