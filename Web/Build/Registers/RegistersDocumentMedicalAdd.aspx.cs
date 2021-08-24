using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class Registers_RegistersDocumentMedicalAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 95, 0, "P&I,UKP,PMU");
            cblPIAdd.DataSource = ds;
            cblPIAdd.DataBind();
            SessionUtil.PageAccessRights(this.ViewState);
            ViewState["DocumentId"] = "";
            if (Request.QueryString["MedicalId"] != null)
            {
                ViewState["DocumentId"] = Request.QueryString["MedicalId"].ToString();
                DocumentMedicalEdit(Int32.Parse(Request.QueryString["MedicalId"].ToString()));
            }
            //chkUserGroup.DataSource = SessionUtil.UserGroupList();
            //chkUserGroup.DataTextField = "FLDGROUPNAME";
            //chkUserGroup.DataValueField = "FLDGROUPCODE";
            //chkUserGroup.DataBind();

            // BindChartererList();
            // BindFlagList();
            // BindManagementList();
            //BindVesselTypeList();
            //BindRankList();
            //  if (Request.QueryString["MedicalId"] != null)
            //       DocumentMedicalEdit(Int32.Parse(Request.QueryString["MedicalId"].ToString()));

            BindOffshoreStages(ddlStage);

            if (chkExpiryYNAdd.Checked == true)
            {
                txtExpiryYNText.Visible = true;
                lblMonths.Visible = true;
            }
            else
            {
                txtExpiryYNText.Visible = false;
                lblMonths.Visible = false;
            }
        }
        PhoenixToolbar toolbar = new PhoenixToolbar();
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        if (Request.QueryString["MedicalId"] != null)
        {
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);

            imgRankPicklist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterRankList.aspx?DocumentMedicalId=" + ViewState["DocumentId"] + "&type=MEDICAL" + "',false,600,400); return false;");
            imgvessseltypelist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterVesselTypeList.aspx?DocumentMedicalId=" + ViewState["DocumentId"] + "&type=MEDICAL" + "',false,600,400); return false;");
            imgflagpicklist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterFlagList.aspx?DocumentMedicalId=" + ViewState["DocumentId"] + "&type=MEDICAL" + "',false,600,400); return false;");
            imgcharterer.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterChartererList.aspx?DocumentMedicalId=" + ViewState["DocumentId"] + "&type=MEDICAL" + "',false,600,400); return false;");
            imgcompany.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterManagementCompanyList.aspx?DocumentMedicalId=" + ViewState["DocumentId"] + "&type=MEDICAL" + "',false,600,400); return false;");
            imgowner.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentOwnerList.aspx?DocumentMedicalId=" + ViewState["DocumentId"] + "&type=MEDICAL" + "',false,600,400); return false;");
            imgusergroup.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentUserWaiverList.aspx?DocumentMedicalId=" + ViewState["DocumentId"] + "&type=MEDICAL" + "',false,600,400); return false;");

        }
        else
        {
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            BindOffshoreStages(ddlStage);
        }
        MenuDoumentLicenceList.AccessRights = this.ViewState;
        MenuDoumentLicenceList.MenuList = toolbar.Show();

    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //DocumentMedicalEdit(Int32.Parse(ViewState["DocumentId"].ToString()));


    }
    protected void MenuDoumentLicenceList_TabStripCommand(object sender, EventArgs e)
    {
        String scriptClosePopup = String.Format("javascript:fnReloadList('codehelp1');");

        String scriptKeepPopupOpen = String.Format("javascript:fnReloadList('codehelp1', null, 'yes');");

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            string type = string.Empty;
            foreach (ButtonListItem li in cblPIAdd.Items)
            {
                type += (li.Selected ? li.Value + "," : string.Empty);
            }
            if (!IsValidDocumentMedical(txtTestname.Text
                 , txtcode.Text
                  , ucFrequencyAdd.SelectedHard
             ))
            {
                ucError.Visible = true;
                return;
            }
            //string UserGroupList = "";
            //string UGList = "";
            //foreach (RadComboBoxItem li in chkUserGroup.Items)
            //{
            //    if (li.Checked)
            //    {
            //        UGList += li.Value + ",";
            //    }
            //}

            //if (UGList != "")
            //{
            //    UserGroupList = "," + UGList;
            //}
            if (ViewState["DocumentId"].ToString() != "")
            {
                PhoenixRegistersDocumentMedical.UpdateDocumentMedical(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , Int32.Parse(ViewState["DocumentId"].ToString())
            , txtTestname.Text
            , null, type.TrimEnd(',')
            , General.GetNullableInteger(chkActiveYesOrNo.Checked == true ? "1" : "0")
            , General.GetNullableInteger(chkExpiryYNAdd.Checked == true ? "1" : "0")
            , null// General.GetNullableInteger(ucExpiryPeriod.Text)
            , General.GetNullableInteger(ucFrequencyAdd.SelectedHard)
            , General.GetNullableString(txtcode.Text)
            , null
            , null
            , General.GetNullableInteger(ddlStage.SelectedValue)
            , General.GetNullableInteger(chkMandatoryYNEdit.Checked == true ? "1" : "0")
            , General.GetNullableInteger(chkWaiverYN.Checked == true ? "1" : "0")
            , null//General.GetNullableString(UserGroupList)
            , (chkAdditionDocYnAdd.Checked == true) ? 1 : 0
            , (chkAuthReqYnAdd.Checked == true) ? 1 : 0
            , General.GetNullableInteger(ucCategory.SelectedDocumentCategoryID)
            , (chkShowInMasterChecklistYNAdd.Checked == true) ? 1 : 0
            , (chkPhotocopyAcceptableYnAdd.Checked == true) ? 1 : 0
             , null//General.GetNullableString(General.RadGetComboboxCheckList(ddlflag))
             , null//General.GetNullableString(General.RadGetComboboxCheckList(chkChartererList))
             , null//General.GetNullableString(General.RadGetComboboxCheckList(ddlcompany))
             , null//General.GetNullableString(General.RadGetComboboxCheckList(chkRankList))
             , null//General.GetNullableString(General.RadGetComboboxCheckList(chkVesselTypeList))
            , General.GetNullableInteger(txtExpiryYNText.Text)
            );
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptClosePopup, true);
            }
            else
            {
                int? resultdocumentid = null;
                PhoenixRegistersDocumentMedical.InsertDocumentMedical(
                     PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , txtTestname.Text
                    , null, type
                    , General.GetNullableInteger(chkActiveYesOrNo.Checked == true ? "1" : "0")
                    , General.GetNullableInteger(chkExpiryYNAdd.Checked == true ? "1" : "0")
                    , null// General.GetNullableInteger(ucExpiryPeriod.Text)
                    , General.GetNullableInteger(ucFrequencyAdd.SelectedHard)
                    , General.GetNullableString(txtcode.Text)
                    , null
                    , null
                    , General.GetNullableInteger(ddlStage.SelectedValue)
                    , General.GetNullableInteger(chkMandatoryYNEdit.Checked == true ? "1" : "0")
                    , General.GetNullableInteger(chkWaiverYN.Checked == true ? "1" : "0")
                    , null//General.GetNullableString(UserGroupList)
                    , (chkAdditionDocYnAdd.Checked == true) ? 1 : 0
                    , (chkAuthReqYnAdd.Checked == true) ? 1 : 0
                    , General.GetNullableInteger(ucCategory.SelectedDocumentCategoryID)
                    , (chkShowInMasterChecklistYNAdd.Checked == true) ? 1 : 0
                    , (chkPhotocopyAcceptableYnAdd.Checked == true) ? 1 : 0
                    , null//General.GetNullableString(General.RadGetComboboxCheckList(ddlflag))
                    , null//General.GetNullableString(General.RadGetComboboxCheckList(chkChartererList))
                    , null//General.GetNullableString(General.RadGetComboboxCheckList(ddlcompany))
                    , null//General.GetNullableString(General.RadGetComboboxCheckList(chkRankList))
                    , null//General.GetNullableString(General.RadGetComboboxCheckList(chkVesselTypeList))
                    , ref resultdocumentid
                    , General.GetNullableInteger(txtExpiryYNText.Text)
              );
                ViewState["DocumentId"] = int.Parse(resultdocumentid.ToString());

                imgRankPicklist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterRankList.aspx?DocumentMedicalId=" + ViewState["DocumentId"] + "&type=MEDICAL" + "',false,600,400); return false;");
                imgvessseltypelist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterVesselTypeList.aspx?DocumentMedicalId=" + ViewState["DocumentId"] + "&type=MEDICAL" + "',false,600,400); return false;");
                imgflagpicklist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterFlagList.aspx?DocumentMedicalId=" + ViewState["DocumentId"] + "&type=MEDICAL" + "',false,600,400); return false;");
                imgcharterer.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterChartererList.aspx?DocumentMedicalId=" + ViewState["DocumentId"] + "&type=MEDICAL" + "',false,600,400); return false;");
                imgcompany.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterManagementCompanyList.aspx?DocumentMedicalId=" + ViewState["DocumentId"] + "&type=MEDICAL" + "',false,600,400); return false;");
                imgowner.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentOwnerList.aspx?DocumentMedicalId=" + ViewState["DocumentId"] + "&type=MEDICAL" + "',false,600,400); return false;");
                imgusergroup.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentUserWaiverList.aspx?DocumentMedicalId=" + ViewState["DocumentId"] + "&type=MEDICAL" + "',false,600,400); return false;");
                // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptClosePopup, true);
            }
        }
    }

    protected void chkExpiryYNAdd_CheckedChanged(object sender, EventArgs e)
    {
        if (chkExpiryYNAdd.Checked == true)
        {
            txtExpiryYNText.Visible = true;
            lblMonths.Visible = true;
        }
        else
        {
            txtExpiryYNText.Visible = false;
            lblMonths.Visible = false;
        }
    }

    protected void chkMandatoryYNEdit_CheckedChanged(object sender, EventArgs e)
    {
        if (chkMandatoryYNEdit.Checked == true)
            chkWaiverYN.Enabled = true;
        else
        {
            chkWaiverYN.Checked = false;
            chkWaiverYN.Enabled = false;

            //chkUserGroup.SelectedIndex = -1;
            imgusergroup.Enabled = false;
        }
    }

    protected void chkWaiverYN_CheckedChanged(object sender, EventArgs e)
    {
        if (chkWaiverYN.Checked == true)
        {
            imgusergroup.Enabled = true;
        }
        else
        {
            //chkUserGroup.SelectedIndex = -1;
            imgusergroup.Enabled = false;
        }
    }
    //private void BindChartererList()
    //{
    //    DataSet ds = PhoenixCrewOffshoreTrainingMatrixStandard.TrainingMatrixStandardList();
    //    chkChartererList.DataSource = ds;
    //    chkChartererList.DataTextField = "FLDNAME";
    //    chkChartererList.DataValueField = "FLDADDRESSCODE";
    //    chkChartererList.DataBind();
    //}
    //private void BindManagementList()
    //{
    //    DataSet ds = PhoenixRegistersAddress.ListAddress(((int)PhoenixAddressType.MANAGER).ToString());
    //    ddlcompany.DataSource = ds;
    //    ddlcompany.DataTextField = "FLDNAME";
    //    ddlcompany.DataValueField = "FLDADDRESSCODE";
    //    ddlcompany.DataBind();
    //}
    //private void BindFlagList()
    //{
    //    ddlflag.DataSource = PhoenixRegistersFlag.ListFlag(null);
    //    ddlflag.DataBind();

    //}
    //private void BindRankList()
    //{
    //    DataSet ds = PhoenixRegistersRank.ListRank();
    //    chkRankList.DataSource = ds;
    //    chkRankList.DataTextField = "FLDRANKNAME";
    //    chkRankList.DataValueField = "FLDRANKID";
    //    chkRankList.DataBind();
    //}
    //private void BindVesselTypeList()
    //{
    //    DataSet ds = PhoenixRegistersVesselType.ListVesselType(0);
    //    chkVesselTypeList.DataSource = ds;
    //    chkVesselTypeList.DataTextField = "FLDTYPEDESCRIPTION";
    //    chkVesselTypeList.DataValueField = "FLDVESSELTYPEID";
    //    chkVesselTypeList.DataBind();
    //}

    private void DocumentMedicalEdit(int documentid)
    {
        DataSet ds = PhoenixRegistersDocumentMedical.EditDocumentMedical(documentid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ucCategory.SelectedDocumentCategoryID = dr["FLDDOCUMENTCATEGORYID"].ToString();
            txtTestname.Text = dr["FLDNAMEOFMEDICAL"].ToString();
            txtcode.Text = dr["FLDCODE"].ToString();
            foreach (ButtonListItem li in cblPIAdd.Items)
            {
                if (("," + dr["FLDMEDICALTYPE"].ToString() + ",").Contains("," + li.Value + ","))
                    li.Selected = true;
            }
            chkActiveYesOrNo.Checked = dr["FLDACTIVEYN"].ToString() == "1" ? true : false;
            chkExpiryYNAdd.Checked = dr["FLDEXPIRYYN"].ToString() == "1" ? true : false;
            ucFrequencyAdd.SelectedHard = dr["FLDFREQUENCY"].ToString();
            ddlStage.SelectedValue = dr["FLDSTAGEID"].ToString();
            chkMandatoryYNEdit.Checked = dr["FLDMANDATORYYN"].ToString() == "1" ? true : false;
            chkWaiverYN.Checked = dr["FLDWAIVERYN"].ToString() == "1" ? true : false;
            //General.RadBindComboBoxCheckList(chkUserGroup, dr["FLDUSERGROUPTOALLOWWAIVER"].ToString());
            chkExpiryYNAdd.Checked = dr["FLDEXPIRYYN"].ToString() == "1" ? true : false;
            chkAdditionDocYnAdd.Checked = dr["FLDADDITIONALDOCYN"].ToString() == "1" ? true : false;
            chkAuthReqYnAdd.Checked = dr["FLDAUTHENTICATIONREQYN"].ToString() == "1" ? true : false;
            chkShowInMasterChecklistYNAdd.Checked = dr["FLDSHOWINMASTERCHECKLISTYN"].ToString() == "1" ? true : false;
            chkPhotocopyAcceptableYnAdd.Checked = dr["FLDPHOTOCOPYACCEPTABLEYN"].ToString() == "1" ? true : false;


            //General.RadBindComboBoxCheckList(chkRankList, dr["FLDRANKS"].ToString());
            //General.RadBindComboBoxCheckList(chkVesselTypeList, dr["FLDVESSELTYPES"].ToString());
            //General.RadBindComboBoxCheckList(chkChartererList, dr["FLDCHARTER"].ToString());
            //General.RadBindComboBoxCheckList(ddlflag, dr["FLDFLAG"].ToString());
            //General.RadBindComboBoxCheckList(ddlcompany, dr["FLDCOMPANIES"].ToString());


            if (chkMandatoryYNEdit.Checked == true)
                chkWaiverYN.Enabled = true;
            else
                chkWaiverYN.Enabled = false;

            if (chkWaiverYN.Checked == true)
                imgusergroup.Enabled = true;
            else
                imgusergroup.Enabled = false;

            if (dr["FLDDOCUMENTCATEGORYID"].ToString() == "2") //STCW
            {

                lblCharterer.Visible = false;
                imgcharterer.Visible = false;
                lblflag.Visible = false;
                imgflagpicklist.Visible = false;
                lblmgr.Visible = false;
                imgcompany.Visible = false;
                lblowner.Visible = false;
                imgowner.Visible = false;


            }
            else if (dr["FLDDOCUMENTCATEGORYID"].ToString() == "3") //COMPANY REQUIREMENT
            {

                lblCharterer.Visible = false;
                imgcharterer.Visible = false;
                lblflag.Visible = false;
                imgflagpicklist.Visible = false;
                lblmgr.Visible = true;
                imgcompany.Visible = true;
                lblowner.Visible = true;
                imgowner.Visible = true;


            }


            else if (dr["FLDDOCUMENTCATEGORYID"].ToString() == "4") //CHARTERER
            {

                lblCharterer.Visible = true;
                imgcharterer.Visible = true;
                lblflag.Visible = false;
                imgflagpicklist.Visible = false;
                lblmgr.Visible = false;
                imgcompany.Visible = false;
                lblowner.Visible = true;
                imgowner.Visible = true;


            }
            else if (dr["FLDDOCUMENTCATEGORYID"].ToString() == "5") //Flag State Documents
            {

                lblCharterer.Visible = false;
                imgcharterer.Visible = false;
                lblflag.Visible = true;
                imgflagpicklist.Visible = true;
                lblmgr.Visible = false;
                imgcompany.Visible = false;
                lblowner.Visible = false;
                imgowner.Visible = false;

            }
            else if (dr["FLDDOCUMENTCATEGORYID"].ToString() == "6") //	Owner Requirement
            {

                lblCharterer.Visible = false;
                imgcharterer.Visible = false;
                lblflag.Visible = false;
                imgflagpicklist.Visible = false;
                lblmgr.Visible = false;
                imgcompany.Visible = false;
                lblowner.Visible = true;
                imgowner.Visible = true;

            }
            else
            {

                lblCharterer.Visible = false;
                imgcharterer.Visible = false;
                lblflag.Visible = false;
                imgflagpicklist.Visible = false;
                lblmgr.Visible = false;
                imgcompany.Visible = false;
                lblowner.Visible = false;
                imgowner.Visible = false;

            }
            if (chkExpiryYNAdd.Checked == true)
            {
                txtExpiryYNText.Visible = true;
                lblMonths.Visible = true;
            }
            else
            {
                txtExpiryYNText.Visible = false;
                lblMonths.Visible = false;
            }
            txtExpiryYNText.Text = dr["FLDEXPIRYTEXTBOX"].ToString();
        }
    }
    protected void BindOffshoreStages(RadComboBox ddl)
    {
        ddl.Items.Clear();
        ddl.DataSource = PhoenixRegistersDocumentLicence.ListOffshoreStage(null, null);
        ddl.DataTextField = "FLDSTAGE";
        ddl.DataValueField = "FLDSTAGEID";
        ddl.DataBind();
        // ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    private bool IsValidDocumentMedical(string nameofmedical, string code, string frequency)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (nameofmedical.Trim().Equals(""))
            ucError.ErrorMessage = "Medical Name is required.";

        if (General.GetNullableString(code) == null)
            ucError.ErrorMessage = "Code is required.";

        if (General.GetNullableInteger(frequency) == null)
            ucError.ErrorMessage = "Frequency is required.";

        if (chkExpiryYNAdd.Checked == true && txtExpiryYNText.Text.Equals(""))
            ucError.ErrorMessage = "Expiry Months is required.";

        return (!ucError.IsError);
    }
    protected void ucCategory_TextChangedEvent(object sender, EventArgs e)
    {
        if (ucCategory.SelectedValue.ToString() == "2") //STCW
        {
            lblCharterer.Visible = false;
            imgcharterer.Visible = false;
            lblflag.Visible = false;
            imgflagpicklist.Visible = false;
            lblmgr.Visible = false;
            imgcompany.Visible = false;
            lblowner.Visible = false;
            imgowner.Visible = false;
        }
        else if (ucCategory.SelectedValue.ToString() == "3") //COMPANY REQUIREMENT
        {
            lblCharterer.Visible = false;
            imgcharterer.Visible = false;
            lblflag.Visible = false;
            imgflagpicklist.Visible = false;
            lblmgr.Visible = true;
            imgcompany.Visible = true;
            lblowner.Visible = true;
            imgowner.Visible = true;
        }
        else if (ucCategory.SelectedValue.ToString() == "4") //CHARTERER
        {
            lblCharterer.Visible = true;
            imgcharterer.Visible = true;
            lblflag.Visible = false;
            imgflagpicklist.Visible = false;
            lblmgr.Visible = false;
            imgcompany.Visible = false;
            lblowner.Visible = true;
            imgowner.Visible = true;
        }
        else if (ucCategory.SelectedValue.ToString() == "5") //Flag State Documents
        {
            lblCharterer.Visible = false;
            imgcharterer.Visible = false;
            lblflag.Visible = true;
            imgflagpicklist.Visible = true;
            lblmgr.Visible = false;
            imgcompany.Visible = false;
            lblowner.Visible = false;
            imgowner.Visible = false;
        }
        else if (ucCategory.SelectedValue.ToString() == "6") //	Owner Requirement
        {
            lblCharterer.Visible = false;
            imgcharterer.Visible = false;
            lblflag.Visible = false;
            imgflagpicklist.Visible = false;
            lblmgr.Visible = false;
            imgcompany.Visible = false;
            lblowner.Visible = true;
            imgowner.Visible = true;
        }
        else
        {
            lblCharterer.Visible = false;
            imgcharterer.Visible = false;
            lblflag.Visible = false;
            imgflagpicklist.Visible = false;
            lblmgr.Visible = false;
            imgcompany.Visible = false;
            lblowner.Visible = false;
            imgowner.Visible = false;
        }
    }
}