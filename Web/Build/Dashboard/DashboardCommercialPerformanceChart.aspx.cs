using System;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Data;
using SouthNests.Phoenix.Dashboard;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections;

public partial class Dashboard_DashboardCommercialPerformanceChart : PhoenixBasePage
{
	public string voyageData = "[[]]";
	public string BfValue = "";
	public string vesselname = "";
	protected void Page_Load(object sender, EventArgs e)
	{
		if (!IsPostBack)
		{
			ViewState["vesselid"] = 0;
			ViewState["vesselid"] = "-";
			ViewState["SELECTEDVOYAGE"] = "";
			ViewState["SELECTEDPASSAGE"] = "";
			if (!string.IsNullOrEmpty(Request.QueryString["vesselid"]))
				ViewState["vesselid"] = Request.QueryString["vesselid"];
			if (!string.IsNullOrEmpty(Request.QueryString["vesselname"]))
				ViewState["vesselname"] = Request.QueryString["vesselname"];

			lblvesselname.Text = ViewState["vesselname"].ToString();
			fromDateInput.Text = DateTime.Now.AddMonths(-1).ToString("dd/MM/yyyy"); 
			ToDateInput.Text = DateTime.Now.ToString("dd/MM/yyyy");
			passagefilter();


		}
		voyageData = "[[" + "'" + fromDateInput.Text + "'," + "'" + ToDateInput.Text + "'," +   "'From','To']]";
		BfValue = b4.Checked == true ? "4" : "5";
	}

