using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using Telerik.Web.UI;

public partial class InspectionRAHazardExtnAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuCARGeneral.AccessRights = this.ViewState;
            MenuCARGeneral.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["HAZARDID"] = "";
                ViewState["TYPE"] = "-1";

                if (Request.QueryString["HAZARDID"] != null && Request.QueryString["HAZARDID"].ToString() != string.Empty)
                    ViewState["HAZARDID"] = Request.QueryString["HAZARDID"].ToString();

                if (Request.QueryString["TYPE"] != null && Request.QueryString["TYPE"].ToString() != string.Empty)
                    ViewState["TYPE"] = Request.QueryString["TYPE"].ToString();

                BindActivity();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void BindActivity()
    {
        if (ViewState["HAZARDID"] != null && ViewState["HAZARDID"].ToString() != string.Empty)
        {
            DataSet dt = PhoenixInspectionRiskAssessmentHazardExtn.EditRiskAssessmentHazard(int.Parse(ViewState["HAZARDID"].ToString()));
            if (dt.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dt.Tables[0].Rows[0];
                txtlName.Text = dr["FLDNAME"].ToString();
                imgPhoto.Attributes.Add("src", dr["FLDIMAGE"].ToString());
                //byte[] imageBytes = Convert.FromBase64String(dr["FLDIMAGE"].ToString());
                //MemoryStream ms = new MemoryStream(imageBytes, 0,
                //  imageBytes.Length);

                //// Convert byte[] to Image
                //ms.Write(imageBytes, 0, imageBytes.Length);
                //Image image = Image.FromStream(ms, true);

                //string path = HttpContext.Current.Server.MapPath("..\\Attachments\\TEMP\\") + txtlName+".png";
                //image.Save(path);
                //imgPhoto.ImageUrl = path;
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
                if (ViewState["HAZARDID"] != null && ViewState["HAZARDID"].ToString() != string.Empty)
                    UpdateActivity();
                else
                    InsertActivity();
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
        

        PhoenixInspectionRiskAssessmentHazardExtn.InsertRiskAssessmentHazardwithImage(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
             General.GetNullableString(txtlName.Text), int.Parse(ViewState["TYPE"].ToString()), General.GetNullableString(result));


        ucStatus.Text = "Information updated.";
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
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

        PhoenixInspectionRiskAssessmentHazardExtn.UpdateRiskAssessmentHazardwithImage(PhoenixSecurityContext.CurrentSecurityContext.UserCode, int.Parse(ViewState["HAZARDID"].ToString()),
                  General.GetNullableString(txtlName.Text.Trim()),General.GetNullableString(result));

        ucStatus.Text = "Information updated.";
        String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
    }

    private bool IsValidRAActivity()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(txtlName.Text.Trim()) == null)
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);
    }
}