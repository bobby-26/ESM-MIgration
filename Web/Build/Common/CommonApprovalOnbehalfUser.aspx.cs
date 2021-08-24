using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;

public partial class CommonApprovalOnbehalfUser : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
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

    private void BindData()
    {
        string[] alColumns = { "FLDUSERNAME", "FLDDESIGNATION", "FLDEMAIL", "FLDONBEHALF1NAME", "FLDONBEHALF2NAME", "FLDSTATUSNAME" };
        string[] alCaptions = { "User Name", "Designation", "E-Mail", "On Behalf 1", "On Behalf 2", "Status" };

        DataTable dt = PhoenixCommonApproval.ListApprovalAdditionalOnbehalfUser(int.Parse(Request.QueryString["aid"]));

       
        gvApproval.DataSource = dt;

        //gvApproval.VirtualItemCount = ds.Tables[0].Rows.Count;

    }

    protected void Rebind()
    {
        gvApproval.SelectedIndexes.Clear();
        gvApproval.EditIndexes.Clear();
        gvApproval.DataSource = null;
        gvApproval.Rebind();
    }
    private bool IsValidApproval(string approvaltype, string username)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int resultInt;
        if (!int.TryParse(approvaltype, out resultInt))
            ucError.ErrorMessage = "Approval Configuration is required.";

        if (!int.TryParse(username, out resultInt))
            ucError.ErrorMessage = "User Name is required.";

        return (!ucError.IsError);
    }

    protected void gvApproval_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                string approvalid = Request.QueryString["aid"];
                string username = ((UserControlUserName)e.Item.FindControl("ddlUserAdd")).SelectedUser;

                if (!IsValidApproval(approvalid, username))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCommonApproval.InsertApprovalAdditionalOnbehalfUser(int.Parse(approvalid), int.Parse(username));
                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                string onbehalfid = (e.Item as GridDataItem).GetDataKeyValue("FLDONBEHALFID").ToString();
               // string onbehalfid = _grid.DataKeys[nCurrentRow].Value.ToString();
                string username = ((UserControlUserName)e.Item.FindControl("ddlUserEdit")).SelectedUser;

                if (!IsValidApproval("1", username))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixCommonApproval.UpdateApprovalAdditionalOnbehalfUser(new Guid(onbehalfid), int.Parse(Request.QueryString["aid"]), int.Parse(username));

                Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
               
                string onbehalfid = (e.Item as GridDataItem).GetDataKeyValue("FLDONBEHALFID").ToString();
                PhoenixCommonApproval.DeleteApprovalAdditionalOnbehalfUser(new Guid(onbehalfid));
              
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
            BindData();
        }
    }

    protected void gvApproval_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null)
                eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

        }
        if (e.Item.IsInEditMode)
        {
            UserControlUserName ucUserName = (UserControlUserName)e.Item.FindControl("ddlUserEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucUserName != null) ucUserName.SelectedUser = DataBinder.Eval(e.Item.DataItem, "FLDUSERCODE").ToString();
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton cmdAdd = (LinkButton)e.Item.FindControl("cmdAdd");
            if (cmdAdd != null)
                cmdAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdAdd.CommandName);
        }
    }

    protected void gvApproval_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvApproval.CurrentPageIndex + 1;
        BindData();
    }
}
