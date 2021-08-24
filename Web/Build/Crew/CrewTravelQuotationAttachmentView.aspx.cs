using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;


public partial class CrewTravelQuotationAttachmentView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["TRAVELID"].ToString() != null && Request.QueryString["QUOTEID"].ToString() != null)
            {
                ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();
                ViewState["QUOTEID"] = Request.QueryString["QUOTEID"].ToString();
            }

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            gvAttachment.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDVESSELNAME", "FLDEVENTDATE", "FLDPORTNAME", "FLDOFFSIGNERCOUNT", "FLDONSIGNERCOUNT", "FLDPORTAGENTNAME", "FLDREMARKS", "FLDSTATUSNAME" };
            string[] alCaptions = { "Vessel", "Event Date", "Port", "Off Signer count", "On Signer count", "Port Agent", "Remarks", "Status", };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixCrewTravelQuoteLine.QuoteAttachmentSearch(
                                                                   General.GetNullableGuid(ViewState["TRAVELID"].ToString())
                                                                 , General.GetNullableGuid(ViewState["QUOTEID"].ToString())
                                                                 , sortexpression, sortdirection,
                                                                  int.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                  gvAttachment.PageSize,
                                                                  ref iRowCount,
                                                                  ref iTotalPageCount);

            General.SetPrintOptions("gvCCPlan", "Crew Change Event", alCaptions, alColumns, ds);

            gvAttachment.DataSource = ds;
            gvAttachment.VirtualItemCount = iRowCount;


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
                    html.InnerHtml = "<span class=\"icon\">" + imgclass + "</span>";
                    imgtype.Controls.Add(html);

                }

                RadLabel lblFilePath = (RadLabel)e.Item.FindControl("lblFilePath");


                HyperLink lnk = (HyperLink)e.Item.FindControl("lnkfilename");

                if (drv["FLDATTACHMENT"] != null && drv["FLDATTACHMENT"].ToString() != "")
                {
                    lnk.NavigateUrl = "../Common/download.aspx?dtkey=" + General.GetNullableGuid(drv["FLDATTACHMENT"].ToString());
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