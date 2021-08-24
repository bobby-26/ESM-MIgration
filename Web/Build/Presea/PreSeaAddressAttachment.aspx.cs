using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;
using SouthNests.Phoenix.Registers;

public partial class PreSeaAddressAttachment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["qfrom"] != null && Request.QueryString["qfrom"] != string.Empty)
        {
            toolbar.AddButton("Address", "ADDRESS");
        }

        toolbar.AddButton("Attachments", "ATTACHMENTS");

        MenuAddressAttachment.AccessRights = this.ViewState;
        MenuAddressAttachment.MenuList = toolbar.Show();
        MenuAddressAttachment.SetTrigger(pnlAddressAttachment);

        if (Request.QueryString["qfrom"] != null && Request.QueryString["qfrom"] != string.Empty)
        {
            MenuAddressAttachment.SelectedMenuIndex = 1;
        }
        else
        {
            MenuAddressAttachment.SelectedMenuIndex = 0;
        }

        if (!IsPostBack)
        {
            if (Request.QueryString["ADDRESSCODE"] != null && Request.QueryString["ADDRESSCODE"] != string.Empty)
            {
                DataSet ds = PhoenixPreSeaAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    long.Parse(Request.QueryString["ADDRESSCODE"].ToString()));

                ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"].ToString();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["DTKey"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                }
            }
            else
            {
                Response.Redirect("../PreSea/PreSeaOffice.aspx", true);
            }
            BindHard();

            rblAttachmentType.SelectedIndex = 0;
            SetValue(null, new EventArgs());
        }
    }

    protected void BindHard()
    {
        DataSet ds = PhoenixRegistersHard.ListHard(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, 179);

        rblAttachmentType.DataSource = ds;

        rblAttachmentType.DataTextField = "FLDHARDNAME";
        rblAttachmentType.DataValueField = "FLDHARDCODE";

        rblAttachmentType.DataBind();
    }

    protected void SetValue(object sender, EventArgs e)
    {
        if (rblAttachmentType.SelectedIndex != -1)
        {
            if (rblAttachmentType.SelectedValue == PhoenixCommonRegisters.GetHardCode(1, 179, "FBC"))
            {
                ViewState["ATTCHMENTTYPE"] = "FEEDBACKS";
            }
            else if (rblAttachmentType.SelectedValue == PhoenixCommonRegisters.GetHardCode(1, 179, "HSE"))
            {
                ViewState["ATTCHMENTTYPE"] = "HSE_QUESTIONNAIRE";
            }
            else if (rblAttachmentType.SelectedValue == PhoenixCommonRegisters.GetHardCode(1, 179, "PBP"))
            {
                ViewState["ATTCHMENTTYPE"] = "BROCHURE";
            }
        }
        else
        {
            ViewState["ATTCHMENTTYPE"] = "FEEDBACKS";
        }

        ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.PRESEA + "&type=" + ViewState["ATTCHMENTTYPE"].ToString();
    }

    protected void AddressAttachment_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("ADDRESS"))
        {
            Response.Redirect("../PreSea/PreSeaOffice.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"].ToString());
        }
    }
}
