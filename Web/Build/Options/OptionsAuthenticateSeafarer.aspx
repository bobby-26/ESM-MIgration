<%@ Page Language="C#" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Purchase" %>
<%@ Import Namespace="SouthNests.Phoenix.DefectTracker" %>

<script language="c#" runat="server">
    
    private string strFunctionName;
    private string[] arrNames;
    private string[] arrValues;

    protected void Page_Load(object o, EventArgs e)
    {
        try
        {
            Session.Timeout = 120;
            GetParameters(Server.UrlDecode((new System.IO.StreamReader(Request.InputStream, Encoding.UTF8)).ReadToEnd()));

            if (strFunctionName == "GenerateReleaseNotes")
            {
                GenerateReleaseNotes();
            }
            if (strFunctionName == "GetSeafarerDetails")
            {
                GetSeafarerDetails();
            }
            if (strFunctionName == "ValidateSeafarer")
            {
                ValidateSeafarer();
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
  
    private void GetParameters(string strQueryString)
    {

        arrNames = new string[Request.QueryString.Count];
        arrValues = new string[Request.QueryString.Count];

        for (int i = 0; i < Request.QueryString.Count; i++)
        {
            arrNames[i] = Request.QueryString.GetKey(i);
            arrValues[i] = Request.QueryString.Get(i);
        }
        strFunctionName = arrValues[0];
    }
  
    private void GenerateReleaseNotes()
    {
        int iRowCount = 1000;
        int iTotalPageCount = 0;
        string actualfromdate = null;
        string actualtodate = null;

        string[] alColumns = { "FLDBUGID", "FLDTYPE", "FLDMODULENAME", "FLDSUBJECT", "FLDDESCRIPTION", "FLDPRIORITY", "FLDSEVERITYNAME", "FLDSTATUSNAME", "FLDREPORTEDBY" };
        string[] alCaptions = { "ID", "Type", "Module", "Subject", "Description", "Priority", "Severity", "Status", "Reported By" };

        DataSet ds = PhoenixDefectTracker.BugListByModule(
                                                             General.GetNullableString(""),
                                                             "",
                                                             General.GetNullableString(""),
                                                             General.GetNullableString(""),
                                                             General.GetNullableString(""),
                                                             General.GetNullableString(""),
                                                             General.GetNullableString(""),
                                                             General.GetNullableDateTime(""),
                                                             General.GetNullableDateTime(""),
                                                             General.GetNullableDateTime(""),
                                                             General.GetNullableDateTime(""),
                                                             General.GetNullableDateTime(""),
                                                             General.GetNullableDateTime(""),
                                                             General.GetNullableDateTime(""),
                                                             General.GetNullableDateTime(""),
                                                             1,
                                                             iRowCount,
                                                             ref iRowCount,
                                                             ref iTotalPageCount,
                                                             ref actualfromdate,
                                                             ref actualtodate
                                                           );

        DataTable dt = ds.Tables[0];
        DataTable dt3 = ds.Tables[1];
        Response.Write("<table width='100%' >");
        Response.Write("<tr>");
        Response.Write("<td  align='center'>");
        Response.Write("<b><h1> Phoenix Release note</h1></b>");
        Response.Write("</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td>");
        Response.Write("Report from " + actualfromdate + " to " + actualtodate);
        Response.Write("</td>");
        Response.Write("<td>");
        Response.Write("</td>");
        Response.Write("</tr>");
        Response.Write("<tr>");
        Response.Write("<td>");
        string module = "";
        string type = "";


        foreach (DataRow dr in dt.Rows)
        {
            Response.Write("<table>");
            if (module != dr["FLDMODULENAME"].ToString())
            {
                Response.Write("<tr>");
                Response.Write("<td colspan='2'>");
                Response.Write("<b><h2>" + dr["FLDMODULENAME"].ToString() + "</h2></b>");
                Response.Write("</td>");
                Response.Write("</tr>");
                module = dr["FLDMODULENAME"].ToString();
            }
            if (type != dr["FLDTYPE"].ToString())
            {
                Response.Write("<tr>");
                Response.Write("<td colspan='2'>");
                Response.Write("<b><h3>" + dr["FLDTYPE"].ToString() + "</h3></b>");
                Response.Write("</td>");
                Response.Write("</tr>");
                type = dr["FLDTYPE"].ToString();
            }
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("Issue ID");
            Response.Write("</td>");
            Response.Write("<td>");
            Response.Write(dr["FLDBUGID"].ToString());
            Response.Write("</td>");
            Response.Write("<td>");
            Response.Write("Posted by " + dr["FLDREPORTEDBY"].ToString() + " On " + dr["FLDOPENDATE"].ToString() + " Marked " + dr["FLDSEVERITYNAME"]);
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("Subject ");
            Response.Write("</td>");
            Response.Write("<td colspan='2'>");
            Response.Write(dr["FLDSUBJECT"].ToString());
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td valign='top'>");
            Response.Write("Description");
            Response.Write("</td>");
            Response.Write("<td colspan='2'>");
            Response.Write(dr["FLDDESCRIPTION"].ToString());
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Write("<table>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("Comments");
            Response.Write("</td>");
            Response.Write("</tr>");
            DataTable dt2 = PhoenixDefectTracker.DefectComments(new Guid(dr["FLDDTKEY"].ToString()));

            if (dt2.Rows.Count > 0)
            {
                foreach (DataRow drv in dt2.Rows)
                {
                    Response.Write("<tr>");
                    Response.Write("<td valign='top'>");
                    Response.Write(String.Format("{0:dd/MM/yyyy}", drv["FLDPOSTEDDATE"]));
                    Response.Write("</td>");
                    Response.Write("<td colspan='2'>");
                    Response.Write(drv["FLDCOMMENTS"].ToString() + "<br></br> - " + drv["FLDUSERNAME"].ToString());
                    Response.Write("</td>");
                    Response.Write("</tr>");
                }
            }
            else
            {
                Response.Write("<tr>");
                Response.Write("<td colspan='3'>");
                Response.Write("There are no comments for this issue");
                Response.Write("</td>");
                Response.Write("</tr>");
            }
            Response.Write("</table>");
            Response.Write("<hr/>");
        }
        Response.Write("<b>Clarification provided</b>");

        foreach (DataRow dr2 in dt3.Rows)
        {
            Response.Write("<table>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("Issue ID");
            Response.Write("</td>");
            Response.Write("<td colspan='2'>");
            Response.Write(dr2["FLDBUGID"].ToString());
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("Subject");
            Response.Write("</td>");
            Response.Write("<td colspan='2'>");
            Response.Write(dr2["FLDSUBJECT"].ToString());
            Response.Write("</td>");
            Response.Write("</tr>");
            Response.Write("</table>");
            Response.Write("<table>");
            Response.Write("<tr>");
            Response.Write("<td>");
            Response.Write("Comments:");
            Response.Write("</td>");
            Response.Write("</tr>");
            DataTable dt4 = PhoenixDefectTracker.DefectComments(new Guid(dr2["FLDDTKEY"].ToString()));
            if (dt4.Rows.Count > 0)
            {
                foreach (DataRow drv in dt4.Rows)
                {
                    Response.Write("<tr>");
                    Response.Write("<td valign='top'>");
                    Response.Write(String.Format("{0:dd/MM/yyyy}", drv["FLDPOSTEDDATE"]));
                    Response.Write("</td>");
                    Response.Write("<td colspan='2'>");
                    Response.Write(drv["FLDCOMMENTS"].ToString() + "<br></br> - " + drv["FLDUSERNAME"].ToString());
                    Response.Write("</td>");
                    Response.Write("</tr>");
                }
            }
            else
            {
                Response.Write("<tr>");
                Response.Write("<td colspan='3'>");
                Response.Write("There are no comments for this issue");
                Response.Write("</td>");
                Response.Write("</tr>");
            }
            Response.Write("</table>");
            Response.Write("<hr/>");
        }
        Response.Write("</td>");
        Response.Write("</tr>");
        Response.Write(" </table>");

        Response.ContentType = "application/vnd.ms-word";
        Response.BufferOutput = false;
        Response.Flush();
        Response.End();
    }
    private void GetSeafarerDetails()
    {
        try
        {
            NameValueCollection nvc = new NameValueCollection();
            string vesselid;
            string args = "";

            vesselid = ((arrValues[1]) == "") ? "0" : (arrValues[1]);
            DataSet ds = PhoenixDefectTracker.GetSeafarerDetails(General.GetNullableInteger(vesselid));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                args += dr["FLDMASTERNAME"].ToString();
                args += "|";
                args += dr["FLDCENAME"].ToString();
            }
            Response.Write(args);
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }
    private void ValidateSeafarer()
    {
        try
        {
            NameValueCollection nvc = new NameValueCollection();
            int vesselid;
            string fileno;
            string args = "";
            DateTime dob;
            fileno = arrValues[1];
            dob = DateTime.Parse(arrValues[2]);
            vesselid = int.Parse(arrValues[3]);
            DataSet ds = PhoenixDefectTracker.ValidateSeafarer(fileno, dob, vesselid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                args += dr["FLDSEAFARER"].ToString();
                args += "|";
                args += dr["FLDFILENO"].ToString();
                args += "|";
                args += dr["FLDEMPLOYEEID"].ToString();
            }
            Response.AddHeader("Access-Control-Allow-Credentials", "true");
            Response.Write(args);
        }
        catch (Exception ex)
        {
            Response.Write("Error" + "|" + ex.Message);
        }
    }
</script>

