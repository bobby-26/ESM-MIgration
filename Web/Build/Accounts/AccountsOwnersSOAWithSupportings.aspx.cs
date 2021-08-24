using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Accounts;
using System.Web.UI;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsOwnersSOAWithSupportings : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Budget Code", "VOUCHER",ToolBarDirection.Right);
            toolbarmain.AddButton("SOA", "SOA", ToolBarDirection.Right);
            

            PhoenixToolbar toolbarowneraccounts = new PhoenixToolbar();
            MenuAccountsowner.AccessRights = this.ViewState;
            toolbarowneraccounts.AddImageButton("../Accounts/AccountsOwnersSOAWithSupportings.aspx", "Find", "search.png", "FIND");
            if (SessionUtil.CanAccess(this.ViewState, "PRINT"))
                toolbarowneraccounts.AddImageLink("javascript:CallPrint('gvOwnersAccount')", "Print Grid", "icon_print.png", "PRINT");
            if (SessionUtil.CanAccess(this.ViewState, "Excel"))
                toolbarowneraccounts.AddImageButton("../Accounts/AccountsOwnersSOAWithSupportings.aspx", "Export to Excel", "icon_xls.png", "Excel");
            MenuAccountsowner.MenuList = toolbarowneraccounts.Show();
            MenuOrderFormMain.AccessRights = this.ViewState;
            MenuOrderFormMain.MenuList = toolbarmain.Show();
            MenuOrderFormMain.SelectedMenuIndex = 1;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["accountid"] = "";
                ViewState["debitnotereference"] = "";
                ViewState["Ownerid"] = "";
                ViewState["debitnoterefID"] = "";

                if (Request.QueryString["debitnoteid"] != null)
                {
                    ViewState["debitnoterefID"] = Request.QueryString["debitnoteid"].ToString();
                    toolbarmain.AddButton("Back", "BACK",ToolBarDirection.Right);
                }
                
                uservesselmap();
                //SetDefaultYear();
                BindLatestYearMonth();

                gvOwnersAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            //BindData();
            //SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void SetDefaultYear()
    //{
    //    int iRowcount = 1, iTotalPageCount = 10;
    //    DataSet dsquick = PhoenixCommonRegisters.QuickSearch(0, 55, DateTime.Now.Year.ToString(), string.Empty, null, 1, 10, ref iTotalPageCount, ref iRowcount);
    //    if (dsquick.Tables[0].Rows.Count > 0)
    //    {
    //        DataRow dr = dsquick.Tables[0].Rows[0];
    //        ViewState["CURRENTYEARCODE"] = dr["FLDQUICKCODE"].ToString();
    //        ucYear.SelectedQuick = dr["FLDQUICKCODE"].ToString();
    //    }

    //    DataSet dshard = PhoenixCommonRegisters.HardSearch(0, "55", DateTime.Now.AddMonths(-1).ToString("MMM"), DateTime.Now.AddMonths(-1).Month.ToString(), string.Empty, null, 1, 10, ref iTotalPageCount, ref iRowcount);
    //    if (dshard.Tables[0].Rows.Count > 0)
    //    {
    //        DataRow dr = dshard.Tables[0].Rows[0];
    //        ViewState["CURRENTMONTHCODE"] = dr["FLDHARDCODE"].ToString();
    //        ucMonth.SelectedHard = dr["FLDHARDCODE"].ToString();
    //    }
    //}

    protected void BindLatestYearMonth()
    {
        DataSet ds = PhoenixAccountsOwnerStatementOfAccount.BindLatestYearMonth();
        if (ds.Tables[0].Rows.Count > 0)
        {
            NameValueCollection nvc = Filter.CurrentSOAWithSupporting;
            if (nvc != null)
            {
                ddlVesselAccount.SelectedValue = nvc.Get("ddlVesselAccount").ToString();
                //ucYear.SelectedQuick = nvc.Get("ucYear").ToString();
                ucYear.SelectedQuick = nvc.Get("ucYear").ToString() == "Dummy" ? "0" : nvc.Get("ucYear").ToString();
                //string month = nvc.Get("ucMonth").ToString();                
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


    protected void uservesselmap()
    {

        ddlVesselAccount.DataSource = PhoenixAccountsOwnerStatementOfAccount.GetvesselAccountid(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlVesselAccount.DataTextField = "FLDDESCRIPTION";
        ddlVesselAccount.DataValueField = "FLDACCOUNTID";
        ddlVesselAccount.DataBind();
        ddlVesselAccount.Items.Insert(0, new RadComboBoxItem("--Office--", ""));

    }
    protected void AccountsownerMenu_TabStripCommand(object sender, EventArgs e)
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
            Filter.CurrentSOAWithSupporting = criteria;

            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvOwnersAccount.Rebind();
        }

        else if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
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
                    ucError.ErrorMessage = "Select an account";
                    ucError.Visible = true;
                    return;
                }
                else
                    Response.Redirect("../Accounts/AccountsOwnersStatementOfAccounts.aspx?accountid=" + ViewState["accountid"].ToString() + "&debitnotereference=" + ViewState["debitnotereference"].ToString() + "&Ownerid=" + ViewState["Ownerid"].ToString(), true);
            }

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Accounts/AccountsSoaGenerationReport.aspx?debitnoteid=" + ViewState["debitnoterefID"].ToString() , true);
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

        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentSOAWithSupporting;

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

        

        ds = PhoenixAccountsOwnerStatementOfAccount.StatementOfAccountsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                 sortexpression,
                                                 sortdirection,
                                                (int)ViewState["PAGENUMBER"],
                                               iRowCount,
                                                ref iRowCount,
                                                ref iTotalPageCount,
                                                General.GetNullableInteger(ddlVesselAccount.SelectedValue),
                                                null,
                                                null,
                                                null,
                                                nvc != null ? General.GetNullableInteger(nvc.Get("ucMonth")) : month,
                                                nvc != null ? General.GetNullableInteger(nvc.Get("ucYear")) : year);       


        Response.AddHeader("Content-Disposition", "attachment; filename=OwnersSOAWithSupportings.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
        Response.Write("<h3><center>Owners SOA With Supportings</center></h3></td>");
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
                gvOwnersAccount.Rebind();
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

        NameValueCollection nvc = Filter.CurrentSOAWithSupporting;

        ds = PhoenixAccountsOwnerStatementOfAccount.StatementOfAccountsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                 sortexpression,
                                                 sortdirection,
                                                (int)ViewState["PAGENUMBER"],
                                                gvOwnersAccount.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount,
                                                General.GetNullableInteger(ddlVesselAccount.SelectedValue),
                                                null,
                                                null,
                                                null,
                                                nvc != null ? General.GetNullableInteger(nvc.Get("ucMonth")) : month,
                                                nvc != null ? General.GetNullableInteger(nvc.Get("ucYear")) : year);


        gvOwnersAccount.DataSource = ds;
        gvOwnersAccount.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        General.SetPrintOptions("gvOwnersAccount", "Owners SOA With Supportings", alCaptions, alColumns, ds);
        ViewState["CURRENTYEARCODE"] = null;
        ViewState["CURRENTMONTHCODE"] = null;
    }


    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
        gvOwnersAccount.Rebind();
    }


    protected void gvOwnersAccount_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
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

    protected void gvOwnersAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT"))
            return;

     
        if (e.CommandName.ToUpper().Equals("SELECT"))
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            ViewState["accountid"] = (RadLabel)e.Item.FindControl("lblAccountId");

            RadLabel lblAccountCode = (RadLabel)e.Item.FindControl("lblAccountCode");

            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            ViewState["debitnotereference"] = (RadLabel)e.Item.FindControl("lblSoaReference");

            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
            ViewState["Ownerid"] = (RadLabel)e.Item.FindControl("lblOwnerId");

            RadLabel lblDebitNoteReferenceID = (RadLabel)e.Item.FindControl("lblSoaReferenceid");
            ViewState["debitnotereferenceID"] = (RadLabel)e.Item.FindControl("lblSoaReferenceid");

            Response.Redirect("../Accounts/AccountsOwnerStatementOfAccountBudget.aspx?accountid="
                + lblAccountId.Text + "&debitnoteref="
                + lblDebitNoteReference.Text + "&accountcode="
                + lblAccountCode.Text + "&debitnoterefID="
                 + lblDebitNoteReferenceID.Text + "&from=SOADebit", true);

        }

        if (e.CommandName.ToUpper().Equals("EXCEL"))
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

            DataSet ds = new DataSet();

            ds = PhoenixAccountsOwnerStatementOfAccount.StatementOfAccountsVoucher(lblDebitNoteReference.Text, General.GetNullableInteger(lblOwnerid.Text), General.GetNullableInteger(lblAccountId.Text));

            if (ds.Tables[0].Rows.Count > 0)
            {

                string[] alColumns = { "FLDOWNERBUDGETCODE", "FLDDESCRIPTION", "FLDVOUCHERDATE", "FLDVOUCHERROW", "FLDLONGDESCRIPTION", "FLDAMOUNT" };
                string[] alCaptions = { "Owner Budget Code", "Owner Budget Code Description", "Voucher Date", "Voucher Row", "Particulars", "Amount(" + ds.Tables[0].Rows[0]["FLDCURRENCYCODE"].ToString() + ")" };
                string prevBcode = null;
                string prevBdescription = null;
                string curBcode = null;
                string curBdescription = null;

                string debitnoteref = null;

                debitnoteref = ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString().Replace(" ", "");

                Response.AddHeader("Content-Disposition", "attachment; filename=" + ds.Tables[0].Rows[0]["FLDOWNERBUDGETCODE"].ToString() + "_" + debitnoteref + ".xls");

                // Response.AddHeader("Content-Disposition", "attachment; filename=" + ds.Tables[0].Rows[0]["FLDOWNERBUDGETCODE"].ToString() + "_" + ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString() + ".xls");
                Response.ContentType = "application/vnd.msexcel";
                Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
                Response.Write("<tr>");
                Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' />");
                Response.Write("<h3><center>" + ds.Tables[0].Rows[0]["FLDACCOUNTCODEDESC"].ToString() + "<br/>" + ds.Tables[0].Rows[0]["FLDDEBITNOTEREFERENCE"].ToString() + "</center></h3></td>");
                //Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
                Response.Write("</tr>");
                Response.Write("</TABLE>");
                Response.Write("<br />");
                Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
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
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
    }

    protected void gvOwnersAccount_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblURL = (RadLabel)e.Item.FindControl("lblURL");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

            RadLabel lblDebitNoteReferenceid = (RadLabel)e.Item.FindControl("lblSoaReferenceid");

            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkAccountCOde");

            //lbr.Attributes.Add


            DataRowView drv = (DataRowView)e.Item.DataItem;

            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
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
                            cmdTBPdf.Visible = true;
                            cmdTBPdf.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELTRAILBALANCE&accountId=" + General.GetNullableInteger(lblAccountId.Text) + "&month=" + ucMonth.SelectedHard + "&year=" + ucYear.SelectedQuick + "&debitnotereference=" + General.GetNullableString(lblDebitNoteReference.Text) + "&debitnoteid=" + General.GetNullableString(lblDebitNoteReferenceid.Text) + "&showmenu=0&ownerid=" + lblOwnerid.Text + "&type=VESSELTRAILBALANCE&subreportcode=VTBA');");
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
                            cmdSummaryPdf.Visible = true;
                            cmdSummaryPdf.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=5&reportcode=STATEMENTOFOWNERACCOUNTSUMMARY&Debitnotereference=" + General.GetNullableString(lblDebitNoteReference.Text) + "&debitnoteid=" + General.GetNullableString(lblDebitNoteReferenceid.Text) + "&showmenu=0&ownerid=" + lblOwnerid.Text + "&type =STATEMENTOFOWNERACCOUNTSUMMARY&subreportcode=SENO');");
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
                            cmdTBYTDOwner.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELTRAILBALANCEYTDOWNER&accountId=" + int.Parse(lblAccountId.Text) + "&debitnoteid=" + new Guid(lblDebitNoteReferenceid.Text) + "&showmenu=0&ownerid=" + lblOwnerid.Text + "&type=VESSELTRAILBALANCE&subreportcode=VTBO');");
                        }
                    }
                }

            }

        }
    }
}
