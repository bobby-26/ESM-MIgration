using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Collections.Specialized;
using System.Data;
using SouthNests.Phoenix.Framework;

public partial class VesselAccounts_VesselAccountEarningDeductionToolTip :  PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.QueryString.ToString().Replace("amp;", "").Replace("amp%3b", ""));
            ViewState["EMPLOYEEID"] = nvc["EMPLOYEEID"];
            ViewState["SIGNONOFFID"] = nvc["SIGNONOFFID"];
            ViewState["MONTH"] = nvc["MONTH"];
            ViewState["YEAR"] = nvc["YEAR"];
            ViewState["TYPE"] = nvc["TYPE"];

            BindData();
        }
    }
    public string BindData()
    {
        string htmlStr = "";
        if (ViewState["TYPE"].ToString() == "1")
        {
            htmlStr += " <tr><td> Entry Type </td><td> Date </td><td>Purpose</td><td>Amount(USD)</td></tr>";
        }
        else
        {
            htmlStr += " <tr><td>Date</td><td>Purpose</td><td>Amount(USD)</td></tr >";
        }
        DataSet ds = PhoenixVesselAccountsEarningsDeductions.VesselEarningDeductionToolTipList(General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString())
                                                           , General.GetNullableInteger(ViewState["SIGNONOFFID"].ToString())
                                                           , General.GetNullableInteger(ViewState["TYPE"].ToString())
                                                           , General.GetNullableInteger(ViewState["MONTH"].ToString())
                                                           , General.GetNullableInteger(ViewState["YEAR"].ToString())
                                                           );
        if (ds.Tables[0].Rows.Count > 0)
        {
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                if (ViewState["TYPE"].ToString() == "3")
                {
                    htmlStr += "<tr><td>" + General.GetDateTimeToString(dr["FLDDATE"].ToString()) + "</td><td>" + dr["FLDPURPOSE"] + "</td><td align=right>" + dr["FLDAMOUNT"] + "</td></tr>";
                }
                else if (ViewState["TYPE"].ToString() == "1")
                {
                    htmlStr += "<tr><td>" + dr["FLDHARDNAME"] + "</td><td>" + DateTime.Parse("01/" + dr["FLDMONTH"].ToString() + "/" + dr["FLDYEAR"].ToString()).ToString("MMM") + "-" + dr["FLDYEAR"].ToString() + "</td><td>" + dr["FLDPURPOSE"] + "</td><td align=right>" + dr["FLDAMOUNT"] + "</td></tr>";
                }
                else
                {
                    htmlStr += "<tr><td>" + DateTime.Parse("01/" + dr["FLDMONTH"].ToString() + "/" + dr["FLDYEAR"].ToString()).ToString("MMM") + "-" + dr["FLDYEAR"].ToString() + "</td><td>" + dr["FLDPURPOSE"] + "</td><td align=right>" + dr["FLDAMOUNT"] + "</td></tr>";
                }

            }
            if (ViewState["TYPE"].ToString() == "1")
            {
                htmlStr += "<tr><td colspan= '3' align=right>Total Earnings  </td><td align=right>" + ds.Tables[0].Rows[0]["FLDEARNINGTOTAL"] + "</td></tr>";
                htmlStr += "<tr><td colspan= '3' align=right>Total Deduction </td><td align=right>" + ds.Tables[0].Rows[0]["FLDDEDUCTIONTOTAL"] + "</td></tr>";
            }
            else
            {
                htmlStr += "<tr><td colspan= '2'  align=right>Total Deductions </td><td align=right>" + ds.Tables[0].Rows[0]["FLDDEDUCTIONTOTAL"] + "</td></tr>";
            }
        }
        return htmlStr;
    }


}