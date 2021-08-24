using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;
using System.Data;

public partial class Dashboard_DashBoardCharterPartyPerformanceChart : PhoenixBasePage
{
   public string dateList = "";
    string conditionList = "";
    string CPwindlist = "";
    string vslWindList = "";
    string CpSOGlist = "";
    string vsSOGlist = "";
    string CpHFOlist = "";
    string vslHFOlist = "";
    string CpDOlist = "";
    string vslDOlist = "";
	string vslstatus = "";
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
        dt = PhoenixDashboardPerformance.DashboardCharterParty(vesselid, General.GetNullableDateTime(fromdate), General.GetNullableDateTime(todate)
                                                            , General.GetNullableInteger(vslcondition), General.GetNullableInteger(weather));
            if (dt.Rows.Count > 0)
            {
                int i = 0;
                while (i < dt.Rows.Count)
                {
                    conditionList = conditionList + "'" + dt.Rows[i]["FLDCONDITION"].ToString() + "'" + ",";
                    CPwindlist = CPwindlist + dt.Rows[i]["FLDCPWIND"].ToString() + ",";
                    vslWindList = vslWindList + dt.Rows[i]["FLDACTUALWIND"].ToString() + ",";
                    CpSOGlist = CpSOGlist + dt.Rows[i]["FLDCPSOG"].ToString() + ",";
                    vsSOGlist = vsSOGlist + dt.Rows[i]["FLDVSLSOG"].ToString() + ",";
                    CpHFOlist = CpHFOlist + dt.Rows[i]["FLDCPHFO"].ToString() + ",";
                    vslHFOlist = vslHFOlist + dt.Rows[i]["FLDVSLHFO"].ToString() + ",";
                    CpDOlist = CpDOlist + dt.Rows[i]["FLDCPDO"].ToString() + ",";
                    vslDOlist = vslDOlist + dt.Rows[i]["FLDVLSDO"].ToString() + ",";
                    dateList = dateList + "'" + dt.Rows[i]["FLDDATE"].ToString() + "'" + ",";
					vslstatus = vslstatus + "'" + dt.Rows[i]["FLDVESSELSTATUS"].ToString() + "'" + ",";
					i = i + 1;
                }
                conditionList = "[" + conditionList.TrimEnd(',') + "],";
                CPwindlist = "[" + CPwindlist.TrimEnd(',') + "],";
                vslWindList = "[" + vslWindList.TrimEnd(',') + "],";
                CpSOGlist = "[" + CpSOGlist.TrimEnd(',') + "],";
                vsSOGlist = "[" + vsSOGlist.TrimEnd(',') + "],";
                CpHFOlist = "[" + CpHFOlist.TrimEnd(',') + "],";
                vslHFOlist = "[" + vslHFOlist.TrimEnd(',') + "],";
                CpDOlist = "[" + CpDOlist.TrimEnd(',') + "],";
                vslDOlist = "[" + vslDOlist.TrimEnd(',') + "],";
				vslstatus = "[" + vslstatus.TrimEnd(',') + "]";

				seriesdata = "[" + conditionList + CPwindlist + vslWindList + CpSOGlist + vsSOGlist + CpHFOlist + vslHFOlist + CpDOlist + vslDOlist + vslstatus + "]";
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
