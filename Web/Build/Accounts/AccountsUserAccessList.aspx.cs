using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;


public partial class AccountsUserAccessList : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        cmdHiddenPick.Attributes.Add("style", "Display:None");

        SessionUtil.PageAccessRights(this.ViewState);
        lblEmail.Attributes.Add("style", "visibility:hidden;");
        txtUserCode.Attributes.Add("style", "visibility:hidden");
        txtusercodes.Attributes.Add("style", "visibility:hidden");

        if (!IsPostBack)
        {
            ImgAccountsUserIdPickList.Attributes.Add("onclick", "return showPickList('spnPickListMaker', 'codehelp1', '', '"+Session["sitepath"]+"/Common/CommonPickListUser.aspx', true); ");
            ViewState["PAGENUMBER"] = 1;
            ViewState["USER"] = "";
        }
            
    }

    private void BindData()
    {
        DataSet ds = PheonixAccountsUserAccessList.SearchCompany(
                                                         General.GetNullableInteger(txtUserCode.Text));
        //if (ds.Tables[0].Rows.Count > 0)
        //{
            gvUserAccessList.DataSource = ds;
           // gvUserAccessList.DataBind();
        //}
        //else
        //{
        //    DataTable dt = ds.Tables[0];
        //    ShowNoRecordsFound(dt, gvUserAccessList);
        //}
    }

  
    protected void gvUserAccessList_RowDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadRadioButtonList rblAccess = (RadRadioButtonList)e.Item.FindControl("rblAccess");
            DataSet ds = PhoenixRegistersHard.ListHardOrderByHardCode(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 251);
            if (rblAccess != null)
            {
                rblAccess.DataBindings.DataTextField = "FLDHARDNAME";
                rblAccess.DataBindings.DataValueField = "FLDHARDCODE";
                rblAccess.DataSource = ds;
                rblAccess.DataBind();

                if (General.GetNullableInteger(drv["FLDACCESS"].ToString()) != null)
                     // rblAccess.Items.FindByValue(drv["FLDACCESS"].ToString()).Selected = true;
                    rblAccess.SelectedValue = drv["FLDACCESS"].ToString();
            }

            
        }        
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        ViewState["USER"] = txtUserCode.Text.ToString();
        gvUserAccessList.DataSource = PheonixAccountsUserAccessList.SearchCompany(
                                                        General.GetNullableInteger(txtUserCode.Text));
        gvUserAccessList.DataBind();
    }
    protected void rblAccess_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            RadRadioButtonList rblAccess = (RadRadioButtonList)sender;

            GridDataItem gvrow = (GridDataItem)rblAccess.Parent.Parent;

            string companyId = ((RadLabel)gvrow.FindControl("lblID")).Text;
            string access = ((RadRadioButtonList)gvrow.FindControl("rblAccess")).SelectedValue;
            if (ViewState["USER"].ToString() == txtUserCode.Text.ToString())
            {
                PheonixAccountsUserAccessList.InsertAccountUserAccess(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    int.Parse(txtUserCode.Text),
                    Int32.Parse(companyId),
                    int.Parse(access));

                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvUserAccessList_Rowcommand(object sender, GridCommandEventArgs e)
    {
        //try
        //{
        //    GridView _gridview = (GridView)sender;
        //    int nCurrentrow = Int32.Parse(e.CommandArgument.ToString());
        //    if (e.CommandName.ToUpper().Equals("ADD"))
        //    {
        //        if (txtUserCode.Text != "")
        //        {

        //            PheonixAccountsUserAccessList.InsertAccountUserAccess(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
        //                int.Parse(txtUserCode.Text),
        //                 Int32.Parse(((Label)_gridview.Rows[nCurrentrow].FindControl("lblID")).Text),
        //                    int.Parse(((RadioButtonList)_gridview.Rows[nCurrentrow].FindControl("rblAccess")).SelectedValue));

        //            BindData();
        //        }
        //    }
        //}
        //catch (Exception ex)
        //{
        //    ucError.ErrorMessage = ex.Message;
        //    ucError.Visible = true;
        //}
    }


    protected void gvUserAccessList_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvUserAccessList.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
