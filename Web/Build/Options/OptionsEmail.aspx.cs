using System;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;

public partial class OptionsEmail : PhoenixBasePage 
{
    DataTable myTable = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {        
        try
        {
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");


            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Send", "SEND");
            toolbarmain.AddButton("Save", "DRAFT");
            toolbarmain.AddButton("Discard", "DISCARD");
            EmailMenu.MenuList = toolbarmain.Show();
            if (ViewState["mailsessionid"] != null)
            {
                string attchpath = Server.MapPath("~/Attachments/EmailAttachments/" + ViewState["mailsessionid"].ToString()).ToString();
                DirectoryInfo di = new DirectoryInfo(attchpath);
                if (di.Exists)
                {
                    lstAttachments.Items.Clear();
                    FileInfo[] fi = di.GetFiles();
                    foreach (FileInfo f in fi)
                    {
                        lstAttachments.Items.Add(new ListItem(f.Name, f.Name));
                    }
                }
            }
            if (!IsPostBack)
            {
                DataTable tblattachment = new DataTable();
                tblattachment.Columns.Add("FileName");
                DataRow rowattachment = null;
                lstAttachments.Attributes["onkeypress"] = "javascript:DeleteFiles(event);";
                //Button1.Attributes.Add("onclick", "Openpopup('Attachments', 'Attachments', 'Attachment.aspx'); return false;"); 
                ViewState["mailsessionid"] = System.DateTime.Now.Day.ToString() + "_" + System.DateTime.Now.Month.ToString() + "_" + System.DateTime.Now.Year.ToString() + "_" + System.DateTime.Now.Hour.ToString() + "_" + System.DateTime.Now.Minute.ToString() + "_" + System.DateTime.Now.Second.ToString() + "_" + System.DateTime.Now.Millisecond.ToString();
                DataTable dt = PhoenixCrewAddress.ListEmployeeAddress(Convert.ToInt32(Filter.CurrentCrewSelection));
                if (dt.Rows.Count > 0)
                    txtTO.Text = dt.Rows[0]["FLDEMAIL"].ToString();
                if (Request.QueryString["mailId"] != null && Request.QueryString["sessionId"] != null)
                {
                    PhoenixMail.Folder folder = (PhoenixMail.Folder)Enum.Parse(typeof(PhoenixMail.Folder), Request.QueryString["mailFolderId"]);
                    string destinationpath = string.Empty;
                    if (folder == PhoenixMail.Folder.Draft || folder == PhoenixMail.Folder.Delete)
                    {
                        ViewState["mailsessionid"] = Request.QueryString["sessionId"];
                        destinationpath = Server.MapPath("~/Attachments/EmailAttachments/" + ViewState["mailsessionid"].ToString()).ToString();
                    }
                    else
                    {
                        destinationpath = Server.MapPath("~/Attachments/EmailAttachments/" + ViewState["mailsessionid"].ToString()).ToString();
                        if (!Directory.Exists(destinationpath)) Directory.CreateDirectory(destinationpath);
                        string sendsessionid = Server.MapPath("~/Attachments/EmailAttachments/") + Request.QueryString["sessionId"];
                        if (Directory.Exists(sendsessionid))
                        {
                            DirectoryInfo di = new DirectoryInfo(sendsessionid);
                            FileInfo[] fi = di.GetFiles();
                            foreach (FileInfo f in fi)
                                f.CopyTo(destinationpath + "\\" + f.Name);
                        }
                    }
                    DataSet dsdraftmaildtl = PhoenixMail.MailList(General.GetNullableInteger(Filter.CurrentCrewSelection).Value, folder, Convert.ToInt32(Request.QueryString["mailId"]));

                    txtTO.Text = dsdraftmaildtl.Tables[0].Rows[0]["FLDTO"].ToString();
                    txtCC.Text = dsdraftmaildtl.Tables[0].Rows[0]["FLDCC"].ToString();
                    txtBCC.Text = dsdraftmaildtl.Tables[0].Rows[0]["FLDBCC"].ToString();
                    txtSubject.Text = dsdraftmaildtl.Tables[0].Rows[0]["FLDSUBJECT"].ToString();

                    edtBody.Content = dsdraftmaildtl.Tables[0].Rows[0]["FLDBODY"].ToString();
                    ddlPriority.SelectedValue = dsdraftmaildtl.Tables[0].Rows[0]["FLDPRIORITY"].ToString();

                    

                    if (System.IO.Directory.Exists(destinationpath))
                    {
                        DirectoryInfo dir = new DirectoryInfo(destinationpath);
                        FileInfo[] files = dir.GetFiles();
                        Array.Sort(files, (x, y) => x.CreationTime.CompareTo(y.CreationTime));

                        for (int i = 0; i < files.Length; i++)
                        {
                            lstAttachments.Items.Add(new ListItem(files[i].Name.ToString(), files[i].Name.ToString()));
                            
                            rowattachment = tblattachment.NewRow();
                            if (i + 1 != files.Length)
                                rowattachment["FileName"] = files[i].Name.ToString() + ",  ";
                            else                            
                                rowattachment["FileName"] = files[i].Name.ToString();
                            
                            tblattachment.Rows.Add(rowattachment);
                        }
                    }

                    if (folder != PhoenixMail.Folder.Draft)
                    {
                        pnlMain.Enabled = false;
                        edtBody.ActiveMode = AjaxControlToolkit.HTMLEditor.ActiveModeType.Preview;
                        rpAttachment.DataSource = tblattachment;
                        rpAttachment.DataBind();
                        foreach (RepeaterItem ri in rpAttachment.Items)
                        {
                            HyperLink hl = ri.FindControl("lnkAtt") as HyperLink;
                            hl.NavigateUrl = hl.NavigateUrl.Trim().TrimEnd(',');
                        }
                        lnkAttachment.Enabled = false;
                        lnkAttachment.OnClientClick = "";
                        lstAttachments.Visible = false;
                    }
                }             
            }
        }
        catch(Exception ex)
        {
            throw ex;
          
        }
    }

