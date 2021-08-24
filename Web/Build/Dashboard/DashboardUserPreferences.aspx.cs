using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Text;

public partial class Dashboard_DashboardUserPreferences : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuDashboardPreferences.AccessRights = this.ViewState;
        MenuDashboardPreferences.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
        }
    }

    protected void MenuDashboardPreferences_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                SaveDashboardPreferences();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindData()
    {
        DataSet ds = new DataSet();

        if (Request.QueryString["usercode"] != null)
        {
            ds = PhoenixCommonDashboard.DashboardPreferencesSearch(null,
                                                        int.Parse(rblType.SelectedValue),
                                                        int.Parse(Request.QueryString["usercode"].ToString()),
                                                        null);
        }
        else
        {
            ds = PhoenixCommonDashboard.DashboardPreferencesSearch(PhoenixSecurityContext.CurrentSecurityContext.UserType,
                                                                    int.Parse(rblType.SelectedValue),
                                                                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    null);
        }
        gvDashBoardPreferences.DataSource = ds;
   
       }

    protected void SaveDashboardPreferences()
    {
        string strpreferences = ",";

        foreach (GridDataItem row in gvDashBoardPreferences.MasterTableView.Items)
            {
            string dashboardcode = ((RadLabel)row.FindControl("lblCOde")).Text;
            string codevalue = ((UserControlMaskNumber)row.FindControl("txtSequence")).Text;
            //string dashboardcode = ((RadLabel)gvDashBoardPreferences.Items.FindControl("lblCOde")).Text;

            //  string codevalue = ((UserControlMaskNumber)gvDashBoardPreferences.Rows[grv.RowIndex].FindControl("txtSequence")).Text;

            if (!string.IsNullOrEmpty(codevalue))
            {
                strpreferences += dashboardcode + "=" + codevalue + ",";
            }
            else
            {
                strpreferences += dashboardcode + "=0,";
            }
        }

        int usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        if (Request.QueryString["usercode"] != null)
        {
            usercode = int.Parse(Request.QueryString["usercode"].ToString());
        }

        PhoenixCommonDashboard.DashboardPreferencesUpdate(usercode
                                                            , strpreferences
                                                            , int.Parse(rblType.SelectedValue)
                                                            , PhoenixSecurityContext.CurrentSecurityContext.UserType);

        ucStatus.Text = "Preferences Updated.";
    }
    protected void Rebind()
    {
        gvDashBoardPreferences.SelectedIndexes.Clear();
        gvDashBoardPreferences.EditIndexes.Clear();
        gvDashBoardPreferences.DataSource = null;
        gvDashBoardPreferences.Rebind();
    }

    protected void rblType_SelectedIndexChanged(object sender, EventArgs e)
    {
        Rebind();

    }

    protected void gvDashBoardPreferences_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDashBoardPreferences.CurrentPageIndex + 1;
        BindData();
    }
}