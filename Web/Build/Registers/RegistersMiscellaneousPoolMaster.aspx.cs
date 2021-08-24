using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
public partial class RegistersMiscellaneousPoolMaster : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Registers/RegistersMiscellaneousPoolMaster.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvPoolMaster')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        MenuRegistersPoolMaster.AccessRights = this.ViewState;
        MenuRegistersPoolMaster.MenuList = toolbar.Show();
     
        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;            
            gvPoolMaster.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDPOOLNAME", "FLDDESCRIPTION", "FLDPARENTPOOLNAME", "FLDCONTRACTCOMPANYSHORTCODE" };
        string[] alCaptions = { "Pool", "Description", "Parent Pool", "Contract Company" };
        string sortexpression;
        int sortdirection;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        else
            sortdirection = 0;

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixRegistersMiscellaneousPoolMaster.MiscellaneousPoolMasterSearch(null, "", null
            , sortexpression, sortdirection, 1, iRowCount, ref iRowCount, ref iTotalPageCount);

        if (ds.Tables.Count > 0)
            General.ShowExcel("Miscellaneous Pool Master", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void RegistersRegistersPoolMaster_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;


        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            gvPoolMaster.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDPOOLNAME", "FLDDESCRIPTION", "FLDPARENTPOOLNAME", "FLDCONTRACTCOMPANYSHORTCODE" };
            string[] alCaptions = { "Pool", "Description", "Parent Pool", "Contract Company" };

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int sortdirection;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            else
                sortdirection = 0;

            DataSet ds = PhoenixRegistersMiscellaneousPoolMaster.MiscellaneousPoolMasterSearch(null, "", null
                , sortexpression, sortdirection
                 , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                , gvPoolMaster.PageSize
                , ref iRowCount, ref iTotalPageCount);

            General.SetPrintOptions("gvPoolMaster", "Miscellaneous Pool Master", alCaptions, alColumns, ds);

            gvPoolMaster.DataSource = ds;
            gvPoolMaster.VirtualItemCount = iRowCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvPoolMaster_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvPoolMaster.CurrentPageIndex + 1;

        BindData();
    }

    protected void gvPoolMaster_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string directsignon = ((RadCheckBox)e.Item.FindControl("chkDirectSignonAdd")).Checked == true ? "1" : "0";
                string ServiceSync = ((RadCheckBox)e.Item.FindControl("chkServiceSyncAdd")).Checked == true ? "1" : "0";
                InsertPoolMaster(
                    ((RadTextBox)e.Item.FindControl("txtPoolNameAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtDescriptionAdd")).Text,
                     General.GetNullableInteger(((UserControlPool)e.Item.FindControl("ddlParentPoolAdd")).SelectedPool),
                     General.GetNullableInteger(((UserControlCompany)e.Item.FindControl("ddlCompanyAdd")).SelectedCompany)
                     , byte.Parse(directsignon)
                     , byte.Parse(ServiceSync)
                     );
                BindData();
                gvPoolMaster.Rebind();

            }
            if (e.CommandName == "Page")
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

    protected void gvPoolMaster_UpdateCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string name = ((RadTextBox)e.Item.FindControl("txtPoolNameEdit")).Text;
            string desc = ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text;
            string parentpool = ((UserControlPool)e.Item.FindControl("ddlParentPoolEdit")).SelectedPool;
            string contractcompany = ((UserControlCompany)e.Item.FindControl("ddlCompanyEdit")).SelectedCompany;
            string directsignon = ((RadCheckBox)e.Item.FindControl("chkDirectSignonEdit")).Checked == true ? "1" : "0";
            string ServiceSync = ((RadCheckBox)e.Item.FindControl("chkServiceSyncEdit")).Checked == true ? "1" : "0";

            if (!IsValidPool(name))
            {
                ucError.Visible = true;
                return;
            }
            UpdatePoolMaster(
               Int16.Parse(((RadLabel)e.Item.FindControl("lblPoolIdEdit")).Text),
               ((RadTextBox)e.Item.FindControl("txtPoolNameEdit")).Text,
               ((RadTextBox)e.Item.FindControl("txtDescriptionEdit")).Text,
               General.GetNullableInteger(parentpool),
               General.GetNullableInteger(contractcompany)
               , byte.Parse(directsignon)
               , byte.Parse(ServiceSync));

            BindData();
            gvPoolMaster.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }

    }

    protected void gvPoolMaster_ItemDataBound(object sender, GridItemEventArgs e)
    {
        GridEditableItem item = e.Item as GridEditableItem;

        LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
        if (del != null)
        {
            del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);
            del.Attributes.Add("onclick", "return fnConfirmDeleteTelerik(event); return false;");
        }

        LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
        if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
        if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

        LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
        if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);
   

        LinkButton map = (LinkButton)e.Item.FindControl("cmdMap");
        if (map != null)
        {
            map.Visible = SessionUtil.CanAccess(this.ViewState, map.CommandName);
            RadLabel poolid = (RadLabel)e.Item.FindControl("lblPoolId");
            map.Attributes.Add("onclick", "openNewWindow('codehelpactivity', 'Other Doc Mapping', '" + Session["sitepath"] + "/Registers/RegistersPoolOtherDocMapping.aspx?pool=" + poolid.Text + "&type=95');return true;");

        }

        UserControlPool ucPool = (UserControlPool)e.Item.FindControl("ddlParentPoolEdit");
        RadLabel ParentPool = (RadLabel)e.Item.FindControl("lblParentpool");
        if (ucPool != null) ucPool.SelectedPool = ParentPool.Text;

        UserControlCompany ucCompany = (UserControlCompany)e.Item.FindControl("ddlCompanyEdit");
        RadLabel CompanyId = (RadLabel)e.Item.FindControl("lblCompanyId");
        if (ucCompany != null) ucCompany.SelectedCompany = CompanyId.Text;

    }


    protected void gvPoolMaster_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem eeditedItem = e.Item as GridEditableItem;
        try
        {
            DeletePoolMaster(Int32.Parse(((RadLabel)eeditedItem.FindControl("lblPoolId")).Text));
            BindData();
            gvPoolMaster.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvPoolMaster_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        gvPoolMaster.Rebind();
    }

    private void InsertPoolMaster(string PoolName, string Description, int? ParentPool, int? contractcompany, byte? directsignon, byte? servicesync)
    {
        try
        {
            if (!IsValidPool(PoolName))
            {
                ucError.Visible = true;
                return;
            }
            PhoenixRegistersMiscellaneousPoolMaster.InsertMiscellaneousPoolMaster(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode, PoolName, Description, ParentPool, contractcompany, directsignon, servicesync);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdatePoolMaster(int Poolid, string PoolName, string Description, int? ParentPool, int? contractcompany, byte? directsignon, byte? servicesync)
    {
        if (!IsValidPool(PoolName))
        {
            ucError.Visible = true;
            return;
        }
        PhoenixRegistersMiscellaneousPoolMaster.UpdateMiscellaneousPoolMaster(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Poolid, PoolName, Description, ParentPool, contractcompany, directsignon, servicesync);

        ucStatus.Text = "Pool information updated";
    }

    private void DeletePoolMaster(int Poolid)
    {
        PhoenixRegistersMiscellaneousPoolMaster.DeleteMiscellaneousPoolMaster(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode, Poolid);
    }

    private bool IsValidPool(string PoolName)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (PoolName.Trim().Equals(""))
            ucError.ErrorMessage = "Pool Name is required.";

        return (!ucError.IsError);
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }


}
