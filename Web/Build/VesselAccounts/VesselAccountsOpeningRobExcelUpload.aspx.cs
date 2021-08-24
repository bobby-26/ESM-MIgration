using System;
using System.IO;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using SouthNests.Phoenix.Framework;
using System.Collections.Generic;
using System.Web.UI;
using System.Drawing;
using SouthNests.Phoenix.VesselAccounts;
using SouthNests.Phoenix.Registers;
//using Ionic.Zip;

public partial class VesselAccountsOpeningRobExcelUpload : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save Start Date", "SAVE");
        //toolbar.AddButton("Send Mail", "SENDMAIL");
        toolbar.AddImageLink("javascript:return showPickList('spnPickListStore', 'codehelp1', '', '../VesselAccounts/VesselAccountsSendMailToVesselForTrash.aspx'); return false;", "Send Mail", "", "SENDMAIL");
        toolbar.AddButton("Upload", "UPLOAD");
        toolbar.AddButton("Initialize", "INITIALIZE");
        MenuOpeningRob.AccessRights = this.ViewState;
        MenuOpeningRob.MenuList = toolbar.Show();

        if (!IsPostBack)
        {

            //btnSendMailForTrash.Attributes.Add("onclick", "Openpopup('codehelp1', '', 'VesselAccountsSendMailToVesselForTrash.aspx'); return false;");

            DataTable dt = PhoenixVesselAccountsCorrections.VesselStartDateSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ucPBStart.Text = dr["FLDHARDNAME"].ToString();
                lblHardCode.Text = dr["FLDHARDCODE"].ToString();
            }


            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }

        BindData();
    }

    protected void MenuOpeningRob_OnTabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToString().ToUpper() == "UPLOAD")
        {
            try
            {
                PhoenixVesselAccountsCorrections.DeleteOpeningRob(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

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
        if (dce.CommandName.ToString().ToUpper() == "SAVE")
        {
            try
            {
                if (!IsValidDate(ucPBStart.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string strStarDate;
                strStarDate = ConvertDateFormat(ucPBStart.Text);


                if (lblHardCode.Text == null || lblHardCode.Text == "" || lblHardCode.Text == string.Empty)
                {
                    //InsertHard("165", strStarDate, PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
                    ucError.ErrorMessage = "Vessel Accounting start date is not defined earlier, Contact Administrator to define the Start Date.";
                    ucError.Visible = true;
                }
                else
                {
                    UpdateStartDate("165", int.Parse(lblHardCode.Text), strStarDate, PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString());
                }
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }
        if (dce.CommandName.ToString().ToUpper() == "INITIALIZE")
        {
            try
            {
                if (!IsValidDate(ucPBStart.Text))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixVesselAccountsCorrections.InitializeOpeningRob(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(ucPBStart.Text));
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
            BindData();
        }
    }

    private void InsertHard(string hardtypecode, string Hardname, string Shortname)
    {
        if (!IsValidHard(Shortname, Hardname))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersHard.InsertHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            hardtypecode, Hardname, Shortname);
        ucStatus.Text = "Vessel Accounting Start Date is Initialized";
    }

    private void UpdateStartDate(string Hardtypecode, int Hardcode, string Hardname, string shortname)
    {
        if (!IsValidHard(shortname, Hardname))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixVesselAccountsCorrections.UpdateStartDate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            Hardtypecode, Hardcode, Hardname, shortname);
        ucStatus.Text = "Vessel Accounting Start Date is Updated";
    }

    private bool IsValidHard(string Shortname, string Hardname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Shortname.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (Hardname.Trim().Equals(""))
            ucError.ErrorMessage = "Date is required.";


        return (!ucError.IsError);
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
            PhoenixVesselAccountsCorrections.InsertOpeningRob
                (
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , li[0].ToString()
                , li[1].ToString()
                , decimal.Parse(li[2].ToString())
                , decimal.Parse(li[3].ToString())
                , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , int.Parse(li[4].ToString())
                );
        }
        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
        }
    }

    public void BindData()
    {

        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 10;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixVesselAccountsCorrections.SearchOpeningRob(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                , sortexpression, sortdirection,
               Convert.ToInt16(ViewState["PAGENUMBER"].ToString()), 200,
               ref iRowCount,
               ref iTotalPageCount);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvOpeningSearch.DataSource = ds;
                gvOpeningSearch.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvOpeningSearch);
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
        gvOpeningSearch.SelectedIndex = -1;
        gvOpeningSearch.EditIndex = -1;
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
    private decimal ProvisionTotalPrice = 0, ProvisionTotalQty = 0, BondTotalPrice = 0, BondTotalQty = 0;
    protected void gvOpeningSearch_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblStoreType = (Label)e.Row.FindControl("lblStoreType");
            if (drv["FLDSTORECLASS"] != null)
            {
                if (drv["FLDSTORECLASS"].ToString() == "411")
                {
                    lblStoreType.Text = "Provision";
                }
                else if (drv["FLDSTORECLASS"].ToString() == "412")
                {
                    lblStoreType.Text = "Bond";
                }
                else
                {
                    lblStoreType.Text = "";
                }
            }

            if (drv["FLDDUPLICATE"].ToString() == "1")
            {
                e.Row.ForeColor = System.Drawing.Color.Red;
            }


            decimal.TryParse(drv["FLDPROVISIONTOTAL"].ToString(), out ProvisionTotalPrice);
            decimal.TryParse(drv["FLDPROVISIONQTY"].ToString(), out ProvisionTotalQty);
            decimal.TryParse(drv["FLDBONDTOTAL"].ToString(), out BondTotalPrice);
            decimal.TryParse(drv["FLDBONDQTY"].ToString(), out BondTotalQty);
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[4].Text = ProvisionTotalPrice.ToString() + "<br/>" + ProvisionTotalQty.ToString("0.00") + "<br/>" + BondTotalPrice.ToString() + "<br/>" + BondTotalQty.ToString();
            e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Font.Bold = true;
        }
    }
    protected void gvOpeningSearch_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            GridViewRow gr = _gridView.Rows[nCurrentRow];
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            string number = ((TextBox)gr.FindControl("txtNumber")).Text;
            string name = ((TextBox)gr.FindControl("txtName")).Text;
            string quantity = ((UserControlMaskNumber)gr.FindControl("txtQuantityEdit")).Text;
            string total = ((UserControlMaskNumber)gr.FindControl("txtTotalEdit")).Text;
            string storeclass = ((UserControlMaskNumber)gr.FindControl("txtStoreClassEdit")).Text;


            if (!IsValidUpdate(number, name, quantity, total, storeclass))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixVesselAccountsCorrections.UpdateOpeningRobSingleItem(id, PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                                                        number, name, decimal.Parse(quantity), decimal.Parse(total), int.Parse(storeclass));
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvOpeningSearch_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        BindData();
    }
    protected void gvOpeningSearch_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = e.NewEditIndex;
        BindData();
    }
    protected void gvOpeningSearch_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = e.RowIndex;
        try
        {
            Guid id = (Guid)_gridView.DataKeys[nCurrentRow].Value;
            PhoenixVesselAccountsCorrections.DeleteOpeningRobSingleItem(PhoenixSecurityContext.CurrentSecurityContext.VesselID, id);
            _gridView.EditIndex = -1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        _gridView.EditIndex = -1;
        BindData();
    }

    private bool IsValidUpdate(string number, string name, string qty, string total, string storeclass)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (number == string.Empty)
        {
            ucError.ErrorMessage = "Store Item Number is required.";
        }
        if (name == string.Empty)
        {
            ucError.ErrorMessage = "Store Item Name is required.";
        }
        if (!General.GetNullableDecimal(qty).HasValue)
        {
            ucError.ErrorMessage = "Quantity is required.";
        }
        if (!General.GetNullableDecimal(total).HasValue)
        {
            ucError.ErrorMessage = "Total is required.";
        }
        if (!General.GetNullableInteger(storeclass).HasValue)
        {
            ucError.ErrorMessage = "Store Class is required.";
        }

        return (!ucError.IsError);
    }
    //protected void btnConfirm_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (!IsValidDate(ucPBStart.Text))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }

    //        PhoenixVesselAccountsCorrections.InitializeOpeningRob(PhoenixSecurityContext.CurrentSecurityContext.VesselID, DateTime.Parse(ucPBStart.Text));
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //    BindData();
    //}
    private bool IsValidDate(string date)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (!General.GetNullableDateTime(date).HasValue)
        {
            ucError.ErrorMessage = "Opening Date is required.";
        }

        if (date != null)
        {
            string day = date.Substring(0, 2);
            if (day != "01")
            {
                ucError.ErrorMessage = "Date Should be the first date of the Month.";
            }
        }

        return (!ucError.IsError);
    }

    private string ConvertDateFormat(string date)
    {
        string StrMonth = date.Substring(3, 2);

        string[] dayinnumber = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };
        string[] dayinword = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

        for (int i = 0; i < dayinnumber.Length; i++)
        {
            if (StrMonth == dayinnumber[i])
            {
                StrMonth = StrMonth.Replace(StrMonth, dayinword[i]);
            }
        }

        string StrYear = date.Remove(0, 6);

        return "1/" + StrMonth + "/" + StrYear;
    }
}
