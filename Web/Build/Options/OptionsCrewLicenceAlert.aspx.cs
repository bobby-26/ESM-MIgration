using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;

public partial class OptionsCrewLicenceAlert : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			string tasktype = Request.QueryString["tasktype"].ToString();
			Filter.CurrentAlertTaskType = tasktype;

			ViewState["SORTEXPRESSION"] = null;
			ViewState["SORTDIRECTION"] = null;
		}
		GetAlertItems(Filter.CurrentAlertTaskType.ToString());
	}

	private void GetAlertItems(string tasktype)
	{
		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;

		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		if (tasktype.Equals("8"))
		{
			gvAlertsTask.DataSource = PhoenixRegistersAlerts.CrewLicenceVerification(
				PhoenixSecurityContext.CurrentSecurityContext.UserCode
			, sortexpression, sortdirection);
			gvAlertsTask.DataBind();
		}

		
	}

	protected void gvAlertsTask_Sorting(object sender, GridViewSortEventArgs se)
	{
		gvAlertsTask.SelectedIndex = -1;
		gvAlertsTask.EditIndex = -1;

		ViewState["SORTEXPRESSION"] = se.SortExpression;

		if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
			ViewState["SORTDIRECTION"] = 1;
		else
			ViewState["SORTDIRECTION"] = 0;

		GetAlertItems(Filter.CurrentAlertTaskType.ToString());
	}

	protected void gvAlertsTask_RowCommand(object sender, GridViewCommandEventArgs e)
	{
		if (e.CommandName.ToUpper().Equals("SORT"))
			return;

		string tasktype = Request.QueryString["tasktype"].ToString();

		
		if (int.Parse(tasktype) == 8)
		{
			int nRow = int.Parse(e.CommandArgument.ToString());
			GridView _gridView = (GridView)sender;

			PhoenixRegistersAlerts.AlertViewHistoryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
				int.Parse(tasktype), ((Label)_gridView.Rows[nRow].FindControl("lblTaskKey")).Text);

			Filter.CurrentCrewSelection = ((Label)_gridView.Rows[nRow].FindControl("lblExpression")).Text;

            string Script = "";
            Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";
            if (((Label)_gridView.Rows[nRow].FindControl("lblDocType")).Text == "1")
                Script += "parent.OpenSearchPage('Crew/CrewPersonalGeneral.aspx?licence=1&empid=" + ((Label)_gridView.Rows[nRow].FindControl("lblExpression")).Text + "', 'CRW-PER-PER');";
            
			else if (((Label)_gridView.Rows[nRow].FindControl("lblDocType")).Text == "5")
                Script += "parent.OpenSearchPage('Crew/CrewPersonalGeneral.aspx?med=1&empid=" + ((Label)_gridView.Rows[nRow].FindControl("lblExpression")).Text + "', 'CRW-PER-PER');";

			else
                Script += "parent.OpenSearchPage('Crew/CrewPersonalGeneral.aspx?documents=1&empid=" + ((Label)_gridView.Rows[nRow].FindControl("lblExpression")).Text + "', 'CRW-PER-PER');";

            Script += "</script>" + "\n";

            Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

			//Response.Redirect("../Crew/CrewPersonalGeneral.aspx?licence=1&empid=" + ((Label)_gridView.Rows[nRow].FindControl("lblExpression")).Text, false);
		}
	}

	protected void gvAlertsTask_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
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
            Label lblExp = (Label)e.Row.FindControl("lblExpiryDate");
            if (lblExp != null && !String.IsNullOrEmpty(lblExp.Text))
            {
                if(DateTime.Parse(lblExp.Text) < DateTime.Today.Date)
                    e.Row.CssClass = "redfont";
            }
        }
	}
}
