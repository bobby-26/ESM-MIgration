using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsCreditDebitHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsCreditDebitHistory.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvInvoiceHistory')", "Print Grid", "icon_print.png", "PRINT");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();
           // MenuOrderForm.SetTrigger(pnlOrderForm);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Credit Notes", "CREDITNOTE");
            toolbarmain.AddButton("History", "HISTORY");


            MenuOrderFormMain.AccessRights = this.ViewState;
            //MenuOrderFormMain.Title = "Invoice History";
            MenuOrderFormMain.MenuList = toolbarmain.Show();
          //  MenuOrderFormMain.SetTrigger(pnlOrderForm);


            if (!IsPostBack)
            {
                ViewState["Invoicenumber"] = "";
                if (Request.QueryString["creditdebitnoteid"] != null)
                    ViewState["creditdebitnotecode"] = Request.QueryString["creditdebitnoteid"].ToString();
                else
                    ViewState["creditdebitnotecode"] = Guid.Empty;

                if (Request.QueryString["qfrom"] != null && Request.QueryString["qfrom"].ToString() != string.Empty)
                    ViewState["qfrom"] = Request.QueryString["qfrom"];
                else
                    ViewState["qfrom"] = "";
                SetRedirectURL();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGEURL"] = null;
                if (ViewState["qfrom"] != null && ViewState["qfrom"].ToString() == "creditdebitnote")
                    MenuOrderFormMain.SelectedMenuIndex = 1;
                else
                    MenuOrderFormMain.SelectedMenuIndex = 1;
                BindInvoiceNumber(ViewState["creditdebitnotecode"].ToString());
            }
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvInvoiceHistory_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("CREDITNOTE"))
            {
                Response.Redirect(ViewState["URL"].ToString() + ViewState["creditdebitnotecode"] + "&qcallfrom=creditdebitnote");
            }
            else
                MenuOrderFormMain.SelectedMenuIndex = 1;
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

        ds = PhoenixAccountsCreditDebitNote.CreditNoteHistoryList(new Guid(ViewState["creditdebitnotecode"].ToString())
                                                     , type);

        Response.AddHeader("Content-Disposition", "attachment; filename=AccountsCreditDebitHistory.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Invoice History" + "-" + ViewState["Invoicenumber"] + "</h3></td>");
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
            if (ViewState["qfrom"].ToString() == "creditdebitnote")
            {
                ViewState["URL"] = "../Accounts/AccountsCreditDebitNoteMaster.aspx?creditdebitnoteid=";
            }
        }
    }

    private void BindInvoiceNumber(string creditdebitnotenumber)
    {
        if (General.GetNullableGuid(creditdebitnotenumber) != null)
        {
            DataSet dsInvoice = PhoenixAccountsInvoice.InvoiceEdit(new Guid(creditdebitnotenumber));
            if (dsInvoice.Tables[0].Rows.Count > 0)
            {
                ViewState["Invoicenumber"] = dsInvoice.Tables[0].Rows[0]["FLDINVOICENUMBER"];
                MenuOrderFormMain.Title = "Rebate Receivable" + " - " + dsInvoice.Tables[0].Rows[0]["FLDINVOICENUMBER"].ToString();
               // frmTitle.Text = frmTitle.Text + " - " + dsInvoice.Tables[0].Rows[0]["FLDINVOICENUMBER"].ToString();
            }
        }
    }
    protected void gvInvoiceHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvInvoiceHistory.CurrentPageIndex + 1;
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
        ds = PhoenixAccountsCreditDebitNote.CreditNoteHistoryList(new Guid(ViewState["creditdebitnotecode"].ToString())
                                                     , type);
        General.SetPrintOptions("gvInvoiceHistory", "Accounts Invoice History" + "-" + ViewState["Invoicenumber"], alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvInvoiceHistory.DataSource = ds;
            gvInvoiceHistory.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
          
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void ReBindData(object sender, EventArgs e)
    {
        BindData();
    }

   // protected void gvInvoiceHistory_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
   // {
   //
   //     if (e.Row.RowType == DataControlRowType.Header)
   //     {
   //         if (ViewState["SORTEXPRESSION"] != null)
   //         {
   //             HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
   //             if (img != null)
   //             {
   //                 if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
   //                     img.Src = Session["images"] + "/arrowUp.png";
   //                 else
   //                     img.Src = Session["images"] + "/arrowDown.png";
   //
   //                 img.Visible = true;
   //             }
   //         }
   //     }
   // }
   //
   // protected void gvInvoiceHistory_ItemCommand(object sender, GridViewCommandEventArgs e)
   // {
   //     if (e.CommandName.ToUpper().Equals("SORT"))
   //         return;
   //     GridView _gridView = (GridView)sender;
   //     int iRowno;
   //     iRowno = int.Parse(e.CommandArgument.ToString());
   //
   //     if (e.CommandName.ToUpper().Equals("SORT"))
   //         return;
   // }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

   
}
