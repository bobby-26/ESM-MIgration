using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsOwnerSOAConfirmationcheck : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Verify", "VERIFY",ToolBarDirection.Right);               
                MenuLineItem.AccessRights = this.ViewState;
                MenuLineItem.MenuList = toolbarmain.Show();
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                ViewState["URL"] = "";
                if (Request.QueryString["qDebitnoteid"] != null && Request.QueryString["qDebitnoteid"] != string.Empty)
                    ViewState["qDebitnoteid"] = Request.QueryString["qDebitnoteid"];
                if (Request.QueryString["IsPhoenixAttachment"] != null && Request.QueryString["IsPhoenixAttachment"]!= string.Empty)                
                    ViewState["IsPhoenixAttachment"] = Request.QueryString["IsPhoenixAttachment"].ToString();
                if (Request.QueryString["Filename"] != null && Request.QueryString["Filename"] != string.Empty)
                    ViewState["Filename"] = Request.QueryString["Filename"];
                if (Request.QueryString["FileDTKey"] != null && Request.QueryString["FileDTKey"] != string.Empty)
                    ViewState["FileDTKey"] = Request.QueryString["FileDTKey"];
                uservesselmap();
                DebitnoteEdit();
                AssignMismatchedFilename();
            }           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void DebitnoteEdit()
    {

        DataSet dsAttachment = new DataSet();
        if (ViewState["qDebitnoteid"] != null)
        {

            DataSet dsDebitnote = PhoenixAccountsDebitNoteReference.DebitNoteReferenceEdit(new Guid(ViewState["qDebitnoteid"].ToString()));
            if (dsDebitnote.Tables.Count > 0)
            {
                DataRow drDebitnote = dsDebitnote.Tables[0].Rows[0];               
                string dtKey = drDebitnote["FLDDTKEY"].ToString();
                BindAttachment(dtKey);
            }
          
        }
    }

    protected bool IsValidPost()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        
         Decimal? amount = General.GetNullableDecimal(txtAmount.TextWithLiterals);
        int? accountid = General.GetNullableInteger(ddlVesselAccount.SelectedValue);
        if (accountid == null )
            ucError.ErrorMessage = "Please select the Vessel Account";
        if (amount == null)
            ucError.ErrorMessage = "Please enter the amount"; 
        return (!ucError.IsError);
    }

    protected void BindAttachment(string dtkey)
    {
        DataSet dsattachment = new DataSet();

        int iRowCount = 0;
        int iTotalPageCount = 0;

        if (ViewState["IsPhoenixAttachment"].ToString() == "1")
        {
            dsattachment = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(dtkey), ViewState["Filename"].ToString(), null, null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            if (dsattachment.Tables[0].Rows.Count > 0)
            {
                DataRow drattachment = dsattachment.Tables[0].Rows[0];
                //string filepath = drattachment["FLDFILEPATH"].ToString();
                string src = "../common/download.aspx?dtkey=" + drattachment["FLDDTKEY"].ToString();
                ifMoreInfo.Attributes["src"] = src;// Session["sitepath"] + "/attachments/" + filepath;
            }
            else
            {
                ucError.ErrorMessage = "Invalid Attachment";
                ucError.Visible = true;
            }
        }
        else
        {
            //string Url = string.Empty;
            string src = string.Empty;
            DataSet dsDebitnote = PhoenixAccountsDebitNoteReference.DebitNoteReferenceEdit(new Guid(ViewState["qDebitnoteid"].ToString()));
            if (dsDebitnote.Tables.Count > 0)
            {
                DataRow drDebitnote = dsDebitnote.Tables[0].Rows[0];
                //Url = drDebitnote["FLDURL"].ToString();
                ViewState["URL"] = drDebitnote["FLDURL"].ToString();
                src = "../common/download.aspx?dtkey=" + drDebitnote["FLDDTKEY"].ToString();
            }
            ifMoreInfo.Attributes["src"] = src;
        }
    }    


    protected void MenuFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("VERIFY") && ViewState["qDebitnoteid"] != null && ViewState["qDebitnoteid"].ToString() != string.Empty)
            {
                
                if (!IsValidPost())
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixAccountsDebitNoteReference.DebitNoteAttachmentVerify(PhoenixSecurityContext.CurrentSecurityContext.UserCode,General.GetNullableGuid(ViewState["qDebitnoteid"].ToString()),
                    int.Parse(ddlVesselAccount.SelectedValue), ViewState["Filename"].ToString(), General.GetNullableString(ViewState["URL"].ToString()),decimal.Parse (txtAmount.Text));
                ucStatus.Text = "Attachment verified.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void uservesselmap()
    {

        ddlVesselAccount.DataSource = PhoenixAccountsOwnerStatementOfAccount.GetvesselAccountid(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlVesselAccount.DataTextField = "FLDDESCRIPTION";
        ddlVesselAccount.DataValueField = "FLDACCOUNTID";
        ddlVesselAccount.DataBind();
        ddlVesselAccount.Items.Insert(0, new RadComboBoxItem("--Office--", ""));
    }

    private void AssignMismatchedFilename()
    {
        if (ViewState["FileDTKey"] != null)
        {
            DataTable dt = PhoenixAccountsDebitNoteReference.DebitNoteReferenceAttachmentDetailsByDTKey(new Guid(ViewState["FileDTKey"].ToString()));
            if (dt.Rows.Count > 0)
            {
                ViewState["Filename"] = dt.Rows[0]["FLDFILENAME"].ToString();
            }
        }
    }

}
