using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class Accounts_AccountsAdvancePaymentDetails : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsAdvancePaymentDetails.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvAdvancePayment')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageButton("../Accounts/AccountsAdvancePaymentDetails.aspx", "Find", "search.png", "FIND");
            MenuAdvanceItem.AccessRights = this.ViewState;
            MenuAdvanceItem.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                if ((Request.QueryString["orderid"] != null) && (Request.QueryString["orderid"] != ""))
                {
                    ViewState["orderid"] = Request.QueryString["orderid"].ToString();

                }

                if (Request.QueryString["ponumber"] != null && (Request.QueryString["ponumber"] != ""))
                {
                    txtPoNumber.Text = Request.QueryString["ponumber"].ToString();
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvAdvancePayment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void MenuAdvanceItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND") && txtAdvancePaymentNumber.Text != null)
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                //Response.Redirect("../Reports/ReportsView.aspx?applicationcode=4&reportcode=ASPERSCREEN&vessellist='59,97'&zonelist=NULL&sailonly=1");
                ShowExcel();
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
        gvAdvancePayment.SelectedIndexes.Clear();
        gvAdvancePayment.EditIndexes.Clear();
        gvAdvancePayment.DataSource = null;
        gvAdvancePayment.Rebind();
    }
    protected void gvAdvancePayment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAdvancePayment.CurrentPageIndex + 1;
            BindData();
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

        string[] alColumns = { "FLDADVANCEPAYMENTNUMBER", "FLDPAYDATE", "FLDAMOUNT" };
        string[] alCaptions = { "Advance Payment Number", "Date", "Amount" };
        string advancepaymentnumber = txtAdvancePaymentNumber.Text;
        string referencedocument = null;
        int? currency = null;
        int? paymentstatus = null;
        string paymentfromdate = null;
        string paymenttodate = null;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if ((Request.QueryString["orderid"] != null) && (Request.QueryString["orderid"] != ""))
            ViewState["orderid"] = Request.QueryString["orderid"].ToString();


        if (Request.QueryString["ponumber"] != null && (Request.QueryString["ponumber"] != ""))
            txtPoNumber.Text = Request.QueryString["ponumber"].ToString();

        string strorderid = ViewState["orderid"].ToString();
        Guid orderid = new Guid(strorderid);

        DataSet ds = PhoenixAccountsAdvancePayment.AdvancePaymentSearch(advancepaymentnumber
                                                                        , orderid
                                                                        , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                        , referencedocument
                                                                        , currency, paymentstatus
                                                                        , paymentfromdate
                                                                        , paymenttodate
                                                                        , null
                                                                        , sortexpression
                                                                        , sortdirection
                                                                        , (int)ViewState["PAGENUMBER"]
                                                                        , gvAdvancePayment.PageSize
                                                                        , ref iRowCount
                                                                        , ref iTotalPageCount
                                                                        , null
                                                                        , null
                                                                        , null);



        gvAdvancePayment.DataSource = ds;
        gvAdvancePayment.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvAdvancePayment", "Advance Payment", alCaptions, alColumns, ds);

    }


    protected void gvAdvancePayment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string advancepaymentnumber = txtAdvancePaymentNumber.Text;
        string referencedocument = null;
        int? currency = null;
        int? paymentstatus = null;
        string paymentfromdate = null;
        string paymenttodate = null;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDADVANCEPAYMENTNUMBER", "FLDPAYDATE", "FLDAMOUNT" };
        string[] alCaptions = { "Advance Payment Number", "Date", "Amount" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string strorderid = ViewState["orderid"].ToString();
        Guid orderid = new Guid(strorderid);

        ds = PhoenixAccountsAdvancePayment.AdvancePaymentSearch(advancepaymentnumber
                                                                         , orderid
                                                                         , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                         , referencedocument
                                                                         , currency, paymentstatus
                                                                         , paymentfromdate
                                                                         , paymenttodate
                                                                         , null
                                                                         , sortexpression
                                                                         , sortdirection
                                                                         , (int)ViewState["PAGENUMBER"]
                                                                         , General.ShowRecords(null)
                                                                         , ref iRowCount
                                                                         , ref iTotalPageCount
                                                                         , null
                                                                         , null
                                                                         , null);

        Response.AddHeader("Content-Disposition", "attachment; filename=AdvancePaymentDetails.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Advance Payment</h3></td>");
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
    double total = 0.00;
    protected void gvAdvancePayment_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                RadLabel l = (RadLabel)e.Item.FindControl("lblAmount");
                if (l != null && l.Text != "")
                {
                    total = total + Convert.ToDouble(l.Text);
                    //Label lb = (Label)(e.Row.FindControl("lblTotalAmount"));                       
                }

            }
        }
        if (e.Item is GridFooterItem)
        {

            RadLabel lb = (RadLabel)e.Item.FindControl("lblTotalAmount");
            lb.Text = Convert.ToString(total) + ".00";
            if (lb.Text == "0.00")
                lb.Visible = false;

        }

    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }

}
