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
using Telerik.Web.UI;

public partial class AccountsOwnersFundsPosition : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);



            PhoenixToolbar toolbarowneraccounts = new PhoenixToolbar();
            MenuAccountsowner.AccessRights = this.ViewState;
            toolbarowneraccounts.AddImageButton("../Accounts/AccountsOwnersFundsPosition.aspx", "Find", "search.png", "FIND");
            MenuAccountsowner.AccessRights = this.ViewState;
            MenuAccountsowner.MenuList = toolbarowneraccounts.Show();
            //MenuOrderFormMain.AccessRights = this.ViewState;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["accountid"] = "";
                ViewState["debitnotereference"] = "";
                ViewState["Ownerid"] = "";
                gvOwnersAccount.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                uservesselmap();
                //SetDefaultYear();
                BindLatestYearMonth();
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
            NameValueCollection nvc = Filter.CurrentFundsPosition;
            if (nvc != null)
            {
                ddlVesselAccount.SelectedValue = nvc.Get("ddlVesselAccount").ToString();
                //ucYear.SelectedQuick = nvc.Get("ucYear").ToString();
                ucYear.SelectedQuick = nvc.Get("ucYear").ToString() == "Dummy" ? "0" : nvc.Get("ucYear").ToString();
                //string month = nvc.Get("ucMonth").ToString();                
                ucMonth.SelectedHard = nvc.Get("ucMonth").ToString() == "Dummy" ? "0" : nvc.Get("ucMonth").ToString();
                ddlType.SelectedValue = nvc.Get("ddlType").ToString();
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

            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("ddlVesselAccount", ddlVesselAccount.SelectedValue);
            criteria.Add("ucYear", ucYear.SelectedQuick);
            criteria.Add("ucMonth", ucMonth.SelectedHard);
            criteria.Add("ddlType", ddlType.SelectedValue);

            Filter.CurrentFundsPosition = criteria;

            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvOwnersAccount.Rebind();
        }


    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentFundsPosition;

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

        ds = PhoenixAccountsOwnerStatementOfAccount.StatementOfAccountsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                , sortexpression, sortdirection,
                                                (int)ViewState["PAGENUMBER"],
                                                iRowCount,
                                                ref iRowCount,
                                                ref iTotalPageCount
                                                , General.GetNullableInteger(null)
                                                , ""
                                                , General.GetNullableString(ddlType.SelectedValue)
                                                , ""
                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucMonth")) : month
                                                , nvc != null ? General.GetNullableInteger(nvc.Get("ucYear")) : year);

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
        //if (ViewState["SORTDIRECTION"] != null)
        //    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
        //    iRowCount = 10;
        //else
        //    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
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

        NameValueCollection nvc = Filter.CurrentFundsPosition;

        ds = PhoenixAccountsOwnerStatementOfAccount.StatementOfAccountsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , sortexpression, sortdirection,
                                                 gvOwnersAccount.CurrentPageIndex + 1,
                                                 gvOwnersAccount.PageSize,
                                                 ref iRowCount,
                                                 ref iTotalPageCount,
                                                 General.GetNullableInteger(ddlVesselAccount.SelectedValue),
                                                 null,
                                                 General.GetNullableString(Convert.ToString(ddlType.SelectedValue)),
                                                 null,
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
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }

    protected void gvOwnersAccount_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblDebitNoteReferenceid = (RadLabel)e.Item.FindControl("lblSoaReferenceid");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");

            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkAccountCOde");

            //lbr.Attributes.Add

            DataRowView drv = (DataRowView)e.Item.DataItem;

            DataTable dt;

            if (!string.IsNullOrEmpty(lblOwnerid.Text))
            {
                ImageButton att = (ImageButton)e.Item.FindControl("cmdPdf");
                if (att != null)
                {
                    dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "FDP");
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                        {
                            att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                            att.Visible = true;
                            att.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELSUMMARYBALANCE&vessel=" + null + "&month=" + ucMonth.SelectedHard + "&year=" + ucYear.SelectedQuick + "&debitnotereference=" + General.GetNullableString(lblDebitNoteReference.Text) + "&debitnoteid=" + General.GetNullableString(lblDebitNoteReferenceid.Text) + "&ownerid=" + lblOwnerid.Text + "&subreportcode=FDP&showmenu=0&type=VESSELSUMMARYBALANCE');");
                        }
                    }
                }
            }

        }
    }
}
