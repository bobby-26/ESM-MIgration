using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using System.Globalization;
using SouthNests.Phoenix.Common;
using iTextSharp.text.pdf;
using System.IO;
using Telerik.Web.UI;

public partial class AccountsDebitReferenceUpdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Update", "UPDATE", ToolBarDirection.Right);
            toolbar.AddButton("Voucher List", "LIST", ToolBarDirection.Right);

            MenuDebitReference.AccessRights = this.ViewState;
            MenuDebitReference.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                cmdHiddenPick.Attributes.Add("style", "display:none;");
                BindDebitReference();
                //divPage.Visible = false;
                lblAttcahMsg.Text = string.Empty;
                lblcaption.Visible = false;
                ViewState["output_filename"] = "";
                ViewState["output_vouchernumber"] = "";
                ViewState["output_error"] = "";

                gvFormDetails.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                imgShowAccount.Attributes.Add("onclick", "return showPickList('spnPickListCreditAccount', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListCompanyAccount.aspx', true); ");
            }
                        

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
        DataSet ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceList(
            txtAccountCOde.Text);

        ddlDebitReference.DataTextField = "FLDDEBITNOTEREFERENCE";
        ddlDebitReference.DataValueField = "FLDDEBINOTEREFERENCEID";
        ddlDebitReference.DataSource = ds;
        ddlDebitReference.DataBind();
        ddlDebitReference.Items.Insert(0, new RadComboBoxItem("--Select--", new Guid().ToString()));

        if (ddlDebitReference.SelectedValue != "")
        {
            chktype(new Guid(ddlDebitReference.SelectedValue));
        }
        else
        {
            ucToDate.Enabled = true;
        }

    }

    public void ddlDebitReference_SelectedIndexChanged(object sender, EventArgs e)
    {
        chktype(new Guid(ddlDebitReference.SelectedValue));
    }


    public void chktype(Guid debitnoterefernceid)
    {
        DataSet ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceEdit(debitnoterefernceid);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            if (dr["FLDTYPE"].ToString() == "Monthly Report")
            {
                ucToDate.Enabled = false;
                ucToDate.Text = dr["FLDLASTDATE"].ToString();
            }

            else
            {
                ucToDate.Enabled = true;
                ucToDate.Text = "";
            }
        }
    }
    protected void MenuDebitReference_TabStripCommand(object sender, EventArgs e)
    {


        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;


        if (CommandName.ToUpper() == "UPDATE")
        {
            BindData();
        }
        else if (CommandName.ToUpper() == "LIST")
        {
            Response.Redirect("../Accounts/AccountsERMVoucherDetail.aspx", false);

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
            // ViewState["Flag"] = 1;
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
             
               // divPage.Visible = true;
                ViewState["ROWCOUNT"] = iRowCount;
                ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
                gvFormDetails.Rebind();
                lblAttcahMsg.Text = "* Cannot bill vouchers. Below vouchers are having non-standard PDF. Please replace PDF Files";
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
                        //CheckPdfProtection(HttpContext.Current.Request.MapPath("~/Attachments/") + dr["FLDFILEPATH"].ToString(), dr["FLDVOUCHERNUMBER"].ToString(), dr["FLDFILENAME"].ToString(), dtPDF);
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

                    //ViewState["Flag"] = 0;
                    gvFormDetails.DataSource = null;
                  
                    //divPage.Visible = false;
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
            //ucPDFError.HeaderMessage = "Please fix following PDF issues";
            //ucPDFError.ErrorMessage = ViewState["output_error"].ToString();
            //ucPDFError.Visible = true;            
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
        //DateTime resultdate, resultDOA;

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
           
            //divPage.Visible = true;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            gvFormDetails.Rebind();
            lblAttcahMsg.Text = "* Cannot bill vouchers. Below vouchers are having non-standard PDF. Please replace PDF Files";
            lblcaption.Visible = true;
        }
        else
        {
            DataTable dtPDF = new DataTable();
            dtPDF.Columns.Add("FLDVOUCHERNUMBER");
            dtPDF.Columns.Add("FLDFILENAME");
            dtPDF.Columns.Add("FLDMSG");

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

            //divPage.Visible = false;
            lblAttcahMsg.Text = string.Empty;
            lblcaption.Visible = false;
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


    private bool CheckPdfProtectionAll()
    {
        bool result = false;
        string filePath = "";
        string voucher = "";
        string filename = "";
        try
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
                    filename = dr["FLDFILENAME"].ToString();
                    string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                    if (!path.EndsWith("/"))
                        path = path + "/";
                    filePath = path + dr["FLDFILEPATH"].ToString();
                    voucher = dr["FLDVOUCHERNUMBER"].ToString();
                    PdfReader reader = new PdfReader(filePath);
                    PdfSharp.Pdf.PdfDocument pdfDoc = PdfSharp.Pdf.IO.PdfReader.Open(filePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
                    if (!reader.IsEncrypted()) return true;

                    if (!PdfEncryptor.IsPrintingAllowed(reader.Permissions))
                        ViewState["output_error"] = ViewState["output_error"] + "</br>" + new InvalidOperationException(voucher + " - " + filename + " the selected file is print protected and cannot be imported");
                    if (!PdfEncryptor.IsModifyContentsAllowed(reader.Permissions))
                        ViewState["output_error"] = ViewState["output_error"] + "</br>" + new InvalidOperationException(voucher + " - " + filename + "the selected file is write protected and cannot be imported");
                }
            }
        }
        catch (BadPasswordException) { ViewState["output_error"] = ViewState["output_error"] + "</br>" + new InvalidOperationException(voucher + " - " + filename + "the selected file is password protected and cannot be imported"); }
        catch (BadPdfFormatException) { ViewState["output_error"] = ViewState["output_error"] + "</br>" + new InvalidDataException(voucher + " - " + filename + "the selected file is having invalid format and cannot be imported"); }
        finally
        {
            ucError.ErrorMessage = ViewState["output_error"].ToString();
            ucError.Visible = true;
            result = true;
        }
        return result;
    }

    private bool IsValidPDF()
    {
        //DateTime resultdate, resultDOA;

        ucPDFError.HeaderMessage = "Please fix following PDF issues";

        string filePath = "";
        string voucher = "";
        string filename = "";
        try
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
                    filename = dr["FLDFILENAME"].ToString();
                    string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                    if (!path.EndsWith("/"))
                        path = path + "/";
                    filePath = path + dr["FLDFILEPATH"].ToString();
                    voucher = dr["FLDVOUCHERNUMBER"].ToString();
                    PdfReader reader = new PdfReader(filePath);
                    PdfSharp.Pdf.PdfDocument pdfDoc = PdfSharp.Pdf.IO.PdfReader.Open(filePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
                    if (!reader.IsEncrypted()) { ucPDFError.ErrorMessage = voucher + " - " + filename + " the selected file is not encrypted"; }
                    if (!PdfEncryptor.IsPrintingAllowed(reader.Permissions))
                        ucPDFError.ErrorMessage = "*" + new InvalidOperationException(voucher + " - " + filename + " the selected file is print protected and cannot be imported");
                    if (!PdfEncryptor.IsModifyContentsAllowed(reader.Permissions))
                        ucPDFError.ErrorMessage = "*" + new InvalidOperationException(voucher + " - " + filename + "the selected file is write protected and cannot be imported");
                }
            }
        }
        catch (BadPasswordException) { ucPDFError.ErrorMessage = "*" + new InvalidOperationException(voucher + " - " + filename + "the selected file is password protected and cannot be imported"); }
        catch (BadPdfFormatException) { ucPDFError.ErrorMessage = "*" + new InvalidDataException(voucher + " - " + filename + "the selected file is having invalid format and cannot be imported"); }
        return (!ucPDFError.IsError);
    }

    protected void gvPDFissues_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.Header)
        //{
        //    if (ViewState["SORTEXPRESSION"] != null)
        //    {
        //        HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
        //        if (img != null)
        //        {
        //            if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
        //                img.Src = Session["images"] + "/arrowUp.png";
        //            else
        //                img.Src = Session["images"] + "/arrowDown.png";

        //            img.Visible = true;
        //        }
        //    }
        //}

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdEdit");
        //    if (cmdEdit != null)
        //        if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
        //}
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataRowView drv = (DataRowView)e.Row.DataItem;
        //    Guid? lblVoucherDTkey = General.GetNullableGuid(drv["FLDVOUCHERDTKEY"].ToString());
        //    Guid? lblVoucherLineDtkey = General.GetNullableGuid(drv["FLDVOUCHERLINEDTKEY"].ToString());
        //    ImageButton cmdAttachment = (ImageButton)e.Row.FindControl("cmdAttachment");
        //    if (cmdAttachment != null)
        //    {
        //        cmdAttachment.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblVoucherDTkey + "&mod="
        //                            + PhoenixModule.ACCOUNTS + "');return true;");
        //        cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
        //    }
        //    ImageButton cmdNoAttachment = (ImageButton)e.Row.FindControl("cmdNoAttachment");
        //    if (cmdNoAttachment != null)
        //    {
        //        cmdNoAttachment.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + lblVoucherLineDtkey + "&mod="
        //                            + PhoenixModule.ACCOUNTS + "');return true;");
        //        cmdNoAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdNoAttachment.CommandName);
        //    }


        //    ImageButton iab = (ImageButton)e.Row.FindControl("cmdAttachment");
        //    ImageButton inab = (ImageButton)e.Row.FindControl("cmdNoAttachment");
        //    if (iab != null) iab.Visible = true;
        //    if (inab != null) inab.Visible = true;
        //    //int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());
        //    //if (n == 0)
        //    //{
        //    //    if (iab != null) iab.Visible = false;
        //    //    if (inab != null) inab.Visible = true;
        //    //}
        //}
    }






    protected void gvCurrencyGrid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
      //  BindData();
    }

    protected void gvFormDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFormDetails.CurrentPageIndex + 1;
           // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFormDetails_ItemDataBound(object sender, GridItemEventArgs e)
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
                cmdAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + lblVoucherDTkey + "&mod="
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

    protected void gvFormDetails_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }
}
