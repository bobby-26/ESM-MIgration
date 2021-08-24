using System;
using System.Data;
using System.IO;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using System.Configuration;

public partial class PurchaseVendorRequoteRemarks : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        try
        {
               toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
               MenuFormDetail.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                if ((Request.QueryString["quotationid"] != null) && (Request.QueryString["quotationid"] != ""))
                {
                    ViewState["quotationid"] = Request.QueryString["quotationid"].ToString();
                    ViewState["dtkey"] = string.Empty;
                    ViewState["orderid"] = "";
                    BindData(Request.QueryString["quotationid"].ToString());
                }
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData(string quotationid)
    {

        DataSet ds = new DataSet();
        ds = PhoenixPurchaseQuotation.EditQuotation(new Guid(quotationid));
        if (ds.Tables[0].Rows.Count > 0)
        {           
            DataRow dr = ds.Tables[0].Rows[0];
            txtFormDetails.Content = dr["FLDREQUOTEREMARKS"].ToString();
            ViewState["orderid"] = dr["FLDORDERID"].ToString();
        }        
    }

    protected void MenuFormDetail_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        try
        {
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidConfig())
                {
                    ucError.Visible = true;
                    return;
                }

                if ((Request.QueryString["quotationid"] != null) && (Request.QueryString["quotationid"] != ""))
                {
                    PhoenixPurchaseQuotation.UpdateQuotationRequoteRemarks(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["quotationid"].ToString()), txtFormDetails.Content);

                    PhoenixCommoneProcessing.ReOpenSession(new Guid(ViewState["quotationid"].ToString()));

                    PhoenixPurchaseQuotation.UpdateQuotationForRequote(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["quotationid"].ToString()));

                    
                    ucStatus.Text = "Quotation is updated to allow re-quote.";
                    ucStatus.Visible = true;

                    SendForRequote();
                }
              
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
 
    }

    //protected void btnInsertPic_Click(object sender, EventArgs e)
    //{
    //    try
    //    {            
    //        if (Request.Files.Count > 0 && txtFormDetails.ActiveMode == AjaxControlToolkit.HTMLEditor.ActiveModeType.Design)
    //        {
    //            Guid gFileName = PhoenixCommonFileAttachment.InsertAttachment(Request.Files, new Guid(ViewState["dtkey"].ToString()), PhoenixModule.PURCHASE, null, ".jpg,.png,.gif");
    //            DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["dtkey"].ToString()));
    //            DataRow[] dr = dt.Select("FLDDTKEY = '" + gFileName.ToString() + "'");
    //            if (dr.Length > 0)
    //               txtFormDetails.Content = txtFormDetails.Content + "<img src=\"" + HttpContext.Current.Session["sitepath"] + "/attachments/" + dr[0]["FLDFILEPATH"].ToString() +"\" />";
    //        }
    //        else
    //        {
    //            ucError.Text = Request.Files.Count > 0 ? "You are not in design mode" : "No Picture selected.";
    //            ucError.Visible = true;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }       
    //}
    private bool IsValidConfig()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtFormDetails.Content) == null)
            ucError.ErrorMessage = "Remarks is required.";


        return (!ucError.IsError);
    }

    private void SendForRequote()
    {
        string emailid;
        DataSet dsvendor = PhoenixPurchaseQuotation.ListQuotationToSendRequote(General.GetNullableGuid(ViewState["orderid"].ToString()), General.GetNullableGuid(ViewState["quotationid"].ToString()));
        if (dsvendor.Tables[0].Rows.Count > 0)
        {

            foreach (DataRow dr in dsvendor.Tables[0].Rows)
            {
                string emailbodytext = "";
                string supemail = "";
                if (dr["FLDEMAIL1"].ToString().Contains(";"))
                    emailid = dr["FLDEMAIL1"].ToString().Replace(";", ",");
                else
                    emailid = dr["FLDEMAIL1"].ToString();

                if (!dr["FLDEMAIL2"].ToString().Equals(""))
                {
                    emailid = emailid + "," + dr["FLDEMAIL2"].ToString().Replace(";", ",");
                }

                if (dr["FLDSUPTEMAIL"].ToString().Equals("") || dr["FLDSUPPLIEREMAIL"].ToString().Equals(""))
                    supemail = dr["FLDSUPTEMAIL"].ToString() + dr["FLDSUPPLIEREMAIL"].ToString();
                else
                    supemail = dr["FLDSUPTEMAIL"].ToString() + "," + dr["FLDSUPPLIEREMAIL"].ToString();

                supemail = dr["FLDSUPPLIEREMAIL"].ToString();

                DataSet dscontact;
                dscontact = PhoenixPurchaseQuotation.QuotationContactsGetEmail(General.GetNullableInteger(dr["FLDVENDORID"].ToString()), Filter.CurrentPurchaseStockType.ToString(), Filter.CurrentPurchaseVesselSelection);
                if (!dscontact.Tables[0].Rows[0]["FLDEMALTO"].ToString().Trim().Equals(""))
                {
                    emailid = emailid + dscontact.Tables[0].Rows[0]["FLDEMALTO"].ToString().Trim();
                }
                if (!dscontact.Tables[0].Rows[0]["FLDEMALCC"].ToString().Trim().Equals(""))
                {
                    supemail = supemail + dscontact.Tables[0].Rows[0]["FLDEMALCC"].ToString().Trim();
                }

                try
                {
                    emailbodytext = PrepareEmailBodyTextForRequote(new Guid(dr["FLDQUOTATIONID"].ToString()), dr["FLDFORMNO"].ToString(), dr["FLDFROMEMAIL"].ToString());
                    PhoenixCommoneProcessing.PrepareEmailMessageRequote(
                            emailid, "RFQ", new Guid(dr["FLDQUOTATIONID"].ToString()),
                            "", supemail.Equals("") ? dr["FLDFROMEMAIL"].ToString() : supemail + "," + dr["FLDFROMEMAIL"].ToString(),
                            dr["FLDNAME"].ToString(), dr["FLDVESSELNAME"].ToString() + " - RE-QUOTE for " + dr["FLDFORMNO"].ToString() + "" + (dr["FLDTITLE"].ToString() == "" ? "" : "-") + dr["FLDTITLE"].ToString(), emailbodytext, "", "");

                    ucConfirm.ErrorMessage = "Email sent to " + dr["FLDNAME"].ToString() + "\n";
                }
                catch (Exception ex)
                {
                    ucConfirm.ErrorMessage = ex.Message + " for  " + dr["FLDNAME"].ToString() + "\n";
                }
            }

            ucConfirm.Visible = true;
        }

    }

    protected string PrepareEmailBodyTextForRequote(Guid quotationid, string orderformnumber, string frommailid)
    {
        StringBuilder sbemailbody = new StringBuilder();
        DataSet ds = PhoenixPurchaseQuotation.GetQuotationDataForEmailBody(PhoenixSecurityContext.CurrentSecurityContext.UserCode, quotationid, "RFQ");
        DataRow dr = ds.Tables[0].Rows[0];
        string mailbody;
        
        mailbody = "This is an automated message. DO NOT REPLY to "+ConfigurationManager.AppSettings.Get("FromMail").ToString()+". Kindly use the \"reply all\" button if you are responding to this message.";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + dr["FLDVENDORNAME"].ToString() + "             " + orderformnumber;
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "Sub: Request for Re-quote";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "Dear Sir";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "<br/>";

        mailbody = mailbody + txtFormDetails.Content;
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "Reply as soon as possible";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "Thank you,";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + dr["FLDUSERNAME"].ToString();
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "For and on behalf of " + dr["FLDCOMPANYNAME"].ToString();
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "(As Agents for Owners)";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "Contact: " + frommailid;
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "--------------------------------------------------------------------";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + dr["FLDVENDORNAME"].ToString() + "             " + orderformnumber;
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "Dear Sir";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + dr["FLDCOMPANYNAME"].ToString() + " hereby requests you to provide your BEST quotation for the following items to be delivered to our vessel ";
        mailbody = mailbody + dr["FLDVESSELNAME"].ToString();
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "Request your IT department to kindly allow access to this URL for submitting quotes.";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "Please click on the link below and key in the relevant fields indicated. If the link is wrapped, please copy and paste it on the address bar of your browser";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "<br/>";

        string url = Session["sitepath"] + "/" + dr["URL"].ToString();
        if (Filter.CurrentPurchaseStockType == null || Filter.CurrentPurchaseStockType.Equals(string.Empty))
            mailbody = mailbody + url + "?SESSIONID=" + dr["SESSIONID"].ToString();
        else
            mailbody = mailbody + url + "?SESSIONID=" + dr["SESSIONID"].ToString() + "&STOCKTYPE=" + Filter.CurrentPurchaseStockType ;
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "<br/>";

        if (dr["FLDEXPIRYDATE"].ToString() != "")
        {
            mailbody = mailbody + "We request you to submit your bid by "+ dr["FLDEXPIRYDATE"].ToString()+", failing which your offer will NOT be accepted. If you wish to decline to bid, please advise us by email with your reasons for declining." ;
        }
        else
        {
            mailbody = mailbody + "We request you to submit your bid, failing which your offer will NOT be accepted. If you wish to decline to bid, please advise us by email with your reasons for declining.";
        }

        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "Note: In our continual effort to keep correct records of your address and contact information, we appreciate your time to verify and correct it where necessary. Please click on the link below to view/correct the address.";
        mailbody = mailbody + "<br/>";

        DataSet dsvendorid = PhoenixPurchaseQuotation.EditQuotation(quotationid);
        DataRow drvendorid = dsvendorid.Tables[0].Rows[0];
        string vendorid = drvendorid["FLDVENDORID"].ToString();

        DataSet dsaddress = PhoenixRegistersAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(vendorid));
        DataRow draddress = dsaddress.Tables[0].Rows[0];

        mailbody = mailbody + "\n" + Session["sitepath"] + "/Purchase/PurchaseVendorAddressEdit.aspx?sessionid=" + draddress["FLDDTKEY"].ToString() ;

        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "Thank you,";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + dr["FLDUSERNAME"].ToString();
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "For and on behalf of " + dr["FLDCOMPANYNAME"].ToString();
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "(As Agents for Owners)";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "<br/>";
        mailbody = mailbody + "This is an automated message.";
        mailbody = mailbody + "If you need personal attention, use \"reply all\" to get your communication across to an email id that is monitored.";
        mailbody = mailbody + "Please note "+ConfigurationManager.AppSettings.Get("FromMail").ToString()+" is NOT monitored.";
      //string str = new HtmlString(sbemailbody.ToString());

        return mailbody;

    }
}
