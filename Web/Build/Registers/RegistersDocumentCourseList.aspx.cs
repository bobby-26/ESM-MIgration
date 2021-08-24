using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;

public partial class RegistersDocumentCourseList : PhoenixBasePage
{
    string documentno = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

        if (!IsPostBack)
        {
            //chkUserGroup.DataSource = SessionUtil.UserGroupList();
            //chkUserGroup.DataTextField = "FLDGROUPNAME";
            //chkUserGroup.DataValueField = "FLDGROUPCODE";
            //chkUserGroup.DataBind();
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
            ViewState["CBT"] = "";

            if (Request.QueryString["CBT"] != null && Request.QueryString["CBT"].ToString() != "")
                ViewState["CBT"] = 1;

            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE") && ViewState["CBT"].ToString() == "")
            {
                ucDocumentType.ShortNameFilter = "0,1,2,3,5,6,7,8,9,10";
                ucTobedoneby.Visible = false;
                lbltobedoneby.Visible = false;
            }
            else if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE") && ViewState["CBT"].ToString() != "")
            {
                ucDocumentType.ShortNameFilter = "4";
                lblMapInCompetenceSubcategoryYN.Visible = false;
                chkMapinCompetencesubcategoryYN.Visible = false;
                ucTobedoneby.Visible = false;
                lbltobedoneby.Visible = false;
            }
            else
                ucDocumentType.ShortNameFilter = "0,1,2,3,4,5,6,7,8,9,10";

            ucDocumentType.bind();
            ucTobedoneby.TypeOfTraining = PhoenixCrewOffshoreTrainingNeeds.ListTypeOfTraining(129, "TRA");
            ucTobedoneby.bind();

            BindRankList();
            BindVesselTypeList();
            BindChartererList();
            BindManagementList();
            BindCategory();
            BindSubCategory("");
            BindOffshoreStages(ddlStage);
            BindAlternateCourse();
            BindFlagList();
            BindTrainingType();
            BindAcceptInstituteType();

            if (Request.QueryString["DocumentCourseId"] != null)
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                ViewState["DocumentCourseId"] = Request.QueryString["DocumentCourseId"].ToString();

                DocumentCourseEdit(Int32.Parse(Request.QueryString["DocumentCourseId"].ToString()));

                imgRankPicklist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterRankList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                imgvessseltypelist.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterVesselTypeList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                imgflagpicklist.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterFlagList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                imgcharterer.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterChartererList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                imgcompany.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterManagementCompanyList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                imgowner.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentOwnerList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                imgalternate.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentAlternateCourseList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                imgusergroup.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentUserWaiverList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                imgcomponent.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentGlobalComponent.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                imgaccinst.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentAcceptInstituteList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                lmgapplicablevessel.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegistersCourseVesselList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                imgflgendorsment.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentFlagEndorsementList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");

            }
            else
            {
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
            }
            MenuDoumentCourseList.AccessRights = this.ViewState;
            MenuDoumentCourseList.MenuList = toolbar.Show();
        }
        //PhoenixQuickTypeCode
        //if(chkAccpInst.SelectedValue)
    }

    public void BindTrainingType()
    {
        DataSet ds = PhoenixRegistersQuick.ListQuick(1, 192);
       
        chktrainingtypeList.DataTextField = "FLDQUICKNAME";
        chktrainingtypeList.DataValueField = "FLDQUICKCODE";
        chktrainingtypeList.DataSource = ds;
        chktrainingtypeList.DataBind();

    }
    public void BindAcceptInstituteType()
    {
        DataSet ds = PhoenixRegistersQuick.ListQuick(1, 193);
        chkAccpInst.Items.Insert(0, new RadComboBoxItem("--Select--"));
        chkAccpInst.DataTextField = "FLDQUICKNAME";
        chkAccpInst.DataValueField = "FLDQUICKCODE";

       
        chkAccpInst.DataSource = ds;
        chkAccpInst.DataBind();

        //DataTable dt = PhoenixRegisterCrewList.ActiveInstituteList();

        //chkAccpInst.DataTextField = "FLDNAME";
        //chkAccpInst.DataValueField = "FLDADDRESSCODE";
        //chkAccpInst.DataSource = dt;
        //chkAccpInst.DataBind();

    }

    protected void BindOffshoreStages(RadComboBox ddl)
    {
        ddl.Items.Clear();
        ddl.DataSource = PhoenixRegistersDocumentCourse.ListOffshoreStage(null, null);
        ddl.DataTextField = "FLDSTAGE";
        ddl.DataValueField = "FLDSTAGEID";
        ddl.DataBind();
        ///  ddl.Items.Insert(0, new ListItem("--Select--", ""));
    }
    protected void BindAlternateCourse()
    {
        //cblAlternateCourse.Items.Clear();
        //cblAlternateCourse.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(null);
        //cblAlternateCourse.DataTextField = "FLDDOCUMENTNAME";
        //cblAlternateCourse.DataValueField = "FLDDOCUMENTID";
        //cblAlternateCourse.DataBind();
    }
    protected void chkMandatoryYN_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox cb = (RadCheckBox)sender;

        if (chkWaiverYN != null)
        {
            if (cb.Checked == true)
            {
                chkWaiverYN.Enabled = true;
            }
            else
            {
                chkWaiverYN.Checked = false;
                chkWaiverYN.Enabled = false;

                imgusergroup.Enabled = false;

                //chkUserGroup.SelectedIndex = -1;
                //chkUserGroup.Enabled = false;
            }
        }
    }
    protected void chkWaiverYN_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox cb = (RadCheckBox)sender;
        //if (chkUserGroup != null)
        //{
        if (cb.Checked == true)
        {
            imgusergroup.Enabled = true;
        }
        else
        {
            // chkUserGroup.SelectedIndex = -1;
            imgusergroup.Enabled = false;
        }
        // }
    }


    protected void DoumentCourseList_TabStripCommand(object sender, EventArgs e)
    {
        String scriptClosePopup = String.Format("javascript:fnReloadList('codehelp1');");

        String scriptKeepPopupOpen = String.Format("javascript:fnReloadList('codehelp1', null, 'yes');");
       
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                //if (IsValidDocumentCourse(strdeparment.ToString(), strlevel.ToString(), strrank.ToString(), strvessel.ToString()))
                if (IsValidDocumentCourse())
                {
                    DocumentNumber();

                    //string UGList = "";
                    //string UserGroupList = "";

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

                    //string RankList = "";
                    //foreach (RadComboBoxItem li in chkRankList.Items)
                    //{
                    //    if (li.Checked)
                    //    {
                    //        RankList += li.Value + ",";
                    //    }
                    //}

                    //if (RankList != "")
                    //{
                    //    RankList = "," + RankList;
                    //}

                    //string VesselTypeList = "";
                    //foreach (RadComboBoxItem li in chkVesselTypeList.Items)
                    //{
                    //    if (li.Checked)
                    //    {
                    //        VesselTypeList += li.Value + ",";
                    //    }
                    //}

                    //if (VesselTypeList != "")
                    //{
                    //    VesselTypeList = "," + VesselTypeList;
                    //}

                    //string ChartererList = "";
                    //foreach (RadComboBoxItem li in chkChartererList.Items)
                    //{
                    //    if (li.Checked)
                    //    {
                    //        ChartererList += li.Value + ",";
                    //    }
                    //}

                    //if (ChartererList != "")
                    //{
                    //    ChartererList = "," + ChartererList;
                    //}
                    //string FlagList = "";
                    //foreach (RadComboBoxItem li in ddlflag.Items)
                    //{
                    //    if (li.Checked)
                    //    {
                    //        FlagList += li.Value + ",";
                    //    }
                    //}

                    //if (FlagList != "")
                    //{
                    //    FlagList = "," + FlagList;
                    //}

                    string TrainingTypeList = "";
                    foreach (RadComboBoxItem li in chktrainingtypeList.Items)
                    {
                        if (li.Checked)
                        {
                            TrainingTypeList += li.Value + ",";
                        }
                    }

                    if (TrainingTypeList != "")
                    {
                        TrainingTypeList = "," + TrainingTypeList;
                    }

                    if (ViewState["DocumentCourseId"] != null)
                    {
                        PhoenixRegistersDocumentCourse.UpdateDocumentCourse(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , Int16.Parse(ViewState["DocumentCourseId"].ToString())
                            , Int16.Parse(ucDocumentType.SelectedHard)
                            , txtCourse.Text
                            , General.GetNullableInteger(chkActiveYesOrNo.Checked == true ? "1" : "0")
                            , documentno
                            , null
                            , null
                            , General.GetNullableInteger(chkExpiry.Checked == true ? "1" : "0")
                            , txtAbbreviation.Text
                            , General.GetNullableInteger(chkMandatory.Checked == true ? "1" : "0")
                            , General.GetNullableString(txtCBTCourse.Text)
                            , null //General.GetNullableString(RankList)
                            , null//General.GetNullableString(VesselTypeList)
                            , null //, General.GetNullableString(strprincipal.ToString())
                            , null //, General.GetNullableString(strvessel.ToString())
                            , null //, General.GetNullableString(strdeparment.ToString())
                            , null //, General.GetNullableString(strlevel.ToString())                           
                            , General.GetNullableInteger(ddlSource.SelectedHard)
                            , null //, General.GetNullableString(strrankend.ToString())
                            , General.GetNullableInteger(ddlCourseOfficer.SelectedValue)
                            , null //General.GetNullableInteger(ucDepCourse.SelectedCourse)
                            , General.GetNullableInteger(ucLevel.SelectedHard)
                            , General.GetNullableInteger(ucMaker.SelectedAddress)
                            , General.GetNullableInteger(ddlStage.SelectedValue)
                            , General.GetNullableInteger(chkMandatoryYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkWaiverYN.Checked == true ? "1" : "0")
                            , null//General.GetNullableString(UserGroupList)
                            , General.GetNullableInteger(chkAdditionalDocYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkAuthenticationReqYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(ucCategory.SelectedDocumentCategoryID)
                            , null//General.GetNullableString(General.RadGetComboboxCheckList(cblAlternateCourse))
                            , General.GetNullableInteger(ddlCategory.SelectedValue)
                            , General.GetNullableInteger(ddlSubCategory.SelectedValue)
                            , null//General.GetNullableString(ChartererList)
                            , General.GetNullableInteger(ucTobedoneby.SelectedToBeDoneBy)
                            , General.GetNullableInteger(chkShowinMasterChecklistYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkPhotocopyAcceptableYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkMapinCompetencesubcategoryYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(ViewState["CBT"].ToString())
                            , General.GetNullableInteger(chkSingleUse.Checked == true ? "1" : "0")
                            , null//General.GetNullableString(General.RadGetComboboxCheckList(ddlflag))
                            , null//General.GetNullableString(General.RadGetComboboxCheckList(ddlcompany))
                             , General.GetNullableInteger(ucNumberAmount.Text)
                            , TrainingTypeList
                            , General.GetNullableInteger(chkAccpInst.SelectedValue.ToString())
                            , General.GetNullableInteger(chkcertreqyn.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkFlagEndorsement.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkWaivetoNextStage.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkSeaServiceAcceptedLieu.Checked == true ? "1" : "0")
                            , General.GetNullableString(txtSeaServiceDetails.Text)
                            , General.GetNullableInteger(txtExpiryYNText.Text)
                            , General.GetNullableInteger(chkCRAAccept.Checked == true ? "1" : "0")

                            );
                        ucStatus.Text = "Course information updated";
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptClosePopup, true);
                    }
                    else
                    {
                        int? resultdocumentid = null;
                        PhoenixRegistersDocumentCourse.InsertDocumentCourse(
                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                            , Int32.Parse(ucDocumentType.SelectedHard)
                            , txtCourse.Text
                            , General.GetNullableInteger(chkActiveYesOrNo.Checked == true ? "1" : "0")
                            , documentno
                             , null
                            , null
                            , General.GetNullableInteger(chkExpiry.Checked == true ? "1" : "0")
                            , txtAbbreviation.Text
                            , General.GetNullableInteger(chkMandatory.Checked == true ? "1" : "0")
                            , General.GetNullableString(txtCBTCourse.Text)
                            , null//General.GetNullableString(RankList)
                            , null//General.GetNullableString(VesselTypeList)
                            , null //, General.GetNullableString(strprincipal.ToString())
                            , null //, General.GetNullableString(strvessel.ToString())
                            , null //, General.GetNullableString(strdeparment.ToString())
                            , null //, General.GetNullableString(strlevel.ToString())
                            , General.GetNullableInteger(ddlSource.SelectedHard)
                            , null //, General.GetNullableString(strrankend.ToString())
                            , General.GetNullableInteger(ddlCourseOfficer.SelectedValue)
                            , null //General.GetNullableInteger(ucDepCourse.SelectedCourse)
                            , General.GetNullableInteger(ucLevel.SelectedHard)
                            , General.GetNullableInteger(ucMaker.SelectedAddress)
                            , General.GetNullableInteger(ddlStage.SelectedValue)
                            , General.GetNullableInteger(chkMandatoryYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkWaiverYN.Checked == true ? "1" : "0")
                            ,null// General.GetNullableString(UserGroupList)
                            , General.GetNullableInteger(chkAdditionalDocYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkAuthenticationReqYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(ucCategory.SelectedDocumentCategoryID)
                            , null//General.GetNullableString(General.RadGetComboboxCheckList(cblAlternateCourse))
                            , General.GetNullableInteger(ddlCategory.SelectedValue)
                            , General.GetNullableInteger(ddlSubCategory.SelectedValue)
                            , null//General.GetNullableString(ChartererList)
                            , General.GetNullableInteger(ucTobedoneby.SelectedToBeDoneBy)
                            , General.GetNullableInteger(chkShowinMasterChecklistYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkPhotocopyAcceptableYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkMapinCompetencesubcategoryYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkSingleUse.Checked == true ? "1" : "0")
                            , null//General.GetNullableString(General.RadGetComboboxCheckList(ddlflag))
                            , null//General.GetNullableString(General.RadGetComboboxCheckList(ddlcompany))
                            , ref resultdocumentid
                            , General.GetNullableInteger(ucNumberAmount.Text)
                            , TrainingTypeList
                            , General.GetNullableInteger(chkAccpInst.SelectedValue.ToString())
                            , General.GetNullableInteger(chkcertreqyn.Checked == true ? "1" : "0")        
                            , General.GetNullableInteger(chkFlagEndorsement.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkWaivetoNextStage.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkSeaServiceAcceptedLieu.Checked == true ? "1" : "0")
                            , General.GetNullableString(txtSeaServiceDetails.Text)
                            , General.GetNullableInteger(txtExpiryYNText.Text)
                            , General.GetNullableInteger(chkCRAAccept.Checked == true ? "1" : "0")
                            );
                        ViewState["DocumentCourseId"] = int.Parse(resultdocumentid.ToString());

                        imgRankPicklist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterRankList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                        imgvessseltypelist.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterVesselTypeList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                        imgflagpicklist.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterFlagList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                        imgcharterer.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterChartererList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                        imgcompany.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterManagementCompanyList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                        imgowner.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentOwnerList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                        imgalternate.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentAlternateCourseList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                        imgusergroup.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentUserWaiverList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                        lmgapplicablevessel.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegistersCourseVesselList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");

                        // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptClosePopup, true);

                        //  Reset();
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
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        // DocumentCourseEdit(Int32.Parse(ViewState["DocumentCourseId"].ToString()));
        if (ViewState["DocumentCourseId"] != null && ViewState["DocumentCourseId"].ToString() != "")
        {
            DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(Int32.Parse(ViewState["DocumentCourseId"].ToString()));
            if (ds.Tables[0].Rows[0]["FLDVESSELMODIFIEDYN"].ToString() == "1")
                lnkvesselmodify.Visible = true;
            else
                lnkvesselmodify.Visible = false;
        }
    }
    protected void EnableCBT(object sender, EventArgs args)
    {
        if (ucDocumentType.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 103, "4"))
        {
            txtCBTCourse.Enabled = true;
            txtCBTCourse.CssClass = "input";
        }
        else if (ucDocumentType.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 103, "2"))
        {
            ddlSource.CssClass = "dropdown_mandatory";
        }
        else
        {
            txtCBTCourse.Enabled = false;
            txtCBTCourse.CssClass = "readonlytextbox";
            ddlSource.CssClass = "input";
        }
    }
    protected void DocumentNumber()
    {
        string source = "";
        if (!string.IsNullOrEmpty(ddlSource.SelectedHard))
            source = ddlSource.SelectedName;
        documentno = source;
    }
    //private bool IsValidDocumentCourse(string department, string level, string rank, string vessel)
    private bool IsValidDocumentCourse()
    {
        Int32 result;

        ucError.HeaderMessage = "Please provide the following required information";

        if (txtCourse.Text.Equals(""))
            ucError.ErrorMessage = "Course Name is required.";
        if (txtAbbreviation.Text.Equals(""))
            ucError.ErrorMessage = "Abbreviation  is required.";
        if (!Int32.TryParse(ucDocumentType.SelectedHard, out result))
            ucError.ErrorMessage = "Document Type is required.";
        if (ucDocumentType.SelectedHard != "Dummy" && ucDocumentType.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 103, "2"))
        {
            if (!Int32.TryParse(ddlSource.SelectedHard, out result))
                ucError.ErrorMessage = "Source is required.";
        }
        //if (General.GetNullableInteger(chkWaiverYN.Checked == true ? "1" : "0") != null && chkWaiverYN.Checked == true)
        //{
        //    if (chkUserGroup.CheckedItems.Count == 0)
        //        ucError.ErrorMessage = "User group is required.";
        //}
        if (chkExpiry.Checked == true && txtExpiryYNText.Text.Equals(""))
            ucError.ErrorMessage = "Expiry Months is required.";
        return (!ucError.IsError);
    }

    private void Reset()
    {
        ViewState["DocumentCourseId"] = null;
        ucDocumentType.SelectedHard = "";

        txtCourse.Text = "";
        chkActiveYesOrNo.Checked = false;
        chkExpiry.Checked = false;
        chkMandatory.Checked = false;
        txtLevel.Text = "";
        txtAbbreviation.Text = "";
        ddlSource.SelectedHard = "";
        ucLevel.SelectedHard = "";
        // ucDepCourse.SelectedCourse = "";
        ucMaker.SelectedAddress = "";
        txtCBTCourse.Text = "";
        ddlStage.SelectedValue = "";
        chkMandatoryYN.Checked = false;
        chkWaiverYN.Checked = false;
       // chkUserGroup.SelectedIndex = -1;
    }

    private void DocumentCourseEdit(int documentcourseid)
    {
        DataSet ds = PhoenixRegistersDocumentCourse.EditDocumentCourse(documentcourseid);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow dr = ds.Tables[0].Rows[0];
            if (dr["FLDDOCUMENTTYPE"].ToString() != "0")
            {
                ucDocumentType.SelectedHard = dr["FLDDOCUMENTTYPE"].ToString();

                txtCourse.Text = dr["FLDCOURSE"].ToString();
                chkActiveYesOrNo.Checked = dr["FLDLOCALACTIVE"].ToString() == "1" ? true : false;
                chkExpiry.Checked = dr["FLDEXPIRY"].ToString() == "1" ? true : false;
                chkMandatory.Checked = dr["FLDMANDATORY"].ToString() == "1" ? true : false;
                txtLevel.Text = dr["FLDLEVEL"].ToString();
                txtAbbreviation.Text = dr["FLDABBREVIATION"].ToString();
                txtCBTCourse.Text = dr["FLDCBTCOURSE"].ToString();
                ddlSource.SelectedHard = dr["FLDSOURCE"].ToString();
                ddlCourseOfficer.SelectedValue = dr["FLDOFFICESTAFFID"].ToString();
                //ucDepCourse.SelectedCourse = dr["FLDDEPENDENTCOURSEID"].ToString();
                ucLevel.SelectedHard = dr["FLDDEPENDENTTYPE"].ToString();
                ucMaker.SelectedAddress = dr["FLDMAKERTYPE"].ToString();
                txtCreatedBy.Text = dr["FLDCREATEDBY"].ToString();
                txtUpdatedBy.Text = dr["FLDMODIFIEDBY"].ToString();
                txtCreatedDate.Text = string.Format("{0:dd/MMM/yyyy}", dr["FLDCREATEDDATE"]);
                txtUpdatedDate.Text = string.Format("{0:dd/MMM/yyyy}", dr["FLDMODIFIEDDATE"]);
                ucCategory.SelectedDocumentCategoryID = dr["FLDDOCUMENTCATEGORYID"].ToString();

                if (dr["FLDVESSELMODIFIEDYN"].ToString() == "1")
                    lnkvesselmodify.Visible = true;
                else
                    lnkvesselmodify.Visible = false;

                if (dr["FLDDOCUMENTCATEGORYID"].ToString() == "2") //STCW
                {                    
                    lblCharterer.Visible = false;
                    imgcharterer.Visible = false;
                    lblflag.Visible = false;
                    imgflagpicklist.Visible = false;
                    RadLabel1.Visible = false;
                    imgcompany.Visible = false;
                    RadLabel2.Visible = false;
                    imgowner.Visible = false;
                    RadLabel3.Visible = false;
                    imgcomponent.Visible = false;                  
                }
                else if (dr["FLDDOCUMENTCATEGORYID"].ToString() == "3") //COMPANY REQUIREMENT
                {                                     
                    lblCharterer.Visible = false;
                    imgcharterer.Visible = false;
                    lblflag.Visible = false;
                    imgflagpicklist.Visible = false;
                    RadLabel1.Visible = true;
                    imgcompany.Visible = true;
                    RadLabel2.Visible = true;
                    imgowner.Visible = true;
                    RadLabel3.Visible = true;
                    imgcomponent.Visible = true;                  
                }
                else if (dr["FLDDOCUMENTCATEGORYID"].ToString() == "4") //CHARTERER
                {                    
                    lblCharterer.Visible = true;
                    imgcharterer.Visible = true;
                    lblflag.Visible = false;
                    imgflagpicklist.Visible = false;
                    RadLabel1.Visible = false;
                    imgcompany.Visible = false;
                    RadLabel2.Visible = true;
                    imgowner.Visible = true;
                    RadLabel3.Visible = true;
                    imgcomponent.Visible = true;                   
                }
                else if(dr["FLDDOCUMENTCATEGORYID"].ToString() == "5") //Flag State Documents
                {                  
                    lblCharterer.Visible = false;
                    imgcharterer.Visible = false;
                    lblflag.Visible = true;
                    imgflagpicklist.Visible = true;
                    RadLabel1.Visible = false;
                    imgcompany.Visible = false;
                    RadLabel2.Visible = false;
                    imgowner.Visible = false;
                    RadLabel3.Visible = false;
                    imgcomponent.Visible = false;                  
                }
                else if (dr["FLDDOCUMENTCATEGORYID"].ToString() == "6") //	Owner Requirement
                {
                    lblCharterer.Visible = false;
                    imgcharterer.Visible = false;
                    lblflag.Visible = false;
                    imgflagpicklist.Visible = false;
                    RadLabel1.Visible = false;
                    imgcompany.Visible = false;
                    RadLabel2.Visible = true;
                    imgowner.Visible = true;
                    RadLabel3.Visible = true;
                    imgcomponent.Visible = true;
                }
                else
                {                  
                    lblCharterer.Visible = false;
                    imgcharterer.Visible = false;
                    lblflag.Visible = false;
                    imgflagpicklist.Visible = false;
                    RadLabel1.Visible = false;
                    imgcompany.Visible = false;
                    RadLabel2.Visible = false;
                    imgowner.Visible = false;
                    RadLabel3.Visible = false;
                    imgcomponent.Visible = false;                   
                }

                chkAdditionalDocYN.Checked = dr["FLDADDITIONALDOCYN"].ToString() == "1" ? true : false;
                chkAuthenticationReqYN.Checked = dr["FLDAUTHENTICATIONREQYN"].ToString() == "1" ? true : false;

                chkSingleUse.Checked = dr["FLDSINGLEUSEYN"].ToString() == "1" ? true : false;

                ucNumberAmount.Text= dr["FLDNUMOFDAYS"].ToString();
                chkcertreqyn.Checked= dr["FLDCERTIFICATEYN"].ToString() == "1" ? true : false;
                chkAccpInst.SelectedValue = dr["FLDAPPLICABLEINSTITUTE"].ToString();
                General.RadBindComboBoxCheckList(chktrainingtypeList, dr["FLDTRAININGTYPE"].ToString());

                string[] dept = dr["FLDDEPARTMENT"].ToString().Split(',');
                string[] level = dr["FLDLEVELTYPE"].ToString().Split(',');
                string[] rank = dr["FLDRANK"].ToString().Split(',');
                string[] vesseltype = dr["FLDVESSELTYPE"].ToString().Split(',');
                string[] principal = dr["FLDPRINCIPAL"].ToString().Split(',');
                string[] vessel = dr["FLDVESSEL"].ToString().Split(',');
                string[] rankend = dr["FLDENDORSEMENTRANKS"].ToString().Split(',');
                if (dr["FLDDOCUMENTTYPE"].ToString() == PhoenixCommonRegisters.GetHardCode(1, 102, "4"))
                {
                    txtCBTCourse.Enabled = true;
                    txtCBTCourse.CssClass = "input";
                }
                else
                {
                    txtCBTCourse.Enabled = false;
                    txtCBTCourse.CssClass = "readonlytextbox";
                }
                //chkUserGroup.DataSource = SessionUtil.UserGroupList();
                //chkUserGroup.DataTextField = "FLDGROUPNAME";
                //chkUserGroup.DataValueField = "FLDGROUPCODE";
                //chkUserGroup.DataBind();

                ddlStage.SelectedValue = dr["FLDSTAGEID"].ToString();
                chkMandatoryYN.Checked = dr["FLDMANDATORYYN"].ToString() == "1" ? true : false;
                chkWaiverYN.Checked = dr["FLDWAIVERYN"].ToString() == "1" ? true : false;
                chkCRAAccept.Checked = dr["FLDCRAACCEPTYN"].ToString() == "1" ? true : false;

                
                //ucUserGroup.SelectedUserGroup = dr["FLDUSERGROUPTOALLOWWAIVER"].ToString();
                //General.RadBindComboBoxCheckList(cblAlternateCourse, dr["FLDALTERNATECOURSE"].ToString());
                // General.RadBindComboBoxCheckList(chkUserGroup, dr["FLDUSERGROUPTOALLOWWAIVER"].ToString());
                // General.RadBindComboBoxCheckList(chkRankList, dr["FLDRANK"].ToString());
                //General.RadBindComboBoxCheckList(chkVesselTypeList, dr["FLDVESSELTYPE"].ToString());
                //General.RadBindComboBoxCheckList(chkChartererList, dr["FLDCHARTERER"].ToString());
                //General.RadBindComboBoxCheckList(ddlflag, dr["FLDFLAG"].ToString());
                //General.RadBindComboBoxCheckList(ddlcompany, dr["FLDCOMPANIES"].ToString());

                ddlCategory.SelectedValue = dr["FLDCATEGORYID"].ToString();
                BindSubCategory(dr["FLDCATEGORYID"].ToString());
                ddlSubCategory.SelectedValue = dr["FLDSUBCATEGORYID"].ToString();

                if (dr["FLDTOBEDONEBY"].ToString() != "")
                    ucTobedoneby.SelectedToBeDoneBy = dr["FLDTOBEDONEBY"].ToString();

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
                chkMapinCompetencesubcategoryYN.Checked = dr["FLDSHOWINSUBCATEGORYYN"].ToString() == "1" ? true : false;

                if (chkAccpInst.SelectedIndex>=0 && chkAccpInst.SelectedItem.Text == "Restricted Institute")
                {
                    imgaccinst.Visible = true;
                }
                else
                {
                    imgaccinst.Visible = false;
                }
                chkFlagEndorsement.Checked = dr["FLDFLAGENDORSEMENTYN"].ToString() == "1" ? true : false;
                chkWaivetoNextStage.Checked = dr["FLDWAIVETONEXTSTAGEYN"].ToString() == "1" ? true : false;
                chkSeaServiceAcceptedLieu.Checked = dr["FLDSEASERVICEACCEPTED"].ToString() == "1" ? true : false;
                if (chkSeaServiceAcceptedLieu.Checked == true)
                    txtSeaServiceDetails.Visible = true;
                else
                    txtSeaServiceDetails.Visible = false;
                txtSeaServiceDetails.Text = dr["FLDSEASERVICEDETAILS"].ToString();

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
    }

    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSubCategory != null)
        {
            if (General.GetNullableInteger(ddlCategory.SelectedValue).HasValue)
                BindSubCategory(ddlCategory.SelectedValue);
        }
    }

    protected void BindCategory()
    {
        DataSet ds = PhoenixRegistersDocumentCourse.ListTrainingCategory(null);
        ddlCategory.DataTextField = "FLDCATEGORYNAME";
        ddlCategory.DataValueField = "FLDCATEGORYID";
        ddlCategory.DataSource = ds;
        ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        ddlCategory.DataBind();
    }

    protected void BindSubCategory(string categoryid)
    {
        ddlSubCategory.Items.Clear();
        ddlSubCategory.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
        if (categoryid != "")
        {
            DataTable dt = PhoenixRegistersDocumentCourse.ListTrainingSubCategory(General.GetNullableInteger(categoryid));
            ddlSubCategory.DataTextField = "FLDNAME";
            ddlSubCategory.DataValueField = "FLDSUBCATEGORYID";
            ddlSubCategory.DataSource = dt;
            ddlSubCategory.DataBind();
        }
    }

    private void BindRankList()
    {
        //DataSet ds = PhoenixRegistersHard.ListHard(1, 90, byte.Parse("0"), "SOF,JDO,JEO,DER,WFP,COK,SMM,DTR,ETR,ELO");
        //chkRankList.DataSource = ds;
        //chkRankList.DataTextField = "FLDHARDNAME";
        //chkRankList.DataValueField = "FLDHARDCODE";
        //chkRankList.DataBind();
    }

    private void BindVesselTypeList()
    {
        //DataSet ds = PhoenixRegistersHard.ListHard(1, 81);
        //chkVesselTypeList.DataSource = ds;
        //chkVesselTypeList.DataTextField = "FLDHARDNAME";
        //chkVesselTypeList.DataValueField = "FLDHARDCODE";
        //chkVesselTypeList.DataBind();
    }

    private void BindChartererList()
    {
        //DataSet ds = PhoenixCrewOffshoreTrainingMatrixStandard.TrainingMatrixStandardList();
        //chkChartererList.DataSource = ds;
        //chkChartererList.DataTextField = "FLDNAME";
        //chkChartererList.DataValueField = "FLDADDRESSCODE";
        //chkChartererList.DataBind();
    }
    private void BindManagementList()
    {
        //DataSet ds = PhoenixRegistersAddress.ListAddress(((int)PhoenixAddressType.MANAGER).ToString());
        //ddlcompany.DataSource = ds;
        //ddlcompany.DataTextField = "FLDNAME";
        //ddlcompany.DataValueField = "FLDADDRESSCODE";
        //ddlcompany.DataBind();
    }
    private void BindFlagList()
    {
        //ddlflag.DataSource = PhoenixRegistersFlag.ListFlag(null);
        //ddlflag.DataBind();

    }


    protected void chkAccpInst_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        if (chkAccpInst.SelectedItem.Text == "Restricted Institute")
        {
            imgaccinst.Visible = true;
        }
        else
        {
            imgaccinst.Visible = false;
        }
    }

    protected void chkSeaServiceAcceptedLieu_CheckedChanged(object sender, EventArgs e)
    {             
        if (chkSeaServiceAcceptedLieu.Checked == true)
        {
            txtSeaServiceDetails.Visible = true;
          //  lblSeaServiceDetails.Visible = true;
        }
        else
        {
          //  lblSeaServiceDetails.Visible = false;
            txtSeaServiceDetails.Visible = false;
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
            RadLabel2.Visible = false;
            imgowner.Visible = false;
            RadLabel3.Visible = false;
            imgcomponent.Visible = false;
        }
        else if (ucCategory.SelectedValue.ToString() == "3") //COMPANY REQUIREMENT
        {
            lblCharterer.Visible = false;
            imgcharterer.Visible = false;
            lblflag.Visible = false;
            imgflagpicklist.Visible = false;
            RadLabel1.Visible = true;
            imgcompany.Visible = true;
            RadLabel2.Visible = true;
            imgowner.Visible = true;
            RadLabel3.Visible = true;
            imgcomponent.Visible = true;
        }
        else if (ucCategory.SelectedValue.ToString() == "4") //CHARTERER
        {
            lblCharterer.Visible = true;
            imgcharterer.Visible = true;
            lblflag.Visible = false;
            imgflagpicklist.Visible = false;
            RadLabel1.Visible = false;
            imgcompany.Visible = false;
            RadLabel2.Visible = true;
            imgowner.Visible = true;
            RadLabel3.Visible = true;
            imgcomponent.Visible = true;
        }
        else if (ucCategory.SelectedValue.ToString() == "5") //Flag State Documents
        {
            lblCharterer.Visible = false;
            imgcharterer.Visible = false;
            lblflag.Visible = true;
            imgflagpicklist.Visible = true;
            RadLabel1.Visible = false;
            imgcompany.Visible = false;
            RadLabel2.Visible = false;
            imgowner.Visible = false;
            RadLabel3.Visible = false;
            imgcomponent.Visible = false;
        }
        else if (ucCategory.SelectedValue.ToString() == "6") //	Owner Requirement
        {

            lblCharterer.Visible = false;
            imgcharterer.Visible = false;
            lblflag.Visible = false;
            imgflagpicklist.Visible = false;
            RadLabel1.Visible = false;
            imgcompany.Visible = false;
            RadLabel2.Visible = true;
            imgowner.Visible = true;
            RadLabel3.Visible = true;
            imgcomponent.Visible = true;

        }
        else
        {
            lblCharterer.Visible = false;
            imgcharterer.Visible = false;
            lblflag.Visible = false;
            imgflagpicklist.Visible = false;
            RadLabel1.Visible = false;
            imgcompany.Visible = false;
            RadLabel2.Visible = false;
            imgowner.Visible = false;
            RadLabel3.Visible = false;
            imgcomponent.Visible = false;
        }
    }

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
