using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Common;
using System.Collections;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Integration;
using SouthNests.Phoenix.Dashboard;

public partial class DocumentManagement_DocumentManagementFormListDetailsUpdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuCommentsEdit.AccessRights = this.ViewState;
        MenuCommentsEdit.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            //lnkImportJHA.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionJHATemplateMapping.aspx?RISKASSESSMENTPROCESSID=" + ViewState["RISKASSESSMENTPROCESSID"] + "');return false;");
            btnShowCategory.Attributes.Add("onclick", "return showPickList('spnPickListCategory', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDocumentCategory.aspx', true); ");
            btnShowComponents.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListGlobalComponent.aspx', true); ");
            //btnShowComponents.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '../Common/CommonPickListGlobalComponent.aspx'); ");

            if (Request.QueryString["FORMID"] != null && Request.QueryString["FORMID"].ToString() != string.Empty)
            {
                ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
            }
            else
                ViewState["FORMID"] = "";

            
           
            BindCategory();
           // BindGroupRankList();
            BindGroupfunctionList();
            FormEdit();
            
        }       

       
        lnkImportJHA.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementJHATemplateMapping.aspx?FORMID=" + ViewState["FORMID"] + "');return false;");
        imgJob.Attributes.Add("onclick", "return showPickList('spnPickListJob', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJob.aspx', true);");


    }
    
    protected void BindCategory()
    {
        DataSet ds = new DataSet();

        ds = PhoenixInspectionRiskAssessmentActivity.ListRiskAssessmentActivity(null, PhoenixSecurityContext.CurrentSecurityContext.CompanyID);

        if (ds.Tables[0].Rows.Count > 0)
        {
            ddlCategory.Items.Clear();
            ddlCategory.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
            ddlCategory.DataSource = ds.Tables[0];
            ddlCategory.DataBind();
        }
    }
    //private void BindGroupRankList()
    //{
    //    DataSet ds = PhoenixDashboardOption.DashboarGroupRanklist();
    //    ddlGroupRank.DataSource = ds;
    //    ddlGroupRank.DataBind();
    //}
    private void BindGroupfunctionList()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentJobHazardExtn.FunctionGroupMappingList();
        ddlgroupfunction.DataSource = ds;
        ddlgroupfunction.DataBind();
    }
    protected void BindFormRA()
    {
        DataSet dss = PhoenixDocumentManagementForm.BindFormRA(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["FORMID"].ToString()));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
        {
            tblRA.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                //CheckBox cbreport = new CheckBox();
                //cbreport.ID = dr["FLDRAID"].ToString();
                //cbreport.Text = "";
                //cbreport.Checked = true;
                //cbreport.AutoPostBack = true;
                // cbreport.CheckedChanged += new EventHandler(cbreport_CheckedChanged);
                HyperLink hlreport = new HyperLink();
                hlreport.Text = dr["FLDNAME"].ToString();
                hlreport.ID = "hlreportra" + number.ToString();
                hlreport.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                hlreport.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'SSRSReports/SsrsReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAPROCESSNEW&showmenu=0&showexcel=NO&processid=" + dr["FLDRAID"].ToString() + "');return false;");
                //hlreport.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'Inspection/InspectionRAProcessExtn.aspx?status=3&processid=" + dr["FLDRAID"].ToString() + "');return false;");
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                //tc.Controls.Add(cbreport);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hlreport);
                tr.Cells.Add(tc);
                tblRA.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblRA.Rows.Add(tr);
                number = number + 1;
            }
            dvRA.Visible = true;
        }
        else
            dvRA.Visible = true;
    }
    protected void BindFormProcedures()
    {
        DataSet dss = PhoenixDocumentManagementForm.BindFormPROCEDURE(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["FORMID"].ToString()));
      
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
        {
            tblPROCEDURE.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                //CheckBox cb = new CheckBox();
                //cb.ID = dr["FLDNAME"].ToString();
                //cb.Text = "";
                //cb.Checked = true;
                //cb.AutoPostBack = true;
                //if (ViewState["status"] != null && ViewState["status"].ToString().Equals("3"))
                //    cb.Enabled = false;
                //cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
                //cb.Attributes.Add("onclick","");


                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDNAME"].ToString();
                hl.ID = "hlink" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                int type = 0;
                PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDNAME"].ToString()), ref type);
                if (type == 2)
                    hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                else if (type == 3)
                    hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDFORMPOSTERID"].ToString() + "');return false;");
                else if (type == 5)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(hl.ID.ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = ds.Tables[0].Rows[0];
                        hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementFormPreview.aspx?FORMID=" + dr["FLDFORMPOSTERID"].ToString() + "&FORMREVISIONID=" + drr["FLDFORMREVISIONID"].ToString() + "');return false;");
                    }
                }
                else if (type == 6)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                    PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , new Guid(dr["FLDFORMPOSTERID"].ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = ds.Tables[0].Rows[0];
                        if (drr["FLDFORMREVISIONDTKEY"] != null && General.GetNullableGuid(drr["FLDFORMREVISIONDTKEY"].ToString()) != null)
                        {
                            DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(drr["FLDFORMREVISIONDTKEY"].ToString()));
                            if (dt.Rows.Count > 0)
                            {
                                DataRow drRow = dt.Rows[0];
                                //ifMoreInfo.Attributes["src"] = Session["sitepath"] + "/attachments/" + drRow["FLDFILEPATH"].ToString(); 
                                hl.Target = "_blank";
                                hl.NavigateUrl = Session["sitepath"] + "/attachments/" + drRow["FLDFILEPATH"].ToString() + "#page=" + 1;
                            }
                        }
                    }
                }

                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                //tc.Controls.Add(cb);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hl);
                tr.Cells.Add(tc);
                tblPROCEDURE.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblPROCEDURE.Rows.Add(tr);
                number = number + 1;
            }
            dvPROCEDURE.Visible = true;
        }
        else
            dvPROCEDURE.Visible = true;
    }
    protected void BindFormJHA()
    {
        DataSet dss = PhoenixDocumentManagementForm.BindFormJHA(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["FORMID"].ToString()));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
        {
            tblJHA.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                CheckBox cbreport = new CheckBox();
                cbreport.ID = dr["FLDJHAID"].ToString();
                cbreport.Text = "";
                cbreport.Checked = true;
                cbreport.AutoPostBack = true;   
                cbreport.CheckedChanged += new EventHandler(cbreport_CheckedChanged);
                HyperLink hlreport = new HyperLink();
                hlreport.Text = dr["FLDNAME"].ToString();
                hlreport.ID = "reportlink" + number.ToString();
                hlreport.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                hlreport.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'SSRSReports/SsrsReportsViewWithSubReport.aspx?applicationcode=9&reportcode=JOBHAZARDNEW&showmenu=0&showexcel=NO&showword=NO&jobhazardid=" + dr["FLDJHAID"].ToString() + "');return false;");
                //hlreport.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'Inspection/InspectionRAProcessExtn.aspx?status=3&processid=" + dr["FLDJHAID"].ToString() + "');return false;");
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tc.Controls.Add(cbreport);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hlreport);
                tr.Cells.Add(tc);
                tblJHA.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblJHA.Rows.Add(tr);
                number = number + 1;
            }
            dvJHA.Visible = true;
        }
        else
            dvJHA.Visible = true;
    }

    void cbreport_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox c = (CheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixDocumentManagementForm.FormJHADelete(new Guid(ViewState["FORMID"].ToString()), new Guid(c.ID));
            //PhoenixInspectionCDISIREMatrix.FormPosterDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            //    new Guid(ViewState["FLDCONTENTID"].ToString()), new Guid(c.ID), General.GetNullableGuid(ViewState["CATEGORYID"].ToString()), 0);
  
            ucStatus.Text = "JHA deleted.";
            BindFormJHA();

        }
    }


    protected void FormEdit()
    {
        if (General.GetNullableGuid(ViewState["FORMID"].ToString()) != null)
        {
            DataSet ds = PhoenixDocumentManagementForm.FormEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["FORMID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtFormNo.Text = dr["FLDFORMNO"].ToString();
                txtCategoryid.Text = dr["FLDCATEGORYID"].ToString();
                txtCategory.Text = dr["FLDCATEGORY"].ToString();
                txtFormName.Text = dr["FLDCAPTION"].ToString();
                if (dr["FLDDISPLAYINFMS"] != null)
                {
                    if (dr["FLDDISPLAYINFMS"].ToString() == "1")
                        radchckfmsyn.Checked = true;
                }
                if (dr["FLDTYPE"] != DBNull.Value)
                {
                    rListAdd.SelectedValue = dr["FLDTYPE"].ToString();
                }
                //txtFileNo.Text = dr["FLDFILENO"].ToString();
                if (dr["FLDACTIVEYN"].ToString() == "1")
                    chkActiveYN.Checked = true;
                else
                    chkActiveYN.Checked = false;
                txtRemarks.Text = dr["FLDPURPOSE"].ToString();
                if (dr["FLDCOMPANYID"] != null && dr["FLDCOMPANYID"].ToString() != "")
                    ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
                //if (dr["FLDPRIMARYOWNERSHIP"] != null && dr["FLDPRIMARYOWNERSHIP"].ToString() != "")
                //    ucRankPrimary.SelectedValue = int.Parse(dr["FLDPRIMARYOWNERSHIP"].ToString());
                //if (dr["FLDSECONDARYOWNERSHIP"] != null && dr["FLDSECONDARYOWNERSHIP"].ToString() != "")
                //    ucRankSecondary.SelectedValue = int.Parse(dr["FLDSECONDARYOWNERSHIP"].ToString());
                //txtOtherParticipants.Text = dr["FLDOTHERPARTICIPANTS"].ToString();
                //txtPrimaryOffice.Text = dr["FLDPRIMARYOWNEROFFICE"].ToString();
                //txtSecondaryOffice.Text = dr["FLDSECONDARYOWNEROFFICE"].ToString();
                //txtShipType.Text = dr["FLDSHIPTYPE"].ToString();
                if (dr["FLDSHIPDEPARTMENT"] != null && dr["FLDSHIPDEPARTMENT"].ToString() != "")
                    ucGroupEdit.SelectedHard = dr["FLDSHIPDEPARTMENT"].ToString();
                if (dr["FLDOFFICEDEPARTMENT"] != null && dr["FLDOFFICEDEPARTMENT"].ToString() != "")
                    ucOfficeDept.SelectedValue = int.Parse(dr["FLDOFFICEDEPARTMENT"].ToString());
                txtTimeInterval.Text = dr["FLDTIMEINTERVAL"].ToString();
                if (dr["FLDCOUNTRYPORT"] != null && dr["FLDCOUNTRYPORT"].ToString() != "")
                    ucPort.SelectedValue = dr["FLDCOUNTRYPORT"].ToString();
                if (dr["FLDEQUIPMENTMAKER"] != null && dr["FLDEQUIPMENTMAKER"].ToString() != "")
                    ucEquipment.SelectedValue = dr["FLDEQUIPMENTMAKER"].ToString();
               // txtEquMaker.Text = dr["FLDEQUIPMENTMAKER"].ToString();
                //txtJHA.Text = dr["FLDJHA"].ToString();
                BindFormJHA();
                //DataTable dt = new DataTable();
                //dt = ds.Tables[0];
                //chkImportedJHAList.DataSource = dt;
                //chkImportedJHAList.DataBindings.DataTextField = "FLDIMPORTED";
                //chkImportedJHAList.DataBindings.DataValueField = "FLDIMPORTEDID";
                //chkImportedJHAList.DataBind();
                //foreach (ButtonListItem chkitem in chkImportedJHAList.Items)
                //    chkitem.Selected = true;
                txtComponentId.Text = dr["FLDPMSCOMPONENT"].ToString();
                //txtPMSCompWO.Text = dr["FLDPMSCOMPONENT"].ToString();
                BindFormProcedures();
                //txtProcedure.Text = dr["FLDPROCEDURE"].ToString();
                //DataTable dt= new DataTable();
                //dt= ds.Tables[0];
                //txtRA.DataSource= dt;
                //txtRA.DataBindings.DataTextField = "FLDRANUMBERLIST";
                //txtRA.DataBindings.DataValueField = "FLDRISKASSESSMENTID";
                //txtRA.DataBind();
                BindFormRA();
                txtJobId.Text = dr["FLDJOB"].ToString();
                txtJobCode.Text = dr["FLDJOBCODE"].ToString();
                txtJobName.Text = dr["FLDJOBTITLE"].ToString();
                
                //txtRA.Text = dr["FLDRANUMBERLIST"].ToString();
                foreach (RadComboBoxItem item in ddlCategory.Items)
                {
                    item.Checked = false;
                    if (dr["FLDACTIVITY"].ToString().Contains("," + item.Value + ","))
                    {
                        item.Checked = true;
                    }
                }
                foreach (RadComboBoxItem item in ddlgroupfunction.Items)
                {
                    item.Checked = false;
                    if (dr["FLDFUNCTION"].ToString().Contains("," + item.Value + ","))
                    {
                        item.Checked = true;
                    }
                }

                //ddlCategory.Text = dr["FLDACTIVITY"].ToString();
                txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
                txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
                
            }
        }
        //}
    }
    //protected void chkImportedJHAList_Changed(object sender, EventArgs e)
    //{
    //    StringBuilder strjhaid = new StringBuilder();
    //    foreach (ButtonListItem item in chkImportedJHAList.Items)
    //    {
    //        if (item.Selected == true && item.Enabled == true)
    //        {
    //            strjhaid.Append(item.Value.ToString());
    //            strjhaid.Append(",");
    //        }
    //    }

    //    if (strjhaid.Length > 1)
    //    {
    //        strjhaid.Remove(strjhaid.Length - 1, 1);
    //    }
    //    string jhaid = strjhaid.ToString();

    //    PhoenixDocumentManagementForm.FormRiskAssessmentJHAImport(
    //                                            General.GetNullableGuid(ViewState["FORMID"].ToString()),
    //                                            General.GetNullableString(jhaid));


    //    //PhoenixInspectionRiskAssessmentProcess.InspectionRiskAssessmentJHAImport(General.GetNullableGuid(ViewState["RISKASSESSMENTPROCESSID"].ToString()),
    //    //                                                                             jhaid);
    //    ucStatus.Text = "Imported JHA has been removed successfully.";

    //    //cmdHiddenSubmit_Click(sender, new EventArgs());
    //}

    protected void MenuCommentsEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            //if (ViewState["FORMID"] != null)
            //{
                if (CommandName.ToUpper().Equals("SAVE"))
                {
                    string category = "";
                    foreach (RadComboBoxItem item in ddlCategory.Items)
                    {
                        if (item.Checked == true)
                            category = category + item.Value + ",";

                    }
                    string function = "";
                    foreach (RadComboBoxItem item in ddlgroupfunction.Items)
                    {
                        if (item.Checked == true)
                            function = function + item.Value + ",";

                    }
                int displayinfms = 0;
                if (radchckfmsyn.Checked == true)
                {
                    displayinfms = 1;
                }
                PhoenixDocumentManagementForm.FormDetailsUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , General.GetNullableString(txtFormNo.Text)
                                                 , General.GetNullableString(txtFormName.Text)
                                                 , General.GetNullableGuid(txtCategoryid.Text)
                                                 , chkActiveYN.Checked == true ? 1 : 0
                                                 , General.GetNullableString(txtRemarks.Text)
                                                 , General.GetNullableGuid(ViewState["FORMID"].ToString())
                                                 , General.GetNullableInteger(rListAdd.SelectedValue)
                                                 , General.GetNullableInteger(ucCompany.SelectedCompany)
                        //, General.GetNullableString(txtFileNo.Text)
                        //, General.GetNullableInteger(ucRankPrimary.SelectedRank)
                        //, General.GetNullableInteger(ucRankSecondary.SelectedRank)
                        //, General.GetNullableString(txtOtherParticipants.Text)
                        //, General.GetNullableString(txtPrimaryOffice.Text)
                        //, General.GetNullableString(txtSecondaryOffice.Text)
                                                 , General.GetNullableInteger(ucGroupEdit.SelectedHard)
                                                 , General.GetNullableInteger(ucOfficeDept.SelectedDepartment)
                                                 , null
                                                 , General.GetNullableInteger(ucPort.SelectedValue)
                                                 , General.GetNullableInteger(ucEquipment.SelectedValue)
                                                 , General.GetNullableGuid(txtComponentId.Text)
                                                 , category
                                                 , General.GetNullableString(txtTimeInterval.Text)
                        // , General.GetNullableGuid(tblRA.)
                        // , General.GetNullableGuid(txtProcedure.Text)
                        // ,null
                        // , General.GetNullableGuid(txtRA.Text)
                        //, General.GetNullableGuid(txtJHA.Text)
                        //,null
                                                   , displayinfms
                                                   ,null
                                                   ,function
                                                   ,General.GetNullableGuid(txtJobId.Text));

                    //FormEdit();

                }
            //}
                 FormEdit();
                //lnkImportJHA.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementJHATemplateMapping.aspx?FORMID=" + ViewState["FORMID"] + "');return false;");
                ucStatus.Text = "Saved successfully.";
                //String script = String.Format("javascript:fnReloadList();");
                //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);


                if (CommandName.ToUpper().Equals("BACK"))
                {
                    Response.Redirect("../DocumentManagement/DocumentManagementFormList.aspx?FORMID=" + ViewState["FORMID"]);
                }
            }
        
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdJobClear_Click(object sender, ImageClickEventArgs e)
    {
        txtJobCode.Text = "";
        txtJobName.Text = "";
        txtJobId.Text = "";
    }
}