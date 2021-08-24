using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class PlannedMaintenanceWorkOrderLogPTW : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {

            ViewState["WorkorderId"] = null;
            if (Request.QueryString["WORKORDERID"] != null)
            {
                ViewState["WorkorderId"] = Request.QueryString["WORKORDERID"].ToString();
            }

        }
    }

    protected void gvPTW_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = PhoenixPlannedMaintenanceWorkOrderGroupExtn.SubworkorderlogPTW(new Guid(ViewState["WorkorderId"].ToString()));
        gvPTW.DataSource = dt;
    }

    protected void gvPTW_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {
            LinkButton btnJha = (LinkButton)e.Item.FindControl("lnkJhaNo");
            RadLabel jhaId = ((RadLabel)e.Item.FindControl("lblJhaId"));
            if (btnJha != null)
            {
                if (jhaId != null)
                {
                    btnJha.Attributes.Add("onclick", "javascript:openNewWindow('JHA','','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=JOBHAZARDNEW&jobhazardid=" + jhaId.Text + "&showmenu=0&showword=NO&showexcel=NO',400,500); return false;");
                }
            }

            LinkButton report = (LinkButton)e.Item.FindControl("lnkFormNo");
            string reportId = ((RadLabel)e.Item.FindControl("lblReportId")).Text;
            string revisionId = ((RadLabel)e.Item.FindControl("lblRevision")).Text;
            if (report != null)
            {
                if (!string.IsNullOrEmpty(reportId) && drv["FLDTYPENAME"].ToString().ToUpper() != "UPLOAD")
                {
                    report.Visible = true;
                    report.Attributes.Add("onclick", "javascript:openNewWindow('PTWForm','','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&ReportId=" + reportId + "&FORMREVISIONID="+ revisionId+"',400,500); return false;");
                }
                else
                {
                    report.Enabled = false;
                    report.Attributes["style"] = "color:Black !important;";
                }
            }
            ImageButton cmdAtt = (ImageButton)e.Item.FindControl("cmdAtt");
            if (cmdAtt != null)
            {
                cmdAtt.Attributes.Add("onclick", "javascript:openNewWindow('cmnattachment','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDREPORTID"].ToString() + "&mod=" + PhoenixModule.DOCUMENTMANAGEMENT + "&DocSource=PTW&RefreshWindowName=maint,RadWindow_NavigateUrl'); return false;");
                cmdAtt.Visible = drv["FLDTYPENAME"].ToString().ToUpper() == "UPLOAD" && SessionUtil.CanAccess(this.ViewState, cmdAtt.CommandName);
            }
        }
    }
}
