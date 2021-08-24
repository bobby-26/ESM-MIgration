using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class AccountsFileTransferBatchUpload : PhoenixBasePage
{
    public int iCompanyid;

    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Upload", "UPLOAD",ToolBarDirection.Right);
        MenutravelInvoice.Title = "CSV File Upload";
        MenutravelInvoice.AccessRights = this.ViewState;
        MenutravelInvoice.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
     

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }
        //BindData();
        //SetPageNavigator();
    }

   

    

    protected void MenutravelInvoice_OnTabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToString().ToUpper() == "UPLOAD")
        {
            try
            {
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(FileUpload.FileName.ToString());

                //if (extension.ToUpper() == ".XLSX")
                //{
                string strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + "TEMP/" + fileName + extension;
                //string strpath = HttpContext.Current.Server.MapPath("~/Attachments/TEMP/") + fileName + extension;
                    FileUpload.PostedFile.SaveAs(strpath);
                    //bool importstatus = false;
                    //CheckImportfile(strpath, ref importstatus);

                   // if (importstatus)
                    //{
                        BankStatementExcelUpload(strpath, fileName);
                    //}
                    //else
                    //{
                    //    ucError.ErrorMessage = "Please upload correct file with data.";
                    //    ucError.Visible = true;
                    //    return;
                    //}
                //}
                //else
                //{
                //    ucError.ErrorMessage = "Please upload .xlsx file only";
                //    ucError.Visible = true;
                //    return;
                //}
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
    }

    private void CheckImportfile(string filepath, ref bool importstatus)
    {
        if (File.Exists(filepath))
            importstatus = true;
        else
            importstatus = false;
    }

    public void BankStatementExcelUpload(string filepath, string FileGuidName)
    {
        try
        {
            string patchid = Guid.NewGuid().ToString();
            Guid BatchID = new Guid(patchid);
            List<string> cellvalues = new List<string>();
            var package = new ExcelPackage(new FileInfo(filepath));
            ExcelWorksheets worksheets = package.Workbook.Worksheets;
           

            if (worksheets.Count > 0)
            {
                foreach (ExcelWorksheet workSheet in worksheets)
                {
                    if (workSheet.Dimension != null)
                    {
                        int endRow = 1;

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            if (workSheet.Cells[i, 1].Value == null)
                            {
                                //endRow = i;
                                break;
                            }
                            endRow = i;
                        }
                                                
                        cellvalues.Clear();

                        //Guid? uploadid = null;
                        //    //InsertBankStatementExcel(
                        //    //int.Parse(ddlBankAccount.SelectedBankAccount),
                        //    //ddlType.SelectedValue,
                        //    //fromdate,
                        //    //todate,
                        //    //month,
                        //    //General.GetNullableInteger(ucCompany.SelectedCompany));

                        //HttpFileCollection postedFiles = Request.Files;
                        //HttpPostedFile postedFile = postedFiles[0];

                        //string filename = postedFile.FileName;
                        //if (filename.LastIndexOfAny(new[] { '/', '\\' }) > 0)
                        //{
                        //    filename = filename.Substring(filename.LastIndexOfAny(new[] { '/', '\\' }) + 1);
                        //}
                        //filepath = "ACCOUNTS/" + FileGuidName + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        //PhoenixCommonFileAttachment.InsertAttachment(
                        //    new Guid(uploadid.ToString()), filename, filepath, postedFile.ContentLength
                        //        , 0
                        //        , 0, "BANKSTATEMENT", new Guid(uploadid.ToString()));


                     
                        string filename = "";
                        string type = "";
                        Guid dtkey;
                         
                        for (int i = workSheet.Dimension.Start.Row + 1; i <= endRow; i++)
                        {
                            filename = workSheet.Cells[i, 1].Value.ToString();
                            type = workSheet.Cells[i, 2].Value.ToString();
                            dtkey =new Guid(workSheet.Cells[i, 3].Value.ToString());
                            PhoenixInvoiceComments.InsertCSVCopyFiles(BatchID, filename, type, dtkey);
                        }                       
                    }
                }
            }
            //PhoenixInvoiceComments.CSVVoucherFilesCopy(BatchID, @"D:\TEMP", @"D:\TEMP\COPY");
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

    private bool VerifyHeaders(List<string> li)
    {
        if (!li[0].ToString().ToUpper().Equals("TT REFERENCE"))
            return false;
        else if (!li[1].ToString().ToUpper().Equals("BANK REFERENCE"))
            return false;
        else if (!li[2].ToString().ToUpper().Equals("CUSTOMER REFERENCE"))
            return false;
        else if (!li[3].ToString().ToUpper().Equals("VALUE DATE (DD/MM/YYYY)"))
            return false;
        else if (!li[4].ToString().ToUpper().Equals("DEBIT AMOUNT"))
            return false;
        else
            return true;
    }



    public void ExcelUpload(string filepath, string FileGuidName)
    {
        try
        {

            //using (TextFieldParser parser = new TextFieldParser(@"c:\temp\test.csv"))
            //{
            //    parser.TextFieldType = FieldType.Delimited;
            //    parser.SetDelimiters(",");
            //    while (!parser.EndOfData)
            //    {
            //        //Processing row
            //        string[] fields = parser.ReadFields();
            //        foreach (string field in fields)
            //        {
            //            //TODO: Process field
            //        }
            //    }
            //}
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

}
