using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Collections.Generic;



public partial class AccountsBankFileUpload : PhoenixBasePage
{

    string attachmentcode = string.Empty;
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvAttachment.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl00");
                Page.ClientScript.RegisterForEventValidation
                        (r.UniqueID + "$ctl01");
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Upload", "UPLOAD");
        AttachmentList.AccessRights = this.ViewState;
        AttachmentList.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["FILEUPLOADID"] = null;

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
        }
        BindData();
        SetPageNavigator();
    }

    protected void AttachmentList_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToString().ToUpper() == "UPLOAD")
        {
            try
            {
                if (!IsValidFile())
                {
                    ucError.Visible = true;
                    return;
                }
                string fileName = Path.GetFileName(txtFileUpload.FileName.ToString());
                if (fileName.Substring(fileName.IndexOf('.'), 5).ToUpper() == ".XLSX")
                {
                    string strpath = HttpContext.Current.Server.MapPath("~/Attachments/ACCOUNTS/") + fileName;
                    txtFileUpload.PostedFile.SaveAs(strpath);
                    bool importstatus = false;
                    CheckImportBankfile(strpath, ref importstatus);
                    if (importstatus)
                    {
                        string iFileUploadid = "";
                        PhoenixAccountsBankSupplierPayment.InsertBankFileUpload(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                         fileName, 1, "", ref iFileUploadid);
                        ViewState["FILEUPLOADID"] = iFileUploadid;
                        //ImportBankfile(strpath);
                        ExchangeDiffUpdate(strpath);
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
                    ucError.ErrorMessage = "Please upload .XLSX file only";
                    ucError.Visible = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
            BindData();
            SetPageNavigator();
        }
    }

    private void CheckImportBankfile(string filepath, ref bool importstatus)
    {
        //using (StreamReader sr = new StreamReader(filepath))
        //{
        //    string line;
        //    line = sr.ReadLine();
        //    string[] colstr = line.Split(',');
        //    if (line != null && colstr.Length == 47)
        //    {
        //        importstatus = true;
        //    }
        //}

        if (File.Exists(filepath))
            importstatus = true;
        else
            importstatus = false;

    }
    private void ImportBankfile(string filepath)
    {
        try
        {
            using (StreamReader sr = new StreamReader(filepath))
            {
                string line;
                bool firstrow = true;
                string ttreferenceno = ""; string debitexchangerate = ""; string paymentvaluedate = ""; string bankreferenceno = ""; string debitamount = "";


                while ((line = sr.ReadLine()) != null)
                {
                    string[] colstr = line.Split(',');
                    ttreferenceno = colstr[1].ToString();
                    debitamount = colstr[4].ToString();
                    debitexchangerate = colstr[5].ToString();
                    paymentvaluedate = colstr[8].ToString();
                    bankreferenceno = colstr[2].ToString();

                    if (!firstrow && ttreferenceno != "")
                    {
                        PhoenixAccountsBankSupplierPayment.UpdateBankSupplierPayment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ViewState["FILEUPLOADID"].ToString()
                        , ttreferenceno, bankreferenceno, General.GetNullableDecimal(debitamount), Convert.ToDecimal(debitexchangerate)
                        , General.GetNullableDateTime(paymentvaluedate));
                    }
                    firstrow = false;
                }
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
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsBankSupplierPayment.BankFileUploadSearch(null, null, null, null, sortexpression, sortdirection,
                                                                       Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                       General.ShowRecords(null),
                                                                       ref iRowCount, ref iTotalPageCount);

        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            gvAttachment.DataSource = ds;
            gvAttachment.DataBind();
        }
        else if (ds.Tables.Count > 0)
        {
            ShowNoRecordsFound(ds.Tables[0], gvAttachment);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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

    protected void gvAttachment_RowEditing(object sender, GridViewEditEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            if (_gridView.EditIndex > -1)
                _gridView.UpdateRow(_gridView.EditIndex, false);
            _gridView.EditIndex = e.NewEditIndex;
            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAttachment_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }

    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            // Get the LinkButton control in the first cell
            LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            if (!SessionUtil.CanAccess(this.ViewState, _doubleClickButton.CommandName)) _doubleClickButton.Visible = false;
            // Get the javascript which is assigned to this LinkButton
            string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            // Add this javascript to the onclick Attribute of the row
            e.Row.Attributes["ondblclick"] = _jsDouble;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
              //  if (!SessionUtil.CanAccess(this.ViewState, ed.CommandName)) ed.Visible = false;
            }
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                ImageButton cmdEdit = (ImageButton)e.Row.FindControl("cmdView");
                if (!SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName)) cmdEdit.Visible = false;
                Label lbluploadedfileid = (Label)e.Row.FindControl("lbluploadedfileid");
                cmdEdit.Attributes.Add("onclick", "Openpopup('AddAddress', '', 'AccountsBankFileUploadLineItem.aspx?UPLOADEDFILEID=" + lbluploadedfileid.Text + "&MODE=EDIT'); return false;");
            }
        }

        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
    }

    protected void gvAttachment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            string lbluploadedfileid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lbluploadedfileid")).Text;
            PhoenixAccountsBankSupplierPayment.DeleteBankFileUpload(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , lbluploadedfileid);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        BindData();
        SetPageNavigator();
    }

    protected void gvAttachment_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvAttachment_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        SetPageNavigator();
        BindData();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        try
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
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        try
        {
            gvAttachment.SelectedIndex = -1;
            gvAttachment.EditIndex = -1;
            if (ce.CommandName == "prev")
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
            else
                ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

            BindData();
            SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetPageNavigator()
    {
        try
        {
            cmdPrevious.Enabled = IsPreviousEnabled();
            cmdNext.Enabled = IsNextEnabled();
            lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
            lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
            lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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


    public bool IsValidFile()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (txtFileUpload.PostedFile.FileName.Trim().Equals(""))
            ucError.ErrorMessage = "Please select a file.";
        return (!ucError.IsError);
    }

    private void ExchangeDiffUpdate(string filepath)
    {
        try
        {
            //using (StreamReader sr = new StreamReader(filepath))
            //{
            //    string line;
            //    bool firstrow = true;
            //    string remittanceno = "";


            //    while ((line = sr.ReadLine()) != null)
            //    {
            //        string[] colstr = line.Split(',');
            //        remittanceno = colstr[46].ToString();

            //        if (!firstrow && remittanceno != "")
            //        {
            //            PhoenixAccountsBankSupplierPayment.ExchangeRateDiffUpdate(remittanceno,remittanceno);
            //        }  
            //        firstrow = false;
            //    }
            //}

            List<string> cellvalues = new List<string>();

            var package = new ExcelPackage(new FileInfo(filepath));

            //if (File.Exists(@filepath))
            //{
            //    File.Delete(@filepath);
            //}

            ExcelWorksheets worksheets = package.Workbook.Worksheets;

            if (worksheets.Count > 0)
            {
                foreach (ExcelWorksheet workSheet in worksheets)
                {
                    if (workSheet.Dimension != null)
                    {
                        for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                        {
                            string remittenceno = "";
                            string ttreferenceno = "";
                            if (workSheet.Dimension.End.Column != 47)
                            {
                                ucError.ErrorMessage = "Please upload correct file with data.";
                                ucError.Visible = true;
                                return;
                            }
                            for (int j = workSheet.Dimension.Start.Column; j <= workSheet.Dimension.End.Column; j++)
                            {
                                if (j == 47)
                                {
                                    ttreferenceno = workSheet.Cells[i, j].Text;
                                }
                                if (j == 47)
                                {
                                    remittenceno = workSheet.Cells[i, j].Text;
                                 
                                }
                                
                            }
                            PhoenixAccountsBankSupplierPayment.ExchangeRateDiffUpdate(remittenceno, ttreferenceno); 
                        }
                    }
                }
                
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