	protected void lstVslCondition_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (chkPassage.Checked == true)
		{
			voyageSummaryDetails();
			passageSummaryDetail();
		}
		FilterSet();
		passagefilter();
	}

	protected void fromDateInput_TextChanged(object sender, EventArgs e)
	{
		if (chkPassage.Checked == true)
		{
			BindVoyage();
			BindPassage();
			voyageSummaryDetails();
			passageSummaryDetail();
		}
		FilterSet();
		passagefilter();
	}

	protected void ToDateInput_TextChanged(object sender, EventArgs e)
	{
		if (chkPassage.Checked == true)
		{
			BindVoyage();
			BindPassage();
			voyageSummaryDetails();
			passageSummaryDetail();
		}
		FilterSet();
		passagefilter();

	}

	protected void lstWeather_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (chkPassage.Checked == true)
		{
			voyageSummaryDetails();
			passageSummaryDetail();
		}
		FilterSet();
		passagefilter();
	}

	protected void FilterSet()
	{
		string badWeather;
		badWeather = b4.Checked == true ? "4" : "5";
		ViewState["SELECTEDVOYAGE"] = lstVoyageNo.SelectedIndex;
		ViewState["SELECTEDPASSAGE"] = lstPassage.SelectedIndex;
		NameValueCollection nvc = new NameValueCollection();

		nvc.Add("vslCondition", lstVslCondition.SelectedValue);
		nvc.Add("weather", lstWeather.SelectedValue);
		nvc.Add("badWeather", badWeather);
		nvc.Add("vslStatus", lstVslStatus.SelectedValue);
		nvc.Add("vesselId", ViewState["vesselid"].ToString());
		if (chkPassage.Checked == true)
		{
			nvc.Add("FromDate", txtCospDate.Text);
			nvc.Add("ToDate", txtEospDate.Text);
			nvc.Add("voyageId", lstVoyageNo.SelectedValue);
			nvc.Add("arrivalId", lstPassage.SelectedValue);
		}
		else
		{
			nvc.Add("FromDate", fromDateInput.Text);
			nvc.Add("ToDate", ToDateInput.Text);
			nvc.Add("voyageId", "");
			nvc.Add("arrivalId", "");
		}

		FilterDashboard.CurrentCommercialPerformanceChart = nvc;
	}
	private void BindVoyage()
	{
		
		DataTable dt = new DataTable();
		dt = PhoenixDashboardCommercialPerformance.DashboardVoyageNoList(Int32.Parse(ViewState["vesselid"].ToString())
																	, General.GetNullableDateTime(fromDateInput.Text)
																	, General.GetNullableDateTime(ToDateInput.Text));
		//if(dt.Rows.Count > 0)
		//{
			lstVoyageNo.DataSource = dt;
			lstVoyageNo.DataBind();

		//}
	}

	protected void lstVoyageNo_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (chkPassage.Checked == true)
		{
			BindPassage();
			voyageSummaryDetails();
			passageSummaryDetail();
		}
			FilterSet();
			passagefilter();

	}

	//protected void fetchVoyageData()
	//{
	//	string voyagedetails = "";
	//	DataTable dt = new DataTable();
	//	FilterSet();
	//	if (lstVoyageNo.SelectedValue != null && lstVoyageNo.SelectedValue != "")
	//	{
	//		dt = PhoenixDashboardCommercialPerformance.DahboardVoyagePortDetails(Int32.Parse(ViewState["vesselid"].ToString()), General.GetNullableGuid(lstVoyageNo.SelectedValue));
	//		if (dt.Rows.Count > 0)
	//		{
	//			DataRow dr = dt.Rows[0];
	//			txtcommencedPort.Text = dr["FLDCOMMENCEDPORT"].ToString();
	//			txtCompletedPort.Text = dr["FLDCOMPLETEDPORT"].ToString();

	//			voyagedetails = "'" + dr["FLDVOYAGENO"].ToString() + "'" + ',';
	//			voyagedetails = voyagedetails + "'" + dr["FLDCOMMENCEDDATE"].ToString() + "'" + ',';
	//			voyagedetails = voyagedetails + "'" + dr["FLDCOMPLETEDDATE"].ToString() + "'" + ',';
	//			voyagedetails = voyagedetails + "'" + dr["FLDCOMMENCEDPORT"].ToString() + "'" + ',';
	//			voyagedetails = voyagedetails + "'" + dr["FLDCOMPLETEDPORT"].ToString() + "'";
	//		}
	//		voyageData = "[[" + voyagedetails + "]]";
	//	}
	//}

	protected void b4_CheckedChanged(object sender, EventArgs e)
	{
		if (chkPassage.Checked == true)
		{
			passageSummaryDetail();
		}
		FilterSet();
		passagefilter();
	}

	protected void b5_CheckedChanged(object sender, EventArgs e)
	{
		if (chkPassage.Checked == true)
		{
			passageSummaryDetail();
		}
		FilterSet();
		passagefilter();
	}

	protected void lstVslStatus_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (chkPassage.Checked == true)
		{
			voyageSummaryDetails();
			passageSummaryDetail();
		}
		FilterSet();
		passagefilter();
	}

	protected void lstPassage_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (chkPassage.Checked == true)
		{
			passageSummaryDetail();
		}
		FilterSet();
		passagefilter();
	}
	protected void BindPassage()
	{
		DataTable dt = new DataTable();
		dt = PhoenixDashboardCommercialPerformance.Passagelist(General.GetNullableGuid(lstVoyageNo.SelectedValue),
						Int32.Parse(ViewState["vesselid"].ToString()));
		//if (dt.Rows.Count > 0)
		//{
			lstPassage.DataSource = dt;
			lstPassage.DataBind();

		//}

	}
	protected void passageSummaryDetail()
	{

			DataTable dt = new DataTable();
			string Weather = b4.Checked == true ? "4" : "5";
			dt = PhoenixDashboardCommercialPerformance.PassageSummary(Int32.Parse(ViewState["vesselid"].ToString()), General.GetNullableGuid(lstPassage.SelectedValue), Int32.Parse(Weather));

            DataRow dr;
            if (dt.Rows.Count > 0)
                dr = dt.Rows[0];
            else
                dr = dt.NewRow();

			//if (dt.Rows.Count > 0)
			//{
				txtcommencedPort.Text = dr["FLDFROMPORT"].ToString();
				txtCompletedPort.Text = dr["FLDTOPORT"].ToString();
			//fromDateInput.Text = dr["FLDCOSP"].ToString();
			//ToDateInput.Text = dr["FLDEOSP"].ToString();
				txtCospDate.Text = General.GetDateTimeToString(dr["FLDCOSP"].ToString())+" "+ dr["FLDCOSPTIME"].ToString();
				txtEospDate.Text = General.GetDateTimeToString(dr["FLDEOSP"].ToString()) + " " + dr["FLDEOSPTIME"].ToString();

				voyageData = "[[" + "'"+ txtCospDate.Text + "'," + "'"+ txtEospDate.Text + "',"+ "'COSP','EOSP']]";
				FilterSet();
			//}
			

			StringBuilder str = new StringBuilder();
			str.Append("<div class='pasSumVsl'><div class='pasSumVslName'><span class='pasSumVslTitle'></span>" + dr["FLDVESSELNAME"].ToString() + "</div></div>");
			str.Append("<div style='position: relative'><div class='companyName'>" + dr["FLDCOMPANY"].ToString());
			str.Append("<span class='voyNum'>Voyage Number:<span class='voyNumVal'>" + dr["FLDVOYAGENUMBER"].ToString() + "</span></span></div></div>");

			str.Append("<table class='pasSumTable'>");

			str.Append("<tbody><tr>");
			str.Append("<td class='tooltpStyle tdHead' align='center'>From Port</td>");
			str.Append("<td class='tdValue'>" + dr["FLDFROMPORT"].ToString() + "</td>");
			str.Append("<td class='tooltpStyle tdHead' align='center'>COSP</td>");
			str.Append("<td class='tdValue'>" + txtCospDate.Text + "</td>");
			str.Append("</tr>");
			str.Append("<tr>");
			str.Append("<td class='tooltpStyle tdHead' align='center'>To Port</td>");
			str.Append("<td class='tdValue'>" + dr["FLDTOPORT"].ToString() + "</td>");
			str.Append("<td class='tooltpStyle tdHead' align='center'>EOSP</td>");
			str.Append("<td class='tdValue'>" + txtEospDate.Text + "</td>");
			str.Append("</tr>");
			str.Append("</tbody>");
			str.Append("</table>");

			//voyage summary
			str.Append("<table class='pasSumTable' style='margin-top: 40px;'>");
			str.Append("<tbody><tr>");
			str.Append("<td colspan = '4' class='tdHeadTitle' align='left'>Passage Summary</td>");
			str.Append("</tr>");
			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='tooltpStyle subHeadTitle' style='background: #ddd;' align='center'></td>");
			str.Append("<td class='tooltpStyle subHeadTitle' style='background: #F7A9B4' align='center'>Bad Weather</td>");
			str.Append("<td class='tooltpStyle subHeadTitle' style='background: #8DC78C' align='center'>Good Weather</td>");
			str.Append("<td class='tooltpStyle subHeadTitle' align='center'>Overall</td>");
			str.Append("</tr>");
			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle'>Distance Sailed (nm)</td>");
			str.Append("<td align = 'center'>" + dr["FLDBADWEATHERDIST"].ToString() + "</td>");

			str.Append("<td align='center'>" + dr["FLDGOODWEATHERDIST"].ToString() + "</td>");
			str.Append("<td align = 'center'>" + dr["FLDOVERALLDIST"].ToString() + "</td>");

			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle'>Time En Route (hr)</td>");
			str.Append("<td align = 'center'>" + dr["FLDBADENROUTE"].ToString() + "</td >");

			str.Append("<td align='center'>" + dr["FLDGOODENROUTE"].ToString() + "</td>");
			str.Append("<td align = 'center'>" + dr["FLDOVERALLENROUTE"].ToString() + "</td>");

			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle'>Avg Speed (kts)</td>");
			str.Append("<td align = 'center'>" + dr["FLDBADSPEED"].ToString() + "</td>");

			str.Append("<td align='center'>" + dr["FLDGOODSPEED"].ToString() + "</td>");
			str.Append("<td align = 'center'>" + dr["FLDOVERALLSPEED"].ToString() + "</td>");

			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle'>Predicted Time(As per CP) (hr)</td>");

			str.Append("<td align = 'center' ></td>");

			str.Append("<td align='center'>" + dr["FLDPREDICTEDTIMEGOOD"].ToString() + "</td>");
			str.Append("<td align = 'center' >" + dr["FLDPREDICTEDTIMEOVERALL"].ToString() + "</td>");

			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle'>Time(Loss)/Gain in Good WX (hr)</td>");
			str.Append("<td align = 'center'></td>");

			if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) < 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) > 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else
				str.Append("<td align = 'center'>" + dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString() + "</td>");


			str.Append("<td align = 'center' ></td>");

			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle'>Actual Good WX ME FOC in Voy (mt)</td>");
		//str.Append("<td align = 'center' >" + dr["FLDFOCBAD"].ToString() + "</td>");
			str.Append("<td align = 'center' ></td>");
			str.Append("<td align='center'>" + dr["FLDFOCGOOD"].ToString() + "</td>");
		//str.Append("<td align = 'center' >" + dr["FLDFOCOVERALL"].ToString() + "</td>");
			str.Append("<td align = 'center' ></td>");
			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle'>Predicted ME FOC in Voy (mt)</td>");
			str.Append("<td align = 'center'>" + dr["FLDPREDICTFOCBAD"].ToString() + "</td>");

			str.Append("<td align='center'>" + dr["FLDPREDICTFOCGOOD"].ToString() + "</td>");
			str.Append("<td align = 'center'>" + dr["FLDPREDICTFOCOVERALL"].ToString() + "</td>");

			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle'>Good WX(Loss) or Gain in FOC (mt)</td>");

		//if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCBAD"].ToString()) < 0)
		//{
		//	str.Append("<td align='center'>");
		//	str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCBAD"].ToString()) + "</span>");
		//	str.Append("</td>");
		//}
		//else
		//	str.Append("<td align = 'center'>" + dr["FLDLOSSORGAINFOCBAD"].ToString() + "</td>");
			str.Append("<td align = 'center' ></td>");
			if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) < 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) > 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else
				str.Append("<td align = 'center'>" + dr["FLDLOSSORGAINFOCGOOD"].ToString() + "</td>");

		//if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCOVERALL"].ToString()) < 0)
		//{
		//	str.Append("<td align='center'>");
		//	str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCOVERALL"].ToString()) + "</span>");
		//	str.Append("</td>");
		//}
		//else
		//	str.Append("<td align = 'center'>" + dr["FLDLOSSORGAINFOCOVERALL"].ToString() + "</td>");
			str.Append("<td align = 'center' ></td>");
			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle'>Avg RPM</td>");
			str.Append("<td align = 'center'>" + dr["FLDAVGRPMBAD"].ToString() + "</td>");

			str.Append("<td align='center'>" + dr["FLDAVGRPMGOOD"].ToString() + "</td>");

			str.Append("<td align = 'center'>" + dr["FLDAVGRPMOVERALL"].ToString() + "</td>");

			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle'>Avg Slip</td>");
		if (General.GetNullableDecimal(dr["FLDSLIPBAD"].ToString()) < 0)
		{
			str.Append("<td align = 'center'>");
			str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDSLIPBAD"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDSLIPBAD"].ToString()) > 0)
		{
			str.Append("<td align = 'center'>");
			str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDSLIPBAD"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else
			str.Append("<td align = 'center'>" + dr["FLDSLIPBAD"].ToString() + "</td>");

		if (General.GetNullableDecimal(dr["FLDSLIPGOOD"].ToString()) < 0)
		{
			str.Append("<td align = 'center'>");
			str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDSLIPGOOD"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDSLIPGOOD"].ToString()) > 0)
		{
			str.Append("<td align = 'center'>");
			str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDSLIPGOOD"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else
			str.Append("<td align = 'center'>" + dr["FLDSLIPGOOD"].ToString() + "</td>");

		if (General.GetNullableDecimal(dr["FLDSLIPOVERALL"].ToString()) < 0)
		{
			str.Append("<td align = 'center'>");
			str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDSLIPOVERALL"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDSLIPOVERALL"].ToString()) > 0)
		{
			str.Append("<td align = 'center'>");
			str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDSLIPOVERALL"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else
			str.Append("<td align = 'center'>" + dr["FLDSLIPOVERALL"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle'>Avg BHP</td>");
			str.Append("<td align = 'center'>" + dr["FLDAVGBHPBAD"].ToString() + "</td>");

			str.Append("<td align='center'>" + dr["FLDAVGBHPGOOD"].ToString() + "</td>");
			str.Append("<td align = 'center'>" + dr["FLDAVGBHPOVERALL"].ToString() + "</td>");

			str.Append("</tr>");

			str.Append("</tbody></table>");

		str.Append("<table class='pasSumTable' style='margin-top: 40px;'>");
		str.Append("<tbody><tr>");
		str.Append("<td colspan = '4' class='tdHeadTitle' align='left'>Superintendent Remarks</td>");
		str.Append("</tr>");
		str.Append("<tr>");
		str.Append("<td colspan = '4' align='left'>" + dr["FLDSUPDTREMARKS"].ToString() + "</td>");
		str.Append("</tr>");
		str.Append("</tbody></table>");

		//Time Analysis and Bunker Analysis 

		str.Append("<table width = '100%' style = 'padding: 0; margin-top: 30px;'>");

			str.Append("<tbody ><tr>");


			str.Append("<td width = '40%' valign = 'top'>");

			str.Append("<table class='pasSumTable' style='width: 100%; margin-right: 20px; margin-top: 0;'>");

			str.Append("<tbody><tr>");
			str.Append("<td colspan = '2' class='tdHeadTitle' align='left'>Time Analysis Based in Good Weather</td>");
			str.Append("</tr>");
			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle' align='center'>CP Speed</td>");
			str.Append("<td align = 'center'>" + dr["FLDCHARTERPARTYSPEED"].ToString() + "</td>");

			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle' align='center'>Predicted Time</td>");
			str.Append("<td align = 'center'>" + dr["FLDPREDICTEDTIMEGOOD"].ToString() + "</td>");

			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle' align='center'>Time(Loss)/Gain in Good WX (hr)</td>");

			if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) < 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) > 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else
				str.Append("<td align = 'center'>" + dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString() + "</td>");

			str.Append("</tr>");
			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle' align='center'>Overall Time(loss)/gain</td>");
			if (General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) < 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else if (General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) > 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else
				str.Append("<td align = 'center'>" + dr["FLDOVERALLTIMELOSSORGAIN2"].ToString() + "</td>");
			str.Append("</tr>");
			str.Append("</tbody></table>");

			str.Append("</td>");

			//Bunker Analysis-- >
			str.Append("<td width='60%' valign='top'>");


			str.Append("<table class='pasSumTable' style='margin-top: 0;' width='100%'>");
			str.Append("<tbody><tr>");
			str.Append("<td colspan = '3' class='tdHeadTitle' align='left'>Bunker Analysis</td>");
			str.Append("</tr>");
			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='tooltpStyle subHeadTitle' style='background: #ddd;' align='center'></td>");
			str.Append("<td class='tooltpStyle subHeadTitle' align='center'>HFO</td>");
			str.Append("<td class='tooltpStyle subHeadTitle' align='center'>MDO</td>");
			str.Append("</tr>");
			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle' align='center'>Daily CP Allowance</td>");
			str.Append("<td align = 'center'> " + dr["FLDCPHFO"].ToString() + " </td>");

			str.Append("<td align='center'>" + dr["FLDCPMDO"].ToString() + "</td>");
			str.Append("</tr>");
			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle' align='center'>Actual Good WX Consumption</td>");
			str.Append("<td align = 'center'>" + dr["FLDACTHFOCONSGOOD"].ToString() + "</td>");

			str.Append("<td align= 'center'>" + dr["FLDACTMDOCONSGOOD"].ToString() + "</td>");

			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle' align='center'>Good WX Avg.Daily Consumption</td>");
			str.Append("<td align = 'center' > " + dr["FLDAVGHFOCONSGOOD"].ToString() + " </td>");

			str.Append("<td align= 'center'>" + dr["FLDAVGMDOCONSGOOD"].ToString() + "</td>");

			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle' align='center'>Overall Avg.Daily Consumption</td>");
			str.Append("<td align = 'center'>" + dr["FLDAVGHFOCONSOVERALL"].ToString() + "</td>");

			str.Append("<td align= 'center'>" + dr["FLDAVGMDOCONSOVERALL"].ToString() + "</td>");

			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle' align='center'>Allowed Good WX Consumption</td>");
			str.Append("<td align = 'center' >" + dr["FLDALLOWEDGOODWXHFOCONS"].ToString() + "</td>");

			str.Append("<td align= 'center' >" + dr["FLDALLOWEDGOODWXMDOCONS"].ToString() + "</td>");

			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle' align='center'>Good WX(Over)/Under Consumption</td>");
			if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) < 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) > 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else
				str.Append("<td align = 'center'>" + dr["FLDGOODWXUNDEROVERHFOCONS"].ToString() + "</td>");

			if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) < 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) > 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else
				str.Append("<td align = 'center'>" + dr["FLDGOODWXUNDEROVERMDOCONS"].ToString() + "</td>");

			str.Append("</tr>");

			str.Append("<tr class='subSideTR'>");
			str.Append("<td class='subSideTitle' align='center'>Overall(Over)/Under Consumption</td>");
			if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) < 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) > 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else
				str.Append("<td align = 'center'>" + dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString() + "</td>");

			if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) < 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) > 0)
			{
				str.Append("<td align='center'>");
				str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) + "</span>");
				str.Append("</td>");
			}
			else
				str.Append("<td align = 'center'>" + dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString() + "</td>");

			str.Append("</tr>");

			str.Append("</tbody></table>");


			str.Append("</td>");

			str.Append("</tr>");

			str.Append("</tbody></table>");


			//Passage-- >


			str.Append("<table class='pasSumTable' cellpading='0' style='margin-top: 20px;' width='100%' cellspacing='0'>");
			str.Append("<tbody><tr class='subSideTR' style='background: #ddd'>");
			str.Append("<td></td>");
			str.Append("<td colspan = '4' style='background: #bbb; color: #0A1D29; font-weight: 700;' align='center'>ROB</td>");
			str.Append("<td colspan = '4' ></td>");

			str.Append("</tr>");

			str.Append("<tr class='subSideTR' style='background: #ddd'>");
			str.Append("<td></td>");
			str.Append("<td colspan = '2' style='background: #ccc; color: #0E5D9E; font-weight: 700;' align='center'>Departure(COSP)</td>");
			str.Append("<td colspan = '2' style='background: #ccc; color: #0E5D9E; font-weight: 700;' align='center'>Arrival(EOSP)</td>");
			str.Append("<td colspan = '4' style='background: #ccc; color: #0E5D9E; font-weight: 700;' align='center'>Consumed</td>");
			str.Append("</tr>");
			str.Append("<tr class='subSideTR lastTable' style='background: #0E5D9E; color: white;'>");
			str.Append("<td>From - To</td>");
			str.Append("<td align = 'center'> HFO </td>");

			str.Append("<td align='center'>MDO</td>");
			str.Append("<td align = 'center'> HFO </td>");

			str.Append("<td align='center'>MDO</td>");
			str.Append("<td align = 'center' > HFO </td >");

			str.Append("<td align='center'>MDO</td>");
			str.Append("<td align = 'center'> M/E </td>");

			str.Append("<td align='center'>A/E</td>");
			str.Append("</tr>");
			str.Append("<tr class='subSideTR lastTable'>");
			str.Append("<td class='subSideTitle' style='color: #0E5D9E; background: #ffff;'>Voy : " + dr["FLDFROMPORT"].ToString() + " - " + dr["FLDTOPORT"].ToString() + "</td>");
			str.Append("<td align = 'center'>" + dr["FLDHFOROBONDEP"].ToString() + "</td>");

			str.Append("<td align='center'>" + dr["FLDMDOROBONDEP"].ToString() + "</td>");
			str.Append("<td align = 'center' >" + dr["FLDHFOROBONARR"].ToString() + "</td>");

			str.Append("<td align='center'>" + dr["FLDMDOROBONARR"].ToString() + "</td>");
			str.Append("<td align = 'center'>" + dr["FLDHFOCONS"].ToString() + "</td>");

			str.Append("<td align='center'>" + dr["FLDMDOCONS"].ToString() + "</td>");
			str.Append("<td align = 'center'>" + dr["FLDMEOVERALLCONS"].ToString() + "</td>");

			str.Append("<td align='center'>" + dr["FLDAEOVERALLCONS"].ToString() + "</td>");
			str.Append("</tr>");
			str.Append("<tr class='subSideTR lastTable'>");
			str.Append("<td colspan = '5' style='background: #ddd; font-weight: 700; border-top: 1px solid #0E5D9E' align='left'>Total</td>");
			str.Append("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDHFOCONS"].ToString() + "</td>");
			str.Append("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDMDOCONS"].ToString() + "</td>");
			str.Append("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDMEOVERALLCONS"].ToString() + "</td>");
			str.Append("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDAEOVERALLCONS"].ToString() + "</td>");
			str.Append("</tr>");
			str.Append("</tbody></table>");

			mp01Chart06Graph.InnerHtml = "";
			mp01Chart06Graph.InnerHtml = str.ToString();
			//div = new HtmlGenericControl(str.ToString());
			////divpassage.Controls.AddAt(0,div);
			//mp01Chart06.Controls.Clear();
			//mp01Chart06.Controls.Add(div);
		}
	protected void voyageSummaryDetails()
	{
		DataTable dt = new DataTable();
		DataRow dr;
		string Weather = b4.Checked == true ? "4" : "5";
		dt = PhoenixDashboardCommercialPerformance.VoyageSummary(Int32.Parse(ViewState["vesselid"].ToString())
																	, General.GetNullableGuid(lstVoyageNo.SelectedValue)
																	, General.GetNullableDateTime(fromDateInput.Text)
																	, General.GetNullableDateTime(ToDateInput.Text));
		if (dt.Rows.Count > 0)
		{
			dr = dt.Rows[0];
		}
		else
		{
			dr = dt.NewRow();
		}

		//FilterSet();

        string cosp = General.GetDateTimeToString(dr["FLDCOSP"].ToString()) + " " + dr["FLDCOSPTIME"].ToString();
        string eosp = General.GetDateTimeToString(dr["FLDEOSP"].ToString()) + " " + dr["FLDEOSPTIME"].ToString();

        StringBuilder str = new StringBuilder();
		str.Append("<div class='pasSumVsl'><div class='pasSumVslName'><span class='pasSumVslTitle'></span>" + dr["FLDVESSELNAME"].ToString() + "</div></div>");
		str.Append("<div style='position: relative'><div class='companyName'>" + dr["FLDCOMPANY"].ToString());
		str.Append("<span class='voyNum'>Voyage Number:<span class='voyNumVal'>" + dr["FLDVOYAGENUMBER"].ToString() + "</span></span></div></div>");

		str.Append("<table class='pasSumTable'>");

		str.Append("<tbody><tr>");
		str.Append("<td class='tooltpStyle tdHead' align='center'>From Port</td>");
		str.Append("<td class='tdValue'>" + dr["FLDFROMPORT"].ToString() + "</td>");
		str.Append("<td class='tooltpStyle tdHead' align='center'>COSP</td>");
		str.Append("<td class='tdValue'>" + cosp + "</td>");
		str.Append("</tr>");
		str.Append("<tr>");
		str.Append("<td class='tooltpStyle tdHead' align='center'>To Port</td>");
		str.Append("<td class='tdValue'>" + dr["FLDTOPORT"].ToString() + "</td>");
		str.Append("<td class='tooltpStyle tdHead' align='center'>EOSP</td>");
		str.Append("<td class='tdValue'>" + eosp + "</td>");
		str.Append("</tr>");
		str.Append("</tbody>");
		str.Append("</table>");

		//voyage summary
		str.Append("<table class='pasSumTable' style='margin-top: 40px;'>");
		str.Append("<tbody><tr>");
		str.Append("<td colspan = '4' class='tdHeadTitle' align='left'>Voyage Summary</td>");
		str.Append("</tr>");
		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='tooltpStyle subHeadTitle' style='background: #ddd;' align='center'></td>");
		str.Append("<td class='tooltpStyle subHeadTitle' style='background: #F7A9B4' align='center'>Bad Weather</td>");
		str.Append("<td class='tooltpStyle subHeadTitle' style='background: #8DC78C' align='center'>Good Weather</td>");
		str.Append("<td class='tooltpStyle subHeadTitle' align='center'>Overall</td>");
		str.Append("</tr>");
		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle'>Distance Sailed (nm)</td>");
		str.Append("<td align = 'center'>" + dr["FLDBADWEATHERDIST"].ToString() + "</td>");

		str.Append("<td align='center'>" + dr["FLDGOODWEATHERDIST"].ToString() + "</td>");
		str.Append("<td align = 'center'>" + dr["FLDOVERALLDIST"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle'>Time En Route (hr)</td>");
		str.Append("<td align = 'center'>" + dr["FLDBADENROUTE"].ToString() + "</td >");

		str.Append("<td align='center'>" + dr["FLDGOODENROUTE"].ToString() + "</td>");
		str.Append("<td align = 'center'>" + dr["FLDOVERALLENROUTE"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle'>Avg Speed (kts)</td>");
		str.Append("<td align = 'center'>" + dr["FLDBADSPEED"].ToString() + "</td>");

		str.Append("<td align='center'>" + dr["FLDGOODSPEED"].ToString() + "</td>");
		str.Append("<td align = 'center'>" + dr["FLDOVERALLSPEED"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle'>Predicted Time(As per CP) (hr)</td>");

		str.Append("<td align = 'center' ></td>");

		str.Append("<td align='center'>" + dr["FLDPREDICTEDTIMEGOOD"].ToString() + "</td>");
		str.Append("<td align = 'center' >" + dr["FLDPREDICTEDTIMEOVERALL"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle'>Time(Loss)/Gain in Good WX (hr)</td>");
		str.Append("<td align = 'center'></td>");

		if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) < 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) > 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else
			str.Append("<td align = 'center'>" + dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString() + "</td>");


		str.Append("<td align = 'center' ></td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle'>Actual Good WX ME FOC in Voy (mt)</td>");
		//str.Append("<td align = 'center' >" + dr["FLDFOCBAD"].ToString() + "</td>");
		str.Append("<td align = 'center' ></td>");
		str.Append("<td align='center'>" + dr["FLDFOCGOOD"].ToString() + "</td>");
		//str.Append("<td align = 'center' >" + dr["FLDFOCOVERALL"].ToString() + "</td>");
		str.Append("<td align = 'center' ></td>");
		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle'>Predicted ME FOC in Voy (mt)</td>");
		str.Append("<td align = 'center'>" + dr["FLDPREDICTFOCBAD"].ToString() + "</td>");

		str.Append("<td align='center'>" + dr["FLDPREDICTFOCGOOD"].ToString() + "</td>");
		str.Append("<td align = 'center'>" + dr["FLDPREDICTFOCOVERALL"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle'>Good WX(Loss) or Gain in FOC (mt)</td>");

		//if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCBAD"].ToString()) < 0)
		//{
		//	str.Append("<td align='center'>");
		//	str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCBAD"].ToString()) + "</span>");
		//	str.Append("</td>");
		//}
		//else
		//	str.Append("<td align = 'center'>" + dr["FLDLOSSORGAINFOCBAD"].ToString() + "</td>");
		str.Append("<td align = 'center' ></td>");
		if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) < 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) > 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else
			str.Append("<td align = 'center'>" + dr["FLDLOSSORGAINFOCGOOD"].ToString() + "</td>");

		//if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCOVERALL"].ToString()) < 0)
		//{
		//	str.Append("<td align='center'>");
		//	str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCOVERALL"].ToString()) + "</span>");
		//	str.Append("</td>");
		//}
		//else
		//	str.Append("<td align = 'center'>" + dr["FLDLOSSORGAINFOCOVERALL"].ToString() + "</td>");
		str.Append("<td align = 'center' ></td>");
		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle'>Avg RPM</td>");
		str.Append("<td align = 'center'>" + dr["FLDAVGRPMBAD"].ToString() + "</td>");

		str.Append("<td align='center'>" + dr["FLDAVGRPMGOOD"].ToString() + "</td>");

		str.Append("<td align = 'center'>" + dr["FLDAVGRPMOVERALL"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle'>Avg Slip</td>");
		if (General.GetNullableDecimal(dr["FLDSLIPBAD"].ToString()) < 0)
		{
			str.Append("<td align = 'center'>");
			str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDSLIPBAD"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDSLIPBAD"].ToString()) > 0)
		{
			str.Append("<td align = 'center'>");
			str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDSLIPBAD"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else
			str.Append("<td align = 'center'>" + dr["FLDSLIPBAD"].ToString() + "</td>");

		if (General.GetNullableDecimal(dr["FLDSLIPGOOD"].ToString()) < 0)
		{
			str.Append("<td align = 'center'>");
			str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDSLIPGOOD"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDSLIPGOOD"].ToString()) > 0)
		{
			str.Append("<td align = 'center'>");
			str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDSLIPGOOD"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else
			str.Append("<td align = 'center'>" + dr["FLDSLIPGOOD"].ToString() + "</td>");

		if (General.GetNullableDecimal(dr["FLDSLIPOVERALL"].ToString()) < 0)
		{
			str.Append("<td align = 'center'>");
			str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDSLIPOVERALL"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDSLIPOVERALL"].ToString()) > 0)
		{
			str.Append("<td align = 'center'>");
			str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDSLIPOVERALL"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else
			str.Append("<td align = 'center'>" + dr["FLDSLIPOVERALL"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle'>Avg BHP</td>");
		str.Append("<td align = 'center'>" + dr["FLDAVGBHPBAD"].ToString() + "</td>");

		str.Append("<td align='center'>" + dr["FLDAVGBHPGOOD"].ToString() + "</td>");
		str.Append("<td align = 'center'>" + dr["FLDAVGBHPOVERALL"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("</tbody></table>");

		//str.Append("<table class='pasSumTable' style='margin-top: 40px;'>");
		//str.Append("<tbody><tr>");
		//str.Append("<td colspan = '4' class='tdHeadTitle' align='left'>Superintendent Remarks</td>");
		//str.Append("</tr>");
		//str.Append("<tr>");
		//str.Append("<td colspan = '4' align='left'>" + dr["FLDSUPDTREMARKS"].ToString() + "</td>");
		//str.Append("</tr>");
		//str.Append("</tbody></table>");

		//Time Analysis and Bunker Analysis 

		str.Append("<table width = '100%' style = 'padding: 0; margin-top: 30px;'>");

		str.Append("<tbody ><tr>");


		str.Append("<td width = '40%' valign = 'top'>");

		str.Append("<table class='pasSumTable' style='width: 400px; margin-right: 20px; margin-top: 0;'>");

		str.Append("<tbody><tr>");
		str.Append("<td colspan = '2' class='tdHeadTitle' align='left'>Time Analysis Based in Good Weather</td>");
		str.Append("</tr>");
		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle' align='center'>CP Speed</td>");
		str.Append("<td align = 'center'>" + dr["FLDCHARTERPARTYSPEED"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle' align='center'>Predicted Time</td>");
		str.Append("<td align = 'center'>" + dr["FLDPREDICTEDTIMEGOOD"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle' align='center'>Time(Loss)/Gain in Good WX (hr)</td>");

		if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) < 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) > 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else
			str.Append("<td align = 'center'>" + dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString() + "</td>");

		str.Append("</tr>");
		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle' align='center'>Overall Time(loss)/gain</td>");
		if (General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) < 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) > 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else
			str.Append("<td align = 'center'>" + dr["FLDOVERALLTIMELOSSORGAIN2"].ToString() + "</td>");
		str.Append("</tr>");
		str.Append("</tbody></table>");

		str.Append("</td>");

		//Bunker Analysis-- >
		str.Append("<td width='60%' valign='top'>");


		str.Append("<table class='pasSumTable' style='margin-top: 0;' width='100%'>");
		str.Append("<tbody><tr>");
		str.Append("<td colspan = '3' class='tdHeadTitle' align='left'>Bunker Analysis</td>");
		str.Append("</tr>");
		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='tooltpStyle subHeadTitle' style='background: #ddd;' align='center'></td>");
		str.Append("<td class='tooltpStyle subHeadTitle' align='center'>HFO</td>");
		str.Append("<td class='tooltpStyle subHeadTitle' align='center'>MDO</td>");
		str.Append("</tr>");
		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle' align='center'>Daily CP Allowance</td>");
		str.Append("<td align = 'center'> " + dr["FLDCPHFO"].ToString() + " </td>");

		str.Append("<td align='center'>" + dr["FLDCPMDO"].ToString() + "</td>");
		str.Append("</tr>");
		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle' align='center'>Actual Good WX Consumption</td>");
		str.Append("<td align = 'center'>" + dr["FLDACTHFOCONSGOOD"].ToString() + "</td>");

		str.Append("<td align= 'center'>" + dr["FLDACTMDOCONSGOOD"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle' align='center'>Good WX Avg.Daily Consumption</td>");
		str.Append("<td align = 'center' > " + dr["FLDAVGHFOCONSGOOD"].ToString() + " </td>");

		str.Append("<td align= 'center'>" + dr["FLDAVGMDOCONSGOOD"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle' align='center'>Overall Avg.Daily Consumption</td>");
		str.Append("<td align = 'center'>" + dr["FLDAVGHFOCONSOVERALL"].ToString() + "</td>");

		str.Append("<td align= 'center'>" + dr["FLDAVGMDOCONSOVERALL"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle' align='center'>Allowed Good WX Consumption</td>");
		str.Append("<td align = 'center' >" + dr["FLDALLOWEDGOODWXHFOCONS"].ToString() + "</td>");

		str.Append("<td align= 'center' >" + dr["FLDALLOWEDGOODWXMDOCONS"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle' align='center'>Good WX(Over)/Under Consumption</td>");
		if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) < 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) > 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else
			str.Append("<td align = 'center'>" + dr["FLDGOODWXUNDEROVERHFOCONS"].ToString() + "</td>");

		if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) < 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) > 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else
			str.Append("<td align = 'center'>" + dr["FLDGOODWXUNDEROVERMDOCONS"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR'>");
		str.Append("<td class='subSideTitle' align='center'>Overall(Over)/Under Consumption</td>");
		if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) < 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) > 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else
			str.Append("<td align = 'center'>" + dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString() + "</td>");

		if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) < 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) > 0)
		{
			str.Append("<td align='center'>");
			str.Append("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) + "</span>");
			str.Append("</td>");
		}
		else
			str.Append("<td align = 'center'>" + dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString() + "</td>");

		str.Append("</tr>");

		str.Append("</tbody></table>");


		str.Append("</td>");

		str.Append("</tr>");

		str.Append("</tbody></table>");


		//Passage-- >


		str.Append("<table class='pasSumTable' cellpading='0' style='margin-top: 20px;' width='100%' cellspacing='0'>");
		str.Append("<tbody><tr class='subSideTR' style='background: #ddd'>");
		str.Append("<td></td>");
		str.Append("<td colspan = '4' style='background: #bbb; color: #0A1D29; font-weight: 700;' align='center'>ROB</td>");
		str.Append("<td colspan = '4' ></td>");

		str.Append("</tr>");

		str.Append("<tr class='subSideTR' style='background: #ddd'>");
		str.Append("<td></td>");
		str.Append("<td colspan = '2' style='background: #ccc; color: #0E5D9E; font-weight: 700;' align='center'>Departure(COSP)</td>");
		str.Append("<td colspan = '2' style='background: #ccc; color: #0E5D9E; font-weight: 700;' align='center'>Arrival(EOSP)</td>");
		str.Append("<td colspan = '4' style='background: #ccc; color: #0E5D9E; font-weight: 700;' align='center'>Consumed</td>");
		str.Append("</tr>");
		str.Append("<tr class='subSideTR lastTable' style='background: #0E5D9E; color: white;'>");
		str.Append("<td>From - To</td>");
		str.Append("<td align = 'center'> HFO </td>");

		str.Append("<td align='center'>MDO</td>");
		str.Append("<td align = 'center'> HFO </td>");

		str.Append("<td align='center'>MDO</td>");
		str.Append("<td align = 'center' > HFO </td >");

		str.Append("<td align='center'>MDO</td>");
		str.Append("<td align = 'center'> M/E </td>");

		str.Append("<td align='center'>A/E</td>");
		str.Append("</tr>");
		str.Append("<tr class='subSideTR lastTable'>");
		str.Append("<td class='subSideTitle' style='color: #0E5D9E; background: #ffff;'>Voy : " + dr["FLDFROMPORT"].ToString() + " - " + dr["FLDTOPORT"].ToString() + "</td>");
		str.Append("<td align = 'center'>" + dr["FLDHFOROBONDEP"].ToString() + "</td>");

		str.Append("<td align='center'>" + dr["FLDMDOROBONDEP"].ToString() + "</td>");
		str.Append("<td align = 'center' >" + dr["FLDHFOROBONARR"].ToString() + "</td>");

		str.Append("<td align='center'>" + dr["FLDMDOROBONARR"].ToString() + "</td>");
		str.Append("<td align = 'center'>" + dr["FLDHFOCONS"].ToString() + "</td>");

		str.Append("<td align='center'>" + dr["FLDMDOCONS"].ToString() + "</td>");
		str.Append("<td align = 'center'>" + dr["FLDMEOVERALLCONS"].ToString() + "</td>");

		str.Append("<td align='center'>" + dr["FLDAEOVERALLCONS"].ToString() + "</td>");
		str.Append("</tr>");
		str.Append("<tr class='subSideTR lastTable'>");
		str.Append("<td colspan = '5' style='background: #ddd; font-weight: 700; border-top: 1px solid #0E5D9E' align='left'>Total</td>");
		str.Append("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDHFOCONS"].ToString() + "</td>");
		str.Append("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDMDOCONS"].ToString() + "</td>");
		str.Append("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDMEOVERALLCONS"].ToString() + "</td>");
		str.Append("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDAEOVERALLCONS"].ToString() + "</td>");
		str.Append("</tr>");
		str.Append("</tbody></table>");

		mp01Chart07Graph.InnerHtml = "";
		mp01Chart07Graph.InnerHtml = str.ToString();
	}

	protected void chkPassage_CheckedChanged(object sender, EventArgs e)
	{
		if (chkPassage.Checked == true)
		{
			BindVoyage();
			BindPassage();
		}
			passagefilter();
	}
	protected void passagefilter()
	{
		if (chkPassage.Checked == true)
		{
			passageSummaryDetail();
			voyageSummaryDetails();
			ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showFn(true);", true);
		}
		else
		{
			filterClear();
			ScriptManager.RegisterStartupScript(this, this.GetType(), "", "showFn(false);", true);
		}
		tabscreation();
		FilterSet();
	}
	protected void filterClear()
	{
		lstVoyageNo.Items.Insert(0, new ListItem("--Select--", ""));
		lstVoyageNo.SelectedIndex = 0;
		lstPassage.Items.Insert(0, new ListItem("--Select--", ""));
		lstPassage.SelectedIndex = 0;
		txtcommencedPort.Text = "";
		txtCompletedPort.Text = "";
		txtCospDate.Text = "";
		txtEospDate.Text = "";
	}
	protected void tabscreation()
	{
		StringBuilder str = new StringBuilder();
		if (chkPassage.Checked == true)
		{
			str.Append("<input type = 'button' data-tab = 'mp01Chart01' class='tabs01Btn tabsBtnActive' value='Charter Party' />");
			str.Append("<input type = 'button' data-tab	= 'mp01Chart02' class='tabs01Btn' value='Fuel Consumption' />");
			str.Append("<input type = 'button' data-tab	= 'mp01Chart03' class='tabs01Btn' value='Vessel Fuel Efficiency' />");
			str.Append("<input type = 'button' data-tab	= 'mp01Chart04' class='tabs01Btn' value='Auxiliary Boiler Fuel' />");
			str.Append("<input type = 'button' data-tab	= 'mp01Chart05' class='tabs01Btn' value='Passage Overview' />");
			str.Append("<input type = 'button' data-tab	= 'mp01Chart06' class='tabs01Btn' value='Passage Summary' />");
			str.Append("<input type = 'button' data-tab	= 'mp01Chart07' class='tabs01Btn' value='Voyage Summary' />");
		}
		else
		{
			str.Append("<input type = 'button' data-tab = 'mp01Chart01' class='tabs01Btn tabsBtnActive' value='Charter Party' />");
			str.Append("<input type = 'button' data-tab	= 'mp01Chart02' class='tabs01Btn' value='Fuel Consumption' />");
			str.Append("<input type = 'button' data-tab	= 'mp01Chart03' class='tabs01Btn' value='Vessel Fuel Efficiency' />");
			str.Append("<input type = 'button' data-tab	= 'mp01Chart04' class='tabs01Btn' value='Auxiliary Boiler Fuel' />");
			str.Append("<input type = 'button' data-tab	= 'mp01Chart05' class='tabs01Btn' value='Passage Overview' />");

		}
		tabs01Nav.InnerHtml = "";
		tabs01Nav.InnerHtml = str.ToString();
	}
	protected void lnkPassageExcel(object sender, EventArgs e)
	{
		DataTable dt = new DataTable();
		string Weather = b4.Checked == true ? "4" : "5";
		dt = PhoenixDashboardCommercialPerformance.PassageSummary(Int32.Parse(ViewState["vesselid"].ToString()), General.GetNullableGuid(lstPassage.SelectedValue), Int32.Parse(Weather));
		DataRow dr = dt.Rows[0];

        string cosp = General.GetDateTimeToString(dr["FLDCOSP"].ToString()) + " " + dr["FLDCOSPTIME"].ToString();
        string eosp = General.GetDateTimeToString(dr["FLDEOSP"].ToString()) + " " + dr["FLDEOSPTIME"].ToString();

        Response.AddHeader("Content-Disposition", "attachment; filename=VoyageSummary.xls");
		Response.ContentType = "application/vnd.msexcel";

		Response.Write("<div class='pasSumVsl'><div class='pasSumVslName'><span class='pasSumVslTitle'></span>" + dr["FLDVESSELNAME"].ToString() + "</div></div>");
		Response.Write("<div style='position: relative'><div class='companyName'>" + dr["FLDCOMPANY"].ToString());
		Response.Write("<span class='voyNum'>Voyage Number:<span class='voyNumVal'>" + dr["FLDVOYAGENUMBER"].ToString() + "</span></span></div></div>");

		Response.Write("<table class='pasSumTable'>");

		Response.Write("<tbody><tr>");
		Response.Write("<td class='tooltpStyle tdHead' align='center'>From Port</td>");
		Response.Write("<td class='tdValue'>" + dr["FLDFROMPORT"].ToString() + "</td>");
		Response.Write("<td class='tooltpStyle tdHead' align='center'>COSP</td>");
		Response.Write("<td class='tdValue'>" + cosp + "</td>");
		Response.Write("</tr>");
		Response.Write("<tr>");
		Response.Write("<td class='tooltpStyle tdHead' align='center'>To Port</td>");
		Response.Write("<td class='tdValue'>" + dr["FLDTOPORT"].ToString() + "</td>");
		Response.Write("<td class='tooltpStyle tdHead' align='center'>EOSP</td>");
		Response.Write("<td class='tdValue'>" + eosp + "</td>");
		Response.Write("</tr>");
		Response.Write("</tbody>");
		Response.Write("</table>");

		//voyage summary
		Response.Write("<table class='pasSumTable' style='margin-top: 40px;'>");
		Response.Write("<tbody><tr>");
		Response.Write("<td colspan = '4' class='tdHeadTitle' align='left'>Passage Summary</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='tooltpStyle subHeadTitle' style='background: #ddd;' align='center'></td>");
		Response.Write("<td class='tooltpStyle subHeadTitle' style='background: #F7A9B4' align='center'>Bad Weather</td>");
		Response.Write("<td class='tooltpStyle subHeadTitle' style='background: #8DC78C' align='center'>Good Weather</td>");
		Response.Write("<td class='tooltpStyle subHeadTitle' align='center'>Overall</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Distance Sailed (nm)</td>");
		Response.Write("<td align = 'center'>" + dr["FLDBADWEATHERDIST"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDGOODWEATHERDIST"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDOVERALLDIST"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Time En Route (hr)</td>");
		Response.Write("<td align = 'center'>" + dr["FLDBADENROUTE"].ToString() + "</td >");

		Response.Write("<td align='center'>" + dr["FLDGOODENROUTE"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDOVERALLENROUTE"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Avg Speed (kts)</td>");
		Response.Write("<td align = 'center'>" + dr["FLDBADSPEED"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDGOODSPEED"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDOVERALLSPEED"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Predicted Time(As per CP) (hr)</td>");

		Response.Write("<td align = 'center' ></td>");

		Response.Write("<td align='center'>" + dr["FLDPREDICTEDTIMEGOOD"].ToString() + "</td>");
		Response.Write("<td align = 'center' >" + dr["FLDPREDICTEDTIMEOVERALL"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Time(Loss)/Gain in Good WX (hr)</td>");
		Response.Write("<td align = 'center'></td>");

		if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString() + "</td>");


		Response.Write("<td align = 'center' ></td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Actual Good WX ME FOC in Voy (mt)</td>");
		//Response.Write("<td align = 'center' >" + dr["FLDFOCBAD"].ToString() + "</td>");
		Response.Write("<td align = 'center' ></td>");
		Response.Write("<td align='center'>" + dr["FLDFOCGOOD"].ToString() + "</td>");
		//Response.Write("<td align = 'center' >" + dr["FLDFOCOVERALL"].ToString() + "</td>");
		Response.Write("<td align = 'center' ></td>");
		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Predicted ME FOC in Voy (mt)</td>");
		Response.Write("<td align = 'center'>" + dr["FLDPREDICTFOCBAD"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDPREDICTFOCGOOD"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDPREDICTFOCOVERALL"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Good WX(Loss) or Gain in FOC (mt)</td>");

		//if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCBAD"].ToString()) < 0)
		//{
		//	Response.Write("<td align='center'>");
		//	Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCBAD"].ToString()) + "</span>");
		//	Response.Write("</td>");
		//}
		//else
		//	Response.Write("<td align = 'center'>" + dr["FLDLOSSORGAINFOCBAD"].ToString() + "</td>");
		Response.Write("<td align = 'center' ></td>");
		if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDLOSSORGAINFOCGOOD"].ToString() + "</td>");

		//if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCOVERALL"].ToString()) < 0)
		//{
		//	Response.Write("<td align='center'>");
		//	Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCOVERALL"].ToString()) + "</span>");
		//	Response.Write("</td>");
		//}
		//else
		//	Response.Write("<td align = 'center'>" + dr["FLDLOSSORGAINFOCOVERALL"].ToString() + "</td>");
		Response.Write("<td align = 'center' ></td>");
		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Avg RPM</td>");
		Response.Write("<td align = 'center'>" + dr["FLDAVGRPMBAD"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDAVGRPMGOOD"].ToString() + "</td>");

		Response.Write("<td align = 'center'>" + dr["FLDAVGRPMOVERALL"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Avg Slip</td>");

		Response.Write("<td align = 'center'>" + dr["FLDSLIPBAD"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDSLIPGOOD"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDSLIPOVERALL"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Avg BHP</td>");
		Response.Write("<td align = 'center'>" + dr["FLDAVGBHPBAD"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDAVGBHPGOOD"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDAVGBHPOVERALL"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("</tbody></table>");

		Response.Write("<table class='pasSumTable' style='margin-top: 40px;'>");
		Response.Write("<tbody><tr>");
		Response.Write("<td colspan = '4' class='tdHeadTitle' align='left'>Superintendent Remarks</td>");
		Response.Write("</tr>");
		Response.Write("<tr>");
		Response.Write("<td colspan = '4' align='left'>" + dr["FLDSUPDTREMARKS"].ToString() + "</td>");
		Response.Write("</tr>");
		Response.Write("</tbody></table>");

		//Time Analysis and Bunker Analysis 

		Response.Write("<table width = '100%' style = 'padding: 0; margin-top: 30px;'>");

		Response.Write("<tbody ><tr>");


		Response.Write("<td width = '40%' valign = 'top'>");

		Response.Write("<table class='pasSumTable' style='width: 100%; margin-right: 20px; margin-top: 0;'>");

		Response.Write("<tbody><tr>");
		Response.Write("<td colspan = '2' class='tdHeadTitle' align='left'>Time Analysis Based in Good Weather</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>CP Speed</td>");
		Response.Write("<td align = 'center'>" + dr["FLDCHARTERPARTYSPEED"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Predicted Time</td>");
		Response.Write("<td align = 'center'>" + dr["FLDPREDICTEDTIMEGOOD"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Time(Loss)/Gain in Good WX (hr)</td>");

		if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString() + "</td>");

		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Overall Time(loss)/gain</td>");
		if (General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDOVERALLTIMELOSSORGAIN2"].ToString() + "</td>");
		Response.Write("</tr>");
		Response.Write("</tbody></table>");

		Response.Write("</td>");

		//Bunker Analysis-- >
		Response.Write("<td width='60%' valign='top'>");


		Response.Write("<table class='pasSumTable' style='margin-top: 0;' width='100%'>");
		Response.Write("<tbody><tr>");
		Response.Write("<td colspan = '3' class='tdHeadTitle' align='left'>Bunker Analysis</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='tooltpStyle subHeadTitle' style='background: #ddd;' align='center'></td>");
		Response.Write("<td class='tooltpStyle subHeadTitle' align='center'>HFO</td>");
		Response.Write("<td class='tooltpStyle subHeadTitle' align='center'>MDO</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Daily CP Allowance</td>");
		Response.Write("<td align = 'center'> " + dr["FLDCPHFO"].ToString() + " </td>");

		Response.Write("<td align='center'>" + dr["FLDCPMDO"].ToString() + "</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Actual Good WX Consumption</td>");
		Response.Write("<td align = 'center'>" + dr["FLDACTHFOCONSGOOD"].ToString() + "</td>");

		Response.Write("<td align= 'center'>" + dr["FLDACTMDOCONSGOOD"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Good WX Avg.Daily Consumption</td>");
		Response.Write("<td align = 'center' > " + dr["FLDAVGHFOCONSGOOD"].ToString() + " </td>");

		Response.Write("<td align= 'center'>" + dr["FLDAVGMDOCONSGOOD"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Overall Avg.Daily Consumption</td>");
		Response.Write("<td align = 'center'>" + dr["FLDAVGHFOCONSOVERALL"].ToString() + "</td>");

		Response.Write("<td align= 'center'>" + dr["FLDAVGMDOCONSOVERALL"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Allowed Good WX Consumption</td>");
		Response.Write("<td align = 'center' >" + dr["FLDALLOWEDGOODWXHFOCONS"].ToString() + "</td>");

		Response.Write("<td align= 'center' >" + dr["FLDALLOWEDGOODWXMDOCONS"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Good WX(Over)/Under Consumption</td>");
		if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDGOODWXUNDEROVERHFOCONS"].ToString() + "</td>");

		if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDGOODWXUNDEROVERMDOCONS"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Overall(Over)/Under Consumption</td>");
		if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString() + "</td>");

		if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("</tbody></table>");


		Response.Write("</td>");

		Response.Write("</tr>");

		Response.Write("</tbody></table>");


		//Passage-- >


		Response.Write("<table class='pasSumTable' cellpading='0' style='margin-top: 20px;' width='100%' cellspacing='0'>");
		Response.Write("<tbody><tr class='subSideTR' style='background: #ddd'>");
		Response.Write("<td></td>");
		Response.Write("<td colspan = '4' style='background: #bbb; color: #0A1D29; font-weight: 700;' align='center'>ROB</td>");
		Response.Write("<td colspan = '4' ></td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR' style='background: #ddd'>");
		Response.Write("<td></td>");
		Response.Write("<td colspan = '2' style='background: #ccc; color: #0E5D9E; font-weight: 700;' align='center'>Departure(COSP)</td>");
		Response.Write("<td colspan = '2' style='background: #ccc; color: #0E5D9E; font-weight: 700;' align='center'>Arrival(EOSP)</td>");
		Response.Write("<td colspan = '4' style='background: #ccc; color: #0E5D9E; font-weight: 700;' align='center'>Consumed</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR lastTable' style='background: #0E5D9E; color: white;'>");
		Response.Write("<td>From - To</td>");
		Response.Write("<td align = 'center'> HFO </td>");

		Response.Write("<td align='center'>MDO</td>");
		Response.Write("<td align = 'center'> HFO </td>");

		Response.Write("<td align='center'>MDO</td>");
		Response.Write("<td align = 'center' > HFO </td >");

		Response.Write("<td align='center'>MDO</td>");
		Response.Write("<td align = 'center'> M/E </td>");

		Response.Write("<td align='center'>A/E</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR lastTable'>");
		Response.Write("<td class='subSideTitle' style='color: #0E5D9E; background: #ffff;'>Voy : " + dr["FLDFROMPORT"].ToString() + " - " + dr["FLDTOPORT"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDHFOROBONDEP"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDMDOROBONDEP"].ToString() + "</td>");
		Response.Write("<td align = 'center' >" + dr["FLDHFOROBONARR"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDMDOROBONARR"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDHFOCONS"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDMDOCONS"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDMEOVERALLCONS"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDAEOVERALLCONS"].ToString() + "</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR lastTable'>");
		Response.Write("<td colspan = '5' style='background: #ddd; font-weight: 700; border-top: 1px solid #0E5D9E' align='left'>Total</td>");
		Response.Write("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDHFOCONS"].ToString() + "</td>");
		Response.Write("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDMDOCONS"].ToString() + "</td>");
		Response.Write("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDMEOVERALLCONS"].ToString() + "</td>");
		Response.Write("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDAEOVERALLCONS"].ToString() + "</td>");
		Response.Write("</tr>");
		Response.Write("</tbody></table>");
		Response.End();
	}
	protected void lnkVoyageExcel(object sender, EventArgs e)
	{
		DataTable dt = new DataTable();
		DataRow dr;
		string Weather = b4.Checked == true ? "4" : "5";
		dt = PhoenixDashboardCommercialPerformance.VoyageSummary(Int32.Parse(ViewState["vesselid"].ToString())
																	, General.GetNullableGuid(lstVoyageNo.SelectedValue)
																	, General.GetNullableDateTime(fromDateInput.Text)
																	, General.GetNullableDateTime(ToDateInput.Text));
		if (dt.Rows.Count > 0)
		{
			dr = dt.Rows[0];
		}
		else
		{
			dr = dt.NewRow();
		}

        string cosp = General.GetDateTimeToString(dr["FLDCOSP"].ToString()) + " " + dr["FLDCOSPTIME"].ToString();
        string eosp = General.GetDateTimeToString(dr["FLDEOSP"].ToString()) + " " + dr["FLDEOSPTIME"].ToString();

        Response.AddHeader("Content-Disposition", "attachment; filename=VoyageSummary.xls");
		Response.ContentType = "application/vnd.msexcel";

		Response.Write("<div class='pasSumVsl'><div class='pasSumVslName'><span class='pasSumVslTitle'></span>" + dr["FLDVESSELNAME"].ToString() + "</div></div>");
		Response.Write("<div style='position: relative'><div class='companyName'>" + dr["FLDCOMPANY"].ToString());
		Response.Write("<span class='voyNum'>Voyage Number:<span class='voyNumVal'>" + dr["FLDVOYAGENUMBER"].ToString() + "</span></span></div></div>");

		Response.Write("<table class='pasSumTable'>");

		Response.Write("<tbody><tr>");
		Response.Write("<td class='tooltpStyle tdHead' align='center'>From Port</td>");
		Response.Write("<td class='tdValue'>" + dr["FLDFROMPORT"].ToString() + "</td>");
		Response.Write("<td class='tooltpStyle tdHead' align='center'>COSP</td>");
		Response.Write("<td class='tdValue'>" + cosp + "</td>");
		Response.Write("</tr>");
		Response.Write("<tr>");
		Response.Write("<td class='tooltpStyle tdHead' align='center'>To Port</td>");
		Response.Write("<td class='tdValue'>" + dr["FLDTOPORT"].ToString() + "</td>");
		Response.Write("<td class='tooltpStyle tdHead' align='center'>EOSP</td>");
		Response.Write("<td class='tdValue'>" + eosp + "</td>");
		Response.Write("</tr>");
		Response.Write("</tbody>");
		Response.Write("</table>");

		//voyage summary
		Response.Write("<table class='pasSumTable' style='margin-top: 40px;'>");
		Response.Write("<tbody><tr>");
		Response.Write("<td colspan = '4' class='tdHeadTitle' align='left'>Voyage Summary</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='tooltpStyle subHeadTitle' style='background: #ddd;' align='center'></td>");
		Response.Write("<td class='tooltpStyle subHeadTitle' style='background: #F7A9B4' align='center'>Bad Weather</td>");
		Response.Write("<td class='tooltpStyle subHeadTitle' style='background: #8DC78C' align='center'>Good Weather</td>");
		Response.Write("<td class='tooltpStyle subHeadTitle' align='center'>Overall</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Distance Sailed (nm)</td>");
		Response.Write("<td align = 'center'>" + dr["FLDBADWEATHERDIST"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDGOODWEATHERDIST"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDOVERALLDIST"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Time En Route (hr)</td>");
		Response.Write("<td align = 'center'>" + dr["FLDBADENROUTE"].ToString() + "</td >");

		Response.Write("<td align='center'>" + dr["FLDGOODENROUTE"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDOVERALLENROUTE"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Avg Speed (kts)</td>");
		Response.Write("<td align = 'center'>" + dr["FLDBADSPEED"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDGOODSPEED"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDOVERALLSPEED"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Predicted Time(As per CP) (hr)</td>");

		Response.Write("<td align = 'center' ></td>");

		Response.Write("<td align='center'>" + dr["FLDPREDICTEDTIMEGOOD"].ToString() + "</td>");
		Response.Write("<td align = 'center' >" + dr["FLDPREDICTEDTIMEOVERALL"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Time(Loss)/Gain in Good WX (hr)</td>");
		Response.Write("<td align = 'center'></td>");

		if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString() + "</td>");


		Response.Write("<td align = 'center' ></td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Actual Good WX ME FOC in Voy (mt)</td>");
		//Response.Write("<td align = 'center' >" + dr["FLDFOCBAD"].ToString() + "</td>");
		Response.Write("<td align = 'center' ></td>");
		Response.Write("<td align='center'>" + dr["FLDFOCGOOD"].ToString() + "</td>");
		//Response.Write("<td align = 'center' >" + dr["FLDFOCOVERALL"].ToString() + "</td>");
		Response.Write("<td align = 'center' ></td>");
		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Predicted ME FOC in Voy (mt)</td>");
		Response.Write("<td align = 'center'>" + dr["FLDPREDICTFOCBAD"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDPREDICTFOCGOOD"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDPREDICTFOCOVERALL"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Good WX(Loss) or Gain in FOC (mt)</td>");

		//if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCBAD"].ToString()) < 0)
		//{
		//	Response.Write("<td align='center'>");
		//	Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCBAD"].ToString()) + "</span>");
		//	Response.Write("</td>");
		//}
		//else
		//	Response.Write("<td align = 'center'>" + dr["FLDLOSSORGAINFOCBAD"].ToString() + "</td>");
		Response.Write("<td align = 'center' ></td>");
		if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDLOSSORGAINFOCGOOD"].ToString() + "</td>");

		//if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCOVERALL"].ToString()) < 0)
		//{
		//	Response.Write("<td align='center'>");
		//	Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCOVERALL"].ToString()) + "</span>");
		//	Response.Write("</td>");
		//}
		//else
		//	Response.Write("<td align = 'center'>" + dr["FLDLOSSORGAINFOCOVERALL"].ToString() + "</td>");
		Response.Write("<td align = 'center' ></td>");
		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Avg RPM</td>");
		Response.Write("<td align = 'center'>" + dr["FLDAVGRPMBAD"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDAVGRPMGOOD"].ToString() + "</td>");

		Response.Write("<td align = 'center'>" + dr["FLDAVGRPMOVERALL"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Avg Slip</td>");

		Response.Write("<td align = 'center'>" + dr["FLDSLIPBAD"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDSLIPGOOD"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDSLIPOVERALL"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle'>Avg BHP</td>");
		Response.Write("<td align = 'center'>" + dr["FLDAVGBHPBAD"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDAVGBHPGOOD"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDAVGBHPOVERALL"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("</tbody></table>");

		//Response.Write("<table class='pasSumTable' style='margin-top: 40px;'>");
		//Response.Write("<tbody><tr>");
		//Response.Write("<td colspan = '4' class='tdHeadTitle' align='left'>Superintendent Remarks</td>");
		//Response.Write("</tr>");
		//Response.Write("<tr>");
		//Response.Write("<td colspan = '4' align='left'>" + dr["FLDSUPDTREMARKS"].ToString() + "</td>");
		//Response.Write("</tr>");
		//Response.Write("</tbody></table>");

		//Time Analysis and Bunker Analysis 

		Response.Write("<table width = '100%' style = 'padding: 0; margin-top: 30px;'>");

		Response.Write("<tbody ><tr>");


		Response.Write("<td width = '40%' valign = 'top'>");

		Response.Write("<table class='pasSumTable' style='width: 400px; margin-right: 20px; margin-top: 0;'>");

		Response.Write("<tbody><tr>");
		Response.Write("<td colspan = '2' class='tdHeadTitle' align='left'>Time Analysis Based in Good Weather</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>CP Speed</td>");
		Response.Write("<td align = 'center'>" + dr["FLDCHARTERPARTYSPEED"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Predicted Time</td>");
		Response.Write("<td align = 'center'>" + dr["FLDPREDICTEDTIMEGOOD"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Time(Loss)/Gain in Good WX (hr)</td>");

		if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString() + "</td>");

		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Overall Time(loss)/gain</td>");
		if (General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDOVERALLTIMELOSSORGAIN2"].ToString() + "</td>");
		Response.Write("</tr>");
		Response.Write("</tbody></table>");

		Response.Write("</td>");

		//Bunker Analysis-- >
		Response.Write("<td width='60%' valign='top'>");


		Response.Write("<table class='pasSumTable' style='margin-top: 0;' width='100%'>");
		Response.Write("<tbody><tr>");
		Response.Write("<td colspan = '3' class='tdHeadTitle' align='left'>Bunker Analysis</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='tooltpStyle subHeadTitle' style='background: #ddd;' align='center'></td>");
		Response.Write("<td class='tooltpStyle subHeadTitle' align='center'>HFO</td>");
		Response.Write("<td class='tooltpStyle subHeadTitle' align='center'>MDO</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Daily CP Allowance</td>");
		Response.Write("<td align = 'center'> " + dr["FLDCPHFO"].ToString() + " </td>");

		Response.Write("<td align='center'>" + dr["FLDCPMDO"].ToString() + "</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Actual Good WX Consumption</td>");
		Response.Write("<td align = 'center'>" + dr["FLDACTHFOCONSGOOD"].ToString() + "</td>");

		Response.Write("<td align= 'center'>" + dr["FLDACTMDOCONSGOOD"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Good WX Avg.Daily Consumption</td>");
		Response.Write("<td align = 'center' > " + dr["FLDAVGHFOCONSGOOD"].ToString() + " </td>");

		Response.Write("<td align= 'center'>" + dr["FLDAVGMDOCONSGOOD"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Overall Avg.Daily Consumption</td>");
		Response.Write("<td align = 'center'>" + dr["FLDAVGHFOCONSOVERALL"].ToString() + "</td>");

		Response.Write("<td align= 'center'>" + dr["FLDAVGMDOCONSOVERALL"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Allowed Good WX Consumption</td>");
		Response.Write("<td align = 'center' >" + dr["FLDALLOWEDGOODWXHFOCONS"].ToString() + "</td>");

		Response.Write("<td align= 'center' >" + dr["FLDALLOWEDGOODWXMDOCONS"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Good WX(Over)/Under Consumption</td>");
		if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDGOODWXUNDEROVERHFOCONS"].ToString() + "</td>");

		if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDGOODWXUNDEROVERMDOCONS"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR'>");
		Response.Write("<td class='subSideTitle' align='center'>Overall(Over)/Under Consumption</td>");
		if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString() + "</td>");

		if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) < 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='lossVal'>" + -1 * General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) > 0)
		{
			Response.Write("<td align='center'>");
			Response.Write("<span class='gainVal'>" + General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) + "</span>");
			Response.Write("</td>");
		}
		else
			Response.Write("<td align = 'center'>" + dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString() + "</td>");

		Response.Write("</tr>");

		Response.Write("</tbody></table>");


		Response.Write("</td>");

		Response.Write("</tr>");

		Response.Write("</tbody></table>");


		//Passage-- >


		Response.Write("<table class='pasSumTable' cellpading='0' style='margin-top: 20px;' width='100%' cellspacing='0'>");
		Response.Write("<tbody><tr class='subSideTR' style='background: #ddd'>");
		Response.Write("<td></td>");
		Response.Write("<td colspan = '4' style='background: #bbb; color: #0A1D29; font-weight: 700;' align='center'>ROB</td>");
		Response.Write("<td colspan = '4' ></td>");

		Response.Write("</tr>");

		Response.Write("<tr class='subSideTR' style='background: #ddd'>");
		Response.Write("<td></td>");
		Response.Write("<td colspan = '2' style='background: #ccc; color: #0E5D9E; font-weight: 700;' align='center'>Departure(COSP)</td>");
		Response.Write("<td colspan = '2' style='background: #ccc; color: #0E5D9E; font-weight: 700;' align='center'>Arrival(EOSP)</td>");
		Response.Write("<td colspan = '4' style='background: #ccc; color: #0E5D9E; font-weight: 700;' align='center'>Consumed</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR lastTable' style='background: #0E5D9E; color: white;'>");
		Response.Write("<td>From - To</td>");
		Response.Write("<td align = 'center'> HFO </td>");

		Response.Write("<td align='center'>MDO</td>");
		Response.Write("<td align = 'center'> HFO </td>");

		Response.Write("<td align='center'>MDO</td>");
		Response.Write("<td align = 'center' > HFO </td >");

		Response.Write("<td align='center'>MDO</td>");
		Response.Write("<td align = 'center'> M/E </td>");

		Response.Write("<td align='center'>A/E</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR lastTable'>");
		Response.Write("<td class='subSideTitle' style='color: #0E5D9E; background: #ffff;'>Voy : " + dr["FLDFROMPORT"].ToString() + " - " + dr["FLDTOPORT"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDHFOROBONDEP"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDMDOROBONDEP"].ToString() + "</td>");
		Response.Write("<td align = 'center' >" + dr["FLDHFOROBONARR"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDMDOROBONARR"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDHFOCONS"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDMDOCONS"].ToString() + "</td>");
		Response.Write("<td align = 'center'>" + dr["FLDMEOVERALLCONS"].ToString() + "</td>");

		Response.Write("<td align='center'>" + dr["FLDAEOVERALLCONS"].ToString() + "</td>");
		Response.Write("</tr>");
		Response.Write("<tr class='subSideTR lastTable'>");
		Response.Write("<td colspan = '5' style='background: #ddd; font-weight: 700; border-top: 1px solid #0E5D9E' align='left'>Total</td>");
		Response.Write("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDHFOCONS"].ToString() + "</td>");
		Response.Write("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDMDOCONS"].ToString() + "</td>");
		Response.Write("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDMEOVERALLCONS"].ToString() + "</td>");
		Response.Write("<td style = 'border-top: 1px solid #0E5D9E' align='center'>" + dr["FLDAEOVERALLCONS"].ToString() + "</td>");
		Response.Write("</tr>");
		Response.Write("</tbody></table>");
		Response.End();
	}
}
