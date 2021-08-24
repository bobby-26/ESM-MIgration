<%@ Page Language="C#" EnableViewState="false" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Reflection" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

    DataSet ds = new DataSet();
    string[] alColumns;
    string[] alCaptions;
    string printheader;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString["issystemsecuritycontext"] != null && Request.QueryString["issystemsecuritycontext"].ToString().ToUpper().Equals("TRUE"))
                PhoenixSecurityContext.CurrentSecurityContext = PhoenixSecurityContext.SystemSecurityContext;

            string gridview = Request.QueryString["gv"].ToString();
            alCaptions = (string[])Session[gridview + "PRINTCAPTIONS"];
            alColumns = (string[])Session[gridview + "PRINTCOLUMNS"];
            ds = (DataSet)Session[gridview + "PRINTDATA"];
            printheader = (string)Session[gridview + "PRINTHEADER"];
        }
        catch (Exception ex)
        {
            Response.Write("Refresh the screen and try again!<br/>" + ex.Message);
        }
    }
</script>
<html>
<head>
    <script language="javascript" type="text/javascript">
        function cmdPrint_Click() {
            document.getElementById('cmdPrint').style.visibility = "hidden";
            window.print();
        }
    </script>
</head>
<body>
    <div style="height:735px; width: 100%; overflow: auto;">
        <form id="form1" runat="server">
            <h3><%=printheader %></h3>
            <table>
                <tr>
                    <td>
                        <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" />
                    </td>
                </tr>
            </table>

            <table border='1' cellpadding='2' cellspacing='0' width='100%'>
                <tr>
                    <%
                        for (int i = 0; i < alCaptions.Length; i++)
                        {
                    %>
                    <td>
                        <b><%=alCaptions[i]%></b>
                    </td>
                    <%}%>
                </tr>
                <%        
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                %>
                <tr>
                    <%
                        for (int i = 0; i < alColumns.Length; i++)
                        {
                    %>
                    <td>
                        <%=dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]].ToString()%>
                    </td>
                    <%}%>
                </tr>
                <%}
                %>
            </table>

            <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility: hidden" />
        </form>
    </div>
</body>
</html>
