using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;
public partial class CrewTravelInvoice : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SessionUtil.PageAccessRights(this.ViewState);            

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;            
            ViewState["CURRENTROW"] = null;
            ViewState["FLDHOPLINEITEMID"] = null;            

            if (Request.QueryString["TRAVELID"] != null)
                ViewState["TRAVELID"] = Request.QueryString["TRAVELID"].ToString();

            if (Request.QueryString["port"] != null)
                ViewState["PORT"] = Request.QueryString["port"];
            if (Request.QueryString["date"] != null)
                ViewState["DATE"] = Request.QueryString["date"];
            if (Request.QueryString["vessel"] != null)
                ViewState["VESSEL"] = Request.QueryString["vessel"];
            if (Request.QueryString["travelrequestedit"] != null)
                ViewState["EDITTRAVELREQUEST"] = Request.QueryString["travelrequestedit"].ToString();

            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Travel Request", "TRAVEL");
            toolbarmain.AddButton("Travel Plan", "TRAVELPLAN");
            toolbarmain.AddButton("Quotation", "QUOTATION");
            toolbarmain.AddButton("Ticket", "TICKET");
            toolbarmain.AddButton("Invoice", "INVOICE");
            MenuTicket.AccessRights = this.ViewState;
            MenuTicket.MenuList = toolbarmain.Show();
            MenuTicket.SelectedMenuIndex = 4;                        
        }
        BindRoutingDetails();
        BindDataTravelBreakUp();

    }      
    private void BindRoutingDetails()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelTicketInvoiceSearch(new Guid(ViewState["TRAVELID"].ToString()), null, null
                    , int.Parse(ViewState["PAGENUMBER"].ToString()), General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
       

        if (ds.Tables[0].Rows.Count > 0)
        {
            gvLineItem.DataSource = ds.Tables[0];
            gvLineItem.DataBind();

            if (ViewState["FLDHOPLINEITEMID"] == null)
            {
                ViewState["FLDHOPLINEITEMID"] = ds.Tables[0].Rows[0]["FLDHOPLINEITEMID"].ToString();
            }
            SetRowSelection();
        }
        else
        {
            DataTable dt = ds.Tables[0];
            ShowNoRecordsFound(dt, gvLineItem);
        }       


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        SetPageNavigator();
    }
    private void SetRowSelection()
    {
        try
        {
            gvLineItem.SelectedIndex = -1;
            for (int i = 0; i < gvLineItem.Rows.Count; i++)
            {
                if (gvLineItem.DataKeys[i].Value.ToString().Equals(ViewState["FLDHOPLINEITEMID"].ToString()))
                {
                    gvLineItem.SelectedIndex = i;

                }
            }
        }

        catch (Exception EX)
        {
            ucError.ErrorMessage = EX.Message;
            ucError.Visible = true;
            return;
        }
    }        
    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        dt.Rows.Add(dt.NewRow());
        gv.DataSource = dt;
        gv.DataBind();

        int colcount = gv.Columns.Count;
        gv.Rows[0].Cells.Clear();
        gv.Rows[0].Cells.Add(new TableCell());
        gv.Rows[0].Cells[0].ColumnSpan = colcount;
        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
        gv.Rows[0].Cells[0].Font.Bold = true;
        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        gv.Rows[0].Attributes["onclick"] = "";
    }
    private void SetPageNavigator()
    {
        cmdPrevious.Enabled = IsPreviousEnabled();
        cmdNext.Enabled = IsNextEnabled();
        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    }
    private Boolean IsPreviousEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iTotalPageCount == 0)
            return false;

        if (iCurrentPageNumber > 1)
            return true;

        return false;
    }
    private Boolean IsNextEnabled()
    {
        int iCurrentPageNumber;
        int iTotalPageCount;

        iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
        iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

        if (iCurrentPageNumber < iTotalPageCount)
        {
            return true;
        }
        return false;
    }
    protected void cmdGo_Click(object sender, EventArgs e)
    {
        gvLineItem.EditIndex = -1;
        gvLineItem.SelectedIndex = -1;
        int result;
        if (Int32.TryParse(txtnopage.Text, out result))
        {
            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

            if (0 >= Int32.Parse(txtnopage.Text))
                ViewState["PAGENUMBER"] = 1;

            if ((int)ViewState["PAGENUMBER"] == 0)
                ViewState["PAGENUMBER"] = 1;

            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
        }


    }
    protected void PagerButtonClick(object sender, CommandEventArgs ce)
    {
        gvLineItem.SelectedIndex = -1;
        gvLineItem.EditIndex = -1;
        if (ce.CommandName == "prev")
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
        else
            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;


        SetPageNavigator();
    }
    protected void MenuTicket_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        try
        {         

            if (dce.CommandName.ToUpper().Equals("TRAVEL"))
            {
                Response.Redirect("CrewTravelRequest.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (dce.CommandName.ToUpper().Equals("TRAVELPLAN"))
            {
                Response.Redirect("CrewTravelRequestGeneral.aspx?from=travel&travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (dce.CommandName.ToUpper().Equals("QUOTATION"))
            {
                Response.Redirect("CrewTravelQuotationAgentDetail.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (dce.CommandName.ToUpper().Equals("TICKET"))
            {
                Response.Redirect("CrewTravelQuoteTicketList.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }
            if (dce.CommandName.ToUpper().Equals("INVOICE"))
            {
                Response.Redirect("CrewTravelInvoice.aspx?travelid=" + ViewState["TRAVELID"].ToString() + "&travelrequestedit=" + ViewState["EDITTRAVELREQUEST"].ToString(), false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLineItem_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
           
            if (e.CommandName.ToUpper() == "SELECT")
            {

                ViewState["FLDHOPLINEITEMID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblhoplineitemid")).Text;
                BindDataTravelBreakUp();
            }            

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindDataTravelBreakUp()
    {
        try
        {
            DataSet ds = PhoenixCrewTravelQuoteLine.CrewTravelticketInvoiceList(
                General.GetNullableGuid (ViewState["FLDHOPLINEITEMID"]==null ? null : ViewState["FLDHOPLINEITEMID"].ToString()));

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvCTBreakUp.DataSource = ds;
                gvCTBreakUp.DataBind();
            }
            else
            {
                DataTable dt = ds.Tables[0];
                ShowNoRecordsFound(dt, gvCTBreakUp);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

}
