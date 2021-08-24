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

public partial class Accounts_AccountsCommittedCostPOBulkExclude : System.Web.UI.Page
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


            ds = PhoenixAccountsCommittedcostBulkPO.AttachmentSearch(null, sortexpression, sortdirection,
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

        //if (e.CommandName.ToUpper().Equals("EDIT"))
        //{
        //    ViewState["lblattacmentid"] = ((RadLabel)e.Item.FindControl("lblattacmentid")).Text;
        //}

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            ViewState["lblattacmentid"] = ((RadLabel)e.Item.FindControl("lblattacmentid")).Text;

            PhoenixAccountsCommittedcostBulkPO.POBulkExcludeFileDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            General.GetNullableGuid(ViewState["lblattacmentid"].ToString()));
            BindDataClosedPOList();
            BindDataFailedPOList();
            BindDataNotFoundPOList();
            gvpoclosed.Rebind();
            gvpofailed.Rebind();
            gvponotfound.Rebind();
        }
        Rebind();
        if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            try
            {
                ViewState["lblattacmentid"] = ((RadLabel)e.Item.FindControl("lblattacmentid")).Text;

                if (!IsValidDate())
                {
                    string errormessage = "";
                    errormessage = ucError.ErrorMessage;
                    errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                    RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                    return;
                }
                BindDataClosedPOList();
                BindDataFailedPOList();
                BindDataNotFoundPOList();
                gvpoclosed.Rebind();
                gvpofailed.Rebind();
                gvponotfound.Rebind();

                if (!IsValidData())
                {
                    string errormessage = "";
                    errormessage = ucError.ErrorMessage;
                    errormessage = errormessage.Replace("<font color='#ff0000'>", "").Replace("</font>", "");
                    RadWindowManager1.RadAlert(errormessage, 400, 150, "Message", null);
                    return;
                }
                DataSet ds = new DataSet();
                ds = PhoenixAccountsCommittedcostBulkPO.POBulkexcludeUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["lblattacmentid"].ToString()), General.GetNullableDateTime(ucExcludeDate.Text));
                BindDataClosedPOList();
                BindDataFailedPOList();
                BindDataNotFoundPOList();
                gvpoclosed.Rebind();
                gvpofailed.Rebind();
                gvponotfound.Rebind();

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    ViewState["COUNT"] = dr["DELETED"].ToString();
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
                //RadWindowManager1.RadConfirm("<b>COUNT!!!</b><br><br>Total number of PO Excluded Successfully:<br><br>" + ViewState["COUNT"].ToString(), "confirm", 320, 180, null, "Information");
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

    protected bool IsValidDate()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        ucError.clear = "";
        if (ucExcludeDate.Text == null)
            ucError.ErrorMessage = "Date is required.";
        return (!ucError.IsError);
    }
    protected bool IsValidData()
    {
        ucError.HeaderMessage = "Please Verify Data and Update.";
        ucError.clear = "";
        if (lblclosedcount.Text != "0")
            ucError.ErrorMessage = "Closed PO Count must be 0.";
        if (lblfailedcount.Text != "0")
            ucError.ErrorMessage = "Failed PO Count must be 0.";
        if (lblnotfouncount.Text != "0")
            ucError.ErrorMessage = "PO cannot be found  Count must be 0.";
        return (!ucError.IsError);
    }
    protected bool Count()
    {
        ucError.HeaderMessage = "COUNT";
        ucError.clear = "";
        if (ViewState["COUNT"].ToString() != "")
            ucError.ErrorMessage = "<b>COUNT!!!</b><br><br>Total number of PO Excluded Successfully:<br><br>" + ViewState["COUNT"].ToString();
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
    protected void Rebind()
    {
        gvAttachment.SelectedIndexes.Clear();
        gvAttachment.EditIndexes.Clear();
        gvAttachment.DataSource = null;
        gvAttachment.Rebind();
    }
    private bool VerifyHeaders(List<string> li)
    {
        if (!li[0].ToString().ToUpper().Equals("PO NUMBER"))
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

                        for (int i = workSheet.Dimension.Start.Row + 1; i <= endRow; i++)
                        {
                            if (!workSheet.Cells[i, 1].Value.ToString().Equals(""))
                                cellvalues.Add(Convert.ToString(workSheet.Cells[i, 1].Value));

                        }

                        HttpFileCollection postedFiles = Request.Files;
                        HttpPostedFile postedFile = postedFiles[0];

                        string filename = postedFile.FileName;
                        if (filename.LastIndexOfAny(new[] { '/', '\\' }) > 0)
                        {
                            filename = filename.Substring(filename.LastIndexOfAny(new[] { '/', '\\' }) + 1);
                        }
                        filepath = "ACCOUNTS/" + FileGuidName + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));

                        Guid? BulkExcludeInsertId = BulkExcludeInsert(filename);

                        PhoenixCommonFileAttachment.InsertAttachment(
                            new Guid(BulkExcludeInsertId.ToString()), filename, filepath, postedFile.ContentLength
                                , 0
                                , 0, "VOUCHERUPLOAD", new Guid(BulkExcludeInsertId.ToString()));

                        if (BulkExcludeInsertId == null)
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

                            InserBulkExcludeLineItems(cellvalues, new Guid(BulkExcludeInsertId.ToString()), i);
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
    private void InserBulkExcludeLineItems(List<string> li, Guid BulkExcludeInsertId, int rowno)
    {
        try
        {
            PhoenixAccountsCommittedcostBulkPO.POListAttachmentLineItemInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , BulkExcludeInsertId
                , rowno
                , li[0].ToString()     //PO Number
                );
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }
    private Guid? BulkExcludeInsert(string filename)
    {
        Guid? BulkExcludeInsertId = null;

        PhoenixAccountsCommittedcostBulkPO.POListAttachmentInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, filename, ref BulkExcludeInsertId);

        return BulkExcludeInsertId;
    }


    protected void gvponotfound_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvponotfound.CurrentPageIndex + 1;
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            ds = PhoenixAccountsCommittedcostBulkPO.AttachmentSearch(null, sortexpression, sortdirection,
                                                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                        General.ShowRecords(null),
                                                                        ref iRowCount, ref iTotalPageCount);


            if (ds.Tables[0].Rows.Count > 0)
            {
                BindDataNotFoundPOList();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataNotFoundPOList()
    {
        try
        {

            DataSet ds = new DataSet();

            ds = PhoenixAccountsCommittedcostBulkPO.PONotfound(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 General.GetNullableGuid(ViewState["lblattacmentid"].ToString()), General.GetNullableDateTime(ucExcludeDate.Text));

            gvponotfound.DataSource = ds.Tables[0];
            gvponotfound.VirtualItemCount = ds.Tables[0].Rows.Count;
            ViewState["NROWCOUNT"] = gvponotfound.VirtualItemCount;
            lblnotfouncount.Text = ViewState["NROWCOUNT"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    protected void gvpofailed_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvpofailed.CurrentPageIndex + 1;
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            ds = PhoenixAccountsCommittedcostBulkPO.AttachmentSearch(null, sortexpression, sortdirection,
                                                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                        General.ShowRecords(null),
                                                                        ref iRowCount, ref iTotalPageCount);


            if (ds.Tables[0].Rows.Count > 0)
            {
                BindDataFailedPOList();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataFailedPOList()
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixAccountsCommittedcostBulkPO.POFailed(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 General.GetNullableGuid(ViewState["lblattacmentid"].ToString()), General.GetNullableDateTime(ucExcludeDate.Text));

            gvpofailed.DataSource = ds;
            gvpofailed.VirtualItemCount = ds.Tables[0].Rows.Count;

            ViewState["FROWCOUNT"] = gvpofailed.VirtualItemCount;
            lblfailedcount.Text = ViewState["FROWCOUNT"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

    protected void gvpoclosed_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvpoclosed.CurrentPageIndex + 1;

            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            ds = PhoenixAccountsCommittedcostBulkPO.AttachmentSearch(null, sortexpression, sortdirection,
                                                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                        General.ShowRecords(null),
                                                                        ref iRowCount, ref iTotalPageCount);


            if (ds.Tables[0].Rows.Count > 0)
            {
                BindDataClosedPOList();
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindDataClosedPOList()
    {
        try
        {
            DataSet ds = new DataSet();

            ds = PhoenixAccountsCommittedcostBulkPO.POClosed(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 General.GetNullableGuid(ViewState["lblattacmentid"].ToString()), General.GetNullableDateTime(ucExcludeDate.Text));

            gvpoclosed.DataSource = ds;
            gvpoclosed.VirtualItemCount = ds.Tables[0].Rows.Count;

            ViewState["CROWCOUNT"] = gvpoclosed.VirtualItemCount;
            lblclosedcount.Text = ViewState["CROWCOUNT"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

}
