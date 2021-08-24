using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.CrewOffshore;
using System.Web.UI;

using Telerik.Web.UI;
public partial class CrewOffshore_CrewOffshoreDMRVoyageROBDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            {
                ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();                
            }
            else
            {

            }

            PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();
            toolbarvoyagetap.AddButton("List", "VOYAGE");
            toolbarvoyagetap.AddButton("Charter", "VOYAGEDATA");
            toolbarvoyagetap.AddButton("Commenced ROB", "CARGOROB");
            toolbarvoyagetap.AddButton("Completed ROB", "COMPLETCARGOROB");
            
           
            
           
           
          

            MenuVoyageTap.AccessRights = this.ViewState;
            MenuVoyageTap.MenuList = toolbarvoyagetap.Show();
            MenuVoyageTap.SelectedMenuIndex = 2;

            PhoenixToolbar toolbarvoyage = new PhoenixToolbar();
            toolbarvoyage.AddButton("Statement Of Facts Report", "REPORT", ToolBarDirection.Right);
            toolbarvoyage.AddButton("Save", "SAVE", ToolBarDirection.Right);
           
            MenuNewSaveTabStrip.AccessRights = this.ViewState;
            MenuNewSaveTabStrip.MenuList = toolbarvoyage.Show();

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CHARTERID"] = "";            
            BindDeckConditionData();

            if (Filter.CurrentVPRSVoyageSelection != null)
            {
                BindDataTradingArea();
            }
        }
       
        
    }

    protected void gvTradingArea_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            DataTable dt = new DataTable();

            dt = PhoenixCrewOffshoreDMRVoyageData.VoyageCharterCargoROBList(new Guid(Filter.CurrentVPRSVoyageSelection), int.Parse(ViewState["VESSELID"].ToString()), 0);

            gvTradingArea.DataSource = dt;
            gvTradingArea.DataBind();
                     

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;



         }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    //protected void gvTradingArea_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    int iCurrentRow = int.Parse(e.CommandArgument.ToString());
    //    if (e.CommandName.ToUpper().Equals("ADD"))
    //    {
    //        PhoenixCrewOffshoreDMRVoyageData.DMRCharterTradingInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                                                                 , General.GetNullableGuid(ViewState["CHARTERID"].ToString())
    //                                                                 , int.Parse(ViewState["VESSELID"].ToString())
    //                                                                 , int.Parse(((DropDownList)_gridView.FooterRow.FindControl("ddlTradingArea")).SelectedValue)
    //                                                                 , DateTime.Parse(((UserControlDate)_gridView.FooterRow.FindControl("txtEffectiveDate")).Text));
    //        BindDataTradingArea();
    //    }
    //    if (e.CommandName.ToUpper().Equals("DELETE"))
    //    {
    //        Guid? id = (Guid)_gridView.DataKeys[iCurrentRow].Value;
    //        PhoenixCrewOffshoreDMRVoyageData.DMRCharterTradingDelete(PhoenixSecurityContext.CurrentSecurityContext.UserCode, id);
    //    }
    //    if (e.CommandName.ToUpper().Equals("UPDATE"))
    //    {
    //        Guid? id = (Guid)_gridView.DataKeys[iCurrentRow].Value;
    //        PhoenixCrewOffshoreDMRVoyageData.DMRCharterTradingUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
    //                                                                 , id
    //                                                                 , General.GetNullableGuid(ViewState["CHARTERID"].ToString())
    //                                                                 , int.Parse(ViewState["VESSELID"].ToString())
    //                                                                 , int.Parse(((DropDownList)_gridView.Rows[iCurrentRow].FindControl("ddlTradingAreaEdit")).SelectedValue)
    //                                                                 , DateTime.Parse(((UserControlDate)_gridView.Rows[iCurrentRow].FindControl("txtEffectiveDateEdit")).Text));
    //    }

    //}

    //protected void gvTradingArea_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = -1;
    //    _gridView.SelectedIndex = e.RowIndex;
    //    BindDataTradingArea();
    //}
    //protected void gvTradingArea_RowEditing(object sender, GridViewEditEventArgs de)
    //{

    //    GridView _gridView = (GridView)sender;
    //    _gridView.EditIndex = de.NewEditIndex;
    //    _gridView.SelectedIndex = de.NewEditIndex;

    //    BindDataTradingArea();
    //}
    //protected void gvTradingArea_ItemDataBound(Object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    //{

    //    GridView _gridView = (GridView)sender;

    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        ImageButton db = (ImageButton)e.Row.FindControl("cmdDelete");
    //        if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

    //        UserControlUnit ucunit = (UserControlUnit)e.Row.FindControl("ucUnitEdit");
    //        ucunit.UnitList = PhoenixRegistersUnit.ListDMRProductUnit();
    //        if (ucunit != null)
    //            ucunit.SelectedUnit = drv["FLDUNIT"].ToString();

    //        Literal lblOilType = (Literal)e.Row.FindControl("lblOilTypeName");
    //        if (lblOilType != null)
    //        {
    //            if (lblOilType.Text.ToString() == "Others")
    //            {
    //                TextBox txtOilType = (TextBox)e.Row.FindControl("txtOthersOilType");
    //                txtOilType.Visible = true;
    //            }
    //        }

    //    }
    //    if (e.Row.RowType == DataControlRowType.Footer)
    //    {
    //    }
    //}
    //protected void gvTradingArea_RowDeleting(object sender, GridViewDeleteEventArgs de)
    //{
    //    BindDataTradingArea();        
    //}
    //protected void gvTradingArea_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = e.RowIndex;
    //        _gridView.EditIndex = -1;
    //        BindDataTradingArea();            
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}
    //protected void PagerButtonClick(object sender, CommandEventArgs ce)
    //{
    //    gvTradingArea.SelectedIndex = -1;
    //    gvTradingArea.EditIndex = -1;
    //    if (ce.CommandName == "prev")
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] - 1;
    //    else
    //        ViewState["PAGENUMBER"] = (int)ViewState["PAGENUMBER"] + 1;

    //    BindDataTradingArea();        
    //}
   
    private void BindDataTradingArea()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataTable dt = new  DataTable();       

        dt = PhoenixCrewOffshoreDMRVoyageData.VoyageCharterCargoROBList(new Guid(Filter.CurrentVPRSVoyageSelection), int.Parse(ViewState["VESSELID"].ToString()), 0);

        gvTradingArea.DataSource = dt;
        gvTradingArea.DataBind();
       

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;        
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
    protected void VoyageNewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (Filter.CurrentVPRSVoyageSelection != null)
            {
                UpdateCargoROBData();
                ucStatus.Text = "Successfully updated";
                Response.Redirect("CrewOffshoreDMRVoyageROBDetails.aspx", false);
                BindDeckConditionData();
                
            }            
        }
        if (CommandName.ToUpper().Equals("REPORT"))
        {
            if (Filter.CurrentVPRSVoyageSelection != null)
            {
                String scriptpopup = String.Format(
                       "javascript:parent.openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/Reports/ReportsView.aspx?applicationcode=11&reportcode=STATEMENTOFFACTSREPORT&VoyageID=" + new Guid(Filter.CurrentVPRSVoyageSelection)
                       + "&VesselID=" + int.Parse(ViewState["VESSELID"].ToString()) + "&Flag=0&showmenu=0');");
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);           
                //Response.Redirect("../Reports/ReportsView.aspx?applicationcode=11&reportcode=STATEMENTOFFACTSREPORT&VoyageID=" + new Guid(Filter.CurrentVPRSVoyageSelection)
                //                                + "&VesselID=" + int.Parse(ViewState["VESSELID"].ToString()) + "&Flag=" + 0, false);
                
            }
            else
            {
                ucError.ErrorMessage = "Charter is not yet Created.";
                ucError.Visible = true;
            }
        }
    }    

    protected void VoyageTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("VOYAGE"))
        {
            Response.Redirect("CrewOffshoreDMRVoyage.aspx", false);
        }

        if (CommandName.ToUpper().Equals("VOYAGEDATA"))
        {
            Response.Redirect("CrewOffshoreDMRVoyageData.aspx", false);
        }

        if (CommandName.ToUpper().Equals("CARGOROB"))
        {

        }
        if (CommandName.ToUpper().Equals("COMPLETCARGOROB"))
        {
            Response.Redirect("CrewOffshoreDMRVoyageCompetedROB.aspx", false);
        }             
    }

    private bool IsValidVoyage(string comdate, string charterer, string commensedport, string completeddate, string nextcharterer, string completedport)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableInteger(ViewState["VESSELID"].ToString()) == null || General.GetNullableInteger(ViewState["VESSELID"].ToString()) == 0)
            ucError.ErrorMessage = "Vessel is required.";

        if (General.GetNullableInteger(charterer) == null)
            ucError.ErrorMessage = "Charterer is required.";

        if (General.GetNullableDateTime(comdate) == null)
            ucError.ErrorMessage = "Commenced Date is required.";
        if (General.GetNullableInteger(commensedport) == null)
            ucError.ErrorMessage = "Commenced Port is required.";       

        return (!ucError.IsError);
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
     
    }

    private void UpdateCargoROBData()
    {
       foreach (GridDataItem item in gvTradingArea.Items)
        {
           
                RadLabel OilTypeID = (RadLabel)item.FindControl("lblOilTypeCode");
                RadLabel OilTypeName = (RadLabel)item.FindControl("lblOilTypeName");
                string ucUnit = ((UserControlUnit)item.FindControl("ucUnitEdit")).SelectedUnit;
                UserControlMaskNumber txtROB = (UserControlMaskNumber)item.FindControl("ucROB");
                RadTextBox txtOilType = (RadTextBox)item.FindControl("txtOthersOilType");

                if (OilTypeID != null)
                {
                    PhoenixCrewOffshoreDMRVoyageData.UpdateCharterCargoROBUpdate(new Guid(Filter.CurrentVPRSVoyageSelection),
                        int.Parse(ViewState["VESSELID"].ToString()), new Guid(OilTypeID.Text.ToString()), 0, 
                        General.GetNullableDecimal(txtROB.Text), General.GetNullableInteger(ucUnit),General.GetNullableString(txtOilType.Text));                    
                }
           
        }
      PhoenixCrewOffshoreDMRVoyageData.UpdateCharterDeckConditionUpdate(new Guid(Filter.CurrentVPRSVoyageSelection), int.Parse(ViewState["VESSELID"].ToString()), txtDeckConditionRemarks.Text, 0,chkAtSea.Checked.Equals(true)?1:0,txtLocation.Text);
      }


    private void BindDeckConditionData()
    {
        if (Filter.CurrentVPRSVoyageSelection != null)
        {
            DataSet ds = PhoenixCrewOffshoreDMRVoyageData.EditVoyageData(General.GetNullableGuid(Filter.CurrentVPRSVoyageSelection));
            DataTable dt = ds.Tables[0];

            if (ds.Tables[0].Rows.Count > 0)
            {
                txtDeckConditionRemarks.Text = dt.Rows[0]["FLDCOMMENCEDECKCONDREMARKS"].ToString();
                chkAtSea.Checked = dt.Rows[0]["FLDCOMMENCEMENTATSEAYN"].ToString().Equals("1") ? true : false;
                txtLocation.Text = dt.Rows[0]["FLDCOMMENCEMENTLOCATION"].ToString();
            }
        }
    }



}