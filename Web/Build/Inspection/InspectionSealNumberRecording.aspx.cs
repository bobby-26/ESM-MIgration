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

public partial class InspectionSealNumberRecording : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                confirm.Attributes.Add("style", "display:none");
                ViewState["REQUESTLINEID"] = Request.QueryString["REQUESTLINEID"];
                ViewState["ISRECEIPTCONFIRMED"] = null;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                Filter.CurrentSelectedSealstoRecord = null;
                BindOfficeLocation();

                if (ViewState["REQUESTLINEID"] != null)
                    EditSealRequisitionLineItem(new Guid(ViewState["REQUESTLINEID"].ToString()));

                gvSealNumber.PageSize= int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
                toolbargrid.AddFontAwesomeButton("../Inspection/InspectionSealNumberRecording.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
                toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSealNumber')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
                MenuSealExport.AccessRights = this.ViewState;
                MenuSealExport.MenuList = toolbargrid.Show();
                SetMenu();
                //BindSealNumbers();
                     
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindOfficeLocation()
    {
        ddlOfficeLocation.DataSource = PhoenixInspectionSealRequisition.LocationTreeList(0);
        ddlOfficeLocation.DataTextField = "FLDLOCATIONNAME";
        ddlOfficeLocation.DataValueField = "FLDLOCATIONID";
        ddlOfficeLocation.DataBind();
    }

    private void BindSealNumbers()
    {
        //DataSet ds = PhoenixInspectionSealRequisition.AvailableSealList(General.GetNullableGuid(ViewState["REQUESTLINEID"].ToString()));
        //ddlFromSealNo.DataSource = ds;
        //ddlFromSealNo.DataTextField = "FLDSEALNO";
        //ddlFromSealNo.DataValueField = "FLDSEALID";
        //ddlFromSealNo.DataBind();
        //ddlFromSealNo.Items.Insert(0, new ListItem("--Select--", "Dummy"));

        //ddlToSealNo.DataSource = ds;
        //ddlToSealNo.DataTextField = "FLDSEALNO";
        //ddlToSealNo.DataValueField = "FLDSEALID";
        //ddlToSealNo.DataBind();
        //ddlToSealNo.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

    private void SetMenu()
    {        
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Issue", "SAVE",ToolBarDirection.Right);
        //toolbar.AddButton("Return to Inventory", "REMOVE");
        MenuSealNumber.AccessRights = this.ViewState;
        MenuSealNumber.MenuList = toolbar.Show();        
    }

    private void EditSealRequisitionLineItem(Guid gRequestlineId)
    {
        try
        {
            DataTable dt = PhoenixInspectionSealRequisition.EditSealRequesitionLineItem(gRequestlineId);
            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];                
                txtIssuedQty.Text = dr["FLDISSUEDQUANTITY"].ToString();
                txtReqQty.Text = dr["FLDREQUESTEDQTY"].ToString();
                ViewState["ISRECEIPTCONFIRMED"] = dr["FLDACTIVEYN"].ToString();
                ViewState["SEALTYPE"] = dr["FLDSEALTYPE"].ToString();
                ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();
                setROB();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void setROB()
    {
        int resultcount = 0;
        int totalpagecount = 0;        
        DataSet ds = PhoenixInspectionSealInventory.SealOfficeInventorySearch(0,
                                    General.GetNullableInteger(ViewState["SEALTYPE"].ToString()), null,null,
                                    1, 20, ref resultcount, ref totalpagecount);
        double count = 0;
        if (ds.Tables[0].Rows.Count > 0)
        {
            for (int i=0; i < ds.Tables[0].Rows.Count; i++)
            {
                double qty = double.Parse(ds.Tables[0].Rows[i]["FLDQUANTITY"].ToString());                
                count = count + qty;
            }
            txtROB.Text = count.ToString();
        }
    }

    //protected void ddlFromSealNo_Changed(object sender, EventArgs e)
    //{
    //    SetSelectedSeals();
    //}

    //protected void ddlToSealNo_Changed(object sender, EventArgs e)
    //{
    //    SetSelectedSeals();
    //}

    protected void txtFromSealNo_Changed(object sender, EventArgs e)
    {
        SetSelectedSeals();
        txtToSealNo.Focus();
    }

    protected void txtToSealNo_Changed(object sender, EventArgs e)
    {
        SetSelectedSeals();
    }
    private void SetSelectedSeals()
    {
        int result = 0;
        //if (General.GetNullableString(ddlFromSealNo.SelectedValue) != null && General.GetNullableString(ddlToSealNo.SelectedValue) != null)
        //{
        //    PhoenixInspectionSealRequisition.GetSelectedSeals(ddlFromSealNo.SelectedItem.Text, ddlToSealNo.SelectedItem.Text, ref result);
        //    txtSelectedSeals.Text = result.ToString();
        //}
        if (General.GetNullableString(txtFromSealNo.Text) != null && General.GetNullableString(txtToSealNo.Text) != null)
        {
            PhoenixInspectionSealRequisition.GetSelectedSeals(txtFromSealNo.Text, txtToSealNo.Text, ref result);
            txtSelectedSeals.Text = result.ToString();
        }
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
            if (CommandName.ToUpper().Equals("SAVE"))
            {
         
                RadWindowManager1.RadConfirm("Are you sure to issue the seals to vessel?", "confirm", 320, 150, null, "Confirm");

                return;
            }
            if (CommandName.ToUpper().Equals("REMOVE"))
            {
                string s = ",";
                if (Filter.CurrentSelectedSealstoRecord != null)
                {
                    ArrayList selectedseals = (ArrayList)Filter.CurrentSelectedSealstoRecord;
                    if (selectedseals != null && selectedseals.Count > 0)
                    {
                        foreach (Guid index in selectedseals)
                        { s = s + index + ","; }
                    }
                }
                //if (s.Length > 1 && General.GetNullableString(ddlFromSealNo.SelectedValue) != null && General.GetNullableString(ddlToSealNo.SelectedValue) != null)
                //{
                //    Filter.CurrentSelectedSealstoRecord = null;
                //    BindData();
                //    SetPageNavigator();
                //    BindSealNumbers();
                //    ddlFromSealNo.SelectedIndex = 0;
                //    ddlToSealNo.SelectedIndex = 0;
                //    ucError.ErrorMessage = "Do not select the seal(s) from the list when the range is specified.";
                //    ucError.Visible = true;
                //    return;
                //}
                if (s.Length > 1 && txtFromSealNo.Text != string.Empty && txtToSealNo.Text != string.Empty)
                {
                    Filter.CurrentSelectedSealstoRecord = null;
                    BindData();
                   
                    ucError.ErrorMessage = "Do not select the seal(s) from the list when the range is specified.";
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionSealRequisition.DeleteSealNumber(new Guid(ViewState["REQUESTLINEID"].ToString())
                                                        , General.GetNullableString(s.Length > 1 ? s : null)
                                                        , General.GetNullableString(txtFromSealNo.Text)
                                                        , General.GetNullableString(txtToSealNo.Text));
                                                        //, General.GetNullableString(ddlFromSealNo.SelectedValue) == null ? null : General.GetNullableString(ddlFromSealNo.SelectedItem.Text)
                                                        //, General.GetNullableString(ddlToSealNo.SelectedValue) == null ? null : General.GetNullableString(ddlToSealNo.SelectedItem.Text));
                Filter.CurrentSelectedSealstoRecord = null;
                EditSealRequisitionLineItem(new Guid(ViewState["REQUESTLINEID"].ToString()));
                BindData();
              
                //BindSealNumbers();
                //ddlFromSealNo.SelectedIndex = 0;
                //ddlToSealNo.SelectedIndex = 0;
                txtFromSealNo.Text = "";
                txtToSealNo.Text = "";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', true);", true);
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            //UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            //if (ucCM.confirmboxvalue == 1)
            //{
                string s = ",";
                if (Filter.CurrentSelectedSealstoRecord != null)
                {
                    ArrayList selectedseals = (ArrayList)Filter.CurrentSelectedSealstoRecord;
                    if (selectedseals != null && selectedseals.Count > 0)
                    {
                        foreach (Guid index in selectedseals)
                        { s = s + index + ","; }
                    }
                }
                //if (s.Length > 1 && General.GetNullableString(ddlFromSealNo.SelectedValue) != null && General.GetNullableString(ddlToSealNo.SelectedValue) != null)
                //{
                //    Filter.CurrentSelectedSealstoRecord = null;
                //    BindData();
                //    SetPageNavigator();
                //    BindSealNumbers();
                //    ddlFromSealNo.SelectedIndex = 0;
                //    ddlToSealNo.SelectedIndex = 0;
                //    txtSelectedSeals.Text = "";
                //    ucError.ErrorMessage = "Do not select the seal(s) from the list when the range is specified.";
                //    ucError.Visible = true;
                //    return;
                //}
                if (s.Length > 1 && txtFromSealNo.Text != string.Empty && txtToSealNo.Text != string.Empty)
                {
                    Filter.CurrentSelectedSealstoRecord = null;
                    BindData();

                    ucError.ErrorMessage = "Do not select the seal(s) from the list when the range is specified.";
                    ucError.Visible = true;
                    return;
                }
                PhoenixInspectionSealRequisition.InsertSealNumber(new Guid(ViewState["REQUESTLINEID"].ToString())
                                                        , General.GetNullableString(s.Length > 1 ? s : null)
                                                        , General.GetNullableString(txtFromSealNo.Text)
                                                        , General.GetNullableString(txtToSealNo.Text));
                                                        //, General.GetNullableString(ddlFromSealNo.SelectedValue) == null? null:General.GetNullableString(ddlFromSealNo.SelectedItem.Text)
                                                        //, General.GetNullableString(ddlToSealNo.SelectedValue) == null ? null:General.GetNullableString(ddlToSealNo.SelectedItem.Text));

                PhoenixInspectionSealRequisition.ConfirmSealIssue(new Guid(ViewState["REQUESTLINEID"].ToString()));

                Filter.CurrentSelectedSealstoRecord = null;
                EditSealRequisitionLineItem(new Guid(ViewState["REQUESTLINEID"].ToString()));
                BindData();
              
                //ddlFromSealNo.SelectedIndex = 0;
                //ddlToSealNo.SelectedIndex = 0;
                txtFromSealNo.Text = "";
                txtToSealNo.Text = "";
                txtSelectedSeals.Text = "";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                //"BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', true);", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                     "BookMarkScript", "fnReloadList('codehelp1', null, true);", true);
            gvSealNumber.Rebind();
            //}
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
            string[] alColumns = { "FLDROWNO", "FLDSEALNO", "FLDRECEIPTSTATUSNAME", "FLDLOCATIONNAME" };
            string[] alCaptions = { "S.No", "Seal Number", "Status", "Office Location" };
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
            
            ds = PhoenixInspectionSealRequisition.AvailableSealNumberSearch((ViewState["REQUESTLINEID"] == null ? null : General.GetNullableGuid(ViewState["REQUESTLINEID"].ToString()))
                            , General.GetNullableInteger((ddlOfficeLocation.SelectedValue == null || ddlOfficeLocation.SelectedValue == "0") ? null : ddlOfficeLocation.SelectedValue)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , General.ShowRecords(null), ref iRowCount, ref iTotalPageCount);
            
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
            string[] alColumns = { "FLDROWNO", "FLDSEALNO", "FLDRECEIPTSTATUSNAME", "FLDLOCATIONNAME" };
            string[] alCaptions = { "S.No", "Seal Number", "Status", "Office Location" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataSet ds = new DataSet();
            EditSealRequisitionLineItem(new Guid(ViewState["REQUESTLINEID"].ToString()));

            ds = PhoenixInspectionSealRequisition.AvailableSealNumberSearch((ViewState["REQUESTLINEID"] == null ? null : General.GetNullableGuid(ViewState["REQUESTLINEID"].ToString()))
                            , General.GetNullableInteger((ddlOfficeLocation.SelectedValue == null || ddlOfficeLocation.SelectedValue == "0") ? null : ddlOfficeLocation.SelectedValue)
                            , sortexpression, sortdirection
                            , Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                            , gvSealNumber.PageSize, ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvSealNumber", "Seal Numbers", alCaptions, alColumns, ds);
            gvSealNumber.DataSource = ds;
            gvSealNumber.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
            CheckBox chkAll = (CheckBox)headerItem.FindControl("chkAllSeal");//.HeaderRow.FindControl("chkAllSeal");
            foreach (GridDataItem row in gvSealNumber.MasterTableView.Items)
            {
                CheckBox cbSelected = (CheckBox)row.FindControl("chkSelect");
                if (chkAll.Checked == true)
                {
                    cbSelected.Checked = true;
                }
                else
                {
                    cbSelected.Checked = false;
                    Filter.CurrentSelectedSealstoRecord = null;
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
        foreach (GridDataItem gvrow in gvSealNumber.Items)
        {
            bool result = false;
            index = new Guid(gvrow.GetDataKeyValue("FLDSEALID").ToString());// gvSealNumber.MasterTableView.Items.DataKeys[gvrow.RowIndex].Value.ToString());

            if (((CheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
            {
                result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
            }

            // Check in the Session
            if (Filter.CurrentSelectedSealstoRecord != null)
                SelectedSeals = (ArrayList)Filter.CurrentSelectedSealstoRecord;
            if (result)
            {
                if (!SelectedSeals.Contains(index))
                    SelectedSeals.Add(index);
            }
            else
                SelectedSeals.Remove(index);
        }
        if (SelectedSeals != null && SelectedSeals.Count > 0)
            Filter.CurrentSelectedSealstoRecord = SelectedSeals;
    }

    private void GetSelectedSeals()
    {
        if (Filter.CurrentSelectedSealstoRecord != null)
        {
            ArrayList SelectedSeals = (ArrayList)Filter.CurrentSelectedSealstoRecord;
            Guid index = new Guid();
            if (SelectedSeals != null && SelectedSeals.Count > 0)
            {
                foreach (GridDataItem row in gvSealNumber.Items )
                {
                    CheckBox chk = (CheckBox)(row.Cells[0].FindControl("chkSelect"));
                    index = new Guid(row.GetDataKeyValue("FLDSEALID").ToString());
                    if (SelectedSeals.Contains(index))
                    {
                        CheckBox myCheckBox = (CheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }
    }

    protected void gvSealNumber_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSealNumber.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealNumber_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvSealNumber_ItemDataBound(object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvSealNumber_SortCommand(object sender, GridSortCommandEventArgs e)
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
}
