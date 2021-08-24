using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.VesselPosition;
using Telerik.Web.UI;

public partial class VesselPositionSIPFuelSystemOilModification : PhoenixBasePage
{
    string tootip = "The ship tank configuration and fuel system may require adjustments. A fully segregated fuel system for distillate fuels and blended fuels is recommended because they may require special attention. Ship tank configuration and segregated fuel system will also allow for better management of potentially incompatible fuels.";
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenSubmit.Attributes.Add("style", "display: none;");

        SessionUtil.PageAccessRights(this.ViewState);
        bindToolTip();

        PhoenixToolbar toolbartab = new PhoenixToolbar();
        toolbartab.AddFontAwesomeButton("", tootip, "<i class=\"fas fa-info-circle\"></i>", "TOOLTIP");
        toolbartab.AddFontAwesomeButton("", "IMO Guidance – Impact on Machinery Systems", "<i class=\"fas fa-file-pdf\"></i>", "PDF");
        toolbartab.AddButton("Back", "BACK",ToolBarDirection.Right);
        TabFuelSystemMofify.AccessRights = this.ViewState;
        TabFuelSystemMofify.MenuList = toolbartab.Show();

        PhoenixToolbar toolbabutton = new PhoenixToolbar();
        toolbabutton.AddButton("Save", "SAVE",ToolBarDirection.Right);
        MenuFuelSystemMofify.AccessRights = this.ViewState;
        MenuFuelSystemMofify.MenuList = toolbabutton.Show();

