using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceRotationLog : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {            
            SessionUtil.PageAccessRights(this.ViewState);
           
            if (!IsPostBack)
            {
                PhoenixToolbar toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("General", "GENERAL");
                toolbarmain.AddButton("Log", "LOG"); 
                MenuPMS.AccessRights = this.ViewState;
                MenuPMS.MenuList = toolbarmain.Show();
                MenuPMS.SelectedMenuIndex = 1;

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvRotation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

            PhoenixToolbar toolbargrid = new PhoenixToolbar();

            toolbargrid.AddFontAwesomeButton("../PlannedMaintenance/PlannedMaintenanceRotationLog.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvRotation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

            //toolbargrid.AddImageButton("../PlannedMaintenance/PlannedMaintenanceRotationLog.aspx", "Export to Excel", "icon_xls.png", "Excel");
            //toolbargrid.AddImageLink("javascript:CallPrint('gvRotation')", "Print Grid", "icon_print.png", "PRINT");
            MenuPMSRotation.AccessRights = this.ViewState;
            MenuPMSRotation.MenuList = toolbargrid.Show();

            //BindData();            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    
    protected void PMS_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                Response.Redirect("PlannedMaintenanceRotation.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void PMSRotation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                int iRowCount = 0;
                int iTotalPageCount = 0;

                string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
                int? sortdirection = 1;
                if (ViewState["SORTDIRECTION"] != null)
                    sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

                if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                    iRowCount = 10;
                else
                    iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

                string[] alColumns = { "FLDROTATEDNUMBER", "FLDROTATEDNAME", "FLDREPLACEDNUMBER", "FLDREPLACEDNAME", "FLDREPLACEDDATE" };
                string[] alCaptions = { "Rotated Component Number", "Rotated Component Name", "Replaced Component Number", "Replaced Component Name", "Replaced Date" };
              
                DataTable dt = PhoenixPlannedMaintenanceRotation.RotationSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                , sortexpression
                                                                                , sortdirection
                                                                                , gvRotation.CurrentPageIndex + 1
                                                                                , gvRotation.PageSize
                                                                                , ref iRowCount
                                                                                , ref iTotalPageCount);

                General.ShowExcel("Rotation Log", dt, alColumns, alCaptions, sortdirection, sortexpression);
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

            string[] alColumns = { "FLDROTATEDNUMBER", "FLDROTATEDNAME", "FLDREPLACEDNUMBER", "FLDREPLACEDNAME", "FLDREPLACEDDATE" };
            string[] alCaptions = { "Rotated Component Number", "Rotated Component Name", "Replaced Component Number", "Replaced Component Name", "Replaced Date" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 1;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataTable dt = PhoenixPlannedMaintenanceRotation.RotationSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                            , sortexpression
                                                                            , sortdirection
                                                                            , gvRotation.CurrentPageIndex + 1
                                                                            , gvRotation.PageSize
                                                                            , ref iRowCount
                                                                            , ref iTotalPageCount);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvRotation", "Rotation Log", alCaptions, alColumns, ds);
            if (dt.Rows.Count > 0)
            {
                gvRotation.DataSource = dt;
                gvRotation.VirtualItemCount = iRowCount;

            }
            else
            {
                gvRotation.DataSource = "";
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

    protected void gvRotation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvRotation_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        //{
        //    GridEditableItem item = e.Item as GridEditableItem;

        //    string disciplineCode = ((RadTextBox)e.Item.FindControl("txtDisciplineCodeEdit")).Text;
        //    string disciplineName = ((RadTextBox)e.Item.FindControl("txtDisciplineNameEdit")).Text;
        //}
    }
    protected void gvRotation_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }


}
