using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Accounts_AccountsDashboardDebitNoteReferenceHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsDebitNoteReferenceHistory.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvInvoiceHistory')", "Print Grid", "icon_print.png", "PRINT");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                ViewState["status"] = "";
                ViewState["type"] = "";
                ViewState["month"] = "";
                ViewState["year"] = "";

                if (Request.QueryString["status"] != null)
                    ViewState["status"] = Request.QueryString["status"].ToString();
                if (Request.QueryString["type"] != null)
                    ViewState["type"] = Request.QueryString["type"].ToString();
                if (Request.QueryString["month"] != null)
                    ViewState["month"] = Request.QueryString["month"].ToString();
                if (Request.QueryString["year"] != null)
                    ViewState["year"] = Request.QueryString["year"].ToString();

                ViewState["ReferenceNumber"] = "";
                if (Request.QueryString["DebitNoteReferenceid"] != null)
                    ViewState["DebitNoteReferencecode"] = Request.QueryString["DebitNoteReferenceid"].ToString();
                else
                    ViewState["DebitNoteReferencecode"] = Guid.Empty;

                if (Request.QueryString["qfrom"] != null && Request.QueryString["qfrom"].ToString() != string.Empty)
                    ViewState["qfrom"] = Request.QueryString["qfrom"];
                else
                    ViewState["qfrom"] = "";
                SetRedirectURL();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = null;

                BindReferenceNumber(ViewState["DebitNoteReferencecode"].ToString());
            }

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);

            MenuOrderFormMain.AccessRights = this.ViewState;

            MenuOrderFormMain.MenuList = toolbarmain.Show();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInvoiceHistory_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;
        BindData();
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect(ViewState["URL"].ToString() + ViewState["DebitNoteReferencecode"] + "&qcallfrom=DebitNoteReference"+ "&status=" + ViewState["status"].ToString() + "&type=" + ViewState["type"].ToString() + "&month=" + ViewState["month"].ToString() + "&year=" + ViewState["year"].ToString() + "&Condition=1");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        //int iTotalPageCount = 0;
        string type = "";

        string[] alCaptions = {
                                "Date/Time of Change",
                                "Type of Change",
                                "User Name",
                                "Field",
                                "Old Value",
                                "New Value",
                                "Procedure Used",
                              };

        string[] alColumns = {  "FLDUPDATEDATE",
                                "FLDTYPENAME",
                                "FLDUSERNAME",
                                "FLDFIELD",
                                "FLDPREVIOUSVALUE",
                                "FLDCURRENTVALUE",
                                "FLDPROCEDURENAME",
                             };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        type = rblHistoryType.SelectedItem.Value;

        ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceHistoryList(new Guid(ViewState["DebitNoteReferencecode"].ToString())
                                                     , type);

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountsDebitNoteReferenceHistory.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Debit Note Reference History" + "-" + ViewState["ReferenceNumber"] + "</h3></td>");
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

    protected void MenuOrderForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
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

    private void SetRedirectURL()
    {
        if (ViewState["qfrom"] != null)
        {
            if (ViewState["qfrom"].ToString() == "DebitNoteReference")
            {
                ViewState["URL"] = "../Accounts/AccountsErmDebitNoteReference.aspx?DebitNoteReferenceid=";
            }
            if (ViewState["qfrom"].ToString() == "SOAGeneration")
            {
                ViewState["URL"] = "../Accounts/AccountsDashboardSOAGeneration.aspx?DebitNoteReferenceid=";
            }
        }
    }

    private void BindReferenceNumber(string DebitNoteReferencenumber)
    {
        if (General.GetNullableGuid(DebitNoteReferencenumber) != null)
        {
            DataSet dsInvoice = PhoenixAccountsDebitNoteReference.DebitNoteReferenceEdit(new Guid(DebitNoteReferencenumber));
            if (dsInvoice.Tables[0].Rows.Count > 0)
            {
                ViewState["ReferenceNumber"] = dsInvoice.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"];
                //MenuOrderFormMain.Title = "History" + " - " + dsInvoice.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString();
            }
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string type = "";
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string[] alCaptions = {
                                "Date/Time of Change",
                                "Type of Change",
                                "User Name",
                                "Field",
                                "Old Value",
                                "New Value",
                                "Procedure Used",
                              };

        string[] alColumns = {  "FLDUPDATEDATE",
                                "FLDTYPENAME",
                                "FLDUSERNAME",
                                "FLDFIELD",
                                "FLDPREVIOUSVALUE",
                                "FLDCURRENTVALUE",
                                "FLDPROCEDURENAME",
                             };

        type = rblHistoryType.SelectedItem.Value;

        if (type == "ATTACHMENTVERIFICATION")
        {
            ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceHistoryList(new Guid(ViewState["DebitNoteReferencecode"].ToString())
                                                         , type, 1);
        }
        else if (type == "STATUS")
        {
            ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceHistoryList(new Guid(ViewState["DebitNoteReferencecode"].ToString())
                                                      , type, null, 1);
        }
        else
        {
            ds = PhoenixAccountsDebitNoteReference.DebitNoteReferenceHistoryList(new Guid(ViewState["DebitNoteReferencecode"].ToString())
                                                      , type);
        }
        General.SetPrintOptions("gvInvoiceHistory", "Debit Note Reference History" + "-" + ViewState["ReferenceNumber"], alCaptions, alColumns, ds);
        gvInvoiceHistory.DataSource = ds;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ReBindData(object sender, EventArgs e)
    {
        BindData();
    }

    protected void cmdSort_Click(object sender, EventArgs e)
    {
        ImageButton ib = (ImageButton)sender;
        try
        {

            ViewState["SORTEXPRESSION"] = ib.CommandName;
            ViewState["SORTDIRECTION"] = int.Parse(ib.CommandArgument);
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvInvoiceHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
