using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using System.Collections;
using Telerik.Web.UI;

public partial class InspectionSealIssue : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            confirm.Attributes.Add("style", "display:none");
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionSealIssue.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvSeal')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("../Inspection/InspectionSealIssue.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuSealExcel.AccessRights = this.ViewState;
            MenuSealExcel.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Return Seals", "RETURN", ToolBarDirection.Right);
            toolbar.AddButton("Issue Seals", "SAVE", ToolBarDirection.Right);

            MenuSealIssue.AccessRights = this.ViewState;
            MenuSealIssue.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "display:none");
            if (!IsPostBack)
            {                
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                Filter.CurrentSelectedSeals = null;
                ucSealStatus.bind();
                ucSealStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 197, "WMS");
                
                
                BindSealNumbers();
                gvSeal.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
            //SetPageNavigator();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindSealNumbers()
    {
        string status="";
        if(ddlType.SelectedValue == "1")
            status = PhoenixCommonRegisters.GetHardCode(1,197, "WMS");
        else if(ddlType.SelectedValue == "2")
            status = PhoenixCommonRegisters.GetHardCode(1,197,"ISS");
        ddlFromSealNo.Items.Clear();
        ddlToSealNo.Items.Clear();
       
        ddlFromSealNo.Items.Add(new DropDownListItem("--Select", "Dummy"));
        ddlToSealNo.Items.Add(new DropDownListItem("--Select", "Dummy"));
        DataSet ds = PhoenixInspectionSealIssue.SealList(PhoenixSecurityContext.CurrentSecurityContext.VesselID,int.Parse(status));
        ddlFromSealNo.DataSource = ds;
        ddlFromSealNo.DataTextField = "FLDSEALNO";
        ddlFromSealNo.DataValueField = "FLDSEALID";
        ddlFromSealNo.DataBind();
        //ddlFromSealNo.Items.Insert(0, new ListItem("--Select--", "Dummy"));

        ddlToSealNo.DataSource = ds;
        ddlToSealNo.DataTextField = "FLDSEALNO";
        ddlToSealNo.DataValueField = "FLDSEALID";
        ddlToSealNo.DataBind();
        //ddlToSealNo.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    }

  


  

    private void SetSelectedSeals()
    {
        string status = "";
        if (ddlType.SelectedValue == "1")
            status = PhoenixCommonRegisters.GetHardCode(1, 197, "WMS");
        else if (ddlType.SelectedValue == "2")
            status = PhoenixCommonRegisters.GetHardCode(1, 197, "ISS");

        int result = 0;
        if (General.GetNullableString(ddlFromSealNo.SelectedValue) != null && General.GetNullableString(ddlToSealNo.SelectedValue) != null)
        {
            PhoenixInspectionSealIssue.GetSelectedSeals(ddlFromSealNo.SelectedItem.Text, ddlToSealNo.SelectedItem.Text,int.Parse(status),ref result);
            txtSelectedSeals.Text = result.ToString();
        }
    }

    protected void ucSealType_Changed(object sender, EventArgs e)
    {
        int resultcount = 0;
        int totalpagecount = 0;
        if (General.GetNullableInteger(ucSealType.SelectedQuick) == null)
            txtROB.Text = "";
        else
        {
            DataSet ds = PhoenixInspectionSealInventory.SealROBSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID,
                                    General.GetNullableInteger(ucSealType.SelectedQuick), null,
                                    1, 1, ref resultcount, ref totalpagecount);
            if (ds.Tables[0].Rows.Count > 0)
                txtROB.Text = ds.Tables[0].Rows[0]["FLDROB"].ToString();
        }
    }

    private bool IsValidFilter()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableInteger(ucSealStatus.SelectedHard) == null)
        {
            ucError.ErrorMessage = "Seal Status is Required.";
        }
        return (!ucError.IsError);
    }

    protected void SealStatus_changed(object sender, EventArgs e)
    {
        Filter.CurrentSelectedSeals = null;
        BindData();
        gvSeal.Rebind();
        //SetPageNavigator();
    }

    protected void MenuSealExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("FIND"))
            {
                if (!IsValidFilter())
                {
                    ucError.Visible = true;
                    return;
                }
                ViewState["PAGENUMBER"] = 1;
                //BindData();
                gvSeal.Rebind();
                //SetPageNavigator();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuSealIssue_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (CommandName.ToUpper().Equals("SAVE"))
            {
                ViewState["type"] = "issue";
               
                RadWindowManager1.RadConfirm("Are you sure to issue the seals to CE?", "confirm", 320, 150, null, "Confirm");
                return;
            }
            else if (CommandName.ToUpper().Equals("RETURN"))
            {
                ViewState["type"] = "return";
              
                RadWindowManager1.RadConfirm("Are you sure to return the seals to Master?", "confirm", 320, 150, null, "Confirm");

                return;               
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
           // UserControlConfirmMessage ucCM = (UserControlConfirmMessage)sender;

            ArrayList SelectedSeals = new ArrayList();
            Guid index = new Guid();
            foreach (GridDataItem gvrow in gvSeal.MasterTableView.Items)
            {
                bool result = false;
                index = new Guid(gvrow.GetDataKeyValue("FLDSEALID").ToString());

                if (((RadCheckBox)(gvrow.FindControl("chkSelect"))).Checked == true)
                {
                    result = true;// ((CheckBox)gvrow.FindControl("chkSelect")).Checked;
                }

                // Check in the Session
                if (Filter.CurrentSelectedSeals != null)
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
                Filter.CurrentSelectedSeals = SelectedSeals;

           
                if (ViewState["type"].ToString() == "issue")
                {
                    if (ucSealStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 197, "ISS"))
                    {
                        ucError.ErrorMessage = "Seal(s) is/are already issued.";
                        ucError.Visible = true;
                        return;
                    }
                    string s = ",";
                    if (Filter.CurrentSelectedSeals != null)
                    {
                        ArrayList selectedseals = (ArrayList)Filter.CurrentSelectedSeals;
                        if (selectedseals != null && selectedseals.Count > 0)
                        {
                            foreach (Guid index1 in selectedseals)
                            { s = s + index1 + ","; }
                        }
                    }
                    if (s.Length > 1 && General.GetNullableString(ddlFromSealNo.SelectedValue) != null && General.GetNullableString(ddlToSealNo.SelectedValue) != null)
                    {
                        Filter.CurrentSelectedSeals = null;
                        BindData();
                        gvSeal.Rebind();
                       // SetPageNavigator();
                        BindSealNumbers();
                        //ddlFromSealNo.SelectedIndex = 0;
                        //ddlToSealNo.SelectedIndex = 0;
                        txtSelectedSeals.Text = "";
                        ucError.ErrorMessage = "Do not select the seal(s) from the list when the range is specified.";
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionSealIssue.SealIssueUpdate(General.GetNullableString(s.Length > 1 ? s : null)
                                                        , General.GetNullableString(ddlFromSealNo.SelectedValue) == null ? null : General.GetNullableString(ddlFromSealNo.SelectedItem.Text)
                                                        , General.GetNullableString(ddlToSealNo.SelectedValue) == null ? null : General.GetNullableString(ddlToSealNo.SelectedItem.Text));
                    Filter.CurrentSelectedSeals = null;
                    BindData();
                    gvSeal.Rebind();
                    BindSealNumbers();
                    //ddlFromSealNo.SelectedIndex = 0;
                    //ddlToSealNo.SelectedIndex = 0;
                    txtSelectedSeals.Text = "";
                    
                   // SetPageNavigator();
                    
                }
                else if (ViewState["type"].ToString() == "return")
                {
                    if (ucSealStatus.SelectedHard == PhoenixCommonRegisters.GetHardCode(1, 197, "WMS"))
                    {
                        ucError.ErrorMessage = "The seal(s) is/are not issued.";
                        ucError.Visible = true;
                        return;
                    }
                    string s = ",";
                    if (Filter.CurrentSelectedSeals != null)
                    {
                        ArrayList selectedseals = (ArrayList)Filter.CurrentSelectedSeals;
                        if (selectedseals != null && selectedseals.Count > 0)
                        {
                            foreach (Guid index1 in selectedseals)
                            { s = s + index1 + ","; }
                        }
                    }
                    if (s.Length > 1 && General.GetNullableString(ddlFromSealNo.SelectedValue) != null && General.GetNullableString(ddlToSealNo.SelectedValue) != null)
                    {
                        Filter.CurrentSelectedSeals = null;
                        BindData();
                        gvSeal.Rebind();
                       // SetPageNavigator();
                        BindSealNumbers();
                        //ddlFromSealNo.SelectedIndex = 0;
                        //ddlToSealNo.SelectedIndex = 0;
                        txtSelectedSeals.Text = "";
                        ucError.ErrorMessage = "Do not select the seal(s) from the list when the range is specified.";
                        ucError.Visible = true;
                        return;
                    }
                    PhoenixInspectionSealIssue.SealIssueReturn(General.GetNullableString(s.Length > 1 ? s : null)
                                                        , General.GetNullableString(ddlFromSealNo.SelectedValue) == null ? null : General.GetNullableString(ddlFromSealNo.SelectedItem.Text)
                                                        , General.GetNullableString(ddlToSealNo.SelectedValue) == null ? null : General.GetNullableString(ddlToSealNo.SelectedItem.Text));
                    Filter.CurrentSelectedSeals = null;
                    BindData();
                    gvSeal.Rebind();
                    //SetPageNavigator();
                    BindSealNumbers();
                    //ddlFromSealNo.SelectedIndex = 0;
                    //ddlToSealNo.SelectedIndex = 0;
                    txtSelectedSeals.Text = "";
                   
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
            string[] alColumns = { "FLDSEALNO", "FLDSEALTYPENAME", "FLDSTATUSNAME" };
            string[] alCaptions = { "Seal Number", "Seal Type", "Status" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataSet ds = PhoenixInspectionSealIssue.SealIssueSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                                        ,General.GetNullableInteger(ucSealType.SelectedQuick)                                                                        
                                                                        ,General.GetNullableString(txtSealNumber.Text)
                                                                        ,sortexpression, sortdirection
                                                                        ,Convert.ToInt16(ViewState["PAGENUMBER"].ToString())
                                                                        ,iRowCount, ref iRowCount, ref iTotalPageCount
                                                                        ,int.Parse(ucSealStatus.SelectedHard));
            
            General.ShowExcel("Seals", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
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
            string[] alColumns = { "FLDSEALNO", "FLDSEALTYPENAME", "FLDSTATUSNAME" };
            string[] alCaptions = { "Seal Number", "Seal Type", "Status" };
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixInspectionSealIssue.SealIssueSearch(General.GetNullableInteger(PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString())
                                                                        , General.GetNullableInteger(ucSealType.SelectedQuick)
                                                                        , General.GetNullableString(txtSealNumber.Text)
                                                                        , sortexpression, sortdirection
                                                                        , int.Parse(ViewState["PAGENUMBER"].ToString())
                                                                        , gvSeal.PageSize, ref iRowCount, ref iTotalPageCount
                                                                        , int.Parse(ucSealStatus.SelectedHard));

            General.SetPrintOptions("gvSeal", "Seals", alCaptions, alColumns, ds);
            gvSeal.DataSource = ds;
            gvSeal.VirtualItemCount = iRowCount;

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

  

   

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = 1;
            //BindData();
            gvSeal.Rebind();
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
           // BindData();
            gvSeal.Rebind();
          //  SetPageNavigator();
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

        if (ctl != null && ctl[0].ToString() == "gvSeal$ctl00$ctl02$ctl01$chkAllSeal")
        {
            GridHeaderItem headerItem = gvSeal.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
            RadCheckBox chkAll = (RadCheckBox)headerItem.FindControl("chkAllSeal");
            foreach (GridDataItem row in gvSeal.MasterTableView.Items)
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

    protected void SaveCheckedValues(Object sender, EventArgs e)
    {
        ArrayList SelectedSeals = new ArrayList();
        Guid index = new Guid();
        foreach (GridDataItem gvrow in gvSeal.MasterTableView.Items)
        {
            bool result = false;
            index = new Guid(gvSeal.MasterTableView.Items[0].GetDataKeyValue("FLDSEALID").ToString());

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
        if (Filter.CurrentSelectedSeals != null)
        {
            ArrayList SelectedSeals = (ArrayList)Filter.CurrentSelectedSeals;
            Guid index = new Guid();
            if (SelectedSeals != null && SelectedSeals.Count > 0)
            {
                foreach (GridDataItem row in gvSeal.MasterTableView.Items)
                {
                    RadCheckBox chk = (RadCheckBox)(row.Cells[0].FindControl("chkSelect"));
                    index = new Guid(gvSeal.MasterTableView.Items[0].GetDataKeyValue("FLDSEALID").ToString());
                    if (SelectedSeals.Contains(index))
                    {
                        RadCheckBox myCheckBox = (RadCheckBox)row.FindControl("chkSelect");
                        myCheckBox.Checked = true;
                    }
                }
            }
        }
    }

    protected void gvSeal_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvSeal.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ddlToSealNo_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        SetSelectedSeals();
    }

    protected void ddlFromSealNo_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        SetSelectedSeals();
    }

    protected void ddlType_SelectedIndexChanged(object sender, DropDownListEventArgs e)
    {
        txtSelectedSeals.Text = "";
        BindSealNumbers();
        if (ddlType.SelectedValue == "1")
            ucSealStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 197, "WMS");
        else if (ddlType.SelectedValue == "2")
            ucSealStatus.SelectedHard = PhoenixCommonRegisters.GetHardCode(1, 197, "ISS");
        BindData();
        gvSeal.Rebind();
    }

    protected void gvSeal_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvSeal_SortCommand(object sender, GridSortCommandEventArgs e)
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
