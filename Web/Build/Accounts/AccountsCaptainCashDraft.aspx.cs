using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using SouthNests.Phoenix.Reports;
using System.Web;
using Telerik.Web.UI;

public partial class AccountsCaptainCashDraft : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Voucher", "VOUCHER");
            toolbar.AddButton("Line Items", "LINEITEM");
            toolbar.AddButton("View Draft", "VIEWDRAFT");
            toolbar.AddButton("D11", "D11");
            toolbar.AddButton("Captain Cash", "CAPTAINCASH");
            toolbar.AddButton("Log", "LOG");
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            MenuReportsFilter.SelectedMenuIndex = 2;

            if (Request.QueryString["balanceid"] != null && Request.QueryString["balanceid"] != string.Empty)
                ViewState["balanceid"] = Request.QueryString["balanceid"];

            if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"] != string.Empty)
                ViewState["vesselid"] = Request.QueryString["vesselid"];
            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            string vesselid, balanceid;
            if (Request.QueryString["vesselid"] != null)
                vesselid = Request.QueryString["vesselid"].ToString();
            else
                vesselid = "";
            if (Request.QueryString["balanceid"] != null)
                balanceid = Request.QueryString["balanceid"].ToString();
            else
                balanceid = "";
            DataSet ds = PhoenixAccountsCaptainCash.OfficeCaptainCashEdit(int.Parse(vesselid), new Guid(balanceid));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                //   ucTitle.Text = dr["FLDVESSELNAME"].ToString() + " - Period Of  " + dr["FLDSDATE"].ToString() + " to " + dr["FLDEDATE"].ToString();                 
                ViewState["month"] = dr["FLDMONTH"].ToString();
                ViewState["year"] = dr["FLDYEAR"].ToString(); ViewState["from"] = dr["FLDSDATE"].ToString(); ViewState["to"] = dr["FLDEDATE"].ToString();

            }
            toolbar2.AddButton("Post Voucher", "POST", ToolBarDirection.Right);
            MenuPost.AccessRights = this.ViewState;
            MenuPost.MenuList = toolbar2.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddImageButton("../Accounts/AccountsCaptainCashDraft.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString() + "", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbar1.AddImageLink("javascript:CallPrint('gvCaptainPettyCash')", "Print Grid", "icon_print.png", "PRINT");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PAGENUMBER2"] = 1;
                ViewState["SORTEXPRESSION2"] = null;
                ViewState["SORTDIRECTION2"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                Response.Redirect("../Accounts/AccountsCaptainCash.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString());
            }
            if (CommandName.ToUpper().Equals("LINEITEM"))
            {
                Response.Redirect("../Accounts/AccountsCaptainCashVoucher.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString());
            }
            if (CommandName.ToUpper().Equals("VIEWDRAFT"))
            {
                Response.Redirect("../Accounts/AccountsCaptainCashDraft.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString());
            }
            if (CommandName.ToUpper().Equals("D11"))
            {
                Response.Redirect("../Accounts/AccountsCTMPostingReport.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString() + "&format=D11");
            }
            if (CommandName.ToUpper().Equals("CAPTAINCASH"))
            {
                Response.Redirect("../Accounts/AccountsCTMPostingReport.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString() + "&format=CAPTAINCASH");
            }
            if (CommandName.ToUpper().Equals("LOG"))
            {
                Response.Redirect("../Accounts/AccountsCTMPostingReport.aspx?balanceid=" + ViewState["balanceid"].ToString() + "&vesselid=" + ViewState["vesselid"].ToString() + "&format=LOG");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPost_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("POST"))
            {
                Guid VoucherCaptainFileDtKey = Guid.Empty;
                Guid VoucherD11FileDtKey = Guid.Empty;
                PhoenixAccountsCaptainCash.OfficeCaptainCashVoucherPosting(int.Parse(ViewState["vesselid"].ToString()), new Guid(ViewState["balanceid"].ToString()), ref VoucherCaptainFileDtKey, ref VoucherD11FileDtKey);
                AttachPD(VoucherD11FileDtKey, "1", 5); //D11 REPORT
                AttachPD(VoucherCaptainFileDtKey, "2", 2); //D11 REPORT
                ucStatus.Text = "Voucher Posted.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void AttachPD(Guid Dtkey, string Type, int arr)
    {
        string Tmpfilelocation = string.Empty; string[] reportfile = new string[arr];
        DataSet ds = new DataSet();
        if (Type == "1")
        {

            ds = PhoenixVesselAccountsReports.ReportD11(General.GetNullableInteger(ViewState["vesselid"].ToString()),
                                                                (byte)Convert.ToInt32(ViewState["month"].ToString()),
                                                                Convert.ToInt32((ViewState["year"].ToString())));
            reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsVesselAccountsD11.rpt");
            reportfile[1] = "ReportsVesselAccountsD11ProvisionPurchase.rpt";
            reportfile[2] = "ReportsVesselAccountsD11BondPurchase.rpt";
            reportfile[3] = "ReportsVesselAccountsD11ProvisionConsumption.rpt";
            reportfile[4] = "ReportsVesselAccountsD11BondConsumption.rpt";
        }
        else if (Type == "2")
        {
            ds = PhoenixVesselAccountsReports.ReportCaptainCash(General.GetNullableInteger(ViewState["vesselid"].ToString()),
                                                                Convert.ToDateTime(ViewState["from"].ToString()),
                                                                Convert.ToDateTime((ViewState["to"].ToString())));
            reportfile[0] = HttpContext.Current.Server.MapPath("../Reports/ReportsVesselAccountsCaptainCashMain.rpt");
            reportfile[1] = "ReportsVesselAccountsCaptainPettyExpenses.rpt";
        }

        Tmpfilelocation = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/Accounts/";
        string fileid = Dtkey.ToString();
        string filename = fileid + ".pdf";
        Tmpfilelocation = Tmpfilelocation + filename;
        PhoenixReportClass.ExportReport(reportfile, Tmpfilelocation, ds);
    }
    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDTYPENAME", "FLDLOGTYPENAME", "FLDBUDGETCODE", "FLDDEBIT", "FLDCREDIT" };
        string[] alCaptions = { "", "Component Type", "Budget Code", "Debit", "Credit" };

        string vesselid, balanceid;

        if (Request.QueryString["vesselid"] != null)
            vesselid = Request.QueryString["vesselid"].ToString();
        else
            vesselid = "";

        if (Request.QueryString["balanceid"] != null)
            balanceid = Request.QueryString["balanceid"].ToString();
        else
            balanceid = "";

        ds = PhoenixAccountsCaptainCash.OfficeCaptainCashPostingDraftView(int.Parse(vesselid), new Guid(balanceid));

        Response.AddHeader("Content-Disposition", "attachment; filename=CaptainPettyCash.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:12px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>EXECUTIVE SHIP MANAGEMENT</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;'>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void ShowReport()
    {
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDTYPENAME", "FLDLOGTYPENAME", "FLDBUDGETCODE", "FLDDEBIT", "FLDCREDIT" };
        string[] alCaptions = { "", "Component Type", "Budget Code", "Debit", "Credit" };
        string vesselid, balanceid;

        if (Request.QueryString["vesselid"] != null)
            vesselid = Request.QueryString["vesselid"].ToString();
        else
            vesselid = "";

        if (Request.QueryString["balanceid"] != null)
            balanceid = Request.QueryString["balanceid"].ToString();
        else
            balanceid = "";
        ds = PhoenixAccountsCaptainCash.OfficeCaptainCashPostingDraftView(int.Parse(vesselid), new Guid(balanceid));
        General.SetPrintOptions("gvCaptainPettyCash", "Captain Cash", alCaptions, alColumns, ds);
        gvCaptainPettyCash.DataSource = ds;
        gvCaptainPettyCash.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
        
        if (ds.Tables[0].Rows.Count > 0)
        {
            gvCaptainPettyCash.MasterTableView.GetColumn("CREDIT").FooterText = ds.Tables[0].Rows[0]["FLDSUMCREDIT"].ToString();
            gvCaptainPettyCash.MasterTableView.GetColumn("DEBIT").FooterText = ds.Tables[0].Rows[0]["FLDSUMDEBIT"].ToString();
        }
        //Label credit = (Label)gvCaptainPettyCash.FooterRow.FindControl("lblCredit");
        //Label debit = (Label)gvCaptainPettyCash.FooterRow.FindControl("lbldebit");
        //credit.Text = ds.Tables[0].Rows[0]["FLDSUMCREDIT"].ToString();
        //debit.Text = ds.Tables[0].Rows[0]["FLDSUMDEBIT"].ToString();
    }
    decimal qtyTotal = 0;
    decimal grQtyTotal = 0;
    string storid = string.Empty;


    protected void gvCaptainPettyCash_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ShowReport();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCaptainPettyCash_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridGroupFooterItem)
            {
                GridGroupFooterItem groupFooter = (GridGroupFooterItem)e.Item;
                groupFooter.Style.Add("font-weight", "bold");
            }
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                storid = drv["FLDTYPENAME"].ToString();
                decimal tmpTotaldebit = decimal.Parse(drv["FLDDEBIT"].ToString() == "" ? "0" : drv["FLDDEBIT"].ToString());
                decimal tmpTotalCredit = decimal.Parse(drv["FLDCREDIT"].ToString() == "" ? "0" : drv["FLDCREDIT"].ToString());
                qtyTotal += tmpTotaldebit;
                grQtyTotal += tmpTotalCredit;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void gvCaptainPettyCash_OnCustomAggregate(object sender, GridCustomAggregateEventArgs e)
    //{
    //    string colName = e.Column.UniqueName;

    //    if (e.Item is GridGroupFooterItem)
    //    {
    //        GridGroupHeaderItem correspondingHeader = (e.Item as GridGroupFooterItem).GroupHeaderItem;
    //        if (((Telerik.Web.UI.GridBoundColumn)e.Column).UniqueName == "CheckAmount")
    //        {

    //            decimal checkAmount = 0;
    //            //foreach (GridDataItem item in gvCaptainPettyCash.MasterTableView.Items)
    //            //{
    //            //    checkAmount += decimal.Parse(item["CheckAmount"].Text.Replace("", "0"));

    //            //}
    //            e.Result = checkAmount;
    //        }
    //        if (colName == "CURRENCY")
    //        {
    //            decimal counter = 0;
    //            GridItem[] groupChildItems = correspondingHeader.GetChildItems();
    //            for (int i = 0; i < groupChildItems.Length; i++)
    //            {
    //                decimal retained = (decimal)DataBinder.Eval((groupChildItems[i] as GridDataItem).DataItem, "FLDRETAINEDAMOUNT");
    //                decimal refund = (decimal)DataBinder.Eval((groupChildItems[i] as GridDataItem).DataItem, "FLDREFUNDAMOUNT");

    //                counter += retained - refund;
    //            }
    //            e.Result = counter;
    //        }

    //        if (colName == "SUBTOTAL")
    //        {
    //            e.Result = "Sub Total :";
    //        }
    //    }
    //}

    protected void gvCaptainPettyCash_CustomAggregate(object sender, GridCustomAggregateEventArgs e)
        {

    }
}
