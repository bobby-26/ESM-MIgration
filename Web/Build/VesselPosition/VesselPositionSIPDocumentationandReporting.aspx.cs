using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Integration;
using Telerik.Web.UI;

public partial class VesselPositionSIPDocumentationandReporting : PhoenixBasePage
{
    string ToolTip = "Should a vessel, despite its best effort to obtain compliant fuel oil, be unable to do so, the master/Company must present a record of actions taken to attempt to bunker correct fuel oil and provide evidence of an attempt to purchase compliant fuel oil in accordance with its voyage plan and, if it was not made available where planned, that attempts were made to locate alternative sources for such fuel oil and that despite best efforts to obtain compliant fuel oil, no such fuel oil was made available for purchase. Best efforts to procure compliant fuel oil include, but are not limited to, investigating alternate sources of fuel oil prior to commencing the voyage. If, despite best efforts, it was not possible to procure compliant fuel oil, the master/owner must immediately notify the port State Administration of the port of destination and its flag Administration, FONAR (regulation 18.2.4 of MARPOL Annex VI). In order to minimize disruption to commerce and avoid delays, the master/Company should submit fuel oil non-availability report (FONAR) as soon as it is determined or becomes aware that it will not be able to procure and use compliant fuel oil.";
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display: none;");

        SessionUtil.PageAccessRights(this.ViewState);

        bindToolTip();

        PhoenixToolbar toolbartab = new PhoenixToolbar();
        toolbartab.AddFontAwesomeButton("", ToolTip, "<i class=\"fas fa-info-circle\"></i>", "TOOLTIP");
        toolbartab.AddButton("Back", "BACK",ToolBarDirection.Right);
        TabRiskassessmentplan.AccessRights = this.ViewState;
        TabRiskassessmentplan.MenuList = toolbartab.Show();

        PhoenixToolbar toolbabutton = new PhoenixToolbar();
        toolbabutton.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuRiskassessmentplan.AccessRights = this.ViewState;
        MenuRiskassessmentplan.MenuList = toolbabutton.Show();

