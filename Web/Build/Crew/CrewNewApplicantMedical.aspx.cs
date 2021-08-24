using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class CrewNewApplicantMedical : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvMedical.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl00");
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$ctl01");
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            AutoArchive();
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;
            ViewState["MEDICALID"] = string.Empty;
            cblSuffered.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)PhoenixHardTypeCode.ILLNESS);
            cblSuffered.DataTextField = "FLDHARDNAME";
            cblSuffered.DataValueField = "FLDHARDCODE";
            cblSuffered.DataBind();
            SetEmployeePrimaryDetails();
            SetMedicalDetail();
        }
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Attachment", "ATTACHMENT", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
       
        //toolbarmain.AddImageLink("javascript:openNewWindow('NAFA','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
        //               + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=NMEDICALUPLOAD'); return false;", "Attachment", "", "ATTACHMENT");
        CrewMedical1.AccessRights = this.ViewState;
        CrewMedical1.MenuList = toolbarmain.Show();

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddFontAwesomeButton("../Crew/CrewNewApplicantMedical.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbarsub.AddFontAwesomeButton("javascript:CallPrint('gvMedical')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        CrewMedicalHistory.AccessRights = this.ViewState;
        CrewMedicalHistory.MenuList = toolbarsub.Show();

        //BindData();
        //SetPageNavigator();
        SetAttachmentMarking();
    }
    private void AutoArchive()
    {
        PhoenixCrewMedicalDocuments.AutoArchiveCewMedical(Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()));
    }
    private void SetMedicalDetail()
    {
        DataTable dt = PhoenixCrewMedical.CrewMedicalList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
        if (dt.Rows.Count > 0)
        {
            ViewState["MEDICALID"] = "1";
            if (dt.Rows[0]["FLDSIGNOFFFROMSHIP"].ToString() != string.Empty)
                rblSignedOffReason.Items.FindByValue(dt.Rows[0]["FLDSIGNOFFFROMSHIP"].ToString()).Selected = true;
            if (dt.Rows[0]["FLDDISEASE"].ToString() != string.Empty)
                rblDiseaseSuffered.Items.FindByValue(dt.Rows[0]["FLDDISEASE"].ToString()).Selected = true;
            if (dt.Rows[0]["FLDDRUGADDICT"].ToString() != string.Empty)
                rblDrugAddict.Items.FindByValue(dt.Rows[0]["FLDDRUGADDICT"].ToString()).Selected = true;
            if (dt.Rows[0]["FLDPSYCHIATRIC"].ToString() != string.Empty)
                rblPsychiatric.Items.FindByValue(dt.Rows[0]["FLDPSYCHIATRIC"].ToString()).Selected = true;
            string[] csvDiseaseSuffered = dt.Rows[0]["FLDSUFFERED"].ToString().Split(',');
            foreach (string str in csvDiseaseSuffered)
            {
                if (str == string.Empty) continue;
                cblSuffered.Items.FindByValue(str).Selected = true;
            }
        }
        else
        {
            ViewState["MEDICALID"] = string.Empty;
            //gvMedical.Visible = false;
            //divPage.Visible = false;
        }
    }

    protected void CrewMedical1_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {

            UpdateEmployeeMedicalAdditional();
            SetMedicalDetail();
        }
        if(CommandName.ToUpper().Equals("ATTACHMENT"))
        {

            String script = String.Format("javascript:openNewWindow('spnPickListVendor', 'codehelp1', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                       + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=NMEDICALUPLOAD');");

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

        }
    }

    protected void CrewMedicalHistory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDDATEOFOCCURRENCE", "FLDDESCRIPTION" };
        string[] alCaptions = { "Vessel Name", "Date of Occurrence", "Description" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixCrewMedical.SearchCrewMedicalHistory(Convert.ToInt32(Filter.CurrentNewApplicantSelection), sortexpression, sortdirection,
                                                                                       (int)ViewState["PAGENUMBER"],
                                                                                       iRowCount,
                                                                                       ref iRowCount,
                                                                                       ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=NewApplicantMedicalHistory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>New Applicant - Medical History</h3></td>");
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
                Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidate()
    {

        ucError.HeaderMessage = "Please provide the following required information";
        if (rblSignedOffReason.SelectedValue == "1")
        {
            //if (txtVesselName.Text.Trim() == string.Empty)
            //    ucError.ErrorMessage = "Vessel Names is required";
            //if (string.IsNullOrEmpty(txtDateofOccurrence.Text))
            //    ucError.ErrorMessage = "Date of Occurrence is required"; 
            //if(txtDescription.Text.Trim() == string.Empty)
            //    ucError.ErrorMessage = "Brief Description is required"; 
        }
        return (!ucError.IsError);
    }

    protected void UpdateEmployeeMedicalAdditional()
    {
        try
        {
            if (IsValidate())
            {
                string csvSufferedDisease = string.Empty;
                foreach (ListItem item in cblSuffered.Items)
                {
                    if (item.Selected) csvSufferedDisease += item.Value + ",";
                }

                PhoenixCrewMedical.UpdateCrewMedical(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                    , General.GetNullableInteger(Filter.CurrentNewApplicantSelection).Value
                                                    , (byte?)General.GetNullableInteger(rblSignedOffReason.SelectedValue)
                                                    , (byte?)General.GetNullableInteger(rblDiseaseSuffered.SelectedValue)
                                                    , (byte?)General.GetNullableInteger(rblDrugAddict.SelectedValue)
                                                    , csvSufferedDisease.TrimEnd(',')
                                                    , (byte?)General.GetNullableInteger(rblPsychiatric.SelectedValue)
                                                    );
                ucStatus.Text = "Medical information updated.";
            }
            else
            {
                ucError.Visible = true;
                return;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void rblSignedOffReason_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadioButtonList rbl = (RadioButtonList)sender;
        GridFooterItem footerItem = (GridFooterItem)gvMedical.MasterTableView.GetItems(GridItemType.Footer)[0];
        // Button btn = (Button)footerItem.FindControl("Button1");
        RadLabel TotalFOC = (RadLabel)footerItem.FindControl("lblTotalFOC");

        LinkButton imgbt = (LinkButton)footerItem.FindControl("cmdAdd");
        if (rbl.SelectedValue == "1")
        {
            imgbt.Visible = true;
        }
        else
        {
            imgbt.Visible = false;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSELNAME", "FLDDATEOFOCCURRENCE", "FLDDESCRIPTION" };
            string[] alCaptions = { "Vessel Name", "Date of Occurrence", "Description" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


            DataSet ds = PhoenixCrewMedical.SearchCrewMedicalHistory(Convert.ToInt32(Filter.CurrentNewApplicantSelection), sortexpression, sortdirection,
                                                                                      (int)ViewState["PAGENUMBER"],
                                                                                      gvMedical.PageSize,
                                                                                      ref iRowCount,
                                                                                      ref iTotalPageCount);

            General.SetPrintOptions("gvMedical", "New Applicant - Medical History", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                rblSignedOffReason.Enabled = false;
                rblSignedOffReason.SelectedIndex = 0;
                gvMedical.DataSource = ds;

            }
            else
            {
                rblSignedOffReason.Enabled = true;
                gvMedical.DataSource = ds;
            }
            gvMedical.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    private bool IsMedicalHistoryValid(string strVesselName, string strDateOccurrence, string strDescription)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;

        if (strVesselName.Trim() == "")
            ucError.ErrorMessage = "Vessel Name is required";

        if (strDateOccurrence == null || !DateTime.TryParse(strDateOccurrence, out resultdate))
            ucError.ErrorMessage = "Occurrence Date is required";
        else if (DateTime.TryParse(strDateOccurrence, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Occurrence Date should be earlier than current date";
        }
        if (strDescription.Trim() == "")
            ucError.ErrorMessage = "Description is required";

        return (!ucError.IsError);

    }



    private void SetAttachmentMarking()
    {
        DataTable dt1 = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["attachmentcode"].ToString()), PhoenixCrewAttachmentType.MEDICAL.ToString());
        if (dt1.Rows.Count > 0)
            imgClip.Visible = true;
        else
            imgClip.Visible = false;
    }

    protected void gvMedical_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMedical.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvMedical_ItemCommand(object sender, GridCommandEventArgs e)
    {

        if (e.CommandName.ToString().ToUpper() == "ADD")
        {

            try
            {
                string vesselname = ((RadTextBox)e.Item.FindControl("txtVesselNameAdd")).Text;
                string occurrencedate = ((UserControlDate)e.Item.FindControl("txtOccurrenceDateAdd")).Text;
                string description = ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text;
                string Type = ((UserControlHard)e.Item.FindControl("ucTypeofcaseAdd")).SelectedHard;

                if (!IsMedicalHistoryValid(vesselname, occurrencedate, description))
                {
                    ucError.Visible = true;

                    return;
                }
                if (string.IsNullOrEmpty(ViewState["MEDICALID"].ToString()))
                {
                    UpdateEmployeeMedicalAdditional();
                }
                PhoenixCrewMedical.InsertCrewMedicalHistory(Convert.ToInt32(Filter.CurrentNewApplicantSelection)
                                                                , vesselname
                                                                , Convert.ToDateTime(occurrencedate), description
                                                                 , Convert.ToInt32(Type));
                BindData();
                gvMedical.Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;

            }
        }
        if (e.CommandName.ToUpper() == "DELETE")
        {

            try
            {

                string medicalhistoryid = ((RadLabel)e.Item.FindControl("lblMedicalHistoryId")).Text;

                PhoenixCrewMedical.DeleteCrewMedicalHistory(Convert.ToInt32(Filter.CurrentNewApplicantSelection), Convert.ToInt32(medicalhistoryid.ToString()));
                BindData();
                gvMedical.Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }

           
        }
        if (e.CommandName.ToUpper() == "UPDATE")
        {

            try
            {
                string vesselname = ((RadTextBox)e.Item.FindControl("txtVesselNameEdit")).Text;
                string occurrencedate = ((UserControlDate)e.Item.FindControl("txtOccurrenceDateEdit")).Text;
                string description = ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text;
                string medicalhistoryid = ((RadLabel)e.Item.FindControl("lblMedicalHistoryIdEdit")).Text;
                string Type = ((UserControlHard)e.Item.FindControl("ucTypeofcaseEdit")).SelectedHard;
                if (!IsMedicalHistoryValid(vesselname, occurrencedate, description))
                {
                    ucError.Visible = true;
                   // BindData();
                    return;
                }

                PhoenixCrewMedical.UpdateCrewMedicalHistory(Convert.ToInt32(Filter.CurrentNewApplicantSelection), Convert.ToInt32(medicalhistoryid.ToString())
                                                             ,vesselname
                                                             , Convert.ToDateTime(occurrencedate), description
                                                             , Convert.ToInt32(Type));
                BindData();
                gvMedical.Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }


            BindData();
            gvMedical.Rebind();
        }

    }

  
    protected void gvMedical_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
                if (ed != null) ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

                UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ucTypeofcaseEdit");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (ucHard != null) ucHard.SelectedHard = drv["FLDTYPEOFMEDICALCASE"].ToString();
            }
            else
            {
                e.Item.Attributes["onclick"] = "";
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton imgbt = (LinkButton)e.Item.FindControl("cmdAdd");
            if (imgbt != null)
                imgbt.Visible = SessionUtil.CanAccess(this.ViewState, imgbt.CommandName);

            if (rblSignedOffReason.SelectedValue == "1")
            {
                imgbt.Visible = true;
            }
            else
            {
                imgbt.Visible = false;
            }
        }
    }
}
