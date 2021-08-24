using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRAProcessMultiple : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            BindData();
            ViewState["RISKASSESSMENTPROCESSID"] = null;
            Filter.CurrentMultipleRASelection = null;
            ViewState["STATUS"] = null;
            ViewState["VESSELID"] = "";

            if (Request.QueryString["processid"] != null)
            {
                ViewState["RISKASSESSMENTPROCESSID"] = Request.QueryString["processid"].ToString();
                RiskAssessmentProcessEdit();
            }
            if (Request.QueryString["processmultipleid"] != null && Request.QueryString["processmultipleid"].ToString() != string.Empty)
            {
                Filter.CurrentMultipleRASelection = Request.QueryString["processmultipleid"].ToString();
                RiskAssessmentProcessMultipleEdit();
            }

        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
        MenuProcess.AccessRights = this.ViewState;
        if ((ViewState["STATUS"] != null && ViewState["STATUS"].ToString() != "3") || ViewState["STATUS"] == null)
            MenuProcess.MenuList = toolbar.Show();
        // MenuProcess.SetTrigger(pnlProcess);
       // BindGrid();
    }
    protected void ucHazardType_TextChangedEvent(object sender, EventArgs e)
    {
        gvHealthSafetyRisk.Rebind();
        GridFooterItem gvHealthSafetyRiskfooteritem = (GridFooterItem)gvHealthSafetyRisk.MasterTableView.GetItems(GridItemType.Footer)[0];
        RadComboBox ddlHealthSubHazardType = (RadComboBox)gvHealthSafetyRiskfooteritem.FindControl("ddlSubHazardType");
        ddlHealthSubHazardType.DataTextField = "FLDNAME";
        ddlHealthSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddlHealthSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucHazardType.SelectedHazardType));
        ddlHealthSubHazardType.DataBind();
    }
    protected void ucEnvHazardType_OnTextChangedEvent(object sender, EventArgs e)
    {
        gvEnvironmentalRisk.Rebind();
        GridFooterItem gvEnvironmentalRiskfooteritem = (GridFooterItem)gvEnvironmentalRisk.MasterTableView.GetItems(GridItemType.Footer)[0];
        RadComboBox ddllEnvSubHazardType = (RadComboBox)gvEnvironmentalRiskfooteritem.FindControl("ddlSubHazardType");
        ddllEnvSubHazardType.DataTextField = "FLDNAME";
        ddllEnvSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddllEnvSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucEnvHazardType.SelectedHazardType));
        ddllEnvSubHazardType.DataBind();
    }

    protected void ucEconomicHazardType_OnTextChangedEvent(object sender, EventArgs e)
    {
        gvEconomicRisk.Rebind();
        GridFooterItem gvEconomicRiskfooteritem = (GridFooterItem)gvEconomicRisk.MasterTableView.GetItems(GridItemType.Footer)[0];
        RadComboBox ddlEcoSubHazardType = (RadComboBox)gvEconomicRiskfooteritem.FindControl("ddlSubHazardType");
        ddlEcoSubHazardType.DataTextField = "FLDNAME";
        ddlEcoSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddlEcoSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucEconomicHazardType.SelectedHazardType));
        ddlEcoSubHazardType.DataBind();
    }
    //protected void BindGrid()
    //{
    //    //BindGridEconomicRisk();
    //    // BindGridEnvironmentalRisk();
    //    //BindGridHealthSafetyRisk();
    //}

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        RiskAssessmentProcessEdit();
        RiskAssessmentProcessMultipleEdit();
    }

    private void RiskAssessmentProcessEdit()
    {
        DataSet dsProcess = PhoenixInspectionRiskAssessmentProcess.EditInspectionRiskAssessmentProcess(
            (ViewState["RISKASSESSMENTPROCESSID"] == null ? null : General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString())));

        if (dsProcess.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsProcess.Tables[0].Rows[0];
            txtCategory.Text = dr["FLDCATEGORYNAME"].ToString();
            txtProcess.Text = dr["FLDJOBACTIVITY"].ToString();
            ViewState["STATUS"] = dr["FLDSTATUS"].ToString();
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
        }
    }

    private void RiskAssessmentProcessMultipleEdit()
    {
        DataSet dsProcess = PhoenixInspectionRiskAssessmentProcess.EditInspectionRAProcessMultiple(
            General.GetNullableGuid(Filter.CurrentMultipleRASelection.ToString()));

        if (dsProcess.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsProcess.Tables[0].Rows[0];
            BindCheckBoxList(cblOtherRisk, dr["FLDOTHERRISK"].ToString());
            txtOtherRisk.Content = dr["FLDOTHERRISKPROPOSEDCONTROL"].ToString();
            //txtSequenceNumber.Text = dr["FLDSERIALNUMBER"].ToString();
            txtActivityCondition.Text = dr["FLDACTIVITYCONDITION"].ToString();

            //if (rblOtherRiskControl.Items.Equals((dr["FLDRISKCONTROL"].ToString()) != null))
            //    //rblOtherRiskControl.Items.FindByValue(dr["FLDRISKCONTROL"].ToString()).Selected = true;
            //    rblOtherRiskControl.SelectedValue = dr["FLDOTHERRISKCONTROL"].ToString();

            if (rblOtherRiskControl.SelectedValue != null)
            {
                rblOtherRiskControl.SelectedValue = dr["FLDRISKCONTROL"].ToString();
            }

            ucCompetencyLevel.SelectedQuick = dr["FLDCOMPETENCYLEVEL"].ToString();
            txtRiskAspects.Content = dr["FLDRISKASPECTS"].ToString();
        }
    }

    private void BindData()
    {
        cblOtherRisk.DataBindings.DataTextField = "FLDNAME";
        cblOtherRisk.DataBindings.DataValueField = "FLDHAZARDID";
        cblOtherRisk.DataSource = PhoenixInspectionRiskAssessmentHazard.ListRiskAssessmentHazard(3, 0);
        cblOtherRisk.DataBind();

        rblOtherRiskControl.DataBindings.DataTextField = "FLDNAME";
        rblOtherRiskControl.DataBindings.DataValueField = "FLDFREQUENCYID";
        rblOtherRiskControl.DataSource = PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4);
        rblOtherRiskControl.DataBind();
    }

    protected void MenuProcess_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (ViewState["RISKASSESSMENTPROCESSID"] == null || ViewState["RISKASSESSMENTPROCESSID"].ToString() == string.Empty)
            {
                ucError.ErrorMessage = "Please go back and save the template and then try adding multiple RA.";
                ucError.Visible = true;
                return;
            }
            SaveRiskAssessmentProcessMultiple();
        }
        else if (CommandName.ToUpper().Equals("NEW"))
        {
            txtOtherRisk.Content = "";
            txtActivityCondition.Text = "";
            ucCompetencyLevel.SelectedQuick = "";
            txtRiskAspects.Content = "";
            //txtSequenceNumber.Text = "";
            Filter.CurrentMultipleRASelection = null;
            BindData();
            //BindGrid();
        }
    }

    private void SaveRiskAssessmentProcessMultiple()
    {
        try
        {
            string otherrisk = ReadCheckBoxList(cblOtherRisk);
            string otherriskproposed = HttpUtility.HtmlDecode(txtOtherRisk.Content);

            if (!IsValidProcessTemplate())
            {
                ucError.Visible = true;
                return;
            }
            Guid? riskassessmentprocessmultipleidout = null;
            if (Filter.CurrentMultipleRASelection != null && Filter.CurrentMultipleRASelection.ToString() != string.Empty)
                riskassessmentprocessmultipleidout = new Guid(Filter.CurrentMultipleRASelection.ToString());

            PhoenixInspectionRiskAssessmentProcess.InsertInspectionRAProcessMultiple(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                ref riskassessmentprocessmultipleidout,
                new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                General.GetNullableString(otherrisk),
                General.GetNullableString(otherriskproposed),
                txtActivityCondition.Text,
                rblOtherRiskControl.SelectedValue,
                General.GetNullableInteger(ucCompetencyLevel.SelectedQuick),
                General.GetNullableString(HttpUtility.HtmlDecode(txtRiskAspects.Content)));

            Filter.CurrentMultipleRASelection = riskassessmentprocessmultipleidout.ToString();
            ucStatus.Text = "RA updated.";
            RiskAssessmentProcessMultipleEdit();

            String scriptinsert = String.Format("javascript:parent.fnReloadList('code1');");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptinsert, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidProcessTemplate()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtActivityCondition.Text) == null)
            ucError.ErrorMessage = "Activity/Condition is required.";

        return (!ucError.IsError);
    }

    private void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                //if (cbl.Items.FindByValue(item) != null)
                //    cbl.Items.FindByValue(item).Selected = true;
                cbl.SelectedValue = item;
            }
        }
    }

    private string ReadCheckBoxList(RadCheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ButtonListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }

    protected void gvHealthSafetyRisk_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentProcess.ListProcessCategory(1,
                (Filter.CurrentMultipleRASelection == null ? null : General.GetNullableGuid(Filter.CurrentMultipleRASelection.ToString()))
                , General.GetNullableInteger(ViewState["VESSELID"].ToString()));

            gvHealthSafetyRisk.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEnvironmentalRisk_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentProcess.ListProcessCategory(2,
                (Filter.CurrentMultipleRASelection == null ? null : General.GetNullableGuid(Filter.CurrentMultipleRASelection.ToString()))
                , General.GetNullableInteger(ViewState["VESSELID"].ToString()));

            gvEnvironmentalRisk.DataSource = ds;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEconomicRisk_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentProcess.ListProcessCategory(4,
                (Filter.CurrentMultipleRASelection == null ? null : General.GetNullableGuid(Filter.CurrentMultipleRASelection.ToString()))
                , General.GetNullableInteger(ViewState["VESSELID"].ToString()));
            gvEconomicRisk.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEconomicRisk_ItemDataBound(object sender, GridItemEventArgs ge)
    {
        if (ge.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)ge.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (ge.Item is GridFooterItem)
        {
        LinkButton cmdAdd = (LinkButton)ge.Item.FindControl("cmdAdd");
        if (cmdAdd != null)
        {
            cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }
        }
    }

    protected void gvEconomicRisk_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = gce.Item.ItemIndex;

            if (gce.CommandName.ToUpper().Equals("CADD"))
            {
                if (!IsValidHazard(ucEconomicHazardType.SelectedHazardType,
                                        ((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue, null, null))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentProcess.InsertProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucEconomicHazardType.SelectedHazardType),
                        new Guid(((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(Filter.CurrentMultipleRASelection.ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)gce.Item.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((RadTextBox)gce.Item.FindControl("txtProposedControlAdd")).Text), null
                        );

                ucEconomicHazardType.SelectedHazardType = "";
                gvEconomicRisk.Rebind();
                ucStatus.Text = "Hazard  Added.";

            }
            else if (gce.CommandName.ToUpper().Equals("CDELETE"))
            {
                string categoryid = ((RadLabel)gce.Item.FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentProcess.DeleteProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(categoryid));
                gvEconomicRisk.Rebind();
                ucStatus.Text = "Hazard  Deleted.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEnvironmentalRisk_ItemCommand(object sender, GridCommandEventArgs gce)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = gce.Item.ItemIndex;

            if (gce.CommandName.ToUpper().Equals("EADD"))
            {
                if (!IsValidHazard(ucEnvHazardType.SelectedHazardType,
                                        ((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue, 2,
                                        ((UserControlRAMiscellaneous)gce.Item.FindControl("ucImpactType")).SelectedMiscellaneous))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentProcess.InsertProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucEnvHazardType.SelectedHazardType),
                        new Guid(((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(Filter.CurrentMultipleRASelection.ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)gce.Item.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((RadTextBox)gce.Item.FindControl("txtProposedControlAdd")).Text),
                        General.GetNullableInteger(((UserControlRAMiscellaneous)gce.Item.FindControl("ucImpactType")).SelectedMiscellaneous)
                        );

                ucEnvHazardType.SelectedHazardType = "";
                gvEnvironmentalRisk.Rebind();
                ucStatus.Text = "Hazard  Added.";

            }
            else if (gce.CommandName.ToUpper().Equals("EDELETE"))
            {
                string categoryid = ((RadLabel)gce.Item.FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentProcess.DeleteProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(categoryid));
                gvEnvironmentalRisk.Rebind();
                ucStatus.Text = "Hazard  Deleted.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEnvironmentalRisk_ItemDataBound(object sender, GridItemEventArgs ge)
    {
        if (ge.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)ge.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (ge.Item is GridFooterItem)
        {
            UserControlRAMiscellaneous ucImpactType = (UserControlRAMiscellaneous)ge.Item.FindControl("ucImpactType");
            if (ucImpactType != null)
            {
                ucImpactType.MiscellaneousList = PhoenixInspectionRiskAssessmentMiscellaneous.ListRiskAssessmentMiscellaneous(3, 0);
                ucImpactType.DataBind();
            }

            LinkButton cmdAdd = (LinkButton)ge.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
            }
        }
    }

    protected void gvHealthSafetyRisk_ItemDataBound(object sender, GridItemEventArgs ge)
    {
        if (ge.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)ge.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }
        if (ge.Item is GridFooterItem)
        {
            LinkButton cmdAdd = (LinkButton)ge.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
            {
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
            }
        }
    }

    protected void gvHealthSafetyRisk_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("HADD"))
            {
                if (!IsValidHazard(ucHazardType.SelectedHazardType,
                                        ((RadComboBox)e.Item.FindControl("ddlSubHazardType")).SelectedValue, null, null))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentProcess.InsertProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        Convert.ToInt32(ucHazardType.SelectedHazardType),
                        new Guid(((RadComboBox)e.Item.FindControl("ddlSubHazardType")).SelectedValue),
                        new Guid(Filter.CurrentMultipleRASelection.ToString()),
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableInteger(((UserControlRAFrequency)e.Item.FindControl("ddlControlAdd")).SelectedFrequency),
                        General.GetNullableString(((RadTextBox)e.Item.FindControl("txtProposedControlAdd")).Text), null
                        );

                ucHazardType.SelectedHazardType = "";
                gvHealthSafetyRisk.Rebind();
                ucStatus.Text = "Hazard  Added.";

            }
            else if (e.CommandName.ToUpper().Equals("HDELETE"))
            {
                string categoryid = ((RadLabel)e.Item.FindControl("lblCategoryid")).Text;
                PhoenixInspectionRiskAssessmentProcess.DeleteProcessCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(categoryid));
                gvHealthSafetyRisk.Rebind();
                ucStatus.Text = "Hazard  Deleted.";
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidHazard(string hazardtypeid, string subhazardid, int? type, string impacttype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (Filter.CurrentMultipleRASelection == null)
            ucError.ErrorMessage = "Please save the RA first and then try adding the Hazards.";

        if (Filter.CurrentMultipleRASelection != null && Filter.CurrentMultipleRASelection.ToString() != string.Empty)
        {
            if (General.GetNullableInteger(hazardtypeid) == null)
                ucError.ErrorMessage = "Please select the Hazard Type and then add the Impact";

            if (type != null && type.ToString() == "2")
            {
                if (General.GetNullableInteger(impacttype) == null)
                    ucError.ErrorMessage = "Impact Type is required.";
            }

            if (General.GetNullableGuid(subhazardid) == null)
                ucError.ErrorMessage = "Impact is required.";

        }

        return (!ucError.IsError);

    }
}
