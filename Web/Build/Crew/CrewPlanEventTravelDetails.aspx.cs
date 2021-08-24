using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewManagement;
public partial class CrewPlanEventTravelDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();

            ViewState["CREWEVENTDETAILID"] = Request.QueryString["eventdetailid"].ToString();
            ViewState["ONSIGNERYN"] = Request.QueryString["onsigneryn"].ToString();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["VESSELID"] = "";
                gvCrewTravelSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void Rebind()
    {
        gvCrewTravelSearch.SelectedIndexes.Clear();
        gvCrewTravelSearch.EditIndexes.Clear();
        gvCrewTravelSearch.DataSource = null;
        gvCrewTravelSearch.Rebind();
    }

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDREQUISITIONNO", "FLDCITYNAME", "FLDDESTINATIONCITY", "FLDARRIVALDATE", "FLDDEPARTUREDATE", "FLDTICKETNO" };
        string[] alCaptions = { "Requisition No", "Origin", "Destination", "Arrival", "Departure" , "Ticket No."};

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {

            DataSet ds = PhoenixCrewChangeEventDetail.CrewPlanEventDetailTravelSearch(General.GetNullableGuid(ViewState["CREWEVENTDETAILID"].ToString())
                                                                                       , General.GetNullableInteger(ViewState["ONSIGNERYN"].ToString())
                                                                                       , sortexpression
                                                                                       , sortdirection
                                                                                       , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                                       , gvCrewTravelSearch.PageSize
                                                                                       , ref iRowCount
                                                                                       , ref iTotalPageCount);
            General.SetPrintOptions("gvCrewTravelSearch", "Crew List", alCaptions, alColumns, ds);

            gvCrewTravelSearch.DataSource = ds;
            gvCrewTravelSearch.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }


    protected void gvCrewTravelSearch_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewTravelSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewTravelSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvCrewTravelSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton ibm = (LinkButton)e.Item.FindControl("cmdAttachmentMapping");
            RadLabel lbm = (RadLabel)e.Item.FindControl("lblAttachment");
            RadLabel ticket = (RadLabel)e.Item.FindControl("lblTicketNo");
            if (ibm != null && lbm.Text != null && ticket.Text != null)
            {
                RadLabel lblattachmentmappingyn = (RadLabel)e.Item.FindControl("lblattachmentmappingyn");
                if (lblattachmentmappingyn != null)
                {
                    if (!lblattachmentmappingyn.Text.Equals("1"))
                    {
                        HtmlGenericControl html = new HtmlGenericControl();
                        html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                        ibm.Controls.Add(html);
                    }
                }
                ibm.Attributes.Add("onclick", "javascript:openNewWindow('Attachment','Attach'," + "'" + Session["sitepath"] + "/Crew/CrewTravelAttachmentView.aspx?ATTACHMENT=" + lbm.Text + "&TICKETNO=" + ticket.Text + "')");
            }


        }
    }

    protected void gvCrewTravelSearch_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }
}
