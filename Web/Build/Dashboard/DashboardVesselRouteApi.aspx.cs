using System;

using SouthNests.Phoenix.Framework;
using System.Data;
using Telerik.Web.UI;
using System.Text;
using SouthNests.Phoenix.VesselRoteAPI;

using System.Net;
using System.IO;


public partial class Dashboard_DashboardVesselRouteApi : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        BindVesselList();

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddButton("Send", "SEND",ToolBarDirection.Right);
        tabmenu.MenuList = toolbargrid.Show();
    }

    private void BindVesselList()
    {
        DataTable dt = PhoenixVesselRouteAPI.VesselList();
        ddlvessel.DataSource = dt;
        ddlvessel.DataTextField = "FLDVESSELNAME";
        ddlvessel.DataValueField = "FLDAPIVESSELID";
        ddlvessel.DataBind();
    }

    protected void tabmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEND"))
            {
                SendResponse(ddlvessel.SelectedValue,txtemail.Text , "ESM " +ddlvessel.SelectedText +" Route");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }
    public static void SendResponse(string VesselId, string Email , string Subject )
    {
       

        xwwwformBodyWebRequest(null, VesselId, Email, Subject);

       


    }
    public static void xwwwformBodyWebRequest(string URL, string EndPoint, string Email , string Subject)
    {

        DataTable dt = PhoenixVesselRouteAPI.APIConfig();

        string UserName = dt.Rows[0]["FLDUSERNAME"].ToString();
        string Password = dt.Rows[0]["FLDPASSWORD"].ToString();
        
        string ClientId = dt.Rows[0]["FLDCLIENTID"].ToString();
        string Resource = dt.Rows[0]["FLDRESOURCE"].ToString();


        var baseAddress = dt.Rows[0]["FLDURL"].ToString() + EndPoint;




        var request = (HttpWebRequest)WebRequest.Create(baseAddress);

        var postData = "grant_type=" + Uri.EscapeDataString("password");
        postData += "&client_id=" + Uri.EscapeDataString(ClientId);
        postData += "&username=" + Uri.EscapeDataString(UserName);
        postData += "&password=" + Uri.EscapeDataString(Password);
        postData += "&scope=" + Uri.EscapeDataString("user_impersonation");
        postData += "&resource=" + Uri.EscapeDataString(Resource);
        var data = Encoding.ASCII.GetBytes(postData);

        request.Method = "POST";
        request.ContentType = "application/x-www-form-urlencoded";
        request.ContentLength = data.Length;

        using (var stream = request.GetRequestStream())
        {
            stream.Write(data, 0, data.Length);
        }
        

        
        var response = (HttpWebResponse)request.GetResponse();
        StreamReader reader = new StreamReader(response.GetResponseStream());
        string Attachment = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "\\TEMP\\" + "RouteAPIResponse.rtz";
        using (TextWriter tw = new StreamWriter(Attachment))
        {
            tw.WriteLine(reader.ReadToEnd());

        }

        string sessionid = Guid.NewGuid().ToString();
      
        string destinationdirectory = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + @"\EmailAttachments\";
        if (!Directory.Exists(destinationdirectory + sessionid))
            Directory.CreateDirectory(destinationdirectory + sessionid);
        if (File.Exists(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "\\TEMP\\" + "RouteAPIResponse.rtz"))
            File.Copy(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "\\TEMP\\" + "RouteAPIResponse.rtz", destinationdirectory + sessionid + "/"  + "RouteAPIResponse.rtz", true);
        PhoenixMail.SendMail(Email.Trim().TrimEnd(','), null, null, Subject, "PFA", true, System.Net.Mail.MailPriority.Normal, sessionid);








    }
}