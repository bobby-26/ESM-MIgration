using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using SouthNests.Phoenix.Elog;
public partial class ElectronicLogEngineParameterConfig : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        UserAccessRights.SetUserAccess(Page.Controls, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 1);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Log/ElectronicLogEngineParameterConfig.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvNRConfig')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuNRRangeConfig.AccessRights = this.ViewState;
        MenuNRRangeConfig.MenuList = toolbar.Show();

        PhoenixToolbar toolbarReporttap = new PhoenixToolbar();
        toolbarReporttap.AddButton("Revise", "REVISE", ToolBarDirection.Right);
        toolbarReporttap.AddButton("Submit", "SUBMIT", ToolBarDirection.Right);   
        toolbarReporttap.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuNewSaveTabStrip.AccessRights = this.ViewState;
        MenuNewSaveTabStrip.MenuList = toolbarReporttap.Show();
        MenuNewSaveTabStrip.SelectedMenuIndex = 0;

        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        try
        {
            if (!IsPostBack)
            {
                bindtype();
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
    protected void bindtype()
    {
        DataSet ds = PhoenixEngineLogAttributes.EngineLogAttributesTypeList();
        ddltype.DataTextField = "FLDNAME";
        ddltype.DataValueField = "FLDTYPEID";
        ddltype.DataSource = ds;
        ddltype.DataBind();
        ddltype.SelectedIndex = 1;
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDDESCRIPTION", "FLDDATATYPE", "FLDFORMAT", "FLDUNIT", "FLDMINVAL", "FLDMAXVAL", "FLDMINALERT", "FLDMAXALERT", "FLDALLOWNAYN"};
        string[] alCaptions = { "Field Name", "Type", "Format", "Unit", "Min Value", "Max Value", "Min Alert Level", "Max Alert Level", "Allow NA" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        DataSet ds = PhoenixEngineLogAttributes.EngineParameterConfigSearch(sortexpression, sortdirection, 1,
            iRowCount, ref iRowCount, ref iTotalPageCount, PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        Response.AddHeader("Content-Disposition", "attachment; filename=EngineParameter.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Engine Parameter</h3></td>");
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

        DataSet ds = PhoenixEngineLogAttributes.EngineParameterConfigSearch(sortexpression, sortdirection, int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvNRConfig.PageSize, ref iRowCount, ref iTotalPageCount, PhoenixSecurityContext.CurrentSecurityContext.VesselID);

        string[] alColumns = { "FLDDESCRIPTION", "FLDDATATYPE", "FLDFORMAT", "FLDUNIT", "FLDMINVAL", "FLDMAXVAL", "FLDMINALERT", "FLDMAXALERT", "FLDALLOWNAYN"};
        string[] alCaptions = { "Field Name", "Type", "Format", "Unit", "Min Value", "Max Value", "Min Alert Level", "Max Alert Level", "Allow NA" };

        General.SetPrintOptions("gvNRConfig", "Engine Parameter", alCaptions, alColumns, ds);

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

    }
    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                foreach (GridDataItem gvr in gvNRConfig.Items)
                {
                    string minvalue, maxvalue, parameterid, configid, alertlevel, maxalertlevel, parameter;
                    int allowna, isactive;
                    parameterid = ((RadLabel)gvr.FindControl("lblparameterId")).Text;
                    configid = ((RadLabel)gvr.FindControl("lblparameterconfigIdEdit")).Text;
                    minvalue = ((UserControlMaskNumber)gvr.FindControl("txtMinValueEdit")).Text;
                    maxvalue = ((UserControlMaskNumber)gvr.FindControl("txtMaxValueEdit")).Text;
                    alertlevel = ((UserControlMaskNumber)gvr.FindControl("txtAlertLevelEdit")).Text;
                    maxalertlevel = ((UserControlMaskNumber)gvr.FindControl("txtMaxAlertLevelEdit")).Text;
                    parameter = ((RadLabel)gvr.FindControl("lblparameter")).Text;
                    allowna = ((RadCheckBox)gvr.FindControl("chkallowna")).Checked == true ? 1 : 0;
                    isactive = 1;

                    if (General.GetNullableGuid(configid) != null)
                    {
                        PhoenixEngineLogAttributes.EngineParameterConfigUpdate(
                            General.GetNullableGuid(configid),
                               General.GetNullableString(minvalue),
                                General.GetNullableString(maxvalue),
                                General.GetNullableString(alertlevel),
                                General.GetNullableString(maxalertlevel),
                                allowna,
                                isactive,
                                General.GetNullableString(parameter));
                    }
                    else
                    {
                        PhoenixEngineLogAttributes.EngineParameterConfigInsert(
                            General.GetNullableInteger(parameterid),
                               General.GetNullableString(minvalue),
                                General.GetNullableString(maxvalue),
                                General.GetNullableString(alertlevel),
                                General.GetNullableString(maxalertlevel),
                                allowna,
                                isactive,
                                General.GetNullableString(parameter),
                                PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                    }
                }
                Status1.Text = "Saved Successfully";
                Rebind();
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
            
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddltype_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if(ddltype.SelectedValue != "3")
        {
            Response.Redirect("../Log/ElectricLogEngineAttributes.aspx");
        }
    }
}
