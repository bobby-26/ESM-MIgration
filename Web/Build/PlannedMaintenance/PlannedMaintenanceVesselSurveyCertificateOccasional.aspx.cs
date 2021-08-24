using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Collections.Specialized;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class PlannedMaintenanceVesselSurveyCertificateOccasional : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuCertificatesRenewal.AccessRights = this.ViewState;
            MenuCertificatesRenewal.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                gvSurveyCOC.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                ViewState["PAGENUMBER"] = 1;
                ViewState["CSVCID"] = Request.QueryString["clist"];
                ViewState["EXT"] = PhoenixCommonRegisters.GetHardCode(1, 144, "EXT");
                ViewState["VSLID"] = Request.QueryString["vslid"] != null ? Request.QueryString["vslid"] : PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                if (string.IsNullOrEmpty(Request.QueryString["clist"]))
                    ViewState["SHOWSURVEYYN"] = 1;
                else
                    ViewState["SHOWSURVEYYN"] = 0;
                rdlAuditSurvey_SelectedIndexChanged(null, null);
                EditLastSurvey();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void EditLastSurvey()
    {
        DataTable dt = PhoenixPlannedMaintenanceSurveySchedule.ListVesselCertificateOccasional(int.Parse(ViewState["VSLID"].ToString())
                                           , ViewState["CSVCID"].ToString()
                                           , byte.Parse(rdlAuditSurvey.SelectedValue)
                                           , (byte?)General.GetNullableInteger(ViewState["SHOWSURVEYYN"].ToString())
                                           , 1);
        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];
            if (dr["FLDSURVEYID"].ToString() != string.Empty)
            {
                ViewState["SURVEYID"] = dr["FLDSURVEYID"].ToString();
                rdlAuditSurvey.SelectedValue = dr["FLDINSPECTIONID"].ToString().Equals("") ? "0" : "1";
                ddlAuditSurvey.SelectedValue = dr["FLDINSPECTIONID"].ToString().Equals("") ? dr["FLDSURVEYTYPEID"].ToString() : dr["FLDINSPECTIONID"].ToString();
                txtPlanDate.Text = dr["FLDPLANNEDDATE"].ToString();
                ddlSurveyPort.SelectedValue = dr["FLDPLANNEDPORT"].ToString();
                ddlSurveyPort.Text = dr["FLDPORTNAME"].ToString();
                txtSurveyorName.Text = dr["FLDSURVEYORNAME"].ToString();
                txtRemarks.Text = dr["FLDCERTIFICATEREMARKS"].ToString();

                rdlAuditSurvey.Enabled = false;
                ddlAuditSurvey.Enabled = false;
            }
        }
    }
    private void BindData()
    {
        DataTable dt = PhoenixPlannedMaintenanceSurveySchedule.ListVesselCertificateOccasional(int.Parse(ViewState["VSLID"].ToString())
                                            , ViewState["CSVCID"].ToString()
                                            , byte.Parse(rdlAuditSurvey.SelectedValue)
                                            , (byte?)General.GetNullableInteger(ViewState["SHOWSURVEYYN"].ToString())
                                            , 1);
        gvSurvey.DataSource = dt;

    }
    protected void gvSurvey_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {

            ImageButton cmdSvyAtt = (ImageButton)e.Item.FindControl("cmdSvyAtt");
            if (cmdSvyAtt != null)
            {
                if (drv["FLDDTKEY"].ToString() == string.Empty) cmdSvyAtt.Visible = false;
                if (drv["FLDATTACHMENTYN"].ToString() == "0") cmdSvyAtt.ImageUrl = Session["images"] + "/no-attachment.png";
                cmdSvyAtt.Attributes.Add("onclick", "javascript:openNewWindow('SUVATT','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.PLANNEDMAINTENANCE + "&type=SURVEYCERTIFICATE'); return false;");

                UserControlQuick quick = (UserControlQuick)e.Item.FindControl("ucRemarks");
                if (quick != null)
                    quick.SelectedQuick = drv["FLDREMARKS"].ToString();
            }

        }
    }
    private void BindDataCOC()  
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        try
        {
            DataSet ds = PhoenixPlannedMaintenanceVesselCertificateCOC.VesselCertificateCOCList(int.Parse(ViewState["VSLID"].ToString())
               , null
               , null
               , null
               , null
               , 1
               , gvSurveyCOC.CurrentPageIndex+1
               , gvSurveyCOC.PageSize
               , ref iRowCount
               , ref iTotalPageCount
               );

            gvSurveyCOC.DataSource = ds;
            gvSurveyCOC.VirtualItemCount = iRowCount;
            //gvSurveyCOC.SelectedIndex = SelectedIndex;

            ViewState["ROWCOUNT"] = iRowCount;
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuCertificatesRenewal_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SAVE"))
            {

                string cid = ",";
                string surveyno = ",";
                string issuedate = ",";
                string expirydate = ",";
                string stauts = ",";
                string dtkey = ",";
                foreach (GridDataItem row in gvSurvey.Items)
                {
                    dtkey += ((RadLabel)row.FindControl("lblDTKey")).Text + ",";
                    cid += ((RadLabel)row.FindControl("lblCertificateId")).Text + ",";
                    surveyno += ((RadTextBox)row.FindControl("txtNumber")).Text + ",";
                    string idate = ((UserControlDate)row.FindControl("txtIssueDate")).Text;
                    if (!string.IsNullOrEmpty(idate))
                        issuedate += string.Format("{0:dd/MMM/yyyy}", DateTime.Parse(idate)) + ",";
                    else
                        issuedate += idate + ",";

                    string edate = ((UserControlDate)row.FindControl("txtExpiryDate")).Text;
                    if (!string.IsNullOrEmpty(edate))
                        expirydate += string.Format("{0:dd/MMM/yyyy}", DateTime.Parse(edate)) + ",";
                    else
                        expirydate += edate + ",";
                    stauts += General.GetNullableInteger(((UserControlQuick)row.FindControl("ucRemarks")).SelectedQuick).ToString() + ",";
                }

                if (!IsValidDetails(cid))
                {
                    ucError.Visible = true;
                    return;
                }
                if (dtkey.TrimEnd(',').Equals(""))
                {
                    PhoenixPlannedMaintenanceSurveySchedule.InsertVesselCertificateOccasional(int.Parse(ViewState["VSLID"].ToString())
                   , cid
                   , surveyno
                   , DateTime.Parse(txtPlanDate.Text)
                   , issuedate
                   , expirydate
                   , stauts
                   , txtRemarks.Text
                   , rdlAuditSurvey.SelectedValue == "0" ? General.GetNullableInteger(ddlAuditSurvey.SelectedValue) : null
                   , rdlAuditSurvey.SelectedValue == "1" ? General.GetNullableGuid(ddlAuditSurvey.SelectedValue) : null
                   , General.GetNullableInteger(ddlSurveyPort.SelectedValue)
                   , General.GetNullableDateTime(txtPlanDate.Text)
                   , txtSurveyorName.Text
                   , null
                   , 1 // occasional
                   );
                }
                else
                {
                    PhoenixPlannedMaintenanceSurveySchedule.UpdateVesselCertificateOccasional(new Guid(ViewState["SURVEYID"].ToString())
                        , int.Parse(ViewState["VSLID"].ToString())
                        , cid
                        , surveyno
                        , DateTime.Parse(txtPlanDate.Text)
                        , issuedate
                        , expirydate
                        , stauts
                        , txtRemarks.Text
                        , General.GetNullableInteger(ddlSurveyPort.SelectedValue)
                        , General.GetNullableDateTime(txtPlanDate.Text)
                        , txtSurveyorName.Text
                        , null
                        , 1 // occasional
                        , dtkey
                        );
                }
                ViewState["SHOWSURVEYYN"] = 1;
                gvSurvey.Rebind();
                EditLastSurvey();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                             "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidDetails(string certificatelist)
    {
        //DateTime resultDate;
        //Int16 resultInt;

        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ddlAuditSurvey.SelectedValue) == null)
            ucError.ErrorMessage = "Type of Audit / Survey is required";

        if (!General.GetNullableDateTime(txtPlanDate.Text).HasValue)
            ucError.ErrorMessage = "Planned Date of Audit / Survey is required";
        if (certificatelist.TrimEnd(',').Equals(""))
        {
            ucError.ErrorMessage = "Certificate list is required";
        }
        return (!ucError.IsError);
    }

    protected void rdlAuditSurvey_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rbl = (RadioButtonList)sender;
        DataSet ds;
        if (rdlAuditSurvey.SelectedValue == "0")
        {
            ds = PhoenixRegistersVesselSurvey.SurveyTypeList(null, "OCC");
            ddlAuditSurvey.DataValueField = "FLDSURVEYTYPEID";
            ddlAuditSurvey.DataTextField = "FLDSURVEYTYPENAME";
            ddlAuditSurvey.DataSource = ds;
            ddlAuditSurvey.DataBind();
            ddlAuditSurvey.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        }
        else
        {
            ds = PhoenixInspection.ListInspection(null, General.GetNullableInteger(ViewState["EXT"].ToString()), null, "OCC");
            ddlAuditSurvey.DataValueField = "FLDINSPECTIONID";
            ddlAuditSurvey.DataTextField = "FLDINSPECTIONNAME";
            ddlAuditSurvey.DataSource = ds;
            ddlAuditSurvey.DataBind();
            ddlAuditSurvey.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        }
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlAuditSurvey.SelectedIndex = 1;
        }
        EditLastSurvey();
    }
    protected void gvSurveyCOC_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            RadLabel lblDescription = (RadLabel)e.Item.FindControl("lblDescription");
            RadLabel lblCOC = (RadLabel)e.Item.FindControl("lblCOC");
            RadLabel lblCompletedRemarks = (RadLabel)e.Item.FindControl("lblCompletedRemarks");


            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucCOCToolTip");
            if (lblCOC != null)
            {
                lblCOC.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lblCOC.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
            UserControlToolTip ucDescriptionToolTip = (UserControlToolTip)e.Item.FindControl("ucDescriptionToolTip");
            if (lblDescription != null)
            {
                lblDescription.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucDescriptionToolTip.ToolTip + "', 'visible');");
                lblDescription.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucDescriptionToolTip.ToolTip + "', 'hidden');");
            }
            UserControlToolTip ucCompletedRemarksToolTip = (UserControlToolTip)e.Item.FindControl("ucCompletedRemarksToolTip");
            if (lblDescription != null && ucCompletedRemarksToolTip != null)
            {
                lblCompletedRemarks.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucCompletedRemarksToolTip.ToolTip + "', 'visible');");
                lblCompletedRemarks.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucCompletedRemarksToolTip.ToolTip + "', 'hidden');");
            }
        }
    }

    private bool IsValidDetails(
       string CompleteRemarks
       , string CompletedDate)
    {
        DateTime resultDate;

        ucError.HeaderMessage = "Please provide the following required information";

        if (CompleteRemarks.Trim().Equals(""))
            ucError.ErrorMessage = "Completed Remarks is required.";

        if (!DateTime.TryParse(CompletedDate, out resultDate))
            ucError.ErrorMessage = "Completed Date is required.";

        if (CompletedDate != null && CompletedDate != null)
        {
            if ((DateTime.TryParse(CompletedDate, out resultDate)))
                if ((DateTime.Parse(CompletedDate)) > DateTime.Today)
                    ucError.ErrorMessage = "'Completed Date' should be earlier then current date.";
        }

        return (!ucError.IsError);
    }

    protected void gvSurveyCOC_ItemCommand1(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            //gvSurveyCOC.
        }
        if (e.CommandName.ToUpper().Equals("UPDATE"))
        {
            string cocid = ((RadLabel)e.Item.FindControl("lblCOCId")).Text;
            string completedremarks = ((RadTextBox)e.Item.FindControl("txtCompletionRemarks")).Text.Trim();
            string completeddate = ((UserControlDate)e.Item.FindControl("txtDoneDate")).Text;
            if (!IsValidDetails(completedremarks, completeddate))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixPlannedMaintenanceVesselCertificateCOC.CompleteSureyCOC(
                      new Guid(cocid)
                     , General.GetNullableString(completedremarks)
                     , General.GetNullableDateTime(completeddate));

            gvSurveyCOC.Rebind();
        }
    }

    protected void gvSurvey_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvSurveyCOC_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataCOC();
    }
}
