using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Owners;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Accounts;
using Telerik.Web.UI;

public partial class OwnersSOAWithSupportings : PhoenixBasePage
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
            MenuOwnersSOAWithSupportings.AccessRights = this.ViewState;
            if (SessionUtil.CanAccess(this.ViewState, "Excel"))
                toolbarowneraccounts.AddFontAwesomeButton("../Owners/OwnersSOAWithSupportings.aspx", "Export to Excel", "<i class=\"fa fa-file-excel\"></i>", "Excel");
            if (SessionUtil.CanAccess(this.ViewState, "PRINT"))
                toolbarowneraccounts.AddFontAwesomeButton("javascript:CallPrint('gvOwnersAccount')", "Print Grid", "<i class=\"fa fa-print\"></i>", "PRINT");
            toolbarowneraccounts.AddFontAwesomeButton("../Owners/OwnersSOAWithSupportings.aspx", "Find", "<i class=\"fa fa-search\"></i>", "FIND");
            toolbarowneraccounts.AddFontAwesomeButton("../Owners/OwnersSOAWithSupportings.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuOwnersSOAWithSupportings.AccessRights = this.ViewState;
            MenuOwnersSOAWithSupportings.MenuList = toolbarowneraccounts.Show();
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
               // Getvesselid();
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
    protected void Rebind()
    {
        gvOwnersAccount.SelectedIndexes.Clear();
        gvOwnersAccount.EditIndexes.Clear();
        gvOwnersAccount.DataSource = null;
        gvOwnersAccount.Rebind();
    }
    protected void BindLatestYearMonth()
    {
        DataSet ds = PhoenixOwnersStatementOfAccounts.BindLatestYearMonth(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        if (ds.Tables[0].Rows.Count > 0)
        {
            NameValueCollection nvc = Filter.CurrentOwnerSOAWithSupporting;
            if (nvc != null)
            {
                ucVesselAccount.SelectedAccount = nvc.Get("ucVesselAccount").ToString();
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

    protected void OwnersSOAWithSupportings_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("FIND"))
            {
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucVesselAccount", ucVesselAccount.SelectedAccount);
                criteria.Add("ucYear", ucYear.SelectedQuick);
                criteria.Add("ucMonth", ucMonth.SelectedHard);
                Filter.CurrentOwnerSOAWithSupporting = criteria;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucVesselAccount.SelectedAccount = "";
                ucMonth.SelectedHard = "";
                ucYear.SelectedQuick = "";
                Filter.CurrentOwnerSOAWithSupporting = null;
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
    //protected void Getvesselid()
    //{
    //    ddlVesselAccount.DataSource = PhoenixOwnersStatementOfAccounts.GetuservesselAccount(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    //    ddlVesselAccount.DataTextField = "FLDDESCRIPTION";
    //    ddlVesselAccount.DataValueField = "FLDACCOUNTID";
    //    ddlVesselAccount.DataBind();
    //    ddlVesselAccount.Items.Insert(0, new DropDownListItem("--Select--", ""));
    //}
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentOwnerSOAWithSupporting;

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
                                                 iRowCount,
                                                 ref iRowCount,
                                                 ref iTotalPageCount,
                                                 General.GetNullableInteger(ucVesselAccount.SelectedAccount),
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
        Response.Write("<h3><center>SOA With Supporting</center></h3></td>");
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

        string[] alColumns = { "FLDDESCRIPTION", "FLDDEBITNOTEREFERENCE", "FLDHARDNAME", "FLDQUICKNAME" };
        string[] alCaptions = { "Account Name", "SOA Reference", "Month", "Year" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
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
        DataSet ds = new DataSet();
        NameValueCollection nvc = Filter.CurrentOwnerSOAWithSupporting;
        ds = PhoenixOwnersStatementOfAccounts.StatementOfAccountsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , sortexpression, sortdirection, (int)ViewState["PAGENUMBER"],
                                                 gvOwnersAccount.PageSize, ref iRowCount, ref iTotalPageCount,
                                                 General.GetNullableInteger(ucVesselAccount.SelectedAccount), General.GetNullableString(null)
                                                 , General.GetNullableString(null), General.GetNullableString(null),
                                                 nvc != null ? General.GetNullableInteger(nvc.Get("ucMonth")) : month,
                                                 nvc != null ? General.GetNullableInteger(nvc.Get("ucYear")) : year);
        gvOwnersAccount.DataSource = ds;
        gvOwnersAccount.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOwnersAccount", "Statement Of SOA With Supporting", alCaptions, alColumns, ds);
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
            RadLabel lblDebitNoteReferenceid = (RadLabel)e.Item.FindControl("lblSoaReferenceId");
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
            DataTable dt;
            if (!string.IsNullOrEmpty(lblOwnerid.Text))
            {
                LinkButton cmdTBPdf = (LinkButton)e.Item.FindControl("cmdTBPdf");
                if (cmdTBPdf != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "VTBA");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            cmdTBPdf.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                            cmdTBPdf.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELTRAILBALANCE&accountId=" + General.GetNullableInteger(lblAccountId.Text) + "&month=" + ucMonth.SelectedHard + "&year=" + ucYear.SelectedQuick + "&debitnotereference=" + General.GetNullableString(lblDebitNoteReference.Text) + "&debitnoteid=" + General.GetNullableString(lblDebitNoteReferenceid.Text) + "&showmenu=0&ownerid=" + lblOwnerid.Text + "&type=VESSELTRAILBALANCE&subreportcode=VTBA');");
                        }
                    }
                }
                LinkButton cmdTBYTD = (LinkButton)e.Item.FindControl("cmdTBYTD");
                if (cmdTBYTD != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "VTBY");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            cmdTBYTD.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                            cmdTBYTD.Visible = true;
                            cmdTBYTD.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELTRAILBALANCEYTD&accountId=" + General.GetNullableInteger(lblAccountId.Text) + "&month=" + ucMonth.SelectedHard + "&year=" + ucYear.SelectedQuick + "&debitnotereference=" + General.GetNullableString(lblDebitNoteReference.Text) + "&debitnoteid=" + General.GetNullableString(lblDebitNoteReferenceid.Text) + "&showmenu=0&ownerid=" + lblOwnerid.Text + "&type=VESSELTRAILBALANCE&subreportcode=VTBY');");
                        }
                    }
                }

                LinkButton cmdSummaryPdf = (LinkButton)e.Item.FindControl("cmdSummaryPdf");
                if (cmdSummaryPdf != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "SENO");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            cmdSummaryPdf.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                            cmdSummaryPdf.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=5&reportcode=STATEMENTOFOWNERACCOUNTSUMMARY&Debitnotereference=" + General.GetNullableString(lblDebitNoteReference.Text) + "&debitnoteid=" + General.GetNullableString(lblDebitNoteReferenceid.Text) + "&showmenu=0&type=STATEMENTOFOWNERACCOUNTSUMMARY&ownerid=" + lblOwnerid.Text + "&subreportcode=SENO');");
                        }
                    }
                }

                LinkButton cmdTBYTDOwner = (LinkButton)e.Item.FindControl("cmdTBYTDOwner");
                if (cmdTBYTDOwner != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "VTBO");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            cmdTBYTDOwner.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                            cmdTBYTDOwner.Visible = true;
                            cmdTBYTDOwner.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELTRAILBALANCEYTDOWNER&accountId=" + int.Parse(lblAccountId.Text) + "&debitnoteid=" + new Guid(lblDebitNoteReferenceid.Text) + "&showmenu=0&ownerid=" + lblOwnerid.Text + "&type=VESSELTRAILBALANCE&subreportcode=VTBO');");
                        }
                    }
                }
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

                Response.Redirect("../Owners/OwnersSOAWithSupportingsBudget.aspx?accountid="
                    + lblAccountId.Text + "&debitnoteref="
                    + lblDebitNoteReference.Text + "&ownerid="
                    + lblOwnerid.Text + "&accountcode="
                    + lblAccountCode.Text, true);
            }
            if (e.CommandName.ToUpper().Equals("SELECTVARIANCE"))
            {
                RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
                RadLabel lblAccountCode = (RadLabel)e.Item.FindControl("lblAccountCode");
                RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
                RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
                RadLabel lblDebitNoteReferenceId = (RadLabel)e.Item.FindControl("lblSoaReferenceId");
                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("lblAccountId", lblAccountId.Text);
                criteria.Add("lblDebitNoteReferenceId", lblDebitNoteReferenceId.Text);
                criteria.Add("lblAccountCode", lblAccountCode.Text);
                criteria.Add("lblDebitNoteReference", lblDebitNoteReference.Text);
                criteria.Add("lblOwnerId", lblOwnerid.Text);

                Filter.CurrentVarianceReport = criteria;

                Response.Redirect("../Owners/OwnersVarianceReportMonthly.aspx?", true);

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
                    try
                    {
                        string[] alColumns = { "FLDOWNERBUDGETCODE", "FLDDESCRIPTION", "FLDVOUCHERDATE", "FLDVOUCHERROW", "FLDLONGDESCRIPTION", "FLDAMOUNT" };
                        string[] alCaptions = { "Owner Budget Code", "Owner Budget Code Description", "Voucher Date", "Voucher Row", "Particulars", "Amount(USD)" };
                        string prevBcode = null;
                        string prevBdescription = null;
                        string curBcode = null;
                        string curBdescription = null;
                        string debitnoteref = null;

                        debitnoteref = ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString().Replace(" ", "");

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + ds.Tables[0].Rows[0]["FLDOWNERBUDGETCODE"].ToString() + "_" + debitnoteref + ".xls");
                        Response.ContentType = "application/vnd.msexcel";
                        Response.Write("<table BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                        Response.Write("<tr>");
                        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
                        Response.Write("<h3><center>" + ds.Tables[0].Rows[0]["FLDACCOUNTCODEDESC"].ToString() + "<br/>" + ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString() + "</center></h3></td>");
                        Response.Write("</tr>");
                        Response.Write("</table>");
                        Response.Write("<br />");
                        Response.Write("<table BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
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
                        Response.Write("</table>");
                        Response.Write("</body>");
                        Response.Write("</html>");
                        Response.Flush();

                        Response.End();
                    }
                    catch (Exception ex) { ucError.ErrorMessage = ex.Message; }
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

}
