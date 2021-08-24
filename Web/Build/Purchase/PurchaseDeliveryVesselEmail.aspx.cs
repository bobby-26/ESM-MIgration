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

public partial class PurchaseDeliveryVesselEmail : PhoenixBasePage
{


    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Discard", "DISCARD", ToolBarDirection.Right);
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

                PhoenixMail.SendMail(txtTO.Text, txtCC.Text, null, txtSubject.Text, txtBody.Content
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
        DataSet ds = PhoenixPurchaseDelivery.VesselEmail(new Guid(ViewState["DELIVERYID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            txtTO.Text = dr["FLDVESSELEMAIL"].ToString();
            txtCC.Text = dr["FLDSUPTEMAIL"].ToString() + ',' + dr["FLDSUPPLIEREMAIL"].ToString() + ',' + dr["FLDUSEREMAIL"].ToString();
            txtSubject.Text = dr["FLDTITLE"].ToString();

            StringBuilder sbemailbody = new StringBuilder();

            sbemailbody.Append("This is an automated message. DO NOT REPLY to esmphoenix@executiveship.com. Kindly use the \"reply all\" button if you are responding to this message.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("Dear Sir,");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Good Day !!!");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("We have arranged following Requisitions at " + dr["FLDSEAPORTNAME"].ToString() + " Port.");
            sbemailbody.AppendLine("<br/>");

            if (ds.Tables[1].Rows.Count > 0)
            {
                sbemailbody.AppendLine("FOLLOWING REQNS FROM " + dr["FLDFORWARDERNAME"].ToString() + " AWB  NO : " + dr["FLDHAWB"].ToString());
                sbemailbody.AppendLine("<br/>");
                sbemailbody.AppendLine("------------------------------------------------------------------------------------------------------");
                sbemailbody.AppendLine("<br/>");

                foreach (DataRow dr1 in ds.Tables[1].Rows)
                {
                    sbemailbody.AppendLine(dr1["FLDFORMNO"].ToString() + "      " + dr1["FLDTITLE"].ToString());
                    sbemailbody.AppendLine("<br/>");
                }
                sbemailbody.AppendLine("<br/>");
                sbemailbody.AppendLine("<br/>");
            }

            if (ds.Tables[2].Rows.Count > 0)
            {
                sbemailbody.Append("We have arranged following Store Requisitions at " + dr["FLDSEAPORTNAME"].ToString() + " Port.");
                sbemailbody.AppendLine("<br/>");

                foreach (DataRow dr2 in ds.Tables[2].Rows)
                {
                    sbemailbody.AppendLine(dr2["FLDFORMNO"].ToString() + "      " + dr2["FLDTITLE"].ToString());
                    sbemailbody.AppendLine("<br/>");
                }
                sbemailbody.AppendLine("<br/>");
                sbemailbody.AppendLine("<br/>");
            }

            if (ds.Tables[3].Rows.Count > 0)
            {
                sbemailbody.Append("We have arranged following Service Requisitions at " + dr["FLDSEAPORTNAME"].ToString() + " Port.");
                sbemailbody.AppendLine("<br/>");

                foreach (DataRow dr3 in ds.Tables[3].Rows)
                {
                    sbemailbody.AppendLine(dr3["FLDFORMNO"].ToString() + "      " + dr3["FLDTITLE"].ToString());
                    sbemailbody.AppendLine("<br/>");
                }
                sbemailbody.AppendLine("<br/>");
                sbemailbody.AppendLine("<br/>");
            }

            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");

            sbemailbody.AppendLine("Kindly liaise with below agent for further arrangement");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Agent Details:");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("---------------");
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

            sbemailbody.AppendLine("If there are any additional orders we will let you know.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Please confirm receipt and revert with arrangements.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("Prior vessels departure please confirm that the above listed items have been received onboard.In case any items are undelivered pleases inform us via return email so that we can take the necessary action.");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.Append("Thanks and Best Regards");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(PhoenixSecurityContext.CurrentSecurityContext.UserName);
            sbemailbody.AppendLine("<br/>");
            DataRow dr4 = ds.Tables[4].Rows[0];
            sbemailbody.AppendLine("For and on behalf of " + dr4["FLDCOMPANYNAME"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("(As Agents for Owners)");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(dr4["FLDCOMPANYADDRESS"].ToString() + ",");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(dr4["FLDCITYNAME"].ToString() + " - " + dr4["FLDPOSTALCODE"].ToString() + ",");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine(dr4["FLDCOUNTRYNAME"].ToString() + ".");
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("PHONE " + dr4["FLDTELEPHONENO"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("FAX " + dr4["FLDFAXNO"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("EMAIL " + dr["FLDUSEREMAIL"].ToString());
            sbemailbody.AppendLine("<br/>");
            sbemailbody.AppendLine("<br/>");
            txtBody.Content = sbemailbody.ToString();
        }
    }
}
