using System;
using System.Web.UI.WebControls;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class OptionModuleVesselConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            // ViewState["SORTEXPRESSION"] = null;
            // ViewState["SORTDIRECTION"] = null;
            ViewState["VesselName"] = null;

            ddlAdministratorMenuList.DataSource = PhoenixCommonModuleVesselConfiguration.ApplicationList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            ddlAdministratorMenuList.DataBind();

            gvVesselList.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            
        }
        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddImageButton("../Options/OptionModuleVesselConfiguration.aspx", "Export to Excel", "icon_xls.png", "Excel");
        toolbargrid.AddImageLink("javascript:CallPrint('gvVesselList')", "Print Grid", "icon_print.png", "Print");
        toolbargrid.AddFontAwesomeButton("../Options/OptionModuleVesselConfiguration.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

        MenuDataSynchronizer.AccessRights = this.ViewState;
        MenuDataSynchronizer.MenuList = toolbargrid.Show();

        ucowner.AddressType = ((int)PhoenixAddressType.PRINCIPAL).ToString();

    }
    protected void MenuDataSynchronizer_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ddlAdministratorMenuList.SelectedIndex = -1;
                txtvesselName.Text = "";
                ucTechFleet.SelectedFleet = "";
                ucManagementType.SelectedHard = "";
                ucowner.Text = "";
                chkoutofmanagement.Checked = false;
                ViewState["PAGENUMBER"] = 1;
                Rebind();
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

        string[] alColumns = { "FLDVESSELCODE", "FLDVESSELNAME", "FLDACTIVE" };
        string[] alCaptions = { "Vessel Code", "Vessel Name", " Active Y/N" };

        //string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        //int? sortdirection = null;

        //if (ViewState["SORTDIRECTION"] != null)
        //    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixCommonModuleVesselConfiguration.ModuleVesselConfigurationsearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                            General.GetNullableString(txtvesselName.Text),
                                                                            General.GetNullableInteger(ucTechFleet.SelectedFleet),
                                                                            General.GetNullableInteger(ddlAdministratorMenuList.SelectedValue),
                                                                            General.GetNullableInteger(ucManagementType.SelectedHard),
                                                                            General.GetNullableInteger(ucowner.SelectedValue),
                                                                            General.GetNullableInteger(chkoutofmanagement.Checked == true ? "1" : "0"),
                                                                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                            gvVesselList.PageSize,
                                                                            ref iRowCount,
                                                                            ref iTotalPageCount
                                                                            );


        General.SetPrintOptions("gvVesselList", "Module Vessel Configuration", alCaptions, alColumns, ds);
        gvVesselList.DataSource = ds;
        gvVesselList.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    private void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELCODE", "FLDVESSELNAME", "FLDACTIVE" };
        string[] alCaptions = { "Vessel Code", "Vessel Name", " Active Y/N" };

        //string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        //int? sortdirection = null;

        //if (ViewState["SORTDIRECTION"] != null)
        //    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixCommonModuleVesselConfiguration.ModuleVesselConfigurationsearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                                    General.GetNullableString(txtvesselName.Text),
                                                                                    General.GetNullableInteger(ucTechFleet.SelectedFleet), 
                                                                                    General.GetNullableInteger(ddlAdministratorMenuList.SelectedValue),
                                                                                    General.GetNullableInteger(ucManagementType.SelectedHard),
                                                                                    General.GetNullableInteger(ucowner.SelectedValue),
                                                                                    General.GetNullableInteger(chkoutofmanagement.Checked == true ? "1" : "0"),
                                                                                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                                    PhoenixGeneralSettings.CurrentGeneralSetting.ExcelRecordsCount,
                                                                                    ref iRowCount,
                                                                                    ref iTotalPageCount
                                                                                    );

        Response.AddHeader("Content-Disposition", "attachment; filename=ModuleVesselConfiguration.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>" + ddlAdministratorMenuList.Text + "</h3></td>");
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
    protected void chk_changed(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvVesselList.Rebind();

    }

    protected void ucFleet_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;       
        gvVesselList.Rebind();

    }

    protected void txtVesselName_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvVesselList.Rebind();

    }
    protected void ddlAdministratorMenuList_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvVesselList.Rebind();

    }
    protected void ucManagementType_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvVesselList.Rebind();

    }
    protected void ucowner_TextChanged(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvVesselList.Rebind();

    }


    protected void Rebind()
    {
        gvVesselList.SelectedIndexes.Clear();
        gvVesselList.EditIndexes.Clear();
        gvVesselList.DataSource = null;
        gvVesselList.Rebind();
    }

    protected void gvVesselList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATEACTIVEYN"))
            {
                if (!IsValidAdd())
                {
                    ucError.Visible = true;
                    return;
                }

                string activeyn = ((RadCheckBox)e.Item.FindControl("chkActiveYN")).Checked == true ? "1" : "0";
                string vesselid = ((RadLabel)e.Item.FindControl("lblVesselId")).Text;

                PhoenixCommonModuleVesselConfiguration.ModuleVesselConfigurationinsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(vesselid),
                                    int.Parse(ddlAdministratorMenuList.SelectedValue), int.Parse(activeyn));

                Rebind();
                gvVesselList.Rebind();

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

    private bool IsValidAdd()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(ddlAdministratorMenuList.SelectedValue) == null)
            ucError.ErrorMessage = "Application is required.";

        return (!ucError.IsError);

    }

    protected void gvVesselList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadCheckBox cb = (RadCheckBox)e.Item.FindControl("chkActiveYN");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            cb.Checked = drv["FLDACTIVEYN"].ToString().Equals("1") ? true : false;

        }
    }
    protected void gvVesselList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvVesselList.CurrentPageIndex + 1;
        BindData();
    }

}
