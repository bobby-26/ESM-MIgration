using iTextSharp.text.pdf;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Accounts_AccountsDashboardDebitReferenceUpdateSOAGeneration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Update", "UPDATE", ToolBarDirection.Right);

            MenuDebitReference.AccessRights = this.ViewState;
            MenuDebitReference.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("SOA", "SOAREFERENCE");
            toolbarsub.AddButton("UnBilled to Billed", "BILLING");

            MenuGenralSub.AccessRights = this.ViewState;
            MenuGenralSub.MenuList = toolbarsub.Show();
            MenuGenralSub.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["type"] = "";
                ViewState["status"] = "";
                ViewState["month"] = "";
                ViewState["year"] = "";

                if (Request.QueryString["type"] != null)
                    ViewState["type"] = Request.QueryString["type"].ToString();

                if (Request.QueryString["status"] != null)
                    ViewState["status"] = Request.QueryString["status"].ToString();

                if (Request.QueryString["month"] != null)
                    ViewState["month"] = Request.QueryString["month"].ToString();

                if (Request.QueryString["year"] != null)
                    ViewState["year"] = Request.QueryString["year"].ToString();

                ViewState["PAGENUMBER"] = 1;
                cmdHiddenPick.Attributes.Add("style", "display:none;");

                if (Request.QueryString["debitnotereferenceID"] != null)
                {
                    ViewState["debitnotereferenceID"] = Request.QueryString["debitnotereferenceID"];
                }

                BindSOADetails(new Guid(ViewState["debitnotereferenceID"].ToString()));
                lblAttcahMsg.Text = string.Empty;
                lblcaption.Visible = false;
                ViewState["output_filename"] = "";
                ViewState["output_vouchernumber"] = "";
                ViewState["output_error"] = "";
            }

            imgShowAccount.Attributes.Add("onclick", "return showPickList('spnPickListCreditAccount', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCompanyAccount.aspx', true); ");
            imgShowAccount.Attributes.Add("style", "Visibility:hidden");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Filter.CurrentPickListSelection == null)
            return;

        BindDebitReference();

        if (IsValidData())
        {
            BindAttachments();
        }
    }

    public void BindDebitReference()
    {
        DataSet ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceList(txtAccountCOde.Text);

        ddlDebitReference.DataTextField = "FLDDEBITNOTEREFERENCE";
        ddlDebitReference.DataValueField = "FLDDEBINOTEREFERENCEID";
        ddlDebitReference.DataSource = ds;
        ddlDebitReference.DataBind();
    }

    protected void MenuDebitReference_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper() == "UPDATE")
        {
            if (!IsValidData())
            {
                ucError.Visible = true;
                return;
            }
            BindData();
        }

    }
    public void BindData()
    {
        DataTable dtPDF = new DataTable();
        dtPDF.Columns.Add("FLDVOUCHERNUMBER");
        dtPDF.Columns.Add("FLDFILENAME");
        dtPDF.Columns.Add("FLDMSG");
        try
        {
            ViewState["output_error"] = "";

            if (!IsValidData())
            {
                ucError.Visible = true;
                return;
            }


            int iRowCount = 0;
            int iTotalPageCount = 0;


            DataTable dt = PhoenixAccountsDebitNoteReferenceAttachments.DebitNoteNonPDFVoucherList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , General.GetNullableGuid(ddlDebitReference.SelectedValue)
                            , txtAccountCOde.Text
                            , General.GetNullableDateTime(ucFromDate.Text)
                            , General.GetNullableDateTime(ucToDate.Text)
                              , (int)ViewState["PAGENUMBER"]
                          , gvFormDetails.PageSize
                          , ref iRowCount
                          , ref iTotalPageCount);



            if (dt.Rows.Count > 0)
            {

                gvFormDetails.DataSource = dt;
                //gvFormDetails.VirtualItemCount = iRowCount;
                //divPage.Visible = true;
                ViewState["ROWCOUNT"] = iRowCount;
                ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
                gvFormDetails.Rebind();
                lblAttcahMsg.Text = "* Cannot bill vouchers. Below vouchers are having non- standard PDF. Please replace PDF Files";
                lblcaption.Visible = true;
            }
            else
            {
                DataTable dtAtt = PhoenixAccountsDebitNoteReference.DebitNoteUnBilltoBilledAttachmentList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                              , General.GetNullableGuid(ddlDebitReference.SelectedValue)
                              , txtAccountCOde.Text
                              , General.GetNullableDateTime(ucFromDate.Text)
                              , General.GetNullableDateTime(ucToDate.Text));
                if (dtAtt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAtt.Rows)
                    {
                        ViewState["output_filename"] = dr["FLDFILENAME"].ToString();
                        ViewState["output_vouchernumber"] = dr["FLDVOUCHERNUMBER"].ToString();
                        string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                        if (!path.EndsWith("/"))
                            path = path + "/";
                        CheckPdfProtection(path + dr["FLDFILEPATH"].ToString(), dr["FLDVOUCHERNUMBER"].ToString(), dr["FLDFILENAME"].ToString(), dtPDF);
                    }
                }
                if (ViewState["output_error"].ToString() == "")
                {

                    DataTable dtcurrency = PhoenixAccountsDebitNoteReferenceAttachments.DebitNoteCurrencyMismatchVoucherList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                       , General.GetNullableGuid(ddlDebitReference.SelectedValue)
                                       , txtAccountCOde.Text
                                       , General.GetNullableDateTime(ucFromDate.Text)
                                       , General.GetNullableDateTime(ucToDate.Text)
                                         );

                    if (dtcurrency.Rows.Count > 0)
                    {

                        gvCurrencyGrid.DataSource = dtcurrency;
                        //divPage.Visible = true;
                        ViewState["ROWCOUNT"] = iRowCount;
                        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
                        gvCurrencyGrid.Rebind();
                        lblAttcahMsg.Text = "* Unable to Bill the below line items as base currency is not matching with vessel currency";
                        lblcaption.Visible = true;
                    }
                    PhoenixAccountsDebitNoteReference.DebitNoteUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                    , General.GetNullableGuid(ddlDebitReference.SelectedValue)
                                    , txtAccountCOde.Text
                                    , General.GetNullableDateTime(ucFromDate.Text)
                                    , General.GetNullableDateTime(ucToDate.Text));
                    ucStatus.Text = "Debit Note Reference Updated.";
                    gvFormDetails.DataSource = null;

                    // divPage.Visible = false;
                    lblAttcahMsg.Text = string.Empty;
                    lblcaption.Visible = false;
                    gvPDFissues.DataSource = null;
                    gvPDFissues.DataBind();
                    lblPDFissue.Visible = false;
                }
            }
        }
        finally
        {
            if (dtPDF.Rows.Count > 0)
            {
                lblPDFissue.Text = "Non-Standard PDF Issues Voucher List";
                lblPDFissue.Visible = true;
                gvPDFissues.DataSource = dtPDF;
                gvPDFissues.DataBind();
            }
        }
    }

    private bool IsValidData()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtAccountCOde.Text) == null)
            ucError.ErrorMessage = "Account code is required";
        if (General.GetNullableDateTime(ucFromDate.Text) == null)
            ucError.ErrorMessage = "From date is required.";
        if (ucToDate.Enabled == false)
        {
        }
        else
        {
            if (General.GetNullableDateTime(ucToDate.Text) == null)
                ucError.ErrorMessage = "To date is required.";
        }
        return (!ucError.IsError);
    }


    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {

            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindAttachments();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindAttachments()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = PhoenixAccountsDebitNoteReferenceAttachments.DebitNoteNonPDFVoucherList(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                          , General.GetNullableGuid(ddlDebitReference.SelectedValue)
                          , txtAccountCOde.Text
                          , General.GetNullableDateTime(ucFromDate.Text)
                          , General.GetNullableDateTime(ucToDate.Text)
                          , (int)ViewState["PAGENUMBER"]
                          , General.ShowRecords(null)
                          , ref iRowCount
                          , ref iTotalPageCount);

        if (dt.Rows.Count > 0)
        {
            gvFormDetails.DataSource = dt;

            // divPage.Visible = true;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            gvFormDetails.Rebind();
            lblAttcahMsg.Text = "* Cannot bill vouchers. Below vouchers are having non-standard PDF. Please replace PDF Files";
            lblcaption.Visible = true;
        }
        else
        {
            //divPage.Visible = false;
            lblAttcahMsg.Text = string.Empty;
            lblcaption.Visible = false;
        }
    }

    protected void MenuGenralSub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper() == "SOAREFERENCE")
            {
                //"&status=" + ViewState["status"].ToString() + "&type=" + ViewState["type"].ToString() + "&month=" + ViewState["month"].ToString() + "&year=" + ViewState["year"].ToString() + "&Condition=1"
                //   Response.Redirect("../Accounts/AccountsSOAGeneration.aspx?status=" + ViewState["status"].ToString()", true);
                Response.Redirect("../Accounts/AccountsDashboardSOAGeneration.aspx?status=" + ViewState["status"].ToString() + "&type=" + ViewState["type"].ToString() + "&month=" + ViewState["month"].ToString() + "&year=" + ViewState["year"].ToString() + "&Condition=1", true);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindSOADetails(Guid DebitNoteRefID)
    {
        DataTable dt = PhoenixAccountsSOAGeneration.SOAGenerationDetails(DebitNoteRefID);
        if (dt.Rows.Count > 0)
        {
            txtCreditAccountId.Text = dt.Rows[0]["FLDACCOUNTID"].ToString();
            txtAccountCOde.Text = dt.Rows[0]["FLDACCOUNTCODE"].ToString();
            txtCreditAccountDescription.Text = dt.Rows[0]["FLDACCOUNTDESCRIPTION"].ToString();
            lblOwner.Text = dt.Rows[0]["FLDOWNERID"].ToString();
            if (!string.IsNullOrEmpty(txtAccountCOde.Text))
            {
                BindDebitReference();
                if (ddlDebitReference.Items.Count > 0)
                    ddlDebitReference.SelectedValue = DebitNoteRefID.ToString();
            }
            if (dt.Rows[0]["FLDTYPE"].ToString() == "Monthly Report")
            {
                ucToDate.Text = dt.Rows[0]["FLDLASTDATE"].ToString();
                ucToDate.Enabled = false;
            }
            else
            {
                ucToDate.Text = "";
                ucToDate.Enabled = true;
            }
        }
    }

    private DataTable CheckPdfProtection(string filePath, string voucher, string filename, DataTable dtPDF)
    {
        try
        {
            PdfReader reader = new PdfReader(filePath);
            try
            {
                PdfSharp.Pdf.PdfDocument pdfDoc = PdfSharp.Pdf.IO.PdfReader.Open(filePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
            }
            catch (Exception)
            {
                throw new Exception("the uploaded pdf file is corrupted and cannot be uploaded");
            }

            if (!reader.IsEncrypted()) return dtPDF;

            if (!PdfEncryptor.IsPrintingAllowed(reader.Permissions))
            {
                ViewState["output_error"] = ViewState["output_error"] + Environment.NewLine + new InvalidOperationException(voucher + " - " + filename + " the selected file is print protected and cannot be imported") + Environment.NewLine;
                dtPDF.Rows.Add(voucher, filename, "print protected and cannot be imported");
            }
            if (!PdfEncryptor.IsModifyContentsAllowed(reader.Permissions))
            {
                ViewState["output_error"] = ViewState["output_error"] + Environment.NewLine + new InvalidOperationException(voucher + " - " + filename + "the selected file is write protected and cannot be imported") + Environment.NewLine;
                dtPDF.Rows.Add(voucher, filename, "write protected and cannot be imported");
            }
        }
        catch (Exception ex)
        {
            dtPDF.Rows.Add(voucher, filename, ex.ToString().Substring(0, ex.ToString().IndexOf(Environment.NewLine)));
            ViewState["output_error"] = ViewState["output_error"] + Environment.NewLine + "Voucher : " + voucher + " File :" + filename + ex.ToString() + Environment.NewLine;
        }
        return dtPDF;
    }

    protected void gvCurrencyGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {

    }

    protected void gvFormDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFormDetails.CurrentPageIndex + 1;
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormDetails_ItemDataBound1(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (cmdEdit != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
        }
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Guid? lblVoucherDTkey = General.GetNullableGuid(drv["FLDVOUCHERDTKEY"].ToString());
            Guid? lblVoucherLineDtkey = General.GetNullableGuid(drv["FLDVOUCHERLINEDTKEY"].ToString());
            ImageButton cmdAttachment = (ImageButton)e.Item.FindControl("cmdAttachment");
            if (cmdAttachment != null)
            {
                cmdAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblVoucherDTkey + "&mod="
                                    + PhoenixModule.ACCOUNTS + "');return true;");
                cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            }
            ImageButton cmdNoAttachment = (ImageButton)e.Item.FindControl("cmdNoAttachment");
            if (cmdNoAttachment != null)
            {
                cmdNoAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblVoucherLineDtkey + "&mod="
                                    + PhoenixModule.ACCOUNTS + "');return true;");
                cmdNoAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdNoAttachment.CommandName);
            }


            ImageButton iab = (ImageButton)e.Item.FindControl("cmdAttachment");
            ImageButton inab = (ImageButton)e.Item.FindControl("cmdNoAttachment");
            if (iab != null) iab.Visible = true;
            if (inab != null) inab.Visible = true;
        }
    }
}
