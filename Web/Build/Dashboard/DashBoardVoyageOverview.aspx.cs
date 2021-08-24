using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;
using System.Data;

public partial class Dashboard_DashBoardVoyageOverview : PhoenixBasePage
{
   public string dateList = "";
    string conditionList = "";
    string windlist = "";
    string SOGlist = "";
    string Loglist = "";
    string CpSpeed = "";
	string Trim = "";
	string Displacement = "";
    string MeloadIndex = "";
    string MeLoadLimit = "";
	string Slip = "";
	string MeRpm = "";
	string FocRate = "";
	string CpWind = "";
	string vslStatus = "";
	string DocRate = "";
	string TrialNCR = "";
	string PowerOutput = "";

	public string seriesdata = "";

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
            //fromDateInput.Text = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy");
            //toDateInput.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbltitle.Text = lbltitle.Text + " - " + ViewState["vesselname"].ToString();
            BindData();
            
        }
    }
    protected void BindData()
    {

        string fromdate = fromDateInput.Text;
        string todate = toDateInput.Text;
        string vslcondition = conditionSelect.SelectedValue;
        string weather = weatherSelect.SelectedValue;
        int vesselid = int.Parse(ViewState["vesselid"].ToString());
        DataTable dt;
        try
        { 
        dt = PhoenixDashboardPerformance.DashboardVoyageoverview(vesselid, General.GetNullableDateTime(fromdate), General.GetNullableDateTime(todate)
                                                            , General.GetNullableInteger(vslcondition), General.GetNullableInteger(weather));
            if (dt.Rows.Count > 0)
            {
                int i = 0;
                while (i < dt.Rows.Count)
                {
                    conditionList = conditionList + "'" + dt.Rows[i]["FLDCONDITION"].ToString() + "'" + ",";
					windlist = windlist + dt.Rows[i]["FLDACTUALWIND"].ToString() + ",";
					SOGlist = SOGlist + dt.Rows[i]["FLDVSLSOG"].ToString() + ",";
					Loglist = Loglist + dt.Rows[i]["FLDVSLLOG"].ToString() + ",";
					CpSpeed = CpSpeed + dt.Rows[i]["FLDCPSPEED"].ToString() + ",";
					Trim = Trim + dt.Rows[i]["FLDTRIM"].ToString() + ",";
					Displacement = Displacement + dt.Rows[i]["FLDDISPLACEMENT"].ToString() + ",";
					MeloadIndex = MeloadIndex + dt.Rows[i]["FLDMEINDEX"].ToString() + ",";
					MeLoadLimit = MeLoadLimit + dt.Rows[i]["FLDLOAD"].ToString() + ",";
					Slip = Slip + dt.Rows[i]["FLDSLIP"].ToString() + ",";
					MeRpm = MeRpm + dt.Rows[i]["FLDMERPM"].ToString() + ",";
					FocRate = FocRate + dt.Rows[i]["FLDVSLFOCRATE"].ToString() + ",";
					DocRate = DocRate + dt.Rows[i]["FLDVSLDOCRATE"].ToString() + ",";
					CpWind = CpWind + dt.Rows[i]["FLDCPWIND"].ToString() + ",";
					vslStatus = vslStatus + "'" + dt.Rows[i]["FLDVESSELSTATUS"].ToString() + "'" + ",";
					TrialNCR = TrialNCR + "'" + dt.Rows[i]["FLDSEATRIALNCRBHP"].ToString() + "'" + ",";
					PowerOutput = PowerOutput + "'" + dt.Rows[i]["FLDPOWEROUTPUT"].ToString() + "'" + ",";
					//DocRate = DocRate + dt.Rows[i]["FLDVSLDOCRATE"].ToString() + ",";

					dateList = dateList + "'" + dt.Rows[i]["FLDDATE"].ToString() + "'" + ",";
                    i = i + 1;
                }
                conditionList = "[" + conditionList.TrimEnd(',') + "],";
				windlist = "[" + windlist.TrimEnd(',') + "],";
				SOGlist = "[" + SOGlist.TrimEnd(',') + "],";
				Loglist = "[" + Loglist.TrimEnd(',') + "],";
				CpSpeed = "[" + CpSpeed.TrimEnd(',') + "],";
				Trim = "[" + Trim.TrimEnd(',') + "],";
				Displacement = "[" + Displacement.TrimEnd(',') + "],";
				MeloadIndex = "[" + MeloadIndex.TrimEnd(',') + "],";
				MeLoadLimit = "[" + MeLoadLimit.TrimEnd(',') + "],";
				Slip = "[" + Slip.TrimEnd(',') + "],";
				MeRpm = "[" + MeRpm.TrimEnd(',') + "],";
				FocRate = "[" + FocRate.TrimEnd(',') + "],";
				DocRate = "[" + DocRate.TrimEnd(',') + "],";
				CpWind = "[" + CpWind.TrimEnd(',') + "],";
				vslStatus = "[" + vslStatus.TrimEnd(',') + "],";
				TrialNCR = "[" + TrialNCR.TrimEnd(',') + "],";
				PowerOutput = "[" + PowerOutput.TrimEnd(',') + "]";
				//DocRate = "[" + DocRate.TrimEnd(',') + "]";

				seriesdata = "[" + conditionList + windlist + SOGlist + Loglist + CpSpeed + Trim + Displacement + MeloadIndex + MeLoadLimit + Slip + MeRpm + FocRate + DocRate + CpWind + vslStatus + TrialNCR + PowerOutput + "]";
                dateList = "[" + dateList.TrimEnd(',') + "]";
            }
        }

        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void conditionChange()
    {
        string test1;
        string test = "test";
        test1 = test;
        
    }

    protected void conditionSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }

    protected void weatherSelect_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }
protected void fromDateInput_TextChanged1(object sender, EventArgs e)
    {
        BindData();
    }

    protected void toDateInput_TextChanged(object sender, EventArgs e)
    {
        BindData();
    }
}
