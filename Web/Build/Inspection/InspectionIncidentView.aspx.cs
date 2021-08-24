using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class InspectionIncidentView : PhoenixBasePage
{
    protected override void Render(HtmlTextWriter writer)
    {
        if (!IsPostBack)
        {
            
        }
        imgPersonInCharge.Attributes.Add("onclick",
               "return showPickList('spnPersonInCharge', 'codehelp1', '','../Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId="
               + ucVessel.SelectedVessel + "', true); ");
        imgImmediateAssignedTo.Attributes.Add("onclick",
               "return showPickList('spnAssignedTo', 'codehelp1', '','../Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId="
               + ucVessel.SelectedVessel + "', true); ");

        imgReportedByShip1.Attributes.Add("onclick",
               "return showPickList('spnReportedByShip1', 'codehelp1', '','../Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId="
               + ucVessel.SelectedVessel + "', true); ");

        imgReportedByShip2.Attributes.Add("onclick",
               "return showPickList('spnReportedByShip2', 'codehelp1', '','../Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId="
               + ucVessel.SelectedVessel + "', true); ");

        imgReportedByShip3.Attributes.Add("onclick",
               "return showPickList('spnReportedByShip3', 'codehelp1', '','../Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId="
               + ucVessel.SelectedVessel + "', true); ");

        base.Render(writer);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        try
        {
            if (!IsPostBack)
            {
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["INCIDENTID"] = null;

                ucConfirmComplete.Visible = false;
                txtCrewId.Attributes.Add("style", "visibility:hidden");
                txtReportedByShipId.Attributes.Add("style", "visibility:hidden");
                txtImmediateAssignedToId.Attributes.Add("style", "visibility:hidden");

                txtReportedByShipId1.Attributes.Add("style", "visibility:hidden");
                txtReportedByShipId2.Attributes.Add("style", "visibility:hidden");
                txtReportedByShipId3.Attributes.Add("style", "visibility:hidden");

                imgReportedByShip1.Attributes.Add("onclick",
                        "return showPickList('spnReportedByShip1', 'codehelp1', '','../Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId="
                        + ucVessel.SelectedVessel + "', true); ");

                imgReportedByShip2.Attributes.Add("onclick",
                       "return showPickList('spnReportedByShip2', 'codehelp1', '','../Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId="
                       + ucVessel.SelectedVessel + "', true); ");

                imgReportedByShip3.Attributes.Add("onclick",
                       "return showPickList('spnReportedByShip3', 'codehelp1', '','../Common/CommonPickListVesselEmployeeOnboard.aspx?VesselId="
                       + ucVessel.SelectedVessel + "', true); ");

                if (Request.QueryString["incidentid"] != null && Request.QueryString["incidentid"].ToString() != "")
                    ViewState["INCIDENTID"] = Request.QueryString["incidentid"].ToString();

                if (ViewState["INCIDENTID"] != null)
                {
                    BindInspectionIncident();
                }
                else
                {
                    Reset();
                }
            }
            BindData();
            BindInvestigation();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void BindInspectionIncident()
    {
        DataSet ds;

        ds = PhoenixInspectionIncident.EditInspectionIncident(new Guid(ViewState["INCIDENTID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ucVessel.SelectedVessel = dr["FLDVESSELID"].ToString();
            ucVessel.Enabled = false;

            txtRefNo.Text = dr["FLDINCIDENTREFNO"].ToString();
            txtTitle.Text = dr["FLDINCIDENTTITLE"].ToString();
            txtDateOfIncident.Text = General.GetDateTimeToString(dr["FLDINCIDENTDATE"].ToString());
            txtTimeOfIncident.Text = dr["FLDINCIDENTDATETIME"].ToString();
            txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
            //txtWindCondition.Text = dr["FLDWINDCONDITION"].ToString();
            txtSeaCondition.Text = dr["FLDSEACONDITION"].ToString();
            ucWindCondionScale.SelectedQuick = dr["FLDWINDCONDITIONSCALE"].ToString();
            txtWindDirection.Text = dr["FLDWINDDIRECTION"].ToString();
            ucSwellLength.SelectedQuick = dr["FLDSWELLLENGTH"].ToString();
            ucSwellHeight.SelectedQuick = dr["FLDSWELLHEIGHT"].ToString();
            txtSwellDirection.Text = dr["FLDSWELLDIRECTION"].ToString();

            ucOnboardLocation.SelectedQuick = dr["FLDLOCATIONOFINCIDENT"].ToString();
            txtDateOfReport.Text = General.GetDateTimeToString(dr["FLDREPORTEDDATE"].ToString());
            ucConsequenceCategory.SelectedHard = dr["FLDINCIDENTCATEGORY"].ToString();
            ucPotentialCategory.SelectedHard = dr["FLDPOTENTIALCATEGORY"].ToString();
            ucActivity.SelectedHard = dr["FLDACTIVITYRELEVENT"].ToString();

            ucLatitude.Text = dr["FLDLATITUDE"].ToString();
            ucLongitude.Text = dr["FLDLONGITUDE"].ToString();
            txtComprehensiveDescription.Text = dr["FLDINCIDENTCOMPREHENSIVEDESCRIPTION"].ToString();

            if (dr["FLDACTIVITYRELEVENT"].ToString().Equals(PhoenixCommonRegisters.GetHardCode(1, 170, "OTH")))
            {
                txtOtherActivity.CssClass = "input_mandatory";
                txtOtherActivity.ReadOnly = false;
            }

            txtOtherActivity.Text = dr["FLDOTHERACTIVITY"].ToString();
            txtCrewId.Text = dr["FLDPERSONINCHARGE"].ToString();
            txtCrewName.Text = dr["FLDPERSONINCHARGENAME"].ToString();
            txtCrewRank.Text = dr["FLDPERSONINCHARGERANK"].ToString();

            txtReportedByShipName.Text = dr["FLDREPORTEDBYNAME"].ToString();
            txtReportedbyDesignation.Text = dr["FLDREPORTEDBYDESIGNATION"].ToString();
            txtReportedByShipId.Text = dr["FLDREPORTEDBYID"].ToString();

            txtReportedbyDesignation.Text = dr["FLDREPORTEDBYDESIGNATION"].ToString();

            ucPort.SelectedSeaport = dr["FLDPORT"].ToString();
            ucQuickVesselActivity.SelectedQuick = dr["FLDVESSELACTIVITY"].ToString();
            
            txtCurrent.Text = dr["FLDCURRENTANDVISIBILITY"].ToString();
            txtVisibility.Text = dr["FLDVISIBILITY"].ToString();            
            
            rblIncidentNearmiss.SelectedValue = dr["FLDISINCIDENTORNEARMISS"].ToString();
            txtRemarks.Text = dr["FLDREMARKS"].ToString();

            if (dr["FLDISINCIDENTORNEARMISS"].ToString() == "1")
            {
                ucCategory.CssClass = "input_mandatory";
                ucSubcategory.CssClass = "input_mandatory";
                ucConsequenceCategory.Enabled = true;
                ucPotentialCategory.Enabled = false;
                ucPotentialCategory.SelectedHard = "";
                txtConsequencePotential.Text = dr["FLDINCIDENTCATEGORYNAME"].ToString();
                lblConsequencePotential.Text = "Consequence Category";
            }
            else
            {
                //ucSubcategory.CssClass = "input";
                //ucCategory.CssClass = "input";
                ucPotentialCategory.Enabled = true;
                ucConsequenceCategory.Enabled = false;
                ucPotentialCategory.SelectedHard = "";
                txtConsequencePotential.Text = dr["FLDPOTENTIALCATEGORYNAME"].ToString();
                lblConsequencePotential.Text = "Potential Category";
            }
            
            ucCategory.TypeId = dr["FLDISINCIDENTORNEARMISS"].ToString();
            ucCategory.CategoryList = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(General.GetNullableInteger(dr["FLDISINCIDENTORNEARMISS"].ToString()));
            ucCategory.DataBind();
            ucCategory.SelectedCategory = dr["FLDCATEGORY"].ToString();

            ucSubcategory.CategoryId = dr["FLDCATEGORY"].ToString();
            ucSubcategory.SubCategoryList = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(dr["FLDCATEGORY"].ToString()));
            ucSubcategory.DataBind();
            ucSubcategory.SelectedSubCategory = dr["FLDSUBCATEGORY"].ToString();

            if (dr["FLDRAISEDFROM"].ToString() == "1")
                txtRemarks.Enabled = false;
            else
                txtRemarks.Enabled = true;
            txtReviewcategory.Text = dr["FLDREVIEWCATEGORYNAME"].ToString();

            txtCancelReason.Text = dr["FLDINCIDENTCANCELREASON"].ToString();
            ucCancelDate.Text = dr["FLDCANCELDATE"].ToString();
            txtCancelledByName.Text = dr["FLDCANCELLEDBYNAME"].ToString();
            txtCancelledByDesignation.Text = dr["FLDCANCELLEDBYDESIGNATION"].ToString();
            txtImmediateActionTaken.Text = dr["FLDIMMEDIATEACTION"].ToString();

            txtImmediateAssignedToId.Text = dr["FLDIMMEDIATEACTIONASSIGNEDTO"].ToString();
            txtImmediateAssignedToName.Text = dr["FLDIMMEDIATEACTIONASSIGNEDNAME"].ToString();
            txtImmediateAssignedToRank.Text = dr["FLDIMMEDIATEACTIONASSIGNEDDESIGNATION"].ToString();

            txtReportedByShipname1.Text = dr["FLDREPORTEDBYSHIPNAME1"].ToString();
            txtReportedByShipname2.Text = dr["FLDREPORTEDBYSHIPNAME2"].ToString();
            txtReportedByShipname3.Text = dr["FLDREPORTEDBYSHIPNAME3"].ToString();

            txtReportedByShipId1.Text = dr["FLDREPORTEDBYSHIPID1"].ToString();
            txtReportedByShipId2.Text = dr["FLDREPORTEDBYSHIPID2"].ToString();
            txtReportedByShipId3.Text = dr["FLDREPORTEDBYSHIPID3"].ToString();

            txtReportedByShipRank1.Text = dr["FLDREPORTEDBYSHIPDESIGNATION1"].ToString();
            txtReportedByShipRank2.Text = dr["FLDREPORTEDBYSHIPDESIGNATION2"].ToString();
            txtReportedByShipRank3.Text = dr["FLDREPORTEDBYSHIPDESIGNATION3"].ToString();

            ucVentilation.SelectedQuick = dr["FLDVENTILATIONYN"].ToString();
            ucLightingCond.SelectedQuick = dr["FLDLIGHTINGCONDITION"].ToString();
            if (dr["FLDISCONTRACTORRELATEDINCIDENT"].ToString() == "1")
                chkContractorIncident.Checked = true;
            else
                chkContractorIncident.Checked = false;
            txtContractorDetails.Text = dr["FLDCONTRACTORDETAILS"].ToString();
            ChangeStatus();

            if (dr["FLDDRUGALCOHOLTESTYN"].ToString() == "1")
                chkAlcoholtest.Checked = true;
            else
                chkAlcoholtest.Checked = false;

            txttestDate.Text = General.GetDateTimeToString(dr["FLDALCOHOLTESTDATE"].ToString());

            if (dr["FLDMAINTENANCEREQUIRED"].ToString() == "1")
                chkMaintenanceRequired.Checked = true;
            else
                chkMaintenanceRequired.Checked = false;

            txtWorkOrderNumber.Text = dr["FLDWORKORDERTEXT"].ToString();
            
            imgPersonInCharge.Visible = false;
            imgReportedByShip1.Visible = false;
            imgReportedByShip2.Visible = false;
            imgReportedByShip3.Visible = false;            
        }
    }   

    protected void Activity_Changed(object sender, EventArgs e)
    {
        try
        {
            UserControlHard ucActivity = (UserControlHard)sender;

            if (ucActivity.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 170, "OTH"))
            {
                txtOtherActivity.CssClass = "input_mandatory";
                txtOtherActivity.ReadOnly = false;
            }
            else
            {
                txtOtherActivity.CssClass = "readonlytextbox";
                txtOtherActivity.ReadOnly = true;
                txtOtherActivity.Text = "";
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void Reset()
    {
        ucVessel.Enabled = true;
        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0 && Filter.CurrentVesselConfiguration != null && Filter.CurrentVesselConfiguration.ToString().Equals("0"))
        {
            ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
            ucVessel.Enabled = false;
        }
        else
        {
            ucVessel.SelectedVessel = Filter.CurrentVesselConfiguration != null ? Filter.CurrentVesselConfiguration.ToString() : "";            
        }

        txtRefNo.Text = "";
        txtTitle.Text = "";
        txtDateOfIncident.Text = "";
        txtTimeOfIncident.Text = "";
        txtReportedbyDesignation.Text = "";
        txtReportedByShipId.Text = "";
        txtReportedByShipName.Text = "";
        txtDescription.Text = "";
        //txtWindCondition.Text = "";
        txtSeaCondition.Text = "";
        //txtWeather.Text = "";
        ucOnboardLocation.SelectedQuick = "";
        ucWindCondionScale.SelectedQuick = "";
        txtWindDirection.Text = "";
        ucSwellLength.SelectedQuick = "";
        ucSwellHeight.SelectedQuick = "";
        txtSwellDirection.Text = "";
        txtDateOfReport.Text = "";
        ucConsequenceCategory.SelectedHard = "";
        ucPotentialCategory.SelectedHard = "";
        ucActivity.SelectedHard = "";
        txtOtherActivity.Text = "";  
        txtCrewName.Text = "";
        txtCrewId.Text = "";
        //txtComprehensiveDescription.Text = "";
        txtCrewRank.Text = "";
       
        txtOtherActivity.CssClass = "readonlytextbox";
        txtOtherActivity.ReadOnly = true;

        ucPort.SelectedSeaport = "";
        ucLatitude.Clear();        
        ucLongitude.Clear();
        ucLatitude.TextSecond = "00";
        ucLongitude.TextSecond = "00";
        ucQuickVesselActivity.SelectedQuick = "";
        txtCurrent.Text = "";
        txtCurrent.CssClass = "input";
        txtVisibility.Text = "";
        txtVisibility.CssClass = "input";
        rblIncidentNearmiss.SelectedValue = "1";
    } 
   
    protected void ucConsequenceCategory_Changed(object sender, EventArgs e)
    {
        
    }

    protected void btnComplete_Click(object sender, EventArgs e)
    {
        
    }    

    protected void ucVessel_Changed(object sender, EventArgs e)
    {

    }    

    protected void rblIncidentNearmiss_Changed(object sender, EventArgs e)
    {
        ucCategory.TypeId = rblIncidentNearmiss.SelectedValue;
        ucCategory.CategoryList = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(General.GetNullableInteger(rblIncidentNearmiss.SelectedValue));
        ucCategory.DataBind();

        ucSubcategory.CategoryId = ucCategory.SelectedCategory;
        ucSubcategory.SubCategoryList = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(ucCategory.SelectedCategory));
        ucSubcategory.DataBind(); 

        if (rblIncidentNearmiss.SelectedValue == "1")
        { 
            ucCategory.CssClass = "input_mandatory";
            ucSubcategory.CssClass = "input_mandatory";
            ucConsequenceCategory.Enabled = true;
            if(General.GetNullableInteger(ucPotentialCategory.SelectedHard)!=null)
                ucConsequenceCategory.SelectedHard = ucPotentialCategory.SelectedHard;
            ucPotentialCategory.Enabled = false;
            ucPotentialCategory.SelectedHard = "";            
        }
        if (rblIncidentNearmiss.SelectedValue == "2" || rblIncidentNearmiss.SelectedValue == "3")
        { 
            //ucCategory.CssClass = "input";
            //ucSubcategory.CssClass = "input";
            if(General.GetNullableInteger(ucConsequenceCategory.SelectedHard)!=null)
                ucPotentialCategory.SelectedHard = ucConsequenceCategory.SelectedHard;
            ucConsequenceCategory.SelectedHard = "";
            ucConsequenceCategory.Enabled = false;
            ucPotentialCategory.Enabled = true;
            ucConsequenceCategory.SelectedHard = "";
        }
    }

    protected void ucCategory_Changed(object sender, EventArgs e)
    {
        ucSubcategory.CategoryId = ucCategory.SelectedCategory;
        ucSubcategory.SubCategoryList = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(ucCategory.SelectedCategory));
        ucSubcategory.DataBind();
    }

    protected void chkContractorIncident_CheckedChanged(object sender, EventArgs e)
    {
        ChangeStatus();
    }
    private void ChangeStatus()
    {
        if (chkContractorIncident.Checked == true)
        {
            txtContractorDetails.CssClass = "input_mandatory";
            txtContractorDetails.Enabled = true;
        }
        else
        {
            txtContractorDetails.CssClass = "input";
            txtContractorDetails.Enabled = false;
            txtContractorDetails.Text = "";
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }   

    private void BindData()
    {
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionIncident.IncidentFindingsSearch(
                      ViewState["INCIDENTID"] != null ? General.GetNullableGuid(ViewState["INCIDENTID"].ToString()) : General.GetNullableGuid(null)
                    , sortexpression
                    , sortdirection);

        //General.SetPrintOptions("gvFindings", "Inspection Review Program", alCaptions, alColumns, ds);
        if (ds.Tables[0].Rows.Count > 0)
        {
            //gvFindings.DataSource = ds;
            //gvFindings.DataBind();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            //ShowNoRecordsFound(dt, gvFindings);
        }
        gvFindings.DataSource = ds;
    }

    protected void gvFindings_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = int.Parse(e.CommandArgument.ToString());            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvFindings_Sorting(object sender, GridViewSortEventArgs e)
    {
        //gvFindings.EditIndex = -1;
        //gvFindings.SelectedIndex = -1;
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void gvFindings_ItemDataBound(object sender, GridItemEventArgs e)
    {
    
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindInvestigation()
    {
        DataTable dt = PhoenixInspectionIncident.ListInvestigation(new Guid(ViewState["INCIDENTID"].ToString()));

        //General.SetPrintOptions("gvFindings", "Inspection Review Program", alCaptions, alColumns, ds);
        if (dt.Rows.Count > 0)
        {
            //gvInvestigation.DataSource = dt;
            //gvInvestigation.DataBind();
        }
        else
        {
           // ShowNoRecordsFound(dt, gvInvestigation);
        }
        gvInvestigation.DataSource = dt;
    }

    protected void gvInvestigation_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvInvestigation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView dr = (DataRowView)e.Item.DataItem;
            RadRadioButtonList rblAnswer = (RadRadioButtonList)e.Item.FindControl("rblAnswer");
            if (rblAnswer != null)
            {
                rblAnswer.DataSource = PhoenixInspectionIncident.ListInspectionAnswer(1, 36, 1, "YES,NO,NA");
                rblAnswer.DataBindings.DataTextField = "FLDHARDNAME";
                rblAnswer.DataBindings.DataValueField = "FLDHARDCODE";
                rblAnswer.DataBind();
                rblAnswer.SelectedValue = dr["FLDANSWER"].ToString();
            }
        }        
    }

    public void rblAnswer_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            BindInvestigation();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            BindInvestigation();
        }
    }
}
