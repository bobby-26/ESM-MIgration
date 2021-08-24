using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class CrewLeaveRecord : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddImageButton("../Crew/CrewLeaveRecord.aspx", "Export to Excel", "icon_xls.png", "Excel");
                toolbar.AddImageLink("javascript:CallPrint('gvLVR')", "Print Grid", "icon_print.png", "PRINT");
              //  toolbar.AddImageLink("javascript:Openpopup('Filter','','CrewLeaveRecordFilter.aspx'); return false;", "Filter", "search.png", "FIND");
                toolbar.AddImageLink("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewLeaveRecordFilter.aspx',false);", "Filter", "search.png", "FIND");
                toolbar.AddImageButton("../Crew/CrewLeaveRecord.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
                MenuLeaveRecord.AccessRights = this.ViewState;
                MenuLeaveRecord.MenuList = toolbar.Show();                

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

              
            }
          //  BindData();
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuLeaveRecord_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
          
            if (CommandName.ToUpper().Equals("FIND"))
            {
               
                ViewState["PAGENUMBER"] = 1;
                BindData();
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CrewLeaveRecordFilter = null;
                BindData();
                Rebind();
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
        gvLVR.SelectedIndexes.Clear();
        gvLVR.EditIndexes.Clear();
        gvLVR.DataSource = null;
        gvLVR.Rebind();
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDVESSELNAME", "FLDLEAVEEARNED", "FLDLEAVEUNPAID", "FLDBTBEARNED", "FLDBTBUNPAID" };
        string[] alCaptions = { "Crew name", "Rank", "Vessel", "Leave Earned", "Leave UnPaid", "BTB Earned", "BTB UnPaid" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

         NameValueCollection nvc = Filter.CrewLeaveRecordFilter;
            DataTable dt = PhoenixCrewLeaveRecord.SearchCrewLeaveRecord(nvc != null ? nvc.Get("txtName") : string.Empty
                                        , nvc != null ? nvc.Get("txtFileNo") : string.Empty
                                        , General.GetNullableInteger(nvc != null ? nvc.Get("ddlRank") : string.Empty)
                                        , General.GetNullableInteger(nvc != null ? nvc.Get("ddlVessel") : string.Empty) ,
                                        sortexpression, sortdirection,
                                        Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                        iRowCount,
                                        ref iRowCount,
                                        ref iTotalPageCount);

        General.ShowExcel("Leave Records", dt, alColumns, alCaptions, sortdirection, sortexpression);

    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDVESSELNAME", "FLDLEAVEEARNED", "FLDLEAVEUNPAID", "FLDBTBEARNED", "FLDBTBUNPAID" };
            string[] alCaptions = { "Crew name", "Rank", "Vessel", "Leave Earned", "Leave UnPaid", "BTB Earned", "BTB UnPaid" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 0; //DEFAULT DESC SORT
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            NameValueCollection nvc = Filter.CrewLeaveRecordFilter;
            DataTable dt = PhoenixCrewLeaveRecord.SearchCrewLeaveRecord(nvc != null ? nvc.Get("txtName") : string.Empty
                                                        , nvc != null ? nvc.Get("txtFileNo") : string.Empty
                                                        , General.GetNullableInteger(nvc != null ? nvc.Get("ddlRank") : string.Empty)
                                                        , General.GetNullableInteger(nvc != null ? nvc.Get("ddlVessel") : string.Empty)
                                                        , sortexpression, sortdirection
                                                        , (int)ViewState["PAGENUMBER"], gvLVR.PageSize
                                                        , ref iRowCount, ref iTotalPageCount);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvLVR", "Leave Record", alCaptions, alColumns, ds);



            gvLVR.DataSource = dt;
            gvLVR.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
         
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvLVR_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLVR.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLVR_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
           
            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string employeeid = ((RadTextBox)e.Item.FindControl("txtEmployeeIdAdd")).Text;
                string rank = ((UserControlRank)e.Item.FindControl("ddlRankAdd")).SelectedRank;
                string vessel = ((UserControlVessel)e.Item.FindControl("ddlVesselAdd")).SelectedVessel;
                string leaveearned = ((UserControlMaskNumber)e.Item.FindControl("txtLeaveEarnedAdd")).Text;
                string leaveunpaid = ((UserControlMaskNumber)e.Item.FindControl("txtLeaveUnPadidAdd")).Text;
                string btbearned = ((UserControlMaskNumber)e.Item.FindControl("txtBTBEarnedAdd")).Text;
                string btbunpaid = ((UserControlMaskNumber)e.Item.FindControl("txtBTBUnPaidAdd")).Text;
                string fromdate = ((UserControlDate)e.Item.FindControl("txtFromDateAdd")).Text;
                string todate = ((UserControlDate)e.Item.FindControl("txtToDateAdd")).Text;
                if (!IsValidLeaveRecord(employeeid, rank, vessel, leaveearned, leaveunpaid, btbearned, btbunpaid, fromdate, todate))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewLeaveRecord.InsertCrewLeaveRecord(int.Parse(employeeid), int.Parse(vessel), int.Parse(rank), decimal.Parse(leaveearned), decimal.Parse(leaveunpaid)
                    , decimal.Parse(btbearned), decimal.Parse(btbunpaid), DateTime.Parse(fromdate), DateTime.Parse(todate));
                Rebind();

            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                Guid dtkey = new Guid(((RadLabel)e.Item.FindControl("lbldtkey")).Text);
                string rank = ((UserControlRank)e.Item.FindControl("ddlRankEdit")).SelectedRank;
                string vessel = ((UserControlVessel)e.Item.FindControl("ddlVesselEdit")).SelectedVessel;
                string leaveearned = ((UserControlMaskNumber)e.Item.FindControl("txtLeaveEarnedEdit")).Text;
                string leaveunpaid = ((UserControlMaskNumber)e.Item.FindControl("txtLeaveUnPadidEdit")).Text;
                string btbearned = ((UserControlMaskNumber)e.Item.FindControl("txtBTBEarnedEdit")).Text;
                string btbunpaid = ((UserControlMaskNumber)e.Item.FindControl("txtBTBUnPaidEdit")).Text;
                string fromdate = ((UserControlDate)e.Item.FindControl("txtFromDateEdit")).Text;
                string todate = ((UserControlDate)e.Item.FindControl("txtToDateEdit")).Text;

                if (!IsValidLeaveRecord("1", rank, vessel, leaveearned, leaveunpaid, btbearned, btbunpaid, fromdate, todate))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCrewLeaveRecord.UpdateCrewLeaveRecord(dtkey, int.Parse(vessel), int.Parse(rank), decimal.Parse(leaveearned), decimal.Parse(leaveunpaid)
                    , decimal.Parse(btbearned), decimal.Parse(btbunpaid), DateTime.Parse(fromdate), DateTime.Parse(todate));

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                Guid dtkey = new Guid(((RadLabel)e.Item.FindControl("lbldtkey")).Text);
                PhoenixCrewLeaveRecord.DeleteCrewLeaveRecord(dtkey);
            }


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvLVR_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

            LinkButton trans = (LinkButton)e.Item.FindControl("cmdTransfer");
            if (trans != null)
            {
                trans.Visible = SessionUtil.CanAccess(this.ViewState, trans.CommandName);
                trans.Attributes.Add("onclick", "javascript:openNewWindow('LAR', '','" + Session["sitepath"] + "/Accounts/AccountsLeaveBTBTransfer.aspx?fileno=" + drv["FLDFILENO"].ToString() + "',false);");
            
            }

            UserControlVessel ucVessel = (UserControlVessel)e.Item.FindControl("ddlVesselEdit");
            if (ucVessel != null) ucVessel.SelectedVessel = drv["FLDVESSELID"].ToString();

            UserControlRank ucRank = (UserControlRank)e.Item.FindControl("ddlRankEdit");
            if (ucRank != null) ucRank.SelectedRank = drv["FLDRANKID"].ToString();
        }
      //  else if (e.Item.RowType == DataControlRowType.Footer)
           LinkButton ADD = (LinkButton)e.Item.FindControl("cmdAdd");
        {
            HtmlGenericControl gc = (HtmlGenericControl)e.Item.FindControl("spnPickListEmployeeAdd");
            ImageButton emp = (ImageButton)e.Item.FindControl("btnShowEmployeeAdd");
            if (emp != null) emp.Attributes.Add("onclick", "showPickList('" + gc.ClientID + "', 'codehelp1', '', '../Common/CommonPickListEmployee.aspx', false); return false;");
        }
    }

    //protected void gvLVR_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        if (e.CommandName.ToUpper().Equals("SORT"))
    //            return;

    //        GridView _gridView = (GridView)sender;

    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        if (e.CommandName.ToUpper().Equals("ADD"))
    //        {
    //            string employeeid = ((TextBox)_gridView.FooterRow.FindControl("txtEmployeeIdAdd")).Text;
    //            string rank = ((UserControlRank)_gridView.FooterRow.FindControl("ddlRankAdd")).SelectedRank;
    //            string vessel = ((UserControlVessel)_gridView.FooterRow.FindControl("ddlVesselAdd")).SelectedVessel;
    //            string leaveearned = ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtLeaveEarnedAdd")).Text;
    //            string leaveunpaid = ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtLeaveUnPadidAdd")).Text;
    //            string btbearned = ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtBTBEarnedAdd")).Text;
    //            string btbunpaid = ((UserControlMaskNumber)_gridView.FooterRow.FindControl("txtBTBUnPaidAdd")).Text;
    //            string fromdate = ((UserControlDate)_gridView.FooterRow.FindControl("txtFromDateAdd")).Text;
    //            string todate = ((UserControlDate)_gridView.FooterRow.FindControl("txtToDateAdd")).Text;
    //            if (!IsValidLeaveRecord(employeeid, rank, vessel, leaveearned, leaveunpaid, btbearned, btbunpaid, fromdate, todate))
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }
    //            PhoenixCrewLeaveRecord.InsertCrewLeaveRecord(int.Parse(employeeid), int.Parse(vessel), int.Parse(rank), decimal.Parse(leaveearned), decimal.Parse(leaveunpaid)
    //                , decimal.Parse(btbearned), decimal.Parse(btbunpaid), DateTime.Parse(fromdate), DateTime.Parse(todate));
    //            BindData();                
    //        }                       
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}



    //protected void gvLVR_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        Guid dtkey = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());            
    //        string rank = ((UserControlRank)_gridView.Rows[nCurrentRow].FindControl("ddlRankEdit")).SelectedRank;
    //        string vessel = ((UserControlVessel)_gridView.Rows[nCurrentRow].FindControl("ddlVesselEdit")).SelectedVessel;
    //        string leaveearned = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtLeaveEarnedEdit")).Text;
    //        string leaveunpaid = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtLeaveUnPadidEdit")).Text;
    //        string btbearned = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtBTBEarnedEdit")).Text;
    //        string btbunpaid = ((UserControlMaskNumber)_gridView.Rows[nCurrentRow].FindControl("txtBTBUnPaidEdit")).Text;
    //        string fromdate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtFromDateEdit")).Text;
    //        string todate = ((UserControlDate)_gridView.Rows[nCurrentRow].FindControl("txtToDateEdit")).Text;

    //        if (!IsValidLeaveRecord("1", rank, vessel, leaveearned, leaveunpaid, btbearned, btbunpaid, fromdate, todate))
    //        {
    //            ucError.Visible = true;
    //            return;
    //        }
    //        PhoenixCrewLeaveRecord.UpdateCrewLeaveRecord(dtkey, int.Parse(vessel), int.Parse(rank), decimal.Parse(leaveearned), decimal.Parse(leaveunpaid)
    //            , decimal.Parse(btbearned), decimal.Parse(btbunpaid), DateTime.Parse(fromdate), DateTime.Parse(todate));
    //        _gridView.EditIndex = -1;
    //        BindData();            

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void gvLVR_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        Guid dtkey = new Guid(_gridView.DataKeys[nCurrentRow].Value.ToString());
    //        PhoenixCrewLeaveRecord.DeleteCrewLeaveRecord(dtkey);
    //        _gridView.EditIndex = -1;
    //        BindData();

    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void gvLVR_RowDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    DataRowView drv = (DataRowView)e.Row.DataItem;
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");            
    //        if (db != null)
    //        {
    //            db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
    //            db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
    //        }

    //        ImageButton edit = (ImageButton)e.Row.FindControl("cmdEdit");
    //        if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

    //        ImageButton save = (ImageButton)e.Row.FindControl("cmdSave");
    //        if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

    //        ImageButton cancel = (ImageButton)e.Row.FindControl("cmdCancel");
    //        if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

    //        ImageButton trans = (ImageButton)e.Row.FindControl("cmdTransfer");
    //        if (trans != null)
    //        {
    //            trans.Visible = SessionUtil.CanAccess(this.ViewState, trans.CommandName);
    //            trans.Attributes.Add("onclick", "Openpopup('LAR', '', '../Accounts/AccountsLeaveBTBTransfer.aspx?fileno=" + drv["FLDFILENO"].ToString() + "');");     
    //        }

    //        UserControlVessel ucVessel = (UserControlVessel)e.Row.FindControl("ddlVesselEdit");            
    //        if (ucVessel != null) ucVessel.SelectedVessel = drv["FLDVESSELID"].ToString();         

    //        UserControlRank ucRank = (UserControlRank)e.Row.FindControl("ddlRankEdit");
    //        if (ucRank != null) ucRank.SelectedRank = drv["FLDRANKID"].ToString();
    //    }
    //    else if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //        HtmlGenericControl gc = (HtmlGenericControl)e.Row.FindControl("spnPickListEmployeeAdd");            
    //        ImageButton emp = (ImageButton)e.Row.FindControl("btnShowEmployeeAdd");
    //        if (emp != null) emp.Attributes.Add("onclick", "showPickList('" + gc.ClientID + "', 'codehelp1', '', '../Common/CommonPickListEmployee.aspx', false); return false;");
    //    }
    //}
   
    private bool IsValidLeaveRecord(string employeeid, string rank, string vessel, string leaveearned
        , string leaveunpaid, string btbearned, string btbunpaid
        , string fromdate, string todate)
    {

        ucError.HeaderMessage = "Please provide the following required information";
        DateTime resultDate;

        if (!General.GetNullableInteger(employeeid).HasValue)
            ucError.ErrorMessage = "Crew Name is required.";

        if (!General.GetNullableInteger(rank).HasValue)
            ucError.ErrorMessage = "Rank is required.";

        if (!General.GetNullableInteger(vessel).HasValue)
            ucError.ErrorMessage = "Vessel Name is required.";

        if (!General.GetNullableDecimal(leaveearned).HasValue)
            ucError.ErrorMessage = "Leave Earned is required.";

        if (!General.GetNullableDecimal(leaveunpaid).HasValue)
            ucError.ErrorMessage = "Leave UnPaid is required.";

        if (!General.GetNullableDecimal(btbearned).HasValue)
            ucError.ErrorMessage = "BTB Earned is required.";

        if (!General.GetNullableDecimal(btbunpaid).HasValue)
            ucError.ErrorMessage = "BTB UnPaid is required.";

        if (!General.GetNullableDateTime(fromdate).HasValue)
            ucError.ErrorMessage = "From Date is required.";
        else if (DateTime.TryParse(fromdate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current";
        }        

        if (!General.GetNullableDateTime(todate).HasValue)
            ucError.ErrorMessage = "To Date is required.";

        else if (DateTime.TryParse(todate, out resultDate) && DateTime.Compare(resultDate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "To Date should be earlier than current";
        }
        else if (General.GetNullableDateTime(fromdate).HasValue && DateTime.TryParse(todate, out resultDate) && DateTime.Compare(DateTime.Parse(fromdate), resultDate) > 0)
        {
            ucError.ErrorMessage = "To Date should be later than From Date";
        }
        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();        
    }
  
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
