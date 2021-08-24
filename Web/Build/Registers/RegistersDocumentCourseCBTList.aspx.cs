using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;

public partial class RegistersDocumentCourseCBTList : PhoenixBasePage
{
    string documentno = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!IsPostBack)
        {

            PhoenixToolbar toolbar = new PhoenixToolbar();

            chkUserGroup.DataSource = SessionUtil.UserGroupList();
            chkUserGroup.DataTextField = "FLDGROUPNAME";
            chkUserGroup.DataValueField = "FLDGROUPCODE";
            chkUserGroup.DataBind();

            ViewState["CBT"] = "";

            if (Request.QueryString["CBT"] != null && Request.QueryString["CBT"].ToString() != "")
                ViewState["CBT"] = 1;

            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE") && ViewState["CBT"].ToString() == "")
                ucDocumentType.ShortNameFilter = "0,1,2,3,5,6,7,8,9";
            else if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE") && ViewState["CBT"].ToString() != "")
            {
                ucDocumentType.ShortNameFilter = "4"; 
                lblMapInCompetenceSubcategoryYN.Visible = false;
                chkMapinCompetencesubcategoryYN.Visible = false;
            }
            else
                ucDocumentType.ShortNameFilter = "0,1,2,3,4,5,6,7,8,9";

            ucDocumentType.bind();

            ucToBeDoneby.TypeOfTraining = PhoenixCrewOffshoreTrainingNeeds.ListTypeOfTraining(129, "CBT");
            ucToBeDoneby.bind();

           // BindRankList();
           // BindVesselTypeList();
            //BindChartererList();
            BindCategory();
            BindSubCategory("");

            BindOffshoreStages(ddlStage);
            BindAlternateCourse();
            if (Request.QueryString["DocumentCourseId"] != null)
            {
                toolbar.AddButton("Save", "SAVE",ToolBarDirection.Right);
                ViewState["DocumentCourseId"] = Request.QueryString["DocumentCourseId"].ToString();

                DocumentCourseEdit(Int32.Parse(Request.QueryString["DocumentCourseId"].ToString()));

                imgRankPicklist.Attributes.Add("onclick", "javascript:openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterRankList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                imgvessseltypelist.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterVesselTypeList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                imgcharterer.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterChartererList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                imgusergroup.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentUserWaiverList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                imgalternate.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentAlternateCourseList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
            }
            else
            {
                toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            MenuDoumentCourseList.AccessRights = this.ViewState;
            MenuDoumentCourseList.MenuList = toolbar.Show();
        }
    }

    protected void BindOffshoreStages(RadComboBox ddl)
    {
        ddl.Items.Clear();
        ddl.DataSource = PhoenixRegistersDocumentCourse.ListOffshoreStage(null, null);
        ddl.DataTextField = "FLDSTAGE";
        ddl.DataValueField = "FLDSTAGEID";
        ddl.DataBind();
        ddl.Items.Insert(0, new RadComboBoxItem("--Select--", ""));
    }
    protected void BindAlternateCourse()
    {
       // cblAlternateCourse.Items.Clear();
       // cblAlternateCourse.DataSource = PhoenixRegistersDocumentCourse.ListDocumentCourse(null);
       // cblAlternateCourse.DataTextField = "FLDDOCUMENTNAME";
       // cblAlternateCourse.DataValueField = "FLDDOCUMENTID";
       // cblAlternateCourse.DataBind();
    }
    protected void chkMandatoryYN_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox cb = (RadCheckBox)sender;

        if (chkWaiverYN != null)
        {
            if (cb.Checked==true)
            {
                chkWaiverYN.Enabled = true;
            }
            else
            {
                chkWaiverYN.Checked = false;
                chkWaiverYN.Enabled = false;

                chkUserGroup.SelectedIndex = -1;
                chkUserGroup.Enabled = false;
            }
        }
    }
    protected void chkWaiverYN_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox cb = (RadCheckBox)sender;
        if (chkUserGroup != null)
        {
            if (cb.Checked==true)
            {
                chkUserGroup.Enabled = true;
            }
            else
            {
                chkUserGroup.SelectedIndex = -1;
                chkUserGroup.Enabled = false;
            }
        }
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
                    //foreach (ListItem li in chkUserGroup.Items)
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

                    //string RankList = "";
                    //foreach (ListItem li in chkRankList.Items)
                    //{
                    //    if (li.Selected)
                    //    {
                    //        RankList += li.Value + ",";
                    //    }
                    //}

                    //if (RankList != "")
                    //{
                    //    RankList = "," + RankList;
                    //}

                    //string VesselTypeList = "";
                    //foreach (ListItem li in chkVesselTypeList.Items)
                    //{
                    //    if (li.Selected)
                    //    {
                    //        VesselTypeList += li.Value + ",";
                    //    }
                    //}

                    //if (VesselTypeList != "")
                    //{
                    //    VesselTypeList = "," + VesselTypeList;
                    //}

                    //string ChartererList = "";
                    //foreach (ListItem li in chkChartererList.Items)
                    //{
                    //    if (li.Selected)
                    //    {
                    //        ChartererList += li.Value + ",";
                    //    }
                    //}

                    //if (ChartererList != "")
                    //{
                    //    ChartererList = "," + ChartererList;
                    //}


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
                            , null//General.GetNullableString(UserGroupList)
                            , General.GetNullableInteger(chkAdditionalDocYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkAuthenticationReqYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(ucCategory.SelectedDocumentCategoryID)
                            , null// General.GetNullableString(General.ReadCheckBoxList(cblAlternateCourse))
                            , General.GetNullableInteger(ddlCategory.SelectedValue)
                            , General.GetNullableInteger(ddlSubCategory.SelectedValue)
                            , null//General.GetNullableString(ChartererList)
                            , General.GetNullableInteger(ucToBeDoneby.SelectedToBeDoneBy)
                            , General.GetNullableInteger(chkShowinMasterChecklistYN.Checked==true ? "1" : "0")
                            , General.GetNullableInteger(chkPhotocopyAcceptableYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkMapinCompetencesubcategoryYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(ViewState["CBT"].ToString())
                            , General.GetNullableInteger(ChkCreateCBTForCrew.Checked == true ? "1" : "0")
                            , txtCBTRemarks.Text
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
                            , null//General.GetNullableString(UserGroupList)
                            , General.GetNullableInteger(chkAdditionalDocYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkAuthenticationReqYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(ucCategory.SelectedDocumentCategoryID)
                            , null//General.GetNullableString(General.ReadCheckBoxList(cblAlternateCourse))
                            , General.GetNullableInteger(ddlCategory.SelectedValue)
                            , General.GetNullableInteger(ddlSubCategory.SelectedValue)
                            , null//General.GetNullableString(ChartererList)
                            , General.GetNullableInteger(ucToBeDoneby.SelectedToBeDoneBy)
                            , General.GetNullableInteger(chkShowinMasterChecklistYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkPhotocopyAcceptableYN.Checked == true ? "1" : "0")
                            , General.GetNullableInteger(chkMapinCompetencesubcategoryYN.Checked == true ? "1" : "0")
                            , null//General.GetNullableInteger(chkSingleUse.Checked == true ? "1" : "0")
                            , null//General.GetNullableString(General.RadGetComboboxCheckList(ddlflag))
                            , null//General.GetNullableString(General.RadGetComboboxCheckList(ddlcompany))
                            , ref resultdocumentid
                            , null//General.GetNullableInteger(ucNumberAmount.Text)
                            , null//TrainingTypeList
                            , null//General.GetNullableInteger(chkAccpInst.SelectedValue.ToString())
                            , null//General.GetNullableInteger(chkcertreqyn.Checked == true ? "1" : "0")
                            , null//General.GetNullableInteger(chkFlagEndorsement.Checked == true ? "1" : "0")
                            );
                        ViewState["DocumentCourseId"] = int.Parse(resultdocumentid.ToString());
                        imgalternate.Attributes.Add("onclick", "javascript:top.openNewWindow('codehelp2', '','" + Session["sitepath"] + "/Registers/RegisterDocumentAlternateCourseList.aspx?DocumentCourseId=" + ViewState["DocumentCourseId"] + "&type=COURSE" + "',false,600,400); return false;");
                        // ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", scriptClosePopup, true);

                        // Reset();
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
        string source = ddlSource.SelectedHard;//.SelectedName;
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
        if (ucToBeDoneby.SelectedToBeDoneBy == "")
        {
            ucError.ErrorMessage = "To be done By is required";
        }

        //if (General.GetNullableInteger(ucLevel.SelectedHard) != null)
        //{
        //    if (!Int32.TryParse(ucDepCourse.SelectedCourse, out result))
        //        ucError.ErrorMessage = "Dependent Course is required.";
        //}        

        if (General.GetNullableInteger(chkWaiverYN.Checked == true ? "1" : "0") != null && chkWaiverYN.Checked == true)
        {
            if (chkUserGroup.SelectedIndex == -1)
                ucError.ErrorMessage = "User group is required.";
        }

        if (ChkCreateCBTForCrew.Checked == true && txtCBTRemarks.Text != null && txtCBTRemarks.Text.Trim() == "")
            ucError.ErrorMessage = "Remarks is required";

        //if (rank.Trim().Length == 0)
        //    ucError.ErrorMessage = "Select Atleast one Rank.";

        //if (vessel.Trim().Trim().Length == 0)
        //    ucError.ErrorMessage = "Select Atleast one Vessel";

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
        chkUserGroup.SelectedIndex = -1;
        ChkCreateCBTForCrew.Checked = false;
        txtCBTRemarks.Text = "";
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

                chkAdditionalDocYN.Checked = dr["FLDADDITIONALDOCYN"].ToString() == "1" ? true : false;
                chkAuthenticationReqYN.Checked = dr["FLDAUTHENTICATIONREQYN"].ToString() == "1" ? true : false;

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
                //ucUserGroup.UserGroupList = SessionUtil.UserGroupList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
                //ucUserGroup.DataBind();

                chkUserGroup.DataSource = SessionUtil.UserGroupList();
                chkUserGroup.DataTextField = "FLDGROUPNAME";
                chkUserGroup.DataValueField = "FLDGROUPCODE";
                chkUserGroup.DataBind();

                ddlStage.SelectedValue = dr["FLDSTAGEID"].ToString();
                chkMandatoryYN.Checked = dr["FLDMANDATORYYN"].ToString() == "1" ? true : false;
                chkWaiverYN.Checked = dr["FLDWAIVERYN"].ToString() == "1" ? true : false;
                //ucUserGroup.SelectedUserGroup = dr["FLDUSERGROUPTOALLOWWAIVER"].ToString();
                //General.BindCheckBoxList(cblAlternateCourse, dr["FLDALTERNATECOURSE"].ToString());
                General.BindCheckBoxList(chkUserGroup, dr["FLDUSERGROUPTOALLOWWAIVER"].ToString());
               // General.BindCheckBoxList(chkRankList, dr["FLDRANK"].ToString());
                //General.BindCheckBoxList(chkVesselTypeList, dr["FLDVESSELTYPE"].ToString());
                //General.BindCheckBoxList(chkChartererList, dr["FLDCHARTERER"].ToString());

                ddlCategory.SelectedValue = dr["FLDCATEGORYID"].ToString();
                BindSubCategory(dr["FLDCATEGORYID"].ToString());
                ddlSubCategory.SelectedValue = dr["FLDSUBCATEGORYID"].ToString();

                if (dr["FLDTOBEDONEBY"].ToString() != "")
                    ucToBeDoneby.SelectedToBeDoneBy = dr["FLDTOBEDONEBY"].ToString();


                if (chkMandatoryYN.Checked == true)
                    chkWaiverYN.Enabled = true;
                else
                    chkWaiverYN.Enabled = false;

                if (chkWaiverYN.Checked == true)
                    chkUserGroup.Enabled = true;
                else
                    chkUserGroup.Enabled = false;

                chkShowinMasterChecklistYN.Checked = dr["FLDSHOWINMASTERCHECKLISTYN"].ToString() == "1" ? true : false;
                chkPhotocopyAcceptableYN.Checked = dr["FLDPHOTOCOPYACCEPTABLEYN"].ToString() == "1" ? true : false;
                chkMapinCompetencesubcategoryYN.Checked = dr["FLDSHOWINSUBCATEGORYYN"].ToString() == "1" ? true : false;
                ChkCreateCBTForCrew.Checked = dr["FLDISCBTAPPLYTOCREWYN"].ToString() == "1" ? true : false;
                txtCBTRemarks.Text = dr["FLDCBTREMARKS"].ToString();
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
    protected void ChkCreateCBTForCrew_Checkedchanged(object sender, EventArgs e)
    {
        if (ChkCreateCBTForCrew != null)
        {
            if (ChkCreateCBTForCrew.Checked == true)
                txtCBTRemarks.CssClass = "input_mandatory";
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

    //private void BindChartererList()
    //{
    //    DataSet ds = PhoenixRegistersAddress.ListAddress(((int)PhoenixAddressType.CHARTERER).ToString());
    //    chkChartererList.DataSource = ds;
    //    chkChartererList.DataTextField = "FLDNAME";
    //    chkChartererList.DataValueField = "FLDADDRESSCODE";
    //    chkChartererList.DataBind();
    //}
}
