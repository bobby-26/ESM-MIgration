using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Collections.Specialized;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;

public partial class DashboardCommon : PhoenixBasePage
{
	protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

			if (Request.QueryString["ModuleType"] != null)                                  //Not redirect from Dashboard.aspx. it  redirect from main.js
			{
				ViewState["ModuleType"] = Request.QueryString["ModuleType"].ToString();     
				AccessScreen();
			}
			else
			{
				ViewState["ModuleType"] = "";
			}
			SelectedScreen();
        }
    }

    private void SelectedScreen()
    {        
        NameValueCollection lsnvc = Filter.CurrentDashboardLastSelection;
        if (lsnvc != null && !string.IsNullOrEmpty(lsnvc.Get("SelectedModuleScreen")))
        {
            string ModuleScreenUrl = lsnvc.Get("SelectedModuleScreen");
            Filter.CurrentDashboardLastSelection["SelectedModuleScreen"] = "";
            Response.Redirect(ModuleScreenUrl, true);
        }
        DataSet ds = PhoenixDashboardOption.DashboardLastSelectedEdit();
		//if (ds.Tables[0].Rows.Count > 0 && lsnvc != null)                   // check redirect from login page or redirect from other page
		if (ds.Tables[0].Rows.Count > 0)                   
		{
			//NameValueCollection querystring = new NameValueCollection();
			//querystring.Add("APP", ds.Tables[0].Rows[0]["FLDSELECTEDMENU"].ToString());
			//querystring.Add("Option", ds.Tables[0].Rows[0]["FLDSELECTEDOPTION"].ToString());
			//querystring.Add("VesselName", ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString());
			//querystring.Add("RankName", ds.Tables[0].Rows[0]["FLDRANKNAME"].ToString());
			//querystring.Add("MeasureName", ds.Tables[0].Rows[0]["FLDMEASURENAME"].ToString());
			//querystring.Add("VesselId", ds.Tables[0].Rows[0]["FLDVESSELID"].ToString());
			//querystring.Add("RankId", ds.Tables[0].Rows[0]["FLDRANKID"].ToString());
			//querystring.Add("measureid", ds.Tables[0].Rows[0]["FLDMEASUREID"].ToString());
			//querystring.Add("Row", ds.Tables[0].Rows[0]["FLDSELECTEDROW"].ToString());
			//querystring.Add("Col", ds.Tables[0].Rows[0]["FLDSELECTEDCOLUMN"].ToString());
			//querystring.Add("ResultRow", ds.Tables[0].Rows[0]["FLDSELECTEDRESULTROW"].ToString());
			//querystring.Add("VesselList", ds.Tables[0].Rows[0]["FLDVESSELLIST"].ToString());
			//querystring.Add("RankList", ds.Tables[0].Rows[0]["FLDRANKLIST"].ToString());        

			//int nRow = (ds.Tables[0].Rows[0]["FLDSELECTEDRESULTROW"].ToString() != "" ? int.Parse(ds.Tables[0].Rows[0]["FLDSELECTEDRESULTROW"].ToString()) + 2 : 2);
			//Response.Redirect(General.RedirectTo(Session["sitepath"].ToString() + ds.Tables[0].Rows[0]["FLDURL"].ToString().TrimStart('~'), querystring) + "#gvMeasureResult_ctl" + (nRow.ToString().Length == 1 ? "0" + nRow.ToString() : nRow.ToString()) + "_lblResultLink", false);
			Response.Redirect(General.RedirectTo(Session["sitepath"].ToString() + ds.Tables[0].Rows[0]["FLDURL"].ToString().TrimStart('~')), false);
		}
        else
        {
			AccessScreen();
        }
    }
	private void AccessScreen()
	{
		DataSet ds;
		ds = PhoenixDashboardOption.DashBoardModule(General.GetNullableInteger(ViewState["ModuleType"].ToString()));     //module type : 1.TECHNICAL 2.CREW
		if (ds.Tables[0].Rows.Count > 0)
		{
			string cmdName = "";
			cmdName = ds.Tables[0].Rows[0]["FLDCOMMANDNAME"].ToString();
			if (cmdName.ToUpper().Equals("PMS"))
			{
				Response.Redirect("../Dashboard/DashboardTechnicalPms.aspx");
			}
			if (cmdName.ToUpper().Equals("PURCHASE"))
			{
				Response.Redirect("../Dashboard/DashboardTechnicalPurchase.aspx");
			}
			if (cmdName.ToUpper().Equals("VETTING"))
			{
				Response.Redirect("../Dashboard/DashboardTechnicalVetting.aspx");
			}
			if (cmdName.ToUpper().Equals("INSPECTION"))
			{
				Response.Redirect("../Dashboard/DashboardTechnicalInspection.aspx");
			}
			if (cmdName.ToUpper().Equals("WRH"))
			{
				Response.Redirect("../Dashboard/DashboardTechnicalWorkRestHours.aspx");
			}
			if (cmdName.ToUpper().Equals("CERTIFICATE"))
			{
				Response.Redirect("../Dashboard/DashboardTechnicalCertificatesandSurveys.aspx");
			}
			if (cmdName.ToUpper().Equals("PERFORMANCE"))
			{
				Response.Redirect("../Dashboard/DashboardTechnicalPerformance.aspx");
			}
			if (cmdName.ToUpper().Equals("APPRAISAL"))
			{
				Response.Redirect("../Dashboard/DashboardCrewAppraisals.aspx");
			}
			if (cmdName.ToUpper().Equals("CLP"))
			{
				Response.Redirect("../Dashboard/DashboardCrewLegal.aspx");
			}
			if (cmdName.ToUpper().Equals("CNC"))
			{
				Response.Redirect("../Dashboard/DashboardCrewCrewList.aspx");
			}
			if (cmdName.ToUpper().Equals("ICEEXP"))
			{
				Response.Redirect("../Dashboard/DashboardCrewVesselPosition.aspx");
			}
			if (cmdName.ToUpper().Equals("ODE"))
			{
				Response.Redirect("../Dashboard/DashboardCrewOnboardDocumentExpiry.aspx");
			}
			if (cmdName.ToUpper().Equals("INVOICE"))
			{
				Response.Redirect("../Dashboard/DashboardCrewAccounts.aspx");
			}
			if (cmdName.ToUpper().Equals("CREW"))
			{
				Response.Redirect("../Dashboard/DashboardCrewAndFamily.aspx");
			}
			if (cmdName.ToUpper().Equals("ANALYTICS"))
			{
				Response.Redirect("../Dashboard/QualityPBI.html");
			}
		}
		else
		{
			Response.Redirect("DashboardVesselParticulars.aspx", false);
		}
	}
}
