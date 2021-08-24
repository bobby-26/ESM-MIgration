using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerPatchQuery : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbaredit = new PhoenixToolbar();
        toolbaredit.AddButton("Save", "SAVE");
        MenuBugAttachment.AccessRights = this.ViewState;
        MenuBugAttachment.MenuList = toolbaredit.Show();

        BindData();
    }

    protected void MenuBugAttachment_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;


        if (dce.CommandName.ToUpper().Equals("SAVE"))
        {
            if (txtSubject.Text.Trim().Length == 0)
            {
                txtSubject.Text = "Specify a Subject";
                return;
            }

            HttpFileCollection postedFiles = Request.Files;
            if (Request.Files["filPatchAttachment"].ContentLength > 0)
            {
                string origpath = HttpContext.Current.Request.MapPath("~/");

                for (int i = 0; i < postedFiles.Count; i++)
                {
                    string path = "";
                    HttpPostedFile postedFile = postedFiles[i];
                    path = origpath + "Attachments\\Patch\\Query";


                    if (postedFile.ContentLength > 0)
                    {
                        string dtkey = Guid.NewGuid().ToString();

                        string filepath = path;
                        if(File.Exists(path + "\\" + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\')+1)))
                        {
                            ucError.ErrorMessage = "The file already exists. Cannot upload file with same name.";
                            ucError.Visible = true;
                            break;
                        }
                        string newfilename = Guid.NewGuid().ToString() + "_" + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\') + 1);
                        postedFile.SaveAs(path + "\\" + newfilename);
                        PhoenixDefectTracker.PatchQueryInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                            txtSubject.Text, newfilename, filepath);
                    }
                }
            }
            BindData();
        }
    }

    private void BindData()
    {
        DataTable dt = new DataTable();

        dt = PhoenixDefectTracker.PatchQueryList();

        gvAttachment.DataSource = dt;
        gvAttachment.DataBind();
    }

    protected void gvAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string path = "";
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFileName = (Label)e.Row.FindControl("lblFileName");
            HyperLink lnk = (HyperLink)e.Row.FindControl("lnkfilename");
            path = Session["sitepath"].ToString() + "/Attachments/Patch/Query/";

            lnk.NavigateUrl = path + lblFileName.Text;
        }
    }
}
