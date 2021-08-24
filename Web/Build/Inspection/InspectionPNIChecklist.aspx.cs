using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class InspectionPNIChecklist : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    try
    //    {

    //        string lastDepartment = String.Empty;
    //        GridGroupPanel gridTable = (GridGroupPanel)gvPNIChecklist.Controls[0];
    //        foreach (GridDataItem gvr in gvPNIChecklist.Items)
    //        {

    //            Page.ClientScript.RegisterForEventValidation(gvPNIChecklist.UniqueID, "Edit$" + gvr.RowIndex.ToString());
    //            RadLabel hfDepartment = (RadLabel)gvr.FindControl("lblDepartmentid");

    //            string currDepartment = hfDepartment.Text;
    //            if (lastDepartment.CompareTo(currDepartment) != 0)
    //            {
    //                int rowIndex = gvr.ItemIndex;// gridTable.GroupPanelItems.[gvr];
    //                // Add new group header row
    //                GridViewRow headerRow = new GridViewRow(rowIndex, rowIndex,
    //                    DataControlRowType.DataRow, DataControlRowState.Normal);
    //                TableCell headerCell = new TableCell();
    //                headerCell.ColumnSpan = gvPNIChecklist.Columns.Count;
    //                headerCell.Text = string.Format("{0}:{1}", "Department",
    //                                                currDepartment);
    //                headerCell.CssClass = "DataGrid-HeaderStyle";

    //                headerCell.Font.Bold = true;

    //                headerRow.Cells.Add(headerCell);
    //                gridTable.Controls.AddAt(rowIndex, headerRow);
    //                // Update lastValue
    //                lastDepartment = currDepartment;
    //            }

    //        }
    //        base.Render(writer);
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionPNIChecklist.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPNIChecklist')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuPNIChecklist.MenuList = toolbar.Show();
           // MenuPNIChecklist.SetTrigger(pnlPNIChecklist);

            PhoenixToolbar toolbar1 = new PhoenixToolbar();

            toolbar1.AddButton("Medical Case", "MEDICALCASE");
            toolbar1.AddButton("Check List", "CHECKLIST");
            MenuPNICheckListMain.MenuList = toolbar1.Show();
            MenuPNICheckListMain.SelectedMenuIndex = 1;
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
               
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["PNICASEID"] = null;
                ViewState["DEPARTMENTID"] = null;

                if (Request.QueryString["PNIID"] != null && !string.IsNullOrEmpty(Request.QueryString["PNIID"].ToString()))
                    ViewState["PNICASEID"] = Request.QueryString["PNIID"].ToString();
                if (Request.QueryString["DEPARTMENTID"] != null && !string.IsNullOrEmpty(Request.QueryString["DEPARTMENTID"].ToString()))
                    ViewState["DEPARTMENTID"] = Request.QueryString["DEPARTMENTID"].ToString();
                
                if (!string.IsNullOrEmpty(Request.QueryString["REFNO"]))
                {
                    ucTitle.Text = "P&I Check List" + "(" + Request.QueryString["REFNO"] + ")";
                }

                gvPNIChecklist.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
          
            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    protected void MenuPNICheckListMain_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("MEDICALCASE"))
            {
                Response.Redirect("../Inspection/InspectionPNIOperation.aspx?PNIID=" + ViewState["PNICASEID"].ToString() , false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuPNIChecklist_TabStripCommand(object sender, EventArgs e)
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
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDQUESTION", "FLDSTATUSNAME", "FLDREMARKS" };
            string[] alCaptions = { "Checklist", "Status", "Remarks" };

            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

            DataTable dt = PhoenixInspectionPNI.PNIChecklistSearch(new Guid(ViewState["PNICASEID"].ToString()),
                    null,
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);

            General.ShowExcel("PNI Checklist", dt, alColumns, alCaptions, sortdirection, sortexpression);
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

            string[] alColumns = { "FLDQUESTION", "FLDSTATUSNAME", "FLDREMARKS" };
            string[] alCaptions = { "Checklist", "Status", "Remarks" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixInspectionPNI.PNIChecklistSearch(new Guid(ViewState["PNICASEID"].ToString()),
                    null,                   
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvPNIChecklist.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());

            General.SetPrintOptions("gvPNIChecklist", "PNI Checklist", alCaptions, alColumns, ds);
            gvPNIChecklist.DataSource = ds;
            gvPNIChecklist.VirtualItemCount = iRowCount;
          
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   
  
    private bool IsValidStatus(string status)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (status.Equals("") || status.Equals("Dummy"))
            ucError.ErrorMessage = "status should be selected.";

        return (!ucError.IsError);
    }
  
    private void BindValue(int rowindex)
    {
        try
        {
            ViewState["PNICASEID"] = ((Label)gvPNIChecklist.Items[rowindex].FindControl("lblPNICaseid")).Text;
            ViewState["DEPARTMENTID"] = ((Label)gvPNIChecklist.Items[rowindex].FindControl("lblDepartmentid")).Text;
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
            //ViewState["DIRECTOBSERVATIONID"] = null;
            BindData();
            //SetRowSelection();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void SetRowSelection()
    {
        
        for (int i = 0; i < gvPNIChecklist.Items.Count; i++)
        {
            if (gvPNIChecklist.MasterTableView.Items[i].GetDataKeyValue("FLDPNICHECKLISTID").ToString().Equals(ViewState["PNICHECKLISTID"].ToString()))
            {

                gvPNIChecklist.Items[i].Selected = true;
                ViewState["DTKEY"] = ((RadLabel)gvPNIChecklist.MasterTableView.Items[i].FindControl("lbldtkey")).Text; 

            }
        }
    }

    protected void gvPNIChecklist_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPNIChecklist.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvPNIChecklist_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if(e.CommandName.ToUpper()=="UPDATE")
        {
            try
            {
              
                UserControlHard ucStatus = ((UserControlHard)e.Item.FindControl("ucStatus"));
                string remarks = ((RadTextBox)e.Item.FindControl("txtRemarks")).Text;
                if (!IsValidStatus(ucStatus.SelectedHard))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoenixInspectionPNI.PNIChecklistUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    new Guid(((RadLabel)e.Item.FindControl("lblPNIChecklistid")).Text),
                    int.Parse(ucStatus.SelectedHard),
                    General.GetNullableString(remarks));

               
                gvPNIChecklist.Rebind();

            }
            catch (Exception ex)
            {
                ucError.ErrorMessage = ex.Message;
                ucError.Visible = true;
            }
        }

        else if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
    string lastDepartment = String.Empty;
    protected void gvPNIChecklist_ItemDataBound(object sender, GridItemEventArgs e)
    {

       
        if (e.Item is GridDataItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            UserControlHard ucStatus = (UserControlHard)e.Item.FindControl("ucStatus");
            if (ucStatus != null)
            {
                ucStatus.HardList = PhoenixRegistersHard.ListHard(1, 36, byte.Parse("0"), "YES,NO,NA");
                ucStatus.ShortNameFilter = "YES,NO,NA";
            }
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucStatus != null)
                ucStatus.SelectedHard = drv["FLDSTATUS"].ToString();
        }
        if (e.Item is GridDataItem)
        {
            RadLabel lblDtkey = (RadLabel)e.Item.FindControl("lblDtkey");
            ImageButton cmdAttachment = (ImageButton)e.Item.FindControl("cmdAttachment");
            if (cmdAttachment != null)
            {
                cmdAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','"+Session["sitepath"]+"/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text.ToString() + "&mod="
                                    + PhoenixModule.QUALITY + "');return true;");
                cmdAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdAttachment.CommandName);
            }
            ImageButton cmdNoAttachment = (ImageButton)e.Item.FindControl("cmdNoAttachment");
            if (cmdNoAttachment != null)
            {
                cmdNoAttachment.Attributes.Add("onclick", "parent.openNewWindow('codehelp','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDtkey.Text.ToString() + "&mod="
                                    + PhoenixModule.QUALITY + "');return true;");
                cmdNoAttachment.Visible = SessionUtil.CanAccess(this.ViewState, cmdNoAttachment.CommandName);
            }

            DataRowView drv = (DataRowView)e.Item.DataItem;
            ImageButton iab = (ImageButton)e.Item.FindControl("cmdAttachment");
            ImageButton inab = (ImageButton)e.Item.FindControl("cmdNoAttachment");
            if (iab != null) iab.Visible = true;
            if (inab != null) inab.Visible = false;
            int? n = General.GetNullableInteger(drv["FLDATTACHMENTCOUNT"].ToString());

            if (n == 0)
            {
                if (iab != null) iab.Visible = false;
                if (inab != null) inab.Visible = true;
            }

        }
        if (e.Item is GridDataItem)
        {
            RadLabel lbtn = (RadLabel)e.Item.FindControl("lblStatus");

            if (lbtn != null)
            {

                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipStatus");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
        }
       
    }

    protected void gvPNIChecklist_SortCommand(object sender, GridSortCommandEventArgs e)
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
