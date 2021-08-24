using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using Telerik.Web.UI;

public partial class AccountsVesselVisitTravelClaimHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddImageButton("../Accounts/AccountsVesselVisitTravelClaimHistory.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbargrid.AddImageLink("javascript:CallPrint('gvTravelClaimHistory')", "Print Grid", "icon_print.png", "PRINT");

            MenuTracelClaim.AccessRights = this.ViewState;
            MenuTracelClaim.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {
                if (Request.QueryString["VisitId"] != "")
                    ViewState["VisitId"] = Request.QueryString["visitId"];
                else
                    ViewState["VisitId"] = null;
                if (Request.QueryString["TravelClaimId"] != "")
                    ViewState["TravelClaimId"] = Request.QueryString["TravelClaimId"];
                else
                    ViewState["TravelClaimId"] = null;

                ViewState["PAGENUMBER"] = 1;
            }
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("History", "HISTORY",ToolBarDirection.Right);
            toolbarmain.AddButton("Voucher Posting", "POSTING",ToolBarDirection.Right);
            toolbarmain.AddButton("Travel Claim", "CLAIM",ToolBarDirection.Right);

            MenuTracelClaimMain.AccessRights = this.ViewState;
            MenuTracelClaimMain.MenuList = toolbarmain.Show();
            MenuTracelClaimMain.SelectedMenuIndex = 0;
            gvTravelClaimHistory.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTracelClaimMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("CLAIM"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitTravelClaimPostMaster.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&TravelClaimId=" + ViewState["TravelClaimId"].ToString());
            }
            else if (CommandName.ToUpper().Equals("POSTING"))
            {
                Response.Redirect("../Accounts/AccountsVesselVisitTravelClaimPosting.aspx?VisitId=" + ViewState["VisitId"].ToString() + "&TravelClaimId=" + ViewState["TravelClaimId"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuTracelClaim_TabStripCommand(object sender, EventArgs e)
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
        try
        {
            DataSet ds = new DataSet();

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

            ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimHistoryList(new Guid(ViewState["TravelClaimId"].ToString())
                                                         , rblHistoryType.SelectedValue);

            General.SetPrintOptions("gvTravelClaimHistory", "Travel Claim History", alCaptions, alColumns, ds);

            gvTravelClaimHistory.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ReBindData(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvTravelClaimHistory_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
    }

    protected void ShowExcel()
    {
        DataSet ds = new DataSet();

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
        ds = PhoenixAccountsVesselVIistTravelClaimRegister.TravelClaimHistoryList(new Guid(ViewState["TravelClaimId"].ToString())
                                                     , rblHistoryType.SelectedValue);

        Response.AddHeader("Content-Disposition", "attachment; filename=TravelClaimHistory.xls");

        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Travel Claim History" + "-" + ViewState["Invoicenumber"] + "</h3></td>");
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

    protected void gvTravelClaimHistory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravelClaimHistory.CurrentPageIndex + 1;
        BindData();
    }
}
