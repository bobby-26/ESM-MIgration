using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CommonBudgetOpeningBalancesGenerate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Upload", "UPLOAD",ToolBarDirection.Right);
            AttachmentList.AccessRights = this.ViewState;
            AttachmentList.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["vesselid"].ToString() != null)
                {
                    ViewState["VESSELACCOUNTID"] = Request.QueryString["vesselid"].ToString();
                }
                
                if (Request.QueryString["ownerreportbalance"] != null)
                {
                    ViewState["OWNERREPORTID"] = Request.QueryString["ownerreportbalance"].ToString();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
     
    protected void MenutravelInvoice_OnTabStripCommand(object sender, EventArgs e)
    {

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToString().ToUpper() == "UPLOAD")
        {
            try
            {
                string fileName = txtFileUpload1.FileName.ToString();
                string extension = Path.GetExtension(txtFileUpload1.FileName.ToString());

                if (extension.ToUpper() == ".XLSX")
                {
                    string strpath = HttpContext.Current.Server.MapPath("~/Attachments/Accounts/") + fileName + extension;
                    txtFileUpload1.PostedFile.SaveAs(strpath);
                    bool importstatus = false;
                    CheckImportfile(strpath, ref importstatus);

                    if (importstatus)
                    {
                        OpeningBalanceExcelUpload(strpath, fileName);
                        BindData();
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

    public void OpeningBalanceExcelUpload(string filepath, string FileName)
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
                        int endRow = 1;

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            if (workSheet.Cells[i, 1].Value == null)
                            {
                                break;
                            }
                            endRow = i;
                        }

                        for (int j = workSheet.Dimension.Start.Column; j <= 9; j++)
                        {
                            if (!workSheet.Cells[1, j].Value.ToString().Equals(""))
                                cellvalues.Add(Convert.ToString(workSheet.Cells[1, j].Value));
                        }
                        if (!VerifyHeaders(cellvalues))
                        {
                            ucError.ErrorMessage = "File is of incorrect format";
                            ucError.Visible = true;
                            return;
                        }

                        cellvalues.Clear();


                        for (int i = workSheet.Dimension.Start.Row + 1; i <= endRow; i++)
                        {
                            for (int j = workSheet.Dimension.Start.Column; j <= 9; j++)
                            {
                                if (!workSheet.Cells[i, 1].Value.ToString().Equals(""))
                                    cellvalues.Add(Convert.ToString(workSheet.Cells[i, j].Value));

                            }

                            string ESMBudgetCode = General.GetNullableString(workSheet.Cells[i, 1].Value == null ? null : workSheet.Cells[i, 1].Value.ToString());

                            if (ESMBudgetCode == null)
                            {
                                ucError.ErrorMessage = "Budget Code in row " + i.ToString() + " is not in correct format";
                                ucError.Visible = true;
                                return;
                            }

                            decimal? AccumulatedExpenses = General.GetNullableDecimal(workSheet.Cells[i, 2].Value == null ? null : workSheet.Cells[i, 2].Value.ToString());

                            if (AccumulatedExpenses == null)
                            {
                                ucError.ErrorMessage = "Accumulated Expenses in row " + i.ToString() + " is not in correct format";
                                ucError.Visible = true;
                                return;
                            }

                            decimal? AccumulatedCommittedCost = General.GetNullableDecimal(workSheet.Cells[i, 3].Value == null ? null : workSheet.Cells[i, 3].Value.ToString());

                            if (AccumulatedCommittedCost == null)
                            {
                                ucError.ErrorMessage = "Accumulated Committed Cost in row " + i.ToString() + " is not in correct format";
                                ucError.Visible = true;
                                return;
                            }
                            decimal? AccumulatedTotalExpense = General.GetNullableDecimal(workSheet.Cells[i, 4].Value == null ? null : workSheet.Cells[i, 4].Value.ToString());

                            if (AccumulatedTotalExpense == null)
                            {
                                ucError.ErrorMessage = "Accumulated Total Expense in row " + i.ToString() + " is not in correct format";
                                ucError.Visible = true;
                                return;
                            }

                            decimal? AccumulatedBudget = General.GetNullableDecimal(workSheet.Cells[i, 5].Value == null ? null : workSheet.Cells[i, 5].Value.ToString());

                            if (AccumulatedBudget == null)
                            {
                                ucError.ErrorMessage = "Accumulated Budget in row " + i.ToString() + " is not in correct format";
                                ucError.Visible = true;
                                return;
                            }

                            decimal? YTDExpenses = General.GetNullableDecimal(workSheet.Cells[i, 6].Value == null ? null : workSheet.Cells[i, 6].Value.ToString());

                            if (YTDExpenses == null)
                            {
                                ucError.ErrorMessage = "YTD Expenses in row " + i.ToString() + " is not in correct format";
                                ucError.Visible = true;
                                return;
                            }

                            decimal? YTDCommittedCost = General.GetNullableDecimal(workSheet.Cells[i, 7].Value == null ? null : workSheet.Cells[i, 7].Value.ToString());

                            if (YTDCommittedCost == null)
                            {
                                ucError.ErrorMessage = "YTD Committed Cost in row " + i.ToString() + " is not in correct format";
                                ucError.Visible = true;
                                return;
                            }
                            decimal? YTDTotalExpense = General.GetNullableDecimal(workSheet.Cells[i, 8].Value == null ? null : workSheet.Cells[i, 8].Value.ToString());

                            if (YTDTotalExpense == null)
                            {
                                ucError.ErrorMessage = "YTD Total Expense in row " + i.ToString() + " is not in correct format";
                                ucError.Visible = true;
                                return;
                            }

                            decimal? YTDBudget = General.GetNullableDecimal(workSheet.Cells[i, 9].Value == null ? null : workSheet.Cells[i, 9].Value.ToString());
                            if (YTDBudget == null)
                            {
                                ucError.ErrorMessage = "YTD Budget in row " + i.ToString() + " is not in correct format";
                                ucError.Visible = true;
                                return;
                            }
                            try
                            {
                                PheonixCommonOpeningBalanceFileUpload.OpeningBalanceBudgetCodeValidate(workSheet.Cells[i, 1].Value.ToString(), i.ToString());
                            }
                            catch (Exception EX)
                            {
                                ucError.ErrorMessage = EX.Message;
                                ucError.Visible = true;
                                return;
                            }
                            cellvalues.Clear();
                        }

                        HttpFileCollection postedFiles = Request.Files;
                        HttpPostedFile postedFile = postedFiles[0];

                        int accountid = Convert.ToInt32(ViewState["VESSELACCOUNTID"].ToString());

                        Guid? uploadid = OpeningBalanceFileUpload(accountid, FileName);

                        PhoenixCommonFileAttachment.InsertAttachment(
                            new Guid(uploadid.ToString()), FileName, filepath, postedFile.ContentLength
                                , 0
                                , 0, "OPENINGBALANCE", new Guid(uploadid.ToString()));

                        if (uploadid == null)
                        {
                            ucError.ErrorMessage = "Document upload failed.";
                            ucError.Visible = true;
                            return;
                        }

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= endRow; i++)
                        {
                            for (int j = workSheet.Dimension.Start.Column; j <= 14; j++)
                            {
                                if (!workSheet.Cells[i, 1].Value.ToString().Equals(""))
                                    cellvalues.Add(Convert.ToString(workSheet.Cells[i, j].Value));
                            }

                            //cellvalues.Add(Convert.ToString(workSheet.Cells[1, 1].Value));
                            OpeningBalanceLineItemInsert(cellvalues, new Guid(uploadid.ToString()),  int.Parse(ViewState["VESSELACCOUNTID"].ToString()));
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

    private void OpeningBalanceLineItemInsert(List<string> li, Guid? uploadid, int accountid)
    {
        try
        {
            PheonixCommonOpeningBalanceFileUpload.OpeningBalanceLineItemInsert(uploadid
                , accountid
                , li[0].ToString()
                , Convert.ToDecimal(li[1].ToString())
                , Convert.ToDecimal(li[2].ToString())
                , Convert.ToDecimal(li[3].ToString())
                , Convert.ToDecimal(li[4].ToString())
                , Convert.ToDecimal(li[5].ToString())
                , Convert.ToDecimal(li[6].ToString())
                , Convert.ToDecimal(li[7].ToString())
                , Convert.ToDecimal(li[8].ToString()));
              
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }
    private Guid? OpeningBalanceFileUpload(int accountid, string FileName)
    {
        Guid? uploadid = null;

        PheonixCommonOpeningBalanceFileUpload.OpeningBalanceFileUploadInsert( FileName, accountid,ref uploadid);

        return uploadid;
    }
    private bool VerifyHeaders(List<string> li)
    {
        if (!li[0].ToString().ToUpper().Equals("BUDGET CODE"))
            return false;
        else if (!li[1].ToString().ToUpper().Equals("ACCUMULATED EXPENSES "))
            return false;
        else if (!li[2].ToString().ToUpper().Equals("ACCUMULATED COMMITTED COST"))
            return false;
        else if (!li[3].ToString().ToUpper().Equals("ACCUMULATED TOTAL EXPENSE"))
            return false;
        else if (!li[4].ToString().ToUpper().Equals("ACCUMULATED BUDGET"))
            return false;
        else if (!li[5].ToString().ToUpper().Equals("YTD EXPENSES"))
            return false;
        else if (!li[6].ToString().ToUpper().Equals("YTD COMMITTED COST"))
            return false;
        else if (!li[7].ToString().ToUpper().Equals("YTD TOTAL EXPENSE"))
            return false;
        else if (!li[8].ToString().ToUpper().Equals("YTD BUDGET"))
            return false;

        else
            return true;
    }

    private void BindData()
    {
        try
        {
            DataSet ds = PheonixCommonOpeningBalanceFileUpload.OpeningBalanceFileUploadList(General.GetNullableInteger(ViewState["VESSELACCOUNTID"].ToString()));

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvAttachment.DataSource = ds;
               
            }
            else if (ds.Tables.Count > 0)
            {
                gvAttachment.DataSource = ds;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["ondblclick"] = "";
    }

  
   
    protected void lnkDownloadExcel_Click(object sender, EventArgs e)
    {
        try
        {
            string file = HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts") + @"\OpeningBalancesUpload.xlsx";
            string destinationpath = HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts\\") + Guid.NewGuid().ToString() + ".xlsx";

            FileInfo fiTemplate = new FileInfo(file);
            fiTemplate.CopyTo(destinationpath);

            FileInfo fidestination = new FileInfo(destinationpath);

            using (ExcelPackage pck = new ExcelPackage(fidestination))
            {
                HttpContext.Current.Response.Clear();
                pck.SaveAs(HttpContext.Current.Response.OutputStream);
                HttpContext.Current.Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12 .xltm";
                HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=OpeningBalancesUpload.xlsx");
                HttpContext.Current.Response.End();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvAttachment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvAttachment_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
       
            if (e.CommandName.ToUpper().Equals("POST"))
            {
                Label fileupload = (Label)e.Item.FindControl("lbluploadedfileid");
                PheonixCommonOpeningBalanceFileUpload.OpeningBalanceFileUploadPost(new Guid(fileupload.Text), new Guid(ViewState["OWNERREPORTID"].ToString()));

                BindData();
                gvAttachment.Rebind();
                ucStatus.Text = "Upload Posted.";

                string script = "javascript:fnReloadList('codehelp1');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PheonixCommonOpeningBalanceFileUpload.OpeningBalanceFileUploadDelete(new Guid(((Label)e.Item.FindControl("lbluploadedfileid")).Text));
                BindData();
                gvAttachment.Rebind();
                ucStatus.Text = "Upload Discared.";
               
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
