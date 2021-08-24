using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Registers_RegistersDocumentOtherAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //DataSet ds = PhoenixRegistersHard.ListHard(PhoenixRegistersHard.ListHard(0, ((int)PhoenixHardTypeCode.CREWDOCUMENTTYPE), byte.Parse("0"), "CDC,VSA,NON,WPT"));
            //cblPIAdd.DataSource = ds;
            //cblPIAdd.DataBind();
            //ucGroupAdd.d = PhoenixRegistersHard.ListHard(((int)PhoenixHardTypeCode.CREWDOCUMENTTYPE), byte.Parse("0"), "CDC,VSA,NON,WPT",null);
            //ucGroupAdd.bind();
            ucGroupAdd.HardTypeCode = ((int)PhoenixHardTypeCode.CREWDOCUMENTTYPE).ToString();
            SessionUtil.PageAccessRights(this.ViewState);
            //chkUserGroup.DataSource = SessionUtil.UserGroupList();
            //chkUserGroup.DataTextField = "FLDGROUPNAME";
            //chkUserGroup.DataValueField = "FLDGROUPCODE";
            //chkUserGroup.DataBind();

            //BindChartererList();
            //BindFlagList();
            //BindManagementList();
            //BindVesselTypeList();
            //BindRankList();
            //   if (Request.QueryString["OtherId"] != null)
            //  DocumentOtherEdit(Int32.Parse(Request.QueryString["OtherId"].ToString()));
            ViewState["DocumentId"] = "";
            if (Request.QueryString["OtherId"] != null)
            {
                ViewState["DocumentId"] = Request.QueryString["OtherId"].ToString();
                DocumentOtherEdit(Int32.Parse(Request.QueryString["OtherId"].ToString()));
            }
            BindOffshoreStages(ddlStage);

            if (chkhavingexpyn.Checked == true)
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

        if (Request.QueryString["OtherId"] != null)
        {
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
           

            imgRankPicklist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterRankList.aspx?OtherDocumentId=" + ViewState["DocumentId"] + "&type=OTHER" + "',false,600,400); return false;");
            imgvessseltypelist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterVesselTypeList.aspx?OtherDocumentId=" + ViewState["DocumentId"] + "&type=OTHER" + "',false,600,400); return false;");
            imgflagpicklist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterFlagList.aspx?OtherDocumentId=" + ViewState["DocumentId"] + "&type=OTHER" + "',false,600,400); return false;");
            imgcharterer.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterChartererList.aspx?OtherDocumentId=" + ViewState["DocumentId"] + "&type=OTHER" + "',false,600,400); return false;");
            imgcompany.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterManagementCompanyList.aspx?OtherDocumentId=" + ViewState["DocumentId"] + "&type=OTHER" + "',false,600,400); return false;");
            imgowner.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentOwnerList.aspx?OtherDocumentId=" + ViewState["DocumentId"] + "&type=OTHER" + "',false,600,400); return false;");
            imgusergroup.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentUserWaiverList.aspx?OtherDocumentId=" + ViewState["DocumentId"] + "&type=OTHER" + "',false,600,400); return false;");


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
    protected void BindOffshoreStages(RadComboBox ddl)
    {
        ddl.Items.Clear();
        ddl.DataSource = PhoenixRegistersDocumentLicence.ListOffshoreStage(null, null);
        ddl.DataTextField = "FLDSTAGE";
        ddl.DataValueField = "FLDSTAGEID";
        ddl.DataBind();
        // ddl.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }
    //private void BindChartererList()
    //{
    //    DataSet ds = PhoenixCrewOffshoreTrainingMatrixStandard.TrainingMatrixStandardList();
    //    chkChartererList.DataSource = ds;
    //    chkChartererList.DataTextField = "FLDNAME";
    //    chkChartererList.DataValueField = "FLDADDRESSCODE";
    //    chkChartererList.DataBind();
    //}
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        DocumentOtherEdit(Int32.Parse(ViewState["DocumentId"].ToString()));

    }
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
    private void DocumentOtherEdit(int documentid)
    {
        DataSet ds = PhoenixRegistersDocumentOther.EditDocumentOther(documentid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];

            ucCategory.SelectedDocumentCategoryID = dr["FLDDOCUMENTCATEGORYID"].ToString();
            txtdocname.Text = dr["FLDDOCUMENTNAME"].ToString();
            txtcode.Text = dr["FLDDOCUMENTCODE"].ToString();

            chkActiveYesOrNo.Checked = dr["FLDLOCALACTIVE"].ToString() == "1" ? true : false;
            chkShortExpiryAdd.Checked = dr["FLDSHORTEXPIRY"].ToString() == "1" ? true : false;
            ucGroupAdd.SelectedHard = dr["FLDGROUP"].ToString();

            ddlStage.SelectedValue = dr["FLDSTAGEID"].ToString();
            chkMandatoryYNEdit.Checked = dr["FLDMANDATORYYN"].ToString() == "1" ? true : false;
            chkWaiverYN.Checked = dr["FLDWAIVERYN"].ToString() == "1" ? true : false;
            //General.RadBindComboBoxCheckList(chkUserGroup, dr["FLDUSERGROUPTOALLOWWAIVER"].ToString());
            chkhavingexpyn.Checked = dr["FLDHAVINGEXPIRY"].ToString() == "1" ? true : false;
            chkAdditionDocYnAdd.Checked = dr["FLDADDITIONALDOCYN"].ToString() == "1" ? true : false;
            chkAuthReqYnAdd.Checked = dr["FLDAUTHENTICATIONREQYN"].ToString() == "1" ? true : false;
            chkShowInMasterChecklistYNAdd.Checked = dr["FLDSHOWINMASTERCHECKLISTYN"].ToString() == "1" ? true : false;
            chkPhotocopyAcceptableYnAdd.Checked = dr["FLDPHOTOCOPYACCEPTABLEYN"].ToString() == "1" ? true : false;
            chkNokYnAdd.Checked = dr["FLDNOKYN"].ToString() == "1" ? true : false;
            chkCBAOtherDocumentAdd.Checked = dr["FLDCBAOTHERMEMBERSHIPYN"].ToString() == "1" ? true : false;

            
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
                RadLabel1.Visible = false;
                imgcompany.Visible = false;
                lblown.Visible = false;
                imgowner.Visible = false;               
            }
            else if (dr["FLDDOCUMENTCATEGORYID"].ToString() == "3") //COMPANY REQUIREMENT
            {
                lblCharterer.Visible = false;
                imgcharterer.Visible = false;
                lblflag.Visible = false;
                imgflagpicklist.Visible = false;
                RadLabel1.Visible = true;
                imgcompany.Visible = true;
                lblown.Visible = true;
                imgowner.Visible = true;               
            }
            else if (dr["FLDDOCUMENTCATEGORYID"].ToString() == "4") //CHARTERER
            {
                lblCharterer.Visible = true;
                imgcharterer.Visible = true;
                lblflag.Visible = false;
                imgflagpicklist.Visible = false;
                RadLabel1.Visible = false;
                imgcompany.Visible = false;
                lblown.Visible = true;
                imgowner.Visible = true;             
            }
            else if (dr["FLDDOCUMENTCATEGORYID"].ToString() == "5") //Flag State Documents
            {
                lblCharterer.Visible = false;
                imgcharterer.Visible = false;
                lblflag.Visible = true;
                imgflagpicklist.Visible = true;
                RadLabel1.Visible = false;
                imgcompany.Visible = false;
                lblown.Visible = false;
                imgowner.Visible = false;               
            }
            else if (dr["FLDDOCUMENTCATEGORYID"].ToString() == "6") //	Owner Requirement
            {
                lblCharterer.Visible = false;
                imgcharterer.Visible = false;
                lblflag.Visible = false;
                imgflagpicklist.Visible = false;
                RadLabel1.Visible = false;
                imgcompany.Visible = false;
                lblown.Visible = true;
                imgowner.Visible = true;            
            }
            else
            {
                lblCharterer.Visible = false;
                imgcharterer.Visible = false;
                lblflag.Visible = false;
                imgflagpicklist.Visible = false;
                RadLabel1.Visible = false;
                imgcompany.Visible = false;
                lblown.Visible = false;
                imgowner.Visible = false;               
            }
            if (chkhavingexpyn.Checked == true)
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

    protected void MenuDoumentLicenceList_TabStripCommand(object sender, EventArgs e)
    {
        String scriptClosePopup = String.Format("javascript:fnReloadList('codehelp1');");

        String scriptKeepPopupOpen = String.Format("javascript:fnReloadList('codehelp1', null, 'yes');");

        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        //string UGList = "";
        //string UserGroupList = "";
        //foreach (RadComboBoxItem li in chkUserGroup.Items)
        //{
        //    if (li.Selected)
        //    {
        //        UGList += li.Value + ",";
        //    }
        //}

        //if (UGList != "")
        //{
        //    UserGroupList = "," + UGList;
        //}
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            int waiver = chkWaiverYN.Checked == true ? 1 : 0;
            string waiverusergroup = null;// UserGroupList;
            if (!IsValidDocumentOther(txtdocname.Text, waiver, waiverusergroup))
            {
                ucError.Visible = true;
                return;
            }
            if (ViewState["DocumentId"].ToString() != "")
            {
                UpdateDocumentOther(
                 Int32.Parse(ViewState["DocumentId"].ToString()),
                 txtcode.Text,
                 txtdocname.Text,
                 (chkActiveYesOrNo.Checked == true) ? 1 : 0,
                 General.GetNullableInteger(ucGroupAdd.SelectedHard),
                 (chkhavingexpyn.Checked == true) ? 1 : 0,
                 (chkShortExpiryAdd.Checked == true) ? 1 : 0,
                  (chkNokYnAdd.Checked == true) ? 1 : 0,
                  General.GetNullableInteger(ddlStage.SelectedValue),
                  General.GetNullableInteger(chkMandatoryYNEdit.Checked == true ? "1" : "0"),
                  General.GetNullableInteger(chkWaiverYN.Checked == true ? "1" : "0"),
                  null,//General.GetNullableString(UserGroupList),
                  General.GetNullableInteger(ucCategory.SelectedDocumentCategoryID),
                  General.GetNullableInteger(chkAdditionDocYnAdd.Checked == true ? "1" : "0"),
                  General.GetNullableInteger(chkAuthReqYnAdd.Checked == true ? "1" : "0"),
                  General.GetNullableInteger(chkCBAOtherDocumentAdd.Checked == true ? "1" : "0"),
                  (chkShowInMasterChecklistYNAdd.Checked == true) ? 1 : 0,
                  (chkPhotocopyAcceptableYnAdd.Checked == true) ? 1 : 0
                  ,General.GetNullableInteger(txtExpiryYNText.Text)
                  );
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptClosePopup, true);

            }
            else
            {
                int? resultdocumentid = null;
                PhoenixRegistersDocumentOther.InsertDocumentOther(
                                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                txtcode.Text,
                                txtdocname.Text,
                                 General.GetNullableInteger(chkActiveYesOrNo.Checked == true ? "1" : "0"),
                                 General.GetNullableInteger(ucGroupAdd.SelectedHard),
                                (chkhavingexpyn.Checked == true) ? 1 : 0,
                                (chkShortExpiryAdd.Checked == true) ? 1 : 0,
                                 (chkNokYnAdd.Checked == true) ? 1 : 0,
                                 General.GetNullableInteger(ddlStage.SelectedValue),
                                 General.GetNullableInteger(chkMandatoryYNEdit.Checked == true ? "1" : "0"),
                                 General.GetNullableInteger(chkWaiverYN.Checked == true ? "1" : "0"),
                                 null,//General.GetNullableString(UserGroupList),
                                 General.GetNullableInteger(ucCategory.SelectedDocumentCategoryID),
                                 General.GetNullableInteger(chkAdditionDocYnAdd.Checked == true ? "1" : "0"),
                                 General.GetNullableInteger(chkAuthReqYnAdd.Checked == true ? "1" : "0"),
                                 General.GetNullableInteger(chkCBAOtherDocumentAdd.Checked == true ? "1" : "0"),
                                 (chkShowInMasterChecklistYNAdd.Checked == true) ? 1 : 0,
                                 (chkPhotocopyAcceptableYnAdd.Checked == true) ? 1 : 0
                                 , null//General.GetNullableString(General.RadGetComboboxCheckList(ddlflag))
                                 , null//General.GetNullableString(General.RadGetComboboxCheckList(chkChartererList))
                                 , null//General.GetNullableString(General.RadGetComboboxCheckList(ddlcompany))
                                 , null//General.GetNullableString(General.RadGetComboboxCheckList(chkRankList))
                                 , null//General.GetNullableString(General.RadGetComboboxCheckList(chkVesselTypeList))
                                 ,ref resultdocumentid
                                 , General.GetNullableInteger(txtExpiryYNText.Text)
                                 );
                // InsertDocumentOther(
                // txtcode.Text,
                // txtdocname.Text,
                //(chkActiveYesOrNo.Checked == true) ? 1 : 0,
                //General.GetNullableInteger(ucGroupAdd.SelectedHard),
                //(chkhavingexpyn.Checked == true) ? 1 : 0,
                //(chkShortExpiryAdd.Checked == true) ? 1 : 0,
                // (chkNokYnAdd.Checked == true) ? 1 : 0,
                // General.GetNullableInteger(ddlStage.SelectedValue),
                // General.GetNullableInteger(chkMandatoryYNEdit.Checked == true ? "1" : "0"),
                // General.GetNullableInteger(chkWaiverYN.Checked == true ? "1" : "0"),
                // General.GetNullableString(UserGroupList),
                // General.GetNullableInteger(ucCategory.SelectedDocumentCategoryID),
                // General.GetNullableInteger(chkAdditionDocYnAdd.Checked == true ? "1" : "0"),
                // General.GetNullableInteger(chkAuthReqYnAdd.Checked == true ? "1" : "0"),
                // General.GetNullableInteger(chkCBAOtherDocumentAdd.Checked == true ? "1" : "0"),
                // (chkShowInMasterChecklistYNAdd.Checked == true) ? 1 : 0,
                // (chkPhotocopyAcceptableYnAdd.Checked == true) ? 1 : 0
                // ,resultdocumentid);

                ViewState["DocumentId"] = int.Parse(resultdocumentid.ToString());

                imgRankPicklist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterRankList.aspx?OtherDocumentId=" + ViewState["DocumentId"] + "&type=OTHER" + "',false,600,400); return false;");
                imgvessseltypelist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterVesselTypeList.aspx?OtherDocumentId=" + ViewState["DocumentId"] + "&type=OTHER" + "',false,600,400); return false;");
                imgflagpicklist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterFlagList.aspx?OtherDocumentId=" + ViewState["DocumentId"] + "&type=OTHER" + "',false,600,400); return false;");
                imgcharterer.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterChartererList.aspx?OtherDocumentId=" + ViewState["DocumentId"] + "&type=OTHER" + "',false,600,400); return false;");
                imgcompany.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterManagementCompanyList.aspx?OtherDocumentId=" + ViewState["DocumentId"] + "&type=OTHER" + "',false,600,400); return false;");
                imgowner.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentOwnerList.aspx?OtherDocumentId=" + ViewState["DocumentId"] + "&type=OTHER" + "',false,600,400); return false;");
                imgusergroup.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentUserWaiverList.aspx?OtherDocumentId=" + ViewState["DocumentId"] + "&type=OTHER" + "',false,600,400); return false;");


                //  ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptClosePopup, true);
            }
        }
    }
    private void UpdateDocumentOther(int documentid, string documentcode, string documentname, int? localactive, int? group, int? havingexpiry, int? ShortExpiry, int? nokyn
         , int? stage, int? offshoremandatory, int? waiver, string waiverusergroup, int? categoryid, int? additionDocYN, int? authenticationYN, int? CBAOtherDocYN
         , int? showinmasterchecklistyn, int? photocopyacceptableyn, int? txtExpiryYNText)
    {
        if (!IsValidDocumentOther(documentname, waiver, waiverusergroup))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersDocumentOther.UpdateDocumentOther(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , documentid, documentcode, documentname, localactive, group, havingexpiry, ShortExpiry, nokyn, stage, offshoremandatory, waiver, waiverusergroup, additionDocYN, authenticationYN, categoryid, CBAOtherDocYN
            , showinmasterchecklistyn, photocopyacceptableyn
             , null//eral.GetNullableString(General.RadGetComboboxCheckList(ddlflag))
             , null//eral.GetNullableString(General.RadGetComboboxCheckList(chkChartererList))
             , null//eral.GetNullableString(General.RadGetComboboxCheckList(ddlcompany))
             , null//eral.GetNullableString(General.RadGetComboboxCheckList(chkRankList))
             , null//eral.GetNullableString(General.RadGetComboboxCheckList(chkVesselTypeList))
            , txtExpiryYNText
            );
    }          
    //private void InsertDocumentOther(string documentcode, string documentname, int? localactive, int? group, int? havingexpiry, int? ShortExpiry, int? nokyn
    //       , int? stage, int? offshoremandatory, int? waiver, string waiverusergroup, int? categoryid, int? additionDocYN, int? authenticationYN, int? CBAOtherDocYN
    //       , int? showinmasterchecklistyn, int? photocopyacceptableyn, int? resultdocumentid)
    //{
    //    if (!IsValidDocumentOther(documentname, waiver, waiverusergroup))
    //    {
    //        ucError.Visible = true;
    //        return;
    //    }
       
    //    PhoenixRegistersDocumentOther.InsertDocumentOther(
    //        PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //        , documentcode, documentname, localactive, group, havingexpiry, ShortExpiry, nokyn, stage, offshoremandatory, waiver, waiverusergroup, additionDocYN, authenticationYN, categoryid, CBAOtherDocYN
    //        , showinmasterchecklistyn, photocopyacceptableyn
    //         , General.GetNullableString(General.RadGetComboboxCheckList(ddlflag))
    //         , General.GetNullableString(General.RadGetComboboxCheckList(chkChartererList))
    //         , General.GetNullableString(General.RadGetComboboxCheckList(ddlcompany))
    //         , General.GetNullableString(General.RadGetComboboxCheckList(chkRankList))
    //         , General.GetNullableString(General.RadGetComboboxCheckList(chkVesselTypeList))
    //         ,ref resultdocumentid
    //        );
    //}
    private bool IsValidDocumentOther(string documentname, int? waiveryn, string usergroup)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (documentname.Trim().Equals(""))
            ucError.ErrorMessage = "Document Name name required.";

        //if (waiveryn != null && waiveryn.Equals("1"))
        //{
        //    if (General.GetNullableInteger(usergroup) == null)
        //        ucError.ErrorMessage = "User group is required.";
        //}
        if(ucGroupAdd.SelectedHard !=null && ucGroupAdd.SelectedHard == "")
            ucError.ErrorMessage = "Group is required.";
        if (chkhavingexpyn.Checked == true && txtExpiryYNText.Text.Equals(""))
            ucError.ErrorMessage = "Expiry Months is required.";
        return (!ucError.IsError);
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

    protected void ucCategory_TextChangedEvent(object sender, EventArgs e)
    {
        if (ucCategory.SelectedValue.ToString() == "2") //STCW
        {

            lblCharterer.Visible = false;
            imgcharterer.Visible = false;
            lblflag.Visible = false;
            imgflagpicklist.Visible = false;
            RadLabel1.Visible = false;
            imgcompany.Visible = false;
            lblown.Visible = false;
            imgowner.Visible = false;
           

        }
        else if (ucCategory.SelectedValue.ToString() == "3") //COMPANY REQUIREMENT
        {

            lblCharterer.Visible = false;
            imgcharterer.Visible = false;
            lblflag.Visible = false;
            imgflagpicklist.Visible = false;
            RadLabel1.Visible = true;
            imgcompany.Visible = true;
            lblown.Visible = true;
            imgowner.Visible = true;
           

        }


        else if (ucCategory.SelectedValue.ToString() == "4") //CHARTERER
        {

            lblCharterer.Visible = true;
            imgcharterer.Visible = true;
            lblflag.Visible = false;
            imgflagpicklist.Visible = false;
            RadLabel1.Visible = false;
            imgcompany.Visible = false;
            lblown.Visible = true;
            imgowner.Visible = true;
          

        }
        else if (ucCategory.SelectedValue.ToString() == "5") //Flag State Documents
        {

            lblCharterer.Visible = false;
            imgcharterer.Visible = false;
            lblflag.Visible = true;
            imgflagpicklist.Visible = true;
            RadLabel1.Visible = false;
            imgcompany.Visible = false;
            lblown.Visible = false;
            imgowner.Visible = false;
          

        }
        else if (ucCategory.SelectedValue.ToString() == "6") //	Owner Requirement
        {

            lblCharterer.Visible = false;
            imgcharterer.Visible = false;
            lblflag.Visible = false;
            imgflagpicklist.Visible = false;
            RadLabel1.Visible = false;
            imgcompany.Visible = false;
            lblown.Visible = true;
            imgowner.Visible = true;
           

        }
        else
        {

            lblCharterer.Visible = false;
            imgcharterer.Visible = false;
            lblflag.Visible = false;
            imgflagpicklist.Visible = false;
            RadLabel1.Visible = false;
            imgcompany.Visible = false;
            lblown.Visible = false;
            imgowner.Visible = false;
            

        }
    }

    protected void chkhavingexpyn_CheckedChanged(object sender, EventArgs e)
    {
        if (chkhavingexpyn.Checked == true)
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