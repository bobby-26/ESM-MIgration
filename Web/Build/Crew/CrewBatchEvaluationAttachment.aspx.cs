using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
public partial class Crew_CrewBatchEvaluationAttachment : PhoenixBasePage
{
    public enum AttachmentType
    {
        CourseReport
      , Marksheet
      , Certificate
      
    }

    public enum AssessmentResult
    {
       Pass
     , Fail     
    }
    string attachmentcode = string.Empty;     
    string cmdname = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            BindAttachmentType();
            //BindAssessmentResult();
            BindAttachment();
            SetEmployeeDetail();
            SetBatch();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            hdndtkey.Value = Request.QueryString["dtkey"].ToString();
            hdnenrollmentid.Value = Request.QueryString["enrollmentId"].ToString();
            //hdnrecommendedId.Value = Request.QueryString["recommendedId"].ToString();
            //ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?dtkey=" + dtkey + "&mod=" + PhoenixModule.CREW + "&type=" + rblAttachmentType.SelectedValue + "&cmdname=COURSEUPLOAD";
            BindInstituteVenue();
            gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            attachmentcode = hdndtkey.Value;
        }
        if ( !String.IsNullOrEmpty(hdndtkey.Value))
        {
            AttachmentType type = (AttachmentType)(Enum.Parse(typeof(AttachmentType), rblAttachmentType.SelectedValue));
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('download','','" + Session["sitepath"] + "/Common/CommonViewAllAttachments.aspx?dtkey=" + hdndtkey.Value + "&mod=" + PhoenixModule.CREW + "&type=" + type + "')", "View All Images", "<i class=\"fas fa-images\"></i>", "VIEWALL");            
            SubMenuAttachment.AccessRights = this.ViewState;
            SubMenuAttachment.MenuList = toolbargrid.Show();
        }            
    }

    private void SetEmployeeDetail()
    {
        string employeeId = Request.QueryString["empId"];
        DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(employeeId));

        if (dt.Rows.Count > 0)
        {
            txtName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString() + " " + dt.Rows[0]["FLDMIDDLENAME"].ToString() + " " + dt.Rows[0]["FLDLASTNAME"].ToString();
            txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
            txtFileNo.Text = dt.Rows[0]["FLDFILENO"].ToString();
            txtNationality.Text = dt.Rows[0]["FLDNATIONALITY"].ToString();
        }
    }
    private void SetBatch()
    {
        string batchId = Request.QueryString["batchId"].ToString();

        DataTable dt = PhoenixCrewInstituteBatch.CrewInstituteBatchEdit(General.GetNullableGuid(batchId));
        if (dt.Rows.Count > 0)
        {
            txtBatchNo.Text = dt.Rows[0]["FLDBATCHNO"].ToString();
            txtInstituteId.Text = dt.Rows[0]["FLDINSTITUTEID"].ToString();
            //txtBatchStatus.Text = ds.Tables[0].Rows[0]["FLDSTATUS"].ToString();
        }
    }
    private void BindAttachmentType()
    {
        Array attachmentType = Enum.GetValues(typeof(AttachmentType));

        foreach (AttachmentType Type in attachmentType)
        {
            rblAttachmentType.Items.Add(new ButtonListItem(Type.ToString(), ((int)Type).ToString()));
        }
        rblAttachmentType.SelectedIndex = 0;
    }

    private void BindAssessmentResult()
    {
        Array resulttype = Enum.GetValues(typeof(AssessmentResult));

        foreach (AssessmentResult Type in resulttype)
        {
            rblResult.Items.Add(new ButtonListItem(Type.ToString(), ((int)Type).ToString()));
        }
        rblResult.SelectedIndex = 0;
    }

    private void BindAttachment()
    {
        string dtkey = Request.QueryString["dtkey"];
        //ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?dtkey=" + dtkey + "&mod=" + PhoenixModule.CREW + "&type=" + rblAttachmentType.SelectedValue + "&cmdname=COURSEUPLOAD";
    }

    private void BindInstituteVenue()
    {        
        DataTable dt = PhoenixCrewInstitute.CrewInstituteAddressSearch(General.GetNullableInteger(txtInstituteId.Text).Value);
        txtPlaceOfIssue.Text = dt.Rows[0]["FLDCITYNAME"].ToString();
    }
    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 152, "CMP") == txtBatchStatus.Text)
            {
                ucError.Text = "Completed batch detail cannot be changed";
                ucError.Visible = true;
                return;
            }
            if (PhoenixCommonRegisters.GetHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 152, "CNL") == txtBatchStatus.Text)
            {
                ucError.Text = "Canceled batch detail cannot be changed";
                ucError.Visible = true;
                return;
            }
            string dtkey = Request.QueryString["dtkey"].ToString();
            string batchId = Request.QueryString["batchId"].ToString();
            string attachmenttype = rblAttachmentType.SelectedValue;
            BindAttachment();            
        }
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
    }

    protected void rblAttachmentType_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rblAttachmentType.SelectedIndex == -1)
            rblAttachmentType.SelectedIndex = 1;
        string dtkey = hdndtkey.Value;       
        AttachmentType type = (AttachmentType)(General.GetNullableInteger(rblAttachmentType.SelectedValue).Value);        
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                    "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', 'keepopen');", true);
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
    }
    
    private bool IsValidCourseCertificate()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(txtCertificateNo.Text))
            ucError.ErrorMessage = "Certificate Number is required.";
        
        if (string.IsNullOrEmpty(txtIssueDate.Text))
            ucError.ErrorMessage = "Issue Date is required.";

        return (!ucError.IsError);
    }

    private void clear()
    {
        txtCertificateNo.Text  = "";
        txtPlaceOfIssue.Text   = "";
        txtIssueDate.Text = "";
     
    }
    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        attachmentcode = hdndtkey.Value;
        if (attachmentcode != string.Empty)
        {
            AttachmentType Attype = (AttachmentType)General.GetNullableInteger(rblAttachmentType.SelectedValue).Value;
            string type = Attype.ToString();
            //geting dtkey of employeecourse if type is certificate
            //if (Attype == AttachmentType.Certificate)
            //{
            //    string empid = Request.QueryString["empId"].ToString();
            //    string courseid = Request.QueryString["courseId"].ToString();
            //    dt = PhoenixCrewBatchEnrollment.CrewGetEmployeeCourse(Convert.ToInt32(empid)
            //                                                             , Convert.ToInt32(courseid)
            //                                                             , txtCertificateNo.Text
            //                                                             , General.GetNullableInteger(txtNationality.Text)
            //                                                             , General.GetNullableDateTime(txtIssueDate.Text));
            //    type = "COURSE";
            //}
            //if (dt!=null && dt.Rows.Count > 0)
            //    attachmentcode = dt.Rows[0]["FLDDTKEY"].ToString();

            ds = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(attachmentcode)
                                                              , null
                                                              , type.ToString()
                                                              , sortexpression
                                                              , sortdirection
                                                              , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                              , gvAttachment.PageSize
                                                              , ref iRowCount
                                                              , ref iTotalPageCount);
         
        }
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            gvAttachment.DataSource = ds;
            gvAttachment.VirtualItemCount = iRowCount;
        }
        else if (ds.Tables.Count > 0)
        {
            gvAttachment.DataSource = "";
        }
    }


    private string ResolveImageType(string extn)
    {
        string imagepath = "";
        if (string.IsNullOrEmpty(extn)) extn = string.Empty;
        switch (extn.ToLower())
        {
            case ".jpg":
            case ".png":
            case ".gif":
            case ".bmp":
                imagepath += "<i class=\"fas fa-file-image\"></i>";
                break;
            case ".doc":
            case ".docx":
                imagepath += "<i class=\"fas fa-file-word\"></i>";
                break;
            case ".xls":
            case ".xlsx":
            case ".xlsm":
                imagepath += "<i class=\"fas fa-file-excel\"></i>";
                break;
            case ".pdf":
                imagepath += "<i class=\"fas fa-file-pdf\"></i>";
                break;
            case ".rar":
            case ".zip":
                imagepath += "<i class=\"fas fa-file-archive\"></i>";
                break;
            case ".txt":
                imagepath += "<i class=\"fas fa-clipboard\"></i>";
                break;
            default:
                imagepath += "<i class=\"fas fa-file\"></i>";
                break;
        }
        return imagepath;
    }


    protected void txtFileUpload1_FileUploaded(object sender, FileUploadedEventArgs e)
    {

        if (!string.IsNullOrEmpty(attachmentcode))
        {
            UploadedFileCollection fileUpload = txtFileUpload1.UploadedFiles;
            try
            {
                AttachmentType Atttype = (AttachmentType)General.GetNullableInteger(rblAttachmentType.SelectedValue).Value;
                string type = Atttype.ToString();
              
                //checking employee course status
                string batchId = Request.QueryString["batchId"].ToString();
                DataTable dt = PhoenixCrewBatchEnrollment.CrewGetEmployeeAttendance(General.GetNullableGuid(hdnenrollmentid.Value)
                                                                    , General.GetNullableGuid(batchId));

                //inserting attachment
                PhoenixCommonFileAttachment.InsertAttachment(fileUpload
                                                            , new Guid(attachmentcode)
                                                            , PhoenixModule.CREW
                                                            , null
                                                            , null
                                                            , string.Empty
                                                            , General.GetNullableString(type)
                                                            , null);
                
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                       "BookMarkScript", "fnReloadList(null,'ifMoreInfo','keepopen');", true);

                PhoenixCommonCrew.CrewAppraisalFinaliseByAttachment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(attachmentcode));
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                         "BookMarkScript", "parent.fnReloadList('codehelp1', 'ifMoreInfo', 'keepopen');", true);

                clear();
                BindData();
                gvAttachment.Rebind();
            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }

        }
        else
        {
            string msg = "Select a record to add attachment";
            msg += string.IsNullOrEmpty(Request.QueryString["mod"]) ? "<br/>* Module is required for attachment" : string.Empty;
            ucError.ErrorMessage = msg;
            ucError.Visible = true;
        }
    }

    protected void gvAttachment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAttachment.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvAttachment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            if (e.CommandName.ToString().ToUpper() == "SELECT")
            {
                LinkButton lblFileName = ((LinkButton)e.Item.FindControl("lblFileName"));

                ViewState["RowIndex"] = e.Item.RowIndex;

                //lblTicket.Text = "Ticket List - " + lblFileName.Text;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvAttachment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            LinkButton ed = (LinkButton)e.Item.FindControl("cmdEdit");
            if (ed != null)
            {
                ed.Visible = SessionUtil.CanAccess(this.ViewState, ed.CommandName);
            }

            if (Request.QueryString["u"] != null)
            {
                db.Visible = false;
                ed.Visible = false;
            }

            LinkButton lblFileName = ((LinkButton)e.Item.FindControl("lblFileName"));
            LinkButton imgtype = (LinkButton)e.Item.FindControl("imgfiletype");
            if (lblFileName.Text != string.Empty)
            {
                string imgclass = ResolveImageType(lblFileName.Text.Substring(lblFileName.Text.LastIndexOf('.')));

                if (imgclass != null)
                {
                    imgtype.Visible = true;
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\">" + imgclass + "</span>";
                    imgtype.Controls.Add(html);

                }

                RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                HyperLink lnk = (HyperLink)e.Item.FindControl("lnkfilename");
                lnk.NavigateUrl = "../Common/download.aspx?dtkey=" + drv["FLDDTKEY"].ToString();
            }
        }
    }
    
    protected void gvAttachment_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {           
            string dtkey = ((RadLabel)e.Item.FindControl("lblDTKey")).Text;
            PhoenixCommonFileAttachment.AttachmentDelete(new Guid(dtkey));
            //RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
            //String path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/" + lblFilePath.Text;            
            //System.IO.File.Delete(path);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        BindData();
        gvAttachment.Rebind();
    }

    protected void gvAttachment_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string dtkey = ((RadLabel)e.Item.FindControl("lblDTKeyEdit")).Text;
            bool chk = ((RadCheckBox)e.Item.FindControl("chkSynch")).Checked == true;
            string filename = ((RadLabel)e.Item.FindControl("lblFileNameEdit")).Text;
            PhoenixCommonFileAttachment.FileSyncUpdate(new Guid(dtkey), (chk ? byte.Parse("1") : byte.Parse("0")), filename);            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        BindData();
        gvAttachment.Rebind();
    }
}