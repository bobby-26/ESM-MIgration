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
using SouthNests.Phoenix.Export2XL;
using Telerik.Web.UI;

public partial class AccountsOwnerVarianceReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            //PhoenixToolbar toolbarmain = new PhoenixToolbar();
            //toolbarmain.AddButton("SOA", "SOA");            

            PhoenixToolbar toolbarowneraccounts = new PhoenixToolbar();
            MenuAccountsowner.AccessRights = this.ViewState;
            toolbarowneraccounts.AddImageButton("../Accounts/AccountsOwnerVarianceReport.aspx", "Find", "search.png", "FIND");
            //if (SessionUtil.CanAccess(this.ViewState, "PRINT"))
            //    toolbarowneraccounts.AddImageLink("javascript:CallPrint('gvOwnersAccount')", "Print Grid", "icon_print.png", "PRINT");
            //if (SessionUtil.CanAccess(this.ViewState, "Excel"))
            //    toolbarowneraccounts.AddImageButton("../Accounts/AccountsOwnerVarianceReport.aspx", "Export to Excel", "icon_xls.png", "Excel");
            MenuAccountsowner.MenuList = toolbarowneraccounts.Show();
            //MenuOrderFormMain.AccessRights = this.ViewState;
            //MenuOrderFormMain.MenuList = toolbarmain.Show();
            //MenuOrderFormMain.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["accountid"] = "";
                ViewState["debitnotereference"] = "";
                ViewState["Ownerid"] = "";
                uservesselmap();
                //SetDefaultYear();
                BindLatestYearMonth();
                gvOwnersAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            //BindData();
            //gvOwnersAccount.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
    protected void BindLatestYearMonth()
    {
        DataSet ds = PhoenixAccountsOwnerStatementOfAccount.BindLatestYearMonth("Monthly Report");
        if (ds.Tables[0].Rows.Count > 0)
        {
            NameValueCollection nvc = Filter.CurrentVarianceReportAccounts;
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
            Filter.CurrentVarianceReportAccounts = criteria;

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

        NameValueCollection nvc = Filter.CurrentVarianceReportAccounts;

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
                                                gvOwnersAccount.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount,
                                                General.GetNullableInteger(ddlVesselAccount.SelectedValue),
                                                "",
                                                "Monthly Report",
                                                "",
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

        NameValueCollection nvc = Filter.CurrentVarianceReportAccounts;

        ds = PhoenixAccountsOwnerStatementOfAccount.StatementOfAccountsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                 sortexpression,
                                                 sortdirection,
                                                (int)ViewState["PAGENUMBER"],
                                                gvOwnersAccount.PageSize,
                                                ref iRowCount,
                                                ref iTotalPageCount,
                                                General.GetNullableInteger(ddlVesselAccount.SelectedValue),
                                                "",
                                                "Monthly Report",
                                                "",
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

    protected void gvOwnersAccount_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

          

            //RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            //RadLabel lblDebitNoteReferenceID = (RadLabel)e.Item.FindControl("lblSoaReferenceID");
            //RadLabel lblAsonDate = (RadLabel)e.Item.FindControl("lblASonDate");
            //RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

            //DateTime date = DateTime.Parse(lblAsonDate.Text);

            String scriptpopup = "";

            if (e.CommandName.ToUpper().Equals("MONTHLYVARIANCE"))
            {
                RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
                RadLabel lblDebitNoteReferenceID = (RadLabel)e.Item.FindControl("lblSoaReferenceID");
                RadLabel lblAsonDate = (RadLabel)e.Item.FindControl("lblASonDate");
                RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

                DateTime date = DateTime.Parse(lblAsonDate.Text);

                scriptpopup = String.Format(
                    "javascript:parent.openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELVARIANCE&vessel=null&type=Monthly&AsOnDate=" + date.ToShortDateString() + "&debitnotereference=" + lblDebitNoteReference.Text + "&debitnoteid=" + lblDebitNoteReferenceID.Text + "&ownerid=" + lblOwnerid.Text + "&showmenu=0&subreportcode=MVRE');");
            }

            if (e.CommandName.ToUpper().Equals("YEARLYVARIANCE"))
            {
                RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
                RadLabel lblDebitNoteReferenceID = (RadLabel)e.Item.FindControl("lblSoaReferenceID");
                RadLabel lblAsonDate = (RadLabel)e.Item.FindControl("lblASonDate");
                RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

                DateTime date = DateTime.Parse(lblAsonDate.Text);

                scriptpopup = String.Format(
                           "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELVARIANCE&vessel=null&type=Yearly&AsOnDate=" + date.ToShortDateString() + "&debitnotereference=" + lblDebitNoteReference.Text + "&debitnoteid=" + lblDebitNoteReferenceID.Text + "&ownerid=" + lblOwnerid.Text + "&showmenu=0&subreportcode=YVRE');");
            }

            if (e.CommandName.ToUpper().Equals("ACCUMALTEDVARIANCE"))
            {
                RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
                RadLabel lblDebitNoteReferenceID = (RadLabel)e.Item.FindControl("lblSoaReferenceID");
                RadLabel lblAsonDate = (RadLabel)e.Item.FindControl("lblASonDate");
                RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

                DateTime date = DateTime.Parse(lblAsonDate.Text);

                scriptpopup = String.Format(
                           "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELVARIANCE&vessel=null&type=Accumulated&AsOnDate=" + date.ToShortDateString() + "&debitnotereference=" + lblDebitNoteReference.Text + "&debitnoteid=" + lblDebitNoteReferenceID.Text + "&ownerid=" + lblOwnerid.Text + "&showmenu=0&subreportcode=AVRE');");
            }

            if (e.CommandName.ToUpper().Equals("YTDDETAILS"))
            {
                RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
                RadLabel lblDebitNoteReferenceID = (RadLabel)e.Item.FindControl("lblSoaReferenceID");
                RadLabel lblAsonDate = (RadLabel)e.Item.FindControl("lblASonDate");
                RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

                DateTime date = DateTime.Parse(lblAsonDate.Text);

                DataTable dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccessByReportcode(int.Parse(lblOwnerid.Text), "YTD");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (dr["FLDACCESSYN"].ToString() == "1" && dr["FLDDBNAME"].ToString() == "PhoenixPCL")
                        {
                            PhoenixAccountsSOAVariance.Export2XLPCLVarianceYTDDetails(date, lblDebitNoteReference.Text, dr["FLDSUBREPORTCODE"].ToString());
                        }
                        else  if (dr["FLDACCESSYN"].ToString() == "1")
                        {
                            PhoenixAccountsSOAVariance.Export2XLVarianceYTDDetails(date, lblDebitNoteReference.Text, dr["FLDSUBREPORTCODE"].ToString());
                        }
                    }
                }
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

            if (scriptpopup != "")
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvOwnersAccount_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkAccountCOde");

            //lbr.Attributes.Add


            DataRowView drv = (DataRowView)e.Item.DataItem;

            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                    att.ImageUrl = Session["images"] + "/no-attachment.png";
                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('att','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.ACCOUNTS + "&U=NO'); return false;");
            }

            DataTable dt;
            if (!(String.IsNullOrEmpty(lblOwnerid.Text)))
            {
                LinkButton cmdMVR = (LinkButton)e.Item.FindControl("cmdMonthlyVariance");
                if (cmdMVR != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "MVRE");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            cmdMVR.Visible = true;
                        }
                    }
                }

                LinkButton cmdYVR = (LinkButton)e.Item.FindControl("cmdYearlyVariance");
                if (cmdYVR != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "YVRE");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            cmdYVR.Visible = true;
                        }
                    }
                }

                LinkButton cmdAVR = (LinkButton)e.Item.FindControl("cmdAccumaltedVariance");
                if (cmdAVR != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "AVRE");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            cmdAVR.Visible = true;
                        }
                    }
                }

                LinkButton cmdYTD = (LinkButton)e.Item.FindControl("cmdYTDDetails");
                if (cmdYTD != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccessByReportcode(int.Parse(lblOwnerid.Text), "YTD");
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (dr["FLDACCESSYN"].ToString() == "1" && SessionUtil.CanAccess(this.ViewState, cmdYTD.CommandName))
                                cmdYTD.Visible = true;
                        }
                    }
                }
            }
        }
    }
}
