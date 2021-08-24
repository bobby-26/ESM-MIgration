using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Web;
using System.Collections.Specialized;
public partial class AccountsEmployeeAllotmentRequestToolTip : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.QueryString.ToString().Replace("amp;", "").Replace("amp%3b", ""));
            ViewState["SIDELETTERYN"] = nvc["sideletteryn"];
            ViewState["EMPLOYEEID"] = nvc["EMPID"];
            ViewState["VESSELID"] = nvc["VESSELID"];
            ViewState["MONTH"] = nvc["MONTH"];
            ViewState["YEAR"] = nvc["YEAR"];
            if (ViewState["SIDELETTERYN"].ToString() == "1")
            { lblsideletter.Visible = true; }
            else { lblsideletter.Visible = false; }
            BindData();
        }
    }
    public string BindData()
    {  string htmlStr = "";    
        DataSet ds = PhoenixAccountsEmployeeAllotmentRequest.AllotmentRequestSearch(1,
                                                                                    General.GetNullableInteger(ViewState["EMPLOYEEID"].ToString()),
                                                                                      General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                                                                                   General.GetNullableInteger(ViewState["MONTH"].ToString()),
                                                                                   General.GetNullableInteger(ViewState["YEAR"].ToString()),
                                                                               null);
        if (ds.Tables[0].Rows.Count > 0)
        {
            htmlStr += "< tr >< td > Number</ td >< td > Type </ td >< td > Amount </ td >< td > Status </ td >< td > Voucher Number </ td ></ tr >";
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                htmlStr += "<tr><td>" + dr["FLDREQUESTNUMBER"] + "</td><td>" + dr["FLDALLOTMENTTYPENAME"] + "</td><td>" + dr["FLDAMOUNT"] + "</td><td>" + dr["FLDREQUESTSTATUSNAME"] + "</td><td>" + dr["FLDPAYMENTVOUCHERNUMBER"] + "</td></tr>";
            }
          
            //gvAllotment.DataSource = ds;
            //gvAllotment.DataBind();
        }
        return htmlStr;
    }
 
}
