using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using OfficeOpenXml;
using Telerik.Web.UI;

public partial class Accounts_AccountsInvoiceAdminPage : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Upload", "UPLOAD", ToolBarDirection.Right);
        MenuAdminPage.AccessRights = this.ViewState;
        MenuAdminPage.MenuList = toolbar.Show();

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsInvoiceAdminPage.aspx", "Excel", "icon_xls.png", "EXCEL");
        toolbargrid.AddImageButton("../Accounts/AccountsInvoiceAdminPage.aspx", "Find", "search.png", "FIND");
        MenuAdminPageSub.AccessRights = this.ViewState;
        MenuAdminPageSub.MenuList = toolbargrid.Show();

        if (!IsPostBack)
        {

            DateTime now = DateTime.Now;
            //string FromDate = now.Date.AddMonths(-1).ToShortDateString();
            //string ToDate = DateTime.Now.ToShortDateString();
            //ddlYearlist.SelectedText = DateTime.Now.Year.ToString();

            ucFromDate.Text = now.Date.AddMonths(-1).ToShortDateString();
            ucToDate.Text = DateTime.Now.ToShortDateString();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

        BindData();
    }

    protected void MenuAdminPageSub_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {

                TimeSpan t = (Convert.ToDateTime(ucToDate.Text) - Convert.ToDateTime(ucFromDate.Text));
                double Noofdays = t.Days;

                if (General.GetNullableInteger(ucMonthHard.SelectedHard) == null && General.GetNullableInteger(ddlYearlist.SelectedQuick) != null)
                {
                    ucError.ErrorMessage = "Please select month.";
                    ucError.Visible = true;
                    return;
                }
                else if (General.GetNullableInteger(ucMonthHard.SelectedHard) != null && General.GetNullableInteger(ddlYearlist.SelectedQuick) == null)
                {
                    ucError.ErrorMessage = "Please select year.";
                    ucError.Visible = true;
                    return;
                }
                else if (Noofdays > 100)
                {
                    ucError.ErrorMessage = "Selected date range should be within 100 days.";
                    ucError.Visible = true;
                    return;
                }

                ViewState["PAGENUMBER"] = 1;
                BindData();
            }

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucMonthHard_Changed(object sender, EventArgs e)
    {
        //int month = Convert.ToInt32(ucMonthHard.SelectedHard);
        DataTable dt = new DataTable();
        dt = PhoenixAccountsInvoice.GetMonthForInvoive(General.GetNullableInteger(ddlYearlist.SelectedQuick.ToString()),
                                                        General.GetNullableInteger(ucMonthHard.SelectedHard.ToString()));

        ucFromDate.Text = dt.Rows[0]["FLDFROMDATE"].ToString();
        ucToDate.Text = dt.Rows[0]["FLDTODATE"].ToString();
        //DateTime.Now.Date(Convert.ToInt32((dt.Rows[0]["FLDSHORTNAME"].ToString()))).ToShortDateString();
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();

            //ds = PhoenixAccountsInvoice.InvoiceAdminPageSearch(ucFromDate.Text != null ? General.GetNullableDateTime(ucFromDate.Text) : General.GetNullableDateTime(ViewState["FROMDATE"].ToString()),
            //    ucToDate.Text != null ? General.GetNullableDateTime(ucToDate.Text) : General.GetNullableDateTime(ViewState["TODATE"].ToString()),
            //    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            //    General.ShowRecords(null),
            //    ref iRowCount, ref iTotalPageCount, txtInvoiceRefNumber.Text, General.GetNullableInteger(ucHard.SelectedHard.ToString()));


            ds = PhoenixAccountsInvoice.InvoiceAdminPageSearch(General.GetNullableDateTime(ucFromDate.Text),
                General.GetNullableDateTime(ucToDate.Text),
                Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                gvAttachment.PageSize,
                ref iRowCount, ref iTotalPageCount, txtInvoiceRefNumber.Text, General.GetNullableInteger(ucHard.SelectedHard.ToString()));


            gvAttachment.DataSource = ds;
            gvAttachment.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDINVOICECODE", "FLDINVOICENUMBER", "FLDHARDNAME", "FLDPICNAME" };
        string[] alCaptions = { "Invoice Code", "Invoice Number", "Status", "PIC" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsInvoice.InvoiceAdminPageSearch(General.GetNullableDateTime(ucFromDate.Text), General.GetNullableDateTime(ucToDate.Text),
           Int32.Parse(ViewState["PAGENUMBER"].ToString()),
           PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
           ref iRowCount, ref iTotalPageCount, txtInvoiceRefNumber.Text);


        Response.AddHeader("Content-Disposition", "attachment; filename=InvoiceCode.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write(alCaptions[i]);
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuAdminPage_OnTabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToString().ToUpper() == "UPLOAD")
        {
            try
            {
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(FileUpload.FileName.ToString());

                if (extension.ToUpper() == ".XLSX")
                {
                    string strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + "Accounts/" + fileName + extension;
                    //string strpath = HttpContext.Current.Server.MapPath("~/Attachments/Accounts/") + fileName + extension;
                    FileUpload.PostedFile.SaveAs(strpath);
                    bool importstatus = false;
                    CheckImportfile(strpath, ref importstatus);

                    if (importstatus)
                    {
                        AccountStatusUpdate(strpath, fileName);
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

    public void AccountStatusUpdate(string filepath, string FileGuidName)
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
                                //endRow = i;
                                break;
                            }
                            endRow = i;
                        }

                        for (int j = workSheet.Dimension.Start.Column; j <= 1; j++)
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

                        if (!IsValidSelection())
                        {
                            ucError.Visible = true;
                            return;
                        }

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= endRow; i++)
                        {
                            if (!workSheet.Cells[i, 1].Value.ToString().Equals(""))
                                cellvalues.Add(Convert.ToString(workSheet.Cells[i, 1].Value));

                            UpdateInvoiceDetails(cellvalues);

                            cellvalues.Clear();
                        }

                        ucStatus.Text = "Invoice Updated";
                        ucStatus.Visible = true;
                        BindData();
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

    private bool VerifyHeaders(List<string> li)
    {
        if (!li[0].ToString().ToUpper().Equals("INVOICE CODE"))
            return false;
        else
            return true;
    }

    private void UpdateInvoiceDetails(List<string> li)
    {
        try
        {
            PhoenixAccountsInvoice.InvoiceBatchUpdate(new Guid(li[0].ToString()), General.GetNullableInteger(ucHard.SelectedHard), General.GetNullableInteger(ddlUser.SelectedUser));
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidSelection()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ddlUser.SelectedUser) == null && General.GetNullableInteger(ucHard.SelectedHard) == null)
            ucError.ErrorMessage = "Select Either User or Status";

        return (!ucError.IsError);
    }


    protected void gvAttachment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            if (((UserControlHard)e.Item.FindControl("ucHard")).SelectedHard.ToString().ToUpper().Equals("DUMMY"))
            {
                ucError.ErrorMessage = "Status is required.";
                ucError.Visible = true;
                return;
            }

            PhoenixAccountsInvoice.InvoiceStatusUpdate(new Guid(((RadLabel)e.Item.FindControl("lblInvoiceCodeEdit")).Text),
                General.GetNullableInteger(((UserControlHard)e.Item.FindControl("ucHard")).SelectedHard),
                General.GetNullableInteger(((UserControlUserName)e.Item.FindControl("ddlUser")).SelectedUser)
                );

            Rebind();
        }
        if (e.CommandName.ToUpper().Equals("VESSELLIST"))
        {
        }
    }

    protected void gvAttachment_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                UserControlHard uchard = (UserControlHard)e.Item.FindControl("ucHard");
                if (uchard != null)
                    uchard.HardList = PhoenixRegistersHard.ListHard(1, 60);

                UserControlUserName ucName = (UserControlUserName)e.Item.FindControl("ddlUser");
                if (ucName != null)
                    ucName.UserNameList = PhoenixUser.UserList(null, null);

                DataRowView drv = (DataRowView)e.Item.DataItem;

                if (drv["FLDINVOICESTATUS"].ToString() != null)
                {
                    if (uchard != null)
                        uchard.SelectedHard = drv["FLDINVOICESTATUS"].ToString();
                }

                if (drv["FLDPIC"].ToString() != null)
                {
                    if (ucName != null)
                        ucName.SelectedUser = drv["FLDPIC"].ToString();
                }

                ImageButton cmdEdit = (ImageButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;

                ImageButton imgVsl = (ImageButton)e.Item.FindControl("cmdVesselList");
                RadLabel lblInvoiceCode = (RadLabel)e.Item.FindControl("lblInvoiceCode");
                if (imgVsl != null && lblInvoiceCode != null && lblInvoiceCode.Text != "")
                    imgVsl.Attributes.Add("onclick", "openNewWindow('Filter', '','" + Session["sitepath"] + "/Accounts/AccountsInvoiceVesselList.aspx?&INVOICECODE=" + lblInvoiceCode.Text.Trim() + "', 'medium')");
            }
        }
    }


    protected void gvAttachment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAttachment.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvAttachment.SelectedIndexes.Clear();
        gvAttachment.EditIndexes.Clear();
        gvAttachment.DataSource = null;
        gvAttachment.Rebind();
    }
}
