using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.StartDateConfiguration;
using Telerik.Web.UI;
public partial class VesselAccountsRHVesselStartDateConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHVesselStartDateConfiguration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvStartDateConfig')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHVesselStartDateConfiguration.aspx", "Search", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../VesselAccounts/VesselAccountsRHVesselStartDateConfiguration.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuStartDateConfig.AccessRights = this.ViewState;
            MenuStartDateConfig.MenuList = toolbar.Show();

            PhoenixToolbar toolbartap = new PhoenixToolbar();
            toolbartap.AddButton("Start Date Config", "CONGIF", ToolBarDirection.Right);
            toolbartap.AddButton("Lock/UnLock", "LOCK", ToolBarDirection.Right);
            MenuConfigTabStrip.AccessRights = this.ViewState;
            MenuConfigTabStrip.MenuList = toolbartap.Show();
            MenuConfigTabStrip.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvStartDateConfig.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ConfigTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("LOCK"))
            {
                Response.Redirect("../VesselAccounts/VesselAccountsRHLockUnlock.aspx");
            }
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuStartDateConfig_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                txtVesselName.Text = "";
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

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSELID", "FLDVESSELNAME", "FLDSTARTDATE" };
            string[] alCaptions = { "ID", "Vessel", "Start Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataTable dt = PhoenixVesselAccountsRHStartDateConfiguration.StartDateConfigurationList(txtVesselName.Text,
                sortexpression, sortdirection,
                1,
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

            General.ShowExcel("Start Date Configuration", dt, alColumns, alCaptions, null, string.Empty);
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
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSELID", "FLDVESSELNAME", "FLDSTARTDATE" };
            string[] alCaptions = { "ID", "Vessel", "Start Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixVesselAccountsRHStartDateConfiguration.StartDateConfigurationList(txtVesselName.Text,
                sortexpression, sortdirection,
                (int)ViewState["PAGENUMBER"],
                gvStartDateConfig.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            gvStartDateConfig.DataSource = dt;
            gvStartDateConfig.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

            General.SetPrintOptions("gvStartDateConfig", "Start Date Configuration", alCaptions, alColumns, ds);
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidStartDateConfig(string StartDate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (StartDate == null)
            ucError.ErrorMessage = "Start Date is required.";

        return (!ucError.IsError);
    }
    protected void gvStartDateConfig_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
                if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

                LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvStartDateConfig_ItemCommand(object sender, GridCommandEventArgs e)
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

    protected void gvStartDateConfig_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string VesselId = ((RadLabel)e.Item.FindControl("lblvesselid")).Text;
            string StartDate = ((UserControlDate)e.Item.FindControl("ucDateEdit")).Text;
            bool opa90YN = ((RadCheckBox)e.Item.FindControl("chkopaedit")).Checked.Equals(true);

            if (!IsValidStartDateConfig(StartDate))
            {
                ucError.Visible = true;
                return;
            }

            PhoenixVesselAccountsRHStartDateConfiguration.StartDateConfigurationInsert
                    (
                        General.GetNullableInteger(VesselId),
                        General.GetNullableDateTime(StartDate),
                        opa90YN ? byte.Parse("1") : byte.Parse("0"),
                        ""
                    );
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvStartDateConfig_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvStartDateConfig.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvStartDateConfig.EditIndexes.Clear();
        gvStartDateConfig.SelectedIndexes.Clear();
        gvStartDateConfig.DataSource = null;
        gvStartDateConfig.Rebind();
    }
}