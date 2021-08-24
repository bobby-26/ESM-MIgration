using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRAProcessExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            VesselConfiguration();
            DateTime dt = DateTime.Today;
            txtDate.Text = dt.ToString();
            ViewState["RISKASSESSMENTPROCESSID"] = "";
            ViewState["VESSELID"] = "";
            ViewState["ACTIVITYID"] = "";
            ViewState["companyid"] = "";
            ViewState["PROCESSID"] = "-1";
            ViewState["STATUS"] = "-1";
            ViewState["MOCRequestid"] = string.IsNullOrEmpty(Request.QueryString["MOCRequestid"]) ? "" : Request.QueryString["MOCRequestid"];
            ViewState["MOCID"] = string.IsNullOrEmpty(Request.QueryString["MOCID"]) ? "" : Request.QueryString["MOCID"];
            ViewState["RATYPE"] = string.IsNullOrEmpty(Request.QueryString["RAType"]) ? "" : Request.QueryString["RAType"];
            ViewState["mocextention"] = string.IsNullOrEmpty(Request.QueryString["mocextention"]) ? "" : Request.QueryString["mocextention"];
            ViewState["VESSELID"] = string.IsNullOrEmpty(Request.QueryString["Vesselid"]) ? "" : Request.QueryString["Vesselid"];
            ViewState["Vesselname"] = string.IsNullOrEmpty(Request.QueryString["Vesselname"]) ? "" : Request.QueryString["Vesselname"];
            BindJob();
            ViewState["QUALITYCOMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["QUALITYCOMPANYID"] = nvc.Get("QMS");
                ucCompany.SelectedCompany = ViewState["QUALITYCOMPANYID"].ToString();
                ucCompany.Enabled = false;
            }
            else
                ucCompany.Enabled = true;

            if (Request.QueryString["DMSYN"] != null)
                ViewState["DMSYN"] = Request.QueryString["DMSYN"].ToString();
            else
                ViewState["DMSYN"] = "0";

            BindCompany();
            BindSupervisionLevel();

            if (Request.QueryString["processid"] != null)
            {
                ViewState["RISKASSESSMENTPROCESSID"] = Request.QueryString["processid"].ToString();
                RiskAssessmentProcessEdit();
            }

            if (Request.QueryString["HSEQADashboardYN"] != null)
                ViewState["HSEQADashboardYN"] = Request.QueryString["HSEQADashboardYN"].ToString();
            else
                ViewState["HSEQADashboardYN"] = "0";
        }
        BindMenu();
    }

    public void BindMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();

        if (ViewState["STATUS"].ToString() != "3") //if status is Draft or Approved
        {
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        }
        if ((Request.QueryString["RAType"] != null && Request.QueryString["RAType"].Equals("1")))
        {
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
        }
        if (ViewState["DMSYN"].ToString() != "1") //if status is Draft or Approved
        {
            toolbar.AddButton("List", "LIST", ToolBarDirection.Right);
        }

        if (ViewState["STATUS"].ToString() != "3")
        {

            PhoenixToolbar toolbarsub1 = new PhoenixToolbar();
            toolbarsub1.AddFontAwesomeButton("javascript: openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionJHATemplateMappingExtn.aspx?RISKASSESSMENTPROCESSID=" + ViewState["RISKASSESSMENTPROCESSID"].ToString() + "')", "Import JHA", "<i class=\"fas fa-tasks\"></i>", "IMPORTJHA");
            MenuImportedJHA.AccessRights = this.ViewState;
            MenuImportedJHA.MenuList = toolbarsub1.Show();
        }

        if (Request.QueryString["RevYN"] != "1")
        {
            MenuProcess.AccessRights = this.ViewState;
            MenuProcess.MenuList = toolbar.Show();
        }

        MenuTeamComposition.Visible = false;
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode.Equals(0) && ViewState["ACTIVITYID"].ToString() != "")
        {
            MenuTeamComposition.Visible = true;
            PhoenixToolbar toolbarTeamComposition = new PhoenixToolbar();
            toolbarTeamComposition.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionProcessRATeamComposition.aspx?Activityid=" + ViewState["ACTIVITYID"].ToString() + "&ActivityName=" + ddlCategory.Text + "')", "Team Composition", "<i class=\"fas fa-user-tie\"></i>", "TEAMADD");
            MenuTeamComposition.AccessRights = this.ViewState;
            MenuTeamComposition.MenuList = toolbarTeamComposition.Show();
        }
    }
    protected void BindJob()
    {
        DataTable dt = new DataTable();

        dt = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentElement();

        if (dt.Rows.Count > 0)
        {
            ucCategory.Items.Clear();
            ucCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ucCategory.DataSource = dt;
            ucCategory.DataBind();
        }
    }
    protected void CommentsLink()
    {
        lnkcomment.Visible = true;
        lnkTeamComposition.Visible = true;
        lnkImportedJHA.Visible = true;

        lnkImportJHA.Enabled = false;
        lnkImportJHA.Visible = false;
        chkImportedJHAList.Enabled = false;
        MenuImportedJHA.Visible = false;

        lnkTeamComposition.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=2&SECTIONID=8&RAID=" + ViewState["RISKASSESSMENTPROCESSID"].ToString() + "'); ");
        lnkcomment.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=2&SECTIONID=1&RAID=" + ViewState["RISKASSESSMENTPROCESSID"].ToString() + "'); ");
        lnkImportedJHA.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=2&SECTIONID=2&RAID=" + ViewState["RISKASSESSMENTPROCESSID"].ToString() + "'); ");
        txtRefNo.ReadOnly = true;
        txtRevNO.ReadOnly = true;
        UCDayNight.ShowDropDownOnTextboxClick = false;
        UCDayNight.ShowToggleImage = false;
        ucCreatedDate.ReadOnly = false;
        ucCreatedDate.Enabled = true;
        UCVisibility.ShowDropDownOnTextboxClick = false;
        UCVisibility.ShowToggleImage = false;
        txtpreparedby.ReadOnly = true;
        txtIssuedBy.ReadOnly = true;
        ucIssuedDate.Enabled = true;
        ucIssuedDate.ReadOnly = true;
        ucIssuedDate.DateIconVisible = false;
        ucCreatedDate.DateIconVisible = false;
        ucSteering.ShowDropDownOnTextboxClick = false;
        ucSteering.ShowToggleImage = false;
        ucCategory.ShowDropDownOnTextboxClick = false;
        ucCategory.ShowToggleImage = false;
        foreach (RadComboBoxItem item in ucCategory.Items)
        {
            item.Enabled = false;
        }
        UCWX.ShowDropDownOnTextboxClick = false;
        UCWX.ShowToggleImage = false;
        ddlCategory.ShowDropDownOnTextboxClick = false;
        ddlCategory.ShowToggleImage = false;
        foreach (RadComboBoxItem item in ddlCategory.Items)
        {
            item.Enabled = false;
        }
        UCUMSPermitted.ShowDropDownOnTextboxClick = false;
        UCUMSPermitted.ShowToggleImage = false;
        ddlsupervisionlist.ShowDropDownOnTextboxClick = false;
        ddlsupervisionlist.ShowToggleImage = false;
        foreach (RadComboBoxItem item in ddlsupervisionlist.Items)
        {
            item.Enabled = false;
        }
        ucMachineryStatus.ShowDropDownOnTextboxClick = false;
        ucMachineryStatus.ShowToggleImage = false;

        txtRefNo.CssClass = "input";
        txtRevNO.CssClass = "input";
        UCDayNight.CssClass = "input";
        ucCreatedDate.CssClass = "input";
        UCVisibility.CssClass = "input";
        txtpreparedby.CssClass = "input";
        txtIssuedBy.CssClass = "input";
        ucIssuedDate.CssClass = "input";
        ucSteering.CssClass = "input";
        ucCategory.CssClass = "input";
        UCWX.CssClass = "input";
        ddlCategory.CssClass = "input";
        UCUMSPermitted.CssClass = "input";
        ddlsupervisionlist.CssClass = "input";
        ucMachineryStatus.CssClass = "input";
        ucCompany.CssClass = "input";
        ucCompany.ShowDropDownOnTextboxClick = false;
        ucCompany.ShowToggleImage = false;
        ucCompany.Enabled = true;

        gvForms.Columns[1].Visible = false;
        gvworkpermit.Columns[2].Visible = false;
        gvEPSS.Columns[1].Visible = false;
        gvprocedure.Columns[1].Visible = false;
        gvImportedJHA.Columns[12].Visible = false;

        gvForms.ShowFooter = false;
        gvworkpermit.ShowFooter = false;
        gvEPSS.ShowFooter = false;
        gvprocedure.ShowFooter = false;

    }

    protected void BindCompany()
    {
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            DataSet ds = PhoenixInspectionRiskAssessmentProcessExtn.MappedVesselCompany(PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            if (ds.Tables[0].Rows.Count > 0)
                ucCompany.SelectedCompany = ds.Tables[0].Rows[0]["FLDCOMPANYID"].ToString();
        }
    }

    protected void BindCategory()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentActivity(General.GetNullableInteger(ViewState["PROCESSID"].ToString()));
        ddlCategory.Items.Clear();
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlCategory.DataSource = ds.Tables[0];
        ddlCategory.DataBind();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        RiskAssessmentProcessEdit();
        gvImportedJHA.Rebind();
        gvenvironmental.Rebind();
        gvhazards.Rebind();
        gvForms.Rebind();
        gvprocedure.Rebind();
        gvworkpermit.Rebind();
        gvEPSS.Rebind();
        gvPPE.Rebind();
        gvPPE2.Rebind();
        gvTeamComposition.Rebind();
        gvevent.Rebind();

        PhoenixToolbar toolbarsub1 = new PhoenixToolbar();
        toolbarsub1.AddFontAwesomeButton("javascript: openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionJHATemplateMappingExtn.aspx?RISKASSESSMENTPROCESSID=" + ViewState["RISKASSESSMENTPROCESSID"].ToString() + "')", "Import JHA", "<i class=\"fas fa-tasks\"></i>", "IMPORTJHA");
        MenuImportedJHA.AccessRights = this.ViewState;
        MenuImportedJHA.MenuList = toolbarsub1.Show();
    }

    protected void gvTeamComposition_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentJobHazardExtn.RAActivityTeamCompositionList(General.GetNullableInteger(ViewState["ACTIVITYID"].ToString()));
        gvTeamComposition.DataSource = ds.Tables[0];
    }

    protected void gvImportedJHA_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindImportedJHA();
    }

    private void BindImportedJHA()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentProcessExtn.ProcessRAJHAList(General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()));
        gvImportedJHA.DataSource = ds.Tables[0];
    }

    protected void gvImportedJHA_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadLabel lblHS = (RadLabel)e.Item.FindControl("lblHS");
                RadLabel lblENV = (RadLabel)e.Item.FindControl("lblENV");
                RadLabel lblECO = (RadLabel)e.Item.FindControl("lblECO");
                RadLabel lblWCS = (RadLabel)e.Item.FindControl("lblWCS");
                RadLabel lblmaxvalue = (RadLabel)e.Item.FindControl("lblmaxvalue");
                RadLabel lblminvalue = (RadLabel)e.Item.FindControl("lblminvalue");

                DataRowView dv = (DataRowView)e.Item.DataItem;

                decimal minscore = 0, maxscore = 0;

                if (!string.IsNullOrEmpty(dv["FLDMINVALUE"].ToString()))
                    minscore = decimal.Parse(dv["FLDMINVALUE"].ToString());

                if (!string.IsNullOrEmpty(dv["FLDMAXVALUE"].ToString()))
                    maxscore = decimal.Parse(dv["FLDMAXVALUE"].ToString());

                if (lblHS != null)
                {
                    lblHS.Attributes.Add("style", "font-weight:bold;");
                    if (!string.IsNullOrEmpty(lblHS.Text))
                    {
                        if (decimal.Parse(lblHS.Text) <= minscore)
                            lblHS.BackColor = System.Drawing.Color.Lime;
                        else if (decimal.Parse(lblHS.Text) > minscore && decimal.Parse(lblHS.Text) <= maxscore)
                            lblHS.BackColor = System.Drawing.Color.Yellow;
                        else if (decimal.Parse(lblHS.Text) > maxscore)
                            lblHS.BackColor = System.Drawing.Color.Red;
                    }
                }

                if (lblENV != null)
                {
                    lblENV.Attributes.Add("style", "font-weight:bold;");
                    if (!string.IsNullOrEmpty(lblENV.Text))
                    {
                        if (decimal.Parse(lblENV.Text) <= minscore)
                            lblENV.BackColor = System.Drawing.Color.Lime;
                        else if (decimal.Parse(lblENV.Text) > minscore && decimal.Parse(lblENV.Text) <= maxscore)
                            lblENV.BackColor = System.Drawing.Color.Yellow;
                        else if (decimal.Parse(lblENV.Text) > maxscore)
                            lblENV.BackColor = System.Drawing.Color.Red;
                    }
                }

                if (lblECO != null)
                {
                    lblECO.Attributes.Add("style", "font-weight:bold;");
                    if (!string.IsNullOrEmpty(lblECO.Text))
                    {
                        if (decimal.Parse(lblECO.Text) <= minscore)
                            lblECO.BackColor = System.Drawing.Color.Lime;
                        else if (decimal.Parse(lblECO.Text) > minscore && decimal.Parse(lblECO.Text) <= maxscore)
                            lblECO.BackColor = System.Drawing.Color.Yellow;
                        else if (decimal.Parse(lblECO.Text) > maxscore)
                            lblECO.BackColor = System.Drawing.Color.Red;
                    }
                }

                if (lblWCS != null)
                {
                    lblWCS.Attributes.Add("style", "font-weight:bold;");
                    if (!string.IsNullOrEmpty(lblWCS.Text))
                    {
                        if (decimal.Parse(lblWCS.Text) <= minscore)
                            lblWCS.BackColor = System.Drawing.Color.Lime;
                        else if (decimal.Parse(lblWCS.Text) > minscore && decimal.Parse(lblWCS.Text) <= maxscore)
                            lblWCS.BackColor = System.Drawing.Color.Yellow;
                        else if (decimal.Parse(lblWCS.Text) > maxscore)
                            lblWCS.BackColor = System.Drawing.Color.Red;
                    }
                }

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null)
                {
                    if (ViewState["STATUS"].ToString() == "3")
                    {
                        del.Visible = false;
                    }
                    else
                    {
                        del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                        del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvTeamComposition_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            GridDecorator.MergeRows(gvTeamComposition, e);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public class GridDecorator
    {
        public static void MergeRows(RadGrid gridView, GridItemEventArgs e)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                int index = e.Item.ItemIndex;
                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string currentDocumentName = ((RadLabel)gridView.Items[rowIndex].FindControl("lblDeparment")).Text;
                string previousDocumentName = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblDeparment")).Text;

                if (currentDocumentName == previousDocumentName)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                     previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }
            }
        }
    }
    protected void gvImportedJHA_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                if (e.CommandName.ToUpper().Equals("JHADELETE"))
                {
                    var editableItem = ((GridEditableItem)e.Item);

                    GridEditableItem eeditedItem = e.Item as GridEditableItem;

                    string lbljhaid = ((RadLabel)editableItem.FindControl("lbljhaid")).Text;
                    PhoenixInspectionRiskAssessmentProcessExtn.InspectionRiskAssessmentJHADelete(General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
                                                                                                 new Guid(lbljhaid.ToString()));
                    ucStatus.Text = "Imported JHA has been removed successfully.";

                    RiskAssessmentProcessEdit();
                    gvImportedJHA.Rebind();

                }

                if (e.CommandName.ToUpper().Equals("JHA"))
                {
                    RadLabel lbl = (RadLabel)e.Item.FindControl("lbljhaid");
                    Response.Redirect("../Inspection/InspectionRAJobHazardAnalysisExtn.aspx?ProcessRAYN=1&jobhazardid=" + lbl.Text + "&processid=" + ViewState["RISKASSESSMENTPROCESSID"].ToString() +"&PROCESSDASHBOARDYN=" + ViewState["HSEQADashboardYN"].ToString(), false);
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void RiskAssessmentProcessEdit()
    {
        DataSet dsProcess = PhoenixInspectionRiskAssessmentProcessExtn.EditInspectionRiskAssessmentProcess(
            General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()));

        if (dsProcess.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsProcess.Tables[0].Rows[0];
            txtRefNo.Text = dr["FLDNUMBER"].ToString();
            txtRevNO.Text = dr["FLDREVISIONNO"].ToString();
            txtpreparedby.Text = dr["FLDPREPAREDBYNAME"].ToString();
            ucCreatedDate.Text = dr["FLDPREPAREDDATE"].ToString();
            txtApprovedby.Text = dr["FLDAPPROVEDBYNAME"].ToString();
            ucApprovedDate.Text = dr["FLDAPPROVEDDATE"].ToString();
            txtIssuedBy.Text = dr["FLDISSUEDBYNAME"].ToString();
            ucIssuedDate.Text = dr["FLDISSUEDDATE"].ToString();

            ViewState["STATUS"] = dr["FLDSTATUS"].ToString();
            txtDate.Text = dr["FLDDATE"].ToString();

            txtActivity.Text = dr["FLDJOBACTIVITY"].ToString();
            txtProcess.Text = dr["FLDPROCESSNAME"].ToString();

            if (!string.IsNullOrEmpty(dr["FLDPROCESSID"].ToString()))
            {
                ucCategory.SelectedValue = dr["FLDPROCESSID"].ToString();
                ViewState["PROCESSID"] = dr["FLDPROCESSID"].ToString();
                BindCategory();
            }

            if (!string.IsNullOrEmpty(dr["FLDACTIVITYID"].ToString()))
            {
                if (ddlCategory.Items.FindItemByValue(dr["FLDACTIVITYID"].ToString()) != null)
                    ddlCategory.SelectedValue = dr["FLDACTIVITYID"].ToString();
            }

            ViewState["ACTIVITYID"] = dr["FLDACTIVITYID"].ToString();


            if (!string.IsNullOrEmpty(dr["FLDCOMPANYID"].ToString()))
            {
                ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
                ucCompany.Enabled = false;
            }
            else
                ucCompany.Enabled = true;

            ViewState["companyid"] = dr["FLDCOMPANYID"].ToString();

            DataTable dt = new DataTable();
            dt = dsProcess.Tables[1];

            chkImportedJHAList.DataSource = dt;
            chkImportedJHAList.DataBindings.DataTextField = "FLDHAZARDNUMBER";
            chkImportedJHAList.DataBindings.DataValueField = "FLDJOBHAZARDID";
            chkImportedJHAList.DataBind();
            foreach (ButtonListItem chkitem in chkImportedJHAList.Items)
                chkitem.Selected = true;
            ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();

            if (!string.IsNullOrEmpty(dr["FLDDAYNIGHT"].ToString()))
            {
                UCDayNight.SelectedQuick = dr["FLDDAYNIGHT"].ToString();
            }

            if (!string.IsNullOrEmpty(dr["FLDWX"].ToString()))
            {
                UCWX.SelectedQuick = dr["FLDWX"].ToString();
            }

            if (!string.IsNullOrEmpty(dr["FLDVISIBILITY"].ToString()))
            {
                UCVisibility.SelectedQuick = dr["FLDVISIBILITY"].ToString();
            }

            if (!string.IsNullOrEmpty(dr["FLDUMS"].ToString()))
            {
                UCUMSPermitted.SelectedQuick = dr["FLDUMS"].ToString();
            }

            if (!string.IsNullOrEmpty(dr["FLDSTEERING"].ToString()))
            {
                ucSteering.SelectedValue = dr["FLDSTEERING"].ToString();
            }

            if (!string.IsNullOrEmpty(dr["FLDMACHINERYSTATUS"].ToString()))
            {
                ucMachineryStatus.SelectedQuick = dr["FLDMACHINERYSTATUS"].ToString();
            }

            if (dr["FLDSTATUS"].ToString() == "3")
            {
                CommentsLink();
            }
            ddlsupervisionlist.SelectedValue = dr["FLDSUPERVISIONID"].ToString();
            gvTeamComposition.Rebind();

            lblLevelofRiskHealth.Text = dr["FLDHSLR"].ToString();
            lblLevelofRiskEnv.Text = dr["FLDENVLR"].ToString();
            lblLevelofRiskEconomic.Text = dr["FLDECOLR"].ToString();
            lblLevelofRiskWorst.Text = dr["FLDWCLR"].ToString();
            lblhscontrols.Text = dr["FLDSUPERVISIONCHECKLISTSCORE"].ToString();
            lblencontrols.Text = dr["FLDSUPERVISIONCHECKLISTSCORE"].ToString();
            lbleccontrols.Text = dr["FLDSUPERVISIONCHECKLISTSCORE"].ToString();
            lblwscontrols.Text = dr["FLDSUPERVISIONCHECKLISTSCORE"].ToString();
            lblreshsrisk.Text = dr["FLDHSRR"].ToString();
            lblresenrisk.Text = dr["FLDENVRR"].ToString();
            lblresecorisk.Text = dr["FLDECORR"].ToString();
            lblreswsrisk.Text = dr["FLDWCRR"].ToString();
            lblHealthDescription.Text = dr["FLDHSRISKLEVELTEXT"].ToString();
            lblEnvDescription.Text = dr["FLDENVRISKLEVELTEXT"].ToString();
            lblEconomicDescription.Text = dr["FLDECORISKLEVELTEXT"].ToString();
            lblWorstDescription.Text = dr["FLDWSRISKLEVELTEXT"].ToString();
            lblimpacthealth.Text = dr["FLDHSIMP"].ToString();
            lblimpacteco.Text = dr["FLDECOIMP"].ToString();
            lblimpactenv.Text = dr["FLDENVIMP"].ToString();
            lblimpactws.Text = dr["FLDWSIMP"].ToString();

            lblPOOhealth.Text = dr["FLDHSPOO"].ToString();
            lblPOOeco.Text = dr["FLDECOPOO"].ToString();
            lblPOOenv.Text = dr["FLDENVPOO"].ToString();
            lblPOOws.Text = dr["FLDWSPOO"].ToString();

            lbllohhealth.Text = dr["FLDHSLOH"].ToString();
            lblloheco.Text = dr["FLDECOLOH"].ToString();
            lbllohenv.Text = dr["FLDENVLOH"].ToString();
            lbllohws.Text = dr["FLDWSLOH"].ToString();

            lblControlshealth.Text = dr["FLDHSLOC"].ToString();
            lblControlseco.Text = dr["FLDECOLOC"].ToString();
            lblControlsenv.Text = dr["FLDENVLOC"].ToString();
            lblControlsws.Text = dr["FLDWSLOC"].ToString();


            decimal minscore = 0, maxscore = 0;

            if (!string.IsNullOrEmpty(dr["FLDMINSCORE"].ToString()))
                minscore = decimal.Parse(dr["FLDMINSCORE"].ToString());

            if (!string.IsNullOrEmpty(dr["FLDMAXSCORE"].ToString()))
                maxscore = decimal.Parse(dr["FLDMAXSCORE"].ToString());

            lblLevelofRiskHealth.Text = dr["FLDHSLR"].ToString();

            lblreshsrisk.Text = dr["FLDHSRR"].ToString();

            if (!string.IsNullOrEmpty(lblreshsrisk.Text))
            {
                if (decimal.Parse(lblreshsrisk.Text) <= minscore)
                    tdreshsrisk.BgColor = "Lime";
                else if (decimal.Parse(lblreshsrisk.Text) > minscore && decimal.Parse(lblreshsrisk.Text) <= maxscore)
                    tdreshsrisk.BgColor = "Yellow";
                else if (decimal.Parse(lblreshsrisk.Text) > maxscore)
                    tdreshsrisk.BgColor = "Red";
            }
            else
                tdreshsrisk.BgColor = "White";

            lblresenrisk.Text = dr["FLDENVRR"].ToString();

            if (!string.IsNullOrEmpty(lblresenrisk.Text))
            {
                if (decimal.Parse(lblresenrisk.Text) <= minscore)
                    tdresenrisk.BgColor = "Lime";
                else if (decimal.Parse(lblresenrisk.Text) > minscore && decimal.Parse(lblresenrisk.Text) <= maxscore)
                    tdresenrisk.BgColor = "Yellow";
                else if (decimal.Parse(lblresenrisk.Text) > maxscore)
                    tdresenrisk.BgColor = "Red";
            }
            else
                tdresenrisk.BgColor = "White";

            lblresecorisk.Text = dr["FLDECORR"].ToString();

            if (!string.IsNullOrEmpty(lblresecorisk.Text))
            {
                if (decimal.Parse(lblresecorisk.Text) <= minscore)
                    tdresecorisk.BgColor = "Lime";
                else if (decimal.Parse(lblresecorisk.Text) > minscore && decimal.Parse(lblresecorisk.Text) <= maxscore)
                    tdresecorisk.BgColor = "Yellow";
                else if (decimal.Parse(lblresecorisk.Text) > maxscore)
                    tdresecorisk.BgColor = "Red";
            }
            else
                tdresecorisk.BgColor = "White";

            lblreswsrisk.Text = dr["FLDWCRR"].ToString();

            if (!string.IsNullOrEmpty(lblreswsrisk.Text))
            {
                if (decimal.Parse(lblreswsrisk.Text) <= minscore)
                    tdreswsrisk.BgColor = "Lime";
                else if (decimal.Parse(lblreswsrisk.Text) > minscore && decimal.Parse(lblreswsrisk.Text) <= maxscore)
                    tdreswsrisk.BgColor = "Yellow";
                else if (decimal.Parse(lblreswsrisk.Text) > maxscore)
                    tdreswsrisk.BgColor = "Red";
            }
            else
                tdreswsrisk.BgColor = "White";

            BindMenu();
            gvPPE.Rebind();
            gvPPE2.Rebind();
        }
    }
    protected void MenuProcess_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;

        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            SaveRiskAssessmentProcess();
            lnkImportJHA.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionJHATemplateMappingExtn.aspx?RISKASSESSMENTPROCESSID=" + ViewState["RISKASSESSMENTPROCESSID"] + "');return false;");
        }
        if (CommandName.ToUpper().Equals("APPROVE"))
        {
            PhoenixInspectionRiskAssessmentProcessExtn.UpdateRiskAssessmentProcessApproval(
                                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString())
                                                                                        , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                        , null
                                                                                        , 2);
            RiskAssessmentProcessEdit();
            ucStatus.Text = "Process Template Approved.";
        }
        if (CommandName.ToUpper().Equals("ISSUE"))
        {
            PhoenixInspectionRiskAssessmentProcessExtn.UpdateRiskAssessmentProcessApproval(
                                                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                       , new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString())
                                                                                       , null
                                                                                       , PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                                       , 3);
            RiskAssessmentProcessEdit();
            ucStatus.Text = "Process Template Issued.";
        }
        if (CommandName.ToUpper().Equals("LIST"))
        {
            if (ViewState["HSEQADashboardYN"].ToString().Equals("1"))
            {

                Response.Redirect("../Inspection/InspectionDashboardMainFleetRAProcessListExtn.aspx");

            }
            else
            {
                Response.Redirect("../Inspection/InspectionMainFleetRAProcessListExtn.aspx", false);
            }
        }
        if (CommandName.ToUpper().Equals("BACK"))
        {
            if ((ViewState["RATYPE"].ToString() == "1") && (ViewState["mocextention"].ToString() == ""))
            {
                Response.Redirect("../Inspection/InspectionMOCRequestAdd.aspx?MOCRequestid=" + ViewState["MOCRequestid"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString() + "&RiskAssessmentid=" + ViewState["RISKASSESSMENTPROCESSID"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString(), false);
            }
            if ((ViewState["RATYPE"].ToString() == "1") && (ViewState["mocextention"].ToString() == "yes"))
            {
                Response.Redirect("../Inspection/InspectionMOCIntermediateVerificationAdd.aspx?MOCRequestid=" + ViewState["MOCRequestid"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString() + "&RiskAssessmentid=" + ViewState["RISKASSESSMENTPROCESSID"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString(), false);
            }
        }
        gvImportedJHA.Rebind();
    }


    private void SaveRiskAssessmentProcess()
    {
        try
        {
            string recorddate = txtDate.Text;

            if (!IsValidProcessTemplate())
            {
                ucError.Visible = true;
                return;
            }
            int? processid = General.GetNullableInteger(ddlCategory.SelectedValue);

            Guid? riskassessmentprocessidout = Guid.NewGuid();
            PhoenixInspectionRiskAssessmentProcessExtn.InsertInspectionRiskAssessmentProcess(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
                General.GetNullableDateTime(recorddate),
                General.GetNullableInteger(ddlCategory.SelectedValue),
                General.GetNullableString(txtActivity.Text),
                (processid != null ? processid.ToString() : null),
                ref riskassessmentprocessidout,
                General.GetNullableInteger(ucCompany.SelectedCompany),
                General.GetNullableInteger(UCDayNight.SelectedQuick),
            General.GetNullableInteger(UCWX.SelectedQuick),
            General.GetNullableInteger(UCVisibility.SelectedQuick),
            General.GetNullableInteger(UCUMSPermitted.SelectedQuick),
            General.GetNullableInteger(ucSteering.SelectedQuick),
            General.GetNullableInteger(ucMachineryStatus.SelectedQuick),
            General.GetNullableInteger(ddlsupervisionlist.SelectedValue));

            ViewState["companyid"] = ucCompany.SelectedCompany;


            ViewState["RISKASSESSMENTPROCESSID"] = riskassessmentprocessidout.ToString();
            Filter.CurrentSelectedProcessRA = riskassessmentprocessidout.ToString(); ;
            RiskAssessmentProcessEdit();
            ucStatus.Text = "Process Template updated.";
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

        if (General.GetNullableInteger(ucCompany.SelectedCompany) == null)
            ucError.ErrorMessage = "Company is required.";

        if (General.GetNullableString(ucCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Process is required.";

        if (General.GetNullableString(ddlCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Activity is required.";

        return (!ucError.IsError);
    }


    protected void chkImportedJHAList_Changed(object sender, EventArgs e)
    {
        StringBuilder strjhaid = new StringBuilder();
        foreach (ButtonListItem item in chkImportedJHAList.Items)
        {
            if (item.Selected == true && item.Enabled == true)
            {
                strjhaid.Append(item.Value.ToString());
                strjhaid.Append(",");
            }
        }

        if (strjhaid.Length > 1)
        {
            strjhaid.Remove(strjhaid.Length - 1, 1);
        }
        string jhaid = strjhaid.ToString();
        PhoenixInspectionRiskAssessmentProcessExtn.InspectionRiskAssessmentJHAImport(General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
                                                                                     jhaid);
        ucStatus.Text = "Imported JHA has been removed successfully.";
        cmdHiddenSubmit_Click(sender, new EventArgs());
    }

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void ucCategory_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["PROCESSID"] = ucCategory.SelectedValue;
        ddlCategory.ClearSelection();
        BindCategory();
    }
    private void BindSupervisionLevel()
    {
        ddlsupervisionlist.DataTextField = "FLDGROUPRANK";
        ddlsupervisionlist.DataValueField = "FLDGROUPRANKID";
        ddlsupervisionlist.DataSource = PhoenixRegistersGroupRank.ListJHAGroupRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlsupervisionlist.Items.Insert(0, new RadComboBoxItem("Not Required", "Dummy"));
        ddlsupervisionlist.DataBind();
    }
    protected void gvForms_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet dss = PhoenixInspectionRiskAssessmentProcessExtn.FormPosterList(ViewState["RISKASSESSMENTPROCESSID"] == null ? null : General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), General.GetNullableInteger("0"));
        gvForms.DataSource = dss;
    }

    protected void gvForms_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;

            Label lblFormId = (Label)e.Item.FindControl("lblFormId");
            Label lbltype = (Label)e.Item.FindControl("lbltype");
            LinkButton lblName = (LinkButton)e.Item.FindControl("lblName");
            if (lblFormId != null)
            {

                if ((lbltype.Text == "5") && (lblName != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + lblFormId.Text + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "');return false;");
                }
                if ((lbltype.Text == "6") && (lblName != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Common/download.aspx?formid=" + lblFormId.Text + "');return false;");
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton btnShowDocuments = (LinkButton)e.Item.FindControl("btnShowDocuments");
            if (btnShowDocuments != null)
            {
                btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["QUALITYCOMPANYID"].ToString() + "', true); ");
            }
        }
    }

    protected void gvevent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionOperationalRiskControls.ListProcessRiskAssessmentworstcase(General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()));
            gvevent.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvevent_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                if (del != null)
                {
                    del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }

                RadLabel lblWorstcaseid = (RadLabel)e.Item.FindControl("lblWorstcaseid");
                HtmlTable tblForms = (HtmlTable)e.Item.FindControl("tblProcedures");
                if (lblWorstcaseid != null)
                {
                    DataSet dss = PhoenixInspectionRiskAssessmentJobHazardExtn.UndesirableeventFormsandposterList(General.GetNullableGuid(lblWorstcaseid.Text));
                    int number = 1;
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        tblForms.Rows.Clear();
                        foreach (DataRow dr in dss.Tables[0].Rows)
                        {
                            HyperLink hl = new HyperLink();
                            hl.Text = dr["FLDNAME"].ToString();
                            hl.ID = "hlink" + number.ToString();
                            hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                            string type = dr["FLDTYPE"].ToString();

                            if (type == "2")
                                hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");

                            else if (type == "3")
                                hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");

                            else if (type == "5")
                            {
                                hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + dr["FLDFORMREVISIONID"].ToString() + "');return false;");
                            }
                            else if (type == "6")
                            {
                                hl.Target = "_blank";
                                hl.NavigateUrl = "../Common/download.aspx?formid=" + dr["FLDFORMPOSTERID"].ToString();
                            }

                            HtmlTableRow tr = new HtmlTableRow();
                            HtmlTableCell tc = new HtmlTableCell();
                            tr.Cells.Add(tc);
                            tc = new HtmlTableCell();
                            tc.Controls.Add(hl);
                            tr.Cells.Add(tc);
                            tblForms.Rows.Add(tr);
                            tc = new HtmlTableCell();
                            tc.InnerHtml = "<br/>";
                            tr.Cells.Add(tc);
                            tblForms.Rows.Add(tr);
                            number = number + 1;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvevent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    gvevent.EditIndexes.Clear();
                }
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    e.Item.OwnerTableView.IsItemInserted = false;
                }

                if (e.CommandName.ToUpper().Equals("UDELETE"))
                {

                    string WorstCaseid = ((RadLabel)editableItem.FindControl("lblWorstCaseid")).Text;
                    PhoenixInspectionOperationalRiskControls.DeleteRiskAssessmentWorstCase(new Guid(WorstCaseid));

                    gvevent.Rebind();
                    ucStatus.Text = "Hazard  Deleted.";
                }

                else if (e.CommandName.ToUpper() == "EMAPPING")
                {
                    //GridDataItem item = (GridDataItem)e.Item;
                    //string id = item.GetDataKeyValue("FLDWORSTCASEID").ToString();
                    //ViewState["EVENTID"] = item.GetDataKeyValue("FLDWORSTCASEID").ToString();
                    //string script = "function sd(){showDialog('Controls Mapping'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest('" + id + ",EVENTEDIT');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    //ViewState["RequestFrom"] = "EVENT";
                }

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidWorstCase(string Worstcase)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Template first and then try adding the Hazard.";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()) != null)
        {
            if (General.GetNullableInteger(Worstcase) == null)
                ucError.ErrorMessage = "Worst Case is required.";
        }

        return (!ucError.IsError);

    }

    protected void gvprocedure_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet dss = PhoenixInspectionRiskAssessmentProcessExtn.FormPosterList(ViewState["RISKASSESSMENTPROCESSID"] == null ? null : General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), General.GetNullableInteger("1"));
        gvprocedure.DataSource = dss;
    }

    protected void gvprocedure_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            Label lblFormId = (Label)e.Item.FindControl("lblFormId");
            Label lbltype = (Label)e.Item.FindControl("lbltype");
            LinkButton lblName = (LinkButton)e.Item.FindControl("lblName");
            HtmlTable tblForms = (HtmlTable)e.Item.FindControl("tblForms");
            if (lblFormId != null)
            {
                if (lbltype.Text == "2")
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + lblFormId.Text + "');return false;");
                else if (lbltype.Text == "3")
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + lblFormId.Text + "');return false;");

            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton btnShowDocuments = (LinkButton)e.Item.FindControl("btnShowDocuments1");
            if (btnShowDocuments != null)
            {
                btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument1', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["QUALITYCOMPANYID"].ToString() + "', true); ");
            }
        }
    }

    protected void gvPPE_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if ((ViewState["RISKASSESSMENTPROCESSID"] != null) && (ViewState["RISKASSESSMENTPROCESSID"].ToString() != string.Empty))
        {
            DataSet DS = PhoenixInspectionRiskAssessmentHazardExtn.ListProcessRAPPEWithMoreIcons(new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()));
            gvPPE.DataSource = DS.Tables[0];
            gvPPE2.DataSource = DS.Tables[1];
        }
    }

    protected void gvPPE_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image img = (Image)e.Item.FindControl("imgPhoto");
            img.Attributes.Add("src", drv["FLDIMAGE"].ToString());
        }
    }

    protected void gvworkpermit_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if ((ViewState["RISKASSESSMENTPROCESSID"] != null) && (ViewState["RISKASSESSMENTPROCESSID"].ToString() != string.Empty))
        {
            DataTable DT = PhoenixInspectionRiskAssessmentHazardExtn.ListProcessRAWorkPermitWithIcons(new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()));
            gvworkpermit.DataSource = DT;
        }
    }

    protected void gvworkpermit_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image img = (Image)e.Item.FindControl("imgPhoto");
            img.Attributes.Add("src", drv["FLDIMAGE"].ToString());

            Label lblFormId = (Label)e.Item.FindControl("lblFormId");
            Label lbltype = (Label)e.Item.FindControl("lbltype");
            LinkButton lblName = (LinkButton)e.Item.FindControl("lblName");
            if (lblFormId != null)
            {

                if ((lbltype.Text != "1") && (lblName != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + lblFormId.Text + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "');return false;");
                }
                if ((lbltype.Text == "1") && (lblName != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Common/download.aspx?formid=" + lblFormId.Text + "');return false;");
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton btnShowDocuments = (LinkButton)e.Item.FindControl("btnShowDocuments5");
            if (btnShowDocuments != null)
            {
                btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument5', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["QUALITYCOMPANYID"].ToString() + "', true); ");
            }
        }
    }

    protected void gvEPSS_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if ((ViewState["RISKASSESSMENTPROCESSID"] != null) && (ViewState["RISKASSESSMENTPROCESSID"].ToString() != string.Empty))
        {
            DataTable DT = PhoenixInspectionRiskAssessmentHazardExtn.ListProcessRAEPPSWithIcons(new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()));
            gvEPSS.DataSource = DT;
        }
    }

    protected void gvEPSS_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;

            Label lblFormId = (Label)e.Item.FindControl("lblFormId");
            Label lbltype = (Label)e.Item.FindControl("lbltype");
            LinkButton lblName = (LinkButton)e.Item.FindControl("lblName");
            if (lblFormId != null)
            {

                if ((lbltype.Text != "1") && (lblName != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + lblFormId.Text + "&FORMREVISIONID=" + drv["FLDFORMREVISIONID"].ToString() + "');return false;");
                }
                if ((lbltype.Text == "1") && (lblName != null))
                {
                    lblName.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Common/download.aspx?formid=" + lblFormId.Text + "');return false;");
                }
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton btnShowDocuments = (LinkButton)e.Item.FindControl("btnShowDocuments3");
            if (btnShowDocuments != null)
            {
                btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument3', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["QUALITYCOMPANYID"].ToString() + "', true); ");
            }
        }
    }

    protected void gvhazards_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if ((ViewState["RISKASSESSMENTPROCESSID"] != null) && (ViewState["RISKASSESSMENTPROCESSID"].ToString() != string.Empty))
        {
            DataTable DT = PhoenixInspectionRiskAssessmentHazardExtn.ListProcessRAHazardWithIcons(1,
               new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()));
            gvhazards.DataSource = DT;

        }
    }

    protected void gvhazards_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image img = (Image)e.Item.FindControl("imgPhoto");
            img.Attributes.Add("src", drv["FLDIMAGE"].ToString());
        }
    }

    protected void gvenvironmental_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        if ((ViewState["RISKASSESSMENTPROCESSID"] != null) && (ViewState["RISKASSESSMENTPROCESSID"].ToString() != string.Empty))
        {
            DataTable DT = PhoenixInspectionRiskAssessmentHazardExtn.ListProcessRAHazardWithIcons(2,
               new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()));
            gvenvironmental.DataSource = DT;

        }
    }

    protected void gvenvironmental_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image img = (Image)e.Item.FindControl("imgPhoto");
            img.Attributes.Add("src", drv["FLDIMAGE"].ToString());
        }
    }

    protected void gvForms_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("FORMADD"))
            {
                if (ViewState["RISKASSESSMENTPROCESSID"] == null || ViewState["RISKASSESSMENTPROCESSID"].ToString() == string.Empty)
                {
                    ucError.ErrorMessage = "Please save the Template first and then try adding the Forms and Checklists.";
                    ucError.Visible = true;
                    return;
                }

                RadTextBox txtDocumentId = ((RadTextBox)e.Item.FindControl("txtDocumentId"));
                RadTextBox txtDocumentName = ((RadTextBox)e.Item.FindControl("txtDocumentName"));

                if (General.GetNullableGuid(txtDocumentId.Text) != null)
                {
                    PhoenixInspectionRiskAssessmentProcessExtn.UpdateRiskAssessmentProcessForms(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), new Guid(txtDocumentId.Text));

                    ucStatus.Text = "Forms & Checklists added.";
                    txtDocumentId.Text = "";
                    txtDocumentName.Text = "";
                    RiskAssessmentProcessEdit();
                    gvworkpermit.Rebind();
                    gvForms.Rebind();
                }
                else
                {
                    ucError.ErrorMessage = "Please select the Forms & Checklists.";
                    ucError.Visible = true;
                    return;
                }
            }
            if (e.CommandName.ToUpper().Equals("FORMDELETE"))
            {
                Label lblFormId = ((Label)e.Item.FindControl("lblFormId"));

                PhoenixInspectionRiskAssessmentProcessExtn.FormPosterDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), new Guid(lblFormId.Text), int.Parse("0"));

                string txt = "Forms & Checklists";

                ucStatus.Text = txt + " deleted.";
                RiskAssessmentProcessEdit();
                gvworkpermit.Rebind();
                gvForms.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvworkpermit_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("WORKPERMITADD"))
            {
                if (ViewState["RISKASSESSMENTPROCESSID"] == null || ViewState["RISKASSESSMENTPROCESSID"].ToString() == string.Empty)
                {
                    ucError.ErrorMessage = "Please save the Template first and then try adding the Work Permit.";
                    ucError.Visible = true;
                    return;
                }

                RadTextBox txtDocumentId = ((RadTextBox)e.Item.FindControl("txtDocumentId5"));
                RadTextBox txtDocumentName = ((RadTextBox)e.Item.FindControl("txtDocumentName5"));

                if (General.GetNullableGuid(txtDocumentId.Text) != null)
                {
                    PhoenixInspectionRiskAssessmentProcessExtn.UpdateRiskAssessmentProcessForms(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), new Guid(txtDocumentId.Text));

                    ucStatus.Text = "Work Permit added.";
                    txtDocumentId.Text = "";
                    txtDocumentName.Text = "";
                    RiskAssessmentProcessEdit();
                    gvworkpermit.Rebind();
                    gvForms.Rebind();
                }
                else
                {
                    ucError.ErrorMessage = "Please select the Work Permit.";
                    ucError.Visible = true;
                    return;
                }
            }
            if (e.CommandName.ToUpper().Equals("WORKPERMITDELETE"))
            {
                Label lblFormId = ((Label)e.Item.FindControl("lblFormId"));

                PhoenixInspectionRiskAssessmentProcessExtn.FormPosterDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), new Guid(lblFormId.Text), int.Parse("0"));

                string txt = "Work Permit";

                ucStatus.Text = txt + " deleted.";
                RiskAssessmentProcessEdit();
                gvworkpermit.Rebind();
                gvForms.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEPSS_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EPSSADD"))
            {
                if (ViewState["RISKASSESSMENTPROCESSID"] == null || ViewState["RISKASSESSMENTPROCESSID"].ToString() == string.Empty)
                {
                    ucError.ErrorMessage = "Please save the Template first and then try adding the EPSS.";
                    ucError.Visible = true;
                    return;
                }

                RadTextBox txtDocumentId = ((RadTextBox)e.Item.FindControl("txtDocumentId3"));
                RadTextBox txtDocumentName = ((RadTextBox)e.Item.FindControl("txtDocumentName3"));

                if (General.GetNullableGuid(txtDocumentId.Text) != null)
                {
                    PhoenixInspectionRiskAssessmentProcessExtn.UpdateRiskAssessmentProcessEPSS(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), new Guid(txtDocumentId.Text));

                    ucStatus.Text = "EPSS added.";
                    txtDocumentId.Text = "";
                    txtDocumentName.Text = "";
                    RiskAssessmentProcessEdit();
                    gvEPSS.Rebind();
                }
                else
                {
                    ucError.ErrorMessage = "Please select the EPSS.";
                    ucError.Visible = true;
                    return;
                }
            }
            if (e.CommandName.ToUpper().Equals("EPSSDELETE"))
            {
                Label lblFormId = ((Label)e.Item.FindControl("lblFormId"));

                PhoenixInspectionRiskAssessmentProcessExtn.FormPosterDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), new Guid(lblFormId.Text), int.Parse("2"));

                string txt = "EPSS";

                ucStatus.Text = txt + " deleted.";
                RiskAssessmentProcessEdit();
                gvEPSS.Rebind();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvprocedure_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("PROCEDUREADD"))
        {
            try
            {
                if (ViewState["RISKASSESSMENTPROCESSID"] == null || ViewState["RISKASSESSMENTPROCESSID"].ToString() == string.Empty)
                {
                    ucError.ErrorMessage = "Please save the Template first and then try adding the Procedure.";
                    ucError.Visible = true;
                    return;
                }

                RadTextBox txtDocumentId = ((RadTextBox)e.Item.FindControl("txtDocumentId1"));
                RadTextBox txtDocumentName = ((RadTextBox)e.Item.FindControl("txtDocumentName1"));

                if (General.GetNullableGuid(txtDocumentId.Text) != null)
                {
                    PhoenixInspectionRiskAssessmentProcessExtn.UpdateRiskAssessmentProcessProcedures(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), new Guid(txtDocumentId.Text));

                    ucStatus.Text = "Procedure added.";
                    txtDocumentId.Text = "";
                    txtDocumentName.Text = "";
                    RiskAssessmentProcessEdit();
                    gvprocedure.Rebind();
                }
                else
                {
                    ucError.ErrorMessage = "Please select the Procedure.";
                    ucError.Visible = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }

        }
        if (e.CommandName.ToUpper().Equals("PROCEDUREDELETE"))
        {
            try
            {
                Label lblFormId = ((Label)e.Item.FindControl("lblFormId"));

                PhoenixInspectionRiskAssessmentProcessExtn.FormPosterDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["RISKASSESSMENTPROCESSID"].ToString()), new Guid(lblFormId.Text), int.Parse("1"));

                string txt = "Procedure";

                ucStatus.Text = txt + " deleted.";
                RiskAssessmentProcessEdit();
                ViewState["RISKASSESSMENTPROCESSID"] = Request.QueryString["processid"].ToString();
                RiskAssessmentProcessEdit();
                gvprocedure.Rebind();
            }

            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
                return;
            }
        }
    }

    protected void gvPPE2_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            Image img = (Image)e.Item.FindControl("imgPhoto");
            img.Attributes.Add("src", drv["FLDIMAGE"].ToString());
        }
    }
}
