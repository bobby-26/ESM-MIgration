using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
public partial class InspectionSealRequisition : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionSealRequisition.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSealRequisition')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('Filter','','"+Session["sitepath"]+"/Inspection/InspectionSealRequisitionFilter.aspx'); return false;", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionSealRequisition.aspx", "Clear", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuSealReq.AccessRights = this.ViewState;
            MenuSealReq.MenuList = toolbargrid.Show();
            //MenuSealReq.SetTrigger(pnlSealReq);

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                ViewState["REQUESTID"] = null;
                Filter.CurrentSealRequesitionFilter = null;
                Filter.CurrentSelectedSealRequisition = null;
                ViewState["CURRENTTAB"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["COMPANYID"] = "";
                NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
                if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                {
                    ViewState["COMPANYID"] = nvc.Get("QMS");
                }
                gvSealRequisition.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            // BindData();
           // SetPageNavigator();
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
            string[] alColumns = { "FLDVESSELNAME", "FLDREFERENCENO", "FLDREQUESTDATE", "FLDREQUESTSTATUS" };
            string[] alCaptions = { "Vessel Name", "Request No", "Requested Date", "Request Status" };
            string sortexpression;
            int? sortdirection = null;
            int? vesselid = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            NameValueCollection nvc = Filter.CurrentSealRequesitionFilter;

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            if (nvc != null)
            {
                if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                    vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
            }

            DataSet ds = PhoenixInspectionSealRequisition.SearchSealRequisition(vesselid
                            , nvc != null ? nvc.Get("txtRefNo") : null
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                            , sortexpression, sortdirection
                            , 1
                            , iRowCount, ref iRowCount, ref iTotalPageCount
                            , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

            General.ShowExcel("Seal Requisition", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSealReq_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvSealRequisition.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ViewState["REQUESTID"] = null;
                Filter.CurrentSelectedSealRequisition = null;
                Filter.CurrentSealRequesitionFilter = null;
                BindData();
                gvSealRequisition.Rebind();
            }
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
            int? vesselid = null;
            string[] alColumns = { "FLDVESSELNAME", "FLDREFERENCENO", "FLDREQUESTDATE", "FLDREQUESTSTATUS" };
            string[] alCaptions = { "Vessel Name", "Request No", "Requested Date", "Request Status" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            NameValueCollection nvc = Filter.CurrentSealRequesitionFilter;

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

            if (nvc != null)
            {
                if (General.GetNullableInteger(nvc.Get("ucVessel")) != null)
                    vesselid = General.GetNullableInteger(nvc.Get("ucVessel"));
            }

            DataSet ds = PhoenixInspectionSealRequisition.SearchSealRequisition(vesselid
                            , nvc != null ? nvc.Get("txtRefNo") : null
                            , General.GetNullableDateTime(nvc != null ? nvc["txtFromDate"] : string.Empty)
                            , General.GetNullableDateTime(nvc != null ? nvc["txtToDate"] : string.Empty)
                            , General.GetNullableInteger(nvc != null ? nvc["ddlStatus"] : string.Empty)
                            , sortexpression, sortdirection
                            , int.Parse(ViewState["PAGENUMBER"].ToString())
                            , gvSealRequisition.PageSize, ref iRowCount, ref iTotalPageCount
                            , General.GetNullableInteger(ViewState["COMPANYID"].ToString()));

            General.SetPrintOptions("gvSealRequisition", "Seal Requisition", alCaptions, alColumns, ds);
            if (ds.Tables[0].Rows.Count > 0)
            {
                gvSealRequisition.DataSource = ds;
              

                if (Filter.CurrentSelectedSealRequisition == null)
                {
                    ViewState["REQUESTID"] = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();
                    Filter.CurrentSelectedSealRequisition = ds.Tables[0].Rows[0]["FLDREQUESTID"].ToString();

                    //ResetMenu();
                }

                if (ViewState["CURRENTTAB"] == null)
                {
                    ViewState["CURRENTTAB"] = "../Inspection/InspectionSealRequisitionGeneral.aspx";
                }

                ifMoreInfo.Attributes["src"] = "../Inspection/InspectionSealRequisitionGeneral.aspx?requestid=" + ViewState["REQUESTID"];

            }
            else
            {

                gvSealRequisition.DataSource = ds;
                ViewState["CURRENTTAB"] = "../Inspection/InspectionSealRequisitionGeneral.aspx";
                ifMoreInfo.Attributes["src"] = "../Inspection/InspectionSealRequisitionGeneral.aspx";
            }
            gvSealRequisition.VirtualItemCount = iRowCount;
            //SetTabHighlight();
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
           // SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealRequisition_Sorting(object sender, GridViewSortEventArgs se)
    {
        try
        {
            ViewState["STORETYPEID"] = null;
            ViewState["SORTEXPRESSION"] = se.SortExpression;

            if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
                ViewState["SORTDIRECTION"] = 1;
            else
                ViewState["SORTDIRECTION"] = 0;
            ViewState["REQUESTID"] = null;
            Filter.CurrentSelectedSealRequisition = null;
            BindData();
           // SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvSealRequisition_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    try
    //    {
    //        GridView _gridView = sender as GridView;
    //        gvSealRequisition.SelectedIndex = se.NewSelectedIndex;
    //        string requestid = ((Label)_gridView.Rows[se.NewSelectedIndex].FindControl("lblRequestId")).Text;
    //        ViewState["REQUESTID"] = requestid;
    //        Filter.CurrentSelectedSealRequisition = requestid;
    //        BindData();
    //        SetPageNavigator();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void gvSealRequisition_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    try
    //    {
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        if (e.Row.RowType == DataControlRowType.Header)
    //        {
    //            if (ViewState["SORTEXPRESSION"] != null)
    //            {
    //                HtmlImage img = (HtmlImage)e.Row.FindControl(ViewState["SORTEXPRESSION"].ToString());
    //                if (img != null)
    //                {
    //                    if (ViewState["SORTDIRECTION"] == null || ViewState["SORTDIRECTION"].ToString() == "0")
    //                        img.Src = Session["images"] + "/arrowUp.png";
    //                    else
    //                        img.Src = Session["images"] + "/arrowDown.png";

    //                    img.Visible = true;
    //                }
    //            }
    //        }
    //        if (e.Row.RowType == DataControlRowType.DataRow)
    //        {
    //            ImageButton ab = (ImageButton)e.Row.FindControl("cmdConfirm");
    //            Label lblStatusId = (Label)e.Row.FindControl("lblStatusId");
    //            if (ab != null)
    //            {
    //                ab.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm ?')");
    //            }
    //            if (lblStatusId != null)
    //            {
    //                if (lblStatusId.Text == PhoenixCommonRegisters.GetHardCode(1, 196, "PEN"))
    //                {
    //                    if (ab != null) ab.Visible = true;
    //                }
    //                else
    //                {
    //                    if (ab != null) ab.Visible = false;
    //                }
    //            }

    //            ImageButton cmdCancel = (ImageButton)e.Row.FindControl("cmdCancel");
    //            Label lblRequestId = (Label)e.Row.FindControl("lblRequestId");
    //            if (cmdCancel != null && lblRequestId != null)
    //            {
    //                cmdCancel.Attributes.Add("onclick", "javascript:parent.Openpopup('CancelReq','','../Inspection/InspectionSealRequisitionCancel.aspx?REQUESTID=" + lblRequestId.Text + "','large'); return true;");
    //            }

    //            if (ab != null)
    //            {
    //                if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName)) ab.Visible = false;
    //            }

    //            if (cmdCancel != null)
    //            {
    //                if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
    //                    cmdCancel.Visible = true;
    //                else
    //                    cmdCancel.Visible = false;

    //                if (!SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName))
    //                    cmdCancel.Visible = false;
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvSealRequisition_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        try
        {
            GridView _gridView = sender as GridView;
            int nCurrentRow = e.RowIndex;
            string requestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestId")).Text;
            string reqstatus = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStatus")).Text;
            ViewState["REQUESTID"] = requestid;
            Filter.CurrentSelectedSealRequisition = requestid;
            PhoenixInspectionSealRequisition.ConfirmSealRequesition(new Guid(requestid));
            BindData();
           // SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
           // SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void cmdGo_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        int result;
    //        if (Int32.TryParse(txtnopage.Text, out result))
    //        {
    //            ViewState["PAGENUMBER"] = Int32.Parse(txtnopage.Text);

    //            if ((int)ViewState["TOTALPAGECOUNT"] < Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = ViewState["TOTALPAGECOUNT"];

    //            if (0 >= Int32.Parse(txtnopage.Text))
    //                ViewState["PAGENUMBER"] = 1;

    //            if ((int)ViewState["PAGENUMBER"] == 0)
    //                ViewState["PAGENUMBER"] = 1;

    //            txtnopage.Text = ViewState["PAGENUMBER"].ToString();
    //        }
    //        ViewState["REQUESTID"] = null;
    //        Filter.CurrentSelectedSealRequisition = null;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    try
    //    {
    //        gvSealRequisition.SelectedIndex = -1;
    //        gvSealRequisition.EditIndex = -1;
    //        if (ce.CommandName == "prev")
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //        else
    //            ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //        ViewState["REQUESTID"] = null;
    //        Filter.CurrentSelectedSealRequisition = null;
    //        BindData();
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private void SetPageNavigator()
    //{
    //    try
    //    {
    //        cmdPrevious.Enabled = IsPreviousEnabled();
    //        cmdNext.Enabled = IsNextEnabled();
    //        lblPagenumber.Text = "Page " + ViewState["PAGENUMBER"].ToString();
    //        lblPages.Text = " of " + ViewState["TOTALPAGECOUNT"].ToString() + " Pages. ";
    //        lblRecords.Text = "(" + ViewState["ROWCOUNT"].ToString() + " records found)";
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    //private Boolean IsPreviousEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iTotalPageCount == 0)
    //        return false;

    //    if (iCurrentPageNumber > 1)
    //        return true;

    //    return false;
    //}

    //private Boolean IsNextEnabled()
    //{
    //    int iCurrentPageNumber;
    //    int iTotalPageCount;

    //    iCurrentPageNumber = (int)ViewState["PAGENUMBER"];
    //    iTotalPageCount = (int)ViewState["TOTALPAGECOUNT"];

    //    if (iCurrentPageNumber < iTotalPageCount)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //private void ShowNoRecordsFound(DataTable dt, GridView gv)
    //{
    //    try
    //    {
    //        dt.Rows.Add(dt.NewRow());
    //        gv.DataSource = dt;
    //        gv.DataBind();

    //        int colcount = gv.Columns.Count;
    //        gv.Rows[0].Cells.Clear();
    //        gv.Rows[0].Cells.Add(new TableCell());
    //        gv.Rows[0].Cells[0].ColumnSpan = colcount;
    //        gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
    //        gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
    //        gv.Rows[0].Cells[0].Font.Bold = true;
    //        gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
    //        gv.Rows[0].Attributes["onclick"] = "";
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = 1;
            if (Filter.CurrentSelectedSealRequisition == null)
                ViewState["REQUESTID"] = null;
       
             BindData();
            gvSealRequisition.Rebind();
            for (int i = 0; i < gvSealRequisition.MasterTableView.DataKeyNames.Length; i++)
            {
                if (gvSealRequisition.MasterTableView.DataKeyNames[i] == Filter.CurrentSelectedSealRequisition)
                {
                    gvSealRequisition.Items[i].Selected = true;
                    break;
                }
            }
           
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvSealRequisition_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;

    //        if (e.CommandName.ToUpper().Equals("CONFIRM"))
    //        {
    //            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //            BindPageURL(nCurrentRow);
    //            SetRowSelection();
    //            string requestid = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblRequestId")).Text;
    //            string reqstatus = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblStatus")).Text;
    //            ViewState["REQUESTID"] = requestid;
    //            Filter.CurrentSelectedSealRequisition = requestid;
    //            PhoenixInspectionSealRequisition.ConfirmSealRequesition(new Guid(requestid));                
    //            BindData();
    //        }
    //        if (e.CommandName.ToUpper().Equals("CANCELREQUISITION"))
    //        {
    //            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //            BindPageURL(nCurrentRow);
    //            SetRowSelection();
    //            BindData();
    //            SetPageNavigator();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private void BindPageURL(int rowindex)
    {
        try
        {
            RadLabel lblRequestId = (RadLabel)gvSealRequisition.Items[rowindex].FindControl("lblRequestId");
            if (lblRequestId != null)
            {
                Filter.CurrentSelectedSealRequisition = lblRequestId.Text;
                ViewState["REQUESTID"] = lblRequestId.Text;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        if (ViewState["QUOTATIONID"] == null || ViewState["QUOTATIONID"].ToString() == string.Empty)
            return;


        foreach (GridDataItem item in gvSealRequisition.MasterTableView.Items)
        {
            if (gvSealRequisition.MasterTableView.Items[0].GetDataKeyValue("FLDREQUESTID").ToString().Equals(ViewState[""].ToString()))
            {

                ViewState["DTKEY"] = ((RadLabel)item.FindControl("lblRequestId")).Text;
                break;
            }
        }
     
      
    }

    protected void gvSealRequisition_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSealRequisition.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    

    protected void gvSealRequisition_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.Item is GridDataItem)
            {
                RadGrid _gridView = (RadGrid)sender;
                int nCurrentRow = e.Item.ItemIndex;

                if (e.CommandName.ToUpper().Equals("SORT"))
                    return;

                if (e.CommandName.ToUpper().Equals("CONFIRM"))
                {             
                    BindPageURL(nCurrentRow);
                    SetRowSelection();
                    string requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                    string reqstatus = ((RadLabel)e.Item.FindControl("lblStatus")).Text;
                    ViewState["REQUESTID"] = requestid;
                    Filter.CurrentSelectedSealRequisition = requestid;
                    PhoenixInspectionSealRequisition.ConfirmSealRequesition(new Guid(requestid));
                    BindData();
                    gvSealRequisition.Rebind();
                }
                if (e.CommandName.ToUpper().Equals("CANCELREQUISITION"))
                {                   
                    BindPageURL(nCurrentRow);
                    SetRowSelection();
                    BindData();
                    gvSealRequisition.Rebind();

                }
                if (e.CommandName.ToUpper().Equals("SELECT"))
                {
                    string requestid = ((RadLabel)e.Item.FindControl("lblRequestId")).Text;
                    ViewState["REQUESTID"] = requestid;
                    Filter.CurrentSelectedSealRequisition = requestid;
                    BindData();
                    gvSealRequisition.Rebind();
                }
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

    protected void gvSealRequisition_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
         
           if (e.Item is GridDataItem)
            {
                LinkButton ab = (LinkButton)e.Item.FindControl("cmdConfirm");
                RadLabel lblStatusId = (RadLabel)e.Item.FindControl("lblStatusId");
                if (ab != null)
                {
                    ab.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure you want to confirm ?')");
                }
                if (lblStatusId != null)
                {
                    if (lblStatusId.Text == PhoenixCommonRegisters.GetHardCode(1, 196, "PEN"))
                    {
                        if (ab != null) ab.Visible = true;
                    }
                    else
                    {
                        if (ab != null) ab.Visible = false;
                    }
                }

                LinkButton cmdCancel = (LinkButton)e.Item.FindControl("cmdCancel");
                RadLabel lblRequestId = (RadLabel)e.Item.FindControl("lblRequestId");
                if (cmdCancel != null && lblRequestId != null)
                {
                    cmdCancel.Attributes.Add("onclick", "javascript:parent.openNewWindow('CancelReq','','" + Session["sitepath"] + "/Inspection/InspectionSealRequisitionCancel.aspx?REQUESTID=" + lblRequestId.Text + "','large'); return true;");
                }

                if (ab != null)
                {
                    if (!SessionUtil.CanAccess(this.ViewState, ab.CommandName)) ab.Visible = false;
                }

                if (cmdCancel != null)
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                        cmdCancel.Visible = true;
                    else
                        cmdCancel.Visible = false;

                    if (!SessionUtil.CanAccess(this.ViewState, cmdCancel.CommandName))
                        cmdCancel.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealRequisition_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var dataItem = gvSealRequisition.SelectedItems[0] as GridDataItem;
            if (dataItem.KeyValues  != null)
            {
                var name = dataItem.KeyValues.ToString();
               

                //// gvSealRequisition.SelectedIndexes = e.NewSelectedIndex;
                string requestid = (name);
                ViewState["REQUESTID"] = requestid;
                Filter.CurrentSelectedSealRequisition = requestid;
                BindData();
                gvSealRequisition.Rebind();
                // SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
