using System;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Data;
using Telerik.Web.UI;
using SouthNests.Phoenix.Integration;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;


public partial class Inspection_InspectionPSCMOUShipRiskProfile : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        //toolbar.AddButton("Calculate", "GO", ToolBarDirection.Right);
        MenuShipRiskProfile.AccessRights = this.ViewState;
        MenuShipRiskProfile.MenuList = toolbar.Show();

        ucClassName.AddressType = ((int)PhoenixAddressType.CLASSIFICATIONSOCIETY).ToString();

        if (!IsPostBack)
        {
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            if (Request.QueryString["Vesselid"] != null)
                ViewState["Vesselid"] = Request.QueryString["Vesselid"].ToString();
            else
                ViewState["Vesselid"] = "";

            if (Request.QueryString["Portid"] != null)
                ViewState["Portid"] = Request.QueryString["Portid"].ToString();
            else
                ViewState["Portid"] = "";
            
            if (Request.QueryString["Portname"] != null)
                ViewState["Portname"] = Request.QueryString["Portname"].ToString();
            else
                ViewState["Portname"] = "";

            ViewState["COMPANYID"] = "";            
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
                ucVessel.Company = nvc.Get("QMS");
                ucVessel.bind();
            }

            BindPSCMOU();
            FlagPerformance();
            ClassPerformance();
            BindExternalOrganization();
            CompanyPerformance();

            if(ViewState["Vesselid"].ToString() != "" && ViewState["Portid"].ToString() != "")
            {
                 BindShipRiskProfile();
                ucVessel.SelectedValue = int.Parse(ViewState["Vesselid"].ToString());
                ucFromPort.SelectedValue = ViewState["Portid"].ToString();
                ucFromPort.Text = ViewState["Portname"].ToString();
                ucVessel.Enabled = false;
                ucFromPort.Enabled = false;
                DisableControls();
            }

        }
       
    }
    
    protected void BindShipRiskProfile()
    {
        DataSet ds = PhoenixInspectionPSCMOUMatrix.ShipRiskProfileEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                    (ViewState["Portid"].ToString() != "" ? General.GetNullableInteger(ViewState["Portid"].ToString()) : General.GetNullableInteger(ucFromPort.SelectedValue)),
                                                                    ViewState["Vesselid"].ToString() != "" ? General.GetNullableInteger(ViewState["Vesselid"].ToString()) : General.GetNullableInteger(ucVessel.SelectedVessel));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            ddlCompany.SelectedValue = dr["FLDPSCMOU"].ToString();
            ucVesselType.SelectedHard = dr["FLDSHIPTYPE"].ToString();
            ddlshipage.SelectedValue = dr["FLDSHIPAGE"].ToString();
            ddlflagperf.SelectedValue = dr["FLDFLAGPERFORMANCE"].ToString();
            rblIMOAudit.SelectedValue = dr["FLDFLAGISIMOAUDITED"].ToString();
            // rblcertissue.SelectedValue = dr["FLDFLAGISIMOAUDITED"].ToString();
            ucClassName.SelectedValue = dr["FLDCLASSID"].ToString();
            ucClassName.Text = dr["FLDCLASSNAME"].ToString();
            ddlclassperf.SelectedValue = dr["FLDCLASSPERFORMANCE"].ToString();
            rblEURecog.SelectedValue = dr["FLDISEURECOG"].ToString();
            ddlcompperf.SelectedValue = dr["FLDCOMPANYPERF"].ToString();
            rbloneinsp.SelectedValue = dr["FLDATLEASTONEINSP"].ToString();
            rbllessdef.SelectedValue = dr["FLDNOOFDEFICIENCIES"].ToString();
            rblnodetention.SelectedValue = dr["FLDNOOFDETENTION"].ToString();

            lblhrshiptype.Text = dr["FLDSHIPTYPEWEIGHTAGE"].ToString();
            lblhrshipage.Text = dr["FLDAGEWEIGHTAGE"].ToString();
            lblhrflagperf.Text = dr["FLDFLAGPERFWEIGHTAGE"].ToString();
            lblhrclassperf.Text = dr["FLDCLASSPERFWEIGHTAGE"].ToString();
            lblhrcompperf.Text = dr["FLDCOMPPERFWEIGHTAGE"].ToString();
               
            lbllrflagperf.Text = dr["FLDFLAGLOWRISK"].ToString();               //White 
            lbllrclassperf.Text = dr["FLDCLASSLOWRISK"].ToString();         //High
            lbllrcompperf.Text = dr["FLDCOMPANYLOWRISK"].ToString();        //High

            if (rblIMOAudit.SelectedValue == "1")
            {
                hrimoaudit.Text = "Not Applicable";
                lrimoaudit.Text = "Yes";
            }
            else
            {
                hrimoaudit.Text = "Not Applicable";
                lrimoaudit.Text = "No";
            }

            if (rblEURecog.SelectedValue == "1")
            {
                lblhrEURecog.Text = "Not Applicable";
                lbllrEURecog.Text = "Yes";
            }
            else
            {
                lblhrEURecog.Text = "Not Applicable";
                lbllrEURecog.Text = "No";
            }

            // Historic Parameter

             lblhroneinsp.Text = "Not applicable";                            //Atleast one inspection
             lbllroneinsp.Text = dr["FLDONEINSPLOWRISK"].ToString();

            lblhrlessdef.Text = dr["FLDHISTORICDEFWEIGHTAGE"].ToString();       //No of deficeineces
            lbllrlessdef.Text = dr["FLDDEFLOWRISK"].ToString();

            //No of detention

            rblnodetention.Items[2].Text = dr["FLDDETENTIONLIMIT"].ToString() + " or More";
            lblhrnodetention.Text = dr["FLDHISTORICDETWEIGHTAGE"].ToString();
            lbllrnodetention.Text = dr["FLDDETLOWRISK"].ToString();
            
            lbltotalscore.Text = dr["FLDTOTALWEIGHTAGE"].ToString();            //result
            lblhighriskscore.Text = dr["FLDHIGHRISKYN"].ToString(); 
            lbllowriskriskscore.Text = dr["FLDLOWRISK"].ToString(); 
            lblshipriskresult.Text = dr["FLDSRPRESULT"].ToString();            
            //ucFlag.SelectedFlag = dr["FLDFLAGID"].ToString();
            ucFlag.Text = dr["FLDFLAGNAME"].ToString();

        }
    }


    protected void CompanyPerformance()
    {
        ddlcompperf.DataSource = PhoenixInspectionPSCMOUMatrix.ListCompanyPerformanceList();
        ddlcompperf.DataTextField = "FLDNAME";
        ddlcompperf.DataValueField = "FLDCOMPANYPERFORMANCEID";
        ddlcompperf.DataBind();
        ddlcompperf.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindExternalOrganization()
    {
        //ddlExternalOrganization.DataSource = PhoenixInspectionOrganization.InspectionOrganizationList(General.GetNullableInteger(PhoenixCommonRegisters.GetHardCode(1, 144, "EXT")));
        //ddlExternalOrganization.DataTextField = "FLDORGANIZATIONNAME";
        //ddlExternalOrganization.DataValueField = "FLDORGANIZATIONID";
        //ddlExternalOrganization.DataBind();
        //ddlExternalOrganization.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void BindPSCMOU()
    {
        ddlCompany.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOURegion(PhoenixSecurityContext.CurrentSecurityContext.CompanyID);
        ddlCompany.DataTextField = "FLDCOMPANYNAME";
        ddlCompany.DataValueField = "FLDCOMPANYID";
        ddlCompany.DataBind();
        ddlCompany.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void FlagPerformance()
    {
        ddlflagperf.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUFlagPerformance();
        ddlflagperf.DataTextField = "FLDNAME";
        ddlflagperf.DataValueField = "FLDFLAGPERFORMANCEID";
        ddlflagperf.DataBind();
        ddlflagperf.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void ClassPerformance()
    {
        ddlclassperf.DataSource = PhoenixInspectionPSCMOUMatrix.ListPSCMOUClassPerformance();
        ddlclassperf.DataTextField = "FLDNAME";
        ddlclassperf.DataValueField = "FLDCLASSSOCIETYPERFORMANCEID";
        ddlclassperf.DataBind();
        ddlclassperf.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }

    protected void MenuShipRiskProfile_TabStripCommand(object sender, EventArgs e)
    {

    }



    protected void ucVessel_TextChangedEvent(object sender, EventArgs e)
    {
        BindShipRiskProfile();
        DisableControls();
    }

    protected void DisableControls()
    {
        ddlCompany.Enabled = false;
        ddlclassperf.Enabled = false;
        ddlcompperf.Enabled = false;
        ddlshipage.Enabled = false;
        ddlflagperf.Enabled = false;
        ucFlag.Enabled = false;
        ucVesselType.Enabled = false;
        rblIMOAudit.Enabled = false;
        rblEURecog.Enabled = false;
        ucClassName.Enabled = false;
        rbloneinsp.Enabled = false;
        rblnodetention.Enabled = false;
        rbllessdef.Enabled = false;

    }
}