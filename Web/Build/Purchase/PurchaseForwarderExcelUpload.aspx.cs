using System;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using SouthNests.Phoenix.Framework;
using System.Collections.Generic;
using SouthNests.Phoenix.CrewManagement;
using System.Web.UI;
using SouthNests.Phoenix.Purchase;

public partial class PurchaseForwarderExcelUpload : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Upload", "UPLOAD");
            MenutravelInvoice.AccessRights = this.ViewState;
            MenutravelInvoice.MenuList = toolbar.Show();
        }
    }

    protected void MenutravelInvoice_OnTabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToString().ToUpper() == "UPLOAD")
        {
            try
            {
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(FileUpload.FileName.ToString());

                if (extension.ToUpper() == ".XLSX")
                {
                    string strpath = HttpContext.Current.Server.MapPath("~/Attachments/Purchase/") + fileName + extension;
                    FileUpload.PostedFile.SaveAs(strpath);
                    bool importstatus = false;
                    CheckImportfile(strpath, ref importstatus);

                    if (importstatus)
                    {
                        //PhoenixCrewTravelQuoteLine.InvoiceUpload(strpath);
                        ForwarderExcelUpload(strpath);

                        ucStatus.Text = "Forwarder Excel Uploaded Successfully.";
                        ucStatus.Visible = true;
                    }
                    else
                    {
                        ucError.ErrorMessage = "Please upload correct file with data.";
                        ucError.Visible = true;
                        return;
                    }
                }
                else
                {
                    ucError.ErrorMessage = "Please upload .xlsx file only";
                    ucError.Visible = true;
                    return;
                }
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

    public void ForwarderExcelUpload(string filepath)
    {
        try
        {
            string patchid = Guid.NewGuid().ToString();

            List<string> cellvalues = new List<string>();
            var package = new ExcelPackage(new FileInfo(filepath));
            ExcelWorksheets worksheets = package.Workbook.Worksheets;

            if (worksheets.Count > 0)
            {
                foreach (ExcelWorksheet workSheet in worksheets)
                {
                    if (workSheet.Dimension != null)
                    {
                        for (int i = workSheet.Dimension.Start.Row + 2; i <= workSheet.Dimension.End.Row; i++)
                        {
                            for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                            {
                                cellvalues.Add(Convert.ToString(workSheet.Cells[i, j].Value));
                            }

                            cellvalues.Add(Convert.ToString(workSheet.Cells[1, 1].Value));
                            InsertDelivery(cellvalues, new Guid(patchid));
                            cellvalues.Clear();
                        }
                    }
                }
            }
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

    private void InsertDelivery(List<string> li, Guid patchid)
    {
        try
        {
            PhoenixPurchaseDelivery.InsertDeliveryExcel
                (
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                li[1].ToString() // vesl name
                , li[8].ToString() // formnos
                , General.GetNullableInteger(li[9].ToString()) // no of packages
                , General.GetNullableDecimal(li[10].ToString()) // total weight
                , li[16].ToString() // forwarder
                , General.GetNullableDateTime(li[3].ToString()) // recd date
                , li[0].ToString() // shipment mode
                , li[7].ToString() // vendor
                , li[12].ToString() // currency code
                , General.GetNullableDecimal(li[13].ToString()) // value
                , li[11].ToString().Equals("Y") ? 1 : 0 // is dgr
                , li[14].ToString() // short note
                , li[4].ToString() // mawb
                , li[5].ToString() // hawb
                , li[2].ToString() // origin
                , li[6].ToString() // location
                , li[15].ToString().Equals("Y") ? 1 : 0 // is delivered
                );

                                       // (
                                       //General.GetNullableString(li[0].ToString()), General.GetNullableDateTime(li[1].ToString()), li[2].ToString(), li[3].ToString()
                                       // , General.GetNullableDateTime(li[4].ToString()), li[5].ToString(), li[6].ToString()
                                       // , li[7].ToString(), li[8].ToString(), li[9].ToString(), li[10].ToString(), li[11].ToString()
                                       // , li[12].ToString(), General.GetNullableDecimal(li[13].ToString()), General.GetNullableDecimal(li[14].ToString())
                                       // , General.GetNullableDecimal(li[15].ToString()), General.GetNullableDecimal(li[16].ToString())
                                       // , General.GetNullableDecimal(li[17].ToString()), General.GetNullableDecimal(li[18].ToString()), General.GetNullableDecimal(li[19].ToString())
                                       // , li[20].ToString(), patchid
                                       // );
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }
}
