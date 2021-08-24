using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;
public partial class CrewOffshoreDMRRequisitionandPODetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddImageButton("../CrewOffshore/CrewOffshoreDMRRequisitionandPODetails.aspx?type=" + Request.QueryString["type"] + "&vesselid=" + Request.QueryString["vesselid"] + "&reportdate=" + Request.QueryString["reportdate"], "Export to Excel", "icon_xls.png", "Excel");
            //MenuShowExcel.AccessRights = this.ViewState;
            //MenuShowExcel.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvRequisitionandPO.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
               // ShowReport();
            }
           
            //SetPageNavigator();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROW", "FLDPONUMBER", "FLDDATE", "FLDSTOCKTYPE", "FLDSTATUS" };
        string[] alCaptions = { "Sl No", "PO Number", "Date", "Type", "Status" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportRequisionsandPODetails(General.GetNullableInteger(Request.QueryString["type"].ToString()),
                                                                        General.GetNullableInteger(Request.QueryString["vesselid"].ToString()),
                                                                        General.GetNullableDateTime(Request.QueryString["reportdate"].ToString()),
                                                                        gvRequisitionandPO.CurrentPageIndex+1,
                                                                        gvRequisitionandPO.PageSize,
                                                                        ref iRowCount,
                                                                        ref iTotalPageCount);
        General.ShowExcel("Content-Disposition", ds.Tables[0], alColumns, alCaptions, 0, null);

        //Response.AddHeader("Content-Disposition", "attachment; filename=Requisitions_and_PO_Report.xls");
        //Response.ContentType = "application/vnd.msexcel";
        //Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center> SHIP MANAGEMENT</center></h5></td></tr>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Requisition and PO's </center></h5></td></tr>");
        //Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        //Response.Write("</tr>");
        //Response.Write("</TABLE>");
        //Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        //Response.Write("<tr>");
        //for (int i = 0; i < alCaptions.Length; i++)
        //{
        //    Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
        //    Response.Write("<b><center>" + alCaptions[i] + "</center></b>");
        //    Response.Write("</td>");
        //}
        //Response.Write("</tr>");
        //foreach (DataRow dr in ds.Tables[0].Rows)
        //{
        //    Response.Write("<tr>");
        //    for (int i = 0; i < alColumns.Length; i++)
        //    {
        //        Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
        //        Response.Write("<center>" + dr[alColumns[i]] + "</center>");
        //        Response.Write("</td>");
        //    }
        //    Response.Write("</tr>");
        //}
        //Response.Write("</TABLE>");
        //Response.End();
    }
    private void ShowReport()
    {
        ViewState["SHOWREPORT"] = 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixCrewOffshoreDMRMidNightReport.CrewOffshoreMidNightReportRequisionsandPODetails(General.GetNullableInteger(Request.QueryString["type"].ToString()),
                                                                        General.GetNullableInteger(Request.QueryString["vesselid"].ToString()),
                                                                        General.GetNullableDateTime(Request.QueryString["reportdate"].ToString()),
                                                                        gvRequisitionandPO.CurrentPageIndex+1,
                                                                        gvRequisitionandPO.PageSize,
                                                                        ref iRowCount,
                                                                        ref iTotalPageCount);

        gvRequisitionandPO.DataSource = ds;
        gvRequisitionandPO.VirtualItemCount = iRowCount;
        // gvRequisitionandPO.DataBind();
        ViewState["ROWSINGRIDVIEW"] = ds.Tables[0].Rows.Count - 1;
       


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvRequisitionandPO_RowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            if (ViewState["SORTEXPRESSION"] != null)
            {
                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
                if (img != null)
                {
                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
                        img.Src = Session["images"] + "/arrowUp.png";
                    else
                        img.Src = Session["images"] + "/arrowDown.png";

                    img.Visible = true;
                }
            }
        }
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

        }
    }

    //protected void gvRequisitionandPO_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;

    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;
    //        if (e.CommandName.ToUpper().Equals("ORDER"))
    //        {
    //            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //            LinkButton lnkPONo = (LinkButton)_gridView.Rows[nCurrentRow].FindControl("lnkPONo");
    //            Label lblOrderid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblOrderid");
    //            Filter.CurrentPurchaseVesselSelection = int.Parse(Request.QueryString["vesselid"].ToString());
    //            Filter.CurrentVesselConfiguration = "0";
    //            Session["launchedfrom"] = "DMR";

    //            NameValueCollection criteria = new NameValueCollection();
    //            criteria.Clear();
    //            criteria.Add("ucVessel", Request.QueryString["vesselid"].ToString());
    //            criteria.Add("ddlStockType", "");
    //            criteria.Add("txtNumber", lnkPONo.Text);
    //            criteria.Add("txtTitle", "");
    //            criteria.Add("txtVendorid", "");
    //            criteria.Add("txtDeliveryLocationId", "");
    //            criteria.Add("txtBudgetId", "");
    //            criteria.Add("txtBudgetgroupId", "");
    //            criteria.Add("ucFinacialYear", "");
    //            criteria.Add("ucFormState", "");
    //            criteria.Add("ucApproval", "");
    //            criteria.Add("UCrecieptCondition", "");
    //            criteria.Add("UCPeority", "");
    //            criteria.Add("ucFormStatus", "");
    //            criteria.Add("ucFormType", "");
    //            criteria.Add("ucComponentclass", "");
    //            criteria.Add("txtMakerReference", "");
    //            criteria.Add("txtOrderedDate", "");
    //            criteria.Add("txtOrderedToDate", "");
    //            criteria.Add("txtCreatedDate", "");
    //            criteria.Add("txtCreatedToDate", "");
    //            criteria.Add("txtApprovedDate", "");
    //            criteria.Add("txtApprovedToDate", "");

    //            Filter.CurrentOrderFormFilterCriteria = criteria;

    //            if (lblOrderid != null && lnkPONo != null)
    //                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
    //                             "Script", "Openpopup('BookMarkScript', '', '../Purchase/PurchaseForm.aspx?launchedfrom=DMR');", true);

    //        }

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;

    //    }
    //}

    protected void gvRequisitionandPO_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        LinkButton lnkPONo = (LinkButton)e.Item.FindControl("lnkPONo");
        if (lnkPONo != null)
            lnkPONo.Attributes.Add("onclick", "javascript:openNewWindow('CrewPage','','" + Session["sitepath"] + "/Purchase/PurchaseForm.aspx?launchedfrom=DMR'); return false;");

    }

    protected void gvRequisitionandPO_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ShowReport();
    }

    protected void gvRequisitionandPO_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
           

            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("ORDER"))
            {
                int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

                LinkButton lnkPONo = (LinkButton)e.Item.FindControl("lnkPONo");
                RadLabel lblOrderid = (RadLabel)e.Item.FindControl("lblOrderid");
                Filter.CurrentPurchaseVesselSelection = int.Parse(Request.QueryString["vesselid"].ToString());
                Filter.CurrentVesselConfiguration = "0";
                Session["launchedfrom"] = "DMR";

                NameValueCollection criteria = new NameValueCollection();
                criteria.Clear();
                criteria.Add("ucVessel", Request.QueryString["vesselid"].ToString());
                criteria.Add("ddlStockType", "");
                criteria.Add("txtNumber", lnkPONo.Text);
                criteria.Add("txtTitle", "");
                criteria.Add("txtVendorid", "");
                criteria.Add("txtDeliveryLocationId", "");
                criteria.Add("txtBudgetId", "");
                criteria.Add("txtBudgetgroupId", "");
                criteria.Add("ucFinacialYear", "");
                criteria.Add("ucFormState", "");
                criteria.Add("ucApproval", "");
                criteria.Add("UCrecieptCondition", "");
                criteria.Add("UCPeority", "");
                criteria.Add("ucFormStatus", "");
                criteria.Add("ucFormType", "");
                criteria.Add("ucComponentclass", "");
                criteria.Add("txtMakerReference", "");
                criteria.Add("txtOrderedDate", "");
                criteria.Add("txtOrderedToDate", "");
                criteria.Add("txtCreatedDate", "");
                criteria.Add("txtCreatedToDate", "");
                criteria.Add("txtApprovedDate", "");
                criteria.Add("txtApprovedToDate", "");

                Filter.CurrentOrderFormFilterCriteria = criteria;

            
                //ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(),
                //                 "Script", "Openpopup('BookMarkScript', '', '../Purchase/PurchaseForm.aspx?launchedfrom=DMR');", true);

            }

            if(e.CommandName == RadGrid.ExportToCsvCommandName)
            {
                gvRequisitionandPO.ExportSettings.IgnorePaging = true;
                ShowExcel();
            }
            if (e.CommandName == RadGrid.ExportToExcelCommandName)
            {
                gvRequisitionandPO.ExportSettings.IgnorePaging = true;
                ShowExcel();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvRequisitionandPO_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        gvRequisitionandPO.CurrentPageIndex = e.NewPageIndex + 1;
    }
}
