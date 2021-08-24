using System;
using System.Collections;
using System.Web;
using System.Data;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.VesselAccounts;
using System.Text;
public partial class VesselAccountAdminLeaveWages :  PhoenixBasePage
{
    private string strFunctionName;
    private string[] arrNames;
    private string[] arrValues;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.QueryString.ToString().Replace("amp;", "").Replace("amp%3b", ""));
            ViewState["PBID"] = nvc["PBID"];
            ViewState["SIGNONOFFID"] = nvc["SIGNONOFFID"];
            ViewState["SIGNOFFYN"] = nvc["SIGNOFFYN"];
            ViewState["CLOSINGDATE"] = nvc["CLOSINGDATE"];

            BindData("1");
            BindData("2");

        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    private void GetParameters(string strQueryString)
    {
        string[] arrURLParameters;


        arrURLParameters = strQueryString.Split('|');
        arrNames = new string[arrURLParameters.Length];
        arrValues = new string[arrURLParameters.Length];

        for (int i = 0; i < arrURLParameters.Length; i++)
        {
            arrNames[i] = arrURLParameters[i].Split('=')[0];
            arrValues[i] = arrURLParameters[i].Split('=')[1];
        }

        strFunctionName = arrValues[0];
    }
    public string BindData(string type)
    {
        string htmlStr = "";
        DataSet ds = PhoenixVesselAccountsAdminPB.FetchPBLeaveWagesList(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                         , new Guid(ViewState["PBID"].ToString())
                                                         , int.Parse(ViewState["SIGNONOFFID"].ToString())
                                                         , int.Parse(ViewState["SIGNOFFYN"].ToString())
                                                         , DateTime.Parse(ViewState["CLOSINGDATE"].ToString()));
        if (type == "1")
        {
            htmlStr += "<tr><td colspan='4'><b>Included in PB</b></td></tr>";
            htmlStr += " <tr class='DataGrid-HeaderStyle'><td> Component </td><td> From </td><td>To</td><td>Amount</td></tr>";
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    htmlStr += "<tr><td>" + dr["FLDCOMPONENTNAME"] + "</td><td>" + General.GetDateTimeToString(dr["FLDFROMDATE"].ToString()) + "</td><td>" + General.GetDateTimeToString(dr["FLDTODATE"].ToString()) + "</td><td  align=right>" + dr["FLDAMOUNT"] + "</td></tr>";
                }
            }
        }
        if (type == "2")
        {
            if (ds.Tables[1].Rows.Count > 0)
            {
                //string jvscript = "";
                //jvscript = "javascript:IncludetoPB('" + ViewState["PBID"] + "','" + ViewState["SIGNONOFFID"] + "','" + ViewState["SIGNOFFYN"] + "','" + ViewState["CLOSINGDATE"] + ");";
                //cmdB.Attributes.Add("onclick", jvscript);
                htmlStr += "<tr><td colspan='4'><b>Not Included in PB</b></td></tr>";
                htmlStr += " <tr class='DataGrid-HeaderStyle'><td> Component </td><td> From </td><td>To</td><td>Amount</td></tr>";
                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    htmlStr += "<tr><td>" + dr["FLDCOMPONENTNAME"] + "</td><td>" + General.GetDateTimeToString(dr["FLDFROMDATE"].ToString()) + "</td><td>" + General.GetDateTimeToString(dr["FLDTODATE"].ToString()) + "</td><td  align=right>" + dr["FLDAMOUNT"] + "</td></tr>";
                }
            }
        }
        return htmlStr;
    }


}