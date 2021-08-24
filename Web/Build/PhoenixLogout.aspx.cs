using SouthNests.Phoenix.Framework;
using System;
using System.Web.UI;

public partial class PhoenixLogout : System.Web.UI.Page
{   
    protected void Page_Load(object sender, EventArgs e)
    {
        string accessedby = string.Empty;
        try
        {
            if (Session != null)
                accessedby = Filter.PortalAccessedBy;
            Session.Clear();
            Session.Abandon();
        }
        catch { }
        //FormsAuthentication.SignOut();
        RegisterAnchorScript(accessedby);
    }

    private void RegisterAnchorScript(string accessedby)
    {
        string Script = "";
        Script += "<script language=JavaScript id='BookMarkScript'>" + "\n";

        if (accessedby == "AGENT")
            Script += "top.location.href='Portal/PortalDefault.aspx'";

        else if (accessedby == "STUDENT")
            Script += "top.location.href='Portal/PortalStudentDefault.aspx'";
        else if (accessedby == "VESSEL")
            Script += "top.location.href='Portal/PortalVesselDefault.aspx'";
        else if (accessedby == "SEAFARER")
            Script += "top.location.href='Portal/PortalSeafarerDefault.aspx'";
        else
        {
            if (Request.FilePath.Split('/').Length > 3)
                Script += "top.location.href='../Default.aspx'";
            else
                Script += "top.location.href='Default.aspx'";
        }

        Script += "</script>" + "\n";

        if (!ClientScript.IsStartupScriptRegistered("BookMarkScript"))
            ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
    }

}
