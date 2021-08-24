using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System.Collections;
using System.IO;
using System.Web;
using SouthNests.Phoenix.Common;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class AccountsRemittanceBatchList : PhoenixBasePage
{
    protected void Page_Prerender(object sender, EventArgs e)
    {
        SetRowSelection();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none");

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsRemittanceBatchlist.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvRemittence')", "Print Grid", "icon_print.png", "PRINT");
            toolbargrid.AddImageButton("../Accounts/AccountsRemittanceBatchFilter.aspx", "Find", "search.png", "FIND");
            toolbargrid.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Accounts/AccountsRemittanceBankProcess.aspx')", "Add", "add.png", "ADDCOMPANY");
            // toolbargrid.AddImageLink("javascript:Openpopup('codehelp1','','AccountsRemittanceBatchInstructionList.aspx')", "Add", "add.png", "ADDCOMPANY");

            MenuOrderForm.AccessRights = this.ViewState;
            MenuOrderForm.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.Title = "Remittance";
            MenuOrderFormMain.MenuList = toolbar.Show();
            // MenuOrderForm.SetTrigger(pnlRemittance);
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceBatchInstructionList.aspx";

            if (!IsPostBack)
            {
                Session["New"] = "N";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["Batchid"] = null;
                gvRemittence.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                if (Request.QueryString["Batchid"] != null)
                {
                    ViewState["Batchid"] = Request.QueryString["Batchid"].ToString();
                    ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceBatchInstructionList.aspx?Batchid=" + ViewState["Batchid"].ToString();
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRemittence_Sorting(object sender, GridSortCommandEventArgs e)
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

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alCaptions = { "Batch Number", "Payment date", "Payment mode", "Account Code" };
        string[] alColumns = { "FLDBATCHNUMBER", "FLDPAYMENTDATE", "FLDPAYMENTMODENAME", "FLDBANKACCOUNT" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        NameValueCollection nvc = Filter.CurrentRemittenceSelection;

        ds = PhoenixAccountsRemittance.RemittanceBatchSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtRemittenceBatchNumberSearch")) : null
                                                            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlAccountCode")) : null
                                                            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlPaymentmode")) : null
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentFromdateSearch")) : string.Empty
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentTodateSearch")) : string.Empty
                                                            , null
                                                            , null
                                                            , 1
                                                            , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtRemittenceNumber")) : null
                                                            );

        //ds = PhoenixAccountsRemittance.RemittanceBatchSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, null, null, null, null, null, null, null, (int)ViewState["PAGENUMBER"], General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=AccountRemittance.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Accounts Remittance</h3></td>");
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
                Rebind();
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        NameValueCollection nvc = Filter.CurrentRemittenceSelection;

        ds = PhoenixAccountsRemittance.RemittanceBatchSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtRemittenceBatchNumberSearch")) : null
                                                            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlAccountCode")) : null
                                                            , nvc != null ? General.GetNullableInteger(nvc.Get("ddlPaymentmode")) : null
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentFromdateSearch")) : string.Empty
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtPaymentTodateSearch")) : string.Empty
                                                            , null
                                                            , null
                                                            , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                            , gvRemittence.PageSize
                                                            , ref iRowCount
                                                            , ref iTotalPageCount
                                                            , nvc != null ? General.GetNullableString(nvc.Get("txtRemittenceNumber")) : null
                                                            );

        string[] alCaptions = { "Batch Number", "Payment date", "Payment mode", "Account Code" };
        string[] alColumns = { "FLDBATCHNUMBER", "FLDPAYMENTDATE", "FLDPAYMENTMODENAME", "FLDBANKACCOUNT" };

        General.SetPrintOptions("gvRemittence", "Remittance", alCaptions, alColumns, ds);

        gvRemittence.DataSource = ds;
        gvRemittence.VirtualItemCount = iRowCount;

        if (ds.Tables[0].Rows.Count > 0)
        {
            if (ViewState["Batchid"] == null)
            {
                ViewState["Batchid"] = ds.Tables[0].Rows[0]["FLDBATCHID"].ToString();
                // gvRemittence.SelectedIndex = 0;
            }
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceBatchInstructionList.aspx?Batchid=" + ViewState["Batchid"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            if (ViewState["PAGEURL"] == null)
            {
                ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceBatchInstructionList.aspx?Batchid=";
            }
        }
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void Rebind()
    {
        gvRemittence.SelectedIndexes.Clear();
        gvRemittence.EditIndexes.Clear();
        gvRemittence.DataSource = null;
        gvRemittence.Rebind();
    }
    protected void gvRemittence_RowCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;


        if (e.CommandName.ToUpper().Equals("SAVE"))
        {
            if (General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtPaymentDateEdit")).Text) == null)
                ucError.ErrorMessage = "Payment date is required.";
            else
            {
                DateTime? PaymentDate = General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtPaymentDateEdit")).Text);
                if (Convert.ToDateTime(PaymentDate).Date < System.DateTime.Now.Date)
                    ucError.ErrorMessage = "Payment date should be later than or equal to current date.";
            }

            if (ucError.IsError)
            {
                ucError.Visible = true;
                return;
            }
            try
            {
                PhoenixAccountsRemittance.RemittanceInstructionBatchPaymentdateUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(((RadLabel)e.Item.FindControl("lblBatchId")).Text), General.GetNullableDateTime(((UserControlDate)e.Item.FindControl("txtPaymentDateEdit")).Text));
                Rebind();

                //DataSet ds = PhoenixAccountsRemittance.RemittanceInstructionSupplierPayableAmountCSV(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchId")).Text);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    string strpath = HttpContext.Current.Request.MapPath("~/Attachments/ACCOUNTS/");
                //    string filename = strpath + ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString() + ".xlsx";
                //    Guid UBatchId = new Guid(ds.Tables[0].Rows[0]["FLDBATCHID"].ToString());
                //    DataTable dt = PhoenixCommonFileAttachment.AttachmentList(UBatchId, "REMITTANCEBATCH");

                //    DataSet ds1 = new DataSet();
                //    DataTable dt1 = new DataTable();
                //    ds1 = PhoenixAccountsRemittance.RemittanceInstructionSupplierPayableAmountCSV(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchId")).Text);
                //    dt1 = ds1.Tables[1];

                //    PhoenixAccounts2XL.DumpExcel(dt1, filename);
                //    FileInfo f = new FileInfo(filename);
                //    long s1 = f.Length;
                //    if (dt.Rows.Count == 0)
                //    { PhoenixCommonFileAttachment.InsertAttachment(UBatchId, ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString() + ".XLSX", "ACCOUNTS/" + ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString() + ".xlsx", s1, null, null, "REMITTANCEBATCH", new Guid(ds.Tables[0].Rows[0]["FLDBATCHID"].ToString())); }
                //}
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }

        if (e.CommandName.ToUpper().Equals("BANKUPLOAD"))
        {

            try
            {
                PhoenixAccountsRemittance.RemittanceInstructionBatchStatusUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(((RadLabel)e.Item.FindControl("lblBatchId")).Text));
                Rebind();

                //DataSet ds3 = PhoenixAccountsRemittanceBankDownload.GetBankInformation(
                //    ((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchId")).Text
                //    , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblCompanyId")).Text)
                //    , null
                //    , General.GetNullableInteger(((Label)_gridView.Rows[nCurrentRow].FindControl("lblAccountId")).Text));

                DataSet ds2 = PhoenixAccountsRemittanceBankDownload.GetBankDownloadMappedTemplates(
                    ((RadLabel)e.Item.FindControl("lblBatchId")).Text
                    , General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblCompanyId")).Text)
                    , null
                    , General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblAccountId")).Text)
                    );

                DataSet ds = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierPayableAmountCSV(((RadLabel)e.Item.FindControl("lblBatchId")).Text);

                //string strpath = HttpContext.Current.Request.MapPath("~/Attachments/ACCOUNTS/");
                string strpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/ACCOUNTS/";
                string filepathext = ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString() + ".csv";
                string filename = strpath + filepathext;
                Guid UBatchId = new Guid(ds.Tables[0].Rows[0]["FLDBATCHID"].ToString());
                DataTable dt = PhoenixCommonFileAttachment.AttachmentList(UBatchId, "REMITTANCEBATCH");
                int i = 0;
                decimal sum = 0;

                DataSet ds1 = new DataSet();
                DataTable dt1 = new DataTable();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds2.Tables[0].Rows.Count > 0)
                    {
                        if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "CITI-TT" && ds2.Tables[0].Rows[0]["FLDABBREVIATION"].ToString().Contains("MY"))
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierCitibankMYTT(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 1;   
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "GDFF MY-TT-EFT" && ds2.Tables[0].Rows[0]["FLDABBREVIATION"].ToString().Contains("MY"))
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierCitibankMYTTEFT(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 12;
                            filepathext = ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString() + ".txt";
                            filename = strpath + filepathext;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "GDFF MY-ACH-DFT" && ds2.Tables[0].Rows[0]["FLDABBREVIATION"].ToString().Contains("MY"))
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierCitibankMYACHDFT(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 13;
                            filepathext = ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString() + ".txt";
                            filename = strpath + filepathext;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "GDFF SG-TT-DFT" && ds2.Tables[0].Rows[0]["FLDABBREVIATION"].ToString().Contains("SG"))
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierCitibankTTSGD(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 14;
                            filepathext = ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString() + ".txt";
                            filename = strpath + filepathext;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "CITI-FT-SG" && ds2.Tables[0].Rows[0]["FLDABBREVIATION"].ToString().Contains("SG"))
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierCitibankFTSGD(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 15;
                            filepathext = ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString() +"FastTransfer"+ ".txt";
                            filename = strpath + filepathext;
                        }

                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "SCB-HK-ACH")
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierCitibankSCBHKACH(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 16;
                            filepathext = ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString() + ".txt";
                            filename = strpath + filepathext;
                        }

                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "CITI-ACH-CLUB" && ds2.Tables[0].Rows[0]["FLDABBREVIATION"].ToString().Contains("MY"))
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierCitibankMYACH(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 2;
                            //filepathext = ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString() + ".csv";
                            //filename = strpath + filepathext;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "CITI-ACH-SPLIT" && ds2.Tables[0].Rows[0]["FLDABBREVIATION"].ToString().Contains("MY"))
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierCitibankMYACH(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 2;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "CITI-TT-SGD" && ds2.Tables[0].Rows[0]["FLDABBREVIATION"].ToString().Contains("SG") && ds2.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString().Contains("SG"))
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierCitibankSGTTforSGD(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 11;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "CITI-TT-SGD" && ds2.Tables[0].Rows[0]["FLDABBREVIATION"].ToString().Contains("SG") && ds2.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString() != "SG")
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierCitibankSGTT(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 3;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "CITI-TT" && ds2.Tables[0].Rows[0]["FLDABBREVIATION"].ToString().Contains("SG") && ds2.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString() != "SG")
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierCitibankSGTT(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 3;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "CITI-TT" && ds2.Tables[0].Rows[0]["FLDABBREVIATION"].ToString().Contains("SG") && ds2.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString() == "SG")
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierCitibankSGTTforSGD(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 11;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "CITI-ACH-CLUB" && ds2.Tables[0].Rows[0]["FLDABBREVIATION"].ToString().Contains("SG"))
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierCitibankSGACH(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 4;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "CITI-ACH-SPLIT" && ds2.Tables[0].Rows[0]["FLDABBREVIATION"].ToString().Contains("SG"))
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierCitibankSGACH(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 4;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "DBS-ACH")
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierDBSACH(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 5;
                            sum = Convert.ToDecimal(ds1.Tables[2].Rows[0]["SUMOFREMITTANCEAMOUNT"]);
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "DBS-TT")
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierDBSTT(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 6;
                            sum = Convert.ToDecimal(ds1.Tables[2].Rows[0]["SUMOFREMITTANCEAMOUNT"]);
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "SCB-TT")
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierSCBTT(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 8;
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "SCB-ACH")
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierSCBACH(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 9;
                            sum = Convert.ToDecimal(ds1.Tables[2].Rows[0]["SUMOFREMITTANCEAMOUNT"]);
                        }
                        else if (ds2.Tables[0].Rows[0]["FLDTEMPLATECODE"].ToString() == "CITI-HK")
                        {
                            ds1 = PhoenixAccountsRemittanceBankDownload.RemittanceBankInstructionSupplierCitibankHK(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 10;
                            filepathext = ds.Tables[0].Rows[0]["FLDBATCHNUMBER"].ToString() + ".txt";
                            filename = strpath + filepathext;
                        }
                        else
                        {
                            ds1 = PhoenixAccountsRemittance.RemittanceInstructionSupplierPayableAmountCSV(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                            dt1 = ds1.Tables[1];
                            i = 7;
                        }
                    }
                    else
                    {

                        ds1 = PhoenixAccountsRemittance.RemittanceInstructionSupplierPayableAmountCSV(((RadLabel)e.Item.FindControl("lblBatchId")).Text);
                        dt1 = ds1.Tables[1];
                        i = 7;
                        //ds1 = PhoenixAccountsRemittance.RemittanceBankInstructionSupplierDBSACH(((Label)_gridView.Rows[nCurrentRow].FindControl("lblBatchId")).Text);
                        //dt1 = ds1.Tables[1];
                        //i = 7;
                        //sum = Convert.ToInt32(ds1.Tables[2].Rows[0]["SUMOFREMITTANCEAMOUNT"]);
                    }
                    PhoenixAccountsBankDownload2XL.DumpExcel(dt1, filename, i, sum);
                    FileInfo f = new FileInfo(filename);
                    long s1 = f.Length;
                    if (dt.Rows.Count == 0)
                    {
                        PhoenixCommonFileAttachment.InsertAttachment(UBatchId, filepathext, "ACCOUNTS/" + filepathext, s1, null, null, "REMITTANCEBATCH", new Guid(ds.Tables[0].Rows[0]["FLDBATCHID"].ToString()));
                    }
                }
            }
            catch (Exception ex)
            {
                ucError.HeaderMessage = "";
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }

        if (e.CommandName.ToUpper().Equals("APPROVE"))
        {
            try
            {
                if (General.GetNullableDateTime(((RadLabel)e.Item.FindControl("lblPaymentDate")).Text) == null)
                    ucError.ErrorMessage = "Payment date is required.";

                DateTime? PaymentDate = General.GetNullableDateTime(((RadLabel)e.Item.FindControl("lblPaymentDate")).Text);
                if (Convert.ToDateTime(PaymentDate).Date < System.DateTime.Now.Date)
                    ucError.ErrorMessage = "Payment date should be later than or equal to current date.";

                if (ucError.IsError)
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsRemittance.PostRemittance(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ((RadLabel)e.Item.FindControl("lblBatchId")).Text, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
                Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
        else if (e.CommandName.ToUpper().Equals("EDITBATCH"))
        {
            BindPageURL(e.Item.ItemIndex);
            SetRowSelection();

            Rebind();
        }
        else if (e.CommandName.ToUpper().Equals("HISTORY"))
        {
            Response.Redirect("AccountsRemittanceBatchDownLoadHistory.aspx?Batchid=" + ((RadLabel)e.Item.FindControl("lblBatchId")).Text);
        }
        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        else
        {
            Rebind();
        }

    }



    protected void gvRemittence_RowDeleting(object sender, GridCommandEventArgs de)
    {
        Rebind();
    }


    protected void gvRemittence_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridHeaderItem)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Item.FindControl(ViewState["SORTEXPRESSION"].ToString());
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

        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                RadLabel lblVoucherid = (RadLabel)e.Item.FindControl("lblVoucherid");
                RadLabel lblFileName = ((RadLabel)e.Item.FindControl("lblFileName"));
                RadLabel lblIsmodified = (RadLabel)e.Item.FindControl("lblIsmodified");

                Image imgtype = (Image)e.Item.FindControl("imgfiletype");
                ImageButton cmdPost = (ImageButton)e.Item.FindControl("cmdPost");
                ImageButton imgUpload = (ImageButton)e.Item.FindControl("imgUpload");


                if (cmdPost != null)
                    cmdPost.Attributes.Add("onclick", "javascript:this.onclick=function(){ return false;};");// this will fix the multiple click problem

                if (lblIsmodified != null && lblIsmodified.Text != "" && lblIsmodified.Text != "2")
                {
                    if (lblVoucherid.Text != "")
                        imgUpload.Visible = true;
                }
                else
                {
                    if (lblFileName != null)
                    {
                        if (lblFileName.Text != string.Empty)
                        {
                            RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                            HyperLink lnk = (HyperLink)e.Item.FindControl("lnkfilename");
                            lnk.NavigateUrl = "../accounts/download.aspx?filename=" + lblFileName.Text + "&filepath=" + PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + lblFilePath.Text; //Session["sitepath"] + "/attachments/" + lblFilePath.Text;
                            //lnk.NavigateUrl = Session["sitepath"] + "/attachments/" + lblFilePath.Text;
                            if (lblVoucherid.Text == string.Empty)
                            {
                                lnk.Visible = false;
                            }
                            else
                            {
                                lnk.Visible = true;
                            }
                        }
                    }
                }

                if (lblVoucherid != null)
                {
                    if (lblVoucherid.Text != string.Empty)
                    {
                        if (cmdPost != null)
                            cmdPost.Visible = false;
                    }
                }

            }
        }
    }


    private string ResolveImageType(string extn)
    {
        string imagepath = Session["images"] + "/";
        if (string.IsNullOrEmpty(extn)) extn = string.Empty;
        switch (extn.ToLower())
        {
            case ".jpg":
            case ".png":
            case ".gif":
            case ".bmp":
                imagepath += "imagefile.png";
                break;
            case ".doc":
                imagepath += "word.png";
                break;
            case ".xls":
                imagepath += "xls.png";
                break;
            case ".pdf":
                imagepath += "pdf.png";
                break;
            case ".rar":
            case ".zip":
                imagepath += "rar.png";
                break;
            case ".txt":
                imagepath += "notepad.png";
                break;
            default:
                imagepath += "anyfile.png";
                break;
        }
        return imagepath;
    }

    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("GENERATE"))
            {
                string selectedagents = ",";
                if (Session["CHECKED_ITEMS"] != null)
                {
                    ArrayList SelectedPvs = (ArrayList)Session["CHECKED_ITEMS"];
                    if (SelectedPvs != null && SelectedPvs.Count > 0)
                    {
                        foreach (string index in SelectedPvs)
                        { selectedagents = selectedagents + index + ","; }
                    }
                }

                if (selectedagents.Length > 10)
                {
                    PhoenixAccountsRemittance.RemittanceInstructionBatchGenerate(PhoenixSecurityContext.CurrentSecurityContext.UserCode, PhoenixSecurityContext.CurrentSecurityContext.CompanyID, selectedagents.Length > 1 ? selectedagents : null);
                }
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvRemittence$ctl01$chkAllRemittance")
        {
            GridHeaderItem headerItem = gvRemittence.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            RadCheckBox chkAll = (RadCheckBox)headerItem.FindControl("chkAllRemittance");
            foreach (GridViewRow row in gvRemittence.Items)
            {
                RadCheckBox cbSelected = (RadCheckBox)row.FindControl("chkSelect");
                if (chkAll.Checked == true)
                {
                    cbSelected.Checked = true;
                }
                else
                {
                    cbSelected.Checked = false;
                }
            }
        }
    }



    //private void SetRowSelection()
    //{
    //    //gvRemittence.SelectedIndex = -1;
    //    //for (int i = 0; i < gvRemittence.Rows.Count; i++)
    //    //{
    //    //    if (gvRemittence.DataKeys[i].Value.ToString().Equals(ViewState["Batchid"].ToString()))
    //    //    {
    //    //        gvRemittence.SelectedIndex = i;
    //    //        //   PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvRemittence.Rows[i].FindControl("lnkRemittenceid")).Text;
    //    //    }
    //    //}
    //    if (gvRemittence.SelectedItems.Count > 0)
    //    {
    //        DataRowView drv = (DataRowView)gvRemittence.SelectedItems[0].DataItem;
    //        PhoenixAccountsVoucher.VoucherNumber = drv["FLDBATCHID"].ToString();
    //    }
    //    else
    //    {
    //        if (gvRemittence.MasterTableView.Items.Count > 0)
    //        {
    //            gvRemittence.MasterTableView.Items[0].Selected = true;
    //            //PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvRemittence.Items[0].FindControl("lnkRemittenceid")).Text;
    //        }
    //    }
    //}

    private void SetRowSelection()
    {
        foreach (GridDataItem item in gvRemittence.Items)
        {

            if (item.GetDataKeyValue("FLDBATCHID").ToString().Equals(ViewState["Batchid"].ToString()))
            {
                gvRemittence.SelectedIndexes.Add(item.ItemIndex);
                //ViewState["DOCUMENTID"] = item.GetDataKeyValue("FLDDOCUMENTID").ToString();
            }
        }
    }


    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["Batchid"] = ((RadLabel)gvRemittence.Items[rowindex].FindControl("lblBatchId")).Text;
            //PhoenixAccountsVoucher.VoucherNumber = ((LinkButton)gvRemittence.Rows[rowindex].FindControl("lnkRemittenceid")).Text;
            ifMoreInfo.Attributes["src"] = "../Accounts/AccountsRemittanceBatchInstructionList.aspx?Batchid=" + ViewState["Batchid"].ToString();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();

        if (Session["New"].ToString() == "Y")
        {
            Session["New"] = "N";
            if (gvRemittence.Items.Count > 0)
                BindPageURL(0);
        }
    }


    protected void gvRemittence_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRemittence.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
