using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselAccounts;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Web.UI;
using Telerik.Web.UI;
using SouthNests.Phoenix.Accounts;

public partial class VesselAccountsCashAdvanceRequest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCashAdvanceRequest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCashAdvanceRequest')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            //toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCashAdvanceRequest.aspx?storeclass=" + Request.QueryString["storeclass"].ToString(), "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCashAdvanceRequest.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCashAdvanceRequest.aspx?requestid=", "Add New", "<i class=\"fa fa-plus-circle\"></i>", "NEW");
            //toolbargrid.AddFontAwesomeButton("../VesselAccounts/VesselAccountsCashAdvanceRequest.aspx?requestid=" + Request.QueryString["RequestId"].ToString(), "Add New", "<i class=\"fa fa-plus-circle\"></i>", "NEW");
            MenuCashAdvanceRequest.AccessRights = this.ViewState;
            MenuCashAdvanceRequest.MenuList = toolbargrid.Show();
            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["pageno"] == null ? "1" : Request.QueryString["pageno"].ToString());
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCashAdvanceRequest.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
            string[] alColumns = { "FLDDATE", "FLDSUPPLIERNAME", "FLDSEAPORTNAME" };
            string[] alCaptions = { "Date", "Supplier Name", "SeaPort Name" };
            string sortexpression;
            int? sortdirection = null;
            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
            DataSet ds = PhoenixVesselAccountsCashAdvanceRequest.SearchCashAdvanceRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                , null
                                                                , null
                                                                , sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);

            General.ShowExcel("Cash Advance Request", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Rebind()
    {
        gvCashAdvanceRequest.SelectedIndexes.Clear();
        gvCashAdvanceRequest.EditIndexes.Clear();
        //gvBondReq.DataSource = null;
        gvCashAdvanceRequest.Rebind();
    }

    protected void MenuCashAdvanceRequest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["REQUESTID"] = null;
                Filter.CurrentVesselOrderFormFilter = null;
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("NEW"))
                Response.Redirect("../VesselAccounts/VesselAccountsCashAdvanceRequestGeneral.aspx?pageno=" + ViewState["PAGENUMBER"], false);
            //Response.Redirect("../VesselAccounts/VesselAccountsCashAdvanceRequestGeneral.aspx?requestid=" + Request.QueryString["RequestId"].ToString() + "&pageno=" + ViewState["PAGENUMBER"], false);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDDATE", "FLDSUPPLIERNAME", "FLDSEAPORTNAME" };
            string[] alCaptions = { "Date", "Supplier Name", "SeaPort Name" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentVesselOrderFormFilter;
            DataSet ds = PhoenixVesselAccountsCashAdvanceRequest.SearchCashAdvanceRequest(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                            , null
                            , null
                            , sortexpression, sortdirection
                            , gvCashAdvanceRequest.CurrentPageIndex + 1
                            , gvCashAdvanceRequest.PageSize, ref iRowCount, ref iTotalPageCount);
            General.SetPrintOptions("gvCashAdvanceRequest", "Cash Advance Request", alCaptions, alColumns, ds);
            gvCashAdvanceRequest.DataSource = ds;
            gvCashAdvanceRequest.VirtualItemCount = iRowCount;
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCashAdvanceRequest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCashAdvanceRequest.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCashAdvanceRequest_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SELECT"))
            {

                string RequestId = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                string DtKey = ((RadLabel)e.Item.FindControl("lblDtKey")).Text;
                Response.Redirect("../VesselAccounts/VesselAccountsCashAdvanceRequestGeneral.aspx?requestid=" + RequestId + "&pageno=" + ViewState["PAGENUMBER"] + "&dtkey=" + DtKey, false);
            }
            else if (e.CommandName == "Page")
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

    protected void gvCashAdvanceRequest_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            if (att != null)
            {
                if (drv["FLDISATTACHMENT"].ToString() == "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes["onclick"] = "javascript:openNewWindow('NAFA','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + drv["FLDDTKEY"].ToString() + "&mod=" + PhoenixModule.VESSELACCOUNTS + "'); return false;";
            }
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        Rebind();
    }


}