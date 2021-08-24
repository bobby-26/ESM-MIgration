using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DataTransfer;


public partial class DataTransferJobProgress : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string schedulejobid = Request.QueryString["scheduledjobid"].ToString();
        string transfercode = Request.QueryString["transfercode"] == null? "" : Request.QueryString["transfercode"].ToString();
        gvProgress.DataSource = PhoenixDataTransferImport.DataSynchronizerProgressList(new Guid(schedulejobid), General.GetNullableGuid(transfercode));
        gvProgress.DataBind();
    }

    protected void gvProgress_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("SHOWTHIS"))
            {
                string scheduledjobid = Request.QueryString["schedulejobid"].ToString();
                string transfercode = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblTransferCode")).Text;
                Response.Redirect("DataTransferJobProgress.aspx?transfercode=" + transfercode + "&scheduledjobid=" + scheduledjobid, false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
