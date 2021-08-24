using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerMailAttachments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["mailinoutid"] != null)
            ViewState["MAILINOUTID"] = Request.QueryString["mailinoutid"].ToString();

        BindData();
    }

    private void BindData()
    {
        DataTable dt = new DataTable();

        dt= PhoenixDefectTracker.GetMailAttachments(new Guid(ViewState["MAILINOUTID"].ToString()));

        gvAttachment.DataSource = dt;
        gvAttachment.DataBind();
    }

    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFileName = (Label)e.Row.FindControl("lblFileName");
            HyperLink lnk = (HyperLink)e.Row.FindControl("lnkfilename");
            lnk.NavigateUrl = "DefectTrackerMailAttachment.ashx?path=" + HttpUtility.UrlEncode(lblFileName.Text);
        }
    }
}
