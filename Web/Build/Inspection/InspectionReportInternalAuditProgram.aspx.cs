using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Framework;

public partial class InspectionReportInternalAuditProgram : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Show Report", "SHOWREPORT");                
                MenuReportsFilter.AccessRights = this.ViewState;
                MenuReportsFilter.MenuList = toolbar.Show();               
                ViewState["PAGEURL"] = "../Reports/ReportsView.aspx";

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    ifMoreInfo.Attributes["src"] = ViewState["PAGEURL"].ToString() + "?applicationcode=9&reportcode=INTERNALAUDITPLAN&reviewscheduleid=" + new Guid(ucAuditSchedule.SelectedAuditSchedule) + "&showmenu=0&showexcel=no";
                }
            }            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information";
       
        if (ucAuditSchedule.SelectedAuditSchedule.ToString() == "" || ucAuditSchedule.SelectedAuditSchedule.ToString() == "Dummy")
        {
            ucError.ErrorMessage = "Audit is Required";
        }

        return (!ucError.IsError);
    }
}
