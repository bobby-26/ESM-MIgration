using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DataTransfer;
using Telerik.Web.UI;
public partial class DashboardImportError : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {


    }

    protected void Jobs_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("SHOWJOBS"))
        {
            Response.Redirect("DataTransfer/DataTransferJobs.aspx?vesselid=" + Request.QueryString["vesselid"].ToString());
        }
    }

    protected void gvFolderList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = null;
            ds = PhoenixDataTransferImport.DataTransferScheduledJobsErrors(
                new Guid(Request.QueryString["scheduledjobid"].ToString()));
            gvFolderList.DataSource = ds;
           
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
            DataTable dt = null;
            dt = PhoenixDataTransferImport.DataTransferDebugTraceList(new Guid(Request.QueryString["scheduledjobid"].ToString()));
            gvTrace.DataSource = dt;          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
