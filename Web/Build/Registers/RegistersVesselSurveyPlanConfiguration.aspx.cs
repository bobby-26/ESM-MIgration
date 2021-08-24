using System;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Data;
using Telerik.Web.UI;

public partial class Registers_RegistersVesselSurveyPlanConfiguration : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersVesselSurveyPlanConfiguration.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvSurveyCycles')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersSurveyCycles.AccessRights = this.ViewState;
            MenuRegistersSurveyCycles.MenuList = toolbar.Show();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("List", "BACK",ToolBarDirection.Right);
            MainMenuRegistersSurveyCycles.AccessRights = this.ViewState;
            MainMenuRegistersSurveyCycles.MenuList = toolbarmain.Show();
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar sctoolbar = new PhoenixToolbar();
            sctoolbar.AddFontAwesomeButton("../Registers/RegistersVesselSurveyPlanConfiguration.aspx", "Create", "<i class=\"fas fa-plus-circle\"></i>", "CREATE");
            sctoolbar.AddFontAwesomeButton("../Registers/RegistersVesselSurveyPlanConfiguration.aspx", "Delete", "<i class=\"fas fa-trash-alt\"></i>", "DELETE");
            MenuSCExcel.AccessRights = this.ViewState;
            MenuSCExcel.MenuList = sctoolbar.Show();

            if (!IsPostBack)
            {
                gvSurveyCycles.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["TemplateId"] = string.IsNullOrEmpty(Request.QueryString["TemplateId"]) ? "" : Request.QueryString["TemplateId"];
                ViewState["PAGENUMBER"] = 1;
                string templateid = ViewState["TemplateId"].ToString();
                DataSet ds;
                ds = PhoenixRegistersVesselSurveyPlanConfiguration.ListSurveyChangeSequence(General.GetNullableGuid(templateid));
                if (ds.Tables[0].Rows.Count > 1)
                {
                   ddlSurveyChange.Items.Add(new RadComboBoxItem("--Select--", "0"));
                }

                ddlSurveyChange.DataSource = ds;
                ddlSurveyChange.DataTextField = "FLDSEQUENCE";
                ddlSurveyChange.DataValueField = "FLDSEQUENCE";
                ddlSurveyChange.DataBind();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSCExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("CREATE"))
            {
                string sequenceno = ddlSurveyChange.SelectedValue;

                if (!IsValidChange(sequenceno))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixRegistersVesselSurveyPlanConfiguration.InsertSurveyChangeCycle(PhoenixSecurityContext.CurrentSecurityContext.UserCode, General.GetNullableGuid(ViewState["TemplateId"].ToString()), General.GetNullableInteger(sequenceno));
                BindDataSurveyChange();
                gvSurveyChange.Rebind();
                ddlSurveyChange.Enabled = false;
            }
            if (CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersVesselSurveyPlanConfiguration.DeleteSurveyChangeCycle(General.GetNullableGuid(ViewState["TemplateId"].ToString()));
                BindDataSurveyChange();
                gvSurveyChange.Rebind();
                ddlSurveyChange.Enabled = true;
                ddlSurveyChange.SelectedIndex = 0;

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
        if (!string.IsNullOrEmpty(ViewState["TemplateId"].ToString()))
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = new DataSet();

            string[] alColumns = { "FLDSURVEYTYPENAME", "FLDSEQUENCE", "FLDNEXTDUE", "FLDNEXTSURVEYTYPENAME", "FLDNEXTSEQUENCE" };
            string[] alCaptions = { "Survey Type", "Sequence", "Months to add for Next Due", "Next Survey Type", "Next Sequence" };

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            ds = PhoenixRegistersVesselSurveyPlanConfiguration.SurveyCycleSearch(new Guid(ViewState["TemplateId"].ToString()),
                1,
                iRowCount,
                ref iRowCount,
                ref iTotalPageCount);

            General.ShowExcel("Validity Cycle", ds.Tables[0], alColumns, alCaptions, null, "");
        }
    }

    protected void MenuRegistersSurveyCycles_TabStripCommand(object sender, EventArgs e)
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MainMenuRegistersSurveyCycles_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("../Registers/RegistersVesselSurveyTemplate.aspx");
        }
    }
    private void BindData()
    {
        if (!string.IsNullOrEmpty(ViewState["TemplateId"].ToString()))
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDSURVEYTYPENAME", "FLDSEQUENCE", "FLDNEXTDUE", "FLDNEXTSURVEYTYPENAME", "FLDNEXTSEQUENCE" };
            string[] alCaptions = { "Survey Type", "Sequence", "Months to add for Next Due", "Next Survey Type", "Next Sequence" };

            DataSet ds = PhoenixRegistersVesselSurveyPlanConfiguration.SurveyCycleSearch(new Guid(ViewState["TemplateId"].ToString()),
                (int)ViewState["PAGENUMBER"],
                gvSurveyCycles.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

            General.SetPrintOptions("gvSurveyCycles", "Validity Cycle", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtTemplate.Text = ds.Tables[0].Rows[0]["FLDTEMPLATENAME"].ToString();
            }
            gvSurveyCycles.DataSource = ds;
            gvSurveyCycles.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;


        }
    }

    private void BindDataSurveyChange()
    {
        if (!string.IsNullOrEmpty(ViewState["TemplateId"].ToString()))
        {
            string templateid = ViewState["TemplateId"].ToString();

            DataSet ds = PhoenixRegistersVesselSurveyPlanConfiguration.SurveyChangeCycleSearch(General.GetNullableGuid(templateid));
            gvSurveyChange.DataSource = ds;
            
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlSurveyChange.Enabled = false;
            }

        }

    }

    private bool IsValidSurveyChange(string Frequency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(Frequency) == null)
            ucError.ErrorMessage = "'Months to add for Next Due Date' is required.";

        if (General.GetNullableInteger(Frequency) == 0)
            ucError.ErrorMessage = "Please enter valid 'Months to add for Next Due Date'.";

        return (!ucError.IsError);

    }

    private bool IsValidChange(string sequence)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if(sequence.Equals("0"))
            ucError.ErrorMessage = "Please select Survey Change";

        return (!ucError.IsError);
    }

    private void InsertSurveyTemplate(Guid? TemplateId, int? SurveyTypeId, int? Sequence, int? Frequency, int? NextSurveyTypeId, int? NextSequence)
    {
        PhoenixRegistersVesselSurveyPlanConfiguration.InsertSurveyCycle(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
             , TemplateId
             , SurveyTypeId
             , Sequence
             , Frequency
             , NextSurveyTypeId, NextSequence);
    }

    private void UpdateSurveyTemplate(Guid ConfigurationId, int? SurveyTypeId, int? Sequence, int? Frequency, int? NextSurveyTypeId, Guid? TemplateId, int? NextSequence)
    {
        PhoenixRegistersVesselSurveyPlanConfiguration.UpdateSurveyCycle(ConfigurationId
            ,PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , TemplateId
            , SurveyTypeId
            , Sequence
            , Frequency
            , NextSurveyTypeId, NextSequence);
    }

    private void UpdateSurveyChange(Guid? Templateid,Guid? Sequenceid, int? Sequence, int? Frequency)
    {
        PhoenixRegistersVesselSurveyPlanConfiguration.UpdateSurveyChangeCycle(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Templateid, Sequenceid, Sequence,Frequency);
    }

    private void DeleteSurveyTemplate(Guid? ConfigurationId)
    {
        PhoenixRegistersVesselSurveyPlanConfiguration.DeleteSurveyCycle(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ConfigurationId);
    }

    private bool IsValidSurveyTemplate(string SurveyTypeId, string Sequence, string Frequency,string NextSurveyTypeId, string NextSequence)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (SurveyTypeId == "")
            ucError.ErrorMessage = "Survey type is required.";

        if (General.GetNullableInteger(Sequence) == null)
            ucError.ErrorMessage = "Sequence is required.";

        if (General.GetNullableInteger(Sequence) == 0)
            ucError.ErrorMessage = "Please enter valid Sequence.";

        if (General.GetNullableInteger(Frequency) == null)
            ucError.ErrorMessage = "'Months to add for Next Due Date' is required.";

        if (General.GetNullableInteger(Frequency) == 0)
            ucError.ErrorMessage = "Please enter valid 'Months to add for Next Due Date'.";

        if (NextSurveyTypeId == "")
            ucError.ErrorMessage = "Next survey type is required.";

        if (General.GetNullableInteger(NextSequence) == null)
            ucError.ErrorMessage = "Next Sequence is required.";

        return (!ucError.IsError);
    }

    protected void gvSurveyCycles_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSurveyCycles.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvSurveyCycles_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            }
        }
        if (e.Item is GridEditableItem)
        {
            RadComboBox ddlSurveyTypeEdit = (RadComboBox)e.Item.FindControl("ddlSurveyTypeEdit");
            RadLabel lblSurveyTypeId = (RadLabel)e.Item.FindControl("lblSurveyTypeId");
            RadComboBox ddlNextSurveyTypeEdit = (RadComboBox)e.Item.FindControl("ddlNextSurveyTypeEdit");
            RadLabel lblNextSurveyTypeId = (RadLabel)e.Item.FindControl("lblNextSurveyTypeId");

            if (ddlSurveyTypeEdit != null && lblSurveyTypeId != null)
                ddlSurveyTypeEdit.SelectedValue = lblSurveyTypeId.Text.Trim();
            if (ddlNextSurveyTypeEdit != null && lblNextSurveyTypeId != null)
                ddlNextSurveyTypeEdit.SelectedValue = lblNextSurveyTypeId.Text.Trim();
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

    protected void gvSurveyCycles_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                GridFooterItem footerItem = (GridFooterItem)gvSurveyCycles.MasterTableView.GetItems(GridItemType.Footer)[0];
                string type = ((RadComboBox)footerItem.FindControl("ddlSurveyTypeAdd")).SelectedValue.Trim();
                string sequence = ((UserControlMaskNumber)footerItem.FindControl("ucSequenceAdd")).Text.Trim();
                string nextdue = ((UserControlMaskNumber)footerItem.FindControl("ucNextDueAdd")).Text;
                string nexttype = ((RadComboBox)footerItem.FindControl("ddlNextSurveyTypeAdd")).SelectedValue.Trim();
                string nextsequence = ((UserControlMaskNumber)footerItem.FindControl("ucNextSequenceAdd")).Text.Trim();
                if (!IsValidSurveyTemplate(type, sequence, nextdue, nexttype, nextsequence))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertSurveyTemplate(General.GetNullableGuid(ViewState["TemplateId"].ToString())
                        , General.GetNullableInteger(type)
                        , General.GetNullableInteger(sequence)
                        , General.GetNullableInteger(nextdue)
                        , General.GetNullableInteger(nexttype)
                        , General.GetNullableInteger(nextsequence)
                );
                ucStatus.Text = "Template added sucessfully";
                ucStatus.Visible = true;
                BindData();
                gvSurveyCycles.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteSurveyTemplate(General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblConfigurationId")).Text));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSurveyCycles_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            string type = ((RadComboBox)e.Item.FindControl("ddlSurveyTypeEdit")).SelectedValue;
            string sequence = ((UserControlMaskNumber)e.Item.FindControl("ucSequenceEdit")).Text.Trim();
            string nextdue = ((UserControlMaskNumber)e.Item.FindControl("ucNextDueEdit")).Text;
            string nexttype = ((RadComboBox)e.Item.FindControl("ddlNextSurveyTypeEdit")).SelectedValue;
            string nextsequence = ((UserControlMaskNumber)e.Item.FindControl("ucNextSequenceEdit")).Text.Trim();

            if (!IsValidSurveyTemplate(type, sequence, nextdue, nexttype, nextsequence))
            {
                ucError.Visible = true;
                return;
            }

            UpdateSurveyTemplate(new Guid(((RadLabel)e.Item.FindControl("lblConfigurationId")).Text.Trim())
                        , General.GetNullableInteger(type)
                        , General.GetNullableInteger(sequence)
                        , General.GetNullableInteger(nextdue)
                        , General.GetNullableInteger(nexttype)
                        , General.GetNullableGuid(ViewState["TemplateId"].ToString())
                        , General.GetNullableInteger(nextsequence)
                 );
            gvSurveyCycles.SelectedIndexes.Clear();
            BindData();
            gvSurveyCycles.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSurveyCycles_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
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

    protected void gvSurveyChange_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataSurveyChange();
    }

    protected void gvSurveyChange_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }
    }
    protected void gvSurveyChange_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string sequenceid = ((RadLabel)e.Item.FindControl("lblSurveyChangeSequenceId")).Text;
            string sequenceno = ((RadLabel)e.Item.FindControl("lblSurveyChangeSequenceNo")).Text;
            string nextdue = ((UserControlMaskNumber)e.Item.FindControl("ucSurveyChangeNextDueEdit")).Text;

            if (!IsValidSurveyChange(nextdue))
            {
                ucError.Visible = true;
                return;
            }

            UpdateSurveyChange(General.GetNullableGuid(ViewState["TemplateId"].ToString()), General.GetNullableGuid(sequenceid), General.GetNullableInteger(sequenceno), General.GetNullableInteger(nextdue));

            gvSurveyChange.SelectedIndexes.Clear();
            BindDataSurveyChange();
            gvSurveyChange.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSurveyChange_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
