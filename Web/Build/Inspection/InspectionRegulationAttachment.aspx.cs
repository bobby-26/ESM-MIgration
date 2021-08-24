using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class Inspection_InspectionRegulationAttachment : PhoenixBasePage
{
    string attachmentcode = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (String.IsNullOrWhiteSpace(Request.QueryString["dtkey"]) == false)
        {
            attachmentcode = Request.QueryString["dtkey"];
        }
    }

    public void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string type = string.Empty;
        DataSet ds = new DataSet();

        if (attachmentcode != string.Empty)
        {
            ds = PhoenixCommonFileAttachment.AttachmentSearch(new Guid(attachmentcode), null, type, null, null,
                                                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                        General.ShowRecords(null),
                                                                        ref iRowCount, ref iTotalPageCount);

            gvRegulationFileAttachment.DataSource = ds;
            gvRegulationFileAttachment.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
    }

    protected void gvRegulationFileAttachment_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRegulationFileAttachment.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvRegulationFileAttachment_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            RadLabel lblFileName = ((RadLabel)e.Item.FindControl("lblFileName"));
            RadLabel lblDtkey = ((RadLabel)e.Item.FindControl("lbldtkey"));
            if (lblFileName.Text != string.Empty && lblDtkey.Text != string.Empty)
            {
                HyperLink lnk = (HyperLink)e.Item.FindControl("lnkfilename");
                lnk.NavigateUrl = "../Common/download.aspx?dtkey=" + lblDtkey.Text;
            }
        }
    }
    protected void gvRegulationFileAttachment_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {

    }


}