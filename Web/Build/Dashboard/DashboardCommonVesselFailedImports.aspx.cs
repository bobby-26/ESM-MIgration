using System;
using System.Data;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class DashboardCommonVesselFailedImports : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SelectedOption();
        }
    }

    private void SelectedOption()
    {
        NameValueCollection criteria = new NameValueCollection();
        criteria.Add("App", "QUAL");
        criteria.Add("Option", "SYE");
        PhoenixDashboardOption.DashboardLastSelected(criteria);
    }

    protected void gvFolderList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = null;
            ds = PhoenixCommonDashboard.DashboardVesselFailedImports();
            gvFolderList.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFolderList_ItemDataBound(object sender, GridItemEventArgs e)
    {

        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                string scheduledjobid = ((RadLabel)item.FindControl("lblScheduledJobID")).Text;
                string vesselid = ((RadLabel)item.FindControl("lblVesselId")).Text;
                LinkButton lbr = (LinkButton)item.FindControl("lblLastRunOk");
                lbr.Attributes.Add("onclick", "javascript:parent.openNewWindow('Errors','codehelp1','" + Session["sitepath"] + "/Dashboard/DashboardImportError.aspx?vesselid=" + vesselid + "&scheduledjobid=" + scheduledjobid + "'); return false;");
              
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


}

