using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using System.Data;
using System.IO;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using OfficeOpenXml;
using System.Data.SqlClient;

public partial class Accounts_AccountsUpdateVoucherList : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Upload", "UPLOAD", ToolBarDirection.Right);
        MenuAdminPage.AccessRights = this.ViewState;
        MenuAdminPage.MenuList = toolbar.Show();

        if (!IsPostBack)
        {

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["lblattacmentid"] = null;

            gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }
    protected void gvAttachment_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                LinkButton cmdupdate = (LinkButton)e.Item.FindControl("cmdupdate");
                if (cmdupdate != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdupdate.CommandName)) cmdupdate.Visible = false;
                LinkButton cmdDelete = (LinkButton)e.Item.FindControl("cmdDelete");
                if (cmdDelete != null)
                    if (!SessionUtil.CanAccess(this.ViewState, cmdDelete.CommandName)) cmdDelete.Visible = false;
            }
        }
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            ds = PhoenixAccountsVoucherList.AttachmentSearch(null, sortexpression, sortdirection,
                                                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                        General.ShowRecords(null),
                                                                        ref iRowCount, ref iTotalPageCount);


            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ViewState["lblattacmentid"] == null)
                {
                    ViewState["lblattacmentid"] = ds.Tables[0].Rows[0]["FLDATTACHMENTID"].ToString();
                }
            }
            else { }
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
    protected void gvAttachment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            ViewState["lblattacmentid"] = ((RadLabel)e.Item.FindControl("lblattacmentid")).Text;

            PhoenixAccountsVoucherList.VoucherdeleteFileDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableGuid(ViewState["lblattacmentid"].ToString()));
            BindDataNotBalancedList();
            BindLockPeriodList();
            BindDataNAAaccountList();
            BindDataBilledList();
            BindDataRateList();
            gvvouchernotbalanced.Rebind();
            gvlockperiod.Rebind();
            gvNAAaccount.Rebind();
            gvvoucherbilled.Rebind();
            gvrate.Rebind();
        }
        Rebind();
        if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            try
            {
                ViewState["lblattacmentid"] = ((RadLabel)e.Item.FindControl("lblattacmentid")).Text;

                BindDataNotBalancedList();
                BindLockPeriodList();
                BindDataNAAaccountList();
                BindDataBilledList();
                BindDataRateList();
                gvvouchernotbalanced.Rebind();
                gvlockperiod.Rebind();
                gvNAAaccount.Rebind();
                gvvoucherbilled.Rebind();
                gvrate.Rebind();
                if (!IsValidData())
                {
                    string errormessage = "";
                    errormessage = ucError.ErrorMessage;
                    errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                    RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                    return;
                }
                DataSet ds = new DataSet();
                ds = PhoenixAccountsVoucherList.VoucherListUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["lblattacmentid"].ToString()));
                BindDataNotBalancedList();
                BindLockPeriodList();
                BindDataNAAaccountList();
                BindDataBilledList();
                BindDataRateList();
                gvvouchernotbalanced.Rebind();
                gvlockperiod.Rebind();
                gvNAAaccount.Rebind();
                gvvoucherbilled.Rebind();
                gvrate.Rebind();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["COUNT"] = dr["UPDATED"].ToString();
                }
                Rebind();
                if (!Count())
                {
                    string errormessage = "";
                    errormessage = ucError.ErrorMessage;
                    errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                    RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                    return;
                }

            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }

        }
    }


    protected bool IsValidData()
    {
        ucError.HeaderMessage = "Please Verify Data and Update.";
        ucError.clear = "";
        if (lblvouchernotbalancedcount.Text != "0")
            ucError.ErrorMessage = "Voucher not balanced Count must be 0.";
        if (lbllockperiodcount.Text != "0")
            ucError.ErrorMessage = "Voucher belong to locked period Count must be 0.";
        if (lblInACodeCount.Text != "0")
            ucError.ErrorMessage = "Account Code not active for some voucher.";
        if (lblbilledcount.Text != "0")
            ucError.ErrorMessage = "Voucher Rows are billed count must be 0.";
        if (lblratemissmatchcount.Text != "0")
            ucError.ErrorMessage = "Rates entered mismatch count must be 0.";

        return (!ucError.IsError);
    }
    protected bool Count()
    {
        ucError.HeaderMessage = "COUNT";
        ucError.clear = "";
        if (ViewState["COUNT"].ToString() != "")
            ucError.ErrorMessage = "<b>COUNT!!!</b><br><br>Total number of Voucher updated Successfully:<br><br>" + ViewState["COUNT"].ToString();
        return (!ucError.IsError);
    }
    private void CheckImportfile(string filepath, ref bool importstatus)
    {
        if (File.Exists(filepath))
            importstatus = true;
        else
            importstatus = false;
    }
    protected void MenuAdminPage_OnTabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToString().ToUpper() == "UPLOAD")
            {
                try
                {
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(FileUpload.FileName.ToString());

                    if (extension.ToUpper() == ".XLSX")
                    {
                        string strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts/" + fileName + extension;

                        FileUpload.PostedFile.SaveAs(strpath);
                        bool importstatus = false;
                        CheckImportfile(strpath, ref importstatus);

                        if (importstatus)
                        {
                            AccountStatusUpdate(strpath, fileName);
                        }
                        else
                        {
                            ucError.ErrorMessage = "Please upload correct file .";
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
                    Rebind();
                }

                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                }
            }
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
    private bool VerifyHeaders(List<string> li)
    {

        if (!li[0].ToString().ToUpper().Equals("VESSEL"))
            return false;
        else if (!li[1].ToString().ToUpper().Equals("VOUCHERNUMBER"))
            return false;
        else if (!li[2].ToString().ToUpper().Equals("VOUCHERDATE"))
            return false;
        else if (!li[3].ToString().ToUpper().Equals("CUR"))
            return false;
        else if (!li[4].ToString().ToUpper().Equals("VOUCHERID"))
            return false;
        else if (!li[5].ToString().ToUpper().Equals("ACCOUNTID"))
            return false;
        else if (!li[6].ToString().ToUpper().Equals("CURRENCYID"))
            return false;
        else if (!li[7].ToString().ToUpper().Equals("BASE RATE"))
            return false;
        else if (!li[8].ToString().ToUpper().Equals("REPORT RATE"))
            return false;
        else
            return true;
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

                        for (int i = 11; i <= workSheet.Dimension.End.Row; i++)
                        {
                            if (workSheet.Cells[i, 1].Value == null)
                            {
                                //endRow = i;
                                break;
                            }
                            endRow = i;
                        }
                        for (int j = workSheet.Dimension.Start.Column; j <= 11; j++)
                        {
                            if ((workSheet.Cells[11, j].Value != null) || (workSheet.Cells[11, 8].Value == null) || (workSheet.Cells[11, 9].Value == null))
                            {
                                cellvalues.Add(Convert.ToString(workSheet.Cells[11, j].Value));
                            }

                        }
                        //if (!VerifyHeaders(cellvalues))
                        //{
                        //    ucError.ErrorMessage = "File is of incorrect format";
                        //    ucError.Visible = true;
                        //    return;
                        //}

                        cellvalues.Clear();

                        for (int i = 11; i <= endRow; i++)
                        {
                            for (int j = workSheet.Dimension.Start.Column; j <= 11; j++)
                            {
                                if (workSheet.Cells[i, 1].Value != null)
                                    cellvalues.Add(Convert.ToString(workSheet.Cells[i, j].Value));
                            }
                        }

                        HttpFileCollection postedFiles = Request.Files;
                        HttpPostedFile postedFile = postedFiles[0];

                        string filename = postedFile.FileName;
                        if (filename.LastIndexOfAny(new[] { '/', '\\' }) > 0)
                        {
                            filename = filename.Substring(filename.LastIndexOfAny(new[] { '/', '\\' }) + 1);
                        }
                        filepath = "ACCOUNTS/" + FileGuidName + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                        Guid? ListInsertId = VoucherListInsert(filename);

                        PhoenixCommonFileAttachment.InsertAttachment(
                            new Guid(ListInsertId.ToString()), filename, filepath, postedFile.ContentLength
                                , 0
                                , 0, "VOUCHERUPLOAD", new Guid(ListInsertId.ToString()));

                        if (ListInsertId == null)
                        {
                            ucError.ErrorMessage = "Document upload failed.";
                            ucError.Visible = true;
                            return;
                        }

                        for (int i = 11; i <= endRow; i++)
                        {
                            for (int j = workSheet.Dimension.Start.Column; j <= 11; j++)
                            {
                                if (workSheet.Cells[i, 1].Value != null)
                                    cellvalues.Add(Convert.ToString(workSheet.Cells[i, j].Value));
                            }

                            InserVoucherListLineItems(cellvalues, new Guid(ListInsertId.ToString()), i);
                            cellvalues.Clear();
                        }

                        ucStatus.Text = "File Updated";
                        ucStatus.Visible = true;

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
    private void InserVoucherListLineItems(List<string> li, Guid ListInsertId, int rowno)
    {
        try
        {
            PhoenixAccountsVoucherList.VoucherListAttachmentLineItemInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , ListInsertId
                , rowno
                , General.GetNullableString(li[0].ToString())
                , General.GetNullableString(li[1].ToString())
                , General.GetNullableDateTime(li[2].ToString())
                , General.GetNullableString(li[3].ToString())
                , General.GetNullableInteger(li[4].ToString())
                , General.GetNullableInteger(li[5].ToString())
                , General.GetNullableInteger(li[6].ToString())
                , General.GetNullableDateTime(li[7].ToString())
                , General.GetNullableString(li[8].ToString())
                , General.GetNullableDecimal(li[9].ToString())
                , General.GetNullableDecimal(li[10].ToString())
                );
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }
    private Guid? VoucherListInsert(string filename)
    {
        Guid? ListInsertId = null;

        PhoenixAccountsVoucherList.VoucherListAttachmentInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, filename, ref ListInsertId);

        return ListInsertId;
    }


    protected void gvvouchernotbalanced_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvvouchernotbalanced.CurrentPageIndex + 1;
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            ds = PhoenixAccountsVoucherList.AttachmentSearch(null, sortexpression, sortdirection,
                                                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                        General.ShowRecords(null),
                                                                        ref iRowCount, ref iTotalPageCount);


            if (ds.Tables[0].Rows.Count > 0)
            {
                BindDataNotBalancedList();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataNotBalancedList()
    {
        try
        {

            DataSet ds = new DataSet();

            ds = PhoenixAccountsVoucherList.NotBalancedlist(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 General.GetNullableGuid(ViewState["lblattacmentid"].ToString()));

            gvvouchernotbalanced.DataSource = ds.Tables[0];
            gvvouchernotbalanced.VirtualItemCount = ds.Tables[0].Rows.Count;
            ViewState["NROWCOUNT"] = gvvouchernotbalanced.VirtualItemCount;
            lblvouchernotbalancedcount.Text = ViewState["NROWCOUNT"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    protected void gvlockperiod_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvlockperiod.CurrentPageIndex + 1;
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            ds = PhoenixAccountsVoucherList.AttachmentSearch(null, sortexpression, sortdirection,
                                                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                        General.ShowRecords(null),
                                                                        ref iRowCount, ref iTotalPageCount);


            if (ds.Tables[0].Rows.Count > 0)
            {
                BindLockPeriodList();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindLockPeriodList()
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixAccountsVoucherList.LockPeriod(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 General.GetNullableGuid(ViewState["lblattacmentid"].ToString()));

            gvlockperiod.DataSource = ds;
            gvlockperiod.VirtualItemCount = ds.Tables[0].Rows.Count;

            ViewState["FROWCOUNT"] = gvlockperiod.VirtualItemCount;
            lbllockperiodcount.Text = ViewState["FROWCOUNT"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

    protected void gvNAAaccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvNAAaccount.CurrentPageIndex + 1;

            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            ds = PhoenixAccountsVoucherList.AttachmentSearch(null, sortexpression, sortdirection,
                                                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                        General.ShowRecords(null),
                                                                        ref iRowCount, ref iTotalPageCount);


            if (ds.Tables[0].Rows.Count > 0)
            {
                BindDataNAAaccountList();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataNAAaccountList()
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixAccountsVoucherList.AccountcodeInActive(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 General.GetNullableGuid(ViewState["lblattacmentid"].ToString()));

            gvNAAaccount.DataSource = ds;
            gvNAAaccount.VirtualItemCount = ds.Tables[0].Rows.Count;

            ViewState["CROWCOUNT"] = gvNAAaccount.VirtualItemCount;
            lblInACodeCount.Text = ViewState["CROWCOUNT"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    protected void gvvoucherbilled_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvvoucherbilled.CurrentPageIndex + 1;

            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            ds = PhoenixAccountsVoucherList.AttachmentSearch(null, sortexpression, sortdirection,
                                                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                        General.ShowRecords(null),
                                                                        ref iRowCount, ref iTotalPageCount);


            if (ds.Tables[0].Rows.Count > 0)
            {
                BindDataBilledList();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataBilledList()
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixAccountsVoucherList.Voucherbilled(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 General.GetNullableGuid(ViewState["lblattacmentid"].ToString()));

            gvvoucherbilled.DataSource = ds;
            gvvoucherbilled.VirtualItemCount = ds.Tables[0].Rows.Count;

            ViewState["CROWCOUNT"] = gvvoucherbilled.VirtualItemCount;
            lblbilledcount.Text = ViewState["CROWCOUNT"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    protected void gvrate_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvrate.CurrentPageIndex + 1;

            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            ds = PhoenixAccountsVoucherList.AttachmentSearch(null, sortexpression, sortdirection,
                                                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                        General.ShowRecords(null),
                                                                        ref iRowCount, ref iTotalPageCount);


            if (ds.Tables[0].Rows.Count > 0)
            {
                BindDataRateList();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataRateList()
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixAccountsVoucherList.Ratecheck(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 General.GetNullableGuid(ViewState["lblattacmentid"].ToString()));

            gvrate.DataSource = ds;
            gvrate.VirtualItemCount = ds.Tables[0].Rows.Count;

            ViewState["CROWCOUNT"] = gvrate.VirtualItemCount;
            lblratemissmatchcount.Text = ViewState["CROWCOUNT"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

}
