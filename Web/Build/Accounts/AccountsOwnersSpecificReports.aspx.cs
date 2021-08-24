using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Accounts;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using System.Text;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Export2XL;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.IO;
using Telerik.Web.UI;


public partial class Accounts_AccountsOwnersSpecificReports : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("SOA", "SOA");
            //toolbarmain.AddButton("Budget Code", "VOUCHER");

            PhoenixToolbar toolbarowneraccounts = new PhoenixToolbar();
            MenuAccountsowner.AccessRights = this.ViewState;
            toolbarowneraccounts.AddImageButton("../Accounts/AccountsOwnersSpecificReports.aspx", "Find", "search.png", "FIND");
            //if (SessionUtil.CanAccess(this.ViewState, "PRINT"))
            //    toolbarowneraccounts.AddImageLink("javascript:CallPrint('gvOwnersAccount')", "Print Grid", "icon_print.png", "PRINT");
            //if (SessionUtil.CanAccess(this.ViewState, "Excel"))
            //    toolbarowneraccounts.AddImageButton("../Accounts/AccountsKOYOSpecificReports.aspx", "Export to Excel", "icon_xls.png", "Excel");
            MenuAccountsowner.AccessRights = this.ViewState;
            MenuAccountsowner.MenuList = toolbarowneraccounts.Show();

            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.Title = "KOYO Specific Reports";
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            //MenuOrderFormMain.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["accountid"] = "";
                ViewState["debitnotereference"] = "";
                ViewState["Ownerid"] = "";
                //ddlVessel.Items.Insert(0, new ListItem("--Select--", "Dummy"));
                ucOwner_Onchange();
            }
            //uservesselmap();
            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void ucOwner_Onchange()
    {
        DataSet dsveaaelname = new DataSet();
        dsveaaelname = PhoenixCommonBudgetGroupAllocation.AccountsOwnerVessellist(General.GetNullableInteger("3811"));
        ddlVessel.DataSource = dsveaaelname;
        ddlVessel.DataTextField = "FLDVESSELNAME";
        ddlVessel.DataValueField = "FLDVESSELID";
        ddlVessel.DataBind();
    }


    protected void MenuOrderFormMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("VOUCHER"))
            {
                if (ViewState["accountid"].ToString() == "" || ViewState["debitnotereference"].ToString() == "" || ViewState["Ownerid"].ToString() == "")
                {
                    ucError.ErrorMessage = "Select a Account";
                    ucError.Visible = true;
                    return;
                }
                else
                    Response.Redirect("../Accounts/AccountsOwnersStatementOfAccounts.aspx?accountid=" + ViewState["accountid"].ToString() + "&debitnotereference=" + ViewState["debitnotereference"].ToString() + "&Ownerid=" + ViewState["Ownerid"].ToString(), true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void AccountsownerMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {

            ViewState["PAGENUMBER"] = 1;
            BindData();
        }

        else if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDDEBITNOTEREFERENCE", "FLDHARDNAME", "FLDQUICKNAME" };
        string[] alCaptions = { "Vessel Name", "SOA Reference", "Month", "Year" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsOwnerStatementOfAccounts.StatementOfAccountsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , sortexpression, sortdirection,
                                                (int)ViewState["PAGENUMBER"],
                                                iRowCount,
                                                ref iRowCount,
                                                ref iTotalPageCount, General.GetNullableInteger(null), "", "", "", General.GetNullableInteger(null), General.GetNullableInteger(null));

        Response.AddHeader("Content-Disposition", "attachment; filename=StatementOfAccounts.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Statement of Accounts</center></h3></td>");
        //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
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

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDDEBITNOTEREFERENCE", "FLDHARDNAME", "FLDQUICKNAME" };
        string[] alCaptions = { "Vessel Name", "SOA Reference", "Month", "Year" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixAccountsOwnerStatementOfAccounts.StatementOfAccountsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , sortexpression, sortdirection,
                                                 (int)ViewState["PAGENUMBER"],
                                                 gvOwnersAccount.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount,
                                                 General.GetNullableInteger(ddlVessel.SelectedValue),
                                                 General.GetNullableString(null),
                                                 General.GetNullableString(null),
                                                 General.GetNullableString(null),
                                                 null,
                                                 null);

        gvOwnersAccount.DataSource = ds;
        gvOwnersAccount.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOwnersAccount", "Statement of Accounts", alCaptions, alColumns, ds);
    }

    protected void gvOwnersAccount_ItemDataBound(Object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
            ImageButton cmdExport2XL = (ImageButton)e.Item.FindControl("cmdExport2XL");

            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkAccountCOde");

            DataRowView drv = (DataRowView)e.Item.DataItem;

            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.Openpopup('att','','../Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.ACCOUNTS + "&U=NO'); return false;");
            }

            if (cmdExport2XL != null)
            {
                cmdExport2XL.Visible = true;

                if (!SessionUtil.CanAccess(this.ViewState, cmdExport2XL.CommandName))
                {
                    cmdExport2XL.Visible = false;
                }
            }
        }
    }

    protected void gvOwnersAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                ViewState["accountid"] = (RadLabel)e.Item.FindControl("lblAccountId");

                RadLabel lblAccountCode = (RadLabel)e.Item.FindControl("lblAccountCode");

                RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
                ViewState["debitnotereference"] = (RadLabel)e.Item.FindControl("lblSoaReference");

                RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
                ViewState["Ownerid"] = null;

                Response.Redirect("../Accounts/AccountsOwnerStatementOfAccountBudget.aspx?accountid="
                     + lblAccountId.Text + "&debitnoteref="
                     + lblDebitNoteReference.Text + "&accountcode="
                     + lblAccountCode.Text + "&SUPPORTINGSYN=NO", true);
            }

            if (e.CommandName.ToUpper().Equals("EXPORT2XL"))
            {
                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
                RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
                RadLabel lblAccountCode = (RadLabel)e.Item.FindControl("lblAccountCode");
                //Export2XLMonthlyVoucherDetails(General.GetNullableInteger(lblAccountId.Text), General.GetNullableString(lblAccountCode.Text),
                //                                                        General.GetNullableString(lblDebitNoteReference.Text), General.GetNullableInteger(lblOwnerid.Text));
                Export2XLMonthlyVoucherDetails(General.GetNullableInteger(lblAccountId.Text), General.GetNullableString(lblAccountCode.Text),
                                                                                    General.GetNullableString(lblDebitNoteReference.Text), General.GetNullableInteger(lblOwnerid.Text));

            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public static void Export2XLMonthlyVoucherDetails(int? accountId, string accountcode, string debitnotereference, int? ownerid)
    {
        //string file = HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts") + @"\OwnerSpecifcDetails.xlsm";
        //string destinationpath = HttpContext.Current.Server.MapPath("..\\Attachments\\Accounts\\") + @"OwnerSpecifc_Details.xlsm";

        string file = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + "Accounts/" + @"\OwnerSpecifcDetails.xlsm";
        string destinationpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + "Accounts/" + @"OwnerSpecifc_Details.xlsm";

        FileInfo Inputfile = new FileInfo(file);
        if (File.Exists(destinationpath)) File.Delete(destinationpath);
        Inputfile.CopyTo(destinationpath);

        FileInfo fidestination = new FileInfo(destinationpath);

        using (ExcelPackage pck = new ExcelPackage(fidestination))
        {
            PopulateAccountsInformation(pck, accountId, accountcode, debitnotereference, ownerid);
            HttpContext.Current.Response.Clear();
            pck.SaveAs(HttpContext.Current.Response.OutputStream);
            HttpCookie token = HttpContext.Current.Request.Cookies["fileDownloadToken"];
            if (token != null)
            {
                token.Expires = DateTime.Now.AddDays(-1d);
                HttpContext.Current.Response.Cookies.Add(token);
            }
            //HttpContext.Current.Response.ContentType = "application/vnd.ms-excel.sheet.macroEnabled.12 .xltm";
            HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment;  filename=OwnerSpecifcDetails.xlsm");
            HttpContext.Current.Response.End();
        }
    }


    private static void PopulateAccountsInformation(ExcelPackage pck, int? accountId, string accountcode, string debitnotereference, int? ownerid)
    {
        ExcelWorksheet ws = pck.Workbook.Worksheets["DATA"];

        DataSet ds = PhoenixAccountsOwnerStatementOfAccounts.StatementOfOwnerSpecificReport(General.GetNullableInteger(accountId.ToString())
                                                                                                , General.GetNullableString(accountcode)
                                                                                                , General.GetNullableString(debitnotereference)
                                                                                                , General.GetNullableInteger(ownerid.ToString()));
        DataTable dt = ds.Tables[0];



        int nrow = 2;
        if (dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {

                ws.Cells[nrow, 1].Value = dr["FLDDEBITNOTEREFERENCEID"];
                ws.Cells[nrow, 2].Value = dr["FLDOWNERBUDGETGROUPID"];
                ws.Cells[nrow, 3].Value = dr["FLDACCOUNTCODE"];
                ws.Cells[nrow, 4].Value = dr["FLDOWNERBUDGETGROUP"];
                //ws.Cells[nrow, 2].Value = dr["FLDOWNERBUDGETGROUPID"];
                ws.Cells[nrow, 5].Value = dr["FLDOWNERBUDGETCODE"];
                // ws.Cells[nrow, 4].Value = dr["FLDPARENTOWNERBUDGETGROUPID"];

                // ws.Cells[nrow, 6].Value = dr["FLDGRANTPARENTOWNERBUDGETGROUPID"];

                //  ws.Cells[nrow, 8].Value = dr["FLDTOPPARENTOWNERBUDGETGROUPID"];

                ws.Cells[nrow, 6].Value = dr["FLDLONGDESCRIPTION"];
                ws.Cells[nrow, 7].Value = dr["FLDTOTALAMOUNTDR"];
                ws.Cells[nrow, 8].Value = dr["FLDTOTALAMOUNTCR"];
                ws.Cells[nrow, 9].Value = dr["FLDDEBITNOTEREFERENCE"];
                ws.Cells[nrow, 10].Value = dr["FLDMONTH"];
                ws.Cells[nrow, 11].Value = dr["FLDYEAR"];
                ws.Cells[nrow, 12].Value = dr["FLDMONTHYEAR"];
                ws.Cells[nrow, 13].Value = dr["FLDACCOUNTDESC"];
                nrow = nrow + 1;


            }

        }

        ExcelWorksheet ws1 = pck.Workbook.Worksheets["SA"];

        ws1.Cells["A7"].Value = "VESSEL:" + dt.Rows[0]["FLDACCOUNTDESC"];

        ws1.Cells["A14"].Value = "PERIOD COVERED : FOR THE MONTH OF " + dt.Rows[0]["FLDMONTHYEAR"].ToString();

    }
    protected void gvOwnersAccount_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOwnersAccount.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvOwnersAccount.SelectedIndexes.Clear();
        gvOwnersAccount.EditIndexes.Clear();
        gvOwnersAccount.DataSource = null;
        gvOwnersAccount.Rebind();
    }
}
