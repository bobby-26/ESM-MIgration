using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersAddressQuestion : System.Web.UI.Page
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {                
                ViewState["ADDRESSCODE"] = Request.QueryString["ADDRESSCODE"];
                
                if (Request.QueryString["VIEWONLY"] == null)
                {
                    PhoenixToolbar toolbar = new PhoenixToolbar();
                    PhoenixToolbar toolbarAddress = new PhoenixToolbar();
                    toolbarAddress.AddButton("Address", "ADDRESS", ToolBarDirection.Left);
                    toolbarAddress.AddButton("Bank", "BANK", ToolBarDirection.Left);
                    toolbarAddress.AddButton("Question", "QUESTION", ToolBarDirection.Left);
                    toolbarAddress.AddButton("Attachments", "ATTACHMENTS", ToolBarDirection.Left);
                    toolbarAddress.AddButton("Agreements", "AGREEMENTSATTACHMENT", ToolBarDirection.Left);
                    toolbarAddress.AddButton("Address Correction", "CORRECTION", ToolBarDirection.Left);
                    toolbarAddress.AddButton("Contacts", "CONTACTS", ToolBarDirection.Left);

                    toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                    MenuQuestion.AccessRights = this.ViewState;
                    MenuQuestion.MenuList = toolbar.Show();
                    MenuAddressMain.AccessRights = this.ViewState;
                    MenuAddressMain.MenuList = toolbarAddress.Show();
                    MenuAddressMain.SelectedMenuIndex = 2;
                   // MenuAddressMain.SetTrigger(pnlAddressEntry);
                }

                BindQuestionData();
            
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindQuestionData()
    {
      
        DataSet dsaddress = PhoenixRegistersAddress.ListAddress(ViewState["ADDRESSCODE"].ToString());

        if (dsaddress.Tables[0].Rows.Count > 0)
        {
            DataRow draddress = dsaddress.Tables[0].Rows[0]; 
            txtProposedBy.Text = draddress["FLDPROPOSEDBY"].ToString();
            txtIntroducingReason.Text = draddress["FLDINTRODUCINGREASON"].ToString();
            txtOtherAlternative.Text = draddress["FLDOTHERALTERNATIVES"].ToString();
            txtRiskAssociated.Text = draddress["FLDRISKASSOCIATED"].ToString();
            txtSuperintendentRemarks.Text = draddress["FLDSUPERINTENDENTREMARKS"].ToString();
        }
    }

    protected void AddressMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("QUESTION"))
            {
                Response.Redirect("../Registers/RegistersAddressQuestion.aspx?addresscode=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("ADDRESS"))
            {
                Response.Redirect("../Registers/RegistersOffice.aspx?addresscode=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
            }
            if (CommandName.ToUpper().Equals("BANK"))
            {
                Response.Redirect("../Registers/RegistersReadOnlyBankInformationtionList.aspx?addresscode=" + ViewState["ADDRESSCODE"] + "&qfrom=ADDRESS");
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

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuQuestion_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                PhoenixRegistersQuestion.UpdateQuestionInformation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Convert.ToInt64(ViewState["ADDRESSCODE"]), General.GetNullableString(txtProposedBy.Text), 
                    General.GetNullableString(txtIntroducingReason.Text),
                    General.GetNullableString(txtOtherAlternative.Text), General.GetNullableString(txtRiskAssociated.Text), 
                    General.GetNullableString(txtSuperintendentRemarks.Text));    
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
