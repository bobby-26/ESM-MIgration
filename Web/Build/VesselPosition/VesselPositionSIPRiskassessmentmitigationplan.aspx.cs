using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselPosition;
using System.Web;
using System.IO;
using System.Net;
using SouthNests.Phoenix.Integration;
using Telerik.Web.UI;

public partial class VesselPositionSIPRiskassessmentmitigationplan : PhoenixBasePage
{
    string tootip = "Ships are advised to assess potential impact on machinery systems with the use of distillates and fuel oil blends and prepare ships in consultation with chief engineers, equipment manufacturers and suppliers.";
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbartab = new PhoenixToolbar();

        bindToolTip();
        toolbartab.AddButton("Back", "BACK",ToolBarDirection.Right);
        toolbartab.AddFontAwesomeButton("", tootip , "<i class=\"fas fa-info-circle\"></i>", "TOOLTIP");
        toolbartab.AddFontAwesomeButton("", "Global Sulphur Cap 2020 – Managing the fuel switch", "<i class=\"fas fa-file-pdf\"></i>", "PDF");
        TabRiskassessmentplan.AccessRights = this.ViewState;
        TabRiskassessmentplan.MenuList = toolbartab.Show();

        PhoenixToolbar toolbabutton = new PhoenixToolbar();
        toolbabutton.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuRiskassessmentplan.AccessRights = this.ViewState;
        MenuRiskassessmentplan.MenuList = toolbabutton.Show();

