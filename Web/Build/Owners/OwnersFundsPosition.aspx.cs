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
using SouthNests.Phoenix.Owners;
using Telerik.Web.UI;

public partial class OwnersFundsPosition : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarowneraccounts = new PhoenixToolbar();
            MenuAccountsowner.AccessRights = this.ViewState;
            toolbarowneraccounts.AddFontAwesomeButton("../Owners/OwnersFundsPosition.aspx", "Find", "<i class=\"fa fa-search\"></i>", "FIND");
            toolbarowneraccounts.AddFontAwesomeButton("../Owners/OwnersFundsPosition.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuAccountsowner.AccessRights = this.ViewState;
            MenuAccountsowner.MenuList = toolbarowneraccounts.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["accountid"] = "";
                ViewState["debitnotereference"] = "";
                ViewState["Ownerid"] = "";
               // uservesselmap();
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
            NameValueCollection nvc = Filter.CurrentOwnerFundsPosition;
            if (nvc != null)
            {
                ucVesselAccount.SelectedAccount = nvc.Get("ucVesselAccount").ToString();
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
    protected void Rebind()
    {
        gvOwnersAccount.SelectedIndexes.Clear();
        gvOwnersAccount.EditIndexes.Clear();
        gvOwnersAccount.DataSource = null;
        gvOwnersAccount.Rebind();
    }
    //protected void uservesselmap()
    //{

    //    ddlVesselAccount.DataSource = PhoenixOwnersStatementOfAccounts.GetuservesselAccount(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
    //    ddlVesselAccount.DataTextField = "FLDDESCRIPTION";
    //    ddlVesselAccount.DataValueField = "FLDACCOUNTID";
    //    ddlVesselAccount.DataBind();
    //    ddlVesselAccount.Items.Insert(0, new DropDownListItem("--Select--", ""));
    //}
    protected void AccountsownerMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {

            NameValueCollection criteria = new NameValueCollection();
            criteria.Clear();
            criteria.Add("ucVesselAccount", ucVesselAccount.SelectedText);
            criteria.Add("ucYear", ucYear.SelectedQuick);
            criteria.Add("ucMonth", ucMonth.SelectedHard);
            criteria.Add("ddlType", ddlType.SelectedValue);
            Filter.CurrentOwnerFundsPosition = criteria;

            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentOwnerFundsPosition = null;
            ucVesselAccount.SelectedAccount = "";
            ucYear.SelectedQuick = "";
            ucMonth.SelectedHard = "";
            ddlType.SelectedValue = "";
            BindLatestYearMonth();
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        NameValueCollection nvc = Filter.CurrentOwnerFundsPosition;

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

        NameValueCollection nvc = Filter.CurrentOwnerFundsPosition;

        ds = PhoenixAccountsOwnerStatementOfAccount.OwnerStatementOfAccountsSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , sortexpression, sortdirection,
                                                 (int)ViewState["PAGENUMBER"],
                                                 General.ShowRecords(null),
                                                 ref iRowCount,
                                                 ref iTotalPageCount,
                                                 General.GetNullableInteger(ucVesselAccount.SelectedAccount),
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
        Rebind();
    }  
    protected void gvOwnersAccount_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblAccountId = (RadLabel)e.Item.FindControl("lblAccountId");
            RadLabel lblDebitNoteReference = (RadLabel)e.Item.FindControl("lblSoaReference");
            RadLabel lblDebitNoteReferenceid = (RadLabel)e.Item.FindControl("lblSoaReferenceid");
            RadLabel lblOwnerid = (RadLabel)e.Item.FindControl("lblOwnerId");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkAccountCOde");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            DataTable dt;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdPdf");
            if (att != null)
            {
                dt = PhoenixAccountsOwnerReportDisplay.OwnerDisplayReporPortalAccess(int.Parse(lblOwnerid.Text), "FDP");
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["FLDACCESSYN"].ToString() == "1")
                    {
                        att.Visible = true;
                        if (SessionUtil.CanAccess(this.ViewState, att.CommandName))
                            att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                        att.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=5&reportcode=VESSELSUMMARYBALANCE&vessel=" + null + "&month=" + ucMonth.SelectedHard + "&year=" + ucYear.SelectedQuick + "&debitnotereference=" + General.GetNullableString(lblDebitNoteReference.Text) + "&debitnoteid=" + General.GetNullableString(lblDebitNoteReferenceid.Text) + "&ownerid=" + lblOwnerid.Text + "&subreportcode=FDP&showmenu=0&type=VESSELSUMMARYBALANCE');");
                    }
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
