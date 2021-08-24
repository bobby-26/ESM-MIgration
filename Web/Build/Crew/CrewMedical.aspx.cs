using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewMedical : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {			
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            ViewState["MPAGENUMBER"] = 1;
            ViewState["MSORTEXPRESSION"] = null;
            ViewState["MSORTDIRECTION"] = null;
            ViewState["MCURRENTINDEX"] = 1;

            ViewState["MEDICALID"] = string.Empty;
            cblSuffered.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, (int)PhoenixHardTypeCode.ILLNESS);
            cblSuffered.DataTextField = "FLDHARDNAME";
            cblSuffered.DataValueField = "FLDHARDCODE";
            cblSuffered.DataBind();           
            SetEmployeePrimaryDetails();
            SetMedicalDetail();

            gvMedical.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvDeficiency.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Attachment", "ATTACHMENT", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        //toolbarmain.AddButton("Save", "SAVE",ToolBarDirection.Right);
        //toolbarmain.AddImageLink("javascript:parent.Openpopup('NAFA','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
        //               + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=MEDICALHISUPLOAD'); return false;", "Attachment", "", "ATTACHMENT");
        CrewMedical1.AccessRights = this.ViewState;
		CrewMedical1.MenuList = toolbarmain.Show();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewMedical.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMedical')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuCrewMedical.AccessRights = this.ViewState;
        MenuCrewMedical.MenuList = toolbar.Show();

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Crew/CrewMedical.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDeficiency')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuMedicalCase.AccessRights = this.ViewState;
        MenuMedicalCase.MenuList = toolbargrid.Show();

         
        SetAttachmentMarking();
    }

    private void SetMedicalDetail()
    {
        DataTable dt = PhoenixCrewMedical.CrewMedicalList(General.GetNullableInteger(Filter.CurrentCrewSelection));
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
        if (CommandName.ToUpper().Equals("ATTACHMENT"))
        {

            String script = String.Format("javascript:openNewWindow('spnPickListVendor', 'codehelp1', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                       + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=MEDICALHISUPLOAD');");

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);

        }
    }

    protected void CrewMedical_TabStripCommand(object sender, EventArgs e)
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

        string[] alColumns = { "FLDDATEOFOCCURRENCE", "FLDVESSELNAME", "FLDDESCRIPTION", "FLDTYPEOFMEDICALCASENAME" };
        string[] alCaptions = { "Date of Incident", "Vessel", "Description", "Type" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixCrewMedical.SearchCrewMedicalHistory(Convert.ToInt32(Filter.CurrentCrewSelection), sortexpression, sortdirection,
                                                                                      (int)ViewState["PAGENUMBER"],
                                                                                      iRowCount,
                                                                                      ref iRowCount,
                                                                                      ref iTotalPageCount);

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewMedicalHistory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Medical History</h3></td>");
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
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
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
                                                    , General.GetNullableInteger(Filter.CurrentCrewSelection).Value
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

        ImageButton imgbt = (ImageButton)footerItem.FindControl("cmdAdd");
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

            string[] alColumns = { "FLDDATEOFOCCURRENCE", "FLDVESSELNAME", "FLDDESCRIPTION", "FLDTYPEOFMEDICALCASENAME" };
            string[] alCaptions = { "Date of Incident", "Vessel", "Description", "Type" };
                    
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

           
            DataSet ds = PhoenixCrewMedical.SearchCrewMedicalHistory(Convert.ToInt32(Filter.CurrentCrewSelection), sortexpression, sortdirection,
                                                                                      (int)ViewState["PAGENUMBER"],
                                                                                      gvMedical.PageSize,
                                                                                      ref iRowCount,
                                                                                      ref iTotalPageCount);

            General.SetPrintOptions("gvMedical", "CrewMedicalHistory", alCaptions, alColumns, ds);

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

    
   
    private bool IsMedicalHistoryValid(string strVesselName, string strDateOccurrence, string strDescription, string strType)
    {
        int resultInt;
        ucError.HeaderMessage = "Please provide the following required information";        
        DateTime resultdate;

        if (strDateOccurrence == null || !DateTime.TryParse(strDateOccurrence, out  resultdate))
            ucError.ErrorMessage = "Date of incident is required";
        else if (DateTime.TryParse(strDateOccurrence, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Incident Date should be earlier than current date";
        }

        if (strVesselName.Trim() == "")
            ucError.ErrorMessage = "Vessel is required";

        if (strDescription.Trim() == "")
            ucError.ErrorMessage = "Description is required";

        if (!int.TryParse(strType, out resultInt))
            ucError.ErrorMessage = "Type is required";
     
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
    protected void MenuMedicalCase_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowMedicalCaseExcel();
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowMedicalCaseExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDDATEOFILLNESS", "FLDVESSELNAME", "FLDILLNESSDESCRIPTION", "FLDTYPEOFMEDICALCASENAME", "FLDCASEOPENEDBY", "FLDREFERENCENO" };
            string[] alCaptions = { "Illness Date", "Vessel", "Description", "Type", "User", "Reference Number" };
           
            if (ViewState["MROWCOUNT"] == null || Int32.Parse(ViewState["MROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["MROWCOUNT"].ToString());

            DataTable dt = PhoenixCrewMedical.SearchCrewMedicalCaseHistory(General.GetNullableInteger(Filter.CurrentCrewSelection),
                   1,
                   iRowCount,
                   ref iRowCount,
                   ref iTotalPageCount
                   );

            General.ShowExcel("Medical Case", dt, alColumns, alCaptions, null, string.Empty);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindMedicalCaseData()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDREFERENCENO", "FLDVESSELNAME", "FLDRANKNAME", "FLDDATEOFILLNESS", "FLDDOCTORVISITDATE" };
            string[] alCaptions = { "Reference Number", "Vessel", "Rank", "Illness/Injury DATE", "Doctor Visit Date" };
           
            DataTable dt = PhoenixCrewMedical.SearchCrewMedicalCaseHistory(General.GetNullableInteger(Filter.CurrentCrewSelection),               
                  Convert.ToInt16(ViewState["MPAGENUMBER"].ToString()),
                  gvDeficiency.PageSize,
                  ref iRowCount,
                  ref iTotalPageCount
                  );

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvDeficiency", "Medical Case", alCaptions, alColumns, ds);

            gvDeficiency.DataSource = ds;
            gvDeficiency.VirtualItemCount = iRowCount;

         
            ViewState["MROWCOUNT"] = iRowCount;
            ViewState["MTOTALPAGECOUNT"] = iTotalPageCount;
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
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

    protected void gvDeficiency_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["MPAGENUMBER"] = ViewState["MPAGENUMBER"] != null ? ViewState["MPAGENUMBER"] : gvMedical.CurrentPageIndex + 1;
            BindMedicalCaseData();
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

                if (!IsMedicalHistoryValid(vesselname, occurrencedate, description, Type))
                {
                    ucError.Visible = true;
                  
                    return;
                }
                if (string.IsNullOrEmpty(ViewState["MEDICALID"].ToString()))
                {
                    UpdateEmployeeMedicalAdditional();
                }
                PhoenixCrewMedical.InsertCrewMedicalHistory(Convert.ToInt32(Filter.CurrentCrewSelection)
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
        if(e.CommandName.ToUpper()=="UPDATE")
        {
            try
            {
                string vesselname = ((RadTextBox)e.Item.FindControl("txtVesselNameEdit")).Text;
                string occurrencedate = ((UserControlDate)e.Item.FindControl("txtOccurrenceDateEdit")).Text;
                string description = ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text;
                string medicalhistoryid = ((RadLabel)e.Item.FindControl("lblMedicalHistoryIdEdit")).Text;
                //string Type = ((DropDownList)e.Item.FindControl("ddlTypeofcaseEdit")).SelectedValue;
                string Type = ((UserControlHard)e.Item.FindControl("ucTypeofcaseEdit")).SelectedHard;

                if (!IsMedicalHistoryValid(vesselname, occurrencedate, description, Type))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixCrewMedical.UpdateCrewMedicalHistory(Convert.ToInt32(Filter.CurrentCrewSelection), Convert.ToInt32(medicalhistoryid.ToString())
                                                                , vesselname
                                                                    , Convert.ToDateTime(occurrencedate), description, Convert.ToInt32(Type));
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
         

            BindData();
            gvMedical.Rebind();
        }
        if(e.CommandName.ToUpper()=="DELETE")
        {
            try
            {
            
                string medicalhistoryid = ((RadLabel)e.Item.FindControl("lblMedicalHistoryId")).Text;

                PhoenixCrewMedical.DeleteCrewMedicalHistory(Convert.ToInt32(Filter.CurrentCrewSelection), Convert.ToInt32(medicalhistoryid.ToString()));

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;

            }
            
            BindData();
            gvMedical.Rebind();
        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvMedical_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if(db!=null)db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                if (db != null) if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;

                LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
                if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;
            }
            else
            {
                e.Item.Attributes["onclick"] = "";
            }
        }

        if (e.Item is GridDataItem)
        {
           
                UserControlHard ucHard = (UserControlHard)e.Item.FindControl("ucTypeofcaseEdit");
                DataRowView drv = (DataRowView)e.Item.DataItem;
                if (ucHard != null) ucHard.SelectedHard = drv["FLDTYPEOFMEDICALCASE"].ToString();
            
        }

        if (e.Item is GridFooterItem)
        {
            LinkButton imgbt = (LinkButton)e.Item.FindControl("cmdAdd");
            if (!SessionUtil.CanAccess(this.ViewState, imgbt.CommandName)) imgbt.Visible = false;
        }
    }

    protected void gvDeficiency_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }

    protected void gvDeficiency_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        try
        {

            if (e.Item is GridDataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;


                LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAttachment");
                if (cmdAttachment != null)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                  
                    if (drv["FLDATTACHMENTCOUNT"].ToString() == "0")
                    {
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                        cmdAttachment.Controls.Add(html);
                    }
                        
               
                    cmdAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                                        + PhoenixModule.QUALITY + "&u=n');return true;");
                    cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
                }

                if (e.Item is GridDataItem)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                   
                    RadLabel lbl = (RadLabel)e.Item.FindControl("lblPNIMedicalCaseId");
                    LinkButton img = (LinkButton)e.Item.FindControl("imgRemarks");
                    UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
                    img.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    img.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                    img.Attributes.Add("onclick", "parent.openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=MEDICALCASE','xlarge')");
                    RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
                    if (string.IsNullOrEmpty(lblR.Text.Trim()))
                    {
                        uct.Visible = false;                     
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-glasses-nr\"></i></span>";
                        img.Controls.Add(html);
                    }
                }
                if (e.Item is GridDataItem)
                {

                    string lblPNIMedicalCaseId = ((RadLabel)e.Item.FindControl("lblPNIMedicalCaseId")).Text;
                    LinkButton lnkRefNumber = (LinkButton)e.Item.FindControl("lnkRefNumber");

                    if (lnkRefNumber != null)
                    {
                        lnkRefNumber.Attributes.Add("onclick", "javascript:openNewWindow('source','','"+Session["sitepath"]+"/Inspection/InspectionPNIOperation.aspx?PNIID=" + lblPNIMedicalCaseId + "&viewonly=1'); return true;");
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
}
