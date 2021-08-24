using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Integration;
using SouthNests.Phoenix.Registers;

public partial class InspectionRAMachineryExtn : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        txtComponentId.Attributes.Add("style", "display:none");


        if (!IsPostBack)
        {
            DateTime dt = DateTime.Today;
            txtDate.Text = dt.ToString();
            ViewState["RISKASSESSMENTMACHINERYID"] = "";
            ViewState["IMPACTID"] = "";
            ViewState["RequestFrom"] = "";
            ViewState["EVENTID"] = "";
            ViewState["ISPOSTPONE"] = "";
            ViewState["StandardRA"] = "0";
            ViewState["ACTIVITYID"] = "";
            ViewState["ACTIVITYLIST"] = "";
            ViewState["StandardRA"] = string.IsNullOrEmpty(Request.QueryString["StandardRA"]) ? "" : Request.QueryString["StandardRA"];

            ViewState["ISPOSTPONE"] = string.IsNullOrEmpty(Request.QueryString["IsPostpone"]) ? "" : Request.QueryString["IsPostpone"];
            ViewState["VESSELID"] = "";
            ViewState["QUALITYCOMPANYID"] = "";
            ViewState["WORKORDERID"] = "";
            ViewState["WORKORDERGROUPID"] = string.Empty;
            ViewState["WORKORDERID"] = string.IsNullOrEmpty(Request.QueryString["WORKORDERID"]) ? "" : Request.QueryString["WORKORDERID"];
            ViewState["WORKORDERGROUPID"] = string.IsNullOrEmpty(Request.QueryString["WORKORDERGROUPID"]) ? "" : Request.QueryString["WORKORDERGROUPID"];
            ViewState["COMPONENTID"] = string.IsNullOrEmpty(Request.QueryString["COMPONENTID"]) ? "" : Request.QueryString["COMPONENTID"];
            ViewState["PAGENUMBER"] = string.IsNullOrEmpty(Request.QueryString["PAGENUMBER"]) ? 1 : int.Parse(Request.QueryString["PAGENUMBER"]);
            ViewState["COMPONENTJOBID"] = string.IsNullOrEmpty(Request.QueryString["COMPONENTJOBID"]) ? "" : Request.QueryString["COMPONENTJOBID"];

            ViewState["MOCRequestid"] = string.IsNullOrEmpty(Request.QueryString["MOCRequestid"]) ? "" : Request.QueryString["MOCRequestid"];
            ViewState["MOCID"] = string.IsNullOrEmpty(Request.QueryString["MOCID"]) ? "" : Request.QueryString["MOCID"];
            ViewState["RATYPE"] = string.IsNullOrEmpty(Request.QueryString["RAType"]) ? "" : Request.QueryString["RAType"];
            ViewState["mocextention"] = string.IsNullOrEmpty(Request.QueryString["mocextention"]) ? "" : Request.QueryString["mocextention"];
            ViewState["VESSELID"] = string.IsNullOrEmpty(Request.QueryString["Vesselid"]) ? "" : Request.QueryString["Vesselid"];
            ViewState["Vesselname"] = string.IsNullOrEmpty(Request.QueryString["Vesselname"]) ? "" : Request.QueryString["Vesselname"];
            ViewState["FLDJOBID"] = "";
            ViewState["FromWorkorder"] = string.IsNullOrEmpty(Request.QueryString["FromWorkorder"]) ? "" : Request.QueryString["FromWorkorder"];
            ViewState["RaCreatedIn"] = string.IsNullOrEmpty(Request.QueryString["RaCreatedIn"]) ? "" : Request.QueryString["RaCreatedIn"];
            ViewState["PMSRAAPPROVAL"] = string.IsNullOrEmpty(Request.QueryString["PMSApproval"]) ? "" : Request.QueryString["PMSApproval"];
            ViewState["FromWorkorderGroup"] = string.IsNullOrEmpty(Request.QueryString["FromWorkorderGroup"]) ? "" : Request.QueryString["FromWorkorderGroup"];
            ViewState["status"] = string.IsNullOrEmpty(Request.QueryString["status"]) ? "" : Request.QueryString["status"];

            ViewState["DefectId"] = string.IsNullOrEmpty(Request.QueryString["DefectId"]) ? "" : Request.QueryString["DefectId"];
            ViewState["DueDate"] = string.IsNullOrEmpty(Request.QueryString["DueDate"]) ? "" : Request.QueryString["DueDate"];
            ViewState["DashboardYN"] = string.IsNullOrEmpty(Request.QueryString["DashboardYN"]) ? "" : Request.QueryString["DashboardYN"];
            BindRecomendedPPE();


            if (ViewState["RATYPE"].ToString() == "4")
            {
                txtVessel.Text = ViewState["Vesselname"].ToString();
                PhoenixSecurityContext.CurrentSecurityContext.VesselID = int.Parse(ViewState["VESSELID"].ToString());
            }
            else
            {
                txtVessel.Text = PhoenixSecurityContext.CurrentSecurityContext.VesselName;
            }

            ViewState["QUALITYCOMPANYID"] = "-1";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["QUALITYCOMPANYID"] = nvc.Get("QMS");
                ucCompany.SelectedCompany = ViewState["QUALITYCOMPANYID"].ToString();
                ucCompany.Enabled = false;
            }
            else
                ucCompany.Enabled = true;

            BindData();
            if (Request.QueryString["machineryid"] != null)
            {
                ViewState["RISKASSESSMENTMACHINERYID"] = Request.QueryString["machineryid"].ToString();
                RiskAssessmentMachineryEdit();
            }

            cmdJobDetail.Attributes.Add("onclick", "javascript:parent.Openpopup('codehelp','','../PlannedMaintenance/PlannedMaintenanceComponentJobDetail.aspx?JOBID=" + ViewState["FLDJOBID"].ToString() + "');return false;");

            if (Request.QueryString["HSEQADashboardYN"] != null)
                ViewState["HSEQADashboardYN"] = Request.QueryString["HSEQADashboardYN"].ToString();
            else
                ViewState["HSEQADashboardYN"] = "0";

        }

        BindComapany();
        BindMenu();
        BindComponents();
        BindFormPosters();
        BindDocumentProcedures();
    }

    protected void CommentsLink()
    {
        lnkcomment.Visible = true;
        lnkaspectcomment.Visible = true;
        lnkhealthcomments.Visible = true;
        lnkEnvironmental.Visible = true;
        lnkEconomic.Visible = true;
        lnkUndesireable.Visible = true;
        lnkPersonal.Visible = true;
        lnkCommissioning.Visible = true;

        gvRAOperationalHazard.Columns[5].Visible = false;
        gvHealthSafetyRisk.Columns[10].Visible = false;
        gvEnvironmentalRisk.Columns[10].Visible = false;
        gvEconomicRisk.Columns[9].Visible = false;
        gvevent.Columns[7].Visible = false;
        MenuOperationalHazard.Visible = false;
        btnShowComponents.Visible = false;
        btnShowDocuments.Visible = false;
        btnShowdocumentProcedure.Visible = false;
        lnkComponentAdd.Visible = false;
        lnkFormAdd.Visible = false;
        lnkdocumentProcedureAdd.Visible = false;

        lnkcomment.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=5&SECTIONID=1&RAID=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "'); ");
        lnkaspectcomment.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=5&SECTIONID=2&RAID=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "'); ");
        lnkhealthcomments.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=5&SECTIONID=3&RAID=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "'); ");
        lnkEnvironmental.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=5&SECTIONID=4&RAID=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "'); ");
        lnkEconomic.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=5&SECTIONID=5&RAID=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "'); ");
        lnkUndesireable.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=5&SECTIONID=6&RAID=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "'); ");
        lnkPersonal.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=5&SECTIONID=9&RAID=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "'); ");
        lnkCommissioning.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRAComments.aspx?RATYPEID=5&SECTIONID=10&RAID=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "'); ");
    }

    public void BindMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        ViewState["COPYTYPE"] = "0";
        ViewState["SHOWALL"] = "0";
        if ((Request.QueryString["showall"] != null) && (Request.QueryString["showall"] != ""))
        {
            ViewState["SHOWALL"] = Request.QueryString["showall"].ToString();
        }

        if (Request.QueryString["CopyType"] == "1" || Request.QueryString["CopyType"] == "2")
        {
            toolbar.AddButton("Confirm", "CONFIRM", ToolBarDirection.Right);
            ViewState["COPYTYPE"] = Request.QueryString["CopyType"].ToString();
            //ucTitle.ShowMenu = "false";
        }
        else
        {
            if (Request.QueryString["RevYN"] != "1")
            {
                if (ViewState["status"].ToString() == "1") //Request Approval
                {
                    toolbar.AddButton("Request for Approval", "REQUESTAPPROVAL", ToolBarDirection.Right);
                }

                if (ViewState["status"].ToString() == "1" || ViewState["status"].ToString() == "4" || ViewState["status"].ToString() == "")
                {
                    toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                }

                if ((ViewState["VESSELID"].ToString() != "") && (ViewState["VESSELID"].ToString() != "0") && (ViewState["status"].ToString() == "5"))
                {
                    toolbar.AddLinkButton("javascript:openNewWindow('codehelp1','','Inspection/InspectionRiskAssessmentVerification.aspx?RATEMPLATEID=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "&TYPE=3" + "')", "Verify", "VERIFY", ToolBarDirection.Right);
                }

                if (ViewState["status"].ToString() == "4" && PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0 && !(string.IsNullOrEmpty(Request.QueryString["WORKORDERID"])))
                {
                    toolbar.AddButton("Approve", "APPROVE", ToolBarDirection.Right);
                }
                if ((Request.QueryString["IsPostpone"] != null && Request.QueryString["IsPostpone"].Equals("1"))
                    || (Request.QueryString["IsComponent"] != null && Request.QueryString["IsComponent"].Equals("1"))
                    || (Request.QueryString["RAType"] != null && Request.QueryString["RAType"].Equals("4"))
                    || (Request.QueryString["FromWorkorder"] != null && Request.QueryString["FromWorkorder"].Equals("1"))
                    || (Request.QueryString["PMSApproval"] != null && Request.QueryString["PMSApproval"].Equals("1"))
                    || (Request.QueryString["FromWorkorderGroup"] != null && Request.QueryString["FromWorkorderGroup"].Equals("1"))
                    || (Request.QueryString["DefectId"] != null))
                {
                    toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
                }

                if (ViewState["DashboardYN"].ToString() == "")
                {
                    toolbar.AddButton("List", "LIST", ToolBarDirection.Right);
                }
            }
        }
        MenuMachinery.AccessRights = this.ViewState;
        MenuMachinery.MenuList = toolbar.Show();

        PhoenixToolbar toolbaroperational = new PhoenixToolbar();
        toolbaroperational.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionRAMachineryOperationalHazardPickList.aspx?RAId=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "')", "Map from Aspects Register", "<i class=\"fas fa-tasks\"></i>", "ADD");
        toolbaroperational.AddFontAwesomeButton("javascript:parent.openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionGobalRAOperationalHazard.aspx?RATYPE=5&RAId=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "')", "Add New Aspect", "<i class=\"fa fa-plus-circle\"></i>", "MADD");
        MenuOperationalHazard.AccessRights = this.ViewState;
        MenuOperationalHazard.MenuList = toolbaroperational.Show();

        btnShowComponents.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListGlobalComponent.aspx'); ");
        btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["QUALITYCOMPANYID"].ToString() + "', true); ");
        btnShowdocumentProcedure.Attributes.Add("onclick", "return showPickList('spnPickListdocumentprocedure', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["QUALITYCOMPANYID"].ToString() + "', true); ");

        PhoenixToolbar toolbarMachinerySafetyAfter = new PhoenixToolbar();
        toolbarMachinerySafetyAfter.AddFontAwesomeButton("../Inspection/InspectionRiskAssessmentTask.aspx?RATYPE=5&TaskMode=1&Status=" + ViewState["status"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString() + "&RAId=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString(), "New Task", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MachinerySafetyAfter.AccessRights = this.ViewState;
        MachinerySafetyAfter.MenuList = toolbarMachinerySafetyAfter.Show();

        PhoenixToolbar toolbarMachinerySafetyDuring = new PhoenixToolbar();
        toolbarMachinerySafetyDuring.AddFontAwesomeButton("../Inspection/InspectionRiskAssessmentTask.aspx?RATYPE=5&TaskMode=2&Status=" + ViewState["status"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString() + "&RAId=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString(), "New Task", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MachinerySafetyDuring.AccessRights = this.ViewState;
        MachinerySafetyDuring.MenuList = toolbarMachinerySafetyDuring.Show();

        PhoenixToolbar toolbarMachinerySafety = new PhoenixToolbar();
        toolbarMachinerySafety.AddFontAwesomeButton("../Inspection/InspectionRiskAssessmentTask.aspx?RATYPE=5&TaskMode=0&Status=" + ViewState["status"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString() + "&RAId=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString(), "New Task", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
        MachinerySafety.AccessRights = this.ViewState;
        MachinerySafety.MenuList = toolbarMachinerySafety.Show();
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

    protected void BindGrid()
    {
        gvHealthSafetyRisk.SelectedIndexes.Clear();
        gvHealthSafetyRisk.EditIndexes.Clear();
        gvHealthSafetyRisk.DataSource = null;
        gvHealthSafetyRisk.Rebind();

        gvEnvironmentalRisk.SelectedIndexes.Clear();
        gvEnvironmentalRisk.EditIndexes.Clear();
        gvEnvironmentalRisk.DataSource = null;
        gvEnvironmentalRisk.Rebind();

        gvEconomicRisk.SelectedIndexes.Clear();
        gvEconomicRisk.EditIndexes.Clear();
        gvEconomicRisk.DataSource = null;
        gvEconomicRisk.Rebind();

        gvevent.SelectedIndexes.Clear();
        gvevent.EditIndexes.Clear();
        gvevent.DataSource = null;
        gvevent.Rebind();

        RiskAssessmentMachineryEdit();
    }

    protected void BindTask()
    {
        gvMachinerySafety.SelectedIndexes.Clear();
        gvMachinerySafety.EditIndexes.Clear();
        gvMachinerySafety.DataSource = null;
        gvMachinerySafety.Rebind();

        gvMachinerySafetyAfter.SelectedIndexes.Clear();
        gvMachinerySafetyAfter.EditIndexes.Clear();
        gvMachinerySafetyAfter.DataSource = null;
        gvMachinerySafetyAfter.Rebind();

        gvMachinerySafetyDuring.SelectedIndexes.Clear();
        gvMachinerySafetyDuring.EditIndexes.Clear();
        gvMachinerySafetyDuring.DataSource = null;
        gvMachinerySafetyDuring.Rebind();
    }

    protected void gvRAOperationalHazard_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentMachineryExtn.ListOperationalHazard(General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()));
            gvRAOperationalHazard.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRAOperationalHazard_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                if (e.CommandName == RadGrid.InitInsertCommandName)
                {
                    gvRAOperationalHazard.EditIndexes.Clear();
                }
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    e.Item.OwnerTableView.IsItemInserted = false;
                }

                string OperationalId = ((RadLabel)editableItem.FindControl("lblOperationalId")).Text;
                if (e.CommandName.ToUpper().Equals("ASPECTDELETE"))
                {

                    PhoenixInspectionRiskAssessmentMachineryExtn.DeleteOperationalHazard(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(OperationalId));

                    gvRAOperationalHazard.SelectedIndexes.Clear();
                    gvRAOperationalHazard.EditIndexes.Clear();
                    gvRAOperationalHazard.DataSource = null;
                    gvRAOperationalHazard.Rebind();
                    BindGrid();
                    RiskAssessmentMachineryEdit();
                    ucStatus.Text = "Hazard  Deleted.";
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRAOperationalHazard_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;

                LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                RadLabel lblOperationalHazard = (RadLabel)e.Item.FindControl("lblOperationalHazard");
                RadLabel lblOperationalId = (RadLabel)e.Item.FindControl("lblOperationalId");
                if (cmdEdit != null) cmdEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdEdit.CommandName);
                if (cmdEdit != null)
                {
                    cmdEdit.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionOperationalRiskHazardMapping.aspx?ratypeid=5&Operationalhazardid=" + lblOperationalId.Text + "&RAID=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "'); return true;");
                }

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton cmdAspectEdit = (LinkButton)e.Item.FindControl("cmdAspectEdit");
                if (cmdAspectEdit != null)
                {
                    cmdAspectEdit.Visible = SessionUtil.CanAccess(this.ViewState, cmdAspectEdit.CommandName);
                    cmdAspectEdit.Attributes.Add("onclick", "javascript:parent.openNewWindow('codehelp1','', '" + Session["sitepath"] + "/Inspection/InspectionRAOperationalAspectsEdit.aspx?RATYPE=5&Operationalhazardid=" + lblOperationalId.Text + "');return true;");
                }

                if (e.Item is GridEditableItem)
                {
                    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    }
                }

                DataRowView dv = (DataRowView)e.Item.DataItem;
                RadioButtonList rblOptionEdit = (RadioButtonList)e.Item.FindControl("rblOptionEdit");
                if (rblOptionEdit != null)
                {
                    rblOptionEdit.SelectedValue = dv["FLDOPTIONID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvevent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionOperationalRiskControls.ListRiskAssessmentworstcase(General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()), 5);
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
                if (del != null)
                {
                    del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
                }
                RadLabel lblWorstcaseid = (RadLabel)e.Item.FindControl("lblWorstCaseid");
                HtmlTable tblForms = (HtmlTable)e.Item.FindControl("tblProcedures");
                if (lblWorstcaseid != null)
                {
                    DataSet dss = PhoenixInspectionRiskAssessmentJobHazardExtn.UndesirableeventFormposterList(General.GetNullableGuid(lblWorstcaseid.Text), General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()));
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

                            int type = 0;

                            PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDFORMPOSTERID"].ToString()), ref type);

                            if (type == 2)
                                hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");

                            else if (type == 3)
                                hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");

                            else if (type == 5)
                            {
                                DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , new Guid(dr["FLDFORMPOSTERID"].ToString()));

                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    DataRow drr = ds.Tables[0].Rows[0];
                                        
                                    hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + drr["FLDFORMREVISIONID"].ToString() + "');return false;");
                                }
                            }
                            else if (type == 6)
                            {
                                DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                        , new Guid(dr["FLDFORMPOSTERID"].ToString()));

                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    DataRow drr = ds.Tables[0].Rows[0];
                                    if (drr["FLDFORMREVISIONDTKEY"] != null && General.GetNullableGuid(drr["FLDFORMREVISIONDTKEY"].ToString()) != null)
                                    {
                                        DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(drr["FLDFORMREVISIONDTKEY"].ToString()));
                                        if (dt.Rows.Count > 0)
                                        {
                                            DataRow drRow = dt.Rows[0];
                                            hl.Target = "_blank";
                                            hl.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
                                        }
                                    }
                                }
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

                    BindGrid();
                    ucStatus.Text = "Hazard  Deleted.";
                }

                else if (e.CommandName.ToUpper() == "EMAPPING")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    string id = item.GetDataKeyValue("FLDWORSTCASEID").ToString();
                    ViewState["EVENTID"] = item.GetDataKeyValue("FLDWORSTCASEID").ToString();
                    string script = "function sd(){showDialog('Controls Mapping'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest('" + id + ",EVENTEDIT');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    ViewState["RequestFrom"] = "EVENT";
                }

            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        gvRAOperationalHazard.Rebind();
        BindGrid();
    }
    protected void ClearComponent(object sender, EventArgs e)
    {
        txtComponentCode.Text = "";
        txtComponentName.Text = "";
        txtComponentId.Text = "";
    }
    private void RiskAssessmentMachineryEdit()
    {
        if ((ViewState["RISKASSESSMENTMACHINERYID"] != null) && (ViewState["RISKASSESSMENTMACHINERYID"].ToString() != ""))
        {
            DataTable dt = PhoenixInspectionRiskAssessmentMachineryExtn.EditRiskAssessmentMachinery(
            General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()));

            foreach (DataRow dr in dt.Rows)
            {
                txtRefNo.Text = dr["FLDNUMBER"].ToString();
                txtRevNO.Text = dr["FLDREVISIONNO"].ToString();
                txtpreparedby.Text = dr["FLDPREPAREDBYNAME"].ToString();
                ucCreatedDate.Text = dr["FLDPREPAREDDATE"].ToString();
                txtApprovedby.Text = dr["FLDAPPROVEDBYNAME"].ToString();
                ucApprovedDate.Text = dr["FLDAPPROVEDDATE"].ToString();
                txtIssuedBy.Text = dr["FLDISSUEDBYNAME"].ToString();
                ucIssuedDate.Text = dr["FLDISSUEDDATE"].ToString();
                txtrejectedby.Text = dr["FLDREJECTEDBYNAME"].ToString();
                ucrejecteddate.Text = dr["FLDREJECTEDDATE"].ToString();

                txtDate.Text = dr["FLDDATE"].ToString();
                //ddlActivity.SelectedMiscellaneous = dr["FLDACTIVITYID"].ToString();
                txtActivityCondition.Text = dr["FLDACTIVITYCONDITIONS"].ToString();
                txtIntendedWorkDate.Text = dr["FLDINTENDEDWORKDATE"].ToString();
                if (General.GetNullableInteger(dr["FLDNUMBEROFPEOPLE"].ToString()) != null)
                    rblPeopleInvolved.SelectedValue = dr["FLDNUMBEROFPEOPLE"].ToString();
                BindCheckBoxList(cblReason, dr["FLDREASON"].ToString());
                txtOtherReason.Text = dr["FLDOTHERREASON"].ToString();
                if (General.GetNullableInteger(dr["FLDWORKDURATION"].ToString()) != null)
                    rblWorkDuration.SelectedValue = dr["FLDWORKDURATION"].ToString();
                if (General.GetNullableInteger(dr["FLDWORKFREQUENCY"].ToString()) != null)
                    rblWorkFrequency.SelectedValue = dr["FLDWORKFREQUENCY"].ToString();

                //txtOtherRisk.Text = dr["FLDOTHERRISKPROPOSEDCONTROL"].ToString();
                txtComponentId.Text = dr["FLDEQUIPMENT"].ToString();
                BindCheckBoxList(cblFunctionality, dr["FLDEQUIPMENTFUNCTIONALITY"].ToString());
                txtFunctionality.Text = dr["FLDEQUIPMENTFUNCTIONALITYDETAIL"].ToString();
                //rblStandByEffective.SelectedValue = dr["FLDSTBYFACILITYYN"].ToString();

                BindCheckBoxList(cblCommisionProcedure, dr["FLDCTPROCEDURE"].ToString());
                txtCtDetail.Text = dr["FLDCTDETAIL"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
                txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
                txtjobid.Text = dr["FLDJOBID"].ToString();
                ViewState["FLDJOBID"] = dr["FLDJOBID"].ToString();
                txtJobCode.Text = dr["FLDJOBCODE"].ToString();
                ViewState["status"] = dr["FLDSTATUS"].ToString();

                ViewState["ACTIVITYID"] = dr["FLDACTIVITYID"].ToString();
                txtverifiedby.Text = dr["FLDVERIFIEDBYNAME"].ToString();
                ucverifieddate.Text = dr["FLDVERIFIEDDATE"].ToString();
                txtppe.Text = dr["FLDPPELIST"].ToString();
                txtppe.Text = txtppe.Text.Replace("</br>", Environment.NewLine);


                OtherDetailClick(null, null);

                txtVessel.Text = dr["FLDVESSELNAME"].ToString();
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();

                BindCheckBoxList(chkActivities, dr["FLDACTIVITYLIST"].ToString());
                ViewState["ACTIVITYLIST"] = dr["FLDACTIVITYLIST"].ToString();

                txtMasterRemarks.Text = dr["FLDAPPROVALREMARKSBYVESSEL"].ToString();

                if (!string.IsNullOrEmpty(dr["FLDAPPROVALREMARKSBYVESSEL"].ToString()))
                    chkOverrideByMaster.Checked = true;

                txtAternativeMethod.Text = dr["FLDALTERNATEWORKMETHOD"].ToString();
                if (dr["FLDMACHINERYRAVERIFEDYN"].ToString() == "0" || dr["FLDMACHINERYRAVERIFEDYN"].ToString() == "1")
                {

                    if (dr["FLDMACHINERYRAVERIFEDYN"].ToString() == "0")
                        txtVerificationRemarks.CssClass = "input_mandatory";
                    else
                        txtVerificationRemarks.CssClass = "input";
                }
                txtVerificationRemarks.Text = dr["FLDVERIFICATIONREMARKS"].ToString();

                txtcompletiondate.Text = dr["FLDCOMPLETIONDATE"].ToString();
                BindComponents();
                if (!string.IsNullOrEmpty(dr["FLDCOMPANYID"].ToString()))
                {
                    ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
                    ViewState["QUALITYCOMPANYID"] = dr["FLDCOMPANYID"].ToString();
                    ucCompany.Enabled = false;
                }
                else
                    ucCompany.Enabled = true;

                decimal minscore = 0, maxscore = 0;

                if (!string.IsNullOrEmpty(dr["FLDMINSCORE"].ToString()))
                    minscore = decimal.Parse(dr["FLDMINSCORE"].ToString());

                if (!string.IsNullOrEmpty(dr["FLDMAXSCORE"].ToString()))
                    maxscore = decimal.Parse(dr["FLDMAXSCORE"].ToString());

                lblLevelofRiskHealth.Text = dr["FLDHSLR"].ToString();

                if (!string.IsNullOrEmpty(lblLevelofRiskHealth.Text))
                {
                    if (decimal.Parse(lblLevelofRiskHealth.Text) <= minscore)
                        levelofriskhealth.BgColor = "Lime";
                    else if (decimal.Parse(lblLevelofRiskHealth.Text) > minscore && decimal.Parse(lblLevelofRiskHealth.Text) <= maxscore)
                        levelofriskhealth.BgColor = "Yellow";
                    else if (decimal.Parse(lblLevelofRiskHealth.Text) > maxscore)
                        levelofriskhealth.BgColor = "Red";
                }
                else
                    levelofriskhealth.BgColor = "White";

                lblLevelofRiskEnv.Text = dr["FLDENVLR"].ToString();

                if (!string.IsNullOrEmpty(lblLevelofRiskEnv.Text))
                {
                    if (decimal.Parse(lblLevelofRiskEnv.Text) <= minscore)
                        levelofriskenv.BgColor = "Lime";
                    else if (decimal.Parse(lblLevelofRiskEnv.Text) > minscore && decimal.Parse(lblLevelofRiskEnv.Text) <= maxscore)
                        levelofriskenv.BgColor = "Yellow";
                    else if (decimal.Parse(lblLevelofRiskEnv.Text) > maxscore)
                        levelofriskenv.BgColor = "Red";
                }
                else
                    levelofriskenv.BgColor = "White";

                lblLevelofRiskEconomic.Text = dr["FLDECOLR"].ToString();

                if (!string.IsNullOrEmpty(lblLevelofRiskEconomic.Text))
                {
                    if (decimal.Parse(lblLevelofRiskEconomic.Text) <= minscore)
                        levelofriskeco.BgColor = "Lime";
                    else if (decimal.Parse(lblLevelofRiskEconomic.Text) > minscore && decimal.Parse(lblLevelofRiskEconomic.Text) <= maxscore)
                        levelofriskeco.BgColor = "Yellow";
                    else if (decimal.Parse(lblLevelofRiskEconomic.Text) > maxscore)
                        levelofriskeco.BgColor = "Red";
                }
                else
                    levelofriskeco.BgColor = "White";

                lblLevelofRiskWorst.Text = dr["FLDWCLR"].ToString();

                if (!string.IsNullOrEmpty(lblLevelofRiskWorst.Text))
                {
                    if (decimal.Parse(lblLevelofRiskWorst.Text) <= minscore)
                        levelofriskworst.BgColor = "Lime";
                    else if (decimal.Parse(lblLevelofRiskWorst.Text) > minscore && decimal.Parse(lblLevelofRiskWorst.Text) <= maxscore)
                        levelofriskworst.BgColor = "Yellow";
                    else if (decimal.Parse(lblLevelofRiskWorst.Text) > maxscore)
                        levelofriskworst.BgColor = "Red";
                }
                else
                    levelofriskworst.BgColor = "White";

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
                lblimpacthealth.Text = dr["FLDHSSCORE"].ToString();
                lblimpacteco.Text = dr["FLDECOSCORE"].ToString();
                lblimpactenv.Text = dr["FLDENVSCORE"].ToString();
                lblimpactws.Text = dr["FLDWSSCORE"].ToString();

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

                string reason = General.ReadCheckBoxList(cblReason);

                if (reason.Contains("100"))
                {

                    txtOtherReason.CssClass = "input";
                    txtOtherReason.ReadOnly = false;
                    txtOtherReason.Text = dr["FLDOTHERREASON"].ToString();
                }
                else
                {
                    txtOtherReason.Text = "";
                    txtOtherReason.ReadOnly = true;
                    txtOtherReason.CssClass = "readonlytextbox";
                }

                if (dr["FLDSUPERVISIONID"].ToString() != "")
                {
                    ddlsupervisionlist.SelectedValue = dr["FLDSUPERVISIONID"].ToString();
                }

                General.BindCheckBoxList(Chkpersonsinvolved, dr["FLDPERSONINVOLVEDLIST"].ToString());

                BindMenu();

                if (dr["FLDSTATUS"].ToString() == "5" || dr["FLDSTATUS"].ToString() == "7" || dr["FLDSTATUS"].ToString() == "9")
                {
                    CommentsLink();
                }
                BindProcessRA();
            }
        }
    }
    protected void OtherDetailClick(object sender, EventArgs e)
    {
        string reason = ReadCheckBoxList(cblReason);
        string functionality = ReadCheckBoxList(cblFunctionality);
        string commissioning = ReadCheckBoxList(cblCommisionProcedure);

        string commissioningname = ReadCheckBoxName(cblCommisionProcedure);
        string functionalityname = ReadCheckBoxName(cblFunctionality);

        if (reason.Contains("100"))
        {

            txtOtherReason.CssClass = "input";
            txtOtherReason.ReadOnly = false;
        }
        else
        {
            txtOtherReason.Text = "";
            txtOtherReason.ReadOnly = true;
            txtOtherReason.CssClass = "readonlyRadTextBox";
        }

        if (functionalityname.ToUpper().Contains("OTHER"))
        {

            txtFunctionality.CssClass = "input";
            txtFunctionality.ReadOnly = false;
        }
        else
        {
            txtFunctionality.Text = "";
            txtFunctionality.ReadOnly = true;
            txtFunctionality.CssClass = "readonlyRadTextBox";
        }
        if (commissioningname.ToUpper().Contains("OTHER"))
        {

            txtCtDetail.CssClass = "input";
            txtCtDetail.ReadOnly = false;
        }
        else
        {
            txtCtDetail.Text = "";
            txtCtDetail.ReadOnly = true;
            txtCtDetail.CssClass = "readonlyRadTextBox";
        }
    }
    private string ReadCheckBoxName(CheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Text.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }
    private void BindData()
    {
        Chkpersonsinvolved.DataTextField = "FLDGROUPRANK";
        Chkpersonsinvolved.DataValueField = "FLDGROUPRANKID";
        Chkpersonsinvolved.DataSource = PhoenixRegistersGroupRank.ListJHAGroupRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        Chkpersonsinvolved.DataBind();

        ddlsupervisionlist.DataTextField = "FLDGROUPRANK";
        ddlsupervisionlist.DataValueField = "FLDGROUPRANKID";
        ddlsupervisionlist.DataSource = PhoenixRegistersGroupRank.ListJHAGroupRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlsupervisionlist.Items.Insert(0, new RadComboBoxItem("Not Required", "Dummy"));
        ddlsupervisionlist.DataBind();

        rblPeopleInvolved.DataTextField = "FLDNAME";
        rblPeopleInvolved.DataValueField = "FLDFREQUENCYID";
        rblPeopleInvolved.DataSource = PhoenixInspectionRiskAssessmentFrequencyExtn.ListRiskAssessmentFrequency(3);
        rblPeopleInvolved.DataBind();

        cblReason.DataTextField = "FLDNAME";
        cblReason.DataValueField = "FLDMISCELLANEOUSID";
        cblReason.DataSource = PhoenixInspectionRiskAssessmentMiscellaneousExtn.ListRiskAssessmentMiscellaneous(1, null);
        cblReason.DataBind();

        rblWorkDuration.DataTextField = "FLDNAME";
        rblWorkDuration.DataValueField = "FLDFREQUENCYID";
        rblWorkDuration.DataSource = PhoenixInspectionRiskAssessmentFrequencyExtn.ListRiskAssessmentFrequency(1);
        rblWorkDuration.DataBind();

        rblWorkFrequency.DataTextField = "FLDNAME";
        rblWorkFrequency.DataValueField = "FLDFREQUENCYID";
        rblWorkFrequency.DataSource = PhoenixInspectionRiskAssessmentFrequencyExtn.ListRiskAssessmentFrequency(2, "OFP");
        rblWorkFrequency.DataBind();
        rblWorkFrequency.SelectedIndex = 0;

        cblFunctionality.DataTextField = "FLDITEM";
        cblFunctionality.DataValueField = "FLDID";
        cblFunctionality.DataSource = PhoenixInspectionRiskAssessmentTypeExtn.ListRiskAssessmentType(
                 Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 201, "REF")), 3);
        cblFunctionality.DataBind();

        cblCommisionProcedure.DataTextField = "FLDITEM";
        cblCommisionProcedure.DataValueField = "FLDID";
        cblCommisionProcedure.DataSource = PhoenixInspectionRiskAssessmentTypeExtn.ListRiskAssessmentType(
                 Convert.ToInt32(PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 201, "RPC")), 3);
        cblCommisionProcedure.DataBind();

        chkActivities.DataTextField = "FLDNAME";
        chkActivities.DataValueField = "FLDACTIVITYID";
        chkActivities.DataSource = PhoenixInspectionRiskAssessmentActivityExtn.ListNonRoutineRiskAssessmentActivity(3, null);
        chkActivities.DataBind();


    }

    protected void MenuMachinery_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                SaveRiskAssessmentMachinery();
                MapRADefect();
            }

            if (CommandName.ToUpper().Equals("RESAVE"))
            {
                SaveRiskAssessmentMachinery();
            }

            if (CommandName.ToUpper().Equals("REQUESTAPPROVAL"))
            {
                if (ViewState["RISKASSESSMENTMACHINERYID"] != null && ViewState["RISKASSESSMENTMACHINERYID"].ToString() != "")
                {
                    PhoenixInspectionRiskAssessmentMachineryExtn.RequestApprovalMachinery(new Guid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()));
                    ucStatus.Text = "Requested Successfully";
                    RiskAssessmentMachineryEdit();
                }
            }

            if (CommandName.ToUpper().Equals("LIST"))
            {
                if (ViewState["StandardRA"].ToString() == "1")
                {
                    if (ViewState["HSEQADashboardYN"].ToString().Equals("1"))
                    {
                        Response.Redirect("../Inspection/InspectionStandardTemplateNonRoutineRA.aspx");
                    }
                    else
                    {
                        Response.Redirect("../Inspection/InspectionStandardNonRoutineRA.aspx", false);
                    }
                }
                else
                {
                    if (ViewState["HSEQADashboardYN"].ToString().Equals("1"))
                    {
                        Response.Redirect("../Inspection/InspectionDashboardNonRoutineRiskAssessmentList.aspx");
                    }
                    else
                    {
                        Response.Redirect("../Inspection/InspectionNonRoutineRiskAssessmentListExtn.aspx", false);
                    }                    
                }
            }
            //if (CommandName.ToUpper().Equals("VERIFY"))
            //{
            //    VerifyRiskAssessmentMachinery();
            //}
            if (CommandName.ToUpper().Equals("BACK"))
            {
                if (!string.IsNullOrEmpty(ViewState["DefectId"].ToString()))
                {
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWODefectJobDetails.aspx?DefectId=" + ViewState["DefectId"].ToString(), false);
                }
                else if (ViewState["ISPOSTPONE"].ToString() == "1")
                {
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrder.aspx?FromRA=1&WORKORDERID="
                        + ViewState["WORKORDERID"].ToString() + "&PAGENUMBER=" + ViewState["PAGENUMBER"].ToString(), false);
                }

                else if (ViewState["PMSRAAPPROVAL"].ToString() == "1")
                {
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceRiskAssessmentPendingApproval.aspx", false);
                }

                else if (Request.QueryString["tv"] != null && Request.QueryString["tv"] != "")
                {
                    Response.Redirect("../Inventory/InventoryComponent.aspx?FromRA=1&WORKORDERID=" + ViewState["WORKORDERID"].ToString()
                        + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&PAGENUMBER=" + ViewState["PAGENUMBER"].ToString() + "&tv=" + Request.QueryString["tv"], false);
                }
                else if (ViewState["COMPONENTJOBID"].ToString() != "")
                {
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceComponentJob.aspx?FromRA=1&WORKORDERID=" + ViewState["WORKORDERID"].ToString()
                                           + "&COMPONENTJOBID=" + ViewState["COMPONENTJOBID"].ToString()
                                           + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&p=" + ViewState["PAGENUMBER"].ToString(), false);
                }
                else if ((ViewState["RATYPE"].ToString() == "4") && (ViewState["mocextention"].ToString() == ""))
                {
                    Response.Redirect("../Inspection/InspectionMOCRequestAdd.aspx?MOCRequestid=" + ViewState["MOCRequestid"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString() + "&RiskAssessmentid=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString(), false);
                }
                else if ((ViewState["RATYPE"].ToString() == "4") && ViewState["mocextention"].ToString() == "yes")
                {
                    Response.Redirect("../Inspection/InspectionMOCIntermediateVerificationAdd.aspx?MOCRequestid=" + ViewState["MOCRequestid"].ToString() + "&MOCID=" + ViewState["MOCID"].ToString() + "&RiskAssessmentid=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString(), false);
                }
                else if (ViewState["FromWorkorder"].ToString() == "1")
                {
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceWorkOrder.aspx?FromRATemplate=1&WORKORDERID="
                        + ViewState["WORKORDERID"].ToString() + "&PAGENUMBER=" + ViewState["PAGENUMBER"].ToString(), false);
                }
                else if (ViewState["FromWorkorderGroup"].ToString() == "1")
                {
                    Response.Redirect("../PlannedMaintenance/PlannedMaintenanceSubWorkOrderList.aspx?groupId=" + ViewState["WORKORDERGROUPID"].ToString() + "&WORKORDERID="
                    + ViewState["WORKORDERID"].ToString() + "&PAGENUMBER=" + ViewState["PAGENUMBER"].ToString(), false);
                }
                else
                    Response.Redirect("../Inventory/InventoryComponent.aspx?FromRA=1&WORKORDERID=" + ViewState["WORKORDERID"].ToString()
                        + "&COMPONENTID=" + ViewState["COMPONENTID"].ToString() + "&PAGENUMBER=" + ViewState["PAGENUMBER"].ToString(), false);
            }

            if (CommandName.ToUpper().Equals("APPROVE"))
            {

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0) //  vessel created RA approved in office.
                {
                    if (ViewState["RaCreatedIn"].ToString() != "" && int.Parse(ViewState["RaCreatedIn"].ToString()) > 0) // office or vessel
                    {
                        string scriptpopup = string.Format("javascript:Openpopup('codehelp1','','../Inspection/InspectionMainFleetRAApprovalExtn.aspx?RATEMPLATEID=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "&TYPE=3','large');");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptpopup, true);
                    }
                }

            }
            if (CommandName.ToUpper().Equals("CONFIRM"))
            {
                if (ViewState["COPYTYPE"].ToString() == "1")
                {
                    if (ViewState["RISKASSESSMENTMACHINERYID"] != null && ViewState["RISKASSESSMENTMACHINERYID"].ToString() != "")
                    {
                        PhoenixInspectionRiskAssessmentMachineryExtn.CopyMachinery(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                        ucStatus.Text = "Copied Successfully";

                        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                    }
                }
                if (ViewState["COPYTYPE"].ToString() == "2")
                {
                    if (ViewState["RISKASSESSMENTMACHINERYID"] != null && ViewState["RISKASSESSMENTMACHINERYID"].ToString() != "")
                    {
                        PhoenixInspectionRiskAssessmentMachineryExtn.MachineryProposeTemplate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            new Guid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()), PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                        ucStatus.Text = "Proposed Successfully";

                        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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

    private void VerifyRiskAssessmentMachinery()
    {
        try
        {
            PhoenixInspectionRiskAssessmentMachineryExtn.RAMachineryVerify(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"] != null ? ViewState["RISKASSESSMENTMACHINERYID"].ToString() : "")
                , General.GetNullableInteger("1")
                , General.GetNullableString(txtVerificationRemarks.Text));

            ucStatus.Text = "RA Completed";
            ucStatus.Visible = true;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MapRADefect()
    {
        if (General.GetNullableGuid(ViewState["DefectId"].ToString()) != null && General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()) != null)
        {
            PhoenixInspectionRiskAssessmentMachineryExtn.RADefectMapping(General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString())
                , General.GetNullableGuid(ViewState["DefectId"].ToString())
                , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
        }

    }
    private void SaveRiskAssessmentMachinery()
    {
        try
        {
            string recorddate = txtDate.Text;
            string assessmentdate = txtIntendedWorkDate.Text;
            string peopleinvolved = rblPeopleInvolved.SelectedValue;
            string reasonforassessment = ReadCheckBoxList(cblReason);
            string otherreason = txtOtherReason.Text;
            string aternativemethod = txtAternativeMethod.Text;
            string durationofworkactivity = rblWorkDuration.SelectedValue;
            string frequencyofworkactivity = rblWorkFrequency.SelectedValue;
            //string otherriskproposed = txtOtherRisk.Text;

            if (!IsValidMachineryTemplate())
            {
                ucError.Visible = true;
                return;
            }
            Guid? riskassessmentmachineryidout = Guid.NewGuid();
            PhoenixInspectionRiskAssessmentMachineryExtn.InsertRiskAssessmentMachinery(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()),
                General.GetNullableDateTime(recorddate),
                General.GetNullableDateTime(assessmentdate),
                PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                null, durationofworkactivity, frequencyofworkactivity, General.GetNullableInteger(rblPeopleInvolved.SelectedValue),
                null, null, reasonforassessment, otherreason,
                 null, null, null, null, null, null,
                null, null, null, null,
                General.GetNullableGuid(txtComponentId.Text),
                null,
                ReadCheckBoxList(cblFunctionality),
                txtFunctionality.Text,
                General.GetNullableInteger(null),
                null,
                null,
                General.GetNullableInteger(null),
                null,
                null,
                ReadCheckBoxList(cblCommisionProcedure),
                txtCtDetail.Text,
                txtRemarks.Text,
                null,
                //General.GetNullableInteger(ddlActivity.SelectedMiscellaneous),
                General.GetNullableString(txtActivityCondition.Text),
                ref riskassessmentmachineryidout,
                General.GetNullableInteger(null),
                General.GetNullableInteger("-1"),
                General.GetNullableGuid(null),
                General.GetNullableString(null),
                General.GetNullableInteger(ucCompany.SelectedCompany),
                General.GetNullableString(aternativemethod),
                General.GetNullableGuid(ViewState["WORKORDERID"].ToString()),
                General.GetNullableDateTime(txtcompletiondate.Text),
                General.GetNullableGuid(null),
                General.GetNullableGuid(null),
                General.GetNullableString(General.ReadCheckBoxList(chkActivities)),
                General.GetNullableString(General.ReadCheckBoxList(Chkpersonsinvolved)),
                General.GetNullableInteger(ddlsupervisionlist.SelectedValue)
                );

            ucStatus.Text = "Machinery Template updated.";

            ViewState["RISKASSESSMENTMACHINERYID"] = riskassessmentmachineryidout.ToString();

            Filter.CurrentSelectedMachineryRA = riskassessmentmachineryidout.ToString();
            RiskAssessmentMachineryEdit();
            BindTask();
            //BindGridMachinerySafety();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private bool IsValidMachineryTemplate()
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucCompany.SelectedCompany) == null)
            ucError.ErrorMessage = "Company is required.";

        if (General.GetNullableDateTime(txtIntendedWorkDate.Text) == null)
            ucError.ErrorMessage = "Date of intended work is required.";

        if (General.GetNullableDateTime(txtcompletiondate.Text) == null)
            ucError.ErrorMessage = "Target Date for completion of the Activity is required.";

        if (General.GetNullableString(txtActivityCondition.Text) == null)
            ucError.ErrorMessage = "Activity / Conditions is required.";

        if (General.GetNullableInteger(rblPeopleInvolved.SelectedValue) == null)
            ucError.ErrorMessage = "No of people affected is required.";

        if (General.GetNullableString(General.ReadCheckBoxList(chkActivities)) == null)
            ucError.ErrorMessage = "Activities Affected is required.";

        if (General.GetNullableString(General.ReadCheckBoxList(Chkpersonsinvolved)) == null)
            ucError.ErrorMessage = "Person's carrying out job is required.";

        //if (General.GetNullableInteger(rblWorkDuration.SelectedValue) == null)
        //    ucError.ErrorMessage = "Duration is required.";

        if (General.GetNullableInteger(rblWorkFrequency.SelectedValue) == null)
            ucError.ErrorMessage = "Frequency is required.";

        return (!ucError.IsError);
    }
    private void BindCheckBoxList(CheckBoxList cbl, string list)
    {
        foreach (string item in list.Split(','))
        {
            if (item.Trim() != "")
            {
                if (cbl.Items.FindByValue(item) != null)
                    cbl.Items.FindByValue(item).Selected = true;
            }
        }
    }

    private string ReadCheckBoxList(CheckBoxList cbl)
    {
        string list = string.Empty;

        foreach (ListItem item in cbl.Items)
        {
            if (item.Selected == true)
            {
                list = list + item.Value.ToString() + ",";
            }
        }
        list = list.TrimEnd(',');
        return list;
    }

    protected void gvHealthSafetyRisk_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentMachineryExtn.ListMachineryCategory(1, General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()));
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
            ds = PhoenixInspectionRiskAssessmentMachineryExtn.ListMachineryCategory(2, General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()));
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
            ds = PhoenixInspectionRiskAssessmentMachineryExtn.ListMachineryCategory(4, General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()),
            General.GetNullableInteger(ViewState["VESSELID"].ToString()));
            gvEconomicRisk.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMachinerySafety_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentMachineryExtn.ListMachinerySafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString())
                , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                , null
                , 0);

            gvMachinerySafety.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvEconomicRisk_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                if (e.Item is GridEditableItem)
                {
                    LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null)
                    {
                        db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                    }
                }
            }
            ECOGridDecorator.MergeRows(gvEconomicRisk, e);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEconomicRisk_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                if (e.CommandName.ToUpper().Equals("CDELETE"))
                {

                    string Healthid = ((RadLabel)editableItem.FindControl("lblHealthid")).Text;
                    PhoenixInspectionRiskAssessmentMachineryExtn.DeleteMachineryCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(Healthid));

                    BindGrid();
                    ucStatus.Text = "Hazard  Deleted.";
                }

                else if (e.CommandName.ToUpper() == "ECOMAPPING")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    string id = item.GetDataKeyValue("FLDCATEGORYID").ToString();
                    ViewState["IMPACTID"] = item.GetDataKeyValue("FLDCATEGORYID").ToString();
                    string script = "function sd(){showDialog('Controls Mapping'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest('" + id + ",EDIT');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    ViewState["RequestFrom"] = "Category";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEnvironmentalRisk_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                GridEditableItem eeditedItem = e.Item as GridEditableItem;

                if (e.CommandName.ToUpper().Equals("EDELETE"))
                {
                    string categoryid = ((RadLabel)editableItem.FindControl("lblHealthid")).Text;
                    PhoenixInspectionRiskAssessmentMachineryExtn.DeleteMachineryCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(categoryid));
                    BindGrid();

                }
                else if (e.CommandName.ToUpper() == "ENVMAPPING")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    string id = item.GetDataKeyValue("FLDCATEGORYID").ToString();
                    ViewState["IMPACTID"] = item.GetDataKeyValue("FLDCATEGORYID").ToString();
                    string script = "function sd(){showDialog('Controls Mapping'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest('" + id + ",EDIT');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    ViewState["RequestFrom"] = "Category";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvEnvironmentalRisk_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                GridEditableItem item = e.Item as GridEditableItem;

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
            }
            ENVGridDecorator.MergeRows(gvEnvironmentalRisk, e);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvHealthSafetyRisk_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem item = e.Item as GridDataItem;

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null)
                {
                    db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                }
            }
            HSGridDecorator.MergeRows(gvHealthSafetyRisk, e);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvHealthSafetyRisk_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                if (e.CommandName.ToUpper().Equals("HDELETE"))
                {
                    string categoryid = ((RadLabel)editableItem.FindControl("lblHealthid")).Text;
                    PhoenixInspectionRiskAssessmentMachineryExtn.DeleteMachineryCategory(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(categoryid));

                    ucStatus.Text = "Hazard  Deleted.";
                    BindGrid();
                }

                else if (e.CommandName.ToUpper() == "HMAPPING")
                {
                    GridDataItem item = (GridDataItem)e.Item;
                    string id = item.GetDataKeyValue("FLDCATEGORYID").ToString();
                    ViewState["IMPACTID"] = item.GetDataKeyValue("FLDCATEGORYID").ToString();
                    string script = "function sd(){showDialog('Controls Mapping'); $find('" + RadAjaxPanel2.ClientID + "').ajaxRequest('" + id + ",EDIT');   Sys.Application.remove_load(sd);}Sys.Application.add_load(sd);";
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
                    ViewState["RequestFrom"] = "Category";
                }
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void RadAjaxPanel2_AjaxRequest(object sender, Telerik.Web.UI.AjaxRequestEventArgs e)
    {
        var args = e.Argument;
        var array = args.Split(',');
        var id = array[0];
        var cmd = array[1];

        if (cmd.ToUpper() == "EDIT")
        {
            tbl2.Visible = false;
            tbl1.Visible = true;
            BindImpactComponents();
        }
        if (cmd.ToUpper() == "EVENTEDIT")
        {
            tbl2.Visible = true;
            tbl1.Visible = false;

            txtProcedure.Text = "";
            txtProcedureid.Text = "";
            txtEquipment.Text = "";
            txtEquipmentCode.Text = "";
            txtEquipmentid.Text = "";
            lnkEquipmentList.Attributes.Add("onclick", "return showPickList('spnPickListEquipment', 'codehelp1', '', '../Common/CommonPickListGlobalComponent.aspx'); ");
            lnkProcedureList.Attributes.Add("onclick", "return showPickList('spnPickListProcedure', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["QUALITYCOMPANYID"].ToString() + "', true); ");
            BindEquipment();
            BindProcedure();
            BindWorstCase();
            BindWorstcaseEvents();
        }
    }

    protected void gvPersonal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentMachineryExtn.ListMachineryControl(General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()),
                General.GetNullableInteger(ViewState["VESSELID"].ToString()));
            gvPersonal.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPersonal_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var editableItem = ((GridEditableItem)e.Item);

                if (e.CommandName.ToUpper().Equals("RDELETE"))
                {
                    string controlid = ((RadLabel)editableItem.FindControl("lblId")).Text;
                    if (General.GetNullableGuid(controlid) != null)
                    {
                        PhoenixInspectionRiskAssessmentMachineryExtn.DeleteMachineryControl(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                       new Guid(controlid));
                        gvPersonal.Rebind();
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

    protected void gvPersonal_ItemDataBound(object sender, GridItemEventArgs e)
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

            if (del != null)
            {
                del.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }

            DataRowView dv = (DataRowView)e.Item.DataItem;
            RadioButtonList rblOptionEdit = (RadioButtonList)e.Item.FindControl("rblOptionEdit");
            if (rblOptionEdit != null)
            {
                rblOptionEdit.SelectedValue = dv["FLDOPTIONID"].ToString();
            }
        }
    }

    protected void gvMachinerySafety_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
        if (db != null)
        {
            db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
        }
    }
    protected void gvMachinerySafety_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var Item = ((GridEditableItem)e.Item);

                string machinerysafteyid = ((RadLabel)Item.FindControl("lblMachinerySafetyId")).Text;
                if (e.CommandName.ToUpper().Equals("SDELETE"))
                {

                    PhoenixInspectionRiskAssessmentMachineryExtn.DeleteMachinerySafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString())
                        , General.GetNullableGuid(machinerysafteyid)
                        );
                    BindTask();
                }
                if (e.CommandName.ToUpper().Equals("SEDIT"))
                {
                    Response.Redirect("../Inspection/InspectionRiskAssessmentTask.aspx?RATYPE=5&TaskMode=0&Status=" + ViewState["status"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString() + "&RAId=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "&Taskid=" + machinerysafteyid, false);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidHazard(string aspectid, string hazardtypeid, string subhazardid, int? type, string impacttype)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Template first and then try adding the Hazard.";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()) != null)
        {
            if (General.GetNullableGuid(aspectid) == null)
                ucError.ErrorMessage = "Aspect is Required.";

            if (General.GetNullableInteger(hazardtypeid) == null)
                ucError.ErrorMessage = "Hazard Type is required.";

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

    private bool IsValidSafety(string task, string estimatedfinishdate)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Template first and then try adding the Hazard.";

        if (General.GetNullableString(task) == null)
        {
            ucError.ErrorMessage = "Task is required";
        }
        if (General.GetNullableDateTime(estimatedfinishdate) == null)
        {
            ucError.ErrorMessage = "Target date is required";
        }
        return (!ucError.IsError);
    }

    private bool IsValidSpareTools(string option)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()) == null)
            ucError.ErrorMessage = "Please save the Template first and then try answering.";

        return (!ucError.IsError);
    }

    public void rblOptions_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            RadioButtonList rblOptions = (RadioButtonList)sender;

            GridDataItem gvrow = (GridDataItem)rblOptions.Parent.Parent;

            string lblId = ((RadLabel)gvrow.FindControl("lblId")).Text;

            RadLabel lblItemid = (RadLabel)gvrow.FindControl("lblItemid");

            if (lblId != null && lblId != "")
            {
                if (!IsValidSpareTools(lblItemid.Text))
                {
                    ucError.Visible = true;
                    gvPersonal.Rebind();
                    return;
                }
                PhoenixInspectionRiskAssessmentMachineryExtn.UpdateMachineryControl(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            new Guid(lblId), new Guid(lblItemid.Text),
                                                            int.Parse(((RadioButtonList)gvrow.FindControl("rblOptionEdit")).SelectedValue));
            }
            else
            {
                if (!IsValidSpareTools(lblItemid.Text))
                {
                    ucError.Visible = true;
                    gvPersonal.Rebind();
                    return;
                }
                PhoenixInspectionRiskAssessmentMachineryExtn.InsertMachineryControl(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                            new Guid(lblItemid.Text), int.Parse(((RadioButtonList)gvrow.FindControl("rblOptionEdit")).SelectedValue),
                                                            new Guid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()),
                                                            int.Parse(ViewState["VESSELID"].ToString()));
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvMachinerySafetyAfter_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
        if (db != null)
        {
            db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
        }
    }
    protected void gvMachinerySafetyAfter_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var Item = ((GridEditableItem)e.Item);
                string machinerysafteyid = ((RadLabel)Item.FindControl("lblMachinerySafetyId")).Text;
                if (e.CommandName.ToUpper().Equals("SADELETE"))
                {
                    PhoenixInspectionRiskAssessmentMachineryExtn.DeleteMachinerySafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString())
                        , General.GetNullableGuid(machinerysafteyid)
                        );
                    BindTask();
                }
                if (e.CommandName.ToUpper().Equals("SAEDIT"))
                {
                    Response.Redirect("../Inspection/InspectionRiskAssessmentTask.aspx?RATYPE=5&TaskMode=1&Status=" + ViewState["status"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString() + "&RAId=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "&Taskid=" + machinerysafteyid, false);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMachinerySafetyAfter_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentMachineryExtn.ListMachinerySafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString())
                , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                , null
                , 1);

            gvMachinerySafetyAfter.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvMachinerySafetyDuring_ItemDataBound(object sender, GridItemEventArgs e)
    {
        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

        LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
        if (db != null)
        {
            db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
        }
    }
    protected void gvMachinerySafetyDuring_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                var Item = ((GridEditableItem)e.Item);
                string machinerysafteyid = ((RadLabel)Item.FindControl("lblMachinerySafetyId")).Text;
                if (e.CommandName.ToUpper().Equals("SDDELETE"))
                {

                    PhoenixInspectionRiskAssessmentMachineryExtn.DeleteMachinerySafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString())
                        , General.GetNullableGuid(machinerysafteyid)
                        );
                    BindTask();
                }
                if (e.CommandName.ToUpper().Equals("SDEDIT"))
                {
                    Response.Redirect("../Inspection/InspectionRiskAssessmentTask.aspx?RATYPE=5&TaskMode=2&Status=" + ViewState["status"].ToString() + "&Vesselid=" + ViewState["VESSELID"].ToString() + "&RAId=" + ViewState["RISKASSESSMENTMACHINERYID"].ToString() + "&Taskid=" + machinerysafteyid, false);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvMachinerySafetyDuring_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            DataSet ds = new DataSet();
            ds = PhoenixInspectionRiskAssessmentMachineryExtn.ListMachinerySafety(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString())
                , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                , null
                , 2);

            gvMachinerySafetyDuring.DataSource = ds;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void lnkComponentAdd_Click(object sender, EventArgs e)
    {
        if (General.GetNullableGuid(txtComponentId.Text) != null)
        {
            if (ViewState["RISKASSESSMENTMACHINERYID"].Equals(""))
            {
                ucError.ErrorMessage = "Add the Template first and then add Hazard.";
                return;
            }
            PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateRAComponents(new Guid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()), new Guid(txtComponentId.Text), 5);
            ucStatus.Text = "Equipment added.";
            txtComponentId.Text = "";
            txtComponentCode.Text = "";
            txtComponentName.Text = "";
            BindComponents();
        }
    }

    protected void BindImpactComponents()
    {
        DataSet ds = PhoenixInspectionRiskAssessmentJobHazardExtn.JHAImpactEquipmentList(General.GetNullableGuid(ViewState["IMPACTID"].ToString()), 5);
        chkEquipment.DataSource = ds.Tables[0];
        chkEquipment.DataBind();

        chkEvent.DataSource = ds.Tables[2];
        chkEvent.DataBind();

        if (ds.Tables[0].Rows.Count > 0)
        {
            General.RadBindCheckBoxList(chkEquipment, ds.Tables[0].Rows[0]["FLDMAPPEDCOMPONENTID"].ToString());
        }

        if (ds.Tables[1].Rows.Count > 0)
        {
            if (ds.Tables[1].Rows[0]["FLDHAZARDTYPE"].ToString().Equals("1"))
                trPPE.Visible = true;
            else
                trPPE.Visible = false;

            cblRecomendedPPE.ClearSelection();
            General.BindCheckBoxList(cblRecomendedPPE, ds.Tables[1].Rows[0]["FLDPPELIST"].ToString());
        }

        if (ds.Tables[2].Rows.Count > 0)
        {

            General.RadBindCheckBoxList(chkEvent, ds.Tables[2].Rows[0]["FLDUNDESIRABLEEVENT"].ToString().ToLower());

        }

    }
    protected void BindEventComponents()
    {
        DataSet ds = PhoenixInspectionRiskAssessmentJobHazardExtn.JHAEventEquipmentList(General.GetNullableGuid(ViewState["EVENTID"].ToString()), 5);
        chkEquipment.DataSource = ds.Tables[0];
        chkEquipment.DataBind();

        if (ds.Tables[0].Rows.Count > 0)
        {
            General.RadBindCheckBoxList(chkEquipment, ds.Tables[0].Rows[0]["FLDMAPPEDCOMPONENTID"].ToString());
        }

        trPPE.Visible = true;
        if (ds.Tables[1].Rows.Count > 0)
        {
            cblRecomendedPPE.ClearSelection();
            General.BindCheckBoxList(cblRecomendedPPE, ds.Tables[1].Rows[0]["FLDPPELIST"].ToString());
        }

    }
    protected void BindComponents()
    {
        DataSet dss = PhoenixInspectionRiskAssessmentJobHazardExtn.RAComponentList(ViewState["RISKASSESSMENTMACHINERYID"].ToString() == "" ? null : General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()), 5);
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tblcomponents.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                CheckBox cb = new CheckBox();
                cb.ID = dr["FLDCOMPONENTID"].ToString();
                cb.Text = "";
                cb.Checked = true;
                if (ViewState["status"] != null && (ViewState["status"].ToString().Equals("5") || ViewState["status"].ToString().Equals("7") || ViewState["status"].ToString().Equals("9")))
                    cb.Enabled = false;

                cb.AutoPostBack = true;
                cb.CheckedChanged += new EventHandler(com_CheckedChanged);
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDCOMPONENTNAME"].ToString();
                hl.ID = "hlink2" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tc.Controls.Add(cb);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hl);
                tr.Cells.Add(tc);
                tblcomponents.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblcomponents.Rows.Add(tr);
                number = number + 1;
            }
            divComponents.Visible = true;
        }
        else
            divComponents.Visible = false;
    }

    void com_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox c = (CheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixInspectionRiskAssessmentJobHazardExtn.RAComponentDelete(new Guid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()), new Guid(c.ID), 5);

            string txt = "";

            ucStatus.Text = txt + "deleted.";
            BindComponents();
        }
    }

    protected void BindFormPosters()
    {
        DataSet dss = PhoenixInspectionRiskAssessmentJobHazardExtn.RAFormPosterCheckList(ViewState["RISKASSESSMENTMACHINERYID"] == null ? null : General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()), General.GetNullableInteger("0"), 5);
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tblForms.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                CheckBox cb = new CheckBox();
                cb.ID = dr["FLDFORMPOSTERID"].ToString();
                cb.Text = "";
                cb.Checked = true;
                cb.AutoPostBack = true;
                if (ViewState["status"] != null && (ViewState["status"].ToString().Equals("5") || ViewState["status"].ToString().Equals("7") || ViewState["status"].ToString().Equals("9")))
                    cb.Enabled = false;
                cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
                //cb.Attributes.Add("onclick","");
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDNAME"].ToString();
                hl.ID = "hlink" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                int type = 0;

                PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDFORMPOSTERID"].ToString()), ref type);
                if (type == 5)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(dr["FLDFORMPOSTERID"].ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = ds.Tables[0].Rows[0];

                        hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + drr["FLDFORMREVISIONID"].ToString() + "');return false;");
                    }
                }
                else if (type == 6)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(dr["FLDFORMPOSTERID"].ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = ds.Tables[0].Rows[0];
                        if (drr["FLDFORMREVISIONDTKEY"] != null && General.GetNullableGuid(drr["FLDFORMREVISIONDTKEY"].ToString()) != null)
                        {
                            DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(drr["FLDFORMREVISIONDTKEY"].ToString()));
                            if (dt.Rows.Count > 0)
                            {
                                DataRow drRow = dt.Rows[0];
                                hl.Target = "_blank";
                                hl.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
                            }
                        }
                    }
                }

                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tc.Controls.Add(cb);
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
            divForms.Visible = true;
        }
        else
            divForms.Visible = false;
    }

    protected void BindDocumentProcedures()
    {
        DataSet dss = PhoenixInspectionRiskAssessmentJobHazardExtn.RAFormPosterCheckList(ViewState["RISKASSESSMENTMACHINERYID"] == null ? null : General.GetNullableGuid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()), General.GetNullableInteger("1"),5);
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tbldocumentprocedure.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                CheckBox doccb = new CheckBox();
                doccb.ID = dr["FLDFORMPOSTERID"].ToString();
                doccb.Text = "";
                doccb.Checked = true;
                if (ViewState["status"] != null && (ViewState["status"].ToString().Equals("5") || ViewState["status"].ToString().Equals("7") || ViewState["status"].ToString().Equals("9")))
                    doccb.Enabled = false;

                doccb.AutoPostBack = true;
                if (ViewState["status"] != null && ViewState["status"].ToString().Equals("3"))
                    doccb.Enabled = false;
                doccb.CheckedChanged += new EventHandler(doccb_CheckedChanged);
                //cb.Attributes.Add("onclick","");
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDNAME"].ToString();
                hl.ID = "hlink1" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                int type = 0;

                PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDFORMPOSTERID"].ToString()), ref type);
                if (type == 2)
                    hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                else if (type == 3)
                    hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");


                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tc.Controls.Add(doccb);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hl);
                tr.Cells.Add(tc);
                tbldocumentprocedure.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tbldocumentprocedure.Rows.Add(tr);
                number = number + 1;
            }
            divdocumentProcedure.Visible = true;
        }
        else
            divdocumentProcedure.Visible = false;
    }
    protected void BindProcessRA()
    {
        DataSet dss = PhoenixInspectionRiskAssessmentActivityExtn.ListActivityProcessRA(General.GetNullableInteger(ViewState["QUALITYCOMPANYID"].ToString())
                                                                                        , General.GetNullableString(ViewState["ACTIVITYLIST"].ToString()));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tbldivprocessra.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDPROCESSNAME"].ToString();
                hl.ID = "hlink3" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAPROCESSNEW&processid=" + dr["FLDRISKASSESSMENTPROCESSID"].ToString() + "&showmenu=0&showexcel=NO');return true;");
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hl);
                tr.Cells.Add(tc);
                tbldivprocessra.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tbldivprocessra.Rows.Add(tr);
                number = number + 1;
            }
        }
    }
    void cb_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox c = (CheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixInspectionRiskAssessmentJobHazardExtn.RAFormPosterChecklistDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()), new Guid(c.ID), int.Parse("0"), 5);

            string txt = "Forms & Checklists";

            ucStatus.Text = txt + " deleted.";
            BindFormPosters();
        }
    }

    void doccb_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox c = (CheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixInspectionRiskAssessmentJobHazardExtn.RAFormPosterChecklistDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                new Guid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()), new Guid(c.ID), int.Parse("1"), 5);

            string txt = "Procedure";

            ucStatus.Text = txt + " deleted.";
            BindDocumentProcedures();
        }
    }

    protected void lnkFormAdd_Click(object sender, EventArgs e)
    {
        try
        {

            if (ViewState["RISKASSESSMENTMACHINERYID"] == null || ViewState["RISKASSESSMENTMACHINERYID"].ToString() == string.Empty)
            {
                ucError.ErrorMessage = "Please save the Template first and then try adding the Forms and Checklists.";
                ucError.Visible = true;
                return;
            }
            if (General.GetNullableGuid(txtDocumentId.Text) != null)
            {
                PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateRAFormsandchecklist(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        new Guid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()), new Guid(txtDocumentId.Text), 5);
                ucStatus.Text = "Forms & Checklists added.";
                txtDocumentId.Text = "";
                txtDocumentName.Text = "";
                BindFormPosters();
            }
            else
            {
                ucError.ErrorMessage = "Please select the Forms & Checklists.";
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

    protected void lnkdocumentProcedureAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["RISKASSESSMENTMACHINERYID"] == null || ViewState["RISKASSESSMENTMACHINERYID"].ToString() == string.Empty)
            {
                ucError.ErrorMessage = "Please save the Template first and then try adding the Procedures.";
                ucError.Visible = true;
                return;
            }
            if (General.GetNullableGuid(txtdocumentProcedureId.Text) != null)
            {
                PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateRAEmergencyProcedures(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(ViewState["RISKASSESSMENTMACHINERYID"].ToString()), new Guid(txtdocumentProcedureId.Text), 5);
                ucStatus.Text = "Procedures added.";
                txtdocumentProcedureId.Text = "";
                txtdocumentProcedureName.Text = "";
                BindDocumentProcedures();
            }
            else
            {
                ucError.ErrorMessage = "Please select the Procedures.";
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

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            try
            {
                if (ViewState["RISKASSESSMENTMACHINERYID"].Equals(""))
                {
                    ucError.ErrorMessage = "Add the Template first and then add Hazard.";
                    return;
                }
                if (ViewState["RequestFrom"].ToString() == "EVENT")
                {
                    PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateJHAEventComponents(new Guid(ViewState["EVENTID"].ToString())
                    , General.ReadCheckBoxList(cbEventEquipment)
                    , General.ReadCheckBoxList(cbProcedure)
                    , General.ReadCheckBoxList(cbEventPPE)
                    , General.GetNullableInteger(ddlWorstCase.SelectedValue), 5);

                    ucStatus.Text = "Updated Successfully";
                    BindWorstcaseEvents();
                }
                else
                {
                    PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateJHAImpactComponents(new Guid(ViewState["IMPACTID"].ToString()), General.RadCheckBoxList(chkEquipment), General.ReadCheckBoxList(cblRecomendedPPE), General.RadCheckBoxList(chkEvent), 5);
                    ucStatus.Text = "Equipment added.";
                    BindImpactComponents();
                }
                gvHealthSafetyRisk.Rebind();
                gvEnvironmentalRisk.Rebind();
                gvEconomicRisk.Rebind();
                string script = "function f(){CloseModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                return;
            }
        }
    }

    protected void rblOptionEdit_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rblOptionEdit = (RadioButtonList)sender;

        GridDataItem gvrow = (GridDataItem)rblOptionEdit.Parent.Parent;

        string lblOperationalId = ((RadLabel)gvrow.FindControl("lblOperationalId")).Text;

        PhoenixInspectionOperationalRiskControls.InsertMachineryOperationalRiskControlsTypeUpdate(new Guid(lblOperationalId),
                                                            int.Parse(((RadioButtonList)gvrow.FindControl("rblOptionEdit")).SelectedValue)
                                                            );

        gvMachinerySafety.Rebind();
        gvMachinerySafetyAfter.Rebind();
        gvMachinerySafetyDuring.Rebind();
    }
    public class HSGridDecorator
    {
        public static void MergeRows(RadGrid gridView, GridItemEventArgs e)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                int index = e.Item.ItemIndex;

                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string currentsno = ((RadLabel)gridView.Items[rowIndex].FindControl("lblRownumber")).Text;
                string previoussno = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblRownumber")).Text;

                if (currentsno == previoussno)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                     previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }

                string currentAspects = ((RadLabel)gridView.Items[rowIndex].FindControl("lblAspects")).Text;
                string previousAspects = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblAspects")).Text;

                if (currentAspects == previousAspects)
                {
                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                     previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;
                }

                string currentHazardRisk = ((RadLabel)gridView.Items[rowIndex].FindControl("lblHazardRisk")).Text;
                string previousHazardRisk = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblHazardRisk")).Text;

                if ((currentAspects == previousAspects) && (currentHazardRisk == previousHazardRisk))
                {
                    row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                     previousRow.Cells[4].RowSpan + 1;
                    previousRow.Cells[4].Visible = false;
                }

                string currentHazardType = ((RadLabel)gridView.Items[rowIndex].FindControl("lblHazardType")).Text;
                string previousHazardType = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblHazardType")).Text;

                if ((currentAspects == previousAspects) && (currentHazardType == previousHazardType))
                {
                    row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
                                     previousRow.Cells[5].RowSpan + 1;
                    previousRow.Cells[5].Visible = false;
                }

                string currentUndesirableEvent = ((RadLabel)gridView.Items[rowIndex].FindControl("lblUndesirableEvent")).Text;
                string previousUndesirableEvent = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblUndesirableEvent")).Text;

                if ((currentAspects == previousAspects) && (currentUndesirableEvent == previousUndesirableEvent))
                {
                    row.Cells[6].RowSpan = previousRow.Cells[6].RowSpan < 2 ? 2 :
                                     previousRow.Cells[6].RowSpan + 1;
                    previousRow.Cells[6].Visible = false;
                }

                string currentOperational = ((RadLabel)gridView.Items[rowIndex].FindControl("lblOperational")).Text;
                string previousOperational = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblOperational")).Text;

                if ((currentAspects == previousAspects) && (currentOperational == previousOperational))
                {
                    row.Cells[10].RowSpan = previousRow.Cells[10].RowSpan < 2 ? 2 :
                                     previousRow.Cells[10].RowSpan + 1;
                    previousRow.Cells[10].Visible = false;
                }
            }
        }
    }
    public class ENVGridDecorator
    {
        public static void MergeRows(RadGrid gridView, GridItemEventArgs e)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                int index = e.Item.ItemIndex;

                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string currentsno = ((RadLabel)gridView.Items[rowIndex].FindControl("lblRownumber")).Text;
                string previoussno = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblRownumber")).Text;

                if (currentsno == previoussno)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                     previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }

                string currentAspects = ((RadLabel)gridView.Items[rowIndex].FindControl("lblAspects")).Text;
                string previousAspects = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblAspects")).Text;

                if (currentAspects == previousAspects)
                {
                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                     previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;
                }

                string currentHazardRisk = ((RadLabel)gridView.Items[rowIndex].FindControl("lblHazardRisk")).Text;
                string previousHazardRisk = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblHazardRisk")).Text;

                if ((currentAspects == previousAspects) && (currentHazardRisk == previousHazardRisk))
                {
                    row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                     previousRow.Cells[4].RowSpan + 1;
                    previousRow.Cells[4].Visible = false;
                }

                string currentHazardType = ((RadLabel)gridView.Items[rowIndex].FindControl("lblHazardType")).Text;
                string previousHazardType = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblHazardType")).Text;

                if ((currentAspects == previousAspects) && (currentHazardType == previousHazardType))
                {
                    row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
                                     previousRow.Cells[5].RowSpan + 1;
                    previousRow.Cells[5].Visible = false;
                }

                string currentUndesirableEvent = ((RadLabel)gridView.Items[rowIndex].FindControl("lblUndesirableEvent")).Text;
                string previousUndesirableEvent = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblUndesirableEvent")).Text;

                if ((currentAspects == previousAspects) && (currentUndesirableEvent == previousUndesirableEvent))
                {
                    row.Cells[6].RowSpan = previousRow.Cells[6].RowSpan < 2 ? 2 :
                                     previousRow.Cells[6].RowSpan + 1;
                    previousRow.Cells[6].Visible = false;
                }

                string currentOperational = ((RadLabel)gridView.Items[rowIndex].FindControl("lblOperational")).Text;
                string previousOperational = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblOperational")).Text;

                if ((currentAspects == previousAspects) && (currentOperational == previousOperational))
                {
                    row.Cells[11].RowSpan = previousRow.Cells[11].RowSpan < 2 ? 2 :
                                     previousRow.Cells[11].RowSpan + 1;
                    previousRow.Cells[11].Visible = false;
                }
            }
        }
    }
    public class ECOGridDecorator
    {
        public static void MergeRows(RadGrid gridView, GridItemEventArgs e)
        {
            for (int rowIndex = gridView.Items.Count - 2; rowIndex >= 0; rowIndex--)
            {
                int index = e.Item.ItemIndex;

                GridDataItem row = gridView.Items[rowIndex];
                GridDataItem previousRow = gridView.Items[rowIndex + 1];

                string currentsno = ((RadLabel)gridView.Items[rowIndex].FindControl("lblRownumber")).Text;
                string previoussno = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblRownumber")).Text;

                if (currentsno == previoussno)
                {
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                     previousRow.Cells[2].RowSpan + 1;
                    previousRow.Cells[2].Visible = false;
                }

                string currentAspects = ((RadLabel)gridView.Items[rowIndex].FindControl("lblAspects")).Text;
                string previousAspects = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblAspects")).Text;

                if (currentAspects == previousAspects)
                {
                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                     previousRow.Cells[3].RowSpan + 1;
                    previousRow.Cells[3].Visible = false;
                }

                string currentHazardRisk = ((RadLabel)gridView.Items[rowIndex].FindControl("lblHazardRisk")).Text;
                string previousHazardRisk = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblHazardRisk")).Text;

                if ((currentAspects == previousAspects) && (currentHazardRisk == previousHazardRisk))
                {
                    row.Cells[4].RowSpan = previousRow.Cells[4].RowSpan < 2 ? 2 :
                                     previousRow.Cells[4].RowSpan + 1;
                    previousRow.Cells[4].Visible = false;
                }

                string currentHazardType = ((RadLabel)gridView.Items[rowIndex].FindControl("lblHazardType")).Text;
                string previousHazardType = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblHazardType")).Text;

                if ((currentAspects == previousAspects) && (currentHazardType == previousHazardType))
                {
                    row.Cells[5].RowSpan = previousRow.Cells[5].RowSpan < 2 ? 2 :
                                     previousRow.Cells[5].RowSpan + 1;
                    previousRow.Cells[5].Visible = false;
                }

                string currentUndesirableEvent = ((RadLabel)gridView.Items[rowIndex].FindControl("lblUndesirableEvent")).Text;
                string previousUndesirableEvent = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblUndesirableEvent")).Text;

                if ((currentAspects == previousAspects) && (currentUndesirableEvent == previousUndesirableEvent))
                {
                    row.Cells[6].RowSpan = previousRow.Cells[6].RowSpan < 2 ? 2 :
                                     previousRow.Cells[6].RowSpan + 1;
                    previousRow.Cells[6].Visible = false;
                }

                string currentOperational = ((RadLabel)gridView.Items[rowIndex].FindControl("lblOperational")).Text;
                string previousOperational = ((RadLabel)gridView.Items[rowIndex + 1].FindControl("lblOperational")).Text;

                if ((currentAspects == previousAspects) && (currentOperational == previousOperational))
                {
                    row.Cells[10].RowSpan = previousRow.Cells[10].RowSpan < 2 ? 2 :
                                     previousRow.Cells[10].RowSpan + 1;
                    previousRow.Cells[10].Visible = false;
                }
            }
        }
    }

    protected void BindRecomendedPPE()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            DataSet ds = new DataSet();

            ds = PhoenixInspectionRiskAssessmentMiscellaneousExtn.RiskAssessmentMiscellaneousSearch(null,
                5,
                null, null,
                1,
                500,
                ref iRowCount,
                ref iTotalPageCount);

            if (ds.Tables[0].Rows.Count > 0)
            {
                cblRecomendedPPE.DataSource = ds.Tables[0];
                cblRecomendedPPE.DataBind();

                cbEventPPE.DataSource = ds.Tables[0];
                cbEventPPE.DataBind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void BindWorstcaseEvents()
    {
        try
        {
            if (ViewState["EVENTID"] != null && ViewState["EVENTID"].ToString() != string.Empty)
            {
                DataSet ds = PhoenixInspectionOperationalRiskControls.EditRiskAssessmentworstcase(General.GetNullableGuid(ViewState["EVENTID"].ToString()));
                cbEventEquipment.ClearSelection();
                cbEventPPE.ClearSelection();
                cbProcedure.ClearSelection();
                ddlWorstCase.ClearSelection();
                General.BindCheckBoxList(cbProcedure, ds.Tables[0].Rows[0]["FLDPROCEDUREID"].ToString().ToLower());
                General.BindCheckBoxList(cbEventPPE, ds.Tables[0].Rows[0]["FLDPPE"].ToString());
                General.BindCheckBoxList(cbEventEquipment, ds.Tables[0].Rows[0]["FLDCOMPONENTID"].ToString());
                ddlWorstCase.SelectedValue = ds.Tables[0].Rows[0]["FLDHAZARDID"].ToString();

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void BindProcedure()
    {
        try
        {
            DataSet dss = PhoenixInspectionOperationalRiskControls.UndesirableProcedureList(General.GetNullableGuid(ViewState["EVENTID"].ToString()));
            cbProcedure.DataSource = dss.Tables[0];
            cbProcedure.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void BindEquipment()
    {
        try
        {
            DataSet ds = PhoenixInspectionRiskAssessmentJobHazardExtn.JHAEventEquipmentList(General.GetNullableGuid(ViewState["EVENTID"].ToString()), 5);
            cbEventEquipment.DataSource = ds.Tables[0];
            cbEventEquipment.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void BindWorstCase()
    {
        try
        {
            ddlWorstCase.DataSource = PhoenixInspectionRiskAssessmentHazardExtn.ListRiskAssessmentHazard(3, null);
            ddlWorstCase.DataTextField = "FLDNAME";
            ddlWorstCase.DataValueField = "FLDHAZARDID";
            ddlWorstCase.DataBind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void cbProcedure_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            PhoenixInspectionOperationalRiskControls.UndesirableeventProcedureDelete(new Guid(ViewState["EVENTID"].ToString()), General.ReadCheckBoxList(cbProcedure));
            ucStatus.Text = "deleted.";
            BindProcedure();
            BindWorstcaseEvents();
            gvevent.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void lnkEquipmentAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableGuid(txtEquipmentid.Text) != null)
            {
                //PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateRAComponents(new Guid(ViewState["JOBHAZARDID"].ToString()), new Guid(txtEquipmentid.Text), 1);
                //PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateWorstCaseComponents(new Guid(ViewState["EVENTID"].ToString()), General.GetNullableString(txtEquipmentid.Text));
                //ucStatus.Text = "Component added.";
                //txtEquipmentid.Text = "";
                //txtEquipmentCode.Text = "";
                //txtEquipment.Text = "";
                //BindEquipment();
                //BindWorstcaseEvents();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void lnkProcedureAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (General.GetNullableGuid(txtProcedureid.Text) != null)
            {
                PhoenixInspectionOperationalRiskControls.UpdateUndesirableProcedure(new Guid(ViewState["EVENTID"].ToString()), General.GetNullableGuid(txtProcedureid.Text));
                ucStatus.Text = "Procedure added.";
                txtProcedureid.Text = "";
                txtProcedure.Text = "";
                BindProcedure();
                BindWorstcaseEvents();
                gvevent.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void btnEventsave_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixInspectionRiskAssessmentJobHazardExtn.UpdateJHAEventComponents(new Guid(ViewState["EVENTID"].ToString())
                    , General.ReadCheckBoxList(cbEventEquipment)
                    , General.ReadCheckBoxList(cbProcedure)
                    , General.ReadCheckBoxList(cbEventPPE)
                    , General.GetNullableInteger(ddlWorstCase.SelectedValue), 5);

            ucStatus.Text = "Updated Successfully";
            BindWorstcaseEvents();
            gvevent.Rebind();
            string script = "function f(){CloseModelWindow(); Sys.Application.remove_load(f);}Sys.Application.add_load(f);";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "key", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            return;
        }
    }

}
