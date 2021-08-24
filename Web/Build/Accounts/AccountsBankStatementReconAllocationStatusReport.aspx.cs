using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;

public partial class Accounts_AccountsBankStatementReconAllocationStatusReport : PhoenixBasePage
{
    public int iCompanyid;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            iCompanyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            SessionUtil.PageAccessRights(this.ViewState);



            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Upload", "UPLOAD");
        MenutravelInvoice.AccessRights = this.ViewState;
        MenutravelInvoice.MenuList = toolbar.Show();


        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Accounts/AccountsBankStatementExcelUploadFilter.aspx", "Find", "search.png", "FIND");
        MenuAttachment.AccessRights = this.ViewState;
        MenuAttachment.MenuList = toolbargrid.Show();

        BindData();
        SetPageNavigator();
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

        NameValueCollection nvc = Filter.CurrentBankStatementUploadSelection;

        ds = PhoenixAccountsBankStatementReconUpload.BankStatementReconUploadSearch(
            General.GetNullableString(nvc != null ? nvc.Get("txtBankAccount") : null),
            General.GetNullableString(nvc != null ? nvc.Get("txtAccDesc") : null),
            General.GetNullableInteger(nvc != null ? nvc.Get("ucCurrency") : ""),
            //General.GetNullableString(nvc != null ? nvc.Get("ddlType") : null),
            General.GetNullableInteger(nvc != null ? nvc.Get("ucMonth") : ""),
            General.GetNullableInteger(nvc != null ? nvc.Get("chkExclPostedBankStmt") : "1"),
            sortdirection,
            sortexpression,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            General.ShowRecords(null),
            ref iRowCount, ref iTotalPageCount,
            General.GetNullableInteger(nvc != null ? nvc.Get("chkExclArchivedBankStmt") : "1"),null);


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
    protected void gvAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //// Get the LinkButton control in the first cell
            //LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
            //// Get the javascript which is assigned to this LinkButton
            //string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            //// Add this javascript to the onclick Attribute of the row
            //e.Row.Attributes["ondblclick"] = _jsDouble;
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                ImageButton ed = (ImageButton)e.Row.FindControl("cmdEdit");
                ImageButton post = (ImageButton)e.Row.FindControl("cmdPost");
                ImageButton archive = (ImageButton)e.Row.FindControl("cmdArchive");
                ImageButton delete = (ImageButton)e.Row.FindControl("cmdDelete");

                if (ed != null)
                {

                    //ed.Visible = false;
                    ed.Attributes["onclick"] = "javascript:Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey="
                       + drv["FLDUPLOADID"].ToString() + "&mod=ACCOUNTS&U=1'); return false;";
                    e.Row.Attributes["ondblclick"] = string.Empty;
                }

                Label lblVoucherRefUpdatedYN = (Label)e.Row.FindControl("lblVoucherRefUpdatedYN");


                if (post != null)
                {
                    if (lblVoucherRefUpdatedYN.Text == "1" || General.GetNullableInteger(drv["FLDISARCHIVED"].ToString()) == 1)
                    {
                        post.Visible = false;
                        delete.Visible = false;
                    }
                    else
                    {
                        post.Visible = true;
                        delete.Visible = true;
                    }
                }

                if (archive != null)
                {
                    if (General.GetNullableInteger(drv["FLDISARCHIVED"].ToString()) == 1)
                    {
                        archive.Visible = false;
                    }
                    else
                    {
                        archive.Visible = true;
                    }
                }

                //ImageButton cmdLineItems = (ImageButton)e.Row.FindControl("cmdLineItems");
                //if (cmdLineItems != null)
                //{
                //    DataRowView drv = (DataRowView)e.Row.DataItem;
                //    //ed.Visible = false;
                //    cmdLineItems.Attributes["onclick"] = "javascript:Openpopup('NATD','','../Accounts/AccountsERMVoucherDetail.aspx?exceluploadid="
                //       + drv["FLDUPLOADID"].ToString() + "'); return false;";
                //}
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
}
