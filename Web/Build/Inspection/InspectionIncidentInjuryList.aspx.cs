using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class InspectionIncidentInjuryList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                SessionUtil.PageAccessRights(this.ViewState);
                VesselConfiguration();
                BindInjured();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvIncidentInjury.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                ViewState["INJURYID"] = null;
                ViewState["INCIDENTVESSELID"] = null;
                ViewState["VESSELID"] = "";
                ViewState["NEW"] = "false";
                ucHealthSafetyCategory.Type = "1";
                ucHealthSafetyCategory.DataBind();

                ucHealthSafetyCategory.HazardTypeList = PhoenixInspectionRiskAssessmentHazard.ListRiskAssessmentHazard(int.Parse("1"), 0);
                BindHealthSafetySubCategory();

                if (Request.QueryString["INJURYID"] != null && Request.QueryString["INJURYID"].ToString() != string.Empty)
                    ViewState["INJURYID"] = Request.QueryString["INJURYID"].ToString();
                if (Request.QueryString["MEDICALCASEID"] != null && Request.QueryString["MEDICALCASEID"].ToString() != "")
                    ViewState["MEDICALCASEID"] = Request.QueryString["MEDICALCASEID"].ToString();

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                }
                else
                    ViewState["VESSELID"] = "Dummy";

                BindVessel();
                BindWorkInjuryCategory();               
                

                if (ViewState["MEDICALCASEID"] != null && !string.IsNullOrEmpty(ViewState["MEDICALCASEID"].ToString()))
                {
                    lnkSicknessReport.Visible = true;
                    lnkSicknessReport.Attributes.Add("onclick", "openNewWindow('codehelp','', '" + Session["sitepath"] + "/Reports/ReportsView.aspx?applicationcode=9&reportcode=SICKNESSREPORT&showexcel=no&pniid=" + ViewState["MEDICALCASEID"].ToString() + "');return true;");
                }
                else
                    lnkSicknessReport.Visible = false;

                imgShowCrewInCharge.Attributes.Add("onclick",
               "return showPickList('spnCrewInCharge', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListVesselCrewListByDate.aspx?VesselId="
               + General.GetNullableInteger(ViewState["VESSELID"].ToString()) + "&Date=" + General.GetNullableDateTime(ViewState["INCIDENTDATE"].ToString()) + "&framename=ifMoreInfo', false); ");

                imgPersonOffice.Attributes.Add("onclick", "return showPickList('spnPersonInChargeOffice', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListUser.aspx?departmentlist=7,8&mod="
                + PhoenixModule.QUALITY + "&framename=ifMoreInfo', false);");
            }

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionIncidentInjuryList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvIncidentInjury')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuGeneral.AccessRights = this.ViewState;
            MenuGeneral.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1 = new PhoenixToolbar();
            toolbar1.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar1.AddButton("New", "NEW", ToolBarDirection.Right);
            MenuIncidentInjuryGeneral.AccessRights = this.ViewState;
            if (Filter.CurrentSelectedIncidentMenu == null)
                MenuIncidentInjuryGeneral.MenuList = toolbar1.Show();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCREWNAME", "FLDAGE", "FLDRANKNAME", "FLDPARTNAMEOFTHEBODYINJURED", "FLDTYPEOFINJURYNAME", "FLDCATEGORYNAMEOFWORKINJURY", "FLDMANHOURSLOST", "FLDESTIMATEDCOST", "FLDCATEGORY" };
        string[] alCaptions = { " Injured's Name", "Age", "Designation", "Part of Body Injured", "Type of Injury", "Category of Work Injury", "Work Days Lost", "Estimated Cost in USD", "Consequence/Potential Category" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixInspectionIncidentInjury.IncidentInjurySearch(
            General.GetNullableGuid(Filter.CurrentIncidentID)
            , sortexpression
            , sortdirection
            , 1
            , iRowCount
            , ref iRowCount
            , ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=IncidentInjuryList.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Incident Injury List</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    protected void Rebind()
    {
        gvIncidentInjury.SelectedIndexes.Clear();
        gvIncidentInjury.EditIndexes.Clear();
        gvIncidentInjury.DataSource = null;
        gvIncidentInjury.Rebind();
    }
    protected void MenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
        else if (CommandName.ToUpper().Equals("CLEAR"))
        {
            Filter.CurrentIncidentFilterCriteria = null;
            Rebind();
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDCREWNAME", "FLDAGE", "FLDRANKNAME", "FLDPARTNAMEOFTHEBODYINJURED", "FLDTYPEOFINJURYNAME", "FLDCATEGORYNAMEOFWORKINJURY", "FLDMANHOURSLOST", "FLDESTIMATEDCOST", "FLDCATEGORY" };
        string[] alCaptions = { " Injured's Name", "Age", "Designation", "Part of Body Injured", "Type of Injury", "Category of Work Injury", "Work Days Lost", "Estimated Cost in USD", "Consequence/Potential Category" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionIncidentInjury.IncidentInjurySearch(
         General.GetNullableGuid(Filter.CurrentIncidentID)
         , sortexpression
         , sortdirection
         , (int)ViewState["PAGENUMBER"]
         , gvIncidentInjury.PageSize
         , ref iRowCount
         , ref iTotalPageCount);

        General.SetPrintOptions("gvIncidentInjury", "Incident Injury List", alCaptions, alColumns, ds);

        if (ds.Tables[0].Rows.Count > 0)
        {
            if ((ViewState["INJURYID"] == null) && (ViewState["NEW"].ToString() == "false"))
            {
                gvIncidentInjury.SelectedIndexes.Clear();
                ViewState["INJURYID"] = ds.Tables[0].Rows[0]["FLDINSPECTIONINCIDENTINJURYID"].ToString();
                ViewState["MEDICALCASEID"] = ds.Tables[0].Rows[0]["FLDMEDICALCASEID"].ToString();
                EditIncidentInjury();
            }
            SetRowSelection();
        }
        else
        {
            ds.Tables[0].Rows.Clear();
            DataTable dt = ds.Tables[0];

            ViewState["NEW"] = "true";
            ViewState["INJURYID"] = null;
            ViewState["MEDICALCASEID"] = null;
        }

        gvIncidentInjury.DataSource = ds;
        gvIncidentInjury.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
    protected void gvIncidentInjury_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                ViewState["INJURYID"] = ((RadLabel)e.Item.FindControl("lblIncidentInjuryId")).Text;
                ViewState["MEDICALCASEID"] = ((RadLabel)e.Item.FindControl("lblMedicalCaseId")).Text;
                e.Item.Selected = true;
                EditIncidentInjury();
            }
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                ViewState["INJURYID"] = ((RadLabel)e.Item.FindControl("lblIncidentInjuryId")).Text;
                ViewState["MEDICALCASEID"] = ((RadLabel)e.Item.FindControl("lblMedicalCaseId")).Text;
                e.Item.Selected = true;
                ViewState["NEW"] = "false";
                EditIncidentInjury();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string incidentinjuryid = ((RadLabel)e.Item.FindControl("lblIncidentInjuryId")).Text;
                ViewState["INJURYID"] = null;
                e.Item.Selected = true;
                DeleteIncidentInjury(PhoenixSecurityContext.CurrentSecurityContext.UserCode, incidentinjuryid);
                EditIncidentInjury();
                Rebind();
            }
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        gvIncidentInjury.SelectedIndexes.Clear();
        for (int i = 0; i < gvIncidentInjury.Items.Count; i++)
        {
            if (gvIncidentInjury.MasterTableView.Items[i].GetDataKeyValue("FLDINSPECTIONINCIDENTINJURYID").ToString().Equals(ViewState["INJURYID"].ToString()))
            {
                gvIncidentInjury.SelectedIndexes.Clear();
                ViewState["INJURYID"] = ((RadLabel)gvIncidentInjury.Items[i].FindControl("lblIncidentInjuryId")).Text;
                ViewState["MEDICALCASEID"] = ((RadLabel)gvIncidentInjury.Items[i].FindControl("lblMedicalCaseId")).Text;
                gvIncidentInjury.MasterTableView.Items[i].Selected = true;
                break;
            }
        }
    }

    private void BindPageURL(int rowindex)
    {
        try
        {
            ViewState["INJURYID"] = ((RadLabel)gvIncidentInjury.Items[rowindex].FindControl("lblIncidentInjuryId")).Text;
            ViewState["MEDICALCASEID"] = ((RadLabel)gvIncidentInjury.Items[rowindex].FindControl("lblMedicalCaseId")).Text;
            SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DeleteIncidentInjury(int rowusercode, string inspectionincidentinjuryid)
    {
        PhoenixInspectionIncidentInjury.DeleteIncidentInjury(rowusercode, new Guid(inspectionincidentinjuryid));
        Rebind();
    }
    protected void gvIncidentInjury_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

            if (db != null)
            {
                if (Filter.CurrentSelectedIncidentMenu == null)
                    db.Visible = true;
                else
                    db.Visible = false;
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);

                HtmlGenericControl html = new HtmlGenericControl();

                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }
                else
                {
                    html.InnerHtml = "<span class=\"icon\" style=\"color:skyblue\"><i class=\"fas fa-paperclip\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "openNewWindow('NAFA','', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                      + PhoenixModule.QUALITY + "&type=INJURYREPORT" + "&cmdname=SICKNESSREPORTUPLOAD&VESSELID=" + drv["FLDVESSELID"].ToString() + "'); return true;");
            }
        }
    }
    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }

    protected void gvIncidentInjury_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvIncidentInjury.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["INJURYID"] = null;
        ViewState["PAGENUMBER"] = 1;
        Rebind();
    }
    protected void SetPickList(string vslid)
    {
        if (vslid == "0")
        {
            spnCrewInCharge.Visible = false;
            spnPersonInChargeOffice.Visible = true;
        }
        else
        {
            spnCrewInCharge.Visible = true;
            spnPersonInChargeOffice.Visible = false;
        }
    }
    protected void BindHealthSafetySubCategory()
    {
        DataTable dt = PhoenixInspectionIncident.ListRiskAssessmentSubHazard(General.GetNullableInteger(ucHealthSafetyCategory.SelectedHazardType));
        ddlHealthSafetySubCategory.Items.Clear();
        ddlHealthSafetySubCategory.DataSource = dt;
        ddlHealthSafetySubCategory.DataTextField = "FLDNAME";
        ddlHealthSafetySubCategory.DataValueField = "FLDSUBHAZARDID";
        ddlHealthSafetySubCategory.DataBind();
        ddlHealthSafetySubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        ddlHealthSafetySubCategory.SelectedIndex = 0;
    }

    protected void ucHealthSafetyCategory_Changed(object sender, EventArgs e)
    {
        BindHealthSafetySubCategory();        
    }

    protected void BindVessel()
    {
        DataSet ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ViewState["INCIDENTDATE"] = ds.Tables[0].Rows[0]["FLDDATEOFINCIDENT"].ToString();
        }
    }
    protected void MenuIncidentInjuryGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                string PartofBodyInjured = GetCsvValue(ddlPartofTheBodyInjured);

                if (IsValidInspectionIncidentInjury(PartofBodyInjured))
                {
                    string personid = "";
                    if (ViewState["VESSELID"].ToString() == "0")
                        personid = txtPersonOfficeId.Text;
                    else
                        personid = txtCrewId.Text;

                    

                    if (ViewState["NEW"].ToString() == "true")
                    {                     

                        PhoenixInspectionIncidentInjury.InsertIncidentInjury(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , null
                        , new Guid(Filter.CurrentIncidentID)
                        , General.GetNullableInteger(personid)
                        , int.Parse(ucTypeOfInjury.SelectedQuick)
                        , General.GetNullableInteger(null)
                        , General.GetNullableString(PartofBodyInjured)
                        , null
                        , General.GetNullableDecimal(ucManHoursLost.Text)
                        , null
                        , General.GetNullableDecimal(ucExtimatedCost.Text)
                        , General.GetNullableInteger(chkThirdPartyInjury.Checked.Equals(true) ? "1" : "0")
                        , General.GetNullableString(txtThirdPartyName.Text)
                        , General.GetNullableInteger(txtThirdPartyAge.Text)
                        , General.GetNullableString(txtDescription.Text)
                        , General.GetNullableString(txtRemarks.Text)
                        , int.Parse(ucHealthSafetyCategory.SelectedHazardType)
                        , new Guid(ddlHealthSafetySubCategory.SelectedValue)
                        , General.GetNullableString(txtThirdPartyDesignation.Text)
                        , General.GetNullableGuid(ddlWorkInjuryCategory.SelectedValue)
                        );

                        ucStatus.Text = "Injury details are added.";
                        Rebind();
                        Filter.CurrentIncidentTab = "CONSEQUENCE";
                        ViewState["NEW"] = "false";
                    }

                    else
                    {
                        PhoenixInspectionIncidentInjury.UpdateIncidentInjury(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , new Guid(ViewState["INJURYID"].ToString())
                            , new Guid(ViewState["INSPECTIONINCIDENTID"].ToString())
                            , General.GetNullableInteger(personid)
                            , int.Parse(ucTypeOfInjury.SelectedQuick)
                            , General.GetNullableInteger(null)
                            , General.GetNullableString(PartofBodyInjured)
                            , null
                            , General.GetNullableDecimal(ucManHoursLost.Text)
                            , null
                            , General.GetNullableDecimal(ucExtimatedCost.Text)
                            , General.GetNullableInteger(chkThirdPartyInjury.Checked.Equals(true) ? "1" : "0")
                            , General.GetNullableString(txtThirdPartyName.Text)
                            , General.GetNullableInteger(txtThirdPartyAge.Text)
                            , General.GetNullableString(txtDescription.Text)
                            , General.GetNullableString(txtRemarks.Text)
                            , int.Parse(ucHealthSafetyCategory.SelectedHazardType)
                            , new Guid(ddlHealthSafetySubCategory.SelectedValue)
                            , General.GetNullableString(txtThirdPartyDesignation.Text)
                            , General.GetNullableGuid(ddlWorkInjuryCategory.SelectedValue)
                            );
                        ucStatus.Text = "Injury details are updated.";
                        Rebind();
                        Filter.CurrentIncidentTab = "CONSEQUENCE";
                        ViewState["NEW"] = "false";
                    }
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            else if (CommandName.ToUpper().Equals("NEW"))
            {
                ViewState["INJURYID"] = null;
                ViewState["NEW"] = "true";
                Reset();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void EditIncidentInjury()
    {
        DataSet ds1 = PhoenixInspectionIncident.EditInspectionIncident(new Guid(Filter.CurrentIncidentID));
        if (ds1.Tables[0].Rows.Count > 0)
        {
            DataRow dr1 = ds1.Tables[0].Rows[0];
            ViewState["VESSELID"] = dr1["FLDVESSELID"].ToString();
            Reset();
        }
        if (ViewState["INJURYID"] != null)
        {
            DataSet ds = PhoenixInspectionIncidentInjury.EditIncidentInjury(new Guid(ViewState["INJURYID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                txtCrewId.Text = dr["FLDINJUREDEMPLOYEEID"].ToString();
                txtAge.Text = dr["FLDAGE"].ToString();
                txtServiceYears.Text = dr["FLDSERVICEYEARS"].ToString();
                ucManHoursLost.Text = dr["FLDMANHOURSLOST"].ToString();
                SetCsvValue(ddlPartofTheBodyInjured, dr["FLDMULTIPARTOFTHEBODYINJURED"].ToString());
                ucTypeOfInjury.SelectedQuick = dr["FLDTYPEOFINJURY"].ToString();
                ViewState["INSPECTIONINCIDENTID"] = dr["FLDINSPECTIONINCIDENTID"].ToString();
                ViewState["INCIDENTVESSELID"] = dr["FLDVESSELID"].ToString();

                if (ViewState["VESSELID"].ToString() != "0")
                {
                    txtCrewName.Text = dr["FLDCREWNAME"].ToString();
                    txtCrewRank.Text = dr["FLDCREWRANK"].ToString();
                }
                else
                {
                    txtPersonOfficeId.Text = dr["FLDINJUREDEMPLOYEEID"].ToString();
                    txtOfficePersonName.Text = dr["FLDCREWNAME"].ToString();
                    txtOfficePersonDesignation.Text = dr["FLDCREWRANK"].ToString();
                }

                lblDtkey.Text = dr["FLDDTKEY"].ToString();
                string typeofworkinjury = dr["FLDCATEGORYOFWORKINJURYSHORTNAME"].ToString();
                ucExtimatedCost.Text = dr["FLDESTIMATEDCOST"].ToString();
                txtServiceYearsAtSea.Text = dr["FLDSERVICEYEARSATSEA"].ToString();                
                ucHealthSafetyCategory.SelectedHazardType = dr["FLDHEALTHAFETYCATEGORY"].ToString();
                BindHealthSafetySubCategory();
                ddlHealthSafetySubCategory.SelectedValue = dr["FLDHEALTHSAFETYSUBCATEGORY"].ToString();
                txtCategory.Text = dr["FLDCATEGORY"].ToString();
                txtThirdPartyDesignation.Text = dr["FLDTHIRDPARTYDESIGNATION"].ToString();

                if (dr["FLDISTHIRDPARTYINJURY"].ToString() == "1")
                {
                    chkThirdPartyInjury.Checked = true;
                    txtCrewName.Text = "";
                    txtCrewRank.Text = "";
                    txtCrewId.Text = "";
                    spnCrewInCharge.Visible = false;
                    spnPersonInChargeOffice.Visible = false;
                    txtAge.Visible = false;

                    spnThirdParty.Visible = true;
                    txtThirdPartyAge.Visible = true;
                }
                else
                {
                    chkThirdPartyInjury.Checked = false;
                    txtThirdPartyName.Text = "";
                    txtThirdPartyDesignation.Text = "";
                    txtThirdPartyAge.Text = "";
                    spnThirdParty.Visible = false;
                    txtThirdPartyAge.Visible = false;
                    if (ViewState["VESSELID"].ToString() == "0")
                    {
                        spnCrewInCharge.Visible = false;
                        spnPersonInChargeOffice.Visible = true;
                    }
                    else
                    {
                        spnCrewInCharge.Visible = true;
                        spnPersonInChargeOffice.Visible = false;
                    }
                    txtAge.Visible = true;
                }
                txtThirdPartyName.Text = dr["FLDTHIRDPARTYNAME"].ToString();
                txtThirdPartyAge.Text = dr["FLDTHIRDPARTYAGE"].ToString();
                txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
                txtRemarks.Text = dr["FLDREMARKS"].ToString();
                if (dr["FLDWORKINJURYCATEGORYID"] != null && dr["FLDWORKINJURYCATEGORYID"].ToString() != string.Empty)
                    ddlWorkInjuryCategory.SelectedValue = dr["FLDWORKINJURYCATEGORYID"].ToString();
            }
        }
        else
        {
            Reset();
        }

    }
    private bool IsValidInspectionIncidentInjury(string PartofBodyInjured)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (chkThirdPartyInjury.Checked == false)
        {
            if (string.IsNullOrEmpty(txtCrewId.Text.Trim()) && string.IsNullOrEmpty(txtPersonOfficeId.Text.Trim()))
                ucError.ErrorMessage = "Injured's Name is Required.";
        }
        else if (chkThirdPartyInjury.Checked == true)
        {
            if (General.GetNullableString(txtThirdPartyName.Text) == null)
                ucError.ErrorMessage = " Injured's Name is required.";
            if (General.GetNullableString(txtThirdPartyDesignation.Text) == null)
                ucError.ErrorMessage = "Designation is required.";
            if (General.GetNullableInteger(txtThirdPartyAge.Text) == null)
                ucError.ErrorMessage = "Age is required.";
        }

        if (PartofBodyInjured == string.Empty)
            ucError.ErrorMessage = " Parts of Body Injured is required.";

        if (string.IsNullOrEmpty(ucTypeOfInjury.SelectedQuick) || ucTypeOfInjury.SelectedQuick.ToUpper().ToString() == "DUMMY")
            ucError.ErrorMessage = " Injury type is required.";

        if (General.GetNullableGuid(ddlWorkInjuryCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Category of work injury is required.";

        if (General.GetNullableInteger(ucHealthSafetyCategory.SelectedHazardType) == null)
            ucError.ErrorMessage = "Health and Safety Category is required.";

        if (General.GetNullableGuid(ddlHealthSafetySubCategory.SelectedValue) == null)
            ucError.ErrorMessage = "Health and Safety Subcategory is required.";

        return (!ucError.IsError);
    }
    private void Reset()
    {
        txtCrewId.Text = txtCrewName.Text = txtAge.Text = txtServiceYears.Text = txtServiceYearsAtSea.Text = "";
        txtOfficePersonName.Text = txtOfficePersonDesignation.Text = txtPersonOfficeId.Text = txtAge.Text = txtServiceYears.Text = txtServiceYearsAtSea.Text = txtPersonOfficeEmail.Text = "";

        ddlPartofTheBodyInjured.ClearCheckedItems();
        ucTypeOfInjury.SelectedQuick = "";
        ucManHoursLost.Text = "";
        txtCrewRank.Text = "";
        ucExtimatedCost.Text = "";
        txtThirdPartyAge.Text = "";
        txtThirdPartyName.Text = "";
        txtDescription.Text = "";
        txtRemarks.Text = "";
        lnkSicknessReport.Visible = false;
        spnCrewInCharge.Visible = true;
        txtAge.Visible = true;
        spnThirdParty.Visible = false;
        txtThirdPartyAge.Visible = false;
        txtCategory.Text = "";
        ucHealthSafetyCategory.SelectedHazardType = "";
        ddlHealthSafetySubCategory.SelectedIndex = 0;
        txtThirdPartyDesignation.Text = "";
        chkThirdPartyInjury.Checked = false;
        ddlWorkInjuryCategory.SelectedIndex = 0;

        if (ViewState["VESSELID"].ToString() == "0")
        {
            spnCrewInCharge.Visible = false;
            spnPersonInChargeOffice.Visible = true;
        }
        else
        {
            spnCrewInCharge.Visible = true;
            spnPersonInChargeOffice.Visible = false;
        }
    }
    protected void ThirdParty_Changed(object sender, EventArgs e)
    {
        if (chkThirdPartyInjury.Checked == true)
        {
            chkThirdPartyInjury.Checked = true;

            txtCrewName.Text = "";
            txtCrewRank.Text = "";
            txtCrewId.Text = "";
            txtServiceYears.Text = "";
            txtServiceYearsAtSea.Text = "";
            txtAge.Text = "";
            spnCrewInCharge.Visible = false;
            spnPersonInChargeOffice.Visible = false;
            txtAge.Visible = false;

            spnThirdParty.Visible = true;
            txtThirdPartyAge.Visible = true;
            txtThirdPartyName.Text = "";
            txtThirdPartyAge.Text = "";
            txtThirdPartyDesignation.Text = "";
        }
        else
        {
            txtThirdPartyName.Text = "";
            txtThirdPartyAge.Text = "";
            txtThirdPartyDesignation.Text = "";
            spnThirdParty.Visible = false;
            txtThirdPartyAge.Visible = false;

            if (ViewState["VESSELID"].ToString() == "0")
                spnPersonInChargeOffice.Visible = true;
            else
                spnCrewInCharge.Visible = true;

            txtAge.Visible = true;
            txtCrewName.Text = "";
            txtCrewRank.Text = "";
            txtCrewId.Text = "";
            txtServiceYears.Text = "";
            txtServiceYearsAtSea.Text = "";
            txtAge.Text = "";
        }
    }

    protected void BindWorkInjuryCategory()
    {
        ddlWorkInjuryCategory.DataSource = PhoenixInspectionRiskAssessmentWorkInjuryCategory.ListRiskAssessmentWorkInjuryCategory();
        ddlWorkInjuryCategory.DataTextField = "FLDNAME";
        ddlWorkInjuryCategory.DataValueField = "FLDWORKINJURYCATEGORYID";
        ddlWorkInjuryCategory.DataBind();
        ddlWorkInjuryCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void BindInjured()
    {
        ddlPartofTheBodyInjured.DataSource = PhoenixRegistersQuick.ListQuick(1,68);
        ddlPartofTheBodyInjured.DataTextField = "FLDQUICKNAME";
        ddlPartofTheBodyInjured.DataValueField ="FLDQUICKCODE";
        ddlPartofTheBodyInjured.DataBind();
    }
    private string GetCsvValue(RadComboBox radComboBox)
    {
        var list = radComboBox.CheckedItems;
        string csv = string.Empty;
        if (list.Count != 0)
        {
            csv = ",";
            foreach (var item in list)
            {
                csv = csv + item.Value + ",";
            }
        }
        return csv;
    }
    private void SetCsvValue(RadComboBox radComboBox, string csvValue)
    {
        foreach (RadComboBoxItem item in radComboBox.Items)
        {
            item.Checked = false;
            if (csvValue.Contains("," + item.Value + ","))
            {
                item.Checked = true;
            }
        }
    }
}
