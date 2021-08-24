using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Integration;
using Telerik.Web.UI;

public partial class VesselPositionSIPProcurementofCompliantFuel : PhoenixBasePage
{
    string tootip = "The fuel oil purchaser should ensure that the fuel oil ordered is correctly specified considering the ship's known technical capabilities and intended area of operation. Bunker specifications and any requirements for bunkering procedures should be stated in the contract. Should a vessel, despite its best effort to obtain compliant fuel oil, be unable to do so, the master/Company must present a record of actions taken to attempt to bunker correct fuel oil and provide evidence of an attempt to purchase compliant fuel oil in accordance with its voyage plan and, if it was not made available where planned, that attempts were made to locate alternative sources for such fuel oil and that despite best efforts to obtain compliant fuel oil, no such fuel oil was made available for purchase. Best efforts to procure compliant fuel oil include, but are not limited to, investigating alternate sources of fuel oil prior to commencing the voyage. If, despite best efforts, it was not possible to procure compliant fuel oil, the master/owner must immediately notify the port State Administration of the port of destination and its flag Administration, FONAR (regulation 18.2.4 of MARPOL Annex VI)";
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        bindToolTip();

        PhoenixToolbar toolbartab = new PhoenixToolbar();
        toolbartab.AddFontAwesomeButton("", tootip, "<i class=\"fas fa-info-circle\"></i>", "TOOLTIP");
        toolbartab.AddFontAwesomeButton("", "IMO Guidance on Best Practice for Fuel Oil Purchasers/Users", "<i class=\"fas fa-file-pdf\"></i>", "PDF");
        toolbartab.AddButton("Back", "BACK",ToolBarDirection.Right);
        TabRiskassessmentplan.AccessRights = this.ViewState;
        TabRiskassessmentplan.MenuList = toolbartab.Show();

        PhoenixToolbar toolbabutton = new PhoenixToolbar();
        toolbabutton.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuRiskassessmentplan.AccessRights = this.ViewState;
        MenuRiskassessmentplan.MenuList = toolbabutton.Show();

        if (!IsPostBack)
        {
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

            ViewState["COMPANYID"] = "";
            ViewState["SIPCONFIGID"] = "";
            ViewState["DTKey"] = "";

            if (Request.QueryString["COMPANYID"] != null)
                ViewState["COMPANYID"] = Request.QueryString["COMPANYID"].ToString();

            if (Request.QueryString["SIPCONFIGID"] != null)
                ViewState["SIPCONFIGID"] = Request.QueryString["SIPCONFIGID"].ToString();

            ViewState["SIPCOMPLIENTFUELID"] = "";

            BindBunkerRegion();

            BindData();
        }

