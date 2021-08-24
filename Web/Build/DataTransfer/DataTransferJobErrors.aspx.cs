using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DataTransfer;
using Telerik.Web.UI;

public partial class DataTransferJobErrors : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Jobs", "SHOWJOBS", ToolBarDirection.Right);
        MenuJobs.MenuList = toolbar.Show();

        scheduleBind();
        traceBind();

    }

    private void traceBind()
    {
        DataTable dt = null;
        dt = PhoenixDataTransferImport.DataTransferDebugTraceList(new Guid(Request.QueryString["scheduledjobid"].ToString()));
        gvTrace.DataSource = dt;
        gvTrace.Rebind();
        gvTrace.VirtualItemCount = 0;
    }

    private void scheduleBind()
    {
        DataSet ds = null;
        ds = PhoenixDataTransferImport.DataTransferScheduledJobsErrors(
            new Guid(Request.QueryString["scheduledjobid"].ToString()));
        gvFolderList.DataSource = ds;
        gvFolderList.Rebind();
        gvFolderList.VirtualItemCount = 0;
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
            scheduleBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTrace_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            traceBind();
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
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Jobs_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SHOWJOBS"))
        {
            Response.Redirect("../DataTransfer/DataTransferJobs.aspx?vesselid=" + Request.QueryString["vesselid"].ToString());
        }
    }

}
