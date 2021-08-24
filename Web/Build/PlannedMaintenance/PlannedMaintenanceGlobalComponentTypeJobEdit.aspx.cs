using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
public partial class PlannedMaintenanceGlobalComponentTypeJobEdit : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargeneral = new PhoenixToolbar();
            toolbargeneral.AddButton("Save", "SAVE", ToolBarDirection.Right);

            MenuPlannedMaintenance.AccessRights = this.ViewState;
            MenuPlannedMaintenance.MenuList = toolbargeneral.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("General", "GENERAL", ToolBarDirection.Left);
            toolbarmain.AddButton("History Template", "HISTORY", ToolBarDirection.Left);
            toolbarmain.AddButton("Include", "INCLUDE", ToolBarDirection.Left);
            toolbarmain.AddButton("Manuals", "MANUALS", ToolBarDirection.Left);
            //toolbarmain.AddButton("PTW", "PTW", ToolBarDirection.Left);

            MenuMain.AccessRights = this.ViewState;
            MenuMain.MenuList = toolbarmain.Show();
            MenuMain.TabStrip = true;


            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobParameter.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvManual')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobParameter.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceJobParameter.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuPMSManual.AccessRights = this.ViewState;
            MenuPMSManual.MenuList = toolbar.Show();


            if (!IsPostBack)
            {
               
                ViewState["GLOBALCOMPONENTTYPEMAPID"] = "";

                if (Request.QueryString["TABINDEX"] != null && General.GetNullableInteger(Request.QueryString["TABINDEX"].ToString())!=null)
                {
                    MenuMain.SelectedMenuIndex = int.Parse(Request.QueryString["TABINDEX"].ToString());
                    RadMultiPage1.SelectedIndex = int.Parse(Request.QueryString["TABINDEX"].ToString());
                }
                else
                {
                    MenuMain.SelectedMenuIndex = 0;
                    RadMultiPage1.SelectedIndex = 0;
                }

                



                if (Request.QueryString["GLOBALCOMPONENTTYPEMAPID"] != null && Request.QueryString["GLOBALCOMPONENTTYPEMAPID"].ToString() != "")
                {
                    ViewState["GLOBALCOMPONENTTYPEMAPID"] = Request.QueryString["GLOBALCOMPONENTTYPEMAPID"].ToString();
                }
                BindData();

                gvHistoryTemplateList.PageSize = General.ShowRecords(null);
                //gvManual.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            ucTypeJob.Model = hdnModel.Value;
            ucTypeJob.ComponentNumber = txtNumber.Text;
            ucRA.Vessel = "0";

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                RadMultiPage1.SelectedIndex = 0;
                MenuMain.SelectedMenuIndex = 0;
            }
            if (CommandName.ToUpper().Equals("HISTORY"))
            {
                RadMultiPage1.SelectedIndex = 1;
                MenuMain.SelectedMenuIndex = 1;
            }
            if (CommandName.ToUpper().Equals("INCLUDE"))
            {
                RadMultiPage1.SelectedIndex = 2;
                MenuMain.SelectedMenuIndex = 2;
            }
            if (CommandName.ToUpper().Equals("MANUALS"))
            {
                RadMultiPage1.SelectedIndex = 3;
                MenuMain.SelectedMenuIndex = 3;
            }
            if (CommandName.ToUpper().Equals("PTW"))
            {
                RadMultiPage1.SelectedIndex = 4;
                MenuMain.SelectedMenuIndex = 4;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PlannedMaintenance_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentTypeJobUpdate(new Guid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString())
                                                                                , chkActive.Checked == true ? 1 : 0
                                                                                , General.GetNullableInteger(txtFrequency.Text)
                                                                                , General.GetNullableInteger(ucFrequency.SelectedHard)
                                                                                , General.GetNullableInteger(txtCounter.Text)
                                                                                , General.GetNullableInteger(ucCounter.SelectedHard)
                                                                                , chkHistory.Checked == true ? 1 : 0
                                                                                , General.GetNullableInteger(ucResponsibility.SelectedDiscipline)
                                                                                , chkRA.Checked == true ? 1 : 0
                                                                                , General.GetNullableGuid(ucRA.SelectedValue)
                                                                                , General.GetNullableInteger(ucPlanningMethod.SelectedHard)
                                                                                , General.GetNullableInteger(txtWindow.Text)
                                                                                , General.GetNullableString(txtReference.Text)
                    );

                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += " fnReloadList('null','null','yes');";
                Script += "</script>" + "\n";
                //Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);
                RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "BookMarkScript", Script, false);
            }
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindData()
    {
        if (ViewState["GLOBALCOMPONENTTYPEMAPID"] != null && General.GetNullableGuid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString()) != null)
        {
            DataSet ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentTypeJobEdit(General.GetNullableGuid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtMake.Text = dr["FLDMAKE"].ToString();
                txtType.Text = dr["FLDTYPE"].ToString();
                txtNumber.Text= dr["FLDCOMPONENTNUMBER"].ToString();
                txtName.Text = dr["FLDCOMPONENTNAME"].ToString();
                txtCode.Text= dr["FLDJOBCODE"].ToString();
                txtICode.Text= dr["FLDJOBCODE"].ToString();
                txtTitle.Text = dr["FLDJOBTITLE"].ToString();
                txtITitle.Text= dr["FLDJOBTITLE"].ToString();

                txtPTWJobCode.Text = dr["FLDJOBCODE"].ToString();
                txtPTWTitle.Text = dr["FLDJOBTITLE"].ToString();

                if (General.GetNullableString(dr["FLDFREQUENCYNAME"].ToString()) == null && General.GetNullableString(dr["FLDCOUNTERFREQUENCYNAME"].ToString()) != null)
                    txtPTWFrequency.Text = dr["FLDCOUNTERFREQUENCYNAME"].ToString();
                else if (General.GetNullableString(dr["FLDCOUNTERFREQUENCYNAME"].ToString()) == null && General.GetNullableString(dr["FLDFREQUENCYNAME"].ToString()) != null)
                    txtPTWFrequency.Text = dr["FLDFREQUENCYNAME"].ToString();
                else if (General.GetNullableString(dr["FLDCOUNTERFREQUENCYNAME"].ToString()) != null && General.GetNullableString(dr["FLDFREQUENCYNAME"].ToString()) != null)
                    txtPTWFrequency.Text = dr["FLDFREQUENCYNAME"].ToString() + " / " + dr["FLDCOUNTERFREQUENCYNAME"].ToString();


                chkActive.Checked = General.GetNullableInteger(dr["FLDAPPLICABLEYN"].ToString()) == 1 ? true : false;
                txtFrequency.Text = dr["FLDFREQUENCY"].ToString();
                
                ucFrequency.SelectedHard = dr["FLDFREQUENCYTYPE"].ToString();
                txtCounter.Text = dr["FLDCOUNTERFREQUENCY"].ToString();
                ucCounter.SelectedHard = dr["FLDCOUNTERTYPE"].ToString();
                txtReference.Text = dr["FLDREFERENCE"].ToString();

                if (General.GetNullableString(dr["FLDFREQUENCYNAME"].ToString()) == null && General.GetNullableString(dr["FLDCOUNTERFREQUENCYNAME"].ToString()) != null)
                    txtIFrequency.Text = dr["FLDCOUNTERFREQUENCYNAME"].ToString();
                else if (General.GetNullableString(dr["FLDCOUNTERFREQUENCYNAME"].ToString()) == null && General.GetNullableString(dr["FLDFREQUENCYNAME"].ToString()) != null)
                    txtIFrequency.Text = dr["FLDFREQUENCYNAME"].ToString();
                else if (General.GetNullableString(dr["FLDCOUNTERFREQUENCYNAME"].ToString()) != null && General.GetNullableString(dr["FLDFREQUENCYNAME"].ToString()) != null)
                    txtIFrequency.Text = dr["FLDFREQUENCYNAME"].ToString() + " / " + dr["FLDCOUNTERFREQUENCYNAME"].ToString();


                ucResponsibility.SelectedDiscipline = dr["FLDRESPONSIBLE"].ToString();
                chkHistory.Checked = General.GetNullableInteger(dr["FLDHISTORYTEMPLATESYN"].ToString()) == 1 ? true : false;
                chkRA.Checked = General.GetNullableInteger(dr["FLDRISKASSESSMENTYN"].ToString()) == 1 ? true : false;
                ucPlanningMethod.SelectedHard = dr["FLDPLANNINGMETHOD"].ToString();
                txtWindow.Text = dr["FLDWINDOWPERIOD"].ToString();

                ucCategory.SelectedQuick = dr["FLDCATEGORY"].ToString();
                ucJobClass.SelectedQuick = dr["FLDJOBCLASS"].ToString();
                hdnModel.Value = dr["FLDMODELID"].ToString();

                ucRA.SelectedValue = dr["FLDRATEMPLATEID"].ToString();
                ucRA.Text = dr["FLDRATEMLATENUMBER"].ToString();

                
            }
        }
    }
    protected void gvHistoryTemplateList_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            string formid = ((RadTextBox)e.Item.FindControl("txtFormIdEdit")).Text;
            string Componentjobid = ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString();
            //if (Request.QueryString["GLOBALCOMPONENTTYPEMAPID"] != null)
            //{
            //    Componentjobid = Request.QueryString["GLOBALCOMPONENTTYPEMAPID"].ToString();
            //}
            RadCheckBox chk = (RadCheckBox)e.Item.FindControl("chkVerifiedEdit");
            string verifiedyn = chk.Checked == true ? "1" : "0";
            
            PhoenixPlannedMaintenanceGlobalComponent.GlobalHistoryTemplateUpdate(new Guid(formid)
                                                                              , byte.Parse(verifiedyn)
                                                                              , new Guid(Componentjobid));
        }
    }
    protected void gvHistoryTemplateList_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        int? sortdirection = null;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();
        if (General.GetNullableGuid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString()) != null)
        {
            ds = PhoenixPlannedMaintenanceGlobalComponent.GlobalComponentTypeJobHistoryTemplateSearch(txtTemplateName.Text
                                                                                         , new Guid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString())
                                                                                         , sortexpression
                                                                                         , sortdirection
                                                                                         , gvHistoryTemplateList.CurrentPageIndex + 1
                                                                                         , gvHistoryTemplateList.PageSize
                                                                                         , ref iRowCount, ref iTotalPageCount);
        }

        
        gvHistoryTemplateList.DataSource = ds;
        gvHistoryTemplateList.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvHistoryTemplateList_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

        }
    }

    protected void txtTemplateName_TextChanged(object sender, EventArgs e)
    {
        gvHistoryTemplateList.Rebind();
    }

    protected void gvIncludeJobs_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();
        if (General.GetNullableGuid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString()) != null)
        {
            ds = PhoenixPlannedMaintenanceGlobalComponent.IncludedJobsList(new Guid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString()));
        }


        gvIncludeJobs.DataSource = ds;
    }
    protected void gvIncludeJobs_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            UserControlMultiColumnComponentTypeJob ucTypeJobs = (UserControlMultiColumnComponentTypeJob)e.Item.FindControl("ucTypeJobs");
            if (ucTypeJobs != null)
            {
                ucTypeJobs.Model = hdnModel.Value;
                ucTypeJobs.ComponentNumber = txtNumber.Text;
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdDelete");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        }
    }
    protected void gvIncludeJobs_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            string Componentjobid = ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString();
            GridDataItem item = (GridDataItem)e.Item;
            PhoenixPlannedMaintenanceGlobalComponent.DeleteIncludedJob(new Guid(((RadLabel)item.FindControl("lblID")).Text));
        }
    }

    protected void cmdAdd_Click(object sender, EventArgs e)
    {
        if (General.GetNullableGuid(ucTypeJob.SelectedValue) == null)
        {
            ucError.ErrorMessage = "Job is required.";
            ucError.Visible = true;
            return;
        }
        PhoenixPlannedMaintenanceGlobalComponent.IncludedJob(new Guid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString()), new Guid(ucTypeJob.SelectedValue));
        gvIncludeJobs.Rebind();
        ucTypeJob.SelectedValue = "";
        ucTypeJob.Text = "";
    }
    protected void MenuPMSManual_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("CLEAR"))
            {

            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
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
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDMANUALNAME" };
        string[] alCaptions = { "Name" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixPlannedMaintenanceComponentJobManual.Search(0
                                         , General.GetNullableGuid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString())
                                         , sortexpression, sortdirection
                                         , 1
                                         , iRowCount
                                         , ref iRowCount
                                         , ref iTotalPageCount);

        General.ShowExcel("Manual", dt, alColumns, alCaptions, null, null);
    }
    protected void gvManual_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper() == "PAGE")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper() == "ADD")
            {
                RadTextBox filename = (RadTextBox)e.Item.FindControl("fileName");
                if (!IsValidManual(filename.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                string file = filename.Text.Replace("PMSManuals/", "/");
                FileInfo fi = new FileInfo(PhoenixGeneralSettings.CurrentGeneralSetting.PMSManualsPath + file);
                PhoenixPlannedMaintenanceComponentJobManual.Insert(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                        , new Guid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString())
                                        , file
                                        , fi.Name);
                gvManual.Rebind();
            }
            else if (e.CommandName.ToUpper() == "DELETE")
            {
                GridDataItem item = (GridDataItem)e.Item;
                //DataRowView drv = (DataRowView)item.DataItem;

                string id = item.GetDataKeyValue("FLDMANUALID").ToString();
                PhoenixPlannedMaintenanceComponentJobManual.Delete(PhoenixSecurityContext.CurrentSecurityContext.VesselID, new Guid(id));
                gvManual.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvManual_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvManual_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDMANUALNAME" };
            string[] alCaptions = { "Name" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixPlannedMaintenanceComponentJobManual.Search(0
                                         , General.GetNullableGuid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString())
                                         , sortexpression, sortdirection
                                         , gvManual.CurrentPageIndex + 1
                                         , gvManual.PageSize
                                         , ref iRowCount
                                         , ref iTotalPageCount
                                         , 1);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvManual", "Manual", alCaptions, alColumns, ds);

            gvManual.DataSource = dt;
            gvManual.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvManual_SortCommand(object sender, GridSortCommandEventArgs e)
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
    private bool IsValidManual(string manual)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (manual.Trim().Equals(""))
            ucError.ErrorMessage = "Manual is required.";

        return (!ucError.IsError);
    }
    protected void gvPTW_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            if (General.GetNullableGuid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString()) != null)
            {
                ds = PhoenixPlannedMaintenanceGlobalComponent.TypeJobPTWMapList(new Guid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString()));
            }

            gvPTW.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }
    protected void gvPTW_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdDelete");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        }
    }
    protected void gvPTW_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string Componentjobid = ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString();
                GridDataItem item = (GridDataItem)e.Item;
                if (General.GetNullableGuid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString()) != null)
                    PhoenixPlannedMaintenanceGlobalComponent.TypeJobPTWUnMap(new Guid(((RadLabel)item.FindControl("lblPTWId")).Text), new Guid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString()));
            }
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string Componentjobid = ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString();
                GridFooterItem item = (GridFooterItem)e.Item;
                if (General.GetNullableGuid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString()) != null && General.GetNullableGuid(((UserControlMultiColumnPMSPTWA)item.FindControl("ucPTW")).SelectedValue) != null)
                    PhoenixPlannedMaintenanceGlobalComponent.TypeJobPTWMap(new Guid(((UserControlMultiColumnPMSPTWA)item.FindControl("ucPTW")).SelectedValue), new Guid(ViewState["GLOBALCOMPONENTTYPEMAPID"].ToString()));

                gvPTW.Rebind();
            }
        }
        catch(Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        
    }
}
