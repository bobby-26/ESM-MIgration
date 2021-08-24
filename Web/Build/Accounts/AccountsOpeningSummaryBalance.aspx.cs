using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class AccountsOpeningSummaryBalance : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsOpeningSummaryBalance.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvOpeningSummary')", "Print Grid", "icon_print.png", "PRINT");
            MenuOpeningSummaryGrid.AccessRights = this.ViewState;
            MenuOpeningSummaryGrid.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;

                gvOpeningSummary.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
       
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuOpeningSummary_TabStripCommand(object sender, EventArgs e)
    {
    }

    protected void MenuOpeningSummaryGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
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

      
       
    protected void gvOpeningSummary_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
       // gvOpeningSummary.SelectedIndex = e.NewSelectedIndex;
        BindPageURL(e.NewSelectedIndex);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION","FLDTYPE", "FLDHARDNAME", "FLDQUICKNAME", "FLDOPENINGAMOUNT" };
        string[] alCaptions = { "Account Code", "Account Description","Type", "Month", "Year", "Opening Amount"};
    
        DataSet ds = new DataSet();

        ds = PhoenixAccountsOpeningVesselSummary.OpeningVesselSummarylist((int)ViewState["PAGENUMBER"],
                                                          gvOpeningSummary.PageSize,
                                                          ref iRowCount,
                                                          ref iTotalPageCount);

        General.SetPrintOptions("gvOpeningSummary", "Opening Vessel Summary", alCaptions, alColumns, ds);

        gvOpeningSummary.DataSource = ds;
        gvOpeningSummary.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }


    private bool IsValidData(string accountcode, string month, string year, string amount, string type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(accountcode) == null)
            ucError.ErrorMessage = "Account code is required.";
        if (General.GetNullableInteger(month) == null)
            ucError.ErrorMessage = "Month is required.";
        if (General.GetNullableInteger(year) == null)
            ucError.ErrorMessage = "Year is required.";
        if (General.GetNullableString(amount) == null)
            ucError.ErrorMessage = "Opening amount is required.";
        if(General.GetNullableString(type) == null)
            ucError.ErrorMessage = "Type is required.";

        return (!ucError.IsError);
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lbl = ((RadLabel)gvOpeningSummary.Items[rowindex].FindControl("lblOpeningSummaryId"));
            if (lbl != null)
                ViewState["OpeningSummaryId"] = lbl.Text;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        //gvOpeningSummary.SelectedIndex = -1;
       
        foreach (GridDataItem item in gvOpeningSummary.MasterTableView.Items)
        {
            if (gvOpeningSummary.MasterTableView.Items[0].GetDataKeyValue("FLDOPENINGSUMMARYID").ToString().Equals(ViewState["OpeningSummaryId"].ToString()))
            {

                ViewState["DTKEY"] = ((RadLabel)item.FindControl("lblOpeningSummaryId")).Text;
                break;
            }
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDACCOUNTCODE", "FLDDESCRIPTION", "FLDTYPE","FLDHARDNAME", "FLDQUICKNAME", "FLDOPENINGAMOUNT" };
        string[] alCaptions = { "Account Code", "Account Description","Type", "Month", "Year", "Opening Amount" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixAccountsOpeningVesselSummary.OpeningVesselSummarylist((int)ViewState["PAGENUMBER"],
                                                          PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                          ref iRowCount,
                                                          ref iTotalPageCount);



        Response.AddHeader("Content-Disposition", "attachment; filename=OpeningVesselSummary.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Opening Vessel Summary</h3></td>");
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

    protected void gvOpeningSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvOpeningSummary.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOpeningSummary_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           

          
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidData(((RadTextBox)e.Item.FindControl("txtAccountId")).Text
                                    , ((UserControlHard)e.Item.FindControl("ucMonth")).SelectedHard
                                    , ((UserControlQuick)e.Item.FindControl("ucYear")).SelectedQuick
                                    , ((UserControlDecimal)e.Item.FindControl("txtOpeningAmountAdd")).Text
                                    , ((RadComboBox)e.Item.FindControl("ddlTypeAdd")).SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsOpeningVesselSummary.OpeningVesselSummaryInsert(int.Parse(((RadTextBox)e.Item.FindControl("txtAccountId")).Text)
                                                                            , int.Parse(((UserControlHard)e.Item.FindControl("ucMonth")).SelectedHard)
                                                                            , int.Parse(((UserControlQuick)e.Item.FindControl("ucYear")).SelectedQuick)
                                                                            , Convert.ToDecimal(((UserControlDecimal)e.Item.FindControl("txtOpeningAmountAdd")).Text)
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , ((RadComboBox)e.Item.FindControl("ddlTypeAdd")).SelectedValue
                                                                            );

                ucStatus.Text = "Opening vessel summary inserted";

                BindData();
                gvOpeningSummary.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidData(((RadTextBox)e.Item.FindControl("txtAccountIdEdit")).Text
                    , ((UserControlHard)e.Item.FindControl("ucMonthEdit")).SelectedHard
                    , ((UserControlQuick)e.Item.FindControl("ucYearEdit")).SelectedQuick
                    , ((UserControlDecimal)e.Item.FindControl("txtOpeningAmountEdit")).Text
                    , ((RadComboBox)e.Item.FindControl("ddlTypeEdit")).SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixAccountsOpeningVesselSummary.OpeningVesselSummaryUpdate(new Guid(((RadLabel)e.Item.FindControl("lblOpeningSummaryIdEdit")).Text)
                                                                            , int.Parse(((RadTextBox)e.Item.FindControl("txtAccountIdEdit")).Text)
                                                                            , int.Parse(((UserControlHard)e.Item.FindControl("ucMonthEdit")).SelectedHard)
                                                                            , int.Parse(((UserControlQuick)e.Item.FindControl("ucYearEdit")).SelectedQuick)
                                                                            , Convert.ToDecimal(((UserControlDecimal)e.Item.FindControl("txtOpeningAmountEdit")).Text)
                                                                            , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                            , ((RadComboBox)e.Item.FindControl("ddlTypeEdit")).SelectedValue);

                ucStatus.Text = "Opening vessel summary updated";
              
                BindData();
                gvOpeningSummary.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixAccountsOpeningVesselSummary.OpeningVesselSummaryDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                , new Guid(((RadLabel)e.Item.FindControl("lblOpeningSummaryId")).Text));

                ucStatus.Text = "Opening vessel summary deleted";
                BindData();
                gvOpeningSummary.Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                int iRowno;
                iRowno = int.Parse(e.CommandArgument.ToString());
                BindPageURL(iRowno);
                SetRowSelection();
            }
            else if (e.CommandName == "Page")
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

    protected void gvOpeningSummary_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {
            ImageButton eb = (ImageButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            ImageButton sb = (ImageButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            ImageButton cb = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                ImageButton db = (ImageButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
            }

            ImageButton imgShowAccountEdit = (ImageButton)e.Item.FindControl("imgShowAccountEdit");
            if (imgShowAccountEdit != null)
                imgShowAccountEdit.Attributes.Add("onclick", "return showPickList('spnPickListAccountEdit', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselAccount.aspx', true); ");

            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadComboBox ddlTypeEdit = (RadComboBox)e.Item.FindControl("ddlTypeEdit");

            if (drv != null && ddlTypeEdit != null)
            {
                ddlTypeEdit.SelectedValue = drv["FLDTYPE"].ToString();
            }

            UserControlHard ucMonthEdit = (UserControlHard)e.Item.FindControl("ucMonthEdit");
            if (ucMonthEdit != null) ucMonthEdit.HardList = PhoenixRegistersHard.ListHard(1, 55, byte.Parse("1"), "");

            UserControlQuick ucYearEdit = (UserControlQuick)e.Item.FindControl("ucYearEdit");

            if (drv != null && ucMonthEdit != null)
                ucMonthEdit.SelectedHard = drv["FLDMONTH"].ToString();

            if (drv != null && ucYearEdit != null)
                ucYearEdit.SelectedQuick = drv["FLDYEAR"].ToString();
        }

        if (e.Item is GridFooterItem)
        {
            ImageButton db = (ImageButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            UserControlHard ucMonth = (UserControlHard)e.Item.FindControl("ucMonth");
            ucMonth.HardList = PhoenixRegistersHard.ListHard(1, 55, byte.Parse("1"), "");

            ImageButton ibtnshowAccount = (ImageButton)e.Item.FindControl("imgShowAccount");
            if (ibtnshowAccount != null)
                ibtnshowAccount.Attributes.Add("onclick", "return showPickList('spnPickListAccount', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselAccount.aspx', true); ");

            UserControlQuick ucYear = (UserControlQuick)e.Item.FindControl("ucYear");
            ucYear.bind();
        }
    }
}
