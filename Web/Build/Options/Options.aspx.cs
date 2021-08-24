using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Owners;

public partial class Options : PhoenixBasePage
{
    int usercode;
    int accessid;

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (Request.QueryString["usercode"] != null)
            toolbar.AddButton("Cancel", "CANCEL", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);       
        PhoenixOptionsForm.MenuList = toolbar.Show();

        usercode = (Request.QueryString["usercode"] != null) ? int.Parse(Request.QueryString["usercode"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        accessid = (Request.QueryString["accessid"] != null) ? int.Parse(Request.QueryString["accessid"].ToString()) : PhoenixSecurityContext.CurrentSecurityContext.AccessId;

        if (!IsPostBack)
        {
            // btnMeasureconfig.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','../Dashboard/DashboardMeasureList.aspx?usercode=" + usercode.ToString() + "');return false;");
            // btnModuleConfig.Attributes.Add("onclick", "javascript:Openpopup('codehelp1','','../Dashboard/DashboardModuleList.aspx?usercode=" + usercode.ToString() + "');return false;");
            //  btnMeasureconfig.Attributes.Add("onclick", "openNewWindow('Measure', '','" + Session["sitepath"] + "/Dashboard/DashboardMeasureList.aspx?usercode=" + usercode.ToString() + "'); return false;");
            // btnModuleConfig.Attributes.Add("onclick", "openNewWindow('Module', '','" + Session["sitepath"] + "/Dashboard/DashboardModuleList.aspx?usercode=" + usercode.ToString() + "'); return false;");
            BindOwnerReportSection();
            DataSet ds1 = PhoenixGeneralSettings.GetUserOptions(usercode);
            GeneralSetting gs = PhoenixGeneralSettings.CurrentGeneralSetting;
            DataSet ds = SessionUtil.ApplicationList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
            DataSet dsAccessList = SessionUtil.Access2AlertList(accessid);
            DataTable dt = new DataTable();
            if(ds.Tables.Count > 0)
            {
                dt = ds1.Tables[0];
            }
            try
            {
                ddlShow.SelectedValue = gs.Records;
                //ddlTheme.SelectedValue = gs.Theme;
                ucCompany.SelectedCompany = gs.CompanyID;
                ucVessel.SelectedVessel = gs.VesselID;
                ddlDateFormat.SelectedValue = gs.UserCulture + ":" + gs.ShortDateFormat;
                chkShowCreditNote.Checked = (gs.ShowCreditNoteDiscount == 1) ? true : false;
                chkShowSupdtFeedback.Checked = (gs.ShowSupdtFeedback == 1) ? true : false;
                ChkShowComponentNumber.Checked = (gs.ShowComponentNumber == 1) ? true : false;
                chkDatePicker.Checked = (dt.Rows.Count > 0 && dt.Rows[0]["FLDDATEPICKERYN"].ToString().Equals("1") ? true : false);
                RadSkinManager1.Skin = Session["skin"].ToString();
                ddlDashboard.SelectedValue = (dt.Rows.Count > 0 ? dt.Rows[0]["FLDDASHBOARDNEW"].ToString() : "");
                ucUserGroup.SelectedUserGroup = (dt.Rows.Count > 0 ? dt.Rows[0]["FLDDASHBOARDUSERGROUP"].ToString() : "");
                ViewState["DASHBOARDUSERGROUP"] = (dt.Rows.Count > 0 ? dt.Rows[0]["FLDDASHBOARDUSERGROUP"].ToString() : "");
                UCOwnerReportUserGroup.SelectedUserGroup = (dt.Rows.Count > 0 ? dt.Rows[0]["FLDOWNERREPORTUSERGROUP"].ToString() : "");
                ViewState["OWNERREPORTUSERGROUP"] = (dt.Rows.Count > 0 ? dt.Rows[0]["FLDOWNERREPORTUSERGROUP"].ToString() : "");
                ddlCountry.SelectedCountry = gs.Nationality.ToString();
                ddlownerreporthomepage.SelectedValue = (dt.Rows.Count > 0 ? dt.Rows[0]["FLDOWNERREPORTDEFAULTSECTIONID"].ToString() : "");
                cblApplication.DataSource = ds;
                cblApplication.DataBind();
                txtExcelRecords.Text = gs.ExcelRecordsCount.ToString();
                string[] modules = gs.Modules.Split(',');
                foreach (string s in modules)
                {
                    foreach (ListItem li in cblApplication.Items)
                    {
                        if (li.Value.ToUpper().Equals(s.ToUpper()))
                            li.Selected = true;
                    }
                }

                //cblAlerts.DataSource = dsAccessList;
                //cblAlerts.DataBind();

                //string[] alerts = gs.AlertList.Split(',');
                //foreach (string s in alerts)
                //{
                //    foreach (ListItem li in cblAlerts.Items)
                //    {
                //        if (li.Value.Equals(s))
                //            li.Selected = true;
                //    }
                //}
                if (PhoenixSecurityContext.CurrentSecurityContext.UserCode == 1)
                {
                    lblShowCreditNoteDiscount.Visible = true;
                    chkShowCreditNote.Visible = true;
                    lblShowSuptFeedback.Visible = true;
                    chkShowSupdtFeedback.Visible = true;

                }
                else
                {
                    lblShowCreditNoteDiscount.Visible = false;
                    chkShowCreditNote.Visible = false;

                    //btnMeasureconfig.Visible = false;
                    //lblDashboardMeasure.Visible = false;
                    //btnModuleConfig.Visible = false;
                    //lblDashboardModuleList.Visible = false;

                    ucUserGroup.Visible = false;
                    lblDashboardUserGroup.Visible = false;
                    lblpreference.Visible = false;
                    rblpreference.Visible = false;
                    lblOwnerReportUserGroup.Visible = false;
                    UCOwnerReportUserGroup.Visible = false;
                }
                if(ds1.Tables.Count > 1)
                {
                    DataRow[] dataRows = ds1.Tables[1].Select("FLDUSERCODE = " + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString());
                    if (dataRows.Length > 0)
                    {
                        lblDashboardUserGroup.Visible = true;
                        ucUserGroup.Visible = true;

                        lblOwnerReportUserGroup.Visible = true;
                        UCOwnerReportUserGroup.Visible = true;
                    }
                }
                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    lblShowSuptFeedback.Visible = false;
                    chkShowSupdtFeedback.Visible = false;
                }
                DashboardPreferenceSearch();
                SOAConfigurationEdit();
            }
            catch (Exception ex)
            {
                ucError.Visible = true;
                ucError.ErrorMessage = ex.Message;                            
            }
        }

        //if (!SessionUtil.CanAccess(this.ViewState, btnMeasureconfig.CommandName.ToUpper()))
        //{
        //	btnMeasureconfig.Visible = false;
        //	lblDashboardMeasure.Visible = false;
        //}
        //if (!SessionUtil.CanAccess(this.ViewState, btnModuleConfig.CommandName.ToUpper()))
        //{
        //	btnModuleConfig.Visible = false;
        //	lblDashboardModuleList.Visible = false;
        //}
    }

