using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Purchase;
using System.IO;
using System.Web;

public partial class PurchaseAttachment : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();

            if (Request.QueryString["launchedfrom"] != null && Request.QueryString["launchedfrom"].ToString().ToUpper().Equals("ACCOUNTS"))
            {
                ViewState["launchedfrom"] = Request.QueryString["launchedfrom"].ToString();
                ViewState["CopyDTKey"] = Request.QueryString["copydtkey"].ToString();
                toolbar.AddButton("Copy", "COPY");
                ttlAttachment.ShowMenu = "false";
                MenuAttachment.TabStrip = "false";
            }
            else
            {
                toolbar.AddButton("Attachment", "ATTACHMENTS");
                toolbar.AddButton("Form", "FORM");
                MenuAttachment.TabStrip = "true";
            }

            MenuAttachment.MenuList = toolbar.Show();
            MenuAttachment.SetTrigger(pnlInvoice);

            if (Request.QueryString["orderid"] != null)
            {
                ViewState["orderid"] = Request.QueryString["orderid"].ToString();
                ViewState["ATTCHMENTTYPE"] = "";
            }

            if (Request.QueryString["DTKEY"] != null)
            {
                ViewState["DTKey"] = Request.QueryString["DTKEY"].ToString();
            }
            else
            {
                int? vesselid = null;

                if (ViewState["launchedfrom"] != null && ViewState["launchedfrom"].ToString().Equals("ACCOUNTS"))
                    vesselid = null;
                else
                    vesselid = Filter.CurrentPurchaseVesselSelection;

                DataSet ds = PhoenixPurchaseOrderForm.EditOrderForm(new Guid(ViewState["orderid"].ToString()), vesselid);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    PhoenixPurchaseOrderForm.FormNumber = dr["FLDFORMNO"].ToString();
                    ViewState["DTKey"] = dr["FLDDTKEY"].ToString();
                    ViewState["VesselId"] = dr["FLDVESSELID"].ToString();
                }
            }
            BindHard();
            ttlAttachment.Text = "Attachment    (  " + PhoenixPurchaseOrderForm.FormNumber + "     )";

            if (ViewState["launchedfrom"] == null)
            {
                MenuAttachment.SelectedMenuIndex = 0;
            }

            ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.ACCOUNTS + "&type=" + ViewState["ATTCHMENTTYPE"].ToString();
            rblAttachmentType.SelectedIndex = 2;
            SetValue(null, null);
        }
    }

    protected void BindHard()
    {
        rblAttachmentType.DataSource = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 93, 0, "MST,OWA,SPD,INV");
        rblAttachmentType.DataTextField = "FLDHARDNAME";
        rblAttachmentType.DataValueField = "FLDHARDCODE";
        rblAttachmentType.DataBind();
    }

    protected void SetValue(object sender, EventArgs e)
    {
        if (rblAttachmentType.SelectedIndex != -1)
            ViewState["ATTCHMENTTYPE"] = rblAttachmentType.Items[rblAttachmentType.SelectedIndex].Text.Trim();

        ifMoreInfo.Attributes["src"] = "../Common/CommonFileAttachment.aspx?DTKEY=" + ViewState["DTKey"].ToString() + "&MOD=" + PhoenixModule.PURCHASE + "&type=" + ViewState["ATTCHMENTTYPE"].ToString();
    }

    protected void MenuAttachment_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("FORM"))
        {
            Response.Redirect("../Purchase/PurchaseFormDetails.aspx?orderid=" + ViewState["orderid"].ToString());
        }
        if (dce.CommandName.ToUpper().Equals("COPY"))
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataSet ds = PhoenixCommonFileAttachment.AttachmentSearch(
                new Guid(ViewState["DTKey"].ToString()),
                null, null,
                null, null,
                1, 100, ref iRowCount, ref iTotalPageCount);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //string sourceDir = Session["sitepath"] + "/attachments/" + dr["FLDFILEPATH"].ToString();
                    //string backupDir = Session["sitepath"] + "Attachments/ACCOUNTS";
                    string sourceDir = Server.MapPath("~") + "\\Attachments\\" + dr["FLDFILEPATH"].ToString().Replace("/", "\\");
                    string backupDir = Server.MapPath("~") + "\\Attachments\\ACCOUNTS";
                    string filename = dr["FLDFILENAME"].ToString();
                    string olddtkey = dr["FLDDTKEY"].ToString();
                    string filepath = "ACCOUNTS/";
                    string filesize = dr["FLDFILESIZE"].ToString();
                    string attachmenttype = dr["FLDATTACHMENTTYPE"].ToString();

                    try
                    {
                        // Remove path from the file name. 
                        string fName = dr["FLDFILEPATH"].ToString().Replace("PURCHASE/", "");
                        string dtkey = string.Empty;

                        dtkey = PhoenixCommonFileAttachment.GenerateDTKey();
                        fName = fName.Replace(olddtkey, dtkey);
                        filepath = filepath + fName;

                        if (File.Exists(sourceDir))
                        {
                            try
                            {
                                // Use the Path.Combine //Path.Combine(sourceDir, fName)// method to safely append the file name to the path.
                                // Will overwrite if the destination file already exists.
                                File.Copy(sourceDir, backupDir + "\\" + fName, true);

                                PhoenixCommonFileAttachment.InsertAttachment(
                                    new Guid(ViewState["CopyDTKey"].ToString()),
                                    filename,
                                    filepath,
                                    long.Parse(filesize),
                                    General.GetNullableInteger(ViewState["VesselId"].ToString()),
                                    null, //sync yes no
                                    attachmenttype,
                                    new Guid(dtkey));

                            }
                            // Catch exception if the file was already copied. 
                            catch (IOException copyError)
                            {
                                ucError.ErrorMessage = copyError.Message;
                                ucError.Visible = true;
                            }
                        }
                    }
                    catch (DirectoryNotFoundException dirNotFound)
                    {
                        ucError.ErrorMessage = dirNotFound.Message;
                        ucError.Visible = true;
                    }
                }
            }
        }
    }
}
