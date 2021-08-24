using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;

public partial class Dashboard_DashboardV2Chart : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
		string vessellist = "";
		string valuelist = "";
		string colorlist = "";
		string measurename = "";
		SessionUtil.PageAccessRights(this.ViewState);		
		ViewState["MODULE"] = string.Empty;
		if (!string.IsNullOrEmpty(Request.QueryString["mod"]))
		{
			ViewState["MODULE"] = Request.QueryString["mod"];
		}
		ViewState["MID"] = string.Empty;
		if (!string.IsNullOrEmpty(Request.QueryString["mid"]))
		{
			ViewState["MID"] = Request.QueryString["mid"];
		}
		if (ViewState["MODULE"].ToString() != string.Empty && ViewState["MID"].ToString() != string.Empty)
		{
			DataSet ds = PhoenixDashboardTechnical.DashboardMeasureChart(ViewState["MODULE"].ToString()
				 , new Guid(ViewState["MID"].ToString())
				, null);
			if (ds.Tables[0].Rows.Count > 0)
			{
				DataTable dt = ds.Tables[0];
				int i = 0;
				measurename = dt.Rows[0]["FLDMEASURENAME"].ToString();
				while (i < dt.Rows.Count)
				{
					vessellist = vessellist + "\"" + dt.Rows[i]["FLDVESSELNAME"].ToString() + "\"" + ",";
					valuelist = valuelist + (dt.Rows[i]["FLDMEASURE"].ToString() == string.Empty ? "'-'" : dt.Rows[i]["FLDMEASURE"].ToString()) + ",";
					colorlist = colorlist + "'" + (dt.Rows[i]["FLDCOLOR"].ToString() == string.Empty ? "#27727B" : dt.Rows[i]["FLDCOLOR"].ToString()) + "'" + ",";
					i++;
				}
				vessellist = vessellist.TrimEnd(',');
				valuelist = valuelist.TrimEnd(',');
				colorlist = colorlist.TrimEnd(',');
			}
			ScriptManager.RegisterStartupScript(this, this.GetType(), "chart popup", "var dataSName = [" + vessellist + "]; var dataValues= [" + valuelist + "]; var colourList = [" + colorlist + "]; callDia('" + measurename.ToString() + "', '', 'PMS',dataSName,dataValues,colourList);", true);
		}
	}
}