        if (!IsPostBack)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                UcVessel.Enabled = false;
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            }
            else
            {
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
                UcVessel.SelectedVessel= Request.QueryString["VESSELID"].ToString();
            }
            UcVessel.DataBind();
            UcVessel.bind();

            ViewState["COMPANYID"] = "";
            ViewState["SIPCONFIGID"] = "";
            ViewState["DTKey"] = "";

            if (Request.QueryString["COMPANYID"] != null)
                ViewState["COMPANYID"] = Request.QueryString["COMPANYID"].ToString();

            if (Request.QueryString["SIPCONFIGID"] != null)
                ViewState["SIPCONFIGID"] = Request.QueryString["SIPCONFIGID"].ToString();

            ViewState["SIPDOCUMENTATIONANDREPORTINGID"] = "";
            BindData();

            //imgtooltip.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTip.ToolTip + "', 'visible');");
            //imgtooltip.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTip.ToolTip + "', 'hidden');");



            btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["COMPANYID"].ToString() + "', true); ");
            btnShowDocuments2.Attributes.Add("onclick", "return showPickList('spnPickListDocument2', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["COMPANYID"].ToString() + "', true); ");
            bindOfficeInstruction();

            fuelsystemmodification();
        }
        //BindFormPosters();
        BindFormPosters2();

    }
    private void bindOfficeInstruction()
    {
        DataSet ds = PhoenixVesselPositionSIPConfiguration.EditSIPfficeinstruction();
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtOfficenoncomplientfueldetail.Text = ds.Tables[0].Rows[0]["FLDDOCNONCOMPLIENT"].ToString();
            txtOffiefuelavailablitydetail.Text = ds.Tables[0].Rows[0]["FLDDOCNONAVAILABLITY"].ToString();

        }
    }
    private void bindToolTip()
    {
        try
        {
            DataSet ds = PhoenixRegistersSIPToolTipConfiguration.SIPToolTipConfigList(General.GetNullableInteger(Request.QueryString["SIPCONFIGID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableString(ds.Tables[0].Rows[0]["FLDDISCRIPTION"].ToString()) != null)
                    ToolTip = ds.Tables[0].Rows[0]["FLDDISCRIPTION"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void fuelsystemmodification()
    {
        DataSet ds = PhoenixVesselPositionSIPFuelOilSystemModification.SIPFueloilsystemmodificationsDetailEdit(General.GetNullableInteger(UcVessel.SelectedVessel));
        if (ds.Tables[0].Rows.Count>0)
        {
            if(ds.Tables[0].Rows[0]["FLDSTRUCTUREMODIFYYN"].ToString() != "0")
            {
                LinShipboard.Enabled = false;
                Linkstablity.Enabled = false;
                LinkTrimbook.Enabled = false;
                LinkOthers.Enabled = false;
            }
        }
        else
        {
            LinShipboard.Enabled = false;
            Linkstablity.Enabled = false;
            LinkTrimbook.Enabled = false;
            LinkOthers.Enabled = false;
        }
    }
    private void BindData()
    {
        DataSet ds = PhoenixVesselPositionSIPDocumentationandReporting.SIPDocumentationandReportingEdit(General.GetNullableInteger(ViewState["VESSELID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];


            txtnoncomplientfueldetail.Text = dr["FLDNOCOMPLIENTFUELDETAIL"].ToString();
            txtfuelavailablitydetail.Text = dr["FLDFONONAVAILABLITYREPORT"].ToString();
            ViewState["DTKey"] = dr["FLDDTKEY"].ToString();

            ViewState["SIPDOCUMENTATIONANDREPORTINGID"] = dr["FLDSIPDOCUMENTATIONANDREPORTINGID"].ToString();
        }
    }
    protected void MenuRiskassessmentplan_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (General.GetNullableGuid(ViewState["SIPDOCUMENTATIONANDREPORTINGID"].ToString()) == null)
                {
                    Guid? documentationandreporting = null;
                    PhoenixVesselPositionSIPDocumentationandReporting.InsertSIPDocumentationandReporting(
                        ref documentationandreporting
                        , General.GetNullableInteger(UcVessel.SelectedVessel)
                        , General.GetNullableString(txtnoncomplientfueldetail.Text.Trim())
                        , General.GetNullableString(txtfuelavailablitydetail.Text.Trim())
                        );

                    ViewState["SIPDOCUMENTATIONANDREPORTINGID"] = documentationandreporting;
                }
                else
                {

                    PhoenixVesselPositionSIPDocumentationandReporting.UpdateSIPDocumentationandReporting(
                        General.GetNullableGuid(ViewState["SIPDOCUMENTATIONANDREPORTINGID"].ToString())
                        , General.GetNullableString(txtnoncomplientfueldetail.Text.Trim())
                        , General.GetNullableString(txtfuelavailablitydetail.Text.Trim())
                        );
                }
                BindData();

                ucStatus.Text = "Information saved successfully.";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void TabRiskassessmentplan_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../VesselPosition/VesselPositionSIPConfiguration.aspx");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void lnkFormAdd_Click(object sender, EventArgs e)
    {
        if (General.GetNullableGuid(txtDocumentId.Text) != null)
        {
            PhoenixVesselPositionSIPConfiguration.UpdateSIPDocumentReference(
                          new Guid(txtDocumentId.Text)
                          , int.Parse(ViewState["SIPCONFIGID"].ToString())
                          , int.Parse(ViewState["COMPANYID"].ToString())
                          , 1
                          );
            BindFormPosters();
            txtDocumentId.Text = "";
            txtDocumentName.Text = "";
        }
    }

    protected void BindFormPosters()
    {
        DataSet dss = PhoenixVesselPositionSIPConfiguration.ListSIPDocumentReference(int.Parse(ViewState["SIPCONFIGID"].ToString()), int.Parse(ViewState["COMPANYID"].ToString()), 1);
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
                cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDNAME"].ToString();
                hl.ID = "hlink" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                int type = 0;
                PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDFORMPOSTERID"].ToString()), ref type);
                if (type == 2)
                    hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                else if (type == 3)
                    hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                else if (type == 5)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(hl.ID.ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = ds.Tables[0].Rows[0];
                        hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementFormPreview.aspx?FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + drr["FLDFORMREVISIONID"].ToString() + "');return false;");
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
                                //ifMoreInfo.Attributes["src"] = Session["sitepath"] + "/attachments/" + drRow["FLDFILEPATH"].ToString();                                              
                                hl.Target = "_blank";
                                //hl.NavigateUrl = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + drRow["FLDFILEPATH"].ToString() + "#page=" + 1;
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

    void cb_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox c = (CheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixVesselPositionSIPConfiguration.DeleteSIPDocumentReference(new Guid(c.ID), int.Parse(ViewState["SIPCONFIGID"].ToString()), int.Parse(ViewState["COMPANYID"].ToString()), 1);

            ucStatus.Text = " deleted Successfully.";
            BindFormPosters();
        }
    }
    protected void lnkFormAdd2_Click(object sender, EventArgs e)
    {
        if (General.GetNullableGuid(txtDocumentId2.Text) != null)
        {
            PhoenixVesselPositionSIPConfiguration.UpdateSIPDocumentReference(
                          new Guid(txtDocumentId2.Text)
                          , int.Parse(ViewState["SIPCONFIGID"].ToString())
                          , int.Parse(ViewState["COMPANYID"].ToString())
                          , 2
                          );
            BindFormPosters2();
            txtDocumentId2.Text = "";
            txtDocumentName2.Text = "";
        }
    }

    protected void BindFormPosters2()
    {
        DataSet dss = PhoenixVesselPositionSIPConfiguration.ListSIPDocumentReference(int.Parse(ViewState["SIPCONFIGID"].ToString()), int.Parse(ViewState["COMPANYID"].ToString()), 2);
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tblForms2.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                RadCheckBox cb2 = new RadCheckBox();
                cb2.ID = dr["FLDFORMPOSTERID"].ToString() + "_2";
                cb2.Text = "";
                cb2.Checked = true;
                cb2.AutoPostBack = true;
                cb2.CheckedChanged += new EventHandler(cb2_CheckedChanged);
                HyperLink hl2 = new HyperLink();
                hl2.Text = dr["FLDNAME"].ToString();
                hl2.ID = "hlink2" + number.ToString();
                hl2.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                int type = 0;
                PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDFORMPOSTERID"].ToString()), ref type);
                if (type == 2)
                    hl2.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                else if (type == 3)
                    hl2.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                else if (type == 5)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(hl2.ID.ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = ds.Tables[0].Rows[0];
                        hl2.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementFormPreview.aspx?FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + drr["FLDFORMREVISIONID"].ToString() + "');return false;");
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
                                hl2.Target = "_blank";
                                hl2.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
                            }
                        }
                    }
                }

                HtmlTableRow tr2 = new HtmlTableRow();
                HtmlTableCell tc2 = new HtmlTableCell();
                tc2.Controls.Add(cb2);
                tr2.Cells.Add(tc2);
                tc2 = new HtmlTableCell();
                tc2.Controls.Add(hl2);
                tr2.Cells.Add(tc2);
                tblForms2.Rows.Add(tr2);
                tc2 = new HtmlTableCell();
                tc2.InnerHtml = "<br/>";
                tr2.Cells.Add(tc2);
                tblForms2.Rows.Add(tr2);
                number = number + 1;
            }
            divForms2.Visible = true;
        }
        else
            divForms2.Visible = false;
    }

    void cb2_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox c = (RadCheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixVesselPositionSIPConfiguration.DeleteSIPDocumentReference(new Guid(c.ID.Remove(36)), int.Parse(ViewState["SIPCONFIGID"].ToString()), int.Parse(ViewState["COMPANYID"].ToString()), 2);

            ucStatus.Text = " deleted Successfully.";
            BindFormPosters2();
        }
    }
    protected void lnkFuelNonAvail_Click(object sender, EventArgs e)
    {
        BindData();
        if (ViewState["DTKey"] != null)
        {
            String script = "parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKey"] + "&mod="
                            + PhoenixModule.VESSELPOSITION + "&type=SINONAVAILABILITY')";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
        }
    }
    protected void LinOtherDoc_Click(object sender, EventArgs e)
    {
        BindData();
        if (ViewState["DTKey"] != null)
        {
            String script = "parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKey"] + "&mod="
                            + PhoenixModule.VESSELPOSITION + "&type=SIPOTHERDOC')";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
        }
    }
    protected void LinShipboard_Click(object sender, EventArgs e)
    {
        BindData();
        if (ViewState["DTKey"] != null)
        {
            String script = "parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKey"] + "&mod="
                            + PhoenixModule.VESSELPOSITION + "&type=SIPSIPBOARD')";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
        }
    }
    protected void Linkstablity_Click(object sender, EventArgs e)
    {
        BindData();
        if (ViewState["DTKey"] != null)
        {
            String script = "parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKey"] + "&mod="
                            + PhoenixModule.VESSELPOSITION + "&type=SIPSTABLITY')";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
        }
    }
    protected void LinkTrimbook_Click(object sender, EventArgs e)
    {
        BindData();
        if (ViewState["DTKey"] != null)
        {
            String script = "parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKey"] + "&mod="
                            + PhoenixModule.VESSELPOSITION + "&type=SIPTRIMBOOKLET')";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
        }
    }
    protected void LinkOthers_Click(object sender, EventArgs e)
    {
        BindData();
        if (ViewState["DTKey"] != null)
        {
            String script = "parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKey"] + "&mod="
                            + PhoenixModule.VESSELPOSITION + "&type=SIPOTHERS')";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
        }
    }
    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        BindData();
    }
}