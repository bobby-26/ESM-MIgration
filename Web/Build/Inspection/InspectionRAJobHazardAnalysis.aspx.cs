using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Data;
using System.Web;
using Telerik.Web.UI;
public partial class InspectionRAJobHazardAnalysis : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            if (!IsPostBack)
            {
                ddlCategory.CategoryList = PhoenixInspectionRiskAssessmentCategory.ListRiskAssessmentCategory();
                ddlCategory.DataBind();
                ddlCategory.SelectedCategory = "5";


                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;

                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucCompany.SelectedCompany = ViewState["COMPANYID"].ToString();
                    ucCompany.Enabled = false;
                }
                else
                    ucCompany.Enabled = true;

                BindJob();

                txtVessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
                ViewState["JOBHAZARDID"] = "";
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["STATUS"] = "";
                BindRecomendedPPE();
                BindPotentialConsequences();
                BindOtherRisk();

                if (Request.QueryString["jobhazardid"] != null)
                {
                    EditJobHazard(new Guid(Request.QueryString["jobhazardid"]));
                }
                if (Request.QueryString["jobhazardid"] != null)
                    ViewState["JOBHAZARDID"] = Request.QueryString["jobhazardid"].ToString();
                if (Request.QueryString["status"] != null)
                    ViewState["STATUS"] = Request.QueryString["status"].ToString();

                if (Request.QueryString["jobhazardid"] != null)
                {
                    EditJobHazard(new Guid(Request.QueryString["jobhazardid"]));
                    ViewState["JOBHAZARDID"] = Request.QueryString["jobhazardid"];
                }

                BindHSHazard();
                BindHealthSafety();
                BindGridEnvironmentalRisk();
                BindGridEconomicRisk();
            }
            BindHealthSafety();
            BindGridEconomicRisk();
            BindGridEnvironmentalRisk();
            BindComapany();
            BindMenu();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        if (General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()) == null)
        {
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        }
        else
        {
            if (General.GetNullableString(ViewState["STATUS"].ToString()) == "8" || General.GetNullableString(ViewState["STATUS"].ToString()) == "6")
            {
                toolbar.AddButton("Request Approval", "REQUESTAPPROVAL", ToolBarDirection.Right);
            }
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
            {
                if (General.GetNullableString(ViewState["STATUS"].ToString()) == "4" || General.GetNullableString(ViewState["STATUS"].ToString()) == "1")
                {
                    toolbar.AddButton("Approve", "APPROVE", ToolBarDirection.Right);
                }
                if ((General.GetNullableString(ViewState["STATUS"].ToString()) == "2"))
                {
                    toolbar.AddButton("Issue", "ISSUE", ToolBarDirection.Right);
                }
            }
            if (!(General.GetNullableString(ViewState["STATUS"].ToString()) == "3" || General.GetNullableString(ViewState["STATUS"].ToString()) == "5" || General.GetNullableString(ViewState["STATUS"].ToString()) == "7")) //if status is Draft or Approved                
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        }
        toolbar.AddButton("List", "LIST", ToolBarDirection.Right);
        MenuJobHazardGeneral.AccessRights = this.ViewState;
        MenuJobHazardGeneral.MenuList = toolbar.Show();
    }
    protected void EditJobHazard(Guid jobhazardid)
    {
        try
        {
            DataSet ds = PhoenixInspectionRiskAssessmentJobHazard.EditRiskAssessmentJobHazard(jobhazardid);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtHazid.Text = ds.Tables[0].Rows[0]["FLDHAZARDNUMBER"].ToString();
                txtRevNO.Text = ds.Tables[0].Rows[0]["FLDREVISIONNO"].ToString();
                txtpreparedby.Text = ds.Tables[0].Rows[0]["FLDPREPAREDBY"].ToString();
                ucCreatedDate.Text = ds.Tables[0].Rows[0]["FLDPREPAREDDATE"].ToString();
                txtApprovedby.Text = ds.Tables[0].Rows[0]["FLDAPPROVEDBY"].ToString();
                ucApprovedDate.Text = ds.Tables[0].Rows[0]["FLDAPPROVEDDATE"].ToString();
                txtIssuedBy.Text = ds.Tables[0].Rows[0]["FLDISSUEDBY"].ToString();
                ucIssuedDate.Text = ds.Tables[0].Rows[0]["FLDISSUEDDATE"].ToString();
                ddlCategory.SelectedCategory = ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString();
                txtJob.Text = ds.Tables[0].Rows[0]["FLDJOB"].ToString();
                ucEditorOperationalHazard.Content = ds.Tables[0].Rows[0]["FLDOPERATIONALHAZARD"].ToString();
                ucEditorConrolPrecautions.Content = ds.Tables[0].Rows[0]["FLDCONTROLSPRECAUTIONS"].ToString();
                ucCompetencyLevel.SelectedQuick = ds.Tables[0].Rows[0]["FLDCOMPETENCYLEVEL"].ToString();
                txtVessel.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();
                txtStatus.Text = ds.Tables[0].Rows[0]["FLDSTATUS"].ToString();
                txtRemarks.Text = ds.Tables[0].Rows[0]["FLDREMARKS"].ToString();
                if (ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString() != "")
                {
                    ddlJob.SelectedValue = ds.Tables[0].Rows[0]["FLDJOBID"].ToString();
                }
                BindCheckBoxList(cblRecomendedPPE, ds.Tables[0].Rows[0]["FLDRECOMMENDEDPPE"].ToString());
                BindCheckBoxList(cblOtherRisk, ds.Tables[0].Rows[0]["FLDOTHERHAZARD"].ToString());
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString()))
                {
                    ucCompany.SelectedCompany = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
                    ucCompany.Enabled = false;
                }
                else
                    ucCompany.Enabled = true;

                ViewState["STATUS"] = ds.Tables[0].Rows[0]["FLDSTATUSID"].ToString();
                BindMenu();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuJobHazardGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidJobHazard(ddlCategory.SelectedCategory, ddlJob.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }
                string potentialconsequence = ""; 
                string recommendedppe = ReadCheckBoxList(cblRecomendedPPE);
                string otherhazard = ReadCheckBoxList(cblOtherRisk);

                string strOperationalHazard = HttpUtility.HtmlDecode(ucEditorOperationalHazard.Content);
                string strConrolPrecautions = HttpUtility.HtmlDecode(ucEditorConrolPrecautions.Content);

                if (ViewState["JOBHAZARDID"].ToString() == "")
                {
                    Guid? jobhazardid = General.GetNullableGuid(null);
                    PhoenixInspectionRiskAssessmentJobHazard.InsertRiskAssessmentJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                        int.Parse(ddlCategory.SelectedCategory),
                                                        General.GetNullableInteger(ddlJob.SelectedValue),
                                                        potentialconsequence,
                                                        recommendedppe, txtJob.Text, otherhazard,
                                                        ref jobhazardid,
                                                        General.GetNullableString(strOperationalHazard),
                                                        General.GetNullableString(strConrolPrecautions),
                                                        General.GetNullableInteger(ucCompetencyLevel.SelectedQuick),
                                                        General.GetNullableInteger(ucCompany.SelectedCompany),
                                                        General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                        );

                    ViewState["JOBHAZARDID"] = jobhazardid;
                    ucStatus.Text = "Job Hazard updated. ";
                    Filter.CurrentSelectedJHA = jobhazardid.ToString();
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                }
                else
                {
                    PhoenixInspectionRiskAssessmentJobHazard.UpdateRiskAssessmentJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                    new Guid(ViewState["JOBHAZARDID"].ToString()),
                                                    int.Parse(ddlCategory.SelectedCategory),
                                                    General.GetNullableInteger(ddlJob.SelectedValue),
                                                    potentialconsequence,
                                                    recommendedppe, txtJob.Text, otherhazard,
                                                    General.GetNullableString(strOperationalHazard),
                                                    General.GetNullableString(strConrolPrecautions),
                                                    General.GetNullableInteger(ucCompetencyLevel.SelectedQuick),
                                                    General.GetNullableInteger(ucCompany.SelectedCompany)
                                                    );
                    ucStatus.Text = "Job Hazard updated. ";
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                }
            }
            if (CommandName.ToUpper().Equals("APPROVE"))
            {
                if (!IsValidJobHazard(ddlCategory.SelectedCategory, ddlJob.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["STATUS"].ToString() == "1" || ViewState["STATUS"].ToString() == "2")
                {
                    PhoenixInspectionRiskAssessmentJobHazard.UpdateRiskAssessmentJobHazardApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                     new Guid(ViewState["JOBHAZARDID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.UserCode, null, 2);
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                    ucStatus.Text = "Job Hazard Approved. ";
                }
                if (ViewState["STATUS"].ToString() == "4")
                {
                    string script = "openNewWindow('Bank','', '" + Session["sitepath"] + "/Inspection/InspectionVesselRAJHAApproval.aspx?RATEMPLATEID=" + ViewState["JOBHAZARDID"].ToString() + "&TYPE=4','medium');";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "openScript", script, true);
                    EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                }
            }
            if (CommandName.ToUpper().Equals("REQUESTAPPROVAL"))
            {
                if (!IsValidJobHazard(ddlCategory.SelectedCategory, ddlJob.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentJobHazard.UpdateRiskAssessmentJobHazardApprovalRequest(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 new Guid(ViewState["JOBHAZARDID"].ToString()));

                EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                ucStatus.Text = "Job Hazard Approval Requested. .";
            }
            if (CommandName.ToUpper().Equals("ISSUE"))
            {
                if (!IsValidJobHazard(ddlCategory.SelectedCategory, ddlJob.SelectedValue))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentJobHazard.UpdateRiskAssessmentJobHazardApproval(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 new Guid(ViewState["JOBHAZARDID"].ToString()), null, PhoenixSecurityContext.CurrentSecurityContext.UserCode, 3);
                EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
                ucStatus.Text = "Job Hazard Issued. ";
            }
            if (CommandName.ToUpper().Equals("LIST"))
            {
                Response.Redirect("../Inspection/InspectionRAJobHazardAnalysisList.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindHSHazard()
    {
    }
    protected void BindPotentialConsequences()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentMiscellaneous.RiskAssessmentMiscellaneousSearch(null,
            4,
            null, null,
            1,
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);
    }
    protected void BindRecomendedPPE()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentMiscellaneous.RiskAssessmentMiscellaneousSearch(null,
            5,
            null, null,
            1,
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            cblRecomendedPPE.DataSource = ds.Tables[0];
            cblRecomendedPPE.DataBindings.DataTextField = "FLDNAME";
            cblRecomendedPPE.DataBindings.DataValueField = "FLDMISCELLANEOUSID";
            cblRecomendedPPE.DataBind();
        }
    }
    protected void BindOtherRisk()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentHazard.RiskAssessmentHazardSearch(null,
            3,
            null, null,
            1,
            General.ShowRecords(null),
            ref iRowCount,
            ref iTotalPageCount);

        if (ds.Tables[0].Rows.Count > 0)
        {
            cblOtherRisk.DataSource = ds.Tables[0];
            cblOtherRisk.DataBindings.DataTextField = "FLDNAME";
            cblOtherRisk.DataBindings.DataValueField = "FLDHAZARDID";
            cblOtherRisk.DataBind();
        }
    }
    protected void BindJob()
    {
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(5, General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlJob.Items.Clear();
            ddlJob.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlJob.DataSource = ds.Tables[0];
            ddlJob.DataBind();
        }
    }
    protected void BindJob_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        BindJob();
    }

    private void BindCheckBoxList(RadCheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                //if (cbl.Items.FindByValue(item) != null)
                //cbl.Items.FindByValue(item).Selected = true;
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
    private void BindHealthSafety()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentJobHazard.ListRiskAssessmentHealthAndSafetyJobHazard(General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()), 1);
        gvHealthSafetyRisk.DataSource = ds;
        gvHealthSafetyRisk.DataBind();

        GridFooterItem gvHealthSafetyRiskfooteritem = (GridFooterItem)gvHealthSafetyRisk.MasterTableView.GetItems(GridItemType.Footer)[0];
        RadComboBox ddlHealthSubHazardType = (RadComboBox)gvHealthSafetyRiskfooteritem.FindControl("ddlSubHazardType");
        ddlHealthSubHazardType.DataTextField = "FLDNAME";
        ddlHealthSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddlHealthSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucHazardType.SelectedHazardType));
        ddlHealthSubHazardType.DataBind();
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
            if (e.CommandName.ToUpper().Equals("HEALTHSAFETYADD"))
            {
                if (!IsValidHazard(ucHazardType.SelectedHazardType,
                                    ((RadComboBox)e.Item.FindControl("ddlSubHazardType")).SelectedValue, null, null
                ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentJobHazard.InsertRiskAssessmentHealthAndSafetyJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["JOBHAZARDID"].ToString()),
                        Convert.ToInt32(ucHazardType.SelectedHazardType),
                        new Guid(((RadComboBox)e.Item.FindControl("ddlSubHazardType")).SelectedValue), null);

                BindHealthSafety();
            }
            else if (e.CommandName.ToUpper().Equals("HEALTHSAFETYDELETE"))
            {
                string Healthid = ((RadLabel)e.Item.FindControl("lblHealthid")).Text;
                PhoenixInspectionRiskAssessmentJobHazard.DeleteRiskAssessmentHealthAndSafetyJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(Healthid));

                BindHealthSafety();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindGridEnvironmentalRisk()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentJobHazard.ListRiskAssessmentHealthAndSafetyJobHazard(General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()), 2);
        gvEnvironmentalRisk.DataSource = ds;
        gvEnvironmentalRisk.DataBind();

        GridFooterItem gvEnvironmentalRiskfooteritem = (GridFooterItem)gvEnvironmentalRisk.MasterTableView.GetItems(GridItemType.Footer)[0];
        RadComboBox ddlEnvSubHazardType = (RadComboBox)gvEnvironmentalRiskfooteritem.FindControl("ddlSubHazardType");
        ddlEnvSubHazardType.DataTextField = "FLDNAME";
        ddlEnvSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddlEnvSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucEnvHazardType.SelectedHazardType));
        ddlEnvSubHazardType.DataBind();
    }
    private void BindGridEconomicRisk()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentJobHazard.ListRiskAssessmentHealthAndSafetyJobHazard(General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()), 4);
        gvEconomicRisk.DataSource = ds;
        gvEconomicRisk.DataBind();

        GridFooterItem gvEconomicRiskfooteritem = (GridFooterItem)gvEconomicRisk.MasterTableView.GetItems(GridItemType.Footer)[0];
        RadComboBox ddlEconmicSubHazardType = (RadComboBox)gvEconomicRiskfooteritem.FindControl("ddlSubHazardType");
        ddlEconmicSubHazardType.DataTextField = "FLDNAME";
        ddlEconmicSubHazardType.DataValueField = "FLDSUBHAZARDID";
        ddlEconmicSubHazardType.DataSource = PhoenixInspectionRiskAssessmentSubHazard.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucEconomicHazardType.SelectedHazardType));
        ddlEconmicSubHazardType.DataBind();
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
            if (gce.CommandName.ToUpper().Equals("ECONOMICADD"))
            {

                if (!IsValidHazard(ucEconomicHazardType.SelectedHazardType,
                                        ((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue, null, null
                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentJobHazard.InsertRiskAssessmentHealthAndSafetyJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(ViewState["JOBHAZARDID"].ToString()),
                            Convert.ToInt32(ucEconomicHazardType.SelectedHazardType),
                            new Guid(((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue), null);

                ucEconomicHazardType.SelectedHazardType = "";
                BindGridEconomicRisk();
            }
            else if (gce.CommandName.ToUpper().Equals("ECONOMICDELETE"))
            {
                string Healthid = ((RadLabel)gce.Item.FindControl("lblEconomicid")).Text;
                PhoenixInspectionRiskAssessmentJobHazard.DeleteRiskAssessmentHealthAndSafetyJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(Healthid));
                BindGridEconomicRisk();
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
            if (gce.CommandName.ToUpper().Equals("ENVIRONMENTALADD"))
            {
                if (!IsValidHazard(ucEnvHazardType.SelectedHazardType,
                                        ((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue, 2,
                                        ((UserControlRAMiscellaneous)gce.Item.FindControl("ucImpactType")).SelectedMiscellaneous
                    ))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionRiskAssessmentJobHazard.InsertRiskAssessmentHealthAndSafetyJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(ViewState["JOBHAZARDID"].ToString()),
                            Convert.ToInt32(ucEnvHazardType.SelectedHazardType),
                            new Guid(((RadComboBox)gce.Item.FindControl("ddlSubHazardType")).SelectedValue),
                            General.GetNullableInteger(((UserControlRAMiscellaneous)gce.Item.FindControl("ucImpactType")).SelectedMiscellaneous));

                ucEnvHazardType.SelectedHazardType = "";
                BindGridEnvironmentalRisk();
            }
            else if (gce.CommandName.ToUpper().Equals("ENVIRONMENTALDELETE"))
            {
                string Healthid = ((RadLabel)gce.Item.FindControl("lblEnvironmentid")).Text;
                PhoenixInspectionRiskAssessmentJobHazard.DeleteRiskAssessmentHealthAndSafetyJobHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(Healthid));
                BindGridEnvironmentalRisk();
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

    private bool IsValidHazard(string hazardtypeid, string subhazardid, int? type, string impacttype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Job Hazard first and then add.";

        if (General.GetNullableGuid(ViewState["JOBHAZARDID"].ToString()) != null)
        {
            if (General.GetNullableInteger(hazardtypeid) == null)
                ucError.ErrorMessage = "Please select the Hazard Type and then add Impact.";

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
    private bool IsValidJobHazard(string categoryid, string jobid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucCompany.SelectedCompany) == null)
            ucError.ErrorMessage = "Company is required.";

        if (General.GetNullableString(categoryid) == null)
            ucError.ErrorMessage = "Type is required.";

        if (General.GetNullableString(jobid) == null)
            ucError.ErrorMessage = "Category is required.";

        if (General.GetNullableString(txtJob.Text) == null)
            ucError.ErrorMessage = "Job is required.";

        return (!ucError.IsError);

    }
    private bool IsValidRAHazard(string Hazard, int type)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ViewState["JOBHAZARDID"].Equals(""))
            ucError.ErrorMessage = "Add the Job Hazard first and then add Hazard.";
        if (type == 1)
        {
            if (Hazard.Trim().Equals(""))
                ucError.ErrorMessage = " Operational Hazard is required.";
        }
        if (type == 2)
        {
            if (Hazard.Trim().Equals(""))
                ucError.ErrorMessage = " Controls/Precautions  is required.";
        }

        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        EditJobHazard(new Guid(ViewState["JOBHAZARDID"].ToString()));
    }
    protected void BindComapany()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            DataSet ds = PhoenixInspectionRiskAssessmentProcess.MappedVesselCompany(PhoenixSecurityContext.CurrentSecurityContext.VesselID);

            if (ds.Tables[0].Rows.Count > 0)
                ucCompany.SelectedCompany = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
        }
    }

    protected void ucHazardType_TextChangedEvent(object sender, EventArgs e)
    {
        BindHealthSafety();
    }

    protected void ucEnvHazardType_TextChangedEvent(object sender, EventArgs e)
    {
        BindGridEnvironmentalRisk();
    }

    protected void ucEconomicHazardType_TextChangedEvent(object sender, EventArgs e)
    {
        BindGridEconomicRisk();
    }
}

