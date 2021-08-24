using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using Telerik.Web.UI;
public partial class Crew_CrewTravelRequestAttachmentView : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                SessionUtil.PageAccessRights(this.ViewState);

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["EDITROW"] = "0";
                ViewState["CURRENTROW"] = null;
                ViewState["ROUTEID"] = null;

                if (Request.QueryString["REQUESTID"] != null)
                    ViewState["REQUESTID"] = Request.QueryString["REQUESTID"].ToString();
                edit();
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
        gvTravel.SelectedIndexes.Clear();
        gvTravel.EditIndexes.Clear();
        gvTravel.DataSource = null;
        gvTravel.Rebind();
    }

    private void edit()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        ds = PhoenixCrewTravelQuoteLine.CrewTravelRequestSearchTicket(
              new Guid(ViewState["REQUESTID"].ToString()), (int)ViewState["PAGENUMBER"],
               General.ShowRecords(null),
               ref iRowCount,
                   ref iTotalPageCount
               );

        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["FLDTICKETNO"] = ds.Tables[0].Rows[0]["FLDTICKETNO"].ToString();
            ViewState["FLDATTACHMENTDTKEY"] = ds.Tables[0].Rows[0]["FLDATTACHMENTDTKEY"].ToString();
        }

    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDNAME", "FLDTICKETNO" };
            string[] alCaptions = { "Name", "Ticket No" };

            DataSet ds = new DataSet();
            ds = PhoenixCrewTravelQuoteLine.CrewTravelRequestSearchTicket(
                  new Guid(ViewState["REQUESTID"].ToString()), (int)ViewState["PAGENUMBER"],
                   General.ShowRecords(null),
                   ref iRowCount,
                   ref iTotalPageCount);

            gvTravel.DataSource = ds;
            gvTravel.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvTravel_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            
            RadLabel lbm = (RadLabel)e.Item.FindControl("lblAttachmentdtkey");
            LinkButton ticket = (LinkButton)e.Item.FindControl("lblTicketNo");
            if (lbm != null && lbm.Text != null && ticket.Text != null)
            {
                lbm.Attributes.Add("onclick", "Openpopup('Attachment','Attach'," + "'../Crew/CrewTravelRequestAttachmentViewDetail.aspx?ATTACHMENT=" + lbm.Text + "&TICKETNO=" + ticket.Text + "')");
            }

            HyperLink lnk = (HyperLink)e.Item.FindControl("lnkfilename");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (drv["FLDATTACHMENTDTKEY"] != null && drv["FLDATTACHMENTDTKEY"].ToString() != "")
            {
                lnk.NavigateUrl = "../Crew/CrewTravelRequestAttachmentViewDetail.aspx?ATTACHMENT=" + ViewState["FLDATTACHMENTDTKEY"].ToString() + "&TICKETNO=" + ViewState["FLDTICKETNO"].ToString();
            }

        }
    }

    protected void gvTravel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;



            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                RadLabel lbm = (RadLabel)e.Item.FindControl("lblAttachmentdtkey");
                LinkButton ticket = (LinkButton)e.Item.FindControl("lblTicketNo");
                if (lbm != null && lbm.Text != null && ticket.Text != null)
                {

                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvTravel_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvTravel.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvTravel_SortCommand(object sender, GridSortCommandEventArgs e)
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