using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.Profile;
using Telerik.Web.UI;
public partial class OptionsMobilePermissions : PhoenixBasePage
{
    private string device = string.Empty;
    private string deviceType = string.Empty;
    private string language = string.Empty;
    private string region = string.Empty;
    private string manufacturer = string.Empty;
    private string username = string.Empty;



    private DateTime? effective;

    private string[] alColumns = { "FLDAPPCODE", "FLDDEVICE", "FLDDEVICETYPE", "FLDMANUFACTURER", "FLDREGION", "FLDLANGUAGE", "FLDAPPOSVERSION", "FLDAPPSDKVERSION", "FLDMODEL", "FLDUNIQUEDEVICEDTKEY", "FLDUSERNAME", "FLDEFFECTIVEDATE", "FLDACTIVE", "FLDLOGINACTIVE" };
    private string[] alCaptions = { "Application", "Device", "Type", "Manufacturer", "Region", "Language", "OSVersion", "SDK", "Model", "Identity", "Username", "Effective Till", "Active", "Logged" };
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Options/OptionsMobilePermissions.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMobilePermissions')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Options/OptionsMobilePermissions.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Options/OptionsMobilePermissions.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");

            //toolbar.AddButton("User Identity", "IDENTITY", ToolBarDirection.Right);
            toolbar.AddButton("Log", "LOG", ToolBarDirection.Right);
            //toolbar.AddButton("Login Audit", "BUDGET", ToolBarDirection.Right);

