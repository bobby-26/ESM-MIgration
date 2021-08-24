using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class OptionsAlerts : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        gvAlert.DataSource = PhoenixRegistersAlerts.GetTaskTypeList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null);
        gvAlert.DataBind();
    }

    protected void gvAlert_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridview = (GridView)sender;
        int currentRow = int.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToUpper().Equals("ALERTTASK"))
        {
            Label lbl = (Label)_gridview.Rows[currentRow].FindControl("lblTaskType");
            LinkButton lb = (LinkButton)_gridview.Rows[currentRow].FindControl("lblDescription");

            if (lbl.Text == "5" || lbl.Text == "6" || lbl.Text == "7" || lbl.Text == "12")
                Response.Redirect("OptionsAlertFollowUp.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text, false);
            else if (lbl.Text == "8")
                Response.Redirect("OptionsCrewLicenceAlert.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text, false);
            else
                Response.Redirect("OptionsAlertsTask.aspx?tasktype=" + lbl.Text + "&description=" + lb.Text, false);

        }
    }

    protected void gvAlert_RowEditing(object sender, GridViewEditEventArgs de)
    {

    }


    protected void gvAlert_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
        }
    }

    protected void CheckBoxClicked(object sender, EventArgs e)
    {
        CheckBox cb = (CheckBox)sender;
        int nCurrentRow = Int32.Parse(cb.Text);
        string alertid = ((Label)gvAlert.Rows[nCurrentRow].FindControl("lblAlertCode")).Text;
        //PhoenixRegistersAlerts.SubscribeAlert(int.Parse(alertid), PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    }
}