        if (!IsPostBack)
        {

            ViewState["VESSELID"] = "";
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

            ViewState["DTKey"] = "";
            ViewState["COMPANYID"] = "";

            if (Request.QueryString["COMPANYID"] != null)
                ViewState["COMPANYID"] = Request.QueryString["COMPANYID"].ToString();

            ViewState["SIPCONFIGID"] = "";

            if (Request.QueryString["SIPCONFIGID"] != null)
                ViewState["SIPCONFIGID"] = Request.QueryString["SIPCONFIGID"].ToString();

            ViewState["SIPRISKASSESSMENTID"] = "";
            BindData();
           
            bindOfficeInstruction();

            imgShowRA.Attributes.Add("onclick",
                       "return showPickList('spnRA', 'codehelp1', '', '../Common/CommonPickListDailyWorkPlanRA.aspx?OPT=M&vesselid="
                       + ViewState["VESSELID"].ToString() + "', true); ");

            
        }
        txtRAId.CssClass = "hidden";
        txtRaType.CssClass = "hidden";
    }
    protected void cmdRAClear_Click(object sender, EventArgs e)
    {
        cmdRA.Visible = false;

        txtRANumber.Text = "";
        txtRA.Text = "";
        txtRAId.Text = "";
        txtRaType.Text = "";
    }

    private void bindToolTip()
    {
        try
        {
            DataSet ds = PhoenixRegistersSIPToolTipConfiguration.SIPToolTipConfigList(General.GetNullableInteger(Request.QueryString["SIPCONFIGID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                if (General.GetNullableString(ds.Tables[0].Rows[0]["FLDDISCRIPTION"].ToString()) != null)
                    tootip = ds.Tables[0].Rows[0]["FLDDISCRIPTION"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void bindOfficeInstruction()
    {
        DataSet ds = PhoenixVesselPositionSIPConfiguration.EditSIPfficeinstruction();
        if(ds.Tables[0].Rows.Count>0)
        {
            txtOfficeDescription.Text = ds.Tables[0].Rows[0]["FLDRISKASSESSMENT"].ToString();
        }
    }
    private void BindData()
    {
        DataSet ds = PhoenixVesselPositionSIPriskassessmentmitigationplan.SIPriskassessmentmitigationplanEdit(General.GetNullableInteger(UcVessel.SelectedVessel));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            //chkriskassest.Checked = dr["FLDIMPACTPERFORMEDYN"].ToString() == "1" ? true : false;
            rdriskassesment.SelectedValue = dr["FLDIMPACTPERFORMEDYN"].ToString();
            txtDescription.Text = dr["FLDDESCRIPTION"].ToString();
            // chklinkedyn.Checked = dr["FLDLINKEDTOONBOARDSAFTYYN"].ToString() == "1" ? true : false;
            rdlinkedyn.SelectedValue = dr["FLDLINKEDTOONBOARDSAFTYYN"].ToString();
            ViewState["SIPRISKASSESSMENTID"] = dr["FLDSIPRISKASSESSMENTID"].ToString();

            ViewState["DTKey"] = dr["FLDDTKEY"].ToString();

            if (General.GetNullableGuid(dr["FLDNONROUTINERAID"].ToString()) != null)
            {
                cmdRA.Visible = true;
                cmdRA.Attributes.Add("onclick", "parent.openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/Reports/ReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAMACHINERY&machineryid=" + dr["FLDNONROUTINERAID"].ToString() + "&showmenu=0&showexcel=NO');return false;");
                txtRA.Text = dr["FLDRISKASSESMENT"].ToString();
                txtRANumber.Text = dr["FLDREFERENCENO"].ToString();
                txtRAId.Text = dr["FLDNONROUTINERAID"].ToString();
                txtRaType.Text = dr["FLDRISKASSEMENTTYPE"].ToString();
            }
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
                if (General.GetNullableGuid(ViewState["SIPRISKASSESSMENTID"].ToString()) == null)
                {
                    Guid? sipriskassessid = null;
                    PhoenixVesselPositionSIPriskassessmentmitigationplan.InsertSIPriskassessmentmitigationplan(
                        General.GetNullableInteger(UcVessel.SelectedValue.ToString())
                        , int.Parse(rdriskassesment.SelectedValue)//chkriskassest.Checked == true ? 1 : 0
                        , General.GetNullableString(txtDescription.Text.Trim())
                        , General.GetNullableString("")
                        , int.Parse(rdlinkedyn.SelectedValue)//chklinkedyn.Checked == true ? 1 : 0
                        , General.GetNullableGuid("")
                        , ref sipriskassessid
                        , General.GetNullableGuid(txtRAId.Text)
                        );
                    ViewState["SIPRISKASSESSMENTID"] = sipriskassessid;
                    BindData();
                }
                else
                {
                    PhoenixVesselPositionSIPriskassessmentmitigationplan.UpdateSIPriskassessmentmitigationplan(
                       General.GetNullableGuid(ViewState["SIPRISKASSESSMENTID"].ToString())
                       , int.Parse(rdriskassesment.SelectedValue)//chkriskassest.Checked == true ? 1 : 0
                       , General.GetNullableString(txtDescription.Text.Trim())
                       , General.GetNullableString("")
                       , int.Parse(rdlinkedyn.SelectedValue)//chklinkedyn.Checked == true ? 1 : 0
                       , General.GetNullableGuid("")
                       , General.GetNullableGuid(txtRAId.Text)
                       );
                    BindData();
                }
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
            else if (CommandName.ToUpper().Equals("PDF"))
            {
                string filePath = Server.MapPath("~/Template/VesselPosition/DNVGL_Global sulphur Cap 2020_Guidance_2019-03.pdf");
                Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=pdf");
            }
            else if (CommandName.ToUpper().Equals("TOOLTIP"))
            {
                
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void imgPdf_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = Server.MapPath("~/Template/VesselPosition/DNVGL_Global sulphur Cap 2020_Guidance_2019-03.pdf");
        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=pdf");
    }

    protected void lnkRiskAssesment_Click(object sender, EventArgs e)
    {
        BindData();
        if (ViewState["DTKey"] != null)
        {
            String script = "parent.Openpopup('codehelp1','','../Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKey"] + "&mod="
                            + PhoenixModule.VESSELPOSITION + "&type=SIPRISKASSESMENT')";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
        }
    }
    protected void linkOnboardSafty_Click(object sender, EventArgs e)
    {
        BindData();
        if (ViewState["DTKey"] != null)
        {
            String script = "parent.openNewWindow('codehelp1','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKey"] + "&mod="
                            + PhoenixModule.VESSELPOSITION + "&type=SIPONBOARDSAFTY')";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
        }
    }
}