    protected void EmailMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

            if (dce.CommandName.ToUpper().Equals("SEND"))
            {
                if (!IsValidateEmail())
                {
                    ucError.Visible = true;
                    return;
                }
                SendMail();
            }
            if (dce.CommandName.ToUpper().Equals("DRAFT"))
            {
                SaveAsDraft();
            }
            if (dce.CommandName.ToUpper().Equals("DISCARD"))
            {
                clearFields();
            }
        }
        catch (Exception ex)
        {
            ucError.Text = ex.Message;
            ucError.Visible = true;
        }
    }       
 

    protected void SendMail()
    {
           
        try
        {
            if (Request.QueryString["mailId"] != null && Request.QueryString["mailFolderId"] != "2")
            {
                PhoenixMail.EmailChangeFolder(int.Parse(Request.QueryString["mailId"]), PhoenixMail.Folder.Sent);
            }
            else
            {
                PhoenixMail.SendMail(txtTO.Text.Trim(), txtCC.Text.Trim(), txtBCC.Text.Trim(), txtSubject.Text.Trim(), edtBody.Content, true, (MailPriority)Convert.ToInt32(ddlPriority.SelectedValue.ToString()), ViewState["mailsessionid"].ToString(), General.GetNullableInteger(Filter.CurrentCrewSelection).Value);
            }
            
            clearFields();

            Session["frmcontentpage"] = "OptionsEmailSent.aspx";
            Response.Redirect("OptionsEmailSent.aspx");
        }
        catch (Exception ex)
        {
            throw ex.InnerException;
        }
    }            

    private void clearFields()
    {
        //ViewState["mailsessionid"] = System.DateTime.Now.Day.ToString() + "_" + System.DateTime.Now.Month.ToString() + "_" + System.DateTime.Now.Year.ToString() + "_" + System.DateTime.Now.Hour.ToString() + "_" + System.DateTime.Now.Minute.ToString() + "_" + System.DateTime.Now.Second.ToString() + "_" + System.DateTime.Now.Millisecond.ToString();
        //txtTO.Text = "";
        //txtCC.Text = "";
        //txtBCC.Text = "";
        //txtSubject.Text = "";
        //lstAttachments.Items.Clear();
        //edtBody.Content = "";
        Session["frmcontentpage"] = "OptionsEmail.aspx";
        Response.Redirect("OptionsEmail.aspx");
        //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", "javascript:parent.frames['ifMoreInfo'].src='OptionsEmail.aspx';", true);
    }

    protected void SaveAsDraft()
    {
        try
        {
            if (Request.QueryString["mailId"] == null)
            {
                PhoenixMail.SaveMail(null, PhoenixMail.MailFrom, txtTO.Text.Trim(), txtCC.Text.Trim(), txtBCC.Text.Trim(), txtSubject.Text.Trim(), edtBody.Content, Convert.ToInt32(ddlPriority.SelectedValue.ToString()), PhoenixMail.Folder.Draft, ViewState["mailsessionid"].ToString(), General.GetNullableInteger(Filter.CurrentCrewSelection).Value);
            }
            else
            {
                PhoenixMail.SaveMail(Convert.ToInt32(Request.QueryString["mailId"]), PhoenixMail.MailFrom, txtTO.Text.Trim(), txtCC.Text.Trim(), txtBCC.Text.Trim(), txtSubject.Text.Trim(), edtBody.Content, Convert.ToInt32(ddlPriority.SelectedValue.ToString()), PhoenixMail.Folder.Draft, ViewState["mailsessionid"].ToString(), General.GetNullableInteger(Filter.CurrentCrewSelection).Value);
            }
            //PhoenixMail.SaveMail(Convert.ToInt32(Request.QueryString["mailId"]), "developer12@southnests.com", txtTO.Text.Trim(), txtCC.Text.Trim(), txtBCC.Text.Trim(), txtSubject.Text.Trim(), edtBody.Content, Convert.ToInt32(ddlPriority.SelectedValue.ToString()), PhoenixMail.Folder.Draft, ViewState["mailsessionid"].ToString());
            Session["frmcontentpage"] = "OptionsEmailDraft.aspx";
            Response.Redirect("OptionsEmailDraft.aspx");
        }
        catch (Exception ex)
        {


            throw ex;
           
            //General.BuildExceptionDetail(ex, sbError);
           // lblError.Text = sbError.ToString();
        }
        
    }

    protected void OpenAttachment(object sender, CommandEventArgs e)
    {
        try
        {
            string s = e.CommandArgument.ToString();
            //string frameScript = "<script language='javascript'>" + "window.parent.frmContent.location='EmailAttachments/" + ViewState["mailsessionid"].ToString()+"/"+ e.CommandArgument.ToString().Replace(",","").Trim()+"';</script>";
            //Response.Write(frameScript);
            string frameScript = "<script language='javascript'>" + "window.open('EmailAttachments/" + ViewState["mailsessionid"].ToString() + "/" + e.CommandArgument.ToString().Replace(",", "").Trim() + "');</script>";
            Response.Write(frameScript);
        }
        catch (Exception ex)
        {
            StringBuilder sbError = new StringBuilder();
            throw ex;
           
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        if (Session["AttachFiles"] != null)
        {
            lstAttachments.Items.Clear();
            DataTable dt = (DataTable)Session["AttachFiles"];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                lstAttachments.Items.Add(new ListItem(dt.Rows[i]["Text"].ToString(), dt.Rows[i]["Value"].ToString()));
            }
            Session["AttachFiles"] = null;
        }
    }

    private bool IsValidateEmail()
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (!General.IsvalidEmail(txtTO.Text))
        {
            ucError.ErrorMessage = "Please enter valid To E-Mail Address";
        }
        if (txtCC.Text.Trim() != string.Empty && !General.IsvalidEmail(txtTO.Text))
        {
            ucError.ErrorMessage = "Please enter valid Cc E-Mail Address";
        }
        if (txtBCC.Text.Trim() != string.Empty && !General.IsvalidEmail(txtBCC.Text))
        {
            ucError.ErrorMessage = "Please enter valid Bcc E-Mail Address";
        }
        return (!ucError.IsError);
    }
}
