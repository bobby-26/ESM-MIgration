using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersNoonReportRangeConfigField : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersNoonReportRangeConfigField.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvNRConfig')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuNRRangeConfig.AccessRights = this.ViewState;
        MenuNRRangeConfig.MenuList = toolbar.Show();

        PhoenixToolbar toolbarReporttap = new PhoenixToolbar();
        //toolbarReporttap.AddButton("Copy", "COPY");
        toolbarReporttap.AddButton("Range Config", "MAP");
        toolbarReporttap.AddButton("Vessel", "LIST");
        toolbarReporttap.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuNewSaveTabStrip.AccessRights = this.ViewState;
        MenuNewSaveTabStrip.MenuList = toolbarReporttap.Show();
        MenuNewSaveTabStrip.SelectedMenuIndex = 0;

        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        try
        {
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvNRConfig.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        string[] alColumns = { "FLDCOLUMNDESCRIPTION", "FLDMINVALUE", "FLDMAXVALUE", "FLDMINALLERTLEVEL", "FLDMAXALLERTLEVEL", "FLDVESSELNAMELIST" };
        string[] alCaptions = { "Field Name", "Min Value", "Max Value", "Min Alert Level", "Max Alert Level", "Exception Vessels" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixRegistersNoonReportRangeConfig.NoonRangeConfigFieldSearch(sortexpression, sortdirection, 1,
            iRowCount, ref iRowCount, ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=NoonReportRangeConfig.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Noon Report Range Config</h3></td>");
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

    protected void MenuNRRangeConfig_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void Rebind()
    {
        gvNRConfig.SelectedIndexes.Clear();
        gvNRConfig.EditIndexes.Clear();
        gvNRConfig.DataSource = null;
        gvNRConfig.Rebind();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixRegistersNoonReportRangeConfig.NoonRangeConfigFieldSearch(sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvNRConfig.PageSize, ref iRowCount, ref iTotalPageCount);

        string[] alColumns = { "FLDCOLUMNDESCRIPTION", "FLDMINVALUE", "FLDMAXVALUE", "FLDMINALLERTLEVEL", "FLDMAXALLERTLEVEL", "FLDVESSELNAMELIST" };
        string[] alCaptions = { "Field Name", "Min Value", "Max Value", "Min Alert Level", "Max Alert Level", "Exception Vessels" };

        General.SetPrintOptions("gvNRConfig", "Noon Report Range Config", alCaptions, alColumns, ds);

        gvNRConfig.DataSource = ds;
        gvNRConfig.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvNRConfig_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvNRConfig_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        RadGrid _gridView = (RadGrid)sender;
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode != 0)
        {
            _gridView.Columns[1].Visible = false;
        }
        if (e.Item is GridEditableItem)
        {
            RadDropDownList ddlFieldNameEdit = (RadDropDownList)e.Item.FindControl("ddlFieldNameEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (ddlFieldNameEdit != null)
            {
                DataSet ds = PhoenixRegistersNoonReportRangeConfig.NoonReportRangeFieldList(null);
                ddlFieldNameEdit.DataSource = ds;
                ddlFieldNameEdit.DataTextField = "FLDCOLUMNDESCRIPTION";
                ddlFieldNameEdit.DataValueField = "FLDCOLUMNNAME";
                ddlFieldNameEdit.DataBind();
            }

            if (ddlFieldNameEdit != null) 
                ddlFieldNameEdit.SelectedValue = drv["FLDCOLUMNNAME"].ToString();

            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            //{
                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");

                if (edit != null)
                    if (!SessionUtil.CanAccess(this.ViewState, edit.CommandName))
                        edit.Visible = false;
            //}

            LinkButton cmdVessel = (LinkButton)e.Item.FindControl("cmdVessel");
            if (cmdVessel != null)
                cmdVessel.Attributes.Add("onclick",
                    "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Registers/RegistersNoonReportRangeConfigVesselMap.aspx?VESSELLIST=" + drv["FLDVESSELLIST"].ToString()
                    + "&COLUMNNAME=" + drv["FLDCOLUMNNAME"].ToString() + "');return true;");

            LinkButton cmdPossibleCause = (LinkButton)e.Item.FindControl("cmdPossibleCause");
            if (cmdPossibleCause != null)
                cmdPossibleCause.Attributes.Add("onclick",
                    "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Registers/RegistersNoonReportRangeConfigPossibelCause.aspx?DISPLAYTEXT=" + drv["FLDCOLUMNDESCRIPTION"].ToString()
                    + "&COLUMNNAME=" + drv["FLDCOLUMNNAME"].ToString() + "');return true;");
        }
       
        if (e.Item is GridDataItem)
        {
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblVesselList");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("uclblVesselList");
            //if (lbtn != null)
            //{
            //    lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            //    lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            //}
            uct.Position = ToolTipPosition.TopCenter;
            uct.TargetControlId = lbtn.ClientID;
        }
    }
    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Registers/RegistersNoonReportRangeConfig.aspx");
            }
            else if(CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridDataItem gvr in gvNRConfig.Items)
                {
                        PhoenixRegistersNoonReportRangeConfig.UpdateNoonRangeConfigField(
                        General.GetNullableInteger(((RadLabel)gvr.FindControl("lblConfigIdEdit")).Text),
                        null,
                        General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtMinValueEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtMaxValueEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtAlertLevelEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtMaxAlertLevelEdit")).Text),
                        ((RadCheckBox)gvr.FindControl("reqinvps")).Checked == true ? 1 : 0,
                        ((RadCheckBox)gvr.FindControl("reqinlog")).Checked == true ? 1 : 0);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvNRConfig_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvNRConfig.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvNRConfig_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "1";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "0";
                break;
        }
    }

    protected void gvNRConfig_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            PhoenixRegistersNoonReportRangeConfig.UpdateNoonRangeConfigField(
                General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblConfigIdEdit")).Text),
                null,
                   General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtMinValueEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtMaxValueEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtAlertLevelEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtMaxAlertLevelEdit")).Text)
                    , ((RadCheckBox)e.Item.FindControl("reqinvps")).Checked == true ? 1 : 0
                    , ((RadCheckBox)e.Item.FindControl("reqinlog")).Checked == true ? 1 : 0);
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
