using System;
using System.Data;
using System.Collections.Generic;
using System.Web;
using System.IO;
using System.Collections.Specialized;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Telerik.Web.UI;

public partial class AccountsInvoiceVoucherAttachmentPdfReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenPick.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuBtn.AccessRights = this.ViewState;
            MenuBtn.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                txtCreditAccountId.Attributes.Add("style", "visibility:hidden");

                ddlMonth.SelectedValue = DateTime.Today.Month.ToString();
                BindYear();
            }
            imgShowAccount.Attributes.Add("onclick", "return showPickList('spnPickListCreditAccount', 'codehelp1', '', '../Common/CommonPickListCompanyAccount.aspx?ignoreiframe=true',null); ");
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindYear()
    {
        for (int i = (DateTime.Today.Year - 4); i <= (DateTime.Today.Year); i++)
        {
            Telerik.Web.UI.DropDownListItem li = new Telerik.Web.UI.DropDownListItem(i.ToString(), i.ToString());
            ddlYear.Items.Add(li);

        }
        ddlYear.DataBind();
        ddlYear.SelectedValue = DateTime.Today.Year.ToString();
    }

    protected void MenuBtn_OnTabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToString().ToUpper() == "SHOWREPORT")
        {
            try
            {
                if (!IsValidReport(ddlMonth.SelectedValue, ddlYear.SelectedValue, ddlReportType.SelectedValue, txtCreditAccountId.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                string FileN = "Posted-Invoice-Supportings-" + Guid.NewGuid().ToString();

                Bind(FileN);

                string src, filepath;
                filepath = HttpContext.Current.Request.MapPath("~/Attachments/Accounts/" + FileN + ".pdf");
                src = Session["sitepath"] + "/attachments/Accounts/" + FileN + ".pdf";

                if (File.Exists(filepath))
                    ifMoreInfo.Attributes["src"] = src;
                else
                    ifMoreInfo.Attributes["src"] = null;

            }
            catch (Exception ex)
            {
                ViewState["ERRMSG"] = ex.Message;

                if (ViewState["INVNUMBER"] != null)
                    ucError.ErrorMessage = ViewState["INVNUMBER"].ToString() + " having incompatible pdf.";
                else
                    ucError.ErrorMessage = ex.Message;

                ucError.Visible = true;
            }
        }
    }

    private bool IsValidReport(string month, string year, string reporttype, string accountid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (month == "")
            ucError.ErrorMessage = "Month is required.";
        if (year == "")
            ucError.ErrorMessage = "Year is required.";
        if (reporttype == "")
            ucError.ErrorMessage = "Report type is required.";
        if (accountid == "")
            ucError.ErrorMessage = "Vessel account is required.";

        return (!ucError.IsError);
    }


    public class lstPdfFiles
    {
        public string FilePath { get; set; }
        public string HtmlContent { get; set; }
        public string VoucherNumber { get; set; }
        public string Flag { get; set; }
    }

    private void Bind(string FileN)
    {
        FontFactory.RegisterDirectories();
        Font fontNormal = new Font(FontFactory.GetFont("Arial", 12, Font.NORMAL));

        string strData;

        List<lstPdfFiles> lst = new List<lstPdfFiles>();

        DataTable DtAL = new DataTable();
        if (ddlReportType.SelectedValue.ToUpper() == "INVOICE AND MST ACKNOWLEDGEMENT")
        {
            DtAL = PhoenixAccountsVoucher.AllVoucherAttachmentList(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, int.Parse(txtCreditAccountId.Text), int.Parse(ddlMonth.SelectedValue), int.Parse(ddlYear.SelectedValue));
        }
        else
        {
            DtAL = PhoenixAccountsVoucher.VoucherAttachmentList(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, int.Parse(txtCreditAccountId.Text), int.Parse(ddlMonth.SelectedValue), int.Parse(ddlYear.SelectedValue), ddlReportType.SelectedValue);
        }

        if (DtAL.Rows.Count > 0)
        {
            foreach (DataRow DrFA in DtAL.Rows) // Loop over the rows.
            {
                strData = "PURCHASE INVOICE VOUCHER NO : " + DrFA["FLDVOUCHERNUMBER"].ToString() + " DATE : " + DrFA["FLDVOUCHERDATE"].ToString() + " DESCRIPTION : " + DrFA["FLDLONGDESCRIPTION"].ToString();

                if (File.Exists(HttpContext.Current.Request.MapPath("~/Attachments/") + DrFA["FLDFILEPATH"].ToString()))
                    lst.Add(new lstPdfFiles { FilePath = HttpContext.Current.Request.MapPath("~/Attachments/") + DrFA["FLDFILEPATH"].ToString(), VoucherNumber = DrFA["FLDVOUCHERNUMBER"].ToString(), HtmlContent = strData, Flag = "1" });

                strData = "";
            }


            if (lst.Count > 0)
            {
                MergePdfFiles(HttpContext.Current.Request.MapPath("~/Attachments/Accounts/" + FileN + ".pdf"), lst);
                FileInfo fi = new FileInfo(HttpContext.Current.Request.MapPath("~/Attachments/Accounts/" + FileN + ".pdf"));
                PhoenixAccountsVoucher.SupportingDocumentInsert(FileN, General.GetNullableInteger(fi.Length.ToString()), Guid.NewGuid());
            }
            else
            {
                ucError.ErrorMessage = "There is no attachment";
                ucError.Visible = true;
            }
        }
        else
        {
            ucError.ErrorMessage = "There is no attachment";
            ucError.Visible = true;
        }
    }

    public void MergePdfFiles(string destinationfile, List<lstPdfFiles> files)
    {
        Document document = null;

        try
        {
            List<PdfReader> readers = new List<PdfReader>();
            List<int> pages = new List<int>();
            List<string> StrData = new List<string>();

            for (int index = 0; index < files.Count; index++)
            {
                lstPdfFiles List = files[index];
                if (File.Exists(List.FilePath))
                {
                    ViewState["INVNUMBER"] = List.VoucherNumber;
                    readers.Add(new PdfReader(List.FilePath));
                    StrData.Add(List.HtmlContent);
                }
            }

            document = new Document(readers[0].GetPageSizeWithRotation(1));

            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(destinationfile, FileMode.Create));

            document.Open();

            int i = 0;
            foreach (PdfReader reader in readers)
            {
                pages.Add(reader.NumberOfPages);
                WritePage(reader, document, writer, StrData[i]);
                i = i + 1;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        finally
        {
            document.Close();
        }
    }

    private void WritePage(PdfReader reader, iTextSharp.text.Document document, PdfWriter writer, string str)
    {
        try
        {
            PdfContentByte cb = writer.DirectContent;
            PdfImportedPage page;

            int rotation = 0;

            for (int i = 1; i <= reader.NumberOfPages; i++)
            {
                document.SetPageSize(reader.GetPageSizeWithRotation(i));
                document.NewPage();

                PdfPTable ft = new PdfPTable(1);
                ft.TotalWidth = document.PageSize.Width - 80;
                ft.DefaultCell.BorderWidth = 0;
                ft.DefaultCell.BackgroundColor = Color.LIGHT_GRAY;
                ft.AddCell(str);
                //ft.WriteSelectedRows(0, 1, document.LeftMargin + 1, document.PageSize.Height - 5, writer.DirectContent);  // Top
                ft.WriteSelectedRows(0, -1, document.LeftMargin + 1, 30, writer.DirectContent); // Bottom

                page = writer.GetImportedPage(reader, i);

                rotation = reader.GetPageRotation(i);

                if (rotation == 90 || rotation == 270)
                {
                    cb.AddTemplate(page, 0, -1f, 1f, 0, 0, reader.GetPageSizeWithRotation(i).Height);
                }
                else
                {
                    cb.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        NameValueCollection nvc = Filter.CurrentPickListSelection;
        txtCreditAccountCode.Text = nvc.Get("txtCreditAccountCode");
        txtCreditAccountDescription.Text = nvc.Get("txtCreditAccountDescription");
        txtCreditAccountId.Text = nvc.Get("txtCreditAccountId");
    }
}

