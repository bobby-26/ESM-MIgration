using System;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaEmail : PhoenixBasePage
{
    string empid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Send", "SEND");
            toolbarmain.AddButton("Cancel", "CANCEL");
            EmailMenu.AccessRights = this.ViewState;
            EmailMenu.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                Session["AttachFiles"] = null;
                ViewState["mailsessionid"] = null;
                ViewState["mailsessionid"] = System.Guid.NewGuid().ToString();
                ViewState["addressnaildlist"] = "";

                if (Request.QueryString["type"] != null)
                {
                    if (Request.QueryString["type"] == "presentation")
                    {

                    }
                }
            }
            SetMailContent();
        }
        catch (Exception ex)
        {
            throw ex;

        }
    }

    protected void EmailMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SEND"))
            {
                if (!IsValidateEmail())
                {
                    ucError.Visible = true;
                    return;
                }

                SendMail();
            }
            else if (dce.CommandName.ToUpper().Equals("CANCEL"))
            {

            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SendMail()
    {

        try
        {
            PhoenixMail.SendMail(txtTO.Text.Trim(), txtCC.Text.Trim(), txtBCC.Text.Trim(), txtSubject.Text.Trim(), edtBody.Content, true, (MailPriority)Convert.ToInt32(ddlPriority.SelectedValue.ToString()), ViewState["mailsessionid"].ToString());
            ucStatus.Visible = true;
            ucStatus.Text = "Mail Sent";
        }
        catch (Exception ex)
        {
            throw ex.InnerException;
        }
    }

    protected void OpenAttachment(object sender, CommandEventArgs e)
    {
        try
        {
            string s = e.CommandArgument.ToString();
            string frameScript = "<script language='javascript'>" + "window.open('EmailAttachments/" + ViewState["mailsessionid"].ToString() + "/" + e.CommandArgument.ToString().Replace(",", "").Trim() + "');</script>";
            Response.Write(frameScript);
        }
        catch (Exception ex)
        {
            StringBuilder sbError = new StringBuilder();
            throw ex;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Session["AttachFiles"] != null)
        {
            lstAttachments.Items.Clear();
            DataTable dt = (DataTable)Session["AttachFiles"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAttachments.Items.Add(new ListItem(dt.Rows[i]["Text"].ToString(), dt.Rows[i]["Value"].ToString()));
            }
            Session["AttachFiles"] = null;
        }
    }

    private bool IsValidateEmail()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.IsvalidEmail(txtTO.Text))
        {
            ucError.ErrorMessage = "Please enter valid To E-Mail Address";
        }
        if (String.IsNullOrEmpty(txtSubject.Text.Trim()))
        {
            ucError.ErrorMessage = "Please enter Subject for the Mail";
        }
        if (String.IsNullOrEmpty(edtBody.Content.Trim()))
        {
            ucError.ErrorMessage = "Plase enter the Content.";
        }
        if (txtCC.Text.Trim() != string.Empty && !General.IsvalidEmail(txtTO.Text))
        {
            ucError.ErrorMessage = "Please enter valid Cc E-Mail Address";
        }
        if (txtBCC.Text.Trim() != string.Empty && !General.IsvalidEmail(txtBCC.Text))
        {
            ucError.ErrorMessage = "Please enter valid Bcc E-Mail Address";
        }
        return (!ucError.IsError);
    }

    private void SetMailContent()
    {
        string MailType = Request.QueryString["mailtype"];
        string MailCode = Request.QueryString["mailcode"];

        if (MailType == "presentation")
        {
            this.PresentationRequestMail();
        }
        else
            this.DefaultMailContent();
    }

    private void PresentationRequestMail()
    {

        try
        {
            if (Request.QueryString["addresscodelist"] != null)
            {
                string addresscodelist = Request.QueryString["addresscodelist"].ToString();
                string mailaddress = string.Empty;

                trMailFormat.Visible = true;

                PhoenixPreSeaAddress.PreSeaAddressMailListGet(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , addresscodelist
                    , ref mailaddress);

                ViewState["addressnaildlist"] = mailaddress;
                if (General.GetNullableString(ViewState["addressnaildlist"].ToString()) != null)
                    txtTO.Text = mailaddress;
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }
    private void DefaultMailContent()
    {
        txtTO.Text = String.Empty;
        txtSubject.Text = String.Empty;
        edtBody.Content = String.Empty;
    }
    protected void ucMail_TextChanged(object sender, EventArgs e)
    {
        DataTable dt = PhoenixPreSeaEmail.GetEmailTemplateMailContent(General.GetNullableInteger(ucMail.SelectedMailTemplate));

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            txtSubject.Text = dr["FLDSUBJECT"].ToString();
            edtBody.Content = dr["FLDEMAILCONTENT"].ToString();
        }
        else
        {
            txtSubject.Text = String.Empty;
            edtBody.Content = String.Empty;
        }

    }
}
