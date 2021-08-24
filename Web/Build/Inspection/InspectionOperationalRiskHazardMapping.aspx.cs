using System;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Web.UI;
using Telerik.Web.UI;
using System.Data;

public partial class InspectionOperationalRiskHazardMapping : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Import", "IMPORT", ToolBarDirection.Right);
        MenuMSCAT.AccessRights = this.ViewState;
        MenuMSCAT.MenuList = toolbar.Show();

        if (!IsPostBack)
        {
            ViewState["OPERATIONALHAZARDID"] = "";

            ViewState["RATYPEID"] = "";

            ViewState["RAID"] = "";

            if ((Request.QueryString["ratypeid"] != null) && (Request.QueryString["ratypeid"] != ""))
            {
                ViewState["RATYPEID"] = Request.QueryString["ratypeid"].ToString();
            }

            if ((Request.QueryString["Operationalhazardid"] != null) && (Request.QueryString["Operationalhazardid"] != ""))
            {
                ViewState["OPERATIONALHAZARDID"] = Request.QueryString["Operationalhazardid"].ToString();
            }

            if ((Request.QueryString["RAID"] != null) && (Request.QueryString["RAID"] != ""))
            {
                ViewState["RAID"] = Request.QueryString["RAID"].ToString();
            }

            BindHazardType();
            BindImpactType();
            BindAspect();
        }
    }

    private void BindAspect()
    {
        if (ViewState["OPERATIONALHAZARDID"].ToString() != "")
        {
            if (ViewState["RATYPEID"].ToString() == "1")
            {
               DataTable dt = PhoenixInspectionRiskAssessmentJobHazardExtn.EditJHAOperationalHazard(General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString())
                                                                                                      , General.GetNullableGuid(ViewState["RAID"].ToString()));
               if (dt.Rows.Count > 0)
               {
                   txtaspect.Text = dt.Rows[0]["FLDASPECT"].ToString();
                   txtHazardRisks.Text = dt.Rows[0]["FLDOPERATIONALHAZARD"].ToString();
                   rblimpacttype.SelectedValue = dt.Rows[0]["FLDIMPACTTYPEID"].ToString();
                    General.BindCheckBoxList(ChkHazard, dt.Rows[0]["FLDHSHEALTHANDSAFETY"].ToString());
                    BindHazardImpact();
                    General.BindCheckBoxList(ChkImpact, dt.Rows[0]["FLDHSSUBHEALTHANDSAFETY"].ToString());
                    General.BindCheckBoxList(ChkEcoHazard, dt.Rows[0]["FLDECOHEALTHANDSAFETY"].ToString());
                    BindHazardEconomicImpact();
                    General.BindCheckBoxList(ChkEcoImpact, dt.Rows[0]["FLDECOSUBHEALTHANDSAFETY"].ToString());
                    General.BindCheckBoxList(ChkEnvHazard, dt.Rows[0]["FLDENVHEALTHANDSAFETY"].ToString());
                    BindHazardEnvironmentImpact();
                    General.BindCheckBoxList(ChkEnvImpact, dt.Rows[0]["FLDENVSUBHEALTHANDSAFETY"].ToString());
                }

            }

            if (ViewState["RATYPEID"].ToString() == "3")
            {
                DataTable dt = PhoenixInspectionRiskAssessmentJobHazardExtn.EditRAGenericOperationalHazard(General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString())
                                                                                                       , General.GetNullableGuid(ViewState["RAID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtaspect.Text = dt.Rows[0]["FLDASPECT"].ToString();
                    txtHazardRisks.Text = dt.Rows[0]["FLDOPERATIONALHAZARD"].ToString();
                    rblimpacttype.SelectedValue = dt.Rows[0]["FLDIMPACTTYPEID"].ToString();
                    General.BindCheckBoxList(ChkHazard, dt.Rows[0]["FLDHSHEALTHANDSAFETY"].ToString());
                    BindHazardImpact();
                    General.BindCheckBoxList(ChkImpact, dt.Rows[0]["FLDHSSUBHEALTHANDSAFETY"].ToString());
                    General.BindCheckBoxList(ChkEcoHazard, dt.Rows[0]["FLDECOHEALTHANDSAFETY"].ToString());
                    BindHazardEconomicImpact();
                    General.BindCheckBoxList(ChkEcoImpact, dt.Rows[0]["FLDECOSUBHEALTHANDSAFETY"].ToString());
                    General.BindCheckBoxList(ChkEnvHazard, dt.Rows[0]["FLDENVHEALTHANDSAFETY"].ToString());
                    BindHazardEnvironmentImpact();
                    General.BindCheckBoxList(ChkEnvImpact, dt.Rows[0]["FLDENVSUBHEALTHANDSAFETY"].ToString());
                }
            }

            if (ViewState["RATYPEID"].ToString() == "4")
            {
                DataTable dt = PhoenixInspectionRiskAssessmentJobHazardExtn.EditRANavigationOperationalHazard(General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString())
                                                                                                       , General.GetNullableGuid(ViewState["RAID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtaspect.Text = dt.Rows[0]["FLDASPECT"].ToString();
                    txtHazardRisks.Text = dt.Rows[0]["FLDOPERATIONALHAZARD"].ToString();
                    rblimpacttype.SelectedValue = dt.Rows[0]["FLDIMPACTTYPEID"].ToString();
                    General.BindCheckBoxList(ChkHazard, dt.Rows[0]["FLDHSHEALTHANDSAFETY"].ToString());
                    BindHazardImpact();
                    General.BindCheckBoxList(ChkImpact, dt.Rows[0]["FLDHSSUBHEALTHANDSAFETY"].ToString());
                    General.BindCheckBoxList(ChkEcoHazard, dt.Rows[0]["FLDECOHEALTHANDSAFETY"].ToString());
                    BindHazardEconomicImpact();
                    General.BindCheckBoxList(ChkEcoImpact, dt.Rows[0]["FLDECOSUBHEALTHANDSAFETY"].ToString());
                    General.BindCheckBoxList(ChkEnvHazard, dt.Rows[0]["FLDENVHEALTHANDSAFETY"].ToString());
                    BindHazardEnvironmentImpact();
                    General.BindCheckBoxList(ChkEnvImpact, dt.Rows[0]["FLDENVSUBHEALTHANDSAFETY"].ToString());
                }
            }

            if (ViewState["RATYPEID"].ToString() == "5")
            {
                DataTable dt = PhoenixInspectionRiskAssessmentJobHazardExtn.EditRAMachineryOperationalHazard(General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString())
                                                                                                       , General.GetNullableGuid(ViewState["RAID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtaspect.Text = dt.Rows[0]["FLDASPECT"].ToString();
                    txtHazardRisks.Text = dt.Rows[0]["FLDOPERATIONALHAZARD"].ToString();
                    rblimpacttype.SelectedValue = dt.Rows[0]["FLDIMPACTTYPEID"].ToString();
                    General.BindCheckBoxList(ChkHazard, dt.Rows[0]["FLDHSHEALTHANDSAFETY"].ToString());
                    BindHazardImpact();
                    General.BindCheckBoxList(ChkImpact, dt.Rows[0]["FLDHSSUBHEALTHANDSAFETY"].ToString());
                    General.BindCheckBoxList(ChkEcoHazard, dt.Rows[0]["FLDECOHEALTHANDSAFETY"].ToString());
                    BindHazardEconomicImpact();
                    General.BindCheckBoxList(ChkEcoImpact, dt.Rows[0]["FLDECOSUBHEALTHANDSAFETY"].ToString());
                    General.BindCheckBoxList(ChkEnvHazard, dt.Rows[0]["FLDENVHEALTHANDSAFETY"].ToString());
                    BindHazardEnvironmentImpact();
                    General.BindCheckBoxList(ChkEnvImpact, dt.Rows[0]["FLDENVSUBHEALTHANDSAFETY"].ToString());
                }
            }

            if (ViewState["RATYPEID"].ToString() == "6")
            {
                DataTable dt = PhoenixInspectionRiskAssessmentJobHazardExtn.EditRACargoOperationalHazard(General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString())
                                                                                                       , General.GetNullableGuid(ViewState["RAID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtaspect.Text = dt.Rows[0]["FLDASPECT"].ToString();
                    txtHazardRisks.Text = dt.Rows[0]["FLDOPERATIONALHAZARD"].ToString();
                    rblimpacttype.SelectedValue = dt.Rows[0]["FLDIMPACTTYPEID"].ToString();
                    General.BindCheckBoxList(ChkHazard, dt.Rows[0]["FLDHSHEALTHANDSAFETY"].ToString());
                    BindHazardImpact();
                    General.BindCheckBoxList(ChkImpact, dt.Rows[0]["FLDHSSUBHEALTHANDSAFETY"].ToString());
                    General.BindCheckBoxList(ChkEcoHazard, dt.Rows[0]["FLDECOHEALTHANDSAFETY"].ToString());
                    BindHazardEconomicImpact();
                    General.BindCheckBoxList(ChkEcoImpact, dt.Rows[0]["FLDECOSUBHEALTHANDSAFETY"].ToString());
                    General.BindCheckBoxList(ChkEnvHazard, dt.Rows[0]["FLDENVHEALTHANDSAFETY"].ToString());
                    BindHazardEnvironmentImpact();
                    General.BindCheckBoxList(ChkEnvImpact, dt.Rows[0]["FLDENVSUBHEALTHANDSAFETY"].ToString());
                }
            }
        }
    }

    private void BindImpacts()
    {
        if (ViewState["RAID"].ToString() != "")
        {
                DataTable dt = PhoenixInspectionRiskAssessmentJobHazardExtn.GetJHAOperationalHazardImpact(General.GetNullableInteger(ViewState["RATYPEID"].ToString())
                                                                                                       , General.GetNullableGuid(ViewState["RAID"].ToString())
                                                                                                       , General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString())
                                                                                                       , int.Parse(rblimpacttype.SelectedValue));
                if (dt.Rows.Count > 0)
                {
                    
                    General.BindCheckBoxList(ChkEnvHazard, dt.Rows[0]["FLDENVHEALTHANDSAFETY"].ToString());
                    General.BindCheckBoxList(ChkEnvImpact, dt.Rows[0]["FLDENVSUBHEALTHANDSAFETY"].ToString());
                }
        }
    }

    protected void MenuMSCAT_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("IMPORT"))
        {
            if (!IsValidAdd())
            {
                ucError.Visible = true;
                return;
            }
            try
            {
                if (ViewState["RATYPEID"].ToString().Equals("1"))
                {
                    if ((General.GetNullableString(General.ReadCheckBoxList(ChkHazard)) != null) && (General.GetNullableString(General.ReadCheckBoxList(ChkImpact)) != null))
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.InsertJHACategoryFromHazardimpact(new Guid(ViewState["OPERATIONALHAZARDID"].ToString()),
                                        General.GetNullableString(General.ReadCheckBoxList(ChkHazard)),
                                        General.GetNullableString(General.ReadCheckBoxList(ChkImpact)),
                                        new Guid(ViewState["RAID"].ToString()),
                                        General.GetNullableInteger(null));
                    }

                    if ((General.GetNullableString(General.ReadCheckBoxList(ChkEcoHazard)) != null) && (General.GetNullableString(General.ReadCheckBoxList(ChkEcoImpact)) != null))
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.InsertJHACategoryFromHazardimpact(new Guid(ViewState["OPERATIONALHAZARDID"].ToString()),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEcoHazard)),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEcoImpact)),
                                    new Guid(ViewState["RAID"].ToString()),
                                    General.GetNullableInteger(null));
                    }

                    if ((General.GetNullableString(General.ReadCheckBoxList(ChkEnvHazard)) != null) && (General.GetNullableString(General.ReadCheckBoxList(ChkEnvImpact)) != null))
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.InsertJHACategoryFromHazardimpact(new Guid(ViewState["OPERATIONALHAZARDID"].ToString()),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEnvHazard)),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEnvImpact)),
                                    new Guid(ViewState["RAID"].ToString()),
                                    General.GetNullableInteger(rblimpacttype.SelectedValue));
                    }
                }

                if (ViewState["RATYPEID"].ToString().Equals("3"))
                {
                    if ((General.GetNullableString(General.ReadCheckBoxList(ChkHazard)) != null) && (General.GetNullableString(General.ReadCheckBoxList(ChkImpact)) != null))
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.InsertGenericRACategoryFromHazardimpact(new Guid(ViewState["OPERATIONALHAZARDID"].ToString()),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkHazard)),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkImpact)),
                                    new Guid(ViewState["RAID"].ToString()),
                                    General.GetNullableInteger(null));
                    }

                    if ((General.GetNullableString(General.ReadCheckBoxList(ChkEcoHazard)) != null) && (General.GetNullableString(General.ReadCheckBoxList(ChkEcoImpact)) != null))
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.InsertGenericRACategoryFromHazardimpact(new Guid(ViewState["OPERATIONALHAZARDID"].ToString()),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEcoHazard)),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEcoImpact)),
                                    new Guid(ViewState["RAID"].ToString()),
                                    General.GetNullableInteger(null));
                    }

                    if ((General.GetNullableString(General.ReadCheckBoxList(ChkEnvHazard)) != null) && (General.GetNullableString(General.ReadCheckBoxList(ChkEnvImpact)) != null))
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.InsertGenericRACategoryFromHazardimpact(new Guid(ViewState["OPERATIONALHAZARDID"].ToString()),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEnvHazard)),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEnvImpact)),
                                    new Guid(ViewState["RAID"].ToString()),
                                    General.GetNullableInteger(rblimpacttype.SelectedValue));
                    }
                }

                if (ViewState["RATYPEID"].ToString().Equals("4"))
                {
                    if ((General.GetNullableString(General.ReadCheckBoxList(ChkHazard)) != null) && (General.GetNullableString(General.ReadCheckBoxList(ChkImpact)) != null))
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.InsertNavigationRACategoryFromHazardimpact(new Guid(ViewState["OPERATIONALHAZARDID"].ToString()),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkHazard)),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkImpact)),
                                    new Guid(ViewState["RAID"].ToString()),
                                    General.GetNullableInteger(null));
                    }

                    if ((General.GetNullableString(General.ReadCheckBoxList(ChkEcoHazard)) != null) && (General.GetNullableString(General.ReadCheckBoxList(ChkEcoImpact)) != null))
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.InsertNavigationRACategoryFromHazardimpact(new Guid(ViewState["OPERATIONALHAZARDID"].ToString()),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEcoHazard)),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEcoImpact)),
                                    new Guid(ViewState["RAID"].ToString()),
                                    General.GetNullableInteger(null));
                    }

                    if ((General.GetNullableString(General.ReadCheckBoxList(ChkEnvHazard)) != null) && (General.GetNullableString(General.ReadCheckBoxList(ChkEnvImpact)) != null))
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.InsertNavigationRACategoryFromHazardimpact(new Guid(ViewState["OPERATIONALHAZARDID"].ToString()),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEnvHazard)),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEnvImpact)),
                                    new Guid(ViewState["RAID"].ToString()),
                                    General.GetNullableInteger(rblimpacttype.SelectedValue));
                    }
                }

                if (ViewState["RATYPEID"].ToString().Equals("5"))
                {
                    if ((General.GetNullableString(General.ReadCheckBoxList(ChkHazard)) != null) && (General.GetNullableString(General.ReadCheckBoxList(ChkImpact)) != null))
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.InsertMachineryRACategoryFromHazardimpact(new Guid(ViewState["OPERATIONALHAZARDID"].ToString()),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkHazard)),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkImpact)),
                                    new Guid(ViewState["RAID"].ToString()),
                                    General.GetNullableInteger(null));
                    }

                    if ((General.GetNullableString(General.ReadCheckBoxList(ChkEcoHazard)) != null) && (General.GetNullableString(General.ReadCheckBoxList(ChkEcoImpact)) != null))
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.InsertMachineryRACategoryFromHazardimpact(new Guid(ViewState["OPERATIONALHAZARDID"].ToString()),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEcoHazard)),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEcoImpact)),
                                    new Guid(ViewState["RAID"].ToString()),
                                    General.GetNullableInteger(null));
                    }

                    if ((General.GetNullableString(General.ReadCheckBoxList(ChkEnvHazard)) != null) && (General.GetNullableString(General.ReadCheckBoxList(ChkEnvImpact)) != null))
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.InsertMachineryRACategoryFromHazardimpact(new Guid(ViewState["OPERATIONALHAZARDID"].ToString()),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEnvHazard)),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEnvImpact)),
                                    new Guid(ViewState["RAID"].ToString()),
                                    General.GetNullableInteger(rblimpacttype.SelectedValue));
                    }
                }

                if (ViewState["RATYPEID"].ToString().Equals("6"))
                {
                    if ((General.GetNullableString(General.ReadCheckBoxList(ChkHazard)) != null) && (General.GetNullableString(General.ReadCheckBoxList(ChkImpact)) != null))
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.InsertCargoRACategoryFromHazardimpact(new Guid(ViewState["OPERATIONALHAZARDID"].ToString()),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkHazard)),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkImpact)),
                                    new Guid(ViewState["RAID"].ToString()),
                                    General.GetNullableInteger(null));
                    }

                    if ((General.GetNullableString(General.ReadCheckBoxList(ChkEcoHazard)) != null) && (General.GetNullableString(General.ReadCheckBoxList(ChkEcoImpact)) != null))
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.InsertCargoRACategoryFromHazardimpact(new Guid(ViewState["OPERATIONALHAZARDID"].ToString()),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEcoHazard)),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEcoImpact)),
                                    new Guid(ViewState["RAID"].ToString()),
                                    General.GetNullableInteger(null));
                    }

                    if ((General.GetNullableString(General.ReadCheckBoxList(ChkEnvHazard)) != null) && (General.GetNullableString(General.ReadCheckBoxList(ChkEnvImpact)) != null))
                    {
                        PhoenixInspectionRiskAssessmentJobHazardExtn.InsertCargoRACategoryFromHazardimpact(new Guid(ViewState["OPERATIONALHAZARDID"].ToString()),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEnvHazard)),
                                    General.GetNullableString(General.ReadCheckBoxList(ChkEnvImpact)),
                                    new Guid(ViewState["RAID"].ToString()),
                                    General.GetNullableInteger(rblimpacttype.SelectedValue));
                    }

                }

                String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
    }

    protected void BindImpactType()
    {

        rblimpacttype.DataSource = PhoenixInspectionRiskAssessmentMiscellaneousExtn.ListRiskAssessmentMiscellaneous(General.GetNullableInteger("3")
            , General.GetNullableInteger("0"));
        rblimpacttype.DataTextField = "FLDNAME";
        rblimpacttype.DataValueField = "FLDMISCELLANEOUSID";
        rblimpacttype.DataBind();
    }

    protected void BindHazardType()
    {
        ChkHazard.DataSource = PhoenixInspectionRiskAssessmentHazardExtn.ListRiskAssessmentHazard(1, 0);
        ChkHazard.DataTextField = "FLDNAME";
        ChkHazard.DataValueField = "FLDHAZARDID";
        ChkHazard.DataBind();

        ChkEnvHazard.DataSource = PhoenixInspectionRiskAssessmentHazardExtn.ListRiskAssessmentHazard(2, 0);
        ChkEnvHazard.DataTextField = "FLDNAME";
        ChkEnvHazard.DataValueField = "FLDHAZARDID";
        ChkEnvHazard.DataBind();

        ChkEcoHazard.DataSource = PhoenixInspectionRiskAssessmentHazardExtn.ListRiskAssessmentHazard(4, 0);
        ChkEcoHazard.DataTextField = "FLDNAME";
        ChkEcoHazard.DataValueField = "FLDHAZARDID";
        ChkEcoHazard.DataBind();
    }

    protected void BindHazardImpact()
    {       
        ChkImpact.DataSource = PhoenixInspectionRiskAssessmentSubHazardExtn.ListRiskAssessmentHazardImpact(General.GetNullableString(General.ReadCheckBoxList(ChkHazard)));
        ChkImpact.DataTextField = "FLDNAME";
        ChkImpact.DataValueField = "FLDSUBHAZARDID";
        ChkImpact.DataBind();
    }

    protected void BindHazardEconomicImpact()
    {
        ChkEcoImpact.DataSource = PhoenixInspectionRiskAssessmentSubHazardExtn.ListRiskAssessmentHazardImpact(General.GetNullableString(General.ReadCheckBoxList(ChkEcoHazard)));
        ChkEcoImpact.DataTextField = "FLDNAME";
        ChkEcoImpact.DataValueField = "FLDSUBHAZARDID";
        ChkEcoImpact.DataBind();
    }

    protected void BindHazardEnvironmentImpact()
    {
        ChkEnvImpact.DataSource = PhoenixInspectionRiskAssessmentSubHazardExtn.ListRiskAssessmentHazardImpact(General.GetNullableString(General.ReadCheckBoxList(ChkEnvHazard)));
        ChkEnvImpact.DataTextField = "FLDNAME";
        ChkEnvImpact.DataValueField = "FLDSUBHAZARDID";
        ChkEnvImpact.DataBind();
    }

    private bool IsValidAdd()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString()) == null)
            ucError.ErrorMessage = "Operational is required";

        //if (General.GetNullableString(General.ReadCheckBoxList(ChkHazard)) == null)
        //    ucError.ErrorMessage = "Hazard Type is required";

        //if ((General.GetNullableInteger(rblimpacttype.SelectedValue)==null)&&(rbltype.SelectedValue =="2"))
        //    ucError.ErrorMessage = "Impact Type is required";

        //if (General.GetNullableString(General.ReadCheckBoxList(ChkImpact)) == null)
        //    ucError.ErrorMessage = "Hazard Level is required.";

        return (!ucError.IsError);
    }

    protected void ChkHazard_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectimpact = General.ReadCheckBoxList(ChkImpact);
        BindHazardImpact();
        General.BindCheckBoxList(ChkImpact, selectimpact);
    }

    //protected void rbltype_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    BindHazardType();
    //    BindHazardImpact();
    //    BindImpactType();
    //    //if (rbltype.SelectedValue.Equals("2"))
    //    //{
    //    //    lblimpacttype.Visible = true;
    //    //    rblimpacttype.Visible = true;           
    //    //}

    //    //if (rbltype.SelectedValue != "2")
    //    //{
    //    //    lblimpacttype.Visible = false;
    //    //    rblimpacttype.Visible = false;            
    //    //}
    //}

    protected void ChkEcoHazard_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectimpact = General.ReadCheckBoxList(ChkEcoImpact);
        BindHazardEconomicImpact();
        General.BindCheckBoxList(ChkEcoImpact, selectimpact);
    }

    protected void ChkEnvHazard_SelectedIndexChanged(object sender, EventArgs e)
    {
        string selectimpact = General.ReadCheckBoxList(ChkEnvImpact);
        BindHazardEnvironmentImpact();
        General.BindCheckBoxList(ChkEnvImpact, selectimpact);
    }

    protected void rblimpacttype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ViewState["RAID"].ToString() != "")
        {
               DataTable dt = PhoenixInspectionRiskAssessmentJobHazardExtn.GetJHAOperationalHazardImpact(General.GetNullableInteger(ViewState["RATYPEID"].ToString())
                                                                                                       , General.GetNullableGuid(ViewState["RAID"].ToString())
                                                                                                       , General.GetNullableGuid(ViewState["OPERATIONALHAZARDID"].ToString())
                                                                                                       , int.Parse(rblimpacttype.SelectedValue));
                if (dt.Rows.Count > 0)
                {
                    ChkEnvHazard.ClearSelection();
                    ChkEnvImpact.ClearSelection();
                    General.BindCheckBoxList(ChkEnvHazard, dt.Rows[0]["FLDENVHEALTHANDSAFETY"].ToString());
                    General.BindCheckBoxList(ChkEnvImpact, dt.Rows[0]["FLDENVSUBHEALTHANDSAFETY"].ToString());
                }
        }
    }
}
