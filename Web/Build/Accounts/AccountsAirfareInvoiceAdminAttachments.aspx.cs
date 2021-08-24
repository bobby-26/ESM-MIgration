using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsAirfareInvoiceAdminAttachments : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"] != string.Empty)
            {
                ViewState["ID"] = Request.QueryString["ID"];
                InvoiceEdit();
            }
            
            if (Request.QueryString["attachmenttype"] != null && Request.QueryString["attachmenttype"] != string.Empty)
            {
                ViewState["ATTCHMENTTYPE"] = Request.QueryString["attachmenttype"].ToString();
                rblAttachmentType.Enabled = false;
            }
            else
                ViewState["ATTCHMENTTYPE"] ="Invoice"; 
            BindHard();
        }

        
        PhoenixToolbar toolbar = new PhoenixToolbar();
       
       
        toolbar.AddButton("Attachments", "ATTACHMENTS",ToolBarDirection.Right);
        toolbar.AddButton("History", "HISTORY", ToolBarDirection.Right);
        toolbar.AddButton("Airfare", "AIRFARE", ToolBarDirection.Right);
        MenuInvoice1.MenuList = toolbar.Show();

        MenuInvoice1.SelectedMenuIndex = 0;

     //   MenuInvoice1.SetTrigger(pnlInvoice);


        ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString();   
        

    }

    protected void BindHard()
    {
        rblAttachmentType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 93, 0, "INV,CDT,CRP"); 
        rblAttachmentType.DataTextField = "FLDHARDNAME";
        rblAttachmentType.DataValueField = "FLDHARDCODE";
        rblAttachmentType.DataBind();
        rblAttachmentType.SelectedIndex = 2;
        
    }

    protected void SetValue(object sender ,EventArgs e)
    {
       if (rblAttachmentType.SelectedIndex !=-1)
       ViewState["ATTCHMENTTYPE"] = rblAttachmentType.Items[rblAttachmentType.SelectedIndex].Text.Trim();

       ifMoreInfo.Attributes["src"] = "../Accounts/AccountsFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString();   
       
    }

    protected void Invoice_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;


        if (CommandName.ToUpper().Equals("AIRFARE") && ViewState["ID"] != null && ViewState["ID"].ToString() != string.Empty)
        {
            Response.Redirect("../Accounts/AccountsAirfareInvoiceAdminMaster.aspx?ID=" + ViewState["ID"].ToString());
        }
        else if (CommandName.ToUpper().Equals("HISTORY") && ViewState["ID"] != null && ViewState["ID"].ToString() != string.Empty)
        {
            Response.Redirect("../Accounts/AccountsAirfareInvoiceAdminHistory.aspx?ID=" + ViewState["ID"].ToString());
        }
        
    }

    protected void InvoiceEdit()
    {
        if (ViewState["ID"] != null)
        {
            DataSet ds = PhoenixAccountsAirfareInvoiceAdmin.AirfareInvoiceAdminEdit(General.GetNullableInteger((ViewState["ID"].ToString())));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ViewState["DTKey"] = dr["FLDDTKEY"].ToString();
            }
        }
    }

}
