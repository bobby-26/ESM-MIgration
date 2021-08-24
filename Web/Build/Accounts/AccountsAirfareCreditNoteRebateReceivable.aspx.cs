using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsAirfareCreditNoteRebateReceivable : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (Request.QueryString["AirfareCreditNoteId"] != null && Request.QueryString["AirfareCreditNoteId"] != string.Empty)
                    ViewState["AirfareCreditNoteId"] = Request.QueryString["AirfareCreditNoteId"];

                BindDataMain();
            }
            PhoenixToolbar toolbargrid;
            PhoenixToolbar toolbarmain;

            SessionUtil.PageAccessRights(this.ViewState);
            toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Rebate Receivable", "REBATE RECEIVABLE", ToolBarDirection.Right);
            toolbarmain.AddButton("Credit Note", "VOUCHER", ToolBarDirection.Right);
            MenuRebateReceivable.AccessRights = this.ViewState;
            MenuRebateReceivable.MenuList = toolbarmain.Show();
            MenuRebateReceivable.SelectedMenuIndex = 0;

            SessionUtil.PageAccessRights(this.ViewState);
            toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsAirfareCreditNoteRebateReceivable.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvRebateReceivable')", "Print Grid", "icon_print.png", "PRINT");

            MenuRebateReceivableGrid.AccessRights = this.ViewState;
            MenuRebateReceivableGrid.MenuList = toolbargrid.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRebateReceivableGrid_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void MenuRebateReceivable_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsAirfareCreditNoteMaster.aspx?AirfareCreditNoteId=" + ViewState["AirfareCreditNoteId"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private void BindDataMain()
    {
        DataSet ds = PhoenixAccountsAirfareCreditNote.EditCreditDebitNote(new Guid(ViewState["AirfareCreditNoteId"].ToString()));

        DataRow dr = ds.Tables[0].Rows[0];
        txtRegisterNo.Text = dr["FLDCNREGISTERNO"].ToString();
        txtSupplier.Text = dr["FLDCODE"].ToString() + " / " + dr["FLDSUPPLIERNAME"].ToString();
        txtCurrencyAmount.Text = dr["FLDCURRENCYCODE"].ToString() + " / " + string.Format(String.Format("{0:###,###,###.00}", dr["FLDAMOUNT"]));
        txtStatus.Text = dr["FLDSTATUSNAME"].ToString();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        string[] alColumns = {"FLDVOUCHERLINEITEMNO","FLDACCOUNTCODE", "FLDDESCRIPTION","FLDCODE","FLDCURRENCYCODE","FLDLONGDESCRIPTION",
                                 "FLDAMOUNT"};
        string[] alCaptions = {"Row No.","Account Code", "Account Description","Sub Account Code","Transaction Currency","Long Description",
                                 "Prime Amount"};

        ds = PhoenixAccountsAirfareCreditNote.AirfareCreditRebateList(new Guid(ViewState["AirfareCreditNoteId"].ToString())
                                                                                  , gvRebateReceivable.CurrentPageIndex + 1
                                                                                  , gvRebateReceivable.PageSize
                                                                                  , ref iRowCount, ref iTotalPageCount);

        General.SetPrintOptions("gvRebateReceivable", "Rebate Receivable Details", alCaptions, alColumns, ds);

        gvRebateReceivable.DataSource = ds;
        gvRebateReceivable.VirtualItemCount = iRowCount;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["creditdebitnoteid"] != null)
            {
                if (ViewState["ISPOSTED"].ToString() == "1")
                    gvRebateReceivable.Columns[7].Visible = false;
            }
        }
        else
        {
            DataTable dt = ds.Tables[0];
            //ShowNoRecordsFound(dt, gvRebateReceivable);
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        string[] alColumns = {"FLDVOUCHERLINEITEMNO","FLDACCOUNTCODE", "FLDDESCRIPTION","FLDCODE","FLDCURRENCYCODE","FLDLONGDESCRIPTION",
                                 "FLDAMOUNT"};
        string[] alCaptions = {"Row No.","Account Code", "Account Description","Sub Account Code","Transaction Currency","Long Description",
                                 "Prime Amount"};


        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsAirfareCreditNote.AirfareCreditRebateList(new Guid(ViewState["AirfareCreditNoteId"].ToString())
                                                                                  , gvRebateReceivable.CurrentPageIndex + 1
                                                                                  , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                                                  , ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=RebateReceivableDetails.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Rebate Receivable Details</h3></td>");
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


    protected void gvRebateReceivable_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvRebateReceivable_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
}
