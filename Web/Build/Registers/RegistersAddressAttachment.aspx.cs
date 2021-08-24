using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersAddressAttachment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();

        //if (Request.QueryString["qfrom"] != null && Request.QueryString["qfrom"] != string.Empty)
        //{
        //    toolbar.AddButton("Address", "ADDRESS");
        //}

       // toolbar.AddButton("Attachments", "ATTACHMENTS");
        toolbar.AddButton("Address", "ADDRESS", ToolBarDirection.Left);
        toolbar.AddButton("Bank", "BANK", ToolBarDirection.Left);
        toolbar.AddButton("Question", "QUESTION", ToolBarDirection.Left);
        toolbar.AddButton("Attachments", "ATTACHMENTS", ToolBarDirection.Left);
        toolbar.AddButton("Agreements", "AGREEMENTSATTACHMENT", ToolBarDirection.Left);
        toolbar.AddButton("Address Correction", "CORRECTION", ToolBarDirection.Left);
        toolbar.AddButton("Contacts", "CONTACTS", ToolBarDirection.Left);

        MenuAddressAttachment.AccessRights = this.ViewState;
        MenuAddressAttachment.MenuList = toolbar.Show();
        // MenuAddressAttachment.SetTrigger(pnlAddressAttachment);
        MenuAddressAttachment.SelectedMenuIndex = 3;
        if (!IsPostBack)
        {
            if (Request.QueryString["ADDRESSCODE"] != null && Request.QueryString["ADDRESSCODE"] != string.Empty)
            {
                DataSet ds = PhoenixRegistersAddress.EditAddress(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    long.Parse(Request.QueryString["ADDRESSCODE"].ToString()));

                ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"].ToString();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    ViewState["DTKey"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                }
            }
            else
            {
                Response.Redirect("../Registers/RegistersOffice.aspx", true);
            }
             
            //if (Request.QueryString["qfrom"] != null && Request.QueryString["qfrom"] != string.Empty)
            //{
            //    MenuAddressAttachment.SelectedMenuIndex = 1;
            //}
            //else
            //{
            //    MenuAddressAttachment.SelectedMenuIndex = 0;
            //}

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

        ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.REGISTERS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString();
    }

    protected void AddressAttachment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("ADDRESS"))
        {
            Response.Redirect("../Registers/RegistersOffice.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
        }
        if (CommandName.ToUpper().Equals("BANK"))
        {
            Response.Redirect("../Registers/RegistersBankInformationList.aspx?addresscode=" + ViewState["ADDRESSCODE"] + "&toolbar=");
        }
        if (CommandName.ToUpper().Equals("QUESTION"))
        {
            Response.Redirect("../Registers/RegistersAddressQuestion.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
        }
        if (CommandName.ToUpper().Equals("ATTACHMENTS"))
        {
            Response.Redirect("../Registers/RegistersAddressAttachment.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
        }
        if (CommandName.ToUpper().Equals("AGREEMENTSATTACHMENT"))
        {
            Response.Redirect("../Registers/RegistersAgreementsAttachment.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
        }
        if (CommandName.ToUpper().Equals("CORRECTION"))
        {
            Response.Redirect("../Registers/RegistersAddressCorrection.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
        }
        if (CommandName.ToUpper().Equals("OFFICEADMIN"))
        {
            Response.Redirect("../Registers/RegistersSupplierInvoiceApprovalUserMap.aspx?ADDRESSCODE=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
        }
        if (CommandName.ToUpper().Equals("CONTACTS"))
        {
            Response.Redirect("../Registers/RegistersAddressPurpose.aspx?addresscode=" + ViewState["ADDRESSCODE"]);
        }
    }
}
