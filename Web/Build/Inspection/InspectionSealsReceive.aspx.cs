using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Collections;
using System.Web.UI;
using Telerik.Web.UI;
public partial class InspectionSealsReceive : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionSealsReceive.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSealNumber')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuSealExport.AccessRights = this.ViewState;
            MenuSealExport.MenuList = toolbargrid.Show();
            SetMenu();
            if (!IsPostBack)
            {
                ViewState["REQUESTLINEID"] = Request.QueryString["REQUESTLINEID"];
                ViewState["ISCONFIRMEDISSUE"] = null;
                ViewState["ISRECEIPTCONFIRMED"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                Filter.CurrentSelectedSealstoReceive = null;

                if (ViewState["REQUESTLINEID"] != null)
                    EditSealRequisitionLineItem(new Guid(ViewState["REQUESTLINEID"].ToString()));

                
                gvSealNumber.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
          //  BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditSealRequisitionLineItem(Guid gRequestlineId)
    {
        try
        {
            DataTable dt = PhoenixInspectionSealRequisition.EditSealRequesitionLineItem(gRequestlineId);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                ViewState["ISRECEIPTCONFIRMED"] = dr["FLDACTIVEYN"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetMenu()
    {
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Not Received", "NOTRECEIVED", ToolBarDirection.Right);
        toolbar.AddButton("Damaged", "DAMAGED", ToolBarDirection.Right);
        toolbar.AddButton("Receive", "RECEIVED",ToolBarDirection.Right);
        
        
        MenuSealNumber.AccessRights = this.ViewState;
        MenuSealNumber.MenuList = toolbar.Show();
    }

    protected void MenuSealExport_TabStripCommand(object sender, EventArgs e)
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

    protected void MenuSealNumber_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            ArrayList SelectedSeals = new ArrayList();
            Guid index = new Guid();
            foreach (GridDataItem gvrow in gvSealNumber.MasterTableView.Items)
            {
                bool result = false;
                index = new Guid(gvrow.GetDataKeyValue("FLDSEALID").ToString());

                if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                }

                // Check in the Session
                if (Filter.CurrentSelectedSealstoReceive != null)
                    SelectedSeals = (ArrayList)Filter.CurrentSelectedSealstoReceive;
                if (result)
                {
                    if (!SelectedSeals.Contains(index))
                        SelectedSeals.Add(index);
                }
                else
                    SelectedSeals.Remove(index);
            }
            if (SelectedSeals != null && SelectedSeals.Count > 0)
                Filter.CurrentSelectedSealstoReceive = SelectedSeals;
            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
            {
                ucError.ErrorMessage = "Only vessel can update the receipt status.";
                ucError.Visible = true;
                return;
            }
            if (CommandName.ToUpper().Equals("RECEIVED"))
            {
                string s = ",";
                if (Filter.CurrentSelectedSealstoReceive != null)
                {
                    ArrayList selectedseals = (ArrayList)Filter.CurrentSelectedSealstoReceive;
                    if (selectedseals != null && selectedseals.Count > 0)
                    {
                        foreach (Guid index1 in selectedseals)
                        { s = s + index1 + ","; }
                    }
                }
                PhoenixInspectionSealRequisition.UpdateReceivedSeals(new Guid(ViewState["REQUESTLINEID"].ToString())
                                                        , General.GetNullableString(s.Length > 1 ? s : null));

                Filter.CurrentSelectedSealstoReceive = null;
                BindData();
                gvSealNumber.Rebind();
                // SetPageNavigator();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //                     "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', true);", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                     "BookMarkScript", "fnReloadList('codehelp1', null, true);", true);
            }
            else if (CommandName.ToUpper().Equals("DAMAGED"))
            {
                string s = ",";
                if (Filter.CurrentSelectedSealstoReceive != null)
                {
                    ArrayList selectedseals = (ArrayList)Filter.CurrentSelectedSealstoReceive;
                    if (selectedseals != null && selectedseals.Count > 0)
                    {
                        foreach (Guid index1 in selectedseals)
                        { s = s + index1 + ","; }
                    }
                }
                PhoenixInspectionSealRequisition.UpdateDamagedSeals(new Guid(ViewState["REQUESTLINEID"].ToString())
                                                        , General.GetNullableString(s.Length > 1 ? s : null));

                Filter.CurrentSelectedSealstoReceive = null;
                BindData();
                gvSealNumber.Rebind();
               // SetPageNavigator();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //                     "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', true);", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                     "BookMarkScript", "fnReloadList('codehelp1', null, true);", true);
            }
            else if (CommandName.ToUpper().Equals("NOTRECEIVED"))
            {
                string s = ",";
                if (Filter.CurrentSelectedSealstoReceive != null)
                {
                    ArrayList selectedseals = (ArrayList)Filter.CurrentSelectedSealstoReceive;
                    if (selectedseals != null && selectedseals.Count > 0)
                    {
                        foreach (Guid index1 in selectedseals)
                        { s = s + index1 + ","; }
                    }
                }
                PhoenixInspectionSealRequisition.UpdateMissingSeals(new Guid(ViewState["REQUESTLINEID"].ToString())
                                                        , General.GetNullableString(s.Length > 1 ? s : null));

                Filter.CurrentSelectedSealstoReceive = null;
                BindData();
                gvSealNumber.Rebind();
                // SetPageNavigator();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //                     "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', true);", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                     "BookMarkScript", "fnReloadList('codehelp1', null, true);", true);
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
            string[] alColumns = { "FLDROWNO", "FLDSEALNO", "FLDRECEIPTSTATUSNAME" };
            string[] alCaptions = { "S.No", "Seal Number", "Receipt Status" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            DataSet ds = new DataSet();
            
            ds = PhoenixInspectionSealRequisition.SealNumberSearch((ViewState["REQUESTLINEID"] == null ? null : General.GetNullableGuid(ViewState["REQUESTLINEID"].ToString()))
                                , sortexpression, sortdirection
                                , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                , iRowCount, ref iRowCount, ref iTotalPageCount);   

            General.ShowExcel("Seal Numbers", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
            string[] alColumns = { "FLDROWNO", "FLDSEALNO", "FLDRECEIPTSTATUSNAME" };
            string[] alCaptions = { "S.No", "Seal Number", "Receipt Status" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds = new DataSet();

            ds = PhoenixInspectionSealRequisition.SealNumberSearch((ViewState["REQUESTLINEID"] == null ? null : General.GetNullableGuid(ViewState["REQUESTLINEID"].ToString()))
                                , sortexpression, sortdirection
                                , gvSealNumber.CurrentPageIndex+1
                                , gvSealNumber.PageSize, ref iRowCount, ref iTotalPageCount);    

            General.SetPrintOptions("gvSealNumber", "Seal Numbers", alCaptions, alColumns, ds);
            gvSealNumber.DataSource = ds;
            gvSealNumber.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void CheckAll(Object sender, EventArgs e)
    {
        string[] ctl = Request.Form.GetValues("__EVENTTARGET");

        if (ctl != null && ctl[0].ToString() == "gvSealNumber$ctl00$ctl02$ctl01$chkAllSeal")//"gvSealNumber$ctl01$chkAllSeal")
        {
            GridHeaderItem headerItem = gvSealNumber.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            RadCheckBox chkAll = (RadCheckBox)headerItem.FindControl("chkAllSeal");
            foreach (GridDataItem row in gvSealNumber.MasterTableView.Items)
            {
                RadCheckBox cbSelected = (RadCheckBox)row.FindControl("chkSelect");
                if (chkAll.Checked == true)
                {
                    cbSelected.Checked = true;
                }
                else
                {
                    cbSelected.Checked = false;
                }
            }
        }
    }

    protected void gvSealNumber_Sorting(object sender, GridViewSortEventArgs se)
    {
        try
        {
            ViewState["SORTEXPRESSION"] = se.SortExpression;

            if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
                ViewState["SORTDIRECTION"] = 1;
            else
                ViewState["SORTDIRECTION"] = 0;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealNumber_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
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
                //CheckBox chkAllSeal = (CheckBox)e.Row.FindControl("chkAllSeal");
                //if (ViewState["ISRECEIPTCONFIRMED"] != null && ViewState["ISRECEIPTCONFIRMED"].ToString() == "0")
                //{
                //    if (chkAllSeal != null) chkAllSeal.Enabled = false;
                //}
                //else
                //{
                //    if (chkAllSeal != null) chkAllSeal.Enabled = true;
                //}
            }
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
            //    if (ViewState["ISRECEIPTCONFIRMED"] != null && ViewState["ISRECEIPTCONFIRMED"].ToString() == "0")
            //    {
            //        if (chkSelect != null) chkSelect.Enabled = false;
            //    }
            //    else
            //    {
            //        if (chkSelect != null) chkSelect.Enabled = true;
            //    }
            //}
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
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


   

   



    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvSealNumber.Rebind();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedSeals = new ArrayList();
        Guid index = new Guid();
        foreach (GridDataItem gvrow in gvSealNumber.MasterTableView.Items)
        {
            bool result = false;
            index = new Guid(gvSealNumber.MasterTableView.Items[0].GetDataKeyValue("FLDSEALID").ToString());

            if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
            }

            // Check in the Session
            if (Filter.CurrentSelectedSealstoReceive != null)
                SelectedSeals = (ArrayList)Filter.CurrentSelectedSealstoReceive;
            if (result)
            {
                if (!SelectedSeals.Contains(index))
                    SelectedSeals.Add(index);
            }
            else
                SelectedSeals.Remove(index);
        }
        if (SelectedSeals != null && SelectedSeals.Count > 0)
            Filter.CurrentSelectedSealstoReceive = SelectedSeals;
    }

    private void GetSelectedSeals()
    {
        if (Filter.CurrentSelectedSealstoReceive != null)
        {
            ArrayList SelectedSeals = (ArrayList)Filter.CurrentSelectedSealstoReceive;
            Guid index = new Guid();
            if (SelectedSeals != null && SelectedSeals.Count > 0)
            {
                foreach (GridDataItem gvrow in gvSealNumber.MasterTableView.Items)
                {
                    RadCheckBox chk = (RadCheckBox)(gvrow.Cells[0].FindControl("chkSelect"));
                    index = new Guid(gvSealNumber.MasterTableView.Items[0].GetDataKeyValue("FLDSEALID").ToString());
                    if (SelectedSeals.Contains(index))
                    {
                        RadCheckBox myCheckBox = (RadCheckBox)gvrow.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }
    }

    protected void gvSealNumber_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
}
