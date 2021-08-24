using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;

public partial class CrewOffshoreDMRMidNightReportRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE");

            MenuRevoke.AccessRights = this.ViewState;
            MenuRevoke.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                

                ViewState["MidNightReportID"] = Request.QueryString["MidNightReportID"];
                if (Request.QueryString["SatC"] == "no")
                {
                    lblSatC.Visible = true;
                    txtSatC.Visible = true;
                }
                if (Request.QueryString["CCTV"] == "no")
                {
                    lblcctv.Visible = true;
                    txtcctv.Visible = true;
                }
                if (Request.QueryString["HiPap"] == "no")
                {
                    lblHiPap.Visible = true;
                    txtHiPap.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuRevoke_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {
            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportRemarksUpdate(new Guid(ViewState["MidNightReportID"].ToString())
                                                                               , txtSatC.Text
                                                                               , txtcctv.Text
                                                                               , txtHiPap.Text);
                string script = "javascript:fnReloadList('codehelp1');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
