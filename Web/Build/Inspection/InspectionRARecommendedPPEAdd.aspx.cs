using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Integration;
using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class InspectionRARecommendedPPEAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("Back", "BACK", ToolBarDirection.Right);
            MenuCARGeneral.AccessRights = this.ViewState;
            MenuCARGeneral.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PPEID"] = "-1";
                ViewState["TYPE"] = "-1";

                if (Request.QueryString["PPEID"] != null && Request.QueryString["PPEID"].ToString() != string.Empty)
                    ViewState["PPEID"] = Request.QueryString["PPEID"].ToString();

                ViewState["UNIQUEID"] = ViewState["PPEID"].ToString();
                ViewState["COMPANYID"] = "0";

                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                    ViewState["COMPANYID"] = nvc.Get("QMS");


                BindPPE();
                btnShowDocuments.Attributes.Add("onclick", "return showPickList('spnPickListDocument', 'codehelp1', '', '../Common/CommonPickListDocumentManagementTree.aspx?iframignore=true&companyid=" + ViewState["COMPANYID"].ToString() + "', true); ");
            }
            BindFormPosters();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindPPE()
    {
        if (ViewState["PPEID"] != null && ViewState["PPEID"].ToString() != string.Empty)
        {
            DataSet dt = PhoenixInspectionRiskAssessmentMiscellaneousExtn.EditRiskAssessmentMiscellaneous(int.Parse(ViewState["PPEID"].ToString()));
            if (dt.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dt.Tables[0].Rows[0];
                txtlName.Text = dr["FLDNAME"].ToString();
                imgPhoto.Attributes.Add("src", dr["FLDIMAGE"].ToString());
            }
        }
    }

    protected void MenuCARGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["PPEID"] != null && ViewState["PPEID"].ToString() != string.Empty)
                    UpdateActivity();
                else
                    InsertActivity();

                BindPPE();
            }

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../Inspection/InspectionRARecommendedPPE.aspx?");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void InsertActivity()
    {
        if (!IsValidRAActivity())
        {
            ucError.Visible = true;
            return;
        }

        string result = "";


        foreach (UploadedFile postedFile in RadUpload1.UploadedFiles)
        {
            if (!Object.Equals(postedFile, null))
            {
                if (postedFile.ContentLength > 0)
                {

                    if (postedFile.ContentLength > (60 * 1024))
                    {
                        ucError.ErrorMessage = "Uploaded Photo size cannot exceed 60kb.";
                        ucError.Visible = true;
                        return;
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        string path = HttpContext.Current.Server.MapPath("..\\Attachments\\TEMP\\") + postedFile.FileName;
                        postedFile.SaveAs(path);

                        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        byte[] bites = new byte[fs.Length];
                        fs.Read(bites, 0, bites.Length);

                        string base64ImageRepresentation = Convert.ToBase64String(bites);

                        result = String.Format("data:image/{0};base64,{1}", "png", base64ImageRepresentation);
                    }

                }
            }
        }


        PhoenixInspectionRiskAssessmentMiscellaneousExtn.InsertRiskAssessmentMiscellaneous(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                   General.GetNullableString(txtlName.Text),
                                                                   Convert.ToInt32(ViewState["TYPE"].ToString()),
                                                                   General.GetNullableInteger(null)
                                                                   );


        ucStatus.Text = "Information updated.";
        BindPPE();

    }

    protected void UpdateActivity()
    {
        if (!IsValidRAActivity())
        {
            ucError.Visible = true;
            return;
        }

        string result = "";


        foreach (UploadedFile postedFile in RadUpload1.UploadedFiles)
        {
            if (!Object.Equals(postedFile, null))
            {
                if (postedFile.ContentLength > 0)
                {

                    if (postedFile.ContentLength > (60 * 1024))
                    {
                        ucError.ErrorMessage = "Uploaded Photo size cannot exceed 60kb.";
                        ucError.Visible = true;
                        return;
                    }

                    using (MemoryStream stream = new MemoryStream())
                    {
                        string path = HttpContext.Current.Server.MapPath("..\\Attachments\\TEMP\\") + postedFile.FileName;
                        postedFile.SaveAs(path);

                        FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        byte[] bites = new byte[fs.Length];
                        fs.Read(bites, 0, bites.Length);

                        string base64ImageRepresentation = Convert.ToBase64String(bites);

                        result = String.Format("data:image/{0};base64,{1}", "png", base64ImageRepresentation);
                    }

                }
            }
        }

        PhoenixInspectionRiskAssessmentMiscellaneousExtn.UpdateRiskAssessmentMiscellaneousImage(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                   int.Parse(ViewState["PPEID"].ToString()),
                                                                   General.GetNullableString(txtlName.Text),
                                                                   General.GetNullableString(result)
                                                                   );

        ucStatus.Text = "Information updated.";

    }

    private bool IsValidRAActivity()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtlName.Text.Trim()) == null)
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);
    }

    protected void BindFormPosters()
    {
        DataSet dss = PhoenixInspectionRiskAssessmentMiscellaneousExtn.PPEEPSSList(ViewState["UNIQUEID"] == null ? null : General.GetNullableInteger(ViewState["UNIQUEID"].ToString()));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0)
        {
            tblForms.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                RadCheckBox cb = new RadCheckBox();
                cb.ID = dr["FLDEPSSID"].ToString();
                cb.Text = "";
                cb.Checked = true;
                cb.AutoPostBack = true;
                cb.CheckedChanged += new EventHandler(cb_CheckedChanged);
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDNAME"].ToString();
                hl.ID = "hlink" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");

                int type = 0;
                PhoenixIntegrationQuality.GetSelectedeTreeNodeType(General.GetNullableGuid(dr["FLDEPSSID"].ToString()), ref type);
                if (type == 2)
                    hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentGeneral.aspx?showall=0&DOCUMENTID=" + dr["FLDEPSSID"].ToString() + "');return false;");
                else if (type == 3)
                    hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../DocumentManagement/DocumentManagementDocumentSectionContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDEPSSID"].ToString() + "');return false;");
                else if (type == 5)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(dr["FLDEPSSID"].ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = ds.Tables[0].Rows[0];
                        hl.Attributes.Add("onclick", "Openpopup('codehelp1', '', '../StandardForm/StandardFormFBformView.aspx?FORMTYPE=DMSForm&FORMID=" + dr["FLDEPSSID"].ToString() + "&FORMREVISIONID=" + drr["FLDFORMREVISIONID"].ToString() + "');return false;");
                    }
                }
                else if (type == 6)
                {
                    DataSet ds = PhoenixIntegrationQuality.FormEdit(
                                            PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                            , new Guid(dr["FLDEPSSID"].ToString()));

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = ds.Tables[0].Rows[0];
                        if (drr["FLDFORMREVISIONDTKEY"] != null && General.GetNullableGuid(drr["FLDFORMREVISIONDTKEY"].ToString()) != null)
                        {
                            DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(drr["FLDFORMREVISIONDTKEY"].ToString()));
                            if (dt.Rows.Count > 0)
                            {
                                DataRow drRow = dt.Rows[0];
                                hl.Target = "_blank";
                                hl.NavigateUrl = "../Common/download.aspx?dtkey=" + drRow["FLDDTKEY"].ToString();
                            }
                        }
                    }
                }

                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tc.Controls.Add(cb);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hl);
                tr.Cells.Add(tc);
                tblForms.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblForms.Rows.Add(tr);
                number = number + 1;
            }
            divForms.Visible = true;
        }
        else
            divForms.Visible = false;
    }

    protected void lnkFormAdd_Click(object sender, EventArgs e)
    {
        if (General.GetNullableGuid(txtDocumentId.Text) != null)
        {

            PhoenixInspectionRiskAssessmentMiscellaneousExtn.UpdatePPEEPSS(int.Parse(ViewState["UNIQUEID"].ToString()), new Guid(txtDocumentId.Text));
            ucStatus.Text = "EPSS added.";
            txtDocumentId.Text = "";
            txtDocumentName.Text = "";
            BindFormPosters();
        }
    }

    void cb_CheckedChanged(object sender, EventArgs e)
    {
        RadCheckBox c = (RadCheckBox)sender;
        if (c.Checked == false)
        {
            PhoenixInspectionRiskAssessmentMiscellaneousExtn.PPEEPSSDelete(int.Parse(ViewState["UNIQUEID"].ToString()), new Guid(c.ID));

            string txt = "";

            ucStatus.Text = txt + "deleted.";
            BindFormPosters();
        }
    }
}