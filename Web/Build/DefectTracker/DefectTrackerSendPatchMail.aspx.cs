using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTracker_DefectTrackerSendPatchMail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Send", "SEND");
        MenuNewMail.AccessRights = this.ViewState;
        MenuNewMail.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["SESSIONID"] = Guid.NewGuid().ToString();
            Guid mailid = (Session["Mailid"] != null ? new Guid(Session["Mailid"].ToString()) : Guid.NewGuid());
            ShowMail(mailid);           
        }
    }


    protected void VesselTo_Changed(object sender, EventArgs e)
    {
        int result = 0;
        Int32.TryParse(UcVessel.SelectedVessel, out result);

        DataTable dt = PhoenixMailManager.MailIdofVessel(result);
        txtTo.Text = "";
        if (dt.Rows.Count > 0)
            txtTo.Text = dt.Rows[0]["FLDEMAIL"].ToString();

        ViewState["vesselid"] = UcVessel.SelectedValue;        

    }
    private void ShowMail(Guid mailid)
    {
        string createdBy = Request.QueryString["createdby"].ToString();
        string filename = Request.QueryString["filename"].ToString();
        string attachment = Request.QueryString["filepath"].ToString() + "\\" + filename;

        lnkfilename.Text+="[" + filename + "]";
        lnkfilename.NavigateUrl = "DefectTrackerMailAttachment.ashx?path=" + attachment;

        txtCc.Text = "projectlead@southnests.com,esmit3@executiveship.com,cto@executiveship.com";
        txtSubject.Text = Request.QueryString["subject"].ToString();

        filename = filename.Substring(0, filename.Length - ".zip".Length);
        string mailbody = "";

        mailbody = "\nGood Day Captain,\n\n Kindly do the following steps to apply the patch,\n\n In the PC where Phoenix is installed.\n\n ";
        mailbody += "\tFor Windows-XP  machine,\n\n";
        mailbody += "\ta.\t Copy the attached zip file \"" + filename + ".zip\" to C:\\ folder.\n";
        mailbody += "\tb.\t Right click on the zip file and click on Extract to \"" + filename + "\\\" \n";
        mailbody += "\tc.\t Go to folder C:\\" + filename + "\n";
        mailbody += "\td.\t Double click on \"phnxPatch.bat\". \n";
        mailbody += "\te.\t Zip the output log file \"" + filename + ".log\" and Send the zip to sep@southnests.com \n\n";


        mailbody += "\tFor Windows-7  machine,\n\n";
        mailbody += "\ta.\t Copy the attached zip file \"" + filename + "\" to C:\\ folder.\n";
        mailbody += "\tb.\t Right click on the zip file and click on Extract to \"" + filename + "\\\" \n";
        mailbody += "\tc.\t Go to folder C:\\" + filename + "\n";
        mailbody += "\td.\t Right click on \"phnxPatch.bat\" and Select Run as Administrator. If prompt for confirmation, Click yes \n";
        mailbody += "\te.\t Zip the output log file \"" + filename + ".log\" and Send the zip to sep@southnests.com \n\n";
        mailbody += "Note :   If you are using Windows-7 , do not double click on \"phnxPatch.bat\". In order to run batch file you must run as Administrator.\n\n";
        mailbody += "Best Regards \n";
        mailbody += createdBy + "(Phoenix Support)";

        if (Session["Mailid"] != null)
        {
            DataTable dt = PhoenixMailManager.MailToRead(mailid);
            DataRow dr = dt.Rows[0];

            txtCc.Text = dr["FLDCC"].ToString();
            txtTo.Text = dr["FLDFROM"].ToString();
            mailbody += "\n\n------------------------------------------";
            mailbody += "\nDate :" + dr["FLDRECEIVEDON"];
            mailbody += "\nFrom :" + dr["FLDFROM"];
            mailbody += "\nCc :" + dr["FLDCC"];
            txtMessage.Text = mailbody + dr["FLDBODY"].ToString();
            ViewState["MESSAGEID"] = dr["FLDMESSAGEID"].ToString();
        }
        else
        {

            txtMessage.Text = mailbody;
            ViewState["MESSAGEID"] = Guid.NewGuid().ToString() + "@esmexchhub1.executiveship.com";
        }


    }

    protected void MenuNewMail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;            

            if (dce.CommandName.ToUpper().Equals("SEND"))            {
               
                PhoenixMailManager.MailAttachmentInsert(General.GetNullableGuid(ViewState["SESSIONID"].ToString()),
                                                        ViewState["MESSAGEID"].ToString(),
                                                        Request.QueryString["filepath"].ToString()+"\\"+Request.QueryString["filename"].ToString()
                                                        );                

                PhoenixDefectTracker.PatchVesselUpdate(General.GetNullableGuid(Request.QueryString["patchdtkey"].ToString()),
                                                        Convert.ToInt32(ViewState["vesselid"]), General.GetNullableDateTime(""), General.GetNullableDateTime(DateTime.Now.ToString()),null,null
                                                       );

                PhoenixMailManager.MailToSend(General.GetNullableGuid(ViewState["SESSIONID"].ToString()), "SEP", ViewState["MESSAGEID"].ToString(),
                                                txtTo.Text, txtCc.Text, txtSubject.Text, txtMessage.Text, "","","",""
                                               );
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
