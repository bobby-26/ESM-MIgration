using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;

public partial class Dashboard_DashboardHSEQAPlannerDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            ViewState["LIID"] = General.GetNullableGuid(Request.QueryString["liid"]);
            ViewState["COLUMN"] = General.GetNullableString(Request.QueryString["columnid"]);
            SessionUtil.PageAccessRights(this.ViewState);
            gvHSEQAPLan.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            DataTable dt = PhoenixDashboardHSEQAPlanner.LIDetails(General.GetNullableGuid(Request.QueryString["liid"]));
            radlbllicode.Text = dt.Rows[0]["FLDLICODE"].ToString();
            Radlblname.Text = dt.Rows[0]["FLDLINAME"].ToString();
        }

    }

    protected void gvHSEQAPLan_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? HSEQAscheduleid = General.GetNullableGuid(item.GetDataKeyValue("FLDHSEQASCHEDULEID").ToString());

                LinkButton reportli = ((LinkButton)item.FindControl("reportbtn"));
                LinkButton info = ((LinkButton)item.FindControl("infobtn"));
                if (reportli != null)
                {
                    reportli.Attributes.Add("onclick", "javascript:parent.openNewWindow('Report','Report LI','Dashboard/DashboardHSEQAPlanReport.aspx?HSEQAscheduleid=" + HSEQAscheduleid + "');");

                }
                if (info != null)
                {
                    info.Attributes.Add("onclick", "javascript:parent.openNewWindow('Report','Report Details','Dashboard/DashboardHSEQAPlanReportDetails.aspx?HSEQAscheduleid=" + HSEQAscheduleid + "');");


                }

                int result = 0;
                PhoenixDashboardHSEQAPlanner.HSEQAPlanStatus(HSEQAscheduleid, ref result);
                if (result != 0)
                {
                    reportli.Visible = false;
                    info.Visible = true;
                }
                else
                    info.Visible = false;

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvHSEQAPLan_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }

    protected void gvHSEQAPLan_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        Guid? LIId = General.GetNullableGuid(ViewState["LIID"].ToString());
        string columnid = ViewState["COLUMN"].ToString();
        int IRowCount = 0;
        DataTable dt = PhoenixDashboardHSEQAPlanner.HSEQPlanOfWeek(LIId, columnid, gvHSEQAPLan.CurrentPageIndex + 1, gvHSEQAPLan.PageSize, ref IRowCount);
        gvHSEQAPLan.DataSource = dt;
        gvHSEQAPLan.VirtualItemCount = IRowCount;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
}