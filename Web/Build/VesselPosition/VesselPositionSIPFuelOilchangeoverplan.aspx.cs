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

public partial class VesselPositionSIPFuelOilchangeoverplan : PhoenixBasePage
{
    string tooltip = "Consider whether a ship-specific fuel changeover plan is to be made available. The plan should include measures to offload or consume any remaining non-compliant fuel oil. The plan should also demonstrate how the ship intends to ensure that all its combustion units will be using compliant fuel oil no later than 1 January 2020.";
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        bindToolTip();
        PhoenixToolbar toolbartab = new PhoenixToolbar();
        toolbartab.AddFontAwesomeButton("", tooltip, "<i class=\"fas fa-info-circle\"></i>", "TOOLTIP");

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

                UcVessel.SelectedVessel = Request.QueryString["VESSELID"];
                ViewState["VESSELID"] = Request.QueryString["VESSELID"];
            }
            UcVessel.DataBind();
            UcVessel.bind();

            ViewState["COMPANYID"] = "";
            ViewState["SIPCONFIGID"] = "";
            ViewState["DTKey"] = "";

            ViewState["SIPCONFIGDTKEY"] = Request.QueryString["DTKEY"].ToString();

            if (Request.QueryString["COMPANYID"] != null)
                ViewState["COMPANYID"] = Request.QueryString["COMPANYID"].ToString();

            if (Request.QueryString["SIPCONFIGID"] != null)
                ViewState["SIPCONFIGID"] = Request.QueryString["SIPCONFIGID"].ToString();

            ViewState["SIPFUELOILCHANGEOVERID"] = "";
            BindData();
            bindOfficeInstruction();
        }
        BindFormPosters();
        BindFormPosters2();
    }
    private void bindOfficeInstruction()
    {
        DataSet ds = PhoenixVesselPositionSIPConfiguration.EditSIPfficeinstruction();
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtOfficestsdetail.Text = ds.Tables[0].Rows[0]["FLDFUELCHANGEOVER"].ToString();
            txtOfficetrainingdetails.Text = ds.Tables[0].Rows[0]["FLDTRAININGDETAIL"].ToString();

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
                    tooltip = ds.Tables[0].Rows[0]["FLDDISCRIPTION"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindData()
    {
        DataSet ds = PhoenixVesselPositionSIPFuelOilchangeoverplan.SIPSIPFuelOilchangeoverplanEdit(General.GetNullableInteger(ViewState["VESSELID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];


            //chkstsyn.Checked = dr["FLDSTSTRANSFERYN"].ToString() == "1" ? true : false;
            rdstsyn.SelectedValue = dr["FLDSTSTRANSFERYN"].ToString();
            txtstsdetail.Text = dr["FLDSTSDETAIL"].ToString();
            txtmaxperiodreq.Text = dr["FLDMAXIMUMPERIODREQ"].ToString();
            UcETDChangeoverDate.Text = dr["FLDCHANGEOVERETD"].ToString();
            //chktrainingreq.Checked = dr["FLDTRAININGNEEDED"].ToString() == "1" ? true : false;
            rdtrainingreq.SelectedValue = dr["FLDTRAININGNEEDED"].ToString();
            txttrainingdetails.Text = dr["FLDTRAININGDETAILS"].ToString();

            ViewState["DTKey"] = dr["FLDDTKEY"].ToString();

            ViewState["SIPFUELOILCHANGEOVERID"] = dr["FLDSIPFUELOILCHANGEOVERID"].ToString();
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
                if (General.GetNullableGuid(ViewState["SIPFUELOILCHANGEOVERID"].ToString()) == null)
                {
                    Guid? sipfueloilchangeoverid = null;
                    PhoenixVesselPositionSIPFuelOilchangeoverplan.InsertSIPFuelOilchangeoverplan(
                        ref sipfueloilchangeoverid
                        ,General.GetNullableInteger(UcVessel.SelectedVessel)
                        , int.Parse(rdstsyn.SelectedValue)// chkstsyn.Checked == true ? 1 : 0
                        , General.GetNullableString(txtstsdetail.Text.Trim())
                        , General.GetNullableString(txtmaxperiodreq.Text)
                        , General.GetNullableDateTime(UcETDChangeoverDate.Text)
                        , int.Parse(rdtrainingreq.SelectedValue)//chktrainingreq.Checked == true ? 1 : 0
                        ,General.GetNullableString(txttrainingdetails.Text.Trim())
                        );

                    ViewState["SIPFUELOILCHANGEOVERID"] = sipfueloilchangeoverid;
                }
                else
                {

                    PhoenixVesselPositionSIPFuelOilchangeoverplan.UpdateSIPFuelOilchangeoverplan(
                        General.GetNullableGuid(ViewState["SIPFUELOILCHANGEOVERID"].ToString())
                        , int.Parse(rdstsyn.SelectedValue)//chkstsyn.Checked == true ? 1 : 0
                        , General.GetNullableString(txtstsdetail.Text.Trim())
                        , General.GetNullableString(txtmaxperiodreq.Text)
                        , General.GetNullableDateTime(UcETDChangeoverDate.Text)
                        , int.Parse(rdtrainingreq.SelectedValue)//chktrainingreq.Checked == true ? 1 : 0
                        , General.GetNullableString(txttrainingdetails.Text.Trim())
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
                                hl.NavigateUrl = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + drRow["FLDFILEPATH"].ToString() + "#page=" + 1;
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
                CheckBox cb2 = new CheckBox();
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
                    hl2.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                else if (type == 3)
                    hl2.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                else if (type == 5)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(hl2.ID.ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = ds.Tables[0].Rows[0];
                        hl2.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementFormPreview.aspx?FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + drr["FLDFORMREVISIONID"].ToString() + "');return false;");
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
                                hl2.Target = "_blank";
                                hl2.NavigateUrl = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + drRow["FLDFILEPATH"].ToString() + "#page=" + 1;
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
        CheckBox c = (CheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixVesselPositionSIPConfiguration.DeleteSIPDocumentReference(new Guid(c.ID.Remove(36)), int.Parse(ViewState["SIPCONFIGID"].ToString()), int.Parse(ViewState["COMPANYID"].ToString()), 2);

            ucStatus.Text = " deleted Successfully.";
            BindFormPosters2();
        }
    }
    protected void linkFuelChangeover_Click(object sender, EventArgs e)
    {
        BindData();
        if (ViewState["DTKey"] != null)
        {
            String script = "parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKey"] + "&mod="
                            + PhoenixModule.VESSELPOSITION + "&type=SIPFUELCHANGEOVER')";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
        }
    }
    protected void lnkTraining_Click(object sender, EventArgs e)
    {
        BindData();
        if (ViewState["DTKey"] != null)
        {
            String script = "parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["SIPCONFIGDTKEY"] + "&mod="
                            + PhoenixModule.VESSELPOSITION + "&type=SIPTRAINING')";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
        }
    }
}