using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsPeriodLock : PhoenixBasePage
{
    public int iUserCode;
    public string strCompanynamedisplay = string.Empty;

    // protected override void Render(HtmlTextWriter writer)
    // {
    //     foreach (GridViewRow r in dgFinancialYearSetup.Rows)
    //     {
    //         if (r.RowType == DataControlRowType.DataRow)
    //         {
    //             Page.ClientScript.RegisterForEventValidation(dgFinancialYearSetup.UniqueID, "Edit$" + r.RowIndex.ToString());
    //         }
    //     }
    //     base.Render(writer);
    // }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        toolbar1.AddButton("Financial Years", "FINANCIALYEAR");
        toolbar1.AddButton("Period Lock", "PERIODLOCK");
        toolbar1.AddButton("Year End Exchange Rates", "EXCHANGERATES");

        MenuFinYearSetup.AccessRights = this.ViewState;
        MenuFinYearSetup.MenuList = toolbar1.Show();
        MenuFinYearSetup.SelectedMenuIndex = 1;

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddImageButton("../Accounts/AccountsPeriodLock.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbar.AddImageLink("javascript:CallPrint('gvLock')", "Print Grid", "icon_print.png", "PRINT");


        MenuFinancialYearSetup.AccessRights = this.ViewState;
        MenuFinancialYearSetup.MenuList = toolbar.Show();
        // MenuFinancialYearSetup.SetTrigger(pnlFinancialYearSetup);
        iUserCode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            SetDefaultYear();
            dgFinancialYearSetup.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            BindData();
        }
        BindData();
    }

    protected void SetDefaultYear()
    {
        int iRowcount = 1, iTotalPageCount = 10;
        DataSet dsquick = PhoenixCommonRegisters.QuickSearch(0, 55, DateTime.Now.Year.ToString(), string.Empty, null, 1, 10, ref iTotalPageCount, ref iRowcount);
        if (dsquick.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsquick.Tables[0].Rows[0];
            ViewState["CURRENTYEARCODE"] = dr["FLDQUICKCODE"].ToString();
            ddlYearlist.SelectedQuick = dr["FLDQUICKCODE"].ToString();
        }

        DataSet dshard = PhoenixCommonRegisters.HardSearch(0, "55", DateTime.Now.ToString("MMM"), DateTime.Now.Month.ToString(), string.Empty, null, 1, 10, ref iTotalPageCount, ref iRowcount);
        if (dshard.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dshard.Tables[0].Rows[0];
            ViewState["CURRENTMONTHCODE"] = dr["FLDHARDCODE"].ToString();
        }
    }

    protected void ShowExcel()
    {
        string[] alColumns = { "FLDHARDNAME", "FLDSTATUSNAME" };
        string[] alCaptions = { "Period Name", "Status" };
        DataSet dshard = new DataSet();
        if (ddlYearlist.SelectedQuick.ToUpper() != "" && ddlYearlist.SelectedQuick.ToUpper() != "DUMMY")
        {
            dshard = PhoenixAccountsPeriodLock.PeriodLockSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, int.Parse(ddlYearlist.SelectedQuick));
            dgFinancialYearSetup.DataSource = dshard;
            dgFinancialYearSetup.DataBind();
        }
        else
        {
            dshard = PhoenixAccountsPeriodLock.PeriodLockSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, int.Parse(ViewState["CURRENTYEARCODE"].ToString()));
            dgFinancialYearSetup.DataSource = dshard;
            dgFinancialYearSetup.DataBind();
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=PeriodLock.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Period Lock</h3></td>");
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
        foreach (DataRow dr in dshard.Tables[0].Rows)
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

    protected void FinancialYearSetup_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.Equals("FINANCIALYEAR"))
        {
            Response.Redirect("../Accounts/AccountsFinancialYearSetup.aspx");
        }
        if (CommandName.ToUpper().Equals("EXCHANGERATES"))
        {
            Response.Redirect("../Registers/RegistersYearEndExchangeRate.aspx");
        }
    }
    private void BindData()
    {
        DataSet dshard = new DataSet();

        if (ddlYearlist.SelectedQuick.ToUpper() != "" && ddlYearlist.SelectedQuick.ToUpper() != "DUMMY")
        {
            dshard = PhoenixAccountsPeriodLock.PeriodLockSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, int.Parse(ddlYearlist.SelectedQuick));
            dgFinancialYearSetup.DataSource = dshard;
            dgFinancialYearSetup.DataBind();
        }
        else
        {
            dshard = PhoenixAccountsPeriodLock.PeriodLockSearch(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, int.Parse(ViewState["CURRENTYEARCODE"].ToString()));
            dgFinancialYearSetup.DataSource = dshard;
            dgFinancialYearSetup.DataBind();
        }

        string[] alColumns = { "FLDHARDNAME", "FLDSTATUSNAME" };
        string[] alCaptions = { "Period Name", "Status" };
        General.SetPrintOptions("gvLock", "Period Lock", alCaptions, alColumns, dshard);
    }

    protected void dgFinancialYearSetup_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //GridView _gridView = (GridView)sender;
        //int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
        if (e.CommandName.ToUpper().Equals("LOCK"))
        {
            FinancialPeriodLock(int.Parse(ddlYearlist.SelectedQuick), int.Parse(((RadLabel)e.Item.FindControl("lblPeriodCode")).Text));

            BindData();
        }
        else if (e.CommandName.ToUpper().Equals("UNLOCK"))
        {
            FinancialPeriodOpen(new Guid((((RadLabel)e.Item.FindControl("lblMonthEndProcessCode")).Text)));

            BindData();

        }
        else
        {

            BindData();
        }
    }

    protected void dgFinancialYearSetup_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
        {
            e.Row.TabIndex = -1;
            e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(dgFinancialYearSetup, "Edit$" + e.Row.RowIndex.ToString(), false);
        }
    }

    protected void dgFinancialYearSetup_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            if (e.Item.DataItem != null)
            {
                ImageButton img1 = (ImageButton)e.Item.FindControl("lock");
                ImageButton img2 = (ImageButton)e.Item.FindControl("unlock");

                RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
                RadLabel lblLockedYN = (RadLabel)e.Item.FindControl("lblLockedYN");

                img1.Visible = true;
                if (lblStatus.Text != "1")
                {
                    img2.Visible = false;
                }
                else
                {
                    img1.Visible = false;
                    img2.Visible = true;
                }
            }
        }


        if (e.Item is GridEditableItem)
        {
            ImageButton cmdlock = (ImageButton)e.Item.FindControl("lock");
            if (cmdlock != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdlock.CommandName)) cmdlock.Visible = false;

            ImageButton cmdunlock = (ImageButton)e.Item.FindControl("unlock");
            if (cmdunlock != null)
                if (!SessionUtil.CanAccess(this.ViewState, cmdunlock.CommandName)) cmdunlock.Visible = false;
        }

        if (e.Item is GridEditableItem)
        {
            // Get the LinkButton control in the first cell
            //LinkButton _doubleClickButton = (LinkButton)e.Item.Cells[0].Controls[0];
            //// Get the javascript which is assigned to this LinkButton
            //string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
            //// Add this javascript to the onclick Attribute of the row
            //e.Item.Attributes["ondblclick"] = _jsDouble;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void FinancialPeriodLock(int iFinancialYearCode, int iPeriodCode)
    {
        try
        {
            PhoenixAccountsPeriodLock.PeriodLock(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, iFinancialYearCode, iPeriodCode, 1, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            ucStatus.Text = "Period Locked Successfully.";
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void FinancialPeriodOpen(Guid gMonthEndProcessCode)
    {
        try
        {
            PhoenixAccountsPeriodLock.PeriodOpen(gMonthEndProcessCode, 0, PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            ucStatus.Text = "Period Opened Successfully.";
        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
