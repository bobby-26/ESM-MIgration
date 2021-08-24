using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using Telerik.Web.UI;

public partial class VesselAccountsReportRHNCAnalysisReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportNonCompliance.AccessRights = this.ViewState;
            MenuReportNonCompliance.MenuList = toolbar.Show();
            ViewState["REPORTPAGEURL"] = "../Reports/ReportsView.aspx";

            if (!IsPostBack)
            {
                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                BindYear();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year); i++)
        {
            ddlYear.Items.Add(i.ToString());
        }
        ddlYear.DataBind();
    }

    protected void MenuReportNonCompliance_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                ifMoreInfo.Attributes["src"] = ViewState["REPORTPAGEURL"].ToString() + "?applicationcode=7&reportcode=RESTHOURSNCSUMMARYNEW&showmenu=false&showexcel=no&showword=no&vesselid="
                    + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString()
                    + "&month=" + ddlMonth.SelectedValue
                    + "&year=" + ddlYear.SelectedItem.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlYear_DataBound(object sender, EventArgs e)
    {
        ddlYear.Items.Sort();
    }
}