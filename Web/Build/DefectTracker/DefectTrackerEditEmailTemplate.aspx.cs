using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTracker_DefectTrackerEditEmailTemplate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
       

        if (!IsPostBack)
        {
            PhoenixToolbar toolbarsave = new PhoenixToolbar();
            toolbarsave.AddButton("Save", "SAVE");
            MenuBugAttachment.AccessRights = this.ViewState;
            MenuBugAttachment.MenuList = toolbarsave.Show();            

            if (Request.QueryString["templateid"] != null)
            {
                string tmpid = Request.QueryString["templateid"].ToString();
                DataTable dt = PhoenixDefectTracker.GetMailTemplateList(General.GetNullableGuid(tmpid));
                BindTemplateData(dt);
            }
        }
    }

    private void BindTemplateData(DataTable dt)
    {
       txtEmailBody.Text = dt.Rows[0]["FLDBODY"].ToString();
       txtIncidentCode.Text = dt.Rows[0]["FLDINCIDENTCODE"].ToString();
       txtSubject.Text = dt.Rows[0]["FLDSUBJECT"].ToString();
       txtDescription.Text = dt.Rows[0]["FLDDESCRIPTION"].ToString();
       txtCc.Text = dt.Rows[0]["FLDCC"].ToString();
       ucModule.SelectedValue = dt.Rows[0]["FLDMODULEID"].ToString();
       ViewState["FILENAME"] = dt.Rows[0]["FLDFILENAMES"].ToString();      
    }

    protected void MenuSaveTemplate_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                //string origpath = HttpContext.Current.Request.MapPath("~/");
                string origpath = "E:\\PhoenixSupport\\";
                origpath = origpath + "Attachment\\Template";
         
                string filename = null;

                HttpFileCollection postedFiles = Request.Files;
                if (Request.Files["TemplateAttachment"].ContentLength > 0)
                {
                    for (int i = 0; i < postedFiles.Count; i++)
                    {
                        HttpPostedFile postedFile = postedFiles[i];
                        if (postedFile.ContentLength > 0)
                        {

                            if (File.Exists(origpath + "\\" + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\') + 1)))
                            {
                                File.Delete(origpath + "\\" + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\') + 1));
                            }

                            postedFile.SaveAs(origpath + "\\" + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\') + 1));
                            filename = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\') + 1);
                        }
                    }
                }
                if (txtIncidentCode.Text != "" && txtDescription.Text != "" && txtSubject.Text != "" && txtEmailBody.Text != "")
                {
                    if ((filename == null || filename == "") && (Request.QueryString["templateid"] != null))
                    {
                        filename = ViewState["FILENAME"].ToString();
                    }
                    string tempid = String.IsNullOrEmpty(Request.QueryString["templateid"]) ? "" : Request.QueryString["templateid"];
                    PhoenixDefectTracker.InsertEmailTemplate(General.GetNullableGuid(tempid)
                                                                , txtIncidentCode.Text
                                                                , txtDescription.Text
                                                                , txtCc.Text
                                                                , txtSubject.Text
                                                                , txtEmailBody.Text
                                                                , filename
                                                                ,General.GetNullableInteger(ucModule.SelectedValue.ToString())
                                                             );
                }              
                string Script = "";
                Script += "<script language='javaScript' id='BookMarkScript'>" + "\n";
                Script += "parent.CloseCodeHelpWindow('MoreInfo');";
                Script += "</script>" + "\n";
                Page.ClientScript.RegisterStartupScript(typeof(Page), "BookMarkScript", Script);

                if (Request.QueryString["templateid"] != null)
                {
                    ucStatus.Text = "Template information updated";
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

}
