using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewManagement;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewApprovedPDAttachment : PhoenixBasePage
{
    PhoenixModule mod;
    string strDocumentId = string.Empty;
    string strApprovalType = string.Empty;
    string csvUserCode = string.Empty;
    string strVesselId = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if(Request.QueryString["mod"] != null)
            mod = (PhoenixModule)Enum.Parse(typeof(PhoenixModule), Request.QueryString["mod"]);
        if (Request.QueryString["type"] != null)
            strApprovalType = Request.QueryString["type"];
        if (Request.QueryString["docid"] != null)
            strDocumentId = Request.QueryString["docid"];
        if (Request.QueryString["vslid"] != null)
            strVesselId = Request.QueryString["vslid"];
        if (Request.QueryString["user"] != null)
            csvUserCode = Request.QueryString["user"];

        if (!IsPostBack)
        {
            BindHard();
            rblAttachmentType.SelectedIndex = 1;
            ViewState["ATTCHMENTTYPE"] = rblAttachmentType.SelectedValue;
            ViewState["ALLAPPROVED"] = 0;
            ViewState["ACTIVEYN"] = 0;
            DataTable dt = PhoenixCrewPlanning.EditCrewPlan(new Guid(Request.QueryString["dtkey"].ToString()));
            if (dt.Rows.Count > 0)
            {
                ViewState["CDTKEY"] = dt.Rows[0]["FLDDTKEY"].ToString();
                ViewState["ACTIVEYN"] = dt.Rows[0]["FLDACTIVEYN"].ToString();
            }
            
            ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?dtkey="
                + ViewState["CDTKEY"].ToString()
                + "&MOD=" + PhoenixModule.CREW
                + "&type=" + ViewState["ATTCHMENTTYPE"].ToString()
                + "&cmdname=PDUPLOAD"
                + (ViewState["ACTIVEYN"].ToString() == "0" ? string.Empty + "&pdapprovalyn=1" : string.Empty + "&pdapprovalyn=1");
     

            dt = PhoenixCommonApproval.GetOwnerApprovalConfig(int.Parse(strVesselId));
			if (dt.Rows.Count > 0)
			{
				if (!dt.Rows[0]["FLDAPPROVALYN"].ToString().Equals("1"))
					rblAttachmentType.Enabled = false;
			}
			else
				rblAttachmentType.Enabled = false;

        }
     

        if (Request.QueryString["proceed"] != null)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Proceed", "PROCEED",ToolBarDirection.Right);
            MenuPDAttachment.AccessRights = this.ViewState;
            MenuPDAttachment.MenuList = toolbar.Show();

            SetData();
            if (rblAttachmentType.SelectedValue == PhoenixCommonRegisters.GetHardCode(1, 154, "APD"))
                lblNote.Text = "Important Note: After uploading Approved PD, Please Click proceed to confirm approval status and further processing of seafarer.";
            else
                lblNote.Text = "Important Note: After uploading Approved Owner Document, Please Click proceed to confirm approval status and further processing of seafarer.";
            lblNote.Visible = true;
        }
    }

    protected void BindHard()
    {
        rblAttachmentType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 154);
        rblAttachmentType.DataTextField = "FLDHARDNAME";
        rblAttachmentType.DataValueField = "FLDHARDCODE";
        rblAttachmentType.DataBind();
    }

    protected void SetValue(object sender, EventArgs e)
    {
        if (rblAttachmentType.SelectedIndex != -1)
        {
            ViewState["ATTCHMENTTYPE"] = rblAttachmentType.SelectedValue;                 
        }

        ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?dtkey=" 
            + ViewState["CDTKEY"].ToString() 
            + "&MOD=" + PhoenixModule.CREW 
            + "&type=" + ViewState["ATTCHMENTTYPE"].ToString()
            + "&cmdname=PDUPLOAD"
            + (ViewState["ACTIVEYN"].ToString() == "0" ? string.Empty + "&pdapprovalyn=1" : string.Empty + "&pdapprovalyn=1");
    }
    protected void PDAttachment_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("PROCEED"))
        {
            if (mod == PhoenixModule.CREW)
            {
                byte bAllApproved = 0;
                DataTable dt = PhoenixCommonApproval.ListApprovalRecord(strDocumentId, int.Parse(strApprovalType), csvUserCode, General.GetNullableInteger(strVesselId), 0, ref bAllApproved);

                ucConfirmCrew.Visible = true;
                ucConfirmCrew.Text = "Please Note: <br/> <ul>" + (Request.QueryString["newapp"] != null && bAllApproved == 1 ? "<li>Officer will be moved to Personnel Master</li>" : string.Empty) +
                    "<li>Officer will be moved to Crew Change Plan</li>" + (ViewState["ALLAPPROVED"].ToString() == "0" ? "<li>Officer should be fully approved prior generating Travel Request</li></ul>" : "</ul>");
                ((RadTextBox)ucConfirmCrew.FindControl("txtRemarks")).Focus();                
            }
        }            
    }
    protected void btnCrewApprove_Click(object sender, EventArgs e)
    {
        try
        {
            UserControlConfirmMessageCrew ucCM = sender as UserControlConfirmMessageCrew;
            if (ucCM.confirmboxvalue == 1)
            {
                if (mod == PhoenixModule.CREW)
                {
                    RadTextBox txtrem = (RadTextBox)ucConfirmCrew.FindControl("txtRemarks");
                    if (txtrem.Text == "")
                    {
                        if (!IsValidateRemarks(txtrem.Text))
                        {
                            ucError.Visible = true;
                            return;
                        }
                    }
                    PhoenixCommonApproval.Proceed(mod, int.Parse(strApprovalType), strDocumentId, csvUserCode, ucCM.RequestList, txtrem.Text);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "fnReloadList('approval','ifMoreInfo'," + (ViewState["ALLAPPROVED"].ToString() == "1" ? ("null") : ("'keepopen'")) + ");", true);                    
                }

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidateRemarks(string remarks)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(remarks))
            ucError.ErrorMessage = "Remarks is required for proceeding.";
        return (!ucError.IsError);
    }
    private void SetData()
    {
        byte bAllApproved = 0;
        DataTable dt = PhoenixCommonApproval.ListApprovalRecord(strDocumentId, int.Parse(strApprovalType), csvUserCode, General.GetNullableInteger(strVesselId), 1, ref bAllApproved);        
        ViewState["ALLAPPROVED"] = bAllApproved;       
    }
}
