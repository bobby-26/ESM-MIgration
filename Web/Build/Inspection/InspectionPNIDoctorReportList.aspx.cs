using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class InspectionPNIDoctorReportList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        { 
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionPNIDoctorReportList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDeficiency')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 || PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                toolbargrid.AddFontAwesomeButton("../Inspection/InspectionIncidentDoctorVisit.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
                //Title1.Text = "Doctor Visit";
            }
            else
            {
                toolbargrid.AddFontAwesomeButton("../Inspection/InspectionPNIOperation.aspx", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
                toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','"+Session["sitepath"]+ "/Inspection/InspectionDoctorVisitFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
                toolbargrid.AddFontAwesomeButton("../Inspection/InspectionPNIDoctorReportList.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            }
            
            MenuDeficiency.AccessRights = this.ViewState;
            MenuDeficiency.MenuList = toolbargrid.Show();
           // MenuDeficiency.SetTrigger(pnlDeficiency);

            if (!IsPostBack)
            {

                DataSet ProcessGuidance = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 249, 0, "MDC");
                VesselConfiguration();
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("List", "LIST");
                //if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 || PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                //    toolbarmain.AddButton("Doctor Visit", "DOCTORVISIT");
                //else
                //    toolbarmain.AddButton("Medical Case", "MEDICALCASE");
                MenuDeficiencyGeneral.AccessRights = this.ViewState;
                MenuDeficiencyGeneral.MenuList = toolbarmain.Show();
                //MenuDeficiencyGeneral.SetTrigger(pnlDeficiency);
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                if (ProcessGuidance.Tables[0].Rows.Count > 0)
                {
                    lnkProcessGuidance.Attributes.Add("onclick", "parent.Openpopup('codehelp','','../Common/CommonFileAttachment.aspx?dtkey=" + ProcessGuidance.Tables[0].Rows[0]["FLDDTKEY"].ToString() + "&mod="
                                            + PhoenixModule.QUALITY + "');return true;");
                }
                MenuDeficiencyGeneral.SelectedMenuIndex = 0;

                gvDeficiency.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void DeficiencyGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("DOCTORVISIT"))
            {
                Response.Redirect("../Inspection/InspectionIncidentDoctorVisit.aspx");
            }
            if (CommandName.ToUpper().Equals("MEDICALCASE"))
            {
                Response.Redirect("../Inspection/InspectionPNIOperation.aspx");
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            int? vesselid = null;

            string[] alColumns = { "FLDREFERENCENO", "FLDVESSELNAME", "FLDCREWNAME", "FLDCREWRANK", "FLDSIGNOFFDATE", "FLDDATEOFILLNESS", "FLDDOCTORVISITDATE", "FLDSTATUS", "FLDPENDINGWITH", "FLDFROMDATE" };
            string[] alCaptions = { "Reference No.", "Vessel", "Name", "Rank", "Sign Off", "Illness On", "Doctor Visit", "Status", "Pending With", "Pending From" };

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

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
            else
            {
                if(PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                    vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
            NameValueCollection nvc = Filter.CurrentDoctorVisitFilter;
            ds = PhoenixInspectionPNI.PNIMedicalCaseSearch
                (nvc != null ? General.GetNullableInteger(nvc["ddlVessel"]) : vesselid,
                 General.GetNullableString(nvc != null ? nvc["ucPrincipal"] : string.Empty),
                 General.GetNullableString(nvc != null ? nvc["ucVesselType"] : string.Empty),
                 General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : string.Empty),
                  sortexpression,
                  sortdirection,
                  Convert.ToInt16(ViewState["PAGENUMBER"].ToString()),
                  iRowCount,
                  ref iRowCount,
                  ref iTotalPageCount,
                  General.GetNullableDateTime(nvc != null? nvc["txtIllnessFrom"] : string.Empty),
                  General.GetNullableDateTime(nvc != null ? nvc["txtIllnessTo"] : string.Empty),
                  General.GetNullableDateTime(nvc != null ? nvc["txtClosingFrom"] : string.Empty),
                  General.GetNullableDateTime(nvc != null ? nvc["txtClosingTo"] : string.Empty),
                  General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty),
                  General.GetNullableString(nvc != null ? nvc["txtName"] : string.Empty),
                  General.GetNullableString(nvc != null ? nvc["txtFileNo"] : string.Empty)
                  );

            General.ShowExcel("Doctor Visit", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
            else if (CommandName.ToUpper().Equals("ADD"))
            {
                Response.Redirect("../Inspection/InspectionIncidentDoctorVisit.aspx");
                //BindData();
                //SetPageNavigator();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentDoctorVisitFilter = null;
                gvDeficiency.Rebind();
                
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
            int? vesselid = null;

            string[] alColumns = { "FLDREFERENCENO", "FLDVESSELNAME", "FLDCREWNAME", "FLDCREWRANK", "FLDSIGNOFFDATE", "FLDDATEOFILLNESS", "FLDDOCTORVISITDATE", "FLDSTATUS", "FLDPENDINGWITH","FLDFROMDATE" };
            string[] alCaptions = { "Reference No.", "Vessel", "Name", "Rank", "Sign Off", "Illness On", "Doctor Visit", "Status", "Pending With", "Pending From" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            
            DataSet ds;

            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
            else
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                    vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            }
            NameValueCollection nvc = Filter.CurrentDoctorVisitFilter;
            ds = PhoenixInspectionPNI.PNIMedicalCaseSearch
                (nvc != null ? General.GetNullableInteger(nvc["ddlVessel"]) : vesselid,
                 General.GetNullableString(nvc!= null ? nvc["ucPrincipal"] : string.Empty),
                 General.GetNullableString(nvc != null ? nvc["ucVesselType"] : string.Empty),
                 General.GetNullableInteger(nvc!=null ? nvc["ddlRank"] : string.Empty),
                  sortexpression,
                  sortdirection,
                  Convert.ToInt16(ViewState["PAGENUMBER"].ToString()),
                  gvDeficiency.PageSize,
                  ref iRowCount,
                  ref iTotalPageCount,
                  General.GetNullableDateTime(nvc != null ? nvc["txtIllnessFrom"] : string.Empty),
                  General.GetNullableDateTime(nvc != null ? nvc["txtIllnessTo"] : string.Empty),
                  General.GetNullableDateTime(nvc != null ? nvc["txtClosingFrom"] : string.Empty),
                  General.GetNullableDateTime(nvc != null ? nvc["txtClosingTo"] : string.Empty),
                  General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty),
                  General.GetNullableString(nvc != null ? nvc["txtName"] : string.Empty),
                  General.GetNullableString(nvc != null ? nvc["txtFileNo"] : string.Empty)
                  ); 

            General.SetPrintOptions("gvDeficiency", "Doctor Visit", alCaptions, alColumns, ds);
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

   

  

   

    
    private void BindPageURL(int rowindex)
    {
        try
        {
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
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
    protected void gvDeficiency_RowEditing(object sender, GridViewEditEventArgs e)
    {

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


                RadLabel lblPNIMedicalCaseId = ((RadLabel)e.Item.FindControl("lblPNIMedicalCaseId"));
                if (lblPNIMedicalCaseId != null)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 || PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                        Response.Redirect("../Inspection/InspectionIncidentDoctorVisit.aspx?PNIID=" + lblPNIMedicalCaseId.Text.ToString(), false);
                    else
                        Response.Redirect("../Inspection/InspectionPNIOperation.aspx?PNIID=" + lblPNIMedicalCaseId.Text.ToString(), false);
                }
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
                RadLabel lblEmpNo = (RadLabel)e.Item.FindControl("lblEmpNo");
                LinkButton lnkName = (LinkButton)e.Item.FindControl("lblCrewName");
                if (lblEmpNo != null && lnkName != null)
                {
                    lnkName.Attributes.Add("onclick", "javascript:parent.openNewWindow('CrewListForAPeriod','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + lblEmpNo.Text + "'); return false;");
                }

                RadLabel lblPNIMedicalCaseId = (RadLabel)e.Item.FindControl("lblPNIMedicalCaseId");
                LinkButton cmdSicknessReport = (LinkButton)e.Item.FindControl("cmdSicknessReport");
                LinkButton lblFrom = (LinkButton)e.Item.FindControl("lblFrom");

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
                {
                    lnkName.Enabled = false;
                    lnkName.Attributes.Clear();
                }
                if (cmdSicknessReport != null)
                {
                    cmdSicknessReport.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','"+Session["sitepath"]+"/Reports/ReportsView.aspx?applicationcode=9&reportcode=SICKNESSREPORT&showmenu=false&showexcel=no&PNIDOCTORREPORTID=" + lblPNIMedicalCaseId.Text.ToString() + "');return true;");
                    cmdSicknessReport.Visible = SessionUtil.CanAccess(this.ViewState, cmdSicknessReport.CommandName);
                }
                if (lblFrom != null)
                {
                    lblFrom.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Inspection/InspectionPNIMedicalCaseStatusDurationList.aspx?PNICASEID=" + lblPNIMedicalCaseId.Text.ToString() + "');return true;");
                    lblFrom.Visible = SessionUtil.CanAccess(this.ViewState, lblFrom.CommandName);
                }
                LinkButton ckl = (LinkButton)e.Item.FindControl("cmdChkList");
                if (ckl != null) ckl.Visible = SessionUtil.CanAccess(this.ViewState, ckl.CommandName);
                if (lblPNIMedicalCaseId != null && ckl != null && !string.IsNullOrEmpty(lblPNIMedicalCaseId.Text))
                {
                    ckl.Visible = true;
                    ckl.Attributes.Add("onclick", "parent.openNewWindow('CrewPage', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&showexcel=no&showword=no&reportcode=PNICHECKLIST&pnicaseid=" + lblPNIMedicalCaseId.Text + "');return false;");
                }
                else
                    ckl.Visible = false;
                LinkButton imgClose = (LinkButton)e.Item.FindControl("cmdClose");
                if (imgClose != null)
                {
                    if (lblPNIMedicalCaseId != null && !string.IsNullOrEmpty(lblPNIMedicalCaseId.Text))
                    {
                        imgClose.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Inspection/InspectionPNIMedicalCaseClose.aspx?pniid=" + lblPNIMedicalCaseId.Text.ToString() + "');return true;");
                        imgClose.Visible = SessionUtil.CanAccess(this.ViewState, imgClose.CommandName);
                    }
                }

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

                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 || PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    LinkButton ckl = (LinkButton)e.Item.FindControl("cmdChkList");
                    ImageButton iab1 = (ImageButton)e.Item.FindControl("cmdAttachment");
                    ImageButton inab1 = (ImageButton)e.Item.FindControl("cmdNoAttachment");


                    LinkButton btnClose = (LinkButton)e.Item.FindControl("cmdClose");
                    iab1.Visible = false;
                    inab1.Visible = false;


                    if (ckl != null) ckl.Visible = false;
                    if (btnClose != null) btnClose.Visible = false;
                }

            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDeficiency_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
