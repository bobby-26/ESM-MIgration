using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;

public partial class CrewPlanRelieveeEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);

            CrewRelieverTabs.AccessRights = this.ViewState;
            CrewRelieverTabs.MenuList = toolbarmain.Show();
            CrewRelieverTabs.SelectedMenuIndex = 0;
            if (!IsPostBack)
            {
                ViewState["CREWPLANID"] = Request.QueryString["crewplanid"];
                ViewState["EMPID"] = Request.QueryString["empid"];
                ViewState["VESSELID"] = Request.QueryString["vesselid"];
                if (ViewState["CREWPLANID"] != null)
                    Edit();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    private void Edit()
    {
        DataTable dt = null;

        dt = PhoenixCrewPlanning.EditCrewPlan(new Guid(ViewState["CREWPLANID"].ToString()));
        if (dt.Rows.Count > 0)
        {
            txtonsignername.Text = dt.Rows[0]["FLDEMPLOYEENAME"].ToString();
            txtVessel.Text = dt.Rows[0]["FLDVESSELNAME"].ToString();
            txtoffsignername.Text = dt.Rows[0]["FLDOFFSIGNERNAME"].ToString();
            txtoffsignerRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
            ucport.SelectedValue = dt.Rows[0]["FLDSEAPORTID"].ToString();
            ucport.Text = dt.Rows[0]["FLDSEAPORTNAME"].ToString();
            txtPlannedReliefDate.Text = dt.Rows[0]["FLDEXPECTEDJOINDATE"].ToString();
            txtDateofReadiness.Text = dt.Rows[0]["FLDDATEOFREADINESS"].ToString();
            txtcrewchangeremarks.Text = dt.Rows[0]["FLDCREWCHANGEREMARKS"].ToString();
            ViewState["Vesselid"] = dt.Rows[0]["FLDVESSELID"].ToString();
        }
    }

    protected void CrewRelieverTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixCrewPlanning.UpdateCrewPlan(new Guid(ViewState["CREWPLANID"].ToString())
                                                   , int.Parse(ViewState["VESSELID"].ToString())
                                                   , General.GetNullableInteger(ViewState["EMPID"].ToString())
                                                   , DateTime.Parse(txtPlannedReliefDate.Text)
                                                   , General.GetNullableInteger(ucport.SelectedValue)
                                                   , string.Empty, string.Empty
                                                   , General.GetNullableDateTime(txtDateofReadiness.Text)
                                                   , txtcrewchangeremarks.Text.Trim());
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('Edit','ifMoreInfo');";
                Script += "</script>" + "\n";
                
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}
