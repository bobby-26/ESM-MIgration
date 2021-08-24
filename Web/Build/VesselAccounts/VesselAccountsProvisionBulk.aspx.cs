using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Web;
using System.Web.UI;
using System.IO;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Drawing;

public partial class VesselAccountsProvisionBulk : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Upload", "UPLOAD");
            toolbar.AddButton("Refresh", "REFRESH");
            MenuPRV.AccessRights = this.ViewState;
            MenuPRV.MenuList = toolbar.Show();

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsProvisionBulk.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvStoreItem')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../VesselAccounts/VesselAccountsProvisionBulk.aspx", "Find", "search.png", "FIND");
            MenuStoreItem.AccessRights = this.ViewState;
            MenuStoreItem.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["VALID"] = null;
                EditProvisionDate();
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditProvisionDate()
    {
        DataTable dt = PhoenixVesselAccountsProvision.EditBulkProvision(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            txtClosingDate.Text = dr["FLDCLOSINGDATE"].ToString();
        }

        if (txtClosingDate.Text == null)
        {
            txtClosingDate.Text = DateTime.Now.ToString();
        }
    }
    protected void MenuStoreItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvStoreItem.EditIndex = -1;
                BindData();
            }
            else if (dce.CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;
                string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDOPENINGSTOCK", "FLDPURCHASEDSTOCK", "FLDCLOSINGSTOCK" };
                string[] alCaptions = { "Number", "Name", "Unit", "Opening Stock", "Purchased Stock", "Closing Stock" };

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int sortdirection;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
                else
                    sortdirection = 0;
                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                DataTable dt = PhoenixVesselAccountsProvision.SearchProvisionConsumption(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                           , General.GetNullableDateTime(txtClosingDate.Text)
                                                           , txtNumber.Text, txtName.Text
                                                           , sortexpression, sortdirection
                                                           , 1, iRowCount
                                                           , ref iRowCount, ref iTotalPageCount
                                                           , General.GetNullableInteger("1"));

                ExportExcel("Stock check of Provision Items", dt, alColumns, alCaptions, sortdirection, sortexpression);
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;
            string[] alColumns = { "FLDNUMBER", "FLDNAME", "FLDUNITNAME", "FLDOPENINGSTOCK", "FLDPURCHASEDSTOCK", "FLDCLOSINGSTOCK", "FLDCONSUMEDSTOCK" };
            string[] alCaptions = { "Number", "Name", "Unit", "Opening Stock", "Purchased Stock", "Closing Stock", "Consumption" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixVesselAccountsProvision.SearchBulkProvisionConsumption(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableDateTime(txtClosingDate.Text)
                , txtNumber.Text, txtName.Text
                , sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null),
               ref iRowCount,
               ref iTotalPageCount, General.GetNullableInteger("1"));
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvStoreItem", "Stock check of Provision Items", alCaptions, alColumns, ds);
            if (dt.Rows.Count > 0)
            {
                gvStoreItem.DataSource = dt;
                gvStoreItem.DataBind();
            }
            else
            {
                ShowNoRecordsFound(dt, gvStoreItem);
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvStoreItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
            if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

            if (drv["FLDVALID"] != null)
            {
                ViewState["VALID"] = drv["FLDVALID"].ToString();
            }
        }
    }
    protected void gvStoreItem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvStoreItem_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        BindData();
    }
    protected void gvStoreItem_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            string closingstock = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtClosingStock")).Text;
            string openingstock = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOpeningStock")).Text;
            string purchasequantity = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblPurchaseQuantity")).Text;
            if (!IsValidProvisionConsumption(txtClosingDate.Text, openingstock, closingstock, purchasequantity))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixVesselAccountsProvision.UpdateBulkProvision(PhoenixSecurityContext.CurrentSecurityContext.VesselID, id
                , decimal.Parse(closingstock));
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvStoreItem_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int nCurrentRow = e.RowIndex;
    //    try
    //    {
    //        Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
    //        PhoenixVesselAccountsProvision.DeleteProvisionConsumption(PhoenixSecurityContext.CurrentSecurityContext.VesselID, id
    //            , DateTime.Parse(txtClosingDate.Text));
    //        _gridView.EditIndex = -1;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    protected void txtClosingDate_TextChanged(object sender, EventArgs e)
    {
        gvStoreItem.EditIndex = -1;
        BindData();
    }
    private bool IsValidProvisionConsumption(string date, string openingstock, string closingstock, string purchasequantity)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Closing Date is required.";
        }
        else if (DateTime.TryParse(date, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Closing Date should be earlier than current date";
        }
        if (!General.GetNullableDecimal(closingstock).HasValue)
        {
            ucError.ErrorMessage = "Closing Stock is required.";
        }
        else if (General.GetNullableDecimal(closingstock).Value < 0)
        {
            ucError.ErrorMessage = "Closing Stock cannot be less than 0.";
        }

        if (General.GetNullableDecimal(openingstock).HasValue && General.GetNullableDecimal(purchasequantity).HasValue
            && General.GetNullableDecimal(closingstock).HasValue && decimal.Parse(closingstock) > (decimal.Parse(openingstock) + decimal.Parse(purchasequantity)))
        {
            ucError.ErrorMessage = "Closing Stock should be lesser than Opening Stock.";
        }

        return (!ucError.IsError);
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];


            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }
        BindData();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvStoreItem.SelectedIndex = -1;
        gvStoreItem.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
    }

    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }

    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
        {
            return true;
        }

        return false;
    }

    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    //public static void ShowExcel(string strHeading, DataTable dt, string[] alColumns, string[] alCaptions, int? strSortDirection, string strSortExpression)
    //{

    //    HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + (string.IsNullOrEmpty(strHeading) ? "Attachment" : strHeading.Replace(" ", "_")) + ".xls");
    //    HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF7;
    //    HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    //    HttpContext.Current.Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
    //    HttpContext.Current.Response.Write("<tr>");
    //    for (int i = 0; i < alCaptions.Length; i++)
    //    {
    //        HttpContext.Current.Response.Write("<td width='20%'>");
    //        HttpContext.Current.Response.Write("<b>" + alCaptions[i] + "</b>");
    //        HttpContext.Current.Response.Write("</td>");
    //    }
    //    HttpContext.Current.Response.Write("</tr>");
    //    foreach (DataRow dr in dt.Rows)
    //    {
    //        HttpContext.Current.Response.Write("<tr>");
    //        for (int i = 0; i < alColumns.Length; i++)
    //        {
    //            HttpContext.Current.Response.Write("<td>");
    //            HttpContext.Current.Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
    //            HttpContext.Current.Response.Write("</td>");
    //        }
    //        HttpContext.Current.Response.Write("</tr>");
    //    }
    //    HttpContext.Current.Response.Write("</TABLE>");
    //    HttpContext.Current.Response.End();
    //}
    protected void MenuPRV_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToString().ToUpper() == "UPLOAD")
        {
            try
            {
                PhoenixVesselAccountsProvision.DeleteBulkProvision(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(FileUpload.FileName.ToString());

                if (extension.ToUpper() == ".XLSX")
                {
                    string strpath = HttpContext.Current.Server.MapPath("~/Attachments/VesselAccounts/") + fileName + extension;
                    FileUpload.PostedFile.SaveAs(strpath);
                    bool importstatus = false;
                    CheckImportfile(strpath, ref importstatus);

                    if (importstatus)
                    {
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
                BindData();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        if (dce.CommandName.ToString().ToUpper() == "REFRESH")
        {
            PhoenixVesselAccountsProvision.DeleteBulkProvision(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            BindData();
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
                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                            {
                                cellvalues.Add(Convert.ToString(workSheet.Cells[i, j].Value));
                            }

                            cellvalues.Add(Convert.ToString(workSheet.Cells[1, 1].Value).Trim());
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
            if (li[0].ToString() != "")
            {
                PhoenixVesselAccountsProvision.InsertBulkProvision
                    (
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , li[0].ToString()
                    , decimal.Parse(li[3].ToString())
                    , decimal.Parse(li[4].ToString())
                    , decimal.Parse(li[5].ToString())
                    , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    );
            }
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

    protected void btnBulkProvision_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixVesselAccountsProvision.UpdateBulkProvisionConsumption(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                                            DateTime.Parse(txtClosingDate.Text),
                                                                            General.GetNullableInteger(ViewState["VALID"].ToString()));
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ExportExcel(string strHeading, DataTable dt, string[] alColumns, string[] alCaptions, int? strSortDirection, string strSortExpression)
    {
        int flag;

        List<string> FldColumns = new List<string>();

        foreach (DataColumn column in dt.Columns)
        {
            flag = 0;
            for (int i = 0; i < alColumns.Length; i++)
            {
                if (alColumns[i] == column.ColumnName)
                {
                    flag = 1;
                    column.Caption = alCaptions[i].ToString();
                }
            }
            if (flag == 0)
            {
                FldColumns.Add(column.ColumnName);
            }

        }

        for (int i = 0; i < FldColumns.Count; i++)
        {
            dt.Columns.Remove(FldColumns[i].ToString());
        }

        using (ExcelPackage pck = new ExcelPackage())
        {
            //Create the worksheet
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Provisions");

            //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
            ws.Cells["A1"].LoadFromDataTable(dt, true);

            //prepare the range for the column headers
            string cellRange = "A1:" + Convert.ToChar('A' + dt.Columns.Count - 1) + 1;

            //Format the header for columns
            using (ExcelRange rng = ws.Cells[cellRange])
            {
                rng.Style.WrapText = false;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
                rng.Style.Font.Bold = true;
                rng.Style.Fill.PatternType = ExcelFillStyle.Solid; //Set Pattern for the background to Solid
                rng.Style.Fill.BackgroundColor.SetColor(Color.Gray);
                rng.Style.Font.Color.SetColor(Color.White);
            }

            //prepare the range for the rows
            string rowsCellRange = "A2:" + Convert.ToChar('A' + dt.Columns.Count - 1) + dt.Rows.Count * dt.Columns.Count;

            //Format the rows
            using (ExcelRange rng = ws.Cells[rowsCellRange])
            {
                rng.Style.WrapText = false;
                rng.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            }

            //Read the Excel file in a byte array
            Byte[] fileBytes = pck.GetAsByteArray();

            //Clear the response
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Cookies.Clear();

            //Add the header & other information
            Response.Cache.SetCacheability(HttpCacheability.Private);
            Response.CacheControl = "private";
            Response.Charset = System.Text.UTF8Encoding.UTF8.WebName;
            Response.ContentEncoding = System.Text.UTF8Encoding.UTF8;
            Response.AppendHeader("Content-Length", fileBytes.Length.ToString());
            Response.AppendHeader("Pragma", "cache");
            Response.AppendHeader("Expires", "60");
            Response.AppendHeader("Content-Disposition",
            "attachment; " +
            "filename=\"" + strHeading + ".xlsx\"; " +
            "size=" + fileBytes.Length.ToString() + "; " +
            "creation-date=" + DateTime.Now.ToString("R") + "; " +
            "modification-date=" + DateTime.Now.ToString("R") + "; " +
            "read-date=" + DateTime.Now.ToString("R"));
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

            //Write it back to the client
            Response.BinaryWrite(fileBytes);
            Response.End();
        }
    }

}
