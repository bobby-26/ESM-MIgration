using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;

public partial class InspectionMOCCopyTemplate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Copy", "COPY");
            MenuComment.AccessRights = this.ViewState;
            MenuComment.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["VESSELID"] != null)
                {
                    ViewState["VESSELID"] = Request.QueryString["VESSELID"];
                    ucVesselFrom.SelectedVessel = ViewState["VESSELID"].ToString();
                }
                if (Request.QueryString["MOCID"] != null)
                {
                    ViewState["MOCID"] = Request.QueryString["MOCID"];
                }
                ucCompany.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void MenuComment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("COPY"))
            {
                if (string.IsNullOrEmpty(ucVesselTo.SelectedVessel) || ucVesselTo.SelectedVessel.ToString().ToUpper() == "DUMMY")
                {
                    ucError.ErrorMessage = "To Vessel is required.";
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["MOCID"] != null && ViewState["VESSELID"] != null)
                {
                    PhoenixInspectionMOCTemplate.MOCCopyInsert(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["MOCID"].ToString()),
                        int.Parse(ucVesselTo.SelectedVessel),
                        General.GetNullableInteger(ucCompany.SelectedCompany));

                }
            }
            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucVesselTo_TextChangedEvent(object sender, EventArgs e)
    {
        ucCompany.SelectedCompany = string.Empty;
        if (ucVesselTo.SelectedVessel == "0")
        {     
            ucCompany.Enabled = true;
            ucCompany.CssClass = "input_mandatory";
        }
        else
        {            
            ucCompany.Enabled = false;
            ucCompany.CssClass = "input";
        }
    }
}
