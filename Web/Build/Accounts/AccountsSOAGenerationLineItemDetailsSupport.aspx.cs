using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using System.Web.UI;

public partial class AccountsSOAGenerationLineItemDetailsSupport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["RECORDNUMBER"] = General.ShowRecords(null);
                ViewState["debitnotereference"] = Request.QueryString["debitnotereference"].ToString();

                if (General.GetNullableGuid(Request.QueryString["debitnoteid"].ToString()) != null)
                    ViewState["debitnotereferenceid"] = Request.QueryString["debitnoteid"].ToString();
                else
                    ViewState["debitnotereferenceid"] = "";

                ViewState["accountid"] = Request.QueryString["accountid"].ToString();
                ViewState["Ownerid"] = Request.QueryString["Ownerid"].ToString();
                ViewState["DtKey"] = Request.QueryString["dtkey"].ToString();
                ViewState["VoucherDtKey"] = Request.QueryString["voucherdtkey"].ToString();
                ViewState["voucherdate"] = "";

                if (ViewState["VoucherDtKey"].ToString() == "")
                    ViewState["VoucherDtKey"] = ViewState["DtKey"].ToString(); 
            }           
            BindData();
            BindAttachmentLink(ViewState["DtKey"].ToString());
            BindVoucherLevelAttachmentLink(ViewState["VoucherDtKey"].ToString());
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
       // int count = 0;
       // int total;

 
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixAccountsSoaChecking.SoaCheckingVoucherEdit(ViewState["debitnotereference"].ToString() 
                                                    , int.Parse(ViewState["Ownerid"].ToString())
                                                    , Filter.CurrentOwnerBudgetCodeSelection
                                                    , int.Parse(ViewState["accountid"].ToString())
                                                    ,new Guid(ViewState["debitnotereferenceid"].ToString())
                                                    , new Guid(ViewState["DtKey"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            lblAccountCOde.Text = ds.Tables[0].Rows[0]["FLDACCOUNTCODE"].ToString();
            lblAccountId.Text = ds.Tables[0].Rows[0]["FLDACCOUNTID"].ToString();
            lblAccountCOdeDescription.Text = ds.Tables[0].Rows[0]["FLDACCOUNTCODEDESC"].ToString();
            lnkOwnerBudgetCode.Text = ds.Tables[0].Rows[0]["FLDOWNERBUDGETCODE"].ToString();
            Filter.CurrentOwnerBudgetCodeSelection = ds.Tables[0].Rows[0]["FLDOWNERBUDGETCODE"].ToString();
            lblBudgetCodeDescription.Text = ds.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString();
            lblStatementReference.Text = ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString();
            ViewState["accountid"] = ds.Tables[0].Rows[0]["FLDACCOUNTID"].ToString();
            ViewState["FLDVOUCHERDETAILID"] = ds.Tables[0].Rows[0]["FLDVOUCHERDETAILID"].ToString();
            ViewState["voucherdate"] = ds.Tables[0].Rows[0]["FLDVOUCHERDATE"].ToString();
            lnkOwnerBudgetCode.Attributes.Add("onclick", "javascript:parent.openNewWindow('Filter','','"+Session["sitepath"]+"/Accounts/AccountsSoaCheckingBudgetSearch.aspx?accountid=" + lblAccountId.Text + "&accountcode=" + lblAccountCOde.Text + "&debitnoteref=" + lblStatementReference.Text + "&ownerid=" + ViewState["Ownerid"].ToString() + "&voucherdate=" + ViewState["voucherdate"].ToString() + "'); return false;");
            BindAttachment(ViewState["DtKey"].ToString(), ViewState["VoucherDtKey"].ToString());
            lblESMBudgetcodedescription.Text = ds.Tables[0].Rows[0]["FLDESMBUDGETCODE"].ToString();           
        }
        else
        {
            DataTable dt = ds.Tables[0];
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;       
    }

    protected void BindAttachment(string dtkey, string voucherleveldtkey)
    {
        DataSet dsattachment = new DataSet();
        DataSet dsvoucherlevelattachment = new DataSet();

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string src = string.Empty;

        dsvoucherlevelattachment = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearch(new Guid(voucherleveldtkey), null, null, null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
        dsattachment = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearch(new Guid(dtkey), null, null, null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (dsvoucherlevelattachment.Tables[0].Rows.Count > 0)
        {
            DataRow drvoucherlevelattachment = dsvoucherlevelattachment.Tables[0].Rows[0];
            string filepath = drvoucherlevelattachment["FLDFILEPATH"].ToString();
            src = "../common/download.aspx?dtkey=" + drvoucherlevelattachment["FLDDTKEY"].ToString();
            ifMoreInfo.Attributes["src"] = src; // Session["sitepath"] + "/attachments/" + filepath;

            //ifMoreInfo.Attributes["src"] = Session["sitepath"] + "/attachments/" + filepath;
        }
        else if (dsattachment.Tables[0].Rows.Count > 0)
        {
            DataRow drattachment = dsattachment.Tables[0].Rows[0];
            string filepath = drattachment["FLDFILEPATH"].ToString();

            src = "../common/download.aspx?dtkey=" + drattachment["FLDDTKEY"].ToString();
            ifMoreInfo.Attributes["src"] = src; // Session["sitepath"] + "/attachments/" + filepath;// +"#page=" + ViewState["INVOICEBANKINFOPAGENUMBER"];

            //ifMoreInfo.Attributes["src"] = Session["sitepath"] + "/attachments/" + filepath;// +"#page=" + ViewState["INVOICEBANKINFOPAGENUMBER"];
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //BindData();
    }

    // Attachment link..
    protected void dlAttachmentLink_RowCommand(object sender, DataListCommandEventArgs e)
    {
        DataList _gridView = (DataList)sender;

        //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        string src = string.Empty;

        ifMoreInfo.Attributes["src"] = null;

        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
             src = "../common/download.aspx?dtkey=" + e.CommandArgument.ToString();
            ifMoreInfo.Attributes["src"] = src;//PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + filepath;//Session["sitepath"] + "/attachments/" + filepath;
           
        }
    }

    protected void VoucherLevelAttachmentLink_RowCommand(object sender, DataListCommandEventArgs e)
    {
        DataList _gridView = (DataList)sender;

        //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        ifMoreInfo.Attributes["src"] = null;
        string src = string.Empty;
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            //string filepath = e.CommandArgument.ToString();
            src = "../common/download.aspx?dtkey=" + e.CommandArgument.ToString();
            ifMoreInfo.Attributes["src"] = src; // PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + filepath;//Session["sitepath"] + "/attachments/" + filepath;
        }
    }

    private void BindAttachmentLink(string dtkey)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearch(
            new Guid(dtkey), null, null, null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            dlAttachmentLink.DataSource = ds;
            dlAttachmentLink.DataBind();
        }
    }

    private void BindVoucherLevelAttachmentLink(string dtkey)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = PhoenixCommonFileAttachment.AttachmentSOACheckingVoucherSearch(
            new Guid(dtkey), null, null, null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            VoucherLevelAttachmentLink.DataSource = ds;
            VoucherLevelAttachmentLink.DataBind();
        }
    }
}
