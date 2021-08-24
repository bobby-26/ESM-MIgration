using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class PlannedMaintenanceComponentList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceComponentList.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvComponent')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:return showPickList('spnPickListMaker', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListComponent.aspx?mode=custom', true);", "Add Component", "<i class=\"fas fa-plus\"></i>", "ADDPART");

            MenuRegistersComponent.AccessRights = this.ViewState;
            MenuRegistersComponent.MenuList = toolbargrid.Show();
            //MenuRegistersComponent.SetTrigger(pnlComponent);

            if (!IsPostBack)
            {
                ViewState["ISTREENODECLICK"] = false;
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["JOBID"] = null;
                ViewState["SETCURRENTNAVIGATIONTAB"] = null;

                if (Request.QueryString["JOBID"] != null)
                {
                    ViewState["JOBID"] = Request.QueryString["JOBID"].ToString();
                }
                gvComponent.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //BindData();
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
            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "COMPONENTCLASSNAME", "FLDFREQUENCYNAME", "FLDJOBLASTDONEDATE", "FLDJOBNEXTDUEDATE" };
            string[] alCaptions = { "Number", "Name", "Component Class", "Frequency", "Last Done Date", "Next Due Date" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());


            DataSet ds = PhoenixPlannedMaintenanceJob.JobComponentSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableGuid(ViewState["JOBID"].ToString()), sortexpression, sortdirection,
                       gvComponent.CurrentPageIndex + 1,
                       gvComponent.PageSize,
                       ref iRowCount,
                       ref iTotalPageCount);
            General.ShowExcel("Component", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);


        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuRegistersComponent_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
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

    private void BindData()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

            string[] alColumns = { "FLDCOMPONENTNUMBER", "FLDCOMPONENTNAME", "COMPONENTCLASSNAME", "FLDFREQUENCYNAME", "FLDJOBLASTDONEDATE", "FLDJOBNEXTDUEDATE" };
            string[] alCaptions = { "Number", "Name", "Component Class", "Frequency", "Last Done Date", "Next Due Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataSet ds = PhoenixPlannedMaintenanceJob.JobComponentSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID, General.GetNullableGuid(ViewState["JOBID"].ToString()), sortexpression, sortdirection,
                          gvComponent.CurrentPageIndex + 1,
                          gvComponent.PageSize,
                         ref iRowCount,
                         ref iTotalPageCount);

            General.SetPrintOptions("gvComponent", "Component", alCaptions, alColumns, ds);

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvComponent.DataSource = ds;
                gvComponent.VirtualItemCount = iRowCount;

                if (ViewState["COMPONENTID"] == null)
                {
                    ViewState["COMPONENTID"] = ds.Tables[0].Rows[0][0].ToString();
                    ViewState["DTKEY"] = ds.Tables[0].Rows[0]["FLDDTKEY"].ToString();
                }
            }
            else
            {
                DataTable dt = ds.Tables[0];
                gvComponent.DataSource = "";
            }

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    //protected void gvComponent_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    //{
    //    gvComponent.SelectedIndex = se.NewSelectedIndex;

    //    ViewState["COMPONENTID"] = ((Label)gvComponent.Rows[se.NewSelectedIndex].FindControl("lblComponentId")).Text;
    //    ViewState["DTKEY"] = ((Label)gvComponent.Rows[se.NewSelectedIndex].FindControl("lbldtkey")).Text;
    //}

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
            NameValueCollection nvc = Filter.CurrentPickListSelection;
            string stockitemid = nvc.Get("lblComponentId").ToString();

            PhoenixPlannedMaintenanceComponentJob.AsignComponentJob(new Guid(stockitemid),
            "," + ViewState["JOBID"].ToString() + ",", PhoenixSecurityContext.CurrentSecurityContext.VesselID);
            
            gvComponent.Rebind();
            //String script = String.Format("javascript:parent.fnReloadList('code1');");
            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvComponent_ItemCommand(object sender, GridCommandEventArgs e)
    {
        //if (e.CommandName == "UPDATE")
        //{
        //    try
        //    {
        //        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        //        RadGrid _gridView = (RadGrid)sender;
        //        int nCurrentRow = e.Item.RowIndex;

        //        RadLabel CounterID = (RadLabel)e.Item.FindControl("lblCounterID");
        //        UserControlDecimal CurrentValue = (UserControlDecimal)e.Item.FindControl("txtCurrentValueEdit");
        //        UserControlDate ReadingDate = (UserControlDate)e.Item.FindControl("txtReadingDateEdit");

        //        UpdateCounterUpdate(CounterID.Text, CurrentValue.Text, ReadingDate.Text);
        //        BindData();

        //    }
        //    catch (Exception ex)
        //    {
        //        ucError.ErrorMessage = ex.Message;
        //        ucError.Visible = true;
        //    }
        //}
        //if (e.CommandName == RadGrid.ExportToExcelCommandName)
        //{
        //    ShowExcel();
        //}
        //if (e.CommandName == RadGrid.RebindGridCommandName)
        //{
        //    RadGrid1.CurrentPageIndex = 0;
        //}
    }

    protected void gvComponent_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvComponent_ItemDataBound(object sender, GridItemEventArgs e)
    {

    }
}