            MenuOptionsMobilePermissions.AccessRights = this.ViewState;
            MenuOptionsMobilePermissions.MenuList = toolbar.Show();
            MenuOptionsMobilePermissions.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {
                chkActive.Checked = true;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvMobilePermissions.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        int active = 0;
        DataSet ds = new DataSet();

        string sortexpression;
        int? sortdirection = null;
        if (chkActive.Checked == true)
        {
            active = 1;
        }

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


        setFilterText();
        ds = PhoenixGeneralSettings.DeviceAccessSearch(username, effective, device, deviceType, language, region, manufacturer, active, sortexpression, sortdirection,
        1,
        iRowCount,
        ref iRowCount,
        ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=DeviceList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Device List</h3></td>");
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

    protected void RegistersMobilePermissions_TabStripCommand(object sender, EventArgs e)
    {
        try
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
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                //txtSearch.Text = string.Empty;
                txtDevice.Text = string.Empty;
                txtDeviceType.Text = string.Empty;
                txtLanguage.Text = string.Empty;
                txtRegion.Text = string.Empty;
                txtManufacturer.Text = string.Empty;
                txtUsername.Text = string.Empty;
                chkActive.Checked = true;
                txtEffective.Text = "";
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            //if (CommandName.ToUpper().Equals("IDENTITY"))
            //{
            //    Response.Redirect("~/Options/OptionsMobilePermissions.aspx");
            //}
            if (CommandName.ToUpper().Equals("LOG"))
            {
                Response.Redirect("~/Options/OptionsMobilePermissionsLog.aspx");
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvMobilePermissions.SelectedIndexes.Clear();
        gvMobilePermissions.EditIndexes.Clear();
        gvMobilePermissions.DataSource = null;
        gvMobilePermissions.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int active = 0;

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (chkActive.Checked == true)
        {
            active = 1;
        }
        setFilterText();
        DataSet ds = PhoenixGeneralSettings.DeviceAccessSearch(username, effective, device, deviceType, language, region, manufacturer, active, sortexpression, sortdirection,
            int.Parse(ViewState["PAGENUMBER"].ToString()),
            gvMobilePermissions.PageSize,
            ref iRowCount,
            ref iTotalPageCount);
        General.SetPrintOptions("gvMobilePermissions", "Device Access", alCaptions, alColumns, ds);

        gvMobilePermissions.DataSource = ds;
        gvMobilePermissions.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvMobilePermissions_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidDevice(((RadTextBox)e.Item.FindControl("txtDeviceAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtDeviceTypeAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtManufacturerAdd")).Text,
                    //((RadTextBox)e.Item.FindControl("txtModelAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtRegionAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtLanguageAdd")).Text
                ))
                {
                    e.Canceled = true;
                    ucError.Visible = true;
                    return;
                }
                //InsertCurrency(
                //    ((RadTextBox)e.Item.FindControl("txtDeviceAdd")).Text,
                //    ((RadTextBox)e.Item.FindControl("txtDeviceTypeAdd")).Text,
                //    (((RadCheckBox)e.Item.FindControl("chkActiveYNAdd")).Checked.Equals(true)) ? 1 : 0,
                //    ((UserControlMaskedTextBox)e.Item.FindControl("txtExchangerateAddUSD")).Text,
                //    ((RadTextBox)e.Item.FindControl("txtModifiedDateAdd")).Text,
                //    ((UserControlCountry)e.Item.FindControl("ucCountryAdd")).SelectedCountry
                //);
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtDeviceAdd")).Focus();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                //if (!IsValidDevice(((RadTextBox)e.Item.FindControl("txtDeviceEdit")).Text,
                //   ((RadTextBox)e.Item.FindControl("txtDeviceTypeEdit")).Text))
                //{
                //    e.Canceled = true;
                //    ucError.Visible = true;
                //    return;
                //}

                string lblApplication = string.Empty;
                string lblmanufacturer = string.Empty;
                string lblmodel = string.Empty;
                string lblsdk = string.Empty;
                string lblosversion = string.Empty;
                string lbldevice = string.Empty;
                string lbldeviceType = string.Empty;
                string lbllanguage = string.Empty;
                string lblregion = string.Empty;
                string lblusername = string.Empty;

                lblApplication = ((RadLabel)e.Item.FindControl("lblApplication")).Text;
                lblmanufacturer = ((RadLabel)e.Item.FindControl("lblManufacturer")).Text;
                lblmodel = ((RadLabel)e.Item.FindControl("lblModel")).Text;
                lblsdk = ((RadLabel)e.Item.FindControl("lblSDKVersion")).Text;
                lblosversion = ((RadLabel)e.Item.FindControl("lblOSVersion")).Text;
                lbldevice = ((RadLabel)e.Item.FindControl("lblDevice")).Text;
                lbldeviceType = ((RadLabel)e.Item.FindControl("lblDeviceType")).Text;
                lbllanguage = ((RadLabel)e.Item.FindControl("lblLanguage")).Text;
                lblregion = ((RadLabel)e.Item.FindControl("lblRegion")).Text;

                if (lblmanufacturer.Trim().Equals(""))
                    lblmanufacturer = null;
                if (lblmodel.Trim().Equals(""))
                    lblmodel = null;
                if (lblsdk.Trim().Equals(""))
                    lblsdk = null;
                if (lblosversion.Trim().Equals(""))
                    lblosversion = null;
                if (lbldevice.Trim().Equals(""))
                    lbldevice = null;
                if (lbldeviceType.Trim().Equals(""))
                    lbldeviceType = null;
                if (lbllanguage.Trim().Equals(""))
                    lbllanguage = null;
                if (lblregion.Trim().Equals(""))
                    lblregion = null;

                int isactive = (((RadCheckBox)e.Item.FindControl("chkActiveYNEdit")).Checked.Equals(true)) ? 1 : 0;
                string loginactive = (((RadCheckBox)e.Item.FindControl("chkLoginActiveYNEdit")).Checked.Equals(true)) ? "Y" : "N";

                if (isactive == 0)
                    loginactive = "N";

                string dateofissue = ((UserControlDate)e.Item.FindControl("ucEffectiveEdit")).Text;

                UpdateDevice(
                     ((RadLabel)e.Item.FindControl("lbluuid")).Text,
                     Convert.ToInt32(((RadLabel)e.Item.FindControl("lblUserId")).Text),
                     lblApplication,
                     General.GetNullableDateTime(dateofissue),
                     //General.GetNullableDateTime(((RadLabel)e.Item.FindControl("lblEffective")).Text),
                     lblmanufacturer,
                     lblmodel,
                     lblsdk,
                     lblosversion,
                     "",
                     lbldevice,
                     lbldeviceType,
                     lblregion,
                     lbllanguage,
                     isactive,
                     loginactive
                 );
                Rebind();
            }

            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                ViewState["DeviceID"] = ((RadLabel)e.Item.FindControl("lblDeviceId")).Text;
                RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Delete");
                return;
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
    protected void gvMobilePermissions_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
        if (e.Item.IsInEditMode)
        {



        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
    private void InsertDevice(string uuid, int userId, string appcode, DateTime? effecticeDate, string manufacturer, string model, string sdk, string osversion, string apikey, string device, string type, string region, string language, int? activeyn, string isLogged)
    {
        PhoenixGeneralSettings.InsertDevice(
                                uuid, appcode, userId, effecticeDate, manufacturer, model, sdk, osversion, apikey, device, type, region, language, activeyn, isLogged);
    }
    private void UpdateDevice(string uuid, int userId, string appcode, DateTime? effecticeDate, string manufacturer, string model, string sdk, string osversion, string apikey, string device, string type, string region, string language, int? activeyn, string isLogged)
    {
        PhoenixGeneralSettings.InsertDevice(
                                uuid, appcode, userId, effecticeDate, manufacturer, model, sdk, osversion, apikey, device, type, region, language, activeyn, isLogged);
        ucStatus.Text = "Device information updated";
    }

    private bool IsValidDevice(string device, string deviceType)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvMobilePermissions;


        if (device.Trim().Equals(""))
            ucError.ErrorMessage = "Device name is required.";

        if (deviceType.Trim().Equals(""))
            ucError.ErrorMessage = "Type is required.";

        return (!ucError.IsError);
    }

    private bool IsValidDevice(string device, string deviceType, string manufacturer, string region, string language)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (device == null)
            device = "";
        RadGrid _gridView = gvMobilePermissions;

        if (device.Trim().Equals(""))
            ucError.ErrorMessage = "Device name is required.";

        if (deviceType.Trim().Equals(""))
            ucError.ErrorMessage = "Type is required.";

        return (!ucError.IsError);
    }
    private void DeleteDevice(Guid deviceId)
    {
        PhoenixGeneralSettings.DeleteDevice(deviceId);
    }

    protected void setFilterText()
    {
        device = null;
        deviceType = null;
        language = null;
        region = null;
        manufacturer = null;
        username = null;
        if (!txtDevice.Text.Trim().Equals(""))
            device = txtDevice.Text;
        if (!txtDeviceType.Text.Trim().Equals(""))
            deviceType = txtDeviceType.Text;
        if (!txtLanguage.Text.Trim().Equals(""))
            language = txtLanguage.Text;
        if (!txtRegion.Text.Trim().Equals(""))
            region = txtRegion.Text;
        if (!txtManufacturer.Text.Trim().Equals(""))
            manufacturer = txtManufacturer.Text;
        if (!txtUsername.Text.Trim().Equals(""))
            username = txtUsername.Text;
        effective = General.GetNullableDateTime(txtEffective.Text);
    }
    protected void gvMobilePermissions_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvMobilePermissions_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMobilePermissions.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    //protected void MenuRemoteUser_TabStripCommand(object sender, EventArgs e)
    //{
    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    //    string usercode = (ViewState["USERCODE"] == null) ? "" : ViewState["USERCODE"].ToString();
    //    if (dce.CommandName.ToUpper().Equals("USERIDENTITY"))
    //    {
    //        Response.Redirect("../Options/OptionsUserIdentity.aspx?usercode=" + usercode);

    //    }
    //    else if (dce.CommandName.ToUpper().Equals("IDENTITYLOG"))
    //    {
    //        Response.Redirect("../Options/OptionsUserIdentityLog.aspx?usercode=" + usercode);
    //    }
    //    else
    //    {
    //        Response.Redirect("../Options/OptionsUserLoginAudit.aspx?usercode=" + usercode);
    //    }
    //}


    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            DeleteDevice(new Guid(ViewState["DeviceID"].ToString()));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
