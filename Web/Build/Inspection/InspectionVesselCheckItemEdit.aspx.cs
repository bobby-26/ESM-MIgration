using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System;
using System.Data;
using Telerik.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Integration;
using SouthNests.Phoenix.Inventory;
using System.Web.UI;
using System.Collections.Specialized;

public partial class InspectionVesselCheckItemEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuAddCheckItem.AccessRights = this.ViewState;
            MenuAddCheckItem.MenuList = toolbarmain.Show();
            // ScriptManager.GetCurrent(this).RegisterPostBackControl(MenuAddCheckItem);
            if (!IsPostBack)
            {
                ViewState["VESSELCHECKITEMID"] = "";

                ucVessel.Enabled = true;
                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                    ucVessel.Company = nvc.Get("QMS");
                    ucVessel.bind();
                }

                cbActive.Checked = true;

                if (Request.QueryString["VESSELCHECKITEMID"] != null && Request.QueryString["VESSELCHECKITEMID"].ToString() != string.Empty)
                    ViewState["VESSELCHECKITEMID"] = Request.QueryString["VESSELCHECKITEMID"].ToString();

                int companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
                btnShowdocumentProcedure.Attributes.Add("onclick", "return showPickList('spnPickListdocumentprocedure', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + companyid + "', true); ");
                btnShowFMS.Attributes.Add("onclick", "return showPickList('spnPickListFMS', 'codehelp1', '', '../Common/CommonPickListFMS.aspx?iframignore=true&companyid=" + companyid + "'); ");
                btnShowComponents.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListGlobalComponent.aspx'); ");
                btnShowLocation.Attributes.Add("onclick", "return showPickList('spnPickListLocation','codehelp1','','../Common/CommonPickListGlobalLocation.aspx');");

                BindProcess();
                BindOwner();
                BindClient();
                BindVesselTypeList();
                BindChapters();
                BindResponsibility();
                BindParentItem();
                BindCheckitem();

                if (ViewState["VESSELCHECKITEMID"] != null && ViewState["VESSELCHECKITEMID"].ToString() != string.Empty)
                    btnChaperAdd.Attributes.Add("onclick", "return showPickList('Add','codehelp1','','../Inspection/InspectionCheckItemChapterMappingAdd.aspx?CHECKITEMID=" + ViewState["VESSELCHECKITEMID"] + "');");

            }
            BindComponent();
            BindLocation();
            BindHSEQA();
            BindFormReports();
            BindCheckItemChapter();
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

   

    private void BindChapters()
    {

        ddlPSC.DataSource = PhoenixInspectionRegisterCheckItems.ListCheckItemChapter(PhoenixSecurityContext.CurrentSecurityContext.CompanyID, "PSC");
        ddlPSC.DataTextField = "FLDCHAPTERNAME";
        ddlPSC.DataValueField = "FLDCHAPTERID";
        ddlPSC.DataBind();

    }
    private void BindResponsibility()
    {
        ChkgroupMem.DataSource = PhoenixRegistersGroupRank.ListJHAGroupRank(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ChkgroupMem.DataTextField = "FLDGROUPRANK";
        ChkgroupMem.DataValueField = "FLDGROUPRANKID";
        ChkgroupMem.DataBind();
    }

  
    private void BindVesselTypeList()
    {
        CbVesselType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 81);
        CbVesselType.DataTextField = "FLDHARDNAME";
        CbVesselType.DataValueField = "FLDHARDCODE";
        CbVesselType.DataBind();
    }
    private void BindOwner()
    {
        chkOwner.DataSource = PhoenixRegistersAddress.ListAddress(General.GetNullableString("127"));
        chkOwner.DataTextField = "FLDNAME";
        chkOwner.DataValueField = "FLDADDRESSCODE";
        chkOwner.DataBind();
    }
    private void BindClient()
    {
        chkClient.DataSource = PhoenixRegistersAddress.ListAddress(General.GetNullableString("123"));
        chkClient.DataTextField = "FLDNAME";
        chkClient.DataValueField = "FLDADDRESSCODE";
        chkClient.DataBind();
    }

    private void BindProcess()
    {
        DataSet ds = PhoenixInspectionRiskAssessmentCategoryExtn.ListVesselTypeRiskAssessmentCategory(null);
        if (ds.Tables.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                ChkProcess.Items.Add(new ButtonListItem(dr["FLDNAME"].ToString(), dr["FLDCATEGORYID"].ToString()));
            }
        }
    }

    private DataTable GetActivity(int ElementId)
    {
        DataSet ds = PhoenixInspectionRiskAssessmentActivityExtn.ListRiskAssessmentActivity(ElementId);
        return ds.Tables[0];
    }

    private void UpdateCheckItem()
    {
        try
        {
            if (!IsValidCheckItem())
            {
                ucError.Visible = true;
                return;
            }
            int active = cbActive.Checked == true ? 1 : 0;

            Guid NewCheckitemid = new Guid();



            PhoenixInspectionVesselCheckItems.InsertVesselCheckItem(General.GetNullableGuid(ViewState["VESSELCHECKITEMID"].ToString())
                    ,General.GetNullableInteger(ucVessel.SelectedVessel)
                    , txtItem.Text
                    , General.GetNullableGuid(ddlPSC.SelectedValue)
                    , txtPSC.Text
                    , General.GetNullableGuid(null)
                    , txtVIQ.Text
                    , General.GetNullableGuid(null)
                    , txtCDI.Text
                    , txtDescription.Text
                    , txtGuidence.Text
                    , General.ReadCheckBoxList(cbJobs)
                    , General.ReadCheckBoxList(ChkgroupMem)
                    , active
                    , General.ReadCheckBoxList(chkOwner)
                    , General.ReadCheckBoxList(chkClient)
                    , General.ReadCheckBoxList(CbVesselType)
                    , General.RadCheckBoxList(ChkProcess)
                    , General.RadCheckBoxList(ChkActivity)
                    , ref NewCheckitemid
                    , General.GetNullableGuid(ddlParentitem.SelectedValue)
                    , General.GetNullableInteger(ucDefeciencyCategory.SelectedQuick)
                    , txtVIR.Text);

            ViewState["VESSELCHECKITEMID"] = NewCheckitemid;

            ucStatus.Text = "Updated Check Item successfully";

        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindCheckitem()
    {
        try
        {
            if (ViewState["VESSELCHECKITEMID"] != null && ViewState["VESSELCHECKITEMID"].ToString() != string.Empty)
            {
                DataSet ds = new DataSet();
           
                ds = PhoenixInspectionVesselCheckItems.EditVesselCheckItem(new Guid(ViewState["VESSELCHECKITEMID"].ToString()));
           
                if (ds.Tables[0].Rows.Count > 0)
                {
                    BindJobs();
                    cbJobs.ClearSelection();
                    chkOwner.ClearSelection();
                    chkClient.ClearSelection();
                    CbVesselType.ClearSelection();
           
                    txtItem.Text = ds.Tables[0].Rows[0]["FLDITEM"].ToString();
                    txtReferencenumber.Text = ds.Tables[0].Rows[0]["FLDREFERENCENUMBER"].ToString();
                    ucVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                    ddlPSC.SelectedValue = ds.Tables[0].Rows[0]["FLDPSCCHAPTERID"].ToString();
                    ucVessel.Enabled = false;
                    txtPSC.Text = ds.Tables[0].Rows[0]["FLDPSCCODE"].ToString();
                    txtVIQ.Text = ds.Tables[0].Rows[0]["FLDVIQCODE"].ToString();
                    txtCDI.Text = ds.Tables[0].Rows[0]["FLDCDICODE"].ToString();
                    txtVIR.Text = ds.Tables[0].Rows[0]["FLDVIRCODE"].ToString();
                    txtDescription.Text = ds.Tables[0].Rows[0]["FLDDESCRIPTION"].ToString();
                    txtGuidence.Text = ds.Tables[0].Rows[0]["FLDGUIDENCENOTE"].ToString();
                    ViewState["CHECKITEMID"] = ds.Tables[0].Rows[0]["FLDINSPECTIONVESSELCHECKITEMID"].ToString();
                    General.BindCheckBoxList(ChkgroupMem, ds.Tables[0].Rows[0]["FLDRESPONSIBILITY"].ToString());
                    cbActive.Checked = ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1" ? true : false;
                    General.BindCheckBoxList(cbJobs, ds.Tables[0].Rows[0]["FLDJOBMAPPING"].ToString().ToLower());
                    General.BindCheckBoxList(chkOwner, ds.Tables[0].Rows[0]["FLDOWNER"].ToString().ToLower());
                    General.BindCheckBoxList(chkClient, ds.Tables[0].Rows[0]["FLDCLIENT"].ToString().ToLower());
                    General.BindCheckBoxList(CbVesselType, ds.Tables[0].Rows[0]["FLDVESSELTYPE"].ToString().ToLower());
                    General.RadBindCheckBoxList(ChkProcess, ds.Tables[0].Rows[0]["FLDPROCESS"].ToString().ToLower());
                    ChkActivity.DataSource = null;
                    ChkActivity.Items.Clear();
                    foreach (var item in ChkProcess.SelectedItems)
                    {
                        DataTable dt = GetActivity(int.Parse(item.Value));
                        foreach (DataRow dr in dt.Rows)
                        {
                            ButtonListItem itm = new ButtonListItem(dr["FLDNAME"].ToString(), dr["FLDACTIVITYID"].ToString() + "~" + dr["FLDCATEGORYID"].ToString());
                            ChkActivity.Items.Add(itm);
                        }
                    }
                    General.RadBindCheckBoxList(ChkActivity, ds.Tables[0].Rows[0]["FLDACTIVITY"].ToString().ToLower());
                    ddlParentitem.SelectedValue = ds.Tables[0].Rows[0]["FLDPARENTITEMID"].ToString();
                    ucDefeciencyCategory.SelectedQuick = ds.Tables[0].Rows[0]["FLDDEFICIENCYCATEGORYID"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    private bool IsValidCheckItem()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableString(ucDefeciencyCategory.SelectedQuick) == null)
            ucError.ErrorMessage = "Defeciency Category is required.";

        if (General.GetNullableString(txtItem.Text) == null)
            ucError.ErrorMessage = "Item is required.";

        if (General.GetNullableString(txtDescription.Text) == null)
            ucError.ErrorMessage = "Description is required.";

        if (General.GetNullableString(txtGuidence.Text) == null)
            ucError.ErrorMessage = "Guidence Note is required.";

        if (General.ReadCheckBoxList(ChkgroupMem) == null)
            ucError.ErrorMessage = "Responsibility pearson is required";

        return (!ucError.IsError);
    }


    protected void ChkProcess_SelectedIndexChanged(object sender, EventArgs e)
    {
        ChkActivity.DataSource = null;
        ChkActivity.Items.Clear();
        foreach (var item in ChkProcess.SelectedItems)
        {
            DataTable dt = GetActivity(int.Parse(item.Value));
            foreach (DataRow dr in dt.Rows)
            {
                ButtonListItem itm = new ButtonListItem(dr["FLDNAME"].ToString(), dr["FLDACTIVITYID"].ToString() + "~" + dr["FLDCATEGORYID"].ToString());
                ChkActivity.Items.Add(itm);
            }
        }
        BindCheckitem();

    }

    protected void MenuAddCheckItem_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Inspection/InspectionVesselCheckItemList.aspx");
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                UpdateCheckItem();
                BindCheckitem();
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    protected void lnkComponentAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["VESSELCHECKITEMID"] != null && ViewState["VESSELCHECKITEMID"].ToString() != string.Empty)
            {
                PhoenixInspectionVesselCheckItems.VesselCheckitemComponentAdd(new Guid(ViewState["VESSELCHECKITEMID"].ToString()), new Guid(txtComponentId.Text));

                txtComponentId.Text = "";
                txtComponentName.Text = "";
                txtComponentCode.Text = "";

                BindComponent();
                BindCheckitem();
            }
            else
            {
                ucError.ErrorMessage = "Please create the checkitem.";
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindComponent()
    {
        try
        {
            if (ViewState["VESSELCHECKITEMID"] != null && ViewState["VESSELCHECKITEMID"].ToString() != string.Empty)
            {
                DataSet dss = PhoenixInspectionVesselCheckItems.VesselCheckitemComponentsList(new Guid(ViewState["VESSELCHECKITEMID"].ToString()));
                int number = 1;
                if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
                {
                    tblComponents.Rows.Clear();
                    foreach (DataRow dr in dss.Tables[0].Rows)
                    {
                        CheckBox cb = new CheckBox();
                        cb.ID = dr["FLDCOMPONENTID"].ToString();
                        cb.Text = "";
                        cb.Checked = true;
                        cb.AutoPostBack = true;
                        cb.CheckedChanged += new EventHandler(cbComponent_CheckedChanged);
                        HyperLink hl = new HyperLink();
                        hl.Text = dr["FLDNAME"].ToString();
                        hl.ID = "hlink2" + number.ToString();
                        hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                        hl.Attributes.Add("onclick", "openNewWindow('codehelp1', '','" + Session["sitepath"] + "/Inspection/InspectionCheckitemChildComponentList.aspx?COMPONENTID=" + dr["FLDCOMPONENTID"].ToString() + "','false','650px','400px');return false;");
                        HtmlTableRow tr = new HtmlTableRow();
                        HtmlTableCell tc = new HtmlTableCell();
                        tc.Controls.Add(cb);
                        tr.Cells.Add(tc);
                        tc = new HtmlTableCell();
                        tc.Controls.Add(hl);
                        tr.Cells.Add(tc);
                        tblComponents.Rows.Add(tr);
                        tc = new HtmlTableCell();
                        tc.InnerHtml = "<br/>";
                        tr.Cells.Add(tc);
                        tblComponents.Rows.Add(tr);
                        number = number + 1;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    void cbComponent_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox cc = (CheckBox)sender;
            if (cc.Checked == false)
            {
                PhoenixInspectionVesselCheckItems.VesselCheckitemComponentDelete(new Guid(ViewState["VESSELCHECKITEMID"].ToString()), new Guid(cc.ID));

                ucStatus.Text = "Component deleted.";

                BindCheckitem();
                BindComponent();
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindJobs()
    {
        if (ViewState["VESSELCHECKITEMID"] != null && ViewState["VESSELCHECKITEMID"].ToString() != string.Empty)
        {
            cbJobs.DataSource = PhoenixInspectionVesselCheckItems.VesselcheckitemJobList(new Guid(ViewState["VESSELCHECKITEMID"].ToString()));
            cbJobs.DataTextField = "FLDJOBTITLE";
            cbJobs.DataValueField = "FLDGLOBALCOMPONENTJOBID";
            cbJobs.DataBind();
        }
    }


    protected void lnkLocationAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["VESSELCHECKITEMID"] != null && ViewState["VESSELCHECKITEMID"].ToString() != string.Empty)
            {
                PhoenixInspectionVesselCheckItems.VesselCheckitemLocationAdd(new Guid(ViewState["VESSELCHECKITEMID"].ToString()), new Guid(txtLocationId.Text));

                txtLocationId.Text = "";
                txtLocationCode.Text = "";
                txtLocationName.Text = "";

                BindLocation();
                BindCheckitem();
            }
            else
            {
                ucError.ErrorMessage = "Please create the checkitem.";
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindLocation()
    {
        try
        {
            if (ViewState["VESSELCHECKITEMID"] != null && ViewState["VESSELCHECKITEMID"].ToString() != string.Empty)
            {
                DataSet ds1 = PhoenixInspectionVesselCheckItems.VesselCheckitemLocationList(new Guid(ViewState["VESSELCHECKITEMID"].ToString()));
                int number = 1;
                if (ds1.Tables[0].Rows.Count > 0 && ds1.Tables[0].Columns.Count > 1)
                {
                    tblLocation.Rows.Clear();
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        CheckBox cb = new CheckBox();
                        cb.ID = "cblcomp" + dr["FLDLOCATIONID"].ToString();
                        cb.Text = "";
                        cb.Checked = true;
                        cb.AutoPostBack = true;
                        cb.CheckedChanged += new EventHandler(cbLocation_CheckedChanged);
                        HyperLink hl = new HyperLink();
                        hl.Text = dr["FLDNAME"].ToString();
                        hl.ID = "hlink3" + number.ToString();
                        hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                        HtmlTableRow tr = new HtmlTableRow();
                        HtmlTableCell tc = new HtmlTableCell();
                        tc.Controls.Add(cb);
                        tr.Cells.Add(tc);
                        tc = new HtmlTableCell();
                        tc.Controls.Add(hl);
                        tr.Cells.Add(tc);
                        tblLocation.Rows.Add(tr);
                        tc = new HtmlTableCell();
                        tc.InnerHtml = "<br/>";
                        tr.Cells.Add(tc);
                        tblLocation.Rows.Add(tr);
                        number = number + 1;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }


    }

    void cbLocation_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox ch = (CheckBox)sender;
            if (ch.Checked == false)
            {
                string chs = ch.ID.ToString();
                chs = chs.Replace("cblcomp", "");
                PhoenixInspectionVesselCheckItems.VesselCheckitemLocationDelete(new Guid(ViewState["VESSELCHECKITEMID"].ToString()), new Guid(chs));

                ucStatus.Text = "Location deleted.";

                BindCheckitem();
                BindLocation();
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void lnkDocumentAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["VESSELCHECKITEMID"] != null && ViewState["VESSELCHECKITEMID"].ToString() != string.Empty)
            {
                PhoenixInspectionVesselCheckItems.VesselCheckitemProcedureFormsAdd(new Guid(ViewState["VESSELCHECKITEMID"].ToString()), new Guid(txtdocumentProcedureId.Text));

                txtdocumentProcedureId.Text = "";
                txtdocumentProcedureName.Text = "";
                BindHSEQA();
            }
            else
            {
                ucError.ErrorMessage = "Please create the checkitem.";
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void BindHSEQA()
    {
        try
        {
            if (ViewState["VESSELCHECKITEMID"] != null && ViewState["VESSELCHECKITEMID"].ToString() != string.Empty)
            {
                DataSet dss = PhoenixInspectionVesselCheckItems.VesselCheckitemProcedureFormsList(new Guid(ViewState["VESSELCHECKITEMID"].ToString()));
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
                        doccb.AutoPostBack = true;
                        if (ViewState["status"] != null && ViewState["status"].ToString().Equals("3"))
                            doccb.Enabled = false;
                        doccb.CheckedChanged += new EventHandler(doc_CheckedChanged);
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
                }
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    void doc_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox c = (CheckBox)sender;
            if (c.Checked == false)
            {
                PhoenixInspectionVesselCheckItems.VesselCheckitemProcedureFormsDelete(new Guid(ViewState["VESSELCHECKITEMID"].ToString()), new Guid(c.ID));

                ucStatus.Text = "Forms Checklist deleted.";
                BindCheckitem();
                BindHSEQA();
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void lnkReportAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (ViewState["VESSELCHECKITEMID"] != null && ViewState["VESSELCHECKITEMID"].ToString() != string.Empty)
            {
                PhoenixInspectionVesselCheckItems.VesselCheckitemFilingSystemAdd(new Guid(ViewState["VESSELCHECKITEMID"].ToString()), new Guid(txtReportId.Text));

                ucStatus.Text = "Reports added.";
                txtReportId.Text = "";
                txtReportName.Text = "";

                BindFormReports();
                BindCheckitem();
            }
            else
            {
                ucError.ErrorMessage = "Please create the checkitem.";
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }

    }

    protected void BindFormReports()
    {
        try
        {
            if (ViewState["VESSELCHECKITEMID"] != null && ViewState["VESSELCHECKITEMID"].ToString() != string.Empty)
            {
                DataSet dss = PhoenixInspectionVesselCheckItems.VesselCheckitemFilingSystemList(new Guid(ViewState["VESSELCHECKITEMID"].ToString()));
                int number = 1;
                if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
                {
                    tblReports.Rows.Clear();
                    foreach (DataRow dr in dss.Tables[0].Rows)
                    {
                        CheckBox cbreport = new CheckBox();
                        cbreport.ID = dr["FLDFORMREPORTID"].ToString();
                        cbreport.Text = "";
                        cbreport.Checked = true;
                        cbreport.AutoPostBack = true;
                        cbreport.CheckedChanged += new EventHandler(cbreport_CheckedChanged);
                        HyperLink hlreport = new HyperLink();
                        hlreport.Text = dr["FLDNAME"].ToString();
                        hlreport.ID = "reportlink" + number.ToString();
                        hlreport.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                        hlreport.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&ReportId=" + dr["FLDFORMREPORTID"].ToString() + "');return false;");
                        HtmlTableRow tr = new HtmlTableRow();
                        HtmlTableCell tc = new HtmlTableCell();
                        tc.Controls.Add(cbreport);
                        tr.Cells.Add(tc);
                        tc = new HtmlTableCell();
                        tc.Controls.Add(hlreport);
                        tr.Cells.Add(tc);
                        tblReports.Rows.Add(tr);
                        tc = new HtmlTableCell();
                        tc.InnerHtml = "<br/>";
                        tr.Cells.Add(tc);
                        tblReports.Rows.Add(tr);
                        number = number + 1;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }

    }
    void cbreport_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox c = (CheckBox)sender;
            if (c.Checked == false)
            {
                PhoenixInspectionVesselCheckItems.VesselCheckitemFilingSystemDelete(new Guid(ViewState["VESSELCHECKITEMID"].ToString()), new Guid(c.ID));

                ucStatus.Text = "Report form deleted.";

                BindFormReports();
                BindCheckitem();
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void BindParentItem()
    {
        ddlParentitem.DataSource = PhoenixInspectionRegisterCheckItems.ListCheckItems();
        ddlParentitem.DataTextField = "FLDITEM";
        ddlParentitem.DataValueField = "FLDINSPECTIONCHECKITEMID";
        ddlParentitem.DataBind();
    }
    private void BindCheckItemChapter()
    {
        try
        {
            if (ViewState["VESSELCHECKITEMID"] != null && ViewState["VESSELCHECKITEMID"].ToString() != string.Empty)
            {
                DataSet dss = PhoenixInspectionVesselCheckItems.ChapterMappingList(new Guid(ViewState["VESSELCHECKITEMID"].ToString()));
                int number = 1;
                if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
                {
                    tblChapter.Rows.Clear();
                    foreach (DataRow dr in dss.Tables[0].Rows)
                    {
                        CheckBox Chapter = new CheckBox();
                        Chapter.ID = dr["FLDCHECKITEMCHAPTERID"].ToString();
                        Chapter.Text = "";
                        Chapter.Checked = true;
                        Chapter.AutoPostBack = true;
                        Chapter.CheckedChanged += new EventHandler(cbChapter_CheckedChanged);
                        Label l1 = new Label();
                        l1.Text = dr["CHAPTERCODE"].ToString();
                        // HyperLink hl = new HyperLink();
                        // hl.Text = dr["CHAPTERCODE"].ToString()
                        // hl.ID = "hlink5" + number.ToString();
                        // hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                        HtmlTableRow tr = new HtmlTableRow();
                        HtmlTableCell tc = new HtmlTableCell();
                        tc.Controls.Add(Chapter);
                        tr.Cells.Add(tc);
                        tc = new HtmlTableCell();
                        tc.Controls.Add(l1);
                        tr.Cells.Add(tc);
                        tblComponents.Rows.Add(tr);
                        tc = new HtmlTableCell();
                        tc.InnerHtml = "<br/>";
                        tr.Cells.Add(tc);
                        tblChapter.Rows.Add(tr);
                        number = number + 1;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    void cbChapter_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            CheckBox Chkcha = (CheckBox)sender;
            if (Chkcha.Checked == false)
            {
                PhoenixInspectionRegisterCheckItems.ChapterMappingDelete(new Guid(Chkcha.ID));

                ucStatus.Text = "Chapter deleted.";
                                
                BindCheckitem();
                BindCheckItemChapter();
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindCheckitem();
        BindCheckItemChapter();
    }
}
