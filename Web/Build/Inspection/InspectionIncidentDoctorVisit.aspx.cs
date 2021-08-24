using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class InspectionIncidentDoctorVisit : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        imgShowCrewInCharge.Attributes.Add("onclick",
                "return showPickList('spnCrewInCharge', 'codehelp1', '','"+Session["sitepath"]+"/Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId="
                + ucVessel.SelectedVessel + "', true); ");

       
        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {
            ViewState["PNIID"] = null;
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            cmdShowAgent.Attributes.Add("onclick",
           "return showPickList('spnPickListAgent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListAddress.aspx?addresstype=1255', true);");

            //ucVessel.bind();
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
            {
                ucVessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
                ucVessel.Enabled = false;
            }

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                ucVessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
                ucVessel.Enabled = false;
            }

            if (Request.QueryString["PNIID"] != null)
                ViewState["PNIID"] = Request.QueryString["PNIID"].ToString();

            if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != "")
            {
                ViewState["FOLLOWUPFOR"] = ViewState["PNIID"].ToString();
                EditDoctorVisit();
                //BindData();
                //SetPageNavigator();

            }
            else
            {
                divGrid.Visible = false;
                MenuDeficiency.Visible = false;
                
            }

            txtAgent.Attributes.Add("style", "visibility:hidden");
            txtCrewId.Attributes.Add("style", "visibility:hidden");
            txtCrewRank.Attributes.Add("style", "visibility:hidden");
            gvDeficiency.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
           
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("List", "LIST");
        toolbar.AddButton("Doctor Visit", "DOCTOR");
        MenuIncidentDoctorVisit.MenuList = toolbar.Show();
        MenuIncidentDoctorVisit.SelectedMenuIndex = 1;

        toolbar = new PhoenixToolbar();
        toolbar.AddButton("Follow Up", "FOLLOWUP", ToolBarDirection.Right);
        toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
        
        MenuIncidentDoctorVisitGeneral.AccessRights = this.ViewState;
        MenuIncidentDoctorVisitGeneral.MenuList = toolbar.Show();

        PhoenixToolbar toolbargrid = new PhoenixToolbar();
        toolbargrid.AddFontAwesomeButton("../Inspection/InspectionIncidentDoctorVisit.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvDeficiency')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        MenuDeficiency.AccessRights = this.ViewState;
        MenuDeficiency.MenuList = toolbargrid.Show();
        //ucVessel.bind();
        if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0)
        {
            ucVessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.InstallCode;
            ucVessel.Enabled = false;
        }
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
        {
            ucVessel.SelectedValue = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
            ucVessel.Enabled = false;
        }
       
    }

    protected void EditDoctorVisit()
    {
        if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != string.Empty)
        {
            DataSet ds = PhoenixInspectionPNI.PNIDoctorReportEdit(new Guid(ViewState["PNIID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
                ucVessel.Enabled = false;
                txtCrewId.Text = dr["FLDEMPLOYEEID"].ToString();
                txtCrewName.Text = dr["FLDCREWNAME"].ToString();
                imgShowCrewInCharge.Visible = false;
                ucPort.SelectedValue = dr["FLDPORTOFCALL"].ToString();
                ucPort.Text = dr["PORTNAME"].ToString();
                txtAgentNumber.Text = dr["FLDPORTAGENTCODE"].ToString();
                txtAgentName.Text = dr["FLDPORTAGENTNAME"].ToString();
                txtAgent.Text = dr["FLDPORTAGENT"].ToString();
                ucDateIllness.Text = dr["FLDDATEOFILLNESS"].ToString();
                ucDateIllness.Enabled = false;
                ucDoctorVisitDate.Text = dr["FLDDOCTORVISITDATE"].ToString();
                txtIllnessDescription.Text = dr["FLDILLNESSDESCRIPTION"].ToString();
                txtRefNo.Text = dr["FLDREFERENCENO"].ToString();

            }
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        EditDoctorVisit();
        BindData();
       
    }

    protected void MenuIncidentDoctorVisit_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("../Inspection/InspectionPNIDoctorReportList.aspx", true);
            MenuIncidentDoctorVisit.SelectedMenuIndex = 0;
        }
        else if (CommandName.ToUpper().Equals("DOCTOR"))
        {
            //Response.Redirect("../Inspection/InspectionIncidentDoctorVisit.aspx", true);
            MenuIncidentDoctorVisit.SelectedMenuIndex = 1;
        }

    }

    protected void MenuIncidentDoctorVisitGeneral_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (IsValidDoctorVisit())
                {

                    if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != string.Empty)
                    {
                        PhoenixInspectionPNI.PNIDoctorReportFollowupUpdate(new Guid(ViewState["PNIID"].ToString())
                                                    , General.GetNullableInteger(ucPort.SelectedValue)
                                                    , General.GetNullableInteger(txtAgent.Text)
                                                    , General.GetNullableDateTime(ucDoctorVisitDate.Text)
                                                    , General.GetNullableString(txtIllnessDescription.Text));

                        //ucStatus.Text = "Doctor Report Updated.";
                        //ucStatus.Visible = true;
                        EditDoctorVisit();
                       
                    }
                    else
                    {
                        Guid pniid = Guid.Empty;
                        PhoenixInspectionPNI.PNIVesselDoctorReportInsert(int.Parse(ucVessel.SelectedVessel)
                                                    , int.Parse(txtCrewId.Text)
                                                    , General.GetNullableInteger(ucPort.SelectedValue)
                                                    , General.GetNullableInteger(txtAgent.Text)
                                                    , General.GetNullableDateTime(ucDateIllness.Text)
                                                    , General.GetNullableString(txtIllnessDescription.Text)
                                                    , General.GetNullableDateTime(ucDoctorVisitDate.Text)
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , null
                                                    , ref pniid
                                        );
                        //ucStatus.Text = "Doctor Report Saved.";
                        //ucStatus.Visible = true;
                        ViewState["PNIID"] = pniid;
                        ViewState["FOLLOWUPFOR"] = pniid;
                        EditDoctorVisit();
                        gvDeficiency.Rebind();

                        divGrid.Visible = true;
                        MenuDeficiency.Visible = true;
                       
                    }
                }
                else
                {
                    ucError.Visible = true;
                    return;
                }
            }
            if (CommandName.ToUpper().Equals("FOLLOWUP"))
            {
                if (ViewState["PNIID"] == null)
                {
                    ucError.ErrorMessage = "Doctor visit is Required";
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionPNI.PNIDoctorReportFollowup(new Guid(ViewState["PNIID"].ToString()), General.GetNullableDateTime(ucDoctorVisitDate.Text));
                //ucStatus.Text = "Followup Created.";
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


    private bool IsValidDoctorVisit()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ucPort.SelectedValue) == null)
            ucError.ErrorMessage = "Port is Required.";

        if (string.IsNullOrEmpty(ucDateIllness.Text))
            ucError.ErrorMessage = "Illness On is Required.";

        if (string.IsNullOrEmpty(ucDoctorVisitDate.Text))
            ucError.ErrorMessage = "Doctor Visit On is Required.";

        if (string.IsNullOrEmpty(txtCrewId.Text))
            ucError.ErrorMessage = "Name is Required.";

        if (string.IsNullOrEmpty(txtAgent.Text))
            ucError.ErrorMessage = "Port Agent is Required.";

        return (!ucError.IsError);
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDREFERENCENO", "FLDVESSELNAME", "FLDCREWNAME", "FLDCREWRANK", "FLDDATEOFILLNESS", "FLDDOCTORVISITDATE" };
            string[] alCaptions = { "Reference No.", "Vessel", "Name", "Rank", "Illness On", "Doctor Visit" };

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
                 (new Guid(ViewState["FOLLOWUPFOR"].ToString()),
                  sortexpression,
                  sortdirection,
                  Convert.ToInt16(ViewState["PAGENUMBER"].ToString()),
                  iRowCount,
                  ref iRowCount,
                  ref iTotalPageCount
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

            string[] alColumns = { "FLDREFERENCENO", "FLDVESSELNAME", "FLDCREWNAME", "FLDCREWRANK", "FLDDATEOFILLNESS", "FLDDOCTORVISITDATE" };
            string[] alCaptions = { "Reference No.", "Vessel", "Name", "Rank", "Illness On", "Doctor Visit" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds;
            ds = PhoenixInspectionPNI.PNIMedicalCaseFollowupSearch
                 (new Guid(ViewState["FOLLOWUPFOR"].ToString()),
                  sortexpression,
                  sortdirection,
                  Convert.ToInt16(ViewState["PAGENUMBER"].ToString()),
                  iRowCount,
                  ref iRowCount,
                  ref iTotalPageCount
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

    private void VesselConfiguration()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionAuditPurchaseForm.EditConfiguration();
        if (ds.Tables[0].Rows.Count > 0)
            Filter.CurrentVesselConfiguration = ds.Tables[0].Rows[0]["FLDINSTALLCODE"].ToString();
    }
 
    
    protected void gvDeficiency_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDeficiency.CurrentPageIndex + 1;
            if (ViewState["PNIID"] != null && ViewState["PNIID"].ToString() != "")
            {
                BindData();
            }

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
                    ViewState["PNIID"] = lblPNIMedicalCaseId.Text.ToString();
                    EditDoctorVisit();
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

    protected void gvDeficiency_ItemDataBound(object sender, GridItemEventArgs e)
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
                    ckl.Visible = false;
                    ckl.Attributes.Add("onclick", "parent.openNewWindow('CrewPage', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&showmenu=false&showexcel=no&showword=no&reportcode=PNICHECKLIST&pnicaseid=" + lblPNIMedicalCaseId.Text + "');return false;");
                }
                else
                    ckl.Visible = false;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