        bindOfficeInstruction();

    }
    private void bindOfficeInstruction()
    {
        DataSet ds = PhoenixVesselPositionSIPConfiguration.EditSIPfficeinstruction();
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtOfficepurchasedetail.Text = ds.Tables[0].Rows[0]["FLDFUELPURCHASE"].ToString();
            txtOfficealternatestepdetails.Text = ds.Tables[0].Rows[0]["FLDALTERNATESTEP"].ToString();
            txtOfficealternetstepavailablity.Text = ds.Tables[0].Rows[0]["FLDCOMPLIEDTFUEL"].ToString();
            txtOfficedisposedetials.Text = ds.Tables[0].Rows[0]["FLDNONCOMPLIENTFUEL"].ToString();
        }
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
    private void BindBunkerRegion()
    {
        DataTable dt = PhoenixRegistersRegion.RegionList();
        ddlDisposeRegion.DataSource = dt;
        ddlDisposeRegion.DataTextField = "FLDREGIONNAME";
        ddlDisposeRegion.DataValueField = "FLDREGIONID";
        ddlDisposeRegion.DataBind();
        ddlDisposeRegion.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    private void BindBunkerCountry()
    {
        DataTable dt = PhoenixRegistersRegion.ListRegion(General.GetNullableInteger(ViewState["Regionid"].ToString()));
        ddlDisposeCountry.DataSource = dt;
        ddlDisposeCountry.DataTextField = "FLDCOUNTRYNAME";
        ddlDisposeCountry.DataValueField = "FLDCOUNTRYCODE";
        ddlDisposeCountry.DataBind();
        ddlDisposeCountry.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    private void BindBunkerPort()
    {
        int? countryid = General.GetNullableInteger(ViewState["Countryid"].ToString()) == null ? 0 : General.GetNullableInteger(ViewState["Countryid"].ToString());
        DataSet ds = PhoenixRegistersSeaport.ListCountrySeaport(countryid);
        ddlDisposePort.DataSource = ds;
        ddlDisposePort.DataTextField = "FLDSEAPORTNAME";
        ddlDisposePort.DataValueField = "FLDSEAPORTID";
        ddlDisposePort.DataBind();
        ddlDisposePort.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    protected void lblDisposeRegion_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Regionid"] = ddlDisposeRegion.SelectedValue;
        BindBunkerCountry();
        BindBunkerPort();
    }
    protected void ddlDisposeCountry_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Countryid"] = ddlDisposeCountry.SelectedValue;
        BindBunkerPort();
    }
    private void BindData()
    {
        DataSet ds = PhoenixVesselPositionSIPProcurementofCompliantFuel.SIPFueloilsystemmodificationsDetailEdit(General.GetNullableInteger(ViewState["VESSELID"].ToString()));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ViewState["Regionid"] = dr["FLDDISPOSEREGION"].ToString();
            ViewState["Countryid"] = dr["FLDDISPOSECOUNTRY"].ToString();

            BindBunkerCountry();
            BindBunkerPort();

            txtpurchasedetail.Text= dr["FLDFUELPURCHASEDETAIL"].ToString();
            UcFirstbunkeringdate.Text = dr["FLDFIRSTBUNKERDATE"].ToString();
            //chkchartererresponsibleyn.Checked = dr["FLDISCHARTERERESPONSIBLE"].ToString() == "1" ? true : false;
            rdchartererresponsibleyn.SelectedValue = dr["FLDISCHARTERERESPONSIBLE"].ToString();
            //chkacceprchartereyn.Checked = dr["FLDACCEPTCONTRACT"].ToString() == "1" ? true : false;
            rdacceprchartereyn.SelectedValue = dr["FLDACCEPTCONTRACT"].ToString();
            txtalternatestepdetails.Text= dr["FLDALTERNATESTEPDETAIL"].ToString();
            //chkconfirmfromsupplier.Checked= dr["FLDCONFIRMEDFROMSUPLIER"].ToString() == "1" ? true : false;
            rdconfirmfromsupplier.SelectedValue = dr["FLDCONFIRMEDFROMSUPLIER"].ToString();
            txtalternetstepavailablity.Text= dr["FLDALTERNATEFUELDETAIL"].ToString();
            UcDisposeDate.Text = dr["FLDFUELDISPOSEDATE"].ToString();
            ddlDisposeRegion.SelectedValue = General.GetNullableInteger(dr["FLDDISPOSEREGION"].ToString()) == null ? "Dummy" : dr["FLDDISPOSEREGION"].ToString();
            ddlDisposeCountry.SelectedValue = General.GetNullableInteger(dr["FLDDISPOSECOUNTRY"].ToString()) == null ? "Dummy" : dr["FLDDISPOSECOUNTRY"].ToString();
            ddlDisposePort.SelectedValue = General.GetNullableInteger(dr["FLDDISPOSEPORT"].ToString()) == null ? "Dummy" : dr["FLDDISPOSEPORT"].ToString(); 
            txtdisposedetials.Text = dr["FLDDISPOSEDETAILS"].ToString();

            ViewState["DTKey"] = dr["FLDDTKEY"].ToString();
            ViewState["SIPCOMPLIENTFUELID"] = dr["FLDSIPCOMPLIENTFUELID"].ToString();
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
                if (General.GetNullableGuid(ViewState["SIPCOMPLIENTFUELID"].ToString()) == null)
                {
                    Guid? sipcomplientfuelidid = null;
                    PhoenixVesselPositionSIPProcurementofCompliantFuel.InsertSIPProcurementofCompliantFuel(
                        ref sipcomplientfuelidid
                        , General.GetNullableInteger(UcVessel.SelectedVessel)
                        ,General.GetNullableString(txtpurchasedetail.Text)
                        ,General.GetNullableDateTime(UcFirstbunkeringdate.Text)
                        ,int.Parse(rdchartererresponsibleyn.SelectedValue)// chkchartererresponsibleyn.Checked==true? 1:0
                        , int.Parse(rdacceprchartereyn.SelectedValue)//chkacceprchartereyn.Checked==true?1:0
                        ,General.GetNullableString(txtalternatestepdetails.Text)
                        , int.Parse(rdconfirmfromsupplier.SelectedValue)//chkconfirmfromsupplier.Checked==true?1:0
                        ,General.GetNullableString(txtalternetstepavailablity.Text)
                        ,General.GetNullableDateTime(UcDisposeDate.Text)
                        ,General.GetNullableInteger(ddlDisposeRegion.SelectedValue)
                        ,General.GetNullableInteger(ddlDisposeCountry.SelectedValue)
                        ,General.GetNullableInteger(ddlDisposePort.SelectedValue)
                        ,General.GetNullableString(txtdisposedetials.Text)
                        );
                    ViewState["SIPCOMPLIENTFUELID"] = sipcomplientfuelidid;
                }
                else
                {
                    PhoenixVesselPositionSIPProcurementofCompliantFuel.UpdateSIPProcurementofCompliantFuel(
                        General.GetNullableGuid(ViewState["SIPCOMPLIENTFUELID"].ToString())
                        , General.GetNullableInteger(UcVessel.SelectedVessel)
                        , General.GetNullableString(txtpurchasedetail.Text)
                        , General.GetNullableDateTime(UcFirstbunkeringdate.Text)
                        , int.Parse(rdchartererresponsibleyn.SelectedValue)//chkchartererresponsibleyn.Checked == true ? 1 : 0
                        , int.Parse(rdacceprchartereyn.SelectedValue)//chkacceprchartereyn.Checked == true ? 1 : 0
                        , General.GetNullableString(txtalternatestepdetails.Text)
                        , int.Parse(rdconfirmfromsupplier.SelectedValue)//chkconfirmfromsupplier.Checked == true ? 1 : 0
                        , General.GetNullableString(txtalternetstepavailablity.Text)
                        , General.GetNullableDateTime(UcDisposeDate.Text)
                        , General.GetNullableInteger(ddlDisposeRegion.SelectedValue)
                        , General.GetNullableInteger(ddlDisposeCountry.SelectedValue)
                        , General.GetNullableInteger(ddlDisposePort.SelectedValue)
                        , General.GetNullableString(txtdisposedetials.Text)
                        );
                }
                BindData();
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
                string filePath = Server.MapPath("~/Template/VesselPosition/MEPC.1-Circ.875 - Guidance On Best Practice For Fuel Oil PurchasersUsers....pdf");
                Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=pdf");
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
        string filePath = Server.MapPath("~/Template/VesselPosition/MEPC.1-Circ.875 - Guidance On Best Practice For Fuel Oil PurchasersUsers....pdf");
        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=pdf");
    }

    protected void chkchartererresponsibleyn_CheckedChanged(object sender, EventArgs e)
    {
        //if (chkchartererresponsibleyn.Checked == true)
        //{
        //    trconfirmationsupplier.Visible = false;
        //    tracceptcharterer.Visible = true;
        //}
        //else
        //{
        //    trconfirmationsupplier.Visible = true;
        //    tracceptcharterer.Visible = false;
        //}

    }
    protected void chkacceprchartereyn_CheckedChanged(object sender, EventArgs e)
    {
        //if (chkchartererresponsibleyn.Checked == true)
        //{
        //    trconfirmationsupplier.Visible = false;
        //    tracceptcharterer.Visible = true;
        //    lblalternatestepdetails.Text = "Details of alternate steps taken:";
        //    trconfirmationsupplier.Visible = false;
        //    // chkconfirmfromsupplier.Checked = false;
        //}
        //else
        //{
        //    trconfirmationsupplier.Visible = true;
        //    tracceptcharterer.Visible = false;
        //    lblalternatestepdetails.Text = "Details of alternate steps taken to ensure timely availability of compliant fuel oil:";
        //    trconfirmationsupplier.Visible = true;
        //    //chkacceprchartereyn.Checked = false;
        //}
    }

    protected void chkconfirmfromsupplier_CheckedChanged(object sender, EventArgs e)
    {
        //if (chkconfirmfromsupplier.Checked == true)
        //{
        //    tralternatedetailHead.Visible = false;
        //    tralternatedetail.Visible = false;
        //}
        //else
        //{
        //    tralternatedetailHead.Visible = true;
        //    tralternatedetail.Visible = true;
        //}
    }
    protected void lnkPurchasedetails_Click(object sender, EventArgs e)
    {
        BindData();
        if (ViewState["DTKey"] != null)
        {
            String script = "parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKey"] + "&mod="
                            + PhoenixModule.VESSELPOSITION + "&type=SIPFUELMODIFICATION')";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
        }
    }
    protected void lnkAlternatestep_Click(object sender, EventArgs e)
    {
        BindData();
        if (ViewState["DTKey"] != null)
        {
            String script = "parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKey"] + "&mod="
                            + PhoenixModule.VESSELPOSITION + "&type=SIPALTERNATESTEP')";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
        }
    }
    protected void linkArrangements_Click(object sender, EventArgs e)
    {
        BindData();
        if (ViewState["DTKey"] != null)
        {
            String script = "parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKey"] + "&mod="
                            + PhoenixModule.VESSELPOSITION + "&type=SIPAARANGEMENTS')";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
        }
    }
    protected void LinkComplientFuel_Click(object sender, EventArgs e)
    {
        BindData();
        if (ViewState["DTKey"] != null)
        {
            String script = "parent.openNewWindow('codehelp1','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKey"] + "&mod="
                            + PhoenixModule.VESSELPOSITION + "&type=SIPCOMPLIENTFUEL')";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
        }
    }
}