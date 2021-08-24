using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.CrewOperation;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class CrewWorkingGearOrderForm : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewWorkingGearOrderForm.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvWorkGearReq')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','" + Session["sitepath"] + "/Crew/CrewWorkingGearOrderFormFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewWorkingGearOrderForm.aspx", "Clear Filter", " <i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("../Crew/CrewWorkGearNeededItem.aspx", "Add New Requisition", "<i class=\"fas fa-plus-circle\"></i>", "ADD");
            MenuBondReq.AccessRights = this.ViewState;
            MenuBondReq.MenuList = toolbargrid.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;                

                gvWorkGearReq.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDREFNUMBER", "FLDNAME", "FLDREQUESTDATE", "FLDORDERSTATUS" };
            string[] alCaptions = { "Reference No", "Supplier", "Requested Date", "Status" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentWorkingGearOrderFormFilter;
            DataSet ds = PhoenixCrewWorkingGearOrderForm.SearchOrderForm(nvc != null ? nvc.Get("txtRefNo") : null
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                             , gvWorkGearReq.PageSize, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Working Gear Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuBondReq_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvWorkGearReq.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentWorkingGearOrderFormFilter = null;
                ViewState["PAGENUMBER"] = 1;
                gvWorkGearReq.CurrentPageIndex = 0;
                BindData();
                gvWorkGearReq.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvWorkGearReq_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvWorkGearReq.CurrentPageIndex + 1;

        BindData();
    }


    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDREFNUMBER", "FLDNAME", "FLDREQUESTDATE", "FLDORDERSTATUS" };
            string[] alCaptions = { "Reference No", "Supplier", "Requested Date", "Status" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CurrentWorkingGearOrderFormFilter;
            DataSet ds = PhoenixCrewWorkingGearOrderForm.SearchOrderForm(nvc != null ? nvc.Get("txtRefNo") : null
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                            , sortexpression, sortdirection
                            , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                            , gvWorkGearReq.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvWorkGearReq", "Working Gear Request", alCaptions, alColumns, ds);

            gvWorkGearReq.DataSource = ds;
            gvWorkGearReq.VirtualItemCount = iRowCount;
            
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void gvWorkGearReq_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().ToUpper() == "EDITREQUEST")
            {
                string orderid = ((RadLabel)e.Item.FindControl("lblOrderId")).Text;
                string Neededid = ((RadLabel)e.Item.FindControl("lblNeededId")).Text;
                string empid = ((RadLabel)e.Item.FindControl("lblempid")).Text;
                string vslid = ((RadLabel)e.Item.FindControl("lblvesselid")).Text;
                string crewplanid = ((RadLabel)e.Item.FindControl("lblcrewplanid")).Text;
                
                Response.Redirect("../Crew/CrewWorkGearNeededItem.aspx?Neededid=" + Neededid + "&empid=" + empid + "&vesselid=" + vslid + "&crewplanid=" + crewplanid + "&newreq=" + 1 + "&Needed=" + Neededid + "&Orderback=" + 1, false);

            }            
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvWorkGearReq_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
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
    
    protected void gvWorkGearReq_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton cmdAttachment = (LinkButton)e.Item.FindControl("cmdAtt");           
            if (cmdAttachment != null)
            {
                cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    cmdAttachment.Controls.Add(html);
                }

                cmdAttachment.Attributes.Add("onclick", "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod="
                    + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.WORKGAERINVOICE + "'); return false;");
            }
            
        }
    }
    
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["ORDERID"] = null;
            BindData();
            gvWorkGearReq.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidOrder(string supplier, string orderdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;
        if (!General.GetNullableDateTime(orderdate).HasValue)
        {
            ucError.ErrorMessage = "Order Date is required.";
        }
        else if (DateTime.TryParse(orderdate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "Order Date should be earlier than current date";
        }
        if (!General.GetNullableInteger(supplier).HasValue)
        {
            ucError.ErrorMessage = "Supplier Name is required.";
        }

        return (!ucError.IsError);
    }

}
