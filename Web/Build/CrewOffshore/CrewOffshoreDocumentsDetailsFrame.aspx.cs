using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using System.Web;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;
using System.Text;

public partial class CrewOffshore_CrewOffshoreDocumentsDetailsFrame : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if(!IsPostBack)
        {
           
        }

    }

    protected void gvCertificateSchedule_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    public void BindData()
    {
        try
        {

            string[] alColumns = { "FLDDOCUMENTNAME", "FLDDOCUMENTNUMBER", "FLDISSUEDDATE", "FLDEXPIRYDATE", "FLDPLACEOFISSUE", "FLDCOUNTRYNAME" };
            string[] alCaptions = { "Document Name", "Document Number", "Issued Date", "Expiry Date", "Place Of Issue", "Country/Flag" };

            //DataSet ds = PhoenixCrewOffshorePersonalMasterOverview.CrewTravelDocsList(
            //            Convert.ToInt32(Filter.CurrentCrewSelection));
            DataSet ds = PhoenixCrewEmployeeDocument.ListCrewDocumentActiveVerified(
                              Int32.Parse(Filter.CurrentCrewSelection));
            General.SetPrintOptions("gvCrewDocs", "Document List", alCaptions, alColumns, ds);

            gvCrewDocs.DataSource = ds;

        }
        catch
        {
            //ucError.ErrorMessage = ex.Message;
            //ucError.Visible = true;
        }

    }

    protected void gvCrewDocs_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            RadLabel lblsubtype = (RadLabel)e.Item.FindControl("lblsubtype");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                string mimetype = ".pdf";

                if (lblsubtype.Text == "PASSPORT")
                {
                    att.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PASSPORT + "&cmdname=PASSPORTUPLOAD&U=1'); return false;";
                }
                if (lblsubtype.Text == "SEAMANBOOK")
                {
                    att.Attributes["onclick"] = "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.SEAMANBOOK + "&cmdname=SEAMANBOOKUPLOAD&U=1'); return false;";
                }

                if (lblsubtype.Text == "TRAVEL")
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=TRAVELDOCUPLOAD&U=1'); return false;");
                }
                if (lblsubtype.Text == "NATIONAL LICENCE")
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                           + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.LICENCE + "&cmdname=LICENCEUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2&U=1'); return false;");
                }
                if (lblsubtype.Text == "COURSE")
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.COURSE + "&cmdname=COURSEUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2&U=1'); return false;");
                }
                if (lblsubtype.Text == "OTHER")
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                          + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.DOCUMENTS + "&cmdname=OTHERDOCUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2&U=1'); return false;");
                }
                if (lblsubtype.Text == "MEDICAL TEST")
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=MEDICALTESTUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2&U=1'); return false;");
                }
                if (lblsubtype.Text == "MEDICAL")
                {
                    att.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                         + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.MEDICAL + "&cmdname=MEDICALUPLOAD&mimetype=" + mimetype + "&maxnoofattachments=2&U=1'); return false;");
                }


            }

        }
    }

    protected void gvCrewDocs_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if(e.CommandName== "Attachment")
        {
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            RadLabel lblsubtype = (RadLabel)e.Item.FindControl("lblsubtype");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
          
            String script = String.Format("javascript:openNewWindow('NATD','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                            + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.PASSPORT + "&cmdname=PASSPORTUPLOAD&U=1'); return false;");

            RadScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }
}