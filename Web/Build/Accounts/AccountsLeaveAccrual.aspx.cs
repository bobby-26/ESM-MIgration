using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class AccountsLeaveAccrual : PhoenixBasePage
{    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvLVP.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvLVP.SelectedIndexes.Clear();
        gvLVP.EditIndexes.Clear();
        gvLVP.DataSource = null;
        gvLVP.Rebind();
    }
    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 0; //DEFAULT DESC SORT
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixAccountsLeave.SearchCrewLeaveWages(General.GetNullableInteger(ddlPrincipal.SelectedAddress),null,null,null,null
                                                        , sortexpression, sortdirection
                                                        , (int)ViewState["PAGENUMBER"], gvLVP.PageSize
                                                        , ref iRowCount, ref iTotalPageCount);
      

            gvLVP.DataSource = dt;
            gvLVP.VirtualItemCount = iRowCount;

            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   
    protected void gvLVP_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SAVE"))
            {
                Guid dtkey = new Guid(((RadLabel)e.Item.FindControl("lbldtkey")).Text);
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


    protected void gvLVP_ItemDataBound(Object sender, GridItemEventArgs e)
    {
      
        if (e.Item is GridDataItem)
        {
           
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");
            if (edit != null) edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            LinkButton save = (LinkButton)e.Item.FindControl("cmdSave");
            if (save != null) save.Visible = SessionUtil.CanAccess(this.ViewState, save.CommandName);

            LinkButton cancel = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cancel != null) cancel.Visible = SessionUtil.CanAccess(this.ViewState, cancel.CommandName);

        }      
    }
    protected void gvLVP_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvLVP.CurrentPageIndex + 1;
            BindData();
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidRankMapping(string description, string ranklist)
    {

        ucError.HeaderMessage = "Please provide the following required information";

        if (description.Equals(""))
            ucError.ErrorMessage = "Description is required.";
        if (ranklist.Equals(""))
            ucError.ErrorMessage = "Group rank is required.";             

        return (!ucError.IsError);
    }
  
   
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
}
