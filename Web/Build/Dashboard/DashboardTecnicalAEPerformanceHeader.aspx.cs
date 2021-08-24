using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Dashboard;
using SouthNests.Phoenix.Framework;

public partial class Dashboard_DashboardTecnicalAEPerformanceHeader : PhoenixBasePage
{
	public string vesselName;
	public string AeModel;
	public string AeMaker;
	public string TypeofTC;
	public string AENo;

	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			if (Request.QueryString["vesselid"] != null)
				ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
			else
				ViewState["vesselid"] = "0";

			if (Request.QueryString["AeNo"] != null)
				ViewState["AeNo"] = Request.QueryString["AeNo"].ToString();
			else
				ViewState["AeNo"] = "";
		}
		int vesselid = int.Parse(ViewState["vesselid"].ToString());
		int AeNo = int.Parse(ViewState["AeNo"].ToString());
		EngineDetails(AeNo, vesselid);
	}

	protected void EngineDetails(int AeNo, int vesselid)
	{
		vesselid = int.Parse(ViewState["vesselid"].ToString());
		vesselName = "-";
		AeModel = "-";
		AeMaker = "-";
		TypeofTC = "-";
		AENo = "-";
		DataTable dt = new DataTable();
		dt = PhoenixDashboardAEPerformance.AeEngineDetails(vesselid, AeNo);
		if (dt.Rows.Count > 0)
		{

			vesselName = dt.Rows[0]["FLDVESSELNAME"].ToString();
			AeModel = dt.Rows[0]["FLDTYPEOFENGINE"].ToString();
			AeMaker = dt.Rows[0]["FLDAEMAKER"].ToString();
			TypeofTC = dt.Rows[0]["FLDTYPEOFTC"].ToString();
			AENo = dt.Rows[0]["FLDAE"].ToString();

		}
	}
}
