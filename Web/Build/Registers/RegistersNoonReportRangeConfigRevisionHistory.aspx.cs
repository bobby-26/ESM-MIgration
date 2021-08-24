using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersNoonReportRangeConfigRevisionHistory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersNoonReportRangeConfigRevisionHistory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvNRConfig')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuNRRangeConfig.AccessRights = this.ViewState;
        MenuNRRangeConfig.MenuList = toolbar.Show();

        
        //if (Request.QueryString["vesselid"] == null)
        //    MenuNewSaveTabStrip.SelectedMenuIndex = 1;
        //else
        //    MenuNewSaveTabStrip.SelectedMenuIndex = 0;

        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
        
        try
        {
            if (!IsPostBack)
            {
                ViewState["VESSELID"] = "";
                if(Request.QueryString["vesselid"]!=null)
                {
                    ViewState["VESSELID"] = Request.QueryString["vesselid"];
                    UcVessel.SelectedVessel = Request.QueryString["vesselid"].ToString();
                    UcVessel.Enabled = false;
                }
                else if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                    {
                        ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                        UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                        //UcVessel.Enabled = false;
                    }
                }
                else
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.InstallCode.ToString();
                    //UcVessel.Enabled = false;
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                UcVessel.DataBind();
                UcVessel.bind();

                gvNRConfig.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
               
            }
          //  revisiondetail();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void revisiondetail()
    {
        DataSet ds = PhoenixRegistersNoonReportRangeConfig.RangeConfigRevisionDetail(General.GetNullableInteger(UcVessel.SelectedVessel));
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            txtrevno.Text = dr["FLDREVISIONNO"].ToString();
            ucdate.Text = dr["REVISIONDATE"].ToString();
            ViewState["PUBLISHEDYN"] = dr["FLDPUBLISHEDYN"].ToString();
            if (dr["FLDPUBLISHEDYN"].ToString().Equals("1"))
                ChkPublishedYN.Checked = true;
            else
                ChkPublishedYN.Checked = false;

            //PhoenixToolbar toolbarReporttap = new PhoenixToolbar();
            //if (Request.QueryString["vesselid"] == null)
            //    toolbarReporttap.AddButton("Range Config", "COPY");

            //toolbarReporttap.AddButton("Vessel", "CONFIG");

            //if (ViewState["PUBLISHEDYN"].ToString() == "1" || General.GetNullableInteger(dr["FLDREVISIONNO"].ToString()) == null)
            //    toolbarReporttap.AddButton("Revise", "REVISE", ToolBarDirection.Right);
            //if (ViewState["PUBLISHEDYN"].ToString() != "1" && General.GetNullableInteger(dr["FLDREVISIONNO"].ToString()) != null)
            //{
            //    toolbarReporttap.AddButton("Submit", "SUBMIT", ToolBarDirection.Right);
            //    toolbarReporttap.AddButton("Save", "SAVE", ToolBarDirection.Right);
            //}
            //MenuNewSaveTabStrip.AccessRights = this.ViewState;
            //MenuNewSaveTabStrip.MenuList = toolbarReporttap.Show();

        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDISPLAYTEXT", "FLDVESSELNAME", "FLDMINVALUE", "FLDMAXVALUE", "FLDREQUIREDYESNO", "FLDALERTLEVEL" };
        string[] alCaptions = { "Field Name", "Vessel Name", "Min Value", "Max Value", "Active Y/N", "Alert Level" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        string vesselid;

        vesselid = Request.QueryString["vesselid"];

        DataSet ds = PhoenixRegistersNoonReportRangeConfig.NoonReportRangeConfigSearch(
            General.GetNullableInteger(vesselid),
            sortexpression, sortdirection, 1,
            iRowCount, ref iRowCount, ref iTotalPageCount,
            General.GetNullableGuid(Request.QueryString["revisionid"]));

        Response.AddHeader("Content-Disposition", "attachment; filename=NoonReportRangeConfig.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Noon Report Range Config - Vessel</h3></td>");
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
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        if (CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
        }
    }
    protected void  Rebind()
    {
        gvNRConfig.SelectedIndexes.Clear();
        gvNRConfig.EditIndexes.Clear();
        gvNRConfig.DataSource = null;
        gvNRConfig.Rebind();
    }

    private void ClearFilter()
    {
        //txtAgentName.Text = "";
        UcVessel.SelectedVessel = "";
        Rebind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        string vesselid;
        vesselid = Request.QueryString["vesselid"];

        DataSet ds = PhoenixRegistersNoonReportRangeConfig.NoonReportRangeConfigSearch(
            General.GetNullableInteger(vesselid),
            sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvNRConfig.PageSize, ref iRowCount, ref iTotalPageCount,
            General.GetNullableGuid(Request.QueryString["revisionid"]));

        string[] alColumns = { "FLDDISPLAYTEXT", "FLDVESSELNAME", "FLDMINVALUE", "FLDMAXVALUE", "FLDREQUIREDYESNO", "FLDALERTLEVEL" };
        string[] alCaptions = { "Field Name", "Vessel Name", "Min Value", "Max Value", "Active Y/N", "Alert Level" };

        General.SetPrintOptions("gvNRConfig", "Noon Report Range Config - Vessel", alCaptions, alColumns, ds);

        gvNRConfig.DataSource = ds;
        gvNRConfig.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvNRConfig_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "fnReloadList('codehelp1', null, null);";
                Script += "</script>" + "\n";

                string fieldname, displaytext, minvalue, maxvalue, requiredyn, Vessel, alertlevel, maxalertlevel;
                fieldname = ((RadDropDownList)e.Item.FindControl("ddlFieldNameAdd")).SelectedValue;
                displaytext = ((RadDropDownList)e.Item.FindControl("ddlFieldNameAdd")).SelectedItem.Text;
                minvalue = ((UserControlMaskNumber)e.Item.FindControl("txtMinValueAdd")).Text;
                maxvalue = ((UserControlMaskNumber)e.Item.FindControl("txtMaxValueAdd")).Text;
                alertlevel = ((UserControlMaskNumber)e.Item.FindControl("txtAlertLevelAdd")).Text;
                maxalertlevel = ((UserControlMaskNumber)e.Item.FindControl("txtMaxAlertLevelAdd")).Text;
                requiredyn = ((RadCheckBox)e.Item.FindControl("chkRequiredAdd")).Checked.Equals(true) ? "1" : "0";
                //Vessel = ((UserControlVessel)_gridView.FooterRow.FindControl("ucVesselAdd")).SelectedVessel;
                Vessel = UcVessel.SelectedVessel;

                if (!IsValidConfig(fieldname))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersNoonReportRangeConfig.InsertNoonReportRangeConfig(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    fieldname,
                    displaytext,
                    General.GetNullableInteger(Vessel),
                   General.GetNullableDecimal( minvalue),
                    General.GetNullableDecimal(maxvalue),
                    General.GetNullableInteger(requiredyn),
                    General.GetNullableDecimal(alertlevel),
                    General.GetNullableDecimal(maxalertlevel));

                BindData();
                String script = "javascript:fnReloadList('codehelp1',null,'');";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersNoonReportRangeConfig.DeleteNoonReportRangeConfig(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid((((RadLabel)e.Item.FindControl("lblConfigId")).Text)));
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

    private bool IsValidConfig(string fieldname)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if ((fieldname == null) || (fieldname == ""))
            ucError.ErrorMessage = "Field Name is required.";

        //if (General.GetNullableInteger(UcVessel.SelectedVessel) == null)
        //    ucError.ErrorMessage = "Vessel is required.";

        return (!ucError.IsError);
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

            if (ddlFieldNameEdit != null)
            {
                DataSet ds = PhoenixRegistersNoonReportRangeConfig.NoonReportRangeFieldList(null);
                ddlFieldNameEdit.DataSource = ds;
                ddlFieldNameEdit.DataTextField = "FLDCOLUMNDESCRIPTION";
                ddlFieldNameEdit.DataValueField = "FLDCOLUMNNAME";
                ddlFieldNameEdit.DataBind();
   
            }

            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ddlFieldNameEdit != null) 
                ddlFieldNameEdit.SelectedValue = drv["FLDCOLUMNNAME"].ToString();

            UserControlVessel ucVesselEdit = (UserControlVessel)e.Item.FindControl("ucVesselEdit");
            if (ucVesselEdit != null)
                ucVesselEdit.SelectedVessel = drv["FLDVESSELID"].ToString();

            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit))
            //{
                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");

                if (del != null)
                    if (!SessionUtil.CanAccess(this.ViewState, del.CommandName) || PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 || Request.QueryString["vesselid"] != null)
                        del.Visible = false;

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");

                if (edit != null)
                    if (!SessionUtil.CanAccess(this.ViewState, edit.CommandName) || PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 || Request.QueryString["vesselid"] != null)
                        edit.Visible = false;

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");

                if (save != null)
                    if (!SessionUtil.CanAccess(this.ViewState, save.CommandName) || PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 || Request.QueryString["vesselid"] != null)
                        save.Visible = false;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                }
            //}
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton add = (LinkButton)e.Item.FindControl("cmdAdd");

            if (add != null)
                if (!SessionUtil.CanAccess(this.ViewState, add.CommandName) || PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 || Request.QueryString["vesselid"] != null)
                    add.Visible = false;

            RadDropDownList ddlFieldNameAdd = (RadDropDownList)e.Item.FindControl("ddlFieldNameAdd");

            if (ddlFieldNameAdd != null)
            {
                DataSet ds = PhoenixRegistersNoonReportRangeConfig.NoonReportRangeFieldList(null);
                ddlFieldNameAdd.DataSource = ds;
                ddlFieldNameAdd.DataTextField = "FLDCOLUMNDESCRIPTION";
                ddlFieldNameAdd.DataValueField = "FLDCOLUMNNAME";
                ddlFieldNameAdd.DataBind();
            }
        }
    }
    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("COPY"))
            {
                Response.Redirect("../Registers/RegistersNoonReportRangeConfigField.aspx");
            }
            if (CommandName.ToUpper().Equals("REVISE"))
            {
                PhoenixRegistersNoonReportRangeConfig.copyrevision(General.GetNullableInteger(UcVessel.SelectedVessel));
                revisiondetail();
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                update();
            }
            if (CommandName.ToUpper().Equals("SUBMIT"))
            {
                PhoenixRegistersNoonReportRangeConfig.RevisionUpdate(General.GetNullableInteger(UcVessel.SelectedVessel));
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void update()
    {
        string Script = "";
        Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
        Script += "fnReloadList('codehelp1', null, null);";
        Script += "</script>" + "\n";
        foreach (GridDataItem gvr in gvNRConfig.Items)
        {

            PhoenixRegistersNoonReportRangeConfig.UpdateNoonReportRangeConfigrevision(
                new Guid(((RadLabel)gvr.FindControl("lblconfigrevisionid")).Text),
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableString(((RadLabel)gvr.FindControl("lblfield")).Text),
                General.GetNullableString(((RadLabel)gvr.FindControl("lbldisplaytext")).Text),
                 General.GetNullableInteger(UcVessel.SelectedVessel),
                   General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtMinValueEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtMaxValueEdit")).Text),
                    General.GetNullableInteger(((RadCheckBox)gvr.FindControl("chkRequiredEdit")).Checked.Equals(true) ? "1" : "0"),
                    General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtAlertLevelEdit")).Text),
                    General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtMaxAlertLevelEdit")).Text));
        }
        Rebind();
        String script = "javascript:fnReloadList('codehelp1',null,'');";
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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

    protected void gvNRConfig_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string Script = "";
            Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
            Script += "fnReloadList('codehelp1', null, null);";
            Script += "</script>" + "\n";

            string fieldname, displaytext, minvalue, maxvalue, requiredyn, configid, Vessel, alertlevel, maxalertlevel;
            configid = ((RadLabel)e.Item.FindControl("lblConfigIdEdit")).Text;
            fieldname = ((RadDropDownList)e.Item.FindControl("ddlFieldNameEdit")).SelectedValue;
            displaytext = ((RadDropDownList)e.Item.FindControl("ddlFieldNameEdit")).SelectedItem.Text;
            minvalue = ((UserControlMaskNumber)e.Item.FindControl("txtMinValueEdit")).Text;
            maxvalue = ((UserControlMaskNumber)e.Item.FindControl("txtMaxValueEdit")).Text;
            alertlevel = ((UserControlMaskNumber)e.Item.FindControl("txtAlertLevelEdit")).Text;
            maxalertlevel = ((UserControlMaskNumber)e.Item.FindControl("txtMaxAlertLevelEdit")).Text;
            requiredyn = ((RadCheckBox)e.Item.FindControl("chkRequiredEdit")).Checked.Equals(true)? "1" : "0";
            //Vessel = ((UserControlVessel)_gridView.Rows[nCurrentRow].FindControl("ucVesselEdit")).SelectedVessel;
            Vessel = UcVessel.SelectedVessel;

            if (!IsValidConfig(fieldname))
            {
                e.Canceled = true;
                ucError.Visible = true;
                return;
            }

            PhoenixRegistersNoonReportRangeConfig.UpdateNoonReportRangeConfig(
                new Guid(configid),
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                fieldname,
                displaytext,
                 General.GetNullableInteger(Vessel),
                   General.GetNullableDecimal(minvalue),
                    General.GetNullableDecimal(maxvalue),
                    General.GetNullableInteger(requiredyn),
                    General.GetNullableDecimal(alertlevel),
                    General.GetNullableDecimal(maxalertlevel));
            Rebind();
            String script = "javascript:fnReloadList('codehelp1',null,'');";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void UcVessel_TextChangedEvent(object sender, EventArgs e)
    {
        try
        {
            revisiondetail();
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
