using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DataTransfer;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class DataTransferJobs : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       // BindData();
    }

    private void BindData()
    {
        DataSet ds = null;
        ds = PhoenixDataTransferImport.DataTransferScheduledJobs(
            short.Parse(Request.QueryString["vesselid"].ToString()));

        gvFolderList.DataSource = ds;
        
    }

    protected void gvFolderList_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    if (ViewState["SORTEXPRESSION"] != null)
        //    {
        //        HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
        //        if (img != null)
        //        {
        //            if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
        //                img.Src = Session["images"] + "/arrowUp.png";
        //            else
        //                img.Src = Session["images"] + "/arrowDown.png";

        //            img.Visible = true;
        //        }
        //    }
        //}
    }

    protected void gvFolderList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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


    protected void gvFolderList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            GridEditableItem eeditedItem = e.Item as GridEditableItem;

            if (e.CommandName.ToUpper().Equals("SHOWERROR"))
            {
                string scheduledjobid = ((RadLabel)e.Item.FindControl("lblScheduledJobID")).Text;
                Response.Redirect("DataTransferJobErrors.aspx?vesselid=" + Request.QueryString["vesselid"].ToString() + "&scheduledjobid=" + scheduledjobid, false);
            }
            if (e.CommandName.ToUpper().Equals("PROGRESS"))
            {
                string scheduledjobid = ((RadLabel)e.Item.FindControl("lblScheduledJobID")).Text;
                Response.Redirect("DataTransferJobProgress.aspx?scheduledjobid=" + scheduledjobid, false);
            }
            if (e.CommandName.ToUpper().Equals("REMOVE"))
            {
                string scheduledjobid = ((RadLabel)e.Item.FindControl("lblScheduledJobID")).Text;
                PhoenixDataTransferImport.DataTransferRemoveScheduledJob(new Guid(scheduledjobid));
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
