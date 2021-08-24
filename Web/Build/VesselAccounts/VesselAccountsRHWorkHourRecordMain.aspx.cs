using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web.UI;
using Telerik.Web.UI;

public partial class VesselAccountsRHWorkHourRecordMain : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Close", "CLOSE",ToolBarDirection.Right);
            MenuWorkHour.AccessRights = this.ViewState;
            MenuWorkHour.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["EMPID"] = null;
                ViewState["RHSTARTID"] = null;
                ViewState["CALENDERID"] = null;

                if (Request.QueryString["EMPID"] != null)
                    ViewState["EMPID"] = Request.QueryString["EMPID"].ToString();

                if (Request.QueryString["RHStartId"] != null)
                    ViewState["RHSTARTID"] = Request.QueryString["RHStartId"].ToString();

                if (Request.QueryString["CalenderId"] != null)
                    ViewState["CALENDERID"] = Request.QueryString["CalenderId"].ToString();

                if (Request.QueryString["SHIPCALENDERID"] != null)
                    ViewState["SHIPCALENDERID"] = Request.QueryString["SHIPCALENDERID"].ToString();
            }
            if (ViewState["CALENDERID"] != null)
            {
                ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsRHWorkHourRecord.aspx?EMPID=" + ViewState["EMPID"].ToString() + "&RHStartId=" + ViewState["RHSTARTID"].ToString() + "&CalenderId=" + ViewState["CALENDERID"].ToString() + "&SHIPCALENDERID=" + ViewState["SHIPCALENDERID"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuWorkHour_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            String script = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }
}
