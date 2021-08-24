using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inventory;
using SouthNests.Phoenix.PlannedMaintenance;
using Telerik.Web.UI;
using SouthNests.Phoenix.Elog;

public partial class ElectronicLog_ElectronicLogItembyTank : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();

            toolbar.AddFontAwesomeButton("../Log/ElectronicLogItembyTank.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvElogItembyTank')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuComponentCounter.AccessRights = this.ViewState;
            MenuComponentCounter.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["COMPONENTID"] = Request.QueryString["COMPONENTID"];
                gvElogItembyTank.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME", "FLDCAPACITY", "FLDROB", "FLDROBDATE", "FLDINITIALYN" };
        string[] alCaptions = { "Location", "Capacity (m3)", "ROB (m3)", "ROB Date", "Initial Y/N" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        //DataSet ds = PhoenixElog.ListItembyTank(PhoenixSecurityContext.CurrentSecurityContext.UserCode
        //                                      , PhoenixSecurityContext.CurrentSecurityContext.VesselID
        //                                      , gvElogItembyTank.CurrentPageIndex + 1
        //                                      , gvElogItembyTank.PageSize
        //                                      , ref iRowCount
        //                                      , ref iTotalPageCount);

        //General.SetPrintOptions("gvElogItembyTank", "Item's by Tank", alCaptions, alColumns, ds);

        //if (ds.Tables[0].Rows.Count > 0)
        //{
        //    gvElogItembyTank.DataSource = ds;
        //    gvElogItembyTank.VirtualItemCount = iRowCount;
        //}
        //else
        //{
        //    DataTable dt = ds.Tables[0];
        //    gvElogItembyTank.DataSource = "";
        //}
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        //int iTotalPageCount = 0;

        string[] alColumns = { "FLDNAME","FLDCAPACITY", "FLDROB", "FLDROBDATE", "FLDINITIALYN" };
        string[] alCaptions = { "Location","Capacity (m3)", "ROB (m3)", "ROB Date", "Initial Y/N" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        //DataSet ds = PhoenixElog.ListItembyTank(PhoenixSecurityContext.CurrentSecurityContext.UserCode
        //                                      , PhoenixSecurityContext.CurrentSecurityContext.VesselID
        //                                      , gvElogItembyTank.CurrentPageIndex + 1
        //                                      , gvElogItembyTank.PageSize
        //                                      , ref iRowCount
        //                                      , ref iTotalPageCount);

        //General.ShowExcel("Item's by Tank", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }
    protected void ComponentCounter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                gvElogItembyTank.CurrentPageIndex = 0;
                BindData();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            if (CommandName.ToUpper().Equals("SAVE"))
            {

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
  
    protected void gvElogItembyTank_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                ViewState["ItembyTank"] = ((RadLabel)e.Item.FindControl("lblItembyTank")).Text;
                RadLabel initialYN = (RadLabel)e.Item.FindControl("lblInitialYN");
                ViewState["initialYN"] = initialYN.Text;
            }

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                //RadDropDownList ddlLoation = (RadDropDownList)e.Item.FindControl("ddlLocationAdd");
                //RadDropDownList ddlItem = (RadDropDownList)e.Item.FindControl("ddlItemAdd");
                //UserControlDecimal ROB = (UserControlDecimal)e.Item.FindControl("txtROBAdd");
                //UserControlDate ROBDate = (UserControlDate)e.Item.FindControl("txtROBDateAdd");
                //RadRadioButtonList InitialYN = (RadRadioButtonList)e.Item.FindControl("chkInitialYNAdd");

                //PhoenixElog.InsertItembyTank(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                //                            , new Guid(ddlLoation.SelectedItem.Value)
                //                            , ddlItem.SelectedItem.Value
                //                            , ddlItem.SelectedItem.Text
                //                            , ROB.Text
                //                            , Convert.ToDateTime(ROBDate.Text)
                //                            , Convert.ToBoolean(InitialYN.SelectedValue)
                //                            , PhoenixSecurityContext.CurrentSecurityContext.VesselID);
                //gvElogItembyTank.Rebind();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                {
                    //RadDropDownList ddlItem = (RadDropDownList)e.Item.FindControl("ddlItemEdit");
                    UserControlDecimal ROB = (UserControlDecimal)e.Item.FindControl("txtROBEdit");
                    UserControlDate ROBDate = (UserControlDate)e.Item.FindControl("txtROBDateEdit");
                    RadRadioButtonList InitialYN = (RadRadioButtonList)e.Item.FindControl("chkInitialYNEdit");
                    RadDropDownList ddlLoation = (RadDropDownList)e.Item.FindControl("ddlLocationEdit");
                    RadTextBox Capacity = (RadTextBox)e.Item.FindControl("txtCapacityEdit");
                    if (ROB.Text == null || ROB.Text == "")
                        ucError.ErrorMessage = "ROB is required";
                    if (ROB.Text == "0")
                        ucError.ErrorMessage = "Please enter the valid ROB";
                    if (ROBDate.Text == null || ROBDate.Text == "")
                        ucError.ErrorMessage = "ROB Date is required";

                    //PhoenixElog.EditItembyTank(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    //                        , ViewState["ItembyTank"].ToString()
                    //                        , null //ddlItem.SelectedItem.Value
                    //                        , null//ddlItem.SelectedItem.Text
                    //                        , ROB.Text == "" ? null : ROB.Text
                    //                        , General.GetNullableDateTime(ROBDate.Text)
                    //                        , Convert.ToBoolean(InitialYN.SelectedValue)
                    //                        , PhoenixSecurityContext.CurrentSecurityContext.VesselID
                    //                        , Capacity.Text == "" ? null : Capacity.Text
                    //                        );
                }
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                throw new MissingMethodException();
                //RadLabel ItemByTank = (RadLabel)e.Item.FindControl("lblItembyTank");
                //PhoenixElog.DeleteItembyTank(PhoenixSecurityContext.CurrentSecurityContext.UserCode, ItemByTank.Text);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvElogItembyTank_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindData();
    }
    protected void gvElogItembyTank_ItemDataBound(object sender, GridItemEventArgs e)
    {
        DataRowView drv = (DataRowView)e.Item.DataItem;

        if (e.Item is GridDataItem && !e.Item.IsInEditMode)
        {

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            RadLabel initialYN = (RadLabel)e.Item.FindControl("lblInitialYN");
            
            if (initialYN.Text != null && initialYN.Text != "")
            {
                initialYN.Text = Convert.ToInt16(initialYN.Text) == 1 ? "Yes" : "No";
            }

        }
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
        }

        if (e.Item is GridFooterItem)
        {
          
        }

        if ((e.Item is GridEditableItem && e.Item.IsInEditMode))
        {
            GridEditableItem item = e.Item as GridEditableItem;

            RadLabel initialYN = (RadLabel)e.Item.FindControl("lblInitialYN");
            RadRadioButtonList initialYNEdit = (RadRadioButtonList)e.Item.FindControl("chkInitialYNEdit");

            string ff = ViewState["initialYN"].ToString();

            if (ViewState["initialYN"].ToString() != null && ViewState["initialYN"].ToString() != "")
            {
                if (ViewState["initialYN"].ToString() == "Yes")
                {
                    initialYNEdit.Items[0].Selected = true;
                }
                else
                {
                    initialYNEdit.Items[1].Selected = true;
                }
            }
            else
            {
                initialYNEdit.Items[0].Selected = true;
            }

        }
    }
}