        if (!IsPostBack)
        {
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
            {
                UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                UcVessel.Enabled = false;
            }
            else
            {
                UcVessel.SelectedVessel = Request.QueryString["VESSELID"];
                UcVessel.Enabled = false;
            }
            UcVessel.DataBind();
            UcVessel.bind();
            ViewState["SIPFOSYSTEMMODIFYETAILID"] = "";
            ViewState["PAGENUMBER"] = 1;
            ViewState["PAGENUMBERCLASS"] = 1;

            ViewState["Regionid"] = "";
            ViewState["ScrubberRegionid"] = "";
            ViewState["DTKey"] = "";

            BindType();
            BindRegion();
            BindScrubberRegion();
           

            EditData();
            bindOfficeInstruction();
            gvMeetingClass.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            gvMeetingManufact.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
    private void bindOfficeInstruction()
    {
        DataSet ds = PhoenixVesselPositionSIPConfiguration.EditSIPfficeinstruction();
        if (ds.Tables[0].Rows.Count > 0)
        {
            txtOfficeDescription.Text = ds.Tables[0].Rows[0]["FLDSTRUCTURALMODIFY"].ToString();
            txtScruberOfficeDiscription.Text = ds.Tables[0].Rows[0]["FLDSCRUBBER"].ToString();
            txtOfficededicatedFOSample.Text = ds.Tables[0].Rows[0]["FLDBUNKERCOMMENT"].ToString();
        }
    }
    private void BindType()
    {
        DataSet ds = PhoenixRegistersEUMRVDeterminationofdestination.ListEVMRVDeterminationCategory(11);
        ddlType.DataSource = ds;
        ddlType.DataTextField = "FLDNAME";
        ddlType.DataValueField = "FLDEUMRVCATEGORIESID";
        ddlType.DataBind();
        //ddlMethod.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    private void BindRegion()
    {
        DataTable dt = PhoenixRegistersRegion.RegionList();
        ddlregion.DataSource = dt;
        ddlregion.DataTextField = "FLDREGIONNAME";
        ddlregion.DataValueField = "FLDREGIONID";
        ddlregion.DataBind();
        ddlregion.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void BindScrubberRegion()
    {
        DataTable dt = PhoenixRegistersRegion.RegionList();
        ddlScruberregion.DataSource = dt;
        ddlScruberregion.DataTextField = "FLDREGIONNAME";
        ddlScruberregion.DataValueField = "FLDREGIONID";
        ddlScruberregion.DataBind();
        ddlScruberregion.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ddlregion_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["Regionid"] = ddlregion.SelectedValue;
        BindCountry();
    }
    private void BindCountry()
    {
        DataTable dt = PhoenixRegistersRegion.ListRegion(General.GetNullableInteger(ViewState["Regionid"].ToString()));
        ddlCountry.DataSource = dt;
        ddlCountry.DataTextField = "FLDCOUNTRYNAME";
        ddlCountry.DataValueField = "FLDCOUNTRYCODE";
        ddlCountry.DataBind();
        ddlCountry.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    protected void ddlScruberregion_SelectedIndexChanged(object sender, EventArgs e)
    {
        ViewState["ScrubberRegionid"] = ddlScruberregion.SelectedValue;
        BindScrubberCountry();
    }
    private void BindScrubberCountry()
    {
        DataTable dt = PhoenixRegistersRegion.ListRegion(General.GetNullableInteger(ViewState["ScrubberRegionid"].ToString()));
        ddlScruberCountry.DataSource = dt;
        ddlScruberCountry.DataTextField = "FLDCOUNTRYNAME";
        ddlScruberCountry.DataValueField = "FLDCOUNTRYCODE";
        ddlScruberCountry.DataBind();
        ddlScruberCountry.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
    }
    private void EditData()
    {
        DataSet ds = PhoenixVesselPositionSIPFuelOilSystemModification.SIPFueloilsystemmodificationsDetailEdit(General.GetNullableInteger(UcVessel.SelectedVessel));

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ViewState["Regionid"] = dr["FLDREGIONID"].ToString();
            ViewState["ScrubberRegionid"] = dr["FLDSCRBREGIONID"].ToString();

            BindCountry();
            BindScrubberCountry();

            //chkFuelMofifyreqyn.Checked = dr["FLDSTRUCTUREMODIFYYN"].ToString() == "1" ? true : false;
            //chkscriberwillinstallyn.Checked = dr["FLDSCRUBBERWILLBEINSTALLED"].ToString() == "1" ? true : false;
            rdFuelMofifyreqyn.SelectedValue= dr["FLDSTRUCTUREMODIFYYN"].ToString();
            rdscriberwillinstallyn.SelectedValue = dr["FLDSCRUBBERWILLBEINSTALLED"].ToString();

            ViewState["SIPFOSYSTEMMODIFYETAILID"] = dr["FLDSIPFOSYSTEMMODIFYETAILID"].ToString();

            //ddlType.SelectedValue = dr["FLDMODIFICATIONTYPE"].ToString();
            UcstartDate.Text = dr["FLDSTARTDATE"].ToString();
            UcEndDate.Text = dr["FLDENDDATE"].ToString();
            //ddlregion.SelectedValue = dr["FLDREGIONID"].ToString();
            //ddlCountry.SelectedValue = dr["FLDCOUNTRYID"].ToString();
            //txtYard.Text = dr["FLDYARD"].ToString();
            txtDiscription.Text = dr["FLDDISCRIPTION"].ToString();
            UcScruberstartDate.Text = dr["FLDSCRBSTARTDATE"].ToString();
            UcScruberEndDate.Text = dr["FLDSCRBENDDATE"].ToString();
            if (General.GetNullableInteger(dr["FLDSCRBREGIONID"].ToString()) != null)
                ddlScruberregion.SelectedValue = dr["FLDSCRBREGIONID"].ToString();
            if (General.GetNullableInteger(dr["FLDSCRBCOUNTRYID"].ToString()) != null)
                ddlScruberCountry.SelectedValue = dr["FLDSCRBCOUNTRYID"].ToString();
            txtScruberYard.Text = dr["FLDSCRBYARD"].ToString();
            txtscrberDiscription.Text = dr["FLDSCRBDISCRIPTION"].ToString();

            rddedicatedFOSample.SelectedValue = dr["FLDDEDICATEDFOSAMPLINGYN"].ToString();
            txtdedicatedFOSample.Text = dr["FLDDEDICATEDFOSAMPLING"].ToString();

            chkfuelstorage.Checked= dr["FLDFUELSTORAGEYN"].ToString() == "1" ? true : false;
            chkfueltransfer.Checked = dr["FLDFUELTRANSFERYN"].ToString() == "1" ? true : false;
            Ucfueltransferstart.Text = dr["FLDFUELTRANSFERSTARTDATE"].ToString();
            Ucfueltransferend.Text = dr["FLDFUELTRANSFERENDDATE"].ToString();
            txtfueltransfer.Text = dr["FLDFUELTRANSFERDETAIL"].ToString();
            chkCombustion.Checked = dr["FLDCOMBUSTIONYN"].ToString() == "1" ? true : false;
            UcCombustionstart.Text = dr["FLDCOMBUSTIONSTARTDATE"].ToString();
            UcCombustionend.Text = dr["FLDCOMBUSTIONENDDATE"].ToString();
            txtCombustion.Text = dr["FLDCOMBUSTIONDETAIL"].ToString();

            ViewState["DTKey"] = dr["FLDDTKEY"].ToString();

            configure();
            configureControls();
        }
        else
        {
            BindCountry();
            BindScrubberCountry();
            configure();
            configureControls();
        }
    }

    private void configure()
    {
        if (rdFuelMofifyreqyn.SelectedValue == "0")
        {
            trHeader.Visible = true;
            trDate.Visible = true;
            //tryardplace.Visible = true;
            trDiscription.Visible = true;
          //  trdescriptionHead.Visible = true;
            trmodfiyDocument.Visible = true;
            // trOfficeDiscription.Visible = true;
            trfueltransfer.Visible = true;
            trfueltransferdesc.Visible = true;
            trCombustion.Visible = true;
            trCombustiondesc.Visible = true;
        }
        else
        {
            trHeader.Visible = false;
            trDate.Visible = false;
           // tryardplace.Visible = false;
            trDiscription.Visible = false;
          //  trdescriptionHead.Visible = false;
            trmodfiyDocument.Visible = false;
            //trOfficeDiscription.Visible = false;
            trfueltransfer.Visible = false;
            trfueltransferdesc.Visible = false;
            trCombustion.Visible = false;
            trCombustiondesc.Visible = false;
        }
        if (rdscriberwillinstallyn.SelectedValue == "0")
        {


            trscrbrHeader.Visible = true;
            trScruberDate.Visible = true;
            trScruberyardplace.Visible = true;
            trScruberDiscription.Visible = true;
          //  trscrbrdescriptionHead.Visible = true;
          //  trScruberOfficeDiscription.Visible = true;

        }
        else
        {
            trscrbrHeader.Visible = false;
            trScruberDate.Visible = false;
            trScruberyardplace.Visible = false;
            trScruberDiscription.Visible = false;
           // trscrbrdescriptionHead.Visible = false;
           // trScruberOfficeDiscription.Visible = false;
        }
        if (rddedicatedFOSample.SelectedValue == "0")
        {
            trdedicatedFOSample.Visible = true;
           // trOfficededicatedFOSample.Visible = true;
            //tr3.Visible = true;
        }
        else
        {
            trdedicatedFOSample.Visible = false;
           // trOfficededicatedFOSample.Visible = false;
           // tr3.Visible = false;
        }
    }
    private void configureControls()
    {
        if(chkfuelstorage.Checked==true)
        {
            UcstartDate.Enabled = true;
            UcstartDate.CssClass= "input";
            UcEndDate.Enabled = true;
            UcEndDate.CssClass= "input";
            txtDiscription.Enabled = true;
            txtDiscription.CssClass= "input";
        }
        else
        {
            UcstartDate.Enabled = false;
            UcstartDate.CssClass = "readonlytextbox";
            UcEndDate.Enabled = false;
            UcEndDate.CssClass = "readonlytextbox";
            txtDiscription.Enabled = false;
            txtDiscription.CssClass = "readonlytextbox";
        }
        if(chkfueltransfer.Checked==true)
        {
            Ucfueltransferstart.Enabled = true;
            Ucfueltransferstart.CssClass = "input";
            Ucfueltransferend.Enabled = true;
            Ucfueltransferend.CssClass = "input";
            txtfueltransfer.Enabled = true;
            txtfueltransfer.CssClass = "input";
        }
        else
        {
            Ucfueltransferstart.Enabled = false;
            Ucfueltransferstart.CssClass = "readonlytextbox";
            Ucfueltransferend.Enabled = false;
            Ucfueltransferend.CssClass = "readonlytextbox";
            txtfueltransfer.Enabled = false;
            txtfueltransfer.CssClass = "readonlytextbox";
        }
        if (chkCombustion.Checked == true)
        {
            UcCombustionstart.Enabled = true;
            UcCombustionstart.CssClass = "input";
            UcCombustionend.Enabled = true;
            UcCombustionend.CssClass = "input";
            txtCombustion.Enabled = true;
            txtCombustion.CssClass = "input";
        }
        else
        {
            UcCombustionstart.Enabled = false;
            UcCombustionstart.CssClass = "readonlytextbox";
            UcCombustionend.Enabled = false;
            UcCombustionend.CssClass = "readonlytextbox";
            txtCombustion.Enabled = false;
            txtCombustion.CssClass = "readonlytextbox";
        }
    }
    protected void MenuFuelSystemMofify_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (General.GetNullableGuid(ViewState["SIPFOSYSTEMMODIFYETAILID"].ToString()) == null)
                {
                    Guid? sipfosystemmodifyetailid = null;
                    PhoenixVesselPositionSIPFuelOilSystemModification.InsertSIPFueloilsystemmodificationsDetail(
                        General.GetNullableInteger(UcVessel.SelectedVessel)
                        , int.Parse(rdFuelMofifyreqyn.SelectedValue)//chkFuelMofifyreqyn.Checked == true ? 1 : 0
                        , int.Parse(rdscriberwillinstallyn.SelectedValue)//chkscriberwillinstallyn.Checked == true ? 1 : 0
                        , ref sipfosystemmodifyetailid
                        , General.GetNullableInteger(ddlType.SelectedValue)
                        , General.GetNullableDateTime(UcstartDate.Text)
                        , General.GetNullableDateTime(UcEndDate.Text)
                        , General.GetNullableInteger(ddlregion.SelectedValue)
                        , General.GetNullableInteger(ddlCountry.SelectedValue)
                        , General.GetNullableString(txtYard.Text)
                        , General.GetNullableString(txtDiscription.Text)
                        , General.GetNullableDateTime(UcScruberstartDate.Text)
                        , General.GetNullableDateTime(UcScruberEndDate.Text)
                        , General.GetNullableInteger(ddlScruberregion.SelectedValue)
                        , General.GetNullableInteger(ddlScruberCountry.SelectedValue)
                        , General.GetNullableString(txtScruberYard.Text)
                        , General.GetNullableString(txtscrberDiscription.Text)
                        , int.Parse(rddedicatedFOSample.SelectedValue)
                        , General.GetNullableString(txtdedicatedFOSample.Text)
                        , chkfueltransfer.Checked==true?1:0
                        , General.GetNullableDateTime(Ucfueltransferstart.Text)
                        , General.GetNullableDateTime(Ucfueltransferend.Text)
                        , General.GetNullableString(txtfueltransfer.Text)
                        , chkCombustion.Checked == true ? 1 : 0
                        , General.GetNullableDateTime(UcCombustionstart.Text)
                        , General.GetNullableDateTime(UcCombustionend.Text)
                        , General.GetNullableString(txtCombustion.Text)
                        , chkfuelstorage.Checked == true ? 1 : 0
                        );
                    ViewState["SIPFOSYSTEMMODIFYETAILID"] = sipfosystemmodifyetailid;
                }
                else
                {
                    PhoenixVesselPositionSIPFuelOilSystemModification.UpdateSIPFueloilsystemmodificationsDetail(
                        General.GetNullableInteger(UcVessel.SelectedVessel)
                        , int.Parse(rdFuelMofifyreqyn.SelectedValue)//chkFuelMofifyreqyn.Checked == true ? 1 : 0
                        , int.Parse(rdscriberwillinstallyn.SelectedValue)//chkscriberwillinstallyn.Checked == true ? 1 : 0
                        , General.GetNullableGuid(ViewState["SIPFOSYSTEMMODIFYETAILID"].ToString())
                        , General.GetNullableInteger(ddlType.SelectedValue)
                        , General.GetNullableDateTime(UcstartDate.Text)
                        , General.GetNullableDateTime(UcEndDate.Text)
                        , General.GetNullableInteger(ddlregion.SelectedValue)
                        , General.GetNullableInteger(ddlCountry.SelectedValue)
                        , General.GetNullableString(txtYard.Text)
                        , General.GetNullableString(txtDiscription.Text)
                        , General.GetNullableDateTime(UcScruberstartDate.Text)
                        , General.GetNullableDateTime(UcScruberEndDate.Text)
                        , General.GetNullableInteger(ddlScruberregion.SelectedValue)
                        , General.GetNullableInteger(ddlScruberCountry.SelectedValue)
                        , General.GetNullableString(txtScruberYard.Text)
                        , General.GetNullableString(txtscrberDiscription.Text)
                        , int.Parse(rddedicatedFOSample.SelectedValue)
                        , General.GetNullableString(txtdedicatedFOSample.Text)
                        , chkfueltransfer.Checked == true ? 1 : 0
                        , General.GetNullableDateTime(Ucfueltransferstart.Text)
                        , General.GetNullableDateTime(Ucfueltransferend.Text)
                        , General.GetNullableString(txtfueltransfer.Text)
                        , chkCombustion.Checked == true ? 1 : 0
                        , General.GetNullableDateTime(UcCombustionstart.Text)
                        , General.GetNullableDateTime(UcCombustionend.Text)
                        , General.GetNullableString(txtCombustion.Text)
                        , chkfuelstorage.Checked == true ? 1 : 0
                        );
                }
                
                ucStatus.Text = "Information saved successfully.";
                EditData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void TabFuelSystemMofify_TabStripCommand(object sender, EventArgs e)
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
                string filePath = Server.MapPath("~/Template/VesselPosition/IMO Guidance - IMPACT ON MACHINERY SYSTEMS.pdf");
                Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=pdf");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MeetingManufact_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string Manufactorclass = (((RadTextBox)e.Item.FindControl("txtFOSystemModifyAdd")).Text);
                string txtdetail = (((RadTextBox)e.Item.FindControl("txtDetailsAdd")).Text);
                string date = (((UserControlDate)e.Item.FindControl("ucDateAdd")).Text);

                PhoenixVesselPositionSIPFuelOilSystemModification.InsertSIPFueloilsystemmodifications(
                    General.GetNullableInteger(UcVessel.SelectedVessel),
                    General.GetNullableDateTime(date),
                    General.GetNullableString(Manufactorclass),
                    General.GetNullableString(txtdetail),
                    0, null, null
                    );

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionSIPFuelOilSystemModification.DeleteSIPFueloilsystemmodifications(
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblFOSystemModifyId")).Text));
                Rebind();
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
    protected void MeetingManufact_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");


                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
                if (att != null)
                {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    if (drv["FLDISATTACHMENT"].ToString() == "0")
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" ><i class=\"fas fa-paperclip-na\"></i></span>";
                        att.Controls.Add(html);
                    }
                    att.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.VESSELPOSITION + "&type=SIPFOMODIFICATION&cmdname=FOMODIFICATION'); return false;");
                }
            }
            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void MeetingManufactRowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {
            string Manufactorclass = (((RadTextBox)e.Item.FindControl("txtFOSystemModifyEdit")).Text);
            string txtdetail = (((RadTextBox)e.Item.FindControl("txtDetailsEdit")).Text);
            string date = (((UserControlDate)e.Item.FindControl("ucDateEdit")).Text);
            string lblFOSystemModifyEdit = (((RadLabel)e.Item.FindControl("lblFOSystemModifyEdit")).Text);

            PhoenixVesselPositionSIPFuelOilSystemModification.UpdateSIPFueloilsystemmodifications(
                General.GetNullableGuid(lblFOSystemModifyEdit),
                General.GetNullableInteger(UcVessel.SelectedVessel),
                General.GetNullableDateTime(date),
                General.GetNullableString(Manufactorclass),
                General.GetNullableString(txtdetail),
                0,null,null
                );

            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
    ////////////////////////////////////////

    protected void gvMeetingClass_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string Manufactorclass = (((RadTextBox)e.Item.FindControl("txtFOSystemModifyAdd")).Text);
                string txtdetail = (((RadTextBox)e.Item.FindControl("txtDetailsAdd")).Text);
                string date = (((UserControlDate)e.Item.FindControl("ucDateAdd")).Text);

                PhoenixVesselPositionSIPFuelOilSystemModification.InsertSIPFueloilsystemmodifications(
                    General.GetNullableInteger(UcVessel.SelectedVessel),
                    General.GetNullableDateTime(date),
                    General.GetNullableString(Manufactorclass),
                    General.GetNullableString(txtdetail),
                    1,
                    General.GetNullableString(txtdetail),
                    General.GetNullableInteger(txtdetail));

                RebindClass();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixVesselPositionSIPFuelOilSystemModification.DeleteSIPFueloilsystemmodifications(
                    General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblFOSystemModifyId")).Text));
                RebindClass();
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
    protected void gvMeetingClass_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            if (e.Item is GridEditableItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;

                LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

                LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
                if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

                LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
                if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

                LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
                if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);


                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtFOSystemModifyIdEdit");
                if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

                LinkButton ib1 = (LinkButton)e.Item.FindControl("btnFOSystemModifyEdit");
                if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListFOSystemModifyEdit', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=137')");

                LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
                if (att != null)
                {
                    att.Visible = SessionUtil.CanAccess(this.ViewState, att.CommandName);
                    if (drv["FLDISATTACHMENT"].ToString() == "0")
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" ><i class=\"fas fa-paperclip-na\"></i></span>";
                        att.Controls.Add(html);
                    }
                    att.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                        + PhoenixModule.VESSELPOSITION + "&type=SIPFOMODIFICATION&cmdname=FOMODIFICATION'); return false;");
                }

            }
            if (e.Item is GridFooterItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
                if (db != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                        db.Visible = false;
                }

                RadTextBox tb1 = (RadTextBox)e.Item.FindControl("txtFOSystemModifyIdAdd");
                if (tb1 != null) tb1.Attributes.Add("style", "visibility:hidden");

                LinkButton ib1 = (LinkButton)e.Item.FindControl("btnFOSystemModifyAdd");
                if (ib1 != null) ib1.Attributes.Add("onclick", "return showPickList('spnPickListFOSystemModifyAdd', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=137')");

            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    protected void gvMeetingClassRowUpdating(object sender, GridCommandEventArgs e)
    {
        try
        {

            string Manufactorclass = (((RadTextBox)e.Item.FindControl("txtFOSystemModifyEdit")).Text);
            string txtdetail = (((RadTextBox)e.Item.FindControl("txtDetailsEdit")).Text);
            string date = (((UserControlDate)e.Item.FindControl("ucDateEdit")).Text);
            string lblFOSystemModifyEdit = (((RadLabel)e.Item.FindControl("lblFOSystemModifyEdit")).Text);

            PhoenixVesselPositionSIPFuelOilSystemModification.UpdateSIPFueloilsystemmodifications(
                General.GetNullableGuid(lblFOSystemModifyEdit),
                General.GetNullableInteger(UcVessel.SelectedVessel),
                General.GetNullableDateTime(date),
                General.GetNullableString(Manufactorclass),
                General.GetNullableString(txtdetail),
                1,
                General.GetNullableString(txtdetail),
                General.GetNullableInteger(txtdetail));

            RebindClass();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
  
    protected void imgPdf_Click(object sender, ImageClickEventArgs e)
    {
        string filePath = Server.MapPath("~/Template/VesselPosition/IMO Guidance - IMPACT ON MACHINERY SYSTEMS.pdf");
        Response.Redirect("../Reports/ReportsDownload.aspx?filename=" + filePath + "&type=pdf");
    }

    protected void chkFuelMofifyreqyn_CheckedChanged(object sender, EventArgs e)
    {
        configure();
    }

    protected void chkscriberwillinstallyn_CheckedChanged(object sender, EventArgs e)
    {
        configure(); 
    }
    protected void linkModification_Click(object sender, EventArgs e)
    {
        EditData();
        if (ViewState["DTKey"] != null)
        {
            String script = "parent.openNewWindow('codehelp1','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + ViewState["DTKey"] + "&mod="
                            + PhoenixModule.VESSELPOSITION + "&type=SIPFUELMODIFICATION')";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "PopupScript", script, true);
        }
    }

    protected void rdFuelMofifyreqyn_SelectedIndexChanged(object sender, EventArgs e)
    {
        configure();
    }

    protected void rdscriberwillinstallyn_SelectedIndexChanged(object sender, EventArgs e)
    {
        configure();
    }
    protected void rddedicatedFOSample_SelectedIndexChanged(object sender, EventArgs e)
    {
        configure();
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
        RebindClass();
    }

    protected void chkfuelstorage_CheckedChanged(object sender, EventArgs e)
    {
        configureControls();
    }

    protected void chkfueltransfer_CheckedChanged(object sender, EventArgs e)
    {
        configureControls();
    }

    protected void chkCombustion_CheckedChanged(object sender, EventArgs e)
    {
        configureControls();
    }

    protected void gvMeetingManufact_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMeetingManufact.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDSHORTNAME", "FLDOPERATIONTYPE" };
        string[] alCaptions = { "Cargo Operations", "Description" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixVesselPositionSIPFuelOilSystemModification.SIPFueloilsystemmodificationsSearch(
            General.GetNullableInteger(UcVessel.SelectedVessel),
            0,
            sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBER"],
            gvMeetingManufact.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        General.SetPrintOptions("gvMeetingManufact", "Meeting with Manufaturer", alCaptions, alColumns, ds);
        gvMeetingManufact.DataSource = ds;
        gvMeetingManufact.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void Rebind()
    {
        gvMeetingManufact.SelectedIndexes.Clear();
        gvMeetingManufact.EditIndexes.Clear();
        gvMeetingManufact.DataSource = null;
        gvMeetingManufact.Rebind();
    }
    protected void gvMeetingClass_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBERCLASS"] = ViewState["PAGENUMBERCLASS"] != null ? ViewState["PAGENUMBERCLASS"] : gvMeetingClass.CurrentPageIndex + 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string sortexpression = (ViewState["SORTEXPRESSIONCLASS"] == null) ? null : (ViewState["SORTEXPRESSIONCLASS"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTIONCLASS"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTIONCLASS"].ToString());

        DataSet ds = PhoenixVesselPositionSIPFuelOilSystemModification.SIPFueloilsystemmodificationsSearch(
            General.GetNullableInteger(UcVessel.SelectedVessel),
            1,
            sortexpression,
            sortdirection,
            (int)ViewState["PAGENUMBERCLASS"],
            gvMeetingClass.PageSize,
            ref iRowCount,
            ref iTotalPageCount);

        gvMeetingClass.DataSource = ds;
        gvMeetingClass.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNTCLASS"] = iRowCount;
        ViewState["TOTALPAGECOUNTCLASS"] = iTotalPageCount;

    }
    protected void RebindClass()
    {
        gvMeetingClass.SelectedIndexes.Clear();
        gvMeetingClass.EditIndexes.Clear();
        gvMeetingClass.DataSource = null;
        gvMeetingClass.Rebind();
    }
    protected void MeetingManufact_Sorting(object sender, GridSortCommandEventArgs e)
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
        Rebind();
    }
    protected void gvMeetingClass_Sorting(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSIONCLASS"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTIONCLASS"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTIONCLASS"] = "1";
                break;
        }
        RebindClass();
    }
}