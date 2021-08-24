using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Purchase;
using SouthNests.Phoenix.Framework;
using System.Text;
using Telerik.Web.UI;

public partial class PurchaseDeliveryForwarderEmail : PhoenixBasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Discard", "DISCARD",ToolBarDirection.Right);
            toolbar.AddButton("Send", "SEND", ToolBarDirection.Right);

            MenuCorrespondenceEmail.AccessRights = this.ViewState;
            MenuCorrespondenceEmail.MenuList = toolbar.Show();
            if (!IsPostBack)
            {

                ViewState["DELIVERYID"] = Request.QueryString["deliveryid"];

                ViewState["mailsessionid"] = "";

                ViewState["mailsessionid"] = System.DateTime.Now.Day.ToString() + "_" + System.DateTime.Now.Month.ToString() + "_" + System.DateTime.Now.Year.ToString() + "_" + System.DateTime.Now.Hour.ToString() + "_" + System.DateTime.Now.Minute.ToString() + "_" + System.DateTime.Now.Second.ToString() + "_" + System.DateTime.Now.Millisecond.ToString();

                BindBody();
            }

        }
        catch (Exception ex)
        {
            MenuCorrespondenceEmail.Visible = false;
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void CorrespondenceEmail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SEND"))
            {
                if (!IsValidCorrespondenceEmail())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixMail.SendMail(txtTO.Text, txtCC.Text, null, txtSubject.Text, txtBody.Content.Trim()
                               , true, System.Net.Mail.MailPriority.Normal, ViewState["mailsessionid"].ToString(), null);

                //ResetFields();

                ucStatus.Text = "Email sent";

            }
            else
            {
                ResetFields();
            }

        }
        catch (Exception ex)
        {

            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ResetFields()
    {
        txtFrom.Text = string.Empty;
        txtTO.Text = string.Empty;
        txtCC.Text = string.Empty;
        txtBCC.Text = string.Empty;
        txtSubject.Text = string.Empty;
        txtBody.Content = string.Empty;

    }
    public bool IsValidCorrespondenceEmail()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        //int resultInt;

        if (string.IsNullOrEmpty(txtSubject.Text))
            ucError.ErrorMessage = "Subject is required.";


        if (txtFrom.Text.Trim() != string.Empty && !General.IsvalidEmail(txtFrom.Text))
            ucError.ErrorMessage = "Please enter valid From E-Mail Address";

        if ((txtTO.Text.Trim() != string.Empty || txtTO.CssClass == "input_mandatory") && !General.IsvalidEmail(txtTO.Text))
            ucError.ErrorMessage = "Please enter valid To E-Mail Address";

        return (!ucError.IsError);

    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    protected void BindBody()
    {
        DataSet ds = PhoenixPurchaseDelivery.ForwarderEmail(new Guid(ViewState["DELIVERYID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtTO.Text = dr["FLDFORWARDERMEAIL"].ToString();
            txtCC.Text = dr["FLDSUPTEMAIL"].ToString() + ',' + dr["FLDSUPPLIEREMAIL"].ToString();
            txtSubject.Text = dr["FLDTITLE"].ToString();
            StringBuilder sbemailbody = new StringBuilder();

            sbemailbody.AppendLine("This is an automated message. DO NOT REPLY to esmphoenix@executiveship.com. Kindly use the \"reply all\" button if you are responding to this message.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Dear Sir");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Please note, we are planning to activate the shipment of following orders with a total weight of " + dr["FLDTOTALWEIGHT"].ToString() + "Kg.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<table style=border-bottom-style:solid border=1><tr><td align=center>From No</td><td align=center>Title</td><td align=center>Vendor</td></tr>");
            foreach (DataRow dr1 in ds.Tables[1].Rows)
            {
                sbemailbody.AppendLine("<tr><td>" + dr1["FLDFORMNO"].ToString() + "</td><td>" + dr1["FLDTITLE"].ToString() + "</td><td>" + dr1["FLDNAME"].ToString() + "</td></tr>");
            }
            sbemailbody.AppendLine("</table>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(dr["FLDTITLE"].ToString());
            sbemailbody.AppendLine("<br/>");

            sbemailbody.AppendLine("Delivery Number: " + ds.Tables[0].Rows[0]["FLDDELIVERYNUMBER"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("VESSEL: " + ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("PORT:   " + ds.Tables[0].Rows[0]["FLDSEAPORTNAME"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("ETA:    " + General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDETA"].ToString()));
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("ETB:    " + General.GetDateTimeToString(ds.Tables[0].Rows[0]["FLDETB"].ToString()));
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("CONSIGNEE ADDRESS");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("==================");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(dr["FLDAGENTNAME"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(dr["FLDAGENTADDRESS"].ToString() + ",");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(dr["FLDAGENTCITY"].ToString() + " - " + dr["FLDAGENTPOSTALCODE"].ToString() + ",");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(dr["FLDAGENTCOUNTRYNAME"].ToString() + ".");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("PHONE " + dr["FLDAGENTPHONE"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("FAX " + dr["FLDAGENTFAX"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("EMAIL " + dr["FLDAGENTEMAIL"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");

            sbemailbody.AppendLine("Kindly advise " + dr["FLDSHIPMENTMODE"].ToString() + " cost and transit time in order to proceed further.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Please note shipment should reach on or before " + General.GetDateTimeToString(dr["FLDDELIVERYBY"].ToString()));
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Please confirm receipt of mail.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Thanks and Best Regards");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(PhoenixSecurityContext.CurrentSecurityContext.UserName);
            sbemailbody.AppendLine("<br/>");

            DataRow dr2 = ds.Tables[2].Rows[0];
            sbemailbody.AppendLine("For and on behalf of " + dr2["FLDCOMPANYNAME"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("(As Agents for Owners)");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(dr2["FLDCOMPANYADDRESS"].ToString() + ",");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(dr2["FLDCITYNAME"].ToString() + " - " + dr2["FLDPOSTALCODE"].ToString() + ",");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(dr2["FLDCOUNTRYNAME"].ToString() + ".");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("PHONE " + dr2["FLDTELEPHONENO"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("FAX " + dr2["FLDFAXNO"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("EMAIL " + dr2["FLDEMAILID"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Contact: " + dr["FLDUSEREMAIL"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("This is an automated message.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("If you need personal attention, use \"reply all\" to get your communication across to an email id that is monitored.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Please note esmphoenix@executiveship.com is NOT monitored.");
            sbemailbody.AppendLine("<br/>");
            txtBody.Content = sbemailbody.ToString();
        }
    }
}
