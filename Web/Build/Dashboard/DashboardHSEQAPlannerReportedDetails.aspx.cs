using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.ComponentModel;
using SouthNests.Phoenix.Common;

public partial class Dashboard_DashboardHSEQAPlannerReportedDetails : System.Web.UI.Page
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

    protected void gvHSEQAPLan_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        Guid? LIId = General.GetNullableGuid(ViewState["LIID"].ToString());
        string columnid = ViewState["COLUMN"].ToString();
        int IRowCount = 0;
        DataTable dt = PhoenixDashboardHSEQAPlanner.HSEQCompletedPlanOfWeek(LIId, columnid, gvHSEQAPLan.CurrentPageIndex + 1, gvHSEQAPLan.PageSize, ref IRowCount);
        gvHSEQAPLan.DataSource = dt;
        gvHSEQAPLan.VirtualItemCount = IRowCount;
    }

    protected void gvHSEQAPLan_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;
                Guid? HSEQAscheduleid = General.GetNullableGuid(item.GetDataKeyValue("FLDHSEQASCHEDULEID").ToString());
                DataTable dt = PhoenixDashboardHSEQAPlanner.Getflddtkey(HSEQAscheduleid);
                Guid? flddtkey = General.GetNullableGuid(dt.Rows[0]["FLDDTKEY"].ToString());
                LinkButton attachments = ((LinkButton)item.FindControl("btnattachments"));
                if (attachments != null)
                {
                    attachments.Attributes.Add("onclick", "javascript:parent.openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + flddtkey + "&mod="
                           + PhoenixModule.QUALITY + "&U=n"+"'); return false;");
                }

                if (General.GetNullableString(dt.Rows[0]["FLDREASON"].ToString()) != null)
                { attachments.Visible = false; }
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
}