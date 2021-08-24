using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewCommon;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOperation;
using Telerik.Web.UI;
public partial class Crew_CrewWorkingGear : PhoenixBasePage
{
    string employeeid;
   
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        
        PhoenixToolbar toolbarmenu = new PhoenixToolbar();     
        MenuWorkingGear.AccessRights = this.ViewState;
        MenuWorkingGear.MenuList = toolbarmenu.Show();
        
        if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
        {
            employeeid = Request.QueryString["empid"];           
        }
        try
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewWorkingGear.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRegistersworkinggearitem')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            MenuRegistersWorkingGearItem.AccessRights = this.ViewState;
            MenuRegistersWorkingGearItem.MenuList = toolbar.Show();
            
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");            
                ViewState["empid"] = null;
                if (Request.QueryString["empid"] != null)
                {
                    ViewState["empid"] = Request.QueryString["empid"];
                    SetEmployeePrimaryDetails();
                }

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvRegistersworkinggearitem.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                
            }
        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void MenuWorkingGear_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewPersonal.EmployeeList(General.GetNullableInteger(Request.QueryString["empid"]));
            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
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
        string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDGEARTYPE", "FLDSIZENAME" };
        string[] alCaptions = { "Working Gear Item", "Working Gear Type", "Size" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixWorkingGearSize.EmployeeWorkingGearItemSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        Convert.ToInt32(ViewState["empid"] != null ? ViewState["empid"].ToString():employeeid)
                                        , sortexpression,
                                        sortdirection
                                        , Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                        iRowCount
                                        , ref iRowCount
                                        , ref iTotalPageCount);


        General.ShowExcel("Working Gear", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void RegistersWorkingGearItem_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
          
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

    protected void gvRegistersworkinggearitem_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRegistersworkinggearitem.CurrentPageIndex + 1;
            BindData();
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

        string[] alColumns = { "FLDWORKINGGEARITEMNAME", "FLDGEARTYPE", "FLDSIZENAME" };
        string[] alCaptions = { "Working Gear Item", "Working Gear Type", "Size" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixWorkingGearSize.EmployeeWorkingGearItemSearch(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                        Convert.ToInt32(ViewState["empid"] != null ? ViewState["empid"].ToString() : employeeid),
                                        sortexpression,
                                        sortdirection,
                                       int.Parse(ViewState["PAGENUMBER"].ToString()),
                                        gvRegistersworkinggearitem.PageSize,
                                        ref iRowCount,
                                        ref iTotalPageCount);

        General.SetPrintOptions("gvRegistersworkinggearitem", "Working Gear", alCaptions, alColumns, ds);
        
        gvRegistersworkinggearitem.DataSource = ds;
        gvRegistersworkinggearitem.VirtualItemCount = iRowCount;
        ViewState["ROWCOUNT"] = iRowCount;       
        //ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
   
    protected void gvRegistersworkinggearitem_RowEditing(object sender, GridViewEditEventArgs de)
    {
        BindData();    
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
   
    private bool IsInteger(string strVal)
    {
        try
        {
            int intout = Int32.Parse(strVal);
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvRegistersworkinggearitem.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvRegistersworkinggearitem_ItemCommand(object sender, GridCommandEventArgs e)
    {
       
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                string size = ((UserControls_UserControlWorkingGearSize)e.Item.FindControl("ucSizeEdit")).SelectedSize;
                string itemid = ((RadLabel)e.Item.FindControl("lblItemidEdit")).Text;

                if (!IsvalidSize(size))
                {
                    ucError.Visible = true;
                    return;
                }

                UpdateEmployeeWorkingGearSize(Convert.ToInt32(Request.QueryString["empid"]), itemid, size);

                Rebind();
            }
            else if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    private bool IsvalidSize(string size)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        if (size == "Dummy" || size=="")
        {
            ucError.ErrorMessage = "Size is required";
        }
        return (!ucError.IsError);
    }

    
    private void UpdateEmployeeWorkingGearSize(int employeeid, string itemid, string sizeid)
    {
        PhoenixWorkingGearSize.UpdateEmployeeWorkingGearSize(PhoenixSecurityContext.CurrentSecurityContext.UserCode, employeeid, new Guid(itemid), int.Parse(sizeid));
    }

    protected void Rebind()
    {
        gvRegistersworkinggearitem.SelectedIndexes.Clear();
        gvRegistersworkinggearitem.EditIndexes.Clear();
        gvRegistersworkinggearitem.DataSource = null;
        gvRegistersworkinggearitem.Rebind();
    }

    protected void gvRegistersworkinggearitem_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
        }

        if (e.Item.IsInEditMode)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            UserControls_UserControlWorkingGearSize ucSize = (UserControls_UserControlWorkingGearSize)e.Item.FindControl("ucSizeEdit");
            if (ucSize != null)
                ucSize.SelectedSize = drv["FLDSIZEID"].ToString();

        }

    }

    protected void gvRegistersworkinggearitem_UpdateCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvRegistersworkinggearitem_SortCommand(object sender, GridSortCommandEventArgs e)
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
