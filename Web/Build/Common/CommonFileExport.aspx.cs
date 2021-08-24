using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web;
using System.Collections.Generic;
using System.IO;
using Telerik.Web.UI;

public partial class CommonFileExport : PhoenixBasePage
{
    string attachmentcode = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(Request.QueryString["dtkey"]))
        {
            attachmentcode = Request.QueryString["dtkey"];
        }
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Export to PDF", "EXPORT", ToolBarDirection.Right);        
        MenuExport.AccessRights = this.ViewState;
        MenuExport.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;

            if (!String.IsNullOrEmpty(attachmentcode))
            {
                string type = Request.QueryString["type"] != null ? Request.QueryString["type"] : string.Empty;
            }

            gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

    }

    protected void MenuExport_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXPORT"))
            {
                if (gvAttachment.Items.Count > 0)
                {
                    List<String> FilePath = new List<string>();

                    for (int i = 0; i < gvAttachment.Items.Count; i++)
                    {
                        GridDataItem row = gvAttachment.Items[i];

                        bool isChecked = ((CheckBox)row.FindControl("chkExport")).Checked;

                        if (isChecked)
                        {
                            RadLabel path = ((RadLabel)row.FindControl("lblFilePath"));

                            string pathName = path.Text.ToLower();
                            string ext = Path.GetExtension(pathName);
                            if (ext == ".png" || ext == ".jpeg" || ext == ".jpg" || ext == ".gif" || ext == ".bmp")
                            {
                                string filename = Server.MapPath("~/Attachments/" + pathName);
                                bool exists = File.Exists(filename);
                                if (exists == true)
                                {
                                    FilePath.Add(filename);
                                }
                            }
                        }
                    }


                    if (FilePath.Count > 0)
                    {
                        Response.Clear();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("Content-Disposition", "attachment;Filename=Attachment.pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);

                        iTextSharp.text.Document doc = new iTextSharp.text.Document();
                        iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);
                        doc.Open();

                        foreach (string filename in FilePath)
                        {
                            iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(filename);
                            jpg.ScaleToFit(500f, 500f);
                            doc.Add(jpg);
                        }
                        doc.Close();
                        Response.Write(doc);
                        Response.End();
                    }
                    else
                    {
                        ucError.ErrorMessage = "Select a valid record to Export";
                        ucError.Visible = true;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        if (gvAttachment.Items.Count > 0)
        {
            List<String> FilePath = new List<string>();

            for (int i = 0; i < gvAttachment.Items.Count; i++)
            {
                GridDataItem row = gvAttachment.Items[i];

                bool isChecked = ((CheckBox)row.FindControl("chkExport")).Checked;

                if (isChecked)
                {
                    RadLabel path = ((RadLabel)row.FindControl("lblFilePath"));

                    string pathName = path.Text.ToLower();
                    string ext = Path.GetExtension(pathName);
                    if (ext == ".png" || ext == ".jpeg" || ext == ".jpg" || ext == ".gif" || ext == ".bmp")
                    {
                        string filename = Server.MapPath("~/Attachments/" + pathName);
                        bool exists = File.Exists(filename);
                        if (exists == true)
                        {
                            FilePath.Add(filename);
                        }
                    }
                }
            }


            if (FilePath.Count > 0)
            {
                Response.Clear();
                Response.ContentType = "application/pdf";
                Response.AddHeader("Content-Disposition", "attachment;Filename=Attachment.pdf");
                Response.Cache.SetCacheability(HttpCacheability.NoCache);

                iTextSharp.text.Document doc = new iTextSharp.text.Document();
                iTextSharp.text.pdf.PdfWriter.GetInstance(doc, Response.OutputStream);
                doc.Open();

                foreach (string filename in FilePath)
                {
                    iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(filename);
                    jpg.ScaleToFit(500f, 500f);
                    doc.Add(jpg);
                }
                doc.Close();
                Response.Write(doc);
                Response.End();
            }
            else
            {
                ucError.ErrorMessage = "Select a valid record to Export";
                ucError.Visible = true;
            }
        }
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

        if (attachmentcode != string.Empty)
        {
            string type = Request.QueryString["type"] != null ? Request.QueryString["type"] : string.Empty;


            ds = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(attachmentcode), null, type, sortexpression, sortdirection,
                                                                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                            gvAttachment.PageSize,
                                                                            ref iRowCount, ref iTotalPageCount);

        }


        gvAttachment.DataSource = ds;
        gvAttachment.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void gvAttachment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvAttachment.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvAttachment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblFileName = ((RadLabel)e.Item.FindControl("lblFileName"));
            Image imgtype = (Image)e.Item.FindControl("imgfiletype");
            if (lblFileName.Text != string.Empty)
            {
                imgtype.ImageUrl = ResolveImageType(lblFileName.Text.Substring(lblFileName.Text.LastIndexOf('.')));
                RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                HyperLink lnk = (HyperLink)e.Item.FindControl("lnkfilename");
                lnk.NavigateUrl = Session["sitepath"] + "/attachments/" + lblFilePath.Text;
            }
        }
    }

    protected void gvAttachment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvAttachment_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = e.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    private string ResolveImageType(string extn)
    {
        string imagepath = Session["images"] + "/";
        if (string.IsNullOrEmpty(extn)) extn = string.Empty;
        switch (extn.ToLower())
        {
            case ".jpg":
            case ".png":
            case ".gif":
            case ".bmp":
                imagepath += "imagefile.png";
                break;
            case ".doc":
            case ".docx":
                imagepath += "word.png";
                break;
            case ".xls":
            case ".xlsx":
            case ".xlsm":
                imagepath += "xls.png";
                break;
            case ".pdf":
                imagepath += "pdf.png";
                break;
            case ".rar":
            case ".zip":
                imagepath += "rar.png";
                break;
            case ".txt":
                imagepath += "notepad.png";
                break;
            default:
                imagepath += "anyfile.png";
                break;
        }
        return imagepath;
    }



}
