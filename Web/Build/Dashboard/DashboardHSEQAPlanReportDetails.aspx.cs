using System;
using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using SouthNests.Phoenix.Common;
using System.Web.UI;

public partial class Dashboard_DashboardHSEQAPlanReportDetails : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
       

        ViewState["HSEQASCHEDULEID"] = General.GetNullableGuid(Request.QueryString["hseqascheduleid"]);

        DataTable dt = PhoenixDashboardHSEQAPlanner.HSEQAScheduleDetails(General.GetNullableGuid(Request.QueryString["hseqascheduleid"]));
        radlblliname.Text = dt.Rows[0]["FLDLINAME"].ToString();
        radlblinterval.Text = dt.Rows[0]["FLDFREQUENCY"].ToString() + " " + dt.Rows[0]["FLDFREQUENCYTYPE"].ToString();
        radlblPlanneddate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDPLANNEDDATE"].ToString());
        tbremarksentry.Text = dt.Rows[0]["FLDREMARKS"].ToString();
        reasonfornoattachments.Text = dt.Rows[0]["FLDREASON"].ToString();
        reasonfornoattachments.ReadOnly = true;
        tbremarksentry.ReadOnly = true;
        radlastdonedate.ReadOnly = true;
        radlastdonedate.Text = dt.Rows[0]["FLDDONEDATE"].ToString();
        Guid? flddtkey = General.GetNullableGuid(dt.Rows[0]["FLDDTKEY"].ToString());
        attachments.Attributes["src"] = "" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + flddtkey + "&mod="
                       + PhoenixModule.QUALITY + "&U=N";
        int attachment = 0;
        PhoenixDashboardHSEQAPlanner.attachmentyn(flddtkey, ref attachment);
        if (attachment == 0)
        {

            Noattachmentreasontitle.Visible = true;
            Noattachmentreasontitle1.Visible = true;
            reasonfornoattachments.Visible = true;
        }
        else
        {

            Noattachmentreasontitle.Visible = false;
            Noattachmentreasontitle1.Visible = false;
            reasonfornoattachments.Visible = false;
            reasonfornoattachments.Text = string.Empty;

        }
    }
}