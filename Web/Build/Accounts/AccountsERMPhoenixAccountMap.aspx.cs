using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsERMPhoenixAccountMap : PhoenixBasePage
{
    // protected override void Render(HtmlTextWriter writer)
    // {
    //     foreach (GridViewRow r in gvERMPhonixAccMap.Rows)
    //     {
    //         if (r.RowType == DataControlRowType.DataRow)
    //         {
    //             Page.ClientScript.RegisterForEventValidation(gvERMPhonixAccMap.UniqueID, "Edit$" + r.RowIndex.ToString());
    //         }
    //     }
    //
    //     base.Render(writer);
    // }

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../Accounts/AccountsERMPhoenixAccountMap.aspx", "Export to Excel", "icon_xls.png", "Excel");
            toolbar.AddImageLink("javascript:CallPrint('gvERMPhonixAccMap')", "Print Grid", "icon_print.png", "PRINT");
            toolbar.AddImageLink("../Accounts/AccountsERMPhoenixAccountMap.aspx", "Find", "search.png", "FIND");
            toolbar.AddImageButton("../Accounts/AccountsERMPhoenixAccountMap.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenufrmERMPhonixAccMap.AccessRights = this.ViewState;
            MenufrmERMPhonixAccMap.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["SHOWYN"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvERMPhonixAccMap.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
              //  BindData();

            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void frmERMPhonixAccMap_TabStripCommand(object sender, EventArgs e)
    {
        try
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
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                xaccount.Text = string.Empty;
                xdescription.Text = string.Empty;
                phoenixaccCode.Text = string.Empty;
                PhoenixaccDescription.Text = string.Empty;
                xaccType.Text = string.Empty;
                XaccUsage.Text = string.Empty;
                XAccGroup.Text = string.Empty;
                Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvERMPhonixAccMap_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (Validate((((RadTextBox)e.Item.FindControl("txtPhoenixACCodeAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtPhoenixACDescAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXaccAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXDescAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXAcccTypeAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXAcccUsageAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXAcccSourceAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXAcccGroupAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtZIDAdd")).Text)))
                {
                    PhoenixERMPhonixAccMap.ERMPhoenixAccMapInsert((((RadTextBox)e.Item.FindControl("txtPhoenixACCodeAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtPhoenixACDescAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXaccAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXDescAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXAcccTypeAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXAcccUsageAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXAcccSourceAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXAcccGroupAdd")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtZIDAdd")).Text)
                                                          );
                    ucStatus.Text = "Account Saved Successfully";
                    gvERMPhonixAccMap.Rebind();
                }
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (Validate((((RadTextBox)e.Item.FindControl("txtPhoenixACCodeEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtPhoenixACDescEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXaccEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXDescEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXAcccTypeEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXAcccUsageEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXAcccSourceEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtXAcccGroupEdit")).Text),
                                                            (((RadTextBox)e.Item.FindControl("txtZIDEdit")).Text)))
                {

                    PhoenixERMPhonixAccMap.ERMPhoenixAccMapUpdate((((RadTextBox)e.Item.FindControl("txtPhoenixACCodeEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtPhoenixACDescEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtXaccEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtXDescEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtXAcccTypeEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtXAcccUsageEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtXAcccSourceEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtXAcccGroupEdit")).Text),
                                                                (((RadTextBox)e.Item.FindControl("txtZIDEdit")).Text),
                                                                General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbldtkeyEdit")).Text)
                                                              );

                    ucStatus.Text = "Account Updated Successfully";
                }
                gvERMPhonixAccMap.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixERMPhonixAccMap.ERMPhoenixAccMapDelete(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lbldtkey")).Text));
                gvERMPhonixAccMap.Rebind();
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool Validate(string PhonixAccountCode
                                                    , string PhonixAccountDescription
                                                    , string XAcc
                                                    , string XAccDesc
                                                    , string XAccType
                                                    , string XAccUsage
                                                    , string XAccSource
                                                    , string XaccGroup
                                                    , string ZID)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        //if (string.IsNullOrEmpty(PhonixAccountDescription))
        //    ucError.ErrorMessage = "Phonix Account Description is required.";
        //if (string.IsNullOrEmpty(XAcc))
        //    ucError.ErrorMessage = "X Account is required.";
        //if (string.IsNullOrEmpty(XAccDesc))
        //    ucError.ErrorMessage = "X Account Description is required.";
        //if (string.IsNullOrEmpty(XAccType))
        //    ucError.ErrorMessage = "X Account Type is required.";
        //if (string.IsNullOrEmpty(XAccUsage))
        //    ucError.ErrorMessage = "X Account Usage is required.";
        //if (string.IsNullOrEmpty(XAccSource))
        //    ucError.ErrorMessage = "X Account Source is required.";
        //if (string.IsNullOrEmpty(XaccGroup))
        //    ucError.ErrorMessage = "X Account Group is required.";
        //if (string.IsNullOrEmpty(ZID))
        //    ucError.ErrorMessage = "ZID is required.";

        if (ucError.ErrorMessage != "")
            ucError.Visible = true;

        return (!ucError.IsError);
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDPHOENIXACCOUNTCODE", "FLDPHOENIXACCTDESCRIPTION", "FLDXACC", "FLDXDESC", "FLDXACCTYPE", "FLDXACCUSAGE", "FLDXACCSOURCE", "FLDXACCGROUP", "FLDZID" };
        string[] alCaptions = { "Phoenix Account Code", "Phoenix Account Description", "X Account", "X Account Description", "X Account Type", "X Account Usage", "X Account Source", "X Account Group", "ZID" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        ds = PhoenixERMPhonixAccMap.ERMPhonixAccMapSearch(xaccount.Text
                                                        , xdescription.Text
                                                        , phoenixaccCode.Text
                                                        , PhoenixaccDescription.Text
                                                        , xaccType.Text
                                                        , XaccUsage.Text
                                                        , XAccGroup.Text
                                                        , (int)ViewState["PAGENUMBER"]
                                                        , gvERMPhonixAccMap.PageSize
                                                        , ref iRowCount
                                                        , ref iTotalPageCount);

        General.SetPrintOptions("gvERMPhonixAccMap", "ERM Phoenix Account Map", alCaptions, alColumns, ds);

        gvERMPhonixAccMap.DataSource = ds;
        gvERMPhonixAccMap.VirtualItemCount = iTotalPageCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvERMPhonixAccMap_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvERMPhonixAccMap.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvERMPhonixAccMap_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            ImageButton edit = (ImageButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            ImageButton save = (ImageButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            ImageButton cancel = (ImageButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    protected void gvERMPhonixAccMap_RowCreated(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
        && e.Row.RowState != (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal))
            {
                e.Row.TabIndex = -1;
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackEventReference(gvERMPhonixAccMap, "Edit$" + e.Row.RowIndex.ToString(), false);
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }
    }

    private void Rebind()
    {
        gvERMPhonixAccMap.EditIndexes.Clear();
        gvERMPhonixAccMap.SelectedIndexes.Clear();
        gvERMPhonixAccMap.DataSource = null;
        gvERMPhonixAccMap.Rebind();
    }

    protected void gvERMPhonixAccMap_Sorting(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDPHOENIXACCOUNTCODE", "FLDPHOENIXACCTDESCRIPTION", "FLDXACC", "FLDXDESC", "FLDXACCTYPE", "FLDXACCUSAGE", "FLDXACCSOURCE", "FLDXACCGROUP", "FLDZID" };
        string[] alCaptions = { "Phoenix Account Code", "Phoenix Account Description", "X Account", "X Account Description", "X Account Type", "X Account Usage", "X Account Source", "X Account Group", "ZID" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixERMPhonixAccMap.ERMPhonixAccMapSearch(xaccount.Text
                                                 , xdescription.Text
                                                 , phoenixaccCode.Text
                                                 , PhoenixaccDescription.Text
                                                 , xaccType.Text
                                                 , XaccUsage.Text
                                                 , XAccGroup.Text
                                                 , (int)ViewState["PAGENUMBER"]
                                                 , PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount
                                                 , ref iRowCount
                                                 , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=\"ERMPhoenixAccounMap.xls\"");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>ERM Phoenix Account Map</h3></td>");
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
}
