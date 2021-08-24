using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;

public partial class Dashboard_DashboardVesselFailedImports : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        DataSet ds = null;
        ds = PhoenixCommonDashboard.DashboardVesselFailedImports();
        gvFolderList.DataSource = ds;
        gvFolderList.DataBind();
    }

    protected void gvFolderList_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string scheduledjobid = ((Label)e.Row.FindControl("lblScheduledJobID")).Text;
            string vesselid = ((Label)e.Row.FindControl("lblVesselId")).Text;
            //Response.Redirect("DataTransferJobErrors.aspx?vesselid=" + vesselid + "&scheduledjobid=" + scheduledjobid, false);
            LinkButton lbr = (LinkButton)e.Row.FindControl("lblLastRunOk");
            lbr.Attributes.Add("onclick", "javascript:parent.Openpopup('Errors','','../Dashboard/DashboardImportError.aspx?vesselid=" + vesselid + "&scheduledjobid=" + scheduledjobid + "'); return false;");
        }
    }

    protected void gvFolderList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
