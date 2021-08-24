using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersNoonReportRangeConfigVesselMap : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
            PhoenixToolbar toolbarReporttap = new PhoenixToolbar();
            toolbarReporttap.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuNewSaveTabStrip.AccessRights = this.ViewState;
            MenuNewSaveTabStrip.MenuList = toolbarReporttap.Show();

            if (!IsPostBack)
            {
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        DataSet Ds = PhoenixRegistersVessel.ListActiveVessel(null, General.GetNullableString(""), 1);
        chkVesselList.DataSource = Ds;
        chkVesselList.DataBindings.DataTextField = "FLDVESSELNAME";
        chkVesselList.DataBindings.DataValueField = "FLDVESSELID";
        chkVesselList.DataBind();

        foreach (ButtonListItem li in chkVesselList.Items)
        {
            if (Request.QueryString["VESSELLIST"] != null)
            {
                string[] slist = Request.QueryString["VESSELLIST"].ToString().Split(',');
                foreach (string s in slist)
                {
                    if (li.Value.Equals(s))
                    {
                        li.Selected = true;
                    }
                }
            }
        }
    }

    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            string Vessellist = "";
            foreach (ButtonListItem li in chkVesselList.Items)
            {
                if (li.Selected)
                {
                    if (Vessellist == "")
                        Vessellist = ",";
                    Vessellist += li.Value + ",";
                }
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixRegistersNoonReportRangeConfig.NoonRangeConfigFieldCopytoVessels(
                General.GetNullableString(Request.QueryString["COLUMNNAME"].ToString()),
                 General.GetNullableString(Vessellist));

                String script = "javascript:fnReloadList('codehelp1',null);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void chkOption_OnCheckedChanged(Object sender, EventArgs args)
    {
        if (chkOption.Checked == true)
        {
            foreach (ButtonListItem li in chkVesselList.Items)
            {
                li.Selected = true;
            }
        }
        else
        {
            foreach (ButtonListItem li in chkVesselList.Items)
            {
                li.Selected = false;
            }
        }
    }
}