    private void BindOwnerReportSection()
    {
        ddlownerreporthomepage.DataTextField = "FLDORSECTIONNAME";
        ddlownerreporthomepage.DataValueField = "FLDORSECTIONID";
        ddlownerreporthomepage.DataSource = PhoenixOwnerReport.ListSection();
        ddlownerreporthomepage.Items.Insert(0, new RadComboBoxItem("Select", "Dummy"));
        ddlownerreporthomepage.DataBind();
    }

    protected void PhoenixOptionsForm_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("CANCEL"))
        {
            Response.Redirect("OptionsUser.aspx?usercode=" + usercode.ToString(), false);
        }
        if (CommandName.ToUpper().Equals("SAVE"))
        {

            

            string modules = "";
            foreach (ListItem li in cblApplication.Items)
            {
                if (li.Selected)
                    modules = modules + "," + li.Value;
            }

            string alertlist = "";
            //foreach (ListItem li in cblAlerts.Items)
            //{
            //    if (li.Selected)
            //        alertlist = alertlist + "," + li.Value;
            //}
                

            short showcreditnote = (short)((chkShowCreditNote.Checked) ? 1 : 0);
            short showsupdtfeedback = (short)((chkShowSupdtFeedback.Checked) ? 1 : 0);
            short showcomponentnumber = (short)((ChkShowComponentNumber.Checked) ? 1 : 0);


            try
            {
                PhoenixGeneralSettings.SaveUserOptions(usercode,
                    "Theme1",
                    int.Parse(ddlShow.SelectedValue),
                    General.GetNullableInteger(ucCompany.SelectedCompany),
                    General.GetNullableInteger(ucVessel.SelectedVessel),
                    null, alertlist, showcreditnote
                    , General.GetNullableString(ddlDateFormat.SelectedValue != string.Empty ? ddlDateFormat.SelectedValue.Split(':')[0] : string.Empty)
                    , General.GetNullableString(ddlDateFormat.SelectedValue != string.Empty ? ddlDateFormat.SelectedValue.Split(':')[1] : string.Empty)
                    , General.GetNullableByte(chkDatePicker.Checked ? "1" : "0")
                    , showsupdtfeedback
                    , General.GetNullableInteger(ddlCountry.SelectedCountry)
                    , General.GetNullableInteger(txtExcelRecords.Text)
                    , showcomponentnumber
                    , RadSkinManager1.Skin
                    , modules
                    , General.GetNullableString(ddlDashboard.SelectedValue)
                    , General.GetNullableInteger(ucUserGroup.SelectedUserGroup.Equals("") ? ViewState["DASHBOARDUSERGROUP"].ToString() : ucUserGroup.SelectedUserGroup)
                    , General.GetNullableInteger(UCOwnerReportUserGroup.SelectedUserGroup.Equals("") ? ViewState["OWNERREPORTUSERGROUP"].ToString() : UCOwnerReportUserGroup.SelectedUserGroup)
                    , General.GetNullableGuid(ddlownerreporthomepage.SelectedValue)
                    );


                DashboardPreferenceUpdate();
                SOAConfigurationUpdate();
                ucStatus.Text = "Saved Successfully";
            }
            catch (Exception ex)
            {
                ucError.Text = ex.Message;
                ucError.Visible = true;
            }                      
        }
    }
    protected void DashboardPreferenceUpdate()
    {
        int preferenceCode;
        try
        {
            preferenceCode = int.Parse(rblpreference.SelectedValue);
            {
                PhoenixGeneralSettings.DashboardPreferenceUpdate(usercode, preferenceCode);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SOAConfigurationUpdate()
    {
        int soaflag;
        try
        {
            soaflag = int.Parse(rblSOAflag.SelectedValue);
            {
                PhoenixGeneralSettings.SOAConfigurationUpdate(soaflag);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void DashboardPreferenceSearch()
    {
        DataTable dt;
        string preferenceCode;
        dt = PhoenixGeneralSettings.DashboardPreferenceSearch(usercode);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            if (dr["FLDPREFERENCECODE"].ToString() != "")
            {
                preferenceCode = dr["FLDPREFERENCECODE"].ToString();
                rblpreference.SelectedValue = preferenceCode;
            }
        }

    }

    protected void SOAConfigurationEdit()
    {
        DataTable dt;
        string soaflag;
        dt = PhoenixGeneralSettings.SOAConfigurationEdit(usercode);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            if (dr["FLDPDFMERGER"].ToString() != "")
            {
                soaflag = dr["FLDPDFMERGER"].ToString();
                rblSOAflag.SelectedValue = soaflag;
            }
        }
    }
    protected void RadSkinManager1_SkinChanged(object sender, SkinChangedEventArgs e)
    {
        Session["skin"] = e.Skin;
        RadScriptManager.RegisterStartupScript(Page, this.Page.GetType(), "refresh", "parent.document.getElementById('cmdHiddenSubmit').click();", true);
    }
}
