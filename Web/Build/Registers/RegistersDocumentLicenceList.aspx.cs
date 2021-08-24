using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class RegistersDocumentLicenceList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);
            //chkUserGroup.DataSource = SessionUtil.UserGroupList();
            //chkUserGroup.DataTextField = "FLDGROUPNAME";
            //chkUserGroup.DataValueField = "FLDGROUPCODE";
            //chkUserGroup.DataBind();
            // BindHigherLicence();
            //BindChartererList();
            //BindFlagList();
            //BindManagementList();
            //BindVesselTypeList();
            //BindRankList();
            BindOffshoreStages(ddlStage);
            if (Request.QueryString["DocumentLicenceId"] != null)
                DocumentLicenceEdit(Int32.Parse(Request.QueryString["DocumentLicenceId"].ToString()));

            if (chkExpiry.Checked == true)
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

        if (Request.QueryString["DocumentLicenceId"] != null)
        {
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            ViewState["DocumentLicenceId"] = Request.QueryString["DocumentLicenceId"].ToString();

          
            imgRankPicklist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterRankList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
            imgvessseltypelist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterVesselTypeList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
            imgflagpicklist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterFlagList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
            imgcharterer.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterChartererList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
            imgcompany.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterManagementCompanyList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
            imgowner.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentOwnerList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
            imgalternate.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentAlternateCourseList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
            imgusergroup.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentUserWaiverList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
            imgflgendorsment.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentFlagEndorsementList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");

        }
        else
        {
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
         
        }
     

        MenuDoumentLicenceList.AccessRights = this.ViewState;
        MenuDoumentLicenceList.MenuList = toolbar.Show();
        ucOffcrew.HardTypeCode = ((int)PhoenixHardTypeCode.OFFCREW).ToString();
        ucGroup.HardTypeCode = ((int)PhoenixHardTypeCode.DOCUMENTGROUP).ToString();
    }
    //protected void BindHigherLicence()
    //{
    //    cblHigherDocument.Items.Clear();
    //    DataSet ds = PhoenixRegistersDocumentLicence.ListDocumentLicence(null);
    //    cblHigherDocument.DataSource = ds.Tables[0];
    //    cblHigherDocument.DataTextField = "FLDDOCUMENTNAME";
    //    cblHigherDocument.DataValueField = "FLDDOCUMENTID";
    //    cblHigherDocument.DataBind();
    //}
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        DocumentLicenceEdit(Int32.Parse(ViewState["DocumentLicenceId"].ToString()));

    }
    protected void DoumentLicenceList_TabStripCommand(object sender, EventArgs e)
    {
        String scriptClosePopup = String.Format("javascript:fnReloadList('codehelp1');");

        String scriptKeepPopupOpen = String.Format("javascript:fnReloadList('codehelp1', null, 'yes');");

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (IsValidDocumentLicence())
            {
               // string UGList = "";
                string UserGroupList = "";
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

                if (ViewState["DocumentLicenceId"] != null)
                {
                    PhoenixRegistersDocumentLicence.UpdateDocumentLicence(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , Int16.Parse(ViewState["DocumentLicenceId"].ToString())
                        , Int16.Parse(ucDocumentType.SelectedHard)
                        , txtLicence.Text
                        , General.GetNullableInteger(chkActiveYesOrNo.Checked == true ? "1" : "0")
                        , Int16.Parse(txtLevel.Text)
                        , General.GetNullableInteger(ucOffcrew.SelectedHard)
                        , General.GetNullableInteger(ucGroup.SelectedHard)
                        , txtAbbreviation.Text
                        , General.GetNullableInteger(chkExpiry.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(ucRank.SelectedRank)
                        , General.GetNullableInteger(ddlStage.SelectedValue)
                        , General.GetNullableInteger(chkMandatoryYN.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(chkWaiverYN.Checked == true ? "1" : "0")
                        , General.GetNullableString(UserGroupList)
                        , General.GetNullableInteger(chkAdditionalDocYN.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(chkAuthenticationReqYN.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(ucCategory.SelectedDocumentCategoryID)
                        , null//General.GetNullableString(General.RadGetComboboxCheckList(cblHigherDocument))
                        , General.GetNullableInteger(chkShowinMasterChecklistYN.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(chkPhotocopyAcceptableYN.Checked == true ? "1" : "0")
                        , txtOCIMFName.Text.Trim()
                        , null//General.GetNullableString(General.RadGetComboboxCheckList(ddlflag))
                        , null//General.GetNullableString(General.RadGetComboboxCheckList(chkChartererList))
                        , null//General.GetNullableString(General.RadGetComboboxCheckList(ddlcompany))
                        , null//General.GetNullableString(General.RadGetComboboxCheckList(chkRankList))
                        , null//General.GetNullableString(General.RadGetComboboxCheckList(chkVesselTypeList))
                        , General.GetNullableInteger(chkFlagEndorsement.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(txtExpiryYNText.Text)
                        , General.GetNullableInteger(chkCRAAccept.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(chkwavetonextyn.Checked == true ? "1" : "0")                       

                        );

                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptClosePopup, true);
                }

                else
                {
                    int? resultdocumentid = null;

                    PhoenixRegistersDocumentLicence.InsertDocumentLicence(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , Int16.Parse(ucDocumentType.SelectedHard)
                        , txtLicence.Text
                        , General.GetNullableInteger(chkActiveYesOrNo.Checked == true ? "1" : "0")
                        , Int16.Parse(txtLevel.Text)
                        , General.GetNullableInteger(ucOffcrew.SelectedHard)
                        , General.GetNullableInteger(ucGroup.SelectedHard)
                        , txtAbbreviation.Text
                        , General.GetNullableInteger(chkExpiry.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(ucRank.SelectedRank)
                        , General.GetNullableInteger(ddlStage.SelectedValue)
                        , General.GetNullableInteger(chkMandatoryYN.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(chkWaiverYN.Checked == true ? "1" : "0")
                        , General.GetNullableString(UserGroupList)
                        , General.GetNullableInteger(chkAdditionalDocYN.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(chkAuthenticationReqYN.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(ucCategory.SelectedDocumentCategoryID)
                        , null//General.GetNullableString(General.RadGetComboboxCheckList(cblHigherDocument))
                        , General.GetNullableInteger(chkShowinMasterChecklistYN.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(chkPhotocopyAcceptableYN.Checked == true ? "1" : "0")
                        , txtOCIMFName.Text.Trim()
                       ,  null//General.GetNullableString(General.RadGetComboboxCheckList(ddlflag))
                        , null//General.GetNullableString(General.RadGetComboboxCheckList(chkChartererList))
                        , null//General.GetNullableString(General.RadGetComboboxCheckList(ddlcompany))
                        , null//General.GetNullableString(General.RadGetComboboxCheckList(chkRankList))
                        , null//General.GetNullableString(General.RadGetComboboxCheckList(chkVesselTypeList))
                        , ref resultdocumentid
                        , General.GetNullableInteger(chkFlagEndorsement.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(txtExpiryYNText.Text)
                        , General.GetNullableInteger(chkCRAAccept.Checked == true ? "1" : "0")
                        , General.GetNullableInteger(chkwavetonextyn.Checked == true ? "1" : "0")


                        );

                    ViewState["DocumentLicenceId"] = int.Parse(resultdocumentid.ToString());

                    imgRankPicklist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterRankList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
                    imgvessseltypelist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterVesselTypeList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
                    imgflagpicklist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterFlagList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
                    imgcharterer.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterChartererList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
                    imgcompany.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterManagementCompanyList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
                    imgcompany.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterManagementCompanyList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
                    imgowner.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentOwnerList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
                    imgalternate.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentAlternateCourseList.aspx?DocumentLicenceId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");
                    imgusergroup.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentUserWaiverList.aspx?DocumentLicenseId=" + ViewState["DocumentLicenceId"] + "&type=LICENSE" + "',false,600,400); return false;");



                    //   ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptKeepPopupOpen, true);
                    //   Reset();
                }
            }
            else
            {
                ucError.Visible = true;
                return;
            }
        }
        if (CommandName.ToUpper().Equals("NEW"))
            Reset();

    }

    private bool IsValidDocumentLicence()
    {
        Int32 result;

        ucError.HeaderMessage = "Please provide the following required information";

        if (txtLicence.Text.Equals(""))
            ucError.ErrorMessage = "Licence Name is required.";

        if (!Int32.TryParse(ucDocumentType.SelectedHard, out result))
            ucError.ErrorMessage = "Document Type is required.";

        if (txtLevel.Text.Equals(""))
            ucError.ErrorMessage = "Level is required.";

        if (chkExpiry.Checked == true && txtExpiryYNText.Text.Equals(""))
            ucError.ErrorMessage = "Expiry Months is required.";

        return (!ucError.IsError);
    }

    private void Reset()
    {
        ViewState["DocumentLicenceId"] = null;

        txtLicence.Text = "";
        chkActiveYesOrNo.Checked = false;
        chkExpiry.Checked = false;
        txtLevel.Text = "";
        txtAbbreviation.Text = "";
        ucDocumentType.SelectedHard = "";
        txtLevel.Text = "";
        ucOffcrew.SelectedHard = "";
        ucGroup.SelectedHard = "";
        ucRank.SelectedRank = "";
        ddlStage.SelectedValue = "";
        ddlStage.Text = "";
        chkMandatoryYN.Checked = false;
        chkWaiverYN.Checked = false;
       // chkUserGroup.SelectedIndex = -1;
        txtOCIMFName.Text = "";
    }

    private void DocumentLicenceEdit(int documentLicenceid)
    {
        DataSet ds = PhoenixRegistersDocumentLicence.EditDocumentLicence(documentLicenceid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ucDocumentType.SelectedHard = dr["FLDDOCUMENTTYPE"].ToString();
            ucOffcrew.SelectedHard = dr["FLDOFFCREW"].ToString();
            ucGroup.SelectedHard = dr["FLDGROUP"].ToString();
            txtLicence.Text = dr["FLDLICENCE"].ToString();
            chkActiveYesOrNo.Checked = dr["FLDLOCALACTIVE"].ToString() == "1" ? true : false;
            chkExpiry.Checked = dr["FLDEXPIRY"].ToString() == "1" ? true : false;
            ucRank.SelectedRank = dr["FLDAPPLIEDTO"].ToString();
            txtLevel.Text = dr["FLDLEVEL"].ToString();
            txtAbbreviation.Text = dr["FLDABREVATION"].ToString();
            ddlStage.SelectedValue = dr["FLDSTAGEID"].ToString();
         //   ddlStage.Text = dr["FLDSTAGE"].ToString();
            chkMandatoryYN.Checked = dr["FLDMANDATORYYN"].ToString() == "1" ? true : false;
            chkWaiverYN.Checked = dr["FLDWAIVERYN"].ToString() == "1" ? true : false;

            ucCategory.SelectedDocumentCategoryID = dr["FLDDOCUMENTCATEGORYID"].ToString();

            chkAdditionalDocYN.Checked = dr["FLDADDITIONALDOCYN"].ToString() == "1" ? true : false;
            chkAuthenticationReqYN.Checked = dr["FLDAUTHENTICATIONREQYN"].ToString() == "1" ? true : false;
            chkCRAAccept.Checked = dr["FLDCRAACCEPTYN"].ToString() == "1" ? true : false;
            chkwavetonextyn.Checked = dr["FLDWAIVETONEXTSTAGEYN"].ToString() == "1" ? true : false;
            //General.RadBindComboBoxCheckList(cblHigherDocument, dr["FLDHIGHERDOCUMENTID"].ToString());
            //General.RadBindComboBoxCheckList(chkUserGroup, dr["FLDUSERGROUPTOALLOWWAIVER"].ToString());
            //General.RadBindComboBoxCheckList(chkRankList, dr["FLDRANKS"].ToString());
            //General.RadBindComboBoxCheckList(chkVesselTypeList, dr["FLDVESSELTYPES"].ToString());
            //General.RadBindComboBoxCheckList(chkChartererList, dr["FLDCHARTER"].ToString());
            //General.RadBindComboBoxCheckList(ddlflag, dr["FLDFLAG"].ToString());
            //General.RadBindComboBoxCheckList(ddlcompany, dr["FLDCOMPANIES"].ToString());
            //foreach (ButtonListItem li in chkUserGroup.Items)
            //{
            //    string[] slist = dr["FLDUSERGROUPTOALLOWWAIVER"].ToString().Split(',');
            //    foreach (string s in slist)
            //    {
            //        if (li.Value.Equals(s))
            //        {
            //            li.Selected = true;
            //        }
            //    }
            //}

            if (chkMandatoryYN.Checked == true)
                chkWaiverYN.Enabled = true;
            else
                chkWaiverYN.Enabled = false;

            if (chkWaiverYN.Checked == true)
                imgusergroup.Enabled = true;
            else
                imgusergroup.Enabled = false;

            chkShowinMasterChecklistYN.Checked = dr["FLDSHOWINMASTERCHECKLISTYN"].ToString() == "1" ? true : false;
            chkPhotocopyAcceptableYN.Checked = dr["FLDPHOTOCOPYACCEPTABLEYN"].ToString() == "1" ? true : false;
            txtOCIMFName.Text = dr["FLDOCIMFNAME"].ToString();
            chkFlagEndorsement.Checked = dr["FLDFLAGENDORSEMENTYN"].ToString() == "1" ? true : false;

            if (chkExpiry.Checked == true)
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

    protected void BindOffshoreStages(RadComboBox ddlStage)
    {
      //  ddlStage.Items.Clear();
        ddlStage.DataSource = PhoenixRegistersDocumentLicence.ListOffshoreStage(null, null);
        ddlStage.DataTextField = "FLDSTAGE";
        ddlStage.DataValueField = "FLDSTAGEID";
       
        ddlStage.DataBind();
       
        // ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    protected void chkMandatoryYN_CheckedChanged(object sender, EventArgs e)
    {
        if (chkMandatoryYN.Checked == true)
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
    //private void BindVesselTypeList()
    //{
    //    DataSet ds = PhoenixRegistersVesselType.ListVesselType(0);
    //    chkVesselTypeList.DataSource = ds;
    //    chkVesselTypeList.DataTextField = "FLDTYPEDESCRIPTION";
    //    chkVesselTypeList.DataValueField = "FLDVESSELTYPEID";
    //    chkVesselTypeList.DataBind();
    //}

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


    protected void chkExpiry_CheckedChanged(object sender, EventArgs e)
    {
        if (chkExpiry.Checked == true)
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
}
