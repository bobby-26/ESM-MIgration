using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Owners;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using System.Text;
using Telerik.Web.UI;

public partial class Owners_OwnersStatementOfAccounts : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("SOA", "SOA");
            toolbarmain.AddButton("Budget Code", "VOUCHER");
            PhoenixToolbar toolbarowneraccounts = new PhoenixToolbar();
            Menuowneraccountsstatement.AccessRights = this.ViewState;
            if (SessionUtil.CanAccess(this.ViewState, "Excel"))
                toolbarowneraccounts.AddFontAwesomeButton("../Owners/OwnersStatementOfAccounts.aspx", "Export to Excel", "<i class=\"fa fa-file-excel\"></i>", "Excel");
            if (SessionUtil.CanAccess(this.ViewState, "PRINT"))
                toolbarowneraccounts.AddFontAwesomeButton("javascript:CallPrint('gvOwnersAccount')", "Print Grid", "<i class=\"fa fa-print\"></i>", "PRINT");
            toolbarowneraccounts.AddFontAwesomeButton("../Owners/OwnersStatementOfAccounts.aspx", "Find", "<i class=\"fa fa-search\"></i>", "FIND");
            toolbarowneraccounts.AddFontAwesomeButton("../Owners/OwnersStatementOfAccounts.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            Menuowneraccountsstatement.AccessRights = this.ViewState;
            Menuowneraccountsstatement.MenuList = toolbarowneraccounts.Show();
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            MenuOrderFormMain.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["accountid"] = "";
                ViewState["debitnotereference"] = "";
                ViewState["Ownerid"] = "";
                Getvesselid();
                BindLatestYearMonth();
                gvOwnersAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindLatestYearMonth()
    {
        DataSet ds = PhoenixOwnersStatementOfAccounts.BindLatestYearMonth(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        if (ds.Tables[0].Rows.Count > 0)
        {
            NameValueCollection nvc = Filter.CurrentOwnerSOAReference;
            if (nvc != null)
            {
                ddlVesselAccount.SelectedValue = nvc.Get("ddlVesselAccount").ToString();
                ucYear.SelectedQuick = nvc.Get("ucYear").ToString() == "Dummy" ? "0" : nvc.Get("ucYear").ToString();
                ucMonth.SelectedHard = nvc.Get("ucMonth").ToString() == "Dummy" ? "0" : nvc.Get("ucMonth").ToString();
            }
            else
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ViewState["CURRENTYEARCODE"] = dr["FLDYEAR"].ToString();
                ViewState["CURRENTMONTHCODE"] = dr["FLDMONTH"].ToString();
                ucYear.SelectedQuick = dr["FLDYEAR"].ToString();
                ucMonth.SelectedHard = dr["FLDMONTH"].ToString();
            }
        }

    }
    protected void Rebind()
    {
        gvOwnersAccount.SelectedIndexes.Clear();
        gvOwnersAccount.EditIndexes.Clear();
        gvOwnersAccount.DataSource = null;
        gvOwnersAccount.Rebind();
    }
    protected void ownerstatementofaccountsMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ddlVesselAccount", ddlVesselAccount.SelectedValue);
                criteria.Add("ucYear", ucYear.SelectedQuick);
                criteria.Add("ucMonth", ucMonth.SelectedHard);
                Filter.CurrentOwnerSOAReference = criteria;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlVesselAccount.SelectedValue = "";
                ucMonth.SelectedHard = "";
                ucYear.SelectedQuick = "";
                Filter.CurrentOwnerSOAReference = null;
                BindLatestYearMonth();
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

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
                    Response.Redirect("../Owners/OwnersStatementOfAccountsVoucher.aspx?accountid=" + ViewState["accountid"].ToString() + "&debitnotereference=" + ViewState["debitnotereference"].ToString() + "&Ownerid=" + ViewState["Ownerid"].ToString(), true);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Getvesselid()
    {

        ddlVesselAccount.DataSource = PhoenixOwnersStatementOfAccounts.GetuservesselAccount(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlVesselAccount.DataTextField = "FLDDESCRIPTION";
        ddlVesselAccount.DataValueField = "FLDACCOUNTID";
        ddlVesselAccount.DataBind();
        ddlVesselAccount.Items.Insert(0, new DropDownListItem("--Select--", ""));

    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentOwnerSOAReference;

        string[] alColumns = { "FLDDESCRIPTION", "FLDDEBITNOTEREFERENCE", "FLDHARDNAME", "FLDQUICKNAME" };
        string[] alCaptions = { "Account Name", "SOA Reference", "Month", "Year" };

        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        int? month;
        int? year;
        if (ucYear.SelectedQuick.ToUpper() != "" && ucYear.SelectedQuick.ToUpper() != "DUMMY")
            year = General.GetNullableInteger(ucYear.SelectedValue);
        else
        {
            if (ViewState["CURRENTYEARCODE"] != null)
                year = General.GetNullableInteger(ViewState["CURRENTYEARCODE"].ToString());
            else
                year = General.GetNullableInteger(ucYear.SelectedValue.ToString());
        }

        if (ucMonth.SelectedHard.ToUpper() != "" && ucMonth.SelectedHard.ToUpper() != "DUMMY")
            month = General.GetNullableInteger(ucMonth.SelectedHard);
        else
        {
            if (ViewState["CURRENTMONTHCODE"] != null)
                month = General.GetNullableInteger(ViewState["CURRENTMONTHCODE"].ToString());
            else
                month = General.GetNullableInteger(ucMonth.SelectedHard);
        }

        ds = PhoenixOwnersStatementOfAccounts.StatementOfAccountsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , sortexpression, sortdirection,
                                                            (int)ViewState["PAGENUMBER"],
                                                          gvOwnersAccount.PageSize,
                                                            ref iRowCount,
                                                            ref iTotalPageCount,
                                                            General.GetNullableInteger(ddlVesselAccount.SelectedValue),
                                                            General.GetNullableString(null),
                                                            General.GetNullableString(null),
                                                            General.GetNullableString(null),
                                                            nvc != null ? General.GetNullableInteger(nvc.Get("ucMonth")) : month,
                                                            nvc != null ? General.GetNullableInteger(nvc.Get("ucYear")) : year);


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

        string[] alColumns = { "FLDDESCRIPTION", "FLDDEBITNOTEREFERENCE", "FLDHARDNAME", "FLDQUICKNAME" };
        string[] alCaptions = { "Account Name", "SOA Reference", "Month", "Year" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? month;
        int? year;
        if (ucYear.SelectedQuick.ToUpper() != "" && ucYear.SelectedQuick.ToUpper() != "DUMMY")
            year = General.GetNullableInteger(ucYear.SelectedValue);
        else
        {
            if (ViewState["CURRENTYEARCODE"] != null)
                year = General.GetNullableInteger(ViewState["CURRENTYEARCODE"].ToString());
            else
                year = General.GetNullableInteger(ucYear.SelectedValue.ToString());
        }

        if (ucMonth.SelectedHard.ToUpper() != "" && ucMonth.SelectedHard.ToUpper() != "DUMMY")
            month = General.GetNullableInteger(ucMonth.SelectedHard);
        else
        {
            if (ViewState["CURRENTMONTHCODE"] != null)
                month = General.GetNullableInteger(ViewState["CURRENTMONTHCODE"].ToString());
            else
                month = General.GetNullableInteger(ucMonth.SelectedHard);
        }
        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentOwnerSOAReference;

        ds = PhoenixOwnersStatementOfAccounts.StatementOfAccountsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                            , sortexpression, sortdirection,
                                                            (int)ViewState["PAGENUMBER"],
                                                            gvOwnersAccount.PageSize,
                                                            ref iRowCount,
                                                            ref iTotalPageCount,
                                                            General.GetNullableInteger(ddlVesselAccount.SelectedValue),
                                                            General.GetNullableString(null),
                                                            General.GetNullableString(null),
                                                            General.GetNullableString(null),
                                                            nvc != null ? General.GetNullableInteger(nvc.Get("ucMonth")) : month,
                                                            nvc != null ? General.GetNullableInteger(nvc.Get("ucYear")) : year);


        gvOwnersAccount.DataSource = ds;
        gvOwnersAccount.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOwnersAccount", "Statement of Accounts", alCaptions, alColumns, ds);
        ViewState["CURRENTYEARCODE"] = null;
        ViewState["CURRENTMONTHCODE"] = null;
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();

    }


    protected void gvOwnersAccount_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkAccountCOde");

            //lbr.Attributes.Add

            DataRowView drv = (DataRowView)e.Item.DataItem;


            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
               + PhoenixModule.ACCOUNTS + "&U=NO'); return false;");
            }
        }
    }
    protected void gvOwnersAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                ViewState["accountid"] = (RadLabel)e.Item.FindControl("lblAccountId");
                RadLabel lblAccountCode = (RadLabel)e.Item.FindControl("lblAccountCode");
                RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
                ViewState["debitnotereference"] = (RadLabel)e.Item.FindControl("lblSoaReference");
                RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
                ViewState["Ownerid"] = (RadLabel)e.Item.FindControl("lblOwnerId");

                Response.Redirect("../Owners/OwnersStatementOfAccountsBudget.aspx?accountid="
                    + lblAccountId.Text + "&debitnoteref="
                    + lblDebitNoteReference.Text + "&ownerid="
                    + lblOwnerid.Text + "&accountcode="
                    + lblAccountCode.Text, true);

            }

            if (e.CommandName.ToUpper().Equals("EXCEL"))
            {
                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
                RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

                DataSet ds = new DataSet();

                ds = PhoenixOwnersStatementOfAccounts.StatementOfAccountsVoucher(lblDebitNoteReference.Text, General.GetNullableInteger(lblOwnerid.Text), General.GetNullableInteger(lblAccountId.Text));

                if (ds.Tables[0].Rows.Count > 0)
                {

                    string[] alColumns = { "FLDOWNERBUDGETCODE", "FLDDESCRIPTION", "FLDVOUCHERDATE", "FLDVOUCHERROW", "FLDLONGDESCRIPTION", "FLDAMOUNT" };
                    string[] alCaptions = { "Owner Budget Code", "Owner Budget Code Description", "Voucher Date", "Voucher Row", "Particulars", "Amount(USD)" };

                    string StrSOAReference = ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString();
                    string prevBcode = null;
                    string prevBdescription = null;
                    string curBcode = null;
                    string curBdescription = null;
                    int columnlength = alColumns.Length;
                    if (StrSOAReference.Length > 0)
                        StrSOAReference = StrSOAReference.Replace(" ", "_");

                    Response.AddHeader("Content-Disposition", "attachment; filename=" + StrSOAReference + ".xls");
                    Response.ContentType = "application/vnd.msexcel";

                    Response.Write("<meta http-equiv=Content-Type content=\"text/html; charset=utf-8\">\n");

                    // add the style props to get the page orientation

                    Response.Write(AddExcelStyling(StrSOAReference));

                    Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                    Response.Write("<tr>");
                    Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
                    Response.Write("<h3><center>" + ds.Tables[0].Rows[0]["FLDACCOUNTCODEDESC"].ToString() + "<br/>" + ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString() + "</center></h3></td>");
                    //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
                    Response.Write("</tr>");
                    Response.Write("</TABLE>");
                    Response.Write("<br />");
                    Response.Write("<TABLE BORDER='1' CELLPADDING='1' CELLSPACING='1' width='100%'>");
                    Response.Write("<tr>");
                    for (int i = 0; i < alCaptions.Length; i++)
                    {

                        if (i == 4)
                            Response.Write("<td colspan='2'>");
                        else
                            Response.Write("<td width='15%'>");
                        Response.Write("<b>" + alCaptions[i].ToString().Trim() + "</b>");
                        Response.Write("</td>");
                    }
                    Response.Write("</tr>");


                    DataTable dt = ds.Tables[0];
                    for (int dr = 0; dr < dt.Rows.Count; dr++)
                    {
                        DataRow previousdatarow;
                        DataRow datarow;
                        Response.Write("<tr>");
                        for (int i = 0; i < alColumns.Length; i++)
                        {
                            if (i == 4)
                                Response.Write("<td colspan='2'>");
                            else
                                Response.Write("<td width='15%'>");
                            decimal s;

                            if (dr > 0)
                            {

                                previousdatarow = dt.Rows[dr - 1];
                                datarow = dt.Rows[dr];
                                prevBcode = datarow[alColumns[0]].ToString();
                                prevBdescription = datarow[alColumns[1]].ToString();
                                curBcode = previousdatarow[alColumns[0]].ToString();
                                curBdescription = previousdatarow[alColumns[1]].ToString();
                                Response.Write("<font color='black'>");
                            }
                            else
                            {
                                datarow = dt.Rows[dr];
                                Response.Write("<font color='black'>");
                            }
                            Response.Write(decimal.TryParse(datarow[alColumns[i]].ToString(), out s) ? String.Format("{0:#,###,###.##}", datarow[alColumns[i]]) : datarow[alColumns[i]]);
                            Response.Write("</font>");
                            Response.Write("</td>");

                        }
                        Response.Write("</tr>");
                    }
                    Response.Write("</TABLE>");
                    Response.Write("</body>");
                    Response.Write("</html>");
                    Response.End();
                }
            }
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
    private string AddExcelStyling(string SheetName)
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("<html xmlns:o='urn:schemas-microsoft-com:office:office'\n" +
        "xmlns:x='urn:schemas-microsoft-com:office:excel'\n" +
        "xmlns='http://www.w3.org/TR/REC-html40'>\n" +
        "<head>\n");

        sb.Append("<style>\n");
        sb.Append("@page");

        sb.Append("{margin:.5in .85in .5in .85in;\n");
        sb.Append("mso-header-margin:.5in;\n");

        sb.Append("mso-footer-margin:.5in;\n");
        sb.Append("mso-page-orientation:landscape;}\n");

        sb.Append("</style>\n");
        sb.Append("<!--[if gte mso 9]><xml>\n");

        sb.Append("<x:ExcelWorkbook>\n");
        sb.Append("<x:ExcelWorksheets>\n");

        sb.Append("<x:ExcelWorksheet>\n");
        sb.Append("<x:Name>" + SheetName + "</x:Name>\n");

        sb.Append("<x:WorksheetOptions>\n");
        sb.Append("<x:Print>\n");

        sb.Append("<x:ValidPrinterInfo/>\n");
        sb.Append("<x:PaperSizeIndex>9</x:PaperSizeIndex>\n");

        sb.Append("<x:HorizontalResolution>600</x:HorizontalResolution\n");
        sb.Append("<x:VerticalResolution>600</x:VerticalResolution\n");

        sb.Append("</x:Print>\n");
        sb.Append("<x:Selected/>\n");

        sb.Append("<x:DoNotDisplayGridlines/>\n");
        sb.Append("<x:ProtectContents>False</x:ProtectContents>\n");

        sb.Append("<x:ProtectObjects>False</x:ProtectObjects>\n");
        sb.Append("<x:ProtectScenarios>False</x:ProtectScenarios>\n");

        sb.Append("</x:WorksheetOptions>\n");
        sb.Append("</x:ExcelWorksheet>\n");

        sb.Append("</x:ExcelWorksheets>\n");
        sb.Append("<x:WindowHeight>12780</x:WindowHeight>\n");

        sb.Append("<x:WindowWidth>19035</x:WindowWidth>\n");
        sb.Append("<x:WindowTopX>0</x:WindowTopX>\n");

        sb.Append("<x:WindowTopY>15</x:WindowTopY>\n");
        sb.Append("<x:ProtectStructure>False</x:ProtectStructure>\n");

        sb.Append("<x:ProtectWindows>False</x:ProtectWindows>\n");
        sb.Append("</x:ExcelWorkbook>\n");

        sb.Append("</xml><![endif]-->\n");
        sb.Append("</head>\n");

        sb.Append("<body>\n");
        return sb.ToString();

    }
}
