using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Data;
using SouthNests.Phoenix.DryDock;
using SouthNests.Phoenix.Export2XL;
using System.IO;
using Telerik.Web.UI;

public partial class DryDockSendQuery : PhoenixBasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Send", "SEND",ToolBarDirection.Right);
        MenuMailReply.AccessRights = this.ViewState;
        MenuMailReply.MenuList = toolbar.Show();
        ViewState["QUOTATIONID"] = Request.QueryString["quotationid"].ToString();
        ViewState["VESSELID"] = Request.QueryString["vslid"].ToString();

        ShowMail(General.GetNullableGuid(ViewState["QUOTATIONID"].ToString().Split(',')[0]));
    }

    private void ShowMail(Guid? quotationid)
    {
        DataTable dt = PhoenixDryDockService.DryDockMailToSend(quotationid, int.Parse(ViewState["VESSELID"].ToString()));
        DataRow dr = dt.Rows[0];

        
        txtFrom.Text = dr["FLDMAILUSERNAME"].ToString();
        if(dr["FLDFROM"].ToString().ToUpper().Contains("SEP"))
            txtTo.Text = dr["FLDTO"].ToString();
        else
            txtTo.Text = dr["FLDFROM"].ToString() + "," + dr["FLDTO"].ToString();

        txtCc.Text = dr["FLDCC"].ToString();
        txtSubject.Text = dr["FLDSUBJECT"].ToString();

        txtMessage.Text = dr["FLDBODY"].ToString();

        

        string attachmentpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
        lnkfilename.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filters1','Download','Drydock/Drydockdownload.aspx?Drydockquotationid=" + ViewState["QUOTATIONID"] + "&vslid=" + ViewState["VESSELID"] + "','false','800px','420px');return false");

        //lnkfilename.NavigateUrl = "javascript:parent.openNewWindow('Filters1','download','Common/Download.aspx?Drydockquotationid=" + ViewState["QUOTATIONID"] + "&vslid=" + ViewState["VESSELID"] + "','false','800px','420px')";
            //attachmentpath + "\\Drydock\\" + PhoenixDryDockQuotation.GetDryDockQuotationYardZipFileName(quotationid.Value, int.Parse(Request.QueryString["vslid"].ToString())) +".zip";
    }


    protected void MenuMailReply_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEND"))
            {
                string temp = string.Empty;
                if (Request.QueryString["type"] != null && Request.QueryString["type"] == "sendquote")
                {                  
                    foreach (string str in ViewState["QUOTATIONID"].ToString().Split(','))
                    {
                        string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                        if (!path.EndsWith("/"))
                            path = path + "/";
                        


                        temp = PhoenixDryDockQuotation.GetDryDockQuotationYardZipFileName(new Guid(str), int.Parse(Request.QueryString["vslid"].ToString()));
                        ShowMail(General.GetNullableGuid(str));
                        PhoenixDryDock2XL.Export2XLDryDockDocuments(General.GetNullableGuid(Request.QueryString["orderid"]), General.GetNullableGuid(str), int.Parse(Request.QueryString["vslid"].ToString()),null);
                        string sessionid = Guid.NewGuid().ToString();
                        string destinationdirectory = Server.MapPath("~/Attachments/EmailAttachments/");
                        if (!Directory.Exists(destinationdirectory + sessionid))
                            Directory.CreateDirectory(destinationdirectory + sessionid);
                        if (File.Exists(PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath+ "\\Drydock\\" + temp + ".zip"))
                            File.Copy(Server.MapPath(path + "Drydock//" + temp + ".zip"), destinationdirectory + sessionid + "/" + temp + ".zip", true);                        
                        PhoenixMail.SendMail(txtTo.Text.Trim().TrimEnd(','), txtCc.Text.Trim().TrimEnd(','), null, txtSubject.Text.Trim(), txtMessage.Text, true, System.Net.Mail.MailPriority.Normal, sessionid);                        
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
}
