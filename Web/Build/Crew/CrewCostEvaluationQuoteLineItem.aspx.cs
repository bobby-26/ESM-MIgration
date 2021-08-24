using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using SouthNests.Phoenix.Common;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Collections;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewCostEvaluationQuoteLineItem : PhoenixBasePage
{
    DataSet dsGrid;
    ArrayList arraySectionType = new ArrayList();
    ArrayList arrayid = new ArrayList();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

                PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddImageButton("../Crew/CrewCostEvaluationQuoteLineItem.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbargrid.AddImageLink("javascript:CallPrint('gvQuote')", "Print Grid", "icon_print.png", "Print");

                MenuQuote.AccessRights = this.ViewState;
                MenuQuote.MenuList = toolbargrid.Show();

                ViewState["QUOTEID"] = null;
                if (Request.QueryString["requestid"] != null)
                    ViewState["REQUESTID"] = Request.QueryString["requestid"].ToString();
                if (Request.QueryString["portagentid"] != null)
                    ViewState["PORTAGENTID"] = Request.QueryString["portagentid"].ToString();

                ViewState["EVALUATIONPORTID"] = "";

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["TOTALPAGECOUNT"] = null;

                ViewState["QPAGENUMBER"] = 1;
                ViewState["QSORTEXPRESSION"] = null;
                ViewState["QSORTDIRECTION"] = null;

                ViewState["CURRENTINDEX"] = 1;
            }

            BindQuoteDetails();
            BindQuoteLineItem();
            SetPageNavigator();
            BindTotal();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindQuoteDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDAGENTNAME", "FLDSEAPORTNAME", "FLDQUOTEREFNO", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDTOTALUSDAMOUNT" };
        string[] alCaptions = { "Port Agent", "Port", "Quotation No", "Currency", "Amount", "Amount(USD)" };
        string requestid = null;
        string portagentid = null;

        string sortexpression = (ViewState["QSORTEXPRESSION"] == null) ? null : (ViewState["QSORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["QSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["QSORTDIRECTION"].ToString());
        if (ViewState["REQUESTID"] != null)
            requestid = ViewState["REQUESTID"].ToString();
        if (ViewState["PORTAGENTID"] != null)
            portagentid = ViewState["PORTAGENTID"].ToString();

        DataSet ds = PhoenixCrewCostEvaluationQuote.CrewCostQuoteSearch(new Guid(requestid)
        , General.GetNullableGuid(portagentid)
        , sortexpression, sortdirection, (int)ViewState["QPAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvQuote.DataSource = ds;
            gvQuote.DataBind();
            if (ViewState["QUOTEID"] == null)
            {
                ViewState["QUOTEID"] = ds.Tables[0].Rows[0]["FLDQUOTEID"].ToString();
                ViewState["EVALUATIONPORTID"] = ds.Tables[0].Rows[0]["FLDEVALUATIONPORTID"].ToString();
                gvQuote.SelectedIndex = 0;
            }
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvQuote);
        }
        General.SetPrintOptions("gvQuote", "Quotation Details", alCaptions, alColumns, ds);
    }

    private void BindQuoteLineItem()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSECTIONTYPE", "FLDSECTION", "FLDAMOUNT", "FLDREMARKS" };
        string[] alCaptions = { "Section Type", "Section", "Amount", "Remarks" };
        string requestid = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["REQUESTID"] != null)
            requestid = ViewState["REQUESTID"].ToString();

        DataSet ds = PhoenixCrewCostEvaluationQuote.CrewCostQuoteLineItemSearch(new Guid(requestid)
            , General.GetNullableGuid(ViewState["QUOTEID"] == null ? "" : ViewState["QUOTEID"].ToString())
        , (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvLineItem.DataSource = ds;
            gvLineItem.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvLineItem);
        }
        General.SetPrintOptions("gvLineItem", "Quotation Section Details", alCaptions, alColumns, ds);

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void BindTotal()
    {
        dsGrid = PhoenixCrewCostEvaluationQuote.QuotationCompareSetionType(new Guid(ViewState["REQUESTID"].ToString())
            , General.GetNullableGuid(ViewState["EVALUATIONPORTID"].ToString()));

        arraySectionType.Clear();

        foreach (DataRow dr in dsGrid.Tables[1].Rows)
        {
            arraySectionType.Add(dr["FLDSECTIONTYPENAME"].ToString());
            arrayid.Add(dr["FLDSECTIONTYPEID"].ToString());
        }

        AddCoumnsInGrid(dsGrid.Tables[0], dsGrid.Tables[1]);
        gvSectionType.DataSource = dsGrid;

        gvSectionType.DataBind();

        if (dsGrid.Tables[0].Rows.Count <= 0)
        {
            ShowNoRecordsFound(dsGrid.Tables[0], gvSectionType);
        }
    }

    private void AddCoumnsInGrid(DataTable datatable, DataTable coursetable)
    {
        if (datatable.Columns.Count > 0 && gvSectionType.Columns.Count < 2)
        {
            for (int i = 1; i < datatable.Columns.Count; i++)
            {
                BoundField total = new BoundField();
                total.DataField = datatable.Columns[i].ColumnName;
                total.HeaderText = coursetable.Rows[i - 1]["FLDSECTIONTYPENAME"].ToString();
                total.ControlStyle.BorderColor = System.Drawing.Color.White;
                gvSectionType.Columns.Add(total);
            }
        }
    }
    protected void gvSectionType_RowCreated(object sender, GridViewRowEventArgs e)
    {

    }
    protected void MenuQuote_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
            if (dce.CommandName.ToUpper() == "EXCEL")
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
    protected void MenuQuoteLI_TabStripCommand(object sender, EventArgs e)
    {

    }
    protected void gvQuote_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            ImageButton com = (ImageButton)e.Row.FindControl("cmdCommunication");

            Label lbl = (Label)e.Row.FindControl("lblRequestId");
            Label lblAgentId = (Label)e.Row.FindControl("lblAgentId");
            Label lblPortAgentId = (Label)e.Row.FindControl("lblPortAgentId");
            Label lblAgentName = (Label)e.Row.FindControl("lblAgentName");
            Label lblQuoteId = (Label)e.Row.FindControl("lblQuoteId");

            Label lblAmount = (Label)e.Row.FindControl("lblAmount");
            Label lblApprovedYN = (Label)e.Row.FindControl("lblApprovedYN");

            ViewState["FLDREQUESTNO"] = PhoenixCrewCostEvaluationRequest.RequestNumber;

            if (com != null)
            {
                com.Visible = true;
                com.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp2','','../Crew/CrewCostEvaluationQuoteChat.aspx?REQUESTID=" + ViewState["REQUESTID"].ToString() + "&AGENTID=" + lblAgentId.Text + "&QUOTEID=" + lblQuoteId.Text + "&PORTAGENTID=" + lblPortAgentId.Text + "&AGENTNAMEOLY=" + lblAgentName.Text.Replace('&', '~').ToString() + "&AGENTNAME=" + lblAgentName.Text.Replace('&', '~').ToString() + " - " + ViewState["FLDREQUESTNO"].ToString() + "&ISOFFICE=1" + "');return false;");
            }
        }

    }

    protected void gvQuote_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper() == "SELECT")
            {
                ViewState["QUOTEID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuoteId")).Text;
                ViewState["EVALUATIONPORTID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEvaluationPortId")).Text;
            }

            if (e.CommandName.ToUpper().Equals("FINALIZE"))
            {

            }
            if (e.CommandName.ToUpper().Equals("COMMUNICATION"))
            {

            }
            if (e.CommandName.ToUpper().Equals("APPROVE"))
            {
                try
                {
                    string quoteid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuoteId")).Text;
                    ViewState["QUOTEID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuoteId")).Text;
                    PhoenixCrewCostEvaluationQuote.CrewCostQuoteApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["REQUESTID"].ToString())
                        , new Guid(quoteid));

                    ucConfirm.ErrorMessage = "Quote is approved";
                    ucConfirm.Visible = true;
                    BindQuoteDetails();
                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
            if (e.CommandName.ToUpper().Equals("DEAPPROVE"))
            {
                try
                {
                    string quoteid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuoteId")).Text;
                    ViewState["QUOTEID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblQuoteId")).Text;
                    PhoenixCrewCostEvaluationQuote.CrewCostQuoteDeApprove(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , new Guid(ViewState["REQUESTID"].ToString())
                        , new Guid(quoteid));

                    ucConfirm.ErrorMessage = "Quote approval is revoked";
                    ucConfirm.Visible = true;

                }
                catch (Exception ex)
                {
                    ucError.HeaderMessage = "";
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }
            }
            _gridView.SelectedIndex = nCurrentRow;
            BindQuoteDetails();
            BindTotal();
            BindQuoteLineItem();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private decimal TotalAmount = 0;
    protected void gvLineItem_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Row.DataItem;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            decimal.TryParse(drv["FLDTOTALAMOUNT"].ToString(), out TotalAmount);
        }
        else if (e.Row.RowType == DataControlRowType.Footer)
        {
            e.Row.Cells[2].Text = TotalAmount.ToString();
            e.Row.Cells[2].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Cells[1].HorizontalAlign = HorizontalAlign.Right;
            e.Row.Font.Bold = true;
        }

    }
    protected void MenuQuotaeLineItem_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

    }
    protected void gvLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDAGENTNAME", "FLDSEAPORTNAME", "FLDQUOTEREFNO", "FLDCURRENCYCODE", "FLDAMOUNT", "FLDTOTALUSDAMOUNT" };
        string[] alCaptions = { "Port Agent", "Port", "Quotation No", "Currency", "Amount", "Amount(USD)" };
        string requestid = null;
        string portagentid = null;
        string sortexpression = (ViewState["QSORTEXPRESSION"] == null) ? null : (ViewState["QSORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["QSORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["QSORTDIRECTION"].ToString());
        if (ViewState["REQUESTID"] != null)
            requestid = ViewState["REQUESTID"].ToString();
        if (ViewState["PORTAGENTID"] != null)
            portagentid = ViewState["PORTAGENTID"].ToString();
        DataSet ds = PhoenixCrewCostEvaluationQuote.CrewCostQuoteSearch(new Guid(requestid)
        , General.GetNullableGuid(portagentid)
        , sortexpression, sortdirection, (int)ViewState["QPAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=Travel Quote Section.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Quote Section</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
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


    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindQuoteDetails();

            if (Session["emailsend"] != null)
            {
                ucConfirm.ErrorMessage = "E-mail send to the below Agents,<BR/>" + Session["emailsend"].ToString();
                ucConfirm.Visible = true;
            }
            Session["emailsend"] = null;
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
        gv.Rows[0].Attributes["onclick"] = "";
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
        BindQuoteDetails();
        SetPageNavigator();
    }

    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvQuote.SelectedIndex = -1;
        gvQuote.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

        BindQuoteDetails();
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
