using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web;
using SouthNests.Phoenix.CrewManagement;
using System.Text;
using Telerik.Web.UI;
using System.IO;
using System.Threading;

public partial class CrewTravelAttachmentView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["TICKETNO"].ToString() != null && Request.QueryString["ATTACHMENT"].ToString() != null)
            {
                ViewState["TICKETNO"] = Request.QueryString["TICKETNO"].ToString();
                ViewState["ATTACHMENT"] = Request.QueryString["ATTACHMENT"].ToString();
            }
        }
    }

    protected void BindData()
    {
        try
        {
            string Attachment = (ViewState["ATTACHMENT"] == null) ? null : (ViewState["ATTACHMENT"].ToString());

            string TicketNo = (ViewState["TICKETNO"] == null) ? null : (ViewState["TICKETNO"].ToString());

            DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelAttachmentMappingSearch(General.GetNullableGuid(Attachment), TicketNo);

            gvAttachment.DataSource = ds;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvAttachment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    protected void gvAttachment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            RadLabel lblFileName = ((RadLabel)e.Item.FindControl("lblFileName"));
            LinkButton imgtype = (LinkButton)e.Item.FindControl("imgfiletype");
            if (lblFileName.Text != string.Empty)
            {
                string imgclass = ResolveImageType(lblFileName.Text.Substring(lblFileName.Text.LastIndexOf('.')));
                
                if (imgclass != null)
                {
                    imgtype.Visible = true;
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\">"+imgclass+"</span>";
                    imgtype.Controls.Add(html);

                }
                
                RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");
                HyperLink lnk = (HyperLink)e.Item.FindControl("lnkfilename");

                if (ViewState["ATTACHMENT"] != null && ViewState["ATTACHMENT"].ToString() != "")
                {
                    lnk.NavigateUrl = "../Common/download.aspx?dtkey=" + General.GetNullableGuid(ViewState["ATTACHMENT"].ToString());
                }
            }

        }
    }
    
    protected void gvAttachment_ItemCommand(object sender, GridCommandEventArgs e)
    {

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


}