using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using System.Web.UI;

public partial class AccountsSoaGenerationLineItemDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarlineitem = new PhoenixToolbar();
            toolbarlineitem.AddButton("BUDGET", "BUDGET");
            //toolbarlineitem.AddButton("BUDGET", "BUDGET");
            MenuSOALineItems.AccessRights = this.ViewState;
            MenuSOALineItems.MenuList = toolbarlineitem.Show();

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
            if(!string.IsNullOrEmpty(ViewState["DtKey"].ToString()))
                BindAttachmentLink(ViewState["DtKey"].ToString());
            if (!string.IsNullOrEmpty(ViewState["VoucherDtKey"].ToString()))
                BindVoucherLevelAttachmentLink(ViewState["VoucherDtKey"].ToString());
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGenralSub_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSOALineItems_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("BUDGET"))
            {
                Response.Redirect("AccountsSoaCheckingBudgetSearch.aspx?accountid=" + lblAccountId.Text + "&accountcode=" + lblAccountCOde.Text + "&debitnoteref=" + lblStatementReference.Text + "&ownerid=" + ViewState["Ownerid"].ToString() + "&voucherdate=" + ViewState["voucherdate"].ToString(), true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        try
        {
            if (dce.CommandName.ToUpper().Equals("SOA"))
            {
                Response.Redirect("../Accounts/AccountsSoaChecking.aspx", true);
            }
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

 
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixAccountsSoaChecking.SoaCheckingVoucherList(ViewState["debitnotereference"].ToString()
                                                    , int.Parse(ViewState["Ownerid"].ToString())
                                                    , Filter.CurrentOwnerBudgetCodeSelection
                                                    , int.Parse(ViewState["accountid"].ToString())
                                                    , (int)ViewState["PAGENUMBER"]
                                                    , (int)ViewState["RECORDNUMBER"]
                                                    , ref iRowCount
                                                    , ref iTotalPageCount
                                                    ,new Guid(ViewState["debitnotereferenceid"].ToString()));

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
            //lnkOwnerBudgetCode.Attributes.Add("onclick", "javascript:parent.Openpopup('Filter','','AccountsSoaCheckingBudgetSearch.aspx?accountid=" + lblAccountId.Text + "&accountcode=" + lblAccountCOde.Text + "&debitnoteref=" + lblStatementReference.Text + "&ownerid=" + ViewState["Ownerid"].ToString() + "&voucherdate=" + ViewState["voucherdate"].ToString() + "'); return false;");
            BindAttachment(ViewState["DtKey"].ToString(), ViewState["VoucherDtKey"].ToString());
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

        if(!string.IsNullOrEmpty(voucherleveldtkey))
            dsvoucherlevelattachment = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(voucherleveldtkey), null, null, null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (!string.IsNullOrEmpty(dtkey))
            dsattachment = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(dtkey), null, null, null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (dsvoucherlevelattachment.Tables.Count>0 && dsvoucherlevelattachment.Tables[0].Rows.Count > 0)
        {
            DataRow drvoucherlevelattachment = dsvoucherlevelattachment.Tables[0].Rows[0];
            string src = "../common/download.aspx?dtkey=" + drvoucherlevelattachment["FLDDTKEY"].ToString();
            ifMoreInfo.Attributes["src"] = src;

            //string filepath = drvoucherlevelattachment["FLDFILEPATH"].ToString();
            //ifMoreInfo.Attributes["src"] = Session["sitepath"] + "/attachments/" + filepath;
        }
        else if (dsattachment.Tables.Count>0 && dsattachment.Tables[0].Rows.Count > 0)
        {
            DataRow drattachment = dsattachment.Tables[0].Rows[0];

            string src = "../common/download.aspx?dtkey=" + drattachment["FLDDTKEY"].ToString();
            ifMoreInfo.Attributes["src"] = src;

            //string filepath = drattachment["FLDFILEPATH"].ToString();
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

        ifMoreInfo.Attributes["src"] = null;

        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            string src = "../common/download.aspx?dtkey=" + e.CommandArgument.ToString();
            ifMoreInfo.Attributes["src"] = src; //Session["sitepath"] + "/attachments/" + filepath;

            //string filepath = e.CommandArgument.ToString();
            //ifMoreInfo.Attributes["src"] = Session["sitepath"] + "/attachments/" + filepath;
        }
    }

    protected void VoucherLevelAttachmentLink_RowCommand(object sender, DataListCommandEventArgs e)
    {
        DataList _gridView = (DataList)sender;

        //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

        ifMoreInfo.Attributes["src"] = null;

        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            string src = "../common/download.aspx?dtkey=" + e.CommandArgument.ToString();
            ifMoreInfo.Attributes["src"] = src; // Session["sitepath"] + "/attachments/" + filepath;

            //string filepath = e.CommandArgument.ToString();
            //ifMoreInfo.Attributes["src"] = Session["sitepath"] + "/attachments/" + filepath;
        }
    }

    private void BindAttachmentLink(string dtkey)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = PhoenixCommonFileAttachment.AttachmentSearch(
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

        DataSet ds = PhoenixCommonFileAttachment.AttachmentSearch(
            new Guid(dtkey), null, null, null, null, 1, General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            VoucherLevelAttachmentLink.DataSource = ds;
            VoucherLevelAttachmentLink.DataBind();
        }
    }
}
