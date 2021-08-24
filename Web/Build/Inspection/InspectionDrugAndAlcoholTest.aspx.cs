using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.Common;

public partial class InspectionDrugAndAlcoholTest : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDrugAndAlcoholTest.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDrugAndAlcoholTest')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDrugAndAlcoholTest.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionDrugAndAlcoholTest.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");         
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','Inspection/InspectionDrugAndAlcoholTestAdd.aspx')", "Add Drug And Alcohol Test", "<i class=\"fa fa-plus-circle\"></i>", "ADD");
            MenuDrugAndAlcoholTest.AccessRights = this.ViewState;
            MenuDrugAndAlcoholTest.MenuList = toolbar.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    ddlVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ddlVessel.Enabled = false;
                }
                else
                {
                    if (ViewState["VESSELID"] != null && ViewState["VESSELID"].ToString() != "")
                        ddlVessel.SelectedVessel = ViewState["VESSELID"].ToString();
                }

                gvDrugAndAlcoholTest.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDVESSELNAME", "FLDALCOHOLTESTDATE", "FLDPORTNAME", "FLDINSPECTERNAME", "FLDCOMPANYNAME", "FLDDRUGALCOHOLTEST" };
        string[] alCaptions = { "Vessel", "Test Date", "Port Name", "Inspecter Name", "Company Name", "Completed Y/N" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionDrugAndAlcoholTest.DrugAndAlcoholTestSearch(
                 General.GetNullableInteger(ddlVessel.SelectedVessel),
                sortexpression, sortdirection,
                1,
                gvDrugAndAlcoholTest.PageSize,
                ref iRowCount,
                ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("DrugAndAlcoholTest", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }


   protected void MenuDrugAndAlcoholTest_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID == 0)
                {
                    ddlVessel.SelectedVessel = "";
                }
               
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("ADD"))
            {
                String scriptpopup = String.Format("javascript:parent.openNewWindow('DrugAndAlcoholTest','','" + Session["sitepath"] + "/Inspection/InspectionDrugAndAlcoholTestAdd.aspx?'); return true;");

                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "script", scriptpopup, true);

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
        gvDrugAndAlcoholTest.SelectedIndexes.Clear();
        gvDrugAndAlcoholTest.EditIndexes.Clear();
        gvDrugAndAlcoholTest.DataSource = null;
        gvDrugAndAlcoholTest.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDVESSELNAME", "FLDALCOHOLTESTDATE", "FLDPORTNAME", "FLDINSPECTERNAME", "FLDCOMPANYNAME", "FLDDRUGALCOHOLTEST" };
        string[] alCaptions = { "Vessel", "Test Date", "Port Name", "Inspecter Name", "Company Name", "Completed Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixInspectionDrugAndAlcoholTest.DrugAndAlcoholTestSearch(
                    //General.GetNullableInteger(UcVessel.SelectedVessel) == null ? 0 : General.GetNullableInteger(UcVessel.SelectedVessel),
                    General.GetNullableInteger(ddlVessel.SelectedVessel),
                    sortexpression, sortdirection,
                    int.Parse(ViewState["PAGENUMBER"].ToString()),
                    gvDrugAndAlcoholTest.PageSize,
                    ref iRowCount,
                    ref iTotalPageCount);

        General.SetPrintOptions("gvDrugAndAlcoholTest", "Drug And Alcohol Test", alCaptions, alColumns, ds);

        gvDrugAndAlcoholTest.DataSource = ds;
        gvDrugAndAlcoholTest.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
    }
  
    protected void gvDrugAndAlcoholTest_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                string drugandalcoholId = (e.Item as GridDataItem).GetDataKeyValue("FLDDRUGALCOHOLTESTID").ToString();
                PhoenixInspectionDrugAndAlcoholTest.DeletDrugAlcoholTest(General.GetNullableGuid(drugandalcoholId).Value);
                Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvDrugAndAlcoholTest.Rebind();
    }

    protected void gvDrugAndAlcoholTest_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        RadLabel lblDrugAlcoholTestId = (RadLabel)e.Item.FindControl("lblDrugAlcoholTestId");
        RadLabel lblTestDate = (RadLabel)e.Item.FindControl("lblTestDate");
        RadLabel lblVesselid = (RadLabel)e.Item.FindControl("lblVesselid");

        DataRowView drv = (DataRowView)e.Item.DataItem;
        if (e.Item is GridDataItem)
        {

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);

                if (drv["FLDDRUGALCOHOLTESTYN"].ToString() == "1")
                {
                    db.Visible = false;
                }
                    
            }

            LinkButton db2 = (LinkButton)e.Item.FindControl("cmdShowCrewInCharge");
            if (db2 != null)
            {
                db2.Attributes.Add("onclick", "openNewWindow('DrugAlcoholTestCrewList', '','" + Session["sitepath"] + "/Inspection/InspectionDrugAndAlcoholTestCrewList.aspx?Date=" + lblTestDate.Text + "&DrugAlcoholTestId=" + lblDrugAlcoholTestId.Text + "&VesselId=" + lblVesselid.Text + "'); return false;");

            }
        }


        if (e.Item is GridEditableItem)
        {

            LinkButton db1 = (LinkButton)e.Item.FindControl("cmdEdit");
            if (db1 != null)
            {
                db1.Attributes.Add("onclick", "openNewWindow('DrugAlcoholTest', '','" + Session["sitepath"] + "/Inspection/InspectionDrugAndAlcoholTestAdd.aspx?DrugAlcoholTestId=" + lblDrugAlcoholTestId.Text + "'); return false;");

                if (drv["FLDDRUGALCOHOLTESTYN"].ToString() == "1")
                {
                    db1.Visible = false;
                }
            }


        }
    }
  
    protected void gvDrugAndAlcoholTest_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvDrugAndAlcoholTest.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvDrugAndAlcoholTest_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression.Replace(" ASC", "").Replace(" DESC", "");
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