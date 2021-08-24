using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Collections.Specialized;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Accounts;
using iTextSharp.text.pdf;
using Telerik.Web.UI;

public partial class Accounts_AccountsSoaCheckingVoucherUpdate : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        foreach (GridViewRow r in gvCountry.Rows)
        {
            if (r.RowType == DataControlRowType.DataRow)
            {
                
            }
        }
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            SessionUtil.PageAccessRights(this.ViewState);

            if (Request.QueryString["ACCOUNTCODE"].ToString() != null)
                ViewState["ACCOUNTCODE"] = Request.QueryString["ACCOUNTCODE"].ToString();
            else
                ViewState["ACCOUNTCODE"] = "";

            if (Request.QueryString["OWNERBUDGETCODE"].ToString() != null)
                ViewState["OWNERBUDGETCODE"] = Request.QueryString["OWNERBUDGETCODE"].ToString();
            else
                ViewState["OWNERBUDGETCODE"] = "";

            if (Request.QueryString["debitnotereferenceid"].ToString() != null)
                ViewState["debitnotereferenceid"] = Request.QueryString["debitnotereferenceid"].ToString();
            else
                ViewState["debitnotereferenceid"] = "";


            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["output_filename"] = "";
                ViewState["output_vouchernumber"] = "";
                ViewState["output_error"] = "";
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
    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        BindData();
        SetPageNavigator();
    }

    protected void RegistersCountry_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            
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
        string[] alColumns = { "FLDACCOUNTCODE", "FLDESMBUDGETCODE", "FLDOWNERBUDGETCODE", "FLDVOUCHERDATE", "FLDDEBITNOTEREFERENCE", "FLDPHOENIXVOUCHER", "FLDREFERENCE", "FLDAMOUNT", "FLDDESCRIPTION", "FLDACCOUNTDESCRIPTION", "FLDBUDGETCODEDESCRIPTION" };
        string[] alCaptions = { "Account Code", "Budget Code", "Owner Budget Code", "Voucher Date", "Debit Note Reference", "Phoenix Voucher", "Reference", "Amount", "Description", "Account Description", "Budget Code Description" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        DataSet ds = new DataSet();

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixAccountsSoaChecking.SoaCheckingVoucherSearch(ViewState["ACCOUNTCODE"].ToString()
                                                               , ViewState["OWNERBUDGETCODE"].ToString()
                                                               , (int)ViewState["PAGENUMBER"]
                                                               , General.ShowRecords(null)
                                                               , ref iRowCount
                                                               , ref iTotalPageCount
                                                               );

        General.SetPrintOptions("gvCountry", "Country", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCountry.DataSource = ds;
            gvCountry.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvCountry);
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCountry_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        _gridView.EditIndex = -1;
        _gridView.SelectedIndex = e.RowIndex;
        BindData();

    }

    protected void gvCountry_Sorting(object sender, GridViewSortEventArgs se)
    {
        gvCountry.EditIndex = -1;
        gvCountry.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvCountry_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            //e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvCountry, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void gvCountry_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {

                Label lblvoucherdetailid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblvoucherdetailid");
                DropDownList ddldebitnoteref = (DropDownList)_gridView.Rows[nCurrentRow].FindControl("ddlStatementReference");

                DataTable dtAtt = PhoenixAccountsDebitNoteReference.SOACheckingVoucherPDFCheckforBilling(new Guid(lblvoucherdetailid.Text));
                if (dtAtt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtAtt.Rows)
                    {
                        ViewState["output_filename"] = dr["FLDFILENAME"].ToString();
                        ViewState["output_vouchernumber"] = dr["FLDVOUCHERNUMBER"].ToString();
                        string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                        if (!path.EndsWith("/"))
                            path = path + "/";
                        CheckPdfProtection(path + dr["FLDFILEPATH"].ToString(), dr["FLDVOUCHERNUMBER"].ToString(), dr["FLDFILENAME"].ToString());
                    }
                }
                if (ViewState["output_error"].ToString() == "")
                {
                    try
                    { 
                    PhoenixAccountsSoaChecking.SoaCheckingVoucherUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ddldebitnoteref.SelectedValue), General.GetNullableGuid(lblvoucherdetailid.Text));

                    String scriptupdate = String.Format("javascript:fnReloadList('NAFA','null','keepupopen');");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptupdate, true);
                    }
                    catch (Exception ex)
                    {
                        ucError.ErrorMessage = ex.Message;
                        ucError.Visible = true;
                    }

                }
            }
            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();
        }
        //catch (Exception ex)
        //{

        //}
        finally
        {
            if(ViewState["output_error"].ToString()!="")
            { 
                ucError.ErrorMessage = ViewState["output_error"].ToString();
                ucError.Visible = true;
            }
        }
    }

    protected void gvCountry_RowDeleting(object sender, GridViewDeleteEventArgs de)
    {
        BindData();
        SetPageNavigator();
    }

    protected void gvCountry_RowEditing(object sender, GridViewEditEventArgs de)
    {

        GridView _gridView = (GridView)sender;

        _gridView.EditIndex = de.NewEditIndex;
        _gridView.SelectedIndex = de.NewEditIndex;

        BindData();
        SetPageNavigator();

    }

    protected void gvCountry_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = e.RowIndex;

            

            _gridView.EditIndex = -1;
            BindData();
            SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCountry_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
          
        }

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton eb = (ImageButton)e.Row.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            DropDownList ddlDebitNoteReference = (DropDownList)e.Row.FindControl("ddlStatementReference");

            if (ddlDebitNoteReference != null)
            {
                DataSet ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceList(ViewState["ACCOUNTCODE"].ToString());
                ddlDebitNoteReference.DataTextField = "FLDDEBITNOTEREFERENCE";
                ddlDebitNoteReference.DataValueField = "FLDDEBINOTEREFERENCEID";
                ddlDebitNoteReference.DataSource = ds;
                ddlDebitNoteReference.DataBind();

                if (!string.IsNullOrEmpty(ViewState["debitnotereferenceid"].ToString()))
                {
                    ddlDebitNoteReference.SelectedValue = ViewState["debitnotereferenceid"].ToString();
                }
            }

            DataRowView drv = (DataRowView)e.Row.DataItem;

            if (eb != null && (drv["FLDVOUCHERSTATUS"].ToString().ToUpper().Equals("DRAFT") || drv["FLDVOUCHERSTATUS"].ToString().ToUpper().Equals("OUT OF BALANCE")))
            {
                eb.Visible = false;
            }


        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvCountry.EditIndex = -1;
        gvCountry.SelectedIndex = -1;
        BindData();
        SetPageNavigator();
    }

    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvCountry.EditIndex = -1;
        gvCountry.SelectedIndex = -1;
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

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvCountry.SelectedIndex = -1;
        gvCountry.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindData();
        SetPageNavigator();
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
            return true;

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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        SetPageNavigator();
    }

    private void CheckPdfProtection(string filePath, string voucher, string filename)
    {
        try
        {
            PdfReader reader = new PdfReader(filePath);
            try
            {
                PdfSharp.Pdf.PdfDocument pdfDoc = PdfSharp.Pdf.IO.PdfReader.Open(filePath, PdfSharp.Pdf.IO.PdfDocumentOpenMode.Import);
            }
            catch (Exception)
            {
                throw new Exception("the uploaded pdf file is corrupted and cannot be uploaded");
            }

            if (!reader.IsEncrypted()) return;

            if (!PdfEncryptor.IsPrintingAllowed(reader.Permissions))
            {
                ViewState["output_error"] = ViewState["output_error"] + Environment.NewLine + new InvalidOperationException(voucher + " - " + filename + " the selected file is print protected and cannot be imported") + Environment.NewLine;
            }
            if (!PdfEncryptor.IsModifyContentsAllowed(reader.Permissions))
            {
                ViewState["output_error"] = ViewState["output_error"] + Environment.NewLine + new InvalidOperationException(voucher + " - " + filename + "the selected file is write protected and cannot be imported") + Environment.NewLine;
            }
        }
        catch (Exception ex)
        {
            ViewState["output_error"] = ViewState["output_error"] + Environment.NewLine + " Voucher : " + voucher + " File :" + filename + ex.ToString().Substring(0, ex.ToString().IndexOf(Environment.NewLine)) + Environment.NewLine;
        }        
    }


}
