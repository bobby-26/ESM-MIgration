using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
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
using System.Text;
using SouthNests.Phoenix.Dashboard;


public partial class DocumentManagement_DocumentManagementFormListAddNew : PhoenixBasePage
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
            ucCompany.Enabled = false;
            ucCompany.SelectedCompany = PhoenixSecurityContext.CurrentSecurityContext.CompanyID.ToString();
            btnShowCategory.Attributes.Add("onclick", "return showPickList('spnPickListCategory', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDocumentCategory.aspx', true); ");
            btnShowComponents.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListGlobalComponent.aspx', true); ");

            BindJobs(txtComponentId.Text);
            if (Request.QueryString["FORMID"] != null && Request.QueryString["FORMID"].ToString() != string.Empty)
            {
                ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();
            }
            else
                ViewState["FORMID"] = "";

            BindGroupfunctionList();
        }
    }
    protected void BindFormRA()
    {
        DataSet dss = PhoenixDocumentManagementQuestion.MappedRAList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["FORMID"].ToString()));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
        {
            tblRA.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                CheckBox cbRA = new CheckBox();
                cbRA.ID = dr["FLDRAID"].ToString();
                cbRA.Text = "";
                cbRA.Checked = true;
                cbRA.Enabled = false;
                HyperLink hlreport = new HyperLink();
                hlreport.Text = dr["FLDNAME"].ToString();
                hlreport.ID = "reportlink" + number.ToString();
                hlreport.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                hlreport.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'SSRSReports/SsrsReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAPROCESSNEW&showmenu=0&showexcel=NO&processid=" + dr["FLDRAID"].ToString() + "');return false;");
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
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
    protected void MenuCommentsEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (ViewState["FORMID"] != null)
            {

                if (CommandName.ToUpper().Equals("SAVE"))
                {

                    if (!IsValidField(
       General.GetNullableString(txtFormNo.Text),
        General.GetNullableString(txtFormName.Text),
        General.GetNullableString(txtCategoryid.Text),
        General.GetNullableString(ucCompany.SelectedCompany)
       ))
                    {
                        ucError.Visible = true;
                        return;
                    }

                    Guid? formid = Guid.Empty;
                    int displayinfms = 0;
                    if (radchckfmsyn.Checked == true)
                    {
                        displayinfms = 1;
                    }
                    string function = "";
                    foreach (RadComboBoxItem item in ddlgroupfunction.Items)
                    {
                        if (item.Checked == true)
                            function = function + item.Value + ",";

                    }
                    PhoenixDocumentManagementForm.FormInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , General.GetNullableString(txtFormNo.Text)
                                                                    , General.GetNullableString(txtFormName.Text)
                                                                    , General.GetNullableGuid(txtCategoryid.Text)
                                                                    , chkActiveYN.Checked == true ? 1 : 0
                                                                    , General.GetNullableString(txtRemarks.Text)
                                                                    , General.GetNullableInteger(rListAdd.SelectedValue)
                                                                    , General.GetNullableInteger(ucCompany.SelectedCompany)
                                                                    , General.GetNullableInteger(ucGroupEdit.SelectedHard)
                                                                    , General.GetNullableInteger(ucOfficeDept.SelectedDepartment)
                                                                    , null
                                                                    , General.GetNullableInteger(ucPort.SelectedValue)
                                                                    , General.GetNullableInteger(ucEquipment.SelectedValue)
                                                                    , General.GetNullableGuid(txtComponentId.Text)
                                                                    , null
                                                                    , General.GetNullableString(txtTimeInterval.Text)
                                                                    , null
                                                                    , null, null, ref formid
                                                                    , displayinfms
                                                                    , null
                                                                    , function
                                                                    , General.GetNullableGuid(null)
                                                                    , General.ReadCheckBoxList(cbJobs));
                    ViewState["FORMID"] = formid.ToString();

                    FormEdit();
                }


            }
            Response.Redirect("../DocumentManagement/DocumentManagementFormListNew.aspx?FORMID=" + ViewState["FORMID"]);
            ucStatus.Text = "Saved successfully.";

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFormListNew.aspx?FORMID=" + ViewState["FORMID"]);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindGroupfunctionList()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentJobHazardExtn.FunctionGroupMappingList();
        ddlgroupfunction.DataSource = ds;
        ddlgroupfunction.DataBind();
    }
    private void BindJobs(string Componentid)
    {

        cbJobs.DataSource = PhoenixDocumentManagementQuestion.ComponentJobList(General.GetNullableGuid(Componentid));
        cbJobs.DataTextField = "FLDJOBTITLE";
        cbJobs.DataValueField = "FLDGLOBALCOMPONENTJOBID";
        cbJobs.DataBind();
    }
    protected void BindFormProcedures()
    {
        DataSet dss = PhoenixDocumentManagementQuestion.DocumentList(new Guid(ViewState["FORMID"].ToString()));

        int number = 1;
        if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
        {
            tblPROCEDURE.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                CheckBox cbDocument = new CheckBox();
                cbDocument.ID = dr["FLDSECTIONID"].ToString();
                cbDocument.Text = "";
                cbDocument.Checked = true;
                cbDocument.Enabled = false;
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDNAME"].ToString();
                hl.ID = "hlink" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                int type = 0;
                PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDNAME"].ToString()), ref type);
                hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDSECTIONID"].ToString() + "');return false;");
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
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
        DataSet dss = PhoenixDocumentManagementQuestion.MappedJHAList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["FORMID"].ToString()));
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
                cbreport.Enabled = false;
                HyperLink hlreport = new HyperLink();
                hlreport.Text = dr["FLDNAME"].ToString();
                hlreport.ID = "reportlink" + number.ToString();
                hlreport.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                hlreport.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'SSRSReports/SsrsReportsViewWithSubReport.aspx?applicationcode=9&reportcode=JOBHAZARDNEW&showmenu=0&showexcel=NO&showword=NO&jobhazardid=" + dr["FLDJHAID"].ToString() + "');return false;");
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
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
    protected void FormEdit()
    {
        if (!string.IsNullOrEmpty(ViewState["FORMID"].ToString()))
        {
            DataSet ds = PhoenixDocumentManagementForm.FormEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["FORMID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtFormNo.Text = dr["FLDFORMNO"].ToString();
                txtCategoryid.Text = dr["FLDCATEGORYID"].ToString();
                txtCategory.Text = dr["FLDCATEGORY"].ToString();
                txtFormName.Text = dr["FLDCAPTION"].ToString();
                if (dr["FLDACTIVEYN"].ToString() == "1")
                    chkActiveYN.Checked = true;
                else
                    chkActiveYN.Checked = false;
                txtRemarks.Text = dr["FLDPURPOSE"].ToString();
                if (dr["FLDCOMPANYID"] != null && dr["FLDCOMPANYID"].ToString() != "")
                    ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
                if (dr["FLDSHIPDEPARTMENT"] != null && dr["FLDSHIPDEPARTMENT"].ToString() != "")
                    ucGroupEdit.SelectedHard = dr["FLDSHIPDEPARTMENT"].ToString();
                if (dr["FLDOFFICEDEPARTMENT"] != null && dr["FLDOFFICEDEPARTMENT"].ToString() != "")
                    ucOfficeDept.SelectedValue = int.Parse(dr["FLDOFFICEDEPARTMENT"].ToString());
                txtTimeInterval.Text = dr["FLDTIMEINTERVAL"].ToString();
                if (dr["FLDCOUNTRYPORT"] != null && dr["FLDCOUNTRYPORT"].ToString() != "")
                    ucPort.SelectedValue = dr["FLDCOUNTRYPORT"].ToString();
                if (dr["FLDEQUIPMENTMAKER"] != null && dr["FLDEQUIPMENTMAKER"].ToString() != "")
                    ucEquipment.SelectedValue = dr["FLDEQUIPMENTMAKER"].ToString();

                BindFormJHA();
                txtComponentId.Text = dr["FLDPMSCOMPONENT"].ToString();
                BindFormProcedures();
                BindFormRA();

                txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
                txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
                BindJobs(txtComponentId.Text);
                General.BindCheckBoxList(cbJobs, dr["FLDJOBLIST"].ToString());
            }
        }

    }
    private bool IsValidField(string formno, string formname, string categoryid, string companyid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(formno) == null)
            ucError.ErrorMessage = "Form Number is required.";

        if (General.GetNullableString(formname) == null)
            ucError.ErrorMessage = "Form Name is required.";

        if (General.GetNullableGuid(categoryid) == null)
            ucError.ErrorMessage = "Form Category is required.";

        if (General.GetNullableInteger(companyid) == null)
            ucError.ErrorMessage = "Company is required.";

        return (!ucError.IsError);
    }

    protected void txtComponentId_TextChanged(object sender, EventArgs e)
    {
        BindJobs(txtComponentId.Text);
    }
}
