using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using System.Data;
using Telerik.Web.UI;

public partial class VesselPositionVPRSChartCPDeviation : PhoenixBasePage
{
    public string dateList = "";
	public string seriesdata = "";
    public string seriesdata1 = "";
    string cpspeed = "";
    string sog = "";
    string cpfoc = "";
    string actualfoc = "";
    string speeddeviation = "";
	string focdeviation = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("CP Deviation", "CPDEVIATION");
        toolbarmain.AddButton("AE Load", "AELOAD");         
        MenuChart.AccessRights = this.ViewState;
        MenuChart.MenuList = toolbarmain.Show();

        MenuChart.SelectedMenuIndex = 0;
        
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

            //toDateInput.Text = General.GetNullableDateTime(Request.QueryString["eosp"]) !=null? DateTime.Parse(Request.QueryString["eosp"].ToString()).ToString("dd/MMM/yyyy") : DateTime.Now.ToString("dd/MMM/yyyy");
            if (General.GetNullableDateTime(Request.QueryString["eosp"]) != null)
                toDateInput.SelectedDate = DateTime.Parse(Request.QueryString["eosp"].ToString());
            else
                toDateInput.SelectedDate = DateTime.Now;

            if (General.GetNullableDateTime(Request.QueryString["cosp"]) != null)
                fromDateInput.SelectedDate = DateTime.Parse(Request.QueryString["cosp"].ToString());
            else
                fromDateInput.SelectedDate = DateTime.Now.AddMonths(-1);
            BindData();
        }
        
    }
    protected void BindData()
    {

        string fromdate = fromDateInput.SelectedDate.ToString();
        string todate = toDateInput.SelectedDate.ToString();
        int vesselid= int.Parse(ViewState["vesselid"].ToString());
        DataSet ds;
		DataTable dt = new DataTable();
        try
        {
			ds = PhoenixVesselPositionVPRSChartAuxillaryEngine.ChartNoonreportCPDeviationList(vesselid, General.GetNullableDateTime(fromdate), General.GetNullableDateTime(todate));
			dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                int i = 0;
                while (i < dt.Rows.Count)
                {
                    cpspeed = cpspeed + dt.Rows[i]["FLDCPSPEED"].ToString() + ",";
                    sog = sog + dt.Rows[i]["FLDACTUALSPEED"].ToString() + ",";
                    cpfoc = cpfoc + dt.Rows[i]["FLDCPFOC"].ToString() + ",";
                    actualfoc = actualfoc + dt.Rows[i]["FLDACTUALFOC"].ToString() + ",";

                    speeddeviation = speeddeviation + dt.Rows[i]["FLDSPEEDDEVIATION"].ToString() + ",";

					dateList = dateList + "'" + dt.Rows[i]["FLDDATE"].ToString() + "'" + ",";

                    focdeviation = focdeviation + dt.Rows[i]["FLDFOCDEVIATION"].ToString() + ",";

                    i = i + 1;
                }
               
               
                speeddeviation = "[" + speeddeviation.TrimEnd(',') + "],";
                cpspeed = "[" + cpspeed.TrimEnd(',') + "],";
                sog = "[" + sog.TrimEnd(',') + "]";

                focdeviation = "[" + focdeviation.TrimEnd(',') + "],";
                cpfoc = "[" + cpfoc.TrimEnd(',') + "],";
                actualfoc = "[" + actualfoc.TrimEnd(',') + "]";
                

                dateList = "[" + dateList.TrimEnd(',') + "]";

                //seriesdata = "[" + speeddeviation +"]";
                //seriesdata1 = "[" + focdeviation  + "]";

                seriesdata = "[" + speeddeviation + cpspeed + sog + "]";
                seriesdata1 = "[" + focdeviation + cpfoc + actualfoc + "]";

            }
        }

        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuChart_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("AELOAD"))
        {
            Response.Redirect("../VesselPosition/VesselPositionVPRSChartAuxillarydetails.aspx?vesselid=" + ViewState["vesselid"].ToString() + "&vesselname=" + ViewState["vesselname"].ToString() + "&cosp="+ Request.QueryString["cosp"]+ "&eosp=" + Request.QueryString["eosp"] + "");
        }
    }
     protected void fromDateInput_TextChangedEvent(object sender, EventArgs e)
    {
        BindData();
    }

    protected void toDateInput_TextChangedEvent(object sender, EventArgs e)
    {
        BindData();
    }

    protected void fromDateInput_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        BindData();
    }

    protected void toDateInput_SelectedDateChanged(object sender, Telerik.Web.UI.Calendar.SelectedDateChangedEventArgs e)
    {
        BindData();
    }
}
