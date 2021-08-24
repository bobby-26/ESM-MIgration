using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;

public partial class PlannedMaintenanceRotation : PhoenixBasePage
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
                MenuMain.AccessRights = this.ViewState;
                MenuMain.MenuList = toolbarmain.Show();
                MenuMain.SelectedMenuIndex = 0;

                toolbarmain = new PhoenixToolbar();
                toolbarmain.AddButton("Rotate", "ROTATE",ToolBarDirection.Right);              
                MenuPMS.AccessRights = this.ViewState;
                MenuPMS.MenuList = toolbarmain.Show();                

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                ViewState["RPAGENUMBER"] = 1;
                ViewState["RSORTEXPRESSION"] = null;
                ViewState["RSORTDIRECTION"] = null;
                ViewState["RCURRENTINDEX"] = 1;

                ViewState["COMPNO"] = string.Empty;
                ViewState["COMPNAME"] = string.Empty;
                ViewState["ROCOMPNO"] = string.Empty;
                ViewState["ROCOMPNAME"] = string.Empty;

                gvRotation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                gvRotation1.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

                btnConfirm.Attributes.Add("style", "display:none");
                btnCancel.Attributes.Add("style", "display:none");

            }

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            MenuPMSRotation.AccessRights = this.ViewState;
            MenuPMSRotation.MenuList = toolbargrid.Show();

            toolbargrid = new PhoenixToolbar();
            MenuPMSRotation1.AccessRights = this.ViewState;
            MenuPMSRotation1.MenuList = toolbargrid.Show();

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
            if (CommandName.ToUpper().Equals("ROTATE"))
            {
                if (!IsValidRotate(General.GetNullableDateTime(txtrotationdate.Text)))
                {
                    ucError.Visible = true;
                    return;
                }
                RadWindowManager1.RadConfirm("Are you sure?", "confirm", 320, 150, null, "Confirm");
            }            
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRotate(DateTime? rotationdate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (gvRotation.SelectedItems.Count < 1)
        {
            ucError.ErrorMessage = "Select the Component to be Rotated";
        }
        if (gvRotation1.SelectedItems.Count < 1)
        {
            ucError.ErrorMessage = "Select the Replaced By Component";
        }
        if (rotationdate == null)
        {
            ucError.ErrorMessage = "Rotated Date is required";
        }

        return (!ucError.IsError);
    }
    protected void MenuMain_TabStripCommand(object sender, EventArgs e)
    {

        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("LOG"))
            {
                Response.Redirect("PlannedMaintenanceRotationLog.aspx", false);
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
            if (CommandName.ToUpper().Equals("FIND"))
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void PMSRotation1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                gvRotation.Rebind();
                gvRotation1.Rebind();
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

            string[] alColumns = { "FLDJOBCODE", "FLDJOBTITLE", "FLDCLASS" };
            string[] alCaptions = { "Job Code", "Job Title", "Job Class" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            DataTable dt = PhoenixPlannedMaintenanceRotation.RotationComponentSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                    , ViewState["COMPNO"].ToString()
                                                                                    , ViewState["COMPNAME"].ToString()
                                                                                    , 1
                                                                                    , sortexpression
                                                                                    , sortdirection
                                                                                    , gvRotation.CurrentPageIndex + 1
                                                                                    , gvRotation.PageSize
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount);


            gvRotation.DataSource = dt;
            gvRotation.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindDataRotation()
    {
        try
        {
            int iRowCount = 10;
            int iTotalPageCount = 10;

          
            string sortexpression = (ViewState["RSORTEXPRESSION"] == null) ? null : (ViewState["RSORTEXPRESSION"].ToString());
            int? sortdirection = null;
            if (ViewState["RSORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["RSORTDIRECTION"].ToString());
            DataTable dt = PhoenixPlannedMaintenanceRotation.RotationComponentSearch(PhoenixSecurityContext.CurrentSecurityContext.VesselID
                                                                                    , ViewState["ROCOMPNO"].ToString()
                                                                                    , ViewState["ROCOMPNAME"].ToString()
                                                                                    , 0
                                                                                    , sortexpression
                                                                                    , sortdirection
                                                                                    , gvRotation1.CurrentPageIndex + 1
                                                                                    , General.ShowRecords(null)
                                                                                    , ref iRowCount
                                                                                    , ref iTotalPageCount);


            gvRotation1.DataSource = dt;
            gvRotation1.VirtualItemCount = iRowCount;


            ViewState["RROWCOUNT"] = iRowCount;
            ViewState["RTOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRotation_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        GridView _gridView = sender as GridView;
        _gridView.SelectedIndex = se.NewSelectedIndex;      
        BindData();
    }
    protected void gvRotation1_SelectedIndexChanging(object sender, GridViewSelectEventArgs se)
    {
        GridView _gridView = sender as GridView;
        _gridView.SelectedIndex = se.NewSelectedIndex;      
        BindDataRotation();
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
       if(e.CommandName == RadGrid.FilterCommandName)
        {
            ViewState["COMPNO"] = gvRotation.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue.ToString();
            ViewState["COMPNAME"] = gvRotation.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue.ToString();

            gvRotation.Rebind();


        }
    }

    protected void gvRotation1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindDataRotation();
    }
    protected void gvRotation1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        //if (e.Item is GridEditableItem && e.Item.IsInEditMode)
        //{
        //    GridEditableItem item = e.Item as GridEditableItem;

        //    string disciplineCode = ((RadTextBox)e.Item.FindControl("txtDisciplineCodeEdit")).Text;
        //    string disciplineName = ((RadTextBox)e.Item.FindControl("txtDisciplineNameEdit")).Text;
        //}
    }
    protected void gvRotation1_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == RadGrid.FilterCommandName)
        {
            ViewState["ROCOMPNO"] = gvRotation1.MasterTableView.GetColumn("FLDCOMPONENTNUMBER").CurrentFilterValue.ToString();
            ViewState["ROCOMPNAME"] = gvRotation1.MasterTableView.GetColumn("FLDCOMPONENTNAME").CurrentFilterValue.ToString();

            gvRotation1.Rebind();


        }
    }


    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            string id = string.Empty;
            string id1 = string.Empty;
            foreach (GridDataItem item in gvRotation.SelectedItems)
            {
                id = item.GetDataKeyValue("FLDCOMPONENTID").ToString();
            }
            foreach (GridDataItem item in gvRotation1.SelectedItems)
            {
                id1 = item.GetDataKeyValue("FLDCOMPONENTID").ToString();
            }
            PhoenixPlannedMaintenanceRotation.RotationComponentUpdate(new Guid(id), new Guid(id1), General.GetNullableDateTime(txtrotationdate.Text));
            ucStatus.Text = "Components Rotated";
            gvRotation.Rebind();
            gvRotation1.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }
}
