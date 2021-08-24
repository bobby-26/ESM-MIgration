using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;


public partial class InspectionPNIOperation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("List", "LIST");
        toolbar.AddButton("Medical Case", "MEDICAL");
        toolbar.AddButton("Check List", "CHECKLIST");
        toolbar.AddButton("Corres.", "CORRESPONDENCE");
        toolbar.AddButton("Remarks", "REMARKS");
        toolbar.AddButton("P&I Costing", "TRAVELCHECKLIST");
        MenuPNIGeneral.AccessRights = this.ViewState;
        MenuPNIGeneral.MenuList = toolbar.Show();

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        PNIListMain.AccessRights = this.ViewState;
        PNIListMain.MenuList = toolbar.Show();

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionPNIOperation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDeficiency')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuDeficiency.AccessRights = this.ViewState;
        MenuDeficiency.MenuList = toolbargrid.Show();

        MenuPNIGeneral.SelectedMenuIndex = 1;
        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            ViewState["PNIID"] = null;
            ViewState["REFNO"] = null;
            ViewState["empid"] = null;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            if (Request.QueryString["PNIID"] != null)
                ViewState["PNIID"] = Request.QueryString["PNIID"].ToString();

            cmdShowAgent.Attributes.Add("OnClick", "return showPickList('spnPickListAgent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=1255', true);");

            gvDeficiency.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

            if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != "")
            {

                EditOperation();
                BindData();
                // SetPageNavigator();

                imgPPClip.Attributes["onclick"] = "javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                  + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.INFORMEDPNI + "&cmdname=FINFORMEDPNIUPLOAD'); return false;";
            }
            else
            {
                divGrid.Visible = false;
                MenuDeficiency.Visible = false;

                imgPPClip.Visible = false;
            }

            txtCrewId.Attributes.Add("style", "display:none;");
            txtAgent.Attributes.Add("style", "display:none;");

            if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != "" && Request.QueryString["viewonly"] != null)
            {
                BindData();
                //SetPageNavigator();

                divgridview.Visible = false;
                divmenustrip.Visible = false;
            }

        }
        DateTime? date = General.GetNullableDateTime(ucInjuryDate.Text);

        imgShowCrewInCharge.Attributes.Add("onclick",
           "return showPickList('spnCrewInCharge', 'codehelp1', '','../Common/CommonPickListPNICrewOnboard.aspx?VesselId="
           + ucVessel.SelectedVessel + "&date=" + date + "', true); ");
       


    }
    protected void MenuPNIGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("CHECKLIST"))
        {
            if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != "")
            {
                Response.Redirect("../Inspection/InspectionPNIChecklist.aspx?PNIID=" + ViewState["PNIID"] + "&REFNO=" + ViewState["REFNO"], true);
            }
        }
        else if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../Inspection/InspectionPNIDoctorReportList.aspx", true);
        }
        else if (CommandName.ToUpper().Equals("CORRESPONDENCE"))
        {
            if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != "")
            {
                Response.Redirect("../Inspection/InspectionPersonalGeneral.aspx?empid=" + ViewState["empid"] + "&PNIID=" + ViewState["PNIID"] + "&REFNO=" + ViewState["REFNO"], true);
            }
        }
        else if (CommandName.ToUpper().Equals("REMARKS"))
        {
            if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != "")
            {
                Response.Redirect("../Inspection/InspectionPNIRemarks.aspx?empid=" + ViewState["empid"] + "&PNIID=" + ViewState["PNIID"] + "&REFNO=" + ViewState["REFNO"], true);
            }
        }
        else if (CommandName.ToUpper().Equals("TRAVELCHECKLIST"))
        {
            if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != "")
            {
                Response.Redirect("../Inspection/InspectionPNITravelDocumentCheckList.aspx?PNIID=" + ViewState["PNIID"] + "&REFNO=" + ViewState["REFNO"], true);
            }
        }

    }
    protected void PNIListMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            Guid pniid = Guid.Empty;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidInspectionPNIOperation())
                {
                    if (ViewState["PNIID"] == null)
                    {
                        DataSet ds = PhoenixInspectionPNI.PNIMedicalCaseDuplicateCheck(General.GetNullableInteger(ucVessel.SelectedVessel),
                                                   General.GetNullableInteger(txtCrewId.Text), General.GetNullableDateTime(ucInjuryDate.Text));

                        DataRow dr = ds.Tables[0].Rows[0];
                        if (int.Parse(dr["FLDMEDICALCASECOUNT"].ToString()) <= 0)
                        {

                            PhoenixInspectionPNI.PNIDoctorReportInsert(int.Parse(ucVessel.SelectedVessel)
                                                    , int.Parse(txtCrewId.Text)
                                                    , General.GetNullableInteger(ucPort.SelectedValue)
                                                    , General.GetNullableInteger(txtAgent.Text)
                                                    , General.GetNullableDateTime(ucInjuryDate.Text)
                                                    , General.GetNullableString(txtDescription.Text)
                                                    , General.GetNullableDateTime(ucDoctorVisitDate.Text)
                                                    , chkHospital.Checked ? 1 : 0
                                                    , null
                                                    , null
                                                    , General.GetNullableInteger(ucPartsinjured.SelectedQuick)
                                                    , General.GetNullableInteger(ucInjuryType.SelectedQuick)
                                                    , General.GetNullableInteger(ucInjuryCategory.SelectedQuick)
                                                    , General.GetNullableInteger(ucTypeofcase.SelectedHard)
                                                    , ref pniid
                                                    , General.GetNullableString(txtPNIClubCaseNo.Text)
                                                    , General.GetNullableString(txtHospitalName.Text)
                                                    , General.GetNullableString(txtHospitalContactNo.Text)
                                                    , General.GetNullableDateTime(ucInformedPNI.Text)
                                        );
                            //ucStatus.Text = "Operation details added.";
                            ViewState["PNIID"] = pniid.ToString();
                            EditOperation();
                           gvDeficiency.Rebind();
                            
                            imgPPClip.Attributes["onclick"] = "javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.INFORMEDPNI + "&cmdname=FINFORMEDPNIUPLOAD'); return false;";
                            divGrid.Visible = true;
                            MenuDeficiency.Visible = true;
                          
                            imgPPClip.Visible = true;
                        }
                        else
                        {
                            ucConfirmDuplicate.Visible = true;
                            ucConfirmDuplicate.Text = "Suspected duplicate entry of a case for this seafarer. Kindly confirm you still wish to continue.";
                            return;
                        }

                    }
                    else
                    {
                        PhoenixInspectionPNI.PNIDoctorReportUpdate(
                                                      General.GetNullableInteger(ucPort.SelectedValue)
                                                    , General.GetNullableInteger(txtAgent.Text)
                                                    , General.GetNullableString(txtDescription.Text)
                                                    , General.GetNullableDateTime(ucDoctorVisitDate.Text)
                                                    , chkHospital.Checked ? 1 : 0
                                                    , null
                                                    , null
                                                    , General.GetNullableInteger(ucPartsinjured.SelectedQuick)
                                                    , General.GetNullableInteger(ucInjuryType.SelectedQuick)
                                                    , General.GetNullableInteger(ucInjuryCategory.SelectedQuick)
                                                    , General.GetNullableInteger(ucTypeofcase.SelectedHard)
                                                    , new Guid(ViewState["PNIID"].ToString())
                                                    , General.GetNullableString(txtPNIClubCaseNo.Text)
                                                    , General.GetNullableString(txtHospitalName.Text)
                                                    , General.GetNullableString(txtHospitalContactNo.Text)
                                                    , General.GetNullableDateTime(ucInformedPNI.Text)
                                        );
                     
                       gvDeficiency.Rebind();
                       
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
                ViewState["PNIID"] = null;
                Reset();
                EditOperation();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void btnConfirmDuplicate_Click(object sender, EventArgs e)
    {
        try
        {
            Guid pniid = Guid.Empty;

            UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            if (ucCM.confirmboxvalue == 1)
            {
                PhoenixInspectionPNI.PNIDoctorReportInsert(int.Parse(ucVessel.SelectedVessel)
                                                   , int.Parse(txtCrewId.Text)
                                                   , General.GetNullableInteger(ucPort.SelectedValue)
                                                   , General.GetNullableInteger(txtAgent.Text)
                                                   , General.GetNullableDateTime(ucInjuryDate.Text)
                                                   , General.GetNullableString(txtDescription.Text)
                                                   , General.GetNullableDateTime(ucDoctorVisitDate.Text)
                                                   , chkHospital.Checked ? 1 : 0
                                                   , null
                                                   , null
                                                   , General.GetNullableInteger(ucPartsinjured.SelectedQuick)
                                                   , General.GetNullableInteger(ucInjuryType.SelectedQuick)
                                                   , General.GetNullableInteger(ucInjuryCategory.SelectedQuick)
                                                   , General.GetNullableInteger(ucTypeofcase.SelectedHard)
                                                   , ref pniid
                                                   , General.GetNullableString(txtPNIClubCaseNo.Text)
                                                   , General.GetNullableString(txtHospitalName.Text)
                                                   , General.GetNullableString(txtHospitalContactNo.Text)
                                                   , General.GetNullableDateTime(ucInformedPNI.Text)
                                       );
                ViewState["PNIID"] = pniid.ToString();
                EditOperation();
                gvDeficiency.Rebind();
              
                imgPPClip.Attributes["onclick"] = "javascript:parent.Openpopup('NATD','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["attachmentcode"] + "&mod="
                + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.INFORMEDPNI + "&cmdname=FINFORMEDPNIUPLOAD'); return false;";
                divGrid.Visible = true;
                MenuDeficiency.Visible = true;
              
                imgPPClip.Visible = true;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }


    private void EditOperation()
    {
        if (ViewState["PNIID"] != null)
        {
            DataSet ds = PhoenixInspectionPNI.PNIDoctorReportEdit(new Guid(ViewState["PNIID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];

                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ucVessel.Enabled = false;
                txtcaseNo.Text = dr["FLDREFERENCENO"].ToString();
                ViewState["REFNO"] = dr["FLDREFERENCENO"].ToString();
                ucInjuryDate.Text = General.GetDateTimeToString(dr["FLDDATEOFILLNESS"].ToString());
                ucInjuryDate.Enabled = false;
                ucMedicalFitDate.Text = dr["FLDMEDICALFITDATE"].ToString();
                txtCrewId.Text = dr["FLDEMPLOYEEID"].ToString();
                ViewState["empid"] = dr["FLDEMPLOYEEID"].ToString();
                txtCrewName.Text = dr["FLDCREWNAME"].ToString();
                txtCrewRank.Text = dr["FLDCREWRANK"].ToString();
                imgShowCrewInCharge.Visible = false;

                txtDescription.Text = dr["FLDILLNESSDESCRIPTION"].ToString();
                chkHospital.Checked = dr["FLDHOSPITALIZEDYN"].ToString() == "1" ? true : false;
                txtPOVessel.Text = dr["FLDPONAME"].ToString();
                txtSingaporePIC.Text = dr["FLDSINGAPOREPIC"].ToString();
                ucTypeofcase.SelectedHard = dr["FLDTYPEOFMEDICALCASE"].ToString();
                txtDaysLost.Text = dr["FLDDAYSLOST"].ToString();
                txtServiceYears.Text = dr["FLDSERVICEYEARS"].ToString();
                ucPartsinjured.SelectedQuick = dr["FLDINJUREDPARTS"].ToString();
                ucInjuryType.SelectedQuick = dr["FLDTYPESOFINJURY"].ToString();
                ucInjuryCategory.SelectedQuick = dr["FLDCATEGORYOFWORKINJURY"].ToString();
                ucPort.SelectedValue = dr["FLDPORTOFCALL"].ToString();
                ucPort.Text = dr["PORTNAME"].ToString(); ;
                ucPort.Enabled = false;
                txtAgentNumber.Text = dr["FLDPORTAGENTCODE"].ToString();
                txtAgentName.Text = dr["FLDPORTAGENTNAME"].ToString();
                txtAgent.Text = dr["FLDPORTAGENT"].ToString();
                cmdShowAgent.Visible = false;
                ucDoctorVisitDate.Text = General.GetDateTimeToString(dr["FLDDOCTORVISITDATE"].ToString());
                //ucDoctorVisitDate.Enabled = false;
                txtServiceYears.Text = dr["FLDCOMPANYEXPERIENCE"].ToString();
                txtCaseOpenedby.Text = dr["FLDCASEOPENEDBY"].ToString();
                ucCaseOpenedDate.Text = dr["FLDCASEOPENEDDATE"].ToString();
                txtPNIClubCaseNo.Text = dr["FLDPNICLUBCASENO"].ToString();
                txtHospitalName.Text = dr["FLDHOSPITALNAME"].ToString();
                txtHospitalContactNo.Text = dr["FLDHOSPITALCONTACTNO"].ToString();
                ucInformedPNI.Text = dr["FLDINFORMEDTOPNI"].ToString();
                ViewState["attachmentcode"] = dr["FLDDTKEY"].ToString();

                if (dr["FLDISATTACHMENT"].ToString() == string.Empty)
                    imgPPClip.ImageUrl = Session["images"] + "/no-attachment.png";
                else
                    imgPPClip.ImageUrl = Session["images"] + "/attachment.png";

                if (chkHospital.Checked == true)
                {
                    txtHospitalName.Enabled = true;
                    txtHospitalContactNo.Enabled = true;
                }
                else
                {
                    txtHospitalName.Enabled = false;
                    txtHospitalContactNo.Enabled = false;
                }
            }
        }
        else
        {
            Reset();
        }

    }
    private bool IsValidInspectionPNIOperation()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultdate;

        if (General.GetNullableDateTime(ucInjuryDate.Text) == null)
            ucError.ErrorMessage = "Injury Date is required.";

        //if (General.GetNullableDateTime(ucDoctorVisitDate.Text) == null)
        //    ucError.ErrorMessage = "Doctor Visit Date is required.";

        else if (DateTime.TryParse(ucInjuryDate.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(DateTime.Now.ToLongDateString())) > 0)
            ucError.ErrorMessage = "Injury Date Should not be a Future Date";

        if (General.GetNullableInteger(ucVessel.SelectedVessel) == null)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(txtCrewId.Text) == null)
            ucError.ErrorMessage = "Crew Name is required.";

        if (string.IsNullOrEmpty(ucTypeofcase.SelectedHard) || ucTypeofcase.SelectedHard.ToUpper().ToString() == "DUMMY")
            ucError.ErrorMessage = "Type of Case is required.";

        if (General.GetNullableInteger(ucPort.SelectedValue) == null)
            ucError.ErrorMessage = "Port is required.";

        if (General.GetNullableInteger(txtAgent.Text) == null)
            ucError.ErrorMessage = "Agent is required.";

        if (DateTime.TryParse(ucInformedPNI.Text, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(DateTime.Now.ToLongDateString())) > 0)
            ucError.ErrorMessage = "Informed to PNI Club Should not be a Future Date";

        if (General.GetNullableDateTime(ucInformedPNI.Text) != null)
        {
            if (DateTime.Parse(ucInjuryDate.Text) > DateTime.Parse(ucInformedPNI.Text))
                ucError.ErrorMessage = "Informed to PNI Club Should not be less than Injury Date";
        }

        if (ucTypeofcase.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 174, "INJ"))
        {
            if (string.IsNullOrEmpty(ucPartsinjured.SelectedQuick) || ucPartsinjured.SelectedQuick.ToUpper().ToString() == "DUMMY")
                ucError.ErrorMessage = " Parts of Body Injured is required.";

            if (string.IsNullOrEmpty(ucInjuryType.SelectedQuick) || ucInjuryType.SelectedQuick.ToUpper().ToString() == "DUMMY")
                ucError.ErrorMessage = "Injury Type is required.";

            if (string.IsNullOrEmpty(ucInjuryCategory.SelectedQuick) || ucInjuryCategory.SelectedQuick.ToUpper().ToString() == "DUMMY")
                ucError.ErrorMessage = "Injury Category is required.";

        }


        return (!ucError.IsError);
    }
    private void Reset()
    {

    }
    protected void ucTypeofcase_Changed(object sender, EventArgs e)
    {
        try
        {
            UserControlHard ucCaseType = (UserControlHard)sender;

            if (ucCaseType.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 174, "INJ"))
            {
                ucInjuryType.Enabled = true;
                ucInjuryType.SelectedQuick = string.Empty;
                ucInjuryCategory.Enabled = true;
                ucInjuryCategory.SelectedQuick = string.Empty;
                ucPartsinjured.Enabled = true;
                ucPartsinjured.SelectedQuick = string.Empty;
            }
            else
            {
                ucInjuryType.Enabled = false;
                ucInjuryType.SelectedQuick = string.Empty;
                ucInjuryCategory.Enabled = false;
                ucInjuryCategory.SelectedQuick = string.Empty;
                ucPartsinjured.Enabled = false;
                ucPartsinjured.SelectedQuick = string.Empty;
            }
            if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != "")
            {
                gvDeficiency.Rebind();
               
            }
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDREFERENCENO", "FLDVESSELNAME", "FLDCREWNAME", "FLDCREWRANK", "FLDDATEOFILLNESS", "FLDDOCTORVISITDATE", "FLDILLNESSDESCRIPTION" };
            string[] alCaptions = { "Reference Number", "Vessel", "Crew Name", "Rank", "Illness Date", "Doctor Visit Date", "Description" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds;

            ds = PhoenixInspectionPNI.PNIMedicalCaseFollowupSearch
                 (new Guid(ViewState["PNIID"].ToString()),
                  sortexpression,
                  sortdirection,
                  Convert.ToInt16(ViewState["PAGENUMBER"].ToString()),
                  iRowCount,
                  ref iRowCount,
                  ref iTotalPageCount
                  );

            General.ShowExcel("Medical Case", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Deficiency_TabStripCommand(object sender, EventArgs e)
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

    private void BindData()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDREFERENCENO", "FLDVESSELNAME", "FLDCREWNAME", "FLDCREWRANK", "FLDDATEOFILLNESS", "FLDDOCTORVISITDATE", "FLDILLNESSDESCRIPTION" };
            string[] alCaptions = { "Reference Number", "Vessel", "Crew Name", "Rank", "Illness Date", "Doctor Visit Date", "Description" };


            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;
            ds = PhoenixInspectionPNI.PNIMedicalCaseFollowupSearch
                 (new Guid(ViewState["PNIID"].ToString()),
                  sortexpression,
                  sortdirection,
                  Convert.ToInt16(ViewState["PAGENUMBER"].ToString()),
                  gvDeficiency.PageSize,
                  ref iRowCount,
                  ref iTotalPageCount
                  );

            General.SetPrintOptions("gvDeficiency", "Medical Case", alCaptions, alColumns, ds);
            gvDeficiency.DataSource = ds;
            gvDeficiency.VirtualItemCount = iRowCount;

           
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   

    protected void gvDeficiency_Sorting(object sender, GridViewSortEventArgs se)
    {
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;


        BindData();
    }

 

   

  
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
            gvDeficiency.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

 


   

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
 

    protected void ucVessel_TextChanged(object sender, EventArgs e)
    {
        txtCrewId.Text = null;
        txtCrewName.Text = null;
        txtCrewRank.Text = null;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            //ViewState["DIRECTOBSERVATIONID"] = null;
            BindData();
            gvDeficiency.Rebind();
            //SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Hospitalizedfn(object sender, EventArgs e)
    {

        if (chkHospital.Checked == true)
        {
            txtHospitalName.Enabled = true;
            txtHospitalContactNo.Enabled = true;
        }
        else
        {
            txtHospitalName.Enabled = false;
            txtHospitalContactNo.Enabled = false;
        }
    }

    protected void gvDeficiency_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDeficiency.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvDeficiency_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

          
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

                e.Item.Selected = true;
               
            }
            else if (e.CommandName == "Page")
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

    protected void gvDeficiency_ItemDataBound1(object sender, GridItemEventArgs e)
    {
        try
        {

            
            if (e.Item is GridDataItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                int? n = General.GetNullableInteger(drv["FLDISFOLLOWUP"].ToString());
                RadLabel lblPNIMedicalCaseId = (RadLabel)e.Item.FindControl("lblPNIMedicalCaseId");
                LinkButton cmdSicknessReport = (LinkButton)e.Item.FindControl("cmdSicknessReport");
                if (cmdSicknessReport != null)
                {
                    cmdSicknessReport.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','"+Session["sitepath"]+"/Reports/ReportsView.aspx?applicationcode=9&reportcode=SICKNESSREPORT&showmenu=false&showexcel=no&PNIDOCTORREPORTID=" + lblPNIMedicalCaseId.Text.ToString() + "');return true;");
                    cmdSicknessReport.Visible = SessionUtil.CanAccess(this.ViewState, cmdSicknessReport.CommandName);
                }
                LinkButton ckl = (LinkButton)e.Item.FindControl("cmdChkList");
                if (ckl != null) ckl.Visible = SessionUtil.CanAccess(this.ViewState, ckl.CommandName);
                if (lblPNIMedicalCaseId != null && ckl != null && !string.IsNullOrEmpty(lblPNIMedicalCaseId.Text) && n == 0)
                {
                    ckl.Visible = true;
                    ckl.Attributes.Add("onclick", "parent.openNewWindow('CrewPage', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&showmenu=false&showexcel=no&showword=no&reportcode=PNICHECKLIST&pnicaseid=" + lblPNIMedicalCaseId.Text + "');return false;");
                }
                else
                    ckl.Visible = false;
            }
            if (e.Item is GridDataItem)
            {
                RadLabel lblDtkey = (RadLabel)e.Item.FindControl("lblDTKey");
                ImageButton cmdAttachment = (ImageButton)e.Item.FindControl("cmdAttachment");
                if (cmdAttachment != null)
                {
                    cmdAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text.ToString() + "&mod="
                                        + PhoenixModule.QUALITY + "');return true;");
                    cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
                }
                ImageButton cmdNoAttachment = (ImageButton)e.Item.FindControl("cmdNoAttachment");
                if (cmdNoAttachment != null)
                {
                    cmdNoAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text.ToString() + "&mod="
                                        + PhoenixModule.QUALITY + "');return true;");
                    cmdNoAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdNoAttachment.CommandName);
                }

                DataRowView drv = (DataRowView)e.Item.DataItem;
                ImageButton iab = (ImageButton)e.Item.FindControl("cmdAttachment");
                ImageButton inab = (ImageButton)e.Item.FindControl("cmdNoAttachment");
                if (iab != null) iab.Visible = true;
                if (inab != null) inab.Visible = false;
                int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());
                if (n == 0)
                {
                    if (iab != null) iab.Visible = false;
                    if (inab != null) inab.Visible = true;
                }
              
            }
            if (e.Item is GridDataItem)
            {
                RadLabel lbl = (RadLabel)e.Item.FindControl("lblPNIMedicalCaseId");
                HtmlImage img = (HtmlImage)e.Item.FindControl("imgRemarks");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
                img.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                img.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                img.Attributes.Add("onclick", "parent.openNewWindow('MoreInfo','','" + Session["sitepath"] + "/Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=MEDICALCASE','xlarge')");
                RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
                if (string.IsNullOrEmpty(lblR.Text.Trim()))
                {
                    uct.Visible = false;
                    img.Src = Session["images"] + "/no-remarks.png";
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
