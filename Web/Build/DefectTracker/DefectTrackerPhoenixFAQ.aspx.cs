using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTracker_DefectTrackerPhoenixFAQ : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void BindData()
    {
        DataTable dt = PhoenixDefectTracker.FAQList();

        if (dt.Rows.Count > 0)
        {
            repDiscussion.DataSource = dt;
            repDiscussion.DataBind();
        }
        else
        {
            ShowNoRecordsFound(dt, repDiscussion);
        }
    }
    protected void repFAQ_OnItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if ((e.Item.ItemType == ListItemType.Item) || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            HyperLink lnk = (HyperLink)e.Item.FindControl("lblAttachments");
            Label lblFileName = (Label)e.Item.FindControl("lblFile");
            Label lblFilePath = (Label)e.Item.FindControl("lblFilePath");
            if (lblFileName.Text != "")
            {
                lnk.Text = lblFileName.Text;
                lnk.NavigateUrl = "DefectTrackerMailAttachment.ashx?path=" + lblFilePath.Text + "\\" + lblFileName.Text;
            }
            else
                lnk.Text = "N/A";
        }
    }

    private void ShowNoRecordsFound(DataTable dt, Repeater rep)
    {
        dt.Rows.Add(dt.NewRow());
        rep.DataSource = dt;
        rep.DataBind();
    }
}
