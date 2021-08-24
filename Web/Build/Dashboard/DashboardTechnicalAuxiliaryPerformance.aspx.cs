using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;
using System.Web.Services;
using System.Web.Script.Serialization;

public partial class Dashboard_DashboardTechnicalAuxiliaryPerformance : PhoenixBasePage
{
	public string vesselName;
	public string AeModel;
	public string AeMaker;
	public string TypeofTC;
	public string AENo;

	public string shoptestSFOC = "";

	public string xdata = "";               //month list
	public int vesselid;
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			if (Request.QueryString["vesselid"] != null)
				ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();
			else
				ViewState["vesselid"] = "0";

			if (Request.QueryString["vesselname"] != null)
				ViewState["vesselname"] = Request.QueryString["vesselname"].ToString();
			else
				ViewState["vesselname"] = "";
		}
		vesselid = int.Parse(ViewState["vesselid"].ToString());
		EngineDetails(1,vesselid);
		
	}


	[WebMethod]
	public void EngineDetails(int AeNo,int vesselid)
	{
		vesselName = ViewState["vesselname"].ToString();
		vesselid = int.Parse(ViewState["vesselid"].ToString());
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

